namespace Rac.VOne.Client.Screen
{
    partial class PF0401
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            GrapeCity.Win.MultiRow.ShortcutKeyManager shortcutKeyManager2 = new GrapeCity.Win.MultiRow.ShortcutKeyManager();
            this.tbcArrearagesList = new System.Windows.Forms.TabControl();
            this.tabArrearagesListSearch = new System.Windows.Forms.TabPage();
            this.lblFromDepartment = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblPaymentDate = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtFromDepartmentCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnFromDepartment = new System.Windows.Forms.Button();
            this.lblFromDepartmentName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.datPaymentDate = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblDepartmentWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtMemo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cbxDepartment = new System.Windows.Forms.CheckBox();
            this.cbxCustomer = new System.Windows.Forms.CheckBox();
            this.txtToDapartmentCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCurrency = new System.Windows.Forms.Button();
            this.btnToDepartment = new System.Windows.Forms.Button();
            this.lblCurrency = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblToDepartmentName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblStaff = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxMemo = new System.Windows.Forms.CheckBox();
            this.txtFromStaffCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblToStaffName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.btnFromStaff = new System.Windows.Forms.Button();
            this.btnToStaff = new System.Windows.Forms.Button();
            this.lblFromStaffName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtToStaffCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblStaffWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxStaff = new System.Windows.Forms.CheckBox();
            this.tabArrearagesListResult = new System.Windows.Forms.TabPage();
            this.lblTotalAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblRecoveryTotalAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.grdArrearagesList = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.tbcArrearagesList.SuspendLayout();
            this.tabArrearagesListSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFromDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datPaymentDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDapartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFromStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToStaffCode)).BeginInit();
            this.tabArrearagesListResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblRecoveryTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdArrearagesList)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcArrearagesList
            // 
            this.tbcArrearagesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcArrearagesList.Controls.Add(this.tabArrearagesListSearch);
            this.tbcArrearagesList.Controls.Add(this.tabArrearagesListResult);
            this.tbcArrearagesList.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbcArrearagesList.Location = new System.Drawing.Point(15, 15);
            this.tbcArrearagesList.Name = "tbcArrearagesList";
            this.tbcArrearagesList.SelectedIndex = 0;
            this.tbcArrearagesList.Size = new System.Drawing.Size(978, 591);
            this.tbcArrearagesList.TabIndex = 0;
            // 
            // tabArrearagesListSearch
            // 
            this.tabArrearagesListSearch.Controls.Add(this.lblFromDepartment);
            this.tabArrearagesListSearch.Controls.Add(this.lblPaymentDate);
            this.tabArrearagesListSearch.Controls.Add(this.txtFromDepartmentCode);
            this.tabArrearagesListSearch.Controls.Add(this.btnFromDepartment);
            this.tabArrearagesListSearch.Controls.Add(this.lblFromDepartmentName);
            this.tabArrearagesListSearch.Controls.Add(this.datPaymentDate);
            this.tabArrearagesListSearch.Controls.Add(this.lblDepartmentWave);
            this.tabArrearagesListSearch.Controls.Add(this.txtMemo);
            this.tabArrearagesListSearch.Controls.Add(this.cbxDepartment);
            this.tabArrearagesListSearch.Controls.Add(this.cbxCustomer);
            this.tabArrearagesListSearch.Controls.Add(this.txtToDapartmentCode);
            this.tabArrearagesListSearch.Controls.Add(this.btnCurrency);
            this.tabArrearagesListSearch.Controls.Add(this.btnToDepartment);
            this.tabArrearagesListSearch.Controls.Add(this.lblCurrency);
            this.tabArrearagesListSearch.Controls.Add(this.lblToDepartmentName);
            this.tabArrearagesListSearch.Controls.Add(this.txtCurrencyCode);
            this.tabArrearagesListSearch.Controls.Add(this.lblStaff);
            this.tabArrearagesListSearch.Controls.Add(this.cbxMemo);
            this.tabArrearagesListSearch.Controls.Add(this.txtFromStaffCode);
            this.tabArrearagesListSearch.Controls.Add(this.lblToStaffName);
            this.tabArrearagesListSearch.Controls.Add(this.btnFromStaff);
            this.tabArrearagesListSearch.Controls.Add(this.btnToStaff);
            this.tabArrearagesListSearch.Controls.Add(this.lblFromStaffName);
            this.tabArrearagesListSearch.Controls.Add(this.txtToStaffCode);
            this.tabArrearagesListSearch.Controls.Add(this.lblStaffWave);
            this.tabArrearagesListSearch.Controls.Add(this.cbxStaff);
            this.tabArrearagesListSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabArrearagesListSearch.Location = new System.Drawing.Point(4, 24);
            this.tabArrearagesListSearch.Name = "tabArrearagesListSearch";
            this.tabArrearagesListSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabArrearagesListSearch.Size = new System.Drawing.Size(970, 563);
            this.tabArrearagesListSearch.TabIndex = 0;
            this.tabArrearagesListSearch.Text = "検索条件";
            this.tabArrearagesListSearch.UseVisualStyleBackColor = true;
            // 
            // lblFromDepartment
            // 
            this.lblFromDepartment.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDepartment.Location = new System.Drawing.Point(421, 20);
            this.lblFromDepartment.Name = "lblFromDepartment";
            this.lblFromDepartment.Size = new System.Drawing.Size(81, 16);
            this.lblFromDepartment.TabIndex = 0;
            this.lblFromDepartment.Text = "請求部門コード";
            this.lblFromDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPaymentDate
            // 
            this.lblPaymentDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentDate.Location = new System.Drawing.Point(21, 20);
            this.lblPaymentDate.Margin = new System.Windows.Forms.Padding(18, 0, 3, 0);
            this.lblPaymentDate.Name = "lblPaymentDate";
            this.lblPaymentDate.Size = new System.Drawing.Size(67, 16);
            this.lblPaymentDate.TabIndex = 0;
            this.lblPaymentDate.Text = "入金基準日";
            this.lblPaymentDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFromDepartmentCode
            // 
            this.txtFromDepartmentCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtFromDepartmentCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFromDepartmentCode.DropDown.AllowDrop = false;
            this.txtFromDepartmentCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromDepartmentCode.HighlightText = true;
            this.txtFromDepartmentCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtFromDepartmentCode.Location = new System.Drawing.Point(508, 18);
            this.txtFromDepartmentCode.Margin = new System.Windows.Forms.Padding(3, 18, 3, 4);
            this.txtFromDepartmentCode.Name = "txtFromDepartmentCode";
            this.txtFromDepartmentCode.Required = false;
            this.txtFromDepartmentCode.Size = new System.Drawing.Size(115, 22);
            this.txtFromDepartmentCode.TabIndex = 8;
            this.txtFromDepartmentCode.Validated += new System.EventHandler(this.txtFromDepartmentCode_Validated);
            // 
            // btnFromDepartment
            // 
            this.btnFromDepartment.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFromDepartment.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnFromDepartment.Location = new System.Drawing.Point(629, 16);
            this.btnFromDepartment.Name = "btnFromDepartment";
            this.btnFromDepartment.Size = new System.Drawing.Size(24, 24);
            this.btnFromDepartment.TabIndex = 9;
            this.btnFromDepartment.UseVisualStyleBackColor = true;
            this.btnFromDepartment.Click += new System.EventHandler(this.btnFromDepartment_Click);
            // 
            // lblFromDepartmentName
            // 
            this.lblFromDepartmentName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFromDepartmentName.DropDown.AllowDrop = false;
            this.lblFromDepartmentName.Enabled = false;
            this.lblFromDepartmentName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDepartmentName.HighlightText = true;
            this.lblFromDepartmentName.Location = new System.Drawing.Point(659, 18);
            this.lblFromDepartmentName.Margin = new System.Windows.Forms.Padding(3, 15, 18, 4);
            this.lblFromDepartmentName.Name = "lblFromDepartmentName";
            this.lblFromDepartmentName.ReadOnly = true;
            this.lblFromDepartmentName.Required = false;
            this.lblFromDepartmentName.Size = new System.Drawing.Size(290, 22);
            this.lblFromDepartmentName.TabIndex = 0;
            // 
            // datPaymentDate
            // 
            this.datPaymentDate.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datPaymentDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datPaymentDate.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datPaymentDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datPaymentDate.Location = new System.Drawing.Point(94, 18);
            this.datPaymentDate.Margin = new System.Windows.Forms.Padding(3, 15, 3, 4);
            this.datPaymentDate.Name = "datPaymentDate";
            this.datPaymentDate.Required = true;
            this.datPaymentDate.Size = new System.Drawing.Size(115, 22);
            this.datPaymentDate.Spin.AllowSpin = false;
            this.datPaymentDate.TabIndex = 2;
            this.datPaymentDate.Value = null;
            // 
            // lblDepartmentWave
            // 
            this.lblDepartmentWave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentWave.Location = new System.Drawing.Point(465, 50);
            this.lblDepartmentWave.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblDepartmentWave.Name = "lblDepartmentWave";
            this.lblDepartmentWave.Size = new System.Drawing.Size(20, 16);
            this.lblDepartmentWave.TabIndex = 0;
            this.lblDepartmentWave.Text = "～";
            this.lblDepartmentWave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMemo
            // 
            this.txtMemo.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMemo.DropDown.AllowDrop = false;
            this.txtMemo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemo.HighlightText = true;
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(94, 48);
            this.txtMemo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Required = false;
            this.txtMemo.Size = new System.Drawing.Size(270, 22);
            this.txtMemo.TabIndex = 5;
            // 
            // cbxDepartment
            // 
            this.cbxDepartment.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxDepartment.Location = new System.Drawing.Point(486, 50);
            this.cbxDepartment.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.cbxDepartment.Name = "cbxDepartment";
            this.cbxDepartment.Size = new System.Drawing.Size(16, 18);
            this.cbxDepartment.TabIndex = 0;
            this.cbxDepartment.TabStop = false;
            this.cbxDepartment.UseVisualStyleBackColor = true;
            // 
            // cbxCustomer
            // 
            this.cbxCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCustomer.Location = new System.Drawing.Point(257, 20);
            this.cbxCustomer.Margin = new System.Windows.Forms.Padding(15, 3, 3, 4);
            this.cbxCustomer.Name = "cbxCustomer";
            this.cbxCustomer.Size = new System.Drawing.Size(107, 18);
            this.cbxCustomer.TabIndex = 3;
            this.cbxCustomer.Text = "得意先毎に集計";
            this.cbxCustomer.UseVisualStyleBackColor = true;
            this.cbxCustomer.Click += new System.EventHandler(this.cbxCustomer_Click);
            // 
            // txtToDapartmentCode
            // 
            this.txtToDapartmentCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtToDapartmentCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtToDapartmentCode.DropDown.AllowDrop = false;
            this.txtToDapartmentCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToDapartmentCode.HighlightText = true;
            this.txtToDapartmentCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtToDapartmentCode.Location = new System.Drawing.Point(508, 48);
            this.txtToDapartmentCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtToDapartmentCode.Name = "txtToDapartmentCode";
            this.txtToDapartmentCode.Required = false;
            this.txtToDapartmentCode.Size = new System.Drawing.Size(115, 22);
            this.txtToDapartmentCode.TabIndex = 10;
            this.txtToDapartmentCode.Validated += new System.EventHandler(this.txtToDapartmentCode_Validated);
            // 
            // btnCurrency
            // 
            this.btnCurrency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCurrency.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrency.Location = new System.Drawing.Point(140, 77);
            this.btnCurrency.Name = "btnCurrency";
            this.btnCurrency.Size = new System.Drawing.Size(24, 24);
            this.btnCurrency.TabIndex = 7;
            this.btnCurrency.UseVisualStyleBackColor = true;
            this.btnCurrency.Visible = false;
            this.btnCurrency.Click += new System.EventHandler(this.btnCurrency_Click);
            // 
            // btnToDepartment
            // 
            this.btnToDepartment.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToDepartment.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnToDepartment.Location = new System.Drawing.Point(629, 46);
            this.btnToDepartment.Name = "btnToDepartment";
            this.btnToDepartment.Size = new System.Drawing.Size(24, 24);
            this.btnToDepartment.TabIndex = 11;
            this.btnToDepartment.UseVisualStyleBackColor = true;
            this.btnToDepartment.Click += new System.EventHandler(this.btnToDepartment_Click);
            // 
            // lblCurrency
            // 
            this.lblCurrency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrency.Location = new System.Drawing.Point(21, 81);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(67, 16);
            this.lblCurrency.TabIndex = 0;
            this.lblCurrency.Text = "通貨コード";
            this.lblCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrency.Visible = false;
            // 
            // lblToDepartmentName
            // 
            this.lblToDepartmentName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblToDepartmentName.DropDown.AllowDrop = false;
            this.lblToDepartmentName.Enabled = false;
            this.lblToDepartmentName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDepartmentName.HighlightText = true;
            this.lblToDepartmentName.Location = new System.Drawing.Point(659, 48);
            this.lblToDepartmentName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblToDepartmentName.Name = "lblToDepartmentName";
            this.lblToDepartmentName.ReadOnly = true;
            this.lblToDepartmentName.Required = false;
            this.lblToDepartmentName.Size = new System.Drawing.Size(290, 22);
            this.lblToDepartmentName.TabIndex = 0;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCurrencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrencyCode.Format = "A";
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCurrencyCode.Location = new System.Drawing.Point(94, 78);
            this.txtCurrencyCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 6;
            this.txtCurrencyCode.Visible = false;
            this.txtCurrencyCode.Validated += new System.EventHandler(this.txtCurrencyCode_Validated);
            // 
            // lblStaff
            // 
            this.lblStaff.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaff.Location = new System.Drawing.Point(421, 80);
            this.lblStaff.Name = "lblStaff";
            this.lblStaff.Size = new System.Drawing.Size(81, 16);
            this.lblStaff.TabIndex = 0;
            this.lblStaff.Text = "担当者コード";
            this.lblStaff.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxMemo
            // 
            this.cbxMemo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cbxMemo.Location = new System.Drawing.Point(21, 50);
            this.cbxMemo.Name = "cbxMemo";
            this.cbxMemo.Size = new System.Drawing.Size(67, 18);
            this.cbxMemo.TabIndex = 4;
            this.cbxMemo.Text = "メモ有り";
            this.cbxMemo.UseVisualStyleBackColor = true;
            // 
            // txtFromStaffCode
            // 
            this.txtFromStaffCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtFromStaffCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFromStaffCode.DropDown.AllowDrop = false;
            this.txtFromStaffCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromStaffCode.HighlightText = true;
            this.txtFromStaffCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtFromStaffCode.Location = new System.Drawing.Point(508, 78);
            this.txtFromStaffCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFromStaffCode.Name = "txtFromStaffCode";
            this.txtFromStaffCode.Required = false;
            this.txtFromStaffCode.Size = new System.Drawing.Size(115, 22);
            this.txtFromStaffCode.TabIndex = 12;
            this.txtFromStaffCode.Validated += new System.EventHandler(this.txtFromStaffCode_Validated);
            // 
            // lblToStaffName
            // 
            this.lblToStaffName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblToStaffName.DropDown.AllowDrop = false;
            this.lblToStaffName.Enabled = false;
            this.lblToStaffName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToStaffName.HighlightText = true;
            this.lblToStaffName.Location = new System.Drawing.Point(659, 108);
            this.lblToStaffName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblToStaffName.Name = "lblToStaffName";
            this.lblToStaffName.ReadOnly = true;
            this.lblToStaffName.Required = false;
            this.lblToStaffName.Size = new System.Drawing.Size(290, 22);
            this.lblToStaffName.TabIndex = 0;
            // 
            // btnFromStaff
            // 
            this.btnFromStaff.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFromStaff.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnFromStaff.Location = new System.Drawing.Point(629, 76);
            this.btnFromStaff.Name = "btnFromStaff";
            this.btnFromStaff.Size = new System.Drawing.Size(24, 24);
            this.btnFromStaff.TabIndex = 13;
            this.btnFromStaff.UseVisualStyleBackColor = true;
            this.btnFromStaff.Click += new System.EventHandler(this.btnFromStaff_Click);
            // 
            // btnToStaff
            // 
            this.btnToStaff.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToStaff.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnToStaff.Location = new System.Drawing.Point(629, 106);
            this.btnToStaff.Name = "btnToStaff";
            this.btnToStaff.Size = new System.Drawing.Size(24, 24);
            this.btnToStaff.TabIndex = 15;
            this.btnToStaff.UseVisualStyleBackColor = true;
            this.btnToStaff.Click += new System.EventHandler(this.btnToStaff_Click);
            // 
            // lblFromStaffName
            // 
            this.lblFromStaffName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFromStaffName.DropDown.AllowDrop = false;
            this.lblFromStaffName.Enabled = false;
            this.lblFromStaffName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromStaffName.HighlightText = true;
            this.lblFromStaffName.Location = new System.Drawing.Point(659, 78);
            this.lblFromStaffName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblFromStaffName.Name = "lblFromStaffName";
            this.lblFromStaffName.ReadOnly = true;
            this.lblFromStaffName.Required = false;
            this.lblFromStaffName.Size = new System.Drawing.Size(290, 22);
            this.lblFromStaffName.TabIndex = 0;
            // 
            // txtToStaffCode
            // 
            this.txtToStaffCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtToStaffCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtToStaffCode.DropDown.AllowDrop = false;
            this.txtToStaffCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToStaffCode.HighlightText = true;
            this.txtToStaffCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtToStaffCode.Location = new System.Drawing.Point(508, 108);
            this.txtToStaffCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtToStaffCode.Name = "txtToStaffCode";
            this.txtToStaffCode.Required = false;
            this.txtToStaffCode.Size = new System.Drawing.Size(115, 22);
            this.txtToStaffCode.TabIndex = 14;
            this.txtToStaffCode.Validated += new System.EventHandler(this.txtToStaffCode_Validated);
            // 
            // lblStaffWave
            // 
            this.lblStaffWave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaffWave.Location = new System.Drawing.Point(465, 109);
            this.lblStaffWave.Margin = new System.Windows.Forms.Padding(3);
            this.lblStaffWave.Name = "lblStaffWave";
            this.lblStaffWave.Size = new System.Drawing.Size(20, 16);
            this.lblStaffWave.TabIndex = 0;
            this.lblStaffWave.Text = "～";
            this.lblStaffWave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbxStaff
            // 
            this.cbxStaff.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxStaff.Location = new System.Drawing.Point(486, 110);
            this.cbxStaff.Name = "cbxStaff";
            this.cbxStaff.Size = new System.Drawing.Size(16, 18);
            this.cbxStaff.TabIndex = 0;
            this.cbxStaff.TabStop = false;
            this.cbxStaff.UseVisualStyleBackColor = true;
            // 
            // tabArrearagesListResult
            // 
            this.tabArrearagesListResult.Controls.Add(this.lblTotalAmount);
            this.tabArrearagesListResult.Controls.Add(this.lblRecoveryTotalAmount);
            this.tabArrearagesListResult.Controls.Add(this.grdArrearagesList);
            this.tabArrearagesListResult.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabArrearagesListResult.Location = new System.Drawing.Point(4, 24);
            this.tabArrearagesListResult.Name = "tabArrearagesListResult";
            this.tabArrearagesListResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabArrearagesListResult.Size = new System.Drawing.Size(970, 563);
            this.tabArrearagesListResult.TabIndex = 1;
            this.tabArrearagesListResult.Text = "検索結果";
            this.tabArrearagesListResult.UseVisualStyleBackColor = true;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(740, 537);
            this.lblTotalAmount.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(103, 16);
            this.lblTotalAmount.TabIndex = 0;
            this.lblTotalAmount.Text = "回収予定金額合計";
            this.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecoveryTotalAmount
            // 
            this.lblRecoveryTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecoveryTotalAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRecoveryTotalAmount.DropDown.AllowDrop = false;
            this.lblRecoveryTotalAmount.Enabled = false;
            this.lblRecoveryTotalAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecoveryTotalAmount.HighlightText = true;
            this.lblRecoveryTotalAmount.Location = new System.Drawing.Point(844, 535);
            this.lblRecoveryTotalAmount.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.lblRecoveryTotalAmount.Name = "lblRecoveryTotalAmount";
            this.lblRecoveryTotalAmount.ReadOnly = true;
            this.lblRecoveryTotalAmount.Required = false;
            this.lblRecoveryTotalAmount.Size = new System.Drawing.Size(120, 22);
            this.lblRecoveryTotalAmount.TabIndex = 0;
            // 
            // grdArrearagesList
            // 
            this.grdArrearagesList.AllowAutoExtend = true;
            this.grdArrearagesList.AllowUserToAddRows = false;
            this.grdArrearagesList.AllowUserToShiftSelect = true;
            this.grdArrearagesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdArrearagesList.HideSelection = true;
            this.grdArrearagesList.Location = new System.Drawing.Point(6, 6);
            this.grdArrearagesList.Name = "grdArrearagesList";
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Copy)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Paste)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Cut)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToNextCell)), System.Windows.Forms.Keys.Tab));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToPreviousCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Tab)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveUp)), System.Windows.Forms.Keys.Up));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveDown)), System.Windows.Forms.Keys.Down));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveLeft)), System.Windows.Forms.Keys.Left));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveRight)), System.Windows.Forms.Keys.Right));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), System.Windows.Forms.Keys.Home));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), System.Windows.Forms.Keys.End));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftUp)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftDown)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftLeft)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftRight)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.DefaultModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(new Rac.VOne.Client.Common.MultiRow.Action.CheckSelectedCells(), System.Windows.Forms.Keys.Space));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Copy)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Paste)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Cut)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToNextCell)), System.Windows.Forms.Keys.Tab));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToPreviousCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Tab)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveUp)), System.Windows.Forms.Keys.Up));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveDown)), System.Windows.Forms.Keys.Down));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveLeft)), System.Windows.Forms.Keys.Left));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveRight)), System.Windows.Forms.Keys.Right));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), System.Windows.Forms.Keys.Home));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), System.Windows.Forms.Keys.End));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftUp)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftDown)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftLeft)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftRight)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.DisplayModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(new Rac.VOne.Client.Common.MultiRow.Action.CheckSelectedCells(), System.Windows.Forms.Keys.Space));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Copy)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Paste)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Cut)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToNextCell)), System.Windows.Forms.Keys.Tab));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToPreviousCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Tab)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveUp)), System.Windows.Forms.Keys.Up));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveDown)), System.Windows.Forms.Keys.Down));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveLeft)), System.Windows.Forms.Keys.Left));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveRight)), System.Windows.Forms.Keys.Right));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), System.Windows.Forms.Keys.Home));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), System.Windows.Forms.Keys.End));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftUp)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftDown)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftLeft)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftRight)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.ListBoxModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(new Rac.VOne.Client.Common.MultiRow.Action.CheckSelectedCells(), System.Windows.Forms.Keys.Space));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Copy)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Paste)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.EditingActions.Cut)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToNextCell)), System.Windows.Forms.Keys.Tab));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToPreviousCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Tab)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveUp)), System.Windows.Forms.Keys.Up));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveDown)), System.Windows.Forms.Keys.Down));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveLeft)), System.Windows.Forms.Keys.Left));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveRight)), System.Windows.Forms.Keys.Right));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), System.Windows.Forms.Keys.Home));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), System.Windows.Forms.Keys.End));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToFirstCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.MoveToLastCell)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftUp)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftDown)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftLeft)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftRight)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Up)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Down)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Left)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCellInRow)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Right)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToFirstCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.Home)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(((GrapeCity.Win.MultiRow.Action)(GrapeCity.Win.MultiRow.SelectionActions.ShiftToLastCell)), ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.End)))));
            shortcutKeyManager2.RowModeList.Add(new GrapeCity.Win.MultiRow.ShortcutKey(new Rac.VOne.Client.Common.MultiRow.Action.CheckSelectedCells(), System.Windows.Forms.Keys.Space));
            this.grdArrearagesList.ShortcutKeyManager = shortcutKeyManager2;
            this.grdArrearagesList.Size = new System.Drawing.Size(958, 523);
            this.grdArrearagesList.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdArrearagesList.TabIndex = 0;
            this.grdArrearagesList.Text = "vOneGridControl1";
            this.grdArrearagesList.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdArrearagesList_CellDoubleClick);
            // 
            // PF0401
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tbcArrearagesList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PF0401";
            this.Load += new System.EventHandler(this.PF0401_Load);
            this.tbcArrearagesList.ResumeLayout(false);
            this.tabArrearagesListSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFromDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datPaymentDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDapartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblToStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFromStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToStaffCode)).EndInit();
            this.tabArrearagesListResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblRecoveryTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdArrearagesList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcArrearagesList;
        private System.Windows.Forms.TabPage tabArrearagesListSearch;
        private System.Windows.Forms.TabPage tabArrearagesListResult;
        private Common.Controls.VOneGridControl grdArrearagesList;
        private Common.Controls.VOneLabelControl lblPaymentDate;
        private Common.Controls.VOneLabelControl lblCurrency;
        private Common.Controls.VOneDateControl datPaymentDate;
        private System.Windows.Forms.CheckBox cbxCustomer;
        private System.Windows.Forms.CheckBox cbxMemo;
        private Common.Controls.VOneTextControl txtMemo;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrency;
        private Common.Controls.VOneLabelControl lblFromDepartment;
        private Common.Controls.VOneTextControl txtFromDepartmentCode;
        private System.Windows.Forms.Button btnFromDepartment;
        private Common.Controls.VOneDispLabelControl lblFromDepartmentName;
        private Common.Controls.VOneLabelControl lblDepartmentWave;
        private System.Windows.Forms.CheckBox cbxDepartment;
        private Common.Controls.VOneTextControl txtToDapartmentCode;
        private System.Windows.Forms.Button btnToDepartment;
        private Common.Controls.VOneDispLabelControl lblToDepartmentName;
        private Common.Controls.VOneLabelControl lblStaff;
        private Common.Controls.VOneTextControl txtFromStaffCode;
        private System.Windows.Forms.Button btnFromStaff;
        private Common.Controls.VOneDispLabelControl lblFromStaffName;
        private Common.Controls.VOneLabelControl lblStaffWave;
        private System.Windows.Forms.CheckBox cbxStaff;
        private Common.Controls.VOneTextControl txtToStaffCode;
        private System.Windows.Forms.Button btnToStaff;
        private Common.Controls.VOneDispLabelControl lblToStaffName;
        private Common.Controls.VOneLabelControl lblTotalAmount;
        private Common.Controls.VOneDispLabelControl lblRecoveryTotalAmount;
    }
}
