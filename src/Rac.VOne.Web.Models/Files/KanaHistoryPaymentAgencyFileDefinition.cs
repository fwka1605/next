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
    public class KanaHistoryPaymentAgencyFileDefinition : RowDefinition<KanaHistoryPaymentAgency>
    {
        public StandardIdToCodeFieldDefinition<KanaHistoryPaymentAgency, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<KanaHistoryPaymentAgency> PayerNameField { get; private set; }
        public StandardIdToCodeFieldDefinition<KanaHistoryPaymentAgency, PaymentAgency> PaymentAgencyIdField { get; private set; }
        public StringFieldDefinition<KanaHistoryPaymentAgency> SourceBankNameField { get; private set; }
        public StringFieldDefinition<KanaHistoryPaymentAgency> SourceBrankNameField { get; private set; }
        public NumberFieldDefinition<KanaHistoryPaymentAgency, int> HitCountField { get; private set; }


        public KanaHistoryPaymentAgencyFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "決済代行会社学習履歴";
            FileNameToken = DataTypeToken;

            CompanyIdField = new StandardIdToCodeFieldDefinition<KanaHistoryPaymentAgency, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            PayerNameField = new StringFieldDefinition<KanaHistoryPaymentAgency>(k => k.PayerName)
            {
                FieldName = "振込依頼人名",
                FieldNumber = 2,
                Required = false,
                Accept = VisitKana,
            };
            PaymentAgencyIdField = new StandardIdToCodeFieldDefinition<KanaHistoryPaymentAgency, PaymentAgency>(
                    k => k.PaymentAgencyId, c => c.Id,
                    k => k.PaymentAgencyCode, c => c.Code)
            {
                FieldName = "決済代行会社コード",
                FieldNumber = 3,
                Required = true,
                Accept = VisitpaymentagencyId,
            };
            SourceBankNameField = new StringFieldDefinition<KanaHistoryPaymentAgency>(k => k.SourceBankName)
            {
                FieldName = "仕向銀行",
                FieldNumber = 4,
                Required = false,
                Accept = VisitSourceBankName,
            };
            SourceBrankNameField = new StringFieldDefinition<KanaHistoryPaymentAgency>(k => k.SourceBranchName)
            {
                FieldName = "仕向支店",
                FieldNumber = 5,
                Required = false,
                Accept = VisitSourceBrankName,
            };
            HitCountField = new NumberFieldDefinition<KanaHistoryPaymentAgency, int>(k => k.HitCount)
            {
                FieldName = "消込回数",
                FieldNumber = 6,
                Required = true,
                Accept = VisitHitCount,
                Format = value => value.ToString(),
            };

            Fields.AddRange(new IFieldDefinition<KanaHistoryPaymentAgency>[] {
                        CompanyIdField, PayerNameField, PaymentAgencyIdField,SourceBankNameField,SourceBrankNameField,HitCountField});
            KeyFields.AddRange(new IFieldDefinition<KanaHistoryPaymentAgency>[]
            {
                PayerNameField,
                PaymentAgencyIdField,
                SourceBankNameField,
                SourceBrankNameField,
            });
        }
        private bool VisitCompanyId(IFieldVisitor<KanaHistoryPaymentAgency> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitKana(IFieldVisitor<KanaHistoryPaymentAgency> visitor)
        {
            return visitor.PayerName(PayerNameField);
        }

        private bool VisitpaymentagencyId(IFieldVisitor<KanaHistoryPaymentAgency> visitor)
        {
            return visitor.PaymentAgencyCode(PaymentAgencyIdField);
        }

        private bool VisitSourceBankName(IFieldVisitor<KanaHistoryPaymentAgency> visitor)
        {
            return visitor.SourceBank(SourceBankNameField);
        }

        private bool VisitSourceBrankName(IFieldVisitor<KanaHistoryPaymentAgency> visitor)
        {
            return visitor.SourceBranch(SourceBrankNameField);
        }

        private bool VisitHitCount(IFieldVisitor<KanaHistoryPaymentAgency> visitor)
        {
            return visitor.StandardNumber<int>(HitCountField);
        }
        public Func<string[], Dictionary<string, int>> GetCategories { get; set; }
    }
}
