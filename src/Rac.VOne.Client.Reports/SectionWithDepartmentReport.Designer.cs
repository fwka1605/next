namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// SectionWithDepartmentReport の概要の説明です。
    /// </summary>
    partial class SectionWithDepartmentReport
    {
        private GrapeCity.ActiveReports.SectionReportModel.PageHeader pageHeader;
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SectionWithDepartmentReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompany = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblSectionName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSectionCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSectionName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSectionName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSectionName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSectionCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompany,
            this.lblCompanyCode,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblSectionCode,
            this.lblSectionName,
            this.lblDepartmentCode,
            this.lblDepartmentName,
            this.lineHeaderHorLower,
            this.lineHeaderHorUpper,
            this.lineHeaderVerSectionCode,
            this.lineHeaderVerSectionName,
            this.lineHeaderVerDepartmentCode});
            this.pageHeader.Height = 0.8437336F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompany
            // 
            this.lblCompany.Height = 0.2F;
            this.lblCompany.HyperLink = null;
            this.lblCompany.Left = 0.02440945F;
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompany.Text = "会社コード　：";
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
            this.lblCompanyCode.Text = "label2";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 3.657087F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 5.856299F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-size: 7pt; text-align: right; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblDate.Text = "出力日付　：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.6153543F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 6.47126F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle; ddo-char-s" +
    "et: 1";
            this.ridate.Top = 0.0001574904F;
            this.ridate.Width = 0.7480315F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline";
            this.lblTitle.Text = "入金・請求部門対応マスター一覧";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 7.244094F;
            // 
            // lblSectionCode
            // 
            this.lblSectionCode.Height = 0.3188976F;
            this.lblSectionCode.Left = 0.001181103F;
            this.lblSectionCode.Name = "lblSectionCode";
            this.lblSectionCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblSectionCode.Text = "入金部門コード";
            this.lblSectionCode.Top = 0.5417323F;
            this.lblSectionCode.Width = 1.181102F;
            // 
            // lblSectionName
            // 
            this.lblSectionName.Height = 0.3228346F;
            this.lblSectionName.HyperLink = null;
            this.lblSectionName.Left = 1.182284F;
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblSectionName.Text = "入金部門名";
            this.lblSectionName.Top = 0.5377953F;
            this.lblSectionName.Width = 2.408268F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.3188976F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 3.538583F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentCode.Text = "請求部門コード";
            this.lblDepartmentCode.Top = 0.5456693F;
            this.lblDepartmentCode.Width = 1.281102F;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Height = 0.3228346F;
            this.lblDepartmentName.HyperLink = null;
            this.lblDepartmentName.Left = 4.822835F;
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentName.Text = "請求部門名";
            this.lblDepartmentName.Top = 0.5377953F;
            this.lblDepartmentName.Width = 2.43189F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 0.86063F;
            this.lineHeaderHorLower.Width = 7.409056F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 7.409056F;
            this.lineHeaderHorLower.Y1 = 0.86063F;
            this.lineHeaderHorLower.Y2 = 0.86063F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0.001181102F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.5417323F;
            this.lineHeaderHorUpper.Width = 7.407875F;
            this.lineHeaderHorUpper.X1 = 0.001181102F;
            this.lineHeaderHorUpper.X2 = 7.409056F;
            this.lineHeaderHorUpper.Y1 = 0.5417323F;
            this.lineHeaderHorUpper.Y2 = 0.5417323F;
            // 
            // lineHeaderVerSectionCode
            // 
            this.lineHeaderVerSectionCode.Height = 0.3188977F;
            this.lineHeaderVerSectionCode.Left = 1.182284F;
            this.lineHeaderVerSectionCode.LineWeight = 1F;
            this.lineHeaderVerSectionCode.Name = "lineHeaderVerSectionCode";
            this.lineHeaderVerSectionCode.Top = 0.5456693F;
            this.lineHeaderVerSectionCode.Width = 0F;
            this.lineHeaderVerSectionCode.X1 = 1.182284F;
            this.lineHeaderVerSectionCode.X2 = 1.182284F;
            this.lineHeaderVerSectionCode.Y1 = 0.5456693F;
            this.lineHeaderVerSectionCode.Y2 = 0.864567F;
            // 
            // lineHeaderVerSectionName
            // 
            this.lineHeaderVerSectionName.Height = 0.3188974F;
            this.lineHeaderVerSectionName.Left = 3.538583F;
            this.lineHeaderVerSectionName.LineWeight = 1F;
            this.lineHeaderVerSectionName.Name = "lineHeaderVerSectionName";
            this.lineHeaderVerSectionName.Top = 0.5456693F;
            this.lineHeaderVerSectionName.Width = 0F;
            this.lineHeaderVerSectionName.X1 = 3.538583F;
            this.lineHeaderVerSectionName.X2 = 3.538583F;
            this.lineHeaderVerSectionName.Y1 = 0.5456693F;
            this.lineHeaderVerSectionName.Y2 = 0.8645667F;
            // 
            // lineHeaderVerDepartmentCode
            // 
            this.lineHeaderVerDepartmentCode.Height = 0.3188973F;
            this.lineHeaderVerDepartmentCode.Left = 4.819685F;
            this.lineHeaderVerDepartmentCode.LineWeight = 1F;
            this.lineHeaderVerDepartmentCode.Name = "lineHeaderVerDepartmentCode";
            this.lineHeaderVerDepartmentCode.Top = 0.5417323F;
            this.lineHeaderVerDepartmentCode.Width = 0F;
            this.lineHeaderVerDepartmentCode.X1 = 4.819685F;
            this.lineHeaderVerDepartmentCode.X2 = 4.819685F;
            this.lineHeaderVerDepartmentCode.Y1 = 0.5417323F;
            this.lineHeaderVerDepartmentCode.Y2 = 0.8606296F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtSectionCode,
            this.txtDepartmentName,
            this.txtSectionName,
            this.txtDepartmentCode,
            this.lineDetailVerDepartmentCode,
            this.lineDetailVerSectionName,
            this.lineDetailVerSectionCode});
            this.detail.Height = 0.3082677F;
            this.detail.Name = "detail";
            this.detail.Format += new System.EventHandler(this.detail_Format);
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.Height = 0.2913386F;
            this.txtSectionCode.Left = 0.07559056F;
            this.txtSectionCode.MultiLine = false;
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Style = "text-align: center; text-justify: auto; vertical-align: middle";
            this.txtSectionCode.Text = null;
            this.txtSectionCode.Top = 0F;
            this.txtSectionCode.Width = 1.05315F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.Height = 0.2913386F;
            this.txtDepartmentName.Left = 4.872047F;
            this.txtDepartmentName.MultiLine = false;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Style = "text-align: left; vertical-align: middle";
            this.txtDepartmentName.Text = null;
            this.txtDepartmentName.Top = 0F;
            this.txtDepartmentName.Width = 2.308662F;
            // 
            // txtSectionName
            // 
            this.txtSectionName.Height = 0.2913386F;
            this.txtSectionName.Left = 1.233465F;
            this.txtSectionName.MultiLine = false;
            this.txtSectionName.Name = "txtSectionName";
            this.txtSectionName.Style = "text-align: left; vertical-align: middle";
            this.txtSectionName.Text = null;
            this.txtSectionName.Top = 0F;
            this.txtSectionName.Width = 2.305118F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.Height = 0.2913386F;
            this.txtDepartmentCode.Left = 3.590551F;
            this.txtDepartmentCode.MultiLine = false;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Style = "text-align: center; vertical-align: middle";
            this.txtDepartmentCode.Text = null;
            this.txtDepartmentCode.Top = 0F;
            this.txtDepartmentCode.Width = 1.187402F;
            // 
            // lineDetailVerDepartmentCode
            // 
            this.lineDetailVerDepartmentCode.Height = 0.3122047F;
            this.lineDetailVerDepartmentCode.Left = 4.815748F;
            this.lineDetailVerDepartmentCode.LineWeight = 1F;
            this.lineDetailVerDepartmentCode.Name = "lineDetailVerDepartmentCode";
            this.lineDetailVerDepartmentCode.Top = 0F;
            this.lineDetailVerDepartmentCode.Width = 0F;
            this.lineDetailVerDepartmentCode.X1 = 4.815748F;
            this.lineDetailVerDepartmentCode.X2 = 4.815748F;
            this.lineDetailVerDepartmentCode.Y1 = 0F;
            this.lineDetailVerDepartmentCode.Y2 = 0.3122047F;
            // 
            // lineDetailVerSectionName
            // 
            this.lineDetailVerSectionName.Height = 0.3122047F;
            this.lineDetailVerSectionName.Left = 3.538583F;
            this.lineDetailVerSectionName.LineWeight = 1F;
            this.lineDetailVerSectionName.Name = "lineDetailVerSectionName";
            this.lineDetailVerSectionName.Top = 0F;
            this.lineDetailVerSectionName.Width = 0F;
            this.lineDetailVerSectionName.X1 = 3.538583F;
            this.lineDetailVerSectionName.X2 = 3.538583F;
            this.lineDetailVerSectionName.Y1 = 0F;
            this.lineDetailVerSectionName.Y2 = 0.3122047F;
            // 
            // lineDetailVerSectionCode
            // 
            this.lineDetailVerSectionCode.Height = 0.3122047F;
            this.lineDetailVerSectionCode.Left = 1.182284F;
            this.lineDetailVerSectionCode.LineWeight = 1F;
            this.lineDetailVerSectionCode.Name = "lineDetailVerSectionCode";
            this.lineDetailVerSectionCode.Top = 0F;
            this.lineDetailVerSectionCode.Width = 0F;
            this.lineDetailVerSectionCode.X1 = 1.182284F;
            this.lineDetailVerSectionCode.X2 = 1.182284F;
            this.lineDetailVerSectionCode.Y1 = 0F;
            this.lineDetailVerSectionCode.Y2 = 0.3122047F;
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
            this.reportInfo1.Width = 7.244094F;
            // 
            // SectionWithDepartmentReport
            // 
            this.MasterReport = false;
            this.PageSettings.DefaultPaperSize = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait;
            this.PageSettings.PaperHeight = 11.69291F;
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.PageSettings.PaperWidth = 8.267716F;
            this.PrintWidth = 7.244094F;
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
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompany;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox lblSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Detail detail;
    }
}
