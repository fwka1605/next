using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// 長期前受検索用
    /// </summary>
    [DataContract]
    public class BillingDivisionContractSearch
    {
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public string  ContractNumber { get; set; }
        [DataMember] public int[] CustomerIds { get; set; }
        [DataMember] public long[] BillingIds { get; set; }
    }
}
