using System.Windows.Forms;
using System.Collections.Generic;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingImporterNewCustomerSectionReport の概要の説明です。
    /// </summary>
    public partial class BillingImporterNewCustomerSectionReport : GrapeCity.ActiveReports.SectionReport
    {

        public BillingImporterNewCustomerSectionReport()
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

        public void SetData(List<BillingImport> generalresult, string reportTitle, bool useForeignCurrency, string Note1)
        {
            lblTitle.Text = reportTitle;
            if (!string.IsNullOrWhiteSpace(Note1))
            {
                lblNote.Text = Note1;
            }

            DataSource = new BindingSource(generalresult, null);
            txtCustomerCode.DataField = "CustomerCode";
            txtCurrencyCode.DataField = "CurrencyCode";
            txtCustomerName.DataField = "CustomerName";
            txtCustomerKana.DataField = "CustomerKana";
            txtInvoiceCode.DataField = "InvoiceCode";
            txtContractNumber.DataField = "ContractNumber";
            txtCompanyCode.DataField = "CompanyCode";
            txtTaxClassId.DataField = "TaxClassIdForPrint";
            txtBilledAt.DataField = "BilledAtForPrint";
            txtDepartmentCode.DataField = "DepartmentCode";
            txtBillAmount.DataField = "BillingAmountForPrint";
            txtTaxAmount.DataField = "TaxAmountForPrint";
            txtDueAt.DataField = "DueAtForPrint";
            txtSaleAt.DataField = "SaleAtForPrint";
            txtClosingAt.DataField = "ClosingAtForPrint";
            txtStaffCode.DataField = "StaffCode";
            txtBillingCategoryCode.DataField = "BillingCategoryCode";
            txtCollectCategoryCode.DataField = "CollectCategoryCode";
            txtStaffCode.DataField = "StaffCode";
            txtNote1.DataField = "Note1";
            txtPrice.DataField = "PriceForPrint";
            lblTotalAmount.Text = generalresult.Count.ToString() + " 件";
            if (!useForeignCurrency)
                lblCurrencyCode.Text = string.Empty;
            txtCurrencyCode.Visible = useForeignCurrency;

        }

    }
}
