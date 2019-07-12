using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReceiptSectionTransfer
    {
        [DataMember] public long SourceReceiptId { get; set; }
        [DataMember] public long DestinationReceiptId { get; set; }
        [DataMember] public int SourceSectionId { get; set; }
        [DataMember] public int DestinationSectionId { get; set; }
        [DataMember] public decimal SourceAmount { get; set; }
        [DataMember] public decimal DestinationAmount { get; set; }
        [DataMember] public int PrintFlag { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string TransferMemo { get; set; }
        [DataMember] public bool CancelFlag { get; set; }
        [DataMember] public long ReceiptId { get; set; }

        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public string ReceiptCategoryCode { get; set; }
        [DataMember] public string ReceiptCategoryName { get; set; }
        public string CategoryCodeName { get { return $"{ReceiptCategoryCode}:{ReceiptCategoryName}"; } }
        [DataMember] public int InputType { get; set; }
        public string InputTypeCodeName { get { return $"{InputType}:{InputTypeName}"; } }
        [DataMember] public string PayerName { get; set; } = string.Empty;
        [DataMember] public string Note1 { get; set; } = string.Empty;
        [DataMember] public string SourceSectionCode { get; set; } = string.Empty;
        [DataMember] public string SourceSectionName { get; set; } = string.Empty;
        [DataMember] public string DestinationSectionCode { get; set; } = string.Empty;
        [DataMember] public string DestinationSectionName { get; set; } = string.Empty;
        [DataMember] public string Memo { get; set; } = string.Empty;
        [DataMember] public string LoginUserCode { get; set; } = string.Empty;
        [DataMember] public string LoginUserName { get; set; } = string.Empty;
        [DataMember]public string CurrencyCode { get; set; } = string.Empty;
        [DataMember]public int Precision { get; set; }

        public string InputTypeName
        => InputType == 1 ? "EB取込"
         : InputType == 2 ? "入力"
         : InputType == 3 ? "インポーター取込" : "電債取込";
    }

    [DataContract]
    public class ReceiptSectionTransfersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public bool NotClearFlag { get; set; }
        [DataMember] public bool TransferFlag { get; set; }
        [DataMember] public List<ReceiptSectionTransfer> ReceiptSectionTransfers { get; set; } = new List<ReceiptSectionTransfer>();
        [DataMember] public List<Receipt> InsertReceipts { get; set; } = new List<Receipt>();
        [DataMember] public List<Receipt> UpdateReceipts { get; set; } = new List<Receipt>();
        [DataMember] public List<Receipt> DeleteReceipts { get; set; } = new List<Receipt>();
    }

    [DataContract]
    public class ReceiptSectionTransferResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReceiptSectionTransfer ReceiptSectionTransfer { get; set; }
    }
}
