using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Importer.Receipt;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;


namespace Rac.VOne.Web.Models.Importers
{
    public class ReceiptImporterBase :
        ImporterBase<ReceiptInput>,
        IImporter<ReceiptInput>
    {
        public CsvParser CsvParser { get; set; } = new CsvParser();

        public Func<int, Task<ImporterSetting>> GetImporterSettingAsync { get; set; }
        public Func<int, Task<List<ImporterSettingDetail>>> GetImporterSettingDetailByIdAsync { get; set; }
        public Func<int, string, Task<string>> GetGeneralSettingValueAsync { get; set; }
        public Func<int, ReceiptImportDuplication[], ImporterSettingDetail[], Task<int[]>> ReceiptImportDuplicationCheckAsync { get; set; }
        public Func<ReceiptInput[], Task<ReceiptInputsResult>> SaveInnerAsync { get; set; }
        public Func<int, string[], Task<List<Currency>>> GetCurrenciesAsync { get; set; }
        public Func<int, int, string[], Task<List<Category>>> GetCategoriesByCodesAsync { get; set; }
        public Func<int, string[], Task<List<Section>>> GetSectionByCodesAsync { get; set; }
        public Func<int, string[], Task<List<Customer>>> GetCustomerByCodesAsync { get; set; }
        public Func<int, Task<IEnumerable<string>>> GetLegalPersonaritiesAsync { get; set; }
        public Func<int, Task<CollationSetting>> GetCollationSettingAsync { get; set; }

        public Func<ImportData, Task<ImportData>> SaveImportDataAsync { get; set; }
        public Func<ReceiptInput, byte[]> Serialize { get; set; }
        public Func<byte[], ReceiptInput> Deserialize { get; set; }
        public Func<Task<ImportData>> LoadImportDataAsync { get; set; }

        public ImportData ImportData { get; set; }


        public ReceiptImporterBase(ApplicationControl applicationControl)
            : base(applicationControl) { }

        private List<ReceiptInput> saveReciptInput { get; set; } = new List<ReceiptInput>();
        private ImporterSettingDetail GetSettingDetail(Fields field,
            IEnumerable<ImporterSettingDetail> source = null)
            => (source ?? ImporterSettingDetail)?.FirstOrDefault(x => x.Sequence == (int)field);

        private List<Customer> ExistCustomer { get; set; }
        private List<Section> ExistSection { get; set; }
        private List<Category> ExistReceiptCategory { get; set; }
        private List<Currency> ExistCurrency { get; set; }
        private CollationSetting CollationSetting { get; set; }
        private IEnumerable<string> legalPersonalities { get; set; }

        private const decimal MaxAmount = 99999999999M;

        private async Task LoadColumnNameSetting()
            => await base.LoadColumnNameSettingAsync(nameof(Receipt));

        private DateTime? formatRecordedAt(string value) => CreateFormatter(GetSettingDetail(Fields.RecordedAt))(value);
        private DateTime? formatDueAt(string value) => CreateFormatter(GetSettingDetail(Fields.DueAt))(value);
        private DateTime? formatBillDrawAt(string value) => CreateFormatter(GetSettingDetail(Fields.BillDrawAt))(value);

        /// <summary>入金フリーインポーター 読込処理
        /// CSVファイルの取込処理で実施すべき処理
        /// ファイル読込
        /// CSVファイルのParse
        /// 検証処理
        ///  field 単品 の検証処理
        ///   未入力
        ///   文字種
        ///   桁数
        ///  field 組合せの検証処理
        ///  検証処理の汎化されたメッセージング
        /// モデルへの詰め替え
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ReadCsvAsync()
        {
            var valid = false;
            PossibleData.Clear();
            ImpossibleData.Clear();
            ErrorReport.Clear();
            saveReciptInput.Clear();
            var duplications = new List<ReceiptImportDuplication>();


            ImporterSetting = await GetImporterSettingAsync(ImporterSettingId);
            ImporterSettingDetail = await GetImporterSettingDetailByIdAsync(ImporterSettingId);

            RoundingType roundingType;
            var roundingModeValue = await GetGeneralSettingValueAsync(CompanyId, "取込時端数処理");
            Enum.TryParse(roundingModeValue, out roundingType);

            var records = CsvParser.Parse(FilePath).ToList();
            if (records.Count < ImporterSetting.StartLineCount)
                return valid;

            await LoadMasterDataAsync(ImporterSettingDetail);
            var dicCustomer = ExistCustomer.ToDictionary(x => x.Code, x => x);

            var settingCustomerCode = GetSettingDetail(Fields.CustomerCode);
            var settingCategoryCode = GetSettingDetail(Fields.ReceiptCategoryCode);
            var settingSectionCode  = GetSettingDetail(Fields.SectionCode);
            var settingDueAt        = GetSettingDetail(Fields.DueAt);
            var settingCurrencyCode = GetSettingDetail(Fields.CurrencyCode);
            var settingBillBankCode = GetSettingDetail(Fields.BillBankCode);
            var settingBillBranchCode = GetSettingDetail(Fields.BillBranchCode);

            var skipCount = 0;
            var decimalScale = 0;
            var currencyDisplayFormat = "###,###,###,##0";

            if (UseForeignCurrency && settingCurrencyCode.ImportDivision == 0)
            {
                decimalScale = (ExistCurrency.FirstOrDefault(x => x.Code == settingCurrencyCode.FixedValue)?.Precision) ?? 0;
                currencyDisplayFormat = GetCurrencyDisplayFormat(decimalScale);
            }

            foreach (var fields in records)
            {
                var values = fields.ToArray();
                if (values == null) continue;
                var index = records.IndexOf(fields);
                var i = index + 1;
                if (ImporterSetting.IgnoreLastLine == 1
                    && i == records.Count) continue;
                if (i < ImporterSetting.StartLineCount) continue;

                var billBankcode = "";
                var billBranchcode = "";
                var errorflg = false;
                var isAnyUnique = false;
                var skipflg = false;
                var printReceiptImporter = new ReceiptInput();
                var receipt = new ReceiptInput();

                var duplicationCheck = new ReceiptImportDuplication {
                    RowNumber = i,
                };


                #region 通貨コード 検証処理
                if (UseForeignCurrency && settingCurrencyCode.ImportDivision == 1)
                {
                    var fieldIndex = settingCurrencyCode.FieldIndex.Value - 1;
                    var value = fieldIndex > -1 ? values[fieldIndex].Trim() : string.Empty;
                    value = EbDataHelper.ConvertToUpperCase(value);
                    receipt.CurrencyCode = value;
                    printReceiptImporter.CurrencyCode = value;
                    var currency = ExistCurrency?.FirstOrDefault(x => x.Code == value);
                    if (fieldIndex >= values.Length)
                    {
                        printReceiptImporter.CurrencyCode = "*";
                        AddErrorLog(i, fieldIndex, settingCurrencyCode.FieldName, ErrorType.InvalidFieldIndex);
                        errorflg = true;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(value))
                    {
                        printReceiptImporter.CurrencyCode = value + "*";
                        AddErrorLog(i, fieldIndex, settingCurrencyCode.FieldName, ErrorType.InputRequired);
                        errorflg = true;
                    }
                    else if (currency == null)
                    {
                        printReceiptImporter.CurrencyCode = value + "*";
                        AddErrorLog(i, fieldIndex, settingCurrencyCode.FieldName, ErrorType.NotExistMaster);
                        errorflg = true;
                    }
                    else
                    {
                        receipt.CurrencyId = currency.Id;
                        decimalScale = currency.Precision;
                        currencyDisplayFormat = GetCurrencyDisplayFormat(decimalScale);
                        if (settingCurrencyCode.IsUnique == 1) duplicationCheck.CurrencyId = receipt.CurrencyId;
                    }
                }
                else
                {
                    var code = (!UseForeignCurrency) ? DefaultCurrencyCode : settingCurrencyCode.FixedValue;
                    receipt.CurrencyCode = code;
                    printReceiptImporter.CurrencyCode = code;
                    var currency = ExistCurrency.Find(x => x.Code == receipt.CurrencyCode);
                    receipt.CurrencyId = currency.Id;
                }
                #endregion

                if (settingBillBankCode.ImportDivision == 1 && settingBillBankCode.FieldIndex.Value <= values.Length)
                    billBankcode = values[settingBillBankCode.FieldIndex.Value - 1];

                if (settingBillBranchCode.ImportDivision == 1 && settingBillBankCode.FieldIndex.Value <= values.Length)
                    billBranchcode = values[settingBillBranchCode.FieldIndex.Value - 1];

                Customer customer = null;
                Category receiptCategory = null;
                foreach (var detail in ImporterSettingDetail.OrderBy(x => GetFirstPriority(x)).ThenBy(f => f.FieldIndex))
                {
                    if (detail.IsUnique == 1)
                        isAnyUnique = true;

                    var settingIndex = detail.FieldIndex.Value;
                    var fieldIndex = settingIndex - 1;
                    // 取込有 本来なら detail.ImportDivision == 1 で判別したかった
                    var doImport = fieldIndex > -1;
                    var fieldName = GetFieldName(detail);
                    if (doImport && !IsExist(fieldIndex, values.Length))
                    {
                        AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidFieldIndex);
                        errorflg = true;
                        break;
                    }
                    var value = fieldIndex > -1 ? values[fieldIndex] : string.Empty;

                    #region 得意先コード 検証処理（取込有）
                    if (doImport && detail.Sequence == (int)Fields.CustomerCode)
                    {
                        //空白チェック
                        if (string.IsNullOrEmpty(value))
                        {
                            printReceiptImporter.CustomerCode = "*";
                            AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                            errorflg = true;
                            continue;
                        }

                        var code = value;
                        if (ApplicationControl?.CustomerCodeType == 0)
                            code = code.PadLeft(ApplicationControl?.CustomerCodeLength ?? 0, '0');
                        else if (ApplicationControl?.CustomerCodeType == 1)
                            code = EbDataHelper.ConvertToUpperCase(code);
                        else if (ApplicationControl?.CustomerCodeType == 2)
                            code = EbDataHelper.ConvertToHankakuKatakana(code).ToUpper();

                        //マスター存在チェックのため設定
                        customer = TryGetCustomer(dicCustomer, code);
                        if (customer == null)
                        {
                            printReceiptImporter.CustomerCode = value + "*";
                            AddErrorLog(i, settingIndex, fieldName, ErrorType.NotExistMaster);
                            errorflg = true;
                            continue;
                        }

                        printReceiptImporter.CustomerCode = code;
                        receipt.CustomerCode    = code;
                        receipt.CustomerId      = customer.Id;
                        receipt.PayerName       = customer.Kana;
                        receipt.PayerNameRaw    = customer.Kana;
                        if (detail.IsUnique == 1) duplicationCheck.CustomerId = customer.Id;

                    }
                    #endregion

                    #region 入金日 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.RecordedAt)
                    {
                        printReceiptImporter.RecordAtForPrint = value;
                        if (ValidateEmpty(value) == false)
                        {
                            printReceiptImporter.RecordAtForPrint = value + "*";
                            AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                            errorflg = true;
                            continue;
                        }
                        var date = formatRecordedAt(value);
                        if (!date.HasValue)
                        {
                            printReceiptImporter.RecordAtForPrint = value + "*";
                            AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidFormat);
                            errorflg = true;
                            continue;
                        }
                        receipt.RecordedAt = date.Value;
                        printReceiptImporter.RecordAtForPrint = receipt.RecordedAt.ToString("yyyy/MM/dd");
                        if (detail.IsUnique == 1) duplicationCheck.RecordedAt = receipt.RecordedAt;
                    }
                    #endregion

                    #region 入金区分 検証処理（取込有）
                    else if (detail.Sequence == (int)Fields.ReceiptCategoryCode)
                    {
                        if (doImport)
                        {
                            printReceiptImporter.ReceiptCategoryCode = value;
                            receipt.ReceiptCategoryCode = value;

                            if (!ValidateEmpty(receipt.ReceiptCategoryCode))
                            {
                                printReceiptImporter.ReceiptCategoryCode = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                                errorflg = true;
                                continue;
                            }

                            if (detail.AttributeDivision == 1)
                                receipt.ReceiptCategoryCode = receipt.ReceiptCategoryCode.PadLeft(2, '0');

                            printReceiptImporter.ReceiptCategoryCode = receipt.ReceiptCategoryCode;

                            Func<Category, string> keySelector = x => x.Code;
                            if (detail.AttributeDivision == 2)
                                keySelector = x => x.ExternalCode;

                            //マスター存在チェックのため設定
                            receiptCategory = ExistReceiptCategory?.FirstOrDefault(x => keySelector(x) == receipt.ReceiptCategoryCode);

                            if (receiptCategory == null)
                            {
                                printReceiptImporter.ReceiptCategoryCode = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.NotExistMaster);
                                errorflg = true;
                                continue;
                            }
                            //エラーなしの場合設定
                            receipt.ReceiptCategoryId = receiptCategory.Id;
                            if (detail.AttributeDivision == 2)
                                printReceiptImporter.ReceiptCategoryCode = receiptCategory.Code;

                            if (receiptCategory.UseLimitDate == 1)
                            {
                                if (settingCustomerCode.ImportDivision == 0)
                                {
                                    printReceiptImporter.ReceiptCategoryCode = value + "*";
                                    ErrorReport.Add(new WorkingReport
                                    {
                                        LineNo = i,
                                        FieldNo = fieldIndex,
                                        FieldName = fieldName,
                                        Value = printReceiptImporter.ReceiptCategoryCode,
                                        Message = $"得意先コードを取込しない場合、期日入力を行う入金区分は選択できません。",
                                    });
                                    errorflg = true;
                                    continue;
                                }
                                if (settingDueAt.ImportDivision == 0)
                                {
                                    printReceiptImporter.ReceiptCategoryCode = value + "*";
                                    ErrorReport.Add(new WorkingReport
                                    {
                                        LineNo = i,
                                        FieldNo = fieldIndex,
                                        FieldName = fieldName,
                                        Value = printReceiptImporter.ReceiptCategoryCode,
                                        Message = $"期日入力を行う入金区分で期日が指定されていません。",
                                    });
                                    errorflg = true;
                                    continue;
                                }
                            }
                            if (detail.IsUnique == 1) duplicationCheck.ReceiptCategoryId = receipt.ReceiptCategoryId;

                            continue;

                        }
                        else if (detail.FixedValue != null)
                        {
                            var code = detail.FixedValue;
                            receipt.ReceiptCategoryCode = code;
                            printReceiptImporter.ReceiptCategoryCode = code;
                            receiptCategory = ExistReceiptCategory?.FirstOrDefault(x => x.Code == code);
                            if (receiptCategory != null)
                            {
                                receipt.ReceiptCategoryId = receiptCategory.Id;
                                if (detail.IsUnique == 1) duplicationCheck.ReceiptCategoryId = receipt.ReceiptCategoryId;
                            }
                        }
                    }
                    #endregion

                    #region 入金額 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.ReceiptAmount)
                    {
                        // todo: refactor
                        if (ValidateEmpty(value) == false)
                        {
                            printReceiptImporter.ReceiptAmountForPrint = value + "*";
                            AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                            errorflg = true;
                            continue;
                        }

                        decimal dec;
                        if (!decimal.TryParse(value, out dec))
                        {
                            printReceiptImporter.ReceiptAmountForPrint = value + "*";
                            AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidFormat);
                            errorflg = true;
                            continue;
                        }

                        var strValue = dec.ToString();  //前ゼロをとる
                        string[] strDueAmount = null;
                        if (strValue.Contains('.'))
                        {
                            strDueAmount = strValue.Split('.');
                            if (decimal.Parse(strDueAmount[0]) < (MaxAmount * -1) || MaxAmount < decimal.Parse(strDueAmount[0]))
                            {
                                printReceiptImporter.ReceiptAmountForPrint = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1] + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.length, 11);
                                errorflg = true;
                                continue;
                            }
                            receipt.ReceiptAmount = decimal.Parse(strDueAmount[0]);
                            receipt.RemainAmount = receipt.ReceiptAmount;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.ReceiptAmount = receipt.ReceiptAmount;
                        }
                        else
                        {
                            strDueAmount = new string[] { strValue, "0" };
                            if (decimal.Parse(strDueAmount[0]) < (MaxAmount * -1) || MaxAmount < decimal.Parse(strDueAmount[0]))
                            {
                                printReceiptImporter.ReceiptAmountForPrint = dec.ToString("#,##0") + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.length, 11);
                                errorflg = true;
                                continue;
                            }

                            if (strDueAmount[0] == "0")
                            {
                                if (detail.ImportDivision == 1)
                                {
                                    //入力値チェック 0 の場合、エラーとする
                                    printReceiptImporter.ReceiptAmountForPrint = dec.ToString() + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.ForbitZeroValue);
                                    errorflg = true;
                                    continue;
                                }
                                else
                                {
                                    skipflg = true;
                                    skipCount++;
                                    break;
                                }
                            }
                            receipt.ReceiptAmount = dec;
                            printReceiptImporter.ReceiptAmountForPrint = receipt.ReceiptAmount.ToString(currencyDisplayFormat);
                            receipt.RemainAmount = receipt.ReceiptAmount;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.ReceiptAmount = receipt.ReceiptAmount;
                            continue;
                        }

                        if (Convert.ToDecimal(strDueAmount[1]) != 0M)
                        {
                            if (strDueAmount[1].TrimEnd('0').Length > decimalScale && roundingType == RoundingType.Error)
                            {
                                printReceiptImporter.ReceiptAmountForPrint = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1] + "*";
                                ErrorReport.Add(new WorkingReport
                                {
                                    LineNo = i,
                                    FieldNo = settingIndex,
                                    FieldName = fieldName,
                                    Value = value,
                                    Message = $"入金額は小数点" + decimalScale + "桁までです。",
                                });
                                errorflg = true;
                                continue;
                            }

                            receipt.ReceiptAmount = roundingType.Calc(dec, decimalScale).Value;
                            strDueAmount = receipt.ReceiptAmount.ToString().Split('.');
                            receipt.RemainAmount = receipt.ReceiptAmount;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.ReceiptAmount = receipt.ReceiptAmount;

                            //入力値チェック 0 の場合、エラーとする
                            if (receipt.ReceiptAmount == 0)
                            {
                                printReceiptImporter.ReceiptAmountForPrint = dec.ToString() + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.ForbitZeroValue);
                                errorflg = true;
                                continue;
                            }
                            if (strDueAmount.Count<string>() == 1)
                            {
                                printReceiptImporter.ReceiptAmountForPrint = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0");
                            }
                            else
                            {
                                printReceiptImporter.ReceiptAmountForPrint = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1];
                            }
                            continue;
                        }
                        else
                        {
                            //入力値チェック 0 の場合、エラーとする
                            if (receipt.ReceiptAmount == 0)
                            {
                                printReceiptImporter.ReceiptAmountForPrint = dec.ToString() + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.ForbitZeroValue);
                                errorflg = true;
                                continue;
                            }
                            receipt.ReceiptAmount = dec;
                            receipt.RemainAmount = receipt.ReceiptAmount;
                            printReceiptImporter.ReceiptAmountForPrint = Convert.ToDecimal(strDueAmount[0]).ToString("#,##0") + "." + strDueAmount[1];
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.ReceiptAmount = receipt.ReceiptAmount;
                        }
                    }
                    #endregion

                    #region 期日 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.DueAt)
                    {
                        // 最初に検証すべき項目があるだろという ここで検証すべきではない
                        if (receipt.ReceiptCategoryCode == null)
                        {
                            receipt.ReceiptCategoryCode = settingCategoryCode.FixedValue;
                        }
                        var categoryData = ExistReceiptCategory?.Find(x => x.Code == receipt.ReceiptCategoryCode);

                        if (categoryData != null && categoryData.UseLimitDate == 1)
                        {
                            printReceiptImporter.DueAtForPrint = value;
                            if (ValidateEmpty(printReceiptImporter.DueAtForPrint) == false)
                            {
                                printReceiptImporter.DueAtForPrint = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                                errorflg = true;
                                continue;
                            }
                            else
                            {
                                DateTime? date = formatDueAt(value);
                                if (!date.HasValue)
                                {
                                    printReceiptImporter.DueAtForPrint = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidFormat);
                                    errorflg = true;

                                }
                                else
                                {
                                    receipt.DueAt = date.Value;
                                    printReceiptImporter.DueAtForPrint = date.Value.ToString("yyyy/MM/dd");
                                    if (receipt.DueAt < receipt.RecordedAt)
                                    {
                                        printReceiptImporter.DueAtForPrint = value + "*";
                                        ErrorReport.Add(new WorkingReport
                                        {
                                            LineNo = i,
                                            FieldNo = settingIndex,
                                            FieldName = fieldName,
                                            Value = value,
                                            Message = $"入金日より前の期日は登録できません。",
                                        });
                                        errorflg = true;
                                    }
                                    else
                                    {
                                        receipt.DueAt = date.Value;
                                        printReceiptImporter.DueAtForPrint = date.Value.ToString("yyyy/MM/dd");
                                        //重複チェックのため条件設定
                                        if (detail.IsUnique == 1) duplicationCheck.DueAt = receipt.DueAt;
                                    }
                                }
                            }
                        }
                        else
                        {
                            receipt.DueAt = null;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.DueAt = receipt.DueAt;
                        }
                    }
                    #endregion

                    #region 備考 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.Note1)
                    {
                        if (!ValidateEmpty(value)) continue;
                        var length = 100;
                        if (!ValidateLength(value, length))
                        {
                            if (detail.AttributeDivision.Value == 1)
                            {
                                printReceiptImporter.Note1 = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, length);
                                errorflg = true;
                                continue;
                            }
                            value = value.Substring(0, length);
                        }
                        receipt.Note1 = value;
                        printReceiptImporter.Note1 = value;
                        if (detail.IsUnique == 1) duplicationCheck.Note1 = value;
                    }
                    #endregion

                    #region 入金部門コード 検証処理（取込有）
                    else if (detail.Sequence == (int)Fields.SectionCode)
                    {
                        if (doImport)
                        {
                            var code = EbDataHelper.ConvertToUpperCase(value);
                            if (!ValidateEmpty(code))
                            {
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                                errorflg = true;
                                continue;
                            }
                            if (ApplicationControl.SectionCodeType == 0)
                                code = code.PadLeft(ApplicationControl.SectionCodeType, '0');

                            printReceiptImporter.SectionCode = code;
                            receipt.SectionCode = code;

                            var section = ExistSection?.Find(x => x.Code == code);
                            if (section == null)
                            {
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.NotExistMaster);
                                errorflg = true;
                                continue;
                            }
                            receipt.SectionId = section.Id;
                            if (detail.IsUnique == 1) duplicationCheck.SectionId = receipt.SectionId;
                            continue;
                        }
                        else if (detail.FixedValue != null)
                        {
                            var code = detail.FixedValue;
                            receipt.SectionCode = code;
                            printReceiptImporter.SectionCode = code;
                            var section = ExistSection?.Find(x => x.Code == code);
                            if (section != null)
                                receipt.SectionId = section.Id;
                        }
                    }
                    #endregion

                    #region  備考2 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.Note2)
                    {
                        if (!ValidateEmpty(value)) continue;
                        var length = 100;
                        if (!ValidateLength(value, length))
                        {
                            if (detail.AttributeDivision.Value == 1)
                            {
                                printReceiptImporter.Note2 = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, length);
                                errorflg = true;
                                continue;
                            }
                            value = value.Substring(0, length);
                        }
                        receipt.Note2 = value;
                        printReceiptImporter.Note2 = value;
                        if (detail.IsUnique == 1) duplicationCheck.Note2 = value;
                    }
                    #endregion

                    #region 備考3 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.Note3)
                    {
                        if (!ValidateEmpty(value)) continue;
                        var length = 100;
                        if (!ValidateLength(value, length))
                        {
                            if (detail.AttributeDivision.Value == 1)
                            {
                                printReceiptImporter.Note3 = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, length);
                                errorflg = true;
                                continue;
                            }
                            value = value.Substring(0, length);
                        }
                        receipt.Note3 = value;
                        printReceiptImporter.Note3 = value;
                        if (detail.IsUnique == 1) duplicationCheck.Note3 = value;
                    }
                    #endregion

                    #region 備考4 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.Note4)
                    {
                        if (!ValidateEmpty(value)) continue;
                        var length = 100;
                        if (!ValidateLength(value, length))
                        {
                            if (detail.AttributeDivision.Value == 1)
                            {
                                printReceiptImporter.Note4 = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, length);
                                errorflg = true;
                                continue;
                            }
                            value = value.Substring(0, length);
                        }
                        receipt.Note4 = value;
                        printReceiptImporter.Note4 = value;
                        if (detail.IsUnique == 1) duplicationCheck.Note4 = value;
                    }
                    #endregion

                    #region 振込依頼人名 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.PayerName)
                    {
                        // validate additional or validation order
                        if (!ValidateEmpty(value))
                        {
                            if (settingCustomerCode.ImportDivision == 0)
                            {
                                printReceiptImporter.PayerName = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                                errorflg = true;
                                continue;
                            }
                            if (customer == null) // Fields.CustomerCode で取得済
                            {
                                printReceiptImporter.PayerName = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                                errorflg = true;
                                continue;
                            }
                            receipt.PayerName = customer.Kana;
                            receipt.PayerNameRaw = customer.Kana;
                            if (detail.IsUnique == 1) duplicationCheck.PayerName = receipt.PayerName;
                        }
                        else
                        {
                            var length = 140;
                            var raw = EbDataHelper.ConvertToValidEbKana(value);
                            receipt.PayerNameRaw = raw;
                            var validKana = EbDataHelper.ConvertToPayerName(raw, legalPersonalities);
                            if (CollationSetting?.RemoveSpaceFromPayerName == 1)
                                validKana = validKana.Replace(" ", "");

                            if (!ValidateEmpty(validKana))
                            {
                                printReceiptImporter.PayerName = validKana + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                                errorflg = true;
                                continue;
                            }
                            if (!EbDataHelper.IsValidEBChars(validKana))
                            {
                                printReceiptImporter.PayerName = validKana + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidCharactor);
                                errorflg = true;
                                continue;
                            }
                            if (!ValidateLength(validKana, length))
                            {
                                if (detail.AttributeDivision == 1)
                                {
                                    printReceiptImporter.PayerName = validKana + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, length);
                                    errorflg = true;
                                    continue;
                                }
                                validKana = validKana.Substring(0, length);
                            }
                            receipt.PayerName = validKana;
                            printReceiptImporter.PayerName = validKana;
                            receipt.CollationKey = Regex.Replace(validKana, "[^0-9]", "");
                            if (receipt.PayerNameRaw.Length > length)
                                receipt.PayerNameRaw = receipt.PayerNameRaw.Substring(0, length);
                            if (detail.IsUnique == 1) duplicationCheck.PayerName = receipt.PayerName;
                        }
                    }
                    #endregion

                    #region 銀行コード
                    else if (detail.Sequence == (int)Fields.BankCode)
                    {
                        if (doImport)
                        {
                            if (!ValidateEmpty(value)) continue;
                            var length = 4;
                            if (!ValidateLength(value, length))
                            {
                                printReceiptImporter.BankCode = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.length, length);
                                errorflg = true;
                                continue;
                            }
                            if (!IsNumber(value))
                            {
                                printReceiptImporter.BankCode = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidCharactor);
                                errorflg = true;
                                continue;
                            }
                            value = value.PadLeft(length, '0');
                            receipt.BankCode = value;
                            printReceiptImporter.BankCode = value;

                            if (detail.IsUnique == 1) duplicationCheck.BankCode = value;
                        }
                        else if (!string.IsNullOrEmpty(detail.FixedValue))
                        {
                            receipt.BankCode = detail.FixedValue;
                            printReceiptImporter.BankCode = detail.FixedValue;
                        }
                    }
                    #endregion

                    #region 銀行名
                    else if (detail.Sequence == (int)Fields.BankName)
                    {
                        if (doImport)
                        {
                            if (!ValidateEmpty(value)) continue;
                            var length = 30;
                            if (!ValidateLength(value, length))
                            {
                                if (detail.AttributeDivision == 1)
                                {
                                    printReceiptImporter.BankName = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.length, length);
                                    errorflg = true;
                                    continue;
                                }
                                value = value.Substring(0, length);
                            }

                            receipt.BankName = value;
                            printReceiptImporter.BankName = value;

                            if (detail.IsUnique == 1) duplicationCheck.BankName = value;
                        }
                        else if (!string.IsNullOrEmpty(detail.FixedValue))
                        {
                            receipt.BankName = detail.FixedValue;
                            printReceiptImporter.BankName = detail.FixedValue;
                        }
                    }
                    #endregion

                    #region 支店コード
                    else if (detail.Sequence == (int)Fields.BranchCode)
                    {
                        if (doImport)
                        {
                            if (!ValidateEmpty(value)) continue;
                            var length = 3;
                            if (!ValidateLength(value, length))
                            {
                                printReceiptImporter.BranchCode = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.length, length);
                                errorflg = true;
                                continue;
                            }
                            if (!IsNumber(value))
                            {
                                printReceiptImporter.BranchCode = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidCharactor);
                                errorflg = true;
                                continue;
                            }
                            value = value.PadLeft(length, '0');
                            receipt.BranchCode = value;
                            printReceiptImporter.BranchCode = value;

                            if (detail.IsUnique == 1) duplicationCheck.BranchCode = value;
                        }
                        else if (!string.IsNullOrEmpty(detail.FixedValue))
                        {
                            receipt.BranchCode = detail.FixedValue;
                            printReceiptImporter.BranchCode = detail.FixedValue;
                        }
                    }
                    #endregion

                    #region 支店名
                    else if (detail.Sequence == (int)Fields.BranchName)
                    {
                        if (doImport)
                        {
                            if (!ValidateEmpty(value)) continue;
                            var length = 30;
                            if (!ValidateLength(value, length))
                            {
                                if (detail.AttributeDivision == 1)
                                {
                                    printReceiptImporter.BranchName = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.length, length);
                                    errorflg = true;
                                    continue;
                                }
                                value = value.Substring(0, length);
                            }
                            receipt.BranchName = value;
                            printReceiptImporter.BranchName = value;

                            if (detail.IsUnique == 1) duplicationCheck.BranchName = value;
                        }
                        else if (!string.IsNullOrEmpty(detail.FixedValue))
                        {
                            receipt.BranchName = detail.FixedValue;
                            printReceiptImporter.BranchName = detail.FixedValue;
                        }
                    }
                    #endregion

                    #region 預金種別
                    else if (detail.Sequence == (int)Fields.AccountTypeId)
                    {
                        if (doImport)
                        {
                            if (!ValidateEmpty(value)) continue;
                            var validValues = new[] { "1", "2", "4", "5" };
                            if (!validValues.Contains(value))
                            {
                                //printReceiptImporter.AccountTypeIdForPrint = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidCharactor);
                                errorflg = true;
                                continue;
                            }
                            var id = int.Parse(value);
                            receipt.AccountTypeId = id;
                            printReceiptImporter.AccountTypeId = id;

                            if (detail.IsUnique == 1) duplicationCheck.AccountTypeId = id;
                        }
                        else if (!string.IsNullOrEmpty(detail.FixedValue))
                        {
                            var id = int.Parse(detail.FixedValue);
                            receipt.AccountTypeId = id;
                            printReceiptImporter.AccountTypeId = id;
                        }
                    }
                    #endregion

                    #region 口座番号
                    else if (detail.Sequence == (int)Fields.AccountNumber)
                    {
                        if (doImport)
                        {
                            if (!ValidateEmpty(value)) continue;
                            var length = 7;
                            if (!ValidateLength(value, length))
                            {
                                printReceiptImporter.AccountNumber = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.length, length);
                                errorflg = true;
                                continue;
                            }
                            if (!IsNumber(value))
                            {
                                printReceiptImporter.AccountNumber = value + "*";
                                AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidCharactor);
                                errorflg = true;
                                continue;
                            }
                            value = value.PadLeft(length, '0');
                            receipt.AccountNumber = value;
                            printReceiptImporter.AccountNumber = value;

                            if (detail.IsUnique == 1) duplicationCheck.AccountNumber = value;
                        }
                        else if (detail.FixedValue != null)
                        {
                            receipt.AccountNumber = detail.FixedValue;
                            printReceiptImporter.AccountNumber = detail.FixedValue;
                        }
                    }
                    #endregion

                    #region 口座名
                    else if (detail.Sequence == (int)Fields.AccountName)
                    {
                        if (doImport)
                        {
                            if (!ValidateEmpty(value)) continue;
                            var length = 140;
                            if (!ValidateLength(value, length))
                            {
                                if (detail.AttributeDivision == 1)
                                {
                                    printReceiptImporter.AccountName = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.length, length);
                                    errorflg = true;
                                    continue;
                                }
                                value = value.Substring(0, length);
                            }
                            receipt.AccountName = value;
                            printReceiptImporter.AccountName = value;

                            if (detail.IsUnique == 1) duplicationCheck.AccountName = value;
                        }
                        else if (!string.IsNullOrEmpty(detail.FixedValue))
                        {
                            receipt.AccountName = detail.FixedValue;
                            printReceiptImporter.AccountName = detail.FixedValue;

                        }
                    }
                    #endregion

                    #region 仕向銀行 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.SourceBankName)
                    {
                        receipt.SourceBankName = value;
                        printReceiptImporter.SourceBankName = value;
                        if (receipt.SourceBankName != "")
                        {
                            if (ValidateLength(receipt.SourceBankName, 140) == false)
                            {
                                if (detail.AttributeDivision == 1)
                                {
                                    printReceiptImporter.SourceBankName = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, 140);
                                    errorflg = true;
                                }
                                else
                                {
                                    receipt.SourceBankName = receipt.SourceBankName.Substring(0, 140);
                                    //重複チェックのため条件設定
                                    if (detail.IsUnique == 1) duplicationCheck.SourceBankName = receipt.SourceBankName;
                                }
                            }
                        }
                    }
                    #endregion

                    #region 仕向支店 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.SourceBranchName)
                    {
                        receipt.SourceBranchName = value;
                        printReceiptImporter.SourceBranchName = receipt.SourceBranchName;
                        if (receipt.SourceBranchName != "")
                        {
                            if (ValidateLength(value, 15) == false)
                            {
                                if (detail.AttributeDivision == 1)
                                {
                                    printReceiptImporter.SourceBranchName = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, 15);
                                    errorflg = true;
                                }
                                else
                                {
                                    receipt.SourceBranchName = receipt.SourceBranchName.Substring(0, 15);
                                    //重複チェックのため条件設定
                                    if (detail.IsUnique == 1) duplicationCheck.SourceBranchName = receipt.SourceBranchName;
                                }
                            }
                        }
                    }
                    #endregion

                    #region 手形番号 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.BillNumber)
                    {
                        if (receiptCategory != null && receiptCategory.UseLimitDate == 1)
                        {
                            printReceiptImporter.BillNumber = value;
                            if (ValidateEmpty(printReceiptImporter.BillNumber))
                            {
                                if (!ValidateLength(printReceiptImporter.BillNumber, 20))
                                {
                                    if (detail.AttributeDivision == 1)
                                    {
                                        printReceiptImporter.BillNumber = value + "*";
                                        AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, 20);
                                        errorflg = true;

                                    }
                                    else
                                    {
                                        receipt.BillNumber = value.Substring(0, 20);
                                        printReceiptImporter.BillNumber = receipt.BillNumber;
                                    }
                                }
                                else
                                {
                                    receipt.BillNumber = value;
                                    printReceiptImporter.BillNumber = receipt.BillNumber;
                                    //重複チェックのため条件設定
                                    if (detail.IsUnique == 1) duplicationCheck.BillNumber = receipt.BillNumber;
                                }
                            }
                        }
                        else
                        {
                            receipt.BillNumber = null;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.BillNumber = receipt.BillNumber;
                        }
                    }
                    #endregion

                    #region 券面銀行コード 検証処理（取込有）
                    else if (doImport && detail.Sequence == (int)Fields.BillBankCode)
                    {
                        if (receiptCategory != null && receiptCategory.UseLimitDate == 1)
                        {
                            printReceiptImporter.BillBankCode = value;
                            if (!string.IsNullOrWhiteSpace(billBankcode) || !string.IsNullOrWhiteSpace(billBranchcode))
                            {
                                if (ValidateEmpty(printReceiptImporter.BillBankCode) == false)
                                {
                                    printReceiptImporter.BillBankCode = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                                    errorflg = true;
                                }
                                else
                                {
                                    if (!IsNumber(printReceiptImporter.BillBankCode))
                                    {
                                        printReceiptImporter.BillBankCode = value + "*";
                                        AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidCharactor);
                                        errorflg = true;
                                    }
                                    else
                                    {
                                        if (ValidateLength(printReceiptImporter.BillBankCode, 4) == false)
                                        {
                                            printReceiptImporter.BillBankCode = value + "*";
                                            AddErrorLog(i, settingIndex, fieldName, ErrorType.length, 4);
                                            errorflg = true;
                                        }
                                        else
                                        {
                                            receipt.BillBankCode = value.PadLeft(4, '0');
                                            printReceiptImporter.BillBankCode = receipt.BillBankCode;
                                            //重複チェックのため条件設定
                                            if (detail.IsUnique == 1) duplicationCheck.BillBankCode = receipt.BillBankCode;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            receipt.BillBankCode = null;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.BillBankCode = receipt.BillBankCode;
                        }
                    }
                    #endregion

                    #region 券面支店コード 検証処理（取込有）
                    else if (detail.Sequence == (int)Fields.BillBranchCode)
                    {
                        if (receiptCategory != null && receiptCategory.UseLimitDate == 1)
                        {
                            printReceiptImporter.BillBranchCode = value;
                            if (!string.IsNullOrWhiteSpace(billBankcode) || !string.IsNullOrWhiteSpace(billBranchcode))
                            {
                                if (ValidateEmpty(printReceiptImporter.BillBranchCode) == false)
                                {
                                    printReceiptImporter.BillBranchCode = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.InputRequired);
                                    errorflg = true;
                                }
                                else
                                {
                                    if (!IsNumber(printReceiptImporter.BillBranchCode))
                                    {
                                        printReceiptImporter.BillBranchCode = value + "*";
                                        AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidCharactor);
                                        errorflg = true;
                                    }
                                    else
                                    {
                                        if (ValidateLength(printReceiptImporter.BillBranchCode, 3) == false)
                                        {
                                            printReceiptImporter.BillBranchCode = value + "*";
                                            AddErrorLog(i, settingIndex, fieldName, ErrorType.length, 3);
                                            errorflg = true;
                                        }
                                        else
                                        {
                                            receipt.BillBranchCode = value.PadLeft(3, '0');
                                            printReceiptImporter.BillBranchCode = receipt.BillBranchCode;
                                            //重複チェックのため条件設定
                                            if (detail.IsUnique == 1) duplicationCheck.BillBranchCode = receipt.BillBranchCode;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            receipt.BillBranchCode = null;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.BillBranchCode = receipt.BillBranchCode;
                        }
                    }
                    #endregion

                    #region 振出日 検証処理（取込有）
                    else if (detail.Sequence == (int)Fields.BillDrawAt)
                    {
                        if (receiptCategory != null && receiptCategory.UseLimitDate == 1)
                        {
                            printReceiptImporter.BillDrawAtForPrint = value;

                            if (ValidateEmpty(printReceiptImporter.BillDrawAtForPrint))
                            {
                                DateTime? date = formatBillDrawAt(value);
                                if (!date.HasValue)
                                {
                                    printReceiptImporter.BillDrawAtForPrint = value + "*";
                                    AddErrorLog(i, settingIndex, fieldName, ErrorType.InvalidFormat);
                                    errorflg = true;
                                }
                                else
                                {
                                    receipt.BillDrawAt = date.Value;
                                    printReceiptImporter.BillDrawAtForPrint = date.Value.ToString("yyyy/MM/dd");
                                    if (receipt.BillDrawAt > receipt.RecordedAt)
                                    {
                                        printReceiptImporter.BillDrawAtForPrint = value + "*";
                                        ErrorReport.Add(new WorkingReport
                                        {
                                            LineNo = i,
                                            FieldNo = settingIndex,
                                            FieldName = fieldName,
                                            Value = value,
                                            Message = $"入金日より後の振出日は登録できません。",
                                        });
                                        errorflg = true;
                                    }
                                    else
                                    {
                                        receipt.BillDrawAt = date.Value;
                                        printReceiptImporter.BillDrawAtForPrint = date.Value.ToString("yyyy/MM/dd");
                                        //重複チェックのため条件設定
                                        if (detail.IsUnique == 1) duplicationCheck.BillDrawAt = receipt.BillDrawAt;
                                    }
                                }
                            }
                        }
                        else
                        {
                            receipt.BillDrawAt = null;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.BillDrawAt = receipt.BillDrawAt;
                        }
                    }
                    #endregion

                    #region 振出人 検証処理（取込有）
                    else if (detail.Sequence == (int)Fields.BillDrawer)
                    {
                        if (receiptCategory != null && receiptCategory.UseLimitDate == 1)
                        {
                            printReceiptImporter.BillDrawer = value;
                            receipt.BillDrawer = printReceiptImporter.BillDrawer;

                            if (ValidateEmpty(printReceiptImporter.BillDrawer))
                            {
                                if (ValidateLength(value, 48) == false)
                                {
                                    if (detail.AttributeDivision == 1)
                                    {
                                        printReceiptImporter.BillDrawer = value + "*";
                                        AddErrorLog(i, settingIndex, fieldName, ErrorType.lengthchar, 48);
                                        errorflg = true;
                                    }
                                    else
                                    {
                                        receipt.BillDrawer = value.Substring(0, 48);
                                        printReceiptImporter.BillDrawer = receipt.BillDrawer;
                                        //重複チェックのため条件設定
                                        if (detail.IsUnique == 1) duplicationCheck.BillDrawer = receipt.BillDrawer;
                                    }
                                }
                            }
                        }
                        else
                        {
                            receipt.BillDrawer = null;
                            //重複チェックのため条件設定
                            if (detail.IsUnique == 1) duplicationCheck.BillDrawer = receipt.BillDrawer;
                        }
                    }
                    #endregion
                }

                #region set receipt default data
                receipt.CompanyId = CompanyId;
                receipt.InputType = 3;//インポーター取込
                receipt.Apportioned = 1;
                receipt.Approved = 1;
                receipt.Workday = DateTime.Today;
                receipt.PayerCode = "";
                receipt.ReferenceNumber = "";
                receipt.RecordNumber = "";
                receipt.Memo = "";
                receipt.CreateBy = LoginUserId;
                receipt.UpdateBy = LoginUserId;
                receipt.LearnKanaHistory = false;
                if (receipt.SourceBankName == null) receipt.SourceBankName = "";
                if (receipt.SourceBranchName == null) receipt.SourceBranchName = "";
                if (receipt.Note1 == null) receipt.Note1 = "";
                if (receipt.Note2 == null) receipt.Note2 = "";
                if (receipt.Note3 == null) receipt.Note3 = "";
                if (receipt.Note4 == null) receipt.Note4 = "";
                if (receipt.BillNumber == null) receipt.BillNumber = "";
                if (receipt.BillBankCode == null) receipt.BillBankCode = "";
                if (receipt.BillBranchCode == null) receipt.BillBranchCode = "";
                if (receipt.BillDrawer == null) receipt.BillDrawer = "";
                #endregion


                if (!skipflg)
                {
                    PossibleData.Add(printReceiptImporter);
                    saveReciptInput.Add(receipt);

                    if (errorflg == false && isAnyUnique)
                    {
                        duplications.Add(duplicationCheck);
                    }
                }
            }

            await ValidateDuplicationAsync(duplications);


            //例外発生しないように今後HashSetに変更する予定
            var errorLineNo = ErrorReport.Where(x => x.LineNo.HasValue)
                .Select(x => (int)x.LineNo.Value).Distinct().ToArray();
            foreach (int index in errorLineNo)
            {
                ImpossibleData.Add(PossibleData[index - ImporterSetting.StartLineCount]);
            }
            PossibleData = PossibleData.Except(ImpossibleData).ToList();
            saveReciptInput = saveReciptInput.Except(ImpossibleData).ToList();
            var totalRecordCount = (ImporterSetting.IgnoreLastLine == 1) ? records.Count - 1 : records.Count;
            ReadCount = (totalRecordCount - (skipCount + (ImporterSetting.StartLineCount - 1)));
            ValidCount = PossibleData.Count;
            InvalidCount = ImpossibleData.Count;
            if (SaveImportDataAsync != null)
            {
                var data = new ImportData {
                    CompanyId       = CompanyId,
                    FileName        = "",
                    FileSize        = 0,
                    CreateBy        = LoginUserId,
                    //CreateAt        = DateTime.Now,
                };
                var details = new List<ImportDataDetail>();
                details.AddRange(saveReciptInput.Select(x => new ImportDataDetail {
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
            return valid;
        }

        private string GetFieldName(ImporterSettingDetail detail)
        {
            var result = detail.FieldName;
            if (detail.Sequence == (int)Fields.Note1) result = string.IsNullOrWhiteSpace(Note1) ? "備考"  : Note1;
            if (detail.Sequence == (int)Fields.Note2) result = string.IsNullOrWhiteSpace(Note2) ? "備考2" : Note2;
            if (detail.Sequence == (int)Fields.Note3) result = string.IsNullOrWhiteSpace(Note3) ? "備考3" : Note3;
            if (detail.Sequence == (int)Fields.Note4) result = string.IsNullOrWhiteSpace(Note4) ? "備考4" : Note4;
            return result;
        }

        private int GetFirstPriority(ImporterSettingDetail detail)
            => detail.Sequence == (int)Fields.CustomerCode          ?  1
             : detail.Sequence == (int)Fields.ReceiptCategoryCode   ?  2
             : detail.Sequence == (int)Fields.RecordedAt            ?  3
             : detail.Sequence == (int)Fields.DueAt                 ?  4
             : detail.Sequence == (int)Fields.PayerName             ?  5
             : detail.Sequence == (int)Fields.SourceBankName        ?  6
             : detail.Sequence == (int)Fields.SourceBranchName      ?  7
             : detail.Sequence == (int)Fields.BillNumber            ?  8
             : detail.Sequence == (int)Fields.BillBankCode          ?  9
             : detail.Sequence == (int)Fields.BillBranchCode        ? 10
             : detail.Sequence == (int)Fields.BillDrawer            ? 11
             : detail.Sequence == (int)Fields.BillDrawAt            ? 12
             : 99;

        #region エラーログ設定
        /// <summary>検証エラー todo: refactor naming</summary>
        private enum ErrorType
        {
            /// <summary>がありません。</summary>
            InvalidFieldIndex,
            /// <summary>空白のためインポートできません。</summary>
            InputRequired,
            /// <summary>フォーマットが異なるため、インポートできません。</summary>
            InvalidFormat,
            /// <summary>桁以上のためインポートできません。</summary>
            length,
            /// <summary>不正な文字が入力されています。</summary>
            InvalidCharactor,
            /// <summary>小数が含まれているためインポートできません。</summary>
            ForbitDecimalValue,
            /// <summary>0 のため、インポートできません。</summary>
            ForbitZeroValue,
            /// <summary>存在しないため、インポートできません。</summary>
            NotExistMaster,
            /// <summary>文字以上のためインポートできません。</summary>
            lengthchar,
            /// <summary>任意のエラーメッセージを追加する用途で利用</summary>
            Custom,
        }

        private void AddErrorLog(int lineNo, int fieldIndex, string fieldName, ErrorType type, int ?length = null, string customMessage = null)
        {
            const string notExistMessage = "がありません。";
            const string emptyMessage = "空白のためインポートできません。";
            const string formatMessage = "フォーマットが異なるため、インポートできません。";
            const string lengthMessage = "桁以上のためインポートできません。";
            const string wrongLetterMessage = "不正な文字が入力されています。";
            const string precisionMessage = "小数が含まれているためインポートできません。";
            const string zeroMessage = "0 のため、インポートできません。";
            const string masterExistMessage = "存在しないため、インポートできません。";
            const string lengthCharMessage = "文字以上のためインポートできません。";
            var error = new WorkingReport
            {
                LineNo = lineNo,
                FieldName = fieldName,
                FieldNo = fieldIndex,
            };
            switch (type)
            {
                case ErrorType.InvalidFieldIndex:       error.Message = fieldName + notExistMessage;    break;
                case ErrorType.InputRequired:           error.Message = emptyMessage;                   break;
                case ErrorType.InvalidFormat:           error.Message = formatMessage;                  break;
                case ErrorType.length:                  error.Message = length + lengthMessage;         break;
                case ErrorType.InvalidCharactor:        error.Message = wrongLetterMessage;             break;
                case ErrorType.ForbitDecimalValue:      error.Message = precisionMessage;               break;
                case ErrorType.ForbitZeroValue:         error.Message = zeroMessage;                    break;
                case ErrorType.NotExistMaster:          error.Message = masterExistMessage;             break;
                case ErrorType.lengthchar:              error.Message = length + lengthCharMessage;     break;
                case ErrorType.Custom:                  error.Message = customMessage ?? string.Empty;  break;
                default:                                error.Message = string.Empty;                   break;
            };
            ErrorReport.Add(error);
        }
        #endregion

        private string GetCurrencyDisplayFormat (int precision)
        {
            var format = "###,###,###,##0";
            if (precision > 0) format += "." + new string('0', precision);
            return format;
        }

        private Customer TryGetCustomer (Dictionary<string, Customer> dicCustomer, string code)
        {
            Customer result;
            return dicCustomer.TryGetValue(code, out result) ? result : null;
        }

        private async Task ValidateDuplicationAsync(IEnumerable<ReceiptImportDuplication> duplications)
        {
            if (!(duplications?.Any() ?? false)) return;
            var groups = duplications.GroupBy(x => new
            {
                x.CustomerId,
                x.RecordedAt,
                x.ReceiptCategoryId,
                x.CurrencyId,
                x.ReceiptAmount,
                x.DueAt,
                x.Note1,
                x.Note2,
                x.Note3,
                x.Note4,
                x.SectionId,
                x.PayerName,
                x.SourceBankName,
                x.SourceBranchName,
                x.BillNumber,
                x.BillBankCode,
                x.BillBranchCode,
                x.BillDrawAt,
                x.BillDrawer,
                x.BankCode,
                x.BankName,
                x.BranchCode,
                x.BranchName,
                x.AccountTypeId,
                x.AccountNumber,
                x.AccountName,
            }).ToArray();
            var distinctKeys = groups.Select(x => x.First()).ToArray();
            var rowNumbers = await GetReceiptImportDuplicationAsync(distinctKeys, ImporterSettingDetail.Where(x => x.IsUnique == 1).ToArray());
            if (!(rowNumbers?.Any() ?? false)) return;
            var dupedInDB = distinctKeys.Where(x => rowNumbers.Contains(x.RowNumber)).ToArray();
            foreach (var dupe in groups.Where(x => dupedInDB.Any(y
                => x.Key.CustomerId         == y.CustomerId
                && x.Key.RecordedAt         == y.RecordedAt
                && x.Key.ReceiptCategoryId  == y.ReceiptCategoryId
                && x.Key.CurrencyId         == y.CurrencyId
                && x.Key.ReceiptAmount      == y.ReceiptAmount
                && x.Key.DueAt              == y.DueAt
                && x.Key.Note1              == y.Note1
                && x.Key.Note2              == y.Note2
                && x.Key.Note3              == y.Note3
                && x.Key.Note4              == y.Note4
                && x.Key.SectionId          == y.SectionId
                && x.Key.PayerName          == y.PayerName
                && x.Key.SourceBankName     == y.SourceBankName
                && x.Key.SourceBranchName   == y.SourceBranchName
                && x.Key.BillNumber         == y.BillNumber
                && x.Key.BillBankCode       == y.BillBankCode
                && x.Key.BillBranchCode     == y.BillBranchCode
                && x.Key.BillDrawAt         == y.BillDrawAt
                && x.Key.BillDrawer         == y.BillDrawer
                && x.Key.BankCode           == y.BankCode
                && x.Key.BankName           == y.BankName
                && x.Key.BranchCode         == y.BranchCode
                && x.Key.BranchName         == y.BranchName
                && x.Key.AccountTypeId      == y.AccountTypeId
                && x.Key.AccountNumber      == y.AccountNumber
                && x.Key.AccountName        == y.AccountName
                )).SelectMany(x => x))
                ErrorReport.Add(new WorkingReport
                {
                    LineNo = dupe.RowNumber,
                    Message = $"重複しているため、インポートできません。",
                });
        }


        public async Task<bool> ImportAsync()
        {
            if (LoadImportDataAsync != null)
            {
                ImportData = await LoadImportDataAsync();
                saveReciptInput.AddRange(ImportData.Details
                    .Where(x => x.ObjectType == 0)
                    .Select(x => Deserialize(x.RecordItem)).ToArray());
            }
            var importResult = false;
            var webResult = await ImportAsync(saveReciptInput.ToArray());
            SaveAmount = saveReciptInput.Sum(x => x.ReceiptAmount);
            SaveCount = saveReciptInput.Count;
            if (webResult.ProcessResult.Result)
            {
                importResult = true;
            }
            return importResult;
        }
        public bool WriteErrorLog(string path)
            => base.WriteErrorLog(path, "入金データ");

        public List<ReceiptInput> GetReportSource(bool isPossible)
            => isPossible ? PossibleData : ImpossibleData;

        #region web service call

        private async Task<int[]> GetReceiptImportDuplicationAsync(
            ReceiptImportDuplication[] receipts,
            ImporterSettingDetail[] details)
            => await ReceiptImportDuplicationCheckAsync(CompanyId, receipts, details)
                ?? new int[] { };

        private async Task<ReceiptInputsResult> ImportAsync(ReceiptInput[] receipts)
            => await SaveInnerAsync(receipts);

        private async Task LoadMasterDataAsync(IEnumerable<ImporterSettingDetail> details)
        {
            var tasks = new List<Task>();
            foreach (var field in new Fields[]
            {
                Fields.CustomerCode,
                Fields.ReceiptCategoryCode,
                Fields.SectionCode,
                Fields.PayerName,
            })
            {
                var setting = details.FirstOrDefault(x => x.Sequence == (int)field
                    && (x.FieldIndex.HasValue || !string.IsNullOrEmpty(x.FixedValue)));
                if (setting == null) continue;
                var isFixed = !string.IsNullOrEmpty(setting.FixedValue);
                var codes = isFixed ? new string[] { setting.FixedValue } : null;
                if (field == Fields.CustomerCode) tasks.Add(LoadCustomerAsync(codes));
                if (field == Fields.ReceiptCategoryCode) tasks.Add(LoadReceiptCategoryAsync(codes));
                if (field == Fields.SectionCode) tasks.Add(LoadSectionAsync(codes));
                if (field == Fields.PayerName) tasks.Add(LoadLeagalPersonalitiesAsync());
            }
            tasks.Add(LoadCurrencyAsync());
            tasks.Add(LoadCollationSettingAsync());
            await Task.WhenAll(tasks);
        }

        private async Task LoadCurrencyAsync(string[] codes = null)
            => ExistCurrency = await GetCurrenciesAsync(CompanyId, codes);
        private async Task LoadReceiptCategoryAsync(string[] codes = null)
            => ExistReceiptCategory = (await GetCategoriesByCodesAsync(CompanyId, CategoryType.Receipt, codes))
            .Where(x => string.CompareOrdinal(x.Code, "98") < 0).ToList();
        private async Task LoadSectionAsync(string[] codes = null)
            => ExistSection = await GetSectionByCodesAsync(CompanyId, codes);
        private async Task LoadCustomerAsync(string[] codes = null)
            => ExistCustomer = await GetCustomerByCodesAsync(CompanyId, codes);
        private async Task LoadLeagalPersonalitiesAsync()
            => legalPersonalities = await GetLegalPersonaritiesAsync(CompanyId);

        private async Task LoadCollationSettingAsync()
            => CollationSetting = await GetCollationSettingAsync(CompanyId);

        #endregion
    }
}
