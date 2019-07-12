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
    /// IndividualClearSectionReport の概要の説明です。
    /// </summary>
    public partial class MatchingIndividualSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private int precision;
        public MatchingIndividualSectionReport()
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

        public void SetAmountSetting(int noOfPre, string TaxError, string TaxPayError, string BankTransferFee, string DiscountTotal, int UseDiscount)
        {
            precision = noOfPre;
            if (TaxError == "")
            {
                txtBillingTaxDifference.Text = "0";
            }
            else
            {
                txtBillingTaxDifference.Text = TaxError;
            }

            if (BankTransferFee == "")
            {
                txtBankTransferFee.Text = "0";
            }
            else
            {
                txtBankTransferFee.Text = BankTransferFee;
            }
            if (TaxPayError == "")
            {
                txtReceiptTaxDifference.Text = "0";
            }
            else
            {
                txtReceiptTaxDifference.Text = TaxPayError;
            }

            if (UseDiscount == 0)
            {
                lnDiscountAmt.Visible = false;
                lblDiscountAmount.Visible = false;
                txtBillingDiscountAmount.Visible = false;
            }
            else
            {
                txtBillingDiscountAmount.Text = DiscountTotal;
            }
            
        }

        private void FormatDateTime()
        {
            if (!string.IsNullOrEmpty(txtRecordedAt.Text) &&  txtRecordedAt.Text!= null && txtRecordedAt.Text.Substring(0, 10) == "0001/01/01")
            {
                txtRecordedAt.Text = "";
            }

            if (!string.IsNullOrEmpty(txtBillingBilledAt.Text) && txtBillingBilledAt.Text!=null && txtBillingBilledAt.Text.Substring(0, 10) == "0001/01/01")
            {
                txtBillingBilledAt.Text = "";
            }

            if (!string.IsNullOrEmpty(txtBillingDueAt.Text) && txtBillingDueAt.Text != null &&  txtBillingDueAt.Text.Substring(0, 10) == "0001/01/01")
            {
                txtBillingDueAt.Text = "";
            }
        }

        private void NumberFormat()
        {
            if (txtReceiptAmount.Text == "0" || txtReceiptAmount.Text == null)
            {
                txtReceiptAmount.Text = "";
            }
            if (txtReceiptRemainAmount.Text == "0" || txtReceiptRemainAmount.Text == null)
            {
                txtReceiptRemainAmount.Text = "";
            }
        }

        public void SetPageDataSetting(List<ExportMatchingIndividual> PrintInfo,List<GridSetting> BillingGridInfo, List<GridSetting> ReceiptGridInfo)
        {
            DataSource = new BindingSource(PrintInfo.ToArray(), null);
            int BillingCount = PrintInfo.Where(x => x.Id != 0).Count();
            int ReceiptCount = PrintInfo.Where(x => x.ReceiptId != 0 || (x.NettingId!=0 && x.NettingId != null)).Count();
            foreach (GridSetting billingGrid in BillingGridInfo)
            {
                switch (billingGrid.ColumnName)
                {
                    case "Note1": lblBillingRemark.Text = billingGrid.ColumnNameJp; break;
                }
            }
            foreach (GridSetting receiptGrid in ReceiptGridInfo)
            {
                switch (receiptGrid.ColumnName)
                {
                    case "Note1": lblReceiptRemark.Text = receiptGrid.ColumnNameJp; break;
                }
            }

            string displayFormat = "#,##0";
            if (precision > 0)
            {
                displayFormat += "." + new string('0', precision);
            }

            //Billing
            txtCustomerCode.DataField = nameof(ExportMatchingIndividual.CustomerCode);
            txtCustomerName.DataField = nameof(ExportMatchingIndividual.CustomerName);
            txtBillingCategoryName.DataField = nameof(ExportMatchingIndividual.BillingCategoryCodeAndName);
            txtInvoiceNo.DataField = nameof(ExportMatchingIndividual.InvoiceCode);
            txtBillingBilledAt.DataField = nameof(ExportMatchingIndividual.BilledAt);
            txtBillingDueAt.DataField = nameof(ExportMatchingIndividual.DueAt);
            txtBillingDepartment.DataField = nameof(ExportMatchingIndividual.DepartmentName);
            txtBillingAmount.DataField = nameof(ExportMatchingIndividual.BillingAmount);
            txtBillingRemainAmount.DataField = nameof(ExportMatchingIndividual.RemainAmount);
            txtMatchingAmount.DataField = nameof(ExportMatchingIndividual.TargetAmount);
            txtBillingNote1.DataField = nameof(ExportMatchingIndividual.Note1);

            txtBillingAmount.OutputFormat = displayFormat;
            txtBillingRemainAmount.OutputFormat = displayFormat;
            txtMatchingAmount.OutputFormat = displayFormat;

            //Receipt
            txtPayerName.DataField = nameof(ExportMatchingIndividual.PayerName);
            txtSourceBankBranchName.DataField = nameof(ExportMatchingIndividual.SourceBank);
            txtRecordedAt.DataField = nameof(ExportMatchingIndividual.RecordedAt);
            txtAccountType.DataField = nameof(ExportMatchingIndividual.AccountTypeName);
            txtReceiptCateoryName.DataField = nameof(ExportMatchingIndividual.ReceiptCategroyCodeAndName);
            txtAccountNumber.DataField = nameof(ExportMatchingIndividual.AccountNumber);
            txtReceiptDueAt.DataField = nameof(ExportMatchingIndividual.ReceiptDueAt);
            txtReceiptAmount.DataField = nameof(ExportMatchingIndividual.ReceiptAmount);
            txtReceiptRemainAmount.DataField = nameof(ExportMatchingIndividual.ReceiptRemainAmount);
            txtReceiptNote1.DataField = nameof(ExportMatchingIndividual.ReceiptNote1);

            txtReceiptAmount.OutputFormat = displayFormat;
            txtReceiptRemainAmount.OutputFormat = displayFormat;

            //Total
            txtBillingCount.Text = BillingCount.ToString();
            txtBillingTotal.DataField = nameof(ExportMatchingIndividual.BillingAmount);
            txtReceiptCount.Text = ReceiptCount.ToString();
            txtReceiptTotal.DataField = nameof(ExportMatchingIndividual.ReceiptAmount);

            txtBillingTotal.OutputFormat = displayFormat;
            txtReceiptTotal.OutputFormat = displayFormat;
            txtBillingTaxDifference.OutputFormat = displayFormat;
            txtBankTransferFee.OutputFormat = displayFormat;
            txtReceiptTaxDifference.OutputFormat = displayFormat;
            txtBillingDiscountAmount.OutputFormat = displayFormat;

        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            FormatDateTime();
            NumberFormat();
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }
    }
}
