namespace Rac.VOne.Client.Screen
{
    partial class PB0504
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
            this.rdoCustomer = new System.Windows.Forms.RadioButton();
            this.rdoFee = new System.Windows.Forms.RadioButton();
            this.rdoDiscount = new System.Windows.Forms.RadioButton();
            this.lblPatternNo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtPatternNumber = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnPatternSearch = new System.Windows.Forms.Button();
            this.lblPatternName = new Rac.VOne.Client.Common.Controls.VOneDispLabelControl(this.components);
            this.vOneLabelControl1 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPatternName)).BeginInit();
            this.SuspendLayout();
            // 
            // rdoCustomer
            // 
            this.rdoCustomer.Checked = true;
            this.rdoCustomer.Location = new System.Drawing.Point(25, 25);
            this.rdoCustomer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoCustomer.Name = "rdoCustomer";
            this.rdoCustomer.Size = new System.Drawing.Size(97, 18);
            this.rdoCustomer.TabIndex = 1;
            this.rdoCustomer.TabStop = true;
            this.rdoCustomer.Text = "得意先マスター";
            this.rdoCustomer.UseVisualStyleBackColor = true;
            // 
            // rdoFee
            // 
            this.rdoFee.Location = new System.Drawing.Point(192, 25);
            this.rdoFee.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoFee.Name = "rdoFee";
            this.rdoFee.Size = new System.Drawing.Size(97, 18);
            this.rdoFee.TabIndex = 2;
            this.rdoFee.Text = "登録手数料";
            this.rdoFee.UseVisualStyleBackColor = true;
            // 
            // rdoDiscount
            // 
            this.rdoDiscount.Location = new System.Drawing.Point(356, 25);
            this.rdoDiscount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoDiscount.Name = "rdoDiscount";
            this.rdoDiscount.Size = new System.Drawing.Size(97, 18);
            this.rdoDiscount.TabIndex = 3;
            this.rdoDiscount.Text = "歩引設定";
            this.rdoDiscount.UseVisualStyleBackColor = true;
            // 
            // lblPatternNo
            // 
            this.lblPatternNo.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatternNo.Location = new System.Drawing.Point(11, 54);
            this.lblPatternNo.Name = "lblPatternNo";
            this.lblPatternNo.Size = new System.Drawing.Size(60, 16);
            this.lblPatternNo.TabIndex = 9;
            this.lblPatternNo.Text = "パターンNo";
            this.lblPatternNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPatternNumber
            // 
            this.txtPatternNumber.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtPatternNumber.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtPatternNumber.DropDown.AllowDrop = false;
            this.txtPatternNumber.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatternNumber.Format = "9";
            this.txtPatternNumber.HighlightText = true;
            this.txtPatternNumber.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPatternNumber.Location = new System.Drawing.Point(77, 52);
            this.txtPatternNumber.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtPatternNumber.MaxLength = 2;
            this.txtPatternNumber.Name = "txtPatternNumber";
            this.txtPatternNumber.Required = false;
            this.txtPatternNumber.Size = new System.Drawing.Size(25, 22);
            this.txtPatternNumber.TabIndex = 4;
            // 
            // btnPatternSearch
            // 
            this.btnPatternSearch.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatternSearch.Image = global::Rac.VOne.Client.Screen.Properties.Resources.search;
            this.btnPatternSearch.Location = new System.Drawing.Point(108, 51);
            this.btnPatternSearch.Name = "btnPatternSearch";
            this.btnPatternSearch.Size = new System.Drawing.Size(24, 24);
            this.btnPatternSearch.TabIndex = 5;
            this.btnPatternSearch.UseVisualStyleBackColor = false;
            // 
            // lblPatternName
            // 
            this.lblPatternName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPatternName.DropDown.AllowDrop = false;
            this.lblPatternName.Enabled = false;
            this.lblPatternName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatternName.HighlightText = true;
            this.lblPatternName.Location = new System.Drawing.Point(77, 83);
            this.lblPatternName.Name = "lblPatternName";
            this.lblPatternName.ReadOnly = true;
            this.lblPatternName.Required = false;
            this.lblPatternName.Size = new System.Drawing.Size(390, 22);
            this.lblPatternName.TabIndex = 6;
            // 
            // vOneLabelControl1
            // 
            this.vOneLabelControl1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vOneLabelControl1.Location = new System.Drawing.Point(11, 85);
            this.vOneLabelControl1.Name = "vOneLabelControl1";
            this.vOneLabelControl1.Size = new System.Drawing.Size(60, 16);
            this.vOneLabelControl1.TabIndex = 9;
            this.vOneLabelControl1.Text = "パターン名";
            this.vOneLabelControl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PB0504
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblPatternName);
            this.Controls.Add(this.btnPatternSearch);
            this.Controls.Add(this.txtPatternNumber);
            this.Controls.Add(this.vOneLabelControl1);
            this.Controls.Add(this.lblPatternNo);
            this.Controls.Add(this.rdoCustomer);
            this.Controls.Add(this.rdoFee);
            this.Controls.Add(this.rdoDiscount);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "PB0504";
            this.Size = new System.Drawing.Size(500, 200);
            ((System.ComponentModel.ISupportInitialize)(this.txtPatternNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPatternName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoCustomer;
        private System.Windows.Forms.RadioButton rdoFee;
        private System.Windows.Forms.RadioButton rdoDiscount;
        private Common.Controls.VOneLabelControl lblPatternNo;
        private Common.Controls.VOneTextControl txtPatternNumber;
        private System.Windows.Forms.Button btnPatternSearch;
        private Common.Controls.VOneDispLabelControl lblPatternName;
        private Common.Controls.VOneLabelControl vOneLabelControl1;
    }
}
