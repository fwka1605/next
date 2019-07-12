namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// ScheduledPaymentListSectionReport の概要の説明です。
    /// </summary>
    partial class ScheduledPaymentListSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ScheduledPaymentListSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSalesAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingId = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblOrginalDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCollectCategory = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDelay = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerOrginalDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerInvoiceNo = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCollectCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorInvoiceNo = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtBillingId = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSalesAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtOriginalDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCollectCategory = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDelay = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtClosingAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerInvoiceNo = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCollectCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerOriginalDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblSpaceTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader2 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter2 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblDepartmentTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtDepartmentAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterVerDepartmentTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblSpaceDepartmentAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerDepartmentAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorDepartmentAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader3 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter3 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblStaffTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtStaffAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterVerStaffTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblSpaceStaffAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerStaffAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorStaffAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader4 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter4 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblCustomerTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtCustomerAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterVerCustomerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblSpaceCustomerAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerCustomerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorCustomerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader5 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter5 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblDueAtTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtDueAtAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterVerDutAtTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblSpaceDueAtAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerDueAtAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorDueAtTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOrginalDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCollectCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOriginalDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceDepartmentAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceStaffAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceCustomerAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAtTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceDueAtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblCustomerCode,
            this.lblCustomerName,
            this.lblBilledAt,
            this.lblSalesAt,
            this.lblClosingAt,
            this.lblDueAt,
            this.lblBillingId,
            this.lblOrginalDueAt,
            this.lblInvoiceCode,
            this.lblNote1,
            this.lblRemainAmount,
            this.lblDepartmentCode,
            this.lblDepartmentName,
            this.lblStaffCode,
            this.lblStaffName,
            this.lblCollectCategory,
            this.lblDelay,
            this.lineHeaderHorLower,
            this.lineHeaderHorUpper,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderVerClosingAt,
            this.lineHeaderVerRemainAmount,
            this.lineHeaderVerOrginalDueAt,
            this.lineHeaderVerInvoiceNo,
            this.lineHeaderVerStaffCode,
            this.lineHeaderVerDepartmentCode,
            this.lineHeaderVerCollectCategoryCode,
            this.lineHeaderVerBillingId,
            this.lineHeaderVerBilledAt,
            this.lineHeaderHorCustomerCode,
            this.lineHeaderHorInvoiceNo});
            this.pageHeader.Height = 1.008F;
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
            this.lblTitle.Text = "入金予定明細表";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.2F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 0.7F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.6F;
            this.lblCustomerCode.Width = 2F;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Height = 0.2F;
            this.lblCustomerName.HyperLink = null;
            this.lblCustomerName.Left = 0.7F;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.Top = 0.8F;
            this.lblCustomerName.Width = 2F;
            // 
            // lblBilledAt
            // 
            this.lblBilledAt.Height = 0.2F;
            this.lblBilledAt.HyperLink = null;
            this.lblBilledAt.Left = 2.7F;
            this.lblBilledAt.Name = "lblBilledAt";
            this.lblBilledAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBilledAt.Text = "請求日";
            this.lblBilledAt.Top = 0.6F;
            this.lblBilledAt.Width = 0.6F;
            // 
            // lblSalesAt
            // 
            this.lblSalesAt.Height = 0.2F;
            this.lblSalesAt.HyperLink = null;
            this.lblSalesAt.Left = 2.7F;
            this.lblSalesAt.Name = "lblSalesAt";
            this.lblSalesAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblSalesAt.Text = "売上日";
            this.lblSalesAt.Top = 0.8F;
            this.lblSalesAt.Width = 0.6F;
            // 
            // lblClosingAt
            // 
            this.lblClosingAt.Height = 0.2F;
            this.lblClosingAt.HyperLink = null;
            this.lblClosingAt.Left = 3.3F;
            this.lblClosingAt.Name = "lblClosingAt";
            this.lblClosingAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblClosingAt.Text = "請求締日";
            this.lblClosingAt.Top = 0.6F;
            this.lblClosingAt.Width = 0.6F;
            // 
            // lblDueAt
            // 
            this.lblDueAt.Height = 0.2F;
            this.lblDueAt.HyperLink = null;
            this.lblDueAt.Left = 3.3F;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDueAt.Text = "入金予定日";
            this.lblDueAt.Top = 0.8F;
            this.lblDueAt.Width = 0.6F;
            // 
            // lblBillingId
            // 
            this.lblBillingId.Height = 0.4F;
            this.lblBillingId.HyperLink = null;
            this.lblBillingId.Left = 0F;
            this.lblBillingId.Name = "lblBillingId";
            this.lblBillingId.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingId.Text = "請求ID";
            this.lblBillingId.Top = 0.6F;
            this.lblBillingId.Width = 0.7F;
            // 
            // lblOrginalDueAt
            // 
            this.lblOrginalDueAt.Height = 0.4F;
            this.lblOrginalDueAt.HyperLink = null;
            this.lblOrginalDueAt.Left = 3.9F;
            this.lblOrginalDueAt.Name = "lblOrginalDueAt";
            this.lblOrginalDueAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblOrginalDueAt.Text = "当初予定日";
            this.lblOrginalDueAt.Top = 0.608F;
            this.lblOrginalDueAt.Width = 0.6F;
            // 
            // lblInvoiceCode
            // 
            this.lblInvoiceCode.Height = 0.2F;
            this.lblInvoiceCode.HyperLink = null;
            this.lblInvoiceCode.Left = 5.5F;
            this.lblInvoiceCode.Name = "lblInvoiceCode";
            this.lblInvoiceCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblInvoiceCode.Text = "請求書番号";
            this.lblInvoiceCode.Top = 0.6F;
            this.lblInvoiceCode.Width = 1.5F;
            // 
            // lblNote1
            // 
            this.lblNote1.Height = 0.2F;
            this.lblNote1.HyperLink = null;
            this.lblNote1.Left = 5.5F;
            this.lblNote1.Name = "lblNote1";
            this.lblNote1.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblNote1.Text = "備考";
            this.lblNote1.Top = 0.8F;
            this.lblNote1.Width = 1.5F;
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Height = 0.4F;
            this.lblRemainAmount.HyperLink = null;
            this.lblRemainAmount.Left = 4.5F;
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1; ddo-font-vertical: true";
            this.lblRemainAmount.Text = "回収予定金額";
            this.lblRemainAmount.Top = 0.6F;
            this.lblRemainAmount.Width = 1F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.2F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 7F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDepartmentCode.Text = "請求部門コード";
            this.lblDepartmentCode.Top = 0.6F;
            this.lblDepartmentCode.Width = 1.1F;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Height = 0.2F;
            this.lblDepartmentName.HyperLink = null;
            this.lblDepartmentName.Left = 7F;
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDepartmentName.Text = "請求部門名";
            this.lblDepartmentName.Top = 0.8F;
            this.lblDepartmentName.Width = 1.1F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.2F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 8.1F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblStaffCode.Text = "担当者コード";
            this.lblStaffCode.Top = 0.6F;
            this.lblStaffCode.Width = 1.1F;
            // 
            // lblStaffName
            // 
            this.lblStaffName.Height = 0.2F;
            this.lblStaffName.HyperLink = null;
            this.lblStaffName.Left = 8.1F;
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblStaffName.Text = "担当者名";
            this.lblStaffName.Top = 0.8F;
            this.lblStaffName.Width = 1.1F;
            // 
            // lblCollectCategory
            // 
            this.lblCollectCategory.Height = 0.4F;
            this.lblCollectCategory.HyperLink = null;
            this.lblCollectCategory.Left = 9.200001F;
            this.lblCollectCategory.Name = "lblCollectCategory";
            this.lblCollectCategory.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCollectCategory.Text = "回収区分";
            this.lblCollectCategory.Top = 0.6F;
            this.lblCollectCategory.Width = 0.9F;
            // 
            // lblDelay
            // 
            this.lblDelay.Height = 0.4F;
            this.lblDelay.HyperLink = null;
            this.lblDelay.Left = 10.1F;
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDelay.Text = "遅延";
            this.lblDelay.Top = 0.6F;
            this.lblDelay.Width = 0.53F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1F;
            this.lineHeaderHorLower.Width = 10.65F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.65F;
            this.lineHeaderHorLower.Y1 = 1F;
            this.lineHeaderHorLower.Y2 = 1F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.6F;
            this.lineHeaderHorUpper.Width = 10.65F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.65F;
            this.lineHeaderHorUpper.Y1 = 0.6F;
            this.lineHeaderHorUpper.Y2 = 0.6F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.4F;
            this.lineHeaderVerCustomerCode.Left = 2.7F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.6F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 2.7F;
            this.lineHeaderVerCustomerCode.X2 = 2.7F;
            this.lineHeaderVerCustomerCode.Y1 = 0.6F;
            this.lineHeaderVerCustomerCode.Y2 = 1F;
            // 
            // lineHeaderVerClosingAt
            // 
            this.lineHeaderVerClosingAt.Height = 0.4F;
            this.lineHeaderVerClosingAt.Left = 3.9F;
            this.lineHeaderVerClosingAt.LineWeight = 1F;
            this.lineHeaderVerClosingAt.Name = "lineHeaderVerClosingAt";
            this.lineHeaderVerClosingAt.Top = 0.6F;
            this.lineHeaderVerClosingAt.Width = 0F;
            this.lineHeaderVerClosingAt.X1 = 3.9F;
            this.lineHeaderVerClosingAt.X2 = 3.9F;
            this.lineHeaderVerClosingAt.Y1 = 0.6F;
            this.lineHeaderVerClosingAt.Y2 = 1F;
            // 
            // lineHeaderVerRemainAmount
            // 
            this.lineHeaderVerRemainAmount.Height = 0.4F;
            this.lineHeaderVerRemainAmount.Left = 5.5F;
            this.lineHeaderVerRemainAmount.LineWeight = 1F;
            this.lineHeaderVerRemainAmount.Name = "lineHeaderVerRemainAmount";
            this.lineHeaderVerRemainAmount.Top = 0.6F;
            this.lineHeaderVerRemainAmount.Width = 0F;
            this.lineHeaderVerRemainAmount.X1 = 5.5F;
            this.lineHeaderVerRemainAmount.X2 = 5.5F;
            this.lineHeaderVerRemainAmount.Y1 = 0.6F;
            this.lineHeaderVerRemainAmount.Y2 = 1F;
            // 
            // lineHeaderVerOrginalDueAt
            // 
            this.lineHeaderVerOrginalDueAt.Height = 0.4F;
            this.lineHeaderVerOrginalDueAt.Left = 4.5F;
            this.lineHeaderVerOrginalDueAt.LineWeight = 1F;
            this.lineHeaderVerOrginalDueAt.Name = "lineHeaderVerOrginalDueAt";
            this.lineHeaderVerOrginalDueAt.Top = 0.6F;
            this.lineHeaderVerOrginalDueAt.Width = 0F;
            this.lineHeaderVerOrginalDueAt.X1 = 4.5F;
            this.lineHeaderVerOrginalDueAt.X2 = 4.5F;
            this.lineHeaderVerOrginalDueAt.Y1 = 0.6F;
            this.lineHeaderVerOrginalDueAt.Y2 = 1F;
            // 
            // lineHeaderVerInvoiceNo
            // 
            this.lineHeaderVerInvoiceNo.Height = 0.4F;
            this.lineHeaderVerInvoiceNo.Left = 7F;
            this.lineHeaderVerInvoiceNo.LineWeight = 1F;
            this.lineHeaderVerInvoiceNo.Name = "lineHeaderVerInvoiceNo";
            this.lineHeaderVerInvoiceNo.Top = 0.6F;
            this.lineHeaderVerInvoiceNo.Width = 0F;
            this.lineHeaderVerInvoiceNo.X1 = 7F;
            this.lineHeaderVerInvoiceNo.X2 = 7F;
            this.lineHeaderVerInvoiceNo.Y1 = 0.6F;
            this.lineHeaderVerInvoiceNo.Y2 = 1F;
            // 
            // lineHeaderVerStaffCode
            // 
            this.lineHeaderVerStaffCode.Height = 0.4F;
            this.lineHeaderVerStaffCode.Left = 9.200001F;
            this.lineHeaderVerStaffCode.LineWeight = 1F;
            this.lineHeaderVerStaffCode.Name = "lineHeaderVerStaffCode";
            this.lineHeaderVerStaffCode.Top = 0.6F;
            this.lineHeaderVerStaffCode.Width = 0F;
            this.lineHeaderVerStaffCode.X1 = 9.200001F;
            this.lineHeaderVerStaffCode.X2 = 9.200001F;
            this.lineHeaderVerStaffCode.Y1 = 0.6F;
            this.lineHeaderVerStaffCode.Y2 = 1F;
            // 
            // lineHeaderVerDepartmentCode
            // 
            this.lineHeaderVerDepartmentCode.Height = 0.4F;
            this.lineHeaderVerDepartmentCode.Left = 8.1F;
            this.lineHeaderVerDepartmentCode.LineWeight = 1F;
            this.lineHeaderVerDepartmentCode.Name = "lineHeaderVerDepartmentCode";
            this.lineHeaderVerDepartmentCode.Top = 0.6F;
            this.lineHeaderVerDepartmentCode.Width = 0F;
            this.lineHeaderVerDepartmentCode.X1 = 8.1F;
            this.lineHeaderVerDepartmentCode.X2 = 8.1F;
            this.lineHeaderVerDepartmentCode.Y1 = 0.6F;
            this.lineHeaderVerDepartmentCode.Y2 = 1F;
            // 
            // lineHeaderVerCollectCategoryCode
            // 
            this.lineHeaderVerCollectCategoryCode.Height = 0.4F;
            this.lineHeaderVerCollectCategoryCode.Left = 10.1F;
            this.lineHeaderVerCollectCategoryCode.LineWeight = 1F;
            this.lineHeaderVerCollectCategoryCode.Name = "lineHeaderVerCollectCategoryCode";
            this.lineHeaderVerCollectCategoryCode.Top = 0.6F;
            this.lineHeaderVerCollectCategoryCode.Width = 0F;
            this.lineHeaderVerCollectCategoryCode.X1 = 10.1F;
            this.lineHeaderVerCollectCategoryCode.X2 = 10.1F;
            this.lineHeaderVerCollectCategoryCode.Y1 = 0.6F;
            this.lineHeaderVerCollectCategoryCode.Y2 = 1F;
            // 
            // lineHeaderVerBillingId
            // 
            this.lineHeaderVerBillingId.Height = 0.4F;
            this.lineHeaderVerBillingId.Left = 0.7F;
            this.lineHeaderVerBillingId.LineWeight = 1F;
            this.lineHeaderVerBillingId.Name = "lineHeaderVerBillingId";
            this.lineHeaderVerBillingId.Top = 0.6F;
            this.lineHeaderVerBillingId.Width = 0F;
            this.lineHeaderVerBillingId.X1 = 0.7F;
            this.lineHeaderVerBillingId.X2 = 0.7F;
            this.lineHeaderVerBillingId.Y1 = 0.6F;
            this.lineHeaderVerBillingId.Y2 = 1F;
            // 
            // lineHeaderVerBilledAt
            // 
            this.lineHeaderVerBilledAt.Height = 0.4F;
            this.lineHeaderVerBilledAt.Left = 3.3F;
            this.lineHeaderVerBilledAt.LineWeight = 1F;
            this.lineHeaderVerBilledAt.Name = "lineHeaderVerBilledAt";
            this.lineHeaderVerBilledAt.Top = 0.6F;
            this.lineHeaderVerBilledAt.Width = 0F;
            this.lineHeaderVerBilledAt.X1 = 3.3F;
            this.lineHeaderVerBilledAt.X2 = 3.3F;
            this.lineHeaderVerBilledAt.Y1 = 0.6F;
            this.lineHeaderVerBilledAt.Y2 = 1F;
            // 
            // lineHeaderHorCustomerCode
            // 
            this.lineHeaderHorCustomerCode.Height = 0F;
            this.lineHeaderHorCustomerCode.Left = 0.7F;
            this.lineHeaderHorCustomerCode.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorCustomerCode.LineWeight = 1F;
            this.lineHeaderHorCustomerCode.Name = "lineHeaderHorCustomerCode";
            this.lineHeaderHorCustomerCode.Top = 0.8F;
            this.lineHeaderHorCustomerCode.Width = 3.2F;
            this.lineHeaderHorCustomerCode.X1 = 0.7F;
            this.lineHeaderHorCustomerCode.X2 = 3.9F;
            this.lineHeaderHorCustomerCode.Y1 = 0.8F;
            this.lineHeaderHorCustomerCode.Y2 = 0.8F;
            // 
            // lineHeaderHorInvoiceNo
            // 
            this.lineHeaderHorInvoiceNo.Height = 0F;
            this.lineHeaderHorInvoiceNo.Left = 5.5F;
            this.lineHeaderHorInvoiceNo.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorInvoiceNo.LineWeight = 1F;
            this.lineHeaderHorInvoiceNo.Name = "lineHeaderHorInvoiceNo";
            this.lineHeaderHorInvoiceNo.Top = 0.8F;
            this.lineHeaderHorInvoiceNo.Width = 3.7F;
            this.lineHeaderHorInvoiceNo.X1 = 5.5F;
            this.lineHeaderHorInvoiceNo.X2 = 9.2F;
            this.lineHeaderHorInvoiceNo.Y1 = 0.8F;
            this.lineHeaderHorInvoiceNo.Y2 = 0.8F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtBillingId,
            this.txtCustomerName,
            this.txtCustomerCode,
            this.txtBilledAt,
            this.txtSalesAt,
            this.txtInvoiceCode,
            this.txtNote1,
            this.txtOriginalDueAt,
            this.txtRemainAmount,
            this.txtDepartmentCode,
            this.txtDepartmentName,
            this.txtStaffCode,
            this.txtStaffName,
            this.txtCollectCategory,
            this.txtDelay,
            this.txtClosingAt,
            this.txtDueAt,
            this.lineDetailHorLower,
            this.lineDetailVerBillingId,
            this.lineDetailVerCustomerCode,
            this.lineDetailVerBilledAt,
            this.lineDetailVerRemainAmount,
            this.lineDetailVerInvoiceNo,
            this.lineDetailVerDepartmentCode,
            this.lineDetailVerStaffCode,
            this.lineDetailVerCollectCategoryCode,
            this.lineDetailVerClosingAt,
            this.lineDetailVerOriginalDueAt});
            this.detail.Height = 0.4F;
            this.detail.Name = "detail";
            // 
            // txtBillingId
            // 
            this.txtBillingId.DataField = "Id";
            this.txtBillingId.Height = 0.4F;
            this.txtBillingId.Left = 0F;
            this.txtBillingId.MultiLine = false;
            this.txtBillingId.Name = "txtBillingId";
            this.txtBillingId.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtBillingId.Style = "font-size: 6pt; text-align: right; vertical-align: middle; white-space: nowrap; d" +
    "do-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBillingId.Text = null;
            this.txtBillingId.Top = 0F;
            this.txtBillingId.Width = 0.68F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.DataField = "CustomerName";
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 0.72F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCustomerName.Text = null;
            this.txtCustomerName.Top = 0.2F;
            this.txtCustomerName.Width = 1.98F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.DataField = "CustomerCode";
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 0.72F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 1.98F;
            // 
            // txtBilledAt
            // 
            this.txtBilledAt.Height = 0.2F;
            this.txtBilledAt.Left = 2.7F;
            this.txtBilledAt.MultiLine = false;
            this.txtBilledAt.Name = "txtBilledAt";
            this.txtBilledAt.OutputFormat = resources.GetString("txtBilledAt.OutputFormat");
            this.txtBilledAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBilledAt.Text = null;
            this.txtBilledAt.Top = 0F;
            this.txtBilledAt.Width = 0.6F;
            // 
            // txtSalesAt
            // 
            this.txtSalesAt.DataField = "Note";
            this.txtSalesAt.Height = 0.2F;
            this.txtSalesAt.Left = 2.7F;
            this.txtSalesAt.MultiLine = false;
            this.txtSalesAt.Name = "txtSalesAt";
            this.txtSalesAt.OutputFormat = resources.GetString("txtSalesAt.OutputFormat");
            this.txtSalesAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtSalesAt.Text = null;
            this.txtSalesAt.Top = 0.2F;
            this.txtSalesAt.Width = 0.6F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.Height = 0.2F;
            this.txtInvoiceCode.Left = 5.52F;
            this.txtInvoiceCode.MultiLine = false;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.OutputFormat = resources.GetString("txtInvoiceCode.OutputFormat");
            this.txtInvoiceCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtInvoiceCode.Text = null;
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 1.48F;
            // 
            // txtNote1
            // 
            this.txtNote1.Height = 0.2F;
            this.txtNote1.Left = 5.52F;
            this.txtNote1.MultiLine = false;
            this.txtNote1.Name = "txtNote1";
            this.txtNote1.OutputFormat = resources.GetString("txtNote1.OutputFormat");
            this.txtNote1.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtNote1.Text = null;
            this.txtNote1.Top = 0.2F;
            this.txtNote1.Width = 1.48F;
            // 
            // txtOriginalDueAt
            // 
            this.txtOriginalDueAt.Height = 0.4F;
            this.txtOriginalDueAt.Left = 3.9F;
            this.txtOriginalDueAt.MultiLine = false;
            this.txtOriginalDueAt.Name = "txtOriginalDueAt";
            this.txtOriginalDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtOriginalDueAt.Text = null;
            this.txtOriginalDueAt.Top = 0F;
            this.txtOriginalDueAt.Width = 0.6F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.DataField = "TotalAmount";
            this.txtRemainAmount.Height = 0.4F;
            this.txtRemainAmount.Left = 4.5F;
            this.txtRemainAmount.MultiLine = false;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtRemainAmount.Text = null;
            this.txtRemainAmount.Top = 0F;
            this.txtRemainAmount.Width = 0.98F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.Height = 0.2F;
            this.txtDepartmentCode.Left = 7.02F;
            this.txtDepartmentCode.MultiLine = false;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.OutputFormat = resources.GetString("txtDepartmentCode.OutputFormat");
            this.txtDepartmentCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDepartmentCode.Text = null;
            this.txtDepartmentCode.Top = 0F;
            this.txtDepartmentCode.Width = 1.08F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.Height = 0.2F;
            this.txtDepartmentName.Left = 7.02F;
            this.txtDepartmentName.MultiLine = false;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.OutputFormat = resources.GetString("txtDepartmentName.OutputFormat");
            this.txtDepartmentName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDepartmentName.Text = null;
            this.txtDepartmentName.Top = 0.2F;
            this.txtDepartmentName.Width = 1.08F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.Height = 0.2F;
            this.txtStaffCode.Left = 8.120001F;
            this.txtStaffCode.MultiLine = false;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.OutputFormat = resources.GetString("txtStaffCode.OutputFormat");
            this.txtStaffCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtStaffCode.Text = null;
            this.txtStaffCode.Top = 0F;
            this.txtStaffCode.Width = 1.08F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.Height = 0.2F;
            this.txtStaffName.Left = 8.120001F;
            this.txtStaffName.MultiLine = false;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.OutputFormat = resources.GetString("txtStaffName.OutputFormat");
            this.txtStaffName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtStaffName.Text = null;
            this.txtStaffName.Top = 0.2F;
            this.txtStaffName.Width = 1.08F;
            // 
            // txtCollectCategory
            // 
            this.txtCollectCategory.Height = 0.4F;
            this.txtCollectCategory.Left = 9.22F;
            this.txtCollectCategory.MultiLine = false;
            this.txtCollectCategory.Name = "txtCollectCategory";
            this.txtCollectCategory.OutputFormat = resources.GetString("txtCollectCategory.OutputFormat");
            this.txtCollectCategory.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCollectCategory.Text = null;
            this.txtCollectCategory.Top = 0F;
            this.txtCollectCategory.Width = 0.88F;
            // 
            // txtDelay
            // 
            this.txtDelay.Height = 0.4F;
            this.txtDelay.Left = 10.1F;
            this.txtDelay.MultiLine = false;
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.OutputFormat = resources.GetString("txtDelay.OutputFormat");
            this.txtDelay.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDelay.Text = null;
            this.txtDelay.Top = 0F;
            this.txtDelay.Width = 0.53F;
            // 
            // txtClosingAt
            // 
            this.txtClosingAt.Height = 0.2F;
            this.txtClosingAt.Left = 3.3F;
            this.txtClosingAt.MultiLine = false;
            this.txtClosingAt.Name = "txtClosingAt";
            this.txtClosingAt.OutputFormat = resources.GetString("txtClosingAt.OutputFormat");
            this.txtClosingAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtClosingAt.Text = null;
            this.txtClosingAt.Top = 0F;
            this.txtClosingAt.Width = 0.6F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.DataField = "Note";
            this.txtDueAt.Height = 0.2F;
            this.txtDueAt.Left = 3.3F;
            this.txtDueAt.MultiLine = false;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDueAt.Text = null;
            this.txtDueAt.Top = 0.2F;
            this.txtDueAt.Width = 0.6F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.4F;
            this.lineDetailHorLower.Width = 10.634F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.634F;
            this.lineDetailHorLower.Y1 = 0.4F;
            this.lineDetailHorLower.Y2 = 0.4F;
            // 
            // lineDetailVerBillingId
            // 
            this.lineDetailVerBillingId.Height = 0.4F;
            this.lineDetailVerBillingId.Left = 0.7F;
            this.lineDetailVerBillingId.LineWeight = 1F;
            this.lineDetailVerBillingId.Name = "lineDetailVerBillingId";
            this.lineDetailVerBillingId.Top = 0F;
            this.lineDetailVerBillingId.Width = 0F;
            this.lineDetailVerBillingId.X1 = 0.7F;
            this.lineDetailVerBillingId.X2 = 0.7F;
            this.lineDetailVerBillingId.Y1 = 0F;
            this.lineDetailVerBillingId.Y2 = 0.4F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.4F;
            this.lineDetailVerCustomerCode.Left = 2.7F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 2.7F;
            this.lineDetailVerCustomerCode.X2 = 2.7F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.4F;
            // 
            // lineDetailVerBilledAt
            // 
            this.lineDetailVerBilledAt.Height = 0.4F;
            this.lineDetailVerBilledAt.Left = 3.3F;
            this.lineDetailVerBilledAt.LineWeight = 1F;
            this.lineDetailVerBilledAt.Name = "lineDetailVerBilledAt";
            this.lineDetailVerBilledAt.Top = 0F;
            this.lineDetailVerBilledAt.Width = 0F;
            this.lineDetailVerBilledAt.X1 = 3.3F;
            this.lineDetailVerBilledAt.X2 = 3.3F;
            this.lineDetailVerBilledAt.Y1 = 0F;
            this.lineDetailVerBilledAt.Y2 = 0.4F;
            // 
            // lineDetailVerRemainAmount
            // 
            this.lineDetailVerRemainAmount.Height = 0.4F;
            this.lineDetailVerRemainAmount.Left = 5.5F;
            this.lineDetailVerRemainAmount.LineWeight = 1F;
            this.lineDetailVerRemainAmount.Name = "lineDetailVerRemainAmount";
            this.lineDetailVerRemainAmount.Top = 0F;
            this.lineDetailVerRemainAmount.Width = 0F;
            this.lineDetailVerRemainAmount.X1 = 5.5F;
            this.lineDetailVerRemainAmount.X2 = 5.5F;
            this.lineDetailVerRemainAmount.Y1 = 0F;
            this.lineDetailVerRemainAmount.Y2 = 0.4F;
            // 
            // lineDetailVerInvoiceNo
            // 
            this.lineDetailVerInvoiceNo.Height = 0.4F;
            this.lineDetailVerInvoiceNo.Left = 7F;
            this.lineDetailVerInvoiceNo.LineWeight = 1F;
            this.lineDetailVerInvoiceNo.Name = "lineDetailVerInvoiceNo";
            this.lineDetailVerInvoiceNo.Top = 0F;
            this.lineDetailVerInvoiceNo.Width = 0F;
            this.lineDetailVerInvoiceNo.X1 = 7F;
            this.lineDetailVerInvoiceNo.X2 = 7F;
            this.lineDetailVerInvoiceNo.Y1 = 0F;
            this.lineDetailVerInvoiceNo.Y2 = 0.4F;
            // 
            // lineDetailVerDepartmentCode
            // 
            this.lineDetailVerDepartmentCode.Height = 0.4F;
            this.lineDetailVerDepartmentCode.Left = 8.1F;
            this.lineDetailVerDepartmentCode.LineWeight = 1F;
            this.lineDetailVerDepartmentCode.Name = "lineDetailVerDepartmentCode";
            this.lineDetailVerDepartmentCode.Top = 0F;
            this.lineDetailVerDepartmentCode.Width = 0F;
            this.lineDetailVerDepartmentCode.X1 = 8.1F;
            this.lineDetailVerDepartmentCode.X2 = 8.1F;
            this.lineDetailVerDepartmentCode.Y1 = 0F;
            this.lineDetailVerDepartmentCode.Y2 = 0.4F;
            // 
            // lineDetailVerStaffCode
            // 
            this.lineDetailVerStaffCode.Height = 0.4F;
            this.lineDetailVerStaffCode.Left = 9.200001F;
            this.lineDetailVerStaffCode.LineWeight = 1F;
            this.lineDetailVerStaffCode.Name = "lineDetailVerStaffCode";
            this.lineDetailVerStaffCode.Top = 0F;
            this.lineDetailVerStaffCode.Width = 0F;
            this.lineDetailVerStaffCode.X1 = 9.200001F;
            this.lineDetailVerStaffCode.X2 = 9.200001F;
            this.lineDetailVerStaffCode.Y1 = 0F;
            this.lineDetailVerStaffCode.Y2 = 0.4F;
            // 
            // lineDetailVerCollectCategoryCode
            // 
            this.lineDetailVerCollectCategoryCode.Height = 0.4F;
            this.lineDetailVerCollectCategoryCode.Left = 10.1F;
            this.lineDetailVerCollectCategoryCode.LineWeight = 1F;
            this.lineDetailVerCollectCategoryCode.Name = "lineDetailVerCollectCategoryCode";
            this.lineDetailVerCollectCategoryCode.Top = 0F;
            this.lineDetailVerCollectCategoryCode.Width = 0F;
            this.lineDetailVerCollectCategoryCode.X1 = 10.1F;
            this.lineDetailVerCollectCategoryCode.X2 = 10.1F;
            this.lineDetailVerCollectCategoryCode.Y1 = 0F;
            this.lineDetailVerCollectCategoryCode.Y2 = 0.4F;
            // 
            // lineDetailVerClosingAt
            // 
            this.lineDetailVerClosingAt.Height = 0.4F;
            this.lineDetailVerClosingAt.Left = 3.9F;
            this.lineDetailVerClosingAt.LineWeight = 1F;
            this.lineDetailVerClosingAt.Name = "lineDetailVerClosingAt";
            this.lineDetailVerClosingAt.Top = 0F;
            this.lineDetailVerClosingAt.Width = 0F;
            this.lineDetailVerClosingAt.X1 = 3.9F;
            this.lineDetailVerClosingAt.X2 = 3.9F;
            this.lineDetailVerClosingAt.Y1 = 0F;
            this.lineDetailVerClosingAt.Y2 = 0.4F;
            // 
            // lineDetailVerOriginalDueAt
            // 
            this.lineDetailVerOriginalDueAt.Height = 0.4F;
            this.lineDetailVerOriginalDueAt.Left = 4.5F;
            this.lineDetailVerOriginalDueAt.LineWeight = 1F;
            this.lineDetailVerOriginalDueAt.Name = "lineDetailVerOriginalDueAt";
            this.lineDetailVerOriginalDueAt.Top = 0F;
            this.lineDetailVerOriginalDueAt.Width = 0F;
            this.lineDetailVerOriginalDueAt.X1 = 4.5F;
            this.lineDetailVerOriginalDueAt.X2 = 4.5F;
            this.lineDetailVerOriginalDueAt.Y1 = 0F;
            this.lineDetailVerOriginalDueAt.Y2 = 0.4F;
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
            this.reportInfo1.Left = 7.209449F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.03937008F;
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
            this.lblTotal,
            this.txtTotalAmount,
            this.lineFooterVerTotal,
            this.lblSpaceTotalAmount,
            this.lineFooterVerTotalAmount,
            this.lineFooterHorAmountTotal});
            this.groupFooter1.Height = 0.3075787F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // lblTotal
            // 
            this.lblTotal.Height = 0.3F;
            this.lblTotal.HyperLink = null;
            this.lblTotal.Left = 0F;
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblTotal.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblTotal.Text = "総合計";
            this.lblTotal.Top = 0F;
            this.lblTotal.Width = 4.5F;
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.DataField = "TotalAmount";
            this.txtTotalAmount.Height = 0.3F;
            this.txtTotalAmount.Left = 4.5F;
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.OutputFormat = resources.GetString("txtTotalAmount.OutputFormat");
            this.txtTotalAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtTotalAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtTotalAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalAmount.Text = null;
            this.txtTotalAmount.Top = 0F;
            this.txtTotalAmount.Width = 0.98F;
            // 
            // lineFooterVerTotal
            // 
            this.lineFooterVerTotal.Height = 0.3F;
            this.lineFooterVerTotal.Left = 4.5F;
            this.lineFooterVerTotal.LineWeight = 1F;
            this.lineFooterVerTotal.Name = "lineFooterVerTotal";
            this.lineFooterVerTotal.Top = 0F;
            this.lineFooterVerTotal.Width = 0F;
            this.lineFooterVerTotal.X1 = 4.5F;
            this.lineFooterVerTotal.X2 = 4.5F;
            this.lineFooterVerTotal.Y1 = 0F;
            this.lineFooterVerTotal.Y2 = 0.3F;
            // 
            // lblSpaceTotalAmount
            // 
            this.lblSpaceTotalAmount.Height = 0.3F;
            this.lblSpaceTotalAmount.HyperLink = null;
            this.lblSpaceTotalAmount.Left = 5.48F;
            this.lblSpaceTotalAmount.Name = "lblSpaceTotalAmount";
            this.lblSpaceTotalAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblSpaceTotalAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblSpaceTotalAmount.Text = "";
            this.lblSpaceTotalAmount.Top = 0F;
            this.lblSpaceTotalAmount.Width = 5.154494F;
            // 
            // lineFooterVerTotalAmount
            // 
            this.lineFooterVerTotalAmount.Height = 0.3F;
            this.lineFooterVerTotalAmount.Left = 5.5F;
            this.lineFooterVerTotalAmount.LineWeight = 1F;
            this.lineFooterVerTotalAmount.Name = "lineFooterVerTotalAmount";
            this.lineFooterVerTotalAmount.Top = 0F;
            this.lineFooterVerTotalAmount.Width = 0F;
            this.lineFooterVerTotalAmount.X1 = 5.5F;
            this.lineFooterVerTotalAmount.X2 = 5.5F;
            this.lineFooterVerTotalAmount.Y1 = 0F;
            this.lineFooterVerTotalAmount.Y2 = 0.3F;
            // 
            // lineFooterHorAmountTotal
            // 
            this.lineFooterHorAmountTotal.Height = 0F;
            this.lineFooterHorAmountTotal.Left = 0F;
            this.lineFooterHorAmountTotal.LineWeight = 1F;
            this.lineFooterHorAmountTotal.Name = "lineFooterHorAmountTotal";
            this.lineFooterHorAmountTotal.Top = 0.3F;
            this.lineFooterHorAmountTotal.Width = 10.62992F;
            this.lineFooterHorAmountTotal.X1 = 0F;
            this.lineFooterHorAmountTotal.X2 = 10.62992F;
            this.lineFooterHorAmountTotal.Y1 = 0.3F;
            this.lineFooterHorAmountTotal.Y2 = 0.3F;
            // 
            // groupHeader2
            // 
            this.groupHeader2.Height = 0F;
            this.groupHeader2.Name = "groupHeader2";
            // 
            // groupFooter2
            // 
            this.groupFooter2.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblDepartmentTotal,
            this.txtDepartmentAmount,
            this.lineFooterVerDepartmentTotal,
            this.lblSpaceDepartmentAmount,
            this.lineFooterVerDepartmentAmount,
            this.lineFooterHorDepartmentAmount});
            this.groupFooter2.Height = 0.3075787F;
            this.groupFooter2.Name = "groupFooter2";
            // 
            // lblDepartmentTotal
            // 
            this.lblDepartmentTotal.Height = 0.3F;
            this.lblDepartmentTotal.HyperLink = null;
            this.lblDepartmentTotal.Left = 0F;
            this.lblDepartmentTotal.Name = "lblDepartmentTotal";
            this.lblDepartmentTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblDepartmentTotal.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblDepartmentTotal.Text = "部門計";
            this.lblDepartmentTotal.Top = 0F;
            this.lblDepartmentTotal.Width = 4.5F;
            // 
            // txtDepartmentAmount
            // 
            this.txtDepartmentAmount.DataField = "TotalAmount";
            this.txtDepartmentAmount.Height = 0.3F;
            this.txtDepartmentAmount.Left = 4.5F;
            this.txtDepartmentAmount.Name = "txtDepartmentAmount";
            this.txtDepartmentAmount.OutputFormat = resources.GetString("txtDepartmentAmount.OutputFormat");
            this.txtDepartmentAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtDepartmentAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtDepartmentAmount.SummaryGroup = "groupHeader2";
            this.txtDepartmentAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtDepartmentAmount.Text = null;
            this.txtDepartmentAmount.Top = 0F;
            this.txtDepartmentAmount.Width = 0.98F;
            // 
            // lineFooterVerDepartmentTotal
            // 
            this.lineFooterVerDepartmentTotal.Height = 0.3F;
            this.lineFooterVerDepartmentTotal.Left = 4.5F;
            this.lineFooterVerDepartmentTotal.LineWeight = 1F;
            this.lineFooterVerDepartmentTotal.Name = "lineFooterVerDepartmentTotal";
            this.lineFooterVerDepartmentTotal.Top = 0F;
            this.lineFooterVerDepartmentTotal.Width = 0F;
            this.lineFooterVerDepartmentTotal.X1 = 4.5F;
            this.lineFooterVerDepartmentTotal.X2 = 4.5F;
            this.lineFooterVerDepartmentTotal.Y1 = 0F;
            this.lineFooterVerDepartmentTotal.Y2 = 0.3F;
            // 
            // lblSpaceDepartmentAmount
            // 
            this.lblSpaceDepartmentAmount.Height = 0.3F;
            this.lblSpaceDepartmentAmount.HyperLink = null;
            this.lblSpaceDepartmentAmount.Left = 5.48F;
            this.lblSpaceDepartmentAmount.Name = "lblSpaceDepartmentAmount";
            this.lblSpaceDepartmentAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblSpaceDepartmentAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblSpaceDepartmentAmount.Text = "";
            this.lblSpaceDepartmentAmount.Top = 0F;
            this.lblSpaceDepartmentAmount.Width = 5.154494F;
            // 
            // lineFooterVerDepartmentAmount
            // 
            this.lineFooterVerDepartmentAmount.Height = 0.3F;
            this.lineFooterVerDepartmentAmount.Left = 5.5F;
            this.lineFooterVerDepartmentAmount.LineWeight = 1F;
            this.lineFooterVerDepartmentAmount.Name = "lineFooterVerDepartmentAmount";
            this.lineFooterVerDepartmentAmount.Top = 0F;
            this.lineFooterVerDepartmentAmount.Width = 0F;
            this.lineFooterVerDepartmentAmount.X1 = 5.5F;
            this.lineFooterVerDepartmentAmount.X2 = 5.5F;
            this.lineFooterVerDepartmentAmount.Y1 = 0F;
            this.lineFooterVerDepartmentAmount.Y2 = 0.3F;
            // 
            // lineFooterHorDepartmentAmount
            // 
            this.lineFooterHorDepartmentAmount.Height = 0F;
            this.lineFooterHorDepartmentAmount.Left = 0F;
            this.lineFooterHorDepartmentAmount.LineWeight = 1F;
            this.lineFooterHorDepartmentAmount.Name = "lineFooterHorDepartmentAmount";
            this.lineFooterHorDepartmentAmount.Top = 0.3F;
            this.lineFooterHorDepartmentAmount.Width = 10.634F;
            this.lineFooterHorDepartmentAmount.X1 = 0F;
            this.lineFooterHorDepartmentAmount.X2 = 10.634F;
            this.lineFooterHorDepartmentAmount.Y1 = 0.3F;
            this.lineFooterHorDepartmentAmount.Y2 = 0.3F;
            // 
            // groupHeader3
            // 
            this.groupHeader3.Height = 0F;
            this.groupHeader3.Name = "groupHeader3";
            // 
            // groupFooter3
            // 
            this.groupFooter3.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblStaffTotal,
            this.txtStaffAmount,
            this.lineFooterVerStaffTotal,
            this.lblSpaceStaffAmount,
            this.lineFooterVerStaffAmount,
            this.lineFooterHorStaffAmountTotal});
            this.groupFooter3.Height = 0.3075787F;
            this.groupFooter3.Name = "groupFooter3";
            // 
            // lblStaffTotal
            // 
            this.lblStaffTotal.Height = 0.3F;
            this.lblStaffTotal.HyperLink = null;
            this.lblStaffTotal.Left = 0F;
            this.lblStaffTotal.Name = "lblStaffTotal";
            this.lblStaffTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblStaffTotal.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblStaffTotal.Text = "担当者計";
            this.lblStaffTotal.Top = 0F;
            this.lblStaffTotal.Width = 4.5F;
            // 
            // txtStaffAmount
            // 
            this.txtStaffAmount.DataField = "TotalAmount";
            this.txtStaffAmount.Height = 0.3F;
            this.txtStaffAmount.Left = 4.5F;
            this.txtStaffAmount.Name = "txtStaffAmount";
            this.txtStaffAmount.OutputFormat = resources.GetString("txtStaffAmount.OutputFormat");
            this.txtStaffAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtStaffAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtStaffAmount.SummaryGroup = "groupHeader3";
            this.txtStaffAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtStaffAmount.Text = null;
            this.txtStaffAmount.Top = 0F;
            this.txtStaffAmount.Width = 0.98F;
            // 
            // lineFooterVerStaffTotal
            // 
            this.lineFooterVerStaffTotal.Height = 0.3F;
            this.lineFooterVerStaffTotal.Left = 4.5F;
            this.lineFooterVerStaffTotal.LineWeight = 1F;
            this.lineFooterVerStaffTotal.Name = "lineFooterVerStaffTotal";
            this.lineFooterVerStaffTotal.Top = 0F;
            this.lineFooterVerStaffTotal.Width = 0F;
            this.lineFooterVerStaffTotal.X1 = 4.5F;
            this.lineFooterVerStaffTotal.X2 = 4.5F;
            this.lineFooterVerStaffTotal.Y1 = 0F;
            this.lineFooterVerStaffTotal.Y2 = 0.3F;
            // 
            // lblSpaceStaffAmount
            // 
            this.lblSpaceStaffAmount.Height = 0.3F;
            this.lblSpaceStaffAmount.HyperLink = null;
            this.lblSpaceStaffAmount.Left = 5.48F;
            this.lblSpaceStaffAmount.Name = "lblSpaceStaffAmount";
            this.lblSpaceStaffAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblSpaceStaffAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblSpaceStaffAmount.Text = "";
            this.lblSpaceStaffAmount.Top = 0F;
            this.lblSpaceStaffAmount.Width = 5.153999F;
            // 
            // lineFooterVerStaffAmount
            // 
            this.lineFooterVerStaffAmount.Height = 0.3F;
            this.lineFooterVerStaffAmount.Left = 5.5F;
            this.lineFooterVerStaffAmount.LineWeight = 1F;
            this.lineFooterVerStaffAmount.Name = "lineFooterVerStaffAmount";
            this.lineFooterVerStaffAmount.Top = 0F;
            this.lineFooterVerStaffAmount.Width = 0F;
            this.lineFooterVerStaffAmount.X1 = 5.5F;
            this.lineFooterVerStaffAmount.X2 = 5.5F;
            this.lineFooterVerStaffAmount.Y1 = 0F;
            this.lineFooterVerStaffAmount.Y2 = 0.3F;
            // 
            // lineFooterHorStaffAmountTotal
            // 
            this.lineFooterHorStaffAmountTotal.Height = 0F;
            this.lineFooterHorStaffAmountTotal.Left = 0F;
            this.lineFooterHorStaffAmountTotal.LineWeight = 1F;
            this.lineFooterHorStaffAmountTotal.Name = "lineFooterHorStaffAmountTotal";
            this.lineFooterHorStaffAmountTotal.Top = 0.3F;
            this.lineFooterHorStaffAmountTotal.Width = 10.634F;
            this.lineFooterHorStaffAmountTotal.X1 = 0F;
            this.lineFooterHorStaffAmountTotal.X2 = 10.634F;
            this.lineFooterHorStaffAmountTotal.Y1 = 0.3F;
            this.lineFooterHorStaffAmountTotal.Y2 = 0.3F;
            // 
            // groupHeader4
            // 
            this.groupHeader4.Height = 0F;
            this.groupHeader4.Name = "groupHeader4";
            // 
            // groupFooter4
            // 
            this.groupFooter4.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCustomerTotal,
            this.txtCustomerAmount,
            this.lineFooterVerCustomerTotal,
            this.lblSpaceCustomerAmount,
            this.lineFooterVerCustomerAmount,
            this.lineFooterHorCustomerTotal});
            this.groupFooter4.Height = 0.3075788F;
            this.groupFooter4.Name = "groupFooter4";
            // 
            // lblCustomerTotal
            // 
            this.lblCustomerTotal.Height = 0.3F;
            this.lblCustomerTotal.HyperLink = null;
            this.lblCustomerTotal.Left = 0F;
            this.lblCustomerTotal.Name = "lblCustomerTotal";
            this.lblCustomerTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblCustomerTotal.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblCustomerTotal.Text = "得意先計";
            this.lblCustomerTotal.Top = 0F;
            this.lblCustomerTotal.Width = 4.5F;
            // 
            // txtCustomerAmount
            // 
            this.txtCustomerAmount.DataField = "TotalAmount";
            this.txtCustomerAmount.Height = 0.3F;
            this.txtCustomerAmount.Left = 4.5F;
            this.txtCustomerAmount.Name = "txtCustomerAmount";
            this.txtCustomerAmount.OutputFormat = resources.GetString("txtCustomerAmount.OutputFormat");
            this.txtCustomerAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtCustomerAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtCustomerAmount.SummaryGroup = "groupHeader4";
            this.txtCustomerAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtCustomerAmount.Text = null;
            this.txtCustomerAmount.Top = 0F;
            this.txtCustomerAmount.Width = 0.98F;
            // 
            // lineFooterVerCustomerTotal
            // 
            this.lineFooterVerCustomerTotal.Height = 0.299F;
            this.lineFooterVerCustomerTotal.Left = 4.5F;
            this.lineFooterVerCustomerTotal.LineWeight = 1F;
            this.lineFooterVerCustomerTotal.Name = "lineFooterVerCustomerTotal";
            this.lineFooterVerCustomerTotal.Top = 0.001F;
            this.lineFooterVerCustomerTotal.Width = 0F;
            this.lineFooterVerCustomerTotal.X1 = 4.5F;
            this.lineFooterVerCustomerTotal.X2 = 4.5F;
            this.lineFooterVerCustomerTotal.Y1 = 0.001F;
            this.lineFooterVerCustomerTotal.Y2 = 0.3F;
            // 
            // lblSpaceCustomerAmount
            // 
            this.lblSpaceCustomerAmount.Height = 0.3F;
            this.lblSpaceCustomerAmount.HyperLink = null;
            this.lblSpaceCustomerAmount.Left = 5.5F;
            this.lblSpaceCustomerAmount.Name = "lblSpaceCustomerAmount";
            this.lblSpaceCustomerAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblSpaceCustomerAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblSpaceCustomerAmount.Text = "";
            this.lblSpaceCustomerAmount.Top = 0F;
            this.lblSpaceCustomerAmount.Width = 5.134495F;
            // 
            // lineFooterVerCustomerAmount
            // 
            this.lineFooterVerCustomerAmount.Height = 0.3F;
            this.lineFooterVerCustomerAmount.Left = 5.5F;
            this.lineFooterVerCustomerAmount.LineWeight = 1F;
            this.lineFooterVerCustomerAmount.Name = "lineFooterVerCustomerAmount";
            this.lineFooterVerCustomerAmount.Top = 0F;
            this.lineFooterVerCustomerAmount.Width = 0F;
            this.lineFooterVerCustomerAmount.X1 = 5.5F;
            this.lineFooterVerCustomerAmount.X2 = 5.5F;
            this.lineFooterVerCustomerAmount.Y1 = 0F;
            this.lineFooterVerCustomerAmount.Y2 = 0.3F;
            // 
            // lineFooterHorCustomerTotal
            // 
            this.lineFooterHorCustomerTotal.Height = 0F;
            this.lineFooterHorCustomerTotal.Left = 0F;
            this.lineFooterHorCustomerTotal.LineWeight = 1F;
            this.lineFooterHorCustomerTotal.Name = "lineFooterHorCustomerTotal";
            this.lineFooterHorCustomerTotal.Top = 0.3F;
            this.lineFooterHorCustomerTotal.Width = 10.634F;
            this.lineFooterHorCustomerTotal.X1 = 0F;
            this.lineFooterHorCustomerTotal.X2 = 10.634F;
            this.lineFooterHorCustomerTotal.Y1 = 0.3F;
            this.lineFooterHorCustomerTotal.Y2 = 0.3F;
            // 
            // groupHeader5
            // 
            this.groupHeader5.Height = 0F;
            this.groupHeader5.Name = "groupHeader5";
            // 
            // groupFooter5
            // 
            this.groupFooter5.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblDueAtTotal,
            this.txtDueAtAmount,
            this.lineFooterVerDutAtTotal,
            this.lblSpaceDueAtAmount,
            this.lineFooterVerDueAtAmount,
            this.lineFooterHorDueAtTotal});
            this.groupFooter5.Height = 0.3075787F;
            this.groupFooter5.Name = "groupFooter5";
            // 
            // lblDueAtTotal
            // 
            this.lblDueAtTotal.Height = 0.3F;
            this.lblDueAtTotal.HyperLink = null;
            this.lblDueAtTotal.Left = 0F;
            this.lblDueAtTotal.Name = "lblDueAtTotal";
            this.lblDueAtTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblDueAtTotal.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblDueAtTotal.Text = "予定日計";
            this.lblDueAtTotal.Top = 0F;
            this.lblDueAtTotal.Width = 4.5F;
            // 
            // txtDueAtAmount
            // 
            this.txtDueAtAmount.DataField = "TotalAmount";
            this.txtDueAtAmount.Height = 0.3F;
            this.txtDueAtAmount.Left = 4.5F;
            this.txtDueAtAmount.Name = "txtDueAtAmount";
            this.txtDueAtAmount.OutputFormat = resources.GetString("txtDueAtAmount.OutputFormat");
            this.txtDueAtAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtDueAtAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtDueAtAmount.SummaryGroup = "groupHeader5";
            this.txtDueAtAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtDueAtAmount.Text = null;
            this.txtDueAtAmount.Top = 0F;
            this.txtDueAtAmount.Width = 0.98F;
            // 
            // lineFooterVerDutAtTotal
            // 
            this.lineFooterVerDutAtTotal.Height = 0.3F;
            this.lineFooterVerDutAtTotal.Left = 4.5F;
            this.lineFooterVerDutAtTotal.LineWeight = 1F;
            this.lineFooterVerDutAtTotal.Name = "lineFooterVerDutAtTotal";
            this.lineFooterVerDutAtTotal.Top = 0F;
            this.lineFooterVerDutAtTotal.Width = 0F;
            this.lineFooterVerDutAtTotal.X1 = 4.5F;
            this.lineFooterVerDutAtTotal.X2 = 4.5F;
            this.lineFooterVerDutAtTotal.Y1 = 0F;
            this.lineFooterVerDutAtTotal.Y2 = 0.3F;
            // 
            // lblSpaceDueAtAmount
            // 
            this.lblSpaceDueAtAmount.Height = 0.3F;
            this.lblSpaceDueAtAmount.HyperLink = null;
            this.lblSpaceDueAtAmount.Left = 5.48F;
            this.lblSpaceDueAtAmount.Name = "lblSpaceDueAtAmount";
            this.lblSpaceDueAtAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblSpaceDueAtAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblSpaceDueAtAmount.Text = "";
            this.lblSpaceDueAtAmount.Top = 0F;
            this.lblSpaceDueAtAmount.Width = 5.153999F;
            // 
            // lineFooterVerDueAtAmount
            // 
            this.lineFooterVerDueAtAmount.Height = 0.3F;
            this.lineFooterVerDueAtAmount.Left = 5.5F;
            this.lineFooterVerDueAtAmount.LineWeight = 1F;
            this.lineFooterVerDueAtAmount.Name = "lineFooterVerDueAtAmount";
            this.lineFooterVerDueAtAmount.Top = 0F;
            this.lineFooterVerDueAtAmount.Width = 0F;
            this.lineFooterVerDueAtAmount.X1 = 5.5F;
            this.lineFooterVerDueAtAmount.X2 = 5.5F;
            this.lineFooterVerDueAtAmount.Y1 = 0F;
            this.lineFooterVerDueAtAmount.Y2 = 0.3F;
            // 
            // lineFooterHorDueAtTotal
            // 
            this.lineFooterHorDueAtTotal.Height = 0F;
            this.lineFooterHorDueAtTotal.Left = 0F;
            this.lineFooterHorDueAtTotal.LineWeight = 1F;
            this.lineFooterHorDueAtTotal.Name = "lineFooterHorDueAtTotal";
            this.lineFooterHorDueAtTotal.Top = 0.3F;
            this.lineFooterHorDueAtTotal.Width = 10.62992F;
            this.lineFooterHorDueAtTotal.X1 = 0F;
            this.lineFooterHorDueAtTotal.X2 = 10.62992F;
            this.lineFooterHorDueAtTotal.Y1 = 0.3F;
            this.lineFooterHorDueAtTotal.Y2 = 0.3F;
            // 
            // ScheduledPaymentListSectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.63449F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.groupHeader1);
            this.Sections.Add(this.groupHeader2);
            this.Sections.Add(this.groupHeader3);
            this.Sections.Add(this.groupHeader4);
            this.Sections.Add(this.groupHeader5);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.groupFooter5);
            this.Sections.Add(this.groupFooter4);
            this.Sections.Add(this.groupFooter3);
            this.Sections.Add(this.groupFooter2);
            this.Sections.Add(this.groupFooter1);
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
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOrginalDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCollectCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOriginalDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceDepartmentAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceStaffAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceCustomerAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAtTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpaceDueAtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblOrginalDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCollectCategory;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDelay;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerOrginalDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerInvoiceNo;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCollectCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader2;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter2;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader3;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter3;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader4;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter4;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader5;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter5;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtOriginalDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCollectCategory;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDelay;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerDutAtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerInvoiceNo;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCollectCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpaceDueAtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerDueAtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpaceTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerDepartmentTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpaceDepartmentAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerDepartmentAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorDepartmentAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerStaffTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpaceStaffAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerStaffAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorStaffAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerCustomerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpaceCustomerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerCustomerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorCustomerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorDueAtTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerOriginalDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorInvoiceNo;
    }
}
