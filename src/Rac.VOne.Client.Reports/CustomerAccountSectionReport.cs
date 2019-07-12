using System;
using System.Drawing;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerAccountSectionReport の概要の説明です。
    /// </summary>
    public partial class CustomerAccountSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        private bool UsePublishInvoice { get; set; }
        private bool UseReminder { get; set; }
        public CustomerAccountSectionReport()
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

        public void SetData(List<Customer> generalresult, bool usePublishInvoice, bool useReminder)
        {

            UsePublishInvoice = usePublishInvoice;
            UseReminder = useReminder;

            DataSource = new BindingSource(generalresult, null);

            txtCustomerCode.DataField = "Code";
            txtCustomerName.DataField = "Name";
            txtKana.DataField = "Kana";
            txtShareTransferFee.DataField = "ShareTransferFeeText";
            cbxIsParent.DataField = "IsParent";
            cbxUseFeeLearning.DataField = "UseFeeLearning";
            cbxUseKanaLearning.DataField = "UseKanaLearning";
            cbxUseFeeTolerance.DataField = "UseFeeTolerance";
            cbxPrioritizeMatchingIndividually.DataField = "PrioritizeMatchingIndividually";
            cbxExcludeInvoicePublish.DataField = "ExcludeInvoicePublish";
            cbxExcludeReminderPublish.DataField = "ExcludeReminderPublish";
            txtHolidayFlag.DataField = "HolidayFlagText";
            txtStaffName.DataField = "StaffName";
            txtPostalCode.DataField = "PostalCode";
            txtAddress.DataField = "Address";
            txtTel.DataField = "Tel";
            txtFax.DataField = "Fax";
            txtDestinationDepartmentName.DataField = "DestinationDepartmentName";
            txtCustomerStaffName.DataField = "CustomerStaffName";
            txtHonorific.DataField = "Honorific";
            txtNote.DataField = "Note";
            txtCollectCategory.DataField = "CollectCategoryName";
            txtClosingDay.DataField = "ClosingDayText";
            txtCollectOffsetDate.DataField = "CollectOffsetText";
            txtSightOfBill.DataField = "SightOfBill";
            txtThresholdValue.DataField = "ThresholdValue";
            txtLessThanCollectCategory.DataField = "LessThanCollectCategoryName";
            txtCategoryName1.DataField = "GreaterThanCollectCategoryName1";
            txtGreaterThanRate1.DataField = "GreaterThanRateOneText";
            txtGreaterThanRoundingMode1.DataField = "GreaterThanRoundingModeOneText";
            txtSightOfBill1.DataField = "GreaterThanSightOfBill1";
            txtCategoryName2.DataField = "GreaterThanCollectCategoryName2";
            txtGreaterThanRate2.DataField = "GreaterThanRateTwoText";
            txtGreaterThanRoundingMode2.DataField = "GreaterThanRoundingModeTwoText";
            txtSightOfBill2.DataField = "GreaterThanSightOfBill2";
            txtCategoryName3.DataField = "GreaterThanCollectCategoryName3";
            txtGreaterThanRate3.DataField = "GreaterThanRateThreeText";
            txtGreaterThanRoundingMode3.DataField = "GreaterThanRoundingModeThreeText";
            txtSightOfBill3.DataField = "GreaterThanSightOfBill3";
            txtDensaiCode.DataField = "DensaiCode";
            txtCreditCode.DataField = "CreditCode";
            txtCreditLimit.DataField = "CreditLimit";
            txtCreditRank.DataField = "CreditRank";
            txtExclusiveBankCode.DataField = "ExclusiveBankCode";
            txtExclusiveBankName.DataField = "ExclusiveBankName";
            txtExclusiveBranchCode.DataField = "ExclusiveBranchCode";
            txtExclusiveAccNumberUp.DataField = "PrefixExclusiveAccountNumber";
            txtExclusiveBranchName.DataField = "ExclusiveBranchName";
            txtExclusiveAccNumberDown.DataField = "SuffixExclusiveAccountNumber";
            txtExclusiveAccType.DataField = "ExclusiveAccountTypeText";
            txtTransferBankCode.DataField = "TransferBankCode";
            txtTransferBankName.DataField = "TransferBankName";
            txtTransferBranchCode.DataField = "TransferBranchCode";
            txtTransferBranchName.DataField = "TransferBranchName";
            txtTransferAccNumber.DataField = "TransferAccountNumber";
            txtTransferAccountType.DataField = "TransferAccountTypeText";
            txtCompanyBankInfo1.DataField = "CompanyBankInfoText1";
            txtCompanyBankInfo2.DataField = "CompanyBankInfoText2";
            txtCompanyBankInfo3.DataField = "CompanyBankInfoText3";
            txtUpdateAt.DataField = "UpdateAt";
            txtLastUpdate.DataField = "LastUpdateUser";
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            FormatThresholdValue();
            FormatSightOfBill();
            FormatSightOfBill1();
            FormatSightOfBill2();
            FormatSightOfBill3();
            FormatCreditLimit();
            SetOptionControls();
        }
        private void FormatThresholdValue()
        {
            if (Convert.ToDecimal(txtThresholdValue.Text) == 0M)
            {
                txtThresholdValue.Text = string.Empty;
                return;
            }
            txtThresholdValue.Text = txtThresholdValue.Text.Split('.')[0];
            txtThresholdValue.Text = string.Concat(txtThresholdValue.Text, "円");
        }
        private void FormatSightOfBill()
        {
            if (string.IsNullOrEmpty(txtSightOfBill.Text) || txtSightOfBill.Text == "0")
            {
                txtSightOfBill.Text = "";
            }
            else
            {
                txtSightOfBill.Text = string.Concat(txtSightOfBill.Text, "日");
            }
        }
        private void FormatSightOfBill1()
        {
            if (string.IsNullOrEmpty(txtSightOfBill1.Text) || txtSightOfBill1.Text == "0")
            {
                txtSightOfBill1.Text = "";
            }
            else
            {
                txtSightOfBill1.Text = string.Concat(txtSightOfBill1.Text, "日");
            }

        }
        private void FormatSightOfBill2()
        {
            if (string.IsNullOrEmpty(txtSightOfBill2.Text) || txtSightOfBill2.Text == "0")
            {
                txtSightOfBill2.Text = "";
            }
            else
            {
                txtSightOfBill2.Text = string.Concat(txtSightOfBill2.Text, "日");
            }

        }
        private void FormatSightOfBill3()
        {
            if (string.IsNullOrEmpty(txtSightOfBill3.Text) || txtSightOfBill3.Text == "0")
            {
                txtSightOfBill3.Text = "";
            }
            else
            {
                txtSightOfBill3.Text = string.Concat(txtSightOfBill3.Text, "日");
            }
        }
        private void FormatCreditLimit()
        {
            if (Convert.ToDecimal(txtCreditLimit.Text) == 0M)
            {
                txtCreditLimit.Text = string.Empty;
                return;
            }
            txtCreditLimit.Text = txtCreditLimit.Text.Split('.')[0];
            txtCreditLimit.Text = string.Concat(txtCreditLimit.Text, "万円");
        }

        private void SetOptionControls()
        {
            cbxExcludeInvoicePublish.Visible = UsePublishInvoice;
            cbxExcludeReminderPublish.Visible = UseReminder;
            if (!UsePublishInvoice)
                cbxExcludeReminderPublish.Location = new PointF(cbxExcludeInvoicePublish.Location.X, cbxExcludeInvoicePublish.Location.Y);
        }

        private void FormatAddress()
        {
            var address1 = Convert.ToString(Fields["Address1"].Value);
            var address2 = Convert.ToString(Fields["Address2"].Value);

            if (string.IsNullOrEmpty(address1) && string.IsNullOrEmpty(address2))
                Fields["Address"].Value = string.Empty;
            else
                Fields["Address"].Value = address1 + " " + address2;
        }

        private void FormatExclusiveAccountNumber()
        {
            var exclusiveAccountNumber = Convert.ToString(Fields["ExclusiveAccountNumber"].Value);

            if (string.IsNullOrEmpty(exclusiveAccountNumber))
            {
                Fields["PrefixExclusiveAccountNumber"].Value = string.Empty;
                Fields["SuffixExclusiveAccountNumber"].Value = string.Empty;
                return;
            }
            else
            {
                Fields["PrefixExclusiveAccountNumber"].Value = (exclusiveAccountNumber.Length < 3) ? exclusiveAccountNumber.Substring(0) : exclusiveAccountNumber.Substring(0,3);
                Fields["SuffixExclusiveAccountNumber"].Value = (exclusiveAccountNumber.Length < 4) ? string.Empty : exclusiveAccountNumber.Substring(4);
            }
        }

        private void FormatCompanyBankInfo()
        {
            var counter = 1;
            for(int i = 1; i <= 3; i++)
            {
                if(Convert.ToInt16(Fields["ReceiveAccountId" + i].Value) == 1)
                {
                    Fields["CompanyBankInfoText" + counter].Value = Convert.ToString(Fields["CompanyBankInfo" + i].Value);
                    counter++;
                }
            }

            while (counter <= 3)
            {
                Fields["CompanyBankInfoText" + counter].Value = string.Empty;
                counter++;
            }
        }

        private void CustomerAccountSectionReport_DataInitialize(object sender, EventArgs e)
        {
            Fields.Add("Address");
            Fields.Add("PrefixExclusiveAccountNumber");
            Fields.Add("SuffixExclusiveAccountNumber");
            Fields.Add("CompanyBankInfoText1");
            Fields.Add("CompanyBankInfoText2");
            Fields.Add("CompanyBankInfoText3");
        }

        private void CustomerAccountSectionReport_FetchData(object sender, FetchEventArgs eArgs)
        {
            FormatAddress();
            FormatExclusiveAccountNumber();
            FormatCompanyBankInfo();
        }
    }
}
