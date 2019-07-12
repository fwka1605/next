namespace Rac.VOne.Client.Screen
{
    partial class PH1302
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
            this.gbxEBFileFormat = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoUseValue1 = new System.Windows.Forms.RadioButton();
            this.rdoUseValue0 = new System.Windows.Forms.RadioButton();
            this.lblUseValueDate = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnPath = new System.Windows.Forms.Button();
            this.cmbFileFieldType = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dbFileFieldType = new GrapeCity.Win.Editors.DropDownButton();
            this.lblEBFileFormatId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtBankCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblFIleFieldType = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cmbEBFileFormatId = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dbEBFileFormatId = new GrapeCity.Win.Editors.DropDownButton();
            this.txtImportableValue = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblDisplayOrder = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtInitialDirectory = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtDisplayOrder = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.lblImportableValue = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblInitialDirectory = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblBankCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxEBFileFormat.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFileFieldType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEBFileFormatId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImportableValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialDirectory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxEBFileFormat
            // 
            this.gbxEBFileFormat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxEBFileFormat.Controls.Add(this.panel1);
            this.gbxEBFileFormat.Controls.Add(this.btnPath);
            this.gbxEBFileFormat.Controls.Add(this.cmbFileFieldType);
            this.gbxEBFileFormat.Controls.Add(this.lblEBFileFormatId);
            this.gbxEBFileFormat.Controls.Add(this.txtBankCode);
            this.gbxEBFileFormat.Controls.Add(this.lblFIleFieldType);
            this.gbxEBFileFormat.Controls.Add(this.cmbEBFileFormatId);
            this.gbxEBFileFormat.Controls.Add(this.txtImportableValue);
            this.gbxEBFileFormat.Controls.Add(this.lblDisplayOrder);
            this.gbxEBFileFormat.Controls.Add(this.lblName);
            this.gbxEBFileFormat.Controls.Add(this.txtInitialDirectory);
            this.gbxEBFileFormat.Controls.Add(this.txtDisplayOrder);
            this.gbxEBFileFormat.Controls.Add(this.lblImportableValue);
            this.gbxEBFileFormat.Controls.Add(this.txtName);
            this.gbxEBFileFormat.Controls.Add(this.lblInitialDirectory);
            this.gbxEBFileFormat.Controls.Add(this.lblBankCode);
            this.gbxEBFileFormat.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxEBFileFormat.Location = new System.Drawing.Point(32, 161);
            this.gbxEBFileFormat.Name = "gbxEBFileFormat";
            this.gbxEBFileFormat.Size = new System.Drawing.Size(943, 301);
            this.gbxEBFileFormat.TabIndex = 0;
            this.gbxEBFileFormat.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoUseValue1);
            this.panel1.Controls.Add(this.rdoUseValue0);
            this.panel1.Controls.Add(this.lblUseValueDate);
            this.panel1.Location = new System.Drawing.Point(25, 237);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(570, 33);
            this.panel1.TabIndex = 10;
            this.panel1.Visible = false;
            // 
            // rdoUseValue1
            // 
            this.rdoUseValue1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdoUseValue1.Location = new System.Drawing.Point(200, 3);
            this.rdoUseValue1.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.rdoUseValue1.Name = "rdoUseValue1";
            this.rdoUseValue1.Size = new System.Drawing.Size(62, 18);
            this.rdoUseValue1.TabIndex = 1;
            this.rdoUseValue1.TabStop = true;
            this.rdoUseValue1.Text = "起算日";
            this.rdoUseValue1.UseVisualStyleBackColor = true;
            // 
            // rdoUseValue0
            // 
            this.rdoUseValue0.Checked = true;
            this.rdoUseValue0.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdoUseValue0.Location = new System.Drawing.Point(120, 3);
            this.rdoUseValue0.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.rdoUseValue0.Name = "rdoUseValue0";
            this.rdoUseValue0.Size = new System.Drawing.Size(62, 18);
            this.rdoUseValue0.TabIndex = 0;
            this.rdoUseValue0.TabStop = true;
            this.rdoUseValue0.Text = "勘定日";
            this.rdoUseValue0.UseVisualStyleBackColor = true;
            // 
            // lblUseValueDate
            // 
            this.lblUseValueDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUseValueDate.Location = new System.Drawing.Point(8, 5);
            this.lblUseValueDate.Margin = new System.Windows.Forms.Padding(3);
            this.lblUseValueDate.Name = "lblUseValueDate";
            this.lblUseValueDate.Size = new System.Drawing.Size(68, 16);
            this.lblUseValueDate.TabIndex = 0;
            this.lblUseValueDate.Text = "入金日指定";
            // 
            // btnPath
            // 
            this.btnPath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPath.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnPath.Location = new System.Drawing.Point(899, 183);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(24, 24);
            this.btnPath.TabIndex = 6;
            this.btnPath.UseVisualStyleBackColor = true;
            // 
            // cmbFileFieldType
            // 
            this.cmbFileFieldType.DisplayMember = null;
            this.cmbFileFieldType.DropDown.AllowResize = false;
            this.cmbFileFieldType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileFieldType.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbFileFieldType.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbFileFieldType.ListHeaderPane.Height = 22;
            this.cmbFileFieldType.ListHeaderPane.Visible = false;
            this.cmbFileFieldType.Location = new System.Drawing.Point(146, 72);
            this.cmbFileFieldType.Name = "cmbFileFieldType";
            this.cmbFileFieldType.Required = true;
            this.cmbFileFieldType.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dbFileFieldType});
            this.cmbFileFieldType.Size = new System.Drawing.Size(117, 22);
            this.cmbFileFieldType.TabIndex = 1;
            this.cmbFileFieldType.ValueMember = null;
            // 
            // dbFileFieldType
            // 
            this.dbFileFieldType.Name = "dbFileFieldType";
            // 
            // lblEBFileFormatId
            // 
            this.lblEBFileFormatId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblEBFileFormatId.Location = new System.Drawing.Point(33, 47);
            this.lblEBFileFormatId.Name = "lblEBFileFormatId";
            this.lblEBFileFormatId.Size = new System.Drawing.Size(107, 19);
            this.lblEBFileFormatId.TabIndex = 0;
            this.lblEBFileFormatId.Text = "EBフォーマット";
            // 
            // txtBankCode
            // 
            this.txtBankCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtBankCode.DropDown.AllowDrop = false;
            this.txtBankCode.Enabled = false;
            this.txtBankCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBankCode.Format = "9";
            this.txtBankCode.HighlightText = true;
            this.txtBankCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBankCode.Location = new System.Drawing.Point(146, 128);
            this.txtBankCode.MaxLength = 4;
            this.txtBankCode.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.PaddingChar = '0';
            this.txtBankCode.Required = true;
            this.txtBankCode.Size = new System.Drawing.Size(60, 22);
            this.txtBankCode.TabIndex = 3;
            // 
            // lblFIleFieldType
            // 
            this.lblFIleFieldType.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFIleFieldType.Location = new System.Drawing.Point(33, 75);
            this.lblFIleFieldType.Name = "lblFIleFieldType";
            this.lblFIleFieldType.Size = new System.Drawing.Size(81, 16);
            this.lblFIleFieldType.TabIndex = 0;
            this.lblFIleFieldType.Text = "区切文字";
            // 
            // cmbEBFileFormatId
            // 
            this.cmbEBFileFormatId.DisplayMember = null;
            this.cmbEBFileFormatId.DropDown.AllowResize = false;
            this.cmbEBFileFormatId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEBFileFormatId.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbEBFileFormatId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbEBFileFormatId.ListHeaderPane.Height = 22;
            this.cmbEBFileFormatId.ListHeaderPane.Visible = false;
            this.cmbEBFileFormatId.Location = new System.Drawing.Point(146, 44);
            this.cmbEBFileFormatId.Name = "cmbEBFileFormatId";
            this.cmbEBFileFormatId.Required = true;
            this.cmbEBFileFormatId.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dbEBFileFormatId});
            this.cmbEBFileFormatId.Size = new System.Drawing.Size(231, 22);
            this.cmbEBFileFormatId.TabIndex = 0;
            this.cmbEBFileFormatId.ValueMember = null;
            // 
            // dbEBFileFormatId
            // 
            this.dbEBFileFormatId.Name = "dbEBFileFormatId";
            // 
            // txtImportableValue
            // 
            this.txtImportableValue.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtImportableValue.DropDown.AllowDrop = false;
            this.txtImportableValue.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtImportableValue.HighlightText = true;
            this.txtImportableValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtImportableValue.Location = new System.Drawing.Point(146, 156);
            this.txtImportableValue.MaxLength = 100;
            this.txtImportableValue.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.txtImportableValue.Name = "txtImportableValue";
            this.txtImportableValue.Required = false;
            this.txtImportableValue.Size = new System.Drawing.Size(747, 22);
            this.txtImportableValue.TabIndex = 4;
            // 
            // lblDisplayOrder
            // 
            this.lblDisplayOrder.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDisplayOrder.Location = new System.Drawing.Point(33, 215);
            this.lblDisplayOrder.Name = "lblDisplayOrder";
            this.lblDisplayOrder.Size = new System.Drawing.Size(81, 16);
            this.lblDisplayOrder.TabIndex = 0;
            this.lblDisplayOrder.Text = "表示順";
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblName.Location = new System.Drawing.Point(33, 103);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(94, 19);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "EBファイル設定名";
            // 
            // txtInitialDirectory
            // 
            this.txtInitialDirectory.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtInitialDirectory.DropDown.AllowDrop = false;
            this.txtInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtInitialDirectory.HighlightText = true;
            this.txtInitialDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtInitialDirectory.Location = new System.Drawing.Point(146, 184);
            this.txtInitialDirectory.MaxLength = 255;
            this.txtInitialDirectory.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.txtInitialDirectory.Name = "txtInitialDirectory";
            this.txtInitialDirectory.Required = false;
            this.txtInitialDirectory.Size = new System.Drawing.Size(747, 22);
            this.txtInitialDirectory.TabIndex = 5;
            // 
            // txtDisplayOrder
            // 
            this.txtDisplayOrder.AllowDeleteToNull = true;
            this.txtDisplayOrder.DropDown.AllowDrop = false;
            this.txtDisplayOrder.Fields.DecimalPart.MaxDigits = 0;
            this.txtDisplayOrder.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtDisplayOrder.HighlightText = true;
            this.txtDisplayOrder.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtDisplayOrder.Location = new System.Drawing.Point(146, 212);
            this.txtDisplayOrder.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.txtDisplayOrder.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtDisplayOrder.Name = "txtDisplayOrder";
            this.txtDisplayOrder.Required = true;
            this.txtDisplayOrder.Size = new System.Drawing.Size(62, 22);
            this.txtDisplayOrder.TabIndex = 7;
            this.txtDisplayOrder.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // lblImportableValue
            // 
            this.lblImportableValue.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblImportableValue.Location = new System.Drawing.Point(33, 159);
            this.lblImportableValue.Name = "lblImportableValue";
            this.lblImportableValue.Size = new System.Drawing.Size(55, 16);
            this.lblImportableValue.TabIndex = 0;
            this.lblImportableValue.Text = "取込区分";
            // 
            // txtName
            // 
            this.txtName.DropDown.AllowDrop = false;
            this.txtName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtName.HighlightText = true;
            this.txtName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtName.Location = new System.Drawing.Point(146, 100);
            this.txtName.MaxLength = 100;
            this.txtName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.txtName.Name = "txtName";
            this.txtName.Required = true;
            this.txtName.Size = new System.Drawing.Size(747, 22);
            this.txtName.TabIndex = 2;
            // 
            // lblInitialDirectory
            // 
            this.lblInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblInitialDirectory.Location = new System.Drawing.Point(33, 187);
            this.lblInitialDirectory.Name = "lblInitialDirectory";
            this.lblInitialDirectory.Size = new System.Drawing.Size(94, 19);
            this.lblInitialDirectory.TabIndex = 0;
            this.lblInitialDirectory.Text = "取込フォルダ";
            // 
            // lblBankCode
            // 
            this.lblBankCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBankCode.Location = new System.Drawing.Point(33, 131);
            this.lblBankCode.Name = "lblBankCode";
            this.lblBankCode.Size = new System.Drawing.Size(81, 16);
            this.lblBankCode.TabIndex = 0;
            this.lblBankCode.Text = "銀行コード";
            // 
            // PH1302
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxEBFileFormat);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PH1302";
            this.Load += new System.EventHandler(this.PH1302_Load);
            this.gbxEBFileFormat.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbFileFieldType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBankCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEBFileFormatId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImportableValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialDirectory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneLabelControl lblDisplayOrder;
        private Common.Controls.VOneLabelControl lblBankCode;
        private Common.Controls.VOneLabelControl lblName;
        private Common.Controls.VOneLabelControl lblFIleFieldType;
        private Common.Controls.VOneLabelControl lblEBFileFormatId;
        private Common.Controls.VOneNumberControl txtDisplayOrder;
        private Common.Controls.VOneTextControl txtName;
        private Common.Controls.VOneComboControl cmbEBFileFormatId;
        private GrapeCity.Win.Editors.DropDownButton dbEBFileFormatId;
        private Common.Controls.VOneTextControl txtImportableValue;
        private Common.Controls.VOneTextControl txtBankCode;
        private System.Windows.Forms.RadioButton rdoUseValue0;
        private System.Windows.Forms.RadioButton rdoUseValue1;
        private Common.Controls.VOneLabelControl lblUseValueDate;
        private Common.Controls.VOneTextControl txtInitialDirectory;
        private Common.Controls.VOneLabelControl lblImportableValue;
        private Common.Controls.VOneLabelControl lblInitialDirectory;
        private System.Windows.Forms.GroupBox gbxEBFileFormat;
        private Common.Controls.VOneComboControl cmbFileFieldType;
        private GrapeCity.Win.Editors.DropDownButton dbFileFieldType;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.Panel panel1;
    }
}
