using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class LoginUserLicense : IByCompany
    {
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public string LicenseKey { get; set; }
    }

    [DataContract]
    public class LoginUserLicenseResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public LoginUserLicense LoginUserLicense { get; set; }
    }


    [DataContract]
    public class LoginUserLicensesResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public List<LoginUserLicense> LoginUserLicenses { get; set; }
    }
}
