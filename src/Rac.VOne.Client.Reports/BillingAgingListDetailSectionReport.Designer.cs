namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillingAgingListDetailSectionReport の概要の説明です。
    /// </summary>
    partial class BillingAgingListDetailSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BillingAgingListDetailSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomer = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineDetailVerCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorBilledAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDueAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSpace = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineFooterVerTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotalBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineFooterVerTotalRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtTotalBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtTotalRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineFooterHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
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
            this.lblCustomer,
            this.lblCustomerName,
            this.lblBilledAt,
            this.lblStaffCode,
            this.lblDueAt,
            this.lblInvoiceCode,
            this.lblStaffName,
            this.lblBillingAmount,
            this.lblRemainAmount,
            this.lblNote,
            this.lineHeaderHorLower,
            this.lineHeaderHorUpper,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderVerCustomerName,
            this.lineHeaderVerBilledAt,
            this.lineHeaderVerInvoiceCode,
            this.lineHeaderVerBillingAmount,
            this.lineHeaderVerRemainAmount,
            this.lineHeaderHorBilledAt,
            this.lineHeaderVerDueAt});
            this.pageHeader.Height = 1.111811F;
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
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "請求残高年齢表（明細）";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblCustomer
            // 
            this.lblCustomer.Height = 0.496063F;
            this.lblCustomer.HyperLink = null;
            this.lblCustomer.Left = 0F;
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomer.Text = "得意先コード";
            this.lblCustomer.Top = 0.6181103F;
            this.lblCustomer.Width = 0.9950001F;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Height = 0.496063F;
            this.lblCustomerName.HyperLink = null;
            this.lblCustomerName.Left = 0.9950001F;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.Top = 0.6181103F;
            this.lblCustomerName.Width = 2.015F;
            // 
            // lblBilledAt
            // 
            this.lblBilledAt.Height = 0.25F;
            this.lblBilledAt.HyperLink = null;
            this.lblBilledAt.Left = 3.007F;
            this.lblBilledAt.Name = "lblBilledAt";
            this.lblBilledAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblBilledAt.Text = "請求日";
            this.lblBilledAt.Top = 0.6181103F;
            this.lblBilledAt.Width = 1.012F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.25F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 3.007087F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblStaffCode.Text = "担当者コード";
            this.lblStaffCode.Top = 0.8598425F;
            this.lblStaffCode.Width = 1.012F;
            // 
            // lblDueAt
            // 
            this.lblDueAt.Height = 0.25F;
            this.lblDueAt.HyperLink = null;
            this.lblDueAt.Left = 4.022F;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDueAt.Text = "入金予定日";
            this.lblDueAt.Top = 0.6181103F;
            this.lblDueAt.Width = 0.9050003F;
            // 
            // lblInvoiceCode
            // 
            this.lblInvoiceCode.Height = 0.25F;
            this.lblInvoiceCode.HyperLink = null;
            this.lblInvoiceCode.Left = 4.927F;
            this.lblInvoiceCode.Name = "lblInvoiceCode";
            this.lblInvoiceCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblInvoiceCode.Text = "請求書番号";
            this.lblInvoiceCode.Top = 0.6181103F;
            this.lblInvoiceCode.Width = 1.313F;
            // 
            // lblStaffName
            // 
            this.lblStaffName.Height = 0.25F;
            this.lblStaffName.HyperLink = null;
            this.lblStaffName.Left = 4.019F;
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblStaffName.Text = "  担当者名";
            this.lblStaffName.Top = 0.8598425F;
            this.lblStaffName.Width = 2.218F;
            // 
            // lblBillingAmount
            // 
            this.lblBillingAmount.Height = 0.496063F;
            this.lblBillingAmount.HyperLink = null;
            this.lblBillingAmount.Left = 6.24F;
            this.lblBillingAmount.Name = "lblBillingAmount";
            this.lblBillingAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblBillingAmount.Text = "請求金額";
            this.lblBillingAmount.Top = 0.6181103F;
            this.lblBillingAmount.Width = 1.214F;
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Height = 0.496063F;
            this.lblRemainAmount.HyperLink = null;
            this.lblRemainAmount.Left = 7.461024F;
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblRemainAmount.Text = "請求残";
            this.lblRemainAmount.Top = 0.6181103F;
            this.lblRemainAmount.Width = 1.228346F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.496063F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 8.683001F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblNote.Text = "備考";
            this.lblNote.Top = 0.6181103F;
            this.lblNote.Width = 1.952F;
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
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.4976378F;
            this.lineHeaderVerCustomerCode.Left = 0.995F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.6141732F;
            this.lineHeaderVerCustomerCode.Width = 1.192093E-07F;
            this.lineHeaderVerCustomerCode.X1 = 0.9950001F;
            this.lineHeaderVerCustomerCode.X2 = 0.995F;
            this.lineHeaderVerCustomerCode.Y1 = 0.6141732F;
            this.lineHeaderVerCustomerCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerCustomerName
            // 
            this.lineHeaderVerCustomerName.Height = 0.4976378F;
            this.lineHeaderVerCustomerName.Left = 3.01F;
            this.lineHeaderVerCustomerName.LineWeight = 1F;
            this.lineHeaderVerCustomerName.Name = "lineHeaderVerCustomerName";
            this.lineHeaderVerCustomerName.Top = 0.6141732F;
            this.lineHeaderVerCustomerName.Width = 0F;
            this.lineHeaderVerCustomerName.X1 = 3.01F;
            this.lineHeaderVerCustomerName.X2 = 3.01F;
            this.lineHeaderVerCustomerName.Y1 = 0.6141732F;
            this.lineHeaderVerCustomerName.Y2 = 1.111811F;
            // 
            // lineHeaderVerBilledAt
            // 
            this.lineHeaderVerBilledAt.Height = 0.4976378F;
            this.lineHeaderVerBilledAt.Left = 4.019F;
            this.lineHeaderVerBilledAt.LineWeight = 1F;
            this.lineHeaderVerBilledAt.Name = "lineHeaderVerBilledAt";
            this.lineHeaderVerBilledAt.Top = 0.6141732F;
            this.lineHeaderVerBilledAt.Width = 0.002999783F;
            this.lineHeaderVerBilledAt.X1 = 4.022F;
            this.lineHeaderVerBilledAt.X2 = 4.019F;
            this.lineHeaderVerBilledAt.Y1 = 0.6141732F;
            this.lineHeaderVerBilledAt.Y2 = 1.111811F;
            // 
            // lineHeaderVerInvoiceCode
            // 
            this.lineHeaderVerInvoiceCode.Height = 0.4976378F;
            this.lineHeaderVerInvoiceCode.Left = 6.24F;
            this.lineHeaderVerInvoiceCode.LineWeight = 1F;
            this.lineHeaderVerInvoiceCode.Name = "lineHeaderVerInvoiceCode";
            this.lineHeaderVerInvoiceCode.Top = 0.6141732F;
            this.lineHeaderVerInvoiceCode.Width = 0F;
            this.lineHeaderVerInvoiceCode.X1 = 6.24F;
            this.lineHeaderVerInvoiceCode.X2 = 6.24F;
            this.lineHeaderVerInvoiceCode.Y1 = 0.6141732F;
            this.lineHeaderVerInvoiceCode.Y2 = 1.111811F;
            // 
            // lineHeaderVerBillingAmount
            // 
            this.lineHeaderVerBillingAmount.Height = 0.4976378F;
            this.lineHeaderVerBillingAmount.Left = 7.454F;
            this.lineHeaderVerBillingAmount.LineWeight = 1F;
            this.lineHeaderVerBillingAmount.Name = "lineHeaderVerBillingAmount";
            this.lineHeaderVerBillingAmount.Top = 0.6141732F;
            this.lineHeaderVerBillingAmount.Width = 0F;
            this.lineHeaderVerBillingAmount.X1 = 7.454F;
            this.lineHeaderVerBillingAmount.X2 = 7.454F;
            this.lineHeaderVerBillingAmount.Y1 = 0.6141732F;
            this.lineHeaderVerBillingAmount.Y2 = 1.111811F;
            // 
            // lineHeaderVerRemainAmount
            // 
            this.lineHeaderVerRemainAmount.Height = 0.4976378F;
            this.lineHeaderVerRemainAmount.Left = 8.683001F;
            this.lineHeaderVerRemainAmount.LineWeight = 1F;
            this.lineHeaderVerRemainAmount.Name = "lineHeaderVerRemainAmount";
            this.lineHeaderVerRemainAmount.Top = 0.6141732F;
            this.lineHeaderVerRemainAmount.Width = 0F;
            this.lineHeaderVerRemainAmount.X1 = 8.683001F;
            this.lineHeaderVerRemainAmount.X2 = 8.683001F;
            this.lineHeaderVerRemainAmount.Y1 = 0.6141732F;
            this.lineHeaderVerRemainAmount.Y2 = 1.111811F;
            // 
            // lineHeaderHorBilledAt
            // 
            this.lineHeaderHorBilledAt.Height = 0F;
            this.lineHeaderHorBilledAt.Left = 3.009843F;
            this.lineHeaderHorBilledAt.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderHorBilledAt.LineWeight = 1F;
            this.lineHeaderHorBilledAt.Name = "lineHeaderHorBilledAt";
            this.lineHeaderHorBilledAt.Top = 0.8622047F;
            this.lineHeaderHorBilledAt.Width = 3.230315F;
            this.lineHeaderHorBilledAt.X1 = 3.009843F;
            this.lineHeaderHorBilledAt.X2 = 6.240158F;
            this.lineHeaderHorBilledAt.Y1 = 0.8622047F;
            this.lineHeaderHorBilledAt.Y2 = 0.8622047F;
            // 
            // lineHeaderVerDueAt
            // 
            this.lineHeaderVerDueAt.Height = 0.2480315F;
            this.lineHeaderVerDueAt.Left = 4.927F;
            this.lineHeaderVerDueAt.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineHeaderVerDueAt.LineWeight = 1F;
            this.lineHeaderVerDueAt.Name = "lineHeaderVerDueAt";
            this.lineHeaderVerDueAt.Top = 0.6141732F;
            this.lineHeaderVerDueAt.Width = 0.0001659393F;
            this.lineHeaderVerDueAt.X1 = 4.927F;
            this.lineHeaderVerDueAt.X2 = 4.927166F;
            this.lineHeaderVerDueAt.Y1 = 0.6141732F;
            this.lineHeaderVerDueAt.Y2 = 0.8622047F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtStaffCode,
            this.txtStaffName,
            this.txtBillingAmount,
            this.txtRemainAmount,
            this.txtDueAt,
            this.txtCustomerCode,
            this.txtCustomerName,
            this.txtNote,
            this.lineDetailHorLower,
            this.lineDetailVerCustomerCode,
            this.txtInvoiceCode,
            this.lineDetailVerCustomerName,
            this.lineDetailVerBilledAt,
            this.lineDetailVerInvoiceCode,
            this.lineDetailVerBillingAmount,
            this.lineDetailVerRemainAmount,
            this.lineDetailVerDueAt,
            this.txtBillingAt,
            this.lineDetailHorBilledAt});
            this.detail.Height = 0.4598425F;
            this.detail.Name = "detail";
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.4566929F;
            this.txtCustomerCode.HyperLink = null;
            this.txtCustomerCode.Left = 0F;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "text-align: center; vertical-align: middle";
            this.txtCustomerCode.Text = "";
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 0.9948819F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.4566929F;
            this.txtCustomerName.HyperLink = null;
            this.txtCustomerName.Left = 1.007874F;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Style = "text-align: left; vertical-align: middle";
            this.txtCustomerName.Text = "";
            this.txtCustomerName.Top = 0F;
            this.txtCustomerName.Width = 1.992126F;
            // 
            // txtNote
            // 
            this.txtNote.Height = 0.4566929F;
            this.txtNote.HyperLink = null;
            this.txtNote.Left = 8.704725F;
            this.txtNote.Name = "txtNote";
            this.txtNote.Style = "text-align: left; vertical-align: middle";
            this.txtNote.Text = "";
            this.txtNote.Top = 0F;
            this.txtNote.Width = 1.925197F;
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
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.4598425F;
            this.lineDetailVerCustomerCode.Left = 0.9949999F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 2.384186E-07F;
            this.lineDetailVerCustomerCode.X1 = 0.9950001F;
            this.lineDetailVerCustomerCode.X2 = 0.9949999F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.4598425F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.Height = 0.2244094F;
            this.txtStaffCode.HyperLink = null;
            this.txtStaffCode.Left = 3.023622F;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Style = "text-align: center; vertical-align: middle";
            this.txtStaffCode.Text = "";
            this.txtStaffCode.Top = 0.2322835F;
            this.txtStaffCode.Width = 0.9921263F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.Height = 0.2244094F;
            this.txtInvoiceCode.HyperLink = null;
            this.txtInvoiceCode.Left = 4.938977F;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Style = "text-align: center; vertical-align: middle";
            this.txtInvoiceCode.Text = "";
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 1.291339F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.Height = 0.2244094F;
            this.txtStaffName.HyperLink = null;
            this.txtStaffName.Left = 4.033859F;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Style = "text-align: left; vertical-align: middle";
            this.txtStaffName.Text = "";
            this.txtStaffName.Top = 0.2322835F;
            this.txtStaffName.Width = 2.19685F;
            // 
            // lineDetailVerCustomerName
            // 
            this.lineDetailVerCustomerName.Height = 0.4598425F;
            this.lineDetailVerCustomerName.Left = 3.01F;
            this.lineDetailVerCustomerName.LineWeight = 1F;
            this.lineDetailVerCustomerName.Name = "lineDetailVerCustomerName";
            this.lineDetailVerCustomerName.Top = 0F;
            this.lineDetailVerCustomerName.Width = 0F;
            this.lineDetailVerCustomerName.X1 = 3.01F;
            this.lineDetailVerCustomerName.X2 = 3.01F;
            this.lineDetailVerCustomerName.Y1 = 0F;
            this.lineDetailVerCustomerName.Y2 = 0.4598425F;
            // 
            // lineDetailVerBilledAt
            // 
            this.lineDetailVerBilledAt.Height = 0.4598425F;
            this.lineDetailVerBilledAt.Left = 4.022F;
            this.lineDetailVerBilledAt.LineWeight = 1F;
            this.lineDetailVerBilledAt.Name = "lineDetailVerBilledAt";
            this.lineDetailVerBilledAt.Top = 0F;
            this.lineDetailVerBilledAt.Width = 0F;
            this.lineDetailVerBilledAt.X1 = 4.022F;
            this.lineDetailVerBilledAt.X2 = 4.022F;
            this.lineDetailVerBilledAt.Y1 = 0F;
            this.lineDetailVerBilledAt.Y2 = 0.4598425F;
            // 
            // lineDetailVerInvoiceCode
            // 
            this.lineDetailVerInvoiceCode.Height = 0.4598425F;
            this.lineDetailVerInvoiceCode.Left = 6.24F;
            this.lineDetailVerInvoiceCode.LineWeight = 1F;
            this.lineDetailVerInvoiceCode.Name = "lineDetailVerInvoiceCode";
            this.lineDetailVerInvoiceCode.Top = 0F;
            this.lineDetailVerInvoiceCode.Width = 0F;
            this.lineDetailVerInvoiceCode.X1 = 6.24F;
            this.lineDetailVerInvoiceCode.X2 = 6.24F;
            this.lineDetailVerInvoiceCode.Y1 = 0F;
            this.lineDetailVerInvoiceCode.Y2 = 0.4598425F;
            // 
            // lineDetailVerBillingAmount
            // 
            this.lineDetailVerBillingAmount.Height = 0.4598425F;
            this.lineDetailVerBillingAmount.Left = 7.454F;
            this.lineDetailVerBillingAmount.LineWeight = 1F;
            this.lineDetailVerBillingAmount.Name = "lineDetailVerBillingAmount";
            this.lineDetailVerBillingAmount.Top = 0F;
            this.lineDetailVerBillingAmount.Width = 0F;
            this.lineDetailVerBillingAmount.X1 = 7.454F;
            this.lineDetailVerBillingAmount.X2 = 7.454F;
            this.lineDetailVerBillingAmount.Y1 = 0F;
            this.lineDetailVerBillingAmount.Y2 = 0.4598425F;
            // 
            // lineDetailVerRemainAmount
            // 
            this.lineDetailVerRemainAmount.Height = 0.4598425F;
            this.lineDetailVerRemainAmount.Left = 8.683001F;
            this.lineDetailVerRemainAmount.LineWeight = 1F;
            this.lineDetailVerRemainAmount.Name = "lineDetailVerRemainAmount";
            this.lineDetailVerRemainAmount.Top = 0F;
            this.lineDetailVerRemainAmount.Width = 0F;
            this.lineDetailVerRemainAmount.X1 = 8.683001F;
            this.lineDetailVerRemainAmount.X2 = 8.683001F;
            this.lineDetailVerRemainAmount.Y1 = 0F;
            this.lineDetailVerRemainAmount.Y2 = 0.4598425F;
            // 
            // lineDetailHorBilledAt
            // 
            this.lineDetailHorBilledAt.Height = 0F;
            this.lineDetailHorBilledAt.Left = 3.009843F;
            this.lineDetailHorBilledAt.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineDetailHorBilledAt.LineWeight = 1F;
            this.lineDetailHorBilledAt.Name = "lineDetailHorBilledAt";
            this.lineDetailHorBilledAt.Top = 0.232F;
            this.lineDetailHorBilledAt.Width = 3.230315F;
            this.lineDetailHorBilledAt.X1 = 3.009843F;
            this.lineDetailHorBilledAt.X2 = 6.240158F;
            this.lineDetailHorBilledAt.Y1 = 0.232F;
            this.lineDetailHorBilledAt.Y2 = 0.232F;
            // 
            // lineDetailVerDueAt
            // 
            this.lineDetailVerDueAt.Height = 0.2318898F;
            this.lineDetailVerDueAt.Left = 4.927F;
            this.lineDetailVerDueAt.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineDetailVerDueAt.LineWeight = 1F;
            this.lineDetailVerDueAt.Name = "lineDetailVerDueAt";
            this.lineDetailVerDueAt.Top = 0F;
            this.lineDetailVerDueAt.Width = 0.0001659393F;
            this.lineDetailVerDueAt.X1 = 4.927166F;
            this.lineDetailVerDueAt.X2 = 4.927F;
            this.lineDetailVerDueAt.Y1 = 0.2318898F;
            this.lineDetailVerDueAt.Y2 = 0F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.DataField = "BillingAmount";
            this.txtBillingAmount.Height = 0.4566929F;
            this.txtBillingAmount.Left = 6.251969F;
            this.txtBillingAmount.MultiLine = false;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.OutputFormat = resources.GetString("txtBillingAmount.OutputFormat");
            this.txtBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingAmount.Style = "text-align: right; vertical-align: middle";
            this.txtBillingAmount.Text = null;
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 1.19685F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.DataField = "RemainAmount";
            this.txtRemainAmount.Height = 0.4566929F;
            this.txtRemainAmount.Left = 7.472442F;
            this.txtRemainAmount.MultiLine = false;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.OutputFormat = resources.GetString("txtRemainAmount.OutputFormat");
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainAmount.Style = "text-align: right; vertical-align: middle";
            this.txtRemainAmount.Text = null;
            this.txtRemainAmount.Top = 0F;
            this.txtRemainAmount.Width = 1.19685F;
            // 
            // txtBillingAt
            // 
            this.txtBillingAt.DataField = "BilledAt";
            this.txtBillingAt.Height = 0.2244094F;
            this.txtBillingAt.Left = 3.023622F;
            this.txtBillingAt.Name = "txtBillingAt";
            this.txtBillingAt.OutputFormat = resources.GetString("txtBillingAt.OutputFormat");
            this.txtBillingAt.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtBillingAt.Text = null;
            this.txtBillingAt.Top = 0F;
            this.txtBillingAt.Width = 0.9921263F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.DataField = "DueAt";
            this.txtDueAt.Height = 0.2244094F;
            this.txtDueAt.Left = 4.033859F;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "text-align: center; vertical-align: middle; white-space: nowrap; ddo-wrap-mode: n" +
    "owrap";
            this.txtDueAt.Text = null;
            this.txtDueAt.Top = 0F;
            this.txtDueAt.Width = 0.8897638F;
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
            // groupHeader1
            // 
            this.groupHeader1.Height = 0F;
            this.groupHeader1.Name = "groupHeader1";
            // 
            // groupFooter1
            // 
            this.groupFooter1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtTotalBillingAmount,
            this.lblTotal,
            this.lblSpace,
            this.lineFooterVerTotal,
            this.lineFooterVerTotalBillingAmount,
            this.lineFooterVerTotalRemainAmount,
            this.txtTotalRemainAmount,
            this.lineFooterHorLower});
            this.groupFooter1.Height = 0.4122047F;
            this.groupFooter1.Name = "groupFooter1";
            this.groupFooter1.Format += new System.EventHandler(this.groupFooter1_Format);
            // 
            // lblTotal
            // 
            this.lblTotal.Height = 0.4122047F;
            this.lblTotal.HyperLink = null;
            this.lblTotal.Left = 0F;
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(15, 0, 0, 0);
            this.lblTotal.Style = "background-color: WhiteSmoke; text-align: left; vertical-align: middle";
            this.lblTotal.Text = "合計";
            this.lblTotal.Top = 0F;
            this.lblTotal.Width = 6.240158F;
            // 
            // lblSpace
            // 
            this.lblSpace.Height = 0.4122047F;
            this.lblSpace.HyperLink = null;
            this.lblSpace.Left = 8.688977F;
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Padding = new GrapeCity.ActiveReports.PaddingEx(10, 0, 0, 0);
            this.lblSpace.Style = "background-color: WhiteSmoke; text-align: left; vertical-align: middle";
            this.lblSpace.Text = "";
            this.lblSpace.Top = 0F;
            this.lblSpace.Width = 1.948819F;
            // 
            // lineFooterVerTotal
            // 
            this.lineFooterVerTotal.Height = 0.4122047F;
            this.lineFooterVerTotal.Left = 6.24F;
            this.lineFooterVerTotal.LineWeight = 1F;
            this.lineFooterVerTotal.Name = "lineFooterVerTotal";
            this.lineFooterVerTotal.Top = 0F;
            this.lineFooterVerTotal.Width = 0F;
            this.lineFooterVerTotal.X1 = 6.24F;
            this.lineFooterVerTotal.X2 = 6.24F;
            this.lineFooterVerTotal.Y1 = 0F;
            this.lineFooterVerTotal.Y2 = 0.4122047F;
            // 
            // lineFooterVerTotalBillingAmount
            // 
            this.lineFooterVerTotalBillingAmount.Height = 0.4122047F;
            this.lineFooterVerTotalBillingAmount.Left = 7.454F;
            this.lineFooterVerTotalBillingAmount.LineWeight = 1F;
            this.lineFooterVerTotalBillingAmount.Name = "lineFooterVerTotalBillingAmount";
            this.lineFooterVerTotalBillingAmount.Top = 0F;
            this.lineFooterVerTotalBillingAmount.Width = 0F;
            this.lineFooterVerTotalBillingAmount.X1 = 7.454F;
            this.lineFooterVerTotalBillingAmount.X2 = 7.454F;
            this.lineFooterVerTotalBillingAmount.Y1 = 0F;
            this.lineFooterVerTotalBillingAmount.Y2 = 0.4122047F;
            // 
            // lineFooterVerTotalRemainAmount
            // 
            this.lineFooterVerTotalRemainAmount.Height = 0.4122047F;
            this.lineFooterVerTotalRemainAmount.Left = 8.683001F;
            this.lineFooterVerTotalRemainAmount.LineWeight = 1F;
            this.lineFooterVerTotalRemainAmount.Name = "lineFooterVerTotalRemainAmount";
            this.lineFooterVerTotalRemainAmount.Top = 0F;
            this.lineFooterVerTotalRemainAmount.Width = 0F;
            this.lineFooterVerTotalRemainAmount.X1 = 8.683001F;
            this.lineFooterVerTotalRemainAmount.X2 = 8.683001F;
            this.lineFooterVerTotalRemainAmount.Y1 = 0F;
            this.lineFooterVerTotalRemainAmount.Y2 = 0.4122047F;
            // 
            // txtTotalBillingAmount
            // 
            this.txtTotalBillingAmount.DataField = "BillingAmount";
            this.txtTotalBillingAmount.Height = 0.4122047F;
            this.txtTotalBillingAmount.Left = 6.251969F;
            this.txtTotalBillingAmount.MultiLine = false;
            this.txtTotalBillingAmount.Name = "txtTotalBillingAmount";
            this.txtTotalBillingAmount.OutputFormat = resources.GetString("txtTotalBillingAmount.OutputFormat");
            this.txtTotalBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtTotalBillingAmount.Style = "background-color: WhiteSmoke; text-align: right; vertical-align: middle";
            this.txtTotalBillingAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalBillingAmount.Text = null;
            this.txtTotalBillingAmount.Top = 0F;
            this.txtTotalBillingAmount.Width = 1.204724F;
            // 
            // txtTotalRemainAmount
            // 
            this.txtTotalRemainAmount.DataField = "RemainAmount";
            this.txtTotalRemainAmount.Height = 0.4122047F;
            this.txtTotalRemainAmount.Left = 7.468898F;
            this.txtTotalRemainAmount.MultiLine = false;
            this.txtTotalRemainAmount.Name = "txtTotalRemainAmount";
            this.txtTotalRemainAmount.OutputFormat = resources.GetString("txtTotalRemainAmount.OutputFormat");
            this.txtTotalRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtTotalRemainAmount.Style = "background-color: WhiteSmoke; text-align: right; vertical-align: middle";
            this.txtTotalRemainAmount.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalRemainAmount.Text = null;
            this.txtTotalRemainAmount.Top = 0F;
            this.txtTotalRemainAmount.Width = 1.204724F;
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
            this.reportInfo1.Left = 6.062993F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.03937008F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1.181102F;
            // 
            // BillingAgingListDetailSectionReport
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
            "t-size: 9pt; font-family: \"ＭＳ 明朝\"; ddo-char-set: 186", "Normal"));
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
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtNote;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSpace;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotalBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineFooterVerTotalRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
    }
}
