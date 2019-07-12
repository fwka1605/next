using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MasterImportSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        /// <summary>
        /// <see cref="Rac.VOne.Import.ImportMethod"/>の enum の値
        /// </summary>
        [DataMember] public int ImportMethod { get; set; }
        [DataMember] public int EncodingCodePage { get; set; } = Encoding.UTF8.CodePage; // 65001
        /// <summary>得意先マスター インポート時の パターンNo</summary>
        [DataMember] public string ImporterSettingCode { get; set; }
        [DataMember] public byte[] Data { get; set; }
    }
}
