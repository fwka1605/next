using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class KanaHistoryPaymentAgency
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public int PaymentAgencyId { get; set; }
        [DataMember] public int HitCount { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        //for join wiht paymentAgency
        [DataMember] public string PaymentAgencyCode { get; set; }
        [DataMember] public string PaymentAgencyName { get; set; }
    }

    [DataContract]
    public class KanaHistoryPaymentAgencyResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public KanaHistoryPaymentAgency[] KanaHistoryPaymentAgency { get; set; }
    }

    [DataContract]
    public class KanaHistoryPaymentAgencysResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<KanaHistoryPaymentAgency> KanaHistoryPaymentAgencys { get; set; }
    }
}
