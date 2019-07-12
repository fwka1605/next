namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingJournalizingSectionReport の概要の説明です。
    /// </summary>
    partial class BillingJournalizingSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BillingJournalizingSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompany = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebit = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebitAccTitleCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebitSubCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCredit = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditAccTitleCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditSupplementary = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCredit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorDebitCredit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebitAccTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditAccTitleCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitAccTitleCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitAccTitleName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitSubCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitSubName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditAccTitleName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditSubCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditSubName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitAccTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitSubCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditAccCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCredit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSum = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblFooterSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerSpace = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerSum = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSlipNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitAccTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAccTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditSupplementary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlipNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccTitleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccTitleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFooterSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblTitle,
            this.lblCompany,
            this.lblCompanyCode,
            this.lblSlipNumber,
            this.lblBilledAt,
            this.lblDebit,
            this.lblDebitDepartment,
            this.lblDebitAccTitleCode,
            this.lblDebitSubCode,
            this.lblCredit,
            this.lblCreditDepartment,
            this.lblCreditAccTitleCode,
            this.lblCreditSupplementary,
            this.lblAmount,
            this.lblNote,
            this.lblCustomerCode,
            this.lblInvoiceCode,
            this.lblCurrencyCode,
            this.lineHeaderHorUpper,
            this.lineHeaderVerBilledAt,
            this.lineHeaderVerSlipNumber,
            this.lineHeaderVerDebit,
            this.lineHeaderVerCredit,
            this.lineHeaderVerAmount,
            this.lineHeaderVerNote,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderHorInvoiceCode,
            this.lineHeaderHorDebitCredit,
            this.lineHeaderVerDebitDepartment,
            this.lineHeaderVerDebitAccTitle,
            this.lineHeaderVerCreditDepartment,
            this.lineHeaderVerCreditAccTitleCode,
            this.lbldate,
            this.ridate,
            this.lineHeaderHorLower});
            this.pageHeader.Height = 1.014589F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline";
            this.lblTitle.Text = "請求仕訳出力";
            this.lblTitle.Top = 0.2704724F;
            this.lblTitle.Width = 10.6F;
            // 
            // lblCompany
            // 
            this.lblCompany.Height = 0.2F;
            this.lblCompany.HyperLink = null;
            this.lblCompany.Left = 0.02440945F;
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompany.Text = "会社コード  :";
            this.lblCompany.Top = 0F;
            this.lblCompany.Width = 0.7874016F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.811811F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCode.Text = "label1";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 3.657087F;
            // 
            // lblSlipNumber
            // 
            this.lblSlipNumber.Height = 0.4F;
            this.lblSlipNumber.HyperLink = null;
            this.lblSlipNumber.Left = 0.55F;
            this.lblSlipNumber.Name = "lblSlipNumber";
            this.lblSlipNumber.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblSlipNumber.Text = "伝票番号";
            this.lblSlipNumber.Top = 0.6F;
            this.lblSlipNumber.Width = 0.55F;
            // 
            // lblBilledAt
            // 
            this.lblBilledAt.Height = 0.4F;
            this.lblBilledAt.HyperLink = null;
            this.lblBilledAt.Left = 0F;
            this.lblBilledAt.Name = "lblBilledAt";
            this.lblBilledAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBilledAt.Text = "伝票日付";
            this.lblBilledAt.Top = 0.6F;
            this.lblBilledAt.Width = 0.55F;
            // 
            // lblDebit
            // 
            this.lblDebit.Height = 0.2F;
            this.lblDebit.HyperLink = null;
            this.lblDebit.Left = 1.1F;
            this.lblDebit.Name = "lblDebit";
            this.lblDebit.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebit.Text = "借方";
            this.lblDebit.Top = 0.6F;
            this.lblDebit.Width = 2.5F;
            // 
            // lblDebitDepartment
            // 
            this.lblDebitDepartment.Height = 0.2F;
            this.lblDebitDepartment.HyperLink = null;
            this.lblDebitDepartment.Left = 1.1F;
            this.lblDebitDepartment.Name = "lblDebitDepartment";
            this.lblDebitDepartment.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebitDepartment.Text = "部門コード";
            this.lblDebitDepartment.Top = 0.8F;
            this.lblDebitDepartment.Width = 0.83F;
            // 
            // lblDebitAccTitleCode
            // 
            this.lblDebitAccTitleCode.Height = 0.2F;
            this.lblDebitAccTitleCode.HyperLink = null;
            this.lblDebitAccTitleCode.Left = 1.94F;
            this.lblDebitAccTitleCode.Name = "lblDebitAccTitleCode";
            this.lblDebitAccTitleCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebitAccTitleCode.Text = "科目コード";
            this.lblDebitAccTitleCode.Top = 0.8F;
            this.lblDebitAccTitleCode.Width = 0.83F;
            // 
            // lblDebitSubCode
            // 
            this.lblDebitSubCode.Height = 0.2F;
            this.lblDebitSubCode.HyperLink = null;
            this.lblDebitSubCode.Left = 2.77F;
            this.lblDebitSubCode.Name = "lblDebitSubCode";
            this.lblDebitSubCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebitSubCode.Text = "補助コード";
            this.lblDebitSubCode.Top = 0.8F;
            this.lblDebitSubCode.Width = 0.83F;
            // 
            // lblCredit
            // 
            this.lblCredit.Height = 0.2F;
            this.lblCredit.HyperLink = null;
            this.lblCredit.Left = 3.6F;
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCredit.Text = "貸方";
            this.lblCredit.Top = 0.6F;
            this.lblCredit.Width = 2.5F;
            // 
            // lblCreditDepartment
            // 
            this.lblCreditDepartment.Height = 0.2F;
            this.lblCreditDepartment.HyperLink = null;
            this.lblCreditDepartment.Left = 3.6F;
            this.lblCreditDepartment.Name = "lblCreditDepartment";
            this.lblCreditDepartment.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCreditDepartment.Text = "部門コード";
            this.lblCreditDepartment.Top = 0.8F;
            this.lblCreditDepartment.Width = 0.83F;
            // 
            // lblCreditAccTitleCode
            // 
            this.lblCreditAccTitleCode.Height = 0.2F;
            this.lblCreditAccTitleCode.HyperLink = null;
            this.lblCreditAccTitleCode.Left = 4.43F;
            this.lblCreditAccTitleCode.Name = "lblCreditAccTitleCode";
            this.lblCreditAccTitleCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCreditAccTitleCode.Text = "科目コード";
            this.lblCreditAccTitleCode.Top = 0.8F;
            this.lblCreditAccTitleCode.Width = 0.83F;
            // 
            // lblCreditSupplementary
            // 
            this.lblCreditSupplementary.Height = 0.2F;
            this.lblCreditSupplementary.HyperLink = null;
            this.lblCreditSupplementary.Left = 5.269F;
            this.lblCreditSupplementary.Name = "lblCreditSupplementary";
            this.lblCreditSupplementary.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCreditSupplementary.Text = "補助コード";
            this.lblCreditSupplementary.Top = 0.8F;
            this.lblCreditSupplementary.Width = 0.83F;
            // 
            // lblAmount
            // 
            this.lblAmount.Height = 0.4F;
            this.lblAmount.HyperLink = null;
            this.lblAmount.Left = 6.1F;
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAmount.Text = "仕訳金額";
            this.lblAmount.Top = 0.6F;
            this.lblAmount.Width = 1.1F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.4F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 7.2F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblNote.Text = "備考";
            this.lblNote.Top = 0.6F;
            this.lblNote.Width = 1.5F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.4F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 8.700001F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.6F;
            this.lblCustomerCode.Width = 1.1F;
            // 
            // lblInvoiceCode
            // 
            this.lblInvoiceCode.Height = 0.2F;
            this.lblInvoiceCode.HyperLink = null;
            this.lblInvoiceCode.Left = 9.8F;
            this.lblInvoiceCode.Name = "lblInvoiceCode";
            this.lblInvoiceCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblInvoiceCode.Text = "請求書番号";
            this.lblInvoiceCode.Top = 0.6F;
            this.lblInvoiceCode.Width = 0.8F;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Height = 0.2F;
            this.lblCurrencyCode.HyperLink = null;
            this.lblCurrencyCode.Left = 9.8F;
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.Top = 0.8F;
            this.lblCurrencyCode.Width = 0.8F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.6F;
            this.lineHeaderHorUpper.Width = 10.6F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.6F;
            this.lineHeaderHorUpper.Y1 = 0.6F;
            this.lineHeaderHorUpper.Y2 = 0.6F;
            // 
            // lineHeaderVerBilledAt
            // 
            this.lineHeaderVerBilledAt.Height = 0.4F;
            this.lineHeaderVerBilledAt.Left = 0.55F;
            this.lineHeaderVerBilledAt.LineWeight = 1F;
            this.lineHeaderVerBilledAt.Name = "lineHeaderVerBilledAt";
            this.lineHeaderVerBilledAt.Top = 0.6F;
            this.lineHeaderVerBilledAt.Width = 9.453297E-05F;
            this.lineHeaderVerBilledAt.X1 = 0.55F;
            this.lineHeaderVerBilledAt.X2 = 0.5500945F;
            this.lineHeaderVerBilledAt.Y1 = 0.6F;
            this.lineHeaderVerBilledAt.Y2 = 1F;
            // 
            // lineHeaderVerSlipNumber
            // 
            this.lineHeaderVerSlipNumber.Height = 0.4F;
            this.lineHeaderVerSlipNumber.Left = 1.1F;
            this.lineHeaderVerSlipNumber.LineWeight = 1F;
            this.lineHeaderVerSlipNumber.Name = "lineHeaderVerSlipNumber";
            this.lineHeaderVerSlipNumber.Top = 0.6F;
            this.lineHeaderVerSlipNumber.Width = 3.898144E-05F;
            this.lineHeaderVerSlipNumber.X1 = 1.1F;
            this.lineHeaderVerSlipNumber.X2 = 1.100039F;
            this.lineHeaderVerSlipNumber.Y1 = 0.6F;
            this.lineHeaderVerSlipNumber.Y2 = 1F;
            // 
            // lineHeaderVerDebit
            // 
            this.lineHeaderVerDebit.Height = 0.4F;
            this.lineHeaderVerDebit.Left = 3.599874F;
            this.lineHeaderVerDebit.LineWeight = 1F;
            this.lineHeaderVerDebit.Name = "lineHeaderVerDebit";
            this.lineHeaderVerDebit.Top = 0.6F;
            this.lineHeaderVerDebit.Width = 0.0001261234F;
            this.lineHeaderVerDebit.X1 = 3.6F;
            this.lineHeaderVerDebit.X2 = 3.599874F;
            this.lineHeaderVerDebit.Y1 = 0.6F;
            this.lineHeaderVerDebit.Y2 = 1F;
            // 
            // lineHeaderVerCredit
            // 
            this.lineHeaderVerCredit.Height = 0.4F;
            this.lineHeaderVerCredit.Left = 6.1F;
            this.lineHeaderVerCredit.LineWeight = 1F;
            this.lineHeaderVerCredit.Name = "lineHeaderVerCredit";
            this.lineHeaderVerCredit.Top = 0.6F;
            this.lineHeaderVerCredit.Width = 0F;
            this.lineHeaderVerCredit.X1 = 6.1F;
            this.lineHeaderVerCredit.X2 = 6.1F;
            this.lineHeaderVerCredit.Y1 = 0.6F;
            this.lineHeaderVerCredit.Y2 = 1F;
            // 
            // lineHeaderVerAmount
            // 
            this.lineHeaderVerAmount.Height = 0.4F;
            this.lineHeaderVerAmount.Left = 7.2F;
            this.lineHeaderVerAmount.LineWeight = 1F;
            this.lineHeaderVerAmount.Name = "lineHeaderVerAmount";
            this.lineHeaderVerAmount.Top = 0.6F;
            this.lineHeaderVerAmount.Width = 0F;
            this.lineHeaderVerAmount.X1 = 7.2F;
            this.lineHeaderVerAmount.X2 = 7.2F;
            this.lineHeaderVerAmount.Y1 = 0.6F;
            this.lineHeaderVerAmount.Y2 = 1F;
            // 
            // lineHeaderVerNote
            // 
            this.lineHeaderVerNote.Height = 0.4F;
            this.lineHeaderVerNote.Left = 8.700001F;
            this.lineHeaderVerNote.LineWeight = 1F;
            this.lineHeaderVerNote.Name = "lineHeaderVerNote";
            this.lineHeaderVerNote.Top = 0.6F;
            this.lineHeaderVerNote.Width = 0F;
            this.lineHeaderVerNote.X1 = 8.700001F;
            this.lineHeaderVerNote.X2 = 8.700001F;
            this.lineHeaderVerNote.Y1 = 0.6F;
            this.lineHeaderVerNote.Y2 = 1F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.4F;
            this.lineHeaderVerCustomerCode.Left = 9.8F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.6F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 9.8F;
            this.lineHeaderVerCustomerCode.X2 = 9.8F;
            this.lineHeaderVerCustomerCode.Y1 = 0.6F;
            this.lineHeaderVerCustomerCode.Y2 = 1F;
            // 
            // lineHeaderHorInvoiceCode
            // 
            this.lineHeaderHorInvoiceCode.Height = 0F;
            this.lineHeaderHorInvoiceCode.Left = 9.8F;
            this.lineHeaderHorInvoiceCode.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorInvoiceCode.LineWeight = 1F;
            this.lineHeaderHorInvoiceCode.Name = "lineHeaderHorInvoiceCode";
            this.lineHeaderHorInvoiceCode.Top = 0.8F;
            this.lineHeaderHorInvoiceCode.Width = 0.8000059F;
            this.lineHeaderHorInvoiceCode.X1 = 9.8F;
            this.lineHeaderHorInvoiceCode.X2 = 10.60001F;
            this.lineHeaderHorInvoiceCode.Y1 = 0.8F;
            this.lineHeaderHorInvoiceCode.Y2 = 0.8F;
            // 
            // lineHeaderHorDebitCredit
            // 
            this.lineHeaderHorDebitCredit.Height = 0F;
            this.lineHeaderHorDebitCredit.Left = 1.1F;
            this.lineHeaderHorDebitCredit.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorDebitCredit.LineWeight = 1F;
            this.lineHeaderHorDebitCredit.Name = "lineHeaderHorDebitCredit";
            this.lineHeaderHorDebitCredit.Top = 0.8F;
            this.lineHeaderHorDebitCredit.Width = 5F;
            this.lineHeaderHorDebitCredit.X1 = 1.1F;
            this.lineHeaderHorDebitCredit.X2 = 6.1F;
            this.lineHeaderHorDebitCredit.Y1 = 0.8F;
            this.lineHeaderHorDebitCredit.Y2 = 0.8F;
            // 
            // lineHeaderVerDebitDepartment
            // 
            this.lineHeaderVerDebitDepartment.Height = 0.2F;
            this.lineHeaderVerDebitDepartment.Left = 1.94F;
            this.lineHeaderVerDebitDepartment.LineWeight = 1F;
            this.lineHeaderVerDebitDepartment.Name = "lineHeaderVerDebitDepartment";
            this.lineHeaderVerDebitDepartment.Top = 0.8F;
            this.lineHeaderVerDebitDepartment.Width = 0F;
            this.lineHeaderVerDebitDepartment.X1 = 1.94F;
            this.lineHeaderVerDebitDepartment.X2 = 1.94F;
            this.lineHeaderVerDebitDepartment.Y1 = 0.8F;
            this.lineHeaderVerDebitDepartment.Y2 = 1F;
            // 
            // lineHeaderVerDebitAccTitle
            // 
            this.lineHeaderVerDebitAccTitle.Height = 0.2F;
            this.lineHeaderVerDebitAccTitle.Left = 2.77F;
            this.lineHeaderVerDebitAccTitle.LineWeight = 1F;
            this.lineHeaderVerDebitAccTitle.Name = "lineHeaderVerDebitAccTitle";
            this.lineHeaderVerDebitAccTitle.Top = 0.8F;
            this.lineHeaderVerDebitAccTitle.Width = 0F;
            this.lineHeaderVerDebitAccTitle.X1 = 2.77F;
            this.lineHeaderVerDebitAccTitle.X2 = 2.77F;
            this.lineHeaderVerDebitAccTitle.Y1 = 0.8F;
            this.lineHeaderVerDebitAccTitle.Y2 = 1F;
            // 
            // lineHeaderVerCreditDepartment
            // 
            this.lineHeaderVerCreditDepartment.Height = 0.2F;
            this.lineHeaderVerCreditDepartment.Left = 4.429827F;
            this.lineHeaderVerCreditDepartment.LineWeight = 1F;
            this.lineHeaderVerCreditDepartment.Name = "lineHeaderVerCreditDepartment";
            this.lineHeaderVerCreditDepartment.Top = 0.8F;
            this.lineHeaderVerCreditDepartment.Width = 0.0001730919F;
            this.lineHeaderVerCreditDepartment.X1 = 4.43F;
            this.lineHeaderVerCreditDepartment.X2 = 4.429827F;
            this.lineHeaderVerCreditDepartment.Y1 = 0.8F;
            this.lineHeaderVerCreditDepartment.Y2 = 1F;
            // 
            // lineHeaderVerCreditAccTitleCode
            // 
            this.lineHeaderVerCreditAccTitleCode.Height = 0.2F;
            this.lineHeaderVerCreditAccTitleCode.Left = 5.26F;
            this.lineHeaderVerCreditAccTitleCode.LineWeight = 1F;
            this.lineHeaderVerCreditAccTitleCode.Name = "lineHeaderVerCreditAccTitleCode";
            this.lineHeaderVerCreditAccTitleCode.Top = 0.8F;
            this.lineHeaderVerCreditAccTitleCode.Width = 0F;
            this.lineHeaderVerCreditAccTitleCode.X1 = 5.26F;
            this.lineHeaderVerCreditAccTitleCode.X2 = 5.26F;
            this.lineHeaderVerCreditAccTitleCode.Y1 = 0.8F;
            this.lineHeaderVerCreditAccTitleCode.Y2 = 1F;
            // 
            // lbldate
            // 
            this.lbldate.Height = 0.2F;
            this.lbldate.HyperLink = null;
            this.lbldate.Left = 8.809055F;
            this.lbldate.Name = "lbldate";
            this.lbldate.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lbldate.Text = "出力日付:";
            this.lbldate.Top = 0F;
            this.lbldate.Width = 0.6984252F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 9.522441F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle";
            this.ridate.Top = 0F;
            this.ridate.Width = 1.014961F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1F;
            this.lineHeaderHorLower.Width = 10.6F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.6F;
            this.lineHeaderHorLower.Y1 = 1F;
            this.lineHeaderHorLower.Y2 = 1F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtBilledAt,
            this.txtSlipNumber,
            this.txtDebitDepartmentCode,
            this.txtDebitDepartmentName,
            this.txtDebitAccTitleCode,
            this.txtDebitAccTitleName,
            this.txtDebitSubCode,
            this.txtDebitSubName,
            this.txtCreditDepartmentCode,
            this.txtCreditDepartmentName,
            this.txtCreditAccountTitleCode,
            this.txtCreditAccTitleName,
            this.txtCreditSubCode,
            this.txtCreditSubName,
            this.txtAmount,
            this.txtNote,
            this.txtCustomerCode,
            this.txtCustomerName,
            this.lineDetailVerBilledAt,
            this.lineDetailVerSlipNumber,
            this.lineDetailVerDebitDepartment,
            this.lineDetailVerDebitAccTitle,
            this.lineDetailVerDebitSubCode,
            this.lineDetailVerCreditDepartment,
            this.lineDetailVerCreditAccCode,
            this.lineDetailVerNote,
            this.txtInvoiceCode,
            this.txtCurrencyCode,
            this.lineDetailVerCustomerCode,
            this.lineDetailVerAmount,
            this.lineDetailHorLower,
            this.lineDetailVerCredit});
            this.detail.Height = 0.4046752F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtBilledAt
            // 
            this.txtBilledAt.Height = 0.4F;
            this.txtBilledAt.Left = 0F;
            this.txtBilledAt.MultiLine = false;
            this.txtBilledAt.Name = "txtBilledAt";
            this.txtBilledAt.OutputFormat = resources.GetString("txtBilledAt.OutputFormat");
            this.txtBilledAt.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtBilledAt.Text = "txtBilledAt";
            this.txtBilledAt.Top = 0F;
            this.txtBilledAt.Width = 0.55F;
            // 
            // txtSlipNumber
            // 
            this.txtSlipNumber.Height = 0.4F;
            this.txtSlipNumber.Left = 0.55F;
            this.txtSlipNumber.MultiLine = false;
            this.txtSlipNumber.Name = "txtSlipNumber";
            this.txtSlipNumber.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtSlipNumber.Text = "txtSlipNumber";
            this.txtSlipNumber.Top = 0F;
            this.txtSlipNumber.Width = 0.55F;
            // 
            // txtDebitDepartmentCode
            // 
            this.txtDebitDepartmentCode.Height = 0.2F;
            this.txtDebitDepartmentCode.Left = 1.1F;
            this.txtDebitDepartmentCode.MultiLine = false;
            this.txtDebitDepartmentCode.Name = "txtDebitDepartmentCode";
            this.txtDebitDepartmentCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtDebitDepartmentCode.Text = "txtDebitDepartmentCode";
            this.txtDebitDepartmentCode.Top = 0F;
            this.txtDebitDepartmentCode.Width = 0.83F;
            // 
            // txtDebitDepartmentName
            // 
            this.txtDebitDepartmentName.Height = 0.2F;
            this.txtDebitDepartmentName.Left = 1.14F;
            this.txtDebitDepartmentName.MultiLine = false;
            this.txtDebitDepartmentName.Name = "txtDebitDepartmentName";
            this.txtDebitDepartmentName.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; text-justify: auto; vertica" +
    "l-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtDebitDepartmentName.Text = "txtDebitDepartmentName";
            this.txtDebitDepartmentName.Top = 0.2F;
            this.txtDebitDepartmentName.Width = 0.8F;
            // 
            // txtDebitAccTitleCode
            // 
            this.txtDebitAccTitleCode.Height = 0.2F;
            this.txtDebitAccTitleCode.Left = 1.94F;
            this.txtDebitAccTitleCode.MultiLine = false;
            this.txtDebitAccTitleCode.Name = "txtDebitAccTitleCode";
            this.txtDebitAccTitleCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtDebitAccTitleCode.Text = "txtDebitAccTitleCode";
            this.txtDebitAccTitleCode.Top = 0F;
            this.txtDebitAccTitleCode.Width = 0.83F;
            // 
            // txtDebitAccTitleName
            // 
            this.txtDebitAccTitleName.Height = 0.2F;
            this.txtDebitAccTitleName.Left = 1.97F;
            this.txtDebitAccTitleName.MultiLine = false;
            this.txtDebitAccTitleName.Name = "txtDebitAccTitleName";
            this.txtDebitAccTitleName.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; text-justify: auto; vertica" +
    "l-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtDebitAccTitleName.Text = "txtDebitAccTitleName";
            this.txtDebitAccTitleName.Top = 0.2F;
            this.txtDebitAccTitleName.Width = 0.8F;
            // 
            // txtDebitSubCode
            // 
            this.txtDebitSubCode.Height = 0.2F;
            this.txtDebitSubCode.Left = 2.77F;
            this.txtDebitSubCode.MultiLine = false;
            this.txtDebitSubCode.Name = "txtDebitSubCode";
            this.txtDebitSubCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtDebitSubCode.Text = "txtDebitSubCode";
            this.txtDebitSubCode.Top = 0F;
            this.txtDebitSubCode.Width = 0.83F;
            // 
            // txtDebitSubName
            // 
            this.txtDebitSubName.Height = 0.2F;
            this.txtDebitSubName.Left = 2.8F;
            this.txtDebitSubName.MultiLine = false;
            this.txtDebitSubName.Name = "txtDebitSubName";
            this.txtDebitSubName.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; text-justify: auto; vertica" +
    "l-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtDebitSubName.Text = "txtDebitSubName";
            this.txtDebitSubName.Top = 0.2F;
            this.txtDebitSubName.Width = 0.8F;
            // 
            // txtCreditDepartmentCode
            // 
            this.txtCreditDepartmentCode.Height = 0.2F;
            this.txtCreditDepartmentCode.Left = 3.6F;
            this.txtCreditDepartmentCode.MultiLine = false;
            this.txtCreditDepartmentCode.Name = "txtCreditDepartmentCode";
            this.txtCreditDepartmentCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCreditDepartmentCode.Text = "txtCreditDepartmentCode";
            this.txtCreditDepartmentCode.Top = 0F;
            this.txtCreditDepartmentCode.Width = 0.83F;
            // 
            // txtCreditDepartmentName
            // 
            this.txtCreditDepartmentName.Height = 0.2F;
            this.txtCreditDepartmentName.Left = 3.63F;
            this.txtCreditDepartmentName.MultiLine = false;
            this.txtCreditDepartmentName.Name = "txtCreditDepartmentName";
            this.txtCreditDepartmentName.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; text-justify: auto; vertica" +
    "l-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCreditDepartmentName.Text = "txtCreditDepartmentName";
            this.txtCreditDepartmentName.Top = 0.2F;
            this.txtCreditDepartmentName.Width = 0.8F;
            // 
            // txtCreditAccountTitleCode
            // 
            this.txtCreditAccountTitleCode.Height = 0.2F;
            this.txtCreditAccountTitleCode.Left = 4.43F;
            this.txtCreditAccountTitleCode.MultiLine = false;
            this.txtCreditAccountTitleCode.Name = "txtCreditAccountTitleCode";
            this.txtCreditAccountTitleCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCreditAccountTitleCode.Text = "txtCreditAccTitleCode";
            this.txtCreditAccountTitleCode.Top = 0F;
            this.txtCreditAccountTitleCode.Width = 0.83F;
            // 
            // txtCreditAccTitleName
            // 
            this.txtCreditAccTitleName.Height = 0.2F;
            this.txtCreditAccTitleName.Left = 4.46F;
            this.txtCreditAccTitleName.MultiLine = false;
            this.txtCreditAccTitleName.Name = "txtCreditAccTitleName";
            this.txtCreditAccTitleName.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; text-justify: auto; vertica" +
    "l-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCreditAccTitleName.Text = "txtCreditAccTitleName";
            this.txtCreditAccTitleName.Top = 0.2F;
            this.txtCreditAccTitleName.Width = 0.8F;
            // 
            // txtCreditSubCode
            // 
            this.txtCreditSubCode.Height = 0.2F;
            this.txtCreditSubCode.Left = 5.269F;
            this.txtCreditSubCode.MultiLine = false;
            this.txtCreditSubCode.Name = "txtCreditSubCode";
            this.txtCreditSubCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCreditSubCode.Text = "txtCreditSubCode";
            this.txtCreditSubCode.Top = 0F;
            this.txtCreditSubCode.Width = 0.8300004F;
            // 
            // txtCreditSubName
            // 
            this.txtCreditSubName.Height = 0.2F;
            this.txtCreditSubName.Left = 5.3F;
            this.txtCreditSubName.MultiLine = false;
            this.txtCreditSubName.Name = "txtCreditSubName";
            this.txtCreditSubName.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; text-justify: auto; vertica" +
    "l-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCreditSubName.Text = "txtCreditSubName";
            this.txtCreditSubName.Top = 0.2F;
            this.txtCreditSubName.Width = 0.8F;
            // 
            // txtAmount
            // 
            this.txtAmount.Height = 0.4F;
            this.txtAmount.Left = 6.1F;
            this.txtAmount.MultiLine = false;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtAmount.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: right; text-justify: auto; vertic" +
    "al-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtAmount.Text = "txtAmount";
            this.txtAmount.Top = 0F;
            this.txtAmount.Width = 1.08F;
            // 
            // txtNote
            // 
            this.txtNote.Height = 0.4F;
            this.txtNote.Left = 7.22F;
            this.txtNote.MultiLine = false;
            this.txtNote.Name = "txtNote";
            this.txtNote.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtNote.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; text-justify: auto; vertica" +
    "l-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtNote.Text = "txtNote";
            this.txtNote.Top = 0F;
            this.txtNote.Width = 1.48F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 8.700001F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCustomerCode.Text = "txtCustomerCode";
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 1.099999F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 8.722F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; text-justify: auto; vertica" +
    "l-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCustomerName.Text = "txtCustomerName";
            this.txtCustomerName.Top = 0.2F;
            this.txtCustomerName.Width = 1.08F;
            // 
            // lineDetailVerBilledAt
            // 
            this.lineDetailVerBilledAt.Height = 0.4F;
            this.lineDetailVerBilledAt.Left = 0.5499055F;
            this.lineDetailVerBilledAt.LineWeight = 1F;
            this.lineDetailVerBilledAt.Name = "lineDetailVerBilledAt";
            this.lineDetailVerBilledAt.Top = 0F;
            this.lineDetailVerBilledAt.Width = 9.453297E-05F;
            this.lineDetailVerBilledAt.X1 = 0.55F;
            this.lineDetailVerBilledAt.X2 = 0.5499055F;
            this.lineDetailVerBilledAt.Y1 = 0F;
            this.lineDetailVerBilledAt.Y2 = 0.4F;
            // 
            // lineDetailVerSlipNumber
            // 
            this.lineDetailVerSlipNumber.Height = 0.4F;
            this.lineDetailVerSlipNumber.Left = 1.099961F;
            this.lineDetailVerSlipNumber.LineWeight = 1F;
            this.lineDetailVerSlipNumber.Name = "lineDetailVerSlipNumber";
            this.lineDetailVerSlipNumber.Top = 0F;
            this.lineDetailVerSlipNumber.Width = 3.898144E-05F;
            this.lineDetailVerSlipNumber.X1 = 1.1F;
            this.lineDetailVerSlipNumber.X2 = 1.099961F;
            this.lineDetailVerSlipNumber.Y1 = 0F;
            this.lineDetailVerSlipNumber.Y2 = 0.4F;
            // 
            // lineDetailVerDebitDepartment
            // 
            this.lineDetailVerDebitDepartment.Height = 0.4F;
            this.lineDetailVerDebitDepartment.Left = 1.94F;
            this.lineDetailVerDebitDepartment.LineWeight = 1F;
            this.lineDetailVerDebitDepartment.Name = "lineDetailVerDebitDepartment";
            this.lineDetailVerDebitDepartment.Top = 0F;
            this.lineDetailVerDebitDepartment.Width = 0F;
            this.lineDetailVerDebitDepartment.X1 = 1.94F;
            this.lineDetailVerDebitDepartment.X2 = 1.94F;
            this.lineDetailVerDebitDepartment.Y1 = 0F;
            this.lineDetailVerDebitDepartment.Y2 = 0.4F;
            // 
            // lineDetailVerDebitAccTitle
            // 
            this.lineDetailVerDebitAccTitle.Height = 0.4F;
            this.lineDetailVerDebitAccTitle.Left = 2.77F;
            this.lineDetailVerDebitAccTitle.LineWeight = 1F;
            this.lineDetailVerDebitAccTitle.Name = "lineDetailVerDebitAccTitle";
            this.lineDetailVerDebitAccTitle.Top = 0F;
            this.lineDetailVerDebitAccTitle.Width = 0F;
            this.lineDetailVerDebitAccTitle.X1 = 2.77F;
            this.lineDetailVerDebitAccTitle.X2 = 2.77F;
            this.lineDetailVerDebitAccTitle.Y1 = 0F;
            this.lineDetailVerDebitAccTitle.Y2 = 0.4F;
            // 
            // lineDetailVerDebitSubCode
            // 
            this.lineDetailVerDebitSubCode.Height = 0.4F;
            this.lineDetailVerDebitSubCode.Left = 3.6F;
            this.lineDetailVerDebitSubCode.LineWeight = 1F;
            this.lineDetailVerDebitSubCode.Name = "lineDetailVerDebitSubCode";
            this.lineDetailVerDebitSubCode.Top = 0F;
            this.lineDetailVerDebitSubCode.Width = 0F;
            this.lineDetailVerDebitSubCode.X1 = 3.6F;
            this.lineDetailVerDebitSubCode.X2 = 3.6F;
            this.lineDetailVerDebitSubCode.Y1 = 0F;
            this.lineDetailVerDebitSubCode.Y2 = 0.4F;
            // 
            // lineDetailVerCreditDepartment
            // 
            this.lineDetailVerCreditDepartment.Height = 0.4F;
            this.lineDetailVerCreditDepartment.Left = 4.43F;
            this.lineDetailVerCreditDepartment.LineWeight = 1F;
            this.lineDetailVerCreditDepartment.Name = "lineDetailVerCreditDepartment";
            this.lineDetailVerCreditDepartment.Top = -4.440892E-16F;
            this.lineDetailVerCreditDepartment.Width = 0F;
            this.lineDetailVerCreditDepartment.X1 = 4.43F;
            this.lineDetailVerCreditDepartment.X2 = 4.43F;
            this.lineDetailVerCreditDepartment.Y1 = -4.440892E-16F;
            this.lineDetailVerCreditDepartment.Y2 = 0.4F;
            // 
            // lineDetailVerCreditAccCode
            // 
            this.lineDetailVerCreditAccCode.Height = 0.4F;
            this.lineDetailVerCreditAccCode.Left = 5.26F;
            this.lineDetailVerCreditAccCode.LineWeight = 1F;
            this.lineDetailVerCreditAccCode.Name = "lineDetailVerCreditAccCode";
            this.lineDetailVerCreditAccCode.Top = 0F;
            this.lineDetailVerCreditAccCode.Width = 0F;
            this.lineDetailVerCreditAccCode.X1 = 5.26F;
            this.lineDetailVerCreditAccCode.X2 = 5.26F;
            this.lineDetailVerCreditAccCode.Y1 = 0F;
            this.lineDetailVerCreditAccCode.Y2 = 0.4F;
            // 
            // lineDetailVerCredit
            // 
            this.lineDetailVerCredit.Height = 0.4F;
            this.lineDetailVerCredit.Left = 6.1F;
            this.lineDetailVerCredit.LineWeight = 1F;
            this.lineDetailVerCredit.Name = "lineDetailVerCredit";
            this.lineDetailVerCredit.Top = 0F;
            this.lineDetailVerCredit.Width = 0F;
            this.lineDetailVerCredit.X1 = 6.1F;
            this.lineDetailVerCredit.X2 = 6.1F;
            this.lineDetailVerCredit.Y1 = 0F;
            this.lineDetailVerCredit.Y2 = 0.4F;
            // 
            // lineDetailVerNote
            // 
            this.lineDetailVerNote.Height = 0.4F;
            this.lineDetailVerNote.Left = 8.7F;
            this.lineDetailVerNote.LineWeight = 1F;
            this.lineDetailVerNote.Name = "lineDetailVerNote";
            this.lineDetailVerNote.Top = 0F;
            this.lineDetailVerNote.Width = 0F;
            this.lineDetailVerNote.X1 = 8.7F;
            this.lineDetailVerNote.X2 = 8.7F;
            this.lineDetailVerNote.Y1 = 0F;
            this.lineDetailVerNote.Y2 = 0.4F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.4F;
            this.lineDetailHorLower.Width = 10.6F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.6F;
            this.lineDetailHorLower.Y1 = 0.4F;
            this.lineDetailHorLower.Y2 = 0.4F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.Height = 0.2F;
            this.txtInvoiceCode.Left = 9.8F;
            this.txtInvoiceCode.MultiLine = false;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtInvoiceCode.Text = "txtInvoiceCode";
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 0.8F;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.Height = 0.2F;
            this.txtCurrencyCode.Left = 9.8F;
            this.txtCurrencyCode.MultiLine = false;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; text-justify: auto; verti" +
    "cal-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCurrencyCode.Text = "txtCurrencyCode";
            this.txtCurrencyCode.Top = 0.2F;
            this.txtCurrencyCode.Width = 0.8F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.4F;
            this.lineDetailVerCustomerCode.Left = 9.8F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 9.8F;
            this.lineDetailVerCustomerCode.X2 = 9.8F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.4F;
            // 
            // lineDetailVerAmount
            // 
            this.lineDetailVerAmount.Height = 0.4F;
            this.lineDetailVerAmount.Left = 7.2F;
            this.lineDetailVerAmount.LineWeight = 1F;
            this.lineDetailVerAmount.Name = "lineDetailVerAmount";
            this.lineDetailVerAmount.Top = 0F;
            this.lineDetailVerAmount.Width = 0F;
            this.lineDetailVerAmount.X1 = 7.2F;
            this.lineDetailVerAmount.X2 = 7.2F;
            this.lineDetailVerAmount.Y1 = 0F;
            this.lineDetailVerAmount.Y2 = 0.4F;
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
            this.lblSum,
            this.lblFooterSpace,
            this.lblNumber,
            this.lineFooterVerTotal,
            this.lineFooterVerSpace,
            this.lineFooterHorLower,
            this.lineFooterVerSum});
            this.groupFooter1.Height = 0.3128445F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // lblTotal
            // 
            this.lblTotal.Height = 0.3F;
            this.lblTotal.HyperLink = null;
            this.lblTotal.Left = 0F;
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(10, 0, 0, 0);
            this.lblTotal.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: l" +
    "eft; text-justify: auto; vertical-align: middle; ddo-char-set: 1; ddo-shrink-to-" +
    "fit: none";
            this.lblTotal.Text = "合計";
            this.lblTotal.Top = 0F;
            this.lblTotal.Width = 6.1F;
            // 
            // lblSum
            // 
            this.lblSum.Height = 0.3F;
            this.lblSum.HyperLink = null;
            this.lblSum.Left = 6.1F;
            this.lblSum.MultiLine = false;
            this.lblSum.Name = "lblSum";
            this.lblSum.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.lblSum.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.lblSum.Text = "";
            this.lblSum.Top = 0F;
            this.lblSum.Width = 1.08F;
            // 
            // lblFooterSpace
            // 
            this.lblFooterSpace.Height = 0.3F;
            this.lblFooterSpace.HyperLink = null;
            this.lblFooterSpace.Left = 7.2F;
            this.lblFooterSpace.Name = "lblFooterSpace";
            this.lblFooterSpace.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 6pt; text-align: lef" +
    "t; text-justify: auto; vertical-align: middle; ddo-char-set: 1; ddo-shrink-to-fi" +
    "t: none";
            this.lblFooterSpace.Text = "";
            this.lblFooterSpace.Top = 0F;
            this.lblFooterSpace.Width = 2.6F;
            // 
            // lblNumber
            // 
            this.lblNumber.Height = 0.3F;
            this.lblNumber.HyperLink = null;
            this.lblNumber.Left = 9.8F;
            this.lblNumber.MultiLine = false;
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.lblNumber.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 6pt; text-align: rig" +
    "ht; text-justify: auto; vertical-align: middle; ddo-char-set: 1; ddo-shrink-to-f" +
    "it: none";
            this.lblNumber.Text = "RecCount";
            this.lblNumber.Top = 0F;
            this.lblNumber.Width = 0.8000002F;
            // 
            // lineFooterVerTotal
            // 
            this.lineFooterVerTotal.Height = 0.3F;
            this.lineFooterVerTotal.Left = 6.1F;
            this.lineFooterVerTotal.LineWeight = 1F;
            this.lineFooterVerTotal.Name = "lineFooterVerTotal";
            this.lineFooterVerTotal.Top = 0F;
            this.lineFooterVerTotal.Width = 0F;
            this.lineFooterVerTotal.X1 = 6.1F;
            this.lineFooterVerTotal.X2 = 6.1F;
            this.lineFooterVerTotal.Y1 = 0F;
            this.lineFooterVerTotal.Y2 = 0.3F;
            // 
            // lineFooterVerSpace
            // 
            this.lineFooterVerSpace.Height = 0.3F;
            this.lineFooterVerSpace.Left = 9.8F;
            this.lineFooterVerSpace.LineWeight = 1F;
            this.lineFooterVerSpace.Name = "lineFooterVerSpace";
            this.lineFooterVerSpace.Top = 0F;
            this.lineFooterVerSpace.Width = 0F;
            this.lineFooterVerSpace.X1 = 9.8F;
            this.lineFooterVerSpace.X2 = 9.8F;
            this.lineFooterVerSpace.Y1 = 0F;
            this.lineFooterVerSpace.Y2 = 0.3F;
            // 
            // lineFooterHorLower
            // 
            this.lineFooterHorLower.Height = 0F;
            this.lineFooterHorLower.Left = 0F;
            this.lineFooterHorLower.LineWeight = 1F;
            this.lineFooterHorLower.Name = "lineFooterHorLower";
            this.lineFooterHorLower.Top = 0.3F;
            this.lineFooterHorLower.Width = 10.6F;
            this.lineFooterHorLower.X1 = 0F;
            this.lineFooterHorLower.X2 = 10.6F;
            this.lineFooterHorLower.Y1 = 0.3F;
            this.lineFooterHorLower.Y2 = 0.3F;
            // 
            // lineFooterVerSum
            // 
            this.lineFooterVerSum.Height = 0.3F;
            this.lineFooterVerSum.Left = 7.2F;
            this.lineFooterVerSum.LineWeight = 1F;
            this.lineFooterVerSum.Name = "lineFooterVerSum";
            this.lineFooterVerSum.Top = 0F;
            this.lineFooterVerSum.Width = 0F;
            this.lineFooterVerSum.X1 = 7.2F;
            this.lineFooterVerSum.X2 = 7.2F;
            this.lineFooterVerSum.Y1 = 0F;
            this.lineFooterVerSum.Y2 = 0.3F;
            // 
            // BillingJournalizingSectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.60676F;
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
            this.DataInitialize += new System.EventHandler(this.BillingJournalizingSectionReport_DataInitialize);
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSlipNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitAccTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAccTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditSupplementary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlipNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccTitleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccTitleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFooterSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompany;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitAccTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditAccTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditSupplementary;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorDebitCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebitAccTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditAccTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitAccTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitAccTitleName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitSubName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditAccTitleName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditSubName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitAccTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditAccCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSum;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblFooterSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerSum;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAmount;
    }
}
