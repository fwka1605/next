namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ReceiptSectionReport の概要の説明です。
    /// </summary>
    partial class ReceiptOmitSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReceiptOmitSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblSourceBranch = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSourceBank = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptId = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPayerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPaymentCategory = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDeleteAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSaletAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInputType = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSection = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblExcludeCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblExcludeAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBankCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAccount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorReceiptId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerReceiptId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerPaymentCategory = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSection = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerExcludeCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBankCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAccount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorSourceBank = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtExcludeCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerExcludeCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtReceiptId = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDeleteAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSaleAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPaymentCategory = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInputType = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPayerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSectionName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtExcludeAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSourceBankName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSourceBranchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerReceiptId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerPaymentCategory = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSectionCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBankCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerAccount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtReceiptTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblSpace1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtExcludeAmtTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblSpace2 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtRemainTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerReceiptAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerExcludeTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceBranch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPaymentCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDeleteAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSaletAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeleteAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBankName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBranchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeAmtTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblSourceBranch,
            this.lblSourceBank,
            this.lblRecordedAt,
            this.lblReceiptId,
            this.lblPayerName,
            this.lblPaymentCategory,
            this.lblCompanyCodeName,
            this.lblCompanyCode,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblDeleteAt,
            this.lblSaletAt,
            this.lblInputType,
            this.lblNote,
            this.lblSection,
            this.lblReceiptAmount,
            this.lblRemainAmount,
            this.lblExcludeCategoryCode,
            this.lblExcludeAmount,
            this.lblBankCode,
            this.lblBranchCode,
            this.lblAccount,
            this.lineHeaderHorUpper,
            this.lineHeaderHorLower,
            this.lineHeaderHorReceiptId,
            this.lineHeaderHorReceiptAmount,
            this.lineHeaderVerReceiptId,
            this.lineHeaderVerRecordedAt,
            this.lineHeaderVerPaymentCategory,
            this.lineHeaderVerPayerName,
            this.lineHeaderVerSection,
            this.lineHeaderVerReceiptAmount,
            this.lineHeaderVerExcludeCategoryCode,
            this.lineHeaderVerBankCode,
            this.lineHeaderVerBranchCode,
            this.lineHeaderVerAccount,
            this.lineHeaderHorSourceBank});
            this.pageHeader.Height = 1.111811F;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.BeforePrint += new System.EventHandler(this.pageHeader_BeforePrint);
            // 
            // lblSourceBranch
            // 
            this.lblSourceBranch.Height = 0.25F;
            this.lblSourceBranch.HyperLink = null;
            this.lblSourceBranch.Left = 9.669292F;
            this.lblSourceBranch.Name = "lblSourceBranch";
            this.lblSourceBranch.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblSourceBranch.Text = "仕向支店";
            this.lblSourceBranch.Top = 0.8598425F;
            this.lblSourceBranch.Width = 0.9606299F;
            // 
            // lblSourceBank
            // 
            this.lblSourceBank.Height = 0.25F;
            this.lblSourceBank.HyperLink = null;
            this.lblSourceBank.Left = 9.669292F;
            this.lblSourceBank.Name = "lblSourceBank";
            this.lblSourceBank.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblSourceBank.Text = "仕向銀行";
            this.lblSourceBank.Top = 0.6181103F;
            this.lblSourceBank.Width = 0.9606299F;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Height = 0.2500001F;
            this.lblRecordedAt.HyperLink = null;
            this.lblRecordedAt.Left = 0.7440946F;
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblRecordedAt.Text = "入金日";
            this.lblRecordedAt.Top = 0.6181103F;
            this.lblRecordedAt.Width = 0.7874016F;
            // 
            // lblReceiptId
            // 
            this.lblReceiptId.Height = 0.2500001F;
            this.lblReceiptId.HyperLink = null;
            this.lblReceiptId.Left = 0F;
            this.lblReceiptId.Name = "lblReceiptId";
            this.lblReceiptId.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblReceiptId.Text = "入金ID";
            this.lblReceiptId.Top = 0.6181102F;
            this.lblReceiptId.Width = 0.738189F;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Height = 0.2500001F;
            this.lblPayerName.HyperLink = null;
            this.lblPayerName.Left = 2.574803F;
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.Top = 0.6181103F;
            this.lblPayerName.Width = 1.248031F;
            // 
            // lblPaymentCategory
            // 
            this.lblPaymentCategory.Height = 0.2500001F;
            this.lblPaymentCategory.HyperLink = null;
            this.lblPaymentCategory.Left = 1.535433F;
            this.lblPaymentCategory.Name = "lblPaymentCategory";
            this.lblPaymentCategory.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblPaymentCategory.Text = "入金区分";
            this.lblPaymentCategory.Top = 0.6181103F;
            this.lblPaymentCategory.Width = 1.03937F;
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.811811F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lblCompanyCodeName.Text = "label2";
            this.lblCompanyCodeName.Top = 0F;
            this.lblCompanyCodeName.Width = 3.657F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.02440945F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lblCompanyCode.Text = "会社コード　：";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 0.7874016F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 8.809055F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
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
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle";
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
            this.lblTitle.Text = "入金未消込削除一覧表";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblDeleteAt
            // 
            this.lblDeleteAt.Height = 0.25F;
            this.lblDeleteAt.HyperLink = null;
            this.lblDeleteAt.Left = 0F;
            this.lblDeleteAt.Name = "lblDeleteAt";
            this.lblDeleteAt.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblDeleteAt.Text = "削除日";
            this.lblDeleteAt.Top = 0.8598425F;
            this.lblDeleteAt.Width = 0.738189F;
            // 
            // lblSaletAt
            // 
            this.lblSaletAt.Height = 0.25F;
            this.lblSaletAt.HyperLink = null;
            this.lblSaletAt.Left = 0.7440946F;
            this.lblSaletAt.Name = "lblSaletAt";
            this.lblSaletAt.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblSaletAt.Text = "期日";
            this.lblSaletAt.Top = 0.8598425F;
            this.lblSaletAt.Width = 0.7874016F;
            // 
            // lblInputType
            // 
            this.lblInputType.Height = 0.25F;
            this.lblInputType.HyperLink = null;
            this.lblInputType.Left = 1.535433F;
            this.lblInputType.Name = "lblInputType";
            this.lblInputType.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblInputType.Text = "入力区分";
            this.lblInputType.Top = 0.8598425F;
            this.lblInputType.Width = 1.03937F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.25F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 2.574803F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblNote.Text = "備考";
            this.lblNote.Top = 0.8598425F;
            this.lblNote.Width = 1.248031F;
            // 
            // lblSection
            // 
            this.lblSection.Height = 0.496063F;
            this.lblSection.HyperLink = null;
            this.lblSection.Left = 3.826772F;
            this.lblSection.Name = "lblSection";
            this.lblSection.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblSection.Text = "入金部門";
            this.lblSection.Top = 0.6181103F;
            this.lblSection.Width = 0.968504F;
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Height = 0.25F;
            this.lblReceiptAmount.HyperLink = null;
            this.lblReceiptAmount.Left = 4.795276F;
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblReceiptAmount.Text = "入金額";
            this.lblReceiptAmount.Top = 0.6181103F;
            this.lblReceiptAmount.Width = 1.267717F;
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Height = 0.25F;
            this.lblRemainAmount.HyperLink = null;
            this.lblRemainAmount.Left = 4.795276F;
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblRemainAmount.Text = "入金残";
            this.lblRemainAmount.Top = 0.8598425F;
            this.lblRemainAmount.Width = 1.267717F;
            // 
            // lblExcludeCategoryCode
            // 
            this.lblExcludeCategoryCode.Height = 0.2500001F;
            this.lblExcludeCategoryCode.HyperLink = null;
            this.lblExcludeCategoryCode.Left = 6.066929F;
            this.lblExcludeCategoryCode.Name = "lblExcludeCategoryCode";
            this.lblExcludeCategoryCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblExcludeCategoryCode.Text = "対象外区分";
            this.lblExcludeCategoryCode.Top = 0.6181103F;
            this.lblExcludeCategoryCode.Width = 1.19685F;
            // 
            // lblExcludeAmount
            // 
            this.lblExcludeAmount.Height = 0.25F;
            this.lblExcludeAmount.HyperLink = null;
            this.lblExcludeAmount.Left = 6.066929F;
            this.lblExcludeAmount.Name = "lblExcludeAmount";
            this.lblExcludeAmount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblExcludeAmount.Text = "対象外金額";
            this.lblExcludeAmount.Top = 0.8598425F;
            this.lblExcludeAmount.Width = 1.19685F;
            // 
            // lblBankCode
            // 
            this.lblBankCode.Height = 0.496063F;
            this.lblBankCode.HyperLink = null;
            this.lblBankCode.Left = 7.265355F;
            this.lblBankCode.Name = "lblBankCode";
            this.lblBankCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblBankCode.Text = "入金銀行";
            this.lblBankCode.Top = 0.6181103F;
            this.lblBankCode.Width = 0.7874016F;
            // 
            // lblBranchCode
            // 
            this.lblBranchCode.Height = 0.496063F;
            this.lblBranchCode.HyperLink = null;
            this.lblBranchCode.Left = 8.050788F;
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblBranchCode.Text = "入金支店";
            this.lblBranchCode.Top = 0.6181103F;
            this.lblBranchCode.Width = 0.8543308F;
            // 
            // lblAccount
            // 
            this.lblAccount.Height = 0.496063F;
            this.lblAccount.HyperLink = null;
            this.lblAccount.Left = 8.905119F;
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblAccount.Text = "口座番号";
            this.lblAccount.Top = 0.6181103F;
            this.lblAccount.Width = 0.7606297F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.6141732F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.62992F;
            this.lineHeaderHorUpper.Y1 = 0.6141732F;
            this.lineHeaderHorUpper.Y2 = 0.6141732F;
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
            // lineHeaderHorReceiptId
            // 
            this.lineHeaderHorReceiptId.Height = 0F;
            this.lineHeaderHorReceiptId.Left = 0F;
            this.lineHeaderHorReceiptId.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorReceiptId.LineWeight = 1F;
            this.lineHeaderHorReceiptId.Name = "lineHeaderHorReceiptId";
            this.lineHeaderHorReceiptId.Top = 0.8622047F;
            this.lineHeaderHorReceiptId.Width = 3.826772F;
            this.lineHeaderHorReceiptId.X1 = 0F;
            this.lineHeaderHorReceiptId.X2 = 3.826772F;
            this.lineHeaderHorReceiptId.Y1 = 0.8622047F;
            this.lineHeaderHorReceiptId.Y2 = 0.8622047F;
            // 
            // lineHeaderHorReceiptAmount
            // 
            this.lineHeaderHorReceiptAmount.Height = 0F;
            this.lineHeaderHorReceiptAmount.Left = 4.795276F;
            this.lineHeaderHorReceiptAmount.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorReceiptAmount.LineWeight = 1F;
            this.lineHeaderHorReceiptAmount.Name = "lineHeaderHorReceiptAmount";
            this.lineHeaderHorReceiptAmount.Top = 0.8622047F;
            this.lineHeaderHorReceiptAmount.Width = 2.470079F;
            this.lineHeaderHorReceiptAmount.X1 = 4.795276F;
            this.lineHeaderHorReceiptAmount.X2 = 7.265355F;
            this.lineHeaderHorReceiptAmount.Y1 = 0.8622047F;
            this.lineHeaderHorReceiptAmount.Y2 = 0.8622047F;
            // 
            // lineHeaderVerReceiptId
            // 
            this.lineHeaderVerReceiptId.Height = 0.4976377F;
            this.lineHeaderVerReceiptId.Left = 0.738189F;
            this.lineHeaderVerReceiptId.LineWeight = 1F;
            this.lineHeaderVerReceiptId.Name = "lineHeaderVerReceiptId";
            this.lineHeaderVerReceiptId.Top = 0.6141733F;
            this.lineHeaderVerReceiptId.Width = 0F;
            this.lineHeaderVerReceiptId.X1 = 0.738189F;
            this.lineHeaderVerReceiptId.X2 = 0.738189F;
            this.lineHeaderVerReceiptId.Y1 = 0.6141733F;
            this.lineHeaderVerReceiptId.Y2 = 1.111811F;
            // 
            // lineHeaderVerRecordedAt
            // 
            this.lineHeaderVerRecordedAt.Height = 0.4976377F;
            this.lineHeaderVerRecordedAt.Left = 1.527559F;
            this.lineHeaderVerRecordedAt.LineWeight = 1F;
            this.lineHeaderVerRecordedAt.Name = "lineHeaderVerRecordedAt";
            this.lineHeaderVerRecordedAt.Top = 0.6141733F;
            this.lineHeaderVerRecordedAt.Width = 0F;
            this.lineHeaderVerRecordedAt.X1 = 1.527559F;
            this.lineHeaderVerRecordedAt.X2 = 1.527559F;
            this.lineHeaderVerRecordedAt.Y1 = 0.6141733F;
            this.lineHeaderVerRecordedAt.Y2 = 1.111811F;
            // 
            // lineHeaderVerPaymentCategory
            // 
            this.lineHeaderVerPaymentCategory.Height = 0.4976377F;
            this.lineHeaderVerPaymentCategory.Left = 2.574803F;
            this.lineHeaderVerPaymentCategory.LineWeight = 1F;
            this.lineHeaderVerPaymentCategory.Name = "lineHeaderVerPaymentCategory";
            this.lineHeaderVerPaymentCategory.Top = 0.6141733F;
            this.lineHeaderVerPaymentCategory.Width = 0F;
            this.lineHeaderVerPaymentCategory.X1 = 2.574803F;
            this.lineHeaderVerPaymentCategory.X2 = 2.574803F;
            this.lineHeaderVerPaymentCategory.Y1 = 0.6141733F;
            this.lineHeaderVerPaymentCategory.Y2 = 1.111811F;
            // 
            // lineHeaderVerPayerName
            // 
            this.lineHeaderVerPayerName.Height = 0.4976377F;
            this.lineHeaderVerPayerName.Left = 3.826772F;
            this.lineHeaderVerPayerName.LineWeight = 1F;
            this.lineHeaderVerPayerName.Name = "lineHeaderVerPayerName";
            this.lineHeaderVerPayerName.Top = 0.6141733F;
            this.lineHeaderVerPayerName.Width = 0F;
            this.lineHeaderVerPayerName.X1 = 3.826772F;
            this.lineHeaderVerPayerName.X2 = 3.826772F;
            this.lineHeaderVerPayerName.Y1 = 0.6141733F;
            this.lineHeaderVerPayerName.Y2 = 1.111811F;
            // 
            // lineHeaderVerSection
            // 
            this.lineHeaderVerSection.Height = 0.4976377F;
            this.lineHeaderVerSection.Left = 4.795276F;
            this.lineHeaderVerSection.LineWeight = 1F;
            this.lineHeaderVerSection.Name = "lineHeaderVerSection";
            this.lineHeaderVerSection.Top = 0.6141733F;
            this.lineHeaderVerSection.Width = 0F;
            this.lineHeaderVerSection.X1 = 4.795276F;
            this.lineHeaderVerSection.X2 = 4.795276F;
            this.lineHeaderVerSection.Y1 = 0.6141733F;
            this.lineHeaderVerSection.Y2 = 1.111811F;
            // 
            // lineHeaderVerReceiptAmount
            // 
            this.lineHeaderVerReceiptAmount.Height = 0.4976377F;
            this.lineHeaderVerReceiptAmount.Left = 6.062993F;
            this.lineHeaderVerReceiptAmount.LineWeight = 1F;
            this.lineHeaderVerReceiptAmount.Name = "lineHeaderVerReceiptAmount";
            this.lineHeaderVerReceiptAmount.Top = 0.6141733F;
            this.lineHeaderVerReceiptAmount.Width = 0F;
            this.lineHeaderVerReceiptAmount.X1 = 6.062993F;
            this.lineHeaderVerReceiptAmount.X2 = 6.062993F;
            this.lineHeaderVerReceiptAmount.Y1 = 0.6141733F;
            this.lineHeaderVerReceiptAmount.Y2 = 1.111811F;
            // 
            // lineHeaderVerExcludeCategoryCode
            // 
            this.lineHeaderVerExcludeCategoryCode.Height = 0.4976377F;
            this.lineHeaderVerExcludeCategoryCode.Left = 7.265355F;
            this.lineHeaderVerExcludeCategoryCode.LineWeight = 1F;
            this.lineHeaderVerExcludeCategoryCode.Name = "lineHeaderVerExcludeCategoryCode";
            this.lineHeaderVerExcludeCategoryCode.Top = 0.6141733F;
            this.lineHeaderVerExcludeCategoryCode.Width = 0F;
            this.lineHeaderVerExcludeCategoryCode.X1 = 7.265355F;
            this.lineHeaderVerExcludeCategoryCode.X2 = 7.265355F;
            this.lineHeaderVerExcludeCategoryCode.Y1 = 0.6141733F;
            this.lineHeaderVerExcludeCategoryCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerBankCode
            // 
            this.lineHeaderVerBankCode.Height = 0.4976377F;
            this.lineHeaderVerBankCode.Left = 8.050788F;
            this.lineHeaderVerBankCode.LineWeight = 1F;
            this.lineHeaderVerBankCode.Name = "lineHeaderVerBankCode";
            this.lineHeaderVerBankCode.Top = 0.6141733F;
            this.lineHeaderVerBankCode.Width = 0F;
            this.lineHeaderVerBankCode.X1 = 8.050788F;
            this.lineHeaderVerBankCode.X2 = 8.050788F;
            this.lineHeaderVerBankCode.Y1 = 0.6141733F;
            this.lineHeaderVerBankCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerBranchCode
            // 
            this.lineHeaderVerBranchCode.Height = 0.4976377F;
            this.lineHeaderVerBranchCode.Left = 8.910236F;
            this.lineHeaderVerBranchCode.LineWeight = 1F;
            this.lineHeaderVerBranchCode.Name = "lineHeaderVerBranchCode";
            this.lineHeaderVerBranchCode.Top = 0.6141733F;
            this.lineHeaderVerBranchCode.Width = 9.536743E-07F;
            this.lineHeaderVerBranchCode.X1 = 8.910237F;
            this.lineHeaderVerBranchCode.X2 = 8.910236F;
            this.lineHeaderVerBranchCode.Y1 = 0.6141733F;
            this.lineHeaderVerBranchCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerAccount
            // 
            this.lineHeaderVerAccount.Height = 0.4976377F;
            this.lineHeaderVerAccount.Left = 9.670867F;
            this.lineHeaderVerAccount.LineWeight = 1F;
            this.lineHeaderVerAccount.Name = "lineHeaderVerAccount";
            this.lineHeaderVerAccount.Top = 0.6141733F;
            this.lineHeaderVerAccount.Width = 0F;
            this.lineHeaderVerAccount.X1 = 9.670867F;
            this.lineHeaderVerAccount.X2 = 9.670867F;
            this.lineHeaderVerAccount.Y1 = 0.6141733F;
            this.lineHeaderVerAccount.Y2 = 1.111811F;
            // 
            // lineHeaderHorSourceBank
            // 
            this.lineHeaderHorSourceBank.Height = 0F;
            this.lineHeaderHorSourceBank.Left = 9.670867F;
            this.lineHeaderHorSourceBank.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorSourceBank.LineWeight = 1F;
            this.lineHeaderHorSourceBank.Name = "lineHeaderHorSourceBank";
            this.lineHeaderHorSourceBank.Top = 0.8622047F;
            this.lineHeaderHorSourceBank.Width = 0.959053F;
            this.lineHeaderHorSourceBank.X1 = 9.670867F;
            this.lineHeaderHorSourceBank.X2 = 10.62992F;
            this.lineHeaderHorSourceBank.Y1 = 0.8622047F;
            this.lineHeaderHorSourceBank.Y2 = 0.8622047F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtExcludeCategoryCode,
            this.lineDetailVerExcludeCategoryCode,
            this.txtReceiptId,
            this.txtDeleteAt,
            this.txtRecordedAt,
            this.txtSaleAt,
            this.txtPaymentCategory,
            this.txtInputType,
            this.txtPayerName,
            this.txtNote1,
            this.txtSectionCode,
            this.txtSectionName,
            this.txtReceiptAmount,
            this.txtRemainAmount,
            this.txtBankCode,
            this.txtBranchCode,
            this.txtBranchName,
            this.txtBankName,
            this.txtExcludeAmount,
            this.txtAccountNumber,
            this.txtSourceBankName,
            this.txtSourceBranchName,
            this.lineDetailVerReceiptId,
            this.lineDetailVerRecordedAt,
            this.lineDetailVerPaymentCategory,
            this.lineDetailVerPayerName,
            this.lineDetailVerSectionCode,
            this.lineDetailVerReceiptAmount,
            this.lineDetailVerBankCode,
            this.lineDetailVerBranchCode,
            this.lineDetailHorLower,
            this.lineDetailVerAccount});
            this.detail.Height = 0.4598425F;
            this.detail.Name = "detail";
            // 
            // txtExcludeCategoryCode
            // 
            this.txtExcludeCategoryCode.Height = 0.2244094F;
            this.txtExcludeCategoryCode.Left = 6.062993F;
            this.txtExcludeCategoryCode.MultiLine = false;
            this.txtExcludeCategoryCode.Name = "txtExcludeCategoryCode";
            this.txtExcludeCategoryCode.Style = "font-size: 9pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtExcludeCategoryCode.Text = "txtExcludeCategoryCode";
            this.txtExcludeCategoryCode.Top = 0F;
            this.txtExcludeCategoryCode.Width = 1.188976F;
            // 
            // lineDetailVerExcludeCategoryCode
            // 
            this.lineDetailVerExcludeCategoryCode.Height = 0.4598425F;
            this.lineDetailVerExcludeCategoryCode.Left = 7.265355F;
            this.lineDetailVerExcludeCategoryCode.LineWeight = 1F;
            this.lineDetailVerExcludeCategoryCode.Name = "lineDetailVerExcludeCategoryCode";
            this.lineDetailVerExcludeCategoryCode.Top = 0F;
            this.lineDetailVerExcludeCategoryCode.Width = 0F;
            this.lineDetailVerExcludeCategoryCode.X1 = 7.265355F;
            this.lineDetailVerExcludeCategoryCode.X2 = 7.265355F;
            this.lineDetailVerExcludeCategoryCode.Y1 = 0F;
            this.lineDetailVerExcludeCategoryCode.Y2 = 0.4598425F;
            // 
            // txtReceiptId
            // 
            this.txtReceiptId.Height = 0.2244094F;
            this.txtReceiptId.Left = 0F;
            this.txtReceiptId.MultiLine = false;
            this.txtReceiptId.Name = "txtReceiptId";
            this.txtReceiptId.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtReceiptId.Style = "font-size: 9pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptId.Text = "txtReceiptId";
            this.txtReceiptId.Top = 0F;
            this.txtReceiptId.Width = 0.738189F;
            // 
            // txtDeleteAt
            // 
            this.txtDeleteAt.Height = 0.2244094F;
            this.txtDeleteAt.Left = 0F;
            this.txtDeleteAt.MultiLine = false;
            this.txtDeleteAt.Name = "txtDeleteAt";
            this.txtDeleteAt.OutputFormat = resources.GetString("txtDeleteAt.OutputFormat");
            this.txtDeleteAt.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtDeleteAt.Text = "txtDeleteAt";
            this.txtDeleteAt.Top = 0.2322835F;
            this.txtDeleteAt.Width = 0.738189F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.2244094F;
            this.txtRecordedAt.Left = 0.7519686F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtRecordedAt.Text = "txtRecordedAt";
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.7771654F;
            // 
            // txtSaleAt
            // 
            this.txtSaleAt.Height = 0.2244094F;
            this.txtSaleAt.Left = 0.7519686F;
            this.txtSaleAt.MultiLine = false;
            this.txtSaleAt.Name = "txtSaleAt";
            this.txtSaleAt.OutputFormat = resources.GetString("txtSaleAt.OutputFormat");
            this.txtSaleAt.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtSaleAt.Text = "txtSaleAt";
            this.txtSaleAt.Top = 0.2322835F;
            this.txtSaleAt.Width = 0.7771654F;
            // 
            // txtPaymentCategory
            // 
            this.txtPaymentCategory.Height = 0.2244094F;
            this.txtPaymentCategory.Left = 1.543307F;
            this.txtPaymentCategory.MultiLine = false;
            this.txtPaymentCategory.Name = "txtPaymentCategory";
            this.txtPaymentCategory.Style = "font-size: 9pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtPaymentCategory.Text = "txtPaymentCategory";
            this.txtPaymentCategory.Top = 0F;
            this.txtPaymentCategory.Width = 1.031496F;
            // 
            // txtInputType
            // 
            this.txtInputType.Height = 0.2244094F;
            this.txtInputType.Left = 1.543307F;
            this.txtInputType.MultiLine = false;
            this.txtInputType.Name = "txtInputType";
            this.txtInputType.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtInputType.Text = "txtInputType";
            this.txtInputType.Top = 0.2322835F;
            this.txtInputType.Width = 1.031496F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.2244094F;
            this.txtPayerName.Left = 2.586614F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtPayerName.Text = "txtPayerName";
            this.txtPayerName.Top = 0F;
            this.txtPayerName.Width = 1.239764F;
            // 
            // txtNote1
            // 
            this.txtNote1.Height = 0.2244094F;
            this.txtNote1.Left = 2.586614F;
            this.txtNote1.MultiLine = false;
            this.txtNote1.Name = "txtNote1";
            this.txtNote1.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtNote1.Text = "txtNote1";
            this.txtNote1.Top = 0.2322835F;
            this.txtNote1.Width = 1.240158F;
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.Height = 0.2244094F;
            this.txtSectionCode.Left = 3.834646F;
            this.txtSectionCode.MultiLine = false;
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtSectionCode.Text = "txtSectionCode";
            this.txtSectionCode.Top = 0F;
            this.txtSectionCode.Width = 0.9606299F;
            // 
            // txtSectionName
            // 
            this.txtSectionName.Height = 0.2244094F;
            this.txtSectionName.Left = 3.834646F;
            this.txtSectionName.MultiLine = false;
            this.txtSectionName.Name = "txtSectionName";
            this.txtSectionName.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtSectionName.Text = "txtSectionName";
            this.txtSectionName.Top = 0.2322835F;
            this.txtSectionName.Width = 0.9606299F;
            // 
            // txtReceiptAmount
            // 
            this.txtReceiptAmount.Height = 0.2244094F;
            this.txtReceiptAmount.Left = 4.80315F;
            this.txtReceiptAmount.MultiLine = false;
            this.txtReceiptAmount.Name = "txtReceiptAmount";
            this.txtReceiptAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtReceiptAmount.Style = "font-size: 9pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptAmount.Text = "txtReceiptAmount";
            this.txtReceiptAmount.Top = 0F;
            this.txtReceiptAmount.Width = 1.259843F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.Height = 0.2244094F;
            this.txtRemainAmount.Left = 4.80315F;
            this.txtRemainAmount.MultiLine = false;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtRemainAmount.Style = "font-size: 9pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtRemainAmount.Text = "txtRemainAmount";
            this.txtRemainAmount.Top = 0.2322835F;
            this.txtRemainAmount.Width = 1.259843F;
            // 
            // txtBankCode
            // 
            this.txtBankCode.Height = 0.2244094F;
            this.txtBankCode.Left = 7.269292F;
            this.txtBankCode.MultiLine = false;
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBankCode.Text = "txtBankCode";
            this.txtBankCode.Top = 0F;
            this.txtBankCode.Width = 0.781496F;
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.Height = 0.2244094F;
            this.txtBranchCode.Left = 8.058662F;
            this.txtBranchCode.MultiLine = false;
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtBranchCode.Text = "txtBranchCode";
            this.txtBranchCode.Top = 0F;
            this.txtBranchCode.Width = 0.8389755F;
            // 
            // txtBranchName
            // 
            this.txtBranchName.Height = 0.2244094F;
            this.txtBranchName.Left = 8.058662F;
            this.txtBranchName.MultiLine = false;
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtBranchName.Text = "txtBranchName";
            this.txtBranchName.Top = 0.2322835F;
            this.txtBranchName.Width = 0.8389755F;
            // 
            // txtBankName
            // 
            this.txtBankName.Height = 0.2244094F;
            this.txtBankName.Left = 7.269292F;
            this.txtBankName.MultiLine = false;
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.ShrinkToFit = true;
            this.txtBankName.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: true";
            this.txtBankName.Text = "txtBankName";
            this.txtBankName.Top = 0.2322835F;
            this.txtBankName.Width = 0.781496F;
            // 
            // txtExcludeAmount
            // 
            this.txtExcludeAmount.Height = 0.2244094F;
            this.txtExcludeAmount.Left = 6.062993F;
            this.txtExcludeAmount.MultiLine = false;
            this.txtExcludeAmount.Name = "txtExcludeAmount";
            this.txtExcludeAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtExcludeAmount.Style = "font-size: 9pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtExcludeAmount.Text = "txtExcludeAmount";
            this.txtExcludeAmount.Top = 0.2322835F;
            this.txtExcludeAmount.Width = 1.188976F;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Height = 0.4566929F;
            this.txtAccountNumber.Left = 8.905119F;
            this.txtAccountNumber.MultiLine = false;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtAccountNumber.Text = "txtAccountNumber";
            this.txtAccountNumber.Top = 0F;
            this.txtAccountNumber.Width = 0.7606297F;
            // 
            // txtSourceBankName
            // 
            this.txtSourceBankName.Height = 0.2244094F;
            this.txtSourceBankName.Left = 9.669292F;
            this.txtSourceBankName.MultiLine = false;
            this.txtSourceBankName.Name = "txtSourceBankName";
            this.txtSourceBankName.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtSourceBankName.Text = "txtSourceBankName";
            this.txtSourceBankName.Top = 0F;
            this.txtSourceBankName.Width = 0.9448819F;
            // 
            // txtSourceBranchName
            // 
            this.txtSourceBranchName.Height = 0.2244094F;
            this.txtSourceBranchName.Left = 9.669292F;
            this.txtSourceBranchName.MultiLine = false;
            this.txtSourceBranchName.Name = "txtSourceBranchName";
            this.txtSourceBranchName.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtSourceBranchName.Text = "txtSourceBranchName";
            this.txtSourceBranchName.Top = 0.2322835F;
            this.txtSourceBranchName.Width = 0.9448819F;
            // 
            // lineDetailVerReceiptId
            // 
            this.lineDetailVerReceiptId.Height = 0.4598425F;
            this.lineDetailVerReceiptId.Left = 0.738189F;
            this.lineDetailVerReceiptId.LineWeight = 1F;
            this.lineDetailVerReceiptId.Name = "lineDetailVerReceiptId";
            this.lineDetailVerReceiptId.Top = 0F;
            this.lineDetailVerReceiptId.Width = 0F;
            this.lineDetailVerReceiptId.X1 = 0.738189F;
            this.lineDetailVerReceiptId.X2 = 0.738189F;
            this.lineDetailVerReceiptId.Y1 = 0F;
            this.lineDetailVerReceiptId.Y2 = 0.4598425F;
            // 
            // lineDetailVerRecordedAt
            // 
            this.lineDetailVerRecordedAt.Height = 0.4598425F;
            this.lineDetailVerRecordedAt.Left = 1.527559F;
            this.lineDetailVerRecordedAt.LineWeight = 1F;
            this.lineDetailVerRecordedAt.Name = "lineDetailVerRecordedAt";
            this.lineDetailVerRecordedAt.Top = 0F;
            this.lineDetailVerRecordedAt.Width = 0F;
            this.lineDetailVerRecordedAt.X1 = 1.527559F;
            this.lineDetailVerRecordedAt.X2 = 1.527559F;
            this.lineDetailVerRecordedAt.Y1 = 0F;
            this.lineDetailVerRecordedAt.Y2 = 0.4598425F;
            // 
            // lineDetailVerPaymentCategory
            // 
            this.lineDetailVerPaymentCategory.Height = 0.4598425F;
            this.lineDetailVerPaymentCategory.Left = 2.574803F;
            this.lineDetailVerPaymentCategory.LineWeight = 1F;
            this.lineDetailVerPaymentCategory.Name = "lineDetailVerPaymentCategory";
            this.lineDetailVerPaymentCategory.Top = 0F;
            this.lineDetailVerPaymentCategory.Width = 0F;
            this.lineDetailVerPaymentCategory.X1 = 2.574803F;
            this.lineDetailVerPaymentCategory.X2 = 2.574803F;
            this.lineDetailVerPaymentCategory.Y1 = 0F;
            this.lineDetailVerPaymentCategory.Y2 = 0.4598425F;
            // 
            // lineDetailVerPayerName
            // 
            this.lineDetailVerPayerName.Height = 0.4598425F;
            this.lineDetailVerPayerName.Left = 3.826772F;
            this.lineDetailVerPayerName.LineWeight = 1F;
            this.lineDetailVerPayerName.Name = "lineDetailVerPayerName";
            this.lineDetailVerPayerName.Top = 0F;
            this.lineDetailVerPayerName.Width = 0F;
            this.lineDetailVerPayerName.X1 = 3.826772F;
            this.lineDetailVerPayerName.X2 = 3.826772F;
            this.lineDetailVerPayerName.Y1 = 0F;
            this.lineDetailVerPayerName.Y2 = 0.4598425F;
            // 
            // lineDetailVerSectionCode
            // 
            this.lineDetailVerSectionCode.Height = 0.4598425F;
            this.lineDetailVerSectionCode.Left = 4.795276F;
            this.lineDetailVerSectionCode.LineWeight = 1F;
            this.lineDetailVerSectionCode.Name = "lineDetailVerSectionCode";
            this.lineDetailVerSectionCode.Top = 0F;
            this.lineDetailVerSectionCode.Width = 0F;
            this.lineDetailVerSectionCode.X1 = 4.795276F;
            this.lineDetailVerSectionCode.X2 = 4.795276F;
            this.lineDetailVerSectionCode.Y1 = 0F;
            this.lineDetailVerSectionCode.Y2 = 0.4598425F;
            // 
            // lineDetailVerReceiptAmount
            // 
            this.lineDetailVerReceiptAmount.Height = 0.4598425F;
            this.lineDetailVerReceiptAmount.Left = 6.062993F;
            this.lineDetailVerReceiptAmount.LineWeight = 1F;
            this.lineDetailVerReceiptAmount.Name = "lineDetailVerReceiptAmount";
            this.lineDetailVerReceiptAmount.Top = 0F;
            this.lineDetailVerReceiptAmount.Width = 0F;
            this.lineDetailVerReceiptAmount.X1 = 6.062993F;
            this.lineDetailVerReceiptAmount.X2 = 6.062993F;
            this.lineDetailVerReceiptAmount.Y1 = 0F;
            this.lineDetailVerReceiptAmount.Y2 = 0.4598425F;
            // 
            // lineDetailVerBankCode
            // 
            this.lineDetailVerBankCode.Height = 0.4598425F;
            this.lineDetailVerBankCode.Left = 8.050788F;
            this.lineDetailVerBankCode.LineWeight = 1F;
            this.lineDetailVerBankCode.Name = "lineDetailVerBankCode";
            this.lineDetailVerBankCode.Top = 0F;
            this.lineDetailVerBankCode.Width = 0F;
            this.lineDetailVerBankCode.X1 = 8.050788F;
            this.lineDetailVerBankCode.X2 = 8.050788F;
            this.lineDetailVerBankCode.Y1 = 0F;
            this.lineDetailVerBankCode.Y2 = 0.4598425F;
            // 
            // lineDetailVerBranchCode
            // 
            this.lineDetailVerBranchCode.Height = 0.4598425F;
            this.lineDetailVerBranchCode.Left = 8.910236F;
            this.lineDetailVerBranchCode.LineWeight = 1F;
            this.lineDetailVerBranchCode.Name = "lineDetailVerBranchCode";
            this.lineDetailVerBranchCode.Top = 0F;
            this.lineDetailVerBranchCode.Width = 9.536743E-07F;
            this.lineDetailVerBranchCode.X1 = 8.910237F;
            this.lineDetailVerBranchCode.X2 = 8.910236F;
            this.lineDetailVerBranchCode.Y1 = 0F;
            this.lineDetailVerBranchCode.Y2 = 0.4598425F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.4598425F;
            this.lineDetailHorLower.Width = 10.62992F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.62992F;
            this.lineDetailHorLower.Y1 = 0.4598425F;
            this.lineDetailHorLower.Y2 = 0.4598425F;
            // 
            // lineDetailVerAccount
            // 
            this.lineDetailVerAccount.Height = 0.4598425F;
            this.lineDetailVerAccount.Left = 9.670865F;
            this.lineDetailVerAccount.LineWeight = 1F;
            this.lineDetailVerAccount.Name = "lineDetailVerAccount";
            this.lineDetailVerAccount.Top = 0F;
            this.lineDetailVerAccount.Width = 1.907349E-06F;
            this.lineDetailVerAccount.X1 = 9.670865F;
            this.lineDetailVerAccount.X2 = 9.670867F;
            this.lineDetailVerAccount.Y1 = 0F;
            this.lineDetailVerAccount.Y2 = 0.4598425F;
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
            this.reportInfo1.Left = 7.328741F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.05905512F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1.181102F;
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
            this.txtReceiptTotal,
            this.lblSpace1,
            this.txtExcludeAmtTotal,
            this.lblSpace2,
            this.txtRemainTotal,
            this.lineFooterVerTotal,
            this.lineFooterVerReceiptAmountTotal,
            this.lineFooterHorLower,
            this.lineFooterVerExcludeTotal});
            this.groupFooter1.Height = 0.4598425F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // lblTotal
            // 
            this.lblTotal.Height = 0.4598425F;
            this.lblTotal.HyperLink = null;
            this.lblTotal.Left = 0F;
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblTotal.Style = "background-color: WhiteSmoke; font-size: 9pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblTotal.Text = "総合計";
            this.lblTotal.Top = 0F;
            this.lblTotal.Width = 4.795276F;
            // 
            // txtReceiptTotal
            // 
            this.txtReceiptTotal.DataField = "ReceiptAmount";
            this.txtReceiptTotal.Height = 0.2244094F;
            this.txtReceiptTotal.Left = 4.795276F;
            this.txtReceiptTotal.MultiLine = false;
            this.txtReceiptTotal.Name = "txtReceiptTotal";
            this.txtReceiptTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtReceiptTotal.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtReceiptTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtReceiptTotal.Text = "txtReceiptAmtTotal";
            this.txtReceiptTotal.Top = 0F;
            this.txtReceiptTotal.Width = 1.267717F;
            // 
            // lblSpace1
            // 
            this.lblSpace1.Height = 0.2244094F;
            this.lblSpace1.HyperLink = null;
            this.lblSpace1.Left = 6.066929F;
            this.lblSpace1.Name = "lblSpace1";
            this.lblSpace1.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblSpace1.Text = "";
            this.lblSpace1.Top = 0F;
            this.lblSpace1.Width = 1.19685F;
            // 
            // txtExcludeAmtTotal
            // 
            this.txtExcludeAmtTotal.DataField = "ExcludeAmount";
            this.txtExcludeAmtTotal.Height = 0.2244094F;
            this.txtExcludeAmtTotal.Left = 6.066929F;
            this.txtExcludeAmtTotal.Name = "txtExcludeAmtTotal";
            this.txtExcludeAmtTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtExcludeAmtTotal.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtExcludeAmtTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtExcludeAmtTotal.Text = "txtExcludeTotal";
            this.txtExcludeAmtTotal.Top = 0.2259843F;
            this.txtExcludeAmtTotal.Width = 1.19685F;
            // 
            // lblSpace2
            // 
            this.lblSpace2.Height = 0.4598425F;
            this.lblSpace2.HyperLink = null;
            this.lblSpace2.Left = 7.265355F;
            this.lblSpace2.Name = "lblSpace2";
            this.lblSpace2.Style = "background-color: WhiteSmoke; font-size: 9pt; ddo-char-set: 128";
            this.lblSpace2.Text = "";
            this.lblSpace2.Top = 0F;
            this.lblSpace2.Width = 3.364173F;
            // 
            // txtRemainTotal
            // 
            this.txtRemainTotal.DataField = "RemainAmount";
            this.txtRemainTotal.Height = 0.2244094F;
            this.txtRemainTotal.Left = 4.795276F;
            this.txtRemainTotal.MultiLine = false;
            this.txtRemainTotal.Name = "txtRemainTotal";
            this.txtRemainTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtRemainTotal.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtRemainTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtRemainTotal.Text = "txtRemainAmtTotal";
            this.txtRemainTotal.Top = 0.2259843F;
            this.txtRemainTotal.Width = 1.267717F;
            // 
            // lineFooterVerTotal
            // 
            this.lineFooterVerTotal.Height = 0.4598425F;
            this.lineFooterVerTotal.Left = 4.795276F;
            this.lineFooterVerTotal.LineWeight = 1F;
            this.lineFooterVerTotal.Name = "lineFooterVerTotal";
            this.lineFooterVerTotal.Top = 0F;
            this.lineFooterVerTotal.Width = 0F;
            this.lineFooterVerTotal.X1 = 4.795276F;
            this.lineFooterVerTotal.X2 = 4.795276F;
            this.lineFooterVerTotal.Y1 = 0F;
            this.lineFooterVerTotal.Y2 = 0.4598425F;
            // 
            // lineFooterVerReceiptAmountTotal
            // 
            this.lineFooterVerReceiptAmountTotal.Height = 0.4598425F;
            this.lineFooterVerReceiptAmountTotal.Left = 6.062993F;
            this.lineFooterVerReceiptAmountTotal.LineWeight = 1F;
            this.lineFooterVerReceiptAmountTotal.Name = "lineFooterVerReceiptAmountTotal";
            this.lineFooterVerReceiptAmountTotal.Top = 0F;
            this.lineFooterVerReceiptAmountTotal.Width = 0F;
            this.lineFooterVerReceiptAmountTotal.X1 = 6.062993F;
            this.lineFooterVerReceiptAmountTotal.X2 = 6.062993F;
            this.lineFooterVerReceiptAmountTotal.Y1 = 0F;
            this.lineFooterVerReceiptAmountTotal.Y2 = 0.4598425F;
            // 
            // lineFooterHorLower
            // 
            this.lineFooterHorLower.Height = 0F;
            this.lineFooterHorLower.Left = 0F;
            this.lineFooterHorLower.LineWeight = 1F;
            this.lineFooterHorLower.Name = "lineFooterHorLower";
            this.lineFooterHorLower.Top = 0.4598425F;
            this.lineFooterHorLower.Width = 10.62992F;
            this.lineFooterHorLower.X1 = 0F;
            this.lineFooterHorLower.X2 = 10.62992F;
            this.lineFooterHorLower.Y1 = 0.4598425F;
            this.lineFooterHorLower.Y2 = 0.4598425F;
            // 
            // lineFooterVerExcludeTotal
            // 
            this.lineFooterVerExcludeTotal.Height = 0.4598425F;
            this.lineFooterVerExcludeTotal.Left = 7.265355F;
            this.lineFooterVerExcludeTotal.LineWeight = 1F;
            this.lineFooterVerExcludeTotal.Name = "lineFooterVerExcludeTotal";
            this.lineFooterVerExcludeTotal.Top = 0F;
            this.lineFooterVerExcludeTotal.Width = 0F;
            this.lineFooterVerExcludeTotal.X1 = 7.265355F;
            this.lineFooterVerExcludeTotal.X2 = 7.265355F;
            this.lineFooterVerExcludeTotal.Y1 = 0F;
            this.lineFooterVerExcludeTotal.Y2 = 0.4598425F;
            // 
            // ReceiptOmitSectionReport
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
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " +
            "color: Black; font-family: \"ＭＳ 明朝\"; ddo-char-set: 186", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceBranch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPaymentCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDeleteAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSaletAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeleteAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBankName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceBranchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeAmtTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDeleteAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSaletAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPaymentCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInputType;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSection;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblExcludeCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblExcludeAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerPaymentCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSection;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerExcludeCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAccount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDeleteAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSaleAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPaymentCategory;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInputType;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSourceBank;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSourceBranch;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtExcludeCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSourceBankName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSourceBranchName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerReceiptId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerPaymentCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerExcludeCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBankCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAccount;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpace2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerReceiptAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtExcludeAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtExcludeAmtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerExcludeTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorSourceBank;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpace1;
    }
}
