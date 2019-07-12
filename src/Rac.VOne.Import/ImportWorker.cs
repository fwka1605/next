using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System.Globalization;

namespace Rac.VOne.Import
{
    // ファイルから取り込んだデータの設定と、書式の検証を行う。
    public class ImportWorker<TModel> : IFieldVisitor<TModel>
            where TModel : class, new()
    {
        ///<summary> A : 大文字のアルファベット（A～Z）</summary>
        private const string UpperCasePermission = "A-Z";

        ///<summary> a : 小文字のアルファベット（a～z）</summary>
        private const string LowerCasePermission = "a-z";

        ///<summary> K : カタカナ（促音・拗音の小書き表記あり）</summary>
        private const string HankakuKanaKPermission = @"\uff61-\uff9f";

        ///<summary> N : カタカナ（促音・拗音の小書き表記なし）</summary>
        private const string HankakuKanaNPermission = @"\uff61-\uff66\uff70-\uff9f";

        ///<summary> 9 : 数字（0～9）</summary>
        private const string DigitPermission = "0-9";

        ///<summary> # : 数字および数字関連記号（0～9、+ - $ % \ , .）</summary>
        private const string NumberPermission = DigitPermission + @"-\+\$%\,\.";

        ///<summary> @ : 記号（! " # $ % & ' ( ) - = ^ ~ \ | @ ` [ { ; + : * ] } , < . > / ? _ ｡ ｢ ｣ ､ ･）</summary>
        private const string SymbolPermission = "][!\"" + @"#\$%&'\(\)-=\^~\\\|@`\{;\+\:\*\},<\.>/\?_｡｢｣､･";

        /// <summary> A9 : 英数 0-9A-Z</summary>
        private const string DigitCharPermission = DigitPermission + UpperCasePermission;

        /// <summary>\uff61-\uff9f0-9A-Z</summary>
        private const string KanaDigitCharPermission = HankakuKanaKPermission + DigitCharPermission;

        /// <summary>0-9A-Za-z</summary>
        private const string DigitAlphabetPermission = DigitPermission + UpperCasePermission + LowerCasePermission;

        /// <summary>数字のみに制限する正規表現パターン ^[0-9]+$</summary>
        private string DigitPermissionPattern { get { return ConvertPattern(DigitPermission); } }

        /// <summary>英数のみに制限する正規表現パターン ^[0-9A-Z]+$</summary>
        private string DigitCharPermissionPattern { get { return ConvertPattern(DigitCharPermission); } }

        /// <summary>得意先コード用英数のみに制限する正規表現パターン ^[-0-9A-Z]+$</summary>
        private string CustomerCodeDigitCharPermissionPattern { get { return ConvertPattern("-" + DigitCharPermission); } }

        /// <summary>得意先コード用カナ英数のみに制限する正規表現パターン ^[-\uff61-\uff9f0-9A-Z]+$</summary>
        private string KanaDigitCharPermissionPattern { get { return ConvertPattern("-" + KanaDigitCharPermission); } }

        /// <summary>英数 小文字許可 ^[0-9A-Za-z]+$</summary>
        private string DigitAlphabetPermissionPattern { get { return ConvertPattern(DigitAlphabetPermission); } }

        /// <summary>電話番号 FAX番号 数字 "-" 許可</summary>
        private string TelFaxPostNumberPermissionPattern { get { return ConvertPattern("-" + DigitPermission); } }

        private const int CategoryCodeLength = 2;
        private const int CurrencyCodeLength = 3;

        /// <summary>正規表現用表記への変換メソッド </summary>
        /// <param name="value"></param>
        /// <returns>行頭から行末まで、入力されたvalue の繰り返しとなる正規表現 ^[{value}]+$</returns>
        private string ConvertPattern(string value) => $@"^[{value}]+$";

        // ユーザー情報
        public int LoginCompanyId { get; set; }
        public string LoginCompanyCode { get; set; }
        public int LoginUserId { get; set; }
        public string LoginUserCode { get; set; }
        private DataExpression DataExpression { get; set; }
        public RowDefinition<TModel> RowDef { get; set; }

        public int RecordCount { get; private set; }
        public string[] FieldValues { get; private set; }
        public TModel Model { get; private set; }

        public List<WorkingReport> Reports { get; } = new List<WorkingReport>();

        // <fieldNo, <code, List<lineNo>>>
        public Dictionary<int, Dictionary<string, List<int>>> CodeResolutionQueue { get; private set; }

        public Dictionary<int, TModel> Models { get; } = new Dictionary<int, TModel>();

        public void Ignore(int lineNo)
        {
            if (Models.Remove(lineNo))
            {
                RecordCount--;
                if (CodeResolutionQueue != null)
                {
                    foreach (var codeDic in CodeResolutionQueue.Values)
                    {
                        foreach (var lineList in codeDic.Values)
                        {
                            lineList.Remove(lineNo);
                        }
                    }
                }
            }
        }

        ///<summary> 歩引率（得意先マスター歩引設定）</summary>
        private bool rateEmpty = false;

        private IEnumerable<string> LegalPersonalities { get; set; }
        public void SetLegalPersonalities(IEnumerable<string> personalities)
        {
            LegalPersonalities = personalities;
        }

        public ImportWorker(RowDefinition<TModel> rowDef)
        {
            RowDef = rowDef;
            DataExpression = rowDef.DataExpression;
        }

        public long NewRecord(string[] fields)
        {
            FieldValues = fields;
            Model = new TModel();
            System.Reflection.PropertyInfo prop = typeof(TModel).GetProperty("CreateBy");
            if (prop != null) prop.SetValue(Model, LoginUserId);

            RecordCount++;
            Models.Add(RecordCount, Model);

            Reports.Clear();

            return RecordCount;
        }

        private bool ValidateFieldCount<TValue>(FieldDefinition<TModel, TValue> def)
        {
            bool valid = FieldValues.Length >= def.FieldIndex;
            return valid;
        }

        private bool Required<TValue>(FieldDefinition<TModel, TValue> def)
        {
            string value = FieldValues[def.FieldIndex - 1];
            if (string.IsNullOrEmpty(value))
            {
                Reports.Add(new WorkingReport
                {
                    LineNo = RecordCount,
                    FieldNo = def.FieldIndex,
                    FieldName = def.FieldName,
                    Value = value,
                    Message = "空白のためインポートできません。",
                });
                return false;
            }
            return true;
        }

        private void LengthError<TValue>(FieldDefinition<TModel, TValue> def, string value, int length)
        {
            Reports.Add(new WorkingReport
            {
                LineNo = RecordCount,
                FieldNo = def.FieldIndex,
                FieldName = def.FieldName,
                Value = value,
                Message = $"{length}文字以上のためインポートできません。",
            });
        }

        private void InvalidCharError<TValue>(FieldDefinition<TModel, TValue> def, string value)
        {
            Reports.Add(new WorkingReport
            {
                LineNo = RecordCount,
                FieldNo = def.FieldIndex,
                FieldName = def.FieldName,
                Value = value,
                Message = $"不正な文字が入力されているためインポートできません。",
            });
        }
        private void KeyNotExistsError<TValue>(FieldDefinition<TModel, TValue> def, string value)
        {
            Reports.Add(new WorkingReport
            {
                LineNo = RecordCount,
                FieldNo = def.FieldIndex,
                FieldName = def.FieldName,
                Value = value,
                Message = $"存在しないため、インポートできません。",
            });
        }
        public void AddKeyDuplicatedError(int lineNo)
        {
            Reports.Add(new WorkingReport
            {
                LineNo = lineNo,
                FieldName = RowDef.KeyFields?.FirstOrDefault()?.FieldName ?? string.Empty,
                Message = $"既に登録されている{RowDef.DataTypeToken}のため、インポートできません。"
            });
        }
        private Dictionary<int, Dictionary<string, List<int>>> StoreCode(
                Dictionary<int, Dictionary<string, List<int>>> queue,
                FieldDefinition<TModel, int> def,
                string value)
        {
            if (queue == null)
            {
                queue = new Dictionary<int, Dictionary<string, List<int>>>();
            }
            // 列番号に対応するキューがあるか
            Dictionary<string, List<int>> fieldQueue = null;
            if (!queue.TryGetValue(def.FieldIndex, out fieldQueue))
            {
                fieldQueue = new Dictionary<string, List<int>>();
                queue.Add(def.FieldIndex, fieldQueue);
            }

            // キューに対象のコードがあるか
            List<int> codeQueue = null;
            if (!fieldQueue.TryGetValue(value, out codeQueue))
            {
                codeQueue = new List<int>();
                fieldQueue.Add(value, codeQueue);
            }
            codeQueue.Add(RecordCount);

            return queue;
        }

        public bool AccountTitleCode(ForeignKeyFieldDefinition<TModel, int, AccountTitle> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (!rateEmpty && (def.Required || !string.IsNullOrEmpty(value)))
            {
                if (DataExpression.AccountTitleCodeFormatString == "9")
                {
                    if (value.Length < DataExpression.AccountTitleCodeLength)
                    {
                        value = value.PadLeft(DataExpression.AccountTitleCodeLength, '0');
                    }
                }
                else if (DataExpression.AccountTitleCodeFormatString == "A9")
                {
                    value = EbDataHelper.ConvertToUpperCase(value);
                }

                if (valid && value.Length > DataExpression.AccountTitleCodeLength)
                {
                    valid = false;
                    KeyNotExistsError(def, value);
                }

                if (valid)
                {
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
            }
            return valid;
        }

        public bool CategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                valid &= Regex.IsMatch(value, @"^\d{1,2}$");
                if (valid)
                {
                    value = (value ?? string.Empty).PadLeft(2, '0');
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
                else
                {
                    KeyNotExistsError(def, value);
                }
            }

            return valid;
        }

        public bool OwnCompanyCode(ForeignKeyFieldDefinition<TModel, int, Company> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            if (DataExpression.CompanyCodeType == 0)
            {
                value = (value ?? string.Empty).PadLeft(4, '0');
            }
            else if (DataExpression.CompanyCodeType == 1)
            {
                value = EbDataHelper.ConvertToUpperCase(value);
            }

            bool valid = (value == LoginCompanyCode);
            if (!valid)
            {
                Reports.Add(new WorkingReport
                {
                    LineNo = RecordCount,
                    FieldNo = def.FieldIndex,
                    FieldName = def.FieldName,
                    Value = value,
                    Message = "ログインしている会社コードではないためインポートできません。",
                });
            }
            else
            {
                if (def.ModelHasCode) def.SetCode(Model, value);
                def.SetValue(Model, LoginCompanyId);
            }

            return valid;
        }

        public bool CustomerCode(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;
            if (def.Required || !string.IsNullOrEmpty(value)) // 入力があれば
            {
                if (DataExpression.CustomerCodeFormatString == "9")
                {
                    if (value.Length < DataExpression.CustomerCodeLength)
                    {
                        value = value.PadLeft(DataExpression.CustomerCodeLength, '0');
                    }
                }
                else if (DataExpression.CustomerCodeFormatString == "A9-")
                {
                    value = EbDataHelper.ConvertToUpperCase(value);
                }
                else
                {
                    value = EbDataHelper.ConvertToUpperCase(value);
                }
            }

            if (valid && value.Length > DataExpression.CustomerCodeLength)
            {
                valid = false;
                KeyNotExistsError(def, value);
            }

            if (valid)
            {
                if (def.ModelHasCode) def.SetCode(Model, value);
                CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
            }
            return valid;
        }

        public bool LoginUserCodeField(ForeignKeyFieldDefinition<TModel, int, LoginUser> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (!rateEmpty && (def.Required || !string.IsNullOrEmpty(value)))
            {
                if (DataExpression.LoginUserCodeFormatString == "9")
                {
                    if (!Regex.IsMatch(value, DigitPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }
                    if (value.Length < DataExpression.LoginUserCodeLength)
                    {
                        value = value.PadLeft(DataExpression.LoginUserCodeLength, '0');
                    }
                }
                else if (DataExpression.LoginUserCodeFormatString == "A9")
                {
                    value = EbDataHelper.ConvertToUpperCase(value);
                }

                if (valid && value.Length > DataExpression.LoginUserCodeLength)
                {
                    valid = false;
                    KeyNotExistsError(def, value);
                }

                if (valid)
                {
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
            }

            return valid;
        }

        public bool PaymentAgencyCode(ForeignKeyFieldDefinition<TModel, int, PaymentAgency> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                valid &= Regex.IsMatch(value, @"^\d{1,2}$");

                if (valid)
                {
                    value = (value ?? string.Empty).PadLeft(2, '0');
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
                else
                {
                    KeyNotExistsError(def, value);
                }
            }

            return valid;
        }

        public bool DepartmentCode(ForeignKeyFieldDefinition<TModel, int, Department> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (!rateEmpty && (def.Required || !string.IsNullOrEmpty(value)))
            {
                if (DataExpression.DepartmentCodeFormatString == "9")
                {
                    if (value.Length < DataExpression.DepartmentCodeLength)
                    {
                        value = value.PadLeft(DataExpression.DepartmentCodeLength, '0');
                    }
                }
                else if (DataExpression.DepartmentCodeFormatString == "A9")
                {
                    value = EbDataHelper.ConvertToUpperCase(value);
                }

                if (valid && value.Length > DataExpression.DepartmentCodeLength)
                {
                    valid = false;
                    KeyNotExistsError(def, value);
                }

                if (valid)
                {
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
            }

            return valid;
        }

        /// <summary>初回パスワード</summary>
        public bool InitialPassword(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            // ここではなくAdditionalValidationにて検証する。

            if (valid)
            {
                def.SetValue(Model, value);
            }

            return valid;
        }

        public bool PayerName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            string converted = value;
            if (converted == null) converted = string.Empty;
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                converted = EbDataHelper.ConvertToPayerName(converted, LegalPersonalities);

                if (EbDataHelper.ContainsKanji(converted))
                {
                    InvalidCharError(def, converted);
                    valid = false;
                }

                // 文字数が140文字を超える場合、超過分を切り捨てる
                if (converted.Length > 140)
                {
                    converted = converted.Substring(0, 140);
                }
            }

            if (valid)
            {
                def.SetValue(Model, converted);
            }
            return valid;
        }

        public bool MailAddress(StringFieldDefinition<TModel> def)
        {
            bool valid = true;

            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (!Regex.IsMatch(value, @"^[!#%&'""=`{}~\.\-\+\*\?\^\|\/\a-zA-Z0-9\.*]*$"))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else
                {
                    if (!Regex.IsMatch(value, @"^[!#%&'""=`{}~\.\-\+\*\?\^\|\/\a-zA-Z0-9\.*]*@[!#%&'""=`{}~\.\-\+\*\?\^\|\/\a-zA-Z0-9\.*]*$"))
                    {
                        Reports.Add(new WorkingReport
                        {
                            LineNo = RecordCount,
                            FieldNo = def.FieldIndex,
                            FieldName = def.FieldName,
                            Value = value,
                            Message = "メールアドレスに不正な文字があります。",
                        });
                    }
                }

                if (value.Length > 254)
                {
                    valid = false;
                    LengthError(def, value, 254);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }

            return valid;

        }

        public bool StaffCode(ForeignKeyFieldDefinition<TModel, int, Staff> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (!rateEmpty && (def.Required || !string.IsNullOrEmpty(value)))
            {
                if (DataExpression.StaffCodeFormatString == "9")
                {
                    if (value.Length < DataExpression.StaffCodeLength)
                    {
                        value = value.PadLeft(DataExpression.StaffCodeLength, '0');
                    }
                }
                else if (DataExpression.StaffCodeFormatString == "A9")
                {
                    value = EbDataHelper.ConvertToUpperCase(value);
                }

                if (valid && value.Length > DataExpression.StaffCodeLength)
                {
                    valid = false;
                    KeyNotExistsError(def, value);
                }

                if (valid)
                {
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
            }

            return valid;

        }

        public bool StandardNumber<TValue>(NumberFieldDefinition<TModel, TValue> def)
           where TValue : struct, IComparable<TValue>
        {
            if (FieldValues.Length < def.FieldIndex) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                Type t = typeof(TValue);
                object data = null;
                if (t == typeof(int)) { int a; if (int.TryParse(value, out a)) data = a; }
                else if (t == typeof(long)) { long a; if (long.TryParse(value, out a)) data = a; }
                else if (t == typeof(DateTime)) { DateTime a; if (DateTime.TryParseExact(value, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out a)) data = a; }
                else if (t == typeof(decimal)) { decimal a; if (decimal.TryParse(value, out a)) data = a; }

                else if (t == typeof(uint)) { uint a; if (uint.TryParse(value, out a)) data = a; }
                else if (t == typeof(ulong)) { ulong a; if (ulong.TryParse(value, out a)) data = a; }
                else if (t == typeof(short)) { short a; if (short.TryParse(value, out a)) data = a; }
                else if (t == typeof(ushort)) { ushort a; if (ushort.TryParse(value, out a)) data = a; }
                else if (t == typeof(float)) { float a; if (float.TryParse(value, out a)) data = a; }
                else if (t == typeof(double)) { double a; if (double.TryParse(value, out a)) data = a; }
                else if (t == typeof(char)) { char a; if (char.TryParse(value, out a)) data = a; }
                else if (t == typeof(byte)) { byte a; if (byte.TryParse(value, out a)) data = a; }
                else if (t == typeof(sbyte)) { sbyte a; if (sbyte.TryParse(value, out a)) data = a; }
                else if (t == typeof(bool)) { bool a; if (bool.TryParse(value, out a)) data = a; }

                if (data == null)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else
                {
                    var number = (TValue)data;
                    def.SetValue(Model, number);
                }
            }
            else
            {
                if (def.IsNullable)
                {
                    (def as NullableNumberFieldDefinition<TModel, TValue>).SetNullableValue(Model, null);
                }
                else
                {
                    def.SetValue(Model, default(TValue));
                }
            }
            return valid;
        }

        public bool StandardString(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            def.SetValue(Model, value);
            return true;
        }

        public bool OwnLoginUserCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                value = EbDataHelper.ConvertToUpperCase(value);

                if (value.Length > DataExpression.LoginUserCodeLength)
                {
                    valid = false;
                    LengthError(def, value, DataExpression.LoginUserCodeLength);
                }

                if (DataExpression.LoginUserCodeFormatString == "9")
                {
                    if (!Regex.IsMatch(value, DigitPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }

                    if (value.Length < DataExpression.LoginUserCodeLength)
                    {
                        value = value.PadLeft(DataExpression.LoginUserCodeLength, '0');
                    }
                }
                else if (DataExpression.LoginUserCodeFormatString == "A9")
                {
                    if (!Regex.IsMatch(value, DigitCharPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }

            return valid;
        }

        public bool OwnLoginUserName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            string converted = value;
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value)) // 入力があれば
            {
                if (value.Length > 40)
                {
                    converted = converted.Substring(0, 40);
                }
            }

            if (valid)
            {
                def.SetValue(Model, converted);
            }
            return valid;
        }

        public bool MenuLevelField(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value)) // 入力があれば
            {
                int result = 0;
                if (!int.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result < 1 || result > 4)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, int.Parse(value));
            }
            return valid;
        }

        public bool FunctionLevelField(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def)) return false;

            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value)) // 入力があれば
            {
                int result = 0;
                if (!int.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result < 1 || result > 6)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, int.Parse(value));
            }
            return valid;
        }

        public bool UseClientField(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                int result = 0;
                if (!int.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result != 1 && result != 0)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, int.Parse(value));
            }
            return valid;
        }

        public bool UseWebViewerField(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;
            int result = 0;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (!int.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result != 1 && result != 0)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, result);
            }
            return valid;
        }

        public bool StaffCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            string converted = value;
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                converted = EbDataHelper.ConvertToUpperCase(converted);

                if (value.Length > DataExpression.StaffCodeLength)
                {
                    valid = false;
                    LengthError(def, value, DataExpression.StaffCodeLength);
                }
                if (DataExpression.StaffCodeFormatString == "9")
                {
                    if (!Regex.IsMatch(value, DigitPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }

                    if (value.Length < DataExpression.StaffCodeLength)
                    {
                        value = (value ?? string.Empty).PadLeft(DataExpression.StaffCodeLength, '0');
                        converted = value;
                    }
                }
                else if (DataExpression.StaffCodeFormatString == "A9")
                {
                    if (!Regex.IsMatch(value, DigitAlphabetPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }
                    else
                    {
                        value = EbDataHelper.ConvertToUpperCase(value);
                    }
                }

                if (valid)
                {
                    def.SetValue(Model, converted);
                }
            }
            return valid;
        }

        public bool StaffName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value)) // 入力があれば
            {
                if (value.Length > 40)
                {
                    value = value.Substring(0, 40);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool AccountTitleCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            string converted = value;

            bool valid = true;
            if (def.Required || !string.IsNullOrEmpty(value))
            {
                converted = EbDataHelper.ConvertToUpperCase(converted);

                if (value.Length > DataExpression.AccountTitleCodeLength)
                {
                    valid = false;
                    LengthError(def, value, DataExpression.AccountTitleCodeLength);
                }

                if (DataExpression.AccountTitleCodeFormatString == "9")
                {
                    if (!Regex.IsMatch(value, DigitPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }

                    if (value.Length < DataExpression.AccountTitleCodeLength)
                    {
                        value = (value ?? string.Empty).PadLeft(DataExpression.AccountTitleCodeLength, '0');
                        converted = value;
                    }
                }
                else if (DataExpression.AccountTitleCodeFormatString == "A9")
                {
                    if (!Regex.IsMatch(value, DigitAlphabetPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }
                    else
                    {
                        value = EbDataHelper.ConvertToUpperCase(value);
                    }
                }

                if (valid)
                {
                    def.SetValue(Model, converted);
                }
            }
            return valid;
        }

        public bool AccountTitleName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value)) // 入力があれば
            {
                if (value.Length > 40)
                {
                    value = value.Substring(0, 40);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool ContraAccountCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 10)
                {
                    valid = false;
                    LengthError(def, value, 10);
                }

                if (!Regex.IsMatch(value, DigitAlphabetPermissionPattern))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else
                {
                    value = EbDataHelper.ConvertToUpperCase(value);
                }

                if (valid)
                {
                    def.SetValue(Model, value);
                }
            }
            return valid;
        }

        public bool ContraAccountName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (def.Required || !string.IsNullOrEmpty(value)) // 入力があれば
                {
                    if (value.Length > 40)
                    {
                        value = value.Substring(0, 40);
                    }
                }

                if (valid)
                {
                    def.SetValue(Model, value);
                }
            }
            return valid;
        }

        public bool ContraAccountSubCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            string converted = value;
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (!Regex.IsMatch(value, DigitAlphabetPermissionPattern))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }

                if (value.Length > 10)
                {
                    valid = false;
                    LengthError(def, value, 10);
                }

                if (valid)
                {
                    def.SetValue(Model, converted);
                }
            }
            return valid;
        }

        public bool SourceBank(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 140)
                {
                    value = value.Substring(0, 140);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool SourceBranch(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            bool valid = true;
            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 15)
                {
                    value = value.Substring(0, 15);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool DepartmentName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 40)
                {
                    value = value.Substring(0, 40);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool DepartmentCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            //string converted = value;

            bool valid = true;
            if (def.Required || !string.IsNullOrEmpty(value))
            {
                value = EbDataHelper.ConvertToUpperCase(value);

                if (value.Length > DataExpression.DepartmentCodeLength)
                {
                    valid = false;
                    LengthError(def, value, DataExpression.DepartmentCodeLength);
                }

                if (DataExpression.DepartmentCodeFormatString == "9")
                {
                    if (!Regex.IsMatch(value, DigitPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }

                    if (value.Length < DataExpression.DepartmentCodeLength)
                    {
                        value = (value ?? string.Empty).PadLeft(DataExpression.DepartmentCodeLength, '0');
                    }
                }
                else if (DataExpression.DepartmentCodeFormatString == "A9")
                {
                    if (!Regex.IsMatch(value, DigitCharPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }
                }

                if (valid)
                {
                    def.SetValue(Model, value);
                }
            }
            return valid;
        }

        public bool DepartmentNote(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 100)
                {
                    value = value.Substring(0, 100);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool CurrencyCode(ForeignKeyFieldDefinition<TModel, int, Currency> def)
        {
            return true;
        }

        public bool BankCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (def.Required || !string.IsNullOrEmpty(value)) // 入力があれば
            {
                if (value.Length > 4)
                {
                    valid = false;
                    LengthError(def, value, 4);
                }
                else
                {
                    value = value.PadLeft(4, '0');
                }

                if (!Regex.IsMatch(value, DigitPermissionPattern))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }

            return valid;
        }

        public bool BankName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 30)
                {
                    value = value.Substring(0, 30);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool BranchCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 3)
                {
                    valid = false;
                    LengthError(def, value, 3);
                }
                else
                {
                    value = value.PadLeft(3, '0');
                }

                if (!Regex.IsMatch(value, DigitPermissionPattern))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }

            return valid;
        }

        public bool BranchName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 30)
                {
                    value = value.Substring(0, 30);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool AccountTypeId(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            int result = 0;
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, out result)
                    && (result.Equals(1) || result.Equals(2)
                        || result.Equals(4) || result.Equals(5)
                        || result.Equals(8)
                    ))
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, result);
            }

            return valid;
        }
        public bool AccountNumber(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (!Regex.IsMatch(value, DigitPermissionPattern))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }

                if (value.Length > 7)
                {
                    valid = false;
                    LengthError(def, value, 7);
                }
                else
                {
                    value = value.PadLeft(7, '0');
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }
        public bool UseValueDate(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            int result = 0;
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (!int.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result != 0 && result != 1)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, result);
            }
            return valid;
        }

        public bool SectionCode(ForeignKeyFieldDefinition<TModel, int, Section> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (!rateEmpty && (def.Required || !string.IsNullOrEmpty(value)))
            {
                if (DataExpression.SectionCodeFormatString == "9")
                {
                    if (value.Length < DataExpression.SectionCodeLength)
                    {
                        value = value.PadLeft(DataExpression.SectionCodeLength, '0');
                    }
                }
                else if (DataExpression.SectionCodeFormatString == "A9")
                {
                    value = EbDataHelper.ConvertToUpperCase(value);
                }

                if (valid && value.Length > DataExpression.SectionCodeLength)
                {
                    valid = false;
                    KeyNotExistsError(def, value);
                }

                if (valid)
                {
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
            }

            return valid;
        }

        public bool collectCategoryFlg = false;

        //14
        public bool CollectCategoryCode(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value == "00")
                {
                    collectCategoryFlg = true;
                }

                if (valid && value.Length > CategoryCodeLength)
                {
                    valid = false;
                    KeyNotExistsError(def, value);
                }

                if (valid)
                {
                    value = (value ?? string.Empty).PadLeft(CategoryCodeLength, '0');
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
                else
                {
                    KeyNotExistsError(def, value);
                }
            }

            return valid;
        }

        //38
        public bool LessThanCollectCategoryId(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (!ValidateFieldCount(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (collectCategoryFlg)
            {
                if (def.Required && !Required(def)) return false;

                if (def.Required || !string.IsNullOrEmpty(value))
                {
                    if (value == "00")
                    {
                        Reports.Add(new WorkingReport
                        {
                            LineNo = RecordCount,
                            FieldNo = def.FieldIndex,
                            FieldName = def.FieldName,
                            Value = value,
                            Message = "約定となっているため、インポートできません。",
                        });
                    }

                    if (valid && value.Length > CategoryCodeLength)
                    {
                        valid = false;
                        KeyNotExistsError(def, value);
                    }

                    if (valid)
                    {
                        value = (value ?? string.Empty).PadLeft(CategoryCodeLength, '0');
                        if (def.ModelHasCode) def.SetCode(Model, value);
                        CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                    }
                    else
                    {
                        KeyNotExistsError(def, value);
                    }
                }

            }

            return valid;
        }

        public bool receiveAccountId1NumberFlg = false;

        //39
        public bool GreaterThanCollectCategoryId1(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (!ValidateFieldCount(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (collectCategoryFlg)
            {
                if (def.Required && !Required(def)) return false;

                if (def.Required || !string.IsNullOrEmpty(value))
                {
                    receiveAccountId1NumberFlg = true;

                    if (value == "00")
                    {
                        Reports.Add(new WorkingReport
                        {
                            LineNo = RecordCount,
                            FieldNo = def.FieldIndex,
                            FieldName = def.FieldName,
                            Value = value,
                            Message = "約定となっているため、インポートできません。",
                        });
                    }

                    if (valid && value.Length > CategoryCodeLength)
                    {
                        valid = false;
                        KeyNotExistsError(def, value);
                    }

                    if (valid)
                    {
                        value = (value ?? string.Empty).PadLeft(CategoryCodeLength, '0');
                        if (def.ModelHasCode) def.SetCode(Model, value);
                        CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                    }
                    else
                    {
                        KeyNotExistsError(def, value);
                    }
                }
            }
            return valid;
        }

        public bool greaterThanCollectCategoryId2NumberFlag = false;
        //43
        public bool GreaterThanCollectCategoryId2(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (!ValidateFieldCount(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (receiveAccountId1NumberFlg)
            {
                if (def.Required || !string.IsNullOrEmpty(value))
                {
                    greaterThanCollectCategoryId2NumberFlag = true;

                    if (value == "00")
                    {
                        valid = false;
                        Reports.Add(new WorkingReport
                        {
                            LineNo = RecordCount,
                            FieldNo = def.FieldIndex,
                            FieldName = def.FieldName,
                            Value = value,
                            Message = "約定となっているため、インポートできません。",
                        });
                    }

                    if (valid && value.Length > CategoryCodeLength)
                    {
                        valid = false;
                        KeyNotExistsError(def, value);
                    }

                    if (valid)
                    {
                        value = (value ?? string.Empty).PadLeft(CategoryCodeLength, '0');
                        if (def.ModelHasCode) def.SetCode(Model, value);
                        CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                    }
                    else
                    {
                        KeyNotExistsError(def, value);
                    }
                }
            }
            return valid;
        }

        public bool greaterThanCollectCategoryId3NumberFlag = false;
        //47
        public bool GreaterThanCollectCategoryId3(ForeignKeyFieldDefinition<TModel, int, Category> def)
        {
            if (!ValidateFieldCount(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (greaterThanCollectCategoryId2NumberFlag)
            {
                if (def.Required || !string.IsNullOrEmpty(value))
                {
                    greaterThanCollectCategoryId3NumberFlag = true;

                    if (value == "00")
                    {
                        Reports.Add(new WorkingReport
                        {
                            LineNo = RecordCount,
                            FieldNo = def.FieldIndex,
                            FieldName = def.FieldName,
                            Value = value,
                            Message = "約定となっているため、インポートできません。",
                        });
                    }

                    if (valid && value.Length > CategoryCodeLength)
                    {
                        valid = false;
                        KeyNotExistsError(def, value);
                    }

                    if (valid)
                    {
                        value = (value ?? string.Empty).PadLeft(CategoryCodeLength, '0');
                        if (def.ModelHasCode) def.SetCode(Model, value);
                        CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                    }
                }
            }
            
            return valid;
        }

        public bool CustomerCodeForDiscount(ForeignKeyFieldDefinition<TModel, int, Customer> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            bool valid = true;
            if (def.Required || !string.IsNullOrEmpty(value))
            {
                value = EbDataHelper.ConvertToUpperCase(value);

                if (value.Length > DataExpression.CustomerCodeLength)
                {
                    valid = false;
                    LengthError(def, value, DataExpression.CustomerCodeLength);
                }

                if (DataExpression.CustomerCodeFormatString == "9")
                {
                    if (!Regex.IsMatch(value, DigitPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }

                    if (value.Length < DataExpression.CustomerCodeLength)
                    {
                        value = value.PadLeft(DataExpression.CustomerCodeLength, '0');
                    }
                }
                else if (DataExpression.CustomerCodeFormatString == "A9-")
                {
                    if (!Regex.IsMatch(value, CustomerCodeDigitCharPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }
                }
                else if (DataExpression.CustomerCodeFormatString == "KA9-")
                {
                    if (!Regex.IsMatch(value, KanaDigitCharPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }
                }

                if (valid)
                {
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }
            }
            return valid;
        }


        public bool MinValue(NumberFieldDefinition<TModel, decimal> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                decimal min = 1;
                decimal max = 99999999999;

                decimal result = 0;
                if (!decimal.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result < min || result > max)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }

                if (valid)
                    def.SetValue(Model, decimal.Parse(value));
            }
            return valid;
        }


        public bool Rate(NumberFieldDefinition<TModel, decimal> def)
        {
            if (!ValidateFieldCount(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                rateEmpty = false;
                decimal min = 0.1M;
                decimal max = 100;

                decimal result = 0;
                if (!decimal.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result < min || result > max)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }

                if (valid)
                    def.SetValue(Model, decimal.Parse(value));
            }
            else
            {
                rateEmpty = true;
            }
            return valid;
        }

        public bool RoundingMode(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (!rateEmpty && (def.Required && !Required(def))) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (!rateEmpty && (def.Required || !string.IsNullOrEmpty(value)))
            {
                int min = 0;
                int max = 3;

                int result = 0;
                if (!int.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result < min || result > max)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }

                if (valid)
                    def.SetValue(Model, int.Parse(value));
            }
            return valid;
        }


        public bool SubCode(StringFieldDefinition<TModel> def)
        {
            int subCodeLength = 10;

            if (!ValidateFieldCount(def)) return false;
            if (!rateEmpty && (def.Required && !Required(def))) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (!rateEmpty && (def.Required || !string.IsNullOrEmpty(value)))
            {
                if (value.Length > subCodeLength)
                {
                    valid = false;
                    LengthError(def, value, subCodeLength);
                }

                if (!Regex.IsMatch(value, DigitCharPermissionPattern))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }

                if (valid)
                    def.SetValue(Model, value);
            }
            return valid;
        }

        public bool CurrencyCodeForFee(ForeignKeyFieldDefinition<TModel, int, Currency> def, int foreignflg)
        {
            if (!ValidateFieldCount(def)) return false;

            string value = FieldValues[def.FieldNumber - 1];

            value = EbDataHelper.ConvertToUpperCase(value);

            if (foreignflg == 1)
            {
                if (def.Required && !Required(def)) return false;

                if (value.Length > CurrencyCodeLength)
                {
                    KeyNotExistsError(def, value);
                    return false;
                }
                else
                {
                    if (def.ModelHasCode) def.SetCode(Model, value);
                    CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
                }

            }
            else
            {
                value = "JPY";
                if (def.ModelHasCode) def.SetCode(Model, value);
                CodeResolutionQueue = StoreCode(CodeResolutionQueue, def, value);
            }

            return true;
        }

        public bool Fee(NullableNumberFieldDefinition<TModel, decimal> def, int foreignflg)
        {
            if (!ValidateFieldCount(def)) return false;

            string value = FieldValues[def.FieldNumber - 1];

            if (string.IsNullOrEmpty(value))
            {
                Reports.Add(new WorkingReport
                {
                    LineNo = RecordCount,
                    FieldNo = def.FieldNumber,
                    FieldName = def.FieldName,
                    Value = value,
                    Message = "空白のためインポートできません。",
                });
                return false;
            }
            else if ((foreignflg == 0) && (Convert.ToDecimal(value).ToString("###0.#####").Split('.').Length > 1))
            {
                Reports.Add(new WorkingReport
                {
                    LineNo = RecordCount,
                    FieldNo = def.FieldIndex,
                    FieldName = def.FieldName,
                    Value = value,
                    Message = "小数が含まれているためインポートできません。",
                });
                return false;
            }
            else
            {
                decimal parseValue;

                if (!decimal.TryParse(value, out parseValue) || !(parseValue >= DataExpression.MinCustomerFee && parseValue <= DataExpression.MaxCustomerFee))
                {
                    Reports.Add(new WorkingReport
                    {
                        LineNo = RecordCount,
                        FieldNo = def.FieldNumber,
                        FieldName = def.FieldName,
                        Value = value,
                        Message = $"不正な文字が入力されているためインポートできません。",
                    });
                    return false;
                }
                else
                {
                    def.SetValue(Model, parseValue);
                }
            }
            return true;
        }

        public bool JuridicalPersonalityKana(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            string Kana = EbDataHelper.ConvertToUpperCase(value);
            Kana = EbDataHelper.ConvertToHankakuKatakana(Kana);
            Kana = EbDataHelper.RemoveProhibitCharacter(Kana);
            Kana = EbDataHelper.ConvertProhibitCharacter(Kana);
            FieldValues[def.FieldIndex - 1] = Kana;

            if (def.Required && !Required(def)) return false;

            bool valid = true;
            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (EbDataHelper.ContainsKanji(Kana))
                {
                    InvalidCharError(def, Kana);
                    valid = false;
                }

                if (Kana.Length > 48)
                {
                    Kana = Kana.Substring(0, 48);
                }

                if (valid)
                {
                    def.SetValue(Model, Kana);
                }
            }
            return valid;
        }

        public bool HolidayCalendar(NumberFieldDefinition<TModel, DateTime> def)
        {
            // 必須
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            string[] formats = { "yyyy/MM/dd" , "yyyy/M/dd" , "yyyy/MM/d", "yyyy/M/d" };
            bool valid = true;
            DateTime result;
            var checkDate =DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (!checkDate)
                {
                    valid = false;
                    Reports.Add(new WorkingReport
                    {
                        LineNo = RecordCount,
                        FieldNo = def.FieldNumber,
                        FieldName = def.FieldName,
                        Value = value,
                        Message = $"フォーマットが異なるため、インポートできません。",
                    });
                }
                else if ((result.DayOfWeek == DayOfWeek.Saturday) || (result.DayOfWeek == DayOfWeek.Sunday))
                {
                    valid = false;
                    Reports.Add(new WorkingReport
                    {
                        LineNo = RecordCount,
                        FieldNo = def.FieldNumber,
                        FieldName = def.FieldName,
                        Value = value,
                        Message = $"土曜日、日曜日はインポートできません。",
                    });
                }
            }
            if (valid)
            {
                def.SetValue(Model, result);
            }
            return valid;
        }

        public bool SectionCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                value = EbDataHelper.ConvertToUpperCase(value);

                if (value.Length > DataExpression.SectionCodeLength)
                {
                    valid = false;
                    LengthError(def, value, DataExpression.SectionCodeLength);
                }

                if (DataExpression.SectionCodeFormatString == "9")
                {
                    if (!Regex.IsMatch(value, DigitPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }

                    if (value.Length < DataExpression.SectionCodeLength)
                    {
                        value = value.PadLeft(DataExpression.SectionCodeLength, '0');
                    }
                }
                else if (DataExpression.SectionCodeFormatString == "A9")
                {
                    if (!Regex.IsMatch(value, DigitAlphabetPermissionPattern))
                    {
                        valid = false;
                        InvalidCharError(def, value);
                    }
                }

                if (valid)
                {
                    def.SetValue(Model, value);
                }
            }

            return valid;
        }

        public bool SectionName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 40)
                {
                    value = value.Substring(0, 40);
                }
            }
            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool SectionNote(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 100)
                {
                    value = value.Substring(0, 100);
                }
            }
            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool BankKanaAndBankBranchKana(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 120)
                {
                    value = value.Substring(0, 120);
                }

                if (!Regex.IsMatch(value, $"^[{SymbolPermission} {DigitPermission}{UpperCasePermission}{LowerCasePermission}{HankakuKanaKPermission}]+$"))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                value = EbDataHelper.ConvertToHankakuKatakana(value);
                value = EbDataHelper.ConvertProhibitCharacter(value);
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool NameForBankBranchMaster(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > 120)
                {
                    value = value.Substring(0, 120);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        #region PB2101
        public bool CurrencyCode(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                value = EbDataHelper.ConvertToUpperCase(value);

                if (value.Length > CurrencyCodeLength)
                {
                    valid = false;
                    LengthError(def, value, CurrencyCodeLength);
                }

                if (!Regex.IsMatch(value, ConvertPattern(UpperCasePermission)))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
                def.SetValue(Model, value);

            return valid;
        }

        public bool CurrencyName(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;
            int subCodeLength = 40;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > subCodeLength)
                {
                    valid = false;
                    LengthError(def, value, subCodeLength);
                }
            }

            if (valid)
                def.SetValue(Model, value);

            return valid;
        }

        public bool CurrencySymbol(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;
            int subCodeLength = 1;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                if (value.Length > subCodeLength)
                {
                    valid = false;
                    LengthError(def, value, subCodeLength);
                }
            }

            if (valid)
                def.SetValue(Model, value);

            return valid;
        }

        public bool CurrencyPrecision(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def) || def.Required && !Required(def))
            {
                def.SetValue(Model, -1);
                return false;
            }

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                int result = 0;
                if (!int.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result < 0 || result > 5)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
            {
                def.SetValue(Model, int.Parse(value));
            }
            else
                def.SetValue(Model, -1);
            
            return valid;
        }

        public bool CurrencyDisplayOrder(NumberFieldDefinition<TModel, int> def)
        {
            if (!ValidateFieldCount(def)) return false;

            // 必須
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            bool valid = true;
            if (def.Required || !string.IsNullOrEmpty(value))
            {
                int result = 0;
                if (!int.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result < 0 || result > 999)
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
            }

            if (valid)
                def.SetValue(Model, int.Parse(value));

            return valid;
        }

        public bool CurrencyNote(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;

            bool valid = true;
            string value = FieldValues[def.FieldIndex - 1];

            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 100)
                {
                    value = value.Substring(0, 100);
                }
            }

            if (valid)
            {
                def.SetValue(Model, value);
            }
            return valid;
        }

        public bool CurrencyTolerance(NumberFieldDefinition<TModel, decimal> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];

            bool valid = true;

            if (def.Required || !string.IsNullOrEmpty(value))
            {
                decimal result = 0;
                if (!decimal.TryParse(value, out result))
                {
                    valid = false;
                    InvalidCharError(def, value);
                }
                else if (result < 0 || result > 9999.99999M)
                {
                    valid = false;
                    Reports.Add(new WorkingReport
                    {
                        LineNo = RecordCount,
                        FieldNo = def.FieldIndex,
                        FieldName = def.FieldName,
                        Value = value,
                        Message = $"手数料誤差金額は0～9999の範囲で入力してください。",
                    });
                }
            }
            if (valid)
            {
                def.SetValue(Model, decimal.Parse(value));
            }
            return valid;
        }

        #endregion

        public bool Tel(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if ((!string.IsNullOrEmpty(value)) && (!Regex.IsMatch(value, TelFaxPostNumberPermissionPattern)))
            {
                valid = false;
                InvalidCharError(def, value);
            }

            if (value.Length > 20)
            {
                valid = false;
                LengthError(def, value, 20);
            }

            if (valid)
                def.SetValue(Model, value);

            return valid;
        }

        public bool Fax(StringFieldDefinition<TModel> def)
        {
            if (!ValidateFieldCount(def)) return false;
            if (def.Required && !Required(def)) return false;

            string value = FieldValues[def.FieldIndex - 1];
            bool valid = true;

            if ((!string.IsNullOrEmpty(value)) && (!Regex.IsMatch(value, TelFaxPostNumberPermissionPattern)))
            {
                valid = false;
                InvalidCharError(def, value);
            }

            if (value.Length > 20)
            {
                valid = false;
                LengthError(def, value, 20);
            }

            if (valid)
                def.SetValue(Model, value);

            return valid;
        }

    }
}

