namespace Rac.VOne.Client.Screen
{
    partial class PH1501
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.datLastClosingMonth = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblBaseDate = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.datClosingMonth = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblClosingMonth = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxJournalizingPattern = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoNoAllowMutching = new System.Windows.Forms.RadioButton();
            this.rdoAllowMutchingPending = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoNoAllowJournalPending = new System.Windows.Forms.RadioButton();
            this.rdoAllowJournalPending = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoSalesAt = new System.Windows.Forms.RadioButton();
            this.rdoBilledAt = new System.Windows.Forms.RadioButton();
            this.vOneLabelControl4 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.vOneLabelControl3 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.vOneLabelControl2 = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.datLastClosingMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datClosingMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.gbxJournalizingPattern.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // datLastClosingMonth
            // 
            this.datLastClosingMonth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datLastClosingMonth.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datLastClosingMonth.DropDownCalendar.CalendarType = GrapeCity.Win.Editors.CalendarType.YearMonth;
            this.datLastClosingMonth.ExitOnLeftRightKey = GrapeCity.Win.Editors.ExitOnLeftRightKey.Right;
            this.datLastClosingMonth.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datLastClosingMonth.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datLastClosingMonth.InputDateType = Rac.VOne.Client.Common.DateType.YearMonth;
            this.datLastClosingMonth.Location = new System.Drawing.Point(337, 11);
            this.datLastClosingMonth.Margin = new System.Windows.Forms.Padding(3, 15, 3, 4);
            this.datLastClosingMonth.Name = "datLastClosingMonth";
            this.datLastClosingMonth.Required = true;
            this.datLastClosingMonth.Size = new System.Drawing.Size(115, 22);
            this.datLastClosingMonth.Spin.AllowSpin = false;
            this.datLastClosingMonth.TabIndex = 3;
            this.datLastClosingMonth.Value = new System.DateTime(2016, 10, 1, 0, 0, 0, 0);
            // 
            // lblBaseDate
            // 
            this.lblBaseDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBaseDate.Location = new System.Drawing.Point(250, 14);
            this.lblBaseDate.Margin = new System.Windows.Forms.Padding(18, 12, 3, 3);
            this.lblBaseDate.Name = "lblBaseDate";
            this.lblBaseDate.Size = new System.Drawing.Size(81, 16);
            this.lblBaseDate.TabIndex = 2;
            this.lblBaseDate.Text = "最終締め年月";
            this.lblBaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datClosingMonth
            // 
            this.datClosingMonth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datClosingMonth.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datClosingMonth.DropDownCalendar.CalendarType = GrapeCity.Win.Editors.CalendarType.YearMonth;
            this.datClosingMonth.ExitOnLeftRightKey = GrapeCity.Win.Editors.ExitOnLeftRightKey.Right;
            this.datClosingMonth.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datClosingMonth.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datClosingMonth.InputDateType = Rac.VOne.Client.Common.DateType.YearMonth;
            this.datClosingMonth.Location = new System.Drawing.Point(337, 39);
            this.datClosingMonth.Margin = new System.Windows.Forms.Padding(3, 15, 3, 4);
            this.datClosingMonth.Name = "datClosingMonth";
            this.datClosingMonth.Required = true;
            this.datClosingMonth.Size = new System.Drawing.Size(115, 22);
            this.datClosingMonth.Spin.AllowSpin = false;
            this.datClosingMonth.TabIndex = 5;
            this.datClosingMonth.Value = new System.DateTime(2016, 10, 1, 0, 0, 0, 0);
            // 
            // lblClosingMonth
            // 
            this.lblClosingMonth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblClosingMonth.Location = new System.Drawing.Point(250, 41);
            this.lblClosingMonth.Margin = new System.Windows.Forms.Padding(18, 12, 3, 3);
            this.lblClosingMonth.Name = "lblClosingMonth";
            this.lblClosingMonth.Size = new System.Drawing.Size(81, 16);
            this.lblClosingMonth.TabIndex = 4;
            this.lblClosingMonth.Text = "締め処理年月";
            this.lblClosingMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToResize = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(247, 68);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(514, 403);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 8;
            // 
            // gbxJournalizingPattern
            // 
            this.gbxJournalizingPattern.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbxJournalizingPattern.Controls.Add(this.panel3);
            this.gbxJournalizingPattern.Controls.Add(this.panel2);
            this.gbxJournalizingPattern.Controls.Add(this.panel1);
            this.gbxJournalizingPattern.Controls.Add(this.vOneLabelControl4);
            this.gbxJournalizingPattern.Controls.Add(this.vOneLabelControl3);
            this.gbxJournalizingPattern.Controls.Add(this.vOneLabelControl2);
            this.gbxJournalizingPattern.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxJournalizingPattern.Location = new System.Drawing.Point(247, 479);
            this.gbxJournalizingPattern.Name = "gbxJournalizingPattern";
            this.gbxJournalizingPattern.Size = new System.Drawing.Size(514, 129);
            this.gbxJournalizingPattern.TabIndex = 9;
            this.gbxJournalizingPattern.TabStop = false;
            this.gbxJournalizingPattern.Text = "締め処理設定";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoNoAllowMutching);
            this.panel3.Controls.Add(this.rdoAllowMutchingPending);
            this.panel3.Location = new System.Drawing.Point(159, 87);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(339, 28);
            this.panel3.TabIndex = 12;
            // 
            // rdoNoAllowMutching
            // 
            this.rdoNoAllowMutching.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoNoAllowMutching.Location = new System.Drawing.Point(10, 3);
            this.rdoNoAllowMutching.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.rdoNoAllowMutching.Name = "rdoNoAllowMutching";
            this.rdoNoAllowMutching.Size = new System.Drawing.Size(78, 18);
            this.rdoNoAllowMutching.TabIndex = 1;
            this.rdoNoAllowMutching.Text = "許可しない";
            this.rdoNoAllowMutching.UseVisualStyleBackColor = true;
            // 
            // rdoAllowMutchingPending
            // 
            this.rdoAllowMutchingPending.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoAllowMutchingPending.Location = new System.Drawing.Point(136, 3);
            this.rdoAllowMutchingPending.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.rdoAllowMutchingPending.Name = "rdoAllowMutchingPending";
            this.rdoAllowMutchingPending.Size = new System.Drawing.Size(86, 18);
            this.rdoAllowMutchingPending.TabIndex = 0;
            this.rdoAllowMutchingPending.Text = "許可する";
            this.rdoAllowMutchingPending.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdoNoAllowJournalPending);
            this.panel2.Controls.Add(this.rdoAllowJournalPending);
            this.panel2.Location = new System.Drawing.Point(159, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(339, 28);
            this.panel2.TabIndex = 11;
            // 
            // rdoNoAllowJournalPending
            // 
            this.rdoNoAllowJournalPending.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoNoAllowJournalPending.Location = new System.Drawing.Point(10, 4);
            this.rdoNoAllowJournalPending.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.rdoNoAllowJournalPending.Name = "rdoNoAllowJournalPending";
            this.rdoNoAllowJournalPending.Size = new System.Drawing.Size(78, 18);
            this.rdoNoAllowJournalPending.TabIndex = 1;
            this.rdoNoAllowJournalPending.Text = "許可しない";
            this.rdoNoAllowJournalPending.UseVisualStyleBackColor = true;
            // 
            // rdoAllowJournalPending
            // 
            this.rdoAllowJournalPending.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoAllowJournalPending.Location = new System.Drawing.Point(136, 4);
            this.rdoAllowJournalPending.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.rdoAllowJournalPending.Name = "rdoAllowJournalPending";
            this.rdoAllowJournalPending.Size = new System.Drawing.Size(86, 18);
            this.rdoAllowJournalPending.TabIndex = 0;
            this.rdoAllowJournalPending.Text = "許可する";
            this.rdoAllowJournalPending.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoSalesAt);
            this.panel1.Controls.Add(this.rdoBilledAt);
            this.panel1.Location = new System.Drawing.Point(159, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 28);
            this.panel1.TabIndex = 10;
            // 
            // rdoSalesAt
            // 
            this.rdoSalesAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoSalesAt.Location = new System.Drawing.Point(136, 4);
            this.rdoSalesAt.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.rdoSalesAt.Name = "rdoSalesAt";
            this.rdoSalesAt.Size = new System.Drawing.Size(78, 18);
            this.rdoSalesAt.TabIndex = 1;
            this.rdoSalesAt.Text = "売上日";
            this.rdoSalesAt.UseVisualStyleBackColor = true;
            // 
            // rdoBilledAt
            // 
            this.rdoBilledAt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoBilledAt.Location = new System.Drawing.Point(10, 4);
            this.rdoBilledAt.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.rdoBilledAt.Name = "rdoBilledAt";
            this.rdoBilledAt.Size = new System.Drawing.Size(86, 18);
            this.rdoBilledAt.TabIndex = 0;
            this.rdoBilledAt.Text = "請求日";
            this.rdoBilledAt.UseVisualStyleBackColor = true;
            // 
            // vOneLabelControl4
            // 
            this.vOneLabelControl4.Location = new System.Drawing.Point(15, 28);
            this.vOneLabelControl4.Margin = new System.Windows.Forms.Padding(18, 12, 3, 3);
            this.vOneLabelControl4.Name = "vOneLabelControl4";
            this.vOneLabelControl4.Size = new System.Drawing.Size(138, 16);
            this.vOneLabelControl4.TabIndex = 5;
            this.vOneLabelControl4.Text = "請求データ基準日付";
            this.vOneLabelControl4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vOneLabelControl3
            // 
            this.vOneLabelControl3.Location = new System.Drawing.Point(15, 60);
            this.vOneLabelControl3.Margin = new System.Windows.Forms.Padding(18, 12, 3, 3);
            this.vOneLabelControl3.Name = "vOneLabelControl3";
            this.vOneLabelControl3.Size = new System.Drawing.Size(138, 16);
            this.vOneLabelControl3.TabIndex = 4;
            this.vOneLabelControl3.Text = "入金仕訳未出力";
            this.vOneLabelControl3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vOneLabelControl2
            // 
            this.vOneLabelControl2.Location = new System.Drawing.Point(15, 91);
            this.vOneLabelControl2.Margin = new System.Windows.Forms.Padding(18, 12, 3, 3);
            this.vOneLabelControl2.Name = "vOneLabelControl2";
            this.vOneLabelControl2.Size = new System.Drawing.Size(138, 16);
            this.vOneLabelControl2.TabIndex = 3;
            this.vOneLabelControl2.Text = "未消込入金データ";
            this.vOneLabelControl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PH1501
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxJournalizingPattern);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.datClosingMonth);
            this.Controls.Add(this.lblClosingMonth);
            this.Controls.Add(this.datLastClosingMonth);
            this.Controls.Add(this.lblBaseDate);
            this.Name = "PH1501";
            ((System.ComponentModel.ISupportInitialize)(this.datLastClosingMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datClosingMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.gbxJournalizingPattern.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneDateControl datLastClosingMonth;
        private Common.Controls.VOneLabelControl lblBaseDate;
        private Common.Controls.VOneDateControl datClosingMonth;
        private Common.Controls.VOneLabelControl lblClosingMonth;
        private Common.Controls.VOneGridControl grid;
        private System.Windows.Forms.GroupBox gbxJournalizingPattern;
        private Common.Controls.VOneLabelControl vOneLabelControl4;
        private Common.Controls.VOneLabelControl vOneLabelControl3;
        private Common.Controls.VOneLabelControl vOneLabelControl2;
        private System.Windows.Forms.RadioButton rdoSalesAt;
        private System.Windows.Forms.RadioButton rdoBilledAt;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdoNoAllowMutching;
        private System.Windows.Forms.RadioButton rdoAllowMutchingPending;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdoNoAllowJournalPending;
        private System.Windows.Forms.RadioButton rdoAllowJournalPending;
        private System.Windows.Forms.Panel panel1;
    }
}
