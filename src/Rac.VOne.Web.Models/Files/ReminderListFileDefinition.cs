using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.Files
{
    public class ReminderListFileDefinition : RowDefinition<ReminderHistory>
    {

       public StringFieldDefinition<ReminderHistory> CustomerCodeDisplayField { get; private set; }
       public StringFieldDefinition<ReminderHistory> CustomerNameDisplayField { get; private set; }
       public NullableNumberFieldDefinition<ReminderHistory, DateTime> CalculateBaseDateDisplayField { get; private set; }
       public StringFieldDefinition<ReminderHistory> CurrencyCodeField { get; private set; }
       public NullableNumberFieldDefinition<ReminderHistory, int> ArrearsDaysDisplayField { get; private set; }
       public NullableNumberFieldDefinition<ReminderHistory, decimal> ReminderAmountField { get; private set; }
       public StringFieldDefinition<ReminderHistory> InputTypeNameField { get; private set; }
       public StringFieldDefinition<ReminderHistory> StatusCodeAndNameField { get; private set; }
       public StringFieldDefinition<ReminderHistory> MemoField { get; private set; }
       public StringFieldDefinition<ReminderHistory> OutputFlagNameField { get; private set; }
       public NumberFieldDefinition<ReminderHistory, DateTime> CreateAtField { get; private set; }
       public StringFieldDefinition<ReminderHistory> CreateByNameField { get; private set; }

        public ReminderListFileDefinition(DataExpression expression, List<GridSetting> GridSettingInfo) : base(expression)
        {
            StartLineNumber = 1;
            DataTypeToken = "請求書データ";
            FileNameToken = DataTypeToken;
            OutputHeader = true;
            Fields.AddRange(InitializeFields(GridSettingInfo));
        }

        private IEnumerable<IFieldDefinition<ReminderHistory>> InitializeFields(List<GridSetting> GridSettingInfo)
        {

            var index = 0;
            foreach (var setting in GridSettingInfo.Where(x => x.DisplayWidth > 0))
            {
                index++;

               if (setting.ColumnName == nameof(ReminderHistory.CustomerCodeDisplay))
                    yield return (CustomerCodeDisplayField = new StringFieldDefinition<ReminderHistory>(k => k.CustomerCodeDisplay,
                        setting.ColumnNameJp, index, accept: x => x.StandardString(CustomerCodeDisplayField)));
               if (setting.ColumnName == nameof(ReminderHistory.CustomerNameDisplay))
                    yield return (CustomerNameDisplayField = new StringFieldDefinition<ReminderHistory>(k => k.CustomerNameDisplay,
                        setting.ColumnNameJp, index, accept: x => x.StandardString(CustomerNameDisplayField)));
               if (setting.ColumnName == nameof(ReminderHistory.CalculateBaseDateDisplay))
                    yield return (CalculateBaseDateDisplayField = new NullableNumberFieldDefinition<ReminderHistory, DateTime>(k => k.CalculateBaseDateDisplay,
                        setting.ColumnNameJp, index, accept: x => x.StandardNumber(CalculateBaseDateDisplayField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToShortDateString()));
               if (setting.ColumnName == nameof(ReminderHistory.CurrencyCode))
                    yield return (CurrencyCodeField = new StringFieldDefinition<ReminderHistory>(k => k.CurrencyCode,
                        setting.ColumnNameJp, index, accept: x => x.StandardString(CurrencyCodeField)));
                if (setting.ColumnName == nameof(ReminderHistory.ArrearsDaysDisplay))
                    yield return (ArrearsDaysDisplayField = new NullableNumberFieldDefinition<ReminderHistory, int>(k => k.ArrearsDaysDisplay,
                        setting.ColumnNameJp, index, accept: x => x.StandardNumber(ArrearsDaysDisplayField), formatter: value => (value.ToString())));
                if (setting.ColumnName == nameof(ReminderHistory.ReminderAmount))
                    yield return (ReminderAmountField = new NullableNumberFieldDefinition<ReminderHistory, decimal>(k => k.ReminderAmount,
                        setting.ColumnNameJp, index, accept: x => x.StandardNumber(ReminderAmountField)));
               if (setting.ColumnName == nameof(ReminderHistory.InputTypeName))
                    yield return (InputTypeNameField = new StringFieldDefinition<ReminderHistory>(k => k.InputTypeName
                    , setting.ColumnNameJp, index, accept: x => x.StandardString(InputTypeNameField)));
               if (setting.ColumnName == nameof(ReminderHistory.StatusCodeAndName))
                    yield return (StatusCodeAndNameField = new StringFieldDefinition<ReminderHistory>(k => k.StatusCodeAndName,
                        setting.ColumnNameJp, index, accept: x => x.StandardString(StatusCodeAndNameField)));
               if (setting.ColumnName == nameof(ReminderHistory.Memo))
                    yield return (MemoField = new StringFieldDefinition<ReminderHistory>(k => k.Memo,
                        setting.ColumnNameJp, index, accept: x => x.StandardString(MemoField)));
               if (setting.ColumnName == nameof(ReminderHistory.OutputFlagName))
                    yield return (OutputFlagNameField = new StringFieldDefinition<ReminderHistory>(k => k.OutputFlagName,
                        setting.ColumnNameJp, index, accept: x => x.StandardString(OutputFlagNameField)));
               if (setting.ColumnName == nameof(ReminderHistory.CreateAt))
                    yield return (CreateAtField = new NumberFieldDefinition<ReminderHistory, DateTime>(k => k.CreateAt,
                        setting.ColumnNameJp, index, accept: x => x.StandardNumber(CreateAtField), formatter: value => (value == DateTime.MinValue) ? "" : value.ToString("yyyy/MM/dd HH:mm:ss")));
                if (setting.ColumnName == nameof(ReminderHistory.CreateByName))
                    yield return (CreateByNameField = new StringFieldDefinition<ReminderHistory>(k => k.CreateByName,
                        setting.ColumnNameJp, index, accept: x => x.StandardString(CreateByNameField)));
            }
        }
        public IFieldDefinition<ReminderHistory> ConvertSettingToField(string column)
        {
            IFieldDefinition<ReminderHistory> field = null;
            if (column == nameof(ReminderHistory.CustomerCodeDisplay)) field = CustomerCodeDisplayField;
            if (column == nameof(ReminderHistory.CustomerNameDisplay)) field = CustomerNameDisplayField;
            if (column == nameof(ReminderHistory.CalculateBaseDateDisplay)) field = CalculateBaseDateDisplayField;
            if (column == nameof(ReminderHistory.CurrencyCode)) field = CurrencyCodeField;
            if (column == nameof(ReminderHistory.ArrearsDaysDisplay)) field = ArrearsDaysDisplayField;
            if (column == nameof(ReminderHistory.ReminderAmount)) field = ReminderAmountField;
            if (column == nameof(ReminderHistory.InputTypeName)) field = InputTypeNameField;
            if (column == nameof(ReminderHistory.StatusCodeAndName)) field = StatusCodeAndNameField;
            if (column == nameof(ReminderHistory.Memo)) field = MemoField;
            if (column == nameof(ReminderHistory.OutputFlagName)) field = OutputFlagNameField;
            if (column == nameof(ReminderHistory.CreateAt)) field = CreateAtField;
            if (column == nameof(ReminderHistory.CreateByName)) field = CreateByNameField;
            return field;
        }
    }
}
