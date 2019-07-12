namespace Rac.VOne.Client.Screen
{
    partial class PH0701
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
            this.gbxPreview = new System.Windows.Forms.GroupBox();
            this.grdPreview = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxGridSetting = new System.Windows.Forms.GroupBox();
            this.gbxMessage = new System.Windows.Forms.GroupBox();
            this.lblExplain = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxScreen = new System.Windows.Forms.GroupBox();
            this.cmbGridType = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblScreen = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdGridColumnSetting = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxUpdateLoginUser = new System.Windows.Forms.GroupBox();
            this.rdoLoginUserOnly = new System.Windows.Forms.RadioButton();
            this.rdoAllUser = new System.Windows.Forms.RadioButton();
            this.gbxPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).BeginInit();
            this.gbxGridSetting.SuspendLayout();
            this.gbxMessage.SuspendLayout();
            this.gbxScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGridType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdGridColumnSetting)).BeginInit();
            this.gbxUpdateLoginUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxPreview
            // 
            this.gbxPreview.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxPreview.Controls.Add(this.grdPreview);
            this.gbxPreview.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxPreview.Location = new System.Drawing.Point(15, 477);
            this.gbxPreview.Name = "gbxPreview";
            this.gbxPreview.Padding = new System.Windows.Forms.Padding(0);
            this.gbxPreview.Size = new System.Drawing.Size(978, 129);
            this.gbxPreview.TabIndex = 0;
            this.gbxPreview.TabStop = false;
            this.gbxPreview.Text = "□　設定プレビュー";
            // 
            // grdPreview
            // 
            this.grdPreview.AllowAutoExtend = true;
            this.grdPreview.AllowUserToAddRows = false;
            this.grdPreview.AllowUserToResize = false;
            this.grdPreview.AllowUserToShiftSelect = true;
            this.grdPreview.Location = new System.Drawing.Point(24, 28);
            this.grdPreview.Margin = new System.Windows.Forms.Padding(12);
            this.grdPreview.MultiSelect = false;
            this.grdPreview.Name = "grdPreview";
            this.grdPreview.Size = new System.Drawing.Size(936, 82);
            this.grdPreview.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdPreview.TabIndex = 5;
            this.grdPreview.TabStop = false;
            this.grdPreview.Text = "vOneGridControl2";
            // 
            // gbxGridSetting
            // 
            this.gbxGridSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxGridSetting.Controls.Add(this.gbxMessage);
            this.gbxGridSetting.Controls.Add(this.gbxScreen);
            this.gbxGridSetting.Controls.Add(this.grdGridColumnSetting);
            this.gbxGridSetting.Controls.Add(this.gbxUpdateLoginUser);
            this.gbxGridSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxGridSetting.Location = new System.Drawing.Point(15, 15);
            this.gbxGridSetting.Name = "gbxGridSetting";
            this.gbxGridSetting.Padding = new System.Windows.Forms.Padding(0);
            this.gbxGridSetting.Size = new System.Drawing.Size(978, 456);
            this.gbxGridSetting.TabIndex = 0;
            this.gbxGridSetting.TabStop = false;
            // 
            // gbxMessage
            // 
            this.gbxMessage.Controls.Add(this.lblExplain);
            this.gbxMessage.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxMessage.Location = new System.Drawing.Point(24, 228);
            this.gbxMessage.Margin = new System.Windows.Forms.Padding(24, 24, 12, 12);
            this.gbxMessage.Name = "gbxMessage";
            this.gbxMessage.Size = new System.Drawing.Size(441, 213);
            this.gbxMessage.TabIndex = 0;
            this.gbxMessage.TabStop = false;
            // 
            // lblExplain
            // 
            this.lblExplain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblExplain.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblExplain.Location = new System.Drawing.Point(3, 44);
            this.lblExplain.Name = "lblExplain";
            this.lblExplain.Padding = new System.Windows.Forms.Padding(3);
            this.lblExplain.Size = new System.Drawing.Size(435, 138);
            this.lblExplain.TabIndex = 0;
            // 
            // gbxScreen
            // 
            this.gbxScreen.Controls.Add(this.cmbGridType);
            this.gbxScreen.Controls.Add(this.lblScreen);
            this.gbxScreen.Location = new System.Drawing.Point(24, 28);
            this.gbxScreen.Margin = new System.Windows.Forms.Padding(24, 12, 12, 12);
            this.gbxScreen.Name = "gbxScreen";
            this.gbxScreen.Size = new System.Drawing.Size(441, 70);
            this.gbxScreen.TabIndex = 0;
            this.gbxScreen.TabStop = false;
            // 
            // cmbGridType
            // 
            this.cmbGridType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbGridType.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbGridType.DisplayMember = null;
            this.cmbGridType.DropDown.AllowResize = false;
            this.cmbGridType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGridType.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbGridType.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGridType.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbGridType.ListHeaderPane.Height = 22;
            this.cmbGridType.ListHeaderPane.Visible = false;
            this.cmbGridType.Location = new System.Drawing.Point(81, 25);
            this.cmbGridType.Margin = new System.Windows.Forms.Padding(6);
            this.cmbGridType.Name = "cmbGridType";
            this.cmbGridType.Required = false;
            this.cmbGridType.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbGridType.Size = new System.Drawing.Size(305, 24);
            this.cmbGridType.TabIndex = 1;
            this.cmbGridType.ValueMember = null;
            this.cmbGridType.SelectedIndexChanged += new System.EventHandler(this.cmbGridType_SelectedIndexChanged);
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // lblScreen
            // 
            this.lblScreen.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreen.Location = new System.Drawing.Point(41, 28);
            this.lblScreen.Name = "lblScreen";
            this.lblScreen.Size = new System.Drawing.Size(31, 16);
            this.lblScreen.TabIndex = 0;
            this.lblScreen.Text = "画面";
            this.lblScreen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdGridColumnSetting
            // 
            this.grdGridColumnSetting.AllowAutoExtend = true;
            this.grdGridColumnSetting.AllowDrop = true;
            this.grdGridColumnSetting.AllowUserToAddRows = false;
            this.grdGridColumnSetting.AllowUserToResize = false;
            this.grdGridColumnSetting.AllowUserToShiftSelect = true;
            this.grdGridColumnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdGridColumnSetting.Location = new System.Drawing.Point(553, 28);
            this.grdGridColumnSetting.Margin = new System.Windows.Forms.Padding(3, 12, 18, 18);
            this.grdGridColumnSetting.MultiSelect = false;
            this.grdGridColumnSetting.Name = "grdGridColumnSetting";
            this.grdGridColumnSetting.Size = new System.Drawing.Size(407, 413);
            this.grdGridColumnSetting.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdGridColumnSetting.TabIndex = 4;
            this.grdGridColumnSetting.TabStop = false;
            this.grdGridColumnSetting.Text = "vOneGridControl1";
            this.grdGridColumnSetting.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdSetting_CellValueChanged);
            this.grdGridColumnSetting.CellEndEdit += new System.EventHandler<GrapeCity.Win.MultiRow.CellEndEditEventArgs>(this.grdSetting_CellEndEdit);
            this.grdGridColumnSetting.SectionPainting += new System.EventHandler<GrapeCity.Win.MultiRow.SectionPaintingEventArgs>(this.grdSetting_SectionPainting);
            this.grdGridColumnSetting.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdSetting_DragDrop);
            this.grdGridColumnSetting.DragOver += new System.Windows.Forms.DragEventHandler(this.grdSetting_DragOver);
            this.grdGridColumnSetting.DragLeave += new System.EventHandler(this.grdSetting_DragLeave);
            this.grdGridColumnSetting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdSetting_MouseDown);
            // 
            // gbxUpdateLoginUser
            // 
            this.gbxUpdateLoginUser.Controls.Add(this.rdoLoginUserOnly);
            this.gbxUpdateLoginUser.Controls.Add(this.rdoAllUser);
            this.gbxUpdateLoginUser.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxUpdateLoginUser.Location = new System.Drawing.Point(24, 136);
            this.gbxUpdateLoginUser.Margin = new System.Windows.Forms.Padding(24, 24, 6, 6);
            this.gbxUpdateLoginUser.Name = "gbxUpdateLoginUser";
            this.gbxUpdateLoginUser.Size = new System.Drawing.Size(441, 62);
            this.gbxUpdateLoginUser.TabIndex = 0;
            this.gbxUpdateLoginUser.TabStop = false;
            this.gbxUpdateLoginUser.Text = "更新対象";
            // 
            // rdoLoginUserOnly
            // 
            this.rdoLoginUserOnly.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoLoginUserOnly.Checked = true;
            this.rdoLoginUserOnly.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoLoginUserOnly.Location = new System.Drawing.Point(81, 25);
            this.rdoLoginUserOnly.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.rdoLoginUserOnly.Name = "rdoLoginUserOnly";
            this.rdoLoginUserOnly.Size = new System.Drawing.Size(98, 18);
            this.rdoLoginUserOnly.TabIndex = 2;
            this.rdoLoginUserOnly.TabStop = true;
            this.rdoLoginUserOnly.Text = "ログイン担当者";
            this.rdoLoginUserOnly.UseVisualStyleBackColor = true;
            // 
            // rdoAllUser
            // 
            this.rdoAllUser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoAllUser.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoAllUser.Location = new System.Drawing.Point(288, 25);
            this.rdoAllUser.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.rdoAllUser.Name = "rdoAllUser";
            this.rdoAllUser.Size = new System.Drawing.Size(98, 18);
            this.rdoAllUser.TabIndex = 3;
            this.rdoAllUser.Text = "全担当者";
            this.rdoAllUser.UseVisualStyleBackColor = true;
            // 
            // PH0701
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxGridSetting);
            this.Controls.Add(this.gbxPreview);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PH0701";
            this.Load += new System.EventHandler(this.PH0701_Load);
            this.gbxPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).EndInit();
            this.gbxGridSetting.ResumeLayout(false);
            this.gbxMessage.ResumeLayout(false);
            this.gbxScreen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbGridType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdGridColumnSetting)).EndInit();
            this.gbxUpdateLoginUser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxPreview;
        private Common.Controls.VOneGridControl grdPreview;
        private System.Windows.Forms.GroupBox gbxGridSetting;
        private System.Windows.Forms.GroupBox gbxScreen;
        private Common.Controls.VOneComboControl cmbGridType;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneLabelControl lblScreen;
        private System.Windows.Forms.GroupBox gbxUpdateLoginUser;
        private System.Windows.Forms.RadioButton rdoLoginUserOnly;
        private System.Windows.Forms.RadioButton rdoAllUser;
        private System.Windows.Forms.GroupBox gbxMessage;
        private Common.Controls.VOneLabelControl lblExplain;
        private Common.Controls.VOneGridControl grdGridColumnSetting;
    }
}