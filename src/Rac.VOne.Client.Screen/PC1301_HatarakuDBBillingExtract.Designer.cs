namespace Rac.VOne.Client.Screen
{
    partial class PC1301
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
            this.gbxBillingFreeImporter = new System.Windows.Forms.GroupBox();
            this.lblPattern = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtPatternCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblPatternName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.gbxPrint = new System.Windows.Forms.GroupBox();
            this.rdoPrintPossible = new System.Windows.Forms.RadioButton();
            this.rdoPrintImpossible = new System.Windows.Forms.RadioButton();
            this.lblExtractCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDispExtractCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblValidCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDispValidCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblInvalidCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDispInvalidCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblNewCustomerCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDispNewCustomerCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblImportCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDispImportCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblImportAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDispImportAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.gbxBillingFreeImporter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPatternName)).BeginInit();
            this.gbxPrint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispValidCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispInvalidCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispNewCustomerCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispImportCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispImportAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxBillingFreeImporter
            // 
            this.gbxBillingFreeImporter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbxBillingFreeImporter.Controls.Add(this.lblPattern);
            this.gbxBillingFreeImporter.Controls.Add(this.txtPatternCode);
            this.gbxBillingFreeImporter.Controls.Add(this.lblPatternName);
            this.gbxBillingFreeImporter.Controls.Add(this.gbxPrint);
            this.gbxBillingFreeImporter.Controls.Add(this.lblExtractCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblDispExtractCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblValidCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblDispValidCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblInvalidCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblDispInvalidCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblNewCustomerCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblDispNewCustomerCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblImportCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblDispImportCount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblImportAmount);
            this.gbxBillingFreeImporter.Controls.Add(this.lblDispImportAmount);
            this.gbxBillingFreeImporter.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxBillingFreeImporter.Location = new System.Drawing.Point(36, 36);
            this.gbxBillingFreeImporter.Margin = new System.Windows.Forms.Padding(24);
            this.gbxBillingFreeImporter.Name = "gbxBillingFreeImporter";
            this.gbxBillingFreeImporter.Size = new System.Drawing.Size(936, 549);
            this.gbxBillingFreeImporter.TabIndex = 0;
            this.gbxBillingFreeImporter.TabStop = false;
            this.gbxBillingFreeImporter.Text = "□　取込指定";
            // 
            // lblPattern
            // 
            this.lblPattern.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPattern.Location = new System.Drawing.Point(41, 86);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(77, 16);
            this.lblPattern.TabIndex = 0;
            this.lblPattern.Text = "パターンNo.";
            this.lblPattern.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPatternCode
            // 
            this.txtPatternCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtPatternCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPatternCode.DropDown.AllowDrop = false;
            this.txtPatternCode.Enabled = false;
            this.txtPatternCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatternCode.Format = "9";
            this.txtPatternCode.HighlightText = true;
            this.txtPatternCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPatternCode.Location = new System.Drawing.Point(124, 84);
            this.txtPatternCode.MaxLength = 2;
            this.txtPatternCode.Name = "txtPatternCode";
            this.txtPatternCode.Required = true;
            this.txtPatternCode.Size = new System.Drawing.Size(60, 22);
            this.txtPatternCode.TabIndex = 1;
            // 
            // lblPatternName
            // 
            this.lblPatternName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPatternName.DropDown.AllowDrop = false;
            this.lblPatternName.Enabled = false;
            this.lblPatternName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatternName.HighlightText = true;
            this.lblPatternName.Location = new System.Drawing.Point(190, 84);
            this.lblPatternName.Name = "lblPatternName";
            this.lblPatternName.ReadOnly = true;
            this.lblPatternName.Required = false;
            this.lblPatternName.Size = new System.Drawing.Size(680, 22);
            this.lblPatternName.TabIndex = 3;
            // 
            // gbxPrint
            // 
            this.gbxPrint.Controls.Add(this.rdoPrintPossible);
            this.gbxPrint.Controls.Add(this.rdoPrintImpossible);
            this.gbxPrint.Enabled = false;
            this.gbxPrint.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxPrint.Location = new System.Drawing.Point(124, 201);
            this.gbxPrint.Name = "gbxPrint";
            this.gbxPrint.Size = new System.Drawing.Size(746, 54);
            this.gbxPrint.TabIndex = 4;
            this.gbxPrint.TabStop = false;
            this.gbxPrint.Text = "印刷対象";
            // 
            // rdoPrintPossible
            // 
            this.rdoPrintPossible.Checked = true;
            this.rdoPrintPossible.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPrintPossible.Location = new System.Drawing.Point(66, 22);
            this.rdoPrintPossible.Margin = new System.Windows.Forms.Padding(3, 3, 21, 3);
            this.rdoPrintPossible.Name = "rdoPrintPossible";
            this.rdoPrintPossible.Size = new System.Drawing.Size(86, 18);
            this.rdoPrintPossible.TabIndex = 0;
            this.rdoPrintPossible.TabStop = true;
            this.rdoPrintPossible.Text = "取込可能";
            this.rdoPrintPossible.UseVisualStyleBackColor = true;
            // 
            // rdoPrintImpossible
            // 
            this.rdoPrintImpossible.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPrintImpossible.Location = new System.Drawing.Point(194, 22);
            this.rdoPrintImpossible.Margin = new System.Windows.Forms.Padding(21, 3, 3, 3);
            this.rdoPrintImpossible.Name = "rdoPrintImpossible";
            this.rdoPrintImpossible.Size = new System.Drawing.Size(86, 18);
            this.rdoPrintImpossible.TabIndex = 1;
            this.rdoPrintImpossible.Text = "取込不可能";
            this.rdoPrintImpossible.UseVisualStyleBackColor = true;
            // 
            // lblExtractCount
            // 
            this.lblExtractCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractCount.Location = new System.Drawing.Point(123, 292);
            this.lblExtractCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblExtractCount.Name = "lblExtractCount";
            this.lblExtractCount.Size = new System.Drawing.Size(115, 16);
            this.lblExtractCount.TabIndex = 0;
            this.lblExtractCount.Text = "抽出件数";
            this.lblExtractCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDispExtractCount
            // 
            this.lblDispExtractCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispExtractCount.DropDown.AllowDrop = false;
            this.lblDispExtractCount.Enabled = false;
            this.lblDispExtractCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispExtractCount.HighlightText = true;
            this.lblDispExtractCount.Location = new System.Drawing.Point(123, 316);
            this.lblDispExtractCount.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.lblDispExtractCount.Name = "lblDispExtractCount";
            this.lblDispExtractCount.ReadOnly = true;
            this.lblDispExtractCount.Required = false;
            this.lblDispExtractCount.Size = new System.Drawing.Size(115, 22);
            this.lblDispExtractCount.TabIndex = 1;
            // 
            // lblValidCount
            // 
            this.lblValidCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValidCount.Location = new System.Drawing.Point(250, 292);
            this.lblValidCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblValidCount.Name = "lblValidCount";
            this.lblValidCount.Size = new System.Drawing.Size(115, 16);
            this.lblValidCount.TabIndex = 0;
            this.lblValidCount.Text = "取込可能件数";
            this.lblValidCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDispValidCount
            // 
            this.lblDispValidCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispValidCount.DropDown.AllowDrop = false;
            this.lblDispValidCount.Enabled = false;
            this.lblDispValidCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispValidCount.HighlightText = true;
            this.lblDispValidCount.Location = new System.Drawing.Point(250, 316);
            this.lblDispValidCount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblDispValidCount.Name = "lblDispValidCount";
            this.lblDispValidCount.ReadOnly = true;
            this.lblDispValidCount.Required = false;
            this.lblDispValidCount.Size = new System.Drawing.Size(115, 22);
            this.lblDispValidCount.TabIndex = 2;
            // 
            // lblInvalidCount
            // 
            this.lblInvalidCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvalidCount.Location = new System.Drawing.Point(377, 292);
            this.lblInvalidCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblInvalidCount.Name = "lblInvalidCount";
            this.lblInvalidCount.Size = new System.Drawing.Size(115, 16);
            this.lblInvalidCount.TabIndex = 0;
            this.lblInvalidCount.Text = "取込不可能件数";
            this.lblInvalidCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDispInvalidCount
            // 
            this.lblDispInvalidCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispInvalidCount.DropDown.AllowDrop = false;
            this.lblDispInvalidCount.Enabled = false;
            this.lblDispInvalidCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispInvalidCount.HighlightText = true;
            this.lblDispInvalidCount.Location = new System.Drawing.Point(377, 316);
            this.lblDispInvalidCount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblDispInvalidCount.Name = "lblDispInvalidCount";
            this.lblDispInvalidCount.ReadOnly = true;
            this.lblDispInvalidCount.Required = false;
            this.lblDispInvalidCount.Size = new System.Drawing.Size(115, 22);
            this.lblDispInvalidCount.TabIndex = 3;
            // 
            // lblNewCustomerCount
            // 
            this.lblNewCustomerCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewCustomerCount.Location = new System.Drawing.Point(504, 292);
            this.lblNewCustomerCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblNewCustomerCount.Name = "lblNewCustomerCount";
            this.lblNewCustomerCount.Size = new System.Drawing.Size(115, 16);
            this.lblNewCustomerCount.TabIndex = 0;
            this.lblNewCustomerCount.Text = "新規得意先件数";
            this.lblNewCustomerCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDispNewCustomerCount
            // 
            this.lblDispNewCustomerCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispNewCustomerCount.DropDown.AllowDrop = false;
            this.lblDispNewCustomerCount.Enabled = false;
            this.lblDispNewCustomerCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispNewCustomerCount.HighlightText = true;
            this.lblDispNewCustomerCount.Location = new System.Drawing.Point(504, 316);
            this.lblDispNewCustomerCount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblDispNewCustomerCount.Name = "lblDispNewCustomerCount";
            this.lblDispNewCustomerCount.ReadOnly = true;
            this.lblDispNewCustomerCount.Required = false;
            this.lblDispNewCustomerCount.Size = new System.Drawing.Size(115, 22);
            this.lblDispNewCustomerCount.TabIndex = 4;
            // 
            // lblImportCount
            // 
            this.lblImportCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportCount.Location = new System.Drawing.Point(631, 292);
            this.lblImportCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblImportCount.Name = "lblImportCount";
            this.lblImportCount.Size = new System.Drawing.Size(115, 16);
            this.lblImportCount.TabIndex = 0;
            this.lblImportCount.Text = "取込件数";
            this.lblImportCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDispImportCount
            // 
            this.lblDispImportCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispImportCount.DropDown.AllowDrop = false;
            this.lblDispImportCount.Enabled = false;
            this.lblDispImportCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispImportCount.HighlightText = true;
            this.lblDispImportCount.Location = new System.Drawing.Point(631, 316);
            this.lblDispImportCount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblDispImportCount.Name = "lblDispImportCount";
            this.lblDispImportCount.ReadOnly = true;
            this.lblDispImportCount.Required = false;
            this.lblDispImportCount.Size = new System.Drawing.Size(115, 22);
            this.lblDispImportCount.TabIndex = 5;
            // 
            // lblImportAmount
            // 
            this.lblImportAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImportAmount.Location = new System.Drawing.Point(758, 292);
            this.lblImportAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblImportAmount.Name = "lblImportAmount";
            this.lblImportAmount.Size = new System.Drawing.Size(115, 16);
            this.lblImportAmount.TabIndex = 0;
            this.lblImportAmount.Text = "取込金額";
            this.lblImportAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDispImportAmount
            // 
            this.lblDispImportAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDispImportAmount.DropDown.AllowDrop = false;
            this.lblDispImportAmount.Enabled = false;
            this.lblDispImportAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispImportAmount.HighlightText = true;
            this.lblDispImportAmount.Location = new System.Drawing.Point(758, 316);
            this.lblDispImportAmount.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.lblDispImportAmount.Name = "lblDispImportAmount";
            this.lblDispImportAmount.ReadOnly = true;
            this.lblDispImportAmount.Required = false;
            this.lblDispImportAmount.Size = new System.Drawing.Size(115, 22);
            this.lblDispImportAmount.TabIndex = 6;
            // 
            // PC1301
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxBillingFreeImporter);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PC1301";
            this.gbxBillingFreeImporter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPatternName)).EndInit();
            this.gbxPrint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblDispExtractCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispValidCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispInvalidCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispNewCustomerCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispImportCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDispImportAmount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxBillingFreeImporter;
        private Common.Controls.VOneLabelControl lblExtractCount;
        private Common.Controls.VOneDispLabelControl lblPatternName;
        private Common.Controls.VOneLabelControl lblValidCount;
        private Common.Controls.VOneLabelControl lblPattern;
        private Common.Controls.VOneLabelControl lblNewCustomerCount;
        private Common.Controls.VOneLabelControl lblInvalidCount;
        private System.Windows.Forms.GroupBox gbxPrint;
        private System.Windows.Forms.RadioButton rdoPrintImpossible;
        private System.Windows.Forms.RadioButton rdoPrintPossible;
        private Common.Controls.VOneLabelControl lblImportCount;
        private Common.Controls.VOneLabelControl lblImportAmount;
        private Common.Controls.VOneDispLabelControl lblDispExtractCount;
        private Common.Controls.VOneDispLabelControl lblDispValidCount;
        private Common.Controls.VOneDispLabelControl lblDispNewCustomerCount;
        private Common.Controls.VOneTextControl txtPatternCode;
        private Common.Controls.VOneDispLabelControl lblDispInvalidCount;
        private Common.Controls.VOneDispLabelControl lblDispImportCount;
        private Common.Controls.VOneDispLabelControl lblDispImportAmount;
    }
}
