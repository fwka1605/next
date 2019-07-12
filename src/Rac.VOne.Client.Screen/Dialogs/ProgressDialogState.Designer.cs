namespace Rac.VOne.Client.Screen.Dialogs
{
    partial class ProgressDialogState
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressDialogState));
            this.lblSummary = new System.Windows.Forms.Label();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.pbxIcon = new System.Windows.Forms.PictureBox();
            this.lblStartDateTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnDispDetail = new System.Windows.Forms.Button();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.lblProgressPercentage = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblTaskProgress = new System.Windows.Forms.Label();
            this.pnlSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.pnlDetail.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("MS UI Gothic", 14F);
            this.lblSummary.Location = new System.Drawing.Point(66, 32);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(85, 19);
            this.lblSummary.TabIndex = 1;
            this.lblSummary.Text = "処理内容";
            // 
            // pnlSummary
            // 
            this.pnlSummary.BackColor = System.Drawing.Color.White;
            this.pnlSummary.Controls.Add(this.pbxIcon);
            this.pnlSummary.Controls.Add(this.lblSummary);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSummary.Location = new System.Drawing.Point(0, 0);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new System.Drawing.Size(487, 82);
            this.pnlSummary.TabIndex = 1;
            // 
            // pbxIcon
            // 
            this.pbxIcon.Location = new System.Drawing.Point(19, 25);
            this.pbxIcon.Name = "pbxIcon";
            this.pbxIcon.Size = new System.Drawing.Size(32, 32);
            this.pbxIcon.TabIndex = 2;
            this.pbxIcon.TabStop = false;
            // 
            // lblStartDateTime
            // 
            this.lblStartDateTime.AutoSize = true;
            this.lblStartDateTime.Location = new System.Drawing.Point(365, 3);
            this.lblStartDateTime.Name = "lblStartDateTime";
            this.lblStartDateTime.Size = new System.Drawing.Size(109, 12);
            this.lblStartDateTime.TabIndex = 2;
            this.lblStartDateTime.Text = "2017/08/02 00:00:00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(306, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "開始時刻";
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblElapsedTime.AutoSize = true;
            this.lblElapsedTime.Location = new System.Drawing.Point(429, 3);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(45, 12);
            this.lblElapsedTime.TabIndex = 4;
            this.lblElapsedTime.Text = "00:00:00";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(370, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "経過時間";
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Location = new System.Drawing.Point(399, 44);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 9;
            this.btnExecute.Text = "キャンセル";
            this.btnExecute.UseVisualStyleBackColor = true;
            // 
            // btnDispDetail
            // 
            this.btnDispDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDispDetail.Location = new System.Drawing.Point(318, 44);
            this.btnDispDetail.Name = "btnDispDetail";
            this.btnDispDetail.Size = new System.Drawing.Size(75, 23);
            this.btnDispDetail.TabIndex = 10;
            this.btnDispDetail.Text = "詳細非表示";
            this.btnDispDetail.UseVisualStyleBackColor = true;
            // 
            // prgBar
            // 
            this.prgBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.prgBar.Location = new System.Drawing.Point(14, 18);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(460, 23);
            this.prgBar.TabIndex = 8;
            // 
            // lblProgressPercentage
            // 
            this.lblProgressPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProgressPercentage.AutoSize = true;
            this.lblProgressPercentage.Location = new System.Drawing.Point(12, 49);
            this.lblProgressPercentage.Name = "lblProgressPercentage";
            this.lblProgressPercentage.Size = new System.Drawing.Size(29, 12);
            this.lblProgressPercentage.TabIndex = 6;
            this.lblProgressPercentage.Text = "100%";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "完了";
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.GridColorType = Rac.VOne.Client.Common.MultiRow.GridColorType.Special;
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(14, 18);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(460, 194);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 11;
            this.grid.Text = "vOneGridControl1";
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.lblStartDateTime);
            this.pnlDetail.Controls.Add(this.grid);
            this.pnlDetail.Controls.Add(this.label1);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetail.Location = new System.Drawing.Point(0, 82);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(487, 216);
            this.pnlDetail.TabIndex = 12;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblTaskProgress);
            this.pnlBottom.Controls.Add(this.lblElapsedTime);
            this.pnlBottom.Controls.Add(this.label2);
            this.pnlBottom.Controls.Add(this.btnExecute);
            this.pnlBottom.Controls.Add(this.label3);
            this.pnlBottom.Controls.Add(this.btnDispDetail);
            this.pnlBottom.Controls.Add(this.lblProgressPercentage);
            this.pnlBottom.Controls.Add(this.prgBar);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBottom.Location = new System.Drawing.Point(0, 298);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(487, 79);
            this.pnlBottom.TabIndex = 13;
            // 
            // lblTaskProgress
            // 
            this.lblTaskProgress.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTaskProgress.AutoSize = true;
            this.lblTaskProgress.Location = new System.Drawing.Point(17, 3);
            this.lblTaskProgress.Name = "lblTaskProgress";
            this.lblTaskProgress.Size = new System.Drawing.Size(39, 12);
            this.lblTaskProgress.TabIndex = 11;
            this.lblTaskProgress.Text = "0/0 件";
            // 
            // ProgressDialogState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 377);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlDetail);
            this.Controls.Add(this.pnlSummary);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgressDialogState";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmProgressDialogState";
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblStartDateTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnDispDetail;
        private System.Windows.Forms.ProgressBar prgBar;
        private System.Windows.Forms.Label lblProgressPercentage;
        private System.Windows.Forms.Label label3;
        private Common.Controls.VOneGridControl grid;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.PictureBox pbxIcon;
        private System.Windows.Forms.Label lblTaskProgress;
    }
}