namespace Rac.VOne.Client.Screen
{
    partial class PB1001
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
            this.txtPayerName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.rdoPaymentAgency = new System.Windows.Forms.RadioButton();
            this.rdoCustomer = new System.Windows.Forms.RadioButton();
            this.txtCustomerCodeFrom = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtCustomerCodeTo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.cbxKana = new System.Windows.Forms.CheckBox();
            this.lblPayerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblWaveSign = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCustomerNameFrom = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.lblCustomerNameTo = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.btnCustomerCodeFromSearch = new System.Windows.Forms.Button();
            this.btnCustomerCodeToSearch = new System.Windows.Forms.Button();
            this.gbxOrderChange = new System.Windows.Forms.GroupBox();
            this.rdoPayerNameOrder = new System.Windows.Forms.RadioButton();
            this.rdoCustomerCodeOrder = new System.Windows.Forms.RadioButton();
            this.gbxKanaHistory = new System.Windows.Forms.GroupBox();
            this.grdKanaHistoryCustomer = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameTo)).BeginInit();
            this.gbxOrderChange.SuspendLayout();
            this.gbxKanaHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdKanaHistoryCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPayerName
            // 
            this.txtPayerName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPayerName.DropDown.AllowDrop = false;
            this.txtPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPayerName.Format = "@NA9";
            this.txtPayerName.HighlightText = true;
            this.txtPayerName.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtPayerName.Location = new System.Drawing.Point(172, 53);
            this.txtPayerName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 4);
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Required = false;
            this.txtPayerName.Size = new System.Drawing.Size(441, 22);
            this.txtPayerName.TabIndex = 4;
            // 
            // rdoPaymentAgency
            // 
            this.rdoPaymentAgency.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rdoPaymentAgency.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdoPaymentAgency.Location = new System.Drawing.Point(292, 23);
            this.rdoPaymentAgency.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.rdoPaymentAgency.Name = "rdoPaymentAgency";
            this.rdoPaymentAgency.Size = new System.Drawing.Size(98, 18);
            this.rdoPaymentAgency.TabIndex = 3;
            this.rdoPaymentAgency.TabStop = true;
            this.rdoPaymentAgency.Text = "決済代行会社";
            this.rdoPaymentAgency.UseVisualStyleBackColor = true;
            this.rdoPaymentAgency.Click += new System.EventHandler(this.rdoPaymentAgency_Click);
            // 
            // rdoCustomer
            // 
            this.rdoCustomer.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rdoCustomer.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdoCustomer.Location = new System.Drawing.Point(172, 23);
            this.rdoCustomer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.rdoCustomer.Name = "rdoCustomer";
            this.rdoCustomer.Size = new System.Drawing.Size(62, 18);
            this.rdoCustomer.TabIndex = 2;
            this.rdoCustomer.TabStop = true;
            this.rdoCustomer.Text = "得意先";
            this.rdoCustomer.UseVisualStyleBackColor = true;
            this.rdoCustomer.Click += new System.EventHandler(this.rdoCustomerCode_Click);
            // 
            // txtCustomerCodeFrom
            // 
            this.txtCustomerCodeFrom.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCodeFrom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCustomerCodeFrom.DropDown.AllowDrop = false;
            this.txtCustomerCodeFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCustomerCodeFrom.HighlightText = true;
            this.txtCustomerCodeFrom.Location = new System.Drawing.Point(172, 83);
            this.txtCustomerCodeFrom.Name = "txtCustomerCodeFrom";
            this.txtCustomerCodeFrom.Required = false;
            this.txtCustomerCodeFrom.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCodeFrom.TabIndex = 5;
            this.txtCustomerCodeFrom.Validated += new System.EventHandler(this.txtCustomerCodeFrom_Validated);
            // 
            // txtCustomerCodeTo
            // 
            this.txtCustomerCodeTo.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCodeTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCustomerCodeTo.DropDown.AllowDrop = false;
            this.txtCustomerCodeTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCustomerCodeTo.HighlightText = true;
            this.txtCustomerCodeTo.Location = new System.Drawing.Point(172, 114);
            this.txtCustomerCodeTo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.txtCustomerCodeTo.Name = "txtCustomerCodeTo";
            this.txtCustomerCodeTo.Required = false;
            this.txtCustomerCodeTo.Size = new System.Drawing.Size(115, 22);
            this.txtCustomerCodeTo.TabIndex = 7;
            this.txtCustomerCodeTo.Validated += new System.EventHandler(this.txtCustomerCodeTo_Validated);
            // 
            // cbxKana
            // 
            this.cbxKana.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbxKana.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cbxKana.Location = new System.Drawing.Point(150, 117);
            this.cbxKana.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.cbxKana.Name = "cbxKana";
            this.cbxKana.Size = new System.Drawing.Size(16, 18);
            this.cbxKana.TabIndex = 0;
            this.cbxKana.TabStop = false;
            this.cbxKana.UseVisualStyleBackColor = true;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPayerName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPayerName.Location = new System.Drawing.Point(87, 56);
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Size = new System.Drawing.Size(79, 16);
            this.lblPayerName.TabIndex = 0;
            this.lblPayerName.Text = "振込依頼人名";
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCustomerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCustomerCode.Location = new System.Drawing.Point(87, 86);
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Size = new System.Drawing.Size(79, 16);
            this.lblCustomerCode.TabIndex = 0;
            this.lblCustomerCode.Text = "得意先コード";
            // 
            // lblWaveSign
            // 
            this.lblWaveSign.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblWaveSign.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWaveSign.Location = new System.Drawing.Point(131, 117);
            this.lblWaveSign.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblWaveSign.Name = "lblWaveSign";
            this.lblWaveSign.Size = new System.Drawing.Size(18, 16);
            this.lblWaveSign.TabIndex = 0;
            this.lblWaveSign.Text = "～";
            this.lblWaveSign.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCustomerNameFrom
            // 
            this.lblCustomerNameFrom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCustomerNameFrom.DropDown.AllowDrop = false;
            this.lblCustomerNameFrom.Enabled = false;
            this.lblCustomerNameFrom.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCustomerNameFrom.HighlightText = true;
            this.lblCustomerNameFrom.Location = new System.Drawing.Point(323, 83);
            this.lblCustomerNameFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblCustomerNameFrom.Name = "lblCustomerNameFrom";
            this.lblCustomerNameFrom.ReadOnly = true;
            this.lblCustomerNameFrom.Required = false;
            this.lblCustomerNameFrom.Size = new System.Drawing.Size(290, 22);
            this.lblCustomerNameFrom.TabIndex = 0;
            // 
            // lblCustomerNameTo
            // 
            this.lblCustomerNameTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCustomerNameTo.DropDown.AllowDrop = false;
            this.lblCustomerNameTo.Enabled = false;
            this.lblCustomerNameTo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCustomerNameTo.HighlightText = true;
            this.lblCustomerNameTo.Location = new System.Drawing.Point(323, 113);
            this.lblCustomerNameTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 6);
            this.lblCustomerNameTo.Name = "lblCustomerNameTo";
            this.lblCustomerNameTo.ReadOnly = true;
            this.lblCustomerNameTo.Required = false;
            this.lblCustomerNameTo.Size = new System.Drawing.Size(290, 22);
            this.lblCustomerNameTo.TabIndex = 0;
            // 
            // btnCustomerCodeFromSearch
            // 
            this.btnCustomerCodeFromSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCustomerCodeFromSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomerCodeFromSearch.Location = new System.Drawing.Point(293, 82);
            this.btnCustomerCodeFromSearch.Name = "btnCustomerCodeFromSearch";
            this.btnCustomerCodeFromSearch.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerCodeFromSearch.TabIndex = 6;
            this.btnCustomerCodeFromSearch.UseVisualStyleBackColor = true;
            this.btnCustomerCodeFromSearch.Click += new System.EventHandler(this.btnCustomerCodeFromSearch_Click);
            // 
            // btnCustomerCodeToSearch
            // 
            this.btnCustomerCodeToSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCustomerCodeToSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnCustomerCodeToSearch.Location = new System.Drawing.Point(293, 112);
            this.btnCustomerCodeToSearch.Name = "btnCustomerCodeToSearch";
            this.btnCustomerCodeToSearch.Size = new System.Drawing.Size(24, 24);
            this.btnCustomerCodeToSearch.TabIndex = 8;
            this.btnCustomerCodeToSearch.UseVisualStyleBackColor = true;
            this.btnCustomerCodeToSearch.Click += new System.EventHandler(this.btnCustomerCodeToSearch_Click);
            // 
            // gbxOrderChange
            // 
            this.gbxOrderChange.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxOrderChange.Controls.Add(this.rdoPayerNameOrder);
            this.gbxOrderChange.Controls.Add(this.rdoCustomerCodeOrder);
            this.gbxOrderChange.Location = new System.Drawing.Point(740, 54);
            this.gbxOrderChange.Name = "gbxOrderChange";
            this.gbxOrderChange.Size = new System.Drawing.Size(237, 66);
            this.gbxOrderChange.TabIndex = 9;
            this.gbxOrderChange.TabStop = false;
            // 
            // rdoPayerNameOrder
            // 
            this.rdoPayerNameOrder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoPayerNameOrder.AutoSize = true;
            this.rdoPayerNameOrder.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdoPayerNameOrder.Location = new System.Drawing.Point(12, 14);
            this.rdoPayerNameOrder.Name = "rdoPayerNameOrder";
            this.rdoPayerNameOrder.Size = new System.Drawing.Size(138, 19);
            this.rdoPayerNameOrder.TabIndex = 10;
            this.rdoPayerNameOrder.Text = "振込依頼人名(カナ)順";
            this.rdoPayerNameOrder.UseVisualStyleBackColor = true;
            this.rdoPayerNameOrder.Click += new System.EventHandler(this.rdoPayerNameOrder_Click);
            // 
            // rdoCustomerCodeOrder
            // 
            this.rdoCustomerCodeOrder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoCustomerCodeOrder.AutoSize = true;
            this.rdoCustomerCodeOrder.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdoCustomerCodeOrder.Location = new System.Drawing.Point(12, 38);
            this.rdoCustomerCodeOrder.Name = "rdoCustomerCodeOrder";
            this.rdoCustomerCodeOrder.Size = new System.Drawing.Size(99, 19);
            this.rdoCustomerCodeOrder.TabIndex = 11;
            this.rdoCustomerCodeOrder.Text = "得意先コード順";
            this.rdoCustomerCodeOrder.UseVisualStyleBackColor = true;
            this.rdoCustomerCodeOrder.Click += new System.EventHandler(this.rdoCustomerCodeOrder_Click);
            // 
            // gbxKanaHistory
            // 
            this.gbxKanaHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxKanaHistory.Controls.Add(this.grdKanaHistoryCustomer);
            this.gbxKanaHistory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxKanaHistory.Location = new System.Drawing.Point(31, 144);
            this.gbxKanaHistory.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.gbxKanaHistory.Name = "gbxKanaHistory";
            this.gbxKanaHistory.Size = new System.Drawing.Size(946, 459);
            this.gbxKanaHistory.TabIndex = 1;
            this.gbxKanaHistory.TabStop = false;
            this.gbxKanaHistory.Text = "□　消込履歴";
            // 
            // grdKanaHistoryCustomer
            // 
            this.grdKanaHistoryCustomer.AllowAutoExtend = true;
            this.grdKanaHistoryCustomer.AllowUserToAddRows = false;
            this.grdKanaHistoryCustomer.AllowUserToShiftSelect = true;
            this.grdKanaHistoryCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdKanaHistoryCustomer.Location = new System.Drawing.Point(22, 22);
            this.grdKanaHistoryCustomer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grdKanaHistoryCustomer.Name = "grdKanaHistoryCustomer";
            this.grdKanaHistoryCustomer.Size = new System.Drawing.Size(900, 428);
            this.grdKanaHistoryCustomer.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdKanaHistoryCustomer.TabIndex = 13;
            this.grdKanaHistoryCustomer.TabStop = false;
            this.grdKanaHistoryCustomer.Text = "vOneGridControl1";
            // 
            // PB1001
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtPayerName);
            this.Controls.Add(this.rdoPaymentAgency);
            this.Controls.Add(this.gbxKanaHistory);
            this.Controls.Add(this.rdoCustomer);
            this.Controls.Add(this.txtCustomerCodeFrom);
            this.Controls.Add(this.gbxOrderChange);
            this.Controls.Add(this.txtCustomerCodeTo);
            this.Controls.Add(this.btnCustomerCodeToSearch);
            this.Controls.Add(this.cbxKana);
            this.Controls.Add(this.btnCustomerCodeFromSearch);
            this.Controls.Add(this.lblPayerName);
            this.Controls.Add(this.lblCustomerNameTo);
            this.Controls.Add(this.lblCustomerCode);
            this.Controls.Add(this.lblCustomerNameFrom);
            this.Controls.Add(this.lblWaveSign);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB1001";
            this.Load += new System.EventHandler(this.PB1001_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCodeTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCustomerNameTo)).EndInit();
            this.gbxOrderChange.ResumeLayout(false);
            this.gbxOrderChange.PerformLayout();
            this.gbxKanaHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdKanaHistoryCustomer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneTextControl txtPayerName;
        private System.Windows.Forms.RadioButton rdoPaymentAgency;
        private System.Windows.Forms.RadioButton rdoCustomer;
        private Common.Controls.VOneTextControl txtCustomerCodeFrom;
        private Common.Controls.VOneTextControl txtCustomerCodeTo;
        private System.Windows.Forms.CheckBox cbxKana;
        private Common.Controls.VOneLabelControl lblPayerName;
        private Common.Controls.VOneLabelControl lblCustomerCode;
        private Common.Controls.VOneLabelControl lblWaveSign;
        private Common.Controls.VOneDispLabelControl lblCustomerNameFrom;
        private Common.Controls.VOneDispLabelControl lblCustomerNameTo;
        private System.Windows.Forms.RadioButton rdoPayerNameOrder;
        private System.Windows.Forms.RadioButton rdoCustomerCodeOrder;
        private Common.Controls.VOneGridControl grdKanaHistoryCustomer;
        private System.Windows.Forms.Button btnCustomerCodeFromSearch;
        private System.Windows.Forms.Button btnCustomerCodeToSearch;
        private System.Windows.Forms.GroupBox gbxOrderChange;
        private System.Windows.Forms.GroupBox gbxKanaHistory;
    }
}
