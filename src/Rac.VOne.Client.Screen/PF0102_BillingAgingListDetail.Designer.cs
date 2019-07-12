namespace Rac.VOne.Client.Screen
{
    partial class PF0102
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
            this.gbxHeader = new System.Windows.Forms.GroupBox();
            this.lblStaffNameTo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblDepartmentCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDepartmentCodeFrom = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblStaffCodeTo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblDepartmentNameFrom = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblStaffNameFrom = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblStaffCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblStaffCodeFrom = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblDepartmentFromTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDepartmentNameTo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblStaffFromTo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDepartmentCodeTo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.grdBillingAgingDetail = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffNameTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCodeFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCodeTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentNameFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffNameFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCodeFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentNameTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCodeTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBillingAgingDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxHeader
            // 
            this.gbxHeader.Controls.Add(this.lblStaffNameTo);
            this.gbxHeader.Controls.Add(this.lblDepartmentCode);
            this.gbxHeader.Controls.Add(this.lblDepartmentCodeFrom);
            this.gbxHeader.Controls.Add(this.lblStaffCodeTo);
            this.gbxHeader.Controls.Add(this.lblDepartmentNameFrom);
            this.gbxHeader.Controls.Add(this.lblStaffNameFrom);
            this.gbxHeader.Controls.Add(this.lblStaffCode);
            this.gbxHeader.Controls.Add(this.lblStaffCodeFrom);
            this.gbxHeader.Controls.Add(this.lblDepartmentFromTo);
            this.gbxHeader.Controls.Add(this.lblDepartmentNameTo);
            this.gbxHeader.Controls.Add(this.lblStaffFromTo);
            this.gbxHeader.Controls.Add(this.lblDepartmentCodeTo);
            this.gbxHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxHeader.Location = new System.Drawing.Point(12, 12);
            this.gbxHeader.Name = "gbxHeader";
            this.gbxHeader.Size = new System.Drawing.Size(984, 127);
            this.gbxHeader.TabIndex = 0;
            this.gbxHeader.TabStop = false;
            // 
            // lblStaffNameTo
            // 
            this.lblStaffNameTo.DropDown.AllowDrop = false;
            this.lblStaffNameTo.Enabled = false;
            this.lblStaffNameTo.HighlightText = true;
            this.lblStaffNameTo.Location = new System.Drawing.Point(253, 96);
            this.lblStaffNameTo.Name = "lblStaffNameTo";
            this.lblStaffNameTo.ReadOnly = true;
            this.lblStaffNameTo.Required = false;
            this.lblStaffNameTo.Size = new System.Drawing.Size(630, 20);
            this.lblStaffNameTo.TabIndex = 0;
            // 
            // lblDepartmentCode
            // 
            this.lblDepartmentCode.AutoSize = true;
            this.lblDepartmentCode.Location = new System.Drawing.Point(14, 22);
            this.lblDepartmentCode.Name = "lblDepartmentCode";
            this.lblDepartmentCode.Size = new System.Drawing.Size(82, 15);
            this.lblDepartmentCode.TabIndex = 0;
            this.lblDepartmentCode.Text = "請求部門コード";
            // 
            // lblDepartmentCodeFrom
            // 
            this.lblDepartmentCodeFrom.DropDown.AllowDrop = false;
            this.lblDepartmentCodeFrom.Enabled = false;
            this.lblDepartmentCodeFrom.HighlightText = true;
            this.lblDepartmentCodeFrom.Location = new System.Drawing.Point(127, 18);
            this.lblDepartmentCodeFrom.Name = "lblDepartmentCodeFrom";
            this.lblDepartmentCodeFrom.ReadOnly = true;
            this.lblDepartmentCodeFrom.Required = false;
            this.lblDepartmentCodeFrom.Size = new System.Drawing.Size(115, 20);
            this.lblDepartmentCodeFrom.TabIndex = 0;
            // 
            // lblStaffCodeTo
            // 
            this.lblStaffCodeTo.DropDown.AllowDrop = false;
            this.lblStaffCodeTo.Enabled = false;
            this.lblStaffCodeTo.HighlightText = true;
            this.lblStaffCodeTo.Location = new System.Drawing.Point(127, 96);
            this.lblStaffCodeTo.Name = "lblStaffCodeTo";
            this.lblStaffCodeTo.ReadOnly = true;
            this.lblStaffCodeTo.Required = false;
            this.lblStaffCodeTo.Size = new System.Drawing.Size(115, 20);
            this.lblStaffCodeTo.TabIndex = 0;
            // 
            // lblDepartmentNameFrom
            // 
            this.lblDepartmentNameFrom.DropDown.AllowDrop = false;
            this.lblDepartmentNameFrom.Enabled = false;
            this.lblDepartmentNameFrom.HighlightText = true;
            this.lblDepartmentNameFrom.Location = new System.Drawing.Point(253, 18);
            this.lblDepartmentNameFrom.Name = "lblDepartmentNameFrom";
            this.lblDepartmentNameFrom.ReadOnly = true;
            this.lblDepartmentNameFrom.Required = false;
            this.lblDepartmentNameFrom.Size = new System.Drawing.Size(630, 20);
            this.lblDepartmentNameFrom.TabIndex = 0;
            // 
            // lblStaffNameFrom
            // 
            this.lblStaffNameFrom.DropDown.AllowDrop = false;
            this.lblStaffNameFrom.Enabled = false;
            this.lblStaffNameFrom.HighlightText = true;
            this.lblStaffNameFrom.Location = new System.Drawing.Point(253, 70);
            this.lblStaffNameFrom.Name = "lblStaffNameFrom";
            this.lblStaffNameFrom.ReadOnly = true;
            this.lblStaffNameFrom.Required = false;
            this.lblStaffNameFrom.Size = new System.Drawing.Size(630, 20);
            this.lblStaffNameFrom.TabIndex = 0;
            // 
            // lblStaffCode
            // 
            this.lblStaffCode.AutoSize = true;
            this.lblStaffCode.Location = new System.Drawing.Point(14, 74);
            this.lblStaffCode.Name = "lblStaffCode";
            this.lblStaffCode.Size = new System.Drawing.Size(70, 15);
            this.lblStaffCode.TabIndex = 0;
            this.lblStaffCode.Text = "担当者コード";
            // 
            // lblStaffCodeFrom
            // 
            this.lblStaffCodeFrom.DropDown.AllowDrop = false;
            this.lblStaffCodeFrom.Enabled = false;
            this.lblStaffCodeFrom.HighlightText = true;
            this.lblStaffCodeFrom.Location = new System.Drawing.Point(127, 70);
            this.lblStaffCodeFrom.Name = "lblStaffCodeFrom";
            this.lblStaffCodeFrom.ReadOnly = true;
            this.lblStaffCodeFrom.Required = false;
            this.lblStaffCodeFrom.Size = new System.Drawing.Size(115, 20);
            this.lblStaffCodeFrom.TabIndex = 0;
            // 
            // lblDepartmentFromTo
            // 
            this.lblDepartmentFromTo.AutoSize = true;
            this.lblDepartmentFromTo.Location = new System.Drawing.Point(101, 48);
            this.lblDepartmentFromTo.Name = "lblDepartmentFromTo";
            this.lblDepartmentFromTo.Size = new System.Drawing.Size(19, 15);
            this.lblDepartmentFromTo.TabIndex = 0;
            this.lblDepartmentFromTo.Text = "～";
            // 
            // lblDepartmentNameTo
            // 
            this.lblDepartmentNameTo.DropDown.AllowDrop = false;
            this.lblDepartmentNameTo.Enabled = false;
            this.lblDepartmentNameTo.HighlightText = true;
            this.lblDepartmentNameTo.Location = new System.Drawing.Point(253, 44);
            this.lblDepartmentNameTo.Name = "lblDepartmentNameTo";
            this.lblDepartmentNameTo.ReadOnly = true;
            this.lblDepartmentNameTo.Required = false;
            this.lblDepartmentNameTo.Size = new System.Drawing.Size(630, 20);
            this.lblDepartmentNameTo.TabIndex = 0;
            // 
            // lblStaffFromTo
            // 
            this.lblStaffFromTo.AutoSize = true;
            this.lblStaffFromTo.Location = new System.Drawing.Point(101, 101);
            this.lblStaffFromTo.Name = "lblStaffFromTo";
            this.lblStaffFromTo.Size = new System.Drawing.Size(19, 15);
            this.lblStaffFromTo.TabIndex = 0;
            this.lblStaffFromTo.Text = "～";
            // 
            // lblDepartmentCodeTo
            // 
            this.lblDepartmentCodeTo.DropDown.AllowDrop = false;
            this.lblDepartmentCodeTo.Enabled = false;
            this.lblDepartmentCodeTo.HighlightText = true;
            this.lblDepartmentCodeTo.Location = new System.Drawing.Point(127, 44);
            this.lblDepartmentCodeTo.Name = "lblDepartmentCodeTo";
            this.lblDepartmentCodeTo.ReadOnly = true;
            this.lblDepartmentCodeTo.Required = false;
            this.lblDepartmentCodeTo.Size = new System.Drawing.Size(115, 20);
            this.lblDepartmentCodeTo.TabIndex = 0;
            // 
            // grdBillingAgingDetail
            // 
            this.grdBillingAgingDetail.AllowAutoExtend = true;
            this.grdBillingAgingDetail.AllowUserToAddRows = false;
            this.grdBillingAgingDetail.AllowUserToShiftSelect = true;
            this.grdBillingAgingDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBillingAgingDetail.Location = new System.Drawing.Point(12, 143);
            this.grdBillingAgingDetail.Name = "grdBillingAgingDetail";
            this.grdBillingAgingDetail.Size = new System.Drawing.Size(984, 466);
            this.grdBillingAgingDetail.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdBillingAgingDetail.TabIndex = 0;
            // 
            // PF0102
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grdBillingAgingDetail);
            this.Controls.Add(this.gbxHeader);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PF0102";
            this.Load += new System.EventHandler(this.PF0102_Load);
            this.gbxHeader.ResumeLayout(false);
            this.gbxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffNameTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCodeFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCodeTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentNameFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffNameFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStaffCodeFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentNameTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDepartmentCodeTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBillingAgingDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxHeader;
        private Common.Controls.VOneLabelControl lblDepartmentCode;
        private Common.Controls.VOneDispLabelControl lblStaffNameTo;
        private Common.Controls.VOneDispLabelControl lblDepartmentCodeFrom;
        private Common.Controls.VOneDispLabelControl lblDepartmentNameFrom;
        private Common.Controls.VOneLabelControl lblStaffCode;
        private Common.Controls.VOneLabelControl lblDepartmentFromTo;
        private Common.Controls.VOneLabelControl lblStaffFromTo;
        private Common.Controls.VOneDispLabelControl lblDepartmentCodeTo;
        private Common.Controls.VOneDispLabelControl lblDepartmentNameTo;
        private Common.Controls.VOneDispLabelControl lblStaffCodeFrom;
        private Common.Controls.VOneDispLabelControl lblStaffNameFrom;
        private Common.Controls.VOneDispLabelControl lblStaffCodeTo;
        private Common.Controls.VOneGridControl grdBillingAgingDetail;
    }
}
