using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ReceiptImporterSectionReport の概要の説明です。
    /// </summary>
    public partial class ReceiptImporterSectionReport : GrapeCity.ActiveReports.SectionReport
    {

        public ReceiptImporterSectionReport()
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

        public void SetData(List<ReceiptInput> generalresult, bool flag, int foreignflag, string note1)
        {
            if (flag)
            {
                lblTitle.Text = "入金フリーインポーター　取込可能データ一覧";
            }
            else if (flag == false)
            {
                lblTitle.Text = "入金フリーインポーター　取込不可能データ一覧";
            }
            if (foreignflag == 0)
            {
                var text = "入金額手形番号";
                lblCurrencyCode.Text = text.Replace("額", "額\n\n");
                txtCurrencyCode.DataField = "";
            }
            else
            {
                var text = "通貨 / 入金額手形番号";
                lblCurrencyCode.Text = text.Replace("額", "額\n\n");
                txtCurrencyCode.DataField = "CurrencyCode";
            }
            if ( !string.IsNullOrWhiteSpace(note1))
            {
                lblNote.Text = note1;
            }
            DataSource = new BindingSource(generalresult, null);

            txtCustomerCode.DataField = "CustomerCode";
            txtPayerName.DataField = "PayerName";
            txtRecordedAt.DataField = "RecordAtForPrint";
            txtDueAt.DataField = "DueAtForPrint";
            txtReceiptCategoryCode.DataField = "ReceiptCategoryCode";
            txtBillDrawAt.DataField = "BillDrawAtForPrint";
            txtReceiptAmount.DataField = "ReceiptAmountForPrint";
            txtBillNumber.DataField = "BillNumber";
            txtSourceBankName.DataField = "SourceBankName";
            txtBillBankCode.DataField = "BillBankCode";
            txtSourceBranchName.DataField = "SourceBranchName";
            txtBillBranchCode.DataField = "BillBranchCode";
            txtReceiptNote1.DataField = "Note1";
            txtReceiptBillDrawer.DataField = "BillDrawer";
            txtTotalAmount.Text = generalresult.Count.ToString("#,##0") + "件";
        }
    }
}
