using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class AccountTransferSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime? DueAtFrom { get; set; }
        [DataMember] public DateTime? DueAtTo { get; set; }
        [DataMember] public int? CollectCategoryId { get; set; }
        [DataMember] public int? CurrencyId { get; set; }

        [DataMember] public long[] AccountTransferLogIds { get; set; }
    }
}
