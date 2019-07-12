using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ReceiptSearchConditionSectionReport の概要の説明です。
    /// </summary>
    public partial class ReceiptSearchConditionSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        long count = 0;

        public ReceiptSearchConditionSectionReport()
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

        public void SetBasicPageSetting(string companyCode, string companyName, string title)
        {
            lbltitle.Text = title;
            lblcompanycode.Text = companyCode + " " + companyName;
        }

        public void SetPageDataSetting(List<object> dt_search)
        {
            DataSource = new BindingSource(dt_search, null);
            txtSearchName1.DataField = "SearchName1";
            txtSearchValue1.DataField = "SearchValue1";
            txtSearchName2.DataField = "SearchName2";
            txtSearchValue2.DataField = "SearchValue2";
        }

        public void SetPageNumber(long pageCount)
        {
            count = pageCount;
        }

        private void detail_Format(object sender, EventArgs e)
        {
            line1.Visible = false;
            line12.Visible = false;
            line6.Visible = false;
            line9.Visible = false;
            line5.Visible = false;
            line2.Visible = false;
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            line1.Visible = true;
            line12.Visible = true;
            line6.Visible = true;
            line9.Visible = true;
            line5.Visible = true;
            line2.Visible = true;
            txtSearchName1.Height = txtSearchValue2.Height;
            line7.Top = txtSearchValue2.Height;
            txtSearchName2.Height = txtSearchValue2.Height;
            line4.Top = txtSearchValue2.Height;
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (count + 1) + " / " + (count + 1);
        }
    }
}
