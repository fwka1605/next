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
    /// AllMatchingSectionReport の概要の説明です。
    /// </summary>
    public partial class MatchingSequentialSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private int useForeignCurrency = 0;
        private string displayFieldString = "#,###,###,###,##0";

        public MatchingSequentialSectionReport()
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

        public void SetPageDataSetting(List<Collation> collationList, bool timeSort,int useForeignCurrency, int Precision, CollationSearch cs)
        {
            this.useForeignCurrency = useForeignCurrency;

            if(useForeignCurrency == 1)
            {
                SetDisplayFormat(Precision);
            }

            DataSource = new BindingSource(collationList.ToArray(), null);
            txtChecked.DataField = "Checked";
            txtCurrency.DataField = "CurrencyCode";
            txtCustomerCode.DataField = "DispCustomerCode";
            txtCustomerName.DataField = "DispCustomerName";
            txtBillingCount.DataField = "DispBillingCount";
            txtBillingAmount.DataField = "DispBillingAmount";
            txtPayerName.DataField = "PayerName";
            txtReceiptCount.DataField = "DispReceiptCount";
            txtReceiptAmount.DataField = "DispReceiptAmount";
            txtFee.DataField = "ReportDispShareTransferFee";
            txtDifference.DataField = "DispDifferent";
        }

        private void AllMatchingSectionReport_ReportStart(object sender, EventArgs e)
        {
            if (useForeignCurrency == 0)
            {
                txtCurrency.Visible = false;
                lblCurrency.Visible = false;
                lineHeaderVerChecked.Visible = false;
                lineDetailVerChecked.Visible = false;

                lblCustomerCode.Location = new System.Drawing.PointF(lblCustomerCode.Location.X - lblCurrency.Width, lblCustomerCode.Location.Y);
                lineHeaderVerCurrency.Location = new System.Drawing.PointF(lineHeaderVerCurrency.Location.X - lblCurrency.Width, lineHeaderVerCurrency.Location.Y);

                txtCustomerCode.Location = new System.Drawing.PointF(txtCustomerCode.Location.X - txtCurrency.Width, txtCustomerCode.Location.Y);
                lineDetailVerCurrency.Location = new System.Drawing.PointF(lineDetailVerCurrency.Location.X - txtCurrency.Width, lineDetailVerCurrency.Location.Y);

                lblCustomerCode.Width += lblCurrency.Width;
                txtCustomerCode.Width += txtCurrency.Width;
                txtBillingAmount.OutputFormat = displayFieldString;
                txtReceiptAmount.OutputFormat = displayFieldString;
                txtDifference.OutputFormat = displayFieldString;

            } else
            {
                txtBillingAmount.OutputFormat = displayFieldString;
                txtReceiptAmount.OutputFormat = displayFieldString;
                txtDifference.OutputFormat = displayFieldString;
            }
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            if (txtChecked.Text == "True")
            {
                txtChecked.Text = "レ";

            } else if(txtChecked.Text == "False")
            {
                txtChecked.Text = "";
            }
        }

        private void SetDisplayFormat(int precision)
        {
            if (precision > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < precision; i++)
                {
                    displayFieldString += "0";
                }
            }
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }
    }
}
