namespace Rac.VOne.Client.Screen
{
    partial class PD0801
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
            this.tbcReceiptSectionTransfer = new System.Windows.Forms.TabControl();
            this.tbpSearchCondition = new System.Windows.Forms.TabPage();
            this.cbxMemo = new System.Windows.Forms.CheckBox();
            this.lblSectionName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtMemo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerNameTo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCurrencyCode = new System.Windows.Forms.Button();
            this.txtPayerName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtCustomerCodeTo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cbxNoAssignment = new System.Windows.Forms.CheckBox();
            this.lblCustomerCodeFrom = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblRecordedAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCustomerCodeFrom = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCustomerCodeFrom = new System.Windows.Forms.Button();
            this.cbxPartAssignment = new System.Windows.Forms.CheckBox();
            this.lblCustomerNameFrom = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblPayerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerCodeTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblAassignmentFlag = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxCustomerCodeTo = new System.Windows.Forms.CheckBox();
            this.datRecordedAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.btnCustomerCodeTo = new System.Windows.Forms.Button();
            this.lblCategoryName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblSectionCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCategoryCode = new System.Windows.Forms.Button();
            this.txtSectionCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCategoryCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnSectionCode = new System.Windows.Forms.Button();
            this.txtCategoryCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.datRecordedAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblRecordedAtTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.tbpSearchResult = new System.Windows.Forms.TabPage();
            this.lblNumber = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.grdReceiptData = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblReceiptAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblReceiptAmountTotal = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblRemainAmountTotal = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblRemainAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.tbcReceiptSectionTransfer.SuspendLayout();
            this.tbpSearchCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtTo)).BeginInit();
            this.tbpSearchResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmountTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmountTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcReceiptSectionTransfer
            // 
            this.tbcReceiptSectionTransfer.Controls.Add(this.tbpSearchCondition);
            this.tbcReceiptSectionTransfer.Controls.Add(this.tbpSearchResult);
            this.tbcReceiptSectionTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcReceiptSectionTransfer.Location = new System.Drawing.Point(12, 12);
            this.tbcReceiptSectionTransfer.Name = "tbcReceiptSectionTransfer";
            this.tbcReceiptSectionTransfer.SelectedIndex = 0;
            this.tbcReceiptSectionTransfer.Size = new System.Drawing.Size(984, 597);
            this.tbcReceiptSectionTransfer.TabIndex = 0;
            // 
            // tbpSearchCondition
            // 
            this.tbpSearchCondition.Controls.Add(this.cbxMemo);
            this.tbpSearchCondition.Controls.Add(this.lblSectionName);
            this.tbpSearchCondition.Controls.Add(this.txtMemo);
            this.tbpSearchCondition.Controls.Add(this.lblCurrencyCode);
            this.tbpSearchCondition.Controls.Add(this.lblCustomerNameTo);
            this.tbpSearchCondition.Controls.Add(this.txtCurrencyCode);
            this.tbpSearchCondition.Controls.Add(this.btnCurrencyCode);
            this.tbpSearchCondition.Controls.Add(this.txtPayerName);
            this.tbpSearchCondition.Controls.Add(this.txtCustomerCodeTo);
            this.tbpSearchCondition.Controls.Add(this.cbxNoAssignment);
            this.tbpSearchCondition.Controls.Add(this.lblCustomerCodeFrom);
            this.tbpSearchCondition.Controls.Add(this.lblRecordedAt);
            this.tbpSearchCondition.Controls.Add(this.txtCustomerCodeFrom);
            this.tbpSearchCondition.Controls.Add(this.btnCustomerCodeFrom);
            this.tbpSearchCondition.Controls.Add(this.cbxPartAssignment);
            this.tbpSearchCondition.Controls.Add(this.lblCustomerNameFrom);
            this.tbpSearchCondition.Controls.Add(this.lblPayerName);
            this.tbpSearchCondition.Controls.Add(this.lblCustomerCodeTo);
            this.tbpSearchCondition.Controls.Add(this.lblAassignmentFlag);
            this.tbpSearchCondition.Controls.Add(this.cbxCustomerCodeTo);
            this.tbpSearchCondition.Controls.Add(this.datRecordedAtFrom);
            this.tbpSearchCondition.Controls.Add(this.btnCustomerCodeTo);
            this.tbpSearchCondition.Controls.Add(this.lblCategoryName);
            this.tbpSearchCondition.Controls.Add(this.lblSectionCode);
            this.tbpSearchCondition.Controls.Add(this.btnCategoryCode);
            this.tbpSearchCondition.Controls.Add(this.txtSectionCode);
            this.tbpSearchCondition.Controls.Add(this.lblCategoryCode);
            this.tbpSearchCondition.Controls.Add(this.btnSectionCode);
            this.tbpSearchCondition.Controls.Add(this.txtCategoryCode);
            this.tbpSearchCondition.Controls.Add(this.datRecordedAtTo);
            this.tbpSearchCondition.Controls.Add(this.lblRecordedAtTo);
            this.tbpSearchCondition.Location = new System.Drawing.Point(4, 24);
            this.tbpSearchCondition.Name = "tbpSearchCondition";
            this.tbpSearchCondition.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSearchCondition.Size = new System.Drawing.Size(976, 569);
            this.tbpSearchCondition.TabIndex = 0;
            this.tbpSearchCondition.Text = "検索条件";
            this.tbpSearchCondition.UseVisualStyleBackColor = true;
            // 
            // cbxMemo
            // 
            this.cbxMemo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cbxMemo.Location = new System.Drawing.Point(27, 257);
            this.cbxMemo.Name = "cbxMemo";
            this.cbxMemo.Size = new System.Drawing.Size(82, 18);
            this.cbxMemo.TabIndex = 14;
            this.cbxMemo.Text = "メモ有り";
            this.cbxMemo.UseVisualStyleBackColor = true;
            // 
            // lblSectionName
            // 
            this.lblSectionName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSectionName.DropDown.AllowDrop = false;
            this.lblSectionName.Enabled = false;
            this.lblSectionName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionName.HighlightText = true;
            this.lblSectionName.Location = new System.Drawing.Point(264, 222);
            this.lblSectionName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.ReadOnly = true;
            this.lblSectionName.Required = false;
            this.lblSectionName.Size = new System.Drawing.Size(311, 22);
            this.lblSectionName.TabIndex = 0;
            // 
            // txtMemo
            // 
            this.txtMemo.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMemo.DropDown.AllowDrop = false;
            this.txtMemo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemo.HighlightText = true;
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(112, 256);
            this.txtMemo.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtMemo.MaxLength = 100;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Required = false;
            this.txtMemo.Size = new System.Drawing.Size(463, 22);
            this.txtMemo.TabIndex = 15;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyCode.Location = new System.Drawing.Point(25, 291);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(81, 16);
            this.lblCurrencyCode.TabIndex = 0;
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomerNameTo
            // 
            this.lblCustomerNameTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerNameTo.DropDown.AllowDrop = false;
            this.lblCustomerNameTo.Enabled = false;
            this.lblCustomerNameTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNameTo.HighlightText = true;
            this.lblCustomerNameTo.Location = new System.Drawing.Point(263, 188);
            this.lblCustomerNameTo.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCustomerNameTo.Name = "lblCustomerNameTo";
            this.lblCustomerNameTo.ReadOnly = true;
            this.lblCustomerNameTo.Required = false;
            this.lblCustomerNameTo.Size = new System.Drawing.Size(313, 22);
            this.lblCustomerNameTo.TabIndex = 0;
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
            this.txtCurrencyCode.Location = new System.Drawing.Point(112, 290);
            this.txtCurrencyCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 16;
            this.txtCurrencyCode.Validated += new System.EventHandler(this.txtCurrencyCode_Validated);
            // 
            // btnCurrencyCode
            // 
            this.btnCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCurrencyCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrencyCode.Location = new System.Drawing.Point(158, 289);
            this.btnCurrencyCode.Name = "btnCurrencyCode";
            this.btnCurrencyCode.Size = new System.Drawing.Size(24, 24);
            this.btnCurrencyCode.TabIndex = 17;
            this.btnCurrencyCode.UseVisualStyleBackColor = false;
            this.btnCurrencyCode.Click += new System.EventHandler(this.btnCurrencyCode_Click);
            // 
            // txtPayerName
            // 
            this.txtPayerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPayerName.DropDown.AllowDrop = false;
            this.txtPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayerName.Format = "9AK@";
            this.txtPayerName.HighlightText = true;
            this.txtPayerName.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtPayerName.Location = new System.Drawing.Point(112, 56);
            this.txtPayerName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtPayerName.MaxLength = 140;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Required = false;
            this.txtPayerName.Size = new System.Drawing.Size(266, 22);
            this.txtPayerName.TabIndex = 3;
            // 
            // txtCustomerCodeTo
            // 
            this.txtCustomerCodeTo.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCodeTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerCodeTo.DropDown.AllowDrop = false;
            this.txtCustomerCodeTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerCodeTo.HighlightText = true;
            this.txtCustomerCodeTo.Location = new System.Drawing.Point(112, 188);
            this.txtCustomerCodeTo.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtCustomerCodeTo.Name = "txtCustomerCodeTo";
            this.txtCustomerCodeTo.Required = false;
            this.txtCustomerCodeTo.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCodeTo.TabIndex = 10;
            this.txtCustomerCodeTo.Validated += new System.EventHandler(this.txtCustomerCodeTo_Validated);
            // 
            // cbxNoAssignment
            // 
            this.cbxNoAssignment.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxNoAssignment.Location = new System.Drawing.Point(192, 124);
            this.cbxNoAssignment.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbxNoAssignment.Name = "cbxNoAssignment";
            this.cbxNoAssignment.Size = new System.Drawing.Size(62, 18);
            this.cbxNoAssignment.TabIndex = 7;
            this.cbxNoAssignment.Text = "未消込";
            this.cbxNoAssignment.UseVisualStyleBackColor = true;
            // 
            // lblCustomerCodeFrom
            // 
            this.lblCustomerCodeFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerCodeFrom.Location = new System.Drawing.Point(25, 156);
            this.lblCustomerCodeFrom.Name = "lblCustomerCodeFrom";
            this.lblCustomerCodeFrom.Size = new System.Drawing.Size(81, 16);
            this.lblCustomerCodeFrom.TabIndex = 0;
            this.lblCustomerCodeFrom.Text = "得意先コード";
            this.lblCustomerCodeFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordedAt.Location = new System.Drawing.Point(25, 24);
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Size = new System.Drawing.Size(81, 16);
            this.lblRecordedAt.TabIndex = 0;
            this.lblRecordedAt.Text = "入金日";
            this.lblRecordedAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCustomerCodeFrom
            // 
            this.txtCustomerCodeFrom.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCodeFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerCodeFrom.DropDown.AllowDrop = false;
            this.txtCustomerCodeFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerCodeFrom.HighlightText = true;
            this.txtCustomerCodeFrom.Location = new System.Drawing.Point(112, 154);
            this.txtCustomerCodeFrom.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtCustomerCodeFrom.Name = "txtCustomerCodeFrom";
            this.txtCustomerCodeFrom.Required = false;
            this.txtCustomerCodeFrom.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCodeFrom.TabIndex = 8;
            this.txtCustomerCodeFrom.Validated += new System.EventHandler(this.txtCustomerCodeFrom_Validated);
            // 
            // btnCustomerCodeFrom
            // 
            this.btnCustomerCodeFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerCodeFrom.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomerCodeFrom.Location = new System.Drawing.Point(233, 152);
            this.btnCustomerCodeFrom.Name = "btnCustomerCodeFrom";
            this.btnCustomerCodeFrom.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerCodeFrom.TabIndex = 9;
            this.btnCustomerCodeFrom.UseVisualStyleBackColor = false;
            this.btnCustomerCodeFrom.Click += new System.EventHandler(this.btnCustomerCodeFrom_Click);
            // 
            // cbxPartAssignment
            // 
            this.cbxPartAssignment.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxPartAssignment.Location = new System.Drawing.Point(112, 124);
            this.cbxPartAssignment.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cbxPartAssignment.Name = "cbxPartAssignment";
            this.cbxPartAssignment.Size = new System.Drawing.Size(74, 18);
            this.cbxPartAssignment.TabIndex = 6;
            this.cbxPartAssignment.Text = "一部消込";
            this.cbxPartAssignment.UseVisualStyleBackColor = true;
            // 
            // lblCustomerNameFrom
            // 
            this.lblCustomerNameFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerNameFrom.DropDown.AllowDrop = false;
            this.lblCustomerNameFrom.Enabled = false;
            this.lblCustomerNameFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNameFrom.HighlightText = true;
            this.lblCustomerNameFrom.Location = new System.Drawing.Point(263, 154);
            this.lblCustomerNameFrom.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCustomerNameFrom.Name = "lblCustomerNameFrom";
            this.lblCustomerNameFrom.ReadOnly = true;
            this.lblCustomerNameFrom.Required = false;
            this.lblCustomerNameFrom.Size = new System.Drawing.Size(313, 22);
            this.lblCustomerNameFrom.TabIndex = 0;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPayerName.Location = new System.Drawing.Point(25, 59);
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Size = new System.Drawing.Size(81, 16);
            this.lblPayerName.TabIndex = 0;
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomerCodeTo
            // 
            this.lblCustomerCodeTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerCodeTo.Location = new System.Drawing.Point(69, 190);
            this.lblCustomerCodeTo.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblCustomerCodeTo.Name = "lblCustomerCodeTo";
            this.lblCustomerCodeTo.Size = new System.Drawing.Size(20, 16);
            this.lblCustomerCodeTo.TabIndex = 0;
            this.lblCustomerCodeTo.Text = "～";
            this.lblCustomerCodeTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAassignmentFlag
            // 
            this.lblAassignmentFlag.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAassignmentFlag.Location = new System.Drawing.Point(25, 125);
            this.lblAassignmentFlag.Name = "lblAassignmentFlag";
            this.lblAassignmentFlag.Size = new System.Drawing.Size(81, 16);
            this.lblAassignmentFlag.TabIndex = 0;
            this.lblAassignmentFlag.Text = "消込区分";
            this.lblAassignmentFlag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxCustomerCodeTo
            // 
            this.cbxCustomerCodeTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCustomerCodeTo.Location = new System.Drawing.Point(90, 190);
            this.cbxCustomerCodeTo.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.cbxCustomerCodeTo.Name = "cbxCustomerCodeTo";
            this.cbxCustomerCodeTo.Size = new System.Drawing.Size(16, 18);
            this.cbxCustomerCodeTo.TabIndex = 0;
            this.cbxCustomerCodeTo.TabStop = false;
            this.cbxCustomerCodeTo.UseVisualStyleBackColor = true;
            // 
            // datRecordedAtFrom
            // 
            this.datRecordedAtFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datRecordedAtFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datRecordedAtFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datRecordedAtFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datRecordedAtFrom.Location = new System.Drawing.Point(112, 22);
            this.datRecordedAtFrom.Margin = new System.Windows.Forms.Padding(3, 18, 3, 6);
            this.datRecordedAtFrom.Name = "datRecordedAtFrom";
            this.datRecordedAtFrom.Required = false;
            this.datRecordedAtFrom.Size = new System.Drawing.Size(115, 22);
            this.datRecordedAtFrom.Spin.AllowSpin = false;
            this.datRecordedAtFrom.TabIndex = 1;
            this.datRecordedAtFrom.Value = null;
            // 
            // btnCustomerCodeTo
            // 
            this.btnCustomerCodeTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerCodeTo.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomerCodeTo.Location = new System.Drawing.Point(233, 186);
            this.btnCustomerCodeTo.Name = "btnCustomerCodeTo";
            this.btnCustomerCodeTo.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerCodeTo.TabIndex = 11;
            this.btnCustomerCodeTo.UseVisualStyleBackColor = false;
            this.btnCustomerCodeTo.Click += new System.EventHandler(this.btnCustomerCodeTo_Click);
            // 
            // lblCategoryName
            // 
            this.lblCategoryName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCategoryName.DropDown.AllowDrop = false;
            this.lblCategoryName.Enabled = false;
            this.lblCategoryName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoryName.HighlightText = true;
            this.lblCategoryName.Location = new System.Drawing.Point(178, 90);
            this.lblCategoryName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCategoryName.Name = "lblCategoryName";
            this.lblCategoryName.ReadOnly = true;
            this.lblCategoryName.Required = false;
            this.lblCategoryName.Size = new System.Drawing.Size(397, 22);
            this.lblCategoryName.TabIndex = 0;
            // 
            // lblSectionCode
            // 
            this.lblSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionCode.Location = new System.Drawing.Point(25, 224);
            this.lblSectionCode.Name = "lblSectionCode";
            this.lblSectionCode.Size = new System.Drawing.Size(81, 16);
            this.lblSectionCode.TabIndex = 0;
            this.lblSectionCode.Text = "入金部門コード";
            this.lblSectionCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCategoryCode
            // 
            this.btnCategoryCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCategoryCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCategoryCode.Location = new System.Drawing.Point(148, 89);
            this.btnCategoryCode.Name = "btnCategoryCode";
            this.btnCategoryCode.Size = new System.Drawing.Size(24, 23);
            this.btnCategoryCode.TabIndex = 5;
            this.btnCategoryCode.UseVisualStyleBackColor = false;
            this.btnCategoryCode.Click += new System.EventHandler(this.btnCategoryCode_Click);
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtSectionCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSectionCode.DropDown.AllowDrop = false;
            this.txtSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSectionCode.HighlightText = true;
            this.txtSectionCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSectionCode.Location = new System.Drawing.Point(112, 222);
            this.txtSectionCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Required = false;
            this.txtSectionCode.Size = new System.Drawing.Size(115, 22);
            this.txtSectionCode.TabIndex = 12;
            this.txtSectionCode.Validated += new System.EventHandler(this.txtSectionCode_Validated);
            // 
            // lblCategoryCode
            // 
            this.lblCategoryCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoryCode.Location = new System.Drawing.Point(25, 92);
            this.lblCategoryCode.Name = "lblCategoryCode";
            this.lblCategoryCode.Size = new System.Drawing.Size(81, 16);
            this.lblCategoryCode.TabIndex = 0;
            this.lblCategoryCode.Text = "入金区分";
            this.lblCategoryCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSectionCode
            // 
            this.btnSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSectionCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSectionCode.Location = new System.Drawing.Point(233, 220);
            this.btnSectionCode.Name = "btnSectionCode";
            this.btnSectionCode.Size = new System.Drawing.Size(24, 24);
            this.btnSectionCode.TabIndex = 13;
            this.btnSectionCode.UseVisualStyleBackColor = false;
            this.btnSectionCode.Click += new System.EventHandler(this.btnSectionCode_Click);
            // 
            // txtCategoryCode
            // 
            this.txtCategoryCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCategoryCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCategoryCode.DropDown.AllowDrop = false;
            this.txtCategoryCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCategoryCode.Format = "9";
            this.txtCategoryCode.HighlightText = true;
            this.txtCategoryCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCategoryCode.Location = new System.Drawing.Point(112, 90);
            this.txtCategoryCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtCategoryCode.MaxLength = 2;
            this.txtCategoryCode.Name = "txtCategoryCode";
            this.txtCategoryCode.PaddingChar = '0';
            this.txtCategoryCode.Required = false;
            this.txtCategoryCode.Size = new System.Drawing.Size(30, 22);
            this.txtCategoryCode.TabIndex = 4;
            this.txtCategoryCode.Validated += new System.EventHandler(this.txtCategoryCode_Validated);
            // 
            // datRecordedAtTo
            // 
            this.datRecordedAtTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datRecordedAtTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datRecordedAtTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datRecordedAtTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datRecordedAtTo.Location = new System.Drawing.Point(263, 22);
            this.datRecordedAtTo.Margin = new System.Windows.Forms.Padding(3, 18, 3, 6);
            this.datRecordedAtTo.Name = "datRecordedAtTo";
            this.datRecordedAtTo.Required = false;
            this.datRecordedAtTo.Size = new System.Drawing.Size(115, 22);
            this.datRecordedAtTo.Spin.AllowSpin = false;
            this.datRecordedAtTo.TabIndex = 2;
            this.datRecordedAtTo.Value = null;
            // 
            // lblRecordedAtTo
            // 
            this.lblRecordedAtTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordedAtTo.Location = new System.Drawing.Point(235, 24);
            this.lblRecordedAtTo.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblRecordedAtTo.Name = "lblRecordedAtTo";
            this.lblRecordedAtTo.Size = new System.Drawing.Size(20, 16);
            this.lblRecordedAtTo.TabIndex = 3;
            this.lblRecordedAtTo.Text = "～";
            this.lblRecordedAtTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbpSearchResult
            // 
            this.tbpSearchResult.Controls.Add(this.lblNumber);
            this.tbpSearchResult.Controls.Add(this.lblCount);
            this.tbpSearchResult.Controls.Add(this.grdReceiptData);
            this.tbpSearchResult.Controls.Add(this.lblReceiptAmount);
            this.tbpSearchResult.Controls.Add(this.lblReceiptAmountTotal);
            this.tbpSearchResult.Controls.Add(this.lblRemainAmountTotal);
            this.tbpSearchResult.Controls.Add(this.lblRemainAmount);
            this.tbpSearchResult.Location = new System.Drawing.Point(4, 24);
            this.tbpSearchResult.Name = "tbpSearchResult";
            this.tbpSearchResult.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSearchResult.Size = new System.Drawing.Size(976, 569);
            this.tbpSearchResult.TabIndex = 1;
            this.tbpSearchResult.Text = "検索結果";
            this.tbpSearchResult.UseVisualStyleBackColor = true;
            // 
            // lblNumber
            // 
            this.lblNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumber.Location = new System.Drawing.Point(418, 543);
            this.lblNumber.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(31, 16);
            this.lblNumber.TabIndex = 0;
            this.lblNumber.Text = "件数";
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCount.DropDown.AllowDrop = false;
            this.lblCount.Enabled = false;
            this.lblCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.HighlightText = true;
            this.lblCount.Location = new System.Drawing.Point(450, 541);
            this.lblCount.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.lblCount.Name = "lblCount";
            this.lblCount.ReadOnly = true;
            this.lblCount.Required = false;
            this.lblCount.Size = new System.Drawing.Size(120, 22);
            this.lblCount.TabIndex = 0;
            // 
            // grdReceiptData
            // 
            this.grdReceiptData.AllowAutoExtend = true;
            this.grdReceiptData.AllowUserToAddRows = false;
            this.grdReceiptData.AllowUserToShiftSelect = true;
            this.grdReceiptData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdReceiptData.Location = new System.Drawing.Point(6, 6);
            this.grdReceiptData.Name = "grdReceiptData";
            this.grdReceiptData.Size = new System.Drawing.Size(964, 529);
            this.grdReceiptData.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdReceiptData.TabIndex = 0;
            this.grdReceiptData.Text = "vOneGridControl1";
            this.grdReceiptData.CellValidated += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptData_CellValidated);
            this.grdReceiptData.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptData_CellValueChanged);
            this.grdReceiptData.CellContentButtonClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptData_CellContentButtonClick);
            this.grdReceiptData.CellEditedFormattedValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs>(this.grdReceiptData_CellEditedFormattedValueChanged);
            this.grdReceiptData.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.grdReceiptData_PreviewKeyDown);
            // 
            // lblReceiptAmount
            // 
            this.lblReceiptAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReceiptAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptAmount.Location = new System.Drawing.Point(582, 543);
            this.lblReceiptAmount.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblReceiptAmount.Name = "lblReceiptAmount";
            this.lblReceiptAmount.Size = new System.Drawing.Size(67, 16);
            this.lblReceiptAmount.TabIndex = 0;
            this.lblReceiptAmount.Text = "入金額合計";
            this.lblReceiptAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReceiptAmountTotal
            // 
            this.lblReceiptAmountTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReceiptAmountTotal.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblReceiptAmountTotal.DropDown.AllowDrop = false;
            this.lblReceiptAmountTotal.Enabled = false;
            this.lblReceiptAmountTotal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptAmountTotal.HighlightText = true;
            this.lblReceiptAmountTotal.Location = new System.Drawing.Point(650, 541);
            this.lblReceiptAmountTotal.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.lblReceiptAmountTotal.Name = "lblReceiptAmountTotal";
            this.lblReceiptAmountTotal.ReadOnly = true;
            this.lblReceiptAmountTotal.Required = false;
            this.lblReceiptAmountTotal.Size = new System.Drawing.Size(120, 22);
            this.lblReceiptAmountTotal.TabIndex = 0;
            // 
            // lblRemainAmountTotal
            // 
            this.lblRemainAmountTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemainAmountTotal.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRemainAmountTotal.DropDown.AllowDrop = false;
            this.lblRemainAmountTotal.Enabled = false;
            this.lblRemainAmountTotal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainAmountTotal.HighlightText = true;
            this.lblRemainAmountTotal.Location = new System.Drawing.Point(850, 541);
            this.lblRemainAmountTotal.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.lblRemainAmountTotal.Name = "lblRemainAmountTotal";
            this.lblRemainAmountTotal.ReadOnly = true;
            this.lblRemainAmountTotal.Required = false;
            this.lblRemainAmountTotal.Size = new System.Drawing.Size(120, 22);
            this.lblRemainAmountTotal.TabIndex = 0;
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemainAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainAmount.Location = new System.Drawing.Point(782, 543);
            this.lblRemainAmount.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Size = new System.Drawing.Size(67, 16);
            this.lblRemainAmount.TabIndex = 0;
            this.lblRemainAmount.Text = "入金残合計";
            this.lblRemainAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PD0801
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tbcReceiptSectionTransfer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PD0801";
            this.Load += new System.EventHandler(this.PD0801_Load);
            this.tbcReceiptSectionTransfer.ResumeLayout(false);
            this.tbpSearchCondition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtTo)).EndInit();
            this.tbpSearchResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblReceiptAmountTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemainAmountTotal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcReceiptSectionTransfer;
        private System.Windows.Forms.TabPage tbpSearchCondition;
        private System.Windows.Forms.TabPage tbpSearchResult;
        private Common.Controls.VOneLabelControl lblRecordedAt;
        private Common.Controls.VOneDateControl datRecordedAtFrom;
        private Common.Controls.VOneDateControl datRecordedAtTo;
        private Common.Controls.VOneLabelControl lblRecordedAtTo;
        private Common.Controls.VOneLabelControl lblPayerName;
        private Common.Controls.VOneTextControl txtPayerName;
        private Common.Controls.VOneLabelControl lblCategoryCode;
        private Common.Controls.VOneTextControl txtCategoryCode;
        private System.Windows.Forms.Button btnCategoryCode;
        private Common.Controls.VOneDispLabelControl lblCategoryName;
        private Common.Controls.VOneLabelControl lblAassignmentFlag;
        private System.Windows.Forms.CheckBox cbxPartAssignment;
        private System.Windows.Forms.CheckBox cbxNoAssignment;
        private Common.Controls.VOneLabelControl lblCustomerCodeFrom;
        private Common.Controls.VOneTextControl txtCustomerCodeFrom;
        private System.Windows.Forms.Button btnCustomerCodeFrom;
        private Common.Controls.VOneDispLabelControl lblCustomerNameFrom;
        private Common.Controls.VOneLabelControl lblCustomerCodeTo;
        private System.Windows.Forms.CheckBox cbxCustomerCodeTo;
        private Common.Controls.VOneTextControl txtCustomerCodeTo;
        private Common.Controls.VOneDispLabelControl lblCustomerNameTo;
        private System.Windows.Forms.Button btnCustomerCodeTo;
        private Common.Controls.VOneLabelControl lblSectionCode;
        private Common.Controls.VOneTextControl txtSectionCode;
        private System.Windows.Forms.Button btnSectionCode;
        private Common.Controls.VOneDispLabelControl lblSectionName;
        private System.Windows.Forms.CheckBox cbxMemo;
        private Common.Controls.VOneTextControl txtMemo;
        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrencyCode;
        private Common.Controls.VOneGridControl grdReceiptData;
        private Common.Controls.VOneLabelControl lblNumber;
        private Common.Controls.VOneDispLabelControl lblCount;
        private Common.Controls.VOneLabelControl lblReceiptAmount;
        private Common.Controls.VOneDispLabelControl lblReceiptAmountTotal;
        private Common.Controls.VOneLabelControl lblRemainAmount;
        private Common.Controls.VOneDispLabelControl lblRemainAmountTotal;
    }
}
