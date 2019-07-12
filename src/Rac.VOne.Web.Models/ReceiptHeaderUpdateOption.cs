using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    ///  入金ヘッダー (EBデータのヘッダーと同等)の 割当(消込)済フラグ更新用
    ///  CompanyId, ReceiptId, ReceiptHeaderId それぞれで択一
    ///  UpdateBy は 必須
    /// </summary>
    [DataContract] public class ReceiptHeaderUpdateOption
    {
        /// <summary>会社ID</summary>
        [DataMember] public int? CompanyId { get; set; }
        /// <summary>入金ID</summary>
        [DataMember] public long? ReceiptId { get; set; }
        /// <summary>入金ヘッダーID</summary>
        [DataMember] public long? ReceiptHeaderId { get; set; }
        [DataMember] public int UpdateBy { get; set; }
    }
}
