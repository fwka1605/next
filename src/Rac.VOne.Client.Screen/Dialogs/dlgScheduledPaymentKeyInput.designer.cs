namespace Rac.VOne.Client.Screen.Dialogs
{
    partial class dlgScheduledPaymentKeyInput
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
            Rac.VOne.Message.XmlMessenger xmlMessenger1 = new Rac.VOne.Message.XmlMessenger();
            this.lblSearchKey = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtSearchKey = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.vOneLabelControl1 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKey)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSearchKey
            // 
            this.lblSearchKey.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSearchKey.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchKey.Location = new System.Drawing.Point(12, 40);
            this.lblSearchKey.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lblSearchKey.Name = "lblSearchKey";
            this.lblSearchKey.Size = new System.Drawing.Size(87, 16);
            this.lblSearchKey.TabIndex = 1;
            this.lblSearchKey.Text = "入金予定キー：";
            this.lblSearchKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSearchKey
            // 
            this.txtSearchKey.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSearchKey.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSearchKey.DropDown.AllowDrop = false;
            this.txtSearchKey.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchKey.HighlightText = true;
            this.txtSearchKey.Location = new System.Drawing.Point(12, 62);
            this.txtSearchKey.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.txtSearchKey.MaxLength = 80;
            this.txtSearchKey.Name = "txtSearchKey";
            this.txtSearchKey.Required = false;
            this.txtSearchKey.Size = new System.Drawing.Size(271, 22);
            this.txtSearchKey.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(188, 96);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "いいえ(&N)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnYes
            // 
            this.btnYes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnYes.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.Location = new System.Drawing.Point(87, 96);
            this.btnYes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(95, 32);
            this.btnYes.TabIndex = 3;
            this.btnYes.Text = "はい(&Y)";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // vOneLabelControl1
            // 
            this.vOneLabelControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.vOneLabelControl1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vOneLabelControl1.Location = new System.Drawing.Point(12, 12);
            this.vOneLabelControl1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.vOneLabelControl1.Name = "vOneLabelControl1";
            this.vOneLabelControl1.Size = new System.Drawing.Size(137, 16);
            this.vOneLabelControl1.TabIndex = 1;
            this.vOneLabelControl1.Text = "更新してもよろしいですか？";
            this.vOneLabelControl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dlgScheduledPaymentKeyInput
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(295, 140);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.vOneLabelControl1);
            this.Controls.Add(this.lblSearchKey);
            this.Controls.Add(this.txtSearchKey);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.Name = "dlgScheduledPaymentKeyInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "確認";
            this.XmlMessenger = xmlMessenger1;
            this.Controls.SetChildIndex(this.txtSearchKey, 0);
            this.Controls.SetChildIndex(this.lblSearchKey, 0);
            this.Controls.SetChildIndex(this.vOneLabelControl1, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnYes, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKey)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Controls.VOneLabelControl lblSearchKey;
        private Common.Controls.VOneTextControl txtSearchKey;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnYes;
        private Common.Controls.VOneLabelControl vOneLabelControl1;
    }
}