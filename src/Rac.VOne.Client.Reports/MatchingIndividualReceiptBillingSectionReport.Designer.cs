namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// IndividualClearSectionReport の概要の説明です。
    /// </summary>
    partial class MatchingIndividualReceiptBillingSectionReport
    {
        private GrapeCity.ActiveReports.SectionReportModel.Detail detail;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        #region ActiveReport Designer generated code
        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MatchingIndividualReceiptBillingSectionReport));
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.shpBilling = new GrapeCity.ActiveReports.SectionReportModel.Shape();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceNo = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingDepartment = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMatchingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPayerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSourceBankBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAccountType = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSeparate = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerReceiptCategoryName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerReceiptDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtReceiptCateoryName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtBillingCategoryName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailHorSeparate = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtBillingCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblDiscountAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingTaxDifference = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblBillingAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblBankTransferFee = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblReceiptTaxDifference = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankTransferFee = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptTaxDifference = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingDiscountAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingTaxDifference = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterHorBillingCount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorReceiptUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorReceiptLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterBillingLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorDiscountAmt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblOutputDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.riOutputDate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label11 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingCategoryName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceNo = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMatchingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingNote1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAccountType = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label16 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label17 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptCategoryName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblBillingDepartment = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptRemark = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorSeparate = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSeparate = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerReceiptCategoryname = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerReceiptDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblBillingInfo = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptInfo = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBankBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptCateoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDiscountAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingTaxDifference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankTransferFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptTaxDifference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankTransferFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptTaxDifference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDiscountAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingTaxDifference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riOutputDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMatchingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.shpBilling,
            this.txtCustomerCode,
            this.txtCustomerName,
            this.txtInvoiceNo,
            this.txtBillingBilledAt,
            this.txtBillingDueAt,
            this.txtBillingAmount,
            this.txtBillingDepartment,
            this.txtBillingRemainAmount,
            this.txtMatchingAmount,
            this.txtBillingNote1,
            this.txtPayerName,
            this.txtSourceBankBranchName,
            this.txtRecordedAt,
            this.txtAccountType,
            this.txtAccountNumber,
            this.txtReceiptDueAt,
            this.txtReceiptAmount,
            this.txtReceiptRemainAmount,
            this.txtReceiptNote1,
            this.lineDetailVerCustomerName,
            this.lineDetailVerBillingBilledAt,
            this.lineDetailVerBillingDueAt,
            this.lineDetailVerBillingAmount,
            this.lineDetailVerBillingRemainAmount,
            this.lineDetailVerSeparate,
            this.lineDetailVerReceiptCategoryName,
            this.lineDetailVerReceiptDueAt,
            this.lineDetailVerReceiptAmount,
            this.lineDetailVerPayerName,
            this.txtReceiptCateoryName,
            this.lineDetailVerRecordedAt,
            this.txtBillingCategoryName,
            this.lineDetailHorSeparate,
            this.lineDetailHorLower,
            this.lineDetailVerCustomerCode});
            this.detail.Height = 0.4507875F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // shpBilling
            // 
            this.shpBilling.BackColor = System.Drawing.Color.LightCyan;
            this.shpBilling.Height = 0.4429134F;
            this.shpBilling.Left = 4.814961F;
            this.shpBilling.LineColor = System.Drawing.Color.Transparent;
            this.shpBilling.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Transparent;
            this.shpBilling.LineWeight = 0F;
            this.shpBilling.Name = "shpBilling";
            this.shpBilling.RoundingRadius = new GrapeCity.ActiveReports.Controls.CornersRadius(10F, null, null, null, null);
            this.shpBilling.Top = 1.192093E-07F;
            this.shpBilling.Width = 5.782284F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2283465F;
            this.txtCustomerCode.Left = 4.814961F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 1.192093E-07F;
            this.txtCustomerCode.Width = 0.8858268F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.2283465F;
            this.txtCustomerName.Left = 5.700788F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtCustomerName.Text = null;
            this.txtCustomerName.Top = 1.192093E-07F;
            this.txtCustomerName.Width = 1.476378F;
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.Height = 0.2283465F;
            this.txtInvoiceNo.Left = 5.700788F;
            this.txtInvoiceNo.MultiLine = false;
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtInvoiceNo.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtInvoiceNo.Text = null;
            this.txtInvoiceNo.Top = 0.2212602F;
            this.txtInvoiceNo.Width = 1.476378F;
            // 
            // txtBillingBilledAt
            // 
            this.txtBillingBilledAt.Height = 0.2283465F;
            this.txtBillingBilledAt.Left = 7.177166F;
            this.txtBillingBilledAt.MultiLine = false;
            this.txtBillingBilledAt.Name = "txtBillingBilledAt";
            this.txtBillingBilledAt.OutputFormat = resources.GetString("txtBillingBilledAt.OutputFormat");
            this.txtBillingBilledAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingBilledAt.Text = null;
            this.txtBillingBilledAt.Top = 1.192093E-07F;
            this.txtBillingBilledAt.Width = 0.7133859F;
            // 
            // txtBillingDueAt
            // 
            this.txtBillingDueAt.Height = 0.2283465F;
            this.txtBillingDueAt.Left = 7.890552F;
            this.txtBillingDueAt.MultiLine = false;
            this.txtBillingDueAt.Name = "txtBillingDueAt";
            this.txtBillingDueAt.OutputFormat = resources.GetString("txtBillingDueAt.OutputFormat");
            this.txtBillingDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingDueAt.Text = null;
            this.txtBillingDueAt.Top = 1.192093E-07F;
            this.txtBillingDueAt.Width = 0.7133861F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.Height = 0.2283465F;
            this.txtBillingAmount.Left = 8.60433F;
            this.txtBillingAmount.MultiLine = false;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.OutputFormat = resources.GetString("txtBillingAmount.OutputFormat");
            this.txtBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingAmount.Text = null;
            this.txtBillingAmount.Top = 1.192093E-07F;
            this.txtBillingAmount.Width = 0.6641732F;
            // 
            // txtBillingDepartment
            // 
            this.txtBillingDepartment.Height = 0.2283465F;
            this.txtBillingDepartment.Left = 7.177166F;
            this.txtBillingDepartment.MultiLine = false;
            this.txtBillingDepartment.Name = "txtBillingDepartment";
            this.txtBillingDepartment.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingDepartment.Text = null;
            this.txtBillingDepartment.Top = 0.2212602F;
            this.txtBillingDepartment.Width = 1.427166F;
            // 
            // txtBillingRemainAmount
            // 
            this.txtBillingRemainAmount.Height = 0.2283465F;
            this.txtBillingRemainAmount.Left = 9.268505F;
            this.txtBillingRemainAmount.MultiLine = false;
            this.txtBillingRemainAmount.Name = "txtBillingRemainAmount";
            this.txtBillingRemainAmount.OutputFormat = resources.GetString("txtBillingRemainAmount.OutputFormat");
            this.txtBillingRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingRemainAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingRemainAmount.Text = null;
            this.txtBillingRemainAmount.Top = 1.192093E-07F;
            this.txtBillingRemainAmount.Width = 0.6641732F;
            // 
            // txtMatchingAmount
            // 
            this.txtMatchingAmount.Height = 0.2283465F;
            this.txtMatchingAmount.Left = 9.933071F;
            this.txtMatchingAmount.MultiLine = false;
            this.txtMatchingAmount.Name = "txtMatchingAmount";
            this.txtMatchingAmount.OutputFormat = resources.GetString("txtMatchingAmount.OutputFormat");
            this.txtMatchingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtMatchingAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMatchingAmount.Text = null;
            this.txtMatchingAmount.Top = 1.192093E-07F;
            this.txtMatchingAmount.Width = 0.6641731F;
            // 
            // txtBillingNote1
            // 
            this.txtBillingNote1.Height = 0.2283465F;
            this.txtBillingNote1.Left = 8.60433F;
            this.txtBillingNote1.MultiLine = false;
            this.txtBillingNote1.Name = "txtBillingNote1";
            this.txtBillingNote1.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingNote1.Text = null;
            this.txtBillingNote1.Top = 0.2212602F;
            this.txtBillingNote1.Width = 1.996063F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.2283465F;
            this.txtPayerName.Left = 0F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtPayerName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtPayerName.Text = null;
            this.txtPayerName.Top = 0F;
            this.txtPayerName.Width = 1.382961F;
            // 
            // txtSourceBankBranchName
            // 
            this.txtSourceBankBranchName.Height = 0.2283465F;
            this.txtSourceBankBranchName.Left = 0.0003937008F;
            this.txtSourceBankBranchName.MultiLine = false;
            this.txtSourceBankBranchName.Name = "txtSourceBankBranchName";
            this.txtSourceBankBranchName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtSourceBankBranchName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtSourceBankBranchName.Text = null;
            this.txtSourceBankBranchName.Top = 0.2212599F;
            this.txtSourceBankBranchName.Width = 1.382678F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.2283465F;
            this.txtRecordedAt.Left = 1.382961F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtRecordedAt.Text = null;
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.7133859F;
            // 
            // txtAccountType
            // 
            this.txtAccountType.Height = 0.2283465F;
            this.txtAccountType.Left = 1.383071F;
            this.txtAccountType.MultiLine = false;
            this.txtAccountType.Name = "txtAccountType";
            this.txtAccountType.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtAccountType.Text = null;
            this.txtAccountType.Top = 0.2212599F;
            this.txtAccountType.Width = 0.7133859F;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Height = 0.2283465F;
            this.txtAccountNumber.Left = 2.102362F;
            this.txtAccountNumber.MultiLine = false;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtAccountNumber.Text = null;
            this.txtAccountNumber.Top = 0.2212599F;
            this.txtAccountNumber.Width = 0.7133861F;
            // 
            // txtReceiptDueAt
            // 
            this.txtReceiptDueAt.Height = 0.2283465F;
            this.txtReceiptDueAt.Left = 2.815639F;
            this.txtReceiptDueAt.MultiLine = false;
            this.txtReceiptDueAt.Name = "txtReceiptDueAt";
            this.txtReceiptDueAt.OutputFormat = resources.GetString("txtReceiptDueAt.OutputFormat");
            this.txtReceiptDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptDueAt.Text = null;
            this.txtReceiptDueAt.Top = 0F;
            this.txtReceiptDueAt.Width = 0.7133861F;
            // 
            // txtReceiptAmount
            // 
            this.txtReceiptAmount.Height = 0.2283465F;
            this.txtReceiptAmount.Left = 3.529025F;
            this.txtReceiptAmount.MultiLine = false;
            this.txtReceiptAmount.Name = "txtReceiptAmount";
            this.txtReceiptAmount.OutputFormat = resources.GetString("txtReceiptAmount.OutputFormat");
            this.txtReceiptAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptAmount.Text = null;
            this.txtReceiptAmount.Top = 0F;
            this.txtReceiptAmount.Width = 0.5905512F;
            // 
            // txtReceiptRemainAmount
            // 
            this.txtReceiptRemainAmount.Height = 0.2283465F;
            this.txtReceiptRemainAmount.Left = 4.119572F;
            this.txtReceiptRemainAmount.MultiLine = false;
            this.txtReceiptRemainAmount.Name = "txtReceiptRemainAmount";
            this.txtReceiptRemainAmount.OutputFormat = resources.GetString("txtReceiptRemainAmount.OutputFormat");
            this.txtReceiptRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptRemainAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1; ddo-s" +
    "hrink-to-fit: none";
            this.txtReceiptRemainAmount.Text = null;
            this.txtReceiptRemainAmount.Top = 0F;
            this.txtReceiptRemainAmount.Width = 0.6751965F;
            // 
            // txtReceiptNote1
            // 
            this.txtReceiptNote1.Height = 0.2283465F;
            this.txtReceiptNote1.Left = 2.815749F;
            this.txtReceiptNote1.MultiLine = false;
            this.txtReceiptNote1.Name = "txtReceiptNote1";
            this.txtReceiptNote1.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptNote1.Text = null;
            this.txtReceiptNote1.Top = 0.2212597F;
            this.txtReceiptNote1.Width = 1.976378F;
            // 
            // lineDetailVerCustomerName
            // 
            this.lineDetailVerCustomerName.Height = 0.4507877F;
            this.lineDetailVerCustomerName.Left = 7.177166F;
            this.lineDetailVerCustomerName.LineWeight = 1F;
            this.lineDetailVerCustomerName.Name = "lineDetailVerCustomerName";
            this.lineDetailVerCustomerName.Top = 1.192093E-07F;
            this.lineDetailVerCustomerName.Width = 0F;
            this.lineDetailVerCustomerName.X1 = 7.177166F;
            this.lineDetailVerCustomerName.X2 = 7.177166F;
            this.lineDetailVerCustomerName.Y1 = 1.192093E-07F;
            this.lineDetailVerCustomerName.Y2 = 0.4507878F;
            // 
            // lineDetailVerBillingBilledAt
            // 
            this.lineDetailVerBillingBilledAt.Height = 0.2216536F;
            this.lineDetailVerBillingBilledAt.Left = 7.890552F;
            this.lineDetailVerBillingBilledAt.LineWeight = 1F;
            this.lineDetailVerBillingBilledAt.Name = "lineDetailVerBillingBilledAt";
            this.lineDetailVerBillingBilledAt.Top = 0F;
            this.lineDetailVerBillingBilledAt.Width = 0F;
            this.lineDetailVerBillingBilledAt.X1 = 7.890552F;
            this.lineDetailVerBillingBilledAt.X2 = 7.890552F;
            this.lineDetailVerBillingBilledAt.Y1 = 0.2216536F;
            this.lineDetailVerBillingBilledAt.Y2 = 0F;
            // 
            // lineDetailVerBillingDueAt
            // 
            this.lineDetailVerBillingDueAt.Height = 0.4507874F;
            this.lineDetailVerBillingDueAt.Left = 8.598425F;
            this.lineDetailVerBillingDueAt.LineWeight = 1F;
            this.lineDetailVerBillingDueAt.Name = "lineDetailVerBillingDueAt";
            this.lineDetailVerBillingDueAt.Top = 0F;
            this.lineDetailVerBillingDueAt.Width = 0F;
            this.lineDetailVerBillingDueAt.X1 = 8.598425F;
            this.lineDetailVerBillingDueAt.X2 = 8.598425F;
            this.lineDetailVerBillingDueAt.Y1 = 0F;
            this.lineDetailVerBillingDueAt.Y2 = 0.4507874F;
            // 
            // lineDetailVerBillingAmount
            // 
            this.lineDetailVerBillingAmount.Height = 0.2216533F;
            this.lineDetailVerBillingAmount.Left = 9.268505F;
            this.lineDetailVerBillingAmount.LineWeight = 1F;
            this.lineDetailVerBillingAmount.Name = "lineDetailVerBillingAmount";
            this.lineDetailVerBillingAmount.Top = 1.192093E-07F;
            this.lineDetailVerBillingAmount.Width = 0F;
            this.lineDetailVerBillingAmount.X1 = 9.268505F;
            this.lineDetailVerBillingAmount.X2 = 9.268505F;
            this.lineDetailVerBillingAmount.Y1 = 0.2216534F;
            this.lineDetailVerBillingAmount.Y2 = 1.192093E-07F;
            // 
            // lineDetailVerBillingRemainAmount
            // 
            this.lineDetailVerBillingRemainAmount.Height = 0.2216533F;
            this.lineDetailVerBillingRemainAmount.Left = 9.933071F;
            this.lineDetailVerBillingRemainAmount.LineWeight = 1F;
            this.lineDetailVerBillingRemainAmount.Name = "lineDetailVerBillingRemainAmount";
            this.lineDetailVerBillingRemainAmount.Top = 1.192093E-07F;
            this.lineDetailVerBillingRemainAmount.Width = 0F;
            this.lineDetailVerBillingRemainAmount.X1 = 9.933071F;
            this.lineDetailVerBillingRemainAmount.X2 = 9.933071F;
            this.lineDetailVerBillingRemainAmount.Y1 = 0.2216534F;
            this.lineDetailVerBillingRemainAmount.Y2 = 1.192093E-07F;
            // 
            // lineDetailVerSeparate
            // 
            this.lineDetailVerSeparate.Height = 0.4507874F;
            this.lineDetailVerSeparate.Left = 4.80315F;
            this.lineDetailVerSeparate.LineWeight = 1F;
            this.lineDetailVerSeparate.Name = "lineDetailVerSeparate";
            this.lineDetailVerSeparate.Top = 0F;
            this.lineDetailVerSeparate.Width = 0F;
            this.lineDetailVerSeparate.X1 = 4.80315F;
            this.lineDetailVerSeparate.X2 = 4.80315F;
            this.lineDetailVerSeparate.Y1 = 0F;
            this.lineDetailVerSeparate.Y2 = 0.4507874F;
            // 
            // lineDetailVerReceiptCategoryName
            // 
            this.lineDetailVerReceiptCategoryName.Height = 0.4507875F;
            this.lineDetailVerReceiptCategoryName.Left = 2.826378F;
            this.lineDetailVerReceiptCategoryName.LineWeight = 1F;
            this.lineDetailVerReceiptCategoryName.Name = "lineDetailVerReceiptCategoryName";
            this.lineDetailVerReceiptCategoryName.Top = 0F;
            this.lineDetailVerReceiptCategoryName.Width = 0F;
            this.lineDetailVerReceiptCategoryName.X1 = 2.826378F;
            this.lineDetailVerReceiptCategoryName.X2 = 2.826378F;
            this.lineDetailVerReceiptCategoryName.Y1 = 0F;
            this.lineDetailVerReceiptCategoryName.Y2 = 0.4507875F;
            // 
            // lineDetailVerReceiptDueAt
            // 
            this.lineDetailVerReceiptDueAt.Height = 0.2216526F;
            this.lineDetailVerReceiptDueAt.Left = 3.532964F;
            this.lineDetailVerReceiptDueAt.LineWeight = 1F;
            this.lineDetailVerReceiptDueAt.Name = "lineDetailVerReceiptDueAt";
            this.lineDetailVerReceiptDueAt.Top = 1.013279E-06F;
            this.lineDetailVerReceiptDueAt.Width = 0F;
            this.lineDetailVerReceiptDueAt.X1 = 3.532964F;
            this.lineDetailVerReceiptDueAt.X2 = 3.532964F;
            this.lineDetailVerReceiptDueAt.Y1 = 0.2216536F;
            this.lineDetailVerReceiptDueAt.Y2 = 1.013279E-06F;
            // 
            // lineDetailVerReceiptAmount
            // 
            this.lineDetailVerReceiptAmount.Height = 0.2216526F;
            this.lineDetailVerReceiptAmount.Left = 4.123511F;
            this.lineDetailVerReceiptAmount.LineWeight = 1F;
            this.lineDetailVerReceiptAmount.Name = "lineDetailVerReceiptAmount";
            this.lineDetailVerReceiptAmount.Top = 1.013279E-06F;
            this.lineDetailVerReceiptAmount.Width = 0F;
            this.lineDetailVerReceiptAmount.X1 = 4.123511F;
            this.lineDetailVerReceiptAmount.X2 = 4.123511F;
            this.lineDetailVerReceiptAmount.Y1 = 0.2216536F;
            this.lineDetailVerReceiptAmount.Y2 = 1.013279E-06F;
            // 
            // lineDetailVerPayerName
            // 
            this.lineDetailVerPayerName.Height = 0.4507875F;
            this.lineDetailVerPayerName.Left = 1.386898F;
            this.lineDetailVerPayerName.LineWeight = 1F;
            this.lineDetailVerPayerName.Name = "lineDetailVerPayerName";
            this.lineDetailVerPayerName.Top = 0F;
            this.lineDetailVerPayerName.Width = 0F;
            this.lineDetailVerPayerName.X1 = 1.386898F;
            this.lineDetailVerPayerName.X2 = 1.386898F;
            this.lineDetailVerPayerName.Y1 = 0F;
            this.lineDetailVerPayerName.Y2 = 0.4507875F;
            // 
            // txtReceiptCateoryName
            // 
            this.txtReceiptCateoryName.Height = 0.2283465F;
            this.txtReceiptCateoryName.Left = 2.102252F;
            this.txtReceiptCateoryName.MultiLine = false;
            this.txtReceiptCateoryName.Name = "txtReceiptCateoryName";
            this.txtReceiptCateoryName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptCateoryName.Text = null;
            this.txtReceiptCateoryName.Top = 0F;
            this.txtReceiptCateoryName.Width = 0.7133859F;
            // 
            // lineDetailVerRecordedAt
            // 
            this.lineDetailVerRecordedAt.Height = 0.4507874F;
            this.lineDetailVerRecordedAt.Left = 2.100284F;
            this.lineDetailVerRecordedAt.LineWeight = 1F;
            this.lineDetailVerRecordedAt.Name = "lineDetailVerRecordedAt";
            this.lineDetailVerRecordedAt.Top = 0F;
            this.lineDetailVerRecordedAt.Width = 0F;
            this.lineDetailVerRecordedAt.X1 = 2.100284F;
            this.lineDetailVerRecordedAt.X2 = 2.100284F;
            this.lineDetailVerRecordedAt.Y1 = 0F;
            this.lineDetailVerRecordedAt.Y2 = 0.4507874F;
            // 
            // txtBillingCategoryName
            // 
            this.txtBillingCategoryName.Height = 0.2283465F;
            this.txtBillingCategoryName.Left = 4.814961F;
            this.txtBillingCategoryName.MultiLine = false;
            this.txtBillingCategoryName.Name = "txtBillingCategoryName";
            this.txtBillingCategoryName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtBillingCategoryName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingCategoryName.Text = null;
            this.txtBillingCategoryName.Top = 0.2210004F;
            this.txtBillingCategoryName.Width = 0.886F;
            // 
            // lineDetailHorSeparate
            // 
            this.lineDetailHorSeparate.Height = 0F;
            this.lineDetailHorSeparate.Left = 0F;
            this.lineDetailHorSeparate.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineDetailHorSeparate.LineWeight = 1F;
            this.lineDetailHorSeparate.Name = "lineDetailHorSeparate";
            this.lineDetailHorSeparate.Top = 0.2212599F;
            this.lineDetailHorSeparate.Width = 10.59055F;
            this.lineDetailHorSeparate.X1 = 0F;
            this.lineDetailHorSeparate.X2 = 10.59055F;
            this.lineDetailHorSeparate.Y1 = 0.2212599F;
            this.lineDetailHorSeparate.Y2 = 0.2212599F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.4507874F;
            this.lineDetailHorLower.Width = 10.59055F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.59055F;
            this.lineDetailHorLower.Y1 = 0.4507874F;
            this.lineDetailHorLower.Y2 = 0.4507874F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.4507877F;
            this.lineDetailVerCustomerCode.Left = 5.700788F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 1.192093E-07F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 5.700788F;
            this.lineDetailVerCustomerCode.X2 = 5.700788F;
            this.lineDetailVerCustomerCode.Y1 = 1.192093E-07F;
            this.lineDetailVerCustomerCode.Y2 = 0.4507878F;
            // 
            // groupHeader1
            // 
            this.groupHeader1.Height = 0F;
            this.groupHeader1.Name = "groupHeader1";
            // 
            // groupFooter1
            // 
            this.groupFooter1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtBillingCount,
            this.lblDiscountAmount,
            this.lblBillingTaxDifference,
            this.lblBillingCount,
            this.txtBillingTotal,
            this.lblBillingAmountTotal,
            this.lblBankTransferFee,
            this.lblReceiptCount,
            this.lblReceiptTaxDifference,
            this.lblReceiptAmountTotal,
            this.txtReceiptCount,
            this.txtReceiptTotal,
            this.txtBankTransferFee,
            this.txtReceiptTaxDifference,
            this.txtBillingDiscountAmount,
            this.txtBillingTaxDifference,
            this.lineFooterHorBillingCount,
            this.lineFooterHorReceiptUpper,
            this.lineFooterHorReceiptLower,
            this.lineFooterBillingLower,
            this.lineFooterHorDiscountAmt});
            this.groupFooter1.Height = 0.5456693F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // txtBillingCount
            // 
            this.txtBillingCount.Height = 0.2283465F;
            this.txtBillingCount.Left = 9.448819F;
            this.txtBillingCount.Name = "txtBillingCount";
            this.txtBillingCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingCount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingCount.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count;
            this.txtBillingCount.Text = null;
            this.txtBillingCount.Top = 0F;
            this.txtBillingCount.Width = 1.151968F;
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.Height = 0.2283465F;
            this.lblDiscountAmount.HyperLink = null;
            this.lblDiscountAmount.Left = 7.29252F;
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            this.lblDiscountAmount.Style = "font-size: 7.5pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDiscountAmount.Text = "歩引額計";
            this.lblDiscountAmount.Top = 0F;
            this.lblDiscountAmount.Width = 0.7133859F;
            // 
            // lblBillingTaxDifference
            // 
            this.lblBillingTaxDifference.Height = 0.2283465F;
            this.lblBillingTaxDifference.HyperLink = null;
            this.lblBillingTaxDifference.Left = 7.29252F;
            this.lblBillingTaxDifference.Name = "lblBillingTaxDifference";
            this.lblBillingTaxDifference.Style = "font-size: 7.5pt; vertical-align: middle; ddo-char-set: 1";
            this.lblBillingTaxDifference.Text = "消費税誤差";
            this.lblBillingTaxDifference.Top = 0.2212599F;
            this.lblBillingTaxDifference.Width = 0.7133859F;
            // 
            // lblBillingCount
            // 
            this.lblBillingCount.Height = 0.2283465F;
            this.lblBillingCount.Left = 8.784646F;
            this.lblBillingCount.Name = "lblBillingCount";
            this.lblBillingCount.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.lblBillingCount.Style = "font-size: 7.5pt; vertical-align: middle; ddo-char-set: 1";
            this.lblBillingCount.Text = "件数";
            this.lblBillingCount.Top = 0F;
            this.lblBillingCount.Width = 0.6641732F;
            // 
            // txtBillingTotal
            // 
            this.txtBillingTotal.Height = 0.2283465F;
            this.txtBillingTotal.Left = 9.448819F;
            this.txtBillingTotal.Name = "txtBillingTotal";
            this.txtBillingTotal.OutputFormat = resources.GetString("txtBillingTotal.OutputFormat");
            this.txtBillingTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingTotal.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingTotal.SummaryGroup = "groupHeader1";
            this.txtBillingTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtBillingTotal.Text = null;
            this.txtBillingTotal.Top = 0.2212599F;
            this.txtBillingTotal.Width = 1.151968F;
            // 
            // lblBillingAmountTotal
            // 
            this.lblBillingAmountTotal.Height = 0.2283465F;
            this.lblBillingAmountTotal.Left = 8.784646F;
            this.lblBillingAmountTotal.Name = "lblBillingAmountTotal";
            this.lblBillingAmountTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.lblBillingAmountTotal.Style = "font-size: 7.5pt; vertical-align: middle; ddo-char-set: 1";
            this.lblBillingAmountTotal.Text = "請求額計";
            this.lblBillingAmountTotal.Top = 0.2212599F;
            this.lblBillingAmountTotal.Width = 0.6641732F;
            // 
            // lblBankTransferFee
            // 
            this.lblBankTransferFee.Height = 0.2283465F;
            this.lblBankTransferFee.HyperLink = null;
            this.lblBankTransferFee.Left = 1.667323F;
            this.lblBankTransferFee.Name = "lblBankTransferFee";
            this.lblBankTransferFee.Style = "font-size: 7.5pt; vertical-align: middle; ddo-char-set: 1";
            this.lblBankTransferFee.Text = "手数料計";
            this.lblBankTransferFee.Top = 0F;
            this.lblBankTransferFee.Width = 0.7728338F;
            // 
            // lblReceiptCount
            // 
            this.lblReceiptCount.Height = 0.2283465F;
            this.lblReceiptCount.Left = 3.246456F;
            this.lblReceiptCount.Name = "lblReceiptCount";
            this.lblReceiptCount.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.lblReceiptCount.Style = "font-size: 7.5pt; vertical-align: middle; ddo-char-set: 1";
            this.lblReceiptCount.Text = "件数";
            this.lblReceiptCount.Top = 0F;
            this.lblReceiptCount.Width = 0.5905512F;
            // 
            // lblReceiptTaxDifference
            // 
            this.lblReceiptTaxDifference.Height = 0.2283465F;
            this.lblReceiptTaxDifference.HyperLink = null;
            this.lblReceiptTaxDifference.Left = 1.667323F;
            this.lblReceiptTaxDifference.Name = "lblReceiptTaxDifference";
            this.lblReceiptTaxDifference.Style = "font-size: 7.5pt; vertical-align: middle; ddo-char-set: 1";
            this.lblReceiptTaxDifference.Text = "消費税誤差";
            this.lblReceiptTaxDifference.Top = 0.2212599F;
            this.lblReceiptTaxDifference.Width = 0.7728338F;
            // 
            // lblReceiptAmountTotal
            // 
            this.lblReceiptAmountTotal.Height = 0.2283465F;
            this.lblReceiptAmountTotal.Left = 3.246456F;
            this.lblReceiptAmountTotal.Name = "lblReceiptAmountTotal";
            this.lblReceiptAmountTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.lblReceiptAmountTotal.Style = "font-size: 7.5pt; vertical-align: middle; ddo-char-set: 1";
            this.lblReceiptAmountTotal.Text = "入金額計";
            this.lblReceiptAmountTotal.Top = 0.2212599F;
            this.lblReceiptAmountTotal.Width = 0.5905512F;
            // 
            // txtReceiptCount
            // 
            this.txtReceiptCount.Height = 0.2283465F;
            this.txtReceiptCount.Left = 3.837008F;
            this.txtReceiptCount.Name = "txtReceiptCount";
            this.txtReceiptCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptCount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptCount.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count;
            this.txtReceiptCount.Text = null;
            this.txtReceiptCount.Top = 0F;
            this.txtReceiptCount.Width = 0.9531496F;
            // 
            // txtReceiptTotal
            // 
            this.txtReceiptTotal.Height = 0.2283465F;
            this.txtReceiptTotal.Left = 3.837008F;
            this.txtReceiptTotal.Name = "txtReceiptTotal";
            this.txtReceiptTotal.OutputFormat = resources.GetString("txtReceiptTotal.OutputFormat");
            this.txtReceiptTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptTotal.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptTotal.SummaryGroup = "groupHeader1";
            this.txtReceiptTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtReceiptTotal.Text = null;
            this.txtReceiptTotal.Top = 0.2212599F;
            this.txtReceiptTotal.Width = 0.9531498F;
            // 
            // txtBankTransferFee
            // 
            this.txtBankTransferFee.Height = 0.2283465F;
            this.txtBankTransferFee.Left = 2.377953F;
            this.txtBankTransferFee.Name = "txtBankTransferFee";
            this.txtBankTransferFee.OutputFormat = resources.GetString("txtBankTransferFee.OutputFormat");
            this.txtBankTransferFee.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBankTransferFee.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBankTransferFee.Text = null;
            this.txtBankTransferFee.Top = 0F;
            this.txtBankTransferFee.Width = 0.8685036F;
            // 
            // txtReceiptTaxDifference
            // 
            this.txtReceiptTaxDifference.Height = 0.2283465F;
            this.txtReceiptTaxDifference.Left = 2.377953F;
            this.txtReceiptTaxDifference.Name = "txtReceiptTaxDifference";
            this.txtReceiptTaxDifference.OutputFormat = resources.GetString("txtReceiptTaxDifference.OutputFormat");
            this.txtReceiptTaxDifference.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptTaxDifference.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptTaxDifference.Text = null;
            this.txtReceiptTaxDifference.Top = 0.2212599F;
            this.txtReceiptTaxDifference.Width = 0.8685036F;
            // 
            // txtBillingDiscountAmount
            // 
            this.txtBillingDiscountAmount.Height = 0.2283465F;
            this.txtBillingDiscountAmount.Left = 8.005906F;
            this.txtBillingDiscountAmount.Name = "txtBillingDiscountAmount";
            this.txtBillingDiscountAmount.OutputFormat = resources.GetString("txtBillingDiscountAmount.OutputFormat");
            this.txtBillingDiscountAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingDiscountAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingDiscountAmount.Text = null;
            this.txtBillingDiscountAmount.Top = 0F;
            this.txtBillingDiscountAmount.Width = 0.7787402F;
            // 
            // txtBillingTaxDifference
            // 
            this.txtBillingTaxDifference.Height = 0.2283465F;
            this.txtBillingTaxDifference.Left = 8.005906F;
            this.txtBillingTaxDifference.Name = "txtBillingTaxDifference";
            this.txtBillingTaxDifference.OutputFormat = resources.GetString("txtBillingTaxDifference.OutputFormat");
            this.txtBillingTaxDifference.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingTaxDifference.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingTaxDifference.Text = null;
            this.txtBillingTaxDifference.Top = 0.2212599F;
            this.txtBillingTaxDifference.Width = 0.7787402F;
            // 
            // lineFooterHorBillingCount
            // 
            this.lineFooterHorBillingCount.Height = 0F;
            this.lineFooterHorBillingCount.Left = 8.784646F;
            this.lineFooterHorBillingCount.LineWeight = 1F;
            this.lineFooterHorBillingCount.Name = "lineFooterHorBillingCount";
            this.lineFooterHorBillingCount.Top = 0.2283465F;
            this.lineFooterHorBillingCount.Width = 1.813784F;
            this.lineFooterHorBillingCount.X1 = 8.784646F;
            this.lineFooterHorBillingCount.X2 = 10.59843F;
            this.lineFooterHorBillingCount.Y1 = 0.2283465F;
            this.lineFooterHorBillingCount.Y2 = 0.2283465F;
            // 
            // lineFooterHorReceiptUpper
            // 
            this.lineFooterHorReceiptUpper.Height = 0F;
            this.lineFooterHorReceiptUpper.Left = 1.667323F;
            this.lineFooterHorReceiptUpper.LineWeight = 1F;
            this.lineFooterHorReceiptUpper.Name = "lineFooterHorReceiptUpper";
            this.lineFooterHorReceiptUpper.Top = 0.2212599F;
            this.lineFooterHorReceiptUpper.Width = 3.122835F;
            this.lineFooterHorReceiptUpper.X1 = 1.667323F;
            this.lineFooterHorReceiptUpper.X2 = 4.790158F;
            this.lineFooterHorReceiptUpper.Y1 = 0.2212599F;
            this.lineFooterHorReceiptUpper.Y2 = 0.2212599F;
            // 
            // lineFooterHorReceiptLower
            // 
            this.lineFooterHorReceiptLower.Height = 0F;
            this.lineFooterHorReceiptLower.Left = 1.667322F;
            this.lineFooterHorReceiptLower.LineWeight = 1F;
            this.lineFooterHorReceiptLower.Name = "lineFooterHorReceiptLower";
            this.lineFooterHorReceiptLower.Top = 0.4429134F;
            this.lineFooterHorReceiptLower.Width = 3.122836F;
            this.lineFooterHorReceiptLower.X1 = 1.667322F;
            this.lineFooterHorReceiptLower.X2 = 4.790158F;
            this.lineFooterHorReceiptLower.Y1 = 0.4429134F;
            this.lineFooterHorReceiptLower.Y2 = 0.4429134F;
            // 
            // lineFooterBillingLower
            // 
            this.lineFooterBillingLower.Height = 0F;
            this.lineFooterBillingLower.Left = 7.299213F;
            this.lineFooterBillingLower.LineWeight = 1F;
            this.lineFooterBillingLower.Name = "lineFooterBillingLower";
            this.lineFooterBillingLower.Top = 0.4429134F;
            this.lineFooterBillingLower.Width = 3.308657F;
            this.lineFooterBillingLower.X1 = 7.299213F;
            this.lineFooterBillingLower.X2 = 10.60787F;
            this.lineFooterBillingLower.Y1 = 0.4429134F;
            this.lineFooterBillingLower.Y2 = 0.4429134F;
            // 
            // lineFooterHorDiscountAmt
            // 
            this.lineFooterHorDiscountAmt.Height = 0F;
            this.lineFooterHorDiscountAmt.Left = 7.29252F;
            this.lineFooterHorDiscountAmt.LineWeight = 1F;
            this.lineFooterHorDiscountAmt.Name = "lineFooterHorDiscountAmt";
            this.lineFooterHorDiscountAmt.Top = 0.2283465F;
            this.lineFooterHorDiscountAmt.Width = 1.526378F;
            this.lineFooterHorDiscountAmt.X1 = 8.818898F;
            this.lineFooterHorDiscountAmt.X2 = 7.29252F;
            this.lineFooterHorDiscountAmt.Y1 = 0.2283465F;
            this.lineFooterHorDiscountAmt.Y2 = 0.2283465F;
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyName,
            this.lblOutputDate,
            this.riOutputDate,
            this.lblTitle,
            this.label11,
            this.lblCustomerCode,
            this.lblBillingCategoryName,
            this.lblCustomerName,
            this.lblInvoiceNo,
            this.lblBillingBilledAt,
            this.lblBillingDueAt,
            this.lblBillingAmount,
            this.lblBillingRemainAmount,
            this.lblMatchingAmount,
            this.lblBillingNote1,
            this.lblRecordedAt,
            this.lblAccountType,
            this.label16,
            this.label17,
            this.lblReceiptCategoryName,
            this.lblAccountNumber,
            this.lblReceiptDueAt,
            this.lblReceiptRemainAmount,
            this.lblReceiptAmount,
            this.lineHeaderHorUpper,
            this.lblBillingDepartment,
            this.lblReceiptRemark,
            this.lineHeaderHorLower,
            this.lineHeaderHorSeparate,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderVerCustomerName,
            this.lineHeaderVerBillingBilledAt,
            this.lineHeaderVerSeparate,
            this.lineHeaderVerBillingDueAt,
            this.lineHeaderVerBillingRemainAmount,
            this.lineHeaderVerBillingAmount,
            this.lineHeaderVerPayerName,
            this.lineHeaderVerRecordedAt,
            this.lineHeaderVerReceiptCategoryname,
            this.lineHeaderVerReceiptDueAt,
            this.lineHeaderVerReceiptAmount,
            this.lblBillingInfo,
            this.lblReceiptInfo});
            this.pageHeader.Height = 1.236614F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Height = 0.2F;
            this.lblCompanyName.HyperLink = null;
            this.lblCompanyName.Left = 0.811811F;
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyName.Text = "label2";
            this.lblCompanyName.Top = 0F;
            this.lblCompanyName.Width = 3.657F;
            // 
            // lblOutputDate
            // 
            this.lblOutputDate.Height = 0.2F;
            this.lblOutputDate.HyperLink = null;
            this.lblOutputDate.Left = 8.809055F;
            this.lblOutputDate.Name = "lblOutputDate";
            this.lblOutputDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblOutputDate.Text = "出力日付　：";
            this.lblOutputDate.Top = 0F;
            this.lblOutputDate.Width = 0.6984252F;
            // 
            // riOutputDate
            // 
            this.riOutputDate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.riOutputDate.Height = 0.2F;
            this.riOutputDate.Left = 9.522441F;
            this.riOutputDate.Name = "riOutputDate";
            this.riOutputDate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle; ddo-char-s" +
    "et: 1";
            this.riOutputDate.Top = 0F;
            this.riOutputDate.Width = 1.015F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "個別消込画面";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // label11
            // 
            this.label11.Height = 0.2F;
            this.label11.HyperLink = null;
            this.label11.Left = 0.02440945F;
            this.label11.Name = "label11";
            this.label11.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.label11.Text = "会社コード　：";
            this.label11.Top = 0F;
            this.label11.Width = 0.7874016F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.2283465F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 4.811024F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.7874016F;
            this.lblCustomerCode.Width = 0.8858268F;
            // 
            // lblBillingCategoryName
            // 
            this.lblBillingCategoryName.Height = 0.2283465F;
            this.lblBillingCategoryName.HyperLink = null;
            this.lblBillingCategoryName.Left = 4.811024F;
            this.lblBillingCategoryName.Name = "lblBillingCategoryName";
            this.lblBillingCategoryName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingCategoryName.Text = "請求区分";
            this.lblBillingCategoryName.Top = 1.009055F;
            this.lblBillingCategoryName.Width = 0.8858268F;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Height = 0.2283465F;
            this.lblCustomerName.HyperLink = null;
            this.lblCustomerName.Left = 5.696851F;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.Top = 0.7874016F;
            this.lblCustomerName.Width = 1.476378F;
            // 
            // lblInvoiceNo
            // 
            this.lblInvoiceNo.Height = 0.2283465F;
            this.lblInvoiceNo.HyperLink = null;
            this.lblInvoiceNo.Left = 5.696851F;
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblInvoiceNo.Text = "請求書番号";
            this.lblInvoiceNo.Top = 1.009055F;
            this.lblInvoiceNo.Width = 1.476378F;
            // 
            // lblBillingBilledAt
            // 
            this.lblBillingBilledAt.Height = 0.2283465F;
            this.lblBillingBilledAt.HyperLink = null;
            this.lblBillingBilledAt.Left = 7.173229F;
            this.lblBillingBilledAt.Name = "lblBillingBilledAt";
            this.lblBillingBilledAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingBilledAt.Text = "請求日";
            this.lblBillingBilledAt.Top = 0.7874016F;
            this.lblBillingBilledAt.Width = 0.7133861F;
            // 
            // lblBillingDueAt
            // 
            this.lblBillingDueAt.Height = 0.2283465F;
            this.lblBillingDueAt.HyperLink = null;
            this.lblBillingDueAt.Left = 7.886615F;
            this.lblBillingDueAt.Name = "lblBillingDueAt";
            this.lblBillingDueAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingDueAt.Text = "入金予定日";
            this.lblBillingDueAt.Top = 0.7874016F;
            this.lblBillingDueAt.Width = 0.7133859F;
            // 
            // lblBillingAmount
            // 
            this.lblBillingAmount.Height = 0.2283465F;
            this.lblBillingAmount.HyperLink = null;
            this.lblBillingAmount.Left = 8.600393F;
            this.lblBillingAmount.Name = "lblBillingAmount";
            this.lblBillingAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingAmount.Text = "請求額";
            this.lblBillingAmount.Top = 0.7874016F;
            this.lblBillingAmount.Width = 0.6641732F;
            // 
            // lblBillingRemainAmount
            // 
            this.lblBillingRemainAmount.Height = 0.2283465F;
            this.lblBillingRemainAmount.HyperLink = null;
            this.lblBillingRemainAmount.Left = 9.264566F;
            this.lblBillingRemainAmount.Name = "lblBillingRemainAmount";
            this.lblBillingRemainAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingRemainAmount.Text = "請求残";
            this.lblBillingRemainAmount.Top = 0.7874016F;
            this.lblBillingRemainAmount.Width = 0.6641732F;
            // 
            // lblMatchingAmount
            // 
            this.lblMatchingAmount.Height = 0.2283465F;
            this.lblMatchingAmount.HyperLink = null;
            this.lblMatchingAmount.Left = 9.929134F;
            this.lblMatchingAmount.Name = "lblMatchingAmount";
            this.lblMatchingAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblMatchingAmount.Text = "消込対象額";
            this.lblMatchingAmount.Top = 0.7874016F;
            this.lblMatchingAmount.Width = 0.6641732F;
            // 
            // lblBillingNote1
            // 
            this.lblBillingNote1.Height = 0.2283465F;
            this.lblBillingNote1.HyperLink = null;
            this.lblBillingNote1.Left = 8.600393F;
            this.lblBillingNote1.Name = "lblBillingNote1";
            this.lblBillingNote1.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingNote1.Text = "備考";
            this.lblBillingNote1.Top = 1.009055F;
            this.lblBillingNote1.Width = 1.992126F;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Height = 0.2283465F;
            this.lblRecordedAt.HyperLink = null;
            this.lblRecordedAt.Left = 1.382677F;
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblRecordedAt.Text = "入金日";
            this.lblRecordedAt.Top = 0.7874016F;
            this.lblRecordedAt.Width = 0.7133859F;
            // 
            // lblAccountType
            // 
            this.lblAccountType.Height = 0.2283465F;
            this.lblAccountType.HyperLink = null;
            this.lblAccountType.Left = 1.382677F;
            this.lblAccountType.Name = "lblAccountType";
            this.lblAccountType.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAccountType.Text = "種別";
            this.lblAccountType.Top = 1.009055F;
            this.lblAccountType.Width = 0.7133859F;
            // 
            // label16
            // 
            this.label16.Height = 0.2283465F;
            this.label16.HyperLink = null;
            this.label16.Left = 2.384186E-07F;
            this.label16.Name = "label16";
            this.label16.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label16.Text = "振込依頼人名";
            this.label16.Top = 0.7874016F;
            this.label16.Width = 1.382678F;
            // 
            // label17
            // 
            this.label17.Height = 0.2283465F;
            this.label17.HyperLink = null;
            this.label17.Left = 2.384186E-07F;
            this.label17.Name = "label17";
            this.label17.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label17.Text = "仕向銀行 / 仕向支店";
            this.label17.Top = 1.009055F;
            this.label17.Width = 1.382678F;
            // 
            // lblReceiptCategoryName
            // 
            this.lblReceiptCategoryName.Height = 0.2283465F;
            this.lblReceiptCategoryName.HyperLink = null;
            this.lblReceiptCategoryName.Left = 2.105905F;
            this.lblReceiptCategoryName.Name = "lblReceiptCategoryName";
            this.lblReceiptCategoryName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptCategoryName.Text = "入金区分";
            this.lblReceiptCategoryName.Top = 0.7874016F;
            this.lblReceiptCategoryName.Width = 0.7133859F;
            // 
            // lblAccountNumber
            // 
            this.lblAccountNumber.Height = 0.2283465F;
            this.lblAccountNumber.HyperLink = null;
            this.lblAccountNumber.Left = 2.105905F;
            this.lblAccountNumber.Name = "lblAccountNumber";
            this.lblAccountNumber.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAccountNumber.Text = "口座番号";
            this.lblAccountNumber.Top = 1.009055F;
            this.lblAccountNumber.Width = 0.7133859F;
            // 
            // lblReceiptDueAt
            // 
            this.lblReceiptDueAt.Height = 0.2283465F;
            this.lblReceiptDueAt.HyperLink = null;
            this.lblReceiptDueAt.Left = 2.819292F;
            this.lblReceiptDueAt.Name = "lblReceiptDueAt";
            this.lblReceiptDueAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptDueAt.Text = "期日";
            this.lblReceiptDueAt.Top = 0.7874016F;
            this.lblReceiptDueAt.Width = 0.7133859F;
            // 
            // lblReceiptRemainAmount
            // 
            this.lblReceiptRemainAmount.Height = 0.2283465F;
            this.lblReceiptRemainAmount.HyperLink = null;
            this.lblReceiptRemainAmount.Left = 4.133859F;
            this.lblReceiptRemainAmount.Name = "lblReceiptRemainAmount";
            this.lblReceiptRemainAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptRemainAmount.Text = "入金残";
            this.lblReceiptRemainAmount.Top = 0.7874016F;
            this.lblReceiptRemainAmount.Width = 0.6692914F;
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Height = 0.2283465F;
            this.lblReceiptAmount.HyperLink = null;
            this.lblReceiptAmount.Left = 3.532678F;
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptAmount.Text = "入金額";
            this.lblReceiptAmount.Top = 0.7874016F;
            this.lblReceiptAmount.Width = 0.5905517F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.7905512F;
            this.lineHeaderHorUpper.Width = 10.59055F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.59055F;
            this.lineHeaderHorUpper.Y1 = 0.7905512F;
            this.lineHeaderHorUpper.Y2 = 0.7905512F;
            // 
            // lblBillingDepartment
            // 
            this.lblBillingDepartment.Height = 0.2283465F;
            this.lblBillingDepartment.HyperLink = null;
            this.lblBillingDepartment.Left = 7.173229F;
            this.lblBillingDepartment.Name = "lblBillingDepartment";
            this.lblBillingDepartment.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingDepartment.Text = "請求部門";
            this.lblBillingDepartment.Top = 1.009055F;
            this.lblBillingDepartment.Width = 1.417323F;
            // 
            // lblReceiptRemark
            // 
            this.lblReceiptRemark.Height = 0.2283465F;
            this.lblReceiptRemark.HyperLink = null;
            this.lblReceiptRemark.Left = 2.819292F;
            this.lblReceiptRemark.Name = "lblReceiptRemark";
            this.lblReceiptRemark.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptRemark.Text = "備考";
            this.lblReceiptRemark.Top = 1.009055F;
            this.lblReceiptRemark.Width = 1.979134F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.236614F;
            this.lineHeaderHorLower.Width = 10.59055F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.59055F;
            this.lineHeaderHorLower.Y1 = 1.236614F;
            this.lineHeaderHorLower.Y2 = 1.236614F;
            // 
            // lineHeaderHorSeparate
            // 
            this.lineHeaderHorSeparate.Height = 0F;
            this.lineHeaderHorSeparate.Left = 0F;
            this.lineHeaderHorSeparate.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorSeparate.LineWeight = 1F;
            this.lineHeaderHorSeparate.Name = "lineHeaderHorSeparate";
            this.lineHeaderHorSeparate.Top = 1.012204F;
            this.lineHeaderHorSeparate.Width = 10.58071F;
            this.lineHeaderHorSeparate.X1 = 0F;
            this.lineHeaderHorSeparate.X2 = 10.58071F;
            this.lineHeaderHorSeparate.Y1 = 1.012204F;
            this.lineHeaderHorSeparate.Y2 = 1.012204F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.4519684F;
            this.lineHeaderVerCustomerCode.Left = 5.700788F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.7874016F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 5.700788F;
            this.lineHeaderVerCustomerCode.X2 = 5.700788F;
            this.lineHeaderVerCustomerCode.Y1 = 0.7874016F;
            this.lineHeaderVerCustomerCode.Y2 = 1.23937F;
            // 
            // lineHeaderVerCustomerName
            // 
            this.lineHeaderVerCustomerName.Height = 0.4519693F;
            this.lineHeaderVerCustomerName.Left = 7.177166F;
            this.lineHeaderVerCustomerName.LineWeight = 1F;
            this.lineHeaderVerCustomerName.Name = "lineHeaderVerCustomerName";
            this.lineHeaderVerCustomerName.Top = 0.7874016F;
            this.lineHeaderVerCustomerName.Width = 0F;
            this.lineHeaderVerCustomerName.X1 = 7.177166F;
            this.lineHeaderVerCustomerName.X2 = 7.177166F;
            this.lineHeaderVerCustomerName.Y1 = 0.7874016F;
            this.lineHeaderVerCustomerName.Y2 = 1.239371F;
            // 
            // lineHeaderVerBillingBilledAt
            // 
            this.lineHeaderVerBillingBilledAt.Height = 0.2216534F;
            this.lineHeaderVerBillingBilledAt.Left = 7.890551F;
            this.lineHeaderVerBillingBilledAt.LineWeight = 1F;
            this.lineHeaderVerBillingBilledAt.Name = "lineHeaderVerBillingBilledAt";
            this.lineHeaderVerBillingBilledAt.Top = 0.7874016F;
            this.lineHeaderVerBillingBilledAt.Width = 0F;
            this.lineHeaderVerBillingBilledAt.X1 = 7.890551F;
            this.lineHeaderVerBillingBilledAt.X2 = 7.890551F;
            this.lineHeaderVerBillingBilledAt.Y1 = 1.009055F;
            this.lineHeaderVerBillingBilledAt.Y2 = 0.7874016F;
            // 
            // lineHeaderVerSeparate
            // 
            this.lineHeaderVerSeparate.Height = 0.4519688F;
            this.lineHeaderVerSeparate.Left = 4.80315F;
            this.lineHeaderVerSeparate.LineWeight = 1F;
            this.lineHeaderVerSeparate.Name = "lineHeaderVerSeparate";
            this.lineHeaderVerSeparate.Top = 0.7905512F;
            this.lineHeaderVerSeparate.Width = 0F;
            this.lineHeaderVerSeparate.X1 = 4.80315F;
            this.lineHeaderVerSeparate.X2 = 4.80315F;
            this.lineHeaderVerSeparate.Y1 = 0.7905512F;
            this.lineHeaderVerSeparate.Y2 = 1.24252F;
            // 
            // lineHeaderVerBillingDueAt
            // 
            this.lineHeaderVerBillingDueAt.Height = 0.4519693F;
            this.lineHeaderVerBillingDueAt.Left = 8.598426F;
            this.lineHeaderVerBillingDueAt.LineWeight = 1F;
            this.lineHeaderVerBillingDueAt.Name = "lineHeaderVerBillingDueAt";
            this.lineHeaderVerBillingDueAt.Top = 0.7874016F;
            this.lineHeaderVerBillingDueAt.Width = 0F;
            this.lineHeaderVerBillingDueAt.X1 = 8.598426F;
            this.lineHeaderVerBillingDueAt.X2 = 8.598426F;
            this.lineHeaderVerBillingDueAt.Y1 = 0.7874016F;
            this.lineHeaderVerBillingDueAt.Y2 = 1.239371F;
            // 
            // lineHeaderVerBillingRemainAmount
            // 
            this.lineHeaderVerBillingRemainAmount.Height = 0.2216534F;
            this.lineHeaderVerBillingRemainAmount.Left = 9.933071F;
            this.lineHeaderVerBillingRemainAmount.LineWeight = 1F;
            this.lineHeaderVerBillingRemainAmount.Name = "lineHeaderVerBillingRemainAmount";
            this.lineHeaderVerBillingRemainAmount.Top = 0.7874016F;
            this.lineHeaderVerBillingRemainAmount.Width = 0F;
            this.lineHeaderVerBillingRemainAmount.X1 = 9.933071F;
            this.lineHeaderVerBillingRemainAmount.X2 = 9.933071F;
            this.lineHeaderVerBillingRemainAmount.Y1 = 1.009055F;
            this.lineHeaderVerBillingRemainAmount.Y2 = 0.7874016F;
            // 
            // lineHeaderVerBillingAmount
            // 
            this.lineHeaderVerBillingAmount.Height = 0.2216534F;
            this.lineHeaderVerBillingAmount.Left = 9.268504F;
            this.lineHeaderVerBillingAmount.LineWeight = 1F;
            this.lineHeaderVerBillingAmount.Name = "lineHeaderVerBillingAmount";
            this.lineHeaderVerBillingAmount.Top = 0.7874016F;
            this.lineHeaderVerBillingAmount.Width = 0F;
            this.lineHeaderVerBillingAmount.X1 = 9.268504F;
            this.lineHeaderVerBillingAmount.X2 = 9.268504F;
            this.lineHeaderVerBillingAmount.Y1 = 1.009055F;
            this.lineHeaderVerBillingAmount.Y2 = 0.7874016F;
            // 
            // lineHeaderVerPayerName
            // 
            this.lineHeaderVerPayerName.Height = 0.4519684F;
            this.lineHeaderVerPayerName.Left = 1.387008F;
            this.lineHeaderVerPayerName.LineWeight = 1F;
            this.lineHeaderVerPayerName.Name = "lineHeaderVerPayerName";
            this.lineHeaderVerPayerName.Top = 0.7874016F;
            this.lineHeaderVerPayerName.Width = 0F;
            this.lineHeaderVerPayerName.X1 = 1.387008F;
            this.lineHeaderVerPayerName.X2 = 1.387008F;
            this.lineHeaderVerPayerName.Y1 = 0.7874016F;
            this.lineHeaderVerPayerName.Y2 = 1.23937F;
            // 
            // lineHeaderVerRecordedAt
            // 
            this.lineHeaderVerRecordedAt.Height = 0.4519693F;
            this.lineHeaderVerRecordedAt.Left = 2.100394F;
            this.lineHeaderVerRecordedAt.LineWeight = 1F;
            this.lineHeaderVerRecordedAt.Name = "lineHeaderVerRecordedAt";
            this.lineHeaderVerRecordedAt.Top = 0.7874016F;
            this.lineHeaderVerRecordedAt.Width = 0F;
            this.lineHeaderVerRecordedAt.X1 = 2.100394F;
            this.lineHeaderVerRecordedAt.X2 = 2.100394F;
            this.lineHeaderVerRecordedAt.Y1 = 0.7874016F;
            this.lineHeaderVerRecordedAt.Y2 = 1.239371F;
            // 
            // lineHeaderVerReceiptCategoryname
            // 
            this.lineHeaderVerReceiptCategoryname.Height = 0.4519684F;
            this.lineHeaderVerReceiptCategoryname.Left = 2.826378F;
            this.lineHeaderVerReceiptCategoryname.LineWeight = 1F;
            this.lineHeaderVerReceiptCategoryname.Name = "lineHeaderVerReceiptCategoryname";
            this.lineHeaderVerReceiptCategoryname.Top = 0.7874016F;
            this.lineHeaderVerReceiptCategoryname.Width = 0F;
            this.lineHeaderVerReceiptCategoryname.X1 = 2.826378F;
            this.lineHeaderVerReceiptCategoryname.X2 = 2.826378F;
            this.lineHeaderVerReceiptCategoryname.Y1 = 0.7874016F;
            this.lineHeaderVerReceiptCategoryname.Y2 = 1.23937F;
            // 
            // lineHeaderVerReceiptDueAt
            // 
            this.lineHeaderVerReceiptDueAt.Height = 0.2216527F;
            this.lineHeaderVerReceiptDueAt.Left = 3.533071F;
            this.lineHeaderVerReceiptDueAt.LineWeight = 1F;
            this.lineHeaderVerReceiptDueAt.Name = "lineHeaderVerReceiptDueAt";
            this.lineHeaderVerReceiptDueAt.Top = 0.7874023F;
            this.lineHeaderVerReceiptDueAt.Width = 0F;
            this.lineHeaderVerReceiptDueAt.X1 = 3.533071F;
            this.lineHeaderVerReceiptDueAt.X2 = 3.533071F;
            this.lineHeaderVerReceiptDueAt.Y1 = 1.009055F;
            this.lineHeaderVerReceiptDueAt.Y2 = 0.7874023F;
            // 
            // lineHeaderVerReceiptAmount
            // 
            this.lineHeaderVerReceiptAmount.Height = 0.2216534F;
            this.lineHeaderVerReceiptAmount.Left = 4.123622F;
            this.lineHeaderVerReceiptAmount.LineWeight = 1F;
            this.lineHeaderVerReceiptAmount.Name = "lineHeaderVerReceiptAmount";
            this.lineHeaderVerReceiptAmount.Top = 0.7874016F;
            this.lineHeaderVerReceiptAmount.Width = 0F;
            this.lineHeaderVerReceiptAmount.X1 = 4.123622F;
            this.lineHeaderVerReceiptAmount.X2 = 4.123622F;
            this.lineHeaderVerReceiptAmount.Y1 = 1.009055F;
            this.lineHeaderVerReceiptAmount.Y2 = 0.7874016F;
            // 
            // lblBillingInfo
            // 
            this.lblBillingInfo.Height = 0.2F;
            this.lblBillingInfo.HyperLink = null;
            this.lblBillingInfo.Left = 4.80315F;
            this.lblBillingInfo.Name = "lblBillingInfo";
            this.lblBillingInfo.Style = "font-weight: bold; text-align: left; vertical-align: middle";
            this.lblBillingInfo.Text = " <請求情報>";
            this.lblBillingInfo.Top = 0.5708662F;
            this.lblBillingInfo.Width = 1F;
            // 
            // lblReceiptInfo
            // 
            this.lblReceiptInfo.Height = 0.2F;
            this.lblReceiptInfo.HyperLink = null;
            this.lblReceiptInfo.Left = 0.01417323F;
            this.lblReceiptInfo.Name = "lblReceiptInfo";
            this.lblReceiptInfo.Style = "font-weight: bold; text-align: left; vertical-align: middle";
            this.lblReceiptInfo.Text = " <入金情報>";
            this.lblReceiptInfo.Top = 0.5708662F;
            this.lblReceiptInfo.Width = 1F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.reportInfo1,
            this.lblPageNumber});
            this.pageFooter.Height = 0.3149606F;
            this.pageFooter.Name = "pageFooter";
            this.pageFooter.BeforePrint += new System.EventHandler(this.pageFooter_BeforePrint);
            // 
            // reportInfo1
            // 
            this.reportInfo1.FormatString = "{PageNumber} / {PageCount}";
            this.reportInfo1.Height = 0.2F;
            this.reportInfo1.Left = 9.037008F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Silver; text-align: center";
            this.reportInfo1.Top = 0F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1.181102F;
            // 
            // lblPageNumber
            // 
            this.lblPageNumber.Height = 0.2F;
            this.lblPageNumber.HyperLink = null;
            this.lblPageNumber.Left = 0F;
            this.lblPageNumber.Name = "lblPageNumber";
            this.lblPageNumber.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle";
            this.lblPageNumber.Text = "PageNumber/PageCount";
            this.lblPageNumber.Top = 0.05748032F;
            this.lblPageNumber.Width = 10.62992F;
            // 
            // MatchingIndividualReceiptBillingSectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.62992F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.groupHeader1);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.groupFooter1);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-size: 9pt; font-family: \"ＭＳ 明朝\"; ddo-char-set: 186", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBankBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptCateoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDiscountAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingTaxDifference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankTransferFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptTaxDifference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankTransferFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptTaxDifference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDiscountAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingTaxDifference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riOutputDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMatchingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceNo;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMatchingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingNote1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSourceBankBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountType;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSeparate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerReceiptCategoryName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerReceiptDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingCount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDiscountAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingTaxDifference;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox lblBillingCount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox lblBillingAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankTransferFee;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox lblReceiptCount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptTaxDifference;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox lblReceiptAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptCount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankTransferFee;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptTaxDifference;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingDiscountAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingTaxDifference;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorBillingCount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorReceiptUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorReceiptLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterBillingLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptCateoryName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingCategoryName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorSeparate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.PageHeader pageHeader;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblOutputDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo riOutputDate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label label11;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingCategoryName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceNo;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblMatchingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccountType;
        private GrapeCity.ActiveReports.SectionReportModel.Label label16;
        private GrapeCity.ActiveReports.SectionReportModel.Label label17;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptCategoryName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptRemark;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorSeparate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSeparate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerReceiptCategoryname;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerReceiptDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingInfo;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptInfo;
        private GrapeCity.ActiveReports.SectionReportModel.PageFooter pageFooter;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorDiscountAmt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        public GrapeCity.ActiveReports.SectionReportModel.Shape shpBilling;
    }
}
