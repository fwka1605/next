using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class EbFileInformation
    {
        [DataMember] public string FilePath { get; set; }
        /// <summary>ファイルパス</summary>
        /// <remarks>フルパス</remarks>
        [DataMember] public string File { get; set; }

        /// <summary>取込ファイルフォーマット</summary>
        [DataMember] public int Format { get; set; }

        /// <summary>ファイル/フィールド 区切り指定</summary>
        [DataMember] public int FileFieldType { get; set; }

        /// <summary>銀行コード</summary>
        [DataMember] public string BankCode { get; set; }

        /// <summary>任意の取込区分 フォーマット毎に指定方法が異なる</summary>
        [DataMember] public string ImportableValue { get; set; }
        /// <summary>起算日を入金日として利用</summary>
        [DataMember] public bool UseValueDate { get; set; }
        /// <summary>読込・取込結果</summary>
        [DataMember] public int Result { get; set; }
        /// <summary>GridのIndex など管理用</summary>
        [DataMember] public int Index { get; set; }
        /// <summary>読込・登録後のデータ</summary>
        [DataMember] public ImportFileLog ImportFileLog { get; set; }
        /// <summary>不足している銀行口座情報</summary>
        [DataMember] public string BankInformation { get; set; }

        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        /// <summary>EBデータ取込 指定年</summary>
        [DataMember] public int Year { get; set; }

    }
}
