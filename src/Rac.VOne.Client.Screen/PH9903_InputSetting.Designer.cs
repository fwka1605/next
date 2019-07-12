namespace Rac.VOne.Client.Screen
{
    partial class PH9903
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
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.HideSelection = true;
            this.grid.Location = new System.Drawing.Point(19, 15);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(300, 362);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 2;
            // 
            // PH9903
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.grid);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PH9903";
            this.Size = new System.Drawing.Size(350, 490);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneGridControl grid;
    }
}
