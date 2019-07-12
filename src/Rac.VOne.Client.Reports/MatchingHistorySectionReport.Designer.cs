namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// TestSectionReport の概要の説明です。
    /// </summary>
    partial class MatchingHistorySectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MatchingHistorySectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblcompanycode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lbltitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreateAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCategory = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingRemain = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptRemain = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptId = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptCategory = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBankCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBankName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBranchName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPayerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblLoginUser = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblProcessType = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line10 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line11 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line13 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line14 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line15 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line16 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line17 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line18 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line19 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line20 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line24 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCreateAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBeforeAccept = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCategory = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptId = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptCategory = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPayerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtLoginUser = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtProcessType = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line26 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line27 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line28 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line29 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line30 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line31 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line32 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line33 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line34 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line35 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line36 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line37 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line38 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line39 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line25 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.ghCreateAt = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.gfCreateAt = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.shape1 = new GrapeCity.ActiveReports.SectionReportModel.Shape();
            this.txtCreateAtTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblCreateAtTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtLoginUserTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtProcessTypeTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingRemainTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptRemainTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line21 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line22 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line23 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line40 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line41 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProcessType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBeforeAccept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcessType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAtTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAtTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcessTypeTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingRemainTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptRemainTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.label1,
            this.lblcompanycode,
            this.lbldate,
            this.ridate,
            this.lbltitle,
            this.lblCreateAt,
            this.lblBillingAmount,
            this.lblAmount,
            this.lblBillingAt,
            this.lblInvoiceCode,
            this.lblCategory,
            this.lblBillingRemain,
            this.lblCode,
            this.lblName,
            this.lblRecordedAt,
            this.lblReceiptAmount,
            this.lblReceiptRemain,
            this.lblReceiptId,
            this.lblReceiptCategory,
            this.lblBankCode,
            this.lblBankName,
            this.lblBranchCode,
            this.lblBranchName,
            this.lblAccountNumber,
            this.lblPayerName,
            this.lblNote,
            this.lblLoginUser,
            this.lblProcessType,
            this.line2,
            this.line8,
            this.line9,
            this.line10,
            this.line11,
            this.line12,
            this.line13,
            this.line14,
            this.line15,
            this.line16,
            this.line17,
            this.line18,
            this.line19,
            this.line20,
            this.line24,
            this.line1});
            this.pageHeader.Height = 1.199667F;
            this.pageHeader.Name = "pageHeader";
            // 
            // label1
            // 
            this.label1.Height = 0.2F;
            this.label1.HyperLink = null;
            this.label1.Left = 0.02440945F;
            this.label1.Name = "label1";
            this.label1.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.label1.Text = "会社コード　：";
            this.label1.Top = 0F;
            this.label1.Width = 0.7874016F;
            // 
            // lblcompanycode
            // 
            this.lblcompanycode.Height = 0.2F;
            this.lblcompanycode.HyperLink = null;
            this.lblcompanycode.Left = 0.811811F;
            this.lblcompanycode.Name = "lblcompanycode";
            this.lblcompanycode.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblcompanycode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblcompanycode.Text = "label2";
            this.lblcompanycode.Top = 0F;
            this.lblcompanycode.Width = 3.657F;
            // 
            // lbldate
            // 
            this.lbldate.Height = 0.2F;
            this.lbldate.HyperLink = null;
            this.lbldate.Left = 8.809055F;
            this.lbldate.Name = "lbldate";
            this.lbldate.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lbldate.Text = "出力日付　：";
            this.lbldate.Top = 0F;
            this.lbldate.Width = 0.6984252F;
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
            // lbltitle
            // 
            this.lbltitle.Height = 0.2311024F;
            this.lbltitle.HyperLink = null;
            this.lbltitle.Left = 0F;
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lbltitle.Text = "消込履歴データ 一覧";
            this.lbltitle.Top = 0.2704725F;
            this.lbltitle.Width = 10.62992F;
            // 
            // lblCreateAt
            // 
            this.lblCreateAt.Height = 0.4F;
            this.lblCreateAt.HyperLink = null;
            this.lblCreateAt.Left = 0F;
            this.lblCreateAt.Name = "lblCreateAt";
            this.lblCreateAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCreateAt.Text = "消込日時";
            this.lblCreateAt.Top = 0.8F;
            this.lblCreateAt.Width = 0.9F;
            // 
            // lblBillingAmount
            // 
            this.lblBillingAmount.Height = 0.2F;
            this.lblBillingAmount.HyperLink = null;
            this.lblBillingAmount.Left = 3.4F;
            this.lblBillingAmount.Name = "lblBillingAmount";
            this.lblBillingAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingAmount.Text = "請求額";
            this.lblBillingAmount.Top = 0.8F;
            this.lblBillingAmount.Width = 0.75F;
            // 
            // lblAmount
            // 
            this.lblAmount.Height = 0.2F;
            this.lblAmount.HyperLink = null;
            this.lblAmount.Left = 3.4F;
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAmount.Text = "消込額";
            this.lblAmount.Top = 1F;
            this.lblAmount.Width = 0.75F;
            // 
            // lblBillingAt
            // 
            this.lblBillingAt.Height = 0.4F;
            this.lblBillingAt.HyperLink = null;
            this.lblBillingAt.Left = 2.2F;
            this.lblBillingAt.Name = "lblBillingAt";
            this.lblBillingAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingAt.Text = "請求日";
            this.lblBillingAt.Top = 0.8F;
            this.lblBillingAt.Width = 0.5F;
            // 
            // lblInvoiceCode
            // 
            this.lblInvoiceCode.Height = 0.2F;
            this.lblInvoiceCode.HyperLink = null;
            this.lblInvoiceCode.Left = 2.7F;
            this.lblInvoiceCode.Name = "lblInvoiceCode";
            this.lblInvoiceCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblInvoiceCode.Text = "請求書番号";
            this.lblInvoiceCode.Top = 0.8F;
            this.lblInvoiceCode.Width = 0.7F;
            // 
            // lblCategory
            // 
            this.lblCategory.Height = 0.2F;
            this.lblCategory.HyperLink = null;
            this.lblCategory.Left = 2.7F;
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCategory.Text = "請求区分";
            this.lblCategory.Top = 1F;
            this.lblCategory.Width = 0.7F;
            // 
            // lblBillingRemain
            // 
            this.lblBillingRemain.Height = 0.4000001F;
            this.lblBillingRemain.HyperLink = null;
            this.lblBillingRemain.Left = 4.15F;
            this.lblBillingRemain.Name = "lblBillingRemain";
            this.lblBillingRemain.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingRemain.Text = "請求残";
            this.lblBillingRemain.Top = 0.8F;
            this.lblBillingRemain.Width = 0.75F;
            // 
            // lblCode
            // 
            this.lblCode.Height = 0.2F;
            this.lblCode.HyperLink = null;
            this.lblCode.Left = 0.9F;
            this.lblCode.Name = "lblCode";
            this.lblCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCode.Text = "得意先コード";
            this.lblCode.Top = 0.8F;
            this.lblCode.Width = 1.3F;
            // 
            // lblName
            // 
            this.lblName.Height = 0.2F;
            this.lblName.HyperLink = null;
            this.lblName.Left = 0.9F;
            this.lblName.Name = "lblName";
            this.lblName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblName.Text = "得意先名";
            this.lblName.Top = 1F;
            this.lblName.Width = 1.3F;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Height = 0.4F;
            this.lblRecordedAt.HyperLink = null;
            this.lblRecordedAt.Left = 4.9F;
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblRecordedAt.Text = "入金日";
            this.lblRecordedAt.Top = 0.8F;
            this.lblRecordedAt.Width = 0.5F;
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Height = 0.2F;
            this.lblReceiptAmount.HyperLink = null;
            this.lblReceiptAmount.Left = 6F;
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptAmount.Text = "入金額";
            this.lblReceiptAmount.Top = 0.8F;
            this.lblReceiptAmount.Width = 0.85F;
            // 
            // lblReceiptRemain
            // 
            this.lblReceiptRemain.Height = 0.2F;
            this.lblReceiptRemain.HyperLink = null;
            this.lblReceiptRemain.Left = 6F;
            this.lblReceiptRemain.Name = "lblReceiptRemain";
            this.lblReceiptRemain.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptRemain.Text = "入金残";
            this.lblReceiptRemain.Top = 1F;
            this.lblReceiptRemain.Width = 0.85F;
            // 
            // lblReceiptId
            // 
            this.lblReceiptId.Height = 0.2F;
            this.lblReceiptId.HyperLink = null;
            this.lblReceiptId.Left = 5.4F;
            this.lblReceiptId.Name = "lblReceiptId";
            this.lblReceiptId.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptId.Text = "入金ID";
            this.lblReceiptId.Top = 0.8F;
            this.lblReceiptId.Width = 0.6F;
            // 
            // lblReceiptCategory
            // 
            this.lblReceiptCategory.Height = 0.2F;
            this.lblReceiptCategory.HyperLink = null;
            this.lblReceiptCategory.Left = 5.4F;
            this.lblReceiptCategory.Name = "lblReceiptCategory";
            this.lblReceiptCategory.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptCategory.Text = "入金区分";
            this.lblReceiptCategory.Top = 1F;
            this.lblReceiptCategory.Width = 0.6F;
            // 
            // lblBankCode
            // 
            this.lblBankCode.Height = 0.2F;
            this.lblBankCode.HyperLink = null;
            this.lblBankCode.Left = 6.85F;
            this.lblBankCode.Name = "lblBankCode";
            this.lblBankCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBankCode.Text = "銀行コード";
            this.lblBankCode.Top = 0.8F;
            this.lblBankCode.Width = 0.7F;
            // 
            // lblBankName
            // 
            this.lblBankName.Height = 0.2F;
            this.lblBankName.HyperLink = null;
            this.lblBankName.Left = 6.85F;
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBankName.Text = "銀行名";
            this.lblBankName.Top = 1F;
            this.lblBankName.Width = 0.7F;
            // 
            // lblBranchCode
            // 
            this.lblBranchCode.Height = 0.2F;
            this.lblBranchCode.HyperLink = null;
            this.lblBranchCode.Left = 7.55F;
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBranchCode.Text = "支店コード";
            this.lblBranchCode.Top = 0.8F;
            this.lblBranchCode.Width = 0.7F;
            // 
            // lblBranchName
            // 
            this.lblBranchName.Height = 0.2F;
            this.lblBranchName.HyperLink = null;
            this.lblBranchName.Left = 7.55F;
            this.lblBranchName.Name = "lblBranchName";
            this.lblBranchName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBranchName.Text = "支店名";
            this.lblBranchName.Top = 1F;
            this.lblBranchName.Width = 0.7F;
            // 
            // lblAccountNumber
            // 
            this.lblAccountNumber.Height = 0.4F;
            this.lblAccountNumber.HyperLink = null;
            this.lblAccountNumber.Left = 8.25F;
            this.lblAccountNumber.Name = "lblAccountNumber";
            this.lblAccountNumber.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAccountNumber.Text = "口座番号";
            this.lblAccountNumber.Top = 0.8F;
            this.lblAccountNumber.Width = 0.6F;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Height = 0.2F;
            this.lblPayerName.HyperLink = null;
            this.lblPayerName.Left = 8.85F;
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.Top = 0.8F;
            this.lblPayerName.Width = 1.08F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.2F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 8.85F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblNote.Text = "備考";
            this.lblNote.Top = 1F;
            this.lblNote.Width = 1.08F;
            // 
            // lblLoginUser
            // 
            this.lblLoginUser.Height = 0.2F;
            this.lblLoginUser.HyperLink = null;
            this.lblLoginUser.Left = 9.93F;
            this.lblLoginUser.Name = "lblLoginUser";
            this.lblLoginUser.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblLoginUser.Text = "実行ユーザー";
            this.lblLoginUser.Top = 0.8F;
            this.lblLoginUser.Width = 0.7F;
            // 
            // lblProcessType
            // 
            this.lblProcessType.Height = 0.2F;
            this.lblProcessType.HyperLink = null;
            this.lblProcessType.Left = 9.93F;
            this.lblProcessType.Name = "lblProcessType";
            this.lblProcessType.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblProcessType.Text = "消込";
            this.lblProcessType.Top = 1F;
            this.lblProcessType.Width = 0.7F;
            // 
            // line2
            // 
            this.line2.Height = 0F;
            this.line2.Left = 0F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 0.8F;
            this.line2.Width = 10.63F;
            this.line2.X1 = 0F;
            this.line2.X2 = 10.63F;
            this.line2.Y1 = 0.8F;
            this.line2.Y2 = 0.8F;
            // 
            // line8
            // 
            this.line8.Height = 0.4F;
            this.line8.Left = 9.93F;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.Top = 0.8F;
            this.line8.Width = 0F;
            this.line8.X1 = 9.93F;
            this.line8.X2 = 9.93F;
            this.line8.Y1 = 0.8F;
            this.line8.Y2 = 1.2F;
            // 
            // line9
            // 
            this.line9.Height = 0.4F;
            this.line9.Left = 8.85F;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.Top = 0.8F;
            this.line9.Width = 0F;
            this.line9.X1 = 8.85F;
            this.line9.X2 = 8.85F;
            this.line9.Y1 = 0.8F;
            this.line9.Y2 = 1.2F;
            // 
            // line10
            // 
            this.line10.Height = 0.4F;
            this.line10.Left = 8.25F;
            this.line10.LineWeight = 1F;
            this.line10.Name = "line10";
            this.line10.Top = 0.8F;
            this.line10.Width = 0F;
            this.line10.X1 = 8.25F;
            this.line10.X2 = 8.25F;
            this.line10.Y1 = 0.8F;
            this.line10.Y2 = 1.2F;
            // 
            // line11
            // 
            this.line11.Height = 0.4F;
            this.line11.Left = 7.55F;
            this.line11.LineWeight = 1F;
            this.line11.Name = "line11";
            this.line11.Top = 0.8F;
            this.line11.Width = 0F;
            this.line11.X1 = 7.55F;
            this.line11.X2 = 7.55F;
            this.line11.Y1 = 0.8F;
            this.line11.Y2 = 1.2F;
            // 
            // line12
            // 
            this.line12.Height = 0.4F;
            this.line12.Left = 6.85F;
            this.line12.LineWeight = 1F;
            this.line12.Name = "line12";
            this.line12.Top = 0.8F;
            this.line12.Width = 0F;
            this.line12.X1 = 6.85F;
            this.line12.X2 = 6.85F;
            this.line12.Y1 = 0.8F;
            this.line12.Y2 = 1.2F;
            // 
            // line13
            // 
            this.line13.Height = 0.4F;
            this.line13.Left = 6F;
            this.line13.LineWeight = 1F;
            this.line13.Name = "line13";
            this.line13.Top = 0.8F;
            this.line13.Width = 0F;
            this.line13.X1 = 6F;
            this.line13.X2 = 6F;
            this.line13.Y1 = 0.8F;
            this.line13.Y2 = 1.2F;
            // 
            // line14
            // 
            this.line14.Height = 0.4F;
            this.line14.Left = 5.4F;
            this.line14.LineWeight = 1F;
            this.line14.Name = "line14";
            this.line14.Top = 0.8F;
            this.line14.Width = 0F;
            this.line14.X1 = 5.4F;
            this.line14.X2 = 5.4F;
            this.line14.Y1 = 0.8F;
            this.line14.Y2 = 1.2F;
            // 
            // line15
            // 
            this.line15.Height = 0.4F;
            this.line15.Left = 4.9F;
            this.line15.LineWeight = 1F;
            this.line15.Name = "line15";
            this.line15.Top = 0.8F;
            this.line15.Width = 0F;
            this.line15.X1 = 4.9F;
            this.line15.X2 = 4.9F;
            this.line15.Y1 = 0.8F;
            this.line15.Y2 = 1.2F;
            // 
            // line16
            // 
            this.line16.Height = 0.4F;
            this.line16.Left = 4.15F;
            this.line16.LineWeight = 1F;
            this.line16.Name = "line16";
            this.line16.Top = 0.8F;
            this.line16.Width = 0F;
            this.line16.X1 = 4.15F;
            this.line16.X2 = 4.15F;
            this.line16.Y1 = 0.8F;
            this.line16.Y2 = 1.2F;
            // 
            // line17
            // 
            this.line17.Height = 0.4F;
            this.line17.Left = 3.4F;
            this.line17.LineWeight = 1F;
            this.line17.Name = "line17";
            this.line17.Top = 0.8F;
            this.line17.Width = 0F;
            this.line17.X1 = 3.4F;
            this.line17.X2 = 3.4F;
            this.line17.Y1 = 0.8F;
            this.line17.Y2 = 1.2F;
            // 
            // line18
            // 
            this.line18.Height = 0.4F;
            this.line18.Left = 2.7F;
            this.line18.LineWeight = 1F;
            this.line18.Name = "line18";
            this.line18.Top = 0.8F;
            this.line18.Width = 0F;
            this.line18.X1 = 2.7F;
            this.line18.X2 = 2.7F;
            this.line18.Y1 = 0.8F;
            this.line18.Y2 = 1.2F;
            // 
            // line19
            // 
            this.line19.Height = 0.4F;
            this.line19.Left = 2.2F;
            this.line19.LineWeight = 1F;
            this.line19.Name = "line19";
            this.line19.Top = 0.8F;
            this.line19.Width = 0F;
            this.line19.X1 = 2.2F;
            this.line19.X2 = 2.2F;
            this.line19.Y1 = 0.8F;
            this.line19.Y2 = 1.2F;
            // 
            // line20
            // 
            this.line20.Height = 0.4F;
            this.line20.Left = 0.9F;
            this.line20.LineWeight = 1F;
            this.line20.Name = "line20";
            this.line20.Top = 0.8F;
            this.line20.Width = 0F;
            this.line20.X1 = 0.9F;
            this.line20.X2 = 0.9F;
            this.line20.Y1 = 0.8F;
            this.line20.Y2 = 1.2F;
            // 
            // line24
            // 
            this.line24.Height = 0.4F;
            this.line24.Left = 4.93F;
            this.line24.LineWeight = 1F;
            this.line24.Name = "line24";
            this.line24.Top = 0.8F;
            this.line24.Width = 0F;
            this.line24.X1 = 4.93F;
            this.line24.X2 = 4.93F;
            this.line24.Y1 = 0.8F;
            this.line24.Y2 = 1.2F;
            // 
            // line1
            // 
            this.line1.Height = 0F;
            this.line1.Left = 0F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 1.2F;
            this.line1.Width = 10.63F;
            this.line1.X1 = 0F;
            this.line1.X2 = 10.63F;
            this.line1.Y1 = 1.2F;
            this.line1.Y2 = 1.2F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCreateAt,
            this.txtCustomerName,
            this.txtBeforeAccept,
            this.txtCustomerCode,
            this.txtBillingAt,
            this.txtInvoiceCode,
            this.txtCategory,
            this.txtBillingAmount,
            this.txtAmount,
            this.txtBillingRemain,
            this.txtRecordedAt,
            this.txtReceiptId,
            this.txtReceiptCategory,
            this.txtReceiptAmount,
            this.txtReceiptRemain,
            this.txtBankCode,
            this.txtBankName,
            this.txtBranchCode,
            this.txtBranchName,
            this.txtAccountNumber,
            this.txtPayerName,
            this.txtNote,
            this.txtLoginUser,
            this.txtProcessType,
            this.line26,
            this.line27,
            this.line28,
            this.line29,
            this.line30,
            this.line31,
            this.line32,
            this.line33,
            this.line34,
            this.line35,
            this.line36,
            this.line37,
            this.line38,
            this.line39,
            this.line25,
            this.line3});
            this.detail.Height = 0.4F;
            this.detail.Name = "detail";
            // 
            // txtCreateAt
            // 
            this.txtCreateAt.Height = 0.4F;
            this.txtCreateAt.Left = 0F;
            this.txtCreateAt.MultiLine = false;
            this.txtCreateAt.Name = "txtCreateAt";
            this.txtCreateAt.OutputFormat = resources.GetString("txtCreateAt.OutputFormat");
            this.txtCreateAt.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCreateAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCreateAt.Text = "CreateAt";
            this.txtCreateAt.Top = 0F;
            this.txtCreateAt.Width = 0.9F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 0.92F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCustomerName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtCustomerName.Text = "Name";
            this.txtCustomerName.Top = 0.2F;
            this.txtCustomerName.Width = 1.28F;
            // 
            // txtBeforeAccept
            // 
            this.txtBeforeAccept.Height = 0.19F;
            this.txtBeforeAccept.Left = 6F;
            this.txtBeforeAccept.Name = "txtBeforeAccept";
            this.txtBeforeAccept.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBeforeAccept.Text = "前";
            this.txtBeforeAccept.Top = 0.1F;
            this.txtBeforeAccept.Width = 0.1499996F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 0.9F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCustomerCode.Text = "Code";
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 1.3F;
            // 
            // txtBillingAt
            // 
            this.txtBillingAt.Height = 0.4F;
            this.txtBillingAt.Left = 2.2F;
            this.txtBillingAt.MultiLine = false;
            this.txtBillingAt.Name = "txtBillingAt";
            this.txtBillingAt.OutputFormat = resources.GetString("txtBillingAt.OutputFormat");
            this.txtBillingAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBillingAt.Text = "BillingAt";
            this.txtBillingAt.Top = 0F;
            this.txtBillingAt.Width = 0.5F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.Height = 0.2F;
            this.txtInvoiceCode.Left = 2.72F;
            this.txtInvoiceCode.MultiLine = false;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtInvoiceCode.Text = "InvoiceCode";
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 0.68F;
            // 
            // txtCategory
            // 
            this.txtCategory.Height = 0.2F;
            this.txtCategory.Left = 2.72F;
            this.txtCategory.MultiLine = false;
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCategory.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtCategory.Text = "Category";
            this.txtCategory.Top = 0.2F;
            this.txtCategory.Width = 0.68F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.Height = 0.2F;
            this.txtBillingAmount.Left = 3.4F;
            this.txtBillingAmount.MultiLine = false;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.OutputFormat = resources.GetString("txtBillingAmount.OutputFormat");
            this.txtBillingAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtBillingAmount.Text = "BillingAmount";
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 0.73F;
            // 
            // txtAmount
            // 
            this.txtAmount.Height = 0.2F;
            this.txtAmount.Left = 3.4F;
            this.txtAmount.MultiLine = false;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.OutputFormat = resources.GetString("txtAmount.OutputFormat");
            this.txtAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtAmount.Text = "Amount";
            this.txtAmount.Top = 0.2F;
            this.txtAmount.Width = 0.73F;
            // 
            // txtBillingRemain
            // 
            this.txtBillingRemain.Height = 0.4F;
            this.txtBillingRemain.Left = 4.15F;
            this.txtBillingRemain.MultiLine = false;
            this.txtBillingRemain.Name = "txtBillingRemain";
            this.txtBillingRemain.OutputFormat = resources.GetString("txtBillingRemain.OutputFormat");
            this.txtBillingRemain.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtBillingRemain.Text = "BillingRemain";
            this.txtBillingRemain.Top = -4.656613E-10F;
            this.txtBillingRemain.Width = 0.73F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.4F;
            this.txtRecordedAt.Left = 4.9F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtRecordedAt.Text = "RecordedAt";
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.5F;
            // 
            // txtReceiptId
            // 
            this.txtReceiptId.Height = 0.2F;
            this.txtReceiptId.Left = 5.4F;
            this.txtReceiptId.MultiLine = false;
            this.txtReceiptId.Name = "txtReceiptId";
            this.txtReceiptId.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptId.Text = "ReceiptId";
            this.txtReceiptId.Top = 0F;
            this.txtReceiptId.Width = 0.58F;
            // 
            // txtReceiptCategory
            // 
            this.txtReceiptCategory.Height = 0.2F;
            this.txtReceiptCategory.Left = 5.42F;
            this.txtReceiptCategory.MultiLine = false;
            this.txtReceiptCategory.Name = "txtReceiptCategory";
            this.txtReceiptCategory.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtReceiptCategory.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtReceiptCategory.Text = "ReceiptCategory";
            this.txtReceiptCategory.Top = 0.2F;
            this.txtReceiptCategory.Width = 0.58F;
            // 
            // txtReceiptAmount
            // 
            this.txtReceiptAmount.Height = 0.2F;
            this.txtReceiptAmount.Left = 6.15F;
            this.txtReceiptAmount.MultiLine = false;
            this.txtReceiptAmount.Name = "txtReceiptAmount";
            this.txtReceiptAmount.OutputFormat = resources.GetString("txtReceiptAmount.OutputFormat");
            this.txtReceiptAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtReceiptAmount.Text = "ReceiptAmount";
            this.txtReceiptAmount.Top = 0.003F;
            this.txtReceiptAmount.Width = 0.7F;
            // 
            // txtReceiptRemain
            // 
            this.txtReceiptRemain.Height = 0.2F;
            this.txtReceiptRemain.Left = 6.15F;
            this.txtReceiptRemain.MultiLine = false;
            this.txtReceiptRemain.Name = "txtReceiptRemain";
            this.txtReceiptRemain.OutputFormat = resources.GetString("txtReceiptRemain.OutputFormat");
            this.txtReceiptRemain.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtReceiptRemain.Text = "ReceiptRemain";
            this.txtReceiptRemain.Top = 0.203F;
            this.txtReceiptRemain.Width = 0.7F;
            // 
            // txtBankCode
            // 
            this.txtBankCode.Height = 0.2F;
            this.txtBankCode.Left = 6.85F;
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBankCode.Text = "BankCode";
            this.txtBankCode.Top = 0F;
            this.txtBankCode.Width = 0.6999993F;
            // 
            // txtBankName
            // 
            this.txtBankName.Height = 0.2F;
            this.txtBankName.Left = 6.87F;
            this.txtBankName.MultiLine = false;
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtBankName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtBankName.Text = "BankName";
            this.txtBankName.Top = 0.2F;
            this.txtBankName.Width = 0.68F;
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.Height = 0.2F;
            this.txtBranchCode.Left = 7.55F;
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBranchCode.Text = "BranchCode";
            this.txtBranchCode.Top = 0F;
            this.txtBranchCode.Width = 0.7F;
            // 
            // txtBranchName
            // 
            this.txtBranchName.Height = 0.2F;
            this.txtBranchName.Left = 7.57F;
            this.txtBranchName.MultiLine = false;
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtBranchName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtBranchName.Text = "BranchName";
            this.txtBranchName.Top = 0.2F;
            this.txtBranchName.Width = 0.68F;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Height = 0.4F;
            this.txtAccountNumber.Left = 8.25F;
            this.txtAccountNumber.MultiLine = false;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtAccountNumber.Text = "AccountNumber";
            this.txtAccountNumber.Top = 0F;
            this.txtAccountNumber.Width = 0.6F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.2F;
            this.txtPayerName.Left = 8.870001F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtPayerName.Text = "PayerName";
            this.txtPayerName.Top = 0F;
            this.txtPayerName.Width = 1.06F;
            // 
            // txtNote
            // 
            this.txtNote.Height = 0.2F;
            this.txtNote.Left = 8.87F;
            this.txtNote.MultiLine = false;
            this.txtNote.Name = "txtNote";
            this.txtNote.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtNote.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtNote.Text = "Note";
            this.txtNote.Top = 0.2F;
            this.txtNote.Width = 1.059999F;
            // 
            // txtLoginUser
            // 
            this.txtLoginUser.Height = 0.2F;
            this.txtLoginUser.Left = 9.950001F;
            this.txtLoginUser.MultiLine = false;
            this.txtLoginUser.Name = "txtLoginUser";
            this.txtLoginUser.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtLoginUser.Text = "LoginUser";
            this.txtLoginUser.Top = 0F;
            this.txtLoginUser.Width = 0.68F;
            // 
            // txtProcessType
            // 
            this.txtProcessType.Height = 0.2F;
            this.txtProcessType.Left = 9.93F;
            this.txtProcessType.MultiLine = false;
            this.txtProcessType.Name = "txtProcessType";
            this.txtProcessType.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtProcessType.Text = "ProcessType";
            this.txtProcessType.Top = 0.2F;
            this.txtProcessType.Width = 0.6999998F;
            // 
            // line26
            // 
            this.line26.Height = 0.397F;
            this.line26.Left = 9.93F;
            this.line26.LineWeight = 1F;
            this.line26.Name = "line26";
            this.line26.Top = 0F;
            this.line26.Width = 0F;
            this.line26.X1 = 9.93F;
            this.line26.X2 = 9.93F;
            this.line26.Y1 = 0F;
            this.line26.Y2 = 0.397F;
            // 
            // line27
            // 
            this.line27.Height = 0.397F;
            this.line27.Left = 8.85F;
            this.line27.LineWeight = 1F;
            this.line27.Name = "line27";
            this.line27.Top = 0F;
            this.line27.Width = 0F;
            this.line27.X1 = 8.85F;
            this.line27.X2 = 8.85F;
            this.line27.Y1 = 0F;
            this.line27.Y2 = 0.397F;
            // 
            // line28
            // 
            this.line28.Height = 0.4F;
            this.line28.Left = 8.25F;
            this.line28.LineWeight = 1F;
            this.line28.Name = "line28";
            this.line28.Top = 0F;
            this.line28.Width = 0F;
            this.line28.X1 = 8.25F;
            this.line28.X2 = 8.25F;
            this.line28.Y1 = 0F;
            this.line28.Y2 = 0.4F;
            // 
            // line29
            // 
            this.line29.Height = 0.397F;
            this.line29.Left = 7.55F;
            this.line29.LineWeight = 1F;
            this.line29.Name = "line29";
            this.line29.Top = 0F;
            this.line29.Width = 0F;
            this.line29.X1 = 7.55F;
            this.line29.X2 = 7.55F;
            this.line29.Y1 = 0F;
            this.line29.Y2 = 0.397F;
            // 
            // line30
            // 
            this.line30.Height = 0.4F;
            this.line30.Left = 6.85F;
            this.line30.LineWeight = 1F;
            this.line30.Name = "line30";
            this.line30.Top = 0F;
            this.line30.Width = 0F;
            this.line30.X1 = 6.85F;
            this.line30.X2 = 6.85F;
            this.line30.Y1 = 0F;
            this.line30.Y2 = 0.4F;
            // 
            // line31
            // 
            this.line31.Height = 0.4F;
            this.line31.Left = 6F;
            this.line31.LineWeight = 1F;
            this.line31.Name = "line31";
            this.line31.Top = 0F;
            this.line31.Width = 0F;
            this.line31.X1 = 6F;
            this.line31.X2 = 6F;
            this.line31.Y1 = 0F;
            this.line31.Y2 = 0.4F;
            // 
            // line32
            // 
            this.line32.Height = 0.397F;
            this.line32.Left = 5.4F;
            this.line32.LineWeight = 1F;
            this.line32.Name = "line32";
            this.line32.Top = 0.003F;
            this.line32.Width = 0F;
            this.line32.X1 = 5.4F;
            this.line32.X2 = 5.4F;
            this.line32.Y1 = 0.003F;
            this.line32.Y2 = 0.4F;
            // 
            // line33
            // 
            this.line33.Height = 0.397F;
            this.line33.Left = 4.9F;
            this.line33.LineWeight = 1F;
            this.line33.Name = "line33";
            this.line33.Top = 0F;
            this.line33.Width = 0F;
            this.line33.X1 = 4.9F;
            this.line33.X2 = 4.9F;
            this.line33.Y1 = 0F;
            this.line33.Y2 = 0.397F;
            // 
            // line34
            // 
            this.line34.Height = 0.397F;
            this.line34.Left = 4.15F;
            this.line34.LineWeight = 1F;
            this.line34.Name = "line34";
            this.line34.Top = 0.003F;
            this.line34.Width = 0F;
            this.line34.X1 = 4.15F;
            this.line34.X2 = 4.15F;
            this.line34.Y1 = 0.003F;
            this.line34.Y2 = 0.4F;
            // 
            // line35
            // 
            this.line35.Height = 0.4F;
            this.line35.Left = 3.4F;
            this.line35.LineWeight = 1F;
            this.line35.Name = "line35";
            this.line35.Top = 0F;
            this.line35.Width = 0F;
            this.line35.X1 = 3.4F;
            this.line35.X2 = 3.4F;
            this.line35.Y1 = 0F;
            this.line35.Y2 = 0.4F;
            // 
            // line36
            // 
            this.line36.Height = 0.4F;
            this.line36.Left = 2.7F;
            this.line36.LineWeight = 1F;
            this.line36.Name = "line36";
            this.line36.Top = 0F;
            this.line36.Width = 0F;
            this.line36.X1 = 2.7F;
            this.line36.X2 = 2.7F;
            this.line36.Y1 = 0F;
            this.line36.Y2 = 0.4F;
            // 
            // line37
            // 
            this.line37.Height = 0.4F;
            this.line37.Left = 2.2F;
            this.line37.LineWeight = 1F;
            this.line37.Name = "line37";
            this.line37.Top = 0F;
            this.line37.Width = 0F;
            this.line37.X1 = 2.2F;
            this.line37.X2 = 2.2F;
            this.line37.Y1 = 0F;
            this.line37.Y2 = 0.4F;
            // 
            // line38
            // 
            this.line38.Height = 0.4F;
            this.line38.Left = 0.9F;
            this.line38.LineWeight = 1F;
            this.line38.Name = "line38";
            this.line38.Top = 0F;
            this.line38.Width = 0F;
            this.line38.X1 = 0.9F;
            this.line38.X2 = 0.9F;
            this.line38.Y1 = 0F;
            this.line38.Y2 = 0.4F;
            // 
            // line39
            // 
            this.line39.Height = 0.4F;
            this.line39.Left = 6.15F;
            this.line39.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dash;
            this.line39.LineWeight = 1F;
            this.line39.Name = "line39";
            this.line39.Top = 0F;
            this.line39.Width = 0F;
            this.line39.X1 = 6.15F;
            this.line39.X2 = 6.15F;
            this.line39.Y1 = 0F;
            this.line39.Y2 = 0.4F;
            // 
            // line25
            // 
            this.line25.Height = 0.397F;
            this.line25.Left = 4.93F;
            this.line25.LineWeight = 1F;
            this.line25.Name = "line25";
            this.line25.Top = 0F;
            this.line25.Width = 0F;
            this.line25.X1 = 4.93F;
            this.line25.X2 = 4.93F;
            this.line25.Y1 = 0F;
            this.line25.Y2 = 0.397F;
            // 
            // line3
            // 
            this.line3.Height = 0F;
            this.line3.Left = 0F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0.4F;
            this.line3.Width = 10.63F;
            this.line3.X1 = 0F;
            this.line3.X2 = 10.63F;
            this.line3.Y1 = 0.4F;
            this.line3.Y2 = 0.4F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblPageNumber,
            this.reportInfo1});
            this.pageFooter.Height = 0.3149606F;
            this.pageFooter.Name = "pageFooter";
            this.pageFooter.BeforePrint += new System.EventHandler(this.pageFooter_BeforePrint);
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
            // reportInfo1
            // 
            this.reportInfo1.FormatString = "{PageNumber} / {PageCount}";
            this.reportInfo1.Height = 0.2F;
            this.reportInfo1.Left = 7.751969F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.05748032F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1.181102F;
            // 
            // ghCreateAt
            // 
            this.ghCreateAt.Height = 0F;
            this.ghCreateAt.Name = "ghCreateAt";
            // 
            // gfCreateAt
            // 
            this.gfCreateAt.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.shape1,
            this.txtCreateAtTotal,
            this.lblCreateAtTotal,
            this.txtLoginUserTotal,
            this.txtProcessTypeTotal,
            this.txtAmountTotal,
            this.txtBillingAmountTotal,
            this.txtBillingRemainTotal,
            this.txtReceiptAmountTotal,
            this.txtReceiptRemainTotal,
            this.line4,
            this.line5,
            this.line6,
            this.line7,
            this.line21,
            this.line22,
            this.line23,
            this.line40,
            this.line41});
            this.gfCreateAt.Height = 0.4F;
            this.gfCreateAt.Name = "gfCreateAt";
            // 
            // shape1
            // 
            this.shape1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.shape1.Height = 0.4F;
            this.shape1.Left = 0.024F;
            this.shape1.LineColor = System.Drawing.Color.WhiteSmoke;
            this.shape1.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Transparent;
            this.shape1.Name = "shape1";
            this.shape1.RoundingRadius = new GrapeCity.ActiveReports.Controls.CornersRadius(10F, null, null, null, null);
            this.shape1.Top = 0F;
            this.shape1.Width = 10.606F;
            // 
            // txtCreateAtTotal
            // 
            this.txtCreateAtTotal.Height = 0.4F;
            this.txtCreateAtTotal.Left = 0.8850001F;
            this.txtCreateAtTotal.MultiLine = false;
            this.txtCreateAtTotal.Name = "txtCreateAtTotal";
            this.txtCreateAtTotal.OutputFormat = resources.GetString("txtCreateAtTotal.OutputFormat");
            this.txtCreateAtTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCreateAtTotal.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCreateAtTotal.Text = "2017/10/10 00:00:00";
            this.txtCreateAtTotal.Top = 0F;
            this.txtCreateAtTotal.Width = 1.2F;
            // 
            // lblCreateAtTotal
            // 
            this.lblCreateAtTotal.Height = 0.397F;
            this.lblCreateAtTotal.HyperLink = null;
            this.lblCreateAtTotal.Left = 0F;
            this.lblCreateAtTotal.Name = "lblCreateAtTotal";
            this.lblCreateAtTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: left; vertical-align: m" +
    "iddle; ddo-char-set: 1";
            this.lblCreateAtTotal.Text = "*消込日時計（消込単位）：";
            this.lblCreateAtTotal.Top = 0.003F;
            this.lblCreateAtTotal.Width = 1.08F;
            // 
            // txtLoginUserTotal
            // 
            this.txtLoginUserTotal.Height = 0.2F;
            this.txtLoginUserTotal.Left = 9.950001F;
            this.txtLoginUserTotal.MultiLine = false;
            this.txtLoginUserTotal.Name = "txtLoginUserTotal";
            this.txtLoginUserTotal.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtLoginUserTotal.Text = "LoginUser";
            this.txtLoginUserTotal.Top = 0F;
            this.txtLoginUserTotal.Width = 0.68F;
            // 
            // txtProcessTypeTotal
            // 
            this.txtProcessTypeTotal.Height = 0.2F;
            this.txtProcessTypeTotal.Left = 9.93F;
            this.txtProcessTypeTotal.MultiLine = false;
            this.txtProcessTypeTotal.Name = "txtProcessTypeTotal";
            this.txtProcessTypeTotal.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtProcessTypeTotal.Text = "ProcessType";
            this.txtProcessTypeTotal.Top = 0.2F;
            this.txtProcessTypeTotal.Width = 0.7F;
            // 
            // txtAmountTotal
            // 
            this.txtAmountTotal.Height = 0.2F;
            this.txtAmountTotal.Left = 3.4F;
            this.txtAmountTotal.MultiLine = false;
            this.txtAmountTotal.Name = "txtAmountTotal";
            this.txtAmountTotal.OutputFormat = resources.GetString("txtAmountTotal.OutputFormat");
            this.txtAmountTotal.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtAmountTotal.SummaryGroup = "ghCreateAt";
            this.txtAmountTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtAmountTotal.Text = "Amount";
            this.txtAmountTotal.Top = 0.2F;
            this.txtAmountTotal.Width = 0.73F;
            // 
            // txtBillingAmountTotal
            // 
            this.txtBillingAmountTotal.Height = 0.2F;
            this.txtBillingAmountTotal.Left = 3.4F;
            this.txtBillingAmountTotal.MultiLine = false;
            this.txtBillingAmountTotal.Name = "txtBillingAmountTotal";
            this.txtBillingAmountTotal.OutputFormat = resources.GetString("txtBillingAmountTotal.OutputFormat");
            this.txtBillingAmountTotal.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtBillingAmountTotal.SummaryGroup = "ghCreateAt";
            this.txtBillingAmountTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtBillingAmountTotal.Text = "BillingAmount";
            this.txtBillingAmountTotal.Top = 0F;
            this.txtBillingAmountTotal.Width = 0.73F;
            // 
            // txtBillingRemainTotal
            // 
            this.txtBillingRemainTotal.Height = 0.4F;
            this.txtBillingRemainTotal.Left = 4.15F;
            this.txtBillingRemainTotal.MultiLine = false;
            this.txtBillingRemainTotal.Name = "txtBillingRemainTotal";
            this.txtBillingRemainTotal.OutputFormat = resources.GetString("txtBillingRemainTotal.OutputFormat");
            this.txtBillingRemainTotal.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtBillingRemainTotal.SummaryGroup = "ghCreateAt";
            this.txtBillingRemainTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtBillingRemainTotal.Text = "BillingRemain";
            this.txtBillingRemainTotal.Top = 0.003F;
            this.txtBillingRemainTotal.Width = 0.73F;
            // 
            // txtReceiptAmountTotal
            // 
            this.txtReceiptAmountTotal.Height = 0.2F;
            this.txtReceiptAmountTotal.Left = 6.15F;
            this.txtReceiptAmountTotal.MultiLine = false;
            this.txtReceiptAmountTotal.Name = "txtReceiptAmountTotal";
            this.txtReceiptAmountTotal.OutputFormat = resources.GetString("txtReceiptAmountTotal.OutputFormat");
            this.txtReceiptAmountTotal.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtReceiptAmountTotal.SummaryGroup = "ghCreateAt";
            this.txtReceiptAmountTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtReceiptAmountTotal.Text = "ReceiptAmount";
            this.txtReceiptAmountTotal.Top = 0F;
            this.txtReceiptAmountTotal.Width = 0.7F;
            // 
            // txtReceiptRemainTotal
            // 
            this.txtReceiptRemainTotal.Height = 0.2F;
            this.txtReceiptRemainTotal.Left = 6.15F;
            this.txtReceiptRemainTotal.MultiLine = false;
            this.txtReceiptRemainTotal.Name = "txtReceiptRemainTotal";
            this.txtReceiptRemainTotal.OutputFormat = resources.GetString("txtReceiptRemainTotal.OutputFormat");
            this.txtReceiptRemainTotal.Style = "font-size: 6pt; text-align: right; vertical-align: middle";
            this.txtReceiptRemainTotal.SummaryGroup = "ghCreateAt";
            this.txtReceiptRemainTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtReceiptRemainTotal.Text = "ReceiptRemain";
            this.txtReceiptRemainTotal.Top = 0.197F;
            this.txtReceiptRemainTotal.Width = 0.7F;
            // 
            // line4
            // 
            this.line4.Height = 0F;
            this.line4.Left = 0F;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.Top = 0F;
            this.line4.Width = 10.63F;
            this.line4.X1 = 0F;
            this.line4.X2 = 10.63F;
            this.line4.Y1 = 0F;
            this.line4.Y2 = 0F;
            // 
            // line5
            // 
            this.line5.Height = 0F;
            this.line5.Left = 0.01417323F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0.4F;
            this.line5.Width = 10.61583F;
            this.line5.X1 = 0.01417323F;
            this.line5.X2 = 10.63F;
            this.line5.Y1 = 0.4F;
            this.line5.Y2 = 0.4F;
            // 
            // line6
            // 
            this.line6.Height = 0.3968504F;
            this.line6.Left = 3.4F;
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.Top = 0F;
            this.line6.Width = 0F;
            this.line6.X1 = 3.4F;
            this.line6.X2 = 3.4F;
            this.line6.Y1 = 0F;
            this.line6.Y2 = 0.3968504F;
            // 
            // line7
            // 
            this.line7.Height = 0.3968504F;
            this.line7.Left = 4.15F;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.Top = 0F;
            this.line7.Width = 0F;
            this.line7.X1 = 4.15F;
            this.line7.X2 = 4.15F;
            this.line7.Y1 = 0F;
            this.line7.Y2 = 0.3968504F;
            // 
            // line21
            // 
            this.line21.Height = 0.4F;
            this.line21.Left = 4.9F;
            this.line21.LineWeight = 1F;
            this.line21.Name = "line21";
            this.line21.Top = 0F;
            this.line21.Width = 0F;
            this.line21.X1 = 4.9F;
            this.line21.X2 = 4.9F;
            this.line21.Y1 = 0F;
            this.line21.Y2 = 0.4F;
            // 
            // line22
            // 
            this.line22.Height = 0.4F;
            this.line22.Left = 6F;
            this.line22.LineWeight = 1F;
            this.line22.Name = "line22";
            this.line22.Top = 0F;
            this.line22.Width = 0F;
            this.line22.X1 = 6F;
            this.line22.X2 = 6F;
            this.line22.Y1 = 0F;
            this.line22.Y2 = 0.4F;
            // 
            // line23
            // 
            this.line23.Height = 0.4F;
            this.line23.Left = 6.85F;
            this.line23.LineWeight = 1F;
            this.line23.Name = "line23";
            this.line23.Top = 0F;
            this.line23.Width = 0F;
            this.line23.X1 = 6.85F;
            this.line23.X2 = 6.85F;
            this.line23.Y1 = 0F;
            this.line23.Y2 = 0.4F;
            // 
            // line40
            // 
            this.line40.Height = 0.397F;
            this.line40.Left = 9.93F;
            this.line40.LineWeight = 1F;
            this.line40.Name = "line40";
            this.line40.Top = 0F;
            this.line40.Width = 0F;
            this.line40.X1 = 9.93F;
            this.line40.X2 = 9.93F;
            this.line40.Y1 = 0F;
            this.line40.Y2 = 0.397F;
            // 
            // line41
            // 
            this.line41.Height = 0.4F;
            this.line41.Left = 4.93F;
            this.line41.LineWeight = 1F;
            this.line41.Name = "line41";
            this.line41.Top = 0F;
            this.line41.Width = 0F;
            this.line41.X1 = 4.93F;
            this.line41.X2 = 4.93F;
            this.line41.Y1 = 0F;
            this.line41.Y2 = 0.4F;
            // 
            // MatchingHistorySectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.63F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.ghCreateAt);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.gfCreateAt);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            this.ReportStart += new System.EventHandler(this.MatchingHistorySectionReport_ReportStart);
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProcessType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBeforeAccept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcessType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAtTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAtTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProcessTypeTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingRemainTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptRemainTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblcompanycode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbltitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingRemain;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptRemain;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblLoginUser;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblProcessType;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
        private GrapeCity.ActiveReports.SectionReportModel.Line line8;
        private GrapeCity.ActiveReports.SectionReportModel.Line line9;
        private GrapeCity.ActiveReports.SectionReportModel.Line line10;
        private GrapeCity.ActiveReports.SectionReportModel.Line line11;
        private GrapeCity.ActiveReports.SectionReportModel.Line line12;
        private GrapeCity.ActiveReports.SectionReportModel.Line line13;
        private GrapeCity.ActiveReports.SectionReportModel.Line line14;
        private GrapeCity.ActiveReports.SectionReportModel.Line line15;
        private GrapeCity.ActiveReports.SectionReportModel.Line line16;
        private GrapeCity.ActiveReports.SectionReportModel.Line line17;
        private GrapeCity.ActiveReports.SectionReportModel.Line line18;
        private GrapeCity.ActiveReports.SectionReportModel.Line line19;
        private GrapeCity.ActiveReports.SectionReportModel.Line line20;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCategory;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptCategory;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLoginUser;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtProcessType;
        private GrapeCity.ActiveReports.SectionReportModel.Line line26;
        private GrapeCity.ActiveReports.SectionReportModel.Line line27;
        private GrapeCity.ActiveReports.SectionReportModel.Line line28;
        private GrapeCity.ActiveReports.SectionReportModel.Line line29;
        private GrapeCity.ActiveReports.SectionReportModel.Line line30;
        private GrapeCity.ActiveReports.SectionReportModel.Line line31;
        private GrapeCity.ActiveReports.SectionReportModel.Line line32;
        private GrapeCity.ActiveReports.SectionReportModel.Line line33;
        private GrapeCity.ActiveReports.SectionReportModel.Line line34;
        private GrapeCity.ActiveReports.SectionReportModel.Line line35;
        private GrapeCity.ActiveReports.SectionReportModel.Line line36;
        private GrapeCity.ActiveReports.SectionReportModel.Line line37;
        private GrapeCity.ActiveReports.SectionReportModel.Line line38;
        private GrapeCity.ActiveReports.SectionReportModel.Line line39;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBeforeAccept;
        private GrapeCity.ActiveReports.SectionReportModel.Line line24;
        private GrapeCity.ActiveReports.SectionReportModel.Line line25;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.Shape shape1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLoginUserTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtProcessTypeTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingRemainTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptRemainTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line line4;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        private GrapeCity.ActiveReports.SectionReportModel.Line line6;
        private GrapeCity.ActiveReports.SectionReportModel.Line line7;
        private GrapeCity.ActiveReports.SectionReportModel.Line line21;
        private GrapeCity.ActiveReports.SectionReportModel.Line line22;
        private GrapeCity.ActiveReports.SectionReportModel.Line line23;
        private GrapeCity.ActiveReports.SectionReportModel.Line line40;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreateAtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreateAtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line line41;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
    }
}
