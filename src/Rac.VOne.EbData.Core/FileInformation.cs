using Rac.VOne.Web.Models;
using System.Linq;

namespace Rac.VOne.EbData
{
    /// <summary>EBデータ取込ファイル情報</summary>
    public class FileInformation
    {
        #region 取込処理に必要な情報
        /// <summary>ファイルパス</summary>
        /// <remarks>フルパス</remarks>
        public string Path { get; set; }
        /// <summary>web api で利用</summary>
        public string File { get; set; }

        /// <summary>ファイルサイズ readonly</summary>
        public int Size { get; internal set; }

        /// <summary>取込ファイルフォーマット</summary>
        public EbFileFormat Format { get; set; }

        /// <summary>ファイル/フィールド 区切り指定</summary>
        public FileFieldType FileFieldType { get; set; }

        /// <summary>銀行コード</summary>
        public string BankCode { get; set; }

        /// <summary>任意の取込区分 フォーマット毎に指定方法が異なる
        /// 
        /// </summary>
        public string ImportableValue { get; set; }
        /// <summary>起算日を入金日として利用</summary>
        public bool UseValueDate { get; set; }

        #endregion

        #region 読込/取込結果連携用

        /// <summary>読込・取込結果</summary>
        public ImportResult Result { get; set; }
        /// <summary>GridのIndex など管理用</summary>
        public int Index { get; set; }
        /// <summary>読込・登録後のデータ</summary>
        public ImportFileLog ImportFileLog { get; set; }
        /// <summary>不足している銀行口座情報</summary>
        public string BankInformation { get; set; }

        #endregion
        /// <summary>カンマ区切り 取込区分等を文字列の配列にして返す</summary>
        /// <returns></returns>
        public string[] GetImportableValues()
        {
            if (string.IsNullOrEmpty(ImportableValue)
                || !ImportableValue.Contains(",")) return new string[] { };
            return ImportableValue.Trim(',').Split(',').Select(x => x.Trim()).ToArray();
        }

        public FileInformation() { }
        public FileInformation(int index, string path, EBFileSetting setting)
        {
            Index           = index;
            Path            = path;
            Format          = (EbFileFormat)setting.EBFormatId;
            FileFieldType   = (FileFieldType)setting.FileFieldType;
            BankCode        = setting.BankCode;
            ImportableValue = setting.ImportableValues;
            UseValueDate    = setting.UseValueDate == 1;
        }


        public FileInformation (EbFileInformation model, int index = 0)
        {
            Index           = index;
            Path            = model.FilePath;
            File            = model.File;
            Format          = (EbFileFormat)model.Format;
            FileFieldType   = (FileFieldType)model.FileFieldType;
            BankCode        = model.BankCode;
            ImportableValue = model.ImportableValue;
            UseValueDate    = model.UseValueDate;
        }

        public EbFileInformation ConvertToModel() => new EbFileInformation {
            Index           = Index,
            FilePath        = Path,
            Format          = (int)Format,
            FileFieldType   = (int)FileFieldType,
            BankCode        = BankCode,
            ImportableValue = ImportableValue,
            UseValueDate    = UseValueDate,
            Result          = (int)Result,
            ImportFileLog   = ImportFileLog,
            BankInformation = BankInformation,
        };
    }
}
