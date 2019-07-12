namespace Rac.VOne.Client.Common
{
    partial class frmVOnePreviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVOnePreviewForm));
            this.rptViewer = new GrapeCity.ActiveReports.Viewer.Win.Viewer();
            this.SuspendLayout();
            // 
            // rptViewer
            // 
            this.rptViewer.AllowSplitter = false;
            this.rptViewer.CurrentPage = 0;
            this.rptViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptViewer.Location = new System.Drawing.Point(0, 0);
            this.rptViewer.MouseModeButtonsVisible = false;
            this.rptViewer.MultiplePageCols = 1;
            this.rptViewer.MultiplePageRows = 1;
            this.rptViewer.Name = "rptViewer";
            this.rptViewer.PreviewPages = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            this.rptViewer.Sidebar.ParametersPanel.ContextMenu = null;
            this.rptViewer.Sidebar.ParametersPanel.Text = "パラメータ";
            this.rptViewer.Sidebar.ParametersPanel.Width = 200;
            // 
            // 
            // 
            this.rptViewer.Sidebar.SearchPanel.ContextMenu = null;
            this.rptViewer.Sidebar.SearchPanel.Text = "検索";
            this.rptViewer.Sidebar.SearchPanel.Width = 200;
            // 
            // 
            // 
            this.rptViewer.Sidebar.ThumbnailsPanel.ContextMenu = null;
            this.rptViewer.Sidebar.ThumbnailsPanel.Text = "サムネイル";
            this.rptViewer.Sidebar.ThumbnailsPanel.Width = 200;
            // 
            // 
            // 
            this.rptViewer.Sidebar.TocPanel.ContextMenu = null;
            this.rptViewer.Sidebar.TocPanel.Text = "見出しマップラベル";
            this.rptViewer.Sidebar.TocPanel.Width = 200;
            this.rptViewer.Sidebar.Width = 200;
            this.rptViewer.Size = new System.Drawing.Size(1008, 730);
            this.rptViewer.TabIndex = 0;
            
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.rptViewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PreviewForm";
            this.Text = "印刷プレビュー";
            this.ResumeLayout(false);

        }

        #endregion

        internal GrapeCity.ActiveReports.Viewer.Win.Viewer rptViewer;
    }
}