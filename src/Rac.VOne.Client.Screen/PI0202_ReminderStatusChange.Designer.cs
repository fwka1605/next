namespace Rac.VOne.Client.Screen
{
    partial class PI0202
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
            this.cmbStatus = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCustomerCode = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCustomerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCustomerName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblBilledAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datBilledAt = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.nmbRemainAmount = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.lblRemainAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblReminderAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.nmbReminderAmount = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.cbxStatus = new System.Windows.Forms.CheckBox();
            this.txtMemo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cbxMemo = new System.Windows.Forms.CheckBox();
            this.pnlReminder = new System.Windows.Forms.Panel();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.pnlMemo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datBilledAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbRemainAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbReminderAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).BeginInit();
            this.pnlReminder.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.pnlMemo.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbStatus
            // 
            this.cmbStatus.DisplayMember = null;
            this.cmbStatus.DropDown.AllowResize = false;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbStatus.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbStatus.ListHeaderPane.Height = 22;
            this.cmbStatus.ListHeaderPane.Visible = false;
            this.cmbStatus.Location = new System.Drawing.Point(134, 6);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Required = false;
            this.cmbStatus.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbStatus.Size = new System.Drawing.Size(304, 22);
            this.cmbStatus.TabIndex = 3;
            this.cmbStatus.ValueMember = null;
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Location = new System.Drawing.Point(20, 10);
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Size = new System.Drawing.Size(108, 16);
            this.lblCustomerCode.TabIndex = 2;
            this.lblCustomerCode.Text = "得意先コード";
            this.lblCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.DropDown.AllowDrop = false;
            this.txtCustomerCode.Enabled = false;
            this.txtCustomerCode.HighlightText = true;
            this.txtCustomerCode.Location = new System.Drawing.Point(134, 8);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.ReadOnly = true;
            this.txtCustomerCode.Required = false;
            this.txtCustomerCode.Size = new System.Drawing.Size(120, 20);
            this.txtCustomerCode.TabIndex = 4;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Location = new System.Drawing.Point(20, 34);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(108, 16);
            this.lblCustomerName.TabIndex = 2;
            this.lblCustomerName.Text = "得意先名";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.DropDown.AllowDrop = false;
            this.txtCustomerName.Enabled = false;
            this.txtCustomerName.HighlightText = true;
            this.txtCustomerName.Location = new System.Drawing.Point(134, 34);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.ReadOnly = true;
            this.txtCustomerName.Required = false;
            this.txtCustomerName.Size = new System.Drawing.Size(301, 20);
            this.txtCustomerName.TabIndex = 4;
            // 
            // lblBilledAt
            // 
            this.lblBilledAt.Location = new System.Drawing.Point(20, 62);
            this.lblBilledAt.Name = "lblBilledAt";
            this.lblBilledAt.Size = new System.Drawing.Size(108, 16);
            this.lblBilledAt.TabIndex = 2;
            this.lblBilledAt.Text = "請求日";
            this.lblBilledAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datBilledAt
            // 
            this.datBilledAt.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datBilledAt.Enabled = false;
            this.datBilledAt.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datBilledAt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datBilledAt.Location = new System.Drawing.Point(134, 60);
            this.datBilledAt.Name = "datBilledAt";
            this.datBilledAt.Required = false;
            this.datBilledAt.Size = new System.Drawing.Size(120, 20);
            this.datBilledAt.Spin.AllowSpin = false;
            this.datBilledAt.TabIndex = 5;
            this.datBilledAt.Value = new System.DateTime(2018, 1, 30, 0, 0, 0, 0);
            // 
            // nmbRemainAmount
            // 
            this.nmbRemainAmount.AllowDeleteToNull = true;
            this.nmbRemainAmount.DropDown.AllowDrop = false;
            this.nmbRemainAmount.Enabled = false;
            this.nmbRemainAmount.Fields.IntegerPart.MinDigits = 1;
            this.nmbRemainAmount.HighlightText = true;
            this.nmbRemainAmount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbRemainAmount.Location = new System.Drawing.Point(134, 86);
            this.nmbRemainAmount.Name = "nmbRemainAmount";
            this.nmbRemainAmount.Required = false;
            this.nmbRemainAmount.Size = new System.Drawing.Size(120, 20);
            this.nmbRemainAmount.Spin.AllowSpin = false;
            this.nmbRemainAmount.TabIndex = 6;
            // 
            // lblRemainAmount
            // 
            this.lblRemainAmount.Location = new System.Drawing.Point(20, 88);
            this.lblRemainAmount.Name = "lblRemainAmount";
            this.lblRemainAmount.Size = new System.Drawing.Size(108, 16);
            this.lblRemainAmount.TabIndex = 2;
            this.lblRemainAmount.Text = "請求残";
            this.lblRemainAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReminderAmount
            // 
            this.lblReminderAmount.Location = new System.Drawing.Point(20, 114);
            this.lblReminderAmount.Name = "lblReminderAmount";
            this.lblReminderAmount.Size = new System.Drawing.Size(108, 16);
            this.lblReminderAmount.TabIndex = 2;
            this.lblReminderAmount.Text = "滞留金額";
            this.lblReminderAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nmbReminderAmount
            // 
            this.nmbReminderAmount.AllowDeleteToNull = true;
            this.nmbReminderAmount.DropDown.AllowDrop = false;
            this.nmbReminderAmount.Enabled = false;
            this.nmbReminderAmount.Fields.IntegerPart.MinDigits = 1;
            this.nmbReminderAmount.HighlightText = true;
            this.nmbReminderAmount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbReminderAmount.Location = new System.Drawing.Point(134, 112);
            this.nmbReminderAmount.Name = "nmbReminderAmount";
            this.nmbReminderAmount.Required = false;
            this.nmbReminderAmount.Size = new System.Drawing.Size(120, 20);
            this.nmbReminderAmount.Spin.AllowSpin = false;
            this.nmbReminderAmount.TabIndex = 6;
            // 
            // cbxStatus
            // 
            this.cbxStatus.AutoSize = true;
            this.cbxStatus.Location = new System.Drawing.Point(23, 8);
            this.cbxStatus.Name = "cbxStatus";
            this.cbxStatus.Size = new System.Drawing.Size(71, 19);
            this.cbxStatus.TabIndex = 7;
            this.cbxStatus.Text = "ステータス";
            this.cbxStatus.UseVisualStyleBackColor = true;
            // 
            // txtMemo
            // 
            this.txtMemo.AcceptsCrLf = GrapeCity.Win.Editors.CrLfMode.NoControl;
            this.txtMemo.DropDown.AllowDrop = false;
            this.txtMemo.HighlightText = true;
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(134, 6);
            this.txtMemo.MaxLength = 100;
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Required = false;
            this.txtMemo.Size = new System.Drawing.Size(304, 85);
            this.txtMemo.TabIndex = 8;
            // 
            // cbxMemo
            // 
            this.cbxMemo.AutoSize = true;
            this.cbxMemo.Location = new System.Drawing.Point(23, 8);
            this.cbxMemo.Name = "cbxMemo";
            this.cbxMemo.Size = new System.Drawing.Size(74, 19);
            this.cbxMemo.TabIndex = 7;
            this.cbxMemo.Text = "対応記録";
            this.cbxMemo.UseVisualStyleBackColor = true;
            // 
            // pnlReminder
            // 
            this.pnlReminder.Controls.Add(this.lblCustomerCode);
            this.pnlReminder.Controls.Add(this.lblCustomerName);
            this.pnlReminder.Controls.Add(this.lblBilledAt);
            this.pnlReminder.Controls.Add(this.lblRemainAmount);
            this.pnlReminder.Controls.Add(this.nmbReminderAmount);
            this.pnlReminder.Controls.Add(this.lblReminderAmount);
            this.pnlReminder.Controls.Add(this.nmbRemainAmount);
            this.pnlReminder.Controls.Add(this.txtCustomerCode);
            this.pnlReminder.Controls.Add(this.datBilledAt);
            this.pnlReminder.Controls.Add(this.txtCustomerName);
            this.pnlReminder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReminder.Location = new System.Drawing.Point(12, 12);
            this.pnlReminder.Name = "pnlReminder";
            this.pnlReminder.Size = new System.Drawing.Size(466, 138);
            this.pnlReminder.TabIndex = 9;
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.cbxStatus);
            this.pnlStatus.Controls.Add(this.cmbStatus);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatus.Location = new System.Drawing.Point(12, 150);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(466, 34);
            this.pnlStatus.TabIndex = 10;
            // 
            // pnlMemo
            // 
            this.pnlMemo.Controls.Add(this.txtMemo);
            this.pnlMemo.Controls.Add(this.cbxMemo);
            this.pnlMemo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMemo.Location = new System.Drawing.Point(12, 184);
            this.pnlMemo.Name = "pnlMemo";
            this.pnlMemo.Size = new System.Drawing.Size(466, 97);
            this.pnlMemo.TabIndex = 11;
            // 
            // PI0202
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlMemo);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pnlReminder);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PI0202";
            this.Size = new System.Drawing.Size(490, 300);
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datBilledAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbRemainAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmbReminderAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).EndInit();
            this.pnlReminder.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.pnlMemo.ResumeLayout(false);
            this.pnlMemo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneComboControl cmbStatus;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneLabelControl lblCustomerCode;
        private Common.Controls.VOneDispLabelControl txtCustomerCode;
        private Common.Controls.VOneLabelControl lblCustomerName;
        private Common.Controls.VOneDispLabelControl txtCustomerName;
        private Common.Controls.VOneLabelControl lblBilledAt;
        private Common.Controls.VOneDateControl datBilledAt;
        private Common.Controls.VOneNumberControl nmbRemainAmount;
        private Common.Controls.VOneLabelControl lblRemainAmount;
        private Common.Controls.VOneLabelControl lblReminderAmount;
        private Common.Controls.VOneNumberControl nmbReminderAmount;
        private System.Windows.Forms.CheckBox cbxStatus;
        private Common.Controls.VOneTextControl txtMemo;
        private System.Windows.Forms.CheckBox cbxMemo;
        private System.Windows.Forms.Panel pnlReminder;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Panel pnlMemo;
    }
}
