using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Microsoft.Win32.TaskScheduler; // Task Scheduler Managed Wrapper
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    using Task = System.Threading.Tasks.Task; // Task Scheduler Managed Wrapper の Task と衝突する

    public partial class PH0501 : VOneScreenBase
    {
        /// <summary>
        /// Windowsタスクスケジューラのタスク登録先フォルダ名
        /// </summary>
        public string TaskSchedulerFolder { get; set; }

        /// <summary>
        /// インポーターEXEファイル名
        /// </summary>
        public const string ImporterExecutableName = "VOneG4Importer.exe";

        /// <summary>
        /// フォルダ選択ダイアログのデフォルトパス
        /// </summary>
        private static readonly string FolderBrowserDefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private bool IsModified { get; set; }

        private bool isCloudEdition {get; set;}

        #region 初期化

        static PH0501()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal); // 権限確認処理の準備
        }

        public PH0501()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            grdTaskList.SetupShortcutKeys();
            Text = "タイムスケジューラー";

            isCloudEdition = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]);
            TaskSchedulerFolder = (isCloudEdition) ? CloudApplicationName : StandardApplicationName;
        }

        private void InitializeHandlers()
        {
            var changed = new System.Action(() => IsModified = true);
            cmbImportType.SelectedIndexChanged          += (sender, e) => changed();
            cmbImportSubType.SelectedIndexChanged       += (sender, e) => changed();
            cmbLogDestination.SelectedIndexChanged      += (sender, e) => changed();
            txtInterval.ValueChanged                    += (sender, e) => changed();
            txtStartDate.ValueChanged                   += (sender, e) => changed();
            txtImportDirectory.TextChanged              += (sender, e) => changed();
            txtSuccessDirectory.TextChanged             += (sender, e) => changed();
            txtFailedDirectory.TextChanged              += (sender, e) => changed();
            rdoBillingAmount0.CheckedChanged            += (sender, e) => changed();
            rdoDuration0.CheckedChanged                 += (sender, e) => changed();
            rdoTargetBillingAssignment0.CheckedChanged  += (sender, e) => changed();
            rdoUpdateSameCustomer0.CheckedChanged       += (sender, e) => changed();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction08Caption("ログ閲覧");
            BaseContext.SetFunction10Caption("終了");

            OnF01ClickHandler = RegisterTaskSchedule;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = DeleteTaskSchedules;
            OnF08ClickHandler = ShowTaskSchedulerLogScreen;
            OnF10ClickHandler = CloseWindow;

            var isAdmin = IsUserLocalMachineAdministrator();

            BaseContext.SetFunction01Enabled(isAdmin);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(isAdmin);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);
        }

        private void PH0501_Load(object sender, EventArgs e)
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
                DispValueDictionaries = new DisplayValueDictionaries(SessionKey, CompanyId);

                ClearEditForm();
                InitializeComboBox();
                cmbImportType_SelectedIndexChanged(cmbImportType, null); // 初期表示時にSelectedIndexChangedが発火しないため

                InitializeGrid();
                LoadGridData();

                CheckPermissions();
                IsModified = false;
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
            cmbImportType.SelectedIndex = -1;

            cmbImportSubType.Items.Clear();
            cmbImportSubType.TextSubItemIndex = 0;
            cmbImportSubType.ValueSubItemIndex = 1;
            cmbImportSubType.SelectedIndex = -1;

            cmbLogDestination.Items.Clear();
            cmbLogDestination.Items.AddRange(
                DispValueDictionaries.LogDestination.Select(disp => new ListItem(disp.Value, disp.Key)).ToArray());
            cmbLogDestination.TextSubItemIndex = 0;
            cmbLogDestination.ValueSubItemIndex = 1;
            cmbLogDestination.SelectedIndex = (!LimitAccessFolder) ? -1 : 0;
        }

        #endregion 初期化(コンボボックス)

        #region 初期化(グリッド)

        private void InitializeGrid()
        {
            grdTaskList.Template = InitializeGridTemplate();
        }

        private Template InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"   , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,   0, "Id"              , dataField: nameof(ViewData.Id), visible: false),
                new CellSetting(height,  50, "IsEnabled"       , dataField: nameof(ViewData.Enabled)                   , caption: "実行"          , cell: builder.GetCheckBoxCell(), readOnly: false),
                new CellSetting(height, 170, "ImportType"      , dataField: nameof(ViewData.ImportTypeDisplayValue)    , caption: "種別"          , cell: builder.GetTextBoxCell()),
                new CellSetting(height, 220, "ImportSubType"   , dataField: nameof(ViewData.ImportSubTypeDisplayValue) , caption: "パターン"      , cell: builder.GetTextBoxCell()),
                new CellSetting(height,  50, "Trigger"         , dataField: nameof(ViewData.DurationDisplayValue)      , caption: "トリガー"      , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 140, "StartDate"       , dataField: nameof(ViewData.StartDate)                 , caption: "開始日時"      , cell: builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height,  50, "Interval"        , dataField: nameof(ViewData.IntervalDisplayValue)      , caption: "間隔"          , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 100, "WeekDay"         , dataField: nameof(ViewData.WeekDayDisplayValue)       , caption: "曜日"          , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 250, "ImportDirectory" , dataField: nameof(ViewData.ImportDirectory)           , caption: "取込フォルダー", cell: builder.GetTextBoxCell()),
                new CellSetting(height, 250, "SuccessDirectory", dataField: nameof(ViewData.SuccessDirectory)          , caption: "成功フォルダー", cell: builder.GetTextBoxCell()),
                new CellSetting(height, 250, "FailedDirectory" , dataField: nameof(ViewData.FailedDirectory)           , caption: "失敗フォルダー", cell: builder.GetTextBoxCell()),
                new CellSetting(height, 170, "LogDestination"  , dataField: nameof(ViewData.LogDestinationDisplayValue), caption: "エラーログ"    , cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, "CreateBy"        , dataField: nameof(ViewData.CreateUserName)            , caption: "登録者"        , cell: builder.GetTextBoxCell()),
            });

            return builder.Build();
        }

        #endregion 初期化(グリッド関連)

        #region 初期化(権限チェック)

        /// <summary>
        /// 現在のWindowsログオンユーザーがローカルマシンの管理者権限を持たない場合、
        /// 入力欄を無効化する。
        /// </summary>
        /// <seealso cref="SetGridCheckBoxStatus"/>
        private void CheckPermissions()
        {
            if (IsUserLocalMachineAdministrator())
            {
                txtImportDirectory.Enabled = !LimitAccessFolder;
                txtSuccessDirectory.Enabled = !LimitAccessFolder;
                txtFailedDirectory.Enabled = !LimitAccessFolder;
                cmbLogDestination.Enabled = !LimitAccessFolder;

                return;
            }

            var disable = new Action<Control>(c => c.Enabled = false);

            var targets = new Control[]
            {
                cmbImportType,
                cmbImportSubType,
                rdoTargetBillingAssignment0,
                rdoTargetBillingAssignment1,
                rdoBillingAmount0,
                rdoBillingAmount1,
                rdoUpdateSameCustomer0,
                rdoUpdateSameCustomer1,
                rdoDuration0,
                rdoDuration1,
                txtStartDate,
                txtInterval,
                cbxWeekDay_Mon,
                cbxWeekDay_Tue,
                cbxWeekDay_Wed,
                cbxWeekDay_Thu,
                cbxWeekDay_Fri,
                cbxWeekDay_Sat,
                cbxWeekDay_Sun,
                txtImportDirectory,
                txtSuccessDirectory,
                txtFailedDirectory,
                btnSelectImportDirectory,
                btnSelectSuccessDirectory,
                btnSelectFailedDirectory,
                cmbLogDestination,
                rdoImportMode0,
                rdoImportMode1,
                rdoImportMode2,
            };

            targets.ForEach(c => disable(c));

            // グリッドのダブルクリックやコンボボックスの選択変更などに連動してEnabledが制御される項目があるので、
            // 連動制御を引き起こすイベントの購読を解除する。
            cmbImportType.SelectedIndexChanged -= this.cmbImportType_SelectedIndexChanged;
            rdoDuration0.CheckedChanged -= this.rdoDuration0_CheckedChanged;
            rdoBillingAmount0.CheckedChanged -= this.rdoBillingAmount0_CheckedChanged;

            // グリッドの「実行」チェックボックスは、ここではなく SetGridCheckBoxStatus メソッドで制御する。
        }

        #endregion 初期化(権限チェック)

        #region 初期化(表示値切り替え用辞書)

        /// <summary>
        /// 表示値切り替え用辞書
        /// </summary>
        private DisplayValueDictionaries DispValueDictionaries;

        internal class DisplayValueDictionaries
        {
            public DisplayValueDictionaries(string sessionKey, int companyId)
            {
                // サーバから表示値を取得する項目
                ImportSubType2 = GetImporterSettingList(sessionKey, companyId, (int)FreeImporterFormatType.Billing)
                    .Select(i => new { ImportSubType = i.Id, DisplayValue = $"{i.Code}:{i.Name}" })
                    .ToDictionary(i => i.ImportSubType, i => i.DisplayValue);

                ImportSubType4 = GetImporterSettingList(sessionKey, companyId, (int)FreeImporterFormatType.Receipt)
                    .Select(i => new { ImportSubType = i.Id, DisplayValue = $"{i.Code}:{i.Name}" })
                    .ToDictionary(i => i.ImportSubType, i => i.DisplayValue);

                ImportSubType5 = GetImporterSettingList(sessionKey, companyId, (int)FreeImporterFormatType.PaymentSchedule)
                    .Select(i => new { ImportSubType = i.Id, DisplayValue = $"{i.Code}:{i.Name}" })
                    .ToDictionary(i => i.ImportSubType, i => i.DisplayValue);

                ImportSubType0 = GetImporterSettingList(sessionKey, companyId, (int)FreeImporterFormatType.Customer)
                    .Select(i => new { ImportSubType = Convert.ToInt32(i.Code), DisplayValue = $"{i.Code}:{i.Name}" })
                    .ToDictionary(i => i.ImportSubType, i => i.DisplayValue);

                ImportSubType3 = GetEBFileSettingList(sessionKey, companyId)
                    .Where(i => i.IsUseable == 1)
                    .Select(i => new { ImportSubType = i.Id, DisplayValue = i.Name })
                    .ToDictionary(i => i.ImportSubType, i => i.DisplayValue);

                // 種別 -> 取込パターンの関連付け
                ImportTypeToImportSubType = new Dictionary<int, Dictionary<int, string>>
                {
                    { 0, ImportSubType0 },
                    { 1, ImportSubType1 },
                    { 2, ImportSubType2 },
                    { 3, ImportSubType3 },
                    { 4, ImportSubType4 },
                    { 5, ImportSubType5 },
                };
            }
            /// <summary>
            /// トリガー
            /// <para>{ TaskScheduleHistory.Result -> 表示値 }</para>
            /// </summary>
            public Dictionary<int, string> Result = new Dictionary<int, string>
            {
                { -1, "すべて" },
                { 1, "失敗" },
                { 0, "成功" },
            };

            /// <summary>
            /// トリガー
            /// <para>{ TaskSchedule.Duration -> 表示値 }</para>
            /// </summary>
            public Dictionary<int, string> Duration = new Dictionary<int, string>
            {
                { 0, "毎日" },
                { 1, "毎週" },
            };

            /// <summary>
            /// エラーログ出力場所
            /// <para>{ TaskSchedule.LogDestination -> 表示値 }</para>
            /// </summary>
            public Dictionary<int, string> LogDestination = new Dictionary<int, string>
            {
                { 0, "失敗時移動フォルダーと同一" },
                { 1, "ユーザーフォルダー" },
            };

            /// <summary>
            /// 処理対象請求データ
            /// <para>{ TaskSchedule.TargetBillingAssignment -> 表示値 }</para>
            /// </summary>
            public Dictionary<int, string> TargetBillingAssignment = new Dictionary<int, string>
            {
                { 0, "未消込のみ" },
                { 1, "すべて" },
            };

            /// <summary>
            /// 処理方法
            /// <para>{ TaskSchedule.BillingAmount -> 表示値 }</para>
            /// </summary>
            public Dictionary<int, string> BillingAmount = new Dictionary<int, string>
            {
                { 0, "更新" },
                { 1, "加算" },
            };

            /// <summary>
            /// 同一得意先請求データ
            /// <para>{ TaskSchedule.UpdateSameCustomer -> 表示値 }</para>
            /// </summary>
            public Dictionary<int, string> UpdateSameCustomer = new Dictionary<int, string>
            {
                { 0, "無視" },
                { 1, "すべて更新" },
            };

            /// <summary>
            /// 種別
            /// <para>{ TaskSchedule.ImportType -> 表示値 }</para>
            /// </summary>
            public Dictionary<int, string> ImportType = new Dictionary<int, string>
            {
                { 0, "得意先マスターインポート" },
                { 1, "債権代表者マスターインポート" },
                { 2, "請求フリーインポーター" },
                { 3, "EBデータ取込" },
                { 4, "入金フリーインポーター" },
                { 5, "入金予定フリーインポーター" },
            };

            /// <summary>
            /// 種別 -> 取込パターン選択肢一覧
            /// <para>{ TaskSchedule.ImportType -> { TaskSchedule.ImportSubType -> 表示値 } }</para>
            /// </summary>
            public Dictionary<int, Dictionary<int, string>> ImportTypeToImportSubType;

            /// <summary>
            /// 種別0
            /// <para>{ TaskSchedule.ImportSubType -> 表示値 } @ TaskSchedule.ImportType == 0</para>
            /// <para>{ ImportSetting.Id -> 表示値 } @ ImportSetting.FormatId == 4</para>
            /// </summary>
            public readonly Dictionary<int, string> ImportSubType0;

            /// <summary>
            /// 種別1
            /// <para>{ TaskSchedule.ImportSubType -> 表示値 } @ TaskSchedule.ImportType == 1</para>
            /// </summary>
            public readonly Dictionary<int, string> ImportSubType1 = new Dictionary<int, string>
            {
                { 0, "00:上書" },
                { 1, "01:追加" },
                { 2, "02:更新" },
            };

            /// <summary>
            /// 種別2
            /// <para>{ TaskSchedule.ImportSubType -> 表示値 } @ TaskSchedule.ImportType == 2</para>
            /// <para>{ ImportSetting.Id -> 表示値 } @ ImportSetting.FormatId == 1</para>
            /// </summary>
            public readonly Dictionary<int, string> ImportSubType2;

            /// <summary>
            /// 種別3
            /// <para>{ TaskSchedule.ImportSubType -> 表示値 } @ TaskSchedule.ImportType == 3</para>
            /// </summary>
            public readonly Dictionary<int, string> ImportSubType3;

            /// <summary>
            /// 種別4
            /// <para>{ TaskSchedule.ImportSubType -> 表示値 } @ TaskSchedule.ImportType == 4</para>
            /// <para>{ ImportSetting.Id -> 表示値 } @ ImportSetting.FormatId == 2</para>
            /// </summary>
            public readonly Dictionary<int, string> ImportSubType4;

            /// <summary>
            /// 種別5
            /// <para>{ TaskSchedule.ImportSubType -> 表示値 } @ TaskSchedule.ImportType == 5</para>
            /// <para>{ ImportSetting.Id -> 表示値 } @ ImportSetting.FormatId == 3</para>
            /// </summary>
            public readonly Dictionary<int, string> ImportSubType5;

            #region Web Service

            /// <summary>
            /// インポーター設定取得処理(ImporterSettingService.svc:GetHeader)を呼び出して結果を取得する。
            /// </summary>
            private static List<ImporterSetting> GetImporterSettingList(string sessionKey, int companyId, int formatId)
            {
                ImporterSettingsResult result = null;

                ServiceProxyFactory.Do<ImporterSettingService.ImporterSettingServiceClient>(client =>
                {
                    result = client.GetHeader(sessionKey, companyId, formatId);
                });

                if (result == null || result.ProcessResult.Result == false)
                {
                    return null;
                }

                return result.ImporterSettings;
            }

            private static List<EBFileSetting> GetEBFileSettingList(string sessionKey, int companyId)
            {
                EBFileSettingsResult result = null;

                ServiceProxyFactory.Do<EBFileSettingMasterService.EBFileSettingMasterClient>(client =>
                {
                    result = client.GetItems(sessionKey, companyId);
                });

                if (result == null || result.ProcessResult.Result == false)
                {
                    return null;
                }

                return result.EBFileSettings;
            }

            #endregion Web Service
        }

        #endregion 初期化(表示値切り替え用の辞書)

        #endregion 初期化

        #region 画面表示データクラス関連

        /// <summary>
        /// 画面表示データクラス
        /// </summary>
        private class ViewData
        {
            public readonly TaskSchedule Model;

            public readonly DisplayValueDictionaries DisplayValueDictionaries;

            public ViewData(TaskSchedule taskScheduleModel, DisplayValueDictionaries dispValue)
            {
                Model = taskScheduleModel;
                DisplayValueDictionaries = dispValue;
            }

            /// <summary>
            /// タスク有効(1)／無効(0)状態
            /// この項目はデータベースではなくWindowsタスクスケジューラのタスク有効／無効状態と連動する。
            /// </summary>
            public int Enabled { get; set; }

            public int Id
            {
                get { return Model.Id; }
                set { Model.Id = value; }
            }

            public int CompanyId
            {
                get { return Model.CompanyId; }
                set { Model.CompanyId = value; }
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

            public int Duration
            {
                get { return Model.Duration; }
                set { Model.Duration = value; }
            }

            public string DurationDisplayValue
            {
                get
                {
                    if (!DisplayValueDictionaries.Duration.Keys.Contains(Model.Duration))
                    {
                        return "";
                    }

                    return DisplayValueDictionaries.Duration[Model.Duration];
                }
            }

            public DateTime StartDate
            {
                get { return Model.StartDate; }
                set { Model.StartDate = value; }
            }

            public int Interval
            {
                get { return Model.Interval; }
                set { Model.Interval = value; }
            }

            public string IntervalDisplayValue
            {
                get
                {
                    if (Interval == 0)
                    {
                        return "";
                    }

                    return $"{Interval}日";
                }
            }

            public byte[] WeekDay
            {
                get { return Model.WeekDay; }
                set { Model.WeekDay = value; }
            }

            public string WeekDayDisplayValue
            {
                get
                {
                    var result = new StringBuilder();

                    if (WeekDay_Mon) result.Append("月");
                    if (WeekDay_Tue) result.Append("火");
                    if (WeekDay_Wed) result.Append("水");
                    if (WeekDay_Thu) result.Append("木");
                    if (WeekDay_Fri) result.Append("金");
                    if (WeekDay_Sat) result.Append("土");
                    if (WeekDay_Sun) result.Append("日");

                    return result.ToString();
                }
            }

            public bool WeekDay_Sat
            {
                get { return (Model.WeekDay[0] & 0x40) != 0; }
                set { if (value) Model.WeekDay[0] |= 0x40; else Model.WeekDay[0] &= 0xbf; }
            }

            public bool WeekDay_Fri
            {
                get { return (Model.WeekDay[0] & 0x20) != 0; }
                set { if (value) Model.WeekDay[0] |= 0x20; else Model.WeekDay[0] &= 0xdf; }
            }

            public bool WeekDay_Thu
            {
                get { return (Model.WeekDay[0] & 0x10) != 0; }
                set { if (value) Model.WeekDay[0] |= 0x10; else Model.WeekDay[0] &= 0xef; }
            }

            public bool WeekDay_Wed
            {
                get { return (Model.WeekDay[0] & 0x08) != 0; }
                set { if (value) Model.WeekDay[0] |= 0x08; else Model.WeekDay[0] &= 0xf7; }
            }

            public bool WeekDay_Tue
            {
                get { return (Model.WeekDay[0] & 0x04) != 0; }
                set { if (value) Model.WeekDay[0] |= 0x04; else Model.WeekDay[0] &= 0xfb; }
            }

            public bool WeekDay_Mon
            {
                get { return (Model.WeekDay[0] & 0x02) != 0; }
                set { if (value) Model.WeekDay[0] |= 0x02; else Model.WeekDay[0] &= 0xfd; }
            }

            public bool WeekDay_Sun
            {
                get { return (Model.WeekDay[0] & 0x01) != 0; }
                set { if (value) Model.WeekDay[0] |= 0x01; else Model.WeekDay[0] &= 0xfe; }
            }

            public string ImportDirectory
            {
                get { return Model.ImportDirectory; }
                set { Model.ImportDirectory = value; }
            }

            public string SuccessDirectory
            {
                get { return Model.SuccessDirectory; }
                set { Model.SuccessDirectory = value; }
            }

            public string FailedDirectory
            {
                get { return Model.FailedDirectory; }
                set { Model.FailedDirectory = value; }
            }

            public int LogDestination
            {
                get { return Model.LogDestination; }
                set { Model.LogDestination = value; }
            }

            public string LogDestinationDisplayValue
            {
                get
                {
                    if (!DisplayValueDictionaries.LogDestination.Keys.Contains(Model.LogDestination))
                    {
                        return "";
                    }

                    return DisplayValueDictionaries.LogDestination[Model.LogDestination];
                }
            }

            public int TargetBillingAssignment
            {
                get { return Model.TargetBillingAssignment; }
                set { Model.TargetBillingAssignment = value; }
            }

            public int BillingAmount
            {
                get { return Model.BillingAmount; }
                set { Model.BillingAmount = value; }
            }

            public int UpdateSameCustomer
            {
                get { return Model.UpdateSameCustomer; }
                set { Model.UpdateSameCustomer = value; }
            }

            public int CreateBy
            {
                get { return Model.CreateBy; }
                set { Model.CreateBy = value; }
            }

            public string CreateUserName
            {
                get { return Model.CreateUserName; }
                set { Model.CreateUserName = value; }
            }

            public DateTime CreateAt
            {
                get { return Model.CreateAt; }
                set { Model.CreateAt = value; }
            }

            public int UpdateBy
            {
                get { return Model.UpdateBy; }
                set { Model.UpdateBy = value; }
            }

            public string UpdateUserName
            {
                get { return Model.UpdateUserName; }
                set { Model.UpdateUserName = value; }
            }

            public DateTime UpdateAt
            {
                get { return Model.UpdateAt; }
                set { Model.UpdateAt = value; }
            }

            public int ImportMode
            {
                get { return Model.ImportMode; }
                set { Model.ImportMode = value; }
            }
        }

        /// <summary>
        /// 画面入力内容からViewData/TaskScheduleオブジェクトを作成する。
        /// 画面上のデータしか拾わない(会社コードやログインユーザ、タスク有効無効状態などはセットしない)ので注意。
        /// </summary>
        /// <param name="viewData"></param>
        /// <returns></returns>
        private ViewData MakeViewDataFromEditForm(ViewData viewData = null)
        {
            var data = viewData ?? new ViewData(new TaskSchedule { WeekDay = new byte[1] }, DispValueDictionaries);

            data.ImportType = (int)(cmbImportType.SelectedValue ?? -1);
            data.ImportSubType = (int)(cmbImportSubType.SelectedValue ?? -1);
            data.TargetBillingAssignment = rdoTargetBillingAssignment1.Checked ? 1 : 0;
            data.BillingAmount = rdoBillingAmount1.Checked ? 1 : 0;
            data.UpdateSameCustomer = rdoUpdateSameCustomer1.Checked ? 1 : 0;
            data.Duration = rdoDuration1.Checked ? 1 : 0;
            data.StartDate = txtStartDate.Value ?? DateTime.MinValue;
            data.Interval = (int)(txtInterval.Value ?? 0);
            data.WeekDay_Mon = cbxWeekDay_Mon.Checked;
            data.WeekDay_Tue = cbxWeekDay_Tue.Checked;
            data.WeekDay_Wed = cbxWeekDay_Wed.Checked;
            data.WeekDay_Thu = cbxWeekDay_Thu.Checked;
            data.WeekDay_Fri = cbxWeekDay_Fri.Checked;
            data.WeekDay_Sat = cbxWeekDay_Sat.Checked;
            data.WeekDay_Sun = cbxWeekDay_Sun.Checked;
            data.ImportDirectory = txtImportDirectory.Text;
            data.SuccessDirectory = txtSuccessDirectory.Text;
            data.FailedDirectory = txtFailedDirectory.Text;
            data.LogDestination = (int)(cmbLogDestination.SelectedValue ?? -1);
            data.ImportMode = rdoImportMode0.Checked ? 0 : rdoImportMode1.Checked ? 1 : 2;

            return data;
        }

        /// <summary>
        /// ViewDataを画面に適用する。
        /// </summary>
        /// <param name="viewData">null時は入力内容を初期化する。</param>
        private void ApplyViewDataToEditForm(ViewData viewData)
        {
            var data = viewData ?? new ViewData(new TaskSchedule { WeekDay = new byte[1] }, DispValueDictionaries)
            {
                // Deselect ComboBoxes
                ImportType = -1,
                ImportSubType = -1,
                LogDestination = (!LimitAccessFolder) ? -1 : 0,
            };

            cmbImportType.SelectedValue = data.ImportType;
            cmbImportSubType.SelectedValue = data.ImportSubType;
            rdoTargetBillingAssignment0.Checked = (data.TargetBillingAssignment == 0);
            rdoTargetBillingAssignment1.Checked = (data.TargetBillingAssignment == 1);
            rdoBillingAmount0.Checked = (data.BillingAmount == 0);
            rdoBillingAmount1.Checked = (data.BillingAmount == 1);
            rdoUpdateSameCustomer0.Checked = (data.UpdateSameCustomer == 0);
            rdoUpdateSameCustomer1.Checked = (data.UpdateSameCustomer == 1);
            rdoDuration0.Checked = (data.Duration == 0);
            rdoDuration1.Checked = (data.Duration == 1);
            txtStartDate.Value = (data.StartDate != DateTime.MinValue) ? data.StartDate : (DateTime?)null;
            txtInterval.Value = (data.Interval != 0) ? data.Interval : (decimal?)null;
            cbxWeekDay_Mon.Checked = data.WeekDay_Mon;
            cbxWeekDay_Tue.Checked = data.WeekDay_Tue;
            cbxWeekDay_Wed.Checked = data.WeekDay_Wed;
            cbxWeekDay_Thu.Checked = data.WeekDay_Thu;
            cbxWeekDay_Fri.Checked = data.WeekDay_Fri;
            cbxWeekDay_Sat.Checked = data.WeekDay_Sat;
            cbxWeekDay_Sun.Checked = data.WeekDay_Sun;
            txtImportDirectory.Text = data.ImportDirectory;
            txtSuccessDirectory.Text = data.SuccessDirectory;
            txtFailedDirectory.Text = data.FailedDirectory;
            cmbLogDestination.SelectedValue = data.LogDestination;
            rdoImportMode0.Checked = (data.ImportMode == 0);
            rdoImportMode1.Checked = (data.ImportMode == 1);
            rdoImportMode2.Checked = (data.ImportMode == 2);

            IsModified = false;
        }

        #endregion 画面表示データクラス関連

        #region 画面操作関連イベントハンドラ

        /// <summary>
        /// タスク一覧データを登録する。
        /// </summary>
        [OperationLog("登録")]
        private void RegisterTaskSchedule()
        {
            ClearStatusMessage();

            ViewData viewData = null;

            try
            {
                if (!ValidateForRegister())
                {
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    return;
                }

                viewData = MakeViewDataFromEditForm();
                var registerResult = RegisterTaskSchedule(viewData);

                if (registerResult)
                {
                    LoadGridData();
                    ClearEditForm();
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }

            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSaveError);

                if (viewData != null)
                {
                    ex.Data["ImportType"] = viewData.ImportType;
                    ex.Data["ImportSubType"] = viewData.ImportSubType;
                }
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();

            if (IsModified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            ClearEditForm();
        }

        private void ClearEditForm()
        {
            try
            {
                ApplyViewDataToEditForm(null);
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// タスクを削除する。
        /// </summary>
        [OperationLog("削除")]
        private void DeleteTaskSchedules()
        {
            ClearStatusMessage();

            ViewData viewData = null;

            try
            {
                if (!ValidateForDeletion())
                {
                    return;
                }

                viewData = MakeViewDataFromEditForm();
                DeleteTaskSchedules(viewData);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrDeleteError);

                if (viewData != null)
                {
                    ex.Data["ImportType"] = viewData.ImportType;
                    ex.Data["ImportSubType"] = viewData.ImportSubType;
                }
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// ログ画面を表示する。
        /// </summary>
        [OperationLog("ログ閲覧")]
        private void ShowTaskSchedulerLogScreen()
        {
            ClearStatusMessage();

            try
            {
                var form = ApplicationContext.Create(nameof(PH0502));
                ApplicationContext.ShowDialog(ParentForm, form);
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
            if (IsModified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            BaseForm.Close();
        }

        /// <summary>
        /// 「種別」コンボボックス 選択変更<para/>
        /// 選択値に応じて「取込パターン」コンボボックスやラジオボタン操作可否などを制御する。
        /// </summary>
        private void cmbImportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = (VOneComboControl)sender;
            var importType = (int)(cmb.SelectedValue ?? -1);

            if (0 <= importType)
            {
                cmbImportSubType.Items.Clear();
                cmbImportSubType.Items.AddRange(DispValueDictionaries
                    .ImportTypeToImportSubType[importType]
                    .Select(disp => new ListItem(disp.Value, disp.Key))
                    .ToArray());
            }
            else
            {
                cmbImportSubType.SelectedIndex = -1;
            }

            gbxImportMode.Visible = importType == 0;
            gbxTargetBillingAssignment.Visible = importType == 5;
            gbxBillingAmount.Visible = importType == 5;
            gbxUpdateSameCustomer.Visible = importType == 5;

            if (importType == 0)
                gbxImportMode.Location = new Point(gbxTargetBillingAssignment.Location.X, gbxTargetBillingAssignment.Location.Y);

            // 種別に「入金予定フリーインポーター」選択時のみ有効
            var isEnabled = (importType == 5);
            rdoTargetBillingAssignment0.Enabled = isEnabled;
            rdoTargetBillingAssignment1.Enabled = isEnabled;
            rdoBillingAmount0.Enabled = isEnabled;
            rdoBillingAmount1.Enabled = isEnabled;

            // 種別に「入金予定フリーインポーター」、処理方法に「更新」選択時のみ有効
            rdoBillingAmount0_CheckedChanged(rdoBillingAmount0, null);

            // 「入金予定フリーインポーター」以外選択時は処理対象請求データと処理方法をクリア
            if (!isEnabled)
            {
                rdoTargetBillingAssignment0.Checked = true;
                rdoBillingAmount0.Checked = true;
            }
        }

        /// <summary>
        /// 「処理方法」ラジオボタン選択変更
        /// </summary>
        private void rdoBillingAmount0_CheckedChanged(object sender, EventArgs e)
        {
            var importType = (int)(cmbImportType.SelectedValue ?? -1);

            // 種別に「入金予定フリーインポーター」、処理方法に「更新」選択時のみ有効
            var isEnabled = (importType == 5) && rdoBillingAmount0.Checked;
            rdoUpdateSameCustomer0.Enabled = isEnabled;
            rdoUpdateSameCustomer1.Enabled = isEnabled;

            // 「加算」選択時は同一得意先請求データをクリア
            if (!isEnabled)
            {
                rdoUpdateSameCustomer0.Checked = true;
            }
        }

        /// <summary>
        /// 「トリガー」ラジオボタン選択変更
        /// </summary>
        private void rdoDuration0_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = (RadioButton)sender;

            txtInterval.Enabled = rdo.Checked;

            cbxWeekDay_Mon.Enabled = !rdo.Checked;
            cbxWeekDay_Tue.Enabled = !rdo.Checked;
            cbxWeekDay_Wed.Enabled = !rdo.Checked;
            cbxWeekDay_Thu.Enabled = !rdo.Checked;
            cbxWeekDay_Fri.Enabled = !rdo.Checked;
            cbxWeekDay_Sat.Enabled = !rdo.Checked;
            cbxWeekDay_Sun.Enabled = !rdo.Checked;

            // 「毎日」選択時は曜日をクリア
            if (rdo.Checked)
            {
                cbxWeekDay_Mon.Checked = false;
                cbxWeekDay_Tue.Checked = false;
                cbxWeekDay_Wed.Checked = false;
                cbxWeekDay_Thu.Checked = false;
                cbxWeekDay_Fri.Checked = false;
                cbxWeekDay_Sat.Checked = false;
                cbxWeekDay_Sun.Checked = false;
            }

            // 「毎週」選択時は間隔をクリア
            else
            {
                txtInterval.Value = null;
            }
        }

        /// <summary>
        /// グリッドのセルダブルクリック<para/>
        /// 選択行のデータを入力欄にセットする。
        /// </summary>
        private void grdTaskList_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (IsModified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

            var grid = (VOneGridControl)sender;

            if (grid.CurrentCell == null || grid.CurrentCell.RowIndex < 0)
            {
                return;
            }

            ApplyViewDataToEditForm((ViewData)grid.CurrentRow.DataBoundItem);

            ClearStatusMessage();
        }

        /// <summary>
        /// 取込フォルダー選択ボタンクリック
        /// </summary>
        private void btnSelectImportDirectory_Click(object sender, EventArgs e)
        {
            var path = txtImportDirectory.Text ?? FolderBrowserDefaultPath;
            var selectedPath = string.Empty;
            var rootBrowserPath = new List<string>();
            if (!LimitAccessFolder ?
                !ShowFolderBrowserDialog(path, out selectedPath, description : "取込フォルダー") :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder)) return;

            txtImportDirectory.Text = (!LimitAccessFolder) ? selectedPath : rootBrowserPath.FirstOrDefault();

        }

        /// <summary>
        /// 成功時移動フォルダー選択ボタンクリック
        /// </summary>
        private void btnSelectSuccessDirectory_Click(object sender, EventArgs e)
        {
            var path = txtSuccessDirectory.Text ?? FolderBrowserDefaultPath;
            var selectedPath = string.Empty;
            var rootBrowserPath = new List<string>();
            if (!LimitAccessFolder ?
                !ShowFolderBrowserDialog(path, out selectedPath, description: "成功時移動フォルダー") :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder)) return;

            txtSuccessDirectory.Text = (!LimitAccessFolder) ? selectedPath : rootBrowserPath.FirstOrDefault();

        }

        /// <summary>
        /// 失敗時移動フォルダー選択ボタンクリック
        /// </summary>
        private void btnSelectFailedDirectory_Click(object sender, EventArgs e)
        {
            var path = txtFailedDirectory.Text ?? FolderBrowserDefaultPath;
            var selectedPath = string.Empty;
            var rootBrowserPath = new List<string>();
            if (!LimitAccessFolder ?
                !ShowFolderBrowserDialog(path, out selectedPath, description: "失敗時移動フォルダー") :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder)) return;

            txtFailedDirectory.Text = (!LimitAccessFolder) ? selectedPath : rootBrowserPath.FirstOrDefault();

        }

        /// <summary>
        /// グリッドのセル(実行チェックボックス)クリック
        /// </summary>
        private void grdTaskList_CellContentClick(object sender, CellEventArgs e)
        {
            try
            {
                var grid = (VOneGridControl)sender;
                var cell = grid.CurrentCell;

                if (cell.CellIndex != 2 || !(cell is CheckBoxCell))
                {
                    return;
                }

                var isEnabled = (bool)cell.EditedFormattedValue;
                var viewData = (ViewData)grid.Rows[cell.RowIndex].DataBoundItem;

                var task = SetTaskAvailabilityAsync(viewData, isEnabled);
                ProgressDialog.Start(BaseForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion 画面操作関連イベントハンドラ

        #region Functions

        #region 登録

        /// <summary>
        /// 入力値検証(登録時)
        /// </summary>
        private bool ValidateForRegister()
        {
            if (cmbImportType.SelectedIndex < 0 || cmbImportType.Items.Count <= cmbImportType.SelectedIndex)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "種別");
                cmbImportType.Focus();
                return false;
            }

            if (cmbImportSubType.SelectedIndex < 0 || cmbImportSubType.Items.Count <= cmbImportSubType.SelectedIndex)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "取込パターン");
                cmbImportSubType.Focus();
                return false;
            }

            if (txtStartDate.Value == null || txtStartDate.Value == DateTime.MinValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "開始日時");
                txtStartDate.Focus();
                return false;
            }

            // 毎日
            if (rdoDuration0.Checked)
            {
                if (txtInterval.Value == null || txtInterval.Value <= 0)
                {
                    ShowWarningDialog(MsgWngInputRequired, "間隔");
                    txtInterval.Focus();
                    return false;
                }
            }

            // 毎週
            else
            {
                var checkboxes = new CheckBox[] {
                    cbxWeekDay_Mon, cbxWeekDay_Tue, cbxWeekDay_Wed, cbxWeekDay_Thu, cbxWeekDay_Fri, cbxWeekDay_Sat, cbxWeekDay_Sun
                };

                if (checkboxes.All(cbx => !cbx.Checked))
                {
                    ShowWarningDialog(MsgWngInputRequired, "曜日");
                    cbxWeekDay_Mon.Focus();
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(txtImportDirectory.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "取込フォルダー");
                txtImportDirectory.Focus();
                return false;
            }

            var gridDataList = grdTaskList.DataSource as List<ViewData>;
            var compareTargetGridDataList = gridDataList.Where(t => // 現在編集中のタスクを除外
            {
                var c1 = (int)cmbImportType.SelectedValue == t.ImportType;
                var c2 = (int)cmbImportSubType.SelectedValue == t.ImportSubType;
                return !(c1 && c2);
            });

            if (!Directory.Exists(txtImportDirectory.Text))
            {
                ShowWarningDialog(MsgWngNotExistFolder);
                txtImportDirectory.Focus();
                return false;
            }
            var importDir = NormalizePath(txtImportDirectory.Text);
            if (compareTargetGridDataList.Any(t => importDir == NormalizePath(t.ImportDirectory) || importDir == NormalizePath(t.SuccessDirectory) || importDir == NormalizePath(t.FailedDirectory))) // サーバではなくグリッドデータでチェック
            {
                ShowWarningDialog(MsgWngCannotSetAlreadyRegisPath);
                txtImportDirectory.Focus();
                return false;
            }
            if (!Path.IsPathRooted(txtImportDirectory.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "絶対パス");
                txtImportDirectory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSuccessDirectory.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "成功時移動フォルダー");
                txtSuccessDirectory.Focus();
                return false;
            }
            if (!Directory.Exists(txtSuccessDirectory.Text))
            {
                ShowWarningDialog(MsgWngNotExistFolder);
                txtSuccessDirectory.Focus();
                return false;
            }
            var successDir = NormalizePath(txtSuccessDirectory.Text);
            if (successDir == NormalizePath(txtImportDirectory.Text))
            {
                ShowWarningDialog(MsgWngCannotSetSameImportFolder);
                txtSuccessDirectory.Focus();
                return false;
            }
            if (compareTargetGridDataList.Any(t => successDir == NormalizePath(t.ImportDirectory))) // サーバではなくグリッドデータでチェック
            {
                ShowWarningDialog(MsgWngCannotSetAlreadyRegistedSameImportFolder);
                txtSuccessDirectory.Focus();
                return false;
            }
            if (!Path.IsPathRooted(txtSuccessDirectory.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "絶対パス");
                txtSuccessDirectory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFailedDirectory.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "失敗時移動フォルダー");
                txtFailedDirectory.Focus();
                return false;
            }
            if (!Directory.Exists(txtFailedDirectory.Text))
            {
                ShowWarningDialog(MsgWngNotExistFolder);
                txtFailedDirectory.Focus();
                return false;
            }
            var failedDir = NormalizePath(txtFailedDirectory.Text);
            if (failedDir == NormalizePath(txtImportDirectory.Text))
            {
                ShowWarningDialog(MsgWngCannotSetSameImportFolder);
                txtFailedDirectory.Focus();
                return false;
            }
            if (compareTargetGridDataList.Any(t => failedDir == NormalizePath(t.ImportDirectory))) // サーバではなくグリッドデータでチェック
            {
                ShowWarningDialog(MsgWngCannotSetAlreadyRegistedSameImportFolder);
                txtFailedDirectory.Focus();
                return false;
            }
            if (!Path.IsPathRooted(txtFailedDirectory.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "絶対パス");
                txtFailedDirectory.Focus();
                return false;
            }

            if (cmbLogDestination.SelectedIndex < 0 || cmbLogDestination.Items.Count <= cmbLogDestination.SelectedIndex)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "エラーログ出力場所");
                cmbLogDestination.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// タスクを登録する。
        /// </summary>
        private bool RegisterTaskSchedule(ViewData viewData)
        {
            // Windows Task Scheduler
            RegisterWindowsTaskSchedule(viewData);

            // Database
            TaskSchedule result = null;
            var saveTask = Task.Run(async () =>
            {
                viewData.CompanyId = CompanyId;
                viewData.CreateBy = Login.UserId;
                viewData.UpdateBy = Login.UserId;

                var taskSchedule = viewData.Model;
                result = await SaveTaskScheduleAsync(SessionKey, taskSchedule);
            });
            ProgressDialog.Start(BaseForm, saveTask, false, SessionKey);

            return (result != null);
        }

        /// <summary>
        /// Windowsタスクスケジューラにタスクを登録する。
        /// </summary>
        private void RegisterWindowsTaskSchedule(ViewData viewData)
        {
            var folder = EnsureTaskFolder(TaskSchedulerFolder);
            var allTasks = folder.AllTasks.ToList();

            var boundTaskName = GetBoundTaskName(viewData);
            var task = allTasks.SingleOrDefault(t => t.Name == boundTaskName);

            if (task == null) // 新規登録
            {
                var definition = TaskService.Instance.NewTask();
                definition.Settings.Enabled = false;

                // ダミー登録情報。最低1つずつないと登録に失敗する。
                var dummyTrigger = (LogonTrigger)definition.Triggers.AddNew(TaskTriggerType.Logon);
                var dummyAction = (ExecAction)definition.Actions.AddNew(TaskActionType.Execute);
                dummyAction.Path = @"dummy.exe";

                task = folder.RegisterTaskDefinition(boundTaskName, definition);
            }

            // 意図しない設定が含まれているかもしれないので、トリガーとアクションは一度全てクリアして作り直す。
            task.Definition.Triggers.Clear();
            task.Definition.Actions.Clear();

            // 毎日
            if (viewData.Duration == 0)
            {
                var trigger = (DailyTrigger)task.Definition.Triggers.AddNew(TaskTriggerType.Daily);
                trigger.StartBoundary = viewData.StartDate;
                trigger.DaysInterval = (short)viewData.Interval;
            }

            // 毎週
            else
            {
                var trigger = (WeeklyTrigger)task.Definition.Triggers.AddNew(TaskTriggerType.Weekly);
                trigger.StartBoundary = viewData.StartDate;
                trigger.DaysOfWeek = (DaysOfTheWeek)viewData.WeekDay[0];
            }

            var appBaseDir = AppDomain.CurrentDomain.BaseDirectory;

            var action = (ExecAction)task.Definition.Actions.AddNew(TaskActionType.Execute);
            action.Path = Path.Combine(appBaseDir, ImporterExecutableName);
            action.Arguments = $"{Login.CompanyCode} {viewData.ImportType} {viewData.ImportSubType}";

            task.Definition.RegistrationInfo.Description = $"{(isCloudEdition ? CloudApplicationName : StandardApplicationName)} タイムスケジューラー";
            task.Definition.Principal.RunLevel = TaskRunLevel.Highest;
            task.RegisterChanges();

            folder.RegisterTaskDefinition(boundTaskName,
                task.Definition, TaskCreation.CreateOrUpdate, "SYSTEM", null, TaskLogonType.ServiceAccount);

        }

        #endregion 登録

        #region 削除

        /// <summary>
        /// 入力値検証(削除時)
        /// </summary>
        private bool ValidateForDeletion()
        {
            if (cmbImportType.SelectedIndex < 0 || cmbImportType.Items.Count <= cmbImportType.SelectedIndex)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "種別");
                cmbImportType.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// タスクを削除する。
        /// </summary>
        private void DeleteTaskSchedules(ViewData viewData)
        {
            bool? existing = null;
            var checkTask = Task.Run(async () =>
            {
                existing = await IsExistingTaskScheduleAsync(SessionKey, CompanyId, viewData.ImportType, viewData.ImportSubType);
            });
            ProgressDialog.Start(BaseForm, checkTask, false, SessionKey);

            if (!existing.HasValue)
            {
                ShowWarningDialog(MsgErrDeleteError);
                return;
            }
            if (!existing.Value)
            {
                ShowWarningDialog(MsgWngNoDeleteData);
                return;
            }

            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                return;
            }

            // Windows Task Scheduler
            DeleteWindowsTaskSchedules(viewData);

            // Database
            int? result = null;
            var deleteTask = Task.Run(async () =>
            {
                result = await DeleteTaskSchedulesAsync(SessionKey, CompanyId, viewData.ImportType, viewData.ImportSubType);
            });
            ProgressDialog.Start(BaseForm, deleteTask, false, SessionKey);

            if (result.HasValue)
            {
                ClearEditForm();
                LoadGridData();
                DispStatusMessage(MsgInfDeleteSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
                return;
            }
        }

        /// <summary>
        /// Windowsタスクスケジューラからタスクを削除する。
        /// </summary>
        /// <param name="viewData"></param>
        private void DeleteWindowsTaskSchedules(ViewData viewData)
        {
            var folder = EnsureTaskFolder(TaskSchedulerFolder);
            var allTasks = folder.AllTasks.ToList();

            var boundTaskName = GetBoundTaskName(viewData);
            var tasks = allTasks.Where(t => t.Name.StartsWith(boundTaskName));

            tasks.ForEach(t =>
            {
                folder.DeleteTask(t.Name);
            });
        }

        #endregion 削除

        #region グリッド

        /// <summary>
        /// スケジュールデータを検索し、グリッドに表示する。
        /// </summary>
        private void LoadGridData()
        {
            List<TaskSchedule> taskScheduleList = null;
            var task = Task.Run(async () =>
            {
                taskScheduleList = await GetTaskScheduleItemsAsync(SessionKey, CompanyId);
            });
            ProgressDialog.Start(BaseForm, task, false, SessionKey);

            var viewDataList = taskScheduleList.Select(row => new ViewData(row, DispValueDictionaries))
                .OrderBy(data => data.ImportType)
                .ThenBy(data => data.ImportSubType)
                .ToList();

            grdTaskList.DataSource = viewDataList;
        }

        /// <summary>
        /// LoadGridDataメソッド内では「実行」チェックボックスの制御が効かないのでDataBindingCompleteイベントを待ってから処理する。
        /// </summary>
        private void grdTaskList_DataBindingComplete(object sender, MultiRowBindingCompleteEventArgs e)
        {
            SetGridCheckBoxStatus();
        }

        /// <summary>
        /// 権限や状況に応じてグリッド「実行」チェックボックスの状態をセットする。
        /// </summary>
        private void SetGridCheckBoxStatus()
        {
            var isAdmin = IsUserLocalMachineAdministrator();

            var viewDataList = (List<ViewData>)grdTaskList.DataSource;
            var folder = EnsureTaskFolder(TaskSchedulerFolder);
            var allTasks = folder.AllTasks.ToList();

            viewDataList.ForEach((data, idx) =>
            {
                var boundTaskName = GetBoundTaskName(data);
                var task = allTasks.SingleOrDefault(t => t.Name == boundTaskName);
                var cbx = (CheckBoxCell)grdTaskList.Rows[idx].Cells[2];

                if (isAdmin && task != null)
                {
                    data.Enabled = task.Enabled ? (int)cbx.TrueValue : (int)cbx.FalseValue;
                }
                else
                {
                    data.Enabled = (int)cbx.FalseValue;
                }
            });
        }

        #endregion グリッド

        /// <summary>
        /// ViewDataからWindowsタスクスケジューラのタスク有効／無効をセットする。
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="isEnabled"></param>
        private Task SetTaskAvailabilityAsync(ViewData viewData, bool isEnabled)
        {
            return Task.Run(() =>
            {
                var folder = EnsureTaskFolder(TaskSchedulerFolder);
                var allTasks = folder.AllTasks.ToList();

                var boundTaskName = GetBoundTaskName(viewData);
                var task = allTasks.SingleOrDefault(t => t.Name == boundTaskName);

                if (task != null)
                {
                    task.Enabled = isEnabled;
                }
            });
        }

        /// <summary>
        /// ViewDataからWindowsタスクスケジューラのタスク名を取得する。
        /// </summary>
        /// <param name="viewData"></param>
        /// <returns></returns>
        private string GetBoundTaskName(ViewData viewData)
        {
            return $"{Login.CompanyCode}{viewData.ImportTypeDisplayValue}{viewData.ImportSubTypeDisplayValue.Split(':')[0]}";
        }

        #endregion Functions

        #region Web Service

        /// <summary>
        /// タスクスケジュール取得処理(TaskScheduleMasterService.svc:GetItems)を呼び出して結果を取得する。
        /// </summary>
        private static async Task<List<TaskSchedule>> GetTaskScheduleItemsAsync(string sessionKey, int companyId)
        {
            TaskSchedulesResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<TaskScheduleMasterService.TaskScheduleMasterClient>();
                result = await client.GetItemsAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.TaskSchedules;
        }

        /// <summary>
        /// タスクスケジュール保存処理(TaskScheduleMasterService.svc:Save)を呼び出して結果を取得する。
        /// </summary>
        private static async Task<TaskSchedule> SaveTaskScheduleAsync(string sessionKey, TaskSchedule taskSchedule)
        {
            TaskScheduleResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<TaskScheduleMasterService.TaskScheduleMasterClient>();
                result = await client.SaveAsync(sessionKey, taskSchedule);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.TaskSchedule;
        }

        /// <summary>
        /// タスクスケジュール存在確認処理(TaskScheduleMasterService.svc:Exists)を呼び出して結果を取得する。
        /// </summary>
        private static async Task<bool?> IsExistingTaskScheduleAsync(string sessionKey, int companyId, int importType, int importSubType)
        {
            var exist = (bool?)null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<TaskScheduleMasterService.TaskScheduleMasterClient>();
                var result = await client.ExistsAsync(sessionKey, companyId, importType, importSubType);
                exist = result?.Exist;
            });
            return exist;
        }

        /// <summary>
        /// タスクスケジュール削除処理(TaskScheduleMasterService.svc:Delete)を呼び出して結果を取得する。
        /// </summary>
        private static async Task<int?> DeleteTaskSchedulesAsync(string sessionKey, int companyId, int importType, int importSubType)
        {
            CountResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<TaskScheduleMasterService.TaskScheduleMasterClient>();
                result = await client.DeleteAsync(sessionKey, companyId, importType, importSubType);
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
        /// 現在のWindowsログオンユーザーがローカルマシンの管理者権限を持っているか確認する。
        /// </summary>
        /// <returns>管理者であるか否か</returns>
        /// <remarks>
        /// 管理者権限を確認する方法は幾つかあるようだが、その中で結果が(ローカルマシンの)タスクマネージャ制御権限と一致する方法を採用した。
        /// <para>現在のユーザーが管理者か調べる http://dobon.net/vb/dotnet/system/isadmin.html </para>
        /// </remarks>
        public static bool IsUserLocalMachineAdministrator()
        {
            var principal = (WindowsPrincipal)Thread.CurrentPrincipal;

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// ディレクトリ／ファイルパスを比較するために文字列を正規化する。
        /// </summary>
        /// <param name="path"></param>
        /// <returns>異常時は何も処理せずそのまま返す(例外揉み消し)。</returns>
        public static string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            try
            {
                return Path
                    .GetFullPath(new Uri(path).LocalPath)
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .ToUpperInvariant();
            }
            catch
            {
                return path;
            }
        }

        /// <summary>
        /// Windowsタスクスケジューラのルートフォルダ直下に指定名称のフォルダを作成する。
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns>該当TaskFolderオブジェクト</returns>
        /// <remarks>例外処理は行わないので、フォルダ名異常や権限エラーなどは呼び出し側で対処</remarks>
        public static TaskFolder EnsureTaskFolder(string folderName)
        {
            var root = TaskService.Instance.RootFolder;

            var folder = root.SubFolders.FirstOrDefault(f => f.Name == folderName);
            if (folder != null)
            {
                return folder;
            }

            return root.CreateFolder(folderName, TaskSecurity.DefaultTaskSecurity);
        }

        #endregion Helper
    }
}