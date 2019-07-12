using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Rac.VOne.Web.Models;
using System.Windows.Forms;
using System.Linq;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerLedgerSectionReport の概要の説明です。
    /// </summary>
    public partial class CustomerLedgerSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        /// <summary>
        /// 次のデータが何か把握するために private property として保持 index で利用
        /// </summary>
        private List<CustomerLedger> CusLedgerList { get; set; }
        /// <summary>
        /// 次データにアクセスするための index
        /// </summary>
        private int CurrentIndex { get; set; }

        public CustomerLedgerSearch SearchOption { private get; set; }

        private bool DisplayDepartment
        {
            get
            {
                return !SearchOption.RequireBillingSubtotal
                    && !SearchOption.RequireBillingInputIdSubotal
                    && SearchOption.DisplayDepartment;
            }
        }
        private bool DisplaySection
        {
            get { return !SearchOption.DoGroupReceipt && SearchOption.DisplaySection; }
        }

        private bool CollapseDepartmentColumn
        {
            get { return !DisplayDepartment && !DisplaySection; }
        }

        private bool CollapseInvoiceCodeColumn
        {
            get { return !SearchOption.DisplayInvoiceCode && !SearchOption.DisplayBillingCategory; }
        }

        public CustomerLedgerSectionReport()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        public void SetBasicPageSetting(string CompanyCode, string CompanyName)
        {
            lblCompanyCodeName.Text = CompanyCode + " " + CompanyName;
        }

        public void SetData(List<CustomerLedger> source,
            int precision)
        {
            CusLedgerList = source;

            if (!SearchOption.RequireMonthlyBreak)
                ghYearMonth.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.None;

            DataSource = new BindingSource(source, null);

            var format = "#,##0";
            if (precision > 0)
                format += "." + new string('0', precision);

            txtRecordedAt.DataField = "RecordedAt";
            txtCategoryName.DataField = "CategoryName";

            txtInvoiceCode.DataField = "InvoiceCode";
            txtDebitAccountTitleName.DataField = "DebitAccountTitleName";
            txtMatchingSymbolBilling.DataField = "MatchingSymbolBilling";
            txtBillingAmount.DataField = "BillingAmount";
            txtSlipTotal.DataField = "SlipTotal";
            txtReceiptAmount.DataField = "ReceiptAmount";
            txtMatchingSymbolReceipt.DataField = "MatchingSymbolReceipt";
            txtMatchingAmount.DataField = "MatchingAmount";
            txtRemainAmount.DataField = "RemainAmount";
            txtCustomerCode.DataField = "CustomerCode";
            txtCustomerName.DataField = "CustomerName";

            txtBillingAmount.OutputFormat = format;
            txtSlipTotal.OutputFormat = format;
            txtMatchingAmount.OutputFormat = format;
            txtRemainAmount.OutputFormat = format;
            txtReceiptAmount.OutputFormat = format;


            if (!SearchOption.DisplayBillingCategory)
            {
                lblCategoryName.Visible = false;
                txtCategoryName.Visible = false;
                lblRecordedAt.Height += lblCategoryName.Height;
                txtRecordedAt.Height += txtCategoryName.Height;
            }

            if (CollapseDepartmentColumn)
            {
                lblDepartmentName.Visible = false;
                txtDeptOrSecName.Visible = false;
                lblSectionName.Visible = false;
                lineHeaderVerRecordedAt.Visible = false;
                lineDetailVerRecordedAt.Visible = false;
                lblRecordedAt.Width += lblDepartmentName.Width;
                lblCategoryName.Width += lblDepartmentName.Width;
                txtRecordedAt.Width += txtDeptOrSecName.Width;
                txtCategoryName.Width += txtDeptOrSecName.Width;
            }
            else if (!DisplayDepartment)
            {
                lblDepartmentName.Visible = false;
                lblSectionName.Height += lblDepartmentName.Height;
                lblSectionName.Location = lblDepartmentName.Location;
            }
            else if (!DisplaySection)
            {
                lblSectionName.Visible = false;
                lblDepartmentName.Height += lblSectionName.Height;
            }

            if (CollapseInvoiceCodeColumn)
            {
                lblInvoiceCode.Visible = false;
                lblDebitAccountTitleName.Visible = false;
                txtInvoiceCode.Visible = false;
                txtDebitAccountTitleName.Visible = false;
                lineHeaderVerDepartmentCode.Visible = false;
                lineDetailVerDeptOrSecName.Visible = false;
                if (!DisplayDepartment && !DisplaySection)
                {
                    lblRecordedAt.Width += lblInvoiceCode.Width;
                    lblCategoryName.Width += lblInvoiceCode.Width;
                    txtRecordedAt.Width += txtInvoiceCode.Width;
                    txtCategoryName.Width += txtInvoiceCode.Width;
                }
                else
                {
                    lblSectionName.Width += lblInvoiceCode.Width;
                    lblDepartmentName.Width += lblInvoiceCode.Width;
                    txtDeptOrSecName.Width += lblInvoiceCode.Width;
                }
            }
            else if (!SearchOption.DisplayInvoiceCode)
            {
                lblInvoiceCode.Visible = false;
                txtInvoiceCode.Visible = false;
                lblDebitAccountTitleName.Height += lblInvoiceCode.Height;
                lblDebitAccountTitleName.Location = lblInvoiceCode.Location;
            }
            else if (!SearchOption.DisplayDebitAccountTitle)
            {
                lblDebitAccountTitleName.Visible = false;
                txtDebitAccountTitleName.Visible = false;
                lblInvoiceCode.Height += lblDebitAccountTitleName.Height;
            }

            if (!SearchOption.DisplaySlipTotal)
            {
                lblSlipTotal.Visible = false;
                txtSlipTotal.Visible = false;
                lineHeaderVerBillingAmount.Visible = false;
                lineDetailVerMatchingSymbolBilling.Visible = false;
                lblBillingAmount.Width += lblSlipTotal.Width;
                txtBillingAmount.Width += txtSlipTotal.Width;
            }

            if (!SearchOption.RequireReceiptData)
            {
                lblReceiptAmount.Visible = false;
                txtReceiptAmount.Visible = false;
                lineHeaderVerReceiptAmount.Visible = false;
                lineDetailVerReceiptAmount.Visible = false;
                lblMatchingAmount.Width += lblReceiptAmount.Width;
                lblMatchingAmount.Location = lblReceiptAmount.Location;
                txtMatchingAmount.Width += txtReceiptAmount.Width;
                txtMatchingAmount.Location = txtReceiptAmount.Location;
                txtMatchingSymbolReceipt.Location = txtReceiptAmount.Location;
            }

            if (!SearchOption.DisplayMatchingSymbol)
            {
                txtMatchingSymbolBilling.Visible = false;
                txtMatchingSymbolReceipt.Visible = false;
            }
        }

        private void detail_Format(object sender, EventArgs e)
        {
            var current = CusLedgerList[CurrentIndex];
            var recordType = current.RecordType;
            txtDeptOrSecName.Text =
                current.DataType == 1 ? current.DepartmentName :
                current.DataType == 2 ? current.SectionName :
                current.DataType == 3 ? current.SectionName : string.Empty;

            CurrentIndex++;
            if (recordType == CustomerLedger.RecordTypeAlias.MonthlySubtotalRecord
                || recordType == CustomerLedger.RecordTypeAlias.TotalRecord)
            {
                lineDetailVerRecordedAt.Visible = false;
                lineDetailVerDeptOrSecName.Visible = false;
                txtCaption.Visible = true;
                txtCaption.BringToFront();

                if (recordType == CustomerLedger.RecordTypeAlias.MonthlySubtotalRecord)
                    txtCaption.Text = $"{current.YearMonth:yyyy/MM} 合計";
                else
                    txtCaption.Text = " 合計";
                spBackColor.BackColor = Color.WhiteSmoke;
            }
            else
            {
                lineDetailVerRecordedAt.Visible = !CollapseDepartmentColumn;
                lineDetailVerDeptOrSecName.Visible = !CollapseInvoiceCodeColumn;
                txtCaption.Visible = false;
                if (recordType == CustomerLedger.RecordTypeAlias.CarryOverRecord)
                {
                    txtRecordedAt.Text = "繰越";
                }
                spBackColor.BackColor = Color.Transparent;
            }
        }

        private void pageFooter_BeforePrint(object sender, EventArgs e)
        {
            lblPageNumber.Text = (this.Document.Pages.Count + 1) + " / " + PageNumber;
        }

    }
}
