using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MFBilling : IByCompany
    {
        [DataMember] public long BillingId { get; set; }
        [DataMember] public int CompanyId { get; set; }
        /// <summary> MFクラウト請求書データのハッシュID </summary>
        [DataMember] public string Id { get; set; }
    }

    public class MFBillingsResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public List<MFBilling> MFBillings { get; set; }
    }
}
