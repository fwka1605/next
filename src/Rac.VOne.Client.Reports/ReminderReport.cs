using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ReminderReport の概要の説明です。
    /// </summary>
    public partial class ReminderReport : GrapeCity.ActiveReports.SectionReport
    {
        public ReminderTemplateSetting Template { get; set; }
        public bool CustomerReceiveAccount1 { get; set; }
        public bool CustomerReceiveAccount2 { get; set; }
        public bool CustomerReceiveAccount3 { get; set; }
        public string CompanyBankAccount1 { get; set; }
        public string CompanyBankAccount2 { get; set; }
        public string CompanyBankAccount3 { get; set; }

        public Action<int> TotalAmountPrintHandler { get; set; }
        private bool detailHeaderFirstPrint = true;
        public ReminderReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
            InitializeUserComponent();
        }

        public void HideDetail()
        {
            ghDetail.Visible = false;
            detail.Visible = false;
        }

        public void HideOutputNo()
        {
            txtOutputNoHeader.Visible = false;
            txtOutputNoDetail.Visible = false;
        }

        private void InitializeUserComponent()
        {
            ghReminder.BeforePrint += ghReminder_BeforePrint;
            ghDetail.BeforePrint += ghDetail_BeforePrint;
        }

        private void ghReminder_BeforePrint(object sender, EventArgs e)
        {
            var customerId = (int)Fields[nameof(ReminderBilling.CustomerId)].Value;
            txtTitle.Text = Template.Title;
            txtGreeting.Text = Template.Greeting;
            txtMainText.Text = Template.MainText;
            txtSubText.Text = Template.SubText;
            txtConclusion.Text = Template.Conclusion;

            PrintBankAccount();

            if (!string.IsNullOrWhiteSpace(txtCustomerPostalCode.Text))
            {
                txtCustomerPostalCode.Text = "〒" + txtCustomerPostalCode.Text;
            }

            if (string.IsNullOrWhiteSpace(txtStaffTel.Text))
            {
                lblTel.Visible = false;
            }
            if (string.IsNullOrWhiteSpace(txtStaffFax.Text))
            {
                lblFax.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(txtAccountName.Text))
            {
                lblAccountName.Visible = false;
            }
            if (string.IsNullOrWhiteSpace(txtBankInfo1.Text)
                && string.IsNullOrWhiteSpace(txtBankInfo2.Text)
                && string.IsNullOrWhiteSpace(txtBankInfo3.Text))
            {
                lblBank.Visible = false;
            }

            detailHeaderFirstPrint = true;
        }

        private void PrintBankAccount()
        {
            var ls = new List<string>();
            if (CustomerReceiveAccount1) ls.Add(CompanyBankAccount1);
            if (CustomerReceiveAccount2) ls.Add(CompanyBankAccount2);
            if (CustomerReceiveAccount3) ls.Add(CompanyBankAccount3);

            if (ls.Count > 0)
                txtBankInfo1.Text = ls[0];
            if (ls.Count > 1)
                txtBankInfo2.Text = ls[1];
            if (ls.Count > 2)
                txtBankInfo3.Text = ls[2];
        }

        private void ghDetail_BeforePrint(object sender, EventArgs e)
        {
            lblTotalAmount.Visible = detailHeaderFirstPrint;
            txtTotalAmount.Visible = detailHeaderFirstPrint;
            lineTotalAmountTop.Visible = detailHeaderFirstPrint;
            lineTotalAmountBottom.Visible = detailHeaderFirstPrint;
            lineTotalAmountLeft.Visible = detailHeaderFirstPrint;
            lineTotalAmountCenter.Visible = detailHeaderFirstPrint;
            lineTotalAmountRight.Visible = detailHeaderFirstPrint;

            if (detailHeaderFirstPrint && Fields[nameof(ReminderBilling.CustomerId)] != null)
            {
                var customerId = (int)Fields[nameof(ReminderBilling.CustomerId)].Value;
                TotalAmountPrintHandler?.Invoke(customerId);
                detailHeaderFirstPrint = false;
            }

            txtCustomerStaffNameDetail.Text = txtDestinationAddressee.Text;
        }

    }
}
