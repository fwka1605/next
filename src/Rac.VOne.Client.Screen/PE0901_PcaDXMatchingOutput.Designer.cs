namespace Rac.VOne.Client.Screen
{
    partial class PE0901
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
            this.lblExtractionNumber = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractNumber = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblExtractionAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblRecordedAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblOutputDisplayNumber = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputNumber = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputDisplayAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.datRecordedAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.datRecordedAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblRecordedAtWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCurrencyCode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtTo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblExtractionNumber
            // 
            this.lblExtractionNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractionNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractionNumber.Location = new System.Drawing.Point(247, 128);
            this.lblExtractionNumber.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblExtractionNumber.Name = "lblExtractionNumber";
            this.lblExtractionNumber.Size = new System.Drawing.Size(115, 16);
            this.lblExtractionNumber.TabIndex = 15;
            this.lblExtractionNumber.Text = "抽出件数";
            this.lblExtractionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExtractNumber
            // 
            this.lblExtractNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractNumber.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblExtractNumber.DropDown.AllowDrop = false;
            this.lblExtractNumber.Enabled = false;
            this.lblExtractNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractNumber.HighlightText = true;
            this.lblExtractNumber.Location = new System.Drawing.Point(247, 153);
            this.lblExtractNumber.Margin = new System.Windows.Forms.Padding(3, 3, 9, 9);
            this.lblExtractNumber.Name = "lblExtractNumber";
            this.lblExtractNumber.ReadOnly = true;
            this.lblExtractNumber.Required = false;
            this.lblExtractNumber.Size = new System.Drawing.Size(115, 22);
            this.lblExtractNumber.TabIndex = 23;
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToResize = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(247, 187);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(514, 409);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 35;
            // 
            // lblExtractionAmount
            // 
            this.lblExtractionAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractionAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractionAmount.Location = new System.Drawing.Point(377, 128);
            this.lblExtractionAmount.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblExtractionAmount.Name = "lblExtractionAmount";
            this.lblExtractionAmount.Size = new System.Drawing.Size(115, 16);
            this.lblExtractionAmount.TabIndex = 22;
            this.lblExtractionAmount.Text = "抽出金額";
            this.lblExtractionAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblRecordedAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordedAt.Location = new System.Drawing.Point(145, 63);
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Size = new System.Drawing.Size(65, 16);
            this.lblRecordedAt.TabIndex = 25;
            this.lblRecordedAt.Text = "伝票日付";
            this.lblRecordedAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExtractAmount
            // 
            this.lblExtractAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblExtractAmount.DropDown.AllowDrop = false;
            this.lblExtractAmount.Enabled = false;
            this.lblExtractAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractAmount.HighlightText = true;
            this.lblExtractAmount.Location = new System.Drawing.Point(380, 153);
            this.lblExtractAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.lblExtractAmount.Name = "lblExtractAmount";
            this.lblExtractAmount.ReadOnly = true;
            this.lblExtractAmount.Required = false;
            this.lblExtractAmount.Size = new System.Drawing.Size(115, 22);
            this.lblExtractAmount.TabIndex = 20;
            // 
            // lblOutputDisplayNumber
            // 
            this.lblOutputDisplayNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputDisplayNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputDisplayNumber.Location = new System.Drawing.Point(510, 128);
            this.lblOutputDisplayNumber.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblOutputDisplayNumber.Name = "lblOutputDisplayNumber";
            this.lblOutputDisplayNumber.Size = new System.Drawing.Size(115, 16);
            this.lblOutputDisplayNumber.TabIndex = 19;
            this.lblOutputDisplayNumber.Text = "出力件数";
            this.lblOutputDisplayNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutputNumber
            // 
            this.lblOutputNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputNumber.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblOutputNumber.DropDown.AllowDrop = false;
            this.lblOutputNumber.Enabled = false;
            this.lblOutputNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputNumber.HighlightText = true;
            this.lblOutputNumber.Location = new System.Drawing.Point(513, 153);
            this.lblOutputNumber.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.lblOutputNumber.Name = "lblOutputNumber";
            this.lblOutputNumber.ReadOnly = true;
            this.lblOutputNumber.Required = false;
            this.lblOutputNumber.Size = new System.Drawing.Size(115, 22);
            this.lblOutputNumber.TabIndex = 18;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyCode.Location = new System.Drawing.Point(145, 97);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(65, 16);
            this.lblCurrencyCode.TabIndex = 17;
            this.lblCurrencyCode.Text = "通貨コード";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutputDisplayAmount
            // 
            this.lblOutputDisplayAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputDisplayAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputDisplayAmount.Location = new System.Drawing.Point(640, 128);
            this.lblOutputDisplayAmount.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblOutputDisplayAmount.Name = "lblOutputDisplayAmount";
            this.lblOutputDisplayAmount.Size = new System.Drawing.Size(115, 16);
            this.lblOutputDisplayAmount.TabIndex = 21;
            this.lblOutputDisplayAmount.Text = "出力金額";
            this.lblOutputDisplayAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutputAmount
            // 
            this.lblOutputAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblOutputAmount.DropDown.AllowDrop = false;
            this.lblOutputAmount.Enabled = false;
            this.lblOutputAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputAmount.HighlightText = true;
            this.lblOutputAmount.Location = new System.Drawing.Point(646, 153);
            this.lblOutputAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.lblOutputAmount.Name = "lblOutputAmount";
            this.lblOutputAmount.ReadOnly = true;
            this.lblOutputAmount.Required = false;
            this.lblOutputAmount.Size = new System.Drawing.Size(115, 22);
            this.lblOutputAmount.TabIndex = 16;
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCurrencyCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCurrencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCurrencyCode.DropDown.AllowDrop = false;
            this.txtCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrencyCode.Format = "A";
            this.txtCurrencyCode.HighlightText = true;
            this.txtCurrencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCurrencyCode.Location = new System.Drawing.Point(216, 94);
            this.txtCurrencyCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtCurrencyCode.MaxLength = 3;
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Required = true;
            this.txtCurrencyCode.Size = new System.Drawing.Size(40, 22);
            this.txtCurrencyCode.TabIndex = 34;
            // 
            // datRecordedAtFrom
            // 
            this.datRecordedAtFrom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datRecordedAtFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datRecordedAtFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datRecordedAtFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datRecordedAtFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datRecordedAtFrom.Location = new System.Drawing.Point(216, 60);
            this.datRecordedAtFrom.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.datRecordedAtFrom.Name = "datRecordedAtFrom";
            this.datRecordedAtFrom.Required = false;
            this.datRecordedAtFrom.Size = new System.Drawing.Size(115, 22);
            this.datRecordedAtFrom.Spin.AllowSpin = false;
            this.datRecordedAtFrom.TabIndex = 29;
            this.datRecordedAtFrom.Value = null;
            // 
            // datRecordedAtTo
            // 
            this.datRecordedAtTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datRecordedAtTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datRecordedAtTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datRecordedAtTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datRecordedAtTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datRecordedAtTo.Location = new System.Drawing.Point(367, 61);
            this.datRecordedAtTo.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.datRecordedAtTo.Name = "datRecordedAtTo";
            this.datRecordedAtTo.Required = false;
            this.datRecordedAtTo.Size = new System.Drawing.Size(115, 22);
            this.datRecordedAtTo.Spin.AllowSpin = false;
            this.datRecordedAtTo.TabIndex = 30;
            this.datRecordedAtTo.Value = null;
            // 
            // lblRecordedAtWave
            // 
            this.lblRecordedAtWave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblRecordedAtWave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordedAtWave.Location = new System.Drawing.Point(339, 63);
            this.lblRecordedAtWave.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblRecordedAtWave.Name = "lblRecordedAtWave";
            this.lblRecordedAtWave.Size = new System.Drawing.Size(20, 16);
            this.lblRecordedAtWave.TabIndex = 26;
            this.lblRecordedAtWave.Text = "～";
            this.lblRecordedAtWave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCurrencyCode
            // 
            this.btnCurrencyCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCurrencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCurrencyCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCurrencyCode.Location = new System.Drawing.Point(262, 92);
            this.btnCurrencyCode.Name = "btnCurrencyCode";
            this.btnCurrencyCode.Size = new System.Drawing.Size(24, 24);
            this.btnCurrencyCode.TabIndex = 36;
            this.btnCurrencyCode.UseVisualStyleBackColor = true;
            // 
            // PE0901
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblExtractionNumber);
            this.Controls.Add(this.lblExtractNumber);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblExtractionAmount);
            this.Controls.Add(this.lblRecordedAt);
            this.Controls.Add(this.lblExtractAmount);
            this.Controls.Add(this.lblOutputDisplayNumber);
            this.Controls.Add(this.lblOutputNumber);
            this.Controls.Add(this.lblCurrencyCode);
            this.Controls.Add(this.lblOutputDisplayAmount);
            this.Controls.Add(this.lblOutputAmount);
            this.Controls.Add(this.txtCurrencyCode);
            this.Controls.Add(this.btnCurrencyCode);
            this.Controls.Add(this.datRecordedAtFrom);
            this.Controls.Add(this.datRecordedAtTo);
            this.Controls.Add(this.lblRecordedAtWave);
            this.Name = "PE0901";
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOutputAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtTo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneLabelControl lblExtractionNumber;
        private Common.Controls.VOneDispLabelControl lblExtractNumber;
        private Common.Controls.VOneGridControl grid;
        private Common.Controls.VOneLabelControl lblExtractionAmount;
        private Common.Controls.VOneLabelControl lblRecordedAt;
        private Common.Controls.VOneDispLabelControl lblExtractAmount;
        private Common.Controls.VOneLabelControl lblOutputDisplayNumber;
        private Common.Controls.VOneDispLabelControl lblOutputNumber;
        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneLabelControl lblOutputDisplayAmount;
        private Common.Controls.VOneDispLabelControl lblOutputAmount;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrencyCode;
        private Common.Controls.VOneDateControl datRecordedAtFrom;
        private Common.Controls.VOneDateControl datRecordedAtTo;
        private Common.Controls.VOneLabelControl lblRecordedAtWave;
    }
}
