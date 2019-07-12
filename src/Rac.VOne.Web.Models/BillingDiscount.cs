using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingDiscount
    {
        [DataMember] public long BillingId { get; set; }
        [DataMember] public int DiscountType { get; set; }
        [DataMember] public decimal DiscountAmount { get; set; }
        [DataMember] public decimal DiscountAmount1 { get; set; }
        [DataMember] public decimal DiscountAmount2 { get; set; }
        [DataMember] public decimal DiscountAmount3 { get; set; }
        [DataMember] public decimal DiscountAmount4 { get; set; }
        [DataMember] public decimal DiscountAmount5 { get; set; }
        [DataMember] public int AssignmentFlag { get; set; }
    }

    [DataContract]
    public class BillingDiscountResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public BillingDiscount[] BillingDiscount { get; set; }
    }

    [DataContract]
    public class BillingDiscountsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingDiscount> BillingDiscounts { get; set; }
    }
}
