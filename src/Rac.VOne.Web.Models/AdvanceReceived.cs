using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class AdvanceReceived
    {
        [DataMember] public long OriginalReceiptId { get; set; }
        [DataMember] public DateTime OriginalUpdateAt { get; set; }
        [DataMember] public long ReceiptId { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int ReceiptCategoryId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int CompanyId { get; set; }
    }

    [DataContract]
    public class AdvanceReceivedResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public List<AdvanceReceived> AdvancedReceiveItems { get; set; }
    }
}
