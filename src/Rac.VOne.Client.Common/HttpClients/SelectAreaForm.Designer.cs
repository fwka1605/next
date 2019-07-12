namespace Rac.VOne.Client.Common.HttpClients
{
    partial class SelectAreaForm
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
            this.SelectButton = new System.Windows.Forms.Button();
            this.CancelationButton = new System.Windows.Forms.Button();
            this.AreasListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // SelectButton
            // 
            this.SelectButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SelectButton.Location = new System.Drawing.Point(12, 336);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(177, 36);
            this.SelectButton.TabIndex = 1;
            this.SelectButton.Text = "選択";
            this.SelectButton.UseVisualStyleBackColor = true;
            // 
            // CancelationButton
            // 
            this.CancelationButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelationButton.Location = new System.Drawing.Point(195, 336);
            this.CancelationButton.Name = "CancelationButton";
            this.CancelationButton.Size = new System.Drawing.Size(177, 36);
            this.CancelationButton.TabIndex = 2;
            this.CancelationButton.Text = "キャンセル";
            this.CancelationButton.UseVisualStyleBackColor = true;
            // 
            // AreasListBox
            // 
            this.AreasListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AreasListBox.FormattingEnabled = true;
            this.AreasListBox.ItemHeight = 12;
            this.AreasListBox.Location = new System.Drawing.Point(12, 12);
            this.AreasListBox.Name = "AreasListBox";
            this.AreasListBox.Size = new System.Drawing.Size(360, 304);
            this.AreasListBox.TabIndex = 0;
            // 
            // SelectAreaForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(384, 384);
            this.ControlBox = false;
            this.Controls.Add(this.AreasListBox);
            this.Controls.Add(this.CancelationButton);
            this.Controls.Add(this.SelectButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "SelectAreaForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button CancelationButton;
        private System.Windows.Forms.ListBox AreasListBox;
    }
}