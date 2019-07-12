namespace Rac.VOne.Client.Screen
{
    partial class PE0112
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
            this.lblCaption = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.pnlOptionButton = new System.Windows.Forms.Panel();
            this.rdoChecked = new System.Windows.Forms.RadioButton();
            this.rdoUnChecked = new System.Windows.Forms.RadioButton();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.pnlOptionButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Location = new System.Drawing.Point(15, 33);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(144, 16);
            this.lblCaption.TabIndex = 4;
            this.lblCaption.Text = "出力内容を選択して下さい。";
            // 
            // pnlOptionButton
            // 
            this.pnlOptionButton.Controls.Add(this.rdoChecked);
            this.pnlOptionButton.Controls.Add(this.rdoUnChecked);
            this.pnlOptionButton.Controls.Add(this.rdoAll);
            this.pnlOptionButton.Location = new System.Drawing.Point(29, 58);
            this.pnlOptionButton.Name = "pnlOptionButton";
            this.pnlOptionButton.Size = new System.Drawing.Size(280, 40);
            this.pnlOptionButton.TabIndex = 5;
            // 
            // rdoChecked
            // 
            this.rdoChecked.Location = new System.Drawing.Point(3, 10);
            this.rdoChecked.Name = "rdoChecked";
            this.rdoChecked.Size = new System.Drawing.Size(77, 18);
            this.rdoChecked.TabIndex = 3;
            this.rdoChecked.TabStop = true;
            this.rdoChecked.Text = "チェックのみ";
            this.rdoChecked.UseVisualStyleBackColor = true;
            // 
            // rdoUnChecked
            // 
            this.rdoUnChecked.Location = new System.Drawing.Point(96, 10);
            this.rdoUnChecked.Name = "rdoUnChecked";
            this.rdoUnChecked.Size = new System.Drawing.Size(96, 18);
            this.rdoUnChecked.TabIndex = 4;
            this.rdoUnChecked.TabStop = true;
            this.rdoUnChecked.Text = "チェックなしのみ";
            this.rdoUnChecked.UseVisualStyleBackColor = true;
            // 
            // rdoAll
            // 
            this.rdoAll.Location = new System.Drawing.Point(211, 10);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(49, 18);
            this.rdoAll.TabIndex = 5;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "全件";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // PE0112
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlOptionButton);
            this.Controls.Add(this.lblCaption);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(336, 214);
            this.Name = "PE0112";
            this.Size = new System.Drawing.Size(336, 214);
            this.pnlOptionButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneLabelControl lblCaption;
        private System.Windows.Forms.Panel pnlOptionButton;
        private System.Windows.Forms.RadioButton rdoChecked;
        private System.Windows.Forms.RadioButton rdoUnChecked;
        private System.Windows.Forms.RadioButton rdoAll;
    }
}