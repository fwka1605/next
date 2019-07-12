namespace Rac.VOne.Client.Screen
{
    partial class PB1601
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
            this.gbxCalendarList = new System.Windows.Forms.GroupBox();
            this.grdHolidayCalendar = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxCalendarInput = new System.Windows.Forms.GroupBox();
            this.lblHoliday = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.btnAddDate = new System.Windows.Forms.Button();
            this.datAddDate = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.datFromHoliday = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.datToHoliday = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblWaveSign = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.gbxCalendarList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHolidayCalendar)).BeginInit();
            this.gbxCalendarInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datAddDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datFromHoliday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datToHoliday)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxCalendarList
            // 
            this.gbxCalendarList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbxCalendarList.Controls.Add(this.grdHolidayCalendar);
            this.gbxCalendarList.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxCalendarList.Location = new System.Drawing.Point(207, 96);
            this.gbxCalendarList.Name = "gbxCalendarList";
            this.gbxCalendarList.Size = new System.Drawing.Size(594, 510);
            this.gbxCalendarList.TabIndex = 1;
            this.gbxCalendarList.TabStop = false;
            this.gbxCalendarList.Text = "□　登録済みの休業日";
            // 
            // grdHolidayCalendar
            // 
            this.grdHolidayCalendar.AllowAutoExtend = true;
            this.grdHolidayCalendar.AllowUserToAddRows = false;
            this.grdHolidayCalendar.AllowUserToShiftSelect = true;
            this.grdHolidayCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.grdHolidayCalendar.Location = new System.Drawing.Point(22, 27);
            this.grdHolidayCalendar.Name = "grdHolidayCalendar";
            this.grdHolidayCalendar.Size = new System.Drawing.Size(552, 477);
            this.grdHolidayCalendar.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdHolidayCalendar.TabIndex = 0;
            this.grdHolidayCalendar.TabStop = false;
            this.grdHolidayCalendar.Text = "vOneGridControl1";
            // 
            // gbxCalendarInput
            // 
            this.gbxCalendarInput.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxCalendarInput.Controls.Add(this.lblHoliday);
            this.gbxCalendarInput.Controls.Add(this.btnAddDate);
            this.gbxCalendarInput.Controls.Add(this.datAddDate);
            this.gbxCalendarInput.Controls.Add(this.datFromHoliday);
            this.gbxCalendarInput.Controls.Add(this.datToHoliday);
            this.gbxCalendarInput.Controls.Add(this.lblWaveSign);
            this.gbxCalendarInput.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbxCalendarInput.Location = new System.Drawing.Point(207, 12);
            this.gbxCalendarInput.Name = "gbxCalendarInput";
            this.gbxCalendarInput.Size = new System.Drawing.Size(594, 78);
            this.gbxCalendarInput.TabIndex = 0;
            this.gbxCalendarInput.TabStop = false;
            // 
            // lblHoliday
            // 
            this.lblHoliday.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblHoliday.Location = new System.Drawing.Point(23, 35);
            this.lblHoliday.Name = "lblHoliday";
            this.lblHoliday.Size = new System.Drawing.Size(55, 16);
            this.lblHoliday.TabIndex = 0;
            this.lblHoliday.Text = "対象期間";
            this.lblHoliday.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddDate
            // 
            this.btnAddDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAddDate.Location = new System.Drawing.Point(498, 32);
            this.btnAddDate.Name = "btnAddDate";
            this.btnAddDate.Size = new System.Drawing.Size(76, 24);
            this.btnAddDate.TabIndex = 6;
            this.btnAddDate.Text = "追加";
            this.btnAddDate.UseVisualStyleBackColor = true;
            this.btnAddDate.Click += new System.EventHandler(this.btnAddDate_Click);
            // 
            // datAddDate
            // 
            this.datAddDate.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datAddDate.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datAddDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datAddDate.Location = new System.Drawing.Point(377, 32);
            this.datAddDate.Name = "datAddDate";
            this.datAddDate.Required = true;
            this.datAddDate.Size = new System.Drawing.Size(115, 22);
            this.datAddDate.Spin.AllowSpin = false;
            this.datAddDate.TabIndex = 5;
            this.datAddDate.Value = null;
            // 
            // datFromHoliday
            // 
            this.datFromHoliday.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datFromHoliday.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datFromHoliday.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datFromHoliday.Location = new System.Drawing.Point(84, 32);
            this.datFromHoliday.Name = "datFromHoliday";
            this.datFromHoliday.Required = false;
            this.datFromHoliday.Size = new System.Drawing.Size(112, 22);
            this.datFromHoliday.Spin.AllowSpin = false;
            this.datFromHoliday.TabIndex = 3;
            this.datFromHoliday.Value = null;
            // 
            // datToHoliday
            // 
            this.datToHoliday.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datToHoliday.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datToHoliday.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datToHoliday.Location = new System.Drawing.Point(232, 32);
            this.datToHoliday.Name = "datToHoliday";
            this.datToHoliday.Required = false;
            this.datToHoliday.Size = new System.Drawing.Size(115, 22);
            this.datToHoliday.Spin.AllowSpin = false;
            this.datToHoliday.TabIndex = 4;
            this.datToHoliday.Value = null;
            // 
            // lblWaveSign
            // 
            this.lblWaveSign.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWaveSign.Location = new System.Drawing.Point(204, 35);
            this.lblWaveSign.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lblWaveSign.Name = "lblWaveSign";
            this.lblWaveSign.Size = new System.Drawing.Size(20, 16);
            this.lblWaveSign.TabIndex = 0;
            this.lblWaveSign.Text = "～";
            this.lblWaveSign.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PB1601
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbxCalendarInput);
            this.Controls.Add(this.gbxCalendarList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Name = "PB1601";
            this.Load += new System.EventHandler(this.PB1601_Load);
            this.gbxCalendarList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdHolidayCalendar)).EndInit();
            this.gbxCalendarInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datAddDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datFromHoliday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datToHoliday)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.Controls.VOneLabelControl lblHoliday;
        private Common.Controls.VOneDateControl datFromHoliday;
        private Common.Controls.VOneLabelControl lblWaveSign;
        private Common.Controls.VOneDateControl datToHoliday;
        private Common.Controls.VOneDateControl datAddDate;
        private System.Windows.Forms.Button btnAddDate;
        private System.Windows.Forms.GroupBox gbxCalendarInput;
        private System.Windows.Forms.GroupBox gbxCalendarList;
        private Common.Controls.VOneGridControl grdHolidayCalendar;
    }
}
