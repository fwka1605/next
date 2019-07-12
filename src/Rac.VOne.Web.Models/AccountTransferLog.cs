using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class AccountTransferLog : IByCompany, ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public int PaymentAgencyId { get; set; }
        [DataMember] public DateTime RequestDate { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public int OutputCount { get; set; }
        [DataMember] public decimal OutputAmount { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string PaymentAgencyCode { get; set; }
        [DataMember] public string PaymentAgencyName { get; set; }

        public bool Checked { get; set; }

        public string CollectCategoryCodeAndName { get { return $"{CollectCategoryCode}：{CollectCategoryName}"; } }
    }

    [DataContract]
    public class AccountTransferLogsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<AccountTransferLog> AccountTransferLog { get; set; }
    }
}
