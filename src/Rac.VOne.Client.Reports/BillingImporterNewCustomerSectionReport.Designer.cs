namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingImporterNewCustomerSectionReport の概要の説明です。
    /// </summary>
    partial class BillingImporterNewCustomerSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BillingImporterNewCustomerSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCustomerNameKana = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPrice = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceContract = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyTax = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillDepCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBilAmtTaxAmt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblClosingSaleAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerInvoice = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBilledAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.linHeaderVerCollectCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCustomerKana = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPrice = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtContractNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTaxClassId = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTaxAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSaleAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtClosingAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUnitSymbol = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtQuantity = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerInvoice = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtBillingCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCollectCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUnitPrice = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerClosingAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCollectCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblTotalbar = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineGroupFooter = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceContract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillDepCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilAmtTaxAmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingSaleAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContractNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxClassId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnitSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnitPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCustomerNameKana,
            this.lblCurrencyCode,
            this.lblCustomerCode,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblPrice,
            this.lblInvoiceContract,
            this.lblCompanyTax,
            this.lblBillDepCode,
            this.lblBilAmtTaxAmt,
            this.lblClosingSaleAt,
            this.lblDueAt,
            this.lblStaffCode,
            this.lineHeaderVerInvoice,
            this.lblCode,
            this.lblNote,
            this.lineHeaderHorUpper,
            this.lineHeaderVerCompanyCode,
            this.lineHeaderVerBilledAt,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderVerBilledAmount,
            this.lineHeaderVerDueAt,
            this.lineHeaderVerClosingAt,
            this.lineHeaderVerStaffCode,
            this.linHeaderVerCollectCategoryCode,
            this.lineHeaderHorLower,
            this.line1});
            this.pageHeader.Height = 1.007645F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCustomerNameKana
            // 
            this.lblCustomerNameKana.Height = 0.4F;
            this.lblCustomerNameKana.HyperLink = null;
            this.lblCustomerNameKana.Left = 0.9F;
            this.lblCustomerNameKana.Name = "lblCustomerNameKana";
            this.lblCustomerNameKana.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblCustomerNameKana.Text = "得意先名称 \r\n\r\n得意先カナ";
            this.lblCustomerNameKana.Top = 0.6F;
            this.lblCustomerNameKana.Width = 1.2F;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Height = 0.2F;
            this.lblCurrencyCode.HyperLink = null;
            this.lblCurrencyCode.Left = 0F;
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.Top = 0.8F;
            this.lblCurrencyCode.Width = 0.9F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.2F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 0F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.6F;
            this.lblCustomerCode.Width = 0.9F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCode.Text = "会社コード　：";
            this.lblCompanyCode.Top = 0.02440945F;
            this.lblCompanyCode.Width = 0.7874014F;
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.7874016F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyCodeName.Text = "label2";
            this.lblCompanyCodeName.Top = 0.02440945F;
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
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 1.907349E-06F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-family: ＭＳ 明朝; font-size: 14.25pt; text-align: center; text-decoration: unde" +
    "rline; ddo-char-set: 128";
            this.lblTitle.Text = "請求フリーインポーター　新規得意先一覧";
            this.lblTitle.Top = 0.2704724F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblPrice
            // 
            this.lblPrice.Height = 0.2F;
            this.lblPrice.HyperLink = null;
            this.lblPrice.Left = 9.200001F;
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblPrice.Text = "金額";
            this.lblPrice.Top = 0.8F;
            this.lblPrice.Width = 1.43F;
            // 
            // lblInvoiceContract
            // 
            this.lblInvoiceContract.Height = 0.4F;
            this.lblInvoiceContract.HyperLink = null;
            this.lblInvoiceContract.Left = 2.1F;
            this.lblInvoiceContract.Name = "lblInvoiceContract";
            this.lblInvoiceContract.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblInvoiceContract.Text = "請求書番号    \r\n\r\n契約番号";
            this.lblInvoiceContract.Top = 0.6F;
            this.lblInvoiceContract.Width = 0.9F;
            // 
            // lblCompanyTax
            // 
            this.lblCompanyTax.Height = 0.4F;
            this.lblCompanyTax.HyperLink = null;
            this.lblCompanyTax.Left = 3F;
            this.lblCompanyTax.Name = "lblCompanyTax";
            this.lblCompanyTax.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblCompanyTax.Text = "会社コード\r\n\r\n 税区";
            this.lblCompanyTax.Top = 0.6F;
            this.lblCompanyTax.Width = 0.8F;
            // 
            // lblBillDepCode
            // 
            this.lblBillDepCode.Height = 0.4F;
            this.lblBillDepCode.HyperLink = null;
            this.lblBillDepCode.Left = 3.8F;
            this.lblBillDepCode.Name = "lblBillDepCode";
            this.lblBillDepCode.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblBillDepCode.Text = "請求日 \r\n\r\n請求部門";
            this.lblBillDepCode.Top = 0.6F;
            this.lblBillDepCode.Width = 0.8F;
            // 
            // lblBilAmtTaxAmt
            // 
            this.lblBilAmtTaxAmt.Height = 0.4F;
            this.lblBilAmtTaxAmt.HyperLink = null;
            this.lblBilAmtTaxAmt.Left = 4.6F;
            this.lblBilAmtTaxAmt.Name = "lblBilAmtTaxAmt";
            this.lblBilAmtTaxAmt.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblBilAmtTaxAmt.Text = "請求金額\r\n\r\n 消費税";
            this.lblBilAmtTaxAmt.Top = 0.6F;
            this.lblBilAmtTaxAmt.Width = 1.2F;
            // 
            // lblClosingSaleAt
            // 
            this.lblClosingSaleAt.Height = 0.4F;
            this.lblClosingSaleAt.HyperLink = null;
            this.lblClosingSaleAt.Left = 5.8F;
            this.lblClosingSaleAt.Name = "lblClosingSaleAt";
            this.lblClosingSaleAt.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblClosingSaleAt.Text = "入金予定日\r\n\r\n 売上日";
            this.lblClosingSaleAt.Top = 0.6F;
            this.lblClosingSaleAt.Width = 0.7F;
            // 
            // lblDueAt
            // 
            this.lblDueAt.Height = 0.4F;
            this.lblDueAt.HyperLink = null;
            this.lblDueAt.Left = 6.5F;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblDueAt.Text = "請求締日 \r\n\r\n数量";
            this.lblDueAt.Top = 0.6F;
            this.lblDueAt.Width = 0.8F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.4F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 7.3F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblStaffCode.Text = "担当者コード \r\n\r\n 単位";
            this.lblStaffCode.Top = 0.6F;
            this.lblStaffCode.Width = 0.9F;
            // 
            // lineHeaderVerInvoice
            // 
            this.lineHeaderVerInvoice.Height = 0.4F;
            this.lineHeaderVerInvoice.Left = 0.9F;
            this.lineHeaderVerInvoice.LineWeight = 1F;
            this.lineHeaderVerInvoice.Name = "lineHeaderVerInvoice";
            this.lineHeaderVerInvoice.Top = 0.6F;
            this.lineHeaderVerInvoice.Width = 0F;
            this.lineHeaderVerInvoice.X1 = 0.9F;
            this.lineHeaderVerInvoice.X2 = 0.9F;
            this.lineHeaderVerInvoice.Y1 = 0.6F;
            this.lineHeaderVerInvoice.Y2 = 1F;
            // 
            // lblCode
            // 
            this.lblCode.Height = 0.4F;
            this.lblCode.HyperLink = null;
            this.lblCode.Left = 8.200001F;
            this.lblCode.Name = "lblCode";
            this.lblCode.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblCode.Text = "請区      回区 \r\n \r\n単価";
            this.lblCode.Top = 0.6F;
            this.lblCode.Width = 1F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.2F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 9.200001F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblNote.Text = "備考 ";
            this.lblNote.Top = 0.6F;
            this.lblNote.Width = 1.43F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.6F;
            this.lineHeaderHorUpper.Width = 10.62F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.62F;
            this.lineHeaderHorUpper.Y1 = 0.6F;
            this.lineHeaderHorUpper.Y2 = 0.6F;
            // 
            // lineHeaderVerCompanyCode
            // 
            this.lineHeaderVerCompanyCode.Height = 0.4F;
            this.lineHeaderVerCompanyCode.Left = 2.1F;
            this.lineHeaderVerCompanyCode.LineWeight = 1F;
            this.lineHeaderVerCompanyCode.Name = "lineHeaderVerCompanyCode";
            this.lineHeaderVerCompanyCode.Top = 0.6F;
            this.lineHeaderVerCompanyCode.Width = 6.318092E-05F;
            this.lineHeaderVerCompanyCode.X1 = 2.1F;
            this.lineHeaderVerCompanyCode.X2 = 2.100063F;
            this.lineHeaderVerCompanyCode.Y1 = 0.6F;
            this.lineHeaderVerCompanyCode.Y2 = 1F;
            // 
            // lineHeaderVerBilledAt
            // 
            this.lineHeaderVerBilledAt.Height = 0.4F;
            this.lineHeaderVerBilledAt.Left = 3.8F;
            this.lineHeaderVerBilledAt.LineWeight = 1F;
            this.lineHeaderVerBilledAt.Name = "lineHeaderVerBilledAt";
            this.lineHeaderVerBilledAt.Top = 0.6F;
            this.lineHeaderVerBilledAt.Width = 0F;
            this.lineHeaderVerBilledAt.X1 = 3.8F;
            this.lineHeaderVerBilledAt.X2 = 3.8F;
            this.lineHeaderVerBilledAt.Y1 = 0.6F;
            this.lineHeaderVerBilledAt.Y2 = 1F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.4F;
            this.lineHeaderVerCustomerCode.Left = 4.6F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.6F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 4.6F;
            this.lineHeaderVerCustomerCode.X2 = 4.6F;
            this.lineHeaderVerCustomerCode.Y1 = 0.6F;
            this.lineHeaderVerCustomerCode.Y2 = 1F;
            // 
            // lineHeaderVerBilledAmount
            // 
            this.lineHeaderVerBilledAmount.Height = 0.4F;
            this.lineHeaderVerBilledAmount.Left = 5.8F;
            this.lineHeaderVerBilledAmount.LineWeight = 1F;
            this.lineHeaderVerBilledAmount.Name = "lineHeaderVerBilledAmount";
            this.lineHeaderVerBilledAmount.Top = 0.6F;
            this.lineHeaderVerBilledAmount.Width = 0F;
            this.lineHeaderVerBilledAmount.X1 = 5.8F;
            this.lineHeaderVerBilledAmount.X2 = 5.8F;
            this.lineHeaderVerBilledAmount.Y1 = 0.6F;
            this.lineHeaderVerBilledAmount.Y2 = 1F;
            // 
            // lineHeaderVerDueAt
            // 
            this.lineHeaderVerDueAt.Height = 0.4F;
            this.lineHeaderVerDueAt.Left = 6.5F;
            this.lineHeaderVerDueAt.LineWeight = 1F;
            this.lineHeaderVerDueAt.Name = "lineHeaderVerDueAt";
            this.lineHeaderVerDueAt.Top = 0.6F;
            this.lineHeaderVerDueAt.Width = 0F;
            this.lineHeaderVerDueAt.X1 = 6.5F;
            this.lineHeaderVerDueAt.X2 = 6.5F;
            this.lineHeaderVerDueAt.Y1 = 0.6F;
            this.lineHeaderVerDueAt.Y2 = 1F;
            // 
            // lineHeaderVerClosingAt
            // 
            this.lineHeaderVerClosingAt.Height = 0.4F;
            this.lineHeaderVerClosingAt.Left = 7.3F;
            this.lineHeaderVerClosingAt.LineWeight = 1F;
            this.lineHeaderVerClosingAt.Name = "lineHeaderVerClosingAt";
            this.lineHeaderVerClosingAt.Top = 0.6F;
            this.lineHeaderVerClosingAt.Width = 0F;
            this.lineHeaderVerClosingAt.X1 = 7.3F;
            this.lineHeaderVerClosingAt.X2 = 7.3F;
            this.lineHeaderVerClosingAt.Y1 = 0.6F;
            this.lineHeaderVerClosingAt.Y2 = 1F;
            // 
            // lineHeaderVerStaffCode
            // 
            this.lineHeaderVerStaffCode.Height = 0.4F;
            this.lineHeaderVerStaffCode.Left = 8.200001F;
            this.lineHeaderVerStaffCode.LineWeight = 1F;
            this.lineHeaderVerStaffCode.Name = "lineHeaderVerStaffCode";
            this.lineHeaderVerStaffCode.Top = 0.6F;
            this.lineHeaderVerStaffCode.Width = 6.961823E-05F;
            this.lineHeaderVerStaffCode.X1 = 8.200001F;
            this.lineHeaderVerStaffCode.X2 = 8.20007F;
            this.lineHeaderVerStaffCode.Y1 = 0.6F;
            this.lineHeaderVerStaffCode.Y2 = 1F;
            // 
            // linHeaderVerCollectCategoryCode
            // 
            this.linHeaderVerCollectCategoryCode.Height = 0.4F;
            this.linHeaderVerCollectCategoryCode.Left = 9.200001F;
            this.linHeaderVerCollectCategoryCode.LineWeight = 1F;
            this.linHeaderVerCollectCategoryCode.Name = "linHeaderVerCollectCategoryCode";
            this.linHeaderVerCollectCategoryCode.Top = 0.6F;
            this.linHeaderVerCollectCategoryCode.Width = 6.866455E-05F;
            this.linHeaderVerCollectCategoryCode.X1 = 9.200001F;
            this.linHeaderVerCollectCategoryCode.X2 = 9.200069F;
            this.linHeaderVerCollectCategoryCode.Y1 = 0.6F;
            this.linHeaderVerCollectCategoryCode.Y2 = 1F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1F;
            this.lineHeaderHorLower.Width = 10.62F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.62F;
            this.lineHeaderHorLower.Y1 = 1F;
            this.lineHeaderHorLower.Y2 = 1F;
            // 
            // line1
            // 
            this.line1.Height = 0.4F;
            this.line1.Left = 3F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0.6F;
            this.line1.Width = 6.29425E-05F;
            this.line1.X1 = 3F;
            this.line1.X2 = 3.000063F;
            this.line1.Y1 = 0.6F;
            this.line1.Y2 = 1F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCustomerKana,
            this.txtCustomerName,
            this.txtPrice,
            this.txtInvoiceCode,
            this.txtContractNumber,
            this.txtCompanyCode,
            this.txtTaxClassId,
            this.txtBilledAt,
            this.txtDepartmentCode,
            this.txtBillAmount,
            this.txtTaxAmount,
            this.txtDueAt,
            this.txtSaleAt,
            this.txtClosingAt,
            this.txtStaffCode,
            this.txtUnitSymbol,
            this.txtQuantity,
            this.txtBillingCategoryCode,
            this.txtCollectCategoryCode,
            this.txtUnitPrice,
            this.txtNote1,
            this.txtCurrencyCode,
            this.lineDetailHorLower,
            this.lineDetailVerBilledAt,
            this.lineDetailVerCustomerCode,
            this.lineDetailVerCompanyCode,
            this.lineDetailVerBillAmount,
            this.lineDetailVerDueAt,
            this.lineDetailVerClosingAt,
            this.lineDetailVerStaffCode,
            this.lineDetailVerCollectCategoryCode,
            this.line2,
            this.txtCustomerCode,
            this.lineDetailVerInvoice});
            this.detail.Height = 0.4029199F;
            this.detail.Name = "detail";
            // 
            // txtCustomerKana
            // 
            this.txtCustomerKana.Height = 0.2F;
            this.txtCustomerKana.Left = 0.92F;
            this.txtCustomerKana.MultiLine = false;
            this.txtCustomerKana.Name = "txtCustomerKana";
            this.txtCustomerKana.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerKana.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; vertical-align: middle; ddo" +
    "-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCustomerKana.Text = "txtCustomerKana";
            this.txtCustomerKana.Top = 0.2F;
            this.txtCustomerKana.Width = 1.18F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 0.92F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerName.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: left; vertical-align: middle; ddo" +
    "-char-set: 1; ddo-shrink-to-fit: none";
            this.txtCustomerName.Text = "txtCustomerName";
            this.txtCustomerName.Top = 0F;
            this.txtCustomerName.Width = 1.18F;
            // 
            // txtPrice
            // 
            this.txtPrice.Height = 0.2F;
            this.txtPrice.Left = 9.200001F;
            this.txtPrice.MultiLine = false;
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtPrice.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtPrice.Text = "txtPrice";
            this.txtPrice.Top = 0.2F;
            this.txtPrice.Width = 1.4F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.Height = 0.2F;
            this.txtInvoiceCode.Left = 2.1F;
            this.txtInvoiceCode.MultiLine = false;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtInvoiceCode.Text = "txtInvoice";
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 0.9F;
            // 
            // txtContractNumber
            // 
            this.txtContractNumber.Height = 0.2F;
            this.txtContractNumber.Left = 2.1F;
            this.txtContractNumber.MultiLine = false;
            this.txtContractNumber.Name = "txtContractNumber";
            this.txtContractNumber.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtContractNumber.Text = "txtContract";
            this.txtContractNumber.Top = 0.2F;
            this.txtContractNumber.Width = 0.9F;
            // 
            // txtCompanyCode
            // 
            this.txtCompanyCode.Height = 0.2F;
            this.txtCompanyCode.Left = 3F;
            this.txtCompanyCode.Name = "txtCompanyCode";
            this.txtCompanyCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCompanyCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtCompanyCode.Text = "txtCompanyCode";
            this.txtCompanyCode.Top = 0F;
            this.txtCompanyCode.Width = 0.8F;
            // 
            // txtTaxClassId
            // 
            this.txtTaxClassId.Height = 0.2F;
            this.txtTaxClassId.Left = 3F;
            this.txtTaxClassId.Name = "txtTaxClassId";
            this.txtTaxClassId.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtTaxClassId.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtTaxClassId.Text = "txtTaxClassId";
            this.txtTaxClassId.Top = 0.2F;
            this.txtTaxClassId.Width = 0.8070866F;
            // 
            // txtBilledAt
            // 
            this.txtBilledAt.Height = 0.2F;
            this.txtBilledAt.Left = 3.8F;
            this.txtBilledAt.Name = "txtBilledAt";
            this.txtBilledAt.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtBilledAt.Text = "txtBilledAt";
            this.txtBilledAt.Top = 0F;
            this.txtBilledAt.Width = 0.8000002F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.Height = 0.2F;
            this.txtDepartmentCode.Left = 3.8F;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtDepartmentCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtDepartmentCode.Text = "txtDepartmentCode";
            this.txtDepartmentCode.Top = 0.2F;
            this.txtDepartmentCode.Width = 0.8F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 0F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtCustomerCode.Text = "txtCustomerCode";
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 0.9F;
            // 
            // txtBillAmount
            // 
            this.txtBillAmount.Height = 0.2F;
            this.txtBillAmount.Left = 4.6F;
            this.txtBillAmount.MultiLine = false;
            this.txtBillAmount.Name = "txtBillAmount";
            this.txtBillAmount.OutputFormat = resources.GetString("txtBillAmount.OutputFormat");
            this.txtBillAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillAmount.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtBillAmount.Text = "txtBillAmt";
            this.txtBillAmount.Top = 0F;
            this.txtBillAmount.Width = 1.18F;
            // 
            // txtTaxAmount
            // 
            this.txtTaxAmount.Height = 0.2F;
            this.txtTaxAmount.Left = 4.6F;
            this.txtTaxAmount.MultiLine = false;
            this.txtTaxAmount.Name = "txtTaxAmount";
            this.txtTaxAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtTaxAmount.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtTaxAmount.Text = "txtTaxAmount";
            this.txtTaxAmount.Top = 0.2F;
            this.txtTaxAmount.Width = 1.18F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.Height = 0.2F;
            this.txtDueAt.Left = 5.8F;
            this.txtDueAt.MultiLine = false;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtDueAt.Text = "txtDueAt";
            this.txtDueAt.Top = 0F;
            this.txtDueAt.Width = 0.7000003F;
            // 
            // txtSaleAt
            // 
            this.txtSaleAt.Height = 0.2F;
            this.txtSaleAt.Left = 5.8F;
            this.txtSaleAt.MultiLine = false;
            this.txtSaleAt.Name = "txtSaleAt";
            this.txtSaleAt.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtSaleAt.Text = "txtSaleAt";
            this.txtSaleAt.Top = 0.2F;
            this.txtSaleAt.Width = 0.7000003F;
            // 
            // txtClosingAt
            // 
            this.txtClosingAt.Height = 0.2F;
            this.txtClosingAt.Left = 6.5F;
            this.txtClosingAt.Name = "txtClosingAt";
            this.txtClosingAt.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtClosingAt.ShrinkToFit = true;
            this.txtClosingAt.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1; ddo-shrink-to-fit: true";
            this.txtClosingAt.Text = "txtClosingAt";
            this.txtClosingAt.Top = 0F;
            this.txtClosingAt.Width = 0.8F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.Height = 0.2F;
            this.txtStaffCode.Left = 7.3F;
            this.txtStaffCode.MultiLine = false;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtStaffCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtStaffCode.Text = "txtStaffCode";
            this.txtStaffCode.Top = 0F;
            this.txtStaffCode.Width = 0.9000006F;
            // 
            // txtUnitSymbol
            // 
            this.txtUnitSymbol.Height = 0.2F;
            this.txtUnitSymbol.Left = 7.3F;
            this.txtUnitSymbol.MultiLine = false;
            this.txtUnitSymbol.Name = "txtUnitSymbol";
            this.txtUnitSymbol.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtUnitSymbol.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtUnitSymbol.Text = null;
            this.txtUnitSymbol.Top = 0.2F;
            this.txtUnitSymbol.Width = 0.9000006F;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Height = 0.2F;
            this.txtQuantity.Left = 6.5F;
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtQuantity.ShrinkToFit = true;
            this.txtQuantity.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1; ddo-" +
    "shrink-to-fit: true";
            this.txtQuantity.Text = null;
            this.txtQuantity.Top = 0.2F;
            this.txtQuantity.Width = 0.8F;
            // 
            // lineDetailVerInvoice
            // 
            this.lineDetailVerInvoice.AnchorBottom = true;
            this.lineDetailVerInvoice.Height = 0.4F;
            this.lineDetailVerInvoice.Left = 0.9F;
            this.lineDetailVerInvoice.LineWeight = 1F;
            this.lineDetailVerInvoice.Name = "lineDetailVerInvoice";
            this.lineDetailVerInvoice.Top = 0F;
            this.lineDetailVerInvoice.Width = 0F;
            this.lineDetailVerInvoice.X1 = 0.9F;
            this.lineDetailVerInvoice.X2 = 0.9F;
            this.lineDetailVerInvoice.Y1 = 0F;
            this.lineDetailVerInvoice.Y2 = 0.4F;
            // 
            // txtBillingCategoryCode
            // 
            this.txtBillingCategoryCode.Height = 0.2F;
            this.txtBillingCategoryCode.Left = 8.200001F;
            this.txtBillingCategoryCode.MultiLine = false;
            this.txtBillingCategoryCode.Name = "txtBillingCategoryCode";
            this.txtBillingCategoryCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtBillingCategoryCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtBillingCategoryCode.Text = "txtBillingCategoryCode";
            this.txtBillingCategoryCode.Top = 0F;
            this.txtBillingCategoryCode.Width = 0.5F;
            // 
            // txtCollectCategoryCode
            // 
            this.txtCollectCategoryCode.Height = 0.2F;
            this.txtCollectCategoryCode.Left = 8.700001F;
            this.txtCollectCategoryCode.MultiLine = false;
            this.txtCollectCategoryCode.Name = "txtCollectCategoryCode";
            this.txtCollectCategoryCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCollectCategoryCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtCollectCategoryCode.Text = "txtCollectCategoryCode";
            this.txtCollectCategoryCode.Top = 0F;
            this.txtCollectCategoryCode.Width = 0.5F;
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Height = 0.2F;
            this.txtUnitPrice.Left = 8.200001F;
            this.txtUnitPrice.MultiLine = false;
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtUnitPrice.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtUnitPrice.Text = null;
            this.txtUnitPrice.Top = 0.2F;
            this.txtUnitPrice.Width = 0.9999995F;
            // 
            // txtNote1
            // 
            this.txtNote1.Height = 0.2F;
            this.txtNote1.Left = 9.23F;
            this.txtNote1.MultiLine = false;
            this.txtNote1.Name = "txtNote1";
            this.txtNote1.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtNote1.Style = "font-family: ＭＳ 明朝; font-size: 6pt; vertical-align: middle; ddo-char-set: 1; ddo-" +
    "shrink-to-fit: none";
            this.txtNote1.Text = "txtNote1";
            this.txtNote1.Top = 0F;
            this.txtNote1.Width = 1.4F;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.Height = 0.2F;
            this.txtCurrencyCode.Left = 0F;
            this.txtCurrencyCode.MultiLine = false;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCurrencyCode.Style = "font-family: ＭＳ 明朝; font-size: 6pt; text-align: center; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtCurrencyCode.Text = "txtCurCode";
            this.txtCurrencyCode.Top = 0.2F;
            this.txtCurrencyCode.Width = 0.9F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.4F;
            this.lineDetailHorLower.Width = 10.62F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.62F;
            this.lineDetailHorLower.Y1 = 0.4F;
            this.lineDetailHorLower.Y2 = 0.4F;
            // 
            // lineDetailVerBilledAt
            // 
            this.lineDetailVerBilledAt.AnchorBottom = true;
            this.lineDetailVerBilledAt.Height = 0.4F;
            this.lineDetailVerBilledAt.Left = 3F;
            this.lineDetailVerBilledAt.LineWeight = 1F;
            this.lineDetailVerBilledAt.Name = "lineDetailVerBilledAt";
            this.lineDetailVerBilledAt.Top = 0F;
            this.lineDetailVerBilledAt.Width = 0F;
            this.lineDetailVerBilledAt.X1 = 3F;
            this.lineDetailVerBilledAt.X2 = 3F;
            this.lineDetailVerBilledAt.Y1 = 0F;
            this.lineDetailVerBilledAt.Y2 = 0.4F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.AnchorBottom = true;
            this.lineDetailVerCustomerCode.Height = 0.4F;
            this.lineDetailVerCustomerCode.Left = 4.6F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 4.6F;
            this.lineDetailVerCustomerCode.X2 = 4.6F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.4F;
            // 
            // lineDetailVerCompanyCode
            // 
            this.lineDetailVerCompanyCode.AnchorBottom = true;
            this.lineDetailVerCompanyCode.Height = 0.4F;
            this.lineDetailVerCompanyCode.Left = 2.1F;
            this.lineDetailVerCompanyCode.LineWeight = 1F;
            this.lineDetailVerCompanyCode.Name = "lineDetailVerCompanyCode";
            this.lineDetailVerCompanyCode.Top = 0F;
            this.lineDetailVerCompanyCode.Width = 0F;
            this.lineDetailVerCompanyCode.X1 = 2.1F;
            this.lineDetailVerCompanyCode.X2 = 2.1F;
            this.lineDetailVerCompanyCode.Y1 = 0F;
            this.lineDetailVerCompanyCode.Y2 = 0.4F;
            // 
            // lineDetailVerBillAmount
            // 
            this.lineDetailVerBillAmount.AnchorBottom = true;
            this.lineDetailVerBillAmount.Height = 0.4F;
            this.lineDetailVerBillAmount.Left = 5.8F;
            this.lineDetailVerBillAmount.LineWeight = 1F;
            this.lineDetailVerBillAmount.Name = "lineDetailVerBillAmount";
            this.lineDetailVerBillAmount.Top = 0F;
            this.lineDetailVerBillAmount.Width = 0F;
            this.lineDetailVerBillAmount.X1 = 5.8F;
            this.lineDetailVerBillAmount.X2 = 5.8F;
            this.lineDetailVerBillAmount.Y1 = 0F;
            this.lineDetailVerBillAmount.Y2 = 0.4F;
            // 
            // lineDetailVerDueAt
            // 
            this.lineDetailVerDueAt.AnchorBottom = true;
            this.lineDetailVerDueAt.Height = 0.4F;
            this.lineDetailVerDueAt.Left = 6.5F;
            this.lineDetailVerDueAt.LineWeight = 1F;
            this.lineDetailVerDueAt.Name = "lineDetailVerDueAt";
            this.lineDetailVerDueAt.Top = 0F;
            this.lineDetailVerDueAt.Width = 0F;
            this.lineDetailVerDueAt.X1 = 6.5F;
            this.lineDetailVerDueAt.X2 = 6.5F;
            this.lineDetailVerDueAt.Y1 = 0F;
            this.lineDetailVerDueAt.Y2 = 0.4F;
            // 
            // lineDetailVerClosingAt
            // 
            this.lineDetailVerClosingAt.AnchorBottom = true;
            this.lineDetailVerClosingAt.Height = 0.4F;
            this.lineDetailVerClosingAt.Left = 7.3F;
            this.lineDetailVerClosingAt.LineWeight = 1F;
            this.lineDetailVerClosingAt.Name = "lineDetailVerClosingAt";
            this.lineDetailVerClosingAt.Top = 0F;
            this.lineDetailVerClosingAt.Width = 0F;
            this.lineDetailVerClosingAt.X1 = 7.3F;
            this.lineDetailVerClosingAt.X2 = 7.3F;
            this.lineDetailVerClosingAt.Y1 = 0F;
            this.lineDetailVerClosingAt.Y2 = 0.4F;
            // 
            // lineDetailVerStaffCode
            // 
            this.lineDetailVerStaffCode.AnchorBottom = true;
            this.lineDetailVerStaffCode.Height = 0.4F;
            this.lineDetailVerStaffCode.Left = 8.200001F;
            this.lineDetailVerStaffCode.LineWeight = 1F;
            this.lineDetailVerStaffCode.Name = "lineDetailVerStaffCode";
            this.lineDetailVerStaffCode.Top = 0F;
            this.lineDetailVerStaffCode.Width = 0F;
            this.lineDetailVerStaffCode.X1 = 8.200001F;
            this.lineDetailVerStaffCode.X2 = 8.200001F;
            this.lineDetailVerStaffCode.Y1 = 0F;
            this.lineDetailVerStaffCode.Y2 = 0.4F;
            // 
            // lineDetailVerCollectCategoryCode
            // 
            this.lineDetailVerCollectCategoryCode.AnchorBottom = true;
            this.lineDetailVerCollectCategoryCode.Height = 0.4F;
            this.lineDetailVerCollectCategoryCode.Left = 9.200001F;
            this.lineDetailVerCollectCategoryCode.LineWeight = 1F;
            this.lineDetailVerCollectCategoryCode.Name = "lineDetailVerCollectCategoryCode";
            this.lineDetailVerCollectCategoryCode.Top = 0F;
            this.lineDetailVerCollectCategoryCode.Width = 0F;
            this.lineDetailVerCollectCategoryCode.X1 = 9.200001F;
            this.lineDetailVerCollectCategoryCode.X2 = 9.200001F;
            this.lineDetailVerCollectCategoryCode.Y1 = 0F;
            this.lineDetailVerCollectCategoryCode.Y2 = 0.4F;
            // 
            // line2
            // 
            this.line2.AnchorBottom = true;
            this.line2.Height = 0.4F;
            this.line2.Left = 3.8F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 0F;
            this.line2.Width = 0F;
            this.line2.X1 = 3.8F;
            this.line2.X2 = 3.8F;
            this.line2.Y1 = 0F;
            this.line2.Y2 = 0.4F;
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
            this.reportInfo1.Style = "color: Gray; font-family: ＭＳ 明朝; font-size: 6.75pt; text-align: center; vertical-" +
    "align: middle; ddo-char-set: 128";
            this.reportInfo1.Top = 0.05748032F;
            this.reportInfo1.Width = 10.62992F;
            // 
            // groupHeader1
            // 
            this.groupHeader1.Height = 0F;
            this.groupHeader1.Name = "groupHeader1";
            // 
            // groupFooter1
            // 
            this.groupFooter1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblTotalbar,
            this.lblTotalAmount,
            this.lineGroupFooter});
            this.groupFooter1.Height = 0.2046095F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // lblTotalbar
            // 
            this.lblTotalbar.Height = 0.2F;
            this.lblTotalbar.HyperLink = null;
            this.lblTotalbar.Left = -1.862645E-09F;
            this.lblTotalbar.Name = "lblTotalbar";
            this.lblTotalbar.Padding = new GrapeCity.ActiveReports.PaddingEx(10, 0, 0, 0);
            this.lblTotalbar.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: l" +
    "eft; vertical-align: middle; ddo-char-set: 1";
            this.lblTotalbar.Text = "件計";
            this.lblTotalbar.Top = 0F;
            this.lblTotalbar.Width = 9.200001F;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Height = 0.2F;
            this.lblTotalAmount.HyperLink = null;
            this.lblTotalAmount.Left = 9.200001F;
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 4, 0);
            this.lblTotalAmount.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: r" +
    "ight; vertical-align: middle; ddo-char-set: 1";
            this.lblTotalAmount.Text = "件";
            this.lblTotalAmount.Top = 0F;
            this.lblTotalAmount.Width = 1.43F;
            // 
            // lineGroupFooter
            // 
            this.lineGroupFooter.Height = 0F;
            this.lineGroupFooter.Left = 9.313208E-10F;
            this.lineGroupFooter.LineWeight = 1F;
            this.lineGroupFooter.Name = "lineGroupFooter";
            this.lineGroupFooter.Top = 0.2F;
            this.lineGroupFooter.Width = 10.62F;
            this.lineGroupFooter.X1 = 9.313208E-10F;
            this.lineGroupFooter.X2 = 10.62F;
            this.lineGroupFooter.Y1 = 0.2F;
            this.lineGroupFooter.Y2 = 0.2F;
            // 
            // BillingImporterNewCustomerSectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.DefaultPaperSize = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11.69291F;
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.PageSettings.PaperWidth = 8.267716F;
            this.PrintWidth = 10.63F;
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
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceContract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillDepCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilAmtTaxAmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingSaleAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContractNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxClassId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnitSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnitPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPrice;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceContract;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyTax;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillDepCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBilAmtTaxAmt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblClosingSaleAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerInvoice;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBilledAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line linHeaderVerCollectCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPrice;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtContractNumber;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTaxClassId;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTaxAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSaleAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUnitSymbol;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtQuantity;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerInvoice;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCollectCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUnitPrice;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCollectCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotalbar;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGroupFooter;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerNameKana;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerKana;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
    }
}
