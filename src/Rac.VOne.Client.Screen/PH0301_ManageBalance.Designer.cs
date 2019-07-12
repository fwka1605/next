namespace Rac.VOne.Client.Screen
{
    partial class PH0301
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
            this.lblLastCarryOverAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCarryOverAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datLastCarryOverAt = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.datCarryOverAt = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.datLastCarryOverAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datCarryOverAt)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLastCarryOverAt
            // 
            this.lblLastCarryOverAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastCarryOverAt.Location = new System.Drawing.Point(26, 30);
            this.lblLastCarryOverAt.Name = "lblLastCarryOverAt";
            this.lblLastCarryOverAt.Size = new System.Drawing.Size(67, 16);
            this.lblLastCarryOverAt.TabIndex = 0;
            this.lblLastCarryOverAt.Text = "前回繰越日";
            this.lblLastCarryOverAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCarryOverAt
            // 
            this.lblCarryOverAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCarryOverAt.Location = new System.Drawing.Point(26, 58);
            this.lblCarryOverAt.Name = "lblCarryOverAt";
            this.lblCarryOverAt.Size = new System.Drawing.Size(67, 16);
            this.lblCarryOverAt.TabIndex = 0;
            this.lblCarryOverAt.Text = "繰越年月日";
            this.lblCarryOverAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datLastCarryOverAt
            // 
            this.datLastCarryOverAt.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datLastCarryOverAt.Enabled = false;
            this.datLastCarryOverAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datLastCarryOverAt.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datLastCarryOverAt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datLastCarryOverAt.Location = new System.Drawing.Point(99, 28);
            this.datLastCarryOverAt.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.datLastCarryOverAt.Name = "datLastCarryOverAt";
            this.datLastCarryOverAt.Required = false;
            this.datLastCarryOverAt.Size = new System.Drawing.Size(115, 22);
            this.datLastCarryOverAt.Spin.AllowSpin = false;
            this.datLastCarryOverAt.TabIndex = 0;
            this.datLastCarryOverAt.Value = new System.DateTime(2016, 11, 1, 0, 0, 0, 0);
            // 
            // datCarryOverAt
            // 
            this.datCarryOverAt.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datCarryOverAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datCarryOverAt.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datCarryOverAt.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datCarryOverAt.Location = new System.Drawing.Point(99, 56);
            this.datCarryOverAt.Name = "datCarryOverAt";
            this.datCarryOverAt.Required = true;
            this.datCarryOverAt.Size = new System.Drawing.Size(115, 22);
            this.datCarryOverAt.Spin.AllowSpin = false;
            this.datCarryOverAt.TabIndex = 1;
            this.datCarryOverAt.Value = new System.DateTime(2016, 11, 1, 0, 0, 0, 0);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.lblLastCarryOverAt);
            this.groupBox1.Controls.Add(this.datLastCarryOverAt);
            this.groupBox1.Controls.Add(this.datCarryOverAt);
            this.groupBox1.Controls.Add(this.lblCarryOverAt);
            this.groupBox1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(379, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // PH0301
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PH0301";
            this.Load += new System.EventHandler(this.PH0301_Load);
            ((System.ComponentModel.ISupportInitialize)(this.datLastCarryOverAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datCarryOverAt)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneLabelControl lblLastCarryOverAt;
        private Common.Controls.VOneLabelControl lblCarryOverAt;
        private Common.Controls.VOneDateControl datLastCarryOverAt;
        private Common.Controls.VOneDateControl datCarryOverAt;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
