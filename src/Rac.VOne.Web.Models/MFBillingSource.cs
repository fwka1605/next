using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MFBillingSource
    {
        /// <summary>登録用 請求データ</summary>
        [DataMember] public Billing[] Billings { get; set; }
        /// <summary>登録用 得意先</summary>
        [DataMember] public Customer[] Customers { get; set; }
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public long[] Ids { get; set; }
        [DataMember] public bool? IsMatched { get; set; }
    }
}
