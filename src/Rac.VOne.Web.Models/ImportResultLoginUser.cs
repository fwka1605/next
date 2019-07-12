using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ImportResultLoginUser : ImportResult
    {
        [DataMember] public bool LicenseIsOrver { get; set;}
        [DataMember] public bool NotExistsLoginUser { get; set;}
        [DataMember] public bool LoginUserHasNotLoginLicense { get; set;}
    }
}
