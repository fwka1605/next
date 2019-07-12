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
    /// SectionReport1 の概要の説明です。
    /// </summary>
    public partial class GeneralSettingSectionReport : GrapeCity.ActiveReports.SectionReport, IMasterSectionReport<GeneralSetting>
    {

        public GeneralSettingSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。

            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyName.Text = companycode + " " + companyname;
        }

        public void SetData(IEnumerable<GeneralSetting> generalresult)
        {
            DataSource = new BindingSource(generalresult, null);
            txtcode.DataField = "Code";
            txtvalue.DataField = "Value";
            txtlength.DataField = "Length";
            txtdescription.DataField = "Description";
        }
    }
}