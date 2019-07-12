using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public  class MatchingReceiptSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime? RecordedAtFrom { get; set; }
        [DataMember] public DateTime? RecordedAtTo { get; set; }
        [DataMember] public int? ParentCustomerId { get; set; }
        [DataMember] public byte[] ClientKey { get; set; }
        [DataMember] public int BillingDataType { get; set; }
        [DataMember] public　int UseReceiptSection { get; set; }
        [DataMember] public int UseCashOnDueDates { get; set; }
        [DataMember] public int UseScheduledPayment { get; set; }
        [DataMember] public int? PaymentAgencyId { get; set; }
        [DataMember] public long? MatchingHeaderId { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public MatchingOrder[] Orders { get; set; }
    }
}
