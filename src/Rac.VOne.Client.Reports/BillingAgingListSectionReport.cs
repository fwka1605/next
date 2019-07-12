using Rac.VOne.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GrapeCity.ActiveReports.SectionReportModel;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// 請求残高年齢表 入金額を使用する/消込額を使用して入金額を表示
    /// </summary>
    public partial class BillingAgingListSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        public bool ConsiderCustomerGroup { get; set; }
        public bool DisplayCutsomerCode { get; set; }
        public bool RequireStaffSubtotal { get; set; }
        public bool RequireDepartmentSubtotal { get; set; }
        private List<BillingAgingList> BillingAgingList { get; set; }
        private int UseForeignCurrency;

        private List<ReportSetting> ReportSetting { get; set; }
        public BillingAgingListSectionReport()
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

        public void SetData(List<BillingAgingList> GeneralResult,
            int Precision,
            int ForeignCurrency)
        {
            InitializePageHeaderSize();
            BillingAgingList = GeneralResult;
            UseForeignCurrency = ForeignCurrency;

            DataSource = new BindingSource(BillingAgingList, null);
            var displayFormat = "#,##0";
            if (Precision > 0)
            {
                displayFormat += "." + new string('0', Precision);
            }

            txtLastMonthRemain.OutputFormat = displayFormat;
            txtCurrentMonthSales.OutputFormat = displayFormat;
            txtCurrentMonthReceipt.OutputFormat = displayFormat;
            txtCurrentMonthMatching.OutputFormat = displayFormat;
            txtCurrentMonthRemain.OutputFormat = displayFormat;
            txtMonthlyRemain0.OutputFormat = displayFormat;
            txtMonthlyRemain1.OutputFormat = displayFormat;
            txtMonthlyRemain2.OutputFormat = displayFormat;
            txtMonthlyRemain3.OutputFormat = displayFormat;
        }

        private void InitializePageHeaderSize()
        {
            var offset = 0.0F;
            if (!RequireDepartmentSubtotal)
            {
                lblDepartmentCode.Visible = false;
                txtDepartmentCode.Visible = false;
                txtDepartmentName.Visible = false;
                offset += lblDepartmentCode.Height;
            }
            if (offset > 0.0F)
            {
                lblStaffCode.Top -= offset;
                txtStaffCode.Top -= offset;
                txtStaffName.Top -= offset;
            }
            if (!RequireStaffSubtotal)
            {
                lblStaffCode.Visible = false;
                txtStaffCode.Visible = false;
                txtStaffName.Visible = false;
                offset += lblStaffCode.Height;
            }
            if (offset > 0.0F)
            {
                var items = pageHeader.Controls.Cast<ARControl>();
                foreach (var item in items)
                {
                    if (!IsHeaderColumnControl(item)) continue;
                    item.Top -= offset;
                }
                pageHeader.Height -= offset;
            }
        }

        private bool IsHeaderColumnControl(ARControl control)
        {
            if (control.Name.StartsWith("lineHead"))    return true;
            if (control == lblCustomer)                 return true;
            if (control == lblLastMonthReamin)          return true;
            if (control == lblCurrentBilling)           return true;
            if (control == lblCurrentReceiptMatching)   return true;
            if (control == lblCurrentReceipt)           return true;
            if (control == lblCurrentMatching)          return true;
            if (control == lblCurrentRemain)            return true;
            if (control == lblMonthlyRemain0)           return true;
            if (control == lblMonthlyRemain1)           return true;
            if (control == lblMonthlyRemain2)           return true;
            if (control == lblMonthlyRemain3)           return true;
            return false;
        }

        private int RowIndex { get; set; }
        private void detail_Format(object sender, EventArgs e)
        {
            var current = BillingAgingList[RowIndex];
            RowIndex++;
            var next = RowIndex < BillingAgingList.Count ? BillingAgingList[RowIndex] : null;

            switch (current.RecordType)
            {
                case 0:
                    if (current.ParentCustomerFlag != 1 && current.ParentCustomerId.HasValue && ConsiderCustomerGroup)
                    {
                        txtParentCustomer.Text = DisplayCutsomerCode
                            ? $"  {current.CustomerCode} {current.CustomerName}"
                            : $"  {current.CustomerName}";
                    }
                    else
                    {
                        txtParentCustomer.Text = DisplayCutsomerCode
                            ? $"{current.CustomerCode} {current.CustomerName}"
                            : $"{current.CustomerName}";
                    }
                    detailBackColor.BackColor = Color.Transparent;
                    break;
                case 1:
                    txtParentCustomer.Text = $"{current.StaffCode} {current.StaffName}" + "  計";
                    detailBackColor.BackColor = Color.WhiteSmoke;
                    break;
                case 2:
                    txtParentCustomer.Text = $"{current.DepartmentCode} {current.DepartmentName}" + "  計";
                    detailBackColor.BackColor = Color.WhiteSmoke;
                    break;
                case 3:
                    txtParentCustomer.Text = UseForeignCurrency == 1 ? "通貨計" : "総合計";
                    detailBackColor.BackColor = Color.WhiteSmoke;
                    break;
            }
            lblChildCustomer.Text = current.ParentCustomerFlag == 1 && ConsiderCustomerGroup ? "小計" : " ";

            if (current.RecordType != 0 && next?.RecordType == 0)
            {
                detail.NewPage = NewPage.After;
            }
            else
            {
                detail.NewPage = NewPage.None;
            }
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }
    }
}
