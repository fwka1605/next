namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// AllMatchingSectionReport の概要の説明です。
    /// </summary>
    partial class MatchingSequentialReceiptBillingSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MatchingSequentialReceiptBillingSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompanyCodeName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblChecked = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCurrency = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblPayerName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblReceiptCount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingCount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblFee = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDifference = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderVerChecked = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingCount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblCompanyCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSeparate1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerSeparate2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerFee = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCurrency = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerReceiptCount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtFee = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCurrency = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtPayerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtReceiptCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDifference = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerFee = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerReceiptCount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtChecked = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerChecked = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerPayerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCurrency = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingCount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingCount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerCustomerName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.linedetail = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSeparate2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerSeparate1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.hidreportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblChecked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDifference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDifference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChecked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hidreportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompanyCodeName,
            this.lblDate,
            this.ridate,
            this.lblTitle,
            this.lblChecked,
            this.lblCurrency,
            this.lblCustomerName,
            this.lblReceiptAmount,
            this.lblPayerName,
            this.lblReceiptCount,
            this.lblBillingCount,
            this.lblBillingAmount,
            this.lblFee,
            this.lblDifference,
            this.lineHeaderVerChecked,
            this.lineHeaderVerBillingCount,
            this.lblCompanyCode,
            this.lblCustomerCode,
            this.lineHeaderHorUpper,
            this.lineHeaderVerSeparate1,
            this.lineHeaderVerSeparate2,
            this.lineHeaderHorLower,
            this.lineHeaderVerFee,
            this.lineHeaderVerCurrency,
            this.lineHeaderVerPayerName,
            this.lineHeaderVerReceiptCount,
            this.lineHeaderVerCustomerCode,
            this.lineHeaderVerCustomerName,
            this.lineHeaderVerBillingAmount});
            this.pageHeader.Height = 0.8545768F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompanyCodeName
            // 
            this.lblCompanyCodeName.Height = 0.2F;
            this.lblCompanyCodeName.HyperLink = null;
            this.lblCompanyCodeName.Left = 0.8118111F;
            this.lblCompanyCodeName.Name = "lblCompanyCodeName";
            this.lblCompanyCodeName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblCompanyCodeName.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
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
            this.lblDate.Style = "color: Gray; font-size: 7pt; vertical-align: middle";
            this.lblDate.Text = "出力日付　：";
            this.lblDate.Top = 0F;
            this.lblDate.Width = 0.6984252F;
            // 
            // ridate
            // 
            this.ridate.FormatString = "{RunDateTime:yyyy年M月d日}";
            this.ridate.Height = 0.2F;
            this.ridate.Left = 9.522442F;
            this.ridate.Name = "ridate";
            this.ridate.Style = "color: Gray; font-size: 7pt; text-align: left; vertical-align: middle";
            this.ridate.Top = 0F;
            this.ridate.Width = 1.015F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "一括消込チェックリスト";
            this.lblTitle.Top = 0.2704725F;
            this.lblTitle.Width = 10.62992F;
            // 
            // lblChecked
            // 
            this.lblChecked.Height = 0.2F;
            this.lblChecked.HyperLink = null;
            this.lblChecked.Left = 0F;
            this.lblChecked.Name = "lblChecked";
            this.lblChecked.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblChecked.Text = "一括";
            this.lblChecked.Top = 0.65F;
            this.lblChecked.Width = 0.4F;
            // 
            // lblCurrency
            // 
            this.lblCurrency.Height = 0.2F;
            this.lblCurrency.HyperLink = null;
            this.lblCurrency.Left = 0.4F;
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCurrency.Text = "通貨";
            this.lblCurrency.Top = 0.65F;
            this.lblCurrency.Width = 0.45F;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Height = 0.2F;
            this.lblCustomerName.HyperLink = null;
            this.lblCustomerName.Left = 4.9F;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerName.Text = "得意先名（代表者）";
            this.lblCustomerName.Top = 0.65F;
            this.lblCustomerName.Width = 2F;
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Height = 0.2F;
            this.lblReceiptAmount.HyperLink = null;
            this.lblReceiptAmount.Left = 2.8F;
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptAmount.Text = "金額";
            this.lblReceiptAmount.Top = 0.65F;
            this.lblReceiptAmount.Width = 1.2F;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Height = 0.2F;
            this.lblPayerName.HyperLink = null;
            this.lblPayerName.Left = 0.85F;
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.Top = 0.65F;
            this.lblPayerName.Width = 1.5F;
            // 
            // lblReceiptCount
            // 
            this.lblReceiptCount.Height = 0.2F;
            this.lblReceiptCount.HyperLink = null;
            this.lblReceiptCount.Left = 2.35F;
            this.lblReceiptCount.Name = "lblReceiptCount";
            this.lblReceiptCount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblReceiptCount.Text = "件数";
            this.lblReceiptCount.Top = 0.65F;
            this.lblReceiptCount.Width = 0.45F;
            // 
            // lblBillingCount
            // 
            this.lblBillingCount.Height = 0.2F;
            this.lblBillingCount.HyperLink = null;
            this.lblBillingCount.Left = 6.9F;
            this.lblBillingCount.Name = "lblBillingCount";
            this.lblBillingCount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingCount.Text = "件数";
            this.lblBillingCount.Top = 0.65F;
            this.lblBillingCount.Width = 0.45F;
            // 
            // lblBillingAmount
            // 
            this.lblBillingAmount.Height = 0.2F;
            this.lblBillingAmount.HyperLink = null;
            this.lblBillingAmount.Left = 7.35F;
            this.lblBillingAmount.Name = "lblBillingAmount";
            this.lblBillingAmount.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblBillingAmount.Text = "金額";
            this.lblBillingAmount.Top = 0.65F;
            this.lblBillingAmount.Width = 1.2F;
            // 
            // lblFee
            // 
            this.lblFee.Height = 0.2F;
            this.lblFee.HyperLink = null;
            this.lblFee.Left = 8.55F;
            this.lblFee.Name = "lblFee";
            this.lblFee.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblFee.Text = "手数料負担";
            this.lblFee.Top = 0.65F;
            this.lblFee.Width = 0.9F;
            // 
            // lblDifference
            // 
            this.lblDifference.Height = 0.2F;
            this.lblDifference.HyperLink = null;
            this.lblDifference.Left = 9.45F;
            this.lblDifference.Name = "lblDifference";
            this.lblDifference.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblDifference.Text = "差額";
            this.lblDifference.Top = 0.65F;
            this.lblDifference.Width = 1.18F;
            // 
            // lineHeaderVerChecked
            // 
            this.lineHeaderVerChecked.Height = 0.2F;
            this.lineHeaderVerChecked.Left = 0.4F;
            this.lineHeaderVerChecked.LineWeight = 1F;
            this.lineHeaderVerChecked.Name = "lineHeaderVerChecked";
            this.lineHeaderVerChecked.Top = 0.65F;
            this.lineHeaderVerChecked.Width = 0F;
            this.lineHeaderVerChecked.X1 = 0.4F;
            this.lineHeaderVerChecked.X2 = 0.4F;
            this.lineHeaderVerChecked.Y1 = 0.85F;
            this.lineHeaderVerChecked.Y2 = 0.65F;
            // 
            // lineHeaderVerBillingCount
            // 
            this.lineHeaderVerBillingCount.Height = 0.2F;
            this.lineHeaderVerBillingCount.Left = 7.35F;
            this.lineHeaderVerBillingCount.LineWeight = 1F;
            this.lineHeaderVerBillingCount.Name = "lineHeaderVerBillingCount";
            this.lineHeaderVerBillingCount.Top = 0.65F;
            this.lineHeaderVerBillingCount.Width = 0F;
            this.lineHeaderVerBillingCount.X1 = 7.35F;
            this.lineHeaderVerBillingCount.X2 = 7.35F;
            this.lineHeaderVerBillingCount.Y1 = 0.85F;
            this.lineHeaderVerBillingCount.Y2 = 0.65F;
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
            this.lblCompanyCode.Width = 0.7874016F;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Height = 0.2F;
            this.lblCustomerCode.HyperLink = null;
            this.lblCustomerCode.Left = 4F;
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.Top = 0.65F;
            this.lblCustomerCode.Width = 0.9F;
            // 
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.65F;
            this.lineHeaderHorUpper.Width = 10.63F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.63F;
            this.lineHeaderHorUpper.Y1 = 0.65F;
            this.lineHeaderHorUpper.Y2 = 0.65F;
            // 
            // lineHeaderVerSeparate1
            // 
            this.lineHeaderVerSeparate1.Height = 0.2F;
            this.lineHeaderVerSeparate1.Left = 4F;
            this.lineHeaderVerSeparate1.LineWeight = 1F;
            this.lineHeaderVerSeparate1.Name = "lineHeaderVerSeparate1";
            this.lineHeaderVerSeparate1.Top = 0.65F;
            this.lineHeaderVerSeparate1.Width = 0F;
            this.lineHeaderVerSeparate1.X1 = 4F;
            this.lineHeaderVerSeparate1.X2 = 4F;
            this.lineHeaderVerSeparate1.Y1 = 0.85F;
            this.lineHeaderVerSeparate1.Y2 = 0.65F;
            // 
            // lineHeaderVerSeparate2
            // 
            this.lineHeaderVerSeparate2.Height = 0.2F;
            this.lineHeaderVerSeparate2.Left = 4.03F;
            this.lineHeaderVerSeparate2.LineWeight = 1F;
            this.lineHeaderVerSeparate2.Name = "lineHeaderVerSeparate2";
            this.lineHeaderVerSeparate2.Top = 0.65F;
            this.lineHeaderVerSeparate2.Width = 0F;
            this.lineHeaderVerSeparate2.X1 = 4.03F;
            this.lineHeaderVerSeparate2.X2 = 4.03F;
            this.lineHeaderVerSeparate2.Y1 = 0.85F;
            this.lineHeaderVerSeparate2.Y2 = 0.65F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 0.85F;
            this.lineHeaderHorLower.Width = 10.63F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.63F;
            this.lineHeaderHorLower.Y1 = 0.85F;
            this.lineHeaderHorLower.Y2 = 0.85F;
            // 
            // lineHeaderVerFee
            // 
            this.lineHeaderVerFee.Height = 0.2F;
            this.lineHeaderVerFee.Left = 9.45F;
            this.lineHeaderVerFee.LineWeight = 1F;
            this.lineHeaderVerFee.Name = "lineHeaderVerFee";
            this.lineHeaderVerFee.Top = 0.65F;
            this.lineHeaderVerFee.Width = 0F;
            this.lineHeaderVerFee.X1 = 9.45F;
            this.lineHeaderVerFee.X2 = 9.45F;
            this.lineHeaderVerFee.Y1 = 0.85F;
            this.lineHeaderVerFee.Y2 = 0.65F;
            // 
            // lineHeaderVerCurrency
            // 
            this.lineHeaderVerCurrency.Height = 0.2F;
            this.lineHeaderVerCurrency.Left = 0.85F;
            this.lineHeaderVerCurrency.LineWeight = 1F;
            this.lineHeaderVerCurrency.Name = "lineHeaderVerCurrency";
            this.lineHeaderVerCurrency.Top = 0.65F;
            this.lineHeaderVerCurrency.Width = 0F;
            this.lineHeaderVerCurrency.X1 = 0.85F;
            this.lineHeaderVerCurrency.X2 = 0.85F;
            this.lineHeaderVerCurrency.Y1 = 0.85F;
            this.lineHeaderVerCurrency.Y2 = 0.65F;
            // 
            // lineHeaderVerPayerName
            // 
            this.lineHeaderVerPayerName.Height = 0.2F;
            this.lineHeaderVerPayerName.Left = 2.35F;
            this.lineHeaderVerPayerName.LineWeight = 1F;
            this.lineHeaderVerPayerName.Name = "lineHeaderVerPayerName";
            this.lineHeaderVerPayerName.Top = 0.65F;
            this.lineHeaderVerPayerName.Width = 0F;
            this.lineHeaderVerPayerName.X1 = 2.35F;
            this.lineHeaderVerPayerName.X2 = 2.35F;
            this.lineHeaderVerPayerName.Y1 = 0.85F;
            this.lineHeaderVerPayerName.Y2 = 0.65F;
            // 
            // lineHeaderVerReceiptCount
            // 
            this.lineHeaderVerReceiptCount.Height = 0.2F;
            this.lineHeaderVerReceiptCount.Left = 2.8F;
            this.lineHeaderVerReceiptCount.LineWeight = 1F;
            this.lineHeaderVerReceiptCount.Name = "lineHeaderVerReceiptCount";
            this.lineHeaderVerReceiptCount.Top = 0.65F;
            this.lineHeaderVerReceiptCount.Width = 0F;
            this.lineHeaderVerReceiptCount.X1 = 2.8F;
            this.lineHeaderVerReceiptCount.X2 = 2.8F;
            this.lineHeaderVerReceiptCount.Y1 = 0.85F;
            this.lineHeaderVerReceiptCount.Y2 = 0.65F;
            // 
            // lineHeaderVerCustomerCode
            // 
            this.lineHeaderVerCustomerCode.Height = 0.2F;
            this.lineHeaderVerCustomerCode.Left = 4.9F;
            this.lineHeaderVerCustomerCode.LineWeight = 1F;
            this.lineHeaderVerCustomerCode.Name = "lineHeaderVerCustomerCode";
            this.lineHeaderVerCustomerCode.Top = 0.65F;
            this.lineHeaderVerCustomerCode.Width = 0F;
            this.lineHeaderVerCustomerCode.X1 = 4.9F;
            this.lineHeaderVerCustomerCode.X2 = 4.9F;
            this.lineHeaderVerCustomerCode.Y1 = 0.85F;
            this.lineHeaderVerCustomerCode.Y2 = 0.65F;
            // 
            // lineHeaderVerCustomerName
            // 
            this.lineHeaderVerCustomerName.Height = 0.2F;
            this.lineHeaderVerCustomerName.Left = 6.9F;
            this.lineHeaderVerCustomerName.LineWeight = 1F;
            this.lineHeaderVerCustomerName.Name = "lineHeaderVerCustomerName";
            this.lineHeaderVerCustomerName.Top = 0.65F;
            this.lineHeaderVerCustomerName.Width = 0F;
            this.lineHeaderVerCustomerName.X1 = 6.9F;
            this.lineHeaderVerCustomerName.X2 = 6.9F;
            this.lineHeaderVerCustomerName.Y1 = 0.85F;
            this.lineHeaderVerCustomerName.Y2 = 0.65F;
            // 
            // lineHeaderVerBillingAmount
            // 
            this.lineHeaderVerBillingAmount.Height = 0.2F;
            this.lineHeaderVerBillingAmount.Left = 8.55F;
            this.lineHeaderVerBillingAmount.LineWeight = 1F;
            this.lineHeaderVerBillingAmount.Name = "lineHeaderVerBillingAmount";
            this.lineHeaderVerBillingAmount.Top = 0.65F;
            this.lineHeaderVerBillingAmount.Width = 0F;
            this.lineHeaderVerBillingAmount.X1 = 8.55F;
            this.lineHeaderVerBillingAmount.X2 = 8.55F;
            this.lineHeaderVerBillingAmount.Y1 = 0.85F;
            this.lineHeaderVerBillingAmount.Y2 = 0.65F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCustomerCode,
            this.txtFee,
            this.txtCurrency,
            this.txtReceiptAmount,
            this.txtPayerName,
            this.txtReceiptCount,
            this.txtDifference,
            this.lineDetailVerFee,
            this.lineDetailVerReceiptCount,
            this.txtChecked,
            this.lineDetailVerChecked,
            this.lineDetailVerPayerName,
            this.lineDetailVerCurrency,
            this.txtCustomerName,
            this.txtBillingCount,
            this.txtBillingAmount,
            this.lineDetailVerCustomerCode,
            this.lineDetailVerBillingCount,
            this.lineDetailVerCustomerName,
            this.linedetail,
            this.lineDetailVerBillingAmount,
            this.lineDetailVerSeparate2,
            this.lineDetailVerSeparate1});
            this.detail.Height = 0.2F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 4.03F;
            this.txtCustomerCode.MultiLine = false;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 0.8699999F;
            // 
            // txtFee
            // 
            this.txtFee.Height = 0.2F;
            this.txtFee.Left = 8.55F;
            this.txtFee.MultiLine = false;
            this.txtFee.Name = "txtFee";
            this.txtFee.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtFee.Text = null;
            this.txtFee.Top = 0F;
            this.txtFee.Width = 0.9000006F;
            // 
            // txtCurrency
            // 
            this.txtCurrency.Height = 0.2F;
            this.txtCurrency.Left = 0.4F;
            this.txtCurrency.MultiLine = false;
            this.txtCurrency.Name = "txtCurrency";
            this.txtCurrency.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtCurrency.Text = null;
            this.txtCurrency.Top = 0F;
            this.txtCurrency.Width = 0.45F;
            // 
            // txtReceiptAmount
            // 
            this.txtReceiptAmount.Height = 0.2F;
            this.txtReceiptAmount.Left = 2.8F;
            this.txtReceiptAmount.MultiLine = false;
            this.txtReceiptAmount.Name = "txtReceiptAmount";
            this.txtReceiptAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptAmount.Text = null;
            this.txtReceiptAmount.Top = 0F;
            this.txtReceiptAmount.Width = 1.18F;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Height = 0.2F;
            this.txtPayerName.Left = 0.8700001F;
            this.txtPayerName.MultiLine = false;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtPayerName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtPayerName.Text = null;
            this.txtPayerName.Top = 0F;
            this.txtPayerName.Width = 1.48F;
            // 
            // txtReceiptCount
            // 
            this.txtReceiptCount.Height = 0.2F;
            this.txtReceiptCount.Left = 2.35F;
            this.txtReceiptCount.MultiLine = false;
            this.txtReceiptCount.Name = "txtReceiptCount";
            this.txtReceiptCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtReceiptCount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtReceiptCount.Text = null;
            this.txtReceiptCount.Top = 0F;
            this.txtReceiptCount.Width = 0.45F;
            // 
            // txtDifference
            // 
            this.txtDifference.Height = 0.2F;
            this.txtDifference.Left = 9.45F;
            this.txtDifference.MultiLine = false;
            this.txtDifference.Name = "txtDifference";
            this.txtDifference.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtDifference.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtDifference.Text = null;
            this.txtDifference.Top = 0F;
            this.txtDifference.Width = 1.18F;
            // 
            // lineDetailVerFee
            // 
            this.lineDetailVerFee.Height = 0.1998504F;
            this.lineDetailVerFee.Left = 9.45F;
            this.lineDetailVerFee.LineWeight = 1F;
            this.lineDetailVerFee.Name = "lineDetailVerFee";
            this.lineDetailVerFee.Top = 0F;
            this.lineDetailVerFee.Width = 0F;
            this.lineDetailVerFee.X1 = 9.45F;
            this.lineDetailVerFee.X2 = 9.45F;
            this.lineDetailVerFee.Y1 = 0F;
            this.lineDetailVerFee.Y2 = 0.1998504F;
            // 
            // lineDetailVerReceiptCount
            // 
            this.lineDetailVerReceiptCount.Height = 0.2F;
            this.lineDetailVerReceiptCount.Left = 2.8F;
            this.lineDetailVerReceiptCount.LineWeight = 1F;
            this.lineDetailVerReceiptCount.Name = "lineDetailVerReceiptCount";
            this.lineDetailVerReceiptCount.Top = 0F;
            this.lineDetailVerReceiptCount.Width = 0F;
            this.lineDetailVerReceiptCount.X1 = 2.8F;
            this.lineDetailVerReceiptCount.X2 = 2.8F;
            this.lineDetailVerReceiptCount.Y1 = 0F;
            this.lineDetailVerReceiptCount.Y2 = 0.2F;
            // 
            // txtChecked
            // 
            this.txtChecked.Height = 0.2F;
            this.txtChecked.Left = 7.450581E-09F;
            this.txtChecked.MultiLine = false;
            this.txtChecked.Name = "txtChecked";
            this.txtChecked.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtChecked.Text = null;
            this.txtChecked.Top = 0F;
            this.txtChecked.Width = 0.4F;
            // 
            // lineDetailVerChecked
            // 
            this.lineDetailVerChecked.Height = 0.2F;
            this.lineDetailVerChecked.Left = 0.4F;
            this.lineDetailVerChecked.LineWeight = 1F;
            this.lineDetailVerChecked.Name = "lineDetailVerChecked";
            this.lineDetailVerChecked.Top = 0F;
            this.lineDetailVerChecked.Width = 0F;
            this.lineDetailVerChecked.X1 = 0.4F;
            this.lineDetailVerChecked.X2 = 0.4F;
            this.lineDetailVerChecked.Y1 = 0F;
            this.lineDetailVerChecked.Y2 = 0.2F;
            // 
            // lineDetailVerPayerName
            // 
            this.lineDetailVerPayerName.Height = 0.2F;
            this.lineDetailVerPayerName.Left = 2.35F;
            this.lineDetailVerPayerName.LineWeight = 1F;
            this.lineDetailVerPayerName.Name = "lineDetailVerPayerName";
            this.lineDetailVerPayerName.Top = 0F;
            this.lineDetailVerPayerName.Width = 0F;
            this.lineDetailVerPayerName.X1 = 2.35F;
            this.lineDetailVerPayerName.X2 = 2.35F;
            this.lineDetailVerPayerName.Y1 = 0F;
            this.lineDetailVerPayerName.Y2 = 0.2F;
            // 
            // lineDetailVerCurrency
            // 
            this.lineDetailVerCurrency.Height = 0.2F;
            this.lineDetailVerCurrency.Left = 0.85F;
            this.lineDetailVerCurrency.LineWeight = 1F;
            this.lineDetailVerCurrency.Name = "lineDetailVerCurrency";
            this.lineDetailVerCurrency.Top = 0F;
            this.lineDetailVerCurrency.Width = 0F;
            this.lineDetailVerCurrency.X1 = 0.85F;
            this.lineDetailVerCurrency.X2 = 0.85F;
            this.lineDetailVerCurrency.Y1 = 0F;
            this.lineDetailVerCurrency.Y2 = 0.2F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 4.92F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerName.Style = "font-size: 6pt; vertical-align: middle; ddo-char-set: 1";
            this.txtCustomerName.Text = null;
            this.txtCustomerName.Top = 0F;
            this.txtCustomerName.Width = 1.98F;
            // 
            // txtBillingCount
            // 
            this.txtBillingCount.Height = 0.2F;
            this.txtBillingCount.Left = 6.9F;
            this.txtBillingCount.MultiLine = false;
            this.txtBillingCount.Name = "txtBillingCount";
            this.txtBillingCount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingCount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingCount.Text = null;
            this.txtBillingCount.Top = 0F;
            this.txtBillingCount.Width = 0.45F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.Height = 0.2F;
            this.txtBillingAmount.Left = 7.35F;
            this.txtBillingAmount.MultiLine = false;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtBillingAmount.Text = null;
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 1.18F;
            // 
            // lineDetailVerCustomerCode
            // 
            this.lineDetailVerCustomerCode.Height = 0.2F;
            this.lineDetailVerCustomerCode.Left = 4.9F;
            this.lineDetailVerCustomerCode.LineWeight = 1F;
            this.lineDetailVerCustomerCode.Name = "lineDetailVerCustomerCode";
            this.lineDetailVerCustomerCode.Top = 0F;
            this.lineDetailVerCustomerCode.Width = 0F;
            this.lineDetailVerCustomerCode.X1 = 4.9F;
            this.lineDetailVerCustomerCode.X2 = 4.9F;
            this.lineDetailVerCustomerCode.Y1 = 0F;
            this.lineDetailVerCustomerCode.Y2 = 0.2F;
            // 
            // lineDetailVerBillingCount
            // 
            this.lineDetailVerBillingCount.Height = 0.2F;
            this.lineDetailVerBillingCount.Left = 7.35F;
            this.lineDetailVerBillingCount.LineWeight = 1F;
            this.lineDetailVerBillingCount.Name = "lineDetailVerBillingCount";
            this.lineDetailVerBillingCount.Top = 0F;
            this.lineDetailVerBillingCount.Width = 0F;
            this.lineDetailVerBillingCount.X1 = 7.35F;
            this.lineDetailVerBillingCount.X2 = 7.35F;
            this.lineDetailVerBillingCount.Y1 = 0F;
            this.lineDetailVerBillingCount.Y2 = 0.2F;
            // 
            // lineDetailVerCustomerName
            // 
            this.lineDetailVerCustomerName.Height = 0.1968504F;
            this.lineDetailVerCustomerName.Left = 6.9F;
            this.lineDetailVerCustomerName.LineWeight = 1F;
            this.lineDetailVerCustomerName.Name = "lineDetailVerCustomerName";
            this.lineDetailVerCustomerName.Top = 0F;
            this.lineDetailVerCustomerName.Width = 0F;
            this.lineDetailVerCustomerName.X1 = 6.9F;
            this.lineDetailVerCustomerName.X2 = 6.9F;
            this.lineDetailVerCustomerName.Y1 = 0F;
            this.lineDetailVerCustomerName.Y2 = 0.1968504F;
            // 
            // linedetail
            // 
            this.linedetail.Height = 0F;
            this.linedetail.Left = 0F;
            this.linedetail.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.linedetail.LineWeight = 1F;
            this.linedetail.Name = "linedetail";
            this.linedetail.Top = 0.2F;
            this.linedetail.Width = 10.63F;
            this.linedetail.X1 = 0F;
            this.linedetail.X2 = 10.63F;
            this.linedetail.Y1 = 0.2F;
            this.linedetail.Y2 = 0.2F;
            // 
            // lineDetailVerBillingAmount
            // 
            this.lineDetailVerBillingAmount.Height = 0.2F;
            this.lineDetailVerBillingAmount.Left = 8.55F;
            this.lineDetailVerBillingAmount.LineWeight = 1F;
            this.lineDetailVerBillingAmount.Name = "lineDetailVerBillingAmount";
            this.lineDetailVerBillingAmount.Top = 0F;
            this.lineDetailVerBillingAmount.Width = 0F;
            this.lineDetailVerBillingAmount.X1 = 8.55F;
            this.lineDetailVerBillingAmount.X2 = 8.55F;
            this.lineDetailVerBillingAmount.Y1 = 0F;
            this.lineDetailVerBillingAmount.Y2 = 0.2F;
            // 
            // lineDetailVerSeparate2
            // 
            this.lineDetailVerSeparate2.Height = 0.2F;
            this.lineDetailVerSeparate2.Left = 4F;
            this.lineDetailVerSeparate2.LineWeight = 1F;
            this.lineDetailVerSeparate2.Name = "lineDetailVerSeparate2";
            this.lineDetailVerSeparate2.Top = 0F;
            this.lineDetailVerSeparate2.Width = 0F;
            this.lineDetailVerSeparate2.X1 = 4F;
            this.lineDetailVerSeparate2.X2 = 4F;
            this.lineDetailVerSeparate2.Y1 = 0F;
            this.lineDetailVerSeparate2.Y2 = 0.2F;
            // 
            // lineDetailVerSeparate1
            // 
            this.lineDetailVerSeparate1.Height = 0.2F;
            this.lineDetailVerSeparate1.Left = 4.03F;
            this.lineDetailVerSeparate1.LineWeight = 1F;
            this.lineDetailVerSeparate1.Name = "lineDetailVerSeparate1";
            this.lineDetailVerSeparate1.Top = 0F;
            this.lineDetailVerSeparate1.Width = 0F;
            this.lineDetailVerSeparate1.X1 = 4.03F;
            this.lineDetailVerSeparate1.X2 = 4.03F;
            this.lineDetailVerSeparate1.Y1 = 0F;
            this.lineDetailVerSeparate1.Y2 = 0.2F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.hidreportInfo1,
            this.lblPageNumber});
            this.pageFooter.Height = 0.3149606F;
            this.pageFooter.Name = "pageFooter";
            this.pageFooter.BeforePrint += new System.EventHandler(this.pageFooter_BeforePrint);
            // 
            // hidreportInfo1
            // 
            this.hidreportInfo1.FormatString = "{PageNumber} / {PageCount}";
            this.hidreportInfo1.Height = 0.2F;
            this.hidreportInfo1.Left = 9.448819F;
            this.hidreportInfo1.Name = "hidreportInfo1";
            this.hidreportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center";
            this.hidreportInfo1.Top = 0F;
            this.hidreportInfo1.Visible = false;
            this.hidreportInfo1.Width = 1.181102F;
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
            // MatchingSequentialReceiptBillingSectionReport
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
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-family: \"ＭＳ 明朝\"; font-size: 9pt; ddo-char-set: 186", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            this.ReportStart += new System.EventHandler(this.AllMatchingSectionReport_ReportStart);
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCodeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblChecked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDifference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompanyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceiptCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDifference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChecked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hidreportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCodeName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblChecked;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCurrency;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblReceiptCount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingCount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblFee;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDifference;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerFee;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerChecked;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingCount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtFee;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCurrency;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingCount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtReceiptCount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDifference;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerBillingCount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSeparate2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerSeparate1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerReceiptCount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtChecked;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerChecked;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompanyCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSeparate1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerSeparate2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerFee;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCurrency;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerReceiptCount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerPayerName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCurrency;
        private GrapeCity.ActiveReports.SectionReportModel.Line linedetail;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo hidreportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerCustomerName;
    }
}
