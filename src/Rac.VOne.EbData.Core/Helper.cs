using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;

namespace Rac.VOne.EbData
{
    public class Helper
    {
        /// <summary>非同期での動作を行うかどうか</summary>
        internal bool IsAsync { get; set; }


        //internal ILogin Login { get; set; }
        internal int Year { get; set; } = DateTime.Today.Year;

        public int CompanyId { get; set; }
        public int LoginUserId { get; set; }

        #region 文字列操作

        internal string ConvertToHankakuKatakana(string value)
        {
            return EbDataHelper.ConvertToHankakuKatakana(value);
        }
        internal string ConvertToValidEbCharacter(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = EbDataHelper.ConvertToValidEbKana(value);
            return value;
        }

        /// <summary>法人格情報削除処理</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal string RemoveLegalPersonality(string value)
        {
            return EbDataHelper.RemoveLegalPersonality(value, LegalPersonalities);
        }

        /// <summary>
        /// 与えられた文字列全てが有効な文字のみで構成されているかチェックする。
        /// 振込依頼人名 未入力も許可
        /// </summary>
        public bool IsValidEBChars(string value)
        {
            if (string.IsNullOrEmpty(value)) return true;
            return EbDataHelper.IsValidEBChars(value);
        }

        /// <summary>法人格除去用</summary>
        public IEnumerable<string> LegalPersonalities { get; set; }
        public Currency DefaultCurrency { get; set; }
        public Category DefaultReceiptCategory { get; set; }
        public ApplicationControl ApplicationControl { get; set; }

        public Action Initialize { get; set; }
        public Func<CancellationToken, Task> InitializeAsync { get; set; }

        /// <summary>文字列のバイト数取得
        /// <see cref="encoding"/>を指定しない場合は、<see cref="EbDataHelper.ShiftJis"/>を利用する</summary>
        internal int GetByteCount(string value, Encoding encoding = null)
            => (encoding ?? EbDataHelper.ShiftJis).GetByteCount(value);

        #endregion

        #region 日付変換関連

        private DateTimeFormatInfo jpCalendar;
        internal DateTimeFormatInfo JpCalendar
        {
            get
            {
                if (jpCalendar == null)
                {
                    var culture = new CultureInfo("ja-JP");
                    var format = culture.DateTimeFormat;
                    format.Calendar = new JapaneseCalendar();
                    jpCalendar = format;
                }
                return jpCalendar;
            }
        }
        private DateTimeFormatInfo defaultCalendar;
        internal DateTimeFormatInfo DefaultCalendar
        {
            get
            {
                if (defaultCalendar == null)
                {
                    var culture = new CultureInfo("ja-JP");
                    var format = culture.DateTimeFormat;
                    format.Calendar = new GregorianCalendar { CalendarType = GregorianCalendarTypes.Localized };
                    defaultCalendar = format;
                }
                return defaultCalendar;
            }
        }

        /// <summary>和暦変換処理</summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="styles"></param>
        /// <returns></returns>
        internal bool TryParseJpDateTime(string value, out DateTime result, DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces)
        {
            const string DateFormat = "ggyy/MM/dd";
            if (value.Length == 6)
                value = value.Insert(4, "/").Insert(2, "/");

            value = DateTime.Today.ToString("gg", JpCalendar) + value;
            return TryParseDateTime(value, out result,
                DateFormat, JpCalendar, styles);
        }

        /// <summary>日付変換処理
        /// 標準 format 対応 → yyyy/M/d, yyyy.M.d, yyyyMMdd
        /// 時分秒を含む場合は、別途 dateFormat を指定すること
        /// 和暦の場合は、wrapした別メソッド TryParseJpDateTime の利用を推奨
        /// </summary>
        /// <param name="value">日付形式の文字列</param>
        /// <param name="result">値を設定したい 日付型変数</param>
        /// <param name="dateFormat">日付書式指定文字列</param>
        /// <param name="provider">日付フォーマットを指定する IFormatProvider
        /// 指定しない場合 グレゴリオ暦 となる</param>
        /// <param name="styles">日付のスタイル</param>
        /// <returns></returns>
        internal bool TryParseDateTime(string value, out DateTime result,
            string dateFormat = null, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces)
        {
            var formats = string.IsNullOrEmpty(dateFormat)
                ? new[] {
                    "yyyy/M/d",
                    "yyyy.M.d",
                    "yyyyMMdd",
                } : new[] {
                    dateFormat
                };
            return DateTime.TryParseExact(value,
                formats,
                provider ?? DefaultCalendar,
                styles,
                out result);
        }

        /// <summary>日付変換処理 年未指定のもの 月,日を文字列で別々に連携</summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        internal bool TryParseDateTimeAnser(string month, string day, out DateTime result)
        {
            result = default(DateTime);

            var imonth = 0;
            var iday = 0;
            if (!int.TryParse(month, out imonth)
                || !int.TryParse(day, out iday)) return false;
            if (imonth < 1 || 12 < imonth
                || iday < 1 || 31 < iday
                || DateTime.DaysInMonth(Year, imonth) < iday) return false;
            var parseResult = false;
            try
            {
                result = new DateTime(Year, imonth, iday);
                parseResult = true;
            }
            catch
            {
            }
            return parseResult;
        }

        #endregion

        public CollationSetting CollationSetting;

        /// <summary>振分機能 利用可否</summary>
        internal bool UseApportion
        {
            get
            {
                return CollationSetting?.UseApportionMenu == 1;
            }
        }

        internal bool RemoveSpaceFromPayerName
        {
            get
            {
                return CollationSetting?.RemoveSpaceFromPayerName == 1;
            }
        }


        /// <summary>銀行コード,支店コード,預金種別,口座番号 から 銀行口座を取得する 同期メソッド</summary>
        public Func<string, string, int, string, BankAccount> GetBankAccount { get; set; }
        /// <summary>銀行コード,支店コード,預金種別,口座番号 から 銀行口座を取得する 非同期メソッド</summary>
        public Func<string, string, int, string, CancellationToken, Task<BankAccount>> GetBankAccountAsync { get; set; }

        /// <summary>銀行名,支店名,預金種別,口座番号 から 銀行口座を取得する 同期メソッド</summary>
        public Func<string, string, int, string, BankAccount> GetBankAccountByBankName { get; set; }

        /// <summary>銀行名,支店名,預金種別,口座番号 から 銀行口座を取得する 非同期メソッド</summary>
        public Func<string, string, int, string, CancellationToken, Task<BankAccount>> GetBankAccountByBankNameAsync { get; set; }

        /// <summary>銀行コード,支店名 から 銀行口座を取得する 同期メソッド</summary>
        public Func<string, string, BankAccount> GetBankAccountByBranchName { get; set; }

        /// <summary>銀行コード,支店名 から 銀行口座を取得する 非同期メソッド</summary>
        public Func<string, string, CancellationToken, Task<BankAccount>> GetBankAccountByBranchNameAsync { get; set; }

        /// <summary>銀行コード、支店名、預金種目、口座番号 から 銀行口座を取得する 同期メソッド</summary>
        public Func<string, string, int, string, BankAccount> GetBankAccountByBranchNameAndNumber { get; set; }

        /// <summary>銀行コード、支店名、預金種目、口座番号 から 銀行口座を取得する 非同期メソッド</summary>
        public Func<string, string, int, string, CancellationToken, Task<BankAccount>> GetBankAccountByBranchNameAndNumberAsync { get; set; }

        internal int GetAccountTypeIdByName(string accountTypeName)
        {
            if (accountTypeName?.IndexOf("普通") >= 0) return 1;
            if (accountTypeName?.IndexOf("当座") >= 0) return 2;
            if (accountTypeName?.IndexOf("貯蓄") >= 0) return 4;
            if (accountTypeName?.IndexOf("通知") >= 0) return 5;
            return 0;
        }

        internal string GetAccountTypeNameById(int accountTypeId)
        {
            if (accountTypeId == 1) return "普通";
            if (accountTypeId == 2) return "当座";
            if (accountTypeId == 4) return "貯蓄";
            if (accountTypeId == 5) return "通知";
            return string.Empty;
        }

        /// <summary>銀行コード、支店コード、振込依頼人コード から 得意先ID を取得する 同期メソッド</summary>
        public Func<string, string, string, int?> GetCustomerIdByExclusiveInfo { get; set; }
        /// <summary>銀行コード、支店コード、振込依頼人コード から 得意先ID を取得する 非同期メソッド</summary>
        public Func<string, string, string, CancellationToken, Task<int?>> GetCustomerIdByExclusiveInfoAsync { get; set; }


        /// <summary>振込依頼人コード から 入金部門ID を取得する 同期メソッド</summary>
        public Func<string, int?> GetSectionIdByPayerCode { get; set; }

        /// <summary>振込依頼人コード から 入金部門ID を取得する 非同期メソッド</summary>
        public Func<string, CancellationToken, Task<int?>> GetSectionIdByPayerCodeAsync { get; set; }

        /// <summary>振込依頼人名 から 対象外区分ID を取得する 同期メソッド</summary>
        public Func<string, int?> GetExcludeCategoryId { get; set; }

        /// <summary>振込依頼人名 から 対象外区分ID を取得する 非同期メソッド</summary>
        public Func<string, CancellationToken, Task<int?>> GetExcludeCategoryIdAsync { get; set; }

        /// <summary>EBデータ取込対象外口座設定 取得 同期メソッド</summary>
        public Func<List<EBExcludeAccountSetting>> GetEBExcludeAccountSettingList { get; set; }
        /// <summary>EBデータ取込対象外口座設定 取得 非同期メソッド</summary>
        public Func<CancellationToken, Task<List<EBExcludeAccountSetting>>> GetEBExcludeAccountSettingListAsync { get; set; }

        /// <summary>データ登録処理 同期メソッド</summary>
        public Func<List<ImportFileLog>, List<ImportFileLog>> SaveDataInner { get; set; }
        /// <summary>データ登録処理 非同期メソッド</summary>
        public Func<List<ImportFileLog>, CancellationToken, Task<List<ImportFileLog>>> SaveDataInnerAsync { get; set; }

        public List<ImportFileLog> SaveData(List<ImportFileLog> logs)
        {
            logs = PrepareSaveData(logs);
            return SaveDataInner(logs);
        }

        public Task<List<ImportFileLog>> SaveDataAsync(List<ImportFileLog> logs, CancellationToken token = default(CancellationToken))
        {
            logs = PrepareSaveData(logs);
            return SaveDataInnerAsync(logs, token);
        }

        private List<ImportFileLog> PrepareSaveData(List<ImportFileLog> logs)
        {
            foreach (var log in logs)
                foreach (var header in log.ReceiptHeaders)
                    foreach (var detail in header.Receipts)
                    {
                        if (RemoveSpaceFromPayerName)
                            detail.PayerName = detail.PayerName.Replace(" ", "");

                        detail.CollationKey = Regex.Replace(detail.PayerName, "[^0-9]", "");
                        detail.BankCode = header.BankCode;
                        detail.BankName = header.BankName;
                        detail.BranchCode = header.BranchCode;
                        detail.BranchName = header.BranchName;
                        detail.AccountTypeId = header.AccountTypeId;
                        detail.AccountNumber = header.AccountNumber;
                        detail.AccountName = header.AccountName;
                    }
            return logs;
        }


        internal void Dispose()
        {
            CollationSetting        = null;
            ApplicationControl      = null;
            DefaultCurrency         = null;
            DefaultReceiptCategory  = null;
            LegalPersonalities      = null;
        }

        #region 銀行情報文字列検証

        internal bool ValidateBankCode(ref string bankCode)
        {
            return ValidateNumberCode(ref bankCode, 4);
        }
        internal bool ValidateBranchCode(ref string branchCode)
        {
            return ValidateNumberCode(ref branchCode, 3);
        }
        internal bool ValidateAccountNumber(ref string accountNumber)
        {
            return ValidateNumberCode(ref accountNumber, 7);
        }
        internal bool ValidatePayerCode(ref string payerCode)
        {
            if (string.IsNullOrEmpty(payerCode)) return true;
            payerCode = ConvertToValidEbCharacter(payerCode).Right(10, '0', true);
            return ValidateInvalidateNumberChar(payerCode);
        }

        private bool ValidateNumberCode(ref string code, int length)
        {
            if (string.IsNullOrEmpty(code)) return false;
            code = ConvertToValidEbCharacter(code).Right(length, '0', true);
            return ValidateInvalidateNumberChar(code);
        }
        private bool ValidateInvalidateNumberChar(string val)
        {
            return Regex.IsMatch(val, string.Format("^[0-9]{{{0}}}$", GetByteCount(val)));
        }

        #endregion
    }
}
