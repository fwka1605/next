using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Models.Files
{
    /// <summary>消込履歴データ 一括消込 消込完了データ エクスポート用</summary>
    public class MatchingHeaderFileDefinition : RowDefinition<MatchingHeader>
    {
        public NumberFieldDefinition<MatchingHeader, bool> CheckedField { get; private set; }
        public StringFieldDefinition<MatchingHeader> CurrencyCodeField { get; private set; }
        public StringFieldDefinition<MatchingHeader> CustomerCodeField { get; private set; }
        public StringFieldDefinition<MatchingHeader> CustomerNameField { get; private set; }
        public NumberFieldDefinition<MatchingHeader, int> BillingCountField { get; private set; }
        public NumberFieldDefinition<MatchingHeader, decimal> BillingAmountField { get; private set; }
        public StringFieldDefinition<MatchingHeader> PayerNameField { get; private set; }
        public NumberFieldDefinition<MatchingHeader, int> ReceiptCountField { get; private set; }
        public NumberFieldDefinition<MatchingHeader, decimal> ReceiptAmountField { get; private set; }
        public StringFieldDefinition<MatchingHeader> ShareTransferFeeField { get; private set; }
        public NumberFieldDefinition<MatchingHeader, decimal> DifferentField { get; private set; }
        public StringFieldDefinition<MatchingHeader> DispMemoField { get; private set; }
        public MatchingHeaderFileDefinition(DataExpression dataExpression) :
            base(dataExpression)
        {
            Fields.Add(CheckedField = new NumberFieldDefinition<MatchingHeader, bool>(x => x.Checked,
                "解除", accept: VisitChecked, formatter: x => x ? "レ" : ""));
            Fields.Add(CurrencyCodeField = new StringFieldDefinition<MatchingHeader>(x => x.CurrencyCode,
                "通貨", accept: VisitCurrencyCode));
            Fields.Add(CustomerCodeField = new StringFieldDefinition<MatchingHeader>(x => x.DispCustomerCode,
                "得意先コード", accept: VisitCustomerCode));
            Fields.Add(CustomerNameField = new StringFieldDefinition<MatchingHeader>(x => x.DispCustomerName,
                "得意先名（代表者）", accept: VisitCustomerName));
            Fields.Add(BillingCountField = new NumberFieldDefinition<MatchingHeader, int>(x => x.BillingCount,
                "件数", accept: VisitBillingCount, formatter: x => x.ToString()));
            Fields.Add(BillingAmountField = new NumberFieldDefinition<MatchingHeader, decimal>(x => x.BillingAmount,
                "金額", accept: VisitBillingAmount));
            Fields.Add(PayerNameField = new StringFieldDefinition<MatchingHeader>(x => x.PayerName,
                "振込依頼人名", accept: VisitPayerName));
            Fields.Add(ReceiptCountField = new NumberFieldDefinition<MatchingHeader, int>(x => x.ReceiptCount,
                "件数", accept: VisitReceiptCount, formatter: x => x.ToString()));
            Fields.Add(ReceiptAmountField = new NumberFieldDefinition<MatchingHeader, decimal>(x => x.ReceiptAmount,
                "金額", accept: VisitReceiptAmount));
            Fields.Add(ShareTransferFeeField = new StringFieldDefinition<MatchingHeader>(x => x.DispShareTransferFee,
                "手数科", accept: VisitShareTransferFee));
            Fields.Add(DifferentField = new NumberFieldDefinition<MatchingHeader, decimal>(x => x.Different,
                "差額", accept: VisitDifferent));
            Fields.Add(DispMemoField = new StringFieldDefinition<MatchingHeader>(x => x.DispMemo,
                "メモ", accept: VisitDispMemo));
        }

        private bool VisitChecked(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardNumber(CheckedField);
        private bool VisitCurrencyCode(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardString(CurrencyCodeField);
        private bool VisitCustomerCode(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardString(CustomerCodeField);
        private bool VisitCustomerName(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardString(CustomerNameField);
        private bool VisitBillingCount(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardNumber(BillingCountField);
        private bool VisitBillingAmount(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardNumber(BillingAmountField);
        private bool VisitPayerName(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardString(PayerNameField);
        private bool VisitReceiptCount(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardNumber(ReceiptCountField);
        private bool VisitReceiptAmount(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardNumber(ReceiptAmountField);
        private bool VisitShareTransferFee(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardString(ShareTransferFeeField);
        private bool VisitDifferent(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardNumber(DifferentField);
        private bool VisitDispMemo(IFieldVisitor<MatchingHeader> visitor)
            => visitor.StandardString(DispMemoField);

    }
}
