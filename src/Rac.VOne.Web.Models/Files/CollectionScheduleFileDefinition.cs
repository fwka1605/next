using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;


namespace Rac.VOne.Web.Models.Files
{
    public class CollectionScheduleFileDefinition : RowDefinition<CollectionSchedule>
    {
        public StringFieldDefinition<CollectionSchedule> CustomerInfoField { get; private set; }
        public StringFieldDefinition<CollectionSchedule> ClosingDayField { get; private set; }
        public StringFieldDefinition<CollectionSchedule> StaffNameField { get; private set; }
        public StringFieldDefinition<CollectionSchedule> DepartmentNameField { get; private set; }
        public StringFieldDefinition<CollectionSchedule> CategoryNameField { get; private set; }
        public NumberFieldDefinition<CollectionSchedule, decimal> UncollectedAmountLastField { get; private set; }
        public NumberFieldDefinition<CollectionSchedule, decimal> UncollectedAmount0Field { get; private set; }
        public NumberFieldDefinition<CollectionSchedule, decimal> UncollectedAmount1Field { get; private set; }
        public NumberFieldDefinition<CollectionSchedule, decimal> UncollectedAmount2Field { get; private set; }
        public NumberFieldDefinition<CollectionSchedule, decimal> UncollectedAmount3Field { get; private set; }
        public NumberFieldDefinition<CollectionSchedule, decimal> UncollectedAmountTotalField { get; private set; }

        public CollectionScheduleFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "回収予定表";
            FileNameToken = DataTypeToken;

            Fields.Add(CustomerInfoField = new StringFieldDefinition<CollectionSchedule>(
                x => x.CustomerInfo, "得意先/回収条件", accept: VisitCustomerInfoField));

            Fields.Add(ClosingDayField = new StringFieldDefinition<CollectionSchedule>(
                k => k.Closing,"締日", accept: VisitClosingDayField));

            Fields.Add(StaffNameField = new StringFieldDefinition<CollectionSchedule>(
                k => k.StaffName, "担当者名", accept: VisitStaffNameField));

            Fields.Add(DepartmentNameField = new StringFieldDefinition<CollectionSchedule>(
                k => k.DepartmentName,"請求部門名",accept: VisitDepartmentNameField));

            Fields.Add(CategoryNameField = new StringFieldDefinition<CollectionSchedule>(
                k => k.CollectCategoryName,"区分", accept: VisitCategoryNameField));

            Fields.Add(UncollectedAmountLastField = new NullableNumberFieldDefinition<CollectionSchedule, decimal>(
                k => k.UncollectedAmountLast, accept: VisitUncollectedAmountLastField));

            Fields.Add(UncollectedAmount0Field = new NullableNumberFieldDefinition<CollectionSchedule, decimal>(
                k => k.UncollectedAmount0, accept: VisitUncollectedAmount0Field));

            Fields.Add(UncollectedAmount1Field = new NullableNumberFieldDefinition<CollectionSchedule, decimal>(
                k => k.UncollectedAmount1, accept: VisitUncollectedAmount1Field));

            Fields.Add(UncollectedAmount2Field = new NullableNumberFieldDefinition<CollectionSchedule, decimal>(
                k => k.UncollectedAmount2, accept: VisitUncollectedAmount2Field));

            Fields.Add(UncollectedAmount3Field = new NullableNumberFieldDefinition<CollectionSchedule, decimal>(
                k => k.UncollectedAmount3, accept: VisitUncollectedAmount3Field));

            Fields.Add(UncollectedAmountTotalField = new NullableNumberFieldDefinition<CollectionSchedule, decimal>(
                k => k.UncollectedAmountTotal, "合計", accept: VisitUncollectedAmountTotalField));
        }

        private bool VisitCustomerInfoField(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardString(CustomerInfoField);
        private bool VisitClosingDayField(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardString(ClosingDayField);
        private bool VisitDepartmentNameField(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardString(DepartmentNameField);
        private bool VisitStaffNameField(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardString(StaffNameField);
        private bool VisitCategoryNameField(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardString(CategoryNameField);
        private bool VisitUncollectedAmountLastField(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardNumber(UncollectedAmountLastField);
        private bool VisitUncollectedAmount0Field(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardNumber(UncollectedAmount0Field);
        private bool VisitUncollectedAmount1Field(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardNumber(UncollectedAmount1Field);
        private bool VisitUncollectedAmount2Field(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardNumber(UncollectedAmount2Field);
        private bool VisitUncollectedAmount3Field(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardNumber(UncollectedAmount3Field);
        private bool VisitUncollectedAmountTotalField(IFieldVisitor<CollectionSchedule> visitor)
            => visitor.StandardNumber(UncollectedAmountTotalField);

    }
}
