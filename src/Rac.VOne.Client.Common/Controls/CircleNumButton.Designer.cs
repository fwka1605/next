namespace Rac.VOne.Client.Common.Controls
{
    partial class CircleNumButton
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.button = new System.Windows.Forms.Button();
            this.pictureBox_SBase = new System.Windows.Forms.PictureBox();
            this.labelNum = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SBase)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(1);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(54, 54);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.button_Click);
            this.pictureBox.MouseEnter += new System.EventHandler(this.numCircle_MouseEnter);
            this.pictureBox.MouseLeave += new System.EventHandler(this.numCircle_MouseLeave);
            // 
            // button
            // 
            this.button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button.BackColor = System.Drawing.Color.White;
            this.button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(169)))));
            this.button.FlatAppearance.BorderSize = 0;
            this.button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button.Font = new System.Drawing.Font("Meiryo UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button.Location = new System.Drawing.Point(70, 0);
            this.button.Margin = new System.Windows.Forms.Padding(0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(164, 54);
            this.button.TabIndex = 1;
            this.button.Text = "button";
            this.button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button.UseVisualStyleBackColor = false;
            this.button.Click += new System.EventHandler(this.button_Click);
            this.button.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.button.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // pictureBox_SBase
            // 
            this.pictureBox_SBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_SBase.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_SBase.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox_SBase.Name = "pictureBox_SBase";
            this.pictureBox_SBase.Size = new System.Drawing.Size(240, 64);
            this.pictureBox_SBase.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_SBase.TabIndex = 3;
            this.pictureBox_SBase.TabStop = false;
            // 
            // labelNum
            // 
            this.labelNum.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelNum.BackColor = System.Drawing.Color.Transparent;
            this.labelNum.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelNum.Location = new System.Drawing.Point(15, 0);
            this.labelNum.Margin = new System.Windows.Forms.Padding(0);
            this.labelNum.Name = "labelNum";
            this.labelNum.Size = new System.Drawing.Size(42, 42);
            this.labelNum.TabIndex = 4;
            this.labelNum.Text = "01";
            this.labelNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNum.Click += new System.EventHandler(this.button_Click);
            this.labelNum.MouseEnter += new System.EventHandler(this.numCircle_MouseEnter);
            this.labelNum.MouseLeave += new System.EventHandler(this.numCircle_MouseLeave);
            // 
            // CircleNumButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button);
            this.Controls.Add(this.labelNum);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.pictureBox_SBase);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "CircleNumButton";
            this.Size = new System.Drawing.Size(240, 64);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SBase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBox;
        public System.Windows.Forms.Button button;
        private System.Windows.Forms.PictureBox pictureBox_SBase;
        private System.Windows.Forms.Label labelNum;
    }
}
