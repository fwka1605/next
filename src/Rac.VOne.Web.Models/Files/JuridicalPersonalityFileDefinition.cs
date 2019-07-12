using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class JuridicalPersonalityFileDefinition : RowDefinition<JuridicalPersonality>
    {
        public StandardIdToCodeFieldDefinition<JuridicalPersonality, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<JuridicalPersonality> KanaField { get; private set; }

        public JuridicalPersonalityFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "法人格";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<JuridicalPersonality, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            KanaField = new StringFieldDefinition<JuridicalPersonality>(
                k => k.Kana)
            {
                FieldName = "法人格",
                FieldNumber = 1,
                Required = true,
                Accept = VisitKanaField,
            };

            Fields.AddRange(new IFieldDefinition<JuridicalPersonality>[] {
                       CompanyIdField,KanaField});
            KeyFields.AddRange(new IFieldDefinition<JuridicalPersonality>[]
            {
                KanaField,
            });
        }
        private bool VisitCompanyId(IFieldVisitor<JuridicalPersonality> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }
        private bool VisitKanaField(IFieldVisitor<JuridicalPersonality> visitor)
        {
            return visitor.JuridicalPersonalityKana(KanaField);
        }
    }
}
