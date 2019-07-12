namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// DepartmentReport の概要の説明です。
    /// </summary>
    partial class DepartmentReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DepartmentReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblOutputDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblHeaderTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo2 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerDeptCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDeptName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerStaffCodeName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerStaffName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorSeparator = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyCode,
            this.lblOutputDate,
            this.lblHeaderTitle,
            this.lblCompanyName,
            this.reportInfo2,
            this.lblDepartmentCode,
            this.lblDepartmentName,
            this.lblStaffCodeName,
            this.lblNote,
            this.lineHeaderVerDeptCode,
            this.lineHeaderVerDeptName,
            this.lineHeaderVerStaffCodeName,
            this.lineHeaderHorUpper,
            this.lineHeaderHorLower});
            this.pageHeader.Height = 1.10475F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.02440945F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCode.Text = "会社コード：";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 0.7874016F;
            // 
            // lblOutputDate
            // 
            this.lblOutputDate.Height = 0.2F;
            this.lblOutputDate.HyperLink = null;
            this.lblOutputDate.Left = 8.809055F;
            this.lblOutputDate.Name = "lblOutputDate";
            this.lblOutputDate.Style = "color: Gray; font-size: 7pt; text-align: right; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblOutputDate.Text = "出力日付　:";
            this.lblOutputDate.Top = 0F;
            this.lblOutputDate.Width = 0.6984252F;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.Height = 0.2311024F;
            this.lblHeaderTitle.HyperLink = null;
            this.lblHeaderTitle.Left = 0F;
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline";
            this.lblHeaderTitle.Text = "請求部門マスター一覧";
            this.lblHeaderTitle.Top = 0.2704725F;
            this.lblHeaderTitle.Width = 10.62992F;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Height = 0.2F;
            this.lblCompanyName.HyperLink = null;
            this.lblCompanyName.Left = 0.811811F;
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyName.Text = "label4";
            this.lblCompanyName.Top = 0F;
            this.lblCompanyName.Width = 3.657087F;
            // 
            // reportInfo2
            // 
            this.reportInfo2.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.reportInfo2.Height = 0.2F;
            this.reportInfo2.Left = 9.522441F;
            this.reportInfo2.Name = "reportInfo2";
            this.reportInfo2.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.reportInfo2.Top = 0F;
            this.reportInfo2.Width = 1.014961F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.319F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 0F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentCode.Text = "請求部門コード";
            this.lblDepartmentCode.Top = 0.7920001F;
            this.lblDepartmentCode.Width = 1.208F;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Height = 0.319F;
            this.lblDepartmentName.HyperLink = null;
            this.lblDepartmentName.Left = 1.208F;
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentName.Text = "請求部門名";
            this.lblDepartmentName.Top = 0.7920001F;
            this.lblDepartmentName.Width = 2.479F;
            // 
            // lblStaffCodeName
            // 
            this.lblStaffCodeName.Height = 0.317F;
            this.lblStaffCodeName.HyperLink = null;
            this.lblStaffCodeName.Left = 3.687F;
            this.lblStaffCodeName.Name = "lblStaffCodeName";
            this.lblStaffCodeName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblStaffCodeName.Text = "回収責任者";
            this.lblStaffCodeName.Top = 0.794F;
            this.lblStaffCodeName.Width = 3.385F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.317F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 7.072001F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblNote.Text = "備考";
            this.lblNote.Top = 0.794F;
            this.lblNote.Width = 3.490204F;
            // 
            // lineHeaderVerDeptCode
            // 
            this.lineHeaderVerDeptCode.Height = 0.3189998F;
            this.lineHeaderVerDeptCode.Left = 1.135F;
            this.lineHeaderVerDeptCode.LineWeight = 1F;
            this.lineHeaderVerDeptCode.Name = "lineHeaderVerDeptCode";
            this.lineHeaderVerDeptCode.Top = 0.7920001F;
            this.lineHeaderVerDeptCode.Width = 0F;
            this.lineHeaderVerDeptCode.X1 = 1.135F;
            this.lineHeaderVerDeptCode.X2 = 1.135F;
            this.lineHeaderVerDeptCode.Y1 = 0.7920001F;
            this.lineHeaderVerDeptCode.Y2 = 1.111F;
            // 
            // lineHeaderVerDeptName
            // 
            this.lineHeaderVerDeptName.Height = 0.319F;
            this.lineHeaderVerDeptName.Left = 3.687F;
            this.lineHeaderVerDeptName.LineWeight = 1F;
            this.lineHeaderVerDeptName.Name = "lineHeaderVerDeptName";
            this.lineHeaderVerDeptName.Top = 0.794F;
            this.lineHeaderVerDeptName.Width = 0F;
            this.lineHeaderVerDeptName.X1 = 3.687F;
            this.lineHeaderVerDeptName.X2 = 3.687F;
            this.lineHeaderVerDeptName.Y1 = 0.794F;
            this.lineHeaderVerDeptName.Y2 = 1.113F;
            // 
            // lineHeaderVerStaffCodeName
            // 
            this.lineHeaderVerStaffCodeName.Height = 0.319F;
            this.lineHeaderVerStaffCodeName.Left = 7.072001F;
            this.lineHeaderVerStaffCodeName.LineWeight = 1F;
            this.lineHeaderVerStaffCodeName.Name = "lineHeaderVerStaffCodeName";
            this.lineHeaderVerStaffCodeName.Top = 0.794F;
            this.lineHeaderVerStaffCodeName.Width = 0F;
            this.lineHeaderVerStaffCodeName.X1 = 7.072001F;
            this.lineHeaderVerStaffCodeName.X2 = 7.072001F;
            this.lineHeaderVerStaffCodeName.Y1 = 0.794F;
            this.lineHeaderVerStaffCodeName.Y2 = 1.113F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 9.459257E-05F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.794F;
            this.lineHeaderHorUpper.Width = 10.5626F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.5626F;
            this.lineHeaderHorUpper.Y1 = 0.794F;
            this.lineHeaderHorUpper.Y2 = 0.7940946F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 7.987022E-06F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.112992F;
            this.lineHeaderHorLower.Width = 10.5626F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.5626F;
            this.lineHeaderHorLower.Y1 = 1.113F;
            this.lineHeaderHorLower.Y2 = 1.112992F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtDepartmentCode,
            this.txtDepartmentName,
            this.txtStaffCode,
            this.txtStaffName,
            this.txtNote,
            this.lineDetailVerDepartmentCode,
            this.lineDetailVerDepartmentName,
            this.lineDetailVerStaffName,
            this.lineDetailHorSeparator,
            this.lineDetailVerStaffCode});
            this.detail.Height = 0.3125001F;
            this.detail.Name = "detail";
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.Height = 0.304F;
            this.txtDepartmentCode.Left = 0F;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Style = "text-align: center; vertical-align: middle";
            this.txtDepartmentCode.Text = "DepartmentCode";
            this.txtDepartmentCode.Top = 0F;
            this.txtDepartmentCode.Width = 1.135F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.Height = 0.304F;
            this.txtDepartmentName.Left = 1.208F;
            this.txtDepartmentName.MultiLine = false;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Style = "vertical-align: middle";
            this.txtDepartmentName.Text = "DepartmentName";
            this.txtDepartmentName.Top = 0F;
            this.txtDepartmentName.Width = 2.479F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.Height = 0.304F;
            this.txtStaffCode.Left = 3.71063F;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Style = "text-align: center; vertical-align: middle";
            this.txtStaffCode.Text = "StaffCode";
            this.txtStaffCode.Top = 0F;
            this.txtStaffCode.Width = 1.083473F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.Height = 0.304F;
            this.txtStaffName.Left = 4.874804F;
            this.txtStaffName.MultiLine = false;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Style = "vertical-align: middle";
            this.txtStaffName.Text = "StaffName";
            this.txtStaffName.Top = 0F;
            this.txtStaffName.Width = 2.197197F;
            // 
            // txtNote
            // 
            this.txtNote.Height = 0.304F;
            this.txtNote.Left = 7.145F;
            this.txtNote.MultiLine = false;
            this.txtNote.Name = "txtNote";
            this.txtNote.Style = "vertical-align: middle";
            this.txtNote.Text = "Note";
            this.txtNote.Top = 0F;
            this.txtNote.Width = 3.406969F;
            // 
            // lineDetailVerDepartmentCode
            // 
            this.lineDetailVerDepartmentCode.Height = 0.304F;
            this.lineDetailVerDepartmentCode.Left = 1.135F;
            this.lineDetailVerDepartmentCode.LineWeight = 1F;
            this.lineDetailVerDepartmentCode.Name = "lineDetailVerDepartmentCode";
            this.lineDetailVerDepartmentCode.Top = 0F;
            this.lineDetailVerDepartmentCode.Width = 0F;
            this.lineDetailVerDepartmentCode.X1 = 1.135F;
            this.lineDetailVerDepartmentCode.X2 = 1.135F;
            this.lineDetailVerDepartmentCode.Y1 = 0.304F;
            this.lineDetailVerDepartmentCode.Y2 = 0F;
            // 
            // lineDetailVerDepartmentName
            // 
            this.lineDetailVerDepartmentName.Height = 0.304F;
            this.lineDetailVerDepartmentName.Left = 3.687F;
            this.lineDetailVerDepartmentName.LineWeight = 1F;
            this.lineDetailVerDepartmentName.Name = "lineDetailVerDepartmentName";
            this.lineDetailVerDepartmentName.Top = 0F;
            this.lineDetailVerDepartmentName.Width = 0F;
            this.lineDetailVerDepartmentName.X1 = 3.687F;
            this.lineDetailVerDepartmentName.X2 = 3.687F;
            this.lineDetailVerDepartmentName.Y1 = 0.304F;
            this.lineDetailVerDepartmentName.Y2 = 0F;
            // 
            // lineDetailVerStaffName
            // 
            this.lineDetailVerStaffName.Height = 0.304F;
            this.lineDetailVerStaffName.Left = 7.072001F;
            this.lineDetailVerStaffName.LineWeight = 1F;
            this.lineDetailVerStaffName.Name = "lineDetailVerStaffName";
            this.lineDetailVerStaffName.Top = 0F;
            this.lineDetailVerStaffName.Width = 0F;
            this.lineDetailVerStaffName.X1 = 7.072001F;
            this.lineDetailVerStaffName.X2 = 7.072001F;
            this.lineDetailVerStaffName.Y1 = 0.304F;
            this.lineDetailVerStaffName.Y2 = 0F;
            // 
            // lineDetailHorSeparator
            // 
            this.lineDetailHorSeparator.Height = 0F;
            this.lineDetailHorSeparator.Left = 0F;
            this.lineDetailHorSeparator.LineWeight = 1F;
            this.lineDetailHorSeparator.Name = "lineDetailHorSeparator";
            this.lineDetailHorSeparator.Top = 0.3066929F;
            this.lineDetailHorSeparator.Width = 10.56267F;
            this.lineDetailHorSeparator.X1 = 0F;
            this.lineDetailHorSeparator.X2 = 10.56267F;
            this.lineDetailHorSeparator.Y1 = 0.3066929F;
            this.lineDetailHorSeparator.Y2 = 0.3066929F;
            // 
            // lineDetailVerStaffCode
            // 
            this.lineDetailVerStaffCode.Height = 0.304F;
            this.lineDetailVerStaffCode.Left = 4.822441F;
            this.lineDetailVerStaffCode.LineWeight = 1F;
            this.lineDetailVerStaffCode.Name = "lineDetailVerStaffCode";
            this.lineDetailVerStaffCode.Top = 0F;
            this.lineDetailVerStaffCode.Width = 0F;
            this.lineDetailVerStaffCode.X1 = 4.822441F;
            this.lineDetailVerStaffCode.X2 = 4.822441F;
            this.lineDetailVerStaffCode.Y1 = 0.304F;
            this.lineDetailVerStaffCode.Y2 = 0F;
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
            // DepartmentReport
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
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblHeaderTitle;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyName;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo2;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDeptCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDeptName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerStaffCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorSeparator;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblOutputDate;
    }
}
