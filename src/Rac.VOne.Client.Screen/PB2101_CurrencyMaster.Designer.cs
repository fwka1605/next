namespace Rac.VOne.Client.Screen
{
    partial class PB2101
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
            this.gbxCurrencyList = new System.Windows.Forms.GroupBox();
            this.grdCurrencyMaster = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxCurrencyInput = new System.Windows.Forms.GroupBox();
            this.lblTolerance = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblNote = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtNote = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDisplayOrder = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblPrecision = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblSymbol = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.nmbPrecision = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.nmbTolerance = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.txtSymbol = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.nmbDisplayOrder = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.gbxCurrencyList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCurrencyMaster)).BeginInit();
            this.gbxCurrencyInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbDisplayOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxCurrencyList
            // 
            this.gbxCurrencyList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxCurrencyList.Controls.Add(this.grdCurrencyMaster);
            this.gbxCurrencyList.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxCurrencyList.Location = new System.Drawing.Point(91, 15);
            this.gbxCurrencyList.Name = "gbxCurrencyList";
            this.gbxCurrencyList.Padding = new System.Windows.Forms.Padding(0);
            this.gbxCurrencyList.Size = new System.Drawing.Size(825, 366);
            this.gbxCurrencyList.TabIndex = 1;
            this.gbxCurrencyList.TabStop = false;
            this.gbxCurrencyList.Text = "□　登録済みの通貨";
            // 
            // grdCurrencyMaster
            // 
            this.grdCurrencyMaster.AllowAutoExtend = true;
            this.grdCurrencyMaster.AllowUserToAddRows = false;
            this.grdCurrencyMaster.AllowUserToShiftSelect = true;
            this.grdCurrencyMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdCurrencyMaster.Location = new System.Drawing.Point(22, 22);
            this.grdCurrencyMaster.Margin = new System.Windows.Forms.Padding(12, 12, 12, 6);
            this.grdCurrencyMaster.Name = "grdCurrencyMaster";
            this.grdCurrencyMaster.Size = new System.Drawing.Size(780, 338);
            this.grdCurrencyMaster.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdCurrencyMaster.TabIndex = 0;
            this.grdCurrencyMaster.TabStop = false;
            this.grdCurrencyMaster.Text = "vOneGridControl1";
            this.grdCurrencyMaster.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdCurrencyMaster_CellDoubleClick);
            // 
            // gbxCurrencyInput
            // 
            this.gbxCurrencyInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxCurrencyInput.Controls.Add(this.lblTolerance);
            this.gbxCurrencyInput.Controls.Add(this.txtCurrencyCode);
            this.gbxCurrencyInput.Controls.Add(this.lblNote);
            this.gbxCurrencyInput.Controls.Add(this.lblCurrencyCode);
            this.gbxCurrencyInput.Controls.Add(this.txtNote);
            this.gbxCurrencyInput.Controls.Add(this.lblName);
            this.gbxCurrencyInput.Controls.Add(this.lblDisplayOrder);
            this.gbxCurrencyInput.Controls.Add(this.lblPrecision);
            this.gbxCurrencyInput.Controls.Add(this.lblSymbol);
            this.gbxCurrencyInput.Controls.Add(this.nmbPrecision);
            this.gbxCurrencyInput.Controls.Add(this.nmbTolerance);
            this.gbxCurrencyInput.Controls.Add(this.txtSymbol);
            this.gbxCurrencyInput.Controls.Add(this.txtName);
            this.gbxCurrencyInput.Controls.Add(this.nmbDisplayOrder);
            this.gbxCurrencyInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxCurrencyInput.Location = new System.Drawing.Point(91, 387);
            this.gbxCurrencyInput.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.gbxCurrencyInput.Name = "gbxCurrencyInput";
            this.gbxCurrencyInput.Size = new System.Drawing.Size(825, 216);
            this.gbxCurrencyInput.TabIndex = 0;
            this.gbxCurrencyInput.TabStop = false;
            // 
            // lblTolerance
            // 
            this.lblTolerance.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTolerance.Location = new System.Drawing.Point(30, 187);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(91, 16);
            this.lblTolerance.TabIndex = 18;
            this.lblTolerance.Text = "手数料誤差金額";
            this.lblTolerance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCurrencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCurrencyCode.Format = "A";
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCurrencyCode.Location = new System.Drawing.Point(127, 18);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 1;
            this.txtCurrencyCode.Validated += new System.EventHandler(this.txtCurrencyCode_Validated);
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNote.Location = new System.Drawing.Point(30, 160);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(91, 16);
            this.lblNote.TabIndex = 16;
            this.lblNote.Text = "備考";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCurrencyCode.Location = new System.Drawing.Point(30, 20);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(91, 16);
            this.lblCurrencyCode.TabIndex = 0;
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNote
            // 
            this.txtNote.DropDown.AllowDrop = false;
            this.txtNote.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtNote.HighlightText = true;
            this.txtNote.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtNote.Location = new System.Drawing.Point(127, 157);
            this.txtNote.MaxLength = 100;
            this.txtNote.Name = "txtNote";
            this.txtNote.Required = false;
            this.txtNote.Size = new System.Drawing.Size(675, 22);
            this.txtNote.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblName.Location = new System.Drawing.Point(30, 48);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(91, 16);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "名称";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDisplayOrder
            // 
            this.lblDisplayOrder.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDisplayOrder.Location = new System.Drawing.Point(30, 132);
            this.lblDisplayOrder.Name = "lblDisplayOrder";
            this.lblDisplayOrder.Size = new System.Drawing.Size(91, 16);
            this.lblDisplayOrder.TabIndex = 17;
            this.lblDisplayOrder.Text = "表示順番";
            this.lblDisplayOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPrecision
            // 
            this.lblPrecision.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPrecision.Location = new System.Drawing.Point(30, 104);
            this.lblPrecision.Name = "lblPrecision";
            this.lblPrecision.Size = new System.Drawing.Size(91, 16);
            this.lblPrecision.TabIndex = 0;
            this.lblPrecision.Text = "小数点以下桁数";
            this.lblPrecision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSymbol
            // 
            this.lblSymbol.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSymbol.Location = new System.Drawing.Point(30, 76);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(91, 16);
            this.lblSymbol.TabIndex = 0;
            this.lblSymbol.Text = "単位";
            this.lblSymbol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nmbPrecision
            // 
            this.nmbPrecision.AllowDeleteToNull = true;
            this.nmbPrecision.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.nmbPrecision.DropDown.AllowDrop = false;
            this.nmbPrecision.Fields.DecimalPart.MaxDigits = 0;
            this.nmbPrecision.Fields.IntegerPart.MinDigits = 1;
            this.nmbPrecision.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nmbPrecision.HighlightText = true;
            this.nmbPrecision.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbPrecision.Location = new System.Drawing.Point(127, 101);
            this.nmbPrecision.MaxMinBehavior = GrapeCity.Win.Editors.MaxMinBehavior.CancelInput;
            this.nmbPrecision.MaxValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmbPrecision.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmbPrecision.Name = "nmbPrecision";
            this.nmbPrecision.Required = true;
            this.nmbPrecision.Size = new System.Drawing.Size(115, 22);
            this.nmbPrecision.Spin.AllowSpin = false;
            this.nmbPrecision.TabIndex = 4;
            // 
            // nmbTolerance
            // 
            this.nmbTolerance.AllowDeleteToNull = true;
            this.nmbTolerance.DropDown.AllowDrop = false;
            this.nmbTolerance.Fields.DecimalPart.MaxDigits = 5;
            this.nmbTolerance.Fields.IntegerPart.MinDigits = 1;
            this.nmbTolerance.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nmbTolerance.HighlightText = true;
            this.nmbTolerance.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbTolerance.Location = new System.Drawing.Point(127, 185);
            this.nmbTolerance.MaxMinBehavior = GrapeCity.Win.Editors.MaxMinBehavior.CancelInput;
            this.nmbTolerance.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            327680});
            this.nmbTolerance.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmbTolerance.Name = "nmbTolerance";
            this.nmbTolerance.Required = true;
            this.nmbTolerance.Size = new System.Drawing.Size(115, 22);
            this.nmbTolerance.Spin.AllowSpin = false;
            this.nmbTolerance.TabIndex = 7;
            // 
            // txtSymbol
            // 
            this.txtSymbol.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtSymbol.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSymbol.DropDown.AllowDrop = false;
            this.txtSymbol.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSymbol.HighlightText = true;
            this.txtSymbol.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtSymbol.Location = new System.Drawing.Point(127, 73);
            this.txtSymbol.MaxLength = 1;
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Required = true;
            this.txtSymbol.Size = new System.Drawing.Size(115, 22);
            this.txtSymbol.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtName.DropDown.AllowDrop = false;
            this.txtName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtName.HighlightText = true;
            this.txtName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtName.Location = new System.Drawing.Point(127, 45);
            this.txtName.MaxLength = 40;
            this.txtName.Name = "txtName";
            this.txtName.Required = true;
            this.txtName.Size = new System.Drawing.Size(500, 22);
            this.txtName.TabIndex = 2;
            // 
            // nmbDisplayOrder
            // 
            this.nmbDisplayOrder.AllowDeleteToNull = true;
            this.nmbDisplayOrder.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.nmbDisplayOrder.DropDown.AllowDrop = false;
            this.nmbDisplayOrder.Fields.DecimalPart.MaxDigits = 0;
            this.nmbDisplayOrder.Fields.IntegerPart.MinDigits = 1;
            this.nmbDisplayOrder.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nmbDisplayOrder.HighlightText = true;
            this.nmbDisplayOrder.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbDisplayOrder.Location = new System.Drawing.Point(127, 129);
            this.nmbDisplayOrder.MaxMinBehavior = GrapeCity.Win.Editors.MaxMinBehavior.CancelInput;
            this.nmbDisplayOrder.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmbDisplayOrder.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmbDisplayOrder.Name = "nmbDisplayOrder";
            this.nmbDisplayOrder.Required = true;
            this.nmbDisplayOrder.Size = new System.Drawing.Size(115, 22);
            this.nmbDisplayOrder.Spin.AllowSpin = false;
            this.nmbDisplayOrder.TabIndex = 5;
            // 
            // PB2101
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxCurrencyInput);
            this.Controls.Add(this.gbxCurrencyList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB2101";
            this.Load += new System.EventHandler(this.PB2101_Load);
            this.gbxCurrencyList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCurrencyMaster)).EndInit();
            this.gbxCurrencyInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbDisplayOrder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxCurrencyList;
        private Common.Controls.VOneGridControl grdCurrencyMaster;
        private System.Windows.Forms.GroupBox gbxCurrencyInput;
        private Common.Controls.VOneLabelControl lblTolerance;
        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneLabelControl lblName;
        private Common.Controls.VOneLabelControl lblSymbol;
        private Common.Controls.VOneTextControl txtName;
        private Common.Controls.VOneTextControl txtSymbol;
        private Common.Controls.VOneLabelControl lblPrecision;
        private Common.Controls.VOneLabelControl lblDisplayOrder;
        private Common.Controls.VOneLabelControl lblNote;
        private Common.Controls.VOneTextControl txtNote;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private Common.Controls.VOneNumberControl nmbPrecision;
        private Common.Controls.VOneNumberControl nmbDisplayOrder;
        private Common.Controls.VOneNumberControl nmbTolerance;
    }
}
