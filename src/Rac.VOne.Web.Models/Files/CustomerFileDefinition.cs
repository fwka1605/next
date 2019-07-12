using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class CustomerFileDefinition : RowDefinition<Customer>
    {
        public StandardIdToCodeFieldDefinition<Customer, Company> CompanyIdField { get; private set; }//1
        public StringFieldDefinition<Customer> CustomerCodeField { get; private set; }//2
        public StringFieldDefinition<Customer> CustomerNameField { get; private set; }//3
        public StringFieldDefinition<Customer> CustomerKanaField { get; private set; }//4
        public StringFieldDefinition<Customer> ExclusiveBankCodeField { get; private set; }//5
        public StringFieldDefinition<Customer> ExclusiveBankNameField { get; private set; }//6
        public StringFieldDefinition<Customer> ExclusiveBranchCodeField { get; private set; }//7
        public StringFieldDefinition<Customer> ExclusiveBranchNameField { get; private set; }//8
        public StringFieldDefinition<Customer> ExclusiveAccountNumberField { get; private set; }//9
        public NullableNumberFieldDefinition<Customer, int> ExclusiveAccountTypeIdField { get; private set; }

        public NumberFieldDefinition<Customer, int> ShareTransferFeeField { get; private set; }
        public NumberFieldDefinition<Customer, decimal> CreditLimitField { get; private set; }
        public NumberFieldDefinition<Customer, int> ClosingDayField { get; private set; }
        public StandardIdToCodeFieldDefinition<Customer, Category> CollectCategoryIdField { get; private set; }
        public NumberFieldDefinition<Customer, int> CollectOffsetMonthField { get; private set; }
        public NumberFieldDefinition<Customer, int> CollectOffsetDayField { get; private set; }//16
        public StandardIdToCodeFieldDefinition<Customer, Staff> StaffCodeField { get; private set; }
        public NumberFieldDefinition<Customer, int> IsParentField { get; private set; }
        public StringFieldDefinition<Customer> PostalCodeField { get; private set; }
        public StringFieldDefinition<Customer> Address1Field { get; private set; }

        public StringFieldDefinition<Customer> Address2Field { get; private set; }
        public StringFieldDefinition<Customer> TelField { get; private set; }
        public StringFieldDefinition<Customer> FaxField { get; private set; }
        public StringFieldDefinition<Customer> CustomerStaffNameField { get; private set; }
        public StringFieldDefinition<Customer> NoteField { get; private set; }
        public NumberFieldDefinition<Customer, int> UseFeeLearningField { get; private set; }
        public NullableNumberFieldDefinition<Customer, int> SightOfBillField { get; private set; }
        public StringFieldDefinition<Customer> DensaiCodeField { get; private set; }
        public StringFieldDefinition<Customer> CreditCodeField { get; private set; }
        public StringFieldDefinition<Customer> CreditRankField { get; private set; }

        public StringFieldDefinition<Customer> TransferBankCodeField { get; private set; }
        public StringFieldDefinition<Customer> TransferBankNameField { get; private set; }
        public StringFieldDefinition<Customer> TransferBranchCodeField { get; private set; }
        public StringFieldDefinition<Customer> TransferBranchNameField { get; private set; }
        public StringFieldDefinition<Customer> TransferAccountNumberField { get; private set; }
        public NullableNumberFieldDefinition<Customer, int> TransferAccountTypeIdField { get; private set; }//36
        public StringFieldDefinition<Customer> TransferCustomerCodeField { get; private set; }
        public StringFieldDefinition<Customer> TransferNewCodeField { get; private set; }
        public StringFieldDefinition<Customer> TransferAccountNameField { get; private set; }

        public NumberFieldDefinition<Customer, decimal> ThresholdValueField { get; private set; }
        public StandardIdToCodeFieldDefinition<Customer, Category> LessThanCollectCategoryIdField { get; private set; }
        public StandardIdToCodeFieldDefinition<Customer, Category> GreaterThanCollectCategoryId1Field { get; private set; }
        public NullableNumberFieldDefinition<Customer, decimal> GreaterThanRate1Field { get; private set; }

        public NullableNumberFieldDefinition<Customer, int> GreaterThanRoundingMode1Field { get; private set; }
        public NullableNumberFieldDefinition<Customer, int> GreaterThanSightOfBill1Field { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<Customer, Category> GreaterThanCollectCategoryId2Field { get; private set; }
        public NullableNumberFieldDefinition<Customer, decimal> GreaterThanRate2Field { get; private set; }
        public NullableNumberFieldDefinition<Customer, int> GreaterThanRoundingMode2Field { get; private set; }
        public NullableNumberFieldDefinition<Customer, int> GreaterThanSightOfBill2Field { get; private set; }
        public StandardNullableIdToCodeFieldDefinition<Customer, Category> GreaterThanCollectCategoryId3Field { get; private set; }
        public NullableNumberFieldDefinition<Customer, decimal> GreaterThanRate3Field { get; private set; }
        public NullableNumberFieldDefinition<Customer, int> GreaterThanRoundingMode3Field { get; private set; }
        public NullableNumberFieldDefinition<Customer, int> GreaterThanSightOfBill3Field { get; private set; }

        public NumberFieldDefinition<Customer, int> UseKanaLearningField { get; private set; }
        public NumberFieldDefinition<Customer, int> HolidayFlagField { get; private set; }
        public NumberFieldDefinition<Customer, int> UseFeeToleranceField { get; private set; }
        public NumberFieldDefinition<Customer, int> PrioritizeMatchingIndividuallyField { get; private set; }
        public StringFieldDefinition<Customer> CollationKeyField { get; private set; }
        public NumberFieldDefinition<Customer, int> ExcludeInvoicePublishField { get; private set; }
        public NumberFieldDefinition<Customer, int> ExcludeReminderPublishField { get; private set; }//61
        public StringFieldDefinition<Customer> DestinationDepartmentNameField { get; private set; }
        public StringFieldDefinition<Customer> HonorificField { get; private set; }//63

        public CustomerFileDefinition(DataExpression expression) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "得意先";
            FileNameToken = DataTypeToken + "マスター";

            //1
            CompanyIdField = new StandardIdToCodeFieldDefinition<Customer, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };

            //2
            CustomerCodeField = new StringFieldDefinition<Customer>(k => k.Code)
            {
                FieldName = "得意先コード",
                FieldNumber = 2,
                Required = true,
                Accept = VisitCustomerCode,
            };

            //3
            CustomerNameField = new StringFieldDefinition<Customer>(k => k.Name)
            {
                FieldName = "得意先名",
                FieldNumber = 3,
                Required = true,
                Accept = VisitCustomerName,
            };

            //4
            CustomerKanaField = new StringFieldDefinition<Customer>(k => k.Kana)
            {
                FieldName = "得意先名カナ",
                FieldNumber = 4,
                Required = true,
                Accept = VisitCustomerKana,
            };

            //5
            ExclusiveBankCodeField = new StringFieldDefinition<Customer>(k => k.ExclusiveBankCode)
            {
                FieldName = "専用銀行コード",
                FieldNumber = 5,
                Required = false,
                Accept = VisitExclusiveBankCode,
            };

            //6
            ExclusiveBankNameField = new StringFieldDefinition<Customer>(k => k.ExclusiveBankName)
            {
                FieldName = "専用銀行名",
                FieldNumber = 6,
                Required = false,
                Accept = VisitExclusiveBankName,
            };

            //7
            ExclusiveBranchCodeField = new StringFieldDefinition<Customer>(k => k.ExclusiveBranchCode)
            {
                FieldName = "専用支店コード",
                FieldNumber = 7,
                Required = false,
                Accept = VisitExclusiveBranchCode,
            };

            //8
            ExclusiveBranchNameField = new StringFieldDefinition<Customer>(k => k.ExclusiveBranchName)
            {
                FieldName = "専用支店名",
                FieldNumber = 8,
                Required = false,
                Accept = VisitExclusiveBranchName,
            };

            //9
            ExclusiveAccountNumberField = new StringFieldDefinition<Customer>(k => k.ExclusiveAccountNumber)
            {
                FieldName = "専用入金口座番号",
                FieldNumber = 9,
                Required = false,
                Accept = VisitExclusiveAccountNumber,
            };

            //10
            ExclusiveAccountTypeIdField = new NullableNumberFieldDefinition<Customer, int>(k => k.ExclusiveAccountTypeId)
            {
                FieldName = "預金種別",
                FieldNumber = 10,
                Required = true,
                Accept = VisitExclusiveAccountTypeId,
                Format = value => value.ToString(),
            };

            //11
            ShareTransferFeeField = new NumberFieldDefinition<Customer, int>(k => k.ShareTransferFee)
            {
                FieldName = "手数料負担区分",
                FieldNumber = 11,
                Required = true,
                Accept = VisitShareTransferFee,
                Format = value => value.ToString(),
            };

            //12
            CreditLimitField = new NumberFieldDefinition<Customer, decimal>(k => k.CreditLimit)
            {
                FieldName = "与信限度額",
                FieldNumber = 12,
                Required = false,
                Accept = VisitCreditLimit,
                Format = value => value.ToString("#"),
            };

            //13
            ClosingDayField = new NumberFieldDefinition<Customer, int>(k => k.ClosingDay)
            {
                FieldName = "締日",
                FieldNumber = 13,
                Required = true,
                Accept = VisitClosingDay,
                Format = value => value.ToString("00"),
            };

            //14
            CollectCategoryIdField = new StandardIdToCodeFieldDefinition<Customer, Category>(
                    k => k.CollectCategoryId, c => c.Id, null, c => c.Code)
            {
                FieldName = "回収方法",
                FieldNumber = 14,
                Required = true,
                Accept = VisitCollectCategoryId,

            };

            //15
            CollectOffsetMonthField = new NumberFieldDefinition<Customer, int>(k => k.CollectOffsetMonth)
            {
                FieldName = "回収予定（月）",
                FieldNumber = 15,
                Required = true,
                Accept = VisitCollectOffsetMonth,
                Format = value => value.ToString(),
            };

            //16
            CollectOffsetDayField = new NumberFieldDefinition<Customer, int>(k => k.CollectOffsetDay)
            {
                FieldName = "回収予定（日）",
                FieldNumber = 16,
                Required = true,
                Accept = VisitCollectOffsetDay,
                Format = value => value.ToString(),
            };

            //17
            StaffCodeField = new StandardIdToCodeFieldDefinition<Customer, Staff>(
                    k => k.StaffId, c => c.Id, l => l.StaffCode, c => c.Code)
            {
                FieldName = "営業担当者",
                FieldNumber = 17,
                Required = true,
                Accept = VisitStaffCodeField,
            };

            //18
            IsParentField = new NumberFieldDefinition<Customer, int>(k => k.IsParent)
            {
                FieldName = "債権代表者フラグ",
                FieldNumber = 18,
                Required = true,
                Accept = VisitIsParent,
                Format = value => value.ToString(),
            };

            //19
            PostalCodeField = new StringFieldDefinition<Customer>(k => k.PostalCode)
            {
                FieldName = "郵便番号",
                FieldNumber = 19,
                Required = false,
                Accept = VisitPostalCode,
            };
            //20
            Address1Field = new StringFieldDefinition<Customer>(k => k.Address1)
            {
                FieldName = "住所1",
                FieldNumber = 20,
                Required = false,
                Accept = VisitAddress1Field,
            };
            //21
            Address2Field = new StringFieldDefinition<Customer>(k => k.Address2)
            {
                FieldName = "住所2",
                FieldNumber = 21,
                Required = false,
                Accept = VisitAddress2Field,
            };

            //22
            TelField = new StringFieldDefinition<Customer>(k => k.Tel)
            {
                FieldName = "TEL番号",
                FieldNumber = 22,
                Required = false,
                Accept = VisitTel,
            };

            //23
            FaxField = new StringFieldDefinition<Customer>(k => k.Fax)
            {
                FieldName = "FAX番号",
                FieldNumber = 23,
                Required = false,
                Accept = VisitFax,
            };

            //24
            CustomerStaffNameField = new StringFieldDefinition<Customer>(k => k.CustomerStaffName)
            {
                FieldName = "相手先担当者名",
                FieldNumber = 24,
                Required = false,
                Accept = VisitCustomerStaffName,
            };

            //25
            NoteField = new StringFieldDefinition<Customer>(k => k.Note)
            {
                FieldName = "備考",
                FieldNumber = 25,
                Required = false,
                Accept = VisitNote,
            };

            //26
            UseFeeLearningField = new NumberFieldDefinition<Customer, int>(k => k.UseFeeLearning)
            {
                FieldName = "手数料自動学習",
                FieldNumber = 26,
                Required = true,
                Accept = VisitUseFeeLearning,
                Format = value => value.ToString(),
            };

            //27
            SightOfBillField = new NullableNumberFieldDefinition<Customer, int>(k => k.SightOfBill)
            {
                FieldName = "回収サイト",
                FieldNumber = 27,
                Required = true,
                Accept = VisitSightOfBill,
                Format = value => value.ToString(),
            };

            //28
            DensaiCodeField = new StringFieldDefinition<Customer>(k => k.DensaiCode)
            {
                FieldName = "電子手形用企業コード",
                FieldNumber = 28,
                Required = false,
                Accept = VisitDensaiCode,
            };

            //29
            CreditCodeField = new StringFieldDefinition<Customer>(k => k.CreditCode)
            {
                FieldName = "信用調査用企業コード",
                FieldNumber = 29,
                Required = false,
                Accept = VisitCreditCode,
            };

            //30
            CreditRankField = new StringFieldDefinition<Customer>(k => k.CreditRank)
            {
                FieldName = "与信ランク",
                FieldNumber = 30,
                Required = false,
                Accept = VisitCreditRank,
            };

            //31
            TransferBankCodeField = new StringFieldDefinition<Customer>(k => k.TransferBankCode)
            {
                FieldName = "口座振替用銀行コード",
                FieldNumber = 31,
                Required = false,
                Accept = VisitTransferBankCode,
            };

            //32
            TransferBankNameField = new StringFieldDefinition<Customer>(k => k.TransferBankName)
            {
                FieldName = "口座振替用銀行名",
                FieldNumber = 32,
                Required = false,
                Accept = VisitTransferBankName,
            };

            //33
            TransferBranchCodeField = new StringFieldDefinition<Customer>(k => k.TransferBranchCode)
            {
                FieldName = "口座振替用支店コード",
                FieldNumber = 33,
                Required = false,
                Accept = VisitTransferBranchCode,
            };

            //34
            TransferBranchNameField = new StringFieldDefinition<Customer>(k => k.TransferBranchName)
            {
                FieldName = "口座振替用支店名",
                FieldNumber = 34,
                Required = false,
                Accept = VisitTransferBranchName,
            };
            //35
            TransferAccountNumberField = new StringFieldDefinition<Customer>(k => k.TransferAccountNumber)
            {
                FieldName = "口座振替用口座番号",
                FieldNumber = 35,
                Required = false,
                Accept = VisitTransferAccountNumber,
            };

            //36
            TransferAccountTypeIdField = new NullableNumberFieldDefinition<Customer, int>(k => k.TransferAccountTypeId)
            {
                FieldName = "口座振替用預金種別",
                FieldNumber = 36,
                Required = true,
                Accept = VisitTransferAccountTypeId,
                Format = value => value.ToString(),
            };

            //37
            TransferCustomerCodeField = new StringFieldDefinition<Customer>(k => k.TransferCustomerCode)
            {
                FieldName = "口座振替用顧客コード",
                FieldNumber = 37,
                Required = false,
                Accept = VisitTransferCustomerCode,
            };

            //38
            TransferNewCodeField = new StringFieldDefinition<Customer>(k => k.TransferNewCode)
            {
                FieldName = "口座振替用新規コード",
                FieldNumber = 38,
                Required = false,
                Accept = VisitTransferNewCode,
            };

            //39
            TransferAccountNameField = new StringFieldDefinition<Customer>(k => k.TransferAccountName)
            {
                FieldName = "口座振替用預金者名",
                FieldNumber = 39,
                Required = false,
                Accept = VisitTransferAccountName,
            };

            //40
            ThresholdValueField = new NumberFieldDefinition<Customer, decimal>(k => k.ThresholdValue)
            {
                FieldName = "約定金額",
                FieldNumber = 40,
                Required = true,
                Accept = VisitThresholdValue,
                Format = value => value.ToString("#"),
            };

            //41
            LessThanCollectCategoryIdField = new StandardIdToCodeFieldDefinition<Customer, Category>(
                    k => k.LessThanCollectCategoryId, c => c.Id, null, c => c.Code)
            {
                FieldName = "約定金額未満",
                FieldNumber = 41,
                Required = true,
                Accept = VisitLessThanCollectCategoryId,
            };

            //42
            GreaterThanCollectCategoryId1Field = new StandardIdToCodeFieldDefinition<Customer, Category>(
                    k => k.GreaterThanCollectCategoryId1, c => c.Id, null, c => c.Code)
            {
                FieldName = "約定金額以上1",
                FieldNumber = 42,
                Required = true,
                Accept = VisitGreaterThanCollectCategoryId1,
            };

            //43
            GreaterThanRate1Field = new NullableNumberFieldDefinition<Customer, decimal>(k => k.GreaterThanRate1)
            {
                FieldName = "分割1",
                FieldNumber = 43,
                Required = true,
                Accept = VisitGreaterThanRate1,
                Format = value => value.ToString(),
            };

            //44
            GreaterThanRoundingMode1Field = new NullableNumberFieldDefinition<Customer, int>(k => k.GreaterThanRoundingMode1)
            {
                FieldName = "端数1",
                FieldNumber = 44,
                Required = true,
                Accept = VisitGreaterThanRoundingMode1,
                Format = value => value.ToString(),
            };

            //45
            GreaterThanSightOfBill1Field = new NullableNumberFieldDefinition<Customer, int>(k => k.GreaterThanSightOfBill1)
            {
                FieldName = "回収サイト1",
                FieldNumber = 45,
                Required = true,
                Accept = VisitGreaterThanSightOfBill1,
                Format = value => value.ToString(),
            };

            //46
            GreaterThanCollectCategoryId2Field = new StandardNullableIdToCodeFieldDefinition<Customer, Category>(
                    k => k.GreaterThanCollectCategoryId2, c => c.Id, null, c => c.Code)
            {
                FieldName = "約定金額以上2",
                FieldNumber = 46,
                Required = false,
                Accept = VisitGreaterThanCollectCategoryId2,
            };

            //47
            GreaterThanRate2Field = new NullableNumberFieldDefinition<Customer, decimal>(k => k.GreaterThanRate2)
            {
                FieldName = "分割2",
                FieldNumber = 47,
                Required = true,
                Accept = VisitGreaterThanRate2,
                Format = value => value.ToString(),
            };

            //48
            GreaterThanRoundingMode2Field = new NullableNumberFieldDefinition<Customer, int>(k => k.GreaterThanRoundingMode2)
            {
                FieldName = "端数2",
                FieldNumber = 48,
                Required = true,
                Accept = VisitGreaterThanRoundingMode2,
                Format = value => value.ToString(),
            };

            //49
            GreaterThanSightOfBill2Field = new NullableNumberFieldDefinition<Customer, int>(k => k.GreaterThanSightOfBill2)
            {
                FieldName = "回収サイト2",
                FieldNumber = 49,
                Required = true,
                Accept = VisitGreaterThanSightOfBill2,
                Format = value => value.ToString(),
            };

            //50
            GreaterThanCollectCategoryId3Field = new StandardNullableIdToCodeFieldDefinition<Customer, Category>(
                    k => k.GreaterThanCollectCategoryId3, c => c.Id, null, c => c.Code)
            {
                FieldName = "約定金額以上3",
                FieldNumber = 50,
                Required = false,
                Accept = VisitGreaterThanCollectCategoryId3,
            };

            //51
            GreaterThanRate3Field = new NullableNumberFieldDefinition<Customer, decimal>(k => k.GreaterThanRate3)
            {
                FieldName = "分割3",
                FieldNumber = 51,
                Required = true,
                Accept = VisitGreaterThanRate3,
                Format = value => value.ToString(),
            };

            //52
            GreaterThanRoundingMode3Field = new NullableNumberFieldDefinition<Customer, int>(k => k.GreaterThanRoundingMode3)
            {
                FieldName = "端数3",
                FieldNumber = 52,
                Required = true,
                Accept = VisitGreaterThanRoundingMode3,
                Format = value => value.ToString(),
            };

            //53
            GreaterThanSightOfBill3Field = new NullableNumberFieldDefinition<Customer, int>(k => k.GreaterThanSightOfBill3)
            {
                FieldName = "回収サイト3",
                FieldNumber = 53,
                Required = true,
                Accept = VisitGreaterThanSightOfBill3,
                Format = value => value.ToString(),
            };

            //54
            UseKanaLearningField = new NumberFieldDefinition<Customer, int>(k => k.UseKanaLearning)
            {
                FieldName = "カナ自動学習",
                FieldNumber = 54,
                Required = true,
                Accept = VisitUseKanaLearning,
                Format = value => value.ToString(),
            };

            //55
            HolidayFlagField = new NumberFieldDefinition<Customer, int>(k => k.HolidayFlag)
            {
                FieldName = "休業日設定",
                FieldNumber = 55,
                Required = true,
                Accept = VisitHolidayFlag,
                Format = value => value.ToString(),
            };

            //56
            UseFeeToleranceField = new NumberFieldDefinition<Customer, int>(k => k.UseFeeTolerance)
            {
                FieldName = "手数料誤差利用",
                FieldNumber = 56,
                Required = true,
                Accept = VisitUseFeeTolerance,
                Format = value => value.ToString(),
            };

            //57
            PrioritizeMatchingIndividuallyField = new NumberFieldDefinition<Customer, int>(k => k.PrioritizeMatchingIndividually)
            {
                FieldName = "一括消込対象外",
                FieldNumber = 57,
                Required = true,
                Accept = VisitPrioritizeMatchingIndividually,
                Format = value => value.ToString(),
            };

            //58
            CollationKeyField = new StringFieldDefinition<Customer>(k => k.CollationKey)
            {
                FieldName = "照合番号",
                FieldNumber = 58,
                Required = false,
                Accept = VisitCollationKey,
            };

            //60
            ExcludeInvoicePublishField = new NumberFieldDefinition<Customer, int>(k => k.ExcludeInvoicePublish)
            {
                FieldName = "請求書発行対象外",
                FieldNumber = 60,
                Required = true,
                Accept = VisitExcludeInvoicePublish,
                Format = value => value.ToString(),
            };

            //61
            ExcludeReminderPublishField = new NumberFieldDefinition<Customer, int>(k => k.ExcludeReminderPublish)
            {
                FieldName = "督促状発行対象外",
                FieldNumber = 61,
                Required = true,
                Accept = VisitExcludeReminderPublish,
                Format = value => value.ToString(),
            };

            //62
            DestinationDepartmentNameField = new StringFieldDefinition<Customer>(k => k.DestinationDepartmentName)
            {
                FieldName = "相手先部署",
                FieldNumber = 62,
                Required = false,
                Accept = VisitDestinationDepartmentName,
            };

            //63
            HonorificField = new StringFieldDefinition<Customer>(k => k.Honorific)
            {
                FieldName = "敬称",
                FieldNumber = 63,
                Required = false,
                Accept = VisitHonorific,
            };

            Fields.AddRange(new IFieldDefinition<Customer>[] {
                    CompanyIdField,CustomerCodeField,CustomerNameField,CustomerKanaField,ExclusiveBankCodeField,

                    ExclusiveBankNameField,ExclusiveBranchCodeField,ExclusiveBranchNameField,ExclusiveAccountNumberField,ExclusiveAccountTypeIdField

                   ,ShareTransferFeeField,CreditLimitField,ClosingDayField,CollectCategoryIdField,CollectOffsetMonthField

                   ,CollectOffsetDayField ,StaffCodeField,IsParentField,PostalCodeField ,Address1Field,

                   Address2Field,TelField,FaxField,CustomerStaffNameField,NoteField,

                   UseFeeLearningField ,SightOfBillField,DensaiCodeField,CreditCodeField,CreditRankField ,

                   TransferBankCodeField,TransferBankNameField,TransferBranchCodeField,TransferBranchNameField,TransferAccountNumberField,

                   TransferAccountTypeIdField,TransferCustomerCodeField,TransferNewCodeField,TransferAccountNameField,

                   ThresholdValueField,LessThanCollectCategoryIdField ,GreaterThanCollectCategoryId1Field,GreaterThanRate1Field,

                   GreaterThanRoundingMode1Field,GreaterThanSightOfBill1Field,GreaterThanCollectCategoryId2Field, GreaterThanRate2Field, GreaterThanRoundingMode2Field

                  ,GreaterThanSightOfBill2Field,GreaterThanCollectCategoryId3Field, GreaterThanRate3Field,GreaterThanRoundingMode3Field,GreaterThanSightOfBill3Field

                  ,UseKanaLearningField,HolidayFlagField,UseFeeToleranceField,PrioritizeMatchingIndividuallyField,CollationKeyField,ExcludeInvoicePublishField, ExcludeReminderPublishField

                  ,DestinationDepartmentNameField, HonorificField
            });

            KeyFields.AddRange(new IFieldDefinition<Customer>[]
            {
                CustomerCodeField,
            });
        }

        private bool VisitCompanyId(IFieldVisitor<Customer> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }

        private bool VisitCustomerCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }

        private bool VisitCustomerName(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }

        private bool VisitCustomerKana(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(CustomerKanaField);
        }

        private bool VisitExclusiveBankCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(ExclusiveBankCodeField);
        }

        private bool VisitExclusiveBankName(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(ExclusiveBankNameField);
        }

        private bool VisitExclusiveBranchCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(ExclusiveBranchCodeField);
        }

        private bool VisitExclusiveBranchName(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(ExclusiveBranchNameField);
        }

        private bool VisitExclusiveAccountNumber(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(ExclusiveAccountNumberField);
        }

        private bool VisitExclusiveAccountTypeId(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(ExclusiveAccountTypeIdField);
        }

        private bool VisitShareTransferFee(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(ShareTransferFeeField);
        }

        private bool VisitCreditLimit(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(CreditLimitField);
        }

        private bool VisitClosingDay(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(ClosingDayField);
        }

        private bool VisitCollectCategoryId(IFieldVisitor<Customer> visitor)
        {
            return visitor.CollectCategoryCode(CollectCategoryIdField);
        }

        private bool VisitCollectOffsetMonth(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(CollectOffsetMonthField);
        }

        private bool VisitCollectOffsetDay(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(CollectOffsetDayField);
        }

        private bool VisitStaffCodeField(IFieldVisitor<Customer> visitor)
        {
            return visitor.StaffCode(StaffCodeField);
        }

        private bool VisitIsParent(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(IsParentField);
        }

        private bool VisitPostalCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(PostalCodeField);
        }

        private bool VisitAddress1Field(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(Address1Field);
        }

        private bool VisitAddress2Field(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(Address2Field);
        }

        private bool VisitTel(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TelField);
        }

        private bool VisitFax(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(FaxField);
        }

        private bool VisitCustomerStaffName(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(CustomerStaffNameField);
        }

        private bool VisitNote(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(NoteField);
        }

        private bool VisitUseFeeLearning(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(UseFeeLearningField);
        }

        private bool VisitSightOfBill(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(SightOfBillField);
        }

        private bool VisitDensaiCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(DensaiCodeField);
        }

        private bool VisitCreditCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(CreditCodeField);
        }

        private bool VisitCreditRank(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(CreditRankField);
        }

        private bool VisitTransferBankCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TransferBankCodeField);
        }

        private bool VisitTransferBankName(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TransferBankNameField);
        }

        private bool VisitTransferBranchCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TransferBranchCodeField);
        }

        private bool VisitTransferBranchName(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TransferBranchNameField);
        }

        private bool VisitTransferAccountNumber(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TransferAccountNumberField);
        }

        private bool VisitTransferAccountTypeId(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(TransferAccountTypeIdField);
        }


        private bool VisitTransferCustomerCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TransferCustomerCodeField);
        }

        private bool VisitTransferNewCode(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TransferNewCodeField);
        }

        private bool VisitTransferAccountName(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(TransferAccountNameField);
        }


        private bool VisitThresholdValue(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(ThresholdValueField);
        }

        private bool VisitLessThanCollectCategoryId(IFieldVisitor<Customer> visitor)
        {
            return visitor.LessThanCollectCategoryId(LessThanCollectCategoryIdField);
        }

        private bool VisitGreaterThanCollectCategoryId1(IFieldVisitor<Customer> visitor)
        {
            return visitor.GreaterThanCollectCategoryId1(GreaterThanCollectCategoryId1Field);
        }

        private bool VisitGreaterThanRate1(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanRate1Field);
        }

        private bool VisitGreaterThanRoundingMode1(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanRoundingMode1Field);
        }

        private bool VisitGreaterThanSightOfBill1(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanSightOfBill1Field);
        }

        private bool VisitGreaterThanCollectCategoryId2(IFieldVisitor<Customer> visitor)
        {
            return visitor.GreaterThanCollectCategoryId2(GreaterThanCollectCategoryId2Field);
        }

        private bool VisitGreaterThanRate2(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanRate2Field);
        }

        private bool VisitGreaterThanRoundingMode2(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanRoundingMode2Field);
        }

        private bool VisitGreaterThanSightOfBill2(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanSightOfBill2Field);
        }

        private bool VisitGreaterThanCollectCategoryId3(IFieldVisitor<Customer> visitor)
        {
            return visitor.GreaterThanCollectCategoryId3(GreaterThanCollectCategoryId3Field);
        }

        private bool VisitGreaterThanRate3(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanRate3Field);
        }

        private bool VisitGreaterThanRoundingMode3(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanRoundingMode3Field);
        }

        private bool VisitGreaterThanSightOfBill3(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(GreaterThanSightOfBill3Field);
        }

        private bool VisitUseKanaLearning(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(UseKanaLearningField);
        }

        private bool VisitHolidayFlag(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(HolidayFlagField);
        }

        private bool VisitUseFeeTolerance(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(UseFeeToleranceField);
        }

        private bool VisitPrioritizeMatchingIndividually(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(PrioritizeMatchingIndividuallyField);
        }

        private bool VisitCollationKey(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(CollationKeyField);
        }
        private bool VisitExcludeInvoicePublish(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(ExcludeInvoicePublishField);
        }

        private bool VisitExcludeReminderPublish(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardNumber(ExcludeReminderPublishField);
        }

        private bool VisitDestinationDepartmentName(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(DestinationDepartmentNameField);
        }

        private bool VisitHonorific(IFieldVisitor<Customer> visitor)
        {
            return visitor.StandardString(HonorificField);
        }
    }
}
