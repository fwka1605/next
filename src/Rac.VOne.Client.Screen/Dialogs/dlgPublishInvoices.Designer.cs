namespace Rac.VOne.Client.Screen.Dialogs
{
    partial class dlgPublishInvoices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgPublishInvoices));
            Rac.VOne.Message.XmlMessenger xmlMessenger1 = new Rac.VOne.Message.XmlMessenger();
            this.btnF10 = new System.Windows.Forms.Button();
            this.btnF03 = new System.Windows.Forms.Button();
            this.rdoPrintImpossible = new System.Windows.Forms.RadioButton();
            this.rdoPrintPossible = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnF10
            // 
            this.btnF10.Location = new System.Drawing.Point(105, 110);
            this.btnF10.Name = "btnF10";
            this.btnF10.Size = new System.Drawing.Size(82, 26);
            this.btnF10.TabIndex = 11;
            this.btnF10.Text = "F10/戻る";
            this.btnF10.UseVisualStyleBackColor = true;
            this.btnF10.Click += new System.EventHandler(this.btnF10_Click);
            // 
            // btnF03
            // 
            this.btnF03.Location = new System.Drawing.Point(8, 110);
            this.btnF03.Name = "btnF03";
            this.btnF03.Size = new System.Drawing.Size(82, 26);
            this.btnF03.TabIndex = 12;
            this.btnF03.Text = "F3/発行";
            this.btnF03.UseVisualStyleBackColor = true;
            this.btnF03.Click += new System.EventHandler(this.btnF03_Click);
            // 
            // rdoPrintImpossible
            // 
            this.rdoPrintImpossible.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPrintImpossible.Location = new System.Drawing.Point(41, 66);
            this.rdoPrintImpossible.Margin = new System.Windows.Forms.Padding(21, 3, 3, 3);
            this.rdoPrintImpossible.Name = "rdoPrintImpossible";
            this.rdoPrintImpossible.Size = new System.Drawing.Size(86, 18);
            this.rdoPrintImpossible.TabIndex = 14;
            this.rdoPrintImpossible.Text = "CSV出力";
            this.rdoPrintImpossible.UseVisualStyleBackColor = true;
            // 
            // rdoPrintPossible
            // 
            this.rdoPrintPossible.Checked = true;
            this.rdoPrintPossible.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPrintPossible.Location = new System.Drawing.Point(41, 26);
            this.rdoPrintPossible.Margin = new System.Windows.Forms.Padding(3, 3, 21, 3);
            this.rdoPrintPossible.Name = "rdoPrintPossible";
            this.rdoPrintPossible.Size = new System.Drawing.Size(86, 18);
            this.rdoPrintPossible.TabIndex = 13;
            this.rdoPrintPossible.TabStop = true;
            this.rdoPrintPossible.Text = "印刷";
            this.rdoPrintPossible.UseVisualStyleBackColor = true;
            // 
            // dlgPublishInvoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(194, 143);
            this.Controls.Add(this.rdoPrintImpossible);
            this.Controls.Add(this.rdoPrintPossible);
            this.Controls.Add(this.btnF03);
            this.Controls.Add(this.btnF10);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "dlgPublishInvoices";
            this.Text = "発行";
            this.XmlMessenger = xmlMessenger1;
            this.Load += new System.EventHandler(this.dlgPublishInvoices_Load);
            this.Controls.SetChildIndex(this.btnF10, 0);
            this.Controls.SetChildIndex(this.btnF03, 0);
            this.Controls.SetChildIndex(this.rdoPrintPossible, 0);
            this.Controls.SetChildIndex(this.rdoPrintImpossible, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnF10;
        private System.Windows.Forms.Button btnF03;
        private System.Windows.Forms.RadioButton rdoPrintImpossible;
        private System.Windows.Forms.RadioButton rdoPrintPossible;
    }
}