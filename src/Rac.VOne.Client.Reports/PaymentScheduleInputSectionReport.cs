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

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// PaymentScheduleInputSectionReport の概要の説明です。
    /// </summary>
    public partial class PaymentScheduleInputSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private string displayFieldString = "#,###,###,###,##0";
        public PaymentScheduleInputSectionReport()
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

        public void SetData(List<Web.Models.Billing> generalresult, int precision)
        {
            if (precision > 0)
            {
                displayFieldString += "." + new string('0', precision);
            }

            DataSource = new BindingSource(generalresult, null);

            txtInvoiceCode.DataField = "InvoiceCode";
            txtCategoryCode.DataField = "BillingCategoryCodeAndName";
            //txtCategoryName.DataField = "Name";
            txtCustomerCode.DataField = "CustomerCode";
            txtCustomerName.DataField = "CustomerName";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtDepartmentName.DataField = "DepartmentName";
            txtBillingAmount.DataField = "BillingAmount";
            txtScheduledPaymentAmt.DataField = "PaymentAmount";  //Billing.RemainAmount - Billing.OffsetAmount
            txtOffsetAmount.DataField = "OffsetAmount";
            txtRemainAmount.DataField = "RemainAmount";
            txtBilledAt.DataField = "BilledAt";
            txtDueAt.DataField = "billingDueAt";
            txtScheduledPaymentKey.DataField = "ScheduledPaymentKey";
            txtBillingAmountTotal.DataField = "BillingAmount";
            txtScheduledPaymentAmtTotal.DataField = "PaymentAmount";
            txtOffsetAmountTotal.DataField = "OffsetAmount";
            txtRemainAmountTotal.DataField = "RemainAmount";

            txtBillingAmount.OutputFormat = displayFieldString;
            txtScheduledPaymentAmt.OutputFormat = displayFieldString;
            txtOffsetAmount.OutputFormat = displayFieldString;
            txtRemainAmount.OutputFormat = displayFieldString;

            txtBillingAmountTotal.OutputFormat = displayFieldString;
            txtScheduledPaymentAmtTotal.OutputFormat = displayFieldString;
            txtOffsetAmountTotal.OutputFormat = displayFieldString;
            txtRemainAmountTotal.OutputFormat = displayFieldString;
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            FormatDateTime();
        }

        private void FormatDateTime()
        {
            try
            {
                if (txtBilledAt.Text.Substring(0, 10) == "0001/01/01")
                {
                    txtBilledAt.Text = "";
                }

                if (txtDueAt.Text.Substring(0, 10) == "0001/01/01")
                {
                    txtDueAt.Text = "";
                }
            }
            catch
            {

            }
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }
    }
}
