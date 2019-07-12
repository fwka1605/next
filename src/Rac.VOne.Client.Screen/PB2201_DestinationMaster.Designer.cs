namespace Rac.VOne.Client.Screen
{
    partial class PB2201
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
            GrapeCity.Win.Editors.Fields.MaskPatternField maskPatternField1 = new GrapeCity.Win.Editors.Fields.MaskPatternField();
            GrapeCity.Win.Editors.Fields.MaskLiteralField maskLiteralField1 = new GrapeCity.Win.Editors.Fields.MaskLiteralField();
            GrapeCity.Win.Editors.Fields.MaskPatternField maskPatternField2 = new GrapeCity.Win.Editors.Fields.MaskPatternField();
            this.gbxDestinationList = new System.Windows.Forms.GroupBox();
            this.grdDestination = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxDestinationInput = new System.Windows.Forms.GroupBox();
            this.lblDestinationName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtDestinationName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cmbHonorific = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton13 = new GrapeCity.Win.Editors.DropDownButton();
            this.txtDestinationCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblHonorific = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblAddress2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblPostalCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDepartmentName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.mskPostalCode = new Rac.VOne.Client.Common.Controls.VOneMaskControl(this.components);
            this.lblAddress1 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblAddressee = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtAddressee = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtAddress1 = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblDestinationCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtAddress2 = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtDepartmentName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCustomerSearch = new System.Windows.Forms.Button();
            this.txtCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.gbxDestinationList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDestination)).BeginInit();
            this.gbxDestinationInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHonorific)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mskPostalCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddressee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxDestinationList
            // 
            this.gbxDestinationList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxDestinationList.Controls.Add(this.grdDestination);
            this.gbxDestinationList.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.gbxDestinationList.Location = new System.Drawing.Point(32, 43);
            this.gbxDestinationList.Name = "gbxDestinationList";
            this.gbxDestinationList.Size = new System.Drawing.Size(940, 302);
            this.gbxDestinationList.TabIndex = 4;
            this.gbxDestinationList.TabStop = false;
            this.gbxDestinationList.Text = "□　登録済みの送付先";
            // 
            // grdDestination
            // 
            this.grdDestination.AllowAutoExtend = true;
            this.grdDestination.AllowUserToAddRows = false;
            this.grdDestination.AllowUserToShiftSelect = true;
            this.grdDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdDestination.CurrentCellBorderLine = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.Black);
            this.grdDestination.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdDestination.Location = new System.Drawing.Point(15, 22);
            this.grdDestination.Margin = new System.Windows.Forms.Padding(12, 3, 12, 6);
            this.grdDestination.Name = "grdDestination";
            this.grdDestination.Size = new System.Drawing.Size(910, 271);
            this.grdDestination.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdDestination.TabIndex = 0;
            this.grdDestination.TabStop = false;
            this.grdDestination.Text = "vOneGridControl1";
            this.grdDestination.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdDestination_CellDoubleClick);
            // 
            // gbxDestinationInput
            // 
            this.gbxDestinationInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxDestinationInput.Controls.Add(this.lblDestinationName);
            this.gbxDestinationInput.Controls.Add(this.txtDestinationName);
            this.gbxDestinationInput.Controls.Add(this.cmbHonorific);
            this.gbxDestinationInput.Controls.Add(this.txtDestinationCode);
            this.gbxDestinationInput.Controls.Add(this.lblHonorific);
            this.gbxDestinationInput.Controls.Add(this.lblAddress2);
            this.gbxDestinationInput.Controls.Add(this.lblPostalCode);
            this.gbxDestinationInput.Controls.Add(this.lblDepartmentName);
            this.gbxDestinationInput.Controls.Add(this.mskPostalCode);
            this.gbxDestinationInput.Controls.Add(this.lblAddress1);
            this.gbxDestinationInput.Controls.Add(this.lblAddressee);
            this.gbxDestinationInput.Controls.Add(this.txtAddressee);
            this.gbxDestinationInput.Controls.Add(this.txtAddress1);
            this.gbxDestinationInput.Controls.Add(this.lblDestinationCode);
            this.gbxDestinationInput.Controls.Add(this.txtAddress2);
            this.gbxDestinationInput.Controls.Add(this.txtDepartmentName);
            this.gbxDestinationInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxDestinationInput.Location = new System.Drawing.Point(32, 351);
            this.gbxDestinationInput.Name = "gbxDestinationInput";
            this.gbxDestinationInput.Size = new System.Drawing.Size(940, 255);
            this.gbxDestinationInput.TabIndex = 11;
            this.gbxDestinationInput.TabStop = false;
            // 
            // lblDestinationName
            // 
            this.lblDestinationName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblDestinationName.Location = new System.Drawing.Point(223, 53);
            this.lblDestinationName.Name = "lblDestinationName";
            this.lblDestinationName.Size = new System.Drawing.Size(69, 16);
            this.lblDestinationName.TabIndex = 2;
            this.lblDestinationName.Text = "送付先名";
            this.lblDestinationName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDestinationName
            // 
            this.txtDestinationName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDestinationName.DropDown.AllowDrop = false;
            this.txtDestinationName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtDestinationName.HighlightText = true;
            this.txtDestinationName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtDestinationName.Location = new System.Drawing.Point(298, 51);
            this.txtDestinationName.MaxLength = 40;
            this.txtDestinationName.Name = "txtDestinationName";
            this.txtDestinationName.Required = false;
            this.txtDestinationName.Size = new System.Drawing.Size(400, 22);
            this.txtDestinationName.TabIndex = 3;
            // 
            // cmbHonorific
            // 
            this.cmbHonorific.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbHonorific.DisplayMember = null;
            this.cmbHonorific.DropDown.AllowResize = false;
            this.cmbHonorific.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHonorific.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbHonorific.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.cmbHonorific.ListHeaderPane.Height = 22;
            this.cmbHonorific.ListHeaderPane.Visible = false;
            this.cmbHonorific.Location = new System.Drawing.Point(298, 219);
            this.cmbHonorific.MaxLength = 6;
            this.cmbHonorific.Name = "cmbHonorific";
            this.cmbHonorific.Padding = new System.Windows.Forms.Padding(0);
            this.cmbHonorific.Required = false;
            this.cmbHonorific.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton13});
            this.cmbHonorific.Size = new System.Drawing.Size(120, 22);
            this.cmbHonorific.TabIndex = 15;
            this.cmbHonorific.ValueMember = null;
            // 
            // dropDownButton13
            // 
            this.dropDownButton13.Name = "dropDownButton13";
            // 
            // txtDestinationCode
            // 
            this.txtDestinationCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtDestinationCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDestinationCode.DropDown.AllowDrop = false;
            this.txtDestinationCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtDestinationCode.Format = "9";
            this.txtDestinationCode.HighlightText = true;
            this.txtDestinationCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtDestinationCode.IntegralHeight = true;
            this.txtDestinationCode.Location = new System.Drawing.Point(298, 23);
            this.txtDestinationCode.MaxLength = 2;
            this.txtDestinationCode.Name = "txtDestinationCode";
            this.txtDestinationCode.Required = true;
            this.txtDestinationCode.Size = new System.Drawing.Size(30, 22);
            this.txtDestinationCode.TabIndex = 1;
            this.txtDestinationCode.Validated += new System.EventHandler(this.txtDestinationCode_Validated);
            // 
            // lblHonorific
            // 
            this.lblHonorific.Location = new System.Drawing.Point(223, 222);
            this.lblHonorific.Margin = new System.Windows.Forms.Padding(3);
            this.lblHonorific.Name = "lblHonorific";
            this.lblHonorific.Size = new System.Drawing.Size(69, 16);
            this.lblHonorific.TabIndex = 14;
            this.lblHonorific.Text = "敬称";
            this.lblHonorific.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddress2
            // 
            this.lblAddress2.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblAddress2.Location = new System.Drawing.Point(223, 137);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(69, 16);
            this.lblAddress2.TabIndex = 8;
            this.lblAddress2.Text = "住所2";
            this.lblAddress2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPostalCode
            // 
            this.lblPostalCode.Location = new System.Drawing.Point(223, 81);
            this.lblPostalCode.Margin = new System.Windows.Forms.Padding(3);
            this.lblPostalCode.Name = "lblPostalCode";
            this.lblPostalCode.Size = new System.Drawing.Size(69, 16);
            this.lblPostalCode.TabIndex = 4;
            this.lblPostalCode.Text = "郵便番号";
            this.lblPostalCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblDepartmentName.Location = new System.Drawing.Point(223, 165);
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Size = new System.Drawing.Size(69, 16);
            this.lblDepartmentName.TabIndex = 10;
            this.lblDepartmentName.Text = "部署";
            this.lblDepartmentName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mskPostalCode
            // 
            this.mskPostalCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            maskPatternField1.MaxLength = 3;
            maskPatternField1.MinLength = 3;
            maskPatternField1.Pattern = "\\D";
            maskLiteralField1.Text = "-";
            maskPatternField2.MaxLength = 4;
            maskPatternField2.MinLength = 4;
            maskPatternField2.Pattern = "\\D";
            this.mskPostalCode.Fields.AddRange(new GrapeCity.Win.Editors.Fields.MaskField[] {
            maskPatternField1,
            maskLiteralField1,
            maskPatternField2});
            this.mskPostalCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.mskPostalCode.Location = new System.Drawing.Point(298, 79);
            this.mskPostalCode.Name = "mskPostalCode";
            this.mskPostalCode.Padding = new System.Windows.Forms.Padding(0);
            this.mskPostalCode.Required = false;
            this.mskPostalCode.Size = new System.Drawing.Size(73, 22);
            this.mskPostalCode.TabIndex = 5;
            this.mskPostalCode.Enter += new System.EventHandler(this.mskPostalCode_Enter);
            // 
            // lblAddress1
            // 
            this.lblAddress1.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblAddress1.Location = new System.Drawing.Point(223, 109);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(69, 16);
            this.lblAddress1.TabIndex = 6;
            this.lblAddress1.Text = "住所1";
            this.lblAddress1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddressee
            // 
            this.lblAddressee.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblAddressee.Location = new System.Drawing.Point(223, 193);
            this.lblAddressee.Name = "lblAddressee";
            this.lblAddressee.Size = new System.Drawing.Size(69, 16);
            this.lblAddressee.TabIndex = 12;
            this.lblAddressee.Text = "宛名";
            this.lblAddressee.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAddressee
            // 
            this.txtAddressee.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtAddressee.DropDown.AllowDrop = false;
            this.txtAddressee.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtAddressee.HighlightText = true;
            this.txtAddressee.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtAddressee.Location = new System.Drawing.Point(298, 191);
            this.txtAddressee.MaxLength = 20;
            this.txtAddressee.Name = "txtAddressee";
            this.txtAddressee.Required = false;
            this.txtAddressee.Size = new System.Drawing.Size(210, 22);
            this.txtAddressee.TabIndex = 13;
            // 
            // txtAddress1
            // 
            this.txtAddress1.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtAddress1.DropDown.AllowDrop = false;
            this.txtAddress1.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtAddress1.HighlightText = true;
            this.txtAddress1.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtAddress1.Location = new System.Drawing.Point(298, 107);
            this.txtAddress1.MaxLength = 40;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Required = false;
            this.txtAddress1.Size = new System.Drawing.Size(400, 22);
            this.txtAddress1.TabIndex = 7;
            // 
            // lblDestinationCode
            // 
            this.lblDestinationCode.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblDestinationCode.Location = new System.Drawing.Point(223, 25);
            this.lblDestinationCode.Name = "lblDestinationCode";
            this.lblDestinationCode.Size = new System.Drawing.Size(69, 16);
            this.lblDestinationCode.TabIndex = 0;
            this.lblDestinationCode.Text = "送付先コード";
            this.lblDestinationCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAddress2
            // 
            this.txtAddress2.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtAddress2.DropDown.AllowDrop = false;
            this.txtAddress2.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtAddress2.HighlightText = true;
            this.txtAddress2.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtAddress2.Location = new System.Drawing.Point(298, 135);
            this.txtAddress2.MaxLength = 40;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Required = false;
            this.txtAddress2.Size = new System.Drawing.Size(400, 22);
            this.txtAddress2.TabIndex = 9;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDepartmentName.DropDown.AllowDrop = false;
            this.txtDepartmentName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtDepartmentName.HighlightText = true;
            this.txtDepartmentName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtDepartmentName.Location = new System.Drawing.Point(298, 163);
            this.txtDepartmentName.MaxLength = 40;
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Required = false;
            this.txtDepartmentName.Size = new System.Drawing.Size(400, 22);
            this.txtDepartmentName.TabIndex = 11;
            // 
            // btnCustomerSearch
            // 
            this.btnCustomerSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCustomerSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomerSearch.Location = new System.Drawing.Point(379, 13);
            this.btnCustomerSearch.Name = "btnCustomerSearch";
            this.btnCustomerSearch.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerSearch.TabIndex = 2;
            this.btnCustomerSearch.UseVisualStyleBackColor = true;
            this.btnCustomerSearch.Click += new System.EventHandler(this.btnCustomerSearch_Click);
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCustomerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCustomerCode.DropDown.AllowDrop = false;
            this.txtCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtCustomerCode.HighlightText = true;
            this.txtCustomerCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCustomerCode.Location = new System.Drawing.Point(258, 15);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Required = false;
            this.txtCustomerCode.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCode.TabIndex = 1;
            this.txtCustomerCode.Validated += new System.EventHandler(this.txtCustomerCode_Validated);
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblCustomerCode.Location = new System.Drawing.Point(171, 17);
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Size = new System.Drawing.Size(81, 16);
            this.lblCustomerCode.TabIndex = 0;
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCustomerName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerName.DropDown.AllowDrop = false;
            this.lblCustomerName.Enabled = false;
            this.lblCustomerName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblCustomerName.HighlightText = true;
            this.lblCustomerName.Location = new System.Drawing.Point(409, 15);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.ReadOnly = true;
            this.lblCustomerName.Required = false;
            this.lblCustomerName.Size = new System.Drawing.Size(548, 22);
            this.lblCustomerName.TabIndex = 3;
            // 
            // PB2201
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxDestinationInput);
            this.Controls.Add(this.gbxDestinationList);
            this.Controls.Add(this.btnCustomerSearch);
            this.Controls.Add(this.txtCustomerCode);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.lblCustomerCode);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PB2201";
            this.Load += new System.EventHandler(this.PB2201_Load);
            this.gbxDestinationList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDestination)).EndInit();
            this.gbxDestinationInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHonorific)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinationCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mskPostalCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddressee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxDestinationList;
        private Common.Controls.VOneGridControl grdDestination;
        private System.Windows.Forms.GroupBox gbxDestinationInput;
        private Common.Controls.VOneTextControl txtAddress2;
        private Common.Controls.VOneTextControl txtDestinationCode;
        private Common.Controls.VOneLabelControl lblDestinationCode;
        private Common.Controls.VOneTextControl txtAddress1;
        private Common.Controls.VOneLabelControl lblAddress1;
        private Common.Controls.VOneLabelControl lblAddress2;
        private System.Windows.Forms.Button btnCustomerSearch;
        private Common.Controls.VOneTextControl txtCustomerCode;
        private Common.Controls.VOneLabelControl lblCustomerCode;
        private Common.Controls.VOneDispLabelControl lblCustomerName;
        private Common.Controls.VOneLabelControl lblDepartmentName;
        private Common.Controls.VOneTextControl txtDepartmentName;
        private Common.Controls.VOneLabelControl lblAddressee;
        private Common.Controls.VOneTextControl txtAddressee;
        private Common.Controls.VOneLabelControl lblPostalCode;
        private Common.Controls.VOneMaskControl mskPostalCode;
        private Common.Controls.VOneComboControl cmbHonorific;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton13;
        private Common.Controls.VOneLabelControl lblHonorific;
        private Common.Controls.VOneLabelControl lblDestinationName;
        private Common.Controls.VOneTextControl txtDestinationName;
    }
}