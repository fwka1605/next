using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class ClientKeySearch
    {
        [DataMember] public string ProgramId { get; set; }
        [DataMember] public string ClientName { get; set; }
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
    }
}
