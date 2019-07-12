using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class ReceiptSaveItem
    {
        /// <summary>入金データ入力 明細 登録用のモデル配列</summary>
        [DataMember] public ReceiptInput[] Receipts { get; set; }
        /// <summary>個別消込画面 債権代表者 指定がある場合に設定</summary>
        [DataMember] public int? ParentCustomerId { get; set; }
        /// <summary>個別消込画面 ワークへの登録が必要な場合に設定</summary>
        [DataMember] public byte[] ClientKey { get; set; }
    }
}
