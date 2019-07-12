namespace Rac.VOne.Client.Screen
{
    partial class PB0601
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PB0601));
            this.gbxChildCustomerInput = new System.Windows.Forms.GroupBox();
            this.lblCustomerNameTo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCustomerNameFrom = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblCustomer = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCustomerTo = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblCustomerFromTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCustomerFrom = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.txtCustomerCodeFrom = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtCustomerCodeTo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.gbxParentCustomerInput = new System.Windows.Forms.GroupBox();
            this.btnParentCustomerSearch = new System.Windows.Forms.Button();
            this.txtParentCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblParentCustomerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblParentCustomerKana = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblParentCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxCustomerGroupList = new System.Windows.Forms.GroupBox();
            this.lblCustomerModify = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdCustomerModify = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblCustomerOrigin = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdCustomerOrigin = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblArrow = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxChildCustomerInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeTo)).BeginInit();
            this.gbxParentCustomerInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentCustomerKana)).BeginInit();
            this.gbxCustomerGroupList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCustomerModify)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCustomerOrigin)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxChildCustomerInput
            // 
            this.gbxChildCustomerInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxChildCustomerInput.Controls.Add(this.lblCustomerNameTo);
            this.gbxChildCustomerInput.Controls.Add(this.lblCustomerNameFrom);
            this.gbxChildCustomerInput.Controls.Add(this.btnDelete);
            this.gbxChildCustomerInput.Controls.Add(this.lblCustomer);
            this.gbxChildCustomerInput.Controls.Add(this.btnCustomerTo);
            this.gbxChildCustomerInput.Controls.Add(this.btnAdd);
            this.gbxChildCustomerInput.Controls.Add(this.lblCustomerFromTo);
            this.gbxChildCustomerInput.Controls.Add(this.btnCustomerFrom);
            this.gbxChildCustomerInput.Controls.Add(this.btnDeleteAll);
            this.gbxChildCustomerInput.Controls.Add(this.txtCustomerCodeFrom);
            this.gbxChildCustomerInput.Controls.Add(this.txtCustomerCodeTo);
            this.gbxChildCustomerInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxChildCustomerInput.Location = new System.Drawing.Point(45, 496);
            this.gbxChildCustomerInput.Name = "gbxChildCustomerInput";
            this.gbxChildCustomerInput.Padding = new System.Windows.Forms.Padding(0);
            this.gbxChildCustomerInput.Size = new System.Drawing.Size(918, 110);
            this.gbxChildCustomerInput.TabIndex = 1;
            this.gbxChildCustomerInput.TabStop = false;
            // 
            // lblCustomerNameTo
            // 
            this.lblCustomerNameTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerNameTo.DropDown.AllowDrop = false;
            this.lblCustomerNameTo.Enabled = false;
            this.lblCustomerNameTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNameTo.HighlightText = true;
            this.lblCustomerNameTo.Location = new System.Drawing.Point(368, 47);
            this.lblCustomerNameTo.Name = "lblCustomerNameTo";
            this.lblCustomerNameTo.ReadOnly = true;
            this.lblCustomerNameTo.Required = false;
            this.lblCustomerNameTo.Size = new System.Drawing.Size(410, 22);
            this.lblCustomerNameTo.TabIndex = 7;
            // 
            // lblCustomerNameFrom
            // 
            this.lblCustomerNameFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerNameFrom.DropDown.AllowDrop = false;
            this.lblCustomerNameFrom.Enabled = false;
            this.lblCustomerNameFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNameFrom.HighlightText = true;
            this.lblCustomerNameFrom.Location = new System.Drawing.Point(368, 17);
            this.lblCustomerNameFrom.Name = "lblCustomerNameFrom";
            this.lblCustomerNameFrom.ReadOnly = true;
            this.lblCustomerNameFrom.Required = false;
            this.lblCustomerNameFrom.Size = new System.Drawing.Size(410, 22);
            this.lblCustomerNameFrom.TabIndex = 3;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(617, 78);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 6, 6, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(76, 24);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblCustomer
            // 
            this.lblCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(142, 19);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(3);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(69, 16);
            this.lblCustomer.TabIndex = 0;
            this.lblCustomer.Text = "得意先コード";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCustomerTo
            // 
            this.btnCustomerTo.Image = ((System.Drawing.Image)(resources.GetObject("btnCustomerTo.Image")));
            this.btnCustomerTo.Location = new System.Drawing.Point(338, 45);
            this.btnCustomerTo.Name = "btnCustomerTo";
            this.btnCustomerTo.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerTo.TabIndex = 6;
            this.btnCustomerTo.UseVisualStyleBackColor = true;
            this.btnCustomerTo.Click += new System.EventHandler(this.btnCustomerTo_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(532, 78);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 6, 6, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(76, 24);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblCustomerFromTo
            // 
            this.lblCustomerFromTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerFromTo.Location = new System.Drawing.Point(196, 49);
            this.lblCustomerFromTo.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblCustomerFromTo.Name = "lblCustomerFromTo";
            this.lblCustomerFromTo.Size = new System.Drawing.Size(20, 16);
            this.lblCustomerFromTo.TabIndex = 4;
            this.lblCustomerFromTo.Text = "～";
            this.lblCustomerFromTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCustomerFrom
            // 
            this.btnCustomerFrom.Image = ((System.Drawing.Image)(resources.GetObject("btnCustomerFrom.Image")));
            this.btnCustomerFrom.Location = new System.Drawing.Point(338, 15);
            this.btnCustomerFrom.Name = "btnCustomerFrom";
            this.btnCustomerFrom.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerFrom.TabIndex = 2;
            this.btnCustomerFrom.UseVisualStyleBackColor = true;
            this.btnCustomerFrom.Click += new System.EventHandler(this.btnCustomerFrom_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Location = new System.Drawing.Point(702, 78);
            this.btnDeleteAll.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(76, 24);
            this.btnDeleteAll.TabIndex = 10;
            this.btnDeleteAll.Text = "一括削除";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // txtCustomerCodeFrom
            // 
            this.txtCustomerCodeFrom.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCodeFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerCodeFrom.DropDown.AllowDrop = false;
            this.txtCustomerCodeFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerCodeFrom.HighlightText = true;
            this.txtCustomerCodeFrom.Location = new System.Drawing.Point(217, 17);
            this.txtCustomerCodeFrom.Name = "txtCustomerCodeFrom";
            this.txtCustomerCodeFrom.Required = true;
            this.txtCustomerCodeFrom.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCodeFrom.TabIndex = 1;
            this.txtCustomerCodeFrom.Validated += new System.EventHandler(this.txtCustomerFrom_Validated);
            // 
            // txtCustomerCodeTo
            // 
            this.txtCustomerCodeTo.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCodeTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerCodeTo.DropDown.AllowDrop = false;
            this.txtCustomerCodeTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerCodeTo.HighlightText = true;
            this.txtCustomerCodeTo.Location = new System.Drawing.Point(217, 47);
            this.txtCustomerCodeTo.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.txtCustomerCodeTo.Name = "txtCustomerCodeTo";
            this.txtCustomerCodeTo.Required = true;
            this.txtCustomerCodeTo.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCodeTo.TabIndex = 5;
            this.txtCustomerCodeTo.Validated += new System.EventHandler(this.txtCustomerTo_Validated);
            // 
            // gbxParentCustomerInput
            // 
            this.gbxParentCustomerInput.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxParentCustomerInput.Controls.Add(this.btnParentCustomerSearch);
            this.gbxParentCustomerInput.Controls.Add(this.txtParentCustomerCode);
            this.gbxParentCustomerInput.Controls.Add(this.lblParentCustomerName);
            this.gbxParentCustomerInput.Controls.Add(this.lblParentCustomerKana);
            this.gbxParentCustomerInput.Controls.Add(this.lblParentCustomerCode);
            this.gbxParentCustomerInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxParentCustomerInput.Location = new System.Drawing.Point(45, 15);
            this.gbxParentCustomerInput.Name = "gbxParentCustomerInput";
            this.gbxParentCustomerInput.Size = new System.Drawing.Size(918, 80);
            this.gbxParentCustomerInput.TabIndex = 0;
            this.gbxParentCustomerInput.TabStop = false;
            // 
            // btnParentCustomerSearch
            // 
            this.btnParentCustomerSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnParentCustomerSearch.Image")));
            this.btnParentCustomerSearch.Location = new System.Drawing.Point(380, 18);
            this.btnParentCustomerSearch.Name = "btnParentCustomerSearch";
            this.btnParentCustomerSearch.Size = new System.Drawing.Size(24, 24);
            this.btnParentCustomerSearch.TabIndex = 2;
            this.btnParentCustomerSearch.UseVisualStyleBackColor = true;
            this.btnParentCustomerSearch.Click += new System.EventHandler(this.btnCustomerGroupSearch_Click);
            // 
            // txtParentCustomerCode
            // 
            this.txtParentCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtParentCustomerCode.DropDown.AllowDrop = false;
            this.txtParentCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParentCustomerCode.Format = "A9K-";
            this.txtParentCustomerCode.HighlightText = true;
            this.txtParentCustomerCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtParentCustomerCode.Location = new System.Drawing.Point(259, 20);
            this.txtParentCustomerCode.Name = "txtParentCustomerCode";
            this.txtParentCustomerCode.Required = true;
            this.txtParentCustomerCode.Size = new System.Drawing.Size(115, 22);
            this.txtParentCustomerCode.TabIndex = 1;
            this.txtParentCustomerCode.Validated += new System.EventHandler(this.txtParentCustomerCode_Validated);
            // 
            // lblParentCustomerName
            // 
            this.lblParentCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParentCustomerName.Location = new System.Drawing.Point(160, 50);
            this.lblParentCustomerName.Name = "lblParentCustomerName";
            this.lblParentCustomerName.Size = new System.Drawing.Size(93, 16);
            this.lblParentCustomerName.TabIndex = 3;
            this.lblParentCustomerName.Text = "債権代表者カナ";
            this.lblParentCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblParentCustomerKana
            // 
            this.lblParentCustomerKana.DropDown.AllowDrop = false;
            this.lblParentCustomerKana.Enabled = false;
            this.lblParentCustomerKana.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParentCustomerKana.HighlightText = true;
            this.lblParentCustomerKana.Location = new System.Drawing.Point(259, 48);
            this.lblParentCustomerKana.Name = "lblParentCustomerKana";
            this.lblParentCustomerKana.ReadOnly = true;
            this.lblParentCustomerKana.Required = false;
            this.lblParentCustomerKana.Size = new System.Drawing.Size(519, 22);
            this.lblParentCustomerKana.TabIndex = 4;
            // 
            // lblParentCustomerCode
            // 
            this.lblParentCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParentCustomerCode.Location = new System.Drawing.Point(160, 22);
            this.lblParentCustomerCode.Name = "lblParentCustomerCode";
            this.lblParentCustomerCode.Size = new System.Drawing.Size(93, 16);
            this.lblParentCustomerCode.TabIndex = 0;
            this.lblParentCustomerCode.Text = "債権代表者コード";
            this.lblParentCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxCustomerGroupList
            // 
            this.gbxCustomerGroupList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxCustomerGroupList.Controls.Add(this.lblCustomerModify);
            this.gbxCustomerGroupList.Controls.Add(this.grdCustomerModify);
            this.gbxCustomerGroupList.Controls.Add(this.lblCustomerOrigin);
            this.gbxCustomerGroupList.Controls.Add(this.grdCustomerOrigin);
            this.gbxCustomerGroupList.Controls.Add(this.lblArrow);
            this.gbxCustomerGroupList.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxCustomerGroupList.Location = new System.Drawing.Point(45, 101);
            this.gbxCustomerGroupList.Name = "gbxCustomerGroupList";
            this.gbxCustomerGroupList.Padding = new System.Windows.Forms.Padding(6, 0, 6, 3);
            this.gbxCustomerGroupList.Size = new System.Drawing.Size(918, 389);
            this.gbxCustomerGroupList.TabIndex = 2;
            this.gbxCustomerGroupList.TabStop = false;
            // 
            // lblCustomerModify
            // 
            this.lblCustomerModify.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCustomerModify.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerModify.Location = new System.Drawing.Point(474, 19);
            this.lblCustomerModify.Name = "lblCustomerModify";
            this.lblCustomerModify.Size = new System.Drawing.Size(110, 16);
            this.lblCustomerModify.TabIndex = 1;
            this.lblCustomerModify.Text = "□　変更後の得意先";
            this.lblCustomerModify.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdCustomerModify
            // 
            this.grdCustomerModify.AllowAutoExtend = true;
            this.grdCustomerModify.AllowUserToAddRows = false;
            this.grdCustomerModify.AllowUserToShiftSelect = true;
            this.grdCustomerModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdCustomerModify.Location = new System.Drawing.Point(477, 41);
            this.grdCustomerModify.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grdCustomerModify.Name = "grdCustomerModify";
            this.grdCustomerModify.Size = new System.Drawing.Size(432, 339);
            this.grdCustomerModify.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdCustomerModify.TabIndex = 1;
            this.grdCustomerModify.TabStop = false;
            this.grdCustomerModify.Text = "vOneGridControl2";
            this.grdCustomerModify.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdCustomerModify_CellDoubleClick);
            // 
            // lblCustomerOrigin
            // 
            this.lblCustomerOrigin.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerOrigin.Location = new System.Drawing.Point(9, 19);
            this.lblCustomerOrigin.Name = "lblCustomerOrigin";
            this.lblCustomerOrigin.Size = new System.Drawing.Size(110, 16);
            this.lblCustomerOrigin.TabIndex = 0;
            this.lblCustomerOrigin.Text = "□　変更前の得意先";
            this.lblCustomerOrigin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdCustomerOrigin
            // 
            this.grdCustomerOrigin.AllowAutoExtend = true;
            this.grdCustomerOrigin.AllowUserToAddRows = false;
            this.grdCustomerOrigin.AllowUserToShiftSelect = true;
            this.grdCustomerOrigin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdCustomerOrigin.Location = new System.Drawing.Point(9, 41);
            this.grdCustomerOrigin.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grdCustomerOrigin.Name = "grdCustomerOrigin";
            this.grdCustomerOrigin.Size = new System.Drawing.Size(432, 339);
            this.grdCustomerOrigin.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdCustomerOrigin.TabIndex = 0;
            this.grdCustomerOrigin.TabStop = false;
            this.grdCustomerOrigin.Text = "vOneGridControl1";
            // 
            // lblArrow
            // 
            this.lblArrow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblArrow.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArrow.Location = new System.Drawing.Point(450, 211);
            this.lblArrow.Name = "lblArrow";
            this.lblArrow.Size = new System.Drawing.Size(20, 16);
            this.lblArrow.TabIndex = 2;
            this.lblArrow.Text = "➡";
            this.lblArrow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PB0601
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxCustomerGroupList);
            this.Controls.Add(this.gbxParentCustomerInput);
            this.Controls.Add(this.gbxChildCustomerInput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB0601";
            this.Load += new System.EventHandler(this.PB0601_Load);
            this.gbxChildCustomerInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeTo)).EndInit();
            this.gbxParentCustomerInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtParentCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblParentCustomerKana)).EndInit();
            this.gbxCustomerGroupList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCustomerModify)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCustomerOrigin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneDispLabelControl lblCustomerNameTo;
        private Common.Controls.VOneLabelControl lblCustomer;
        private Common.Controls.VOneLabelControl lblCustomerFromTo;
        private Common.Controls.VOneTextControl txtCustomerCodeFrom;
        private Common.Controls.VOneTextControl txtCustomerCodeTo;
        private System.Windows.Forms.Button btnCustomerFrom;
        private System.Windows.Forms.Button btnCustomerTo;
        private Common.Controls.VOneDispLabelControl lblCustomerNameFrom;
        private System.Windows.Forms.Button btnParentCustomerSearch;
        private Common.Controls.VOneDispLabelControl lblParentCustomerKana;
        private Common.Controls.VOneTextControl txtParentCustomerCode;
        private Common.Controls.VOneLabelControl lblParentCustomerCode;
        private Common.Controls.VOneLabelControl lblParentCustomerName;
        private Common.Controls.VOneLabelControl lblCustomerOrigin;
        private Common.Controls.VOneLabelControl lblCustomerModify;
        private Common.Controls.VOneGridControl grdCustomerOrigin;
        private Common.Controls.VOneGridControl grdCustomerModify;
        private Common.Controls.VOneLabelControl lblArrow;
        private System.Windows.Forms.GroupBox gbxParentCustomerInput;
        private System.Windows.Forms.GroupBox gbxChildCustomerInput;
        private System.Windows.Forms.GroupBox gbxCustomerGroupList;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDeleteAll;
    }
}
