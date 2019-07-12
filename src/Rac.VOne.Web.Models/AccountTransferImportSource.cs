using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class AccountTransferImportSource
    {
        /// <summary><see cref="Encoding.CodePage"/>通常 shift-jis cp932</summary>
        [DataMember] public int EncodingCodePage { get; set; } = 932;

        /// <summary>CSVデータのバイト配列</summary>
        [DataMember] public byte[] Data { get; set; }
        /// <summary>ファイル名 特定フォーマットで、振替（引落）日がファイル名から取得する必要がある</summary>
        [DataMember] public string FileName { get; set; }

        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int TransferYear { get; set; }

        /// <summary>決済代行会社ID</summary>
        [DataMember] public int PaymentAgencyId { get; set; }

        /// <summary>振替不能時に変換する回収区分ID <see cref="Category.Id"/></summary>
        [DataMember] public int? NewCollectCategoryId { get; set; }

        /// <summary>取込後登録された <see cref="ImportData.Id"/></summary>
        [DataMember] public long? ImportDataId { get; set; }
    }
}
