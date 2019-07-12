namespace Rac.VOne.Client.Screen
{
    partial class PB0503
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbxImportItemsSetting = new System.Windows.Forms.GroupBox();
            this.grdSetting = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxImportFileSetting = new System.Windows.Forms.GroupBox();
            this.lblInitialDirectory = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.nmbStartLineCount = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.cbxIgnoreLastLine = new System.Windows.Forms.CheckBox();
            this.btnInitialDirectory = new System.Windows.Forms.Button();
            this.txtInitialDirectory = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblStartLineCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblPatternNumber = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtPatternNumber = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnPatternNoSearch = new System.Windows.Forms.Button();
            this.lblPatternName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtPatternName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.gbxImportItemsSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSetting)).BeginInit();
            this.gbxImportFileSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbStartLineCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialDirectory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxImportItemsSetting
            // 
            this.gbxImportItemsSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxImportItemsSetting.Controls.Add(this.grdSetting);
            this.gbxImportItemsSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxImportItemsSetting.Location = new System.Drawing.Point(15, 123);
            this.gbxImportItemsSetting.Name = "gbxImportItemsSetting";
            this.gbxImportItemsSetting.Size = new System.Drawing.Size(978, 489);
            this.gbxImportItemsSetting.TabIndex = 9;
            this.gbxImportItemsSetting.TabStop = false;
            this.gbxImportItemsSetting.Text = "□　取込列指定";
            // 
            // grdSetting
            // 
            this.grdSetting.AllowAutoExtend = true;
            this.grdSetting.AllowUserToAddRows = false;
            this.grdSetting.AllowUserToShiftSelect = true;
            this.grdSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdSetting.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdSetting.Location = new System.Drawing.Point(9, 22);
            this.grdSetting.Name = "grdSetting";
            this.grdSetting.Size = new System.Drawing.Size(960, 461);
            this.grdSetting.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdSetting.TabIndex = 6;
            this.grdSetting.Text = "vOneGridControl1";
            this.grdSetting.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdSetting_CellEnter);
            this.grdSetting.EditingControlShowing += new System.EventHandler<GrapeCity.Win.MultiRow.EditingControlShowingEventArgs>(this.grdSetting_EditingControlShowing);
            this.grdSetting.CellEditedFormattedValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs>(this.grdSetting_CellEditedFormattedValueChanged);
            // 
            // gbxImportFileSetting
            // 
            this.gbxImportFileSetting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxImportFileSetting.Controls.Add(this.lblInitialDirectory);
            this.gbxImportFileSetting.Controls.Add(this.nmbStartLineCount);
            this.gbxImportFileSetting.Controls.Add(this.cbxIgnoreLastLine);
            this.gbxImportFileSetting.Controls.Add(this.btnInitialDirectory);
            this.gbxImportFileSetting.Controls.Add(this.txtInitialDirectory);
            this.gbxImportFileSetting.Controls.Add(this.lblStartLineCount);
            this.gbxImportFileSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxImportFileSetting.Location = new System.Drawing.Point(15, 37);
            this.gbxImportFileSetting.Name = "gbxImportFileSetting";
            this.gbxImportFileSetting.Size = new System.Drawing.Size(978, 80);
            this.gbxImportFileSetting.TabIndex = 4;
            this.gbxImportFileSetting.TabStop = false;
            this.gbxImportFileSetting.Text = "□　取込ファイル指定";
            // 
            // lblInitialDirectory
            // 
            this.lblInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitialDirectory.Location = new System.Drawing.Point(115, 22);
            this.lblInitialDirectory.Name = "lblInitialDirectory";
            this.lblInitialDirectory.Size = new System.Drawing.Size(67, 16);
            this.lblInitialDirectory.TabIndex = 0;
            this.lblInitialDirectory.Text = "取込フォルダ";
            this.lblInitialDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nmbStartLineCount
            // 
            this.nmbStartLineCount.AllowDeleteToNull = true;
            this.nmbStartLineCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.nmbStartLineCount.DropDown.AllowDrop = false;
            this.nmbStartLineCount.Fields.IntegerPart.MaxDigits = 1;
            this.nmbStartLineCount.Fields.IntegerPart.MinDigits = 1;
            this.nmbStartLineCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmbStartLineCount.HighlightText = true;
            this.nmbStartLineCount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbStartLineCount.Location = new System.Drawing.Point(188, 48);
            this.nmbStartLineCount.MaxValue = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nmbStartLineCount.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmbStartLineCount.Name = "nmbStartLineCount";
            this.nmbStartLineCount.Required = true;
            this.nmbStartLineCount.Size = new System.Drawing.Size(25, 22);
            this.nmbStartLineCount.Spin.AllowSpin = false;
            this.nmbStartLineCount.TabIndex = 7;
            this.nmbStartLineCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbxIgnoreLastLine
            // 
            this.cbxIgnoreLastLine.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxIgnoreLastLine.Location = new System.Drawing.Point(266, 50);
            this.cbxIgnoreLastLine.Name = "cbxIgnoreLastLine";
            this.cbxIgnoreLastLine.Size = new System.Drawing.Size(148, 18);
            this.cbxIgnoreLastLine.TabIndex = 8;
            this.cbxIgnoreLastLine.Text = "最終行を取込処理しない";
            this.cbxIgnoreLastLine.UseVisualStyleBackColor = true;
            // 
            // btnInitialDirectory
            // 
            this.btnInitialDirectory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitialDirectory.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnInitialDirectory.Location = new System.Drawing.Point(870, 18);
            this.btnInitialDirectory.Name = "btnInitialDirectory";
            this.btnInitialDirectory.Size = new System.Drawing.Size(24, 24);
            this.btnInitialDirectory.TabIndex = 6;
            this.btnInitialDirectory.UseVisualStyleBackColor = false;
            this.btnInitialDirectory.Click += new System.EventHandler(this.btnInitialDirectory_Click);
            // 
            // txtInitialDirectory
            // 
            this.txtInitialDirectory.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtInitialDirectory.DropDown.AllowDrop = false;
            this.txtInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInitialDirectory.HighlightText = true;
            this.txtInitialDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtInitialDirectory.Location = new System.Drawing.Point(188, 20);
            this.txtInitialDirectory.MaxLength = 255;
            this.txtInitialDirectory.Name = "txtInitialDirectory";
            this.txtInitialDirectory.Required = true;
            this.txtInitialDirectory.Size = new System.Drawing.Size(676, 22);
            this.txtInitialDirectory.TabIndex = 5;
            // 
            // lblStartLineCount
            // 
            this.lblStartLineCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartLineCount.Location = new System.Drawing.Point(115, 50);
            this.lblStartLineCount.Name = "lblStartLineCount";
            this.lblStartLineCount.Size = new System.Drawing.Size(67, 16);
            this.lblStartLineCount.TabIndex = 0;
            this.lblStartLineCount.Text = "取込開始行";
            this.lblStartLineCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPatternNumber
            // 
            this.lblPatternNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPatternNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatternNumber.Location = new System.Drawing.Point(61, 11);
            this.lblPatternNumber.Name = "lblPatternNumber";
            this.lblPatternNumber.Size = new System.Drawing.Size(67, 16);
            this.lblPatternNumber.TabIndex = 6;
            this.lblPatternNumber.Text = "パターンNo.";
            this.lblPatternNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPatternNumber
            // 
            this.txtPatternNumber.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtPatternNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPatternNumber.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPatternNumber.DropDown.AllowDrop = false;
            this.txtPatternNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatternNumber.Format = "9";
            this.txtPatternNumber.HighlightText = true;
            this.txtPatternNumber.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPatternNumber.Location = new System.Drawing.Point(134, 9);
            this.txtPatternNumber.MaxLength = 2;
            this.txtPatternNumber.Name = "txtPatternNumber";
            this.txtPatternNumber.Required = true;
            this.txtPatternNumber.Size = new System.Drawing.Size(30, 22);
            this.txtPatternNumber.TabIndex = 1;
            this.txtPatternNumber.Validated += new System.EventHandler(this.txtPatternNumber_Validated);
            // 
            // btnPatternNoSearch
            // 
            this.btnPatternNoSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPatternNoSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatternNoSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnPatternNoSearch.Location = new System.Drawing.Point(170, 8);
            this.btnPatternNoSearch.Name = "btnPatternNoSearch";
            this.btnPatternNoSearch.Size = new System.Drawing.Size(24, 24);
            this.btnPatternNoSearch.TabIndex = 2;
            this.btnPatternNoSearch.UseVisualStyleBackColor = true;
            this.btnPatternNoSearch.Click += new System.EventHandler(this.btnPatternNoSearch_Click);
            // 
            // lblPatternName
            // 
            this.lblPatternName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPatternName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatternName.Location = new System.Drawing.Point(200, 12);
            this.lblPatternName.Name = "lblPatternName";
            this.lblPatternName.Size = new System.Drawing.Size(56, 16);
            this.lblPatternName.TabIndex = 7;
            this.lblPatternName.Text = "パターン名";
            this.lblPatternName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPatternName
            // 
            this.txtPatternName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPatternName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPatternName.DropDown.AllowDrop = false;
            this.txtPatternName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatternName.HighlightText = true;
            this.txtPatternName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtPatternName.Location = new System.Drawing.Point(262, 9);
            this.txtPatternName.MaxLength = 40;
            this.txtPatternName.Name = "txtPatternName";
            this.txtPatternName.Required = true;
            this.txtPatternName.Size = new System.Drawing.Size(617, 22);
            this.txtPatternName.TabIndex = 3;
            // 
            // PB0503
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblPatternNumber);
            this.Controls.Add(this.txtPatternNumber);
            this.Controls.Add(this.btnPatternNoSearch);
            this.Controls.Add(this.lblPatternName);
            this.Controls.Add(this.txtPatternName);
            this.Controls.Add(this.gbxImportItemsSetting);
            this.Controls.Add(this.gbxImportFileSetting);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Name = "PB0503";
            this.Load += new System.EventHandler(this.PB0503_Load);
            this.gbxImportItemsSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSetting)).EndInit();
            this.gbxImportFileSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmbStartLineCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialDirectory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxImportFileSetting;
        private Common.Controls.VOneTextControl txtInitialDirectory;
        private System.Windows.Forms.Button btnInitialDirectory;
        private System.Windows.Forms.CheckBox cbxIgnoreLastLine;
        private Common.Controls.VOneLabelControl lblInitialDirectory;
        private Common.Controls.VOneLabelControl lblStartLineCount;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox gbxImportItemsSetting;
        private Common.Controls.VOneGridControl grdSetting;
        private Common.Controls.VOneNumberControl nmbStartLineCount;
        private Common.Controls.VOneLabelControl lblPatternNumber;
        private Common.Controls.VOneTextControl txtPatternNumber;
        private System.Windows.Forms.Button btnPatternNoSearch;
        private Common.Controls.VOneLabelControl lblPatternName;
        private Common.Controls.VOneTextControl txtPatternName;
    }
}
