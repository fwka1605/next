using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen
{
    using Common.Settings;
    using static Message.Constants;

    /// <summary>
    /// タイムスケジューラー・ログ管理
    /// </summary>
    public partial class PH0502 : VOneScreenBase
    {
        private const string EditorPath = "notepad.exe";

        #region 初期化

        public PH0502()
        {
            InitializeComponent();

            grdTaskScheduleHistory.SetupShortcutKeys();
            Text = "タイムスケジューラー・ログ管理";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("出力");
            BaseContext.SetFunction04Caption("削除");
            BaseContext.SetFunction10Caption("終了");

            OnF01ClickHandler = SearchHistory;
            OnF02ClickHandler = ClearScreen;
            OnF03ClickHandler = ExportHistory;
            OnF04ClickHandler = DeleteAllHistory;
            OnF10ClickHandler = CloseWindow;

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);
        }

        private void PH0502_Load(object sender, EventArgs e)
        {
            try
            {
                var loadTask = new List<Task>();
                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());
                loadTask.Add(LoadControlColorAsync());

                ProgressDialog.Start(BaseForm, Task.WhenAll(loadTask), false, SessionKey);

                SetScreenName();
                DispValueDictionaries = new PH0501.DisplayValueDictionaries(SessionKey, CompanyId);
                InitializeComboBox();
                InitializeScreen();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #region 初期化(コンボボックス)

        private void InitializeComboBox()
        {
            cmbImportType.Items.Clear();
            cmbImportType.Items.AddRange(
                DispValueDictionaries.ImportType.Where(x => { if (x.Key == 5) return UseScheduledPayment; return true; })
                                                .Select(disp => new ListItem(disp.Value, disp.Key)).ToArray());
            cmbImportType.TextSubItemIndex = 0;
            cmbImportType.ValueSubItemIndex = 1;

            cmbImportSubType.Items.Clear();
            cmbImportSubType.TextSubItemIndex = 0;
            cmbImportSubType.ValueSubItemIndex = 1;

            cmbResult.Items.Clear();
            cmbResult.Items.AddRange(
                DispValueDictionaries.Result.Select(disp => new ListItem(disp.Value, disp.Key)).ToArray());
            cmbResult.TextSubItemIndex = 0;
            cmbResult.ValueSubItemIndex = 1;
        }

        #endregion 初期化(コンボボックス)

        /// <summary>
        /// 画面をクリアする。
        /// </summary>
        private void InitializeScreen()
        {
            InitializeSearchCondition();
            InitializeGrid();

            cmbImportType.Select();
        }

        /// <summary>
        /// 検索条件を初期化する。
        /// </summary>
        private void InitializeSearchCondition()
        {
            cmbImportType.SelectedValue = -1;
            cmbResult.SelectedValue = -1;
            txtStartAt_From.Value = null;
            txtStartAt_To.Value = null;
            txtEndAt_From.Value = null;
            txtEndAt_To.Value = null;
        }

        #region 初期化(グリッド)

        private void InitializeGrid()
        {
            grdTaskScheduleHistory.Template = InitializeGridTemplate();
            grdTaskScheduleHistory.DataSource = null;
        }

        private Template InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                              , cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 140, "StartAt"      , caption: "開始日時"  , cell: builder.GetDateCell_yyyyMMddHHmmss()                         , dataField: nameof(ViewData.StartAt)                  ),
                new CellSetting(height, 140, "EndAt"        , caption: "終了日時"  , cell: builder.GetDateCell_yyyyMMddHHmmss()                         , dataField: nameof(ViewData.EndAt)                    ),
                new CellSetting(height, 170, "ImportType"   , caption: "種別"      , cell: builder.GetTextBoxCell()                                     , dataField: nameof(ViewData.ImportTypeDisplayValue)   ),
                new CellSetting(height, 220, "ImportSubType", caption: "パターン"  , cell: builder.GetTextBoxCell()                                     , dataField: nameof(ViewData.ImportSubTypeDisplayValue)),
                new CellSetting(height,  70, "Result"       , caption: "実行結果"  , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(ViewData.ResultDisplayValue)       ),
                new CellSetting(height, 350, "Errors"       , caption: "エラーログ", cell: builder.GetTextBoxCell()                                     , dataField: nameof(ViewData.Errors)                   ),
            });

            return builder.Build();
        }

        #endregion 初期化(グリッド関連)

        #region 初期化(表示値切り替え用辞書)

        /// <summary>
        /// 表示値切り替え用辞書
        /// </summary>
        private PH0501.DisplayValueDictionaries DispValueDictionaries;

        #endregion 初期化(表示値切り替え用の辞書)

        #endregion 初期化

        #region 画面表示データクラス関連

        /// <summary>
        /// 画面表示データクラス
        /// </summary>
        private class ViewData
        {
            public readonly TaskScheduleHistory Model;

            public readonly PH0501.DisplayValueDictionaries DisplayValueDictionaries;

            public ViewData(TaskScheduleHistory taskScheduleModel, PH0501.DisplayValueDictionaries dispValue)
            {
                Model = taskScheduleModel;
                DisplayValueDictionaries = dispValue;
            }

            public long Id
            {
                get { return Model.Id; }
                set { Model.Id = value; }
            }

            public int CompanyId
            {
                get { return Model.CompanyId; }
                set { Model.CompanyId = value; }
            }

            public DateTime StartAt
            {
                get { return Model.StartAt; }
                set { Model.StartAt = value; }
            }

            public DateTime EndAt
            {
                get { return Model.EndAt; }
                set { Model.EndAt = value; }
            }

            public int ImportType
            {
                get { return Model.ImportType; }
                set { Model.ImportType = value; }
            }

            public string ImportTypeDisplayValue
            {
                get
                {
                    if (!DisplayValueDictionaries.ImportType.Keys.Contains(ImportType))
                    {
                        return "";
                    }

                    return DisplayValueDictionaries.ImportType[ImportType];
                }
            }

            public int ImportSubType
            {
                get { return Model.ImportSubType; }
                set { Model.ImportSubType = value; }
            }

            public string ImportSubTypeDisplayValue
            {
                get
                {
                    var c1 = !DisplayValueDictionaries.ImportTypeToImportSubType.Keys.Contains(ImportType);
                    var c2 = !DisplayValueDictionaries.ImportTypeToImportSubType[ImportType].Keys.Contains(ImportSubType);
                    if (c1 || c2)
                    {
                        return "";
                    }

                    return DisplayValueDictionaries.ImportTypeToImportSubType[ImportType][ImportSubType];
                }
            }

            public int Result
            {
                get { return Model.Result; }
                set { Model.Result = value; }
            }

            public string ResultDisplayValue
            {
                get
                {
                    if (!DisplayValueDictionaries.Result.Keys.Contains(Model.Result))
                    {
                        return "";
                    }

                    return DisplayValueDictionaries.Result[Model.Result];
                }
            }

            public string Errors
            {
                get { return Model.Errors; }
                set { Model.Errors = value; }
            }
        }

        #endregion 画面表示データクラス関連

        #region 画面操作関連イベントハンドラ

        /// <summary>
        /// 検索する。
        /// </summary>
        [OperationLog("検索")]
        private void SearchHistory()
        {
            ClearStatusMessage();

            try
            {
                SearchTaskScheduleHistory();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// 画面をクリアする。
        /// </summary>
        [OperationLog("クリア")]
        private void ClearScreen()
        {
            ClearStatusMessage();

            try
            {
                InitializeScreen();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// グリッドに表示されているログをCSVファイルにエクスポートする。
        /// </summary>
        [OperationLog("出力")]
        private void ExportHistory()
        {
            ClearStatusMessage();

            try
            {
                ExportPresentHistory();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrSomethingError, "出力");
            }
        }

        /// <summary>
        /// 全てのログを削除する。
        /// </summary>
        [OperationLog("削除")]
        private void DeleteAllHistory()
        {
            ClearStatusMessage();

            try
            {
                DeleteAllTaskScheduleHistory();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// 画面を閉じる。
        /// </summary>
        [OperationLog("終了")]
        private void CloseWindow()
        {
            BaseForm.Close();
        }

        /// <summary>
        /// 「種別」コンボボックス 選択変更<para/>
        /// 選択値に応じて「取込パターン」コンボボックスを制御する。
        /// </summary>
        private void cmbImportType_SelectedValueChanged(object sender, EventArgs e)
        {
            var cmb = (VOneComboControl)sender;
            var importType = (int)(cmb.SelectedValue ?? -1);

            SuspendLayout();

            cmbImportSubType.Items.Clear();
            cmbImportSubType.Items.Add(new ListItem("すべて", -1));

            if (0 <= importType)
            {
                cmbImportSubType.Items.AddRange(DispValueDictionaries
                    .ImportTypeToImportSubType[importType]
                    .Select(disp => new ListItem(disp.Value, disp.Key))
                    .ToArray());
            }

            cmbImportSubType.SelectedValue = -1;
            cmbImportSubType.Enabled = (importType != -1); // 種別が「すべて」の場合は取引パターンを無効化 ※「すべて」で固定

            ResumeLayout();
        }

        /// <summary>
        /// グリッドのセルダブルクリック
        /// 「エラーログ」欄に有効なファイルパスが表示されている場合はNotepadで開く。
        /// </summary>
        private void grdTaskScheduleHistory_CellDoubleClick(object sender, CellEventArgs e)
        {
            var grid = (VOneGridControl)sender;
            if (grid.CurrentCell == null || grid.CurrentCell.RowIndex < 0)
            {
                return;
            }

            var viewData = (ViewData)grid.CurrentRow.DataBoundItem;

            var logPath = viewData.Errors;
            if (!string.IsNullOrWhiteSpace(logPath)) // ｢エラーログ｣欄に、目に見える何かが表示されている場合 ※ 実行結果(成功／失敗)は見ない
            {
                if (!File.Exists(logPath))
                {
                    ShowWarningDialog(MsgWngOpenFileNotFound);
                    return;
                }

                Process.Start(EditorPath, logPath);
            }
        }

        private void grdTaskScheduleHistory_DataSourceChanged(object sender, EventArgs e)
        {
            var grid = (VOneGridControl)sender;
            var viewDataList = (List<ViewData>)grid.DataSource;

            // 「出力」ボタン制御
            BaseContext.SetFunction03Enabled(viewDataList != null && viewDataList.Count != 0);
        }

        #endregion 画面操作関連イベントハンドラ

        #region Functions

        #region 検索

        /// <summary>
        /// スケジュールデータを検索し、グリッドに表示する。
        /// </summary>
        private void SearchTaskScheduleHistory()
        {
            var importType = (int?)cmbImportType.SelectedValue;
            var importSubType = (int?)cmbImportSubType.SelectedValue;
            var result = (int?)cmbResult.SelectedValue;

            var searchCondition = new TaskScheduleHistorySearch
            {
                CompanyId = CompanyId,

                // 以下、条件無指定時はnull値
                ImportType = (importType != -1) ? importType : null,
                ImportSubType = (importSubType != -1) ? importSubType : null,
                Result = (result != -1) ? result : null,
                StartAt_From = txtStartAt_From.Value,
                StartAt_To = txtStartAt_To.Value,
                EndAt_From = txtEndAt_From.Value,
                EndAt_To = txtEndAt_To.Value,
            };

            if (!ValidateSearchConditions(searchCondition))
            {
                return;
            }

            List<TaskScheduleHistory> taskScheduleHistory = null;
            var task = Task.Run(async () =>
            {
                taskScheduleHistory = await SearchTaskScheduleHistoryAsync(SessionKey, searchCondition);
            });
            ProgressDialog.Start(BaseForm, task, false, SessionKey);

            if (taskScheduleHistory == null)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                grdTaskScheduleHistory.DataSource = null;
                return;
            }

            var viewDataList = taskScheduleHistory.Select(row => new ViewData(row, DispValueDictionaries))
                .OrderBy(data => data.StartAt)
                .ToList();

            grdTaskScheduleHistory.DataSource = viewDataList;

            if (viewDataList.Count == 0)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
        }

        private bool ValidateSearchConditions(TaskScheduleHistorySearch search)
        {
            if (search.StartAt_From.HasValue && search.StartAt_To.HasValue)
            {
                if (search.StartAt_To < search.StartAt_From)
                {
                    ShowWarningDialog(MsgWngInputRangeChecked, "開始日時");
                    txtStartAt_From.Focus();
                    return false;
                }
            }
            if (search.EndAt_From.HasValue && search.EndAt_To.HasValue)
            {
                if (search.EndAt_To < search.EndAt_From)
                {
                    ShowWarningDialog(MsgWngInputRangeChecked, "終了日時");
                    txtEndAt_From.Focus();
                    return false;
                }
            }

            return true;
        }

        #endregion 検索

        #region 出力

        /// <summary>
        /// グリッドに表示されているログをCSVファイルにエクスポートする。
        /// </summary>
        private void ExportPresentHistory()
        {
            var filePath = GetExportFilePath();
            if (string.IsNullOrEmpty(filePath)
                || !ShowConfirmDialog(MsgQstConfirmOutputExtractData))
            {
                ShowWarningDialog(MsgInfCancelProcess, "出力");
                return;
            }

            var definition = new TaskScheduleHistoryFileDefinition(new DataExpression(ApplicationControl));
            definition.ImportTypeToImportSubType = DispValueDictionaries.ImportTypeToImportSubType;
            definition.ImportType = DispValueDictionaries.ImportType;
            definition.ImportSubType0 = DispValueDictionaries.ImportSubType0;
            definition.ImportSubType1 = DispValueDictionaries.ImportSubType1;
            definition.ImportSubType2 = DispValueDictionaries.ImportSubType2;
            definition.ImportSubType3 = DispValueDictionaries.ImportSubType3;
            definition.ImportSubType4 = DispValueDictionaries.ImportSubType4;
            definition.ImportSubType5 = DispValueDictionaries.ImportSubType5;
            definition.Result = DispValueDictionaries.Result;

            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            var list = ((List<ViewData>)grdTaskScheduleHistory.DataSource).Select(d => d.Model).ToList();

            ProgressDialog.Start(ParentForm, (cancel, progress) =>
            {
                return exporter.ExportAsync(filePath, list, cancel, progress);
            }, true, SessionKey);

            if (exporter.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                ShowWarningDialog(MsgErrExportError);
                return;
            }

            DispStatusMessage(MsgInfFinishDataExtracting);
            Settings.SavePath<TaskScheduleHistory>(Login, filePath);
        }

        private string GetExportFilePath()
        {
            var serverPath = string.Empty;
            var task = Task.Run(async () => serverPath = await Util.GetGeneralSettingServerPathAsync(Login));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            serverPath = Util.GetDirectoryName(serverPath);

            var fileName = $"タイムスケジューラーログ{DateTime.Now:yyyyMMddHHmmss}.csv";
            var filePath = string.Empty;
            if (ShowSaveFileDialog(serverPath, fileName, out filePath)) return filePath;
            return string.Empty;
        }

        #endregion 出力

        #region 削除

        /// <summary>
        /// 全てのログを削除する。
        /// </summary>
        private void DeleteAllTaskScheduleHistory()
        {
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                return;
            }

            int? count = null;
            var task = Task.Run(async () =>
            {
                count = await DeleteAllTaskScheduleHistoryAsync(SessionKey, CompanyId);
            });
            ProgressDialog.Start(BaseForm, task, false, SessionKey);

            if (!count.HasValue)
            {
                ShowWarningDialog(MsgErrDeleteError);
                return;
            }

            grdTaskScheduleHistory.DataSource = null;
            ShowWarningDialog(MsgInfDeleteSuccess);
        }

        #endregion 削除

        #endregion Functions

        #region Web Service

        /// <summary>
        /// ログ検索処理(TaskScheduleHistoryService.svc:GetItems)を呼び出して結果を取得する。
        /// </summary>
        private static async Task<List<TaskScheduleHistory>> SearchTaskScheduleHistoryAsync(string sessionKey, TaskScheduleHistorySearch searchConditions)
        {
            TaskScheduleHistoryResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<TaskScheduleHistoryService.TaskScheduleHistoryServiceClient>();
                result = await client.GetItemsAsync(sessionKey, searchConditions);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.TaskScheduleHistoryList;
        }

        /// <summary>
        /// 全ログ検索処理(TaskScheduleHistoryService.svc:Delete)を呼び出して結果を取得する。
        /// </summary>
        private static async Task<int?> DeleteAllTaskScheduleHistoryAsync(string sessionKey, int companyId)
        {
            CountResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<TaskScheduleHistoryService.TaskScheduleHistoryServiceClient>();
                result = await client.DeleteAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.Count;
        }

        #endregion Web Service
    }
}
