using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// 得意先マスター パフォーマンス対応版
    /// プロパティの安易な追加 厳禁
    /// </summary>
    [DataContract] public class CustomerMin : IMaster
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Kana { get; set; }
    }

    [DataContract]
    public class CustomerMinsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CustomerMin> Customers { get; set; }
    }
}
