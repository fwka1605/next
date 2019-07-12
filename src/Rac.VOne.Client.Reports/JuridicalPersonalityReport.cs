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
    /// JuridicalPersonalityReport の概要の説明です。
    /// </summary>
    public partial class JuridicalPersonalityReport : GrapeCity.ActiveReports.SectionReport, IMasterSectionReport<JuridicalPersonality>
    {

        public JuridicalPersonalityReport()
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

        public void SetData(IEnumerable<JuridicalPersonality> JuridicalResult)
        {

            DataSource = new BindingSource(JuridicalResult, null);
            txtKana.DataField = "Kana";

        }
    }
}
