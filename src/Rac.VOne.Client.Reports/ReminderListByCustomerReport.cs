using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// RiminderListByCustomerReport の概要の説明です。
    /// </summary>
    public partial class ReminderListByCustomerReport : GrapeCity.ActiveReports.SectionReport
    {
        public ReminderListByCustomerReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
            InitializeControls();
        }
        private void InitializeControls()
        {
            Name = "督促管理帳票" + DateTime.Today.ToString("yyyyMMdd");

            txtCustomerCode.DataField = nameof(ReminderHistory.CustomerCode);
            txtCustomerName.DataField = nameof(ReminderHistory.CustomerName);
            txtReminderAmount.DataField = nameof(ReminderHistory.ReminderAmount);
            txtInputTypeName.DataField = nameof(ReminderHistory.InputTypeName);
            txtMemo.DataField = nameof(ReminderHistory.MemoRemoveLine);
            txtCreateAt.DataField = nameof(ReminderHistory.CreateAt);
            txtCreateBy.DataField = nameof(ReminderHistory.CreateByName);

            txtCustomerCode.SummaryGroup = groupHeader1.Name;
            txtCustomerName.SummaryGroup = groupHeader1.Name;

            txtMemo.MultiLine = true;

            groupHeader1.CanShrink = true;

            groupHeader1.DataField = nameof(ReminderHistory.CustomerCode);
            groupHeader1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            groupHeader1.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPage;

            detail.CanShrink = true;

            var displayFormat = "#,##0";
            txtReminderAmount.OutputFormat = displayFormat;
        }
        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyCode.Text = companycode + " " + companyname;
        }

    }
}
