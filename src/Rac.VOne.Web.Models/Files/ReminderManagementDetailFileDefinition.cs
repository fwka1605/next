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
    public class ReminderManagementDetailFileDifinition : RowDefinition<Reminder>
    {
        public StringFieldDefinition<Reminder> CustomerCodeField { get; private set; }
        public StringFieldDefinition<Reminder> CustomerNameField { get; private set; }
        public NumberFieldDefinition<Reminder, DateTime> CalculateBaseDateField { get; private set; }
        public NumberFieldDefinition<Reminder, int> DetailCountField { get; private set; }
        public StringFieldDefinition<Reminder> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<Reminder, decimal> RemainAmountField { get; private set; }
        public NullableNumberFieldDefinition<Reminder, decimal> ReminderAmountField { get; private set; }
        public NullableNumberFieldDefinition<Reminder, decimal> ArrearsInterestField { get; private set; }
        public NullableNumberFieldDefinition<Reminder, int> ArrearsDaysField { get; private set; }
        public StringFieldDefinition<Reminder> StatusField { get; private set; }
        public StringFieldDefinition<Reminder> MemoField { get; private set; }
        public NumberFieldDefinition<Reminder, DateTime> ClosingAtField { get; private set; }
        public StringFieldDefinition<Reminder> InvoiceCodeField { get; private set; }
        public StringFieldDefinition<Reminder> CollectCategoryField { get; private set; }
        public StringFieldDefinition<Reminder> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<Reminder> DepartmentNameField { get; private set; }
        public StringFieldDefinition<Reminder> StaffCodeField { get; private set; }
        public StringFieldDefinition<Reminder> StaffNameField { get; private set; }
        public StringFieldDefinition<Reminder> DestinationCodeField { get; private set; }
        public NumberFieldDefinition<Reminder, DateTime> BaseDateField { get; private set; }


        public ReminderManagementDetailFileDifinition(DataExpression applicationControl, string baseDateCaption)
            : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "督促データ一覧";
            FileNameToken = DataTypeToken;
            Fields.AddRange(InitializeFields(baseDateCaption));
        }

        private IEnumerable<IFieldDefinition<Reminder>> InitializeFields(string baseDateCaption)
        {
            yield return (BaseDateField = new NumberFieldDefinition<Reminder, DateTime>(k => k.BaseDate,
                "基準日", 1, accept: x => x.StandardNumber(BaseDateField), formatter: value => value.ToShortDateString()));
            yield return (CustomerCodeField = new StringFieldDefinition<Reminder>(k => k.CustomerCode,
                "得意先コード", 2, accept: x => x.StandardString(CustomerCodeField)));
            yield return (CustomerNameField = new StringFieldDefinition<Reminder>(k => k.CustomerName,
                "得意先名", 3, accept: x => x.StandardString(CustomerNameField)));
            yield return (CalculateBaseDateField = new NumberFieldDefinition<Reminder, DateTime>(k => k.CalculateBaseDate,
                baseDateCaption, 4, accept: x => x.StandardNumber(CalculateBaseDateField), formatter: value => value.ToShortDateString()));
            yield return (DetailCountField = new NumberFieldDefinition<Reminder, int>(k => k.DetailCount,
                "明細件数", 5, accept: x => x.StandardNumber(DetailCountField), formatter: value => value.ToString()));
            yield return (CurrencyCodeField = new StringFieldDefinition<Reminder>(k => k.CurrencyCode,
                "通貨コード", 6, accept: x => x.StandardString(CurrencyCodeField)));
            yield return (RemainAmountField = new NumberFieldDefinition<Reminder, decimal>(k => k.RemainAmount,
                "請求残", 7, accept: x => x.StandardNumber(RemainAmountField), formatter: value => value.ToString()));
            yield return (ReminderAmountField = new NullableNumberFieldDefinition<Reminder, decimal>(k => k.ReminderAmount,
                "滞留金額", 8, accept: x => x.StandardNumber(ReminderAmountField), formatter: value => value.ToString()));
            yield return (ArrearsInterestField = new NullableNumberFieldDefinition<Reminder, decimal>(k => k.ArrearsInterest,
                "延滞利息", 9, accept: x => x.StandardNumber(ArrearsInterestField), formatter: value => value.ToString()));
            yield return (ArrearsDaysField = new NullableNumberFieldDefinition<Reminder, int>(k => k.ArrearsDays,
                "滞留日数", 10, accept: x => x.StandardNumber(ArrearsDaysField), formatter: value => value.ToString()));
            yield return (StatusField = new StringFieldDefinition<Reminder>(k => k.StatusCodeAndName,
                "ステータス", 11, accept: x => x.StandardString(StatusField)));
            yield return (MemoField = new StringFieldDefinition<Reminder>(k => k.Memo,
                "対応記録", 12, accept: x => x.StandardString(MemoField)));
            yield return (ClosingAtField = new NumberFieldDefinition<Reminder, DateTime>(k => k.ClosingAt,
                "請求締日", 13, accept: x => x.StandardNumber(ClosingAtField), formatter: value => value.ToShortDateString()));
            yield return (InvoiceCodeField = new StringFieldDefinition<Reminder>(k => k.InvoiceCode,
                "請求書番号", 14, accept: x => x.StandardString(InvoiceCodeField)));
            yield return (CollectCategoryField = new StringFieldDefinition<Reminder>(k => k.CollectCategoryCodeAndName,
                "回収区分", 15, accept: x => x.StandardString(CollectCategoryField)));
            yield return (DepartmentCodeField = new StringFieldDefinition<Reminder>(k => k.DepartmentCode,
                "請求部門コード", 16, accept: x => x.StandardString(DepartmentCodeField)));
            yield return (DepartmentNameField = new StringFieldDefinition<Reminder>(k => k.DepartmentName,
                "請求部門名", 17, accept: x => x.StandardString(DepartmentNameField)));
            yield return (StaffCodeField = new StringFieldDefinition<Reminder>(k => k.StaffCode,
                "担当者コード", 18, accept: x => x.StandardString(StaffCodeField)));
            yield return (StaffNameField = new StringFieldDefinition<Reminder>(k => k.StaffName,
                "担当者名", 19, accept: x => x.StandardString(StaffNameField)));
            yield return (DestinationCodeField = new StringFieldDefinition<Reminder>(k => k.DestinationCode,
                "送付先コード", 20, accept: x => x.StandardString(DestinationCodeField)));
        }
    }
}
