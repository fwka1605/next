using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class LoginUserFileDefinition : RowDefinition<LoginUser>
    {
        public StandardIdToCodeFieldDefinition<LoginUser, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<LoginUser> LoginUserCodeField { get; private set; }
        public StringFieldDefinition<LoginUser> LoginUserNameField { get; private set; }
        public StandardIdToCodeFieldDefinition<LoginUser, Department> DepartmentCodeField { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<LoginUser, Staff> StaffCodeField { get; private set; }
        public StringFieldDefinition<LoginUser> MailField { get; private set; }
        public NumberFieldDefinition<LoginUser, int> MenuLevelField { get; private set; }
        public NumberFieldDefinition<LoginUser, int> FunctionLevelField { get; private set; }
        public NumberFieldDefinition<LoginUser, int> UseClientField { get; private set; }
        public NumberFieldDefinition<LoginUser, int> UseWebViewerField { get; private set; }
        public StringFieldDefinition<LoginUser> InitialPasswordField { get; private set; }

        public LoginUserFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "ログインユーザー";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<LoginUser, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };

            LoginUserCodeField = new StringFieldDefinition<LoginUser>(k => k.Code)
            {
                FieldName = "ログインユーザーコード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitLoginUserCode,
            };

            LoginUserNameField = new StringFieldDefinition<LoginUser>(k => k.Name)
            {
                FieldName = "ログインユーザー名",
                FieldNumber = 3,
                Required = true,
                Accept = VisitLoginUserName,
            };

            DepartmentCodeField = new StandardIdToCodeFieldDefinition<LoginUser, Department>(
                    k => k.DepartmentId, c => c.Id, l => l.DepartmentCode, c => c.Code)
            {
                FieldName = "請求部門コード",
                FieldNumber = 4,
                Required = true,
                Accept = VisitDepartmentCode,
            };

            MailField = new StringFieldDefinition<LoginUser>(k => k.Mail)
            {
                FieldName = "メールアドレス",
                FieldNumber = 5,
                Accept = VisitMail,
            };

            MenuLevelField = new NumberFieldDefinition<LoginUser, int>(k => k.MenuLevel)
            {
                FieldName = "権限レベル",
                FieldNumber = 6,
                Required = true,
                Accept = VisitMenuLevelField,
                Format = value => value.ToString(),
            };

            FunctionLevelField = new NumberFieldDefinition<LoginUser, int>(k => k.FunctionLevel)
            {
                FieldName = "セキュリティ",
                FieldNumber = 7,
                Required = true,
                Accept = VisitFunctionLevelField,
                Format = value => value.ToString(),
            };

            UseClientField = new NumberFieldDefinition<LoginUser, int>(k => k.UseClient)
            {
                FieldName = "V-ONE利用",
                FieldNumber = 8,
                Required = true,
                Accept = VisitUseClientField,
                Format = value => value.ToString(),
            };

            UseWebViewerField = new NumberFieldDefinition<LoginUser, int>(k => k.UseWebViewer)
            {
                FieldName = "WebViewer利用",
                FieldNumber = 9,
                Required = true,
                Accept = VisitUseWebViewerField,
                Format = value => value.ToString(),
            };

            StaffCodeField = new StandardNullableIdToCodeFieldDefinition<LoginUser, Staff>(
                    k => k.AssignedStaffId, c => c.Id, l => l.StaffCode, c => c.Code)
            {
                FieldName = "営業担当者コード",
                FieldNumber = 10,
                Required = false,
                Accept = VisitStaffCode,
            };

            InitialPasswordField = new StringFieldDefinition<LoginUser>(k => k.InitialPassword)
            {
                FieldName = "初回パスワード",
                FieldNumber = 11,
                Required = false,
                Accept = VisitInitialPassword,
            };

            Fields.AddRange(new IFieldDefinition<LoginUser>[] {
                    CompanyIdField, LoginUserCodeField, LoginUserNameField, DepartmentCodeField, MailField,
                    MenuLevelField, UseClientField, UseWebViewerField, FunctionLevelField, StaffCodeField,
                    InitialPasswordField
            });
            KeyFields.AddRange(new IFieldDefinition<LoginUser>[]
            {
                LoginUserCodeField,
            });
        }

        private bool VisitCompanyId(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitLoginUserCode(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.OwnLoginUserCode(LoginUserCodeField);
        }

        private bool VisitLoginUserName(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.OwnLoginUserName(LoginUserNameField);
        }

        private bool VisitDepartmentCode(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.DepartmentCode(DepartmentCodeField);
        }

        private bool VisitMail(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.MailAddress(MailField);
        }

        private bool VisitMenuLevelField(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.MenuLevelField(MenuLevelField);
        }

        private bool VisitFunctionLevelField(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.FunctionLevelField(FunctionLevelField);
        }

        private bool VisitUseClientField(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.UseClientField(UseClientField);
        }

        private bool VisitUseWebViewerField(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.UseWebViewerField(UseWebViewerField);
        }

        private bool VisitStaffCode(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.StaffCode(StaffCodeField);
        }

        private bool VisitInitialPassword(IFieldVisitor<LoginUser> visitor)
        {
            return visitor.InitialPassword(InitialPasswordField);
        }
    }
}
