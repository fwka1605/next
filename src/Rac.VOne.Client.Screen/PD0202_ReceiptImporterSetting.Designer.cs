namespace Rac.VOne.Client.Screen
{
    partial class PD0202
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            this.gbxMain = new System.Windows.Forms.GroupBox();
            this.lblColumnSetting = new System.Windows.Forms.Label();
            this.gbxFile = new System.Windows.Forms.GroupBox();
            this.txtInitialDirectory = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnPath = new System.Windows.Forms.Button();
            this.lblStartLineCount = new System.Windows.Forms.Label();
            this.cbxIgnoreLastLine = new System.Windows.Forms.CheckBox();
            this.nmbStartLineCount = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.gbxPostAction = new System.Windows.Forms.GroupBox();
            this.rdoRename = new System.Windows.Forms.RadioButton();
            this.rdoDelete = new System.Windows.Forms.RadioButton();
            this.rdoDoNothing = new System.Windows.Forms.RadioButton();
            this.lblInitialDirectory = new System.Windows.Forms.Label();
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.txtSettingCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtSettingName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblSettingName = new System.Windows.Forms.Label();
            this.lblSettingCode = new System.Windows.Forms.Label();
            this.btnSearchSetting = new System.Windows.Forms.Button();
            this.gbxMain.SuspendLayout();
            this.gbxFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialDirectory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbStartLineCount)).BeginInit();
            this.gbxPostAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSettingCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSettingName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxMain
            // 
            this.gbxMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxMain.Controls.Add(this.lblColumnSetting);
            this.gbxMain.Controls.Add(this.gbxFile);
            this.gbxMain.Controls.Add(this.grid);
            this.gbxMain.Controls.Add(this.txtSettingCode);
            this.gbxMain.Controls.Add(this.txtSettingName);
            this.gbxMain.Controls.Add(this.lblSettingName);
            this.gbxMain.Controls.Add(this.lblSettingCode);
            this.gbxMain.Controls.Add(this.btnSearchSetting);
            this.gbxMain.Location = new System.Drawing.Point(15, 15);
            this.gbxMain.Name = "gbxMain";
            this.gbxMain.Size = new System.Drawing.Size(978, 591);
            this.gbxMain.TabIndex = 0;
            this.gbxMain.TabStop = false;
            // 
            // lblColumnSetting
            // 
            this.lblColumnSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumnSetting.Location = new System.Drawing.Point(26, 194);
            this.lblColumnSetting.Margin = new System.Windows.Forms.Padding(3);
            this.lblColumnSetting.Name = "lblColumnSetting";
            this.lblColumnSetting.Size = new System.Drawing.Size(88, 16);
            this.lblColumnSetting.TabIndex = 13;
            this.lblColumnSetting.Text = "□　取込列指定";
            this.lblColumnSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxFile
            // 
            this.gbxFile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxFile.Controls.Add(this.txtInitialDirectory);
            this.gbxFile.Controls.Add(this.btnPath);
            this.gbxFile.Controls.Add(this.lblStartLineCount);
            this.gbxFile.Controls.Add(this.cbxIgnoreLastLine);
            this.gbxFile.Controls.Add(this.nmbStartLineCount);
            this.gbxFile.Controls.Add(this.gbxPostAction);
            this.gbxFile.Controls.Add(this.lblInitialDirectory);
            this.gbxFile.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxFile.Location = new System.Drawing.Point(15, 52);
            this.gbxFile.Margin = new System.Windows.Forms.Padding(12, 6, 12, 3);
            this.gbxFile.Name = "gbxFile";
            this.gbxFile.Size = new System.Drawing.Size(948, 136);
            this.gbxFile.TabIndex = 4;
            this.gbxFile.TabStop = false;
            this.gbxFile.Text = "□　取込ファイル指定";
            // 
            // txtInitialDirectory
            // 
            this.txtInitialDirectory.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtInitialDirectory.DropDown.AllowDrop = false;
            this.txtInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInitialDirectory.HighlightText = true;
            this.txtInitialDirectory.Location = new System.Drawing.Point(124, 22);
            this.txtInitialDirectory.MaxLength = 255;
            this.txtInitialDirectory.Name = "txtInitialDirectory";
            this.txtInitialDirectory.Required = true;
            this.txtInitialDirectory.Size = new System.Drawing.Size(788, 22);
            this.txtInitialDirectory.TabIndex = 5;
            // 
            // btnPath
            // 
            this.btnPath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPath.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnPath.Location = new System.Drawing.Point(918, 21);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(24, 24);
            this.btnPath.TabIndex = 6;
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // lblStartLineCount
            // 
            this.lblStartLineCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartLineCount.Location = new System.Drawing.Point(51, 105);
            this.lblStartLineCount.Margin = new System.Windows.Forms.Padding(3);
            this.lblStartLineCount.Name = "lblStartLineCount";
            this.lblStartLineCount.Size = new System.Drawing.Size(67, 16);
            this.lblStartLineCount.TabIndex = 5;
            this.lblStartLineCount.Text = "取込開始行";
            this.lblStartLineCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxIgnoreLastLine
            // 
            this.cbxIgnoreLastLine.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxIgnoreLastLine.Location = new System.Drawing.Point(342, 105);
            this.cbxIgnoreLastLine.Name = "cbxIgnoreLastLine";
            this.cbxIgnoreLastLine.Size = new System.Drawing.Size(148, 18);
            this.cbxIgnoreLastLine.TabIndex = 12;
            this.cbxIgnoreLastLine.Text = "最終行を取込処理しない";
            this.cbxIgnoreLastLine.UseVisualStyleBackColor = true;
            // 
            // nmbStartLineCount
            // 
            this.nmbStartLineCount.AllowDeleteToNull = true;
            this.nmbStartLineCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.nmbStartLineCount.DropDown.AllowDrop = false;
            this.nmbStartLineCount.Fields.IntegerPart.MinDigits = 1;
            this.nmbStartLineCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmbStartLineCount.HighlightText = true;
            this.nmbStartLineCount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbStartLineCount.Location = new System.Drawing.Point(124, 103);
            this.nmbStartLineCount.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
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
            this.nmbStartLineCount.Size = new System.Drawing.Size(30, 22);
            this.nmbStartLineCount.Spin.AllowSpin = false;
            this.nmbStartLineCount.TabIndex = 11;
            this.nmbStartLineCount.Value = null;
            // 
            // gbxPostAction
            // 
            this.gbxPostAction.Controls.Add(this.rdoRename);
            this.gbxPostAction.Controls.Add(this.rdoDelete);
            this.gbxPostAction.Controls.Add(this.rdoDoNothing);
            this.gbxPostAction.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxPostAction.Location = new System.Drawing.Point(124, 50);
            this.gbxPostAction.Name = "gbxPostAction";
            this.gbxPostAction.Padding = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.gbxPostAction.Size = new System.Drawing.Size(788, 44);
            this.gbxPostAction.TabIndex = 7;
            this.gbxPostAction.TabStop = false;
            this.gbxPostAction.Text = "取込後のファイル操作";
            // 
            // rdoRename
            // 
            this.rdoRename.AutoSize = true;
            this.rdoRename.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoRename.Location = new System.Drawing.Point(386, 18);
            this.rdoRename.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
            this.rdoRename.Name = "rdoRename";
            this.rdoRename.Size = new System.Drawing.Size(106, 19);
            this.rdoRename.TabIndex = 10;
            this.rdoRename.Text = "取込日時を付加";
            this.rdoRename.UseVisualStyleBackColor = true;
            // 
            // rdoDelete
            // 
            this.rdoDelete.AutoSize = true;
            this.rdoDelete.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDelete.Location = new System.Drawing.Point(218, 18);
            this.rdoDelete.Margin = new System.Windows.Forms.Padding(50, 3, 50, 3);
            this.rdoDelete.Name = "rdoDelete";
            this.rdoDelete.Size = new System.Drawing.Size(68, 19);
            this.rdoDelete.TabIndex = 9;
            this.rdoDelete.Text = "削除する";
            this.rdoDelete.UseVisualStyleBackColor = true;
            // 
            // rdoDoNothing
            // 
            this.rdoDoNothing.AutoSize = true;
            this.rdoDoNothing.Checked = true;
            this.rdoDoNothing.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDoNothing.Location = new System.Drawing.Point(36, 18);
            this.rdoDoNothing.Margin = new System.Windows.Forms.Padding(3, 3, 50, 3);
            this.rdoDoNothing.Name = "rdoDoNothing";
            this.rdoDoNothing.Size = new System.Drawing.Size(82, 19);
            this.rdoDoNothing.TabIndex = 8;
            this.rdoDoNothing.TabStop = true;
            this.rdoDoNothing.Text = "なにもしない";
            this.rdoDoNothing.UseVisualStyleBackColor = true;
            // 
            // lblInitialDirectory
            // 
            this.lblInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitialDirectory.Location = new System.Drawing.Point(51, 24);
            this.lblInitialDirectory.Name = "lblInitialDirectory";
            this.lblInitialDirectory.Size = new System.Drawing.Size(67, 16);
            this.lblInitialDirectory.TabIndex = 0;
            this.lblInitialDirectory.Text = "取込フォルダ";
            this.lblInitialDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            cellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.grid.DefaultCellStyle = cellStyle1;
            this.grid.HorizontalScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(15, 219);
            this.grid.Margin = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.grid.Name = "grid";
            this.grid.ShowRowErrors = false;
            this.grid.Size = new System.Drawing.Size(948, 363);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 14;
            // 
            // txtSettingCode
            // 
            this.txtSettingCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtSettingCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSettingCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSettingCode.DropDown.AllowDrop = false;
            this.txtSettingCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSettingCode.Format = "9";
            this.txtSettingCode.HighlightText = true;
            this.txtSettingCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSettingCode.Location = new System.Drawing.Point(139, 20);
            this.txtSettingCode.MaxLength = 2;
            this.txtSettingCode.Name = "txtSettingCode";
            this.txtSettingCode.PaddingChar = '0';
            this.txtSettingCode.Required = true;
            this.txtSettingCode.Size = new System.Drawing.Size(30, 22);
            this.txtSettingCode.TabIndex = 1;
            this.txtSettingCode.Validated += new System.EventHandler(this.txtPatternNo_Validated);
            // 
            // txtSettingName
            // 
            this.txtSettingName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSettingName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSettingName.DropDown.AllowDrop = false;
            this.txtSettingName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSettingName.HighlightText = true;
            this.txtSettingName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtSettingName.Location = new System.Drawing.Point(267, 20);
            this.txtSettingName.MaxLength = 40;
            this.txtSettingName.Name = "txtSettingName";
            this.txtSettingName.Required = true;
            this.txtSettingName.Size = new System.Drawing.Size(660, 22);
            this.txtSettingName.TabIndex = 3;
            // 
            // lblSettingName
            // 
            this.lblSettingName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSettingName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettingName.Location = new System.Drawing.Point(205, 22);
            this.lblSettingName.Margin = new System.Windows.Forms.Padding(3);
            this.lblSettingName.Name = "lblSettingName";
            this.lblSettingName.Size = new System.Drawing.Size(56, 16);
            this.lblSettingName.TabIndex = 2;
            this.lblSettingName.Text = "パターン名";
            this.lblSettingName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSettingCode
            // 
            this.lblSettingCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSettingCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettingCode.Location = new System.Drawing.Point(66, 23);
            this.lblSettingCode.Margin = new System.Windows.Forms.Padding(3);
            this.lblSettingCode.Name = "lblSettingCode";
            this.lblSettingCode.Size = new System.Drawing.Size(67, 16);
            this.lblSettingCode.TabIndex = 0;
            this.lblSettingCode.Text = "パターンNo.";
            this.lblSettingCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearchSetting
            // 
            this.btnSearchSetting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSearchSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchSetting.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSearchSetting.Location = new System.Drawing.Point(175, 19);
            this.btnSearchSetting.Name = "btnSearchSetting";
            this.btnSearchSetting.Size = new System.Drawing.Size(24, 24);
            this.btnSearchSetting.TabIndex = 2;
            this.btnSearchSetting.UseVisualStyleBackColor = true;
            this.btnSearchSetting.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // PD0202
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PD0202";
            this.Load += new System.EventHandler(this.PD0202_Load);
            this.gbxMain.ResumeLayout(false);
            this.gbxFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialDirectory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbStartLineCount)).EndInit();
            this.gbxPostAction.ResumeLayout(false);
            this.gbxPostAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSettingCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSettingName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxMain;
        private Common.Controls.VOneTextControl txtSettingCode;
        private System.Windows.Forms.GroupBox gbxFile;
        private Common.Controls.VOneNumberControl nmbStartLineCount;
        private System.Windows.Forms.CheckBox cbxIgnoreLastLine;
        private System.Windows.Forms.Label lblStartLineCount;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.GroupBox gbxPostAction;
        private System.Windows.Forms.RadioButton rdoRename;
        private System.Windows.Forms.RadioButton rdoDelete;
        private System.Windows.Forms.RadioButton rdoDoNothing;
        private Common.Controls.VOneTextControl txtInitialDirectory;
        private System.Windows.Forms.Label lblInitialDirectory;
        private System.Windows.Forms.Label lblSettingName;
        private System.Windows.Forms.Button btnSearchSetting;
        private Common.Controls.VOneTextControl txtSettingName;
        private System.Windows.Forms.Label lblSettingCode;
        private System.Windows.Forms.Label lblColumnSetting;
        private Common.Controls.VOneGridControl grid;
    }
}