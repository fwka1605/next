namespace Rac.VOne.Client.Screen
{
    partial class PE0801
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
            this.lblExtractNumber = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDispExtractNumber = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblExtractAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblRecordedAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDispExtractAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCurrencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.datRecordedAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.datRecordedAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblRecordedAtWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCurrencyCode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtTo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblExtractNumber
            // 
            this.lblExtractNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractNumber.Location = new System.Drawing.Point(380, 128);
            this.lblExtractNumber.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblExtractNumber.Name = "lblExtractNumber";
            this.lblExtractNumber.Size = new System.Drawing.Size(115, 16);
            this.lblExtractNumber.TabIndex = 15;
            this.lblExtractNumber.Text = "連携件数";
            this.lblExtractNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDispExtractNumber
            // 
            this.lblDispExtractNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDispExtractNumber.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispExtractNumber.DropDown.AllowDrop = false;
            this.lblDispExtractNumber.Enabled = false;
            this.lblDispExtractNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispExtractNumber.HighlightText = true;
            this.lblDispExtractNumber.Location = new System.Drawing.Point(380, 153);
            this.lblDispExtractNumber.Margin = new System.Windows.Forms.Padding(3, 3, 9, 9);
            this.lblDispExtractNumber.Name = "lblDispExtractNumber";
            this.lblDispExtractNumber.ReadOnly = true;
            this.lblDispExtractNumber.Required = false;
            this.lblDispExtractNumber.Size = new System.Drawing.Size(115, 22);
            this.lblDispExtractNumber.TabIndex = 23;
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
            // lblExtractAmount
            // 
            this.lblExtractAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractAmount.Location = new System.Drawing.Point(510, 128);
            this.lblExtractAmount.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblExtractAmount.Name = "lblExtractAmount";
            this.lblExtractAmount.Size = new System.Drawing.Size(115, 16);
            this.lblExtractAmount.TabIndex = 22;
            this.lblExtractAmount.Text = "連携金額";
            this.lblExtractAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // lblDispExtractAmount
            // 
            this.lblDispExtractAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDispExtractAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispExtractAmount.DropDown.AllowDrop = false;
            this.lblDispExtractAmount.Enabled = false;
            this.lblDispExtractAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispExtractAmount.HighlightText = true;
            this.lblDispExtractAmount.Location = new System.Drawing.Point(513, 153);
            this.lblDispExtractAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.lblDispExtractAmount.Name = "lblDispExtractAmount";
            this.lblDispExtractAmount.ReadOnly = true;
            this.lblDispExtractAmount.Required = false;
            this.lblDispExtractAmount.Size = new System.Drawing.Size(115, 22);
            this.lblDispExtractAmount.TabIndex = 20;
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
            this.btnCurrencyCode.Location = new System.Drawing.Point(262, 93);
            this.btnCurrencyCode.Name = "btnCurrencyCode";
            this.btnCurrencyCode.Size = new System.Drawing.Size(24, 24);
            this.btnCurrencyCode.TabIndex = 36;
            this.btnCurrencyCode.UseVisualStyleBackColor = true;
            // 
            // PE0801
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblExtractNumber);
            this.Controls.Add(this.lblDispExtractNumber);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblExtractAmount);
            this.Controls.Add(this.lblRecordedAt);
            this.Controls.Add(this.lblDispExtractAmount);
            this.Controls.Add(this.lblCurrencyCode);
            this.Controls.Add(this.txtCurrencyCode);
            this.Controls.Add(this.btnCurrencyCode);
            this.Controls.Add(this.datRecordedAtFrom);
            this.Controls.Add(this.datRecordedAtTo);
            this.Controls.Add(this.lblRecordedAtWave);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PE0801";
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datRecordedAtTo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneLabelControl lblExtractNumber;
        private Common.Controls.VOneDispLabelControl lblDispExtractNumber;
        private Common.Controls.VOneGridControl grid;
        private Common.Controls.VOneLabelControl lblExtractAmount;
        private Common.Controls.VOneLabelControl lblRecordedAt;
        private Common.Controls.VOneDispLabelControl lblDispExtractAmount;
        private Common.Controls.VOneLabelControl lblCurrencyCode;
        private Common.Controls.VOneTextControl txtCurrencyCode;
        private System.Windows.Forms.Button btnCurrencyCode;
        private Common.Controls.VOneDateControl datRecordedAtFrom;
        private Common.Controls.VOneDateControl datRecordedAtTo;
        private Common.Controls.VOneLabelControl lblRecordedAtWave;
    }
}
