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
    public class CurrencyFileDefinition : RowDefinition<Currency>
    {
        public StandardIdToCodeFieldDefinition<Currency, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<Currency> CurrencyCodeField { get; private set; }
        public StringFieldDefinition<Currency> CurrencyNameField { get; private set; }
        public StringFieldDefinition<Currency> CurrencySymbolField { get; private set; }
        public NumberFieldDefinition<Currency, int> CurrencyPrecisionField { get; private set; }
        public NumberFieldDefinition<Currency, int> CurrencyDisplayOrderField { get; private set; }
        public StringFieldDefinition<Currency> CurrencyNoteField { get; private set; }
        public NumberFieldDefinition<Currency, decimal> CurrencyToleranceField { get; private set; }

        public CurrencyFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "通貨";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<Currency, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            CurrencyCodeField = new StringFieldDefinition<Currency>(k => k.Code)
            {
                FieldName = "通貨コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitCurrencyCode,
            };
            CurrencyNameField = new StringFieldDefinition<Currency>(k => k.Name)
            {
                FieldName = "名称",
                FieldNumber = 3,
                Required = true,
                Accept = VisitCurrencyName,
            };
            CurrencySymbolField = new StringFieldDefinition<Currency>(k => k.Symbol)
            {
                FieldName = "単位",
                FieldNumber = 4,
                Required = true,
                Accept = VisitCurrencySymbol,
            };
            CurrencyPrecisionField = new NumberFieldDefinition<Currency, int>(k => k.Precision)
            {
                FieldName = "小数点以下桁数",
                FieldNumber = 5,
                Required = true,
                Accept = VisitCurrencyPrecision,
                Format = value => value.ToString(),
            };
            CurrencyDisplayOrderField = new NumberFieldDefinition<Currency, int>(k => k.DisplayOrder)
            {
                FieldName = "表示順番",
                FieldNumber = 6,
                Required = true,
                Accept = VisitCurrencyDisplayOrder,
                Format = value => value.ToString(),
            };
            CurrencyNoteField = new StringFieldDefinition<Currency>(k => k.Note)
            {
                FieldName = "備考",
                FieldNumber = 7,
                Required = false,
                Accept = VisitCurrencyNote,
            };
            CurrencyToleranceField = new NumberFieldDefinition<Currency, decimal>(k => k.Tolerance)
            {
                FieldName = "手数料誤差金額",
                FieldNumber = 8,
                Required = true,
                Accept = VisitCurrencyTolerance,
                Format = value => value.ToString(),
            };

            Fields.AddRange(new IFieldDefinition<Currency>[] {
                        CompanyIdField, CurrencyCodeField, CurrencyNameField, CurrencySymbolField, CurrencyPrecisionField
                        , CurrencyDisplayOrderField, CurrencyNoteField, CurrencyToleranceField});
            KeyFields.AddRange(new IFieldDefinition<Currency>[]
            {
                CurrencyCodeField,
            });
        }
        private bool VisitCompanyId(IFieldVisitor<Currency> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitCurrencyCode(IFieldVisitor<Currency> visitor)
        {
            return visitor.CurrencyCode(CurrencyCodeField);
        }

        private bool VisitCurrencyName(IFieldVisitor<Currency> visitor)
        {
            return visitor.CurrencyName(CurrencyNameField);
        }

        private bool VisitCurrencySymbol(IFieldVisitor<Currency> visitor)
        {
            return visitor.CurrencySymbol(CurrencySymbolField);
        }

        private bool VisitCurrencyPrecision(IFieldVisitor<Currency> visitor)
        {
            return visitor.CurrencyPrecision(CurrencyPrecisionField);
        }

        private bool VisitCurrencyDisplayOrder(IFieldVisitor<Currency> visitor)
        {
            return visitor.CurrencyDisplayOrder(CurrencyDisplayOrderField);
        }

        private bool VisitCurrencyNote(IFieldVisitor<Currency> visitor)
        {
            return visitor.CurrencyNote(CurrencyNoteField);
        }
        private bool VisitCurrencyTolerance(IFieldVisitor<Currency> visitor)
        {
            return visitor.CurrencyTolerance(CurrencyToleranceField);
        }
    }
}
