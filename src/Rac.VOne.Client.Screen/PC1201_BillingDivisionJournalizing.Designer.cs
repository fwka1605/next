namespace Rac.VOne.Client.Screen
{
    partial class PC1201
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
            this.lblOutputAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.grdBilling = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblOutputAmt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputNumber = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputNum = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datBillingTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblExtractionAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblExtAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractionNumber = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.datBillingFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblExtNumber = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblBilling = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtWriteFile = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblWriteFile = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblBillingTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCurrencyCode = new System.Windows.Forms.Button();
            this.btnFilePath = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datBillingTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractionAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractionNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datBillingFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWriteFile)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOutputAmount
            // 
            this.lblOutputAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblOutputAmount.DropDown.AllowDrop = false;
            this.lblOutputAmount.Enabled = false;
            this.lblOutputAmount.HighlightText = true;
            this.lblOutputAmount.Location = new System.Drawing.Point(581, 160);
            this.lblOutputAmount.Name = "lblOutputAmount";
            this.lblOutputAmount.ReadOnly = true;
            this.lblOutputAmount.Required = false;
            this.lblOutputAmount.Size = new System.Drawing.Size(120, 20);
            this.lblOutputAmount.TabIndex = 8;
            // 
            // grdBilling
            // 
            this.grdBilling.AllowAutoExtend = true;
            this.grdBilling.AllowUserToAddRows = false;
            this.grdBilling.AllowUserToResize = false;
            this.grdBilling.AllowUserToShiftSelect = true;
            this.grdBilling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdBilling.Location = new System.Drawing.Point(243, 225);
            this.grdBilling.Name = "grdBilling";
            this.grdBilling.Size = new System.Drawing.Size(522, 372);
            this.grdBilling.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdBilling.TabIndex = 26;
            this.grdBilling.Text = "vOneGridControl1";
            // 
            // lblOutputAmt
            // 
            this.lblOutputAmt.AutoSize = true;
            this.lblOutputAmt.Location = new System.Drawing.Point(581, 139);
            this.lblOutputAmt.Name = "lblOutputAmt";
            this.lblOutputAmt.Size = new System.Drawing.Size(53, 12);
            this.lblOutputAmt.TabIndex = 17;
            this.lblOutputAmt.Text = "出力金額";
            // 
            // lblOutputNumber
            // 
            this.lblOutputNumber.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblOutputNumber.DropDown.AllowDrop = false;
            this.lblOutputNumber.Enabled = false;
            this.lblOutputNumber.HighlightText = true;
            this.lblOutputNumber.Location = new System.Drawing.Point(455, 160);
            this.lblOutputNumber.Name = "lblOutputNumber";
            this.lblOutputNumber.ReadOnly = true;
            this.lblOutputNumber.Required = false;
            this.lblOutputNumber.Size = new System.Drawing.Size(120, 20);
            this.lblOutputNumber.TabIndex = 16;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Format = "A";
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCurrencyCode.Location = new System.Drawing.Point(203, 82);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 20);
            this.txtCurrencyCode.TabIndex = 24;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.AutoSize = true;
            this.lblCurrencyCode.Location = new System.Drawing.Point(134, 86);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(56, 12);
            this.lblCurrencyCode.TabIndex = 18;
            this.lblCurrencyCode.Text = "通貨コード";
            // 
            // lblOutputNum
            // 
            this.lblOutputNum.AutoSize = true;
            this.lblOutputNum.Location = new System.Drawing.Point(455, 139);
            this.lblOutputNum.Name = "lblOutputNum";
            this.lblOutputNum.Size = new System.Drawing.Size(53, 12);
            this.lblOutputNum.TabIndex = 14;
            this.lblOutputNum.Text = "出力件数";
            // 
            // datBillingTo
            // 
            this.datBillingTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datBillingTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datBillingTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datBillingTo.Location = new System.Drawing.Point(352, 54);
            this.datBillingTo.Name = "datBillingTo";
            this.datBillingTo.Required = false;
            this.datBillingTo.Size = new System.Drawing.Size(115, 20);
            this.datBillingTo.Spin.AllowSpin = false;
            this.datBillingTo.TabIndex = 23;
            this.datBillingTo.Value = null;
            // 
            // lblExtractionAmount
            // 
            this.lblExtractionAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblExtractionAmount.DropDown.AllowDrop = false;
            this.lblExtractionAmount.Enabled = false;
            this.lblExtractionAmount.HighlightText = true;
            this.lblExtractionAmount.Location = new System.Drawing.Point(329, 160);
            this.lblExtractionAmount.Name = "lblExtractionAmount";
            this.lblExtractionAmount.ReadOnly = true;
            this.lblExtractionAmount.Required = false;
            this.lblExtractionAmount.Size = new System.Drawing.Size(120, 20);
            this.lblExtractionAmount.TabIndex = 15;
            // 
            // lblExtAmount
            // 
            this.lblExtAmount.AutoSize = true;
            this.lblExtAmount.Location = new System.Drawing.Point(329, 139);
            this.lblExtAmount.Name = "lblExtAmount";
            this.lblExtAmount.Size = new System.Drawing.Size(53, 12);
            this.lblExtAmount.TabIndex = 13;
            this.lblExtAmount.Text = "抽出金額";
            // 
            // lblExtractionNumber
            // 
            this.lblExtractionNumber.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblExtractionNumber.DropDown.AllowDrop = false;
            this.lblExtractionNumber.Enabled = false;
            this.lblExtractionNumber.HighlightText = true;
            this.lblExtractionNumber.Location = new System.Drawing.Point(203, 160);
            this.lblExtractionNumber.Name = "lblExtractionNumber";
            this.lblExtractionNumber.ReadOnly = true;
            this.lblExtractionNumber.Required = false;
            this.lblExtractionNumber.Size = new System.Drawing.Size(120, 20);
            this.lblExtractionNumber.TabIndex = 12;
            // 
            // datBillingFrom
            // 
            this.datBillingFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datBillingFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datBillingFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datBillingFrom.Location = new System.Drawing.Point(203, 54);
            this.datBillingFrom.Name = "datBillingFrom";
            this.datBillingFrom.Required = false;
            this.datBillingFrom.Size = new System.Drawing.Size(115, 20);
            this.datBillingFrom.Spin.AllowSpin = false;
            this.datBillingFrom.TabIndex = 22;
            this.datBillingFrom.Value = null;
            // 
            // lblExtNumber
            // 
            this.lblExtNumber.AutoSize = true;
            this.lblExtNumber.Location = new System.Drawing.Point(203, 139);
            this.lblExtNumber.Name = "lblExtNumber";
            this.lblExtNumber.Size = new System.Drawing.Size(53, 12);
            this.lblExtNumber.TabIndex = 11;
            this.lblExtNumber.Text = "抽出件数";
            // 
            // lblBilling
            // 
            this.lblBilling.AutoSize = true;
            this.lblBilling.Location = new System.Drawing.Point(134, 58);
            this.lblBilling.Name = "lblBilling";
            this.lblBilling.Size = new System.Drawing.Size(41, 12);
            this.lblBilling.TabIndex = 10;
            this.lblBilling.Text = "請求日";
            // 
            // txtWriteFile
            // 
            this.txtWriteFile.DropDown.AllowDrop = false;
            this.txtWriteFile.HighlightText = true;
            this.txtWriteFile.Location = new System.Drawing.Point(203, 26);
            this.txtWriteFile.MaxLength = 255;
            this.txtWriteFile.Name = "txtWriteFile";
            this.txtWriteFile.Required = true;
            this.txtWriteFile.Size = new System.Drawing.Size(602, 20);
            this.txtWriteFile.TabIndex = 20;
            // 
            // lblWriteFile
            // 
            this.lblWriteFile.AutoSize = true;
            this.lblWriteFile.Location = new System.Drawing.Point(134, 30);
            this.lblWriteFile.Name = "lblWriteFile";
            this.lblWriteFile.Size = new System.Drawing.Size(63, 12);
            this.lblWriteFile.TabIndex = 9;
            this.lblWriteFile.Text = "書込ファイル";
            // 
            // lblBillingTo
            // 
            this.lblBillingTo.AutoSize = true;
            this.lblBillingTo.Location = new System.Drawing.Point(329, 58);
            this.lblBillingTo.Name = "lblBillingTo";
            this.lblBillingTo.Size = new System.Drawing.Size(17, 12);
            this.lblBillingTo.TabIndex = 19;
            this.lblBillingTo.Text = "～";
            // 
            // btnCurrencyCode
            // 
            this.btnCurrencyCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrencyCode.Location = new System.Drawing.Point(249, 80);
            this.btnCurrencyCode.Name = "btnCurrencyCode";
            this.btnCurrencyCode.Size = new System.Drawing.Size(24, 24);
            this.btnCurrencyCode.TabIndex = 25;
            this.btnCurrencyCode.UseVisualStyleBackColor = true;
            // 
            // btnFilePath
            // 
            this.btnFilePath.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnFilePath.Location = new System.Drawing.Point(811, 24);
            this.btnFilePath.Name = "btnFilePath";
            this.btnFilePath.Size = new System.Drawing.Size(24, 24);
            this.btnFilePath.TabIndex = 21;
            this.btnFilePath.UseVisualStyleBackColor = true;
            // 
            // PC1201
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblOutputAmount);
            this.Controls.Add(this.grdBilling);
            this.Controls.Add(this.btnCurrencyCode);
            this.Controls.Add(this.lblOutputAmt);
            this.Controls.Add(this.lblOutputNumber);
            this.Controls.Add(this.txtCurrencyCode);
            this.Controls.Add(this.lblCurrencyCode);
            this.Controls.Add(this.lblOutputNum);
            this.Controls.Add(this.datBillingTo);
            this.Controls.Add(this.lblExtractionAmount);
            this.Controls.Add(this.lblExtAmount);
            this.Controls.Add(this.lblExtractionNumber);
            this.Controls.Add(this.datBillingFrom);
            this.Controls.Add(this.lblExtNumber);
            this.Controls.Add(this.lblBilling);
            this.Controls.Add(this.txtWriteFile);
            this.Controls.Add(this.lblWriteFile);
            this.Controls.Add(this.btnFilePath);
            this.Controls.Add(this.lblBillingTo);
            this.Name = "PC1201";
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datBillingTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractionAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractionNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datBillingFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWriteFile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Controls.VOneDispLabelControl lblOutputAmount;
        private Common.Controls.VOneGridControl grdBilling;
        private System.Windows.Forms.Button btnCurrencyCode;
        private Common.Controls.VOneLabelControl lblOutputAmt;
        private Common.Controls.VOneDispLabelControl lblOutputNumber;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneLabelControl lblOutputNum;
        private Common.Controls.VOneDateControl datBillingTo;
        private Common.Controls.VOneDispLabelControl lblExtractionAmount;
        private Common.Controls.VOneLabelControl lblExtAmount;
        private Common.Controls.VOneDispLabelControl lblExtractionNumber;
        private Common.Controls.VOneDateControl datBillingFrom;
        private Common.Controls.VOneLabelControl lblExtNumber;
        private Common.Controls.VOneLabelControl lblBilling;
        private Common.Controls.VOneTextControl txtWriteFile;
        private Common.Controls.VOneLabelControl lblWriteFile;
        private System.Windows.Forms.Button btnFilePath;
        private Common.Controls.VOneLabelControl lblBillingTo;
    }
}
