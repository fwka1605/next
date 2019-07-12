namespace Rac.VOne.Client.Screen
{
    partial class PD0701
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
            this.grdReceiptOutput = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblDispOutputAmt = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblDispOutputNo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblDispExtractAmt = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblDispExtractNo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblOutputAmt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputNo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractAmt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractNo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCurrency = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblWriteFile = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblRecordAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datRecordAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.datRecordAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCurrency = new System.Windows.Forms.Button();
            this.lblReceiptAtFromTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtOutputPath = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnWriteFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispOutputAmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispOutputNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractAmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordAtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputPath)).BeginInit();
            this.SuspendLayout();
            // 
            // grdReceiptOutput
            // 
            this.grdReceiptOutput.AllowAutoExtend = true;
            this.grdReceiptOutput.AllowUserToAddRows = false;
            this.grdReceiptOutput.AllowUserToShiftSelect = true;
            this.grdReceiptOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdReceiptOutput.HideSelection = true;
            this.grdReceiptOutput.Location = new System.Drawing.Point(247, 197);
            this.grdReceiptOutput.Name = "grdReceiptOutput";
            this.grdReceiptOutput.Size = new System.Drawing.Size(514, 409);
            this.grdReceiptOutput.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdReceiptOutput.TabIndex = 12;
            this.grdReceiptOutput.Text = "vOneGridControl1";
            this.grdReceiptOutput.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptOutput_CellValueChanged);
            this.grdReceiptOutput.CellEditedFormattedValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs>(this.grdReceiptOutput_CellEditedFormattedValueChanged);
            // 
            // lblDispOutputAmt
            // 
            this.lblDispOutputAmt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDispOutputAmt.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispOutputAmt.DropDown.AllowDrop = false;
            this.lblDispOutputAmt.Enabled = false;
            this.lblDispOutputAmt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDispOutputAmt.HighlightText = true;
            this.lblDispOutputAmt.Location = new System.Drawing.Point(646, 163);
            this.lblDispOutputAmt.Name = "lblDispOutputAmt";
            this.lblDispOutputAmt.ReadOnly = true;
            this.lblDispOutputAmt.Required = false;
            this.lblDispOutputAmt.Size = new System.Drawing.Size(115, 22);
            this.lblDispOutputAmt.TabIndex = 20;
            this.lblDispOutputAmt.Paint += new System.Windows.Forms.PaintEventHandler(this.lblDispOutputAmt_Paint);
            // 
            // lblDispOutputNo
            // 
            this.lblDispOutputNo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDispOutputNo.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispOutputNo.DropDown.AllowDrop = false;
            this.lblDispOutputNo.Enabled = false;
            this.lblDispOutputNo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDispOutputNo.HighlightText = true;
            this.lblDispOutputNo.Location = new System.Drawing.Point(513, 163);
            this.lblDispOutputNo.Name = "lblDispOutputNo";
            this.lblDispOutputNo.ReadOnly = true;
            this.lblDispOutputNo.Required = false;
            this.lblDispOutputNo.Size = new System.Drawing.Size(115, 22);
            this.lblDispOutputNo.TabIndex = 19;
            this.lblDispOutputNo.Paint += new System.Windows.Forms.PaintEventHandler(this.lblDispOutputNo_Paint);
            // 
            // lblDispExtractAmt
            // 
            this.lblDispExtractAmt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDispExtractAmt.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispExtractAmt.DropDown.AllowDrop = false;
            this.lblDispExtractAmt.Enabled = false;
            this.lblDispExtractAmt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDispExtractAmt.HighlightText = true;
            this.lblDispExtractAmt.Location = new System.Drawing.Point(380, 163);
            this.lblDispExtractAmt.Name = "lblDispExtractAmt";
            this.lblDispExtractAmt.ReadOnly = true;
            this.lblDispExtractAmt.Required = false;
            this.lblDispExtractAmt.Size = new System.Drawing.Size(115, 22);
            this.lblDispExtractAmt.TabIndex = 18;
            this.lblDispExtractAmt.Paint += new System.Windows.Forms.PaintEventHandler(this.lblDispExtractAmt_Paint);
            // 
            // lblDispExtractNo
            // 
            this.lblDispExtractNo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDispExtractNo.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispExtractNo.DropDown.AllowDrop = false;
            this.lblDispExtractNo.Enabled = false;
            this.lblDispExtractNo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDispExtractNo.HighlightText = true;
            this.lblDispExtractNo.Location = new System.Drawing.Point(247, 163);
            this.lblDispExtractNo.Margin = new System.Windows.Forms.Padding(3, 3, 9, 9);
            this.lblDispExtractNo.Name = "lblDispExtractNo";
            this.lblDispExtractNo.ReadOnly = true;
            this.lblDispExtractNo.Required = false;
            this.lblDispExtractNo.Size = new System.Drawing.Size(115, 22);
            this.lblDispExtractNo.TabIndex = 17;
            this.lblDispExtractNo.Paint += new System.Windows.Forms.PaintEventHandler(this.lblDispExtractNo_Paint);
            // 
            // lblOutputAmt
            // 
            this.lblOutputAmt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputAmt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOutputAmt.Location = new System.Drawing.Point(640, 138);
            this.lblOutputAmt.Margin = new System.Windows.Forms.Padding(3);
            this.lblOutputAmt.Name = "lblOutputAmt";
            this.lblOutputAmt.Size = new System.Drawing.Size(115, 16);
            this.lblOutputAmt.TabIndex = 16;
            this.lblOutputAmt.Text = "出力金額";
            this.lblOutputAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutputNo
            // 
            this.lblOutputNo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputNo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOutputNo.Location = new System.Drawing.Point(510, 138);
            this.lblOutputNo.Margin = new System.Windows.Forms.Padding(3);
            this.lblOutputNo.Name = "lblOutputNo";
            this.lblOutputNo.Size = new System.Drawing.Size(115, 16);
            this.lblOutputNo.TabIndex = 15;
            this.lblOutputNo.Text = "出力件数";
            this.lblOutputNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExtractAmt
            // 
            this.lblExtractAmt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractAmt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExtractAmt.Location = new System.Drawing.Point(377, 138);
            this.lblExtractAmt.Margin = new System.Windows.Forms.Padding(3);
            this.lblExtractAmt.Name = "lblExtractAmt";
            this.lblExtractAmt.Size = new System.Drawing.Size(115, 16);
            this.lblExtractAmt.TabIndex = 14;
            this.lblExtractAmt.Text = "抽出金額";
            this.lblExtractAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExtractNo
            // 
            this.lblExtractNo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractNo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExtractNo.Location = new System.Drawing.Point(247, 138);
            this.lblExtractNo.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblExtractNo.Name = "lblExtractNo";
            this.lblExtractNo.Size = new System.Drawing.Size(115, 16);
            this.lblExtractNo.TabIndex = 13;
            this.lblExtractNo.Text = "抽出件数";
            this.lblExtractNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrency
            // 
            this.lblCurrency.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCurrency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCurrency.Location = new System.Drawing.Point(176, 107);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(65, 16);
            this.lblCurrency.TabIndex = 3;
            this.lblCurrency.Text = "通貨コード";
            this.lblCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWriteFile
            // 
            this.lblWriteFile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblWriteFile.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWriteFile.Location = new System.Drawing.Point(176, 38);
            this.lblWriteFile.Name = "lblWriteFile";
            this.lblWriteFile.Size = new System.Drawing.Size(65, 16);
            this.lblWriteFile.TabIndex = 1;
            this.lblWriteFile.Text = "書込ファイル";
            this.lblWriteFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecordAt
            // 
            this.lblRecordAt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblRecordAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRecordAt.Location = new System.Drawing.Point(176, 73);
            this.lblRecordAt.Name = "lblRecordAt";
            this.lblRecordAt.Size = new System.Drawing.Size(65, 16);
            this.lblRecordAt.TabIndex = 2;
            this.lblRecordAt.Text = "入金日";
            this.lblRecordAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datRecordAtFrom
            // 
            this.datRecordAtFrom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datRecordAtFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datRecordAtFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.datRecordAtFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datRecordAtFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datRecordAtFrom.Location = new System.Drawing.Point(247, 70);
            this.datRecordAtFrom.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.datRecordAtFrom.Name = "datRecordAtFrom";
            this.datRecordAtFrom.Required = false;
            this.datRecordAtFrom.Size = new System.Drawing.Size(115, 22);
            this.datRecordAtFrom.Spin.AllowSpin = false;
            this.datRecordAtFrom.TabIndex = 3;
            this.datRecordAtFrom.Value = null;
            // 
            // datRecordAtTo
            // 
            this.datRecordAtTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datRecordAtTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datRecordAtTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.datRecordAtTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datRecordAtTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datRecordAtTo.Location = new System.Drawing.Point(398, 71);
            this.datRecordAtTo.Name = "datRecordAtTo";
            this.datRecordAtTo.Required = false;
            this.datRecordAtTo.Size = new System.Drawing.Size(115, 22);
            this.datRecordAtTo.Spin.AllowSpin = false;
            this.datRecordAtTo.TabIndex = 4;
            this.datRecordAtTo.Value = null;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCurrencyCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCurrencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCurrencyCode.Format = "A";
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCurrencyCode.Location = new System.Drawing.Point(247, 104);
            this.txtCurrencyCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 5;
            this.txtCurrencyCode.Validated += new System.EventHandler(this.txtCurrencyCode_Validated);
            // 
            // btnCurrency
            // 
            this.btnCurrency.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCurrency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCurrency.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrency.Location = new System.Drawing.Point(293, 102);
            this.btnCurrency.Name = "btnCurrency";
            this.btnCurrency.Size = new System.Drawing.Size(24, 24);
            this.btnCurrency.TabIndex = 6;
            this.btnCurrency.UseVisualStyleBackColor = true;
            this.btnCurrency.Click += new System.EventHandler(this.btnCurrency_Click);
            // 
            // lblReceiptAtFromTo
            // 
            this.lblReceiptAtFromTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblReceiptAtFromTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblReceiptAtFromTo.Location = new System.Drawing.Point(370, 74);
            this.lblReceiptAtFromTo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblReceiptAtFromTo.Name = "lblReceiptAtFromTo";
            this.lblReceiptAtFromTo.Size = new System.Drawing.Size(20, 16);
            this.lblReceiptAtFromTo.TabIndex = 12;
            this.lblReceiptAtFromTo.Text = "～";
            // 
            // txtWriteFile
            // 
            this.txtOutputPath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtOutputPath.DropDown.AllowDrop = false;
            this.txtOutputPath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtOutputPath.HighlightText = true;
            this.txtOutputPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtOutputPath.Location = new System.Drawing.Point(247, 36);
            this.txtOutputPath.Margin = new System.Windows.Forms.Padding(3, 24, 3, 6);
            this.txtOutputPath.MaxLength = 255;
            this.txtOutputPath.Name = "txtWriteFile";
            this.txtOutputPath.Required = true;
            this.txtOutputPath.Size = new System.Drawing.Size(610, 22);
            this.txtOutputPath.TabIndex = 1;
            // 
            // btnWriteFile
            // 
            this.btnWriteFile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnWriteFile.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnWriteFile.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnWriteFile.Location = new System.Drawing.Point(863, 34);
            this.btnWriteFile.Name = "btnWriteFile";
            this.btnWriteFile.Size = new System.Drawing.Size(24, 24);
            this.btnWriteFile.TabIndex = 2;
            this.btnWriteFile.UseVisualStyleBackColor = true;
            this.btnWriteFile.Click += new System.EventHandler(this.btnWriteFile_Click);
            // 
            // PD0701
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblDispOutputAmt);
            this.Controls.Add(this.grdReceiptOutput);
            this.Controls.Add(this.lblDispOutputNo);
            this.Controls.Add(this.lblDispExtractAmt);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.lblDispExtractNo);
            this.Controls.Add(this.btnWriteFile);
            this.Controls.Add(this.lblOutputAmt);
            this.Controls.Add(this.lblReceiptAtFromTo);
            this.Controls.Add(this.lblOutputNo);
            this.Controls.Add(this.btnCurrency);
            this.Controls.Add(this.lblExtractAmt);
            this.Controls.Add(this.txtCurrencyCode);
            this.Controls.Add(this.lblExtractNo);
            this.Controls.Add(this.datRecordAtTo);
            this.Controls.Add(this.lblCurrency);
            this.Controls.Add(this.datRecordAtFrom);
            this.Controls.Add(this.lblWriteFile);
            this.Controls.Add(this.lblRecordAt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PD0701";
            this.Load += new System.EventHandler(this.PD0701_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispOutputAmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispOutputNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractAmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordAtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputPath)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneLabelControl lblCurrency;
        private Common.Controls.VOneLabelControl lblWriteFile;
        private Common.Controls.VOneLabelControl lblRecordAt;
        private Common.Controls.VOneDateControl datRecordAtFrom;
        private Common.Controls.VOneDateControl datRecordAtTo;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrency;
        private Common.Controls.VOneLabelControl lblReceiptAtFromTo;
        private Common.Controls.VOneTextControl txtOutputPath;
        private System.Windows.Forms.Button btnWriteFile;
        private Common.Controls.VOneLabelControl lblOutputNo;
        private Common.Controls.VOneLabelControl lblExtractAmt;
        private Common.Controls.VOneLabelControl lblExtractNo;
        private Common.Controls.VOneLabelControl lblOutputAmt;
        private Common.Controls.VOneDispLabelControl lblDispOutputAmt;
        private Common.Controls.VOneDispLabelControl lblDispOutputNo;
        private Common.Controls.VOneDispLabelControl lblDispExtractAmt;
        private Common.Controls.VOneDispLabelControl lblDispExtractNo;
        private Common.Controls.VOneGridControl grdReceiptOutput;
    }
}
