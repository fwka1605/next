namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// SectionMasterReport の概要の説明です。
    /// </summary>
    partial class SectionMasterReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SectionMasterReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lbltitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblSectionName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSectionName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblUpdateDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerUpdateDate = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblUpdateName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerUpdateName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.lineDetailVerSeparator = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerSectionName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtSectionName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerBranchCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtPayerCodeLeft = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerAccountNumber = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtPayerCodeRight = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerUpdateDate = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtUpdateDate = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtLoginUserName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerUpdateName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUpdateDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUpdateName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerCodeLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerCodeRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdateDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.label1,
            this.lblCompanyCode,
            this.lbldate,
            this.ridate,
            this.lbltitle,
            this.lblSectionCode,
            this.lblSectionName,
            this.lblBranchCode,
            this.lblAccountNumber,
            this.lineHeaderHorLower,
            this.lineHeaderVerSectionName,
            this.lineHeaderVerBranchCode,
            this.lineHeaderVerAccountNumber,
            this.lblUpdateDate,
            this.lineHeaderVerUpdateDate,
            this.lineHeaderHorUpper,
            this.lblUpdateName,
            this.lineHeaderVerUpdateName});
            this.pageHeader.Height = 0.8852527F;
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
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.811811F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCode.Text = "label2";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 3.657087F;
            // 
            // lbldate
            // 
            this.lbldate.Height = 0.2F;
            this.lbldate.HyperLink = null;
            this.lbldate.Left = 8.809055F;
            this.lbldate.Name = "lbldate";
            this.lbldate.Style = "color: Gray; font-size: 7pt; text-align: right; vertical-align: middle; ddo-char-" +
    "set: 1";
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
            this.lbltitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; vertical-align: " +
    "top";
            this.lbltitle.Text = "入金部門マスター一覧";
            this.lbltitle.Top = 0.2704725F;
            this.lbltitle.Width = 10.62992F;
            // 
            // lblSectionCode
            // 
            this.lblSectionCode.Height = 0.3267716F;
            this.lblSectionCode.Left = 0F;
            this.lblSectionCode.Name = "lblSectionCode";
            this.lblSectionCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblSectionCode.Text = "入金部門コード";
            this.lblSectionCode.Top = 0.5472441F;
            this.lblSectionCode.Width = 1.433465F;
            // 
            // lblSectionName
            // 
            this.lblSectionName.Height = 0.3267716F;
            this.lblSectionName.HyperLink = null;
            this.lblSectionName.Left = 1.44252F;
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblSectionName.Text = "入金部門名称";
            this.lblSectionName.Top = 0.5472441F;
            this.lblSectionName.Width = 3.037402F;
            // 
            // lblBranchCode
            // 
            this.lblBranchCode.Height = 0.3267716F;
            this.lblBranchCode.HyperLink = null;
            this.lblBranchCode.Left = 4.475985F;
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblBranchCode.Text = "仮想支店コード";
            this.lblBranchCode.Top = 0.5472441F;
            this.lblBranchCode.Width = 1.091339F;
            // 
            // lblAccountNumber
            // 
            this.lblAccountNumber.Height = 0.3267716F;
            this.lblAccountNumber.HyperLink = null;
            this.lblAccountNumber.Left = 5.577953F;
            this.lblAccountNumber.Name = "lblAccountNumber";
            this.lblAccountNumber.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblAccountNumber.Text = "仮想口座番号";
            this.lblAccountNumber.Top = 0.5472441F;
            this.lblAccountNumber.Width = 1.559449F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 0.8854331F;
            this.lineHeaderHorLower.Width = 10.62992F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.62992F;
            this.lineHeaderHorLower.Y1 = 0.8854331F;
            this.lineHeaderHorLower.Y2 = 0.8854331F;
            // 
            // lineHeaderVerSectionName
            // 
            this.lineHeaderVerSectionName.Height = 0.353937F;
            this.lineHeaderVerSectionName.Left = 1.44252F;
            this.lineHeaderVerSectionName.LineWeight = 1F;
            this.lineHeaderVerSectionName.Name = "lineHeaderVerSectionName";
            this.lineHeaderVerSectionName.Top = 0.5417323F;
            this.lineHeaderVerSectionName.Width = 0F;
            this.lineHeaderVerSectionName.X1 = 1.44252F;
            this.lineHeaderVerSectionName.X2 = 1.44252F;
            this.lineHeaderVerSectionName.Y1 = 0.5417323F;
            this.lineHeaderVerSectionName.Y2 = 0.8956693F;
            // 
            // lineHeaderVerBranchCode
            // 
            this.lineHeaderVerBranchCode.Height = 0.353937F;
            this.lineHeaderVerBranchCode.Left = 4.475985F;
            this.lineHeaderVerBranchCode.LineWeight = 1F;
            this.lineHeaderVerBranchCode.Name = "lineHeaderVerBranchCode";
            this.lineHeaderVerBranchCode.Top = 0.5417323F;
            this.lineHeaderVerBranchCode.Width = 0F;
            this.lineHeaderVerBranchCode.X1 = 4.475985F;
            this.lineHeaderVerBranchCode.X2 = 4.475985F;
            this.lineHeaderVerBranchCode.Y1 = 0.5417323F;
            this.lineHeaderVerBranchCode.Y2 = 0.8956693F;
            // 
            // lineHeaderVerAccountNumber
            // 
            this.lineHeaderVerAccountNumber.Height = 0.353937F;
            this.lineHeaderVerAccountNumber.Left = 5.577953F;
            this.lineHeaderVerAccountNumber.LineWeight = 1F;
            this.lineHeaderVerAccountNumber.Name = "lineHeaderVerAccountNumber";
            this.lineHeaderVerAccountNumber.Top = 0.5417323F;
            this.lineHeaderVerAccountNumber.Width = 0F;
            this.lineHeaderVerAccountNumber.X1 = 5.577953F;
            this.lineHeaderVerAccountNumber.X2 = 5.577953F;
            this.lineHeaderVerAccountNumber.Y1 = 0.5417323F;
            this.lineHeaderVerAccountNumber.Y2 = 0.8956693F;
            // 
            // lblUpdateDate
            // 
            this.lblUpdateDate.Height = 0.3267716F;
            this.lblUpdateDate.HyperLink = null;
            this.lblUpdateDate.Left = 7.137402F;
            this.lblUpdateDate.Name = "lblUpdateDate";
            this.lblUpdateDate.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblUpdateDate.Text = "最新更新日";
            this.lblUpdateDate.Top = 0.5472441F;
            this.lblUpdateDate.Width = 1.302756F;
            // 
            // lineHeaderVerUpdateDate
            // 
            this.lineHeaderVerUpdateDate.Height = 0.353937F;
            this.lineHeaderVerUpdateDate.Left = 7.137352F;
            this.lineHeaderVerUpdateDate.LineWeight = 1F;
            this.lineHeaderVerUpdateDate.Name = "lineHeaderVerUpdateDate";
            this.lineHeaderVerUpdateDate.Top = 0.5417323F;
            this.lineHeaderVerUpdateDate.Width = 5.00679E-05F;
            this.lineHeaderVerUpdateDate.X1 = 7.137402F;
            this.lineHeaderVerUpdateDate.X2 = 7.137352F;
            this.lineHeaderVerUpdateDate.Y1 = 0.8956693F;
            this.lineHeaderVerUpdateDate.Y2 = 0.5417323F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.5377953F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 10.62992F;
            this.lineHeaderHorUpper.X2 = 0F;
            this.lineHeaderHorUpper.Y1 = 0.5377953F;
            this.lineHeaderHorUpper.Y2 = 0.5377953F;
            // 
            // lblUpdateName
            // 
            this.lblUpdateName.Height = 0.3267716F;
            this.lblUpdateName.HyperLink = null;
            this.lblUpdateName.Left = 8.440158F;
            this.lblUpdateName.Name = "lblUpdateName";
            this.lblUpdateName.Style = "background-color: WhiteSmoke; font-size: 9pt; text-align: center; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.lblUpdateName.Text = "最新更新者";
            this.lblUpdateName.Top = 0.5472441F;
            this.lblUpdateName.Width = 2.18622F;
            // 
            // lineHeaderVerUpdateName
            // 
            this.lineHeaderVerUpdateName.Height = 0.353937F;
            this.lineHeaderVerUpdateName.Left = 8.440107F;
            this.lineHeaderVerUpdateName.LineWeight = 1F;
            this.lineHeaderVerUpdateName.Name = "lineHeaderVerUpdateName";
            this.lineHeaderVerUpdateName.Top = 0.5417323F;
            this.lineHeaderVerUpdateName.Width = 5.054474E-05F;
            this.lineHeaderVerUpdateName.X1 = 8.440158F;
            this.lineHeaderVerUpdateName.X2 = 8.440107F;
            this.lineHeaderVerUpdateName.Y1 = 0.8956693F;
            this.lineHeaderVerUpdateName.Y2 = 0.5417323F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lineDetailVerSeparator,
            this.txtSectionCode,
            this.lineDetailVerSectionName,
            this.txtSectionName,
            this.lineDetailVerBranchCode,
            this.txtPayerCodeLeft,
            this.lineDetailVerAccountNumber,
            this.txtPayerCodeRight,
            this.lineDetailVerUpdateDate,
            this.txtUpdateDate,
            this.txtLoginUserName,
            this.lineDetailVerUpdateName});
            this.detail.Height = 0.3228346F;
            this.detail.Name = "detail";
            // 
            // lineDetailVerSeparator
            // 
            this.lineDetailVerSeparator.Height = 0F;
            this.lineDetailVerSeparator.Left = 0F;
            this.lineDetailVerSeparator.LineWeight = 1F;
            this.lineDetailVerSeparator.Name = "lineDetailVerSeparator";
            this.lineDetailVerSeparator.Top = 0.3228346F;
            this.lineDetailVerSeparator.Width = 10.62992F;
            this.lineDetailVerSeparator.X1 = 0F;
            this.lineDetailVerSeparator.X2 = 10.62992F;
            this.lineDetailVerSeparator.Y1 = 0.3228346F;
            this.lineDetailVerSeparator.Y2 = 0.3228346F;
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.Height = 0.3149607F;
            this.txtSectionCode.Left = 0.001181096F;
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtSectionCode.Text = null;
            this.txtSectionCode.Top = 0F;
            this.txtSectionCode.Width = 1.416142F;
            // 
            // lineDetailVerSectionName
            // 
            this.lineDetailVerSectionName.Height = 0.3231299F;
            this.lineDetailVerSectionName.Left = 1.44252F;
            this.lineDetailVerSectionName.LineWeight = 1F;
            this.lineDetailVerSectionName.Name = "lineDetailVerSectionName";
            this.lineDetailVerSectionName.Top = 0F;
            this.lineDetailVerSectionName.Width = 0F;
            this.lineDetailVerSectionName.X1 = 1.44252F;
            this.lineDetailVerSectionName.X2 = 1.44252F;
            this.lineDetailVerSectionName.Y1 = 0F;
            this.lineDetailVerSectionName.Y2 = 0.3231299F;
            // 
            // txtSectionName
            // 
            this.txtSectionName.Height = 0.3149607F;
            this.txtSectionName.Left = 1.476378F;
            this.txtSectionName.Name = "txtSectionName";
            this.txtSectionName.Style = "font-size: 9pt; vertical-align: middle; ddo-char-set: 1";
            this.txtSectionName.Text = null;
            this.txtSectionName.Top = 0F;
            this.txtSectionName.Width = 2.952756F;
            // 
            // lineDetailVerBranchCode
            // 
            this.lineDetailVerBranchCode.Height = 0.3232283F;
            this.lineDetailVerBranchCode.Left = 4.475985F;
            this.lineDetailVerBranchCode.LineWeight = 1F;
            this.lineDetailVerBranchCode.Name = "lineDetailVerBranchCode";
            this.lineDetailVerBranchCode.Top = 0F;
            this.lineDetailVerBranchCode.Width = 0F;
            this.lineDetailVerBranchCode.X1 = 4.475985F;
            this.lineDetailVerBranchCode.X2 = 4.475985F;
            this.lineDetailVerBranchCode.Y1 = 0F;
            this.lineDetailVerBranchCode.Y2 = 0.3232283F;
            // 
            // txtPayerCodeLeft
            // 
            this.txtPayerCodeLeft.Height = 0.3149606F;
            this.txtPayerCodeLeft.Left = 4.488189F;
            this.txtPayerCodeLeft.Name = "txtPayerCodeLeft";
            this.txtPayerCodeLeft.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtPayerCodeLeft.Text = null;
            this.txtPayerCodeLeft.Top = 0F;
            this.txtPayerCodeLeft.Width = 1.062992F;
            // 
            // lineDetailVerAccountNumber
            // 
            this.lineDetailVerAccountNumber.Height = 0.3232284F;
            this.lineDetailVerAccountNumber.Left = 5.577953F;
            this.lineDetailVerAccountNumber.LineWeight = 1F;
            this.lineDetailVerAccountNumber.Name = "lineDetailVerAccountNumber";
            this.lineDetailVerAccountNumber.Top = 0F;
            this.lineDetailVerAccountNumber.Width = 0F;
            this.lineDetailVerAccountNumber.X1 = 5.577953F;
            this.lineDetailVerAccountNumber.X2 = 5.577953F;
            this.lineDetailVerAccountNumber.Y1 = 0F;
            this.lineDetailVerAccountNumber.Y2 = 0.3232284F;
            // 
            // txtPayerCodeRight
            // 
            this.txtPayerCodeRight.Height = 0.3149606F;
            this.txtPayerCodeRight.Left = 5.590551F;
            this.txtPayerCodeRight.Name = "txtPayerCodeRight";
            this.txtPayerCodeRight.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtPayerCodeRight.Text = null;
            this.txtPayerCodeRight.Top = 0F;
            this.txtPayerCodeRight.Width = 1.515748F;
            // 
            // lineDetailVerUpdateDate
            // 
            this.lineDetailVerUpdateDate.Height = 0.3232284F;
            this.lineDetailVerUpdateDate.Left = 7.137402F;
            this.lineDetailVerUpdateDate.LineWeight = 1F;
            this.lineDetailVerUpdateDate.Name = "lineDetailVerUpdateDate";
            this.lineDetailVerUpdateDate.Top = 0F;
            this.lineDetailVerUpdateDate.Width = 0F;
            this.lineDetailVerUpdateDate.X1 = 7.137402F;
            this.lineDetailVerUpdateDate.X2 = 7.137402F;
            this.lineDetailVerUpdateDate.Y1 = 0F;
            this.lineDetailVerUpdateDate.Y2 = 0.3232284F;
            // 
            // txtUpdateDate
            // 
            this.txtUpdateDate.Height = 0.3149606F;
            this.txtUpdateDate.Left = 7.165355F;
            this.txtUpdateDate.Name = "txtUpdateDate";
            this.txtUpdateDate.Style = "font-size: 9pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtUpdateDate.Text = null;
            this.txtUpdateDate.Top = 0F;
            this.txtUpdateDate.Width = 1.240157F;
            // 
            // txtLoginUserName
            // 
            this.txtLoginUserName.Height = 0.3149606F;
            this.txtLoginUserName.Left = 8.464567F;
            this.txtLoginUserName.Name = "txtLoginUserName";
            this.txtLoginUserName.OutputFormat = resources.GetString("txtLoginUserName.OutputFormat");
            this.txtLoginUserName.Style = "font-size: 9pt; text-justify: auto; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtLoginUserName.Text = null;
            this.txtLoginUserName.Top = 0F;
            this.txtLoginUserName.Width = 2.125984F;
            // 
            // lineDetailVerUpdateName
            // 
            this.lineDetailVerUpdateName.Height = 0.3232284F;
            this.lineDetailVerUpdateName.Left = 8.440158F;
            this.lineDetailVerUpdateName.LineWeight = 1F;
            this.lineDetailVerUpdateName.Name = "lineDetailVerUpdateName";
            this.lineDetailVerUpdateName.Top = 0F;
            this.lineDetailVerUpdateName.Width = 0F;
            this.lineDetailVerUpdateName.X1 = 8.440158F;
            this.lineDetailVerUpdateName.X2 = 8.440158F;
            this.lineDetailVerUpdateName.Y1 = 0F;
            this.lineDetailVerUpdateName.Y2 = 0.3232284F;
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
            // SectionMasterReport
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
            this.Sections.Add(this.detail);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUpdateDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUpdateName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerCodeLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerCodeRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpdateDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbltitle;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox lblSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblUpdateDate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerUpdateDate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerUpdateName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblUpdateName;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBranchCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerCodeLeft;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAccountNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerCodeRight;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerUpdateDate;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUpdateDate;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLoginUserName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSeparator;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerUpdateName;
    }
}
