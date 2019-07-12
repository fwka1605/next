namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ReceiptJournalizingOutputSectionReport の概要の説明です。
    /// </summary>
    partial class ReceiptJournalizingSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReceiptJournalizingSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBranckCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBankCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebitDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditSubCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebitSubCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCredit = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDebit = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBankName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBranchName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblDebitAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorDebit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerBankCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblPayerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDebitAccountTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditAccountTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCredit = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditSubCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitAccountTitleName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitSubCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitAccountTitleCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitSubName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditAccountTitleName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditSubName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPayerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitAccountTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDebitSubCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditAccountTitle = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditSubCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSlipNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerNote = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBankCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTotalCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerSpace = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranckCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSlipNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlipNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblAccountNumber,
            this.lblBranckCode,
            this.lblBankCode,
            this.lblNote,
            this.lblDebitDepartmentCode,
            this.lblCreditSubCode,
            this.lblCreditAccountTitleCode,
            this.lblCreditDepartmentCode,
            this.lblDebitSubCode,
            this.lblCredit,
            this.lblDebit,
            this.lblSlipNumber,
            this.lblRecordedAt,
            this.lblBankName,
            this.lblCurrencyCode,
            this.lblBranchName,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lineHeaderVerRecordedAt,
            this.lineHeaderVerSlipNumber,
            this.lblDebitAccountTitleCode,
            this.lineHeaderHorDebit,
            this.lineHeaderVerDebit,
            this.lineHeaderVerDebitDepartment,
            this.lblAmount,
            this.lineHeaderVerBankCode,
            this.lblPayerName,
            this.lineHeaderVerNote,
            this.lineHeaderVerBranchCode,
            this.lineHeaderHorNote,
            this.lineHeaderVerDebitAccountTitle,
            this.lineHeaderVerCreditAccountTitle,
            this.lineHeaderVerCreditDepartment,
            this.lineHeaderVerCredit,
            this.lineHeaderVerAmount,
            this.lblDueAt,
            this.lineHeaderVerAccountNumber,
            this.lineHeaderHorUpper,
            this.lineHeaderHorDueAt,
            this.lineHeaderHorLower});
            this.pageHeader.Height = 1.007644F;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.BeforePrint += new System.EventHandler(this.pageHeader_BeforePrint);
            // 
            // lblAccountNumber
            // 
            this.lblAccountNumber.Height = 0.4F;
            this.lblAccountNumber.HyperLink = null;
            this.lblAccountNumber.Left = 9.400001F;
            this.lblAccountNumber.Name = "lblAccountNumber";
            this.lblAccountNumber.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAccountNumber.Text = "口座番号";
            this.lblAccountNumber.Top = 0.6F;
            this.lblAccountNumber.Width = 0.5F;
            // 
            // lblBranckCode
            // 
            this.lblBranckCode.Height = 0.2F;
            this.lblBranckCode.HyperLink = null;
            this.lblBranckCode.Left = 8.6F;
            this.lblBranckCode.Name = "lblBranckCode";
            this.lblBranckCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBranckCode.Text = "支店コード";
            this.lblBranckCode.Top = 0.6F;
            this.lblBranckCode.Width = 0.8F;
            // 
            // lblBankCode
            // 
            this.lblBankCode.Height = 0.2F;
            this.lblBankCode.HyperLink = null;
            this.lblBankCode.Left = 7.8F;
            this.lblBankCode.Name = "lblBankCode";
            this.lblBankCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBankCode.Text = "銀行コード";
            this.lblBankCode.Top = 0.6F;
            this.lblBankCode.Width = 0.8F;
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
            this.lblNote.Width = 1.3F;
            // 
            // lblDebitDepartmentCode
            // 
            this.lblDebitDepartmentCode.Height = 0.2F;
            this.lblDebitDepartmentCode.HyperLink = null;
            this.lblDebitDepartmentCode.Left = 1.2F;
            this.lblDebitDepartmentCode.Name = "lblDebitDepartmentCode";
            this.lblDebitDepartmentCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebitDepartmentCode.Text = "部門コード";
            this.lblDebitDepartmentCode.Top = 0.8F;
            this.lblDebitDepartmentCode.Width = 0.75F;
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
            // lblCreditDepartmentCode
            // 
            this.lblCreditDepartmentCode.Height = 0.2F;
            this.lblCreditDepartmentCode.HyperLink = null;
            this.lblCreditDepartmentCode.Left = 3.45F;
            this.lblCreditDepartmentCode.Name = "lblCreditDepartmentCode";
            this.lblCreditDepartmentCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCreditDepartmentCode.Text = "部門コード";
            this.lblCreditDepartmentCode.Top = 0.8F;
            this.lblCreditDepartmentCode.Width = 0.75F;
            // 
            // lblDebitSubCode
            // 
            this.lblDebitSubCode.Height = 0.2F;
            this.lblDebitSubCode.HyperLink = null;
            this.lblDebitSubCode.Left = 2.7F;
            this.lblDebitSubCode.Name = "lblDebitSubCode";
            this.lblDebitSubCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDebitSubCode.Text = "補助コード";
            this.lblDebitSubCode.Top = 0.8F;
            this.lblDebitSubCode.Width = 0.75F;
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
            // lblBankName
            // 
            this.lblBankName.Height = 0.2F;
            this.lblBankName.HyperLink = null;
            this.lblBankName.Left = 7.8F;
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBankName.Text = "銀行名";
            this.lblBankName.Top = 0.8F;
            this.lblBankName.Width = 0.8F;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Height = 0.2F;
            this.lblCurrencyCode.HyperLink = null;
            this.lblCurrencyCode.Left = 9.900001F;
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.Top = 0.8F;
            this.lblCurrencyCode.Width = 0.7F;
            // 
            // lblBranchName
            // 
            this.lblBranchName.Height = 0.2F;
            this.lblBranchName.HyperLink = null;
            this.lblBranchName.Left = 8.6F;
            this.lblBranchName.Name = "lblBranchName";
            this.lblBranchName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBranchName.Text = "支店名";
            this.lblBranchName.Top = 0.8F;
            this.lblBranchName.Width = 0.8F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.02440945F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCode.Text = "会社コード　：";
            this.lblCompanyCode.Top = 3.72529E-09F;
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
            this.lblCompanyCodeName.Top = 3.72529E-09F;
            this.lblCompanyCodeName.Width = 3.657F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 8.809055F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDate.Text = "出力日付：";
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
            this.lblTitle.Height = 0.231F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "入金仕訳出力";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.6F;
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
            // lblAmount
            // 
            this.lblAmount.Height = 0.4F;
            this.lblAmount.HyperLink = null;
            this.lblAmount.Left = 5.7F;
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAmount.Text = "仕訳金額";
            this.lblAmount.Top = 0.6F;
            this.lblAmount.Width = 0.8F;
            // 
            // lineHeaderVerBankCode
            // 
            this.lineHeaderVerBankCode.Height = 0.4F;
            this.lineHeaderVerBankCode.Left = 8.6F;
            this.lineHeaderVerBankCode.LineWeight = 1F;
            this.lineHeaderVerBankCode.Name = "lineHeaderVerBankCode";
            this.lineHeaderVerBankCode.Top = 0.6F;
            this.lineHeaderVerBankCode.Width = 0F;
            this.lineHeaderVerBankCode.X1 = 8.6F;
            this.lineHeaderVerBankCode.X2 = 8.6F;
            this.lineHeaderVerBankCode.Y1 = 0.6F;
            this.lineHeaderVerBankCode.Y2 = 1F;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Height = 0.2F;
            this.lblPayerName.HyperLink = null;
            this.lblPayerName.Left = 6.5F;
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.Top = 0.8F;
            this.lblPayerName.Width = 1.3F;
            // 
            // lineHeaderVerNote
            // 
            this.lineHeaderVerNote.Height = 0.4F;
            this.lineHeaderVerNote.Left = 7.8F;
            this.lineHeaderVerNote.LineWeight = 1F;
            this.lineHeaderVerNote.Name = "lineHeaderVerNote";
            this.lineHeaderVerNote.Top = 0.6F;
            this.lineHeaderVerNote.Width = 0F;
            this.lineHeaderVerNote.X1 = 7.8F;
            this.lineHeaderVerNote.X2 = 7.8F;
            this.lineHeaderVerNote.Y1 = 0.6F;
            this.lineHeaderVerNote.Y2 = 1F;
            // 
            // lineHeaderVerBranchCode
            // 
            this.lineHeaderVerBranchCode.Height = 0.4F;
            this.lineHeaderVerBranchCode.Left = 9.400001F;
            this.lineHeaderVerBranchCode.LineWeight = 1F;
            this.lineHeaderVerBranchCode.Name = "lineHeaderVerBranchCode";
            this.lineHeaderVerBranchCode.Top = 0.6F;
            this.lineHeaderVerBranchCode.Width = 0F;
            this.lineHeaderVerBranchCode.X1 = 9.400001F;
            this.lineHeaderVerBranchCode.X2 = 9.400001F;
            this.lineHeaderVerBranchCode.Y1 = 0.6F;
            this.lineHeaderVerBranchCode.Y2 = 1F;
            // 
            // lineHeaderHorNote
            // 
            this.lineHeaderHorNote.Height = 0F;
            this.lineHeaderHorNote.Left = 6.5F;
            this.lineHeaderHorNote.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorNote.LineWeight = 1F;
            this.lineHeaderHorNote.Name = "lineHeaderHorNote";
            this.lineHeaderHorNote.Top = 0.8F;
            this.lineHeaderHorNote.Width = 2.900001F;
            this.lineHeaderHorNote.X1 = 6.5F;
            this.lineHeaderHorNote.X2 = 9.400001F;
            this.lineHeaderHorNote.Y1 = 0.8F;
            this.lineHeaderHorNote.Y2 = 0.8F;
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
            // lineHeaderVerCreditAccountTitle
            // 
            this.lineHeaderVerCreditAccountTitle.Height = 0.2F;
            this.lineHeaderVerCreditAccountTitle.Left = 4.95F;
            this.lineHeaderVerCreditAccountTitle.LineWeight = 1F;
            this.lineHeaderVerCreditAccountTitle.Name = "lineHeaderVerCreditAccountTitle";
            this.lineHeaderVerCreditAccountTitle.Top = 0.8F;
            this.lineHeaderVerCreditAccountTitle.Width = 0F;
            this.lineHeaderVerCreditAccountTitle.X1 = 4.95F;
            this.lineHeaderVerCreditAccountTitle.X2 = 4.95F;
            this.lineHeaderVerCreditAccountTitle.Y1 = 0.8F;
            this.lineHeaderVerCreditAccountTitle.Y2 = 1F;
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
            // lblDueAt
            // 
            this.lblDueAt.Height = 0.2F;
            this.lblDueAt.HyperLink = null;
            this.lblDueAt.Left = 9.900001F;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDueAt.Text = "期日";
            this.lblDueAt.Top = 0.6F;
            this.lblDueAt.Width = 0.7F;
            // 
            // lineHeaderVerAccountNumber
            // 
            this.lineHeaderVerAccountNumber.Height = 0.4F;
            this.lineHeaderVerAccountNumber.Left = 9.900001F;
            this.lineHeaderVerAccountNumber.LineWeight = 1F;
            this.lineHeaderVerAccountNumber.Name = "lineHeaderVerAccountNumber";
            this.lineHeaderVerAccountNumber.Top = 0.6F;
            this.lineHeaderVerAccountNumber.Width = 0F;
            this.lineHeaderVerAccountNumber.X1 = 9.900001F;
            this.lineHeaderVerAccountNumber.X2 = 9.900001F;
            this.lineHeaderVerAccountNumber.Y1 = 0.6F;
            this.lineHeaderVerAccountNumber.Y2 = 1F;
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
            // lineHeaderHorDueAt
            // 
            this.lineHeaderHorDueAt.Height = 0F;
            this.lineHeaderHorDueAt.Left = 9.900001F;
            this.lineHeaderHorDueAt.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorDueAt.LineWeight = 1F;
            this.lineHeaderHorDueAt.Name = "lineHeaderHorDueAt";
            this.lineHeaderHorDueAt.Top = 0.8F;
            this.lineHeaderHorDueAt.Width = 0.6999998F;
            this.lineHeaderHorDueAt.X1 = 9.900001F;
            this.lineHeaderHorDueAt.X2 = 10.6F;
            this.lineHeaderHorDueAt.Y1 = 0.8F;
            this.lineHeaderHorDueAt.Y2 = 0.8F;
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
            this.txtAmount,
            this.txtRecordedAt,
            this.txtSlipNumber,
            this.txtCreditSubCode,
            this.txtCreditAccountTitleCode,
            this.txtCreditDepartmentCode,
            this.txtDebitAccountTitleName,
            this.txtDebitSubCode,
            this.txtDebitAccountTitleCode,
            this.txtDebitSubName,
            this.txtCreditDepartmentName,
            this.txtCreditAccountTitleName,
            this.txtCreditSubName,
            this.txtNote,
            this.txtPayerName,
            this.txtBankCode,
            this.txtBankName,
            this.txtBranchCode,
            this.txtBranchName,
            this.txtAccountNumber,
            this.txtDueAt,
            this.txtCurrencyCode,
            this.txtDebitDepartmentName,
            this.txtDebitDepartmentCode,
            this.lineDetailHorLower,
            this.lineDetailVerDebitDepartment,
            this.lineDetailVerDebitAccountTitle,
            this.lineDetailVerDebitSubCode,
            this.lineDetailVerCreditAccountTitle,
            this.lineDetailVerCreditDepartment,
            this.lineDetailVerCreditSubCode,
            this.lineDetailVerRecordedAt,
            this.lineDetailVerSlipNumber,
            this.lineDetailVerAmount,
            this.lineDetailVerNote,
            this.lineDetailVerBankCode,
            this.lineDetailVerBranchCode,
            this.lineDetailVerAccountNumber});
            this.detail.Height = 0.4077592F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtAmount
            // 
            this.txtAmount.Height = 0.4F;
            this.txtAmount.Left = 5.7F;
            this.txtAmount.MultiLine = false;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1; ddo-s" +
    "hrink-to-fit: none";
            this.txtAmount.Text = "98,765,432,100";
            this.txtAmount.Top = 0F;
            this.txtAmount.Width = 0.8000002F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.4F;
            this.txtRecordedAt.Left = 0F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtRecordedAt.Text = "txtRecordedAt";
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.6F;
            // 
            // txtSlipNumber
            // 
            this.txtSlipNumber.Height = 0.4F;
            this.txtSlipNumber.Left = 0.6F;
            this.txtSlipNumber.MultiLine = false;
            this.txtSlipNumber.Name = "txtSlipNumber";
            this.txtSlipNumber.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtSlipNumber.Text = "txtSlipNumber";
            this.txtSlipNumber.Top = 0F;
            this.txtSlipNumber.Width = 0.6F;
            // 
            // txtCreditSubCode
            // 
            this.txtCreditSubCode.Height = 0.2F;
            this.txtCreditSubCode.Left = 4.95F;
            this.txtCreditSubCode.MultiLine = false;
            this.txtCreditSubCode.Name = "txtCreditSubCode";
            this.txtCreditSubCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCreditSubCode.Text = "txtCreditSubCode";
            this.txtCreditSubCode.Top = 0F;
            this.txtCreditSubCode.Width = 0.75F;
            // 
            // txtCreditAccountTitleCode
            // 
            this.txtCreditAccountTitleCode.Height = 0.2F;
            this.txtCreditAccountTitleCode.Left = 4.2F;
            this.txtCreditAccountTitleCode.MultiLine = false;
            this.txtCreditAccountTitleCode.Name = "txtCreditAccountTitleCode";
            this.txtCreditAccountTitleCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCreditAccountTitleCode.Text = "txtCreditAccountTitleCode";
            this.txtCreditAccountTitleCode.Top = 0F;
            this.txtCreditAccountTitleCode.Width = 0.75F;
            // 
            // txtCreditDepartmentCode
            // 
            this.txtCreditDepartmentCode.Height = 0.2F;
            this.txtCreditDepartmentCode.Left = 3.45F;
            this.txtCreditDepartmentCode.MultiLine = false;
            this.txtCreditDepartmentCode.Name = "txtCreditDepartmentCode";
            this.txtCreditDepartmentCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCreditDepartmentCode.Text = "txtCreditDepartmentCode";
            this.txtCreditDepartmentCode.Top = 0F;
            this.txtCreditDepartmentCode.Width = 0.75F;
            // 
            // txtDebitAccountTitleName
            // 
            this.txtDebitAccountTitleName.Height = 0.2F;
            this.txtDebitAccountTitleName.Left = 1.97F;
            this.txtDebitAccountTitleName.MultiLine = false;
            this.txtDebitAccountTitleName.Name = "txtDebitAccountTitleName";
            this.txtDebitAccountTitleName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtDebitAccountTitleName.Text = "txtDebitAccountTitleName";
            this.txtDebitAccountTitleName.Top = 0.2F;
            this.txtDebitAccountTitleName.Width = 0.73F;
            // 
            // txtDebitSubCode
            // 
            this.txtDebitSubCode.Height = 0.2F;
            this.txtDebitSubCode.Left = 2.7F;
            this.txtDebitSubCode.MultiLine = false;
            this.txtDebitSubCode.Name = "txtDebitSubCode";
            this.txtDebitSubCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtDebitSubCode.Text = "txtDebitSubCode";
            this.txtDebitSubCode.Top = 0F;
            this.txtDebitSubCode.Width = 0.75F;
            // 
            // txtDebitAccountTitleCode
            // 
            this.txtDebitAccountTitleCode.Height = 0.2F;
            this.txtDebitAccountTitleCode.Left = 1.95F;
            this.txtDebitAccountTitleCode.MultiLine = false;
            this.txtDebitAccountTitleCode.Name = "txtDebitAccountTitleCode";
            this.txtDebitAccountTitleCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtDebitAccountTitleCode.Text = "txtDebitAccountTitleCode";
            this.txtDebitAccountTitleCode.Top = 0F;
            this.txtDebitAccountTitleCode.Width = 0.75F;
            // 
            // txtDebitSubName
            // 
            this.txtDebitSubName.Height = 0.2F;
            this.txtDebitSubName.Left = 2.72F;
            this.txtDebitSubName.MultiLine = false;
            this.txtDebitSubName.Name = "txtDebitSubName";
            this.txtDebitSubName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtDebitSubName.Text = "txtDebitSubName";
            this.txtDebitSubName.Top = 0.2F;
            this.txtDebitSubName.Width = 0.73F;
            // 
            // txtCreditDepartmentName
            // 
            this.txtCreditDepartmentName.Height = 0.2F;
            this.txtCreditDepartmentName.Left = 3.47F;
            this.txtCreditDepartmentName.MultiLine = false;
            this.txtCreditDepartmentName.Name = "txtCreditDepartmentName";
            this.txtCreditDepartmentName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtCreditDepartmentName.Text = "txtCreditDepartmentName";
            this.txtCreditDepartmentName.Top = 0.2F;
            this.txtCreditDepartmentName.Width = 0.73F;
            // 
            // txtCreditAccountTitleName
            // 
            this.txtCreditAccountTitleName.Height = 0.2F;
            this.txtCreditAccountTitleName.Left = 4.22F;
            this.txtCreditAccountTitleName.MultiLine = false;
            this.txtCreditAccountTitleName.Name = "txtCreditAccountTitleName";
            this.txtCreditAccountTitleName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtCreditAccountTitleName.Text = "txtCreditAccountTitleName";
            this.txtCreditAccountTitleName.Top = 0.2F;
            this.txtCreditAccountTitleName.Width = 0.73F;
            // 
            // txtCreditSubName
            // 
            this.txtCreditSubName.Height = 0.2F;
            this.txtCreditSubName.Left = 4.97F;
            this.txtCreditSubName.MultiLine = false;
            this.txtCreditSubName.Name = "txtCreditSubName";
            this.txtCreditSubName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtCreditSubName.Text = "txtCreditSubName";
            this.txtCreditSubName.Top = 0.2F;
            this.txtCreditSubName.Width = 0.73F;
            // 
            // txtNote
            // 
            this.txtNote.Height = 0.2F;
            this.txtNote.Left = 6.52F;
            this.txtNote.MultiLine = false;
            this.txtNote.Name = "txtNote";
            this.txtNote.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtNote.Text = "txtNote";
            this.txtNote.Top = 0F;
            this.txtNote.Width = 1.28F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.2F;
            this.txtPayerName.Left = 6.52F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtPayerName.Text = "txtPayerName";
            this.txtPayerName.Top = 0.2F;
            this.txtPayerName.Width = 1.28F;
            // 
            // txtBankCode
            // 
            this.txtBankCode.Height = 0.2F;
            this.txtBankCode.Left = 7.8F;
            this.txtBankCode.MultiLine = false;
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBankCode.Text = "txtBankCode";
            this.txtBankCode.Top = 0F;
            this.txtBankCode.Width = 0.8000002F;
            // 
            // txtBankName
            // 
            this.txtBankName.Height = 0.2F;
            this.txtBankName.Left = 7.8F;
            this.txtBankName.MultiLine = false;
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtBankName.Text = "txtBankName";
            this.txtBankName.Top = 0.2F;
            this.txtBankName.Width = 0.8000002F;
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.Height = 0.2F;
            this.txtBranchCode.Left = 8.6F;
            this.txtBranchCode.MultiLine = false;
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBranchCode.Text = "txtBranchCode";
            this.txtBranchCode.Top = 0F;
            this.txtBranchCode.Width = 0.8000002F;
            // 
            // txtBranchName
            // 
            this.txtBranchName.Height = 0.2F;
            this.txtBranchName.Left = 8.620001F;
            this.txtBranchName.MultiLine = false;
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtBranchName.Text = "txtBranchName";
            this.txtBranchName.Top = 0.2F;
            this.txtBranchName.Width = 0.78F;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Height = 0.4F;
            this.txtAccountNumber.Left = 9.400001F;
            this.txtAccountNumber.MultiLine = false;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtAccountNumber.Text = "txtAccountNumber";
            this.txtAccountNumber.Top = 0F;
            this.txtAccountNumber.Width = 0.5F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.Height = 0.2F;
            this.txtDueAt.Left = 9.900001F;
            this.txtDueAt.MultiLine = false;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtDueAt.Text = "txtDueAt";
            this.txtDueAt.Top = 0F;
            this.txtDueAt.Width = 0.6999998F;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.Height = 0.2F;
            this.txtCurrencyCode.Left = 9.900001F;
            this.txtCurrencyCode.MultiLine = false;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrencyCode.Text = "txtCurrencyCode";
            this.txtCurrencyCode.Top = 0.2F;
            this.txtCurrencyCode.Width = 0.6988974F;
            // 
            // txtDebitDepartmentName
            // 
            this.txtDebitDepartmentName.Height = 0.2F;
            this.txtDebitDepartmentName.Left = 1.22F;
            this.txtDebitDepartmentName.MultiLine = false;
            this.txtDebitDepartmentName.Name = "txtDebitDepartmentName";
            this.txtDebitDepartmentName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtDebitDepartmentName.Text = "txtDebitDepartmentName";
            this.txtDebitDepartmentName.Top = 0.2F;
            this.txtDebitDepartmentName.Width = 0.73F;
            // 
            // txtDebitDepartmentCode
            // 
            this.txtDebitDepartmentCode.Height = 0.2F;
            this.txtDebitDepartmentCode.Left = 1.2F;
            this.txtDebitDepartmentCode.MultiLine = false;
            this.txtDebitDepartmentCode.Name = "txtDebitDepartmentCode";
            this.txtDebitDepartmentCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtDebitDepartmentCode.Text = "txtDebitDepartmentCode";
            this.txtDebitDepartmentCode.Top = 0F;
            this.txtDebitDepartmentCode.Width = 0.75F;
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
            // lineDetailVerDebitDepartment
            // 
            this.lineDetailVerDebitDepartment.Height = 0.4F;
            this.lineDetailVerDebitDepartment.Left = 1.95F;
            this.lineDetailVerDebitDepartment.LineWeight = 1F;
            this.lineDetailVerDebitDepartment.Name = "lineDetailVerDebitDepartment";
            this.lineDetailVerDebitDepartment.Top = 0F;
            this.lineDetailVerDebitDepartment.Width = 0F;
            this.lineDetailVerDebitDepartment.X1 = 1.95F;
            this.lineDetailVerDebitDepartment.X2 = 1.95F;
            this.lineDetailVerDebitDepartment.Y1 = 0F;
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
            // lineDetailVerDebitSubCode
            // 
            this.lineDetailVerDebitSubCode.Height = 0.4F;
            this.lineDetailVerDebitSubCode.Left = 3.45F;
            this.lineDetailVerDebitSubCode.LineWeight = 1F;
            this.lineDetailVerDebitSubCode.Name = "lineDetailVerDebitSubCode";
            this.lineDetailVerDebitSubCode.Top = 0F;
            this.lineDetailVerDebitSubCode.Width = 0F;
            this.lineDetailVerDebitSubCode.X1 = 3.45F;
            this.lineDetailVerDebitSubCode.X2 = 3.45F;
            this.lineDetailVerDebitSubCode.Y1 = 0F;
            this.lineDetailVerDebitSubCode.Y2 = 0.4F;
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
            // lineDetailVerCreditDepartment
            // 
            this.lineDetailVerCreditDepartment.Height = 0.4F;
            this.lineDetailVerCreditDepartment.Left = 4.2F;
            this.lineDetailVerCreditDepartment.LineWeight = 1F;
            this.lineDetailVerCreditDepartment.Name = "lineDetailVerCreditDepartment";
            this.lineDetailVerCreditDepartment.Top = 0F;
            this.lineDetailVerCreditDepartment.Width = 0F;
            this.lineDetailVerCreditDepartment.X1 = 4.2F;
            this.lineDetailVerCreditDepartment.X2 = 4.2F;
            this.lineDetailVerCreditDepartment.Y1 = 0F;
            this.lineDetailVerCreditDepartment.Y2 = 0.4F;
            // 
            // lineDetailVerCreditSubCode
            // 
            this.lineDetailVerCreditSubCode.Height = 0.4F;
            this.lineDetailVerCreditSubCode.Left = 5.7F;
            this.lineDetailVerCreditSubCode.LineWeight = 1F;
            this.lineDetailVerCreditSubCode.Name = "lineDetailVerCreditSubCode";
            this.lineDetailVerCreditSubCode.Top = 0F;
            this.lineDetailVerCreditSubCode.Width = 0F;
            this.lineDetailVerCreditSubCode.X1 = 5.7F;
            this.lineDetailVerCreditSubCode.X2 = 5.7F;
            this.lineDetailVerCreditSubCode.Y1 = 0F;
            this.lineDetailVerCreditSubCode.Y2 = 0.4F;
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
            // lineDetailVerAmount
            // 
            this.lineDetailVerAmount.Height = 0.4F;
            this.lineDetailVerAmount.Left = 6.5F;
            this.lineDetailVerAmount.LineWeight = 1F;
            this.lineDetailVerAmount.Name = "lineDetailVerAmount";
            this.lineDetailVerAmount.Top = 0F;
            this.lineDetailVerAmount.Width = 0F;
            this.lineDetailVerAmount.X1 = 6.5F;
            this.lineDetailVerAmount.X2 = 6.5F;
            this.lineDetailVerAmount.Y1 = 0F;
            this.lineDetailVerAmount.Y2 = 0.4F;
            // 
            // lineDetailVerNote
            // 
            this.lineDetailVerNote.Height = 0.4F;
            this.lineDetailVerNote.Left = 7.8F;
            this.lineDetailVerNote.LineWeight = 1F;
            this.lineDetailVerNote.Name = "lineDetailVerNote";
            this.lineDetailVerNote.Top = 0F;
            this.lineDetailVerNote.Width = 0F;
            this.lineDetailVerNote.X1 = 7.8F;
            this.lineDetailVerNote.X2 = 7.8F;
            this.lineDetailVerNote.Y1 = 0F;
            this.lineDetailVerNote.Y2 = 0.4F;
            // 
            // lineDetailVerBankCode
            // 
            this.lineDetailVerBankCode.Height = 0.4F;
            this.lineDetailVerBankCode.Left = 8.6F;
            this.lineDetailVerBankCode.LineWeight = 1F;
            this.lineDetailVerBankCode.Name = "lineDetailVerBankCode";
            this.lineDetailVerBankCode.Top = 0F;
            this.lineDetailVerBankCode.Width = 0F;
            this.lineDetailVerBankCode.X1 = 8.6F;
            this.lineDetailVerBankCode.X2 = 8.6F;
            this.lineDetailVerBankCode.Y1 = 0F;
            this.lineDetailVerBankCode.Y2 = 0.4F;
            // 
            // lineDetailVerBranchCode
            // 
            this.lineDetailVerBranchCode.Height = 0.4F;
            this.lineDetailVerBranchCode.Left = 9.400001F;
            this.lineDetailVerBranchCode.LineWeight = 1F;
            this.lineDetailVerBranchCode.Name = "lineDetailVerBranchCode";
            this.lineDetailVerBranchCode.Top = 0F;
            this.lineDetailVerBranchCode.Width = 0F;
            this.lineDetailVerBranchCode.X1 = 9.400001F;
            this.lineDetailVerBranchCode.X2 = 9.400001F;
            this.lineDetailVerBranchCode.Y1 = 0F;
            this.lineDetailVerBranchCode.Y2 = 0.4F;
            // 
            // lineDetailVerAccountNumber
            // 
            this.lineDetailVerAccountNumber.Height = 0.4F;
            this.lineDetailVerAccountNumber.Left = 9.900001F;
            this.lineDetailVerAccountNumber.LineWeight = 1F;
            this.lineDetailVerAccountNumber.Name = "lineDetailVerAccountNumber";
            this.lineDetailVerAccountNumber.Top = 0F;
            this.lineDetailVerAccountNumber.Width = 0F;
            this.lineDetailVerAccountNumber.X1 = 9.900001F;
            this.lineDetailVerAccountNumber.X2 = 9.900001F;
            this.lineDetailVerAccountNumber.Y1 = 0F;
            this.lineDetailVerAccountNumber.Y2 = 0.4F;
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
            this.reportInfo1.Width = 10.6F;
            // 
            // groupHeader1
            // 
            this.groupHeader1.Height = 0F;
            this.groupHeader1.Name = "groupHeader1";
            // 
            // groupFooter1
            // 
            this.groupFooter1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblSpace,
            this.txtTotalCount,
            this.txtTotalAmount,
            this.lblTotal,
            this.lineFooterVerTotal,
            this.lineFooterVerTotalAmount,
            this.lineFooterVerSpace,
            this.lineFooterHorLower});
            this.groupFooter1.Height = 0.3080381F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // lblSpace
            // 
            this.lblSpace.Height = 0.3F;
            this.lblSpace.HyperLink = null;
            this.lblSpace.Left = 6.5F;
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblSpace.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblSpace.Text = "";
            this.lblSpace.Top = 0F;
            this.lblSpace.Width = 3.4F;
            // 
            // txtTotalCount
            // 
            this.txtTotalCount.Height = 0.3F;
            this.txtTotalCount.Left = 9.900001F;
            this.txtTotalCount.MultiLine = false;
            this.txtTotalCount.Name = "txtTotalCount";
            this.txtTotalCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtTotalCount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtTotalCount.Text = "txtTotalCount";
            this.txtTotalCount.Top = 0F;
            this.txtTotalCount.Width = 0.7F;
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.DataField = "Amount";
            this.txtTotalAmount.Height = 0.3F;
            this.txtTotalAmount.Left = 5.7F;
            this.txtTotalAmount.MultiLine = false;
            this.txtTotalAmount.Name = "txtTotalAmount";
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
            this.lblTotal.Text = "合計";
            this.lblTotal.Top = 9.313226E-10F;
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
            this.lineFooterVerSpace.Left = 9.900001F;
            this.lineFooterVerSpace.LineWeight = 1F;
            this.lineFooterVerSpace.Name = "lineFooterVerSpace";
            this.lineFooterVerSpace.Top = 0F;
            this.lineFooterVerSpace.Width = 0F;
            this.lineFooterVerSpace.X1 = 9.900001F;
            this.lineFooterVerSpace.X2 = 9.900001F;
            this.lineFooterVerSpace.Y1 = 0F;
            this.lineFooterVerSpace.Y2 = 0.3F;
            // 
            // lineFooterHorLower
            // 
            this.lineFooterHorLower.Height = 6.854534E-07F;
            this.lineFooterHorLower.Left = 0F;
            this.lineFooterHorLower.LineWeight = 1F;
            this.lineFooterHorLower.Name = "lineFooterHorLower";
            this.lineFooterHorLower.Top = 0.3F;
            this.lineFooterHorLower.Width = 10.6F;
            this.lineFooterHorLower.X1 = 0F;
            this.lineFooterHorLower.X2 = 10.6F;
            this.lineFooterHorLower.Y1 = 0.3F;
            this.lineFooterHorLower.Y2 = 0.3000007F;
            // 
            // ReceiptJournalizingSectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.60909F;
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
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranckCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCredit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSlipNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlipNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitSubName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAccountTitleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditSubName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebit;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCredit;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranckCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitAccountTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditAccountTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDebitDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSlipNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitAccountTitleName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitSubCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitAccountTitleCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitSubName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditAccountTitleName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditSubName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerSpace;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalCount;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDebitAccountTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditAccountTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorDueAt;
    }
}
