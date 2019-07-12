namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ReceiptSectionTransferSectionReport の概要の説明です。
    /// </summary>
    partial class ReceiptSectionTransferSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReceiptSectionTransferSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompany = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblId = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCategoryCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInputType = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPayerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSourceAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSourceSection = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDestinationSection = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDestinationAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMemo = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblLoginUserCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSourceAmt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSourceSection = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line11 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line13 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line24 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorLowerSource = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLowerDest = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtId = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCategoryCodeName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInputType = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPayerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSourceAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSourceSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSourceSectionName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblArrow = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtDestinationSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDestinationSectionName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDestinationAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreateAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMemo = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtLoginUserCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtLoginUserName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSourceAmt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSoruceSection = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerArrow = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDestSection = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDestinationAmt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreateAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorSource = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line25 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.ghCurrency = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategoryCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceSection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDestinationSection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDestinationAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMemo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUserCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompany,
            this.label1,
            this.lbldate,
            this.ridate,
            this.lblId,
            this.lblRecordedAt,
            this.lblDueAt,
            this.lblCategoryCodeName,
            this.lblInputType,
            this.lblPayerName,
            this.lblNote1,
            this.lblSourceAmount,
            this.lblSourceSection,
            this.lblDestinationSection,
            this.lblDestinationAmount,
            this.lblCreditAt,
            this.lblMemo,
            this.lblLoginUserCode,
            this.line1,
            this.lineHeaderVerId,
            this.lineHeaderVerRecordedAt,
            this.lineHeaderVerCategoryCode,
            this.lineHeaderVerPayerName,
            this.lineHeaderVerSourceAmt,
            this.line9,
            this.lineHeaderVerSourceSection,
            this.line11,
            this.line12,
            this.line13,
            this.line24,
            this.lblCompanyCode,
            this.lineHeaderHorLowerSource,
            this.lineHeaderHorLowerDest,
            this.lblCurrencyCode});
            this.pageHeader.Height = 1.408662F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompany
            // 
            this.lblCompany.Height = 0.2F;
            this.lblCompany.HyperLink = null;
            this.lblCompany.Left = 0.02440945F;
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Style = "color: Gray; font-size: 7pt; ddo-char-set: 1";
            this.lblCompany.Text = "会社コード :";
            this.lblCompany.Top = 0F;
            this.lblCompany.Width = 0.7874016F;
            // 
            // label1
            // 
            this.label1.Height = 0.3248032F;
            this.label1.HyperLink = null;
            this.label1.Left = 0F;
            this.label1.Name = "label1";
            this.label1.Style = "font-size: 14pt; text-align: center; text-decoration: underline; vertical-align: " +
    "middle";
            this.label1.Text = "入金部門振替済チェックリスト";
            this.label1.Top = 0.2704725F;
            this.label1.Width = 10.62992F;
            // 
            // lbldate
            // 
            this.lbldate.Height = 0.2F;
            this.lbldate.HyperLink = null;
            this.lbldate.Left = 8.809055F;
            this.lbldate.Name = "lbldate";
            this.lbldate.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lbldate.Text = "出力日付 :";
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
            // lblId
            // 
            this.lblId.Height = 0.2204724F;
            this.lblId.HyperLink = null;
            this.lblId.Left = 0F;
            this.lblId.MultiLine = false;
            this.lblId.Name = "lblId";
            this.lblId.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblId.Text = "入金ID";
            this.lblId.Top = 0.964567F;
            this.lblId.Width = 0.8169292F;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Height = 0.2204724F;
            this.lblRecordedAt.HyperLink = null;
            this.lblRecordedAt.Left = 0.8307086F;
            this.lblRecordedAt.MultiLine = false;
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblRecordedAt.Text = "入金日";
            this.lblRecordedAt.Top = 0.9645669F;
            this.lblRecordedAt.Width = 0.7874016F;
            // 
            // lblDueAt
            // 
            this.lblDueAt.Height = 0.2137795F;
            this.lblDueAt.HyperLink = null;
            this.lblDueAt.Left = 0.8307086F;
            this.lblDueAt.MultiLine = false;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDueAt.Text = "期日";
            this.lblDueAt.Top = 1.182284F;
            this.lblDueAt.Width = 0.7874016F;
            // 
            // lblCategoryCodeName
            // 
            this.lblCategoryCodeName.Height = 0.2204724F;
            this.lblCategoryCodeName.HyperLink = null;
            this.lblCategoryCodeName.Left = 1.649606F;
            this.lblCategoryCodeName.MultiLine = false;
            this.lblCategoryCodeName.Name = "lblCategoryCodeName";
            this.lblCategoryCodeName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCategoryCodeName.Text = "入金区分";
            this.lblCategoryCodeName.Top = 0.9645669F;
            this.lblCategoryCodeName.Width = 0.7874016F;
            // 
            // lblInputType
            // 
            this.lblInputType.Height = 0.2137795F;
            this.lblInputType.HyperLink = null;
            this.lblInputType.Left = 1.649606F;
            this.lblInputType.Name = "lblInputType";
            this.lblInputType.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblInputType.Text = "入力区分";
            this.lblInputType.Top = 1.182284F;
            this.lblInputType.Width = 0.7874016F;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Height = 0.2204724F;
            this.lblPayerName.HyperLink = null;
            this.lblPayerName.Left = 2.46063F;
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.Top = 0.9645669F;
            this.lblPayerName.Width = 1.488189F;
            // 
            // lblNote1
            // 
            this.lblNote1.Height = 0.2137795F;
            this.lblNote1.HyperLink = null;
            this.lblNote1.Left = 2.46063F;
            this.lblNote1.Name = "lblNote1";
            this.lblNote1.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblNote1.Text = "備考";
            this.lblNote1.Top = 1.182284F;
            this.lblNote1.Width = 1.488189F;
            // 
            // lblSourceAmount
            // 
            this.lblSourceAmount.Height = 0.427559F;
            this.lblSourceAmount.HyperLink = null;
            this.lblSourceAmount.Left = 3.972441F;
            this.lblSourceAmount.Name = "lblSourceAmount";
            this.lblSourceAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle; ddo-shr" +
    "ink-to-fit: none";
            this.lblSourceAmount.Text = "振替前入金額";
            this.lblSourceAmount.Top = 0.9645669F;
            this.lblSourceAmount.Width = 0.984252F;
            // 
            // lblSourceSection
            // 
            this.lblSourceSection.Height = 0.427559F;
            this.lblSourceSection.HyperLink = null;
            this.lblSourceSection.Left = 4.968504F;
            this.lblSourceSection.Name = "lblSourceSection";
            this.lblSourceSection.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblSourceSection.Text = "振替前入金部門";
            this.lblSourceSection.Top = 0.9645669F;
            this.lblSourceSection.Width = 0.984252F;
            // 
            // lblDestinationSection
            // 
            this.lblDestinationSection.Height = 0.427559F;
            this.lblDestinationSection.HyperLink = null;
            this.lblDestinationSection.Left = 6.188977F;
            this.lblDestinationSection.Name = "lblDestinationSection";
            this.lblDestinationSection.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDestinationSection.Text = "振替後入金部門";
            this.lblDestinationSection.Top = 0.9645669F;
            this.lblDestinationSection.Width = 0.984252F;
            // 
            // lblDestinationAmount
            // 
            this.lblDestinationAmount.Height = 0.427559F;
            this.lblDestinationAmount.HyperLink = null;
            this.lblDestinationAmount.Left = 7.188977F;
            this.lblDestinationAmount.Name = "lblDestinationAmount";
            this.lblDestinationAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDestinationAmount.Text = "振替金額";
            this.lblDestinationAmount.Top = 0.9645669F;
            this.lblDestinationAmount.Width = 0.984252F;
            // 
            // lblCreditAt
            // 
            this.lblCreditAt.Height = 0.2204724F;
            this.lblCreditAt.HyperLink = null;
            this.lblCreditAt.Left = 8.188976F;
            this.lblCreditAt.Name = "lblCreditAt";
            this.lblCreditAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCreditAt.Text = "振替日時";
            this.lblCreditAt.Top = 0.9645669F;
            this.lblCreditAt.Width = 1.488189F;
            // 
            // lblMemo
            // 
            this.lblMemo.Height = 0.2137795F;
            this.lblMemo.HyperLink = null;
            this.lblMemo.Left = 8.188976F;
            this.lblMemo.Name = "lblMemo";
            this.lblMemo.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMemo.Text = "振替メモ";
            this.lblMemo.Top = 1.182284F;
            this.lblMemo.Width = 1.488189F;
            // 
            // lblLoginUserCode
            // 
            this.lblLoginUserCode.Height = 0.427559F;
            this.lblLoginUserCode.HyperLink = null;
            this.lblLoginUserCode.Left = 9.701575F;
            this.lblLoginUserCode.Name = "lblLoginUserCode";
            this.lblLoginUserCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblLoginUserCode.Text = "実行ユーザー";
            this.lblLoginUserCode.Top = 0.9645669F;
            this.lblLoginUserCode.Width = 0.9283465F;
            // 
            // line1
            // 
            this.line1.Height = 0F;
            this.line1.Left = 0F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0.9606299F;
            this.line1.Width = 5.963386F;
            this.line1.X1 = 0F;
            this.line1.X2 = 5.963386F;
            this.line1.Y1 = 0.9606299F;
            this.line1.Y2 = 0.9606299F;
            // 
            // lineHeaderVerId
            // 
            this.lineHeaderVerId.Height = 0.4480311F;
            this.lineHeaderVerId.Left = 0.8267716F;
            this.lineHeaderVerId.LineWeight = 1F;
            this.lineHeaderVerId.Name = "lineHeaderVerId";
            this.lineHeaderVerId.Top = 0.9606299F;
            this.lineHeaderVerId.Width = 0F;
            this.lineHeaderVerId.X1 = 0.8267716F;
            this.lineHeaderVerId.X2 = 0.8267716F;
            this.lineHeaderVerId.Y1 = 0.9606299F;
            this.lineHeaderVerId.Y2 = 1.408661F;
            // 
            // lineHeaderVerRecordedAt
            // 
            this.lineHeaderVerRecordedAt.Height = 0.4480311F;
            this.lineHeaderVerRecordedAt.Left = 1.633858F;
            this.lineHeaderVerRecordedAt.LineWeight = 1F;
            this.lineHeaderVerRecordedAt.Name = "lineHeaderVerRecordedAt";
            this.lineHeaderVerRecordedAt.Top = 0.9606299F;
            this.lineHeaderVerRecordedAt.Width = 0F;
            this.lineHeaderVerRecordedAt.X1 = 1.633858F;
            this.lineHeaderVerRecordedAt.X2 = 1.633858F;
            this.lineHeaderVerRecordedAt.Y1 = 0.9606299F;
            this.lineHeaderVerRecordedAt.Y2 = 1.408661F;
            // 
            // lineHeaderVerCategoryCode
            // 
            this.lineHeaderVerCategoryCode.Height = 0.4480311F;
            this.lineHeaderVerCategoryCode.Left = 2.444882F;
            this.lineHeaderVerCategoryCode.LineWeight = 1F;
            this.lineHeaderVerCategoryCode.Name = "lineHeaderVerCategoryCode";
            this.lineHeaderVerCategoryCode.Top = 0.9606299F;
            this.lineHeaderVerCategoryCode.Width = 0F;
            this.lineHeaderVerCategoryCode.X1 = 2.444882F;
            this.lineHeaderVerCategoryCode.X2 = 2.444882F;
            this.lineHeaderVerCategoryCode.Y1 = 0.9606299F;
            this.lineHeaderVerCategoryCode.Y2 = 1.408661F;
            // 
            // lineHeaderVerPayerName
            // 
            this.lineHeaderVerPayerName.Height = 0.4480311F;
            this.lineHeaderVerPayerName.Left = 3.956693F;
            this.lineHeaderVerPayerName.LineWeight = 1F;
            this.lineHeaderVerPayerName.Name = "lineHeaderVerPayerName";
            this.lineHeaderVerPayerName.Top = 0.9606299F;
            this.lineHeaderVerPayerName.Width = 0F;
            this.lineHeaderVerPayerName.X1 = 3.956693F;
            this.lineHeaderVerPayerName.X2 = 3.956693F;
            this.lineHeaderVerPayerName.Y1 = 0.9606299F;
            this.lineHeaderVerPayerName.Y2 = 1.408661F;
            // 
            // lineHeaderVerSourceAmt
            // 
            this.lineHeaderVerSourceAmt.Height = 0.4480311F;
            this.lineHeaderVerSourceAmt.Left = 4.96063F;
            this.lineHeaderVerSourceAmt.LineWeight = 1F;
            this.lineHeaderVerSourceAmt.Name = "lineHeaderVerSourceAmt";
            this.lineHeaderVerSourceAmt.Top = 0.9606299F;
            this.lineHeaderVerSourceAmt.Width = 0F;
            this.lineHeaderVerSourceAmt.X1 = 4.96063F;
            this.lineHeaderVerSourceAmt.X2 = 4.96063F;
            this.lineHeaderVerSourceAmt.Y1 = 0.9606299F;
            this.lineHeaderVerSourceAmt.Y2 = 1.408661F;
            // 
            // line9
            // 
            this.line9.Height = 0.4480311F;
            this.line9.Left = 6.173229F;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.Top = 0.9606299F;
            this.line9.Width = 0F;
            this.line9.X1 = 6.173229F;
            this.line9.X2 = 6.173229F;
            this.line9.Y1 = 0.9606299F;
            this.line9.Y2 = 1.408661F;
            // 
            // lineHeaderVerSourceSection
            // 
            this.lineHeaderVerSourceSection.Height = 0.4480311F;
            this.lineHeaderVerSourceSection.Left = 5.96063F;
            this.lineHeaderVerSourceSection.LineWeight = 1F;
            this.lineHeaderVerSourceSection.Name = "lineHeaderVerSourceSection";
            this.lineHeaderVerSourceSection.Top = 0.9606299F;
            this.lineHeaderVerSourceSection.Width = 0F;
            this.lineHeaderVerSourceSection.X1 = 5.96063F;
            this.lineHeaderVerSourceSection.X2 = 5.96063F;
            this.lineHeaderVerSourceSection.Y1 = 0.9606299F;
            this.lineHeaderVerSourceSection.Y2 = 1.408661F;
            // 
            // line11
            // 
            this.line11.Height = 0.4440941F;
            this.line11.Left = 8.173228F;
            this.line11.LineWeight = 1F;
            this.line11.Name = "line11";
            this.line11.Top = 0.9645669F;
            this.line11.Width = 0F;
            this.line11.X1 = 8.173228F;
            this.line11.X2 = 8.173228F;
            this.line11.Y1 = 0.9645669F;
            this.line11.Y2 = 1.408661F;
            // 
            // line12
            // 
            this.line12.Height = 0.4480311F;
            this.line12.Left = 7.173228F;
            this.line12.LineWeight = 1F;
            this.line12.Name = "line12";
            this.line12.Top = 0.9606299F;
            this.line12.Width = 0F;
            this.line12.X1 = 7.173228F;
            this.line12.X2 = 7.173228F;
            this.line12.Y1 = 0.9606299F;
            this.line12.Y2 = 1.408661F;
            // 
            // line13
            // 
            this.line13.Height = 0.4440941F;
            this.line13.Left = 9.689764F;
            this.line13.LineWeight = 1F;
            this.line13.Name = "line13";
            this.line13.Top = 0.9645669F;
            this.line13.Width = 0F;
            this.line13.X1 = 9.689764F;
            this.line13.X2 = 9.689764F;
            this.line13.Y1 = 0.9645669F;
            this.line13.Y2 = 1.408661F;
            // 
            // line24
            // 
            this.line24.Height = 0F;
            this.line24.Left = 6.173229F;
            this.line24.LineWeight = 1F;
            this.line24.Name = "line24";
            this.line24.Top = 0.9606299F;
            this.line24.Width = 4.56142F;
            this.line24.X1 = 6.173229F;
            this.line24.X2 = 10.73465F;
            this.line24.Y1 = 0.9606299F;
            this.line24.Y2 = 0.9606299F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.811811F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; ddo-char-set: 1";
            this.lblCompanyCode.Text = "label1";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 3.657087F;
            // 
            // lineHeaderHorLowerSource
            // 
            this.lineHeaderHorLowerSource.Height = 0F;
            this.lineHeaderHorLowerSource.Left = 0F;
            this.lineHeaderHorLowerSource.LineWeight = 1F;
            this.lineHeaderHorLowerSource.Name = "lineHeaderHorLowerSource";
            this.lineHeaderHorLowerSource.Top = 1.408661F;
            this.lineHeaderHorLowerSource.Width = 5.963386F;
            this.lineHeaderHorLowerSource.X1 = 0F;
            this.lineHeaderHorLowerSource.X2 = 5.963386F;
            this.lineHeaderHorLowerSource.Y1 = 1.408661F;
            this.lineHeaderHorLowerSource.Y2 = 1.408661F;
            // 
            // lineHeaderHorLowerDest
            // 
            this.lineHeaderHorLowerDest.Height = 0F;
            this.lineHeaderHorLowerDest.Left = 6.173229F;
            this.lineHeaderHorLowerDest.LineWeight = 1F;
            this.lineHeaderHorLowerDest.Name = "lineHeaderHorLowerDest";
            this.lineHeaderHorLowerDest.Top = 1.408661F;
            this.lineHeaderHorLowerDest.Width = 4.56142F;
            this.lineHeaderHorLowerDest.X1 = 6.173229F;
            this.lineHeaderHorLowerDest.X2 = 10.73465F;
            this.lineHeaderHorLowerDest.Y1 = 1.408661F;
            this.lineHeaderHorLowerDest.Y2 = 1.408661F;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Height = 0.2137795F;
            this.lblCurrencyCode.HyperLink = null;
            this.lblCurrencyCode.Left = 0F;
            this.lblCurrencyCode.MultiLine = false;
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.Top = 1.182284F;
            this.lblCurrencyCode.Width = 0.8169292F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtId,
            this.txtRecordedAt,
            this.txtDueAt,
            this.txtCategoryCodeName,
            this.txtInputType,
            this.txtPayerName,
            this.txtNote1,
            this.txtSourceAmount,
            this.txtSourceSectionCode,
            this.txtSourceSectionName,
            this.lblArrow,
            this.txtDestinationSectionCode,
            this.txtDestinationSectionName,
            this.txtDestinationAmount,
            this.txtCreateAt,
            this.txtMemo,
            this.txtLoginUserCode,
            this.txtLoginUserName,
            this.lineDetailVerId,
            this.lineDetailVerRecordedAt,
            this.lineDetailVerCategoryCode,
            this.lineDetailVerPayerName,
            this.lineDetailVerSourceAmt,
            this.lineDetailVerSoruceSection,
            this.lineDetailVerArrow,
            this.lineDetailVerDestSection,
            this.lineDetailVerDestinationAmt,
            this.lineDetailVerCreateAt,
            this.lineDetailHorSource,
            this.line25,
            this.txtCurrencyCode});
            this.detail.Height = 0.427559F;
            this.detail.Name = "detail";
            // 
            // txtId
            // 
            this.txtId.Height = 0.2137795F;
            this.txtId.Left = 0F;
            this.txtId.MultiLine = false;
            this.txtId.Name = "txtId";
            this.txtId.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtId.Style = "text-align: right; vertical-align: middle";
            this.txtId.Text = "txtId";
            this.txtId.Top = 0F;
            this.txtId.Width = 0.8169292F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.2137795F;
            this.txtRecordedAt.Left = 0.8307086F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "text-align: center; vertical-align: middle";
            this.txtRecordedAt.Text = "txtRecordedAt";
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.7874016F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.Height = 0.2137795F;
            this.txtDueAt.Left = 0.8307086F;
            this.txtDueAt.MultiLine = false;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "text-align: center; vertical-align: middle";
            this.txtDueAt.Text = "txtDueAt";
            this.txtDueAt.Top = 0.2137795F;
            this.txtDueAt.Width = 0.7874016F;
            // 
            // txtCategoryCodeName
            // 
            this.txtCategoryCodeName.Height = 0.2137795F;
            this.txtCategoryCodeName.Left = 1.649606F;
            this.txtCategoryCodeName.MultiLine = false;
            this.txtCategoryCodeName.Name = "txtCategoryCodeName";
            this.txtCategoryCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCategoryCodeName.Style = "text-align: left; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; d" +
    "do-wrap-mode: nowrap";
            this.txtCategoryCodeName.Text = "txtCategoryCodeName";
            this.txtCategoryCodeName.Top = 0F;
            this.txtCategoryCodeName.Width = 0.7874016F;
            // 
            // txtInputType
            // 
            this.txtInputType.Height = 0.2137795F;
            this.txtInputType.Left = 1.649606F;
            this.txtInputType.MultiLine = false;
            this.txtInputType.Name = "txtInputType";
            this.txtInputType.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtInputType.Style = "text-align: left; vertical-align: middle";
            this.txtInputType.Text = "txtInputType";
            this.txtInputType.Top = 0.2137795F;
            this.txtInputType.Width = 0.7874016F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.2137795F;
            this.txtPayerName.Left = 2.46063F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtPayerName.Style = "text-align: left; vertical-align: middle";
            this.txtPayerName.Text = "txtPayerName";
            this.txtPayerName.Top = 0F;
            this.txtPayerName.Width = 1.488189F;
            // 
            // txtNote1
            // 
            this.txtNote1.Height = 0.2137795F;
            this.txtNote1.Left = 2.456693F;
            this.txtNote1.MultiLine = false;
            this.txtNote1.Name = "txtNote1";
            this.txtNote1.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtNote1.Style = "text-align: left; vertical-align: middle";
            this.txtNote1.Text = "txtNote1";
            this.txtNote1.Top = 0.2137795F;
            this.txtNote1.Width = 1.488189F;
            // 
            // txtSourceAmount
            // 
            this.txtSourceAmount.Height = 0.4275591F;
            this.txtSourceAmount.Left = 3.964567F;
            this.txtSourceAmount.MultiLine = false;
            this.txtSourceAmount.Name = "txtSourceAmount";
            this.txtSourceAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtSourceAmount.Style = "text-align: right; vertical-align: middle";
            this.txtSourceAmount.Text = "txtSourceAmount";
            this.txtSourceAmount.Top = 0F;
            this.txtSourceAmount.Width = 0.9842521F;
            // 
            // txtSourceSectionCode
            // 
            this.txtSourceSectionCode.Height = 0.2137795F;
            this.txtSourceSectionCode.Left = 4.964567F;
            this.txtSourceSectionCode.MultiLine = false;
            this.txtSourceSectionCode.Name = "txtSourceSectionCode";
            this.txtSourceSectionCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtSourceSectionCode.Style = "text-align: left; vertical-align: middle";
            this.txtSourceSectionCode.Text = "txtSourceSectionCode";
            this.txtSourceSectionCode.Top = 0F;
            this.txtSourceSectionCode.Width = 0.984252F;
            // 
            // txtSourceSectionName
            // 
            this.txtSourceSectionName.Height = 0.2137795F;
            this.txtSourceSectionName.Left = 4.964567F;
            this.txtSourceSectionName.MultiLine = false;
            this.txtSourceSectionName.Name = "txtSourceSectionName";
            this.txtSourceSectionName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtSourceSectionName.Style = "text-align: left; vertical-align: middle";
            this.txtSourceSectionName.Text = "txtSourceSectionName";
            this.txtSourceSectionName.Top = 0.2137795F;
            this.txtSourceSectionName.Width = 0.984252F;
            // 
            // lblArrow
            // 
            this.lblArrow.Height = 0.4275591F;
            this.lblArrow.HyperLink = null;
            this.lblArrow.Left = 5.972441F;
            this.lblArrow.Name = "lblArrow";
            this.lblArrow.Style = "text-align: center; vertical-align: middle";
            this.lblArrow.Text = "→";
            this.lblArrow.Top = 0F;
            this.lblArrow.Width = 0.1968504F;
            // 
            // txtDestinationSectionCode
            // 
            this.txtDestinationSectionCode.Height = 0.2137795F;
            this.txtDestinationSectionCode.Left = 6.188977F;
            this.txtDestinationSectionCode.MultiLine = false;
            this.txtDestinationSectionCode.Name = "txtDestinationSectionCode";
            this.txtDestinationSectionCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtDestinationSectionCode.Style = "text-align: left; vertical-align: middle";
            this.txtDestinationSectionCode.Text = "txtDestinationSectionCode";
            this.txtDestinationSectionCode.Top = 0F;
            this.txtDestinationSectionCode.Width = 0.984252F;
            // 
            // txtDestinationSectionName
            // 
            this.txtDestinationSectionName.Height = 0.2137795F;
            this.txtDestinationSectionName.Left = 6.188977F;
            this.txtDestinationSectionName.MultiLine = false;
            this.txtDestinationSectionName.Name = "txtDestinationSectionName";
            this.txtDestinationSectionName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtDestinationSectionName.Style = "text-align: left; vertical-align: middle";
            this.txtDestinationSectionName.Text = "txtDestinationSectionName";
            this.txtDestinationSectionName.Top = 0.2137795F;
            this.txtDestinationSectionName.Width = 0.984252F;
            // 
            // txtDestinationAmount
            // 
            this.txtDestinationAmount.Height = 0.4275591F;
            this.txtDestinationAmount.Left = 7.188977F;
            this.txtDestinationAmount.MultiLine = false;
            this.txtDestinationAmount.Name = "txtDestinationAmount";
            this.txtDestinationAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtDestinationAmount.Style = "text-align: right; vertical-align: middle";
            this.txtDestinationAmount.Text = "txtDestinationAmount";
            this.txtDestinationAmount.Top = 0F;
            this.txtDestinationAmount.Width = 0.984252F;
            // 
            // txtCreateAt
            // 
            this.txtCreateAt.Height = 0.2137795F;
            this.txtCreateAt.Left = 8.188977F;
            this.txtCreateAt.MultiLine = false;
            this.txtCreateAt.Name = "txtCreateAt";
            this.txtCreateAt.OutputFormat = resources.GetString("txtCreateAt.OutputFormat");
            this.txtCreateAt.Style = "text-align: center; vertical-align: middle";
            this.txtCreateAt.Text = "txtCreateAt";
            this.txtCreateAt.Top = 0F;
            this.txtCreateAt.Width = 1.488189F;
            // 
            // txtMemo
            // 
            this.txtMemo.Height = 0.2137795F;
            this.txtMemo.Left = 8.188977F;
            this.txtMemo.MultiLine = false;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtMemo.Style = "text-align: left; vertical-align: middle";
            this.txtMemo.Text = "txtMemo";
            this.txtMemo.Top = 0.2137795F;
            this.txtMemo.Width = 1.488189F;
            // 
            // txtLoginUserCode
            // 
            this.txtLoginUserCode.Height = 0.2137795F;
            this.txtLoginUserCode.Left = 9.701575F;
            this.txtLoginUserCode.MultiLine = false;
            this.txtLoginUserCode.Name = "txtLoginUserCode";
            this.txtLoginUserCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtLoginUserCode.Style = "text-align: left; vertical-align: middle";
            this.txtLoginUserCode.Text = "txtLoginUserCode";
            this.txtLoginUserCode.Top = 0F;
            this.txtLoginUserCode.Width = 1.025197F;
            // 
            // txtLoginUserName
            // 
            this.txtLoginUserName.Height = 0.2137795F;
            this.txtLoginUserName.Left = 9.701575F;
            this.txtLoginUserName.MultiLine = false;
            this.txtLoginUserName.Name = "txtLoginUserName";
            this.txtLoginUserName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtLoginUserName.Style = "text-align: left; vertical-align: middle";
            this.txtLoginUserName.Text = "txtLoginUserName";
            this.txtLoginUserName.Top = 0.2137795F;
            this.txtLoginUserName.Width = 1.025197F;
            // 
            // lineDetailVerId
            // 
            this.lineDetailVerId.Height = 0.427559F;
            this.lineDetailVerId.Left = 0.8267716F;
            this.lineDetailVerId.LineWeight = 1F;
            this.lineDetailVerId.Name = "lineDetailVerId";
            this.lineDetailVerId.Top = 0F;
            this.lineDetailVerId.Width = 0F;
            this.lineDetailVerId.X1 = 0.8267716F;
            this.lineDetailVerId.X2 = 0.8267716F;
            this.lineDetailVerId.Y1 = 0F;
            this.lineDetailVerId.Y2 = 0.427559F;
            // 
            // lineDetailVerRecordedAt
            // 
            this.lineDetailVerRecordedAt.Height = 0.4275589F;
            this.lineDetailVerRecordedAt.Left = 1.633858F;
            this.lineDetailVerRecordedAt.LineWeight = 1F;
            this.lineDetailVerRecordedAt.Name = "lineDetailVerRecordedAt";
            this.lineDetailVerRecordedAt.Top = 0F;
            this.lineDetailVerRecordedAt.Width = 0F;
            this.lineDetailVerRecordedAt.X1 = 1.633858F;
            this.lineDetailVerRecordedAt.X2 = 1.633858F;
            this.lineDetailVerRecordedAt.Y1 = 0F;
            this.lineDetailVerRecordedAt.Y2 = 0.4275589F;
            // 
            // lineDetailVerCategoryCode
            // 
            this.lineDetailVerCategoryCode.Height = 0.427559F;
            this.lineDetailVerCategoryCode.Left = 2.444882F;
            this.lineDetailVerCategoryCode.LineWeight = 1F;
            this.lineDetailVerCategoryCode.Name = "lineDetailVerCategoryCode";
            this.lineDetailVerCategoryCode.Top = 0F;
            this.lineDetailVerCategoryCode.Width = 0F;
            this.lineDetailVerCategoryCode.X1 = 2.444882F;
            this.lineDetailVerCategoryCode.X2 = 2.444882F;
            this.lineDetailVerCategoryCode.Y1 = 0F;
            this.lineDetailVerCategoryCode.Y2 = 0.427559F;
            // 
            // lineDetailVerPayerName
            // 
            this.lineDetailVerPayerName.Height = 0.427559F;
            this.lineDetailVerPayerName.Left = 3.956693F;
            this.lineDetailVerPayerName.LineWeight = 1F;
            this.lineDetailVerPayerName.Name = "lineDetailVerPayerName";
            this.lineDetailVerPayerName.Top = 0F;
            this.lineDetailVerPayerName.Width = 0F;
            this.lineDetailVerPayerName.X1 = 3.956693F;
            this.lineDetailVerPayerName.X2 = 3.956693F;
            this.lineDetailVerPayerName.Y1 = 0F;
            this.lineDetailVerPayerName.Y2 = 0.427559F;
            // 
            // lineDetailVerSourceAmt
            // 
            this.lineDetailVerSourceAmt.Height = 0.427559F;
            this.lineDetailVerSourceAmt.Left = 4.96063F;
            this.lineDetailVerSourceAmt.LineWeight = 1F;
            this.lineDetailVerSourceAmt.Name = "lineDetailVerSourceAmt";
            this.lineDetailVerSourceAmt.Top = 0F;
            this.lineDetailVerSourceAmt.Width = 0F;
            this.lineDetailVerSourceAmt.X1 = 4.96063F;
            this.lineDetailVerSourceAmt.X2 = 4.96063F;
            this.lineDetailVerSourceAmt.Y1 = 0F;
            this.lineDetailVerSourceAmt.Y2 = 0.427559F;
            // 
            // lineDetailVerSoruceSection
            // 
            this.lineDetailVerSoruceSection.Height = 0.427559F;
            this.lineDetailVerSoruceSection.Left = 5.96063F;
            this.lineDetailVerSoruceSection.LineWeight = 1F;
            this.lineDetailVerSoruceSection.Name = "lineDetailVerSoruceSection";
            this.lineDetailVerSoruceSection.Top = 0F;
            this.lineDetailVerSoruceSection.Width = 0F;
            this.lineDetailVerSoruceSection.X1 = 5.96063F;
            this.lineDetailVerSoruceSection.X2 = 5.96063F;
            this.lineDetailVerSoruceSection.Y1 = 0F;
            this.lineDetailVerSoruceSection.Y2 = 0.427559F;
            // 
            // lineDetailVerArrow
            // 
            this.lineDetailVerArrow.Height = 0.427559F;
            this.lineDetailVerArrow.Left = 6.173229F;
            this.lineDetailVerArrow.LineWeight = 1F;
            this.lineDetailVerArrow.Name = "lineDetailVerArrow";
            this.lineDetailVerArrow.Top = 0F;
            this.lineDetailVerArrow.Width = 0F;
            this.lineDetailVerArrow.X1 = 6.173229F;
            this.lineDetailVerArrow.X2 = 6.173229F;
            this.lineDetailVerArrow.Y1 = 0F;
            this.lineDetailVerArrow.Y2 = 0.427559F;
            // 
            // lineDetailVerDestSection
            // 
            this.lineDetailVerDestSection.Height = 0.427559F;
            this.lineDetailVerDestSection.Left = 7.173229F;
            this.lineDetailVerDestSection.LineWeight = 1F;
            this.lineDetailVerDestSection.Name = "lineDetailVerDestSection";
            this.lineDetailVerDestSection.Top = 0F;
            this.lineDetailVerDestSection.Width = 0F;
            this.lineDetailVerDestSection.X1 = 7.173229F;
            this.lineDetailVerDestSection.X2 = 7.173229F;
            this.lineDetailVerDestSection.Y1 = 0F;
            this.lineDetailVerDestSection.Y2 = 0.427559F;
            // 
            // lineDetailVerDestinationAmt
            // 
            this.lineDetailVerDestinationAmt.Height = 0.427559F;
            this.lineDetailVerDestinationAmt.Left = 8.173229F;
            this.lineDetailVerDestinationAmt.LineWeight = 1F;
            this.lineDetailVerDestinationAmt.Name = "lineDetailVerDestinationAmt";
            this.lineDetailVerDestinationAmt.Top = 0F;
            this.lineDetailVerDestinationAmt.Width = 0F;
            this.lineDetailVerDestinationAmt.X1 = 8.173229F;
            this.lineDetailVerDestinationAmt.X2 = 8.173229F;
            this.lineDetailVerDestinationAmt.Y1 = 0F;
            this.lineDetailVerDestinationAmt.Y2 = 0.427559F;
            // 
            // lineDetailVerCreateAt
            // 
            this.lineDetailVerCreateAt.Height = 0.4275589F;
            this.lineDetailVerCreateAt.Left = 9.689764F;
            this.lineDetailVerCreateAt.LineWeight = 1F;
            this.lineDetailVerCreateAt.Name = "lineDetailVerCreateAt";
            this.lineDetailVerCreateAt.Top = 0F;
            this.lineDetailVerCreateAt.Width = 0F;
            this.lineDetailVerCreateAt.X1 = 9.689764F;
            this.lineDetailVerCreateAt.X2 = 9.689764F;
            this.lineDetailVerCreateAt.Y1 = 0F;
            this.lineDetailVerCreateAt.Y2 = 0.4275589F;
            // 
            // lineDetailHorSource
            // 
            this.lineDetailHorSource.Height = 0F;
            this.lineDetailHorSource.Left = 0F;
            this.lineDetailHorSource.LineWeight = 1F;
            this.lineDetailHorSource.Name = "lineDetailHorSource";
            this.lineDetailHorSource.Top = 0.4275591F;
            this.lineDetailHorSource.Width = 5.963386F;
            this.lineDetailHorSource.X1 = 0F;
            this.lineDetailHorSource.X2 = 5.963386F;
            this.lineDetailHorSource.Y1 = 0.4275591F;
            this.lineDetailHorSource.Y2 = 0.4275591F;
            // 
            // line25
            // 
            this.line25.Height = 0F;
            this.line25.Left = 6.173229F;
            this.line25.LineWeight = 1F;
            this.line25.Name = "line25";
            this.line25.Top = 0.4275591F;
            this.line25.Width = 4.56142F;
            this.line25.X1 = 6.173229F;
            this.line25.X2 = 10.73465F;
            this.line25.Y1 = 0.4275591F;
            this.line25.Y2 = 0.4275591F;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.Height = 0.2137795F;
            this.txtCurrencyCode.Left = 0F;
            this.txtCurrencyCode.MultiLine = false;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtCurrencyCode.Style = "text-align: center; vertical-align: middle";
            this.txtCurrencyCode.Text = "txtCurrencyCode";
            this.txtCurrencyCode.Top = 0.2137795F;
            this.txtCurrencyCode.Width = 0.8169292F;
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
            // ghCurrency
            // 
            this.ghCurrency.DataField = "CurrencyCode";
            this.ghCurrency.Height = 0F;
            this.ghCurrency.Name = "ghCurrency";
            this.ghCurrency.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            this.ghCurrency.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
            // 
            // groupFooter1
            // 
            this.groupFooter1.Height = 0F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // ReceiptSectionTransferSectionReport
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
            this.Sections.Add(this.ghCurrency);
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
            this.DataInitialize += new System.EventHandler(this.ReceiptSectionTransferSectionReport_DataInitialize);
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategoryCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSourceSection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDestinationSection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDestinationAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMemo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUserCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompany;
        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblId;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCategoryCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInputType;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSourceAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSourceSection;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDestinationSection;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDestinationAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblMemo;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblLoginUserCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtId;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCategoryCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInputType;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSourceAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSourceSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSourceSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblArrow;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDestinationSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDestinationSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDestinationAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMemo;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLoginUserCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLoginUserName;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSourceAmt;
        private GrapeCity.ActiveReports.SectionReportModel.Line line9;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSourceSection;
        private GrapeCity.ActiveReports.SectionReportModel.Line line11;
        private GrapeCity.ActiveReports.SectionReportModel.Line line12;
        private GrapeCity.ActiveReports.SectionReportModel.Line line13;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSourceAmt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSoruceSection;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerArrow;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDestSection;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDestinationAmt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line24;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorSource;
        private GrapeCity.ActiveReports.SectionReportModel.Line line25;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLowerSource;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLowerDest;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghCurrency;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
    }
}
