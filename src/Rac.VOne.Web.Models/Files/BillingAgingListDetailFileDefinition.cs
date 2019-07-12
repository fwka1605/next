using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class BillingAgingListDetailFileDefinition : RowDefinition<BillingAgingListDetail>
    {
        public StringFieldDefinition<BillingAgingListDetail> CustomerCodeField { get; private set; }
        public StringFieldDefinition<BillingAgingListDetail> CustomerNameField { get; private set; }
        public StringFieldDefinition<BillingAgingListDetail> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<BillingAgingListDetail, DateTime> BilledAtField { get; private set; }
        public NumberFieldDefinition<BillingAgingListDetail,DateTime> DueAtField { get; private set; }
        public NumberFieldDefinition<BillingAgingListDetail,DateTime> SalesAtField { get; private set; }
        public NumberFieldDefinition<BillingAgingListDetail,decimal> BillingAmountField { get; private set; }
        public NumberFieldDefinition<BillingAgingListDetail, decimal> RemainAmountField { get; private set; }
        public StringFieldDefinition<BillingAgingListDetail> StaffCodeField { get; private set; }
        public StringFieldDefinition<BillingAgingListDetail> StaffNameField { get; private set; }
        public StringFieldDefinition<BillingAgingListDetail> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<BillingAgingListDetail> NoteField { get; private set; }
        public BillingAgingListDetailFileDefinition(DataExpression expression, ColumnNameSetting ColumnNameSetting)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "請求残高年齢表";
            FileNameToken = DataTypeToken;

            CustomerCodeField = new StringFieldDefinition<BillingAgingListDetail>(
                    k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCustomerCodeField,
            };
            CustomerNameField = new StringFieldDefinition<BillingAgingListDetail>(
                k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCustomerNameField,
            };
            CurrencyCodeField = new StringFieldDefinition<BillingAgingListDetail>(
                k => k.CurrencyCode)
            {
                FieldName = "通貨コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCurrencyCodeField,
            };
            BilledAtField = new NumberFieldDefinition<BillingAgingListDetail, DateTime>(
                k => k.BilledAt)
            {
                FieldName = "請求日",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBilledAtField,
                Format = value => value.ToShortDateString(),
            };
            DueAtField = new NumberFieldDefinition<BillingAgingListDetail, DateTime>(
                k => k.DueAt)
            {
                FieldName = "入金予定日",
                FieldNumber = 1,
                Required = false,
                Accept = VisitDueAtField,
                Format = value => value.ToShortDateString(),
            };
            SalesAtField = new NumberFieldDefinition<BillingAgingListDetail, DateTime>(
               k => k.SalesAt)
            {
                FieldName = "売上日",
                FieldNumber = 1,
                Required = false,
                Accept = VisitSalesAtField,
                Format = value => value.ToShortDateString(),
            };
            BillingAmountField = new NumberFieldDefinition<BillingAgingListDetail, decimal>(k => k.BillingAmount)
            {
                FieldName = "請求金額",
                FieldNumber = 1,
                Required = false,
                Accept = VisitBillingAmountField,
                Format = value => value.ToString(),
            };
            RemainAmountField = new NumberFieldDefinition<BillingAgingListDetail, decimal>(k => k.RemainAmount)
            {
                FieldName = "請求残",
                FieldNumber = 1,
                Required = false,
                Accept = VisitRemainAmountField,
                Format = value => value.ToString(),
            };
            StaffCodeField = new StringFieldDefinition<BillingAgingListDetail>(
                k => k.StaffCode)
            {
                FieldName = "担当者コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitStaffCodeField,
            };
            StaffNameField = new StringFieldDefinition<BillingAgingListDetail>(
                k => k.StaffName)
            {
                FieldName = "担当者名",
                FieldNumber = 1,
                Required = false,
                Accept = VisitStaffNameField,
            };
            InvoiceCodeField = new StringFieldDefinition<BillingAgingListDetail>(
                k => k.InvoiceCode)
            {
                FieldName = "請求書番号",
                FieldNumber = 1,
                Required = false,
                Accept = VisitInvoiceCodeField,
            };

            NoteField = new StringFieldDefinition<BillingAgingListDetail>(
                k => k.Note)
            {
                FieldName = ColumnNameSetting.DisplayColumnName,
                FieldNumber = 1,
                Required = false,
                Accept = VisitNoteField,
            };
            Fields.AddRange(new IFieldDefinition<BillingAgingListDetail>[] {
                       CustomerCodeField,CustomerNameField,CurrencyCodeField,BilledAtField,DueAtField
                       ,SalesAtField,BillingAmountField,RemainAmountField,StaffCodeField,StaffNameField
                       ,InvoiceCodeField,NoteField});
        }
        private bool VisitCustomerCodeField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }
        private bool VisitCustomerNameField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }
        private bool VisitCurrencyCodeField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }
        private bool VisitBilledAtField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardNumber(BilledAtField);
        }
        private bool VisitDueAtField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardNumber(DueAtField);
        }
        private bool VisitSalesAtField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardNumber(SalesAtField);
        }
        private bool VisitBillingAmountField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardNumber(BillingAmountField);
        }
        private bool VisitRemainAmountField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardNumber(RemainAmountField);
        }
        private bool VisitStaffCodeField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardString(StaffCodeField);
        }
        private bool VisitStaffNameField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardString(StaffNameField);
        }
        private bool VisitInvoiceCodeField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }
        private bool VisitNoteField(IFieldVisitor<BillingAgingListDetail> visitor)
        {
            return visitor.StandardString(NoteField);
        }
    }
}
