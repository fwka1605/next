using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingBillingDiscount
    {
        [DataMember] public long MatchingId { get; set; }
        [DataMember] public int DiscountType { get; set; }
        [DataMember] public decimal DiscountAmount { get; set; }
    }

    [DataContract]
    public class MatchingBillingDiscountResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public MatchingBillingDiscount[] MatchingHistory { get; set; }
    }

    [DataContract]
    public class MatchingBillingDiscountsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MatchingBillingDiscount> MatchingHistorys { get; set; }
    }
}
