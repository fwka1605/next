using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>データトランスファー</summary>
    public partial class PH0401 : VOneScreenBase
    {
        #region 変数宣言
        private List<ListItem> ClientFileList { get; set; }
        private List<ListItem> ServerFileList { get; set; }
        private string ClientDesktopDirectory { get; set; }
        private string ServerDesktopDirectory { get; set; }

        //private const string DefaultClient = "\\\\tsclient\\";
        #endregion

        #region PH0401 データトランスファー
        public PH0401()
        {
            InitializeComponent();
            grdClient.SetupShortcutKeys();
            grdServer.SetupShortcutKeys();
            Text = "データトランスファー";
        }
        #endregion

        #region InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption(string.Empty);
            BaseContext.SetFunction01Enabled(false);

            BaseContext.SetFunction02Caption(string.Empty);
            BaseContext.SetFunction02Enabled(false);

            BaseContext.SetFunction03Caption(string.Empty);
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption(string.Empty);
            BaseContext.SetFunction04Enabled(false);

            BaseContext.SetFunction05Caption("ドライブ切替");
            BaseContext.SetFunction05Enabled(false);
            OnF05ClickHandler = ChangeDrive;

            BaseContext.SetFunction06Caption(string.Empty);
            BaseContext.SetFunction06Enabled(false);

            BaseContext.SetFunction07Caption(string.Empty);
            BaseContext.SetFunction07Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PH0401_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var loadTask = new List<Task>();

                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());

                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());

                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask.ToArray()), false, SessionKey);

                ClientFileList = new List<ListItem>();
                ServerFileList = new List<ListItem>();
                ClientDesktopDirectory = DefaultClient;
                ServerDesktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                InitializeGridTemplate(grdClient);
                InitializeGridTemplate(grdServer);
                SetClient();
                SetServer();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region クライアントパス・データ読込
        private void SetClient()
        {
            try
            {
                var prevPath = Settings.RestorePath<PH0401>(Login);
                txtClientPath.Text = !string.IsNullOrEmpty(prevPath) ? OmitPath(prevPath) : string.Empty;

                if (string.IsNullOrEmpty(prevPath) || !Directory.Exists(prevPath))
                    return;

                ClientFileList.Clear();
                ClientFileList = GetFiles(prevPath);
                grdClient.DataSource = new BindingSource(ClientFileList, null);
                btnClientCheckAll.Enabled = ClientFileList.Count > 0;
                btnClientUncheckAll.Enabled = ClientFileList.Count > 0;
                BaseContext.SetFunction05Enabled(!string.IsNullOrEmpty(txtClientPath.Text));
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is UnauthorizedAccessException)
                    txtClientPath.Clear();
                else
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region サーバーパス・データ読込
        private void SetServer()
        {
            try
            {
                var task = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var path = task.Result;
                if (!string.IsNullOrEmpty(path))
                    txtServerPath.Text = path;

                if (string.IsNullOrEmpty(txtServerPath.Text) || !Directory.Exists(txtServerPath.Text))
                    return;

                ServerFileList.Clear();
                ServerFileList = GetFiles(txtServerPath.Text);
                grdServer.DataSource = new BindingSource(ServerFileList, null);
                btnServerCheckAll.Enabled = ServerFileList.Count > 0;
                btnServerUncheckAll.Enabled = ServerFileList.Count > 0;
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is UnauthorizedAccessException)
                    txtServerPath.Clear();
                else
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region クライアント/サーバーグリッド初期設定
        private void InitializeGridTemplate(Common.Controls.VOneGridControl grid)
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "CheckBox", caption: "選択", readOnly: false, cell: builder.GetCheckBoxCell()),
                new CellSetting(height, 115, "FileName", caption: "ファイル名"           , cell: builder.GetTextBoxCell(), dataField: nameof(ListItem.Text)),
            });

            grid.Template = builder.Build();
            grid.HideSelection = true;
            grid.ColumnHeaders[0].Cells[0].Style.Multiline = MultiRowTriState.True;
            grid.ColumnHeaders[0].Cells[0].Value = "選\r\n択";
            grid.ColumnHeaders[0].Cells[0].PerformVerticalAutoFit();
        }

        #endregion

        #region クライアントグリッドの選択チェック変更
        private void grdClient_CellContentClick(object sender, CellEventArgs e)
        {
            CheckChanged(sender, e);
        }

        private void grdClient_CellValueChanged(object sender, CellEventArgs e)
        {
            CheckChanged(sender, e);
        }
        #endregion

        #region サーバーグリッドの選択チェック変更
        private void grdServer_CellContentClick(object sender, CellEventArgs e)
        {
            CheckChanged(sender, e);
        }

        private void grdServer_CellValueChanged(object sender, CellEventArgs e)
        {
            CheckChanged(sender, e);
        }
        #endregion

        #region グリッドの選択チェック変更
        private void CheckChanged(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0 || e.CellIndex > 0)
                return;

            var grid = sender.Equals(grdServer) ? grdServer : grdClient;
            var button = sender.Equals(grdServer) ? btnDL : btnTransfer;

            button.Enabled = false;
            for (var i = 0; i < grid.RowCount; i++)
            {
                var cellValue = grid.Rows[i].Cells[0].EditedFormattedValue;
                var checkValue = Convert.ToBoolean(cellValue);

                if (checkValue)
                {
                    button.Enabled = true;
                    break;
                }
            }
        }
        #endregion

        #region クライアントフォルダの選択ボタン押下
        private void btnClientFolderChoose_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnClientFolderChoose, alternativeText: lblClient.Text + ":" + btnClientFolderChoose.Text);
            ClearStatusMessage();

            try
            {
                string clientPath = string.Empty;
                if (string.IsNullOrEmpty(txtClientPath.Text))
                {
                    using (var dlg = CreateDialog<dlgDriveSelect>())
                    {
                        dlg.StartPosition = FormStartPosition.CenterParent;
                        DialogResult result = ApplicationContext.ShowDialog(ParentForm, dlg, true);
                        if (this.Confirmed(result) != DialogResult.OK)
                            return;

                        clientPath = SystemInformation.TerminalServerSession ? DefaultClient + dlg.Drive + @"\" : dlg.Drive + @":\";
                    }
                }
                else
                {
                    clientPath = txtClientPath.Text;
                }

                var rootBrowserPath = new List<string>();
                if (!ShowRootFolderBrowserDialog(clientPath, out rootBrowserPath, FolderBrowserType.SelectClientFolder)) return;

                ShowClientSide(rootBrowserPath.FirstOrDefault());
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is UnauthorizedAccessException)
                {
                    grdClient.DataSource = null;
                    btnTransfer.Enabled = false;
                    btnClientCheckAll.Enabled = false;
                    btnClientUncheckAll.Enabled = false;
                }
                else
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
            }
        }
        #endregion

        #region サーバーフォルダの選択ボタン押下
        private void btnServerFileChoose_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnServerFileChoose, alternativeText: lblServer.Text + ":" + btnServerFileChoose.Text);
            ClearStatusMessage();

            try
            {
                var serverPath = !string.IsNullOrEmpty(txtServerPath.Text) ? txtServerPath.Text : ServerDesktopDirectory;
                var rootBrowserPath = new List<string>();
                if (!ShowRootFolderBrowserDialog(serverPath, out rootBrowserPath, FolderBrowserType.SelectAndEditFolder)) return;

                txtServerPath.Text = rootBrowserPath.FirstOrDefault();
                ServerFileList.Clear();
                ServerFileList = GetFiles(rootBrowserPath.FirstOrDefault());
                grdServer.DataSource = new BindingSource(ServerFileList, null);
                btnDL.Enabled = false;
                btnServerCheckAll.Enabled = ServerFileList.Count > 0;
                btnServerUncheckAll.Enabled = ServerFileList.Count > 0;

            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is UnauthorizedAccessException)
                {
                    grdServer.DataSource = null;
                    btnDL.Enabled = false;
                    btnServerCheckAll.Enabled = false;
                    btnServerUncheckAll.Enabled = false;
                }
                else
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
            }
        }
        #endregion

        #region ファイルの取得
        private List<ListItem> GetFiles(string selectedPath)
        {
            List<ListItem> files = new List<ListItem>();
            try
            {
                if (string.IsNullOrEmpty(selectedPath) || !Directory.Exists(selectedPath))
                    return files;

                var directory = new DirectoryInfo(selectedPath);
                var filePath = directory.GetFiles()
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.System) && !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .OrderByDescending(p => p.LastWriteTime)
                    .ToList();

                foreach (var path in filePath)
                    files.Add(new ListItem(Path.GetFileName(path.Name)));
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is UnauthorizedAccessException)
                    throw;
                else
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return files;
        }
        #endregion

        #region クライアントグリッドの全選択ボタン押下
        private void btnClientCheckAll_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            if (grdClient.RowCount <= 0)
                return;

            this.ButtonClicked(btnClientCheckAll, alternativeText: lblClient.Text + ":" + btnClientCheckAll.Text);
            CheckAllChanged(sender: grdClient, check: true);
            btnTransfer.Enabled = true;
        }
        #endregion

        #region サーバーグリッドの全選択ボタン押下
        private void btnServerCheckAll_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            if (grdServer.RowCount <= 0)
                return;

            this.ButtonClicked(btnServerCheckAll, alternativeText: lblServer.Text + ":" + btnServerCheckAll.Text);
            CheckAllChanged(sender: grdServer, check: true);
            btnDL.Enabled = true;
        }
        #endregion

        #region クライアントグリッドの全解除ボタン押下
        private void btnClientUncheckAll_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            if (grdClient.RowCount <= 0)
                return;

            this.ButtonClicked(btnClientUncheckAll, alternativeText: lblClient.Text + ":" + btnClientUncheckAll.Text);
            CheckAllChanged(sender: grdClient, check: false);
            btnTransfer.Enabled = false;
        }
        #endregion

        #region サーバーグリッドの全解除ボタン押下
        private void btnServerUncheckAll_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            if (grdServer.RowCount <= 0)
                return;

            this.ButtonClicked(btnServerUncheckAll, alternativeText: lblServer.Text + ":" + btnServerUncheckAll.Text);
            CheckAllChanged(sender: grdServer, check: false);
            btnDL.Enabled = false;
        }
        #endregion

        #region グリッドの全選択・全解除
        private void CheckAllChanged(object sender, bool check)
        {
            var grid = sender.Equals(grdServer) ? grdServer : grdClient;

            for (var i = 0; i < grid.RowCount; i++)
                grid.Rows[i].Cells[0].Value = check;
        }
        #endregion

        #region クライアントファイル削除ボタン押下
        private void btnClientFileDelete_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnClientFileDelete, alternativeText: lblClient.Text + ":" + btnClientFileDelete.Text);
            DeleteFile(grdClient);
        }
        #endregion

        #region サーバーファイル削除ボタン押下
        private void btnServerFileDelete_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnServerFileDelete, alternativeText: lblServer.Text + ":" + btnServerFileDelete.Text);
            DeleteFile(grdServer);
        }
        #endregion

        #region ファイル削除
        private void DeleteFile(object sender)
        {
            ClearStatusMessage();

            if (!ValidateCheckBox(sender))
            {
                ShowWarningDialog(MsgWngSelectionRequired, "削除するファイル");
                return;
            }

            if (!ShowConfirmDialog(MsgQstConfirmDeleteSpecificFile))
                return;

            var fileName = string.Empty;
            try
            {
                var grid = sender.Equals(grdServer) ? grdServer : grdClient;
                var path = sender.Equals(grdServer) ? txtServerPath.Text : AppendPath(txtClientPath.Text);

                for (var i = 0; i < grid.RowCount; i++)
                {
                    var check = Convert.ToBoolean(grid.Rows[i].Cells[0].EditedFormattedValue);

                    if (check)
                    {
                        fileName = grid.Rows[i].Cells[1].Value.ToString();
                        var fileInfo = new FileInfo(path + "\\" + fileName);

                        if (!IsFileinUse(fileInfo))
                            fileInfo.Delete();
                        else
                            throw new IOException();
                    }
                }

                LoadLatestGrid();
                ShowWarningDialog(MsgInfDeleteSuccess);
            }
            catch (IOException)
            {
                LoadLatestGrid();
                ShowWarningDialog(MsgErrSpecificFileUsingOtherProcess, fileName, "削除");
            }
            catch (UnauthorizedAccessException)
            {
                LoadLatestGrid();
                ShowWarningDialog(MsgErrAccessDeniedForSpecificFile, fileName, "削除");
            }
            catch (Exception)
            {
                LoadLatestGrid();
                ShowWarningDialog(MsgErrDeleteError);
            }
        }
        #endregion

        #region 転送ボタン押下
        private void btnTransfer_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnTransfer);

            ClearStatusMessage();

            if (string.IsNullOrEmpty(txtServerPath.Text))
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "サーバー側のフォルダ");
                return;
            }

            if (!Directory.Exists(txtServerPath.Text))
            {
                ShowWarningDialog(MsgWngNotExistFolder);
                return;
            }

            if (!ValidateFileExists(sender, duplicate: true))
                return;

            if (!ShowConfirmDialog(MsgQstConfirmTransferForSpecificFile))
                return;

            if (!ValidateFileExists(sender, duplicate: false))
                return;

            CopyFile(sender);
        }
        #endregion

        #region DLボタン押下
        private void btnDL_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnDL);

            ClearStatusMessage();

            if (string.IsNullOrEmpty(txtClientPath.Text))
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "クライアント側のフォルダ");
                return;
            }

            if (!Directory.Exists(AppendPath(txtClientPath.Text)))
            {
                ShowWarningDialog(MsgWngNotExistFolder);
                return;
            }

            if (!ValidateFileExists(sender, duplicate: true))
                return;

            if (!ShowConfirmDialog(MsgQstConfirmTransferForSpecificFile))
                return;

            if (!ValidateFileExists(sender, duplicate: false))
                return;

            CopyFile(sender);
        }
        #endregion

        # region ファイルコピー
        private void CopyFile(object sender)
        {
            var fileName = string.Empty;
            var grid = sender.Equals(btnDL) ? grdServer : grdClient;
            var sourcePath = sender.Equals(btnDL) ? txtServerPath.Text : AppendPath(txtClientPath.Text);
            var destinationPath = sender.Equals(btnDL) ? AppendPath(txtClientPath.Text) : txtServerPath.Text;
            var msgParams = sender.Equals(btnDL) ? "ダウンロード" : "転送";

            try
            {
                for (var i = 0; i < grid.RowCount; i++)
                {
                    var check = Convert.ToBoolean(grid.Rows[i].Cells[0].EditedFormattedValue);

                    if (check)
                    {
                        fileName = grid.Rows[i].Cells[1].Value.ToString();
                        var fileInfo = new FileInfo(sourcePath + "\\" + fileName);

                        if (!IsFileinUse(fileInfo))
                            fileInfo.CopyTo(destinationPath + "\\" + fileName);
                        else
                            throw new IOException();
                    }
                }

                LoadLatestGrid();
                ShowWarningDialog(MsgInfFinishDataTransfer);
            }
            catch (UnauthorizedAccessException)
            {
                LoadLatestGrid();
                ShowWarningDialog(MsgErrAccessDeniedForSpecificFolder, destinationPath);
            }
            catch (IOException)
            {
                LoadLatestGrid();
                ShowWarningDialog(MsgErrAccessDeniedForSpecificFile, fileName, msgParams);
            }
            catch (Exception)
            {
                LoadLatestGrid();
                ShowWarningDialog(MsgErrSomethingError, msgParams);
            }
        }
        #endregion

        #region グリッドの最新表示
        private void LoadLatestGrid()
        {
            ClientFileList.Clear();
            ClientFileList = GetFiles(AppendPath(txtClientPath.Text));
            grdClient.DataSource = new BindingSource(ClientFileList, null);

            ServerFileList.Clear();
            ServerFileList = GetFiles(txtServerPath.Text);
            grdServer.DataSource = new BindingSource(ServerFileList, null);

            btnTransfer.Enabled = false;
            btnDL.Enabled = false;
        }
        #endregion

        #region チェックボックスの入力チェック
        private bool ValidateCheckBox(object sender)
        {
            var grid = sender.Equals(grdServer) ? grdServer : grdClient;

            for (var i = 0; i < grid.RowCount; i++)
            {
                var check = Convert.ToBoolean(grid.Rows[i].Cells[0].EditedFormattedValue);

                if (check)
                    return true;
            }

            return false;
        }
        #endregion

        #region 同名ファイルチェック・ファイル存在チェック
        private bool ValidateFileExists(object sender, bool duplicate)
        {
            var grid = sender.Equals(btnDL) ? grdServer : grdClient;
            var list = sender.Equals(btnDL) ? ClientFileList : ServerFileList;
            var path = sender.Equals(btnDL) ? txtServerPath.Text : AppendPath(txtClientPath.Text);
            var msgId = duplicate ? MsgWngSpecifiedFileAlreadyExists : MsgWngSpecifiedFileNotFound;

            for (var i = 0; i < grid.RowCount; i++)
            {
                var check = Convert.ToBoolean(grid.Rows[i].Cells[0].EditedFormattedValue);
                var fileName = grid.Rows[i].Cells[1].Value.ToString();
                var fileInfo = new FileInfo(path + "\\" + fileName);
                var fileExists = duplicate ? list.Exists(x => x.Text == fileName) : !fileInfo.Exists;

                if (check && fileExists)
                {
                    ShowWarningDialog(msgId, fileName);
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region プロセスで使用中チェック
        private bool IsFileinUse(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
        #endregion

        #region パスの加わり処理
        private string AppendPath(string path)
        {
            return Path.Combine(DefaultClient, path);
        }
        #endregion

        #region パスの切捨て処理
        private string OmitPath(string path)
        {
            return path.Replace(DefaultClient, string.Empty);
        }
        #endregion

        #region クライアント側ドライブ切替
        private void ChnageClientPathDrive()
        {
            var clientPath = txtClientPath.Text;
            if (string.IsNullOrEmpty(clientPath)) return;

            using (var dlg = CreateDialog<dlgDriveSelect>())
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.InitialDrive = Directory.GetDirectoryRoot(clientPath);
                DialogResult result = ApplicationContext.ShowDialog(ParentForm, dlg, true);
                if (this.Confirmed(result) != DialogResult.OK) return;

                clientPath = SystemInformation.TerminalServerSession ? DefaultClient + dlg.Drive + @"\" : dlg.Drive + @":\";
                ShowClientSide(clientPath);
            }
        }
        #endregion

        #region クライアント側表示
        private void ShowClientSide(string clientPath)
        {
            txtClientPath.Text = OmitPath(clientPath);
            ClientFileList.Clear();
            ClientFileList = GetFiles(clientPath);
            grdClient.DataSource = new BindingSource(ClientFileList, null);
            btnTransfer.Enabled = false;
            btnClientCheckAll.Enabled = ClientFileList.Count > 0;
            btnClientUncheckAll.Enabled = ClientFileList.Count > 0;
            BaseContext.SetFunction05Enabled(!string.IsNullOrEmpty(txtClientPath.Text));
        }
        #endregion

        #region ドライブ切替
        [OperationLog("ドライブ切替")]
        private void ChangeDrive()
        {
            ChnageClientPathDrive();
        }
        #endregion

        #region 終了処理
        [OperationLog("終了")]
        private void Exit()
        {
            Settings.SavePath<PH0401>(Login, AppendPath(txtClientPath.Text));
            BaseForm.Close();
        }
        #endregion

    }
}
