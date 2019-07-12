using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models.Files
{
    public class HolidayCalendarFileDefinition : RowDefinition<HolidayCalendar>
    {
        public StandardIdToCodeFieldDefinition<HolidayCalendar, Company> CompanyIdField { get; private set; }
        public NumberFieldDefinition<HolidayCalendar, DateTime> HolidayField { get; private set; }

        public HolidayCalendarFileDefinition(DataExpression expression)
            : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "カレンダー";
            FileNameToken = DataTypeToken + "マスター";

            CompanyIdField = new StandardIdToCodeFieldDefinition<HolidayCalendar, Company>(
                    k => k.CompanyId, c => c.Id, null, c => c.Code)
            {
                FieldName = "会社コード",
                FieldNumber = 1,
                Required = false,
                Accept = VisitCompanyId,
            };
            HolidayField = new NumberFieldDefinition<HolidayCalendar, DateTime>(k => k.Holiday)
            {
                FieldName = "休業日",
                FieldNumber = 2,
                Required = true,
                Accept = VisitHolidayField,
                Format = value => (value == DateTime.MinValue) ? "" : value.ToShortDateString(),
            };
            Fields.AddRange(new IFieldDefinition<HolidayCalendar>[] {
                       CompanyIdField,HolidayField});
            KeyFields.AddRange(new IFieldDefinition<HolidayCalendar>[]
            {
                HolidayField,
            });
        }
        private bool VisitCompanyId(IFieldVisitor<HolidayCalendar> visitor)
        {
            return visitor.OwnCompanyCode(CompanyIdField);
        }
        private bool VisitHolidayField(IFieldVisitor<HolidayCalendar> visitor)
        {
            return visitor.HolidayCalendar(HolidayField);
        }
    }
}
