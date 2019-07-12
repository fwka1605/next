using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// フリーインポーター 設定のヘッダー情報
    /// </summary>
    [DataContract]
    public class ImporterSetting : IIdentical
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        /// <summary>
        /// フリーインポーター フォーマット種別
        /// 1: 請求, 2:入金, 3:入金予定, 4:得意先
        /// MasterImportSetting と異なるので注意
        /// </summary>
        [DataMember] public int FormatId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string InitialDirectory { get; set; }
        [DataMember] public int EncodingCodePage { get; set; }
        [DataMember] public int StartLineCount { get; set; }
        [DataMember] public int IgnoreLastLine { get; set; }
        [DataMember] public int AutoCreationCustomer { get; set; }
        /// <summary>
        /// 取込後の処理 0:何もしない, 1:削除, 2:取込日時を付与
        /// </summary>
        [DataMember] public int PostAction { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string FieldName { get; set; }
        [DataMember] public int ImportDivisions { get; set; }
        [DataMember] public int Sequence { get; set; }
        [DataMember] public int FieldIndex { get; set; }
        [DataMember] public string Caption { get; set; }
        [DataMember] public int AttributeDivisions { get; set; }
        [DataMember] public int ItemPriority { get; set; }
        [DataMember] public DateTime DetailUpdateAt { get; set; }
        [DataMember] public int IsUnique { get; set; }
        [DataMember] public string FixedValue { get; set; }

        [DataMember] public List<ImporterSettingDetail> Details { get; set; }
    }

    [DataContract]
    public class ImporterSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ImporterSetting ImporterSetting { get; set; }
    }

    [DataContract]
    public class ImporterSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ImporterSetting> ImporterSettings { get; set; }
    }

    [DataContract]
    public class ImporterSettingAndDetailResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ImporterSetting ImporterSetting { get; set; }
        [DataMember] public ImporterSettingDetail[] ImporterSettingDetail { get; set; }
    }
}