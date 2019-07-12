namespace Rac.VOne.Client.Screen
{
    partial class PH9907
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
            Rac.VOne.Client.Common.Controls.VOneLabelControl lblF04Description;
            Rac.VOne.Client.Common.Controls.VOneLabelControl lblDescription;
            GrapeCity.Win.Editors.ListItem listItem1 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem1 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.ListItem listItem2 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem2 = new GrapeCity.Win.Editors.SubItem();
            this.cbxOutputLogFile = new System.Windows.Forms.CheckBox();
            this.cmbLogFilePath = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblF06Description = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblF08Description = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pbxIcon = new System.Windows.Forms.PictureBox();
            lblF04Description = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            lblDescription = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLogFilePath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblF04Description
            // 
            lblF04Description.Location = new System.Drawing.Point(53, 64);
            lblF04Description.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            lblF04Description.Name = "lblF04Description";
            lblF04Description.Size = new System.Drawing.Size(359, 16);
            lblF04Description.TabIndex = 1;
            lblF04Description.Text = "上書（F4）：マスターを削除し、インポートファイルの内容に置き換えます。";
            lblF04Description.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescription
            // 
            lblDescription.Location = new System.Drawing.Point(53, 28);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new System.Drawing.Size(169, 16);
            lblDescription.TabIndex = 0;
            lblDescription.Text = "インポート方法を選択してください。";
            lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxOutputLogFile
            // 
            this.cbxOutputLogFile.Location = new System.Drawing.Point(56, 142);
            this.cbxOutputLogFile.Name = "cbxOutputLogFile";
            this.cbxOutputLogFile.Size = new System.Drawing.Size(129, 18);
            this.cbxOutputLogFile.TabIndex = 0;
            this.cbxOutputLogFile.Text = "取込エラーログの出力";
            this.cbxOutputLogFile.UseVisualStyleBackColor = true;
            // 
            // cmbLogFilePath
            // 
            this.cmbLogFilePath.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbLogFilePath.DisplayMember = null;
            this.cmbLogFilePath.DropDown.AllowResize = false;
            this.cmbLogFilePath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogFilePath.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbLogFilePath.ImeMode = System.Windows.Forms.ImeMode.Disable;
            subItem1.Value = "ユーザーフォルダー";
            listItem1.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem1});
            subItem2.Value = "取込ファイルと同一フォルダー";
            listItem2.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem2});
            this.cmbLogFilePath.Items.AddRange(new GrapeCity.Win.Editors.ListItem[] {
            listItem1,
            listItem2});
            this.cmbLogFilePath.ListHeaderPane.Height = 22;
            this.cmbLogFilePath.ListHeaderPane.Visible = false;
            this.cmbLogFilePath.Location = new System.Drawing.Point(191, 140);
            this.cmbLogFilePath.Name = "cmbLogFilePath";
            this.cmbLogFilePath.Required = false;
            this.cmbLogFilePath.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbLogFilePath.Size = new System.Drawing.Size(198, 22);
            this.cmbLogFilePath.TabIndex = 1;
            this.cmbLogFilePath.ValueMember = null;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // lblF06Description
            // 
            this.lblF06Description.Location = new System.Drawing.Point(53, 88);
            this.lblF06Description.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblF06Description.Name = "lblF06Description";
            this.lblF06Description.Size = new System.Drawing.Size(359, 16);
            this.lblF06Description.TabIndex = 2;
            this.lblF06Description.Text = "追加（F6）：マスターに無いものだけ、インポートファイルから追加します。";
            this.lblF06Description.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblF08Description
            // 
            this.lblF08Description.Location = new System.Drawing.Point(53, 112);
            this.lblF08Description.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblF08Description.Name = "lblF08Description";
            this.lblF08Description.Size = new System.Drawing.Size(359, 16);
            this.lblF08Description.TabIndex = 3;
            this.lblF08Description.Text = "更新（F8）：マスターに無いものは追加、あるものは上書きします。";
            this.lblF08Description.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbxIcon
            // 
            this.pbxIcon.Location = new System.Drawing.Point(15, 19);
            this.pbxIcon.Name = "pbxIcon";
            this.pbxIcon.Size = new System.Drawing.Size(32, 32);
            this.pbxIcon.TabIndex = 1;
            this.pbxIcon.TabStop = false;
            // 
            // PH9907
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.cbxOutputLogFile);
            this.Controls.Add(this.cmbLogFilePath);
            this.Controls.Add(lblDescription);
            this.Controls.Add(lblF04Description);
            this.Controls.Add(this.lblF06Description);
            this.Controls.Add(this.pbxIcon);
            this.Controls.Add(this.lblF08Description);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PH9907";
            this.Size = new System.Drawing.Size(500, 280);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLogFilePath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox cbxOutputLogFile;
        private Common.Controls.VOneComboControl cmbLogFilePath;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneLabelControl lblF06Description;
        private Common.Controls.VOneLabelControl lblF08Description;
        private System.Windows.Forms.PictureBox pbxIcon;
    }
}
