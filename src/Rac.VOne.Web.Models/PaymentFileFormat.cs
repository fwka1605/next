using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class PaymentFileFormat
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        [DataMember] public int Available { get; set; }
        [DataMember] public int IsNeedYear { get; set; }
    }

    [DataContract]
    public class PaymentFileFormatResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<PaymentFileFormat> PaymentFileFormats { get; set; }
    }
}
