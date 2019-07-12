namespace Rac.VOne.Client.Screen
{
    partial class PB2301
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
            this.gbxStatusList = new System.Windows.Forms.GroupBox();
            this.grdStatus = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxStatusInput = new System.Windows.Forms.GroupBox();
            this.nmbDisplayOrder = new Rac.VOne.Client.Common.Controls.VOneNumberControl(this.components);
            this.txtStatusCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblStatusName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblDisplayOrder = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtStatusName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblStatusCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxStatusList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).BeginInit();
            this.gbxStatusInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmbDisplayOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStatusCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStatusName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxStatusList
            // 
            this.gbxStatusList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxStatusList.Controls.Add(this.grdStatus);
            this.gbxStatusList.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.gbxStatusList.Location = new System.Drawing.Point(184, 15);
            this.gbxStatusList.Name = "gbxStatusList";
            this.gbxStatusList.Size = new System.Drawing.Size(640, 471);
            this.gbxStatusList.TabIndex = 11;
            this.gbxStatusList.TabStop = false;
            this.gbxStatusList.Text = "□　登録済みのステータス";
            // 
            // grdStatus
            // 
            this.grdStatus.AllowAutoExtend = true;
            this.grdStatus.AllowUserToAddRows = false;
            this.grdStatus.AllowUserToShiftSelect = true;
            this.grdStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdStatus.CurrentCellBorderLine = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.Black);
            this.grdStatus.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdStatus.Location = new System.Drawing.Point(15, 22);
            this.grdStatus.Margin = new System.Windows.Forms.Padding(12, 3, 12, 6);
            this.grdStatus.Name = "grdStatus";
            this.grdStatus.Size = new System.Drawing.Size(610, 440);
            this.grdStatus.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdStatus.TabIndex = 0;
            this.grdStatus.TabStop = false;
            this.grdStatus.Text = "vOneGridControl1";
            this.grdStatus.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdStatus_CellDoubleClick);
            // 
            // gbxStatusInput
            // 
            this.gbxStatusInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxStatusInput.Controls.Add(this.nmbDisplayOrder);
            this.gbxStatusInput.Controls.Add(this.txtStatusCode);
            this.gbxStatusInput.Controls.Add(this.lblStatusName);
            this.gbxStatusInput.Controls.Add(this.lblDisplayOrder);
            this.gbxStatusInput.Controls.Add(this.txtStatusName);
            this.gbxStatusInput.Controls.Add(this.lblStatusCode);
            this.gbxStatusInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxStatusInput.Location = new System.Drawing.Point(184, 492);
            this.gbxStatusInput.Name = "gbxStatusInput";
            this.gbxStatusInput.Size = new System.Drawing.Size(640, 114);
            this.gbxStatusInput.TabIndex = 12;
            this.gbxStatusInput.TabStop = false;
            // 
            // nmbDisplayOrder
            // 
            this.nmbDisplayOrder.AllowDeleteToNull = true;
            this.nmbDisplayOrder.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.nmbDisplayOrder.DropDown.AllowDrop = false;
            this.nmbDisplayOrder.Fields.DecimalPart.MaxDigits = 0;
            this.nmbDisplayOrder.Fields.IntegerPart.MinDigits = 1;
            this.nmbDisplayOrder.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nmbDisplayOrder.HighlightText = true;
            this.nmbDisplayOrder.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nmbDisplayOrder.Location = new System.Drawing.Point(156, 73);
            this.nmbDisplayOrder.MaxMinBehavior = GrapeCity.Win.Editors.MaxMinBehavior.CancelInput;
            this.nmbDisplayOrder.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmbDisplayOrder.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmbDisplayOrder.Name = "nmbDisplayOrder";
            this.nmbDisplayOrder.Required = false;
            this.nmbDisplayOrder.Size = new System.Drawing.Size(60, 22);
            this.nmbDisplayOrder.Spin.AllowSpin = false;
            this.nmbDisplayOrder.TabIndex = 3;
            // 
            // txtStatusCode
            // 
            this.txtStatusCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtStatusCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtStatusCode.DropDown.AllowDrop = false;
            this.txtStatusCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtStatusCode.Format = "9";
            this.txtStatusCode.HighlightText = true;
            this.txtStatusCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtStatusCode.Location = new System.Drawing.Point(156, 17);
            this.txtStatusCode.MaxLength = 2;
            this.txtStatusCode.Name = "txtStatusCode";
            this.txtStatusCode.Required = true;
            this.txtStatusCode.Size = new System.Drawing.Size(30, 22);
            this.txtStatusCode.TabIndex = 1;
            this.txtStatusCode.Validated += new System.EventHandler(this.txtStatusCode_Validated);
            // 
            // lblStatusName
            // 
            this.lblStatusName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblStatusName.Location = new System.Drawing.Point(72, 47);
            this.lblStatusName.Name = "lblStatusName";
            this.lblStatusName.Size = new System.Drawing.Size(78, 16);
            this.lblStatusName.TabIndex = 0;
            this.lblStatusName.Text = "ステータス名称";
            this.lblStatusName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDisplayOrder
            // 
            this.lblDisplayOrder.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblDisplayOrder.Location = new System.Drawing.Point(72, 75);
            this.lblDisplayOrder.Name = "lblDisplayOrder";
            this.lblDisplayOrder.Size = new System.Drawing.Size(78, 16);
            this.lblDisplayOrder.TabIndex = 0;
            this.lblDisplayOrder.Text = "表示順番";
            this.lblDisplayOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtStatusName
            // 
            this.txtStatusName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtStatusName.DropDown.AllowDrop = false;
            this.txtStatusName.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.txtStatusName.HighlightText = true;
            this.txtStatusName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtStatusName.Location = new System.Drawing.Point(156, 45);
            this.txtStatusName.MaxLength = 40;
            this.txtStatusName.Name = "txtStatusName";
            this.txtStatusName.Required = true;
            this.txtStatusName.Size = new System.Drawing.Size(400, 22);
            this.txtStatusName.TabIndex = 2;
            // 
            // lblStatusCode
            // 
            this.lblStatusCode.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblStatusCode.Location = new System.Drawing.Point(72, 19);
            this.lblStatusCode.Name = "lblStatusCode";
            this.lblStatusCode.Size = new System.Drawing.Size(78, 16);
            this.lblStatusCode.TabIndex = 0;
            this.lblStatusCode.Text = "ステータスコード";
            this.lblStatusCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PB2301
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxStatusInput);
            this.Controls.Add(this.gbxStatusList);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PB2301";
            this.Load += new System.EventHandler(this.PB2301_Load);
            this.gbxStatusList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).EndInit();
            this.gbxStatusInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmbDisplayOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStatusCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStatusName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxStatusList;
        private Common.Controls.VOneGridControl grdStatus;
        private System.Windows.Forms.GroupBox gbxStatusInput;
        private Common.Controls.VOneTextControl txtStatusCode;
        private Common.Controls.VOneLabelControl lblStatusName;
        private Common.Controls.VOneLabelControl lblDisplayOrder;
        private Common.Controls.VOneTextControl txtStatusName;
        private Common.Controls.VOneLabelControl lblStatusCode;
        private Common.Controls.VOneNumberControl nmbDisplayOrder;
    }
}
