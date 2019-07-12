﻿namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillSectionReport の概要の説明です。
    /// </summary>
    partial class ArrearagesCustomerListReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ArrearagesCustomerListReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffname = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerRemianAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblRemainAmount,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblCustomerCode,
            this.lblCustomerName,
            this.lblDepartmentCode,
            this.lblDepartmentName,
            this.lblStaffCode,
            this.lblStaffname,
            this.lineHeaderHorLower,
            this.lineHeaderHorUpper,
            this.lineHeaderVerDepartmentCode,
            this.lineHeaderVerRemainAmount,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderHorCustomerCode,
            this.lineHeaderHorDepartmentCode});
            this.pageHeader.Height = 1.111811F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Height = 0.496063F;
            this.lblRemainAmount.HyperLink = null;
            this.lblRemainAmount.Left = 2.76378F;
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblRemainAmount.Text = "回収予定金額";
            this.lblRemainAmount.Top = 0.6181102F;
            this.lblRemainAmount.Width = 2.362205F;
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
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.811811F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
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
            this.lblDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDate.Text = "出力日付　：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.6984252F;
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
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "滞留明細一覧表";
            this.lblTitle.Top = 0.2704724F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.25F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 0F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.6181103F;
            this.lblCustomerCode.Width = 2.755906F;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Height = 0.25F;
            this.lblCustomerName.HyperLink = null;
            this.lblCustomerName.Left = 0F;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.Top = 0.8598425F;
            this.lblCustomerName.Width = 2.755906F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.25F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 5.129921F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentCode.Text = "請求部門コード";
            this.lblDepartmentCode.Top = 0.6181102F;
            this.lblDepartmentCode.Width = 2.755906F;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Height = 0.25F;
            this.lblDepartmentName.HyperLink = null;
            this.lblDepartmentName.Left = 5.129921F;
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentName.Text = "請求部門名";
            this.lblDepartmentName.Top = 0.8598425F;
            this.lblDepartmentName.Width = 2.755906F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.25F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 7.889764F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblStaffCode.Text = "担当者コード";
            this.lblStaffCode.Top = 0.6181102F;
            this.lblStaffCode.Width = 2.755906F;
            // 
            // lblStaffname
            // 
            this.lblStaffname.Height = 0.25F;
            this.lblStaffname.HyperLink = null;
            this.lblStaffname.Left = 7.889764F;
            this.lblStaffname.Name = "lblStaffname";
            this.lblStaffname.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblStaffname.Text = "担当者名";
            this.lblStaffname.Top = 0.8598425F;
            this.lblStaffname.Width = 2.755906F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.111811F;
            this.lineHeaderHorLower.Width = 10.62992F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.62992F;
            this.lineHeaderHorLower.Y1 = 1.111811F;
            this.lineHeaderHorLower.Y2 = 1.111811F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.6141732F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.62992F;
            this.lineHeaderHorUpper.Y1 = 0.6141732F;
            this.lineHeaderHorUpper.Y2 = 0.6141732F;
            // 
            // lineHeaderVerDepartmentCode
            // 
            this.lineHeaderVerDepartmentCode.Height = 0.4976379F;
            this.lineHeaderVerDepartmentCode.Left = 7.88189F;
            this.lineHeaderVerDepartmentCode.LineWeight = 1F;
            this.lineHeaderVerDepartmentCode.Name = "lineHeaderVerDepartmentCode";
            this.lineHeaderVerDepartmentCode.Top = 0.6141732F;
            this.lineHeaderVerDepartmentCode.Width = 0F;
            this.lineHeaderVerDepartmentCode.X1 = 7.88189F;
            this.lineHeaderVerDepartmentCode.X2 = 7.88189F;
            this.lineHeaderVerDepartmentCode.Y1 = 0.6141732F;
            this.lineHeaderVerDepartmentCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerRemainAmount
            // 
            this.lineHeaderVerRemainAmount.Height = 0.4976379F;
            this.lineHeaderVerRemainAmount.Left = 5.122047F;
            this.lineHeaderVerRemainAmount.LineWeight = 1F;
            this.lineHeaderVerRemainAmount.Name = "lineHeaderVerRemainAmount";
            this.lineHeaderVerRemainAmount.Top = 0.6141732F;
            this.lineHeaderVerRemainAmount.Width = 0F;
            this.lineHeaderVerRemainAmount.X1 = 5.122047F;
            this.lineHeaderVerRemainAmount.X2 = 5.122047F;
            this.lineHeaderVerRemainAmount.Y1 = 0.6141732F;
            this.lineHeaderVerRemainAmount.Y2 = 1.111811F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.4976379F;
            this.lineHeaderVerCustomerCode.Left = 2.755906F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.6141732F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 2.755906F;
            this.lineHeaderVerCustomerCode.X2 = 2.755906F;
            this.lineHeaderVerCustomerCode.Y1 = 0.6141732F;
            this.lineHeaderVerCustomerCode.Y2 = 1.111811F;
            // 
            // lineHeaderHorCustomerCode
            // 
            this.lineHeaderHorCustomerCode.Height = 0F;
            this.lineHeaderHorCustomerCode.Left = 0F;
            this.lineHeaderHorCustomerCode.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorCustomerCode.LineWeight = 1F;
            this.lineHeaderHorCustomerCode.Name = "lineHeaderHorCustomerCode";
            this.lineHeaderHorCustomerCode.Top = 0.8622047F;
            this.lineHeaderHorCustomerCode.Width = 2.755906F;
            this.lineHeaderHorCustomerCode.X1 = 0F;
            this.lineHeaderHorCustomerCode.X2 = 2.755906F;
            this.lineHeaderHorCustomerCode.Y1 = 0.8622047F;
            this.lineHeaderHorCustomerCode.Y2 = 0.8622047F;
            // 
            // lineHeaderHorDepartmentCode
            // 
            this.lineHeaderHorDepartmentCode.Height = 0F;
            this.lineHeaderHorDepartmentCode.Left = 5.122047F;
            this.lineHeaderHorDepartmentCode.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorDepartmentCode.LineWeight = 1F;
            this.lineHeaderHorDepartmentCode.Name = "lineHeaderHorDepartmentCode";
            this.lineHeaderHorDepartmentCode.Top = 0.8622047F;
            this.lineHeaderHorDepartmentCode.Width = 5.507873F;
            this.lineHeaderHorDepartmentCode.X1 = 5.122047F;
            this.lineHeaderHorDepartmentCode.X2 = 10.62992F;
            this.lineHeaderHorDepartmentCode.Y1 = 0.8622047F;
            this.lineHeaderHorDepartmentCode.Y2 = 0.8622047F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCustomerName,
            this.txtStaffCode,
            this.txtDepartmentName,
            this.txtCustomerCode,
            this.txtDepartmentCode,
            this.txtStaffName,
            this.lineDetailVerDepartmentCode,
            this.txtRemainAmount,
            this.lineDetailVerRemianAmount,
            this.lineDetailVerCustomerCode,
            this.lineDetailHorLower});
            this.detail.Height = 0.4598425F;
            this.detail.Name = "detail";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.DataField = "CustomerName";
            this.txtCustomerName.Height = 0.2244094F;
            this.txtCustomerName.Left = 0F;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerName.Style = "text-align: left; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: now" +
    "rap";
            this.txtCustomerName.Text = null;
            this.txtCustomerName.Top = 0.2322835F;
            this.txtCustomerName.Width = 2.755906F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.DataField = "StaffCode";
            this.txtStaffCode.Height = 0.2244094F;
            this.txtStaffCode.Left = 7.889764F;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtStaffCode.Text = null;
            this.txtStaffCode.Top = 0F;
            this.txtStaffCode.Width = 2.740158F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.DataField = "DepartmentName";
            this.txtDepartmentName.Height = 0.2244094F;
            this.txtDepartmentName.Left = 5.133858F;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtDepartmentName.Text = null;
            this.txtDepartmentName.Top = 0.2322835F;
            this.txtDepartmentName.Width = 2.740158F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.DataField = "CustomerCode";
            this.txtCustomerCode.Height = 0.2244094F;
            this.txtCustomerCode.Left = 0F;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerCode.Style = "text-align: left; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: now" +
    "rap";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 2.755906F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.DataField = "DepartmentCode";
            this.txtDepartmentCode.Height = 0.2244094F;
            this.txtDepartmentCode.Left = 5.133858F;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Style = "white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtDepartmentCode.Text = null;
            this.txtDepartmentCode.Top = 0F;
            this.txtDepartmentCode.Width = 2.740158F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.DataField = "StaffName";
            this.txtStaffName.Height = 0.2244094F;
            this.txtStaffName.Left = 7.889764F;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtStaffName.Text = null;
            this.txtStaffName.Top = 0.2322835F;
            this.txtStaffName.Width = 2.740158F;
            // 
            // lineDetailVerDepartmentCode
            // 
            this.lineDetailVerDepartmentCode.Height = 0.4598425F;
            this.lineDetailVerDepartmentCode.Left = 7.88189F;
            this.lineDetailVerDepartmentCode.LineWeight = 1F;
            this.lineDetailVerDepartmentCode.Name = "lineDetailVerDepartmentCode";
            this.lineDetailVerDepartmentCode.Top = 0F;
            this.lineDetailVerDepartmentCode.Width = 0F;
            this.lineDetailVerDepartmentCode.X1 = 7.88189F;
            this.lineDetailVerDepartmentCode.X2 = 7.88189F;
            this.lineDetailVerDepartmentCode.Y1 = 0F;
            this.lineDetailVerDepartmentCode.Y2 = 0.4598425F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.DataField = "TotalAmount";
            this.txtRemainAmount.Height = 0.4566929F;
            this.txtRemainAmount.Left = 2.755906F;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainAmount.Style = "text-align: right; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: no" +
    "wrap";
            this.txtRemainAmount.Text = null;
            this.txtRemainAmount.Top = 0F;
            this.txtRemainAmount.Width = 2.35748F;
            // 
            // lineDetailVerRemianAmount
            // 
            this.lineDetailVerRemianAmount.Height = 0.4598425F;
            this.lineDetailVerRemianAmount.Left = 5.121933F;
            this.lineDetailVerRemianAmount.LineWeight = 1F;
            this.lineDetailVerRemianAmount.Name = "lineDetailVerRemianAmount";
            this.lineDetailVerRemianAmount.Top = 0F;
            this.lineDetailVerRemianAmount.Width = 0.0001139641F;
            this.lineDetailVerRemianAmount.X1 = 5.122047F;
            this.lineDetailVerRemianAmount.X2 = 5.121933F;
            this.lineDetailVerRemianAmount.Y1 = 0F;
            this.lineDetailVerRemianAmount.Y2 = 0.4598425F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.4598425F;
            this.lineDetailVerCustomerCode.Left = 2.755906F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 2.755906F;
            this.lineDetailVerCustomerCode.X2 = 2.755906F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.4598425F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.4598425F;
            this.lineDetailHorLower.Width = 10.62992F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.62992F;
            this.lineDetailHorLower.Y1 = 0.4598425F;
            this.lineDetailHorLower.Y2 = 0.4598425F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.reportInfo1,
            this.lblPageNumber});
            this.pageFooter.Height = 0.3149606F;
            this.pageFooter.Name = "pageFooter";
            this.pageFooter.BeforePrint += new System.EventHandler(this.pageFooter_BeforePrint);
            // 
            // reportInfo1
            // 
            this.reportInfo1.FormatString = "{PageNumber} / {PageCount}";
            this.reportInfo1.Height = 0.2F;
            this.reportInfo1.Left = 7.065001F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1.181102F;
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
            // groupHeader1
            // 
            this.groupHeader1.Height = 0F;
            this.groupHeader1.Name = "groupHeader1";
            // 
            // groupFooter1
            // 
            this.groupFooter1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblSpace,
            this.txtTotalAmount,
            this.lblTotalAmount,
            this.lineFooterHorLower,
            this.lineFooterVerTotal,
            this.lineFooterVerTotalAmount});
            this.groupFooter1.Height = 0.4122047F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // lblSpace
            // 
            this.lblSpace.Height = 0.4122047F;
            this.lblSpace.HyperLink = null;
            this.lblSpace.Left = 5.133858F;
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblSpace.Style = "background-color: WhiteSmoke; vertical-align: middle";
            this.lblSpace.Text = "";
            this.lblSpace.Top = 0F;
            this.lblSpace.Width = 5.492126F;
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.DataField = "TotalAmount";
            this.txtTotalAmount.Height = 0.4122047F;
            this.txtTotalAmount.Left = 2.755906F;
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtTotalAmount.Style = "background-color: WhiteSmoke; text-align: right; vertical-align: middle; white-sp" +
    "ace: nowrap; ddo-wrap-mode: nowrap";
            this.txtTotalAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalAmount.Text = null;
            this.txtTotalAmount.Top = 0F;
            this.txtTotalAmount.Width = 2.362205F;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Height = 0.4122047F;
            this.lblTotalAmount.HyperLink = null;
            this.lblTotalAmount.Left = 0F;
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblTotalAmount.Style = "background-color: WhiteSmoke; vertical-align: middle";
            this.lblTotalAmount.Text = "総合計";
            this.lblTotalAmount.Top = 0F;
            this.lblTotalAmount.Width = 2.755906F;
            // 
            // lineFooterHorLower
            // 
            this.lineFooterHorLower.Height = 0F;
            this.lineFooterHorLower.Left = 0F;
            this.lineFooterHorLower.LineWeight = 1F;
            this.lineFooterHorLower.Name = "lineFooterHorLower";
            this.lineFooterHorLower.Top = 0.4122047F;
            this.lineFooterHorLower.Width = 10.62992F;
            this.lineFooterHorLower.X1 = 0F;
            this.lineFooterHorLower.X2 = 10.62992F;
            this.lineFooterHorLower.Y1 = 0.4122047F;
            this.lineFooterHorLower.Y2 = 0.4122047F;
            // 
            // lineFooterVerTotal
            // 
            this.lineFooterVerTotal.Height = 0.4122047F;
            this.lineFooterVerTotal.Left = 2.755906F;
            this.lineFooterVerTotal.LineWeight = 1F;
            this.lineFooterVerTotal.Name = "lineFooterVerTotal";
            this.lineFooterVerTotal.Top = 0F;
            this.lineFooterVerTotal.Width = 0F;
            this.lineFooterVerTotal.X1 = 2.755906F;
            this.lineFooterVerTotal.X2 = 2.755906F;
            this.lineFooterVerTotal.Y1 = 0F;
            this.lineFooterVerTotal.Y2 = 0.4122047F;
            // 
            // lineFooterVerTotalAmount
            // 
            this.lineFooterVerTotalAmount.Height = 0.4122047F;
            this.lineFooterVerTotalAmount.Left = 5.123229F;
            this.lineFooterVerTotalAmount.LineWeight = 1F;
            this.lineFooterVerTotalAmount.Name = "lineFooterVerTotalAmount";
            this.lineFooterVerTotalAmount.Top = 0F;
            this.lineFooterVerTotalAmount.Width = 0F;
            this.lineFooterVerTotalAmount.X1 = 5.123229F;
            this.lineFooterVerTotalAmount.X2 = 5.123229F;
            this.lineFooterVerTotalAmount.Y1 = 0F;
            this.lineFooterVerTotalAmount.Y2 = 0.4122047F;
            // 
            // ArrearagesCustomerListReport
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
            this.Sections.Add(this.groupHeader1);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.groupFooter1);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffname;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRemianAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorDepartmentCode;
    }
}