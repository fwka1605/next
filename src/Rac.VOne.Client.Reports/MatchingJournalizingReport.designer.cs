namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillSectionReport の概要の説明です。
    /// </summary>
    partial class MatchingJournalizingReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MatchingJournalizingReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblBranchName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBankName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBankCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditSubCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebitSubCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebitAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebit = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCredit = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebitAccountTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditAccountTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCredit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBankCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorDebit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCreditSubCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitSubCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitSubName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitAccountTitleName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtDebitDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtCreditDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditAccountTitleName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditSubName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerDebit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCredit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBankCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtDebitAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditAccountTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitAccountTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line14 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtTotalCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerSpace = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSlipNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlipNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblBranchName,
            this.lblBankName,
            this.lblAccountNumber,
            this.lblBranchCode,
            this.lblBankCode,
            this.lblCurrencyCode,
            this.lblCustomerCodeName,
            this.lblCreditSubCode,
            this.lblCreditAccountTitleCode,
            this.lblCreditDepartment,
            this.lblDebitSubCode,
            this.lblDebitAccountTitleCode,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblSlipNumber,
            this.lblDebitDepartment,
            this.lblDebit,
            this.lblCredit,
            this.lblAmount,
            this.lblNote,
            this.lblInvoiceCode,
            this.lineHeaderVerSlipNumber,
            this.lineHeaderVerDebitDepartment,
            this.lineHeaderVerDebit,
            this.lblRecordedAt,
            this.lineHeaderVerRecordedAt,
            this.lineHeaderHorUpper,
            this.lineHeaderVerDebitAccountTitle,
            this.lineHeaderVerCreditDepartment,
            this.lineHeaderVerCreditAccountTitle,
            this.lineHeaderVerCredit,
            this.lineHeaderVerAmount,
            this.lineHeaderVerNote,
            this.lineHeaderVerBankCode,
            this.lineHeaderVerBranchCode,
            this.lineHeaderVerAccountNumber,
            this.lineHeaderHorDebit,
            this.lineHeaderHorNote,
            this.lineHeaderHorCurrencyCode,
            this.lineHeaderHorLower});
            this.pageHeader.Height = 1.007644F;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.BeforePrint += new System.EventHandler(this.pageHeader_BeforePrint);
            // 
            // lblBranchName
            // 
            this.lblBranchName.Height = 0.2F;
            this.lblBranchName.HyperLink = null;
            this.lblBranchName.Left = 8.73F;
            this.lblBranchName.Name = "lblBranchName";
            this.lblBranchName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblBranchName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBranchName.Text = "支店名";
            this.lblBranchName.Top = 0.8F;
            this.lblBranchName.Width = 0.7F;
            // 
            // lblBankName
            // 
            this.lblBankName.Height = 0.2F;
            this.lblBankName.HyperLink = null;
            this.lblBankName.Left = 8.030001F;
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblBankName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBankName.Text = "銀行名";
            this.lblBankName.Top = 0.8F;
            this.lblBankName.Width = 0.7F;
            // 
            // lblAccountNumber
            // 
            this.lblAccountNumber.Height = 0.4F;
            this.lblAccountNumber.HyperLink = null;
            this.lblAccountNumber.Left = 9.43F;
            this.lblAccountNumber.Name = "lblAccountNumber";
            this.lblAccountNumber.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; white-space: nowrap; ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.lblAccountNumber.Text = "口座番号";
            this.lblAccountNumber.Top = 0.6F;
            this.lblAccountNumber.Width = 0.5F;
            // 
            // lblBranchCode
            // 
            this.lblBranchCode.Height = 0.2F;
            this.lblBranchCode.HyperLink = null;
            this.lblBranchCode.Left = 8.73F;
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBranchCode.Text = "支店コード";
            this.lblBranchCode.Top = 0.6F;
            this.lblBranchCode.Width = 0.7F;
            // 
            // lblBankCode
            // 
            this.lblBankCode.Height = 0.2F;
            this.lblBankCode.HyperLink = null;
            this.lblBankCode.Left = 8.030001F;
            this.lblBankCode.Name = "lblBankCode";
            this.lblBankCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBankCode.Text = "銀行コード";
            this.lblBankCode.Top = 0.6F;
            this.lblBankCode.Width = 0.7F;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Height = 0.2F;
            this.lblCurrencyCode.HyperLink = null;
            this.lblCurrencyCode.Left = 9.93F;
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.Top = 0.8F;
            this.lblCurrencyCode.Width = 0.7F;
            // 
            // lblCustomerCodeName
            // 
            this.lblCustomerCodeName.Height = 0.1999999F;
            this.lblCustomerCodeName.HyperLink = null;
            this.lblCustomerCodeName.Left = 6.5F;
            this.lblCustomerCodeName.Name = "lblCustomerCodeName";
            this.lblCustomerCodeName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerCodeName.Text = "得意先コード / 得意先名";
            this.lblCustomerCodeName.Top = 0.8F;
            this.lblCustomerCodeName.Width = 1.53F;
            // 
            // lblCreditSubCode
            // 
            this.lblCreditSubCode.Height = 0.2F;
            this.lblCreditSubCode.HyperLink = null;
            this.lblCreditSubCode.Left = 4.95F;
            this.lblCreditSubCode.Name = "lblCreditSubCode";
            this.lblCreditSubCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCreditSubCode.Text = "補助コード";
            this.lblCreditSubCode.Top = 0.8F;
            this.lblCreditSubCode.Width = 0.75F;
            // 
            // lblCreditAccountTitleCode
            // 
            this.lblCreditAccountTitleCode.Height = 0.2F;
            this.lblCreditAccountTitleCode.HyperLink = null;
            this.lblCreditAccountTitleCode.Left = 4.2F;
            this.lblCreditAccountTitleCode.Name = "lblCreditAccountTitleCode";
            this.lblCreditAccountTitleCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCreditAccountTitleCode.Text = "科目コード";
            this.lblCreditAccountTitleCode.Top = 0.8F;
            this.lblCreditAccountTitleCode.Width = 0.75F;
            // 
            // lblCreditDepartment
            // 
            this.lblCreditDepartment.Height = 0.2F;
            this.lblCreditDepartment.HyperLink = null;
            this.lblCreditDepartment.Left = 3.45F;
            this.lblCreditDepartment.Name = "lblCreditDepartment";
            this.lblCreditDepartment.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCreditDepartment.Text = "部門コード";
            this.lblCreditDepartment.Top = 0.8F;
            this.lblCreditDepartment.Width = 0.75F;
            // 
            // lblDebitSubCode
            // 
            this.lblDebitSubCode.Height = 0.1999999F;
            this.lblDebitSubCode.HyperLink = null;
            this.lblDebitSubCode.Left = 2.7F;
            this.lblDebitSubCode.Name = "lblDebitSubCode";
            this.lblDebitSubCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebitSubCode.Text = "補助コード";
            this.lblDebitSubCode.Top = 0.8F;
            this.lblDebitSubCode.Width = 0.75F;
            // 
            // lblDebitAccountTitleCode
            // 
            this.lblDebitAccountTitleCode.Height = 0.2F;
            this.lblDebitAccountTitleCode.HyperLink = null;
            this.lblDebitAccountTitleCode.Left = 1.95F;
            this.lblDebitAccountTitleCode.Name = "lblDebitAccountTitleCode";
            this.lblDebitAccountTitleCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebitAccountTitleCode.Text = "科目コード";
            this.lblDebitAccountTitleCode.Top = 0.8F;
            this.lblDebitAccountTitleCode.Width = 0.75F;
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
            this.lblTitle.Text = "消込仕訳出力";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblSlipNumber
            // 
            this.lblSlipNumber.Height = 0.4F;
            this.lblSlipNumber.HyperLink = null;
            this.lblSlipNumber.Left = 0.6F;
            this.lblSlipNumber.Name = "lblSlipNumber";
            this.lblSlipNumber.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblSlipNumber.Text = "伝票番号";
            this.lblSlipNumber.Top = 0.6F;
            this.lblSlipNumber.Width = 0.6F;
            // 
            // lblDebitDepartment
            // 
            this.lblDebitDepartment.Height = 0.2F;
            this.lblDebitDepartment.HyperLink = null;
            this.lblDebitDepartment.Left = 1.2F;
            this.lblDebitDepartment.Name = "lblDebitDepartment";
            this.lblDebitDepartment.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebitDepartment.Text = "部門コード";
            this.lblDebitDepartment.Top = 0.8F;
            this.lblDebitDepartment.Width = 0.75F;
            // 
            // lblDebit
            // 
            this.lblDebit.Height = 0.2F;
            this.lblDebit.HyperLink = null;
            this.lblDebit.Left = 1.2F;
            this.lblDebit.Name = "lblDebit";
            this.lblDebit.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebit.Text = "借方";
            this.lblDebit.Top = 0.6F;
            this.lblDebit.Width = 2.25F;
            // 
            // lblCredit
            // 
            this.lblCredit.Height = 0.2F;
            this.lblCredit.HyperLink = null;
            this.lblCredit.Left = 3.45F;
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCredit.Text = "貸方";
            this.lblCredit.Top = 0.6F;
            this.lblCredit.Width = 2.25F;
            // 
            // lblAmount
            // 
            this.lblAmount.Height = 0.4F;
            this.lblAmount.HyperLink = null;
            this.lblAmount.Left = 5.7F;
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; white-space: nowrap; ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.lblAmount.Text = "仕訳金額";
            this.lblAmount.Top = 0.6F;
            this.lblAmount.Width = 0.8F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.2F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 6.5F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblNote.Text = "備考";
            this.lblNote.Top = 0.6F;
            this.lblNote.Width = 1.53F;
            // 
            // lblInvoiceCode
            // 
            this.lblInvoiceCode.Height = 0.2F;
            this.lblInvoiceCode.HyperLink = null;
            this.lblInvoiceCode.Left = 9.93F;
            this.lblInvoiceCode.Name = "lblInvoiceCode";
            this.lblInvoiceCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblInvoiceCode.Text = "請求書番号";
            this.lblInvoiceCode.Top = 0.6F;
            this.lblInvoiceCode.Width = 0.7F;
            // 
            // lineHeaderVerSlipNumber
            // 
            this.lineHeaderVerSlipNumber.Height = 0.4F;
            this.lineHeaderVerSlipNumber.Left = 1.2F;
            this.lineHeaderVerSlipNumber.LineWeight = 1F;
            this.lineHeaderVerSlipNumber.Name = "lineHeaderVerSlipNumber";
            this.lineHeaderVerSlipNumber.Top = 0.6F;
            this.lineHeaderVerSlipNumber.Width = 0F;
            this.lineHeaderVerSlipNumber.X1 = 1.2F;
            this.lineHeaderVerSlipNumber.X2 = 1.2F;
            this.lineHeaderVerSlipNumber.Y1 = 0.6F;
            this.lineHeaderVerSlipNumber.Y2 = 1F;
            // 
            // lineHeaderVerDebitDepartment
            // 
            this.lineHeaderVerDebitDepartment.Height = 0.2F;
            this.lineHeaderVerDebitDepartment.Left = 1.95F;
            this.lineHeaderVerDebitDepartment.LineWeight = 1F;
            this.lineHeaderVerDebitDepartment.Name = "lineHeaderVerDebitDepartment";
            this.lineHeaderVerDebitDepartment.Top = 0.8F;
            this.lineHeaderVerDebitDepartment.Width = 0F;
            this.lineHeaderVerDebitDepartment.X1 = 1.95F;
            this.lineHeaderVerDebitDepartment.X2 = 1.95F;
            this.lineHeaderVerDebitDepartment.Y1 = 0.8F;
            this.lineHeaderVerDebitDepartment.Y2 = 1F;
            // 
            // lineHeaderVerDebit
            // 
            this.lineHeaderVerDebit.Height = 0.4F;
            this.lineHeaderVerDebit.Left = 3.45F;
            this.lineHeaderVerDebit.LineWeight = 1F;
            this.lineHeaderVerDebit.Name = "lineHeaderVerDebit";
            this.lineHeaderVerDebit.Top = 0.6F;
            this.lineHeaderVerDebit.Width = 0F;
            this.lineHeaderVerDebit.X1 = 3.45F;
            this.lineHeaderVerDebit.X2 = 3.45F;
            this.lineHeaderVerDebit.Y1 = 0.6F;
            this.lineHeaderVerDebit.Y2 = 1F;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Height = 0.4F;
            this.lblRecordedAt.HyperLink = null;
            this.lblRecordedAt.Left = 0F;
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblRecordedAt.Text = "伝票日付";
            this.lblRecordedAt.Top = 0.6F;
            this.lblRecordedAt.Width = 0.6F;
            // 
            // lineHeaderVerRecordedAt
            // 
            this.lineHeaderVerRecordedAt.Height = 0.4F;
            this.lineHeaderVerRecordedAt.Left = 0.6F;
            this.lineHeaderVerRecordedAt.LineWeight = 1F;
            this.lineHeaderVerRecordedAt.Name = "lineHeaderVerRecordedAt";
            this.lineHeaderVerRecordedAt.Top = 0.6F;
            this.lineHeaderVerRecordedAt.Width = 0F;
            this.lineHeaderVerRecordedAt.X1 = 0.6F;
            this.lineHeaderVerRecordedAt.X2 = 0.6F;
            this.lineHeaderVerRecordedAt.Y1 = 0.6F;
            this.lineHeaderVerRecordedAt.Y2 = 1F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 5.960464E-08F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.6F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 5.960464E-08F;
            this.lineHeaderHorUpper.X2 = 10.62992F;
            this.lineHeaderHorUpper.Y1 = 0.6F;
            this.lineHeaderHorUpper.Y2 = 0.6F;
            // 
            // lineHeaderVerDebitAccountTitle
            // 
            this.lineHeaderVerDebitAccountTitle.Height = 0.2F;
            this.lineHeaderVerDebitAccountTitle.Left = 2.7F;
            this.lineHeaderVerDebitAccountTitle.LineWeight = 1F;
            this.lineHeaderVerDebitAccountTitle.Name = "lineHeaderVerDebitAccountTitle";
            this.lineHeaderVerDebitAccountTitle.Top = 0.8F;
            this.lineHeaderVerDebitAccountTitle.Width = 0F;
            this.lineHeaderVerDebitAccountTitle.X1 = 2.7F;
            this.lineHeaderVerDebitAccountTitle.X2 = 2.7F;
            this.lineHeaderVerDebitAccountTitle.Y1 = 0.8F;
            this.lineHeaderVerDebitAccountTitle.Y2 = 1F;
            // 
            // lineHeaderVerCreditDepartment
            // 
            this.lineHeaderVerCreditDepartment.Height = 0.2F;
            this.lineHeaderVerCreditDepartment.Left = 4.2F;
            this.lineHeaderVerCreditDepartment.LineWeight = 1F;
            this.lineHeaderVerCreditDepartment.Name = "lineHeaderVerCreditDepartment";
            this.lineHeaderVerCreditDepartment.Top = 0.8F;
            this.lineHeaderVerCreditDepartment.Width = 0F;
            this.lineHeaderVerCreditDepartment.X1 = 4.2F;
            this.lineHeaderVerCreditDepartment.X2 = 4.2F;
            this.lineHeaderVerCreditDepartment.Y1 = 0.8F;
            this.lineHeaderVerCreditDepartment.Y2 = 1F;
            // 
            // lineHeaderVerCreditAccountTitle
            // 
            this.lineHeaderVerCreditAccountTitle.Height = 0.1999999F;
            this.lineHeaderVerCreditAccountTitle.Left = 4.95F;
            this.lineHeaderVerCreditAccountTitle.LineWeight = 1F;
            this.lineHeaderVerCreditAccountTitle.Name = "lineHeaderVerCreditAccountTitle";
            this.lineHeaderVerCreditAccountTitle.Top = 0.8F;
            this.lineHeaderVerCreditAccountTitle.Width = 0F;
            this.lineHeaderVerCreditAccountTitle.X1 = 4.95F;
            this.lineHeaderVerCreditAccountTitle.X2 = 4.95F;
            this.lineHeaderVerCreditAccountTitle.Y1 = 0.8F;
            this.lineHeaderVerCreditAccountTitle.Y2 = 0.9999999F;
            // 
            // lineHeaderVerCredit
            // 
            this.lineHeaderVerCredit.Height = 0.4F;
            this.lineHeaderVerCredit.Left = 5.7F;
            this.lineHeaderVerCredit.LineWeight = 1F;
            this.lineHeaderVerCredit.Name = "lineHeaderVerCredit";
            this.lineHeaderVerCredit.Top = 0.6F;
            this.lineHeaderVerCredit.Width = 0F;
            this.lineHeaderVerCredit.X1 = 5.7F;
            this.lineHeaderVerCredit.X2 = 5.7F;
            this.lineHeaderVerCredit.Y1 = 0.6F;
            this.lineHeaderVerCredit.Y2 = 1F;
            // 
            // lineHeaderVerAmount
            // 
            this.lineHeaderVerAmount.Height = 0.4F;
            this.lineHeaderVerAmount.Left = 6.5F;
            this.lineHeaderVerAmount.LineWeight = 1F;
            this.lineHeaderVerAmount.Name = "lineHeaderVerAmount";
            this.lineHeaderVerAmount.Top = 0.6F;
            this.lineHeaderVerAmount.Width = 0F;
            this.lineHeaderVerAmount.X1 = 6.5F;
            this.lineHeaderVerAmount.X2 = 6.5F;
            this.lineHeaderVerAmount.Y1 = 0.6F;
            this.lineHeaderVerAmount.Y2 = 1F;
            // 
            // lineHeaderVerNote
            // 
            this.lineHeaderVerNote.Height = 0.4F;
            this.lineHeaderVerNote.Left = 8.030001F;
            this.lineHeaderVerNote.LineWeight = 1F;
            this.lineHeaderVerNote.Name = "lineHeaderVerNote";
            this.lineHeaderVerNote.Top = 0.6F;
            this.lineHeaderVerNote.Width = 0F;
            this.lineHeaderVerNote.X1 = 8.030001F;
            this.lineHeaderVerNote.X2 = 8.030001F;
            this.lineHeaderVerNote.Y1 = 0.6F;
            this.lineHeaderVerNote.Y2 = 1F;
            // 
            // lineHeaderVerBankCode
            // 
            this.lineHeaderVerBankCode.Height = 0.4F;
            this.lineHeaderVerBankCode.Left = 8.73F;
            this.lineHeaderVerBankCode.LineWeight = 1F;
            this.lineHeaderVerBankCode.Name = "lineHeaderVerBankCode";
            this.lineHeaderVerBankCode.Top = 0.6F;
            this.lineHeaderVerBankCode.Width = 0F;
            this.lineHeaderVerBankCode.X1 = 8.73F;
            this.lineHeaderVerBankCode.X2 = 8.73F;
            this.lineHeaderVerBankCode.Y1 = 0.6F;
            this.lineHeaderVerBankCode.Y2 = 1F;
            // 
            // lineHeaderVerBranchCode
            // 
            this.lineHeaderVerBranchCode.Height = 0.4F;
            this.lineHeaderVerBranchCode.Left = 9.43F;
            this.lineHeaderVerBranchCode.LineWeight = 1F;
            this.lineHeaderVerBranchCode.Name = "lineHeaderVerBranchCode";
            this.lineHeaderVerBranchCode.Top = 0.6F;
            this.lineHeaderVerBranchCode.Width = 0F;
            this.lineHeaderVerBranchCode.X1 = 9.43F;
            this.lineHeaderVerBranchCode.X2 = 9.43F;
            this.lineHeaderVerBranchCode.Y1 = 0.6F;
            this.lineHeaderVerBranchCode.Y2 = 1F;
            // 
            // lineHeaderVerAccountNumber
            // 
            this.lineHeaderVerAccountNumber.Height = 0.4F;
            this.lineHeaderVerAccountNumber.Left = 9.93F;
            this.lineHeaderVerAccountNumber.LineWeight = 1F;
            this.lineHeaderVerAccountNumber.Name = "lineHeaderVerAccountNumber";
            this.lineHeaderVerAccountNumber.Top = 0.6F;
            this.lineHeaderVerAccountNumber.Width = 0F;
            this.lineHeaderVerAccountNumber.X1 = 9.93F;
            this.lineHeaderVerAccountNumber.X2 = 9.93F;
            this.lineHeaderVerAccountNumber.Y1 = 0.6F;
            this.lineHeaderVerAccountNumber.Y2 = 1F;
            // 
            // lineHeaderHorDebit
            // 
            this.lineHeaderHorDebit.Height = 0F;
            this.lineHeaderHorDebit.Left = 1.2F;
            this.lineHeaderHorDebit.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorDebit.LineWeight = 1F;
            this.lineHeaderHorDebit.Name = "lineHeaderHorDebit";
            this.lineHeaderHorDebit.Top = 0.8F;
            this.lineHeaderHorDebit.Width = 4.5F;
            this.lineHeaderHorDebit.X1 = 1.2F;
            this.lineHeaderHorDebit.X2 = 5.7F;
            this.lineHeaderHorDebit.Y1 = 0.8F;
            this.lineHeaderHorDebit.Y2 = 0.8F;
            // 
            // lineHeaderHorNote
            // 
            this.lineHeaderHorNote.Height = 0F;
            this.lineHeaderHorNote.Left = 6.5F;
            this.lineHeaderHorNote.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorNote.LineWeight = 1F;
            this.lineHeaderHorNote.Name = "lineHeaderHorNote";
            this.lineHeaderHorNote.Top = 0.8F;
            this.lineHeaderHorNote.Width = 2.93F;
            this.lineHeaderHorNote.X1 = 6.5F;
            this.lineHeaderHorNote.X2 = 9.43F;
            this.lineHeaderHorNote.Y1 = 0.8F;
            this.lineHeaderHorNote.Y2 = 0.8F;
            // 
            // lineHeaderHorCurrencyCode
            // 
            this.lineHeaderHorCurrencyCode.Height = 0F;
            this.lineHeaderHorCurrencyCode.Left = 9.93F;
            this.lineHeaderHorCurrencyCode.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorCurrencyCode.LineWeight = 1F;
            this.lineHeaderHorCurrencyCode.Name = "lineHeaderHorCurrencyCode";
            this.lineHeaderHorCurrencyCode.Top = 0.8F;
            this.lineHeaderHorCurrencyCode.Width = 0.7004499F;
            this.lineHeaderHorCurrencyCode.X1 = 9.93F;
            this.lineHeaderHorCurrencyCode.X2 = 10.63045F;
            this.lineHeaderHorCurrencyCode.Y1 = 0.8F;
            this.lineHeaderHorCurrencyCode.Y2 = 0.8F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1F;
            this.lineHeaderHorLower.Width = 10.62992F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.62992F;
            this.lineHeaderHorLower.Y1 = 1F;
            this.lineHeaderHorLower.Y2 = 1F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCreditSubCode,
            this.txtDebitSubCode,
            this.txtDebitDepartmentName,
            this.txtAmount,
            this.txtNote,
            this.txtInvoiceCode,
            this.txtBranchName,
            this.txtBankCode,
            this.txtSlipNumber,
            this.txtBranchCode,
            this.txtCurrencyCode,
            this.txtDebitSubName,
            this.txtDebitAccountTitleName,
            this.txtRecordedAt,
            this.txtCustomerCode,
            this.txtBankName,
            this.lineDetailVerRecordedAt,
            this.txtDebitDepartmentCode,
            this.lineDetailVerSlipNumber,
            this.txtCreditDepartmentCode,
            this.txtCreditDepartmentName,
            this.txtCreditAccountTitleName,
            this.txtCreditSubName,
            this.txtAccountNumber,
            this.lineDetailVerDebit,
            this.lineDetailVerCredit,
            this.lineDetailVerAmount,
            this.lineDetailVerBankCode,
            this.lineDetailVerBranchCode,
            this.lineDetailVerAccountNumber,
            this.txtDebitAccountTitleCode,
            this.txtCreditAccountTitleCode,
            this.txtCustomerName,
            this.lineDetailVerCreditDepartment,
            this.lineDetailVerCreditAccountTitle,
            this.lineDetailVerNote,
            this.lineDetailVerDebitDepartment,
            this.lineDetailVerDebitAccountTitle,
            this.line14});
            this.detail.Height = 0.3973425F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            this.detail.AfterPrint += new System.EventHandler(this.detail_AfterPrint);
            // 
            // txtCreditSubCode
            // 
            this.txtCreditSubCode.DataField = "CreditSubCode";
            this.txtCreditSubCode.Height = 0.2F;
            this.txtCreditSubCode.Left = 4.95F;
            this.txtCreditSubCode.Name = "txtCreditSubCode";
            this.txtCreditSubCode.OutputFormat = resources.GetString("txtCreditSubCode.OutputFormat");
            this.txtCreditSubCode.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCreditSubCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCreditSubCode.Text = null;
            this.txtCreditSubCode.Top = 0F;
            this.txtCreditSubCode.Width = 0.7497559F;
            // 
            // txtDebitSubCode
            // 
            this.txtDebitSubCode.DataField = "DebitSubCode";
            this.txtDebitSubCode.Height = 0.2F;
            this.txtDebitSubCode.Left = 2.7F;
            this.txtDebitSubCode.Name = "txtDebitSubCode";
            this.txtDebitSubCode.OutputFormat = resources.GetString("txtDebitSubCode.OutputFormat");
            this.txtDebitSubCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDebitSubCode.Text = null;
            this.txtDebitSubCode.Top = 0F;
            this.txtDebitSubCode.Width = 0.75F;
            // 
            // txtDebitDepartmentName
            // 
            this.txtDebitDepartmentName.DataField = "DebitDepartmentName";
            this.txtDebitDepartmentName.Height = 0.2F;
            this.txtDebitDepartmentName.Left = 1.22F;
            this.txtDebitDepartmentName.Name = "txtDebitDepartmentName";
            this.txtDebitDepartmentName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDebitDepartmentName.Text = null;
            this.txtDebitDepartmentName.Top = 0.2F;
            this.txtDebitDepartmentName.Width = 0.73F;
            // 
            // txtAmount
            // 
            this.txtAmount.DataField = "Amount";
            this.txtAmount.Height = 0.4F;
            this.txtAmount.Left = 5.7F;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtAmount.Style = "font-size: 6pt; text-align: right; text-justify: auto; vertical-align: middle; wh" +
    "ite-space: inherit; ddo-char-set: 1; ddo-wrap-mode: inherit";
            this.txtAmount.Text = "98,765,432,100";
            this.txtAmount.Top = 0F;
            this.txtAmount.Width = 0.8000002F;
            // 
            // txtNote
            // 
            this.txtNote.DataField = "Note";
            this.txtNote.Height = 0.2F;
            this.txtNote.Left = 6.53F;
            this.txtNote.Name = "txtNote";
            this.txtNote.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtNote.Text = null;
            this.txtNote.Top = 0F;
            this.txtNote.Width = 1.5F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.DataField = "InvoiceCode";
            this.txtInvoiceCode.Height = 0.2F;
            this.txtInvoiceCode.Left = 9.93F;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtInvoiceCode.Text = null;
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 0.7000322F;
            // 
            // txtBranchName
            // 
            this.txtBranchName.DataField = "BranchName";
            this.txtBranchName.Height = 0.2F;
            this.txtBranchName.Left = 8.75F;
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtBranchName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtBranchName.Text = null;
            this.txtBranchName.Top = 0.2F;
            this.txtBranchName.Width = 0.68F;
            // 
            // txtBankCode
            // 
            this.txtBankCode.DataField = "BankCode";
            this.txtBankCode.Height = 0.2F;
            this.txtBankCode.Left = 8.030001F;
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBankCode.Text = null;
            this.txtBankCode.Top = 0F;
            this.txtBankCode.Width = 0.6999998F;
            // 
            // txtSlipNumber
            // 
            this.txtSlipNumber.DataField = "SlipNumber";
            this.txtSlipNumber.Height = 0.4F;
            this.txtSlipNumber.Left = 0.6F;
            this.txtSlipNumber.Name = "txtSlipNumber";
            this.txtSlipNumber.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtSlipNumber.Text = null;
            this.txtSlipNumber.Top = 0F;
            this.txtSlipNumber.Width = 0.6F;
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.DataField = "BranchCode";
            this.txtBranchCode.Height = 0.2F;
            this.txtBranchCode.Left = 8.73F;
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBranchCode.Text = null;
            this.txtBranchCode.Top = 0F;
            this.txtBranchCode.Width = 0.7F;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.DataField = "CurrencyCode";
            this.txtCurrencyCode.Height = 0.2F;
            this.txtCurrencyCode.Left = 9.93F;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCurrencyCode.Text = null;
            this.txtCurrencyCode.Top = 0.2F;
            this.txtCurrencyCode.Width = 0.7000322F;
            // 
            // txtDebitSubName
            // 
            this.txtDebitSubName.DataField = "DebitSubName";
            this.txtDebitSubName.Height = 0.2F;
            this.txtDebitSubName.Left = 2.72F;
            this.txtDebitSubName.Name = "txtDebitSubName";
            this.txtDebitSubName.OutputFormat = resources.GetString("txtDebitSubName.OutputFormat");
            this.txtDebitSubName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDebitSubName.Text = null;
            this.txtDebitSubName.Top = 0.2F;
            this.txtDebitSubName.Width = 0.73F;
            // 
            // txtDebitAccountTitleName
            // 
            this.txtDebitAccountTitleName.DataField = "DebitAccountTitleName";
            this.txtDebitAccountTitleName.Height = 0.2F;
            this.txtDebitAccountTitleName.Left = 1.97F;
            this.txtDebitAccountTitleName.Name = "txtDebitAccountTitleName";
            this.txtDebitAccountTitleName.OutputFormat = resources.GetString("txtDebitAccountTitleName.OutputFormat");
            this.txtDebitAccountTitleName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDebitAccountTitleName.Text = null;
            this.txtDebitAccountTitleName.Top = 0.2F;
            this.txtDebitAccountTitleName.Width = 0.73F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.DataField = "RecordedAt";
            this.txtRecordedAt.Height = 0.4F;
            this.txtRecordedAt.Left = 0F;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtRecordedAt.Text = null;
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.6F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.DataField = "CustomerCode";
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 6.5F;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 0.2F;
            this.txtCustomerCode.Width = 0.75F;
            // 
            // txtBankName
            // 
            this.txtBankName.DataField = "BankName";
            this.txtBankName.Height = 0.2F;
            this.txtBankName.Left = 8.047F;
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtBankName.Text = null;
            this.txtBankName.Top = 0.2F;
            this.txtBankName.Width = 0.68F;
            // 
            // lineDetailVerRecordedAt
            // 
            this.lineDetailVerRecordedAt.Height = 0.4F;
            this.lineDetailVerRecordedAt.Left = 0.6F;
            this.lineDetailVerRecordedAt.LineWeight = 1F;
            this.lineDetailVerRecordedAt.Name = "lineDetailVerRecordedAt";
            this.lineDetailVerRecordedAt.Top = 0F;
            this.lineDetailVerRecordedAt.Width = 0F;
            this.lineDetailVerRecordedAt.X1 = 0.6F;
            this.lineDetailVerRecordedAt.X2 = 0.6F;
            this.lineDetailVerRecordedAt.Y1 = 0F;
            this.lineDetailVerRecordedAt.Y2 = 0.4F;
            // 
            // txtDebitDepartmentCode
            // 
            this.txtDebitDepartmentCode.DataField = "DebitDepartmentCode";
            this.txtDebitDepartmentCode.Height = 0.2F;
            this.txtDebitDepartmentCode.Left = 1.2F;
            this.txtDebitDepartmentCode.Name = "txtDebitDepartmentCode";
            this.txtDebitDepartmentCode.OutputFormat = resources.GetString("txtDebitDepartmentCode.OutputFormat");
            this.txtDebitDepartmentCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDebitDepartmentCode.Text = null;
            this.txtDebitDepartmentCode.Top = 0F;
            this.txtDebitDepartmentCode.Width = 0.75F;
            // 
            // lineDetailVerSlipNumber
            // 
            this.lineDetailVerSlipNumber.Height = 0.4F;
            this.lineDetailVerSlipNumber.Left = 1.2F;
            this.lineDetailVerSlipNumber.LineWeight = 1F;
            this.lineDetailVerSlipNumber.Name = "lineDetailVerSlipNumber";
            this.lineDetailVerSlipNumber.Top = 0F;
            this.lineDetailVerSlipNumber.Width = 0F;
            this.lineDetailVerSlipNumber.X1 = 1.2F;
            this.lineDetailVerSlipNumber.X2 = 1.2F;
            this.lineDetailVerSlipNumber.Y1 = 0F;
            this.lineDetailVerSlipNumber.Y2 = 0.4F;
            // 
            // txtCreditDepartmentCode
            // 
            this.txtCreditDepartmentCode.DataField = "CreditDepartmentCode";
            this.txtCreditDepartmentCode.Height = 0.2F;
            this.txtCreditDepartmentCode.Left = 3.45F;
            this.txtCreditDepartmentCode.Name = "txtCreditDepartmentCode";
            this.txtCreditDepartmentCode.OutputFormat = resources.GetString("txtCreditDepartmentCode.OutputFormat");
            this.txtCreditDepartmentCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCreditDepartmentCode.Text = null;
            this.txtCreditDepartmentCode.Top = 0F;
            this.txtCreditDepartmentCode.Width = 0.75F;
            // 
            // txtCreditDepartmentName
            // 
            this.txtCreditDepartmentName.DataField = "CreditDepartmentName";
            this.txtCreditDepartmentName.Height = 0.2F;
            this.txtCreditDepartmentName.Left = 3.47F;
            this.txtCreditDepartmentName.Name = "txtCreditDepartmentName";
            this.txtCreditDepartmentName.OutputFormat = resources.GetString("txtCreditDepartmentName.OutputFormat");
            this.txtCreditDepartmentName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCreditDepartmentName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCreditDepartmentName.Text = null;
            this.txtCreditDepartmentName.Top = 0.2F;
            this.txtCreditDepartmentName.Width = 0.73F;
            // 
            // txtCreditAccountTitleName
            // 
            this.txtCreditAccountTitleName.DataField = "CreditAccountTitleName";
            this.txtCreditAccountTitleName.Height = 0.2F;
            this.txtCreditAccountTitleName.Left = 4.216F;
            this.txtCreditAccountTitleName.Name = "txtCreditAccountTitleName";
            this.txtCreditAccountTitleName.OutputFormat = resources.GetString("txtCreditAccountTitleName.OutputFormat");
            this.txtCreditAccountTitleName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCreditAccountTitleName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCreditAccountTitleName.Text = null;
            this.txtCreditAccountTitleName.Top = 0.2F;
            this.txtCreditAccountTitleName.Width = 0.73F;
            // 
            // txtCreditSubName
            // 
            this.txtCreditSubName.DataField = "CreditSubName";
            this.txtCreditSubName.Height = 0.2F;
            this.txtCreditSubName.Left = 4.966F;
            this.txtCreditSubName.Name = "txtCreditSubName";
            this.txtCreditSubName.OutputFormat = resources.GetString("txtCreditSubName.OutputFormat");
            this.txtCreditSubName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCreditSubName.Text = null;
            this.txtCreditSubName.Top = 0.2F;
            this.txtCreditSubName.Width = 0.73F;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.DataField = "AccountNumber";
            this.txtAccountNumber.Height = 0.4F;
            this.txtAccountNumber.Left = 9.43F;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Style = "font-size: 6pt; text-align: center; text-justify: auto; vertical-align: middle; w" +
    "hite-space: nowrap; ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtAccountNumber.Text = null;
            this.txtAccountNumber.Top = 0F;
            this.txtAccountNumber.Width = 0.5F;
            // 
            // lineDetailVerDebit
            // 
            this.lineDetailVerDebit.Height = 0.4F;
            this.lineDetailVerDebit.Left = 3.45F;
            this.lineDetailVerDebit.LineWeight = 1F;
            this.lineDetailVerDebit.Name = "lineDetailVerDebit";
            this.lineDetailVerDebit.Top = 0F;
            this.lineDetailVerDebit.Width = 0F;
            this.lineDetailVerDebit.X1 = 3.45F;
            this.lineDetailVerDebit.X2 = 3.45F;
            this.lineDetailVerDebit.Y1 = 0F;
            this.lineDetailVerDebit.Y2 = 0.4F;
            // 
            // lineDetailVerCredit
            // 
            this.lineDetailVerCredit.Height = 0.4F;
            this.lineDetailVerCredit.Left = 5.7F;
            this.lineDetailVerCredit.LineWeight = 1F;
            this.lineDetailVerCredit.Name = "lineDetailVerCredit";
            this.lineDetailVerCredit.Top = -3.552714E-15F;
            this.lineDetailVerCredit.Width = 0F;
            this.lineDetailVerCredit.X1 = 5.7F;
            this.lineDetailVerCredit.X2 = 5.7F;
            this.lineDetailVerCredit.Y1 = -3.552714E-15F;
            this.lineDetailVerCredit.Y2 = 0.4F;
            // 
            // lineDetailVerAmount
            // 
            this.lineDetailVerAmount.Height = 0.4F;
            this.lineDetailVerAmount.Left = 6.5F;
            this.lineDetailVerAmount.LineWeight = 1F;
            this.lineDetailVerAmount.Name = "lineDetailVerAmount";
            this.lineDetailVerAmount.Top = -3.552714E-15F;
            this.lineDetailVerAmount.Width = 0F;
            this.lineDetailVerAmount.X1 = 6.5F;
            this.lineDetailVerAmount.X2 = 6.5F;
            this.lineDetailVerAmount.Y1 = -3.552714E-15F;
            this.lineDetailVerAmount.Y2 = 0.4F;
            // 
            // lineDetailVerBankCode
            // 
            this.lineDetailVerBankCode.Height = 0.4F;
            this.lineDetailVerBankCode.Left = 8.727F;
            this.lineDetailVerBankCode.LineWeight = 1F;
            this.lineDetailVerBankCode.Name = "lineDetailVerBankCode";
            this.lineDetailVerBankCode.Top = -3.552714E-15F;
            this.lineDetailVerBankCode.Width = 0F;
            this.lineDetailVerBankCode.X1 = 8.727F;
            this.lineDetailVerBankCode.X2 = 8.727F;
            this.lineDetailVerBankCode.Y1 = -3.552714E-15F;
            this.lineDetailVerBankCode.Y2 = 0.4F;
            // 
            // lineDetailVerBranchCode
            // 
            this.lineDetailVerBranchCode.Height = 0.4F;
            this.lineDetailVerBranchCode.Left = 9.43F;
            this.lineDetailVerBranchCode.LineWeight = 1F;
            this.lineDetailVerBranchCode.Name = "lineDetailVerBranchCode";
            this.lineDetailVerBranchCode.Top = -3.552714E-15F;
            this.lineDetailVerBranchCode.Width = 0F;
            this.lineDetailVerBranchCode.X1 = 9.43F;
            this.lineDetailVerBranchCode.X2 = 9.43F;
            this.lineDetailVerBranchCode.Y1 = -3.552714E-15F;
            this.lineDetailVerBranchCode.Y2 = 0.4F;
            // 
            // lineDetailVerAccountNumber
            // 
            this.lineDetailVerAccountNumber.Height = 0.4F;
            this.lineDetailVerAccountNumber.Left = 9.93F;
            this.lineDetailVerAccountNumber.LineWeight = 1F;
            this.lineDetailVerAccountNumber.Name = "lineDetailVerAccountNumber";
            this.lineDetailVerAccountNumber.Top = 0F;
            this.lineDetailVerAccountNumber.Width = 0F;
            this.lineDetailVerAccountNumber.X1 = 9.93F;
            this.lineDetailVerAccountNumber.X2 = 9.93F;
            this.lineDetailVerAccountNumber.Y1 = 0F;
            this.lineDetailVerAccountNumber.Y2 = 0.4F;
            // 
            // txtDebitAccountTitleCode
            // 
            this.txtDebitAccountTitleCode.DataField = "DebitAccountTitleCode";
            this.txtDebitAccountTitleCode.Height = 0.2F;
            this.txtDebitAccountTitleCode.Left = 1.95F;
            this.txtDebitAccountTitleCode.Name = "txtDebitAccountTitleCode";
            this.txtDebitAccountTitleCode.OutputFormat = resources.GetString("txtDebitAccountTitleCode.OutputFormat");
            this.txtDebitAccountTitleCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDebitAccountTitleCode.Text = null;
            this.txtDebitAccountTitleCode.Top = 0F;
            this.txtDebitAccountTitleCode.Width = 0.75F;
            // 
            // txtCreditAccountTitleCode
            // 
            this.txtCreditAccountTitleCode.DataField = "CreditAccountTitleCode";
            this.txtCreditAccountTitleCode.Height = 0.2F;
            this.txtCreditAccountTitleCode.Left = 4.2F;
            this.txtCreditAccountTitleCode.Name = "txtCreditAccountTitleCode";
            this.txtCreditAccountTitleCode.OutputFormat = resources.GetString("txtCreditAccountTitleCode.OutputFormat");
            this.txtCreditAccountTitleCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCreditAccountTitleCode.Text = null;
            this.txtCreditAccountTitleCode.Top = 0F;
            this.txtCreditAccountTitleCode.Width = 0.7497559F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.DataField = "CustomerName";
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 7.28F;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtCustomerName.Text = null;
            this.txtCustomerName.Top = 0.2F;
            this.txtCustomerName.Width = 0.75F;
            // 
            // lineDetailVerCreditDepartment
            // 
            this.lineDetailVerCreditDepartment.Height = 0.4F;
            this.lineDetailVerCreditDepartment.Left = 4.2F;
            this.lineDetailVerCreditDepartment.LineWeight = 1F;
            this.lineDetailVerCreditDepartment.Name = "lineDetailVerCreditDepartment";
            this.lineDetailVerCreditDepartment.Top = -3.552714E-15F;
            this.lineDetailVerCreditDepartment.Width = 0F;
            this.lineDetailVerCreditDepartment.X1 = 4.2F;
            this.lineDetailVerCreditDepartment.X2 = 4.2F;
            this.lineDetailVerCreditDepartment.Y1 = -3.552714E-15F;
            this.lineDetailVerCreditDepartment.Y2 = 0.4F;
            // 
            // lineDetailVerCreditAccountTitle
            // 
            this.lineDetailVerCreditAccountTitle.Height = 0.4F;
            this.lineDetailVerCreditAccountTitle.Left = 4.95F;
            this.lineDetailVerCreditAccountTitle.LineWeight = 1F;
            this.lineDetailVerCreditAccountTitle.Name = "lineDetailVerCreditAccountTitle";
            this.lineDetailVerCreditAccountTitle.Top = 0F;
            this.lineDetailVerCreditAccountTitle.Width = 0F;
            this.lineDetailVerCreditAccountTitle.X1 = 4.95F;
            this.lineDetailVerCreditAccountTitle.X2 = 4.95F;
            this.lineDetailVerCreditAccountTitle.Y1 = 0F;
            this.lineDetailVerCreditAccountTitle.Y2 = 0.4F;
            // 
            // lineDetailVerNote
            // 
            this.lineDetailVerNote.Height = 0.4F;
            this.lineDetailVerNote.Left = 8.030001F;
            this.lineDetailVerNote.LineWeight = 1F;
            this.lineDetailVerNote.Name = "lineDetailVerNote";
            this.lineDetailVerNote.Top = 0F;
            this.lineDetailVerNote.Width = 0F;
            this.lineDetailVerNote.X1 = 8.030001F;
            this.lineDetailVerNote.X2 = 8.030001F;
            this.lineDetailVerNote.Y1 = 0F;
            this.lineDetailVerNote.Y2 = 0.4F;
            // 
            // lineDetailVerDebitDepartment
            // 
            this.lineDetailVerDebitDepartment.Height = 0.4F;
            this.lineDetailVerDebitDepartment.Left = 1.95F;
            this.lineDetailVerDebitDepartment.LineWeight = 1F;
            this.lineDetailVerDebitDepartment.Name = "lineDetailVerDebitDepartment";
            this.lineDetailVerDebitDepartment.Top = -3.552714E-15F;
            this.lineDetailVerDebitDepartment.Width = 0F;
            this.lineDetailVerDebitDepartment.X1 = 1.95F;
            this.lineDetailVerDebitDepartment.X2 = 1.95F;
            this.lineDetailVerDebitDepartment.Y1 = -3.552714E-15F;
            this.lineDetailVerDebitDepartment.Y2 = 0.4F;
            // 
            // lineDetailVerDebitAccountTitle
            // 
            this.lineDetailVerDebitAccountTitle.Height = 0.4F;
            this.lineDetailVerDebitAccountTitle.Left = 2.7F;
            this.lineDetailVerDebitAccountTitle.LineWeight = 1F;
            this.lineDetailVerDebitAccountTitle.Name = "lineDetailVerDebitAccountTitle";
            this.lineDetailVerDebitAccountTitle.Top = 0F;
            this.lineDetailVerDebitAccountTitle.Width = 0F;
            this.lineDetailVerDebitAccountTitle.X1 = 2.7F;
            this.lineDetailVerDebitAccountTitle.X2 = 2.7F;
            this.lineDetailVerDebitAccountTitle.Y1 = 0F;
            this.lineDetailVerDebitAccountTitle.Y2 = 0.4F;
            // 
            // line14
            // 
            this.line14.Height = 0F;
            this.line14.Left = 0F;
            this.line14.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line14.LineWeight = 1F;
            this.line14.Name = "line14";
            this.line14.Top = 0.4F;
            this.line14.Width = 10.62992F;
            this.line14.X1 = 0F;
            this.line14.X2 = 10.62992F;
            this.line14.Y1 = 0.4F;
            this.line14.Y2 = 0.4F;
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
            this.txtTotalAmount,
            this.lblTotal,
            this.lineFooterVerTotal,
            this.txtTotalCount,
            this.lblSpace,
            this.lineFooterHorLower,
            this.lineFooterVerTotalAmount,
            this.lineFooterVerSpace});
            this.groupFooter1.Height = 0.3080381F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.DataField = "Amount";
            this.txtTotalAmount.Height = 0.3F;
            this.txtTotalAmount.Left = 5.7F;
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.OutputFormat = resources.GetString("txtTotalAmount.OutputFormat");
            this.txtTotalAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtTotalAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtTotalAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalAmount.Text = "98,765,432,100";
            this.txtTotalAmount.Top = 0F;
            this.txtTotalAmount.Width = 0.8F;
            // 
            // lblTotal
            // 
            this.lblTotal.Height = 0.3F;
            this.lblTotal.HyperLink = null;
            this.lblTotal.Left = 0F;
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblTotal.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblTotal.Text = " 合計";
            this.lblTotal.Top = 0F;
            this.lblTotal.Width = 5.7F;
            // 
            // lineFooterVerTotal
            // 
            this.lineFooterVerTotal.Height = 0.3F;
            this.lineFooterVerTotal.Left = 5.7F;
            this.lineFooterVerTotal.LineWeight = 1F;
            this.lineFooterVerTotal.Name = "lineFooterVerTotal";
            this.lineFooterVerTotal.Top = 0F;
            this.lineFooterVerTotal.Width = 0F;
            this.lineFooterVerTotal.X1 = 5.7F;
            this.lineFooterVerTotal.X2 = 5.7F;
            this.lineFooterVerTotal.Y1 = 0F;
            this.lineFooterVerTotal.Y2 = 0.3F;
            // 
            // txtTotalCount
            // 
            this.txtTotalCount.Height = 0.3F;
            this.txtTotalCount.Left = 9.93F;
            this.txtTotalCount.Name = "txtTotalCount";
            this.txtTotalCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtTotalCount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtTotalCount.Text = null;
            this.txtTotalCount.Top = 0F;
            this.txtTotalCount.Width = 0.7274714F;
            // 
            // lblSpace
            // 
            this.lblSpace.Height = 0.3F;
            this.lblSpace.HyperLink = null;
            this.lblSpace.Left = 6.5F;
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Style = "background-color: WhiteSmoke; font-size: 7.5pt; ddo-char-set: 1";
            this.lblSpace.Text = "";
            this.lblSpace.Top = 0F;
            this.lblSpace.Width = 3.43F;
            // 
            // lineFooterHorLower
            // 
            this.lineFooterHorLower.Height = 0F;
            this.lineFooterHorLower.Left = 0F;
            this.lineFooterHorLower.LineWeight = 1F;
            this.lineFooterHorLower.Name = "lineFooterHorLower";
            this.lineFooterHorLower.Top = 0.3F;
            this.lineFooterHorLower.Width = 10.62992F;
            this.lineFooterHorLower.X1 = 0F;
            this.lineFooterHorLower.X2 = 10.62992F;
            this.lineFooterHorLower.Y1 = 0.3F;
            this.lineFooterHorLower.Y2 = 0.3F;
            // 
            // lineFooterVerTotalAmount
            // 
            this.lineFooterVerTotalAmount.Height = 0.3F;
            this.lineFooterVerTotalAmount.Left = 6.5F;
            this.lineFooterVerTotalAmount.LineWeight = 1F;
            this.lineFooterVerTotalAmount.Name = "lineFooterVerTotalAmount";
            this.lineFooterVerTotalAmount.Top = 0F;
            this.lineFooterVerTotalAmount.Width = 0F;
            this.lineFooterVerTotalAmount.X1 = 6.5F;
            this.lineFooterVerTotalAmount.X2 = 6.5F;
            this.lineFooterVerTotalAmount.Y1 = 0F;
            this.lineFooterVerTotalAmount.Y2 = 0.3F;
            // 
            // lineFooterVerSpace
            // 
            this.lineFooterVerSpace.Height = 0.3F;
            this.lineFooterVerSpace.Left = 9.93F;
            this.lineFooterVerSpace.LineWeight = 1F;
            this.lineFooterVerSpace.Name = "lineFooterVerSpace";
            this.lineFooterVerSpace.Top = 0F;
            this.lineFooterVerSpace.Width = 0F;
            this.lineFooterVerSpace.X1 = 9.93F;
            this.lineFooterVerSpace.X2 = 9.93F;
            this.lineFooterVerSpace.Y1 = 0F;
            this.lineFooterVerSpace.Y2 = 0.3F;
            // 
            // MatchingJournalizingReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.63045F;
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
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSlipNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlipNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebit;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line line14;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitAccountTitleName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalCount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebitAccountTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditAccountTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitSubName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditAccountTitleName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditSubName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditAccountTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitAccountTitle;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotalAmount;
    }
}
