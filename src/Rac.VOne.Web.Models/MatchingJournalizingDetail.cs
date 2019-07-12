using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingJournalizingDetail
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int HeaderId { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public DateTime? CreateAt { get; set; }
        [DataMember] public DateTime? RecordedAt { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public decimal ReceiptAmount { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public DateTime? BilledAt { get; set; }
        [DataMember] public decimal? BillingAmount { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public int JournalizingType { get; set; }
        [DataMember] public int GroupIndex { get; set; }
        public string JournalizingName
        {
            get
            {
                if (JournalizingType == Common.JournalizingType.Receipt) return "入金仕訳";
                if (JournalizingType == Common.JournalizingType.Matching) return "消込仕訳";
                if (JournalizingType == Common.JournalizingType.AdvanceReceivedOccured) return "前受計上仕訳";
                if (JournalizingType == Common.JournalizingType.AdvanceReceivedTransfer) return "前受振替仕訳";
                if (JournalizingType == Common.JournalizingType.ReceiptExclude) return "対象外仕訳";
                return string.Empty;
            }
        }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
    }

    [DataContract]
    public class MatchingJournalizingDetailsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MatchingJournalizingDetail> MatchingJournalizingDetails { get; set; }
    }
}
