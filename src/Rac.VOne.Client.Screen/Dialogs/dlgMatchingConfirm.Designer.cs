namespace Rac.VOne.Client.Screen.Dialogs
{
    partial class dlgMatchingConfirm
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
            this.lblConfirmMessage = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.txtMatchingMemo = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblMatchingMemo = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingMemo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblConfirmMessage
            // 
            this.lblConfirmMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfirmMessage.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblConfirmMessage.Location = new System.Drawing.Point(12, 9);
            this.lblConfirmMessage.Name = "lblConfirmMessage";
            this.lblConfirmMessage.Size = new System.Drawing.Size(460, 110);
            this.lblConfirmMessage.TabIndex = 0;
            this.lblConfirmMessage.Text = "消込をしてよろしいでしょうか。\r\nはい：\r\nいいえ：";
            this.lblConfirmMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMatchingMemo
            // 
            this.txtMatchingMemo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMatchingMemo.DropDown.AllowDrop = false;
            this.txtMatchingMemo.HighlightText = true;
            this.txtMatchingMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMatchingMemo.Location = new System.Drawing.Point(12, 138);
            this.txtMatchingMemo.MaxLength = 100;
            this.txtMatchingMemo.Name = "txtMatchingMemo";
            this.txtMatchingMemo.Required = false;
            this.txtMatchingMemo.Size = new System.Drawing.Size(460, 22);
            this.txtMatchingMemo.TabIndex = 5;
            // 
            // lblMatchingMemo
            // 
            this.lblMatchingMemo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMatchingMemo.Location = new System.Drawing.Point(12, 119);
            this.lblMatchingMemo.Name = "lblMatchingMemo";
            this.lblMatchingMemo.Size = new System.Drawing.Size(60, 16);
            this.lblMatchingMemo.TabIndex = 4;
            this.lblMatchingMemo.Text = "消込メモ：";
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.Location = new System.Drawing.Point(175, 163);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(95, 32);
            this.btnYes.TabIndex = 6;
            this.btnYes.Text = "はい（&Y）";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(377, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 32);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNo.Location = new System.Drawing.Point(276, 163);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(95, 32);
            this.btnNo.TabIndex = 7;
            this.btnNo.Text = "いいえ（&N）";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // dlgMatchingConfirm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(484, 201);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblConfirmMessage);
            this.Controls.Add(this.txtMatchingMemo);
            this.Controls.Add(this.lblMatchingMemo);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "dlgMatchingConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "消込確認";
            this.XmlMessenger = xmlMessenger1;
            this.Load += new System.EventHandler(this.dlgMatchingConfirm_Load);
            this.Controls.SetChildIndex(this.lblMatchingMemo, 0);
            this.Controls.SetChildIndex(this.txtMatchingMemo, 0);
            this.Controls.SetChildIndex(this.lblConfirmMessage, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnNo, 0);
            this.Controls.SetChildIndex(this.btnYes, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtMatchingMemo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Common.Controls.VOneLabelControl lblConfirmMessage;
        private System.Windows.Forms.Button btnNo;
        private Common.Controls.VOneLabelControl lblMatchingMemo;
        private Common.Controls.VOneTextControl txtMatchingMemo;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnCancel;
    }
}