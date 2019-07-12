namespace Rac.VOne.Client.Screen
{
    partial class PH9904
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
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblExportFileName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.cmbDateFormat = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblDateFormat = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxRequireHeader = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExportFileName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDateFormat)).BeginInit();
            this.SuspendLayout();
            // 
            // grdExportFields
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowDrop = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToResize = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(14, 45);
            this.grid.MultiSelect = false;
            this.grid.Name = "grdExportFields";
            this.grid.Size = new System.Drawing.Size(360, 400);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 1;
            this.grid.Text = "vOneGridControl1";
            // 
            // lblExportFileName
            // 
            this.lblExportFileName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblExportFileName.DropDown.AllowDrop = false;
            this.lblExportFileName.Enabled = false;
            this.lblExportFileName.HighlightText = true;
            this.lblExportFileName.Location = new System.Drawing.Point(14, 15);
            this.lblExportFileName.Name = "lblExportFileName";
            this.lblExportFileName.ReadOnly = true;
            this.lblExportFileName.Required = false;
            this.lblExportFileName.Size = new System.Drawing.Size(360, 22);
            this.lblExportFileName.TabIndex = 0;
            // 
            // cmbDateFormat
            // 
            this.cmbDateFormat.DisplayMember = null;
            this.cmbDateFormat.DropDown.AllowResize = false;
            this.cmbDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateFormat.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbDateFormat.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbDateFormat.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbDateFormat.ListHeaderPane.Height = 22;
            this.cmbDateFormat.ListHeaderPane.Visible = false;
            this.cmbDateFormat.Location = new System.Drawing.Point(240, 453);
            this.cmbDateFormat.Name = "cmbDateFormat";
            this.cmbDateFormat.Required = false;
            this.cmbDateFormat.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbDateFormat.Size = new System.Drawing.Size(134, 24);
            this.cmbDateFormat.TabIndex = 3;
            this.cmbDateFormat.ValueMember = null;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // lblDateFormat
            // 
            this.lblDateFormat.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDateFormat.Location = new System.Drawing.Point(169, 455);
            this.lblDateFormat.Margin = new System.Windows.Forms.Padding(3);
            this.lblDateFormat.Name = "lblDateFormat";
            this.lblDateFormat.Size = new System.Drawing.Size(65, 16);
            this.lblDateFormat.TabIndex = 2;
            this.lblDateFormat.Text = "日付の書式";
            this.lblDateFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxRequireHeader
            // 
            this.cbxRequireHeader.AutoSize = true;
            this.cbxRequireHeader.Location = new System.Drawing.Point(260, 484);
            this.cbxRequireHeader.Name = "cbxRequireHeader";
            this.cbxRequireHeader.Size = new System.Drawing.Size(114, 19);
            this.cbxRequireHeader.TabIndex = 4;
            this.cbxRequireHeader.Text = "項目名を出力する";
            this.cbxRequireHeader.UseVisualStyleBackColor = true;
            // 
            // PH9904
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.cbxRequireHeader);
            this.Controls.Add(this.cmbDateFormat);
            this.Controls.Add(this.lblDateFormat);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblExportFileName);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PH9904";
            this.Size = new System.Drawing.Size(400, 554);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExportFileName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDateFormat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Controls.VOneGridControl grid;
        private Common.Controls.VOneDispLabelControl lblExportFileName;
        private Common.Controls.VOneComboControl cmbDateFormat;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneLabelControl lblDateFormat;
        private System.Windows.Forms.CheckBox cbxRequireHeader;
    }
}
