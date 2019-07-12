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
    public class ReminderManagementFileDifinition : RowDefinition<ReminderSummary>
    {
        public StringFieldDefinition<ReminderSummary> CustomerCodeField { get; private set; }
        public StringFieldDefinition<ReminderSummary> CustomerNameField { get; private set; }
        public NumberFieldDefinition<ReminderSummary, int> ReminderCountField { get; private set; }
        public NumberFieldDefinition<ReminderSummary, int> BillingCountField { get; private set; }
        public StringFieldDefinition<ReminderSummary> CurrencyCodeField { get; private set; }
        public NumberFieldDefinition<ReminderSummary, decimal> RemainAmountField { get; private set; }
        public NumberFieldDefinition<ReminderSummary, decimal> ReminderAmountField { get; private set; }
        public StringFieldDefinition<ReminderSummary> MemoField { get; private set; }
        public StringFieldDefinition<ReminderSummary> CustomerStaffNameField { get; set; }
        public StringFieldDefinition<ReminderSummary> CustomerNoteField { get; set; }
        public StringFieldDefinition<ReminderSummary> CustomerTelField { get; set; }
        public StringFieldDefinition<ReminderSummary> CustomerFaxField { get; set; }


        public ReminderManagementFileDifinition(DataExpression applicationControl)
            : base(applicationControl)
        {
            StartLineNumber = 1;
            DataTypeToken = "督促データ管理";
            FileNameToken = DataTypeToken;
            Fields.AddRange(InitializeFields());
        }

        private IEnumerable<IFieldDefinition<ReminderSummary>> InitializeFields()
        {
            yield return (CustomerCodeField = new StringFieldDefinition<ReminderSummary>(k => k.CustomerCode,
                "得意先コード", 1, accept: x => x.StandardString(CustomerCodeField)));
            yield return (CustomerNameField = new StringFieldDefinition<ReminderSummary>(k => k.CustomerName,
                "得意先名", 2, accept: x => x.StandardString(CustomerNameField)));
            yield return (CurrencyCodeField = new StringFieldDefinition<ReminderSummary>(k => k.CurrencyCode,
                "通貨コード", 3, accept: x => x.StandardString(CurrencyCodeField)));
            yield return (RemainAmountField = new NumberFieldDefinition<ReminderSummary, decimal>(k => k.RemainAmount,
                "請求残", 4, accept: x => x.StandardNumber(RemainAmountField), formatter: value => value.ToString()));
            yield return (ReminderAmountField = new NumberFieldDefinition<ReminderSummary, decimal>(k => k.ReminderAmount,
                "滞留金額", 5, accept: x => x.StandardNumber(ReminderAmountField), formatter: value => value.ToString()));
            yield return (MemoField = new StringFieldDefinition<ReminderSummary>(k => k.Memo,
                "対応記録", 6, accept: x => x.StandardString(MemoField)));
            yield return (CustomerStaffNameField = new StringFieldDefinition<ReminderSummary>(k => k.CustomerStaffName,
                "相手先担当者", 7, accept: x => x.StandardString(CustomerStaffNameField)));
            yield return (CustomerNoteField = new StringFieldDefinition<ReminderSummary>(k => k.CustomerNote,
                "得意先備考", 8, accept: x => x.StandardString(CustomerNoteField)));
            yield return (CustomerTelField = new StringFieldDefinition<ReminderSummary>(k => k.CustomerTel,
                "電話番号", 9, accept: x => x.StandardString(CustomerTelField)));
            yield return (CustomerFaxField = new StringFieldDefinition<ReminderSummary>(k => k.CustomerFax,
                "FAX番号", 10, accept: x => x.StandardString(CustomerFaxField)));
        }
    }
}
