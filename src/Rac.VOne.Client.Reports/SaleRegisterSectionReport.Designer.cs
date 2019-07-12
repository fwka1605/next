namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// SaleRegisterSectionReport の概要の説明です。
    /// </summary>
    partial class SaleRegisterSectionReport
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SaleRegisterSectionReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.lbltitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepCode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDepName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblMail = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label1 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblcompanycode = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lbldate = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.ridate = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lineHeaderHorUpper = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDeptName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerDeptCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerStaffName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtStaffCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMail = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerDeptName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailHorLower = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerDeptCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerStaffName = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineDetailVerStaffCode = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.lineHeaderVerMail = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblTel = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTel = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lineDetailVerMail = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineHeaderVerTel = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lblFax = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineDetailVerTel = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtFax = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblFax,
            this.lblTel,
            this.lbltitle,
            this.lblStaffCode,
            this.lblName,
            this.lblDepCode,
            this.lblDepName,
            this.lblMail,
            this.label1,
            this.lblcompanycode,
            this.lbldate,
            this.ridate,
            this.lineHeaderHorUpper,
            this.lineHeaderVerDeptName,
            this.lineHeaderVerDeptCode,
            this.lineHeaderVerStaffName,
            this.lineHeaderVerStaffCode,
            this.lineHeaderHorLower,
            this.lineHeaderVerMail,
            this.lineHeaderVerTel});
            this.pageHeader.Height = 0.9944168F;
            this.pageHeader.Name = "pageHeader";
            // 
            // lbltitle
            // 
            this.lbltitle.Height = 0.2311024F;
            this.lbltitle.HyperLink = null;
            this.lbltitle.Left = 0F;
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Style = "font-size: 14pt; text-align: center; text-decoration: underline; vertical-align: " +
    "middle; ddo-char-set: 1";
            this.lbltitle.Text = "営業担当者マスター一覧";
            this.lbltitle.Top = 0.2704725F;
            this.lbltitle.Width = 10.62992F;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Height = 0.319F;
            this.lblStaffCode.HyperLink = null;
            this.lblStaffCode.Left = 0F;
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblStaffCode.Text = "営業担当者コード";
            this.lblStaffCode.Top = 0.675F;
            this.lblStaffCode.Width = 1.062992F;
            // 
            // lblName
            // 
            this.lblName.Height = 0.319F;
            this.lblName.HyperLink = null;
            this.lblName.Left = 1.064961F;
            this.lblName.Name = "lblName";
            this.lblName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblName.Text = "営業担当者名";
            this.lblName.Top = 0.6751969F;
            this.lblName.Width = 1.980315F;
            // 
            // lblDepCode
            // 
            this.lblDepCode.Height = 0.319F;
            this.lblDepCode.HyperLink = null;
            this.lblDepCode.Left = 3.03937F;
            this.lblDepCode.Name = "lblDepCode";
            this.lblDepCode.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepCode.Text = "請求部門コード";
            this.lblDepCode.Top = 0.6748032F;
            this.lblDepCode.Width = 0.9448819F;
            // 
            // lblDepName
            // 
            this.lblDepName.Height = 0.319F;
            this.lblDepName.HyperLink = null;
            this.lblDepName.Left = 3.992126F;
            this.lblDepName.Name = "lblDepName";
            this.lblDepName.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblDepName.Text = "請求部門名";
            this.lblDepName.Top = 0.6748032F;
            this.lblDepName.Width = 1.980315F;
            // 
            // lblMail
            // 
            this.lblMail.Height = 0.319F;
            this.lblMail.HyperLink = null;
            this.lblMail.Left = 5.972441F;
            this.lblMail.Name = "lblMail";
            this.lblMail.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblMail.Text = "メールアドレス";
            this.lblMail.Top = 0.6751969F;
            this.lblMail.Width = 2.334646F;
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
            this.label1.Width = 0.7874016F;
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
            this.lbldate.Width = 0.6984252F;
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
            // lineHeaderHorUpper
            // 
            this.lineHeaderHorUpper.Height = 0F;
            this.lineHeaderHorUpper.Left = 0F;
            this.lineHeaderHorUpper.LineWeight = 1F;
            this.lineHeaderHorUpper.Name = "lineHeaderHorUpper";
            this.lineHeaderHorUpper.Top = 0.675F;
            this.lineHeaderHorUpper.Width = 10.6063F;
            this.lineHeaderHorUpper.X1 = 0F;
            this.lineHeaderHorUpper.X2 = 10.6063F;
            this.lineHeaderHorUpper.Y1 = 0.675F;
            this.lineHeaderHorUpper.Y2 = 0.675F;
            // 
            // lineHeaderVerDeptName
            // 
            this.lineHeaderVerDeptName.Height = 0.322417F;
            this.lineHeaderVerDeptName.Left = 5.972441F;
            this.lineHeaderVerDeptName.LineWeight = 1F;
            this.lineHeaderVerDeptName.Name = "lineHeaderVerDeptName";
            this.lineHeaderVerDeptName.Top = 0.6791339F;
            this.lineHeaderVerDeptName.Width = 0F;
            this.lineHeaderVerDeptName.X1 = 5.972441F;
            this.lineHeaderVerDeptName.X2 = 5.972441F;
            this.lineHeaderVerDeptName.Y1 = 0.6791339F;
            this.lineHeaderVerDeptName.Y2 = 1.001551F;
            // 
            // lineHeaderVerDeptCode
            // 
            this.lineHeaderVerDeptCode.Height = 0.3224167F;
            this.lineHeaderVerDeptCode.Left = 3.988189F;
            this.lineHeaderVerDeptCode.LineWeight = 1F;
            this.lineHeaderVerDeptCode.Name = "lineHeaderVerDeptCode";
            this.lineHeaderVerDeptCode.Top = 0.6728347F;
            this.lineHeaderVerDeptCode.Width = 0F;
            this.lineHeaderVerDeptCode.X1 = 3.988189F;
            this.lineHeaderVerDeptCode.X2 = 3.988189F;
            this.lineHeaderVerDeptCode.Y1 = 0.6728347F;
            this.lineHeaderVerDeptCode.Y2 = 0.9952514F;
            // 
            // lineHeaderVerStaffName
            // 
            this.lineHeaderVerStaffName.Height = 0.3224167F;
            this.lineHeaderVerStaffName.Left = 3.03937F;
            this.lineHeaderVerStaffName.LineWeight = 1F;
            this.lineHeaderVerStaffName.Name = "lineHeaderVerStaffName";
            this.lineHeaderVerStaffName.Top = 0.6748032F;
            this.lineHeaderVerStaffName.Width = 0F;
            this.lineHeaderVerStaffName.X1 = 3.03937F;
            this.lineHeaderVerStaffName.X2 = 3.03937F;
            this.lineHeaderVerStaffName.Y1 = 0.6748032F;
            this.lineHeaderVerStaffName.Y2 = 0.9972199F;
            // 
            // lineHeaderVerStaffCode
            // 
            this.lineHeaderVerStaffCode.Height = 0.3224167F;
            this.lineHeaderVerStaffCode.Left = 1.066929F;
            this.lineHeaderVerStaffCode.LineWeight = 1F;
            this.lineHeaderVerStaffCode.Name = "lineHeaderVerStaffCode";
            this.lineHeaderVerStaffCode.Top = 0.6740158F;
            this.lineHeaderVerStaffCode.Width = 0F;
            this.lineHeaderVerStaffCode.X1 = 1.066929F;
            this.lineHeaderVerStaffCode.X2 = 1.066929F;
            this.lineHeaderVerStaffCode.Y1 = 0.6740158F;
            this.lineHeaderVerStaffCode.Y2 = 0.9964325F;
            // 
            // lineHeaderHorLower
            // 
            this.lineHeaderHorLower.Height = 0F;
            this.lineHeaderHorLower.Left = 0F;
            this.lineHeaderHorLower.LineWeight = 1F;
            this.lineHeaderHorLower.Name = "lineHeaderHorLower";
            this.lineHeaderHorLower.Top = 0.998F;
            this.lineHeaderHorLower.Width = 10.6063F;
            this.lineHeaderHorLower.X1 = 0F;
            this.lineHeaderHorLower.X2 = 10.6063F;
            this.lineHeaderHorLower.Y1 = 0.998F;
            this.lineHeaderHorLower.Y2 = 0.998F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtFax,
            this.txtTel,
            this.txtStaffCode,
            this.txtStaffName,
            this.txtDepartmentCode,
            this.txtDepartmentName,
            this.txtMail,
            this.lineDetailVerDeptName,
            this.lineDetailHorLower,
            this.lineDetailVerDeptCode,
            this.lineDetailVerStaffName,
            this.lineDetailVerStaffCode,
            this.lineDetailVerMail,
            this.lineDetailVerTel});
            this.detail.Height = 0.2797499F;
            this.detail.Name = "detail";
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.Height = 0.27F;
            this.txtStaffCode.Left = 0.01F;
            this.txtStaffCode.MultiLine = false;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Style = "text-align: center; vertical-align: middle";
            this.txtStaffCode.Text = "txtStaffCode";
            this.txtStaffCode.Top = 0.003F;
            this.txtStaffCode.Width = 1.062992F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.Height = 0.27F;
            this.txtStaffName.Left = 1.068929F;
            this.txtStaffName.MultiLine = false;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Padding = new GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0);
            this.txtStaffName.Style = "vertical-align: middle";
            this.txtStaffName.Text = "txtStaffName";
            this.txtStaffName.Top = 0.003149607F;
            this.txtStaffName.Width = 1.968504F;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.Height = 0.27F;
            this.txtDepartmentCode.Left = 3.051181F;
            this.txtDepartmentCode.MultiLine = false;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Style = "text-align: center; vertical-align: middle";
            this.txtDepartmentCode.Text = "txtDepartmentCode";
            this.txtDepartmentCode.Top = 0F;
            this.txtDepartmentCode.Width = 0.9251968F;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.Height = 0.27F;
            this.txtDepartmentName.Left = 4.003937F;
            this.txtDepartmentName.MultiLine = false;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Padding = new GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0);
            this.txtDepartmentName.Style = "vertical-align: middle";
            this.txtDepartmentName.Text = "txtDepartmentName";
            this.txtDepartmentName.Top = 0.001181102F;
            this.txtDepartmentName.Width = 1.968504F;
            // 
            // txtMail
            // 
            this.txtMail.Height = 0.27F;
            this.txtMail.Left = 5.984252F;
            this.txtMail.MultiLine = false;
            this.txtMail.Name = "txtMail";
            this.txtMail.Padding = new GrapeCity.ActiveReports.PaddingEx(3, 0, 0, 0);
            this.txtMail.Style = "vertical-align: middle";
            this.txtMail.Text = "txtMail";
            this.txtMail.Top = 0.004000001F;
            this.txtMail.Width = 2.322835F;
            // 
            // lineDetailVerDeptName
            // 
            this.lineDetailVerDeptName.Height = 0.271F;
            this.lineDetailVerDeptName.Left = 5.972441F;
            this.lineDetailVerDeptName.LineWeight = 1F;
            this.lineDetailVerDeptName.Name = "lineDetailVerDeptName";
            this.lineDetailVerDeptName.Top = 0F;
            this.lineDetailVerDeptName.Width = 0F;
            this.lineDetailVerDeptName.X1 = 5.972441F;
            this.lineDetailVerDeptName.X2 = 5.972441F;
            this.lineDetailVerDeptName.Y1 = 0F;
            this.lineDetailVerDeptName.Y2 = 0.271F;
            // 
            // lineDetailHorLower
            // 
            this.lineDetailHorLower.Height = 0F;
            this.lineDetailHorLower.Left = 0F;
            this.lineDetailHorLower.LineWeight = 1F;
            this.lineDetailHorLower.Name = "lineDetailHorLower";
            this.lineDetailHorLower.Top = 0.274F;
            this.lineDetailHorLower.Width = 10.6063F;
            this.lineDetailHorLower.X1 = 0F;
            this.lineDetailHorLower.X2 = 10.6063F;
            this.lineDetailHorLower.Y1 = 0.274F;
            this.lineDetailHorLower.Y2 = 0.274F;
            // 
            // lineDetailVerDeptCode
            // 
            this.lineDetailVerDeptCode.Height = 0.271F;
            this.lineDetailVerDeptCode.Left = 3.988189F;
            this.lineDetailVerDeptCode.LineWeight = 1F;
            this.lineDetailVerDeptCode.Name = "lineDetailVerDeptCode";
            this.lineDetailVerDeptCode.Top = 0F;
            this.lineDetailVerDeptCode.Width = 0F;
            this.lineDetailVerDeptCode.X1 = 3.988189F;
            this.lineDetailVerDeptCode.X2 = 3.988189F;
            this.lineDetailVerDeptCode.Y1 = 0F;
            this.lineDetailVerDeptCode.Y2 = 0.271F;
            // 
            // lineDetailVerStaffName
            // 
            this.lineDetailVerStaffName.Height = 0.271F;
            this.lineDetailVerStaffName.Left = 3.03937F;
            this.lineDetailVerStaffName.LineWeight = 1F;
            this.lineDetailVerStaffName.Name = "lineDetailVerStaffName";
            this.lineDetailVerStaffName.Top = 0F;
            this.lineDetailVerStaffName.Width = 0F;
            this.lineDetailVerStaffName.X1 = 3.03937F;
            this.lineDetailVerStaffName.X2 = 3.03937F;
            this.lineDetailVerStaffName.Y1 = 0F;
            this.lineDetailVerStaffName.Y2 = 0.271F;
            // 
            // lineDetailVerStaffCode
            // 
            this.lineDetailVerStaffCode.Height = 0.2710001F;
            this.lineDetailVerStaffCode.Left = 1.066929F;
            this.lineDetailVerStaffCode.LineWeight = 1F;
            this.lineDetailVerStaffCode.Name = "lineDetailVerStaffCode";
            this.lineDetailVerStaffCode.Top = 0F;
            this.lineDetailVerStaffCode.Width = 0F;
            this.lineDetailVerStaffCode.X1 = 1.066929F;
            this.lineDetailVerStaffCode.X2 = 1.066929F;
            this.lineDetailVerStaffCode.Y1 = 0F;
            this.lineDetailVerStaffCode.Y2 = 0.2710001F;
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
            this.reportInfo1.Style = "color: Gray; font-size: 7pt; text-align: center; vertical-align: middle; ddo-char" +
    "-set: 1";
            this.reportInfo1.Top = 0.05748032F;
            this.reportInfo1.Width = 10.62992F;
            // 
            // lineHeaderVerMail
            // 
            this.lineHeaderVerMail.Height = 0.3224168F;
            this.lineHeaderVerMail.Left = 8.299213F;
            this.lineHeaderVerMail.LineWeight = 1F;
            this.lineHeaderVerMail.Name = "lineHeaderVerMail";
            this.lineHeaderVerMail.Top = 0.6791339F;
            this.lineHeaderVerMail.Width = 0F;
            this.lineHeaderVerMail.X1 = 8.299213F;
            this.lineHeaderVerMail.X2 = 8.299213F;
            this.lineHeaderVerMail.Y1 = 0.6791339F;
            this.lineHeaderVerMail.Y2 = 1.001551F;
            // 
            // lblTel
            // 
            this.lblTel.Height = 0.319F;
            this.lblTel.HyperLink = null;
            this.lblTel.Left = 8.307087F;
            this.lblTel.Name = "lblTel";
            this.lblTel.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblTel.Text = "電話番号";
            this.lblTel.Top = 0.6751969F;
            this.lblTel.Width = 1.153543F;
            // 
            // txtTel
            // 
            this.txtTel.Height = 0.27F;
            this.txtTel.Left = 8.314961F;
            this.txtTel.MultiLine = false;
            this.txtTel.Name = "txtTel";
            this.txtTel.Style = "text-align: left; vertical-align: middle";
            this.txtTel.Text = "txtTel";
            this.txtTel.Top = 0.003937008F;
            this.txtTel.Width = 1.141732F;
            // 
            // lineDetailVerMail
            // 
            this.lineDetailVerMail.Height = 0.271F;
            this.lineDetailVerMail.Left = 8.299213F;
            this.lineDetailVerMail.LineWeight = 1F;
            this.lineDetailVerMail.Name = "lineDetailVerMail";
            this.lineDetailVerMail.Top = 0F;
            this.lineDetailVerMail.Width = 0F;
            this.lineDetailVerMail.X1 = 8.299213F;
            this.lineDetailVerMail.X2 = 8.299213F;
            this.lineDetailVerMail.Y1 = 0F;
            this.lineDetailVerMail.Y2 = 0.271F;
            // 
            // lineHeaderVerTel
            // 
            this.lineHeaderVerTel.Height = 0.3224168F;
            this.lineHeaderVerTel.Left = 9.448819F;
            this.lineHeaderVerTel.LineWeight = 1F;
            this.lineHeaderVerTel.Name = "lineHeaderVerTel";
            this.lineHeaderVerTel.Top = 0.6791339F;
            this.lineHeaderVerTel.Width = 0F;
            this.lineHeaderVerTel.X1 = 9.448819F;
            this.lineHeaderVerTel.X2 = 9.448819F;
            this.lineHeaderVerTel.Y1 = 0.6791339F;
            this.lineHeaderVerTel.Y2 = 1.001551F;
            // 
            // lblFax
            // 
            this.lblFax.Height = 0.319F;
            this.lblFax.HyperLink = null;
            this.lblFax.Left = 9.448819F;
            this.lblFax.Name = "lblFax";
            this.lblFax.Style = "background-color: WhiteSmoke; text-align: center; vertical-align: middle";
            this.lblFax.Text = "FAX番号";
            this.lblFax.Top = 0.6751969F;
            this.lblFax.Width = 1.153543F;
            // 
            // lineDetailVerTel
            // 
            this.lineDetailVerTel.Height = 0.271F;
            this.lineDetailVerTel.Left = 9.448819F;
            this.lineDetailVerTel.LineWeight = 1F;
            this.lineDetailVerTel.Name = "lineDetailVerTel";
            this.lineDetailVerTel.Top = 0F;
            this.lineDetailVerTel.Width = 0F;
            this.lineDetailVerTel.X1 = 9.448819F;
            this.lineDetailVerTel.X2 = 9.448819F;
            this.lineDetailVerTel.Y1 = 0F;
            this.lineDetailVerTel.Y2 = 0.271F;
            // 
            // txtFax
            // 
            this.txtFax.Height = 0.27F;
            this.txtFax.Left = 9.464567F;
            this.txtFax.MultiLine = false;
            this.txtFax.Name = "txtFax";
            this.txtFax.Style = "text-align: left; vertical-align: middle";
            this.txtFax.Text = "txtFax";
            this.txtFax.Top = 0.003937008F;
            this.txtFax.Width = 1.141732F;
            // 
            // SaleRegisterSectionReport
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
            this.Sections.Add(this.detail);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.lbltitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblcompanycode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbldate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ridate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private GrapeCity.ActiveReports.SectionReportModel.Label lbltitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDepName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblMail;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentCode;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDepartmentName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtMail;
        private GrapeCity.ActiveReports.SectionReportModel.Label label1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblcompanycode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lbldate;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo ridate;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorUpper;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDeptName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDeptName;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerDeptCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailHorLower;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerDeptCode;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerStaffCode;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTel;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerMail;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtTel;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerMail;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblFax;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineHeaderVerTel;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineDetailVerTel;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtFax;
    }
}
