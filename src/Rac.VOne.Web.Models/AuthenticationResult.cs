using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class AuthenticationResult : ProcessResult
    {
        [DataMember] public string SessionKey { get; set; }

        public AuthenticationResult() : base()
        {
            SessionKey = string.Empty;
        }
    }
}
