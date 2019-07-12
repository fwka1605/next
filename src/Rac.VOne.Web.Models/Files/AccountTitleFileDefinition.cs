using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class AccountTitleFileDefinition : RowDefinition<AccountTitle>
    {
        public StandardIdToCodeFieldDefinition<AccountTitle, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<AccountTitle> AccountTitleCodeField { get; private set; }
        public StringFieldDefinition<AccountTitle> AccountTitleNameField { get; private set; }
        public StringFieldDefinition<AccountTitle> ContraAccountCodeField { get; private set; }
        public StringFieldDefinition<AccountTitle> ContraAccountNameField { get; private set; }
        public StringFieldDefinition<AccountTitle> ContraAccountSubCodeField { get; private set; }


        public AccountTitleFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "科目";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<AccountTitle, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            AccountTitleCodeField = new StringFieldDefinition<AccountTitle>(k => k.Code)
            {
                FieldName = "科目コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitAccountTitleCode,
            };
            AccountTitleNameField = new StringFieldDefinition<AccountTitle>(k => k.Name)
            {
                FieldName = "科目名",
                FieldNumber = 3,
                Required = true,
                Accept = VisitAccountTitleName,
            };
            ContraAccountCodeField = new StringFieldDefinition<AccountTitle>(k => k.ContraAccountCode)
            {
                FieldName = "相手科目コード",
                FieldNumber = 4,
                Required = false,
                Accept = VisitContraAccountCode,
            };
            ContraAccountNameField = new StringFieldDefinition<AccountTitle>(k => k.ContraAccountName)
            {
                FieldName = "相手科目名",
                FieldNumber = 5,
                Required = false,
                Accept = VisitContraAccountName,
            };
            ContraAccountSubCodeField = new StringFieldDefinition<AccountTitle>(k => k.ContraAccountSubCode)
            {
                FieldName = "相手科目補助コード",
                FieldNumber = 6,
                Required = false,
                Accept = VisitContraAccountSubCode,
            };

            Fields.AddRange(new IFieldDefinition<AccountTitle>[] {
                            CompanyIdField, AccountTitleCodeField, AccountTitleNameField, ContraAccountCodeField, ContraAccountNameField, ContraAccountSubCodeField});
            KeyFields.AddRange(new IFieldDefinition<AccountTitle>[]
            {
                AccountTitleCodeField,
            });
        }
        private bool VisitCompanyId(IFieldVisitor<AccountTitle> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitAccountTitleCode(IFieldVisitor<AccountTitle> visitor)
        {
            return visitor.AccountTitleCode(AccountTitleCodeField);
        }

        private bool VisitAccountTitleName(IFieldVisitor<AccountTitle> visitor)
        {
            return visitor.AccountTitleName(AccountTitleNameField);
        }

        private bool VisitContraAccountCode(IFieldVisitor<AccountTitle> visitor)
        {
            return visitor.ContraAccountCode(ContraAccountCodeField);
        }

        private bool VisitContraAccountName(IFieldVisitor<AccountTitle> visitor)
        {
            return visitor.ContraAccountName(ContraAccountNameField);
        }

        private bool VisitContraAccountSubCode(IFieldVisitor<AccountTitle> visitor)
        {
            return visitor.ContraAccountSubCode(ContraAccountSubCodeField);
        }
    }
}
