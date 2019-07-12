namespace Rac.VOne.Client.Screen
{
    partial class PE0401
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
            this.tbcJournalizingDetail = new System.Windows.Forms.TabControl();
            this.tbpSearchCondition = new System.Windows.Forms.TabPage();
            this.lblCustomercode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblJournalizingType = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datFromCreateAt = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCurrencyCode = new System.Windows.Forms.Button();
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cbxAdvanceReceivedOccured = new System.Windows.Forms.CheckBox();
            this.lblUpdateDate = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxReceiptExclude = new System.Windows.Forms.CheckBox();
            this.lblCreateAtWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxMatching = new System.Windows.Forms.CheckBox();
            this.lblCustomername = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.datToCreateAt = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.btnCustomer = new System.Windows.Forms.Button();
            this.txtCustomer = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.tbpSearchResult = new System.Windows.Forms.TabPage();
            this.lbladdCustomerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.txtaddCustomerName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCustomerNameSearch = new System.Windows.Forms.Button();
            this.lblPayerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnPayerName = new System.Windows.Forms.Button();
            this.txtPayerName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCustomerCodeSearch = new System.Windows.Forms.Button();
            this.lbladdCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnAddCustomerCode = new System.Windows.Forms.Button();
            this.txtaddCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cbxAdvanceReceivedTransfer = new System.Windows.Forms.CheckBox();
            this.tbcJournalizingDetail.SuspendLayout();
            this.tbpSearchCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datFromCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datToCreateAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            this.tbpSearchResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtaddCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtaddCustomerCode)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcJournalizingDetail
            // 
            this.tbcJournalizingDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcJournalizingDetail.Controls.Add(this.tbpSearchCondition);
            this.tbcJournalizingDetail.Controls.Add(this.tbpSearchResult);
            this.tbcJournalizingDetail.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbcJournalizingDetail.Location = new System.Drawing.Point(15, 15);
            this.tbcJournalizingDetail.Name = "tbcJournalizingDetail";
            this.tbcJournalizingDetail.SelectedIndex = 0;
            this.tbcJournalizingDetail.Size = new System.Drawing.Size(978, 591);
            this.tbcJournalizingDetail.TabIndex = 1;
            // 
            // tbpSearchCondition
            // 
            this.tbpSearchCondition.Controls.Add(this.lblCustomercode);
            this.tbpSearchCondition.Controls.Add(this.lblJournalizingType);
            this.tbpSearchCondition.Controls.Add(this.datFromCreateAt);
            this.tbpSearchCondition.Controls.Add(this.lblCurrencyCode);
            this.tbpSearchCondition.Controls.Add(this.btnCurrencyCode);
            this.tbpSearchCondition.Controls.Add(this.txtCurrencyCode);
            this.tbpSearchCondition.Controls.Add(this.cbxAdvanceReceivedTransfer);
            this.tbpSearchCondition.Controls.Add(this.cbxAdvanceReceivedOccured);
            this.tbpSearchCondition.Controls.Add(this.lblUpdateDate);
            this.tbpSearchCondition.Controls.Add(this.cbxReceiptExclude);
            this.tbpSearchCondition.Controls.Add(this.lblCreateAtWave);
            this.tbpSearchCondition.Controls.Add(this.cbxMatching);
            this.tbpSearchCondition.Controls.Add(this.lblCustomername);
            this.tbpSearchCondition.Controls.Add(this.datToCreateAt);
            this.tbpSearchCondition.Controls.Add(this.btnCustomer);
            this.tbpSearchCondition.Controls.Add(this.txtCustomer);
            this.tbpSearchCondition.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpSearchCondition.Location = new System.Drawing.Point(4, 24);
            this.tbpSearchCondition.Name = "tbpSearchCondition";
            this.tbpSearchCondition.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSearchCondition.Size = new System.Drawing.Size(970, 563);
            this.tbpSearchCondition.TabIndex = 0;
            this.tbpSearchCondition.Text = "検索条件";
            this.tbpSearchCondition.UseVisualStyleBackColor = true;
            // 
            // lblCustomercode
            // 
            this.lblCustomercode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomercode.Location = new System.Drawing.Point(21, 57);
            this.lblCustomercode.Name = "lblCustomercode";
            this.lblCustomercode.Size = new System.Drawing.Size(69, 16);
            this.lblCustomercode.TabIndex = 0;
            this.lblCustomercode.Text = "得意先コード";
            this.lblCustomercode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblJournalizingType
            // 
            this.lblJournalizingType.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJournalizingType.Location = new System.Drawing.Point(21, 91);
            this.lblJournalizingType.Name = "lblJournalizingType";
            this.lblJournalizingType.Size = new System.Drawing.Size(69, 16);
            this.lblJournalizingType.TabIndex = 0;
            this.lblJournalizingType.Text = "仕訳区分";
            this.lblJournalizingType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datFromCreateAt
            // 
            this.datFromCreateAt.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datFromCreateAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datFromCreateAt.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datFromCreateAt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datFromCreateAt.Location = new System.Drawing.Point(96, 21);
            this.datFromCreateAt.Margin = new System.Windows.Forms.Padding(3, 18, 3, 6);
            this.datFromCreateAt.Name = "datFromCreateAt";
            this.datFromCreateAt.Required = true;
            this.datFromCreateAt.Size = new System.Drawing.Size(115, 22);
            this.datFromCreateAt.Spin.AllowSpin = false;
            this.datFromCreateAt.TabIndex = 0;
            this.datFromCreateAt.Value = null;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyCode.Location = new System.Drawing.Point(21, 126);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(69, 16);
            this.lblCurrencyCode.TabIndex = 0;
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrencyCode.Visible = false;
            // 
            // btnCurrencyCode
            // 
            this.btnCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCurrencyCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrencyCode.Location = new System.Drawing.Point(142, 122);
            this.btnCurrencyCode.Name = "btnCurrencyCode";
            this.btnCurrencyCode.Size = new System.Drawing.Size(24, 24);
            this.btnCurrencyCode.TabIndex = 10;
            this.btnCurrencyCode.UseVisualStyleBackColor = true;
            this.btnCurrencyCode.Visible = false;
            this.btnCurrencyCode.Click += new System.EventHandler(this.btnCurrencyCode_Click);
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
            this.txtCurrencyCode.Location = new System.Drawing.Point(96, 123);
            this.txtCurrencyCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 9;
            this.txtCurrencyCode.Visible = false;
            this.txtCurrencyCode.Validated += new System.EventHandler(this.txtCurrencyCode_Validated);
            // 
            // cbxAdvanceReceivedOccured
            // 
            this.cbxAdvanceReceivedOccured.Checked = true;
            this.cbxAdvanceReceivedOccured.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxAdvanceReceivedOccured.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxAdvanceReceivedOccured.Location = new System.Drawing.Point(308, 91);
            this.cbxAdvanceReceivedOccured.Margin = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.cbxAdvanceReceivedOccured.Name = "cbxAdvanceReceivedOccured";
            this.cbxAdvanceReceivedOccured.Size = new System.Drawing.Size(101, 18);
            this.cbxAdvanceReceivedOccured.TabIndex = 7;
            this.cbxAdvanceReceivedOccured.Text = "前受計上仕訳";
            this.cbxAdvanceReceivedOccured.UseVisualStyleBackColor = true;
            // 
            // lblUpdateDate
            // 
            this.lblUpdateDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateDate.Location = new System.Drawing.Point(21, 23);
            this.lblUpdateDate.Name = "lblUpdateDate";
            this.lblUpdateDate.Size = new System.Drawing.Size(69, 16);
            this.lblUpdateDate.TabIndex = 0;
            this.lblUpdateDate.Text = "更新日付";
            this.lblUpdateDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxReceiptExclude
            // 
            this.cbxReceiptExclude.Checked = true;
            this.cbxReceiptExclude.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxReceiptExclude.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxReceiptExclude.Location = new System.Drawing.Point(198, 91);
            this.cbxReceiptExclude.Margin = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.cbxReceiptExclude.Name = "cbxReceiptExclude";
            this.cbxReceiptExclude.Size = new System.Drawing.Size(86, 18);
            this.cbxReceiptExclude.TabIndex = 6;
            this.cbxReceiptExclude.Text = "対象外仕訳";
            this.cbxReceiptExclude.UseVisualStyleBackColor = true;
            // 
            // lblCreateAtWave
            // 
            this.lblCreateAtWave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreateAtWave.Location = new System.Drawing.Point(219, 24);
            this.lblCreateAtWave.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblCreateAtWave.Name = "lblCreateAtWave";
            this.lblCreateAtWave.Size = new System.Drawing.Size(20, 16);
            this.lblCreateAtWave.TabIndex = 0;
            this.lblCreateAtWave.Text = "～";
            this.lblCreateAtWave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbxMatching
            // 
            this.cbxMatching.Checked = true;
            this.cbxMatching.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxMatching.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxMatching.Location = new System.Drawing.Point(96, 91);
            this.cbxMatching.Margin = new System.Windows.Forms.Padding(3, 8, 12, 8);
            this.cbxMatching.Name = "cbxMatching";
            this.cbxMatching.Size = new System.Drawing.Size(78, 18);
            this.cbxMatching.TabIndex = 5;
            this.cbxMatching.Text = " 消込仕訳";
            this.cbxMatching.UseVisualStyleBackColor = true;
            // 
            // lblCustomername
            // 
            this.lblCustomername.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomername.DropDown.AllowDrop = false;
            this.lblCustomername.Enabled = false;
            this.lblCustomername.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomername.HighlightText = true;
            this.lblCustomername.Location = new System.Drawing.Point(247, 55);
            this.lblCustomername.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCustomername.Name = "lblCustomername";
            this.lblCustomername.ReadOnly = true;
            this.lblCustomername.Required = false;
            this.lblCustomername.Size = new System.Drawing.Size(290, 22);
            this.lblCustomername.TabIndex = 4;
            // 
            // datToCreateAt
            // 
            this.datToCreateAt.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datToCreateAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datToCreateAt.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datToCreateAt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datToCreateAt.Location = new System.Drawing.Point(247, 21);
            this.datToCreateAt.Margin = new System.Windows.Forms.Padding(3, 18, 3, 6);
            this.datToCreateAt.Name = "datToCreateAt";
            this.datToCreateAt.Required = true;
            this.datToCreateAt.Size = new System.Drawing.Size(115, 22);
            this.datToCreateAt.Spin.AllowSpin = false;
            this.datToCreateAt.TabIndex = 1;
            this.datToCreateAt.Value = null;
            // 
            // btnCustomer
            // 
            this.btnCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomer.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomer.Location = new System.Drawing.Point(217, 53);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Size = new System.Drawing.Size(24, 24);
            this.btnCustomer.TabIndex = 3;
            this.btnCustomer.UseVisualStyleBackColor = true;
            this.btnCustomer.Click += new System.EventHandler(this.btnCustomer_Click);
            // 
            // txtCustomer
            // 
            this.txtCustomer.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomer.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomer.DropDown.AllowDrop = false;
            this.txtCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.HighlightText = true;
            this.txtCustomer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtCustomer.Location = new System.Drawing.Point(96, 55);
            this.txtCustomer.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Required = false;
            this.txtCustomer.Size = new System.Drawing.Size(115, 22);
            this.txtCustomer.TabIndex = 2;
            this.txtCustomer.Validated += new System.EventHandler(this.txtCustomer_Validated);
            // 
            // tbpSearchResult
            // 
            this.tbpSearchResult.Controls.Add(this.lbladdCustomerName);
            this.tbpSearchResult.Controls.Add(this.grid);
            this.tbpSearchResult.Controls.Add(this.txtaddCustomerName);
            this.tbpSearchResult.Controls.Add(this.btnCustomerNameSearch);
            this.tbpSearchResult.Controls.Add(this.lblPayerName);
            this.tbpSearchResult.Controls.Add(this.btnPayerName);
            this.tbpSearchResult.Controls.Add(this.txtPayerName);
            this.tbpSearchResult.Controls.Add(this.btnCustomerCodeSearch);
            this.tbpSearchResult.Controls.Add(this.lbladdCustomerCode);
            this.tbpSearchResult.Controls.Add(this.btnAddCustomerCode);
            this.tbpSearchResult.Controls.Add(this.txtaddCustomerCode);
            this.tbpSearchResult.Location = new System.Drawing.Point(4, 24);
            this.tbpSearchResult.Name = "tbpSearchResult";
            this.tbpSearchResult.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSearchResult.Size = new System.Drawing.Size(970, 563);
            this.tbpSearchResult.TabIndex = 1;
            this.tbpSearchResult.Text = "検索結果";
            this.tbpSearchResult.UseVisualStyleBackColor = true;
            // 
            // lbladdCustomerName
            // 
            this.lbladdCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbladdCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbladdCustomerName.Location = new System.Drawing.Point(15, 494);
            this.lbladdCustomerName.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.lbladdCustomerName.Name = "lbladdCustomerName";
            this.lbladdCustomerName.Size = new System.Drawing.Size(69, 16);
            this.lbladdCustomerName.TabIndex = 0;
            this.lbladdCustomerName.Text = "得意先名";
            this.lbladdCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.HorizontalScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(6, 6);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(958, 477);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 0;
            this.grid.Text = "";
            this.grid.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grid_CellValueChanged);
            this.grid.CellEditedFormattedValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs>(this.grid_CellEditedFormattedValueChanged);
            // 
            // txtaddCustomerName
            // 
            this.txtaddCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtaddCustomerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtaddCustomerName.DropDown.AllowDrop = false;
            this.txtaddCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtaddCustomerName.HighlightText = true;
            this.txtaddCustomerName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtaddCustomerName.Location = new System.Drawing.Point(90, 492);
            this.txtaddCustomerName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.txtaddCustomerName.MaxLength = 140;
            this.txtaddCustomerName.Name = "txtaddCustomerName";
            this.txtaddCustomerName.Required = false;
            this.txtaddCustomerName.Size = new System.Drawing.Size(250, 22);
            this.txtaddCustomerName.TabIndex = 2;
            // 
            // btnCustomerNameSearch
            // 
            this.btnCustomerNameSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCustomerNameSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerNameSearch.Location = new System.Drawing.Point(346, 491);
            this.btnCustomerNameSearch.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.btnCustomerNameSearch.Name = "btnCustomerNameSearch";
            this.btnCustomerNameSearch.Size = new System.Drawing.Size(51, 24);
            this.btnCustomerNameSearch.TabIndex = 3;
            this.btnCustomerNameSearch.Text = "検索";
            this.btnCustomerNameSearch.UseVisualStyleBackColor = true;
            this.btnCustomerNameSearch.Click += new System.EventHandler(this.btnCustomerNameSearch_Click);
            // 
            // lblPayerName
            // 
            this.lblPayerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayerName.Location = new System.Drawing.Point(415, 495);
            this.lblPayerName.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Size = new System.Drawing.Size(79, 16);
            this.lblPayerName.TabIndex = 0;
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPayerName
            // 
            this.btnPayerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayerName.Location = new System.Drawing.Point(756, 491);
            this.btnPayerName.Name = "btnPayerName";
            this.btnPayerName.Size = new System.Drawing.Size(51, 24);
            this.btnPayerName.TabIndex = 5;
            this.btnPayerName.Text = "検索";
            this.btnPayerName.UseVisualStyleBackColor = true;
            this.btnPayerName.Click += new System.EventHandler(this.btnPayerName_Click);
            // 
            // txtPayerName
            // 
            this.txtPayerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPayerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPayerName.DropDown.AllowDrop = false;
            this.txtPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayerName.Format = "9AK@";
            this.txtPayerName.HighlightText = true;
            this.txtPayerName.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtPayerName.Location = new System.Drawing.Point(500, 492);
            this.txtPayerName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.txtPayerName.MaxLength = 140;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Required = false;
            this.txtPayerName.Size = new System.Drawing.Size(250, 22);
            this.txtPayerName.TabIndex = 4;
            // 
            // btnCustomerCodeSearch
            // 
            this.btnCustomerCodeSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCustomerCodeSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerCodeSearch.Location = new System.Drawing.Point(241, 525);
            this.btnCustomerCodeSearch.Name = "btnCustomerCodeSearch";
            this.btnCustomerCodeSearch.Size = new System.Drawing.Size(51, 24);
            this.btnCustomerCodeSearch.TabIndex = 8;
            this.btnCustomerCodeSearch.Text = "検索";
            this.btnCustomerCodeSearch.UseVisualStyleBackColor = true;
            this.btnCustomerCodeSearch.Click += new System.EventHandler(this.btnCustomerCodeSearch_Click);
            // 
            // lbladdCustomerCode
            // 
            this.lbladdCustomerCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbladdCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbladdCustomerCode.Location = new System.Drawing.Point(15, 528);
            this.lbladdCustomerCode.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.lbladdCustomerCode.Name = "lbladdCustomerCode";
            this.lbladdCustomerCode.Size = new System.Drawing.Size(69, 16);
            this.lbladdCustomerCode.TabIndex = 0;
            this.lbladdCustomerCode.Text = "得意先コード";
            this.lbladdCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddCustomerCode
            // 
            this.btnAddCustomerCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCustomerCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnAddCustomerCode.Location = new System.Drawing.Point(211, 525);
            this.btnAddCustomerCode.Name = "btnAddCustomerCode";
            this.btnAddCustomerCode.Size = new System.Drawing.Size(24, 24);
            this.btnAddCustomerCode.TabIndex = 7;
            this.btnAddCustomerCode.UseVisualStyleBackColor = true;
            this.btnAddCustomerCode.Click += new System.EventHandler(this.btnAddCustomerCode_Click);
            // 
            // txtaddCustomerCode
            // 
            this.txtaddCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtaddCustomerCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtaddCustomerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtaddCustomerCode.DropDown.AllowDrop = false;
            this.txtaddCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtaddCustomerCode.HighlightText = true;
            this.txtaddCustomerCode.Location = new System.Drawing.Point(90, 526);
            this.txtaddCustomerCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 12);
            this.txtaddCustomerCode.Name = "txtaddCustomerCode";
            this.txtaddCustomerCode.Required = false;
            this.txtaddCustomerCode.Size = new System.Drawing.Size(115, 22);
            this.txtaddCustomerCode.TabIndex = 6;
            this.txtaddCustomerCode.Validated += new System.EventHandler(this.txtaddCustomerCode_Validated);
            // 
            // cbxAdvanceReceivedTransfer
            // 
            this.cbxAdvanceReceivedTransfer.Checked = true;
            this.cbxAdvanceReceivedTransfer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxAdvanceReceivedTransfer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxAdvanceReceivedTransfer.Location = new System.Drawing.Point(433, 91);
            this.cbxAdvanceReceivedTransfer.Margin = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.cbxAdvanceReceivedTransfer.Name = "cbxAdvanceReceivedTransfer";
            this.cbxAdvanceReceivedTransfer.Size = new System.Drawing.Size(104, 18);
            this.cbxAdvanceReceivedTransfer.TabIndex = 8;
            this.cbxAdvanceReceivedTransfer.Text = "前受振替仕訳";
            this.cbxAdvanceReceivedTransfer.UseVisualStyleBackColor = true;
            // 
            // PE0401
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tbcJournalizingDetail);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PE0401";
            this.Load += new System.EventHandler(this.PE0401_Load);
            this.tbcJournalizingDetail.ResumeLayout(false);
            this.tbpSearchCondition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datFromCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datToCreateAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            this.tbpSearchResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtaddCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtaddCustomerCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tbcJournalizingDetail;
        private System.Windows.Forms.TabPage tbpSearchCondition;
        private Common.Controls.VOneLabelControl lblCustomercode;
        private Common.Controls.VOneLabelControl lblUpdateDate;
        private Common.Controls.VOneLabelControl lblCreateAtWave;
        private Common.Controls.VOneDateControl datFromCreateAt;
        private Common.Controls.VOneDateControl datToCreateAt;
        private Common.Controls.VOneTextControl txtCustomer;
        private System.Windows.Forms.Button btnCustomer;
        private Common.Controls.VOneDispLabelControl lblCustomername;
        private System.Windows.Forms.TabPage tbpSearchResult;
        private Common.Controls.VOneGridControl grid;
        private Common.Controls.VOneLabelControl lbladdCustomerName;
        private Common.Controls.VOneTextControl txtaddCustomerName;
        private System.Windows.Forms.Button btnCustomerNameSearch;
        private Common.Controls.VOneLabelControl lblPayerName;
        private Common.Controls.VOneTextControl txtPayerName;
        private Common.Controls.VOneLabelControl lblJournalizingType;
        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrencyCode;
        private System.Windows.Forms.CheckBox cbxMatching;
        private System.Windows.Forms.CheckBox cbxReceiptExclude;
        private System.Windows.Forms.CheckBox cbxAdvanceReceivedOccured;
        private System.Windows.Forms.Button btnPayerName;
        private Common.Controls.VOneLabelControl lbladdCustomerCode;
        private Common.Controls.VOneTextControl txtaddCustomerCode;
        private System.Windows.Forms.Button btnAddCustomerCode;
        private System.Windows.Forms.Button btnCustomerCodeSearch;
        private System.Windows.Forms.CheckBox cbxAdvanceReceivedTransfer;
    }
}
