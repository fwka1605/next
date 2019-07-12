namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// BillSectionReport の概要の説明です。
    /// </summary>
    partial class BillingServiceSearchSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BillingServiceSearchSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblcompanycode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lbltitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label2 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label3 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label4 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label5 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label6 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label7 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label10 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label11 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label12 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label13 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label14 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label16 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label17 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label18 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label19 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line10 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line11 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line13 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.label8 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label9 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line24 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line25 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line41 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRemainAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCategoryCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingID = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBilledAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtClosingAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInvoiceCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSalesAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAssignmentFlag = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtInputType = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtNote1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line26 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line15 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line30 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line17 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line18 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line19 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line21 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line20 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line23 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line14 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.ghGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.gfGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtBillingGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.label23 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line34 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtRemainGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.label24 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line16 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line35 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line39 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.ghDepartmentTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.gfDepartmentTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtBillingDepartmentTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.label22 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line32 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtRemainDepartmentTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.label25 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line33 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line38 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.ghStaffTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.gfStaffTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.txtBillingStaffTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.label21 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line31 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtRemainStaffTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.label26 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line22 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line28 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line37 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtBillingCustomerTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.ghCustomerTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.gfCustomerTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.label20 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line27 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtRemainCustomerTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.label27 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line29 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line36 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAssignmentFlag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingGrandTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainGrandTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDepartmentTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainDepartmentTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingStaffTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainStaffTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCustomerTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainCustomerTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.label1,
            this.lblcompanycode,
            this.lbldate,
            this.ridate,
            this.lbltitle,
            this.label2,
            this.label3,
            this.label4,
            this.label5,
            this.label6,
            this.label7,
            this.label10,
            this.label11,
            this.label12,
            this.label13,
            this.label14,
            this.lblNote1,
            this.label16,
            this.label17,
            this.label18,
            this.label19,
            this.line6,
            this.line7,
            this.line8,
            this.line9,
            this.line10,
            this.line11,
            this.line12,
            this.line13,
            this.label8,
            this.label9,
            this.line24,
            this.line1,
            this.line2,
            this.line25,
            this.line41});
            this.pageHeader.Height = 1.108917F;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.BeforePrint += new System.EventHandler(this.pageHeader_BeforePrint);
            // 
            // label1
            // 
            this.label1.Height = 0.2F;
            this.label1.HyperLink = null;
            this.label1.Left = 0.02440945F;
            this.label1.Name = "label1";
            this.label1.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.label1.Text = "会社コード　：";
            this.label1.Top = 0F;
            this.label1.Width = 0.726189F;
            // 
            // lblcompanycode
            // 
            this.lblcompanycode.Height = 0.2F;
            this.lblcompanycode.HyperLink = null;
            this.lblcompanycode.Left = 0.811811F;
            this.lblcompanycode.Name = "lblcompanycode";
            this.lblcompanycode.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblcompanycode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblcompanycode.Text = "label2";
            this.lblcompanycode.Top = 0F;
            this.lblcompanycode.Width = 3.657F;
            // 
            // lbldate
            // 
            this.lbldate.Height = 0.2F;
            this.lbldate.HyperLink = null;
            this.lbldate.Left = 8.809055F;
            this.lbldate.Name = "lbldate";
            this.lbldate.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lbldate.Text = "出力日付　：";
            this.lbldate.Top = 0F;
            this.lbldate.Width = 0.614173F;
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
            this.ridate.Width = 0.8669291F;
            // 
            // lbltitle
            // 
            this.lbltitle.Height = 0.2311024F;
            this.lbltitle.HyperLink = null;
            this.lbltitle.Left = 0F;
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lbltitle.Text = "請求データ一覧";
            this.lbltitle.Top = 0.2704724F;
            this.lbltitle.Width = 10.553F;
            // 
            // label2
            // 
            this.label2.Height = 0.2F;
            this.label2.HyperLink = null;
            this.label2.Left = 0.7F;
            this.label2.Name = "label2";
            this.label2.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label2.Text = "得意先コード";
            this.label2.Top = 0.7F;
            this.label2.Width = 1.8F;
            // 
            // label3
            // 
            this.label3.Height = 0.2F;
            this.label3.HyperLink = null;
            this.label3.Left = 0.7F;
            this.label3.Name = "label3";
            this.label3.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label3.Text = "得意先名";
            this.label3.Top = 0.9F;
            this.label3.Width = 1.8F;
            // 
            // label4
            // 
            this.label4.Height = 0.2F;
            this.label4.HyperLink = null;
            this.label4.Left = 2.5F;
            this.label4.Name = "label4";
            this.label4.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label4.Text = "請求日";
            this.label4.Top = 0.7F;
            this.label4.Width = 0.55F;
            // 
            // label5
            // 
            this.label5.Height = 0.2F;
            this.label5.HyperLink = null;
            this.label5.Left = 2.5F;
            this.label5.Name = "label5";
            this.label5.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label5.Text = "売上日";
            this.label5.Top = 0.9F;
            this.label5.Width = 0.55F;
            // 
            // label6
            // 
            this.label6.Height = 0.2F;
            this.label6.HyperLink = null;
            this.label6.Left = 3.05F;
            this.label6.Name = "label6";
            this.label6.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label6.Text = "請求締日";
            this.label6.Top = 0.7F;
            this.label6.Width = 0.55F;
            // 
            // label7
            // 
            this.label7.Height = 0.2F;
            this.label7.HyperLink = null;
            this.label7.Left = 3.05F;
            this.label7.Name = "label7";
            this.label7.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label7.Text = "入金予定日";
            this.label7.Top = 0.9F;
            this.label7.Width = 0.55F;
            // 
            // label10
            // 
            this.label10.Height = 0.4F;
            this.label10.HyperLink = null;
            this.label10.Left = 3.6F;
            this.label10.Name = "label10";
            this.label10.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label10.Text = "請求金額（税込）";
            this.label10.Top = 0.7F;
            this.label10.Width = 1F;
            // 
            // label11
            // 
            this.label11.Height = 0.4F;
            this.label11.HyperLink = null;
            this.label11.Left = 4.6F;
            this.label11.Name = "label11";
            this.label11.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label11.Text = "請求残";
            this.label11.Top = 0.7F;
            this.label11.Width = 1F;
            // 
            // label12
            // 
            this.label12.Height = 0.2F;
            this.label12.HyperLink = null;
            this.label12.Left = 5.6F;
            this.label12.Name = "label12";
            this.label12.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label12.Text = "請求区分";
            this.label12.Top = 0.7F;
            this.label12.Width = 1F;
            // 
            // label13
            // 
            this.label13.Height = 0.2F;
            this.label13.HyperLink = null;
            this.label13.Left = 5.6F;
            this.label13.Name = "label13";
            this.label13.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label13.Text = "入力区分";
            this.label13.Top = 0.9F;
            this.label13.Width = 1F;
            // 
            // label14
            // 
            this.label14.Height = 0.2F;
            this.label14.HyperLink = null;
            this.label14.Left = 6.6F;
            this.label14.Name = "label14";
            this.label14.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label14.Text = "請求書番号";
            this.label14.Top = 0.7F;
            this.label14.Width = 1.770001F;
            // 
            // lblNote1
            // 
            this.lblNote1.Height = 0.2F;
            this.lblNote1.HyperLink = null;
            this.lblNote1.Left = 6.6F;
            this.lblNote1.Name = "lblNote1";
            this.lblNote1.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.lblNote1.Text = "備考";
            this.lblNote1.Top = 0.9F;
            this.lblNote1.Width = 1.770001F;
            // 
            // label16
            // 
            this.label16.Height = 0.2F;
            this.label16.HyperLink = null;
            this.label16.Left = 8.370001F;
            this.label16.Name = "label16";
            this.label16.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label16.Text = "請求部門コード";
            this.label16.Top = 0.7F;
            this.label16.Width = 1.1F;
            // 
            // label17
            // 
            this.label17.Height = 0.2F;
            this.label17.HyperLink = null;
            this.label17.Left = 8.370001F;
            this.label17.Name = "label17";
            this.label17.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label17.Text = "請求部門名";
            this.label17.Top = 0.9F;
            this.label17.Width = 1.1F;
            // 
            // label18
            // 
            this.label18.Height = 0.2F;
            this.label18.HyperLink = null;
            this.label18.Left = 9.47F;
            this.label18.Name = "label18";
            this.label18.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label18.Text = "担当者コード";
            this.label18.Top = 0.7F;
            this.label18.Width = 1.13F;
            // 
            // label19
            // 
            this.label19.Height = 0.2F;
            this.label19.HyperLink = null;
            this.label19.Left = 9.47F;
            this.label19.Name = "label19";
            this.label19.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label19.Text = "担当者名";
            this.label19.Top = 0.9F;
            this.label19.Width = 1.13F;
            // 
            // line6
            // 
            this.line6.Height = 0.4F;
            this.line6.Left = 2.5F;
            this.line6.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line6.LineWeight = 1F;
            this.line6.Name = "line6";
            this.line6.Top = 0.7F;
            this.line6.Width = 0F;
            this.line6.X1 = 2.5F;
            this.line6.X2 = 2.5F;
            this.line6.Y1 = 0.7F;
            this.line6.Y2 = 1.1F;
            // 
            // line7
            // 
            this.line7.Height = 0.4F;
            this.line7.Left = 3.05F;
            this.line7.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.Top = 0.7F;
            this.line7.Width = 0F;
            this.line7.X1 = 3.05F;
            this.line7.X2 = 3.05F;
            this.line7.Y1 = 0.7F;
            this.line7.Y2 = 1.1F;
            // 
            // line8
            // 
            this.line8.Height = 0.4F;
            this.line8.Left = 3.6F;
            this.line8.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line8.LineWeight = 1F;
            this.line8.Name = "line8";
            this.line8.Top = 0.7F;
            this.line8.Width = 0F;
            this.line8.X1 = 3.6F;
            this.line8.X2 = 3.6F;
            this.line8.Y1 = 0.7F;
            this.line8.Y2 = 1.1F;
            // 
            // line9
            // 
            this.line9.Height = 0.4F;
            this.line9.Left = 4.6F;
            this.line9.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.Top = 0.7F;
            this.line9.Width = 0F;
            this.line9.X1 = 4.6F;
            this.line9.X2 = 4.6F;
            this.line9.Y1 = 0.7F;
            this.line9.Y2 = 1.1F;
            // 
            // line10
            // 
            this.line10.Height = 0.4F;
            this.line10.Left = 5.6F;
            this.line10.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line10.LineWeight = 1F;
            this.line10.Name = "line10";
            this.line10.Top = 0.7F;
            this.line10.Width = 0F;
            this.line10.X1 = 5.6F;
            this.line10.X2 = 5.6F;
            this.line10.Y1 = 0.7F;
            this.line10.Y2 = 1.1F;
            // 
            // line11
            // 
            this.line11.Height = 0.4F;
            this.line11.Left = 6.6F;
            this.line11.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line11.LineWeight = 1F;
            this.line11.Name = "line11";
            this.line11.Top = 0.7F;
            this.line11.Width = 0F;
            this.line11.X1 = 6.6F;
            this.line11.X2 = 6.6F;
            this.line11.Y1 = 0.7F;
            this.line11.Y2 = 1.1F;
            // 
            // line12
            // 
            this.line12.Height = 0.4F;
            this.line12.Left = 8.370001F;
            this.line12.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line12.LineWeight = 1F;
            this.line12.Name = "line12";
            this.line12.Top = 0.7F;
            this.line12.Width = 0F;
            this.line12.X1 = 8.370001F;
            this.line12.X2 = 8.370001F;
            this.line12.Y1 = 0.7F;
            this.line12.Y2 = 1.1F;
            // 
            // line13
            // 
            this.line13.Height = 0.4F;
            this.line13.Left = 9.47F;
            this.line13.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line13.LineWeight = 1F;
            this.line13.Name = "line13";
            this.line13.Top = 0.7F;
            this.line13.Width = 0F;
            this.line13.X1 = 9.47F;
            this.line13.X2 = 9.47F;
            this.line13.Y1 = 0.7F;
            this.line13.Y2 = 1.1F;
            // 
            // label8
            // 
            this.label8.Height = 0.2F;
            this.label8.HyperLink = null;
            this.label8.Left = 0F;
            this.label8.Name = "label8";
            this.label8.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label8.Text = "請求ID";
            this.label8.Top = 0.7F;
            this.label8.Width = 0.7F;
            // 
            // label9
            // 
            this.label9.Height = 0.2F;
            this.label9.HyperLink = null;
            this.label9.Left = 0F;
            this.label9.Name = "label9";
            this.label9.Style = "background-color: WhiteSmoke; font-size: 7.5pt; text-align: center; vertical-alig" +
    "n: middle; ddo-char-set: 1";
            this.label9.Text = "消込区分";
            this.label9.Top = 0.9F;
            this.label9.Width = 0.7F;
            // 
            // line24
            // 
            this.line24.Height = 0.4F;
            this.line24.Left = 0.7F;
            this.line24.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line24.LineWeight = 1F;
            this.line24.Name = "line24";
            this.line24.Top = 0.7F;
            this.line24.Width = 0F;
            this.line24.X1 = 0.7F;
            this.line24.X2 = 0.7F;
            this.line24.Y1 = 0.7F;
            this.line24.Y2 = 1.1F;
            // 
            // line1
            // 
            this.line1.Height = 0F;
            this.line1.Left = 0F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 1.1F;
            this.line1.Width = 10.6F;
            this.line1.X1 = 0F;
            this.line1.X2 = 10.6F;
            this.line1.Y1 = 1.1F;
            this.line1.Y2 = 1.1F;
            // 
            // line2
            // 
            this.line2.Height = 0F;
            this.line2.Left = 0F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 0.7F;
            this.line2.Width = 10.6F;
            this.line2.X1 = 0F;
            this.line2.X2 = 10.6F;
            this.line2.Y1 = 0.7F;
            this.line2.Y2 = 0.7F;
            // 
            // line25
            // 
            this.line25.Height = 0F;
            this.line25.Left = 0F;
            this.line25.LineWeight = 1F;
            this.line25.Name = "line25";
            this.line25.Top = 0.9F;
            this.line25.Width = 3.6F;
            this.line25.X1 = 0F;
            this.line25.X2 = 3.6F;
            this.line25.Y1 = 0.9F;
            this.line25.Y2 = 0.9F;
            // 
            // line41
            // 
            this.line41.Height = 0F;
            this.line41.Left = 5.6F;
            this.line41.LineWeight = 1F;
            this.line41.Name = "line41";
            this.line41.Top = 0.9F;
            this.line41.Width = 5F;
            this.line41.X1 = 5.6F;
            this.line41.X2 = 10.6F;
            this.line41.Y1 = 0.9F;
            this.line41.Y2 = 0.9F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCustomerName,
            this.txtBillingAmount,
            this.txtRemainAmount,
            this.txtCategoryCode,
            this.txtBillingID,
            this.txtDepartmentName,
            this.txtBilledAt,
            this.txtClosingAt,
            this.txtInvoiceCode,
            this.txtCustomerCode,
            this.txtDepartmentCode,
            this.txtStaffName,
            this.txtSalesAt,
            this.txtDueAt,
            this.txtAssignmentFlag,
            this.txtInputType,
            this.txtNote1,
            this.line26,
            this.line15,
            this.line30,
            this.line17,
            this.line18,
            this.line19,
            this.line21,
            this.line4,
            this.line20,
            this.txtStaffCode,
            this.line23});
            this.detail.Height = 0.4043334F;
            this.detail.Name = "detail";
            this.detail.BeforePrint += new System.EventHandler(this.detail_BeforePrint);
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.CanGrow = false;
            this.txtCustomerName.DataField = "CustomerName";
            this.txtCustomerName.Height = 0.2F;
            this.txtCustomerName.Left = 0.7F;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerName.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCustomerName.Top = 0.2F;
            this.txtCustomerName.Width = 1.8F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.DataField = "BillingAmount";
            this.txtBillingAmount.Height = 0.4F;
            this.txtBillingAmount.Left = 3.6F;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingAmount.Style = "font-size: 6pt; text-align: right; vertical-align: middle; white-space: nowrap; d" +
    "do-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBillingAmount.Text = null;
            this.txtBillingAmount.Top = 0F;
            this.txtBillingAmount.Width = 1F;
            // 
            // txtRemainAmount
            // 
            this.txtRemainAmount.DataField = "RemainAmount";
            this.txtRemainAmount.Height = 0.4F;
            this.txtRemainAmount.Left = 4.6F;
            this.txtRemainAmount.Name = "txtRemainAmount";
            this.txtRemainAmount.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainAmount.Style = "font-size: 6pt; text-align: right; text-justify: auto; vertical-align: middle; wh" +
    "ite-space: nowrap; ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtRemainAmount.Top = 0F;
            this.txtRemainAmount.Width = 1F;
            // 
            // txtCategoryCode
            // 
            this.txtCategoryCode.CanGrow = false;
            this.txtCategoryCode.DataField = "BillingCategoryCodeAndName";
            this.txtCategoryCode.Height = 0.2F;
            this.txtCategoryCode.Left = 5.62F;
            this.txtCategoryCode.Name = "txtCategoryCode";
            this.txtCategoryCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCategoryCode.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtCategoryCode.Text = null;
            this.txtCategoryCode.Top = 0F;
            this.txtCategoryCode.Width = 0.98F;
            // 
            // txtBillingID
            // 
            this.txtBillingID.CanGrow = false;
            this.txtBillingID.DataField = "Id";
            this.txtBillingID.Height = 0.2F;
            this.txtBillingID.Left = 1.862645E-09F;
            this.txtBillingID.Name = "txtBillingID";
            this.txtBillingID.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingID.Style = "font-size: 6pt; text-align: right; vertical-align: middle; white-space: nowrap; d" +
    "do-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBillingID.Text = null;
            this.txtBillingID.Top = 0F;
            this.txtBillingID.Width = 0.68F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.CanGrow = false;
            this.txtDepartmentName.DataField = "DepartmentName";
            this.txtDepartmentName.Height = 0.2F;
            this.txtDepartmentName.Left = 8.39F;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtDepartmentName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtDepartmentName.Text = null;
            this.txtDepartmentName.Top = 0.2F;
            this.txtDepartmentName.Width = 1.08F;
            // 
            // txtBilledAt
            // 
            this.txtBilledAt.CanGrow = false;
            this.txtBilledAt.DataField = "BilledAt";
            this.txtBilledAt.Height = 0.2F;
            this.txtBilledAt.Left = 2.5F;
            this.txtBilledAt.Name = "txtBilledAt";
            this.txtBilledAt.OutputFormat = resources.GetString("txtBilledAt.OutputFormat");
            this.txtBilledAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtBilledAt.Text = null;
            this.txtBilledAt.Top = 1.862645E-09F;
            this.txtBilledAt.Width = 0.55F;
            // 
            // txtClosingAt
            // 
            this.txtClosingAt.CanGrow = false;
            this.txtClosingAt.DataField = "ClosingAt";
            this.txtClosingAt.Height = 0.2F;
            this.txtClosingAt.Left = 3.05F;
            this.txtClosingAt.Name = "txtClosingAt";
            this.txtClosingAt.OutputFormat = resources.GetString("txtClosingAt.OutputFormat");
            this.txtClosingAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtClosingAt.Text = null;
            this.txtClosingAt.Top = 0F;
            this.txtClosingAt.Width = 0.55F;
            // 
            // txtInvoiceCode
            // 
            this.txtInvoiceCode.CanGrow = false;
            this.txtInvoiceCode.DataField = "InvoiceCode";
            this.txtInvoiceCode.Height = 0.2F;
            this.txtInvoiceCode.Left = 6.62F;
            this.txtInvoiceCode.Name = "txtInvoiceCode";
            this.txtInvoiceCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtInvoiceCode.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtInvoiceCode.Text = null;
            this.txtInvoiceCode.Top = 0F;
            this.txtInvoiceCode.Width = 1.75F;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.CanGrow = false;
            this.txtCustomerCode.DataField = "CustomerCode";
            this.txtCustomerCode.Height = 0.2F;
            this.txtCustomerCode.Left = 0.7F;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtCustomerCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; white-space: nowrap; dd" +
    "o-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtCustomerCode.Text = null;
            this.txtCustomerCode.Top = 0F;
            this.txtCustomerCode.Width = 1.8F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.DataField = "DepartmentCode";
            this.txtDepartmentCode.Height = 0.2F;
            this.txtDepartmentCode.Left = 8.39F;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtDepartmentCode.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtDepartmentCode.Text = null;
            this.txtDepartmentCode.Top = 0F;
            this.txtDepartmentCode.Width = 1.08F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.CanGrow = false;
            this.txtStaffName.DataField = "StaffName";
            this.txtStaffName.Height = 0.2F;
            this.txtStaffName.Left = 9.5F;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtStaffName.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtStaffName.Text = null;
            this.txtStaffName.Top = 0.2F;
            this.txtStaffName.Width = 1.1F;
            // 
            // txtSalesAt
            // 
            this.txtSalesAt.CanGrow = false;
            this.txtSalesAt.DataField = "SalesAt";
            this.txtSalesAt.Height = 0.2F;
            this.txtSalesAt.Left = 2.5F;
            this.txtSalesAt.Name = "txtSalesAt";
            this.txtSalesAt.OutputFormat = resources.GetString("txtSalesAt.OutputFormat");
            this.txtSalesAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtSalesAt.Top = 0.2F;
            this.txtSalesAt.Width = 0.55F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.CanGrow = false;
            this.txtDueAt.DataField = "billingDueAt";
            this.txtDueAt.Height = 0.2F;
            this.txtDueAt.Left = 3.05F;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "font-size: 6pt; text-align: center; vertical-align: middle; white-space: nowrap; " +
    "ddo-char-set: 1; ddo-wrap-mode: nowrap";
            this.txtDueAt.Text = null;
            this.txtDueAt.Top = 0.2F;
            this.txtDueAt.Width = 0.55F;
            // 
            // txtAssignmentFlag
            // 
            this.txtAssignmentFlag.CanGrow = false;
            this.txtAssignmentFlag.DataField = "AssignmentFlagName";
            this.txtAssignmentFlag.Height = 0.2F;
            this.txtAssignmentFlag.Left = 0F;
            this.txtAssignmentFlag.Name = "txtAssignmentFlag";
            this.txtAssignmentFlag.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtAssignmentFlag.Style = "font-size: 6pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtAssignmentFlag.Text = null;
            this.txtAssignmentFlag.Top = 0.2F;
            this.txtAssignmentFlag.Width = 0.7F;
            // 
            // txtInputType
            // 
            this.txtInputType.CanGrow = false;
            this.txtInputType.DataField = "InputTypeName";
            this.txtInputType.Height = 0.2F;
            this.txtInputType.Left = 5.62F;
            this.txtInputType.Name = "txtInputType";
            this.txtInputType.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtInputType.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtInputType.Top = 0.2F;
            this.txtInputType.Width = 0.98F;
            // 
            // txtNote1
            // 
            this.txtNote1.CanGrow = false;
            this.txtNote1.DataField = "Note1";
            this.txtNote1.Height = 0.2F;
            this.txtNote1.Left = 6.62F;
            this.txtNote1.Name = "txtNote1";
            this.txtNote1.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtNote1.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtNote1.Text = null;
            this.txtNote1.Top = 0.2F;
            this.txtNote1.Width = 1.75F;
            // 
            // line26
            // 
            this.line26.Height = 0.4F;
            this.line26.Left = 0.7F;
            this.line26.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line26.LineWeight = 1F;
            this.line26.Name = "line26";
            this.line26.Top = 0F;
            this.line26.Width = 0F;
            this.line26.X1 = 0.7F;
            this.line26.X2 = 0.7F;
            this.line26.Y1 = 0F;
            this.line26.Y2 = 0.4F;
            // 
            // line15
            // 
            this.line15.Height = 0.4F;
            this.line15.Left = 2.5F;
            this.line15.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line15.LineWeight = 1F;
            this.line15.Name = "line15";
            this.line15.Top = 0F;
            this.line15.Width = 0F;
            this.line15.X1 = 2.5F;
            this.line15.X2 = 2.5F;
            this.line15.Y1 = 0F;
            this.line15.Y2 = 0.4F;
            // 
            // line30
            // 
            this.line30.Height = 0.4F;
            this.line30.Left = 3.05F;
            this.line30.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line30.LineWeight = 1F;
            this.line30.Name = "line30";
            this.line30.Top = 0F;
            this.line30.Width = 0F;
            this.line30.X1 = 3.05F;
            this.line30.X2 = 3.05F;
            this.line30.Y1 = 0F;
            this.line30.Y2 = 0.4F;
            // 
            // line17
            // 
            this.line17.Height = 0.4F;
            this.line17.Left = 3.6F;
            this.line17.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line17.LineWeight = 1F;
            this.line17.Name = "line17";
            this.line17.Top = 0F;
            this.line17.Width = 0F;
            this.line17.X1 = 3.6F;
            this.line17.X2 = 3.6F;
            this.line17.Y1 = 0F;
            this.line17.Y2 = 0.4F;
            // 
            // line18
            // 
            this.line18.Height = 0.4F;
            this.line18.Left = 4.6F;
            this.line18.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line18.LineWeight = 1F;
            this.line18.Name = "line18";
            this.line18.Top = 0F;
            this.line18.Width = 0F;
            this.line18.X1 = 4.6F;
            this.line18.X2 = 4.6F;
            this.line18.Y1 = 0F;
            this.line18.Y2 = 0.4F;
            // 
            // line19
            // 
            this.line19.Height = 0.4F;
            this.line19.Left = 5.6F;
            this.line19.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line19.LineWeight = 1F;
            this.line19.Name = "line19";
            this.line19.Top = 0F;
            this.line19.Width = 0F;
            this.line19.X1 = 5.6F;
            this.line19.X2 = 5.6F;
            this.line19.Y1 = 0F;
            this.line19.Y2 = 0.4F;
            // 
            // line21
            // 
            this.line21.Height = 0.4F;
            this.line21.Left = 6.6F;
            this.line21.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line21.LineWeight = 1F;
            this.line21.Name = "line21";
            this.line21.Top = -9.313226E-10F;
            this.line21.Width = 0F;
            this.line21.X1 = 6.6F;
            this.line21.X2 = 6.6F;
            this.line21.Y1 = -9.313226E-10F;
            this.line21.Y2 = 0.4F;
            // 
            // line4
            // 
            this.line4.Height = 0.4F;
            this.line4.Left = 8.370001F;
            this.line4.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line4.LineWeight = 1F;
            this.line4.Name = "line4";
            this.line4.Top = 0F;
            this.line4.Width = 0F;
            this.line4.X1 = 8.370001F;
            this.line4.X2 = 8.370001F;
            this.line4.Y1 = 0F;
            this.line4.Y2 = 0.4F;
            // 
            // line20
            // 
            this.line20.Height = 0F;
            this.line20.Left = 0F;
            this.line20.LineWeight = 1F;
            this.line20.Name = "line20";
            this.line20.Top = 0.4F;
            this.line20.Width = 10.6F;
            this.line20.X1 = 0F;
            this.line20.X2 = 10.6F;
            this.line20.Y1 = 0.4F;
            this.line20.Y2 = 0.4F;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.CanGrow = false;
            this.txtStaffCode.DataField = "StaffCode";
            this.txtStaffCode.Height = 0.2F;
            this.txtStaffCode.Left = 9.5F;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Padding = new GrapeCity.ActiveReports.PaddingEx(2, 0, 0, 0);
            this.txtStaffCode.Style = "font-size: 6pt; vertical-align: middle; white-space: nowrap; ddo-char-set: 1; ddo" +
    "-wrap-mode: nowrap";
            this.txtStaffCode.Text = null;
            this.txtStaffCode.Top = 0F;
            this.txtStaffCode.Width = 1.1F;
            // 
            // line23
            // 
            this.line23.Height = 0.4F;
            this.line23.Left = 9.47F;
            this.line23.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line23.LineWeight = 1F;
            this.line23.Name = "line23";
            this.line23.Top = 0F;
            this.line23.Width = 0F;
            this.line23.X1 = 9.47F;
            this.line23.X2 = 9.47F;
            this.line23.Y1 = 0F;
            this.line23.Y2 = 0.4F;
            // 
            // line14
            // 
            this.line14.Height = 3.939867E-05F;
            this.line14.Left = 0.039F;
            this.line14.LineWeight = 1F;
            this.line14.Name = "line14";
            this.line14.Top = 0F;
            this.line14.Width = 10.56181F;
            this.line14.X1 = 0.039F;
            this.line14.X2 = 10.60081F;
            this.line14.Y1 = 0F;
            this.line14.Y2 = 3.939867E-05F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblPageNumber,
            this.reportInfo1});
            this.pageFooter.Height = 0.49625F;
            this.pageFooter.Name = "pageFooter";
            this.pageFooter.BeforePrint += new System.EventHandler(this.pageFooter_BeforePrint);
            // 
            // lblPageNumber
            // 
            this.lblPageNumber.Height = 0.2F;
            this.lblPageNumber.HyperLink = null;
            this.lblPageNumber.Left = 0.024F;
            this.lblPageNumber.Name = "lblPageNumber";
            this.lblPageNumber.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle";
            this.lblPageNumber.Text = "PageNumber/PageCount";
            this.lblPageNumber.Top = 0.057F;
            this.lblPageNumber.Width = 10.541F;
            // 
            // reportInfo1
            // 
            this.reportInfo1.FormatString = "{PageNumber} / {PageCount}";
            this.reportInfo1.Height = 0.2F;
            this.reportInfo1.Left = 7.169292F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.05748032F;
            this.reportInfo1.Visible = false;
            this.reportInfo1.Width = 1.181102F;
            // 
            // ghGrandTotal
            // 
            this.ghGrandTotal.Height = 0F;
            this.ghGrandTotal.Name = "ghGrandTotal";
            // 
            // gfGrandTotal
            // 
            this.gfGrandTotal.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtBillingGrandTotal,
            this.label23,
            this.line34,
            this.txtRemainGrandTotal,
            this.label24,
            this.line16,
            this.line35,
            this.line39});
            this.gfGrandTotal.Name = "gfGrandTotal";
            // 
            // txtBillingGrandTotal
            // 
            this.txtBillingGrandTotal.DataField = "BillingAmount";
            this.txtBillingGrandTotal.Height = 0.25F;
            this.txtBillingGrandTotal.Left = 3.6F;
            this.txtBillingGrandTotal.Name = "txtBillingGrandTotal";
            this.txtBillingGrandTotal.OutputFormat = resources.GetString("txtBillingGrandTotal.OutputFormat");
            this.txtBillingGrandTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingGrandTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtBillingGrandTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtBillingGrandTotal.Text = null;
            this.txtBillingGrandTotal.Top = 0F;
            this.txtBillingGrandTotal.Width = 1F;
            // 
            // label23
            // 
            this.label23.Height = 0.25F;
            this.label23.HyperLink = null;
            this.label23.Left = 0F;
            this.label23.Name = "label23";
            this.label23.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.label23.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.label23.Text = "総合計";
            this.label23.Top = 0F;
            this.label23.Width = 3.6F;
            // 
            // line34
            // 
            this.line34.Height = 0.25F;
            this.line34.Left = 3.6F;
            this.line34.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line34.LineWeight = 1F;
            this.line34.Name = "line34";
            this.line34.Top = 0F;
            this.line34.Width = 0F;
            this.line34.X1 = 3.6F;
            this.line34.X2 = 3.6F;
            this.line34.Y1 = 0F;
            this.line34.Y2 = 0.25F;
            // 
            // txtRemainGrandTotal
            // 
            this.txtRemainGrandTotal.DataField = "RemainAmount";
            this.txtRemainGrandTotal.Height = 0.25F;
            this.txtRemainGrandTotal.Left = 4.6F;
            this.txtRemainGrandTotal.Name = "txtRemainGrandTotal";
            this.txtRemainGrandTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainGrandTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtRemainGrandTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtRemainGrandTotal.Text = null;
            this.txtRemainGrandTotal.Top = 0F;
            this.txtRemainGrandTotal.Width = 1F;
            // 
            // label24
            // 
            this.label24.Height = 0.25F;
            this.label24.HyperLink = null;
            this.label24.Left = 5.6F;
            this.label24.Name = "label24";
            this.label24.Style = "background-color: WhiteSmoke; font-size: 7.5pt; ddo-char-set: 1";
            this.label24.Text = "";
            this.label24.Top = 0F;
            this.label24.Width = 5.004F;
            // 
            // line16
            // 
            this.line16.Height = 0F;
            this.line16.Left = 0F;
            this.line16.LineWeight = 1F;
            this.line16.Name = "line16";
            this.line16.Top = 0.25F;
            this.line16.Width = 10.6F;
            this.line16.X1 = 0F;
            this.line16.X2 = 10.6F;
            this.line16.Y1 = 0.25F;
            this.line16.Y2 = 0.25F;
            // 
            // line35
            // 
            this.line35.Height = 0.25F;
            this.line35.Left = 4.6F;
            this.line35.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line35.LineWeight = 1F;
            this.line35.Name = "line35";
            this.line35.Top = 0F;
            this.line35.Width = 0F;
            this.line35.X1 = 4.6F;
            this.line35.X2 = 4.6F;
            this.line35.Y1 = 0F;
            this.line35.Y2 = 0.25F;
            // 
            // line39
            // 
            this.line39.Height = 0.25F;
            this.line39.Left = 5.6F;
            this.line39.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line39.LineWeight = 1F;
            this.line39.Name = "line39";
            this.line39.Top = 0F;
            this.line39.Width = 0F;
            this.line39.X1 = 5.6F;
            this.line39.X2 = 5.6F;
            this.line39.Y1 = 0F;
            this.line39.Y2 = 0.25F;
            // 
            // ghDepartmentTotal
            // 
            this.ghDepartmentTotal.DataField = "DepartmentId";
            this.ghDepartmentTotal.Height = 0F;
            this.ghDepartmentTotal.Name = "ghDepartmentTotal";
            // 
            // gfDepartmentTotal
            // 
            this.gfDepartmentTotal.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtBillingDepartmentTotal,
            this.label22,
            this.line32,
            this.txtRemainDepartmentTotal,
            this.label25,
            this.line3,
            this.line33,
            this.line38});
            this.gfDepartmentTotal.Name = "gfDepartmentTotal";
            // 
            // txtBillingDepartmentTotal
            // 
            this.txtBillingDepartmentTotal.DataField = "BillingAmount";
            this.txtBillingDepartmentTotal.Height = 0.25F;
            this.txtBillingDepartmentTotal.Left = 3.6F;
            this.txtBillingDepartmentTotal.Name = "txtBillingDepartmentTotal";
            this.txtBillingDepartmentTotal.OutputFormat = resources.GetString("txtBillingDepartmentTotal.OutputFormat");
            this.txtBillingDepartmentTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingDepartmentTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtBillingDepartmentTotal.SummaryGroup = "ghDepartmentTotal";
            this.txtBillingDepartmentTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtBillingDepartmentTotal.Text = null;
            this.txtBillingDepartmentTotal.Top = 0F;
            this.txtBillingDepartmentTotal.Width = 1F;
            // 
            // label22
            // 
            this.label22.Height = 0.25F;
            this.label22.HyperLink = null;
            this.label22.Left = 0F;
            this.label22.Name = "label22";
            this.label22.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.label22.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.label22.Text = "部門計";
            this.label22.Top = 0F;
            this.label22.Width = 3.6F;
            // 
            // line32
            // 
            this.line32.Height = 0.25F;
            this.line32.Left = 3.6F;
            this.line32.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line32.LineWeight = 1F;
            this.line32.Name = "line32";
            this.line32.Top = 0F;
            this.line32.Width = 0F;
            this.line32.X1 = 3.6F;
            this.line32.X2 = 3.6F;
            this.line32.Y1 = 0F;
            this.line32.Y2 = 0.25F;
            // 
            // txtRemainDepartmentTotal
            // 
            this.txtRemainDepartmentTotal.DataField = "RemainAmount";
            this.txtRemainDepartmentTotal.Height = 0.25F;
            this.txtRemainDepartmentTotal.Left = 4.6F;
            this.txtRemainDepartmentTotal.Name = "txtRemainDepartmentTotal";
            this.txtRemainDepartmentTotal.OutputFormat = resources.GetString("txtRemainDepartmentTotal.OutputFormat");
            this.txtRemainDepartmentTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainDepartmentTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtRemainDepartmentTotal.SummaryGroup = "ghDepartmentTotal";
            this.txtRemainDepartmentTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtRemainDepartmentTotal.Text = null;
            this.txtRemainDepartmentTotal.Top = 0F;
            this.txtRemainDepartmentTotal.Width = 1F;
            // 
            // label25
            // 
            this.label25.Height = 0.25F;
            this.label25.HyperLink = null;
            this.label25.Left = 5.6F;
            this.label25.Name = "label25";
            this.label25.Style = "background-color: WhiteSmoke; font-size: 7.5pt; ddo-char-set: 1";
            this.label25.Text = "";
            this.label25.Top = 0F;
            this.label25.Width = 5F;
            // 
            // line3
            // 
            this.line3.Height = 0F;
            this.line3.Left = 0.039F;
            this.line3.LineWeight = 1F;
            this.line3.Name = "line3";
            this.line3.Top = 0.25F;
            this.line3.Width = 10.565F;
            this.line3.X1 = 0.039F;
            this.line3.X2 = 10.604F;
            this.line3.Y1 = 0.25F;
            this.line3.Y2 = 0.25F;
            // 
            // line33
            // 
            this.line33.Height = 0.25F;
            this.line33.Left = 4.599843F;
            this.line33.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line33.LineWeight = 1F;
            this.line33.Name = "line33";
            this.line33.Top = 0F;
            this.line33.Width = 0.0001568794F;
            this.line33.X1 = 4.6F;
            this.line33.X2 = 4.599843F;
            this.line33.Y1 = 0F;
            this.line33.Y2 = 0.25F;
            // 
            // line38
            // 
            this.line38.Height = 0.25F;
            this.line38.Left = 5.599687F;
            this.line38.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line38.LineWeight = 1F;
            this.line38.Name = "line38";
            this.line38.Top = 0F;
            this.line38.Width = 0.0003128052F;
            this.line38.X1 = 5.6F;
            this.line38.X2 = 5.599687F;
            this.line38.Y1 = 0F;
            this.line38.Y2 = 0.25F;
            // 
            // ghStaffTotal
            // 
            this.ghStaffTotal.DataField = "StaffId";
            this.ghStaffTotal.Height = 0F;
            this.ghStaffTotal.Name = "ghStaffTotal";
            // 
            // gfStaffTotal
            // 
            this.gfStaffTotal.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtBillingStaffTotal,
            this.label21,
            this.line31,
            this.txtRemainStaffTotal,
            this.label26,
            this.line22,
            this.line28,
            this.line37});
            this.gfStaffTotal.Name = "gfStaffTotal";
            // 
            // txtBillingStaffTotal
            // 
            this.txtBillingStaffTotal.DataField = "BillingAmount";
            this.txtBillingStaffTotal.Height = 0.25F;
            this.txtBillingStaffTotal.Left = 3.6F;
            this.txtBillingStaffTotal.Name = "txtBillingStaffTotal";
            this.txtBillingStaffTotal.OutputFormat = resources.GetString("txtBillingStaffTotal.OutputFormat");
            this.txtBillingStaffTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingStaffTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtBillingStaffTotal.SummaryGroup = "ghStaffTotal";
            this.txtBillingStaffTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtBillingStaffTotal.Text = null;
            this.txtBillingStaffTotal.Top = 0F;
            this.txtBillingStaffTotal.Width = 1F;
            // 
            // label21
            // 
            this.label21.Height = 0.25F;
            this.label21.HyperLink = null;
            this.label21.Left = 0F;
            this.label21.Name = "label21";
            this.label21.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.label21.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.label21.Text = "担当者計";
            this.label21.Top = 0F;
            this.label21.Width = 3.6F;
            // 
            // line31
            // 
            this.line31.Height = 0.25F;
            this.line31.Left = 3.6F;
            this.line31.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line31.LineWeight = 1F;
            this.line31.Name = "line31";
            this.line31.Top = 0F;
            this.line31.Width = 0F;
            this.line31.X1 = 3.6F;
            this.line31.X2 = 3.6F;
            this.line31.Y1 = 0F;
            this.line31.Y2 = 0.25F;
            // 
            // txtRemainStaffTotal
            // 
            this.txtRemainStaffTotal.DataField = "RemainAmount";
            this.txtRemainStaffTotal.Height = 0.25F;
            this.txtRemainStaffTotal.Left = 4.6F;
            this.txtRemainStaffTotal.Name = "txtRemainStaffTotal";
            this.txtRemainStaffTotal.OutputFormat = resources.GetString("txtRemainStaffTotal.OutputFormat");
            this.txtRemainStaffTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainStaffTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtRemainStaffTotal.SummaryGroup = "ghStaffTotal";
            this.txtRemainStaffTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtRemainStaffTotal.Text = null;
            this.txtRemainStaffTotal.Top = 0F;
            this.txtRemainStaffTotal.Width = 1F;
            // 
            // label26
            // 
            this.label26.Height = 0.25F;
            this.label26.HyperLink = null;
            this.label26.Left = 5.6F;
            this.label26.Name = "label26";
            this.label26.Style = "background-color: WhiteSmoke; font-size: 7.5pt; ddo-char-set: 1";
            this.label26.Text = "";
            this.label26.Top = 0F;
            this.label26.Width = 5F;
            // 
            // line22
            // 
            this.line22.Height = 0F;
            this.line22.Left = 0F;
            this.line22.LineWeight = 1F;
            this.line22.Name = "line22";
            this.line22.Top = 0.25F;
            this.line22.Width = 10.6F;
            this.line22.X1 = 0F;
            this.line22.X2 = 10.6F;
            this.line22.Y1 = 0.25F;
            this.line22.Y2 = 0.25F;
            // 
            // line28
            // 
            this.line28.Height = 0.25F;
            this.line28.Left = 4.599843F;
            this.line28.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line28.LineWeight = 1F;
            this.line28.Name = "line28";
            this.line28.Top = 0F;
            this.line28.Width = 0.0001568794F;
            this.line28.X1 = 4.6F;
            this.line28.X2 = 4.599843F;
            this.line28.Y1 = 0F;
            this.line28.Y2 = 0.25F;
            // 
            // line37
            // 
            this.line37.Height = 0.25F;
            this.line37.Left = 5.599687F;
            this.line37.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line37.LineWeight = 1F;
            this.line37.Name = "line37";
            this.line37.Top = 0F;
            this.line37.Width = 0.0003128052F;
            this.line37.X1 = 5.6F;
            this.line37.X2 = 5.599687F;
            this.line37.Y1 = 0F;
            this.line37.Y2 = 0.25F;
            // 
            // txtBillingCustomerTotal
            // 
            this.txtBillingCustomerTotal.DataField = "BillingAmount";
            this.txtBillingCustomerTotal.Height = 0.25F;
            this.txtBillingCustomerTotal.Left = 3.6F;
            this.txtBillingCustomerTotal.Name = "txtBillingCustomerTotal";
            this.txtBillingCustomerTotal.OutputFormat = resources.GetString("txtBillingCustomerTotal.OutputFormat");
            this.txtBillingCustomerTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtBillingCustomerTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtBillingCustomerTotal.SummaryGroup = "ghCustomerTotal";
            this.txtBillingCustomerTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtBillingCustomerTotal.Text = null;
            this.txtBillingCustomerTotal.Top = 0F;
            this.txtBillingCustomerTotal.Width = 1F;
            // 
            // ghCustomerTotal
            // 
            this.ghCustomerTotal.DataField = "CustomerId";
            this.ghCustomerTotal.Height = 0F;
            this.ghCustomerTotal.Name = "ghCustomerTotal";
            // 
            // gfCustomerTotal
            // 
            this.gfCustomerTotal.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtBillingCustomerTotal,
            this.label20,
            this.line27,
            this.txtRemainCustomerTotal,
            this.label27,
            this.line5,
            this.line29,
            this.line36,
            this.line14});
            this.gfCustomerTotal.Name = "gfCustomerTotal";
            // 
            // label20
            // 
            this.label20.Height = 0.25F;
            this.label20.HyperLink = null;
            this.label20.Left = 0F;
            this.label20.Name = "label20";
            this.label20.Padding = new GrapeCity.ActiveReports.PaddingEx(20, 0, 0, 0);
            this.label20.Style = "background-color: WhiteSmoke; font-size: 7.5pt; vertical-align: middle; ddo-char-" +
    "set: 1";
            this.label20.Text = "得意先計";
            this.label20.Top = 0F;
            this.label20.Width = 3.6F;
            // 
            // line27
            // 
            this.line27.Height = 0.25F;
            this.line27.Left = 3.6F;
            this.line27.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line27.LineWeight = 1F;
            this.line27.Name = "line27";
            this.line27.Top = 0F;
            this.line27.Width = 0F;
            this.line27.X1 = 3.6F;
            this.line27.X2 = 3.6F;
            this.line27.Y1 = 0F;
            this.line27.Y2 = 0.25F;
            // 
            // txtRemainCustomerTotal
            // 
            this.txtRemainCustomerTotal.DataField = "RemainAmount";
            this.txtRemainCustomerTotal.Height = 0.25F;
            this.txtRemainCustomerTotal.Left = 4.6F;
            this.txtRemainCustomerTotal.Name = "txtRemainCustomerTotal";
            this.txtRemainCustomerTotal.OutputFormat = resources.GetString("txtRemainCustomerTotal.OutputFormat");
            this.txtRemainCustomerTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 2, 0);
            this.txtRemainCustomerTotal.Style = "background-color: WhiteSmoke; font-size: 6pt; text-align: right; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.txtRemainCustomerTotal.SummaryGroup = "ghCustomerTotal";
            this.txtRemainCustomerTotal.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtRemainCustomerTotal.Text = null;
            this.txtRemainCustomerTotal.Top = 0F;
            this.txtRemainCustomerTotal.Width = 1F;
            // 
            // label27
            // 
            this.label27.Height = 0.25F;
            this.label27.HyperLink = null;
            this.label27.Left = 5.6F;
            this.label27.Name = "label27";
            this.label27.Style = "background-color: WhiteSmoke; font-size: 7.5pt; ddo-char-set: 1";
            this.label27.Text = "";
            this.label27.Top = 0F;
            this.label27.Width = 5F;
            // 
            // line5
            // 
            this.line5.Height = 0F;
            this.line5.Left = 0F;
            this.line5.LineWeight = 1F;
            this.line5.Name = "line5";
            this.line5.Top = 0.25F;
            this.line5.Width = 10.6F;
            this.line5.X1 = 0F;
            this.line5.X2 = 10.6F;
            this.line5.Y1 = 0.25F;
            this.line5.Y2 = 0.25F;
            // 
            // line29
            // 
            this.line29.Height = 0.25F;
            this.line29.Left = 4.6F;
            this.line29.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line29.LineWeight = 1F;
            this.line29.Name = "line29";
            this.line29.Top = 0F;
            this.line29.Width = 0F;
            this.line29.X1 = 4.6F;
            this.line29.X2 = 4.6F;
            this.line29.Y1 = 0F;
            this.line29.Y2 = 0.25F;
            // 
            // line36
            // 
            this.line36.Height = 0.25F;
            this.line36.Left = 5.6F;
            this.line36.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.line36.LineWeight = 1F;
            this.line36.Name = "line36";
            this.line36.Top = 0F;
            this.line36.Width = 0F;
            this.line36.X1 = 5.6F;
            this.line36.X2 = 5.6F;
            this.line36.Y1 = 0F;
            this.line36.Y2 = 0.25F;
            // 
            // BillingServiceSearchSectionReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            this.PrintWidth = 10.604F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.ghGrandTotal);
            this.Sections.Add(this.ghDepartmentTotal);
            this.Sections.Add(this.ghStaffTotal);
            this.Sections.Add(this.ghCustomerTotal);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.gfCustomerTotal);
            this.Sections.Add(this.gfStaffTotal);
            this.Sections.Add(this.gfDepartmentTotal);
            this.Sections.Add(this.gfGrandTotal);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSalesAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAssignmentFlag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInputType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingGrandTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainGrandTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingDepartmentTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainDepartmentTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingStaffTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainStaffTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCustomerTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainCustomerTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblcompanycode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label label2;
        private GrapeCity.ActiveReports.SectionReportModel.Label label3;
        private GrapeCity.ActiveReports.SectionReportModel.Label label4;
        private GrapeCity.ActiveReports.SectionReportModel.Label label5;
        private GrapeCity.ActiveReports.SectionReportModel.Label label6;
        private GrapeCity.ActiveReports.SectionReportModel.Label label7;
        private GrapeCity.ActiveReports.SectionReportModel.Label label10;
        private GrapeCity.ActiveReports.SectionReportModel.Label label11;
        private GrapeCity.ActiveReports.SectionReportModel.Label label12;
        private GrapeCity.ActiveReports.SectionReportModel.Label label13;
        private GrapeCity.ActiveReports.SectionReportModel.Label label14;
        private GrapeCity.ActiveReports.SectionReportModel.Label label16;
        private GrapeCity.ActiveReports.SectionReportModel.Label label17;
        private GrapeCity.ActiveReports.SectionReportModel.Label label18;
        private GrapeCity.ActiveReports.SectionReportModel.Label label19;
        private GrapeCity.ActiveReports.SectionReportModel.Line line6;
        private GrapeCity.ActiveReports.SectionReportModel.Line line7;
        private GrapeCity.ActiveReports.SectionReportModel.Line line8;
        private GrapeCity.ActiveReports.SectionReportModel.Line line9;
        private GrapeCity.ActiveReports.SectionReportModel.Line line10;
        private GrapeCity.ActiveReports.SectionReportModel.Line line11;
        private GrapeCity.ActiveReports.SectionReportModel.Line line12;
        private GrapeCity.ActiveReports.SectionReportModel.Line line13;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCategoryCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line line17;
        private GrapeCity.ActiveReports.SectionReportModel.Line line19;
        private GrapeCity.ActiveReports.SectionReportModel.Line line21;
        private GrapeCity.ActiveReports.SectionReportModel.Line line23;
        private GrapeCity.ActiveReports.SectionReportModel.Label label8;
        private GrapeCity.ActiveReports.SectionReportModel.Label label9;
        private GrapeCity.ActiveReports.SectionReportModel.Line line24;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingID;
        private GrapeCity.ActiveReports.SectionReportModel.Line line14;
        private GrapeCity.ActiveReports.SectionReportModel.Line line15;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
        private GrapeCity.ActiveReports.SectionReportModel.Line line18;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBilledAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtClosingAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInvoiceCode;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingStaffTotal;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghStaffTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingCustomerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingDepartmentTotal;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghCustomerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label label23;
        private GrapeCity.ActiveReports.SectionReportModel.Label label22;
        private GrapeCity.ActiveReports.SectionReportModel.Label label21;
        private GrapeCity.ActiveReports.SectionReportModel.Label label20;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtSalesAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtAssignmentFlag;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtInputType;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line28;
        private GrapeCity.ActiveReports.SectionReportModel.Line line34;
        private GrapeCity.ActiveReports.SectionReportModel.Line line35;
        private GrapeCity.ActiveReports.SectionReportModel.Line line32;
        private GrapeCity.ActiveReports.SectionReportModel.Line line33;
        private GrapeCity.ActiveReports.SectionReportModel.Line line31;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainStaffTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line line29;
        private GrapeCity.ActiveReports.SectionReportModel.Line line27;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainCustomerTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line line39;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRemainDepartmentTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line line38;
        private GrapeCity.ActiveReports.SectionReportModel.Line line37;
        private GrapeCity.ActiveReports.SectionReportModel.Line line36;
        private GrapeCity.ActiveReports.SectionReportModel.Line line25;
        private GrapeCity.ActiveReports.SectionReportModel.Line line41;
        private GrapeCity.ActiveReports.SectionReportModel.Line line30;
        private GrapeCity.ActiveReports.SectionReportModel.Label label24;
        private GrapeCity.ActiveReports.SectionReportModel.Label label25;
        private GrapeCity.ActiveReports.SectionReportModel.Label label26;
        private GrapeCity.ActiveReports.SectionReportModel.Label label27;
        private GrapeCity.ActiveReports.SectionReportModel.Line line26;
        private GrapeCity.ActiveReports.SectionReportModel.Line line4;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line line22;
        private GrapeCity.ActiveReports.SectionReportModel.Line line16;
        private GrapeCity.ActiveReports.SectionReportModel.Line line3;
        private GrapeCity.ActiveReports.SectionReportModel.Line line5;
        public GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfStaffTotal;
        public GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfCustomerTotal;
        public GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfDepartmentTotal;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblNote1;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line20;
        public GrapeCity.ActiveReports.SectionReportModel.Label lbltitle;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghDepartmentTotal;
    }
}
