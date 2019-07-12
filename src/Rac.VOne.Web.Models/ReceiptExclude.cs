using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReceiptExclude : ITransactionData, ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public long ReceiptId { get; set; }
        [DataMember] public decimal ExcludeAmount { get; set; }
        [DataMember] public int? ExcludeCategoryId { get; set; }
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int ExcludeFlag { get; set; }

        [DataMember] public DateTime ReceiptUpdateAt { get; set; }
    }

    [DataContract]
    public class ReceiptExcludeResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReceiptExclude[] ReceiptExclude { get; set; }
    }

    [DataContract]
    public class ReceiptExcludesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReceiptExclude> ReceiptExcludes { get; set; }
    }
}
