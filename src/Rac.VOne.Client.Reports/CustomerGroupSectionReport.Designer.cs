namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerGroupSectionReport の概要の説明です。
    /// </summary>
    partial class CustomerGroupSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CustomerGroupSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblOutputDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblParentCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblParentCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblParentCustomerKana = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblChildCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblChildCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line13 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtParentCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtChildCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line11 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line14 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line10 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtChildCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtParentCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtParentCustomerKana = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentCustomerKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblChildCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblChildCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChildCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChildCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomerKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyCode,
            this.lblCompanyName,
            this.lblOutputDate,
            this.ridate,
            this.lblTitle,
            this.lblParentCustomerCode,
            this.lblParentCustomerName,
            this.lblParentCustomerKana,
            this.lblChildCustomerCode,
            this.lblChildCustomerName,
            this.line4,
            this.line5,
            this.line8,
            this.line6,
            this.line3,
            this.line13});
            this.pageHeader.Height = 0.9159167F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.02440945F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCode.Text = "会社コード　：";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 0.7874016F;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Height = 0.2F;
            this.lblCompanyName.HyperLink = null;
            this.lblCompanyName.Left = 0.811811F;
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyName.Text = "label2";
            this.lblCompanyName.Top = 0F;
            this.lblCompanyName.Width = 3.657087F;
            // 
            // lblOutputDate
            // 
            this.lblOutputDate.Height = 0.2F;
            this.lblOutputDate.HyperLink = null;
            this.lblOutputDate.Left = 5.856299F;
            this.lblOutputDate.Name = "lblOutputDate";
            this.lblOutputDate.Style = "color: Gray; font-size: 7pt; text-align: right; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblOutputDate.Text = "出力日付　：";
            this.lblOutputDate.Top = 0F;
            this.lblOutputDate.Width = 0.6153543F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 6.47126F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle; ddo-char-s" +
    "et: 1";
            this.ridate.Top = 0F;
            this.ridate.Width = 0.7480315F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline";
            this.lblTitle.Text = "債権代表者マスター一覧";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 7.244094F;
            // 
            // lblParentCustomerCode
            // 
            this.lblParentCustomerCode.Height = 0.319F;
            this.lblParentCustomerCode.Left = 0.001F;
            this.lblParentCustomerCode.Name = "lblParentCustomerCode";
            this.lblParentCustomerCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblParentCustomerCode.Text = "親得意先コード";
            this.lblParentCustomerCode.Top = 0.594F;
            this.lblParentCustomerCode.Width = 1.052F;
            // 
            // lblParentCustomerName
            // 
            this.lblParentCustomerName.Height = 0.319F;
            this.lblParentCustomerName.HyperLink = null;
            this.lblParentCustomerName.Left = 1.052F;
            this.lblParentCustomerName.Name = "lblParentCustomerName";
            this.lblParentCustomerName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblParentCustomerName.Text = "親得意先名";
            this.lblParentCustomerName.Top = 0.594F;
            this.lblParentCustomerName.Width = 1.72F;
            // 
            // lblParentCustomerKana
            // 
            this.lblParentCustomerKana.Height = 0.319F;
            this.lblParentCustomerKana.HyperLink = null;
            this.lblParentCustomerKana.Left = 2.767F;
            this.lblParentCustomerKana.Name = "lblParentCustomerKana";
            this.lblParentCustomerKana.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblParentCustomerKana.Text = "親得意先カナ";
            this.lblParentCustomerKana.Top = 0.594F;
            this.lblParentCustomerKana.Width = 1.879F;
            // 
            // lblChildCustomerCode
            // 
            this.lblChildCustomerCode.Height = 0.319F;
            this.lblChildCustomerCode.HyperLink = null;
            this.lblChildCustomerCode.Left = 4.649F;
            this.lblChildCustomerCode.Name = "lblChildCustomerCode";
            this.lblChildCustomerCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblChildCustomerCode.Text = "子得意先コード";
            this.lblChildCustomerCode.Top = 0.594F;
            this.lblChildCustomerCode.Width = 0.9700003F;
            // 
            // lblChildCustomerName
            // 
            this.lblChildCustomerName.Height = 0.319F;
            this.lblChildCustomerName.HyperLink = null;
            this.lblChildCustomerName.Left = 5.615F;
            this.lblChildCustomerName.Name = "lblChildCustomerName";
            this.lblChildCustomerName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblChildCustomerName.Text = "子得意先名";
            this.lblChildCustomerName.Top = 0.594F;
            this.lblChildCustomerName.Width = 1.617F;
            // 
            // line4
            // 
            this.line4.Height = 0.3267717F;
            this.line4.Left = 5.614961F;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.Top = 0.5917323F;
            this.line4.Width = 0F;
            this.line4.X1 = 5.614961F;
            this.line4.X2 = 5.614961F;
            this.line4.Y1 = 0.5917323F;
            this.line4.Y2 = 0.918504F;
            // 
            // line5
            // 
            this.line5.Height = 0F;
            this.line5.Left = 0F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0.595F;
            this.line5.Width = 7.23F;
            this.line5.X1 = 0F;
            this.line5.X2 = 7.23F;
            this.line5.Y1 = 0.595F;
            this.line5.Y2 = 0.595F;
            // 
            // line8
            // 
            this.line8.Height = 0F;
            this.line8.Left = 0F;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.Top = 0.9120001F;
            this.line8.Width = 7.229999F;
            this.line8.X1 = 0F;
            this.line8.X2 = 7.229999F;
            this.line8.Y1 = 0.9120001F;
            this.line8.Y2 = 0.9120001F;
            // 
            // line6
            // 
            this.line6.Height = 0.3287402F;
            this.line6.Left = 1.05315F;
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.Top = 0.5897638F;
            this.line6.Width = 0F;
            this.line6.X1 = 1.05315F;
            this.line6.X2 = 1.05315F;
            this.line6.Y1 = 0.5897638F;
            this.line6.Y2 = 0.918504F;
            // 
            // line3
            // 
            this.line3.Height = 0.3271654F;
            this.line3.Left = 4.644882F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0.5909449F;
            this.line3.Width = 0F;
            this.line3.X1 = 4.644882F;
            this.line3.X2 = 4.644882F;
            this.line3.Y1 = 0.5909449F;
            this.line3.Y2 = 0.9181103F;
            // 
            // line13
            // 
            this.line13.Height = 0.3271654F;
            this.line13.Left = 2.770079F;
            this.line13.LineWeight = 1F;
            this.line13.Name = "line13";
            this.line13.Top = 0.5909449F;
            this.line13.Width = 0F;
            this.line13.X1 = 2.770079F;
            this.line13.X2 = 2.770079F;
            this.line13.Y1 = 0.5909449F;
            this.line13.Y2 = 0.9181103F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtParentCustomerCode,
            this.txtChildCustomerCode,
            this.line11,
            this.line1,
            this.line14,
            this.line10,
            this.txtChildCustomerName,
            this.txtParentCustomerName,
            this.txtParentCustomerKana});
            this.detail.Height = 0.3033386F;
            this.detail.Name = "detail";
            // 
            // txtParentCustomerCode
            // 
            this.txtParentCustomerCode.CanGrow = false;
            this.txtParentCustomerCode.Height = 0.2992126F;
            this.txtParentCustomerCode.Left = 0F;
            this.txtParentCustomerCode.Name = "txtParentCustomerCode";
            this.txtParentCustomerCode.Style = "text-align: center; vertical-align: middle";
            this.txtParentCustomerCode.Text = null;
            this.txtParentCustomerCode.Top = 0F;
            this.txtParentCustomerCode.Width = 0.98F;
            // 
            // txtChildCustomerCode
            // 
            this.txtChildCustomerCode.CanGrow = false;
            this.txtChildCustomerCode.Height = 0.3031496F;
            this.txtChildCustomerCode.Left = 4.724803F;
            this.txtChildCustomerCode.Name = "txtChildCustomerCode";
            this.txtChildCustomerCode.Style = "text-align: center; vertical-align: middle";
            this.txtChildCustomerCode.Text = null;
            this.txtChildCustomerCode.Top = 1.862645E-09F;
            this.txtChildCustomerCode.Width = 0.8241973F;
            // 
            // line11
            // 
            this.line11.Height = 0.3031496F;
            this.line11.Left = 5.614961F;
            this.line11.LineWeight = 1F;
            this.line11.Name = "line11";
            this.line11.Top = 0F;
            this.line11.Width = 0F;
            this.line11.X1 = 5.614961F;
            this.line11.X2 = 5.614961F;
            this.line11.Y1 = 0F;
            this.line11.Y2 = 0.3031496F;
            // 
            // line1
            // 
            this.line1.Height = 0.3031496F;
            this.line1.Left = 4.644882F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0F;
            this.line1.Width = 0F;
            this.line1.X1 = 4.644882F;
            this.line1.X2 = 4.644882F;
            this.line1.Y1 = 0F;
            this.line1.Y2 = 0.3031496F;
            // 
            // line14
            // 
            this.line14.Height = 0.3031496F;
            this.line14.Left = 2.770079F;
            this.line14.LineWeight = 1F;
            this.line14.Name = "line14";
            this.line14.Top = 0F;
            this.line14.Width = 0F;
            this.line14.X1 = 2.770079F;
            this.line14.X2 = 2.770079F;
            this.line14.Y1 = 0F;
            this.line14.Y2 = 0.3031496F;
            // 
            // line10
            // 
            this.line10.Height = 0.3031496F;
            this.line10.Left = 1.05315F;
            this.line10.LineWeight = 1F;
            this.line10.Name = "line10";
            this.line10.Top = 0F;
            this.line10.Width = 0F;
            this.line10.X1 = 1.05315F;
            this.line10.X2 = 1.05315F;
            this.line10.Y1 = 0F;
            this.line10.Y2 = 0.3031496F;
            // 
            // txtChildCustomerName
            // 
            this.txtChildCustomerName.CanGrow = false;
            this.txtChildCustomerName.Height = 0.3033386F;
            this.txtChildCustomerName.Left = 5.699F;
            this.txtChildCustomerName.Name = "txtChildCustomerName";
            this.txtChildCustomerName.Style = "text-align: left; vertical-align: middle";
            this.txtChildCustomerName.Text = null;
            this.txtChildCustomerName.Top = 0F;
            this.txtChildCustomerName.Width = 1.545F;
            // 
            // txtParentCustomerName
            // 
            this.txtParentCustomerName.CanGrow = false;
            this.txtParentCustomerName.Height = 0.3033386F;
            this.txtParentCustomerName.Left = 1.105F;
            this.txtParentCustomerName.Name = "txtParentCustomerName";
            this.txtParentCustomerName.Style = "text-align: left; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: now" +
    "rap";
            this.txtParentCustomerName.Text = null;
            this.txtParentCustomerName.Top = 0F;
            this.txtParentCustomerName.Width = 1.552F;
            // 
            // txtParentCustomerKana
            // 
            this.txtParentCustomerKana.CanGrow = false;
            this.txtParentCustomerKana.Height = 0.2973386F;
            this.txtParentCustomerKana.Left = 2.824F;
            this.txtParentCustomerKana.Name = "txtParentCustomerKana";
            this.txtParentCustomerKana.Style = "text-align: left; vertical-align: middle";
            this.txtParentCustomerKana.Text = null;
            this.txtParentCustomerKana.Top = -4.656613E-10F;
            this.txtParentCustomerKana.Width = 1.716001F;
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
            // CustomerGroupSectionReport
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
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentCustomerKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblChildCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblChildCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChildCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChildCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomerKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblOutputDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox lblParentCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblParentCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblParentCustomerKana;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblChildCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblChildCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line line4;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtParentCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtChildCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line line11;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        private GrapeCity.ActiveReports.SectionReportModel.Line line8;
        private GrapeCity.ActiveReports.SectionReportModel.Line line6;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.Line line13;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line14;
        private GrapeCity.ActiveReports.SectionReportModel.Line line10;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtChildCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtParentCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtParentCustomerKana;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
    }
}
