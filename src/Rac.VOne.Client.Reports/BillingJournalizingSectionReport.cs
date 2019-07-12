using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingJournalizingSectionReport の概要の説明です。
    /// </summary>
    public partial class BillingJournalizingSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private bool ForeignCurrency;
        private string DisplayFormat = "#,##0";
        List<Web.Models.BillingJournalizing> ExtractList = null;

        public BillingJournalizingSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyCode.Text = companycode + " " + companyname;
        }

        public void SetData(List<Web.Models.BillingJournalizing> GeneralResult, bool UseForeignCurrency, int PrecisionNum, List<ColumnNameSetting> ColumnNameList)
        {
            var columnName1 = ColumnNameList.Exists(x => x.TableName == "Billing" && x.ColumnName == "Note1")
                ? ColumnNameList.Find(x => x.TableName == "Billing" && x.ColumnName == "Note1")
                : new ColumnNameSetting() { TableName = "Billing", ColumnName = "Note4", OriginalName = "備考" };

            var note1 = columnName1.DisplayColumnName;

            lblNote.Text = note1;

            ExtractList = GeneralResult;
            ForeignCurrency = UseForeignCurrency;
            int Precision = PrecisionNum;
            if (ForeignCurrency)
            {
                DisplayFormat += "." + new string('0', Precision);
            }
            DataSource = new BindingSource(GeneralResult, null);

            txtBilledAt.DataField = "BilledAt";
            txtSlipNumber.DataField = "SlipNumber";
            txtDebitDepartmentCode.DataField = "DebitDepartmentCode";
            txtDebitDepartmentName.DataField = "DebitDepartmentName";
            txtDebitAccTitleCode.DataField = "DebitAccountTitleCode";
            txtDebitAccTitleName.DataField = "DebitAccountTitleName";
            txtDebitSubCode.DataField = "DebitSubCode";
            txtDebitSubName.DataField = "DebitSubName";
            txtCreditDepartmentCode.DataField = "CreditDepartmentCode";
            txtCreditDepartmentName.DataField = "CreditDepartmentName";
            txtCreditAccountTitleCode.DataField = "CreditAccountTitleCode";
            txtCreditAccTitleName.DataField = "CreditAccountTitleName";
            txtCreditSubCode.DataField = "CreditSubCode";
            txtCreditSubName.DataField = "CreditSubName";
            txtAmount.DataField = "BillingAmount";
            txtAmount.OutputFormat = DisplayFormat;
            txtNote.DataField = "Note";
            txtCustomerCode.DataField = "CustomerCode";
            txtCustomerName.DataField = "CustomerName";
            txtInvoiceCode.DataField = "InvoiceCode";
            txtCurrencyCode.DataField = "CurrencyCode";
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            txtAmount.OutputFormat = DisplayFormat;
            decimal billingAmount = 0M;
            ExtractList.ForEach(x => billingAmount += x.BillingAmount);
            lblSum.Text = billingAmount.ToString(DisplayFormat);
            lblNumber.Text = string.Concat(ExtractList.Count.ToString("#,##0"), " 件");
        }

        private void BillingJournalizingSectionReport_DataInitialize(object sender, EventArgs e)
        {
            if (!ForeignCurrency)
            {
                lineHeaderHorInvoiceCode.Visible = false;
                txtCurrencyCode.Visible = false;
                txtInvoiceCode.Height += txtCurrencyCode.Height;
                lblCurrencyCode.Visible = false;
                lblInvoiceCode.Height += lblInvoiceCode.Height;
            }

        }
    }
}
