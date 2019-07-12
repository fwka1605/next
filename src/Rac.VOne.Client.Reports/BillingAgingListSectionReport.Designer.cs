namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingAgingListSectionReport の概要の説明です。
    /// </summary>
    partial class BillingAgingListSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BillingAgingListSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCustomer = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblLastMonthReamin = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrentBilling = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrentReceipt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrentMatching = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrentReceiptMatching = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrentRemain = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMonthlyRemain0 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMonthlyRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMonthlyRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMonthlyRemain3 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomer = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerLastMonthRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentBilling1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentReceipt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentReceiptMatching1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerMonthlyRemain0 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerMonthlyRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerMonthlyRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentBilling2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerLastMonthRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentReceiptMatching2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorCurrentReceiptMatching = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.detailBackColor = new GrapeCity.ActiveReports.SectionReportModel.Shape();
            this.txtMonthlyRemain3 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMonthlyRemain2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMonthlyRemain1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMonthlyRemain0 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrentMonthRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrentMonthMatching = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrentMonthReceipt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrentMonthSales = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtLastMonthRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblChildCustomer = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtParentCustomer = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerChildCustomer = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerLastMonthRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMonthSales1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMonthSales2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMonthReceipt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMonthMatching1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMonthMatching2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMonthRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerLastMonthRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerMonthlyRemain0 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerMonthlyRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerMonthlyRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMonthRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLastMonthReamin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentMatching)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentReceiptMatching)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonthlyRemain3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonthlyRemain2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonthlyRemain1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonthlyRemain0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthMatching)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastMonthRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblChildCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCustomer,
            this.lblLastMonthReamin,
            this.lblCurrentBilling,
            this.lblCurrentReceipt,
            this.lblCurrentMatching,
            this.lblCurrentReceiptMatching,
            this.lblCurrentRemain,
            this.lblMonthlyRemain0,
            this.lblMonthlyRemain1,
            this.lblMonthlyRemain2,
            this.lblMonthlyRemain3,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblDepartmentCode,
            this.txtDepartmentCode,
            this.txtDepartmentName,
            this.lblStaffCode,
            this.txtStaffCode,
            this.txtStaffName,
            this.lineHeaderHorUpper,
            this.lineHeaderHorLower,
            this.lineHeaderVerCustomer,
            this.lineHeaderVerLastMonthRemain1,
            this.lineHeaderVerCurrentBilling1,
            this.lineHeaderVerCurrentReceipt,
            this.lineHeaderVerCurrentReceiptMatching1,
            this.lineHeaderVerCurrentRemain1,
            this.lineHeaderVerMonthlyRemain0,
            this.lineHeaderVerMonthlyRemain1,
            this.lineHeaderVerMonthlyRemain2,
            this.lineHeaderVerCurrentBilling2,
            this.lineHeaderVerLastMonthRemain2,
            this.lineHeaderVerCurrentReceiptMatching2,
            this.lineHeaderVerCurrentRemain2,
            this.lineHeaderHorCurrentReceiptMatching});
            this.pageHeader.Height = 1.338583F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCustomer
            // 
            this.lblCustomer.Height = 0.4F;
            this.lblCustomer.HyperLink = null;
            this.lblCustomer.Left = 0F;
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomer.Text = "得意先";
            this.lblCustomer.Top = 0.944882F;
            this.lblCustomer.Width = 1.948032F;
            // 
            // lblLastMonthReamin
            // 
            this.lblLastMonthReamin.Height = 0.4F;
            this.lblLastMonthReamin.HyperLink = null;
            this.lblLastMonthReamin.Left = 1.948032F;
            this.lblLastMonthReamin.Name = "lblLastMonthReamin";
            this.lblLastMonthReamin.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblLastMonthReamin.Text = "前月請求残";
            this.lblLastMonthReamin.Top = 0.944882F;
            this.lblLastMonthReamin.Width = 0.9614173F;
            // 
            // lblCurrentBilling
            // 
            this.lblCurrentBilling.Height = 0.4F;
            this.lblCurrentBilling.HyperLink = null;
            this.lblCurrentBilling.Left = 2.909449F;
            this.lblCurrentBilling.Name = "lblCurrentBilling";
            this.lblCurrentBilling.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrentBilling.Text = "当月売上高";
            this.lblCurrentBilling.Top = 0.944882F;
            this.lblCurrentBilling.Width = 0.9614173F;
            // 
            // lblCurrentReceipt
            // 
            this.lblCurrentReceipt.Height = 0.2F;
            this.lblCurrentReceipt.HyperLink = null;
            this.lblCurrentReceipt.Left = 3.870866F;
            this.lblCurrentReceipt.Name = "lblCurrentReceipt";
            this.lblCurrentReceipt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrentReceipt.Text = "当月入金";
            this.lblCurrentReceipt.Top = 1.144882F;
            this.lblCurrentReceipt.Width = 0.976378F;
            // 
            // lblCurrentMatching
            // 
            this.lblCurrentMatching.Height = 0.2F;
            this.lblCurrentMatching.HyperLink = null;
            this.lblCurrentMatching.Left = 4.851969F;
            this.lblCurrentMatching.Name = "lblCurrentMatching";
            this.lblCurrentMatching.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrentMatching.Text = "当月消込";
            this.lblCurrentMatching.Top = 1.144882F;
            this.lblCurrentMatching.Width = 0.972441F;
            // 
            // lblCurrentReceiptMatching
            // 
            this.lblCurrentReceiptMatching.Height = 0.2F;
            this.lblCurrentReceiptMatching.HyperLink = null;
            this.lblCurrentReceiptMatching.Left = 3.870866F;
            this.lblCurrentReceiptMatching.Name = "lblCurrentReceiptMatching";
            this.lblCurrentReceiptMatching.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrentReceiptMatching.Text = "当月入金";
            this.lblCurrentReceiptMatching.Top = 0.944882F;
            this.lblCurrentReceiptMatching.Width = 1.944882F;
            // 
            // lblCurrentRemain
            // 
            this.lblCurrentRemain.Height = 0.4F;
            this.lblCurrentRemain.HyperLink = null;
            this.lblCurrentRemain.Left = 5.82126F;
            this.lblCurrentRemain.Name = "lblCurrentRemain";
            this.lblCurrentRemain.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrentRemain.Text = "当月請求残";
            this.lblCurrentRemain.Top = 0.944882F;
            this.lblCurrentRemain.Width = 0.9614173F;
            // 
            // lblMonthlyRemain0
            // 
            this.lblMonthlyRemain0.Height = 0.4F;
            this.lblMonthlyRemain0.HyperLink = null;
            this.lblMonthlyRemain0.Left = 6.782677F;
            this.lblMonthlyRemain0.Name = "lblMonthlyRemain0";
            this.lblMonthlyRemain0.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMonthlyRemain0.Text = "WW月発生額";
            this.lblMonthlyRemain0.Top = 0.944882F;
            this.lblMonthlyRemain0.Width = 0.9614173F;
            // 
            // lblMonthlyRemain1
            // 
            this.lblMonthlyRemain1.Height = 0.4F;
            this.lblMonthlyRemain1.HyperLink = null;
            this.lblMonthlyRemain1.Left = 7.744095F;
            this.lblMonthlyRemain1.Name = "lblMonthlyRemain1";
            this.lblMonthlyRemain1.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMonthlyRemain1.Text = "WW月発生額";
            this.lblMonthlyRemain1.Top = 0.944882F;
            this.lblMonthlyRemain1.Width = 0.9614173F;
            // 
            // lblMonthlyRemain2
            // 
            this.lblMonthlyRemain2.Height = 0.4F;
            this.lblMonthlyRemain2.HyperLink = null;
            this.lblMonthlyRemain2.Left = 8.70315F;
            this.lblMonthlyRemain2.Name = "lblMonthlyRemain2";
            this.lblMonthlyRemain2.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMonthlyRemain2.Text = "WW月発生額";
            this.lblMonthlyRemain2.Top = 0.944882F;
            this.lblMonthlyRemain2.Width = 0.9614173F;
            // 
            // lblMonthlyRemain3
            // 
            this.lblMonthlyRemain3.Height = 0.4F;
            this.lblMonthlyRemain3.HyperLink = null;
            this.lblMonthlyRemain3.Left = 9.664566F;
            this.lblMonthlyRemain3.Name = "lblMonthlyRemain3";
            this.lblMonthlyRemain3.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMonthlyRemain3.Text = "WW月発生額";
            this.lblMonthlyRemain3.Top = 0.944882F;
            this.lblMonthlyRemain3.Width = 0.9614173F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCode.Text = "会社コード　：";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 0.8661418F;
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.8661418F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCodeName.Text = "label2";
            this.lblCompanyCodeName.Top = 0F;
            this.lblCompanyCodeName.Width = 3.602669F;
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
            this.lblDate.Width = 0.6397635F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 9.448819F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle; ddo-char-s" +
    "et: 1";
            this.ridate.Top = 0F;
            this.ridate.Width = 1.149607F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "請求残高年齢表";
            this.lblTitle.Top = 0.236F;
            this.lblTitle.Width = 10.63F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.2F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 0F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDepartmentCode.Text = "請求部門コード：";
            this.lblDepartmentCode.Top = 0.472441F;
            this.lblDepartmentCode.Width = 0.8661418F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.DataField = "DepartmentCode";
            this.txtDepartmentCode.Height = 0.2F;
            this.txtDepartmentCode.HyperLink = null;
            this.txtDepartmentCode.Left = 0.8661418F;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtDepartmentCode.Text = "";
            this.txtDepartmentCode.Top = 0.472441F;
            this.txtDepartmentCode.Width = 0.5511811F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.DataField = "DepartmentName";
            this.txtDepartmentName.Height = 0.2F;
            this.txtDepartmentName.HyperLink = null;
            this.txtDepartmentName.Left = 1.417323F;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtDepartmentName.Text = "";
            this.txtDepartmentName.Top = 0.472441F;
            this.txtDepartmentName.Width = 3.051575F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.2F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 0F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblStaffCode.Text = "担当者コード　：";
            this.lblStaffCode.Top = 0.672441F;
            this.lblStaffCode.Width = 0.8661418F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.DataField = "StaffCode";
            this.txtStaffCode.Height = 0.2F;
            this.txtStaffCode.HyperLink = null;
            this.txtStaffCode.Left = 0.8661418F;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtStaffCode.Text = "";
            this.txtStaffCode.Top = 0.672441F;
            this.txtStaffCode.Width = 0.5511811F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.DataField = "StaffName";
            this.txtStaffName.Height = 0.2F;
            this.txtStaffName.HyperLink = null;
            this.txtStaffName.Left = 1.417323F;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtStaffName.Text = "";
            this.txtStaffName.Top = 0.672441F;
            this.txtStaffName.Width = 3.051575F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.944882F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.62992F;
            this.lineHeaderHorUpper.Y1 = 0.944882F;
            this.lineHeaderHorUpper.Y2 = 0.944882F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.338583F;
            this.lineHeaderHorLower.Width = 10.62992F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.62992F;
            this.lineHeaderHorLower.Y1 = 1.338583F;
            this.lineHeaderHorLower.Y2 = 1.338583F;
            // 
            // lineHeaderVerCustomer
            // 
            this.lineHeaderVerCustomer.Height = 0.3937011F;
            this.lineHeaderVerCustomer.Left = 1.948032F;
            this.lineHeaderVerCustomer.LineWeight = 1F;
            this.lineHeaderVerCustomer.Name = "lineHeaderVerCustomer";
            this.lineHeaderVerCustomer.Top = 0.9448819F;
            this.lineHeaderVerCustomer.Width = 0F;
            this.lineHeaderVerCustomer.X1 = 1.948032F;
            this.lineHeaderVerCustomer.X2 = 1.948032F;
            this.lineHeaderVerCustomer.Y1 = 0.9448819F;
            this.lineHeaderVerCustomer.Y2 = 1.338583F;
            // 
            // lineHeaderVerLastMonthRemain1
            // 
            this.lineHeaderVerLastMonthRemain1.Height = 0.3937011F;
            this.lineHeaderVerLastMonthRemain1.Left = 2.909449F;
            this.lineHeaderVerLastMonthRemain1.LineWeight = 1F;
            this.lineHeaderVerLastMonthRemain1.Name = "lineHeaderVerLastMonthRemain1";
            this.lineHeaderVerLastMonthRemain1.Top = 0.9448819F;
            this.lineHeaderVerLastMonthRemain1.Width = 0F;
            this.lineHeaderVerLastMonthRemain1.X1 = 2.909449F;
            this.lineHeaderVerLastMonthRemain1.X2 = 2.909449F;
            this.lineHeaderVerLastMonthRemain1.Y1 = 0.9448819F;
            this.lineHeaderVerLastMonthRemain1.Y2 = 1.338583F;
            // 
            // lineHeaderVerCurrentBilling1
            // 
            this.lineHeaderVerCurrentBilling1.Height = 0.3937011F;
            this.lineHeaderVerCurrentBilling1.Left = 3.870866F;
            this.lineHeaderVerCurrentBilling1.LineWeight = 1F;
            this.lineHeaderVerCurrentBilling1.Name = "lineHeaderVerCurrentBilling1";
            this.lineHeaderVerCurrentBilling1.Top = 0.9448819F;
            this.lineHeaderVerCurrentBilling1.Width = 0F;
            this.lineHeaderVerCurrentBilling1.X1 = 3.870866F;
            this.lineHeaderVerCurrentBilling1.X2 = 3.870866F;
            this.lineHeaderVerCurrentBilling1.Y1 = 0.9448819F;
            this.lineHeaderVerCurrentBilling1.Y2 = 1.338583F;
            // 
            // lineHeaderVerCurrentReceipt
            // 
            this.lineHeaderVerCurrentReceipt.Height = 0.193701F;
            this.lineHeaderVerCurrentReceipt.Left = 4.851969F;
            this.lineHeaderVerCurrentReceipt.LineWeight = 1F;
            this.lineHeaderVerCurrentReceipt.Name = "lineHeaderVerCurrentReceipt";
            this.lineHeaderVerCurrentReceipt.Top = 1.144882F;
            this.lineHeaderVerCurrentReceipt.Width = 0F;
            this.lineHeaderVerCurrentReceipt.X1 = 4.851969F;
            this.lineHeaderVerCurrentReceipt.X2 = 4.851969F;
            this.lineHeaderVerCurrentReceipt.Y1 = 1.144882F;
            this.lineHeaderVerCurrentReceipt.Y2 = 1.338583F;
            // 
            // lineHeaderVerCurrentReceiptMatching1
            // 
            this.lineHeaderVerCurrentReceiptMatching1.Height = 0.393701F;
            this.lineHeaderVerCurrentReceiptMatching1.Left = 5.797638F;
            this.lineHeaderVerCurrentReceiptMatching1.LineWeight = 1F;
            this.lineHeaderVerCurrentReceiptMatching1.Name = "lineHeaderVerCurrentReceiptMatching1";
            this.lineHeaderVerCurrentReceiptMatching1.Top = 0.944882F;
            this.lineHeaderVerCurrentReceiptMatching1.Width = 0F;
            this.lineHeaderVerCurrentReceiptMatching1.X1 = 5.797638F;
            this.lineHeaderVerCurrentReceiptMatching1.X2 = 5.797638F;
            this.lineHeaderVerCurrentReceiptMatching1.Y1 = 0.944882F;
            this.lineHeaderVerCurrentReceiptMatching1.Y2 = 1.338583F;
            // 
            // lineHeaderVerCurrentRemain1
            // 
            this.lineHeaderVerCurrentRemain1.Height = 0.3937011F;
            this.lineHeaderVerCurrentRemain1.Left = 6.782677F;
            this.lineHeaderVerCurrentRemain1.LineWeight = 1F;
            this.lineHeaderVerCurrentRemain1.Name = "lineHeaderVerCurrentRemain1";
            this.lineHeaderVerCurrentRemain1.Top = 0.9448819F;
            this.lineHeaderVerCurrentRemain1.Width = 0F;
            this.lineHeaderVerCurrentRemain1.X1 = 6.782677F;
            this.lineHeaderVerCurrentRemain1.X2 = 6.782677F;
            this.lineHeaderVerCurrentRemain1.Y1 = 0.9448819F;
            this.lineHeaderVerCurrentRemain1.Y2 = 1.338583F;
            // 
            // lineHeaderVerMonthlyRemain0
            // 
            this.lineHeaderVerMonthlyRemain0.Height = 0.393701F;
            this.lineHeaderVerMonthlyRemain0.Left = 7.744095F;
            this.lineHeaderVerMonthlyRemain0.LineWeight = 1F;
            this.lineHeaderVerMonthlyRemain0.Name = "lineHeaderVerMonthlyRemain0";
            this.lineHeaderVerMonthlyRemain0.Top = 0.944882F;
            this.lineHeaderVerMonthlyRemain0.Width = 0F;
            this.lineHeaderVerMonthlyRemain0.X1 = 7.744095F;
            this.lineHeaderVerMonthlyRemain0.X2 = 7.744095F;
            this.lineHeaderVerMonthlyRemain0.Y1 = 0.944882F;
            this.lineHeaderVerMonthlyRemain0.Y2 = 1.338583F;
            // 
            // lineHeaderVerMonthlyRemain1
            // 
            this.lineHeaderVerMonthlyRemain1.Height = 0.3937011F;
            this.lineHeaderVerMonthlyRemain1.Left = 8.70315F;
            this.lineHeaderVerMonthlyRemain1.LineWeight = 1F;
            this.lineHeaderVerMonthlyRemain1.Name = "lineHeaderVerMonthlyRemain1";
            this.lineHeaderVerMonthlyRemain1.Top = 0.9448819F;
            this.lineHeaderVerMonthlyRemain1.Width = 0F;
            this.lineHeaderVerMonthlyRemain1.X1 = 8.70315F;
            this.lineHeaderVerMonthlyRemain1.X2 = 8.70315F;
            this.lineHeaderVerMonthlyRemain1.Y1 = 0.9448819F;
            this.lineHeaderVerMonthlyRemain1.Y2 = 1.338583F;
            // 
            // lineHeaderVerMonthlyRemain2
            // 
            this.lineHeaderVerMonthlyRemain2.Height = 0.393701F;
            this.lineHeaderVerMonthlyRemain2.Left = 9.664566F;
            this.lineHeaderVerMonthlyRemain2.LineWeight = 1F;
            this.lineHeaderVerMonthlyRemain2.Name = "lineHeaderVerMonthlyRemain2";
            this.lineHeaderVerMonthlyRemain2.Top = 0.944882F;
            this.lineHeaderVerMonthlyRemain2.Width = 0F;
            this.lineHeaderVerMonthlyRemain2.X1 = 9.664566F;
            this.lineHeaderVerMonthlyRemain2.X2 = 9.664566F;
            this.lineHeaderVerMonthlyRemain2.Y1 = 0.944882F;
            this.lineHeaderVerMonthlyRemain2.Y2 = 1.338583F;
            // 
            // lineHeaderVerCurrentBilling2
            // 
            this.lineHeaderVerCurrentBilling2.Height = 0.3937011F;
            this.lineHeaderVerCurrentBilling2.Left = 3.890945F;
            this.lineHeaderVerCurrentBilling2.LineWeight = 1F;
            this.lineHeaderVerCurrentBilling2.Name = "lineHeaderVerCurrentBilling2";
            this.lineHeaderVerCurrentBilling2.Top = 0.9448819F;
            this.lineHeaderVerCurrentBilling2.Width = 0F;
            this.lineHeaderVerCurrentBilling2.X1 = 3.890945F;
            this.lineHeaderVerCurrentBilling2.X2 = 3.890945F;
            this.lineHeaderVerCurrentBilling2.Y1 = 0.9448819F;
            this.lineHeaderVerCurrentBilling2.Y2 = 1.338583F;
            // 
            // lineHeaderVerLastMonthRemain2
            // 
            this.lineHeaderVerLastMonthRemain2.Height = 0.3937011F;
            this.lineHeaderVerLastMonthRemain2.Left = 2.925984F;
            this.lineHeaderVerLastMonthRemain2.LineWeight = 1F;
            this.lineHeaderVerLastMonthRemain2.Name = "lineHeaderVerLastMonthRemain2";
            this.lineHeaderVerLastMonthRemain2.Top = 0.9448819F;
            this.lineHeaderVerLastMonthRemain2.Width = 0F;
            this.lineHeaderVerLastMonthRemain2.X1 = 2.925984F;
            this.lineHeaderVerLastMonthRemain2.X2 = 2.925984F;
            this.lineHeaderVerLastMonthRemain2.Y1 = 0.9448819F;
            this.lineHeaderVerLastMonthRemain2.Y2 = 1.338583F;
            // 
            // lineHeaderVerCurrentReceiptMatching2
            // 
            this.lineHeaderVerCurrentReceiptMatching2.Height = 0.393701F;
            this.lineHeaderVerCurrentReceiptMatching2.Left = 5.826772F;
            this.lineHeaderVerCurrentReceiptMatching2.LineWeight = 1F;
            this.lineHeaderVerCurrentReceiptMatching2.Name = "lineHeaderVerCurrentReceiptMatching2";
            this.lineHeaderVerCurrentReceiptMatching2.Top = 0.944882F;
            this.lineHeaderVerCurrentReceiptMatching2.Width = 0F;
            this.lineHeaderVerCurrentReceiptMatching2.X1 = 5.826772F;
            this.lineHeaderVerCurrentReceiptMatching2.X2 = 5.826772F;
            this.lineHeaderVerCurrentReceiptMatching2.Y1 = 0.944882F;
            this.lineHeaderVerCurrentReceiptMatching2.Y2 = 1.338583F;
            // 
            // lineHeaderVerCurrentRemain2
            // 
            this.lineHeaderVerCurrentRemain2.Height = 0.3937011F;
            this.lineHeaderVerCurrentRemain2.Left = 6.799213F;
            this.lineHeaderVerCurrentRemain2.LineWeight = 1F;
            this.lineHeaderVerCurrentRemain2.Name = "lineHeaderVerCurrentRemain2";
            this.lineHeaderVerCurrentRemain2.Top = 0.9448819F;
            this.lineHeaderVerCurrentRemain2.Width = 0F;
            this.lineHeaderVerCurrentRemain2.X1 = 6.799213F;
            this.lineHeaderVerCurrentRemain2.X2 = 6.799213F;
            this.lineHeaderVerCurrentRemain2.Y1 = 0.9448819F;
            this.lineHeaderVerCurrentRemain2.Y2 = 1.338583F;
            // 
            // lineHeaderHorCurrentReceiptMatching
            // 
            this.lineHeaderHorCurrentReceiptMatching.Height = 0F;
            this.lineHeaderHorCurrentReceiptMatching.Left = 3.890945F;
            this.lineHeaderHorCurrentReceiptMatching.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorCurrentReceiptMatching.LineWeight = 0.5F;
            this.lineHeaderHorCurrentReceiptMatching.Name = "lineHeaderHorCurrentReceiptMatching";
            this.lineHeaderHorCurrentReceiptMatching.Top = 1.144882F;
            this.lineHeaderHorCurrentReceiptMatching.Width = 1.902756F;
            this.lineHeaderHorCurrentReceiptMatching.X1 = 5.793701F;
            this.lineHeaderHorCurrentReceiptMatching.X2 = 3.890945F;
            this.lineHeaderHorCurrentReceiptMatching.Y1 = 1.144882F;
            this.lineHeaderHorCurrentReceiptMatching.Y2 = 1.144882F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.detailBackColor,
            this.txtMonthlyRemain3,
            this.txtMonthlyRemain2,
            this.txtMonthlyRemain1,
            this.txtMonthlyRemain0,
            this.txtCurrentMonthRemain,
            this.txtCurrentMonthMatching,
            this.txtCurrentMonthReceipt,
            this.txtCurrentMonthSales,
            this.txtLastMonthRemain,
            this.lblChildCustomer,
            this.txtParentCustomer,
            this.lineDetailHorLower,
            this.lineDetailVerChildCustomer,
            this.lineDetailVerLastMonthRemain2,
            this.lineDetailVerCurrentMonthSales1,
            this.lineDetailVerCurrentMonthSales2,
            this.lineDetailVerCurrentMonthReceipt,
            this.lineDetailVerCurrentMonthMatching1,
            this.lineDetailVerCurrentMonthMatching2,
            this.lineDetailVerCurrentMonthRemain1,
            this.lineDetailVerLastMonthRemain1,
            this.lineDetailVerMonthlyRemain0,
            this.lineDetailVerMonthlyRemain1,
            this.lineDetailVerMonthlyRemain2,
            this.lineDetailVerCurrentMonthRemain2});
            this.detail.Height = 0.2F;
            this.detail.Name = "detail";
            this.detail.Format += new System.EventHandler(this.detail_Format);
            // 
            // detailBackColor
            // 
            this.detailBackColor.Height = 0.2F;
            this.detailBackColor.Left = 0F;
            this.detailBackColor.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Transparent;
            this.detailBackColor.LineWeight = 0F;
            this.detailBackColor.Name = "detailBackColor";
            this.detailBackColor.RoundingRadius = new GrapeCity.ActiveReports.Controls.CornersRadius(10F, null, null, null, null);
            this.detailBackColor.Top = 0F;
            this.detailBackColor.Width = 10.62992F;
            // 
            // txtMonthlyRemain3
            // 
            this.txtMonthlyRemain3.DataField = "MonthlyRemain3";
            this.txtMonthlyRemain3.Height = 0.2F;
            this.txtMonthlyRemain3.Left = 9.669292F;
            this.txtMonthlyRemain3.MultiLine = false;
            this.txtMonthlyRemain3.Name = "txtMonthlyRemain3";
            this.txtMonthlyRemain3.OutputFormat = resources.GetString("txtMonthlyRemain3.OutputFormat");
            this.txtMonthlyRemain3.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMonthlyRemain3.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMonthlyRemain3.Text = null;
            this.txtMonthlyRemain3.Top = 0F;
            this.txtMonthlyRemain3.Width = 0.9614173F;
            // 
            // txtMonthlyRemain2
            // 
            this.txtMonthlyRemain2.DataField = "MonthlyRemain2";
            this.txtMonthlyRemain2.Height = 0.2F;
            this.txtMonthlyRemain2.Left = 8.712599F;
            this.txtMonthlyRemain2.MultiLine = false;
            this.txtMonthlyRemain2.Name = "txtMonthlyRemain2";
            this.txtMonthlyRemain2.OutputFormat = resources.GetString("txtMonthlyRemain2.OutputFormat");
            this.txtMonthlyRemain2.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMonthlyRemain2.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMonthlyRemain2.Text = null;
            this.txtMonthlyRemain2.Top = 0F;
            this.txtMonthlyRemain2.Width = 0.9614173F;
            // 
            // txtMonthlyRemain1
            // 
            this.txtMonthlyRemain1.DataField = "MonthlyRemain1";
            this.txtMonthlyRemain1.Height = 0.2F;
            this.txtMonthlyRemain1.Left = 7.748032F;
            this.txtMonthlyRemain1.MultiLine = false;
            this.txtMonthlyRemain1.Name = "txtMonthlyRemain1";
            this.txtMonthlyRemain1.OutputFormat = resources.GetString("txtMonthlyRemain1.OutputFormat");
            this.txtMonthlyRemain1.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMonthlyRemain1.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMonthlyRemain1.Text = null;
            this.txtMonthlyRemain1.Top = 0F;
            this.txtMonthlyRemain1.Width = 0.9614173F;
            // 
            // txtMonthlyRemain0
            // 
            this.txtMonthlyRemain0.DataField = "MonthlyRemain0";
            this.txtMonthlyRemain0.Height = 0.2F;
            this.txtMonthlyRemain0.Left = 6.779528F;
            this.txtMonthlyRemain0.MultiLine = false;
            this.txtMonthlyRemain0.Name = "txtMonthlyRemain0";
            this.txtMonthlyRemain0.OutputFormat = resources.GetString("txtMonthlyRemain0.OutputFormat");
            this.txtMonthlyRemain0.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMonthlyRemain0.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMonthlyRemain0.Text = null;
            this.txtMonthlyRemain0.Top = 0F;
            this.txtMonthlyRemain0.Width = 0.9614173F;
            // 
            // txtCurrentMonthRemain
            // 
            this.txtCurrentMonthRemain.DataField = "CurrentMonthRemain";
            this.txtCurrentMonthRemain.Height = 0.2F;
            this.txtCurrentMonthRemain.Left = 5.826772F;
            this.txtCurrentMonthRemain.MultiLine = false;
            this.txtCurrentMonthRemain.Name = "txtCurrentMonthRemain";
            this.txtCurrentMonthRemain.OutputFormat = resources.GetString("txtCurrentMonthRemain.OutputFormat");
            this.txtCurrentMonthRemain.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCurrentMonthRemain.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrentMonthRemain.Text = null;
            this.txtCurrentMonthRemain.Top = 0F;
            this.txtCurrentMonthRemain.Width = 0.9614173F;
            // 
            // txtCurrentMonthMatching
            // 
            this.txtCurrentMonthMatching.DataField = "CurrentMonthMatching";
            this.txtCurrentMonthMatching.Height = 0.2F;
            this.txtCurrentMonthMatching.Left = 4.855906F;
            this.txtCurrentMonthMatching.MultiLine = false;
            this.txtCurrentMonthMatching.Name = "txtCurrentMonthMatching";
            this.txtCurrentMonthMatching.OutputFormat = resources.GetString("txtCurrentMonthMatching.OutputFormat");
            this.txtCurrentMonthMatching.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCurrentMonthMatching.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrentMonthMatching.Text = null;
            this.txtCurrentMonthMatching.Top = 0F;
            this.txtCurrentMonthMatching.Width = 0.9448819F;
            // 
            // txtCurrentMonthReceipt
            // 
            this.txtCurrentMonthReceipt.DataField = "CurrentMonthReceipt";
            this.txtCurrentMonthReceipt.Height = 0.2F;
            this.txtCurrentMonthReceipt.Left = 3.870866F;
            this.txtCurrentMonthReceipt.MultiLine = false;
            this.txtCurrentMonthReceipt.Name = "txtCurrentMonthReceipt";
            this.txtCurrentMonthReceipt.OutputFormat = resources.GetString("txtCurrentMonthReceipt.OutputFormat");
            this.txtCurrentMonthReceipt.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCurrentMonthReceipt.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrentMonthReceipt.Text = null;
            this.txtCurrentMonthReceipt.Top = 0F;
            this.txtCurrentMonthReceipt.Width = 0.9744094F;
            // 
            // txtCurrentMonthSales
            // 
            this.txtCurrentMonthSales.DataField = "CurrentMonthSales";
            this.txtCurrentMonthSales.Height = 0.2F;
            this.txtCurrentMonthSales.Left = 2.909449F;
            this.txtCurrentMonthSales.MultiLine = false;
            this.txtCurrentMonthSales.Name = "txtCurrentMonthSales";
            this.txtCurrentMonthSales.OutputFormat = resources.GetString("txtCurrentMonthSales.OutputFormat");
            this.txtCurrentMonthSales.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCurrentMonthSales.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrentMonthSales.Text = null;
            this.txtCurrentMonthSales.Top = 0F;
            this.txtCurrentMonthSales.Width = 0.9614173F;
            // 
            // txtLastMonthRemain
            // 
            this.txtLastMonthRemain.DataField = "LastMonthRemain";
            this.txtLastMonthRemain.Height = 0.2F;
            this.txtLastMonthRemain.Left = 1.948032F;
            this.txtLastMonthRemain.MultiLine = false;
            this.txtLastMonthRemain.Name = "txtLastMonthRemain";
            this.txtLastMonthRemain.OutputFormat = resources.GetString("txtLastMonthRemain.OutputFormat");
            this.txtLastMonthRemain.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtLastMonthRemain.Style = "font-size: 7.5pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtLastMonthRemain.Text = null;
            this.txtLastMonthRemain.Top = 0F;
            this.txtLastMonthRemain.Width = 0.9614173F;
            // 
            // lblChildCustomer
            // 
            this.lblChildCustomer.Height = 0.2F;
            this.lblChildCustomer.HyperLink = null;
            this.lblChildCustomer.Left = 1.681103F;
            this.lblChildCustomer.Name = "lblChildCustomer";
            this.lblChildCustomer.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblChildCustomer.Text = "";
            this.lblChildCustomer.Top = 0F;
            this.lblChildCustomer.Width = 0.267F;
            // 
            // txtParentCustomer
            // 
            this.txtParentCustomer.Height = 0.2F;
            this.txtParentCustomer.HyperLink = null;
            this.txtParentCustomer.Left = 0F;
            this.txtParentCustomer.Name = "txtParentCustomer";
            this.txtParentCustomer.Style = "color: Black; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtParentCustomer.Text = "";
            this.txtParentCustomer.Top = 0F;
            this.txtParentCustomer.Width = 1.681103F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.2F;
            this.lineDetailHorLower.Width = 10.62992F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.62992F;
            this.lineDetailHorLower.Y1 = 0.2F;
            this.lineDetailHorLower.Y2 = 0.2F;
            // 
            // lineDetailVerChildCustomer
            // 
            this.lineDetailVerChildCustomer.Height = 0.2F;
            this.lineDetailVerChildCustomer.Left = 1.948032F;
            this.lineDetailVerChildCustomer.LineWeight = 1F;
            this.lineDetailVerChildCustomer.Name = "lineDetailVerChildCustomer";
            this.lineDetailVerChildCustomer.Top = 0F;
            this.lineDetailVerChildCustomer.Width = 0F;
            this.lineDetailVerChildCustomer.X1 = 1.948032F;
            this.lineDetailVerChildCustomer.X2 = 1.948032F;
            this.lineDetailVerChildCustomer.Y1 = 0F;
            this.lineDetailVerChildCustomer.Y2 = 0.2F;
            // 
            // lineDetailVerLastMonthRemain2
            // 
            this.lineDetailVerLastMonthRemain2.Height = 0.2F;
            this.lineDetailVerLastMonthRemain2.Left = 2.925984F;
            this.lineDetailVerLastMonthRemain2.LineWeight = 1F;
            this.lineDetailVerLastMonthRemain2.Name = "lineDetailVerLastMonthRemain2";
            this.lineDetailVerLastMonthRemain2.Top = 0F;
            this.lineDetailVerLastMonthRemain2.Width = 0F;
            this.lineDetailVerLastMonthRemain2.X1 = 2.925984F;
            this.lineDetailVerLastMonthRemain2.X2 = 2.925984F;
            this.lineDetailVerLastMonthRemain2.Y1 = 0F;
            this.lineDetailVerLastMonthRemain2.Y2 = 0.2F;
            // 
            // lineDetailVerCurrentMonthSales1
            // 
            this.lineDetailVerCurrentMonthSales1.Height = 0.2F;
            this.lineDetailVerCurrentMonthSales1.Left = 3.870866F;
            this.lineDetailVerCurrentMonthSales1.LineWeight = 1F;
            this.lineDetailVerCurrentMonthSales1.Name = "lineDetailVerCurrentMonthSales1";
            this.lineDetailVerCurrentMonthSales1.Top = 0F;
            this.lineDetailVerCurrentMonthSales1.Width = 0F;
            this.lineDetailVerCurrentMonthSales1.X1 = 3.870866F;
            this.lineDetailVerCurrentMonthSales1.X2 = 3.870866F;
            this.lineDetailVerCurrentMonthSales1.Y1 = 0F;
            this.lineDetailVerCurrentMonthSales1.Y2 = 0.2F;
            // 
            // lineDetailVerCurrentMonthSales2
            // 
            this.lineDetailVerCurrentMonthSales2.Height = 0.2F;
            this.lineDetailVerCurrentMonthSales2.Left = 3.890945F;
            this.lineDetailVerCurrentMonthSales2.LineWeight = 1F;
            this.lineDetailVerCurrentMonthSales2.Name = "lineDetailVerCurrentMonthSales2";
            this.lineDetailVerCurrentMonthSales2.Top = 0F;
            this.lineDetailVerCurrentMonthSales2.Width = 0F;
            this.lineDetailVerCurrentMonthSales2.X1 = 3.890945F;
            this.lineDetailVerCurrentMonthSales2.X2 = 3.890945F;
            this.lineDetailVerCurrentMonthSales2.Y1 = 0F;
            this.lineDetailVerCurrentMonthSales2.Y2 = 0.2F;
            // 
            // lineDetailVerCurrentMonthReceipt
            // 
            this.lineDetailVerCurrentMonthReceipt.Height = 0.2F;
            this.lineDetailVerCurrentMonthReceipt.Left = 4.851969F;
            this.lineDetailVerCurrentMonthReceipt.LineWeight = 1F;
            this.lineDetailVerCurrentMonthReceipt.Name = "lineDetailVerCurrentMonthReceipt";
            this.lineDetailVerCurrentMonthReceipt.Top = 0F;
            this.lineDetailVerCurrentMonthReceipt.Width = 0F;
            this.lineDetailVerCurrentMonthReceipt.X1 = 4.851969F;
            this.lineDetailVerCurrentMonthReceipt.X2 = 4.851969F;
            this.lineDetailVerCurrentMonthReceipt.Y1 = 0F;
            this.lineDetailVerCurrentMonthReceipt.Y2 = 0.2F;
            // 
            // lineDetailVerCurrentMonthMatching1
            // 
            this.lineDetailVerCurrentMonthMatching1.Height = 0.2F;
            this.lineDetailVerCurrentMonthMatching1.Left = 5.797638F;
            this.lineDetailVerCurrentMonthMatching1.LineWeight = 1F;
            this.lineDetailVerCurrentMonthMatching1.Name = "lineDetailVerCurrentMonthMatching1";
            this.lineDetailVerCurrentMonthMatching1.Top = 0F;
            this.lineDetailVerCurrentMonthMatching1.Width = 0F;
            this.lineDetailVerCurrentMonthMatching1.X1 = 5.797638F;
            this.lineDetailVerCurrentMonthMatching1.X2 = 5.797638F;
            this.lineDetailVerCurrentMonthMatching1.Y1 = 0F;
            this.lineDetailVerCurrentMonthMatching1.Y2 = 0.2F;
            // 
            // lineDetailVerCurrentMonthMatching2
            // 
            this.lineDetailVerCurrentMonthMatching2.Height = 0.2F;
            this.lineDetailVerCurrentMonthMatching2.Left = 5.826772F;
            this.lineDetailVerCurrentMonthMatching2.LineWeight = 1F;
            this.lineDetailVerCurrentMonthMatching2.Name = "lineDetailVerCurrentMonthMatching2";
            this.lineDetailVerCurrentMonthMatching2.Top = 0F;
            this.lineDetailVerCurrentMonthMatching2.Width = 0F;
            this.lineDetailVerCurrentMonthMatching2.X1 = 5.826772F;
            this.lineDetailVerCurrentMonthMatching2.X2 = 5.826772F;
            this.lineDetailVerCurrentMonthMatching2.Y1 = 0F;
            this.lineDetailVerCurrentMonthMatching2.Y2 = 0.2F;
            // 
            // lineDetailVerCurrentMonthRemain1
            // 
            this.lineDetailVerCurrentMonthRemain1.Height = 0.2F;
            this.lineDetailVerCurrentMonthRemain1.Left = 6.782677F;
            this.lineDetailVerCurrentMonthRemain1.LineWeight = 1F;
            this.lineDetailVerCurrentMonthRemain1.Name = "lineDetailVerCurrentMonthRemain1";
            this.lineDetailVerCurrentMonthRemain1.Top = 0F;
            this.lineDetailVerCurrentMonthRemain1.Width = 0F;
            this.lineDetailVerCurrentMonthRemain1.X1 = 6.782677F;
            this.lineDetailVerCurrentMonthRemain1.X2 = 6.782677F;
            this.lineDetailVerCurrentMonthRemain1.Y1 = 0F;
            this.lineDetailVerCurrentMonthRemain1.Y2 = 0.2F;
            // 
            // lineDetailVerLastMonthRemain1
            // 
            this.lineDetailVerLastMonthRemain1.Height = 0.2F;
            this.lineDetailVerLastMonthRemain1.Left = 2.909449F;
            this.lineDetailVerLastMonthRemain1.LineWeight = 1F;
            this.lineDetailVerLastMonthRemain1.Name = "lineDetailVerLastMonthRemain1";
            this.lineDetailVerLastMonthRemain1.Top = 0F;
            this.lineDetailVerLastMonthRemain1.Width = 0F;
            this.lineDetailVerLastMonthRemain1.X1 = 2.909449F;
            this.lineDetailVerLastMonthRemain1.X2 = 2.909449F;
            this.lineDetailVerLastMonthRemain1.Y1 = 0F;
            this.lineDetailVerLastMonthRemain1.Y2 = 0.2F;
            // 
            // lineDetailVerMonthlyRemain0
            // 
            this.lineDetailVerMonthlyRemain0.Height = 0.2F;
            this.lineDetailVerMonthlyRemain0.Left = 7.744095F;
            this.lineDetailVerMonthlyRemain0.LineWeight = 1F;
            this.lineDetailVerMonthlyRemain0.Name = "lineDetailVerMonthlyRemain0";
            this.lineDetailVerMonthlyRemain0.Top = 0F;
            this.lineDetailVerMonthlyRemain0.Width = 0F;
            this.lineDetailVerMonthlyRemain0.X1 = 7.744095F;
            this.lineDetailVerMonthlyRemain0.X2 = 7.744095F;
            this.lineDetailVerMonthlyRemain0.Y1 = 0F;
            this.lineDetailVerMonthlyRemain0.Y2 = 0.2F;
            // 
            // lineDetailVerMonthlyRemain1
            // 
            this.lineDetailVerMonthlyRemain1.Height = 0.2F;
            this.lineDetailVerMonthlyRemain1.Left = 8.70315F;
            this.lineDetailVerMonthlyRemain1.LineWeight = 1F;
            this.lineDetailVerMonthlyRemain1.Name = "lineDetailVerMonthlyRemain1";
            this.lineDetailVerMonthlyRemain1.Top = 0F;
            this.lineDetailVerMonthlyRemain1.Width = 0F;
            this.lineDetailVerMonthlyRemain1.X1 = 8.70315F;
            this.lineDetailVerMonthlyRemain1.X2 = 8.70315F;
            this.lineDetailVerMonthlyRemain1.Y1 = 0F;
            this.lineDetailVerMonthlyRemain1.Y2 = 0.2F;
            // 
            // lineDetailVerMonthlyRemain2
            // 
            this.lineDetailVerMonthlyRemain2.Height = 0.2F;
            this.lineDetailVerMonthlyRemain2.Left = 9.664566F;
            this.lineDetailVerMonthlyRemain2.LineWeight = 1F;
            this.lineDetailVerMonthlyRemain2.Name = "lineDetailVerMonthlyRemain2";
            this.lineDetailVerMonthlyRemain2.Top = 0F;
            this.lineDetailVerMonthlyRemain2.Width = 0F;
            this.lineDetailVerMonthlyRemain2.X1 = 9.664566F;
            this.lineDetailVerMonthlyRemain2.X2 = 9.664566F;
            this.lineDetailVerMonthlyRemain2.Y1 = 0F;
            this.lineDetailVerMonthlyRemain2.Y2 = 0.2F;
            // 
            // lineDetailVerCurrentMonthRemain2
            // 
            this.lineDetailVerCurrentMonthRemain2.Height = 0.2F;
            this.lineDetailVerCurrentMonthRemain2.Left = 6.799213F;
            this.lineDetailVerCurrentMonthRemain2.LineWeight = 1F;
            this.lineDetailVerCurrentMonthRemain2.Name = "lineDetailVerCurrentMonthRemain2";
            this.lineDetailVerCurrentMonthRemain2.Top = 0F;
            this.lineDetailVerCurrentMonthRemain2.Width = 0F;
            this.lineDetailVerCurrentMonthRemain2.X1 = 6.799213F;
            this.lineDetailVerCurrentMonthRemain2.X2 = 6.799213F;
            this.lineDetailVerCurrentMonthRemain2.Y1 = 0F;
            this.lineDetailVerCurrentMonthRemain2.Y2 = 0.2F;
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
            this.reportInfo1.Left = 7.007874F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.07874016F;
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
            // BillingAgingListSectionReport
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
            "t-size: 9pt; font-family: \"ＭＳ 明朝\"; ddo-char-set: 186", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLastMonthReamin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentMatching)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentReceiptMatching)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonthlyRemain3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonthlyRemain2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonthlyRemain1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonthlyRemain0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthMatching)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastMonthRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblChildCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblLastMonthReamin;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrentBilling;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrentReceiptMatching;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrentReceipt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrentMatching;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrentRemain;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblMonthlyRemain0;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblMonthlyRemain1;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblMonthlyRemain2;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblMonthlyRemain3;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerLastMonthRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentBilling1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentReceipt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentReceiptMatching1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerMonthlyRemain0;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerMonthlyRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerMonthlyRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtParentCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMonthMatching2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerChildCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMonthSales1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMonthSales2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMonthReceipt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerLastMonthRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMonthlyRemain0;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMonthlyRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMonthlyRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentBilling2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerLastMonthRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentReceiptMatching2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorCurrentReceiptMatching;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerLastMonthRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMonthMatching1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMonthRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLastMonthRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrentMonthSales;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrentMonthReceipt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrentMonthMatching;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrentMonthRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMonthlyRemain0;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMonthlyRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMonthlyRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMonthlyRemain3;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMonthRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblChildCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Shape detailBackColor;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
    }
}
