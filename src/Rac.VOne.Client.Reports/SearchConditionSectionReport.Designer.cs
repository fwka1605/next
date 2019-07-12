namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// AllMatchingSectionReport の概要の説明です。
    /// </summary>
    partial class SearchConditionSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SearchConditionSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line15 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lnnyukinbi = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblSearchCondition = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtSearchValue = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSearchName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSearchValue = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lnverStart = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSearchName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSearchCondition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.line15,
            this.lblCompanyCode,
            this.line5,
            this.lnnyukinbi,
            this.line2,
            this.lblSearchCondition});
            this.pageHeader.Height = 0.8680775F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.8118111F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
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
            this.lblDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lblDate.Text = "出力日付　：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.6983147F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 9.522442F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle";
            this.ridate.Top = 0F;
            this.ridate.Width = 1.015F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "lbltitle";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // line15
            // 
            this.line15.Height = 0.216536F;
            this.line15.Left = 10.9004F;
            this.line15.LineWeight = 1F;
            this.line15.Name = "line15";
            this.line15.Top = 0.9153541F;
            this.line15.Width = 0F;
            this.line15.X1 = 10.9004F;
            this.line15.X2 = 10.9004F;
            this.line15.Y1 = 1.13189F;
            this.line15.Y2 = 0.9153541F;
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
            this.lblCompanyCode.Width = 0.7874017F;
            // 
            // line5
            // 
            this.line5.Height = 0F;
            this.line5.Left = 0F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 1.156299F;
            this.line5.Width = 10.554F;
            this.line5.X1 = 0F;
            this.line5.X2 = 10.554F;
            this.line5.Y1 = 1.156299F;
            this.line5.Y2 = 1.156299F;
            // 
            // lnnyukinbi
            // 
            this.lnnyukinbi.Height = 0.0004173517F;
            this.lnnyukinbi.Left = 0.02400064F;
            this.lnnyukinbi.LineWeight = 1F;
            this.lnnyukinbi.Name = "lnnyukinbi";
            this.lnnyukinbi.Top = 0.961F;
            this.lnnyukinbi.Width = 10.355F;
            this.lnnyukinbi.X1 = 10.379F;
            this.lnnyukinbi.X2 = 0.02400064F;
            this.lnnyukinbi.Y1 = 0.961F;
            this.lnnyukinbi.Y2 = 0.9614174F;
            // 
            // line2
            // 
            this.line2.Height = 0.0002832413F;
            this.line2.Left = 0.02400017F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 0.8677167F;
            this.line2.Width = 10.355F;
            this.line2.X1 = 10.379F;
            this.line2.X2 = 0.02400017F;
            this.line2.Y1 = 0.868F;
            this.line2.Y2 = 0.8677167F;
            // 
            // lblSearchCondition
            // 
            this.lblSearchCondition.Height = 0.2F;
            this.lblSearchCondition.HyperLink = null;
            this.lblSearchCondition.Left = 0F;
            this.lblSearchCondition.Name = "lblSearchCondition";
            this.lblSearchCondition.Style = "font-size: 12pt; vertical-align: middle; ddo-char-set: 128";
            this.lblSearchCondition.Text = "検索条件";
            this.lblSearchCondition.Top = 0.5905512F;
            this.lblSearchCondition.Width = 1F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtSearchValue,
            this.txtSearchName,
            this.lineDetailHorLower,
            this.lineDetailVerSearchValue,
            this.lnverStart,
            this.lineDetailVerSearchName});
            this.detail.Height = 0.2003937F;
            this.detail.Name = "detail";
            this.detail.Format += new System.EventHandler(this.detail_Format);
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtSearchValue
            // 
            this.txtSearchValue.Height = 0.2F;
            this.txtSearchValue.Left = 2.37874F;
            this.txtSearchValue.Name = "txtSearchValue";
            this.txtSearchValue.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtSearchValue.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtSearchValue.Text = "SearchValue";
            this.txtSearchValue.Top = 0F;
            this.txtSearchValue.Width = 8F;
            // 
            // txtSearchName
            // 
            this.txtSearchName.DataField = "SearchName";
            this.txtSearchName.Height = 0.2F;
            this.txtSearchName.Left = 0.024F;
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtSearchName.Style = "background-color: WhiteSmoke; vertical-align: middle";
            this.txtSearchName.Text = "searchName";
            this.txtSearchName.Top = 0F;
            this.txtSearchName.Width = 2.355F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0.024F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.2F;
            this.lineDetailHorLower.Width = 10.355F;
            this.lineDetailHorLower.X1 = 10.379F;
            this.lineDetailHorLower.X2 = 0.024F;
            this.lineDetailHorLower.Y1 = 0.2F;
            this.lineDetailHorLower.Y2 = 0.2F;
            // 
            // lineDetailVerSearchValue
            // 
            this.lineDetailVerSearchValue.AnchorBottom = true;
            this.lineDetailVerSearchValue.Height = 0.2F;
            this.lineDetailVerSearchValue.Left = 10.379F;
            this.lineDetailVerSearchValue.LineWeight = 1F;
            this.lineDetailVerSearchValue.Name = "lineDetailVerSearchValue";
            this.lineDetailVerSearchValue.Top = 0F;
            this.lineDetailVerSearchValue.Width = 0F;
            this.lineDetailVerSearchValue.X1 = 10.379F;
            this.lineDetailVerSearchValue.X2 = 10.379F;
            this.lineDetailVerSearchValue.Y1 = 0F;
            this.lineDetailVerSearchValue.Y2 = 0.2F;
            // 
            // lnverStart
            // 
            this.lnverStart.AnchorBottom = true;
            this.lnverStart.Height = 0.2003937F;
            this.lnverStart.Left = 0.024F;
            this.lnverStart.LineWeight = 1F;
            this.lnverStart.Name = "lnverStart";
            this.lnverStart.Top = 0F;
            this.lnverStart.Width = 0F;
            this.lnverStart.X1 = 0.024F;
            this.lnverStart.X2 = 0.024F;
            this.lnverStart.Y1 = 0F;
            this.lnverStart.Y2 = 0.2003937F;
            // 
            // lineDetailVerSearchName
            // 
            this.lineDetailVerSearchName.AnchorBottom = true;
            this.lineDetailVerSearchName.Height = 0.2F;
            this.lineDetailVerSearchName.Left = 2.379F;
            this.lineDetailVerSearchName.LineWeight = 1F;
            this.lineDetailVerSearchName.Name = "lineDetailVerSearchName";
            this.lineDetailVerSearchName.Top = 0F;
            this.lineDetailVerSearchName.Width = 0.0003931522F;
            this.lineDetailVerSearchName.X1 = 2.379F;
            this.lineDetailVerSearchName.X2 = 2.379393F;
            this.lineDetailVerSearchName.Y1 = 0F;
            this.lineDetailVerSearchName.Y2 = 0.2F;
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
            this.lblPageNumber.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle";
            this.lblPageNumber.Text = "PageNumber/PageCount";
            this.lblPageNumber.Top = 0.05748032F;
            this.lblPageNumber.Width = 10.62992F;
            // 
            // SearchConditionSectionReport
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
            "t-family: \"ＭＳ 明朝\"; font-size: 9pt; ddo-char-set: 186", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSearchCondition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line line15;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSearchValue;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSearchName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lnnyukinbi;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSearchValue;
        private GrapeCity.ActiveReports.SectionReportModel.Line lnverStart;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSearchName;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSearchCondition;
    }
}
