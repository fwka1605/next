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
    public class StaffFileDefinition : RowDefinition<Staff>
    {
        public StandardIdToCodeFieldDefinition<Staff, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<Staff> StaffCodeField { get; private set; }
        public StringFieldDefinition<Staff> StaffNameField { get; private set; }
        public StandardIdToCodeFieldDefinition<Staff, Department> DepartmentIdField { get; private set; }
        public StringFieldDefinition<Staff> MailField { get; private set; }
        public StringFieldDefinition<Staff> TelField { get; private set; }
        public StringFieldDefinition<Staff> FaxField { get; private set; }

        public StaffFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "営業担当者";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<Staff, Company>(
                   k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };

            StaffCodeField = new StringFieldDefinition<Staff>(k => k.Code)
            {
                FieldName = "営業担当者コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitStaffCode,
            };

            StaffNameField = new StringFieldDefinition<Staff>(k => k.Name)
            {
                FieldName = "営業担当者名",
                FieldNumber = 3,
                Required = true,
                Accept = VisitStaffName,
            };

            DepartmentIdField = new StandardIdToCodeFieldDefinition<Staff, Department>(
                    k => k.DepartmentId, c => c.Id,
                    k => k.DepartmentCode, c => c.Code)
            {
                FieldName = "請求部門コード",
                FieldNumber = 4,
                Required = true,
                Accept = VisitDepartmentId,
            };

            MailField = new StringFieldDefinition<Staff>(k => k.Mail)
            {
                FieldName = "メールアドレス",
                FieldNumber = 5,
                Accept = VisitStaffMail,
            };

            TelField = new StringFieldDefinition<Staff>(k => k.Tel)
            {
                FieldName = "電話番号",
                FieldNumber = 6,
                Required = false,
                Accept = VisitStaffTel,
            };

            FaxField = new StringFieldDefinition<Staff>(k => k.Fax)
            {
                FieldName = "FAX番号",
                FieldNumber = 7,
                Required = false,
                Accept = VisitStaffFax,
            };

            Fields.AddRange(new IFieldDefinition<Staff>[] {
                    CompanyIdField, StaffCodeField, StaffNameField, DepartmentIdField, MailField, TelField, FaxField });
            KeyFields.AddRange(new IFieldDefinition<Staff>[]
            {
                StaffCodeField,
            });
        }

        private bool VisitCompanyId(IFieldVisitor<Staff> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }
        private bool VisitStaffCode(IFieldVisitor<Staff> visitor)
        {
            return visitor.StaffCode(StaffCodeField);
        }
        private bool VisitStaffName(IFieldVisitor<Staff> visitor)
        {
            return visitor.StaffName(StaffNameField);
        }
        private bool VisitDepartmentId(IFieldVisitor<Staff> visitor)
        {
            return visitor.DepartmentCode(DepartmentIdField);
        }
        private bool VisitStaffMail(IFieldVisitor<Staff> visitor)
        {
            return visitor.MailAddress(MailField);
        }
        private bool VisitStaffTel(IFieldVisitor<Staff> visitor)
        {
            return visitor.Tel(TelField);
        }
        private bool VisitStaffFax(IFieldVisitor<Staff> visitor)
        {
            return visitor.Fax(FaxField);
        }
    }
}
