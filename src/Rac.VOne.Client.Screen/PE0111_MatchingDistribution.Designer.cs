namespace Rac.VOne.Client.Screen
{
    partial class PE0111
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.vOneLabelControl1 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlOptionButton = new System.Windows.Forms.Panel();
            this.rdoCheckOnly = new System.Windows.Forms.RadioButton();
            this.rdoUncheckOnly = new System.Windows.Forms.RadioButton();
            this.chkOnlyBillingData = new System.Windows.Forms.CheckBox();
            this.chkOnlyReceiptData = new System.Windows.Forms.CheckBox();
            this.pnlOptionButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // vOneLabelControl1
            // 
            this.vOneLabelControl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.vOneLabelControl1.Location = new System.Drawing.Point(29, 12);
            this.vOneLabelControl1.Name = "vOneLabelControl1";
            this.vOneLabelControl1.Size = new System.Drawing.Size(170, 16);
            this.vOneLabelControl1.TabIndex = 2;
            this.vOneLabelControl1.Text = "配信するデータを選択してください。";
            this.vOneLabelControl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlOptionButton
            // 
            this.pnlOptionButton.Controls.Add(this.rdoCheckOnly);
            this.pnlOptionButton.Controls.Add(this.rdoUncheckOnly);
            this.pnlOptionButton.Location = new System.Drawing.Point(29, 32);
            this.pnlOptionButton.Name = "pnlOptionButton";
            this.pnlOptionButton.Size = new System.Drawing.Size(269, 46);
            this.pnlOptionButton.TabIndex = 3;
            // 
            // rdoCheckOnly
            // 
            this.rdoCheckOnly.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoCheckOnly.Location = new System.Drawing.Point(3, 13);
            this.rdoCheckOnly.Name = "rdoCheckOnly";
            this.rdoCheckOnly.Size = new System.Drawing.Size(77, 18);
            this.rdoCheckOnly.TabIndex = 2;
            this.rdoCheckOnly.TabStop = true;
            this.rdoCheckOnly.Text = "チェックのみ";
            this.rdoCheckOnly.UseVisualStyleBackColor = true;
            // 
            // rdoUncheckOnly
            // 
            this.rdoUncheckOnly.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoUncheckOnly.Location = new System.Drawing.Point(138, 13);
            this.rdoUncheckOnly.Name = "rdoUncheckOnly";
            this.rdoUncheckOnly.Size = new System.Drawing.Size(96, 18);
            this.rdoUncheckOnly.TabIndex = 3;
            this.rdoUncheckOnly.TabStop = true;
            this.rdoUncheckOnly.Text = "チェックなしのみ";
            this.rdoUncheckOnly.UseVisualStyleBackColor = true;
            // 
            // chkOnlyBillingData
            // 
            this.chkOnlyBillingData.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkOnlyBillingData.Location = new System.Drawing.Point(29, 85);
            this.chkOnlyBillingData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkOnlyBillingData.Name = "chkOnlyBillingData";
            this.chkOnlyBillingData.Size = new System.Drawing.Size(149, 18);
            this.chkOnlyBillingData.TabIndex = 4;
            this.chkOnlyBillingData.Text = "請求のみのデータも含める";
            this.chkOnlyBillingData.UseVisualStyleBackColor = true;
            // 
            // chkOnlyReceiptData
            // 
            this.chkOnlyReceiptData.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkOnlyReceiptData.Location = new System.Drawing.Point(29, 111);
            this.chkOnlyReceiptData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkOnlyReceiptData.Name = "chkOnlyReceiptData";
            this.chkOnlyReceiptData.Size = new System.Drawing.Size(149, 18);
            this.chkOnlyReceiptData.TabIndex = 5;
            this.chkOnlyReceiptData.Text = "入金のみのデータも含める";
            this.chkOnlyReceiptData.UseVisualStyleBackColor = true;
            // 
            // PE0111
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.chkOnlyBillingData);
            this.Controls.Add(this.chkOnlyReceiptData);
            this.Controls.Add(this.pnlOptionButton);
            this.Controls.Add(this.vOneLabelControl1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(336, 214);
            this.Name = "PE0111";
            this.Size = new System.Drawing.Size(336, 214);
            this.pnlOptionButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneLabelControl vOneLabelControl1;
        private System.Windows.Forms.Panel pnlOptionButton;
        private System.Windows.Forms.RadioButton rdoCheckOnly;
        private System.Windows.Forms.RadioButton rdoUncheckOnly;
        private System.Windows.Forms.CheckBox chkOnlyBillingData;
        private System.Windows.Forms.CheckBox chkOnlyReceiptData;
    }
}