using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.ApplicationControlMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.LogDataService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>操作ログ管理</summary>
    public partial class PH0901 : VOneScreenBase
    {
        //private readonly List<LogData> LogDataList = new List<LogData>();

        public PH0901()
        {
            Text = "操作ログ管理";
            InitializeComponent();
            grdLogData.SetupShortcutKeys();
        }

        #region 画面の初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = ExportData;

            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }

        private void InitializeLogDataGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 135, nameof(LogData.LoggedAt)     , dataField: nameof(LogData.LoggedAt)     , sortable: true, caption: "日時"          , cell: builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height, 120, nameof(LogData.LoginUserCode), dataField: nameof(LogData.LoginUserCode), sortable: true, caption: "ユーザーコード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 140, nameof(LogData.LoginUserName), dataField: nameof(LogData.LoginUserName), sortable: true, caption: "ユーザー名"),
                new CellSetting(height, 200, nameof(LogData.MenuName)     , dataField: nameof(LogData.MenuName)     , sortable: true, caption: "名称"),
                new CellSetting(height, 150, nameof(LogData.OperationName), dataField: nameof(LogData.OperationName), sortable: true, caption: "操作")
            });

            grdLogData.Template = builder.Build();
            grdLogData.SetRowColor(ColorContext);
        }
        #endregion

        #region Webサービス呼び出し

        private void GetLogCount()
        {
            var task = ServiceProxyFactory.DoAsync<LogDataServiceClient>(async client =>
            {
                var logData = await client.GetStatsAsync(SessionKey, CompanyId);
                if (logData.ProcessResult.Result)
                {
                    if (logData.Datas[0].LogCount == 0)
                    {
                        lblLoggedAt.Text = "ログ情報なし";
                        lblLoggedCounts.Text = "0";
                    }
                    else
                    {
                        lblLoggedAt.Text = logData.Datas[0].FirstLoggedAt.ToString();
                        lblLoggedCounts.Text = logData.Datas[0].LogCount.ToString("#,##0");
                    }
                }
            });
        }

        #endregion

        #region Search Dialog Setting
        private void btnLoginUser_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                txtLoginUserCode.Text = loginUser.Code;
                lblLoginUserName.Text = loginUser.Name;
            }
        }

        private void txtLoginUserCode_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoginUserCode.Text))
            {
                lblLoginUserName.Clear();
                ClearStatusMessage();
                return;
            }

            try
            {
                LoginUser userResult = null;
                var task = ServiceProxyFactory.DoAsync<LoginUserMasterClient>(async client =>
                {
                    var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtLoginUserCode.Text });
                    if (result.ProcessResult.Result)
                    {
                        userResult = result.Users.FirstOrDefault();
                    }

                    if (userResult != null)
                    {
                        txtLoginUserCode.Text = userResult.Code;
                        lblLoginUserName.Text = userResult.Name;
                        ClearStatusMessage();
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (userResult == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", txtLoginUserCode.Text);
                    txtLoginUserCode.Clear();
                    lblLoginUserName.Clear();
                    txtLoginUserCode.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region Function Key

        private void Search()
        {
            try
            {
                if (ApplicationControl?.UseOperationLogging == 1)
                    SaveLogData("F1:検索", ParentForm.Text);

                ClearStatusMessage();

                if (!CheckCondition()) return;

                var loadListTask = LoadGrid();
                loadListTask.ContinueWith(task =>
                {
                    var log = task.Result;
                    if (log.Any())
                    {
                        grdLogData.DataSource = new BindingSource(log, null);
                        BaseContext.SetFunction06Enabled(true);
                    }
                    else
                    {
                        grdLogData.DataSource = null;
                        ShowWarningDialog(MsgWngNotExistSearchData, "");
                        BaseContext.SetFunction06Enabled(false);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadListTask), false, SessionKey);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return;
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            try
            {
                txtLoginUserCode.Clear();
                lblLoginUserName.Clear();
                datLoggedAtFrom.Clear();
                datLoggedAtTo.Clear();
                datLoggedAtFrom.Select();
                grdLogData.DataSource = null;
                BaseContext.SetFunction06Enabled(false);
                ClearStatusMessage();
                GetLogCount();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

        }

        private void Delete()
        {
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                var task = ServiceProxyFactory.DoAsync<LogDataServiceClient>(async client =>
                {
                    var deleteResult = await client.DeleteAllAsync(SessionKey, CompanyId);
                    if (deleteResult.Count == 1)
                    {
                        Clear();
                        DispStatusMessage(MsgInfDeleteSuccess);

                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                GetLogCount();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ExportData()
        {
            try
            {
                if (ApplicationControl?.UseOperationLogging == 1)
                    SaveLogData("F6:エクスポート", ParentForm.Text);

                var task = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var serverPath = task.Result;

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"操作ログ{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var loadListTask = LoadGrid();
                ProgressDialog.Start(ParentForm, loadListTask, false, SessionKey);
                var LogDataList = loadListTask.Result;

                var definition = new LogDataFileDefinition(new DataExpression(ApplicationControl));
                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, LogDataList, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                DispStatusMessage(MsgInfFinishExport);
                Settings.SavePath<Web.Models.LoginUser>(Login, filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        private void Exit()
        {
            if (ApplicationControl?.UseOperationLogging == 1)
                SaveLogData("F10:終了", ParentForm.Text);

            BaseForm.Close();
        }
        #endregion

        #region その他Function
        private void PH0901_Load(object sender, EventArgs e)
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

                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                GetLogCount();
                InitializeLogDataGrid();
                SetOnFormLoad();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool CheckCondition()
        {
            if (!datLoggedAtFrom.ValidateRange(datLoggedAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblLogged.Text))) return false;
            return true;
        }

        private void SetOnFormLoad()
        {
            var useOperationLogging = ApplicationControl?.UseOperationLogging ?? 0;
            var expression = new DataExpression(ApplicationControl);

            if (useOperationLogging == 1)
            {
                lblLoggedSwitch.Text = "する";
            }
            else
            {
                lblLoggedSwitch.Text = "しない";
            }

            txtLoginUserCode.Format = expression.LoginUserCodeFormatString;

            if (expression.LoginUserCodeFormatString == "9")
            {
                txtLoginUserCode.PaddingChar = '0';
            }
            txtLoginUserCode.MaxLength = expression.LoginUserCodeLength;
        }

        private async Task<List<LogData>> LoadGrid()
        {
            List<LogData> logs = null;
            DateTime? dateTo = null;
            string loginCode = null;

            if (datLoggedAtTo.Value.HasValue)
            {
                dateTo = datLoggedAtTo.Value.Value.AddDays(1).AddMilliseconds(-1);
            }

            if (txtLoginUserCode.Text != "")
            {
                loginCode = txtLoginUserCode.Text;
            }

            await ServiceProxyFactory.DoAsync<LogDataServiceClient>(async client =>
            {
                var logData = await client.GetItemsAsync(SessionKey, CompanyId, datLoggedAtFrom.Value, dateTo, loginCode);
                if (logData.ProcessResult.Result && logData.LogData.Any())
                {
                    logs = logData.LogData;
                }
            });

            return logs ?? new List<LogData>();
        }

        private void btnLoggedSwitch_Click(object sender, EventArgs e)
        {
            var operationName = "";
            var userOpertionLogging = 0;
            try
            {
                if (lblLoggedSwitch.Text == "しない")
                {
                    lblLoggedSwitch.Text = "する";
                    operationName = "開始";
                    userOpertionLogging = 1;
                }
                else
                {
                    lblLoggedSwitch.Text = "しない";
                    operationName = "終了";
                    userOpertionLogging = 0;
                }

                UpdateApplicationData(userOpertionLogging, operationName);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void UpdateApplicationData(int useOperation, string operationName)
        {
            ApplicationControl.UseOperationLogging = useOperation;

            var task = ServiceProxyFactory.DoAsync<ApplicationControlMasterClient>(async client =>
            {
                var result = await client.SaveAsync(SessionKey, ApplicationControl);
                if (result.ProcessResult.Result)
                {
                    SaveLogData(operationName, "ログ切替");
                    GetLogCount();
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
            });
        }

        private void SaveLogData(string operationName, string menuName)
        {
            var logData = new LogData();
            logData.CompanyId = CompanyId;
            logData.ClientName = Util.GetRemoteHostName();
            logData.LoginUserCode = Login.UserCode;
            logData.LoginUserName = Login.UserName;
            logData.MenuId = null;
            logData.MenuName = menuName;
            logData.OperationName = operationName;
            var task = ServiceProxyFactory.DoAsync<LogDataServiceClient>(async client
                => await client.LogAsync(SessionKey, logData));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }

        #endregion
    }
}
