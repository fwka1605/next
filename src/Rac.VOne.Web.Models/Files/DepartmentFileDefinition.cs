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
    public class DepartmentFileDefinition : RowDefinition<Department>
    {
        public StandardIdToCodeFieldDefinition<Department, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<Department> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<Department> DepartmentNameField { get; private set; }
        public StringFieldDefinition<Department> NoteField { get; private set; }
        public NullableForeignKeyFieldDefinition<Department, int, Staff> StaffIdField { get; private set; }

        public DepartmentFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "請求部門";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<Department, Company>(
                  k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            DepartmentCodeField = new StringFieldDefinition<Department>(k => k.Code)
            {
                FieldName = "請求部門コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitDepartmentCode,
            };
            DepartmentNameField = new StringFieldDefinition<Department>(k => k.Name)
            {
                FieldName = "請求部門名",
                FieldNumber = 3,
                Required = true,
                Accept = VisitDepartmentName,
            };

            StaffIdField = new NullableForeignKeyFieldDefinition<Department, int, Staff>(
                    //k => k.StaffId ?? 0, c => c.Id,
                    k => k.StaffId, c => c.Id,
                    k => k.StaffCode, c => c.Code)
            {
                FieldName = "回収責任者コード",
                FieldNumber = 4,
                Required = false,
                Accept = VisitStaffId,
            };

            NoteField = new StringFieldDefinition<Department>(k => k.Note)
            {
                FieldName = "備考",
                FieldNumber = 5,
                Required = false,
                Accept = VisitNote,
            };
            Fields.AddRange(new IFieldDefinition<Department>[] {
                    CompanyIdField, DepartmentCodeField, DepartmentNameField, StaffIdField, NoteField });
            KeyFields.AddRange(new IFieldDefinition<Department>[]
            {
                DepartmentCodeField,
            });
        }

        private bool VisitCompanyId(IFieldVisitor<Department> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitDepartmentCode(IFieldVisitor<Department> visitor)
        {
            return visitor.DepartmentCode(DepartmentCodeField);
        }

        private bool VisitStaffId(IFieldVisitor<Department> visitor)
        {
            return visitor.StaffCode(StaffIdField);
        }

        private bool VisitDepartmentName(IFieldVisitor<Department> visitor)
        {
            return visitor.DepartmentName(DepartmentNameField);
        }

        private bool VisitNote(IFieldVisitor<Department> visitor)
        {
            return visitor.DepartmentNote(NoteField);
        }
    }
}
