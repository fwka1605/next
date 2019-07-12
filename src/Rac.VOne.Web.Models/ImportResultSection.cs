using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ImportResultSection : ImportResult
    {
        [DataMember] public List<Section> Section { get; set; } = new List<Section>();
    }
}
