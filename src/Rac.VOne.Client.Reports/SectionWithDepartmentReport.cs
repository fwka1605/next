using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// SectionWithDepartmentReport の概要の説明です。
    /// </summary>
    public partial class SectionWithDepartmentReport : GrapeCity.ActiveReports.SectionReport, IMasterSectionReport<SectionWithDepartment>
    {
        private List<Web.Models.SectionWithDepartment> List { get; set; }
        private int CurrentIndex = -1;

        public SectionWithDepartmentReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companyCode, string companyName)
        {
            lblCompanyCode.Text = companyCode + " " + companyName;
        }

        public void SetData(IEnumerable<SectionWithDepartment> generalresult)
        {
            List = generalresult.ToList();
            DataSource = new BindingSource(generalresult, null);

            txtSectionCode.DataField = "SectionCode";
            txtSectionName.DataField = "SectionName";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtDepartmentName.DataField = "DepartmentName";
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            lineDetailVerSectionCode.Visible = true;
            lineDetailVerSectionName.Visible = true;
            lineDetailVerDepartmentCode.Visible = true;
        }

        private void detail_Format(object sender, EventArgs e)
        {
            SectionWithDepartment prev = CurrentIndex >= 0 ? List[CurrentIndex] : null;

            if (txtSectionCode.Text == prev?.SectionCode)
            {
                txtSectionCode.Text = string.Empty;
                txtSectionName.Text = string.Empty;
            }
            CurrentIndex++;

            lineDetailVerSectionCode.Visible = false;
            lineDetailVerSectionName.Visible = false;
            lineDetailVerDepartmentCode.Visible = false;
        }
    }
}
