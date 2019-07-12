namespace Rac.VOne.Client.Screen
{
    partial class PC1502
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
            this.pnlSearchMain = new System.Windows.Forms.Panel();
            this.txtSearchKey = new Rac.VOne.Client.Common.Controls.VOneTextControl(this.components);
            this.lblSearchKey = new Rac.VOne.Client.Common.Controls.VOneLabelControl();
            this.grdSearch = new Rac.VOne.Client.Common.Controls.VOneGridControl();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.pnlSearchMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.pnlGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSearchMain
            // 
            this.pnlSearchMain.Controls.Add(this.txtSearchKey);
            this.pnlSearchMain.Controls.Add(this.lblSearchKey);
            this.pnlSearchMain.Location = new System.Drawing.Point(0, 0);
            this.pnlSearchMain.Name = "pnlSearchMain";
            this.pnlSearchMain.Size = new System.Drawing.Size(644, 47);
            this.pnlSearchMain.TabIndex = 1;
            // 
            // txtSearchKey
            // 
            this.txtSearchKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchKey.DropDown.AllowDrop = false;
            this.txtSearchKey.HighlightText = true;
            this.txtSearchKey.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtSearchKey.Location = new System.Drawing.Point(64, 12);
            this.txtSearchKey.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.txtSearchKey.Name = "txtSearchKey";
            this.txtSearchKey.Required = false;
            this.txtSearchKey.Size = new System.Drawing.Size(571, 20);
            this.txtSearchKey.TabIndex = 1;
            // 
            // lblSearchKey
            // 
            this.lblSearchKey.AutoSize = true;
            this.lblSearchKey.Location = new System.Drawing.Point(9, 15);
            this.lblSearchKey.Margin = new System.Windows.Forms.Padding(9, 0, 3, 0);
            this.lblSearchKey.Name = "lblSearchKey";
            this.lblSearchKey.Size = new System.Drawing.Size(51, 15);
            this.lblSearchKey.TabIndex = 0;
            this.lblSearchKey.Text = "検索キー";
            // 
            // grdSearch
            // 
            this.grdSearch.AllowAutoExtend = true;
            this.grdSearch.AllowUserToAddRows = false;
            this.grdSearch.AllowUserToShiftSelect = true;
            this.grdSearch.DefaultCellNoteStyle.BackColor = System.Drawing.Color.LightYellow;
            this.grdSearch.DefaultCellNoteStyle.BackgroundGradientEffect = new GrapeCity.Win.MultiRow.GradientEffect(null, GrapeCity.Win.MultiRow.GradientStyle.None, GrapeCity.Win.MultiRow.GradientDirection.Center);
            this.grdSearch.DefaultCellNoteStyle.BackgroundImageLayout = GrapeCity.Win.MultiRow.MultiRowImageLayout.Tile;
            this.grdSearch.DefaultCellNoteStyle.BackgroundImageOpacity = 1D;
            this.grdSearch.DefaultCellNoteStyle.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grdSearch.DefaultCellNoteStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.grdSearch.DefaultCellNoteStyle.LineAdjustment = GrapeCity.Win.MultiRow.LineAdjustment.None;
            this.grdSearch.DefaultCellNoteStyle.LineColor = System.Drawing.Color.Black;
            this.grdSearch.DefaultCellNoteStyle.LineStyle = GrapeCity.Win.MultiRow.CellNoteLineStyle.Thin;
            this.grdSearch.DefaultCellNoteStyle.LineWidth = 1F;
            this.grdSearch.DefaultCellNoteStyle.Multiline = GrapeCity.Win.MultiRow.MultiRowTriState.True;
            this.grdSearch.DefaultCellNoteStyle.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.grdSearch.DefaultCellNoteStyle.PatternColor = System.Drawing.SystemColors.WindowText;
            this.grdSearch.DefaultCellNoteStyle.PatternStyle = GrapeCity.Win.MultiRow.MultiRowHatchStyle.None;
            this.grdSearch.DefaultCellNoteStyle.ShowShadow = GrapeCity.Win.MultiRow.MultiRowTriState.True;
            this.grdSearch.DefaultCellNoteStyle.TextAdjustment = GrapeCity.Win.MultiRow.TextAdjustment.Near;
            this.grdSearch.DefaultCellNoteStyle.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.TopLeft;
            this.grdSearch.DefaultCellNoteStyle.TextAngle = 0F;
            this.grdSearch.DefaultCellNoteStyle.TextEffect = GrapeCity.Win.MultiRow.TextEffect.Flat;
            this.grdSearch.DefaultCellNoteStyle.TextIndent = 0;
            this.grdSearch.DefaultCellNoteStyle.TextVertical = GrapeCity.Win.MultiRow.MultiRowTriState.False;
            this.grdSearch.DefaultCellNoteStyle.TriangleSymbolColor = System.Drawing.Color.Red;
            this.grdSearch.DefaultCellNoteStyle.UseCompatibleTextRendering = GrapeCity.Win.MultiRow.MultiRowTriState.False;
            this.grdSearch.DefaultCellNoteStyle.WordWrap = GrapeCity.Win.MultiRow.MultiRowTriState.True;
            this.grdSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSearch.HorizontalScrollMode = GrapeCity.Win.MultiRow.ScrollMode.Cell;
            this.grdSearch.Location = new System.Drawing.Point(0, 0);
            this.grdSearch.Margin = new System.Windows.Forms.Padding(9, 3, 9, 3);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(644, 501);
            this.grdSearch.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.grdSearch.TabIndex = 4;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.grdSearch);
            this.pnlGrid.Location = new System.Drawing.Point(0, 53);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(644, 501);
            this.pnlGrid.TabIndex = 5;
            // 
            // PC1502
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSearchMain);
            this.MinimumSize = new System.Drawing.Size(650, 550);
            this.Name = "PC1502";
            this.Size = new System.Drawing.Size(650, 550);
            this.pnlSearchMain.ResumeLayout(false);
            this.pnlSearchMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSearchMain;
        private Common.Controls.VOneTextControl txtSearchKey;
        private Common.Controls.VOneLabelControl lblSearchKey;
        private Common.Controls.VOneGridControl grdSearch;
        private System.Windows.Forms.Panel pnlGrid;
    }
}
