namespace Rac.VOne.Client.Reports
{
    /// <summary>
    /// CustomerGroupSectionReport の概要の説明です。
    /// </summary>
    partial class ReminderReport
    {
        private GrapeCity.ActiveReports.SectionReportModel.PageHeader pageHeader;
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReminderReport));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.txtNote = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.textBox3 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtRowNumber = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line10 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtBillingStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDueAt = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.line14 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line1 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line24 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line25 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line26 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line27 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line28 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.ghReminder = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.txtCustomerPostalCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerAddress1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerAddress2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtOutputNoHeader = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtOutputAtHeader = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCompanyPostalCode = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCompanyAddress1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCompanyAddress2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCompanyName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtOwnDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtAccountingStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtStaffTel = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblTel = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtStaffFax = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblFax = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTitle = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtGreeting = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtMainText = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblBank = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtBankInfo1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblAccountName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtAccountName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtSubText = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtConclusion = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtTotalAmountHeader = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankInfo2 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtBankInfo3 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblTriFoldSymbolRight = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblTriFoldSymbolLeft = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.txtDestinationDepartmentName = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtDestinationAddressee = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.gfDetail = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.ghDetail = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.lblDetailTitle = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblNote = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingStaffName = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblBillingAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lblDueAt = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.label4 = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.line2 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line7 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line9 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line12 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line15 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line18 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line19 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line20 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.line21 = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.lblTotalAmount = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.lineTotalAmountTop = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineTotalAmountBottom = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineTotalAmountLeft = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineTotalAmountCenter = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.lineTotalAmountRight = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.txtOutputNoDetail = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtOutputAtDetail = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.txtCustomerStaffNameDetail = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.gfReminder = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRowNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerPostalCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerAddress1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerAddress2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputNoHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputAtHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyPostalCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyAddress1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyAddress2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOwnDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountingStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffFax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGreeting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMainText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConclusion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmountHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankInfo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankInfo3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTriFoldSymbolRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTriFoldSymbolLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationAddressee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDetailTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputNoDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputAtDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerStaffNameDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Height = 0F;
            this.pageHeader.Name = "pageHeader";
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtNote,
            this.textBox3,
            this.txtRowNumber,
            this.line10,
            this.txtBillingStaffName,
            this.txtBillingAmount,
            this.txtDueAt,
            this.line14,
            this.line1,
            this.line24,
            this.line25,
            this.line26,
            this.line27,
            this.line28});
            this.detail.Height = 0.3137551F;
            this.detail.Name = "detail";
            // 
            // txtNote
            // 
            this.txtNote.CanGrow = false;
            this.txtNote.Height = 0.306F;
            this.txtNote.Left = 0.95F;
            this.txtNote.Name = "txtNote";
            this.txtNote.Style = "text-align: left; vertical-align: middle";
            this.txtNote.Text = null;
            this.txtNote.Top = 9.313226E-10F;
            this.txtNote.Width = 3.348032F;
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = false;
            this.textBox3.DataField = "SalesAt";
            this.textBox3.Height = 0.306F;
            this.textBox3.Left = 0.255F;
            this.textBox3.Name = "textBox3";
            this.textBox3.OutputFormat = resources.GetString("textBox3.OutputFormat");
            this.textBox3.Style = "text-align: center; vertical-align: middle";
            this.textBox3.Text = null;
            this.textBox3.Top = 0F;
            this.textBox3.Width = 0.6950001F;
            // 
            // txtRowNumber
            // 
            this.txtRowNumber.CanGrow = false;
            this.txtRowNumber.DataField = "RowNumber";
            this.txtRowNumber.Height = 0.297F;
            this.txtRowNumber.Left = 0.025F;
            this.txtRowNumber.Name = "txtRowNumber";
            this.txtRowNumber.Style = "text-align: center; vertical-align: middle";
            this.txtRowNumber.Text = null;
            this.txtRowNumber.Top = 0F;
            this.txtRowNumber.Width = 0.23F;
            // 
            // line10
            // 
            this.line10.Height = 0.297F;
            this.line10.Left = 0.95F;
            this.line10.LineWeight = 1F;
            this.line10.Name = "line10";
            this.line10.Top = 9.313226E-10F;
            this.line10.Width = 0F;
            this.line10.X1 = 0.95F;
            this.line10.X2 = 0.95F;
            this.line10.Y1 = 9.313226E-10F;
            this.line10.Y2 = 0.297F;
            // 
            // txtBillingStaffName
            // 
            this.txtBillingStaffName.CanGrow = false;
            this.txtBillingStaffName.DataField = "StaffName";
            this.txtBillingStaffName.Height = 0.306F;
            this.txtBillingStaffName.Left = 4.298032F;
            this.txtBillingStaffName.Name = "txtBillingStaffName";
            this.txtBillingStaffName.Style = "text-align: center; vertical-align: middle";
            this.txtBillingStaffName.Text = null;
            this.txtBillingStaffName.Top = 0F;
            this.txtBillingStaffName.Width = 1.058268F;
            // 
            // txtBillingAmount
            // 
            this.txtBillingAmount.CanGrow = false;
            this.txtBillingAmount.DataField = "RemainAmount";
            this.txtBillingAmount.Height = 0.306F;
            this.txtBillingAmount.Left = 5.356299F;
            this.txtBillingAmount.Name = "txtBillingAmount";
            this.txtBillingAmount.OutputFormat = resources.GetString("txtBillingAmount.OutputFormat");
            this.txtBillingAmount.Style = "text-align: right; vertical-align: middle";
            this.txtBillingAmount.Text = "-123,456,789,012";
            this.txtBillingAmount.Top = 9.313226E-10F;
            this.txtBillingAmount.Width = 1.037702F;
            // 
            // txtDueAt
            // 
            this.txtDueAt.CanGrow = false;
            this.txtDueAt.DataField = "DueAt";
            this.txtDueAt.Height = 0.306F;
            this.txtDueAt.Left = 6.394095F;
            this.txtDueAt.Name = "txtDueAt";
            this.txtDueAt.OutputFormat = resources.GetString("txtDueAt.OutputFormat");
            this.txtDueAt.Style = "text-align: center; vertical-align: middle";
            this.txtDueAt.Text = null;
            this.txtDueAt.Top = 0F;
            this.txtDueAt.Width = 0.8255906F;
            // 
            // line14
            // 
            this.line14.Height = 9.450316E-05F;
            this.line14.Left = 0.025F;
            this.line14.LineWeight = 1F;
            this.line14.Name = "line14";
            this.line14.Top = 0.3059055F;
            this.line14.Width = 7.194686F;
            this.line14.X1 = 0.025F;
            this.line14.X2 = 7.219686F;
            this.line14.Y1 = 0.306F;
            this.line14.Y2 = 0.3059055F;
            // 
            // line1
            // 
            this.line1.Height = 0.297F;
            this.line1.Left = 0.255F;
            this.line1.LineWeight = 1F;
            this.line1.Name = "line1";
            this.line1.Top = 0F;
            this.line1.Width = 0F;
            this.line1.X1 = 0.255F;
            this.line1.X2 = 0.255F;
            this.line1.Y1 = 0F;
            this.line1.Y2 = 0.297F;
            // 
            // line24
            // 
            this.line24.Height = 0.297F;
            this.line24.Left = 4.298032F;
            this.line24.LineWeight = 1F;
            this.line24.Name = "line24";
            this.line24.Top = 0F;
            this.line24.Width = 0F;
            this.line24.X1 = 4.298032F;
            this.line24.X2 = 4.298032F;
            this.line24.Y1 = 0F;
            this.line24.Y2 = 0.297F;
            // 
            // line25
            // 
            this.line25.Height = 0.297F;
            this.line25.Left = 5.356299F;
            this.line25.LineWeight = 1F;
            this.line25.Name = "line25";
            this.line25.Top = 0F;
            this.line25.Width = 0F;
            this.line25.X1 = 5.356299F;
            this.line25.X2 = 5.356299F;
            this.line25.Y1 = 0F;
            this.line25.Y2 = 0.297F;
            // 
            // line26
            // 
            this.line26.Height = 0.297F;
            this.line26.Left = 6.394F;
            this.line26.LineWeight = 1F;
            this.line26.Name = "line26";
            this.line26.Top = 0.009000001F;
            this.line26.Width = 0F;
            this.line26.X1 = 6.394F;
            this.line26.X2 = 6.394F;
            this.line26.Y1 = 0.009000001F;
            this.line26.Y2 = 0.306F;
            // 
            // line27
            // 
            this.line27.Height = 0.297F;
            this.line27.Left = 7.219686F;
            this.line27.LineWeight = 1F;
            this.line27.Name = "line27";
            this.line27.Top = 0F;
            this.line27.Width = 0F;
            this.line27.X1 = 7.219686F;
            this.line27.X2 = 7.219686F;
            this.line27.Y1 = 0F;
            this.line27.Y2 = 0.297F;
            // 
            // line28
            // 
            this.line28.Height = 0.297F;
            this.line28.Left = 0.02500001F;
            this.line28.LineWeight = 1F;
            this.line28.Name = "line28";
            this.line28.Top = 0F;
            this.line28.Width = 0F;
            this.line28.X1 = 0.02500001F;
            this.line28.X2 = 0.02500001F;
            this.line28.Y1 = 0F;
            this.line28.Y2 = 0.297F;
            // 
            // pageFooter
            // 
            this.pageFooter.Height = 0.1354167F;
            this.pageFooter.Name = "pageFooter";
            // 
            // ghReminder
            // 
            this.ghReminder.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.txtCustomerPostalCode,
            this.txtCustomerAddress1,
            this.txtCustomerAddress2,
            this.txtCustomerName,
            this.txtOutputNoHeader,
            this.txtOutputAtHeader,
            this.txtCompanyPostalCode,
            this.txtCompanyAddress1,
            this.txtCompanyAddress2,
            this.txtCompanyName,
            this.txtOwnDepartmentName,
            this.txtAccountingStaffName,
            this.txtStaffName,
            this.txtStaffTel,
            this.lblTel,
            this.txtStaffFax,
            this.lblFax,
            this.txtTitle,
            this.txtGreeting,
            this.txtMainText,
            this.lblBank,
            this.txtBankInfo1,
            this.lblAccountName,
            this.txtAccountName,
            this.txtSubText,
            this.txtConclusion,
            this.lblAmount,
            this.txtTotalAmountHeader,
            this.txtBankInfo2,
            this.txtBankInfo3,
            this.lblTriFoldSymbolRight,
            this.lblTriFoldSymbolLeft,
            this.txtDestinationDepartmentName,
            this.txtDestinationAddressee});
            this.ghReminder.DataField = "CustomerId";
            this.ghReminder.Height = 9.395834F;
            this.ghReminder.Name = "ghReminder";
            this.ghReminder.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            // 
            // txtCustomerPostalCode
            // 
            this.txtCustomerPostalCode.CanGrow = false;
            this.txtCustomerPostalCode.Height = 0.1574803F;
            this.txtCustomerPostalCode.Left = 0.1248032F;
            this.txtCustomerPostalCode.Name = "txtCustomerPostalCode";
            this.txtCustomerPostalCode.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 128";
            this.txtCustomerPostalCode.Text = "〒101-0031";
            this.txtCustomerPostalCode.Top = 0.06889764F;
            this.txtCustomerPostalCode.Width = 0.9799213F;
            // 
            // txtCustomerAddress1
            // 
            this.txtCustomerAddress1.CanGrow = false;
            this.txtCustomerAddress1.Height = 0.1574803F;
            this.txtCustomerAddress1.Left = 0.1248032F;
            this.txtCustomerAddress1.MultiLine = false;
            this.txtCustomerAddress1.Name = "txtCustomerAddress1";
            this.txtCustomerAddress1.ShrinkToFit = true;
            this.txtCustomerAddress1.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 128; ddo-" +
    "shrink-to-fit: true";
            this.txtCustomerAddress1.Text = "得意先住所１２３４５１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０";
            this.txtCustomerAddress1.Top = 0.226378F;
            this.txtCustomerAddress1.Width = 3.40748F;
            // 
            // txtCustomerAddress2
            // 
            this.txtCustomerAddress2.CanGrow = false;
            this.txtCustomerAddress2.Height = 0.1574803F;
            this.txtCustomerAddress2.Left = 0.1248032F;
            this.txtCustomerAddress2.MultiLine = false;
            this.txtCustomerAddress2.Name = "txtCustomerAddress2";
            this.txtCustomerAddress2.ShrinkToFit = true;
            this.txtCustomerAddress2.Style = "font-size: 6pt; text-align: left; vertical-align: middle; ddo-char-set: 128; ddo-" +
    "shrink-to-fit: true";
            this.txtCustomerAddress2.Text = "得意先住所１２３４５１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０";
            this.txtCustomerAddress2.Top = 0.3744095F;
            this.txtCustomerAddress2.Width = 3.40748F;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.CanGrow = false;
            this.txtCustomerName.Height = 0.1574803F;
            this.txtCustomerName.Left = 0.1248032F;
            this.txtCustomerName.MultiLine = false;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.ShrinkToFit = true;
            this.txtCustomerName.Style = "text-align: left; vertical-align: middle; ddo-shrink-to-fit: true";
            this.txtCustomerName.Text = "得意先名５６７８９０１２３４５６７８９０１２３４";
            this.txtCustomerName.Top = 0.6437008F;
            this.txtCustomerName.Width = 3.40748F;
            // 
            // txtOutputNoHeader
            // 
            this.txtOutputNoHeader.CanGrow = false;
            this.txtOutputNoHeader.DataField = "OutputNo";
            this.txtOutputNoHeader.Height = 0.122F;
            this.txtOutputNoHeader.Left = 6F;
            this.txtOutputNoHeader.Name = "txtOutputNoHeader";
            this.txtOutputNoHeader.OutputFormat = resources.GetString("txtOutputNoHeader.OutputFormat");
            this.txtOutputNoHeader.Style = "text-align: right; vertical-align: middle";
            this.txtOutputNoHeader.Text = "000001";
            this.txtOutputNoHeader.Top = 0.06900001F;
            this.txtOutputNoHeader.Width = 0.98F;
            // 
            // txtOutputAtHeader
            // 
            this.txtOutputAtHeader.CanGrow = false;
            this.txtOutputAtHeader.Height = 0.122F;
            this.txtOutputAtHeader.Left = 6F;
            this.txtOutputAtHeader.Name = "txtOutputAtHeader";
            this.txtOutputAtHeader.OutputFormat = resources.GetString("txtOutputAtHeader.OutputFormat");
            this.txtOutputAtHeader.Style = "text-align: right; vertical-align: middle";
            this.txtOutputAtHeader.Text = "yyyy年MM月dd日";
            this.txtOutputAtHeader.Top = 0.191F;
            this.txtOutputAtHeader.Width = 0.98F;
            // 
            // txtCompanyPostalCode
            // 
            this.txtCompanyPostalCode.CanGrow = false;
            this.txtCompanyPostalCode.Height = 0.1574803F;
            this.txtCompanyPostalCode.Left = 4.125197F;
            this.txtCompanyPostalCode.Name = "txtCompanyPostalCode";
            this.txtCompanyPostalCode.Style = "text-align: left; vertical-align: middle";
            this.txtCompanyPostalCode.Text = "〒101-0031";
            this.txtCompanyPostalCode.Top = 0.3838583F;
            this.txtCompanyPostalCode.Width = 0.9799213F;
            // 
            // txtCompanyAddress1
            // 
            this.txtCompanyAddress1.CanGrow = false;
            this.txtCompanyAddress1.Height = 0.1574803F;
            this.txtCompanyAddress1.Left = 4.125197F;
            this.txtCompanyAddress1.Name = "txtCompanyAddress1";
            this.txtCompanyAddress1.Style = "text-align: left; vertical-align: middle";
            this.txtCompanyAddress1.Text = "自社住所５６７８９０１２３４５６７８９０";
            this.txtCompanyAddress1.Top = 0.5413386F;
            this.txtCompanyAddress1.Width = 2.855118F;
            // 
            // txtCompanyAddress2
            // 
            this.txtCompanyAddress2.CanGrow = false;
            this.txtCompanyAddress2.Height = 0.1574803F;
            this.txtCompanyAddress2.Left = 4.124804F;
            this.txtCompanyAddress2.Name = "txtCompanyAddress2";
            this.txtCompanyAddress2.Style = "text-align: left; vertical-align: middle";
            this.txtCompanyAddress2.Text = "自社住所５６７８９０１２３４５６７８９０";
            this.txtCompanyAddress2.Top = 0.6988189F;
            this.txtCompanyAddress2.Width = 2.855118F;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.CanGrow = false;
            this.txtCompanyName.Height = 0.1574803F;
            this.txtCompanyName.Left = 4.125591F;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Style = "text-align: left; vertical-align: middle";
            this.txtCompanyName.Text = "自社名４５６７８９０１２３４５６７８９０";
            this.txtCompanyName.Top = 0.9236221F;
            this.txtCompanyName.Width = 2.855118F;
            // 
            // txtOwnDepartmentName
            // 
            this.txtOwnDepartmentName.CanGrow = false;
            this.txtOwnDepartmentName.Height = 0.1574803F;
            this.txtOwnDepartmentName.Left = 4.125394F;
            this.txtOwnDepartmentName.Name = "txtOwnDepartmentName";
            this.txtOwnDepartmentName.Style = "text-align: left; vertical-align: middle";
            this.txtOwnDepartmentName.Text = "自社部署名６７８９０１２３４５６７８９０";
            this.txtOwnDepartmentName.Top = 1.162929F;
            this.txtOwnDepartmentName.Width = 2.855118F;
            // 
            // txtAccountingStaffName
            // 
            this.txtAccountingStaffName.CanGrow = false;
            this.txtAccountingStaffName.Height = 0.1574803F;
            this.txtAccountingStaffName.Left = 4.125198F;
            this.txtAccountingStaffName.Name = "txtAccountingStaffName";
            this.txtAccountingStaffName.Style = "text-align: left; vertical-align: middle";
            this.txtAccountingStaffName.Text = "経理担当者名７８９０１２３４５６７８９０";
            this.txtAccountingStaffName.Top = 1.320512F;
            this.txtAccountingStaffName.Width = 2.855118F;
            // 
            // txtStaffName
            // 
            this.txtStaffName.CanGrow = false;
            this.txtStaffName.DataField = "StaffName";
            this.txtStaffName.Height = 0.1574803F;
            this.txtStaffName.Left = 4.125198F;
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Style = "text-align: left; vertical-align: middle";
            this.txtStaffName.Text = "回収責任者名７８９０１２３４５６７８９０";
            this.txtStaffName.Top = 1.477992F;
            this.txtStaffName.Width = 2.855118F;
            // 
            // txtStaffTel
            // 
            this.txtStaffTel.CanGrow = false;
            this.txtStaffTel.DataField = "StaffTel";
            this.txtStaffTel.Height = 0.1573938F;
            this.txtStaffTel.Left = 4.540591F;
            this.txtStaffTel.Name = "txtStaffTel";
            this.txtStaffTel.Style = "text-align: left; vertical-align: middle";
            this.txtStaffTel.Text = "01-2345-6789";
            this.txtStaffTel.Top = 1.71815F;
            this.txtStaffTel.Width = 1.367126F;
            // 
            // lblTel
            // 
            this.lblTel.Height = 0.1574803F;
            this.lblTel.HyperLink = null;
            this.lblTel.Left = 4.125591F;
            this.lblTel.Name = "lblTel";
            this.lblTel.Style = "vertical-align: middle";
            this.lblTel.Text = "TEL : ";
            this.lblTel.Top = 1.71815F;
            this.lblTel.Width = 0.4149607F;
            // 
            // txtStaffFax
            // 
            this.txtStaffFax.CanGrow = false;
            this.txtStaffFax.DataField = "StaffFax";
            this.txtStaffFax.Height = 0.1574804F;
            this.txtStaffFax.Left = 4.540395F;
            this.txtStaffFax.Name = "txtStaffFax";
            this.txtStaffFax.Style = "text-align: left; vertical-align: middle";
            this.txtStaffFax.Text = "01-2345-6789";
            this.txtStaffFax.Top = 1.875544F;
            this.txtStaffFax.Width = 1.367125F;
            // 
            // lblFax
            // 
            this.lblFax.Height = 0.1574803F;
            this.lblFax.HyperLink = null;
            this.lblFax.Left = 4.125394F;
            this.lblFax.Name = "lblFax";
            this.lblFax.Style = "vertical-align: middle";
            this.lblFax.Text = "FAX : ";
            this.lblFax.Top = 1.875544F;
            this.lblFax.Width = 0.4149607F;
            // 
            // txtTitle
            // 
            this.txtTitle.CanGrow = false;
            this.txtTitle.Height = 0.289F;
            this.txtTitle.Left = 0.02454865F;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Style = "font-size: 18pt; text-align: center; vertical-align: middle; ddo-char-set: 1";
            this.txtTitle.Text = "ご送金確認のお願い";
            this.txtTitle.Top = 2.631F;
            this.txtTitle.Width = 7.195F;
            // 
            // txtGreeting
            // 
            this.txtGreeting.CanGrow = false;
            this.txtGreeting.Height = 0.1574803F;
            this.txtGreeting.Left = 0.6070867F;
            this.txtGreeting.Name = "txtGreeting";
            this.txtGreeting.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtGreeting.Text = "拝啓";
            this.txtGreeting.Top = 3.287402F;
            this.txtGreeting.Width = 6.031103F;
            // 
            // txtMainText
            // 
            this.txtMainText.CanGrow = false;
            this.txtMainText.Height = 1.476771F;
            this.txtMainText.Left = 0.6070867F;
            this.txtMainText.LineSpacing = 5F;
            this.txtMainText.Name = "txtMainText";
            this.txtMainText.Style = "font-size: 10pt; text-align: left; vertical-align: top; ddo-char-set: 1";
            this.txtMainText.Text = "貴社ますますご清祥のこととお喜び申し上げます。\r\n平素は格別のご高配を賜り、厚くお礼申し上げます。\r\nさて、下記債権明細記載の請求書に対するご入金が先月末日時点に" +
    "て確認が取れておりません。\r\nお手数とは存じますが、下記明細をご確認の上、至急下記の銀行口座にお支払い頂ますよう、お願い申し上げます。";
            this.txtMainText.Top = 3.496851F;
            this.txtMainText.Width = 6.031001F;
            // 
            // lblBank
            // 
            this.lblBank.Height = 0.1574803F;
            this.lblBank.HyperLink = null;
            this.lblBank.Left = 0.6070867F;
            this.lblBank.Name = "lblBank";
            this.lblBank.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.lblBank.Text = "お振込銀行　　：";
            this.lblBank.Top = 7.248821F;
            this.lblBank.Width = 1.20748F;
            // 
            // txtBankInfo1
            // 
            this.txtBankInfo1.CanGrow = false;
            this.txtBankInfo1.Height = 0.1574803F;
            this.txtBankInfo1.Left = 1.814567F;
            this.txtBankInfo1.Name = "txtBankInfo1";
            this.txtBankInfo1.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtBankInfo1.Text = null;
            this.txtBankInfo1.Top = 7.248821F;
            this.txtBankInfo1.Width = 4.585039F;
            // 
            // lblAccountName
            // 
            this.lblAccountName.Height = 0.1574803F;
            this.lblAccountName.HyperLink = null;
            this.lblAccountName.Left = 0.6070867F;
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.lblAccountName.Text = "口座振込人名称：";
            this.lblAccountName.Top = 7.044096F;
            this.lblAccountName.Width = 1.207479F;
            // 
            // txtAccountName
            // 
            this.txtAccountName.CanGrow = false;
            this.txtAccountName.Height = 0.1574803F;
            this.txtAccountName.Left = 1.814567F;
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtAccountName.Text = "アール・アンド・エー・シー";
            this.txtAccountName.Top = 7.044096F;
            this.txtAccountName.Width = 4.58504F;
            // 
            // txtSubText
            // 
            this.txtSubText.CanGrow = false;
            this.txtSubText.Height = 0.8724412F;
            this.txtSubText.Left = 0.6070867F;
            this.txtSubText.LineSpacing = 5F;
            this.txtSubText.Name = "txtSubText";
            this.txtSubText.Style = "font-size: 10pt; text-align: left; vertical-align: top; ddo-char-set: 1";
            this.txtSubText.Text = "尚、本書と行き違いで既にお支払いを頂いております場合は、何卒ご容赦くださいますよう\r\n宜しくお願い申し上げます。";
            this.txtSubText.Top = 5.148426F;
            this.txtSubText.Width = 6.031001F;
            // 
            // txtConclusion
            // 
            this.txtConclusion.CanGrow = false;
            this.txtConclusion.Height = 0.1574803F;
            this.txtConclusion.Left = 0.6070867F;
            this.txtConclusion.Name = "txtConclusion";
            this.txtConclusion.Style = "font-size: 10pt; text-align: right; vertical-align: middle; ddo-char-set: 1";
            this.txtConclusion.Text = "敬具";
            this.txtConclusion.Top = 6.083071F;
            this.txtConclusion.Width = 6.031103F;
            // 
            // lblAmount
            // 
            this.lblAmount.Height = 0.1574803F;
            this.lblAmount.HyperLink = null;
            this.lblAmount.Left = 0.6070867F;
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.lblAmount.Text = "お支払金額　　：";
            this.lblAmount.Top = 6.834646F;
            this.lblAmount.Width = 1.20748F;
            // 
            // txtTotalAmountHeader
            // 
            this.txtTotalAmountHeader.CanGrow = false;
            this.txtTotalAmountHeader.DataField = "RemainAmount";
            this.txtTotalAmountHeader.Height = 0.1574803F;
            this.txtTotalAmountHeader.Left = 1.814567F;
            this.txtTotalAmountHeader.Name = "txtTotalAmountHeader";
            this.txtTotalAmountHeader.OutputFormat = resources.GetString("txtTotalAmountHeader.OutputFormat");
            this.txtTotalAmountHeader.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtTotalAmountHeader.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.txtTotalAmountHeader.Text = "9,999,999,999円";
            this.txtTotalAmountHeader.Top = 6.834648F;
            this.txtTotalAmountHeader.Width = 1.949213F;
            // 
            // txtBankInfo2
            // 
            this.txtBankInfo2.CanGrow = false;
            this.txtBankInfo2.Height = 0.1574803F;
            this.txtBankInfo2.Left = 1.814567F;
            this.txtBankInfo2.Name = "txtBankInfo2";
            this.txtBankInfo2.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtBankInfo2.Text = null;
            this.txtBankInfo2.Top = 7.457088F;
            this.txtBankInfo2.Width = 4.585039F;
            // 
            // txtBankInfo3
            // 
            this.txtBankInfo3.CanGrow = false;
            this.txtBankInfo3.Height = 0.1574803F;
            this.txtBankInfo3.Left = 1.814567F;
            this.txtBankInfo3.Name = "txtBankInfo3";
            this.txtBankInfo3.Style = "font-size: 10pt; text-align: left; vertical-align: middle; ddo-char-set: 1";
            this.txtBankInfo3.Text = null;
            this.txtBankInfo3.Top = 7.673623F;
            this.txtBankInfo3.Width = 4.585039F;
            // 
            // lblTriFoldSymbolRight
            // 
            this.lblTriFoldSymbolRight.Height = 0.1685039F;
            this.lblTriFoldSymbolRight.HyperLink = null;
            this.lblTriFoldSymbolRight.Left = 7.138977F;
            this.lblTriFoldSymbolRight.Name = "lblTriFoldSymbolRight";
            this.lblTriFoldSymbolRight.Style = "font-size: 9.75pt; text-align: left; text-decoration: none; ddo-char-set: 128";
            this.lblTriFoldSymbolRight.Text = "<";
            this.lblTriFoldSymbolRight.Top = 3.215748F;
            this.lblTriFoldSymbolRight.Width = 0.1059055F;
            // 
            // lblTriFoldSymbolLeft
            // 
            this.lblTriFoldSymbolLeft.Height = 0.1685039F;
            this.lblTriFoldSymbolLeft.HyperLink = null;
            this.lblTriFoldSymbolLeft.Left = 0F;
            this.lblTriFoldSymbolLeft.Name = "lblTriFoldSymbolLeft";
            this.lblTriFoldSymbolLeft.Style = "font-family: ＭＳ 明朝; font-size: 9.75pt; text-align: left; text-decoration: none; d" +
    "do-char-set: 128";
            this.lblTriFoldSymbolLeft.Text = ">";
            this.lblTriFoldSymbolLeft.Top = 3.215748F;
            this.lblTriFoldSymbolLeft.Width = 0.1059054F;
            // 
            // txtDestinationDepartmentName
            // 
            this.txtDestinationDepartmentName.CanGrow = false;
            this.txtDestinationDepartmentName.Height = 0.1574803F;
            this.txtDestinationDepartmentName.Left = 0.1248032F;
            this.txtDestinationDepartmentName.MultiLine = false;
            this.txtDestinationDepartmentName.Name = "txtDestinationDepartmentName";
            this.txtDestinationDepartmentName.ShrinkToFit = true;
            this.txtDestinationDepartmentName.Style = "text-align: left; vertical-align: middle; ddo-shrink-to-fit: true";
            this.txtDestinationDepartmentName.Text = "送付先部署８９０１２３４５６７８９０";
            this.txtDestinationDepartmentName.Top = 0.8661418F;
            this.txtDestinationDepartmentName.Width = 3.40748F;
            // 
            // txtDestinationAddressee
            // 
            this.txtDestinationAddressee.CanGrow = false;
            this.txtDestinationAddressee.Height = 0.1574803F;
            this.txtDestinationAddressee.Left = 0.1248032F;
            this.txtDestinationAddressee.MultiLine = false;
            this.txtDestinationAddressee.Name = "txtDestinationAddressee";
            this.txtDestinationAddressee.ShrinkToFit = true;
            this.txtDestinationAddressee.Style = "text-align: left; vertical-align: middle; ddo-shrink-to-fit: true";
            this.txtDestinationAddressee.Text = "送付先宛名８９０１２３４５６７８９０";
            this.txtDestinationAddressee.Top = 1.102363F;
            this.txtDestinationAddressee.Width = 3.40748F;
            // 
            // gfDetail
            // 
            this.gfDetail.Height = 0F;
            this.gfDetail.Name = "gfDetail";
            // 
            // ghDetail
            // 
            this.ghDetail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lblDetailTitle,
            this.lblNote,
            this.lblBillingStaffName,
            this.lblBillingAmount,
            this.lblDueAt,
            this.label4,
            this.line2,
            this.line7,
            this.line9,
            this.line12,
            this.line15,
            this.line18,
            this.line19,
            this.line20,
            this.line21,
            this.txtTotalAmount,
            this.lblTotalAmount,
            this.lineTotalAmountTop,
            this.lineTotalAmountBottom,
            this.lineTotalAmountLeft,
            this.lineTotalAmountCenter,
            this.lineTotalAmountRight,
            this.txtOutputNoDetail,
            this.txtOutputAtDetail,
            this.txtCustomerStaffNameDetail});
            this.ghDetail.DataField = "CustomerId";
            this.ghDetail.Height = 1.647897F;
            this.ghDetail.Name = "ghDetail";
            this.ghDetail.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            this.ghDetail.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.Height = 0.2311024F;
            this.lblDetailTitle.HyperLink = null;
            this.lblDetailTitle.Left = 0.0007874016F;
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Style = "font-size: 14pt; text-align: center";
            this.lblDetailTitle.Text = "債権明細";
            this.lblDetailTitle.Top = 0.5669292F;
            this.lblDetailTitle.Width = 7.244097F;
            // 
            // lblNote
            // 
            this.lblNote.Height = 0.306F;
            this.lblNote.HyperLink = null;
            this.lblNote.Left = 0.9493307F;
            this.lblNote.Name = "lblNote";
            this.lblNote.Style = "text-align: center; vertical-align: middle";
            this.lblNote.Text = "備考５";
            this.lblNote.Top = 1.331496F;
            this.lblNote.Width = 3.347914F;
            // 
            // lblBillingStaffName
            // 
            this.lblBillingStaffName.Height = 0.306F;
            this.lblBillingStaffName.HyperLink = null;
            this.lblBillingStaffName.Left = 4.297245F;
            this.lblBillingStaffName.Name = "lblBillingStaffName";
            this.lblBillingStaffName.Style = "text-align: center; vertical-align: middle";
            this.lblBillingStaffName.Text = "弊社担当";
            this.lblBillingStaffName.Top = 1.331496F;
            this.lblBillingStaffName.Width = 1.058268F;
            // 
            // lblBillingAmount
            // 
            this.lblBillingAmount.Height = 0.306F;
            this.lblBillingAmount.HyperLink = null;
            this.lblBillingAmount.Left = 5.355512F;
            this.lblBillingAmount.Name = "lblBillingAmount";
            this.lblBillingAmount.Style = "text-align: center; vertical-align: middle";
            this.lblBillingAmount.Text = "金額";
            this.lblBillingAmount.Top = 1.331496F;
            this.lblBillingAmount.Width = 1.037915F;
            // 
            // lblDueAt
            // 
            this.lblDueAt.Height = 0.306F;
            this.lblDueAt.HyperLink = null;
            this.lblDueAt.Left = 6.393426F;
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Style = "text-align: center; vertical-align: middle";
            this.lblDueAt.Text = "支払期日";
            this.lblDueAt.Top = 1.331102F;
            this.lblDueAt.Width = 0.8255906F;
            // 
            // label4
            // 
            this.label4.Height = 0.3059055F;
            this.label4.HyperLink = null;
            this.label4.Left = 0.2543307F;
            this.label4.Name = "label4";
            this.label4.Style = "text-align: center; vertical-align: middle";
            this.label4.Text = "取引日";
            this.label4.Top = 1.331394F;
            this.label4.Width = 0.6950001F;
            // 
            // line2
            // 
            this.line2.Height = 0.0001020432F;
            this.line2.Left = 0.02433073F;
            this.line2.LineWeight = 1F;
            this.line2.Name = "line2";
            this.line2.Top = 1.331394F;
            this.line2.Width = 7.194686F;
            this.line2.X1 = 0.02433073F;
            this.line2.X2 = 7.219017F;
            this.line2.Y1 = 1.331394F;
            this.line2.Y2 = 1.331496F;
            // 
            // line7
            // 
            this.line7.Height = 0.0003859997F;
            this.line7.Left = 0.02433073F;
            this.line7.LineWeight = 1F;
            this.line7.Name = "line7";
            this.line7.Top = 1.637008F;
            this.line7.Width = 7.194686F;
            this.line7.X1 = 0.02433073F;
            this.line7.X2 = 7.219017F;
            this.line7.Y1 = 1.637394F;
            this.line7.Y2 = 1.637008F;
            // 
            // line9
            // 
            this.line9.Height = 0.306F;
            this.line9.Left = 0.02440945F;
            this.line9.LineWeight = 1F;
            this.line9.Name = "line9";
            this.line9.Top = 1.331394F;
            this.line9.Width = 0F;
            this.line9.X1 = 0.02440945F;
            this.line9.X2 = 0.02440945F;
            this.line9.Y1 = 1.331394F;
            this.line9.Y2 = 1.637394F;
            // 
            // line12
            // 
            this.line12.Height = 0.306F;
            this.line12.Left = 0.2543307F;
            this.line12.LineWeight = 1F;
            this.line12.Name = "line12";
            this.line12.Top = 1.331394F;
            this.line12.Width = 0F;
            this.line12.X1 = 0.2543307F;
            this.line12.X2 = 0.2543307F;
            this.line12.Y1 = 1.331394F;
            this.line12.Y2 = 1.637394F;
            // 
            // line15
            // 
            this.line15.Height = 0.3060009F;
            this.line15.Left = 0.9494257F;
            this.line15.LineWeight = 1F;
            this.line15.Name = "line15";
            this.line15.Top = 1.331496F;
            this.line15.Width = 0F;
            this.line15.X1 = 0.9494257F;
            this.line15.X2 = 0.9494257F;
            this.line15.Y1 = 1.331496F;
            this.line15.Y2 = 1.637497F;
            // 
            // line18
            // 
            this.line18.Height = 0.3060011F;
            this.line18.Left = 4.297245F;
            this.line18.LineWeight = 1F;
            this.line18.Name = "line18";
            this.line18.Top = 1.331F;
            this.line18.Width = 0F;
            this.line18.X1 = 4.297245F;
            this.line18.X2 = 4.297245F;
            this.line18.Y1 = 1.331F;
            this.line18.Y2 = 1.637001F;
            // 
            // line19
            // 
            this.line19.Height = 0.306F;
            this.line19.Left = 5.355512F;
            this.line19.LineWeight = 1F;
            this.line19.Name = "line19";
            this.line19.Top = 1.331394F;
            this.line19.Width = 0F;
            this.line19.X1 = 5.355512F;
            this.line19.X2 = 5.355512F;
            this.line19.Y1 = 1.331394F;
            this.line19.Y2 = 1.637394F;
            // 
            // line20
            // 
            this.line20.Height = 0.3060009F;
            this.line20.Left = 6.393426F;
            this.line20.LineWeight = 1F;
            this.line20.Name = "line20";
            this.line20.Top = 1.331496F;
            this.line20.Width = 0F;
            this.line20.X1 = 6.393426F;
            this.line20.X2 = 6.393426F;
            this.line20.Y1 = 1.331496F;
            this.line20.Y2 = 1.637497F;
            // 
            // line21
            // 
            this.line21.Height = 0.3060009F;
            this.line21.Left = 7.219016F;
            this.line21.LineWeight = 1F;
            this.line21.Name = "line21";
            this.line21.Top = 1.331102F;
            this.line21.Width = 0F;
            this.line21.X1 = 7.219016F;
            this.line21.X2 = 7.219016F;
            this.line21.Y1 = 1.331102F;
            this.line21.Y2 = 1.637103F;
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.CanGrow = false;
            this.txtTotalAmount.Height = 0.3059055F;
            this.txtTotalAmount.Left = 5.357087F;
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.OutputFormat = resources.GetString("txtTotalAmount.OutputFormat");
            this.txtTotalAmount.Style = "text-align: right; vertical-align: middle";
            this.txtTotalAmount.Text = "-123,456,789,012";
            this.txtTotalAmount.Top = 0.9780553F;
            this.txtTotalAmount.Width = 1.858267F;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Height = 0.3059055F;
            this.lblTotalAmount.HyperLink = null;
            this.lblTotalAmount.Left = 4.298819F;
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Style = "text-align: center; vertical-align: middle";
            this.lblTotalAmount.Text = "合計金額";
            this.lblTotalAmount.Top = 0.9779528F;
            this.lblTotalAmount.Width = 1.058268F;
            // 
            // lineTotalAmountTop
            // 
            this.lineTotalAmountTop.Height = 0F;
            this.lineTotalAmountTop.Left = 4.298819F;
            this.lineTotalAmountTop.LineWeight = 1F;
            this.lineTotalAmountTop.Name = "lineTotalAmountTop";
            this.lineTotalAmountTop.Top = 0.9779528F;
            this.lineTotalAmountTop.Width = 2.921654F;
            this.lineTotalAmountTop.X1 = 4.298819F;
            this.lineTotalAmountTop.X2 = 7.220473F;
            this.lineTotalAmountTop.Y1 = 0.9779528F;
            this.lineTotalAmountTop.Y2 = 0.9779528F;
            // 
            // lineTotalAmountBottom
            // 
            this.lineTotalAmountBottom.Height = 0F;
            this.lineTotalAmountBottom.Left = 4.298032F;
            this.lineTotalAmountBottom.LineWeight = 1F;
            this.lineTotalAmountBottom.Name = "lineTotalAmountBottom";
            this.lineTotalAmountBottom.Top = 1.283858F;
            this.lineTotalAmountBottom.Width = 2.921654F;
            this.lineTotalAmountBottom.X1 = 4.298032F;
            this.lineTotalAmountBottom.X2 = 7.219686F;
            this.lineTotalAmountBottom.Y1 = 1.283858F;
            this.lineTotalAmountBottom.Y2 = 1.283858F;
            // 
            // lineTotalAmountLeft
            // 
            this.lineTotalAmountLeft.Height = 0.3059052F;
            this.lineTotalAmountLeft.Left = 4.298819F;
            this.lineTotalAmountLeft.LineWeight = 1F;
            this.lineTotalAmountLeft.Name = "lineTotalAmountLeft";
            this.lineTotalAmountLeft.Top = 0.9779528F;
            this.lineTotalAmountLeft.Width = 0F;
            this.lineTotalAmountLeft.X1 = 4.298819F;
            this.lineTotalAmountLeft.X2 = 4.298819F;
            this.lineTotalAmountLeft.Y1 = 0.9779528F;
            this.lineTotalAmountLeft.Y2 = 1.283858F;
            // 
            // lineTotalAmountCenter
            // 
            this.lineTotalAmountCenter.Height = 0.3059052F;
            this.lineTotalAmountCenter.Left = 5.357087F;
            this.lineTotalAmountCenter.LineWeight = 1F;
            this.lineTotalAmountCenter.Name = "lineTotalAmountCenter";
            this.lineTotalAmountCenter.Top = 0.9779528F;
            this.lineTotalAmountCenter.Width = 0F;
            this.lineTotalAmountCenter.X1 = 5.357087F;
            this.lineTotalAmountCenter.X2 = 5.357087F;
            this.lineTotalAmountCenter.Y1 = 0.9779528F;
            this.lineTotalAmountCenter.Y2 = 1.283858F;
            // 
            // lineTotalAmountRight
            // 
            this.lineTotalAmountRight.Height = 0.3059052F;
            this.lineTotalAmountRight.Left = 7.219686F;
            this.lineTotalAmountRight.LineWeight = 1F;
            this.lineTotalAmountRight.Name = "lineTotalAmountRight";
            this.lineTotalAmountRight.Top = 0.9779528F;
            this.lineTotalAmountRight.Width = 0F;
            this.lineTotalAmountRight.X1 = 7.219686F;
            this.lineTotalAmountRight.X2 = 7.219686F;
            this.lineTotalAmountRight.Y1 = 0.9779528F;
            this.lineTotalAmountRight.Y2 = 1.283858F;
            // 
            // txtOutputNoDetail
            // 
            this.txtOutputNoDetail.CanGrow = false;
            this.txtOutputNoDetail.DataField = "OutputNo";
            this.txtOutputNoDetail.Height = 0.122F;
            this.txtOutputNoDetail.Left = 6F;
            this.txtOutputNoDetail.Name = "txtOutputNoDetail";
            this.txtOutputNoDetail.OutputFormat = resources.GetString("txtOutputNoDetail.OutputFormat");
            this.txtOutputNoDetail.Style = "text-align: right; vertical-align: middle";
            this.txtOutputNoDetail.Text = "000001";
            this.txtOutputNoDetail.Top = 0.06889763F;
            this.txtOutputNoDetail.Width = 0.98F;
            // 
            // txtOutputAtDetail
            // 
            this.txtOutputAtDetail.CanGrow = false;
            this.txtOutputAtDetail.Height = 0.122F;
            this.txtOutputAtDetail.Left = 6F;
            this.txtOutputAtDetail.Name = "txtOutputAtDetail";
            this.txtOutputAtDetail.Style = "text-align: right; vertical-align: middle";
            this.txtOutputAtDetail.Text = "yyyy年MM月dd日";
            this.txtOutputAtDetail.Top = 0.1909449F;
            this.txtOutputAtDetail.Width = 0.98F;
            // 
            // txtCustomerStaffNameDetail
            // 
            this.txtCustomerStaffNameDetail.CanGrow = false;
            this.txtCustomerStaffNameDetail.Height = 0.1574803F;
            this.txtCustomerStaffNameDetail.Left = 0.1251968F;
            this.txtCustomerStaffNameDetail.MultiLine = false;
            this.txtCustomerStaffNameDetail.Name = "txtCustomerStaffNameDetail";
            this.txtCustomerStaffNameDetail.ShrinkToFit = true;
            this.txtCustomerStaffNameDetail.Style = "text-align: left; vertical-align: middle; ddo-shrink-to-fit: true";
            this.txtCustomerStaffNameDetail.Text = null;
            this.txtCustomerStaffNameDetail.Top = 0.1909449F;
            this.txtCustomerStaffNameDetail.Width = 3.40748F;
            // 
            // gfReminder
            // 
            this.gfReminder.Height = 0F;
            this.gfReminder.Name = "gfReminder";
            // 
            // ReminderReport
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
            this.PrintWidth = 7.244884F;
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.ghReminder);
            this.Sections.Add(this.ghDetail);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.gfReminder);
            this.Sections.Add(this.gfDetail);
            this.Sections.Add(this.pageFooter);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; color: Black; fon" +
            "t-family: \"ＭＳ 明朝\"; ddo-char-set: 186; font-size: 9pt", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; font-family: \"MS UI Gothic\"; ddo-char-set: 12" +
            "8", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 14pt; font-weight: bold; font-style: inherit; font-family: \"MS UI Goth" +
            "ic\"; ddo-char-set: 128", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRowNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerPostalCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerAddress1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerAddress2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputNoHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputAtHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyPostalCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyAddress1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyAddress2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompanyName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOwnDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountingStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffFax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGreeting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMainText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccountName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccountName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConclusion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmountHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankInfo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankInfo3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTriFoldSymbolRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTriFoldSymbolLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationAddressee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDetailTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputNoDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputAtDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerStaffNameDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtRowNumber;
        private GrapeCity.ActiveReports.SectionReportModel.Line line14;
        private GrapeCity.ActiveReports.SectionReportModel.Line line10;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox textBox3;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Line line1;
        private GrapeCity.ActiveReports.SectionReportModel.Line line24;
        private GrapeCity.ActiveReports.SectionReportModel.Line line25;
        private GrapeCity.ActiveReports.SectionReportModel.Line line26;
        private GrapeCity.ActiveReports.SectionReportModel.Line line27;
        private GrapeCity.ActiveReports.SectionReportModel.Line line28;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghReminder;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtOutputNoHeader;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBank;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAccountName;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfDetail;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader ghDetail;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDetailTitle;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingStaffName;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblBillingAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblDueAt;
        private GrapeCity.ActiveReports.SectionReportModel.Label label4;
        private GrapeCity.ActiveReports.SectionReportModel.Line line2;
        private GrapeCity.ActiveReports.SectionReportModel.Line line7;
        private GrapeCity.ActiveReports.SectionReportModel.Line line9;
        private GrapeCity.ActiveReports.SectionReportModel.Line line12;
        private GrapeCity.ActiveReports.SectionReportModel.Line line15;
        private GrapeCity.ActiveReports.SectionReportModel.Line line18;
        private GrapeCity.ActiveReports.SectionReportModel.Line line19;
        private GrapeCity.ActiveReports.SectionReportModel.Line line20;
        private GrapeCity.ActiveReports.SectionReportModel.Line line21;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineTotalAmountTop;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineTotalAmountBottom;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineTotalAmountLeft;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineTotalAmountCenter;
        private GrapeCity.ActiveReports.SectionReportModel.Line lineTotalAmountRight;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter gfReminder;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtCompanyPostalCode;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtCompanyAddress1;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtCompanyAddress2;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtCompanyName;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtOwnDepartmentName;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountingStaffName;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtTitle;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtGreeting;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtMainText;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtAccountName;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtSubText;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtConclusion;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffName;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffTel;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblTel;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtStaffFax;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblFax;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtOutputNoDetail;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtOutputAtHeader;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtOutputAtDetail;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerStaffNameDetail;
        public GrapeCity.ActiveReports.SectionReportModel.Label lblNote;
        private GrapeCity.ActiveReports.SectionReportModel.Detail detail;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblAmount;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankInfo2;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox txtBankInfo3;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtTotalAmountHeader;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtNote;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTriFoldSymbolRight;
        private GrapeCity.ActiveReports.SectionReportModel.Label lblTriFoldSymbolLeft;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerPostalCode;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerAddress1;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerAddress2;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtCustomerName;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtDestinationDepartmentName;
        public GrapeCity.ActiveReports.SectionReportModel.TextBox txtDestinationAddressee;
    }
}
