namespace Rac.VOne.Client.Screen
{
    partial class PC0701
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
            this.lblExtractCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblPaymentAgencyName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblInvalidCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblPaymentAgency = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblExtractAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblFileFormat = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.dlblExtractCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.dlblInvalidCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.txtPaymentAgencyCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.dlblExtractAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.dlblOutputCount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.dlblOutputAmount = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.btnOutputFilePath = new System.Windows.Forms.Button();
            this.datDueAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.cmbCollectCategory = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblCollectCategory = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDueAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datDueAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblDueAtFromTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblOutputFilePath = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtOutputFilePath = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblFileFormatName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblNewDueAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datNewDueAt = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.datNewDueAt2nd = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblNewDueAt2nd = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.lblPaymentAgencyName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblExtractCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblInvalidCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentAgencyCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblExtractAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblOutputCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblOutputAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datDueAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCollectCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datDueAtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputFilePath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFileFormatName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datNewDueAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datNewDueAt2nd)).BeginInit();
            this.SuspendLayout();
            // 
            // lblExtractCount
            // 
            this.lblExtractCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractCount.Location = new System.Drawing.Point(193, 159);
            this.lblExtractCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblExtractCount.Name = "lblExtractCount";
            this.lblExtractCount.Size = new System.Drawing.Size(115, 16);
            this.lblExtractCount.TabIndex = 16;
            this.lblExtractCount.Text = "抽出件数";
            this.lblExtractCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPaymentAgencyName
            // 
            this.lblPaymentAgencyName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPaymentAgencyName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPaymentAgencyName.DropDown.AllowDrop = false;
            this.lblPaymentAgencyName.Enabled = false;
            this.lblPaymentAgencyName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentAgencyName.HighlightText = true;
            this.lblPaymentAgencyName.Location = new System.Drawing.Point(193, 43);
            this.lblPaymentAgencyName.Name = "lblPaymentAgencyName";
            this.lblPaymentAgencyName.ReadOnly = true;
            this.lblPaymentAgencyName.Required = false;
            this.lblPaymentAgencyName.Size = new System.Drawing.Size(711, 22);
            this.lblPaymentAgencyName.TabIndex = 8;
            // 
            // lblInvalidCount
            // 
            this.lblInvalidCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInvalidCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvalidCount.Location = new System.Drawing.Point(320, 159);
            this.lblInvalidCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblInvalidCount.Name = "lblInvalidCount";
            this.lblInvalidCount.Size = new System.Drawing.Size(115, 16);
            this.lblInvalidCount.TabIndex = 18;
            this.lblInvalidCount.Text = "出力不可能件数";
            this.lblInvalidCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPaymentAgency
            // 
            this.lblPaymentAgency.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPaymentAgency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentAgency.Location = new System.Drawing.Point(72, 45);
            this.lblPaymentAgency.Name = "lblPaymentAgency";
            this.lblPaymentAgency.Size = new System.Drawing.Size(79, 16);
            this.lblPaymentAgency.TabIndex = 6;
            this.lblPaymentAgency.Text = "決済代行会社";
            this.lblPaymentAgency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExtractAmount
            // 
            this.lblExtractAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtractAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExtractAmount.Location = new System.Drawing.Point(447, 159);
            this.lblExtractAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblExtractAmount.Name = "lblExtractAmount";
            this.lblExtractAmount.Size = new System.Drawing.Size(115, 16);
            this.lblExtractAmount.TabIndex = 20;
            this.lblExtractAmount.Text = "抽出金額";
            this.lblExtractAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutputCount
            // 
            this.lblOutputCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputCount.Location = new System.Drawing.Point(574, 159);
            this.lblOutputCount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblOutputCount.Name = "lblOutputCount";
            this.lblOutputCount.Size = new System.Drawing.Size(115, 16);
            this.lblOutputCount.TabIndex = 22;
            this.lblOutputCount.Text = "出力件数";
            this.lblOutputCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFileFormat
            // 
            this.lblFileFormat.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFileFormat.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileFormat.Location = new System.Drawing.Point(72, 74);
            this.lblFileFormat.Name = "lblFileFormat";
            this.lblFileFormat.Size = new System.Drawing.Size(79, 16);
            this.lblFileFormat.TabIndex = 9;
            this.lblFileFormat.Text = "フォーマット";
            this.lblFileFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutputAmount
            // 
            this.lblOutputAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputAmount.Location = new System.Drawing.Point(701, 159);
            this.lblOutputAmount.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblOutputAmount.Name = "lblOutputAmount";
            this.lblOutputAmount.Size = new System.Drawing.Size(115, 16);
            this.lblOutputAmount.TabIndex = 24;
            this.lblOutputAmount.Text = "出力金額";
            this.lblOutputAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dlblExtractCount
            // 
            this.dlblExtractCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblExtractCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblExtractCount.DropDown.AllowDrop = false;
            this.dlblExtractCount.Enabled = false;
            this.dlblExtractCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblExtractCount.HighlightText = true;
            this.dlblExtractCount.Location = new System.Drawing.Point(193, 183);
            this.dlblExtractCount.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.dlblExtractCount.Name = "dlblExtractCount";
            this.dlblExtractCount.ReadOnly = true;
            this.dlblExtractCount.Required = false;
            this.dlblExtractCount.Size = new System.Drawing.Size(115, 22);
            this.dlblExtractCount.TabIndex = 17;
            // 
            // dlblInvalidCount
            // 
            this.dlblInvalidCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblInvalidCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblInvalidCount.DropDown.AllowDrop = false;
            this.dlblInvalidCount.Enabled = false;
            this.dlblInvalidCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblInvalidCount.HighlightText = true;
            this.dlblInvalidCount.Location = new System.Drawing.Point(320, 183);
            this.dlblInvalidCount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dlblInvalidCount.Name = "dlblInvalidCount";
            this.dlblInvalidCount.ReadOnly = true;
            this.dlblInvalidCount.Required = false;
            this.dlblInvalidCount.Size = new System.Drawing.Size(115, 22);
            this.dlblInvalidCount.TabIndex = 19;
            // 
            // txtPaymentAgencyCode
            // 
            this.txtPaymentAgencyCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtPaymentAgencyCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPaymentAgencyCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPaymentAgencyCode.DropDown.AllowDrop = false;
            this.txtPaymentAgencyCode.Enabled = false;
            this.txtPaymentAgencyCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaymentAgencyCode.Format = "9";
            this.txtPaymentAgencyCode.HighlightText = true;
            this.txtPaymentAgencyCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPaymentAgencyCode.Location = new System.Drawing.Point(157, 43);
            this.txtPaymentAgencyCode.MaxLength = 2;
            this.txtPaymentAgencyCode.Name = "txtPaymentAgencyCode";
            this.txtPaymentAgencyCode.Required = false;
            this.txtPaymentAgencyCode.Size = new System.Drawing.Size(30, 22);
            this.txtPaymentAgencyCode.TabIndex = 7;
            // 
            // dlblExtractAmount
            // 
            this.dlblExtractAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblExtractAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblExtractAmount.DropDown.AllowDrop = false;
            this.dlblExtractAmount.Enabled = false;
            this.dlblExtractAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblExtractAmount.HighlightText = true;
            this.dlblExtractAmount.Location = new System.Drawing.Point(447, 183);
            this.dlblExtractAmount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dlblExtractAmount.Name = "dlblExtractAmount";
            this.dlblExtractAmount.ReadOnly = true;
            this.dlblExtractAmount.Required = false;
            this.dlblExtractAmount.Size = new System.Drawing.Size(115, 22);
            this.dlblExtractAmount.TabIndex = 21;
            // 
            // dlblOutputCount
            // 
            this.dlblOutputCount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblOutputCount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblOutputCount.DropDown.AllowDrop = false;
            this.dlblOutputCount.Enabled = false;
            this.dlblOutputCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblOutputCount.HighlightText = true;
            this.dlblOutputCount.Location = new System.Drawing.Point(574, 183);
            this.dlblOutputCount.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dlblOutputCount.Name = "dlblOutputCount";
            this.dlblOutputCount.ReadOnly = true;
            this.dlblOutputCount.Required = false;
            this.dlblOutputCount.Size = new System.Drawing.Size(115, 22);
            this.dlblOutputCount.TabIndex = 23;
            // 
            // dlblOutputAmount
            // 
            this.dlblOutputAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dlblOutputAmount.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.dlblOutputAmount.DropDown.AllowDrop = false;
            this.dlblOutputAmount.Enabled = false;
            this.dlblOutputAmount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlblOutputAmount.HighlightText = true;
            this.dlblOutputAmount.Location = new System.Drawing.Point(701, 183);
            this.dlblOutputAmount.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.dlblOutputAmount.Name = "dlblOutputAmount";
            this.dlblOutputAmount.ReadOnly = true;
            this.dlblOutputAmount.Required = false;
            this.dlblOutputAmount.Size = new System.Drawing.Size(115, 22);
            this.dlblOutputAmount.TabIndex = 26;
            // 
            // btnOutputFilePath
            // 
            this.btnOutputFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOutputFilePath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOutputFilePath.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnOutputFilePath.Location = new System.Drawing.Point(909, 98);
            this.btnOutputFilePath.Name = "btnOutputFilePath";
            this.btnOutputFilePath.Size = new System.Drawing.Size(24, 24);
            this.btnOutputFilePath.TabIndex = 13;
            this.btnOutputFilePath.UseVisualStyleBackColor = true;
            // 
            // datDueAtFrom
            // 
            this.datDueAtFrom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datDueAtFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datDueAtFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datDueAtFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datDueAtFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datDueAtFrom.Location = new System.Drawing.Point(642, 15);
            this.datDueAtFrom.Name = "datDueAtFrom";
            this.datDueAtFrom.Required = false;
            this.datDueAtFrom.Size = new System.Drawing.Size(115, 22);
            this.datDueAtFrom.Spin.AllowSpin = false;
            this.datDueAtFrom.TabIndex = 3;
            this.datDueAtFrom.Value = null;
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(259, 213);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(514, 393);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 27;
            this.grid.Text = "vOneGridControl1";
            // 
            // cmbCollectCategory
            // 
            this.cmbCollectCategory.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbCollectCategory.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbCollectCategory.DisplayMember = null;
            this.cmbCollectCategory.DropDown.AllowResize = false;
            this.cmbCollectCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCollectCategory.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbCollectCategory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbCollectCategory.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbCollectCategory.ListHeaderPane.Height = 22;
            this.cmbCollectCategory.ListHeaderPane.Visible = false;
            this.cmbCollectCategory.Location = new System.Drawing.Point(157, 15);
            this.cmbCollectCategory.Name = "cmbCollectCategory";
            this.cmbCollectCategory.Required = true;
            this.cmbCollectCategory.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbCollectCategory.Size = new System.Drawing.Size(318, 22);
            this.cmbCollectCategory.TabIndex = 1;
            this.cmbCollectCategory.ValueMember = null;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // lblCollectCategory
            // 
            this.lblCollectCategory.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCollectCategory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCollectCategory.Location = new System.Drawing.Point(72, 18);
            this.lblCollectCategory.Name = "lblCollectCategory";
            this.lblCollectCategory.Size = new System.Drawing.Size(79, 16);
            this.lblCollectCategory.TabIndex = 0;
            this.lblCollectCategory.Text = "回収区分";
            this.lblCollectCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDueAt
            // 
            this.lblDueAt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDueAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDueAt.Location = new System.Drawing.Point(559, 18);
            this.lblDueAt.Name = "lblDueAt";
            this.lblDueAt.Size = new System.Drawing.Size(77, 16);
            this.lblDueAt.TabIndex = 2;
            this.lblDueAt.Text = "入金予定日";
            this.lblDueAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datDueAtTo
            // 
            this.datDueAtTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datDueAtTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datDueAtTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datDueAtTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datDueAtTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datDueAtTo.Location = new System.Drawing.Point(789, 15);
            this.datDueAtTo.Name = "datDueAtTo";
            this.datDueAtTo.Required = false;
            this.datDueAtTo.Size = new System.Drawing.Size(115, 22);
            this.datDueAtTo.Spin.AllowSpin = false;
            this.datDueAtTo.TabIndex = 5;
            this.datDueAtTo.Value = null;
            // 
            // lblDueAtFromTo
            // 
            this.lblDueAtFromTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDueAtFromTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDueAtFromTo.Location = new System.Drawing.Point(763, 17);
            this.lblDueAtFromTo.Name = "lblDueAtFromTo";
            this.lblDueAtFromTo.Size = new System.Drawing.Size(20, 16);
            this.lblDueAtFromTo.TabIndex = 4;
            this.lblDueAtFromTo.Text = "～";
            this.lblDueAtFromTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutputFilePath
            // 
            this.lblOutputFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblOutputFilePath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputFilePath.Location = new System.Drawing.Point(72, 102);
            this.lblOutputFilePath.Name = "lblOutputFilePath";
            this.lblOutputFilePath.Size = new System.Drawing.Size(79, 16);
            this.lblOutputFilePath.TabIndex = 11;
            this.lblOutputFilePath.Text = "出力ファイル名";
            this.lblOutputFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOutputFilePath
            // 
            this.txtOutputFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtOutputFilePath.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtOutputFilePath.DropDown.AllowDrop = false;
            this.txtOutputFilePath.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutputFilePath.HighlightText = true;
            this.txtOutputFilePath.Location = new System.Drawing.Point(157, 99);
            this.txtOutputFilePath.MaxLength = 255;
            this.txtOutputFilePath.Name = "txtOutputFilePath";
            this.txtOutputFilePath.Required = true;
            this.txtOutputFilePath.Size = new System.Drawing.Size(747, 22);
            this.txtOutputFilePath.TabIndex = 12;
            // 
            // lblFileFormatName
            // 
            this.lblFileFormatName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFileFormatName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFileFormatName.DropDown.AllowDrop = false;
            this.lblFileFormatName.Enabled = false;
            this.lblFileFormatName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileFormatName.HighlightText = true;
            this.lblFileFormatName.Location = new System.Drawing.Point(157, 71);
            this.lblFileFormatName.Name = "lblFileFormatName";
            this.lblFileFormatName.ReadOnly = true;
            this.lblFileFormatName.Required = false;
            this.lblFileFormatName.Size = new System.Drawing.Size(747, 22);
            this.lblFileFormatName.TabIndex = 10;
            // 
            // lblNewDueAt
            // 
            this.lblNewDueAt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNewDueAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewDueAt.Location = new System.Drawing.Point(72, 129);
            this.lblNewDueAt.Name = "lblNewDueAt";
            this.lblNewDueAt.Size = new System.Drawing.Size(79, 16);
            this.lblNewDueAt.TabIndex = 14;
            this.lblNewDueAt.Text = "引落日";
            this.lblNewDueAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datNewDueAt
            // 
            this.datNewDueAt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datNewDueAt.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datNewDueAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datNewDueAt.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datNewDueAt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datNewDueAt.Location = new System.Drawing.Point(157, 127);
            this.datNewDueAt.Name = "datNewDueAt";
            this.datNewDueAt.Required = true;
            this.datNewDueAt.Size = new System.Drawing.Size(115, 22);
            this.datNewDueAt.Spin.AllowSpin = false;
            this.datNewDueAt.TabIndex = 15;
            this.datNewDueAt.Value = null;
            // 
            // datNewDueAt2nd
            // 
            this.datNewDueAt2nd.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datNewDueAt2nd.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datNewDueAt2nd.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datNewDueAt2nd.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datNewDueAt2nd.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datNewDueAt2nd.Location = new System.Drawing.Point(372, 127);
            this.datNewDueAt2nd.Name = "datNewDueAt2nd";
            this.datNewDueAt2nd.Required = true;
            this.datNewDueAt2nd.Size = new System.Drawing.Size(115, 22);
            this.datNewDueAt2nd.Spin.AllowSpin = false;
            this.datNewDueAt2nd.TabIndex = 29;
            this.datNewDueAt2nd.Value = null;
            // 
            // lblNewDueAt2nd
            // 
            this.lblNewDueAt2nd.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNewDueAt2nd.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewDueAt2nd.Location = new System.Drawing.Point(296, 129);
            this.lblNewDueAt2nd.Name = "lblNewDueAt2nd";
            this.lblNewDueAt2nd.Size = new System.Drawing.Size(58, 16);
            this.lblNewDueAt2nd.TabIndex = 28;
            this.lblNewDueAt2nd.Text = "再引落日";
            this.lblNewDueAt2nd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PC0701
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.datNewDueAt2nd);
            this.Controls.Add(this.lblNewDueAt2nd);
            this.Controls.Add(this.cmbCollectCategory);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.datDueAtTo);
            this.Controls.Add(this.datNewDueAt);
            this.Controls.Add(this.datDueAtFrom);
            this.Controls.Add(this.lblExtractCount);
            this.Controls.Add(this.lblFileFormatName);
            this.Controls.Add(this.lblPaymentAgencyName);
            this.Controls.Add(this.lblInvalidCount);
            this.Controls.Add(this.lblNewDueAt);
            this.Controls.Add(this.lblDueAtFromTo);
            this.Controls.Add(this.lblDueAt);
            this.Controls.Add(this.lblCollectCategory);
            this.Controls.Add(this.lblPaymentAgency);
            this.Controls.Add(this.lblExtractAmount);
            this.Controls.Add(this.txtOutputFilePath);
            this.Controls.Add(this.lblOutputFilePath);
            this.Controls.Add(this.lblOutputCount);
            this.Controls.Add(this.lblFileFormat);
            this.Controls.Add(this.lblOutputAmount);
            this.Controls.Add(this.btnOutputFilePath);
            this.Controls.Add(this.dlblExtractCount);
            this.Controls.Add(this.dlblInvalidCount);
            this.Controls.Add(this.txtPaymentAgencyCode);
            this.Controls.Add(this.dlblExtractAmount);
            this.Controls.Add(this.dlblOutputCount);
            this.Controls.Add(this.dlblOutputAmount);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PC0701";
            ((System.ComponentModel.ISupportInitialize)(this.lblPaymentAgencyName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblExtractCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblInvalidCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentAgencyCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblExtractAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblOutputCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dlblOutputAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datDueAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCollectCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datDueAtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutputFilePath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFileFormatName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datNewDueAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datNewDueAt2nd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneDispLabelControl lblPaymentAgencyName;
        private Common.Controls.VOneLabelControl lblInvalidCount;
        private Common.Controls.VOneLabelControl lblPaymentAgency;
        private Common.Controls.VOneLabelControl lblExtractAmount;
        private Common.Controls.VOneLabelControl lblOutputCount;
        private Common.Controls.VOneLabelControl lblFileFormat;
        private Common.Controls.VOneLabelControl lblOutputAmount;
        private System.Windows.Forms.Button btnOutputFilePath;
        private Common.Controls.VOneDispLabelControl dlblExtractCount;
        private Common.Controls.VOneDispLabelControl dlblInvalidCount;
        private Common.Controls.VOneTextControl txtPaymentAgencyCode;
        private Common.Controls.VOneDispLabelControl dlblExtractAmount;
        private Common.Controls.VOneDispLabelControl dlblOutputCount;
        private Common.Controls.VOneDispLabelControl dlblOutputAmount;
        private Common.Controls.VOneDateControl datDueAtFrom;
        private Common.Controls.VOneGridControl grid;
        private Common.Controls.VOneComboControl cmbCollectCategory;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneLabelControl lblCollectCategory;
        private Common.Controls.VOneLabelControl lblDueAt;
        private Common.Controls.VOneDateControl datDueAtTo;
        private Common.Controls.VOneLabelControl lblDueAtFromTo;
        private Common.Controls.VOneLabelControl lblOutputFilePath;
        private Common.Controls.VOneTextControl txtOutputFilePath;
        private Common.Controls.VOneDispLabelControl lblFileFormatName;
        private Common.Controls.VOneLabelControl lblNewDueAt;
        private Common.Controls.VOneDateControl datNewDueAt;
        private Common.Controls.VOneLabelControl lblExtractCount;
        private Common.Controls.VOneDateControl datNewDueAt2nd;
        private Common.Controls.VOneLabelControl lblNewDueAt2nd;
    }
}
