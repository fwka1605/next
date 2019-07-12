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
    /// ArrearagesCustomerListReport の概要の説明です。
    /// </summary>
    public partial class ArrearagesCustomerListReport : GrapeCity.ActiveReports.SectionReport
    {
        public ArrearagesCustomerListReport()
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

        public void SetData(List<ArrearagesList> GeneralResult, int Precision, List<ReportSetting> ReportSettingList)
        {

            var displayFormat = "#,###,###,###,##0";
            if (Precision > 0)
            {
                displayFormat += "." + new string('0', Precision);
            }

            DataSource = new BindingSource(GeneralResult, null);
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