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
    public class KanaHistoryCustomerFileDefinition : RowDefinition<KanaHistoryCustomer>
    {
        public StandardIdToCodeFieldDefinition<KanaHistoryCustomer, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<KanaHistoryCustomer> PayerNameField { get; private set; }
        public StandardIdToCodeFieldDefinition<KanaHistoryCustomer, Customer> CustomerIdField { get; private set; }
        public StringFieldDefinition<KanaHistoryCustomer> SourceBankNameField { get; private set; }
        public StringFieldDefinition<KanaHistoryCustomer> SourceBrankNameField { get; private set; }
        public NumberFieldDefinition<KanaHistoryCustomer, int> HitCountField { get; private set; }


        public KanaHistoryCustomerFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "得意先学習履歴";
            FileNameToken = DataTypeToken;

            CompanyIdField = new StandardIdToCodeFieldDefinition<KanaHistoryCustomer, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            PayerNameField = new StringFieldDefinition<KanaHistoryCustomer>(k => k.PayerName)
            {
                FieldName = "振込依頼人名",
                FieldNumber = 2,
                Required = false,
                Accept = VisitKana,
            };
            CustomerIdField = new StandardIdToCodeFieldDefinition<KanaHistoryCustomer, Customer>(
                    k => k.CustomerId, c => c.Id,
                    k => k.CustomerCode, c => c.Code)
            {
                FieldName = "得意先コード",
                FieldNumber = 3,
                Required = true,
                Accept = VisitcustomerId,
            };
            SourceBankNameField = new StringFieldDefinition<KanaHistoryCustomer>(k => k.SourceBankName)
            {
                FieldName = "仕向銀行",
                FieldNumber = 4,
                Required = false,
                Accept = VisitSourceBankName,
            };
            SourceBrankNameField = new StringFieldDefinition<KanaHistoryCustomer>(k => k.SourceBranchName)
            {
                FieldName = "仕向支店",
                FieldNumber = 5,
                Required = false,
                Accept = VisitSourceBrankName,
            };
            HitCountField = new NumberFieldDefinition<KanaHistoryCustomer, int>(k => k.HitCount)
            {
                FieldName = "消込回数",
                FieldNumber = 6,
                Required = true,
                Accept = VisitHitCount,
                Format = value => value.ToString(),
            };

            Fields.AddRange(new IFieldDefinition<KanaHistoryCustomer>[] {
                        CompanyIdField, PayerNameField, CustomerIdField,SourceBankNameField,SourceBrankNameField,HitCountField});
            KeyFields.AddRange(new IFieldDefinition<KanaHistoryCustomer>[]
            {
                PayerNameField,
                CustomerIdField,
                SourceBankNameField,
                SourceBrankNameField,
            });
        }
        private bool VisitCompanyId(IFieldVisitor<KanaHistoryCustomer> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitKana(IFieldVisitor<KanaHistoryCustomer> visitor)
        {
            return visitor.PayerName(PayerNameField);
        }

        private bool VisitcustomerId(IFieldVisitor<KanaHistoryCustomer> visitor)
        {
            return visitor.CustomerCode(CustomerIdField);
        }

        private bool VisitSourceBankName(IFieldVisitor<KanaHistoryCustomer> visitor)
        {
            return visitor.SourceBank(SourceBankNameField);
        }

        private bool VisitSourceBrankName(IFieldVisitor<KanaHistoryCustomer> visitor)
        {
            return visitor.SourceBranch(SourceBrankNameField);
        }

        private bool VisitHitCount(IFieldVisitor<KanaHistoryCustomer> visitor)
        {
            return visitor.StandardNumber<int>(HitCountField);
        }
        public Func<string[], Dictionary<string, int>> GetCategories { get; set; }
    }
}
