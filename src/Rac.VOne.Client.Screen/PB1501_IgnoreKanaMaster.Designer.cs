namespace Rac.VOne.Client.Screen
{
    partial class PB1501
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox gbxGrid;
            System.Windows.Forms.GroupBox gbxEntry;
            this.grdExcludeKanaList = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.txtExcludeCategoryCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtKana = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnSearchExcludeCategory = new System.Windows.Forms.Button();
            this.lblKana = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExcludeCategoryName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblExcludeCategoryCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            gbxGrid = new System.Windows.Forms.GroupBox();
            gbxEntry = new System.Windows.Forms.GroupBox();
            gbxGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExcludeKanaList)).BeginInit();
            gbxEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeCategoryName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxGrid
            // 
            gbxGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            gbxGrid.Controls.Add(this.grdExcludeKanaList);
            gbxGrid.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            gbxGrid.Location = new System.Drawing.Point(15, 12);
            gbxGrid.Name = "gbxGrid";
            gbxGrid.Size = new System.Drawing.Size(978, 517);
            gbxGrid.TabIndex = 1;
            gbxGrid.TabStop = false;
            gbxGrid.Text = "□　除外カナ";
            // 
            // grdExcludeKanaList
            // 
            this.grdExcludeKanaList.AllowAutoExtend = true;
            this.grdExcludeKanaList.AllowUserToAddRows = false;
            this.grdExcludeKanaList.AllowUserToShiftSelect = true;
            this.grdExcludeKanaList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdExcludeKanaList.Location = new System.Drawing.Point(12, 22);
            this.grdExcludeKanaList.Name = "grdExcludeKanaList";
            this.grdExcludeKanaList.Size = new System.Drawing.Size(954, 489);
            this.grdExcludeKanaList.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdExcludeKanaList.TabIndex = 0;
            this.grdExcludeKanaList.TabStop = false;
            this.grdExcludeKanaList.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdExcludeKanaList_CellDoubleClick);
            // 
            // gbxEntry
            // 
            gbxEntry.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            gbxEntry.Controls.Add(this.txtExcludeCategoryCode);
            gbxEntry.Controls.Add(this.txtKana);
            gbxEntry.Controls.Add(this.btnSearchExcludeCategory);
            gbxEntry.Controls.Add(this.lblKana);
            gbxEntry.Controls.Add(this.lblExcludeCategoryName);
            gbxEntry.Controls.Add(this.lblExcludeCategoryCode);
            gbxEntry.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            gbxEntry.Location = new System.Drawing.Point(15, 535);
            gbxEntry.Name = "gbxEntry";
            gbxEntry.Size = new System.Drawing.Size(978, 74);
            gbxEntry.TabIndex = 0;
            gbxEntry.TabStop = false;
            // 
            // txtExcludeCategoryCode
            // 
            this.txtExcludeCategoryCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtExcludeCategoryCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtExcludeCategoryCode.DropDown.AllowDrop = false;
            this.txtExcludeCategoryCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtExcludeCategoryCode.Format = "9";
            this.txtExcludeCategoryCode.HighlightText = true;
            this.txtExcludeCategoryCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtExcludeCategoryCode.Location = new System.Drawing.Point(156, 44);
            this.txtExcludeCategoryCode.MaxLength = 2;
            this.txtExcludeCategoryCode.Name = "txtExcludeCategoryCode";
            this.txtExcludeCategoryCode.PaddingChar = '0';
            this.txtExcludeCategoryCode.Required = true;
            this.txtExcludeCategoryCode.Size = new System.Drawing.Size(30, 22);
            this.txtExcludeCategoryCode.TabIndex = 3;
            this.txtExcludeCategoryCode.Validated += new System.EventHandler(this.txtExcludeCategoryCode_Validated);
            // 
            // txtKana
            // 
            this.txtKana.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtKana.DropDown.AllowDrop = false;
            this.txtKana.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtKana.Format = "@NA9";
            this.txtKana.HighlightText = true;
            this.txtKana.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtKana.Location = new System.Drawing.Point(156, 16);
            this.txtKana.Name = "txtKana";
            this.txtKana.Required = true;
            this.txtKana.Size = new System.Drawing.Size(762, 22);
            this.txtKana.TabIndex = 1;
            this.txtKana.Validated += new System.EventHandler(this.txtKana_Validated);
            // 
            // btnSearchExcludeCategory
            // 
            this.btnSearchExcludeCategory.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSearchExcludeCategory.Location = new System.Drawing.Point(192, 43);
            this.btnSearchExcludeCategory.Name = "btnSearchExcludeCategory";
            this.btnSearchExcludeCategory.Size = new System.Drawing.Size(24, 24);
            this.btnSearchExcludeCategory.TabIndex = 4;
            this.btnSearchExcludeCategory.UseVisualStyleBackColor = true;
            this.btnSearchExcludeCategory.Click += new System.EventHandler(this.btnSearchExcludeCategory_Click);
            // 
            // lblKana
            // 
            this.lblKana.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKana.Location = new System.Drawing.Point(57, 19);
            this.lblKana.Name = "lblKana";
            this.lblKana.Size = new System.Drawing.Size(93, 16);
            this.lblKana.TabIndex = 0;
            this.lblKana.Text = "カナ名";
            this.lblKana.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExcludeCategoryName
            // 
            this.lblExcludeCategoryName.AutoSize = true;
            this.lblExcludeCategoryName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblExcludeCategoryName.Enabled = false;
            this.lblExcludeCategoryName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExcludeCategoryName.HighlightText = true;
            this.lblExcludeCategoryName.Location = new System.Drawing.Point(222, 45);
            this.lblExcludeCategoryName.Name = "lblExcludeCategoryName";
            this.lblExcludeCategoryName.ReadOnly = true;
            this.lblExcludeCategoryName.Required = false;
            this.lblExcludeCategoryName.Size = new System.Drawing.Size(696, 21);
            this.lblExcludeCategoryName.TabIndex = 5;
            // 
            // lblExcludeCategoryCode
            // 
            this.lblExcludeCategoryCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExcludeCategoryCode.Location = new System.Drawing.Point(57, 47);
            this.lblExcludeCategoryCode.Name = "lblExcludeCategoryCode";
            this.lblExcludeCategoryCode.Size = new System.Drawing.Size(93, 16);
            this.lblExcludeCategoryCode.TabIndex = 2;
            this.lblExcludeCategoryCode.Text = "対象外区分コード";
            this.lblExcludeCategoryCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PB1501
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(gbxEntry);
            this.Controls.Add(gbxGrid);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB1501";
            this.Load += new System.EventHandler(this.PB1501_Load);
            gbxGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExcludeKanaList)).EndInit();
            gbxEntry.ResumeLayout(false);
            gbxEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcludeCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblExcludeCategoryName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneLabelControl lblKana;
        private Common.Controls.VOneTextControl txtExcludeCategoryCode;
        private Common.Controls.VOneTextControl txtKana;
        private Common.Controls.VOneLabelControl lblExcludeCategoryCode;
        private System.Windows.Forms.Button btnSearchExcludeCategory;
        private Common.Controls.VOneDispLabelControl lblExcludeCategoryName;
        private Common.Controls.VOneGridControl grdExcludeKanaList;
    }
}
