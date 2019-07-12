namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// DepartmentReport の概要の説明です。
    /// </summary>
    partial class MatchingJournalizingCancellationReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MatchingJournalizingCancellationReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPayerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblOutputAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCreateAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblJournalizingName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCreateAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerOutputAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerJournalizingType = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtOutputAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPayerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCreateAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtJJournalizingName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerCreateAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerJournalizingName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrencyCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerOutputAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblJournalizingName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtJJournalizingName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblBilledAt,
            this.lblInvoiceCode,
            this.lblPayerName,
            this.lblRecordedAt,
            this.lblReceiptAmount,
            this.lblOutputAt,
            this.lblCustomerName,
            this.lblCustomerCode,
            this.lblAmount,
            this.lblCreateAt,
            this.lblJournalizingName,
            this.lblCurrencyCode,
            this.lblBillingAmount,
            this.lineHeaderHorUpper,
            this.lineHeaderVerCreateAt,
            this.lineHeaderVerAmount,
            this.lineHeaderVerOutputAt,
            this.lineHeaderVerReceiptAmount,
            this.lineHeaderVerRecordedAt,
            this.lineHeaderVerPayerName,
            this.lineHeaderVerBilledAt,
            this.lineHeaderVerInvoiceCode,
            this.lineHeaderVerJournalizingType,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblCompanyCode,
            this.lblTitle,
            this.lineHeaderVerCustomerName,
            this.lineHeaderVerCurrencyCode,
            this.lineHeaderHorLower,
            this.lineHeaderVerCustomerCode});
            this.pageHeader.Height = 0.9216536F;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.BeforePrint += new System.EventHandler(this.pageHeader_BeforePrint);
            // 
            // lblBilledAt
            // 
            this.lblBilledAt.Height = 0.3283465F;
            this.lblBilledAt.HyperLink = null;
            this.lblBilledAt.Left = 8.305906F;
            this.lblBilledAt.Name = "lblBilledAt";
            this.lblBilledAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblBilledAt.Text = "請求日";
            this.lblBilledAt.Top = 0.5866142F;
            this.lblBilledAt.Width = 0.7244095F;
            // 
            // lblInvoiceCode
            // 
            this.lblInvoiceCode.Height = 0.3283465F;
            this.lblInvoiceCode.HyperLink = null;
            this.lblInvoiceCode.Left = 9.034646F;
            this.lblInvoiceCode.Name = "lblInvoiceCode";
            this.lblInvoiceCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblInvoiceCode.Text = "請求書番号";
            this.lblInvoiceCode.Top = 0.5866142F;
            this.lblInvoiceCode.Width = 0.7244095F;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Height = 0.3283465F;
            this.lblPayerName.HyperLink = null;
            this.lblPayerName.Left = 7.325591F;
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.Top = 0.5866142F;
            this.lblPayerName.Width = 0.976378F;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Height = 0.3283465F;
            this.lblRecordedAt.HyperLink = null;
            this.lblRecordedAt.Left = 6.596064F;
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblRecordedAt.Text = "入金日";
            this.lblRecordedAt.Top = 0.5866142F;
            this.lblRecordedAt.Width = 0.7244095F;
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Height = 0.3283465F;
            this.lblReceiptAmount.HyperLink = null;
            this.lblReceiptAmount.Left = 5.616142F;
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblReceiptAmount.Text = "入金額";
            this.lblReceiptAmount.Top = 0.5866142F;
            this.lblReceiptAmount.Width = 0.976378F;
            // 
            // lblOutputAt
            // 
            this.lblOutputAt.Height = 0.3283465F;
            this.lblOutputAt.HyperLink = null;
            this.lblOutputAt.Left = 4.903937F;
            this.lblOutputAt.Name = "lblOutputAt";
            this.lblOutputAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblOutputAt.Text = "仕訳日";
            this.lblOutputAt.Top = 0.5866142F;
            this.lblOutputAt.Width = 0.7086614F;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Height = 0.3283465F;
            this.lblCustomerName.HyperLink = null;
            this.lblCustomerName.Left = 2.512205F;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.Top = 0.5866142F;
            this.lblCustomerName.Width = 1F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.3283465F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 1.637795F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.5866142F;
            this.lblCustomerCode.Width = 0.8740157F;
            // 
            // lblAmount
            // 
            this.lblAmount.Height = 0.3283465F;
            this.lblAmount.HyperLink = null;
            this.lblAmount.Left = 3.909449F;
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblAmount.Text = "金額";
            this.lblAmount.Top = 0.5866142F;
            this.lblAmount.Width = 1F;
            // 
            // lblCreateAt
            // 
            this.lblCreateAt.Height = 0.3283465F;
            this.lblCreateAt.HyperLink = null;
            this.lblCreateAt.Left = 0F;
            this.lblCreateAt.Name = "lblCreateAt";
            this.lblCreateAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCreateAt.Text = "処理日";
            this.lblCreateAt.Top = 0.5866142F;
            this.lblCreateAt.Width = 0.7165354F;
            // 
            // lblJournalizingName
            // 
            this.lblJournalizingName.Height = 0.3283465F;
            this.lblJournalizingName.HyperLink = null;
            this.lblJournalizingName.Left = 0.7204725F;
            this.lblJournalizingName.Name = "lblJournalizingName";
            this.lblJournalizingName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblJournalizingName.Text = "仕訳区分";
            this.lblJournalizingName.Top = 0.5866142F;
            this.lblJournalizingName.Width = 0.9133858F;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Height = 0.3283465F;
            this.lblCurrencyCode.HyperLink = null;
            this.lblCurrencyCode.Left = 3.514961F;
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCurrencyCode.Text = "通貨";
            this.lblCurrencyCode.Top = 0.5866142F;
            this.lblCurrencyCode.Width = 0.3937008F;
            // 
            // lblBillingAmount
            // 
            this.lblBillingAmount.Height = 0.3283465F;
            this.lblBillingAmount.HyperLink = null;
            this.lblBillingAmount.Left = 9.764567F;
            this.lblBillingAmount.Name = "lblBillingAmount";
            this.lblBillingAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblBillingAmount.Text = "請求額";
            this.lblBillingAmount.Top = 0.5866142F;
            this.lblBillingAmount.Width = 0.8661417F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 6.300211E-05F;
            this.lineHeaderHorUpper.Left = 2.384186E-07F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.5866142F;
            this.lineHeaderHorUpper.Width = 10.62992F;
            this.lineHeaderHorUpper.X1 = 2.384186E-07F;
            this.lineHeaderHorUpper.X2 = 10.62992F;
            this.lineHeaderHorUpper.Y1 = 0.5866142F;
            this.lineHeaderHorUpper.Y2 = 0.5866772F;
            // 
            // lineHeaderVerCreateAt
            // 
            this.lineHeaderVerCreateAt.Height = 0.3346456F;
            this.lineHeaderVerCreateAt.Left = 0.7236223F;
            this.lineHeaderVerCreateAt.LineWeight = 1F;
            this.lineHeaderVerCreateAt.Name = "lineHeaderVerCreateAt";
            this.lineHeaderVerCreateAt.Top = 0.5866142F;
            this.lineHeaderVerCreateAt.Width = 0F;
            this.lineHeaderVerCreateAt.X1 = 0.7236223F;
            this.lineHeaderVerCreateAt.X2 = 0.7236223F;
            this.lineHeaderVerCreateAt.Y1 = 0.5866142F;
            this.lineHeaderVerCreateAt.Y2 = 0.9212598F;
            // 
            // lineHeaderVerAmount
            // 
            this.lineHeaderVerAmount.Height = 0.3346456F;
            this.lineHeaderVerAmount.Left = 4.903937F;
            this.lineHeaderVerAmount.LineWeight = 1F;
            this.lineHeaderVerAmount.Name = "lineHeaderVerAmount";
            this.lineHeaderVerAmount.Top = 0.5866142F;
            this.lineHeaderVerAmount.Width = 0F;
            this.lineHeaderVerAmount.X1 = 4.903937F;
            this.lineHeaderVerAmount.X2 = 4.903937F;
            this.lineHeaderVerAmount.Y1 = 0.5866142F;
            this.lineHeaderVerAmount.Y2 = 0.9212598F;
            // 
            // lineHeaderVerOutputAt
            // 
            this.lineHeaderVerOutputAt.Height = 0.3346456F;
            this.lineHeaderVerOutputAt.Left = 5.616482F;
            this.lineHeaderVerOutputAt.LineWeight = 1F;
            this.lineHeaderVerOutputAt.Name = "lineHeaderVerOutputAt";
            this.lineHeaderVerOutputAt.Top = 0.5866142F;
            this.lineHeaderVerOutputAt.Width = 5.435944E-05F;
            this.lineHeaderVerOutputAt.X1 = 5.616536F;
            this.lineHeaderVerOutputAt.X2 = 5.616482F;
            this.lineHeaderVerOutputAt.Y1 = 0.5866142F;
            this.lineHeaderVerOutputAt.Y2 = 0.9212598F;
            // 
            // lineHeaderVerReceiptAmount
            // 
            this.lineHeaderVerReceiptAmount.Height = 0.3346456F;
            this.lineHeaderVerReceiptAmount.Left = 6.596064F;
            this.lineHeaderVerReceiptAmount.LineWeight = 1F;
            this.lineHeaderVerReceiptAmount.Name = "lineHeaderVerReceiptAmount";
            this.lineHeaderVerReceiptAmount.Top = 0.5866142F;
            this.lineHeaderVerReceiptAmount.Width = 0F;
            this.lineHeaderVerReceiptAmount.X1 = 6.596064F;
            this.lineHeaderVerReceiptAmount.X2 = 6.596064F;
            this.lineHeaderVerReceiptAmount.Y1 = 0.5866142F;
            this.lineHeaderVerReceiptAmount.Y2 = 0.9212598F;
            // 
            // lineHeaderVerRecordedAt
            // 
            this.lineHeaderVerRecordedAt.Height = 0.3346456F;
            this.lineHeaderVerRecordedAt.Left = 7.327559F;
            this.lineHeaderVerRecordedAt.LineWeight = 1F;
            this.lineHeaderVerRecordedAt.Name = "lineHeaderVerRecordedAt";
            this.lineHeaderVerRecordedAt.Top = 0.5866142F;
            this.lineHeaderVerRecordedAt.Width = 0F;
            this.lineHeaderVerRecordedAt.X1 = 7.327559F;
            this.lineHeaderVerRecordedAt.X2 = 7.327559F;
            this.lineHeaderVerRecordedAt.Y1 = 0.5866142F;
            this.lineHeaderVerRecordedAt.Y2 = 0.9212598F;
            // 
            // lineHeaderVerPayerName
            // 
            this.lineHeaderVerPayerName.Height = 0.3346456F;
            this.lineHeaderVerPayerName.Left = 8.298032F;
            this.lineHeaderVerPayerName.LineWeight = 1F;
            this.lineHeaderVerPayerName.Name = "lineHeaderVerPayerName";
            this.lineHeaderVerPayerName.Top = 0.5866142F;
            this.lineHeaderVerPayerName.Width = 0F;
            this.lineHeaderVerPayerName.X1 = 8.298032F;
            this.lineHeaderVerPayerName.X2 = 8.298032F;
            this.lineHeaderVerPayerName.Y1 = 0.5866142F;
            this.lineHeaderVerPayerName.Y2 = 0.9212598F;
            // 
            // lineHeaderVerBilledAt
            // 
            this.lineHeaderVerBilledAt.Height = 0.3346456F;
            this.lineHeaderVerBilledAt.Left = 9.030709F;
            this.lineHeaderVerBilledAt.LineWeight = 1F;
            this.lineHeaderVerBilledAt.Name = "lineHeaderVerBilledAt";
            this.lineHeaderVerBilledAt.Top = 0.5866142F;
            this.lineHeaderVerBilledAt.Width = 0F;
            this.lineHeaderVerBilledAt.X1 = 9.030709F;
            this.lineHeaderVerBilledAt.X2 = 9.030709F;
            this.lineHeaderVerBilledAt.Y1 = 0.5866142F;
            this.lineHeaderVerBilledAt.Y2 = 0.9212598F;
            // 
            // lineHeaderVerInvoiceCode
            // 
            this.lineHeaderVerInvoiceCode.Height = 0.3346456F;
            this.lineHeaderVerInvoiceCode.Left = 9.760237F;
            this.lineHeaderVerInvoiceCode.LineWeight = 1F;
            this.lineHeaderVerInvoiceCode.Name = "lineHeaderVerInvoiceCode";
            this.lineHeaderVerInvoiceCode.Top = 0.5866142F;
            this.lineHeaderVerInvoiceCode.Width = 0F;
            this.lineHeaderVerInvoiceCode.X1 = 9.760237F;
            this.lineHeaderVerInvoiceCode.X2 = 9.760237F;
            this.lineHeaderVerInvoiceCode.Y1 = 0.5866142F;
            this.lineHeaderVerInvoiceCode.Y2 = 0.9212598F;
            // 
            // lineHeaderVerJournalizingType
            // 
            this.lineHeaderVerJournalizingType.Height = 0.3346456F;
            this.lineHeaderVerJournalizingType.Left = 1.637796F;
            this.lineHeaderVerJournalizingType.LineWeight = 1F;
            this.lineHeaderVerJournalizingType.Name = "lineHeaderVerJournalizingType";
            this.lineHeaderVerJournalizingType.Top = 0.5866142F;
            this.lineHeaderVerJournalizingType.Width = 0.0001409054F;
            this.lineHeaderVerJournalizingType.X1 = 1.637796F;
            this.lineHeaderVerJournalizingType.X2 = 1.637937F;
            this.lineHeaderVerJournalizingType.Y1 = 0.5866142F;
            this.lineHeaderVerJournalizingType.Y2 = 0.9212598F;
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.811811F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lblCompanyCodeName.Text = "label4";
            this.lblCompanyCodeName.Top = 0F;
            this.lblCompanyCodeName.Width = 3.657F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 8.809055F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lblDate.Text = "出力日付　：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.6983146F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 9.522441F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle";
            this.ridate.Top = 0F;
            this.ridate.Width = 1.015F;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Height = 0.2F;
            this.lblCompanyCode.HyperLink = null;
            this.lblCompanyCode.Left = 0.02440945F;
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lblCompanyCode.Text = "会社コード　：";
            this.lblCompanyCode.Top = 0F;
            this.lblCompanyCode.Width = 0.7874014F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline";
            this.lblTitle.Text = "消込仕訳取消リスト";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lineHeaderVerCustomerName
            // 
            this.lineHeaderVerCustomerName.Height = 0.3346456F;
            this.lineHeaderVerCustomerName.Left = 3.509448F;
            this.lineHeaderVerCustomerName.LineWeight = 1F;
            this.lineHeaderVerCustomerName.Name = "lineHeaderVerCustomerName";
            this.lineHeaderVerCustomerName.Top = 0.5866142F;
            this.lineHeaderVerCustomerName.Width = 9.536743E-07F;
            this.lineHeaderVerCustomerName.X1 = 3.509449F;
            this.lineHeaderVerCustomerName.X2 = 3.509448F;
            this.lineHeaderVerCustomerName.Y1 = 0.5866142F;
            this.lineHeaderVerCustomerName.Y2 = 0.9212598F;
            // 
            // lineHeaderVerCurrencyCode
            // 
            this.lineHeaderVerCurrencyCode.Height = 0.3346456F;
            this.lineHeaderVerCurrencyCode.Left = 3.901968F;
            this.lineHeaderVerCurrencyCode.LineWeight = 1F;
            this.lineHeaderVerCurrencyCode.Name = "lineHeaderVerCurrencyCode";
            this.lineHeaderVerCurrencyCode.Top = 0.5866142F;
            this.lineHeaderVerCurrencyCode.Width = 9.536743E-07F;
            this.lineHeaderVerCurrencyCode.X1 = 3.901969F;
            this.lineHeaderVerCurrencyCode.X2 = 3.901968F;
            this.lineHeaderVerCurrencyCode.Y1 = 0.5866142F;
            this.lineHeaderVerCurrencyCode.Y2 = 0.9212598F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 6.29425E-05F;
            this.lineHeaderHorLower.Left = 2.384186E-07F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 0.9212599F;
            this.lineHeaderHorLower.Width = 10.62992F;
            this.lineHeaderHorLower.X1 = 2.384186E-07F;
            this.lineHeaderHorLower.X2 = 10.62992F;
            this.lineHeaderHorLower.Y1 = 0.9212599F;
            this.lineHeaderHorLower.Y2 = 0.9213228F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.3346456F;
            this.lineHeaderVerCustomerCode.Left = 2.51378F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.5866142F;
            this.lineHeaderVerCustomerCode.Width = 0.0001418591F;
            this.lineHeaderVerCustomerCode.X1 = 2.51378F;
            this.lineHeaderVerCustomerCode.X2 = 2.513922F;
            this.lineHeaderVerCustomerCode.Y1 = 0.5866142F;
            this.lineHeaderVerCustomerCode.Y2 = 0.9212598F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCurrencyCode,
            this.txtCustomerName,
            this.txtAmount,
            this.txtOutputAt,
            this.txtReceiptAmount,
            this.txtRecordedAt,
            this.txtPayerName,
            this.txtBilledAt,
            this.txtInvoiceCode,
            this.txtBillingAmount,
            this.txtCreateAt,
            this.txtJJournalizingName,
            this.txtCustomerCode,
            this.lineDetailVerCreateAt,
            this.lineDetailVerJournalizingName,
            this.lineDetailVerCustomerName,
            this.lineDetailHorLower,
            this.lineDetailVerCurrencyCode,
            this.lineDetailVerPayerName,
            this.lineDetailVerAmount,
            this.lineDetailVerCustomerCode,
            this.lineDetailVerOutputAt,
            this.lineDetailVerReceiptAmount,
            this.lineDetailVerRecordedAt,
            this.lineDetailVerBilledAt,
            this.lineDetailVerInvoiceCode});
            this.detail.Height = 0.3070866F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.Height = 0.3070866F;
            this.txtCurrencyCode.Left = 3.517323F;
            this.txtCurrencyCode.MultiLine = false;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Style = "text-align: center; vertical-align: middle";
            this.txtCurrencyCode.Text = "CurrencyCode";
            this.txtCurrencyCode.Top = 0F;
            this.txtCurrencyCode.Width = 0.3799213F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.3070866F;
            this.txtCustomerName.Left = 2.519685F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Style = "text-align: left; vertical-align: middle";
            this.txtCustomerName.Text = "CustomerName";
            this.txtCustomerName.Top = 0F;
            this.txtCustomerName.Width = 0.984252F;
            // 
            // txtAmount
            // 
            this.txtAmount.Height = 0.3070866F;
            this.txtAmount.Left = 3.913386F;
            this.txtAmount.MultiLine = false;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.OutputFormat = resources.GetString("txtAmount.OutputFormat");
            this.txtAmount.Style = "text-align: right; vertical-align: middle";
            this.txtAmount.Text = "Amount";
            this.txtAmount.Top = 0F;
            this.txtAmount.Width = 0.984252F;
            // 
            // txtOutputAt
            // 
            this.txtOutputAt.Height = 0.3070866F;
            this.txtOutputAt.Left = 4.903543F;
            this.txtOutputAt.MultiLine = false;
            this.txtOutputAt.Name = "txtOutputAt";
            this.txtOutputAt.OutputFormat = resources.GetString("txtOutputAt.OutputFormat");
            this.txtOutputAt.Style = "text-align: center; vertical-align: middle";
            this.txtOutputAt.Text = "OutputAt";
            this.txtOutputAt.Top = 0F;
            this.txtOutputAt.Width = 0.7086614F;
            // 
            // txtReceiptAmount
            // 
            this.txtReceiptAmount.Height = 0.3070866F;
            this.txtReceiptAmount.Left = 5.616536F;
            this.txtReceiptAmount.MultiLine = false;
            this.txtReceiptAmount.Name = "txtReceiptAmount";
            this.txtReceiptAmount.OutputFormat = resources.GetString("txtReceiptAmount.OutputFormat");
            this.txtReceiptAmount.Style = "text-align: right; vertical-align: middle";
            this.txtReceiptAmount.Text = "ReceiptAmount";
            this.txtReceiptAmount.Top = 0F;
            this.txtReceiptAmount.Width = 0.9645669F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.3070866F;
            this.txtRecordedAt.Left = 6.605906F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "text-align: center; vertical-align: middle";
            this.txtRecordedAt.Text = "RecordedAt";
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.7086614F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.3070866F;
            this.txtPayerName.Left = 7.333465F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Style = "vertical-align: middle";
            this.txtPayerName.Text = "PayerName";
            this.txtPayerName.Top = 0F;
            this.txtPayerName.Width = 0.9645669F;
            // 
            // txtBilledAt
            // 
            this.txtBilledAt.Height = 0.3070866F;
            this.txtBilledAt.Left = 8.309843F;
            this.txtBilledAt.MultiLine = false;
            this.txtBilledAt.Name = "txtBilledAt";
            this.txtBilledAt.OutputFormat = resources.GetString("txtBilledAt.OutputFormat");
            this.txtBilledAt.Style = "text-align: center; vertical-align: middle";
            this.txtBilledAt.Text = "BilledAt";
            this.txtBilledAt.Top = 0F;
            this.txtBilledAt.Width = 0.7086614F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.Height = 0.3070866F;
            this.txtInvoiceCode.Left = 9.038584F;
            this.txtInvoiceCode.MultiLine = false;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Style = "text-align: center; vertical-align: middle";
            this.txtInvoiceCode.Text = "InvoiceCode";
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 0.7086614F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.Height = 0.3070866F;
            this.txtBillingAmount.Left = 9.772048F;
            this.txtBillingAmount.MultiLine = false;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.OutputFormat = resources.GetString("txtBillingAmount.OutputFormat");
            this.txtBillingAmount.Style = "text-align: right; vertical-align: middle";
            this.txtBillingAmount.Text = "BillingAmount";
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 0.8582678F;
            // 
            // txtCreateAt
            // 
            this.txtCreateAt.Height = 0.3070866F;
            this.txtCreateAt.Left = 0F;
            this.txtCreateAt.MultiLine = false;
            this.txtCreateAt.Name = "txtCreateAt";
            this.txtCreateAt.OutputFormat = resources.GetString("txtCreateAt.OutputFormat");
            this.txtCreateAt.Style = "text-align: center; vertical-align: middle";
            this.txtCreateAt.Text = "CreateAt";
            this.txtCreateAt.Top = 0F;
            this.txtCreateAt.Width = 0.7086614F;
            // 
            // txtJJournalizingName
            // 
            this.txtJJournalizingName.Height = 0.3070866F;
            this.txtJJournalizingName.Left = 0.7204725F;
            this.txtJJournalizingName.MultiLine = false;
            this.txtJJournalizingName.Name = "txtJJournalizingName";
            this.txtJJournalizingName.Style = "text-align: center; vertical-align: middle";
            this.txtJJournalizingName.Text = "JournalizingName";
            this.txtJJournalizingName.Top = 0F;
            this.txtJJournalizingName.Width = 0.9011811F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.3070866F;
            this.txtCustomerCode.Left = 1.649606F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "text-align: center; vertical-align: middle";
            this.txtCustomerCode.Text = "CustomerCode";
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 0.8661417F;
            // 
            // lineDetailVerCreateAt
            // 
            this.lineDetailVerCreateAt.Height = 0.3070866F;
            this.lineDetailVerCreateAt.Left = 0.7236221F;
            this.lineDetailVerCreateAt.LineWeight = 1F;
            this.lineDetailVerCreateAt.Name = "lineDetailVerCreateAt";
            this.lineDetailVerCreateAt.Top = 0F;
            this.lineDetailVerCreateAt.Width = 0F;
            this.lineDetailVerCreateAt.X1 = 0.7236221F;
            this.lineDetailVerCreateAt.X2 = 0.7236221F;
            this.lineDetailVerCreateAt.Y1 = 0F;
            this.lineDetailVerCreateAt.Y2 = 0.3070866F;
            // 
            // lineDetailVerJournalizingName
            // 
            this.lineDetailVerJournalizingName.Height = 0.3070866F;
            this.lineDetailVerJournalizingName.Left = 1.637795F;
            this.lineDetailVerJournalizingName.LineWeight = 1F;
            this.lineDetailVerJournalizingName.Name = "lineDetailVerJournalizingName";
            this.lineDetailVerJournalizingName.Top = 0F;
            this.lineDetailVerJournalizingName.Width = 0F;
            this.lineDetailVerJournalizingName.X1 = 1.637795F;
            this.lineDetailVerJournalizingName.X2 = 1.637795F;
            this.lineDetailVerJournalizingName.Y1 = 0F;
            this.lineDetailVerJournalizingName.Y2 = 0.3070866F;
            // 
            // lineDetailVerCustomerName
            // 
            this.lineDetailVerCustomerName.Height = 0.3070866F;
            this.lineDetailVerCustomerName.Left = 3.509448F;
            this.lineDetailVerCustomerName.LineWeight = 1F;
            this.lineDetailVerCustomerName.Name = "lineDetailVerCustomerName";
            this.lineDetailVerCustomerName.Top = 0F;
            this.lineDetailVerCustomerName.Width = 0F;
            this.lineDetailVerCustomerName.X1 = 3.509448F;
            this.lineDetailVerCustomerName.X2 = 3.509448F;
            this.lineDetailVerCustomerName.Y1 = 0F;
            this.lineDetailVerCustomerName.Y2 = 0.3070866F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.3070866F;
            this.lineDetailHorLower.Width = 10.62992F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.62992F;
            this.lineDetailHorLower.Y1 = 0.3070866F;
            this.lineDetailHorLower.Y2 = 0.3070866F;
            // 
            // lineDetailVerCurrencyCode
            // 
            this.lineDetailVerCurrencyCode.Height = 0.3070866F;
            this.lineDetailVerCurrencyCode.Left = 3.901969F;
            this.lineDetailVerCurrencyCode.LineWeight = 1F;
            this.lineDetailVerCurrencyCode.Name = "lineDetailVerCurrencyCode";
            this.lineDetailVerCurrencyCode.Top = 0F;
            this.lineDetailVerCurrencyCode.Width = 0F;
            this.lineDetailVerCurrencyCode.X1 = 3.901969F;
            this.lineDetailVerCurrencyCode.X2 = 3.901969F;
            this.lineDetailVerCurrencyCode.Y1 = 0F;
            this.lineDetailVerCurrencyCode.Y2 = 0.3070866F;
            // 
            // lineDetailVerPayerName
            // 
            this.lineDetailVerPayerName.Height = 0.3070866F;
            this.lineDetailVerPayerName.Left = 8.298032F;
            this.lineDetailVerPayerName.LineWeight = 1F;
            this.lineDetailVerPayerName.Name = "lineDetailVerPayerName";
            this.lineDetailVerPayerName.Top = 0F;
            this.lineDetailVerPayerName.Width = 0F;
            this.lineDetailVerPayerName.X1 = 8.298032F;
            this.lineDetailVerPayerName.X2 = 8.298032F;
            this.lineDetailVerPayerName.Y1 = 0F;
            this.lineDetailVerPayerName.Y2 = 0.3070866F;
            // 
            // lineDetailVerAmount
            // 
            this.lineDetailVerAmount.Height = 0.3070866F;
            this.lineDetailVerAmount.Left = 4.903937F;
            this.lineDetailVerAmount.LineWeight = 1F;
            this.lineDetailVerAmount.Name = "lineDetailVerAmount";
            this.lineDetailVerAmount.Top = 0F;
            this.lineDetailVerAmount.Width = 0F;
            this.lineDetailVerAmount.X1 = 4.903937F;
            this.lineDetailVerAmount.X2 = 4.903937F;
            this.lineDetailVerAmount.Y1 = 0F;
            this.lineDetailVerAmount.Y2 = 0.3070866F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.3070866F;
            this.lineDetailVerCustomerCode.Left = 2.51378F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 2.51378F;
            this.lineDetailVerCustomerCode.X2 = 2.51378F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.3070866F;
            // 
            // lineDetailVerOutputAt
            // 
            this.lineDetailVerOutputAt.Height = 0.3070866F;
            this.lineDetailVerOutputAt.Left = 5.616481F;
            this.lineDetailVerOutputAt.LineWeight = 1F;
            this.lineDetailVerOutputAt.Name = "lineDetailVerOutputAt";
            this.lineDetailVerOutputAt.Top = 0F;
            this.lineDetailVerOutputAt.Width = 0F;
            this.lineDetailVerOutputAt.X1 = 5.616481F;
            this.lineDetailVerOutputAt.X2 = 5.616481F;
            this.lineDetailVerOutputAt.Y1 = 0F;
            this.lineDetailVerOutputAt.Y2 = 0.3070866F;
            // 
            // lineDetailVerReceiptAmount
            // 
            this.lineDetailVerReceiptAmount.Height = 0.3070866F;
            this.lineDetailVerReceiptAmount.Left = 6.596064F;
            this.lineDetailVerReceiptAmount.LineWeight = 1F;
            this.lineDetailVerReceiptAmount.Name = "lineDetailVerReceiptAmount";
            this.lineDetailVerReceiptAmount.Top = 0F;
            this.lineDetailVerReceiptAmount.Width = 0F;
            this.lineDetailVerReceiptAmount.X1 = 6.596064F;
            this.lineDetailVerReceiptAmount.X2 = 6.596064F;
            this.lineDetailVerReceiptAmount.Y1 = 0F;
            this.lineDetailVerReceiptAmount.Y2 = 0.3070866F;
            // 
            // lineDetailVerRecordedAt
            // 
            this.lineDetailVerRecordedAt.Height = 0.3070866F;
            this.lineDetailVerRecordedAt.Left = 7.327559F;
            this.lineDetailVerRecordedAt.LineWeight = 1F;
            this.lineDetailVerRecordedAt.Name = "lineDetailVerRecordedAt";
            this.lineDetailVerRecordedAt.Top = 0F;
            this.lineDetailVerRecordedAt.Width = 0F;
            this.lineDetailVerRecordedAt.X1 = 7.327559F;
            this.lineDetailVerRecordedAt.X2 = 7.327559F;
            this.lineDetailVerRecordedAt.Y1 = 0F;
            this.lineDetailVerRecordedAt.Y2 = 0.3070866F;
            // 
            // lineDetailVerBilledAt
            // 
            this.lineDetailVerBilledAt.Height = 0.3070866F;
            this.lineDetailVerBilledAt.Left = 9.030709F;
            this.lineDetailVerBilledAt.LineWeight = 1F;
            this.lineDetailVerBilledAt.Name = "lineDetailVerBilledAt";
            this.lineDetailVerBilledAt.Top = 0F;
            this.lineDetailVerBilledAt.Width = 0F;
            this.lineDetailVerBilledAt.X1 = 9.030709F;
            this.lineDetailVerBilledAt.X2 = 9.030709F;
            this.lineDetailVerBilledAt.Y1 = 0F;
            this.lineDetailVerBilledAt.Y2 = 0.3070866F;
            // 
            // lineDetailVerInvoiceCode
            // 
            this.lineDetailVerInvoiceCode.Height = 0.3070866F;
            this.lineDetailVerInvoiceCode.Left = 9.760237F;
            this.lineDetailVerInvoiceCode.LineWeight = 1F;
            this.lineDetailVerInvoiceCode.Name = "lineDetailVerInvoiceCode";
            this.lineDetailVerInvoiceCode.Top = 0F;
            this.lineDetailVerInvoiceCode.Width = 0F;
            this.lineDetailVerInvoiceCode.X1 = 9.760237F;
            this.lineDetailVerInvoiceCode.X2 = 9.760237F;
            this.lineDetailVerInvoiceCode.Y1 = 0F;
            this.lineDetailVerInvoiceCode.Y2 = 0.3070866F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblPageNumber,
            this.reportInfo1});
            this.pageFooter.Height = 0.34375F;
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
            this.reportInfo1.Left = 8.267717F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.07874016F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1.48189F;
            // 
            // MatchingJournalizingCancellationReport
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
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblJournalizingName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtJJournalizingName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblJournalizingName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtJJournalizingName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerJournalizingName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblOutputAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCreateAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerOutputAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtOutputAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerJournalizingType;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrencyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerOutputAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
    }
}
