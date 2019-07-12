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

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerSectionReport の概要の説明です。
    /// </summary>
    public partial class CustomerSectionReport : GrapeCity.ActiveReports.SectionReport, IMasterSectionReport<Customer>
    {

        public CustomerSectionReport()
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

        public void SetData(IEnumerable<Customer> generalresult)
        {

            DataSource = new BindingSource(generalresult, null);

            txtCode.DataField = "Code";
            txtIsParent.DataField = "IsParentText";
            txtCustomerName.DataField = "Name";
            txtStaffName.DataField = "StaffName";
            txtKana.DataField = "Kana";
            txtNote.DataField = "Note";
            txtShareTransferFee.DataField = "ShareTransferFeeText";
            txtClosingDay.DataField = "ClosingDayText";
            txtCreditLimit.DataField = "CreditLimit";
            txtCollectOffsetDate.DataField = "CollectOffsetText";
            txtCategoryName.DataField = "CollectCategoryName";
            txtPostalCode.DataField = "PostalCode";
            txtAddress1.DataField = "Address1";
            txtAddress2.DataField = "Address2";
            txtTel.DataField = "Tel";
            txtFax.DataField = "Fax";
        }

        private void detail_Format(object sender, EventArgs e)
        {
            line1.Visible = false;
            line14.Visible = false;
            line15.Visible = false;
            line16.Visible = false;
            line17.Visible = false;
            line18.Visible = false;
            line21.Visible = false;
        }

        private void detail_BeforePrint(object sender, EventArgs e)
        {
            line1.Visible = true;
            line14.Visible = true;
            line15.Visible = true;
            line16.Visible = true;
            line17.Visible = true;
            line18.Visible = true;
            line21.Visible = true;
        }
    }
}
