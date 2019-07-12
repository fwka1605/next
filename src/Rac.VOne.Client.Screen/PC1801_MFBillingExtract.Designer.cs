namespace Rac.VOne.Client.Screen
{
    partial class PC1801
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
            this.lblBilledAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datBilledAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblSalesAtWave = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datBilledAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblExtractionAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.vOneLabelControl1 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtSelectedCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.datBilledAtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datBilledAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSelectedCount)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBilledAt
            // 
            this.lblBilledAt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBilledAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBilledAt.Location = new System.Drawing.Point(28, 18);
            this.lblBilledAt.Name = "lblBilledAt";
            this.lblBilledAt.Size = new System.Drawing.Size(48, 16);
            this.lblBilledAt.TabIndex = 3;
            this.lblBilledAt.Text = "請求日";
            this.lblBilledAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datBilledAtTo
            // 
            this.datBilledAtTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datBilledAtTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datBilledAtTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.datBilledAtTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datBilledAtTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datBilledAtTo.Location = new System.Drawing.Point(225, 16);
            this.datBilledAtTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.datBilledAtTo.Name = "datBilledAtTo";
            this.datBilledAtTo.Required = false;
            this.datBilledAtTo.Size = new System.Drawing.Size(115, 22);
            this.datBilledAtTo.Spin.AllowSpin = false;
            this.datBilledAtTo.TabIndex = 7;
            this.datBilledAtTo.Value = null;
            // 
            // lblSalesAtWave
            // 
            this.lblSalesAtWave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSalesAtWave.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSalesAtWave.Location = new System.Drawing.Point(197, 18);
            this.lblSalesAtWave.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblSalesAtWave.Name = "lblSalesAtWave";
            this.lblSalesAtWave.Size = new System.Drawing.Size(20, 16);
            this.lblSalesAtWave.TabIndex = 5;
            this.lblSalesAtWave.Text = "～";
            this.lblSalesAtWave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // datBilledAtFrom
            // 
            this.datBilledAtFrom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datBilledAtFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datBilledAtFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.datBilledAtFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datBilledAtFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datBilledAtFrom.Location = new System.Drawing.Point(74, 16);
            this.datBilledAtFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.datBilledAtFrom.Name = "datBilledAtFrom";
            this.datBilledAtFrom.Required = false;
            this.datBilledAtFrom.Size = new System.Drawing.Size(115, 22);
            this.datBilledAtFrom.Spin.AllowSpin = false;
            this.datBilledAtFrom.TabIndex = 6;
            this.datBilledAtFrom.Value = null;
            // 
            // lblExtractionAmount
            // 
            this.lblExtractionAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExtractionAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractionAmount.Location = new System.Drawing.Point(793, 590);
            this.lblExtractionAmount.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblExtractionAmount.Name = "lblExtractionAmount";
            this.lblExtractionAmount.Size = new System.Drawing.Size(88, 16);
            this.lblExtractionAmount.TabIndex = 10;
            this.lblExtractionAmount.Text = "合計金額";
            this.lblExtractionAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExtractAmount
            // 
            this.lblExtractAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExtractAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblExtractAmount.DropDown.AllowDrop = false;
            this.lblExtractAmount.Enabled = false;
            this.lblExtractAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractAmount.HighlightText = true;
            this.lblExtractAmount.Location = new System.Drawing.Point(887, 588);
            this.lblExtractAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.lblExtractAmount.Name = "lblExtractAmount";
            this.lblExtractAmount.ReadOnly = true;
            this.lblExtractAmount.Required = false;
            this.lblExtractAmount.Size = new System.Drawing.Size(106, 22);
            this.lblExtractAmount.TabIndex = 11;
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToResize = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(15, 56);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(978, 525);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 16;
            // 
            // vOneLabelControl1
            // 
            this.vOneLabelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.vOneLabelControl1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vOneLabelControl1.Location = new System.Drawing.Point(609, 590);
            this.vOneLabelControl1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.vOneLabelControl1.Name = "vOneLabelControl1";
            this.vOneLabelControl1.Size = new System.Drawing.Size(65, 16);
            this.vOneLabelControl1.TabIndex = 46;
            this.vOneLabelControl1.Text = "選択件数";
            this.vOneLabelControl1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSelectedCount
            // 
            this.txtSelectedCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelectedCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.txtSelectedCount.DropDown.AllowDrop = false;
            this.txtSelectedCount.Enabled = false;
            this.txtSelectedCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSelectedCount.HighlightText = true;
            this.txtSelectedCount.Location = new System.Drawing.Point(680, 588);
            this.txtSelectedCount.Margin = new System.Windows.Forms.Padding(3, 3, 9, 9);
            this.txtSelectedCount.Name = "txtSelectedCount";
            this.txtSelectedCount.ReadOnly = true;
            this.txtSelectedCount.Required = false;
            this.txtSelectedCount.Size = new System.Drawing.Size(115, 22);
            this.txtSelectedCount.TabIndex = 47;
            // 
            // PC1801
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vOneLabelControl1);
            this.Controls.Add(this.txtSelectedCount);
            this.Controls.Add(this.datBilledAtFrom);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblExtractionAmount);
            this.Controls.Add(this.lblExtractAmount);
            this.Controls.Add(this.datBilledAtTo);
            this.Controls.Add(this.lblSalesAtWave);
            this.Controls.Add(this.lblBilledAt);
            this.Name = "PC1801";
            ((System.ComponentModel.ISupportInitialize)(this.datBilledAtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datBilledAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExtractAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSelectedCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneLabelControl lblBilledAt;
        private Common.Controls.VOneDateControl datBilledAtTo;
        private Common.Controls.VOneLabelControl lblSalesAtWave;
        private Common.Controls.VOneDateControl datBilledAtFrom;
        private Common.Controls.VOneLabelControl lblExtractionAmount;
        private Common.Controls.VOneDispLabelControl lblExtractAmount;
        private Common.Controls.VOneGridControl grid;
        private Common.Controls.VOneLabelControl vOneLabelControl1;
        private Common.Controls.VOneDispLabelControl txtSelectedCount;
    }
}
