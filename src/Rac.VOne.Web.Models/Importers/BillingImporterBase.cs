using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Importer.Billing;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models.Importers
{
    public class BillingImporterBase :
        ImporterBase<BillingImport>,
        IImporter<BillingImport>
    {
        public ICsvParser CsvParser { get; set; } = new CsvParser();

        public Func<int, Task<ImporterSetting>> GetImporterSettingAsync { get; set; }
        public Func<int, Task<List<ImporterSettingDetail>>> GetImporterSettingDetailAsync { get; set; }
        public Func<int, Task<List<Currency>>> GetCurrencyAsync { get; set; }
        public Func<int, Task<IEnumerable<string>>> GetJuridicalParsonalitiesAsync { get; set; }
        public Func<int, string, Task<string>> GetGeneralSettingValueAsync { get; set; }
        public Func<Task<List<TaxClass>>> GetTaxClassAsync { get; set; }
        public Func<IEnumerable<int>, Task<List<BillingDivisionContract>>> GetBillingDivisionContractByCustomerIdsAsync { get; set; }

        public Func<int, Task<List<HolidayCalendar>>> GetHolidayCalendarsAsync { get; set; }

        public Func<int, BillingImportDuplicationWithCode[], ImporterSettingDetail[], Task<int[]>> BillingImportDuplicationCheckAsync { get; set; }

        public Func<int, int, int, BillingImport[], Task<BillingImportResult>> ImportInnerAsync { get; set; }
        public Func<int, string[], Task<List<Customer>>> GetCustomerByCodesAsync { get; set; }
        public Func<int, string[], Task<List<Department>>> GetDepartmentByCodesAsync { get;set; }

        public Func<int, string[], Task<List<AccountTitle>>> GetAccountTitleByCodesAsync { get; set; }
        public Func<int, string[], Task<List<Staff>>> GetStaffByCodesAsync { get; set; }
        public Func<int, int, string[], Task<List<Category>>> GetCategoriesByCodesAsync { get; set; }
        public Func<int, Task<bool>> GetIsEnableToEditNoteAsync { get; set; }

        public Func<ImportData, Task<ImportData>> SaveImportDataAsync { get; set; }
        public Func<Task<ImportData>> LoadImportDataAsync { get; set; }
        public Func<BillingImport, byte[]> Serialize { get; set; }
        public Func<byte[], BillingImport> Deserialize { get; set; }
        public ImportData ImportData { get; set; }

        public BillingImporterBase(ApplicationControl applicationControl)
            : base(applicationControl) { }

        #region 処理実行後の状態確認用
        public int NewCustomerCreationCount { get; private set; }

        #endregion

        private ImporterSettingDetail GetSettingDetail(Fields field,
            IEnumerable<ImporterSettingDetail> source = null)
            => (source ?? ImporterSettingDetail)?.FirstOrDefault(x => x.Sequence == (int)field);

        private List<HolidayCalendar> Holidays { get; set; } = new List<HolidayCalendar>();

        private List<Customer> ExistCustomer { get; set; }
        private List<Department> ExistDepartment { get; set; }
        private List<Staff> ExistStaff { get; set; }
        private List<AccountTitle> ExistAccountTitle { get; set; }
        private List<Currency> ExistCurrencies { get; set; }
        private List<Category> ExistBillingCategory { get; set; }
        private List<Category> ExistCollectCategory { get; set; }
        private List<BillingDivisionContract> ExistBillingDivicionContract { get; set; }
        private List<TaxClass> taxClasses { get; set; }
        private IEnumerable<string> legalPersonalities { get; set; }
        private RoundingType roundingType { get; set; }
        private List<BillingImportDuplicationWithCode> BillingImportDuplication { get; set; }
            = new List<BillingImportDuplicationWithCode>();

        private bool UseControlInputNote { get; set; } = false;

        private DateTime? formatBilledAt(string value) => CreateFormatter(GetSettingDetail(Fields.BilledAt))(value);

        private DateTime? formatDueAt(string value) => CreateFormatter(GetSettingDetail(Fields.DueAt))(value);

        private DateTime? formatSalesAt(string value) => CreateFormatter(GetSettingDetail(Fields.SalesAt))(value);

        private DateTime? formatClosingAt(string value) => CreateFormatter(GetSettingDetail(Fields.ClosingAt))(value);

        public async Task<bool> ReadCsvAsync()
        {
            var valid = false;
            PossibleData.Clear();
            ImpossibleData.Clear();
            ErrorReport.Clear();
            BillingImportDuplication.Clear();
            try
            {
                var records = CsvParser.Parse(FilePath).ToList();

                if (records.Count < ImporterSetting.StartLineCount)
                {
                    return valid;
                }
                await LoadInitialDataAsync();
                var currencyFieldSetting = GetSettingDetail(Fields.CurrencyCode);

                #region エラーログ設定
                string notExistMessage = "がありません。";
                string emptyMessage = "空白のためインポートできません。";
                string formatMessage = "フォーマットが異なるため、インポートできません。";
                string lengthMessage = "桁以上のためインポートできません。";
                string lengthChar = "文字以上のためインポートできません。";
                string wrongLetterMessage = "不正な文字が入力されているためインポートできません。";
                string precisionMessage = "小数が含まれているためインポートできません。";
                string zeroMessage = "0 のため、インポートできません。";
                string notMatchSignTaxExcluded = "金額（抜）と消費税の符号が異なるため、インポートできません。";
                string compareAmountTaxExcluded = "金額（抜）が消費税よりも小さいため、インポートできません。";
                string notMatchSignTaxIncluded = "請求金額と消費税の符号が異なるため、インポートできません。";
                string compareAmountTaxIncluded = "請求金額が消費税よりも小さいため、インポートできません。";
                string emptyKanaNameMessage = "文字変換処理によって空白になったためインポートできません。";
                string valueOutOfBoundMessage = "消費税率に範囲外の値があります。";
                string lengthInvoiceChar = "請求書印字用制限文字数以上のためインポートできません。";

                Action<int, int, string, string, int?> errorHandler = (lineNo, fieldIndex, fieldName, key, length) =>
                {
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
                        case "lengthChar": error.Message = length + lengthChar; break;
                        case "wrong": error.Message = wrongLetterMessage; break;
                        case "precision": error.Message = precisionMessage; break;
                        case "zero": error.Message = zeroMessage; break;
                        case "notMatchSignTaxExcluded": error.Message = notMatchSignTaxExcluded; break;
                        case "compareAmountTaxExcluded": error.Message = compareAmountTaxExcluded; break;
                        case "notMatchSignTaxIncluded": error.Message = notMatchSignTaxIncluded; break;
                        case "compareAmountTaxIncluded": error.Message = compareAmountTaxIncluded; break;
                        case "emptyKanaName": error.Message = emptyKanaNameMessage; break;
                        case "valueOutOfBound": error.Message = valueOutOfBoundMessage; break;
                        case "lengthInvoiceChar": error.Message = lengthInvoiceChar; break;
                        default: error.Message = key; break;
                    };
                    ErrorReport.Add(error);
                };
                #endregion

                #region 金額表示フォーマット
                var currencyDisplayFormat = "###,###,###,##0";
                Action<int> setCurrencyDisplayFormat = (precision) =>
                {
                    if (precision > 0)
                    {
                        currencyDisplayFormat += ".";
                        for (int i = 0; i < precision; i++)
                        {
                            currencyDisplayFormat += "0";
                        }
                    }
                };
                #endregion

                #region CSV読んで検証処理実施
                var totalRecordCount = (ImporterSetting.IgnoreLastLine == 1) ? records.Count - 1 : records.Count;
                var skipDataCount = 0;
                var decimalScale = 0;

                if (UseForeignCurrency && currencyFieldSetting.ImportDivision == 0)
                {
                    decimalScale = (ExistCurrencies.FirstOrDefault(x => x.Code == currencyFieldSetting.FixedValue)?.Precision) ?? 0;
                    setCurrencyDisplayFormat(decimalScale);
                }

                foreach (var fields in records)
                {
                    var index = records.IndexOf(fields);
                    var i = index + 1;
                    if (ImporterSetting.IgnoreLastLine == 1
                        && i == records.Count) continue;
                    var values = fields.ToArray();
                    var billingImport = new BillingImport();
                    billingImport.CompanyId = CompanyId;
                    billingImport.LineNo = i;
                    billingImport.AutoCreationCustomerFlag = ImporterSetting.AutoCreationCustomer;
                    var billingImportDuplication = new BillingImportDuplicationWithCode();
                    billingImportDuplication.RowNumber = i;
                    var fieldIndex = 0;
                    var skipData = false;

                    if (i < ImporterSetting.StartLineCount) continue;

                    #region 通貨コード検証処理
                    if (UseForeignCurrency && currencyFieldSetting.ImportDivision == 1)
                    {
                        int settingIndex = currencyFieldSetting.FieldIndex.Value;
                        fieldIndex = currencyFieldSetting.FieldIndex.Value - 1;

                        string currencyCode = values[fieldIndex].Trim();

                        //空白チェック
                        if (string.IsNullOrEmpty(currencyCode))
                        {
                            billingImport.CurrencyCode = "*";
                            errorHandler(i, fieldIndex, currencyFieldSetting.FieldName, "empty", null);
                        }
                        else
                        {
                            if (IsAlphabet(currencyCode)) currencyCode = EbDataHelper.ConvertToUpperCase(currencyCode);

                            var currency = ExistCurrencies.FirstOrDefault(x => x.Code == currencyCode);
                            decimalScale = currency?.Precision ?? 0;
                            setCurrencyDisplayFormat(decimalScale);

                            // 重複チェックのため条件設定
                            if (currencyFieldSetting.IsUnique == 1) billingImportDuplication.CurrencyCode = currencyCode;

                            //エラーなしの場合設定
                            billingImport.CurrencyCode = currencyCode;
                        }
                    }
                    else
                    {
                        billingImport.CurrencyCode = (!UseForeignCurrency) ? DefaultCurrencyCode : currencyFieldSetting.FixedValue;
                        var currency = ExistCurrencies.Find(x => x.Code == billingImport.CurrencyCode);
                        billingImport.CurrencyId = currency.Id;
                    }
                    #endregion

                    #region 読込＆読込後すぐできる検証処理
                    foreach (var detail in ImporterSettingDetail)
                    {
                        fieldIndex = detail.FieldIndex.Value - 1;
                        int settingIndex = detail.FieldIndex.Value;

                        //項目存在チェック
                        if (!IsExist(fieldIndex, values.Length))
                        {
                            errorHandler(i, settingIndex, detail.FieldName, "not", null);
                            break;
                        }

                        #region 得意先コード検証処理
                        if (detail.Sequence == (int)Fields.CustomerCode)
                        {
                            //空白チェック
                            if (string.IsNullOrEmpty(values[fieldIndex]))
                            {
                                billingImport.CustomerCode = "*";
                                errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                continue;
                            }

                            string code = values[fieldIndex];
                            code = EbDataHelper.ConvertToUpperCase(code);
                            code = EbDataHelper.ConvertToHankakuKatakana(code);

                            bool validCodeType = Regex.IsMatch(code, CustomerHelper.CustomerPermission(ApplicationControl.CustomerCodeType));
                            
                            if (validCodeType && ApplicationControl.CustomerCodeType == 0)
                            {
                                code = code.PadLeft(ApplicationControl?.CustomerCodeLength ?? 0, '0');
                            }
                            if (validCodeType && ApplicationControl.CustomerCodeType == 2)
                            {
                                code = EbDataHelper.ConvertToHankakuKatakana(code);
                            }
                            
                            //得意先マスターを自動作成する
                            if (ImporterSetting.AutoCreationCustomer == 1)
                            {
                                //桁数チェック
                                if (code.Length > ApplicationControl?.CustomerCodeLength)
                                {
                                    billingImport.CustomerCode = code + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", ApplicationControl.CustomerCodeLength);
                                    continue;
                                }

                                //文字種チェック
                                if (!validCodeType)
                                {
                                    billingImport.CustomerCode = code + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }
                            }

                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) billingImportDuplication.CustomerCode = code;

                            //エラーなしの場合設定
                            billingImport.CustomerCode = code;
                            continue;
                        }
                        #endregion

                        #region 請求日検証処理
                        if (detail.Sequence == (int)Fields.BilledAt)
                        {
                            //空白チェック
                            if (string.IsNullOrEmpty(values[fieldIndex]))
                            {
                                billingImport.BilledAtForPrint = "*";
                                errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                continue;
                            }

                            //フォーマットチェック
                            DateTime? date = formatBilledAt(values[fieldIndex]);
                            if (!date.HasValue)
                            {
                                billingImport.BilledAtForPrint = values[fieldIndex] + "*";
                                errorHandler(i, settingIndex, detail.FieldName, "format", null);
                                continue;
                            }

                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) billingImportDuplication.BilledAt = date.Value;

                            //エラーなしの場合設定
                            billingImport.BilledAt = date.Value;
                            billingImport.BilledAtForPrint = billingImport.BilledAt.ToString().Substring(0, 11);
                            continue;
                        }
                        #endregion

                        #region 請求金額検証処理
                        if (detail.Sequence == (int)Fields.BillingAmount)
                        {
                            #region 取込無の場合設定
                            if (detail.ImportDivision == 0)
                            {
                                decimal taxAmount = 0M;
                                decimal price = 0M;
                                decimal taxRate = 0M;
                                int taxFieldIndex = (ImporterSettingDetail.FirstOrDefault(x => x.Sequence == (int)Fields.TaxAmount).FieldIndex) ?? 5;
                                int priceFieldIndex = (ImporterSettingDetail.FirstOrDefault(x => x.Sequence == (int)Fields.Price).FieldIndex) ?? 20;
                                int taxRateFieldIndex = (ImporterSettingDetail.FirstOrDefault(x => x.Sequence == (int)Fields.TaxRate).FieldIndex) ?? 38;
                                int taxImportDivision = ImporterSettingDetail.FirstOrDefault(k => k.Sequence == (int)Fields.TaxAmount).ImportDivision;
                                int taxRateAttributeDivision = (int)ImporterSettingDetail.FirstOrDefault(k => k.Sequence == (int)Fields.TaxRate).AttributeDivision;

                                if (taxImportDivision == 2
                                     && values[priceFieldIndex - 1] != "" && values[taxFieldIndex - 1] != ""
                                     && (decimal.TryParse(values[taxFieldIndex - 1], out taxAmount))
                                     && (decimal.TryParse(values[priceFieldIndex - 1], out price)))
                                {
                                    billingImport.BillingAmount = price + taxAmount;
                                    billingImport.BillingAmountForPrint = billingImport.BillingAmount.ToString(currencyDisplayFormat);
                                }
                                else if (taxImportDivision == 1
                                         && values[priceFieldIndex - 1] != "" && values[taxRateFieldIndex - 1] != ""
                                         && (IsValidTaxRate(values[taxRateFieldIndex - 1], taxRateAttributeDivision, out taxRate))
                                         && (decimal.TryParse(values[priceFieldIndex - 1], out price)))
                                {
                                    taxAmount = price < 0 ? ((Math.Abs(price) * taxRate) * -1) : (price * taxRate);
                                    billingImport.BillingAmount = price + taxAmount;
                                    billingImport.BillingAmountForPrint = billingImport.BillingAmount.ToString(currencyDisplayFormat);

                                    billingImport.TaxAmount = taxAmount;
                                    billingImport.TaxAmountForPrint = billingImport.TaxAmount.ToString(currencyDisplayFormat);
                                }

                                continue;
                            }
                            #endregion

                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1 || detail.ImportDivision == 2 ||
                                detail.ImportDivision == 3 || detail.ImportDivision == 4)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.BillingAmountForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //フォーマットチェック
                                decimal billingAmount = 0M;
                                if (!decimal.TryParse(values[fieldIndex], out billingAmount))
                                {
                                    billingImport.BillingAmountForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "format", null);
                                    continue;
                                }

                                //桁数チェック
                                var divBillingAmount = billingAmount.ToString().Split('.');
                                if (decimal.Parse(divBillingAmount[0]) < (MaxAmount * -1) || MaxAmount < decimal.Parse(divBillingAmount[0]))
                                {
                                    billingImport.BillingAmountForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", 11);
                                    continue;
                                }

                                bool isExistScale = billingAmount != 0M && divBillingAmount.Count() > 1;
                                if (isExistScale && divBillingAmount[1].TrimEnd('0').Length > decimalScale && roundingType == RoundingType.Error)
                                {
                                    billingImport.BillingAmountForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "precision", null);
                                    continue;
                                }

                                //ゼロの場合チェック
                                if (billingAmount == 0M && detail.ImportDivision == 1)
                                {
                                    billingImport.BillingAmountForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "zero", null);
                                    continue;
                                }

                                if (billingAmount == 0M && detail.ImportDivision == 2)
                                {
                                    skipData = true;
                                    skipDataCount++;
                                    break;
                                }
                                //重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.BillingAmount = billingAmount;

                                //エラーなしの場合設定
                                billingImport.BillingAmount = billingAmount;
                                billingImport.BillingAmountForPrint = billingImport.BillingAmount.ToString(currencyDisplayFormat);
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 消費税検証処理
                        if (detail.Sequence == (int)Fields.TaxAmount)
                        {
                            #region 取込無の場合設定
                            if (detail.ImportDivision == 0)
                            {
                                billingImport.TaxAmount = 0M;
                                billingImport.TaxAmountForPrint = billingImport.TaxAmount.ToString(currencyDisplayFormat);
                                continue;
                            }
                            #endregion

                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 2)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.TaxAmountForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //フォーマットチェック
                                decimal taxAmount = 0M;
                                if (!decimal.TryParse(values[fieldIndex], out taxAmount))
                                {
                                    billingImport.TaxAmountForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "format", null);
                                    continue;
                                }

                                //桁数チェック
                                var divTaxAmount = taxAmount.ToString().Split('.');
                                if (decimal.Parse(divTaxAmount[0]) < (MaxAmount * -1) || MaxAmount < decimal.Parse(divTaxAmount[0]))
                                {
                                    billingImport.TaxAmountForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", 11);
                                    continue;
                                }

                                bool isExistScale = taxAmount != 0M && divTaxAmount.Count() > 1;
                                if (isExistScale && divTaxAmount[1].TrimEnd('0').Length > decimalScale && roundingType == RoundingType.Error)
                                {
                                    billingImport.TaxAmountForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "precision", null);
                                    continue;
                                }

                                var detailBillingAmount = GetSettingDetail(Fields.BillingAmount);
                                //請求金額+税額 ゼロの場合チェック
                                if ((billingImport.BillingAmount + taxAmount) == 0M && (detailBillingAmount != null && detailBillingAmount.ImportDivision == 3))
                                {
                                    billingImport.BillingAmountForPrint += "*";
                                    errorHandler(i, settingIndex, detailBillingAmount.FieldName, "zero", null);
                                    billingImport.TaxAmountForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "zero", null);
                                    continue;
                                }
                                //請求金額+税額 ゼロの場合スキップ
                                if ((billingImport.BillingAmount + taxAmount) == 0M && (detailBillingAmount != null && detailBillingAmount.ImportDivision == 4))
                                {
                                    skipData = true;
                                    skipDataCount++;
                                    break;
                                }

                                //重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.TaxAmount = taxAmount;

                                //エラーなしの場合設定
                                billingImport.TaxAmount = taxAmount;
                                billingImport.TaxAmountForPrint = billingImport.TaxAmount.ToString(currencyDisplayFormat);
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 入金予定日検証処理
                        if (detail.Sequence == (int)Fields.DueAt)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1 || (detail.ImportDivision == 2 && !string.IsNullOrWhiteSpace(values[detail.FieldIndex.Value - 1])))
                            {
                                //空白チェック
                                if (string.IsNullOrWhiteSpace(values[fieldIndex]))
                                {
                                    billingImport.DueAtForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //フォーマットチェック
                                DateTime? date = formatDueAt(values[detail.FieldIndex.Value - 1]);
                                if (!date.HasValue)
                                {
                                    billingImport.DueAtForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "format", null);
                                    continue;
                                }

                                //重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.DueAt = date.Value;

                                //エラーなしの場合設定
                                billingImport.DueAt = date.Value;
                                billingImport.DueAtForPrint = billingImport.DueAt.ToString().Substring(0, 11);
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 請求部門検証処理
                        if (detail.Sequence == (int)Fields.DepartmentCode)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.DepartmentCode = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                string code = values[fieldIndex];

                                if (ApplicationControl.DepartmentCodeType == 0 && IsMoney(code))
                                    code = code.PadLeft(ApplicationControl?.DepartmentCodeLength ?? 0, '0');
                                else if (ApplicationControl.DepartmentCodeType == 1 && IsNumberAlphabet(code))
                                    code = EbDataHelper.ConvertToUpperCase(code);

                                //重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.DepartmentCode = code;

                                //エラーなしの場合設定
                                billingImport.DepartmentCode = code;
                                continue;
                            }
                            #endregion

                            #region 固定値設定の場合
                            if (detail.ImportDivision == 2)
                            {
                                billingImport.DepartmentCode = detail.FixedValue;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 債権科目検証処理
                        if (detail.Sequence == (int)Fields.DebitAccountTitleCode)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                string code = values[fieldIndex];

                                if (ApplicationControl.AccountTitleCodeType == 0 && IsMoney(code))
                                    code = code.PadLeft(ApplicationControl?.AccountTitleCodeLength ?? 0, '0');
                                else if (ApplicationControl.AccountTitleCodeType == 1 && IsNumberAlphabet(code))
                                    code = EbDataHelper.ConvertToUpperCase(code);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.DebitAccountTitleCode = code;

                                //エラーなしの場合設定
                                billingImport.DebitAccountTitleCode = code;
                                continue;
                            }
                            #endregion

                            #region 固定値設定の場合
                            if (detail.ImportDivision == 2)
                            {
                                billingImport.DebitAccountTitleCode = detail.FixedValue;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 売上日検証処理
                        if (detail.Sequence == (int)Fields.SalesAt)
                        {
                            #region 取込無の設定
                            if (detail.ImportDivision == 0)
                            {
                                billingImport.SalesAt = billingImport.BilledAt;
                                billingImport.SaleAtForPrint = billingImport.SalesAt.ToString();
                                if (billingImport.SaleAtForPrint.Contains("0001/01/01"))
                                    billingImport.SaleAtForPrint = "";
                                continue;
                            }
                            #endregion

                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.SaleAtForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //フォーマットチェック
                                DateTime? date = formatSalesAt(values[fieldIndex]);
                                if (!date.HasValue)
                                {
                                    billingImport.SaleAtForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "format", null);
                                    continue;
                                }

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.SalesAt = date.Value;

                                //エラーなしの場合設定
                                billingImport.SalesAt = date.Value;
                                billingImport.SaleAtForPrint = billingImport.SalesAt.ToString().Substring(0, 11);
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 請求書番号検証処理
                        if (detail.Sequence == (int)Fields.InvoiceCode)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string code = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && code.Length > 20)
                                {
                                    billingImport.InvoiceCode = code + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "lengthChar", 20);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && code.Length > 20) code = code.Substring(0, 20);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.InvoiceCode = code;

                                //エラーなしの場合設定
                                billingImport.InvoiceCode = code;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 請求締日検証処理
                        if (detail.Sequence == (int)Fields.ClosingAt)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.ClosingAtForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //フォーマットチェック
                                DateTime? date = formatClosingAt(values[fieldIndex]);
                                if (!date.HasValue)
                                {
                                    billingImport.ClosingAtForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "format", null);
                                    continue;
                                }

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.ClosingAt = date.Value;

                                //エラーなしの場合設定
                                billingImport.ClosingAt = date.Value;
                                billingImport.ClosingAtForPrint = billingImport.ClosingAt.ToString().Substring(0, 11);
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region　担当者検証処理
                        if (detail.Sequence == (int)Fields.StaffCode)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.StaffCode = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                string code = values[fieldIndex];

                                if (ApplicationControl.StaffCodeType == 0 && IsMoney(code))
                                    code = code.PadLeft(ApplicationControl?.StaffCodeLength ?? 0, '0');
                                else if (ApplicationControl.StaffCodeType == 1 && IsNumberAlphabet(code))
                                    code = EbDataHelper.ConvertToUpperCase(code);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.StaffCode = code;


                                //エラーなしの場合設定
                                billingImport.StaffCode = code;
                                continue;
                            }
                            #endregion

                            #region 固定値の設定
                            if (detail.ImportDivision == 2)
                            {
                                billingImport.StaffCode = detail.FixedValue;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 備考検証処理
                        if (detail.Sequence == (int)Fields.Note1)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string note = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && IsValidNoteLength(note, UseControlInputNote))
                                {
                                    billingImport.Note1 = note + "*";
                                    var key = UseControlInputNote ? "lengthInvoiceChar" : "lengthChar";
                                    int? length = UseControlInputNote ? (int?)null : 100;
                                    errorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note1) ? detail.FieldName : Note1, key, length);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && IsValidNoteLength(note, UseControlInputNote)) note = GetSubStringNote(note, UseControlInputNote);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Note1 = note;

                                //エラーなしの場合設定
                                billingImport.Note1 = note;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 請求区分検証処理
                        if (detail.Sequence == (int)Fields.BillingCategoryCode)
                        {
                            #region 固定値設定
                            if (detail.ImportDivision == 0)
                            {
                                billingImport.BillingCategoryCode = detail.FixedValue;
                                continue;
                            }
                            #endregion

                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.BillingCategoryCode = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                billingImport.BillingCategoryCode = detail.AttributeDivision == 1 ? values[fieldIndex].PadLeft(2, '0') : values[fieldIndex];

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.BillingCategoryCode = billingImport.BillingCategoryCode;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 回収区分検証処理
                        if (detail.Sequence == (int)Fields.CollectCategoryCode)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.CollectCategoryCode = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                billingImport.CollectCategoryCode = detail.AttributeDivision == 1 ? values[fieldIndex].PadLeft(2, '0') : values[fieldIndex];

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.CollectCategoryCode = billingImport.CollectCategoryCode;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 契約番号の検証処理
                        if (detail.Sequence == (int)Fields.ContractNumber)
                        {
                            #region 取込有の場合検証するためデータ設定
                            if (detail.ImportDivision == 1)
                            {
                                billingImport.ContractNumber = values[fieldIndex];

                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 金額（抜)検証処理
                        if (detail.Sequence == (int)Fields.Price)
                        {
                            #region 取込無の設定
                            if (detail.ImportDivision == 0)
                            {
                                billingImport.Price = billingImport.BillingAmount - billingImport.TaxAmount;
                                billingImport.PriceForPrint = billingImport.Price.ToString(currencyDisplayFormat);
                                continue;
                            }
                            #endregion

                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1 || detail.ImportDivision == 2)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.PriceForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //フォーマットチェック
                                decimal price = 0M;
                                if (!decimal.TryParse(values[fieldIndex], out price))
                                {
                                    billingImport.PriceForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "format", null);
                                    continue;
                                }

                                //桁数チェック
                                var divPrice = price.ToString().Split('.');
                                if (decimal.Parse(divPrice[0]) < (MaxAmount * -1) || MaxAmount < decimal.Parse(divPrice[0]))
                                {
                                    billingImport.PriceForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", 11);
                                    continue;
                                }

                                bool isExistScale = price != 0M && divPrice.Count() > 1;
                                if (isExistScale && divPrice[1].TrimEnd('0').Length > decimalScale && roundingType == RoundingType.Error)
                                {
                                    billingImport.PriceForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "precision", null);
                                    continue;
                                }

                                //ゼロの場合チェック
                                if (price == 0M && detail.ImportDivision == 1)
                                {
                                    billingImport.PriceForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "zero", null);
                                    continue;
                                }

                                if (price == 0M && detail.ImportDivision == 2)
                                {
                                    skipData = true;
                                    skipDataCount++;
                                    break;
                                }

                                //sign match check
                                if ((price > 0 && billingImport.TaxAmount < 0)
                                    || (price < 0 && billingImport.TaxAmount > 0))
                                {
                                    billingImport.PriceForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "notMatchSignTaxExcluded", null);
                                    continue;
                                }

                                // check amount
                                if (Math.Abs(price) < Math.Abs(billingImport.TaxAmount))
                                {
                                    billingImport.PriceForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "compareAmountTaxExcluded", null);
                                    continue;
                                }

                                //重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Price = price;

                                //エラーなしの場合設定
                                billingImport.Price = price;
                                billingImport.PriceForPrint = billingImport.BillingAmount.ToString(currencyDisplayFormat);
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 税区検証処理
                        if (detail.Sequence == (int)Fields.TaxClassId)
                        {
                            #region 取込無の設定
                            if (detail.ImportDivision == 0)
                            {
                                billingImport.TaxClassId = 0;
                                billingImport.TaxClassIdForPrint = "0";
                                continue;
                            }
                            #endregion

                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.TaxClassIdForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //入力値チェック
                                TaxClass taxClass = null;
                                int taxId = 0;
                                bool isValidValue = int.TryParse(values[fieldIndex], out taxId);

                                if (isValidValue && taxClasses != null)
                                {
                                    taxClass = taxClasses.FirstOrDefault(d => d.Id == taxId);
                                }

                                if (taxClass == null)
                                {
                                    billingImport.TaxClassIdForPrint = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.TaxClassId = taxId;

                                //エラーなしの場合設定
                                billingImport.TaxClassId = taxId;
                                billingImport.TaxClassIdForPrint = values[fieldIndex];
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 備考2検証処理
                        if (detail.Sequence == (int)Fields.Note2)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string note = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && IsValidNoteLength(note, UseControlInputNote))
                                {
                                    billingImport.Note2 = note + "*";
                                    var key = UseControlInputNote ? "lengthInvoiceChar" : "lengthChar";
                                    int? length = UseControlInputNote ? (int?)null : 100;
                                    errorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note2) ? detail.FieldName : Note2, key, length);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && IsValidNoteLength(note, UseControlInputNote)) note = GetSubStringNote(note, UseControlInputNote);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Note2 = note;

                                //エラーなしの場合設定
                                billingImport.Note2 = note;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 備考3検証処理
                        if (detail.Sequence == (int)Fields.Note3)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string note = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && IsValidNoteLength(note))
                                {
                                    billingImport.Note3 = note + "*";
                                    errorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note3) ? detail.FieldName : Note3, "lengthChar", 100);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && IsValidNoteLength(note)) note = GetSubStringNote(note);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Note3 = note;

                                //エラーなしの場合設定
                                billingImport.Note3 = note;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 備考4検証処理
                        if (detail.Sequence == (int)Fields.Note4)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string note = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && IsValidNoteLength(note))
                                {
                                    billingImport.Note4 = note + "*";
                                    errorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note4) ? detail.FieldName : Note4, "lengthChar", 100);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && IsValidNoteLength(note)) note = GetSubStringNote(note);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Note4 = note;

                                //エラーなしの場合設定
                                billingImport.Note4 = note;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 備考5検証処理
                        if (detail.Sequence == (int)Fields.Note5)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string note = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && IsValidNoteLength(note))
                                {
                                    billingImport.Note5 = note + "*";
                                    errorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note5) ? detail.FieldName : Note5, "lengthChar", 100);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && IsValidNoteLength(note)) note = GetSubStringNote(note);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Note5 = note;

                                //エラーなしの場合設定
                                billingImport.Note5 = note;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 備考6検証処理
                        if (detail.Sequence == (int)Fields.Note6)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string note = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && IsValidNoteLength(note))
                                {
                                    billingImport.Note6 = note + "*";
                                    errorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note6) ? detail.FieldName : Note6, "lengthChar", 100);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && IsValidNoteLength(note)) note = GetSubStringNote(note);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Note6 = note;

                                //エラーなしの場合設定
                                billingImport.Note6 = note;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 備考7検証処理
                        if (detail.Sequence == (int)Fields.Note7)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string note = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && IsValidNoteLength(note))
                                {
                                    billingImport.Note7 = note + "*";
                                    errorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note7) ? detail.FieldName : Note7, "lengthChar", 100);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && IsValidNoteLength(note)) note = GetSubStringNote(note);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Note7 = note;

                                //エラーなしの場合設定
                                billingImport.Note7 = note;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 備考8検証処理
                        if (detail.Sequence == (int)Fields.Note8)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白の場合スキップ
                                if (string.IsNullOrWhiteSpace(values[fieldIndex])) continue;

                                //桁数チェック
                                string note = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && IsValidNoteLength(note))
                                {
                                    billingImport.Note8 = note + "*";
                                    errorHandler(i, settingIndex, string.IsNullOrWhiteSpace(Note8) ? detail.FieldName : Note8, "lengthChar", 100);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && IsValidNoteLength(note)) note = GetSubStringNote(note);

                                // 重複チェックのため条件設定
                                if (detail.IsUnique == 1) billingImportDuplication.Note8 = note;

                                //エラーなしの場合設定
                                billingImport.Note8 = note;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 得意先名称検証処理
                        if (detail.Sequence == (int)Fields.CustomerName)
                        {
                            #region 固定値の設定
                            if (detail.ImportDivision == 1)
                            {
                                billingImport.CustomerName = detail.FixedValue;
                                continue;
                            }
                            #endregion

                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 2)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.CustomerName = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //桁数チェック
                                string customerName = values[fieldIndex];
                                if (detail.AttributeDivision == 1 && customerName.Length > 140)
                                {
                                    billingImport.CustomerName = customerName + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "lengthChar", 140);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && customerName.Length > 140)
                                    customerName = customerName.Substring(0, 140);

                                //エラーなしの場合設定
                                billingImport.CustomerName = customerName;
                                continue;
                            }
                            #endregion

                            #region 得意先コードの設定
                            if (detail.ImportDivision == 3)
                            {
                                billingImport.CustomerName = billingImport.CustomerCode;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 得意先カナ検証処理
                        if (detail.Sequence == (int)Fields.CustomerKana)
                        {
                            #region 固定値の設定
                            if (detail.ImportDivision == 1)
                            {
                                billingImport.CustomerKana = detail.FixedValue;
                                continue;
                            }
                            #endregion

                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 2)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.CustomerKana = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                string customerNameKana = values[fieldIndex];

                                if (EbDataHelper.ContainsKanji(customerNameKana))
                                {
                                    billingImport.CustomerKana = customerNameKana + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }

                                customerNameKana = ConvertValidKana(customerNameKana);

                                if (string.IsNullOrEmpty(customerNameKana))
                                {
                                    billingImport.CustomerKana = customerNameKana + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "emptyKanaName", null);
                                    continue;
                                }

                                if (detail.AttributeDivision == 1 && customerNameKana.Length > 140)
                                {
                                    billingImport.CustomerKana = customerNameKana + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "lengthChar", 140);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && customerNameKana.Length > 140)
                                    customerNameKana = customerNameKana.Substring(0, 140);

                                //エラーなしの場合設定
                                billingImport.CustomerKana = customerNameKana;
                                continue;
                            }
                            #endregion

                            #region 得意先コードの設定
                            if (detail.ImportDivision == 3)
                            {
                                billingImport.CustomerKana = billingImport.CustomerCode;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 照合番号検証処理
                        if (detail.Sequence == (int)Fields.CollationKey)
                        {
                            if (detail.ImportDivision == 1)
                            {
                                var value = values[fieldIndex];
                                if (string.IsNullOrEmpty(value)) continue;
                                value = EbDataHelper.ConvertToHankakuKatakana(value);
                                billingImport.CollationKey = value;

                                if (!IsNumber(value))
                                {
                                    billingImport.CollationKey += "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }

                                if (detail.AttributeDivision == 1 && value.Length > 48)
                                {
                                    billingImport.CollationKey += "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", 48);
                                    continue;
                                }

                                if (detail.AttributeDivision == 2 && value.Length > 48)
                                {
                                    billingImport.CollationKey = value.Substring(0, 48);
                                }

                            }
                        }
                        #endregion

                        #region 歩引利用検証処理
                        if (detail.Sequence == (int)Fields.UseDiscount)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    billingImport.UseDiscounForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                //入力チェック
                                if (values[fieldIndex] != "0" && values[fieldIndex] != "1")
                                {
                                    billingImport.UseDiscounForPrint = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }
                                else
                                {
                                    billingImport.UseDiscount = int.Parse(values[fieldIndex]);
                                }
                            }
                            #endregion

                            #region 固定値設定
                            if (detail.ImportDivision == 2)
                            {
                                billingImport.UseDiscount = int.Parse(detail.FixedValue);
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 銀行コード検証処理
                        if (detail.Sequence == (int)Fields.ExclusiveBankCode)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //フォーマットチェック
                                string exclusiveBankCode = values[fieldIndex].Trim();
                                bool allowSpace = detail.AttributeDivision == 2 && string.IsNullOrEmpty(exclusiveBankCode);

                                if (!allowSpace && string.IsNullOrEmpty(exclusiveBankCode))
                                {
                                    billingImport.ExclusiveBankCode = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }
                                else if (!allowSpace && !string.IsNullOrEmpty(exclusiveBankCode) && !IsMoney(exclusiveBankCode))
                                {
                                    billingImport.ExclusiveBankCode = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }

                                //桁数チェック
                                if (exclusiveBankCode.Length > 4)
                                {
                                    billingImport.ExclusiveBankCode = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", 4);
                                    continue;
                                }

                                //エラーなしの場合設定
                                billingImport.ExclusiveBankCode = allowSpace ? exclusiveBankCode : exclusiveBankCode.PadLeft(4, '0');
                                continue;
                            }
                            #endregion

                            #region 固定値設定の場合
                            if (detail.ImportDivision == 2)
                            {
                                billingImport.ExclusiveBankCode = detail.FixedValue;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 支店コード検証処理
                        if (detail.Sequence == (int)Fields.ExclusiveBranchCode)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //フォーマットチェック
                                string exclusiveBranchCode = values[fieldIndex].Trim();
                                bool allowSpace = detail.AttributeDivision == 2 && string.IsNullOrEmpty(exclusiveBranchCode);

                                if (!allowSpace && string.IsNullOrEmpty(exclusiveBranchCode))
                                {
                                    billingImport.ExclusiveBranchCode = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }
                                else if (!allowSpace && !string.IsNullOrEmpty(exclusiveBranchCode) && !IsMoney(exclusiveBranchCode))
                                {
                                    billingImport.ExclusiveBranchCode = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }

                                //桁数チェック
                                if (exclusiveBranchCode.Length > 3)
                                {
                                    billingImport.ExclusiveBranchCode = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", 3);
                                    continue;
                                }

                                //エラーなしの場合設定
                                billingImport.ExclusiveBranchCode = allowSpace ? exclusiveBranchCode : exclusiveBranchCode.PadLeft(3, '0');
                                continue;
                            }
                            #endregion

                            #region 固定値設定の場合
                            if (detail.ImportDivision == 2)
                            {
                                billingImport.ExclusiveBranchCode = detail.FixedValue;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 仮想支店コード検証処理
                        if (detail.Sequence == (int)Fields.ExclusiveVirtualBranchCode)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //フォーマットチェック
                                string exclusiveVirtualBranchCode = values[fieldIndex].Trim();
                                bool allowSpace = detail.AttributeDivision == 2 && string.IsNullOrEmpty(exclusiveVirtualBranchCode);

                                if (!allowSpace && string.IsNullOrEmpty(exclusiveVirtualBranchCode))
                                {
                                    billingImport.ExclusiveVirtualBranchCode = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }
                                else if (!allowSpace && !string.IsNullOrEmpty(exclusiveVirtualBranchCode) && !IsMoney(exclusiveVirtualBranchCode))
                                {
                                    billingImport.ExclusiveVirtualBranchCode = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }

                                //桁数チェック
                                if (exclusiveVirtualBranchCode.Length > 3)
                                {
                                    billingImport.ExclusiveVirtualBranchCode = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", 3);
                                    continue;
                                }

                                //エラーなしの場合設定
                                billingImport.ExclusiveVirtualBranchCode = allowSpace ? exclusiveVirtualBranchCode : exclusiveVirtualBranchCode.PadLeft(3, '0');
                                continue;
                            }
                            #endregion

                            #region 固定値設定の場合
                            if (detail.ImportDivision == 2)
                            {
                                billingImport.ExclusiveVirtualBranchCode = detail.FixedValue;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 仮想口座番号検証処理
                        if (detail.Sequence == (int)Fields.ExclusiveAccountNumber)
                        {
                            #region 取込有の場合チェック
                            if (detail.ImportDivision == 1)
                            {
                                //フォーマットチェック
                                string exclusiveAccountNumber = values[fieldIndex].Trim();
                                bool allowSpace = detail.AttributeDivision == 2 && string.IsNullOrEmpty(exclusiveAccountNumber);

                                if (!allowSpace && string.IsNullOrEmpty(exclusiveAccountNumber))
                                {
                                    billingImport.ExclusiveAccountNumber = "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }
                                else if (!allowSpace && !string.IsNullOrEmpty(exclusiveAccountNumber) && !IsMoney(exclusiveAccountNumber))
                                {
                                    billingImport.ExclusiveAccountNumber = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "wrong", null);
                                    continue;
                                }

                                //桁数チェック
                                if (exclusiveAccountNumber.Length > 7)
                                {
                                    billingImport.ExclusiveAccountNumber = values[fieldIndex] + "*";
                                    errorHandler(i, settingIndex, detail.FieldName, "length", 7);
                                    continue;
                                }

                                //エラーなしの場合設定
                                billingImport.ExclusiveAccountNumber = allowSpace ? exclusiveAccountNumber : exclusiveAccountNumber.PadLeft(7, '0');
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                        #region 消費税率検証処理
                        if (detail.Sequence == (int)Fields.TaxRate)
                        {
                            #region 消費税検証処理
                            if (detail.ImportDivision == 1)
                            {
                                //空白チェック
                                if (string.IsNullOrEmpty(values[fieldIndex]))
                                {
                                    errorHandler(i, settingIndex, detail.FieldName, "empty", null);
                                    continue;
                                }

                                decimal taxRate = 0M;
                                if (!IsValidTaxRate(values[fieldIndex], (int)detail.AttributeDivision, out taxRate))
                                {
                                    errorHandler(i, settingIndex, detail.FieldName, "format", null);
                                    continue;
                                }
                                if (taxRate < 0M || taxRate > 1M)
                                {
                                    errorHandler(i, settingIndex, detail.FieldName, "valueOutOfBound", null);
                                    continue;
                                }

                                billingImport.TaxRate = taxRate;
                                continue;
                            }
                            #endregion
                        }
                        #endregion

                    }
                    #endregion

                    if (skipData) continue;

                    BillingImportDuplication.Add(billingImportDuplication);

                    billingImport.CompanyCode = CompanyCode;
                    billingImport.UseLongTermAdvanceReceived = ApplicationControl.UseLongTermAdvanceReceived;
                    billingImport.RegisterContractInAdvance = ApplicationControl.RegisterContractInAdvance;

                    PossibleData.Add(billingImport);
                }
                #endregion

                #region マスター存在チェック
                if (PossibleData.Count > 0)
                {
                    await SolveCodeToIdAsync();

                    //金額の端数処理
                    if (roundingType != RoundingType.Error && ExistCurrencies != null)
                    {
                        foreach (var billingImport in PossibleData)
                        {
                            var code = billingImport.CurrencyCode;
                            if (code == null || code == "*") continue;

                            var currency = ExistCurrencies.Find(x => x.Code == code);
                            int precision = currency?.Precision ?? 0;

                            billingImport.BillingAmount = roundingType.Calc(billingImport.BillingAmount, precision).Value;
                            billingImport.TaxAmount = roundingType.Calc(billingImport.TaxAmount, precision).Value;
                            billingImport.Price = roundingType.Calc(billingImport.Price, precision).Value;
                            billingImport.BillingAmountForPrint = string.IsNullOrEmpty(billingImport.BillingAmountForPrint) ? string.Empty :
                                billingImport.BillingAmountForPrint.Contains("*") ? billingImport.BillingAmountForPrint :
                                billingImport.BillingAmount.ToString(currencyDisplayFormat);
                            billingImport.TaxAmountForPrint = string.IsNullOrEmpty(billingImport.TaxAmountForPrint) ? string.Empty :
                                billingImport.TaxAmountForPrint.Contains("*") ? billingImport.TaxAmountForPrint :
                                billingImport.TaxAmount.ToString(currencyDisplayFormat);
                            billingImport.PriceForPrint = string.IsNullOrEmpty(billingImport.PriceForPrint) ? string.Empty :
                                billingImport.PriceForPrint.Contains("*") ? billingImport.PriceForPrint :
                                billingImport.Price.ToString(currencyDisplayFormat);
                        }
                    }
                }
                #endregion

                #region 請求金額（税込)チェック
                if (PossibleData.Count > 0)
                {
                    var detailBillingAmount = GetSettingDetail(Fields.BillingAmount);

                    #region 外税設定不可チェック
                    if (detailBillingAmount.ImportDivision == 1
                        || detailBillingAmount.ImportDivision == 2)
                    {
                        var detailBillingCategory = GetSettingDetail(Fields.BillingCategoryCode);
                        var detailTaxClass = GetSettingDetail(Fields.TaxClassId);

                        foreach (var billing in PossibleData)
                        {
                            if (detailTaxClass != null
                                && detailTaxClass.ImportDivision == 0
                                && !billing.BillingCategoryCode.Contains("*"))
                            {
                                var category = ExistBillingCategory.First(x => x.Code == billing.BillingCategoryCode);
                                if (category.TaxClassId == (int)TaxClassId.TaxExclusive)
                                {
                                    var code = billing.BillingCategoryCode;
                                    ErrorReport.Add(GetWorkingReport(detailBillingCategory,
                                        billing.LineNo,
                                        code,
                                        MsgErrBillingCategoryInvalidTaxClass));

                                    billing.BillingCategoryCode += "*";
                                }
                            }
                            else if (detailTaxClass != null && detailTaxClass.ImportDivision == 1)
                            {
                                if (billing.TaxClassId == (int)TaxClassId.TaxExclusive)
                                {
                                    var taxClassId = billing.TaxClassId.ToString();
                                    ErrorReport.Add(GetWorkingReport(detailTaxClass,
                                        billing.LineNo,
                                        taxClassId,
                                        MsgErrInvalidTaxClass));

                                    billing.TaxClassIdForPrint = taxClassId + "*";
                                }
                            }
                        }
                    }
                    #endregion

                    #region 消費税との組み合わせチェック
                    if (detailBillingAmount.ImportDivision == 3
                        || detailBillingAmount.ImportDivision == 4)
                    {
                        foreach (var billingImport in PossibleData)
                        {
                            //sign match check
                            if ((billingImport.BillingAmount > 0 && billingImport.TaxAmount < 0)
                                || (billingImport.BillingAmount < 0 && billingImport.TaxAmount > 0))
                            {
                                billingImport.BillingAmountForPrint = billingImport.BillingAmountForPrint + "*";
                                errorHandler(billingImport.LineNo,
                                    detailBillingAmount.FieldIndex.Value,
                                    detailBillingAmount.FieldName,
                                    "notMatchSignTaxIncluded",
                                    null);
                                continue;
                            }

                            // check amount
                            if (Math.Abs(billingImport.BillingAmount - billingImport.TaxAmount) < Math.Abs(billingImport.TaxAmount))
                            {
                                billingImport.BillingAmountForPrint = billingImport.BillingAmountForPrint + "*";
                                errorHandler(billingImport.LineNo,
                                    detailBillingAmount.FieldIndex.Value,
                                    detailBillingAmount.FieldName,
                                    "compareAmountTaxIncluded",
                                    null);
                                continue;
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region 重複チェック
                //ファイル内のチェック
                var notRequireDuplicate = new List<BillingImportDuplicationWithCode>();
                {   // 既存の検証処理で エラーとなった項目を除外
                    var errorLines = new HashSet<int>(ErrorReport.Where(x => x.LineNo.HasValue).Select(x => x.LineNo.Value).Distinct());
                    var errorIndexes = PossibleData.Where(x => errorLines.Contains(x.LineNo)).Select(x => PossibleData.IndexOf(x)).ToArray();
                    foreach (var index in errorIndexes)
                    {
                        ImpossibleData.Add(PossibleData[index]);
                        notRequireDuplicate.Add(BillingImportDuplication[index]);
                    }
                }
                PossibleData = PossibleData.Except(ImpossibleData).ToList();
                BillingImportDuplication = BillingImportDuplication.Except(notRequireDuplicate).ToList();

                var dupeCheckItems = ImporterSettingDetail.Where(d => d.IsUnique == 1).ToList();
                if (dupeCheckItems.Count > 0)
                {
                    var duplist = BillingImportDuplication.GroupBy(x => new
                    {
                        x.CustomerCode,
                        x.BilledAt,
                        x.BillingAmount,
                        x.TaxAmount,
                        x.DueAt,
                        x.DepartmentCode,
                        x.DebitAccountTitleCode,
                        x.SalesAt,
                        x.InvoiceCode,
                        x.ClosingAt,
                        x.StaffCode,
                        x.Note1,
                        x.BillingCategoryCode,
                        x.CollectCategoryCode,
                        x.Price,
                        x.TaxClassId,
                        x.Note2,
                        x.Note3,
                        x.Note4,
                        x.Note5,
                        x.Note6,
                        x.Note7,
                        x.Note8,
                        x.CurrencyCode
                    }).Where(x => x.Count() > 1).SelectMany(x => x).ToList();
                    if (duplist.Count > 0)
                    {
                        foreach (var error in duplist)
                        {
                            ErrorReport.Add(new WorkingReport
                            {
                                LineNo = error.RowNumber,
                                Message = $"重複しているため、インポートできません。",
                            });
                        }
                        var errorLines = new HashSet<int>(duplist.Select(x => x.RowNumber).Distinct());
                        var errorIndexes = PossibleData.Where(x => errorLines.Contains(x.LineNo)).Select(x => PossibleData.IndexOf(x)).ToArray();
                        foreach (var index in errorIndexes)
                        {
                            ImpossibleData.Add(PossibleData[index]);
                            notRequireDuplicate.Add(BillingImportDuplication[index]);
                        }
                    }

                    //DBのチェック
                    if (duplist.Count == 0)
                    {
                        try
                        {
                            var RowNumbers = await GetBillingImportDuplicationAsync(
                                BillingImportDuplication.ToArray(),
                                dupeCheckItems.ToArray());

                            if (!(RowNumbers?.Any() ?? false))
                            {
                                foreach (var errorLine in RowNumbers)
                                {
                                    ErrorReport.Add(new WorkingReport
                                    {
                                        LineNo = errorLine,
                                        Message = $"重複しているため、インポートできません。",
                                    });
                                }
                                var errorLines = new HashSet<int>(RowNumbers.Distinct());
                                var errorIndexes = PossibleData.Where(x => errorLines.Contains(x.LineNo)).Select(x => PossibleData.IndexOf(x)).ToArray();
                                foreach (var index in errorIndexes)
                                {
                                    ImpossibleData.Add(PossibleData[index]);
                                    notRequireDuplicate.Add(BillingImportDuplication[index]);
                                }
                            }
                            else
                            {
                                //DispStatusMessage(MsgErrImportErrorWithoutLog);
                            }
                        }
                        catch (Exception ex)
                        {
                            //Debug.Fail(ex.ToString());
                            LogError?.Invoke(ex);
                            //NLogHandler.WriteErrorLog(this, ex, SessionKey);
                        }
                    }
                }
                PossibleData = PossibleData.Except(ImpossibleData).ToList();
                #endregion

                #region マスター算出などの設定
                if (PossibleData.Count > 0)
                {
                    var dueAtSetting = GetSettingDetail(Fields.DueAt);
                    var departmentSetting = GetSettingDetail(Fields.DepartmentCode);
                    var closingAtSetting = GetSettingDetail(Fields.ClosingAt);
                    var staffSetting = GetSettingDetail(Fields.StaffCode);
                    var collectCategorySetting = GetSettingDetail(Fields.CollectCategoryCode);
                    var useDiscountSetting = GetSettingDetail(Fields.UseDiscount);
                    var taxClass = GetSettingDetail(Fields.TaxClassId);

                    var dicCustomer = ExistCustomer.ToDictionary(x => x.Id, x => x);
                    foreach (var billing in PossibleData)
                    {
                        var customer = dicCustomer.ContainsKey(billing.CustomerId) ? dicCustomer[billing.CustomerId] : null;
                        if (billing.AutoCreationCustomerFlag == 0 && customer == null) continue;

                        #region 入金予定日計算
                        if (dueAtSetting != null && (dueAtSetting.ImportDivision == 0 || (dueAtSetting.ImportDivision == 2 && billing.DueAt == DateTime.MinValue)))
                        {
                            // TODO : BilledAt -> ClosingAt   ClosingAt.HasValue
                            // TODO : !dueAt.HasValue -> error 
                            //既存の得意先の場合
                            if (billing.AutoCreationCustomerFlag == 0 || (billing.AutoCreationCustomerFlag == 1 && customer != null))
                            {
                                var closingAt = customer.GetClosingAt(billing.BilledAt);
                                var dueAt = customer.GetDueAt(closingAt ?? DateTime.Today, Holidays);
                                billing.DueAt = dueAt ?? DateTime.Today;
                                billing.DueAtForPrint = billing.DueAt.ToString().Substring(0, 11);
                            }
                            //新規得意先の場合
                            else
                            {
                                var newCustomer = new Customer();
                                newCustomer.ClosingDay = int.Parse(closingAtSetting.FixedValue);
                                newCustomer.CollectOffsetMonth = int.Parse(dueAtSetting.FixedValue.Substring(0, 1));
                                newCustomer.CollectOffsetDay = int.Parse(dueAtSetting.FixedValue.Substring(1, 2));
                                var closingAt = newCustomer.GetClosingAt(billing.BilledAt);
                                var dueAt = newCustomer.GetDueAt(closingAt ?? DateTime.Today, Holidays);
                                billing.DueAt = dueAt ?? DateTime.Today;
                                billing.DueAtForPrint = billing.DueAt.ToString().Substring(0, 11);
                            }
                        }
                        #endregion

                        #region 請求部門設定
                        if (departmentSetting != null && departmentSetting.ImportDivision == 0)
                        {
                            //既存の得意先の場合
                            if (billing.AutoCreationCustomerFlag == 0 || (billing.AutoCreationCustomerFlag == 1 && customer != null))
                            {
                                var staff = ExistStaff.First(x => x.Id == customer.StaffId);
                                billing.DepartmentId = staff.DepartmentId;
                                billing.DepartmentCode = staff.DepartmentCode;
                            }
                            //新規得意先の場合
                            else
                            {
                                var staff = ExistStaff.First(x => x.Code == staffSetting.FixedValue);
                                billing.DepartmentId = staff.DepartmentId;
                                billing.DepartmentCode = staff.DepartmentCode;
                            }
                        }
                        #endregion

                        #region 請求締日設定
                        if (closingAtSetting != null && closingAtSetting.ImportDivision == 0)
                        {
                            // TODO: !closingAt.HasValue -> error
                            //既存の得意先の場合
                            if (billing.AutoCreationCustomerFlag == 0 || (billing.AutoCreationCustomerFlag == 1 && customer != null))
                            {
                                var closingAt = customer.GetClosingAt(billing.BilledAt);
                                billing.ClosingAt = closingAt ?? DateTime.Today;
                                billing.ClosingAtForPrint = billing.ClosingAt.ToString().Substring(0, 11);
                            }
                            //新規得意先の場合
                            else
                            {
                                var newCustomer = new Customer();
                                newCustomer.ClosingDay = int.Parse(closingAtSetting.FixedValue);
                                var closingAt = newCustomer.GetClosingAt(billing.BilledAt);
                                billing.ClosingAt = closingAt ?? DateTime.Today;
                                billing.ClosingAtForPrint = billing.ClosingAt.ToString().Substring(0, 11);
                            }
                        }
                        #endregion

                        #region 担当者設定
                        if (staffSetting != null && staffSetting.ImportDivision == 0)
                        {
                            //既存の得意先の場合
                            if (billing.AutoCreationCustomerFlag == 0 || (billing.AutoCreationCustomerFlag == 1 && customer != null))
                            {
                                billing.StaffId = customer.StaffId;
                                billing.StaffCode = customer.StaffCode;
                            }
                            //新規得意先の場合
                            else
                            {
                                var staff = ExistStaff.First(x => x.Code == staffSetting.FixedValue);
                                billing.StaffId = staff.Id;
                                billing.StaffCode = staff.Code;
                            }
                        }
                        #endregion

                        #region 回収区分設定
                        if (collectCategorySetting != null && collectCategorySetting.ImportDivision == 0)
                        {
                            //既存の得意先の場合
                            if (billing.AutoCreationCustomerFlag == 0 || (billing.AutoCreationCustomerFlag == 1 && customer != null))
                            {
                                billing.CollectCategoryId = customer.CollectCategoryId;
                                billing.CollectCategoryCode = customer.CollectCategoryCode;
                            }
                            //新規得意先の場合
                            else
                            {
                                var category = ExistCollectCategory.First(x => x.Code == collectCategorySetting.FixedValue);
                                billing.CollectCategoryId = category.Id;
                                billing.CollectCategoryCode = category.Code;
                            }
                        }
                        #endregion

                        #region 歩引利用設定
                        if (useDiscountSetting != null && useDiscountSetting.ImportDivision == 0)
                        {
                            if (UseDiscount)
                            {
                                var category = ExistBillingCategory.First(x => x.Code == billing.BillingCategoryCode);
                                billing.UseDiscount = category.UseDiscount;
                            }
                        }
                        #endregion

                        #region 税区
                        if (taxClass != null && taxClass.ImportDivision == 0)
                        {
                            var category = ExistBillingCategory.First(x => x.Code == billing.BillingCategoryCode);
                            billing.TaxClassId = category.TaxClassId ?? 0;
                            billing.TaxClassIdForPrint = billing.TaxClassId.ToString();
                        }
                        #endregion

                        #region 日付 大小比較 請求日/請求締日/入金予定日
                        // 請求締日
                        if (billing.ClosingAt < billing.BilledAt)
                        {
                            billing.ClosingAtForPrint += "*";
                            errorHandler(billing.LineNo, closingAtSetting.FieldIndex ?? 0, closingAtSetting.FieldName, "請求締日は請求日より大きい値を入力してください。", null);
                            ImpossibleData.Add(billing);
                            continue;
                        }

                        // 入金予定日
                        if (billing.DueAt < billing.ClosingAt)
                        {
                            billing.DueAtForPrint += "*";
                            errorHandler(billing.LineNo, dueAtSetting.FieldIndex ?? 0, dueAtSetting.FieldName, "入金予定日は請求締日より大きい値を入力してください。", null);
                            ImpossibleData.Add(billing);
                            continue;
                        }

                        #endregion
                    }
                    PossibleData = PossibleData.Except(ImpossibleData).ToList();
                }
                #endregion

                ReadCount = (totalRecordCount - (skipDataCount + (ImporterSetting.StartLineCount - 1)));
                ValidCount = PossibleData.Count;
                InvalidCount = ImpossibleData.Count;
                DoOverWrite = ImporterSettingDetail.Any(x => x.DoOverwrite == 1);

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
                    details.AddRange(PossibleData.Select(x => new ImportDataDetail {
                        ObjectType      = 0,
                        RecordItem      = Serialize(x),
                    }).ToArray());
                    details.AddRange(ImpossibleData.Select(x => new ImportDataDetail {
                        ObjectType      = 1,
                        RecordItem      = Serialize(x),
                    }).ToArray());
                    data.Details = details.ToArray();
                    ImportData = await SaveImportDataAsync(data);
                }

                valid = true;
            }
            catch (Exception ex)
            {
                LogError?.Invoke(ex);
            }
            return valid;
        }


        private async Task LoadInitialDataAsync()
        {
            ImporterSetting = await GetImporterSettingAsync(ImporterSettingId);
            ImporterSettingDetail = await GetImporterSettingDetailAsync(ImporterSettingId);
            ExistCurrencies = await GetCurrencyAsync(CompanyId);
            legalPersonalities = await GetJuridicalParsonalitiesAsync(CompanyId);
            RoundingType roundingType;
            var roundingModeValue = await GetGeneralSettingValueAsync(CompanyId, "取込時端数処理");
            Enum.TryParse(roundingModeValue, out roundingType);
            this.roundingType = roundingType;
            taxClasses = await GetTaxClassAsync();
            await LoadHolidayCalendarAsync();
            await LoadColumnNameSettingAsync();
            await GetUseControlInputNoteAsync();

        }

        private string ConvertValidKana(string value)
        {
            return EbDataHelper.ConvertToPayerName(value, legalPersonalities);
        }

        private async Task SolveCodeToIdAsync()
        {
            var customerCodes = new HashSet<string>();
            var departmentCodes = new HashSet<string>();
            var accountTitleCodes = new HashSet<string>();
            Action<string, HashSet<string>> addCode = (code, hash) => {
                if (!IsValidCode(code) || hash.Contains(code)) return;
                hash.Add(code);
            };
            foreach (var x in PossibleData)
            {
                addCode(x.CustomerCode, customerCodes);
                addCode(x.DepartmentCode, departmentCodes);
                addCode(x.DebitAccountTitleCode, accountTitleCodes);
            }

            var detailCustomer = GetSettingDetail(Fields.CustomerCode);
            var detailDepartment = GetSettingDetail(Fields.DepartmentCode);
            var detailStaff = GetSettingDetail(Fields.StaffCode);
            var detailAccount = GetSettingDetail(Fields.DebitAccountTitleCode);
            var detailBillingCategory = GetSettingDetail(Fields.BillingCategoryCode);
            var detailCollectCategory = GetSettingDetail(Fields.CollectCategoryCode);
            var detailContract = GetSettingDetail(Fields.ContractNumber);
            var detailCurrency = GetSettingDetail(Fields.CurrencyCode);

            var tasks = new List<Task>();
            tasks.Add(LoadCustomerByCodeAsync(customerCodes.ToArray()));
            tasks.Add(LoadDepartmentByCodeAsync(departmentCodes.ToArray()));
            tasks.Add(LoadStaffByCodesAsync(null));
            tasks.Add(LoadAccountTitleByCodesAsync(accountTitleCodes.ToArray()));
            tasks.Add(LoadBillingCategoryByCodeAsync(null));
            tasks.Add(LoadCollectCategoryByCodeAsync(null));
            await Task.WhenAll(tasks);
            var requireContractValidation = detailContract?.FieldIndex.HasValue ?? false;
            if (requireContractValidation)
            {
                var ids = ExistCustomer.Select(x => x.Id).ToArray();
                ExistBillingDivicionContract = await GetBillingDivisionContractByCustomerIdsAsync(ids);
            }

            var dicCustomer = ExistCustomer.ToDictionary(x => x.Code, x => x.Id);
            var dicDepartment = ExistDepartment.ToDictionary(x => x.Code, x => x.Id);
            var dicStaff = ExistStaff.ToDictionary(x => x.Code, x => x.Id);
            var dicAccount = ExistAccountTitle.ToDictionary(x => x.Code, x => x.Id);
            var dicBillingCategory = GetCategoryDictionary(ExistBillingCategory, detailBillingCategory.AttributeDivision == 2);
            var dicCollectCategory = GetCategoryDictionary(ExistCollectCategory, detailCollectCategory.AttributeDivision == 2);
            var dicCurrency = ExistCurrencies.ToDictionary(x => x.Code, x => x.Id);
            Dictionary<int, Category > dicCategoryForContract = null;        // key : category.Id
            Dictionary<int, BillingDivisionContract[]> dicContract = null;    // key : contract.CustomerId
            if (requireContractValidation)
            {
                dicCategoryForContract = ExistBillingCategory.ToDictionary(x => x.Id, x => x);
                dicContract = ExistBillingDivicionContract.GroupBy(x => x.CustomerId).ToDictionary(x => x.Key, x => x.ToArray());
            }

            var newCustomerCodes = new HashSet<string>();
            foreach (var x in PossibleData)
            {
                var index = PossibleData.IndexOf(x);
                ValidateCustomerCode(x, index, dicCustomer, detailCustomer, newCustomerCodes);
                ValidateDepartmentCode(x, index, dicDepartment, detailDepartment);
                ValidateStaffCode(x, index, dicStaff, detailStaff);
                ValidateAccountTitleCode(x, index, dicAccount, detailAccount);
                ValidateBillingCategoryCode(x, index, dicBillingCategory, detailBillingCategory);
                ValidateCollectCategoryCode(x, index, dicCollectCategory, detailCollectCategory);
                ValidateCurrencyCode(x, index, dicCurrency, detailCurrency);
                ValidateContractNumber(x, index, detailContract, dicCategoryForContract, dicContract);
            }
            NewCustomerCreationCount = newCustomerCodes.Count;
        }
        private Dictionary<string, int> GetCategoryDictionary(List<Category> categories, bool useExternalCode)
            => categories.Where(x => ! useExternalCode || !string.IsNullOrEmpty(x.ExternalCode))
            .GroupBy(x => useExternalCode ? x.ExternalCode : x.Code)
            .ToDictionary(g => g.Key, g => g.First().Id);

        private const string MsgErrRequireInput = "空白のためインポートできません。";
        private const string MsgErrNotExist = "存在しないため、インポートできません。";
        private const string MsgErrAlreadyAssigned = "既に割り当てられている契約番号のため、インポートできません。";
        private const string MsgErrBillingCategoryInvalidTaxClass = "請求金額（税込）と外税課税となっている請求区分の組み合わせでは、インポートできません。";
        private const string MsgErrInvalidTaxClass = "請求金額（税込）と外税課税の組み合わせでは、インポートできません。";
        private WorkingReport GetWorkingReport(ImporterSettingDetail detail, int lineNo, string value, string message)
            => new WorkingReport {
                LineNo = lineNo,
                FieldNo = detail.FieldIndex ?? 0,
                FieldName = detail.FieldName,
                Value = value,
                Message = message,
            };

        private bool IsValidCode(string code)
        {
            return !(string.IsNullOrEmpty(code) || code.EndsWith("*"));
        }

        private bool IsValidTaxRate(string value, int attributeDivision, out decimal taxRate)
        {
            var targetValue = value.Normalize(NormalizationForm.FormKC);
            decimal decValue = 0M;
            taxRate = 0M;

            switch (attributeDivision)
            {
                case 1:
                    if (!decimal.TryParse(targetValue, out decValue)) return false;
                    taxRate = (decimal)Amount.Calc(RoundingType.Floor, decValue, 4);
                    break;
                case 2:
                    if (!decimal.TryParse(targetValue.Replace("%", ""), out decValue)) return false;
                    decValue = (decimal)Amount.Calc(RoundingType.Floor, decValue, 2);
                    taxRate = decValue / 100;
                    break;
            }

            return true;
        }

        private bool IsValidNoteLength(string value, bool useControlInputNote = false)
        {
            if (useControlInputNote)
            {
                Encoding encoding = Encoding.GetEncoding(932);
                return encoding.GetByteCount(value) > NoteInputByteCount;
            }
            else
                return value.Length > 100;
        }

        private string GetSubStringNote(string value, bool useControlInputNote = false)
        {
            var result = string.Empty;
            if (useControlInputNote)
            {
                Encoding encoding = Encoding.GetEncoding(932);
                byte[] bytes = encoding.GetBytes(value);
                result = encoding.GetString(bytes, 0, NoteInputByteCount);
            }
            else
                result = value.Substring(0, 100);

            return result;
        }

        private void ValidateCustomerCode(BillingImport x, int index, Dictionary<string, int> dic, ImporterSettingDetail detail, HashSet<string> newCustomerCodes)
        {
            var createCustomer = ImporterSetting.AutoCreationCustomer == 1;
            if (!IsValidCode(x.CustomerCode)) return;
            if (dic.ContainsKey(x.CustomerCode))
            {
                x.CustomerId = dic[x.CustomerCode];
                BillingImportDuplication[index].CustomerId = x.CustomerId;
                if (createCustomer) x.AutoCreationCustomerFlag = 0;
            }
            else if (createCustomer)
            {
                if (!newCustomerCodes.Contains(x.CustomerCode)) newCustomerCodes.Add(x.CustomerCode);
            }
            else
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, x.CustomerCode, MsgErrNotExist));
                x.CustomerCode += "*";
            }
        }
        private void ValidateDepartmentCode(BillingImport x, int index, Dictionary<string, int> dic, ImporterSettingDetail detail)
        {
            if (detail.ImportDivision == 0) return;
            if (!IsValidCode(x.DepartmentCode)) return;
            if (dic.ContainsKey(x.DepartmentCode))
            {
                x.DepartmentId = dic[x.DepartmentCode];
                BillingImportDuplication[index].DepartmentId = x.DepartmentId;
            }
            else
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, x.DepartmentCode, MsgErrNotExist));
                x.DepartmentCode += "*";
            }
        }
        private void ValidateStaffCode(BillingImport x, int index, Dictionary<string, int> dic, ImporterSettingDetail detail)
        {
            if (detail.ImportDivision == 0) return;
            if (!IsValidCode(x.StaffCode)) return;
            if (dic.ContainsKey(x.StaffCode))
            {
                x.StaffId = dic[x.StaffCode];
                BillingImportDuplication[index].StaffId = x.StaffId;
            }
            else
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, x.StaffCode, MsgErrNotExist));
                x.StaffCode += "*";
            }
        }
        private void ValidateAccountTitleCode(BillingImport x, int index, Dictionary<string, int> dic, ImporterSettingDetail detail)
        {
            if (detail.ImportDivision == 0) return;
            if (!IsValidCode(x.DebitAccountTitleCode)) return;
            if (dic.ContainsKey(x.DebitAccountTitleCode))
            {
                x.DebitAccountTitleId = dic[x.DebitAccountTitleCode];
                BillingImportDuplication[index].DebitAccountTitleId = x.DebitAccountTitleId;
            }
            else
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, x.DebitAccountTitleCode, MsgErrNotExist));
                x.DebitAccountTitleCode += "*";
            }
        }
        private void ValidateBillingCategoryCode(BillingImport x, int index, Dictionary<string, int> dic, ImporterSettingDetail detail)
        {
            var code = x.BillingCategoryCode;
            if (!IsValidCode(code)) return;
            if (dic.ContainsKey(code))
            {
                x.BillingCategoryId = dic[code];
                if(detail.AttributeDivision == 2)
                    x.BillingCategoryCode = ExistBillingCategory.First(c => c.Id == dic[code]).Code;
                BillingImportDuplication[index].BillingCategoryId = x.BillingCategoryId;
            }
            else
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, code, MsgErrNotExist));
                x.BillingCategoryCode += "*";
            }
        }
        private void ValidateCollectCategoryCode(BillingImport x, int index, Dictionary<string, int> dic, ImporterSettingDetail detail)
        {
            if (detail.ImportDivision == 0) return;
            var code = x.CollectCategoryCode;
            if (!IsValidCode(code)) return;
            if (dic.ContainsKey(code))
            {
                x.CollectCategoryId = dic[code];
                if (detail.AttributeDivision == 2)
                    x.CollectCategoryCode = ExistCollectCategory.First(c => c.Id == dic[code]).Code;
                BillingImportDuplication[index].CollectCategoryId = x.CollectCategoryId;
            }
            else
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, code, MsgErrNotExist));
                x.CollectCategoryCode += "*";
            }
        }
        private void ValidateCurrencyCode(BillingImport x, int index, Dictionary<string, int> dic, ImporterSettingDetail detail)
        {
            if (detail == null) return;
            if (!IsValidCode(x.CurrencyCode)) return;
            if (dic.ContainsKey(x.CurrencyCode))
            {
                x.CurrencyId = dic[x.CurrencyCode];
                BillingImportDuplication[index].CurrencyId = x.CurrencyId;
            }
            else
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, x.CurrencyCode, MsgErrNotExist));
                x.CurrencyCode += "*";
            }
        }
        private void ValidateContractNumber(BillingImport x, int index, ImporterSettingDetail detail,
            Dictionary<int, Category> dicBillingCategory,
            Dictionary<int, BillingDivisionContract[]> dicContract)
        {
            if (detail == null) return;
            if (!dicBillingCategory.ContainsKey(x.BillingCategoryId)) return;
            if (dicBillingCategory[x.BillingCategoryId].UseLongTermAdvanceReceived != 1) return;

            if (string.IsNullOrEmpty(x.ContractNumber))
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, string.Empty, MsgErrRequireInput));
                x.ContractNumber += "*";
                return;
            }

            var contract = dicContract.ContainsKey(x.CustomerId)
                ? dicContract[x.CustomerId].FirstOrDefault(y => y.ContractNumber == x.ContractNumber) : null;
            if (contract == null && detail.AttributeDivision == 2)
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, x.ContractNumber, MsgErrNotExist));
                x.ContractNumber += "*";
                return;
            }
            if (contract?.BillingId.HasValue ?? false)
            {
                ErrorReport.Add(GetWorkingReport(detail, x.LineNo, x.ContractNumber, MsgErrAlreadyAssigned));
                x.ContractNumber += "*";
                return;
            }
        }

        public async Task<bool> ImportAsync()
        {
            var result = false;
            try
            {
                if (LoadImportDataAsync != null)
                {
                    ImportData = await LoadImportDataAsync();
                    PossibleData.AddRange(ImportData.Details
                        .Where(x => x.ObjectType == 0)
                        .Select(x => Deserialize(x.RecordItem)).ToArray());
                }

                var webResult = await ImportAsync(PossibleData.ToArray());
                SaveAmount = PossibleData.Sum(x => x.BillingAmount);
                SaveCount = PossibleData.Count;
                result = ( webResult?.ProcessResult.Result ?? false);
            }
            catch (Exception ex)
            {
                LogError?.Invoke(ex);
            }
            return result;
        }

        /// <summary>
        /// エラーログの出力
        /// ファイルI/O系の詳細なメッセージ表示が必要な場合、
        /// 例外を呼び出し側で細かく catch して対応すること
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool WriteErrorLog(string path)
            => base.WriteErrorLog(path, "請求データ");

        public List<BillingImport> GetReportSource(bool isPossible)
        {
            var source = new List<BillingImport>(isPossible ? PossibleData : ImpossibleData);
            //var departmentIds = source.Where(x => string.IsNullOrEmpty(x.DepartmentCode))
            //    .Select(x => x.DepartmentId).Distinct().ToArray();
            //var departments = Util.GetDepartmentByIdsAsync(Login, departmentIds).Result;
            foreach (var department in ExistDepartment) // todo: confirm
                foreach (var detail in source.Where(x => string.IsNullOrEmpty(x.DepartmentCode) && x.DepartmentId == department.Id))
                {
                    detail.DepartmentCode = department.Code;
                }
            var fixedValues = ImporterSettingDetail
                .Where(x => !string.IsNullOrEmpty(x.FixedValue)
                    && (x.Sequence == (int)Fields.StaffCode
                        || x.Sequence == (int)Fields.CollectCategoryCode));
            if (fixedValues.Any())
            {
                foreach (var detail in source)
                {
                    foreach (var fixedValue in fixedValues)
                    {
                        if (fixedValue.Sequence == (int)Fields.StaffCode)
                            detail.StaffCode = fixedValue.FixedValue;
                        if (fixedValue.Sequence == (int)Fields.CollectCategoryCode)
                            detail.CollectCategoryCode = fixedValue.FixedValue;
                    }
                }
            }
            return source;
        }

        public List<Customer> GetNewCustomers()
        {
            var details = ImporterSettingDetail;
            var dic = ExistCollectCategory.ToDictionary(x => x.Code);
            return PossibleData.Concat(ImpossibleData)
                .ConvertToCustomers(details, LoginUserId, dic).ToList();
        }

        public List<BillingImport> GetBillingImportListForNewCustomers()
        {
            return PossibleData.Concat(ImpossibleData)
                .ToLookupCustomerCode().ToList();
        }

        #region call get db datas

        private async Task LoadHolidayCalendarAsync()
            => Holidays = await GetHolidayCalendarsAsync(CompanyId);

        private async Task LoadColumnNameSettingAsync()
            => await base.LoadColumnNameSettingAsync(nameof(Billing));

        private async Task<int[]> GetBillingImportDuplicationAsync(
            BillingImportDuplicationWithCode[] billings,
            ImporterSettingDetail[] details)
            =>  await BillingImportDuplicationCheckAsync(CompanyId, billings, details)
            ?? new int[] { };

        private async Task<BillingImportResult> ImportAsync(BillingImport[] source)
            => await ImportInnerAsync(CompanyId, LoginUserId, ImporterSettingId, source);

        private async Task LoadCustomerByCodeAsync(string[] codes)
            => ExistCustomer = await GetCustomerByCodesAsync(CompanyId, codes);

        private async Task LoadDepartmentByCodeAsync(string[] codes)
            => ExistDepartment = await GetDepartmentByCodesAsync(CompanyId, codes);

        private async Task LoadAccountTitleByCodesAsync(string[] codes)
            => ExistAccountTitle = await GetAccountTitleByCodesAsync(CompanyId, codes);

        private async Task LoadStaffByCodesAsync(string[] codes)
            => ExistStaff = await GetStaffByCodesAsync(CompanyId, codes);

        private async Task LoadBillingCategoryByCodeAsync(string[] codes)
            => ExistBillingCategory = await GetCategoriesByCodesAsync(CompanyId, CategoryType.Billing, codes);

        private async Task LoadCollectCategoryByCodeAsync(string[] codes)
            => ExistCollectCategory = await GetCategoriesByCodesAsync(CompanyId, CategoryType.Collect, codes);

        private async Task GetUseControlInputNoteAsync()
            => UseControlInputNote = await GetIsEnableToEditNoteAsync(CompanyId);

        #endregion
    }
}
