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
    public class ArrearagesListFileDefinition : RowDefinition<ArrearagesList>
    {
        public NumberFieldDefinition<ArrearagesList, DateTime> BaseDateField { get; private set; }
        public NumberFieldDefinition<ArrearagesList, long> IDField { get; private set; }
        public StringFieldDefinition<ArrearagesList> CustomerCodeField { get; private set; }
        public StringFieldDefinition<ArrearagesList> CustomerNameField { get; private set; }
        public NumberFieldDefinition<ArrearagesList, DateTime> BilledAtField { get; private set; }
        public NumberFieldDefinition<ArrearagesList, DateTime> SalesAtField { get; private set; }
        public NumberFieldDefinition<ArrearagesList, DateTime> ClosingAtField { get; private set; }
        public NumberFieldDefinition<ArrearagesList, DateTime> DueAtField { get; private set; }
        public NumberFieldDefinition<ArrearagesList, DateTime> OriginalDueAtField { get; private set; }
        public StringFieldDefinition<ArrearagesList> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<ArrearagesList, decimal> AmountField { get; private set; }
        public NumberFieldDefinition<ArrearagesList, int> ArrearagesDayCountField { get; private set; }
        public StringFieldDefinition<ArrearagesList>CodeAndNameField { get; private set; }
        public StringFieldDefinition<ArrearagesList> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<ArrearagesList> Note1Field { get; private set; }
        public StringFieldDefinition<ArrearagesList> MemoField { get; private set; }
        public StringFieldDefinition<ArrearagesList> CustomerStaffNameField { get; private set; }
        public StringFieldDefinition<ArrearagesList> NoteField { get; private set; }
        public StringFieldDefinition<ArrearagesList> TelField { get; private set; }
        public StringFieldDefinition<ArrearagesList> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<ArrearagesList> DepartmentNameField { get; private set; }
        public StringFieldDefinition<ArrearagesList> StaffCodeField { get; private set; }
        public StringFieldDefinition<ArrearagesList> StaffNameField { get; private set; }
        public StringFieldDefinition<ArrearagesList> Note2Field { get; private set; }
        public StringFieldDefinition<ArrearagesList> Note3Field { get; private set; }
        public StringFieldDefinition<ArrearagesList> Note4Field { get; private set; }

        public ArrearagesListFileDefinition(DataExpression applicationControl, List<ColumnNameSetting> ColumnNameSettingInfo)
            : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "滞留明細一覧表";
            FileNameToken = DataTypeToken;

            BaseDateField = new NumberFieldDefinition<ArrearagesList, DateTime>(k => k.BaseDate)
            {
                FieldName = "入金基準日",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBaseDate,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            IDField = new NumberFieldDefinition<ArrearagesList, long>(k => k.Id)
            {
                FieldName = "請求ID",
                FieldNumber = 2,
                Required = false,
                Accept = VisitId,
                Format = value => value.ToString(),
            };
            CustomerCodeField = new StringFieldDefinition<ArrearagesList>(k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 3,
                Required = false,
                Accept = VisitCustomerCode,
            };
            CustomerNameField = new StringFieldDefinition<ArrearagesList>(k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 4,
                Required = false,
                Accept = VisitCustomerName,
            };
            BilledAtField = new NumberFieldDefinition<ArrearagesList, DateTime>(k => k.BilledAt)
            {
                FieldName = "請求日",
                FieldNumber = 5,
                Required = true,
                Accept = VisitBilledAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            SalesAtField = new NumberFieldDefinition<ArrearagesList, DateTime>(k => k.SalesAt)
            {
                FieldName = "売上日",
                FieldNumber = 6,
                Required = true,
                Accept = VisitSalesAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            ClosingAtField = new NumberFieldDefinition<ArrearagesList, DateTime>(k => k.ClosingAt)
            {
                FieldName = "請求締日",
                FieldNumber = 7,
                Required = true,
                Accept = VisitClosingAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            DueAtField = new NumberFieldDefinition<ArrearagesList, DateTime>(k => k.DueAt)
            {
                FieldName = "入金予定日",
                FieldNumber = 8,
                Required = true,
                Accept = VisitDueAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            OriginalDueAtField = new NumberFieldDefinition<ArrearagesList, DateTime>(k => k.OriginalDueAt)
            {
                FieldName = "当初予定日",
                FieldNumber = 9,
                Required = true,
                Accept = VisitOriginalDueAtField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            CurrencyCodeField = new StringFieldDefinition<ArrearagesList>(k => k.CurrencyCode)
            {
                FieldName = "通貨コード",
                FieldNumber = 10,
                Required = false,
                Accept = VisitCurrencyCode,
            };
            AmountField = new NumberFieldDefinition<ArrearagesList, decimal>(k => k.RemainAmount)
            {
                FieldName = "回収予定金額",
                FieldNumber = 11,
                Required = true,
                Accept = VisitAmount,
                Format = value => value.ToString(),
            };
            ArrearagesDayCountField = new NumberFieldDefinition<ArrearagesList, int>(k => k.ArrearagesDayCount)
            {
                FieldName = "滞留日数",
                FieldNumber = 12,
                Required = false,
                Accept = VisitArrearagesDayCountField,
                Format = value => value.ToString(),
            };
            CodeAndNameField = new StringFieldDefinition<ArrearagesList>(k => k.CodeAndName)
            {
                FieldName = "回収区分",
                FieldNumber = 13,
                Required = false,
                Accept = VisitCodeAndNameField,
            };
            InvoiceCodeField = new StringFieldDefinition<ArrearagesList>(k => k.InvoiceCode)
            {
                FieldName = "請求書番号",
                FieldNumber = 14,
                Required = false,
                Accept = VisitInvoiceCodeField,
            };
            foreach (ColumnNameSetting gs in ColumnNameSettingInfo)
            {
                switch (gs.ColumnName)
                {
                    case "Note1":
                        Note1Field = new StringFieldDefinition<ArrearagesList>(k => k.Note1)
                        {
                            FieldName = gs.DisplayColumnName,
                            FieldNumber = 15,
                            Required = false,
                            Accept = VisitNote1Field,
                        };
                        break;
                }
            }
            MemoField = new StringFieldDefinition<ArrearagesList>(k => k.Memo)
            {
                FieldName = "メモ",
                FieldNumber = 16,
                Required = false,
                Accept = VisitMemoField,
            };
            CustomerStaffNameField = new StringFieldDefinition<ArrearagesList>(k => k.CustomerStaffName)
            {
                FieldName = "相手先担当者",
                FieldNumber = 17,
                Required = false,
                Accept = VisitCustomerStaffNameField,
            };
            NoteField = new StringFieldDefinition<ArrearagesList>(k => k.CustomerNote)
            {
                FieldName = "得意先備考",
                FieldNumber = 18,
                Required = false,
                Accept = VisitNoteField,
            };
            TelField = new StringFieldDefinition<ArrearagesList>(k => k.Tel)
            {
                FieldName = "電話番号",
                FieldNumber = 19,
                Required = true,
                Accept = VisitTelField,
            };
            DepartmentCodeField = new StringFieldDefinition<ArrearagesList>(k => k.DepartmentCode)
            {
                FieldName = "請求部門コード",
                FieldNumber = 20,
                Required = false,
                Accept = VisitDepartmentCodeField,
            };
            DepartmentNameField = new StringFieldDefinition<ArrearagesList>(k => k.DepartmentName)
            {
                FieldName = "請求部門名",
                FieldNumber = 21,
                Required = false,
                Accept = VisitDepartmentNameField,
            };
            StaffCodeField = new StringFieldDefinition<ArrearagesList>(k => k.StaffCode)
            {
                FieldName = "担当者コード",
                FieldNumber = 22,
                Required = false,
                Accept = VisitStaffCodeField,
            };
            StaffNameField = new StringFieldDefinition<ArrearagesList>(k => k.StaffName)
            {
                FieldName = "担当者名",
                FieldNumber = 23,
                Required = false,
                Accept = VisitStaffNameField,
            };
            foreach (ColumnNameSetting gs in ColumnNameSettingInfo)
            {
                switch (gs.ColumnName)
                {
                    case "Note2":
                        Note2Field = new StringFieldDefinition<ArrearagesList>(k => k.Note2)
                        {
                            FieldName = gs.DisplayColumnName,
                            FieldNumber = 24,
                            Required = false,
                            Accept = VisitNote2Field,
                        };
                        break;
                    case "Note3":
                        Note3Field = new StringFieldDefinition<ArrearagesList>(k => k.Note3)
                        {
                            FieldName = gs.DisplayColumnName,
                            FieldNumber = 25,
                            Required = false,
                            Accept = VisitNote3Field,
                        };
                        break;
                    case "Note4":
                        Note4Field = new StringFieldDefinition<ArrearagesList>(k => k.Note4)
                        {
                            FieldName = gs.DisplayColumnName,
                            FieldNumber = 26,
                            Required = false,
                            Accept = VisitNote4Field,
                        };
                        break;
                }
            }

            Fields.AddRange(new IFieldDefinition<ArrearagesList>[] {
            BaseDateField,IDField,CustomerCodeField,CustomerNameField,BilledAtField,SalesAtField,ClosingAtField,
            DueAtField,OriginalDueAtField,CurrencyCodeField,AmountField,ArrearagesDayCountField,
            CodeAndNameField,InvoiceCodeField,Note1Field,MemoField,CustomerStaffNameField,
            NoteField,TelField,DepartmentCodeField,DepartmentNameField,StaffCodeField,StaffNameField,
            Note2Field,Note3Field,Note4Field});
        }

        private bool VisitBaseDate(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(BaseDateField);
        }
        private bool VisitId(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(IDField);
        }
        private bool VisitCustomerCode(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }
        private bool VisitCustomerName(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }
        private bool VisitBilledAtField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(BilledAtField);
        }
        private bool VisitSalesAtField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(SalesAtField);
        }
        private bool VisitClosingAtField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(ClosingAtField);
        }
        private bool VisitDueAtField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(DueAtField);
        }
        private bool VisitOriginalDueAtField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(OriginalDueAtField);
        }
        private bool VisitCurrencyCode(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }
        private bool VisitAmount(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(AmountField);
        }
        private bool VisitArrearagesDayCountField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardNumber(ArrearagesDayCountField);
        }
        private bool VisitCodeAndNameField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(CodeAndNameField);
        }
        private bool VisitInvoiceCodeField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }
        private bool VisitNote1Field(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(Note1Field);
        }
        private bool VisitMemoField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(MemoField);
        }
        private bool VisitCustomerStaffNameField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(CustomerStaffNameField);
        }
        private bool VisitNoteField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(NoteField);
        }
        private bool VisitTelField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(TelField);
        }
        private bool VisitDepartmentCodeField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(DepartmentCodeField);
        }
        private bool VisitDepartmentNameField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(DepartmentNameField);
        }
        private bool VisitStaffCodeField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(StaffCodeField);
        }
        private bool VisitStaffNameField(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(StaffNameField);
        }
        private bool VisitNote2Field(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(Note2Field);
        }
        private bool VisitNote3Field(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(Note3Field);
        }
        private bool VisitNote4Field(IFieldVisitor<ArrearagesList> visitor)
        {
            return visitor.StandardString(Note4Field);
        }
    }
}
