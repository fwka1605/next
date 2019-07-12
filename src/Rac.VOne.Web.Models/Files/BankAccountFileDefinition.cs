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
    public class BankAccountFileDefinition : RowDefinition<BankAccount>
    {
        public StandardIdToCodeFieldDefinition<BankAccount, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<BankAccount> BankCodeField { get; private set; }
        public StringFieldDefinition<BankAccount> BankNameField { get; private set; }
        public StringFieldDefinition<BankAccount> BranchCodeField { get; private set; }
        public StringFieldDefinition<BankAccount> BranchNameField { get; private set; }
        public NumberFieldDefinition<BankAccount, int> AccountTypeIdField { get; private set; }
        public StringFieldDefinition<BankAccount> AccountNumberField { get; private set; }
        public NullableForeignKeyFieldDefinition<BankAccount, int, Category> CategoryIdField { get; private set; }
        public NumberFieldDefinition<BankAccount, int> ImportSkippingField { get; private set; }
        public NullableForeignKeyFieldDefinition<BankAccount, int, Section> SectionIdField { get; private set; }

        public BankAccountFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "銀行口座";
            FileNameToken = DataTypeToken + "マスター";

            Fields.AddRange(GetFields());
            KeyFields.AddRange(new IFieldDefinition<BankAccount>[]
            {
                BankCodeField,
                BranchCodeField,
                AccountTypeIdField,
                AccountNumberField,
            });
        }

        private IEnumerable<IFieldDefinition<BankAccount>> GetFields()
        {
            var id = 0;
            //return null;
            yield return (CompanyIdField = new StandardIdToCodeFieldDefinition<BankAccount, Company>(k => k.CompanyId, c => c.Id, null, c => c.Code,
                "会社コード", ++id, accept: x => x.OwnCompanyCode(CompanyIdField)));
            yield return (BankCodeField = new StringFieldDefinition<BankAccount>(k => k.BankCode,
                "銀行コード", ++id, required: true, accept: x => x.BankCode(BankCodeField)));
            yield return (BankNameField = new StringFieldDefinition<BankAccount>(k => k.BankName,
                "銀行名", ++id, required: true, accept: x => x.BankName(BankNameField)));
            yield return (BranchCodeField = new StringFieldDefinition<BankAccount>(k => k.BranchCode,
                "支店コード", ++id, required: true, accept: x => x.BranchCode(BranchCodeField)));
            yield return (BranchNameField = new StringFieldDefinition<BankAccount>(x => x.BranchName,
                "支店名", ++id, required: true, accept: x => x.BranchName(BranchNameField)));
            yield return (AccountTypeIdField = new NumberFieldDefinition<BankAccount, int>(k => k.AccountTypeId,
                "預金種別", ++id, required: true, accept: x => x.AccountTypeId(AccountTypeIdField), formatter: x => x.ToString()));
            yield return (AccountNumberField = new StringFieldDefinition<BankAccount>(k => k.AccountNumber,
                "口座番号", ++id, required: true, accept: x => x.AccountNumber(AccountNumberField)));
            yield return (CategoryIdField = new NullableForeignKeyFieldDefinition<BankAccount, int, Category>(
                k => k.ReceiptCategoryId, c => c.Id,
                k => k.ReceiptCategoryCode, c => c.Code,
                "入金区分コード", ++id, required: true, accept: x => x.CategoryCode(CategoryIdField)));
            yield return (ImportSkippingField = new NumberFieldDefinition<BankAccount, int>(k => k.ImportSkipping,
                "取込対象外", ++id, required: true, accept: x => x.UseValueDate(ImportSkippingField), formatter: x => x.ToString()));
            yield return (SectionIdField = new NullableForeignKeyFieldDefinition<BankAccount, int, Section>(
                k => k.SectionId, c => c.Id,
                k => k.SectionCode, c => c.Code,
                "入金部門コード", ++id, accept: x => x.SectionCode(SectionIdField)));
        }

    }
}
