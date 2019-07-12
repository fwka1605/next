using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    public partial class MatchingHistorySectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private string displayFieldString = "#,###,###,###,##0";
        private int precision;

        public MatchingHistorySectionReport()
        {
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname, bool takeTotal)
        {
            lblcompanycode.Text = companycode + " " + companyname;
            ghCreateAt.Visible = takeTotal;
            gfCreateAt.Visible = takeTotal;
        }

        public void SetPageDataSetting(List<ExportMatchingHistory> matchingList, bool timeSort, int precisionLength, string NoteColumnName)
        {
            precision = precisionLength;
            lblNote.Text = NoteColumnName;
            DataSource = new BindingSource(matchingList, null);
            txtCreateAt.DataField = nameof(ExportMatchingHistory.CreateAt);
            txtLoginUserTotal.DataField = nameof(ExportMatchingHistory.LoginUserName);
            txtCustomerCode.DataField = nameof(ExportMatchingHistory.CustomerCode);
            txtCustomerName.DataField = nameof(ExportMatchingHistory.CustomerName);
            txtBillingAt.DataField = nameof(ExportMatchingHistory.BilledAt);
            txtInvoiceCode.DataField = nameof(ExportMatchingHistory.InvoiceCode);
            txtCategory.DataField = nameof(ExportMatchingHistory.BillingCategory);
            txtBillingAmount.DataField = nameof(ExportMatchingHistory.BillingAmount);
            txtAmount.DataField = nameof(ExportMatchingHistory.MatchingAmount);
            txtBillingRemain.DataField = nameof(ExportMatchingHistory.BillingRemain);
            txtRecordedAt.DataField = nameof(ExportMatchingHistory.RecordedAt);
            txtReceiptId.DataField = nameof(ExportMatchingHistory.ReceiptId);
            txtReceiptCategory.DataField = nameof(ExportMatchingHistory.ReceiptCategory);
            txtReceiptAmount.DataField = nameof(ExportMatchingHistory.ReceiptAmount);
            txtBeforeAccept.DataField = nameof(ExportMatchingHistory.AdvanceReceivedOccuredString);
            txtReceiptRemain.DataField = nameof(ExportMatchingHistory.ReceiptRemain);
            txtBankCode.DataField = nameof(ExportMatchingHistory.BankCode);
            txtBankName.DataField = nameof(ExportMatchingHistory.BankName);
            txtBranchCode.DataField = nameof(ExportMatchingHistory.BranchCode);
            txtBranchName.DataField = nameof(ExportMatchingHistory.BranchName);
            txtAccountNumber.DataField = nameof(ExportMatchingHistory.AccountNumber);
            txtPayerName.DataField = nameof(ExportMatchingHistory.PayerName);
            txtNote.DataField = nameof(ExportMatchingHistory.ReceiptNote1);
            txtLoginUser.DataField = nameof(ExportMatchingHistory.LoginUserName);
            txtProcessType.DataField = nameof(ExportMatchingHistory.MatchingProcessTypeString);
            txtBeforeAccept.DataField = nameof(ExportMatchingHistory.AdvanceReceivedOccuredString);

            ghCreateAt.DataField = nameof(ExportMatchingHistory.CreateAtSource);
            txtCreateAtTotal.DataField = nameof(ExportMatchingHistory.CreateAtSource);
            txtBillingAmountTotal.DataField = nameof(ExportMatchingHistory.BillingAmount);
            txtAmountTotal.DataField = nameof(ExportMatchingHistory.MatchingAmount);
            txtBillingRemainTotal.DataField = nameof(ExportMatchingHistory.BillingRemain);
            txtReceiptAmountTotal.DataField = nameof(ExportMatchingHistory.ReceiptAmount);
            txtReceiptRemainTotal.DataField = nameof(ExportMatchingHistory.ReceiptRemain);
            txtProcessTypeTotal.DataField = nameof(ExportMatchingHistory.MatchingProcessTypeString);
            txtLoginUserTotal.DataField = nameof(ExportMatchingHistory.LoginUserName);
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }

        private void MatchingHistorySectionReport_ReportStart(object sender, EventArgs e)
        {
            if (precision > 0)
            {
                displayFieldString += "." + new string('0', precision);
            }
            txtAmount.OutputFormat = displayFieldString;
            txtBillingAmount.OutputFormat = displayFieldString;
            txtBillingRemain.OutputFormat = displayFieldString;
            txtReceiptAmount.OutputFormat = displayFieldString;
            txtReceiptRemain.OutputFormat = displayFieldString;

            txtAmountTotal.OutputFormat = displayFieldString;
            txtBillingAmountTotal.OutputFormat = displayFieldString;
            txtBillingRemainTotal.OutputFormat = displayFieldString;
            txtReceiptAmountTotal.OutputFormat = displayFieldString;
            txtReceiptRemainTotal.OutputFormat = displayFieldString;
        }
    }
}
