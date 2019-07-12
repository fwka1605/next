using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
   public class JournalizingSummary
    {
        public bool Selected { get; set; }
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public int Count { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public decimal Amount { get; set; }
        /// <summary>対象に含まれる Transaction.UpdateAt の MAX 値</summary>
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class JournalizingSummariesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<JournalizingSummary> JournalizingsSummaries { get; set; }
    }
}
