namespace Rac.VOne.Client.Screen
{
    partial class PB1201
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
            this.lblDepartmentToName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtDepartmentFrom = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtDepartmentTo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblDepartmentCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblDepartmentFromTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnDepartmentTo = new System.Windows.Forms.Button();
            this.btnDepartmentFrom = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblDepartmentFromName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.gbxSectionWithDeptList = new System.Windows.Forms.GroupBox();
            this.lblDepartmentModify = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDepartmentOrigin = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblArrow = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdDepartmentOrigin = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.grdDepartmentModify = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxSectionInput = new System.Windows.Forms.GroupBox();
            this.btnSectionSearch = new System.Windows.Forms.Button();
            this.txtSection = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblSectionCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblPaymentName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblSectionName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxDepartmentInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentToName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentFromName)).BeginInit();
            this.gbxSectionWithDeptList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartmentOrigin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartmentModify)).BeginInit();
            this.gbxSectionInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPaymentName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxDepartmentInput
            // 
            this.gbxDepartmentInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxDepartmentInput.Controls.Add(this.lblDepartmentToName);
            this.gbxDepartmentInput.Controls.Add(this.txtDepartmentFrom);
            this.gbxDepartmentInput.Controls.Add(this.txtDepartmentTo);
            this.gbxDepartmentInput.Controls.Add(this.lblDepartmentCode);
            this.gbxDepartmentInput.Controls.Add(this.btnDelete);
            this.gbxDepartmentInput.Controls.Add(this.lblDepartmentFromTo);
            this.gbxDepartmentInput.Controls.Add(this.btnDepartmentTo);
            this.gbxDepartmentInput.Controls.Add(this.btnDepartmentFrom);
            this.gbxDepartmentInput.Controls.Add(this.btnAdd);
            this.gbxDepartmentInput.Controls.Add(this.lblDepartmentFromName);
            this.gbxDepartmentInput.Controls.Add(this.btnDeleteAll);
            this.gbxDepartmentInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxDepartmentInput.Location = new System.Drawing.Point(45, 496);
            this.gbxDepartmentInput.Name = "gbxDepartmentInput";
            this.gbxDepartmentInput.Size = new System.Drawing.Size(918, 110);
            this.gbxDepartmentInput.TabIndex = 1;
            this.gbxDepartmentInput.TabStop = false;
            // 
            // lblDepartmentToName
            // 
            this.lblDepartmentToName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDepartmentToName.DropDown.AllowDrop = false;
            this.lblDepartmentToName.Enabled = false;
            this.lblDepartmentToName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentToName.HighlightText = true;
            this.lblDepartmentToName.Location = new System.Drawing.Point(378, 47);
            this.lblDepartmentToName.Name = "lblDepartmentToName";
            this.lblDepartmentToName.ReadOnly = true;
            this.lblDepartmentToName.Required = false;
            this.lblDepartmentToName.Size = new System.Drawing.Size(410, 22);
            this.lblDepartmentToName.TabIndex = 10;
            // 
            // txtDepartmentFrom
            // 
            this.txtDepartmentFrom.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtDepartmentFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDepartmentFrom.DropDown.AllowDrop = false;
            this.txtDepartmentFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepartmentFrom.HighlightText = true;
            this.txtDepartmentFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtDepartmentFrom.Location = new System.Drawing.Point(227, 17);
            this.txtDepartmentFrom.Name = "txtDepartmentFrom";
            this.txtDepartmentFrom.Required = true;
            this.txtDepartmentFrom.Size = new System.Drawing.Size(115, 22);
            this.txtDepartmentFrom.TabIndex = 5;
            this.txtDepartmentFrom.Validated += new System.EventHandler(this.txtDepartmentFrom_Validated);
            // 
            // txtDepartmentTo
            // 
            this.txtDepartmentTo.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtDepartmentTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDepartmentTo.DropDown.AllowDrop = false;
            this.txtDepartmentTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepartmentTo.HighlightText = true;
            this.txtDepartmentTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtDepartmentTo.Location = new System.Drawing.Point(227, 47);
            this.txtDepartmentTo.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.txtDepartmentTo.Name = "txtDepartmentTo";
            this.txtDepartmentTo.Required = true;
            this.txtDepartmentTo.Size = new System.Drawing.Size(115, 22);
            this.txtDepartmentTo.TabIndex = 7;
            this.txtDepartmentTo.Validated += new System.EventHandler(this.txtDepartmentTo_Validated);
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentCode.Location = new System.Drawing.Point(140, 19);
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Size = new System.Drawing.Size(81, 16);
            this.lblDepartmentCode.TabIndex = 3;
            this.lblDepartmentCode.Text = "請求部門コード";
            this.lblDepartmentCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(627, 78);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 6, 6, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(76, 24);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblDepartmentFromTo
            // 
            this.lblDepartmentFromTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentFromTo.Location = new System.Drawing.Point(206, 49);
            this.lblDepartmentFromTo.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblDepartmentFromTo.Name = "lblDepartmentFromTo";
            this.lblDepartmentFromTo.Size = new System.Drawing.Size(20, 16);
            this.lblDepartmentFromTo.TabIndex = 4;
            this.lblDepartmentFromTo.Text = "～";
            this.lblDepartmentFromTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDepartmentTo
            // 
            this.btnDepartmentTo.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnDepartmentTo.Location = new System.Drawing.Point(348, 46);
            this.btnDepartmentTo.Name = "btnDepartmentTo";
            this.btnDepartmentTo.Size = new System.Drawing.Size(24, 24);
            this.btnDepartmentTo.TabIndex = 8;
            this.btnDepartmentTo.UseVisualStyleBackColor = true;
            this.btnDepartmentTo.Click += new System.EventHandler(this.btnDepartmentTo_Click);
            // 
            // btnDepartmentFrom
            // 
            this.btnDepartmentFrom.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnDepartmentFrom.Location = new System.Drawing.Point(348, 16);
            this.btnDepartmentFrom.Name = "btnDepartmentFrom";
            this.btnDepartmentFrom.Size = new System.Drawing.Size(24, 24);
            this.btnDepartmentFrom.TabIndex = 6;
            this.btnDepartmentFrom.UseVisualStyleBackColor = true;
            this.btnDepartmentFrom.Click += new System.EventHandler(this.btnDepartmentFrom_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(542, 78);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 6, 6, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(76, 24);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblDepartmentFromName
            // 
            this.lblDepartmentFromName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDepartmentFromName.DropDown.AllowDrop = false;
            this.lblDepartmentFromName.Enabled = false;
            this.lblDepartmentFromName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentFromName.HighlightText = true;
            this.lblDepartmentFromName.Location = new System.Drawing.Point(378, 17);
            this.lblDepartmentFromName.Name = "lblDepartmentFromName";
            this.lblDepartmentFromName.ReadOnly = true;
            this.lblDepartmentFromName.Required = false;
            this.lblDepartmentFromName.Size = new System.Drawing.Size(410, 22);
            this.lblDepartmentFromName.TabIndex = 9;
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Location = new System.Drawing.Point(712, 78);
            this.btnDeleteAll.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(76, 24);
            this.btnDeleteAll.TabIndex = 11;
            this.btnDeleteAll.Text = "一括削除";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // gbxSectionWithDeptList
            // 
            this.gbxSectionWithDeptList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxSectionWithDeptList.Controls.Add(this.lblDepartmentModify);
            this.gbxSectionWithDeptList.Controls.Add(this.lblDepartmentOrigin);
            this.gbxSectionWithDeptList.Controls.Add(this.lblArrow);
            this.gbxSectionWithDeptList.Controls.Add(this.grdDepartmentOrigin);
            this.gbxSectionWithDeptList.Controls.Add(this.grdDepartmentModify);
            this.gbxSectionWithDeptList.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxSectionWithDeptList.Location = new System.Drawing.Point(45, 101);
            this.gbxSectionWithDeptList.Name = "gbxSectionWithDeptList";
            this.gbxSectionWithDeptList.Padding = new System.Windows.Forms.Padding(6, 0, 6, 3);
            this.gbxSectionWithDeptList.Size = new System.Drawing.Size(918, 389);
            this.gbxSectionWithDeptList.TabIndex = 2;
            this.gbxSectionWithDeptList.TabStop = false;
            // 
            // lblDepartmentModify
            // 
            this.lblDepartmentModify.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDepartmentModify.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentModify.Location = new System.Drawing.Point(474, 19);
            this.lblDepartmentModify.Name = "lblDepartmentModify";
            this.lblDepartmentModify.Size = new System.Drawing.Size(122, 16);
            this.lblDepartmentModify.TabIndex = 1;
            this.lblDepartmentModify.Text = "□　変更後の請求部門";
            this.lblDepartmentModify.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDepartmentOrigin
            // 
            this.lblDepartmentOrigin.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartmentOrigin.Location = new System.Drawing.Point(9, 19);
            this.lblDepartmentOrigin.Name = "lblDepartmentOrigin";
            this.lblDepartmentOrigin.Size = new System.Drawing.Size(122, 16);
            this.lblDepartmentOrigin.TabIndex = 0;
            this.lblDepartmentOrigin.Text = "□　変更前の請求部門";
            this.lblDepartmentOrigin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblArrow
            // 
            this.lblArrow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblArrow.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArrow.Location = new System.Drawing.Point(450, 211);
            this.lblArrow.Name = "lblArrow";
            this.lblArrow.Size = new System.Drawing.Size(20, 16);
            this.lblArrow.TabIndex = 4;
            this.lblArrow.Text = "➡";
            this.lblArrow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grdDepartmentOrigin
            // 
            this.grdDepartmentOrigin.AllowAutoExtend = true;
            this.grdDepartmentOrigin.AllowUserToAddRows = false;
            this.grdDepartmentOrigin.AllowUserToShiftSelect = true;
            this.grdDepartmentOrigin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdDepartmentOrigin.Location = new System.Drawing.Point(9, 41);
            this.grdDepartmentOrigin.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grdDepartmentOrigin.Name = "grdDepartmentOrigin";
            this.grdDepartmentOrigin.Size = new System.Drawing.Size(432, 339);
            this.grdDepartmentOrigin.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdDepartmentOrigin.TabIndex = 1;
            this.grdDepartmentOrigin.TabStop = false;
            this.grdDepartmentOrigin.Text = "vOneGridControl1";
            // 
            // grdDepartmentModify
            // 
            this.grdDepartmentModify.AllowAutoExtend = true;
            this.grdDepartmentModify.AllowUserToAddRows = false;
            this.grdDepartmentModify.AllowUserToShiftSelect = true;
            this.grdDepartmentModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdDepartmentModify.Location = new System.Drawing.Point(477, 41);
            this.grdDepartmentModify.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grdDepartmentModify.Name = "grdDepartmentModify";
            this.grdDepartmentModify.Size = new System.Drawing.Size(432, 339);
            this.grdDepartmentModify.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdDepartmentModify.TabIndex = 3;
            this.grdDepartmentModify.TabStop = false;
            this.grdDepartmentModify.Text = "vOneGridControl2";
            this.grdDepartmentModify.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdDepartmentModify_CellDoubleClick);
            // 
            // gbxSectionInput
            // 
            this.gbxSectionInput.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxSectionInput.Controls.Add(this.btnSectionSearch);
            this.gbxSectionInput.Controls.Add(this.txtSection);
            this.gbxSectionInput.Controls.Add(this.lblSectionCode);
            this.gbxSectionInput.Controls.Add(this.lblPaymentName);
            this.gbxSectionInput.Controls.Add(this.lblSectionName);
            this.gbxSectionInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxSectionInput.Location = new System.Drawing.Point(45, 15);
            this.gbxSectionInput.Name = "gbxSectionInput";
            this.gbxSectionInput.Size = new System.Drawing.Size(918, 80);
            this.gbxSectionInput.TabIndex = 0;
            this.gbxSectionInput.TabStop = false;
            // 
            // btnSectionSearch
            // 
            this.btnSectionSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSectionSearch.Location = new System.Drawing.Point(348, 21);
            this.btnSectionSearch.Name = "btnSectionSearch";
            this.btnSectionSearch.Size = new System.Drawing.Size(24, 24);
            this.btnSectionSearch.TabIndex = 4;
            this.btnSectionSearch.UseVisualStyleBackColor = true;
            this.btnSectionSearch.Click += new System.EventHandler(this.btnSectionSearch_Click);
            // 
            // txtSection
            // 
            this.txtSection.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtSection.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSection.DropDown.AllowDrop = false;
            this.txtSection.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSection.Format = "A9K-";
            this.txtSection.HighlightText = true;
            this.txtSection.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSection.Location = new System.Drawing.Point(227, 22);
            this.txtSection.Name = "txtSection";
            this.txtSection.Required = true;
            this.txtSection.Size = new System.Drawing.Size(115, 22);
            this.txtSection.TabIndex = 1;
            this.txtSection.Validated += new System.EventHandler(this.txtSection_Validated);
            // 
            // lblSectionCode
            // 
            this.lblSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionCode.Location = new System.Drawing.Point(140, 24);
            this.lblSectionCode.Name = "lblSectionCode";
            this.lblSectionCode.Size = new System.Drawing.Size(81, 16);
            this.lblSectionCode.TabIndex = 0;
            this.lblSectionCode.Text = "入金部門コード";
            this.lblSectionCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPaymentName
            // 
            this.lblPaymentName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPaymentName.DropDown.AllowDrop = false;
            this.lblPaymentName.Enabled = false;
            this.lblPaymentName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentName.HighlightText = true;
            this.lblPaymentName.Location = new System.Drawing.Point(227, 50);
            this.lblPaymentName.Name = "lblPaymentName";
            this.lblPaymentName.ReadOnly = true;
            this.lblPaymentName.Required = false;
            this.lblPaymentName.Size = new System.Drawing.Size(500, 22);
            this.lblPaymentName.TabIndex = 2;
            // 
            // lblSectionName
            // 
            this.lblSectionName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionName.Location = new System.Drawing.Point(140, 52);
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.Size = new System.Drawing.Size(81, 16);
            this.lblSectionName.TabIndex = 1;
            this.lblSectionName.Text = "入金部門名";
            this.lblSectionName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PB1201
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxDepartmentInput);
            this.Controls.Add(this.gbxSectionWithDeptList);
            this.Controls.Add(this.gbxSectionInput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB1201";
            this.Load += new System.EventHandler(this.PB1201_Load);
            this.gbxDepartmentInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentToName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentFromName)).EndInit();
            this.gbxSectionWithDeptList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartmentOrigin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartmentModify)).EndInit();
            this.gbxSectionInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPaymentName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxSectionWithDeptList;
        private Common.Controls.VOneLabelControl lblDepartmentOrigin;
        private Common.Controls.VOneLabelControl lblDepartmentModify;
        private Common.Controls.VOneGridControl grdDepartmentOrigin;
        private Common.Controls.VOneGridControl grdDepartmentModify;
        private Common.Controls.VOneLabelControl lblArrow;
        private System.Windows.Forms.GroupBox gbxDepartmentInput;
        private Common.Controls.VOneDispLabelControl lblDepartmentToName;
        private Common.Controls.VOneDispLabelControl lblDepartmentFromName;
        private Common.Controls.VOneLabelControl lblDepartmentCode;
        private Common.Controls.VOneLabelControl lblDepartmentFromTo;
        private Common.Controls.VOneTextControl txtDepartmentFrom;
        private Common.Controls.VOneTextControl txtDepartmentTo;
        private System.Windows.Forms.Button btnDepartmentFrom;
        private System.Windows.Forms.Button btnDepartmentTo;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox gbxSectionInput;
        private System.Windows.Forms.Button btnSectionSearch;
        private Common.Controls.VOneDispLabelControl lblPaymentName;
        private Common.Controls.VOneTextControl txtSection;
        private Common.Controls.VOneLabelControl lblSectionCode;
        private Common.Controls.VOneLabelControl lblSectionName;
    }
}
