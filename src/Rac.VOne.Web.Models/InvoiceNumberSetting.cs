using System;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class InvoiceNumberSetting : IByCompany
    {
        ///<summary>会社ID</summary>
        [DataMember] public int CompanyId { get; set; }
        ///<summary>採番をおこなう</summary>
        [DataMember] public int UseNumbering { get; set; }
        ///<summary>桁数</summary>
        [DataMember] public int Length { get; set; }
        ///<summary>前ゼロ設定</summary>
        [DataMember] public int ZeroPadding { get; set; }
        ///<summary>連番リセット</summary>
        [DataMember] public int ResetType { get; set; }
        ///<summary>リセット月</summary>
        [DataMember] public int? ResetMonth { get; set; }
        ///<summary>採番書式設定</summary>
        [DataMember] public int FormatType { get; set; }
        ///<summary>使用日付</summary>
        [DataMember] public int? DateType { get; set; }
        ///<summary>年月日フォーマット</summary>
        [DataMember] public int? DateFormat { get; set; }
        ///<summary>固定文字列設定</summary>
        [DataMember] public int? FixedStringType { get; set; }
        ///<summary>固定文字列</summary>
        [DataMember] public string FixedString { get; set; } = string.Empty;
        ///<summary>書式の配置</summary>
        [DataMember] public int DisplayFormat { get; set; }
        ///<summary>区切文字</summary>
        [DataMember] public string Delimiter { get; set; } = string.Empty;
        ///<summary>登録者ID</summary>
        [DataMember] public int CreateBy { get; set; }
        ///<summary>登録日時</summary>
        [DataMember] public DateTime CreateAt { get; set; }
        ///<summary>更新者ID</summary>
        [DataMember] public int UpdateBy { get; set; }
        ///<summary>更新日時</summary>
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class InvoiceNumberSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public InvoiceNumberSetting InvoiceNumberSetting { get; set; }
    }
}
