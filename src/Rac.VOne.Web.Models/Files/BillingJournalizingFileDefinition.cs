using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class BillingJournalizingFileDefinition : RowDefinition<BillingJournalizing>
    {
        public StandardIdToCodeFieldDefinition<BillingJournalizing, Company> CompanyCodeField { get; private set; }
        public NumberFieldDefinition<BillingJournalizing, DateTime> BilledAtField { get; private set; }
        public NumberFieldDefinition<BillingJournalizing, long> SlipNumberField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> DebitDepartmentCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> DebitDepartmentNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> DebitAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> DebitAccountTitleNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> DebitSubCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> DebitSubNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CreditDepartmentCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CreditDepartmentNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CreditAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CreditAccountTitleNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CreditSubCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CreditSubNameField { get; private set; }
        public NumberFieldDefinition<BillingJournalizing, decimal> BillingAmountField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> NoteField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CustomerCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CustomerNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> StaffCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> PayerCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> PayerNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> SourceBankNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> SourceBranchNameField { get; private set; }
        public NumberFieldDefinition<BillingJournalizing, DateTime> DueAtField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> BankCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> BankNameField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> BranchCodeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> BranchNameField { get; private set; }
        public NullableNumberFieldDefinition<BillingJournalizing, int> AccountTypeField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> AccountNumberField { get; private set; }
        public StringFieldDefinition<BillingJournalizing> CurrencyCodeField { get; private set; }

        public BillingJournalizingFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "請求仕訳出力";
            FileNameToken = DataTypeToken;
            OutputHeader = false;

            CompanyCodeField = new StandardIdToCodeFieldDefinition<BillingJournalizing, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyCode,
            };
            BilledAtField = new NumberFieldDefinition<BillingJournalizing, DateTime>(
            k => k.BilledAt)
            {
                FieldName = "伝票日付",
                FieldNumber = 2,
                Required = true,
                Accept = VisitBilledAt,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            SlipNumberField = new NumberFieldDefinition<BillingJournalizing, long>(
            k => k.SlipNumber)
            {
                FieldName = "伝票番号",
                FieldNumber = 3,
                Required = true,
                Accept = VisitSlipNumber,
                Format = value => value.ToString(),
            };
            DebitDepartmentCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.DebitDepartmentCode)
            {
                FieldName = "借方部門コード",
                FieldNumber = 4,
                Required = true,
                Accept = VisitDebitDepartmentCode,
            };
            DebitDepartmentNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.DebitDepartmentName)
            {
                FieldName = "借方部門名",
                FieldNumber = 5,
                Required = true,
                Accept = VisitDebitDepartmentCode,
            };
            DebitAccountTitleCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.DebitAccountTitleCode)
            {
                FieldName = "借方科目コード",
                FieldNumber = 6,
                Required = true,
                Accept = VisitDebitAccountTitleCode,
            };
            DebitAccountTitleNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.DebitAccountTitleName)
            {
                FieldName = "借方科目名",
                FieldNumber = 7,
                Required = true,
                Accept = VisitDebitAccountTitleName,
            };
            DebitSubCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.DebitSubCode)
            {
                FieldName = "借方補助コード",
                FieldNumber = 8,
                Required = true,
                Accept = VisitDebitSubCode,
            };
            DebitSubNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.DebitSubName)
            {
                FieldName = "借方補助名",
                FieldNumber = 9,
                Required = true,
                Accept = VisitDebitSubName,
            };
            CreditDepartmentCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CreditDepartmentCode)
            {
                FieldName = "貸方部門コード",
                FieldNumber = 10,
                Required = true,
                Accept = VisitCreditDepartmentCode,
            };
            CreditDepartmentNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CreditDepartmentName)
            {
                FieldName = "貸方部門名",
                FieldNumber = 11,
                Required = true,
                Accept = VisitCreditDepartmentName,
            };
            CreditAccountTitleCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CreditAccountTitleCode)
            {
                FieldName = "貸方科目コード",
                FieldNumber = 12,
                Required = true,
                Accept = VisitCreditAccountTitleCode,
            };
            CreditAccountTitleNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CreditAccountTitleName)
            {
                FieldName = "貸方科目名",
                FieldNumber = 13,
                Required = true,
                Accept = VisitCreditAccountTitleName,
            };
            CreditSubCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CreditSubCode)
            {
                FieldName = "貸方補助コード",
                FieldNumber = 14,
                Required = true,
                Accept = VisitCreditSubCode,
            };
            CreditSubNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CreditSubName)
            {
                FieldName = "貸方補助名",
                FieldNumber = 15,
                Required = true,
                Accept = VisitCreditSubName,
            };
            BillingAmountField = new NumberFieldDefinition<BillingJournalizing, decimal>(
            k => k.BillingAmount)
            {
                FieldName = "仕訳金額",
                FieldNumber = 16,
                Required = true,
                Accept = VisitBillingAmount,
                Format = value => value.ToString(),
            };
            NoteField = new StringFieldDefinition<BillingJournalizing>(
            k => k.Note)
            {
                FieldName = "備考",
                FieldNumber = 17,
                Required = true,
                Accept = VisitNote,
            };
            CustomerCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = 18,
                Required = true,
                Accept = VisitCustomerCode,
            };
            CustomerNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 19,
                Required = true,
                Accept = VisitCustomerName,
            };
            InvoiceCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.InvoiceCode)
            {
                FieldName = "請求書番号",
                FieldNumber = 20,
                Required = true,
                Accept = VisitInvoiceCode,
            };
            StaffCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.StaffCode)
            {
                FieldName = "担当者コード",
                FieldNumber = 21,
                Required = true,
                Accept = VisitStaffCode,
            };
            PayerCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.PayerCode)
            {
                FieldName = "振込依頼人コード",
                FieldNumber = 22,
                Required = true,
                Accept = VisitPayerCode,
            };
            PayerNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.PayerName)
            {
                FieldName = "振込依頼人名",
                FieldNumber = 23,
                Required = true,
                Accept = VisitPayerName,
            };
            SourceBankNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.SourceBankName)
            {
                FieldName = "仕向銀行",
                FieldNumber = 24,
                Required = true,
                Accept = VisitSourceBankName,
            };
            SourceBranchNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.SourceBranchName)
            {
                FieldName = "仕向支店",
                FieldNumber = 25,
                Required = true,
                Accept = VisitSourceBranchName,
            };
            DueAtField = new NumberFieldDefinition<BillingJournalizing, DateTime>(
            k => k.DueAt)
            {
                FieldName = "期日",
                FieldNumber = 26,
                Required = true,
                Accept = VisitDueAt,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToString(),
            };
            BankCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.BankCode)
            {
                FieldName = "銀行コード",
                FieldNumber = 27,
                Required = true,
                Accept = VisitBankCode,
            };
            BankNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.BankName)
            {
                FieldName = "銀行名",
                FieldNumber = 28,
                Required = true,
                Accept = VisitBankName,
            };
            BranchCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.BranchCode)
            {
                FieldName = "支店コード",
                FieldNumber = 29,
                Required = true,
                Accept = VisitBranchCode,
            };
            BranchNameField = new StringFieldDefinition<BillingJournalizing>(
            k => k.BranchName)
            {
                FieldName = "支店名",
                FieldNumber = 30,
                Required = true,
                Accept = VisitBranchName,
            };
            AccountTypeField = new NullableNumberFieldDefinition<BillingJournalizing, int>(
            k => k.AccountType)
            {
                FieldName = "預金種別",
                FieldNumber = 31,
                Required = true,
                Accept = VisitAccountType,
                Format = value => value.ToString(),
            };
            AccountNumberField = new StringFieldDefinition<BillingJournalizing>(
            k => k.AccountNumber)
            {
                FieldName = "口座番号",
                FieldNumber = 32,
                Required = true,
                Accept = VisitAccountNumber,
            };
            CurrencyCodeField = new StringFieldDefinition<BillingJournalizing>(
            k => k.CurrencyCode)
            {
                FieldName = "通貨コード",
                FieldNumber = 33,
                Required = true,
                Accept = VisitCurrencyCode,
            };

            Fields.AddRange(new IFieldDefinition<BillingJournalizing>[]
            {
                CompanyCodeField, BilledAtField, SlipNumberField, DebitDepartmentCodeField,
                DebitDepartmentNameField, DebitAccountTitleCodeField, DebitAccountTitleNameField,
                DebitSubCodeField, DebitSubNameField, CreditDepartmentCodeField,
                CreditDepartmentNameField, CreditAccountTitleCodeField, CreditAccountTitleNameField,
                CreditSubCodeField, CreditSubNameField, BillingAmountField,
                NoteField, CustomerCodeField, CustomerNameField,
                InvoiceCodeField, StaffCodeField, PayerCodeField,
                PayerNameField, SourceBankNameField, SourceBranchNameField,
                DueAtField, BankCodeField, BankNameField,
                BranchCodeField, BranchNameField, AccountTypeField,
                AccountNumberField, CurrencyCodeField
            });
        }

        private bool VisitCompanyCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.OwnCompanyCode(CompanyCodeField);
        }
        private bool VisitBilledAt(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardNumber<DateTime>(BilledAtField);
        }
        private bool VisitSlipNumber(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardNumber<long>(SlipNumberField);
        }
        private bool VisitDebitDepartmentCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(DebitDepartmentCodeField);
        }
        private bool VisitDebitDepartmentName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(DebitDepartmentNameField);
        }
        private bool VisitDebitAccountTitleCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(DebitAccountTitleCodeField);
        }
        private bool VisitDebitAccountTitleName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(DebitAccountTitleNameField);
        }
        private bool VisitDebitSubCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(DebitSubCodeField);
        }
        private bool VisitDebitSubName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(DebitSubNameField);
        }
        private bool VisitCreditDepartmentCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CreditDepartmentCodeField);
        }
        private bool VisitCreditDepartmentName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CreditDepartmentNameField);
        }
        private bool VisitCreditAccountTitleCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CreditAccountTitleCodeField);
        }
        private bool VisitCreditAccountTitleName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CreditAccountTitleNameField);
        }
        private bool VisitCreditSubCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CreditSubCodeField);
        }
        private bool VisitCreditSubName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CreditSubNameField);
        }
        private bool VisitBillingAmount(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardNumber<decimal>(BillingAmountField);
        }
        private bool VisitNote(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(NoteField);
        }
        private bool VisitCustomerCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }
        private bool VisitCustomerName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }
        private bool VisitInvoiceCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }
        private bool VisitStaffCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(StaffCodeField);
        }
        private bool VisitPayerCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(PayerCodeField);
        }
        private bool VisitPayerName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(PayerNameField);
        }
        private bool VisitSourceBankName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(SourceBankNameField);
        }
        private bool VisitSourceBranchName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(SourceBranchNameField);
        }
        private bool VisitDueAt(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardNumber<DateTime>(DueAtField);
        }
        private bool VisitBankCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(BankCodeField);
        }
        private bool VisitBankName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(BankNameField);
        }
        private bool VisitBranchCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(BranchCodeField);
        }
        private bool VisitBranchName(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(BranchNameField);
        }
        private bool VisitAccountType(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardNumber<int>(AccountTypeField);
        }
        private bool VisitAccountNumber(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(AccountNumberField);
        }
        private bool VisitCurrencyCode(IFieldVisitor<BillingJournalizing> visitor)
        {
            return visitor.StandardString(CurrencyCodeField);
        }
    }
}
