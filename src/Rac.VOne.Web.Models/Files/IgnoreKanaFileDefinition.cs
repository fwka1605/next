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
    public class IgnoreKanaFileDefinition : RowDefinition<IgnoreKana>
    {
        public StandardIdToCodeFieldDefinition<IgnoreKana, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<IgnoreKana> KanaField { get; private set; }
        public StandardIdToCodeFieldDefinition<IgnoreKana, Category> ExcludeCategoryIdField { get; private set; }

        public IgnoreKanaFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "除外カナ";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<IgnoreKana, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = true,
                Accept = VisitCompanyId,
            };
            KanaField = new StringFieldDefinition<IgnoreKana>(k => k.Kana)
            {
                FieldName = "カナ名",
                FieldNumber = 2,
                Required = false,
                Accept = VisitKana,
            };
            ExcludeCategoryIdField = new StandardIdToCodeFieldDefinition<IgnoreKana, Category>(
                    k => k.ExcludeCategoryId, c => c.Id,
                    k => k.ExcludeCategoryCode, c => c.Code)
            {
                FieldName = "対象外区分コード",
                FieldNumber = 3,
                Required = true,
                Accept = VisitExcludeCategoryId,
            };

            Fields.AddRange(new IFieldDefinition<IgnoreKana>[] {
                    CompanyIdField, KanaField, ExcludeCategoryIdField });
            KeyFields.AddRange(new IFieldDefinition<IgnoreKana>[]
            {
                KanaField,
            });
        }

        private bool VisitCompanyId(IFieldVisitor<IgnoreKana> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitKana(IFieldVisitor<IgnoreKana> visitor)
        {
            return visitor.PayerName(KanaField);
        }

        private bool VisitExcludeCategoryId(IFieldVisitor<IgnoreKana> visitor)
        {
            return visitor.CategoryCode(ExcludeCategoryIdField);
        }
        public Func<string[], Dictionary<string, int>> GetCategories { get; set; }
    }
}
