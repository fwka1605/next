using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class BillingImportDuplicationSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public BillingImportDuplicationWithCode[] Items { get; set; }
        [DataMember] public ImporterSettingDetail[] Details { get; set; }

    }
}
