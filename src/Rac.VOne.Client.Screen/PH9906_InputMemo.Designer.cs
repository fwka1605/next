namespace Rac.VOne.Client.Screen
{
    partial class PH9906
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
            this.txtMemo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMemo
            // 
            this.txtMemo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMemo.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtMemo.DropDown.AllowDrop = false;
            this.txtMemo.HighlightText = true;
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(15, 17);
            this.txtMemo.MaxLength = 100;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Required = false;
            this.txtMemo.Size = new System.Drawing.Size(470, 22);
            this.txtMemo.TabIndex = 2;
            // 
            // PH9906
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtMemo);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PH9906";
            this.Size = new System.Drawing.Size(500, 60);
            ((System.ComponentModel.ISupportInitialize)(this.txtMemo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneTextControl txtMemo;
    }
}
