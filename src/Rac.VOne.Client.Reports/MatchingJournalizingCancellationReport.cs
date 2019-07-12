using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Rac.VOne.Web.Models;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// MatchingJournalizingCancellationReport の概要の説明です。
    /// </summary>
    public partial class MatchingJournalizingCancellationReport : GrapeCity.ActiveReports.SectionReport
    {
        private bool UseForeignCurrency;
        private string displayFieldString = "#,###,###,###,##0";
        public MatchingJournalizingCancellationReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companyCode, string companyName)
        {
            lblCompanyCodeName.Text = companyCode + " " + companyName;
        }

        public void SetData(List<MatchingJournalizingDetail> matchingJournalizingresult, int noOfPre, bool useForeginCurrency)
        {
            UseForeignCurrency = useForeginCurrency;
            if (noOfPre > 0)
            {
                displayFieldString += "." + new string('0', noOfPre);
            }

            DataSource = new BindingSource(matchingJournalizingresult, null);
            txtCreateAt.Text = DateTime.Today.ToShortDateString();
            txtJJournalizingName.DataField = "JournalizingName";
            txtCustomerCode.DataField = "CustomerCode";
            txtCustomerName.DataField = "CustomerName";
            txtCurrencyCode.DataField = "CurrencyCode";
            txtAmount.DataField = "Amount";
            txtOutputAt.DataField = "OutputAt";
            txtReceiptAmount.DataField = "ReceiptAmount";
            txtRecordedAt.DataField = "RecordedAt";
            txtPayerName.DataField = "PayerName";
            txtBilledAt.DataField = "BilledAt";
            txtInvoiceCode.DataField = "InvoiceCode";
            txtBillingAmount.DataField = "BillingAmount";

            txtAmount.OutputFormat = displayFieldString;
            txtReceiptAmount.OutputFormat = displayFieldString;
            txtBillingAmount.OutputFormat = displayFieldString;
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            FormatCurrency();
        }

        private void FormatCurrency()
        {
            if(UseForeignCurrency)
            {
                txtCurrencyCode.Visible = true;
                lineDetailVerCustomerName.Visible = true;
            }
            else
            {
                txtCurrencyCode.Visible = false;
                txtCustomerName.Width += txtCurrencyCode.Width;
                lineDetailVerCustomerName.Visible = false;
            }
        }
        
        private void pageHeader_BeforePrint(object sender, EventArgs e)
        {
            if(UseForeignCurrency)
            {
                lblCurrencyCode.Visible = true;
                lineHeaderVerCustomerName.Visible = true;
            }
            else
            {
                lblCurrencyCode.Visible = false;
                lblCustomerName.Width += lblCurrencyCode.Width;
                lineHeaderVerCustomerName.Visible = false;
            }
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }
    }
}
