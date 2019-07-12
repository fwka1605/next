using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class GridSetting : IByCompany
    {

        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int GridId { get; set; }
        [DataMember] public string ColumnName { get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        [DataMember] public int DisplayWidth { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string ColumnNameJp { get; set; }
        [DataMember] public bool AllLoginUser { get; set;}
    }

    [DataContract]
    public class GridSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public GridSetting GridSetting { get; set; }
    }

    [DataContract]
    public class GridSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<GridSetting> GridSettings { get; set; }
    }
}
