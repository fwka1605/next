using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using Header = Rac.VOne.Web.Models.PeriodicBillingSetting;

namespace Rac.VOne.Web.Models.Files
{
    public class PeriodicBillingSettingFileDifinition : RowDefinition<Header>
    {
        #region fields
        public StandardIdToCodeFieldDefinition<Header, Company> CompanyIdField { get; private set; }
        public StringFieldDefinition<Header> NameField { get; private set; }
        public StringFieldDefinition<Header> CustomerCodeField { get; private set; }
        public StringFieldDefinition<Header> CustomerNameField { get; private set; }
        public StringFieldDefinition<Header> DestinationCodeField { get; private set; }
        public StringFieldDefinition<Header> BilledCycleStatusField { get; private set; }
        public NumberFieldDefinition<Header, int> BilledDayField { get; private set; }
        public NumberFieldDefinition<Header, DateTime> StartMonthField { get; private set; }
        public NullableNumberFieldDefinition<Header, DateTime> EndMonthField { get; private set; }
        public StringFieldDefinition<Header> InvoiceCodeField { get; private set; }
        public NumberFieldDefinition<Header, int> SetNote1Field { get; private set; }
        public NumberFieldDefinition<Header, int> SetNote2Field { get; private set; }
        public NumberFieldDefinition<Header, decimal> BillingAmountField { get; private set; }

        #endregion

        public PeriodicBillingSettingFileDifinition(DataExpression dataExpression)
            : base(dataExpression)
        {
            StartLineNumber = 1;
            DataTypeToken = "定期請求パターン";
            FileNameToken = DataTypeToken;
            OutputHeader = true;

            Fields.AddRange(GetFields());
        }

        private const string YearMonthFormat = "yyyy/MM";

        private IEnumerable<IFieldDefinition<Header>> GetFields()
        {
            yield return (CompanyIdField = new StandardIdToCodeFieldDefinition<Header, Company>(x => x.CompanyId, x => x.Id, null, x => x.Code,
                "会社コード", 1, accept: x => x.OwnCompanyCode(CompanyIdField)));
            yield return (NameField = new StringFieldDefinition<Header>(x => x.Name,
                "パターン名", 2, accept: x => x.StandardString(NameField)));
            yield return (CustomerCodeField = new StringFieldDefinition<Header>(x => x.CustomerCode,
                "得意先コード", 3, accept: x => x.StandardString(CustomerCodeField)));
            yield return (CustomerNameField = new StringFieldDefinition<Header>(x => x.CustomerName,
                "得意先名", 4, accept: x => x.StandardString(CustomerNameField)));
            yield return (DestinationCodeField = new StringFieldDefinition<Header>(x => x.DestinationCode,
                "仕向先コード", 5, accept: x => x.StandardString(DestinationCodeField)));
            yield return (BilledCycleStatusField = new StringFieldDefinition<Header>(x => x.BilledCycleStatus,
                "請求サイクル", 6, accept: x => x.StandardString(BilledCycleStatusField)));
            yield return (BilledDayField = new NumberFieldDefinition<Header, int>(x => x.BilledDay,
                "請求日", 7, accept: x => x.StandardNumber(BilledDayField), formatter: x => x.ToString()));
            yield return (StartMonthField = new NumberFieldDefinition<Header, DateTime>(x => x.StartMonth,
                "開始月", 8, accept: x => x.StandardNumber(StartMonthField), formatter: x => x.ToString(YearMonthFormat)));
            yield return (EndMonthField = new NullableNumberFieldDefinition<Header, DateTime>(x => x.EndMonth,
                "終了月", 9, accept: x => x.StandardNumber(EndMonthField), formatter: x => x.ToString(YearMonthFormat)));
            yield return (InvoiceCodeField = new StringFieldDefinition<Header>(x => x.InvoiceCode,
                "請求書番号", 10, accept: x => x.StandardString(InvoiceCodeField)));
            yield return (SetNote1Field = new NumberFieldDefinition<Header, int>(x => x.SetBillingNote1,
                "備考1設定", 11, accept: x => x.StandardNumber(SetNote1Field), formatter: x => x.ToString()));
            yield return (SetNote2Field = new NumberFieldDefinition<Header, int>(x => x.SetBillingNote2,
                "備考2設定", 12, accept: x => x.StandardNumber(SetNote2Field), formatter: x => x.ToString()));
            yield return (BillingAmountField = new NumberFieldDefinition<Header, decimal>(x => x.BillingAmount,
                "請求額合計", 13, accept: x => x.StandardNumber(BillingAmountField), formatter: x => x.ToString("0")));
        }


    }
}
