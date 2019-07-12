namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingAgingListSectionReport の概要の説明です。
    /// </summary>
    partial class BillingAgingListSectionReport1
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BillingAgingListSectionReport1));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblMonthlyRemain3 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMonthlyRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMonthlyRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMonthlyRemain0 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrentRemain = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrentMatching = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrentBilling = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblLastMonthRemain = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomer = new GrapeCity.ActiveReports.SectionReportModel.Label();
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
            this.lineHeaderVerCustomer = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerLastMonthRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerLastMonthRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentBilling1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentBilling2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentMatching1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentMatching2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrentRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerMonthlyRemain0 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerMonthlyRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerMonthlyRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.detailBackColor = new GrapeCity.ActiveReports.SectionReportModel.Shape();
            this.txtMonthlyRemain3 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMonthlyRemain2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMonthlyRemain1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMonthlyRemain0 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrentMonthRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrentMonthMatching = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrentMonthSales = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtLastMonthRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtChildCustomer = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtParentCustomer = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomer = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerLastMonthRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerLastMonthRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentBilling1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentBilling2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMatching1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentMatching2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrentRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerMonthlyRemain0 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerMonthlyRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerMonthlyRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentMatching)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLastMonthRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomer)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastMonthRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChildCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblMonthlyRemain3,
            this.lblMonthlyRemain2,
            this.lblMonthlyRemain1,
            this.lblMonthlyRemain0,
            this.lblCurrentRemain,
            this.lblCurrentMatching,
            this.lblCurrentBilling,
            this.lblLastMonthRemain,
            this.lblCustomer,
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
            this.lineHeaderVerCustomer,
            this.lineHeaderHorUpper,
            this.lineHeaderHorLower,
            this.lineHeaderVerLastMonthRemain1,
            this.lineHeaderVerLastMonthRemain2,
            this.lineHeaderVerCurrentBilling1,
            this.lineHeaderVerCurrentBilling2,
            this.lineHeaderVerCurrentMatching1,
            this.lineHeaderVerCurrentMatching2,
            this.lineHeaderVerCurrentRemain1,
            this.lineHeaderVerCurrentRemain2,
            this.lineHeaderVerMonthlyRemain0,
            this.lineHeaderVerMonthlyRemain1,
            this.lineHeaderVerMonthlyRemain2});
            this.pageHeader.Height = 1.223622F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblMonthlyRemain3
            // 
            this.lblMonthlyRemain3.Height = 0.2F;
            this.lblMonthlyRemain3.HyperLink = null;
            this.lblMonthlyRemain3.Left = 9.612206F;
            this.lblMonthlyRemain3.Name = "lblMonthlyRemain3";
            this.lblMonthlyRemain3.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMonthlyRemain3.Text = "WW月発生額";
            this.lblMonthlyRemain3.Top = 1.023622F;
            this.lblMonthlyRemain3.Width = 1.019685F;
            // 
            // lblMonthlyRemain2
            // 
            this.lblMonthlyRemain2.Height = 0.2F;
            this.lblMonthlyRemain2.HyperLink = null;
            this.lblMonthlyRemain2.Left = 8.600394F;
            this.lblMonthlyRemain2.Name = "lblMonthlyRemain2";
            this.lblMonthlyRemain2.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMonthlyRemain2.Text = "WW月発生額";
            this.lblMonthlyRemain2.Top = 1.023622F;
            this.lblMonthlyRemain2.Width = 1.011811F;
            // 
            // lblMonthlyRemain1
            // 
            this.lblMonthlyRemain1.Height = 0.2F;
            this.lblMonthlyRemain1.HyperLink = null;
            this.lblMonthlyRemain1.Left = 7.588583F;
            this.lblMonthlyRemain1.Name = "lblMonthlyRemain1";
            this.lblMonthlyRemain1.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMonthlyRemain1.Text = "WW月発生額";
            this.lblMonthlyRemain1.Top = 1.023622F;
            this.lblMonthlyRemain1.Width = 1.011811F;
            // 
            // lblMonthlyRemain0
            // 
            this.lblMonthlyRemain0.Height = 0.2F;
            this.lblMonthlyRemain0.HyperLink = null;
            this.lblMonthlyRemain0.Left = 6.580709F;
            this.lblMonthlyRemain0.Name = "lblMonthlyRemain0";
            this.lblMonthlyRemain0.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMonthlyRemain0.Text = "WW月発生額";
            this.lblMonthlyRemain0.Top = 1.023622F;
            this.lblMonthlyRemain0.Width = 1.011811F;
            // 
            // lblCurrentRemain
            // 
            this.lblCurrentRemain.Height = 0.2F;
            this.lblCurrentRemain.HyperLink = null;
            this.lblCurrentRemain.Left = 5.564961F;
            this.lblCurrentRemain.Name = "lblCurrentRemain";
            this.lblCurrentRemain.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrentRemain.Text = "当月請求残";
            this.lblCurrentRemain.Top = 1.023622F;
            this.lblCurrentRemain.Width = 1.011811F;
            // 
            // lblCurrentMatching
            // 
            this.lblCurrentMatching.Height = 0.2F;
            this.lblCurrentMatching.HyperLink = null;
            this.lblCurrentMatching.Left = 4.55315F;
            this.lblCurrentMatching.Name = "lblCurrentMatching";
            this.lblCurrentMatching.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrentMatching.Text = "当月消込";
            this.lblCurrentMatching.Top = 1.023622F;
            this.lblCurrentMatching.Width = 1.011811F;
            // 
            // lblCurrentBilling
            // 
            this.lblCurrentBilling.Height = 0.2F;
            this.lblCurrentBilling.HyperLink = null;
            this.lblCurrentBilling.Left = 3.541339F;
            this.lblCurrentBilling.Name = "lblCurrentBilling";
            this.lblCurrentBilling.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrentBilling.Text = "当月売上高";
            this.lblCurrentBilling.Top = 1.023622F;
            this.lblCurrentBilling.Width = 1.011811F;
            // 
            // lblLastMonthRemain
            // 
            this.lblLastMonthRemain.Height = 0.2F;
            this.lblLastMonthRemain.HyperLink = null;
            this.lblLastMonthRemain.Left = 2.529527F;
            this.lblLastMonthRemain.Name = "lblLastMonthRemain";
            this.lblLastMonthRemain.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblLastMonthRemain.Text = "前月請求残";
            this.lblLastMonthRemain.Top = 1.023622F;
            this.lblLastMonthRemain.Width = 1.011811F;
            // 
            // lblCustomer
            // 
            this.lblCustomer.Height = 0.2F;
            this.lblCustomer.HyperLink = null;
            this.lblCustomer.Left = 0F;
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomer.Text = "得意先";
            this.lblCustomer.Top = 1.023622F;
            this.lblCustomer.Width = 2.527559F;
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
            this.lblCompanyCode.Width = 0.8118109F;
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
            this.lblDate.Width = 0.698425F;
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
            this.ridate.Width = 1.085828F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = -1.862645E-09F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "請求残高年齢表";
            this.lblTitle.Top = 0.2704724F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.2F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 0F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDepartmentCode.Text = "請求部門コード：";
            this.lblDepartmentCode.Top = 0.5015748F;
            this.lblDepartmentCode.Width = 0.8118111F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.DataField = "DepartmentCode";
            this.txtDepartmentCode.Height = 0.2F;
            this.txtDepartmentCode.HyperLink = null;
            this.txtDepartmentCode.Left = 0.8118111F;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtDepartmentCode.Text = "";
            this.txtDepartmentCode.Top = 0.5015748F;
            this.txtDepartmentCode.Width = 0.5858268F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.DataField = "DepartmentName";
            this.txtDepartmentName.Height = 0.2F;
            this.txtDepartmentName.HyperLink = null;
            this.txtDepartmentName.Left = 1.397638F;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtDepartmentName.Text = "";
            this.txtDepartmentName.Top = 0.5015748F;
            this.txtDepartmentName.Width = 3.07126F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.2F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 0F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblStaffCode.Text = "担当者コード　：";
            this.lblStaffCode.Top = 0.7015749F;
            this.lblStaffCode.Width = 0.8118111F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.DataField = "StaffCode";
            this.txtStaffCode.Height = 0.2F;
            this.txtStaffCode.HyperLink = null;
            this.txtStaffCode.Left = 0.8118111F;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtStaffCode.Text = "";
            this.txtStaffCode.Top = 0.7015749F;
            this.txtStaffCode.Width = 0.5858268F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.DataField = "StaffName";
            this.txtStaffName.Height = 0.2F;
            this.txtStaffName.HyperLink = null;
            this.txtStaffName.Left = 1.397638F;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtStaffName.Text = "";
            this.txtStaffName.Top = 0.7015749F;
            this.txtStaffName.Width = 3.07126F;
            // 
            // lineHeaderVerCustomer
            // 
            this.lineHeaderVerCustomer.Height = 0.199898F;
            this.lineHeaderVerCustomer.Left = 2.529528F;
            this.lineHeaderVerCustomer.LineWeight = 1F;
            this.lineHeaderVerCustomer.Name = "lineHeaderVerCustomer";
            this.lineHeaderVerCustomer.Top = 1.023724F;
            this.lineHeaderVerCustomer.Width = 0F;
            this.lineHeaderVerCustomer.X1 = 2.529528F;
            this.lineHeaderVerCustomer.X2 = 2.529528F;
            this.lineHeaderVerCustomer.Y1 = 1.223622F;
            this.lineHeaderVerCustomer.Y2 = 1.023724F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 1.023622F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.62992F;
            this.lineHeaderHorUpper.Y1 = 1.023622F;
            this.lineHeaderHorUpper.Y2 = 1.023622F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.223622F;
            this.lineHeaderHorLower.Width = 10.62992F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.62992F;
            this.lineHeaderHorLower.Y1 = 1.223622F;
            this.lineHeaderHorLower.Y2 = 1.223622F;
            // 
            // lineHeaderVerLastMonthRemain1
            // 
            this.lineHeaderVerLastMonthRemain1.Height = 0.199898F;
            this.lineHeaderVerLastMonthRemain1.Left = 3.541339F;
            this.lineHeaderVerLastMonthRemain1.LineWeight = 1F;
            this.lineHeaderVerLastMonthRemain1.Name = "lineHeaderVerLastMonthRemain1";
            this.lineHeaderVerLastMonthRemain1.Top = 1.023724F;
            this.lineHeaderVerLastMonthRemain1.Width = 0F;
            this.lineHeaderVerLastMonthRemain1.X1 = 3.541339F;
            this.lineHeaderVerLastMonthRemain1.X2 = 3.541339F;
            this.lineHeaderVerLastMonthRemain1.Y1 = 1.223622F;
            this.lineHeaderVerLastMonthRemain1.Y2 = 1.023724F;
            // 
            // lineHeaderVerLastMonthRemain2
            // 
            this.lineHeaderVerLastMonthRemain2.Height = 0.199898F;
            this.lineHeaderVerLastMonthRemain2.Left = 3.561024F;
            this.lineHeaderVerLastMonthRemain2.LineWeight = 1F;
            this.lineHeaderVerLastMonthRemain2.Name = "lineHeaderVerLastMonthRemain2";
            this.lineHeaderVerLastMonthRemain2.Top = 1.023724F;
            this.lineHeaderVerLastMonthRemain2.Width = 0F;
            this.lineHeaderVerLastMonthRemain2.X1 = 3.561024F;
            this.lineHeaderVerLastMonthRemain2.X2 = 3.561024F;
            this.lineHeaderVerLastMonthRemain2.Y1 = 1.223622F;
            this.lineHeaderVerLastMonthRemain2.Y2 = 1.023724F;
            // 
            // lineHeaderVerCurrentBilling1
            // 
            this.lineHeaderVerCurrentBilling1.Height = 0.199898F;
            this.lineHeaderVerCurrentBilling1.Left = 4.55315F;
            this.lineHeaderVerCurrentBilling1.LineWeight = 1F;
            this.lineHeaderVerCurrentBilling1.Name = "lineHeaderVerCurrentBilling1";
            this.lineHeaderVerCurrentBilling1.Top = 1.023724F;
            this.lineHeaderVerCurrentBilling1.Width = 0F;
            this.lineHeaderVerCurrentBilling1.X1 = 4.55315F;
            this.lineHeaderVerCurrentBilling1.X2 = 4.55315F;
            this.lineHeaderVerCurrentBilling1.Y1 = 1.223622F;
            this.lineHeaderVerCurrentBilling1.Y2 = 1.023724F;
            // 
            // lineHeaderVerCurrentBilling2
            // 
            this.lineHeaderVerCurrentBilling2.Height = 0.199898F;
            this.lineHeaderVerCurrentBilling2.Left = 4.577953F;
            this.lineHeaderVerCurrentBilling2.LineWeight = 1F;
            this.lineHeaderVerCurrentBilling2.Name = "lineHeaderVerCurrentBilling2";
            this.lineHeaderVerCurrentBilling2.Top = 1.023724F;
            this.lineHeaderVerCurrentBilling2.Width = 0F;
            this.lineHeaderVerCurrentBilling2.X1 = 4.577953F;
            this.lineHeaderVerCurrentBilling2.X2 = 4.577953F;
            this.lineHeaderVerCurrentBilling2.Y1 = 1.223622F;
            this.lineHeaderVerCurrentBilling2.Y2 = 1.023724F;
            // 
            // lineHeaderVerCurrentMatching1
            // 
            this.lineHeaderVerCurrentMatching1.Height = 0.199898F;
            this.lineHeaderVerCurrentMatching1.Left = 5.564961F;
            this.lineHeaderVerCurrentMatching1.LineWeight = 1F;
            this.lineHeaderVerCurrentMatching1.Name = "lineHeaderVerCurrentMatching1";
            this.lineHeaderVerCurrentMatching1.Top = 1.023724F;
            this.lineHeaderVerCurrentMatching1.Width = 0F;
            this.lineHeaderVerCurrentMatching1.X1 = 5.564961F;
            this.lineHeaderVerCurrentMatching1.X2 = 5.564961F;
            this.lineHeaderVerCurrentMatching1.Y1 = 1.223622F;
            this.lineHeaderVerCurrentMatching1.Y2 = 1.023724F;
            // 
            // lineHeaderVerCurrentMatching2
            // 
            this.lineHeaderVerCurrentMatching2.Height = 0.199898F;
            this.lineHeaderVerCurrentMatching2.Left = 5.590158F;
            this.lineHeaderVerCurrentMatching2.LineWeight = 1F;
            this.lineHeaderVerCurrentMatching2.Name = "lineHeaderVerCurrentMatching2";
            this.lineHeaderVerCurrentMatching2.Top = 1.023724F;
            this.lineHeaderVerCurrentMatching2.Width = 0F;
            this.lineHeaderVerCurrentMatching2.X1 = 5.590158F;
            this.lineHeaderVerCurrentMatching2.X2 = 5.590158F;
            this.lineHeaderVerCurrentMatching2.Y1 = 1.223622F;
            this.lineHeaderVerCurrentMatching2.Y2 = 1.023724F;
            // 
            // lineHeaderVerCurrentRemain1
            // 
            this.lineHeaderVerCurrentRemain1.Height = 0.199898F;
            this.lineHeaderVerCurrentRemain1.Left = 6.576772F;
            this.lineHeaderVerCurrentRemain1.LineWeight = 1F;
            this.lineHeaderVerCurrentRemain1.Name = "lineHeaderVerCurrentRemain1";
            this.lineHeaderVerCurrentRemain1.Top = 1.023724F;
            this.lineHeaderVerCurrentRemain1.Width = 0F;
            this.lineHeaderVerCurrentRemain1.X1 = 6.576772F;
            this.lineHeaderVerCurrentRemain1.X2 = 6.576772F;
            this.lineHeaderVerCurrentRemain1.Y1 = 1.223622F;
            this.lineHeaderVerCurrentRemain1.Y2 = 1.023724F;
            // 
            // lineHeaderVerCurrentRemain2
            // 
            this.lineHeaderVerCurrentRemain2.Height = 0.199898F;
            this.lineHeaderVerCurrentRemain2.Left = 6.603938F;
            this.lineHeaderVerCurrentRemain2.LineWeight = 1F;
            this.lineHeaderVerCurrentRemain2.Name = "lineHeaderVerCurrentRemain2";
            this.lineHeaderVerCurrentRemain2.Top = 1.023724F;
            this.lineHeaderVerCurrentRemain2.Width = 0F;
            this.lineHeaderVerCurrentRemain2.X1 = 6.603938F;
            this.lineHeaderVerCurrentRemain2.X2 = 6.603938F;
            this.lineHeaderVerCurrentRemain2.Y1 = 1.223622F;
            this.lineHeaderVerCurrentRemain2.Y2 = 1.023724F;
            // 
            // lineHeaderVerMonthlyRemain0
            // 
            this.lineHeaderVerMonthlyRemain0.Height = 0.199898F;
            this.lineHeaderVerMonthlyRemain0.Left = 7.588583F;
            this.lineHeaderVerMonthlyRemain0.LineWeight = 1F;
            this.lineHeaderVerMonthlyRemain0.Name = "lineHeaderVerMonthlyRemain0";
            this.lineHeaderVerMonthlyRemain0.Top = 1.023724F;
            this.lineHeaderVerMonthlyRemain0.Width = 0F;
            this.lineHeaderVerMonthlyRemain0.X1 = 7.588583F;
            this.lineHeaderVerMonthlyRemain0.X2 = 7.588583F;
            this.lineHeaderVerMonthlyRemain0.Y1 = 1.223622F;
            this.lineHeaderVerMonthlyRemain0.Y2 = 1.023724F;
            // 
            // lineHeaderVerMonthlyRemain1
            // 
            this.lineHeaderVerMonthlyRemain1.Height = 0.199898F;
            this.lineHeaderVerMonthlyRemain1.Left = 8.600394F;
            this.lineHeaderVerMonthlyRemain1.LineWeight = 1F;
            this.lineHeaderVerMonthlyRemain1.Name = "lineHeaderVerMonthlyRemain1";
            this.lineHeaderVerMonthlyRemain1.Top = 1.023724F;
            this.lineHeaderVerMonthlyRemain1.Width = 0F;
            this.lineHeaderVerMonthlyRemain1.X1 = 8.600394F;
            this.lineHeaderVerMonthlyRemain1.X2 = 8.600394F;
            this.lineHeaderVerMonthlyRemain1.Y1 = 1.223622F;
            this.lineHeaderVerMonthlyRemain1.Y2 = 1.023724F;
            // 
            // lineHeaderVerMonthlyRemain2
            // 
            this.lineHeaderVerMonthlyRemain2.Height = 0.199898F;
            this.lineHeaderVerMonthlyRemain2.Left = 9.612206F;
            this.lineHeaderVerMonthlyRemain2.LineWeight = 1F;
            this.lineHeaderVerMonthlyRemain2.Name = "lineHeaderVerMonthlyRemain2";
            this.lineHeaderVerMonthlyRemain2.Top = 1.023724F;
            this.lineHeaderVerMonthlyRemain2.Width = 0F;
            this.lineHeaderVerMonthlyRemain2.X1 = 9.612206F;
            this.lineHeaderVerMonthlyRemain2.X2 = 9.612206F;
            this.lineHeaderVerMonthlyRemain2.Y1 = 1.223622F;
            this.lineHeaderVerMonthlyRemain2.Y2 = 1.023724F;
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
            this.txtCurrentMonthSales,
            this.txtLastMonthRemain,
            this.txtChildCustomer,
            this.txtParentCustomer,
            this.lineDetailHorLower,
            this.lineDetailVerCustomer,
            this.lineDetailVerLastMonthRemain1,
            this.lineDetailVerLastMonthRemain2,
            this.lineDetailVerCurrentBilling1,
            this.lineDetailVerCurrentBilling2,
            this.lineDetailVerCurrentMatching1,
            this.lineDetailVerCurrentMatching2,
            this.lineDetailVerCurrentRemain1,
            this.lineDetailVerCurrentRemain2,
            this.lineDetailVerMonthlyRemain0,
            this.lineDetailVerMonthlyRemain1,
            this.lineDetailVerMonthlyRemain2});
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
            this.txtMonthlyRemain3.Left = 9.616142F;
            this.txtMonthlyRemain3.MultiLine = false;
            this.txtMonthlyRemain3.Name = "txtMonthlyRemain3";
            this.txtMonthlyRemain3.OutputFormat = resources.GetString("txtMonthlyRemain3.OutputFormat");
            this.txtMonthlyRemain3.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMonthlyRemain3.Style = "font-size: 8.25pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMonthlyRemain3.Text = null;
            this.txtMonthlyRemain3.Top = 0F;
            this.txtMonthlyRemain3.Width = 1.011811F;
            // 
            // txtMonthlyRemain2
            // 
            this.txtMonthlyRemain2.DataField = "MonthlyRemain2";
            this.txtMonthlyRemain2.Height = 0.2F;
            this.txtMonthlyRemain2.Left = 8.600394F;
            this.txtMonthlyRemain2.MultiLine = false;
            this.txtMonthlyRemain2.Name = "txtMonthlyRemain2";
            this.txtMonthlyRemain2.OutputFormat = resources.GetString("txtMonthlyRemain2.OutputFormat");
            this.txtMonthlyRemain2.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMonthlyRemain2.Style = "font-size: 8.25pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMonthlyRemain2.Text = null;
            this.txtMonthlyRemain2.Top = 0F;
            this.txtMonthlyRemain2.Width = 1.011811F;
            // 
            // txtMonthlyRemain1
            // 
            this.txtMonthlyRemain1.DataField = "MonthlyRemain1";
            this.txtMonthlyRemain1.Height = 0.2F;
            this.txtMonthlyRemain1.Left = 7.59252F;
            this.txtMonthlyRemain1.MultiLine = false;
            this.txtMonthlyRemain1.Name = "txtMonthlyRemain1";
            this.txtMonthlyRemain1.OutputFormat = resources.GetString("txtMonthlyRemain1.OutputFormat");
            this.txtMonthlyRemain1.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMonthlyRemain1.Style = "font-size: 8.25pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMonthlyRemain1.Text = null;
            this.txtMonthlyRemain1.Top = 0F;
            this.txtMonthlyRemain1.Width = 1.011811F;
            // 
            // txtMonthlyRemain0
            // 
            this.txtMonthlyRemain0.DataField = "MonthlyRemain0";
            this.txtMonthlyRemain0.Height = 0.2F;
            this.txtMonthlyRemain0.Left = 6.580709F;
            this.txtMonthlyRemain0.MultiLine = false;
            this.txtMonthlyRemain0.Name = "txtMonthlyRemain0";
            this.txtMonthlyRemain0.OutputFormat = resources.GetString("txtMonthlyRemain0.OutputFormat");
            this.txtMonthlyRemain0.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMonthlyRemain0.Style = "font-size: 8.25pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtMonthlyRemain0.Text = null;
            this.txtMonthlyRemain0.Top = 0F;
            this.txtMonthlyRemain0.Width = 1.011811F;
            // 
            // txtCurrentMonthRemain
            // 
            this.txtCurrentMonthRemain.DataField = "CurrentMonthRemain";
            this.txtCurrentMonthRemain.Height = 0.2F;
            this.txtCurrentMonthRemain.Left = 5.564961F;
            this.txtCurrentMonthRemain.MultiLine = false;
            this.txtCurrentMonthRemain.Name = "txtCurrentMonthRemain";
            this.txtCurrentMonthRemain.OutputFormat = resources.GetString("txtCurrentMonthRemain.OutputFormat");
            this.txtCurrentMonthRemain.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCurrentMonthRemain.Style = "font-size: 8.25pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrentMonthRemain.Text = null;
            this.txtCurrentMonthRemain.Top = 0F;
            this.txtCurrentMonthRemain.Width = 1.011811F;
            // 
            // txtCurrentMonthMatching
            // 
            this.txtCurrentMonthMatching.DataField = "CurrentMonthMatching";
            this.txtCurrentMonthMatching.Height = 0.2F;
            this.txtCurrentMonthMatching.Left = 4.55315F;
            this.txtCurrentMonthMatching.MultiLine = false;
            this.txtCurrentMonthMatching.Name = "txtCurrentMonthMatching";
            this.txtCurrentMonthMatching.OutputFormat = resources.GetString("txtCurrentMonthMatching.OutputFormat");
            this.txtCurrentMonthMatching.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCurrentMonthMatching.Style = "font-size: 8.25pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrentMonthMatching.Text = null;
            this.txtCurrentMonthMatching.Top = 0F;
            this.txtCurrentMonthMatching.Width = 1.011811F;
            // 
            // txtCurrentMonthSales
            // 
            this.txtCurrentMonthSales.DataField = "CurrentMonthSales";
            this.txtCurrentMonthSales.Height = 0.2F;
            this.txtCurrentMonthSales.Left = 3.541339F;
            this.txtCurrentMonthSales.MultiLine = false;
            this.txtCurrentMonthSales.Name = "txtCurrentMonthSales";
            this.txtCurrentMonthSales.OutputFormat = resources.GetString("txtCurrentMonthSales.OutputFormat");
            this.txtCurrentMonthSales.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCurrentMonthSales.Style = "font-size: 8.25pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrentMonthSales.Text = null;
            this.txtCurrentMonthSales.Top = 0F;
            this.txtCurrentMonthSales.Width = 1.011811F;
            // 
            // txtLastMonthRemain
            // 
            this.txtLastMonthRemain.DataField = "LastMonthRemain";
            this.txtLastMonthRemain.Height = 0.2F;
            this.txtLastMonthRemain.Left = 2.529528F;
            this.txtLastMonthRemain.MultiLine = false;
            this.txtLastMonthRemain.Name = "txtLastMonthRemain";
            this.txtLastMonthRemain.OutputFormat = resources.GetString("txtLastMonthRemain.OutputFormat");
            this.txtLastMonthRemain.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtLastMonthRemain.Style = "font-size: 8.25pt; text-align: right; vertical-align: middle; ddo-char-set: 128";
            this.txtLastMonthRemain.Text = null;
            this.txtLastMonthRemain.Top = 0F;
            this.txtLastMonthRemain.Width = 1.011811F;
            // 
            // txtChildCustomer
            // 
            this.txtChildCustomer.Height = 0.2F;
            this.txtChildCustomer.HyperLink = null;
            this.txtChildCustomer.Left = 2.141732F;
            this.txtChildCustomer.Name = "txtChildCustomer";
            this.txtChildCustomer.Style = "color: Gray; font-size: 7pt; text-align: right; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.txtChildCustomer.Text = "";
            this.txtChildCustomer.Top = 0F;
            this.txtChildCustomer.Width = 0.4F;
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
            this.txtParentCustomer.Width = 2.133858F;
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
            // lineDetailVerCustomer
            // 
            this.lineDetailVerCustomer.Height = 0.2F;
            this.lineDetailVerCustomer.Left = 2.529528F;
            this.lineDetailVerCustomer.LineWeight = 1F;
            this.lineDetailVerCustomer.Name = "lineDetailVerCustomer";
            this.lineDetailVerCustomer.Top = 0F;
            this.lineDetailVerCustomer.Width = 0F;
            this.lineDetailVerCustomer.X1 = 2.529528F;
            this.lineDetailVerCustomer.X2 = 2.529528F;
            this.lineDetailVerCustomer.Y1 = 0.2F;
            this.lineDetailVerCustomer.Y2 = 0F;
            // 
            // lineDetailVerLastMonthRemain1
            // 
            this.lineDetailVerLastMonthRemain1.Height = 0.2F;
            this.lineDetailVerLastMonthRemain1.Left = 3.541339F;
            this.lineDetailVerLastMonthRemain1.LineWeight = 1F;
            this.lineDetailVerLastMonthRemain1.Name = "lineDetailVerLastMonthRemain1";
            this.lineDetailVerLastMonthRemain1.Top = -8.881784E-16F;
            this.lineDetailVerLastMonthRemain1.Width = 0F;
            this.lineDetailVerLastMonthRemain1.X1 = 3.541339F;
            this.lineDetailVerLastMonthRemain1.X2 = 3.541339F;
            this.lineDetailVerLastMonthRemain1.Y1 = 0.2F;
            this.lineDetailVerLastMonthRemain1.Y2 = -8.881784E-16F;
            // 
            // lineDetailVerLastMonthRemain2
            // 
            this.lineDetailVerLastMonthRemain2.Height = 0.2F;
            this.lineDetailVerLastMonthRemain2.Left = 3.561024F;
            this.lineDetailVerLastMonthRemain2.LineWeight = 1F;
            this.lineDetailVerLastMonthRemain2.Name = "lineDetailVerLastMonthRemain2";
            this.lineDetailVerLastMonthRemain2.Top = 0F;
            this.lineDetailVerLastMonthRemain2.Width = 0F;
            this.lineDetailVerLastMonthRemain2.X1 = 3.561024F;
            this.lineDetailVerLastMonthRemain2.X2 = 3.561024F;
            this.lineDetailVerLastMonthRemain2.Y1 = 0.2F;
            this.lineDetailVerLastMonthRemain2.Y2 = 0F;
            // 
            // lineDetailVerCurrentBilling1
            // 
            this.lineDetailVerCurrentBilling1.Height = 0.2F;
            this.lineDetailVerCurrentBilling1.Left = 4.55315F;
            this.lineDetailVerCurrentBilling1.LineWeight = 1F;
            this.lineDetailVerCurrentBilling1.Name = "lineDetailVerCurrentBilling1";
            this.lineDetailVerCurrentBilling1.Top = 0F;
            this.lineDetailVerCurrentBilling1.Width = 0F;
            this.lineDetailVerCurrentBilling1.X1 = 4.55315F;
            this.lineDetailVerCurrentBilling1.X2 = 4.55315F;
            this.lineDetailVerCurrentBilling1.Y1 = 0.2F;
            this.lineDetailVerCurrentBilling1.Y2 = 0F;
            // 
            // lineDetailVerCurrentBilling2
            // 
            this.lineDetailVerCurrentBilling2.Height = 0.2F;
            this.lineDetailVerCurrentBilling2.Left = 4.577953F;
            this.lineDetailVerCurrentBilling2.LineWeight = 1F;
            this.lineDetailVerCurrentBilling2.Name = "lineDetailVerCurrentBilling2";
            this.lineDetailVerCurrentBilling2.Top = 0F;
            this.lineDetailVerCurrentBilling2.Width = 0F;
            this.lineDetailVerCurrentBilling2.X1 = 4.577953F;
            this.lineDetailVerCurrentBilling2.X2 = 4.577953F;
            this.lineDetailVerCurrentBilling2.Y1 = 0.2F;
            this.lineDetailVerCurrentBilling2.Y2 = 0F;
            // 
            // lineDetailVerCurrentMatching1
            // 
            this.lineDetailVerCurrentMatching1.Height = 0.2F;
            this.lineDetailVerCurrentMatching1.Left = 5.564961F;
            this.lineDetailVerCurrentMatching1.LineWeight = 1F;
            this.lineDetailVerCurrentMatching1.Name = "lineDetailVerCurrentMatching1";
            this.lineDetailVerCurrentMatching1.Top = 0F;
            this.lineDetailVerCurrentMatching1.Width = 0F;
            this.lineDetailVerCurrentMatching1.X1 = 5.564961F;
            this.lineDetailVerCurrentMatching1.X2 = 5.564961F;
            this.lineDetailVerCurrentMatching1.Y1 = 0.2F;
            this.lineDetailVerCurrentMatching1.Y2 = 0F;
            // 
            // lineDetailVerCurrentMatching2
            // 
            this.lineDetailVerCurrentMatching2.Height = 0.2F;
            this.lineDetailVerCurrentMatching2.Left = 5.590158F;
            this.lineDetailVerCurrentMatching2.LineWeight = 1F;
            this.lineDetailVerCurrentMatching2.Name = "lineDetailVerCurrentMatching2";
            this.lineDetailVerCurrentMatching2.Top = 0F;
            this.lineDetailVerCurrentMatching2.Width = 0F;
            this.lineDetailVerCurrentMatching2.X1 = 5.590158F;
            this.lineDetailVerCurrentMatching2.X2 = 5.590158F;
            this.lineDetailVerCurrentMatching2.Y1 = 0.2F;
            this.lineDetailVerCurrentMatching2.Y2 = 0F;
            // 
            // lineDetailVerCurrentRemain1
            // 
            this.lineDetailVerCurrentRemain1.Height = 0.2F;
            this.lineDetailVerCurrentRemain1.Left = 6.576772F;
            this.lineDetailVerCurrentRemain1.LineWeight = 1F;
            this.lineDetailVerCurrentRemain1.Name = "lineDetailVerCurrentRemain1";
            this.lineDetailVerCurrentRemain1.Top = 0F;
            this.lineDetailVerCurrentRemain1.Width = 0F;
            this.lineDetailVerCurrentRemain1.X1 = 6.576772F;
            this.lineDetailVerCurrentRemain1.X2 = 6.576772F;
            this.lineDetailVerCurrentRemain1.Y1 = 0.2F;
            this.lineDetailVerCurrentRemain1.Y2 = 0F;
            // 
            // lineDetailVerCurrentRemain2
            // 
            this.lineDetailVerCurrentRemain2.Height = 0.2F;
            this.lineDetailVerCurrentRemain2.Left = 6.603938F;
            this.lineDetailVerCurrentRemain2.LineWeight = 1F;
            this.lineDetailVerCurrentRemain2.Name = "lineDetailVerCurrentRemain2";
            this.lineDetailVerCurrentRemain2.Top = 0F;
            this.lineDetailVerCurrentRemain2.Width = 0F;
            this.lineDetailVerCurrentRemain2.X1 = 6.603938F;
            this.lineDetailVerCurrentRemain2.X2 = 6.603938F;
            this.lineDetailVerCurrentRemain2.Y1 = 0.2F;
            this.lineDetailVerCurrentRemain2.Y2 = 0F;
            // 
            // lineDetailVerMonthlyRemain0
            // 
            this.lineDetailVerMonthlyRemain0.Height = 0.2F;
            this.lineDetailVerMonthlyRemain0.Left = 7.588583F;
            this.lineDetailVerMonthlyRemain0.LineWeight = 1F;
            this.lineDetailVerMonthlyRemain0.Name = "lineDetailVerMonthlyRemain0";
            this.lineDetailVerMonthlyRemain0.Top = 0F;
            this.lineDetailVerMonthlyRemain0.Width = 0F;
            this.lineDetailVerMonthlyRemain0.X1 = 7.588583F;
            this.lineDetailVerMonthlyRemain0.X2 = 7.588583F;
            this.lineDetailVerMonthlyRemain0.Y1 = 0.2F;
            this.lineDetailVerMonthlyRemain0.Y2 = 0F;
            // 
            // lineDetailVerMonthlyRemain1
            // 
            this.lineDetailVerMonthlyRemain1.Height = 0.2F;
            this.lineDetailVerMonthlyRemain1.Left = 8.600394F;
            this.lineDetailVerMonthlyRemain1.LineWeight = 1F;
            this.lineDetailVerMonthlyRemain1.Name = "lineDetailVerMonthlyRemain1";
            this.lineDetailVerMonthlyRemain1.Top = 0F;
            this.lineDetailVerMonthlyRemain1.Width = 0F;
            this.lineDetailVerMonthlyRemain1.X1 = 8.600394F;
            this.lineDetailVerMonthlyRemain1.X2 = 8.600394F;
            this.lineDetailVerMonthlyRemain1.Y1 = 0.2F;
            this.lineDetailVerMonthlyRemain1.Y2 = 0F;
            // 
            // lineDetailVerMonthlyRemain2
            // 
            this.lineDetailVerMonthlyRemain2.Height = 0.2F;
            this.lineDetailVerMonthlyRemain2.Left = 9.612206F;
            this.lineDetailVerMonthlyRemain2.LineWeight = 1F;
            this.lineDetailVerMonthlyRemain2.Name = "lineDetailVerMonthlyRemain2";
            this.lineDetailVerMonthlyRemain2.Top = 0F;
            this.lineDetailVerMonthlyRemain2.Width = 0F;
            this.lineDetailVerMonthlyRemain2.X1 = 9.612206F;
            this.lineDetailVerMonthlyRemain2.X2 = 9.612206F;
            this.lineDetailVerMonthlyRemain2.Y1 = 0.2F;
            this.lineDetailVerMonthlyRemain2.Y2 = 0F;
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
            this.reportInfo1.Left = 7.086615F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray";
            this.reportInfo1.Top = 0.07874016F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1F;
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
            // BillingAgingListSectionReport1
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
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMonthlyRemain0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentMatching)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrentBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLastMonthRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomer)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrentMonthSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastMonthRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChildCustomer)).EndInit();
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
        private GrapeCity.ActiveReports.SectionReportModel.Label lblLastMonthRemain;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrentBilling;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrentMatching;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrentRemain;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblMonthlyRemain0;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblMonthlyRemain1;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblMonthlyRemain2;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblMonthlyRemain3;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtParentCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtChildCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerLastMonthRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentBilling1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMatching2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMonthlyRemain0;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMonthlyRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMonthlyRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerLastMonthRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentBilling2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentMatching1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrentRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtLastMonthRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrentMonthSales;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrentMonthMatching;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrentMonthRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMonthlyRemain0;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMonthlyRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMonthlyRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMonthlyRemain3;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerLastMonthRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerLastMonthRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentBilling1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentBilling2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentMatching1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentMatching2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrentRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerMonthlyRemain0;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerMonthlyRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerMonthlyRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Shape detailBackColor;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
    }
}
