namespace Rac.VOne.Client.Screen
{
    partial class PH0502
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
            this.grpSearchConditions = new System.Windows.Forms.GroupBox();
            this.lblWaveDash2 = new System.Windows.Forms.Label();
            this.lblWaveDash1 = new System.Windows.Forms.Label();
            this.txtEndAt_From = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.txtEndAt_To = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblImportType = new System.Windows.Forms.Label();
            this.txtStartAt_From = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.txtStartAt_To = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.cmbResult = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton3 = new GrapeCity.Win.Editors.DropDownButton();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblStartAt = new System.Windows.Forms.Label();
            this.lblImportSubType = new System.Windows.Forms.Label();
            this.lblEndAt = new System.Windows.Forms.Label();
            this.cmbImportSubType = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton2 = new GrapeCity.Win.Editors.DropDownButton();
            this.cmbImportType = new Rac.VOne.Client.Common.Controls.VOneComboControl(this.components);
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.grdTaskScheduleHistory = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.grpSearchConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndAt_From)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndAt_To)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartAt_From)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartAt_To)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbImportSubType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbImportType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTaskScheduleHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSearchConditions
            // 
            this.grpSearchConditions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSearchConditions.Controls.Add(this.lblWaveDash2);
            this.grpSearchConditions.Controls.Add(this.lblWaveDash1);
            this.grpSearchConditions.Controls.Add(this.txtEndAt_From);
            this.grpSearchConditions.Controls.Add(this.txtEndAt_To);
            this.grpSearchConditions.Controls.Add(this.lblImportType);
            this.grpSearchConditions.Controls.Add(this.txtStartAt_From);
            this.grpSearchConditions.Controls.Add(this.txtStartAt_To);
            this.grpSearchConditions.Controls.Add(this.cmbResult);
            this.grpSearchConditions.Controls.Add(this.lblResult);
            this.grpSearchConditions.Controls.Add(this.lblStartAt);
            this.grpSearchConditions.Controls.Add(this.lblImportSubType);
            this.grpSearchConditions.Controls.Add(this.lblEndAt);
            this.grpSearchConditions.Controls.Add(this.cmbImportSubType);
            this.grpSearchConditions.Controls.Add(this.cmbImportType);
            this.grpSearchConditions.Location = new System.Drawing.Point(15, 15);
            this.grpSearchConditions.Name = "grpSearchConditions";
            this.grpSearchConditions.Size = new System.Drawing.Size(978, 122);
            this.grpSearchConditions.TabIndex = 1;
            this.grpSearchConditions.TabStop = false;
            // 
            // lblWaveDash2
            // 
            this.lblWaveDash2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblWaveDash2.AutoSize = true;
            this.lblWaveDash2.Location = new System.Drawing.Point(270, 90);
            this.lblWaveDash2.Name = "lblWaveDash2";
            this.lblWaveDash2.Size = new System.Drawing.Size(19, 15);
            this.lblWaveDash2.TabIndex = 4;
            this.lblWaveDash2.Text = "～";
            this.lblWaveDash2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWaveDash1
            // 
            this.lblWaveDash1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblWaveDash1.AutoSize = true;
            this.lblWaveDash1.Location = new System.Drawing.Point(270, 57);
            this.lblWaveDash1.Name = "lblWaveDash1";
            this.lblWaveDash1.Size = new System.Drawing.Size(19, 15);
            this.lblWaveDash1.TabIndex = 3;
            this.lblWaveDash1.Text = "～";
            this.lblWaveDash1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEndAt_From
            // 
            this.txtEndAt_From.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtEndAt_From.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtEndAt_From.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.txtEndAt_From.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtEndAt_From.Location = new System.Drawing.Point(89, 87);
            this.txtEndAt_From.Name = "txtEndAt_From";
            this.txtEndAt_From.Required = false;
            this.txtEndAt_From.Size = new System.Drawing.Size(170, 22);
            this.txtEndAt_From.Spin.AllowSpin = false;
            this.txtEndAt_From.TabIndex = 5;
            this.txtEndAt_From.Value = new System.DateTime(2017, 3, 24, 0, 0, 0, 0);
            // 
            // txtEndAt_To
            // 
            this.txtEndAt_To.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtEndAt_To.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtEndAt_To.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.txtEndAt_To.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtEndAt_To.Location = new System.Drawing.Point(300, 87);
            this.txtEndAt_To.Name = "txtEndAt_To";
            this.txtEndAt_To.Required = false;
            this.txtEndAt_To.Size = new System.Drawing.Size(171, 22);
            this.txtEndAt_To.Spin.AllowSpin = false;
            this.txtEndAt_To.TabIndex = 6;
            this.txtEndAt_To.Value = new System.DateTime(2017, 3, 24, 0, 0, 0, 0);
            // 
            // lblImportType
            // 
            this.lblImportType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblImportType.AutoSize = true;
            this.lblImportType.Location = new System.Drawing.Point(9, 24);
            this.lblImportType.Name = "lblImportType";
            this.lblImportType.Size = new System.Drawing.Size(31, 15);
            this.lblImportType.TabIndex = 0;
            this.lblImportType.Text = "種別";
            // 
            // txtStartAt_From
            // 
            this.txtStartAt_From.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtStartAt_From.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtStartAt_From.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.txtStartAt_From.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtStartAt_From.Location = new System.Drawing.Point(89, 54);
            this.txtStartAt_From.Name = "txtStartAt_From";
            this.txtStartAt_From.Required = false;
            this.txtStartAt_From.Size = new System.Drawing.Size(170, 22);
            this.txtStartAt_From.Spin.AllowSpin = false;
            this.txtStartAt_From.TabIndex = 3;
            this.txtStartAt_From.Value = new System.DateTime(2017, 3, 24, 0, 0, 0, 0);
            // 
            // txtStartAt_To
            // 
            this.txtStartAt_To.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtStartAt_To.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtStartAt_To.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.txtStartAt_To.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtStartAt_To.Location = new System.Drawing.Point(300, 54);
            this.txtStartAt_To.Name = "txtStartAt_To";
            this.txtStartAt_To.Required = false;
            this.txtStartAt_To.Size = new System.Drawing.Size(171, 22);
            this.txtStartAt_To.Spin.AllowSpin = false;
            this.txtStartAt_To.TabIndex = 4;
            this.txtStartAt_To.Value = new System.DateTime(2017, 3, 24, 0, 0, 0, 0);
            // 
            // cmbResult
            // 
            this.cmbResult.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbResult.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbResult.DisplayMember = null;
            this.cmbResult.DropDown.AllowResize = false;
            this.cmbResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResult.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbResult.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbResult.ListHeaderPane.Height = 22;
            this.cmbResult.ListHeaderPane.Visible = false;
            this.cmbResult.Location = new System.Drawing.Point(587, 54);
            this.cmbResult.Name = "cmbResult";
            this.cmbResult.Required = false;
            this.cmbResult.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton3});
            this.cmbResult.Size = new System.Drawing.Size(120, 22);
            this.cmbResult.TabIndex = 7;
            this.cmbResult.ValueMember = null;
            // 
            // dropDownButton3
            // 
            this.dropDownButton3.Name = "dropDownButton3";
            // 
            // lblResult
            // 
            this.lblResult.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(507, 56);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(55, 15);
            this.lblResult.TabIndex = 6;
            this.lblResult.Text = "実行結果";
            // 
            // lblStartAt
            // 
            this.lblStartAt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblStartAt.AutoSize = true;
            this.lblStartAt.Location = new System.Drawing.Point(9, 56);
            this.lblStartAt.Name = "lblStartAt";
            this.lblStartAt.Size = new System.Drawing.Size(55, 15);
            this.lblStartAt.TabIndex = 1;
            this.lblStartAt.Text = "開始日時";
            // 
            // lblImportSubType
            // 
            this.lblImportSubType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblImportSubType.AutoSize = true;
            this.lblImportSubType.Location = new System.Drawing.Point(507, 24);
            this.lblImportSubType.Name = "lblImportSubType";
            this.lblImportSubType.Size = new System.Drawing.Size(68, 15);
            this.lblImportSubType.TabIndex = 5;
            this.lblImportSubType.Text = "取込パターン";
            // 
            // lblEndAt
            // 
            this.lblEndAt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblEndAt.AutoSize = true;
            this.lblEndAt.Location = new System.Drawing.Point(9, 89);
            this.lblEndAt.Name = "lblEndAt";
            this.lblEndAt.Size = new System.Drawing.Size(55, 15);
            this.lblEndAt.TabIndex = 2;
            this.lblEndAt.Text = "終了日時";
            // 
            // cmbImportSubType
            // 
            this.cmbImportSubType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbImportSubType.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbImportSubType.DisplayMember = null;
            this.cmbImportSubType.DropDown.AllowResize = false;
            this.cmbImportSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImportSubType.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbImportSubType.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbImportSubType.ListHeaderPane.Height = 22;
            this.cmbImportSubType.ListHeaderPane.Visible = false;
            this.cmbImportSubType.Location = new System.Drawing.Point(587, 22);
            this.cmbImportSubType.Name = "cmbImportSubType";
            this.cmbImportSubType.Required = false;
            this.cmbImportSubType.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton2});
            this.cmbImportSubType.Size = new System.Drawing.Size(382, 22);
            this.cmbImportSubType.TabIndex = 2;
            this.cmbImportSubType.ValueMember = null;
            // 
            // dropDownButton2
            // 
            this.dropDownButton2.Name = "dropDownButton2";
            // 
            // cmbImportType
            // 
            this.cmbImportType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbImportType.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbImportType.DisplayMember = null;
            this.cmbImportType.DropDown.AllowResize = false;
            this.cmbImportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImportType.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Flat;
            this.cmbImportType.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cmbImportType.ListHeaderPane.Height = 22;
            this.cmbImportType.ListHeaderPane.Visible = false;
            this.cmbImportType.Location = new System.Drawing.Point(89, 22);
            this.cmbImportType.Name = "cmbImportType";
            this.cmbImportType.Required = false;
            this.cmbImportType.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton1});
            this.cmbImportType.Size = new System.Drawing.Size(382, 22);
            this.cmbImportType.TabIndex = 1;
            this.cmbImportType.ValueMember = null;
            this.cmbImportType.SelectedValueChanged += new System.EventHandler(this.cmbImportType_SelectedValueChanged);
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // grdTaskScheduleHistory
            // 
            this.grdTaskScheduleHistory.AllowAutoExtend = true;
            this.grdTaskScheduleHistory.AllowUserToAddRows = false;
            this.grdTaskScheduleHistory.AllowUserToShiftSelect = true;
            this.grdTaskScheduleHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTaskScheduleHistory.Location = new System.Drawing.Point(15, 143);
            this.grdTaskScheduleHistory.Name = "grdTaskScheduleHistory";
            this.grdTaskScheduleHistory.Size = new System.Drawing.Size(978, 463);
            this.grdTaskScheduleHistory.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdTaskScheduleHistory.TabIndex = 2;
            this.grdTaskScheduleHistory.Text = "vOneGridControl1";
            this.grdTaskScheduleHistory.DataSourceChanged += new System.EventHandler(this.grdTaskScheduleHistory_DataSourceChanged);
            this.grdTaskScheduleHistory.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdTaskScheduleHistory_CellDoubleClick);
            // 
            // PH0502
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grdTaskScheduleHistory);
            this.Controls.Add(this.grpSearchConditions);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PH0502";
            this.Load += new System.EventHandler(this.PH0502_Load);
            this.grpSearchConditions.ResumeLayout(false);
            this.grpSearchConditions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndAt_From)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndAt_To)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartAt_From)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartAt_To)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbImportSubType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbImportType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTaskScheduleHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSearchConditions;
        private Common.Controls.VOneGridControl grdTaskScheduleHistory;
        private System.Windows.Forms.Label lblImportType;
        private System.Windows.Forms.Label lblStartAt;
        private System.Windows.Forms.Label lblWaveDash1;
        private System.Windows.Forms.Label lblWaveDash2;
        private System.Windows.Forms.Label lblImportSubType;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblEndAt;
        private Common.Controls.VOneComboControl cmbImportType;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private Common.Controls.VOneComboControl cmbImportSubType;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton2;
        private Common.Controls.VOneComboControl cmbResult;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton3;
        private Common.Controls.VOneDateControl txtEndAt_From;
        private Common.Controls.VOneDateControl txtEndAt_To;
        private Common.Controls.VOneDateControl txtStartAt_From;
        private Common.Controls.VOneDateControl txtStartAt_To;
    }
}