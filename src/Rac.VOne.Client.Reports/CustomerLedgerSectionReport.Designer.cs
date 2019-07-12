namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerLedgerSectionReport の概要の説明です。
    /// </summary>
    partial class CustomerLedgerSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CustomerLedgerSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblDebitAccountTitleName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCategoryName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSectionName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblSlipTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMatchingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSlipTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerMatchingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTitleCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTitleCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCaption = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.spBackColor = new GrapeCity.ActiveReports.SectionReportModel.Shape();
            this.txtMatchingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMatchingSymbolReceipt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMatchingSymbolBilling = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSlipTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCategoryName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDeptOrSecName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDebitAccountTitleName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerRecordedAt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDeptOrSecName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerMatchingSymbolBilling = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSlipTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerMatchingSymbolReceipt = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ghParentCustomer = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.gfParentCustomer = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.ghYearMonth = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.gfYearMonth = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitAccountTitleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSlipTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMatchingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitleCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitleCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCaption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingSymbolReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingSymbolBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlipTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptOrSecName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblDebitAccountTitleName,
            this.lblInvoiceCode,
            this.lblCategoryName,
            this.lblRecordedAt,
            this.lblSectionName,
            this.lblDepartmentName,
            this.lblCompanyCode,
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lineHeaderVerRecordedAt,
            this.lblBillingAmount,
            this.lblSlipTotal,
            this.lblRemainAmount,
            this.lblReceiptAmount,
            this.lblMatchingAmount,
            this.lblCustomerCodeName,
            this.lineHeaderHorLower,
            this.lineHeaderVerDepartmentCode,
            this.lineHeaderVerInvoiceCode,
            this.lineHeaderVerBillingAmount,
            this.lineHeaderVerSlipTotal,
            this.lineHeaderVerReceiptAmount,
            this.lineHeaderVerMatchingAmount,
            this.lineHeaderVerRemainAmount,
            this.lblCustomerCode,
            this.txtTitleCustomerCode,
            this.txtTitleCustomerName,
            this.lineHeaderHorUpper});
            this.pageHeader.Height = 1.505118F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblDebitAccountTitleName
            // 
            this.lblDebitAccountTitleName.Height = 0.2F;
            this.lblDebitAccountTitleName.HyperLink = null;
            this.lblDebitAccountTitleName.Left = 1.453937F;
            this.lblDebitAccountTitleName.Name = "lblDebitAccountTitleName";
            this.lblDebitAccountTitleName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDebitAccountTitleName.Text = "債権科目";
            this.lblDebitAccountTitleName.Top = 1.309236F;
            this.lblDebitAccountTitleName.Width = 0.7F;
            // 
            // lblInvoiceCode
            // 
            this.lblInvoiceCode.Height = 0.2F;
            this.lblInvoiceCode.HyperLink = null;
            this.lblInvoiceCode.Left = 1.453937F;
            this.lblInvoiceCode.Name = "lblInvoiceCode";
            this.lblInvoiceCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblInvoiceCode.Text = "請求書番号";
            this.lblInvoiceCode.Top = 1.109055F;
            this.lblInvoiceCode.Width = 0.7F;
            // 
            // lblCategoryName
            // 
            this.lblCategoryName.Height = 0.2F;
            this.lblCategoryName.HyperLink = null;
            this.lblCategoryName.Left = 0F;
            this.lblCategoryName.Name = "lblCategoryName";
            this.lblCategoryName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCategoryName.Text = "区分";
            this.lblCategoryName.Top = 1.301181F;
            this.lblCategoryName.Width = 0.653937F;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Height = 0.2F;
            this.lblRecordedAt.HyperLink = null;
            this.lblRecordedAt.Left = 0F;
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblRecordedAt.Text = "日付";
            this.lblRecordedAt.Top = 1.101181F;
            this.lblRecordedAt.Width = 0.653937F;
            // 
            // lblSectionName
            // 
            this.lblSectionName.Height = 0.2F;
            this.lblSectionName.HyperLink = null;
            this.lblSectionName.Left = 0.653937F;
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblSectionName.Text = "入金部門名";
            this.lblSectionName.Top = 1.301362F;
            this.lblSectionName.Width = 0.8F;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Height = 0.2F;
            this.lblDepartmentName.HyperLink = null;
            this.lblDepartmentName.Left = 0.653937F;
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepartmentName.Text = "請求部門名";
            this.lblDepartmentName.Top = 1.101181F;
            this.lblDepartmentName.Width = 0.8F;
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
            this.lblCompanyCodeName.Width = 3.532677F;
            // 
            // lblDate
            // 
            this.lblDate.Height = 0.2F;
            this.lblDate.HyperLink = null;
            this.lblDate.Left = 5.881102F;
            this.lblDate.Name = "lblDate";
            this.lblDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblDate.Text = "出力日付　：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.6153545F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 6.496063F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle; ddo-char-s" +
    "et: 1";
            this.ridate.Top = 0F;
            this.ridate.Width = 0.7480315F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "得意先別消込台帳";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 7.244094F;
            // 
            // lineHeaderVerRecordedAt
            // 
            this.lineHeaderVerRecordedAt.Height = 0.4F;
            this.lineHeaderVerRecordedAt.Left = 0.653937F;
            this.lineHeaderVerRecordedAt.LineWeight = 1F;
            this.lineHeaderVerRecordedAt.Name = "lineHeaderVerRecordedAt";
            this.lineHeaderVerRecordedAt.Top = 1.101181F;
            this.lineHeaderVerRecordedAt.Width = 0F;
            this.lineHeaderVerRecordedAt.X1 = 0.653937F;
            this.lineHeaderVerRecordedAt.X2 = 0.653937F;
            this.lineHeaderVerRecordedAt.Y1 = 1.101181F;
            this.lineHeaderVerRecordedAt.Y2 = 1.501181F;
            // 
            // lblBillingAmount
            // 
            this.lblBillingAmount.Height = 0.4F;
            this.lblBillingAmount.HyperLink = null;
            this.lblBillingAmount.Left = 2.153937F;
            this.lblBillingAmount.Name = "lblBillingAmount";
            this.lblBillingAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblBillingAmount.Text = "請求金額";
            this.lblBillingAmount.Top = 1.101181F;
            this.lblBillingAmount.Width = 0.85F;
            // 
            // lblSlipTotal
            // 
            this.lblSlipTotal.Height = 0.4F;
            this.lblSlipTotal.HyperLink = null;
            this.lblSlipTotal.Left = 3.003937F;
            this.lblSlipTotal.Name = "lblSlipTotal";
            this.lblSlipTotal.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblSlipTotal.Text = "伝票合計";
            this.lblSlipTotal.Top = 1.101181F;
            this.lblSlipTotal.Width = 0.85F;
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Height = 0.4F;
            this.lblRemainAmount.HyperLink = null;
            this.lblRemainAmount.Left = 5.553937F;
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblRemainAmount.Text = "残高";
            this.lblRemainAmount.Top = 1.101181F;
            this.lblRemainAmount.Width = 0.85F;
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Height = 0.4F;
            this.lblReceiptAmount.HyperLink = null;
            this.lblReceiptAmount.Left = 3.853937F;
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblReceiptAmount.Text = "入金額";
            this.lblReceiptAmount.Top = 1.101181F;
            this.lblReceiptAmount.Width = 0.85F;
            // 
            // lblMatchingAmount
            // 
            this.lblMatchingAmount.Height = 0.4F;
            this.lblMatchingAmount.HyperLink = null;
            this.lblMatchingAmount.Left = 4.703938F;
            this.lblMatchingAmount.Name = "lblMatchingAmount";
            this.lblMatchingAmount.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMatchingAmount.Text = "消込額";
            this.lblMatchingAmount.Top = 1.101181F;
            this.lblMatchingAmount.Width = 0.85F;
            // 
            // lblCustomerCodeName
            // 
            this.lblCustomerCodeName.Height = 0.4F;
            this.lblCustomerCodeName.HyperLink = null;
            this.lblCustomerCodeName.Left = 6.404094F;
            this.lblCustomerCodeName.Name = "lblCustomerCodeName";
            this.lblCustomerCodeName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblCustomerCodeName.Text = "得意先名";
            this.lblCustomerCodeName.Top = 1.109055F;
            this.lblCustomerCodeName.Width = 0.8400002F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 1.501181F;
            this.lineHeaderHorLower.Width = 7.244095F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 7.244095F;
            this.lineHeaderHorLower.Y1 = 1.501181F;
            this.lineHeaderHorLower.Y2 = 1.501181F;
            // 
            // lineHeaderVerDepartmentCode
            // 
            this.lineHeaderVerDepartmentCode.Height = 0.4F;
            this.lineHeaderVerDepartmentCode.Left = 1.453937F;
            this.lineHeaderVerDepartmentCode.LineWeight = 1F;
            this.lineHeaderVerDepartmentCode.Name = "lineHeaderVerDepartmentCode";
            this.lineHeaderVerDepartmentCode.Top = 1.101362F;
            this.lineHeaderVerDepartmentCode.Width = 0F;
            this.lineHeaderVerDepartmentCode.X1 = 1.453937F;
            this.lineHeaderVerDepartmentCode.X2 = 1.453937F;
            this.lineHeaderVerDepartmentCode.Y1 = 1.101362F;
            this.lineHeaderVerDepartmentCode.Y2 = 1.501362F;
            // 
            // lineHeaderVerInvoiceCode
            // 
            this.lineHeaderVerInvoiceCode.Height = 0.4F;
            this.lineHeaderVerInvoiceCode.Left = 2.153937F;
            this.lineHeaderVerInvoiceCode.LineWeight = 1F;
            this.lineHeaderVerInvoiceCode.Name = "lineHeaderVerInvoiceCode";
            this.lineHeaderVerInvoiceCode.Top = 1.101181F;
            this.lineHeaderVerInvoiceCode.Width = 0F;
            this.lineHeaderVerInvoiceCode.X1 = 2.153937F;
            this.lineHeaderVerInvoiceCode.X2 = 2.153937F;
            this.lineHeaderVerInvoiceCode.Y1 = 1.101181F;
            this.lineHeaderVerInvoiceCode.Y2 = 1.501181F;
            // 
            // lineHeaderVerBillingAmount
            // 
            this.lineHeaderVerBillingAmount.Height = 0.4F;
            this.lineHeaderVerBillingAmount.Left = 3.003937F;
            this.lineHeaderVerBillingAmount.LineWeight = 1F;
            this.lineHeaderVerBillingAmount.Name = "lineHeaderVerBillingAmount";
            this.lineHeaderVerBillingAmount.Top = 1.101181F;
            this.lineHeaderVerBillingAmount.Width = 0F;
            this.lineHeaderVerBillingAmount.X1 = 3.003937F;
            this.lineHeaderVerBillingAmount.X2 = 3.003937F;
            this.lineHeaderVerBillingAmount.Y1 = 1.101181F;
            this.lineHeaderVerBillingAmount.Y2 = 1.501181F;
            // 
            // lineHeaderVerSlipTotal
            // 
            this.lineHeaderVerSlipTotal.Height = 0.4F;
            this.lineHeaderVerSlipTotal.Left = 3.853937F;
            this.lineHeaderVerSlipTotal.LineWeight = 1F;
            this.lineHeaderVerSlipTotal.Name = "lineHeaderVerSlipTotal";
            this.lineHeaderVerSlipTotal.Top = 1.101181F;
            this.lineHeaderVerSlipTotal.Width = 0F;
            this.lineHeaderVerSlipTotal.X1 = 3.853937F;
            this.lineHeaderVerSlipTotal.X2 = 3.853937F;
            this.lineHeaderVerSlipTotal.Y1 = 1.101181F;
            this.lineHeaderVerSlipTotal.Y2 = 1.501181F;
            // 
            // lineHeaderVerReceiptAmount
            // 
            this.lineHeaderVerReceiptAmount.Height = 0.4F;
            this.lineHeaderVerReceiptAmount.Left = 4.703938F;
            this.lineHeaderVerReceiptAmount.LineWeight = 1F;
            this.lineHeaderVerReceiptAmount.Name = "lineHeaderVerReceiptAmount";
            this.lineHeaderVerReceiptAmount.Top = 1.101181F;
            this.lineHeaderVerReceiptAmount.Width = 0F;
            this.lineHeaderVerReceiptAmount.X1 = 4.703938F;
            this.lineHeaderVerReceiptAmount.X2 = 4.703938F;
            this.lineHeaderVerReceiptAmount.Y1 = 1.101181F;
            this.lineHeaderVerReceiptAmount.Y2 = 1.501181F;
            // 
            // lineHeaderVerMatchingAmount
            // 
            this.lineHeaderVerMatchingAmount.Height = 0.4F;
            this.lineHeaderVerMatchingAmount.Left = 5.553937F;
            this.lineHeaderVerMatchingAmount.LineWeight = 1F;
            this.lineHeaderVerMatchingAmount.Name = "lineHeaderVerMatchingAmount";
            this.lineHeaderVerMatchingAmount.Top = 1.101181F;
            this.lineHeaderVerMatchingAmount.Width = 0F;
            this.lineHeaderVerMatchingAmount.X1 = 5.553937F;
            this.lineHeaderVerMatchingAmount.X2 = 5.553937F;
            this.lineHeaderVerMatchingAmount.Y1 = 1.101181F;
            this.lineHeaderVerMatchingAmount.Y2 = 1.501181F;
            // 
            // lineHeaderVerRemainAmount
            // 
            this.lineHeaderVerRemainAmount.Height = 0.4F;
            this.lineHeaderVerRemainAmount.Left = 6.403937F;
            this.lineHeaderVerRemainAmount.LineWeight = 1F;
            this.lineHeaderVerRemainAmount.Name = "lineHeaderVerRemainAmount";
            this.lineHeaderVerRemainAmount.Top = 1.101181F;
            this.lineHeaderVerRemainAmount.Width = 0F;
            this.lineHeaderVerRemainAmount.X1 = 6.403937F;
            this.lineHeaderVerRemainAmount.X2 = 6.403937F;
            this.lineHeaderVerRemainAmount.Y1 = 1.101181F;
            this.lineHeaderVerRemainAmount.Y2 = 1.501181F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.2F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 0F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCustomerCode.Text = "得意先コード   :";
            this.lblCustomerCode.Top = 0.7566929F;
            this.lblCustomerCode.Width = 0.8661418F;
            // 
            // txtTitleCustomerCode
            // 
            this.txtTitleCustomerCode.DataField = "ParentCustomerCode";
            this.txtTitleCustomerCode.Height = 0.2F;
            this.txtTitleCustomerCode.HyperLink = null;
            this.txtTitleCustomerCode.Left = 0.8661418F;
            this.txtTitleCustomerCode.Name = "txtTitleCustomerCode";
            this.txtTitleCustomerCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtTitleCustomerCode.Text = "";
            this.txtTitleCustomerCode.Top = 0.7566929F;
            this.txtTitleCustomerCode.Width = 0.7874014F;
            // 
            // txtTitleCustomerName
            // 
            this.txtTitleCustomerName.DataField = "ParentCustomerName";
            this.txtTitleCustomerName.Height = 0.2F;
            this.txtTitleCustomerName.HyperLink = null;
            this.txtTitleCustomerName.Left = 1.653543F;
            this.txtTitleCustomerName.Name = "txtTitleCustomerName";
            this.txtTitleCustomerName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.txtTitleCustomerName.Text = "";
            this.txtTitleCustomerName.Top = 0.7566929F;
            this.txtTitleCustomerName.Width = 2.283465F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 1.101181F;
            this.lineHeaderHorUpper.Width = 7.244094F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 7.244094F;
            this.lineHeaderHorUpper.Y1 = 1.101181F;
            this.lineHeaderHorUpper.Y2 = 1.101181F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCaption,
            this.spBackColor,
            this.txtMatchingAmount,
            this.txtMatchingSymbolReceipt,
            this.txtBillingAmount,
            this.txtMatchingSymbolBilling,
            this.txtCustomerName,
            this.txtCustomerCode,
            this.txtRemainAmount,
            this.txtSlipTotal,
            this.txtReceiptAmount,
            this.txtCategoryName,
            this.txtRecordedAt,
            this.txtDeptOrSecName,
            this.txtInvoiceCode,
            this.txtDebitAccountTitleName,
            this.lineDetailVerRecordedAt,
            this.lineDetailVerDeptOrSecName,
            this.lineDetailVerInvoiceCode,
            this.lineDetailVerMatchingSymbolBilling,
            this.lineDetailVerSlipTotal,
            this.lineDetailVerReceiptAmount,
            this.lineDetailVerMatchingSymbolReceipt,
            this.lineDetailVerRemainAmount,
            this.lineDetailHorLower});
            this.detail.Height = 0.407874F;
            this.detail.Name = "detail";
            this.detail.Format += new System.EventHandler(this.detail_Format);
            // 
            // txtCaption
            // 
            this.txtCaption.Height = 0.4F;
            this.txtCaption.Left = 0F;
            this.txtCaption.MultiLine = false;
            this.txtCaption.Name = "txtCaption";
            this.txtCaption.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtCaption.Style = "font-size: 6.4pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtCaption.Text = null;
            this.txtCaption.Top = 0F;
            this.txtCaption.Visible = false;
            this.txtCaption.Width = 2.153937F;
            // 
            // spBackColor
            // 
            this.spBackColor.Height = 0.3984252F;
            this.spBackColor.Left = 0F;
            this.spBackColor.LineColor = System.Drawing.Color.Transparent;
            this.spBackColor.LineWeight = 0F;
            this.spBackColor.Name = "spBackColor";
            this.spBackColor.RoundingRadius = new GrapeCity.ActiveReports.Controls.CornersRadius(10F, null, null, null, null);
            this.spBackColor.Top = 0F;
            this.spBackColor.Width = 7.244094F;
            // 
            // txtMatchingAmount
            // 
            this.txtMatchingAmount.Height = 0.4F;
            this.txtMatchingAmount.Left = 4.703938F;
            this.txtMatchingAmount.MultiLine = false;
            this.txtMatchingAmount.Name = "txtMatchingAmount";
            this.txtMatchingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtMatchingAmount.Style = "color: Black; font-size: 6.4pt; text-align: right; vertical-align: middle; ddo-ch" +
    "ar-set: 1";
            this.txtMatchingAmount.Text = "9,999,999,999,999";
            this.txtMatchingAmount.Top = 0F;
            this.txtMatchingAmount.Width = 0.8283467F;
            // 
            // txtMatchingSymbolReceipt
            // 
            this.txtMatchingSymbolReceipt.Height = 0.1574803F;
            this.txtMatchingSymbolReceipt.Left = 4.703938F;
            this.txtMatchingSymbolReceipt.MultiLine = false;
            this.txtMatchingSymbolReceipt.Name = "txtMatchingSymbolReceipt";
            this.txtMatchingSymbolReceipt.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtMatchingSymbolReceipt.Style = "color: Black; font-size: 5pt; text-align: left; vertical-align: top; ddo-char-set" +
    ": 1";
            this.txtMatchingSymbolReceipt.Text = "AB,AC,AD,AE,AF,AE,…";
            this.txtMatchingSymbolReceipt.Top = 0F;
            this.txtMatchingSymbolReceipt.Width = 0.8283467F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.Height = 0.4F;
            this.txtBillingAmount.Left = 2.153937F;
            this.txtBillingAmount.MultiLine = false;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtBillingAmount.Style = "font-size: 6.4pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingAmount.Text = "-9,999,999,999,999";
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 0.85F;
            // 
            // txtMatchingSymbolBilling
            // 
            this.txtMatchingSymbolBilling.Height = 0.1574803F;
            this.txtMatchingSymbolBilling.Left = 2.153937F;
            this.txtMatchingSymbolBilling.MultiLine = false;
            this.txtMatchingSymbolBilling.Name = "txtMatchingSymbolBilling";
            this.txtMatchingSymbolBilling.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtMatchingSymbolBilling.Style = "font-size: 5pt; text-align: left; vertical-align: top; ddo-char-set: 1";
            this.txtMatchingSymbolBilling.Text = "AB,AC,AD,AE,AF,AE,…";
            this.txtMatchingSymbolBilling.Top = 0F;
            this.txtMatchingSymbolBilling.Width = 0.85F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 6.403937F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCustomerName.Style = "font-size: 6.4pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtCustomerName.Text = "txtCustomerName";
            this.txtCustomerName.Top = 0.2F;
            this.txtCustomerName.Width = 0.84F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 6.403937F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCustomerCode.Style = "font-size: 6.4pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtCustomerCode.Text = "0123456789";
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 0.84F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.Height = 0.4F;
            this.txtRemainAmount.Left = 5.553937F;
            this.txtRemainAmount.MultiLine = false;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtRemainAmount.Style = "font-size: 6.4pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtRemainAmount.Text = "9,999,999,999,999";
            this.txtRemainAmount.Top = 0F;
            this.txtRemainAmount.Width = 0.85F;
            // 
            // txtSlipTotal
            // 
            this.txtSlipTotal.Height = 0.4F;
            this.txtSlipTotal.Left = 3.003937F;
            this.txtSlipTotal.MultiLine = false;
            this.txtSlipTotal.Name = "txtSlipTotal";
            this.txtSlipTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtSlipTotal.Style = "font-size: 6.4pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtSlipTotal.Text = "9,999,999,999,999";
            this.txtSlipTotal.Top = 0F;
            this.txtSlipTotal.Width = 0.85F;
            // 
            // txtReceiptAmount
            // 
            this.txtReceiptAmount.Height = 0.4F;
            this.txtReceiptAmount.Left = 3.853937F;
            this.txtReceiptAmount.MultiLine = false;
            this.txtReceiptAmount.Name = "txtReceiptAmount";
            this.txtReceiptAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtReceiptAmount.Style = "color: Black; font-size: 6.4pt; text-align: right; vertical-align: middle; ddo-ch" +
    "ar-set: 1";
            this.txtReceiptAmount.Text = "9,999,999,999,999";
            this.txtReceiptAmount.Top = 0F;
            this.txtReceiptAmount.Width = 0.85F;
            // 
            // txtCategoryName
            // 
            this.txtCategoryName.Height = 0.2F;
            this.txtCategoryName.Left = 0F;
            this.txtCategoryName.MultiLine = false;
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCategoryName.Style = "font-size: 6.4pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtCategoryName.Text = null;
            this.txtCategoryName.Top = 0.2F;
            this.txtCategoryName.Width = 0.653937F;
            // 
            // txtRecordedAt
            // 
            this.txtRecordedAt.Height = 0.2F;
            this.txtRecordedAt.Left = 0F;
            this.txtRecordedAt.MultiLine = false;
            this.txtRecordedAt.Name = "txtRecordedAt";
            this.txtRecordedAt.OutputFormat = resources.GetString("txtRecordedAt.OutputFormat");
            this.txtRecordedAt.Style = "font-size: 6.4pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtRecordedAt.Text = "9999/12/31";
            this.txtRecordedAt.Top = 0F;
            this.txtRecordedAt.Width = 0.653937F;
            // 
            // txtDeptOrSecName
            // 
            this.txtDeptOrSecName.Height = 0.4F;
            this.txtDeptOrSecName.Left = 0.653937F;
            this.txtDeptOrSecName.MultiLine = false;
            this.txtDeptOrSecName.Name = "txtDeptOrSecName";
            this.txtDeptOrSecName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtDeptOrSecName.Style = "font-size: 6.4pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtDeptOrSecName.Text = null;
            this.txtDeptOrSecName.Top = 0F;
            this.txtDeptOrSecName.Width = 0.8F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.Height = 0.2F;
            this.txtInvoiceCode.Left = 1.453937F;
            this.txtInvoiceCode.MultiLine = false;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtInvoiceCode.Style = "font-size: 6.4pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtInvoiceCode.Text = "txtInvoiceCode";
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 0.7F;
            // 
            // txtDebitAccountTitleName
            // 
            this.txtDebitAccountTitleName.Height = 0.2F;
            this.txtDebitAccountTitleName.Left = 1.453937F;
            this.txtDebitAccountTitleName.MultiLine = false;
            this.txtDebitAccountTitleName.Name = "txtDebitAccountTitleName";
            this.txtDebitAccountTitleName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtDebitAccountTitleName.Style = "font-size: 6.4pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtDebitAccountTitleName.Text = "txtDebitAccountTitleName";
            this.txtDebitAccountTitleName.Top = 0.2F;
            this.txtDebitAccountTitleName.Width = 0.7F;
            // 
            // lineDetailVerRecordedAt
            // 
            this.lineDetailVerRecordedAt.Height = 0.4F;
            this.lineDetailVerRecordedAt.Left = 0.653937F;
            this.lineDetailVerRecordedAt.LineWeight = 1F;
            this.lineDetailVerRecordedAt.Name = "lineDetailVerRecordedAt";
            this.lineDetailVerRecordedAt.Top = 0F;
            this.lineDetailVerRecordedAt.Width = 0F;
            this.lineDetailVerRecordedAt.X1 = 0.653937F;
            this.lineDetailVerRecordedAt.X2 = 0.653937F;
            this.lineDetailVerRecordedAt.Y1 = 0F;
            this.lineDetailVerRecordedAt.Y2 = 0.4F;
            // 
            // lineDetailVerDeptOrSecName
            // 
            this.lineDetailVerDeptOrSecName.Height = 0.4F;
            this.lineDetailVerDeptOrSecName.Left = 1.453937F;
            this.lineDetailVerDeptOrSecName.LineWeight = 1F;
            this.lineDetailVerDeptOrSecName.Name = "lineDetailVerDeptOrSecName";
            this.lineDetailVerDeptOrSecName.Top = 0F;
            this.lineDetailVerDeptOrSecName.Width = 0F;
            this.lineDetailVerDeptOrSecName.X1 = 1.453937F;
            this.lineDetailVerDeptOrSecName.X2 = 1.453937F;
            this.lineDetailVerDeptOrSecName.Y1 = 0F;
            this.lineDetailVerDeptOrSecName.Y2 = 0.4F;
            // 
            // lineDetailVerInvoiceCode
            // 
            this.lineDetailVerInvoiceCode.Height = 0.4F;
            this.lineDetailVerInvoiceCode.Left = 2.153937F;
            this.lineDetailVerInvoiceCode.LineWeight = 1F;
            this.lineDetailVerInvoiceCode.Name = "lineDetailVerInvoiceCode";
            this.lineDetailVerInvoiceCode.Top = 0F;
            this.lineDetailVerInvoiceCode.Width = 0F;
            this.lineDetailVerInvoiceCode.X1 = 2.153937F;
            this.lineDetailVerInvoiceCode.X2 = 2.153937F;
            this.lineDetailVerInvoiceCode.Y1 = 0F;
            this.lineDetailVerInvoiceCode.Y2 = 0.4F;
            // 
            // lineDetailVerMatchingSymbolBilling
            // 
            this.lineDetailVerMatchingSymbolBilling.Height = 0.4F;
            this.lineDetailVerMatchingSymbolBilling.Left = 3.003937F;
            this.lineDetailVerMatchingSymbolBilling.LineWeight = 1F;
            this.lineDetailVerMatchingSymbolBilling.Name = "lineDetailVerMatchingSymbolBilling";
            this.lineDetailVerMatchingSymbolBilling.Top = 0F;
            this.lineDetailVerMatchingSymbolBilling.Width = 0F;
            this.lineDetailVerMatchingSymbolBilling.X1 = 3.003937F;
            this.lineDetailVerMatchingSymbolBilling.X2 = 3.003937F;
            this.lineDetailVerMatchingSymbolBilling.Y1 = 0F;
            this.lineDetailVerMatchingSymbolBilling.Y2 = 0.4F;
            // 
            // lineDetailVerSlipTotal
            // 
            this.lineDetailVerSlipTotal.Height = 0.4F;
            this.lineDetailVerSlipTotal.Left = 3.853937F;
            this.lineDetailVerSlipTotal.LineWeight = 1F;
            this.lineDetailVerSlipTotal.Name = "lineDetailVerSlipTotal";
            this.lineDetailVerSlipTotal.Top = 0F;
            this.lineDetailVerSlipTotal.Width = 0F;
            this.lineDetailVerSlipTotal.X1 = 3.853937F;
            this.lineDetailVerSlipTotal.X2 = 3.853937F;
            this.lineDetailVerSlipTotal.Y1 = 0F;
            this.lineDetailVerSlipTotal.Y2 = 0.4F;
            // 
            // lineDetailVerReceiptAmount
            // 
            this.lineDetailVerReceiptAmount.Height = 0.4F;
            this.lineDetailVerReceiptAmount.Left = 4.703938F;
            this.lineDetailVerReceiptAmount.LineWeight = 1F;
            this.lineDetailVerReceiptAmount.Name = "lineDetailVerReceiptAmount";
            this.lineDetailVerReceiptAmount.Top = 0F;
            this.lineDetailVerReceiptAmount.Width = 0F;
            this.lineDetailVerReceiptAmount.X1 = 4.703938F;
            this.lineDetailVerReceiptAmount.X2 = 4.703938F;
            this.lineDetailVerReceiptAmount.Y1 = 0F;
            this.lineDetailVerReceiptAmount.Y2 = 0.4F;
            // 
            // lineDetailVerMatchingSymbolReceipt
            // 
            this.lineDetailVerMatchingSymbolReceipt.Height = 0.4F;
            this.lineDetailVerMatchingSymbolReceipt.Left = 5.553937F;
            this.lineDetailVerMatchingSymbolReceipt.LineWeight = 1F;
            this.lineDetailVerMatchingSymbolReceipt.Name = "lineDetailVerMatchingSymbolReceipt";
            this.lineDetailVerMatchingSymbolReceipt.Top = 0F;
            this.lineDetailVerMatchingSymbolReceipt.Width = 0F;
            this.lineDetailVerMatchingSymbolReceipt.X1 = 5.553937F;
            this.lineDetailVerMatchingSymbolReceipt.X2 = 5.553937F;
            this.lineDetailVerMatchingSymbolReceipt.Y1 = 0F;
            this.lineDetailVerMatchingSymbolReceipt.Y2 = 0.4F;
            // 
            // lineDetailVerRemainAmount
            // 
            this.lineDetailVerRemainAmount.Height = 0.4F;
            this.lineDetailVerRemainAmount.Left = 6.403937F;
            this.lineDetailVerRemainAmount.LineWeight = 1F;
            this.lineDetailVerRemainAmount.Name = "lineDetailVerRemainAmount";
            this.lineDetailVerRemainAmount.Top = 0F;
            this.lineDetailVerRemainAmount.Width = 0F;
            this.lineDetailVerRemainAmount.X1 = 6.403937F;
            this.lineDetailVerRemainAmount.X2 = 6.403937F;
            this.lineDetailVerRemainAmount.Y1 = 0F;
            this.lineDetailVerRemainAmount.Y2 = 0.4F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.4F;
            this.lineDetailHorLower.Width = 7.244094F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 7.244094F;
            this.lineDetailHorLower.Y1 = 0.4F;
            this.lineDetailHorLower.Y2 = 0.4F;
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
            this.reportInfo1.Left = 4.645669F;
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
            this.lblPageNumber.Width = 7.244094F;
            // 
            // ghParentCustomer
            // 
            this.ghParentCustomer.DataField = "ParentCustomerId";
            this.ghParentCustomer.Height = 0F;
            this.ghParentCustomer.Name = "ghParentCustomer";
            this.ghParentCustomer.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            // 
            // gfParentCustomer
            // 
            this.gfParentCustomer.Height = 0F;
            this.gfParentCustomer.Name = "gfParentCustomer";
            // 
            // ghYearMonth
            // 
            this.ghYearMonth.DataField = "YearMonth";
            this.ghYearMonth.Height = 0F;
            this.ghYearMonth.Name = "ghYearMonth";
            this.ghYearMonth.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            // 
            // gfYearMonth
            // 
            this.gfYearMonth.Height = 0F;
            this.gfYearMonth.Name = "gfYearMonth";
            // 
            // CustomerLedgerSectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.DefaultPaperSize = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait;
            this.PageSettings.PaperHeight = 11.69291F;
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.PageSettings.PaperWidth = 8.267716F;
            this.PrintWidth = 7.244094F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.ghParentCustomer);
            this.Sections.Add(this.ghYearMonth);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.gfYearMonth);
            this.Sections.Add(this.gfParentCustomer);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblDebitAccountTitleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSlipTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMatchingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitleCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitleCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCaption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingSymbolReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingSymbolBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlipTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptOrSecName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebitAccountTitleName)).EndInit();
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
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCategoryName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSectionName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDebitAccountTitleName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblSlipTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblMatchingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSlipTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerMatchingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCategoryName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDeptOrSecName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRecordedAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDeptOrSecName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDebitAccountTitleName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMatchingSymbolBilling;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSlipTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSlipTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMatchingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMatchingSymbolBilling;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMatchingSymbolReceipt;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtTitleCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtTitleCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMatchingSymbolReceipt;
        private GrapeCity.ActiveReports.SectionReportModel.Shape spBackColor;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghParentCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfParentCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghYearMonth;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfYearMonth;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCaption;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
    }
}
