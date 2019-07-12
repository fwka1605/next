using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class CreditAgingListFileDefinition : RowDefinition<CreditAgingList>
    {
        public StringFieldDefinition<CreditAgingList> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<CreditAgingList> DepartmentNameField { get; private set; }
        public StringFieldDefinition<CreditAgingList> StaffCodeField { get; private set; }
        public StringFieldDefinition<CreditAgingList> StaffNameField { get; private set; }
        public StringFieldDefinition<CreditAgingList> CustomerCodeField { get; private set; }
        public StringFieldDefinition<CreditAgingList> CustomerNameField { get; private set; }
        public StringFieldDefinition<CreditAgingList> CollectCategoryField { get; private set;}
        public NumberFieldDefinition<CreditAgingList, decimal> CreditAmountField { get; private set; }
        public NumberFieldDefinition<CreditAgingList, decimal> UnsettledRemainField { get; private set; }
        public NumberFieldDefinition<CreditAgingList, decimal> BillingRemainField { get; private set; }
        public NumberFieldDefinition<CreditAgingList, decimal> CreditLimitField { get; private set; }
        public NumberFieldDefinition<CreditAgingList, decimal> CreditRemainField { get; private set; }
        public NumberFieldDefinition<CreditAgingList, decimal> ArrivalDueDate1Field { get; private set; }
        public NumberFieldDefinition<CreditAgingList, decimal> ArrivalDueDate2Field { get; private set; }
        public NumberFieldDefinition<CreditAgingList, decimal> ArrivalDueDate3Field { get; private set; }
        public NumberFieldDefinition<CreditAgingList, decimal> ArrivalDueDate4Field { get; private set; }
        public StringFieldDefinition<CreditAgingList> CreditRemainStarField { get; set; }
        public StringFieldDefinition<BillingAgingListDetail> CurrencyCodeField { get; private set; }
        public CreditAgingListFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "債権総額管理表";
            FileNameToken = DataTypeToken;
            Fields.AddRange(InitializeFields());
        }

        private IEnumerable<IFieldDefinition<CreditAgingList>> InitializeFields()
        {
            yield return (DepartmentCodeField = new StringFieldDefinition<CreditAgingList>(x => x.DepartmentCode,
                "請求部門コード", 1, accept: x => x.StandardString(DepartmentCodeField)));
            yield return (DepartmentNameField = new StringFieldDefinition<CreditAgingList>(k => k.DepartmentName,
                "請求部門名", 2, accept: x => x.StandardString(DepartmentNameField)));
            yield return (StaffCodeField = new StringFieldDefinition<CreditAgingList>(k => k.StaffCode,
                "担当者コード", 3, accept: x => x.StandardString(StaffCodeField)));
            yield return (StaffNameField = new StringFieldDefinition<CreditAgingList>(k => k.StaffName,
                "担当者名", 4, accept: x => x.StandardString(StaffNameField)));
            yield return (CustomerCodeField = new StringFieldDefinition<CreditAgingList>(x => x.CustomerCode,
                "得意先コード", 5, accept: x => x.StandardString(CustomerCodeField)));
            yield return (CustomerNameField = new StringFieldDefinition<CreditAgingList>(x => x.CustomerName,
                "得意先名", 6, accept: x => x.StandardString(CustomerNameField)));
            yield return (CollectCategoryField = new StringFieldDefinition<CreditAgingList>(k => k.TotalText,
                "回収条件", 7, accept: x => x.StandardString(CollectCategoryField)));
            yield return (CreditAmountField = new NumberFieldDefinition<CreditAgingList, decimal>(k => k.CreditAmount,
                "当月債権総額", 8, accept: x => x.StandardNumber(CreditAmountField), formatter: x => x.ToString()));
            yield return (UnsettledRemainField = new NumberFieldDefinition<CreditAgingList, decimal>(k => k.UnsettledRemain,
                "当月末未決済残高", 9, accept: x => x.StandardNumber(UnsettledRemainField), formatter: x => x.ToString()));
            yield return (BillingRemainField = new NumberFieldDefinition<CreditAgingList, decimal>(k => k.BillingRemain,
                "当月請求残高", 10, accept: x => x.StandardNumber(BillingRemainField), formatter: x => x.ToString()));
            yield return (CreditLimitField = new NullableNumberFieldDefinition<CreditAgingList, decimal>(k => k.CreditLimit,
                "与信限度額", 11, accept: x => x.StandardNumber(CreditLimitField), formatter: x => x.ToString()));
            yield return (CreditRemainField = new NullableNumberFieldDefinition<CreditAgingList, decimal>(k => k.CreditBalance,
                "当月与信残高",  12, accept: x => x.StandardNumber(CreditRemainField), formatter: x => x.ToString()));
            yield return (CreditRemainStarField = new StringFieldDefinition<CreditAgingList>(k => k.CreditBalanceMark,
                " ", 13, accept: x => x.StandardString(CreditRemainStarField)));
            yield return (ArrivalDueDate1Field = new NumberFieldDefinition<CreditAgingList, decimal>(k => k.ArrivalDueDate1,
                "", 14, accept: x => x.StandardNumber(ArrivalDueDate1Field), formatter: x => x.ToString()));
            yield return (ArrivalDueDate2Field = new NumberFieldDefinition<CreditAgingList, decimal>(k => k.ArrivalDueDate2,
                "", 15, accept: x => x.StandardNumber(ArrivalDueDate2Field), formatter: x => x.ToString()));
            yield return (ArrivalDueDate3Field = new NumberFieldDefinition<CreditAgingList, decimal>(k => k.ArrivalDueDate3,
                "", 16, accept: x => x.StandardNumber(ArrivalDueDate3Field), formatter: x => x.ToString()));
            yield return (ArrivalDueDate4Field = new NumberFieldDefinition<CreditAgingList, decimal>(k => k.ArrivalDueDate4,
                "", 17, accept: x => x.StandardNumber(ArrivalDueDate4Field), formatter: x => x.ToString()));
        }

    }
}
