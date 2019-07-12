namespace Rac.VOne.Client.Screen
{
    partial class PC1701
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
            this.datBaseDate = new Rac.VOne.Client.Common.Controls.VOneDateControl(this.components);
            this.lblBaseDate = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.cbxReCreate = new System.Windows.Forms.CheckBox();
            this.grid = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.gbxPeriodicBilling = new System.Windows.Forms.GroupBox();
            this.lblPeriodicDate = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.datBaseDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.gbxPeriodicBilling.SuspendLayout();
            this.SuspendLayout();
            // 
            // datBaseDate
            // 
            this.datBaseDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.datBaseDate.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.datBaseDate.DropDownCalendar.CalendarType = GrapeCity.Win.Editors.CalendarType.YearMonth;
            this.datBaseDate.ExitOnLeftRightKey = GrapeCity.Win.Editors.ExitOnLeftRightKey.Right;
            this.datBaseDate.HighlightText = GrapeCity.Win.Editors.HighlightText.All;
            this.datBaseDate.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.datBaseDate.InputDateType = Rac.VOne.Client.Common.DateType.YearMonth;
            this.datBaseDate.Location = new System.Drawing.Point(93, 27);
            this.datBaseDate.Margin = new System.Windows.Forms.Padding(3, 15, 3, 4);
            this.datBaseDate.Name = "datBaseDate";
            this.datBaseDate.Required = true;
            this.datBaseDate.Size = new System.Drawing.Size(115, 22);
            this.datBaseDate.Spin.AllowSpin = false;
            this.datBaseDate.TabIndex = 3;
            this.datBaseDate.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.datBaseDate.ValueChanged += new System.EventHandler(this.datBaseDate_ValueChanged);
            // 
            // lblBaseDate
            // 
            this.lblBaseDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBaseDate.Location = new System.Drawing.Point(214, 29);
            this.lblBaseDate.Margin = new System.Windows.Forms.Padding(3);
            this.lblBaseDate.Name = "lblBaseDate";
            this.lblBaseDate.Size = new System.Drawing.Size(31, 16);
            this.lblBaseDate.TabIndex = 2;
            this.lblBaseDate.Text = "月分";
            this.lblBaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxReCreate
            // 
            this.cbxReCreate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbxReCreate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cbxReCreate.Location = new System.Drawing.Point(840, 29);
            this.cbxReCreate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cbxReCreate.Name = "cbxReCreate";
            this.cbxReCreate.Size = new System.Drawing.Size(153, 18);
            this.cbxReCreate.TabIndex = 4;
            this.cbxReCreate.Text = "登録済みデータを表示する";
            this.cbxReCreate.UseVisualStyleBackColor = true;
            this.cbxReCreate.CheckedChanged += new System.EventHandler(this.cbxReConfirm_CheckedChanged);
            // 
            // grid
            // 
            this.grid.AllowAutoExtend = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToShiftSelect = true;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.CurrentCellBorderLine = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.Black);
            this.grid.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grid.Location = new System.Drawing.Point(15, 22);
            this.grid.Margin = new System.Windows.Forms.Padding(12, 3, 12, 6);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(939, 519);
            this.grid.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grid.TabIndex = 5;
            this.grid.TabStop = false;
            this.grid.Text = "vOneGridControl1";
            this.grid.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.grid_CellValueChanged);
            this.grid.CellEditedFormattedValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs>(this.grid_CellEditedFormattedValueChanged);
            // 
            // gbxPeriodicBilling
            // 
            this.gbxPeriodicBilling.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxPeriodicBilling.Controls.Add(this.grid);
            this.gbxPeriodicBilling.Location = new System.Drawing.Point(24, 56);
            this.gbxPeriodicBilling.Name = "gbxPeriodicBilling";
            this.gbxPeriodicBilling.Size = new System.Drawing.Size(969, 550);
            this.gbxPeriodicBilling.TabIndex = 6;
            this.gbxPeriodicBilling.TabStop = false;
            this.gbxPeriodicBilling.Text = "定期請求データ";
            // 
            // lblPeriodicDate
            // 
            this.lblPeriodicDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPeriodicDate.Location = new System.Drawing.Point(32, 29);
            this.lblPeriodicDate.Margin = new System.Windows.Forms.Padding(3);
            this.lblPeriodicDate.Name = "lblPeriodicDate";
            this.lblPeriodicDate.Size = new System.Drawing.Size(55, 16);
            this.lblPeriodicDate.TabIndex = 2;
            this.lblPeriodicDate.Text = "処理年月";
            this.lblPeriodicDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PC1701
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxPeriodicBilling);
            this.Controls.Add(this.cbxReCreate);
            this.Controls.Add(this.datBaseDate);
            this.Controls.Add(this.lblPeriodicDate);
            this.Controls.Add(this.lblBaseDate);
            this.Name = "PC1701";
            this.Load += new System.EventHandler(this.PC1701_Load);
            ((System.ComponentModel.ISupportInitialize)(this.datBaseDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.gbxPeriodicBilling.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.Controls.VOneDateControl datBaseDate;
        private Common.Controls.VOneLabelControl lblBaseDate;
        private System.Windows.Forms.CheckBox cbxReCreate;
        private Common.Controls.VOneGridControl grid;
        private System.Windows.Forms.GroupBox gbxPeriodicBilling;
        private Common.Controls.VOneLabelControl lblPeriodicDate;
    }
}
