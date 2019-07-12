using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Importer.PaymentSchedule;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;


namespace Rac.VOne.Web.Models.Importers
{
    public class PaymentScheduleImporterBase :
        ImporterBase<PaymentSchedule>,
        IImporter<PaymentSchedule>
    {
        public ICsvParser CsvParser { get; set; } = new CsvParser();

        public Func<int, Task<ImporterSetting>> GetImporterSettingAsync { get; set; }
        public Func<int, Task<List<ImporterSettingDetail>>> GetImporterSettingDetailByIdAsync { get; set; }
        public Func<int, string, Task<string>> GetGeneralSettingValueAsync { get; set; }
        public Func<int, ScheduledPaymentImport[], ImporterSettingDetail[], Task<List<Billing>>> GetItemsForScheduledPaymentImportAsync { get; set; }
        public Func<int, int, int, ScheduledPaymentImport[], Task<ScheduledPaymentImportResult>> SaveInnerAsync { get; set; }
        public Func<int, string[], Task<List<Customer>>> GetCustomerByCodesAsync { get; set; }
        public Func<int, string[], Task<List<Department>>> GetDepartmentByCodesAsync { get; set; }
        public Func<int, string[], Task<List<AccountTitle>>> GetAccountTitleByCodesAsync { get; set; }
        public Func<int, int, string[], Task<List<Category>>> GetCategoriesByCodesAsync { get; set; }
        public Func<int, string[], Task<List<Currency>>> GetCurrenciesAsync { get; set; }
        public Func<ImportData, Task<ImportData>> SaveImportDataAsync { get; set; }
        public Func<Task<ImportData>> LoadImportDataAsync { get; set; }
        public Func<ScheduledPaymentImport, byte[]> SerializeScheduledPaymentImport { get; set; }
        public Func<PaymentSchedule, byte[]> SerializePaymentSchedule { get; set; }
        public Func<byte[], ScheduledPaymentImport> Deserialize { get; set; }
        public ImportData ImportData { get; set; }

        public PaymentScheduleImporterBase(ApplicationControl applicationControl)
            : base(applicationControl)
        {
        }
        /// <summary>処理対象請求データ true: 未消込のみ, false: 一部消込も対象</summary>
        public bool DoTargetNotMatchedData { get; set; }
        /// <summary>金額処理方法 true: 更新(書換), false : 加算</summary>
        public bool DoReplaceAmount { get; set; }
        /// <summary>同一得意先（債権代表者）請求データ true: 無視, false: 考慮する</summary>
        public bool DoIgnoreSameCustomerGroup { get; set; }

        private ImporterSettingDetail GetSettingDetail(Fields field,
            IEnumerable<ImporterSettingDetail> source = null)
            => (source ?? ImporterSettingDetail)?.FirstOrDefault(x => x.Sequence == (int)field);

        private List<ScheduledPaymentImport> schedulePaymentImport = new List<ScheduledPaymentImport>();
        private List<Customer> ExistCustomer { get; set; }
        private List<Category> ExistCategory { get; set; }
        private List<Currency> ExistCurrency { get; set; }
        private List<Department> ExistDepartment { get; set; }
        private List<AccountTitle> ExistAccountTitle { get; set; }
        private int errorCount { get; set; } = 0;
        private bool nullFlag { get; set; } = false;
        private bool existFlag { get; set; } = false;

        private DateTime? formatBilledAt(string value) => CreateFormatter(GetSettingDetail(Fields.BilledAt))(value);
        private DateTime? formatDueAt   (string value) => CreateFormatter(GetSettingDetail(Fields.DueAt))(value);
        private DateTime? formatSalesAt (string value) => CreateFormatter(GetSettingDetail(Fields.SalesAt))(value);
        private DateTime? formatClosingAt(string value) => CreateFormatter(GetSettingDetail(Fields.ClosingAt))(value);

        public async Task<bool> ReadCsvAsync()
        {
            var valid = false;
            PossibleData.Clear();
            ImpossibleData.Clear();
            schedulePaymentImport.Clear();
            ErrorReport.Clear();
            errorCount = 0;

            ImporterSetting = await GetImporterSettingAsync(ImporterSettingId);
            ImporterSettingDetail = await GetImporterSettingDetailByIdAsync(ImporterSettingId);

            RoundingType roundingType;
            var roundingModeValue = await GetGeneralSettingValueAsync(CompanyId, "取込時端数処理");
            Enum.TryParse(roundingModeValue, out roundingType);

            var possibleNo = 0;
            var impossibleNo = 0;
            var lineNo = 0;

            await LoadMasterDataAsync(ImporterSettingDetail);

            var records = CsvParser.Parse(FilePath).ToArray();
            int totalRecordCount = records.Length;

            if (totalRecordCount < ImporterSetting.StartLineCount)
            {
                return valid;
            }
            if (ImporterSetting.IgnoreLastLine == 1)
            {
                totalRecordCount -= 1;
            }

            for (var index = 0; index < records.Length; index++)
            {
                var values = records[index];
                var i = index + 1;
                if (i < ImporterSetting.StartLineCount) continue;

                var billing = GetImportInstance();
                var payment = new PaymentSchedule();
                nullFlag = false;
                existFlag = false;

                //外貨の小数桁を先に取得しておく
                int? decimalScale = null;
                if (ApplicationControl.UseForeignCurrency == 0)
                {
                    decimalScale = 0;
                    var currency = ExistCurrency?.Find(x => x.Code == DefaultCurrencyCode);
                    if (currency != null) billing.CurrencyId = currency.Id;
                }
                else
                {
                    foreach (var detail in ImporterSettingDetail)
                    {
                        int settingIndex = int.Parse(detail.FieldIndex.ToString());

                        if (detail.FieldIndex != 0 && detail.ImportDivision != 0 && detail.Sequence == (int)Fields.CurrencyCode)
                        {
                            if (detail.FieldIndex - 1 >= values.Length)
                            {
                                break;
                            }
                            else
                            {
                                CheckCurrencyCode(payment, values[detail.FieldIndex.Value - 1], i, billing, ref decimalScale, settingIndex);
                            }
                        }
                    }
                }

                foreach (var detail in ImporterSettingDetail.OrderBy(f => f.FieldIndex == 0).ThenBy(f => f.FieldIndex).ToList())
                {
                    if (detail.FieldIndex == 0) continue;

                    var settingIndex = detail.FieldIndex ?? 0;
                    var attribute = detail.AttributeDivision ?? 0;

                    #region 入金予定額検証処理
                    if (detail.Sequence == (int)Fields.ReceiptAmount)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.DueAmount = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckPaymentScheduleAmount(payment, values[detail.FieldIndex.Value - 1], i, billing, decimalScale, roundingType, settingIndex);
                        }
                    }
                    #endregion

                    #region 債権代表者コード検証処理
                    else if (detail.Sequence == (int)Fields.ParentCustomerCode && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckParentCustomerCode(payment, values[detail.FieldIndex.Value - 1], i, billing, settingIndex);
                        }
                    }
                    #endregion

                    #region 得意先コード検証処理
                    else if (detail.Sequence == (int)Fields.CustomerCode && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.CustomerCode = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckCustomerCode(payment, values[detail.FieldIndex.Value - 1], i, billing, settingIndex);
                        }
                    }
                    #endregion

                    #region 請求日検証処理
                    else if (detail.Sequence == (int)Fields.BilledAt && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.BilledAt = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckBilledAt(payment, values[detail.FieldIndex.Value - 1], i, billing, formatBilledAt, settingIndex);
                        }
                    }
                    #endregion

                    #region 請求金額検証処理
                    else if (detail.Sequence == (int)Fields.BillingAmount && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.BillingAmount = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckBillingAmount(payment, values[detail.FieldIndex.Value - 1], i, billing, decimalScale, roundingType, settingIndex);
                        }
                    }
                    #endregion

                    #region 入金予定日
                    else if (detail.Sequence == (int)Fields.DueAt && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.DueAt = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckDueAt(payment, values[detail.FieldIndex.Value - 1], i, billing, formatDueAt, settingIndex);
                        }
                    }
                    #endregion

                    #region 請求部門検証処理
                    else if (detail.Sequence == (int)Fields.DepartmentCode && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.DepartmentCode = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckDepartmentCode(payment, values[detail.FieldIndex.Value - 1], i, billing, settingIndex);
                        }
                    }
                    #endregion

                    #region 債権科目検証処理
                    else if (detail.Sequence == (int)Fields.DebitAccountTitleCode && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.DebitAccountTitleCode = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckAccountTitleCode(payment, values[detail.FieldIndex.Value - 1], i, billing, settingIndex);
                        }
                    }
                    #endregion

                    #region 売上日検証処理
                    else if (detail.Sequence == (int)Fields.SalesAt && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.SalesAt = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckSalesAt(payment, values[detail.FieldIndex.Value - 1], i, billing, formatSalesAt, settingIndex);
                        }
                    }
                    #endregion

                    #region 請求書番号検証処理
                    else if (detail.Sequence == (int)Fields.InvoiceCode && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.InvoiceCode = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckInvoiceCode(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 請求締日検証処理
                    else if (detail.Sequence == (int)Fields.ClosingAt && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.ClosingAt = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckClosingAt(payment, values[detail.FieldIndex.Value - 1], i, billing, formatClosingAt, settingIndex);
                        }
                    }
                    #endregion

                    #region 備考検証処理
                    else if (detail.Sequence == (int)Fields.Note1 && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.Note1 = "*";
                            ErrorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note1) ? "備考" : Note1, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckNote1(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 請求区分検証処理
                    else if (detail.Sequence == (int)Fields.BillingCategoryCode && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.BillingCategoryCode = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckBillingCategoryCode(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 備考2検証処理
                    else if (detail.Sequence == (int)Fields.Note2 && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.Note2 = "*";
                            ErrorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note2) ? "備考2" : Note2, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckNote2(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 備考3検証処理
                    else if (detail.Sequence == (int)Fields.Note3 && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.Note3 = "*";
                            ErrorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note3) ? "備考3" : Note3, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckNote3(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 備考4検証処理
                    else if (detail.Sequence == (int)Fields.Note4 && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.Note4 = "*";
                            ErrorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note4) ? "備考4" : Note4, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckNote4(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 備考5検証処理
                    else if (detail.Sequence == (int)Fields.Note5 && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.Note5 = "*";
                            ErrorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note5) ? "備考5" : Note5, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckNote5(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 備考6検証処理
                    else if (detail.Sequence == (int)Fields.Note6 && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.Note6 = "*";
                            ErrorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note6) ? "備考6" : Note6, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckNote6(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 備考7検証処理
                    else if (detail.Sequence == (int)Fields.Note7 && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.Note7 = "*";
                            ErrorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note7) ? "備考7" : Note7, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckNote7(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 備考8検証処理
                    else if (detail.Sequence == (int)Fields.Note8 && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.Note8 = "*";
                            ErrorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note8) ? "備考8" : Note8, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckNote8(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion

                    #region 通貨コード検証処理
                    else if (detail.Sequence == (int)Fields.CurrencyCode && detail.ImportDivision != 0)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            payment.CurrencyCode = "*";
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            if (nullFlag)
                            {
                                ErrorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                continue;
                            }
                            if (existFlag)
                            {
                                ErrorHandler(i, settingIndex, detail.FieldName, "master", null);
                                continue;
                            }
                            continue;
                        }
                    }
                    #endregion

                    #region 入金予定キー検証処理
                    else if (detail.Sequence == (int)Fields.ScheduledPaymentKey)
                    {
                        if (detail.FieldIndex - 1 >= values.Length)
                        {
                            ErrorHandler(i, settingIndex, detail.FieldName, "not", null);
                            errorCount++;
                            break;
                        }
                        else
                        {
                            CheckScheduledPaymentKey(payment, values[detail.FieldIndex.Value - 1], i, billing, attribute, settingIndex);
                        }
                    }
                    #endregion
                }
                billing.CompanyCode = CompanyCode;
                payment.CompanyCode = billing.CompanyCode;

                if (errorCount > 0)
                {
                    impossibleNo += 1;
                    ImpossibleData.Add(payment);
                }
                else
                {
                    possibleNo += 1;
                    PossibleData.Add(payment);

                    schedulePaymentImport.Add(billing);
                    billing.LineNo = i;
                }
                errorCount = 0;

                lineNo++;

            }

            if (schedulePaymentImport.Count > 0)
            {
                var billings = await GetItemsForScheduledPaymentImportAsync(CompanyId, schedulePaymentImport.ToArray(), ImporterSettingDetail.ToArray());

                var invalidIndices = new List<int>();
                for (var i = 0; i < billings.Count; i++) // todo: confirm max count -> schedulePaymentImport.Count
                {
                    errorCount = 0;
                    if (billings[i] == null)
                    {
                        errorCount++;
                        ErrorHandler(schedulePaymentImport[i].LineNo, 0, "", "取込キーに合致する請求データが存在しないためインポートできません。", null);
                    }
                    else if ((schedulePaymentImport[i].AssignmentAmount > 0 && billings[i].RemainAmount < 0)
                          || (schedulePaymentImport[i].AssignmentAmount < 0 && billings[i].RemainAmount > 0))
                    {
                        errorCount++;
                        ErrorHandler(schedulePaymentImport[i].LineNo, 0, "", "取込ファイルの入金予定額と符号が異なる請求残の請求データが存在するためインポートできません。", null);
                    }
                    if (errorCount == 0) continue;

                    impossibleNo += 1;
                    possibleNo -= 1;
                    ImpossibleData.Add(PossibleData[i]);
                    invalidIndices.Add(i);
                }
                for (var i = invalidIndices.Count - 1; i >= 0; i--)
                {
                    PossibleData.Remove(PossibleData[invalidIndices[i]]);
                }
            }

            ReadCount = lineNo;
            ValidCount = possibleNo;
            InvalidCount = impossibleNo;


            if (SaveImportDataAsync != null)
            {
                var data = new ImportData {
                    CompanyId   = CompanyId,
                    FileName    = "",
                    FileSize    = 0,
                    CreateBy    = LoginUserId,
                    //CreateAt    = DateTime.Now,
                };
                var details = new List<ImportDataDetail>();
                details.AddRange(schedulePaymentImport.Select(x => new ImportDataDetail {
                    ObjectType  = 0,
                    RecordItem  = SerializeScheduledPaymentImport(x),
                }).ToArray());
                details.AddRange(PossibleData.Select(x => new ImportDataDetail {
                    ObjectType  = 1,
                    RecordItem  = SerializePaymentSchedule(x),
                }).ToArray());
                details.AddRange(ImpossibleData.Select(x => new ImportDataDetail {
                    ObjectType  = 2,
                    RecordItem  = SerializePaymentSchedule(x),
                }).ToArray());
            }

            valid = true;
            return valid;
        }

        private void CheckPaymentScheduleAmount(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int? decimalScale, RoundingType roundingType, int fieldIndex)
        {
            decimal MAX_VALUE = 99999999999M;
            if (ValidateEmpty(field) == false)
            {
                errorCount++;
                payment.DueAmount = field + "*";
                ErrorHandler(lineNo, fieldIndex, "入金予定額", "empty", null);
                return;
            }

            decimal dec;
            if (!decimal.TryParse(field, out dec))
            {
                errorCount++;
                payment.DueAmount = field + "*";
                ErrorHandler(lineNo, fieldIndex, "入金予定額", "format", null);
                return;
            }

            var strValue = dec.ToString(); //前ゼロをとる
            string[] strDueAmount = null;
            if (strValue.Contains('.'))
            {
                strDueAmount = strValue.Split('.');
                if (decimal.Parse(strDueAmount[0]) < (MAX_VALUE * -1) || MAX_VALUE < decimal.Parse(strDueAmount[0]))
                {
                    errorCount++;
                    payment.DueAmount = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1] + "*";
                    ErrorHandler(lineNo, fieldIndex, "入金予定額", "入金予定額は最大11桁までです。", null);
                    return;
                }
            }
            else
            {
                strDueAmount = new string[] { strValue, "0" };
                if (decimal.Parse(strDueAmount[0]) < (MAX_VALUE * -1) || MAX_VALUE < decimal.Parse(strDueAmount[0]))
                {
                    errorCount++;
                    payment.DueAmount = dec.ToString("#,##0") + "*";
                    ErrorHandler(lineNo, fieldIndex, "入金予定額", "入金予定額は最大11桁までです。", null);
                    return;
                }
            }

            if (Convert.ToDecimal(strDueAmount[1]) != 0M)
            {

                if (roundingType == RoundingType.Error && ApplicationControl.UseForeignCurrency == 0)
                {
                    errorCount++;
                    payment.DueAmount = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1] + "*";
                    ErrorHandler(lineNo, fieldIndex, "入金予定額", "precision", null);
                    return;
                }
                else if (roundingType == RoundingType.Error
                     && ApplicationControl.UseForeignCurrency != 0
                     && strDueAmount[1].TrimEnd('0').Length > decimalScale)
                {
                    errorCount++;
                    payment.DueAmount = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1] + "*";
                    ErrorHandler(lineNo, fieldIndex, "入金予定額", "入金予定額は小数点" + decimalScale + "桁までです。", null);
                    return;
                }


                if (roundingType == RoundingType.Error && ApplicationControl.UseForeignCurrency != 0)
                {
                    billing.AssignmentAmount = dec;
                }
                else
                {
                    if (decimalScale == null) return;
                    billing.AssignmentAmount = roundingType.Calc(dec, decimalScale ?? 0).Value;
                }
                payment.DueAmount = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1];
                return;
            }

            billing.AssignmentAmount = dec;
            payment.DueAmount = dec.ToString("#,##0");
        }

        private void CheckParentCustomerCode(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int fieldindex)
        {
            billing.ParentCustomerCode = field;
            bool nullCheckErrorForParent = ValidateEmpty(billing.ParentCustomerCode);
            if (nullCheckErrorForParent == false)
            {
                errorCount++;
                ErrorHandler(lineNo, fieldindex, "債権代表者コード", "empty", null);
            }
            else
            {
                billing.ParentCustomerCode = EbDataHelper.ConvertToUpperCase(billing.ParentCustomerCode);

                if (ApplicationControl.CustomerCodeType == 0)
                {
                    billing.ParentCustomerCode = billing.ParentCustomerCode.PadLeft(ApplicationControl.CustomerCodeLength, '0');
                }

                //マスター存在チェックのため設定
                var customer = ExistCustomer?.Find(x => x.Code == billing.ParentCustomerCode);
                if (customer?.IsParent == 1)
                {
                    billing.ParentCustomerId = customer.Id;
                }
                else
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, "債権代表者コード", "master", null);
                }
            }
        }

        private void CheckCustomerCode(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int fieldindex)
        {
            billing.CustomerCode = field;
            payment.CustomerCode = field;
            bool nullCheckErrorForCustomerCode = ValidateEmpty(billing.CustomerCode);
            if (nullCheckErrorForCustomerCode == false)
            {
                errorCount++;
                payment.CustomerCode = field + "*";
                ErrorHandler(lineNo, fieldindex, "得意先コード", "empty", null);
            }
            else
            {
                billing.CustomerCode = EbDataHelper.ConvertToUpperCase(billing.CustomerCode);
                payment.CustomerCode = billing.CustomerCode;

                if (ApplicationControl.CustomerCodeType == 0)
                {
                    billing.CustomerCode = billing.CustomerCode.PadLeft(ApplicationControl.CustomerCodeLength, '0');
                    payment.CustomerCode = billing.CustomerCode;
                }
                //マスター存在チェックのため設定
                var customer = ExistCustomer?.Find(x => x.Code == payment.CustomerCode);
                if (customer != null)
                {
                    billing.CustomerId = customer.Id;
                }
                else
                {
                    errorCount++;
                    payment.CustomerCode = payment.CustomerCode + "*";
                    ErrorHandler(lineNo, fieldindex, "得意先コード", "master", null);
                }
            }
        }

        private void CheckBilledAt(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, Func<string, DateTime?> format, int fieldindex)
        {
            payment.BilledAt = field;

            bool nullCheckErrorForBilledAt = ValidateEmpty(payment.BilledAt);
            if (nullCheckErrorForBilledAt == false)
            {
                errorCount++;
                payment.BilledAt = field + "*";
                ErrorHandler(lineNo, fieldindex, "請求日", "empty", null);
            }
            else
            {
                DateTime? date = format(field);
                if (!date.HasValue)
                {
                    errorCount++;
                    payment.BilledAt = field + "*";
                    ErrorHandler(lineNo, fieldindex, "請求日", "format", null);
                }
                else
                {
                    billing.BilledAt = date.Value;
                    payment.BilledAt = billing.BilledAt.ToString();
                }
            }
        }

        private void CheckBillingAmount(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int? decimalScale, RoundingType roundingType, int fieldindex)
        {
            decimal MAX_VALUE = 99999999999M;
            if (ValidateEmpty(field) == false)
            {
                errorCount++;
                payment.BillingAmount = field + "*";
                ErrorHandler(lineNo, fieldindex, "請求金額", "empty", null);
                return;
            }

            decimal dec;
            if (!decimal.TryParse(field, out dec))
            {
                errorCount++;
                payment.BillingAmount = field + "*";
                ErrorHandler(lineNo, fieldindex, "請求金額", "format", null);
                return;
            }

            var strValue = dec.ToString();  //前ゼロをとる
            string[] strDueAmount = null;
            if (strValue.Contains('.'))
            {
                strDueAmount = strValue.Split('.');
                if (decimal.Parse(strDueAmount[0]) < (MAX_VALUE * -1) || MAX_VALUE < decimal.Parse(strDueAmount[0]))
                {
                    errorCount++;
                    payment.BillingAmount = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1] + "*";
                    ErrorHandler(lineNo, fieldindex, "請求金額", "請求金額は最大11桁までです。", null);
                    return;
                }
            }
            else
            {
                strDueAmount = new string[] { strValue, "0" };
                if (decimal.Parse(strDueAmount[0]) < (MAX_VALUE * -1) || MAX_VALUE < decimal.Parse(strDueAmount[0]))
                {
                    errorCount++;
                    payment.BillingAmount = dec.ToString("#,##0") + "*";
                    ErrorHandler(lineNo, fieldindex, "請求金額", "請求金額は最大11桁までです。", null);
                    return;
                }
            }

            if (Convert.ToDecimal(strDueAmount[1]) != 0M)
            {
                if (roundingType == RoundingType.Error && ApplicationControl.UseForeignCurrency == 0)
                {
                    errorCount++;
                    payment.BillingAmount = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1] + "*";
                    ErrorHandler(lineNo, fieldindex, "請求金額", "precision", null);
                    return;
                }
                else if (roundingType == RoundingType.Error
                     && ApplicationControl.UseForeignCurrency != 0
                     && strDueAmount[1].TrimEnd('0').Length > decimalScale)
                {
                    errorCount++;
                    payment.BillingAmount = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1] + "*";
                    ErrorHandler(lineNo, fieldindex, "請求金額", "請求金額は小数点" + decimalScale + "桁までです。", null);
                    return;
                }


                if (roundingType == RoundingType.Error && ApplicationControl.UseForeignCurrency != 0)
                {
                    billing.BillingAmount = dec;
                }
                else
                {
                    if (decimalScale == null) return;
                    billing.BillingAmount = roundingType.Calc(dec, decimalScale ?? 0).Value;
                }
                payment.BillingAmount = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1];
                return;
            }

            billing.BillingAmount = dec;
            payment.BillingAmount = dec.ToString("#,##0");
        }

        private void CheckDueAt(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, Func<string, DateTime?> format, int fieldindex)
        {
            payment.DueAt = field;

            bool nullCheckErrorForDueAt = ValidateEmpty(payment.DueAt);
            if (nullCheckErrorForDueAt == false)
            {
                errorCount++;
                payment.DueAt = field + "*";
                ErrorHandler(lineNo, fieldindex, "入金予定日", "empty", null);
            }
            else
            {
                DateTime? date = format(field);
                if (!date.HasValue)
                {
                    errorCount++;
                    payment.DueAt = field + "*";
                    ErrorHandler(lineNo, fieldindex, "入金予定日", "format", null);
                }
                else
                {
                    billing.DueAt = date.Value;
                    payment.DueAt = billing.DueAt.ToString();
                }
            }
        }

        private void CheckDepartmentCode(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int fieldindex)
        {
            billing.DepartmentCode = field;
            payment.DepartmentCode = field;

            bool nullCheckErrorForDepartmentCode = ValidateEmpty(billing.DepartmentCode.ToString());
            if (nullCheckErrorForDepartmentCode == false)
            {
                errorCount++;
                payment.DepartmentCode = field + "*";
                ErrorHandler(lineNo, fieldindex, "請求部門", "empty", null);
            }
            else
            {
                billing.DepartmentCode = EbDataHelper.ConvertToUpperCase(billing.DepartmentCode);
                payment.DepartmentCode = billing.DepartmentCode;

                if (ApplicationControl.DepartmentCodeType == 0)
                {
                    billing.DepartmentCode = billing.DepartmentCode.PadLeft(ApplicationControl.DepartmentCodeLength, '0');
                    payment.DepartmentCode = billing.DepartmentCode;
                }
                //マスター存在チェックのため設定
                var department = ExistDepartment?.Find(x => x.Code == payment.DepartmentCode);
                if (department != null)
                {
                    billing.DepartmentId = department.Id;
                }
                else
                {
                    errorCount++;
                    payment.DepartmentCode = field + "*";
                    ErrorHandler(lineNo, fieldindex, "請求部門", "master", null);
                }
            }
        }

        private void CheckAccountTitleCode(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int fieldindex)
        {
            billing.AccountTitleCode = field;
            payment.DebitAccountTitleCode = billing.AccountTitleCode;
            bool nullCheckErrorForAccountTitleCode = ValidateEmpty(billing.AccountTitleCode);
            if (nullCheckErrorForAccountTitleCode == false)
            {
                errorCount++;
                ErrorHandler(lineNo, fieldindex, "債権科目", "empty", null);
            }
            else
            {
                billing.AccountTitleCode = EbDataHelper.ConvertToUpperCase(billing.AccountTitleCode);
                payment.DebitAccountTitleCode = billing.AccountTitleCode;

                if (ApplicationControl.AccountTitleCodeType == 0)
                {
                    billing.AccountTitleCode = billing.AccountTitleCode.PadLeft(ApplicationControl.AccountTitleCodeLength, '0');
                    payment.DebitAccountTitleCode = billing.AccountTitleCode;
                }
                //マスター存在チェックのため設定
                var account = ExistAccountTitle?.Find(x => x.Code == payment.DebitAccountTitleCode);
                if (account != null)
                {
                    billing.DebitAccountTitleId = account.Id;
                }
                else
                {
                    errorCount++;
                    payment.DebitAccountTitleCode = payment.DebitAccountTitleCode + "*";
                    ErrorHandler(lineNo, fieldindex, "債権科目", "master", null);
                }
            }
        }

        private void CheckSalesAt(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, Func<string, DateTime?> format, int fieldindex)
        {
            payment.SalesAt = field;

            bool nullCheckErrorForSalesAt = ValidateEmpty(field);

            if (nullCheckErrorForSalesAt == false)
            {
                errorCount++;
                payment.SalesAt = field + "*";
                ErrorHandler(lineNo, fieldindex, "売上日", "empty", null);
            }
            else
            {
                DateTime? date = format(field);
                if (!date.HasValue)
                {
                    errorCount++;
                    payment.SalesAt = field + "*";
                    ErrorHandler(lineNo, fieldindex, "売上日", "format", null);
                }
                else
                {
                    billing.SalesAt = date.Value;
                    payment.SalesAt = billing.SalesAt.ToString();
                }
            }
        }

        private void CheckInvoiceCode(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.InvoiceCode = field;
            payment.InvoiceCode = field;
            bool checklengthInvoiceCode = ValidateLength(field, 20);

            if (checklengthInvoiceCode == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    payment.SalesAt = field + "*";
                    ErrorHandler(lineNo, fieldindex, "請求書番号", "length", 20);
                }
                else
                {
                    billing.InvoiceCode = billing.InvoiceCode.Substring(0, 20);
                    payment.InvoiceCode = billing.InvoiceCode;
                }
            }
        }

        private void CheckClosingAt(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, Func<string, DateTime?> format, int fieldindex)
        {
            payment.ClosingAt = field;

            bool nullCheckErrorForClosingAt = ValidateEmpty(field);
            if (nullCheckErrorForClosingAt == false)
            {
                errorCount++;
                payment.ClosingAt = field + "*";
                ErrorHandler(lineNo, fieldindex, "請求締日", "empty", null);
            }
            else
            {
                DateTime? date = format(field);
                if (!date.HasValue)
                {
                    errorCount++;
                    payment.ClosingAt = field + "*";
                    ErrorHandler(lineNo, fieldindex, "請求締日", "format", null);
                }
                else
                {
                    billing.ClosingAt = date.Value;
                    payment.ClosingAt = billing.ClosingAt.ToString();
                }
            }
        }

        private void CheckNote1(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.Note1 = field;
            payment.Note1 = billing.Note1;
            bool checklengthNote1 = ValidateLength(field, 100);

            if (checklengthNote1 == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    payment.Note1 = field + "*";
                    ErrorHandler(lineNo, fieldindex, string.IsNullOrWhiteSpace(Note1) ? "備考" : Note1, "length", 100);
                }
                else
                {
                    billing.Note1 = billing.Note1.Substring(0, 100);
                    payment.Note1 = billing.Note1;
                }
            }
        }

        private void CheckBillingCategoryCode(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.BillingCategoryCode = field;
            payment.BillingCategoryCode = field;

            bool nullCheckErrorForBillingCategoryCode = ValidateEmpty(billing.BillingCategoryCode);
            if (nullCheckErrorForBillingCategoryCode == false)
            {
                errorCount++;
                payment.BillingCategoryCode = field + "*";
                ErrorHandler(lineNo, fieldindex, "請求区分", "empty", null);
            }
            else
            {
                if (attribute == 1)
                    billing.BillingCategoryCode = billing.BillingCategoryCode.PadLeft(2, '0');

                payment.BillingCategoryCode = billing.BillingCategoryCode;
                Func<Category, string> keySelector = x => x.Code;
                if (attribute == 2)
                    keySelector = x => x.ExternalCode;
                //マスター存在チェックのため設定
                var category = ExistCategory?.Find(x => keySelector(x) == payment.BillingCategoryCode);

                if (category != null)
                {
                    billing.BillingCategoryId = category.Id;
                    if(attribute == 2)
                        payment.BillingCategoryCode = category.Code;
                }
                else
                {
                    errorCount++;
                    payment.BillingCategoryCode = payment.BillingCategoryCode + "*";
                    ErrorHandler(lineNo, fieldindex, "請求区分", "master", null);
                }
            }
        }

        private void CheckNote2(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.Note2 = field;
            bool checklengthNote2 = ValidateLength(field, 100);

            if (checklengthNote2 == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, string.IsNullOrWhiteSpace(Note2) ? "備考2" : Note2, "length", 100);
                }
                else
                {
                    billing.Note2 = billing.Note2.Substring(0, 100);
                }
            }
        }

        private void CheckNote3(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.Note3 = field;
            bool checklengthNote3 = ValidateLength(field, 100);

            if (checklengthNote3 == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, string.IsNullOrWhiteSpace(Note3) ? "備考3" : Note3, "length", 100);
                }
                else
                {
                    billing.Note3 = billing.Note3.Substring(0, 100);
                }
            }
        }

        private void CheckNote4(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.Note4 = field;
            bool checklengthNote4 = ValidateLength(field, 100);

            if (checklengthNote4 == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, string.IsNullOrWhiteSpace(Note4) ? "備考4" : Note4, "length", 100);
                }
                else
                {
                    billing.Note4 = billing.Note4.Substring(0, 100);
                }
            }
        }

        private void CheckNote5(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.Note5 = field;
            bool checklengthNote5 = ValidateLength(field, 100);

            if (checklengthNote5 == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, string.IsNullOrWhiteSpace(Note5) ? "備考5" : Note5, "length", 100);
                }
                else
                {
                    billing.Note5 = billing.Note5.Substring(0, 100);
                }
            }
        }

        private void CheckNote6(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.Note6 = field;
            bool checklengthNote6 = ValidateLength(field, 100);

            if (checklengthNote6 == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, string.IsNullOrWhiteSpace(Note6) ? "備考6" : Note6, "length", 100);
                }
                else
                {
                    billing.Note6 = billing.Note6.Substring(0, 100);
                }
            }
        }

        private void CheckNote7(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.Note7 = field;
            bool checklengthNote7 = ValidateLength(field, 100);

            if (checklengthNote7 == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, string.IsNullOrWhiteSpace(Note7) ? "備考7" : Note7, "length", 100);
                }
                else
                {
                    billing.Note7 = billing.Note7.Substring(0, 100);
                }
            }
        }

        private void CheckNote8(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.Note8 = field;
            bool checklengthNote8 = ValidateLength(field, 100);

            if (checklengthNote8 == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, string.IsNullOrWhiteSpace(Note8) ? "備考8" : Note8, "length", 100);
                }
                else
                {
                    billing.Note8 = billing.Note8.Substring(0, 100);
                }
            }
        }

        private void CheckCurrencyCode(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, ref int? decimalScale, int fieldindex)
        {
            billing.CurrencyCode = field;
            payment.CurrencyCode = field;

            bool nullCheckErrorForCurrencyCode = ValidateEmpty(billing.CurrencyCode);
            if (nullCheckErrorForCurrencyCode == false)
            {
                errorCount++;
                payment.CurrencyCode = "*";
                nullFlag = true;
                return;
            }

            billing.CurrencyCode = EbDataHelper.ConvertToUpperCase(billing.CurrencyCode);
            payment.CurrencyCode = billing.CurrencyCode;
            //マスター存在チェックのため設定
            var currency = ExistCurrency?.Find(x => x.Code == payment.CurrencyCode);

            if (currency != null)
            {
                billing.CurrencyId = currency.Id;
            }
            else
            {
                errorCount++;
                payment.CurrencyCode = payment.CurrencyCode + "*";
                existFlag = true;
                return;
            }

            decimalScale = currency.Precision;
        }

        private void CheckScheduledPaymentKey(PaymentSchedule payment, string field, int lineNo, ScheduledPaymentImport billing, int attribute, int fieldindex)
        {
            billing.ScheduledPaymentKey = field;
            bool checklengthSchedulePaymentKey = ValidateLength(field, 20);

            if (checklengthSchedulePaymentKey == false)
            {
                if (attribute == 1)
                {
                    errorCount++;
                    ErrorHandler(lineNo, fieldindex, "入金予定キー", "length", 20);
                }
                else
                {
                    billing.ScheduledPaymentKey = billing.ScheduledPaymentKey.Substring(0, 20);
                }
            }
        }


        private void ErrorHandler(int lineNo, int fieldIndex, string fieldName, string key, int? length)
        {
            string notExistMessage = "がありません。";
            string emptyMessage = "空白のためインポートできません。";
            string formatMessage = "フォーマットが異なるため、インポートできません。";
            string lengthMessage = "文字以上のためインポートできません。";
            string wrongLetterMessage = "不正な文字が入力されています。";
            string precisionMessage = "小数が含まれているためインポートできません。";
            string masterExistMessage = "存在しないため、インポートできません。";

            var error = new WorkingReport();
            error.LineNo = lineNo;
            error.FieldName = fieldName;
            error.FieldNo = fieldIndex;
            switch (key)
            {
                case "not": error.Message = fieldName + notExistMessage; break;
                case "empty": error.Message = emptyMessage; break;
                case "format": error.Message = formatMessage; break;
                case "length": error.Message = length + lengthMessage; break;
                case "wrong": error.Message = wrongLetterMessage; break;
                case "precision": error.Message = precisionMessage; break;
                case "master": error.Message = masterExistMessage; break;
                default: error.Message = key; break;
            };
            ErrorReport.Add(error);
        }

        public async Task<bool> ImportAsync()
        {
            var result = false;
            if (LoadImportDataAsync != null)
            {
                ImportData = await LoadImportDataAsync();
                schedulePaymentImport.AddRange(ImportData.Details
                    .Where(x => x.ObjectType == 0)
                    .Select(x => Deserialize(x.RecordItem)).ToArray());
            }

            // 読込・登録 時に変更したオプションを反映させいたいらしい
            foreach (var instance in schedulePaymentImport)
                SetImportInstance(instance);

            var details = await SaveInnerAsync(CompanyId, LoginUserId, ImporterSettingId, schedulePaymentImport.ToArray());

            if (details.ProcessResult.Result)
            {
                SaveCount = schedulePaymentImport.Count;
                SaveAmount = schedulePaymentImport.Sum(x => x.AssignmentAmount);
                result = true;
            }
            return result;
        }
        private ScheduledPaymentImport GetImportInstance()
        {
            var item = new ScheduledPaymentImport {
                CompanyId = CompanyId,
            };
            SetImportInstance(item);
            return item;
        }

        private void SetImportInstance(ScheduledPaymentImport instance)
        {
            instance.AssignmentFlag = DoTargetNotMatchedData    ? 1 : 0;
            instance.UpdateFlg      = DoReplaceAmount           ? 1 : 0;
            instance.CustomerFlg    = DoIgnoreSameCustomerGroup ? 1 : 0;
        }

        public bool WriteErrorLog(string path)
            => base.WriteErrorLog(path, "入金予定データ");

        public List<PaymentSchedule> GetReportSource(bool isPossible)
            => isPossible ? PossibleData : ImpossibleData;

        #region web service call

        private async Task LoadMasterDataAsync(IEnumerable<ImporterSettingDetail> details)
        {
            var tasks = new List<Task>();
            Func<ImporterSettingDetail, bool> isRequired = x
                => x.FieldIndex.HasValue || !string.IsNullOrEmpty(x.FixedValue);
            foreach (var field in new Fields[]
            {
                Fields.DepartmentCode,
                Fields.DebitAccountTitleCode,
                Fields.BillingCategoryCode,
            })
            {
                var setting = details.FirstOrDefault(x => x.Sequence == (int)field && isRequired(x));
                if (setting == null) continue;
                var isFixed = !string.IsNullOrEmpty(setting.FixedValue);
                var codes = isFixed ? new string[] { setting.FixedValue } : null;
                if (field == Fields.DepartmentCode) tasks.Add(LoadDepartmentAsync(codes));
                if (field == Fields.DebitAccountTitleCode) tasks.Add(LoadAccountTitleAsync(codes));
                if (field == Fields.BillingCategoryCode) tasks.Add(LoadBillingCategoryAsync(codes));
            }
            tasks.Add(LoadCurrencyAsync());
            if (details.Any(x
                => (x.Sequence == (int)Fields.ParentCustomerCode || x.Sequence == (int)Fields.CustomerCode) && isRequired(x)))
                tasks.Add(LoadCustomerAsync());
            await Task.WhenAll(tasks);

        }

        private async Task LoadCustomerAsync(string[] codes = null)
            => ExistCustomer = await GetCustomerByCodesAsync(CompanyId, codes);

        private async Task LoadDepartmentAsync(string[] codes = null)
            => ExistDepartment = await GetDepartmentByCodesAsync(CompanyId, codes);

        private async Task LoadAccountTitleAsync(string[] codes = null)
            => ExistAccountTitle = await GetAccountTitleByCodesAsync(CompanyId, codes);

        private async Task LoadBillingCategoryAsync(string[] codes = null)
            => ExistCategory = await GetCategoriesByCodesAsync(CompanyId, CategoryType.Billing, codes);

        private async Task LoadCurrencyAsync(string[] codes = null)
            => ExistCurrency = await GetCurrenciesAsync(CompanyId, codes);

        #endregion
    }
}
