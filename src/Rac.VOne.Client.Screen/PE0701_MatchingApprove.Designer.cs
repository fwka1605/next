namespace Rac.VOne.Client.Screen
{
    partial class PE0701
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
            this.tbcMatchingApproval = new System.Windows.Forms.TabControl();
            this.tbpSearchCondition = new System.Windows.Forms.TabPage();
            this.pnlCreateAt = new System.Windows.Forms.Panel();
            this.datCreateAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblCreateAtFromTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datCreateAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblCreateAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlSection = new System.Windows.Forms.Panel();
            this.lblSectionName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.btnInitializeSectionSelection = new System.Windows.Forms.Button();
            this.btnSectionName = new System.Windows.Forms.Button();
            this.lblSection = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlDepartment = new System.Windows.Forms.Panel();
            this.btnDepartmentCode = new System.Windows.Forms.Button();
            this.btnInitializeDepartmentSelection = new System.Windows.Forms.Button();
            this.lblDepartmentName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblDepartmentCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.cbxApprovalData = new System.Windows.Forms.CheckBox();
            this.tbpSearchResult = new System.Windows.Forms.TabPage();
            this.btnIncrease = new System.Windows.Forms.Button();
            this.lblLength = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdApprovalResult = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.btnDecrease = new System.Windows.Forms.Button();
            this.txtCustomerName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCustomerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnCustomerCodeSearch = new System.Windows.Forms.Button();
            this.btnCustomerCode = new System.Windows.Forms.Button();
            this.btnCustomerNameSearch = new System.Windows.Forms.Button();
            this.txtCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblPayerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtPayerName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnPayerName = new System.Windows.Forms.Button();
            this.nmbLength = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.tbcMatchingApproval.SuspendLayout();
            this.tbpSearchCondition.SuspendLayout();
            this.pnlCreateAt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datCreateAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datCreateAtTo)).BeginInit();
            this.pnlSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).BeginInit();
            this.pnlDepartment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.tbpSearchResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApprovalResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbLength)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcMatchingApproval
            // 
            this.tbcMatchingApproval.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcMatchingApproval.Controls.Add(this.tbpSearchCondition);
            this.tbcMatchingApproval.Controls.Add(this.tbpSearchResult);
            this.tbcMatchingApproval.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbcMatchingApproval.Location = new System.Drawing.Point(15, 15);
            this.tbcMatchingApproval.Name = "tbcMatchingApproval";
            this.tbcMatchingApproval.SelectedIndex = 0;
            this.tbcMatchingApproval.Size = new System.Drawing.Size(978, 591);
            this.tbcMatchingApproval.TabIndex = 1;
            // 
            // tbpSearchCondition
            // 
            this.tbpSearchCondition.Controls.Add(this.pnlCreateAt);
            this.tbpSearchCondition.Controls.Add(this.pnlSection);
            this.tbpSearchCondition.Controls.Add(this.pnlDepartment);
            this.tbpSearchCondition.Controls.Add(this.pnlMain);
            this.tbpSearchCondition.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpSearchCondition.Location = new System.Drawing.Point(4, 24);
            this.tbpSearchCondition.Name = "tbpSearchCondition";
            this.tbpSearchCondition.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSearchCondition.Size = new System.Drawing.Size(970, 563);
            this.tbpSearchCondition.TabIndex = 0;
            this.tbpSearchCondition.Text = "検索条件";
            this.tbpSearchCondition.UseVisualStyleBackColor = true;
            // 
            // pnlCreateAt
            // 
            this.pnlCreateAt.Controls.Add(this.datCreateAtFrom);
            this.pnlCreateAt.Controls.Add(this.lblCreateAtFromTo);
            this.pnlCreateAt.Controls.Add(this.datCreateAtTo);
            this.pnlCreateAt.Controls.Add(this.lblCreateAt);
            this.pnlCreateAt.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCreateAt.Location = new System.Drawing.Point(3, 90);
            this.pnlCreateAt.Name = "pnlCreateAt";
            this.pnlCreateAt.Size = new System.Drawing.Size(964, 29);
            this.pnlCreateAt.TabIndex = 3;
            this.pnlCreateAt.Visible = false;
            // 
            // datCreateAtFrom
            // 
            this.datCreateAtFrom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.datCreateAtFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datCreateAtFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datCreateAtFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datCreateAtFrom.Location = new System.Drawing.Point(90, 3);
            this.datCreateAtFrom.MaxDate = new System.DateTime(2999, 12, 31, 23, 59, 59, 0);
            this.datCreateAtFrom.MaxValue = new System.DateTime(2999, 12, 31, 23, 59, 59, 0);
            this.datCreateAtFrom.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.datCreateAtFrom.MinValue = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.datCreateAtFrom.Name = "datCreateAtFrom";
            this.datCreateAtFrom.Required = false;
            this.datCreateAtFrom.Size = new System.Drawing.Size(115, 22);
            this.datCreateAtFrom.Spin.AllowSpin = false;
            this.datCreateAtFrom.TabIndex = 1;
            this.datCreateAtFrom.Value = null;
            // 
            // lblCreateAtFromTo
            // 
            this.lblCreateAtFromTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCreateAtFromTo.Location = new System.Drawing.Point(213, 6);
            this.lblCreateAtFromTo.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblCreateAtFromTo.Name = "lblCreateAtFromTo";
            this.lblCreateAtFromTo.Size = new System.Drawing.Size(20, 16);
            this.lblCreateAtFromTo.TabIndex = 2;
            this.lblCreateAtFromTo.Text = "～";
            this.lblCreateAtFromTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // datCreateAtTo
            // 
            this.datCreateAtTo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.datCreateAtTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datCreateAtTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datCreateAtTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datCreateAtTo.Location = new System.Drawing.Point(241, 4);
            this.datCreateAtTo.MaxDate = new System.DateTime(2999, 12, 31, 23, 59, 59, 0);
            this.datCreateAtTo.MaxValue = new System.DateTime(2999, 12, 31, 23, 59, 59, 0);
            this.datCreateAtTo.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.datCreateAtTo.MinValue = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.datCreateAtTo.Name = "datCreateAtTo";
            this.datCreateAtTo.Required = false;
            this.datCreateAtTo.Size = new System.Drawing.Size(115, 22);
            this.datCreateAtTo.Spin.AllowSpin = false;
            this.datCreateAtTo.TabIndex = 3;
            this.datCreateAtTo.Value = null;
            // 
            // lblCreateAt
            // 
            this.lblCreateAt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCreateAt.Location = new System.Drawing.Point(16, 5);
            this.lblCreateAt.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.lblCreateAt.Name = "lblCreateAt";
            this.lblCreateAt.Size = new System.Drawing.Size(68, 16);
            this.lblCreateAt.TabIndex = 0;
            this.lblCreateAt.Text = "消込日時";
            this.lblCreateAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSection
            // 
            this.pnlSection.Controls.Add(this.lblSectionName);
            this.pnlSection.Controls.Add(this.btnInitializeSectionSelection);
            this.pnlSection.Controls.Add(this.btnSectionName);
            this.pnlSection.Controls.Add(this.lblSection);
            this.pnlSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSection.Location = new System.Drawing.Point(3, 61);
            this.pnlSection.Name = "pnlSection";
            this.pnlSection.Size = new System.Drawing.Size(964, 29);
            this.pnlSection.TabIndex = 2;
            // 
            // lblSectionName
            // 
            this.lblSectionName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSectionName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSectionName.DropDown.AllowDrop = false;
            this.lblSectionName.Enabled = false;
            this.lblSectionName.HighlightText = true;
            this.lblSectionName.Location = new System.Drawing.Point(90, 4);
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.ReadOnly = true;
            this.lblSectionName.Required = false;
            this.lblSectionName.Size = new System.Drawing.Size(115, 22);
            this.lblSectionName.TabIndex = 7;
            // 
            // btnInitializeSectionSelection
            // 
            this.btnInitializeSectionSelection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnInitializeSectionSelection.Location = new System.Drawing.Point(241, 2);
            this.btnInitializeSectionSelection.Name = "btnInitializeSectionSelection";
            this.btnInitializeSectionSelection.Size = new System.Drawing.Size(60, 24);
            this.btnInitializeSectionSelection.TabIndex = 9;
            this.btnInitializeSectionSelection.Text = "初期化";
            this.btnInitializeSectionSelection.UseVisualStyleBackColor = true;
            this.btnInitializeSectionSelection.Click += new System.EventHandler(this.btnInitializeSectionSelection_Click);
            // 
            // btnSectionName
            // 
            this.btnSectionName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSectionName.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSectionName.Location = new System.Drawing.Point(211, 2);
            this.btnSectionName.Name = "btnSectionName";
            this.btnSectionName.Size = new System.Drawing.Size(24, 24);
            this.btnSectionName.TabIndex = 8;
            this.btnSectionName.UseVisualStyleBackColor = true;
            this.btnSectionName.Click += new System.EventHandler(this.btnSectionName_Click);
            // 
            // lblSection
            // 
            this.lblSection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSection.Location = new System.Drawing.Point(16, 6);
            this.lblSection.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(68, 16);
            this.lblSection.TabIndex = 11;
            this.lblSection.Text = "入金部門";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDepartment
            // 
            this.pnlDepartment.Controls.Add(this.btnDepartmentCode);
            this.pnlDepartment.Controls.Add(this.btnInitializeDepartmentSelection);
            this.pnlDepartment.Controls.Add(this.lblDepartmentName);
            this.pnlDepartment.Controls.Add(this.lblDepartmentCode);
            this.pnlDepartment.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDepartment.Location = new System.Drawing.Point(3, 32);
            this.pnlDepartment.Name = "pnlDepartment";
            this.pnlDepartment.Size = new System.Drawing.Size(964, 29);
            this.pnlDepartment.TabIndex = 1;
            // 
            // btnDepartmentCode
            // 
            this.btnDepartmentCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDepartmentCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnDepartmentCode.Location = new System.Drawing.Point(211, 2);
            this.btnDepartmentCode.Name = "btnDepartmentCode";
            this.btnDepartmentCode.Size = new System.Drawing.Size(24, 24);
            this.btnDepartmentCode.TabIndex = 6;
            this.btnDepartmentCode.UseVisualStyleBackColor = true;
            this.btnDepartmentCode.Click += new System.EventHandler(this.btnDepartmentCode_Click);
            // 
            // btnInitializeDepartmentSelection
            // 
            this.btnInitializeDepartmentSelection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnInitializeDepartmentSelection.Location = new System.Drawing.Point(241, 2);
            this.btnInitializeDepartmentSelection.Name = "btnInitializeDepartmentSelection";
            this.btnInitializeDepartmentSelection.Size = new System.Drawing.Size(60, 24);
            this.btnInitializeDepartmentSelection.TabIndex = 9;
            this.btnInitializeDepartmentSelection.Text = "初期化";
            this.btnInitializeDepartmentSelection.UseVisualStyleBackColor = true;
            this.btnInitializeDepartmentSelection.Click += new System.EventHandler(this.btnInitializeDepartmentSelection_Click);
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDepartmentName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDepartmentName.DropDown.AllowDrop = false;
            this.lblDepartmentName.Enabled = false;
            this.lblDepartmentName.HighlightText = true;
            this.lblDepartmentName.Location = new System.Drawing.Point(90, 3);
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.ReadOnly = true;
            this.lblDepartmentName.Required = false;
            this.lblDepartmentName.Size = new System.Drawing.Size(115, 22);
            this.lblDepartmentName.TabIndex = 7;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDepartmentCode.Location = new System.Drawing.Point(16, 6);
            this.lblDepartmentCode.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Size = new System.Drawing.Size(68, 16);
            this.lblDepartmentCode.TabIndex = 7;
            this.lblDepartmentCode.Text = "請求部門";
            this.lblDepartmentCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.cbxApprovalData);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMain.Location = new System.Drawing.Point(3, 3);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(964, 29);
            this.pnlMain.TabIndex = 0;
            // 
            // cbxApprovalData
            // 
            this.cbxApprovalData.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxApprovalData.Location = new System.Drawing.Point(21, 5);
            this.cbxApprovalData.Margin = new System.Windows.Forms.Padding(18, 18, 3, 3);
            this.cbxApprovalData.Name = "cbxApprovalData";
            this.cbxApprovalData.Size = new System.Drawing.Size(123, 18);
            this.cbxApprovalData.TabIndex = 1;
            this.cbxApprovalData.Text = "承認済データを表示";
            this.cbxApprovalData.UseVisualStyleBackColor = true;
            this.cbxApprovalData.CheckedChanged += new System.EventHandler(this.cbxApprovalData_CheckedChanged);
            // 
            // tbpSearchResult
            // 
            this.tbpSearchResult.Controls.Add(this.btnIncrease);
            this.tbpSearchResult.Controls.Add(this.lblLength);
            this.tbpSearchResult.Controls.Add(this.grdApprovalResult);
            this.tbpSearchResult.Controls.Add(this.btnDecrease);
            this.tbpSearchResult.Controls.Add(this.txtCustomerName);
            this.tbpSearchResult.Controls.Add(this.lblCustomerName);
            this.tbpSearchResult.Controls.Add(this.btnCustomerCodeSearch);
            this.tbpSearchResult.Controls.Add(this.btnCustomerCode);
            this.tbpSearchResult.Controls.Add(this.btnCustomerNameSearch);
            this.tbpSearchResult.Controls.Add(this.txtCustomerCode);
            this.tbpSearchResult.Controls.Add(this.lblPayerName);
            this.tbpSearchResult.Controls.Add(this.lblCustomerCode);
            this.tbpSearchResult.Controls.Add(this.txtPayerName);
            this.tbpSearchResult.Controls.Add(this.btnPayerName);
            this.tbpSearchResult.Controls.Add(this.nmbLength);
            this.tbpSearchResult.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpSearchResult.Location = new System.Drawing.Point(4, 24);
            this.tbpSearchResult.Name = "tbpSearchResult";
            this.tbpSearchResult.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSearchResult.Size = new System.Drawing.Size(970, 563);
            this.tbpSearchResult.TabIndex = 1;
            this.tbpSearchResult.Text = "検索結果";
            this.tbpSearchResult.UseVisualStyleBackColor = true;
            // 
            // btnIncrease
            // 
            this.btnIncrease.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnIncrease.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncrease.Location = new System.Drawing.Point(250, 467);
            this.btnIncrease.Name = "btnIncrease";
            this.btnIncrease.Size = new System.Drawing.Size(33, 24);
            this.btnIncrease.TabIndex = 3;
            this.btnIncrease.Text = "→";
            this.btnIncrease.UseVisualStyleBackColor = true;
            this.btnIncrease.Click += new System.EventHandler(this.btnIncrease_Click);
            // 
            // lblLength
            // 
            this.lblLength.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblLength.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLength.Location = new System.Drawing.Point(15, 471);
            this.lblLength.Margin = new System.Windows.Forms.Padding(12, 0, 3, 0);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(69, 16);
            this.lblLength.TabIndex = 0;
            this.lblLength.Text = "表示桁数";
            this.lblLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdApprovalResult
            // 
            this.grdApprovalResult.AllowAutoExtend = true;
            this.grdApprovalResult.AllowUserToAddRows = false;
            this.grdApprovalResult.AllowUserToResize = false;
            this.grdApprovalResult.AllowUserToShiftSelect = true;
            this.grdApprovalResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdApprovalResult.HorizontalScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.grdApprovalResult.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdApprovalResult.Location = new System.Drawing.Point(1, 3);
            this.grdApprovalResult.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.grdApprovalResult.Name = "grdApprovalResult";
            this.grdApprovalResult.Size = new System.Drawing.Size(965, 459);
            this.grdApprovalResult.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdApprovalResult.TabIndex = 0;
            this.grdApprovalResult.Text = "vOneGridControl1";
            this.grdApprovalResult.CellClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdApprovalResult_CellClick);
            // 
            // btnDecrease
            // 
            this.btnDecrease.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDecrease.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecrease.Location = new System.Drawing.Point(211, 467);
            this.btnDecrease.Name = "btnDecrease";
            this.btnDecrease.Size = new System.Drawing.Size(33, 24);
            this.btnDecrease.TabIndex = 2;
            this.btnDecrease.Text = "←";
            this.btnDecrease.UseVisualStyleBackColor = true;
            this.btnDecrease.Click += new System.EventHandler(this.btnDecrease_Click);
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtCustomerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerName.DropDown.AllowDrop = false;
            this.txtCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerName.HighlightText = true;
            this.txtCustomerName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtCustomerName.Location = new System.Drawing.Point(90, 497);
            this.txtCustomerName.MaxLength = 140;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Required = false;
            this.txtCustomerName.Size = new System.Drawing.Size(290, 22);
            this.txtCustomerName.TabIndex = 4;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(15, 499);
            this.lblCustomerName.Margin = new System.Windows.Forms.Padding(12, 0, 3, 0);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(69, 16);
            this.lblCustomerName.TabIndex = 0;
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCustomerCodeSearch
            // 
            this.btnCustomerCodeSearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCustomerCodeSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerCodeSearch.Location = new System.Drawing.Point(241, 525);
            this.btnCustomerCodeSearch.Name = "btnCustomerCodeSearch";
            this.btnCustomerCodeSearch.Size = new System.Drawing.Size(51, 24);
            this.btnCustomerCodeSearch.TabIndex = 10;
            this.btnCustomerCodeSearch.Text = "検索";
            this.btnCustomerCodeSearch.UseVisualStyleBackColor = true;
            this.btnCustomerCodeSearch.Click += new System.EventHandler(this.btnCustomerCodeSearch_Click);
            // 
            // btnCustomerCode
            // 
            this.btnCustomerCode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomerCode.Location = new System.Drawing.Point(211, 525);
            this.btnCustomerCode.Name = "btnCustomerCode";
            this.btnCustomerCode.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerCode.TabIndex = 9;
            this.btnCustomerCode.UseVisualStyleBackColor = true;
            this.btnCustomerCode.Click += new System.EventHandler(this.btnCustomerCode_Click);
            // 
            // btnCustomerNameSearch
            // 
            this.btnCustomerNameSearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCustomerNameSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerNameSearch.Location = new System.Drawing.Point(386, 495);
            this.btnCustomerNameSearch.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.btnCustomerNameSearch.Name = "btnCustomerNameSearch";
            this.btnCustomerNameSearch.Size = new System.Drawing.Size(51, 24);
            this.btnCustomerNameSearch.TabIndex = 5;
            this.btnCustomerNameSearch.Text = "検索";
            this.btnCustomerNameSearch.UseVisualStyleBackColor = true;
            this.btnCustomerNameSearch.Click += new System.EventHandler(this.btnCustomerNameSearch_Click);
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtCustomerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerCode.DropDown.AllowDrop = false;
            this.txtCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerCode.HighlightText = true;
            this.txtCustomerCode.Location = new System.Drawing.Point(90, 526);
            this.txtCustomerCode.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Required = false;
            this.txtCustomerCode.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCode.TabIndex = 8;
            this.txtCustomerCode.Validated += new System.EventHandler(this.txtCustomerCode_Validated);
            // 
            // lblPayerName
            // 
            this.lblPayerName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayerName.Location = new System.Drawing.Point(455, 499);
            this.lblPayerName.Margin = new System.Windows.Forms.Padding(9, 0, 3, 0);
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Size = new System.Drawing.Size(79, 16);
            this.lblPayerName.TabIndex = 0;
            this.lblPayerName.Text = "振込依頼人名";
            this.lblPayerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerCode.Location = new System.Drawing.Point(15, 529);
            this.lblCustomerCode.Margin = new System.Windows.Forms.Padding(12, 0, 3, 0);
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Size = new System.Drawing.Size(69, 16);
            this.lblCustomerCode.TabIndex = 0;
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPayerName
            // 
            this.txtPayerName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtPayerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPayerName.DropDown.AllowDrop = false;
            this.txtPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayerName.Format = "@NA9";
            this.txtPayerName.HighlightText = true;
            this.txtPayerName.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtPayerName.Location = new System.Drawing.Point(540, 497);
            this.txtPayerName.MaxLength = 140;
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Required = false;
            this.txtPayerName.Size = new System.Drawing.Size(250, 22);
            this.txtPayerName.TabIndex = 6;
            // 
            // btnPayerName
            // 
            this.btnPayerName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayerName.Location = new System.Drawing.Point(796, 495);
            this.btnPayerName.Name = "btnPayerName";
            this.btnPayerName.Size = new System.Drawing.Size(51, 24);
            this.btnPayerName.TabIndex = 7;
            this.btnPayerName.Text = "検索";
            this.btnPayerName.UseVisualStyleBackColor = true;
            this.btnPayerName.Click += new System.EventHandler(this.btnPayerName_Click);
            // 
            // nmbLength
            // 
            this.nmbLength.AllowDeleteToNull = true;
            this.nmbLength.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.nmbLength.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.nmbLength.DropDown.AllowDrop = false;
            this.nmbLength.Fields.IntegerPart.MinDigits = 1;
            this.nmbLength.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmbLength.HighlightText = true;
            this.nmbLength.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbLength.Location = new System.Drawing.Point(90, 468);
            this.nmbLength.MaxValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmbLength.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmbLength.Name = "nmbLength";
            this.nmbLength.Required = false;
            this.nmbLength.Size = new System.Drawing.Size(115, 22);
            this.nmbLength.Spin.AllowSpin = false;
            this.nmbLength.TabIndex = 1;
            this.nmbLength.Leave += new System.EventHandler(this.nmbLength_Leave);
            // 
            // PE0701
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tbcMatchingApproval);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PE0701";
            this.Load += new System.EventHandler(this.PE0701_Load);
            this.tbcMatchingApproval.ResumeLayout(false);
            this.tbpSearchCondition.ResumeLayout(false);
            this.pnlCreateAt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datCreateAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datCreateAtTo)).EndInit();
            this.pnlSection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblSectionName)).EndInit();
            this.pnlDepartment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentName)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.tbpSearchResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdApprovalResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbLength)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcMatchingApproval;
        private System.Windows.Forms.TabPage tbpSearchCondition;
        private System.Windows.Forms.CheckBox cbxApprovalData;
        private System.Windows.Forms.TabPage tbpSearchResult;
        private System.Windows.Forms.Button btnIncrease;
        private Common.Controls.VOneLabelControl lblLength;
        private System.Windows.Forms.Button btnDecrease;
        private Common.Controls.VOneLabelControl lblCustomerName;
        private Common.Controls.VOneTextControl txtCustomerName;
        private System.Windows.Forms.Button btnCustomerNameSearch;
        private Common.Controls.VOneLabelControl lblPayerName;
        private Common.Controls.VOneTextControl txtPayerName;
        private Common.Controls.VOneNumberControl nmbLength;
        private System.Windows.Forms.Button btnPayerName;
        private Common.Controls.VOneLabelControl lblCustomerCode;
        private Common.Controls.VOneTextControl txtCustomerCode;
        private System.Windows.Forms.Button btnCustomerCode;
        private System.Windows.Forms.Button btnCustomerCodeSearch;
        private Common.Controls.VOneGridControl grdApprovalResult;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlSection;
        private Common.Controls.VOneDispLabelControl lblSectionName;
        private System.Windows.Forms.Button btnInitializeSectionSelection;
        private System.Windows.Forms.Button btnSectionName;
        private Common.Controls.VOneLabelControl lblSection;
        private System.Windows.Forms.Panel pnlDepartment;
        private System.Windows.Forms.Button btnDepartmentCode;
        private System.Windows.Forms.Button btnInitializeDepartmentSelection;
        private Common.Controls.VOneDispLabelControl lblDepartmentName;
        private Common.Controls.VOneLabelControl lblDepartmentCode;
        private System.Windows.Forms.Panel pnlCreateAt;
        private Common.Controls.VOneDateControl datCreateAtFrom;
        private Common.Controls.VOneLabelControl lblCreateAtFromTo;
        private Common.Controls.VOneDateControl datCreateAtTo;
        private Common.Controls.VOneLabelControl lblCreateAt;
    }
}
