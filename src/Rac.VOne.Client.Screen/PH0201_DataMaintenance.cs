using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.SessionService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    public partial class PH0201 : VOneScreenBase
    {
        /// <summary>
        /// フォルダ選択ダイアログのデフォルトパス(デスクトップ)
        /// </summary>
        private static readonly string DefaultDirecoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// アプリケーションが現在使用しているデータベース接続文字列(Branch接続用)
        /// </summary>
        private string ConnectionString;

        #region 初期化

        public PH0201()
        {
            InitializeComponent();

            Text = "不要データ削除＆バックアップ";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("");
            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction10Caption("終了");

            OnF10ClickHandler = CloseWindow;

            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);
        }

        private void PH0201_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                loadTask.Add(Task.Run(async () =>
                {
                    ConnectionString = await GetConnectionInfoAsync(Login.SessionKey);
                }));
                ProgressDialog.Start(BaseForm, Task.WhenAll(loadTask), false, SessionKey);

                InitializeScreen();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeScreen()
        {
            ResetScreen();

            if (!CanAccessDatabase())
            {
                SetDBGroupBoxState(false);
            }
        }

        #endregion 初期化

        #region 定義

        /// <summary>
        /// 処理エラー情報
        /// </summary>
        private class FunctionError
        {
            public string MessageId { get; private set; }
            public string[] MessageArgs { get; private set; }

            public FunctionError(string messageId, params string[] messageArgs)
            {
                MessageId = messageId;
                MessageArgs = messageArgs;
            }
        }

        /// <summary>
        /// 入力値検証エラー情報
        /// </summary>
        private class ValidationError : FunctionError
        {
            public Control FocusTargetControl { get; private set; }

            public ValidationError(Control focusTargetControl, string messageId, params string[] messageArgs) : base(messageId, messageArgs)
            {
                FocusTargetControl = focusTargetControl;
            }
        }

        #endregion 定義

        #region 画面操作関連イベントハンドラ

        /// <summary>
        /// 画面を閉じる。
        /// </summary>
        [OperationLog("終了")]
        private void CloseWindow()
        {
            BaseForm.Close();
        }

        /// <summary>
        /// 「不要データを削除する」ボタン
        /// </summary>
        [OperationLog("削除")]
        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            try
            {
                var error = ValidateForDeletion();
                if (error != null)
                {
                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    if (error.FocusTargetControl != null)
                        error.FocusTargetControl.Focus();
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    return;
                }

                FunctionError deleteError = null;
                var deleteTask = Task.Run(() =>
                {
                    deleteError = DeleteDataBefore(datDeleteDate.Value.Value);
                });
                ProgressDialog.Start(BaseForm, deleteTask, false, SessionKey);

                if (deleteError != null)
                {
                    ShowWarningDialog(deleteError.MessageId, deleteError.MessageArgs);
                    return;
                }

                ShowWarningDialog(MsgInfDeleteUnnecessaryData);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrUnneededDataDeleteError);
                ex.Data["DeleteDate"] = datDeleteDate.Value;
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// 「DBをバックアップする」ボタン
        /// </summary>
        [OperationLog("DBのバックアップ")]
        private void btnBackupDB_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnBackupDB);

            ClearStatusMessage();

            try
            {
                var error = ValidateForBackup();
                if (error != null)
                {
                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    if (error.FocusTargetControl != null)
                        error.FocusTargetControl.Focus();
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmDBBackup))
                {
                    return;
                }

                FunctionError backupError = null;
                var backupTask = Task.Run(() =>
                {
                    backupError = BackupDatabase(txtDBBackupFilePath.Text);
                });
                ProgressDialog.Start(BaseForm, backupTask, false, SessionKey);

                if (backupError != null)
                {
                    ShowWarningDialog(backupError.MessageId, backupError.MessageArgs);
                    return;
                }

                ShowWarningDialog(MsgInfFinishDbBackup);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrDBBackupError);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// 「DBを復元する」ボタン
        /// </summary>
        [OperationLog("DBのリストア")]
        private void btnRestoreDB_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnRestoreDB);

            ClearStatusMessage();

            try
            {
                var error = ValidateForRestore();
                if (error != null)
                {
                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    if (error.FocusTargetControl != null)
                        error.FocusTargetControl.Focus();
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmDBRestore))
                {
                    return;
                }

                FunctionError restoreError = null;
                var restoreTask = Task.Run(() =>
                {
                    restoreError = RestoreDatabase(txtDBRestoreFilePath.Text);
                });
                ProgressDialog.Start(BaseForm, restoreTask, false, SessionKey);

                if (restoreError != null)
                {
                    ShowWarningDialog(restoreError.MessageId, restoreError.MessageArgs);
                    return;
                }

                ShowConfirmDialog(MsgInfFinishRestoreDb, "システムを終了します。");
                Application.Exit();
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrDBRestoreError);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// DBバックアップ欄のファイル選択ボタン
        /// </summary>
        private void btnSelectDBBackupFile_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var dir = GetExistingDirectoryPath(txtDBBackupFilePath.Text) ?? DefaultDirecoryPath;
            var filePath = string.Empty;
            var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.BAK";
            if (!ShowSaveFileDialog(dir, fileName, out filePath, "すべてのファイル(*.*)|*.*|BAKファイル(*.bak)|*.bak")) return;

            txtDBBackupFilePath.Text = filePath;
        }

        /// <summary>
        /// DBリストア欄のファイル選択ボタン
        /// </summary>
        private void btnSelectDBRestoreFile_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            var path = txtDBRestoreFilePath.Text;
            var fileNames = new List<string>();
            if (!LimitAccessFolder ?
                !ShowOpenFileDialog(path, out fileNames, filter : "すべてのファイル(*.*)|*.*|BAKファイル(*.bak)|*.bak", initialFileName: $"{DateTime.Now:yyyyMMdd_HHmmss}.BAK") :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out fileNames, FolderBrowserType.SelectFile)) return;

            txtDBRestoreFilePath.Text = fileNames?.FirstOrDefault() ?? string.Empty;

        }

        #endregion 画面操作関連イベントハンドラ

        #region Functions

        /// <summary>
        /// 画面を初期表示状態にリセットする。
        /// </summary>
        private void ResetScreen()
        {
            datDeleteDate.Value = null;
            txtDBBackupFilePath.Text = null;
            txtDBRestoreFilePath.Text = null;
            SetDBGroupBoxState(true);
            datDeleteDate.Focus();
        }

        /// <summary>
        /// 「DBのバックアップ」「DBのリストア」グループボックスの使用可否を制御する。
        /// </summary>
        /// <param name="isEnabled"></param>
        private void SetDBGroupBoxState(bool isEnabled)
        {
            lblBackupDB.Enabled = isEnabled;
            grpBackupDB.Enabled = isEnabled;
            lblRestoreDB.Enabled = isEnabled;
            grpRestoreDB.Enabled = isEnabled;
            if (isEnabled)
            {
                txtDBBackupFilePath.Enabled = !LimitAccessFolder;
                txtDBRestoreFilePath.Enabled = !LimitAccessFolder;
            }
        }

        /// <summary>
        /// 「DBのバックアップ」「DBのリストア」機能の使用可否を取得する。
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="machineName"></param>
        /// <returns></returns>
        private bool CanAccessDatabase()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Environment.MachineName))
            {
                return false;
            }

            try
            {
                var dataSource = new SqlConnectionStringBuilder(ConnectionString).DataSource.ToLower();

                var c1 = dataSource.StartsWith(Environment.MachineName.ToLower());
                var c2 = dataSource.StartsWith("(local)");
                var c3 = dataSource.StartsWith("localhost");
                var c4 = dataSource.StartsWith(".");

                return (c1 || c2 || c3 || c4);
            }
            catch (ArgumentException ex)
            {
#if DEBUG
                ex.Data["ConnectionString"] = ConnectionString;
#endif
                ex.Data["MachineName"] = Environment.MachineName;
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
        }

        #region 削除

        private ValidationError ValidateForDeletion()
        {
            if (datDeleteDate.Value == null)
            {
                return new ValidationError(datDeleteDate, MsgWngInputRequired, "削除対象日");
            }

            return null;
        }

        /// <summary>
        /// 以下の条件を満たす請求・入金データを削除する。<para/>
        /// 論理削除済み (DeleteAt IS NOT NULL)<para/>
        /// 未消込 (AssignmentFlag = 0)<para/>
        /// 入金予定日(Billing.DueAt)または入金日(Receipt.RecordedAt)が削除対象日以前
        /// </summary>
        /// <param name="date">削除対象日</param>
        /// <returns></returns>
        private FunctionError DeleteDataBefore(DateTime date)
        {
            var count = DeleteDataBeforeAsync(Login.SessionKey, date).Result;

            if (!count.HasValue)
            {
                return new FunctionError(MsgErrUnneededDataDeleteError);
            }
            if (count == 0)
            {
                return new FunctionError(MsgWngNoDeleteData);
            }

            return null;
        }

        #endregion 削除

        #region バックアップ／リストア

        private ValidationError ValidateForBackup()
        {
            if (string.IsNullOrWhiteSpace(txtDBBackupFilePath.Text))
            {
                return new ValidationError(txtDBBackupFilePath, MsgWngInputRequired, "バックアップするファイル名");
            }
            if (GetExistingDirectoryPath(txtDBBackupFilePath.Text) == null)
            {
                return new ValidationError(txtDBBackupFilePath, MsgWngNotExistFolder);
            }
            if (Path.GetExtension(txtDBBackupFilePath.Text).ToUpper() != ".BAK")
            {
                return new ValidationError(txtDBBackupFilePath, MsgWngNotExistUpdateData, "データベースファイル");
            }

            return null;
        }

        private FunctionError BackupDatabase(string backupFilePath)
        {
            var maintConnection = GetDataMaintenanceConnctionString(GetAppSettingsValue("IsBackUpBySqlUser", true));
            using (var conn = new SqlConnection(maintConnection.ConnectionString))
            {
                var database = new SqlConnectionStringBuilder(ConnectionString).InitialCatalog;

                try
                {
                    conn.Open();

                    var command = conn.CreateCommand();
                    command.CommandTimeout = 0;
                    command.CommandText = $@"
BACKUP DATABASE {database}
TO DISK = N'{backupFilePath}'
WITH
    NOFORMAT,
    INIT,
    NAME = N'{database}-完全データベース バックアップ',
    SKIP,
    NOREWIND,
    NOUNLOAD,
    STATS = 10
";
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    FunctionError error;

                    switch (ex.Number)
                    {
                        case 3101:  // データベース使用中
                            error = new FunctionError(MsgWngOtherAppliUsingDBandCannotBkOrRestore, "DBのバックアップ");
                            break;

                        case 3201:  // フォルダアクセス不可
                            error = new FunctionError(MsgErrUnauthorizedAccessTargetFolder);
                            break;

                        case 2:     // サーバ接続不可
                        case 942:   // データベースオフライン
                        case 4060:  // Open不可
                        case 18456: // ユーザ名またはパスワード
                            error = new FunctionError(MsgErrServerLoginError);
                            break;

                        case 262:   // 権限なし (BACKUP DATABASE)
                        default:
                            error = new FunctionError(MsgErrDBBackupError);
                            break;
                    }

                    ex.Data["Number"] = ex.Number;
                    ex.Data["txtDBBackupFilePath.Text"] = txtDBBackupFilePath.Text;
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                    return error;
                }
                catch (Exception ex)
                {
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                    return new FunctionError(MsgErrDBBackupError);
                }
                finally
                {
                    conn.Close();
                }

                return null;
            }
        }

        private ValidationError ValidateForRestore()
        {
            if (string.IsNullOrWhiteSpace(txtDBRestoreFilePath.Text))
            {
                return new ValidationError(txtDBRestoreFilePath, MsgWngInputRequired, "復元するファイル名");
            }
            if (GetExistingDirectoryPath(txtDBRestoreFilePath.Text) == null)
            {
                return new ValidationError(txtDBRestoreFilePath, MsgWngNotExistFolder);
            }
            if (Path.GetExtension(txtDBRestoreFilePath.Text).ToUpper() != ".BAK")
            {
                return new ValidationError(txtDBRestoreFilePath, MsgWngNotExistUpdateData, "データベースファイル");
            }

            return null;
        }

        private FunctionError RestoreDatabase(string backupFilePath)
        {
            var maintConnection = GetDataMaintenanceConnctionString(GetAppSettingsValue("IsRestoreBySqlUser", true));
            using (var conn = new SqlConnection(maintConnection.ConnectionString))
            {
                var database = new SqlConnectionStringBuilder(ConnectionString).InitialCatalog;

                try
                {
                    conn.Open();

                    var command = conn.CreateCommand();
                    command.CommandTimeout = 0;
                    command.CommandText = $@"
ALTER DATABASE {database} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE {database}
FROM DISK = N'{backupFilePath}'
WITH
    FILE = 1,
    UNLOAD,
    REPLACE,
    STATS = 10;
ALTER DATABASE {database} SET MULTI_USER;
";
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    FunctionError error;

                    switch (ex.Number)
                    {
                        case 3101:  // データベース使用中
                            error = new FunctionError(MsgWngOtherAppliUsingDBandCannotBkOrRestore, "DBのリストア");
                            break;

                        case 3201:  // フォルダアクセス不可
                            error = new FunctionError(MsgErrUnauthorizedAccessTargetFolder);
                            break;

                        case 2:     // サーバ接続不可
                        case 942:   // データベースオフライン
                        case 4060:  // Open不可
                        case 18456: // ユーザ名またはパスワード
                            error = new FunctionError(MsgErrServerLoginError);
                            break;

                        case 262:   // 権限なし (ALTER DATABASE / RESTORE DATABASE)
                        case 5058:  // 設定不可 (SINGLE_USER)
                        default:
                            error = new FunctionError(MsgErrDBRestoreError);
                            break;
                    }

                    ex.Data["Number"] = ex.Number;
                    ex.Data["txtDBRestoreFilePath.Text"] = txtDBRestoreFilePath.Text;
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                    return error;
                }
                catch (Exception ex)
                {
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                    return new FunctionError(MsgErrDBRestoreError);
                }
                finally
                {
                    conn.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// App.config の appSettings セクションから設定値を取得する。bool値専用。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static bool GetAppSettingsValue(string key, bool defaultValue)
        {
            var valueString = System.Configuration.ConfigurationManager.AppSettings[key];

            bool result;
            if (valueString == null || !bool.TryParse(valueString, out result))
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// バックアップ／リストア実行用のデータベース接続文字列を取得する。
        /// </summary>
        /// <param name="maintenanceBySqlUser">SQL Server認証を使う場合はtrueを指定、Windows認証を使う場合はfalseを指定する。</param>
        /// <returns></returns>
        private SqlConnectionStringBuilder GetDataMaintenanceConnctionString(bool maintenanceBySqlUser)
        {
            var app = new SqlConnectionStringBuilder(ConnectionString);
            var maint = new SqlConnectionStringBuilder();

            maint.DataSource = app.DataSource;
            maint.InitialCatalog = "master";
            if (maintenanceBySqlUser)
            {
                maint.UserID = app.UserID;
                maint.Password = app.Password;
            }
            else
            {
                maint.IntegratedSecurity = true;
            }

            return maint;
        }

        #endregion バックアップ／リストア

        #endregion Functions

        #region Web Service

        /// <summary>
        /// 現在使用している接続文字列を取得する。
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns>string: 接続文字列，null: エラー</returns>
        private static async Task<string> GetConnectionInfoAsync(string sessionKey)
        {
            ConnectionInfoResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<SessionService.SessionServiceClient>();
                result = await client.GetConnectionInfoAsync(sessionKey);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.ConnectionInfo;
        }

        /// <summary>
        /// 指定日以前の不要な請求・入金データを削除する。<para/>
        /// </summary>
        /// <param name="date">削除対象日</param>
        /// <returns>int: 削除件数，null: エラー</returns>
        private static async Task<int?> DeleteDataBeforeAsync(string SessionKey, DateTime date)
        {
            CountResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<DataMaintenanceService.DataMaintenanceServiceClient>();
                result = await client.DeleteDataAsync(SessionKey, date);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.Count;
        }

        #endregion Web Service

        #region Helper

        /// <summary>
        /// ディレクトリの存在をチェックし、見つかった場合はそのパスを取得する。
        /// 見つからない場合はファイルパスが指定されたものと見なし、直上ディレクトリの存在をチェックする。
        /// それでも見つからない場合はnullを返す。
        /// </summary>
        /// <param name="path">ディレクトリパス または ファイルパス</param>
        /// <returns>string: ディレクトリパス(存在時)，null: 不在</returns>
        private static string GetExistingDirectoryPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            path = path.Trim();
            if (path.EndsWith(":")) // "C:"などでtrueになってしまうので弾く
            {
                return null;
            }

            if (Directory.Exists(path)) // path == 既存のディレクトリパス
            {
                return path;
            }

            if (path.Last() == Path.DirectorySeparatorChar || path.Last() == Path.AltDirectorySeparatorChar) // '\' や '/' で終わっている場合、pathはファイルパスではない
            {
                return null;
            }

            path = Path.GetDirectoryName(path);

            return Directory.Exists(path) ? path : null;
        }

        #endregion Helper
    }
}
