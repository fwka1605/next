namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerLedgerSearchConditionSectionReport の概要の説明です。
    /// </summary>
    partial class CustomerLedgerSearchConditionSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CustomerLedgerSearchConditionSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompany = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSearchCondition = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtSearchValue = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSearchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSearchName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSearchValue = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSearchCondition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompany,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblSearchCondition,
            this.lineHeaderHorUpper});
            this.pageHeader.Height = 0.8730861F;
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
            this.lblCompany.Width = 0.8661417F;
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.8661417F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.lblCompanyCodeName.Text = "label2";
            this.lblCompanyCodeName.Top = 0F;
            this.lblCompanyCodeName.Width = 3.532677F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 5.881103F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.lblDate.Text = "出力日付　：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.615F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 6.496063F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; text-align: left; vertical-align" +
    ": middle; ddo-char-set: 1";
            this.ridate.Top = 0F;
            this.ridate.Width = 0.749F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.231F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-family: ＭＳ 明朝; font-size: 14pt; text-align: center; text-decoration: underli" +
    "ne; ddo-char-set: 1";
            this.lblTitle.Text = "lbltitle";
            this.lblTitle.Top = 0.2704724F;
            this.lblTitle.Width = 7.244094F;
            // 
            // lblSearchCondition
            // 
            this.lblSearchCondition.Height = 0.2F;
            this.lblSearchCondition.HyperLink = null;
            this.lblSearchCondition.Left = 0F;
            this.lblSearchCondition.Name = "lblSearchCondition";
            this.lblSearchCondition.Style = "font-family: ＭＳ 明朝; font-size: 12pt; vertical-align: middle; ddo-char-set: 1";
            this.lblSearchCondition.Text = "検索条件";
            this.lblSearchCondition.Top = 0.5905512F;
            this.lblSearchCondition.Width = 1F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0.0001417398F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.8660001F;
            this.lineHeaderHorUpper.Width = 6.987F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 6.987F;
            this.lineHeaderHorUpper.Y1 = 0.8661418F;
            this.lineHeaderHorUpper.Y2 = 0.8660001F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtSearchValue,
            this.txtSearchName,
            this.lineDetailHorLower,
            this.lineDetailVerSearchName,
            this.lineDetailVerSearchValue,
            this.line1});
            this.detail.Height = 0.2F;
            this.detail.Name = "detail";
            // 
            // txtSearchValue
            // 
            this.txtSearchValue.Height = 0.2F;
            this.txtSearchValue.Left = 1.997F;
            this.txtSearchValue.Name = "txtSearchValue";
            this.txtSearchValue.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtSearchValue.Style = "font-family: ＭＳ 明朝; font-size: 9pt; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtSearchValue.Text = "searchValue";
            this.txtSearchValue.Top = 0F;
            this.txtSearchValue.Width = 4.990001F;
            // 
            // txtSearchName
            // 
            this.txtSearchName.DataField = "SearchName";
            this.txtSearchName.Height = 0.2F;
            this.txtSearchName.Left = 0F;
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; vertical-align:" +
    " middle; ddo-char-set: 1";
            this.txtSearchName.Text = "searchName";
            this.txtSearchName.Top = 0F;
            this.txtSearchName.Width = 2F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.2F;
            this.lineDetailHorLower.Width = 6.987F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 6.987F;
            this.lineDetailHorLower.Y1 = 0.2F;
            this.lineDetailHorLower.Y2 = 0.2F;
            // 
            // lineDetailVerSearchName
            // 
            this.lineDetailVerSearchName.AnchorBottom = true;
            this.lineDetailVerSearchName.Height = 0.1999999F;
            this.lineDetailVerSearchName.Left = 2F;
            this.lineDetailVerSearchName.LineWeight = 1F;
            this.lineDetailVerSearchName.Name = "lineDetailVerSearchName";
            this.lineDetailVerSearchName.Top = 0F;
            this.lineDetailVerSearchName.Width = 0.0003929138F;
            this.lineDetailVerSearchName.X1 = 2F;
            this.lineDetailVerSearchName.X2 = 2.000393F;
            this.lineDetailVerSearchName.Y1 = 0F;
            this.lineDetailVerSearchName.Y2 = 0.1999999F;
            // 
            // lineDetailVerSearchValue
            // 
            this.lineDetailVerSearchValue.AnchorBottom = true;
            this.lineDetailVerSearchValue.Height = 0.2F;
            this.lineDetailVerSearchValue.Left = 6.987F;
            this.lineDetailVerSearchValue.LineWeight = 1F;
            this.lineDetailVerSearchValue.Name = "lineDetailVerSearchValue";
            this.lineDetailVerSearchValue.Top = 0F;
            this.lineDetailVerSearchValue.Width = 0F;
            this.lineDetailVerSearchValue.X1 = 6.987F;
            this.lineDetailVerSearchValue.X2 = 6.987F;
            this.lineDetailVerSearchValue.Y1 = 0F;
            this.lineDetailVerSearchValue.Y2 = 0.2F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblPageNumber});
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
            this.lblPageNumber.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; text-align: center; vertical-ali" +
    "gn: middle; ddo-char-set: 1";
            this.lblPageNumber.Text = "PageNumber/PageCount";
            this.lblPageNumber.Top = 0.05748032F;
            this.lblPageNumber.Width = 7.244094F;
            // 
            // line1
            // 
            this.line1.AnchorBottom = true;
            this.line1.Height = 0.2F;
            this.line1.Left = 0F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0F;
            this.line1.Width = 0F;
            this.line1.X1 = 0F;
            this.line1.X2 = 0F;
            this.line1.Y1 = 0F;
            this.line1.Y2 = 0.2F;
            // 
            // CustomerLedgerSearchConditionSectionReport
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
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " +
            "color: Black; font-family: \"MS UI Gothic\"; ddo-char-set: 128", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSearchCondition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSearchCondition;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompany;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSearchName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSearchValue;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSearchName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSearchValue;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
    }
}
