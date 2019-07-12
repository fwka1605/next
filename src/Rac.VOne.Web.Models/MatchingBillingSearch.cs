using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingBillingSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int? CurrencyId { get; set; }
        [DataMember] public DateTime? DueAtFrom { get; set; }
        [DataMember] public DateTime? DueAtTo { get; set; }
        [DataMember] public int? DepartmentId { get; set; }
        [DataMember] public int? PaymentAgencyId { get; set; }
        [DataMember] public int? ParentCustomerId { get; set; }
        [DataMember] public byte[] ClientKey { get; set; }
        [DataMember] public int BillingDataType { get; set; }
        [DataMember] public int UseReceiptSection { get; set; }
        [DataMember] public int UseCashOnDueDates { get; set; }
        [DataMember] public int IsParent { get; set; }
        [DataMember] public long? MatchingHeaderId { get; set; }
        [DataMember] public bool UseDepartmentWork { get; set; }
        [DataMember] public MatchingOrder[] Orders { get; set; }
    }
}
