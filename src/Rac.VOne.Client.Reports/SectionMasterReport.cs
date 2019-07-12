using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// SectionMasterReport の概要の説明です。
    /// </summary>
    public partial class SectionMasterReport : GrapeCity.ActiveReports.SectionReport, IMasterSectionReport<Section>
    {

        public SectionMasterReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }
        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyCode.Text = companycode + " " + companyname;
        }
        public void SetData(IEnumerable<Section> generalResult)
        {
            DataSource = new BindingSource(generalResult, null);
            txtSectionCode.DataField = "Code";
            txtSectionName.DataField = "Name";
            txtPayerCodeLeft.DataField = "PayerCodeLeft";
            txtPayerCodeRight.DataField = "PayerCodeRight";
            txtUpdateDate.DataField = "UpdateDate";
            txtLoginUserName.DataField = "LoginUserName";
        }
    }
}
