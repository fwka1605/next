namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// JuridicalPersonalityReport の概要の説明です。
    /// </summary>
    partial class JuridicalPersonalityReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(JuridicalPersonalityReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo2 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblIndex = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblJuridicalName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerIndex = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtIndex = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerIndex = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtKana = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailHorSeparator = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblJuridicalName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblTitle,
            this.lineHeaderHorUpper,
            this.lblDate,
            this.reportInfo2,
            this.lblIndex,
            this.lblJuridicalName,
            this.lineHeaderVerIndex,
            this.lineHeaderHorLower});
            this.pageHeader.Height = 1.00748F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.02440945F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.lblCompanyCode.Text = "会社コード：";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 0.7874016F;
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.811811F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.lblCompanyCodeName.Text = "label2";
            this.lblCompanyCodeName.Top = 0F;
            this.lblCompanyCodeName.Width = 3.657087F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline";
            this.lblTitle.Text = "法人格マスター一覧\r\n";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 1.957677F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.688189F;
            this.lineHeaderHorUpper.Width = 6.714764F;
            this.lineHeaderHorUpper.X1 = 1.957677F;
            this.lineHeaderHorUpper.X2 = 8.672441F;
            this.lineHeaderHorUpper.Y1 = 0.688189F;
            this.lineHeaderHorUpper.Y2 = 0.688189F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 8.809055F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-size: 7pt; text-align: right; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblDate.Text = "出力日付 :";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.6984252F;
            // 
            // reportInfo2
            // 
            this.reportInfo2.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.reportInfo2.Height = 0.2F;
            this.reportInfo2.Left = 9.518898F;
            this.reportInfo2.Name = "reportInfo2";
            this.reportInfo2.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.reportInfo2.Top = 0F;
            this.reportInfo2.Width = 1.014961F;
            // 
            // lblIndex
            // 
            this.lblIndex.Height = 0.3082677F;
            this.lblIndex.HyperLink = null;
            this.lblIndex.Left = 1.95748F;
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblIndex.Text = "";
            this.lblIndex.Top = 0.6948819F;
            this.lblIndex.Width = 0.5929134F;
            // 
            // lblJuridicalName
            // 
            this.lblJuridicalName.Height = 0.3082677F;
            this.lblJuridicalName.HyperLink = null;
            this.lblJuridicalName.Left = 2.550394F;
            this.lblJuridicalName.Name = "lblJuridicalName";
            this.lblJuridicalName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblJuridicalName.Text = "法人格\r\n";
            this.lblJuridicalName.Top = 0.6948819F;
            this.lblJuridicalName.Width = 6.121654F;
            // 
            // lineHeaderVerIndex
            // 
            this.lineHeaderVerIndex.Height = 0.319F;
            this.lineHeaderVerIndex.Left = 2.560236F;
            this.lineHeaderVerIndex.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderVerIndex.LineWeight = 1F;
            this.lineHeaderVerIndex.Name = "lineHeaderVerIndex";
            this.lineHeaderVerIndex.Top = 0.688189F;
            this.lineHeaderVerIndex.Width = 0F;
            this.lineHeaderVerIndex.X1 = 2.560236F;
            this.lineHeaderVerIndex.X2 = 2.560236F;
            this.lineHeaderVerIndex.Y1 = 0.688189F;
            this.lineHeaderVerIndex.Y2 = 1.007189F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 1.95748F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.007087F;
            this.lineHeaderHorLower.Width = 6.714961F;
            this.lineHeaderHorLower.X1 = 1.95748F;
            this.lineHeaderHorLower.X2 = 8.672441F;
            this.lineHeaderHorLower.Y1 = 1.007087F;
            this.lineHeaderHorLower.Y2 = 1.007087F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtIndex,
            this.lineDetailVerIndex,
            this.txtKana,
            this.lineDetailHorSeparator});
            this.detail.Height = 0.3125984F;
            this.detail.Name = "detail";
            // 
            // txtIndex
            // 
            this.txtIndex.CountNullValues = true;
            this.txtIndex.Height = 0.3003937F;
            this.txtIndex.Left = 1.95748F;
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Style = "text-align: right; vertical-align: middle";
            this.txtIndex.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count;
            this.txtIndex.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All;
            this.txtIndex.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtIndex.Text = "textBox1";
            this.txtIndex.Top = 0F;
            this.txtIndex.Width = 0.5511816F;
            // 
            // lineDetailVerIndex
            // 
            this.lineDetailVerIndex.Height = 0.3003937F;
            this.lineDetailVerIndex.Left = 2.560189F;
            this.lineDetailVerIndex.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineDetailVerIndex.LineWeight = 1F;
            this.lineDetailVerIndex.Name = "lineDetailVerIndex";
            this.lineDetailVerIndex.Top = 0F;
            this.lineDetailVerIndex.Width = 4.696846E-05F;
            this.lineDetailVerIndex.X1 = 2.560236F;
            this.lineDetailVerIndex.X2 = 2.560189F;
            this.lineDetailVerIndex.Y1 = 0F;
            this.lineDetailVerIndex.Y2 = 0.3003937F;
            // 
            // txtKana
            // 
            this.txtKana.Height = 0.3003937F;
            this.txtKana.Left = 2.572047F;
            this.txtKana.MultiLine = false;
            this.txtKana.Name = "txtKana";
            this.txtKana.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtKana.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtKana.Text = "textBox2";
            this.txtKana.Top = 0F;
            this.txtKana.Width = 6.103937F;
            // 
            // lineDetailHorSeparator
            // 
            this.lineDetailHorSeparator.Height = 0F;
            this.lineDetailHorSeparator.Left = 1.957677F;
            this.lineDetailHorSeparator.LineWeight = 1F;
            this.lineDetailHorSeparator.Name = "lineDetailHorSeparator";
            this.lineDetailHorSeparator.Top = 0.3003937F;
            this.lineDetailHorSeparator.Width = 6.714764F;
            this.lineDetailHorSeparator.X1 = 1.957677F;
            this.lineDetailHorSeparator.X2 = 8.672441F;
            this.lineDetailHorSeparator.Y1 = 0.3003937F;
            this.lineDetailHorSeparator.Y2 = 0.3003937F;
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
            // JuridicalPersonalityReport
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
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblJuridicalName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo2;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblIndex;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblJuridicalName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerIndex;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerIndex;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtKana;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorSeparator;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtIndex;
    }
}
