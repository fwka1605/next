using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReceiptMemo
    {
        [DataMember] public long ReceiptId { get; set; }
        [DataMember] public string Memo { get; set; }
    }

    [DataContract]
    public class ReceiptMemosResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReceiptMemo[] ReceiptMemo { get; set; }
    }

    [DataContract]
    public class ReceiptMemoResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public string ReceiptMemo { get; set; }
    }
}
