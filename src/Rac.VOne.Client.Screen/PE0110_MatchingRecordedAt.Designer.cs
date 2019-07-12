namespace Rac.VOne.Client.Screen
{
    partial class PE0110
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
            this.lblRecordedAt = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.lblCaption2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datMatchingDate = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblCaption1 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.datMatchingDate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRecordedAt
            // 
            this.lblRecordedAt.Location = new System.Drawing.Point(43, 79);
            this.lblRecordedAt.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
            this.lblRecordedAt.Name = "lblRecordedAt";
            this.lblRecordedAt.Size = new System.Drawing.Size(91, 16);
            this.lblRecordedAt.TabIndex = 7;
            this.lblRecordedAt.Text = "消込処理年月日";
            this.lblRecordedAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCaption2
            // 
            this.lblCaption2.Location = new System.Drawing.Point(43, 47);
            this.lblCaption2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblCaption2.Name = "lblCaption2";
            this.lblCaption2.Size = new System.Drawing.Size(246, 16);
            this.lblCaption2.TabIndex = 5;
            this.lblCaption2.Text = "消込処理年月日を指定してください。";
            // 
            // datMatchingDate
            // 
            this.datMatchingDate.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datMatchingDate.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datMatchingDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datMatchingDate.Location = new System.Drawing.Point(140, 77);
            this.datMatchingDate.Name = "datMatchingDate";
            this.datMatchingDate.Required = false;
            this.datMatchingDate.Size = new System.Drawing.Size(115, 22);
            this.datMatchingDate.Spin.AllowSpin = false;
            this.datMatchingDate.TabIndex = 6;
            this.datMatchingDate.Value = null;
            // 
            // lblCaption1
            // 
            this.lblCaption1.Location = new System.Drawing.Point(43, 23);
            this.lblCaption1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblCaption1.Name = "lblCaption1";
            this.lblCaption1.Size = new System.Drawing.Size(246, 16);
            this.lblCaption1.TabIndex = 4;
            this.lblCaption1.Text = "消込対象の入金明細に前受金が含まれています。";
            this.lblCaption1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PE0110
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblRecordedAt);
            this.Controls.Add(this.lblCaption2);
            this.Controls.Add(this.datMatchingDate);
            this.Controls.Add(this.lblCaption1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(336, 214);
            this.Name = "PE0110";
            this.Size = new System.Drawing.Size(336, 214);
            ((System.ComponentModel.ISupportInitialize)(this.datMatchingDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneLabelControl lblRecordedAt;
        private Common.Controls.VOneLabelControl lblCaption2;
        private Common.Controls.VOneDateControl datMatchingDate;
        private Common.Controls.VOneLabelControl lblCaption1;
    }
}