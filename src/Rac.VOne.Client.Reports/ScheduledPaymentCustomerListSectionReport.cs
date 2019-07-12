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
    /// ScheduledPaymentCustomerListSectionReport の概要の説明です。
    /// </summary>
    public partial class ScheduledPaymentCustomerListSectionReport : GrapeCity.ActiveReports.SectionReport
    {

        public ScheduledPaymentCustomerListSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyCodeName.Text = companycode + " " + companyname;
        }

        public void SetData(List<ScheduledPaymentList> ScheduledPaymentList, int Precision, List<ReportSetting> ReportSettingList)
        {
            var displayFormat = "#,###,###,###,##0";
            if (Precision > 0)
            {
                displayFormat += "." + new string('0', Precision);
            }

            DataSource = new BindingSource(ScheduledPaymentList, null);
            txtCustomerCode.DataField = "CustomerCode";
            txtCustomerName.DataField = "CustomerName";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtDepartmentName.DataField = "DepartmentName";
            txtStaffCode.DataField = "StaffCode";
            txtStaffName.DataField = "StaffName";
            txtRemainAmount.OutputFormat = displayFormat;
            txtTotalAmount.OutputFormat = displayFormat;
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }
    }
}
