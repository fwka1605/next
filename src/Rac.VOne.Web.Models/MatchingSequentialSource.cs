using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MatchingSequentialSource
    {
        [DataMember] public Collation[] Collations { get; set; }
        [DataMember] public CollationSearch Option { get; set; }
    }
}
