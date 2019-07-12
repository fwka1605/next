namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// IndividualClearSectionReport の概要の説明です。
    /// </summary>
    partial class MatchingIndividualSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MatchingIndividualSectionReport));
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
            this.line21 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line22 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line23 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line24 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line25 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line26 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line29 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line30 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line31 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line27 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtReceiptCateoryName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line28 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtBillingCategoryName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line19 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line36 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line18 = new GrapeCity.ActiveReports.SectionReportModel.Line();
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
            this.line32 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line34 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line33 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line35 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lnDiscountAmt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblOutputDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.riOutputDate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label11 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label6 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label2 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label3 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label4 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label7 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label8 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label9 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label10 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingRemark = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label14 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label15 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label16 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label17 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label18 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label19 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label20 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label22 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label23 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblDepartment = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptRemark = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line10 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line11 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line14 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line15 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line16 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line17 = new GrapeCity.ActiveReports.SectionReportModel.Line();
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
            ((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartment)).BeginInit();
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
            this.line21,
            this.line22,
            this.line23,
            this.line24,
            this.line25,
            this.line26,
            this.line29,
            this.line30,
            this.line31,
            this.line27,
            this.txtReceiptCateoryName,
            this.line28,
            this.txtBillingCategoryName,
            this.line19,
            this.line36,
            this.line18});
            this.detail.Height = 0.4583334F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // shpBilling
            // 
            this.shpBilling.BackColor = System.Drawing.Color.LightCyan;
            this.shpBilling.Height = 0.4429134F;
            this.shpBilling.Left = 0F;
            this.shpBilling.LineColor = System.Drawing.Color.Transparent;
            this.shpBilling.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Transparent;
            this.shpBilling.LineWeight = 0F;
            this.shpBilling.Name = "shpBilling";
            this.shpBilling.RoundingRadius = new GrapeCity.ActiveReports.Controls.CornersRadius(10F, null, null, null, null);
            this.shpBilling.Top = 0F;
            this.shpBilling.Width = 5.782284F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2283465F;
            this.txtCustomerCode.Left = 0F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "text-align: center; vertical-align: middle";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 0.8858268F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.2283465F;
            this.txtCustomerName.Left = 0.8858268F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerName.Style = "vertical-align: middle";
            this.txtCustomerName.Text = null;
            this.txtCustomerName.Top = 0F;
            this.txtCustomerName.Width = 1.476378F;
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.Height = 0.2283465F;
            this.txtInvoiceNo.Left = 0.8858268F;
            this.txtInvoiceNo.MultiLine = false;
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtInvoiceNo.Style = "vertical-align: middle";
            this.txtInvoiceNo.Text = null;
            this.txtInvoiceNo.Top = 0.2212599F;
            this.txtInvoiceNo.Width = 1.476378F;
            // 
            // txtBillingBilledAt
            // 
            this.txtBillingBilledAt.Height = 0.2283465F;
            this.txtBillingBilledAt.Left = 2.362205F;
            this.txtBillingBilledAt.MultiLine = false;
            this.txtBillingBilledAt.Name = "txtBillingBilledAt";
            this.txtBillingBilledAt.OutputFormat = resources.GetString("txtBillingBilledAt.OutputFormat");
            this.txtBillingBilledAt.Style = "text-align: center; vertical-align: middle";
            this.txtBillingBilledAt.Text = null;
            this.txtBillingBilledAt.Top = 0F;
            this.txtBillingBilledAt.Width = 0.7133859F;
            // 
            // txtBillingDueAt
            // 
            this.txtBillingDueAt.Height = 0.2283465F;
            this.txtBillingDueAt.Left = 3.075591F;
            this.txtBillingDueAt.MultiLine = false;
            this.txtBillingDueAt.Name = "txtBillingDueAt";
            this.txtBillingDueAt.OutputFormat = resources.GetString("txtBillingDueAt.OutputFormat");
            this.txtBillingDueAt.Style = "text-align: center; vertical-align: middle";
            this.txtBillingDueAt.Text = null;
            this.txtBillingDueAt.Top = 0F;
            this.txtBillingDueAt.Width = 0.7133861F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.Height = 0.2283465F;
            this.txtBillingAmount.Left = 3.78937F;
            this.txtBillingAmount.MultiLine = false;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.OutputFormat = resources.GetString("txtBillingAmount.OutputFormat");
            this.txtBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingAmount.Style = "text-align: right; vertical-align: middle";
            this.txtBillingAmount.Text = null;
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 0.6641732F;
            // 
            // txtBillingDepartment
            // 
            this.txtBillingDepartment.Height = 0.2283465F;
            this.txtBillingDepartment.Left = 2.362205F;
            this.txtBillingDepartment.MultiLine = false;
            this.txtBillingDepartment.Name = "txtBillingDepartment";
            this.txtBillingDepartment.Style = "text-align: left; vertical-align: middle";
            this.txtBillingDepartment.Text = null;
            this.txtBillingDepartment.Top = 0.2212599F;
            this.txtBillingDepartment.Width = 1.427166F;
            // 
            // txtBillingRemainAmount
            // 
            this.txtBillingRemainAmount.Height = 0.2283465F;
            this.txtBillingRemainAmount.Left = 4.453544F;
            this.txtBillingRemainAmount.MultiLine = false;
            this.txtBillingRemainAmount.Name = "txtBillingRemainAmount";
            this.txtBillingRemainAmount.OutputFormat = resources.GetString("txtBillingRemainAmount.OutputFormat");
            this.txtBillingRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingRemainAmount.Style = "text-align: right; vertical-align: middle";
            this.txtBillingRemainAmount.Text = null;
            this.txtBillingRemainAmount.Top = 0F;
            this.txtBillingRemainAmount.Width = 0.6641732F;
            // 
            // txtMatchingAmount
            // 
            this.txtMatchingAmount.Height = 0.2283465F;
            this.txtMatchingAmount.Left = 5.118111F;
            this.txtMatchingAmount.MultiLine = false;
            this.txtMatchingAmount.Name = "txtMatchingAmount";
            this.txtMatchingAmount.OutputFormat = resources.GetString("txtMatchingAmount.OutputFormat");
            this.txtMatchingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtMatchingAmount.Style = "text-align: right; vertical-align: middle";
            this.txtMatchingAmount.Text = null;
            this.txtMatchingAmount.Top = 0F;
            this.txtMatchingAmount.Width = 0.6641731F;
            // 
            // txtBillingNote1
            // 
            this.txtBillingNote1.Height = 0.2283465F;
            this.txtBillingNote1.Left = 3.78937F;
            this.txtBillingNote1.MultiLine = false;
            this.txtBillingNote1.Name = "txtBillingNote1";
            this.txtBillingNote1.Style = "text-align: left; vertical-align: middle";
            this.txtBillingNote1.Text = null;
            this.txtBillingNote1.Top = 0.2212599F;
            this.txtBillingNote1.Width = 1.992913F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.2283465F;
            this.txtPayerName.Left = 5.782F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtPayerName.Style = "vertical-align: middle";
            this.txtPayerName.Text = null;
            this.txtPayerName.Top = 0F;
            this.txtPayerName.Width = 1.382961F;
            // 
            // txtSourceBankBranchName
            // 
            this.txtSourceBankBranchName.Height = 0.2283465F;
            this.txtSourceBankBranchName.Left = 5.782284F;
            this.txtSourceBankBranchName.MultiLine = false;
            this.txtSourceBankBranchName.Name = "txtSourceBankBranchName";
            this.txtSourceBankBranchName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtSourceBankBranchName.Style = "vertical-align: middle";
            this.txtSourceBankBranchName.Text = null;
            this.txtSourceBankBranchName.Top = 0.2212599F;
            this.txtSourceBankBranchName.Width = 1.382678F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.2283465F;
            this.txtRecordedAt.Left = 7.164961F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "text-align: center; vertical-align: middle";
            this.txtRecordedAt.Text = null;
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.7133859F;
            // 
            // txtAccountType
            // 
            this.txtAccountType.Height = 0.2283465F;
            this.txtAccountType.Left = 7.164961F;
            this.txtAccountType.MultiLine = false;
            this.txtAccountType.Name = "txtAccountType";
            this.txtAccountType.Style = "text-align: center; vertical-align: middle";
            this.txtAccountType.Text = null;
            this.txtAccountType.Top = 0.2212599F;
            this.txtAccountType.Width = 0.7133859F;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Height = 0.2283465F;
            this.txtAccountNumber.Left = 7.888189F;
            this.txtAccountNumber.MultiLine = false;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Style = "text-align: center; vertical-align: middle";
            this.txtAccountNumber.Text = null;
            this.txtAccountNumber.Top = 0.2212599F;
            this.txtAccountNumber.Width = 0.7133861F;
            // 
            // txtReceiptDueAt
            // 
            this.txtReceiptDueAt.Height = 0.2283465F;
            this.txtReceiptDueAt.Left = 8.601576F;
            this.txtReceiptDueAt.MultiLine = false;
            this.txtReceiptDueAt.Name = "txtReceiptDueAt";
            this.txtReceiptDueAt.OutputFormat = resources.GetString("txtReceiptDueAt.OutputFormat");
            this.txtReceiptDueAt.Style = "text-align: center; vertical-align: middle";
            this.txtReceiptDueAt.Text = null;
            this.txtReceiptDueAt.Top = 0F;
            this.txtReceiptDueAt.Width = 0.7133861F;
            // 
            // txtReceiptAmount
            // 
            this.txtReceiptAmount.Height = 0.2283465F;
            this.txtReceiptAmount.Left = 9.314961F;
            this.txtReceiptAmount.MultiLine = false;
            this.txtReceiptAmount.Name = "txtReceiptAmount";
            this.txtReceiptAmount.OutputFormat = resources.GetString("txtReceiptAmount.OutputFormat");
            this.txtReceiptAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptAmount.Style = "text-align: right; vertical-align: middle";
            this.txtReceiptAmount.Text = null;
            this.txtReceiptAmount.Top = 0F;
            this.txtReceiptAmount.Width = 0.5905512F;
            // 
            // txtReceiptRemainAmount
            // 
            this.txtReceiptRemainAmount.Height = 0.2283465F;
            this.txtReceiptRemainAmount.Left = 9.905513F;
            this.txtReceiptRemainAmount.MultiLine = false;
            this.txtReceiptRemainAmount.Name = "txtReceiptRemainAmount";
            this.txtReceiptRemainAmount.OutputFormat = resources.GetString("txtReceiptRemainAmount.OutputFormat");
            this.txtReceiptRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptRemainAmount.Style = "text-align: right; vertical-align: middle; ddo-shrink-to-fit: none";
            this.txtReceiptRemainAmount.Text = null;
            this.txtReceiptRemainAmount.Top = 0F;
            this.txtReceiptRemainAmount.Width = 0.6751965F;
            // 
            // txtReceiptNote1
            // 
            this.txtReceiptNote1.Height = 0.2283465F;
            this.txtReceiptNote1.Left = 8.618111F;
            this.txtReceiptNote1.MultiLine = false;
            this.txtReceiptNote1.Name = "txtReceiptNote1";
            this.txtReceiptNote1.Style = "text-align: left; vertical-align: middle";
            this.txtReceiptNote1.Text = null;
            this.txtReceiptNote1.Top = 0.2212599F;
            this.txtReceiptNote1.Width = 1.962599F;
            // 
            // line21
            // 
            this.line21.Height = 0.4507874F;
            this.line21.Left = 2.362205F;
            this.line21.LineWeight = 1F;
            this.line21.Name = "line21";
            this.line21.Top = 0F;
            this.line21.Width = 0F;
            this.line21.X1 = 2.362205F;
            this.line21.X2 = 2.362205F;
            this.line21.Y1 = 0F;
            this.line21.Y2 = 0.4507874F;
            // 
            // line22
            // 
            this.line22.Height = 0.2216533F;
            this.line22.Left = 3.075591F;
            this.line22.LineWeight = 1F;
            this.line22.Name = "line22";
            this.line22.Top = -5.960464E-08F;
            this.line22.Width = 0F;
            this.line22.X1 = 3.075591F;
            this.line22.X2 = 3.075591F;
            this.line22.Y1 = 0.2216532F;
            this.line22.Y2 = -5.960464E-08F;
            // 
            // line23
            // 
            this.line23.Height = 0.4519687F;
            this.line23.Left = 3.787402F;
            this.line23.LineWeight = 1F;
            this.line23.Name = "line23";
            this.line23.Top = -5.960464E-08F;
            this.line23.Width = 0F;
            this.line23.X1 = 3.787402F;
            this.line23.X2 = 3.787402F;
            this.line23.Y1 = -5.960464E-08F;
            this.line23.Y2 = 0.4519686F;
            // 
            // line24
            // 
            this.line24.Height = 0.2216529F;
            this.line24.Left = 4.453544F;
            this.line24.LineWeight = 1F;
            this.line24.Name = "line24";
            this.line24.Top = 0F;
            this.line24.Width = 0F;
            this.line24.X1 = 4.453544F;
            this.line24.X2 = 4.453544F;
            this.line24.Y1 = 0.2216529F;
            this.line24.Y2 = 0F;
            // 
            // line25
            // 
            this.line25.Height = 0.2216529F;
            this.line25.Left = 5.118111F;
            this.line25.LineWeight = 1F;
            this.line25.Name = "line25";
            this.line25.Top = 0F;
            this.line25.Width = 0F;
            this.line25.X1 = 5.118111F;
            this.line25.X2 = 5.118111F;
            this.line25.Y1 = 0.2216529F;
            this.line25.Y2 = 0F;
            // 
            // line26
            // 
            this.line26.Height = 0.4507874F;
            this.line26.Left = 5.782284F;
            this.line26.LineWeight = 1F;
            this.line26.Name = "line26";
            this.line26.Top = 3.552714E-15F;
            this.line26.Width = 0F;
            this.line26.X1 = 5.782284F;
            this.line26.X2 = 5.782284F;
            this.line26.Y1 = 3.552714E-15F;
            this.line26.Y2 = 0.4507874F;
            // 
            // line29
            // 
            this.line29.Height = 0.4507874F;
            this.line29.Left = 8.601576F;
            this.line29.LineWeight = 1F;
            this.line29.Name = "line29";
            this.line29.Top = 0F;
            this.line29.Width = 0F;
            this.line29.X1 = 8.601576F;
            this.line29.X2 = 8.601576F;
            this.line29.Y1 = 0F;
            this.line29.Y2 = 0.4507874F;
            // 
            // line30
            // 
            this.line30.Height = 0.2216526F;
            this.line30.Left = 9.314961F;
            this.line30.LineWeight = 1F;
            this.line30.Name = "line30";
            this.line30.Top = 1.02818E-06F;
            this.line30.Width = 0F;
            this.line30.X1 = 9.314961F;
            this.line30.X2 = 9.314961F;
            this.line30.Y1 = 0.2216536F;
            this.line30.Y2 = 1.02818E-06F;
            // 
            // line31
            // 
            this.line31.Height = 0.2216526F;
            this.line31.Left = 9.905513F;
            this.line31.LineWeight = 1F;
            this.line31.Name = "line31";
            this.line31.Top = 1.02818E-06F;
            this.line31.Width = 0F;
            this.line31.X1 = 9.905513F;
            this.line31.X2 = 9.905513F;
            this.line31.Y1 = 0.2216536F;
            this.line31.Y2 = 1.02818E-06F;
            // 
            // line27
            // 
            this.line27.Height = 0.4507874F;
            this.line27.Left = 7.164961F;
            this.line27.LineWeight = 1F;
            this.line27.Name = "line27";
            this.line27.Top = 0F;
            this.line27.Width = 0F;
            this.line27.X1 = 7.164961F;
            this.line27.X2 = 7.164961F;
            this.line27.Y1 = 0F;
            this.line27.Y2 = 0.4507874F;
            // 
            // txtReceiptCateoryName
            // 
            this.txtReceiptCateoryName.Height = 0.2283465F;
            this.txtReceiptCateoryName.Left = 7.888189F;
            this.txtReceiptCateoryName.MultiLine = false;
            this.txtReceiptCateoryName.Name = "txtReceiptCateoryName";
            this.txtReceiptCateoryName.Style = "vertical-align: middle";
            this.txtReceiptCateoryName.Text = null;
            this.txtReceiptCateoryName.Top = 0F;
            this.txtReceiptCateoryName.Width = 0.7133859F;
            // 
            // line28
            // 
            this.line28.Height = 0.4519687F;
            this.line28.Left = 7.878347F;
            this.line28.LineWeight = 1F;
            this.line28.Name = "line28";
            this.line28.Top = 0F;
            this.line28.Width = 0F;
            this.line28.X1 = 7.878347F;
            this.line28.X2 = 7.878347F;
            this.line28.Y1 = 0F;
            this.line28.Y2 = 0.4519687F;
            // 
            // txtBillingCategoryName
            // 
            this.txtBillingCategoryName.Height = 0.2283465F;
            this.txtBillingCategoryName.Left = 0F;
            this.txtBillingCategoryName.MultiLine = false;
            this.txtBillingCategoryName.Name = "txtBillingCategoryName";
            this.txtBillingCategoryName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtBillingCategoryName.Style = "vertical-align: middle";
            this.txtBillingCategoryName.Text = null;
            this.txtBillingCategoryName.Top = 0.221F;
            this.txtBillingCategoryName.Width = 0.886F;
            // 
            // line19
            // 
            this.line19.Height = 0F;
            this.line19.Left = 0F;
            this.line19.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line19.LineWeight = 1F;
            this.line19.Name = "line19";
            this.line19.Top = 0.2212599F;
            this.line19.Width = 10.58071F;
            this.line19.X1 = 0F;
            this.line19.X2 = 10.58071F;
            this.line19.Y1 = 0.2212599F;
            this.line19.Y2 = 0.2212599F;
            // 
            // line36
            // 
            this.line36.Height = 0F;
            this.line36.Left = 0F;
            this.line36.LineWeight = 1F;
            this.line36.Name = "line36";
            this.line36.Top = 0.4429134F;
            this.line36.Width = 10.58071F;
            this.line36.X1 = 0F;
            this.line36.X2 = 10.58071F;
            this.line36.Y1 = 0.4429134F;
            this.line36.Y2 = 0.4429134F;
            // 
            // line18
            // 
            this.line18.Height = 0.4507874F;
            this.line18.Left = 0.8858268F;
            this.line18.LineWeight = 1F;
            this.line18.Name = "line18";
            this.line18.Top = 3.552714E-15F;
            this.line18.Width = 0F;
            this.line18.X1 = 0.8858268F;
            this.line18.X2 = 0.8858268F;
            this.line18.Y1 = 3.552714E-15F;
            this.line18.Y2 = 0.4507874F;
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
            this.line32,
            this.line34,
            this.line33,
            this.line35,
            this.lnDiscountAmt});
            this.groupFooter1.Height = 0.5457186F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // txtBillingCount
            // 
            this.txtBillingCount.Height = 0.2283465F;
            this.txtBillingCount.Left = 4.605512F;
            this.txtBillingCount.Name = "txtBillingCount";
            this.txtBillingCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingCount.Style = "text-align: right; vertical-align: middle";
            this.txtBillingCount.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count;
            this.txtBillingCount.Text = null;
            this.txtBillingCount.Top = 0F;
            this.txtBillingCount.Width = 1.151968F;
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.Height = 0.2283465F;
            this.lblDiscountAmount.HyperLink = null;
            this.lblDiscountAmount.Left = 2.449213F;
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            this.lblDiscountAmount.Style = "vertical-align: middle";
            this.lblDiscountAmount.Text = "歩引額計";
            this.lblDiscountAmount.Top = 0F;
            this.lblDiscountAmount.Width = 0.7133859F;
            // 
            // lblBillingTaxDifference
            // 
            this.lblBillingTaxDifference.Height = 0.2283465F;
            this.lblBillingTaxDifference.HyperLink = null;
            this.lblBillingTaxDifference.Left = 2.449213F;
            this.lblBillingTaxDifference.Name = "lblBillingTaxDifference";
            this.lblBillingTaxDifference.Style = "vertical-align: middle";
            this.lblBillingTaxDifference.Text = "消費税誤差";
            this.lblBillingTaxDifference.Top = 0.2212599F;
            this.lblBillingTaxDifference.Width = 0.7133859F;
            // 
            // lblBillingCount
            // 
            this.lblBillingCount.Height = 0.2283465F;
            this.lblBillingCount.Left = 3.941339F;
            this.lblBillingCount.Name = "lblBillingCount";
            this.lblBillingCount.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.lblBillingCount.Style = "vertical-align: middle";
            this.lblBillingCount.Text = "件数";
            this.lblBillingCount.Top = 0F;
            this.lblBillingCount.Width = 0.6641732F;
            // 
            // txtBillingTotal
            // 
            this.txtBillingTotal.Height = 0.2283465F;
            this.txtBillingTotal.Left = 4.605512F;
            this.txtBillingTotal.Name = "txtBillingTotal";
            this.txtBillingTotal.OutputFormat = resources.GetString("txtBillingTotal.OutputFormat");
            this.txtBillingTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingTotal.Style = "text-align: right; vertical-align: middle";
            this.txtBillingTotal.SummaryGroup = "groupHeader1";
            this.txtBillingTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtBillingTotal.Text = null;
            this.txtBillingTotal.Top = 0.2212599F;
            this.txtBillingTotal.Width = 1.151968F;
            // 
            // lblBillingAmountTotal
            // 
            this.lblBillingAmountTotal.Height = 0.2283465F;
            this.lblBillingAmountTotal.Left = 3.941339F;
            this.lblBillingAmountTotal.Name = "lblBillingAmountTotal";
            this.lblBillingAmountTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.lblBillingAmountTotal.Style = "vertical-align: middle";
            this.lblBillingAmountTotal.Text = "請求額計";
            this.lblBillingAmountTotal.Top = 0.2212599F;
            this.lblBillingAmountTotal.Width = 0.6641732F;
            // 
            // lblBankTransferFee
            // 
            this.lblBankTransferFee.Height = 0.2283465F;
            this.lblBankTransferFee.HyperLink = null;
            this.lblBankTransferFee.Left = 7.457875F;
            this.lblBankTransferFee.Name = "lblBankTransferFee";
            this.lblBankTransferFee.Style = "vertical-align: middle";
            this.lblBankTransferFee.Text = "手数料計";
            this.lblBankTransferFee.Top = 0F;
            this.lblBankTransferFee.Width = 0.7728338F;
            // 
            // lblReceiptCount
            // 
            this.lblReceiptCount.Height = 0.2283465F;
            this.lblReceiptCount.Left = 9.037008F;
            this.lblReceiptCount.Name = "lblReceiptCount";
            this.lblReceiptCount.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.lblReceiptCount.Style = "vertical-align: middle";
            this.lblReceiptCount.Text = "件数";
            this.lblReceiptCount.Top = 0F;
            this.lblReceiptCount.Width = 0.5905512F;
            // 
            // lblReceiptTaxDifference
            // 
            this.lblReceiptTaxDifference.Height = 0.2283465F;
            this.lblReceiptTaxDifference.HyperLink = null;
            this.lblReceiptTaxDifference.Left = 7.457875F;
            this.lblReceiptTaxDifference.Name = "lblReceiptTaxDifference";
            this.lblReceiptTaxDifference.Style = "vertical-align: middle";
            this.lblReceiptTaxDifference.Text = "消費税誤差";
            this.lblReceiptTaxDifference.Top = 0.2212599F;
            this.lblReceiptTaxDifference.Width = 0.7728338F;
            // 
            // lblReceiptAmountTotal
            // 
            this.lblReceiptAmountTotal.Height = 0.2283465F;
            this.lblReceiptAmountTotal.Left = 9.037008F;
            this.lblReceiptAmountTotal.Name = "lblReceiptAmountTotal";
            this.lblReceiptAmountTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.lblReceiptAmountTotal.Style = "vertical-align: middle";
            this.lblReceiptAmountTotal.Text = "入金額計";
            this.lblReceiptAmountTotal.Top = 0.2212599F;
            this.lblReceiptAmountTotal.Width = 0.5905512F;
            // 
            // txtReceiptCount
            // 
            this.txtReceiptCount.Height = 0.2283465F;
            this.txtReceiptCount.Left = 9.62756F;
            this.txtReceiptCount.Name = "txtReceiptCount";
            this.txtReceiptCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptCount.Style = "text-align: right; vertical-align: middle";
            this.txtReceiptCount.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count;
            this.txtReceiptCount.Text = null;
            this.txtReceiptCount.Top = 0F;
            this.txtReceiptCount.Width = 0.9531496F;
            // 
            // txtReceiptTotal
            // 
            this.txtReceiptTotal.Height = 0.2283465F;
            this.txtReceiptTotal.Left = 9.62756F;
            this.txtReceiptTotal.Name = "txtReceiptTotal";
            this.txtReceiptTotal.OutputFormat = resources.GetString("txtReceiptTotal.OutputFormat");
            this.txtReceiptTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptTotal.Style = "text-align: right; vertical-align: middle";
            this.txtReceiptTotal.SummaryGroup = "groupHeader1";
            this.txtReceiptTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtReceiptTotal.Text = null;
            this.txtReceiptTotal.Top = 0.2212599F;
            this.txtReceiptTotal.Width = 0.9531498F;
            // 
            // txtBankTransferFee
            // 
            this.txtBankTransferFee.Height = 0.2283465F;
            this.txtBankTransferFee.Left = 8.168505F;
            this.txtBankTransferFee.Name = "txtBankTransferFee";
            this.txtBankTransferFee.OutputFormat = resources.GetString("txtBankTransferFee.OutputFormat");
            this.txtBankTransferFee.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBankTransferFee.Style = "text-align: right; vertical-align: middle";
            this.txtBankTransferFee.Text = null;
            this.txtBankTransferFee.Top = 0F;
            this.txtBankTransferFee.Width = 0.8685036F;
            // 
            // txtReceiptTaxDifference
            // 
            this.txtReceiptTaxDifference.Height = 0.2283465F;
            this.txtReceiptTaxDifference.Left = 8.168505F;
            this.txtReceiptTaxDifference.Name = "txtReceiptTaxDifference";
            this.txtReceiptTaxDifference.OutputFormat = resources.GetString("txtReceiptTaxDifference.OutputFormat");
            this.txtReceiptTaxDifference.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptTaxDifference.Style = "text-align: right; vertical-align: middle";
            this.txtReceiptTaxDifference.Text = null;
            this.txtReceiptTaxDifference.Top = 0.2212599F;
            this.txtReceiptTaxDifference.Width = 0.8685036F;
            // 
            // txtBillingDiscountAmount
            // 
            this.txtBillingDiscountAmount.Height = 0.2283465F;
            this.txtBillingDiscountAmount.Left = 3.162599F;
            this.txtBillingDiscountAmount.Name = "txtBillingDiscountAmount";
            this.txtBillingDiscountAmount.OutputFormat = resources.GetString("txtBillingDiscountAmount.OutputFormat");
            this.txtBillingDiscountAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingDiscountAmount.Style = "text-align: right; vertical-align: middle";
            this.txtBillingDiscountAmount.Text = null;
            this.txtBillingDiscountAmount.Top = 0F;
            this.txtBillingDiscountAmount.Width = 0.7787402F;
            // 
            // txtBillingTaxDifference
            // 
            this.txtBillingTaxDifference.Height = 0.2283465F;
            this.txtBillingTaxDifference.Left = 3.162599F;
            this.txtBillingTaxDifference.Name = "txtBillingTaxDifference";
            this.txtBillingTaxDifference.OutputFormat = resources.GetString("txtBillingTaxDifference.OutputFormat");
            this.txtBillingTaxDifference.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingTaxDifference.Style = "text-align: right; vertical-align: middle";
            this.txtBillingTaxDifference.Text = null;
            this.txtBillingTaxDifference.Top = 0.2212599F;
            this.txtBillingTaxDifference.Width = 0.7787402F;
            // 
            // line32
            // 
            this.line32.Height = 0F;
            this.line32.Left = 4.293307F;
            this.line32.LineWeight = 1F;
            this.line32.Name = "line32";
            this.line32.Top = 0.2283465F;
            this.line32.Width = 1.464567F;
            this.line32.X1 = 4.293307F;
            this.line32.X2 = 5.757874F;
            this.line32.Y1 = 0.2283465F;
            this.line32.Y2 = 0.2283465F;
            // 
            // line34
            // 
            this.line34.Height = 0F;
            this.line34.Left = 7.457875F;
            this.line34.LineWeight = 1F;
            this.line34.Name = "line34";
            this.line34.Top = 0.2212599F;
            this.line34.Width = 3.122836F;
            this.line34.X1 = 7.457875F;
            this.line34.X2 = 10.58071F;
            this.line34.Y1 = 0.2212599F;
            this.line34.Y2 = 0.2212599F;
            // 
            // line33
            // 
            this.line33.Height = 0F;
            this.line33.Left = 7.457874F;
            this.line33.LineWeight = 1F;
            this.line33.Name = "line33";
            this.line33.Top = 0.4429134F;
            this.line33.Width = 3.122837F;
            this.line33.X1 = 7.457874F;
            this.line33.X2 = 10.58071F;
            this.line33.Y1 = 0.4429134F;
            this.line33.Y2 = 0.4429134F;
            // 
            // line35
            // 
            this.line35.Height = 0F;
            this.line35.Left = 2.449213F;
            this.line35.LineWeight = 1F;
            this.line35.Name = "line35";
            this.line35.Top = 0.4429134F;
            this.line35.Width = 3.308661F;
            this.line35.X1 = 2.449213F;
            this.line35.X2 = 5.757874F;
            this.line35.Y1 = 0.4429134F;
            this.line35.Y2 = 0.4429134F;
            // 
            // lnDiscountAmt
            // 
            this.lnDiscountAmt.Height = 0F;
            this.lnDiscountAmt.Left = 2.449213F;
            this.lnDiscountAmt.LineWeight = 1F;
            this.lnDiscountAmt.Name = "lnDiscountAmt";
            this.lnDiscountAmt.Top = 0.2283465F;
            this.lnDiscountAmt.Width = 1.844094F;
            this.lnDiscountAmt.X1 = 4.293307F;
            this.lnDiscountAmt.X2 = 2.449213F;
            this.lnDiscountAmt.Y1 = 0.2283465F;
            this.lnDiscountAmt.Y2 = 0.2283465F;
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyName,
            this.lblOutputDate,
            this.riOutputDate,
            this.lblTitle,
            this.label11,
            this.label6,
            this.label1,
            this.label2,
            this.label3,
            this.label4,
            this.label7,
            this.label8,
            this.label9,
            this.label10,
            this.lblBillingRemark,
            this.label14,
            this.label15,
            this.label16,
            this.label17,
            this.label18,
            this.label19,
            this.label20,
            this.label22,
            this.label23,
            this.line1,
            this.lblDepartment,
            this.lblReceiptRemark,
            this.line3,
            this.line4,
            this.line5,
            this.line6,
            this.line7,
            this.line9,
            this.line8,
            this.line10,
            this.line11,
            this.line12,
            this.line14,
            this.line15,
            this.line16,
            this.line17,
            this.lblBillingInfo,
            this.lblReceiptInfo});
            this.pageHeader.Height = 1.249999F;
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
            // label6
            // 
            this.label6.Height = 0.2283465F;
            this.label6.HyperLink = null;
            this.label6.Left = 0F;
            this.label6.Name = "label6";
            this.label6.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label6.Text = "得意先コード";
            this.label6.Top = 0.7905512F;
            this.label6.Width = 0.8858268F;
            // 
            // label1
            // 
            this.label1.Height = 0.2283465F;
            this.label1.HyperLink = null;
            this.label1.Left = 0F;
            this.label1.Name = "label1";
            this.label1.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label1.Text = "請求区分";
            this.label1.Top = 1.012204F;
            this.label1.Width = 0.8858268F;
            // 
            // label2
            // 
            this.label2.Height = 0.2283465F;
            this.label2.HyperLink = null;
            this.label2.Left = 0.8858268F;
            this.label2.Name = "label2";
            this.label2.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label2.Text = "得意先名";
            this.label2.Top = 0.7905512F;
            this.label2.Width = 1.476378F;
            // 
            // label3
            // 
            this.label3.Height = 0.2283465F;
            this.label3.HyperLink = null;
            this.label3.Left = 0.8858268F;
            this.label3.Name = "label3";
            this.label3.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label3.Text = "請求書番号";
            this.label3.Top = 1.012204F;
            this.label3.Width = 1.476378F;
            // 
            // label4
            // 
            this.label4.Height = 0.2283465F;
            this.label4.HyperLink = null;
            this.label4.Left = 2.362205F;
            this.label4.Name = "label4";
            this.label4.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label4.Text = "請求日";
            this.label4.Top = 0.7905512F;
            this.label4.Width = 0.7133861F;
            // 
            // label7
            // 
            this.label7.Height = 0.2283465F;
            this.label7.HyperLink = null;
            this.label7.Left = 3.075591F;
            this.label7.Name = "label7";
            this.label7.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label7.Text = "入金予定日";
            this.label7.Top = 0.7905512F;
            this.label7.Width = 0.7133859F;
            // 
            // label8
            // 
            this.label8.Height = 0.2283465F;
            this.label8.HyperLink = null;
            this.label8.Left = 3.78937F;
            this.label8.Name = "label8";
            this.label8.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label8.Text = "請求額";
            this.label8.Top = 0.7905512F;
            this.label8.Width = 0.6641732F;
            // 
            // label9
            // 
            this.label9.Height = 0.2283465F;
            this.label9.HyperLink = null;
            this.label9.Left = 4.453544F;
            this.label9.Name = "label9";
            this.label9.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label9.Text = "請求残";
            this.label9.Top = 0.7905512F;
            this.label9.Width = 0.6641732F;
            // 
            // label10
            // 
            this.label10.Height = 0.2283465F;
            this.label10.HyperLink = null;
            this.label10.Left = 5.118111F;
            this.label10.Name = "label10";
            this.label10.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label10.Text = "消込対象額";
            this.label10.Top = 0.7905512F;
            this.label10.Width = 0.6641732F;
            // 
            // lblBillingRemark
            // 
            this.lblBillingRemark.Height = 0.2283465F;
            this.lblBillingRemark.HyperLink = null;
            this.lblBillingRemark.Left = 3.78937F;
            this.lblBillingRemark.Name = "lblBillingRemark";
            this.lblBillingRemark.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblBillingRemark.Text = "備考";
            this.lblBillingRemark.Top = 1.012204F;
            this.lblBillingRemark.Width = 1.992914F;
            // 
            // label14
            // 
            this.label14.Height = 0.2283465F;
            this.label14.HyperLink = null;
            this.label14.Left = 7.164961F;
            this.label14.Name = "label14";
            this.label14.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label14.Text = "入金日";
            this.label14.Top = 0.7905512F;
            this.label14.Width = 0.7133859F;
            // 
            // label15
            // 
            this.label15.Height = 0.2283465F;
            this.label15.HyperLink = null;
            this.label15.Left = 7.164961F;
            this.label15.Name = "label15";
            this.label15.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label15.Text = "種別";
            this.label15.Top = 1.012204F;
            this.label15.Width = 0.7133859F;
            // 
            // label16
            // 
            this.label16.Height = 0.2283465F;
            this.label16.HyperLink = null;
            this.label16.Left = 5.782284F;
            this.label16.Name = "label16";
            this.label16.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label16.Text = "振込依頼人名";
            this.label16.Top = 0.7905512F;
            this.label16.Width = 1.382678F;
            // 
            // label17
            // 
            this.label17.Height = 0.2283465F;
            this.label17.HyperLink = null;
            this.label17.Left = 5.782284F;
            this.label17.Name = "label17";
            this.label17.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label17.Text = "仕向銀行 / 仕向支店";
            this.label17.Top = 1.012204F;
            this.label17.Width = 1.382678F;
            // 
            // label18
            // 
            this.label18.Height = 0.2283465F;
            this.label18.HyperLink = null;
            this.label18.Left = 7.888189F;
            this.label18.Name = "label18";
            this.label18.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label18.Text = "入金区分";
            this.label18.Top = 0.7905512F;
            this.label18.Width = 0.7133859F;
            // 
            // label19
            // 
            this.label19.Height = 0.2283465F;
            this.label19.HyperLink = null;
            this.label19.Left = 7.888189F;
            this.label19.Name = "label19";
            this.label19.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label19.Text = "口座番号";
            this.label19.Top = 1.012204F;
            this.label19.Width = 0.7133859F;
            // 
            // label20
            // 
            this.label20.Height = 0.2283465F;
            this.label20.HyperLink = null;
            this.label20.Left = 8.601576F;
            this.label20.Name = "label20";
            this.label20.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label20.Text = "期日";
            this.label20.Top = 0.7905512F;
            this.label20.Width = 0.7133859F;
            // 
            // label22
            // 
            this.label22.Height = 0.2283465F;
            this.label22.HyperLink = null;
            this.label22.Left = 9.916142F;
            this.label22.Name = "label22";
            this.label22.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label22.Text = "入金残";
            this.label22.Top = 0.7905512F;
            this.label22.Width = 0.6692914F;
            // 
            // label23
            // 
            this.label23.Height = 0.2283465F;
            this.label23.HyperLink = null;
            this.label23.Left = 9.314961F;
            this.label23.Name = "label23";
            this.label23.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label23.Text = "入金額";
            this.label23.Top = 0.7905512F;
            this.label23.Width = 0.5905517F;
            // 
            // line1
            // 
            this.line1.Height = 0F;
            this.line1.Left = 0F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0.7905512F;
            this.line1.Width = 10.58071F;
            this.line1.X1 = 0F;
            this.line1.X2 = 10.58071F;
            this.line1.Y1 = 0.7905512F;
            this.line1.Y2 = 0.7905512F;
            // 
            // lblDepartment
            // 
            this.lblDepartment.Height = 0.2283465F;
            this.lblDepartment.HyperLink = null;
            this.lblDepartment.Left = 2.362205F;
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartment.Text = "請求部門";
            this.lblDepartment.Top = 1.012204F;
            this.lblDepartment.Width = 1.417323F;
            // 
            // lblReceiptRemark
            // 
            this.lblReceiptRemark.Height = 0.2283465F;
            this.lblReceiptRemark.HyperLink = null;
            this.lblReceiptRemark.Left = 8.601576F;
            this.lblReceiptRemark.Name = "lblReceiptRemark";
            this.lblReceiptRemark.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblReceiptRemark.Text = "備考";
            this.lblReceiptRemark.Top = 1.012204F;
            this.lblReceiptRemark.Width = 1.979134F;
            // 
            // line3
            // 
            this.line3.Height = 0F;
            this.line3.Left = 0F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 1.236614F;
            this.line3.Width = 10.58071F;
            this.line3.X1 = 0F;
            this.line3.X2 = 10.58071F;
            this.line3.Y1 = 1.236614F;
            this.line3.Y2 = 1.236614F;
            // 
            // line4
            // 
            this.line4.Height = 0F;
            this.line4.Left = 0F;
            this.line4.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.Top = 1.012204F;
            this.line4.Width = 10.58071F;
            this.line4.X1 = 0F;
            this.line4.X2 = 10.58071F;
            this.line4.Y1 = 1.012204F;
            this.line4.Y2 = 1.012204F;
            // 
            // line5
            // 
            this.line5.Height = 0.4519678F;
            this.line5.Left = 0.8858268F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0.7905512F;
            this.line5.Width = 0F;
            this.line5.X1 = 0.8858268F;
            this.line5.X2 = 0.8858268F;
            this.line5.Y1 = 0.7905512F;
            this.line5.Y2 = 1.242519F;
            // 
            // line6
            // 
            this.line6.Height = 0.4519688F;
            this.line6.Left = 2.362205F;
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.Top = 0.7905512F;
            this.line6.Width = 0F;
            this.line6.X1 = 2.362205F;
            this.line6.X2 = 2.362205F;
            this.line6.Y1 = 0.7905512F;
            this.line6.Y2 = 1.24252F;
            // 
            // line7
            // 
            this.line7.Height = 0.2216529F;
            this.line7.Left = 3.075591F;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.Top = 0.7905512F;
            this.line7.Width = 0F;
            this.line7.X1 = 3.075591F;
            this.line7.X2 = 3.075591F;
            this.line7.Y1 = 1.012204F;
            this.line7.Y2 = 0.7905512F;
            // 
            // line9
            // 
            this.line9.Height = 0.4519688F;
            this.line9.Left = 5.782284F;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.Top = 0.7905512F;
            this.line9.Width = 0F;
            this.line9.X1 = 5.782284F;
            this.line9.X2 = 5.782284F;
            this.line9.Y1 = 0.7905512F;
            this.line9.Y2 = 1.24252F;
            // 
            // line8
            // 
            this.line8.Height = 0.4519688F;
            this.line8.Left = 3.787402F;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.Top = 0.7905512F;
            this.line8.Width = 0F;
            this.line8.X1 = 3.787402F;
            this.line8.X2 = 3.787402F;
            this.line8.Y1 = 0.7905512F;
            this.line8.Y2 = 1.24252F;
            // 
            // line10
            // 
            this.line10.Height = 0.2216529F;
            this.line10.Left = 5.118111F;
            this.line10.LineWeight = 1F;
            this.line10.Name = "line10";
            this.line10.Top = 0.7905512F;
            this.line10.Width = 0F;
            this.line10.X1 = 5.118111F;
            this.line10.X2 = 5.118111F;
            this.line10.Y1 = 1.012204F;
            this.line10.Y2 = 0.7905512F;
            // 
            // line11
            // 
            this.line11.Height = 0.2216529F;
            this.line11.Left = 4.453544F;
            this.line11.LineWeight = 1F;
            this.line11.Name = "line11";
            this.line11.Top = 0.7905512F;
            this.line11.Width = 0F;
            this.line11.X1 = 4.453544F;
            this.line11.X2 = 4.453544F;
            this.line11.Y1 = 1.012204F;
            this.line11.Y2 = 0.7905512F;
            // 
            // line12
            // 
            this.line12.Height = 0.4519678F;
            this.line12.Left = 7.164961F;
            this.line12.LineWeight = 1F;
            this.line12.Name = "line12";
            this.line12.Top = 0.7905512F;
            this.line12.Width = 0F;
            this.line12.X1 = 7.164961F;
            this.line12.X2 = 7.164961F;
            this.line12.Y1 = 0.7905512F;
            this.line12.Y2 = 1.242519F;
            // 
            // line14
            // 
            this.line14.Height = 0.4519688F;
            this.line14.Left = 7.878347F;
            this.line14.LineWeight = 1F;
            this.line14.Name = "line14";
            this.line14.Top = 0.7905512F;
            this.line14.Width = 0F;
            this.line14.X1 = 7.878347F;
            this.line14.X2 = 7.878347F;
            this.line14.Y1 = 0.7905512F;
            this.line14.Y2 = 1.24252F;
            // 
            // line15
            // 
            this.line15.Height = 0.4519678F;
            this.line15.Left = 8.601576F;
            this.line15.LineWeight = 1F;
            this.line15.Name = "line15";
            this.line15.Top = 0.7905512F;
            this.line15.Width = 0F;
            this.line15.X1 = 8.601576F;
            this.line15.X2 = 8.601576F;
            this.line15.Y1 = 0.7905512F;
            this.line15.Y2 = 1.242519F;
            // 
            // line16
            // 
            this.line16.Height = 0.2216529F;
            this.line16.Left = 9.314961F;
            this.line16.LineWeight = 1F;
            this.line16.Name = "line16";
            this.line16.Top = 0.7905512F;
            this.line16.Width = 0F;
            this.line16.X1 = 9.314961F;
            this.line16.X2 = 9.314961F;
            this.line16.Y1 = 1.012204F;
            this.line16.Y2 = 0.7905512F;
            // 
            // line17
            // 
            this.line17.Height = 0.2216529F;
            this.line17.Left = 9.905513F;
            this.line17.LineWeight = 1F;
            this.line17.Name = "line17";
            this.line17.Top = 0.7905512F;
            this.line17.Width = 0F;
            this.line17.X1 = 9.905513F;
            this.line17.X2 = 9.905513F;
            this.line17.Y1 = 1.012204F;
            this.line17.Y2 = 0.7905512F;
            // 
            // lblBillingInfo
            // 
            this.lblBillingInfo.Height = 0.2F;
            this.lblBillingInfo.HyperLink = null;
            this.lblBillingInfo.Left = 0.1129921F;
            this.lblBillingInfo.Name = "lblBillingInfo";
            this.lblBillingInfo.Style = "font-weight: bold; text-align: center; vertical-align: middle";
            this.lblBillingInfo.Text = " <請求情報>";
            this.lblBillingInfo.Top = 0.5708662F;
            this.lblBillingInfo.Width = 1F;
            // 
            // lblReceiptInfo
            // 
            this.lblReceiptInfo.Height = 0.2F;
            this.lblReceiptInfo.HyperLink = null;
            this.lblReceiptInfo.Left = 6.042913F;
            this.lblReceiptInfo.Name = "lblReceiptInfo";
            this.lblReceiptInfo.Style = "font-weight: bold; text-align: center; vertical-align: middle";
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
            // MatchingIndividualSectionReport
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
            ((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartment)).EndInit();
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
        private GrapeCity.ActiveReports.SectionReportModel.Line line21;
        private GrapeCity.ActiveReports.SectionReportModel.Line line22;
        private GrapeCity.ActiveReports.SectionReportModel.Line line23;
        private GrapeCity.ActiveReports.SectionReportModel.Line line24;
        private GrapeCity.ActiveReports.SectionReportModel.Line line25;
        private GrapeCity.ActiveReports.SectionReportModel.Line line26;
        private GrapeCity.ActiveReports.SectionReportModel.Line line29;
        private GrapeCity.ActiveReports.SectionReportModel.Line line30;
        private GrapeCity.ActiveReports.SectionReportModel.Line line31;
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
        private GrapeCity.ActiveReports.SectionReportModel.Line line32;
        private GrapeCity.ActiveReports.SectionReportModel.Line line34;
        private GrapeCity.ActiveReports.SectionReportModel.Line line33;
        private GrapeCity.ActiveReports.SectionReportModel.Line line35;
        private GrapeCity.ActiveReports.SectionReportModel.Line line27;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptCateoryName;
        private GrapeCity.ActiveReports.SectionReportModel.Line line28;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingCategoryName;
        private GrapeCity.ActiveReports.SectionReportModel.Line line19;
        private GrapeCity.ActiveReports.SectionReportModel.Line line36;
        private GrapeCity.ActiveReports.SectionReportModel.Line line18;
        private GrapeCity.ActiveReports.SectionReportModel.PageHeader pageHeader;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblOutputDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo riOutputDate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label label11;
        private GrapeCity.ActiveReports.SectionReportModel.Label label6;
        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        private GrapeCity.ActiveReports.SectionReportModel.Label label2;
        private GrapeCity.ActiveReports.SectionReportModel.Label label3;
        private GrapeCity.ActiveReports.SectionReportModel.Label label4;
        private GrapeCity.ActiveReports.SectionReportModel.Label label7;
        private GrapeCity.ActiveReports.SectionReportModel.Label label8;
        private GrapeCity.ActiveReports.SectionReportModel.Label label9;
        private GrapeCity.ActiveReports.SectionReportModel.Label label10;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingRemark;
        private GrapeCity.ActiveReports.SectionReportModel.Label label14;
        private GrapeCity.ActiveReports.SectionReportModel.Label label15;
        private GrapeCity.ActiveReports.SectionReportModel.Label label16;
        private GrapeCity.ActiveReports.SectionReportModel.Label label17;
        private GrapeCity.ActiveReports.SectionReportModel.Label label18;
        private GrapeCity.ActiveReports.SectionReportModel.Label label19;
        private GrapeCity.ActiveReports.SectionReportModel.Label label20;
        private GrapeCity.ActiveReports.SectionReportModel.Label label22;
        private GrapeCity.ActiveReports.SectionReportModel.Label label23;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptRemark;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.Line line4;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        private GrapeCity.ActiveReports.SectionReportModel.Line line6;
        private GrapeCity.ActiveReports.SectionReportModel.Line line7;
        private GrapeCity.ActiveReports.SectionReportModel.Line line9;
        private GrapeCity.ActiveReports.SectionReportModel.Line line8;
        private GrapeCity.ActiveReports.SectionReportModel.Line line10;
        private GrapeCity.ActiveReports.SectionReportModel.Line line11;
        private GrapeCity.ActiveReports.SectionReportModel.Line line12;
        private GrapeCity.ActiveReports.SectionReportModel.Line line14;
        private GrapeCity.ActiveReports.SectionReportModel.Line line15;
        private GrapeCity.ActiveReports.SectionReportModel.Line line16;
        private GrapeCity.ActiveReports.SectionReportModel.Line line17;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingInfo;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptInfo;
        private GrapeCity.ActiveReports.SectionReportModel.PageFooter pageFooter;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lnDiscountAmt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        public GrapeCity.ActiveReports.SectionReportModel.Shape shpBilling;
    }
}
