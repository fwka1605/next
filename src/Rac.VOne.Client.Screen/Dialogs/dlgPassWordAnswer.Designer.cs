namespace Rac.VOne.Client.Screen
{
    partial class dlgPassWordAnswer
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
            this.btnCheck = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtKeyWord = new System.Windows.Forms.TextBox();
            this.txtGetPassWord = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(59, 93);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(98, 23);
            this.btnCheck.TabIndex = 2;
            this.btnCheck.Text = "確認";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(180, 93);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // txtKeyWord
            // 
            this.txtKeyWord.Location = new System.Drawing.Point(59, 21);
            this.txtKeyWord.Name = "txtKeyWord";
            this.txtKeyWord.ReadOnly = true;
            this.txtKeyWord.Size = new System.Drawing.Size(219, 19);
            this.txtKeyWord.TabIndex = 0;
            // 
            // txtGetPassWord
            // 
            this.txtGetPassWord.Location = new System.Drawing.Point(59, 55);
            this.txtGetPassWord.Name = "txtGetPassWord";
            this.txtGetPassWord.Size = new System.Drawing.Size(219, 19);
            this.txtGetPassWord.TabIndex = 1;
            // 
            // dlgPassWordAnswer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 147);
            this.Controls.Add(this.txtGetPassWord);
            this.Controls.Add(this.txtKeyWord);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCheck);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgPassWordAnswer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "パスワード答え";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtKeyWord;
        private System.Windows.Forms.TextBox txtGetPassWord;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Button btnClose;

    }
}