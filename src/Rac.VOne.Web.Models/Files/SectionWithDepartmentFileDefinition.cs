using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class SectionWithDepartmentFileDefinition : RowDefinition<SectionWithDepartment>
    {
        public StandardIdToCodeFieldDefinition<SectionWithDepartment, Company> CompanyIdField { get; private set; }
        public StandardIdToCodeFieldDefinition<SectionWithDepartment, Section> SectionCodeField { get; private set; }
        public StandardIdToCodeFieldDefinition<SectionWithDepartment, Department> DepartmentCodeField { get; private set; }

        public SectionWithDepartmentFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "入金・請求部門対応";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<SectionWithDepartment, Company>(
                        null, null,
                        null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            SectionCodeField = new StandardIdToCodeFieldDefinition<SectionWithDepartment, Section>(
                    swd => swd.SectionId, s => s.Id,
                    swd => swd.SectionCode, s => s.Code)
            {
                FieldName = "入金部門コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitSectionCode,
            };

            DepartmentCodeField = new StandardIdToCodeFieldDefinition<SectionWithDepartment, Department>(
                    swd => swd.DepartmentId, d => d.Id,
                    swd => swd.DepartmentCode, d => d.Code)
            {
                FieldName = "請求部門コード",
                FieldNumber = 3,
                Required = true,
                Accept = VisitDepartmentCode
            };
            Fields.AddRange(new IFieldDefinition<SectionWithDepartment>[] {
                        CompanyIdField, SectionCodeField, DepartmentCodeField});
            KeyFields.AddRange(new IFieldDefinition<SectionWithDepartment>[]
            {
                SectionCodeField,
                DepartmentCodeField,
            });
        }
        private bool VisitCompanyId(IFieldVisitor<SectionWithDepartment> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitSectionCode(IFieldVisitor<SectionWithDepartment> visitor)
        {
            return visitor.SectionCode(SectionCodeField);

        }

        private bool VisitDepartmentCode(IFieldVisitor<SectionWithDepartment> visitor)
        {
            return visitor.DepartmentCode(DepartmentCodeField);
        }
    }
}
