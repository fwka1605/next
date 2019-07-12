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
    /// DepartmentReport の概要の説明です。
    /// </summary>
    public partial class DepartmentReport : GrapeCity.ActiveReports.SectionReport,
        IMasterSectionReport<Department>
    {

        public DepartmentReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }
        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyName.Text = companycode + " " + companyname;
        }

        public void SetData(IEnumerable<Department> generalresult)
        {
            DataSource = new BindingSource(generalresult, null);
            txtDepartmentCode.DataField = "Code";
            txtDepartmentName.DataField = "Name";
            txtStaffCode.DataField = "StaffCode";
            txtStaffName.DataField = "StaffName";
            txtNote.DataField = "Note";
        }
    }
}
