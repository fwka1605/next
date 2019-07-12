namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillSectionReport の概要の説明です。
    /// </summary>
    partial class BillingOmitSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BillingOmitSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblAssignmentFlag = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSalesAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingId = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.llbBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInputType = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceNo = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffname = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRemainingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerInvoiceNo = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorBillingId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtSalesAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInputType = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtBillingID = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtClosingAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAssignmentFlag = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerBillingId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerInvoiceNo = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtRemainGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerBillingGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerRemainGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblAssignmentFlag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.llbBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAssignmentFlag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainGrandTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingGrandTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrandTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblAssignmentFlag,
            this.lblCustomerName,
            this.lblSalesAt,
            this.lblDueAt,
            this.lblClosingAt,
            this.lblBilledAt,
            this.lblCustomerCode,
            this.lblBillingId,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.llbBillingAmount,
            this.lblRemainAmount,
            this.lblCategoryCode,
            this.lblInputType,
            this.lblInvoiceNo,
            this.lblNote1,
            this.lblDepartmentCode,
            this.lblDepartmentName,
            this.lblStaffCode,
            this.lblStaffname,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderVerBilledAt,
            this.lineHeaderVerClosingAt,
            this.lineHeaderVerBillingAmount,
            this.lineHeaderVerRemainingAmount,
            this.lineHeaderVerCategoryCode,
            this.lineHeaderVerInvoiceNo,
            this.lineHeaderVerDepartmentCode,
            this.lineHeaderVerBillingId,
            this.lineHeaderHorLower,
            this.lineHeaderHorUpper,
            this.lineHeaderHorCategoryCode,
            this.lineHeaderHorBillingId});
            this.pageHeader.Height = 1.111811F;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.BeforePrint += new System.EventHandler(this.pageHeader_BeforePrint);
            // 
            // lblAssignmentFlag
            // 
            this.lblAssignmentFlag.Height = 0.25F;
            this.lblAssignmentFlag.HyperLink = null;
            this.lblAssignmentFlag.Left = 0F;
            this.lblAssignmentFlag.Name = "lblAssignmentFlag";
            this.lblAssignmentFlag.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblAssignmentFlag.Text = "消込区分";
            this.lblAssignmentFlag.Top = 0.8681103F;
            this.lblAssignmentFlag.Width = 0.738189F;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Height = 0.25F;
            this.lblCustomerName.HyperLink = null;
            this.lblCustomerName.Left = 0.738189F;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.Top = 0.8681103F;
            this.lblCustomerName.Width = 1.135827F;
            // 
            // lblSalesAt
            // 
            this.lblSalesAt.Height = 0.25F;
            this.lblSalesAt.HyperLink = null;
            this.lblSalesAt.Left = 1.874016F;
            this.lblSalesAt.Name = "lblSalesAt";
            this.lblSalesAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblSalesAt.Text = "売上日";
            this.lblSalesAt.Top = 0.8681103F;
            this.lblSalesAt.Width = 0.9055118F;
            // 
            // lblDueAt
            // 
            this.lblDueAt.Height = 0.25F;
            this.lblDueAt.HyperLink = null;
            this.lblDueAt.Left = 2.777953F;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDueAt.Text = "入金予定日";
            this.lblDueAt.Top = 0.8681103F;
            this.lblDueAt.Width = 0.8799213F;
            // 
            // lblClosingAt
            // 
            this.lblClosingAt.Height = 0.25F;
            this.lblClosingAt.HyperLink = null;
            this.lblClosingAt.Left = 2.770079F;
            this.lblClosingAt.Name = "lblClosingAt";
            this.lblClosingAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblClosingAt.Text = "請求締日";
            this.lblClosingAt.Top = 0.6181102F;
            this.lblClosingAt.Width = 0.8799213F;
            // 
            // lblBilledAt
            // 
            this.lblBilledAt.Height = 0.25F;
            this.lblBilledAt.HyperLink = null;
            this.lblBilledAt.Left = 1.874016F;
            this.lblBilledAt.Name = "lblBilledAt";
            this.lblBilledAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblBilledAt.Text = "請求日";
            this.lblBilledAt.Top = 0.6181102F;
            this.lblBilledAt.Width = 0.9055118F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.25F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 0.738189F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.6181102F;
            this.lblCustomerCode.Width = 1.135827F;
            // 
            // lblBillingId
            // 
            this.lblBillingId.Height = 0.25F;
            this.lblBillingId.HyperLink = null;
            this.lblBillingId.Left = 0F;
            this.lblBillingId.Name = "lblBillingId";
            this.lblBillingId.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblBillingId.Text = "請求ID";
            this.lblBillingId.Top = 0.6181102F;
            this.lblBillingId.Width = 0.738189F;
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
            this.lblTitle.Text = "請求データ一覧";
            this.lblTitle.Top = 0.2704724F;
            this.lblTitle.Width = 10.62992F;
            // 
            // llbBillingAmount
            // 
            this.llbBillingAmount.Height = 0.496063F;
            this.llbBillingAmount.HyperLink = null;
            this.llbBillingAmount.Left = 3.65F;
            this.llbBillingAmount.Name = "llbBillingAmount";
            this.llbBillingAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.llbBillingAmount.Text = "請求金額（税込）";
            this.llbBillingAmount.Top = 0.6181102F;
            this.llbBillingAmount.Width = 1.240157F;
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Height = 0.496063F;
            this.lblRemainAmount.HyperLink = null;
            this.lblRemainAmount.Left = 4.890158F;
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblRemainAmount.Text = "請求残";
            this.lblRemainAmount.Top = 0.6181102F;
            this.lblRemainAmount.Width = 1.259843F;
            // 
            // lblCategoryCode
            // 
            this.lblCategoryCode.Height = 0.25F;
            this.lblCategoryCode.HyperLink = null;
            this.lblCategoryCode.Left = 6.155F;
            this.lblCategoryCode.Name = "lblCategoryCode";
            this.lblCategoryCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCategoryCode.Text = "請求区分";
            this.lblCategoryCode.Top = 0.6181102F;
            this.lblCategoryCode.Width = 1.036F;
            // 
            // lblInputType
            // 
            this.lblInputType.Height = 0.25F;
            this.lblInputType.HyperLink = null;
            this.lblInputType.Left = 6.155118F;
            this.lblInputType.Name = "lblInputType";
            this.lblInputType.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.lblInputType.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblInputType.Text = "入力区分";
            this.lblInputType.Top = 0.8681103F;
            this.lblInputType.Width = 1.036F;
            // 
            // lblInvoiceNo
            // 
            this.lblInvoiceNo.Height = 0.25F;
            this.lblInvoiceNo.HyperLink = null;
            this.lblInvoiceNo.Left = 7.190945F;
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblInvoiceNo.Text = "請求書番号";
            this.lblInvoiceNo.Top = 0.6181102F;
            this.lblInvoiceNo.Width = 1.055F;
            // 
            // lblNote1
            // 
            this.lblNote1.Height = 0.25F;
            this.lblNote1.HyperLink = null;
            this.lblNote1.Left = 7.190945F;
            this.lblNote1.Name = "lblNote1";
            this.lblNote1.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblNote1.Text = "備考";
            this.lblNote1.Top = 0.8681103F;
            this.lblNote1.Width = 1.055F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.25F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 8.246063F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentCode.Text = "請求部門コード";
            this.lblDepartmentCode.Top = 0.6181102F;
            this.lblDepartmentCode.Width = 1.193F;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Height = 0.25F;
            this.lblDepartmentName.HyperLink = null;
            this.lblDepartmentName.Left = 8.246063F;
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentName.Text = "請求部門名";
            this.lblDepartmentName.Top = 0.8681103F;
            this.lblDepartmentName.Width = 1.193F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.25F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 9.438976F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblStaffCode.Text = "担当者コード";
            this.lblStaffCode.Top = 0.6181102F;
            this.lblStaffCode.Width = 1.188976F;
            // 
            // lblStaffname
            // 
            this.lblStaffname.Height = 0.25F;
            this.lblStaffname.HyperLink = null;
            this.lblStaffname.Left = 9.438977F;
            this.lblStaffname.Name = "lblStaffname";
            this.lblStaffname.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblStaffname.Text = "担当者名";
            this.lblStaffname.Top = 0.8681103F;
            this.lblStaffname.Width = 1.188976F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.4976379F;
            this.lineHeaderVerCustomerCode.Left = 1.874F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.6141732F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 1.874F;
            this.lineHeaderVerCustomerCode.X2 = 1.874F;
            this.lineHeaderVerCustomerCode.Y1 = 0.6141732F;
            this.lineHeaderVerCustomerCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerBilledAt
            // 
            this.lineHeaderVerBilledAt.Height = 0.4976377F;
            this.lineHeaderVerBilledAt.Left = 2.777953F;
            this.lineHeaderVerBilledAt.LineWeight = 1F;
            this.lineHeaderVerBilledAt.Name = "lineHeaderVerBilledAt";
            this.lineHeaderVerBilledAt.Top = 0.6141733F;
            this.lineHeaderVerBilledAt.Width = 0F;
            this.lineHeaderVerBilledAt.X1 = 2.777953F;
            this.lineHeaderVerBilledAt.X2 = 2.777953F;
            this.lineHeaderVerBilledAt.Y1 = 0.6141733F;
            this.lineHeaderVerBilledAt.Y2 = 1.111811F;
            // 
            // lineHeaderVerClosingAt
            // 
            this.lineHeaderVerClosingAt.Height = 0.4976379F;
            this.lineHeaderVerClosingAt.Left = 3.65F;
            this.lineHeaderVerClosingAt.LineWeight = 1F;
            this.lineHeaderVerClosingAt.Name = "lineHeaderVerClosingAt";
            this.lineHeaderVerClosingAt.Top = 0.6141732F;
            this.lineHeaderVerClosingAt.Width = 0F;
            this.lineHeaderVerClosingAt.X1 = 3.65F;
            this.lineHeaderVerClosingAt.X2 = 3.65F;
            this.lineHeaderVerClosingAt.Y1 = 0.6141732F;
            this.lineHeaderVerClosingAt.Y2 = 1.111811F;
            // 
            // lineHeaderVerBillingAmount
            // 
            this.lineHeaderVerBillingAmount.Height = 0.4976379F;
            this.lineHeaderVerBillingAmount.Left = 4.890158F;
            this.lineHeaderVerBillingAmount.LineWeight = 1F;
            this.lineHeaderVerBillingAmount.Name = "lineHeaderVerBillingAmount";
            this.lineHeaderVerBillingAmount.Top = 0.6141732F;
            this.lineHeaderVerBillingAmount.Width = 0F;
            this.lineHeaderVerBillingAmount.X1 = 4.890158F;
            this.lineHeaderVerBillingAmount.X2 = 4.890158F;
            this.lineHeaderVerBillingAmount.Y1 = 0.6141732F;
            this.lineHeaderVerBillingAmount.Y2 = 1.111811F;
            // 
            // lineHeaderVerRemainingAmount
            // 
            this.lineHeaderVerRemainingAmount.Height = 0.4976379F;
            this.lineHeaderVerRemainingAmount.Left = 6.155118F;
            this.lineHeaderVerRemainingAmount.LineWeight = 1F;
            this.lineHeaderVerRemainingAmount.Name = "lineHeaderVerRemainingAmount";
            this.lineHeaderVerRemainingAmount.Top = 0.6141732F;
            this.lineHeaderVerRemainingAmount.Width = 0F;
            this.lineHeaderVerRemainingAmount.X1 = 6.155118F;
            this.lineHeaderVerRemainingAmount.X2 = 6.155118F;
            this.lineHeaderVerRemainingAmount.Y1 = 0.6141732F;
            this.lineHeaderVerRemainingAmount.Y2 = 1.111811F;
            // 
            // lineHeaderVerCategoryCode
            // 
            this.lineHeaderVerCategoryCode.Height = 0.4976379F;
            this.lineHeaderVerCategoryCode.Left = 7.190945F;
            this.lineHeaderVerCategoryCode.LineWeight = 1F;
            this.lineHeaderVerCategoryCode.Name = "lineHeaderVerCategoryCode";
            this.lineHeaderVerCategoryCode.Top = 0.6141732F;
            this.lineHeaderVerCategoryCode.Width = 0F;
            this.lineHeaderVerCategoryCode.X1 = 7.190945F;
            this.lineHeaderVerCategoryCode.X2 = 7.190945F;
            this.lineHeaderVerCategoryCode.Y1 = 0.6141732F;
            this.lineHeaderVerCategoryCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerInvoiceNo
            // 
            this.lineHeaderVerInvoiceNo.Height = 0.4976379F;
            this.lineHeaderVerInvoiceNo.Left = 8.246063F;
            this.lineHeaderVerInvoiceNo.LineWeight = 1F;
            this.lineHeaderVerInvoiceNo.Name = "lineHeaderVerInvoiceNo";
            this.lineHeaderVerInvoiceNo.Top = 0.6141732F;
            this.lineHeaderVerInvoiceNo.Width = 0F;
            this.lineHeaderVerInvoiceNo.X1 = 8.246063F;
            this.lineHeaderVerInvoiceNo.X2 = 8.246063F;
            this.lineHeaderVerInvoiceNo.Y1 = 0.6141732F;
            this.lineHeaderVerInvoiceNo.Y2 = 1.111811F;
            // 
            // lineHeaderVerDepartmentCode
            // 
            this.lineHeaderVerDepartmentCode.Height = 0.4976379F;
            this.lineHeaderVerDepartmentCode.Left = 9.439F;
            this.lineHeaderVerDepartmentCode.LineWeight = 1F;
            this.lineHeaderVerDepartmentCode.Name = "lineHeaderVerDepartmentCode";
            this.lineHeaderVerDepartmentCode.Top = 0.6141732F;
            this.lineHeaderVerDepartmentCode.Width = 0F;
            this.lineHeaderVerDepartmentCode.X1 = 9.439F;
            this.lineHeaderVerDepartmentCode.X2 = 9.439F;
            this.lineHeaderVerDepartmentCode.Y1 = 0.6141732F;
            this.lineHeaderVerDepartmentCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerBillingId
            // 
            this.lineHeaderVerBillingId.Height = 0.4976379F;
            this.lineHeaderVerBillingId.Left = 0.738189F;
            this.lineHeaderVerBillingId.LineWeight = 1F;
            this.lineHeaderVerBillingId.Name = "lineHeaderVerBillingId";
            this.lineHeaderVerBillingId.Top = 0.6141732F;
            this.lineHeaderVerBillingId.Width = 0F;
            this.lineHeaderVerBillingId.X1 = 0.738189F;
            this.lineHeaderVerBillingId.X2 = 0.738189F;
            this.lineHeaderVerBillingId.Y1 = 0.6141732F;
            this.lineHeaderVerBillingId.Y2 = 1.111811F;
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
            // lineHeaderHorCategoryCode
            // 
            this.lineHeaderHorCategoryCode.Height = 1.013279E-06F;
            this.lineHeaderHorCategoryCode.Left = 6.151575F;
            this.lineHeaderHorCategoryCode.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorCategoryCode.LineWeight = 1F;
            this.lineHeaderHorCategoryCode.Name = "lineHeaderHorCategoryCode";
            this.lineHeaderHorCategoryCode.Top = 0.8622047F;
            this.lineHeaderHorCategoryCode.Width = 4.478345F;
            this.lineHeaderHorCategoryCode.X1 = 6.151575F;
            this.lineHeaderHorCategoryCode.X2 = 10.62992F;
            this.lineHeaderHorCategoryCode.Y1 = 0.8622057F;
            this.lineHeaderHorCategoryCode.Y2 = 0.8622047F;
            // 
            // lineHeaderHorBillingId
            // 
            this.lineHeaderHorBillingId.Height = 0F;
            this.lineHeaderHorBillingId.Left = 0F;
            this.lineHeaderHorBillingId.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorBillingId.LineWeight = 1F;
            this.lineHeaderHorBillingId.Name = "lineHeaderHorBillingId";
            this.lineHeaderHorBillingId.Top = 0.8622047F;
            this.lineHeaderHorBillingId.Width = 3.65F;
            this.lineHeaderHorBillingId.X1 = 0F;
            this.lineHeaderHorBillingId.X2 = 3.65F;
            this.lineHeaderHorBillingId.Y1 = 0.8622047F;
            this.lineHeaderHorBillingId.Y2 = 0.8622047F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtSalesAt,
            this.txtBilledAt,
            this.txtInputType,
            this.txtCategoryCode,
            this.txtNote1,
            this.txtInvoiceCode,
            this.txtStaffName,
            this.txtStaffCode,
            this.txtCustomerName,
            this.txtBillingAmount,
            this.txtRemainAmount,
            this.lineDetailVerRemainAmount,
            this.lineDetailVerCategoryCode,
            this.lineDetailVerDepartmentCode,
            this.txtBillingID,
            this.lineDetailVerBilledAt,
            this.lineDetailVerBillingAmount,
            this.txtDepartmentName,
            this.txtClosingAt,
            this.txtCustomerCode,
            this.txtDepartmentCode,
            this.txtDueAt,
            this.txtAssignmentFlag,
            this.lineDetailVerBillingId,
            this.lineDetailVerInvoiceNo,
            this.lineDetailVerCustomerCode,
            this.lineDetailVerClosingAt,
            this.lineDetailHorLower});
            this.detail.Height = 0.4598425F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtSalesAt
            // 
            this.txtSalesAt.DataField = "SalesAt";
            this.txtSalesAt.Height = 0.2244094F;
            this.txtSalesAt.Left = 1.874016F;
            this.txtSalesAt.Name = "txtSalesAt";
            this.txtSalesAt.OutputFormat = resources.GetString("txtSalesAt.OutputFormat");
            this.txtSalesAt.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtSalesAt.Text = null;
            this.txtSalesAt.Top = 0.234252F;
            this.txtSalesAt.Width = 0.9055118F;
            // 
            // txtBilledAt
            // 
            this.txtBilledAt.DataField = "BilledAt";
            this.txtBilledAt.Height = 0.2244094F;
            this.txtBilledAt.Left = 1.874016F;
            this.txtBilledAt.Name = "txtBilledAt";
            this.txtBilledAt.OutputFormat = resources.GetString("txtBilledAt.OutputFormat");
            this.txtBilledAt.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtBilledAt.Text = null;
            this.txtBilledAt.Top = 1.490116E-08F;
            this.txtBilledAt.Width = 0.9055118F;
            // 
            // txtInputType
            // 
            this.txtInputType.DataField = "InputTypeName";
            this.txtInputType.Height = 0.2244094F;
            this.txtInputType.Left = 6.155118F;
            this.txtInputType.Name = "txtInputType";
            this.txtInputType.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtInputType.Text = null;
            this.txtInputType.Top = 0.234252F;
            this.txtInputType.Width = 1.035827F;
            // 
            // txtCategoryCode
            // 
            this.txtCategoryCode.DataField = "BillingCategoryCodeAndName";
            this.txtCategoryCode.Height = 0.2244094F;
            this.txtCategoryCode.Left = 6.155118F;
            this.txtCategoryCode.Name = "txtCategoryCode";
            this.txtCategoryCode.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtCategoryCode.Text = null;
            this.txtCategoryCode.Top = 0F;
            this.txtCategoryCode.Width = 1.035827F;
            // 
            // txtNote1
            // 
            this.txtNote1.DataField = "Note1";
            this.txtNote1.Height = 0.2244094F;
            this.txtNote1.Left = 7.190945F;
            this.txtNote1.Name = "txtNote1";
            this.txtNote1.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtNote1.Text = null;
            this.txtNote1.Top = 0.234252F;
            this.txtNote1.Width = 1.047244F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.DataField = "InvoiceCode";
            this.txtInvoiceCode.Height = 0.2244094F;
            this.txtInvoiceCode.Left = 7.190945F;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtInvoiceCode.Text = null;
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 1.047244F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.DataField = "StaffName";
            this.txtStaffName.Height = 0.2244094F;
            this.txtStaffName.Left = 9.438977F;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtStaffName.Text = null;
            this.txtStaffName.Top = 0.234252F;
            this.txtStaffName.Width = 1.188976F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.DataField = "StaffCode";
            this.txtStaffCode.Height = 0.2244094F;
            this.txtStaffCode.Left = 9.438977F;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtStaffCode.Text = null;
            this.txtStaffCode.Top = 0F;
            this.txtStaffCode.Width = 1.188976F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.DataField = "CustomerName";
            this.txtCustomerName.Height = 0.2244094F;
            this.txtCustomerName.Left = 0.738189F;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Style = "text-align: left; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: now" +
    "rap";
            this.txtCustomerName.Text = null;
            this.txtCustomerName.Top = 0.234252F;
            this.txtCustomerName.Width = 1.135827F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.DataField = "BillingAmount";
            this.txtBillingAmount.Height = 0.4598425F;
            this.txtBillingAmount.Left = 3.65F;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtBillingAmount.Style = "text-align: right; vertical-align: middle; white-space: inherit; ddo-wrap-mode: i" +
    "nherit";
            this.txtBillingAmount.Text = "98,765,432,100";
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 1.240157F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.DataField = "RemainAmount";
            this.txtRemainAmount.Height = 0.4598425F;
            this.txtRemainAmount.Left = 4.890158F;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtRemainAmount.Style = "text-align: right; text-justify: auto; vertical-align: middle; white-space: inher" +
    "it; ddo-wrap-mode: inherit";
            this.txtRemainAmount.Text = "98,765,432,100";
            this.txtRemainAmount.Top = 0F;
            this.txtRemainAmount.Width = 1.259843F;
            // 
            // lineDetailVerRemainAmount
            // 
            this.lineDetailVerRemainAmount.Height = 0.4598425F;
            this.lineDetailVerRemainAmount.Left = 6.155118F;
            this.lineDetailVerRemainAmount.LineWeight = 1F;
            this.lineDetailVerRemainAmount.Name = "lineDetailVerRemainAmount";
            this.lineDetailVerRemainAmount.Top = 0F;
            this.lineDetailVerRemainAmount.Width = 0.0001177788F;
            this.lineDetailVerRemainAmount.X1 = 6.155118F;
            this.lineDetailVerRemainAmount.X2 = 6.155236F;
            this.lineDetailVerRemainAmount.Y1 = 0F;
            this.lineDetailVerRemainAmount.Y2 = 0.4598425F;
            // 
            // lineDetailVerCategoryCode
            // 
            this.lineDetailVerCategoryCode.Height = 0.4598425F;
            this.lineDetailVerCategoryCode.Left = 7.191F;
            this.lineDetailVerCategoryCode.LineWeight = 1F;
            this.lineDetailVerCategoryCode.Name = "lineDetailVerCategoryCode";
            this.lineDetailVerCategoryCode.Top = 0F;
            this.lineDetailVerCategoryCode.Width = 0F;
            this.lineDetailVerCategoryCode.X1 = 7.191F;
            this.lineDetailVerCategoryCode.X2 = 7.191F;
            this.lineDetailVerCategoryCode.Y1 = 0F;
            this.lineDetailVerCategoryCode.Y2 = 0.4598425F;
            // 
            // lineDetailVerDepartmentCode
            // 
            this.lineDetailVerDepartmentCode.Height = 0.4598425F;
            this.lineDetailVerDepartmentCode.Left = 9.439F;
            this.lineDetailVerDepartmentCode.LineWeight = 1F;
            this.lineDetailVerDepartmentCode.Name = "lineDetailVerDepartmentCode";
            this.lineDetailVerDepartmentCode.Top = 0F;
            this.lineDetailVerDepartmentCode.Width = 0F;
            this.lineDetailVerDepartmentCode.X1 = 9.439F;
            this.lineDetailVerDepartmentCode.X2 = 9.439F;
            this.lineDetailVerDepartmentCode.Y1 = 0F;
            this.lineDetailVerDepartmentCode.Y2 = 0.4598425F;
            // 
            // txtBillingID
            // 
            this.txtBillingID.DataField = "Id";
            this.txtBillingID.Height = 0.2244094F;
            this.txtBillingID.Left = 0F;
            this.txtBillingID.Name = "txtBillingID";
            this.txtBillingID.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtBillingID.Style = "text-align: right; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: no" +
    "wrap";
            this.txtBillingID.Text = null;
            this.txtBillingID.Top = 0F;
            this.txtBillingID.Width = 0.738189F;
            // 
            // lineDetailVerBilledAt
            // 
            this.lineDetailVerBilledAt.Height = 0.4598425F;
            this.lineDetailVerBilledAt.Left = 2.777953F;
            this.lineDetailVerBilledAt.LineWeight = 1F;
            this.lineDetailVerBilledAt.Name = "lineDetailVerBilledAt";
            this.lineDetailVerBilledAt.Top = 0F;
            this.lineDetailVerBilledAt.Width = 0F;
            this.lineDetailVerBilledAt.X1 = 2.777953F;
            this.lineDetailVerBilledAt.X2 = 2.777953F;
            this.lineDetailVerBilledAt.Y1 = 0F;
            this.lineDetailVerBilledAt.Y2 = 0.4598425F;
            // 
            // lineDetailVerBillingAmount
            // 
            this.lineDetailVerBillingAmount.Height = 0.4598425F;
            this.lineDetailVerBillingAmount.Left = 4.890158F;
            this.lineDetailVerBillingAmount.LineWeight = 1F;
            this.lineDetailVerBillingAmount.Name = "lineDetailVerBillingAmount";
            this.lineDetailVerBillingAmount.Top = 0F;
            this.lineDetailVerBillingAmount.Width = 0F;
            this.lineDetailVerBillingAmount.X1 = 4.890158F;
            this.lineDetailVerBillingAmount.X2 = 4.890158F;
            this.lineDetailVerBillingAmount.Y1 = 0F;
            this.lineDetailVerBillingAmount.Y2 = 0.4598425F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.DataField = "DepartmentName";
            this.txtDepartmentName.Height = 0.2244094F;
            this.txtDepartmentName.Left = 8.246063F;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Style = "vertical-align: middle; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtDepartmentName.Text = null;
            this.txtDepartmentName.Top = 0.234252F;
            this.txtDepartmentName.Width = 1.181102F;
            // 
            // txtClosingAt
            // 
            this.txtClosingAt.DataField = "ClosingAt";
            this.txtClosingAt.Height = 0.2244094F;
            this.txtClosingAt.Left = 2.770079F;
            this.txtClosingAt.Name = "txtClosingAt";
            this.txtClosingAt.OutputFormat = resources.GetString("txtClosingAt.OutputFormat");
            this.txtClosingAt.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtClosingAt.Tag = "";
            this.txtClosingAt.Text = null;
            this.txtClosingAt.Top = 0F;
            this.txtClosingAt.Width = 0.8799213F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.DataField = "CustomerCode";
            this.txtCustomerCode.Height = 0.2244094F;
            this.txtCustomerCode.Left = 0.738189F;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 1.490116E-08F;
            this.txtCustomerCode.Width = 1.135827F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.DataField = "DepartmentCode";
            this.txtDepartmentCode.Height = 0.2244094F;
            this.txtDepartmentCode.Left = 8.246063F;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Style = "text-align: center; white-space: nowrap; ddo-wrap-mode: nowrap";
            this.txtDepartmentCode.Text = null;
            this.txtDepartmentCode.Top = 1.490116E-08F;
            this.txtDepartmentCode.Width = 1.181102F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.DataField = "billingDueAt";
            this.txtDueAt.Height = 0.2244094F;
            this.txtDueAt.Left = 2.770079F;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtDueAt.Text = null;
            this.txtDueAt.Top = 0.234252F;
            this.txtDueAt.Width = 0.8799213F;
            // 
            // txtAssignmentFlag
            // 
            this.txtAssignmentFlag.Height = 0.2244094F;
            this.txtAssignmentFlag.Left = 0F;
            this.txtAssignmentFlag.Name = "txtAssignmentFlag";
            this.txtAssignmentFlag.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtAssignmentFlag.Text = null;
            this.txtAssignmentFlag.Top = 0.234252F;
            this.txtAssignmentFlag.Width = 0.738189F;
            // 
            // lineDetailVerBillingId
            // 
            this.lineDetailVerBillingId.Height = 0.4598425F;
            this.lineDetailVerBillingId.Left = 0.738189F;
            this.lineDetailVerBillingId.LineWeight = 1F;
            this.lineDetailVerBillingId.Name = "lineDetailVerBillingId";
            this.lineDetailVerBillingId.Top = 0F;
            this.lineDetailVerBillingId.Width = 0F;
            this.lineDetailVerBillingId.X1 = 0.738189F;
            this.lineDetailVerBillingId.X2 = 0.738189F;
            this.lineDetailVerBillingId.Y1 = 0F;
            this.lineDetailVerBillingId.Y2 = 0.4598425F;
            // 
            // lineDetailVerInvoiceNo
            // 
            this.lineDetailVerInvoiceNo.Height = 0.4598425F;
            this.lineDetailVerInvoiceNo.Left = 8.246063F;
            this.lineDetailVerInvoiceNo.LineWeight = 1F;
            this.lineDetailVerInvoiceNo.Name = "lineDetailVerInvoiceNo";
            this.lineDetailVerInvoiceNo.Top = 0F;
            this.lineDetailVerInvoiceNo.Width = 0F;
            this.lineDetailVerInvoiceNo.X1 = 8.246063F;
            this.lineDetailVerInvoiceNo.X2 = 8.246063F;
            this.lineDetailVerInvoiceNo.Y1 = 0F;
            this.lineDetailVerInvoiceNo.Y2 = 0.4598425F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.4598425F;
            this.lineDetailVerCustomerCode.Left = 1.874F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 1.874F;
            this.lineDetailVerCustomerCode.X2 = 1.874F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.4598425F;
            // 
            // lineDetailVerClosingAt
            // 
            this.lineDetailVerClosingAt.Height = 0.4598426F;
            this.lineDetailVerClosingAt.Left = 3.65F;
            this.lineDetailVerClosingAt.LineWeight = 1F;
            this.lineDetailVerClosingAt.Name = "lineDetailVerClosingAt";
            this.lineDetailVerClosingAt.Top = 0.003543307F;
            this.lineDetailVerClosingAt.Width = 0F;
            this.lineDetailVerClosingAt.X1 = 3.65F;
            this.lineDetailVerClosingAt.X2 = 3.65F;
            this.lineDetailVerClosingAt.Y1 = 0.003543307F;
            this.lineDetailVerClosingAt.Y2 = 0.4633859F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
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
            this.lblPageNumber,
            this.reportInfo1});
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
            // reportInfo1
            // 
            this.reportInfo1.FormatString = "{PageNumber} / {PageCount}";
            this.reportInfo1.Height = 0.2F;
            this.reportInfo1.Left = 7.234252F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.04488189F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1.181102F;
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
            this.txtRemainGrandTotal,
            this.txtBillingGrandTotal,
            this.lblGrandTotal,
            this.lineFooterVerGrandTotal,
            this.lineFooterVerBillingGrandTotal,
            this.lineFooterVerRemainGrandTotal,
            this.lineFooterHorLower});
            this.groupFooter1.Height = 0.4122047F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // lblSpace
            // 
            this.lblSpace.Height = 0.4043307F;
            this.lblSpace.HyperLink = null;
            this.lblSpace.Left = 6.155F;
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Style = "background-color: WhiteSmoke";
            this.lblSpace.Text = "";
            this.lblSpace.Top = 0F;
            this.lblSpace.Width = 4.472441F;
            // 
            // txtRemainGrandTotal
            // 
            this.txtRemainGrandTotal.DataField = "RemainAmount";
            this.txtRemainGrandTotal.Height = 0.4043307F;
            this.txtRemainGrandTotal.Left = 4.890158F;
            this.txtRemainGrandTotal.Name = "txtRemainGrandTotal";
            this.txtRemainGrandTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtRemainGrandTotal.Style = "background-color: WhiteSmoke; text-align: right; vertical-align: middle";
            this.txtRemainGrandTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtRemainGrandTotal.Text = null;
            this.txtRemainGrandTotal.Top = 0F;
            this.txtRemainGrandTotal.Width = 1.259843F;
            // 
            // txtBillingGrandTotal
            // 
            this.txtBillingGrandTotal.DataField = "BillingAmount";
            this.txtBillingGrandTotal.Height = 0.4043307F;
            this.txtBillingGrandTotal.Left = 3.65F;
            this.txtBillingGrandTotal.Name = "txtBillingGrandTotal";
            this.txtBillingGrandTotal.OutputFormat = resources.GetString("txtBillingGrandTotal.OutputFormat");
            this.txtBillingGrandTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtBillingGrandTotal.Style = "background-color: WhiteSmoke; text-align: right; vertical-align: middle";
            this.txtBillingGrandTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtBillingGrandTotal.Text = null;
            this.txtBillingGrandTotal.Top = 0F;
            this.txtBillingGrandTotal.Width = 1.240157F;
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.Height = 0.4043307F;
            this.lblGrandTotal.HyperLink = null;
            this.lblGrandTotal.Left = 0F;
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblGrandTotal.Style = "background-color: WhiteSmoke; vertical-align: middle";
            this.lblGrandTotal.Text = "総合計";
            this.lblGrandTotal.Top = 0F;
            this.lblGrandTotal.Width = 3.645669F;
            // 
            // lineFooterVerGrandTotal
            // 
            this.lineFooterVerGrandTotal.Height = 0.4122047F;
            this.lineFooterVerGrandTotal.Left = 3.65F;
            this.lineFooterVerGrandTotal.LineWeight = 1F;
            this.lineFooterVerGrandTotal.Name = "lineFooterVerGrandTotal";
            this.lineFooterVerGrandTotal.Top = 0F;
            this.lineFooterVerGrandTotal.Width = 0F;
            this.lineFooterVerGrandTotal.X1 = 3.65F;
            this.lineFooterVerGrandTotal.X2 = 3.65F;
            this.lineFooterVerGrandTotal.Y1 = 0F;
            this.lineFooterVerGrandTotal.Y2 = 0.4122047F;
            // 
            // lineFooterVerBillingGrandTotal
            // 
            this.lineFooterVerBillingGrandTotal.Height = 0.4122047F;
            this.lineFooterVerBillingGrandTotal.Left = 4.890158F;
            this.lineFooterVerBillingGrandTotal.LineWeight = 1F;
            this.lineFooterVerBillingGrandTotal.Name = "lineFooterVerBillingGrandTotal";
            this.lineFooterVerBillingGrandTotal.Top = 0F;
            this.lineFooterVerBillingGrandTotal.Width = 0F;
            this.lineFooterVerBillingGrandTotal.X1 = 4.890158F;
            this.lineFooterVerBillingGrandTotal.X2 = 4.890158F;
            this.lineFooterVerBillingGrandTotal.Y1 = 0F;
            this.lineFooterVerBillingGrandTotal.Y2 = 0.4122047F;
            // 
            // lineFooterVerRemainGrandTotal
            // 
            this.lineFooterVerRemainGrandTotal.Height = 0.4122047F;
            this.lineFooterVerRemainGrandTotal.Left = 6.155118F;
            this.lineFooterVerRemainGrandTotal.LineWeight = 1F;
            this.lineFooterVerRemainGrandTotal.Name = "lineFooterVerRemainGrandTotal";
            this.lineFooterVerRemainGrandTotal.Top = 0F;
            this.lineFooterVerRemainGrandTotal.Width = 0F;
            this.lineFooterVerRemainGrandTotal.X1 = 6.155118F;
            this.lineFooterVerRemainGrandTotal.X2 = 6.155118F;
            this.lineFooterVerRemainGrandTotal.Y1 = 0F;
            this.lineFooterVerRemainGrandTotal.Y2 = 0.4122047F;
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
            // BillingOmitSectionReport
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
            ((System.ComponentModel.ISupportInitialize)(this.lblAssignmentFlag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.llbBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAssignmentFlag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainGrandTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingGrandTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrandTotal)).EndInit();
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
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label llbBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInputType;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceNo;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffname;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRemainingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerInvoiceNo;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAssignmentFlag;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAssignmentFlag;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInputType;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerInvoiceNo;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerBillingGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerRemainGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainGrandTotal;
        protected GrapeCity.ActiveReports.SectionReportModel.Label lblSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingID;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerClosingAt;
    }
}
