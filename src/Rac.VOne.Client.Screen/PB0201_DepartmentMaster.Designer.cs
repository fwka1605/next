namespace Rac.VOne.Client.Screen
{
    partial class PB0201
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
            this.gbxDepartmentInput = new System.Windows.Forms.GroupBox();
            this.txtNote = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtDepartmentCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblDepartmentCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtStaffCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblStaffCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblStaffName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtDepartmentName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblDepartmentName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblNote = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxDepartmentList = new System.Windows.Forms.GroupBox();
            this.grdDepartment = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxDepartmentInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            this.gbxDepartmentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartment)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxDepartmentInput
            // 
            this.gbxDepartmentInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxDepartmentInput.Controls.Add(this.txtNote);
            this.gbxDepartmentInput.Controls.Add(this.txtDepartmentCode);
            this.gbxDepartmentInput.Controls.Add(this.btnSearch);
            this.gbxDepartmentInput.Controls.Add(this.lblDepartmentCode);
            this.gbxDepartmentInput.Controls.Add(this.txtStaffCode);
            this.gbxDepartmentInput.Controls.Add(this.lblStaffCode);
            this.gbxDepartmentInput.Controls.Add(this.lblStaffName);
            this.gbxDepartmentInput.Controls.Add(this.txtDepartmentName);
            this.gbxDepartmentInput.Controls.Add(this.lblDepartmentName);
            this.gbxDepartmentInput.Controls.Add(this.lblNote);
            this.gbxDepartmentInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxDepartmentInput.Location = new System.Drawing.Point(150, 495);
            this.gbxDepartmentInput.Name = "gbxDepartmentInput";
            this.gbxDepartmentInput.Size = new System.Drawing.Size(708, 112);
            this.gbxDepartmentInput.TabIndex = 0;
            this.gbxDepartmentInput.TabStop = false;
            // 
            // txtNote
            // 
            this.txtNote.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtNote.DropDown.AllowDrop = false;
            this.txtNote.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtNote.HighlightText = true;
            this.txtNote.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtNote.Location = new System.Drawing.Point(112, 77);
            this.txtNote.MaxLength = 100;
            this.txtNote.Name = "txtNote";
            this.txtNote.Required = false;
            this.txtNote.Size = new System.Drawing.Size(576, 22);
            this.txtNote.TabIndex = 6;
            // 
            // txtDepartmentCode
            // 
            this.txtDepartmentCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtDepartmentCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDepartmentCode.DropDown.AllowDrop = false;
            this.txtDepartmentCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtDepartmentCode.HighlightText = true;
            this.txtDepartmentCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtDepartmentCode.Location = new System.Drawing.Point(112, 19);
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Required = true;
            this.txtDepartmentCode.Size = new System.Drawing.Size(115, 22);
            this.txtDepartmentCode.TabIndex = 1;
            this.txtDepartmentCode.Validated += new System.EventHandler(this.txtDepartmentCode_Validated);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSearch.Location = new System.Drawing.Point(233, 48);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(24, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblDepartmentCode.Location = new System.Drawing.Point(25, 21);
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Size = new System.Drawing.Size(81, 16);
            this.lblDepartmentCode.TabIndex = 0;
            this.lblDepartmentCode.Text = "請求部門コード";
            this.lblDepartmentCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtStaffCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtStaffCode.DropDown.AllowDrop = false;
            this.txtStaffCode.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtStaffCode.HighlightText = true;
            this.txtStaffCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtStaffCode.Location = new System.Drawing.Point(112, 48);
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.Required = false;
            this.txtStaffCode.Size = new System.Drawing.Size(115, 22);
            this.txtStaffCode.TabIndex = 3;
            this.txtStaffCode.Validated += new System.EventHandler(this.txtStaffCode_Validated);
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblStaffCode.Location = new System.Drawing.Point(25, 50);
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Size = new System.Drawing.Size(81, 16);
            this.lblStaffCode.TabIndex = 0;
            this.lblStaffCode.Text = "回収責任者";
            this.lblStaffCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStaffName
            // 
            this.lblStaffName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStaffName.DropDown.AllowDrop = false;
            this.lblStaffName.Enabled = false;
            this.lblStaffName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblStaffName.HighlightText = true;
            this.lblStaffName.Location = new System.Drawing.Point(263, 48);
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.ReadOnly = true;
            this.lblStaffName.Required = false;
            this.lblStaffName.Size = new System.Drawing.Size(425, 22);
            this.lblStaffName.TabIndex = 5;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDepartmentName.DropDown.AllowDrop = false;
            this.txtDepartmentName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtDepartmentName.HighlightText = true;
            this.txtDepartmentName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtDepartmentName.Location = new System.Drawing.Point(313, 19);
            this.txtDepartmentName.MaxLength = 40;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Required = true;
            this.txtDepartmentName.Size = new System.Drawing.Size(375, 22);
            this.txtDepartmentName.TabIndex = 2;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblDepartmentName.Location = new System.Drawing.Point(240, 21);
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Size = new System.Drawing.Size(67, 16);
            this.lblDepartmentName.TabIndex = 0;
            this.lblDepartmentName.Text = "請求部門名";
            this.lblDepartmentName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblNote.Location = new System.Drawing.Point(25, 79);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(81, 16);
            this.lblNote.TabIndex = 0;
            this.lblNote.Text = "備考";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxDepartmentList
            // 
            this.gbxDepartmentList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxDepartmentList.Controls.Add(this.grdDepartment);
            this.gbxDepartmentList.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.gbxDepartmentList.Location = new System.Drawing.Point(150, 15);
            this.gbxDepartmentList.Name = "gbxDepartmentList";
            this.gbxDepartmentList.Size = new System.Drawing.Size(708, 474);
            this.gbxDepartmentList.TabIndex = 1;
            this.gbxDepartmentList.TabStop = false;
            this.gbxDepartmentList.Text = "□　登録済みの請求部門";
            // 
            // grdDepartment
            // 
            this.grdDepartment.AllowAutoExtend = true;
            this.grdDepartment.AllowUserToAddRows = false;
            this.grdDepartment.AllowUserToShiftSelect = true;
            this.grdDepartment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdDepartment.CurrentCellBorderLine = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.Black);
            this.grdDepartment.Location = new System.Drawing.Point(22, 22);
            this.grdDepartment.Name = "grdDepartment";
            this.grdDepartment.Size = new System.Drawing.Size(666, 442);
            this.grdDepartment.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdDepartment.TabIndex = 0;
            this.grdDepartment.TabStop = false;
            this.grdDepartment.Text = "vOneGridControl1";
            this.grdDepartment.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdDepartment_CellDoubleClick);
            // 
            // PB0201
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxDepartmentInput);
            this.Controls.Add(this.gbxDepartmentList);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "PB0201";
            this.Load += new System.EventHandler(this.PB0201_Load);
            this.gbxDepartmentInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            this.gbxDepartmentList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxDepartmentInput;
        private Common.Controls.VOneLabelControl lblDepartmentCode;
        private Common.Controls.VOneLabelControl lblStaffCode;
        private Common.Controls.VOneLabelControl lblNote;
        private Common.Controls.VOneLabelControl lblDepartmentName;
        private Common.Controls.VOneTextControl txtDepartmentCode;
        private Common.Controls.VOneTextControl txtStaffCode;
        private System.Windows.Forms.Button btnSearch;
        private Common.Controls.VOneTextControl txtDepartmentName;
        private Common.Controls.VOneDispLabelControl lblStaffName;
        private Common.Controls.VOneTextControl txtNote;
        private System.Windows.Forms.GroupBox gbxDepartmentList;
        private Common.Controls.VOneGridControl grdDepartment;
    }
}
