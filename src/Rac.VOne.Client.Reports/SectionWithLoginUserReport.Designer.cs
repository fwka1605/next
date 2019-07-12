namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// SectionWithLoginUserReport の概要の説明です。
    /// </summary>
    partial class SectionWithLoginUserReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SectionWithLoginUserReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblLoginUserCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblLoginUserName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSectionCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSectionName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerLoginUserCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSectionCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerLoginUserName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.lineDetailVerLoginUserCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerLoginUserName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSectionCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtLoginUserCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtLoginUserName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSectionCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSectionName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUserCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyCode,
            this.lblCompanyName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblLoginUserCode,
            this.lblLoginUserName,
            this.lblSectionCode,
            this.lblSectionName,
            this.lineHeaderVerLoginUserCode,
            this.lineHeaderVerSectionCode,
            this.lineHeaderVerLoginUserName,
            this.lineHeaderHorUpper,
            this.lineHeaderHorLower});
            this.pageHeader.Height = 0.9531499F;
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
            this.lblTitle.Text = "入金部門・担当者対応マスター一覧";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 7.244094F;
            // 
            // lblLoginUserCode
            // 
            this.lblLoginUserCode.Height = 0.3468504F;
            this.lblLoginUserCode.Left = 0.001F;
            this.lblLoginUserCode.Name = "lblLoginUserCode";
            this.lblLoginUserCode.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle";
            this.lblLoginUserCode.Text = "ログインユーザーコード";
            this.lblLoginUserCode.Top = 0.6059055F;
            this.lblLoginUserCode.Width = 1.181102F;
            // 
            // lblLoginUserName
            // 
            this.lblLoginUserName.Height = 0.3468504F;
            this.lblLoginUserName.HyperLink = null;
            this.lblLoginUserName.Left = 1.182284F;
            this.lblLoginUserName.Name = "lblLoginUserName";
            this.lblLoginUserName.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle";
            this.lblLoginUserName.Text = "ログインユーザー名";
            this.lblLoginUserName.Top = 0.6059055F;
            this.lblLoginUserName.Width = 2.274803F;
            // 
            // lblSectionCode
            // 
            this.lblSectionCode.Height = 0.3468504F;
            this.lblSectionCode.HyperLink = null;
            this.lblSectionCode.Left = 3.457087F;
            this.lblSectionCode.Name = "lblSectionCode";
            this.lblSectionCode.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle";
            this.lblSectionCode.Text = "入金部門コード";
            this.lblSectionCode.Top = 0.6059055F;
            this.lblSectionCode.Width = 1.280709F;
            // 
            // lblSectionName
            // 
            this.lblSectionName.Height = 0.3468504F;
            this.lblSectionName.HyperLink = null;
            this.lblSectionName.Left = 4.737795F;
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle";
            this.lblSectionName.Text = "入金部門名";
            this.lblSectionName.Top = 0.6059055F;
            this.lblSectionName.Width = 2.5F;
            // 
            // lineHeaderVerLoginUserCode
            // 
            this.lineHeaderVerLoginUserCode.Height = 0.3429134F;
            this.lineHeaderVerLoginUserCode.Left = 1.182284F;
            this.lineHeaderVerLoginUserCode.LineWeight = 1F;
            this.lineHeaderVerLoginUserCode.Name = "lineHeaderVerLoginUserCode";
            this.lineHeaderVerLoginUserCode.Top = 0.6059055F;
            this.lineHeaderVerLoginUserCode.Width = 0F;
            this.lineHeaderVerLoginUserCode.X1 = 1.182284F;
            this.lineHeaderVerLoginUserCode.X2 = 1.182284F;
            this.lineHeaderVerLoginUserCode.Y1 = 0.6059055F;
            this.lineHeaderVerLoginUserCode.Y2 = 0.9488189F;
            // 
            // lineHeaderVerSectionCode
            // 
            this.lineHeaderVerSectionCode.Height = 0.3429134F;
            this.lineHeaderVerSectionCode.Left = 4.737796F;
            this.lineHeaderVerSectionCode.LineWeight = 1F;
            this.lineHeaderVerSectionCode.Name = "lineHeaderVerSectionCode";
            this.lineHeaderVerSectionCode.Top = 0.6059055F;
            this.lineHeaderVerSectionCode.Width = 0F;
            this.lineHeaderVerSectionCode.X1 = 4.737796F;
            this.lineHeaderVerSectionCode.X2 = 4.737796F;
            this.lineHeaderVerSectionCode.Y1 = 0.6059055F;
            this.lineHeaderVerSectionCode.Y2 = 0.9488189F;
            // 
            // lineHeaderVerLoginUserName
            // 
            this.lineHeaderVerLoginUserName.Height = 0.3429134F;
            this.lineHeaderVerLoginUserName.Left = 3.457087F;
            this.lineHeaderVerLoginUserName.LineWeight = 1F;
            this.lineHeaderVerLoginUserName.Name = "lineHeaderVerLoginUserName";
            this.lineHeaderVerLoginUserName.Top = 0.6059055F;
            this.lineHeaderVerLoginUserName.Width = 0F;
            this.lineHeaderVerLoginUserName.X1 = 3.457087F;
            this.lineHeaderVerLoginUserName.X2 = 3.457087F;
            this.lineHeaderVerLoginUserName.Y1 = 0.6059055F;
            this.lineHeaderVerLoginUserName.Y2 = 0.9488189F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0.001181102F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.6059055F;
            this.lineHeaderHorUpper.Width = 7.235039F;
            this.lineHeaderHorUpper.X1 = 0.001181102F;
            this.lineHeaderHorUpper.X2 = 7.23622F;
            this.lineHeaderHorUpper.Y1 = 0.6059055F;
            this.lineHeaderHorUpper.Y2 = 0.6059055F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0.001181102F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 0.9527559F;
            this.lineHeaderHorLower.Width = 7.235039F;
            this.lineHeaderHorLower.X1 = 0.001181102F;
            this.lineHeaderHorLower.X2 = 7.23622F;
            this.lineHeaderHorLower.Y1 = 0.9527559F;
            this.lineHeaderHorLower.Y2 = 0.9527559F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lineDetailVerLoginUserCode,
            this.lineDetailVerLoginUserName,
            this.lineDetailVerSectionCode,
            this.txtLoginUserCode,
            this.txtLoginUserName,
            this.txtSectionCode,
            this.txtSectionName});
            this.detail.Height = 0.5129102F;
            this.detail.Name = "detail";
            // 
            // lineDetailVerLoginUserCode
            // 
            this.lineDetailVerLoginUserCode.Height = 0.3287403F;
            this.lineDetailVerLoginUserCode.Left = 1.182284F;
            this.lineDetailVerLoginUserCode.LineWeight = 1F;
            this.lineDetailVerLoginUserCode.Name = "lineDetailVerLoginUserCode";
            this.lineDetailVerLoginUserCode.Top = 0F;
            this.lineDetailVerLoginUserCode.Width = 0F;
            this.lineDetailVerLoginUserCode.X1 = 1.182284F;
            this.lineDetailVerLoginUserCode.X2 = 1.182284F;
            this.lineDetailVerLoginUserCode.Y1 = 0F;
            this.lineDetailVerLoginUserCode.Y2 = 0.3287403F;
            // 
            // lineDetailVerLoginUserName
            // 
            this.lineDetailVerLoginUserName.Height = 0.3287403F;
            this.lineDetailVerLoginUserName.Left = 3.457087F;
            this.lineDetailVerLoginUserName.LineWeight = 1F;
            this.lineDetailVerLoginUserName.Name = "lineDetailVerLoginUserName";
            this.lineDetailVerLoginUserName.Top = 0F;
            this.lineDetailVerLoginUserName.Width = 0F;
            this.lineDetailVerLoginUserName.X1 = 3.457087F;
            this.lineDetailVerLoginUserName.X2 = 3.457087F;
            this.lineDetailVerLoginUserName.Y1 = 0F;
            this.lineDetailVerLoginUserName.Y2 = 0.3287403F;
            // 
            // lineDetailVerSectionCode
            // 
            this.lineDetailVerSectionCode.Height = 0.3287403F;
            this.lineDetailVerSectionCode.Left = 4.737796F;
            this.lineDetailVerSectionCode.LineWeight = 1F;
            this.lineDetailVerSectionCode.Name = "lineDetailVerSectionCode";
            this.lineDetailVerSectionCode.Top = 0F;
            this.lineDetailVerSectionCode.Width = 0F;
            this.lineDetailVerSectionCode.X1 = 4.737796F;
            this.lineDetailVerSectionCode.X2 = 4.737796F;
            this.lineDetailVerSectionCode.Y1 = 0F;
            this.lineDetailVerSectionCode.Y2 = 0.3287403F;
            // 
            // txtLoginUserCode
            // 
            this.txtLoginUserCode.Height = 0.2870079F;
            this.txtLoginUserCode.Left = 0.02204728F;
            this.txtLoginUserCode.Name = "txtLoginUserCode";
            this.txtLoginUserCode.Style = "text-align: center";
            this.txtLoginUserCode.Text = null;
            this.txtLoginUserCode.Top = 0.007086615F;
            this.txtLoginUserCode.Width = 1.104331F;
            // 
            // txtLoginUserName
            // 
            this.txtLoginUserName.Height = 0.2870079F;
            this.txtLoginUserName.Left = 1.270866F;
            this.txtLoginUserName.Name = "txtLoginUserName";
            this.txtLoginUserName.Style = "white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtLoginUserName.Text = null;
            this.txtLoginUserName.Top = 0.007086615F;
            this.txtLoginUserName.Width = 2.134252F;
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.Height = 0.2870079F;
            this.txtSectionCode.Left = 3.551969F;
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Style = "text-align: center";
            this.txtSectionCode.Text = null;
            this.txtSectionCode.Top = 0.007086615F;
            this.txtSectionCode.Width = 1.135433F;
            // 
            // txtSectionName
            // 
            this.txtSectionName.Height = 0.2799213F;
            this.txtSectionName.Left = 4.820867F;
            this.txtSectionName.Name = "txtSectionName";
            this.txtSectionName.Style = "white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtSectionName.Text = null;
            this.txtSectionName.Top = 0.01417323F;
            this.txtSectionName.Width = 2.326772F;
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
            // SectionWithLoginUserReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.MirrorMargins = true;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
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
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUserCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox lblLoginUserCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblLoginUserName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerLoginUserCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerLoginUserName;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerLoginUserCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerLoginUserName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLoginUserCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLoginUserName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSectionName;
    }
}
