using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class MatchingSequentialFileDefinition : RowDefinition<Collation>
    {
        public NumberFieldDefinition<Collation, bool> CheckedField { get; private set; }
        public StringFieldDefinition<Collation> CurrencyCodeField { get; private set; }
        public StringFieldDefinition<Collation> CustomerCodeField { get; private set; }
        public StringFieldDefinition<Collation> CustomerNameField { get; private set; }
        public NumberFieldDefinition<Collation, int> BillingCountField { get; private set; }
        public NumberFieldDefinition<Collation, decimal> BillingAmountField { get; private set; }
        public StringFieldDefinition<Collation> PayerNameField { get; private set; }
        public NumberFieldDefinition<Collation, int> ReceiptCountField { get; private set; }
        public NumberFieldDefinition<Collation, decimal> ReceiptAmountField { get; private set; }
        public StringFieldDefinition<Collation> ShareTransferFee { get; private set; }
        public NumberFieldDefinition<Collation, decimal> Different { get; private set; }
        public StringFieldDefinition<Collation> DisDispAdvanceReceivedCountField { get; private set; }

        public MatchingSequentialFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "一括消込チェックリスト";
            FileNameToken = DataTypeToken;

            CheckedField = new NumberFieldDefinition<Collation, bool>(
                k => k.Checked)
            {
                FieldName = "一括",
                FieldNumber = 1,
                Required = false,
                Accept = VisitChecked,
                Format = value => (value == false) ? "  " : "レ",
            };
            CurrencyCodeField = new StringFieldDefinition<Collation>(
                k => k.CurrencyCode )
            {
                FieldName = "通貨",
                FieldNumber = 2,
                Required = false,
                Accept = VisitCurrencyCode,
            };
            CustomerCodeField = new StringFieldDefinition<Collation>(
                k => k.DispCustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 3,
                Required = false,
                Accept = VisitCustomerCode,
            };
            CustomerNameField = new StringFieldDefinition<Collation>(
               k => k.DispCustomerName)
            {
                FieldName = "得意先名（代表者）",
                FieldNumber = 4,
                Required = false,
                Accept = VisitCustomerName,
            };
            BillingCountField = new NullableNumberFieldDefinition<Collation, int>(
              k => k.DispBillingCount)
            {
                FieldName = "件数",
                FieldNumber = 5,
                Required = false,
                Accept = VisitBillingCount,
                Format = value => (value == 0) ? " " : value.ToString(""),
            };
            BillingAmountField = new NullableNumberFieldDefinition<Collation, decimal>(
             k => k.DispBillingAmount)
            {
                FieldName = "金額",
                FieldNumber = 6,
                Required = false,
                Accept = VisitBillingAmount,
                Format = value => (value == 0) ? " " : value.ToString(""),
            };
            PayerNameField = new StringFieldDefinition<Collation>(
            k => k.PayerName)
            {
                FieldName = "振込依頼人名",
                FieldNumber = 7,
                Required = false,
                Accept = VisitPayerName,
            };
            ReceiptCountField = new NullableNumberFieldDefinition<Collation, int>(
            k => k.DispReceiptCount)
            {
                FieldName = "件数",
                FieldNumber = 8,
                Required = false,
                Accept = VisitReceiptCount,
                Format = value => (value == 0) ? " " : value.ToString(""),
            };
            ReceiptAmountField = new NullableNumberFieldDefinition<Collation, decimal>(
            k => k.DispReceiptAmount)
            {
                FieldName = "金額",
                FieldNumber = 9,
                Required = false,
                Accept = VisitReceiptAmount,
                Format = value => (value == 0) ? " " : value.ToString(""),
            };
            ShareTransferFee = new StringFieldDefinition<Collation>(
            k => k.DispShareTransferFee)
            {
                FieldName = "手数科",
                FieldNumber = 10,
                Required = false,
                Accept = VisitShareTransferfee,
            };
            Different = new NullableNumberFieldDefinition<Collation, decimal>(
            k => k.DispDifferent)
            {
                FieldName = "差額",
                FieldNumber = 11,
                Required = false,
                Accept = VisitDifferent,
                Format = value => (value == 0) ? " " : value.ToString(""),
            };
            DisDispAdvanceReceivedCountField = new StringFieldDefinition<Collation>(
            k => k.DispAdvanceReceivedCount)
            {
                FieldName = "前受",
                FieldNumber = 12,
                Required = false,
                Accept = VisitorDisDispAdvanceReceivedCount,
            };
            Fields.AddRange(new IFieldDefinition<Collation>[] {
                       CheckedField,CurrencyCodeField, CustomerCodeField,
                       CustomerNameField, BillingCountField, BillingAmountField,
                       PayerNameField, ReceiptCountField, ReceiptAmountField,
                       ShareTransferFee, Different, DisDispAdvanceReceivedCountField});
        }

        private bool VisitChecked(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardNumber(CheckedField);
        }

        private bool VisitCurrencyCode(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }

        private bool VisitCustomerCode(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }

        private bool VisitCustomerName(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }

        private bool VisitBillingCount(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardNumber(BillingCountField);
        }

        private bool VisitBillingAmount(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardNumber(BillingAmountField);
        }

        private bool VisitPayerName(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardString(PayerNameField);
        }

        private bool VisitReceiptCount(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardNumber(ReceiptCountField);
        }

        private bool VisitReceiptAmount(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardNumber(ReceiptAmountField);
        }

        private bool VisitShareTransferfee(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardString(ShareTransferFee);
        }

        private bool VisitDifferent(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardNumber(Different);
        }

        private bool VisitorDisDispAdvanceReceivedCount(IFieldVisitor<Collation> visitor)
        {
            return visitor.StandardString(DisDispAdvanceReceivedCountField);
        }

    }
}
