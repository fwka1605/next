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
    /// ReceiptJournalizingOutputSectionReport の概要の説明です。
    /// </summary>
    public partial class ReceiptJournalizingSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private bool foreignCurrency;
        public ReceiptJournalizingSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblCompanyCodeName.Text = companycode + " " + companyname;
        }

        public void SetData(List<Web.Models.ReceiptJournalizing> generalresult, int noOfpre, bool useForeginCurrency
                            , List<ColumnNameSetting> SettingInfo)
        {
            foreignCurrency = useForeginCurrency;
            DataSource = new BindingSource(generalresult, null);

            string displayFormat = "#,##0";
            if (noOfpre > 0)
            {
                displayFormat += "." + new string('0', noOfpre);
            }

            txtRecordedAt.DataField = "RecordedAt";
            txtSlipNumber.DataField = "SlipNumber";
            txtDebitDepartmentCode.DataField = "DebitDepartmentCode";
            txtDebitDepartmentName.DataField = "DebitDepartmentName";
            txtDebitAccountTitleCode.DataField = "DebitAccountTitleCode";
            txtDebitAccountTitleName.DataField = "DebitAccountTitleName";
            txtDebitSubCode.DataField = "DebitSubCode";
            txtDebitSubName.DataField = "DebitSubName";
            txtCreditDepartmentCode.DataField = "CreditDepartmentCode";
            txtCreditDepartmentName.DataField = "CreditDepartmentName";
            txtCreditAccountTitleCode.DataField = "CreditAccountTitleCode";
            txtCreditAccountTitleName.DataField = "CreditAccountTitleName";
            txtCreditSubCode.DataField = "CreditSubCode";
            txtCreditSubName.DataField = "CreditSubName";
            txtAmount.DataField = "Amount";
            txtNote.DataField = "Note";
            txtPayerName.DataField = "PayerName";
            txtBankCode.DataField = "BankCode";
            txtBankName.DataField = "BankName";
            txtBranchCode.DataField = "BranchCode";
            txtBranchName.DataField = "BranchName";
            txtAccountNumber.DataField = "AccountNumber";
            txtDueAt.DataField = "DueAt";
            txtCurrencyCode.DataField = "CurrencyCode";

            txtTotalCount.Text = generalresult.Count + "件";
            txtTotalCount.OutputFormat = displayFormat;

            txtAmount.OutputFormat = displayFormat;
            txtTotalAmount.OutputFormat = displayFormat;

            foreach (ColumnNameSetting gs in SettingInfo)
            {
                switch (gs.ColumnName)
                {
                    case "Note1": lblNote.Text = gs.DisplayColumnName; break;
                }
            }
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            if (foreignCurrency)
            {
                txtCurrencyCode.Visible = true;
            }
            else
            {
                txtCurrencyCode.Visible = false;
                txtDueAt.Height += txtCurrencyCode.Height;
            }
        }

        private void pageHeader_BeforePrint(object sender, EventArgs e)
        {
            if (foreignCurrency)
            {
                lblCurrencyCode.Visible = true;
                lineHeaderHorDueAt.Visible = true;
            }
            else
            {
                lblCurrencyCode.Visible = false;
                lineHeaderHorDueAt.Visible = false;
                lblDueAt.Height += lblCurrencyCode.Height;
            }
        }
    }
}
