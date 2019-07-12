namespace Rac.VOne.Client.Screen
{
    partial class PB1101
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
            this.gbxSectionList = new System.Windows.Forms.GroupBox();
            this.grdSectionMaster = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxSectionInput = new System.Windows.Forms.GroupBox();
            this.lblSectionCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtSectionCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblLeftPayerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblRightPayerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblSectionName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtNote = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtLeftPayerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtSectionName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblNote = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtRightPayerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.gbxSectionList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSectionMaster)).BeginInit();
            this.gbxSectionInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeftPayerCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRightPayerCode)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxSectionList
            // 
            this.gbxSectionList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxSectionList.Controls.Add(this.grdSectionMaster);
            this.gbxSectionList.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxSectionList.Location = new System.Drawing.Point(139, 6);
            this.gbxSectionList.Name = "gbxSectionList";
            this.gbxSectionList.Size = new System.Drawing.Size(729, 457);
            this.gbxSectionList.TabIndex = 1;
            this.gbxSectionList.TabStop = false;
            this.gbxSectionList.Text = "□　登録済みの入金部門";
            // 
            // grdSectionMaster
            // 
            this.grdSectionMaster.AllowAutoExtend = true;
            this.grdSectionMaster.AllowUserToAddRows = false;
            this.grdSectionMaster.AllowUserToShiftSelect = true;
            this.grdSectionMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdSectionMaster.Location = new System.Drawing.Point(22, 24);
            this.grdSectionMaster.Name = "grdSectionMaster";
            this.grdSectionMaster.Size = new System.Drawing.Size(684, 427);
            this.grdSectionMaster.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdSectionMaster.TabIndex = 2;
            this.grdSectionMaster.TabStop = false;
            this.grdSectionMaster.Text = "vOneGridControl1";
            this.grdSectionMaster.CellClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdSectionMaster_CellClick);
            this.grdSectionMaster.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdSectionMaster_CellDoubleClick);
            // 
            // gbxSectionInput
            // 
            this.gbxSectionInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxSectionInput.Controls.Add(this.lblSectionCode);
            this.gbxSectionInput.Controls.Add(this.txtSectionCode);
            this.gbxSectionInput.Controls.Add(this.lblLeftPayerCode);
            this.gbxSectionInput.Controls.Add(this.lblRightPayerCode);
            this.gbxSectionInput.Controls.Add(this.lblSectionName);
            this.gbxSectionInput.Controls.Add(this.txtNote);
            this.gbxSectionInput.Controls.Add(this.txtLeftPayerCode);
            this.gbxSectionInput.Controls.Add(this.txtSectionName);
            this.gbxSectionInput.Controls.Add(this.lblNote);
            this.gbxSectionInput.Controls.Add(this.txtRightPayerCode);
            this.gbxSectionInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxSectionInput.Location = new System.Drawing.Point(139, 469);
            this.gbxSectionInput.Name = "gbxSectionInput";
            this.gbxSectionInput.Size = new System.Drawing.Size(729, 133);
            this.gbxSectionInput.TabIndex = 0;
            this.gbxSectionInput.TabStop = false;
            // 
            // lblSectionCode
            // 
            this.lblSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSectionCode.Location = new System.Drawing.Point(26, 20);
            this.lblSectionCode.Name = "lblSectionCode";
            this.lblSectionCode.Size = new System.Drawing.Size(81, 16);
            this.lblSectionCode.TabIndex = 0;
            this.lblSectionCode.Text = "入金部門コード";
            this.lblSectionCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSectionCode
            // 
            this.txtSectionCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtSectionCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSectionCode.DropDown.AllowDrop = false;
            this.txtSectionCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSectionCode.HighlightText = true;
            this.txtSectionCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSectionCode.Location = new System.Drawing.Point(113, 17);
            this.txtSectionCode.Name = "txtSectionCode";
            this.txtSectionCode.Required = true;
            this.txtSectionCode.Size = new System.Drawing.Size(115, 22);
            this.txtSectionCode.TabIndex = 1;
            this.txtSectionCode.Validated += new System.EventHandler(this.txtSectionCode_Validated);
            // 
            // lblLeftPayerCode
            // 
            this.lblLeftPayerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblLeftPayerCode.Location = new System.Drawing.Point(26, 104);
            this.lblLeftPayerCode.Name = "lblLeftPayerCode";
            this.lblLeftPayerCode.Size = new System.Drawing.Size(81, 16);
            this.lblLeftPayerCode.TabIndex = 6;
            this.lblLeftPayerCode.Text = "仮想支店コード";
            this.lblLeftPayerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRightPayerCode
            // 
            this.lblRightPayerCode.AllowDrop = true;
            this.lblRightPayerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRightPayerCode.Location = new System.Drawing.Point(160, 104);
            this.lblRightPayerCode.Name = "lblRightPayerCode";
            this.lblRightPayerCode.Size = new System.Drawing.Size(79, 16);
            this.lblRightPayerCode.TabIndex = 8;
            this.lblRightPayerCode.Text = "仮想口座番号";
            this.lblRightPayerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSectionName
            // 
            this.lblSectionName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSectionName.Location = new System.Drawing.Point(26, 48);
            this.lblSectionName.Name = "lblSectionName";
            this.lblSectionName.Size = new System.Drawing.Size(81, 16);
            this.lblSectionName.TabIndex = 2;
            this.lblSectionName.Text = "入金部門名";
            this.lblSectionName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNote
            // 
            this.txtNote.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtNote.DropDown.AllowDrop = false;
            this.txtNote.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtNote.HighlightText = true;
            this.txtNote.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtNote.Location = new System.Drawing.Point(113, 73);
            this.txtNote.MaxLength = 100;
            this.txtNote.Name = "txtNote";
            this.txtNote.Required = false;
            this.txtNote.Size = new System.Drawing.Size(593, 22);
            this.txtNote.TabIndex = 3;
            // 
            // txtLeftPayerCode
            // 
            this.txtLeftPayerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtLeftPayerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtLeftPayerCode.DropDown.AllowDrop = false;
            this.txtLeftPayerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtLeftPayerCode.Format = "9";
            this.txtLeftPayerCode.HighlightText = true;
            this.txtLeftPayerCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtLeftPayerCode.Location = new System.Drawing.Point(113, 101);
            this.txtLeftPayerCode.MaxLength = 3;
            this.txtLeftPayerCode.Name = "txtLeftPayerCode";
            this.txtLeftPayerCode.Required = false;
            this.txtLeftPayerCode.Size = new System.Drawing.Size(40, 22);
            this.txtLeftPayerCode.TabIndex = 4;
            // 
            // txtSectionName
            // 
            this.txtSectionName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSectionName.DropDown.AllowDrop = false;
            this.txtSectionName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSectionName.HighlightText = true;
            this.txtSectionName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtSectionName.Location = new System.Drawing.Point(113, 45);
            this.txtSectionName.MaxLength = 40;
            this.txtSectionName.Name = "txtSectionName";
            this.txtSectionName.Required = true;
            this.txtSectionName.Size = new System.Drawing.Size(350, 22);
            this.txtSectionName.TabIndex = 2;
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNote.Location = new System.Drawing.Point(26, 76);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(81, 16);
            this.lblNote.TabIndex = 4;
            this.lblNote.Text = "備考";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRightPayerCode
            // 
            this.txtRightPayerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtRightPayerCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtRightPayerCode.DropDown.AllowDrop = false;
            this.txtRightPayerCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtRightPayerCode.Format = "9";
            this.txtRightPayerCode.HighlightText = true;
            this.txtRightPayerCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtRightPayerCode.Location = new System.Drawing.Point(245, 101);
            this.txtRightPayerCode.MaxLength = 7;
            this.txtRightPayerCode.Name = "txtRightPayerCode";
            this.txtRightPayerCode.Required = false;
            this.txtRightPayerCode.Size = new System.Drawing.Size(60, 22);
            this.txtRightPayerCode.TabIndex = 5;
            // 
            // PB1101
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxSectionList);
            this.Controls.Add(this.gbxSectionInput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB1101";
            this.Load += new System.EventHandler(this.PB1101_Load);
            this.gbxSectionList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSectionMaster)).EndInit();
            this.gbxSectionInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeftPayerCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRightPayerCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxSectionList;
        private Common.Controls.VOneGridControl grdSectionMaster;
        private System.Windows.Forms.GroupBox gbxSectionInput;
        private Common.Controls.VOneLabelControl lblSectionCode;
        private Common.Controls.VOneTextControl txtSectionCode;
        private Common.Controls.VOneLabelControl lblSectionName;
        private Common.Controls.VOneTextControl txtSectionName;
        private Common.Controls.VOneLabelControl lblNote;
        private Common.Controls.VOneTextControl txtNote;
        private Common.Controls.VOneLabelControl lblLeftPayerCode;
        private Common.Controls.VOneLabelControl lblRightPayerCode;
        private Common.Controls.VOneTextControl txtLeftPayerCode;
        private Common.Controls.VOneTextControl txtRightPayerCode;
    }
}
