using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Rac.VOne.Web.Models.Files
{
    public class BillingInvoiceFileDifinition : RowDefinition<BillingInvoice>
    {
        public NumberFieldDefinition<BillingInvoice, int> CheckedField { get; private set; }
        public StringFieldDefinition<BillingInvoice> InvoiceTemplateCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoice> InvoiceCodeField { get; private set; }
        public NumberFieldDefinition<BillingInvoice, int> DetailsCountField { get; private set; }
        public StringFieldDefinition<BillingInvoice> CustomerCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoice> CustomerNameField { get; private set; }
        public NumberFieldDefinition<BillingInvoice, decimal> AmountSumField { get; private set; }
        public NumberFieldDefinition<BillingInvoice, decimal> RemainAmountSumField { get; private set; }
        public StringFieldDefinition<BillingInvoice> CollectCategoryCodeAndNemeField { get; private set; }
        public NumberFieldDefinition<BillingInvoice, DateTime> ClosingAtField { get; private set; }
        public NumberFieldDefinition<BillingInvoice, DateTime> BilledAtField { get; private set; }
        public StringFieldDefinition<BillingInvoice> DepartmentCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoice> DepartmentNameField { get; private set; }
        public StringFieldDefinition<BillingInvoice> StaffCodeField { get; private set; }
        public StringFieldDefinition<BillingInvoice> StaffNameField { get; private set; }
        public StringFieldDefinition<BillingInvoice> DestnationCodeField { get; private set; }
        public NumberFieldDefinition<BillingInvoice, DateTime> PublishAtField { get; private set; }
        public NumberFieldDefinition<BillingInvoice, DateTime> PublishAt1stField { get; private set; }

        public BillingInvoiceFileDifinition(DataExpression expression, List<GridSetting> GridSettingInfo) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "請求書データ";
            FileNameToken = DataTypeToken;
            OutputHeader = true;
            Fields.AddRange(InitializeFields(GridSettingInfo));
            }

        private IEnumerable<IFieldDefinition<BillingInvoice>> InitializeFields(List<GridSetting> GridSettingInfo)
        {

            var index = 0;
            foreach (var setting in GridSettingInfo.Where(x => x.DisplayWidth > 0))
            {
                index++;

                if (setting.ColumnName == nameof(BillingInvoice.Checked))
                    yield return (CheckedField = new NumberFieldDefinition<BillingInvoice, int>(k => k.Checked,
                setting.ColumnNameJp, index, accept: x => x.StandardNumber(CheckedField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoice.InvoiceTemplateCode))
                    yield return (InvoiceTemplateCodeField = new StringFieldDefinition<BillingInvoice>(k => k.InvoiceTemplateCode,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(InvoiceTemplateCodeField)));
                if (setting.ColumnName == nameof(BillingInvoice.InvoiceCode))
                    yield return (InvoiceCodeField = new StringFieldDefinition<BillingInvoice>(k => k.InvoiceCode,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(InvoiceCodeField)));
                if (setting.ColumnName == nameof(BillingInvoice.DetailsCount))
                    yield return (DetailsCountField = new NumberFieldDefinition<BillingInvoice, int>(k => k.DetailsCount,
                    setting.ColumnNameJp, index, accept: x => x.StandardNumber(DetailsCountField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoice.CustomerCode))
                    yield return (CustomerCodeField = new StringFieldDefinition<BillingInvoice>(k => k.CustomerCode,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(CustomerCodeField)));
                if (setting.ColumnName == nameof(BillingInvoice.CustomerName))
                    yield return (CustomerNameField = new StringFieldDefinition<BillingInvoice>(k => k.CustomerName,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(CustomerNameField)));
                if (setting.ColumnName == nameof(BillingInvoice.AmountSum))
                    yield return (AmountSumField = new NumberFieldDefinition<BillingInvoice, decimal>(k => k.AmountSum,
                    setting.ColumnNameJp, index, accept: x => x.StandardNumber(AmountSumField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoice.RemainAmountSum))
                    yield return (RemainAmountSumField = new NumberFieldDefinition<BillingInvoice, decimal>(k => k.RemainAmountSum,
                    setting.ColumnNameJp, index, accept: x => x.StandardNumber(RemainAmountSumField), formatter: value => value.ToString()));
                if (setting.ColumnName == nameof(BillingInvoice.CollectCategoryCodeAndNeme))
                    yield return (CollectCategoryCodeAndNemeField = new StringFieldDefinition<BillingInvoice>(k => k.CollectCategoryCodeAndNeme,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(CollectCategoryCodeAndNemeField)));
                if (setting.ColumnName == nameof(BillingInvoice.ClosingAt))
                    yield return (ClosingAtField = new NumberFieldDefinition<BillingInvoice, DateTime>(k => k.ClosingAt,
                    setting.ColumnNameJp, index, accept: x => x.StandardNumber(ClosingAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));
                if (setting.ColumnName == nameof(BillingInvoice.BilledAt))
                    yield return (BilledAtField = new NumberFieldDefinition<BillingInvoice, DateTime>(k => k.BilledAt,
                    setting.ColumnNameJp, index, accept: x => x.StandardNumber(BilledAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));
                if (setting.ColumnName == nameof(BillingInvoice.DepartmentCode))
                    yield return (DepartmentCodeField = new StringFieldDefinition<BillingInvoice>(k => k.DepartmentCode,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(DepartmentCodeField)));
                if (setting.ColumnName == nameof(BillingInvoice.DepartmentName))
                    yield return (DepartmentNameField = new StringFieldDefinition<BillingInvoice>(k => k.DepartmentName,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(DepartmentNameField)));
                if (setting.ColumnName == nameof(BillingInvoice.StaffCode))
                    yield return (StaffCodeField = new StringFieldDefinition<BillingInvoice>(k => k.StaffCode,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(StaffCodeField)));
                if (setting.ColumnName == nameof(BillingInvoice.StaffName))
                    yield return (StaffNameField = new StringFieldDefinition<BillingInvoice>(k => k.StaffName,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(StaffNameField)));
                if (setting.ColumnName == nameof(BillingInvoice.DestnationCode))
                    yield return (DestnationCodeField = new StringFieldDefinition<BillingInvoice>(k => k.DestnationCode,
                    setting.ColumnNameJp, index, accept: x => x.StandardString(DestnationCodeField)));
                if (setting.ColumnName == nameof(BillingInvoice.PublishAt))
                    yield return (PublishAtField = new NumberFieldDefinition<BillingInvoice, DateTime>(k => k.PublishAt,
                    setting.ColumnNameJp, index, accept: x => x.StandardNumber(PublishAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));
                if (setting.ColumnName == nameof(BillingInvoice.PublishAt1st))
                    yield return (PublishAt1stField = new NumberFieldDefinition<BillingInvoice, DateTime>(k => k.PublishAt1st,
                    setting.ColumnNameJp, index, accept: x => x.StandardNumber(PublishAt1stField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));
            }
        }
        public IFieldDefinition<BillingInvoice> ConvertSettingToField(string column)
        {
            IFieldDefinition<BillingInvoice> field = null;
            if (column == nameof(BillingInvoice.Checked)) field = CheckedField;
            if (column == nameof(BillingInvoice.DetailsCount)) field = DetailsCountField;
            if (column == nameof(BillingInvoice.InvoiceTemplateCode)) field = InvoiceTemplateCodeField;
            if (column == nameof(BillingInvoice.InvoiceCode)) field = InvoiceCodeField;
            if (column == nameof(BillingInvoice.CustomerCode)) field = CustomerCodeField;
            if (column == nameof(BillingInvoice.CustomerName)) field = CustomerNameField;
            if (column == nameof(BillingInvoice.AmountSum)) field = AmountSumField;
            if (column == nameof(BillingInvoice.RemainAmountSum)) field = RemainAmountSumField;
            if (column == nameof(BillingInvoice.CollectCategoryCodeAndNeme)) field = CollectCategoryCodeAndNemeField;
            if (column == nameof(BillingInvoice.ClosingAt)) field = ClosingAtField;
            if (column == nameof(BillingInvoice.BilledAt)) field = BilledAtField;
            if (column == nameof(BillingInvoice.DepartmentCode)) field = DepartmentCodeField;
            if (column == nameof(BillingInvoice.DepartmentName)) field = DepartmentNameField;
            if (column == nameof(BillingInvoice.StaffCode)) field = StaffCodeField;
            if (column == nameof(BillingInvoice.StaffName)) field = StaffNameField;
            if (column == nameof(BillingInvoice.DestnationCode)) field = DestnationCodeField;
            if (column == nameof(BillingInvoice.PublishAt)) field = PublishAtField;
            if (column == nameof(BillingInvoice.PublishAt1st)) field = PublishAt1stField;
            return field;
        }
    }
}
