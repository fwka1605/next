using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class BillingSearchFileDefinition : RowDefinition<Billing>
    {
        public NullableNumberFieldDefinition<Billing, DateTime> DeleteAtField { get; private set; }
        public NumberFieldDefinition<Billing, long> IdField { get; private set; }
        public StringFieldDefinition<Billing> AssignmentFlagField { get; private set; }
        public StringFieldDefinition<Billing> CustomerCodeField { get; private set; }
        public StringFieldDefinition<Billing> CustomerNameField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> BilledAtField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> SalesAtField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> ClosingAtField { get; private set; }
        public NumberFieldDefinition<Billing, DateTime> DueAtField { get; private set; }
        public StringFieldDefinition<Billing> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> BillingAmountField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> RemainAmountField { get; private set; }
        public StringFieldDefinition<Billing> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<Billing> BillingCategoryField { get; private set; }
        public StringFieldDefinition<Billing> CollectCategoryField { get; private set; }
        public StringFieldDefinition<Billing> InputTypeField { get; private set; }
        public StringFieldDefinition<Billing> Note1Field { get; private set; }
        public StringFieldDefinition<Billing> Note2Field { get; private set; }
        public StringFieldDefinition<Billing> Note3Field { get; private set; }
        public StringFieldDefinition<Billing> Note4Field { get; private set; }
        public StringFieldDefinition<Billing> Note5Field { get; private set; }
        public StringFieldDefinition<Billing> Note6Field { get; private set; }
        public StringFieldDefinition<Billing> Note7Field { get; private set; }
        public StringFieldDefinition<Billing> Note8Field { get; private set; }
        public StringFieldDefinition<Billing> BillingMemoField { get; private set; }
        public StringFieldDefinition<Billing> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<Billing> DepartmentNameField { get; private set; }
        public StringFieldDefinition<Billing> StaffCodeField { get; private set; }
        public StringFieldDefinition<Billing> StaffNameField { get; private set; }
        public StringFieldDefinition<Billing> ContractNumberField { get; private set; }
        public StringFieldDefinition<Billing> ConfirmField { get; private set; }
        public NullableNumberFieldDefinition<Billing, DateTime> RequestDateField { get; private set; }
        public StringFieldDefinition<Billing> ResultCodeField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> DiscountAmount1Field { get; private set; }
        public NumberFieldDefinition<Billing, decimal> DiscountAmount2Field { get; private set; }
        public NumberFieldDefinition<Billing, decimal> DiscountAmount3Field { get; private set; }
        public NumberFieldDefinition<Billing, decimal> DiscountAmount4Field { get; private set; }
        public NumberFieldDefinition<Billing, decimal> DiscountAmount5Field { get; private set; }
        public NumberFieldDefinition<Billing, decimal> DiscountAmountTotalField { get; private set; }
        public NullableNumberFieldDefinition<Billing, DateTime> FirstRecordedAtField { get; private set; }
        public NullableNumberFieldDefinition<Billing, DateTime> LastRecordedAtField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> PriceField { get; private set; }
        public NumberFieldDefinition<Billing, decimal> TaxAmountField { get; private set; }

        public BillingSearchFileDefinition(DataExpression expression, List<GridSetting> GridSettingInfo) : base(expression)
        {
            Func<DateTime, string> dateTimeFormatter = value => value.ToString("yyyy/MM/dd");
            StartLineNumber = 1;
            DataTypeToken = "請求データ";
            FileNameToken = DataTypeToken;

            Fields.Add(DeleteAtField = new NullableNumberFieldDefinition<Billing, DateTime>(k => k.DeleteAt,
                fieldName: "削除日", fieldNumber: 1,
                accept: VisitDeleteAtField,
                formatter: dateTimeFormatter));

            int fieldNumber = 1;
            foreach (GridSetting gs in GridSettingInfo)
            {
                fieldNumber++;
                IFieldDefinition<Billing> field = null;
                switch (gs.ColumnName)
                {
                    case "Id":
                        field = (IdField = new NumberFieldDefinition<Billing, long>(k => k.Id,
                            accept: VisitIdField,
                            formatter: value => value.ToString()));
                        break;
                    case "InvoiceCode":
                        field = (InvoiceCodeField = new StringFieldDefinition<Billing>(k => k.InvoiceCode,
                            accept: VisitInvoiceCodeField));
                        break;
                    case "BillingCategory":
                        field = (BillingCategoryField = new StringFieldDefinition<Billing>(k => k.BillingCategoryCodeAndName,
                            accept: VisitBillingCategoryField));
                        break;
                    case "CollectCategory":
                        field = (CollectCategoryField = new StringFieldDefinition<Billing>(k => k.CollectCategoryCodeAndName,
                            accept: VisitCollectCategoryField));
                        break;
                    case "InputType":
                        field = (InputTypeField = new StringFieldDefinition<Billing>(k => k.InputTypeName,
                            accept: VisitInputTypeField));
                        break;
                    case "Note1":
                        field = (Note1Field = new StringFieldDefinition<Billing>(k => k.Note1, accept: VisitNote1Field));
                        break;
                    case "Note2":
                        field = (Note2Field = new StringFieldDefinition<Billing>(k => k.Note2, accept: VisitNote2Field));
                        break;
                    case "Note3":
                        field = (Note3Field = new StringFieldDefinition<Billing>(k => k.Note3, accept: VisitNote3Field));
                        break;
                    case "Note4":
                        field = (Note4Field = new StringFieldDefinition<Billing>(k => k.Note4, accept: VisitNote4Field));
                        break;
                    case "Note5":
                        field = (Note5Field = new StringFieldDefinition<Billing>(k => k.Note5, accept: x => x.StandardString(Note5Field)));
                        break;
                    case "Note6":
                        field = (Note6Field = new StringFieldDefinition<Billing>(k => k.Note6, accept: x => x.StandardString(Note6Field)));
                        break;
                    case "Note7":
                        field = (Note7Field = new StringFieldDefinition<Billing>(k => k.Note7, accept: x => x.StandardString(Note7Field)));
                        break;
                    case "Note8":
                        field = (Note8Field = new StringFieldDefinition<Billing>(k => k.Note8, accept: x => x.StandardString(Note8Field)));
                        break;
                    case "Memo":
                        field = (BillingMemoField = new StringFieldDefinition<Billing>(k => k.Memo,
                            accept: VisitBillingMemoField));
                        break;
                    case "CustomerCode":
                        field = (CustomerCodeField = new StringFieldDefinition<Billing>(k => k.CustomerCode,
                            accept: VisitCustomerCodeField));
                        break;
                    case "StaffCode":
                        field = (StaffCodeField = new StringFieldDefinition<Billing>(k => k.StaffCode,
                            accept: VisitStaffCodeField));
                        break;
                    case "ResultCode":
                        field = (ResultCodeField = new StringFieldDefinition<Billing>(k => k.ResultCodeName,
                            accept: VisitResultCodeField));
                        break;
                    case "ContractNumber":
                        field = (ContractNumberField = new StringFieldDefinition<Billing>(k => k.ContractNumber,
                            accept: VisitContractNumberField));
                        break;
                    case "Confirm":
                        field = (ConfirmField = new StringFieldDefinition<Billing>(k => k.ConfirmName,
                            accept: VisitConfirmField));
                        break;
                    case "CurrencyCode":
                        field = (CurrencyCodeField = new StringFieldDefinition<Billing>(k => k.CurrencyCode,
                            accept: VisitCurrencyCodeField));
                        break;
                    case "CustomerName":
                        field = (CustomerNameField = new StringFieldDefinition<Billing>(k => k.CustomerName,
                            accept: VisitCustomerNameField));
                        break;
                    case "StaffName":
                        field = (StaffNameField = new StringFieldDefinition<Billing>(k => k.StaffName,
                            accept: VisitStaffNameField));
                        break;
                    case "AssignmentState":
                        field = (AssignmentFlagField = new StringFieldDefinition<Billing>(k => k.AssignmentFlagName,
                            accept: VisitAssignmentFlagField));
                        break;
                    case "BilledAt" :
                        field = (BilledAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.BilledAt,
                            accept: VisitBilledAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case "SalesAt":
                        field = (SalesAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.SalesAt,
                            accept: VisitSalesAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case "ClosingAt":
                        field = (ClosingAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.ClosingAt,
                            accept: VisitClosingAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case "DueAt":
                        field = (DueAtField = new NumberFieldDefinition<Billing, DateTime>(k => k.DueAt,
                            accept: VisitDueAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case "RequestDate":
                        field = (RequestDateField = new NullableNumberFieldDefinition<Billing, DateTime>(k => k.RequestDate,
                            accept: VisitRequestField,
                            formatter: dateTimeFormatter));
                        break;
                    case "BillingAmount":
                        field = (BillingAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.BillingAmount,
                            accept: VisitBillingAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case "RemainAmount":
                        field = (RemainAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.RemainAmount,
                            accept: VisitRemainAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case "DiscountAmount1":
                        field = (DiscountAmount1Field = new NumberFieldDefinition<Billing, decimal>(k => k.DiscountAmount1,
                            accept: VisitDiscountAmount1Field,
                            formatter: value => value.ToString()));
                        break;
                    case "DiscountAmount2":
                        field = (DiscountAmount2Field = new NumberFieldDefinition<Billing, decimal>(k => k.DiscountAmount2,
                            accept: VisitDiscountAmount2Field,
                            formatter: value => value.ToString()));
                        break;
                    case "DiscountAmount3":
                        field = (DiscountAmount3Field = new NumberFieldDefinition<Billing, decimal>(k => k.DiscountAmount3,
                            accept: VisitDiscountAmount3Field,
                            formatter: value => value.ToString()));
                        break;
                    case "DiscountAmount4":
                        field = (DiscountAmount4Field = new NumberFieldDefinition<Billing, decimal>(k => k.DiscountAmount4,
                            accept: VisitDiscountAmount4Field,
                            formatter: value => value.ToString()));
                        break;
                    case "DiscountAmount5":
                        field = (DiscountAmount5Field = new NumberFieldDefinition<Billing, decimal>(k => k.DiscountAmount5,
                            accept: VisitDiscountAmount5Field,
                            formatter: value => value.ToString()));
                        break;
                    case "DepartmentCode":
                        field = (DepartmentCodeField = new StringFieldDefinition<Billing>(k => k.DepartmentCode,
                            accept: VisitDepartmentCodeField));
                        break;
                    case "DepartmentName":
                        field = (DepartmentNameField = new StringFieldDefinition<Billing>(k => k.DepartmentName,
                            accept: VisitDepartmentNameField));
                        break;
                    case "DiscountAmountSummary":
                        field = (DiscountAmountTotalField = new NumberFieldDefinition<Billing, decimal>(k => k.DiscountAmount,
                            accept: VisitDiscountAmountTotalField,
                            formatter: value => value.ToString()));
                        break;
                    case "FirstRecordedAt":
                        field = (FirstRecordedAtField = new NullableNumberFieldDefinition<Billing, DateTime>(k => k.FirstRecordedAt,
                            accept: VisitFirstRecordedAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case "LastRecordedAt":
                        field = (LastRecordedAtField = new NullableNumberFieldDefinition<Billing, DateTime>(k => k.LastRecordedAt,
                            accept: VisitLastRecordedAtField,
                            formatter: dateTimeFormatter));
                        break;
                    case "Price":
                        field = (PriceField = new NumberFieldDefinition<Billing, decimal>(k => k.BillingAmountExcludingTax,
                            accept: VisitPriceField,
                            formatter: value => value.ToString()));
                        break;
                    case "TaxAmount":
                        field = (TaxAmountField = new NumberFieldDefinition<Billing, decimal>(k => k.TaxAmount,
                            accept: VisitTaxAmountField,
                            formatter: value => value.ToString()));
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
            if (column == "Id")                     field = IdField;
            if (column == "AssignmentState")        field = AssignmentFlagField;
            if (column == "CustomerName")           field = CustomerNameField;
            if (column == "CustomerCode")           field = CustomerCodeField;
            if (column == "BilledAt")               field = BilledAtField;
            if (column == "SalesAt")                field = SalesAtField;
            if (column == "ClosingAt")              field = ClosingAtField;
            if (column == "DueAt")                  field = DueAtField;
            if (column == "CurrencyCode")           field = CurrencyCodeField;
            if (column == "BillingAmount")          field = BillingAmountField;
            if (column == "RemainAmount")           field = RemainAmountField;
            if (column == "InvoiceCode")            field = InvoiceCodeField;
            if (column == "BillingCategory")        field = BillingCategoryField;
            if (column == "CollectCategory")        field = CollectCategoryField;
            if (column == "InputType")              field = InputTypeField;
            if (column == "Note1")                  field = Note1Field;
            if (column == "Memo")                   field = BillingMemoField;
            if (column == "DepartmentCode")         field = DepartmentCodeField;
            if (column == "DepartmentName")         field = DepartmentNameField;
            if (column == "StaffCode")              field = StaffCodeField;
            if (column == "StaffName")              field = StaffNameField;
            if (column == "ContractNumber")         field = ContractNumberField;
            if (column == "Confirm")                field = ConfirmField;
            if (column == "RequestDate")            field = RequestDateField;
            if (column == "ResultCode")             field = ResultCodeField;
            if (column == "Note2")                  field = Note2Field;
            if (column == "Note3")                  field = Note3Field;
            if (column == "Note4")                  field = Note4Field;
            if (column == "Note5")                  field = Note5Field;
            if (column == "Note6")                  field = Note6Field;
            if (column == "Note7")                  field = Note7Field;
            if (column == "Note8")                  field = Note8Field;
            if (column == "DiscountAmount1")        field = DiscountAmount1Field;
            if (column == "DiscountAmount2")        field = DiscountAmount2Field;
            if (column == "DiscountAmount3")        field = DiscountAmount3Field;
            if (column == "DiscountAmount4")        field = DiscountAmount4Field;
            if (column == "DiscountAmount5")        field = DiscountAmount5Field;
            if (column == "DiscountAmountSummary")  field = DiscountAmountTotalField;
            if (column == "FirstRecordedAt")        field = FirstRecordedAtField;
            if (column == "LastRecordedAt")         field = LastRecordedAtField;
            if (column == "Price")                  field = PriceField;
            if (column == "TaxAmount")              field = TaxAmountField;
            return field;
        }

        private bool VisitDeleteAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DeleteAtField);
        }

        private bool VisitIdField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(IdField);
        }

        private bool VisitAssignmentFlagField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(AssignmentFlagField);
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

        private bool VisitSalesAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(SalesAtField);
        }

        private bool VisitClosingAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(ClosingAtField);
        }

        private bool VisitDueAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DueAtField);
        }

        private bool VisitCurrencyCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }

        private bool VisitBillingAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(BillingAmountField);
        }

        private bool VisitRemainAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(RemainAmountField);
        }

        private bool VisitInvoiceCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }

        private bool VisitBillingCategoryField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingCategoryField);
        }

        private bool VisitCollectCategoryField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(CollectCategoryField);
        }

        private bool VisitInputTypeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(InputTypeField);
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

        private bool VisitBillingMemoField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(BillingMemoField);
        }

        private bool VisitDepartmentCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(DepartmentCodeField);
        }

        private bool VisitDepartmentNameField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(DepartmentNameField);
        }

        private bool VisitStaffCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(StaffCodeField);
        }

        private bool VisitStaffNameField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(StaffNameField);
        }

        private bool VisitContractNumberField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(ContractNumberField);
        }

        private bool VisitConfirmField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(ConfirmField);
        }

        private bool VisitRequestField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(RequestDateField);
        }

        private bool VisitResultCodeField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardString(ResultCodeField);
        }

        private bool VisitDiscountAmount1Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DiscountAmount1Field);
        }

        private bool VisitDiscountAmount2Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DiscountAmount2Field);
        }

        private bool VisitDiscountAmount3Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DiscountAmount3Field);
        }

        private bool VisitDiscountAmount4Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DiscountAmount4Field);
        }

        private bool VisitDiscountAmount5Field(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DiscountAmount5Field);
        }

        private bool VisitDiscountAmountTotalField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(DiscountAmountTotalField);
        }
        private bool VisitFirstRecordedAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(FirstRecordedAtField);
        }
        private bool VisitLastRecordedAtField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(LastRecordedAtField);
        }
        private bool VisitPriceField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(PriceField);
        }
        private bool VisitTaxAmountField(IFieldVisitor<Billing> visitor)
        {
            return visitor.StandardNumber(TaxAmountField);
        }
    }
}
