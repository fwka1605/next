namespace Rac.VOne.Client.Screen
{
    partial class PI0206
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
            this.pnlMemo = new System.Windows.Forms.Panel();
            this.txtMemo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.vOneLabelControl2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.cmbStatus = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblStatus = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlReminder = new System.Windows.Forms.Panel();
            this.lblCreateAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblInputType = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCreateBy = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.nmbReminderAmount = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.lblReminderAmount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCreateAtDisplay = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCreateByName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblInputTypeName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.pnlMemo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).BeginInit();
            this.pnlStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus)).BeginInit();
            this.pnlReminder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbReminderAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAtDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateByName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputTypeName)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMemo
            // 
            this.pnlMemo.Controls.Add(this.txtMemo);
            this.pnlMemo.Controls.Add(this.vOneLabelControl2);
            this.pnlMemo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMemo.Location = new System.Drawing.Point(12, 184);
            this.pnlMemo.Name = "pnlMemo";
            this.pnlMemo.Size = new System.Drawing.Size(466, 97);
            this.pnlMemo.TabIndex = 14;
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
            // vOneLabelControl2
            // 
            this.vOneLabelControl2.Location = new System.Drawing.Point(20, 6);
            this.vOneLabelControl2.Name = "vOneLabelControl2";
            this.vOneLabelControl2.Size = new System.Drawing.Size(108, 16);
            this.vOneLabelControl2.TabIndex = 2;
            this.vOneLabelControl2.Text = "対応記録";
            this.vOneLabelControl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.cmbStatus);
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatus.Location = new System.Drawing.Point(12, 150);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(466, 34);
            this.pnlStatus.TabIndex = 13;
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
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(20, 6);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(108, 16);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "ステータス";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlReminder
            // 
            this.pnlReminder.Controls.Add(this.lblCreateAt);
            this.pnlReminder.Controls.Add(this.lblInputType);
            this.pnlReminder.Controls.Add(this.lblCreateBy);
            this.pnlReminder.Controls.Add(this.nmbReminderAmount);
            this.pnlReminder.Controls.Add(this.lblReminderAmount);
            this.pnlReminder.Controls.Add(this.lblCreateAtDisplay);
            this.pnlReminder.Controls.Add(this.lblCreateByName);
            this.pnlReminder.Controls.Add(this.lblInputTypeName);
            this.pnlReminder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReminder.Location = new System.Drawing.Point(12, 12);
            this.pnlReminder.Name = "pnlReminder";
            this.pnlReminder.Size = new System.Drawing.Size(466, 138);
            this.pnlReminder.TabIndex = 12;
            // 
            // lblCreateAt
            // 
            this.lblCreateAt.Location = new System.Drawing.Point(20, 17);
            this.lblCreateAt.Name = "lblCreateAt";
            this.lblCreateAt.Size = new System.Drawing.Size(108, 16);
            this.lblCreateAt.TabIndex = 2;
            this.lblCreateAt.Text = "更新日時";
            this.lblCreateAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInputType
            // 
            this.lblInputType.Location = new System.Drawing.Point(20, 43);
            this.lblInputType.Name = "lblInputType";
            this.lblInputType.Size = new System.Drawing.Size(108, 16);
            this.lblInputType.TabIndex = 2;
            this.lblInputType.Text = "アクション";
            this.lblInputType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCreateBy
            // 
            this.lblCreateBy.Location = new System.Drawing.Point(20, 101);
            this.lblCreateBy.Name = "lblCreateBy";
            this.lblCreateBy.Size = new System.Drawing.Size(108, 16);
            this.lblCreateBy.TabIndex = 2;
            this.lblCreateBy.Text = "更新者名";
            this.lblCreateBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nmbReminderAmount
            // 
            this.nmbReminderAmount.AllowDeleteToNull = true;
            this.nmbReminderAmount.DropDown.AllowDrop = false;
            this.nmbReminderAmount.Enabled = false;
            this.nmbReminderAmount.Fields.IntegerPart.MinDigits = 1;
            this.nmbReminderAmount.HighlightText = true;
            this.nmbReminderAmount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbReminderAmount.Location = new System.Drawing.Point(134, 71);
            this.nmbReminderAmount.Name = "nmbReminderAmount";
            this.nmbReminderAmount.Required = false;
            this.nmbReminderAmount.Size = new System.Drawing.Size(120, 22);
            this.nmbReminderAmount.Spin.AllowSpin = false;
            this.nmbReminderAmount.TabIndex = 6;
            // 
            // lblReminderAmount
            // 
            this.lblReminderAmount.Location = new System.Drawing.Point(20, 73);
            this.lblReminderAmount.Name = "lblReminderAmount";
            this.lblReminderAmount.Size = new System.Drawing.Size(108, 16);
            this.lblReminderAmount.TabIndex = 2;
            this.lblReminderAmount.Text = "滞留金額";
            this.lblReminderAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCreateAtDisplay
            // 
            this.lblCreateAtDisplay.DropDown.AllowDrop = false;
            this.lblCreateAtDisplay.Enabled = false;
            this.lblCreateAtDisplay.HighlightText = true;
            this.lblCreateAtDisplay.Location = new System.Drawing.Point(134, 15);
            this.lblCreateAtDisplay.Name = "lblCreateAtDisplay";
            this.lblCreateAtDisplay.ReadOnly = true;
            this.lblCreateAtDisplay.Required = false;
            this.lblCreateAtDisplay.Size = new System.Drawing.Size(166, 22);
            this.lblCreateAtDisplay.TabIndex = 4;
            // 
            // lblCreateByName
            // 
            this.lblCreateByName.DropDown.AllowDrop = false;
            this.lblCreateByName.Enabled = false;
            this.lblCreateByName.HighlightText = true;
            this.lblCreateByName.Location = new System.Drawing.Point(134, 99);
            this.lblCreateByName.Name = "lblCreateByName";
            this.lblCreateByName.ReadOnly = true;
            this.lblCreateByName.Required = false;
            this.lblCreateByName.Size = new System.Drawing.Size(166, 22);
            this.lblCreateByName.TabIndex = 4;
            // 
            // lblInputTypeName
            // 
            this.lblInputTypeName.DropDown.AllowDrop = false;
            this.lblInputTypeName.Enabled = false;
            this.lblInputTypeName.HighlightText = true;
            this.lblInputTypeName.Location = new System.Drawing.Point(134, 43);
            this.lblInputTypeName.Name = "lblInputTypeName";
            this.lblInputTypeName.ReadOnly = true;
            this.lblInputTypeName.Required = false;
            this.lblInputTypeName.Size = new System.Drawing.Size(166, 22);
            this.lblInputTypeName.TabIndex = 4;
            // 
            // PI0206
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMemo);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pnlReminder);
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PI0206";
            this.Size = new System.Drawing.Size(490, 300);
            this.pnlMemo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus)).EndInit();
            this.pnlReminder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmbReminderAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateAtDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreateByName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInputTypeName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMemo;
        private Common.Controls.VOneLabelControl vOneLabelControl2;
        private System.Windows.Forms.Panel pnlStatus;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneLabelControl lblStatus;
        private System.Windows.Forms.Panel pnlReminder;
        private Common.Controls.VOneLabelControl lblCreateAt;
        private Common.Controls.VOneLabelControl lblInputType;
        private Common.Controls.VOneLabelControl lblCreateBy;
        private Common.Controls.VOneNumberControl nmbReminderAmount;
        private Common.Controls.VOneLabelControl lblReminderAmount;
        private Common.Controls.VOneDispLabelControl lblCreateAtDisplay;
        private Common.Controls.VOneDispLabelControl lblCreateByName;
        private Common.Controls.VOneDispLabelControl lblInputTypeName;
        internal Common.Controls.VOneTextControl txtMemo;
        internal Common.Controls.VOneComboControl cmbStatus;
    }
}
