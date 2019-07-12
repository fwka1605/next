using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ExportFieldSetting
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int ExportFileType { get; set; }
        [DataMember] public string ColumnName { get; set; }
        [DataMember] public int ColumnOrder { get; set; }
        [DataMember] public int AllowExport { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string Caption { get; set; }
        /// <summary>項目のデータ型
        /// 0 : 通常の出力項目
        /// 1 : 日付型出力項目
        /// 2 : フィールド系出力項目ではない設定 ヘッダー出力有無など</summary>
        [DataMember] public int DataType { get; set; }
        [DataMember] public int DataFormat { get; set; }

        /// <summary>通常フィールド項目か否か</summary>
        public bool IsStandardField => DataType == 0 || DataType == 1;

        /// <summary>日付型 書式文字列
        /// DataFormat 0 : yyyy/MM/dd
        /// DataFormat 1 : yy/MM/dd
        /// DataFormat 2 : yyyyMMdd
        /// DataFormat 3 : yyMMdd
        /// </summary>
        public string DateFormat
        {
            get
            {
                if (DataType != 1) return string.Empty;
                if (DataFormat == 0) return "yyyy/MM/dd";
                if (DataFormat == 1) return "yy/MM/dd";
                if (DataFormat == 2) return "yyyyMMdd";
                if (DataFormat == 3) return "yyMMdd";
                return string.Empty;
            }
        }
    }

    [DataContract]
    public class ExportFieldSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ExportFieldSetting> ExportFieldSettings { get; set; }
    }
}
