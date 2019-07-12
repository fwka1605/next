using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Rac.VOne.Web.Models;
using System.Windows.Forms;
using System.Linq;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingAgingListDetailSectionReport の概要の説明です。
    /// </summary>
    public partial class BillingAgingListDetailSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        List<BillingAgingListDetail> BillingAgingDetailList { get; set; }
        int PrintPrecision { get; set; }
        public BillingAgingListDetailSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }
        public void SetBasicPageSetting(string CompanyCode, string CompanyName)
        {
            lblCompanyCodeName.Text = CompanyCode + " " + CompanyName;
        }

        public void SetData(List<BillingAgingListDetail> GeneralResult, int Precision, ColumnNameSetting SettingInfo)
        {
            BillingAgingDetailList = GeneralResult;
            PrintPrecision = Precision;
            DataSource = new BindingSource(GeneralResult, null);
            var displayFormat = "#,##0";
            if (Precision > 0)
            {
                displayFormat += "." + new string('0', Precision);
            }

            txtCustomerCode.DataField = "CustomerCode";
            txtCustomerName.DataField = "CustomerName";
            txtBillingAt.DataField = "BilledAt";
            txtStaffCode.DataField = "StaffCode";
            txtDueAt.DataField = "DueAt";
            txtInvoiceCode.DataField = "InvoiceCode";
            txtStaffName.DataField = "StaffName";
            txtBillingAmount.OutputFormat = displayFormat ;
            txtRemainAmount.OutputFormat = displayFormat;
            txtNote.DataField = "Note";

           lblNote.Text = SettingInfo.DisplayColumnName;
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            //txtPage.Text = (int.Parse(txtPage.Text) + 1).ToString();
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }

        private void groupFooter1_Format(object sender, EventArgs e)
        {
            var displayFormat = "#,##0";
            if (PrintPrecision > 0)
            {
                displayFormat += "." + new string('0', PrintPrecision);
            }
            txtTotalBillingAmount.OutputFormat = displayFormat;
            txtTotalRemainAmount.OutputFormat = displayFormat;
        }
    }
}
