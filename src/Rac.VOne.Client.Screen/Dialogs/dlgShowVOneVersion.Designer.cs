namespace Rac.VOne.Client.Screen.Dialogs
{
    partial class dlgShowVOneVersion
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
            this.Copyright1 = new System.Windows.Forms.Label();
            this.Copyright2 = new System.Windows.Forms.Label();
            this.OK_button = new System.Windows.Forms.Button();
            this.VOneVer1 = new System.Windows.Forms.Label();
            this.VOneVer2 = new System.Windows.Forms.Label();
            this.picturelogo = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picturelogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Copyright1
            // 
            this.Copyright1.AutoSize = true;
            this.Copyright1.Location = new System.Drawing.Point(30, 99);
            this.Copyright1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Copyright1.Name = "Copyright1";
            this.Copyright1.Size = new System.Drawing.Size(147, 12);
            this.Copyright1.TabIndex = 0;
            this.Copyright1.Text = "Copyright (C) R&AC Co,Ltd.";
            this.Copyright1.UseMnemonic = false;
            // 
            // Copyright2
            // 
            this.Copyright2.AutoSize = true;
            this.Copyright2.BackColor = System.Drawing.SystemColors.Control;
            this.Copyright2.Location = new System.Drawing.Point(30, 120);
            this.Copyright2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Copyright2.Name = "Copyright2";
            this.Copyright2.Size = new System.Drawing.Size(187, 12);
            this.Copyright2.TabIndex = 1;
            this.Copyright2.Text = "Copyright (c) 2003-2010 David Hall";
            // 
            // OK_button
            // 
            this.OK_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.OK_button.Location = new System.Drawing.Point(88, 144);
            this.OK_button.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(74, 21);
            this.OK_button.TabIndex = 2;
            this.OK_button.Text = "OK";
            this.OK_button.UseVisualStyleBackColor = false;
            this.OK_button.Click += new System.EventHandler(this.OKButtonClicked);
            // 
            // VOneVer1
            // 
            this.VOneVer1.AutoSize = true;
            this.VOneVer1.BackColor = System.Drawing.Color.White;
            this.VOneVer1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.VOneVer1.Location = new System.Drawing.Point(95, 26);
            this.VOneVer1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.VOneVer1.Name = "VOneVer1";
            this.VOneVer1.Size = new System.Drawing.Size(105, 12);
            this.VOneVer1.TabIndex = 3;
            this.VOneVer1.Text = "Victory ONE G4";
            // 
            // VOneVer2
            // 
            this.VOneVer2.AutoSize = true;
            this.VOneVer2.BackColor = System.Drawing.Color.White;
            this.VOneVer2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.VOneVer2.Location = new System.Drawing.Point(95, 47);
            this.VOneVer2.Name = "VOneVer2";
            this.VOneVer2.Size = new System.Drawing.Size(41, 12);
            this.VOneVer2.TabIndex = 5;
            this.VOneVer2.Text = "label1";
            // 
            // picturelogo
            // 
            this.picturelogo.BackColor = System.Drawing.Color.White;
            this.picturelogo.Image = global::Rac.VOne.Client.Screen.Properties.Resources.logo_002;
            this.picturelogo.Location = new System.Drawing.Point(32, 12);
            this.picturelogo.Margin = new System.Windows.Forms.Padding(0);
            this.picturelogo.Name = "picturelogo";
            this.picturelogo.Size = new System.Drawing.Size(58, 56);
            this.picturelogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picturelogo.TabIndex = 6;
            this.picturelogo.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(-1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(252, 85);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // ShowVOneVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(252, 177);
            this.Controls.Add(this.picturelogo);
            this.Controls.Add(this.VOneVer2);
            this.Controls.Add(this.VOneVer1);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.Copyright2);
            this.Controls.Add(this.Copyright1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShowVOneVersion";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.picturelogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Copyright1;
        private System.Windows.Forms.Label Copyright2;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Label VOneVer1;
        internal System.Windows.Forms.Label VOneVer2;
        private System.Windows.Forms.PictureBox picturelogo;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}