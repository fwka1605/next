namespace Rac.VOne.Client.Screen
{
    partial class PC1503
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
            this.gbCompression = new System.Windows.Forms.GroupBox();
            this.nmbMaxSize = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.vOneLabelControl3 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblMaxSize = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxUseCompression = new System.Windows.Forms.CheckBox();
            this.gbFileNameSetting = new System.Windows.Forms.GroupBox();
            this.vOneLabelControl6 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.vOneLabelControl4 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.vOneLabelControl7 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDate = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtFileName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.vOneLabelControl2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblFileName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblNumber = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.vOneLabelControl1 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDueDateYMD = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxUnitSetting = new System.Windows.Forms.GroupBox();
            this.rdoAllInOne = new System.Windows.Forms.RadioButton();
            this.rdoByReport = new System.Windows.Forms.RadioButton();
            this.gbCompression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbMaxSize)).BeginInit();
            this.gbFileNameSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName)).BeginInit();
            this.gbxUnitSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCompression
            // 
            this.gbCompression.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbCompression.Controls.Add(this.nmbMaxSize);
            this.gbCompression.Controls.Add(this.vOneLabelControl3);
            this.gbCompression.Controls.Add(this.lblMaxSize);
            this.gbCompression.Controls.Add(this.cbxUseCompression);
            this.gbCompression.Location = new System.Drawing.Point(175, 373);
            this.gbCompression.Name = "gbCompression";
            this.gbCompression.Size = new System.Drawing.Size(659, 90);
            this.gbCompression.TabIndex = 2;
            this.gbCompression.TabStop = false;
            this.gbCompression.Text = "圧縮設定";
            // 
            // nmbMaxSize
            // 
            this.nmbMaxSize.AllowDeleteToNull = true;
            this.nmbMaxSize.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.nmbMaxSize.DropDown.AllowDrop = false;
            this.nmbMaxSize.Fields.DecimalPart.MaxDigits = 1;
            this.nmbMaxSize.Fields.IntegerPart.MinDigits = 1;
            this.nmbMaxSize.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nmbMaxSize.HighlightText = true;
            this.nmbMaxSize.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbMaxSize.Location = new System.Drawing.Point(148, 52);
            this.nmbMaxSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nmbMaxSize.MaxMinBehavior = GrapeCity.Win.Editors.MaxMinBehavior.CancelInput;
            this.nmbMaxSize.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nmbMaxSize.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nmbMaxSize.Name = "nmbMaxSize";
            this.nmbMaxSize.Required = false;
            this.nmbMaxSize.Size = new System.Drawing.Size(112, 22);
            this.nmbMaxSize.Spin.AllowSpin = false;
            this.nmbMaxSize.TabIndex = 130;
            this.nmbMaxSize.Value = null;
            // 
            // vOneLabelControl3
            // 
            this.vOneLabelControl3.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.vOneLabelControl3.Location = new System.Drawing.Point(266, 54);
            this.vOneLabelControl3.Name = "vOneLabelControl3";
            this.vOneLabelControl3.Size = new System.Drawing.Size(39, 16);
            this.vOneLabelControl3.TabIndex = 129;
            this.vOneLabelControl3.Text = "MB";
            this.vOneLabelControl3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMaxSize
            // 
            this.lblMaxSize.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblMaxSize.Location = new System.Drawing.Point(11, 54);
            this.lblMaxSize.Name = "lblMaxSize";
            this.lblMaxSize.Size = new System.Drawing.Size(140, 16);
            this.lblMaxSize.TabIndex = 128;
            this.lblMaxSize.Text = "圧縮前 最大ファイルサイズ";
            this.lblMaxSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxUseCompression
            // 
            this.cbxUseCompression.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cbxUseCompression.Location = new System.Drawing.Point(14, 24);
            this.cbxUseCompression.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cbxUseCompression.Name = "cbxUseCompression";
            this.cbxUseCompression.Size = new System.Drawing.Size(260, 18);
            this.cbxUseCompression.TabIndex = 126;
            this.cbxUseCompression.Text = "ファイルを圧縮（zip形式）して保存する";
            this.cbxUseCompression.UseVisualStyleBackColor = true;
            // 
            // gbFileNameSetting
            // 
            this.gbFileNameSetting.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbFileNameSetting.Controls.Add(this.vOneLabelControl6);
            this.gbFileNameSetting.Controls.Add(this.vOneLabelControl4);
            this.gbFileNameSetting.Controls.Add(this.vOneLabelControl7);
            this.gbFileNameSetting.Controls.Add(this.lblDate);
            this.gbFileNameSetting.Controls.Add(this.txtFileName);
            this.gbFileNameSetting.Controls.Add(this.vOneLabelControl2);
            this.gbFileNameSetting.Controls.Add(this.lblFileName);
            this.gbFileNameSetting.Controls.Add(this.lblNumber);
            this.gbFileNameSetting.Controls.Add(this.vOneLabelControl1);
            this.gbFileNameSetting.Controls.Add(this.lblDueDateYMD);
            this.gbFileNameSetting.Location = new System.Drawing.Point(175, 235);
            this.gbFileNameSetting.Name = "gbFileNameSetting";
            this.gbFileNameSetting.Size = new System.Drawing.Size(659, 117);
            this.gbFileNameSetting.TabIndex = 1;
            this.gbFileNameSetting.TabStop = false;
            this.gbFileNameSetting.Text = "ファイル名設定";
            // 
            // vOneLabelControl6
            // 
            this.vOneLabelControl6.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.vOneLabelControl6.Location = new System.Drawing.Point(582, 65);
            this.vOneLabelControl6.Name = "vOneLabelControl6";
            this.vOneLabelControl6.Size = new System.Drawing.Size(63, 16);
            this.vOneLabelControl6.TabIndex = 127;
            this.vOneLabelControl6.Text = "：[NO]";
            this.vOneLabelControl6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vOneLabelControl4
            // 
            this.vOneLabelControl4.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.vOneLabelControl4.Location = new System.Drawing.Point(582, 44);
            this.vOneLabelControl4.Name = "vOneLabelControl4";
            this.vOneLabelControl4.Size = new System.Drawing.Size(63, 16);
            this.vOneLabelControl4.TabIndex = 125;
            this.vOneLabelControl4.Text = "：[NAME]";
            this.vOneLabelControl4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vOneLabelControl7
            // 
            this.vOneLabelControl7.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.vOneLabelControl7.Location = new System.Drawing.Point(582, 86);
            this.vOneLabelControl7.Name = "vOneLabelControl7";
            this.vOneLabelControl7.Size = new System.Drawing.Size(106, 16);
            this.vOneLabelControl7.TabIndex = 128;
            this.vOneLabelControl7.Text = "：[DATE]";
            this.vOneLabelControl7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblDate.Location = new System.Drawing.Point(515, 86);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(76, 16);
            this.lblDate.TabIndex = 127;
            this.lblDate.Text = "{Date}";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFileName
            // 
            this.txtFileName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFileName.DropDown.AllowDrop = false;
            this.txtFileName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtFileName.HighlightText = true;
            this.txtFileName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtFileName.Location = new System.Drawing.Point(117, 25);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFileName.MaxLength = 50;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Required = true;
            this.txtFileName.Size = new System.Drawing.Size(372, 22);
            this.txtFileName.TabIndex = 111;
            // 
            // vOneLabelControl2
            // 
            this.vOneLabelControl2.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.vOneLabelControl2.Location = new System.Drawing.Point(582, 23);
            this.vOneLabelControl2.Name = "vOneLabelControl2";
            this.vOneLabelControl2.Size = new System.Drawing.Size(63, 16);
            this.vOneLabelControl2.TabIndex = 124;
            this.vOneLabelControl2.Text = "：[CODE]";
            this.vOneLabelControl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFileName
            // 
            this.lblFileName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblFileName.Location = new System.Drawing.Point(11, 27);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(100, 16);
            this.lblFileName.TabIndex = 112;
            this.lblFileName.Text = "ファイル名";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNumber
            // 
            this.lblNumber.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblNumber.Location = new System.Drawing.Point(515, 65);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(77, 16);
            this.lblNumber.TabIndex = 126;
            this.lblNumber.Text = "{Number}";
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vOneLabelControl1
            // 
            this.vOneLabelControl1.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.vOneLabelControl1.Location = new System.Drawing.Point(515, 44);
            this.vOneLabelControl1.Name = "vOneLabelControl1";
            this.vOneLabelControl1.Size = new System.Drawing.Size(77, 16);
            this.vOneLabelControl1.TabIndex = 124;
            this.vOneLabelControl1.Text = "得意先名";
            this.vOneLabelControl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDueDateYMD
            // 
            this.lblDueDateYMD.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblDueDateYMD.Location = new System.Drawing.Point(515, 23);
            this.lblDueDateYMD.Name = "lblDueDateYMD";
            this.lblDueDateYMD.Size = new System.Drawing.Size(77, 16);
            this.lblDueDateYMD.TabIndex = 123;
            this.lblDueDateYMD.Text = "得意先コード";
            this.lblDueDateYMD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxUnitSetting
            // 
            this.gbxUnitSetting.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbxUnitSetting.Controls.Add(this.rdoAllInOne);
            this.gbxUnitSetting.Controls.Add(this.rdoByReport);
            this.gbxUnitSetting.Location = new System.Drawing.Point(175, 158);
            this.gbxUnitSetting.Name = "gbxUnitSetting";
            this.gbxUnitSetting.Size = new System.Drawing.Size(659, 57);
            this.gbxUnitSetting.TabIndex = 0;
            this.gbxUnitSetting.TabStop = false;
            this.gbxUnitSetting.Text = "出力単位設定";
            // 
            // rdoAllInOne
            // 
            this.rdoAllInOne.Checked = true;
            this.rdoAllInOne.Location = new System.Drawing.Point(14, 22);
            this.rdoAllInOne.Name = "rdoAllInOne";
            this.rdoAllInOne.Size = new System.Drawing.Size(196, 18);
            this.rdoAllInOne.TabIndex = 9;
            this.rdoAllInOne.TabStop = true;
            this.rdoAllInOne.Text = "ひとつのファイルにまとめて出力する";
            this.rdoAllInOne.UseVisualStyleBackColor = true;
            // 
            // rdoByReport
            // 
            this.rdoByReport.Location = new System.Drawing.Point(213, 22);
            this.rdoByReport.Name = "rdoByReport";
            this.rdoByReport.Size = new System.Drawing.Size(225, 18);
            this.rdoByReport.TabIndex = 10;
            this.rdoByReport.Text = "帳票ごとにファイルを分けて出力する";
            this.rdoByReport.UseVisualStyleBackColor = true;
            // 
            // PC1503
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCompression);
            this.Controls.Add(this.gbFileNameSetting);
            this.Controls.Add(this.gbxUnitSetting);
            this.Name = "PC1503";
            this.Load += new System.EventHandler(this.PC1503_Load);
            this.gbCompression.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmbMaxSize)).EndInit();
            this.gbFileNameSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName)).EndInit();
            this.gbxUnitSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxUnitSetting;
        private System.Windows.Forms.GroupBox gbFileNameSetting;
        private System.Windows.Forms.GroupBox gbCompression;
        private System.Windows.Forms.RadioButton rdoAllInOne;
        private System.Windows.Forms.RadioButton rdoByReport;
        private Common.Controls.VOneTextControl txtFileName;
        private Common.Controls.VOneLabelControl lblFileName;
        private Common.Controls.VOneLabelControl vOneLabelControl1;
        private Common.Controls.VOneLabelControl lblDueDateYMD;
        private Common.Controls.VOneLabelControl vOneLabelControl3;
        private Common.Controls.VOneLabelControl lblMaxSize;
        private System.Windows.Forms.CheckBox cbxUseCompression;
        private Common.Controls.VOneNumberControl nmbMaxSize;
        private Common.Controls.VOneLabelControl vOneLabelControl2;
        private Common.Controls.VOneLabelControl vOneLabelControl4;
        private Common.Controls.VOneLabelControl vOneLabelControl6;
        private Common.Controls.VOneLabelControl vOneLabelControl7;
        public Common.Controls.VOneLabelControl lblNumber;
        public Common.Controls.VOneLabelControl lblDate;
    }
}
