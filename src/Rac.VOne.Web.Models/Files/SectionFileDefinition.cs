using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class SectionFileDefinition : RowDefinition<Section>
    {
        public StandardIdToCodeFieldDefinition<Section, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<Section> SectionCodeField { get; private set; }
        public StringFieldDefinition<Section> SectionNameField { get; private set; }
        public StringFieldDefinition<Section> NoteField { get; private set; }
        public StringFieldDefinition<Section> PayerCodeLeftField { get; private set; }
        public StringFieldDefinition<Section> PayerCodeRightField { get; private set; }

        public SectionFileDefinition(DataExpression applicationControl) : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "入金部門";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<Section, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };

            SectionCodeField = new StringFieldDefinition<Section>(k => k.Code)
            {
                FieldName = "入金部門コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitSectionCode,
            };

            SectionNameField = new StringFieldDefinition<Section>(k => k.Name)
            {
                FieldName = "入金部門名",
                FieldNumber = 3,
                Required = true,
                Accept = VisitSectionName,
            };

            NoteField = new StringFieldDefinition<Section>(k => k.Note)
            {
                FieldName = "備考",
                FieldNumber = 4,
                Required = false,
                Accept = VisitNote,
            };

            PayerCodeLeftField = new StringFieldDefinition<Section>(k => k.PayerCodeLeft)
            {
                FieldName = "仮想支店コード",
                FieldNumber = 5,
                Required = false,
                Accept = VisitPyaerCodeLeft,
            };

            PayerCodeRightField = new StringFieldDefinition<Section>(k => k.PayerCodeRight)
            {
                FieldName = "仮想口座番号",
                FieldNumber = 6,
                Required = false,
                Accept = VisitPyaerCodeRight,
            };

            Fields.AddRange(new IFieldDefinition<Section>[] {
                    CompanyIdField, SectionCodeField, SectionNameField, NoteField,PayerCodeLeftField,PayerCodeRightField
            });
            KeyFields.AddRange(new IFieldDefinition<Section>[] { SectionCodeField });
        }

        private bool VisitCompanyId(IFieldVisitor<Section> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitSectionCode(IFieldVisitor<Section> visitor)
        {
            return visitor.SectionCode(SectionCodeField);
        }

        private bool VisitSectionName(IFieldVisitor<Section> visitor)
        {
            return visitor.SectionName(SectionNameField);
        }

        private bool VisitNote(IFieldVisitor<Section> visitor)
        {
            return visitor.SectionNote(NoteField);
        }

        private bool VisitPyaerCodeLeft(IFieldVisitor<Section> visitor)
        {
            return visitor.BranchCode(PayerCodeLeftField);
        }

        private bool VisitPyaerCodeRight(IFieldVisitor<Section> visitor)
        {
            return visitor.AccountNumber(PayerCodeRightField);
        }
    }
}
