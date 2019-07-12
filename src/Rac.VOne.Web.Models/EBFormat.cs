using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Rac.VOne.Common.Extensions;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class EBFormat
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        /// <summary>銀行コード 必須</summary>
        [DataMember] public int RequireBankCode { get; set; }
        /// <summary>取込時に、年情報が必要</summary>
        [DataMember] public int RequireYear { get; set; }
        /// <summary>勘定日/起算日 などの 入金日 選択可能かどうか</summary>
        [DataMember] public int IsDateSelectable { get; set; }

        /// <summary>ファイル形式 取り得る値のフラグ合算値
        /// 1 : カンマ区切り
        /// 2 : タブ区切り
        /// 4 : 固定長 改行あり
        /// 8 : 固定長 改行なし</summary>
        [DataMember] public int FileFieldTypes { get; set; }
        [DataMember] public string ImportableValues { get; set; }

        /// <summary>FileFieldTypes の 値から、取り得る ファイル形式の Dictionary を返す</summary>
        public Dictionary<int, string> GetFileFieldTypesSource()
            => ((FileFieldTypes)FileFieldTypes).GetFileFieldTypesSource();

    }

    [DataContract] public class EBFormatsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<EBFormat> EBFileFormats { get; set; }
    }
}
