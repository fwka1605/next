using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class FunctionAuthoritySearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int? LoginUserId { get; set; }
        [DataMember] public int[] FunctionTypes { get; set; }
    }
}
