using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Rac.VOne.Client.Common
{
    public partial class frmVOnePreviewForm : Form
    {
        /// <summary>
        ///  印刷/PDF出力 完了時の 更新処理用 ハンドラ
        /// </summary>
        public Action<Form> OutputHandler { private get; set; }

        /// <summary>PDF出力の初期フォルダ</summary>
        public string InitialExportPdfPath { get; set; }

        public delegate bool ShowSaveFileDialog(string initialDirectory, string fileName, string filter, out string path);
        public ShowSaveFileDialog ShowSaveFileDialogHandler { get; set; }

        private SectionReport Report { get; set; }
        public frmVOnePreviewForm(SectionReport report)
        {
            InitializeComponent();

            var isCloudEdition = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]);
            Icon = (isCloudEdition) ? Properties.Resources.cloud_icon : Properties.Resources.app_icon;

            Report = report;

            if (Report != null
                && Report.Document != null
                && Report.Document.Printer != null)
            {
                Report.Document.Printer.EndPrint += (sender, e) => OutputHandler?.Invoke(this);
            }
            ShowSaveFileDialogHandler = ShowSaveFileDialogInner;

            Load += frmVOnePreviewForm_Load;
            rptViewer.Click += rptViewer_Click;
            rptViewer.MouseDown += rptViewer_MouseDown;
            rptViewer.PrintingSettings
                = GrapeCity.Viewer.Common.PrintingSettings.ShowPrintDialog
                | GrapeCity.Viewer.Common.PrintingSettings.ShowPrintProgressDialog
                | GrapeCity.Viewer.Common.PrintingSettings.UsePrintingThread
                | GrapeCity.Viewer.Common.PrintingSettings.UseStandardDialog;
        }

        /// <summary>
        ///  エクスポート処理実施中
        /// </summary>
        private bool IsExporting { get; set; }
        private void btnPdf_Click(object sender, EventArgs e)
        {
            if (IsExporting) return;

            var initialDirectory = InitialExportPdfPath;
            if (string.IsNullOrWhiteSpace(initialDirectory)
                || !Directory.Exists(initialDirectory))
            {
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            var filter = "PDFファイル(*.pdf)|*.pdf";
            var fileName = $"{Report.Name}.pdf";
            var path = string.Empty;
            if (!ShowSaveFileDialogHandler(initialDirectory, fileName, filter, out path)) return;

            // TODO: export 失敗時のメッセージ
            try
            {
                IsExporting = true;
                Cursor = Cursors.WaitCursor;
                using (var export = new PdfExport())
                    export.Export(Report.Document, path);
                OutputHandler?.Invoke(this);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
                IsExporting = false;
            }
        }

        private bool ShowSaveFileDialogInner(string initialDirecotry, string fileName, string filter, out string path)
        {
            path = string.Empty;
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = filter;
                dialog.FileName = fileName;
                dialog.InitialDirectory = initialDirecotry;
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK) return false;
                path = dialog.FileName;
            }
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmVOnePreviewForm_Load(object sender, EventArgs e)
        {
            try
            {
                rptViewer.Toolbar.ToolStrip.Items.RemoveAt(30);

                var btnPdf = new ToolStripButton()
                {
                    Text = "PDF出力",
                    ToolTipText = "PDF出力",
                    Enabled = true
                };
                rptViewer.Toolbar.ToolStrip.Items.Insert(3, btnPdf);
                btnPdf.Click += btnPdf_Click;

                var btnClose = new ToolStripButton()
                {
                    Text = "閉じる",
                    ToolTipText = "閉じる",
                    Enabled = true
                };
                rptViewer.Toolbar.ToolStrip.Items.Insert(30, btnClose);
                btnClose.Click += btnClose_Click;

                rptViewer.Toolbar.ToolStrip.Items[0].Visible = false; //サイドバー
                rptViewer.Toolbar.ToolStrip.Items[1].Visible = false; //Separater

                rptViewer.Toolbar.ToolStrip.Items[2].Visible = true;  //印刷
                rptViewer.Toolbar.ToolStrip.Items[2].Text = "印刷";

                rptViewer.Toolbar.ToolStrip.Items[4].Visible = true;  //空き？
                rptViewer.Toolbar.ToolStrip.Items[5].Visible = true;  //Separater
                rptViewer.Toolbar.ToolStrip.Items[6].Visible = false; //コピー
                rptViewer.Toolbar.ToolStrip.Items[7].Visible = false; //検索
                rptViewer.Toolbar.ToolStrip.Items[8].Visible = false; //Separater
                rptViewer.Toolbar.ToolStrip.Items[9].Visible = true;  //拡大
                rptViewer.Toolbar.ToolStrip.Items[10].Visible = true;  //縮小
                rptViewer.Toolbar.ToolStrip.Items[11].Visible = true;  //ズーム
                rptViewer.Toolbar.ToolStrip.Items[12].Visible = true;  //Separater
                rptViewer.Toolbar.ToolStrip.Items[13].Visible = true;  //幅
                rptViewer.Toolbar.ToolStrip.Items[14].Visible = true;  //ページ全体
                rptViewer.Toolbar.ToolStrip.Items[15].Visible = true;  //Separater
                rptViewer.Toolbar.ToolStrip.Items[16].Visible = true;  //単一ページ
                rptViewer.Toolbar.ToolStrip.Items[17].Visible = true;  //連続ページ
                rptViewer.Toolbar.ToolStrip.Items[18].Visible = true;  //複数ページ
                rptViewer.Toolbar.ToolStrip.Items[19].Visible = true;  //Separater
                rptViewer.Toolbar.ToolStrip.Items[20].Visible = true;  //最初のページ
                rptViewer.Toolbar.ToolStrip.Items[21].Visible = true;  //前
                rptViewer.Toolbar.ToolStrip.Items[22].Visible = true;  //現ページ
                rptViewer.Toolbar.ToolStrip.Items[23].Visible = true;  //次
                rptViewer.Toolbar.ToolStrip.Items[24].Visible = true;  //最後のページ
                rptViewer.Toolbar.ToolStrip.Items[25].Visible = false; //Separater
                rptViewer.Toolbar.ToolStrip.Items[26].Visible = false; //戻る
                rptViewer.Toolbar.ToolStrip.Items[27].Visible = false; //進む
                rptViewer.Toolbar.ToolStrip.Items[28].Visible = false; //Separater
                rptViewer.Toolbar.ToolStrip.Items[29].Visible = false; //親レポートに戻る
                rptViewer.Toolbar.ToolStrip.Items[30].Visible = true;  //閉じる
                rptViewer.Toolbar.ToolStrip.Items[31].Visible = false; //更新
                rptViewer.Toolbar.ToolStrip.Items[32].Visible = false; //キャンセル
                rptViewer.Toolbar.ToolStrip.Items[33].Visible = false; //Separater
                rptViewer.Toolbar.ToolStrip.Items[34].Visible = false; //パンモード
                rptViewer.Toolbar.ToolStrip.Items[35].Visible = false; //選択モード
                rptViewer.Toolbar.ToolStrip.Items[36].Visible = false; //スナップショット
                rptViewer.Toolbar.ToolStrip.Items[37].Visible = false; //Separater
                rptViewer.Toolbar.ToolStrip.Items[38].Visible = false; //注釈

                rptViewer.ContextMenu = null;
                rptViewer.ContextMenuStrip = null;
                rptViewer.LoadDocument(Report.Document);
                rptViewer.ReportViewer.Zoom = -2;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Point clickPoint = new Point();
        private void rptViewer_MouseDown(object sender, EventArgs e)
        {
            clickPoint = MousePosition;
        }

        private void rptViewer_Click(object sender, EventArgs e)
        {
            if (!(e is MouseEventArgs)) return;
            switch (((MouseEventArgs)e).Button)
            {
                case MouseButtons.Left:
                    if (clickPoint != MousePosition) return;
                    ChangeZoomMode(true);
                    break;
                case MouseButtons.Right:
                    ChangeZoomMode(false);
                    break;
            }
        }

        private void ChangeZoomMode(bool isZoomUp)
        {
            if (rptViewer.Zoom < 0) rptViewer.Zoom = 1.0f;
            if (isZoomUp)
            {
                if (rptViewer.Zoom > 7.9f) return;
                rptViewer.Zoom += 0.1f;
            }
            else
            {
                if (rptViewer.Zoom < 0.5f) return;
                rptViewer.Zoom -= 0.1f;
            }
        }

        private enum ToolIds
        {
            Ruler = 5001,
            First = 5002,
            Last = 5003,
            PDF = 5004,
            CloseForm = 5005
        }

        /// <summary>
        /// 印刷プレビュー表示処理
        /// </summary>
        /// <param name="owner">表示する元のForm</param>
        /// <param name="report"><see cref="SectionReport"/>のインスタンス</param>
        /// <param name="initialDirectory">PDF エクスポート時のフォルダ指定</param>
        /// <param name="outputHandler">印刷/PDF エクスポート時の更新処理用 ハンドラ</param>
        /// <returns></returns>
        public static DialogResult ShowDialogPreview(IWin32Window owner,
            SectionReport report,
            string initialDirectory = null,
            Action<Form> outputHandler = null,
            ShowSaveFileDialog handler = null)
        {
            var form = new frmVOnePreviewForm(report);
            form.InitialExportPdfPath = initialDirectory;
            form.OutputHandler = outputHandler;
            form.ShowSaveFileDialogHandler = handler;
            var ownerForm = owner as Form;
            if (ownerForm != null)
            {
                form.StartPosition = FormStartPosition.Manual;
                var loc = ownerForm.Location;
                form.SetBounds(loc.X, loc.Y, ownerForm.Width, ownerForm.Height);
            }
            return form.ShowDialog(owner);
        }
    }
}
