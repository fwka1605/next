using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ImportResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public int InsertCount { get; set; }
        [DataMember] public int UpdateCount { get; set; }
        [DataMember] public int DeleteCount { get; set; }

        [DataMember] public List<string> Logs { get; set; }

        public int ValidItemCount { get; set; }
        public int InvalidItemCount { get; set; }
    }
}
