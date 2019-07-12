using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class CustomerDiscountFileDefinition : RowDefinition<CustomerDiscount>
    {
        public StandardIdToCodeFieldDefinition<CustomerDiscount, Company> CompanyIdField { get; private set; }
        public StandardIdToCodeFieldDefinition<CustomerDiscount, Customer> CustomerIdField { get; private set; }
        public StringFieldDefinition<CustomerDiscount> CustomerNameField { get; private set; }
        public NumberFieldDefinition<CustomerDiscount, decimal> MinValueField { get; private set; }
        public NullableNumberFieldDefinition<CustomerDiscount, decimal> Rate1Field { get; private set; }
        public NullableNumberFieldDefinition<CustomerDiscount, decimal> Rate2Field { get; private set; }
        public NullableNumberFieldDefinition<CustomerDiscount, decimal> Rate3Field { get; private set; }
        public NullableNumberFieldDefinition<CustomerDiscount, decimal> Rate4Field { get; private set; }
        public NullableNumberFieldDefinition<CustomerDiscount, decimal> Rate5Field { get; private set; }
        public NumberFieldDefinition<CustomerDiscount, int> RoundingMode1Field { get; private set; }
        public NumberFieldDefinition<CustomerDiscount, int> RoundingMode2Field { get; private set; }
        public NumberFieldDefinition<CustomerDiscount, int> RoundingMode3Field { get; private set; }
        public NumberFieldDefinition<CustomerDiscount, int> RoundingMode4Field { get; private set; }
        public NumberFieldDefinition<CustomerDiscount, int> RoundingMode5Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department> DepartmentId1Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department> DepartmentId2Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department> DepartmentId3Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department> DepartmentId4Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department> DepartmentId5Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle> AccountTitleId1Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle> AccountTitleId2Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle> AccountTitleId3Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle> AccountTitleId4Field { get; private set; }
        public NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle> AccountTitleId5Field { get; private set; }
        public StringFieldDefinition<CustomerDiscount> SubCode1Field { get; private set; }
        public StringFieldDefinition<CustomerDiscount> SubCode2Field { get; private set; }
        public StringFieldDefinition<CustomerDiscount> SubCode3Field { get; private set; }
        public StringFieldDefinition<CustomerDiscount> SubCode4Field { get; private set; }
        public StringFieldDefinition<CustomerDiscount> SubCode5Field { get; private set; }

        public CustomerDiscountFileDefinition(DataExpression applicationControl)
            : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "得意先";
            FileNameToken = DataTypeToken + "マスター";

            // FieldNumber 1 会社コード
            CompanyIdField = new StandardIdToCodeFieldDefinition<CustomerDiscount, Company>(
                cd => cd.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };

            // FieldNumber 2 得意先コード
            CustomerIdField = new StandardIdToCodeFieldDefinition<CustomerDiscount, Customer>(
                cd => cd.CustomerId, c => c.Id, cd => cd.CustomerCode, c => c.Code)
            {
                FieldName = "得意先コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitCustomerId,
            };

            // FieldNumber 3 得意先名
            CustomerNameField = new StringFieldDefinition<CustomerDiscount>(cd => cd.CustomerName)
            {
                FieldName = "得意先名",
                FieldNumber = 3,
                Required = false,
                Accept = VisitCustomerName,
            };

            // FieldNumber 4 最低実行金額
            MinValueField = new NumberFieldDefinition<CustomerDiscount, decimal>(cd => cd.MinValue)
            {
                FieldName = "最低実行金額",
                FieldNumber = 4,
                Required = true,
                Accept = VisitMinValue,
                Format = value => value.ToString(),
            };

            // FieldNumber 5 歩引率1
            Rate1Field = new NullableNumberFieldDefinition<CustomerDiscount, decimal>(cd => cd.Rate1)
            {
                FieldName = "歩引率1",
                FieldNumber = 5,
                Required = false,
                Accept = VisitRate1,
                Format = value => value.ToString(),
            };

            // FieldNumber 6 端数処理1
            RoundingMode1Field = new NumberFieldDefinition<CustomerDiscount, int>(cd => cd.RoundingMode1)
            {
                FieldName = "端数処理1",
                FieldNumber = 6,
                Required = true,
                Accept = VisitRoundingMode1,
                Format = value => value.ToString(),
            };

            // FieldNumber 7 部門コード1
            DepartmentId1Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department>(
                cd => cd.DepartmentId1, d => d.Id, null, d => d.Code)
            {
                FieldName = "部門コード1",
                FieldNumber = 7,
                Required = false,
                Accept = VisitDepartmentId1,
            };

            // FieldNumber 8 科目コード1
            AccountTitleId1Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle>(
                cd => cd.AccountTitleId1, a => a.Id, null, a => a.Code)
            {
                FieldName = "科目コード1",
                FieldNumber = 8,
                Required = false,
                Accept = VisitAccountTitleId1,
            };

            // FieldNumber 9 補助コード1
            SubCode1Field = new StringFieldDefinition<CustomerDiscount>(cd => cd.SubCode1)
            {
                FieldName = "補助コード1",
                FieldNumber = 9,
                Required = false,
                Accept = VisitSubCode1,
            };

            // FieldNumber 10 歩引率2
            Rate2Field = new NullableNumberFieldDefinition<CustomerDiscount, decimal>(cd => cd.Rate2)
            {
                FieldName = "歩引率2",
                FieldNumber = 10,
                Required = false,
                Accept = VisitRate2,
                Format = value => value.ToString(),
            };

            // FieldNumber 11 端数処理2
            RoundingMode2Field = new NumberFieldDefinition<CustomerDiscount, int>(cd => cd.RoundingMode2)
            {
                FieldName = "端数処理2",
                FieldNumber = 11,
                Required = true,
                Accept = VisitRoundingMode2,
                Format = value => value.ToString(),
            };

            // FieldNumber 12 部門コード2
            DepartmentId2Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department>(
                cd => cd.DepartmentId2, d => d.Id, null, d => d.Code)
            {
                FieldName = "部門コード2",
                FieldNumber = 12,
                Required = false,
                Accept = VisitDepartmentId2,
            };

            // FieldNumber 13 科目コード2
            AccountTitleId2Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle>(
                cd => cd.AccountTitleId2, a => a.Id, null, a => a.Code)
            {
                FieldName = "科目コード2",
                FieldNumber = 13,
                Required = false,
                Accept = VisitAccountTitleId2,
            };

            // FieldNumber 14 補助コード2
            SubCode2Field = new StringFieldDefinition<CustomerDiscount>(cd => cd.SubCode2)
            {
                FieldName = "補助コード2",
                FieldNumber = 14,
                Required = false,
                Accept = VisitSubCode2,
            };

            // FieldNumber 15 歩引率3
            Rate3Field = new NullableNumberFieldDefinition<CustomerDiscount, decimal>(cd => cd.Rate3)
            {
                FieldName = "歩引率3",
                FieldNumber = 15,
                Required = false,
                Accept = VisitRate3,
                Format = value => value.ToString(),
            };

            // FieldNumber 16 端数処理3
            RoundingMode3Field = new NumberFieldDefinition<CustomerDiscount, int>(cd => cd.RoundingMode3)
            {
                FieldName = "端数処理3",
                FieldNumber = 16,
                Required = true,
                Accept = VisitRoundingMode3,
                Format = value => value.ToString(),
            };

            // FieldNumber 17 部門コード3
            DepartmentId3Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department>(
                cd => cd.DepartmentId3, d => d.Id, null, d => d.Code)
            {
                FieldName = "部門コード3",
                FieldNumber = 17,
                Required = false,
                Accept = VisitDepartmentId3,
            };

            // FieldNumber 18 科目コード3
            AccountTitleId3Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle>(
                cd => cd.AccountTitleId3, a => a.Id, null, a => a.Code)
            {
                FieldName = "科目コード3",
                FieldNumber = 18,
                Required = false,
                Accept = VisitAccountTitleId3,
            };

            // FieldNumber 19 補助コード3
            SubCode3Field = new StringFieldDefinition<CustomerDiscount>(cd => cd.SubCode3)
            {
                FieldName = "補助コード3",
                FieldNumber = 19,
                Required = false,
                Accept = VisitSubCode3,
            };

            // FieldNumber 20 歩引率4
            Rate4Field = new NullableNumberFieldDefinition<CustomerDiscount, decimal>(cd => cd.Rate4)
            {
                FieldName = "歩引率4",
                FieldNumber = 20,
                Required = false,
                Accept = VisitRate4,
                Format = value => value.ToString(),
            };

            // FieldNumber 21 端数処理4
            RoundingMode4Field = new NumberFieldDefinition<CustomerDiscount, int>(cd => cd.RoundingMode4)
            {
                FieldName = "端数処理4",
                FieldNumber = 21,
                Required = true,
                Accept = VisitRoundingMode4,
                Format = value => value.ToString(),
            };

            // FieldNumber 22 部門コード4
            DepartmentId4Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department>(
                cd => cd.DepartmentId4, d => d.Id, null, d => d.Code)
            {
                FieldName = "部門コード4",
                FieldNumber = 22,
                Required = false,
                Accept = VisitDepartmentId4,
            };

            // FieldNumber 23 科目コード4
            AccountTitleId4Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle>(
                cd => cd.AccountTitleId4, a => a.Id, null, a => a.Code)
            {
                FieldName = "科目コード4",
                FieldNumber = 23,
                Required = false,
                Accept = VisitAccountTitleId4,
            };

            // FieldNumber 24 補助コード4
            SubCode4Field = new StringFieldDefinition<CustomerDiscount>(cd => cd.SubCode4)
            {
                FieldName = "補助コード4",
                FieldNumber = 24,
                Required = false,
                Accept = VisitSubCode4,
            };

            // FieldNumber 25 歩引率5
            Rate5Field = new NullableNumberFieldDefinition<CustomerDiscount, decimal>(cd => cd.Rate5)
            {
                FieldName = "歩引率5",
                FieldNumber = 25,
                Required = false,
                Accept = VisitRate5,
                Format = value => value.ToString(),
            };

            // FieldNumber 26 端数処理5
            RoundingMode5Field = new NumberFieldDefinition<CustomerDiscount, int>(cd => cd.RoundingMode5)
            {
                FieldName = "端数処理5",
                FieldNumber = 26,
                Required = true,
                Accept = VisitRoundingMode5,
                Format = value => value.ToString(),
            };

            // FieldNumber 27 部門コード5
            DepartmentId5Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, Department>(
                cd => cd.DepartmentId5, d => d.Id, null, d => d.Code)
            {
                FieldName = "部門コード5",
                FieldNumber = 27,
                Required = false,
                Accept = VisitDepartmentId5,
            };

            // FieldNumber 28 科目コード5
            AccountTitleId5Field = new NullableForeignKeyFieldDefinition<CustomerDiscount, int, AccountTitle>(
                cd => cd.AccountTitleId5, a => a.Id, null, a => a.Code)
            {
                FieldName = "科目コード5",
                FieldNumber = 28,
                Required = false,
                Accept = VisitAccountTitleId5,
            };

            // FieldNumber 29 補助コード5
            SubCode5Field = new StringFieldDefinition<CustomerDiscount>(cd => cd.SubCode5)
            {
                FieldName = "補助コード5",
                FieldNumber = 29,
                Required = false,
                Accept = VisitSubCode5,
            };

            Fields.AddRange(new IFieldDefinition<CustomerDiscount>[] {
                CompanyIdField
                , CustomerIdField
                , CustomerNameField
                , MinValueField
                , Rate1Field
                , RoundingMode1Field
                , DepartmentId1Field
                , AccountTitleId1Field
                , SubCode1Field
                , Rate2Field
                , RoundingMode2Field
                , DepartmentId2Field
                , AccountTitleId2Field
                , SubCode2Field
                , Rate3Field
                , RoundingMode3Field
                , DepartmentId3Field
                , AccountTitleId3Field
                , SubCode3Field
                , Rate4Field
                , RoundingMode4Field
                , DepartmentId4Field
                , AccountTitleId4Field
                , SubCode4Field
                , Rate5Field
                , RoundingMode5Field
                , DepartmentId5Field
                , AccountTitleId5Field
                , SubCode5Field
            });
            KeyFields.AddRange(new IFieldDefinition<CustomerDiscount>[]
            {
                CustomerIdField,
            });
        }

        private bool VisitCompanyId(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitCustomerId(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.CustomerCodeForDiscount(CustomerIdField);
        }

        private bool VisitCustomerName(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }

        private bool VisitMinValue(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.MinValue(MinValueField);
        }

        private bool VisitRate1(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.Rate(Rate1Field);
        }

        private bool VisitRate2(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.Rate(Rate2Field);
        }

        private bool VisitRate3(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.Rate(Rate3Field);
        }

        private bool VisitRate4(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.Rate(Rate4Field);
        }

        private bool VisitRate5(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.Rate(Rate5Field);
        }

        private bool VisitRoundingMode1(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.RoundingMode(RoundingMode1Field);
        }

        private bool VisitRoundingMode2(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.RoundingMode(RoundingMode2Field);
        }

        private bool VisitRoundingMode3(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.RoundingMode(RoundingMode3Field);
        }

        private bool VisitRoundingMode4(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.RoundingMode(RoundingMode4Field);
        }

        private bool VisitRoundingMode5(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.RoundingMode(RoundingMode5Field);
        }

        private bool VisitDepartmentId1(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.DepartmentCode(DepartmentId1Field);
        }

        private bool VisitDepartmentId2(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.DepartmentCode(DepartmentId2Field);
        }

        private bool VisitDepartmentId3(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.DepartmentCode(DepartmentId3Field);
        }

        private bool VisitDepartmentId4(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.DepartmentCode(DepartmentId4Field);
        }

        private bool VisitDepartmentId5(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.DepartmentCode(DepartmentId5Field);
        }

        private bool VisitAccountTitleId1(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.AccountTitleCode(AccountTitleId1Field);
        }

        private bool VisitAccountTitleId2(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.AccountTitleCode(AccountTitleId2Field);
        }

        private bool VisitAccountTitleId3(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.AccountTitleCode(AccountTitleId3Field);
        }

        private bool VisitAccountTitleId4(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.AccountTitleCode(AccountTitleId4Field);
        }

        private bool VisitAccountTitleId5(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.AccountTitleCode(AccountTitleId5Field);
        }

        private bool VisitSubCode1(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.SubCode(SubCode1Field);
        }

        private bool VisitSubCode2(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.SubCode(SubCode2Field);
        }

        private bool VisitSubCode3(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.SubCode(SubCode3Field);
        }

        private bool VisitSubCode4(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.SubCode(SubCode4Field);
        }

        private bool VisitSubCode5(IFieldVisitor<CustomerDiscount> visitor)
        {
            return visitor.SubCode(SubCode5Field);
        }
    }
}
