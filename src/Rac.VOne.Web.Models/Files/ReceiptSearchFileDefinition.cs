using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class ReceiptSearchFileDefinition : RowDefinition<Receipt>
    {
        public NumberFieldDefinition<Receipt, DateTime> DeleteAtField { get; private set; }

        public NumberFieldDefinition<Receipt, int> ExcludeFlagField { get; private set; }
        public StringFieldDefinition<Receipt> ExcludeCategoryNameField { get; private set; }
        public NumberFieldDefinition<Receipt, long> IdField { get; private set; }
        public StringFieldDefinition<Receipt> AssignmentFlagNameField { get; private set; }
        public NumberFieldDefinition<Receipt, DateTime> RecordedAtField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<Receipt, Customer> CustomerCodeField { get; private set; }
        public StringFieldDefinition<Receipt> CustomerNameField { get; private set; }
        public StringFieldDefinition<Receipt> PayerNameField { get; private set; }
        public StandardIdToCodeFieldDefinition<Receipt, Currency> CurrencyIdField { get; private set; }
        public NumberFieldDefinition<Receipt, decimal> ReceiptAmountField { get; private set; }
        public NumberFieldDefinition<Receipt, decimal> RemainAmountField { get; private set; }
        public NumberFieldDefinition<Receipt, decimal> ExcludeAmountField { get; private set; }
        public StringFieldDefinition<Receipt> ReceiptCategoryNameField { get; private set; }
        public StringFieldDefinition<Receipt> InputTypeField { get; private set; }
        public StringFieldDefinition<Receipt> Note1Field { get; private set; }
        public StringFieldDefinition<Receipt> Note2Field { get; private set; }
        public StringFieldDefinition<Receipt> Note3Field { get; private set; }
        public StringFieldDefinition<Receipt> Note4Field { get; private set; }
        public StringFieldDefinition<Receipt> ReceiptMemoField { get; private set; }
        public NumberFieldDefinition<Receipt, DateTime> DueAtField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<Receipt, Section> SectionIdField { get; private set; }
        public StringFieldDefinition<Receipt> SectionNameField { get; private set; }
        public StringFieldDefinition<Receipt> BankCodeField { get; private set; }
        public StringFieldDefinition<Receipt> BankNameField { get; private set; }
        public StringFieldDefinition<Receipt> BranchCodeField { get; private set; }
        public StringFieldDefinition<Receipt> BranchNameField { get; private set; }
        public StringFieldDefinition<Receipt> AccountNumberField { get; private set; }
        public StringFieldDefinition<Receipt> SourceBankNameField { get; private set; }
        public StringFieldDefinition<Receipt> SourceBranchNameField { get; private set; }
        public StringFieldDefinition<Receipt> PayerCodePrefixField { get; private set; }
        public StringFieldDefinition<Receipt> PayerCodeSuffixField { get; private set; }
        public NumberFieldDefinition<Receipt, DateTime> OutputAtField { get; private set; }
        public StringFieldDefinition<Receipt> BillNumberField { get; private set; }
        public StringFieldDefinition<Receipt> BillBankCodeField { get; private set; }
        public StringFieldDefinition<Receipt> BillBranchCodeField { get; private set; }
        public NumberFieldDefinition<Receipt, DateTime> BillDrawAtField { get; private set; }
        public StringFieldDefinition<Receipt> BillDrawerField { get; private set; }

        public ReceiptSearchFileDefinition(DataExpression expression, List<GridSetting> GridSettingInfo) 
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "入金データ";
            FileNameToken = DataTypeToken;

            int fieldNumber = 1;
            Fields.Add(DeleteAtField = new NullableNumberFieldDefinition<Receipt, DateTime>(k => k.DeleteAt,
                fieldName: "削除日", fieldNumber: fieldNumber,
                accept: VisitDeleteAtField,
                formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));

            foreach (GridSetting gs in GridSettingInfo)
            {
                fieldNumber++;
                IFieldDefinition<Receipt> field = null;
                switch (gs.ColumnName)
                {
                    case "ExcludeFlag":
                        field = (ExcludeFlagField = new NumberFieldDefinition<Receipt, int>(k => k.ExcludeFlag,
                            accept: VisitExcludeFlagField,
                            formatter: value => value.ToString()));
                        break;
                    case "ExcludeCategory":
                        field = (ExcludeCategoryNameField = new StringFieldDefinition<Receipt>(k => k.ExcludeCategoryName,
                            accept: VisitExcludeCategoryNameField));
                        break;
                    case "Id":
                        field = (IdField = new NumberFieldDefinition<Receipt, long>(k => k.Id,
                            accept: VisitIdField,
                            formatter: value => value.ToString()));
                        break;
                    case "AssignmentState":
                        field = (AssignmentFlagNameField = new StringFieldDefinition<Receipt>(k => k.AssignmentFlagName,
                            accept: VisitAssignmentFlagNameField));
                        break;
                    case "RecordedAt":
                        field = (RecordedAtField = new NumberFieldDefinition<Receipt, DateTime>(k => k.RecordedAt,
                            accept: VisitRecordedAtField,
                            formatter: value => (value == null) ? "" : value.ToShortDateString()));
                        break;
                    case "CustomerCode":
                        field = (CustomerCodeField = new StandardNullableIdToCodeFieldDefinition<Receipt, Customer>(
                            k => k.CustomerId, c => c.Id, k => k.CustomerCode, c => c.Code,
                            accept: VisitCustomerCodeField));
                        break;
                    case "CustomerName":
                        field = (CustomerNameField = new StringFieldDefinition<Receipt>(k => k.CustomerName,
                            accept: VisitCustomerNameField));
                        break;
                    case "PayerName":
                        field = (PayerNameField = new StringFieldDefinition<Receipt>(k => k.PayerName,
                            accept: VisitPayerNameField));
                        break;
                    case "CurrencyCode":
                        field = (CurrencyIdField = new StandardIdToCodeFieldDefinition<Receipt, Currency>(
                            k => k.CurrencyId, c => c.Id,
                            k => k.CurrencyCode, c => c.Code,
                            accept: VisitCurrencyIdField));
                        break;
                    case "ReceiptAmount":
                        field = (ReceiptAmountField = new NumberFieldDefinition<Receipt, decimal>(k => k.ReceiptAmount,
                            accept: VisitReceiptAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case "RemainAmount":
                        field = (RemainAmountField = new NumberFieldDefinition<Receipt, decimal>(k => k.RemainAmount,
                            accept: VisitRemainAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case "ExcludeAmount":
                        field = (ExcludeAmountField = new NumberFieldDefinition<Receipt, decimal>(k => k.ExcludeAmount,
                            accept: VisitExcludeAmountField,
                            formatter: value => value.ToString()));
                        break;
                    case "ReceiptCategoryName":
                        field = (ReceiptCategoryNameField = new StringFieldDefinition<Receipt>(k => k.CategoryCodeName,
                            accept: VisitReceiptCategoryNameField));
                        break;
                    case "InputType":
                        field = (InputTypeField = new StringFieldDefinition<Receipt>(k => k.InputTypeName,
                            accept: VisitInputTypeField));
                        break;
                    case "Note1":
                        field = (Note1Field = new StringFieldDefinition<Receipt>(k => k.Note1,
                            accept: VisitNote1Field));
                        break;
                    case "Note2":
                        field = (Note2Field = new StringFieldDefinition<Receipt>(k => k.Note2,
                            accept: VisitNote2Field));
                        break;
                    case "Note3":
                        field = (Note3Field = new StringFieldDefinition<Receipt>(k => k.Note3,
                            accept: VisitNote3Field));
                        break;
                    case "Note4":
                        field = (Note4Field = new StringFieldDefinition<Receipt>(k => k.Note4,
                            accept: VisitNote4Field));
                        break;
                    case "Memo":
                        field = (ReceiptMemoField = new StringFieldDefinition<Receipt>(k => k.ReceiptMemo,
                            accept: VisitReceiptMemoField));
                        break;
                    case "DueAt":
                        field = (DueAtField = new NullableNumberFieldDefinition<Receipt, DateTime>(k => k.DueAt,
                            accept: VisitDueAtField,
                            formatter: value => (value == null) ? "" : value.ToShortDateString()));
                        break;
                    case "SectionCode":
                        field = (SectionIdField = new StandardNullableIdToCodeFieldDefinition<Receipt, Section>(
                            k => k.SectionId, c => c.Id,
                            k => k.SectionCode, c => c.Code,
                            accept: VisitSectionIdField));
                        break;
                    case "SectionName":
                        field = (SectionNameField = new StringFieldDefinition<Receipt>(k => k.SectionName,
                            accept: VisitSectionNameField));
                        break;
                    case "BankCode":
                        field = (BankCodeField = new StringFieldDefinition<Receipt>(k => k.BankCode,
                            accept: VisitBankCodeField));
                        break;
                    case "BankName":
                        field = (BankNameField = new StringFieldDefinition<Receipt>(k => k.BankName,
                            accept: VisitBankNameField));
                        break;
                    case "BranchCode":
                        field = (BranchCodeField = new StringFieldDefinition<Receipt>(k => k.BranchCode,
                            accept: VisitBranchCodeField));
                        break;
                    case "BranchName":
                        field = (BranchNameField = new StringFieldDefinition<Receipt>(k => k.BranchName,
                            accept: VisitBranchNameField));
                        break;
                    case "AccountNumber":
                        field = (AccountNumberField = new StringFieldDefinition<Receipt>(k => k.AccountNumber,
                            accept: VisitAccountNumberField));
                        break;
                    case "SourceBankName":
                        field = (SourceBankNameField = new StringFieldDefinition<Receipt>(k => k.SourceBankName,
                            accept: VisitSourceBankNameField));
                        break;
                    case "SourceBranchName":
                        field = (SourceBranchNameField = new StringFieldDefinition<Receipt>(k => k.SourceBranchName,
                            accept: VisitSourceBranchNameField));
                        break;
                    case "VirtualBranchCode":
                        field = (PayerCodePrefixField = new StringFieldDefinition<Receipt>(k => k.PayerCodePrefix,
                            accept: VisitPayerCodePrefixField));
                        break;
                    case "VirtualAccountNumber":
                        field = (PayerCodeSuffixField = new StringFieldDefinition<Receipt>(k => k.PayerCodeSuffix,
                            accept: VisitPayerCodeSuffixField));
                        break;
                    case "OutputAt":
                        field = (OutputAtField = new NullableNumberFieldDefinition<Receipt, DateTime>(k => k.OutputAt,
                            accept: VisitOutputAtField,
                            formatter: value => (value == null) ? "" : value.ToString()));
                        break;
                    case "BillNumber":
                        field = (BillNumberField = new StringFieldDefinition<Receipt>(k => k.BillNumber,
                            accept: VisitBillNumberField));
                        break;
                    case "BillBankCode":
                        field = (BillBankCodeField = new StringFieldDefinition<Receipt>(k => k.BillBankCode,
                            accept: VisitBillBankCodeField));
                        break;
                    case "BillBranchCode":
                        field = (BillBranchCodeField = new StringFieldDefinition<Receipt>(k => k.BillBranchCode,
                            accept: VisitBillBranchCodeField));
                        break;
                    case "BillDrawAt":
                        field = (BillDrawAtField = new NullableNumberFieldDefinition<Receipt, DateTime>(k => k.BillDrawAt,
                            accept: VisitBillDrawAtField,
                            formatter: value => (value == null) ? "" : value.ToShortDateString()));
                        break;
                    case "BillDrawer":
                        field = (BillDrawerField = new StringFieldDefinition<Receipt>(k => k.BillDrawer,
                            accept: VisitBillDrawerField));
                        break;
                }
                if (field == null) continue;
                field.FieldName = gs.ColumnNameJp;
                field.FieldNumber = fieldNumber;
                Fields.Add(field);
            }

            KeyFields.AddRange(new IFieldDefinition<Receipt>[]
            {
                CurrencyIdField,
                CustomerCodeField,
                SectionIdField,
            });
        }

        public IFieldDefinition<Receipt> ConvertSettingToField(string column)
        {
            IFieldDefinition<Receipt> field = null;
            if (column == "ExcludeFlag")            field = ExcludeFlagField;
            if (column == "ExcludeCategory")        field = ExcludeCategoryNameField ;
            if (column == "Id")                     field = IdField;
            if (column == "AssignmentState")        field = AssignmentFlagNameField;
            if (column == "RecordedAt")             field = RecordedAtField;
            if (column == "CustomerCode")           field = CustomerCodeField;
            if (column == "CustomerName")           field = CustomerNameField;
            if (column == "PayerName")              field = PayerNameField;
            if (column == "CurrencyCode")           field = CurrencyIdField;
            if (column == "ReceiptAmount")          field = ReceiptAmountField;
            if (column == "RemainAmount")           field = RemainAmountField;
            if (column == "ExcludeAmount")          field = ExcludeAmountField;
            if (column == "ReceiptCategoryName")    field = ReceiptCategoryNameField;
            if (column == "InputType")              field = InputTypeField;
            if (column == "Note1")                  field = Note1Field;
            if (column == "Note2")                  field = Note2Field;
            if (column == "Note3")                  field = Note3Field;
            if (column == "Note4")                  field = Note4Field;
            if (column == "Memo")                   field = ReceiptMemoField;
            if (column == "DueAt")                  field = DueAtField;
            if (column == "SectionCode")            field = SectionIdField;
            if (column == "SectionName")            field = SectionNameField;
            if (column == "BankCode")               field = BankCodeField;
            if (column == "BankName")               field = BankNameField ;
            if (column == "BranchCode")             field = BranchCodeField;
            if (column == "BranchName")             field = BranchNameField;
            if (column == "AccountNumber")          field = AccountNumberField;
            if (column == "SourceBankName")         field = SourceBankNameField;
            if (column == "SourceBranchName")       field = SourceBranchNameField;
            if (column == "VirtualBranchCode")      field = PayerCodePrefixField;
            if (column == "VirtualAccountNumber")   field = PayerCodeSuffixField;
            if (column == "OutputAt")               field = OutputAtField ;
            if (column == "BillNumber")             field = BillNumberField;
            if (column == "BillBankCode")           field = BillBankCodeField;
            if (column == "BillBranchCode")         field = BillBranchCodeField;
            if (column == "BillDrawAt")             field = BillDrawAtField;
            if (column == "BillDrawer")             field = BillDrawerField;
            return field;
        }

        private bool VisitDeleteAtField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(DeleteAtField);
        }

        private bool VisitExcludeFlagField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(ExcludeFlagField);
        }

        private bool VisitExcludeCategoryNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(ExcludeCategoryNameField);
        }

        private bool VisitIdField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(IdField);
        }

        private bool VisitAssignmentFlagNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(AssignmentFlagNameField);
        }

        private bool VisitRecordedAtField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(RecordedAtField);
        }

        private bool VisitCustomerCodeField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.CustomerCode(CustomerCodeField);
        }

        private bool VisitCustomerNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }

        private bool VisitPayerNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(PayerNameField);
        }

        private bool VisitCurrencyIdField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.CurrencyCode(CurrencyIdField);
        }

        private bool VisitReceiptAmountField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(ReceiptAmountField);
        }

        private bool VisitRemainAmountField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(RemainAmountField);
        }

        private bool VisitExcludeAmountField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(ExcludeAmountField);
        }

        private bool VisitReceiptCategoryNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(ReceiptCategoryNameField);
        }

        private bool VisitInputTypeField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(InputTypeField);
        }

        private bool VisitNote1Field(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(Note1Field);
        }

        private bool VisitNote2Field(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(Note2Field);
        }

        private bool VisitNote3Field(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(Note3Field);
        }

        private bool VisitNote4Field(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(Note4Field);
        }

        private bool VisitReceiptMemoField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(ReceiptMemoField);
        }

        private bool VisitDueAtField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(DueAtField);
        }

        private bool VisitSectionIdField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.SectionCode(SectionIdField);
        }

        private bool VisitSectionNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(SectionNameField);
        }

        private bool VisitBankCodeField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(BankCodeField);
        }

        private bool VisitBankNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(BankNameField);
        }

        private bool VisitBranchCodeField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(BranchCodeField);
        }

        private bool VisitBranchNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(BranchNameField);
        }

        private bool VisitAccountNumberField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(AccountNumberField);
        }

        private bool VisitSourceBankNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(SourceBankNameField);
        }

        private bool VisitSourceBranchNameField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(SourceBranchNameField);
        }

        private bool VisitPayerCodePrefixField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(PayerCodePrefixField);
        }

        private bool VisitPayerCodeSuffixField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(PayerCodeSuffixField);
        }

        private bool VisitOutputAtField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(OutputAtField);
        }

        private bool VisitBillNumberField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(BillNumberField);
        }

        private bool VisitBillBankCodeField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(BillBankCodeField);
        }

        private bool VisitBillBranchCodeField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(BillBranchCodeField);
        }

        private bool VisitBillDrawAtField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardNumber(BillDrawAtField);
        }

        private bool VisitBillDrawerField(IFieldVisitor<Receipt> visitor)
        {
            return visitor.StandardString(BillDrawerField);
        }

    }
}
