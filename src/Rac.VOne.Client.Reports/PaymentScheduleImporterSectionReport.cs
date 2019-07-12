using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// PaymentScheduleImporterSectionReport の概要の説明です。
    /// </summary>
    public partial class PaymentScheduleImporterSectionReport : GrapeCity.ActiveReports.SectionReport
    {

        public PaymentScheduleImporterSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }
        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblcompanyName.Text = companycode + " " + companyname;
        }

        public void SetData(List<PaymentSchedule> generalresult, bool flag , int foreignflag, string note1)
        {
            if (flag)
            {
                lblTitle.Text = "入金予定フリーインポーター　取込可能データ一覧";
            }
            else if (flag == false)
            {
                lblTitle.Text = "入金予定フリーインポーター　取込不可能データ一覧";
            }
            if (foreignflag == 0)
            {
                lineCurrency.Visible = false;
                lineCurrencyHeader.Visible = false;
                lblCurrencyCode.Visible = false;
                txtCurrencyCode.Visible = false;
                lblCustomerCode.Text = "得意先コード";
                lblCustomerCode.Width += lblCurrencyCode.Width;
                txtCustomerCode.Width += txtCurrencyCode.Width;
            }
            else
            {
                lineCurrency.Visible = true;
                lineCurrencyHeader.Visible = true;
            }
            if (!string.IsNullOrWhiteSpace(note1))
            {
                lblNote.Text = note1;
            }
            DataSource = new BindingSource(generalresult, null);
            txtInvoiceCode.DataField = "InvoiceCode";
            txtCompanyCode.DataField = "CompanyCode";
            txtBilledAt.DataField = "BilledAt";
            txtDueAt.DataField = "DueAt";
            txtSalesAt.DataField = "SalesAt";
            txtClosingAt.DataField = "ClosingAt";
            txtBillingAmount.DataField = "BillingAmount";
            txtSubtractAmt.DataField = "DueAmount";
            txtCustomerCode.DataField = "CustomerCode";
            txtCurrencyCode.DataField = "CurrencyCode";
            txtDebitAccountTitleCode.DataField = "DebitAccountTitleCode";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtBillingCategoryCode.DataField = "BillingCategoryCode";
            txtNote1.DataField = "Note1";
            lblTotalAmount.Text = generalresult.Count.ToString("#,##0") + "件";
        }
    }
}
