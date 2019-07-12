using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class EBFileFormatSetting : IIdentical, IByCompany
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Name{ get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        [DataMember] public int UseFormat { get; set; }
        [DataMember] public int EBFileFormatId { get; set; }
        [DataMember] public int Delimiter { get; set; }
        [DataMember] public string BankCode  { get; set; }
        [DataMember] public int UseValueDate{ get; set; }
        [DataMember] public string ImportableValues { get; set; }
        [DataMember] public string FilePath{ get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }    
    }

    [DataContract] public class EBFileFormatSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public EBFileFormatSetting EBFileFormatSetting { get; set; }
    }

    [DataContract] public class EBFileFormatSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<EBFileFormatSetting> EBFileFormatSettings { get; set; }
    }
}
