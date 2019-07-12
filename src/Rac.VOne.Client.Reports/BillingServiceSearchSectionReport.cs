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
    /// BillSectionReport の概要の説明です。
    /// </summary>
    public partial class BillingServiceSearchSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        //   true:削除済みデータ false:未削除データ ,
        private bool IsDeleted { get; set; } = false;

        public BillingServiceSearchSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            detail.BeforePrint += (sender, e) =>
            {
                txtBillingAmount.Truncate(UnitPrice, precision);
                txtRemainAmount.Truncate(UnitPrice, precision);
            };
            gfCustomerTotal.BeforePrint += (sender, e) =>
            {
                txtBillingCustomerTotal.Truncate(UnitPrice, precision);
                txtRemainCustomerTotal.Truncate(UnitPrice, precision);
            };
            gfStaffTotal.BeforePrint += (sender, e) =>
            {
                txtBillingStaffTotal.Truncate(UnitPrice, precision);
                txtRemainStaffTotal.Truncate(UnitPrice, precision);
            };
            gfDepartmentTotal.BeforePrint += (sender, e) =>
            {
                txtBillingDepartmentTotal.Truncate(UnitPrice, precision);
                txtRemainDepartmentTotal.Truncate(UnitPrice, precision);
            };
            gfGrandTotal.BeforePrint += (sender, e) =>
            {
                txtBillingGrandTotal.Truncate(UnitPrice, precision);
                txtRemainGrandTotal.Truncate(UnitPrice, precision);
            };
        }

        public void SetBasicPageSetting(string CompanyCode, string CompanyName)
        {
            lblcompanycode.Text = CompanyCode + " " + CompanyName;
        }
        public decimal UnitPrice { private get; set; }

        private int precision;
        public int Precision
        {
            set
            {
                precision = value;
                var displayFormat = "#,##0";
                if (precision > 0)
                {
                    displayFormat += "." + new string('0', precision);
                }

                txtBillingAmount.OutputFormat = displayFormat;
                txtRemainAmount.OutputFormat = displayFormat;
                txtBillingCustomerTotal.OutputFormat = displayFormat;
                txtBillingStaffTotal.OutputFormat = displayFormat;
                txtBillingDepartmentTotal.OutputFormat = displayFormat;
                txtBillingGrandTotal.OutputFormat = displayFormat;
                txtRemainCustomerTotal.OutputFormat = displayFormat;
                txtRemainStaffTotal.OutputFormat = displayFormat;
                txtRemainDepartmentTotal.OutputFormat = displayFormat;
                txtRemainGrandTotal.OutputFormat = displayFormat;
            }
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

            if (IsDeleted)
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
                label9.Text = "削除日";
                //lbltitle.Text = "請求未消込削除一覧表";
            }
            else
            {
                label9.Text = "消込区分";
                //lbltitle.Text = "請求未消込一覧表";
            }
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            //削除日のみ
            if (IsDeleted && !string.IsNullOrEmpty(txtAssignmentFlag.Text))
            {
                txtAssignmentFlag.Text = txtAssignmentFlag.Text.Substring(0, 10);
            }
        }

    }
} 