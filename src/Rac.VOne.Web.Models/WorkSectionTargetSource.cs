using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class WorkSectionTargetSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public byte[] ClientKey { get; set; }
        [DataMember] public int[] SectionIds { get; set; }
    }
}
