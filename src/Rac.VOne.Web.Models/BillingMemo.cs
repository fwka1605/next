using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingMemo
    {
        [DataMember] public long BillingId { get; set; }
        [DataMember] public string Memo { get; set; }
    }

    [DataContract]
    public class BillingMemoResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public BillingMemo[] BillingMemo { get; set; }
    }

    [DataContract]
    public class BillingMemosResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingMemo> BillingMemos { get; set; }
    }

    [DataContract]
    public class BillMemoResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public string BillingMemo { get; set; }
    }
}
