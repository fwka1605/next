using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class KanaHistoryCustomer
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int HitCount { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        //Join for customer
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
    }

    [DataContract]
    public class KanaHistoryCustomerResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public KanaHistoryCustomer[] KanaHistoryCustomer { get; set; }
    }

    [DataContract]
    public class KanaHistoryCustomersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<KanaHistoryCustomer> KanaHistoryCustomers { get; set; }
    }
}
