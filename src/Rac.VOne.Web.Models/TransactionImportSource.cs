using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class TransactionImportSource
    {
        /// <summary>会社ID</summary>
        [DataMember] public int CompanyId { get; set; }
        /// <summary>ログインユーザーID</summary>
        [DataMember] public int LoginUserId { get; set; }

        /// <summary><see cref="Data"/>の Encoding CodePage</summary>
        [DataMember] public int EncodingCodePage { get; set; } = 932; // MSCP932 shift-jis
        /// <summary><see cref="ImporterSetting.Id"/>フリーインポーターの設定ID</summary>
        [DataMember] public int ImporterSettingId { get; set; }
        /// <summary>インポート用データのバイト配列</summary>
        [DataMember] public byte[] Data { get; set; }

        /// <summary>登録時に利用 <see cref="ImportData.Id"/></summary>
        [DataMember] public long? ImportDataId { get; set; }
        /// <summary>帳票出力で利用 正常・不正 データの指定に利用
        /// <see cref="ImportDataDetail.ObjectType"/> は インポートの種類により異なるので、各処理の内部で判定して変更する
        /// </summary>
        [DataMember] public bool IsValidData { get; set; }
        /// <summary>入金予定フリーインポーター のみ利用
        /// 処理対象請求データ true: 未消込のみ, false: 一部消込も対象</summary>
        [DataMember] public bool DoTargetNotMatchedData { get; set; }
        /// <summary>入金予定フリーインポーター のみ利用
        /// 金額処理方法 true: 更新(書換), false : 加算</summary>
        [DataMember] public bool DoReplaceAmount { get; set; }
        /// <summary>入金予定フリーインポーター のみ利用
        /// 同一得意先（債権代表者）請求データ true: 無視, false: 考慮する</summary>
        [DataMember] public bool DoIgnoreSameCustomerGroup { get; set; }
    }
}
