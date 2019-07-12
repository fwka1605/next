using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ProcessResult
    {
        [DataMember] public bool Result { get; set; }
        [DataMember] public string ErrorCode { get; set; }
        [DataMember] public string ErrorMessage { get; set; }

        public ProcessResult()
        {
            ErrorCode = Common.ErrorCode.None;
            ErrorMessage = "";
        }
    }
}
