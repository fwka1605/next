using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class AccountTransferImportResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ImportData ImportData { get; set; }

        /// <summary>取込後、エラーとなって グリッドに表示する内容</summary>
        [DataMember] public List<AccountTransferSource> InvalidSources { get; set; }

        /// <summary>読込件数</summary>
        [DataMember] public int ReadCount { get; set; }
        /// <summary>振替済件数</summary>
        [DataMember] public int ValidCount { get; set; }
        /// <summary>振替済金額</summary>
        [DataMember] public decimal ValidAmount { get; set; }
        /// <summary>振替不能件数</summary>
        [DataMember] public int InvalidCount { get; set; }
        /// <summary>振替不能金額</summary>
        [DataMember] public decimal InvalidAmount { get; set; }

        /// <summary>取込検証エラーログ</summary>
        [DataMember] public List<string> Logs { get; set; }
    }
}
