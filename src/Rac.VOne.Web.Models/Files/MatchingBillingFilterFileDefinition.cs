using Rac.VOne.Common.DataHandling;
using System;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Models.Files
{
    public class MatchingBillingFilterFileDefinition : RowDefinition<Billing>
    {
        public StringFieldDefinition<Billing> CurrencyCodeField { get; private set; }
        public StringFieldDefinition<Billing> BillCheckField { get; private set; }
        public StringFieldDefinition<Billing> CustomerCodeField { get; private set; }
        public StringFieldDefinition<Billing> CustomerNameField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> BilledAtField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> DueAtField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> BillingAmountField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> RemainAmountField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> TargetAmountField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> DiscountAmountField { get; private set; }
        public StringFieldDefinition<Billing> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<Billing> BillingCategoryField { get; private set; }
        public StringFieldDefinition<Billing> BillingDepartmentField { get; private set; }
        public StringFieldDefinition<Billing> BillingNote1Field { get; private set; }
        public StringFieldDefinition<Billing> BillingNote2Field { get; private set; }
        public StringFieldDefinition<Billing> BillingNote3Field { get; private set; }
        public StringFieldDefinition<Billing> BillingNote4Field { get; private set; }
        public StringFieldDefinition<Billing> BillingMemoField { get; private set; }
        public StringFieldDefinition<Billing> BillingInputTypeField { get; private set; }
        public StringFieldDefinition<Billing> ScheduledPaymentKeyField { get; private set; }

        public MatchingBillingFilterFileDefinition(DataExpression applicationControl) 
            : base(applicationControl)
        {
            //請求データ
            StartLineNumber = 1;
            DataTypeToken = "個別消込";
            FileNameToken = DataTypeToken;

            CurrencyCodeField = new StringFieldDefinition<Billing>(k => k.CurrencyCode)
            {
                FieldName = "通貨コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCurrencyCodeField,
            };
            BillCheckField = new StringFieldDefinition<Billing>(k => k.BillCheck)
            {
                FieldName = "消",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillCheckField,
            };
            CustomerCodeField = new StringFieldDefinition<Billing>(k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCustomerCodeField,
            };
            CustomerNameField = new StringFieldDefinition<Billing>(k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCustomerNameField,
            };
            BilledAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.BilledAt)
            {
                FieldName = "請求日",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBilledAtField,
                Format = value => value.ToShortDateString(),
            };
            DueAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.DueAt)
            {
                FieldName = "予定日",
                FieldNumber = 1,
                Required = false,
                Accept = VisitDueAtField,
                Format = value => value.ToShortDateString(),
            };
            BillingAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.BillingAmount)
            {
                FieldName = "請求額",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingAmountField,
                Format = value => value.ToString(),
            };
            RemainAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.RemainAmount)
            {
                FieldName = "請求残",
                FieldNumber = 1,
                Required = false,
                Accept = VisitRemainAmountField,
                Format = value => value.ToString(),
            };
            TargetAmountField = new NumberFieldDefinition<Billing,decimal>(k => k.TargetAmount)
            {
                FieldName = "消込対象額",
                FieldNumber = 1,
                Required = false,
                Accept = VisitTargetAmountField,
                Format = value => value.ToString(),
            };
            DiscountAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.DiscountAmount)
            {
                FieldName = "歩引額",
                FieldNumber = 1,
                Required = false,
                Accept = VisitDiscountAmountField,
                Format = value => value.ToString(),
            };
            InvoiceCodeField = new StringFieldDefinition<Billing>(k => k.InvoiceCode)
            {
                FieldName = "請求番号",
                FieldNumber = 1,
                Required = false,
                Accept = VisitInvoiceCode,
            };
            BillingCategoryField = new StringFieldDefinition<Billing>(k => k.BillingCategoryCodeAndName)
            {
                FieldName = "請求区分",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingCategoryField,
            };
            BillingDepartmentField = new StringFieldDefinition<Billing>(k => k.DepartmentName)
            {
                FieldName = "請求部門名",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingDepartmentField,
            };
            BillingNote1Field = new StringFieldDefinition<Billing>(k => k.Note1)
            {
                FieldName = "備考",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingNote1Field,
            };
            BillingNote2Field = new StringFieldDefinition<Billing>(k => k.Note2)
            {
                FieldName = "備考2",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingNote2Field,
            };
            BillingNote3Field = new StringFieldDefinition<Billing>(k => k.Note3)
            {
                FieldName = "備考3",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingNote3Field,
            };
            BillingNote4Field = new StringFieldDefinition<Billing>(k => k.Note4)
            {
                FieldName = "備考4",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingNote4Field,
            };
            BillingMemoField = new StringFieldDefinition<Billing>(k => k.Memo)
            {
                FieldName = "請求メモ",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingMemoField,
            };
            BillingInputTypeField = new StringFieldDefinition<Billing>(k => k.InputTypeName)
            {
                FieldName = "データ区分",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingInputTypeField,
            };
            ScheduledPaymentKeyField = new StringFieldDefinition<Billing>(k => k.ScheduledPaymentKey)
            {
                FieldName = "入金予定キー",
                FieldNumber = 1,
                Required = false,
                Accept = VisitScheduledPaymentKeyField,
            };

            Fields.AddRange(new IFieldDefinition<Billing>[]
           {
                CurrencyCodeField
                ,BillCheckField
                ,CustomerCodeField
                ,CustomerNameField
                ,BilledAtField
                ,DueAtField
                ,BillingAmountField
                ,RemainAmountField
                ,TargetAmountField
                ,DiscountAmountField
                ,InvoiceCodeField
                ,BillingCategoryField
                ,BillingDepartmentField
                ,BillingNote1Field
                ,BillingNote2Field
                ,BillingNote3Field
                ,BillingNote4Field
                ,BillingMemoField
                ,BillingInputTypeField
                ,ScheduledPaymentKeyField
           });
        }
        private bool VisitCurrencyCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }

        private bool VisitBillCheckField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillCheckField);
        }

        private bool VisitCustomerCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }

        private bool VisitCustomerNameField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }

        private bool VisitBilledAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(BilledAtField);
        }

        private bool VisitDueAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DueAtField);
        }

        private bool VisitBillingAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(BillingAmountField);
        }

        private bool VisitRemainAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(RemainAmountField);
        }

        private bool VisitTargetAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(TargetAmountField);
        }

        private bool VisitDiscountAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DiscountAmountField);
        }

        private bool VisitInvoiceCode(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }

        private bool VisitBillingCategoryField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingCategoryField);
        }

        private bool VisitBillingDepartmentField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingDepartmentField);
        }

        private bool VisitBillingNote1Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingNote1Field);
        }

        private bool VisitBillingNote2Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingNote2Field);
        }

        private bool VisitBillingNote3Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingNote3Field);
        }

        private bool VisitBillingNote4Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingNote4Field);
        }

        private bool VisitBillingMemoField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingMemoField);
        }

        private bool VisitBillingInputTypeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingInputTypeField);
        }

        private bool VisitScheduledPaymentKeyField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(ScheduledPaymentKeyField);
        }
    }
}
