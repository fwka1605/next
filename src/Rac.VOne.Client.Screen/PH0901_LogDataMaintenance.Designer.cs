namespace Rac.VOne.Client.Screen
{
    partial class PH0901
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
            this.gbxLogSearchItem = new System.Windows.Forms.GroupBox();
            this.datLoggedAtFrom = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblLoginUserName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblUserCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datLoggedAtTo = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblLogged = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnLoginUser = new System.Windows.Forms.Button();
            this.lblLogDataTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtLoginUserCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblHeaderTitle = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxLogData = new System.Windows.Forms.GroupBox();
            this.grdLogData = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxLogAction = new System.Windows.Forms.GroupBox();
            this.lblLogCount = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblLogCollection = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblLoggedAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblLoggedCounts = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnLoggedSwitch = new System.Windows.Forms.Button();
            this.lblFrom = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCounts = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblLoggedSwitch = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxLogSearchItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datLoggedAtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datLoggedAtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserCode)).BeginInit();
            this.gbxLogData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLogData)).BeginInit();
            this.gbxLogAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxLogSearchItem
            // 
            this.gbxLogSearchItem.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxLogSearchItem.Controls.Add(this.datLoggedAtFrom);
            this.gbxLogSearchItem.Controls.Add(this.lblLoginUserName);
            this.gbxLogSearchItem.Controls.Add(this.lblUserCode);
            this.gbxLogSearchItem.Controls.Add(this.datLoggedAtTo);
            this.gbxLogSearchItem.Controls.Add(this.lblLogged);
            this.gbxLogSearchItem.Controls.Add(this.btnLoginUser);
            this.gbxLogSearchItem.Controls.Add(this.lblLogDataTo);
            this.gbxLogSearchItem.Controls.Add(this.txtLoginUserCode);
            this.gbxLogSearchItem.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxLogSearchItem.Location = new System.Drawing.Point(72, 39);
            this.gbxLogSearchItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.gbxLogSearchItem.Name = "gbxLogSearchItem";
            this.gbxLogSearchItem.Size = new System.Drawing.Size(864, 86);
            this.gbxLogSearchItem.TabIndex = 0;
            this.gbxLogSearchItem.TabStop = false;
            this.gbxLogSearchItem.Text = "ログ検索項目";
            // 
            // datLoggedAtFrom
            // 
            this.datLoggedAtFrom.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datLoggedAtFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datLoggedAtFrom.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datLoggedAtFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datLoggedAtFrom.Location = new System.Drawing.Point(116, 23);
            this.datLoggedAtFrom.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.datLoggedAtFrom.Name = "datLoggedAtFrom";
            this.datLoggedAtFrom.Required = false;
            this.datLoggedAtFrom.Size = new System.Drawing.Size(115, 22);
            this.datLoggedAtFrom.Spin.AllowSpin = false;
            this.datLoggedAtFrom.TabIndex = 1;
            this.datLoggedAtFrom.Value = null;
            // 
            // lblLoginUserName
            // 
            this.lblLoginUserName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLoginUserName.DropDown.AllowDrop = false;
            this.lblLoginUserName.Enabled = false;
            this.lblLoginUserName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginUserName.HighlightText = true;
            this.lblLoginUserName.Location = new System.Drawing.Point(273, 53);
            this.lblLoginUserName.Name = "lblLoginUserName";
            this.lblLoginUserName.ReadOnly = true;
            this.lblLoginUserName.Required = false;
            this.lblLoginUserName.Size = new System.Drawing.Size(290, 22);
            this.lblLoginUserName.TabIndex = 7;
            // 
            // lblUserCode
            // 
            this.lblUserCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserCode.Location = new System.Drawing.Point(43, 55);
            this.lblUserCode.Name = "lblUserCode";
            this.lblUserCode.Size = new System.Drawing.Size(67, 16);
            this.lblUserCode.TabIndex = 4;
            this.lblUserCode.Text = "実行担当者";
            this.lblUserCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datLoggedAtTo
            // 
            this.datLoggedAtTo.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datLoggedAtTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datLoggedAtTo.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datLoggedAtTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datLoggedAtTo.Location = new System.Drawing.Point(273, 23);
            this.datLoggedAtTo.Margin = new System.Windows.Forms.Padding(6);
            this.datLoggedAtTo.Name = "datLoggedAtTo";
            this.datLoggedAtTo.Required = false;
            this.datLoggedAtTo.Size = new System.Drawing.Size(115, 22);
            this.datLoggedAtTo.Spin.AllowSpin = false;
            this.datLoggedAtTo.TabIndex = 3;
            this.datLoggedAtTo.Value = null;
            // 
            // lblLogged
            // 
            this.lblLogged.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogged.Location = new System.Drawing.Point(43, 25);
            this.lblLogged.Name = "lblLogged";
            this.lblLogged.Size = new System.Drawing.Size(67, 16);
            this.lblLogged.TabIndex = 0;
            this.lblLogged.Text = "日時";
            this.lblLogged.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLoginUser
            // 
            this.btnLoginUser.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnLoginUser.Location = new System.Drawing.Point(240, 52);
            this.btnLoginUser.Name = "btnLoginUser";
            this.btnLoginUser.Size = new System.Drawing.Size(24, 24);
            this.btnLoginUser.TabIndex = 6;
            this.btnLoginUser.UseVisualStyleBackColor = false;
            this.btnLoginUser.Click += new System.EventHandler(this.btnLoginUser_Click);
            // 
            // lblLogDataTo
            // 
            this.lblLogDataTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogDataTo.Location = new System.Drawing.Point(242, 25);
            this.lblLogDataTo.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblLogDataTo.Name = "lblLogDataTo";
            this.lblLogDataTo.Size = new System.Drawing.Size(20, 16);
            this.lblLogDataTo.TabIndex = 2;
            this.lblLogDataTo.Text = "～";
            this.lblLogDataTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtLoginUserCode
            // 
            this.txtLoginUserCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtLoginUserCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtLoginUserCode.DropDown.AllowDrop = false;
            this.txtLoginUserCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginUserCode.Format = "A9";
            this.txtLoginUserCode.HighlightText = true;
            this.txtLoginUserCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtLoginUserCode.Location = new System.Drawing.Point(116, 53);
            this.txtLoginUserCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLoginUserCode.Name = "txtLoginUserCode";
            this.txtLoginUserCode.Required = false;
            this.txtLoginUserCode.Size = new System.Drawing.Size(115, 22);
            this.txtLoginUserCode.TabIndex = 5;
            this.txtLoginUserCode.Validated += new System.EventHandler(this.txtLoginUserCode_Validated);
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.Location = new System.Drawing.Point(69, 15);
            this.lblHeaderTitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(73, 16);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "操作ログ管理";
            this.lblHeaderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxLogData
            // 
            this.gbxLogData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxLogData.BackColor = System.Drawing.SystemColors.Control;
            this.gbxLogData.Controls.Add(this.grdLogData);
            this.gbxLogData.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxLogData.Location = new System.Drawing.Point(72, 125);
            this.gbxLogData.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.gbxLogData.Name = "gbxLogData";
            this.gbxLogData.Padding = new System.Windows.Forms.Padding(0);
            this.gbxLogData.Size = new System.Drawing.Size(864, 418);
            this.gbxLogData.TabIndex = 1;
            this.gbxLogData.TabStop = false;
            // 
            // grdLogData
            // 
            this.grdLogData.AllowAutoExtend = true;
            this.grdLogData.AllowUserToAddRows = false;
            this.grdLogData.AllowUserToShiftSelect = true;
            this.grdLogData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdLogData.HorizontalScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.grdLogData.Location = new System.Drawing.Point(8, 17);
            this.grdLogData.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grdLogData.Name = "grdLogData";
            this.grdLogData.Size = new System.Drawing.Size(846, 393);
            this.grdLogData.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdLogData.TabIndex = 0;
            this.grdLogData.Text = "vOneGridControl1";
            // 
            // gbxLogAction
            // 
            this.gbxLogAction.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxLogAction.Controls.Add(this.lblLogCount);
            this.gbxLogAction.Controls.Add(this.lblLogCollection);
            this.gbxLogAction.Controls.Add(this.lblLoggedAt);
            this.gbxLogAction.Controls.Add(this.lblLoggedCounts);
            this.gbxLogAction.Controls.Add(this.btnLoggedSwitch);
            this.gbxLogAction.Controls.Add(this.lblFrom);
            this.gbxLogAction.Controls.Add(this.lblCounts);
            this.gbxLogAction.Controls.Add(this.lblLoggedSwitch);
            this.gbxLogAction.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxLogAction.Location = new System.Drawing.Point(72, 543);
            this.gbxLogAction.Name = "gbxLogAction";
            this.gbxLogAction.Size = new System.Drawing.Size(864, 66);
            this.gbxLogAction.TabIndex = 3;
            this.gbxLogAction.TabStop = false;
            // 
            // lblLogCount
            // 
            this.lblLogCount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLogCount.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogCount.Location = new System.Drawing.Point(290, 29);
            this.lblLogCount.Name = "lblLogCount";
            this.lblLogCount.Size = new System.Drawing.Size(49, 16);
            this.lblLogCount.TabIndex = 6;
            this.lblLogCount.Text = "ログ件数";
            this.lblLogCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLogCollection
            // 
            this.lblLogCollection.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLogCollection.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogCollection.Location = new System.Drawing.Point(40, 29);
            this.lblLogCollection.Name = "lblLogCollection";
            this.lblLogCollection.Size = new System.Drawing.Size(49, 16);
            this.lblLogCollection.TabIndex = 0;
            this.lblLogCollection.Text = "ログ採取";
            this.lblLogCollection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLoggedAt
            // 
            this.lblLoggedAt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLoggedAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggedAt.Location = new System.Drawing.Point(345, 29);
            this.lblLoggedAt.Margin = new System.Windows.Forms.Padding(3);
            this.lblLoggedAt.Name = "lblLoggedAt";
            this.lblLoggedAt.Size = new System.Drawing.Size(135, 16);
            this.lblLoggedAt.TabIndex = 0;
            this.lblLoggedAt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLoggedCounts
            // 
            this.lblLoggedCounts.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLoggedCounts.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggedCounts.Location = new System.Drawing.Point(517, 29);
            this.lblLoggedCounts.Margin = new System.Windows.Forms.Padding(3);
            this.lblLoggedCounts.Name = "lblLoggedCounts";
            this.lblLoggedCounts.Size = new System.Drawing.Size(115, 16);
            this.lblLoggedCounts.TabIndex = 0;
            this.lblLoggedCounts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLoggedSwitch
            // 
            this.btnLoggedSwitch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLoggedSwitch.Location = new System.Drawing.Point(168, 25);
            this.btnLoggedSwitch.Name = "btnLoggedSwitch";
            this.btnLoggedSwitch.Size = new System.Drawing.Size(63, 24);
            this.btnLoggedSwitch.TabIndex = 5;
            this.btnLoggedSwitch.Text = "切替";
            this.btnLoggedSwitch.UseVisualStyleBackColor = true;
            this.btnLoggedSwitch.Click += new System.EventHandler(this.btnLoggedSwitch_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(486, 29);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(25, 16);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "から";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCounts
            // 
            this.lblCounts.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCounts.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCounts.Location = new System.Drawing.Point(638, 29);
            this.lblCounts.Name = "lblCounts";
            this.lblCounts.Size = new System.Drawing.Size(19, 16);
            this.lblCounts.TabIndex = 0;
            this.lblCounts.Text = "件";
            this.lblCounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLoggedSwitch
            // 
            this.lblLoggedSwitch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLoggedSwitch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggedSwitch.Location = new System.Drawing.Point(97, 29);
            this.lblLoggedSwitch.Margin = new System.Windows.Forms.Padding(3);
            this.lblLoggedSwitch.Name = "lblLoggedSwitch";
            this.lblLoggedSwitch.Size = new System.Drawing.Size(60, 16);
            this.lblLoggedSwitch.TabIndex = 0;
            this.lblLoggedSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PH0901
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblHeaderTitle);
            this.Controls.Add(this.gbxLogSearchItem);
            this.Controls.Add(this.gbxLogAction);
            this.Controls.Add(this.gbxLogData);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PH0901";
            this.Load += new System.EventHandler(this.PH0901_Load);
            this.gbxLogSearchItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datLoggedAtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLoginUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datLoggedAtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginUserCode)).EndInit();
            this.gbxLogData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdLogData)).EndInit();
            this.gbxLogAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxLogSearchItem;
        private Common.Controls.VOneGridControl grdLogData;
        private Common.Controls.VOneLabelControl lblHeaderTitle;
        private System.Windows.Forms.GroupBox gbxLogData;
        private System.Windows.Forms.GroupBox gbxLogAction;
        private Common.Controls.VOneLabelControl lblLoggedSwitch;
        private System.Windows.Forms.Button btnLoggedSwitch;
        private Common.Controls.VOneLabelControl lblLoggedCounts;
        private Common.Controls.VOneLabelControl lblLogCollection;
        private Common.Controls.VOneLabelControl lblFrom;
        private Common.Controls.VOneLabelControl lblCounts;
        private Common.Controls.VOneLabelControl lblLoggedAt;
        private Common.Controls.VOneLabelControl lblLogCount;
        private Common.Controls.VOneLabelControl lblLogged;
        private Common.Controls.VOneLabelControl lblUserCode;
        private Common.Controls.VOneDateControl datLoggedAtFrom;
        private Common.Controls.VOneTextControl txtLoginUserCode;
        private Common.Controls.VOneLabelControl lblLogDataTo;
        private System.Windows.Forms.Button btnLoginUser;
        private Common.Controls.VOneDateControl datLoggedAtTo;
        private Common.Controls.VOneDispLabelControl lblLoginUserName;
    }
}
