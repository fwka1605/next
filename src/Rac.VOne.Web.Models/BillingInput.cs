using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingInput
    {
        [DataMember] public long Id { get; set; }
    }

    [DataContract]
    public class BillingInputResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<long> BillingInputIds { get; set; }
    }

    [DataContract]
    public class BillingInputSource
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public bool IsFirstPublish { get; set; }
        [DataMember] public int? InvoiceTemplateId { get; set; }
        [DataMember] public int? UseInvoiceCodeNumbering { get; set; }
    }
}
