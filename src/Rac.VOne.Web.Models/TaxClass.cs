using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class TaxClass
    {
        /// <summary>消費税区分
        /// 0 : 内税課税
        /// 1 : 外税課税
        /// 2 : 非課税
        /// 3 : 免税
        /// 4 : 対象外</summary>
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
    }

    [DataContract]
    public class TaxClassResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<TaxClass> TaxClass { get; set; }
    }
}
