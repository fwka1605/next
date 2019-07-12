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
    /// BillingOmitSectionReport の概要の説明です。
    /// </summary>
    public partial class BillingOmitSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        //   true:削除済みデータ false:未削除データ ,
        private bool IsDeleted { get; set; } = false;

        public BillingOmitSectionReport()
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

        public void SetData(List<Billing> BillingData, int Precision, bool IsDeleted, string note)
        {
            this.IsDeleted = IsDeleted;
            var displayFormat = "#,##0";

            if (Precision > 0)
            {
                displayFormat += "." + new string('0', Precision);
            }

            txtBillingAmount.OutputFormat = displayFormat;
            txtRemainAmount.OutputFormat = displayFormat;
            txtBillingGrandTotal.OutputFormat = displayFormat;
            txtRemainGrandTotal.OutputFormat = displayFormat;
            lblNote1.Text = note;
            DataSource = new BindingSource(BillingData, null);

            if(IsDeleted)
            {
                txtAssignmentFlag.DataField = "DeleteAt";
            }
            else
            {
                txtAssignmentFlag.DataField = "AssignmentFlagName";
            }
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }

        private void pageHeader_BeforePrint(object sender, EventArgs e)
        {
            if (IsDeleted)
            {
                lblAssignmentFlag.Text = "削除日";
                lblTitle.Text = "請求未消込削除一覧表";
            }
            else
            {
                lblAssignmentFlag.Text = "消込区分";
                lblTitle.Text = "請求未消込一覧表";
            }
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            //削除日のみ
            if (IsDeleted && !string.IsNullOrEmpty(txtAssignmentFlag.Text))
            {
                txtAssignmentFlag.Text = txtAssignmentFlag.Text.Substring(0,10);
            }
        }
    }
} 