using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class ReceiptImportDuplicationSource
    {
        [DataMember] public int CompanyId { get; set; }
        /// <summary>インポート設定ID <see cref="Details"/>に設定した場合は不要</summary>
        [DataMember] public int ImporterSettingId { get; set; }
        [DataMember] public ReceiptImportDuplication[] Items { get; set; }
        /// <summary>取込詳細</summary>
        [DataMember] public ImporterSettingDetail[] Details { get; set; }
    }
}
