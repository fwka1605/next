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

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerFeeSectionReport の概要の説明です。
    /// </summary>
    public partial class CustomerFeeSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private int foreignCurrency;

        private int precision;

        public int Precision
        {
            set
            {
                precision = value;
                var displayFormat = "#,##0";
                if (precision > 0)
                    displayFormat += "." + new string('0', precision);

                txtFee1.OutputFormat = displayFormat;
                txtFee2.OutputFormat = displayFormat;
                txtFee3.OutputFormat = displayFormat;
            }
        }

        public CustomerFeeSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblcompanycode.Text = companycode + " " + companyname;
        }

        public void SetData(List<Web.Models.CustomerFee> generalresult, int UseForeignCurrency)
        {
            foreignCurrency = UseForeignCurrency;
            DataSource = new BindingSource(generalresult, null);

            txtCustomerCode.DataField = "CustomerCode";
            txtCustomerName.DataField = "CustomerName";
            txtCurrencyCode.DataField = "CurrencyCode";
            txtFee1.DataField = "Fee1";
            txtFee2.DataField = "Fee2";
            txtFee3.DataField = "Fee3";
            txtUpdateAt1.DataField = "UpdateAt1";
            txtUpdateAt2.DataField = "UpdateAt2";
            txtUpdateAt3.DataField = "UpdateAt3";
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtFee1.Text) == 0M)
                txtFee1.Text = "";
            if (Convert.ToDecimal(txtFee2.Text) == 0M)
                txtFee2.Text = "";
            if (Convert.ToDecimal(txtFee3.Text) == 0M)
                txtFee3.Text = "";

        }

        private void CustomerFeeSectionReport_DataInitialize(object sender, EventArgs e)
        {
            if(foreignCurrency == 0)
            {
                lblCurrencyCode.Visible = false;
                lblCustomerName.Width += lblCurrencyCode.Width;
                txtCurrencyCode.Visible = false;
                txtCustomerName.Width += txtCurrencyCode.Width;
                line14.Visible = false;
                line16.Visible = false;
            }
        }
    }
}
