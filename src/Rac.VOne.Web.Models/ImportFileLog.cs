using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]

    public class ImportFileLog : IIdentical, IByCompany
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string FileName { get; set; }
        [DataMember] public int FileSize { get; set; }
        /// <summary>明細読込件数
        /// 銀行口座マスター<see cref="BankAccount"/>で、除外するものは、カウントしない
        /// それ以外の 入出金明細 の 出金 などはカウントする
        /// </summary>
        [DataMember] public int ReadCount { get; set; }
        [DataMember] public int ImportCount { get; set; }
        [DataMember] public decimal ImportAmount { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }

        //formappingReceipt
        [DataMember] public int Apportioned { get; set; }
        [DataMember] public List<ReceiptHeader> ReceiptHeaders { get; set; } = new List<ReceiptHeader>();


        /// <summary>画面グリッド用</summary>
        public bool DoDelete { get; set; }
    }

    [DataContract]
    public class ImportFileLogResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ImportFileLog[] ImportFileLog { get; set; }
    }

    [DataContract]
    public class ImportFileLogsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ImportFileLog> ImportFileLogs { get; set; }
    }
}
