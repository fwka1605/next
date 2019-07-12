using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]

    public class MasterData
    {
        [DataMember]public string  Code { get; set; }
        [DataMember]public string  Name { get; set; }

        public MasterData() { }
        public MasterData(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }


    [DataContract]
    public class MasterDatasResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MasterData> MasterDatas { get; set; }
    }
}
