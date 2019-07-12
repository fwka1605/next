using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class InvoiceTemplateSetting : IMaster
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; } = string.Empty;
        [DataMember] public string Name { get; set; } = string.Empty;
        [DataMember] public int? CollectCategoryId { get; set; }
        [DataMember] public string Title { get; set; } = string.Empty;
        [DataMember] public string Greeting { get; set; } = string.Empty;
        [DataMember] public int DisplayStaff { get; set; }
        [DataMember] public string DueDateComment { get; set; } = string.Empty;
        [DataMember] public int? DueDateFormat { get; set; }
        [DataMember] public string TransferFeeComment { get; set; } = string.Empty;
        [DataMember] public string FixedString { get; set; } = string.Empty;
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class InvoiceTemplateSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public InvoiceTemplateSetting InvoiceTemplateSetting { get; set; }
    }

    [DataContract]
    public class InvoiceTemplateSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<InvoiceTemplateSetting> InvoiceTemplateSettings { get; set; }
    }
}
