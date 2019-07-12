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
    public class ScheduledPaymentListFileDefinition : RowDefinition<ScheduledPaymentList>
    {
        public NumberFieldDefinition<ScheduledPaymentList, DateTime> BaseDateField { get; private set; }
        public NumberFieldDefinition<ScheduledPaymentList, int> IDField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> CustomerCodeField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> CustomerNameField { get; private set; }
        public NumberFieldDefinition<ScheduledPaymentList, DateTime> BilledAtField { get; private set; }
        public NumberFieldDefinition<ScheduledPaymentList, DateTime> SalesAtField { get; private set; }
        public NumberFieldDefinition<ScheduledPaymentList, DateTime> ClosingAtField { get; private set; }
        public NumberFieldDefinition<ScheduledPaymentList, DateTime> DueAtField { get; private set; }
        public NumberFieldDefinition<ScheduledPaymentList, DateTime> OriginalDueAtField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<ScheduledPaymentList, decimal> AmountField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> DelayDivisionField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> CodeAndNameField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> DepartmentNameField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> StaffCodeField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> StaffNameField { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> Note1Field { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> Note2Field { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> Note3Field { get; private set; }
        public StringFieldDefinition<ScheduledPaymentList> Note4Field { get; private set; }

        public ScheduledPaymentListFileDefinition(DataExpression applicationControl, List<ColumnNameSetting> ColumnNameSettingInfo)
            : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "入金予定明細表";
            FileNameToken = DataTypeToken;

            BaseDateField = new NullableNumberFieldDefinition<ScheduledPaymentList, DateTime>(k => k.BaseDate)
            {
                FieldName = "基準日",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBaseDate,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            IDField = new NumberFieldDefinition<ScheduledPaymentList, int>(k => k.Id)
            {
                FieldName = "請求ID",
                FieldNumber = 2,
                Required = false,
                Accept = VisitId,
                Format = value => value.ToString(),
            };
            CustomerCodeField = new StringFieldDefinition<ScheduledPaymentList>(k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 3,
                Required = false,
                Accept = VisitCustomerCode,
            };
            CustomerNameField = new StringFieldDefinition<ScheduledPaymentList>(k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 4,
                Required = false,
                Accept = VisitCustomerName,
            };
            BilledAtField = new NullableNumberFieldDefinition<ScheduledPaymentList, DateTime>(k => k.BilledAt)
            {
                FieldName = "請求日",
                FieldNumber = 5,
                Required = true,
                Accept = VisitBilledAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            SalesAtField = new NullableNumberFieldDefinition<ScheduledPaymentList, DateTime>(k => k.SalesAt)
            {
                FieldName = "売上日",
                FieldNumber = 6,
                Required = true,
                Accept = VisitSalesAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            ClosingAtField = new NullableNumberFieldDefinition<ScheduledPaymentList, DateTime>(k => k.ClosingAt)
            {
                FieldName = "請求締日",
                FieldNumber = 7,
                Required = true,
                Accept = VisitClosingAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            DueAtField = new NullableNumberFieldDefinition<ScheduledPaymentList, DateTime>(k => k.DueAt)
            {
                FieldName = "入金予定日",
                FieldNumber = 8,
                Required = true,
                Accept = VisitDueAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            OriginalDueAtField = new NullableNumberFieldDefinition<ScheduledPaymentList, DateTime>(k => k.OriginalDueAt)
            {
                FieldName = "当初予定日",
                FieldNumber = 9,
                Required = true,
                Accept = VisitOriginalDueAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            CurrencyCodeField = new StringFieldDefinition<ScheduledPaymentList>(k => k.CurrencyCode)
            {
                FieldName = "通貨コード",
                FieldNumber = 10,
                Required = false,
                Accept = VisitCurrencyCode,
            };
            AmountField = new NumberFieldDefinition<ScheduledPaymentList, decimal>(k => k.RemainAmount)
            {
                FieldName = "回収予定金額",
                FieldNumber = 11,
                Required = true,
                Accept = VisitAmount,
                Format = value => value.ToString(),
            };
            DelayDivisionField = new StringFieldDefinition<ScheduledPaymentList>(k => k.DelayDivision)
            {
                FieldName = "遅延",
                FieldNumber = 12,
                Required = false,
                Accept = VisitDelayDivisionField,
            };
            CodeAndNameField = new StringFieldDefinition<ScheduledPaymentList>(k => k.CodeAndName)
            {
                FieldName = "回収区分",
                FieldNumber = 13,
                Required = false,
                Accept = VisitCodeAndNameField,
            };
            InvoiceCodeField = new StringFieldDefinition<ScheduledPaymentList>(k => k.InvoiceCode)
            {
                FieldName = "請求書番号",
                FieldNumber = 14,
                Required = false,
                Accept = VisitInvoiceCodeField,
            };
            DepartmentCodeField = new StringFieldDefinition<ScheduledPaymentList>(k => k.DepartmentCode)
            {
                FieldName = "請求部門コード",
                FieldNumber = 15,
                Required = false,
                Accept = VisitDepartmentCodeField,
            };
            DepartmentNameField = new StringFieldDefinition<ScheduledPaymentList>(k => k.DepartmentName)
            {
                FieldName = "請求部門名",
                FieldNumber = 16,
                Required = false,
                Accept = VisitDepartmentNameField,
            };
            StaffCodeField = new StringFieldDefinition<ScheduledPaymentList>(k => k.StaffCode)
            {
                FieldName = "担当者コード",
                FieldNumber = 17,
                Required = false,
                Accept = VisitStaffCodeField,
            };
            StaffNameField = new StringFieldDefinition<ScheduledPaymentList>(k => k.StaffName)
            {
                FieldName = "担当者名",
                FieldNumber = 18,
                Required = false,
                Accept = VisitStaffNameField,
            };
            foreach (ColumnNameSetting gs in ColumnNameSettingInfo)
            {
                switch (gs.ColumnName)
                {
                    case "Note1":
                        Note1Field = new StringFieldDefinition<ScheduledPaymentList>(k => k.Note1)
                        {
                            FieldName = gs.DisplayColumnName,
                            FieldNumber = 19,
                            Required = false,
                            Accept = VisitNote1Field,
                        };
                        break;
                    case "Note2":
                        Note2Field = new StringFieldDefinition<ScheduledPaymentList>(k => k.Note2)
                        {
                            FieldName = gs.DisplayColumnName,
                            FieldNumber = 20,
                            Required = false,
                            Accept = VisitNote2Field,
                        };
                        break;
                    case "Note3":
                        Note3Field = new StringFieldDefinition<ScheduledPaymentList>(k => k.Note3)
                        {
                            FieldName = gs.DisplayColumnName,
                            FieldNumber = 21,
                            Required = false,
                            Accept = VisitNote3Field,
                        };
                        break;
                    case "Note4":
                        Note4Field = new StringFieldDefinition<ScheduledPaymentList>(k => k.Note4)
                        {
                            FieldName = gs.DisplayColumnName,
                            FieldNumber = 22,
                            Required = false,
                            Accept = VisitNote4Field,
                        };
                        break;
                }
            }

            Fields.AddRange(new IFieldDefinition<ScheduledPaymentList>[] {
            BaseDateField,IDField,CustomerCodeField,CustomerNameField,BilledAtField,SalesAtField,ClosingAtField,
            DueAtField,OriginalDueAtField,CurrencyCodeField,AmountField,DelayDivisionField,
            CodeAndNameField,InvoiceCodeField,DepartmentCodeField,DepartmentNameField,StaffCodeField,StaffNameField,Note1Field,
            Note2Field,Note3Field,Note4Field});
        }
        private bool VisitBaseDate(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardNumber(BaseDateField);
        }
        private bool VisitId(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardNumber(IDField);
        }
        private bool VisitCustomerCode(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }
        private bool VisitCustomerName(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }
        private bool VisitBilledAtField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardNumber(BilledAtField);
        }
        private bool VisitSalesAtField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardNumber(SalesAtField);
        }
        private bool VisitClosingAtField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardNumber(ClosingAtField);
        }
        private bool VisitDueAtField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardNumber(DueAtField);
        }
        private bool VisitOriginalDueAtField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardNumber(OriginalDueAtField);
        }
        private bool VisitCurrencyCode(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }
        private bool VisitAmount(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardNumber<decimal>(AmountField);
        }
        private bool VisitDelayDivisionField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(DelayDivisionField);
        }
        private bool VisitCodeAndNameField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(CodeAndNameField);
        }
        private bool VisitInvoiceCodeField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }
        private bool VisitDepartmentCodeField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(DepartmentCodeField);
        }
        private bool VisitDepartmentNameField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(DepartmentNameField);
        }
        private bool VisitStaffCodeField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(StaffCodeField);
        }
        private bool VisitStaffNameField(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(StaffNameField);
        }
        private bool VisitNote1Field(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(Note1Field);
        }
        private bool VisitNote2Field(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(Note2Field);
        }
        private bool VisitNote3Field(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(Note3Field);
        }
        private bool VisitNote4Field(IFieldVisitor<ScheduledPaymentList> visitor)
        {
            return visitor.StandardString(Note4Field);
        }
    }
}
