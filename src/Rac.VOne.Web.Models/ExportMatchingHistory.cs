using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{

    public class ExportMatchingHistory
    {
        public long? MatchingHeaderId { get; set; }
        public string CreateAt { get; set; }
        public string CreateAtSource { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string BilledAt { get; set; }
        public string SalesAt { get; set; }
        public string InvoiceCode { get; set; }
        public string BillingCategory { get; set; }
        public string CollectCategory { get; set; }
        public string BillingAmount { get; set; }
        public string BillingAmountExcludingTax { get; set; }
        public string TaxAmount { get; set; }
        public string MatchingAmount { get; set; }
        public string BillingRemain { get; set; }
        public string BillingNote1 { get; set; }
        public string BillingNote2 { get; set; }
        public string BillingNote3 { get; set; }
        public string BillingNote4 { get; set; }
        public string RecordedAt { get; set; }
        public string ReceiptId { get; set; }
        public string ReceiptCategory { get; set; }
        public string ReceiptAmount { get; set; }
        public string AdvanceReceivedOccuredString { get; set; }
        public string ReceiptRemain { get; set; }
        public string PayerName { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string ReceiptNote1 { get; set; }
        public string ReceiptNote2 { get; set; }
        public string ReceiptNote3 { get; set; }
        public string ReceiptNote4 { get; set; }
        public string VirtualBranchCode { get; set; }
        public string VirtualAccountNumber { get; set; }
        public string LoginUserName { get; set; }
        public string MatchingProcessTypeString { get; set; }
        public string MatchingMemo { get; set; }
    }
}
