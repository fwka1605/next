namespace Rac.VOne.Client.Screen
{
    partial class PE0115
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
            this.btnPayerName = new System.Windows.Forms.Button();
            this.btnCustomerName = new System.Windows.Forms.Button();
            this.btnCustomerCode = new System.Windows.Forms.Button();
            this.txtPayerName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.txtCustomerName = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblPayerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtCustomerCode = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblCustomerName = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnSearchCustomerCode = new System.Windows.Forms.Button();
            this.lblCustomerCode = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPayerName
            // 
            this.btnPayerName.Location = new System.Drawing.Point(381, 66);
            this.btnPayerName.Name = "btnPayerName";
            this.btnPayerName.Size = new System.Drawing.Size(55, 24);
            this.btnPayerName.TabIndex = 9;
            this.btnPayerName.Text = "検索";
            this.btnPayerName.UseVisualStyleBackColor = true;
            // 
            // btnCustomerName
            // 
            this.btnCustomerName.Location = new System.Drawing.Point(381, 40);
            this.btnCustomerName.Name = "btnCustomerName";
            this.btnCustomerName.Size = new System.Drawing.Size(55, 24);
            this.btnCustomerName.TabIndex = 6;
            this.btnCustomerName.Text = "検索";
            this.btnCustomerName.UseVisualStyleBackColor = true;
            // 
            // btnCustomerCode
            // 
            this.btnCustomerCode.Location = new System.Drawing.Point(230, 14);
            this.btnCustomerCode.Name = "btnCustomerCode";
            this.btnCustomerCode.Size = new System.Drawing.Size(55, 24);
            this.btnCustomerCode.TabIndex = 3;
            this.btnCustomerCode.Text = "検索";
            this.btnCustomerCode.UseVisualStyleBackColor = true;
            // 
            // txtPayerName
            // 
            this.txtPayerName.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtPayerName.DropDown.AllowDrop = false;
            this.txtPayerName.Format = "9AK@";
            this.txtPayerName.HighlightText = true;
            this.txtPayerName.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtPayerName.Location = new System.Drawing.Point(104, 67);
            this.txtPayerName.Name = "txtPayerName";
            this.txtPayerName.Required = false;
            this.txtPayerName.Size = new System.Drawing.Size(271, 22);
            this.txtPayerName.TabIndex = 8;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerName.DropDown.AllowDrop = false;
            this.txtCustomerName.HighlightText = true;
            this.txtCustomerName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtCustomerName.Location = new System.Drawing.Point(104, 41);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Required = false;
            this.txtCustomerName.Size = new System.Drawing.Size(271, 22);
            this.txtCustomerName.TabIndex = 5;
            // 
            // lblPayerName
            // 
            this.lblPayerName.Location = new System.Drawing.Point(21, 70);
            this.lblPayerName.Name = "lblPayerName";
            this.lblPayerName.Size = new System.Drawing.Size(77, 16);
            this.lblPayerName.TabIndex = 7;
            this.lblPayerName.Text = "振込依頼人名";
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtCustomerCode.DropDown.AllowDrop = false;
            this.txtCustomerCode.HighlightText = true;
            this.txtCustomerCode.Location = new System.Drawing.Point(104, 15);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.Required = false;
            this.txtCustomerCode.Size = new System.Drawing.Size(90, 22);
            this.txtCustomerCode.TabIndex = 1;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Location = new System.Drawing.Point(21, 44);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(77, 16);
            this.lblCustomerName.TabIndex = 4;
            this.lblCustomerName.Text = "得意先名";
            // 
            // btnSearchCustomerCode
            // 
            this.btnSearchCustomerCode.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnSearchCustomerCode.Location = new System.Drawing.Point(200, 14);
            this.btnSearchCustomerCode.Name = "btnSearchCustomerCode";
            this.btnSearchCustomerCode.Size = new System.Drawing.Size(24, 24);
            this.btnSearchCustomerCode.TabIndex = 2;
            this.btnSearchCustomerCode.UseVisualStyleBackColor = true;
            // 
            // lblCustomerCode
            // 
            this.lblCustomerCode.Location = new System.Drawing.Point(21, 18);
            this.lblCustomerCode.Name = "lblCustomerCode";
            this.lblCustomerCode.Size = new System.Drawing.Size(77, 16);
            this.lblCustomerCode.TabIndex = 0;
            this.lblCustomerCode.Text = "得意先コード";
            // 
            // PE0115
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPayerName);
            this.Controls.Add(this.btnCustomerName);
            this.Controls.Add(this.btnCustomerCode);
            this.Controls.Add(this.txtPayerName);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.lblPayerName);
            this.Controls.Add(this.txtCustomerCode);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.btnSearchCustomerCode);
            this.Controls.Add(this.lblCustomerCode);
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PE0115";
            this.Size = new System.Drawing.Size(460, 160);
            ((System.ComponentModel.ISupportInitialize)(this.txtPayerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPayerName;
        private System.Windows.Forms.Button btnCustomerName;
        private System.Windows.Forms.Button btnCustomerCode;
        private Common.Controls.VOneTextControl txtPayerName;
        private Common.Controls.VOneTextControl txtCustomerName;
        private Common.Controls.VOneLabelControl lblPayerName;
        private Common.Controls.VOneTextControl txtCustomerCode;
        private Common.Controls.VOneLabelControl lblCustomerName;
        private System.Windows.Forms.Button btnSearchCustomerCode;
        private Common.Controls.VOneLabelControl lblCustomerCode;
    }
}
