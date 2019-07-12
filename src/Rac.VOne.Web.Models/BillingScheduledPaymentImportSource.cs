using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// 入金予定フリーインポーター用 モデル
    /// </summary>
    [DataContract] public class BillingScheduledPaymentImportSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int ImporterSettingId { get; set; }
        [DataMember] public ImporterSettingDetail[] Details { get; set; }
        [DataMember] public ScheduledPaymentImport[] Items { get; set; }
    }
}
