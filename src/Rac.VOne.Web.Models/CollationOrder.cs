using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CollationOrder : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CollationTypeId { get; set; }
        [DataMember] public int ExecutionOrder { get; set; }
        [DataMember] public int Available { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string CollationTypeName { get; set; }
    }

    [DataContract]
    public class CollationOrdersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CollationOrder> CollationOrders { get; set; }
    }
}
