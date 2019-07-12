namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// SectionReport1 の概要の説明です。
    /// </summary>
    partial class GeneralSettingSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GeneralSettingSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lbltitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label6 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label7 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label8 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label9 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lnheader1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.lndetail1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtcode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtvalue = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtlength = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtdescription = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtlength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtdescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyName,
            this.lbldate,
            this.lblCompanyCode,
            this.ridate,
            this.lbltitle,
            this.label6,
            this.label7,
            this.label8,
            this.label9,
            this.line6,
            this.lnheader1,
            this.line4,
            this.line7,
            this.line8});
            this.pageHeader.Height = 1.007416F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Height = 0.2F;
            this.lblCompanyName.HyperLink = null;
            this.lblCompanyName.Left = 0.811811F;
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyName.Style = "color: Gray; font-size: 7pt";
            this.lblCompanyName.Text = "label2";
            this.lblCompanyName.Top = 0F;
            this.lblCompanyName.Width = 3.657087F;
            // 
            // lbldate
            // 
            this.lbldate.Height = 0.2F;
            this.lbldate.HyperLink = null;
            this.lbldate.Left = 8.809055F;
            this.lbldate.Name = "lbldate";
            this.lbldate.Style = "color: Gray; font-size: 7pt; text-align: right";
            this.lbldate.Text = "出力日付　：";
            this.lbldate.Top = 0F;
            this.lbldate.Width = 0.6984252F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.02440945F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt";
            this.lblCompanyCode.Text = "会社コード　：";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 0.7874016F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 9.522441F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left";
            this.ridate.Top = 0F;
            this.ridate.Width = 1.015F;
            // 
            // lbltitle
            // 
            this.lbltitle.Height = 0.2311024F;
            this.lbltitle.HyperLink = null;
            this.lbltitle.Left = 0F;
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline";
            this.lbltitle.Text = "管理マスター一覧";
            this.lbltitle.Top = 0.2704725F;
            this.lbltitle.Width = 10.62992F;
            // 
            // label6
            // 
            this.label6.Height = 0.3189999F;
            this.label6.HyperLink = null;
            this.label6.Left = 0.062F;
            this.label6.Name = "label6";
            this.label6.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label6.Text = "管理コード";
            this.label6.Top = 0.678F;
            this.label6.Width = 2.522F;
            // 
            // label7
            // 
            this.label7.Height = 0.3189999F;
            this.label7.HyperLink = null;
            this.label7.Left = 2.584F;
            this.label7.Name = "label7";
            this.label7.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label7.Text = "データ";
            this.label7.Top = 0.678F;
            this.label7.Width = 3.073F;
            // 
            // label8
            // 
            this.label8.Height = 0.3189999F;
            this.label8.HyperLink = null;
            this.label8.Left = 5.657F;
            this.label8.Name = "label8";
            this.label8.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label8.Text = "有効桁数";
            this.label8.Top = 0.6810001F;
            this.label8.Width = 1.125677F;
            // 
            // label9
            // 
            this.label9.Height = 0.3189999F;
            this.label9.HyperLink = null;
            this.label9.Left = 6.782284F;
            this.label9.Name = "label9";
            this.label9.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.label9.Text = "説明";
            this.label9.Top = 0.6783465F;
            this.label9.Width = 3.769765F;
            // 
            // line6
            // 
            this.line6.Height = 0.003149509F;
            this.line6.Left = 0.07300001F;
            this.line6.LineWeight = 2F;
            this.line6.Name = "line6";
            this.line6.Top = 0.9968505F;
            this.line6.Width = 10.47936F;
            this.line6.X1 = 0.07300001F;
            this.line6.X2 = 10.55236F;
            this.line6.Y1 = 1F;
            this.line6.Y2 = 0.9968505F;
            // 
            // lnheader1
            // 
            this.lnheader1.Height = 4.714727E-05F;
            this.lnheader1.Left = 0.07300001F;
            this.lnheader1.LineWeight = 2F;
            this.lnheader1.Name = "lnheader1";
            this.lnheader1.Top = 0.6779528F;
            this.lnheader1.Width = 10.47897F;
            this.lnheader1.X1 = 0.07300001F;
            this.lnheader1.X2 = 10.55197F;
            this.lnheader1.Y1 = 0.678F;
            this.lnheader1.Y2 = 0.6779528F;
            // 
            // line4
            // 
            this.line4.Height = 0.3220473F;
            this.line4.Left = 2.583858F;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.Top = 0.6740158F;
            this.line4.Width = 0F;
            this.line4.X1 = 2.583858F;
            this.line4.X2 = 2.583858F;
            this.line4.Y1 = 0.6740158F;
            this.line4.Y2 = 0.9960631F;
            // 
            // line7
            // 
            this.line7.Height = 0.3220473F;
            this.line7.Left = 5.657087F;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.Top = 0.6740158F;
            this.line7.Width = 0F;
            this.line7.X1 = 5.657087F;
            this.line7.X2 = 5.657087F;
            this.line7.Y1 = 0.6740158F;
            this.line7.Y2 = 0.9960631F;
            // 
            // line8
            // 
            this.line8.Height = 0.3322832F;
            this.line8.Left = 6.786221F;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.Top = 0.6740158F;
            this.line8.Width = 0F;
            this.line8.X1 = 6.786221F;
            this.line8.X2 = 6.786221F;
            this.line8.Y1 = 0.6740158F;
            this.line8.Y2 = 1.006299F;
            // 
            // detail
            // 
            this.detail.CanGrow = false;
            this.detail.ColumnDirection = GrapeCity.ActiveReports.SectionReportModel.ColumnDirection.AcrossDown;
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lndetail1,
            this.txtcode,
            this.txtvalue,
            this.txtlength,
            this.line3,
            this.line5,
            this.txtdescription,
            this.line1});
            this.detail.Height = 0.3295014F;
            this.detail.Name = "detail";
            // 
            // lndetail1
            // 
            this.lndetail1.Height = 0.000110209F;
            this.lndetail1.Left = 0.06181103F;
            this.lndetail1.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lndetail1.LineWeight = 1F;
            this.lndetail1.Name = "lndetail1";
            this.lndetail1.Top = 0.3278426F;
            this.lndetail1.Width = 10.5006F;
            this.lndetail1.X1 = 0.06181103F;
            this.lndetail1.X2 = 10.56241F;
            this.lndetail1.Y1 = 0.3279528F;
            this.lndetail1.Y2 = 0.3278426F;
            // 
            // txtcode
            // 
            this.txtcode.Height = 0.302F;
            this.txtcode.Left = 0.062F;
            this.txtcode.Name = "txtcode";
            this.txtcode.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtcode.Style = "vertical-align: middle";
            this.txtcode.Text = "textBox1";
            this.txtcode.Top = 0.01F;
            this.txtcode.Width = 2.522F;
            // 
            // txtvalue
            // 
            this.txtvalue.Height = 0.302F;
            this.txtvalue.Left = 2.584F;
            this.txtvalue.Name = "txtvalue";
            this.txtvalue.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtvalue.Style = "vertical-align: middle";
            this.txtvalue.Text = "textBox2";
            this.txtvalue.Top = 0.01F;
            this.txtvalue.Width = 3.073F;
            // 
            // txtlength
            // 
            this.txtlength.Height = 0.302F;
            this.txtlength.Left = 5.657F;
            this.txtlength.Name = "txtlength";
            this.txtlength.OutputFormat = resources.GetString("txtlength.OutputFormat");
            this.txtlength.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.txtlength.Style = "text-align: right; text-justify: auto; vertical-align: middle";
            this.txtlength.Text = "textBox3";
            this.txtlength.Top = 0.01F;
            this.txtlength.Width = 1.125677F;
            // 
            // line3
            // 
            this.line3.Height = 0.3322835F;
            this.line3.Left = 2.583858F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0F;
            this.line3.Width = 0.0001420975F;
            this.line3.X1 = 2.584F;
            this.line3.X2 = 2.583858F;
            this.line3.Y1 = 0F;
            this.line3.Y2 = 0.3322835F;
            // 
            // line5
            // 
            this.line5.Height = 0.3320001F;
            this.line5.Left = 5.657087F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0F;
            this.line5.Width = 0F;
            this.line5.X1 = 5.657087F;
            this.line5.X2 = 5.657087F;
            this.line5.Y1 = 0F;
            this.line5.Y2 = 0.3320001F;
            // 
            // txtdescription
            // 
            this.txtdescription.Height = 0.302F;
            this.txtdescription.Left = 6.776772F;
            this.txtdescription.Name = "txtdescription";
            this.txtdescription.Padding = new GrapeCity.ActiveReports.PaddingEx(4, 0, 0, 0);
            this.txtdescription.Style = "vertical-align: middle";
            this.txtdescription.Text = "textBox4";
            this.txtdescription.Top = 0.009842521F;
            this.txtdescription.Width = 3.769764F;
            // 
            // line1
            // 
            this.line1.Height = 0.3295276F;
            this.line1.Left = 6.780709F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0F;
            this.line1.Width = 0F;
            this.line1.X1 = 6.780709F;
            this.line1.X2 = 6.780709F;
            this.line1.Y1 = 0F;
            this.line1.Y2 = 0.3295276F;
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
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center";
            this.reportInfo1.Top = 0.05748032F;
            this.reportInfo1.Width = 10.62992F;
            // 
            // GeneralSettingSectionReport
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
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtlength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtdescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbltitle;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label label6;
        private GrapeCity.ActiveReports.SectionReportModel.Label label7;
        private GrapeCity.ActiveReports.SectionReportModel.Label label8;
        private GrapeCity.ActiveReports.SectionReportModel.Label label9;
        private GrapeCity.ActiveReports.SectionReportModel.Line lndetail1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line6;
        private GrapeCity.ActiveReports.SectionReportModel.Line lnheader1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtlength;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtdescription;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtcode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtvalue;
        private GrapeCity.ActiveReports.SectionReportModel.Line line4;
        private GrapeCity.ActiveReports.SectionReportModel.Line line7;
        private GrapeCity.ActiveReports.SectionReportModel.Line line8;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
    }
}
