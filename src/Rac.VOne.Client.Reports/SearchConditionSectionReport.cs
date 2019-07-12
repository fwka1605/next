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
    /// AllMatchingSectionReport の概要の説明です。
    /// </summary>
    public partial class SearchConditionSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        long count = 0;

        public SearchConditionSectionReport()
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
            if (title == "請求残高年齢表（明細）")
            {
                lblTitle.Font = new Font("ＭＳ 明朝", 14, FontStyle.Underline);
                txtSearchName.Font = new Font("ＭＳ 明朝", 9);
                txtSearchValue.Font = new Font("ＭＳ 明朝", 9);
            }
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
            lblPageNumber.Text = (count + 1) + " / " + (count+1);
        }

        private void detail_Format(object sender, EventArgs e)
        {
            lineDetailVerSearchName.Visible = false;
            lineDetailVerSearchValue.Visible = false;
            lnverStart.Visible = false;
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            lineDetailVerSearchName.Visible = true;
            lineDetailVerSearchValue.Visible = true;
            lnverStart.Visible = true;

            txtSearchName.Height = txtSearchValue.Height;
            lineDetailHorLower.Top = txtSearchValue.Height;
        }
    }
}
