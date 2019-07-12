using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class OmitSource
    {
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public bool DoDelete { get; set; }
        [DataMember] public Transaction[] Transactions { get; set; }
    }
}
