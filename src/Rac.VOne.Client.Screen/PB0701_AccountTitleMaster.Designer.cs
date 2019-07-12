namespace Rac.VOne.Client.Screen
{
    partial class PB0701
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
            this.gbxAccountTitleInput = new System.Windows.Forms.GroupBox();
            this.lblCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblContraAccountCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblContraAccountName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtContraAccountSubCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblContraAccountSubCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtContraAccountCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtContraAccountName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.gbxAccountTitle = new System.Windows.Forms.GroupBox();
            this.grdAccountTitleMaster = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxAccountTitleInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContraAccountSubCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContraAccountCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContraAccountName)).BeginInit();
            this.gbxAccountTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountTitleMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxAccountTitleInput
            // 
            this.gbxAccountTitleInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxAccountTitleInput.Controls.Add(this.lblCode);
            this.gbxAccountTitleInput.Controls.Add(this.txtCode);
            this.gbxAccountTitleInput.Controls.Add(this.lblName);
            this.gbxAccountTitleInput.Controls.Add(this.lblContraAccountCode);
            this.gbxAccountTitleInput.Controls.Add(this.lblContraAccountName);
            this.gbxAccountTitleInput.Controls.Add(this.txtContraAccountSubCode);
            this.gbxAccountTitleInput.Controls.Add(this.lblContraAccountSubCode);
            this.gbxAccountTitleInput.Controls.Add(this.txtName);
            this.gbxAccountTitleInput.Controls.Add(this.txtContraAccountCode);
            this.gbxAccountTitleInput.Controls.Add(this.txtContraAccountName);
            this.gbxAccountTitleInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxAccountTitleInput.Location = new System.Drawing.Point(104, 507);
            this.gbxAccountTitleInput.Name = "gbxAccountTitleInput";
            this.gbxAccountTitleInput.Size = new System.Drawing.Size(800, 102);
            this.gbxAccountTitleInput.TabIndex = 0;
            this.gbxAccountTitleInput.TabStop = false;
            // 
            // lblCode
            // 
            this.lblCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCode.Location = new System.Drawing.Point(22, 18);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(105, 16);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "科目コード";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCode
            // 
            this.txtCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtCode.DropDown.AllowDrop = false;
            this.txtCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtCode.HighlightText = true;
            this.txtCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtCode.Location = new System.Drawing.Point(133, 18);
            this.txtCode.Name = "txtCode";
            this.txtCode.Required = true;
            this.txtCode.Size = new System.Drawing.Size(115, 22);
            this.txtCode.TabIndex = 2;
            this.txtCode.Validated += new System.EventHandler(this.txtCode_Validated);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblName.Location = new System.Drawing.Point(254, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(67, 16);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "科目名";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContraAccountCode
            // 
            this.lblContraAccountCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblContraAccountCode.Location = new System.Drawing.Point(22, 46);
            this.lblContraAccountCode.Name = "lblContraAccountCode";
            this.lblContraAccountCode.Size = new System.Drawing.Size(105, 16);
            this.lblContraAccountCode.TabIndex = 0;
            this.lblContraAccountCode.Text = "相手科目コード";
            this.lblContraAccountCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContraAccountName
            // 
            this.lblContraAccountName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblContraAccountName.Location = new System.Drawing.Point(254, 46);
            this.lblContraAccountName.Name = "lblContraAccountName";
            this.lblContraAccountName.Size = new System.Drawing.Size(67, 16);
            this.lblContraAccountName.TabIndex = 0;
            this.lblContraAccountName.Text = "相手科目名";
            this.lblContraAccountName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtContraAccountSubCode
            // 
            this.txtContraAccountSubCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtContraAccountSubCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtContraAccountSubCode.DropDown.AllowDrop = false;
            this.txtContraAccountSubCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtContraAccountSubCode.Format = "A9";
            this.txtContraAccountSubCode.HighlightText = true;
            this.txtContraAccountSubCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtContraAccountSubCode.Location = new System.Drawing.Point(133, 74);
            this.txtContraAccountSubCode.MaxLength = 10;
            this.txtContraAccountSubCode.Name = "txtContraAccountSubCode";
            this.txtContraAccountSubCode.Required = false;
            this.txtContraAccountSubCode.Size = new System.Drawing.Size(115, 22);
            this.txtContraAccountSubCode.TabIndex = 6;
            // 
            // lblContraAccountSubCode
            // 
            this.lblContraAccountSubCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblContraAccountSubCode.Location = new System.Drawing.Point(22, 74);
            this.lblContraAccountSubCode.Name = "lblContraAccountSubCode";
            this.lblContraAccountSubCode.Size = new System.Drawing.Size(105, 16);
            this.lblContraAccountSubCode.TabIndex = 0;
            this.lblContraAccountSubCode.Text = "相手科目補助コード";
            this.lblContraAccountSubCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtName.DropDown.AllowDrop = false;
            this.txtName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtName.HighlightText = true;
            this.txtName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtName.Location = new System.Drawing.Point(327, 18);
            this.txtName.MaxLength = 40;
            this.txtName.Name = "txtName";
            this.txtName.Required = true;
            this.txtName.Size = new System.Drawing.Size(451, 22);
            this.txtName.TabIndex = 3;
            // 
            // txtContraAccountCode
            // 
            this.txtContraAccountCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtContraAccountCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtContraAccountCode.DropDown.AllowDrop = false;
            this.txtContraAccountCode.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtContraAccountCode.HighlightText = true;
            this.txtContraAccountCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtContraAccountCode.Location = new System.Drawing.Point(133, 46);
            this.txtContraAccountCode.Name = "txtContraAccountCode";
            this.txtContraAccountCode.Required = false;
            this.txtContraAccountCode.Size = new System.Drawing.Size(115, 22);
            this.txtContraAccountCode.TabIndex = 4;
            // 
            // txtContraAccountName
            // 
            this.txtContraAccountName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtContraAccountName.DropDown.AllowDrop = false;
            this.txtContraAccountName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtContraAccountName.HighlightText = true;
            this.txtContraAccountName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtContraAccountName.Location = new System.Drawing.Point(327, 46);
            this.txtContraAccountName.MaxLength = 40;
            this.txtContraAccountName.Name = "txtContraAccountName";
            this.txtContraAccountName.Required = false;
            this.txtContraAccountName.Size = new System.Drawing.Size(451, 22);
            this.txtContraAccountName.TabIndex = 5;
            // 
            // gbxAccountTitle
            // 
            this.gbxAccountTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxAccountTitle.Controls.Add(this.grdAccountTitleMaster);
            this.gbxAccountTitle.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxAccountTitle.Location = new System.Drawing.Point(104, 15);
            this.gbxAccountTitle.Name = "gbxAccountTitle";
            this.gbxAccountTitle.Size = new System.Drawing.Size(800, 486);
            this.gbxAccountTitle.TabIndex = 1;
            this.gbxAccountTitle.TabStop = false;
            this.gbxAccountTitle.Text = "□ 登録済みの科目";
            // 
            // grdAccountTitleMaster
            // 
            this.grdAccountTitleMaster.AllowAutoExtend = true;
            this.grdAccountTitleMaster.AllowUserToAddRows = false;
            this.grdAccountTitleMaster.AllowUserToShiftSelect = true;
            this.grdAccountTitleMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdAccountTitleMaster.HideSelection = true;
            this.grdAccountTitleMaster.Location = new System.Drawing.Point(22, 22);
            this.grdAccountTitleMaster.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.grdAccountTitleMaster.Name = "grdAccountTitleMaster";
            this.grdAccountTitleMaster.Size = new System.Drawing.Size(756, 458);
            this.grdAccountTitleMaster.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdAccountTitleMaster.TabIndex = 2;
            this.grdAccountTitleMaster.TabStop = false;
            this.grdAccountTitleMaster.Text = "vOneGridControl1";
            this.grdAccountTitleMaster.VirtualMode = true;
            this.grdAccountTitleMaster.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdAccountTitleMaster_CellDoubleClick);
            // 
            // PB0701
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxAccountTitle);
            this.Controls.Add(this.gbxAccountTitleInput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB0701";
            this.Load += new System.EventHandler(this.PB0701_Load);
            this.gbxAccountTitleInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContraAccountSubCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContraAccountCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContraAccountName)).EndInit();
            this.gbxAccountTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountTitleMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxAccountTitle;
        private Common.Controls.VOneGridControl grdAccountTitleMaster;
        private System.Windows.Forms.GroupBox gbxAccountTitleInput;
        private Common.Controls.VOneLabelControl lblName;
        private Common.Controls.VOneLabelControl lblContraAccountCode;
        private Common.Controls.VOneLabelControl lblContraAccountName;
        private Common.Controls.VOneTextControl txtContraAccountCode;
        private Common.Controls.VOneLabelControl lblCode;
        private Common.Controls.VOneTextControl txtCode;
        private Common.Controls.VOneLabelControl lblContraAccountSubCode;
        private Common.Controls.VOneTextControl txtContraAccountSubCode;
        private Common.Controls.VOneTextControl txtName;
        private Common.Controls.VOneTextControl txtContraAccountName;
    }
}
