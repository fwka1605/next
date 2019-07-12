namespace Rac.VOne.Client.Screen
{
    partial class PD0103
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
            this.grpInputForm = new System.Windows.Forms.GroupBox();
            this.cmbAccountType = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.txtPayerCode2 = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtPayerCode1 = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtBranchCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtBankCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblPayerCode2 = new System.Windows.Forms.Label();
            this.lblAccountType = new System.Windows.Forms.Label();
            this.lblPayerCode1 = new System.Windows.Forms.Label();
            this.lblBranchCode = new System.Windows.Forms.Label();
            this.lblBankCode = new System.Windows.Forms.Label();
            this.grdSettings = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.grpInputForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccountType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerCode2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerCode1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // grpInputForm
            // 
            this.grpInputForm.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpInputForm.Controls.Add(this.cmbAccountType);
            this.grpInputForm.Controls.Add(this.txtPayerCode2);
            this.grpInputForm.Controls.Add(this.txtPayerCode1);
            this.grpInputForm.Controls.Add(this.txtBranchCode);
            this.grpInputForm.Controls.Add(this.txtBankCode);
            this.grpInputForm.Controls.Add(this.lblPayerCode2);
            this.grpInputForm.Controls.Add(this.lblAccountType);
            this.grpInputForm.Controls.Add(this.lblPayerCode1);
            this.grpInputForm.Controls.Add(this.lblBranchCode);
            this.grpInputForm.Controls.Add(this.lblBankCode);
            this.grpInputForm.Location = new System.Drawing.Point(194, 501);
            this.grpInputForm.Name = "grpInputForm";
            this.grpInputForm.Size = new System.Drawing.Size(621, 105);
            this.grpInputForm.TabIndex = 1;
            this.grpInputForm.TabStop = false;
            // 
            // cmbAccountType
            // 
            this.cmbAccountType.DisplayMember = null;
            this.cmbAccountType.DropDown.AllowResize = false;
            this.cmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountType.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbAccountType.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbAccountType.ListHeaderPane.Height = 22;
            this.cmbAccountType.ListHeaderPane.Visible = false;
            this.cmbAccountType.Location = new System.Drawing.Point(479, 29);
            this.cmbAccountType.Name = "cmbAccountType";
            this.cmbAccountType.Required = false;
            this.cmbAccountType.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbAccountType.Size = new System.Drawing.Size(100, 22);
            this.cmbAccountType.TabIndex = 3;
            this.cmbAccountType.ValueMember = null;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // txtPayerCode2
            // 
            this.txtPayerCode2.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtPayerCode2.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPayerCode2.DropDown.AllowDrop = false;
            this.txtPayerCode2.Format = "9";
            this.txtPayerCode2.HighlightText = true;
            this.txtPayerCode2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPayerCode2.Location = new System.Drawing.Point(479, 62);
            this.txtPayerCode2.MaxLength = 7;
            this.txtPayerCode2.Name = "txtPayerCode2";
            this.txtPayerCode2.PaddingChar = '0';
            this.txtPayerCode2.Required = true;
            this.txtPayerCode2.Size = new System.Drawing.Size(100, 22);
            this.txtPayerCode2.TabIndex = 5;
            // 
            // txtPayerCode1
            // 
            this.txtPayerCode1.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtPayerCode1.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPayerCode1.DropDown.AllowDrop = false;
            this.txtPayerCode1.Format = "9";
            this.txtPayerCode1.HighlightText = true;
            this.txtPayerCode1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPayerCode1.Location = new System.Drawing.Point(294, 62);
            this.txtPayerCode1.MaxLength = 3;
            this.txtPayerCode1.Name = "txtPayerCode1";
            this.txtPayerCode1.PaddingChar = '0';
            this.txtPayerCode1.Required = true;
            this.txtPayerCode1.Size = new System.Drawing.Size(60, 22);
            this.txtPayerCode1.TabIndex = 4;
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtBranchCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBranchCode.DropDown.AllowDrop = false;
            this.txtBranchCode.Format = "9";
            this.txtBranchCode.HighlightText = true;
            this.txtBranchCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBranchCode.Location = new System.Drawing.Point(294, 29);
            this.txtBranchCode.MaxLength = 3;
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.PaddingChar = '0';
            this.txtBranchCode.Required = true;
            this.txtBranchCode.Size = new System.Drawing.Size(60, 22);
            this.txtBranchCode.TabIndex = 2;
            // 
            // txtBankCode
            // 
            this.txtBankCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtBankCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBankCode.DropDown.AllowDrop = false;
            this.txtBankCode.Format = "9";
            this.txtBankCode.HighlightText = true;
            this.txtBankCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBankCode.Location = new System.Drawing.Point(104, 29);
            this.txtBankCode.MaxLength = 4;
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.PaddingChar = '0';
            this.txtBankCode.Required = true;
            this.txtBankCode.Size = new System.Drawing.Size(60, 22);
            this.txtBankCode.TabIndex = 1;
            // 
            // lblPayerCode2
            // 
            this.lblPayerCode2.AutoSize = true;
            this.lblPayerCode2.Location = new System.Drawing.Point(396, 65);
            this.lblPayerCode2.Name = "lblPayerCode2";
            this.lblPayerCode2.Size = new System.Drawing.Size(79, 15);
            this.lblPayerCode2.TabIndex = 0;
            this.lblPayerCode2.Text = "仮想口座番号";
            // 
            // lblAccountType
            // 
            this.lblAccountType.AutoSize = true;
            this.lblAccountType.Location = new System.Drawing.Point(396, 32);
            this.lblAccountType.Name = "lblAccountType";
            this.lblAccountType.Size = new System.Drawing.Size(55, 15);
            this.lblAccountType.TabIndex = 0;
            this.lblAccountType.Text = "預金種別";
            // 
            // lblPayerCode1
            // 
            this.lblPayerCode1.AutoSize = true;
            this.lblPayerCode1.Location = new System.Drawing.Point(206, 65);
            this.lblPayerCode1.Name = "lblPayerCode1";
            this.lblPayerCode1.Size = new System.Drawing.Size(81, 15);
            this.lblPayerCode1.TabIndex = 0;
            this.lblPayerCode1.Text = "仮想支店コード";
            // 
            // lblBranchCode
            // 
            this.lblBranchCode.AutoSize = true;
            this.lblBranchCode.Location = new System.Drawing.Point(206, 32);
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Size = new System.Drawing.Size(57, 15);
            this.lblBranchCode.TabIndex = 0;
            this.lblBranchCode.Text = "支店コード";
            // 
            // lblBankCode
            // 
            this.lblBankCode.AutoSize = true;
            this.lblBankCode.Location = new System.Drawing.Point(40, 32);
            this.lblBankCode.Name = "lblBankCode";
            this.lblBankCode.Size = new System.Drawing.Size(57, 15);
            this.lblBankCode.TabIndex = 0;
            this.lblBankCode.Text = "銀行コード";
            // 
            // grdSettings
            // 
            this.grdSettings.AllowAutoExtend = true;
            this.grdSettings.AllowUserToAddRows = false;
            this.grdSettings.AllowUserToShiftSelect = true;
            this.grdSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdSettings.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdSettings.Location = new System.Drawing.Point(194, 15);
            this.grdSettings.Name = "grdSettings";
            this.grdSettings.Size = new System.Drawing.Size(621, 480);
            this.grdSettings.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdSettings.TabIndex = 0;
            this.grdSettings.CellContentDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdSettings_CellContentDoubleClick);
            // 
            // PD0103
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpInputForm);
            this.Controls.Add(this.grdSettings);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PD0103";
            this.Load += new System.EventHandler(this.PD0103_Load);
            this.grpInputForm.ResumeLayout(false);
            this.grpInputForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccountType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerCode2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerCode1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBranchCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneGridControl grdSettings;
        private System.Windows.Forms.GroupBox grpInputForm;
        private Common.Controls.VOneComboControl cmbAccountType;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneTextControl txtPayerCode2;
        private Common.Controls.VOneTextControl txtPayerCode1;
        private Common.Controls.VOneTextControl txtBranchCode;
        private Common.Controls.VOneTextControl txtBankCode;
        private System.Windows.Forms.Label lblPayerCode2;
        private System.Windows.Forms.Label lblAccountType;
        private System.Windows.Forms.Label lblPayerCode1;
        private System.Windows.Forms.Label lblBranchCode;
        private System.Windows.Forms.Label lblBankCode;
    }
}
