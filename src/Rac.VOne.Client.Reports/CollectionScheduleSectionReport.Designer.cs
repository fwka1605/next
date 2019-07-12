namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CollectionScheduleSectionReport の概要の説明です。
    /// </summary>
    partial class CollectionScheduleSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CollectionScheduleSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lblCompany = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblcompanycode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblHeaderDepartment = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblHeaderDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblHeaderDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblHeaderStaff = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblHeaderStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblHeaderStaffName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomer = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblCustomerCollectInfo = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblClosingDay = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTanto = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBumon = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblKubun = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblUncollectedAmountLast = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblUncollectAmount0 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblUncollectAmount1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblUncollectAmount2 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblUncollectAmount3 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblKingakuK = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineHeaderTop = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderBottom = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV01 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV02 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV03 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV04 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV05 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV06 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV07 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV09 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV10 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV11 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV13 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderV08 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtCustomerCodeandName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtClosingDay = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartment = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCategoryName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmountLast = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmount0 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmount1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmount2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmount3 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmountTotal = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDV01 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV02 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV03 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV04 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV05 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV06 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV07 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV09 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV10 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV11 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV13 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineBottomDetail = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDV08 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailTop = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lblPageNumber = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.grhGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.grfGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblGrandTotalBack = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblGrandTotalCaption = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtUncollectedAmountLastSum = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmount0Sum = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmount1Sum = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmount2Sum = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmount3Sum = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtUncollectedAmountTotalSum = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineBottomGrandTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV01 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV02 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV03 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV04 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV05 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV06 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV07 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV08 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineGTV09 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.grhTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.grfTotal = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblCaptionTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineBottomTotal = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT0 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVT9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.grhDepartment = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.grfDepartment = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblCaptionDepartmentTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineBottomDepartment = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD0 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVD9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.grhStaff = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.grfStaff = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.lblCaptionStaffTotal = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineBottomStaff = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS0 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS3 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS4 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS5 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS6 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS8 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineVS9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.linTantoTop = new GrapeCity.ActiveReports.SectionReportModel.Line();
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderStaff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCollectInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTanto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBumon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblKubun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectedAmountLast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectAmount0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectAmount1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectAmount2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectAmount3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblKingakuK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeandName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmountLast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrandTotalBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrandTotalCaption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmountLastSum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount0Sum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount1Sum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount2Sum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount3Sum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmountTotalSum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCaptionTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCaptionDepartmentTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCaptionStaffTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCompany,
            this.lblcompanycode,
            this.lblTitle,
            this.lbldate,
            this.ridate,
            this.lblHeaderDepartment,
            this.lblHeaderDepartmentCode,
            this.lblHeaderDepartmentName,
            this.lblHeaderStaff,
            this.lblHeaderStaffCode,
            this.lblHeaderStaffName,
            this.lblCustomer,
            this.lblCustomerCollectInfo,
            this.lblClosingDay,
            this.lblTanto,
            this.lblBumon,
            this.lblKubun,
            this.lblUncollectedAmountLast,
            this.lblUncollectAmount0,
            this.lblUncollectAmount1,
            this.lblUncollectAmount2,
            this.lblUncollectAmount3,
            this.lblKingakuK,
            this.lineHeaderTop,
            this.lineHeaderBottom,
            this.lineHeaderV01,
            this.lineHeaderV02,
            this.lineHeaderV03,
            this.lineHeaderV04,
            this.lineHeaderV05,
            this.lineHeaderV06,
            this.lineHeaderV07,
            this.lineHeaderV09,
            this.lineHeaderV10,
            this.lineHeaderV11,
            this.lineHeaderV12,
            this.lineHeaderV13,
            this.lineHeaderV08});
            this.pageHeader.Height = 1.281102F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lblCompany
            // 
            this.lblCompany.Height = 0.2F;
            this.lblCompany.HyperLink = null;
            this.lblCompany.Left = 0F;
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblCompany.Text = "会社コード　：";
            this.lblCompany.Top = 0F;
            this.lblCompany.Width = 0.7874014F;
            // 
            // lblcompanycode
            // 
            this.lblcompanycode.Height = 0.2F;
            this.lblcompanycode.HyperLink = null;
            this.lblcompanycode.Left = 0.7874016F;
            this.lblcompanycode.Name = "lblcompanycode";
            this.lblcompanycode.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.lblcompanycode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblcompanycode.Text = "label2";
            this.lblcompanycode.Top = 0F;
            this.lblcompanycode.Width = 3.657F;
            // 
            // lblTitle
            // 
            this.lblTitle.Height = 0.2311024F;
            this.lblTitle.HyperLink = null;
            this.lblTitle.Left = 0F;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; ddo-char-set: 1";
            this.lblTitle.Text = "回収予定表";
            this.lblTitle.Top = 0.2F;
            this.lblTitle.Width = 10.62992F;
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
            this.lbldate.Width = 0.698425F;
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
            // lblHeaderDepartment
            // 
            this.lblHeaderDepartment.Height = 0.2F;
            this.lblHeaderDepartment.HyperLink = null;
            this.lblHeaderDepartment.Left = 0F;
            this.lblHeaderDepartment.Name = "lblHeaderDepartment";
            this.lblHeaderDepartment.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblHeaderDepartment.Text = "請求部門コード :";
            this.lblHeaderDepartment.Top = 0.4311024F;
            this.lblHeaderDepartment.Width = 0.9350395F;
            // 
            // lblHeaderDepartmentCode
            // 
            this.lblHeaderDepartmentCode.DataField = "DepartmentCode";
            this.lblHeaderDepartmentCode.Height = 0.2F;
            this.lblHeaderDepartmentCode.HyperLink = null;
            this.lblHeaderDepartmentCode.Left = 0.9350395F;
            this.lblHeaderDepartmentCode.Name = "lblHeaderDepartmentCode";
            this.lblHeaderDepartmentCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblHeaderDepartmentCode.Text = "";
            this.lblHeaderDepartmentCode.Top = 0.4311024F;
            this.lblHeaderDepartmentCode.Width = 0.5579998F;
            // 
            // lblHeaderDepartmentName
            // 
            this.lblHeaderDepartmentName.DataField = "DepartmentName";
            this.lblHeaderDepartmentName.Height = 0.2F;
            this.lblHeaderDepartmentName.HyperLink = null;
            this.lblHeaderDepartmentName.Left = 1.492914F;
            this.lblHeaderDepartmentName.Name = "lblHeaderDepartmentName";
            this.lblHeaderDepartmentName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblHeaderDepartmentName.Text = "";
            this.lblHeaderDepartmentName.Top = 0.4311024F;
            this.lblHeaderDepartmentName.Width = 3.452237F;
            // 
            // lblHeaderStaff
            // 
            this.lblHeaderStaff.Height = 0.2F;
            this.lblHeaderStaff.HyperLink = null;
            this.lblHeaderStaff.Left = 0F;
            this.lblHeaderStaff.Name = "lblHeaderStaff";
            this.lblHeaderStaff.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblHeaderStaff.Text = "担当者コード   :";
            this.lblHeaderStaff.Top = 0.6311024F;
            this.lblHeaderStaff.Width = 0.9350395F;
            // 
            // lblHeaderStaffCode
            // 
            this.lblHeaderStaffCode.DataField = "StaffCode";
            this.lblHeaderStaffCode.Height = 0.2F;
            this.lblHeaderStaffCode.HyperLink = null;
            this.lblHeaderStaffCode.Left = 0.9350395F;
            this.lblHeaderStaffCode.Name = "lblHeaderStaffCode";
            this.lblHeaderStaffCode.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblHeaderStaffCode.Text = "";
            this.lblHeaderStaffCode.Top = 0.6311024F;
            this.lblHeaderStaffCode.Width = 0.5579998F;
            // 
            // lblHeaderStaffName
            // 
            this.lblHeaderStaffName.DataField = "StaffName";
            this.lblHeaderStaffName.Height = 0.2F;
            this.lblHeaderStaffName.HyperLink = null;
            this.lblHeaderStaffName.Left = 1.492914F;
            this.lblHeaderStaffName.Name = "lblHeaderStaffName";
            this.lblHeaderStaffName.Style = "color: Gray; font-size: 7pt; vertical-align: middle; ddo-char-set: 1";
            this.lblHeaderStaffName.Text = "";
            this.lblHeaderStaffName.Top = 0.6311024F;
            this.lblHeaderStaffName.Width = 3.452237F;
            // 
            // lblCustomer
            // 
            this.lblCustomer.Height = 0.1875F;
            this.lblCustomer.HyperLink = null;
            this.lblCustomer.Left = 0F;
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblCustomer.Text = "得意先";
            this.lblCustomer.Top = 0.906F;
            this.lblCustomer.Width = 2.342126F;
            // 
            // lblCustomerCollectInfo
            // 
            this.lblCustomerCollectInfo.Height = 0.1875F;
            this.lblCustomerCollectInfo.HyperLink = null;
            this.lblCustomerCollectInfo.Left = 0F;
            this.lblCustomerCollectInfo.Name = "lblCustomerCollectInfo";
            this.lblCustomerCollectInfo.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblCustomerCollectInfo.Text = "マスター回収条件";
            this.lblCustomerCollectInfo.Top = 1.093F;
            this.lblCustomerCollectInfo.Width = 2.342126F;
            // 
            // lblClosingDay
            // 
            this.lblClosingDay.Height = 0.375F;
            this.lblClosingDay.HyperLink = null;
            this.lblClosingDay.Left = 2.342126F;
            this.lblClosingDay.Name = "lblClosingDay";
            this.lblClosingDay.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblClosingDay.Text = "締日";
            this.lblClosingDay.Top = 0.9051182F;
            this.lblClosingDay.Width = 0.25F;
            // 
            // lblTanto
            // 
            this.lblTanto.Height = 0.375F;
            this.lblTanto.HyperLink = null;
            this.lblTanto.Left = 2.592126F;
            this.lblTanto.Name = "lblTanto";
            this.lblTanto.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblTanto.Text = "担当者";
            this.lblTanto.Top = 0.9051182F;
            this.lblTanto.Width = 0.9479167F;
            // 
            // lblBumon
            // 
            this.lblBumon.Height = 0.375F;
            this.lblBumon.HyperLink = null;
            this.lblBumon.Left = 3.540158F;
            this.lblBumon.Name = "lblBumon";
            this.lblBumon.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblBumon.Text = "請求部門";
            this.lblBumon.Top = 0.9051182F;
            this.lblBumon.Width = 0.9479167F;
            // 
            // lblKubun
            // 
            this.lblKubun.Height = 0.375F;
            this.lblKubun.HyperLink = null;
            this.lblKubun.Left = 4.488189F;
            this.lblKubun.Name = "lblKubun";
            this.lblKubun.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblKubun.Text = "区分";
            this.lblKubun.Top = 0.9051182F;
            this.lblKubun.Width = 0.6307125F;
            // 
            // lblUncollectedAmountLast
            // 
            this.lblUncollectedAmountLast.Height = 0.375F;
            this.lblUncollectedAmountLast.HyperLink = null;
            this.lblUncollectedAmountLast.Left = 5.118898F;
            this.lblUncollectedAmountLast.Name = "lblUncollectedAmountLast";
            this.lblUncollectedAmountLast.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblUncollectedAmountLast.Text = "99月迄未回収";
            this.lblUncollectedAmountLast.Top = 0.9051182F;
            this.lblUncollectedAmountLast.Width = 0.92F;
            // 
            // lblUncollectAmount0
            // 
            this.lblUncollectAmount0.Height = 0.375F;
            this.lblUncollectAmount0.HyperLink = null;
            this.lblUncollectAmount0.Left = 6.038977F;
            this.lblUncollectAmount0.Name = "lblUncollectAmount0";
            this.lblUncollectAmount0.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblUncollectAmount0.Text = "99月";
            this.lblUncollectAmount0.Top = 0.9051182F;
            this.lblUncollectAmount0.Width = 0.92F;
            // 
            // lblUncollectAmount1
            // 
            this.lblUncollectAmount1.Height = 0.375F;
            this.lblUncollectAmount1.HyperLink = null;
            this.lblUncollectAmount1.Left = 6.959055F;
            this.lblUncollectAmount1.Name = "lblUncollectAmount1";
            this.lblUncollectAmount1.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblUncollectAmount1.Text = "99月";
            this.lblUncollectAmount1.Top = 0.9051182F;
            this.lblUncollectAmount1.Width = 0.92F;
            // 
            // lblUncollectAmount2
            // 
            this.lblUncollectAmount2.Height = 0.375F;
            this.lblUncollectAmount2.HyperLink = null;
            this.lblUncollectAmount2.Left = 7.879134F;
            this.lblUncollectAmount2.Name = "lblUncollectAmount2";
            this.lblUncollectAmount2.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblUncollectAmount2.Text = "99月";
            this.lblUncollectAmount2.Top = 0.9051182F;
            this.lblUncollectAmount2.Width = 0.92F;
            // 
            // lblUncollectAmount3
            // 
            this.lblUncollectAmount3.Height = 0.375F;
            this.lblUncollectAmount3.HyperLink = null;
            this.lblUncollectAmount3.Left = 8.799213F;
            this.lblUncollectAmount3.Name = "lblUncollectAmount3";
            this.lblUncollectAmount3.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblUncollectAmount3.Text = "99月以降";
            this.lblUncollectAmount3.Top = 0.9051182F;
            this.lblUncollectAmount3.Width = 0.92F;
            // 
            // lblKingakuK
            // 
            this.lblKingakuK.Height = 0.375F;
            this.lblKingakuK.HyperLink = null;
            this.lblKingakuK.Left = 9.719292F;
            this.lblKingakuK.Name = "lblKingakuK";
            this.lblKingakuK.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 9pt; text-align: cen" +
    "ter; vertical-align: middle; ddo-char-set: 128";
            this.lblKingakuK.Text = "合計";
            this.lblKingakuK.Top = 0.9051182F;
            this.lblKingakuK.Width = 0.92F;
            // 
            // lineHeaderTop
            // 
            this.lineHeaderTop.Height = 0F;
            this.lineHeaderTop.Left = 0F;
            this.lineHeaderTop.LineWeight = 1F;
            this.lineHeaderTop.Name = "lineHeaderTop";
            this.lineHeaderTop.Top = 0.905F;
            this.lineHeaderTop.Width = 10.62992F;
            this.lineHeaderTop.X1 = 0F;
            this.lineHeaderTop.X2 = 10.62992F;
            this.lineHeaderTop.Y1 = 0.905F;
            this.lineHeaderTop.Y2 = 0.905F;
            // 
            // lineHeaderBottom
            // 
            this.lineHeaderBottom.Height = 0F;
            this.lineHeaderBottom.Left = 0F;
            this.lineHeaderBottom.LineWeight = 1F;
            this.lineHeaderBottom.Name = "lineHeaderBottom";
            this.lineHeaderBottom.Top = 1.281F;
            this.lineHeaderBottom.Width = 10.62992F;
            this.lineHeaderBottom.X1 = 0F;
            this.lineHeaderBottom.X2 = 10.62992F;
            this.lineHeaderBottom.Y1 = 1.281F;
            this.lineHeaderBottom.Y2 = 1.281F;
            // 
            // lineHeaderV01
            // 
            this.lineHeaderV01.Height = 0.375F;
            this.lineHeaderV01.Left = 2.342126F;
            this.lineHeaderV01.LineWeight = 1F;
            this.lineHeaderV01.Name = "lineHeaderV01";
            this.lineHeaderV01.Top = 0.906F;
            this.lineHeaderV01.Width = 0F;
            this.lineHeaderV01.X1 = 2.342126F;
            this.lineHeaderV01.X2 = 2.342126F;
            this.lineHeaderV01.Y1 = 0.906F;
            this.lineHeaderV01.Y2 = 1.281F;
            // 
            // lineHeaderV02
            // 
            this.lineHeaderV02.Height = 0.375F;
            this.lineHeaderV02.Left = 2.592126F;
            this.lineHeaderV02.LineWeight = 1F;
            this.lineHeaderV02.Name = "lineHeaderV02";
            this.lineHeaderV02.Top = 0.905F;
            this.lineHeaderV02.Width = 0F;
            this.lineHeaderV02.X1 = 2.592126F;
            this.lineHeaderV02.X2 = 2.592126F;
            this.lineHeaderV02.Y1 = 0.905F;
            this.lineHeaderV02.Y2 = 1.28F;
            // 
            // lineHeaderV03
            // 
            this.lineHeaderV03.Height = 0.375F;
            this.lineHeaderV03.Left = 3.540158F;
            this.lineHeaderV03.LineWeight = 1F;
            this.lineHeaderV03.Name = "lineHeaderV03";
            this.lineHeaderV03.Top = 0.906F;
            this.lineHeaderV03.Width = 0F;
            this.lineHeaderV03.X1 = 3.540158F;
            this.lineHeaderV03.X2 = 3.540158F;
            this.lineHeaderV03.Y1 = 0.906F;
            this.lineHeaderV03.Y2 = 1.281F;
            // 
            // lineHeaderV04
            // 
            this.lineHeaderV04.Height = 0.3749998F;
            this.lineHeaderV04.Left = 4.488189F;
            this.lineHeaderV04.LineWeight = 1F;
            this.lineHeaderV04.Name = "lineHeaderV04";
            this.lineHeaderV04.Top = 0.9051182F;
            this.lineHeaderV04.Width = 0F;
            this.lineHeaderV04.X1 = 4.488189F;
            this.lineHeaderV04.X2 = 4.488189F;
            this.lineHeaderV04.Y1 = 0.9051182F;
            this.lineHeaderV04.Y2 = 1.280118F;
            // 
            // lineHeaderV05
            // 
            this.lineHeaderV05.Height = 0.3749998F;
            this.lineHeaderV05.Left = 5.118898F;
            this.lineHeaderV05.LineWeight = 1F;
            this.lineHeaderV05.Name = "lineHeaderV05";
            this.lineHeaderV05.Top = 0.9051182F;
            this.lineHeaderV05.Width = 0F;
            this.lineHeaderV05.X1 = 5.118898F;
            this.lineHeaderV05.X2 = 5.118898F;
            this.lineHeaderV05.Y1 = 0.9051182F;
            this.lineHeaderV05.Y2 = 1.280118F;
            // 
            // lineHeaderV06
            // 
            this.lineHeaderV06.Height = 0.3749998F;
            this.lineHeaderV06.Left = 5.138583F;
            this.lineHeaderV06.LineWeight = 1F;
            this.lineHeaderV06.Name = "lineHeaderV06";
            this.lineHeaderV06.Top = 0.9051182F;
            this.lineHeaderV06.Width = 0F;
            this.lineHeaderV06.X1 = 5.138583F;
            this.lineHeaderV06.X2 = 5.138583F;
            this.lineHeaderV06.Y1 = 0.9051182F;
            this.lineHeaderV06.Y2 = 1.280118F;
            // 
            // lineHeaderV07
            // 
            this.lineHeaderV07.Height = 0.3749998F;
            this.lineHeaderV07.Left = 6.038977F;
            this.lineHeaderV07.LineWeight = 1F;
            this.lineHeaderV07.Name = "lineHeaderV07";
            this.lineHeaderV07.Top = 0.9051182F;
            this.lineHeaderV07.Width = 0F;
            this.lineHeaderV07.X1 = 6.038977F;
            this.lineHeaderV07.X2 = 6.038977F;
            this.lineHeaderV07.Y1 = 0.9051182F;
            this.lineHeaderV07.Y2 = 1.280118F;
            // 
            // lineHeaderV09
            // 
            this.lineHeaderV09.Height = 0.3749998F;
            this.lineHeaderV09.Left = 6.959055F;
            this.lineHeaderV09.LineWeight = 1F;
            this.lineHeaderV09.Name = "lineHeaderV09";
            this.lineHeaderV09.Top = 0.9051182F;
            this.lineHeaderV09.Width = 0F;
            this.lineHeaderV09.X1 = 6.959055F;
            this.lineHeaderV09.X2 = 6.959055F;
            this.lineHeaderV09.Y1 = 0.9051182F;
            this.lineHeaderV09.Y2 = 1.280118F;
            // 
            // lineHeaderV10
            // 
            this.lineHeaderV10.Height = 0.3749998F;
            this.lineHeaderV10.Left = 7.879134F;
            this.lineHeaderV10.LineWeight = 1F;
            this.lineHeaderV10.Name = "lineHeaderV10";
            this.lineHeaderV10.Top = 0.9051182F;
            this.lineHeaderV10.Width = 0F;
            this.lineHeaderV10.X1 = 7.879134F;
            this.lineHeaderV10.X2 = 7.879134F;
            this.lineHeaderV10.Y1 = 0.9051182F;
            this.lineHeaderV10.Y2 = 1.280118F;
            // 
            // lineHeaderV11
            // 
            this.lineHeaderV11.Height = 0.3749998F;
            this.lineHeaderV11.Left = 8.799213F;
            this.lineHeaderV11.LineWeight = 1F;
            this.lineHeaderV11.Name = "lineHeaderV11";
            this.lineHeaderV11.Top = 0.9051182F;
            this.lineHeaderV11.Width = 0F;
            this.lineHeaderV11.X1 = 8.799213F;
            this.lineHeaderV11.X2 = 8.799213F;
            this.lineHeaderV11.Y1 = 0.9051182F;
            this.lineHeaderV11.Y2 = 1.280118F;
            // 
            // lineHeaderV12
            // 
            this.lineHeaderV12.Height = 0.3750004F;
            this.lineHeaderV12.Left = 9.719292F;
            this.lineHeaderV12.LineWeight = 1F;
            this.lineHeaderV12.Name = "lineHeaderV12";
            this.lineHeaderV12.Top = 0.9059056F;
            this.lineHeaderV12.Width = 0F;
            this.lineHeaderV12.X1 = 9.719292F;
            this.lineHeaderV12.X2 = 9.719292F;
            this.lineHeaderV12.Y1 = 0.9059056F;
            this.lineHeaderV12.Y2 = 1.280906F;
            // 
            // lineHeaderV13
            // 
            this.lineHeaderV13.Height = 0.3749998F;
            this.lineHeaderV13.Left = 9.73819F;
            this.lineHeaderV13.LineWeight = 1F;
            this.lineHeaderV13.Name = "lineHeaderV13";
            this.lineHeaderV13.Top = 0.9051182F;
            this.lineHeaderV13.Width = 0F;
            this.lineHeaderV13.X1 = 9.73819F;
            this.lineHeaderV13.X2 = 9.73819F;
            this.lineHeaderV13.Y1 = 0.9051182F;
            this.lineHeaderV13.Y2 = 1.280118F;
            // 
            // lineHeaderV08
            // 
            this.lineHeaderV08.Height = 0.3749997F;
            this.lineHeaderV08.Left = 6.058661F;
            this.lineHeaderV08.LineWeight = 1F;
            this.lineHeaderV08.Name = "lineHeaderV08";
            this.lineHeaderV08.Top = 0.9062993F;
            this.lineHeaderV08.Width = 0F;
            this.lineHeaderV08.X1 = 6.058661F;
            this.lineHeaderV08.X2 = 6.058661F;
            this.lineHeaderV08.Y1 = 0.9062993F;
            this.lineHeaderV08.Y2 = 1.281299F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCustomerCodeandName,
            this.txtClosingDay,
            this.txtStaffName,
            this.txtDepartment,
            this.txtCategoryName,
            this.txtUncollectedAmountLast,
            this.txtUncollectedAmount0,
            this.txtUncollectedAmount1,
            this.txtUncollectedAmount2,
            this.txtUncollectedAmount3,
            this.txtUncollectedAmountTotal,
            this.lineDV01,
            this.lineDV02,
            this.lineDV03,
            this.lineDV04,
            this.lineDV05,
            this.lineDV06,
            this.lineDV07,
            this.lineDV09,
            this.lineDV10,
            this.lineDV11,
            this.lineDV12,
            this.lineDV13,
            this.lineBottomDetail,
            this.lineDV08,
            this.lineDetailTop});
            this.detail.Height = 0.203937F;
            this.detail.KeepTogether = true;
            this.detail.Name = "detail";
            this.detail.Format += new System.EventHandler(this.detail_Format);
            // 
            // txtCustomerCodeandName
            // 
            this.txtCustomerCodeandName.CanGrow = false;
            this.txtCustomerCodeandName.DataField = "CustomerName     CustomerCode";
            this.txtCustomerCodeandName.Height = 0.202F;
            this.txtCustomerCodeandName.Left = 0F;
            this.txtCustomerCodeandName.MultiLine = false;
            this.txtCustomerCodeandName.Name = "txtCustomerCodeandName";
            this.txtCustomerCodeandName.OutputFormat = resources.GetString("txtCustomerCodeandName.OutputFormat");
            this.txtCustomerCodeandName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtCustomerCodeandName.Style = "font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: left; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtCustomerCodeandName.Text = "ＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＥ";
            this.txtCustomerCodeandName.Top = 0F;
            this.txtCustomerCodeandName.Width = 2.342126F;
            // 
            // txtClosingDay
            // 
            this.txtClosingDay.DataField = "ClosingDay";
            this.txtClosingDay.Height = 0.202F;
            this.txtClosingDay.HyperLink = null;
            this.txtClosingDay.Left = 2.342126F;
            this.txtClosingDay.Name = "txtClosingDay";
            this.txtClosingDay.Style = "font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: center; vertical-align: middle;" +
    " ddo-char-set: 1";
            this.txtClosingDay.Text = "99";
            this.txtClosingDay.Top = 0F;
            this.txtClosingDay.Width = 0.25F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.DataField = "StaffName";
            this.txtStaffName.Height = 0.202F;
            this.txtStaffName.Left = 2.592126F;
            this.txtStaffName.MultiLine = false;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.OutputFormat = resources.GetString("txtStaffName.OutputFormat");
            this.txtStaffName.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtStaffName.Style = "font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: left; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtStaffName.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            this.txtStaffName.Text = "WWWWWWWWWWWW";
            this.txtStaffName.Top = 0F;
            this.txtStaffName.Width = 0.9479167F;
            // 
            // txtDepartment
            // 
            this.txtDepartment.DataField = "DepartmentName";
            this.txtDepartment.Height = 0.202F;
            this.txtDepartment.Left = 3.540158F;
            this.txtDepartment.MultiLine = false;
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.OutputFormat = resources.GetString("txtDepartment.OutputFormat");
            this.txtDepartment.Padding = new GrapeCity.ActiveReports.PaddingEx(1, 0, 0, 0);
            this.txtDepartment.Style = "font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: left; vertical-align: middle; d" +
    "do-char-set: 1";
            this.txtDepartment.Text = "WWWWWWWWWWWW";
            this.txtDepartment.Top = 0F;
            this.txtDepartment.Width = 0.9479167F;
            // 
            // txtCategoryName
            // 
            this.txtCategoryName.DataField = "CategoryName";
            this.txtCategoryName.Height = 0.202F;
            this.txtCategoryName.Left = 4.488189F;
            this.txtCategoryName.MultiLine = false;
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.OutputFormat = resources.GetString("txtCategoryName.OutputFormat");
            this.txtCategoryName.Style = "font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: center; vertical-align: middle;" +
    " ddo-char-set: 1";
            this.txtCategoryName.Text = "NNNNNNNN";
            this.txtCategoryName.Top = 0F;
            this.txtCategoryName.Width = 0.6307125F;
            // 
            // txtUncollectedAmountLast
            // 
            this.txtUncollectedAmountLast.DataField = "UncollectedAmountLast";
            this.txtUncollectedAmountLast.Height = 0.202F;
            this.txtUncollectedAmountLast.Left = 5.118898F;
            this.txtUncollectedAmountLast.MultiLine = false;
            this.txtUncollectedAmountLast.Name = "txtUncollectedAmountLast";
            this.txtUncollectedAmountLast.OutputFormat = resources.GetString("txtUncollectedAmountLast.OutputFormat");
            this.txtUncollectedAmountLast.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmountLast.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmountLast.Text = "9,999,999,999,999";
            this.txtUncollectedAmountLast.Top = 0F;
            this.txtUncollectedAmountLast.Width = 0.92F;
            // 
            // txtUncollectedAmount0
            // 
            this.txtUncollectedAmount0.DataField = "UncollectedAmount0";
            this.txtUncollectedAmount0.Height = 0.202F;
            this.txtUncollectedAmount0.Left = 6.038977F;
            this.txtUncollectedAmount0.MultiLine = false;
            this.txtUncollectedAmount0.Name = "txtUncollectedAmount0";
            this.txtUncollectedAmount0.OutputFormat = resources.GetString("txtUncollectedAmount0.OutputFormat");
            this.txtUncollectedAmount0.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmount0.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmount0.Text = "9,999,999,999,999";
            this.txtUncollectedAmount0.Top = 0F;
            this.txtUncollectedAmount0.Width = 0.92F;
            // 
            // txtUncollectedAmount1
            // 
            this.txtUncollectedAmount1.DataField = "UncollectedAmount1";
            this.txtUncollectedAmount1.Height = 0.202F;
            this.txtUncollectedAmount1.Left = 6.959055F;
            this.txtUncollectedAmount1.MultiLine = false;
            this.txtUncollectedAmount1.Name = "txtUncollectedAmount1";
            this.txtUncollectedAmount1.OutputFormat = resources.GetString("txtUncollectedAmount1.OutputFormat");
            this.txtUncollectedAmount1.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmount1.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmount1.Text = "9,999,999,999,999";
            this.txtUncollectedAmount1.Top = 0F;
            this.txtUncollectedAmount1.Width = 0.92F;
            // 
            // txtUncollectedAmount2
            // 
            this.txtUncollectedAmount2.DataField = "UncollectedAmount2";
            this.txtUncollectedAmount2.Height = 0.202F;
            this.txtUncollectedAmount2.Left = 7.879134F;
            this.txtUncollectedAmount2.MultiLine = false;
            this.txtUncollectedAmount2.Name = "txtUncollectedAmount2";
            this.txtUncollectedAmount2.OutputFormat = resources.GetString("txtUncollectedAmount2.OutputFormat");
            this.txtUncollectedAmount2.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmount2.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmount2.Text = "9,999,999,999,999";
            this.txtUncollectedAmount2.Top = 0F;
            this.txtUncollectedAmount2.Width = 0.92F;
            // 
            // txtUncollectedAmount3
            // 
            this.txtUncollectedAmount3.DataField = "UncollectedAmount3";
            this.txtUncollectedAmount3.Height = 0.202F;
            this.txtUncollectedAmount3.Left = 8.799213F;
            this.txtUncollectedAmount3.MultiLine = false;
            this.txtUncollectedAmount3.Name = "txtUncollectedAmount3";
            this.txtUncollectedAmount3.OutputFormat = resources.GetString("txtUncollectedAmount3.OutputFormat");
            this.txtUncollectedAmount3.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmount3.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmount3.Text = "9,999,999,999,999";
            this.txtUncollectedAmount3.Top = 0F;
            this.txtUncollectedAmount3.Width = 0.92F;
            // 
            // txtUncollectedAmountTotal
            // 
            this.txtUncollectedAmountTotal.DataField = "UncollectedAmountTotal";
            this.txtUncollectedAmountTotal.Height = 0.202F;
            this.txtUncollectedAmountTotal.Left = 9.719292F;
            this.txtUncollectedAmountTotal.MultiLine = false;
            this.txtUncollectedAmountTotal.Name = "txtUncollectedAmountTotal";
            this.txtUncollectedAmountTotal.OutputFormat = resources.GetString("txtUncollectedAmountTotal.OutputFormat");
            this.txtUncollectedAmountTotal.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmountTotal.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmountTotal.Text = "9,999,999,999,999";
            this.txtUncollectedAmountTotal.Top = 0F;
            this.txtUncollectedAmountTotal.Width = 0.92F;
            // 
            // lineDV01
            // 
            this.lineDV01.AnchorBottom = true;
            this.lineDV01.Height = 0.202F;
            this.lineDV01.Left = 2.342126F;
            this.lineDV01.LineWeight = 1F;
            this.lineDV01.Name = "lineDV01";
            this.lineDV01.Top = 0F;
            this.lineDV01.Width = 0F;
            this.lineDV01.X1 = 2.342126F;
            this.lineDV01.X2 = 2.342126F;
            this.lineDV01.Y1 = 0F;
            this.lineDV01.Y2 = 0.202F;
            // 
            // lineDV02
            // 
            this.lineDV02.AnchorBottom = true;
            this.lineDV02.Height = 0.202F;
            this.lineDV02.Left = 2.592126F;
            this.lineDV02.LineWeight = 1F;
            this.lineDV02.Name = "lineDV02";
            this.lineDV02.Top = 0F;
            this.lineDV02.Width = 0F;
            this.lineDV02.X1 = 2.592126F;
            this.lineDV02.X2 = 2.592126F;
            this.lineDV02.Y1 = 0F;
            this.lineDV02.Y2 = 0.202F;
            // 
            // lineDV03
            // 
            this.lineDV03.AnchorBottom = true;
            this.lineDV03.Height = 0.202F;
            this.lineDV03.Left = 3.540158F;
            this.lineDV03.LineWeight = 1F;
            this.lineDV03.Name = "lineDV03";
            this.lineDV03.Top = 0F;
            this.lineDV03.Width = 0F;
            this.lineDV03.X1 = 3.540158F;
            this.lineDV03.X2 = 3.540158F;
            this.lineDV03.Y1 = 0F;
            this.lineDV03.Y2 = 0.202F;
            // 
            // lineDV04
            // 
            this.lineDV04.AnchorBottom = true;
            this.lineDV04.Height = 0.202F;
            this.lineDV04.Left = 4.488189F;
            this.lineDV04.LineWeight = 1F;
            this.lineDV04.Name = "lineDV04";
            this.lineDV04.Top = 0F;
            this.lineDV04.Width = 0F;
            this.lineDV04.X1 = 4.488189F;
            this.lineDV04.X2 = 4.488189F;
            this.lineDV04.Y1 = 0F;
            this.lineDV04.Y2 = 0.202F;
            // 
            // lineDV05
            // 
            this.lineDV05.AnchorBottom = true;
            this.lineDV05.Height = 0.202F;
            this.lineDV05.Left = 5.118898F;
            this.lineDV05.LineWeight = 1F;
            this.lineDV05.Name = "lineDV05";
            this.lineDV05.Top = 0F;
            this.lineDV05.Width = 0F;
            this.lineDV05.X1 = 5.118898F;
            this.lineDV05.X2 = 5.118898F;
            this.lineDV05.Y1 = 0F;
            this.lineDV05.Y2 = 0.202F;
            // 
            // lineDV06
            // 
            this.lineDV06.AnchorBottom = true;
            this.lineDV06.Height = 0.2019685F;
            this.lineDV06.Left = 5.138583F;
            this.lineDV06.LineWeight = 1F;
            this.lineDV06.Name = "lineDV06";
            this.lineDV06.Top = 0F;
            this.lineDV06.Width = 0F;
            this.lineDV06.X1 = 5.138583F;
            this.lineDV06.X2 = 5.138583F;
            this.lineDV06.Y1 = 0F;
            this.lineDV06.Y2 = 0.2019685F;
            // 
            // lineDV07
            // 
            this.lineDV07.AnchorBottom = true;
            this.lineDV07.Height = 0.202F;
            this.lineDV07.Left = 6.038977F;
            this.lineDV07.LineWeight = 1F;
            this.lineDV07.Name = "lineDV07";
            this.lineDV07.Top = 0F;
            this.lineDV07.Width = 0F;
            this.lineDV07.X1 = 6.038977F;
            this.lineDV07.X2 = 6.038977F;
            this.lineDV07.Y1 = 0F;
            this.lineDV07.Y2 = 0.202F;
            // 
            // lineDV09
            // 
            this.lineDV09.AnchorBottom = true;
            this.lineDV09.Height = 0.202F;
            this.lineDV09.Left = 6.959055F;
            this.lineDV09.LineWeight = 1F;
            this.lineDV09.Name = "lineDV09";
            this.lineDV09.Top = 0F;
            this.lineDV09.Width = 0F;
            this.lineDV09.X1 = 6.959055F;
            this.lineDV09.X2 = 6.959055F;
            this.lineDV09.Y1 = 0F;
            this.lineDV09.Y2 = 0.202F;
            // 
            // lineDV10
            // 
            this.lineDV10.AnchorBottom = true;
            this.lineDV10.Height = 0.202F;
            this.lineDV10.Left = 7.879134F;
            this.lineDV10.LineWeight = 1F;
            this.lineDV10.Name = "lineDV10";
            this.lineDV10.Top = 0.001574803F;
            this.lineDV10.Width = 0F;
            this.lineDV10.X1 = 7.879134F;
            this.lineDV10.X2 = 7.879134F;
            this.lineDV10.Y1 = 0.001574803F;
            this.lineDV10.Y2 = 0.2035748F;
            // 
            // lineDV11
            // 
            this.lineDV11.AnchorBottom = true;
            this.lineDV11.Height = 0.202F;
            this.lineDV11.Left = 8.799213F;
            this.lineDV11.LineWeight = 1F;
            this.lineDV11.Name = "lineDV11";
            this.lineDV11.Top = 0F;
            this.lineDV11.Width = 0F;
            this.lineDV11.X1 = 8.799213F;
            this.lineDV11.X2 = 8.799213F;
            this.lineDV11.Y1 = 0F;
            this.lineDV11.Y2 = 0.202F;
            // 
            // lineDV12
            // 
            this.lineDV12.AnchorBottom = true;
            this.lineDV12.Height = 0.202F;
            this.lineDV12.Left = 9.719292F;
            this.lineDV12.LineWeight = 1F;
            this.lineDV12.Name = "lineDV12";
            this.lineDV12.Top = 0F;
            this.lineDV12.Width = 0F;
            this.lineDV12.X1 = 9.719292F;
            this.lineDV12.X2 = 9.719292F;
            this.lineDV12.Y1 = 0F;
            this.lineDV12.Y2 = 0.202F;
            // 
            // lineDV13
            // 
            this.lineDV13.AnchorBottom = true;
            this.lineDV13.Height = 0.2019685F;
            this.lineDV13.Left = 9.73819F;
            this.lineDV13.LineWeight = 1F;
            this.lineDV13.Name = "lineDV13";
            this.lineDV13.Top = 0F;
            this.lineDV13.Width = 0F;
            this.lineDV13.X1 = 9.73819F;
            this.lineDV13.X2 = 9.73819F;
            this.lineDV13.Y1 = 0F;
            this.lineDV13.Y2 = 0.2019685F;
            // 
            // lineBottomDetail
            // 
            this.lineBottomDetail.Height = 0F;
            this.lineBottomDetail.Left = 0F;
            this.lineBottomDetail.LineWeight = 1F;
            this.lineBottomDetail.Name = "lineBottomDetail";
            this.lineBottomDetail.Top = 0.202F;
            this.lineBottomDetail.Width = 10.62992F;
            this.lineBottomDetail.X1 = 0F;
            this.lineBottomDetail.X2 = 10.62992F;
            this.lineBottomDetail.Y1 = 0.202F;
            this.lineBottomDetail.Y2 = 0.202F;
            // 
            // lineDV08
            // 
            this.lineDV08.AnchorBottom = true;
            this.lineDV08.Height = 0.202F;
            this.lineDV08.Left = 6.058662F;
            this.lineDV08.LineWeight = 1F;
            this.lineDV08.Name = "lineDV08";
            this.lineDV08.Top = 0F;
            this.lineDV08.Width = 0F;
            this.lineDV08.X1 = 6.058662F;
            this.lineDV08.X2 = 6.058662F;
            this.lineDV08.Y1 = 0F;
            this.lineDV08.Y2 = 0.202F;
            // 
            // lineDetailTop
            // 
            this.lineDetailTop.Height = 0F;
            this.lineDetailTop.Left = 0F;
            this.lineDetailTop.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot;
            this.lineDetailTop.LineWeight = 1F;
            this.lineDetailTop.Name = "lineDetailTop";
            this.lineDetailTop.Top = 0F;
            this.lineDetailTop.Visible = false;
            this.lineDetailTop.Width = 10.62992F;
            this.lineDetailTop.X1 = 0F;
            this.lineDetailTop.X2 = 10.62992F;
            this.lineDetailTop.Y1 = 0F;
            this.lineDetailTop.Y2 = 0F;
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
            this.reportInfo1.Left = 7.480316F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.07874016F;
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
            // grhGrandTotal
            // 
            this.grhGrandTotal.Height = 0F;
            this.grhGrandTotal.Name = "grhGrandTotal";
            // 
            // grfGrandTotal
            // 
            this.grfGrandTotal.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblGrandTotalBack,
            this.lblGrandTotalCaption,
            this.txtUncollectedAmountLastSum,
            this.txtUncollectedAmount0Sum,
            this.txtUncollectedAmount1Sum,
            this.txtUncollectedAmount2Sum,
            this.txtUncollectedAmount3Sum,
            this.txtUncollectedAmountTotalSum,
            this.lineBottomGrandTotal,
            this.lineGTV01,
            this.lineGTV02,
            this.lineGTV03,
            this.lineGTV04,
            this.lineGTV05,
            this.lineGTV06,
            this.lineGTV07,
            this.lineGTV08,
            this.lineGTV09});
            this.grfGrandTotal.Height = 0.203937F;
            this.grfGrandTotal.Name = "grfGrandTotal";
            // 
            // lblGrandTotalBack
            // 
            this.lblGrandTotalBack.Height = 0.2019685F;
            this.lblGrandTotalBack.HyperLink = null;
            this.lblGrandTotalBack.Left = 0F;
            this.lblGrandTotalBack.Name = "lblGrandTotalBack";
            this.lblGrandTotalBack.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblGrandTotalBack.Text = "";
            this.lblGrandTotalBack.Top = 0F;
            this.lblGrandTotalBack.Width = 5.118897F;
            // 
            // lblGrandTotalCaption
            // 
            this.lblGrandTotalCaption.Height = 0.2019685F;
            this.lblGrandTotalCaption.HyperLink = null;
            this.lblGrandTotalCaption.Left = 0F;
            this.lblGrandTotalCaption.Name = "lblGrandTotalCaption";
            this.lblGrandTotalCaption.Style = "font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: center; vertical-align: middle;" +
    " ddo-char-set: 1";
            this.lblGrandTotalCaption.Text = "総合計";
            this.lblGrandTotalCaption.Top = 0F;
            this.lblGrandTotalCaption.Width = 4.488189F;
            // 
            // txtUncollectedAmountLastSum
            // 
            this.txtUncollectedAmountLastSum.DataField = "UncollectedAmountLast";
            this.txtUncollectedAmountLastSum.Height = 0.202F;
            this.txtUncollectedAmountLastSum.Left = 5.118898F;
            this.txtUncollectedAmountLastSum.MultiLine = false;
            this.txtUncollectedAmountLastSum.Name = "txtUncollectedAmountLastSum";
            this.txtUncollectedAmountLastSum.OutputFormat = resources.GetString("txtUncollectedAmountLastSum.OutputFormat");
            this.txtUncollectedAmountLastSum.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmountLastSum.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmountLastSum.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtUncollectedAmountLastSum.Text = "9,999,999,999,999";
            this.txtUncollectedAmountLastSum.Top = 0F;
            this.txtUncollectedAmountLastSum.Width = 0.92F;
            // 
            // txtUncollectedAmount0Sum
            // 
            this.txtUncollectedAmount0Sum.DataField = "UncollectedAmount0";
            this.txtUncollectedAmount0Sum.Height = 0.202F;
            this.txtUncollectedAmount0Sum.Left = 6.038977F;
            this.txtUncollectedAmount0Sum.MultiLine = false;
            this.txtUncollectedAmount0Sum.Name = "txtUncollectedAmount0Sum";
            this.txtUncollectedAmount0Sum.OutputFormat = resources.GetString("txtUncollectedAmount0Sum.OutputFormat");
            this.txtUncollectedAmount0Sum.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmount0Sum.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmount0Sum.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtUncollectedAmount0Sum.Text = "9,999,999,999,999";
            this.txtUncollectedAmount0Sum.Top = 0F;
            this.txtUncollectedAmount0Sum.Width = 0.92F;
            // 
            // txtUncollectedAmount1Sum
            // 
            this.txtUncollectedAmount1Sum.DataField = "UncollectedAmount1";
            this.txtUncollectedAmount1Sum.Height = 0.202F;
            this.txtUncollectedAmount1Sum.Left = 6.959055F;
            this.txtUncollectedAmount1Sum.MultiLine = false;
            this.txtUncollectedAmount1Sum.Name = "txtUncollectedAmount1Sum";
            this.txtUncollectedAmount1Sum.OutputFormat = resources.GetString("txtUncollectedAmount1Sum.OutputFormat");
            this.txtUncollectedAmount1Sum.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmount1Sum.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmount1Sum.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtUncollectedAmount1Sum.Text = "9,999,999,999,999";
            this.txtUncollectedAmount1Sum.Top = 0F;
            this.txtUncollectedAmount1Sum.Width = 0.92F;
            // 
            // txtUncollectedAmount2Sum
            // 
            this.txtUncollectedAmount2Sum.DataField = "UncollectedAmount2";
            this.txtUncollectedAmount2Sum.Height = 0.202F;
            this.txtUncollectedAmount2Sum.Left = 7.879134F;
            this.txtUncollectedAmount2Sum.MultiLine = false;
            this.txtUncollectedAmount2Sum.Name = "txtUncollectedAmount2Sum";
            this.txtUncollectedAmount2Sum.OutputFormat = resources.GetString("txtUncollectedAmount2Sum.OutputFormat");
            this.txtUncollectedAmount2Sum.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmount2Sum.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmount2Sum.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtUncollectedAmount2Sum.Text = "9,999,999,999,999";
            this.txtUncollectedAmount2Sum.Top = 0F;
            this.txtUncollectedAmount2Sum.Width = 0.92F;
            // 
            // txtUncollectedAmount3Sum
            // 
            this.txtUncollectedAmount3Sum.DataField = "UncollectedAmount3";
            this.txtUncollectedAmount3Sum.Height = 0.202F;
            this.txtUncollectedAmount3Sum.Left = 8.799213F;
            this.txtUncollectedAmount3Sum.MultiLine = false;
            this.txtUncollectedAmount3Sum.Name = "txtUncollectedAmount3Sum";
            this.txtUncollectedAmount3Sum.OutputFormat = resources.GetString("txtUncollectedAmount3Sum.OutputFormat");
            this.txtUncollectedAmount3Sum.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmount3Sum.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmount3Sum.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtUncollectedAmount3Sum.Text = "9,999,999,999,999";
            this.txtUncollectedAmount3Sum.Top = 0F;
            this.txtUncollectedAmount3Sum.Width = 0.92F;
            // 
            // txtUncollectedAmountTotalSum
            // 
            this.txtUncollectedAmountTotalSum.DataField = "UncollectedAmountTotal";
            this.txtUncollectedAmountTotalSum.Height = 0.202F;
            this.txtUncollectedAmountTotalSum.Left = 9.719292F;
            this.txtUncollectedAmountTotalSum.MultiLine = false;
            this.txtUncollectedAmountTotalSum.Name = "txtUncollectedAmountTotalSum";
            this.txtUncollectedAmountTotalSum.OutputFormat = resources.GetString("txtUncollectedAmountTotalSum.OutputFormat");
            this.txtUncollectedAmountTotalSum.Padding = new GrapeCity.ActiveReports.PaddingEx(0, 0, 1, 0);
            this.txtUncollectedAmountTotalSum.Style = "font-family: ＭＳ 明朝; font-size: 7pt; text-align: right; vertical-align: middle; dd" +
    "o-char-set: 1";
            this.txtUncollectedAmountTotalSum.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtUncollectedAmountTotalSum.Text = "9,999,999,999,999";
            this.txtUncollectedAmountTotalSum.Top = 0F;
            this.txtUncollectedAmountTotalSum.Width = 0.92F;
            // 
            // lineBottomGrandTotal
            // 
            this.lineBottomGrandTotal.Height = 0F;
            this.lineBottomGrandTotal.Left = 0.004F;
            this.lineBottomGrandTotal.LineWeight = 0.5F;
            this.lineBottomGrandTotal.Name = "lineBottomGrandTotal";
            this.lineBottomGrandTotal.Top = 0.202F;
            this.lineBottomGrandTotal.Width = 10.62592F;
            this.lineBottomGrandTotal.X1 = 0.004F;
            this.lineBottomGrandTotal.X2 = 10.62992F;
            this.lineBottomGrandTotal.Y1 = 0.202F;
            this.lineBottomGrandTotal.Y2 = 0.202F;
            // 
            // lineGTV01
            // 
            this.lineGTV01.Height = 0.2019685F;
            this.lineGTV01.Left = 5.118898F;
            this.lineGTV01.LineWeight = 1F;
            this.lineGTV01.Name = "lineGTV01";
            this.lineGTV01.Top = -7.450581E-09F;
            this.lineGTV01.Width = 0F;
            this.lineGTV01.X1 = 5.118898F;
            this.lineGTV01.X2 = 5.118898F;
            this.lineGTV01.Y1 = -7.450581E-09F;
            this.lineGTV01.Y2 = 0.2019685F;
            // 
            // lineGTV02
            // 
            this.lineGTV02.Height = 0.2019685F;
            this.lineGTV02.Left = 5.138583F;
            this.lineGTV02.LineWeight = 1F;
            this.lineGTV02.Name = "lineGTV02";
            this.lineGTV02.Top = 0F;
            this.lineGTV02.Width = 0F;
            this.lineGTV02.X1 = 5.138583F;
            this.lineGTV02.X2 = 5.138583F;
            this.lineGTV02.Y1 = 0F;
            this.lineGTV02.Y2 = 0.2019685F;
            // 
            // lineGTV03
            // 
            this.lineGTV03.Height = 0.2019685F;
            this.lineGTV03.Left = 6.038977F;
            this.lineGTV03.LineWeight = 1F;
            this.lineGTV03.Name = "lineGTV03";
            this.lineGTV03.Top = 0F;
            this.lineGTV03.Width = 0F;
            this.lineGTV03.X1 = 6.038977F;
            this.lineGTV03.X2 = 6.038977F;
            this.lineGTV03.Y1 = 0F;
            this.lineGTV03.Y2 = 0.2019685F;
            // 
            // lineGTV04
            // 
            this.lineGTV04.Height = 0.2019685F;
            this.lineGTV04.Left = 6.058662F;
            this.lineGTV04.LineWeight = 1F;
            this.lineGTV04.Name = "lineGTV04";
            this.lineGTV04.Top = 0F;
            this.lineGTV04.Width = 0F;
            this.lineGTV04.X1 = 6.058662F;
            this.lineGTV04.X2 = 6.058662F;
            this.lineGTV04.Y1 = 0F;
            this.lineGTV04.Y2 = 0.2019685F;
            // 
            // lineGTV05
            // 
            this.lineGTV05.Height = 0.2019685F;
            this.lineGTV05.Left = 6.959055F;
            this.lineGTV05.LineWeight = 1F;
            this.lineGTV05.Name = "lineGTV05";
            this.lineGTV05.Top = 0F;
            this.lineGTV05.Width = 0F;
            this.lineGTV05.X1 = 6.959055F;
            this.lineGTV05.X2 = 6.959055F;
            this.lineGTV05.Y1 = 0F;
            this.lineGTV05.Y2 = 0.2019685F;
            // 
            // lineGTV06
            // 
            this.lineGTV06.Height = 0.2019685F;
            this.lineGTV06.Left = 7.879134F;
            this.lineGTV06.LineWeight = 1F;
            this.lineGTV06.Name = "lineGTV06";
            this.lineGTV06.Top = 0F;
            this.lineGTV06.Width = 0F;
            this.lineGTV06.X1 = 7.879134F;
            this.lineGTV06.X2 = 7.879134F;
            this.lineGTV06.Y1 = 0F;
            this.lineGTV06.Y2 = 0.2019685F;
            // 
            // lineGTV07
            // 
            this.lineGTV07.Height = 0.2019685F;
            this.lineGTV07.Left = 8.799213F;
            this.lineGTV07.LineWeight = 1F;
            this.lineGTV07.Name = "lineGTV07";
            this.lineGTV07.Top = 0F;
            this.lineGTV07.Width = 0F;
            this.lineGTV07.X1 = 8.799213F;
            this.lineGTV07.X2 = 8.799213F;
            this.lineGTV07.Y1 = 0F;
            this.lineGTV07.Y2 = 0.2019685F;
            // 
            // lineGTV08
            // 
            this.lineGTV08.Height = 0.2019685F;
            this.lineGTV08.Left = 9.719292F;
            this.lineGTV08.LineWeight = 1F;
            this.lineGTV08.Name = "lineGTV08";
            this.lineGTV08.Top = 0F;
            this.lineGTV08.Width = 0F;
            this.lineGTV08.X1 = 9.719292F;
            this.lineGTV08.X2 = 9.719292F;
            this.lineGTV08.Y1 = 0F;
            this.lineGTV08.Y2 = 0.2019685F;
            // 
            // lineGTV09
            // 
            this.lineGTV09.Height = 0.2019685F;
            this.lineGTV09.Left = 9.73819F;
            this.lineGTV09.LineWeight = 1F;
            this.lineGTV09.Name = "lineGTV09";
            this.lineGTV09.Top = 0F;
            this.lineGTV09.Width = 0F;
            this.lineGTV09.X1 = 9.73819F;
            this.lineGTV09.X2 = 9.73819F;
            this.lineGTV09.Y1 = 0F;
            this.lineGTV09.Y2 = 0.2019685F;
            // 
            // grhTotal
            // 
            this.grhTotal.Height = 0F;
            this.grhTotal.Name = "grhTotal";
            // 
            // grfTotal
            // 
            this.grfTotal.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCaptionTotal,
            this.lineBottomTotal,
            this.lineVT0,
            this.lineVT2,
            this.lineVT1,
            this.lineVT3,
            this.lineVT4,
            this.lineVT5,
            this.lineVT6,
            this.lineVT7,
            this.lineVT8,
            this.lineVT9});
            this.grfTotal.Height = 0.2019685F;
            this.grfTotal.Name = "grfTotal";
            // 
            // lblCaptionTotal
            // 
            this.lblCaptionTotal.Height = 0.2019685F;
            this.lblCaptionTotal.HyperLink = null;
            this.lblCaptionTotal.Left = 0F;
            this.lblCaptionTotal.Name = "lblCaptionTotal";
            this.lblCaptionTotal.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblCaptionTotal.Text = "合計";
            this.lblCaptionTotal.Top = 0F;
            this.lblCaptionTotal.Width = 4.488189F;
            // 
            // lineBottomTotal
            // 
            this.lineBottomTotal.Height = 0F;
            this.lineBottomTotal.Left = 0F;
            this.lineBottomTotal.LineWeight = 0.5F;
            this.lineBottomTotal.Name = "lineBottomTotal";
            this.lineBottomTotal.Top = 0.202F;
            this.lineBottomTotal.Width = 10.62992F;
            this.lineBottomTotal.X1 = 0F;
            this.lineBottomTotal.X2 = 10.62992F;
            this.lineBottomTotal.Y1 = 0.202F;
            this.lineBottomTotal.Y2 = 0.202F;
            // 
            // lineVT0
            // 
            this.lineVT0.Height = 0.2019685F;
            this.lineVT0.Left = 4.488189F;
            this.lineVT0.LineWeight = 1F;
            this.lineVT0.Name = "lineVT0";
            this.lineVT0.Top = 0F;
            this.lineVT0.Width = 0F;
            this.lineVT0.X1 = 4.488189F;
            this.lineVT0.X2 = 4.488189F;
            this.lineVT0.Y1 = 0F;
            this.lineVT0.Y2 = 0.2019685F;
            // 
            // lineVT2
            // 
            this.lineVT2.Height = 0.2019685F;
            this.lineVT2.Left = 5.138583F;
            this.lineVT2.LineWeight = 1F;
            this.lineVT2.Name = "lineVT2";
            this.lineVT2.Top = 0F;
            this.lineVT2.Width = 0F;
            this.lineVT2.X1 = 5.138583F;
            this.lineVT2.X2 = 5.138583F;
            this.lineVT2.Y1 = 0F;
            this.lineVT2.Y2 = 0.2019685F;
            // 
            // lineVT1
            // 
            this.lineVT1.Height = 0.2019685F;
            this.lineVT1.Left = 5.118898F;
            this.lineVT1.LineWeight = 1F;
            this.lineVT1.Name = "lineVT1";
            this.lineVT1.Top = 0F;
            this.lineVT1.Width = 0F;
            this.lineVT1.X1 = 5.118898F;
            this.lineVT1.X2 = 5.118898F;
            this.lineVT1.Y1 = 0F;
            this.lineVT1.Y2 = 0.2019685F;
            // 
            // lineVT3
            // 
            this.lineVT3.Height = 0.2019685F;
            this.lineVT3.Left = 6.038977F;
            this.lineVT3.LineWeight = 1F;
            this.lineVT3.Name = "lineVT3";
            this.lineVT3.Top = 0F;
            this.lineVT3.Width = 0F;
            this.lineVT3.X1 = 6.038977F;
            this.lineVT3.X2 = 6.038977F;
            this.lineVT3.Y1 = 0F;
            this.lineVT3.Y2 = 0.2019685F;
            // 
            // lineVT4
            // 
            this.lineVT4.Height = 0.2019685F;
            this.lineVT4.Left = 6.058662F;
            this.lineVT4.LineWeight = 1F;
            this.lineVT4.Name = "lineVT4";
            this.lineVT4.Top = 0F;
            this.lineVT4.Width = 0F;
            this.lineVT4.X1 = 6.058662F;
            this.lineVT4.X2 = 6.058662F;
            this.lineVT4.Y1 = 0F;
            this.lineVT4.Y2 = 0.2019685F;
            // 
            // lineVT5
            // 
            this.lineVT5.Height = 0.2019685F;
            this.lineVT5.Left = 6.959055F;
            this.lineVT5.LineWeight = 1F;
            this.lineVT5.Name = "lineVT5";
            this.lineVT5.Top = 0F;
            this.lineVT5.Width = 0F;
            this.lineVT5.X1 = 6.959055F;
            this.lineVT5.X2 = 6.959055F;
            this.lineVT5.Y1 = 0F;
            this.lineVT5.Y2 = 0.2019685F;
            // 
            // lineVT6
            // 
            this.lineVT6.Height = 0.2019685F;
            this.lineVT6.Left = 7.879134F;
            this.lineVT6.LineWeight = 1F;
            this.lineVT6.Name = "lineVT6";
            this.lineVT6.Top = 0F;
            this.lineVT6.Width = 0F;
            this.lineVT6.X1 = 7.879134F;
            this.lineVT6.X2 = 7.879134F;
            this.lineVT6.Y1 = 0F;
            this.lineVT6.Y2 = 0.2019685F;
            // 
            // lineVT7
            // 
            this.lineVT7.Height = 0.2019685F;
            this.lineVT7.Left = 8.799213F;
            this.lineVT7.LineWeight = 1F;
            this.lineVT7.Name = "lineVT7";
            this.lineVT7.Top = 0F;
            this.lineVT7.Width = 0F;
            this.lineVT7.X1 = 8.799213F;
            this.lineVT7.X2 = 8.799213F;
            this.lineVT7.Y1 = 0F;
            this.lineVT7.Y2 = 0.2019685F;
            // 
            // lineVT8
            // 
            this.lineVT8.Height = 0.2019685F;
            this.lineVT8.Left = 9.719292F;
            this.lineVT8.LineWeight = 1F;
            this.lineVT8.Name = "lineVT8";
            this.lineVT8.Top = 0F;
            this.lineVT8.Width = 0F;
            this.lineVT8.X1 = 9.719292F;
            this.lineVT8.X2 = 9.719292F;
            this.lineVT8.Y1 = 0F;
            this.lineVT8.Y2 = 0.2019685F;
            // 
            // lineVT9
            // 
            this.lineVT9.Height = 0.2019685F;
            this.lineVT9.Left = 9.73819F;
            this.lineVT9.LineWeight = 1F;
            this.lineVT9.Name = "lineVT9";
            this.lineVT9.Top = 0F;
            this.lineVT9.Width = 0F;
            this.lineVT9.X1 = 9.73819F;
            this.lineVT9.X2 = 9.73819F;
            this.lineVT9.Y1 = 0F;
            this.lineVT9.Y2 = 0.2019685F;
            // 
            // grhDepartment
            // 
            this.grhDepartment.DataField = "DepartmentCode";
            this.grhDepartment.GroupKeepTogether = GrapeCity.ActiveReports.SectionReportModel.GroupKeepTogether.All;
            this.grhDepartment.Height = 0F;
            this.grhDepartment.KeepTogether = true;
            this.grhDepartment.Name = "grhDepartment";
            this.grhDepartment.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            this.grhDepartment.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
            // 
            // grfDepartment
            // 
            this.grfDepartment.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCaptionDepartmentTotal,
            this.lineBottomDepartment,
            this.lineVD1,
            this.lineVD0,
            this.lineVD2,
            this.lineVD3,
            this.lineVD4,
            this.lineVD5,
            this.lineVD6,
            this.lineVD7,
            this.lineVD8,
            this.lineVD9});
            this.grfDepartment.Height = 0.2019685F;
            this.grfDepartment.KeepTogether = true;
            this.grfDepartment.Name = "grfDepartment";
            // 
            // lblCaptionDepartmentTotal
            // 
            this.lblCaptionDepartmentTotal.Height = 0.2019685F;
            this.lblCaptionDepartmentTotal.HyperLink = null;
            this.lblCaptionDepartmentTotal.Left = 0F;
            this.lblCaptionDepartmentTotal.Name = "lblCaptionDepartmentTotal";
            this.lblCaptionDepartmentTotal.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblCaptionDepartmentTotal.Text = "部門計";
            this.lblCaptionDepartmentTotal.Top = 0F;
            this.lblCaptionDepartmentTotal.Width = 4.488189F;
            // 
            // lineBottomDepartment
            // 
            this.lineBottomDepartment.Height = 0F;
            this.lineBottomDepartment.Left = 0F;
            this.lineBottomDepartment.LineWeight = 1F;
            this.lineBottomDepartment.Name = "lineBottomDepartment";
            this.lineBottomDepartment.Top = 0.202F;
            this.lineBottomDepartment.Width = 10.62992F;
            this.lineBottomDepartment.X1 = 0F;
            this.lineBottomDepartment.X2 = 10.62992F;
            this.lineBottomDepartment.Y1 = 0.202F;
            this.lineBottomDepartment.Y2 = 0.202F;
            // 
            // lineVD1
            // 
            this.lineVD1.Height = 0.202F;
            this.lineVD1.Left = 5.118898F;
            this.lineVD1.LineWeight = 1F;
            this.lineVD1.Name = "lineVD1";
            this.lineVD1.Top = 0F;
            this.lineVD1.Width = 0F;
            this.lineVD1.X1 = 5.118898F;
            this.lineVD1.X2 = 5.118898F;
            this.lineVD1.Y1 = 0F;
            this.lineVD1.Y2 = 0.202F;
            // 
            // lineVD0
            // 
            this.lineVD0.Height = 0.202F;
            this.lineVD0.Left = 4.488189F;
            this.lineVD0.LineWeight = 1F;
            this.lineVD0.Name = "lineVD0";
            this.lineVD0.Top = 0F;
            this.lineVD0.Width = 0F;
            this.lineVD0.X1 = 4.488189F;
            this.lineVD0.X2 = 4.488189F;
            this.lineVD0.Y1 = 0F;
            this.lineVD0.Y2 = 0.202F;
            // 
            // lineVD2
            // 
            this.lineVD2.Height = 0.202F;
            this.lineVD2.Left = 5.138583F;
            this.lineVD2.LineWeight = 1F;
            this.lineVD2.Name = "lineVD2";
            this.lineVD2.Top = 0F;
            this.lineVD2.Width = 0F;
            this.lineVD2.X1 = 5.138583F;
            this.lineVD2.X2 = 5.138583F;
            this.lineVD2.Y1 = 0F;
            this.lineVD2.Y2 = 0.202F;
            // 
            // lineVD3
            // 
            this.lineVD3.Height = 0.202F;
            this.lineVD3.Left = 6.038977F;
            this.lineVD3.LineWeight = 1F;
            this.lineVD3.Name = "lineVD3";
            this.lineVD3.Top = 0F;
            this.lineVD3.Width = 0F;
            this.lineVD3.X1 = 6.038977F;
            this.lineVD3.X2 = 6.038977F;
            this.lineVD3.Y1 = 0F;
            this.lineVD3.Y2 = 0.202F;
            // 
            // lineVD4
            // 
            this.lineVD4.Height = 0.202F;
            this.lineVD4.Left = 6.058662F;
            this.lineVD4.LineWeight = 1F;
            this.lineVD4.Name = "lineVD4";
            this.lineVD4.Top = 0F;
            this.lineVD4.Width = 0F;
            this.lineVD4.X1 = 6.058662F;
            this.lineVD4.X2 = 6.058662F;
            this.lineVD4.Y1 = 0F;
            this.lineVD4.Y2 = 0.202F;
            // 
            // lineVD5
            // 
            this.lineVD5.Height = 0.202F;
            this.lineVD5.Left = 6.959055F;
            this.lineVD5.LineWeight = 1F;
            this.lineVD5.Name = "lineVD5";
            this.lineVD5.Top = 0F;
            this.lineVD5.Width = 0F;
            this.lineVD5.X1 = 6.959055F;
            this.lineVD5.X2 = 6.959055F;
            this.lineVD5.Y1 = 0F;
            this.lineVD5.Y2 = 0.202F;
            // 
            // lineVD6
            // 
            this.lineVD6.Height = 0.202F;
            this.lineVD6.Left = 7.879134F;
            this.lineVD6.LineWeight = 1F;
            this.lineVD6.Name = "lineVD6";
            this.lineVD6.Top = 0F;
            this.lineVD6.Width = 0F;
            this.lineVD6.X1 = 7.879134F;
            this.lineVD6.X2 = 7.879134F;
            this.lineVD6.Y1 = 0F;
            this.lineVD6.Y2 = 0.202F;
            // 
            // lineVD7
            // 
            this.lineVD7.Height = 0.202F;
            this.lineVD7.Left = 8.799213F;
            this.lineVD7.LineWeight = 1F;
            this.lineVD7.Name = "lineVD7";
            this.lineVD7.Top = 0F;
            this.lineVD7.Width = 0F;
            this.lineVD7.X1 = 8.799213F;
            this.lineVD7.X2 = 8.799213F;
            this.lineVD7.Y1 = 0F;
            this.lineVD7.Y2 = 0.202F;
            // 
            // lineVD8
            // 
            this.lineVD8.Height = 0.202F;
            this.lineVD8.Left = 9.719292F;
            this.lineVD8.LineWeight = 1F;
            this.lineVD8.Name = "lineVD8";
            this.lineVD8.Top = 0F;
            this.lineVD8.Width = 0F;
            this.lineVD8.X1 = 9.719292F;
            this.lineVD8.X2 = 9.719292F;
            this.lineVD8.Y1 = 0F;
            this.lineVD8.Y2 = 0.202F;
            // 
            // lineVD9
            // 
            this.lineVD9.Height = 0.202F;
            this.lineVD9.Left = 9.73819F;
            this.lineVD9.LineWeight = 1F;
            this.lineVD9.Name = "lineVD9";
            this.lineVD9.Top = 0F;
            this.lineVD9.Width = 0F;
            this.lineVD9.X1 = 9.73819F;
            this.lineVD9.X2 = 9.73819F;
            this.lineVD9.Y1 = 0F;
            this.lineVD9.Y2 = 0.202F;
            // 
            // grhStaff
            // 
            this.grhStaff.DataField = "StaffCode";
            this.grhStaff.GroupKeepTogether = GrapeCity.ActiveReports.SectionReportModel.GroupKeepTogether.All;
            this.grhStaff.Height = 0F;
            this.grhStaff.KeepTogether = true;
            this.grhStaff.Name = "grhStaff";
            this.grhStaff.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            this.grhStaff.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
            // 
            // grfStaff
            // 
            this.grfStaff.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblCaptionStaffTotal,
            this.lineBottomStaff,
            this.lineVS0,
            this.lineVS1,
            this.lineVS2,
            this.lineVS3,
            this.lineVS4,
            this.lineVS5,
            this.lineVS6,
            this.lineVS7,
            this.lineVS8,
            this.lineVS9,
            this.linTantoTop});
            this.grfStaff.Height = 0.2019685F;
            this.grfStaff.KeepTogether = true;
            this.grfStaff.Name = "grfStaff";
            // 
            // lblCaptionStaffTotal
            // 
            this.lblCaptionStaffTotal.Height = 0.202F;
            this.lblCaptionStaffTotal.HyperLink = null;
            this.lblCaptionStaffTotal.Left = 0F;
            this.lblCaptionStaffTotal.Name = "lblCaptionStaffTotal";
            this.lblCaptionStaffTotal.Style = "background-color: WhiteSmoke; font-family: ＭＳ 明朝; font-size: 7.5pt; text-align: c" +
    "enter; vertical-align: middle; ddo-char-set: 1";
            this.lblCaptionStaffTotal.Text = "担当者計";
            this.lblCaptionStaffTotal.Top = 0F;
            this.lblCaptionStaffTotal.Width = 4.488189F;
            // 
            // lineBottomStaff
            // 
            this.lineBottomStaff.Height = 0F;
            this.lineBottomStaff.Left = 0F;
            this.lineBottomStaff.LineWeight = 1F;
            this.lineBottomStaff.Name = "lineBottomStaff";
            this.lineBottomStaff.Top = 0.202F;
            this.lineBottomStaff.Width = 10.62992F;
            this.lineBottomStaff.X1 = 0F;
            this.lineBottomStaff.X2 = 10.62992F;
            this.lineBottomStaff.Y1 = 0.202F;
            this.lineBottomStaff.Y2 = 0.202F;
            // 
            // lineVS0
            // 
            this.lineVS0.Height = 0.202F;
            this.lineVS0.Left = 4.488189F;
            this.lineVS0.LineWeight = 1F;
            this.lineVS0.Name = "lineVS0";
            this.lineVS0.Top = 0F;
            this.lineVS0.Width = 0F;
            this.lineVS0.X1 = 4.488189F;
            this.lineVS0.X2 = 4.488189F;
            this.lineVS0.Y1 = 0F;
            this.lineVS0.Y2 = 0.202F;
            // 
            // lineVS1
            // 
            this.lineVS1.Height = 0.202F;
            this.lineVS1.Left = 5.118898F;
            this.lineVS1.LineWeight = 1F;
            this.lineVS1.Name = "lineVS1";
            this.lineVS1.Top = 0F;
            this.lineVS1.Width = 0F;
            this.lineVS1.X1 = 5.118898F;
            this.lineVS1.X2 = 5.118898F;
            this.lineVS1.Y1 = 0F;
            this.lineVS1.Y2 = 0.202F;
            // 
            // lineVS2
            // 
            this.lineVS2.Height = 0.202F;
            this.lineVS2.Left = 5.138583F;
            this.lineVS2.LineWeight = 1F;
            this.lineVS2.Name = "lineVS2";
            this.lineVS2.Top = 0F;
            this.lineVS2.Width = 0F;
            this.lineVS2.X1 = 5.138583F;
            this.lineVS2.X2 = 5.138583F;
            this.lineVS2.Y1 = 0F;
            this.lineVS2.Y2 = 0.202F;
            // 
            // lineVS3
            // 
            this.lineVS3.Height = 0.202F;
            this.lineVS3.Left = 6.038977F;
            this.lineVS3.LineWeight = 1F;
            this.lineVS3.Name = "lineVS3";
            this.lineVS3.Top = 7.450581E-09F;
            this.lineVS3.Width = 0F;
            this.lineVS3.X1 = 6.038977F;
            this.lineVS3.X2 = 6.038977F;
            this.lineVS3.Y1 = 7.450581E-09F;
            this.lineVS3.Y2 = 0.202F;
            // 
            // lineVS4
            // 
            this.lineVS4.Height = 0.202F;
            this.lineVS4.Left = 6.058662F;
            this.lineVS4.LineWeight = 1F;
            this.lineVS4.Name = "lineVS4";
            this.lineVS4.Top = 0F;
            this.lineVS4.Width = 0F;
            this.lineVS4.X1 = 6.058662F;
            this.lineVS4.X2 = 6.058662F;
            this.lineVS4.Y1 = 0F;
            this.lineVS4.Y2 = 0.202F;
            // 
            // lineVS5
            // 
            this.lineVS5.Height = 0.202F;
            this.lineVS5.Left = 6.959055F;
            this.lineVS5.LineWeight = 1F;
            this.lineVS5.Name = "lineVS5";
            this.lineVS5.Top = 0F;
            this.lineVS5.Width = 0F;
            this.lineVS5.X1 = 6.959055F;
            this.lineVS5.X2 = 6.959055F;
            this.lineVS5.Y1 = 0F;
            this.lineVS5.Y2 = 0.202F;
            // 
            // lineVS6
            // 
            this.lineVS6.Height = 0.202F;
            this.lineVS6.Left = 7.879134F;
            this.lineVS6.LineWeight = 1F;
            this.lineVS6.Name = "lineVS6";
            this.lineVS6.Top = 0F;
            this.lineVS6.Width = 0F;
            this.lineVS6.X1 = 7.879134F;
            this.lineVS6.X2 = 7.879134F;
            this.lineVS6.Y1 = 0F;
            this.lineVS6.Y2 = 0.202F;
            // 
            // lineVS7
            // 
            this.lineVS7.Height = 0.202F;
            this.lineVS7.Left = 8.799213F;
            this.lineVS7.LineWeight = 1F;
            this.lineVS7.Name = "lineVS7";
            this.lineVS7.Top = 0F;
            this.lineVS7.Width = 0F;
            this.lineVS7.X1 = 8.799213F;
            this.lineVS7.X2 = 8.799213F;
            this.lineVS7.Y1 = 0F;
            this.lineVS7.Y2 = 0.202F;
            // 
            // lineVS8
            // 
            this.lineVS8.Height = 0.202F;
            this.lineVS8.Left = 9.719292F;
            this.lineVS8.LineWeight = 1F;
            this.lineVS8.Name = "lineVS8";
            this.lineVS8.Top = 0F;
            this.lineVS8.Width = 0F;
            this.lineVS8.X1 = 9.719292F;
            this.lineVS8.X2 = 9.719292F;
            this.lineVS8.Y1 = 0F;
            this.lineVS8.Y2 = 0.202F;
            // 
            // lineVS9
            // 
            this.lineVS9.Height = 0.202F;
            this.lineVS9.Left = 9.73819F;
            this.lineVS9.LineWeight = 1F;
            this.lineVS9.Name = "lineVS9";
            this.lineVS9.Top = 0F;
            this.lineVS9.Width = 0F;
            this.lineVS9.X1 = 9.73819F;
            this.lineVS9.X2 = 9.73819F;
            this.lineVS9.Y1 = 0F;
            this.lineVS9.Y2 = 0.202F;
            // 
            // linTantoTop
            // 
            this.linTantoTop.Height = 0F;
            this.linTantoTop.Left = 0F;
            this.linTantoTop.LineWeight = 1F;
            this.linTantoTop.Name = "linTantoTop";
            this.linTantoTop.Top = 0F;
            this.linTantoTop.Visible = false;
            this.linTantoTop.Width = 10.62992F;
            this.linTantoTop.X1 = 0F;
            this.linTantoTop.X2 = 10.62992F;
            this.linTantoTop.Y1 = 0F;
            this.linTantoTop.Y2 = 0F;
            // 
            // CollectionScheduleSectionReport
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
            this.Sections.Add(this.grhGrandTotal);
            this.Sections.Add(this.grhTotal);
            this.Sections.Add(this.grhDepartment);
            this.Sections.Add(this.grhStaff);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.grfStaff);
            this.Sections.Add(this.grfDepartment);
            this.Sections.Add(this.grfTotal);
            this.Sections.Add(this.grfGrandTotal);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " +
            "color: Black; font-family: \"ＭＳ 明朝\"; ddo-char-set: 186", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lblCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderStaff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeaderStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerCollectInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblClosingDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTanto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBumon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblKubun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectedAmountLast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectAmount0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectAmount1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectAmount2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUncollectAmount3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblKingakuK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeandName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmountLast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrandTotalBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGrandTotalCaption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmountLastSum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount0Sum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount1Sum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount2Sum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmount3Sum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUncollectedAmountTotalSum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCaptionTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCaptionDepartmentTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCaptionStaffTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label lblCompany;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblcompanycode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblHeaderDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblHeaderDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblHeaderDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblHeaderStaff;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblHeaderStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblHeaderStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomer;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCustomerCollectInfo;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblClosingDay;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTanto;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBumon;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblKubun;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblUncollectedAmountLast;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblUncollectAmount0;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblUncollectAmount1;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblUncollectAmount2;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblUncollectAmount3;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblKingakuK;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderTop;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderBottom;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV01;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV02;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV03;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV04;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV05;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV06;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV07;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV09;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV10;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV11;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV12;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV13;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerCodeandName;
        private GrapeCity.ActiveReports.SectionReportModel.Label txtClosingDay;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCategoryName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmountLast;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmount0;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmount1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmount2;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmount3;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmountTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineBottomDetail;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV01;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV02;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV03;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV04;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV05;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV06;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV07;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV09;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV10;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV11;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV12;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV13;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderV08;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDV08;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader grhGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter grfGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader grhTotal;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter grfTotal;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader grhDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter grfDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader grhStaff;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter grfStaff;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCaptionStaffTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineBottomStaff;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS0;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS3;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS4;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS5;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS6;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS7;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS8;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVS9;
        private GrapeCity.ActiveReports.SectionReportModel.Line linTantoTop;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblGrandTotalBack;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblGrandTotalCaption;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmountLastSum;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmount0Sum;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmount1Sum;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmount2Sum;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmount3Sum;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtUncollectedAmountTotalSum;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineBottomGrandTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV01;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV02;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV03;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV04;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV05;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV06;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV07;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV08;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineGTV09;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCaptionTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineBottomTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT0;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT3;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT4;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT5;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT6;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT7;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT8;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVT9;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblCaptionDepartmentTotal;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineBottomDepartment;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD0;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD2;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD3;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD4;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD5;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD6;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD7;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD8;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineVD9;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailTop;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblPageNumber;
    }
}
