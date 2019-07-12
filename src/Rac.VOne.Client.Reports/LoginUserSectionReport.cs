using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// LoginUserSectionReports の概要の説明です。
    /// </summary>
    public partial class LoginUserSectionReport : GrapeCity.ActiveReports.SectionReport, IMasterSectionReport<LoginUser>
    {
        public bool UseDistribution { get; set; }

        public LoginUserSectionReport()
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

        public void SetData(IEnumerable<LoginUser> generalresult)
        {
            DataSource = new BindingSource(generalresult, null);

            txtUserCode.DataField = "Code";
            txtUserName.DataField = "Name";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtDepartmentName.DataField = "DepartmentName";
            txtMenuLevel.DataField = "MenuLevel";
            txtFuctionLevel.DataField = "FunctionLevel";
            txtMail.DataField = "Mail";
            txtVone.DataField = "UseClient";
            txtWebview.DataField = "UseWebViewer";
        }

        private void LoginUserSectionReport_DataInitialize(object sender, EventArgs e)
        {
            if (!UseDistribution)
            {
                lineHeaderVerMail.Visible = false;
                lineHeaderVerVOne.Visible = false;
                lblVOne.Visible = false;
                lblWebViewer.Visible = false;

                lineDetailVerMail.Visible = false;
                lineDetailVerVOne.Visible = false;
                txtVone.Visible = false;
                txtWebview.Visible = false;

                lblMail.Width += lblVOne.Width + lblWebViewer.Width;
                txtMail.Width += txtVone.Width + txtWebview.Width;
            }
        }
    }
}
