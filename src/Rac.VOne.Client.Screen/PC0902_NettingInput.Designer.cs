namespace Rac.VOne.Client.Screen
{
    partial class PC0902
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlNetting = new System.Windows.Forms.Panel();
            this.nmbTotalOffset = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.lblOffsetTotal = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdNettingDisplay = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblData = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlBilling = new System.Windows.Forms.Panel();
            this.nmbTotalFee = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.lblReceiptTotal = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdNettingInput = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.grbTop = new System.Windows.Forms.GroupBox();
            this.lblPaymentDueDate = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.datPaymentDate = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnSectionCode = new System.Windows.Forms.Button();
            this.lblSectionCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtSectionCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblSectionName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.pnlNetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbTotalOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdNettingDisplay)).BeginInit();
            this.pnlBilling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbTotalFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdNettingInput)).BeginInit();
            this.grbTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datPaymentDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlNetting
            // 
            this.pnlNetting.Controls.Add(this.nmbTotalOffset);
            this.pnlNetting.Controls.Add(this.lblOffsetTotal);
            this.pnlNetting.Controls.Add(this.grdNettingDisplay);
            this.pnlNetting.Controls.Add(this.lblData);
            this.pnlNetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlNetting.Location = new System.Drawing.Point(12, 302);
            this.pnlNetting.Name = "pnlNetting";
            this.pnlNetting.Size = new System.Drawing.Size(984, 307);
            this.pnlNetting.TabIndex = 1;
            // 
            // nmbTotalOffset
            // 
            this.nmbTotalOffset.AllowDeleteToNull = true;
            this.nmbTotalOffset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.nmbTotalOffset.DropDown.AllowDrop = false;
            this.nmbTotalOffset.Enabled = false;
            this.nmbTotalOffset.Fields.IntegerPart.MinDigits = 1;
            this.nmbTotalOffset.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nmbTotalOffset.HighlightText = true;
            this.nmbTotalOffset.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbTotalOffset.Location = new System.Drawing.Point(860, 280);
            this.nmbTotalOffset.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.nmbTotalOffset.Name = "nmbTotalOffset";
            this.nmbTotalOffset.ReadOnly = true;
            this.nmbTotalOffset.Required = false;
            this.nmbTotalOffset.Size = new System.Drawing.Size(115, 22);
            this.nmbTotalOffset.Spin.AllowSpin = false;
            this.nmbTotalOffset.TabIndex = 8;
            // 
            // lblOffsetTotal
            // 
            this.lblOffsetTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblOffsetTotal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOffsetTotal.Location = new System.Drawing.Point(804, 283);
            this.lblOffsetTotal.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblOffsetTotal.Name = "lblOffsetTotal";
            this.lblOffsetTotal.Size = new System.Drawing.Size(55, 16);
            this.lblOffsetTotal.TabIndex = 3;
            this.lblOffsetTotal.Text = "相殺合計";
            // 
            // grdNettingDisplay
            // 
            this.grdNettingDisplay.AllowAutoExtend = true;
            this.grdNettingDisplay.AllowUserToAddRows = false;
            this.grdNettingDisplay.AllowUserToShiftSelect = true;
            this.grdNettingDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdNettingDisplay.Location = new System.Drawing.Point(9, 28);
            this.grdNettingDisplay.Name = "grdNettingDisplay";
            this.grdNettingDisplay.Size = new System.Drawing.Size(966, 246);
            this.grdNettingDisplay.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdNettingDisplay.TabIndex = 5;
            this.grdNettingDisplay.Text = "vOneGridControl1";
            this.grdNettingDisplay.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdNettingDisplay_CellEnter);
            // 
            // lblData
            // 
            this.lblData.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblData.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblData.Location = new System.Drawing.Point(9, 6);
            this.lblData.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(66, 16);
            this.lblData.TabIndex = 3;
            this.lblData.Text = "相殺データ";
            this.lblData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBilling
            // 
            this.pnlBilling.Controls.Add(this.nmbTotalFee);
            this.pnlBilling.Controls.Add(this.lblReceiptTotal);
            this.pnlBilling.Controls.Add(this.grdNettingInput);
            this.pnlBilling.Controls.Add(this.grbTop);
            this.pnlBilling.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBilling.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlBilling.Location = new System.Drawing.Point(12, 12);
            this.pnlBilling.Name = "pnlBilling";
            this.pnlBilling.Size = new System.Drawing.Size(984, 290);
            this.pnlBilling.TabIndex = 1;
            // 
            // nmbTotalFee
            // 
            this.nmbTotalFee.AllowDeleteToNull = true;
            this.nmbTotalFee.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.nmbTotalFee.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.nmbTotalFee.DropDown.AllowDrop = false;
            this.nmbTotalFee.Enabled = false;
            this.nmbTotalFee.Fields.IntegerPart.MinDigits = 1;
            this.nmbTotalFee.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nmbTotalFee.HighlightText = true;
            this.nmbTotalFee.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbTotalFee.Location = new System.Drawing.Point(860, 262);
            this.nmbTotalFee.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.nmbTotalFee.Name = "nmbTotalFee";
            this.nmbTotalFee.ReadOnly = true;
            this.nmbTotalFee.Required = false;
            this.nmbTotalFee.Size = new System.Drawing.Size(115, 22);
            this.nmbTotalFee.Spin.AllowSpin = false;
            this.nmbTotalFee.TabIndex = 6;
            // 
            // lblReceiptTotal
            // 
            this.lblReceiptTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblReceiptTotal.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblReceiptTotal.Location = new System.Drawing.Point(804, 264);
            this.lblReceiptTotal.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblReceiptTotal.Name = "lblReceiptTotal";
            this.lblReceiptTotal.Size = new System.Drawing.Size(55, 16);
            this.lblReceiptTotal.TabIndex = 3;
            this.lblReceiptTotal.Text = "合計金額";
            this.lblReceiptTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdNettingInput
            // 
            this.grdNettingInput.AllowAutoExtend = true;
            this.grdNettingInput.AllowUserToAddRows = false;
            this.grdNettingInput.AllowUserToShiftSelect = true;
            this.grdNettingInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdNettingInput.Location = new System.Drawing.Point(9, 113);
            this.grdNettingInput.Name = "grdNettingInput";
            this.grdNettingInput.Size = new System.Drawing.Size(966, 143);
            this.grdNettingInput.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdNettingInput.TabIndex = 4;
            this.grdNettingInput.Text = "vOneGridControl1";
            this.grdNettingInput.CellValidated += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdNettingInput_CellValidated);
            this.grdNettingInput.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdNettingInput_CellEnter);
            this.grdNettingInput.CellLeave += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdNettingInput_CellLeave);
            this.grdNettingInput.CellClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdNettingInput_CellClick);
            this.grdNettingDisplay.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdNettingDisplay_CurrentCellDirtyStateChanged);
            // 
            // grbTop
            // 
            this.grbTop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grbTop.Controls.Add(this.lblPaymentDueDate);
            this.grbTop.Controls.Add(this.lblCustomerName);
            this.grbTop.Controls.Add(this.datPaymentDate);
            this.grbTop.Controls.Add(this.txtCurrencyCode);
            this.grbTop.Controls.Add(this.btnSectionCode);
            this.grbTop.Controls.Add(this.lblSectionCode);
            this.grbTop.Controls.Add(this.lblCustomerCode);
            this.grbTop.Controls.Add(this.lblCurrencyCode);
            this.grbTop.Controls.Add(this.txtSectionCode);
            this.grbTop.Controls.Add(this.txtCustomerCode);
            this.grbTop.Controls.Add(this.lblSectionName);
            this.grbTop.Location = new System.Drawing.Point(9, 3);
            this.grbTop.Margin = new System.Windows.Forms.Padding(9, 3, 9, 3);
            this.grbTop.Name = "grbTop";
            this.grbTop.Padding = new System.Windows.Forms.Padding(0);
            this.grbTop.Size = new System.Drawing.Size(966, 104);
            this.grbTop.TabIndex = 1;
            this.grbTop.TabStop = false;
            // 
            // lblPaymentDueDate
            // 
            this.lblPaymentDueDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPaymentDueDate.Location = new System.Drawing.Point(19, 20);
            this.lblPaymentDueDate.Margin = new System.Windows.Forms.Padding(18, 3, 3, 3);
            this.lblPaymentDueDate.Name = "lblPaymentDueDate";
            this.lblPaymentDueDate.Size = new System.Drawing.Size(81, 16);
            this.lblPaymentDueDate.TabIndex = 19;
            this.lblPaymentDueDate.Text = "入金日";
            this.lblPaymentDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerName.DropDown.AllowDrop = false;
            this.lblCustomerName.Enabled = false;
            this.lblCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCustomerName.HighlightText = true;
            this.lblCustomerName.Location = new System.Drawing.Point(257, 45);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.ReadOnly = true;
            this.lblCustomerName.Required = false;
            this.lblCustomerName.Size = new System.Drawing.Size(290, 22);
            this.lblCustomerName.TabIndex = 13;
            // 
            // datPaymentDate
            // 
            this.datPaymentDate.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datPaymentDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.datPaymentDate.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datPaymentDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datPaymentDate.Location = new System.Drawing.Point(106, 17);
            this.datPaymentDate.Name = "datPaymentDate";
            this.datPaymentDate.Required = true;
            this.datPaymentDate.Size = new System.Drawing.Size(115, 22);
            this.datPaymentDate.Spin.AllowSpin = false;
            this.datPaymentDate.TabIndex = 1;
            this.datPaymentDate.Value = null;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Enabled = false;
            this.txtCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.Location = new System.Drawing.Point(317, 17);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.ReadOnly = true;
            this.txtCurrencyCode.Required = false;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 12;
            // 
            // btnSectionCode
            // 
            this.btnSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSectionCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSectionCode.Location = new System.Drawing.Point(227, 72);
            this.btnSectionCode.Name = "btnSectionCode";
            this.btnSectionCode.Size = new System.Drawing.Size(24, 24);
            this.btnSectionCode.TabIndex = 4;
            this.btnSectionCode.UseVisualStyleBackColor = true;
            this.btnSectionCode.Click += new System.EventHandler(this.btnSectionCode_Click);
            // 
            // lblSectionCode
            // 
            this.lblSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSectionCode.Location = new System.Drawing.Point(19, 76);
            this.lblSectionCode.Margin = new System.Windows.Forms.Padding(18, 3, 3, 3);
            this.lblSectionCode.Name = "lblSectionCode";
            this.lblSectionCode.Size = new System.Drawing.Size(81, 16);
            this.lblSectionCode.TabIndex = 16;
            this.lblSectionCode.Text = "入金部門コード";
            this.lblSectionCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCustomerCode.Location = new System.Drawing.Point(19, 48);
            this.lblCustomerCode.Margin = new System.Windows.Forms.Padding(18, 3, 3, 3);
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Size = new System.Drawing.Size(81, 16);
            this.lblCustomerCode.TabIndex = 3;
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCurrencyCode.Location = new System.Drawing.Point(254, 19);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(57, 16);
            this.lblCurrencyCode.TabIndex = 3;
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtSectionCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSectionCode.DropDown.AllowDrop = false;
            this.txtSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSectionCode.HighlightText = true;
            this.txtSectionCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSectionCode.Location = new System.Drawing.Point(106, 73);
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Required = true;
            this.txtSectionCode.Size = new System.Drawing.Size(115, 22);
            this.txtSectionCode.TabIndex = 3;
            this.txtSectionCode.Validated += new System.EventHandler(this.txtSectionCode_Validated);
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerCode.DropDown.AllowDrop = false;
            this.txtCustomerCode.Enabled = false;
            this.txtCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCustomerCode.HighlightText = true;
            this.txtCustomerCode.Location = new System.Drawing.Point(106, 45);
            this.txtCustomerCode.MaxLength = 20;
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.ReadOnly = true;
            this.txtCustomerCode.Required = false;
            this.txtCustomerCode.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCode.TabIndex = 2;
            // 
            // lblSectionName
            // 
            this.lblSectionName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSectionName.DropDown.AllowDrop = false;
            this.lblSectionName.Enabled = false;
            this.lblSectionName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSectionName.HighlightText = true;
            this.lblSectionName.Location = new System.Drawing.Point(257, 73);
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.ReadOnly = true;
            this.lblSectionName.Required = false;
            this.lblSectionName.Size = new System.Drawing.Size(290, 22);
            this.lblSectionName.TabIndex = 18;
            // 
            // PC0902
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlNetting);
            this.Controls.Add(this.pnlBilling);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PC0902";
            this.Load += new System.EventHandler(this.PC0902_Load);
            this.pnlNetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmbTotalOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdNettingDisplay)).EndInit();
            this.pnlBilling.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmbTotalFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdNettingInput)).EndInit();
            this.grbTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datPaymentDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneGridControl grdNettingInput;
        private Common.Controls.VOneGridControl grdNettingDisplay;
        private Common.Controls.VOneDateControl datPaymentDate;
        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneLabelControl lblReceiptTotal;
        private Common.Controls.VOneNumberControl nmbTotalFee;
        private Common.Controls.VOneLabelControl lblOffsetTotal;
        private Common.Controls.VOneNumberControl nmbTotalOffset;
        private Common.Controls.VOneTextControl txtCustomerCode;
        private Common.Controls.VOneLabelControl lblCustomerCode;
        private Common.Controls.VOneDispLabelControl lblCustomerName;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private Common.Controls.VOneLabelControl lblPaymentDueDate;
        private Common.Controls.VOneDispLabelControl lblSectionName;
        private Common.Controls.VOneLabelControl lblSectionCode;
        private System.Windows.Forms.Button btnSectionCode;
        private Common.Controls.VOneTextControl txtSectionCode;
        private Common.Controls.VOneLabelControl lblData;
        private System.Windows.Forms.GroupBox grbTop;
        private System.Windows.Forms.Panel pnlBilling;
        private System.Windows.Forms.Panel pnlNetting;
    }
}