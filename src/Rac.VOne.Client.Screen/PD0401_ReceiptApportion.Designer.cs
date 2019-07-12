namespace Rac.VOne.Client.Screen
{
    partial class PD0401
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
            this.gbxImportHistory = new System.Windows.Forms.GroupBox();
            this.lblNewDescription = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdImportHistory = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.lblUpdatedDescription = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.picUpdatedColor = new System.Windows.Forms.PictureBox();
            this.picNewColor = new System.Windows.Forms.PictureBox();
            this.gbxReceiptDetail = new System.Windows.Forms.GroupBox();
            this.cbxSectionAssign = new System.Windows.Forms.CheckBox();
            this.grdReceiptDetails = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.cbxLearnIgnoreKana = new System.Windows.Forms.CheckBox();
            this.gbxImportHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdImportHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpdatedColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewColor)).BeginInit();
            this.gbxReceiptDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxImportHistory
            // 
            this.gbxImportHistory.Controls.Add(this.lblNewDescription);
            this.gbxImportHistory.Controls.Add(this.grdImportHistory);
            this.gbxImportHistory.Controls.Add(this.lblUpdatedDescription);
            this.gbxImportHistory.Controls.Add(this.picUpdatedColor);
            this.gbxImportHistory.Controls.Add(this.picNewColor);
            this.gbxImportHistory.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxImportHistory.Location = new System.Drawing.Point(12, 15);
            this.gbxImportHistory.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.gbxImportHistory.Name = "gbxImportHistory";
            this.gbxImportHistory.Padding = new System.Windows.Forms.Padding(0);
            this.gbxImportHistory.Size = new System.Drawing.Size(984, 236);
            this.gbxImportHistory.TabIndex = 0;
            this.gbxImportHistory.TabStop = false;
            this.gbxImportHistory.Text = "□　取込履歴";
            // 
            // lblNewDescription
            // 
            this.lblNewDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNewDescription.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNewDescription.Location = new System.Drawing.Point(824, 18);
            this.lblNewDescription.Name = "lblNewDescription";
            this.lblNewDescription.Size = new System.Drawing.Size(43, 16);
            this.lblNewDescription.TabIndex = 0;
            this.lblNewDescription.Text = "更新前";
            this.lblNewDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdImportHistory
            // 
            this.grdImportHistory.AllowAutoExtend = true;
            this.grdImportHistory.AllowUserToAddRows = false;
            this.grdImportHistory.AllowUserToShiftSelect = true;
            this.grdImportHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdImportHistory.GridColorType = Rac.VOne.Client.Common.MultiRow.GridColorType.Special;
            this.grdImportHistory.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdImportHistory.Location = new System.Drawing.Point(9, 42);
            this.grdImportHistory.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grdImportHistory.Name = "grdImportHistory";
            this.grdImportHistory.Size = new System.Drawing.Size(966, 188);
            this.grdImportHistory.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdImportHistory.TabIndex = 0;
            this.grdImportHistory.DataBindingComplete += new System.EventHandler<GrapeCity.Win.MultiRow.MultiRowBindingCompleteEventArgs>(this.grdImportHistory_DataBindingComplete);
            this.grdImportHistory.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdImportHistory_CellValueChanged);
            this.grdImportHistory.CellContentButtonClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdImportHistory_CellContentButtonClick);
            // 
            // lblUpdatedDescription
            // 
            this.lblUpdatedDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdatedDescription.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUpdatedDescription.Location = new System.Drawing.Point(910, 18);
            this.lblUpdatedDescription.Name = "lblUpdatedDescription";
            this.lblUpdatedDescription.Size = new System.Drawing.Size(43, 16);
            this.lblUpdatedDescription.TabIndex = 0;
            this.lblUpdatedDescription.Text = "更新済";
            this.lblUpdatedDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picUpdatedColor
            // 
            this.picUpdatedColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picUpdatedColor.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.picUpdatedColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picUpdatedColor.Location = new System.Drawing.Point(874, 16);
            this.picUpdatedColor.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.picUpdatedColor.Name = "picUpdatedColor";
            this.picUpdatedColor.Size = new System.Drawing.Size(30, 20);
            this.picUpdatedColor.TabIndex = 6;
            this.picUpdatedColor.TabStop = false;
            // 
            // picNewColor
            // 
            this.picNewColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picNewColor.BackColor = System.Drawing.SystemColors.Window;
            this.picNewColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picNewColor.Location = new System.Drawing.Point(788, 16);
            this.picNewColor.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.picNewColor.Name = "picNewColor";
            this.picNewColor.Size = new System.Drawing.Size(30, 20);
            this.picNewColor.TabIndex = 5;
            this.picNewColor.TabStop = false;
            // 
            // gbxReceiptDetail
            // 
            this.gbxReceiptDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxReceiptDetail.Controls.Add(this.cbxSectionAssign);
            this.gbxReceiptDetail.Controls.Add(this.grdReceiptDetails);
            this.gbxReceiptDetail.Controls.Add(this.cbxLearnIgnoreKana);
            this.gbxReceiptDetail.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxReceiptDetail.Location = new System.Drawing.Point(12, 257);
            this.gbxReceiptDetail.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.gbxReceiptDetail.Name = "gbxReceiptDetail";
            this.gbxReceiptDetail.Padding = new System.Windows.Forms.Padding(0);
            this.gbxReceiptDetail.Size = new System.Drawing.Size(984, 349);
            this.gbxReceiptDetail.TabIndex = 1;
            this.gbxReceiptDetail.TabStop = false;
            this.gbxReceiptDetail.Text = "□　入金明細";
            // 
            // cbxSectionAssign
            // 
            this.cbxSectionAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSectionAssign.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cbxSectionAssign.Location = new System.Drawing.Point(669, 16);
            this.cbxSectionAssign.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.cbxSectionAssign.Name = "cbxSectionAssign";
            this.cbxSectionAssign.Size = new System.Drawing.Size(198, 18);
            this.cbxSectionAssign.TabIndex = 3;
            this.cbxSectionAssign.Text = "入金部門 未指定のデータのみ表示";
            this.cbxSectionAssign.UseVisualStyleBackColor = true;
            this.cbxSectionAssign.CheckedChanged += new System.EventHandler(this.cbxSectionAssign_CheckedChanged);
            // 
            // grdReceiptDetails
            // 
            this.grdReceiptDetails.AllowAutoExtend = true;
            this.grdReceiptDetails.AllowUserToAddRows = false;
            this.grdReceiptDetails.AllowUserToShiftSelect = true;
            this.grdReceiptDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdReceiptDetails.GridColorType = Rac.VOne.Client.Common.MultiRow.GridColorType.Special;
            this.grdReceiptDetails.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdReceiptDetails.Location = new System.Drawing.Point(9, 40);
            this.grdReceiptDetails.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grdReceiptDetails.Name = "grdReceiptDetails";
            this.grdReceiptDetails.Size = new System.Drawing.Size(966, 303);
            this.grdReceiptDetails.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdReceiptDetails.TabIndex = 0;
            this.grdReceiptDetails.CellValidating += new System.EventHandler<GrapeCity.Win.MultiRow.CellValidatingEventArgs>(this.grdReceiptDetails_CellValidating);
            this.grdReceiptDetails.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptDetails_CellValueChanged);
            this.grdReceiptDetails.CellEndEdit += new System.EventHandler<GrapeCity.Win.MultiRow.CellEndEditEventArgs>(this.grdReceiptDetails_CellEndEdit);
            this.grdReceiptDetails.CellDoubleClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptDetails_CellDoubleClick);
            this.grdReceiptDetails.CellContentClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptDetails_CellContentClick);
            this.grdReceiptDetails.CellContentButtonClick += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grdReceiptDetails_CellBtnClick);
            // 
            // cbxLearnIgnoreKana
            // 
            this.cbxLearnIgnoreKana.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxLearnIgnoreKana.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cbxLearnIgnoreKana.Location = new System.Drawing.Point(873, 16);
            this.cbxLearnIgnoreKana.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.cbxLearnIgnoreKana.Name = "cbxLearnIgnoreKana";
            this.cbxLearnIgnoreKana.Size = new System.Drawing.Size(102, 18);
            this.cbxLearnIgnoreKana.TabIndex = 4;
            this.cbxLearnIgnoreKana.Text = "除外カナを学習";
            this.cbxLearnIgnoreKana.UseVisualStyleBackColor = true;
            // 
            // PD0401
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxReceiptDetail);
            this.Controls.Add(this.gbxImportHistory);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PD0401";
            this.Load += new System.EventHandler(this.PD0401_Load);
            this.gbxImportHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdImportHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpdatedColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewColor)).EndInit();
            this.gbxReceiptDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdReceiptDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxImportHistory;
        private Common.Controls.VOneLabelControl lblNewDescription;
        private System.Windows.Forms.GroupBox gbxReceiptDetail;
        private System.Windows.Forms.CheckBox cbxSectionAssign;
        private System.Windows.Forms.CheckBox cbxLearnIgnoreKana;
        private Common.Controls.VOneGridControl grdImportHistory;
        private Common.Controls.VOneLabelControl lblUpdatedDescription;
        private Common.Controls.VOneGridControl grdReceiptDetails;
        private System.Windows.Forms.PictureBox picUpdatedColor;
        private System.Windows.Forms.PictureBox picNewColor;
    }
}
