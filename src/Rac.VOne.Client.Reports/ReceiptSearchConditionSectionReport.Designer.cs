namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ReceiptSearchConditionSectionReport の概要の説明です。
    /// </summary>
    partial class ReceiptSearchConditionSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReceiptSearchConditionSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblcompanycode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lbltitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label11 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtSearchValue1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSearchName1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtSearchValue2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSearchName2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchValue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchName1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchName2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblcompanycode,
            this.lbldate,
            this.ridate,
            this.lbltitle,
            this.label11,
            this.line8,
            this.line3,
            this.label1});
            this.pageHeader.Height = 0.9602252F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblcompanycode
            // 
            this.lblcompanycode.Height = 0.2F;
            this.lblcompanycode.HyperLink = null;
            this.lblcompanycode.Left = 0.811811F;
            this.lblcompanycode.Name = "lblcompanycode";
            this.lblcompanycode.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblcompanycode.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
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
            this.lbldate.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
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
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle";
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
            this.lbltitle.Width = 10.62992F;
            // 
            // label11
            // 
            this.label11.Height = 0.2F;
            this.label11.HyperLink = null;
            this.label11.Left = 0.02440945F;
            this.label11.Name = "label11";
            this.label11.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.label11.Text = "会社コード　：";
            this.label11.Top = 0F;
            this.label11.Width = 0.7874016F;
            // 
            // line8
            // 
            this.line8.Height = 0F;
            this.line8.Left = 0.02600096F;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.Top = 0.9594489F;
            this.line8.Width = 5.105495F;
            this.line8.X1 = 5.131496F;
            this.line8.X2 = 0.02600096F;
            this.line8.Y1 = 0.9594489F;
            this.line8.Y2 = 0.9594489F;
            // 
            // line3
            // 
            this.line3.Height = 0F;
            this.line3.Left = 5.31143F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0.9594489F;
            this.line3.Width = 5.1055F;
            this.line3.X1 = 10.41693F;
            this.line3.X2 = 5.31143F;
            this.line3.Y1 = 0.9594489F;
            this.line3.Y2 = 0.9594489F;
            // 
            // label1
            // 
            this.label1.Height = 0.2F;
            this.label1.HyperLink = null;
            this.label1.Left = 0.02440945F;
            this.label1.Name = "label1";
            this.label1.Style = "font-size: 12pt; vertical-align: middle; ddo-char-set: 128";
            this.label1.Text = "検索条件";
            this.label1.Top = 0.6397638F;
            this.label1.Width = 1F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtSearchValue1,
            this.txtSearchName1,
            this.line7,
            this.line12,
            this.line1,
            this.line2,
            this.txtSearchValue2,
            this.txtSearchName2,
            this.line4,
            this.line5,
            this.line6,
            this.line9});
            this.detail.Height = 0.1979171F;
            this.detail.Name = "detail";
            this.detail.Format += new System.EventHandler(this.detail_Format);
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtSearchValue1
            // 
            this.txtSearchValue1.Height = 0.2F;
            this.txtSearchValue1.Left = 1.983F;
            this.txtSearchValue1.Name = "txtSearchValue1";
            this.txtSearchValue1.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtSearchValue1.Style = "vertical-align: middle";
            this.txtSearchValue1.Text = "textBox1";
            this.txtSearchValue1.Top = 0F;
            this.txtSearchValue1.Width = 3.148426F;
            // 
            // txtSearchName1
            // 
            this.txtSearchName1.Height = 0.2F;
            this.txtSearchName1.Left = 0.025F;
            this.txtSearchName1.Name = "txtSearchName1";
            this.txtSearchName1.Style = "background-color: Gainsboro; vertical-align: middle";
            this.txtSearchName1.Text = null;
            this.txtSearchName1.Top = 0F;
            this.txtSearchName1.Width = 1.958268F;
            // 
            // line7
            // 
            this.line7.Height = 0F;
            this.line7.Left = 0.026F;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.Top = 0.2F;
            this.line7.Width = 5.105693F;
            this.line7.X1 = 0.026F;
            this.line7.X2 = 5.131693F;
            this.line7.Y1 = 0.2F;
            this.line7.Y2 = 0.2F;
            // 
            // line12
            // 
            this.line12.AnchorBottom = true;
            this.line12.Height = 0.2000001F;
            this.line12.Left = 5.132F;
            this.line12.LineWeight = 1F;
            this.line12.Name = "line12";
            this.line12.Top = 0F;
            this.line12.Width = 0F;
            this.line12.X1 = 5.132F;
            this.line12.X2 = 5.132F;
            this.line12.Y1 = 0F;
            this.line12.Y2 = 0.2000001F;
            // 
            // line1
            // 
            this.line1.AnchorBottom = true;
            this.line1.Height = 0.2000001F;
            this.line1.Left = 0.0250001F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0F;
            this.line1.Width = 0F;
            this.line1.X1 = 0.0250001F;
            this.line1.X2 = 0.0250001F;
            this.line1.Y1 = 0F;
            this.line1.Y2 = 0.2000001F;
            // 
            // line2
            // 
            this.line2.AnchorBottom = true;
            this.line2.Height = 0.2000001F;
            this.line2.Left = 1.983F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 0F;
            this.line2.Width = 0F;
            this.line2.X1 = 1.983F;
            this.line2.X2 = 1.983F;
            this.line2.Y1 = 0F;
            this.line2.Y2 = 0.2000001F;
            // 
            // txtSearchValue2
            // 
            this.txtSearchValue2.Height = 0.2F;
            this.txtSearchValue2.Left = 7.268F;
            this.txtSearchValue2.Name = "txtSearchValue2";
            this.txtSearchValue2.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtSearchValue2.Style = "vertical-align: middle";
            this.txtSearchValue2.Text = "textBox1";
            this.txtSearchValue2.Top = 0F;
            this.txtSearchValue2.Width = 3.148426F;
            // 
            // txtSearchName2
            // 
            this.txtSearchName2.Height = 0.2F;
            this.txtSearchName2.Left = 5.31F;
            this.txtSearchName2.Name = "txtSearchName2";
            this.txtSearchName2.Style = "background-color: Gainsboro; vertical-align: middle";
            this.txtSearchName2.Text = null;
            this.txtSearchName2.Top = 0F;
            this.txtSearchName2.Width = 1.958268F;
            // 
            // line4
            // 
            this.line4.Height = 0F;
            this.line4.Left = 5.311F;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.Top = 0.2F;
            this.line4.Width = 5.10569F;
            this.line4.X1 = 5.311F;
            this.line4.X2 = 10.41669F;
            this.line4.Y1 = 0.2F;
            this.line4.Y2 = 0.2F;
            // 
            // line5
            // 
            this.line5.AnchorBottom = true;
            this.line5.Height = 0.2000001F;
            this.line5.Left = 10.417F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0F;
            this.line5.Width = 0F;
            this.line5.X1 = 10.417F;
            this.line5.X2 = 10.417F;
            this.line5.Y1 = 0F;
            this.line5.Y2 = 0.2000001F;
            // 
            // line6
            // 
            this.line6.AnchorBottom = true;
            this.line6.Height = 0.2000001F;
            this.line6.Left = 5.31F;
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.Top = 0F;
            this.line6.Width = 0F;
            this.line6.X1 = 5.31F;
            this.line6.X2 = 5.31F;
            this.line6.Y1 = 0F;
            this.line6.Y2 = 0.2000001F;
            // 
            // line9
            // 
            this.line9.AnchorBottom = true;
            this.line9.Height = 0.2000001F;
            this.line9.Left = 7.268F;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.Top = 0F;
            this.line9.Width = 0F;
            this.line9.X1 = 7.268F;
            this.line9.X2 = 7.268F;
            this.line9.Y1 = 0F;
            this.line9.Y2 = 0.2000001F;
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
            // ReceiptSearchConditionSectionReport
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
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchValue1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchName1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchName2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblcompanycode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label label11;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSearchValue1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSearchName1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line8;
        private GrapeCity.ActiveReports.SectionReportModel.Line line7;
        private GrapeCity.ActiveReports.SectionReportModel.Line line12;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSearchValue2;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSearchName2;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.Line line4;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        private GrapeCity.ActiveReports.SectionReportModel.Line line6;
        private GrapeCity.ActiveReports.SectionReportModel.Line line9;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        public GrapeCity.ActiveReports.SectionReportModel.Label lbltitle;
    }
}
