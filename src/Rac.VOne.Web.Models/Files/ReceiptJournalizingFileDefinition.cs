using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class ReceiptJournalizingFileDefinition : RowDefinition<ReceiptJournalizing>
    {
        public StandardIdToCodeFieldDefinition<ReceiptJournalizing, Company> CompanyCodeField { get; private set; }
        public NumberFieldDefinition<ReceiptJournalizing, DateTime> RecordedAtField { get; private set; }
        public NumberFieldDefinition<ReceiptJournalizing, long> SlipNumberField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> DebitDepartmentCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> DebitDepartmentNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> DebitAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> DebitAccountTitleNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> DebitSubCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> DebitSubNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CreditDepartmentCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CreditDepartmentNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CreditAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CreditAccountTitleNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CreditSubCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CreditSubNameField { get; private set; }
        public NumberFieldDefinition<ReceiptJournalizing, decimal> AmountField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> NoteField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CustomerCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CustomerNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> StaffCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> PayerCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> PayerNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> SourceBankNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> SourceBranchNameField { get; private set; }
        public NullableNumberFieldDefinition<ReceiptJournalizing, DateTime> DueDateField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> BankCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> BankNameField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> BranchCodeField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> BranchNameField { get; private set; }
        public NumberFieldDefinition<ReceiptJournalizing, int> AccountTypeIdField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> AccountNumberField { get; private set; }
        public StringFieldDefinition<ReceiptJournalizing> CurrencyCodeField { get; private set; }

        public ReceiptJournalizingFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "消込仕訳";
            FileNameToken = DataTypeToken;
            OutputHeader = false;

            CompanyCodeField = new StandardIdToCodeFieldDefinition<ReceiptJournalizing, Company>(
            k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyCode,
            };
            RecordedAtField = new NumberFieldDefinition<ReceiptJournalizing, DateTime>(
            k => k.RecordedAt)
            {
                FieldName = "伝票日付",
                FieldNumber = 2,
                Required = true,
                Accept = VisitRecordedAtField,
                Format = value => (value == null) ? "" : value.ToShortDateString(),
            };
            SlipNumberField = new NumberFieldDefinition<ReceiptJournalizing, long>(
            k => k.SlipNumber)
            {
                FieldName = "伝票番号",
                FieldNumber = 3,
                Required = true,
                Accept = VisitSlipNumberField,
                Format = value => value.ToString(),
            };
            DebitDepartmentCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.DebitDepartmentCode)
            {
                FieldName = "借方部門コード",
                FieldNumber = 4,
                Required = true,
                Accept = VisitDebitDepartmentCodeField,
            };
            DebitDepartmentNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.DebitDepartmentName)
            {
                FieldName = "借方部門名",
                FieldNumber = 5,
                Required = true,
                Accept = VisitDebitDepartmentNameField,
            };
            DebitAccountTitleCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.DebitAccountTitleCode)
            {
                FieldName = "借方科目コード",
                FieldNumber = 6,
                Required = true,
                Accept = VisitDebitAccountTitleCodeField,
            };
            DebitAccountTitleNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.DebitAccountTitleName)
            {
                FieldName = "借方科目名",
                FieldNumber = 7,
                Required = true,
                Accept = VisitDebitAccountTitleNameField,
            };
            DebitSubCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.DebitSubCode)
            {
                FieldName = "借方補助コード",
                FieldNumber = 8,
                Required = true,
                Accept = VisitDebitSubCodeField,
            };
            DebitSubNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.DebitSubName)
            {
                FieldName = "借方補助名",
                FieldNumber = 9,
                Required = true,
                Accept = VisitDebitSubNameField,
            };
            CreditDepartmentCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CreditDepartmentCode)
            {
                FieldName = "貸方部門コード",
                FieldNumber = 10,
                Required = true,
                Accept = VisitCreditDepartmentCodeField,
            };
            CreditDepartmentNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CreditDepartmentName)
            {
                FieldName = "貸方部門名",
                FieldNumber = 11,
                Required = true,
                Accept = VisitCreditDepartmentNameField,
            };
            CreditAccountTitleCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CreditAccountTitleCode)
            {
                FieldName = "貸方科目コード",
                FieldNumber = 12,
                Required = true,
                Accept = VisitCreditAccountTitleCodeField,
            };
            CreditAccountTitleNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CreditAccountTitleName)
            {
                FieldName = "貸方科目名",
                FieldNumber = 13,
                Required = true,
                Accept = VisitCreditAccountTitleNameField,
            };
            CreditSubCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CreditSubCode)
            {
                FieldName = "貸方補助コード",
                FieldNumber = 14,
                Required = true,
                Accept = VisitCreditSubCodeField,
            };
            CreditSubNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CreditSubName)
            {
                FieldName = "貸方補助コード名",
                FieldNumber = 15,
                Required = true,
                Accept = VisitCreditSubNameField,
            };
            AmountField = new NumberFieldDefinition<ReceiptJournalizing, decimal>(
            k => k.Amount)
            {
                FieldName = "仕訳金額",
                FieldNumber = 16,
                Required = true,
                Accept = VisitAmountField,
                Format = value => value.ToString(),
            };
            NoteField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.Note)
            {
                FieldName = "備考",
                FieldNumber = 17,
                Required = true,
                Accept = VisitNoteField,
            };
            CustomerCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 18,
                Required = true,
                Accept = VisitCustomerCodeField,
            };
            CustomerNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 19,
                Required = true,
                Accept = VisitCustomerNameField,
            };
            InvoiceCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.InvoiceCode)
            {
                FieldName = "請求書番号",
                FieldNumber = 20,
                Required = true,
                Accept = VisitInvoiceCodeField,
            };
            StaffCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.StaffCode)
            {
                FieldName = "担当者コード",
                FieldNumber = 21,
                Required = true,
                Accept = VisitStaffCodeField,
            };
            PayerCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.PayerCode)
            {
                FieldName = "振込依頼人コード",
                FieldNumber = 22,
                Required = true,
                Accept = VisitPayerCodeField,
            };
            PayerNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.PayerName)
            {
                FieldName = "振込依頼人名",
                FieldNumber = 23,
                Required = true,
                Accept = VisitPayerNameField,
            };
            SourceBankNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.SourceBankName)
            {
                FieldName = "仕向銀行",
                FieldNumber = 24,
                Required = true,
                Accept = VisitSourceBankNameField,
            };
            SourceBranchNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.SourceBranchName)
            {
                FieldName = "仕向支店",
                FieldNumber = 25,
                Required = true,
                Accept = VisitSourceBranchNameField,
            };
            DueDateField = new NullableNumberFieldDefinition<ReceiptJournalizing, DateTime>(
            k => k.DueAt)
            {
                FieldName = "期日",
                FieldNumber = 26,
                Required = true,
                Accept = VisitDueDateField,
                Format = value => (value == null) ? "" : value.ToShortDateString(),
            };
            BankCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.BankCode)
            {
                FieldName = "銀行コード",
                FieldNumber = 27,
                Required = true,
                Accept = VisitBankCodeField,
            };
            BankNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.BankName)
            {
                FieldName = "銀行名",
                FieldNumber = 28,
                Required = true,
                Accept = VisitBankNameField,
            };
            BranchCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.BranchCode)
            {
                FieldName = "支店コード",
                FieldNumber = 29,
                Required = true,
                Accept = VisitBranchCodeField,
            };
            BranchNameField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.BranchName)
            {
                FieldName = "支店名",
                FieldNumber = 30,
                Required = true,
                Accept = VisitBranchNameField,
            };
            AccountTypeIdField = new NumberFieldDefinition<ReceiptJournalizing, int>(
            k => k.AccountTypeId)
            {
                FieldName = "預金種別",
                FieldNumber = 31,
                Required = true,
                Accept = VisitAccountTypeIdField,
                Format = value => (value == 0) ? "" : value.ToString(),
            };
            AccountNumberField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.AccountNumber)
            {
                FieldName = "口座番号",
                FieldNumber = 32,
                Required = true,
                Accept = VisitAccountNumberField,
            };
            CurrencyCodeField = new StringFieldDefinition<ReceiptJournalizing>(
            k => k.CurrencyCode)
            {
                FieldName = "通貨コード",
                FieldNumber = 34,
                Required = true,
                Accept = VisitCurrencyCodeField,
            };

            Fields.AddRange(new IFieldDefinition<ReceiptJournalizing>[]
            {
                CompanyCodeField, RecordedAtField, SlipNumberField, DebitDepartmentCodeField,
                DebitDepartmentNameField, DebitAccountTitleCodeField, DebitAccountTitleNameField,
                DebitSubCodeField, DebitSubNameField, CreditDepartmentCodeField,
                CreditDepartmentNameField, CreditAccountTitleCodeField, CreditAccountTitleNameField,
                CreditSubCodeField, CreditSubNameField, AmountField,
                NoteField, CustomerCodeField, CustomerNameField,
                InvoiceCodeField, StaffCodeField, PayerCodeField,
                PayerNameField, SourceBankNameField, SourceBranchNameField,
                DueDateField, BankCodeField, BankNameField,
                BranchCodeField, BranchNameField, AccountTypeIdField,
                AccountNumberField, CurrencyCodeField
            });
        }

        private bool VisitCompanyCode(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.OwnCompanyCode(CompanyCodeField);
        }

        private bool VisitRecordedAtField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardNumber(RecordedAtField);
        }

        private bool VisitSlipNumberField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardNumber(SlipNumberField);
        }

        private bool VisitDebitDepartmentCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(DebitDepartmentCodeField);
        }

        private bool VisitDebitDepartmentNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(DebitDepartmentNameField);
        }

        private bool VisitDebitAccountTitleCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(DebitAccountTitleCodeField);
        }

        private bool VisitDebitAccountTitleNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(DebitAccountTitleNameField);
        }

        private bool VisitDebitSubCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(DebitSubCodeField);
        }

        private bool VisitDebitSubNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(DebitSubNameField);
        }

        private bool VisitCreditDepartmentCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CreditDepartmentCodeField);
        }

        private bool VisitCreditDepartmentNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CreditDepartmentNameField);
        }

        private bool VisitCreditAccountTitleCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CreditAccountTitleCodeField);
        }

        private bool VisitCreditAccountTitleNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CreditAccountTitleNameField);
        }

        private bool VisitCreditSubCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CreditSubCodeField);
        }

        private bool VisitCreditSubNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CreditSubNameField);
        }

        private bool VisitAmountField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardNumber(AmountField);
        }

        private bool VisitNoteField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(NoteField);
        }

        private bool VisitCustomerCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }

        private bool VisitCustomerNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }

        private bool VisitStaffCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(StaffCodeField);
        }

        private bool VisitInvoiceCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }

        private bool VisitPayerCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(PayerCodeField);
        }

        private bool VisitPayerNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(PayerNameField);
        }

        private bool VisitSourceBankNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(SourceBankNameField);
        }

        private bool VisitSourceBranchNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(SourceBranchNameField);
        }

        private bool VisitDueDateField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardNumber(DueDateField);
        }

        private bool VisitBankCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(BankCodeField);
        }

        private bool VisitBankNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(BankNameField);
        }

        private bool VisitBranchCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(BranchCodeField);
        }

        private bool VisitBranchNameField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(BranchNameField);
        }

        private bool VisitAccountTypeIdField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardNumber(AccountTypeIdField);
        }

        private bool VisitAccountNumberField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(AccountNumberField);
        }

        private bool VisitCurrencyCodeField(IFieldVisitor<ReceiptJournalizing> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }
    }
}
