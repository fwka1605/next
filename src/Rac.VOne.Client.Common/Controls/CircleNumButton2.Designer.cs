namespace Rac.VOne.Client.Common.Controls
{
    partial class CircleNumButton2
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
            this.button = new System.Windows.Forms.Button();
            this.labelNum = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.pictureBox_SBase = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SBase)).BeginInit();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button.BackColor = System.Drawing.Color.White;
            this.button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.button.FlatAppearance.BorderSize = 0;
            this.button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button.Font = new System.Drawing.Font("Meiryo UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button.Location = new System.Drawing.Point(72, 1);
            this.button.Margin = new System.Windows.Forms.Padding(0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(204, 71);
            this.button.TabIndex = 6;
            this.button.Text = "button";
            this.button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button.UseVisualStyleBackColor = false;
            this.button.Click += new System.EventHandler(this.button_Click);
            this.button.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.button.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // labelNum
            // 
            this.labelNum.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelNum.BackColor = System.Drawing.Color.Transparent;
            this.labelNum.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelNum.Location = new System.Drawing.Point(17, 13);
            this.labelNum.Margin = new System.Windows.Forms.Padding(0);
            this.labelNum.Name = "labelNum";
            this.labelNum.Size = new System.Drawing.Size(42, 42);
            this.labelNum.TabIndex = 8;
            this.labelNum.Text = "01";
            this.labelNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNum.Click += new System.EventHandler(this.button_Click);
            this.labelNum.MouseEnter += new System.EventHandler(this.numCircle_MouseEnter);
            this.labelNum.MouseLeave += new System.EventHandler(this.numCircle_MouseLeave);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox.Location = new System.Drawing.Point(1, 1);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(71, 71);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.button_Click);
            this.pictureBox.MouseEnter += new System.EventHandler(this.numCircle_MouseEnter);
            this.pictureBox.MouseLeave += new System.EventHandler(this.numCircle_MouseLeave);
            // 
            // pictureBox_SBase
            // 
            this.pictureBox_SBase.BackColor = System.Drawing.Color.Black;
            this.pictureBox_SBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_SBase.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_SBase.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox_SBase.Name = "pictureBox_SBase";
            this.pictureBox_SBase.Size = new System.Drawing.Size(278, 74);
            this.pictureBox_SBase.TabIndex = 7;
            this.pictureBox_SBase.TabStop = false;
            // 
            // CircleNumButton2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.button);
            this.Controls.Add(this.labelNum);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.pictureBox_SBase);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CircleNumButton2";
            this.Size = new System.Drawing.Size(278, 74);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SBase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button button;
        private System.Windows.Forms.Label labelNum;
        public System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.PictureBox pictureBox_SBase;
    }
}
