using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class CustomerRegistrationFeeFileDefinition : RowDefinition<CustomerFee>
    {
        public StandardIdToCodeFieldDefinition<CustomerFee, Company> CompanyIdField { get; private set; }
        public StandardIdToCodeFieldDefinition<CustomerFee, Customer> CustomerIdField { get; private set; }
        public NullableNumberFieldDefinition<CustomerFee,decimal> FeeField { get; private set; }
        public StandardIdToCodeFieldDefinition<CustomerFee, Currency> CurrencyCodeField { get; private set; }

        private int foreignflg = 0;

        public CustomerRegistrationFeeFileDefinition(DataExpression applicationControl)
            : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "得意先";
            FileNameToken = DataTypeToken + "マスター登録手数料";

            // FieldNumber 1 会社コード
            CompanyIdField = new StandardIdToCodeFieldDefinition<CustomerFee, Company>(
                cd => cd.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };

            // FieldNumber 2 得意先コード
            CustomerIdField = new StandardIdToCodeFieldDefinition<CustomerFee, Customer>(
                cd => cd.CustomerId, c => c.Id, l => l.CustomerCode, c => c.Code)
            {
                FieldName = "得意先コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitCustomerId,
            };

            // FieldNumber 3 登録手数料
            FeeField = new NullableNumberFieldDefinition<CustomerFee, decimal>(cd => cd.Fee)
            {
                FieldName = "登録手数料",
                FieldNumber = 3,
                Required = true,
                Accept = VisitFee,
                Format = value => value.ToString(),
            };

            // FieldNumber 4 通貨コード
            CurrencyCodeField = new StandardIdToCodeFieldDefinition<CustomerFee, Currency>(
                cd => cd.CurrencyId, c => c.Id, l => l.CurrencyCode, c => c.Code)
            {
                FieldName = "通貨コード",
                FieldNumber = 4,
                Required = true,
                Accept = VisitCurrencyCode,
            };

            Fields.AddRange(new IFieldDefinition<CustomerFee>[] {
                CompanyIdField
                ,CustomerIdField
                , FeeField
                ,CurrencyCodeField
            });
            if (applicationControl.UseForeignCurrency == 1)
            {
                KeyFields.AddRange(new IFieldDefinition<CustomerFee>[]
                {
                    CustomerIdField,
                    CurrencyCodeField,
                    FeeField,
                });
            }
            else
            {
                KeyFields.AddRange(new IFieldDefinition<CustomerFee>[]
                {
                    CustomerIdField,
                    FeeField,
                });

            }
            foreignflg = applicationControl.UseForeignCurrency;
        }

        private bool VisitCompanyId(IFieldVisitor<CustomerFee> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitCustomerId(IFieldVisitor<CustomerFee> visitor)
        {
            return visitor.CustomerCodeForDiscount(CustomerIdField);
        }

        private bool VisitFee(IFieldVisitor<CustomerFee> visitor)
        {
            return visitor.Fee(FeeField, foreignflg);
        }

        private bool VisitCurrencyCode(IFieldVisitor<CustomerFee> visitor)
        {
            return visitor.CurrencyCodeForFee(CurrencyCodeField, foreignflg);
        }
    }
}
