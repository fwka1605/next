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
    /// ArrearagesListReport の概要の説明です。
    /// </summary>
    public partial class ArrearagesListReport : GrapeCity.ActiveReports.SectionReport
    {
        public ArrearagesListReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companyCode, string companyName)
        {
            lblCompanyCodeName.Text = companyCode + " " + companyName;
        }

        public void SetData(List<ArrearagesList> GeneralResult, int Precision, List<ReportSetting> ReportSettingList, List<ColumnNameSetting> SettingInfo)
        {
            var displayFormat = "#,###,###,###,##0";

            foreach (ColumnNameSetting gs in SettingInfo)
            {
                switch (gs.ColumnName)
                {
                    case "Note1": lblNote.Text = gs.DisplayColumnName; break;
                }
            }

            if (ReportSettingList[0].ItemKey == "0")
            {
                groupFooter2.Visible = false;
                groupFooter2.Height = 0;
            }
            else
            {
                groupHeader2.DataField = nameof(ArrearagesList.DepartmentCode);
            }

            if (ReportSettingList[1].ItemKey == "0")
            {
                groupFooter3.Visible = false;
                groupFooter3.Height = 0;
            }
            else
            {
                groupHeader3.DataField = nameof(ArrearagesList.StaffCode);
            }

            if (ReportSettingList[2].ItemKey == "0")
            {
                groupFooter4.Visible = false;
                groupFooter4.Height = 0;
            }
            else
            {
                groupHeader4.DataField = nameof(ArrearagesList.CustomerCode);
            }

            if (ReportSettingList[3].ItemKey == "0")
            {
                groupFooter5.Visible = false;
                groupFooter5.Height = 0;
            }
            else
            {
                groupHeader5.DataField = nameof(ArrearagesList.DueAt);
            }

            //order by
            var orders = GeneralResult.AsQueryable().OrderBy(x => 0);
            if (ReportSettingList[0].ItemKey == "1")
            {
                orders = orders.ThenBy(q => q.DepartmentCode);
            }
            if (ReportSettingList[1].ItemKey == "1")
            {
                orders = orders.ThenBy(q => q.StaffCode);
            }
            if (ReportSettingList[2].ItemKey == "1")
            {
                orders = orders.ThenBy(q => q.CustomerCode);
            }
            if (ReportSettingList[3].ItemKey == "1")
            {
                orders = orders.ThenBy(q => q.DueAt);
            }
            if (ReportSettingList[5].ItemKey == "0" && ReportSettingList[2].ItemKey == "0")
            {
                orders = orders.ThenBy(q => q.CustomerCode);
            }
            if (ReportSettingList[5].ItemKey == "2")
            {
                orders = orders.ThenBy(q => q.Id);
            }
            if (ReportSettingList[6].ItemKey == "0")
            {
                orders = orders.ThenBy(q => q.BilledAt);
            }
            if (ReportSettingList[6].ItemKey == "1")
            {
                orders = orders.ThenBy(q => q.SalesAt);
            }
            if (ReportSettingList[6].ItemKey == "2")
            {
                orders = orders.ThenBy(q => q.ClosingAt);
            }
            if (ReportSettingList[6].ItemKey == "3" && ReportSettingList[3].ItemKey == "0")
            {
                orders = orders.ThenBy(q => q.DueAt);
            }
            if (ReportSettingList[6].ItemKey == "4")
            {
                orders = orders.ThenBy(q => q.OriginalDueAt);
            }

            DataSource = new BindingSource(orders, null);

            if (Precision > 0)
            {
                displayFormat += "." + new string('0', Precision);
            }

            txtID.DataField = "Id";
            txtCustomerCode.DataField = "CustomerCode";
            txtCustomerName.DataField = "CustomerName";
            txtTel.DataField = "Tel";
            txtCustomerNote.DataField = "CustomerNote";
            txtBilledAt.DataField = "BilledAt";
            txtSalesAt.DataField = "SalesAt";
            txtClosingAt.DataField = "ClosingAt";
            txtDueAt.DataField = "DueAt";
            txtOriginalDueAt.DataField = "OriginalDueAt";
            txtArrearagesDayCount.DataField = "ArrearagesDayCount";
            txtRemainAmount.OutputFormat = displayFormat;
            txtInvoiceCode.DataField = "InvoiceCode";
            txtNote1.DataField = "Note1";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtDepartmentName.DataField = "DepartmentName";
            txtStaffCode.DataField = "StaffCode";
            txtStaffName.DataField = "StaffName";

            txtDueAtAmount.OutputFormat = displayFormat;
            txtCustomerAmount.OutputFormat = displayFormat;
            txtStaffAmount.OutputFormat = displayFormat;
            txtDepartmentAmount.OutputFormat = displayFormat;
            txtTotalAmount.OutputFormat = displayFormat;
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            txtArrearagesDayCount.Text = txtArrearagesDayCount.Text + "日";
        }
    }
}