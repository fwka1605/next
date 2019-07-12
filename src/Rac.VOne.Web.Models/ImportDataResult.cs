using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class ImportDataResult
    {
        [DataMember] public ImportData ImportData { get; set; }
        [DataMember] public List<string> Logs { get; set; }

        [DataMember] public int ReadCount { get; set; }
        [DataMember] public int ValidCount { get; set; }
        [DataMember] public int InvalidCount { get; set; }
        [DataMember] public int SaveCount { get; set; }
        [DataMember] public decimal SaveAmount { get; set; }
        [DataMember] public int NewCustomerCreationCount { get; set; }
    }
}
