using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class PdfOutputSetting : IMasterData, ISynchronization
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int ReportType { get; set; }
        [DataMember] public int OutputUnit { get; set; }
        /// <summary>まとめて出力する</summary>
        public bool IsAllInOne =>
            OutputUnit == (int)PdfOutputSettingOutputUnit.AllInOne;
        [DataMember] public string FileName { get; set; } = "";
        [DataMember] public int UseCompression { get; set; }
        /// <summary>ZIP化を行う</summary>
        public bool UseZip => UseCompression != 0;
        /// <summary>ZIPファイル最大バイト数 単位:MByte</summary>
        [DataMember] public decimal? MaximumSize { get; set; }
        /// <summary>ZIPファイル最大バイト数 単位:Byte</summary>
        public long? MaximumByte
        {
            get
            {
                if (MaximumSize.HasValue)
                    return (long)(1000000m * MaximumSize.Value);
                else
                    return null;
            }
            set
            {
                if (value == null)
                    MaximumSize = null;
                else
                    MaximumSize = ((decimal)value) / 1000000m;
            }
        }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class PdfOutputSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public PdfOutputSetting PdfOutputSetting { get; set; }
    }
    public enum PdfOutputSettingReportType
    {
        /// <summary>請求書</summary>
        Invoice = 0,
        /// <summary>督促状</summary>
        Reminder = 1,
    }

    /// <summary>
    /// 出力単位
    /// </summary>
    public enum PdfOutputSettingOutputUnit
    {
        /// <summary>まとめる</summary>
        AllInOne = 0,
        /// <summary>帳票ごと</summary>
        ByReport = 1,
    }
}
