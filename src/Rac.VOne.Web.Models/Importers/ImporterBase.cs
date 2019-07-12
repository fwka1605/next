using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Importers
{
    public abstract class ImporterBase<TModel>
        where TModel: class, new()
    {

        /// <summary>例外エラーログの出力</summary>
        public Action<Exception> LogError { get; set; }
        public Func<string, Task<ColumnNameSetting[]>> LoadColumnNameSettingsInnerAsync { get; set; }


        public ImporterBase(ApplicationControl applicationControl)
        {
            //Login = login;
            ApplicationControl = applicationControl;
        }

        #region 処理実行時に必要な情報
        public virtual string FilePath { protected get; set; }
        public int ImporterSettingId { protected get; set; }
        //protected ILogin Login { get; }
        protected ApplicationControl ApplicationControl { get; }
        #endregion

        #region 処理実行後の状態確認用
        public int ReadCount { get; protected set; }
        public int ValidCount { get; protected set; }
        public int InvalidCount { get; protected set; }
        public int SaveCount { get; protected set; }
        public decimal SaveAmount { get; protected set; }
        public bool DoOverWrite { get; protected set; }
        #endregion

        //protected string SessionKey { get { return Login?.SessionKey; } }
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public int LoginUserId { get; set; }

        protected bool UseSection { get { return ApplicationControl?.UseReceiptSection == 1; } }
        protected bool UseForeignCurrency { get { return ApplicationControl?.UseForeignCurrency == 1; } }
        protected bool UseDiscount { get { return ApplicationControl?.UseDiscount == 1; } }


        protected List<WorkingReport> ErrorReport { get; set; } = new List<WorkingReport>();

        protected List<TModel> PossibleData { get; set; }   = new List<TModel>();
        protected List<TModel> ImpossibleData { get; set; } = new List<TModel>();

        protected ImporterSetting ImporterSetting { get; set; } = new ImporterSetting();
        protected List<ImporterSettingDetail> ImporterSettingDetail { get; set; } = new List<ImporterSettingDetail>();

        protected ImporterSettingDetail GetSettingDetail(int field,
            IEnumerable<ImporterSettingDetail> source = null)
           => (source ?? ImporterSettingDetail)?.FirstOrDefault(x => x.Sequence == field);

        protected string Note1 { get; set; }
        protected string Note2 { get; set; }
        protected string Note3 { get; set; }
        protected string Note4 { get; set; }
        protected string Note5 { get; set; }
        protected string Note6 { get; set; }
        protected string Note7 { get; set; }
        protected string Note8 { get; set; }


        protected Func<string, DateTime?> CreateFormatter(ImporterSettingDetail detail)
        {
            if (detail == null || !detail.AttributeDivision.HasValue) return null;

            var attributeDivision = detail.AttributeDivision.Value;
            var provider = CultureInfo.CurrentCulture.DateTimeFormat.Clone() as DateTimeFormatInfo;
            var isJapaneseCalendar = (attributeDivision == 5 || attributeDivision == 6);
            if (isJapaneseCalendar)
                provider.Calendar = new JapaneseCalendar();

            var format = "";
            var pattern = "";
            switch (attributeDivision)
            {
                case 1: /* yyyy/MM/dd */
                case 3: /* yyyyMMdd   */
                    format = "yyyy/M/d"; pattern = @"^\d{4}/\d{1,2}/\d{1,2}"; break;
                case 2: /* yy/MM/dd   */
                case 4: /* yyMMdd     */
                case 5: /* gg/MM/dd   */
                case 6: /* ggMMdd     */
                    format = "yy/M/d"; pattern = @"^\d{2}/\d{1,2}/\d{1,2}"; break;
            }

            if (isJapaneseCalendar)
                format = $"gg{format}";
            return value =>
            {
                value = value.Replace("-", "/");
                if (attributeDivision == 3
                 || attributeDivision == 4
                 || attributeDivision == 6)
                {
                    var offset = attributeDivision == 3 ? 2 : 0;
                    if (value.Length == offset + "yyMMdd".Length)
                        value = value
                            .Insert(offset +  4, "/")
                            .Insert(offset +  2, "/");
                    if (value.Length == offset + "yyMMddHHmmss".Length)
                        value = value
                            .Insert(offset + 10, ":")
                            .Insert(offset +  8, ":")
                            .Insert(offset +  6, " ")
                            .Insert(offset +  4, "/")
                            .Insert(offset +  2, "/");
                    if (value.Length >= offset + "yyMMddHHmmssf".Length)
                        value = value
                            .Insert(offset + 12, ".")
                            .Insert(offset + 10, ":")
                            .Insert(offset +  8, ":")
                            .Insert(offset +  6, " ")
                            .Insert(offset +  4, "/")
                            .Insert(offset +  2, "/");
                }
                pattern += @"(\s[0-9]{1,2}:[0-9]{1,2}:[0-9]{1,2}(\.[0-9]{1,3})?)?$";
                if (!Regex.IsMatch(value, pattern)) return null;
                DateTime result;
                var times = new string[] { "", " H:m:s", " H:m:s.f", " H:m:s.ff", " H:m:s.fff" };
                var formats = times.Select(x => format + x).ToArray();
                if (isJapaneseCalendar)
                    value = DateTime.Today.ToString("gg", provider) + value;
                if (DateTime.TryParseExact(value, formats, provider, DateTimeStyles.AssumeLocal, out result)) return result;
                return null;
            };
        }
        /// <summary>配列に fieldIndex が存在するか確認する処理</summary>
        /// <param name="fieldIndex"></param>
        /// <param name="valueLength"></param>
        protected bool IsExist(int fieldIndex, int valueLength) => fieldIndex < valueLength;

        /// <summary>金額などの判定 正負符合（任意）、小数点以下（任意） ^[-+]?[0-9]+(\.[0-9]+)?$</summary>
        protected bool IsMoney(string value) => Regex.IsMatch(value, @"^[-+]?[0-9]+(\.[0-9]+)?$");

        /// <summary>数字かどうかの判定 ^[0-9]+$</summary>
        protected bool IsNumber(string value) => Regex.IsMatch(value, @"^[0-9]+$");

        /// <summary>英数字かどうかの判定 ^[a-zA-Z0-9]+$</summary>

        protected bool IsNumberAlphabet(string value) => Regex.IsMatch(value, @"^[a-zA-Z0-9]+$");

        /// <summary>得意先コード用 英数 ハイフン 要求で アンダーバーも追加する必要あり</summary>
        protected bool IsCustomerCodeNumberAlphabet(string value) => Regex.IsMatch(value, @"^[-a-zA-Z0-9]+$");

        /// <summary>得意先コード用 半角カナ
        /// TODO : 他画面で利用している ユニコード判定にするか検討</summary>
        protected bool IsCustomerCodeKana(string value) => Regex.IsMatch(value, @"^[-ｧ-ﾝﾞﾟァ-ンa-zA-Z0-9]+$");

        /// <summary>英字かどうかの判定 ^[a-zA-Z]+$ ほぼ通貨コード専用</summary>
        protected bool IsAlphabet(string value) => Regex.IsMatch(value, @"^[a-zA-Z]+$");

        protected bool ValidateEmpty(string value) => !string.IsNullOrWhiteSpace(value);
        protected bool ValidateLength(string value, int length) => ((value?.Trim())?.Length ?? 0) <= length;

        /// <summary>
        /// 固定長マルチバイトのPadding
        /// </summary>
        /// <param name="encoding">shift-jis を想定</param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns>
        /// value "abc", length 10
        /// abc_______
        /// value "EDI情報", length 10
        /// EDI情報___
        /// * _ は 半角スペースの代替
        /// </returns>
        protected string PadRightMultiByte(Encoding encoding, string value, int length)
        {
            value = value ?? string.Empty;
            var byteCount = encoding.GetByteCount(value);
            if (length < byteCount)
            {
                value = new string(value.TakeWhile((c, i) =>
                    encoding.GetByteCount(value.Substring(0, i + 1)) <= length).ToArray());
                byteCount = encoding.GetByteCount(value);
            }

            return value.PadRight(length - (byteCount - value.Length));
        }

        public virtual bool MoveFile()
        {
            var result = false;
            try
            {
                var PostAction = ImporterSetting.PostAction;
                if (PostAction == 1)
                {
                    var fileInfo = new FileInfo(FilePath);
                    try
                    {
                        fileInfo.Delete();
                    }
                    catch (IOException  /*ex*/)
                    {
                        //Debug.WriteLine(ex.Message);
                    }
                }
                else if (PostAction == 2)
                {
                    var nameBase = string.Empty;
                    var fileExtPos = FilePath.LastIndexOf(".");
                    if (fileExtPos >= 0)
                        nameBase = FilePath.Substring(0, fileExtPos);
                    else
                        nameBase = FilePath;
                    var newfilename = $"{nameBase}{DateTime.Now:yyyyMMddHHmmss}.csv";
                    System.IO.File.Copy(FilePath, newfilename);
                    System.IO.File.Delete(FilePath);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogError?.Invoke(ex);
            }
            return result;
        }
        protected virtual bool WriteErrorLog(string path, string importDataName)
        {
            var writeResult = false;
            try
            {
                System.IO.File.AppendAllText(path, $"{DateTime.Now:yyyy年MM月dd日 HH時mm分ss秒}{Environment.NewLine}");
                System.IO.File.AppendAllText(path, $"{importDataName}：{Path.GetFileName(FilePath)}{Environment.NewLine}");
                System.IO.File.AppendAllText(path, string.Join(Environment.NewLine, GetValidationLogs()));
                System.IO.File.AppendAllText(path, Environment.NewLine);
                writeResult = true;
            }
            catch (Exception ex)
            {
                LogError?.Invoke(ex);
                //NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return writeResult;
        }

        public virtual List<string> GetValidationLogs()
        {
            var encoding = Encoding.GetEncoding(932);
            return ErrorReport.OrderBy(x => x.LineNo).ThenBy(x => x.FieldNo).Select(x => {
                var fieldName = PadRightMultiByte(encoding, x.FieldName, 30);
                return $"{x.LineNo:D8}行目  {fieldName}{x.Message}";
            }).ToList();
        }


        protected async Task LoadColumnNameSettingAsync(string tableName)
        {
            var settings = (await LoadColumnNameSettingsInnerAsync(tableName)).ToArray();
            foreach (var setting in settings)
            {
                if (setting.ColumnName == nameof(Note1)) Note1 = setting.DisplayColumnName;
                if (setting.ColumnName == nameof(Note2)) Note2 = setting.DisplayColumnName;
                if (setting.ColumnName == nameof(Note3)) Note3 = setting.DisplayColumnName;
                if (setting.ColumnName == nameof(Note4)) Note4 = setting.DisplayColumnName;
                if (setting.ColumnName == nameof(Note5)) Note5 = setting.DisplayColumnName;
                if (setting.ColumnName == nameof(Note6)) Note6 = setting.DisplayColumnName;
                if (setting.ColumnName == nameof(Note7)) Note7 = setting.DisplayColumnName;
                if (setting.ColumnName == nameof(Note8)) Note8 = setting.DisplayColumnName;
            }
        }

    }
}
