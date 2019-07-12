namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// RiminderListByCustomerReport の概要の説明です。
    /// </summary>
    partial class ReminderListByCustomerReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReminderListByCustomerReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompany = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.riDate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtReminderAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInputTypeName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreateBy = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMemo = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line19 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line22 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtCreateAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line14 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line24 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.riPageNumber = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblReminderAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAction = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreateBy = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblMemo = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreateAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line13 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label2 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReminderAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputTypeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReminderAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMemo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompany,
            this.lblCompanyCode,
            this.lblDate,
            this.riDate,
            this.lblTitle});
            this.pageHeader.Height = 0.6354661F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompany
            // 
            this.lblCompany.Height = 0.2F;
            this.lblCompany.HyperLink = null;
            this.lblCompany.Left = 0F;
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.lblCompany.Text = "会社コード　：";
            this.lblCompany.Top = 0F;
            this.lblCompany.Width = 0.7874014F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.7874014F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCode.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.lblCompanyCode.Text = "label2";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 3.657F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 8.784646F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.lblDate.Text = "出力日付：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.698425F;
            // 
            // riDate
            // 
            this.riDate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.riDate.Height = 0.2F;
            this.riDate.Left = 9.498028F;
            this.riDate.Name = "riDate";
            this.riDate.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; text-align: left; vertical-align" +
    ": middle; ddo-char-set: 1";
            this.riDate.Top = 0F;
            this.riDate.Width = 1.014961F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-family: ＭＳ 明朝; font-size: 14pt; text-align: center; text-decoration: underli" +
    "ne; ddo-char-set: 1";
            this.lblTitle.Text = "督促管理帳票";
            this.lblTitle.Top = 0.2F;
            this.lblTitle.Width = 10.62992F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtReminderAmount,
            this.txtInputTypeName,
            this.txtCreateBy,
            this.txtMemo,
            this.line19,
            this.line22,
            this.txtCreateAt,
            this.line14,
            this.line24,
            this.line2});
            this.detail.Height = 0.9583333F;
            this.detail.Name = "detail";
            // 
            // txtReminderAmount
            // 
            this.txtReminderAmount.Height = 0.4023622F;
            this.txtReminderAmount.Left = 1.383465F;
            this.txtReminderAmount.MultiLine = false;
            this.txtReminderAmount.Name = "txtReminderAmount";
            this.txtReminderAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtReminderAmount.Style = "font-family: ＭＳ 明朝; font-size: 9pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtReminderAmount.Text = "99,999,999,999";
            this.txtReminderAmount.Top = 0F;
            this.txtReminderAmount.Width = 1.13622F;
            // 
            // txtInputTypeName
            // 
            this.txtInputTypeName.Height = 0.4011811F;
            this.txtInputTypeName.Left = 2.522441F;
            this.txtInputTypeName.MultiLine = false;
            this.txtInputTypeName.Name = "txtInputTypeName";
            this.txtInputTypeName.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtInputTypeName.Style = "font-family: ＭＳ 明朝; font-size: 9pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtInputTypeName.Text = "入力";
            this.txtInputTypeName.Top = 0.001181102F;
            this.txtInputTypeName.Width = 0.9374015F;
            // 
            // txtCreateBy
            // 
            this.txtCreateBy.Height = 0.4F;
            this.txtCreateBy.Left = 9.500001F;
            this.txtCreateBy.MultiLine = false;
            this.txtCreateBy.Name = "txtCreateBy";
            this.txtCreateBy.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtCreateBy.Style = "font-family: ＭＳ 明朝; font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtCreateBy.Text = "小梅太夫";
            this.txtCreateBy.Top = 0.002362207F;
            this.txtCreateBy.Width = 1.07559F;
            // 
            // txtMemo
            // 
            this.txtMemo.Height = 0.4023622F;
            this.txtMemo.Left = 3.459843F;
            this.txtMemo.MultiLine = false;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtMemo.Style = "font-family: ＭＳ 明朝; font-size: 9pt; vertical-align: top; ddo-char-set: 1";
            this.txtMemo.Text = "入金未確認。要督促";
            this.txtMemo.Top = 0F;
            this.txtMemo.Width = 6.03819F;
            // 
            // line19
            // 
            this.line19.Height = 0.4023622F;
            this.line19.Left = 2.520473F;
            this.line19.LineWeight = 1F;
            this.line19.Name = "line19";
            this.line19.Top = 0F;
            this.line19.Width = 0.001968145F;
            this.line19.X1 = 2.522441F;
            this.line19.X2 = 2.520473F;
            this.line19.Y1 = 0F;
            this.line19.Y2 = 0.4023622F;
            // 
            // line22
            // 
            this.line22.Height = 0.4023622F;
            this.line22.Left = 3.46063F;
            this.line22.LineWeight = 1F;
            this.line22.Name = "line22";
            this.line22.Top = 0F;
            this.line22.Width = 0F;
            this.line22.X1 = 3.46063F;
            this.line22.X2 = 3.46063F;
            this.line22.Y1 = 0F;
            this.line22.Y2 = 0.4023622F;
            // 
            // txtCreateAt
            // 
            this.txtCreateAt.Height = 0.4023622F;
            this.txtCreateAt.Left = 0.001181103F;
            this.txtCreateAt.MultiLine = false;
            this.txtCreateAt.Name = "txtCreateAt";
            this.txtCreateAt.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtCreateAt.Style = "font-family: ＭＳ 明朝; font-size: 9pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtCreateAt.Text = "2018/01/01 12:00:00";
            this.txtCreateAt.Top = 0F;
            this.txtCreateAt.Width = 1.381102F;
            // 
            // line14
            // 
            this.line14.Height = 0.4011811F;
            this.line14.Left = 1.382284F;
            this.line14.LineWeight = 1F;
            this.line14.Name = "line14";
            this.line14.Top = 0.001181102F;
            this.line14.Width = 0.001181006F;
            this.line14.X1 = 1.383465F;
            this.line14.X2 = 1.382284F;
            this.line14.Y1 = 0.001181102F;
            this.line14.Y2 = 0.4023622F;
            // 
            // line24
            // 
            this.line24.Height = 0.4023622F;
            this.line24.Left = 9.498033F;
            this.line24.LineWeight = 1F;
            this.line24.Name = "line24";
            this.line24.Top = 0F;
            this.line24.Width = 0.001968384F;
            this.line24.X1 = 9.500001F;
            this.line24.X2 = 9.498033F;
            this.line24.Y1 = 0F;
            this.line24.Y2 = 0.4023622F;
            // 
            // line2
            // 
            this.line2.Height = 0F;
            this.line2.Left = 0F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 0.4023622F;
            this.line2.Width = 10.575F;
            this.line2.X1 = 0F;
            this.line2.X2 = 10.575F;
            this.line2.Y1 = 0.4023622F;
            this.line2.Y2 = 0.4023622F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.riPageNumber});
            this.pageFooter.Name = "pageFooter";
            // 
            // riPageNumber
            // 
            this.riPageNumber.FormatString = "{PageNumber} / {PageCount}";
            this.riPageNumber.Height = 0.2F;
            this.riPageNumber.Left = 0.01673126F;
            this.riPageNumber.Name = "riPageNumber";
            this.riPageNumber.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; text-align: center; vertical-ali" +
    "gn: middle; ddo-char-set: 1";
            this.riPageNumber.Top = 0.025F;
            this.riPageNumber.Width = 10.59646F;
            // 
            // lblReminderAmount
            // 
            this.lblReminderAmount.Height = 0.23F;
            this.lblReminderAmount.HyperLink = null;
            this.lblReminderAmount.Left = 1.383465F;
            this.lblReminderAmount.Name = "lblReminderAmount";
            this.lblReminderAmount.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblReminderAmount.Text = "滞留金額";
            this.lblReminderAmount.Top = 0.4035433F;
            this.lblReminderAmount.Width = 1.13622F;
            // 
            // lblAction
            // 
            this.lblAction.Height = 0.23F;
            this.lblAction.HyperLink = null;
            this.lblAction.Left = 2.522441F;
            this.lblAction.Name = "lblAction";
            this.lblAction.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblAction.Text = "アクション";
            this.lblAction.Top = 0.4023622F;
            this.lblAction.Width = 0.9374015F;
            // 
            // lblCreateBy
            // 
            this.lblCreateBy.Height = 0.23F;
            this.lblCreateBy.HyperLink = null;
            this.lblCreateBy.Left = 9.501182F;
            this.lblCreateBy.Name = "lblCreateBy";
            this.lblCreateBy.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblCreateBy.Text = "更新者名";
            this.lblCreateBy.Top = 0.4033622F;
            this.lblCreateBy.Width = 1.072603F;
            // 
            // line7
            // 
            this.line7.Height = 0.2299916F;
            this.line7.Left = 1.382091F;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.Top = 0.4035433F;
            this.line7.Width = 0.0001929998F;
            this.line7.X1 = 1.382284F;
            this.line7.X2 = 1.382091F;
            this.line7.Y1 = 0.4035433F;
            this.line7.Y2 = 0.6335349F;
            // 
            // line8
            // 
            this.line8.Height = 0.2299916F;
            this.line8.Left = 2.519685F;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.Top = 0.4035433F;
            this.line8.Width = 0.0008940697F;
            this.line8.X1 = 2.519685F;
            this.line8.X2 = 2.520579F;
            this.line8.Y1 = 0.4035433F;
            this.line8.Y2 = 0.6335349F;
            // 
            // line9
            // 
            this.line9.Height = 0.2299916F;
            this.line9.Left = 3.459843F;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.Top = 0.4035433F;
            this.line9.Width = 0.0008940697F;
            this.line9.X1 = 3.459843F;
            this.line9.X2 = 3.460737F;
            this.line9.Y1 = 0.4035433F;
            this.line9.Y2 = 0.6335349F;
            // 
            // lblMemo
            // 
            this.lblMemo.Height = 0.23F;
            this.lblMemo.HyperLink = null;
            this.lblMemo.Left = 3.46063F;
            this.lblMemo.Name = "lblMemo";
            this.lblMemo.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblMemo.Text = "対応記録";
            this.lblMemo.Top = 0.4035433F;
            this.lblMemo.Width = 6.022441F;
            // 
            // lblCreateAt
            // 
            this.lblCreateAt.Height = 0.23F;
            this.lblCreateAt.HyperLink = null;
            this.lblCreateAt.Left = 0.001180649F;
            this.lblCreateAt.Name = "lblCreateAt";
            this.lblCreateAt.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblCreateAt.Text = "更新日時";
            this.lblCreateAt.Top = 0.4035433F;
            this.lblCreateAt.Width = 1.381103F;
            // 
            // line3
            // 
            this.line3.Height = 0F;
            this.line3.Left = 0.0007875115F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0.4033622F;
            this.line3.Width = 10.57499F;
            this.line3.X1 = 0.0007875115F;
            this.line3.X2 = 10.57578F;
            this.line3.Y1 = 0.4033622F;
            this.line3.Y2 = 0.4033622F;
            // 
            // line13
            // 
            this.line13.Height = 0.2288103F;
            this.line13.Left = 9.500001F;
            this.line13.LineWeight = 1F;
            this.line13.Name = "line13";
            this.line13.Top = 0.4035433F;
            this.line13.Width = 0.001069069F;
            this.line13.X1 = 9.500001F;
            this.line13.X2 = 9.50107F;
            this.line13.Y1 = 0.4035433F;
            this.line13.Y2 = 0.6323537F;
            // 
            // label1
            // 
            this.label1.Height = 0.2F;
            this.label1.HyperLink = null;
            this.label1.Left = 0F;
            this.label1.Name = "label1";
            this.label1.Style = "color: Black; font-family: ＭＳ 明朝; font-size: 9pt; vertical-align: middle; ddo-cha" +
    "r-set: 1";
            this.label1.Text = "得意先コード：";
            this.label1.Top = 0.0001811981F;
            this.label1.Width = 0.9125985F;
            // 
            // label2
            // 
            this.label2.Height = 0.2F;
            this.label2.HyperLink = null;
            this.label2.Left = 0.001181126F;
            this.label2.Name = "label2";
            this.label2.Style = "color: Black; font-family: ＭＳ 明朝; font-size: 9pt; vertical-align: middle; ddo-cha" +
    "r-set: 1";
            this.label2.Text = "得意先名    ：";
            this.label2.Top = 0.2001812F;
            this.label2.Width = 0.9114174F;
            // 
            // groupHeader1
            // 
            this.groupHeader1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblReminderAmount,
            this.lblAction,
            this.lblCreateBy,
            this.line7,
            this.line8,
            this.line9,
            this.lblMemo,
            this.lblCreateAt,
            this.line3,
            this.line13,
            this.label1,
            this.label2,
            this.txtCustomerCode,
            this.txtCustomerName,
            this.line1});
            this.groupHeader1.Height = 0.9043766F;
            this.groupHeader1.Name = "groupHeader1";
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 0.9125985F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtCustomerCode.Style = "font-family: ＭＳ 明朝; font-size: 9pt; text-align: left; vertical-align: middle; ddo" +
    "-char-set: 1";
            this.txtCustomerCode.Text = "CustomerCode";
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 4.25F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 0.9125985F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Style = "font-family: ＭＳ 明朝; font-size: 9pt; text-align: left; vertical-align: middle; ddo" +
    "-char-set: 1";
            this.txtCustomerName.Text = "CustomerName";
            this.txtCustomerName.Top = 0.2F;
            this.txtCustomerName.Width = 4.25F;
            // 
            // line1
            // 
            this.line1.Height = 0F;
            this.line1.Left = 0F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0.6322835F;
            this.line1.Width = 10.57499F;
            this.line1.X1 = 0F;
            this.line1.X2 = 10.57499F;
            this.line1.Y1 = 0.6322835F;
            this.line1.Y2 = 0.6322835F;
            // 
            // groupFooter1
            // 
            this.groupFooter1.Name = "groupFooter1";
            // 
            // ReminderListByCustomerReport
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
            "color: Black; font-family: \"MS UI Gothic\"; ddo-char-set: 128", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReminderAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputTypeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReminderAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMemo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompany;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo riDate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReminderAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAction;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreateBy;
        private GrapeCity.ActiveReports.SectionReportModel.Line line7;
        private GrapeCity.ActiveReports.SectionReportModel.Line line8;
        private GrapeCity.ActiveReports.SectionReportModel.Line line9;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblMemo;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.Line line13;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReminderAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInputTypeName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreateBy;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMemo;
        private GrapeCity.ActiveReports.SectionReportModel.Line line19;
        private GrapeCity.ActiveReports.SectionReportModel.Line line22;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line line14;
        private GrapeCity.ActiveReports.SectionReportModel.Line line24;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo riPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        private GrapeCity.ActiveReports.SectionReportModel.Label label2;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
    }
}
