using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>EBファイル設定</summary>
    [DataContract] public class EBFileSetting : IIdentical, IByCompany
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        /// <summary>利用可否</summary>
        [DataMember] public int IsUseable { get; set; }
        /// <summary>EBフォーマットID</summary>
        [DataMember] public int EBFormatId { get; set; }
        /// <summary>ファイルフィールド形式
        /// 1: カンマ区切り, 2:タブ区切り, 3:固定長（改行あり）, 4:固定長（改行なし）</summary>
        [DataMember] public int FileFieldType { get; set; }
        [DataMember] public string BankCode { get; set; } = string.Empty;
        [DataMember] public int UseValueDate { get; set; }
        [DataMember] public string ImportableValues { get; set; }
        [DataMember] public string FilePath { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        #region EBFormat column

        [DataMember] public int RequireYear { get; set; }

        #endregion

        public string FileFieldTypeName
            => FileFieldType == 1 ? "カンマ区切"
             : FileFieldType == 2 ? "タブ区切"
             : FileFieldType == 3 ? "固定長"
             : FileFieldType == 4 ? "固定長（改行なし）" : string.Empty;

    }

    [DataContract] public class EBFileSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public EBFileSetting EBFileSetting { get; set; }
    }

    [DataContract] public class EBFileSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<EBFileSetting> EBFileSettings { get; set; }
    }

}
