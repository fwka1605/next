using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Logs
    {
        [DataMember] public string Level { get; set; }
        [DataMember] public string Logger { get; set; }
        [DataMember] public string SessionKey { get; set; }
        //[DataMember] public string CompanyCode { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public string Exception { get; set; }
        [DataMember] public string DatabaseName { get; set; }
        [DataMember] public string Query { get; set; }
        [DataMember] public string Parameters { get; set; }
    }
}
