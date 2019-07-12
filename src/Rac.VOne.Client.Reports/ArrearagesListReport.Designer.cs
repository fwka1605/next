namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillSectionReport の概要の説明です。
    /// </summary>
    partial class ArrearagesListReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ArrearagesListReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRemainingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTel = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSalesAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblOriginalDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblArrearagesDayCount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerTel = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblBillingId = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerBillingId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerOrginalDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRemainingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtClosingAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtID = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTel = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtOriginalDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerNote = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSalesAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtArrearagesDayCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerBillingId = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerOrginalDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerTel = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAmountTotalSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterHorTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader2 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter2 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtDepartmentAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblDepartmentAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentAmountSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerDepartmentAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorDepartmentAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader3 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter3 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtStaffAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblStaffAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffAmountSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerStaff = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorStaffAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerStaffAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtCustomerAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.groupHeader4 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter4 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblCustomerAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerAmountSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerCustomer = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerCustomerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterHorCustomerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader5 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter5 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtDueAtAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblDueAtAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAtAmountSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterHorDueAtAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerDueAtAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOriginalDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrearagesDayCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOriginalDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrearagesDayCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmountTotalSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentAmountSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffAmountSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerAmountSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAtAmountSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblNote,
            this.lblInvoiceCode,
            this.lblRemainingAmount,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblCustomerCode,
            this.lblCustomerName,
            this.lblTel,
            this.lblCustomerNote,
            this.lblBilledAt,
            this.lblSalesAt,
            this.lblClosingAt,
            this.lblDueAt,
            this.lblOriginalDueAt,
            this.lblArrearagesDayCount,
            this.lblDepartmentCode,
            this.lblDepartmentName,
            this.lblStaffCode,
            this.lblStaffName,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderVerTel,
            this.lineHeaderVerBilledAt,
            this.lineHeaderVerClosingAt,
            this.lineHeaderVerInvoiceCode,
            this.lineHeaderVerDepartmentCode,
            this.lblBillingId,
            this.lineHeaderVerBillingId,
            this.lineHeaderHorLower,
            this.lineHeaderHorUpper,
            this.lineHeaderVerOrginalDueAt,
            this.lineHeaderVerRemainingAmount,
            this.lineHeaderHorCustomerCode,
            this.lineHeaderHorInvoiceCode});
            this.pageHeader.Height = 1.008F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.2F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 6.7F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblNote.Text = "備考";
            this.lblNote.Top = 0.8F;
            this.lblNote.Width = 1.5F;
            // 
            // lblInvoiceCode
            // 
            this.lblInvoiceCode.Height = 0.2F;
            this.lblInvoiceCode.HyperLink = null;
            this.lblInvoiceCode.Left = 6.7F;
            this.lblInvoiceCode.Name = "lblInvoiceCode";
            this.lblInvoiceCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblInvoiceCode.Text = "請求書番号";
            this.lblInvoiceCode.Top = 0.6F;
            this.lblInvoiceCode.Width = 1.5F;
            // 
            // lblRemainingAmount
            // 
            this.lblRemainingAmount.Height = 0.4F;
            this.lblRemainingAmount.HyperLink = null;
            this.lblRemainingAmount.Left = 5.7F;
            this.lblRemainingAmount.Name = "lblRemainingAmount";
            this.lblRemainingAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblRemainingAmount.Text = "回収予定金額";
            this.lblRemainingAmount.Top = 0.6F;
            this.lblRemainingAmount.Width = 1F;
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
            this.lblCustomerCode.Height = 0.2F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 0.6F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.6F;
            this.lblCustomerCode.Width = 1.9F;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Height = 0.2F;
            this.lblCustomerName.HyperLink = null;
            this.lblCustomerName.Left = 0.6F;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.Top = 0.8F;
            this.lblCustomerName.Width = 1.9F;
            // 
            // lblTel
            // 
            this.lblTel.Height = 0.2F;
            this.lblTel.HyperLink = null;
            this.lblTel.Left = 2.5F;
            this.lblTel.Name = "lblTel";
            this.lblTel.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblTel.Text = "得意先電話番号";
            this.lblTel.Top = 0.6F;
            this.lblTel.Width = 1.4F;
            // 
            // lblCustomerNote
            // 
            this.lblCustomerNote.Height = 0.2F;
            this.lblCustomerNote.HyperLink = null;
            this.lblCustomerNote.Left = 2.5F;
            this.lblCustomerNote.Name = "lblCustomerNote";
            this.lblCustomerNote.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerNote.Text = "得意先備考";
            this.lblCustomerNote.Top = 0.8F;
            this.lblCustomerNote.Width = 1.4F;
            // 
            // lblBilledAt
            // 
            this.lblBilledAt.Height = 0.2F;
            this.lblBilledAt.HyperLink = null;
            this.lblBilledAt.Left = 3.9F;
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
            this.lblSalesAt.Left = 3.9F;
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
            this.lblClosingAt.Left = 4.5F;
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
            this.lblDueAt.Left = 4.5F;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDueAt.Text = "入金予定日";
            this.lblDueAt.Top = 0.8F;
            this.lblDueAt.Width = 0.6F;
            // 
            // lblOriginalDueAt
            // 
            this.lblOriginalDueAt.Height = 0.2F;
            this.lblOriginalDueAt.HyperLink = null;
            this.lblOriginalDueAt.Left = 5.1F;
            this.lblOriginalDueAt.Name = "lblOriginalDueAt";
            this.lblOriginalDueAt.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblOriginalDueAt.Text = "当初予定日";
            this.lblOriginalDueAt.Top = 0.6F;
            this.lblOriginalDueAt.Width = 0.6F;
            // 
            // lblArrearagesDayCount
            // 
            this.lblArrearagesDayCount.Height = 0.2F;
            this.lblArrearagesDayCount.HyperLink = null;
            this.lblArrearagesDayCount.Left = 5.1F;
            this.lblArrearagesDayCount.Name = "lblArrearagesDayCount";
            this.lblArrearagesDayCount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblArrearagesDayCount.Text = "滞留日数";
            this.lblArrearagesDayCount.Top = 0.8F;
            this.lblArrearagesDayCount.Width = 0.6F;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Height = 0.2F;
            this.lblDepartmentCode.HyperLink = null;
            this.lblDepartmentCode.Left = 8.200001F;
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDepartmentCode.Text = "請求部門コード";
            this.lblDepartmentCode.Top = 0.6F;
            this.lblDepartmentCode.Width = 1.2F;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Height = 0.2F;
            this.lblDepartmentName.HyperLink = null;
            this.lblDepartmentName.Left = 8.200001F;
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDepartmentName.Text = "請求部門名";
            this.lblDepartmentName.Top = 0.8F;
            this.lblDepartmentName.Width = 1.2F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.2F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 9.400001F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblStaffCode.Text = "担当者コード";
            this.lblStaffCode.Top = 0.6F;
            this.lblStaffCode.Width = 1.25F;
            // 
            // lblStaffName
            // 
            this.lblStaffName.Height = 0.2F;
            this.lblStaffName.HyperLink = null;
            this.lblStaffName.Left = 9.400001F;
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblStaffName.Text = "担当者名";
            this.lblStaffName.Top = 0.8F;
            this.lblStaffName.Width = 1.25F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.4F;
            this.lineHeaderVerCustomerCode.Left = 2.5F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.608F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 2.5F;
            this.lineHeaderVerCustomerCode.X2 = 2.5F;
            this.lineHeaderVerCustomerCode.Y1 = 0.608F;
            this.lineHeaderVerCustomerCode.Y2 = 1.008F;
            // 
            // lineHeaderVerTel
            // 
            this.lineHeaderVerTel.Height = 0.4F;
            this.lineHeaderVerTel.Left = 3.9F;
            this.lineHeaderVerTel.LineWeight = 1F;
            this.lineHeaderVerTel.Name = "lineHeaderVerTel";
            this.lineHeaderVerTel.Top = 0.6F;
            this.lineHeaderVerTel.Width = 0F;
            this.lineHeaderVerTel.X1 = 3.9F;
            this.lineHeaderVerTel.X2 = 3.9F;
            this.lineHeaderVerTel.Y1 = 0.6F;
            this.lineHeaderVerTel.Y2 = 1F;
            // 
            // lineHeaderVerBilledAt
            // 
            this.lineHeaderVerBilledAt.Height = 0.4F;
            this.lineHeaderVerBilledAt.Left = 4.5F;
            this.lineHeaderVerBilledAt.LineWeight = 1F;
            this.lineHeaderVerBilledAt.Name = "lineHeaderVerBilledAt";
            this.lineHeaderVerBilledAt.Top = 0.6F;
            this.lineHeaderVerBilledAt.Width = 0F;
            this.lineHeaderVerBilledAt.X1 = 4.5F;
            this.lineHeaderVerBilledAt.X2 = 4.5F;
            this.lineHeaderVerBilledAt.Y1 = 0.6F;
            this.lineHeaderVerBilledAt.Y2 = 1F;
            // 
            // lineHeaderVerClosingAt
            // 
            this.lineHeaderVerClosingAt.Height = 0.4F;
            this.lineHeaderVerClosingAt.Left = 5.1F;
            this.lineHeaderVerClosingAt.LineWeight = 1F;
            this.lineHeaderVerClosingAt.Name = "lineHeaderVerClosingAt";
            this.lineHeaderVerClosingAt.Top = 0.6F;
            this.lineHeaderVerClosingAt.Width = 0F;
            this.lineHeaderVerClosingAt.X1 = 5.1F;
            this.lineHeaderVerClosingAt.X2 = 5.1F;
            this.lineHeaderVerClosingAt.Y1 = 0.6F;
            this.lineHeaderVerClosingAt.Y2 = 1F;
            // 
            // lineHeaderVerInvoiceCode
            // 
            this.lineHeaderVerInvoiceCode.Height = 0.4F;
            this.lineHeaderVerInvoiceCode.Left = 8.200001F;
            this.lineHeaderVerInvoiceCode.LineWeight = 1F;
            this.lineHeaderVerInvoiceCode.Name = "lineHeaderVerInvoiceCode";
            this.lineHeaderVerInvoiceCode.Top = 0.6F;
            this.lineHeaderVerInvoiceCode.Width = 0F;
            this.lineHeaderVerInvoiceCode.X1 = 8.200001F;
            this.lineHeaderVerInvoiceCode.X2 = 8.200001F;
            this.lineHeaderVerInvoiceCode.Y1 = 0.6F;
            this.lineHeaderVerInvoiceCode.Y2 = 1F;
            // 
            // lineHeaderVerDepartmentCode
            // 
            this.lineHeaderVerDepartmentCode.Height = 0.4F;
            this.lineHeaderVerDepartmentCode.Left = 9.400001F;
            this.lineHeaderVerDepartmentCode.LineWeight = 1F;
            this.lineHeaderVerDepartmentCode.Name = "lineHeaderVerDepartmentCode";
            this.lineHeaderVerDepartmentCode.Top = 0.6F;
            this.lineHeaderVerDepartmentCode.Width = 0F;
            this.lineHeaderVerDepartmentCode.X1 = 9.400001F;
            this.lineHeaderVerDepartmentCode.X2 = 9.400001F;
            this.lineHeaderVerDepartmentCode.Y1 = 0.6F;
            this.lineHeaderVerDepartmentCode.Y2 = 1F;
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
            this.lblBillingId.Width = 0.6F;
            // 
            // lineHeaderVerBillingId
            // 
            this.lineHeaderVerBillingId.Height = 0.4F;
            this.lineHeaderVerBillingId.Left = 0.6F;
            this.lineHeaderVerBillingId.LineWeight = 1F;
            this.lineHeaderVerBillingId.Name = "lineHeaderVerBillingId";
            this.lineHeaderVerBillingId.Top = 0.6F;
            this.lineHeaderVerBillingId.Width = 0F;
            this.lineHeaderVerBillingId.X1 = 0.6F;
            this.lineHeaderVerBillingId.X2 = 0.6F;
            this.lineHeaderVerBillingId.Y1 = 0.6F;
            this.lineHeaderVerBillingId.Y2 = 1F;
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
            // lineHeaderVerOrginalDueAt
            // 
            this.lineHeaderVerOrginalDueAt.Height = 0.3998268F;
            this.lineHeaderVerOrginalDueAt.Left = 5.7F;
            this.lineHeaderVerOrginalDueAt.LineWeight = 1F;
            this.lineHeaderVerOrginalDueAt.Name = "lineHeaderVerOrginalDueAt";
            this.lineHeaderVerOrginalDueAt.Top = 0.6F;
            this.lineHeaderVerOrginalDueAt.Width = 0F;
            this.lineHeaderVerOrginalDueAt.X1 = 5.7F;
            this.lineHeaderVerOrginalDueAt.X2 = 5.7F;
            this.lineHeaderVerOrginalDueAt.Y1 = 0.6F;
            this.lineHeaderVerOrginalDueAt.Y2 = 0.9998268F;
            // 
            // lineHeaderVerRemainingAmount
            // 
            this.lineHeaderVerRemainingAmount.Height = 0.4F;
            this.lineHeaderVerRemainingAmount.Left = 6.7F;
            this.lineHeaderVerRemainingAmount.LineWeight = 1F;
            this.lineHeaderVerRemainingAmount.Name = "lineHeaderVerRemainingAmount";
            this.lineHeaderVerRemainingAmount.Top = 0.6F;
            this.lineHeaderVerRemainingAmount.Width = 0F;
            this.lineHeaderVerRemainingAmount.X1 = 6.7F;
            this.lineHeaderVerRemainingAmount.X2 = 6.7F;
            this.lineHeaderVerRemainingAmount.Y1 = 0.6F;
            this.lineHeaderVerRemainingAmount.Y2 = 1F;
            // 
            // lineHeaderHorCustomerCode
            // 
            this.lineHeaderHorCustomerCode.Height = 0F;
            this.lineHeaderHorCustomerCode.Left = 0.6F;
            this.lineHeaderHorCustomerCode.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorCustomerCode.LineWeight = 1F;
            this.lineHeaderHorCustomerCode.Name = "lineHeaderHorCustomerCode";
            this.lineHeaderHorCustomerCode.Top = 0.8F;
            this.lineHeaderHorCustomerCode.Width = 5.1F;
            this.lineHeaderHorCustomerCode.X1 = 0.6F;
            this.lineHeaderHorCustomerCode.X2 = 5.7F;
            this.lineHeaderHorCustomerCode.Y1 = 0.8F;
            this.lineHeaderHorCustomerCode.Y2 = 0.8F;
            // 
            // lineHeaderHorInvoiceCode
            // 
            this.lineHeaderHorInvoiceCode.Height = 0F;
            this.lineHeaderHorInvoiceCode.Left = 6.7F;
            this.lineHeaderHorInvoiceCode.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorInvoiceCode.LineWeight = 1F;
            this.lineHeaderHorInvoiceCode.Name = "lineHeaderHorInvoiceCode";
            this.lineHeaderHorInvoiceCode.Top = 0.8F;
            this.lineHeaderHorInvoiceCode.Width = 3.949F;
            this.lineHeaderHorInvoiceCode.X1 = 6.7F;
            this.lineHeaderHorInvoiceCode.X2 = 10.649F;
            this.lineHeaderHorInvoiceCode.Y1 = 0.8F;
            this.lineHeaderHorInvoiceCode.Y2 = 0.8F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtInvoiceCode,
            this.txtNote1,
            this.txtRemainAmount,
            this.txtCustomerName,
            this.txtClosingAt,
            this.txtStaffCode,
            this.lineDetailVerRemainAmount,
            this.txtID,
            this.txtDepartmentName,
            this.txtTel,
            this.txtBilledAt,
            this.txtOriginalDueAt,
            this.txtCustomerCode,
            this.txtDepartmentCode,
            this.txtStaffName,
            this.txtCustomerNote,
            this.txtSalesAt,
            this.txtDueAt,
            this.txtArrearagesDayCount,
            this.lineDetailVerBillingId,
            this.lineDetailVerInvoiceCode,
            this.lineDetailVerOrginalDueAt,
            this.lineDetailVerClosingAt,
            this.lineDetailVerBilledAt,
            this.lineDetailVerCustomerCode,
            this.lineDetailVerTel,
            this.lineDetailHorLower,
            this.lineDetailVerDepartmentCode});
            this.detail.Height = 0.408F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.DataField = "InvoiceCode";
            this.txtInvoiceCode.Height = 0.2F;
            this.txtInvoiceCode.Left = 6.72F;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Style = "font-size: 6pt; white-space: nowrap; ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtInvoiceCode.Text = null;
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 1.48F;
            // 
            // txtNote1
            // 
            this.txtNote1.DataField = "Note1";
            this.txtNote1.Height = 0.2F;
            this.txtNote1.Left = 6.72F;
            this.txtNote1.Name = "txtNote1";
            this.txtNote1.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtNote1.Text = null;
            this.txtNote1.Top = 0.2F;
            this.txtNote1.Width = 1.48F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.DataField = "TotalAmount";
            this.txtRemainAmount.Height = 0.4F;
            this.txtRemainAmount.Left = 5.7F;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; white-space: nowrap; d" +
    "do-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtRemainAmount.Text = null;
            this.txtRemainAmount.Top = 0F;
            this.txtRemainAmount.Width = 1F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.DataField = "CustomerName";
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 0.62F;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCustomerName.Text = null;
            this.txtCustomerName.Top = 0.2F;
            this.txtCustomerName.Width = 1.88F;
            // 
            // txtClosingAt
            // 
            this.txtClosingAt.DataField = "ClosingAt";
            this.txtClosingAt.Height = 0.2F;
            this.txtClosingAt.Left = 4.5F;
            this.txtClosingAt.Name = "txtClosingAt";
            this.txtClosingAt.OutputFormat = resources.GetString("txtClosingAt.OutputFormat");
            this.txtClosingAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtClosingAt.Text = null;
            this.txtClosingAt.Top = 0F;
            this.txtClosingAt.Width = 0.6F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.DataField = "StaffCode";
            this.txtStaffCode.Height = 0.2F;
            this.txtStaffCode.Left = 9.42F;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtStaffCode.Text = null;
            this.txtStaffCode.Top = 0F;
            this.txtStaffCode.Width = 1.23F;
            // 
            // lineDetailVerRemainAmount
            // 
            this.lineDetailVerRemainAmount.Height = 0.4F;
            this.lineDetailVerRemainAmount.Left = 6.7F;
            this.lineDetailVerRemainAmount.LineWeight = 1F;
            this.lineDetailVerRemainAmount.Name = "lineDetailVerRemainAmount";
            this.lineDetailVerRemainAmount.Top = 0F;
            this.lineDetailVerRemainAmount.Width = 0F;
            this.lineDetailVerRemainAmount.X1 = 6.7F;
            this.lineDetailVerRemainAmount.X2 = 6.7F;
            this.lineDetailVerRemainAmount.Y1 = 0F;
            this.lineDetailVerRemainAmount.Y2 = 0.4F;
            // 
            // txtID
            // 
            this.txtID.DataField = "Id";
            this.txtID.Height = 0.4F;
            this.txtID.Left = 0F;
            this.txtID.Name = "txtID";
            this.txtID.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtID.Style = "font-size: 6pt; text-align: right; vertical-align: middle; white-space: nowrap; d" +
    "do-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtID.Text = null;
            this.txtID.Top = 0F;
            this.txtID.Width = 0.58F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.DataField = "DepartmentName";
            this.txtDepartmentName.Height = 0.2F;
            this.txtDepartmentName.Left = 8.22F;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtDepartmentName.Text = null;
            this.txtDepartmentName.Top = 0.2F;
            this.txtDepartmentName.Width = 1.18F;
            // 
            // txtTel
            // 
            this.txtTel.DataField = "Tel";
            this.txtTel.Height = 0.2F;
            this.txtTel.Left = 2.52F;
            this.txtTel.Name = "txtTel";
            this.txtTel.OutputFormat = resources.GetString("txtTel.OutputFormat");
            this.txtTel.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtTel.Text = null;
            this.txtTel.Top = 0F;
            this.txtTel.Width = 1.38F;
            // 
            // txtBilledAt
            // 
            this.txtBilledAt.DataField = "BilledAt";
            this.txtBilledAt.Height = 0.2F;
            this.txtBilledAt.Left = 3.9F;
            this.txtBilledAt.Name = "txtBilledAt";
            this.txtBilledAt.OutputFormat = resources.GetString("txtBilledAt.OutputFormat");
            this.txtBilledAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBilledAt.Text = null;
            this.txtBilledAt.Top = 0F;
            this.txtBilledAt.Width = 0.6F;
            // 
            // txtOriginalDueAt
            // 
            this.txtOriginalDueAt.DataField = "OriginalDueAt";
            this.txtOriginalDueAt.Height = 0.2F;
            this.txtOriginalDueAt.Left = 5.1F;
            this.txtOriginalDueAt.Name = "txtOriginalDueAt";
            this.txtOriginalDueAt.OutputFormat = resources.GetString("txtOriginalDueAt.OutputFormat");
            this.txtOriginalDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtOriginalDueAt.Text = null;
            this.txtOriginalDueAt.Top = 0F;
            this.txtOriginalDueAt.Width = 0.6F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.DataField = "CustomerCode";
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 0.62F;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 1.88F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.DataField = "DepartmentCode";
            this.txtDepartmentCode.Height = 0.2F;
            this.txtDepartmentCode.Left = 8.22F;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtDepartmentCode.Text = null;
            this.txtDepartmentCode.Top = 0F;
            this.txtDepartmentCode.Width = 1.18F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.DataField = "StaffName";
            this.txtStaffName.Height = 0.2F;
            this.txtStaffName.Left = 9.42F;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtStaffName.Text = null;
            this.txtStaffName.Top = 0.208F;
            this.txtStaffName.Width = 1.23F;
            // 
            // txtCustomerNote
            // 
            this.txtCustomerNote.DataField = "Note";
            this.txtCustomerNote.Height = 0.2F;
            this.txtCustomerNote.Left = 2.52F;
            this.txtCustomerNote.Name = "txtCustomerNote";
            this.txtCustomerNote.OutputFormat = resources.GetString("txtCustomerNote.OutputFormat");
            this.txtCustomerNote.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCustomerNote.Text = null;
            this.txtCustomerNote.Top = 0.2F;
            this.txtCustomerNote.Width = 1.38F;
            // 
            // txtSalesAt
            // 
            this.txtSalesAt.DataField = "SalesAt";
            this.txtSalesAt.Height = 0.2F;
            this.txtSalesAt.Left = 3.9F;
            this.txtSalesAt.Name = "txtSalesAt";
            this.txtSalesAt.OutputFormat = resources.GetString("txtSalesAt.OutputFormat");
            this.txtSalesAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtSalesAt.Text = null;
            this.txtSalesAt.Top = 0.2F;
            this.txtSalesAt.Width = 0.6F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.DataField = "DueAt";
            this.txtDueAt.Height = 0.2F;
            this.txtDueAt.Left = 4.5F;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDueAt.Text = null;
            this.txtDueAt.Top = 0.2F;
            this.txtDueAt.Width = 0.5999999F;
            // 
            // txtArrearagesDayCount
            // 
            this.txtArrearagesDayCount.DataField = "Count";
            this.txtArrearagesDayCount.Height = 0.2F;
            this.txtArrearagesDayCount.Left = 5.1F;
            this.txtArrearagesDayCount.Name = "txtArrearagesDayCount";
            this.txtArrearagesDayCount.OutputFormat = resources.GetString("txtArrearagesDayCount.OutputFormat");
            this.txtArrearagesDayCount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; white-space: nowrap; d" +
    "do-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtArrearagesDayCount.Text = null;
            this.txtArrearagesDayCount.Top = 0.2F;
            this.txtArrearagesDayCount.Width = 0.6F;
            // 
            // lineDetailVerBillingId
            // 
            this.lineDetailVerBillingId.Height = 0.4F;
            this.lineDetailVerBillingId.Left = 0.6F;
            this.lineDetailVerBillingId.LineWeight = 1F;
            this.lineDetailVerBillingId.Name = "lineDetailVerBillingId";
            this.lineDetailVerBillingId.Top = 0F;
            this.lineDetailVerBillingId.Width = 0F;
            this.lineDetailVerBillingId.X1 = 0.6F;
            this.lineDetailVerBillingId.X2 = 0.6F;
            this.lineDetailVerBillingId.Y1 = 0F;
            this.lineDetailVerBillingId.Y2 = 0.4F;
            // 
            // lineDetailVerInvoiceCode
            // 
            this.lineDetailVerInvoiceCode.Height = 0.4F;
            this.lineDetailVerInvoiceCode.Left = 8.200001F;
            this.lineDetailVerInvoiceCode.LineWeight = 1F;
            this.lineDetailVerInvoiceCode.Name = "lineDetailVerInvoiceCode";
            this.lineDetailVerInvoiceCode.Top = 0F;
            this.lineDetailVerInvoiceCode.Width = 0F;
            this.lineDetailVerInvoiceCode.X1 = 8.200001F;
            this.lineDetailVerInvoiceCode.X2 = 8.200001F;
            this.lineDetailVerInvoiceCode.Y1 = 0F;
            this.lineDetailVerInvoiceCode.Y2 = 0.4F;
            // 
            // lineDetailVerOrginalDueAt
            // 
            this.lineDetailVerOrginalDueAt.Height = 0.4F;
            this.lineDetailVerOrginalDueAt.Left = 5.7F;
            this.lineDetailVerOrginalDueAt.LineWeight = 1F;
            this.lineDetailVerOrginalDueAt.Name = "lineDetailVerOrginalDueAt";
            this.lineDetailVerOrginalDueAt.Top = 0F;
            this.lineDetailVerOrginalDueAt.Width = 0F;
            this.lineDetailVerOrginalDueAt.X1 = 5.7F;
            this.lineDetailVerOrginalDueAt.X2 = 5.7F;
            this.lineDetailVerOrginalDueAt.Y1 = 0F;
            this.lineDetailVerOrginalDueAt.Y2 = 0.4F;
            // 
            // lineDetailVerClosingAt
            // 
            this.lineDetailVerClosingAt.Height = 0.4F;
            this.lineDetailVerClosingAt.Left = 5.1F;
            this.lineDetailVerClosingAt.LineWeight = 1F;
            this.lineDetailVerClosingAt.Name = "lineDetailVerClosingAt";
            this.lineDetailVerClosingAt.Top = 0.008F;
            this.lineDetailVerClosingAt.Width = 0F;
            this.lineDetailVerClosingAt.X1 = 5.1F;
            this.lineDetailVerClosingAt.X2 = 5.1F;
            this.lineDetailVerClosingAt.Y1 = 0.008F;
            this.lineDetailVerClosingAt.Y2 = 0.408F;
            // 
            // lineDetailVerBilledAt
            // 
            this.lineDetailVerBilledAt.Height = 0.4F;
            this.lineDetailVerBilledAt.Left = 4.499607F;
            this.lineDetailVerBilledAt.LineWeight = 1F;
            this.lineDetailVerBilledAt.Name = "lineDetailVerBilledAt";
            this.lineDetailVerBilledAt.Top = 0F;
            this.lineDetailVerBilledAt.Width = 0.0003929138F;
            this.lineDetailVerBilledAt.X1 = 4.5F;
            this.lineDetailVerBilledAt.X2 = 4.499607F;
            this.lineDetailVerBilledAt.Y1 = 0F;
            this.lineDetailVerBilledAt.Y2 = 0.4F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.4F;
            this.lineDetailVerCustomerCode.Left = 2.5F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 2.5F;
            this.lineDetailVerCustomerCode.X2 = 2.5F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.4F;
            // 
            // lineDetailVerTel
            // 
            this.lineDetailVerTel.Height = 0.4F;
            this.lineDetailVerTel.Left = 3.9F;
            this.lineDetailVerTel.LineWeight = 1F;
            this.lineDetailVerTel.Name = "lineDetailVerTel";
            this.lineDetailVerTel.Top = 0F;
            this.lineDetailVerTel.Width = 0F;
            this.lineDetailVerTel.X1 = 3.9F;
            this.lineDetailVerTel.X2 = 3.9F;
            this.lineDetailVerTel.Y1 = 0F;
            this.lineDetailVerTel.Y2 = 0.4F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.4F;
            this.lineDetailHorLower.Width = 10.65F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.65F;
            this.lineDetailHorLower.Y1 = 0.4F;
            this.lineDetailHorLower.Y2 = 0.4F;
            // 
            // lineDetailVerDepartmentCode
            // 
            this.lineDetailVerDepartmentCode.Height = 0.4F;
            this.lineDetailVerDepartmentCode.Left = 9.400001F;
            this.lineDetailVerDepartmentCode.LineWeight = 1F;
            this.lineDetailVerDepartmentCode.Name = "lineDetailVerDepartmentCode";
            this.lineDetailVerDepartmentCode.Top = 0F;
            this.lineDetailVerDepartmentCode.Width = 0F;
            this.lineDetailVerDepartmentCode.X1 = 9.400001F;
            this.lineDetailVerDepartmentCode.X2 = 9.400001F;
            this.lineDetailVerDepartmentCode.Y1 = 0F;
            this.lineDetailVerDepartmentCode.Y2 = 0.4F;
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
            this.reportInfo1.Left = 7.60315F;
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
            this.txtTotalAmount,
            this.lblTotalAmount,
            this.lblAmountTotalSpace,
            this.lineFooterHorTotalAmount,
            this.lineFooterVerTotal,
            this.lineFooterVerTotalAmount});
            this.groupFooter1.Height = 0.3075787F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.DataField = "TotalAmount";
            this.txtTotalAmount.Height = 0.3F;
            this.txtTotalAmount.Left = 5.7F;
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.OutputFormat = resources.GetString("txtTotalAmount.OutputFormat");
            this.txtTotalAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtTotalAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtTotalAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalAmount.Text = null;
            this.txtTotalAmount.Top = 0F;
            this.txtTotalAmount.Width = 1F;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Height = 0.3F;
            this.lblTotalAmount.HyperLink = null;
            this.lblTotalAmount.Left = 0F;
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblTotalAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblTotalAmount.Text = "総合計";
            this.lblTotalAmount.Top = 0F;
            this.lblTotalAmount.Width = 5.7F;
            // 
            // lblAmountTotalSpace
            // 
            this.lblAmountTotalSpace.Height = 0.3F;
            this.lblAmountTotalSpace.HyperLink = null;
            this.lblAmountTotalSpace.Left = 6.7F;
            this.lblAmountTotalSpace.Name = "lblAmountTotalSpace";
            this.lblAmountTotalSpace.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblAmountTotalSpace.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblAmountTotalSpace.Text = "";
            this.lblAmountTotalSpace.Top = 0F;
            this.lblAmountTotalSpace.Width = 3.95F;
            // 
            // lineFooterHorTotalAmount
            // 
            this.lineFooterHorTotalAmount.Height = 0F;
            this.lineFooterHorTotalAmount.Left = 0F;
            this.lineFooterHorTotalAmount.LineWeight = 1F;
            this.lineFooterHorTotalAmount.Name = "lineFooterHorTotalAmount";
            this.lineFooterHorTotalAmount.Top = 0.3F;
            this.lineFooterHorTotalAmount.Width = 10.65F;
            this.lineFooterHorTotalAmount.X1 = 0F;
            this.lineFooterHorTotalAmount.X2 = 10.65F;
            this.lineFooterHorTotalAmount.Y1 = 0.3F;
            this.lineFooterHorTotalAmount.Y2 = 0.3F;
            // 
            // lineFooterVerTotal
            // 
            this.lineFooterVerTotal.Height = 0.3F;
            this.lineFooterVerTotal.Left = 5.7F;
            this.lineFooterVerTotal.LineWeight = 1F;
            this.lineFooterVerTotal.Name = "lineFooterVerTotal";
            this.lineFooterVerTotal.Top = 0F;
            this.lineFooterVerTotal.Width = 0F;
            this.lineFooterVerTotal.X1 = 5.7F;
            this.lineFooterVerTotal.X2 = 5.7F;
            this.lineFooterVerTotal.Y1 = 0F;
            this.lineFooterVerTotal.Y2 = 0.3F;
            // 
            // lineFooterVerTotalAmount
            // 
            this.lineFooterVerTotalAmount.Height = 0.3F;
            this.lineFooterVerTotalAmount.Left = 6.7F;
            this.lineFooterVerTotalAmount.LineWeight = 1F;
            this.lineFooterVerTotalAmount.Name = "lineFooterVerTotalAmount";
            this.lineFooterVerTotalAmount.Top = 0F;
            this.lineFooterVerTotalAmount.Width = 0F;
            this.lineFooterVerTotalAmount.X1 = 6.7F;
            this.lineFooterVerTotalAmount.X2 = 6.7F;
            this.lineFooterVerTotalAmount.Y1 = 0F;
            this.lineFooterVerTotalAmount.Y2 = 0.3F;
            // 
            // groupHeader2
            // 
            this.groupHeader2.Height = 0F;
            this.groupHeader2.Name = "groupHeader2";
            // 
            // groupFooter2
            // 
            this.groupFooter2.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtDepartmentAmount,
            this.lblDepartmentAmount,
            this.lblDepartmentAmountSpace,
            this.lineFooterVerDepartment,
            this.lineFooterVerDepartmentAmount,
            this.lineFooterHorDepartmentAmount});
            this.groupFooter2.Height = 0.3075787F;
            this.groupFooter2.Name = "groupFooter2";
            // 
            // txtDepartmentAmount
            // 
            this.txtDepartmentAmount.DataField = "TotalAmount";
            this.txtDepartmentAmount.Height = 0.3F;
            this.txtDepartmentAmount.Left = 5.7F;
            this.txtDepartmentAmount.Name = "txtDepartmentAmount";
            this.txtDepartmentAmount.OutputFormat = resources.GetString("txtDepartmentAmount.OutputFormat");
            this.txtDepartmentAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtDepartmentAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtDepartmentAmount.SummaryGroup = "groupHeader2";
            this.txtDepartmentAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtDepartmentAmount.Text = null;
            this.txtDepartmentAmount.Top = 0F;
            this.txtDepartmentAmount.Width = 1F;
            // 
            // lblDepartmentAmount
            // 
            this.lblDepartmentAmount.Height = 0.3F;
            this.lblDepartmentAmount.HyperLink = null;
            this.lblDepartmentAmount.Left = 0F;
            this.lblDepartmentAmount.Name = "lblDepartmentAmount";
            this.lblDepartmentAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblDepartmentAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblDepartmentAmount.Text = "部門計";
            this.lblDepartmentAmount.Top = 0F;
            this.lblDepartmentAmount.Width = 5.7F;
            // 
            // lblDepartmentAmountSpace
            // 
            this.lblDepartmentAmountSpace.Height = 0.3F;
            this.lblDepartmentAmountSpace.HyperLink = null;
            this.lblDepartmentAmountSpace.Left = 6.7F;
            this.lblDepartmentAmountSpace.Name = "lblDepartmentAmountSpace";
            this.lblDepartmentAmountSpace.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblDepartmentAmountSpace.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblDepartmentAmountSpace.Text = "";
            this.lblDepartmentAmountSpace.Top = 0F;
            this.lblDepartmentAmountSpace.Width = 3.95F;
            // 
            // lineFooterVerDepartment
            // 
            this.lineFooterVerDepartment.Height = 0.3F;
            this.lineFooterVerDepartment.Left = 5.7F;
            this.lineFooterVerDepartment.LineWeight = 1F;
            this.lineFooterVerDepartment.Name = "lineFooterVerDepartment";
            this.lineFooterVerDepartment.Top = 0F;
            this.lineFooterVerDepartment.Width = 0F;
            this.lineFooterVerDepartment.X1 = 5.7F;
            this.lineFooterVerDepartment.X2 = 5.7F;
            this.lineFooterVerDepartment.Y1 = 0F;
            this.lineFooterVerDepartment.Y2 = 0.3F;
            // 
            // lineFooterVerDepartmentAmount
            // 
            this.lineFooterVerDepartmentAmount.Height = 0.3F;
            this.lineFooterVerDepartmentAmount.Left = 6.7F;
            this.lineFooterVerDepartmentAmount.LineWeight = 1F;
            this.lineFooterVerDepartmentAmount.Name = "lineFooterVerDepartmentAmount";
            this.lineFooterVerDepartmentAmount.Top = 0F;
            this.lineFooterVerDepartmentAmount.Width = 0F;
            this.lineFooterVerDepartmentAmount.X1 = 6.7F;
            this.lineFooterVerDepartmentAmount.X2 = 6.7F;
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
            this.lineFooterHorDepartmentAmount.Width = 10.62992F;
            this.lineFooterHorDepartmentAmount.X1 = 0F;
            this.lineFooterHorDepartmentAmount.X2 = 10.62992F;
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
            this.txtStaffAmount,
            this.lblStaffAmount,
            this.lblStaffAmountSpace,
            this.lineFooterVerStaff,
            this.lineFooterHorStaffAmount,
            this.lineFooterVerStaffAmount});
            this.groupFooter3.Height = 0.3075787F;
            this.groupFooter3.Name = "groupFooter3";
            // 
            // txtStaffAmount
            // 
            this.txtStaffAmount.DataField = "TotalAmount";
            this.txtStaffAmount.Height = 0.3F;
            this.txtStaffAmount.Left = 5.7F;
            this.txtStaffAmount.Name = "txtStaffAmount";
            this.txtStaffAmount.OutputFormat = resources.GetString("txtStaffAmount.OutputFormat");
            this.txtStaffAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtStaffAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtStaffAmount.SummaryGroup = "groupHeader3";
            this.txtStaffAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtStaffAmount.Text = null;
            this.txtStaffAmount.Top = 0F;
            this.txtStaffAmount.Width = 1F;
            // 
            // lblStaffAmount
            // 
            this.lblStaffAmount.Height = 0.3F;
            this.lblStaffAmount.HyperLink = null;
            this.lblStaffAmount.Left = 0F;
            this.lblStaffAmount.Name = "lblStaffAmount";
            this.lblStaffAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblStaffAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblStaffAmount.Text = "担当者計";
            this.lblStaffAmount.Top = 0F;
            this.lblStaffAmount.Width = 5.7F;
            // 
            // lblStaffAmountSpace
            // 
            this.lblStaffAmountSpace.Height = 0.3F;
            this.lblStaffAmountSpace.HyperLink = null;
            this.lblStaffAmountSpace.Left = 6.7F;
            this.lblStaffAmountSpace.Name = "lblStaffAmountSpace";
            this.lblStaffAmountSpace.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.lblStaffAmountSpace.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblStaffAmountSpace.Text = "";
            this.lblStaffAmountSpace.Top = 0F;
            this.lblStaffAmountSpace.Width = 3.95F;
            // 
            // lineFooterVerStaff
            // 
            this.lineFooterVerStaff.Height = 0.3F;
            this.lineFooterVerStaff.Left = 5.7F;
            this.lineFooterVerStaff.LineWeight = 1F;
            this.lineFooterVerStaff.Name = "lineFooterVerStaff";
            this.lineFooterVerStaff.Top = 0F;
            this.lineFooterVerStaff.Width = 0F;
            this.lineFooterVerStaff.X1 = 5.7F;
            this.lineFooterVerStaff.X2 = 5.7F;
            this.lineFooterVerStaff.Y1 = 0F;
            this.lineFooterVerStaff.Y2 = 0.3F;
            // 
            // lineFooterHorStaffAmount
            // 
            this.lineFooterHorStaffAmount.Height = 0F;
            this.lineFooterHorStaffAmount.Left = 0F;
            this.lineFooterHorStaffAmount.LineWeight = 1F;
            this.lineFooterHorStaffAmount.Name = "lineFooterHorStaffAmount";
            this.lineFooterHorStaffAmount.Top = 0.3F;
            this.lineFooterHorStaffAmount.Width = 10.65F;
            this.lineFooterHorStaffAmount.X1 = 0F;
            this.lineFooterHorStaffAmount.X2 = 10.65F;
            this.lineFooterHorStaffAmount.Y1 = 0.3F;
            this.lineFooterHorStaffAmount.Y2 = 0.3F;
            // 
            // lineFooterVerStaffAmount
            // 
            this.lineFooterVerStaffAmount.Height = 0.3F;
            this.lineFooterVerStaffAmount.Left = 6.7F;
            this.lineFooterVerStaffAmount.LineWeight = 1F;
            this.lineFooterVerStaffAmount.Name = "lineFooterVerStaffAmount";
            this.lineFooterVerStaffAmount.Top = 0F;
            this.lineFooterVerStaffAmount.Width = 0F;
            this.lineFooterVerStaffAmount.X1 = 6.7F;
            this.lineFooterVerStaffAmount.X2 = 6.7F;
            this.lineFooterVerStaffAmount.Y1 = 0F;
            this.lineFooterVerStaffAmount.Y2 = 0.3F;
            // 
            // txtCustomerAmount
            // 
            this.txtCustomerAmount.DataField = "TotalAmount";
            this.txtCustomerAmount.Height = 0.3F;
            this.txtCustomerAmount.Left = 5.7F;
            this.txtCustomerAmount.Name = "txtCustomerAmount";
            this.txtCustomerAmount.OutputFormat = resources.GetString("txtCustomerAmount.OutputFormat");
            this.txtCustomerAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtCustomerAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtCustomerAmount.SummaryGroup = "groupHeader4";
            this.txtCustomerAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtCustomerAmount.Text = null;
            this.txtCustomerAmount.Top = 0F;
            this.txtCustomerAmount.Width = 1F;
            // 
            // groupHeader4
            // 
            this.groupHeader4.Height = 0F;
            this.groupHeader4.Name = "groupHeader4";
            // 
            // groupFooter4
            // 
            this.groupFooter4.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCustomerAmount,
            this.lblCustomerAmount,
            this.lblCustomerAmountSpace,
            this.lineFooterVerCustomer,
            this.lineFooterVerCustomerAmount,
            this.lineFooterHorCustomerAmount});
            this.groupFooter4.Height = 0.3075787F;
            this.groupFooter4.Name = "groupFooter4";
            // 
            // lblCustomerAmount
            // 
            this.lblCustomerAmount.Height = 0.3F;
            this.lblCustomerAmount.HyperLink = null;
            this.lblCustomerAmount.Left = 0F;
            this.lblCustomerAmount.Name = "lblCustomerAmount";
            this.lblCustomerAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblCustomerAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblCustomerAmount.Text = "得意先計";
            this.lblCustomerAmount.Top = 0F;
            this.lblCustomerAmount.Width = 5.7F;
            // 
            // lblCustomerAmountSpace
            // 
            this.lblCustomerAmountSpace.Height = 0.3F;
            this.lblCustomerAmountSpace.HyperLink = null;
            this.lblCustomerAmountSpace.Left = 6.7F;
            this.lblCustomerAmountSpace.Name = "lblCustomerAmountSpace";
            this.lblCustomerAmountSpace.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblCustomerAmountSpace.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblCustomerAmountSpace.Text = "";
            this.lblCustomerAmountSpace.Top = 0F;
            this.lblCustomerAmountSpace.Width = 3.95F;
            // 
            // lineFooterVerCustomer
            // 
            this.lineFooterVerCustomer.Height = 0.3F;
            this.lineFooterVerCustomer.Left = 5.7F;
            this.lineFooterVerCustomer.LineWeight = 1F;
            this.lineFooterVerCustomer.Name = "lineFooterVerCustomer";
            this.lineFooterVerCustomer.Top = 0F;
            this.lineFooterVerCustomer.Width = 0F;
            this.lineFooterVerCustomer.X1 = 5.7F;
            this.lineFooterVerCustomer.X2 = 5.7F;
            this.lineFooterVerCustomer.Y1 = 0F;
            this.lineFooterVerCustomer.Y2 = 0.3F;
            // 
            // lineFooterVerCustomerAmount
            // 
            this.lineFooterVerCustomerAmount.Height = 0.3F;
            this.lineFooterVerCustomerAmount.Left = 6.7F;
            this.lineFooterVerCustomerAmount.LineWeight = 1F;
            this.lineFooterVerCustomerAmount.Name = "lineFooterVerCustomerAmount";
            this.lineFooterVerCustomerAmount.Top = 0F;
            this.lineFooterVerCustomerAmount.Width = 0F;
            this.lineFooterVerCustomerAmount.X1 = 6.7F;
            this.lineFooterVerCustomerAmount.X2 = 6.7F;
            this.lineFooterVerCustomerAmount.Y1 = 0F;
            this.lineFooterVerCustomerAmount.Y2 = 0.3F;
            // 
            // lineFooterHorCustomerAmount
            // 
            this.lineFooterHorCustomerAmount.Height = 0F;
            this.lineFooterHorCustomerAmount.Left = 0.02F;
            this.lineFooterHorCustomerAmount.LineWeight = 1F;
            this.lineFooterHorCustomerAmount.Name = "lineFooterHorCustomerAmount";
            this.lineFooterHorCustomerAmount.Top = 0.3F;
            this.lineFooterHorCustomerAmount.Width = 10.62992F;
            this.lineFooterHorCustomerAmount.X1 = 0.02F;
            this.lineFooterHorCustomerAmount.X2 = 10.64992F;
            this.lineFooterHorCustomerAmount.Y1 = 0.3F;
            this.lineFooterHorCustomerAmount.Y2 = 0.3F;
            // 
            // groupHeader5
            // 
            this.groupHeader5.Height = 0F;
            this.groupHeader5.Name = "groupHeader5";
            // 
            // groupFooter5
            // 
            this.groupFooter5.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtDueAtAmount,
            this.lblDueAtAmount,
            this.lblDueAtAmountSpace,
            this.lineFooterHorDueAtAmount,
            this.lineFooterVerDueAt,
            this.lineFooterVerDueAtAmount});
            this.groupFooter5.Height = 0.3075787F;
            this.groupFooter5.Name = "groupFooter5";
            // 
            // txtDueAtAmount
            // 
            this.txtDueAtAmount.DataField = "TotalAmount";
            this.txtDueAtAmount.Height = 0.3F;
            this.txtDueAtAmount.Left = 5.7F;
            this.txtDueAtAmount.Name = "txtDueAtAmount";
            this.txtDueAtAmount.OutputFormat = resources.GetString("txtDueAtAmount.OutputFormat");
            this.txtDueAtAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtDueAtAmount.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtDueAtAmount.SummaryGroup = "groupHeader5";
            this.txtDueAtAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtDueAtAmount.Text = null;
            this.txtDueAtAmount.Top = 0F;
            this.txtDueAtAmount.Width = 1F;
            // 
            // lblDueAtAmount
            // 
            this.lblDueAtAmount.Height = 0.3F;
            this.lblDueAtAmount.HyperLink = null;
            this.lblDueAtAmount.Left = 0F;
            this.lblDueAtAmount.Name = "lblDueAtAmount";
            this.lblDueAtAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblDueAtAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.lblDueAtAmount.Text = "予定日計";
            this.lblDueAtAmount.Top = 0F;
            this.lblDueAtAmount.Width = 5.7F;
            // 
            // lblDueAtAmountSpace
            // 
            this.lblDueAtAmountSpace.Height = 0.3F;
            this.lblDueAtAmountSpace.HyperLink = null;
            this.lblDueAtAmountSpace.Left = 6.7F;
            this.lblDueAtAmountSpace.Name = "lblDueAtAmountSpace";
            this.lblDueAtAmountSpace.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.lblDueAtAmountSpace.Style = "background-color: WhiteSmoke; font-size: 6pt; vertical-align: middle; ddo-char-se" +
    "t: 1";
            this.lblDueAtAmountSpace.Text = "";
            this.lblDueAtAmountSpace.Top = 0F;
            this.lblDueAtAmountSpace.Width = 3.95F;
            // 
            // lineFooterHorDueAtAmount
            // 
            this.lineFooterHorDueAtAmount.Height = 0F;
            this.lineFooterHorDueAtAmount.Left = 0.024F;
            this.lineFooterHorDueAtAmount.LineWeight = 1F;
            this.lineFooterHorDueAtAmount.Name = "lineFooterHorDueAtAmount";
            this.lineFooterHorDueAtAmount.Top = 0.3F;
            this.lineFooterHorDueAtAmount.Width = 10.626F;
            this.lineFooterHorDueAtAmount.X1 = 0.024F;
            this.lineFooterHorDueAtAmount.X2 = 10.65F;
            this.lineFooterHorDueAtAmount.Y1 = 0.3F;
            this.lineFooterHorDueAtAmount.Y2 = 0.3F;
            // 
            // lineFooterVerDueAt
            // 
            this.lineFooterVerDueAt.Height = 0.299F;
            this.lineFooterVerDueAt.Left = 5.7F;
            this.lineFooterVerDueAt.LineWeight = 1F;
            this.lineFooterVerDueAt.Name = "lineFooterVerDueAt";
            this.lineFooterVerDueAt.Top = 0.001F;
            this.lineFooterVerDueAt.Width = 0F;
            this.lineFooterVerDueAt.X1 = 5.7F;
            this.lineFooterVerDueAt.X2 = 5.7F;
            this.lineFooterVerDueAt.Y1 = 0.001F;
            this.lineFooterVerDueAt.Y2 = 0.3F;
            // 
            // lineFooterVerDueAtAmount
            // 
            this.lineFooterVerDueAtAmount.Height = 0.3F;
            this.lineFooterVerDueAtAmount.Left = 6.7F;
            this.lineFooterVerDueAtAmount.LineWeight = 1F;
            this.lineFooterVerDueAtAmount.Name = "lineFooterVerDueAtAmount";
            this.lineFooterVerDueAtAmount.Top = 0F;
            this.lineFooterVerDueAtAmount.Width = 0F;
            this.lineFooterVerDueAtAmount.X1 = 6.7F;
            this.lineFooterVerDueAtAmount.X2 = 6.7F;
            this.lineFooterVerDueAtAmount.Y1 = 0F;
            this.lineFooterVerDueAtAmount.Y2 = 0.3F;
            // 
            // ArrearagesListReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.65F;
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
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOriginalDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblArrearagesDayCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOriginalDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArrearagesDayCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmountTotalSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentAmountSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffAmountSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerAmountSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAtAmountSpace)).EndInit();
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
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTel;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRemainingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblOriginalDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblArrearagesDayCount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerTel;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtID;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTel;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtOriginalDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader2;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter2;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffAmount;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader3;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter3;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentAmount;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader4;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter4;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerNote;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtArrearagesDayCount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerStaffAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingId;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader5;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter5;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAtAmountSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAmountTotalSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentAmountSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffAmountSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerAmountSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorDueAtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerOrginalDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerTel;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerOrginalDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRemainingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerDepartmentAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerStaff;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerCustomerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerDueAtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorDepartmentAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorStaffAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorCustomerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorInvoiceCode;
    }
}
