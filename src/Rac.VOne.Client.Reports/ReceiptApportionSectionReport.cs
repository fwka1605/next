using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    public partial class ReceiptApportionSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private int sectionFlg = 0;
        private int precision = 0;
        private string displayFieldString = "#,###,###,###,##0";

        public ReceiptApportionSectionReport()
        {
            InitializeComponent();
        }

        public void SetBasicPageSetting(string companycode, string companyname)
        {
            lblcompanycode.Text = companycode + " " + companyname;
        }

        public void SetHeaderSetting(ReceiptHeader result, string category)
        {
            lblBankInfo.Text = result.BankName;
            lblBranchInfo.Text = result.BranchName;
            lblBankAccountType.Text = result.AccountTypeName;
            lblAccountNo.Text = result.AccountNumber;
            lblCreateAt.Text = result.CreateAt.ToString();
            lblCategory.Text = category;
        }

        public void SetPageDataSetting(List<ReceiptApportion> receiptList, int useSection, int precision)
        {
            this.sectionFlg = useSection;
            this.precision = precision;

            DataSource = new BindingSource(receiptList.ToArray(), null);
            cbxExcludeFlg.DataField = "ExcludeFlag";
            txtExclude.DataField = "CustomerName";
            txtExcludeAmount.DataField = "ExcludeAmount";
            txtPayerName.DataField = "PayerName";
            txtReceiptAmount.DataField = "ReceiptAmount";
            txtSection.DataField = "SectionName";
            txtSourceBankName.DataField = "SourceBankName";
            txtSourceBranchName.DataField = "SourceBranchName";
            txtWorkDay.DataField = "WorkDay";
            txtRecordTime.DataField = "RecordedAt";
        }

        private void ReceiptApportionReport_DataInitialize(object sender, EventArgs e)
        {
           if (sectionFlg == 0)
            {
                lblSection.Width = 0;
                lblSection.Visible = false;
                lblPayerName.Location = new System.Drawing.PointF(lblSection.Location.X, lblSection.Location.Y);
                lblPayerName.Width = float.Parse("2.4");
                txtSection.Width = 0;
                txtSection.Visible = false;
                txtPayerName.Location = new System.Drawing.PointF(txtSection.Location.X, txtSection.Location.Y);
                txtPayerName.Width = float.Parse("2.4");
                line12.Visible = false;
                line14.Visible = false;
            }
        }

        private void ReceiptApportionSectionReport_ReportStart(object sender, EventArgs e)
        {
            if (precision > 0)
            {
                displayFieldString += "." + new string('0', precision);
            }
            txtExcludeAmount.OutputFormat = displayFieldString;
            txtReceiptAmount.OutputFormat = displayFieldString;
        }
    }
}