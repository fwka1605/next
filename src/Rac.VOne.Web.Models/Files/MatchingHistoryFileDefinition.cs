using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Models.Files
{
    public class MatchingHistoryFileDefinition : RowDefinition<ExportMatchingHistory>
    {
        public StringFieldDefinition<ExportMatchingHistory> MatchingAtField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> SectionCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> SectionNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> DepartmentNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> CustomerCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> CustomerNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BilledAtField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> SalesAtField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BillingCategoryField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> CollectCategoryField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BilledAmountField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BilledAmountExcludingTaxField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> TaxAmountField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> MatchingAmountField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BilledRemainField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BillingNote1Field { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BillingNote2Field { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BillingNote3Field { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BillingNote4Field { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> RecordedAtField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> ReceiptIdField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> ReceiptCategoryField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> ReceiptAmountField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> AdvanceReceivedOccuredField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> ReceiptRemainField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> PayerNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BankCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BankNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BranchCodeField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> BranchNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> AccountNumberField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> ReceiptNote1Field { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> ReceiptNote2Field { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> ReceiptNote3Field { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> ReceiptNote4Field { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> VirtualBranchCode { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> VirtualAccountNumberField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> LoginUserNameField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> MatchingProcessTypeField { get; private set; }
        public StringFieldDefinition<ExportMatchingHistory> MatchingMemoField { get; private set; }

        public MatchingHistoryFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "消込履歴データ";
            FileNameToken = DataTypeToken;

            var fieldNumber = 1;
            MatchingAtField = new StringFieldDefinition<ExportMatchingHistory>(k => k.CreateAt)
            {
                FieldName = "消込日時",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitMatchingAt,
            };
            SectionCodeField = new StringFieldDefinition<ExportMatchingHistory>(k => k.SectionCode)
            {
                FieldName = "入金部門コード",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitSectionCode,
            };
            SectionNameField = new StringFieldDefinition<ExportMatchingHistory>(k => k.SectionName)
            {
                FieldName = "入金部門名",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitSectionName,
            };
            DepartmentCodeField = new StringFieldDefinition<ExportMatchingHistory>(k => k.DepartmentCode)
            {
                FieldName = "請求部門コード",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitDepartmentCode,
            };
            DepartmentNameField = new StringFieldDefinition<ExportMatchingHistory>(k => k.DepartmentName)
            {
                FieldName = "請求部門名",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitDepartmentName,
            };
            CustomerCodeField = new StringFieldDefinition<ExportMatchingHistory>(k => k.CustomerCode)
            {
                FieldName = "得意先コード",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitCustomerCode,
            };
            CustomerNameField = new StringFieldDefinition<ExportMatchingHistory>(k => k.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitCustomerName,
            };
            BilledAtField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BilledAt)
            {
                FieldName = "請求日",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBilledAt,
            };
            SalesAtField = new StringFieldDefinition<ExportMatchingHistory>(k => k.SalesAt)
            {
                FieldName = "売上日",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitSalesAt,
            };
            InvoiceCodeField = new StringFieldDefinition<ExportMatchingHistory>(k => k.InvoiceCode)
            {
                FieldName = "請求書番号",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitInvoiceCode,
            };
            BillingCategoryField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BillingCategory)
            {
                FieldName = "請求区分",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBillingCategory,
            };
            CollectCategoryField = new StringFieldDefinition<ExportMatchingHistory>(k => k.CollectCategory)
            {
                FieldName = "回収区分",
                FieldNumber = fieldNumber++,
                Required = false,
                Accept = VisitCollectCategory,
            };
            BilledAmountField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BillingAmount)
            {
                FieldName = "請求額(税込)",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBilledAmount,
            };
            BilledAmountExcludingTaxField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BillingAmountExcludingTax)
            {
                FieldName = "請求額(税抜)",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBilledAmountExcludingTax,
            };
            TaxAmountField = new StringFieldDefinition<ExportMatchingHistory>(k => k.TaxAmount)
            {
                FieldName = "消費税",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitTaxAmount,
            };
            MatchingAmountField = new StringFieldDefinition<ExportMatchingHistory>(k => k.MatchingAmount)
            {
                FieldName = "消込金額",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitMatchingAmount,
            };
            BilledRemainField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BillingRemain)
            {
                FieldName = "請求残",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBilledRemain,
            };
            BillingNote1Field = new StringFieldDefinition<ExportMatchingHistory>(k => k.BillingNote1)
            {
                FieldName = "備考",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBillingNote1,
            };
            BillingNote2Field = new StringFieldDefinition<ExportMatchingHistory>(k => k.BillingNote2)
            {
                FieldName = "備考2",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBillingNote2,
            };
            BillingNote3Field = new StringFieldDefinition<ExportMatchingHistory>(k => k.BillingNote3)
            {
                FieldName = "備考3",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBillingNote3,
            };
            BillingNote4Field = new StringFieldDefinition<ExportMatchingHistory>(k => k.BillingNote4)
            {
                FieldName = "備考4",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBillingNote4,
            };
            RecordedAtField = new StringFieldDefinition<ExportMatchingHistory>(k => k.RecordedAt)
            {
                FieldName = "入金日",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitRecordedAt,
            };
            ReceiptIdField = new StringFieldDefinition<ExportMatchingHistory>(k => k.ReceiptId)
            {
                FieldName = "入金ID",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitReceiptId,
            };
            ReceiptCategoryField = new StringFieldDefinition<ExportMatchingHistory>(k => k.ReceiptCategory)
            {
                FieldName = "入金区分",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitReceiptCategory,
            };
            ReceiptAmountField = new StringFieldDefinition<ExportMatchingHistory>(k => k.ReceiptAmount)
            {
                FieldName = "入金額",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitReceiptAmount,
            };
            AdvanceReceivedOccuredField = new StringFieldDefinition<ExportMatchingHistory>(k => k.AdvanceReceivedOccuredString)
            {
                FieldName = "前受",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitAdvanceReceivedOccured,
            };
            ReceiptRemainField = new StringFieldDefinition<ExportMatchingHistory>(k => k.ReceiptRemain)
            {
                FieldName = "入金残",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitReceiptRemain,
            };
            PayerNameField = new StringFieldDefinition<ExportMatchingHistory>(k => k.PayerName)
            {
                FieldName = "振込依頼人名",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitPayerName,
            };
            BankCodeField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BankCode)
            {
                FieldName = "銀行コード",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBankCode,
            };
            BankNameField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BankName)
            {
                FieldName = "銀行名",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBankName,
            };
            BranchCodeField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BranchCode)
            {
                FieldName = "支店コード",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBranchCode,
            };
            BranchNameField = new StringFieldDefinition<ExportMatchingHistory>(k => k.BranchName)
            {
                FieldName = "支店名",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitBranchName,
            };
            AccountNumberField = new StringFieldDefinition<ExportMatchingHistory>(k => k.AccountNumber)
            {
                FieldName = "口座番号",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitAccountNumber,
            };
            ReceiptNote1Field = new StringFieldDefinition<ExportMatchingHistory>(k => k.ReceiptNote1)
            {
                FieldName = "備考",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitReceiptNote1,
            };
            ReceiptNote2Field = new StringFieldDefinition<ExportMatchingHistory>(k => k.ReceiptNote2)
            {
                FieldName = "備考2",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitReceiptNote2,
            };
            ReceiptNote3Field = new StringFieldDefinition<ExportMatchingHistory>(k => k.ReceiptNote3)
            {
                FieldName = "備考3",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitReceiptNote3,
            };
            ReceiptNote4Field = new StringFieldDefinition<ExportMatchingHistory>(k => k.ReceiptNote4)
            {
                FieldName = "備考4",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitReceiptNote4,
            };
            VirtualBranchCode = new StringFieldDefinition<ExportMatchingHistory>(k => k.VirtualBranchCode)
            {
                FieldName = "仮想支店コード",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitPayerCodeFirst,
            };
            VirtualAccountNumberField = new StringFieldDefinition<ExportMatchingHistory>(k => k.VirtualAccountNumber)
            {
                FieldName = "仮想口座番号",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitPayerCodeLast,
            };
            LoginUserNameField = new StringFieldDefinition<ExportMatchingHistory>(k => k.LoginUserName)
            {
                FieldName = "消込実行ユーザー",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitLoginUserName,
            };
            MatchingProcessTypeField = new StringFieldDefinition<ExportMatchingHistory>(k => k.MatchingProcessTypeString)
            {
                FieldName = "消込",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitMatchingProcessType,
            };
            MatchingMemoField = new StringFieldDefinition<ExportMatchingHistory>(k => k.MatchingMemo)
            {
                FieldName = "消込メモ",
                FieldNumber = fieldNumber++,
                Required = true,
                Accept = VisitMatchingMemo,
            };
            

            Fields.AddRange(new IFieldDefinition<ExportMatchingHistory>[] {
                MatchingAtField
                ,SectionCodeField
                ,SectionNameField
                ,DepartmentCodeField
                ,DepartmentNameField
                ,CustomerCodeField
                ,CustomerNameField
                ,BilledAtField
                ,SalesAtField
                ,InvoiceCodeField
                ,BillingCategoryField,
                CollectCategoryField,
                BilledAmountField
                ,BilledAmountExcludingTaxField
                ,TaxAmountField
                ,MatchingAmountField
                ,BilledRemainField
                ,BillingNote1Field
                ,BillingNote2Field
                ,BillingNote3Field
                ,BillingNote4Field
                ,RecordedAtField
                ,ReceiptIdField
                ,ReceiptCategoryField
                ,ReceiptAmountField
                ,AdvanceReceivedOccuredField
                ,ReceiptRemainField
                ,PayerNameField
                ,BankCodeField
                ,BankNameField
                ,BranchCodeField
                ,BranchNameField
                ,AccountNumberField
                ,ReceiptNote1Field
                ,ReceiptNote2Field
                ,ReceiptNote3Field
                ,ReceiptNote4Field
                ,VirtualBranchCode
                ,VirtualAccountNumberField
                ,LoginUserNameField
                ,MatchingProcessTypeField
                ,MatchingMemoField
            });
        }

        private bool VisitMatchingAt(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(MatchingAtField);
        }

        private bool VisitSectionCode(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(SectionCodeField);
        }

        private bool VisitSectionName(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(SectionNameField);
        }

        private bool VisitDepartmentCode(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(DepartmentCodeField);
        }

        private bool VisitDepartmentName(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(DepartmentNameField);
        }

        private bool VisitCustomerCode(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }

        private bool VisitCustomerName(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }

        private bool VisitBilledAt(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BilledAtField);
        }

        private bool VisitSalesAt(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(SalesAtField);
        }

        private bool VisitInvoiceCode(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(InvoiceCodeField);
        }

        private bool VisitBillingCategory(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BillingCategoryField);
        }
        private bool VisitCollectCategory(IFieldVisitor<ExportMatchingHistory> visitor)
            => visitor.StandardString(CollectCategoryField);

        private bool VisitBilledAmount(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BilledAmountField);
        }

        private bool VisitBilledAmountExcludingTax(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BilledAmountExcludingTaxField);
        }

        private bool VisitTaxAmount(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(TaxAmountField);
        }

        private bool VisitMatchingAmount(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(MatchingAmountField);
        }

        private bool VisitBilledRemain(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BilledRemainField);
        }

        private bool VisitBillingNote1(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BillingNote1Field);
        }

        private bool VisitBillingNote2(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BillingNote2Field);
        }

        private bool VisitBillingNote3(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BillingNote3Field);
        }

        private bool VisitBillingNote4(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BillingNote4Field);
        }

        private bool VisitRecordedAt(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(RecordedAtField);
        }

        private bool VisitReceiptId(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(ReceiptIdField);
        }

        private bool VisitReceiptCategory(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(ReceiptCategoryField);
        }

        private bool VisitReceiptAmount(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(ReceiptAmountField);
        }

        private bool VisitAdvanceReceivedOccured(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(AdvanceReceivedOccuredField);
        }

        private bool VisitReceiptRemain(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(ReceiptRemainField);
        }

        private bool VisitPayerName(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(PayerNameField);
        }

        private bool VisitBankCode(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BankCodeField);
        }

        private bool VisitBankName(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BankNameField);
        }

        private bool VisitBranchCode(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BranchCodeField);
        }

        private bool VisitBranchName(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(BranchNameField);
        }

        private bool VisitAccountNumber(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(AccountNumberField);
        }

        private bool VisitReceiptNote1(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(ReceiptNote1Field);
        }

        private bool VisitReceiptNote2(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(ReceiptNote2Field);
        }

        private bool VisitReceiptNote3(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(ReceiptNote3Field);
        }

        private bool VisitReceiptNote4(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(ReceiptNote4Field);
        }

        private bool VisitPayerCodeFirst(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(VirtualBranchCode);
        }

        private bool VisitPayerCodeLast(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(VirtualAccountNumberField);
        }

        private bool VisitLoginUserName(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(LoginUserNameField);
        }

        private bool VisitMatchingProcessType(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(MatchingProcessTypeField);
        }

        private bool VisitMatchingMemo(IFieldVisitor<ExportMatchingHistory> visitor)
        {
            return visitor.StandardString(MatchingMemoField);
        }
    }
}
