using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MFWebApiOption
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public string ClientId { get; set; }
        [DataMember] public string ClientSecret { get; set; }
        [DataMember] public string ExtractSetting { get; set; }
        [DataMember] public string AuthorizationCode { get; set; }
        [DataMember] public DateTime? BillingDateFrom { get; set; }
        [DataMember] public DateTime? BillingDateTo { get; set; }
        [DataMember] public string[] MFIds { get; set; }
        [DataMember] public bool IsMatched { get; set; }
        [DataMember] public string PartnerId { get; set; }
        [DataMember] public string ApiVersion { get; set; }
    }
}
