namespace Rac.VOne.Client.Screen
{
    partial class PB0102
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
            this.grdImportSetting = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.grdImportSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // grdImportSetting
            // 
            this.grdImportSetting.AllowAutoExtend = true;
            this.grdImportSetting.AllowUserToAddRows = false;
            this.grdImportSetting.AllowUserToShiftSelect = true;
            this.grdImportSetting.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grdImportSetting.HorizontalScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.grdImportSetting.Location = new System.Drawing.Point(99, 60);
            this.grdImportSetting.Name = "grdImportSetting";
            this.grdImportSetting.Size = new System.Drawing.Size(810, 492);
            this.grdImportSetting.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdImportSetting.TabIndex = 0;
            this.grdImportSetting.Text = "マスターインポート設定";
            this.grdImportSetting.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grid_CellValueChanged);
            this.grdImportSetting.CellEditedFormattedValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs>(this.grid_CellEditedFormattedValueChanged);
            // 
            // PB0102
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grdImportSetting);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.MinimumSize = new System.Drawing.Size(1008, 612);
            this.Name = "PB0102";
            this.Size = new System.Drawing.Size(1008, 612);
            this.Load += new System.EventHandler(this.PB0102_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdImportSetting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneGridControl grdImportSetting;
    }
}
