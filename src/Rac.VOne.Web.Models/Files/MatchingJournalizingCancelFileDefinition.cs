using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class MatchingJournalizingCancelFileDefinition : RowDefinition<MatchingJournalizingDetail>
    {
        public StandardIdToCodeFieldDefinition<MatchingJournalizingDetail, Company> CompanyIdField { get; private set; }
        public NullableNumberFieldDefinition<MatchingJournalizingDetail, DateTime> CreateAtField { get; private set; }
        public StringFieldDefinition<MatchingJournalizingDetail> JournalizingNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizingDetail> CustomerCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizingDetail> CustomerNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizingDetail> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizingDetail, decimal> AmountField { get; private set; }
        public NullableNumberFieldDefinition<MatchingJournalizingDetail, DateTime> OutputAtField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizingDetail, decimal> ReceiptAmountField { get; private set; }
        public NullableNumberFieldDefinition<MatchingJournalizingDetail, DateTime> RecordedAtField { get; private set; }
        public StringFieldDefinition<MatchingJournalizingDetail> PayerNameField { get; private set; }
        public NullableNumberFieldDefinition<MatchingJournalizingDetail, DateTime> BilledAtField { get; private set; }
        public StringFieldDefinition<MatchingJournalizingDetail> InvoiceCodeField { get; private set; }
        public NullableNumberFieldDefinition<MatchingJournalizingDetail, decimal> BillingAmountField { get; private set; }

        public MatchingJournalizingCancelFileDefinition(DataExpression applicationControl)
            : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "消込仕訳出力取消";
            FileNameToken = DataTypeToken;

            CompanyIdField = new StandardIdToCodeFieldDefinition<MatchingJournalizingDetail, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };

            CreateAtField = new NullableNumberFieldDefinition<MatchingJournalizingDetail, DateTime>(k => k.CreateAt)
            {
                FieldName = "更新日時",
                FieldNumber = 2,
                Required = true,
                Accept = VisitCreateAtField,
                Format = value => value.ToString(),
            };

            JournalizingNameField = new StringFieldDefinition<MatchingJournalizingDetail>(k => k.JournalizingName)
            {
                FieldName = "仕訳区分",
                FieldNumber = 3,
                Required = false,
                Accept = VisitJournalizingName,
            };

            CustomerCodeField = new StringFieldDefinition<MatchingJournalizingDetail>(k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 4,
                Required = false,
                Accept = VisitCustomerCode,
            };

            CustomerNameField = new StringFieldDefinition<MatchingJournalizingDetail>(k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 5,
                Required = false,
                Accept = VisitCustomerName,
            };

            CurrencyCodeField = new StringFieldDefinition<MatchingJournalizingDetail>(k => k.CurrencyCode)
            {
                FieldName = "通貨コード",
                FieldNumber = 6,
                Required = false,
                Accept = VisitCurrencyCode,
            };

            AmountField = new NumberFieldDefinition<MatchingJournalizingDetail, decimal>(k => k.Amount)
            {
                FieldName = "金額",
                FieldNumber = 7,
                Required = true,
                Accept = VisitAmount,
                Format = value => value.ToString(),
            };

            OutputAtField = new NullableNumberFieldDefinition<MatchingJournalizingDetail, DateTime>(k => k.OutputAt)
            {
                FieldName = "仕訳日",
                FieldNumber = 8,
                Required = true,
                Accept = VisitOutputAtField,
                Format = value => value.ToString(),
            };

            ReceiptAmountField = new NumberFieldDefinition<MatchingJournalizingDetail, decimal>(k => k.ReceiptAmount)
            {
                FieldName = "入金額",
                FieldNumber = 9,
                Required = true,
                Accept = VisitReceiptAmount,
                Format = value => value.ToString(),
            };

            RecordedAtField = new NullableNumberFieldDefinition<MatchingJournalizingDetail, DateTime>(k => k.RecordedAt)
            {
                FieldName = "入金日",
                FieldNumber = 10,
                Required = true,
                Accept = VisitRecordedAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };

            PayerNameField = new StringFieldDefinition<MatchingJournalizingDetail>(k => k.PayerName)
            {
                FieldName = "振込依頼人名",
                FieldNumber = 11,
                Required = false,
                Accept = VisitPayerName,
            };

            BilledAtField = new NullableNumberFieldDefinition<MatchingJournalizingDetail, DateTime>(k => k.BilledAt)
            {
                FieldName = "請求日",
                FieldNumber = 12,
                Required = true,
                Accept = VisitBillingAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };

            InvoiceCodeField = new StringFieldDefinition<MatchingJournalizingDetail>(k => k.InvoiceCode)
            {
                FieldName = "請求書番号",
                FieldNumber = 13,
                Required = false,
                Accept = VisitInvoiceCode,
            };

            BillingAmountField = new NullableNumberFieldDefinition<MatchingJournalizingDetail, decimal>(k => k.BillingAmount)
            {
                FieldName = "請求額",
                FieldNumber = 14,
                Required = true,
                Accept = VisitBillingAmount,
                Format = value => value.ToString(),
            };

            Fields.AddRange(new IFieldDefinition<MatchingJournalizingDetail>[] {
            CompanyIdField,CreateAtField,JournalizingNameField,CustomerCodeField,CustomerNameField,CurrencyCodeField,AmountField,OutputAtField,
            ReceiptAmountField,RecordedAtField,PayerNameField,BilledAtField,InvoiceCodeField,BillingAmountField});
        }

        private bool VisitCompanyId(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitCreateAtField(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardNumber(CreateAtField);
        }

        private bool VisitJournalizingName(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardString(JournalizingNameField);
        }

        private bool VisitCustomerCode(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }

        private bool VisitCustomerName(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }

        private bool VisitCurrencyCode(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }

        private bool VisitAmount(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardNumber(AmountField);
        }

        private bool VisitOutputAtField(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardNumber(OutputAtField);
        }

        private bool VisitReceiptAmount(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardNumber(ReceiptAmountField);
        }

        private bool VisitRecordedAtField(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardNumber(RecordedAtField);
        }

        private bool VisitPayerName(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardString(PayerNameField);
        }

        private bool VisitBillingAtField(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardNumber(BilledAtField);
        }

        private bool VisitInvoiceCode(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }

        private bool VisitBillingAmount(IFieldVisitor<MatchingJournalizingDetail> visitor)
        {
            return visitor.StandardNumber(BillingAmountField);
        }
    }
}
