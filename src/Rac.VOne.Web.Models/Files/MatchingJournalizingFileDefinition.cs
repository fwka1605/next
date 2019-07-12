using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class MatchingJournalizingFileDefinition : RowDefinition<MatchingJournalizing>
    {
        public StandardIdToCodeFieldDefinition<MatchingJournalizing, Company> CompanyCodeField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, DateTime> RecordedAtField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, long> SlipNumberField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitDepartmentCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitDepartmentNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitAccountTitleNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitSubCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> DebitSubNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditDepartmentCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditDepartmentNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditAccountTitleCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditAccountTitleNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditSubCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CreditSubNameField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, decimal> AmountField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> NoteField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CustomerCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CustomerNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> StaffCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> PayerCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> PayerNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> SourceBankNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> SourceBranchNameField { get; private set; }
        public NumberFieldDefinition<MatchingJournalizing, DateTime> DueDateField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> BankCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> BankNameField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> BranchCodeField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> BranchNameField { get; private set; }
        public NullableNumberFieldDefinition<MatchingJournalizing, int> AccountTypeIdField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> AccountNumberField { get; private set; }
        public NullableNumberFieldDefinition<MatchingJournalizing, int> TaxClassIdField { get; private set; }
        public StringFieldDefinition<MatchingJournalizing> CurrencyCodeField { get; private set; }

        public MatchingJournalizingFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "消込仕訳";
            FileNameToken = DataTypeToken;
            OutputHeader = false;
            Fields.AddRange(InitializeFields());
        }

        private IEnumerable<IFieldDefinition<MatchingJournalizing>> InitializeFields()
        {
            yield return (CompanyCodeField = new StandardIdToCodeFieldDefinition<MatchingJournalizing, Company>(
                k => k.CompanyId, c => c.Id, null, c => c.Code,
                "会社コード", 1, accept: x => x.OwnCompanyCode(CompanyCodeField)));

            yield return (RecordedAtField = new NumberFieldDefinition<MatchingJournalizing, DateTime>(k => k.RecordedAt,
                "伝票日付", 2, accept: x => x.StandardNumber(RecordedAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));

            yield return (SlipNumberField = new NumberFieldDefinition<MatchingJournalizing, long>(k => k.SlipNumber,
                "伝票番号", 3, accept: x => x.StandardNumber(SlipNumberField), formatter: value => value.ToString()));

            yield return (DebitDepartmentCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitDepartmentCode,
                "借方部門コード", 4, accept: x => x.StandardString(DebitDepartmentCodeField)));

            yield return (DebitDepartmentNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitDepartmentName,
                "借方部門名", 5, accept: x => x.StandardString(DebitDepartmentNameField)));

            yield return (DebitAccountTitleCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitAccountTitleCode,
                "借方科目コード", 6, accept: x => x.StandardString(DebitAccountTitleCodeField)));

            yield return (DebitAccountTitleNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitAccountTitleName,
                "借方科目名", 7, accept: x => x.StandardString(DebitAccountTitleNameField)));

            yield return (DebitSubCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitSubCode,
                "借方補助コード", 8, accept: x => x.StandardString(DebitSubCodeField)));

            yield return (DebitSubNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.DebitSubName,
                "借方補助名", 9, accept: x => x.StandardString(DebitSubNameField)));

            yield return (CreditDepartmentCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditDepartmentCode,
                "貸方部門コード", 10, accept: x => x.StandardString(CreditDepartmentCodeField)));

            yield return (CreditDepartmentNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditDepartmentName,
                "貸方部門名", 11, accept: x => x.StandardString(CreditDepartmentNameField)));

            yield return (CreditAccountTitleCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditAccountTitleCode,
                "貸方科目コード", 12, accept: x => x.StandardString(CreditAccountTitleCodeField)));

            yield return (CreditAccountTitleNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditAccountTitleName,
                "貸方科目名", 13, accept: x => x.StandardString(CreditAccountTitleNameField)));

            yield return (CreditSubCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditSubCode,
                "貸方補助コード", 14, accept: x => x.StandardString(CreditSubCodeField)));

            yield return (CreditSubNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.CreditSubName,
                "貸方補助名", 15, accept: x => x.StandardString(CreditSubNameField)));

            yield return (AmountField = new NumberFieldDefinition<MatchingJournalizing, decimal>(k => k.Amount,
                "金額", 16, accept: x => x.StandardNumber(AmountField), formatter: value => value.ToString()));

            yield return (NoteField = new StringFieldDefinition<MatchingJournalizing>(k => k.Note,
                "備考", 17, accept: x => x.StandardString(NoteField)));

            yield return (CustomerCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.CustomerCode,
                "得意先コード", 18, accept: x => x.StandardString(CustomerCodeField)));

            yield return (CustomerNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.CustomerName,
                "得意先名", 19, accept: x => x.StandardString(CustomerNameField)));

            yield return (InvoiceCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.InvoiceCode,
                "請求書番号", 20, accept: x => x.StandardString(InvoiceCodeField)));

            yield return (StaffCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.StaffCode,
                "担当者コード", 21, accept: x => x.StandardString(StaffCodeField)));

            yield return (PayerCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.PayerCode,
                "振込依頼人コード", 22, accept: x => x.StandardString(PayerCodeField)));

            yield return (PayerNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.PayerName,
                "振込依頼人名", 23, accept: x => x.StandardString(PayerNameField)));

            yield return (SourceBankNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.SourceBankName,
                "仕向銀行", 24, accept: x => x.StandardString(SourceBankNameField)));

            yield return (SourceBranchNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.SourceBranchName,
                "仕向支店", 25, accept: x => x.StandardString(SourceBranchNameField)));

            yield return (DueDateField = new NumberFieldDefinition<MatchingJournalizing, DateTime>(k => k.DueDate,
                "期日", 26, accept: x => x.StandardNumber(DueDateField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));

            yield return (BankCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.BankCode,
                "銀行コード", 27, accept: x => x.StandardString(BankCodeField)));

            yield return (BankNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.BankName,
                "銀行名", 28, accept: x => x.StandardString(BankNameField)));

            yield return (BranchCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.BranchCode,
                "支店コード", 29, accept: x => x.StandardString(BranchCodeField)));

            yield return (BranchNameField = new StringFieldDefinition<MatchingJournalizing>(k => k.BranchName,
                "支店名", 30, accept: x => x.StandardString(BranchNameField)));

            yield return (AccountTypeIdField = new NullableNumberFieldDefinition<MatchingJournalizing, int>(k => k.AccountTypeId,
                "預金種別", 31, accept: x => x.StandardNumber(AccountTypeIdField), formatter: value => value.ToString()));

            yield return (AccountNumberField = new StringFieldDefinition<MatchingJournalizing>(k => k.AccountNumber,
                "口座番号", 32, accept: x => x.StandardString(AccountNumberField)));

            yield return (TaxClassIdField = new NullableNumberFieldDefinition<MatchingJournalizing, int>(k => k.TaxClassId,
                "税区分コード", 33, accept: x => x.StandardNumber(TaxClassIdField), formatter: value => value.ToString()));

            yield return (CurrencyCodeField = new StringFieldDefinition<MatchingJournalizing>(k => k.CurrencyCode,
                "通貨コード", 34, accept: x => x.StandardString(CurrencyCodeField)));

        }

    }
}
