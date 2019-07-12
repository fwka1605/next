using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Windows.Forms;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingImporterSectionReport の概要の説明です。
    /// </summary>
    public partial class BillingImporterSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        public BillingImporterSectionReport()
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

        public void SetData(List<BillingImport> generalresult, string reportTitle, bool useForeignCurrency , string Note1)
        {
            lblTitle.Text = reportTitle;
            if (!string.IsNullOrWhiteSpace(Note1))
            {
                lblNote.Text = Note1;
            }

            DataSource = new BindingSource(generalresult, null);
            txtInvoiceCode.DataField = "InvoiceCode";
            txtCompanyCode.DataField = "CompanyCode";
            txtBilledAt.DataField = "BilledAtForPrint";
            txtCustomerCode.DataField = "CustomerCode";
            txtBillAmount.DataField = "BillingAmountForPrint";
            txtDueAt.DataField = "DueAtForPrint";
            txtSaleAt.DataField = "SaleAtForPrint"; 
            txtClosingAt.DataField = "ClosingAtForPrint";
            txtStaffCode.DataField = "StaffCode";
            txtBillingCategoryCode.DataField = "BillingCategoryCode";
            txtCollectCategoryCode.DataField = "CollectCategoryCode";
            txtContractNumber.DataField = "ContractNumber";
            txtTaxClassId.DataField = "TaxClassIdForPrint";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtAccountTitleCode.DataField = "DebitAccountTitleCode";
            txtTaxAmount.DataField = "TaxAmountForPrint";
            txtStaffCode.DataField = "StaffCode";           
            txtNote1.DataField = "Note1";
            txtCurrencyCode.DataField = "CurrencyCode";
            txtPrice.DataField = "PriceForPrint";
            lblTotalAmount.Text = generalresult.Count.ToString() + " 件";
            if (!useForeignCurrency)
            {
                lblCurrencyMoney.Text = "金額";
                txtCurrencyCode.Visible = false;
            }
        }
    }
}
