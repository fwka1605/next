using Rac.VOne.Common.DataHandling;
using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Models.Files
{
    public class MatchingIndividualFileDefinition : RowDefinition<ExportMatchingIndividual>
    {
        //請求データ
        public StringFieldDefinition<ExportMatchingIndividual> CurrencyCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillCheckField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> CustomerCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> CustomerNameField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime> BilledAtField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime> SalesAtField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime> DueAtField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, decimal> BillingAmountField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, decimal> RemainAmountField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, decimal> DiscountAmountField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, decimal> TargetAmountField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingCategoryField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingDepartmentField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingNote1Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingNote2Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingNote3Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingNote4Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingNote5Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingNote6Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingNote7Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingNote8Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingMemoField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillingInputTypeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ScheduledPaymentKeyField { get; private set; }

        //入金データ
        public StringFieldDefinition<ExportMatchingIndividual> RecCheckField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptPayerNameField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime> ReceiptRecordAtField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptCategoryField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, decimal> ReceiptAmountField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, decimal> ReceiptRemainAmountField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> NettingStateField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> SourceBankInfoField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BankCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BankNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BranchCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BranchNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> AccountTypeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> AccountNumberField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> SectionCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> SectionNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptMemoField { get; private set; }
        public NumberFieldDefinition<ExportMatchingIndividual, DateTime> ReceiptDueDateField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptExcludeCategoryField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> PayerCodePrefixField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> PayerCodeSuffixField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptCustomerCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptCustomerNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptNote1Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptNote2Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptNote3Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptNote4Field { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillNumberField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillBankCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillBranchCodeField { get; private set; }
        public NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime> BillDrawAtField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> BillDrawerField { get; private set; }
        public StringFieldDefinition<ExportMatchingIndividual> ReceiptPayerNameRawField { get; private set; }

        public MatchingIndividualFileDefinition(ApplicationControl app) :this(new DataExpression(app))
        {
            if (CurrencyCodeField.Ignored = (app.UseForeignCurrency == 0))
            {
                CurrencyCodeField.FieldName = null;
            }
            if (ScheduledPaymentKeyField.Ignored = (app.UseScheduledPayment == 0))
            {
                ScheduledPaymentKeyField.FieldName = null;
            }
            if (SectionCodeField.Ignored = (app.UseReceiptSection == 0))
            {
                SectionCodeField.FieldName = null;
                SectionNameField.Ignored = true;
                SectionNameField.FieldName = null;
            }
        }


        //ReceiptExportFields
        public MatchingIndividualFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "個別消込";

            Fields.AddRange(GetFields());
        }

        private IEnumerable<IFieldDefinition<ExportMatchingIndividual>> GetFields()
        {
            var fieldNumber = 0;
            yield return (CurrencyCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.CurrencyCode,
                fieldNumber: ++fieldNumber, fieldName: "通貨コード",
                accept: visitor => visitor.StandardString(CurrencyCodeField)));

            yield return (BillCheckField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BillCheck,
                fieldNumber: ++fieldNumber, fieldName: "消",
                accept: visitor => visitor.StandardString(BillCheckField)));

            yield return (CustomerCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.CustomerCode,
                fieldNumber: ++fieldNumber, fieldName: "得意先コード",
                accept: visitor => visitor.StandardString(CustomerCodeField)));

            yield return (CustomerNameField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.CustomerName,
                fieldNumber: ++fieldNumber, fieldName: "得意先名",
                accept: visitor => visitor.StandardString(CustomerNameField)));

            yield return (BilledAtField = new NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime>(k => k.BilledAt,
                fieldNumber: ++fieldNumber, fieldName: "請求日",
                accept: visitor => visitor.StandardNumber(BilledAtField),
                formatter: value => value.ToShortDateString()));

            yield return (SalesAtField = new NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime>(k => k.SalesAt,
                fieldNumber: ++fieldNumber, fieldName: "売上日",
                accept: visitor => visitor.StandardNumber(SalesAtField),
                formatter: value => value.ToShortDateString()));

            yield return (DueAtField = new NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime>(k => k.DueAt,
                fieldNumber: ++fieldNumber, fieldName: "予定日",
                accept: visitor => visitor.StandardNumber(DueAtField),
                formatter: value => value.ToShortDateString()));

            yield return (BillingAmountField = new NullableNumberFieldDefinition<ExportMatchingIndividual, decimal>(k => k.BillingAmount,
                fieldNumber: ++fieldNumber, fieldName: "請求額",
                accept: visitor => visitor.StandardNumber(BillingAmountField),
                formatter: value => value.ToString()));

            yield return (RemainAmountField = new NullableNumberFieldDefinition<ExportMatchingIndividual, decimal>(k => k.RemainAmount,
                fieldNumber: ++fieldNumber, fieldName: "請求残",
                accept: visitor => visitor.StandardNumber(RemainAmountField),
                formatter: value => value.ToString()));

            yield return (DiscountAmountField = new NullableNumberFieldDefinition<ExportMatchingIndividual, decimal>(k => k.DiscountAmount,
                fieldNumber: ++fieldNumber, fieldName: "歩引額",
                accept: visitor => visitor.StandardNumber(DiscountAmountField),
                formatter: value => value.ToString()));

            yield return (TargetAmountField = new NullableNumberFieldDefinition<ExportMatchingIndividual, decimal>(k => k.TargetAmount,
                fieldNumber: ++fieldNumber, fieldName: "消込対象額",
                accept: visitor => visitor.StandardNumber(TargetAmountField),
                formatter: value => value.ToString()));

            yield return (InvoiceCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.InvoiceCode,
                fieldNumber: ++fieldNumber, fieldName: "請求番号",
                accept: visitor => visitor.StandardString(InvoiceCodeField)));

            yield return (BillingCategoryField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BillingCategoryCodeAndName,
                fieldNumber: ++fieldNumber, fieldName: "請求区分",
                accept: visitor => visitor.StandardString(BillingCategoryField)));

            yield return (BillingDepartmentField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.DepartmentName,
                fieldNumber: ++fieldNumber, fieldName: "請求部門名",
                accept: visitor => visitor.StandardString(BillingDepartmentField)));

            yield return (BillingNote1Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Note1,
                fieldNumber: ++fieldNumber, fieldName: "備考",
                accept: visitor => visitor.StandardString(BillingNote1Field)));

            yield return (BillingNote2Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Note2,
                fieldNumber: ++fieldNumber, fieldName: "備考2",
                accept: visitor => visitor.StandardString(BillingNote2Field)));

            yield return (BillingNote3Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Note3,
                fieldNumber: ++fieldNumber, fieldName: "備考3",
                accept: visitor => visitor.StandardString(BillingNote3Field)));

            yield return (BillingNote4Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Note4,
                fieldNumber: ++fieldNumber, fieldName: "備考4",
                accept: visitor => visitor.StandardString(BillingNote4Field)));

            yield return (BillingNote5Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Note5,
                fieldNumber: ++fieldNumber, fieldName: "備考5",
                accept: visitor => visitor.StandardString(BillingNote5Field)));

            yield return (BillingNote6Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Note6,
                fieldNumber: ++fieldNumber, fieldName: "備考6",
                accept: visitor => visitor.StandardString(BillingNote6Field)));

            yield return (BillingNote7Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Note7,
                fieldNumber: ++fieldNumber, fieldName: "備考7",
                accept: visitor => visitor.StandardString(BillingNote7Field)));

            yield return (BillingNote8Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Note8,
                fieldNumber: ++fieldNumber, fieldName: "備考8",
                accept: visitor => visitor.StandardString(BillingNote8Field)));

            yield return (BillingMemoField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.Memo,
                fieldNumber: ++fieldNumber, fieldName: "請求メモ",
                accept: visitor => visitor.StandardString(BillingMemoField)));

            yield return (BillingInputTypeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.InputTypeName,
                fieldNumber: ++fieldNumber, fieldName: "データ区分",
                accept: visitor => visitor.StandardString(BillingInputTypeField)));

            yield return (ScheduledPaymentKeyField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ScheduledPaymentKey,
                fieldNumber: ++fieldNumber, fieldName: "入金予定キー",
                accept: visitor => visitor.StandardString(ScheduledPaymentKeyField)));

            yield return (RecCheckField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.RecCheck,
                fieldNumber: ++fieldNumber, fieldName: "消",
                accept: visitor => visitor.StandardString(RecCheckField)));

            yield return (ReceiptPayerNameField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.PayerName,
                fieldNumber: ++fieldNumber, fieldName: "振込依頼人名",
                accept: visitor => visitor.StandardString(ReceiptPayerNameField)));

            yield return (ReceiptRecordAtField = new NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime>(k => k.RecordedAt,
                fieldNumber: ++fieldNumber, fieldName: "入金日",
                accept: visitor => visitor.StandardNumber(ReceiptRecordAtField),
                formatter: value => value.ToShortDateString()));

            yield return (ReceiptCategoryField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.CategoryName,
                fieldNumber: ++fieldNumber, fieldName: "区分",
                accept: visitor => visitor.StandardString(ReceiptCategoryField)));

            yield return (ReceiptAmountField = new NullableNumberFieldDefinition<ExportMatchingIndividual, decimal>(k => k.ReceiptAmount,
                fieldNumber: ++fieldNumber, fieldName: "入金額",
                accept: visitor => visitor.StandardNumber(ReceiptAmountField),
                formatter: value => value.ToString()));

            yield return (ReceiptRemainAmountField = new NullableNumberFieldDefinition<ExportMatchingIndividual, decimal>(k => k.ReceiptRemainAmount,
                fieldNumber: ++fieldNumber, fieldName: "入金残",
                accept: visitor => visitor.StandardNumber(ReceiptRemainAmountField),
                formatter: value => value.ToString()));

            yield return (NettingStateField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.NettingState,
                fieldNumber: ++fieldNumber, fieldName: "相殺",
                accept: visitor => visitor.StandardString(NettingStateField)));

            yield return (SourceBankInfoField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.SourceBank,
                fieldNumber: ++fieldNumber, fieldName: "仕向",
                accept: visitor => visitor.StandardString(SourceBankInfoField)));

            yield return (BankCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BankCode,
                fieldNumber: ++fieldNumber, fieldName: "銀行コード",
                accept: visitor => visitor.StandardString(BankCodeField)));

            yield return (BankNameField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BankName,
                fieldNumber: ++fieldNumber, fieldName: "銀行名",
                accept: visitor => visitor.StandardString(BankNameField)));

            yield return (BranchCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BranchCode,
                fieldNumber: ++fieldNumber, fieldName: "支店コード",
                accept: visitor => visitor.StandardString(BranchCodeField)));

            yield return (BranchNameField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BranchName,
                fieldNumber: ++fieldNumber, fieldName: "支店名",
                accept: visitor => visitor.StandardString(BranchNameField)));

            yield return (AccountTypeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.AccountTypeName,
                fieldNumber: ++fieldNumber, fieldName: "種別",
                accept: visitor => visitor.StandardString(AccountTypeField)));

            yield return (AccountNumberField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.AccountNumber,
                fieldNumber: ++fieldNumber, fieldName: "口座",
                accept: visitor => visitor.StandardString(AccountNumberField)));

            yield return (SectionCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.SectionCode,
                fieldNumber: ++fieldNumber, fieldName: "入金部門コード",
                accept: visitor => visitor.StandardString(SectionCodeField)));

            yield return (SectionNameField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.SectionName,
                fieldNumber: ++fieldNumber, fieldName: "入金部門名",
                accept: visitor => visitor.StandardString(SectionNameField)));

            yield return (ReceiptMemoField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ReceiptMemo,
                fieldNumber: ++fieldNumber, fieldName: "入金メモ",
                accept: visitor => visitor.StandardString(ReceiptMemoField)));

            yield return (ReceiptDueDateField = new NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime>(k => k.ReceiptDueAt,
                fieldNumber: ++fieldNumber, fieldName: "期日",
                accept: visitor => visitor.StandardNumber(ReceiptDueDateField),
                formatter: value => value.ToShortDateString()));

            yield return (ReceiptExcludeCategoryField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ExcludeCategoryName,
                fieldNumber: ++fieldNumber, fieldName: "対象外理由",
                accept: visitor => visitor.StandardString(ReceiptExcludeCategoryField)));

            yield return (PayerCodePrefixField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.PayerCodePrefix,
                fieldNumber: ++fieldNumber, fieldName: "仮想支店コード",
                accept: visitor => visitor.StandardString(PayerCodePrefixField)));

            yield return (PayerCodeSuffixField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.PayerCodeSuffix,
                fieldNumber: ++fieldNumber, fieldName: "仮想口座番号",
                accept: visitor => visitor.StandardString(PayerCodeSuffixField)));

            yield return (ReceiptCustomerCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ReceiptCustomerCode,
                fieldNumber: ++fieldNumber, fieldName: "得意先コード",
                accept: visitor => visitor.StandardString(ReceiptCustomerCodeField)));

            yield return (ReceiptCustomerNameField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ReceiptCustomerName,
                fieldNumber: ++fieldNumber, fieldName: "得意先名",
                accept: visitor => visitor.StandardString(ReceiptCustomerNameField)));

            yield return (ReceiptNote1Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ReceiptNote1,
                fieldNumber: ++fieldNumber, fieldName: "備考",
                accept: visitor => visitor.StandardString(ReceiptNote1Field)));

            yield return (ReceiptNote2Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ReceiptNote2,
                fieldNumber: ++fieldNumber, fieldName: "備考2",
                accept: visitor => visitor.StandardString(ReceiptNote2Field)));

            yield return (ReceiptNote3Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ReceiptNote3,
                fieldNumber: ++fieldNumber, fieldName: "備考3",
                accept: visitor => visitor.StandardString(ReceiptNote3Field)));

            yield return (ReceiptNote4Field = new StringFieldDefinition<ExportMatchingIndividual>(k => k.ReceiptNote4,
                fieldNumber: ++fieldNumber, fieldName: "備考4",
                accept: visitor => visitor.StandardString(ReceiptNote4Field)));

            yield return (BillNumberField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BillNumber,
                fieldNumber: ++fieldNumber, fieldName: "手形番号",
                accept: visitor => visitor.StandardString(BillNumberField)));

            yield return (BillBankCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BillBankCode,
                fieldNumber: ++fieldNumber, fieldName: "券面銀行コード",
                accept: visitor => visitor.StandardString(BillBankCodeField)));

            yield return (BillBranchCodeField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BillBranchCode,
                fieldNumber: ++fieldNumber, fieldName: "券面支店コード",
                accept: visitor => visitor.StandardString(BillBranchCodeField)));

            yield return (BillDrawAtField = new NullableNumberFieldDefinition<ExportMatchingIndividual, DateTime>(k => k.BillDrawAt,
                fieldNumber: ++fieldNumber, fieldName: "振出日",
                accept: visitor => visitor.StandardNumber(BillDrawAtField),
                formatter: value => value.ToShortDateString()));

            yield return (BillDrawerField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.BillDrawer,
                fieldNumber: ++fieldNumber, fieldName: "振出人",
                accept: visitor => visitor.StandardString(BillDrawerField)));

            yield return (ReceiptPayerNameRawField = new StringFieldDefinition<ExportMatchingIndividual>(k => k.PayerNameRaw,
                fieldNumber: ++fieldNumber, fieldName: "振込依頼人名（全て）",
                accept: visitor => visitor.StandardString(ReceiptPayerNameRawField)));
        }


        public void SetCurrencyFormat(int scale)
        {
            var format = scale == 0 ? "0" : "0." + new string('0', scale);
            BillingAmountField.Format = value => value.ToString(format);
            RemainAmountField.Format = value => value.ToString(format);
            DiscountAmountField.Format = value => value.ToString(format);
            TargetAmountField.Format = value => value.ToString(format);
            ReceiptAmountField.Format = value => value.ToString(format);
            ReceiptRemainAmountField.Format = value => value.ToString(format);
        }

        public IFieldDefinition<ExportMatchingIndividual> ConvertBillingSettingToFieldDefinition(string column)
        {
            IFieldDefinition<ExportMatchingIndividual> field = null;

            if      (column == "AssignmentFlag")            field = BillCheckField;
            else if (column == "CustomerCode")              field = CustomerCodeField;
            else if (column == "CustomerName")              field = CustomerNameField;
            else if (column == "BilledAt")                  field = BilledAtField;
            else if (column == "SalesAt")                   field = SalesAtField;
            else if (column == "DueAt")                     field = DueAtField;
            else if (column == "BillingAmount")             field = BillingAmountField;
            else if (column == "RemainAmount")              field = RemainAmountField;
            else if (column == "DiscountAmountSummary")     field = DiscountAmountField;
            else if (column == "TargetAmount")              field = TargetAmountField;
            else if (column == "InvoiceCode")               field = InvoiceCodeField;
            else if (column == "BillingCategory")           field = BillingCategoryField;
            else if (column == "DepartmentName")            field = BillingDepartmentField;
            else if (column == "Note1")                     field = BillingNote1Field;
            else if (column == "Note2")                     field = BillingNote2Field;
            else if (column == "Note3")                     field = BillingNote3Field;
            else if (column == "Note4")                     field = BillingNote4Field;
            else if (column == "Note5")                     field = BillingNote5Field;
            else if (column == "Note6")                     field = BillingNote6Field;
            else if (column == "Note7")                     field = BillingNote7Field;
            else if (column == "Note8")                     field = BillingNote8Field;
            else if (column == "Memo")                      field = BillingMemoField;
            else if (column == "InputType")                 field = BillingInputTypeField;
            else if (column == "ScheduledPaymentKey")       field = ScheduledPaymentKeyField;
            return field;
        }

        public IFieldDefinition<ExportMatchingIndividual> ConvertReceiptSettingToFieldDefinition(string column)
        {
            IFieldDefinition<ExportMatchingIndividual> field = null;

            if      (column == "AssignmentFlag")        field = RecCheckField;
            else if (column == "PayerName")             field = ReceiptPayerNameField;
            else if (column == "RecordedAt")            field = ReceiptRecordAtField;
            else if (column == "ReceiptCategoryName")   field = ReceiptCategoryField;
            else if (column == "ReceiptAmount")         field = ReceiptAmountField;
            else if (column == "RemainAmount")          field = ReceiptRemainAmountField;
            else if (column == "NettingState")          field = NettingStateField;
            else if (column == "SourceBank")            field = SourceBankInfoField;
            else if (column == "BankCode")              field = BankCodeField;
            else if (column == "BankName")              field = BankNameField;
            else if (column == "BranchCode")            field = BranchCodeField;
            else if (column == "BranchName")            field = BranchNameField;
            else if (column == "AccountTypeName")       field = AccountTypeField;
            else if (column == "AccountNumber")         field = AccountNumberField;
            else if (column == "SectionCode")           field = SectionCodeField;
            else if (column == "SectionName")           field = SectionNameField;
            else if (column == "Memo")                  field = ReceiptMemoField;
            else if (column == "DueAt")                 field = ReceiptDueDateField;
            else if (column == "ExcludeCategoryName")   field = ReceiptExcludeCategoryField;
            else if (column == "VirtualBranchCode")     field = PayerCodePrefixField;
            else if (column == "VirtualAccountNumber")  field = PayerCodeSuffixField;
            else if (column == "CustomerCode")          field = ReceiptCustomerCodeField;
            else if (column == "CustomerName")          field = ReceiptCustomerNameField;
            else if (column == "Note1")                 field = ReceiptNote1Field;
            else if (column == "Note2")                 field = ReceiptNote2Field;
            else if (column == "Note3")                 field = ReceiptNote3Field;
            else if (column == "Note4")                 field = ReceiptNote4Field;
            else if (column == "BillNumber")            field = BillNumberField;
            else if (column == "BillBankCode")          field = BillBankCodeField;
            else if (column == "BillBranchCode")        field = BillBranchCodeField;
            else if (column == "BillDrawAt")            field = BillDrawAtField;
            else if (column == "BillDrawer")            field = BillDrawerField;
            else if (column == "PayerNameRaw") field = ReceiptPayerNameRawField;
            return field;
        }
    }
}
