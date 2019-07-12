namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CreditAgingList の概要の説明です。
    /// </summary>
    partial class CreditAgingListReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CreditAgingListReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCollectCategory = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblUnsettleRemain = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingRemain = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditLimit = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreditRemain = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblArrivalDueDate1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblArrivalDueDate2 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblArrivalDueDate3 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblArrivalDueDate4 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartment = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaff = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCollectCategory = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerUnsettleRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditAmount1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingRemain = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditLimit1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerArrivalDueDate1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerArrivalDueDate2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerArrivalDueDate3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditAmount2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerUnsettleRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreditRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblDepartmentCodeAndName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCodeAndName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSubTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCollectCategory = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUnsettledRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditLimit = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditRemain = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreditRemainStar = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtArrivalDueDate1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtArrivalDueDate2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtArrivalDueDate3 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtArrivalDueDate4 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerSubTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCollectCategory = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditAmount2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerUnsettledRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingRemain = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditLimit1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerArrivalDueDate1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerArrivalDueDate2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerArrivalDueDate3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditAmount1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerUnsettledRemain1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCreditRemain2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCollectCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUnsettleRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrivalDueDate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrivalDueDate2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrivalDueDate3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrivalDueDate4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCodeAndName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCodeAndName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnsettledRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditRemain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditRemainStar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrivalDueDate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrivalDueDate2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrivalDueDate3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrivalDueDate4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCustomerCode,
            this.lblCollectCategory,
            this.lblCreditAmount,
            this.lblUnsettleRemain,
            this.lblBillingRemain,
            this.lblCreditLimit,
            this.lblCreditRemain,
            this.lblArrivalDueDate1,
            this.lblArrivalDueDate2,
            this.lblArrivalDueDate3,
            this.lblArrivalDueDate4,
            this.lblTitle,
            this.lblDepartment,
            this.lblStaff,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderVerCollectCategory,
            this.lineHeaderVerUnsettleRemain2,
            this.lineHeaderVerCreditAmount1,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lineHeaderHorLower,
            this.lineHeaderVerBillingRemain,
            this.lineHeaderVerCreditLimit1,
            this.lineHeaderVerCreditRemain1,
            this.lineHeaderVerArrivalDueDate1,
            this.lineHeaderVerArrivalDueDate2,
            this.lineHeaderVerArrivalDueDate3,
            this.lineHeaderVerCreditAmount2,
            this.lineHeaderVerUnsettleRemain1,
            this.lineHeaderVerCreditRemain2,
            this.lblDepartmentCodeAndName,
            this.lblStaffCodeAndName,
            this.lblDepartmentCode,
            this.lblStaffCode,
            this.lineHeaderHorUpper});
            this.pageHeader.Height = 1.343307F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.4F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 0F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1; ddo-font-vertical: none";
            this.lblCustomerCode.Text = "得意先";
            this.lblCustomerCode.Top = 0.9448819F;
            this.lblCustomerCode.Width = 2.170079F;
            // 
            // lblCollectCategory
            // 
            this.lblCollectCategory.Height = 0.4F;
            this.lblCollectCategory.HyperLink = null;
            this.lblCollectCategory.Left = 2.170079F;
            this.lblCollectCategory.Name = "lblCollectCategory";
            this.lblCollectCategory.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblCollectCategory.Text = "回収条件";
            this.lblCollectCategory.Top = 0.9511812F;
            this.lblCollectCategory.Width = 0.5496063F;
            // 
            // lblCreditAmount
            // 
            this.lblCreditAmount.Height = 0.4F;
            this.lblCreditAmount.HyperLink = null;
            this.lblCreditAmount.Left = 2.719685F;
            this.lblCreditAmount.Name = "lblCreditAmount";
            this.lblCreditAmount.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblCreditAmount.Text = "当月債権総額";
            this.lblCreditAmount.Top = 0.944882F;
            this.lblCreditAmount.Width = 0.8661418F;
            // 
            // lblUnsettleRemain
            // 
            this.lblUnsettleRemain.Height = 0.4F;
            this.lblUnsettleRemain.HyperLink = null;
            this.lblUnsettleRemain.Left = 3.585827F;
            this.lblUnsettleRemain.Name = "lblUnsettleRemain";
            this.lblUnsettleRemain.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblUnsettleRemain.Text = "当月末未決済残高";
            this.lblUnsettleRemain.Top = 0.944882F;
            this.lblUnsettleRemain.Width = 0.8661418F;
            // 
            // lblBillingRemain
            // 
            this.lblBillingRemain.Height = 0.4F;
            this.lblBillingRemain.HyperLink = null;
            this.lblBillingRemain.Left = 4.451969F;
            this.lblBillingRemain.Name = "lblBillingRemain";
            this.lblBillingRemain.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblBillingRemain.Text = "当月請求残高";
            this.lblBillingRemain.Top = 0.9511812F;
            this.lblBillingRemain.Width = 0.8661418F;
            // 
            // lblCreditLimit
            // 
            this.lblCreditLimit.Height = 0.4F;
            this.lblCreditLimit.HyperLink = null;
            this.lblCreditLimit.Left = 5.31811F;
            this.lblCreditLimit.Name = "lblCreditLimit";
            this.lblCreditLimit.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblCreditLimit.Text = "与信限度額";
            this.lblCreditLimit.Top = 0.944882F;
            this.lblCreditLimit.Width = 0.8661418F;
            // 
            // lblCreditRemain
            // 
            this.lblCreditRemain.Height = 0.4F;
            this.lblCreditRemain.HyperLink = null;
            this.lblCreditRemain.Left = 6.184252F;
            this.lblCreditRemain.Name = "lblCreditRemain";
            this.lblCreditRemain.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblCreditRemain.Text = "当月与信残高";
            this.lblCreditRemain.Top = 0.9511812F;
            this.lblCreditRemain.Width = 0.9811025F;
            // 
            // lblArrivalDueDate1
            // 
            this.lblArrivalDueDate1.Height = 0.4F;
            this.lblArrivalDueDate1.HyperLink = null;
            this.lblArrivalDueDate1.Left = 7.165355F;
            this.lblArrivalDueDate1.Name = "lblArrivalDueDate1";
            this.lblArrivalDueDate1.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblArrivalDueDate1.Text = "WW月期日到来";
            this.lblArrivalDueDate1.Top = 0.944882F;
            this.lblArrivalDueDate1.Width = 0.8661418F;
            // 
            // lblArrivalDueDate2
            // 
            this.lblArrivalDueDate2.Height = 0.4F;
            this.lblArrivalDueDate2.HyperLink = null;
            this.lblArrivalDueDate2.Left = 8.026772F;
            this.lblArrivalDueDate2.Name = "lblArrivalDueDate2";
            this.lblArrivalDueDate2.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblArrivalDueDate2.Text = "XX月期日到来";
            this.lblArrivalDueDate2.Top = 0.944882F;
            this.lblArrivalDueDate2.Width = 0.8661418F;
            // 
            // lblArrivalDueDate3
            // 
            this.lblArrivalDueDate3.Height = 0.4F;
            this.lblArrivalDueDate3.HyperLink = null;
            this.lblArrivalDueDate3.Left = 8.892914F;
            this.lblArrivalDueDate3.Name = "lblArrivalDueDate3";
            this.lblArrivalDueDate3.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblArrivalDueDate3.Text = "YY月期日到来";
            this.lblArrivalDueDate3.Top = 0.944882F;
            this.lblArrivalDueDate3.Width = 0.8661418F;
            // 
            // lblArrivalDueDate4
            // 
            this.lblArrivalDueDate4.Height = 0.4F;
            this.lblArrivalDueDate4.HyperLink = null;
            this.lblArrivalDueDate4.Left = 9.759056F;
            this.lblArrivalDueDate4.Name = "lblArrivalDueDate4";
            this.lblArrivalDueDate4.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 1";
            this.lblArrivalDueDate4.Text = "ZZ月以降期日到来";
            this.lblArrivalDueDate4.Top = 0.944882F;
            this.lblArrivalDueDate4.Width = 0.8661418F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.231F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.lblTitle.Text = "債権総額管理表";
            this.lblTitle.Top = 0.2362205F;
            this.lblTitle.Width = 10.63F;
            // 
            // lblDepartment
            // 
            this.lblDepartment.Height = 0.2F;
            this.lblDepartment.HyperLink = null;
            this.lblDepartment.Left = 0F;
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Style = "font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDepartment.Text = "請求部門コード :";
            this.lblDepartment.Top = 0.472441F;
            this.lblDepartment.Width = 0.8661417F;
            // 
            // lblStaff
            // 
            this.lblStaff.Height = 0.2F;
            this.lblStaff.HyperLink = null;
            this.lblStaff.Left = 0F;
            this.lblStaff.Name = "lblStaff";
            this.lblStaff.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: left; vertical-align: middle; ddo" +
    "-char-set: 1";
            this.lblStaff.Text = "担当者コード   :";
            this.lblStaff.Top = 0.6724409F;
            this.lblStaff.Width = 0.8661417F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.393701F;
            this.lineHeaderVerCustomerCode.Left = 2.170079F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.944882F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 2.170079F;
            this.lineHeaderVerCustomerCode.X2 = 2.170079F;
            this.lineHeaderVerCustomerCode.Y1 = 0.944882F;
            this.lineHeaderVerCustomerCode.Y2 = 1.338583F;
            // 
            // lineHeaderVerCollectCategory
            // 
            this.lineHeaderVerCollectCategory.Height = 0.393701F;
            this.lineHeaderVerCollectCategory.Left = 2.719685F;
            this.lineHeaderVerCollectCategory.LineWeight = 1F;
            this.lineHeaderVerCollectCategory.Name = "lineHeaderVerCollectCategory";
            this.lineHeaderVerCollectCategory.Top = 0.944882F;
            this.lineHeaderVerCollectCategory.Width = 0F;
            this.lineHeaderVerCollectCategory.X1 = 2.719685F;
            this.lineHeaderVerCollectCategory.X2 = 2.719685F;
            this.lineHeaderVerCollectCategory.Y1 = 0.944882F;
            this.lineHeaderVerCollectCategory.Y2 = 1.338583F;
            // 
            // lineHeaderVerUnsettleRemain2
            // 
            this.lineHeaderVerUnsettleRemain2.Height = 0.393701F;
            this.lineHeaderVerUnsettleRemain2.Left = 4.467716F;
            this.lineHeaderVerUnsettleRemain2.LineWeight = 1F;
            this.lineHeaderVerUnsettleRemain2.Name = "lineHeaderVerUnsettleRemain2";
            this.lineHeaderVerUnsettleRemain2.Top = 0.944882F;
            this.lineHeaderVerUnsettleRemain2.Width = 0F;
            this.lineHeaderVerUnsettleRemain2.X1 = 4.467716F;
            this.lineHeaderVerUnsettleRemain2.X2 = 4.467716F;
            this.lineHeaderVerUnsettleRemain2.Y1 = 0.944882F;
            this.lineHeaderVerUnsettleRemain2.Y2 = 1.338583F;
            // 
            // lineHeaderVerCreditAmount1
            // 
            this.lineHeaderVerCreditAmount1.Height = 0.393701F;
            this.lineHeaderVerCreditAmount1.Left = 3.585827F;
            this.lineHeaderVerCreditAmount1.LineWeight = 1F;
            this.lineHeaderVerCreditAmount1.Name = "lineHeaderVerCreditAmount1";
            this.lineHeaderVerCreditAmount1.Top = 0.944882F;
            this.lineHeaderVerCreditAmount1.Width = 0F;
            this.lineHeaderVerCreditAmount1.X1 = 3.585827F;
            this.lineHeaderVerCreditAmount1.X2 = 3.585827F;
            this.lineHeaderVerCreditAmount1.Y1 = 0.944882F;
            this.lineHeaderVerCreditAmount1.Y2 = 1.338583F;
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
            this.lblCompanyCode.Width = 0.7874014F;
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
            this.ridate.Width = 1.014961F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.338583F;
            this.lineHeaderHorLower.Width = 10.62993F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.62993F;
            this.lineHeaderHorLower.Y1 = 1.338583F;
            this.lineHeaderHorLower.Y2 = 1.338583F;
            // 
            // lineHeaderVerBillingRemain
            // 
            this.lineHeaderVerBillingRemain.Height = 0.393701F;
            this.lineHeaderVerBillingRemain.Left = 5.31811F;
            this.lineHeaderVerBillingRemain.LineWeight = 1F;
            this.lineHeaderVerBillingRemain.Name = "lineHeaderVerBillingRemain";
            this.lineHeaderVerBillingRemain.Top = 0.944882F;
            this.lineHeaderVerBillingRemain.Width = 0F;
            this.lineHeaderVerBillingRemain.X1 = 5.31811F;
            this.lineHeaderVerBillingRemain.X2 = 5.31811F;
            this.lineHeaderVerBillingRemain.Y1 = 0.944882F;
            this.lineHeaderVerBillingRemain.Y2 = 1.338583F;
            // 
            // lineHeaderVerCreditLimit1
            // 
            this.lineHeaderVerCreditLimit1.Height = 0.393701F;
            this.lineHeaderVerCreditLimit1.Left = 6.184252F;
            this.lineHeaderVerCreditLimit1.LineWeight = 1F;
            this.lineHeaderVerCreditLimit1.Name = "lineHeaderVerCreditLimit1";
            this.lineHeaderVerCreditLimit1.Top = 0.944882F;
            this.lineHeaderVerCreditLimit1.Width = 0F;
            this.lineHeaderVerCreditLimit1.X1 = 6.184252F;
            this.lineHeaderVerCreditLimit1.X2 = 6.184252F;
            this.lineHeaderVerCreditLimit1.Y1 = 0.944882F;
            this.lineHeaderVerCreditLimit1.Y2 = 1.338583F;
            // 
            // lineHeaderVerCreditRemain1
            // 
            this.lineHeaderVerCreditRemain1.Height = 0.393701F;
            this.lineHeaderVerCreditRemain1.Left = 7.165355F;
            this.lineHeaderVerCreditRemain1.LineWeight = 1F;
            this.lineHeaderVerCreditRemain1.Name = "lineHeaderVerCreditRemain1";
            this.lineHeaderVerCreditRemain1.Top = 0.944882F;
            this.lineHeaderVerCreditRemain1.Width = 0F;
            this.lineHeaderVerCreditRemain1.X1 = 7.165355F;
            this.lineHeaderVerCreditRemain1.X2 = 7.165355F;
            this.lineHeaderVerCreditRemain1.Y1 = 0.944882F;
            this.lineHeaderVerCreditRemain1.Y2 = 1.338583F;
            // 
            // lineHeaderVerArrivalDueDate1
            // 
            this.lineHeaderVerArrivalDueDate1.Height = 0.393701F;
            this.lineHeaderVerArrivalDueDate1.Left = 8.031497F;
            this.lineHeaderVerArrivalDueDate1.LineWeight = 1F;
            this.lineHeaderVerArrivalDueDate1.Name = "lineHeaderVerArrivalDueDate1";
            this.lineHeaderVerArrivalDueDate1.Top = 0.944882F;
            this.lineHeaderVerArrivalDueDate1.Width = 0F;
            this.lineHeaderVerArrivalDueDate1.X1 = 8.031497F;
            this.lineHeaderVerArrivalDueDate1.X2 = 8.031497F;
            this.lineHeaderVerArrivalDueDate1.Y1 = 0.944882F;
            this.lineHeaderVerArrivalDueDate1.Y2 = 1.338583F;
            // 
            // lineHeaderVerArrivalDueDate2
            // 
            this.lineHeaderVerArrivalDueDate2.Height = 0.3937008F;
            this.lineHeaderVerArrivalDueDate2.Left = 8.892914F;
            this.lineHeaderVerArrivalDueDate2.LineWeight = 1F;
            this.lineHeaderVerArrivalDueDate2.Name = "lineHeaderVerArrivalDueDate2";
            this.lineHeaderVerArrivalDueDate2.Top = 0.9511812F;
            this.lineHeaderVerArrivalDueDate2.Width = 0F;
            this.lineHeaderVerArrivalDueDate2.X1 = 8.892914F;
            this.lineHeaderVerArrivalDueDate2.X2 = 8.892914F;
            this.lineHeaderVerArrivalDueDate2.Y1 = 0.9511812F;
            this.lineHeaderVerArrivalDueDate2.Y2 = 1.344882F;
            // 
            // lineHeaderVerArrivalDueDate3
            // 
            this.lineHeaderVerArrivalDueDate3.Height = 0.393701F;
            this.lineHeaderVerArrivalDueDate3.Left = 9.759056F;
            this.lineHeaderVerArrivalDueDate3.LineWeight = 1F;
            this.lineHeaderVerArrivalDueDate3.Name = "lineHeaderVerArrivalDueDate3";
            this.lineHeaderVerArrivalDueDate3.Top = 0.944882F;
            this.lineHeaderVerArrivalDueDate3.Width = 0F;
            this.lineHeaderVerArrivalDueDate3.X1 = 9.759056F;
            this.lineHeaderVerArrivalDueDate3.X2 = 9.759056F;
            this.lineHeaderVerArrivalDueDate3.Y1 = 0.944882F;
            this.lineHeaderVerArrivalDueDate3.Y2 = 1.338583F;
            // 
            // lineHeaderVerCreditAmount2
            // 
            this.lineHeaderVerCreditAmount2.Height = 0.393701F;
            this.lineHeaderVerCreditAmount2.Left = 3.601575F;
            this.lineHeaderVerCreditAmount2.LineWeight = 1F;
            this.lineHeaderVerCreditAmount2.Name = "lineHeaderVerCreditAmount2";
            this.lineHeaderVerCreditAmount2.Top = 0.944882F;
            this.lineHeaderVerCreditAmount2.Width = 0F;
            this.lineHeaderVerCreditAmount2.X1 = 3.601575F;
            this.lineHeaderVerCreditAmount2.X2 = 3.601575F;
            this.lineHeaderVerCreditAmount2.Y1 = 0.944882F;
            this.lineHeaderVerCreditAmount2.Y2 = 1.338583F;
            // 
            // lineHeaderVerUnsettleRemain1
            // 
            this.lineHeaderVerUnsettleRemain1.Height = 0.393701F;
            this.lineHeaderVerUnsettleRemain1.Left = 4.451969F;
            this.lineHeaderVerUnsettleRemain1.LineWeight = 1F;
            this.lineHeaderVerUnsettleRemain1.Name = "lineHeaderVerUnsettleRemain1";
            this.lineHeaderVerUnsettleRemain1.Top = 0.944882F;
            this.lineHeaderVerUnsettleRemain1.Width = 0F;
            this.lineHeaderVerUnsettleRemain1.X1 = 4.451969F;
            this.lineHeaderVerUnsettleRemain1.X2 = 4.451969F;
            this.lineHeaderVerUnsettleRemain1.Y1 = 0.944882F;
            this.lineHeaderVerUnsettleRemain1.Y2 = 1.338583F;
            // 
            // lineHeaderVerCreditRemain2
            // 
            this.lineHeaderVerCreditRemain2.Height = 0.393701F;
            this.lineHeaderVerCreditRemain2.Left = 7.181102F;
            this.lineHeaderVerCreditRemain2.LineWeight = 1F;
            this.lineHeaderVerCreditRemain2.Name = "lineHeaderVerCreditRemain2";
            this.lineHeaderVerCreditRemain2.Top = 0.944882F;
            this.lineHeaderVerCreditRemain2.Width = 0F;
            this.lineHeaderVerCreditRemain2.X1 = 7.181102F;
            this.lineHeaderVerCreditRemain2.X2 = 7.181102F;
            this.lineHeaderVerCreditRemain2.Y1 = 0.944882F;
            this.lineHeaderVerCreditRemain2.Y2 = 1.338583F;
            // 
            // lblDepartmentCodeAndName
            // 
            this.lblDepartmentCodeAndName.Height = 0.2F;
            this.lblDepartmentCodeAndName.HyperLink = null;
            this.lblDepartmentCodeAndName.Left = 1.417323F;
            this.lblDepartmentCodeAndName.Name = "lblDepartmentCodeAndName";
            this.lblDepartmentCodeAndName.Style = "font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDepartmentCodeAndName.Text = "";
            this.lblDepartmentCodeAndName.Top = 0.472441F;
            this.lblDepartmentCodeAndName.Width = 2.105118F;
            // 
            // lblStaffCodeAndName
            // 
            this.lblStaffCodeAndName.Height = 0.2F;
            this.lblStaffCodeAndName.HyperLink = null;
            this.lblStaffCodeAndName.Left = 1.417323F;
            this.lblStaffCodeAndName.Name = "lblStaffCodeAndName";
            this.lblStaffCodeAndName.Style = "font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblStaffCodeAndName.Text = "";
            this.lblStaffCodeAndName.Top = 0.6724409F;
            this.lblStaffCodeAndName.Width = 2.105118F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.2F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 0.8661418F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDepartmentCode.Text = "";
            this.lblDepartmentCode.Top = 0.472441F;
            this.lblDepartmentCode.Width = 0.5511811F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.2F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 0.8661418F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblStaffCode.Text = "";
            this.lblStaffCode.Top = 0.6724409F;
            this.lblStaffCode.Width = 0.5511811F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.9448819F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.62992F;
            this.lineHeaderHorUpper.Y1 = 0.9448819F;
            this.lineHeaderHorUpper.Y2 = 0.9448819F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCustomerCode,
            this.txtSubTotal,
            this.txtCollectCategory,
            this.txtCreditAmount,
            this.txtUnsettledRemain,
            this.txtBillingRemain,
            this.txtCreditLimit,
            this.txtCreditRemain,
            this.txtCreditRemainStar,
            this.txtArrivalDueDate1,
            this.txtArrivalDueDate2,
            this.txtArrivalDueDate3,
            this.txtArrivalDueDate4,
            this.lineDetailVerSubTotal,
            this.lineDetailVerCollectCategory,
            this.lineDetailVerCreditAmount2,
            this.lineDetailVerUnsettledRemain2,
            this.lineDetailVerBillingRemain,
            this.lineDetailVerCreditLimit1,
            this.lineDetailVerCreditRemain1,
            this.lineDetailVerArrivalDueDate1,
            this.lineDetailVerArrivalDueDate2,
            this.lineDetailVerArrivalDueDate3,
            this.lineDetailVerCreditAmount1,
            this.lineDetailVerUnsettledRemain1,
            this.lineDetailVerCreditRemain2,
            this.lineDetailHorLower});
            this.detail.Height = 0.2015748F;
            this.detail.Name = "detail";
            this.detail.Format += new System.EventHandler(this.detail_Format);
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 0F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-family: ＭＳ 明朝; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtCustomerCode.Text = "textBox1";
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 1.889764F;
            // 
            // txtSubTotal
            // 
            this.txtSubTotal.Height = 0.2F;
            this.txtSubTotal.Left = 1.889764F;
            this.txtSubTotal.Name = "txtSubTotal";
            this.txtSubTotal.Style = "font-size: 7pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtSubTotal.Text = "sub";
            this.txtSubTotal.Top = 0F;
            this.txtSubTotal.Width = 0.2952756F;
            // 
            // txtCollectCategory
            // 
            this.txtCollectCategory.Height = 0.2F;
            this.txtCollectCategory.Left = 2.170079F;
            this.txtCollectCategory.MultiLine = false;
            this.txtCollectCategory.Name = "txtCollectCategory";
            this.txtCollectCategory.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtCollectCategory.Text = "textBox1";
            this.txtCollectCategory.Top = 0F;
            this.txtCollectCategory.Width = 0.5496061F;
            // 
            // txtCreditAmount
            // 
            this.txtCreditAmount.Height = 0.2F;
            this.txtCreditAmount.Left = 2.719685F;
            this.txtCreditAmount.Name = "txtCreditAmount";
            this.txtCreditAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCreditAmount.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtCreditAmount.Text = "textBox1";
            this.txtCreditAmount.Top = 0F;
            this.txtCreditAmount.Width = 0.8661418F;
            // 
            // txtUnsettledRemain
            // 
            this.txtUnsettledRemain.Height = 0.2F;
            this.txtUnsettledRemain.Left = 3.585827F;
            this.txtUnsettledRemain.Name = "txtUnsettledRemain";
            this.txtUnsettledRemain.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUnsettledRemain.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUnsettledRemain.Text = "textBox1";
            this.txtUnsettledRemain.Top = 0F;
            this.txtUnsettledRemain.Width = 0.8661418F;
            // 
            // txtBillingRemain
            // 
            this.txtBillingRemain.Height = 0.2F;
            this.txtBillingRemain.Left = 4.451969F;
            this.txtBillingRemain.Name = "txtBillingRemain";
            this.txtBillingRemain.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtBillingRemain.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtBillingRemain.Text = "123,456,789,012";
            this.txtBillingRemain.Top = 0F;
            this.txtBillingRemain.Width = 0.8661418F;
            // 
            // txtCreditLimit
            // 
            this.txtCreditLimit.Height = 0.2F;
            this.txtCreditLimit.Left = 5.31811F;
            this.txtCreditLimit.Name = "txtCreditLimit";
            this.txtCreditLimit.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCreditLimit.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; text-justify: distribute; " +
    "vertical-align: middle; ddo-char-set: 1";
            this.txtCreditLimit.Text = "textBox1";
            this.txtCreditLimit.Top = 0F;
            this.txtCreditLimit.Width = 0.8661418F;
            // 
            // txtCreditRemain
            // 
            this.txtCreditRemain.Height = 0.2F;
            this.txtCreditRemain.Left = 6.184252F;
            this.txtCreditRemain.Name = "txtCreditRemain";
            this.txtCreditRemain.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCreditRemain.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtCreditRemain.Text = "textBox1";
            this.txtCreditRemain.Top = 0F;
            this.txtCreditRemain.Width = 0.8661418F;
            // 
            // txtCreditRemainStar
            // 
            this.txtCreditRemainStar.Height = 0.2F;
            this.txtCreditRemainStar.Left = 7.050394F;
            this.txtCreditRemainStar.Name = "txtCreditRemainStar";
            this.txtCreditRemainStar.Style = "font-size: 7pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCreditRemainStar.Text = "textBox1";
            this.txtCreditRemainStar.Top = 0F;
            this.txtCreditRemainStar.Width = 0.1149606F;
            // 
            // txtArrivalDueDate1
            // 
            this.txtArrivalDueDate1.Height = 0.2F;
            this.txtArrivalDueDate1.Left = 7.165355F;
            this.txtArrivalDueDate1.Name = "txtArrivalDueDate1";
            this.txtArrivalDueDate1.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtArrivalDueDate1.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtArrivalDueDate1.Text = "textBox1";
            this.txtArrivalDueDate1.Top = 0F;
            this.txtArrivalDueDate1.Width = 0.8661418F;
            // 
            // txtArrivalDueDate2
            // 
            this.txtArrivalDueDate2.Height = 0.2F;
            this.txtArrivalDueDate2.Left = 8.026772F;
            this.txtArrivalDueDate2.Name = "txtArrivalDueDate2";
            this.txtArrivalDueDate2.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtArrivalDueDate2.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtArrivalDueDate2.Text = "textBox1";
            this.txtArrivalDueDate2.Top = 0F;
            this.txtArrivalDueDate2.Width = 0.8661418F;
            // 
            // txtArrivalDueDate3
            // 
            this.txtArrivalDueDate3.Height = 0.2F;
            this.txtArrivalDueDate3.Left = 8.892914F;
            this.txtArrivalDueDate3.Name = "txtArrivalDueDate3";
            this.txtArrivalDueDate3.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtArrivalDueDate3.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtArrivalDueDate3.Text = "textBox1";
            this.txtArrivalDueDate3.Top = 0F;
            this.txtArrivalDueDate3.Width = 0.8661418F;
            // 
            // txtArrivalDueDate4
            // 
            this.txtArrivalDueDate4.Height = 0.2F;
            this.txtArrivalDueDate4.Left = 9.759056F;
            this.txtArrivalDueDate4.Name = "txtArrivalDueDate4";
            this.txtArrivalDueDate4.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtArrivalDueDate4.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtArrivalDueDate4.Text = "textBox1";
            this.txtArrivalDueDate4.Top = 0F;
            this.txtArrivalDueDate4.Width = 0.8661418F;
            // 
            // lineDetailVerSubTotal
            // 
            this.lineDetailVerSubTotal.Height = 0.2F;
            this.lineDetailVerSubTotal.Left = 2.170079F;
            this.lineDetailVerSubTotal.LineWeight = 1F;
            this.lineDetailVerSubTotal.Name = "lineDetailVerSubTotal";
            this.lineDetailVerSubTotal.Top = 0F;
            this.lineDetailVerSubTotal.Width = 0F;
            this.lineDetailVerSubTotal.X1 = 2.170079F;
            this.lineDetailVerSubTotal.X2 = 2.170079F;
            this.lineDetailVerSubTotal.Y1 = 0F;
            this.lineDetailVerSubTotal.Y2 = 0.2F;
            // 
            // lineDetailVerCollectCategory
            // 
            this.lineDetailVerCollectCategory.Height = 0.2F;
            this.lineDetailVerCollectCategory.Left = 2.719685F;
            this.lineDetailVerCollectCategory.LineWeight = 1F;
            this.lineDetailVerCollectCategory.Name = "lineDetailVerCollectCategory";
            this.lineDetailVerCollectCategory.Top = 0F;
            this.lineDetailVerCollectCategory.Width = 0F;
            this.lineDetailVerCollectCategory.X1 = 2.719685F;
            this.lineDetailVerCollectCategory.X2 = 2.719685F;
            this.lineDetailVerCollectCategory.Y1 = 0F;
            this.lineDetailVerCollectCategory.Y2 = 0.2F;
            // 
            // lineDetailVerCreditAmount2
            // 
            this.lineDetailVerCreditAmount2.Height = 0.2F;
            this.lineDetailVerCreditAmount2.Left = 3.601575F;
            this.lineDetailVerCreditAmount2.LineWeight = 1F;
            this.lineDetailVerCreditAmount2.Name = "lineDetailVerCreditAmount2";
            this.lineDetailVerCreditAmount2.Top = 0F;
            this.lineDetailVerCreditAmount2.Width = 0F;
            this.lineDetailVerCreditAmount2.X1 = 3.601575F;
            this.lineDetailVerCreditAmount2.X2 = 3.601575F;
            this.lineDetailVerCreditAmount2.Y1 = 0F;
            this.lineDetailVerCreditAmount2.Y2 = 0.2F;
            // 
            // lineDetailVerUnsettledRemain2
            // 
            this.lineDetailVerUnsettledRemain2.Height = 0.2F;
            this.lineDetailVerUnsettledRemain2.Left = 4.467716F;
            this.lineDetailVerUnsettledRemain2.LineWeight = 1F;
            this.lineDetailVerUnsettledRemain2.Name = "lineDetailVerUnsettledRemain2";
            this.lineDetailVerUnsettledRemain2.Top = 0F;
            this.lineDetailVerUnsettledRemain2.Width = 0F;
            this.lineDetailVerUnsettledRemain2.X1 = 4.467716F;
            this.lineDetailVerUnsettledRemain2.X2 = 4.467716F;
            this.lineDetailVerUnsettledRemain2.Y1 = 0F;
            this.lineDetailVerUnsettledRemain2.Y2 = 0.2F;
            // 
            // lineDetailVerBillingRemain
            // 
            this.lineDetailVerBillingRemain.Height = 0.2F;
            this.lineDetailVerBillingRemain.Left = 5.31811F;
            this.lineDetailVerBillingRemain.LineWeight = 1F;
            this.lineDetailVerBillingRemain.Name = "lineDetailVerBillingRemain";
            this.lineDetailVerBillingRemain.Top = 0F;
            this.lineDetailVerBillingRemain.Width = 0F;
            this.lineDetailVerBillingRemain.X1 = 5.31811F;
            this.lineDetailVerBillingRemain.X2 = 5.31811F;
            this.lineDetailVerBillingRemain.Y1 = 0F;
            this.lineDetailVerBillingRemain.Y2 = 0.2F;
            // 
            // lineDetailVerCreditLimit1
            // 
            this.lineDetailVerCreditLimit1.Height = 0.2F;
            this.lineDetailVerCreditLimit1.Left = 6.184252F;
            this.lineDetailVerCreditLimit1.LineWeight = 1F;
            this.lineDetailVerCreditLimit1.Name = "lineDetailVerCreditLimit1";
            this.lineDetailVerCreditLimit1.Top = 0F;
            this.lineDetailVerCreditLimit1.Width = 0F;
            this.lineDetailVerCreditLimit1.X1 = 6.184252F;
            this.lineDetailVerCreditLimit1.X2 = 6.184252F;
            this.lineDetailVerCreditLimit1.Y1 = 0F;
            this.lineDetailVerCreditLimit1.Y2 = 0.2F;
            // 
            // lineDetailVerCreditRemain1
            // 
            this.lineDetailVerCreditRemain1.Height = 0.2F;
            this.lineDetailVerCreditRemain1.Left = 7.165355F;
            this.lineDetailVerCreditRemain1.LineWeight = 1F;
            this.lineDetailVerCreditRemain1.Name = "lineDetailVerCreditRemain1";
            this.lineDetailVerCreditRemain1.Top = 0F;
            this.lineDetailVerCreditRemain1.Width = 0F;
            this.lineDetailVerCreditRemain1.X1 = 7.165355F;
            this.lineDetailVerCreditRemain1.X2 = 7.165355F;
            this.lineDetailVerCreditRemain1.Y1 = 0F;
            this.lineDetailVerCreditRemain1.Y2 = 0.2F;
            // 
            // lineDetailVerArrivalDueDate1
            // 
            this.lineDetailVerArrivalDueDate1.Height = 0.2F;
            this.lineDetailVerArrivalDueDate1.Left = 8.026772F;
            this.lineDetailVerArrivalDueDate1.LineWeight = 1F;
            this.lineDetailVerArrivalDueDate1.Name = "lineDetailVerArrivalDueDate1";
            this.lineDetailVerArrivalDueDate1.Top = 0F;
            this.lineDetailVerArrivalDueDate1.Width = 0F;
            this.lineDetailVerArrivalDueDate1.X1 = 8.026772F;
            this.lineDetailVerArrivalDueDate1.X2 = 8.026772F;
            this.lineDetailVerArrivalDueDate1.Y1 = 0F;
            this.lineDetailVerArrivalDueDate1.Y2 = 0.2F;
            // 
            // lineDetailVerArrivalDueDate2
            // 
            this.lineDetailVerArrivalDueDate2.Height = 0.2F;
            this.lineDetailVerArrivalDueDate2.Left = 8.892914F;
            this.lineDetailVerArrivalDueDate2.LineWeight = 1F;
            this.lineDetailVerArrivalDueDate2.Name = "lineDetailVerArrivalDueDate2";
            this.lineDetailVerArrivalDueDate2.Top = 0F;
            this.lineDetailVerArrivalDueDate2.Width = 0F;
            this.lineDetailVerArrivalDueDate2.X1 = 8.892914F;
            this.lineDetailVerArrivalDueDate2.X2 = 8.892914F;
            this.lineDetailVerArrivalDueDate2.Y1 = 0F;
            this.lineDetailVerArrivalDueDate2.Y2 = 0.2F;
            // 
            // lineDetailVerArrivalDueDate3
            // 
            this.lineDetailVerArrivalDueDate3.Height = 0.2F;
            this.lineDetailVerArrivalDueDate3.Left = 9.763781F;
            this.lineDetailVerArrivalDueDate3.LineWeight = 1F;
            this.lineDetailVerArrivalDueDate3.Name = "lineDetailVerArrivalDueDate3";
            this.lineDetailVerArrivalDueDate3.Top = 0F;
            this.lineDetailVerArrivalDueDate3.Width = 0F;
            this.lineDetailVerArrivalDueDate3.X1 = 9.763781F;
            this.lineDetailVerArrivalDueDate3.X2 = 9.763781F;
            this.lineDetailVerArrivalDueDate3.Y1 = 0F;
            this.lineDetailVerArrivalDueDate3.Y2 = 0.2F;
            // 
            // lineDetailVerCreditAmount1
            // 
            this.lineDetailVerCreditAmount1.Height = 0.2F;
            this.lineDetailVerCreditAmount1.Left = 3.585827F;
            this.lineDetailVerCreditAmount1.LineWeight = 1F;
            this.lineDetailVerCreditAmount1.Name = "lineDetailVerCreditAmount1";
            this.lineDetailVerCreditAmount1.Top = 0F;
            this.lineDetailVerCreditAmount1.Width = 0F;
            this.lineDetailVerCreditAmount1.X1 = 3.585827F;
            this.lineDetailVerCreditAmount1.X2 = 3.585827F;
            this.lineDetailVerCreditAmount1.Y1 = 0F;
            this.lineDetailVerCreditAmount1.Y2 = 0.2F;
            // 
            // lineDetailVerUnsettledRemain1
            // 
            this.lineDetailVerUnsettledRemain1.Height = 0.2F;
            this.lineDetailVerUnsettledRemain1.Left = 4.451969F;
            this.lineDetailVerUnsettledRemain1.LineWeight = 1F;
            this.lineDetailVerUnsettledRemain1.Name = "lineDetailVerUnsettledRemain1";
            this.lineDetailVerUnsettledRemain1.Top = 0F;
            this.lineDetailVerUnsettledRemain1.Width = 0F;
            this.lineDetailVerUnsettledRemain1.X1 = 4.451969F;
            this.lineDetailVerUnsettledRemain1.X2 = 4.451969F;
            this.lineDetailVerUnsettledRemain1.Y1 = 0F;
            this.lineDetailVerUnsettledRemain1.Y2 = 0.2F;
            // 
            // lineDetailVerCreditRemain2
            // 
            this.lineDetailVerCreditRemain2.Height = 0.2F;
            this.lineDetailVerCreditRemain2.Left = 7.181102F;
            this.lineDetailVerCreditRemain2.LineWeight = 1F;
            this.lineDetailVerCreditRemain2.Name = "lineDetailVerCreditRemain2";
            this.lineDetailVerCreditRemain2.Top = 0F;
            this.lineDetailVerCreditRemain2.Width = 0F;
            this.lineDetailVerCreditRemain2.X1 = 7.181102F;
            this.lineDetailVerCreditRemain2.X2 = 7.181102F;
            this.lineDetailVerCreditRemain2.Y1 = 0.2F;
            this.lineDetailVerCreditRemain2.Y2 = 0F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.2F;
            this.lineDetailHorLower.Width = 10.62993F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.62993F;
            this.lineDetailHorLower.Y1 = 0.2F;
            this.lineDetailHorLower.Y2 = 0.2F;
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
            this.reportInfo1.Left = 7.952756F;
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
            // CreditAgingListReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.63F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " +
            "color: Black; font-family: \"ＭＳ 明朝\"; ddo-char-set: 186", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCollectCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUnsettleRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreditRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrivalDueDate1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrivalDueDate2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrivalDueDate3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrivalDueDate4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCodeAndName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCodeAndName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnsettledRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditRemain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditRemainStar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrivalDueDate1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrivalDueDate2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrivalDueDate3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrivalDueDate4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaff;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCollectCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCollectCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditAmount1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerUnsettleRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblUnsettleRemain;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingRemain;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditLimit;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreditRemain;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSubTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCollectCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditAmount2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerUnsettledRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingRemain;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditLimit1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerArrivalDueDate1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerArrivalDueDate2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerArrivalDueDate3;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCollectCategory;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUnsettledRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditLimit;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtArrivalDueDate1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtArrivalDueDate2;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtArrivalDueDate3;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtArrivalDueDate4;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingRemain;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditLimit1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerArrivalDueDate1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerArrivalDueDate2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerArrivalDueDate3;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditAmount2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerUnsettleRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreditRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditAmount1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerUnsettledRemain1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreditRemain2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCodeAndName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCodeAndName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreditRemainStar;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingRemain;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSubTotal;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblArrivalDueDate1;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblArrivalDueDate2;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblArrivalDueDate3;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblArrivalDueDate4;
    }
}
