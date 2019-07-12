using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerLedgerSearchConditionSectionReport の概要の説明です。
    /// </summary>
    public partial class CustomerLedgerSearchConditionSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private long count = 0;

        public CustomerLedgerSearchConditionSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname, string title)
        {
            lblTitle.Text = title;
            lblCompanyCodeName.Text = companycode + " " + companyname;
        }

        public void SetPageDataSetting(List<object> dt_search)
        {
            DataSource = new BindingSource(dt_search, null);
            txtSearchName.DataField = "SearchName";
            txtSearchValue.DataField = "SearchValue";
        }

        public void SetPageNumber(long pageCount)
        {
            count = pageCount;
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (count + 1) + " / " + (count + 1);
        }
    }
}
