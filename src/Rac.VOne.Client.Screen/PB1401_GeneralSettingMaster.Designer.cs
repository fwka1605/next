namespace Rac.VOne.Client.Screen
{
    partial class PB1401
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
            this.gbxGeneralSetting = new System.Windows.Forms.GroupBox();
            this.grdGeneralSetting = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxInsertData = new System.Windows.Forms.GroupBox();
            this.lblDescription = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtValue = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblLength = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtDescription = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblgeneralsettingId = new System.Windows.Forms.Label();
            this.txtLength = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblData = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCodeData = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxGeneralSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGeneralSetting)).BeginInit();
            this.gbxInsertData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLength)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxGeneralSetting
            // 
            this.gbxGeneralSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxGeneralSetting.Controls.Add(this.grdGeneralSetting);
            this.gbxGeneralSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxGeneralSetting.Location = new System.Drawing.Point(21, 21);
            this.gbxGeneralSetting.Name = "gbxGeneralSetting";
            this.gbxGeneralSetting.Size = new System.Drawing.Size(966, 507);
            this.gbxGeneralSetting.TabIndex = 1;
            this.gbxGeneralSetting.TabStop = false;
            this.gbxGeneralSetting.Text = "□ 登録済みの管理情報";
            // 
            // grdGeneralSetting
            // 
            this.grdGeneralSetting.AllowAutoExtend = true;
            this.grdGeneralSetting.AllowUserToAddRows = false;
            this.grdGeneralSetting.AllowUserToShiftSelect = true;
            this.grdGeneralSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdGeneralSetting.HideSelection = true;
            this.grdGeneralSetting.Location = new System.Drawing.Point(22, 22);
            this.grdGeneralSetting.Name = "grdGeneralSetting";
            this.grdGeneralSetting.Size = new System.Drawing.Size(924, 477);
            this.grdGeneralSetting.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdGeneralSetting.TabIndex = 0;
            this.grdGeneralSetting.TabStop = false;
            this.grdGeneralSetting.Text = "vOneGridControl1";
            this.grdGeneralSetting.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdGeneralSetting_CellDoubleClick);
            // 
            // gbxInsertData
            // 
            this.gbxInsertData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxInsertData.Controls.Add(this.lblDescription);
            this.gbxInsertData.Controls.Add(this.txtValue);
            this.gbxInsertData.Controls.Add(this.lblCode);
            this.gbxInsertData.Controls.Add(this.lblLength);
            this.gbxInsertData.Controls.Add(this.txtDescription);
            this.gbxInsertData.Controls.Add(this.lblgeneralsettingId);
            this.gbxInsertData.Controls.Add(this.txtLength);
            this.gbxInsertData.Controls.Add(this.lblData);
            this.gbxInsertData.Controls.Add(this.lblCodeData);
            this.gbxInsertData.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxInsertData.Location = new System.Drawing.Point(21, 534);
            this.gbxInsertData.Name = "gbxInsertData";
            this.gbxInsertData.Size = new System.Drawing.Size(966, 75);
            this.gbxInsertData.TabIndex = 0;
            this.gbxInsertData.TabStop = false;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDescription.Location = new System.Drawing.Point(179, 47);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(35, 16);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "説明";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtValue
            // 
            this.txtValue.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtValue.DropDown.AllowDrop = false;
            this.txtValue.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtValue.HighlightText = true;
            this.txtValue.Location = new System.Drawing.Point(220, 17);
            this.txtValue.Name = "txtValue";
            this.txtValue.Required = true;
            this.txtValue.Size = new System.Drawing.Size(340, 22);
            this.txtValue.TabIndex = 1;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCode.Location = new System.Drawing.Point(22, 48);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(150, 16);
            this.lblCode.TabIndex = 4;
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLength
            // 
            this.lblLength.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblLength.Location = new System.Drawing.Point(567, 19);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(55, 16);
            this.lblLength.TabIndex = 0;
            this.lblLength.Text = "有効桁数";
            this.lblLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            this.txtDescription.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDescription.DropDown.AllowDrop = false;
            this.txtDescription.Enabled = false;
            this.txtDescription.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtDescription.HighlightText = true;
            this.txtDescription.Location = new System.Drawing.Point(220, 45);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Required = false;
            this.txtDescription.Size = new System.Drawing.Size(340, 22);
            this.txtDescription.TabIndex = 3;
            // 
            // lblgeneralsettingId
            // 
            this.lblgeneralsettingId.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblgeneralsettingId.Location = new System.Drawing.Point(566, 51);
            this.lblgeneralsettingId.Name = "lblgeneralsettingId";
            this.lblgeneralsettingId.Size = new System.Drawing.Size(140, 16);
            this.lblgeneralsettingId.TabIndex = 4;
            this.lblgeneralsettingId.Text = "GeneralSettingId";
            this.lblgeneralsettingId.Visible = false;
            // 
            // txtLength
            // 
            this.txtLength.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtLength.DisabledForeColor = System.Drawing.SystemColors.WindowText;
            this.txtLength.DropDown.AllowDrop = false;
            this.txtLength.Enabled = false;
            this.txtLength.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtLength.HighlightText = true;
            this.txtLength.Location = new System.Drawing.Point(628, 17);
            this.txtLength.Name = "txtLength";
            this.txtLength.ReadOnlyBackColor = System.Drawing.Color.LightGray;
            this.txtLength.Required = false;
            this.txtLength.Size = new System.Drawing.Size(50, 22);
            this.txtLength.TabIndex = 2;
            // 
            // lblData
            // 
            this.lblData.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblData.Location = new System.Drawing.Point(179, 19);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(35, 16);
            this.lblData.TabIndex = 0;
            this.lblData.Text = "データ";
            this.lblData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCodeData
            // 
            this.lblCodeData.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCodeData.Location = new System.Drawing.Point(22, 21);
            this.lblCodeData.Name = "lblCodeData";
            this.lblCodeData.Size = new System.Drawing.Size(150, 16);
            this.lblCodeData.TabIndex = 0;
            this.lblCodeData.Text = "管理コード";
            this.lblCodeData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PB1401
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxInsertData);
            this.Controls.Add(this.gbxGeneralSetting);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB1401";
            this.Load += new System.EventHandler(this.PB1401_Load);
            this.gbxGeneralSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdGeneralSetting)).EndInit();
            this.gbxInsertData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLength)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxInsertData;
       
        private Common.Controls.VOneLabelControl lblDescription;
        private Common.Controls.VOneLabelControl lblLength;
        private Common.Controls.VOneTextControl txtLength;
        private Common.Controls.VOneLabelControl lblData;
        private Common.Controls.VOneLabelControl lblCodeData;
        private Common.Controls.VOneLabelControl lblCode;
        private Common.Controls.VOneTextControl txtValue;
        private Common.Controls.VOneTextControl txtDescription;
        private Common.Controls.VOneGridControl grdGeneralSetting;
        private System.Windows.Forms.Label lblgeneralsettingId;
        private System.Windows.Forms.GroupBox gbxGeneralSetting;
    }
}
