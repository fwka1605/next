namespace Rac.VOne.Client.Screen
{
    partial class PC0801
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
            this.grdErrorDataList = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblReadCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblTransferredCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblTransferErrorCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblTransferredAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblTransferErrorAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.dlblReadCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.dlblTransferredCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.dlblTransferErrorCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.dlblTransferredAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.dlblTransferErrorAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblPaymentAgencyCode = new System.Windows.Forms.Label();
            this.lblImportFileFormat = new System.Windows.Forms.Label();
            this.lblImportFilePath = new System.Windows.Forms.Label();
            this.lblTransferYear = new System.Windows.Forms.Label();
            this.btnPaymentAgency = new System.Windows.Forms.Button();
            this.btnImportFilePath = new System.Windows.Forms.Button();
            this.txtPaymentAgencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cmbCollectCategory = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.txtPaymentAgencyName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtImportFileFormat = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtImportFilePath = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cbxConsiderUncollected = new System.Windows.Forms.CheckBox();
            this.lblErrorDataList = new System.Windows.Forms.Label();
            this.ofdImportFile = new System.Windows.Forms.OpenFileDialog();
            this.txtTransferYear = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdErrorDataList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblReadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblTransferredCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblTransferErrorCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblTransferredAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblTransferErrorAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentAgencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCollectCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentAgencyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImportFileFormat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImportFilePath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferYear)).BeginInit();
            this.SuspendLayout();
            // 
            // grdErrorDataList
            // 
            this.grdErrorDataList.AllowAutoExtend = true;
            this.grdErrorDataList.AllowUserToAddRows = false;
            this.grdErrorDataList.AllowUserToShiftSelect = true;
            this.grdErrorDataList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdErrorDataList.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdErrorDataList.Location = new System.Drawing.Point(15, 222);
            this.grdErrorDataList.Name = "grdErrorDataList";
            this.grdErrorDataList.Size = new System.Drawing.Size(978, 384);
            this.grdErrorDataList.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdErrorDataList.TabIndex = 15;
            // 
            // lblReadCount
            // 
            this.lblReadCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblReadCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReadCount.Location = new System.Drawing.Point(176, 150);
            this.lblReadCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblReadCount.Name = "lblReadCount";
            this.lblReadCount.Size = new System.Drawing.Size(115, 16);
            this.lblReadCount.TabIndex = 0;
            this.lblReadCount.Text = "読込件数";
            this.lblReadCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTransferredCount
            // 
            this.lblTransferredCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTransferredCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransferredCount.Location = new System.Drawing.Point(303, 150);
            this.lblTransferredCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblTransferredCount.Name = "lblTransferredCount";
            this.lblTransferredCount.Size = new System.Drawing.Size(115, 16);
            this.lblTransferredCount.TabIndex = 0;
            this.lblTransferredCount.Text = "振替済件数";
            this.lblTransferredCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTransferErrorCount
            // 
            this.lblTransferErrorCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTransferErrorCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransferErrorCount.Location = new System.Drawing.Point(557, 150);
            this.lblTransferErrorCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblTransferErrorCount.Name = "lblTransferErrorCount";
            this.lblTransferErrorCount.Size = new System.Drawing.Size(115, 16);
            this.lblTransferErrorCount.TabIndex = 0;
            this.lblTransferErrorCount.Text = "振替不能件数";
            this.lblTransferErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTransferredAmount
            // 
            this.lblTransferredAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTransferredAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransferredAmount.Location = new System.Drawing.Point(430, 150);
            this.lblTransferredAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblTransferredAmount.Name = "lblTransferredAmount";
            this.lblTransferredAmount.Size = new System.Drawing.Size(115, 16);
            this.lblTransferredAmount.TabIndex = 0;
            this.lblTransferredAmount.Text = "振替済金額";
            this.lblTransferredAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTransferErrorAmount
            // 
            this.lblTransferErrorAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTransferErrorAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransferErrorAmount.Location = new System.Drawing.Point(684, 150);
            this.lblTransferErrorAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblTransferErrorAmount.Name = "lblTransferErrorAmount";
            this.lblTransferErrorAmount.Size = new System.Drawing.Size(115, 16);
            this.lblTransferErrorAmount.TabIndex = 0;
            this.lblTransferErrorAmount.Text = "振替不能金額";
            this.lblTransferErrorAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dlblReadCount
            // 
            this.dlblReadCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblReadCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblReadCount.DropDown.AllowDrop = false;
            this.dlblReadCount.Enabled = false;
            this.dlblReadCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblReadCount.HighlightText = true;
            this.dlblReadCount.Location = new System.Drawing.Point(176, 174);
            this.dlblReadCount.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.dlblReadCount.Name = "dlblReadCount";
            this.dlblReadCount.ReadOnly = true;
            this.dlblReadCount.Required = false;
            this.dlblReadCount.Size = new System.Drawing.Size(115, 22);
            this.dlblReadCount.TabIndex = 10;
            // 
            // dlblTransferredCount
            // 
            this.dlblTransferredCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblTransferredCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblTransferredCount.DropDown.AllowDrop = false;
            this.dlblTransferredCount.Enabled = false;
            this.dlblTransferredCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblTransferredCount.HighlightText = true;
            this.dlblTransferredCount.Location = new System.Drawing.Point(303, 174);
            this.dlblTransferredCount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dlblTransferredCount.Name = "dlblTransferredCount";
            this.dlblTransferredCount.ReadOnly = true;
            this.dlblTransferredCount.Required = false;
            this.dlblTransferredCount.Size = new System.Drawing.Size(115, 22);
            this.dlblTransferredCount.TabIndex = 11;
            // 
            // dlblTransferErrorCount
            // 
            this.dlblTransferErrorCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblTransferErrorCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblTransferErrorCount.DropDown.AllowDrop = false;
            this.dlblTransferErrorCount.Enabled = false;
            this.dlblTransferErrorCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblTransferErrorCount.HighlightText = true;
            this.dlblTransferErrorCount.Location = new System.Drawing.Point(557, 174);
            this.dlblTransferErrorCount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dlblTransferErrorCount.Name = "dlblTransferErrorCount";
            this.dlblTransferErrorCount.ReadOnly = true;
            this.dlblTransferErrorCount.Required = false;
            this.dlblTransferErrorCount.Size = new System.Drawing.Size(115, 22);
            this.dlblTransferErrorCount.TabIndex = 13;
            // 
            // dlblTransferredAmount
            // 
            this.dlblTransferredAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblTransferredAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblTransferredAmount.DropDown.AllowDrop = false;
            this.dlblTransferredAmount.Enabled = false;
            this.dlblTransferredAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblTransferredAmount.HighlightText = true;
            this.dlblTransferredAmount.Location = new System.Drawing.Point(430, 174);
            this.dlblTransferredAmount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dlblTransferredAmount.Name = "dlblTransferredAmount";
            this.dlblTransferredAmount.ReadOnly = true;
            this.dlblTransferredAmount.Required = false;
            this.dlblTransferredAmount.Size = new System.Drawing.Size(115, 22);
            this.dlblTransferredAmount.TabIndex = 12;
            // 
            // dlblTransferErrorAmount
            // 
            this.dlblTransferErrorAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblTransferErrorAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblTransferErrorAmount.DropDown.AllowDrop = false;
            this.dlblTransferErrorAmount.Enabled = false;
            this.dlblTransferErrorAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblTransferErrorAmount.HighlightText = true;
            this.dlblTransferErrorAmount.Location = new System.Drawing.Point(684, 174);
            this.dlblTransferErrorAmount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dlblTransferErrorAmount.Name = "dlblTransferErrorAmount";
            this.dlblTransferErrorAmount.ReadOnly = true;
            this.dlblTransferErrorAmount.Required = false;
            this.dlblTransferErrorAmount.Size = new System.Drawing.Size(115, 22);
            this.dlblTransferErrorAmount.TabIndex = 14;
            // 
            // lblPaymentAgencyCode
            // 
            this.lblPaymentAgencyCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPaymentAgencyCode.Location = new System.Drawing.Point(15, 13);
            this.lblPaymentAgencyCode.Name = "lblPaymentAgencyCode";
            this.lblPaymentAgencyCode.Size = new System.Drawing.Size(81, 16);
            this.lblPaymentAgencyCode.TabIndex = 0;
            this.lblPaymentAgencyCode.Text = "決済手段コード";
            // 
            // lblImportFileFormat
            // 
            this.lblImportFileFormat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblImportFileFormat.Location = new System.Drawing.Point(15, 41);
            this.lblImportFileFormat.Name = "lblImportFileFormat";
            this.lblImportFileFormat.Size = new System.Drawing.Size(81, 16);
            this.lblImportFileFormat.TabIndex = 0;
            this.lblImportFileFormat.Text = "取込フォーマット";
            // 
            // lblImportFilePath
            // 
            this.lblImportFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblImportFilePath.Location = new System.Drawing.Point(15, 69);
            this.lblImportFilePath.Name = "lblImportFilePath";
            this.lblImportFilePath.Size = new System.Drawing.Size(81, 16);
            this.lblImportFilePath.TabIndex = 0;
            this.lblImportFilePath.Text = "取込ファイル名";
            // 
            // lblTransferYear
            // 
            this.lblTransferYear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTransferYear.Location = new System.Drawing.Point(612, 104);
            this.lblTransferYear.Name = "lblTransferYear";
            this.lblTransferYear.Size = new System.Drawing.Size(43, 16);
            this.lblTransferYear.TabIndex = 0;
            this.lblTransferYear.Text = "引落年";
            // 
            // btnPaymentAgency
            // 
            this.btnPaymentAgency.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPaymentAgency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPaymentAgency.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnPaymentAgency.Location = new System.Drawing.Point(138, 9);
            this.btnPaymentAgency.Name = "btnPaymentAgency";
            this.btnPaymentAgency.Size = new System.Drawing.Size(24, 24);
            this.btnPaymentAgency.TabIndex = 2;
            this.btnPaymentAgency.UseVisualStyleBackColor = true;
            this.btnPaymentAgency.Click += new System.EventHandler(this.btnPaymentAgency_Click);
            // 
            // btnImportFilePath
            // 
            this.btnImportFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnImportFilePath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportFilePath.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnImportFilePath.Location = new System.Drawing.Point(969, 65);
            this.btnImportFilePath.Name = "btnImportFilePath";
            this.btnImportFilePath.Size = new System.Drawing.Size(24, 24);
            this.btnImportFilePath.TabIndex = 6;
            this.btnImportFilePath.UseVisualStyleBackColor = true;
            this.btnImportFilePath.Click += new System.EventHandler(this.btnImportFilePath_Click);
            // 
            // txtPaymentAgencyCode
            // 
            this.txtPaymentAgencyCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPaymentAgencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPaymentAgencyCode.DropDown.AllowDrop = false;
            this.txtPaymentAgencyCode.Format = "9";
            this.txtPaymentAgencyCode.HighlightText = true;
            this.txtPaymentAgencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPaymentAgencyCode.Location = new System.Drawing.Point(102, 10);
            this.txtPaymentAgencyCode.MaxLength = 2;
            this.txtPaymentAgencyCode.Name = "txtPaymentAgencyCode";
            this.txtPaymentAgencyCode.PaddingChar = '0';
            this.txtPaymentAgencyCode.Required = true;
            this.txtPaymentAgencyCode.Size = new System.Drawing.Size(30, 22);
            this.txtPaymentAgencyCode.TabIndex = 1;
            this.txtPaymentAgencyCode.Validated += new System.EventHandler(this.txtPaymentAgencyCode_Validated);
            // 
            // cmbCollectCategory
            // 
            this.cmbCollectCategory.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbCollectCategory.DisplayMember = null;
            this.cmbCollectCategory.DropDown.AllowResize = false;
            this.cmbCollectCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCollectCategory.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbCollectCategory.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbCollectCategory.ListHeaderPane.Height = 22;
            this.cmbCollectCategory.ListHeaderPane.Visible = false;
            this.cmbCollectCategory.Location = new System.Drawing.Point(244, 102);
            this.cmbCollectCategory.Name = "cmbCollectCategory";
            this.cmbCollectCategory.Required = false;
            this.cmbCollectCategory.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbCollectCategory.Size = new System.Drawing.Size(300, 22);
            this.cmbCollectCategory.TabIndex = 8;
            this.cmbCollectCategory.ValueMember = null;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // txtPaymentAgencyName
            // 
            this.txtPaymentAgencyName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPaymentAgencyName.DropDown.AllowDrop = false;
            this.txtPaymentAgencyName.Enabled = false;
            this.txtPaymentAgencyName.HighlightText = true;
            this.txtPaymentAgencyName.Location = new System.Drawing.Point(168, 10);
            this.txtPaymentAgencyName.Name = "txtPaymentAgencyName";
            this.txtPaymentAgencyName.ReadOnly = true;
            this.txtPaymentAgencyName.Required = false;
            this.txtPaymentAgencyName.Size = new System.Drawing.Size(795, 22);
            this.txtPaymentAgencyName.TabIndex = 3;
            // 
            // txtImportFileFormat
            // 
            this.txtImportFileFormat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtImportFileFormat.DropDown.AllowDrop = false;
            this.txtImportFileFormat.Enabled = false;
            this.txtImportFileFormat.HighlightText = true;
            this.txtImportFileFormat.Location = new System.Drawing.Point(102, 38);
            this.txtImportFileFormat.Name = "txtImportFileFormat";
            this.txtImportFileFormat.ReadOnly = true;
            this.txtImportFileFormat.Required = false;
            this.txtImportFileFormat.Size = new System.Drawing.Size(861, 22);
            this.txtImportFileFormat.TabIndex = 4;
            // 
            // txtImportFilePath
            // 
            this.txtImportFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtImportFilePath.DropDown.AllowDrop = false;
            this.txtImportFilePath.HighlightText = true;
            this.txtImportFilePath.Location = new System.Drawing.Point(102, 66);
            this.txtImportFilePath.Name = "txtImportFilePath";
            this.txtImportFilePath.Required = true;
            this.txtImportFilePath.Size = new System.Drawing.Size(861, 22);
            this.txtImportFilePath.TabIndex = 5;
            // 
            // cbxConsiderUncollected
            // 
            this.cbxConsiderUncollected.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbxConsiderUncollected.Location = new System.Drawing.Point(19, 104);
            this.cbxConsiderUncollected.Name = "cbxConsiderUncollected";
            this.cbxConsiderUncollected.Size = new System.Drawing.Size(219, 18);
            this.cbxConsiderUncollected.TabIndex = 7;
            this.cbxConsiderUncollected.Text = "振替不能時に回収区分を自動更新する";
            this.cbxConsiderUncollected.UseVisualStyleBackColor = true;
            this.cbxConsiderUncollected.CheckedChanged += new System.EventHandler(this.cbxConsiderUncollected_CheckedChanged);
            // 
            // lblErrorDataList
            // 
            this.lblErrorDataList.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblErrorDataList.Location = new System.Drawing.Point(15, 200);
            this.lblErrorDataList.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblErrorDataList.Name = "lblErrorDataList";
            this.lblErrorDataList.Size = new System.Drawing.Size(107, 16);
            this.lblErrorDataList.TabIndex = 0;
            this.lblErrorDataList.Text = "振替不能データ一覧";
            // 
            // ofdImportFile
            // 
            this.ofdImportFile.Title = "ファイルを開く";
            // 
            // txtTransferYear
            // 
            this.txtTransferYear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTransferYear.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtTransferYear.DropDown.AllowDrop = false;
            this.txtTransferYear.Format = "9";
            this.txtTransferYear.HighlightText = true;
            this.txtTransferYear.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtTransferYear.Location = new System.Drawing.Point(661, 102);
            this.txtTransferYear.MaxLength = 4;
            this.txtTransferYear.Name = "txtTransferYear";
            this.txtTransferYear.Required = false;
            this.txtTransferYear.Size = new System.Drawing.Size(70, 22);
            this.txtTransferYear.TabIndex = 9;
            // 
            // PC0801
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtTransferYear);
            this.Controls.Add(this.lblReadCount);
            this.Controls.Add(this.dlblTransferErrorAmount);
            this.Controls.Add(this.lblErrorDataList);
            this.Controls.Add(this.dlblTransferredAmount);
            this.Controls.Add(this.cbxConsiderUncollected);
            this.Controls.Add(this.dlblTransferErrorCount);
            this.Controls.Add(this.dlblTransferredCount);
            this.Controls.Add(this.txtImportFilePath);
            this.Controls.Add(this.dlblReadCount);
            this.Controls.Add(this.txtImportFileFormat);
            this.Controls.Add(this.lblTransferErrorAmount);
            this.Controls.Add(this.lblTransferredAmount);
            this.Controls.Add(this.txtPaymentAgencyName);
            this.Controls.Add(this.lblTransferErrorCount);
            this.Controls.Add(this.cmbCollectCategory);
            this.Controls.Add(this.lblTransferredCount);
            this.Controls.Add(this.txtPaymentAgencyCode);
            this.Controls.Add(this.btnImportFilePath);
            this.Controls.Add(this.btnPaymentAgency);
            this.Controls.Add(this.lblTransferYear);
            this.Controls.Add(this.lblImportFilePath);
            this.Controls.Add(this.lblImportFileFormat);
            this.Controls.Add(this.lblPaymentAgencyCode);
            this.Controls.Add(this.grdErrorDataList);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PC0801";
            this.Load += new System.EventHandler(this.PC0801_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdErrorDataList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblReadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblTransferredCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblTransferErrorCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblTransferredAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblTransferErrorAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentAgencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCollectCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentAgencyName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImportFileFormat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtImportFilePath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransferYear)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneGridControl grdErrorDataList;
        private Common.Controls.VOneLabelControl lblReadCount;
        private Common.Controls.VOneLabelControl lblTransferredCount;
        private Common.Controls.VOneLabelControl lblTransferErrorCount;
        private Common.Controls.VOneLabelControl lblTransferredAmount;
        private Common.Controls.VOneLabelControl lblTransferErrorAmount;
        private Common.Controls.VOneDispLabelControl dlblReadCount;
        private Common.Controls.VOneDispLabelControl dlblTransferredCount;
        private Common.Controls.VOneDispLabelControl dlblTransferErrorCount;
        private Common.Controls.VOneDispLabelControl dlblTransferredAmount;
        private Common.Controls.VOneDispLabelControl dlblTransferErrorAmount;
        private System.Windows.Forms.Label lblPaymentAgencyCode;
        private System.Windows.Forms.Label lblImportFileFormat;
        private System.Windows.Forms.Label lblImportFilePath;
        private System.Windows.Forms.Label lblTransferYear;
        private System.Windows.Forms.Button btnPaymentAgency;
        private System.Windows.Forms.Button btnImportFilePath;
        private Common.Controls.VOneTextControl txtPaymentAgencyCode;
        private Common.Controls.VOneComboControl cmbCollectCategory;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneTextControl txtPaymentAgencyName;
        private Common.Controls.VOneTextControl txtImportFileFormat;
        private Common.Controls.VOneTextControl txtImportFilePath;
        private System.Windows.Forms.CheckBox cbxConsiderUncollected;
        private System.Windows.Forms.Label lblErrorDataList;
        private System.Windows.Forms.OpenFileDialog ofdImportFile;
        private Common.Controls.VOneTextControl txtTransferYear;
    }
}
