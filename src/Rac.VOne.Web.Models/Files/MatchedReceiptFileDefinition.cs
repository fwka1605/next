using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class MatchedReceiptFileDefinition : RowDefinition<MatchedReceipt>
    {
        public StringFieldDefinition<MatchedReceipt> CompanyCodeField { get; private set; }
        public NumberFieldDefinition<MatchedReceipt, long> SlipNumberField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> CustomerCodeField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> CustomerNameField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> InvoiceCodeField { get; private set; }
        public NumberFieldDefinition<MatchedReceipt, DateTime> BilledAtField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> ReceiptCategoryCodeField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> ReceiptCategoryNameField { get; private set; }
        public NumberFieldDefinition<MatchedReceipt, DateTime> RecordedAtField { get; private set; }
        public NullableNumberFieldDefinition<MatchedReceipt, DateTime> DueAtField { get; private set; }
        public NumberFieldDefinition<MatchedReceipt, decimal> AmountField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> DepartmentNameField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<MatchedReceipt, decimal> ReceiptAmountField { get; private set; }
        public NumberFieldDefinition<MatchedReceipt, long> IdField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillingNote1Field { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillingNote2Field { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillingNote3Field { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillingNote4Field { get; private set; }
        public StringFieldDefinition<MatchedReceipt> ReceiptNote1Field { get; private set; }
        public StringFieldDefinition<MatchedReceipt> ReceiptNote2Field { get; private set; }
        public StringFieldDefinition<MatchedReceipt> ReceiptNote3Field { get; private set; }
        public StringFieldDefinition<MatchedReceipt> ReceiptNote4Field { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillNumberField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillBankCodeField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillBranchCodeField { get; private set; }
        public NullableNumberFieldDefinition<MatchedReceipt, DateTime> BillDrawAtField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillDrawerField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BillingMemoField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> ReceiptMemoField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> MatchingMemoField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BankCodeField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BankNameField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BranchCodeField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> BranchNameField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> AccountNumberField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> SourceBankNameField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> SourceBranchNameField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> VirtualBranchCodeField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> VirtualAccountNumberField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> SectionCodeField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> SectionNameField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> ReceiptCategoryExternalCodeField { get; private set; }
        public NumberFieldDefinition<MatchedReceipt, long> OriginalReceiptIdField { get; private set; }
        public StringFieldDefinition<MatchedReceipt> JournalizingCategoryField { get; private set; }

        public MatchedReceiptFileDefinition(
            DataExpression expression,
            List<ExportFieldSetting> settings,
            List<ColumnNameSetting> columnNames) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "消込済み入金データ";
            FileNameToken = DataTypeToken;
            OutputHeader = settings.FirstOrDefault(x => x.ColumnName == "RequireHeader")?.AllowExport == 1;
            Fields.AddRange(GetFields(settings, columnNames));
        }

        private IEnumerable<IFieldDefinition<MatchedReceipt>> GetFields(
            IEnumerable<ExportFieldSetting> settings,
            List<ColumnNameSetting> columnNames)
        {
            var fieldNumber = 0;
            foreach (var setting in settings.Where(x => x.IsStandardField && x.AllowExport == 1))
            {
                fieldNumber++;
                if (setting.ColumnName == "CompanyCode")
                    yield return (CompanyCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.CompanyCode, setting.Caption, fieldNumber, accept: x => x.StandardString(CompanyCodeField)));
                if (setting.ColumnName == "SlipNumber")
                    yield return (SlipNumberField = new NumberFieldDefinition<MatchedReceipt, long>(
                        k => k.SlipNumber, setting.Caption, fieldNumber, accept: x => x.StandardNumber(SlipNumberField), formatter: x => x.ToString()));

                if (setting.ColumnName == "CustomerCode")
                    yield return (CustomerCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.CustomerCode, setting.Caption, fieldNumber, accept: x => x.StandardString(CustomerCodeField)));

                if (setting.ColumnName == "CustomerName")
                    yield return (CustomerNameField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.CustomerName, setting.Caption, fieldNumber, accept: x => x.StandardString(CustomerNameField)));

                if (setting.ColumnName == "InvoiceCode")
                    yield return (InvoiceCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.InvoiceCode, setting.Caption, fieldNumber, accept: x => x.StandardString(InvoiceCodeField)));

                if (setting.ColumnName == "BilledAt")
                    yield return (BilledAtField = new NumberFieldDefinition<MatchedReceipt, DateTime>(
                        k => k.BilledAt, setting.Caption, fieldNumber, accept: x => x.StandardNumber(BilledAtField), formatter: x => x.ToString(setting.DateFormat)));

                if (setting.ColumnName == "ReceiptCategoryCode")
                    yield return (ReceiptCategoryCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.ReceiptCategoryCode, setting.Caption, fieldNumber, accept: x => x.StandardString(ReceiptCategoryCodeField)));

                if (setting.ColumnName == "ReceiptCategoryName")
                    yield return (ReceiptCategoryNameField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.ReceiptCategoryName, setting.Caption, fieldNumber, accept: x => x.StandardString(ReceiptCategoryNameField)));

                if (setting.ColumnName == "RecordedAt")
                    yield return (RecordedAtField = new NumberFieldDefinition<MatchedReceipt, DateTime>(
                        k => k.RecordedAt, setting.Caption, fieldNumber, accept: x => x.StandardNumber(RecordedAtField), formatter: x => x.ToString(setting.DateFormat)));

                if (setting.ColumnName == "DueAt")
                    yield return (DueAtField = new NullableNumberFieldDefinition<MatchedReceipt, DateTime>(
                        k => k.DueAt, setting.Caption, fieldNumber, accept: x => x.StandardNumber(DueAtField), formatter: x => x.ToString(setting.DateFormat)));

                if (setting.ColumnName == "Amount")
                    yield return (AmountField = new NumberFieldDefinition<MatchedReceipt, decimal>(
                        k => k.Amount, setting.Caption, fieldNumber, accept: x => x.StandardNumber(AmountField), formatter: x => x.ToString()));

                if (setting.ColumnName == "DepartmentCode")
                    yield return (DepartmentCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.DepartmentCode, setting.Caption, fieldNumber, accept: x => x.StandardString(DepartmentCodeField)));

                if (setting.ColumnName == "DepartmentName")
                    yield return (DepartmentNameField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.DepartmentName, setting.Caption, fieldNumber, accept: x => x.StandardString(DepartmentNameField)));

                if (setting.ColumnName == "CurrencyCode")
                    yield return (CurrencyCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.CurrencyCode, setting.Caption, fieldNumber, accept: x => x.StandardString(CurrencyCodeField)));

                if (setting.ColumnName == "ReceiptAmount")
                    yield return (ReceiptAmountField = new NumberFieldDefinition<MatchedReceipt, decimal>(
                        k => k.ReceiptAmount, setting.Caption, fieldNumber, accept: x => x.StandardNumber(ReceiptAmountField), formatter: x => x.ToString()));

                if (setting.ColumnName == "ReceiptId")
                    yield return (IdField = new NumberFieldDefinition<MatchedReceipt, long>(
                        k => k.Id, setting.Caption, fieldNumber, accept: x => x.StandardNumber(IdField), formatter: x => x.ToString()));

                if (setting.ColumnName == "BillingNote1")
                    yield return (BillingNote1Field = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillingNote1, GetColumnNames(columnNames, nameof(Billing), "Note1", "備考"), fieldNumber, accept: x => x.StandardString(BillingNote1Field)));

                if (setting.ColumnName == "BillingNote2")
                    yield return (BillingNote2Field = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillingNote2, GetColumnNames(columnNames, nameof(Billing), "Note2", "備考2"), fieldNumber, accept: x => x.StandardString(BillingNote2Field)));

                if (setting.ColumnName == "BillingNote3")
                    yield return (BillingNote3Field = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillingNote3, GetColumnNames(columnNames, nameof(Billing), "Note3", "備考3"), fieldNumber, accept: x => x.StandardString(BillingNote3Field)));

                if (setting.ColumnName == "BillingNote4")
                    yield return (BillingNote4Field = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillingNote4, GetColumnNames(columnNames, nameof(Billing), "Note4", "備考4"), fieldNumber, accept: x => x.StandardString(BillingNote4Field)));

                if (setting.ColumnName == "ReceiptNote1")
                    yield return (ReceiptNote1Field = new StringFieldDefinition<MatchedReceipt>(
                        k => k.ReceiptNote1, GetColumnNames(columnNames, nameof(Receipt), "Note1", "備考"), fieldNumber, accept: x => x.StandardString(ReceiptNote1Field)));

                if (setting.ColumnName == "ReceiptNote2")
                    yield return (ReceiptNote2Field = new StringFieldDefinition<MatchedReceipt>(
                        k => k.ReceiptNote2, GetColumnNames(columnNames, nameof(Receipt), "Note2", "備考2"), fieldNumber, accept: x => x.StandardString(ReceiptNote2Field)));

                if (setting.ColumnName == "ReceiptNote3")
                    yield return (ReceiptNote3Field = new StringFieldDefinition<MatchedReceipt>(
                        k => k.ReceiptNote3, GetColumnNames(columnNames, nameof(Receipt), "Note3", "備考3"), fieldNumber, accept: x => x.StandardString(ReceiptNote3Field)));

                if (setting.ColumnName == "ReceiptNote4")
                    yield return (ReceiptNote4Field = new StringFieldDefinition<MatchedReceipt>(
                        k => k.ReceiptNote4, GetColumnNames(columnNames, nameof(Receipt), "Note4", "備考4"), fieldNumber, accept: x => x.StandardString(ReceiptNote4Field)));

                if (setting.ColumnName == "BillNumber")
                    yield return (BillNumberField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillNumber, setting.Caption, fieldNumber, accept: x => x.StandardString(BillNumberField)));

                if (setting.ColumnName == "BillBankCode")
                    yield return (BillBankCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillBankCode, setting.Caption, fieldNumber, accept: x => x.StandardString(BillBankCodeField)));

                if (setting.ColumnName == "BillBranchCode")
                    yield return (BillBranchCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillBranchCode, setting.Caption, fieldNumber, accept: x => x.StandardString(BillBranchCodeField)));

                if (setting.ColumnName == "BillDrawAt")
                    yield return (BillDrawAtField = new NullableNumberFieldDefinition<MatchedReceipt, DateTime>(
                        k => k.BillDrawAt, setting.Caption, fieldNumber, accept: x => x.StandardNumber(BillDrawAtField), formatter: x => x.ToString(setting.DateFormat)));

                if (setting.ColumnName == "BillDrawer")
                    yield return (BillDrawerField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillDrawer, setting.Caption, fieldNumber, accept: x => x.StandardString(BillDrawerField)));

                if (setting.ColumnName == "BillingMemo")
                    yield return (BillingMemoField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BillingMemo, setting.Caption, fieldNumber, accept: x => x.StandardString(BillingMemoField)));

                if (setting.ColumnName == "ReceiptMemo")
                    yield return (ReceiptMemoField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.ReceiptMemo, setting.Caption, fieldNumber, accept: x => x.StandardString(ReceiptMemoField)));

                if (setting.ColumnName == "MatchingMemo")
                    yield return (MatchingMemoField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.MatchingMemo, setting.Caption, fieldNumber, accept: x => x.StandardString(MatchingMemoField)));

                if (setting.ColumnName == "BankCode")
                    yield return (BankCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BankCode, setting.Caption, fieldNumber, accept: x => x.StandardString(BankCodeField)));

                if (setting.ColumnName == "BankName")
                    yield return (BankNameField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BankName, setting.Caption, fieldNumber, accept: x => x.StandardString(BankNameField)));

                if (setting.ColumnName == "BranchCode")
                    yield return (BranchCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BranchCode, setting.Caption, fieldNumber, accept: x => x.StandardString(BranchCodeField)));

                if (setting.ColumnName == "BranchName")
                    yield return (BranchNameField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.BranchName, setting.Caption, fieldNumber, accept: x => x.StandardString(BranchNameField)));

                if (setting.ColumnName == "AccountNumber")
                    yield return (AccountNumberField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.AccountNumber, setting.Caption, fieldNumber, accept: x => x.StandardString(AccountNumberField)));

                if (setting.ColumnName == "SourceBankName")
                    yield return (SourceBankNameField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.SourceBankName, setting.Caption, fieldNumber, accept: x => x.StandardString(SourceBankNameField)));

                if (setting.ColumnName == "SourceBranchName")
                    yield return (SourceBranchNameField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.SourceBranchName, setting.Caption, fieldNumber, accept: x => x.StandardString(SourceBranchNameField)));

                if (setting.ColumnName == "VirtualBranchCode")
                    yield return (VirtualBranchCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.VirtualBranchCode, setting.Caption, fieldNumber, accept: x => x.StandardString(VirtualBranchCodeField)));

                if (setting.ColumnName == "VirtualAccountNumber")
                    yield return (VirtualAccountNumberField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.VirtualAccountNumber, setting.Caption, fieldNumber, accept: x => x.StandardString(VirtualAccountNumberField)));

                if (setting.ColumnName == "SectionCode")
                    yield return (SectionCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.SectionCode, setting.Caption, fieldNumber, accept: x => x.StandardString(SectionCodeField)));

                if (setting.ColumnName == "SectionName")
                    yield return (SectionNameField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.SectionName, setting.Caption, fieldNumber, accept: x => x.StandardString(SectionNameField)));

                if (setting.ColumnName == "ReceiptCategoryExternalCode")
                    yield return (ReceiptCategoryExternalCodeField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.ReceiptCategoryExternalCode, setting.Caption, fieldNumber, accept: x => x.StandardString(ReceiptCategoryExternalCodeField)));

                if (setting.ColumnName == "OriginalReceiptId")
                    yield return (OriginalReceiptIdField = new NumberFieldDefinition<MatchedReceipt, long>(
                        k => k.OriginalReceiptId, setting.Caption, fieldNumber, accept: x => x.StandardNumber(OriginalReceiptIdField), formatter: x => x.ToString()));

                if (setting.ColumnName == "JournalizingCategory")
                    yield return (JournalizingCategoryField = new StringFieldDefinition<MatchedReceipt>(
                        k => k.JournalizingCategory, setting.Caption, fieldNumber, accept: x => x.StandardString(JournalizingCategoryField)));
            }
        }

        private string GetColumnNames(List<ColumnNameSetting> columnNames, string table, string column, string alias)
            => columnNames.FirstOrDefault(x => x.TableName == table && x.ColumnName == column)?.DisplayColumnName ?? alias;
    }
}
