namespace Rac.VOne.Client.Screen
{
    partial class PH1301
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
            this.gbxEBFileFormat = new System.Windows.Forms.GroupBox();
            this.grdEBFileFormatSetting = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxEBFileFormat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEBFileFormatSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxEBFileFormat
            // 
            this.gbxEBFileFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxEBFileFormat.Controls.Add(this.grdEBFileFormatSetting);
            this.gbxEBFileFormat.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxEBFileFormat.Location = new System.Drawing.Point(20, 12);
            this.gbxEBFileFormat.Name = "gbxEBFileFormat";
            this.gbxEBFileFormat.Size = new System.Drawing.Size(968, 594);
            this.gbxEBFileFormat.TabIndex = 1;
            this.gbxEBFileFormat.TabStop = false;
            this.gbxEBFileFormat.Text = "□　登録済みのEBファイル設定";
            // 
            // grdEBFileFormatSetting
            // 
            this.grdEBFileFormatSetting.AllowAutoExtend = true;
            this.grdEBFileFormatSetting.AllowUserToAddRows = false;
            this.grdEBFileFormatSetting.AllowUserToShiftSelect = true;
            this.grdEBFileFormatSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdEBFileFormatSetting.HideSelection = true;
            this.grdEBFileFormatSetting.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdEBFileFormatSetting.Location = new System.Drawing.Point(22, 22);
            this.grdEBFileFormatSetting.Name = "grdEBFileFormatSetting";
            this.grdEBFileFormatSetting.Size = new System.Drawing.Size(924, 566);
            this.grdEBFileFormatSetting.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdEBFileFormatSetting.TabIndex = 16;
            this.grdEBFileFormatSetting.TabStop = false;
            this.grdEBFileFormatSetting.Text = "vOneGridControl1";
            // 
            // PH1301
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxEBFileFormat);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PH1301";
            this.gbxEBFileFormat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdEBFileFormatSetting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbxEBFileFormat;
        private Common.Controls.VOneGridControl grdEBFileFormatSetting;
    }
}
