using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class SectionWithLoginUserFileDefinition : RowDefinition<SectionWithLoginUser>

    {
        public StandardIdToCodeFieldDefinition<SectionWithLoginUser, Company> CompanyIdField { get; private set; }
        public StandardIdToCodeFieldDefinition<SectionWithLoginUser, LoginUser> LoginUserCodeField { get; private set; }
        public StandardIdToCodeFieldDefinition<SectionWithLoginUser, Section> SectionCodeField { get; private set; }

        public SectionWithLoginUserFileDefinition(DataExpression applicationControl) : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "入金部門・担当者対応";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<SectionWithLoginUser, Company>(
                    null, null,  null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };

            SectionCodeField = new StandardIdToCodeFieldDefinition<SectionWithLoginUser, Section>(
                k => k.SectionId, c => c.Id,
                k => k.SectionCode, c => c.Code)
            {
                FieldName = "入金部門コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitSectionCode,
            };
            LoginUserCodeField = new StandardIdToCodeFieldDefinition<SectionWithLoginUser, LoginUser>(
               k => k.LoginUserId, c => c.Id,
               k => k.LoginUserCode, c => c.Code)
            {
                FieldName = "ログインユーザーコード",
                FieldNumber = 3,
                Required = true,
                Accept = VisitLoginUserCode,
            };
            Fields.AddRange(new IFieldDefinition<SectionWithLoginUser>[] {
                    CompanyIdField,SectionCodeField, LoginUserCodeField
            });
            KeyFields.AddRange(new IFieldDefinition<SectionWithLoginUser>[]
            {
                SectionCodeField,
                LoginUserCodeField,
            });
        }

        private bool VisitCompanyId(IFieldVisitor<SectionWithLoginUser> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }
        private bool VisitSectionCode(IFieldVisitor<SectionWithLoginUser> visitor)
        {
            return visitor.SectionCode(SectionCodeField);
        }
        private bool VisitLoginUserCode(IFieldVisitor<SectionWithLoginUser> visitor)
        {
            return visitor.LoginUserCodeField(LoginUserCodeField);
        }

        public Func<string[], Dictionary<string, int>> GetCategories { get; set; }
    }
}
