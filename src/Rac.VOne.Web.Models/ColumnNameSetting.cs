using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ColumnNameSetting : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string TableName { get; set; }
        [DataMember] public string ColumnName { get; set; }
        [DataMember] public string OriginalName { get; set; }
        [DataMember] public string Alias { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        public string TableNameText => TableName == nameof(Billing) ? "請求" : "入金";
        public string DisplayColumnName => !string.IsNullOrEmpty(Alias) ? Alias : OriginalName;
    }

    [DataContract]
    public class ColumnNameSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ColumnNameSetting ColumnName { get; set; }
    }

    [DataContract]
    public class ColumnNameSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ColumnNameSetting> ColumnNames { get; set; }
    }
}
