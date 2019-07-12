using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>インポートデータの詳細</summary>
    [DataContract] public class ImportDataDetail : ITransactional
    {
        /// <summary>単純な primary key</summary>
        [DataMember] public long Id { get; set; }
        /// <summary><see cref="ImportData.Id"/></summary>
        [DataMember] public long ImportDataId { get; set; }
        /// <summary>オブジェクトタイプ
        /// 請求フリーインポーター 0 : 正常請求データ, 1 : 不正請求データ
        /// 入金フリーインポーター 0 : 正常入金データ, 1 : 不正入金データ
        /// 入金予定フリーインポーター 0 : 登録用オブジェクト, 1 : 正常検証データ, 2 : 不正検証データ
        /// </summary>
        [DataMember] public int ObjectType { get; set; }
        /// <summary>オブジェクトを MessagePack で シリアライズしたバイト配列</summary>
        [DataMember] public byte[] RecordItem { get; set; }
    }
}
