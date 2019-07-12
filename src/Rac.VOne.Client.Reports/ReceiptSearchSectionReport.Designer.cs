namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// PaymentDataSectionReport の概要の説明です。
    /// </summary>
    partial class ReceiptSearchSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReceiptSearchSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblcompanycode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lbltitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblId = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAssignmentFlg = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPaymentCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInputType = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPayerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblExcludeCategory = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblExcludeAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label14 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSourceBranch = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBankCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBranchCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line10 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line11 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line13 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorReceiptId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblSection = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptId = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAssignmentFlag = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPaymentCategory = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInputType = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPayerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtExcludeCategory = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtExcludeAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSourceBankName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSourceBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line16 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line17 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line18 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line19 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line20 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line24 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtBankName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line22 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line26 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line27 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSectionName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line21 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtReceiptAmtTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.label20 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtRemainAmtTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtExcludeAmtTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line30 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line15 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line23 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line29 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAssignmentFlg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPaymentCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceBranch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAssignmentFlag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmtTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmtTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeAmtTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
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
            this.lblId,
            this.lblAssignmentFlg,
            this.lblRecordedAt,
            this.lblDueAt,
            this.lblPaymentCategoryCode,
            this.lblInputType,
            this.lblPayerName,
            this.lblNote,
            this.lblReceiptAmount,
            this.lblRemainAmount,
            this.lblExcludeCategory,
            this.lblExcludeAmount,
            this.label14,
            this.lblSourceBranch,
            this.lblAccountNumber,
            this.lblBankCodeName,
            this.lblBranchCodeName,
            this.line2,
            this.line4,
            this.line5,
            this.line6,
            this.line7,
            this.line9,
            this.line10,
            this.line11,
            this.line12,
            this.line13,
            this.lineHeaderHorReceiptId,
            this.lblSection,
            this.line3,
            this.line1,
            this.lineHeaderHorReceiptAmount,
            this.line8,
            this.lineHeaderVerPayerName});
            this.pageHeader.Height = 1.200063F;
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
            this.lbldate.Text = "出力日付：";
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
            this.lbltitle.Text = "入金データ一覧";
            this.lbltitle.Top = 0.2704725F;
            this.lbltitle.Width = 10.6F;
            // 
            // lblId
            // 
            this.lblId.Height = 0.2F;
            this.lblId.HyperLink = null;
            this.lblId.Left = 0F;
            this.lblId.Name = "lblId";
            this.lblId.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblId.Text = "入金ID";
            this.lblId.Top = 0.8F;
            this.lblId.Width = 0.8F;
            // 
            // lblAssignmentFlg
            // 
            this.lblAssignmentFlg.Height = 0.2F;
            this.lblAssignmentFlg.HyperLink = null;
            this.lblAssignmentFlg.Left = 0F;
            this.lblAssignmentFlg.Name = "lblAssignmentFlg";
            this.lblAssignmentFlg.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAssignmentFlg.Text = "消込区分";
            this.lblAssignmentFlg.Top = 1F;
            this.lblAssignmentFlg.Width = 0.8F;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Height = 0.2F;
            this.lblRecordedAt.HyperLink = null;
            this.lblRecordedAt.Left = 0.8F;
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblRecordedAt.Text = "入金日";
            this.lblRecordedAt.Top = 0.8F;
            this.lblRecordedAt.Width = 0.6F;
            // 
            // lblDueAt
            // 
            this.lblDueAt.Height = 0.2F;
            this.lblDueAt.HyperLink = null;
            this.lblDueAt.Left = 0.8F;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDueAt.Text = "期日";
            this.lblDueAt.Top = 1F;
            this.lblDueAt.Width = 0.6F;
            // 
            // lblPaymentCategoryCode
            // 
            this.lblPaymentCategoryCode.Height = 0.2F;
            this.lblPaymentCategoryCode.HyperLink = null;
            this.lblPaymentCategoryCode.Left = 1.4F;
            this.lblPaymentCategoryCode.Name = "lblPaymentCategoryCode";
            this.lblPaymentCategoryCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblPaymentCategoryCode.Text = "入金区分";
            this.lblPaymentCategoryCode.Top = 0.8F;
            this.lblPaymentCategoryCode.Width = 0.9F;
            // 
            // lblInputType
            // 
            this.lblInputType.Height = 0.2F;
            this.lblInputType.HyperLink = null;
            this.lblInputType.Left = 1.4F;
            this.lblInputType.Name = "lblInputType";
            this.lblInputType.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblInputType.Text = "入力区分";
            this.lblInputType.Top = 1F;
            this.lblInputType.Width = 0.9F;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Height = 0.2F;
            this.lblPayerName.HyperLink = null;
            this.lblPayerName.Left = 2.3F;
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.Top = 0.8F;
            this.lblPayerName.Width = 1.6F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.2F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 2.3F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblNote.Text = "備考";
            this.lblNote.Top = 1F;
            this.lblNote.Width = 1.6F;
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Height = 0.2F;
            this.lblReceiptAmount.HyperLink = null;
            this.lblReceiptAmount.Left = 4.6F;
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptAmount.Text = "入金額";
            this.lblReceiptAmount.Top = 0.8F;
            this.lblReceiptAmount.Width = 1F;
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Height = 0.2F;
            this.lblRemainAmount.HyperLink = null;
            this.lblRemainAmount.Left = 4.6F;
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblRemainAmount.Text = "入金残";
            this.lblRemainAmount.Top = 1F;
            this.lblRemainAmount.Width = 1F;
            // 
            // lblExcludeCategory
            // 
            this.lblExcludeCategory.Height = 0.2F;
            this.lblExcludeCategory.HyperLink = null;
            this.lblExcludeCategory.Left = 5.6F;
            this.lblExcludeCategory.Name = "lblExcludeCategory";
            this.lblExcludeCategory.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblExcludeCategory.Text = "対象外区分";
            this.lblExcludeCategory.Top = 0.8F;
            this.lblExcludeCategory.Width = 1.2F;
            // 
            // lblExcludeAmount
            // 
            this.lblExcludeAmount.Height = 0.2F;
            this.lblExcludeAmount.HyperLink = null;
            this.lblExcludeAmount.Left = 5.6F;
            this.lblExcludeAmount.Name = "lblExcludeAmount";
            this.lblExcludeAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblExcludeAmount.Text = "対象外金額";
            this.lblExcludeAmount.Top = 1F;
            this.lblExcludeAmount.Width = 1.2F;
            // 
            // label14
            // 
            this.label14.Height = 0.2F;
            this.label14.HyperLink = null;
            this.label14.Left = 9.700001F;
            this.label14.Name = "label14";
            this.label14.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label14.Text = "仕向銀行";
            this.label14.Top = 0.8F;
            this.label14.Width = 0.9F;
            // 
            // lblSourceBranch
            // 
            this.lblSourceBranch.Height = 0.2F;
            this.lblSourceBranch.HyperLink = null;
            this.lblSourceBranch.Left = 9.700001F;
            this.lblSourceBranch.Name = "lblSourceBranch";
            this.lblSourceBranch.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblSourceBranch.Text = "仕向支店";
            this.lblSourceBranch.Top = 1F;
            this.lblSourceBranch.Width = 0.9F;
            // 
            // lblAccountNumber
            // 
            this.lblAccountNumber.Height = 0.4F;
            this.lblAccountNumber.HyperLink = null;
            this.lblAccountNumber.Left = 8.8F;
            this.lblAccountNumber.Name = "lblAccountNumber";
            this.lblAccountNumber.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblAccountNumber.Text = "口座番号";
            this.lblAccountNumber.Top = 0.8F;
            this.lblAccountNumber.Width = 0.9F;
            // 
            // lblBankCodeName
            // 
            this.lblBankCodeName.Height = 0.4F;
            this.lblBankCodeName.HyperLink = null;
            this.lblBankCodeName.Left = 6.8F;
            this.lblBankCodeName.Name = "lblBankCodeName";
            this.lblBankCodeName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBankCodeName.Text = "入金銀行";
            this.lblBankCodeName.Top = 0.8F;
            this.lblBankCodeName.Width = 1F;
            // 
            // lblBranchCodeName
            // 
            this.lblBranchCodeName.Height = 0.4F;
            this.lblBranchCodeName.HyperLink = null;
            this.lblBranchCodeName.Left = 7.8F;
            this.lblBranchCodeName.Name = "lblBranchCodeName";
            this.lblBranchCodeName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBranchCodeName.Text = "入金支店";
            this.lblBranchCodeName.Top = 0.8F;
            this.lblBranchCodeName.Width = 1F;
            // 
            // line2
            // 
            this.line2.Height = 0F;
            this.line2.Left = -3.72529E-09F;
            this.line2.LineColor = System.Drawing.Color.WhiteSmoke;
            this.line2.LineWeight = 6F;
            this.line2.Name = "line2";
            this.line2.Top = 1F;
            this.line2.Width = 6.8F;
            this.line2.X1 = -3.72529E-09F;
            this.line2.X2 = 6.8F;
            this.line2.Y1 = 1F;
            this.line2.Y2 = 1F;
            // 
            // line4
            // 
            this.line4.Height = 0F;
            this.line4.Left = 9.700001F;
            this.line4.LineColor = System.Drawing.Color.WhiteSmoke;
            this.line4.LineWeight = 6F;
            this.line4.Name = "line4";
            this.line4.Top = 1F;
            this.line4.Width = 0.8999996F;
            this.line4.X1 = 9.700001F;
            this.line4.X2 = 10.6F;
            this.line4.Y1 = 1F;
            this.line4.Y2 = 1F;
            // 
            // line5
            // 
            this.line5.Height = 0.4F;
            this.line5.Left = 0.8F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0.8F;
            this.line5.Width = 0F;
            this.line5.X1 = 0.8F;
            this.line5.X2 = 0.8F;
            this.line5.Y1 = 0.8F;
            this.line5.Y2 = 1.2F;
            // 
            // line6
            // 
            this.line6.Height = 0.4F;
            this.line6.Left = 1.4F;
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.Top = 0.8F;
            this.line6.Width = 0F;
            this.line6.X1 = 1.4F;
            this.line6.X2 = 1.4F;
            this.line6.Y1 = 0.8F;
            this.line6.Y2 = 1.2F;
            // 
            // line7
            // 
            this.line7.Height = 0.4F;
            this.line7.Left = 2.3F;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.Top = 0.8F;
            this.line7.Width = 0F;
            this.line7.X1 = 2.3F;
            this.line7.X2 = 2.3F;
            this.line7.Y1 = 0.8F;
            this.line7.Y2 = 1.2F;
            // 
            // line9
            // 
            this.line9.Height = 0.4F;
            this.line9.Left = 5.6F;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.Top = 0.8F;
            this.line9.Width = 0F;
            this.line9.X1 = 5.6F;
            this.line9.X2 = 5.6F;
            this.line9.Y1 = 0.8F;
            this.line9.Y2 = 1.2F;
            // 
            // line10
            // 
            this.line10.Height = 0.4F;
            this.line10.Left = 6.8F;
            this.line10.LineWeight = 1F;
            this.line10.Name = "line10";
            this.line10.Top = 0.8F;
            this.line10.Width = 0F;
            this.line10.X1 = 6.8F;
            this.line10.X2 = 6.8F;
            this.line10.Y1 = 0.8F;
            this.line10.Y2 = 1.2F;
            // 
            // line11
            // 
            this.line11.Height = 0.4F;
            this.line11.Left = 7.8F;
            this.line11.LineWeight = 1F;
            this.line11.Name = "line11";
            this.line11.Top = 0.8F;
            this.line11.Width = 0F;
            this.line11.X1 = 7.8F;
            this.line11.X2 = 7.8F;
            this.line11.Y1 = 0.8F;
            this.line11.Y2 = 1.2F;
            // 
            // line12
            // 
            this.line12.Height = 0.4F;
            this.line12.Left = 8.8F;
            this.line12.LineWeight = 1F;
            this.line12.Name = "line12";
            this.line12.Top = 0.8F;
            this.line12.Width = 0F;
            this.line12.X1 = 8.8F;
            this.line12.X2 = 8.8F;
            this.line12.Y1 = 0.8F;
            this.line12.Y2 = 1.2F;
            // 
            // line13
            // 
            this.line13.Height = 0.4F;
            this.line13.Left = 9.700001F;
            this.line13.LineWeight = 1F;
            this.line13.Name = "line13";
            this.line13.Top = 0.8F;
            this.line13.Width = 0F;
            this.line13.X1 = 9.700001F;
            this.line13.X2 = 9.700001F;
            this.line13.Y1 = 0.8F;
            this.line13.Y2 = 1.2F;
            // 
            // lineHeaderHorReceiptId
            // 
            this.lineHeaderHorReceiptId.Height = 0F;
            this.lineHeaderHorReceiptId.Left = 0F;
            this.lineHeaderHorReceiptId.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorReceiptId.LineWeight = 1F;
            this.lineHeaderHorReceiptId.Name = "lineHeaderHorReceiptId";
            this.lineHeaderHorReceiptId.Top = 1F;
            this.lineHeaderHorReceiptId.Width = 3.9F;
            this.lineHeaderHorReceiptId.X1 = 0F;
            this.lineHeaderHorReceiptId.X2 = 3.9F;
            this.lineHeaderHorReceiptId.Y1 = 1F;
            this.lineHeaderHorReceiptId.Y2 = 1F;
            // 
            // lblSection
            // 
            this.lblSection.Height = 0.4F;
            this.lblSection.HyperLink = null;
            this.lblSection.Left = 3.9F;
            this.lblSection.Name = "lblSection";
            this.lblSection.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblSection.Text = "入金部門";
            this.lblSection.Top = 0.8F;
            this.lblSection.Width = 0.7F;
            // 
            // line3
            // 
            this.line3.Height = 0F;
            this.line3.Left = 0F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0.8F;
            this.line3.Width = 10.6F;
            this.line3.X1 = 0F;
            this.line3.X2 = 10.6F;
            this.line3.Y1 = 0.8F;
            this.line3.Y2 = 0.8F;
            // 
            // line1
            // 
            this.line1.Height = 0F;
            this.line1.Left = 0F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 1.2F;
            this.line1.Width = 10.6F;
            this.line1.X1 = 0F;
            this.line1.X2 = 10.6F;
            this.line1.Y1 = 1.2F;
            this.line1.Y2 = 1.2F;
            // 
            // lineHeaderHorReceiptAmount
            // 
            this.lineHeaderHorReceiptAmount.Height = 0F;
            this.lineHeaderHorReceiptAmount.Left = 4.6F;
            this.lineHeaderHorReceiptAmount.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorReceiptAmount.LineWeight = 1F;
            this.lineHeaderHorReceiptAmount.Name = "lineHeaderHorReceiptAmount";
            this.lineHeaderHorReceiptAmount.Top = 1F;
            this.lineHeaderHorReceiptAmount.Width = 2.200078F;
            this.lineHeaderHorReceiptAmount.X1 = 4.6F;
            this.lineHeaderHorReceiptAmount.X2 = 6.800078F;
            this.lineHeaderHorReceiptAmount.Y1 = 1F;
            this.lineHeaderHorReceiptAmount.Y2 = 1F;
            // 
            // line8
            // 
            this.line8.Height = 0.4F;
            this.line8.Left = 4.6F;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.Top = 0.8F;
            this.line8.Width = 0F;
            this.line8.X1 = 4.6F;
            this.line8.X2 = 4.6F;
            this.line8.Y1 = 0.8F;
            this.line8.Y2 = 1.2F;
            // 
            // lineHeaderVerPayerName
            // 
            this.lineHeaderVerPayerName.Height = 0.399638F;
            this.lineHeaderVerPayerName.Left = 3.9F;
            this.lineHeaderVerPayerName.LineWeight = 1F;
            this.lineHeaderVerPayerName.Name = "lineHeaderVerPayerName";
            this.lineHeaderVerPayerName.Top = 0.8F;
            this.lineHeaderVerPayerName.Width = 0F;
            this.lineHeaderVerPayerName.X1 = 3.9F;
            this.lineHeaderVerPayerName.X2 = 3.9F;
            this.lineHeaderVerPayerName.Y1 = 0.8F;
            this.lineHeaderVerPayerName.Y2 = 1.199638F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtRecordedAt,
            this.txtDueAt,
            this.txtReceiptId,
            this.txtAssignmentFlag,
            this.txtPaymentCategory,
            this.txtInputType,
            this.txtPayerName,
            this.txtNote1,
            this.txtReceiptAmount,
            this.txtRemainAmount,
            this.txtExcludeCategory,
            this.txtExcludeAmount,
            this.txtSourceBankName,
            this.txtSourceBranchName,
            this.txtBankCode,
            this.txtBranchCode,
            this.txtAccountNumber,
            this.line16,
            this.line17,
            this.line18,
            this.line19,
            this.line20,
            this.line24,
            this.txtBankName,
            this.txtBranchName,
            this.line22,
            this.line26,
            this.line27,
            this.txtSectionCode,
            this.txtSectionName,
            this.lineDetailVerPayerName,
            this.line21});
            this.detail.Height = 0.3999999F;
            this.detail.Name = "detail";
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.2F;
            this.txtRecordedAt.Left = 0.8F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtRecordedAt.Text = "txtRecordedAt";
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.6F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.Height = 0.2F;
            this.txtDueAt.Left = 0.8F;
            this.txtDueAt.MultiLine = false;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtDueAt.Text = "txtDueAt";
            this.txtDueAt.Top = 0.2F;
            this.txtDueAt.Width = 0.6F;
            // 
            // txtReceiptId
            // 
            this.txtReceiptId.Height = 0.2F;
            this.txtReceiptId.Left = 0F;
            this.txtReceiptId.MultiLine = false;
            this.txtReceiptId.Name = "txtReceiptId";
            this.txtReceiptId.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtReceiptId.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptId.Text = "txtReceiptId";
            this.txtReceiptId.Top = 0F;
            this.txtReceiptId.Width = 0.78F;
            // 
            // txtAssignmentFlag
            // 
            this.txtAssignmentFlag.Height = 0.2F;
            this.txtAssignmentFlag.Left = 0F;
            this.txtAssignmentFlag.MultiLine = false;
            this.txtAssignmentFlag.Name = "txtAssignmentFlag";
            this.txtAssignmentFlag.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtAssignmentFlag.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtAssignmentFlag.Text = "txtAssiFlag";
            this.txtAssignmentFlag.Top = 0.2F;
            this.txtAssignmentFlag.Width = 0.8F;
            // 
            // txtPaymentCategory
            // 
            this.txtPaymentCategory.Height = 0.2F;
            this.txtPaymentCategory.Left = 1.42F;
            this.txtPaymentCategory.MultiLine = false;
            this.txtPaymentCategory.Name = "txtPaymentCategory";
            this.txtPaymentCategory.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtPaymentCategory.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtPaymentCategory.Text = "txtPaymentCategory";
            this.txtPaymentCategory.Top = 0F;
            this.txtPaymentCategory.Width = 0.88F;
            // 
            // txtInputType
            // 
            this.txtInputType.Height = 0.2F;
            this.txtInputType.Left = 1.42F;
            this.txtInputType.MultiLine = false;
            this.txtInputType.Name = "txtInputType";
            this.txtInputType.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtInputType.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtInputType.Text = "txtInputType";
            this.txtInputType.Top = 0.2F;
            this.txtInputType.Width = 0.88F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.2F;
            this.txtPayerName.Left = 2.32F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtPayerName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtPayerName.Text = "txtPayerName";
            this.txtPayerName.Top = 0F;
            this.txtPayerName.Width = 1.58F;
            // 
            // txtNote1
            // 
            this.txtNote1.Height = 0.2F;
            this.txtNote1.Left = 2.32F;
            this.txtNote1.MultiLine = false;
            this.txtNote1.Name = "txtNote1";
            this.txtNote1.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtNote1.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtNote1.Text = "txtNote1";
            this.txtNote1.Top = 0.2F;
            this.txtNote1.Width = 1.58F;
            // 
            // txtReceiptAmount
            // 
            this.txtReceiptAmount.Height = 0.2F;
            this.txtReceiptAmount.Left = 4.6F;
            this.txtReceiptAmount.MultiLine = false;
            this.txtReceiptAmount.Name = "txtReceiptAmount";
            this.txtReceiptAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtReceiptAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptAmount.Text = "txtReceiptAmt";
            this.txtReceiptAmount.Top = -1.164153E-10F;
            this.txtReceiptAmount.Width = 0.98F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.Height = 0.2F;
            this.txtRemainAmount.Left = 4.6F;
            this.txtRemainAmount.MultiLine = false;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtRemainAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtRemainAmount.Text = "txtRemainAmt";
            this.txtRemainAmount.Top = 0.2F;
            this.txtRemainAmount.Width = 0.98F;
            // 
            // txtExcludeCategory
            // 
            this.txtExcludeCategory.Height = 0.2F;
            this.txtExcludeCategory.Left = 5.62F;
            this.txtExcludeCategory.MultiLine = false;
            this.txtExcludeCategory.Name = "txtExcludeCategory";
            this.txtExcludeCategory.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtExcludeCategory.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtExcludeCategory.Text = "txtExcludeCategory";
            this.txtExcludeCategory.Top = 0F;
            this.txtExcludeCategory.Width = 1.18F;
            // 
            // txtExcludeAmount
            // 
            this.txtExcludeAmount.Height = 0.2F;
            this.txtExcludeAmount.Left = 5.6F;
            this.txtExcludeAmount.MultiLine = false;
            this.txtExcludeAmount.Name = "txtExcludeAmount";
            this.txtExcludeAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtExcludeAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtExcludeAmount.Text = "txtExcludeAmount";
            this.txtExcludeAmount.Top = 0.2F;
            this.txtExcludeAmount.Width = 1.18F;
            // 
            // txtSourceBankName
            // 
            this.txtSourceBankName.Height = 0.2F;
            this.txtSourceBankName.Left = 9.72F;
            this.txtSourceBankName.MultiLine = false;
            this.txtSourceBankName.Name = "txtSourceBankName";
            this.txtSourceBankName.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtSourceBankName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtSourceBankName.Text = "txtSourceBankName";
            this.txtSourceBankName.Top = 0F;
            this.txtSourceBankName.Width = 0.88F;
            // 
            // txtSourceBranchName
            // 
            this.txtSourceBranchName.Height = 0.2F;
            this.txtSourceBranchName.Left = 9.72F;
            this.txtSourceBranchName.MultiLine = false;
            this.txtSourceBranchName.Name = "txtSourceBranchName";
            this.txtSourceBranchName.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtSourceBranchName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtSourceBranchName.Text = "txtSourceBranchName";
            this.txtSourceBranchName.Top = 0.2F;
            this.txtSourceBranchName.Width = 0.88F;
            // 
            // txtBankCode
            // 
            this.txtBankCode.Height = 0.2F;
            this.txtBankCode.Left = 6.82F;
            this.txtBankCode.MultiLine = false;
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtBankCode.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtBankCode.Text = "txtBankCode";
            this.txtBankCode.Top = 0F;
            this.txtBankCode.Width = 0.98F;
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.Height = 0.2F;
            this.txtBranchCode.Left = 7.82F;
            this.txtBranchCode.MultiLine = false;
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtBranchCode.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtBranchCode.Text = "txtBranchCode";
            this.txtBranchCode.Top = 0F;
            this.txtBranchCode.Width = 0.98F;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Height = 0.4F;
            this.txtAccountNumber.Left = 8.8F;
            this.txtAccountNumber.MultiLine = false;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtAccountNumber.Text = "txtAccountNumber";
            this.txtAccountNumber.Top = 0F;
            this.txtAccountNumber.Width = 0.9F;
            // 
            // line16
            // 
            this.line16.Height = 0.4F;
            this.line16.Left = 0.8F;
            this.line16.LineWeight = 1F;
            this.line16.Name = "line16";
            this.line16.Top = 0F;
            this.line16.Width = 0F;
            this.line16.X1 = 0.8F;
            this.line16.X2 = 0.8F;
            this.line16.Y1 = 0F;
            this.line16.Y2 = 0.4F;
            // 
            // line17
            // 
            this.line17.Height = 0.4F;
            this.line17.Left = 1.4F;
            this.line17.LineWeight = 1F;
            this.line17.Name = "line17";
            this.line17.Top = 0F;
            this.line17.Width = 0F;
            this.line17.X1 = 1.4F;
            this.line17.X2 = 1.4F;
            this.line17.Y1 = 0F;
            this.line17.Y2 = 0.4F;
            // 
            // line18
            // 
            this.line18.Height = 0.4F;
            this.line18.Left = 2.3F;
            this.line18.LineWeight = 1F;
            this.line18.Name = "line18";
            this.line18.Top = 0F;
            this.line18.Width = 0F;
            this.line18.X1 = 2.3F;
            this.line18.X2 = 2.3F;
            this.line18.Y1 = 0F;
            this.line18.Y2 = 0.4F;
            // 
            // line19
            // 
            this.line19.Height = 0.4F;
            this.line19.Left = 4.6F;
            this.line19.LineWeight = 1F;
            this.line19.Name = "line19";
            this.line19.Top = 0F;
            this.line19.Width = 0.0001182556F;
            this.line19.X1 = 4.6F;
            this.line19.X2 = 4.600118F;
            this.line19.Y1 = 0F;
            this.line19.Y2 = 0.4F;
            // 
            // line20
            // 
            this.line20.Height = 0.4F;
            this.line20.Left = 5.6F;
            this.line20.LineWeight = 1F;
            this.line20.Name = "line20";
            this.line20.Top = 0F;
            this.line20.Width = 0.0001182556F;
            this.line20.X1 = 5.6F;
            this.line20.X2 = 5.600118F;
            this.line20.Y1 = 0F;
            this.line20.Y2 = 0.4F;
            // 
            // line24
            // 
            this.line24.Height = 0.395F;
            this.line24.Left = 9.700001F;
            this.line24.LineWeight = 1F;
            this.line24.Name = "line24";
            this.line24.Top = 0F;
            this.line24.Width = 0F;
            this.line24.X1 = 9.700001F;
            this.line24.X2 = 9.700001F;
            this.line24.Y1 = 0F;
            this.line24.Y2 = 0.395F;
            // 
            // txtBankName
            // 
            this.txtBankName.Height = 0.2F;
            this.txtBankName.Left = 6.82F;
            this.txtBankName.MultiLine = false;
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtBankName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtBankName.Text = "txtBankName";
            this.txtBankName.Top = 0.2F;
            this.txtBankName.Width = 0.98F;
            // 
            // txtBranchName
            // 
            this.txtBranchName.Height = 0.2F;
            this.txtBranchName.Left = 7.82F;
            this.txtBranchName.MultiLine = false;
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtBranchName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtBranchName.Text = "txtBranchName";
            this.txtBranchName.Top = 0.195F;
            this.txtBranchName.Width = 0.98F;
            // 
            // line22
            // 
            this.line22.Height = 0.4F;
            this.line22.Left = 7.8F;
            this.line22.LineWeight = 1F;
            this.line22.Name = "line22";
            this.line22.Top = 0F;
            this.line22.Width = 9.536743E-07F;
            this.line22.X1 = 7.8F;
            this.line22.X2 = 7.800001F;
            this.line22.Y1 = 0F;
            this.line22.Y2 = 0.4F;
            // 
            // line26
            // 
            this.line26.Height = 0.4F;
            this.line26.Left = 8.8F;
            this.line26.LineWeight = 1F;
            this.line26.Name = "line26";
            this.line26.Top = 0F;
            this.line26.Width = 0F;
            this.line26.X1 = 8.8F;
            this.line26.X2 = 8.8F;
            this.line26.Y1 = 0F;
            this.line26.Y2 = 0.4F;
            // 
            // line27
            // 
            this.line27.Height = 0.395F;
            this.line27.Left = 6.8F;
            this.line27.LineWeight = 1F;
            this.line27.Name = "line27";
            this.line27.Top = 0F;
            this.line27.Width = 0.000181675F;
            this.line27.X1 = 6.8F;
            this.line27.X2 = 6.800182F;
            this.line27.Y1 = 0F;
            this.line27.Y2 = 0.395F;
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.Height = 0.2F;
            this.txtSectionCode.Left = 3.92F;
            this.txtSectionCode.MultiLine = false;
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtSectionCode.Text = "txtSectionCode";
            this.txtSectionCode.Top = 0.005F;
            this.txtSectionCode.Width = 0.68F;
            // 
            // txtSectionName
            // 
            this.txtSectionName.Height = 0.2F;
            this.txtSectionName.Left = 3.92F;
            this.txtSectionName.MultiLine = false;
            this.txtSectionName.Name = "txtSectionName";
            this.txtSectionName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtSectionName.Text = "txtSectionName";
            this.txtSectionName.Top = 0.2F;
            this.txtSectionName.Width = 0.68F;
            // 
            // lineDetailVerPayerName
            // 
            this.lineDetailVerPayerName.Height = 0.395F;
            this.lineDetailVerPayerName.Left = 3.9F;
            this.lineDetailVerPayerName.LineWeight = 1F;
            this.lineDetailVerPayerName.Name = "lineDetailVerPayerName";
            this.lineDetailVerPayerName.Top = 0.005F;
            this.lineDetailVerPayerName.Width = 0F;
            this.lineDetailVerPayerName.X1 = 3.9F;
            this.lineDetailVerPayerName.X2 = 3.9F;
            this.lineDetailVerPayerName.Y1 = 0.005F;
            this.lineDetailVerPayerName.Y2 = 0.4F;
            // 
            // line21
            // 
            this.line21.Height = 0F;
            this.line21.Left = 0F;
            this.line21.LineWeight = 1F;
            this.line21.Name = "line21";
            this.line21.Top = 0.4F;
            this.line21.Width = 10.6F;
            this.line21.X1 = 0F;
            this.line21.X2 = 10.6F;
            this.line21.Y1 = 0.4F;
            this.line21.Y2 = 0.4F;
            // 
            // lblTotal
            // 
            this.lblTotal.Height = 0.4F;
            this.lblTotal.HyperLink = null;
            this.lblTotal.Left = 0F;
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblTotal.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblTotal.Text = "総合計";
            this.lblTotal.Top = 0F;
            this.lblTotal.Width = 4.6F;
            // 
            // txtReceiptAmtTotal
            // 
            this.txtReceiptAmtTotal.DataField = "ReceiptAmount";
            this.txtReceiptAmtTotal.Height = 0.2F;
            this.txtReceiptAmtTotal.Left = 4.6F;
            this.txtReceiptAmtTotal.Name = "txtReceiptAmtTotal";
            this.txtReceiptAmtTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtReceiptAmtTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtReceiptAmtTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtReceiptAmtTotal.Text = "txtReceiptAmtTotal";
            this.txtReceiptAmtTotal.Top = 0F;
            this.txtReceiptAmtTotal.Width = 1F;
            // 
            // label20
            // 
            this.label20.Height = 0.4F;
            this.label20.HyperLink = null;
            this.label20.Left = 6.8F;
            this.label20.Name = "label20";
            this.label20.Style = "background-color: WhiteSmoke";
            this.label20.Text = "";
            this.label20.Top = 0F;
            this.label20.Width = 3.8F;
            // 
            // txtRemainAmtTotal
            // 
            this.txtRemainAmtTotal.DataField = "RemainAmount";
            this.txtRemainAmtTotal.Height = 0.2F;
            this.txtRemainAmtTotal.Left = 4.6F;
            this.txtRemainAmtTotal.Name = "txtRemainAmtTotal";
            this.txtRemainAmtTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtRemainAmtTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtRemainAmtTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtRemainAmtTotal.Text = "txtRemainAmtTotal";
            this.txtRemainAmtTotal.Top = 0.2F;
            this.txtRemainAmtTotal.Width = 1F;
            // 
            // txtExcludeAmtTotal
            // 
            this.txtExcludeAmtTotal.DataField = "ExcludeAmount";
            this.txtExcludeAmtTotal.Height = 0.4F;
            this.txtExcludeAmtTotal.Left = 5.6F;
            this.txtExcludeAmtTotal.Name = "txtExcludeAmtTotal";
            this.txtExcludeAmtTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtExcludeAmtTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtExcludeAmtTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtExcludeAmtTotal.Text = "txtExcludeAmtTotal";
            this.txtExcludeAmtTotal.Top = 0F;
            this.txtExcludeAmtTotal.Width = 1.2F;
            // 
            // line30
            // 
            this.line30.Height = 0F;
            this.line30.Left = 0F;
            this.line30.LineWeight = 1F;
            this.line30.Name = "line30";
            this.line30.Top = 0.4F;
            this.line30.Width = 10.6F;
            this.line30.X1 = 0F;
            this.line30.X2 = 10.6F;
            this.line30.Y1 = 0.4F;
            this.line30.Y2 = 0.4F;
            // 
            // line15
            // 
            this.line15.Height = 0.4F;
            this.line15.Left = 4.6F;
            this.line15.LineWeight = 1F;
            this.line15.Name = "line15";
            this.line15.Top = 0F;
            this.line15.Width = 0F;
            this.line15.X1 = 4.6F;
            this.line15.X2 = 4.6F;
            this.line15.Y1 = 0F;
            this.line15.Y2 = 0.4F;
            // 
            // line23
            // 
            this.line23.Height = 0.4F;
            this.line23.Left = 5.6F;
            this.line23.LineWeight = 1F;
            this.line23.Name = "line23";
            this.line23.Top = 0F;
            this.line23.Width = 0F;
            this.line23.X1 = 5.6F;
            this.line23.X2 = 5.6F;
            this.line23.Y1 = 0F;
            this.line23.Y2 = 0.4F;
            // 
            // line29
            // 
            this.line29.Height = 0.4F;
            this.line29.Left = 6.8F;
            this.line29.LineWeight = 1F;
            this.line29.Name = "line29";
            this.line29.Top = 0F;
            this.line29.Width = 0.0001807213F;
            this.line29.X1 = 6.8F;
            this.line29.X2 = 6.800181F;
            this.line29.Y1 = 0F;
            this.line29.Y2 = 0.4F;
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
            this.reportInfo1.Left = 7.287008F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.03937008F;
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
            this.lblPageNumber.Width = 10.6F;
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
            this.txtReceiptAmtTotal,
            this.label20,
            this.txtRemainAmtTotal,
            this.txtExcludeAmtTotal,
            this.line30,
            this.line15,
            this.line23,
            this.line29});
            this.groupFooter1.Height = 0.4005831F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // ReceiptSearchSectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.6F;
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
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAssignmentFlg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPaymentCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceBranch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAssignmentFlag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmtTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmtTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeAmtTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblcompanycode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblId;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAssignmentFlg;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPaymentCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInputType;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblExcludeCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblExcludeAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label label14;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSourceBranch;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranchCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
        private GrapeCity.ActiveReports.SectionReportModel.Line line4;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        private GrapeCity.ActiveReports.SectionReportModel.Line line6;
        private GrapeCity.ActiveReports.SectionReportModel.Line line7;
        private GrapeCity.ActiveReports.SectionReportModel.Line line8;
        private GrapeCity.ActiveReports.SectionReportModel.Line line9;
        private GrapeCity.ActiveReports.SectionReportModel.Line line10;
        private GrapeCity.ActiveReports.SectionReportModel.Line line11;
        private GrapeCity.ActiveReports.SectionReportModel.Line line12;
        private GrapeCity.ActiveReports.SectionReportModel.Line line13;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAssignmentFlag;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPaymentCategory;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInputType;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtExcludeCategory;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtExcludeAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSourceBankName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSourceBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line line16;
        private GrapeCity.ActiveReports.SectionReportModel.Line line17;
        private GrapeCity.ActiveReports.SectionReportModel.Line line18;
        private GrapeCity.ActiveReports.SectionReportModel.Line line19;
        private GrapeCity.ActiveReports.SectionReportModel.Line line20;
        private GrapeCity.ActiveReports.SectionReportModel.Line line24;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.Line line22;
        private GrapeCity.ActiveReports.SectionReportModel.Line line26;
        private GrapeCity.ActiveReports.SectionReportModel.Line line27;
        private GrapeCity.ActiveReports.SectionReportModel.Line line21;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label label20;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtExcludeAmtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line line30;
        private GrapeCity.ActiveReports.SectionReportModel.Line line15;
        private GrapeCity.ActiveReports.SectionReportModel.Line line23;
        private GrapeCity.ActiveReports.SectionReportModel.Line line29;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSection;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionName;
        public GrapeCity.ActiveReports.SectionReportModel.Label lbltitle;
    }
}
