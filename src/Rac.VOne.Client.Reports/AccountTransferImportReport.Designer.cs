namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillSectionReport の概要の説明です。
    /// </summary>
    partial class AccountTransferImportReport
    {
        private GrapeCity.ActiveReports.SectionReportModel.PageHeader pageHeader;
        private GrapeCity.ActiveReports.SectionReportModel.Detail detail;
        private GrapeCity.ActiveReports.SectionReportModel.PageFooter pageFooter;

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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AccountTransferImportReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblBillingCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingNote1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingSalesAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTransferBranchName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTransferBankName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTransferAccountName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTransferCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTransferResult = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTransferAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCredit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBankCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtTransferResult = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTransferAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTransferCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTransferAccountName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTransferBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTransferBankName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingClosingAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingSalesAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCredit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBankCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line14 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTotalBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTotalTransferAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTotalCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferAccountName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferAccountName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalTransferAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblBillingCustomerName,
            this.lblBillingCustomerCode,
            this.lblBillingNote1,
            this.lblBillingInvoiceCode,
            this.lblBillingBilledAt,
            this.lblBillingClosingAt,
            this.lblBillingDueAt,
            this.lblBillingSalesAt,
            this.lblTransferBranchName,
            this.lblTransferBankName,
            this.lblTransferAccountName,
            this.lblTransferCustomerCode,
            this.lblTransferResult,
            this.lblTransferAmount,
            this.lblBillingDepartmentCode,
            this.lblBillingDepartmentName,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblBillingAmount,
            this.lineHeaderVerSlipNumber,
            this.lineHeaderHorUpper,
            this.lineHeaderVerCredit,
            this.lineHeaderVerAmount,
            this.lineHeaderVerNote,
            this.lineHeaderVerBankCode,
            this.lineHeaderVerAccountNumber,
            this.lineHeaderHorLower,
            this.lineHeaderVerRecordedAt,
            this.line2,
            this.line3});
            this.pageHeader.Height = 1.118111F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblBillingCustomerName
            // 
            this.lblBillingCustomerName.Height = 0.2488189F;
            this.lblBillingCustomerName.HyperLink = null;
            this.lblBillingCustomerName.Left = 0F;
            this.lblBillingCustomerName.Name = "lblBillingCustomerName";
            this.lblBillingCustomerName.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingCustomerName.Text = "得意先名";
            this.lblBillingCustomerName.Top = 0.8692914F;
            this.lblBillingCustomerName.Width = 1.353543F;
            // 
            // lblBillingCustomerCode
            // 
            this.lblBillingCustomerCode.Height = 0.2511811F;
            this.lblBillingCustomerCode.HyperLink = null;
            this.lblBillingCustomerCode.Left = 0F;
            this.lblBillingCustomerCode.Name = "lblBillingCustomerCode";
            this.lblBillingCustomerCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingCustomerCode.Text = "得意先コード";
            this.lblBillingCustomerCode.Top = 0.6181103F;
            this.lblBillingCustomerCode.Width = 1.353543F;
            // 
            // lblBillingNote1
            // 
            this.lblBillingNote1.Height = 0.2476377F;
            this.lblBillingNote1.HyperLink = null;
            this.lblBillingNote1.Left = 2.687008F;
            this.lblBillingNote1.Name = "lblBillingNote1";
            this.lblBillingNote1.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingNote1.Text = "備考";
            this.lblBillingNote1.Top = 0.8692914F;
            this.lblBillingNote1.Width = 1.332677F;
            // 
            // lblBillingInvoiceCode
            // 
            this.lblBillingInvoiceCode.Height = 0.2511811F;
            this.lblBillingInvoiceCode.HyperLink = null;
            this.lblBillingInvoiceCode.Left = 2.687008F;
            this.lblBillingInvoiceCode.Name = "lblBillingInvoiceCode";
            this.lblBillingInvoiceCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingInvoiceCode.Text = "請求書番号";
            this.lblBillingInvoiceCode.Top = 0.6181103F;
            this.lblBillingInvoiceCode.Width = 1.332677F;
            // 
            // lblBillingBilledAt
            // 
            this.lblBillingBilledAt.Height = 0.2500001F;
            this.lblBillingBilledAt.HyperLink = null;
            this.lblBillingBilledAt.Left = 4.019685F;
            this.lblBillingBilledAt.Name = "lblBillingBilledAt";
            this.lblBillingBilledAt.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingBilledAt.Text = "請求日";
            this.lblBillingBilledAt.Top = 0.6192914F;
            this.lblBillingBilledAt.Width = 0.7948818F;
            // 
            // lblBillingClosingAt
            // 
            this.lblBillingClosingAt.Height = 0.2570866F;
            this.lblBillingClosingAt.HyperLink = null;
            this.lblBillingClosingAt.Left = 4.814567F;
            this.lblBillingClosingAt.Name = "lblBillingClosingAt";
            this.lblBillingClosingAt.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingClosingAt.Text = "請求締日";
            this.lblBillingClosingAt.Top = 0.6122048F;
            this.lblBillingClosingAt.Width = 0.7740159F;
            // 
            // lblBillingDueAt
            // 
            this.lblBillingDueAt.Height = 0.2476377F;
            this.lblBillingDueAt.HyperLink = null;
            this.lblBillingDueAt.Left = 4.814567F;
            this.lblBillingDueAt.Name = "lblBillingDueAt";
            this.lblBillingDueAt.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingDueAt.Text = "入金予定日";
            this.lblBillingDueAt.Top = 0.8692914F;
            this.lblBillingDueAt.Width = 0.7740159F;
            // 
            // lblBillingSalesAt
            // 
            this.lblBillingSalesAt.Height = 0.2476377F;
            this.lblBillingSalesAt.HyperLink = null;
            this.lblBillingSalesAt.Left = 4.019685F;
            this.lblBillingSalesAt.Name = "lblBillingSalesAt";
            this.lblBillingSalesAt.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingSalesAt.Text = "売上日";
            this.lblBillingSalesAt.Top = 0.8692914F;
            this.lblBillingSalesAt.Width = 0.7948813F;
            // 
            // lblTransferBranchName
            // 
            this.lblTransferBranchName.Height = 0.2488188F;
            this.lblTransferBranchName.HyperLink = null;
            this.lblTransferBranchName.Left = 6.615748F;
            this.lblTransferBranchName.Name = "lblTransferBranchName";
            this.lblTransferBranchName.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblTransferBranchName.Text = "引落支店";
            this.lblTransferBranchName.Top = 0.8692914F;
            this.lblTransferBranchName.Width = 1.186615F;
            // 
            // lblTransferBankName
            // 
            this.lblTransferBankName.Height = 0.25F;
            this.lblTransferBankName.HyperLink = null;
            this.lblTransferBankName.Left = 6.615748F;
            this.lblTransferBankName.Name = "lblTransferBankName";
            this.lblTransferBankName.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblTransferBankName.Text = "引落銀行";
            this.lblTransferBankName.Top = 0.6192914F;
            this.lblTransferBankName.Width = 1.186615F;
            // 
            // lblTransferAccountName
            // 
            this.lblTransferAccountName.Height = 0.2488191F;
            this.lblTransferAccountName.HyperLink = null;
            this.lblTransferAccountName.Left = 7.802363F;
            this.lblTransferAccountName.Name = "lblTransferAccountName";
            this.lblTransferAccountName.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblTransferAccountName.Text = "預金者名";
            this.lblTransferAccountName.Top = 0.8692914F;
            this.lblTransferAccountName.Width = 1.027559F;
            // 
            // lblTransferCustomerCode
            // 
            this.lblTransferCustomerCode.Height = 0.2500001F;
            this.lblTransferCustomerCode.HyperLink = null;
            this.lblTransferCustomerCode.Left = 7.802363F;
            this.lblTransferCustomerCode.Name = "lblTransferCustomerCode";
            this.lblTransferCustomerCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblTransferCustomerCode.Text = "顧客コード";
            this.lblTransferCustomerCode.Top = 0.6192914F;
            this.lblTransferCustomerCode.Width = 1.027559F;
            // 
            // lblTransferResult
            // 
            this.lblTransferResult.Height = 0.4988189F;
            this.lblTransferResult.HyperLink = null;
            this.lblTransferResult.Left = 9.842914F;
            this.lblTransferResult.Name = "lblTransferResult";
            this.lblTransferResult.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.lblTransferResult.Text = "振替結果";
            this.lblTransferResult.Top = 0.6192914F;
            this.lblTransferResult.Width = 0.7870092F;
            // 
            // lblTransferAmount
            // 
            this.lblTransferAmount.Height = 0.496063F;
            this.lblTransferAmount.HyperLink = null;
            this.lblTransferAmount.Left = 8.829922F;
            this.lblTransferAmount.Name = "lblTransferAmount";
            this.lblTransferAmount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.lblTransferAmount.Text = "引落金額";
            this.lblTransferAmount.Top = 0.61063F;
            this.lblTransferAmount.Width = 1.005905F;
            // 
            // lblBillingDepartmentCode
            // 
            this.lblBillingDepartmentCode.Height = 0.2511811F;
            this.lblBillingDepartmentCode.HyperLink = null;
            this.lblBillingDepartmentCode.Left = 1.353543F;
            this.lblBillingDepartmentCode.Name = "lblBillingDepartmentCode";
            this.lblBillingDepartmentCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingDepartmentCode.Text = "請求部門コード";
            this.lblBillingDepartmentCode.Top = 0.6181103F;
            this.lblBillingDepartmentCode.Width = 1.333465F;
            // 
            // lblBillingDepartmentName
            // 
            this.lblBillingDepartmentName.Height = 0.2476377F;
            this.lblBillingDepartmentName.HyperLink = null;
            this.lblBillingDepartmentName.Left = 1.353543F;
            this.lblBillingDepartmentName.Name = "lblBillingDepartmentName";
            this.lblBillingDepartmentName.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle";
            this.lblBillingDepartmentName.Text = "請求部門名";
            this.lblBillingDepartmentName.Top = 0.8692914F;
            this.lblBillingDepartmentName.Width = 1.333465F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.02440945F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCode.Text = "会社コード　：";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 0.7874016F;
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.811811F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCodeName.Text = "label2";
            this.lblCompanyCodeName.Top = 0F;
            this.lblCompanyCodeName.Width = 3.657F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 8.809055F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDate.Text = "出力日付　：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.6984252F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 9.522441F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle; ddo-char-s" +
    "et: 1";
            this.ridate.Top = 0F;
            this.ridate.Width = 1.014961F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "口座振替結果データ一覧";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblBillingAmount
            // 
            this.lblBillingAmount.Height = 0.496063F;
            this.lblBillingAmount.HyperLink = null;
            this.lblBillingAmount.Left = 5.588583F;
            this.lblBillingAmount.Name = "lblBillingAmount";
            this.lblBillingAmount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.lblBillingAmount.Text = "請求金額(税込)";
            this.lblBillingAmount.Top = 0.6122048F;
            this.lblBillingAmount.Width = 1.027165F;
            // 
            // lineHeaderVerSlipNumber
            // 
            this.lineHeaderVerSlipNumber.Height = 0.4976377F;
            this.lineHeaderVerSlipNumber.Left = 2.687008F;
            this.lineHeaderVerSlipNumber.LineWeight = 1F;
            this.lineHeaderVerSlipNumber.Name = "lineHeaderVerSlipNumber";
            this.lineHeaderVerSlipNumber.Top = 0.6141733F;
            this.lineHeaderVerSlipNumber.Width = 0F;
            this.lineHeaderVerSlipNumber.X1 = 2.687008F;
            this.lineHeaderVerSlipNumber.X2 = 2.687008F;
            this.lineHeaderVerSlipNumber.Y1 = 0.6141733F;
            this.lineHeaderVerSlipNumber.Y2 = 1.111811F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 5.960464E-08F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.6141732F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 5.960464E-08F;
            this.lineHeaderHorUpper.X2 = 10.62992F;
            this.lineHeaderHorUpper.Y1 = 0.6141732F;
            this.lineHeaderHorUpper.Y2 = 0.6141732F;
            // 
            // lineHeaderVerCredit
            // 
            this.lineHeaderVerCredit.Height = 0.4976377F;
            this.lineHeaderVerCredit.Left = 5.588583F;
            this.lineHeaderVerCredit.LineWeight = 1F;
            this.lineHeaderVerCredit.Name = "lineHeaderVerCredit";
            this.lineHeaderVerCredit.Top = 0.6141733F;
            this.lineHeaderVerCredit.Width = 0F;
            this.lineHeaderVerCredit.X1 = 5.588583F;
            this.lineHeaderVerCredit.X2 = 5.588583F;
            this.lineHeaderVerCredit.Y1 = 0.6141733F;
            this.lineHeaderVerCredit.Y2 = 1.111811F;
            // 
            // lineHeaderVerAmount
            // 
            this.lineHeaderVerAmount.Height = 0.4976377F;
            this.lineHeaderVerAmount.Left = 4.019685F;
            this.lineHeaderVerAmount.LineWeight = 1F;
            this.lineHeaderVerAmount.Name = "lineHeaderVerAmount";
            this.lineHeaderVerAmount.Top = 0.6141733F;
            this.lineHeaderVerAmount.Width = 0F;
            this.lineHeaderVerAmount.X1 = 4.019685F;
            this.lineHeaderVerAmount.X2 = 4.019685F;
            this.lineHeaderVerAmount.Y1 = 0.6141733F;
            this.lineHeaderVerAmount.Y2 = 1.111811F;
            // 
            // lineHeaderVerNote
            // 
            this.lineHeaderVerNote.Height = 0.4976377F;
            this.lineHeaderVerNote.Left = 6.615748F;
            this.lineHeaderVerNote.LineWeight = 1F;
            this.lineHeaderVerNote.Name = "lineHeaderVerNote";
            this.lineHeaderVerNote.Top = 0.6141733F;
            this.lineHeaderVerNote.Width = 0F;
            this.lineHeaderVerNote.X1 = 6.615748F;
            this.lineHeaderVerNote.X2 = 6.615748F;
            this.lineHeaderVerNote.Y1 = 0.6141733F;
            this.lineHeaderVerNote.Y2 = 1.111811F;
            // 
            // lineHeaderVerBankCode
            // 
            this.lineHeaderVerBankCode.Height = 0.4976377F;
            this.lineHeaderVerBankCode.Left = 7.802363F;
            this.lineHeaderVerBankCode.LineWeight = 1F;
            this.lineHeaderVerBankCode.Name = "lineHeaderVerBankCode";
            this.lineHeaderVerBankCode.Top = 0.6141733F;
            this.lineHeaderVerBankCode.Width = 0F;
            this.lineHeaderVerBankCode.X1 = 7.802363F;
            this.lineHeaderVerBankCode.X2 = 7.802363F;
            this.lineHeaderVerBankCode.Y1 = 0.6141733F;
            this.lineHeaderVerBankCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerAccountNumber
            // 
            this.lineHeaderVerAccountNumber.Height = 0.4976377F;
            this.lineHeaderVerAccountNumber.Left = 9.842914F;
            this.lineHeaderVerAccountNumber.LineWeight = 1F;
            this.lineHeaderVerAccountNumber.Name = "lineHeaderVerAccountNumber";
            this.lineHeaderVerAccountNumber.Top = 0.6141733F;
            this.lineHeaderVerAccountNumber.Width = 0F;
            this.lineHeaderVerAccountNumber.X1 = 9.842914F;
            this.lineHeaderVerAccountNumber.X2 = 9.842914F;
            this.lineHeaderVerAccountNumber.Y1 = 0.6141733F;
            this.lineHeaderVerAccountNumber.Y2 = 1.111811F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.111811F;
            this.lineHeaderHorLower.Width = 10.62992F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.62992F;
            this.lineHeaderHorLower.Y1 = 1.111811F;
            this.lineHeaderHorLower.Y2 = 1.111811F;
            // 
            // lineHeaderVerRecordedAt
            // 
            this.lineHeaderVerRecordedAt.Height = 0.4976377F;
            this.lineHeaderVerRecordedAt.Left = 1.353543F;
            this.lineHeaderVerRecordedAt.LineWeight = 1F;
            this.lineHeaderVerRecordedAt.Name = "lineHeaderVerRecordedAt";
            this.lineHeaderVerRecordedAt.Top = 0.6141733F;
            this.lineHeaderVerRecordedAt.Width = 0F;
            this.lineHeaderVerRecordedAt.X1 = 1.353543F;
            this.lineHeaderVerRecordedAt.X2 = 1.353543F;
            this.lineHeaderVerRecordedAt.Y1 = 0.6141733F;
            this.lineHeaderVerRecordedAt.Y2 = 1.111811F;
            // 
            // line2
            // 
            this.line2.Height = 0.4976377F;
            this.line2.Left = 8.829922F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 0.6141733F;
            this.line2.Width = 0F;
            this.line2.X1 = 8.829922F;
            this.line2.X2 = 8.829922F;
            this.line2.Y1 = 0.6141733F;
            this.line2.Y2 = 1.111811F;
            // 
            // line3
            // 
            this.line3.Height = 0.4976377F;
            this.line3.Left = 4.814567F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0.6141733F;
            this.line3.Width = 0F;
            this.line3.X1 = 4.814567F;
            this.line3.X2 = 4.814567F;
            this.line3.Y1 = 0.6141733F;
            this.line3.Y2 = 1.111811F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtTransferResult,
            this.txtTransferAmount,
            this.txtTransferCustomerCode,
            this.txtTransferAccountName,
            this.txtTransferBranchName,
            this.txtTransferBankName,
            this.txtBillingAmount,
            this.txtBillingDueAt,
            this.txtBillingClosingAt,
            this.txtBillingBilledAt,
            this.txtBillingSalesAt,
            this.txtBillingNote1,
            this.txtBillingInvoiceCode,
            this.txtBillingDepartmentCode,
            this.txtBillingDepartmentName,
            this.txtBillingCustomerName,
            this.txtBillingCustomerCode,
            this.lineDetailVerSlipNumber,
            this.lineDetailVerDebit,
            this.lineDetailVerCredit,
            this.lineDetailVerAmount,
            this.lineDetailVerBankCode,
            this.lineDetailVerCreditDepartment,
            this.lineDetailVerNote,
            this.lineDetailVerDebitDepartment,
            this.line14,
            this.line1});
            this.detail.Height = 0.4429792F;
            this.detail.Name = "detail";
            // 
            // txtTransferResult
            // 
            this.txtTransferResult.DataField = "TransferResult";
            this.txtTransferResult.Height = 0.4598425F;
            this.txtTransferResult.Left = 9.842914F;
            this.txtTransferResult.Name = "txtTransferResult";
            this.txtTransferResult.Style = "font-size: 9pt; text-align: center; text-justify: auto; vertical-align: middle; w" +
    "hite-space: nowrap; ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtTransferResult.Text = null;
            this.txtTransferResult.Top = 0F;
            this.txtTransferResult.Width = 0.7563F;
            // 
            // txtTransferAmount
            // 
            this.txtTransferAmount.DataField = "TransferAmount";
            this.txtTransferAmount.Height = 0.4598425F;
            this.txtTransferAmount.Left = 8.829922F;
            this.txtTransferAmount.Name = "txtTransferAmount";
            this.txtTransferAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtTransferAmount.Style = "font-size: 9pt; text-align: right; text-justify: auto; vertical-align: middle; wh" +
    "ite-space: inherit; ddo-char-set: 128; ddo-wrap-mode: inherit";
            this.txtTransferAmount.Text = "10,987,654,321";
            this.txtTransferAmount.Top = 0F;
            this.txtTransferAmount.Width = 1.005907F;
            // 
            // txtTransferCustomerCode
            // 
            this.txtTransferCustomerCode.DataField = "TransferCustomerCode";
            this.txtTransferCustomerCode.Height = 0.2405512F;
            this.txtTransferCustomerCode.Left = 7.802363F;
            this.txtTransferCustomerCode.Name = "txtTransferCustomerCode";
            this.txtTransferCustomerCode.OutputFormat = resources.GetString("txtTransferCustomerCode.OutputFormat");
            this.txtTransferCustomerCode.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtTransferCustomerCode.Text = null;
            this.txtTransferCustomerCode.Top = 0F;
            this.txtTransferCustomerCode.Width = 1.027559F;
            // 
            // txtTransferAccountName
            // 
            this.txtTransferAccountName.DataField = "TransferAccountName";
            this.txtTransferAccountName.Height = 0.2192914F;
            this.txtTransferAccountName.Left = 7.802363F;
            this.txtTransferAccountName.Name = "txtTransferAccountName";
            this.txtTransferAccountName.OutputFormat = resources.GetString("txtTransferAccountName.OutputFormat");
            this.txtTransferAccountName.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtTransferAccountName.Text = null;
            this.txtTransferAccountName.Top = 0.2405512F;
            this.txtTransferAccountName.Width = 1.027559F;
            // 
            // txtTransferBranchName
            // 
            this.txtTransferBranchName.DataField = "TransferBranchName";
            this.txtTransferBranchName.Height = 0.2181103F;
            this.txtTransferBranchName.Left = 6.615748F;
            this.txtTransferBranchName.Name = "txtTransferBranchName";
            this.txtTransferBranchName.OutputFormat = resources.GetString("txtTransferBranchName.OutputFormat");
            this.txtTransferBranchName.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtTransferBranchName.Text = null;
            this.txtTransferBranchName.Top = 0.2405512F;
            this.txtTransferBranchName.Width = 1.186615F;
            // 
            // txtTransferBankName
            // 
            this.txtTransferBankName.DataField = "TransferBankName";
            this.txtTransferBankName.Height = 0.2405512F;
            this.txtTransferBankName.Left = 6.615748F;
            this.txtTransferBankName.Name = "txtTransferBankName";
            this.txtTransferBankName.OutputFormat = resources.GetString("txtTransferBankName.OutputFormat");
            this.txtTransferBankName.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtTransferBankName.Text = null;
            this.txtTransferBankName.Top = 0F;
            this.txtTransferBankName.Width = 1.186615F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.DataField = "BillingAmount";
            this.txtBillingAmount.Height = 0.4598425F;
            this.txtBillingAmount.Left = 5.588583F;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtBillingAmount.Style = "font-size: 9pt; text-align: right; text-justify: auto; vertical-align: middle; wh" +
    "ite-space: inherit; ddo-char-set: 128; ddo-wrap-mode: inherit";
            this.txtBillingAmount.Text = "10,987,654,321";
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 1.027165F;
            // 
            // txtBillingDueAt
            // 
            this.txtBillingDueAt.DataField = "BillingDueAt";
            this.txtBillingDueAt.Height = 0.2192914F;
            this.txtBillingDueAt.Left = 4.814567F;
            this.txtBillingDueAt.Name = "txtBillingDueAt";
            this.txtBillingDueAt.OutputFormat = resources.GetString("txtBillingDueAt.OutputFormat");
            this.txtBillingDueAt.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingDueAt.Text = null;
            this.txtBillingDueAt.Top = 0.2405512F;
            this.txtBillingDueAt.Width = 0.7740159F;
            // 
            // txtBillingClosingAt
            // 
            this.txtBillingClosingAt.DataField = "BillingClosingAt";
            this.txtBillingClosingAt.Height = 0.2405512F;
            this.txtBillingClosingAt.Left = 4.814567F;
            this.txtBillingClosingAt.Name = "txtBillingClosingAt";
            this.txtBillingClosingAt.OutputFormat = resources.GetString("txtBillingClosingAt.OutputFormat");
            this.txtBillingClosingAt.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingClosingAt.Text = null;
            this.txtBillingClosingAt.Top = 0F;
            this.txtBillingClosingAt.Width = 0.7740159F;
            // 
            // txtBillingBilledAt
            // 
            this.txtBillingBilledAt.DataField = "BillingBilledAt";
            this.txtBillingBilledAt.Height = 0.2405512F;
            this.txtBillingBilledAt.Left = 4.019685F;
            this.txtBillingBilledAt.Name = "txtBillingBilledAt";
            this.txtBillingBilledAt.OutputFormat = resources.GetString("txtBillingBilledAt.OutputFormat");
            this.txtBillingBilledAt.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingBilledAt.Text = null;
            this.txtBillingBilledAt.Top = 0F;
            this.txtBillingBilledAt.Width = 0.7948823F;
            // 
            // txtBillingSalesAt
            // 
            this.txtBillingSalesAt.DataField = "BillingSalesAt";
            this.txtBillingSalesAt.Height = 0.2192913F;
            this.txtBillingSalesAt.Left = 4.019685F;
            this.txtBillingSalesAt.Name = "txtBillingSalesAt";
            this.txtBillingSalesAt.OutputFormat = resources.GetString("txtBillingSalesAt.OutputFormat");
            this.txtBillingSalesAt.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingSalesAt.Text = null;
            this.txtBillingSalesAt.Top = 0.2405512F;
            this.txtBillingSalesAt.Width = 0.7948818F;
            // 
            // txtBillingNote1
            // 
            this.txtBillingNote1.DataField = "BillingNote1";
            this.txtBillingNote1.Height = 0.2192914F;
            this.txtBillingNote1.Left = 2.687008F;
            this.txtBillingNote1.Name = "txtBillingNote1";
            this.txtBillingNote1.OutputFormat = resources.GetString("txtBillingNote1.OutputFormat");
            this.txtBillingNote1.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingNote1.Text = null;
            this.txtBillingNote1.Top = 0.2405512F;
            this.txtBillingNote1.Width = 1.332677F;
            // 
            // txtBillingInvoiceCode
            // 
            this.txtBillingInvoiceCode.DataField = "BillingInvoiceCode";
            this.txtBillingInvoiceCode.Height = 0.2405512F;
            this.txtBillingInvoiceCode.Left = 2.687008F;
            this.txtBillingInvoiceCode.Name = "txtBillingInvoiceCode";
            this.txtBillingInvoiceCode.OutputFormat = resources.GetString("txtBillingInvoiceCode.OutputFormat");
            this.txtBillingInvoiceCode.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingInvoiceCode.Text = null;
            this.txtBillingInvoiceCode.Top = 0F;
            this.txtBillingInvoiceCode.Width = 1.332677F;
            // 
            // txtBillingDepartmentCode
            // 
            this.txtBillingDepartmentCode.DataField = "BillingDepartmentCode";
            this.txtBillingDepartmentCode.Height = 0.2405512F;
            this.txtBillingDepartmentCode.Left = 1.353543F;
            this.txtBillingDepartmentCode.Name = "txtBillingDepartmentCode";
            this.txtBillingDepartmentCode.OutputFormat = resources.GetString("txtBillingDepartmentCode.OutputFormat");
            this.txtBillingDepartmentCode.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingDepartmentCode.Text = null;
            this.txtBillingDepartmentCode.Top = 0F;
            this.txtBillingDepartmentCode.Width = 1.333465F;
            // 
            // txtBillingDepartmentName
            // 
            this.txtBillingDepartmentName.DataField = "BillingDepartmentName";
            this.txtBillingDepartmentName.Height = 0.2192914F;
            this.txtBillingDepartmentName.Left = 1.353543F;
            this.txtBillingDepartmentName.Name = "txtBillingDepartmentName";
            this.txtBillingDepartmentName.OutputFormat = resources.GetString("txtBillingDepartmentName.OutputFormat");
            this.txtBillingDepartmentName.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingDepartmentName.Text = null;
            this.txtBillingDepartmentName.Top = 0.2405512F;
            this.txtBillingDepartmentName.Width = 1.333465F;
            // 
            // txtBillingCustomerName
            // 
            this.txtBillingCustomerName.DataField = "BillingCustomerName";
            this.txtBillingCustomerName.Height = 0.2192913F;
            this.txtBillingCustomerName.Left = 0F;
            this.txtBillingCustomerName.Name = "txtBillingCustomerName";
            this.txtBillingCustomerName.OutputFormat = resources.GetString("txtBillingCustomerName.OutputFormat");
            this.txtBillingCustomerName.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingCustomerName.Text = null;
            this.txtBillingCustomerName.Top = 0.2405512F;
            this.txtBillingCustomerName.Width = 1.353543F;
            // 
            // txtBillingCustomerCode
            // 
            this.txtBillingCustomerCode.DataField = "BillingCustomerCode";
            this.txtBillingCustomerCode.Height = 0.2405512F;
            this.txtBillingCustomerCode.Left = 0.02440945F;
            this.txtBillingCustomerCode.Name = "txtBillingCustomerCode";
            this.txtBillingCustomerCode.OutputFormat = resources.GetString("txtBillingCustomerCode.OutputFormat");
            this.txtBillingCustomerCode.Style = "font-size: 9pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 128; ddo-wrap-mode: nowrap";
            this.txtBillingCustomerCode.Text = null;
            this.txtBillingCustomerCode.Top = 0F;
            this.txtBillingCustomerCode.Width = 1.329134F;
            // 
            // lineDetailVerSlipNumber
            // 
            this.lineDetailVerSlipNumber.Height = 0.4598425F;
            this.lineDetailVerSlipNumber.Left = 1.353543F;
            this.lineDetailVerSlipNumber.LineWeight = 1F;
            this.lineDetailVerSlipNumber.Name = "lineDetailVerSlipNumber";
            this.lineDetailVerSlipNumber.Top = 0F;
            this.lineDetailVerSlipNumber.Width = 0F;
            this.lineDetailVerSlipNumber.X1 = 1.353543F;
            this.lineDetailVerSlipNumber.X2 = 1.353543F;
            this.lineDetailVerSlipNumber.Y1 = 0F;
            this.lineDetailVerSlipNumber.Y2 = 0.4598425F;
            // 
            // lineDetailVerDebit
            // 
            this.lineDetailVerDebit.Height = 0.4598425F;
            this.lineDetailVerDebit.Left = 4.019685F;
            this.lineDetailVerDebit.LineWeight = 1F;
            this.lineDetailVerDebit.Name = "lineDetailVerDebit";
            this.lineDetailVerDebit.Top = 0F;
            this.lineDetailVerDebit.Width = 0F;
            this.lineDetailVerDebit.X1 = 4.019685F;
            this.lineDetailVerDebit.X2 = 4.019685F;
            this.lineDetailVerDebit.Y1 = 0F;
            this.lineDetailVerDebit.Y2 = 0.4598425F;
            // 
            // lineDetailVerCredit
            // 
            this.lineDetailVerCredit.Height = 0.4598425F;
            this.lineDetailVerCredit.Left = 5.588583F;
            this.lineDetailVerCredit.LineWeight = 1F;
            this.lineDetailVerCredit.Name = "lineDetailVerCredit";
            this.lineDetailVerCredit.Top = 0F;
            this.lineDetailVerCredit.Width = 0F;
            this.lineDetailVerCredit.X1 = 5.588583F;
            this.lineDetailVerCredit.X2 = 5.588583F;
            this.lineDetailVerCredit.Y1 = 0F;
            this.lineDetailVerCredit.Y2 = 0.4598425F;
            // 
            // lineDetailVerAmount
            // 
            this.lineDetailVerAmount.Height = 0.4598425F;
            this.lineDetailVerAmount.Left = 6.615748F;
            this.lineDetailVerAmount.LineWeight = 1F;
            this.lineDetailVerAmount.Name = "lineDetailVerAmount";
            this.lineDetailVerAmount.Top = 0F;
            this.lineDetailVerAmount.Width = 0F;
            this.lineDetailVerAmount.X1 = 6.615748F;
            this.lineDetailVerAmount.X2 = 6.615748F;
            this.lineDetailVerAmount.Y1 = 0F;
            this.lineDetailVerAmount.Y2 = 0.4598425F;
            // 
            // lineDetailVerBankCode
            // 
            this.lineDetailVerBankCode.Height = 0.4598425F;
            this.lineDetailVerBankCode.Left = 8.829922F;
            this.lineDetailVerBankCode.LineWeight = 1F;
            this.lineDetailVerBankCode.Name = "lineDetailVerBankCode";
            this.lineDetailVerBankCode.Top = 0F;
            this.lineDetailVerBankCode.Width = 0F;
            this.lineDetailVerBankCode.X1 = 8.829922F;
            this.lineDetailVerBankCode.X2 = 8.829922F;
            this.lineDetailVerBankCode.Y1 = 0F;
            this.lineDetailVerBankCode.Y2 = 0.4598425F;
            // 
            // lineDetailVerCreditDepartment
            // 
            this.lineDetailVerCreditDepartment.Height = 0.4598425F;
            this.lineDetailVerCreditDepartment.Left = 4.814567F;
            this.lineDetailVerCreditDepartment.LineWeight = 1F;
            this.lineDetailVerCreditDepartment.Name = "lineDetailVerCreditDepartment";
            this.lineDetailVerCreditDepartment.Top = 0F;
            this.lineDetailVerCreditDepartment.Width = 0F;
            this.lineDetailVerCreditDepartment.X1 = 4.814567F;
            this.lineDetailVerCreditDepartment.X2 = 4.814567F;
            this.lineDetailVerCreditDepartment.Y1 = 0F;
            this.lineDetailVerCreditDepartment.Y2 = 0.4598425F;
            // 
            // lineDetailVerNote
            // 
            this.lineDetailVerNote.Height = 0.4598425F;
            this.lineDetailVerNote.Left = 7.802363F;
            this.lineDetailVerNote.LineWeight = 1F;
            this.lineDetailVerNote.Name = "lineDetailVerNote";
            this.lineDetailVerNote.Top = 0F;
            this.lineDetailVerNote.Width = 0F;
            this.lineDetailVerNote.X1 = 7.802363F;
            this.lineDetailVerNote.X2 = 7.802363F;
            this.lineDetailVerNote.Y1 = 0F;
            this.lineDetailVerNote.Y2 = 0.4598425F;
            // 
            // lineDetailVerDebitDepartment
            // 
            this.lineDetailVerDebitDepartment.Height = 0.4598425F;
            this.lineDetailVerDebitDepartment.Left = 2.687008F;
            this.lineDetailVerDebitDepartment.LineWeight = 1F;
            this.lineDetailVerDebitDepartment.Name = "lineDetailVerDebitDepartment";
            this.lineDetailVerDebitDepartment.Top = 0F;
            this.lineDetailVerDebitDepartment.Width = 0F;
            this.lineDetailVerDebitDepartment.X1 = 2.687008F;
            this.lineDetailVerDebitDepartment.X2 = 2.687008F;
            this.lineDetailVerDebitDepartment.Y1 = 0F;
            this.lineDetailVerDebitDepartment.Y2 = 0.4598425F;
            // 
            // line14
            // 
            this.line14.Height = 0F;
            this.line14.Left = 0F;
            this.line14.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line14.LineWeight = 1F;
            this.line14.Name = "line14";
            this.line14.Top = 0.4598425F;
            this.line14.Width = 10.62992F;
            this.line14.X1 = 0F;
            this.line14.X2 = 10.62992F;
            this.line14.Y1 = 0.4598425F;
            this.line14.Y2 = 0.4598425F;
            // 
            // line1
            // 
            this.line1.Height = 0.4598425F;
            this.line1.Left = 9.842914F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0F;
            this.line1.Width = 0F;
            this.line1.X1 = 9.842914F;
            this.line1.X2 = 9.842914F;
            this.line1.Y1 = 0F;
            this.line1.Y2 = 0.4598425F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.reportInfo1});
            this.pageFooter.Height = 0.3149606F;
            this.pageFooter.Name = "pageFooter";
            // 
            // reportInfo1
            // 
            this.reportInfo1.FormatString = "{PageNumber} / {PageCount}";
            this.reportInfo1.Height = 0.2F;
            this.reportInfo1.Left = 0F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.05748032F;
            this.reportInfo1.Width = 10.62992F;
            // 
            // groupHeader1
            // 
            this.groupHeader1.Height = 0F;
            this.groupHeader1.Name = "groupHeader1";
            // 
            // groupFooter1
            // 
            this.groupFooter1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblTotal,
            this.txtTotalBillingAmount,
            this.lblSpace,
            this.txtTotalTransferAmount,
            this.txtTotalCount,
            this.lineFooterVerTotal,
            this.lineFooterHorLower,
            this.lineFooterVerTotalAmount,
            this.line4,
            this.line5,
            this.line6});
            this.groupFooter1.Height = 0.4191492F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // lblTotal
            // 
            this.lblTotal.Height = 0.4043307F;
            this.lblTotal.HyperLink = null;
            this.lblTotal.Left = 0F;
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblTotal.Style = "background-color: WhiteSmoke; font-size: 9pt; vertical-align: middle";
            this.lblTotal.Text = " 合計";
            this.lblTotal.Top = 0F;
            this.lblTotal.Width = 5.588583F;
            // 
            // txtTotalBillingAmount
            // 
            this.txtTotalBillingAmount.DataField = "BillingAmount";
            this.txtTotalBillingAmount.Height = 0.4043307F;
            this.txtTotalBillingAmount.Left = 5.588583F;
            this.txtTotalBillingAmount.Name = "txtTotalBillingAmount";
            this.txtTotalBillingAmount.OutputFormat = resources.GetString("txtTotalBillingAmount.OutputFormat");
            this.txtTotalBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtTotalBillingAmount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 128";
            this.txtTotalBillingAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalBillingAmount.Text = "10,987,654,321";
            this.txtTotalBillingAmount.Top = 0F;
            this.txtTotalBillingAmount.Width = 1.027165F;
            // 
            // lblSpace
            // 
            this.lblSpace.Height = 0.4043307F;
            this.lblSpace.HyperLink = null;
            this.lblSpace.Left = 6.615748F;
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Style = "background-color: WhiteSmoke; font-size: 8pt";
            this.lblSpace.Text = "";
            this.lblSpace.Top = 0F;
            this.lblSpace.Width = 2.214173F;
            // 
            // txtTotalTransferAmount
            // 
            this.txtTotalTransferAmount.DataField = "TransferAmount";
            this.txtTotalTransferAmount.Height = 0.4043307F;
            this.txtTotalTransferAmount.Left = 8.829922F;
            this.txtTotalTransferAmount.Name = "txtTotalTransferAmount";
            this.txtTotalTransferAmount.OutputFormat = resources.GetString("txtTotalTransferAmount.OutputFormat");
            this.txtTotalTransferAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtTotalTransferAmount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 128";
            this.txtTotalTransferAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalTransferAmount.Text = "10,987,654,321";
            this.txtTotalTransferAmount.Top = 0F;
            this.txtTotalTransferAmount.Width = 1.005906F;
            // 
            // txtTotalCount
            // 
            this.txtTotalCount.Height = 0.4043307F;
            this.txtTotalCount.Left = 9.842914F;
            this.txtTotalCount.Name = "txtTotalCount";
            this.txtTotalCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtTotalCount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 128";
            this.txtTotalCount.Text = null;
            this.txtTotalCount.Top = 0F;
            this.txtTotalCount.Width = 0.8149595F;
            // 
            // lineFooterVerTotal
            // 
            this.lineFooterVerTotal.Height = 0.4122047F;
            this.lineFooterVerTotal.Left = 5.588583F;
            this.lineFooterVerTotal.LineWeight = 1F;
            this.lineFooterVerTotal.Name = "lineFooterVerTotal";
            this.lineFooterVerTotal.Top = 0F;
            this.lineFooterVerTotal.Width = 0F;
            this.lineFooterVerTotal.X1 = 5.588583F;
            this.lineFooterVerTotal.X2 = 5.588583F;
            this.lineFooterVerTotal.Y1 = 0F;
            this.lineFooterVerTotal.Y2 = 0.4122047F;
            // 
            // lineFooterHorLower
            // 
            this.lineFooterHorLower.Height = 0F;
            this.lineFooterHorLower.Left = 0F;
            this.lineFooterHorLower.LineWeight = 1F;
            this.lineFooterHorLower.Name = "lineFooterHorLower";
            this.lineFooterHorLower.Top = 0.4122047F;
            this.lineFooterHorLower.Width = 10.62992F;
            this.lineFooterHorLower.X1 = 0F;
            this.lineFooterHorLower.X2 = 10.62992F;
            this.lineFooterHorLower.Y1 = 0.4122047F;
            this.lineFooterHorLower.Y2 = 0.4122047F;
            // 
            // lineFooterVerTotalAmount
            // 
            this.lineFooterVerTotalAmount.Height = 0.4122047F;
            this.lineFooterVerTotalAmount.Left = 6.615748F;
            this.lineFooterVerTotalAmount.LineWeight = 1F;
            this.lineFooterVerTotalAmount.Name = "lineFooterVerTotalAmount";
            this.lineFooterVerTotalAmount.Top = 0F;
            this.lineFooterVerTotalAmount.Width = 0F;
            this.lineFooterVerTotalAmount.X1 = 6.615748F;
            this.lineFooterVerTotalAmount.X2 = 6.615748F;
            this.lineFooterVerTotalAmount.Y1 = 0F;
            this.lineFooterVerTotalAmount.Y2 = 0.4122047F;
            // 
            // line4
            // 
            this.line4.Height = 0.4122047F;
            this.line4.Left = 8.829922F;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.Top = 0F;
            this.line4.Width = 0F;
            this.line4.X1 = 8.829922F;
            this.line4.X2 = 8.829922F;
            this.line4.Y1 = 0F;
            this.line4.Y2 = 0.4122047F;
            // 
            // line5
            // 
            this.line5.Height = 0.4122047F;
            this.line5.Left = 9.842914F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0F;
            this.line5.Width = 0F;
            this.line5.X1 = 9.842914F;
            this.line5.X2 = 9.842914F;
            this.line5.Y1 = 0F;
            this.line5.Y2 = 0.4122047F;
            // 
            // line6
            // 
            this.line6.Height = 0F;
            this.line6.Left = 0F;
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.Top = 0F;
            this.line6.Width = 10.62992F;
            this.line6.X1 = 0F;
            this.line6.X2 = 10.62992F;
            this.line6.Y1 = 0F;
            this.line6.Y2 = 0F;
            // 
            // AccountTransferImportReport
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
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferAccountName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTransferAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferAccountName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalTransferAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line line14;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalCount;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTransferBankName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTransferBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTransferCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTransferAccountName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTransferAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingNote1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTransferAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTransferBankName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTransferBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTransferCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTransferAccountName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalTransferAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.Line line4;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        private GrapeCity.ActiveReports.SectionReportModel.Line line6;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblTransferResult;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtTransferResult;
    }
}
