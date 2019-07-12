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
    /// SectionWithLoginUserReport の概要の説明です。
    /// </summary>
    public partial class SectionWithLoginUserReport : GrapeCity.ActiveReports.SectionReport,
        IMasterSectionReport<SectionWithLoginUser>
    {
        public SectionWithLoginUserReport()
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

        public void SetData(IEnumerable<SectionWithLoginUser> items)
        {
            List<string> s = new List<string>();
            foreach (var item in items)
            {
                string lCode = item.LoginUserCode;
                bool res = s.Exists(x => x == lCode);

                if (res)
                {
                    txtLoginUserCode.Text += "" + Environment.NewLine + Environment.NewLine;
                    txtLoginUserName.Text += "" + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    txtLoginUserCode.Text += item.LoginUserCode + Environment.NewLine + Environment.NewLine ;
                    txtLoginUserName.Text += item.LoginUserName + Environment.NewLine + Environment.NewLine;
                    s.Add(item.LoginUserCode);
                }
                txtSectionCode.Text += item.SectionCode + Environment.NewLine + Environment.NewLine;
                txtSectionName.Text += item.SectionName + Environment.NewLine + Environment.NewLine;
                lineDetailVerLoginUserCode.Y2 += float.Parse("0.26");
                lineDetailVerLoginUserName.Y2 += float.Parse("0.26");
                lineDetailVerSectionCode.Y2 += float.Parse("0.26");
            }
        }
    }
}
