namespace Rac.VOne.Client.Screen.Dialogs
{
    partial class dlgMFBillingExtractSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgMFBillingExtractSetting));
            Rac.VOne.Message.XmlMessenger xmlMessenger1 = new Rac.VOne.Message.XmlMessenger();
            this.btnF10 = new System.Windows.Forms.Button();
            this.btnF01 = new System.Windows.Forms.Button();
            this.lblClosingDay = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblRecoveryPlan = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDay = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblAfterMonth = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxCustomer = new System.Windows.Forms.GroupBox();
            this.cbxIssueBillEachTime = new System.Windows.Forms.CheckBox();
            this.lblLimitDay = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCollectOffsetDay = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtCollectOffsetMonth = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtClosingDay = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtStaffCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cmbCollectCategoryId = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblCollectCategoryId = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnStaffCode = new System.Windows.Forms.Button();
            this.lblStaffName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblStaffCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxBilling = new System.Windows.Forms.GroupBox();
            this.lblBillingCategory = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtBillingCategory = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnBillingCategory = new System.Windows.Forms.Button();
            this.lblBillingCategoryName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.gbxCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectOffsetDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectOffsetMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCollectCategoryId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).BeginInit();
            this.gbxBilling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCategoryName)).BeginInit();
            this.SuspendLayout();
            // 
            // btnF10
            // 
            this.btnF10.Location = new System.Drawing.Point(469, 246);
            this.btnF10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnF10.Name = "btnF10";
            this.btnF10.Size = new System.Drawing.Size(96, 32);
            this.btnF10.TabIndex = 3;
            this.btnF10.Text = "F10/戻る";
            this.btnF10.UseVisualStyleBackColor = true;
            this.btnF10.Click += new System.EventHandler(this.btnF10_Click);
            // 
            // btnF01
            // 
            this.btnF01.Location = new System.Drawing.Point(336, 246);
            this.btnF01.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnF01.Name = "btnF01";
            this.btnF01.Size = new System.Drawing.Size(96, 32);
            this.btnF01.TabIndex = 2;
            this.btnF01.Text = "F1/登録";
            this.btnF01.UseVisualStyleBackColor = true;
            this.btnF01.Click += new System.EventHandler(this.btnF03_Click);
            // 
            // lblClosingDay
            // 
            this.lblClosingDay.Location = new System.Drawing.Point(26, 58);
            this.lblClosingDay.Margin = new System.Windows.Forms.Padding(14, 0, 3, 0);
            this.lblClosingDay.Name = "lblClosingDay";
            this.lblClosingDay.Size = new System.Drawing.Size(67, 16);
            this.lblClosingDay.TabIndex = 4;
            this.lblClosingDay.Text = "締日";
            this.lblClosingDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecoveryPlan
            // 
            this.lblRecoveryPlan.Location = new System.Drawing.Point(153, 58);
            this.lblRecoveryPlan.Name = "lblRecoveryPlan";
            this.lblRecoveryPlan.Size = new System.Drawing.Size(55, 16);
            this.lblRecoveryPlan.TabIndex = 6;
            this.lblRecoveryPlan.Text = "回収予定";
            this.lblRecoveryPlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDay
            // 
            this.lblDay.Location = new System.Drawing.Point(341, 58);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(45, 16);
            this.lblDay.TabIndex = 10;
            this.lblDay.Text = "日";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAfterMonth
            // 
            this.lblAfterMonth.Location = new System.Drawing.Point(250, 58);
            this.lblAfterMonth.Name = "lblAfterMonth";
            this.lblAfterMonth.Size = new System.Drawing.Size(55, 16);
            this.lblAfterMonth.TabIndex = 8;
            this.lblAfterMonth.Text = "ヶ月後の";
            this.lblAfterMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxCustomer
            // 
            this.gbxCustomer.Controls.Add(this.cbxIssueBillEachTime);
            this.gbxCustomer.Controls.Add(this.lblLimitDay);
            this.gbxCustomer.Controls.Add(this.txtCollectOffsetDay);
            this.gbxCustomer.Controls.Add(this.txtCollectOffsetMonth);
            this.gbxCustomer.Controls.Add(this.txtClosingDay);
            this.gbxCustomer.Controls.Add(this.txtStaffCode);
            this.gbxCustomer.Controls.Add(this.cmbCollectCategoryId);
            this.gbxCustomer.Controls.Add(this.lblCollectCategoryId);
            this.gbxCustomer.Controls.Add(this.btnStaffCode);
            this.gbxCustomer.Controls.Add(this.lblStaffName);
            this.gbxCustomer.Controls.Add(this.lblStaffCode);
            this.gbxCustomer.Controls.Add(this.lblClosingDay);
            this.gbxCustomer.Controls.Add(this.lblRecoveryPlan);
            this.gbxCustomer.Controls.Add(this.lblAfterMonth);
            this.gbxCustomer.Controls.Add(this.lblDay);
            this.gbxCustomer.Location = new System.Drawing.Point(14, 89);
            this.gbxCustomer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxCustomer.Name = "gbxCustomer";
            this.gbxCustomer.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxCustomer.Size = new System.Drawing.Size(551, 150);
            this.gbxCustomer.TabIndex = 1;
            this.gbxCustomer.TabStop = false;
            this.gbxCustomer.Text = "得意先";
            // 
            // cbxIssueBillEachTime
            // 
            this.cbxIssueBillEachTime.Location = new System.Drawing.Point(99, 86);
            this.cbxIssueBillEachTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbxIssueBillEachTime.Name = "cbxIssueBillEachTime";
            this.cbxIssueBillEachTime.Size = new System.Drawing.Size(74, 18);
            this.cbxIssueBillEachTime.TabIndex = 11;
            this.cbxIssueBillEachTime.Text = "都度請求";
            this.cbxIssueBillEachTime.UseVisualStyleBackColor = true;
            this.cbxIssueBillEachTime.CheckedChanged += new System.EventHandler(this.cbxIssueBillEachTime_CheckedChanged);
            // 
            // lblLimitDay
            // 
            this.lblLimitDay.Location = new System.Drawing.Point(299, 85);
            this.lblLimitDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblLimitDay.Name = "lblLimitDay";
            this.lblLimitDay.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblLimitDay.Size = new System.Drawing.Size(163, 16);
            this.lblLimitDay.TabIndex = 12;
            this.lblLimitDay.Text = "※末日（２８日以降）＝９９";
            this.lblLimitDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCollectOffsetDay
            // 
            this.txtCollectOffsetDay.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCollectOffsetDay.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCollectOffsetDay.DropDown.AllowDrop = false;
            this.txtCollectOffsetDay.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCollectOffsetDay.Format = "9";
            this.txtCollectOffsetDay.HighlightText = true;
            this.txtCollectOffsetDay.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCollectOffsetDay.Location = new System.Drawing.Point(305, 55);
            this.txtCollectOffsetDay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtCollectOffsetDay.MaxLength = 2;
            this.txtCollectOffsetDay.Name = "txtCollectOffsetDay";
            this.txtCollectOffsetDay.PaddingChar = '0';
            this.txtCollectOffsetDay.Required = false;
            this.txtCollectOffsetDay.Size = new System.Drawing.Size(30, 22);
            this.txtCollectOffsetDay.TabIndex = 9;
            this.txtCollectOffsetDay.TextChanged += new System.EventHandler(this.txtCollectOffsetDay_TextChanged);
            this.txtCollectOffsetDay.Validated += new System.EventHandler(this.txtCollectOffsetDay_Validated);
            // 
            // txtCollectOffsetMonth
            // 
            this.txtCollectOffsetMonth.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCollectOffsetMonth.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCollectOffsetMonth.DropDown.AllowDrop = false;
            this.txtCollectOffsetMonth.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCollectOffsetMonth.Format = "9";
            this.txtCollectOffsetMonth.HighlightText = true;
            this.txtCollectOffsetMonth.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCollectOffsetMonth.Location = new System.Drawing.Point(214, 55);
            this.txtCollectOffsetMonth.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtCollectOffsetMonth.MaxLength = 1;
            this.txtCollectOffsetMonth.Name = "txtCollectOffsetMonth";
            this.txtCollectOffsetMonth.PaddingChar = '0';
            this.txtCollectOffsetMonth.Required = false;
            this.txtCollectOffsetMonth.Size = new System.Drawing.Size(30, 22);
            this.txtCollectOffsetMonth.TabIndex = 7;
            // 
            // txtClosingDay
            // 
            this.txtClosingDay.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtClosingDay.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtClosingDay.DropDown.AllowDrop = false;
            this.txtClosingDay.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtClosingDay.Format = "9";
            this.txtClosingDay.HighlightText = true;
            this.txtClosingDay.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtClosingDay.Location = new System.Drawing.Point(99, 55);
            this.txtClosingDay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtClosingDay.MaxLength = 2;
            this.txtClosingDay.Name = "txtClosingDay";
            this.txtClosingDay.PaddingChar = '0';
            this.txtClosingDay.Required = false;
            this.txtClosingDay.Size = new System.Drawing.Size(30, 22);
            this.txtClosingDay.TabIndex = 5;
            this.txtClosingDay.Validated += new System.EventHandler(this.txtCloseDay_Validated);
            // 
            // txtStaffCode
            // 
            this.txtStaffCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtStaffCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtStaffCode.DropDown.AllowDrop = false;
            this.txtStaffCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtStaffCode.Format = "9";
            this.txtStaffCode.HighlightText = true;
            this.txtStaffCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtStaffCode.Location = new System.Drawing.Point(99, 24);
            this.txtStaffCode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtStaffCode.MaxLength = 10;
            this.txtStaffCode.Name = "txtStaffCode";
            this.txtStaffCode.PaddingChar = '0';
            this.txtStaffCode.Required = false;
            this.txtStaffCode.Size = new System.Drawing.Size(115, 22);
            this.txtStaffCode.TabIndex = 1;
            this.txtStaffCode.Validated += new System.EventHandler(this.txtStaffCode_Validated);
            // 
            // cmbCollectCategoryId
            // 
            this.cmbCollectCategoryId.DisplayMember = null;
            this.cmbCollectCategoryId.DropDown.AllowResize = false;
            this.cmbCollectCategoryId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCollectCategoryId.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbCollectCategoryId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbCollectCategoryId.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbCollectCategoryId.ListHeaderPane.Height = 22;
            this.cmbCollectCategoryId.ListHeaderPane.Visible = false;
            this.cmbCollectCategoryId.Location = new System.Drawing.Point(99, 113);
            this.cmbCollectCategoryId.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cmbCollectCategoryId.Name = "cmbCollectCategoryId";
            this.cmbCollectCategoryId.Required = false;
            this.cmbCollectCategoryId.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbCollectCategoryId.Size = new System.Drawing.Size(142, 22);
            this.cmbCollectCategoryId.TabIndex = 14;
            this.cmbCollectCategoryId.ValueMember = null;
            this.cmbCollectCategoryId.SelectedIndexChanged += new System.EventHandler(this.cmbCollectCategoryId_SelectedIndexChanged);
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // lblCollectCategoryId
            // 
            this.lblCollectCategoryId.Location = new System.Drawing.Point(26, 115);
            this.lblCollectCategoryId.Margin = new System.Windows.Forms.Padding(14, 4, 3, 4);
            this.lblCollectCategoryId.Name = "lblCollectCategoryId";
            this.lblCollectCategoryId.Size = new System.Drawing.Size(67, 16);
            this.lblCollectCategoryId.TabIndex = 13;
            this.lblCollectCategoryId.Text = "回収方法";
            this.lblCollectCategoryId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStaffCode
            // 
            this.btnStaffCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnStaffCode.Location = new System.Drawing.Point(220, 22);
            this.btnStaffCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStaffCode.Name = "btnStaffCode";
            this.btnStaffCode.Size = new System.Drawing.Size(24, 24);
            this.btnStaffCode.TabIndex = 2;
            this.btnStaffCode.UseVisualStyleBackColor = true;
            this.btnStaffCode.Click += new System.EventHandler(this.btnStaffCode_Click);
            // 
            // lblStaffName
            // 
            this.lblStaffName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStaffName.DropDown.AllowDrop = false;
            this.lblStaffName.Enabled = false;
            this.lblStaffName.HighlightText = true;
            this.lblStaffName.Location = new System.Drawing.Point(250, 24);
            this.lblStaffName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.ReadOnly = true;
            this.lblStaffName.Required = false;
            this.lblStaffName.Size = new System.Drawing.Size(253, 22);
            this.lblStaffName.TabIndex = 3;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.Location = new System.Drawing.Point(26, 26);
            this.lblStaffCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Size = new System.Drawing.Size(67, 16);
            this.lblStaffCode.TabIndex = 0;
            this.lblStaffCode.Text = "営業担当者";
            this.lblStaffCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxBilling
            // 
            this.gbxBilling.Controls.Add(this.lblBillingCategory);
            this.gbxBilling.Controls.Add(this.txtBillingCategory);
            this.gbxBilling.Controls.Add(this.btnBillingCategory);
            this.gbxBilling.Controls.Add(this.lblBillingCategoryName);
            this.gbxBilling.Location = new System.Drawing.Point(14, 19);
            this.gbxBilling.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxBilling.Name = "gbxBilling";
            this.gbxBilling.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxBilling.Size = new System.Drawing.Size(551, 62);
            this.gbxBilling.TabIndex = 0;
            this.gbxBilling.TabStop = false;
            this.gbxBilling.Text = "請求データ";
            // 
            // lblBillingCategory
            // 
            this.lblBillingCategory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBillingCategory.Location = new System.Drawing.Point(26, 26);
            this.lblBillingCategory.Name = "lblBillingCategory";
            this.lblBillingCategory.Size = new System.Drawing.Size(67, 16);
            this.lblBillingCategory.TabIndex = 0;
            this.lblBillingCategory.Text = "請求区分";
            this.lblBillingCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBillingCategory
            // 
            this.txtBillingCategory.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtBillingCategory.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtBillingCategory.DropDown.AllowDrop = false;
            this.txtBillingCategory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBillingCategory.Format = "9";
            this.txtBillingCategory.HighlightText = true;
            this.txtBillingCategory.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBillingCategory.Location = new System.Drawing.Point(99, 24);
            this.txtBillingCategory.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtBillingCategory.MaxLength = 2;
            this.txtBillingCategory.Name = "txtBillingCategory";
            this.txtBillingCategory.PaddingChar = '0';
            this.txtBillingCategory.Required = false;
            this.txtBillingCategory.Size = new System.Drawing.Size(30, 22);
            this.txtBillingCategory.TabIndex = 1;
            this.txtBillingCategory.Validated += new System.EventHandler(this.txtBillingCategory_Validated);
            // 
            // btnBillingCategory
            // 
            this.btnBillingCategory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnBillingCategory.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnBillingCategory.Location = new System.Drawing.Point(135, 22);
            this.btnBillingCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBillingCategory.Name = "btnBillingCategory";
            this.btnBillingCategory.Size = new System.Drawing.Size(24, 24);
            this.btnBillingCategory.TabIndex = 2;
            this.btnBillingCategory.UseVisualStyleBackColor = true;
            this.btnBillingCategory.Click += new System.EventHandler(this.btnBillingCategory_Click);
            // 
            // lblBillingCategoryName
            // 
            this.lblBillingCategoryName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBillingCategoryName.DropDown.AllowDrop = false;
            this.lblBillingCategoryName.Enabled = false;
            this.lblBillingCategoryName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBillingCategoryName.HighlightText = true;
            this.lblBillingCategoryName.Location = new System.Drawing.Point(165, 24);
            this.lblBillingCategoryName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblBillingCategoryName.Name = "lblBillingCategoryName";
            this.lblBillingCategoryName.ReadOnly = true;
            this.lblBillingCategoryName.Required = false;
            this.lblBillingCategoryName.Size = new System.Drawing.Size(338, 22);
            this.lblBillingCategoryName.TabIndex = 3;
            // 
            // dlgMFBillingExtractSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 310);
            this.Controls.Add(this.gbxBilling);
            this.Controls.Add(this.gbxCustomer);
            this.Controls.Add(this.btnF01);
            this.Controls.Add(this.btnF10);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "dlgMFBillingExtractSetting";
            this.Text = "取込設定";
            this.XmlMessenger = xmlMessenger1;
            this.Load += new System.EventHandler(this.dlgPublishInvoices_Load);
            this.Controls.SetChildIndex(this.btnF10, 0);
            this.Controls.SetChildIndex(this.btnF01, 0);
            this.Controls.SetChildIndex(this.gbxCustomer, 0);
            this.Controls.SetChildIndex(this.gbxBilling, 0);
            this.gbxCustomer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectOffsetDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCollectOffsetMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClosingDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCollectCategoryId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffName)).EndInit();
            this.gbxBilling.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBillingCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblBillingCategoryName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnF10;
        private System.Windows.Forms.Button btnF01;
        private Common.Controls.VOneLabelControl lblClosingDay;
        private Common.Controls.VOneLabelControl lblRecoveryPlan;
        private Common.Controls.VOneLabelControl lblDay;
        private Common.Controls.VOneLabelControl lblAfterMonth;
        private System.Windows.Forms.GroupBox gbxCustomer;
        private System.Windows.Forms.GroupBox gbxBilling;
        private Common.Controls.VOneLabelControl lblBillingCategory;
        private Common.Controls.VOneTextControl txtBillingCategory;
        private System.Windows.Forms.Button btnBillingCategory;
        private Common.Controls.VOneDispLabelControl lblBillingCategoryName;
        private System.Windows.Forms.Button btnStaffCode;
        private Common.Controls.VOneDispLabelControl lblStaffName;
        private Common.Controls.VOneLabelControl lblStaffCode;
        private Common.Controls.VOneLabelControl lblCollectCategoryId;
        private Common.Controls.VOneTextControl txtCollectOffsetDay;
        private Common.Controls.VOneTextControl txtCollectOffsetMonth;
        private Common.Controls.VOneTextControl txtClosingDay;
        private Common.Controls.VOneTextControl txtStaffCode;
        private Common.Controls.VOneComboControl cmbCollectCategoryId;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private System.Windows.Forms.CheckBox cbxIssueBillEachTime;
        private Common.Controls.VOneLabelControl lblLimitDay;
    }
}