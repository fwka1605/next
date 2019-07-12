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
    /// ReceiptSectionTransferSectionReport の概要の説明です。
    /// </summary>
    public partial class ReceiptSectionTransferSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private bool ForeignCurrency;
        private string DisplayFormat = "#,##0";

        public ReceiptSectionTransferSectionReport()
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

        public void SetData(List<ReceiptSectionTransfer> GeneralResult,bool UseForeignCurrency, int Precision)
        {
            ForeignCurrency = UseForeignCurrency;
            if (ForeignCurrency)
            {
                DisplayFormat += "." + new string('0', Precision);
            }
            DataSource = new BindingSource(GeneralResult, null);

            txtId.DataField = "SourceReceiptId";
            txtRecordedAt.DataField = "RecordedAt";
            txtDueAt.DataField = "DueAt";
            txtCategoryCodeName.DataField = "CategoryCodeName";
            txtInputType.DataField = "InputTypeCodeName";
            txtPayerName.DataField = "PayerName";
            txtNote1.DataField = "Note1";
            txtSourceAmount.DataField = "SourceAmount";
            txtSourceAmount.OutputFormat = DisplayFormat;
            txtSourceSectionCode.DataField = "SourceSectionCode";
            txtSourceSectionName.DataField = "SourceSectionName";
            txtDestinationSectionCode.DataField = "DestinationSectionCode";
            txtDestinationSectionName.DataField = "DestinationSectionName";
            txtDestinationAmount.DataField = "DestinationAmount";
            txtDestinationAmount.OutputFormat = DisplayFormat;
            txtCreateAt.DataField = "CreateAt";
            txtMemo.DataField = "Memo";
            txtLoginUserCode.DataField = "LoginUserCode";
            txtLoginUserName.DataField = "LoginUserName";
            txtCurrencyCode.DataField = "CurrencyCode";
        }

        private void ReceiptSectionTransferSectionReport_DataInitialize(object sender, EventArgs e)
        {
            if (!ForeignCurrency)
            {
                txtCurrencyCode.Visible = false;
                txtId.Height += txtCurrencyCode.Height;
                lblCurrencyCode.Visible = false;
                lblId.Height += lblCurrencyCode.Height;
            }
        }
    }
}
