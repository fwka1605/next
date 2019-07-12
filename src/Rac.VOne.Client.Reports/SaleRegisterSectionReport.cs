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
    /// SaleRegisterSectionReport の概要の説明です。
    /// </summary>
    public partial class SaleRegisterSectionReport : GrapeCity.ActiveReports.SectionReport,
        IMasterSectionReport<Staff>
    {
        public SaleRegisterSectionReport()
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

        public void SetData(IEnumerable<Staff> staffresult)
        {
            DataSource = new BindingSource(staffresult, null);

            txtStaffCode.DataField = "Code";
            txtStaffName.DataField = "Name";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtDepartmentName.DataField = "DepartmentName";
            txtMail.DataField = "Mail";
            txtTel.DataField = "Tel";
            txtFax.DataField = "Fax";
        }
    }
}
