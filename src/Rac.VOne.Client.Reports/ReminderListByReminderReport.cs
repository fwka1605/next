using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ReminderListReportByReminder の概要の説明です。
    /// </summary>
    public partial class ReminderListByReminderReport : GrapeCity.ActiveReports.SectionReport
    {
        public ReminderListByReminderReport()
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

            txtCustomerCode.DataField = nameof(ReminderHistory.CustomerCodeDisplay);
            txtCustomerName.DataField = nameof(ReminderHistory.CustomerNameDisplay);
            txtCalculateBaseDate.DataField = nameof(ReminderHistory.CalculateBaseDateDisplay);
            txtReminderAmount.DataField = nameof(ReminderHistory.ReminderAmount);
            txtInputTypeName.DataField = nameof(ReminderHistory.InputTypeName);
            txtStatus.DataField = nameof(ReminderHistory.StatusCodeAndName);
            txtMemo.DataField = nameof(ReminderHistory.MemoRemoveLine);
            txtOutputFlagName.DataField = nameof(ReminderHistory.OutputFlagName);
            txtCreateAt.DataField = nameof(ReminderHistory.CreateAt);
            txtCreateBy.DataField = nameof(ReminderHistory.CreateByName);

            pageHeader.CanShrink = true;
            detail.CanShrink = true;

            var displayFormat = "#,##0";
            txtReminderAmount.OutputFormat = displayFormat;

            txtMemo.MultiLine = true;
        }
        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyCode.Text = companycode + " " + companyname;
        }
    }
}
