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
    public class PaymentScheduleInputFileDefinition : RowDefinition<Billing>
    {
        //public StandardIdToCodeFieldDefinition<Billing, Company> CompanyIdField { get; private set; }
        public NumberFieldDefinition<Billing, long> IdField { get; private set; }
        public StringFieldDefinition<Billing> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<Billing> CustomerCodeField { get; private set; }
        public StringFieldDefinition<Billing> CustomerNameField { get; private set; }
        public StringFieldDefinition<Billing> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<Billing> DepartmentNameField { get; private set; }
        public StringFieldDefinition<Billing> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> BillingAmountField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> DiscountAmountSummaryField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> RemainAmountField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> PaymentAmountField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> OffsetAmountField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> BilledAtField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> BillingDueAtField { get; private set; }
        public StringFieldDefinition<Billing> BillingCategoryField { get; private set; }
        public StringFieldDefinition<Billing> ScheduledPaymentKeyField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> SalesAtField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> ClosingAtField { get; private set; }
        public StringFieldDefinition<Billing> CollectCategoryField { get; private set; }
        public StringFieldDefinition<Billing> Note1Field { get; private set; }
        public StringFieldDefinition<Billing> Note2Field { get; private set; }
        public StringFieldDefinition<Billing> Note3Field { get; private set; }
        public StringFieldDefinition<Billing> Note4Field { get; private set; }
        public StringFieldDefinition<Billing> StaffCodeField { get; private set; }
        public StringFieldDefinition<Billing> StaffNameField { get; private set; }
        public StringFieldDefinition<Billing> InputTypeField { get; private set; }
        public StringFieldDefinition<Billing> BillingMemoField { get; private set; }
        public StringFieldDefinition<Billing> Note5Field { get; private set; }
        public StringFieldDefinition<Billing> Note6Field { get; private set; }
        public StringFieldDefinition<Billing> Note7Field { get;  private set;}
        public StringFieldDefinition<Billing> Note8Field { get; private set; }

        public PaymentScheduleInputFileDefinition(DataExpression expression, List<GridSetting> GridSettingInfo) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "入金予定入力";
            FileNameToken = DataTypeToken;

            int fieldNumber = 1;
            foreach (GridSetting gs in GridSettingInfo)
            {
                Func<DateTime, string> dateTimeFormatter = value => value.ToString("yyyy/MM/dd");
                fieldNumber++;
                IFieldDefinition<Billing> field = null;
                switch (gs.ColumnName)
                {
                    case nameof(Billing.BillingId):
                        field = (IdField = new NumberFieldDefinition<Billing, long>(k => k.Id,
                            accept: VisitIdField,
                            formatter: value => value.ToString()));
                        break;
                    case nameof(Billing.InvoiceCode):
                        field = (InvoiceCodeField = new StringFieldDefinition<Billing>(k => k.InvoiceCode,
                            accept: VisitInvoiceCodeField));
                        break;
                    case nameof(Billing.CustomerCode):
                        field = (CustomerCodeField = new StringFieldDefinition<Billing>(k => k.CustomerCode,
                            accept: VisitCustomerCodeField));
                        break;
                    case nameof(Billing.CustomerName):
                        field = (CustomerNameField = new StringFieldDefinition<Billing>(k => k.CustomerName,
                            accept: VisitCustomerNameField));
                        break;
                    case nameof(Billing.DepartmentCode):
                        field = (DepartmentCodeField = new StringFieldDefinition<Billing>(k => k.DepartmentCode,
                            accept: VisitDepartmentCodeField));
                        break;
                    case nameof(Billing.DepartmentName):
                        field = (DepartmentNameField = new StringFieldDefinition<Billing>(k => k.DepartmentName,
                            accept: VisitDepartmentNameField));
                        break;
                    case nameof(Billing.CurrencyCode):
                        field = (CurrencyCodeField = new StringFieldDefinition<Billing>(k => k.CurrencyCode,
                            accept: VisitCurrencyCodeField));
                        break;
                    case nameof(Billing.BillingAmount):
                        field = (BillingAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.BillingAmount,
                            accept: VisitBillingAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case "DiscountAmountSummary":
                        field = (DiscountAmountSummaryField = new NumberFieldDefinition<Billing, decimal>(k => k.DiscountAmount,
                            accept: VisitDiscountAmountSummaryField,
                            formatter: value => value.ToString()));
                        break;
                    case nameof(Billing.RemainAmount):
                        field = (RemainAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.RemainAmount,
                            accept: VisitRemainAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case nameof(Billing.PaymentAmount):
                        field = (PaymentAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.PaymentAmount,
                            accept: VisitPaymentAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case nameof(Billing.OffsetAmount):
                        field = (OffsetAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.OffsetAmount,
                            accept: VisitOffsetAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case nameof(Billing.BilledAt):
                        field = (BilledAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.BilledAt,
                            accept: VisitBilledAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case nameof(Billing.BillingDueAt):
                        field = (BillingDueAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.BillingDueAt,
                            accept: VisitBillingDueAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case "BillingCategory":
                        field = (BillingCategoryField = new StringFieldDefinition<Billing>(k => k.BillingCategoryCodeAndName,
                            accept: VisitBillingCategoryField));
                        break;
                    case nameof(Billing.ScheduledPaymentKey):
                        field = (ScheduledPaymentKeyField = new StringFieldDefinition<Billing>(k => k.ScheduledPaymentKey,
                            accept: VisitScheduledPaymentKeyField));
                        break;
                    case nameof(Billing.SalesAt):
                        field = (SalesAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.SalesAt,
                            accept: VisitSalesAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case nameof(Billing.ClosingAt):
                        field = (ClosingAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.ClosingAt,
                            accept: VisitClosingAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case "CollectCategory":
                        field = (CollectCategoryField = new StringFieldDefinition<Billing>(k => k.CollectCategoryCodeAndName,
                            accept: VisitCollectCategoryField));
                        break;
                    case nameof(Billing.Note1):
                        field = (Note1Field = new StringFieldDefinition<Billing>(k => k.Note1,
                            accept: VisitNote1Field));
                        break;
                    case nameof(Billing.Note2):
                        field = (Note2Field = new StringFieldDefinition<Billing>(k => k.Note2,
                            accept: VisitNote2Field));
                        break;
                    case nameof(Billing.Note3):
                        field = (Note3Field = new StringFieldDefinition<Billing>(k => k.Note3,
                            accept: VisitNote3Field));
                        break;
                    case nameof(Billing.Note4):
                        field = (Note4Field = new StringFieldDefinition<Billing>(k => k.Note4,
                            accept: VisitNote4Field));
                        break;
                    case nameof(Billing.StaffCode):
                        field = (StaffCodeField = new StringFieldDefinition<Billing>(k => k.StaffCode,
                            accept: VisitStaffCodeField));
                        break;
                    case nameof(Billing.StaffName):
                        field = (StaffNameField = new StringFieldDefinition<Billing>(k => k.StaffName,
                            accept: VisitStaffNameField));
                        break;
                    case nameof(Billing.InputType):
                        field = (InputTypeField = new StringFieldDefinition<Billing>(k => k.InputTypeName,
                            accept: VisitInputTypeField));
                        break;
                    case nameof(Billing.Memo):
                        field = (BillingMemoField = new StringFieldDefinition<Billing>(k => k.Memo,
                            accept: VisitBillingMemoField));
                        break;
                    case nameof(Billing.Note5):
                        field = (Note5Field = new StringFieldDefinition<Billing>(k => k.Note5,
                            accept: VisitNote5Field));
                        break;
                    case nameof(Billing.Note6):
                        field = (Note6Field = new StringFieldDefinition<Billing>(k => k.Note6,
                            accept: VisitNote6Field));
                        break;
                    case nameof(Billing.Note7):
                        field = (Note7Field = new StringFieldDefinition<Billing>(k => k.Note7,
                            accept: VisitNote7Field));
                        break;
                    case nameof(Billing.Note8):
                        field = (Note8Field = new StringFieldDefinition<Billing>(k => k.Note8,
                            accept: VisitNote8Field));
                        break;
                    default:
                        break;
                }
                if (field == null) continue;
                field.FieldName = gs.ColumnNameJp;
                field.FieldNumber = fieldNumber;
                Fields.Add(field);
            }
        }

        public IFieldDefinition<Billing> ConvertSettingToField(string column)
        {
            IFieldDefinition<Billing> field = null;
            if (column == nameof(Billing.BillingId))             field = IdField;
            if (column == nameof(Billing.InvoiceCode))           field = InvoiceCodeField;
            if (column == nameof(Billing.CustomerCode))          field = CustomerCodeField;
            if (column == nameof(Billing.CustomerName))          field = CustomerNameField;
            if (column == nameof(Billing.DepartmentCode))        field = DepartmentCodeField;
            if (column == nameof(Billing.DepartmentName))        field = DepartmentNameField;
            if (column == nameof(Billing.CurrencyCode))          field = CurrencyCodeField;
            if (column == nameof(Billing.BillingAmount))         field = BillingAmountField;
            if (column == "DiscountAmountSummary")               field = DiscountAmountSummaryField;
            if (column == nameof(Billing.RemainAmount))          field = RemainAmountField;
            if (column == nameof(Billing.PaymentAmount))         field = PaymentAmountField;
            if (column == nameof(Billing.OffsetAmount))          field = OffsetAmountField;
            if (column == nameof(Billing.BilledAt))              field = BilledAtField;
            if (column == nameof(Billing.BillingDueAt))          field = BillingDueAtField;
            if (column == "BillingCategory")                     field = BillingCategoryField;
            if (column == nameof(Billing.ScheduledPaymentKey))   field = ScheduledPaymentKeyField;
            if (column == nameof(Billing.SalesAt))               field = SalesAtField;
            if (column == nameof(Billing.ClosingAt))             field = ClosingAtField;
            if (column == "CollectCategory")                     field = CollectCategoryField;
            if (column == nameof(Billing.Note1))                 field = Note1Field;
            if (column == nameof(Billing.Note2))                 field = Note2Field;
            if (column == nameof(Billing.Note3))                 field = Note3Field;
            if (column == nameof(Billing.Note4))                 field = Note4Field;
            if (column == nameof(Billing.StaffCode))             field = StaffCodeField;
            if (column == nameof(Billing.StaffName))             field = StaffNameField;
            if (column == nameof(Billing.InputType))             field = InputTypeField;
            if (column == nameof(Billing.Memo))                  field = BillingMemoField;
            if (column == nameof(Billing.Note5))                 field = Note5Field;
            if (column == nameof(Billing.Note6))                 field = Note6Field;
            if (column == nameof(Billing.Note7))                 field = Note7Field;
            if (column == nameof(Billing.Note8))                 field = Note8Field;
            return field;
        }

        private bool VisitIdField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(IdField);
        }
        private bool VisitInvoiceCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.OwnLoginUserCode(InvoiceCodeField);
        }
        private bool VisitCustomerCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.OwnLoginUserName(CustomerCodeField);
        }
        private bool VisitCustomerNameField(IFieldVisitor<Billing> visitor)
        {
            return visitor.OwnLoginUserName(CustomerNameField);
        }
        private bool VisitDepartmentCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.OwnLoginUserCode(DepartmentCodeField);
        }
        private bool VisitDepartmentNameField(IFieldVisitor<Billing> visitor)
        {
            return visitor.OwnLoginUserName(DepartmentNameField);
        }
        private bool VisitCurrencyCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.OwnLoginUserCode(CurrencyCodeField);
        }
        private bool VisitBillingAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(BillingAmountField);
        }
        private bool VisitDiscountAmountSummaryField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DiscountAmountSummaryField);
        }
        private bool VisitRemainAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(RemainAmountField);
        }
        private bool VisitPaymentAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(PaymentAmountField);
        }
        private bool VisitOffsetAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(OffsetAmountField);
        }
        private bool VisitBilledAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(BilledAtField);
        }
        private bool VisitBillingDueAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(BillingDueAtField);
        }
        private bool VisitBillingCategoryField(IFieldVisitor<Billing> visitor)
        {
            return visitor.OwnLoginUserName(BillingCategoryField);
        }
        private bool VisitScheduledPaymentKeyField(IFieldVisitor<Billing> visitor)
        {
            return visitor.OwnLoginUserName(ScheduledPaymentKeyField);
        }
        private bool VisitSalesAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(SalesAtField);
        }
        private bool VisitClosingAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(ClosingAtField);
        }
        private bool VisitCollectCategoryField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(CollectCategoryField);
        }
        private bool VisitNote1Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(Note1Field);
        }
        private bool VisitNote2Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(Note2Field);
        }
        private bool VisitNote3Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(Note3Field);
        }
        private bool VisitNote4Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(Note4Field);
        }
        private bool VisitStaffCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(StaffCodeField);
        }
        private bool VisitStaffNameField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(StaffNameField);
        }
        private bool VisitInputTypeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(InputTypeField);
        }
        private bool VisitBillingMemoField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingMemoField);
        }
        private bool VisitNote5Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(Note5Field);
        }
        private bool VisitNote6Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(Note6Field);
        }
        private bool VisitNote7Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(Note7Field);
        }
        private bool VisitNote8Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(Note8Field);
        }
    }
}
