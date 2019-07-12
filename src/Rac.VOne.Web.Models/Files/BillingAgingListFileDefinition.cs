using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class BillingAgingListFileDefinition : RowDefinition<BillingAgingList>
    {
        public StringFieldDefinition<BillingAgingList> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<BillingAgingList> DepartmentNameField { get; private set; }
        public StringFieldDefinition<BillingAgingList> StaffCodeField { get; private set; }
        public StringFieldDefinition<BillingAgingList> StaffNameField { get; private set; }
        public StringFieldDefinition<BillingAgingList> CustomerCodeField { get; private set; }
        public StringFieldDefinition<BillingAgingList> CustomerNameField { get; private set; }
        public NullableNumberFieldDefinition<BillingAgingList, decimal> LastMonthRemainField { get; private set; }
        public NumberFieldDefinition<BillingAgingList, decimal> CurrenctMonthSaleField { get; private set; }
        public NullableNumberFieldDefinition<BillingAgingList, decimal> CurrentMonthReceiptField { get; private set; }
        public NumberFieldDefinition<BillingAgingList, decimal> CurrentMonthMatchingField { get; private set; }
        public NullableNumberFieldDefinition<BillingAgingList, decimal> CurrenttMonthRemainField { get; private set; }
        public NullableNumberFieldDefinition<BillingAgingList, decimal> MonthlyRemain0Field { get; private set; }
        public NullableNumberFieldDefinition<BillingAgingList, decimal> MonthlyRemain1Field { get; private set; }
        public NullableNumberFieldDefinition<BillingAgingList, decimal> MonthlyRemain2Field { get; private set; }
        public NullableNumberFieldDefinition<BillingAgingList, decimal> MonthlyRemain3Field { get; private set; }
        public BillingAgingListFileDefinition(DataExpression expression,
            string Remain0Header,
            string Remain1Header,
            string Remain2Header,
            string Remain3Header)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "請求残高年齢表";
            FileNameToken = DataTypeToken;

            Fields.Add(DepartmentCodeField = new StringFieldDefinition<BillingAgingList>(k => k.DepartmentCode,
                fieldName: "請求部門コード",
                accept : VisitDepartmentCodeField));
            Fields.Add(DepartmentNameField = new StringFieldDefinition<BillingAgingList>(k => k.DepartmentName,
                fieldName : "請求部門名",
                accept : VisitDepartmentNameField));
            Fields.Add(StaffCodeField = new StringFieldDefinition<BillingAgingList>(k => k.StaffCode,
                fieldName : "担当者コード",
                accept : VisitStaffCodeField));
            Fields.Add(StaffNameField = new StringFieldDefinition<BillingAgingList>(k => k.StaffName,
                fieldName : "担当者名",
                accept : VisitStaffNameField));
            Fields.Add(CustomerCodeField = new StringFieldDefinition<BillingAgingList>(k => k.CustomerCode,
                fieldName : "得意先コード",
                accept : VisitCustomerCodeField));

            Fields.Add(CustomerNameField = new StringFieldDefinition<BillingAgingList>(k => k.CustomerName,
                fieldName : "得意先名",
                accept : VisitCustomerNameField));

            Fields.Add(LastMonthRemainField = new NullableNumberFieldDefinition<BillingAgingList, decimal>(k => k.LastMonthRemain,
                fieldName : "前月請求残",
                accept : VisitLastMonthRemainField,
                formatter : value => value.ToString()));

            Fields.Add(CurrenctMonthSaleField = new NumberFieldDefinition<BillingAgingList, decimal>(k => k.CurrentMonthSales,
                fieldName : "当月売上高",
                accept : VisitCurrentMonthSaleField,
                formatter : value => value.ToString()));

            Fields.Add(CurrentMonthReceiptField = new NullableNumberFieldDefinition<BillingAgingList, decimal>(k => k.CurrentMonthReceipt,
                fieldName : "当月入金",
                accept : VisitCurrentMonthReceiptField,
                formatter : value => value.ToString()));

            Fields.Add(CurrentMonthMatchingField = new NumberFieldDefinition<BillingAgingList, decimal>(k => k.CurrentMonthMatching,
                fieldName : "当月消込",
                accept : VisitCurrentMonthMatchingField,
                formatter : value => value.ToString()));

            Fields.Add(CurrenttMonthRemainField = new NullableNumberFieldDefinition<BillingAgingList, decimal>(k => k.CurrentMonthRemain,
                fieldName : "当月請求残",
                accept : VisitCurrentMonthRemainField,
                formatter : value => value.ToString()));

            Fields.Add(MonthlyRemain0Field = new NullableNumberFieldDefinition<BillingAgingList, decimal>(k => k.MonthlyRemain0,
                fieldName: Remain0Header,
                accept: VisitMonthlyRemain0Field,
                formatter: value => value.ToString()));

            Fields.Add(MonthlyRemain1Field = new NullableNumberFieldDefinition<BillingAgingList, decimal>(k => k.MonthlyRemain1,
                fieldName: Remain1Header,
                accept: VisitMonthlyRemain1Field,
                formatter: value => value.ToString()));

            Fields.Add(MonthlyRemain2Field = new NullableNumberFieldDefinition<BillingAgingList, decimal>(k => k.MonthlyRemain2,
                fieldName: Remain2Header,
                accept : VisitMonthlyRemain2Field,
                formatter: value => value.ToString()));

            Fields.Add(MonthlyRemain3Field = new NullableNumberFieldDefinition<BillingAgingList, decimal>(k => k.MonthlyRemain3,
                fieldName: Remain3Header,
                accept : VisitMonthlyRemain3Field,
                formatter: value => value.ToString()));

        }
        private bool VisitDepartmentCodeField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardString(DepartmentCodeField);
        }
        private bool VisitDepartmentNameField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardString(DepartmentNameField);
        }
        private bool VisitStaffCodeField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardString(StaffCodeField);
        }
        private bool VisitStaffNameField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardString(StaffNameField);
        }
        private bool VisitCustomerCodeField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardString(CustomerCodeField);
        }
        private bool VisitCustomerNameField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardString(CustomerNameField);
        }
        private bool VisitLastMonthRemainField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(LastMonthRemainField);
        }
        private bool VisitCurrentMonthSaleField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(CurrenctMonthSaleField);
        }
        private bool VisitCurrentMonthReceiptField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(CurrentMonthReceiptField);
        }
        private bool VisitCurrentMonthMatchingField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(CurrentMonthMatchingField);
        }
        private bool VisitCurrentMonthRemainField(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(CurrenttMonthRemainField);
        }
        private bool VisitMonthlyRemain0Field(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(MonthlyRemain0Field);
        }
        private bool VisitMonthlyRemain1Field(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(MonthlyRemain1Field);
        }
        private bool VisitMonthlyRemain2Field(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(MonthlyRemain2Field);
        }
        private bool VisitMonthlyRemain3Field(IFieldVisitor<BillingAgingList> visitor)
        {
            return visitor.StandardNumber(MonthlyRemain3Field);
        }
    }
}
