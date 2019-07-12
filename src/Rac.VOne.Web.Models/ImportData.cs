using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>インポートデータ ヘッダー</summary>
    [DataContract] public class ImportData : ITransactional, IByCompany
    {
        /// <summary>インポートデータID</summary>
        [DataMember] public long Id { get; set; }
        /// <summary>会社ID</summary>
        [DataMember] public int CompanyId { get; set; }
        /// <summary>ファイル名</summary>
        [DataMember] public string FileName { get; set; }
        /// <summary>ファイルサイズ</summary>
        [DataMember] public int FileSize { get; set; }
        /// <summary>登録者ID</summary>
        [DataMember] public int CreateBy { get; set; }
        /// <summary>登録日時</summary>
        [DataMember] public DateTime CreateAt { get; set; }

        [DataMember] public ImportDataDetail[] Details { get; set; }
    }
}
