using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using GrapeCity.ActiveReports.SectionReportModel;
using Rac.VOne.Web.Models;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// MatchingJournalizingReport の概要の説明です。
    /// </summary>
    public partial class MatchingJournalizingReport : GrapeCity.ActiveReports.SectionReport
    {
        private bool UseForeignCurrency;
        private string TotalCount;

        public MatchingJournalizingReport()
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

        public void SetData(List<MatchingJournalizing> matchingJournalizingResult, int precision, bool useForeginCurrency)
        {
            UseForeignCurrency = useForeginCurrency;
            DataSource = new BindingSource(matchingJournalizingResult, null);

            string displayFormat = "#,##0";
            if (precision > 0)
            {
                displayFormat += "." + new string('0', precision);
            }

            txtAmount.DataField = "Amount";
            txtAmount.OutputFormat = displayFormat;
            txtTotalAmount.OutputFormat = displayFormat;
            TotalCount = matchingJournalizingResult.Count.ToString("#,##0") + " 件";
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            if (!UseForeignCurrency)
            {
                txtCurrencyCode.Visible = false;
                txtInvoiceCode.Height += txtCurrencyCode.Height;
            }
        }

        private void pageHeader_BeforePrint(object sender, EventArgs e)
        {
            if (!UseForeignCurrency)
            {
                lblCurrencyCode.Visible = false;
                lblInvoiceCode.Height += lblCurrencyCode.Height;
                lineHeaderHorCurrencyCode.Visible = false;
            }
        }

        private void detail_AfterPrint(object sender, EventArgs e)
        {
            txtTotalCount.Text = TotalCount;
        }
    }
} 