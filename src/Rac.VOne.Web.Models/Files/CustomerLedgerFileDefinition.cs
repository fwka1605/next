using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class CustomerLedgerFileDefinition : RowDefinition<CustomerLedger>
    {
        public StringFieldDefinition<CustomerLedger> RecordedAtField { get; private set; }
        public StringFieldDefinition<CustomerLedger> ParentCustomerCodeField { get; private set; }
        public StringFieldDefinition<CustomerLedger> ParentCustomerNameField { get; private set; }
        public StringFieldDefinition<CustomerLedger> SectionNameField { get; private set; }
        public StringFieldDefinition<CustomerLedger> DepartmentNameField { get; private set; }
        public StringFieldDefinition<CustomerLedger> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<CustomerLedger> CategoryNameField { get; private set; }
        public StringFieldDefinition<CustomerLedger> DebitAccountTitleNameField { get; private set; }
        public StringFieldDefinition<CustomerLedger> CurrencyCodeField { get; private set; }
        public StringFieldDefinition<CustomerLedger> MatchingSymbolBillingField { get; private set; }
        public NullableNumberFieldDefinition<CustomerLedger, decimal> BillingAmountField { get; private set; }
        public NullableNumberFieldDefinition<CustomerLedger, decimal> SlipTotalField { get; private set; }
        public NullableNumberFieldDefinition<CustomerLedger, decimal> ReceiptAmountField { get; private set; }
        public StringFieldDefinition<CustomerLedger> MatchingSymbolReceiptField { get; private set; }
        public NullableNumberFieldDefinition<CustomerLedger, decimal> MatchingAmountField { get; private set; }
        public NullableNumberFieldDefinition<CustomerLedger, decimal> RemainAmountField { get; private set; }
        public StringFieldDefinition<CustomerLedger> CustomerCodeField { get; private set; }
        public StringFieldDefinition<CustomerLedger> CustomerNameField { get; private set; }
        //public NumberFieldDefinition<CustomerLedger, int> RecordTypeField { get; private set; }

        public CustomerLedgerFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "得意先別消込台帳";
            FileNameToken = DataTypeToken;

            RecordedAtField = new StringFieldDefinition<CustomerLedger>(
            k => k.RecordTypeName)
            {
                FieldName = "日付",
                FieldNumber = 1,
                Required = true,
                Accept = VisitRecordedAtField,
            };
            ParentCustomerCodeField = new StringFieldDefinition<CustomerLedger>(
                    k => k.ParentCustomerCode)
            {
                FieldName = "代表得意先コード",
                FieldNumber = 2,
                Required = false,
                Accept = VisitParentCustomerCodeField,
            };
            ParentCustomerNameField = new StringFieldDefinition<CustomerLedger>(
                k => k.ParentCustomerName)
            {
                FieldName = "代表得意先名",
                FieldNumber = 3,
                Required = true,
                Accept = VisitParentCustomerNameField,
            };
            CurrencyCodeField = new StringFieldDefinition<CustomerLedger>(
                k => k.CurrencyCode)
            {
                FieldName = "通貨コード",
                FieldNumber = 9,
                Required = true,
                Accept = VisitCurrencyCodeField,
            };
            SectionNameField = new StringFieldDefinition<CustomerLedger>(
                k => k.SectionName)
            {
                FieldName = "入金部門",
                FieldNumber = 4,
                Required = true,
                Accept = VisitSectionNameField,
            };
            DepartmentNameField = new StringFieldDefinition<CustomerLedger>(
                k => k.DepartmentName)
            {
                FieldName = "請求部門",
                FieldNumber = 5,
                Required = true,
                Accept = VisitDepartmentNameField,
            };
            InvoiceCodeField = new StringFieldDefinition<CustomerLedger>(
                k => k.InvoiceCode)
            {
                FieldName = "請求書番号",
                FieldNumber = 6,
                Required = true,
                Accept = VisitInvoiceCodeField,
            };
            CategoryNameField = new StringFieldDefinition<CustomerLedger>(
                k => k.CategoryName)
            {
                FieldName = "区分",
                FieldNumber = 7,
                Required = true,
                Accept = VisitCategoryNameField,
            };
            DebitAccountTitleNameField = new StringFieldDefinition<CustomerLedger>(
                k => k.AccountTitleName)
            {
                FieldName = "債権科目",
                FieldNumber = 8,
                Required = true,
                Accept = VisitDebitAccountTitleNameField,
            };
            MatchingSymbolBillingField = new StringFieldDefinition<CustomerLedger>(
                k => k.MatchingSymbolBilling)
            {
                FieldName = "消込記号_請求",
                FieldNumber = 10,
                Required = false,
                Accept = VisitMatchingSymbolBillingField,
            };
            BillingAmountField = new  NullableNumberFieldDefinition<CustomerLedger, decimal>(
            k => k.BillingAmount)
            {
                FieldName = "請求額",
                FieldNumber = 11,
                Required = true,
                Accept = VisitBillingAmountField,
                Format = value => value.ToString(),
            };
            SlipTotalField = new NullableNumberFieldDefinition<CustomerLedger, decimal>(
            k => k.SlipTotal)
            {
                FieldName = "伝票合計",
                FieldNumber = 12,
                Required = true,
                Accept = VisitSlipTotalField,
                Format = value => value.ToString(),
            };
            ReceiptAmountField = new NullableNumberFieldDefinition<CustomerLedger, decimal>(
            k => k.ReceiptAmount)
            {
                FieldName = "入金額",
                FieldNumber = 13,
                Required = true,
                Accept = VisitReceiptAmountField,
                Format = value => value.ToString(),
            };

            MatchingSymbolReceiptField = new StringFieldDefinition<CustomerLedger>(
                k => k.MatchingSymbolReceipt)
            {
                FieldName = "消込記号_消込",
                FieldNumber = 14,
                Required = false,
                Accept = VisitMatchingSymbolReceiptField,
            };
            MatchingAmountField = new NullableNumberFieldDefinition<CustomerLedger, decimal>(
            k => k.MatchingAmount)
            {
                FieldName = "消込額",
                FieldNumber = 15,
                Required = true,
                Accept = VisitMatchingAmountField,
                Format = value => value.ToString(),
            };
            RemainAmountField = new NullableNumberFieldDefinition<CustomerLedger, decimal>(
            k => k.RemainAmount)
            {
                FieldName = "残高",
                FieldNumber = 16,
                Required = true,
                Accept = VisitRemainAmountField,
                Format = value => value.ToString(),
            };
            CustomerCodeField = new StringFieldDefinition<CustomerLedger>(
                    k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 17,
                Required = false,
                Accept = VisitCustomerCodeField,
            };
            CustomerNameField = new StringFieldDefinition<CustomerLedger>(
                k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 18,
                Required = true,
                Accept = VisitCustomerNameField,
            };

            Fields.AddRange(new IFieldDefinition<CustomerLedger>[] {
                       RecordedAtField, ParentCustomerCodeField, ParentCustomerNameField, SectionNameField, DepartmentNameField
                       , InvoiceCodeField, CategoryNameField, DebitAccountTitleNameField, CurrencyCodeField, MatchingSymbolBillingField
                       , BillingAmountField, SlipTotalField, ReceiptAmountField, MatchingSymbolReceiptField, MatchingAmountField
                       , RemainAmountField, CustomerCodeField, CustomerNameField });
        }

        private bool VisitRecordedAtField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(RecordedAtField); 
        }
        private bool VisitParentCustomerCodeField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(ParentCustomerCodeField);
        }
        private bool VisitParentCustomerNameField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(ParentCustomerNameField);
        }
        private bool VisitSectionNameField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(SectionNameField); 
        }
        private bool VisitDepartmentNameField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(DepartmentNameField);
        }
        private bool VisitInvoiceCodeField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }
        private bool VisitCategoryNameField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(CategoryNameField);
        }
        private bool VisitDebitAccountTitleNameField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(DebitAccountTitleNameField);
        }
        private bool VisitCurrencyCodeField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }
        private bool VisitMatchingSymbolBillingField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(MatchingSymbolBillingField);
        }
        private bool VisitBillingAmountField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardNumber(BillingAmountField);
        }
        private bool VisitSlipTotalField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardNumber(SlipTotalField);
        }
        private bool VisitReceiptAmountField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardNumber(ReceiptAmountField);
        }
        private bool VisitMatchingSymbolReceiptField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(MatchingSymbolReceiptField);
        }
        private bool VisitMatchingAmountField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardNumber(MatchingAmountField);
        }
        private bool VisitRemainAmountField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardNumber(RemainAmountField);
        }
        private bool VisitCustomerCodeField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(CustomerCodeField); 
        }
        private bool VisitCustomerNameField(IFieldVisitor<CustomerLedger> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }
        //private bool VisitRecordTypeField(IFieldVisitor<CustomerLedger> visitor)
        //{
        //    return visitor.StandardNumber(RecordTypeField);
        //}
    }
}
