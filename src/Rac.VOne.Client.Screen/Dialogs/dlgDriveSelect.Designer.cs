namespace Rac.VOne.Client.Screen.Dialogs
{
    partial class dlgDriveSelect
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgDriveSelect));
            Rac.VOne.Message.XmlMessenger xmlMessenger1 = new Rac.VOne.Message.XmlMessenger();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtDrive = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblDrive = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtDrive)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(161, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtDrive
            // 
            this.txtDrive.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.txtDrive.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDrive.DropDown.AllowDrop = false;
            this.txtDrive.Format = "A";
            this.txtDrive.HighlightText = true;
            this.txtDrive.Location = new System.Drawing.Point(113, 28);
            this.txtDrive.MaxLength = 1;
            this.txtDrive.Name = "txtDrive";
            this.txtDrive.Required = true;
            this.txtDrive.Size = new System.Drawing.Size(22, 22);
            this.txtDrive.TabIndex = 0;
            // 
            // lblDrive
            // 
            this.lblDrive.AutoSize = true;
            this.lblDrive.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDrive.Location = new System.Drawing.Point(22, 32);
            this.lblDrive.Name = "lblDrive";
            this.lblDrive.Size = new System.Drawing.Size(85, 15);
            this.lblDrive.TabIndex = 14;
            this.lblDrive.Text = "使 用 ド ラ イ ブ";
            this.lblDrive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(60, 77);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(95, 32);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dlgDriveSelect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(268, 145);
            this.Controls.Add(this.lblDrive);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtDrive);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "dlgDriveSelect";
            this.Text = "クライアント側使用ドライブ登録";
            this.XmlMessenger = xmlMessenger1;
            this.Load += new System.EventHandler(this.dlgDriveSelect_Load);
            this.Controls.SetChildIndex(this.txtDrive, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.lblDrive, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtDrive)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private Common.Controls.VOneTextControl txtDrive;
        private System.Windows.Forms.Label lblDrive;
        private System.Windows.Forms.Button btnOk;
    }
}