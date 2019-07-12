namespace Rac.VOne.Client.Screen
{
    partial class PC1002
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
            this.txtPatternName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtPatternNumber = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblPatternName = new System.Windows.Forms.Label();
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.btnPatternNoSearch = new System.Windows.Forms.Button();
            this.gbxImportFileConfig = new System.Windows.Forms.GroupBox();
            this.nmbStartLineCount = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.cbxIgnoreLastLine = new System.Windows.Forms.CheckBox();
            this.lblStartLineCount = new System.Windows.Forms.Label();
            this.btnInitialDirectory = new System.Windows.Forms.Button();
            this.gbxActiion = new System.Windows.Forms.GroupBox();
            this.rdoAddDate = new System.Windows.Forms.RadioButton();
            this.rdoDelete = new System.Windows.Forms.RadioButton();
            this.rdoNoAction = new System.Windows.Forms.RadioButton();
            this.txtInitialDirectory = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblInitialDirectory = new System.Windows.Forms.Label();
            this.lblColumnSetting = new System.Windows.Forms.Label();
            this.lblPatternNumber = new System.Windows.Forms.Label();
            this.gbxMain = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.gbxImportFileConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbStartLineCount)).BeginInit();
            this.gbxActiion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialDirectory)).BeginInit();
            this.gbxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPatternName
            // 
            this.txtPatternName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPatternName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPatternName.DropDown.AllowDrop = false;
            this.txtPatternName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatternName.HighlightText = true;
            this.txtPatternName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtPatternName.Location = new System.Drawing.Point(267, 20);
            this.txtPatternName.MaxLength = 40;
            this.txtPatternName.Name = "txtPatternName";
            this.txtPatternName.Required = true;
            this.txtPatternName.Size = new System.Drawing.Size(660, 22);
            this.txtPatternName.TabIndex = 3;
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
            this.txtPatternNumber.Location = new System.Drawing.Point(139, 20);
            this.txtPatternNumber.MaxLength = 2;
            this.txtPatternNumber.Name = "txtPatternNumber";
            this.txtPatternNumber.Required = true;
            this.txtPatternNumber.Size = new System.Drawing.Size(30, 22);
            this.txtPatternNumber.TabIndex = 1;
            this.txtPatternNumber.Validated += new System.EventHandler(this.txtPatternNo_Validated);
            // 
            // lblPatternName
            // 
            this.lblPatternName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPatternName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatternName.Location = new System.Drawing.Point(204, 22);
            this.lblPatternName.Name = "lblPatternName";
            this.lblPatternName.Size = new System.Drawing.Size(57, 16);
            this.lblPatternName.TabIndex = 2;
            this.lblPatternName.Text = "パターン名";
            this.lblPatternName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            cellStyle1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grid.DefaultCellStyle = cellStyle1;
            this.grid.HorizontalScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.grid.Location = new System.Drawing.Point(15, 219);
            this.grid.Margin = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(948, 363);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 7;
            this.grid.Text = "";
            this.grid.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grid_CellValueChanged);
            this.grid.CellEditedFormattedValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs>(this.grid_CellEditedFormattedValueChanged);
            // 
            // btnPatternNoSearch
            // 
            this.btnPatternNoSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPatternNoSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatternNoSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnPatternNoSearch.Location = new System.Drawing.Point(175, 19);
            this.btnPatternNoSearch.Name = "btnPatternNoSearch";
            this.btnPatternNoSearch.Size = new System.Drawing.Size(24, 24);
            this.btnPatternNoSearch.TabIndex = 4;
            this.btnPatternNoSearch.UseVisualStyleBackColor = true;
            this.btnPatternNoSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gbxImportFileConfig
            // 
            this.gbxImportFileConfig.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxImportFileConfig.Controls.Add(this.nmbStartLineCount);
            this.gbxImportFileConfig.Controls.Add(this.cbxIgnoreLastLine);
            this.gbxImportFileConfig.Controls.Add(this.lblStartLineCount);
            this.gbxImportFileConfig.Controls.Add(this.btnInitialDirectory);
            this.gbxImportFileConfig.Controls.Add(this.gbxActiion);
            this.gbxImportFileConfig.Controls.Add(this.txtInitialDirectory);
            this.gbxImportFileConfig.Controls.Add(this.lblInitialDirectory);
            this.gbxImportFileConfig.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxImportFileConfig.Location = new System.Drawing.Point(15, 52);
            this.gbxImportFileConfig.Name = "gbxImportFileConfig";
            this.gbxImportFileConfig.Size = new System.Drawing.Size(948, 136);
            this.gbxImportFileConfig.TabIndex = 5;
            this.gbxImportFileConfig.TabStop = false;
            this.gbxImportFileConfig.Text = "□　取込ファイル指定";
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
            this.nmbStartLineCount.TabIndex = 5;
            this.nmbStartLineCount.Value = null;
            // 
            // cbxIgnoreLastLine
            // 
            this.cbxIgnoreLastLine.AutoSize = true;
            this.cbxIgnoreLastLine.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxIgnoreLastLine.Location = new System.Drawing.Point(342, 105);
            this.cbxIgnoreLastLine.Name = "cbxIgnoreLastLine";
            this.cbxIgnoreLastLine.Size = new System.Drawing.Size(148, 19);
            this.cbxIgnoreLastLine.TabIndex = 6;
            this.cbxIgnoreLastLine.Text = "最終行を取込処理しない";
            this.cbxIgnoreLastLine.UseVisualStyleBackColor = true;
            // 
            // lblStartLineCount
            // 
            this.lblStartLineCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartLineCount.Location = new System.Drawing.Point(51, 105);
            this.lblStartLineCount.Name = "lblStartLineCount";
            this.lblStartLineCount.Size = new System.Drawing.Size(67, 16);
            this.lblStartLineCount.TabIndex = 4;
            this.lblStartLineCount.Text = "取込開始行";
            this.lblStartLineCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnInitialDirectory
            // 
            this.btnInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitialDirectory.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnInitialDirectory.Location = new System.Drawing.Point(918, 21);
            this.btnInitialDirectory.Name = "btnInitialDirectory";
            this.btnInitialDirectory.Size = new System.Drawing.Size(24, 24);
            this.btnInitialDirectory.TabIndex = 2;
            this.btnInitialDirectory.UseVisualStyleBackColor = true;
            this.btnInitialDirectory.Click += new System.EventHandler(this.btnInitialDirectory_Click);
            // 
            // gbxActiion
            // 
            this.gbxActiion.Controls.Add(this.rdoAddDate);
            this.gbxActiion.Controls.Add(this.rdoDelete);
            this.gbxActiion.Controls.Add(this.rdoNoAction);
            this.gbxActiion.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxActiion.Location = new System.Drawing.Point(124, 50);
            this.gbxActiion.Name = "gbxActiion";
            this.gbxActiion.Size = new System.Drawing.Size(788, 44);
            this.gbxActiion.TabIndex = 3;
            this.gbxActiion.TabStop = false;
            this.gbxActiion.Text = "取込後のファイル操作";
            // 
            // rdoAddDate
            // 
            this.rdoAddDate.AutoSize = true;
            this.rdoAddDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoAddDate.Location = new System.Drawing.Point(386, 18);
            this.rdoAddDate.Margin = new System.Windows.Forms.Padding(50, 3, 50, 3);
            this.rdoAddDate.Name = "rdoAddDate";
            this.rdoAddDate.Size = new System.Drawing.Size(106, 19);
            this.rdoAddDate.TabIndex = 2;
            this.rdoAddDate.Text = "取込日時を付加";
            this.rdoAddDate.UseVisualStyleBackColor = true;
            // 
            // rdoDelete
            // 
            this.rdoDelete.AutoSize = true;
            this.rdoDelete.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDelete.Location = new System.Drawing.Point(218, 18);
            this.rdoDelete.Margin = new System.Windows.Forms.Padding(50, 3, 50, 3);
            this.rdoDelete.Name = "rdoDelete";
            this.rdoDelete.Size = new System.Drawing.Size(68, 19);
            this.rdoDelete.TabIndex = 1;
            this.rdoDelete.Text = "削除する";
            this.rdoDelete.UseVisualStyleBackColor = true;
            // 
            // rdoNoAction
            // 
            this.rdoNoAction.AutoSize = true;
            this.rdoNoAction.Checked = true;
            this.rdoNoAction.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoNoAction.Location = new System.Drawing.Point(36, 18);
            this.rdoNoAction.Margin = new System.Windows.Forms.Padding(3, 3, 50, 3);
            this.rdoNoAction.Name = "rdoNoAction";
            this.rdoNoAction.Size = new System.Drawing.Size(82, 19);
            this.rdoNoAction.TabIndex = 0;
            this.rdoNoAction.TabStop = true;
            this.rdoNoAction.Text = "なにもしない";
            this.rdoNoAction.UseVisualStyleBackColor = true;
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
            this.txtInitialDirectory.TabIndex = 1;
            // 
            // lblInitialDirectory
            // 
            this.lblInitialDirectory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitialDirectory.Location = new System.Drawing.Point(51, 25);
            this.lblInitialDirectory.Name = "lblInitialDirectory";
            this.lblInitialDirectory.Size = new System.Drawing.Size(67, 16);
            this.lblInitialDirectory.TabIndex = 0;
            this.lblInitialDirectory.Text = "取込フォルダ";
            this.lblInitialDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColumnSetting
            // 
            this.lblColumnSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumnSetting.Location = new System.Drawing.Point(26, 194);
            this.lblColumnSetting.Margin = new System.Windows.Forms.Padding(3);
            this.lblColumnSetting.Name = "lblColumnSetting";
            this.lblColumnSetting.Size = new System.Drawing.Size(88, 16);
            this.lblColumnSetting.TabIndex = 6;
            this.lblColumnSetting.Text = "□　取込列指定";
            this.lblColumnSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPatternNumber
            // 
            this.lblPatternNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPatternNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatternNumber.Location = new System.Drawing.Point(66, 22);
            this.lblPatternNumber.Name = "lblPatternNumber";
            this.lblPatternNumber.Size = new System.Drawing.Size(67, 16);
            this.lblPatternNumber.TabIndex = 0;
            this.lblPatternNumber.Text = "パターンNo.";
            this.lblPatternNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxMain
            // 
            this.gbxMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxMain.Controls.Add(this.txtPatternName);
            this.gbxMain.Controls.Add(this.grid);
            this.gbxMain.Controls.Add(this.txtPatternNumber);
            this.gbxMain.Controls.Add(this.lblPatternNumber);
            this.gbxMain.Controls.Add(this.lblPatternName);
            this.gbxMain.Controls.Add(this.lblColumnSetting);
            this.gbxMain.Controls.Add(this.gbxImportFileConfig);
            this.gbxMain.Controls.Add(this.btnPatternNoSearch);
            this.gbxMain.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxMain.Location = new System.Drawing.Point(15, 15);
            this.gbxMain.Name = "gbxMain";
            this.gbxMain.Size = new System.Drawing.Size(978, 591);
            this.gbxMain.TabIndex = 8;
            this.gbxMain.TabStop = false;
            // 
            // PC1002
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PC1002";
            this.Load += new System.EventHandler(this.PC1002_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.gbxImportFileConfig.ResumeLayout(false);
            this.gbxImportFileConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbStartLineCount)).EndInit();
            this.gbxActiion.ResumeLayout(false);
            this.gbxActiion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitialDirectory)).EndInit();
            this.gbxMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPatternNumber;
        private Common.Controls.VOneTextControl txtPatternNumber;
        private System.Windows.Forms.Label lblPatternName;
        private Common.Controls.VOneTextControl txtPatternName;
        private System.Windows.Forms.GroupBox gbxImportFileConfig;
        private Common.Controls.VOneTextControl txtInitialDirectory;
        private System.Windows.Forms.Label lblInitialDirectory;
        private System.Windows.Forms.GroupBox gbxActiion;
        private System.Windows.Forms.RadioButton rdoAddDate;
        private System.Windows.Forms.RadioButton rdoDelete;
        private System.Windows.Forms.RadioButton rdoNoAction;
        private System.Windows.Forms.Button btnInitialDirectory;
        private System.Windows.Forms.Label lblStartLineCount;
        private System.Windows.Forms.CheckBox cbxIgnoreLastLine;
        private System.Windows.Forms.Label lblColumnSetting;
        private Common.Controls.VOneGridControl grid;
        private System.Windows.Forms.Button btnPatternNoSearch;
        private Common.Controls.VOneNumberControl nmbStartLineCount;
        private System.Windows.Forms.GroupBox gbxMain;
    }
}