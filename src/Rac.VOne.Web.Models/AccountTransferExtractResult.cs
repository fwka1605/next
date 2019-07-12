using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class AccountTransferExtractResult
    {
        [DataMember] public AccountTransferDetail[] Details { get; set; }
        [DataMember] public string[] Logs { get; set; }
    }
}
