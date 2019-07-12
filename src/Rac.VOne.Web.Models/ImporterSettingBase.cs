using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ImporterSettingBase
    {
        [DataMember] public int FormatId { get; set; }
        [DataMember] public int Sequence { get; set; }
        [DataMember] public string FieldName { get; set; }
        [DataMember] public string TargetColumn { get; set; }
        [DataMember] public int ImportDivision { get; set; }
        [DataMember] public int AttributeDivision { get; set; }
    }

    [DataContract]
    public class ImporterSettingBaseResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ImporterSettingBase[] ImporterSettngBase { get; set; }
    }

    [DataContract]
    public class ImporterSettingBasesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ImporterSettingBase> ImporterSettngBases { get; set; }
    }
}