namespace Rac.VOne.Client.Screen
{
    partial class PD0101
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
            this.gbxImport = new System.Windows.Forms.GroupBox();
            this.cmbDefaultEBFileSetting = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblYear = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdImportItems = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblDefaultEBFileSetting = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datYear = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.btnShowOpenFilesDialog = new System.Windows.Forms.Button();
            this.gbxHistory = new System.Windows.Forms.GroupBox();
            this.grdHistory = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDefaultEBFileSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdImportItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datYear)).BeginInit();
            this.gbxHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxImport
            // 
            this.gbxImport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxImport.Controls.Add(this.cmbDefaultEBFileSetting);
            this.gbxImport.Controls.Add(this.lblYear);
            this.gbxImport.Controls.Add(this.grdImportItems);
            this.gbxImport.Controls.Add(this.lblDefaultEBFileSetting);
            this.gbxImport.Controls.Add(this.datYear);
            this.gbxImport.Controls.Add(this.btnShowOpenFilesDialog);
            this.gbxImport.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxImport.Location = new System.Drawing.Point(12, 15);
            this.gbxImport.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.gbxImport.Name = "gbxImport";
            this.gbxImport.Size = new System.Drawing.Size(984, 289);
            this.gbxImport.TabIndex = 1;
            this.gbxImport.TabStop = false;
            this.gbxImport.Text = "□　取込指定";
            // 
            // cmbDefaultEBFileSetting
            // 
            this.cmbDefaultEBFileSetting.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbDefaultEBFileSetting.DisplayMember = null;
            this.cmbDefaultEBFileSetting.DropDown.AllowResize = false;
            this.cmbDefaultEBFileSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultEBFileSetting.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbDefaultEBFileSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDefaultEBFileSetting.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbDefaultEBFileSetting.ListHeaderPane.Height = 22;
            this.cmbDefaultEBFileSetting.ListHeaderPane.Visible = false;
            this.cmbDefaultEBFileSetting.Location = new System.Drawing.Point(206, 22);
            this.cmbDefaultEBFileSetting.Name = "cmbDefaultEBFileSetting";
            this.cmbDefaultEBFileSetting.Required = false;
            this.cmbDefaultEBFileSetting.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbDefaultEBFileSetting.Size = new System.Drawing.Size(250, 22);
            this.cmbDefaultEBFileSetting.TabIndex = 1;
            this.cmbDefaultEBFileSetting.ValueMember = null;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // lblYear
            // 
            this.lblYear.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.Location = new System.Drawing.Point(550, 25);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(53, 16);
            this.lblYear.TabIndex = 0;
            this.lblYear.Text = "入金年";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdImportItems
            // 
            this.grdImportItems.AllowAutoExtend = true;
            this.grdImportItems.AllowUserToAddRows = false;
            this.grdImportItems.AllowUserToShiftSelect = true;
            this.grdImportItems.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grdImportItems.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdImportItems.Location = new System.Drawing.Point(9, 51);
            this.grdImportItems.Margin = new System.Windows.Forms.Padding(9, 3, 9, 6);
            this.grdImportItems.Name = "grdImportItems";
            this.grdImportItems.Size = new System.Drawing.Size(966, 229);
            this.grdImportItems.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdImportItems.TabIndex = 0;
            // 
            // lblDefaultEBFileSetting
            // 
            this.lblDefaultEBFileSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultEBFileSetting.Location = new System.Drawing.Point(60, 25);
            this.lblDefaultEBFileSetting.Name = "lblDefaultEBFileSetting";
            this.lblDefaultEBFileSetting.Size = new System.Drawing.Size(140, 16);
            this.lblDefaultEBFileSetting.TabIndex = 0;
            this.lblDefaultEBFileSetting.Text = "標準EBファイル設定";
            this.lblDefaultEBFileSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datYear
            // 
            this.datYear.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datYear.DropDownCalendar.CalendarType = GrapeCity.Win.Editors.CalendarType.YearMonth;
            this.datYear.Enabled = false;
            this.datYear.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datYear.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datYear.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datYear.InputDateType = Rac.VOne.Client.Common.DateType.Year;
            this.datYear.Location = new System.Drawing.Point(609, 22);
            this.datYear.Name = "datYear";
            this.datYear.Required = false;
            this.datYear.Size = new System.Drawing.Size(90, 22);
            this.datYear.Spin.AllowSpin = false;
            this.datYear.TabIndex = 2;
            this.datYear.Value = new System.DateTime(2016, 6, 20, 0, 0, 0, 0);
            // 
            // btnShowOpenFilesDialog
            // 
            this.btnShowOpenFilesDialog.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowOpenFilesDialog.Location = new System.Drawing.Point(787, 21);
            this.btnShowOpenFilesDialog.Name = "btnShowOpenFilesDialog";
            this.btnShowOpenFilesDialog.Size = new System.Drawing.Size(76, 24);
            this.btnShowOpenFilesDialog.TabIndex = 3;
            this.btnShowOpenFilesDialog.Text = "ファイル指定";
            this.btnShowOpenFilesDialog.UseVisualStyleBackColor = true;
            // 
            // gbxHistory
            // 
            this.gbxHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxHistory.Controls.Add(this.grdHistory);
            this.gbxHistory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxHistory.Location = new System.Drawing.Point(12, 310);
            this.gbxHistory.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.gbxHistory.Name = "gbxHistory";
            this.gbxHistory.Size = new System.Drawing.Size(984, 296);
            this.gbxHistory.TabIndex = 7;
            this.gbxHistory.TabStop = false;
            this.gbxHistory.Text = "□　取込履歴";
            // 
            // grdHistory
            // 
            this.grdHistory.AllowAutoExtend = true;
            this.grdHistory.AllowUserToAddRows = false;
            this.grdHistory.AllowUserToShiftSelect = true;
            this.grdHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHistory.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdHistory.Location = new System.Drawing.Point(9, 22);
            this.grdHistory.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(966, 268);
            this.grdHistory.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdHistory.TabIndex = 0;
            // 
            // PD0101
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxHistory);
            this.Controls.Add(this.gbxImport);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.Name = "PD0101";
            this.gbxImport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbDefaultEBFileSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdImportItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datYear)).EndInit();
            this.gbxHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxImport;
        private Common.Controls.VOneComboControl cmbDefaultEBFileSetting;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneLabelControl lblYear;
        private System.Windows.Forms.Button btnShowOpenFilesDialog;
        private Common.Controls.VOneDateControl datYear;
        private Common.Controls.VOneLabelControl lblDefaultEBFileSetting;
        private Common.Controls.VOneGridControl grdImportItems;
        private System.Windows.Forms.GroupBox gbxHistory;
        private Common.Controls.VOneGridControl grdHistory;
    }
}
