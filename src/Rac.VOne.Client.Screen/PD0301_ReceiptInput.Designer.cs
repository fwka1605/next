namespace Rac.VOne.Client.Screen
{
    partial class PD0301
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
            this.grdReceiptInput = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblMatchingBillingRemain = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblTotal = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblMatchingDifferent = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblStatus = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxItemsInput = new System.Windows.Forms.GroupBox();
            this.lblPayerName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblReceipt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxSaveKanaHistory = new System.Windows.Forms.CheckBox();
            this.lblMatchingRecordedAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCurrencyCode = new System.Windows.Forms.Button();
            this.lblReceiptAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCustomer = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblSectionName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCurrency = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblSection = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblPayer = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblReceiptId = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnSectionCode = new System.Windows.Forms.Button();
            this.btnCustomerCode = new System.Windows.Forms.Button();
            this.txtSectionCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.datRecordedAt = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblTotalAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblMatchingBillingRemainAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblMatchingDifferentAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptInput)).BeginInit();
            this.gbxItemsInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMatchingBillingRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMatchingDifferentAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // grdReceiptInput
            // 
            this.grdReceiptInput.AllowAutoExtend = true;
            this.grdReceiptInput.AllowUserToAddRows = false;
            this.grdReceiptInput.AllowUserToShiftSelect = true;
            this.grdReceiptInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grdReceiptInput.HorizontalScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.grdReceiptInput.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdReceiptInput.Location = new System.Drawing.Point(24, 170);
            this.grdReceiptInput.Margin = new System.Windows.Forms.Padding(8);
            this.grdReceiptInput.Size = new System.Drawing.Size(960, 364);
            this.grdReceiptInput.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdReceiptInput.TabIndex = 9;
            this.grdReceiptInput.Text = "vOneGridControl1";
            this.grdReceiptInput.VerticalScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.grdReceiptInput.CellValidating += new System.EventHandler<GrapeCity.Win.MultiRow.CellValidatingEventArgs>(this.grdReceiptInput_CellValidating);
            this.grdReceiptInput.CellValidated += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptInput_CellValidated);
            this.grdReceiptInput.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptInput_CellValueChanged);
            this.grdReceiptInput.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptInput_CellEnter);
            this.grdReceiptInput.CellLeave += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptInput_CellLeave);
            this.grdReceiptInput.EditingControlShowing += new System.EventHandler<GrapeCity.Win.MultiRow.EditingControlShowingEventArgs>(this.grdReceiptInput_EditingControlShowing);
            this.grdReceiptInput.CellContentButtonClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptInput_CellContentButtonClick);
            // 
            // lblMatchingBillingRemain
            // 
            this.lblMatchingBillingRemain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMatchingBillingRemain.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchingBillingRemain.Location = new System.Drawing.Point(648, 547);
            this.lblMatchingBillingRemain.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblMatchingBillingRemain.Name = "lblMatchingBillingRemain";
            this.lblMatchingBillingRemain.Size = new System.Drawing.Size(43, 16);
            this.lblMatchingBillingRemain.TabIndex = 0;
            this.lblMatchingBillingRemain.Text = "請求残";
            this.lblMatchingBillingRemain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTotal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(813, 547);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(55, 16);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "合計金額";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMatchingDifferent
            // 
            this.lblMatchingDifferent.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMatchingDifferent.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchingDifferent.Location = new System.Drawing.Point(837, 582);
            this.lblMatchingDifferent.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblMatchingDifferent.Name = "lblMatchingDifferent";
            this.lblMatchingDifferent.Size = new System.Drawing.Size(31, 16);
            this.lblMatchingDifferent.TabIndex = 0;
            this.lblMatchingDifferent.Text = "差額";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblStatus.BackColor = System.Drawing.Color.Aqua;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStatus.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblStatus.Location = new System.Drawing.Point(24, 12);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(9, 0, 3, 3);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(30, 147);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "新規";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbxItemsInput
            // 
            this.gbxItemsInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbxItemsInput.Controls.Add(this.lblPayerName);
            this.gbxItemsInput.Controls.Add(this.lblReceipt);
            this.gbxItemsInput.Controls.Add(this.cbxSaveKanaHistory);
            this.gbxItemsInput.Controls.Add(this.lblMatchingRecordedAt);
            this.gbxItemsInput.Controls.Add(this.btnCurrencyCode);
            this.gbxItemsInput.Controls.Add(this.lblReceiptAt);
            this.gbxItemsInput.Controls.Add(this.lblCustomerName);
            this.gbxItemsInput.Controls.Add(this.txtCurrencyCode);
            this.gbxItemsInput.Controls.Add(this.lblCustomer);
            this.gbxItemsInput.Controls.Add(this.lblSectionName);
            this.gbxItemsInput.Controls.Add(this.lblCurrency);
            this.gbxItemsInput.Controls.Add(this.lblSection);
            this.gbxItemsInput.Controls.Add(this.lblPayer);
            this.gbxItemsInput.Controls.Add(this.lblReceiptId);
            this.gbxItemsInput.Controls.Add(this.txtCustomerCode);
            this.gbxItemsInput.Controls.Add(this.btnSectionCode);
            this.gbxItemsInput.Controls.Add(this.btnCustomerCode);
            this.gbxItemsInput.Controls.Add(this.txtSectionCode);
            this.gbxItemsInput.Controls.Add(this.datRecordedAt);
            this.gbxItemsInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxItemsInput.Location = new System.Drawing.Point(60, 3);
            this.gbxItemsInput.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.gbxItemsInput.Name = "gbxItemsInput";
            this.gbxItemsInput.Size = new System.Drawing.Size(924, 156);
            this.gbxItemsInput.TabIndex = 0;
            this.gbxItemsInput.TabStop = false;
            // 
            // lblPayerName
            // 
            this.lblPayerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPayerName.DropDown.AllowDrop = false;
            this.lblPayerName.Enabled = false;
            this.lblPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayerName.HighlightText = true;
            this.lblPayerName.Location = new System.Drawing.Point(492, 28);
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.ReadOnly = true;
            this.lblPayerName.Required = false;
            this.lblPayerName.Size = new System.Drawing.Size(250, 22);
            this.lblPayerName.TabIndex = 0;
            // 
            // lblReceipt
            // 
            this.lblReceipt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceipt.Location = new System.Drawing.Point(25, 31);
            this.lblReceipt.Name = "lblReceipt";
            this.lblReceipt.Size = new System.Drawing.Size(81, 16);
            this.lblReceipt.TabIndex = 0;
            this.lblReceipt.Text = "入金ID";
            this.lblReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxSaveKanaHistory
            // 
            this.cbxSaveKanaHistory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxSaveKanaHistory.Location = new System.Drawing.Point(616, 60);
            this.cbxSaveKanaHistory.Name = "cbxSaveKanaHistory";
            this.cbxSaveKanaHistory.Size = new System.Drawing.Size(126, 18);
            this.cbxSaveKanaHistory.TabIndex = 4;
            this.cbxSaveKanaHistory.Text = "学習履歴に登録する";
            this.cbxSaveKanaHistory.UseVisualStyleBackColor = true;
            // 
            // lblMatchingRecordedAt
            // 
            this.lblMatchingRecordedAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchingRecordedAt.Location = new System.Drawing.Point(233, 60);
            this.lblMatchingRecordedAt.Name = "lblMatchingRecordedAt";
            this.lblMatchingRecordedAt.Size = new System.Drawing.Size(168, 16);
            this.lblMatchingRecordedAt.TabIndex = 0;
            this.lblMatchingRecordedAt.Text = "2016/07/04";
            // 
            // btnCurrencyCode
            // 
            this.btnCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCurrencyCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrencyCode.Location = new System.Drawing.Point(538, 56);
            this.btnCurrencyCode.Name = "btnCurrencyCode";
            this.btnCurrencyCode.Size = new System.Drawing.Size(24, 24);
            this.btnCurrencyCode.TabIndex = 3;
            this.btnCurrencyCode.UseVisualStyleBackColor = true;
            this.btnCurrencyCode.Click += new System.EventHandler(this.btnCurrencyCode_Click);
            // 
            // lblReceiptAt
            // 
            this.lblReceiptAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptAt.Location = new System.Drawing.Point(25, 60);
            this.lblReceiptAt.Name = "lblReceiptAt";
            this.lblReceiptAt.Size = new System.Drawing.Size(81, 16);
            this.lblReceiptAt.TabIndex = 0;
            this.lblReceiptAt.Text = "入金日";
            this.lblReceiptAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerName.DropDown.AllowDrop = false;
            this.lblCustomerName.Enabled = false;
            this.lblCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.HighlightText = true;
            this.lblCustomerName.Location = new System.Drawing.Point(263, 86);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.ReadOnly = true;
            this.lblCustomerName.Required = false;
            this.lblCustomerName.Size = new System.Drawing.Size(223, 22);
            this.lblCustomerName.TabIndex = 0;
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
            this.txtCurrencyCode.Location = new System.Drawing.Point(492, 57);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 2;
            this.txtCurrencyCode.Validated += new System.EventHandler(this.txtCurrencyCode_Validated);
            // 
            // lblCustomer
            // 
            this.lblCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(25, 89);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(81, 16);
            this.lblCustomer.TabIndex = 0;
            this.lblCustomer.Text = "得意先コード";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSectionName
            // 
            this.lblSectionName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSectionName.DropDown.AllowDrop = false;
            this.lblSectionName.Enabled = false;
            this.lblSectionName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionName.HighlightText = true;
            this.lblSectionName.Location = new System.Drawing.Point(263, 115);
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.ReadOnly = true;
            this.lblSectionName.Required = false;
            this.lblSectionName.Size = new System.Drawing.Size(223, 22);
            this.lblSectionName.TabIndex = 0;
            // 
            // lblCurrency
            // 
            this.lblCurrency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrency.Location = new System.Drawing.Point(407, 60);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(79, 16);
            this.lblCurrency.TabIndex = 0;
            this.lblCurrency.Text = "通貨コード";
            this.lblCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSection
            // 
            this.lblSection.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSection.Location = new System.Drawing.Point(25, 117);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(81, 16);
            this.lblSection.TabIndex = 0;
            this.lblSection.Text = "入金部門コード";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPayer
            // 
            this.lblPayer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayer.Location = new System.Drawing.Point(407, 30);
            this.lblPayer.Name = "lblPayer";
            this.lblPayer.Size = new System.Drawing.Size(79, 16);
            this.lblPayer.TabIndex = 0;
            this.lblPayer.Text = "振込依頼人名";
            this.lblPayer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReceiptId
            // 
            this.lblReceiptId.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblReceiptId.DropDown.AllowDrop = false;
            this.lblReceiptId.Enabled = false;
            this.lblReceiptId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptId.HighlightText = true;
            this.lblReceiptId.Location = new System.Drawing.Point(112, 28);
            this.lblReceiptId.Name = "lblReceiptId";
            this.lblReceiptId.ReadOnly = true;
            this.lblReceiptId.Required = false;
            this.lblReceiptId.Size = new System.Drawing.Size(115, 22);
            this.lblReceiptId.TabIndex = 0;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerCode.DropDown.AllowDrop = false;
            this.txtCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerCode.HighlightText = true;
            this.txtCustomerCode.Location = new System.Drawing.Point(112, 86);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Required = true;
            this.txtCustomerCode.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCode.TabIndex = 5;
            this.txtCustomerCode.Validated += new System.EventHandler(this.txtCustomerCode_Validated);
            // 
            // btnSectionCode
            // 
            this.btnSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSectionCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSectionCode.Location = new System.Drawing.Point(233, 114);
            this.btnSectionCode.Name = "btnSectionCode";
            this.btnSectionCode.Size = new System.Drawing.Size(24, 24);
            this.btnSectionCode.TabIndex = 8;
            this.btnSectionCode.UseVisualStyleBackColor = true;
            this.btnSectionCode.Click += new System.EventHandler(this.btnSectionCode_Click);
            // 
            // btnCustomerCode
            // 
            this.btnCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomerCode.Location = new System.Drawing.Point(233, 84);
            this.btnCustomerCode.Name = "btnCustomerCode";
            this.btnCustomerCode.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerCode.TabIndex = 6;
            this.btnCustomerCode.UseVisualStyleBackColor = true;
            this.btnCustomerCode.Click += new System.EventHandler(this.btnCustomerCode_Click);
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtSectionCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSectionCode.DropDown.AllowDrop = false;
            this.txtSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSectionCode.HighlightText = true;
            this.txtSectionCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSectionCode.Location = new System.Drawing.Point(112, 115);
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Required = true;
            this.txtSectionCode.Size = new System.Drawing.Size(115, 22);
            this.txtSectionCode.TabIndex = 7;
            this.txtSectionCode.Validated += new System.EventHandler(this.txtSectionCode_Validated);
            // 
            // datRecordedAt
            // 
            this.datRecordedAt.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datRecordedAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datRecordedAt.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datRecordedAt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datRecordedAt.Location = new System.Drawing.Point(112, 57);
            this.datRecordedAt.Name = "datRecordedAt";
            this.datRecordedAt.Required = true;
            this.datRecordedAt.Size = new System.Drawing.Size(115, 22);
            this.datRecordedAt.Spin.AllowSpin = false;
            this.datRecordedAt.TabIndex = 1;
            this.datRecordedAt.Value = new System.DateTime(2016, 7, 4, 0, 0, 0, 0);
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTotalAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTotalAmount.DropDown.AllowDrop = false;
            this.lblTotalAmount.Enabled = false;
            this.lblTotalAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTotalAmount.HighlightText = true;
            this.lblTotalAmount.Location = new System.Drawing.Point(869, 545);
            this.lblTotalAmount.Margin = new System.Windows.Forms.Padding(1, 3, 3, 6);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.ReadOnly = true;
            this.lblTotalAmount.Required = false;
            this.lblTotalAmount.Size = new System.Drawing.Size(115, 22);
            this.lblTotalAmount.TabIndex = 23;
            this.lblTotalAmount.Text = "0";
            // 
            // lblMatchingBillingRemainAmount
            // 
            this.lblMatchingBillingRemainAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMatchingBillingRemainAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMatchingBillingRemainAmount.DropDown.AllowDrop = false;
            this.lblMatchingBillingRemainAmount.Enabled = false;
            this.lblMatchingBillingRemainAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMatchingBillingRemainAmount.HighlightText = true;
            this.lblMatchingBillingRemainAmount.Location = new System.Drawing.Point(692, 545);
            this.lblMatchingBillingRemainAmount.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.lblMatchingBillingRemainAmount.Name = "lblMatchingBillingRemainAmount";
            this.lblMatchingBillingRemainAmount.ReadOnly = true;
            this.lblMatchingBillingRemainAmount.Required = false;
            this.lblMatchingBillingRemainAmount.Size = new System.Drawing.Size(115, 22);
            this.lblMatchingBillingRemainAmount.TabIndex = 23;
            this.lblMatchingBillingRemainAmount.Text = "0";
            // 
            // lblMatchingDifferentAmount
            // 
            this.lblMatchingDifferentAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMatchingDifferentAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMatchingDifferentAmount.DropDown.AllowDrop = false;
            this.lblMatchingDifferentAmount.Enabled = false;
            this.lblMatchingDifferentAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMatchingDifferentAmount.HighlightText = true;
            this.lblMatchingDifferentAmount.Location = new System.Drawing.Point(869, 579);
            this.lblMatchingDifferentAmount.Margin = new System.Windows.Forms.Padding(1, 6, 3, 3);
            this.lblMatchingDifferentAmount.Name = "lblMatchingDifferentAmount";
            this.lblMatchingDifferentAmount.ReadOnly = true;
            this.lblMatchingDifferentAmount.Required = false;
            this.lblMatchingDifferentAmount.Size = new System.Drawing.Size(115, 22);
            this.lblMatchingDifferentAmount.TabIndex = 23;
            this.lblMatchingDifferentAmount.Text = "0";
            // 
            // PD0301
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.gbxItemsInput);
            this.Controls.Add(this.lblMatchingDifferent);
            this.Controls.Add(this.lblMatchingBillingRemainAmount);
            this.Controls.Add(this.lblMatchingDifferentAmount);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblMatchingBillingRemain);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.grdReceiptInput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PD0301";
            this.Load += new System.EventHandler(this.PD0301_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptInput)).EndInit();
            this.gbxItemsInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMatchingBillingRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMatchingDifferentAmount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneGridControl grdReceiptInput;
        private Common.Controls.VOneLabelControl lblMatchingBillingRemain;
        private Common.Controls.VOneLabelControl lblTotal;
        private Common.Controls.VOneLabelControl lblMatchingDifferent;
        private Common.Controls.VOneLabelControl lblStatus;
        private System.Windows.Forms.GroupBox gbxItemsInput;
        private Common.Controls.VOneLabelControl lblReceipt;
        private Common.Controls.VOneLabelControl lblReceiptAt;
        private Common.Controls.VOneLabelControl lblCustomer;
        private Common.Controls.VOneLabelControl lblSection;
        private Common.Controls.VOneTextControl txtCustomerCode;
        private Common.Controls.VOneTextControl txtSectionCode;
        private Common.Controls.VOneDateControl datRecordedAt;
        private System.Windows.Forms.Button btnCustomerCode;
        private System.Windows.Forms.Button btnSectionCode;
        private Common.Controls.VOneLabelControl lblPayer;
        private Common.Controls.VOneLabelControl lblCurrency;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrencyCode;
        private System.Windows.Forms.CheckBox cbxSaveKanaHistory;
        private Common.Controls.VOneLabelControl lblMatchingRecordedAt;
        private Common.Controls.VOneDispLabelControl lblCustomerName;
        private Common.Controls.VOneDispLabelControl lblSectionName;
        private Common.Controls.VOneDispLabelControl lblPayerName;
        private Common.Controls.VOneDispLabelControl lblReceiptId;
        private Common.Controls.VOneDispLabelControl lblTotalAmount;
        private Common.Controls.VOneDispLabelControl lblMatchingBillingRemainAmount;
        private Common.Controls.VOneDispLabelControl lblMatchingDifferentAmount;
    }
}
