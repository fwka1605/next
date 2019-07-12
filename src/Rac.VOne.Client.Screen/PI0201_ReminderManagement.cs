using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.ReminderService;
using Rac.VOne.Client.Screen.ReminderSettingService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Common;
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
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>督促データ管理</summary>
    public partial class PI0201 : VOneScreenBase
    {
        // 変数宣言
        private int Precision { get; set; }
        List<ReportSetting> ReportSettingList { get; set; }
        private List<ColumnNameSetting> ColumnNameSettingInfo { get; set; }
        private ReminderSearch SearchCondition { get; set; }
        private string CellName(string value) => $"cel{value}";
        private ReminderCommonSetting ReminderCommonSetting { get; set; }
        private List<ReminderSummarySetting> ReminderSummarySetting { get; set; }
        private List<ReminderTemplateSetting> ReminderTemplateSetting { get; set; }
        private Currency Currency { get; set; }
        private int DefaultScreenHeight { get; }
        private int DefaultPanelHeight { get; }

        public PI0201()
        {
            InitializeComponent();
            gridReminder.SetupShortcutKeys();
            Text = "督促データ管理";
            ReportSettingList = new List<ReportSetting>();
            InitializeHandlers();
            DefaultScreenHeight = this.Height;
            DefaultPanelHeight = pnlHistory.Height;
        }
        private void InitializeHandlers()
        {
            tbcReminder.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcReminder.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = Exit;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
        }

        #region PF0401 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("一括変更");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = StatusChangeAltogether;

            BaseContext.SetFunction04Caption("督促状発行");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = Print;

            BaseContext.SetFunction05Caption("発行履歴");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = PrintHistory;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Caption("消込台帳");
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = CustomerLedger;

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PI0201_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync()
                    .ContinueWith(t =>
                    {
                        return LoadGridSetting();
                    })
                    .Unwrap());
                }
                loadTask.Add(LoadControlColorAsync());
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                loadTask.Add(LoadReminderCommonSetting());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SuspendLayout();
                tbcReminder.SelectedIndex = 0;
                ResumeLayout();
                SetInitialSetting();
                InitializeReminderGrid();
                InitializeHistoryGrid();
                ClearControls();
                gridReminder.Text = "";

                //対応履歴の縦幅を画面サイズに応じて伸縮
                //this.SizeChanged += (s, ev) => SetHistoryPanelHeight();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task LoadGridSetting()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ColumnNameSettingMasterClient>();

                var result = await service.GetItemsAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    ColumnNameSettingInfo = result.ColumnNames.Where(x => x.TableName == nameof(Billing)).ToList();
                    
                }
            });
        }

        #endregion

        #region 画面の Format & Warning Setting
        private void SetInitialSetting()
        {
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);

                txtFromCustomerCode.Format = expression.CustomerCodeFormatString;
                txtFromCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtFromCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;
                txtFromCustomerCode.ImeMode = expression.CustomerCodeImeMode();

                txtToCustomerCode.Format = expression.CustomerCodeFormatString;
                txtToCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtToCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;
                txtToCustomerCode.ImeMode = expression.CustomerCodeImeMode();

                if (UseForeignCurrency)
                {
                    lblCurrency.Visible = true;
                    txtCurrencyCode.Visible = true;
                    btnCurrency.Visible = true;
                }

                if (ReminderCommonSetting?.ReminderManagementMode == (int)ReminderManagementMode.ByReminder)
                    BaseContext.SetFunction03Caption("");
            }

            Settings.SetCheckBoxValue<PF0401>(Login, cbxCustomer);
        }

        private async Task<Currency> GetCurrencyInfo()
        {
            var currency = new Currency();

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });

                currency = result.Currencies.FirstOrDefault();

                if (currency != null)
                    Precision = currency.Precision;
            });

            return currency;
        }

        #endregion

        #region グリッド作成
        private void InitializeReminderGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var widthCcy = UseForeignCurrency ? 60 : 0;

            var memoCell = builder.GetTextBoxCell();
            memoCell.Style.Multiline = MultiRowTriState.True;

            builder.Items.Add(new CellSetting(height,  30, nameof(ReminderSummary.Checked),      dataField: nameof(ReminderSummary.Checked),      caption: "選択",        cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false));
            if (ReminderCommonSetting?.ReminderManagementMode == (int)ReminderManagementMode.ByReminder)
                builder.Items.Add(new CellSetting(height, 30, "Detail", caption: "明細", cell: builder.GetButtonCell(), readOnly: false));
            builder.Items.Add(new CellSetting(height, 115, nameof(ReminderSummary.CustomerCode), dataField: nameof(ReminderSummary.CustomerCode), caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 150, nameof(ReminderSummary.CustomerName), dataField: nameof(ReminderSummary.CustomerName), caption: "得意先名",    cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, widthCcy, nameof(ReminderSummary.CurrencyCode), dataField: nameof(ReminderSummary.CurrencyCode), caption: "通貨コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(ReminderSummary.RemainAmount), dataField: nameof(ReminderSummary.RemainAmount), caption: "請求残", cell: builder.GetNumberCellCurrency(Precision, Precision, 0), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(ReminderSummary.ReminderAmount), dataField: nameof(ReminderSummary.ReminderAmount), caption: "滞留金額", cell: builder.GetNumberCellCurrency(Precision, Precision, 0), sortable: true));
            if (ReminderCommonSetting?.ReminderManagementMode == (int)ReminderManagementMode.ByCustomer)
                builder.Items.Add(new CellSetting(height, 240, nameof(ReminderSummary.Memo), dataField: nameof(ReminderSummary.Memo), caption: "対応記録", cell: memoCell, sortable: true));
            builder.Items.Add(new CellSetting(height, 200, nameof(ReminderSummary.DestinationDepartmentName), dataField: nameof(ReminderSummary.DestinationDepartmentName), caption: "相手先部署", cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, 200, nameof(ReminderSummary.CustomerStaffName), dataField: nameof(ReminderSummary.CustomerStaffName), caption: "相手先担当者", cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, 200, nameof(ReminderSummary.CustomerNote), dataField: nameof(ReminderSummary.CustomerNote), caption: "得意先備考", cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(ReminderSummary.CustomerTel), dataField: nameof(ReminderSummary.CustomerTel), caption: "電話番号", cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(ReminderSummary.CustomerFax), dataField: nameof(ReminderSummary.CustomerFax), caption: "FAX番号", cell: builder.GetTextBoxCell(), sortable: true));

            gridReminder.Template = builder.Build();
            gridReminder.HideSelection = true;
            gridReminder.AllowAutoExtend = false;
            gridReminder.FreezeLeftCellName = CellName(nameof(ReminderSummary.Checked));
            if (ReminderCommonSetting?.ReminderManagementMode == (int)ReminderManagementMode.ByReminder)
                gridReminder.FreezeLeftCellName = CellName("Detail");
        }

        private void InitializeHistoryGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var memoCell = builder.GetTextBoxCell();
            memoCell.Style.Multiline = MultiRowTriState.True;

            builder.Items.Add(new CellSetting(height, 140, nameof(ReminderSummaryHistory.CreateAt), dataField: nameof(ReminderSummaryHistory.CreateAt), caption: "更新日時", cell: builder.GetDateCell_yyyyMMddHHmmss(MultiRowContentAlignment.MiddleCenter)));
            builder.Items.Add(new CellSetting(height, 140, nameof(ReminderSummaryHistory.InputTypeName), dataField: nameof(ReminderSummaryHistory.InputTypeName), caption: "アクション", cell: builder.GetTextBoxCell()));
            builder.Items.Add(new CellSetting(height, 120, nameof(ReminderSummaryHistory.ReminderAmount), dataField: nameof(ReminderSummaryHistory.ReminderAmount), caption: "滞留金額", cell: builder.GetNumberCell()));
            builder.Items.Add(new CellSetting(height, 380, nameof(ReminderSummaryHistory.Memo), dataField: nameof(ReminderHistory.Memo), caption: "対応記録", cell: memoCell));
            builder.Items.Add(new CellSetting(height, 80, nameof(ReminderSummaryHistory.CreateByName), dataField: nameof(ReminderSummaryHistory.CreateByName), caption: "更新者名", cell: builder.GetTextBoxCell(middleCenter)));
            gridHistory.Template = builder.Build();
            gridHistory.HideSelection = true;
            gridReminder.AllowAutoExtend = false;
        }
        #endregion

        #region Function Key Event
        [OperationLog("検索")]
        private void Search()
        {
            SearchReminder();
        }

        private void SearchReminder()
        {
            try
            {
                ClearStatusMessage();

                var loadTask = new List<Task>();
                loadTask.Add(LoadReminderCommonSetting());
                loadTask.Add(LoadReminderSummarySetting());
                loadTask.Add(LoadCurrencyAsync(UseForeignCurrency ? txtCurrencyCode.Text : DefaultCurrencyCode));
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                if (!ValidateSearchData()) return;
                SearchCondition = GetSearchDataCondition();
                int count = 0;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    count = await SearchReminderSummaryAsync(SearchCondition);
                });
                NLogHandler.WriteDebug(this, "督促データ 検索開始");
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                NLogHandler.WriteDebug(this, "督促データ 検索終了");

                if (count == 0)
                    ShowWarningDialog(MsgWngNotExistSearchData);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>検索項目チェック</summary>
        /// <returns>検索チェック結果（bool）</returns>
        private bool ValidateSearchData()
        {
            if (ReminderCommonSetting == null)
            {
                ShowWarningDialog(MsgWngNotSettingMaster, "督促共通設定");
                return false;
            }

            if (ReminderSummarySetting == null || ReminderSummarySetting.Count() == 0)
            {
                ShowWarningDialog(MsgWngNotSettingMaster, "督促集計設定");
                return false;
            }

            if (!datCalculateBaseDate.Value.HasValue)
            {
                tbcReminder.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, lblCalculateBaseDate.Text);
                datCalculateBaseDate.Focus();
                return false;
            }

            if (!txtFromArrearDays.ValidateRange(txtToArrearDays,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblArrearDays.Text))) return false;

            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                tbcReminder.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text);
                txtCurrencyCode.Focus();
                return false;
            }

            if (!txtFromCustomerCode.ValidateRange(txtToCustomerCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomer.Text))) return false;

            if (ReminderCommonSetting.ReminderManagementMode == (int)ReminderManagementMode.ByCustomer)
            {
                pnlHistory.Visible = true;
                lblReminderHistory.Visible = true;
                //SetHistoryPanelHeight();
            }
            else
            {
                pnlHistory.Visible = false;
                lblReminderHistory.Visible = false;
            }

            return true;
        }

        private void SetHistoryPanelHeight()
        {
            var dif = Height - DefaultScreenHeight;
            pnlHistory.Height = DefaultPanelHeight + dif;
        }

        private async Task LoadReminderCommonSetting()
        {
            ReminderCommonSetting setting = null;
            await ServiceProxyFactory.DoAsync<ReminderSettingServiceClient>(async client =>
            {
                var result = await client.GetReminderCommonSettingAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    setting = result.ReminderCommonSetting;
                ReminderCommonSetting = setting;
            });
        }
        private async Task LoadReminderSummarySetting()
        {
            var setting = new List<ReminderSummarySetting>();
            await ServiceProxyFactory.DoAsync<ReminderSettingServiceClient>(async client =>
            {
                var result = await client.GetReminderSummarySettingsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    setting = result.ReminderSummarySettings;
                ReminderSummarySetting = setting;
            });
        }

        private async Task LoadCurrencyAsync(string currencyCode)
        {
            var currency = new Currency();
            await ServiceProxyFactory.DoAsync<CurrencyMasterClient>(async client =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { currencyCode });
                if (result.ProcessResult.Result)
                    currency = result.Currencies.First();
                Currency = currency;
            });
        }

        /// <summary>検索データ</summary>
        /// <returns>ArrearagesListSearch</returns>
        private ReminderSearch GetSearchDataCondition()
        {
            var reminderSearch = new ReminderSearch();

            reminderSearch.CompanyId = CompanyId;
            reminderSearch.CalculateBaseDate = datCalculateBaseDate.Value.Value;
            reminderSearch.ContainReminderAmountZero = cbxContainReminderAmountZero.Checked;
            reminderSearch.RemoveExcludeReminderPublishCustomer = cbxRemoveExcludeReminderPublishCustomer.Checked;
            reminderSearch.CurrencyCode = UseForeignCurrency ? reminderSearch.CurrencyCode = txtCurrencyCode.Text : Constants.DefaultCurrencyCode;
            
            if (!string.IsNullOrWhiteSpace(txtFromCustomerCode.Text))
                reminderSearch.CustomerCodeFrom = txtFromCustomerCode.Text;
            if (!string.IsNullOrWhiteSpace(txtToCustomerCode.Text))
                reminderSearch.CustomerCodeTo = txtToCustomerCode.Text;
            if (!string.IsNullOrEmpty(txtCustomerName.Text))
                reminderSearch.CustomerName = txtCustomerName.Text;

            reminderSearch.ArrearDaysFrom = (int?)txtFromArrearDays.Value;
            reminderSearch.ArrearDaysTo = (int?)txtToArrearDays.Value;

            reminderSearch.ReminderManaged = true;

            return reminderSearch;
        }

        private async Task<int> SearchReminderSummaryAsync(ReminderSearch reminderSearch)
        {
            var result = new ReminderSummaryResult();
            int count = 0;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var reminderClient = factory.Create<ReminderServiceClient>();
                result = await reminderClient.GetSummaryItemsAsync(SessionKey, SearchCondition, ReminderCommonSetting);

                var settingClient = factory.Create<ReminderSettingServiceClient>();
                var levelResult = await settingClient.GetReminderLevelSettingsAsync(SessionKey, CompanyId);
                
                if (result.ProcessResult.Result && result.ReminderSummary.Count > 0)
                {
                    InitializeReminderGrid();
                    var reminders = result.ReminderSummary;
                    gridReminder.DataSource = new BindingSource(reminders, null);
                    lblRemainAmountTotal.Text = result.ReminderSummary.Sum(x => x.RemainAmount).ToString(GetNumberFormat());
                    lblReminderAmountTotal.Text = result.ReminderSummary.Sum(x => x.ReminderAmount).ToString(GetNumberFormat());
                    tbcReminder.SelectedTab = tabReminderResult;
                    if (ReminderCommonSetting.ReminderManagementMode == (int)ReminderManagementMode.ByCustomer)
                        BaseContext.SetFunction03Enabled(true);

                    BaseContext.SetFunction04Enabled(true);
                    BaseContext.SetFunction06Enabled(true);
                    BaseContext.SetFunction07Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                    count = result.ReminderSummary.Count;
                    gridReminder.Select();
                }
                else
                {
                    tbcReminder.SelectedTab = tabReminderSearch;
                    gridReminder.DataSource = null;
                    gridHistory.DataSource = null;
                    lblDispCustomerCode.Clear();
                    lblDispCustomerName.Clear();
                    lblRemainAmountTotal.Clear();
                    lblReminderAmountTotal.Clear();
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction06Enabled(false);
                    BaseContext.SetFunction07Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
            });
            return count;
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearControls();
        }

        private void ClearControls()
        {
            ClearStatusMessage();
            datCalculateBaseDate.Value = DateTime.Today;
            cbxContainReminderAmountZero.Checked = false;
            cbxRemoveExcludeReminderPublishCustomer.Checked = false;
            txtFromArrearDays.Clear();
            txtToArrearDays.Clear();
            txtCurrencyCode.Clear();
            txtFromCustomerCode.Clear();
            lblFromCustomerName.Clear();
            txtToCustomerCode.Clear();
            lblToCustomerName.Clear();
            txtCustomerName.Clear();
            lblRemainAmountTotal.Clear();
            lblReminderAmountTotal.Clear();
            gridReminder.DataSource = null;
            gridHistory.DataSource = null;
            tbcReminder.SelectedTab = tabReminderSearch;
            datCalculateBaseDate.Select();
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            lblDispCustomerCode.Clear();
            lblDispCustomerName.Clear();

            this.ActiveControl = datCalculateBaseDate;
        }

        /// <summary>ReportSettingデータ取得処理</summary>
        private async Task SetReportSetting()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReportSettingMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId, nameof(PF0401));

                if (result.ProcessResult.Result)
                {
                    ReportSettingList = result.ReportSettings;
                }
            });
        }

        [OperationLog("一括変更")]
        private void StatusChangeAltogether()
        {
            ClearStatusMessage();

            if (!ValidateAnyCheck())
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "更新するデータ");
                return;
            }

            var Reminder = gridReminder.Rows.Select(x => x.DataBoundItem as ReminderSummary).Where(x => x.Checked);
            StatusChange(Reminder);
        }

        private bool ValidateAnyCheck()
        {
            if (!gridReminder.Rows.Any(x => Convert.ToBoolean(x[CellName(nameof(ReminderSummary.Checked))].Value)))
            {
                return false;
            }
            return true;
        }

        private void StatusChange(IEnumerable<ReminderSummary> reminders)
        {
            using (var form = ApplicationContext.Create(nameof(PI0202)))
            {
                var screen = form.GetAll<PI0202>().First();
                screen.InitializeUserComponent(Currency.Precision, reminders.Count() == 1 ? reminders.First() : null);
                screen.InitializeParentForm("対応記録");
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                foreach (var r in reminders)
                {
                    if (screen.UpdateMemo)
                        r.Memo = screen.Memo;
                    else
                        r.Memo = r.Memo;
                }
            }

            var success = false;

            ProgressDialog.Start(ParentForm, async (cancel, progress) =>
            {
                try
                {
                    await ServiceProxyFactory.DoAsync<ReminderServiceClient>(async client =>
                    {
                        var result = await client.UpdateSummaryStatusAsync(SessionKey, Login.UserId, reminders.ToArray());
                        if (result.ProcessResult.Result)
                        {
                            success = true;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
            }, false, SessionKey);

            if (success)
            {
                tbcReminder.SelectedIndex = 1;
                DisplaySelectedCustomer(gridReminder.CurrentCell.RowIndex);
                DispStatusMessage(MsgInfUpdateSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrUpdateError);
            }
        }


        private void HistoryChange(ReminderSummaryHistory reminderSummaryHistory)
        {
            using (var form = ApplicationContext.Create(nameof(PI0206)))
            {
                var screen = form.GetAll<PI0206>().First();
                screen.InitializeUserComponent(Currency.Precision, reminderSummaryHistory);
                screen.InitializeParentForm("対応記録");
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (dialogResult != DialogResult.OK) return;

                if (screen.UpdateMemo)
                    reminderSummaryHistory.Memo = screen.txtMemo.Text;

                reminderSummaryHistory.IsUpdateSummaryMemo = IsUpdateReminderSummaryMemo(reminderSummaryHistory);

                if (!screen.IsDelete && screen.UpdateMemo)
                    UpdateSummaryHistory(reminderSummaryHistory);
                else if (screen.IsDelete)
                    DeleteSummaryHistory(reminderSummaryHistory);

                if (reminderSummaryHistory.IsUpdateSummaryMemo)
                    UpdateReminderSummaryMemo(reminderSummaryHistory, screen.IsDelete);
            }
        }

        private void UpdateSummaryHistory(ReminderSummaryHistory reminderSummaryHistory)
        {
            var success = false;

            Task task = UpdateReminderSummaryHistoryAsync(reminderSummaryHistory)
            .ContinueWith(async t =>
            {
                if (t.Result.ProcessResult.Result)
                {
                    var result = await GetReminderSummaryHistoriesAsync(reminderSummaryHistory.ReminderSummaryId);
                    success = result.ProcessResult.Result;
                    gridHistory.DataSource = result.ReminderSummaryHistories;
                }

            }, TaskScheduler.FromCurrentSynchronizationContext())
            .Unwrap();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (success)
                DispStatusMessage(MsgInfUpdateSuccess);
            else
                ShowWarningDialog(MsgErrUpdateError);
        }

        private void DeleteSummaryHistory(ReminderSummaryHistory reminderSummaryHistory)
        {
            var success = false;

            Task task = DeleteSummaryHistoryAsync(reminderSummaryHistory)
            .ContinueWith(async t =>
            {
                if (t.Result.ProcessResult.Result)
                {
                    var result = await GetReminderSummaryHistoriesAsync(reminderSummaryHistory.ReminderSummaryId);
                    success = result.ProcessResult.Result;
                    gridHistory.DataSource = result.ReminderSummaryHistories;
                }

            }, TaskScheduler.FromCurrentSynchronizationContext())
            .Unwrap();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (success)
                DispStatusMessage(MsgInfDeleteSuccess);
            else
                ShowWarningDialog(MsgErrDeleteError);
        }

        private bool IsUpdateReminderSummaryMemo(ReminderSummaryHistory reminderSummaryHistory)
        {
            var source = gridHistory?.DataSource as List<ReminderSummaryHistory>;
            if (source == null) return false;

            return reminderSummaryHistory.CreateAt == source.FirstOrDefault().CreateAt;
        }

        private void UpdateReminderSummaryMemo(ReminderSummaryHistory reminderSummaryHistory, bool isDelete)
        {
            var rsSource = (gridReminder.DataSource as BindingSource)?.DataSource as List<ReminderSummary> ?? null;
            if (rsSource == null) return;

            var reminderSummary = rsSource.Where(x => x.Id == reminderSummaryHistory.ReminderSummaryId).FirstOrDefault();

            if (isDelete)
            {
                var rshSource = gridHistory?.DataSource as List<ReminderSummaryHistory>;
                reminderSummary.Memo = rshSource.Any() ? rshSource.FirstOrDefault().Memo : string.Empty;
            }
            else
            {
                reminderSummary.Memo = reminderSummaryHistory.Memo;
            }
        }

        [OperationLog("督促状発行")]
        private void Print()
        {
            ClearStatusMessage();
            ProgressDialog.Start(ParentForm, LoadReminderTemplateSetting(), false, SessionKey);
            if (!ValidateInputValueForPrint()) return;

            using (var form = ApplicationContext.Create(nameof(PI0203)))
            {
                var screen = form.GetAll<PI0203>().First();
                var reminderSummary = gridReminder.Rows.Select(x => x.DataBoundItem as ReminderSummary).Where(x => x.Checked);

                if (ReminderSummarySetting.Any(x => x.ColumnName == "DestinationCode" && x.Available == 1))
                {
                    SearchCondition.CustomerCodeFrom = reminderSummary.Min(x => x.CustomerCode);
                    SearchCondition.CustomerCodeTo = reminderSummary.Max(x => x.CustomerCode);

                    var task = GetSummaryItemsByDestinationAsync(SearchCondition, ReminderCommonSetting);
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    reminderSummary = task.Result.ReminderSummary;
                }

                screen.ReminderSummary = reminderSummary;
                screen.ReminderCommonSetting = ReminderCommonSetting;
                screen.ReminderSummarySetting = ReminderSummarySetting;
                screen.ReminderTemplateSetting = ReminderTemplateSetting;
                ApplicationContext.ShowDialog(ParentForm, form);

                if (form.DialogResult == DialogResult.OK)
                {
                    DispStatusMessage(MsgInfOutputUnreadData, "選択したデータ");
                }
            }
        }

        private async Task LoadReminderTemplateSetting()
        {
            await ServiceProxyFactory.DoAsync<ReminderSettingServiceClient>(async client =>
            {
                var result = await client.GetReminderTemplateSettingsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    ReminderTemplateSetting = result.ReminderTemplateSettings;
                }
            });
        }

        private bool ValidateInputValueForPrint()
        {
            if (ReminderTemplateSetting?.Count == 0)
            {
                ShowWarningDialog(MsgWngNotSettingMaster, "督促状文面設定");
                return false;
            }
            if (!ValidateAnyCheck())
            {
                ShowWarningDialog(MsgWngSelectionRequired, "督促状を発行するデータ");
                return false;
            }
            if (gridReminder.Rows.Select(x => x.DataBoundItem as ReminderSummary).Any(x => x.Checked && x.ReminderAmount == 0))
            {
                ShowWarningDialog(MsgWngNotPrintReminder);
                return false;
            }
            foreach (var row in gridReminder.Rows)
            {
                var item = row.DataBoundItem as ReminderSummary;
                if (item.Checked && item.ExcludeReminderPublish == 1)
                {
                    ShowWarningDialog(MsgWngExcludeReminderCannotPublish);
                    gridReminder.Select();
                    gridReminder.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(ReminderSummary.Checked)));
                    row.Selected = true;
                    return false;
                }
            }
            return true;
        }

        /// <summary>Get Server Path </summary>
        /// <returns>Server Path</returns>
        private async Task<string> GetServerPath()
        {
            string serverPath = string.Empty;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var pathService = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await pathService.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });

            return serverPath;
        }

        private async Task SaveOutputAtReminder(List<ReminderOutputed> reminderOutpued)
        {
            await ServiceProxyFactory.DoAsync<ReminderServiceClient>(async client =>
            {
                var countResult = await client.UpdateReminderOutputedAsync(SessionKey, Login.UserId, reminderOutpued.ToArray());

                if (!countResult.ProcessResult.Result || countResult.Count <= 0)
                {
                    DispStatusMessage(MsgErrUpdateError);
                }
            });
        }

        [OperationLog("発行履歴")]
        private void PrintHistory()
        {
            try
            {
                var form = ApplicationContext.Create(nameof(PI0204));
                var screen = form.GetAll<PI0204>().FirstOrDefault();

                form.StartPosition = FormStartPosition.CenterParent;
                ApplicationContext.ShowDialog(ParentForm, form);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();
                var reminderList = gridReminder.Rows.Select(x => x.DataBoundItem as ReminderSummary).ToList();
                string serverPath = string.Empty;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    serverPath = await GetServerPath();
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"督促データ管理{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new ReminderManagementFileDifinition(new DataExpression(ApplicationControl));
                definition.RemainAmountField.Format = value => value.ToString("0." + new string('0', Precision));
                definition.ReminderAmountField.Format = value => value.ToString("0." + new string('0', Precision));

                if (ReminderCommonSetting.ReminderManagementMode == (int)ReminderManagementMode.ByReminder)
                    definition.MemoField.Ignored = true;

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                if (!UseForeignCurrency)
                {
                    definition.CurrencyCodeField.Ignored = true;
                }

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, reminderList, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                DispStatusMessage(MsgInfFinishExport);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        [OperationLog("消込台帳")]
        private void CustomerLedger()
        {
            if (gridReminder.CurrentRow == null) return;

            var r = gridReminder.CurrentRow.DataBoundItem as ReminderSummary;
            Customer customer = null;
            ServiceProxyFactory.Do<CustomerMasterClient>(client =>
            {
                var result = client.Get(SessionKey, new int[] { r.CustomerId });

                if (result.ProcessResult.Result)
                    customer = result.Customers.First();
            });

            if (customer == null)
            {
                return;
            }

            using (var form = ApplicationContext.Create(nameof(PF0501)))
            {
                var screen = form.GetAll<PF0501>().First();
                screen.Customer = customer;
                ApplicationContext.ShowDialog(ParentForm, form);
            }
        }

        [OperationLog("全選択")]
        private void SelectAll()
        {
            gridReminder.EndEdit();
            gridReminder.Focus();
            AllCheckedChange(true);
        }

        [OperationLog("全解除")]
        private void DeselectAll()
        {
            gridReminder.EndEdit();
            gridReminder.Focus();
            AllCheckedChange(false);
        }

        private void AllCheckedChange(bool check)
        {
            foreach (var row in gridReminder.Rows)
            {
                var reminder = row.DataBoundItem as ReminderSummary;
                reminder.Checked = check;
            }
        }

        [OperationLog("終了")]
        private void Exit()
        {
            try
            {
                Settings.SaveControlValue<PF0401>(Login, cbxCustomer.Name, cbxCustomer.Checked);
                ParentForm.Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void ReturnToSearchCondition()
        {
            tbcReminder.SelectedIndex = 0;
        }
#endregion

        #region ControlEventHandler

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var currency = await GetCurrencyInfo();

                        if (currency == null)
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                            txtCurrencyCode.Focus();
                            txtCurrencyCode.Clear();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code.ToString();
                Precision = currency.Precision;
                ClearStatusMessage();
            }
        }

        private void txtFromCustomerCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var fromCustomerCode = txtFromCustomerCode.Text;
                var fromCustomerName = string.Empty;

                if (!string.IsNullOrEmpty(fromCustomerCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CustomerMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { fromCustomerCode });
                        var customerResult = result.Customers.FirstOrDefault();

                        if (customerResult != null)
                        {
                            fromCustomerCode = customerResult.Code;
                            fromCustomerName = customerResult.Name;
                            ClearStatusMessage();
                        }

                        txtFromCustomerCode.Text = fromCustomerCode;
                        lblFromCustomerName.Text = fromCustomerName;

                        if (cbxCustomer.Checked)
                        {
                            txtToCustomerCode.Text = fromCustomerCode;
                            lblToCustomerName.Text = fromCustomerName;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    if (cbxCustomer.Checked)
                    {
                        txtToCustomerCode.Text = fromCustomerCode;
                        lblToCustomerName.Clear();
                    }
                    lblFromCustomerName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnFromCustomer_Click(object sender, EventArgs e)
        {
            var department = this.ShowCustomerSearchDialog();
            if (department != null)
            {
                txtFromCustomerCode.Text = department.Code;
                lblFromCustomerName.Text = department.Name;
                if (cbxCustomer.Checked)
                {
                    txtToCustomerCode.Text = department.Code;
                    lblToCustomerName.Text = department.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnToCustomer_Click(object sender, EventArgs e)
        {
            var department = this.ShowCustomerSearchDialog();
            if (department != null)
            {
                txtToCustomerCode.Text = department.Code;
                lblToCustomerName.Text = department.Name;
                ClearStatusMessage();
            }
        }

        private void txtToCustomerCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var toCustomerCode = txtToCustomerCode.Text;
                var toCustomerName = string.Empty;

                if (!string.IsNullOrEmpty(toCustomerCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CustomerMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { toCustomerCode });
                        var departmentResult = result.Customers.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            toCustomerCode = departmentResult.Code;
                            toCustomerName = departmentResult.Name;
                            ClearStatusMessage();
                        }
                        txtToCustomerCode.Text = toCustomerCode;
                        lblToCustomerName.Text = toCustomerName;
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtToCustomerCode.Text = toCustomerCode;
                    lblToCustomerName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region GridEventHandler

        private void grdReminder_CellDoubleClick(object sender, CellEventArgs e)
        {
            if ((e.RowIndex < 0) ||
                (e.CellName == CellName(nameof(ReminderSummary.Checked)))) return;

            var isCustomer = e.CellName == CellName(nameof(ReminderSummary.CustomerCode)) ||
                             e.CellName == CellName(nameof(ReminderSummary.CustomerName));

            if (!isCustomer && ReminderCommonSetting.ReminderManagementMode == (int)ReminderManagementMode.ByCustomer)
            {
                var data = gridReminder.Rows[e.RowIndex].DataBoundItem as ReminderSummary;
                if (data == null) return;

                var list = new List<ReminderSummary>() { data };
                StatusChange(list);
            }

            if (isCustomer)
            {
                var reminderSummary = (ReminderSummary)gridReminder.Rows[e.RowIndex].DataBoundItem;

                string code = reminderSummary.CustomerCode;
                var form = ApplicationContext.Create(nameof(PB0501));
                var screen = form.GetAll<PB0501>().FirstOrDefault();
                form.StartPosition = FormStartPosition.CenterParent;
                screen.CustomerCode = code;
                screen.ReturnScreen = this;

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form);

                if (dialogResult != DialogResult.OK) return;

                SearchCondition = GetSearchDataCondition();

                txtFromCustomerCode_Validated(null, null);
                txtToCustomerCode_Validated(null, null);

                var task = SearchReminderSummaryAsync(SearchCondition);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (task.Result == 0)
                    ShowWarningDialog(MsgWngNotExistSearchData);
            }
        }

        private void grdHistory_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0 || ReminderCommonSetting.ReminderManagementMode != (int)ReminderManagementMode.ByCustomer) return;

            var data = gridHistory.Rows[e.RowIndex].DataBoundItem as ReminderSummaryHistory;
            if (data == null) return;

            if (data.InputType != (int)ReminderSummaryHistory.ReminderSummaryHistoryInputType.StatusChange)
            {
                ShowWarningDialog(MsgWngEditableOnlyManualInput, "手入力履歴");
                return;
            }

            ClearStatusMessage();
            HistoryChange(data);
        }

        private void grdReminder_CellClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.CellName == CellName("Detail"))
            {
                ClearStatusMessage();
                using (var form = ApplicationContext.Create(nameof(PI0205)))
                {
                    var screen = form.GetAll<PI0205>().First();
                    var reminderSummary = gridReminder.Rows[e.RowIndex].DataBoundItem as ReminderSummary;
                    screen.CustomerId = reminderSummary.CustomerId;
                    screen.CalculateBaseDate = (DateTime)datCalculateBaseDate.Value;
                    screen.CurrencyCode = UseForeignCurrency ? txtCurrencyCode.Text : DefaultCurrencyCode;
                    screen.ArrearDaysFrom = (int?)txtFromArrearDays.Value;
                    screen.ArrearDaysTo = (int?)txtToArrearDays.Value;
                    ApplicationContext.ShowDialog(ParentForm, form);

                    if (screen.IsCustomerEdit)
                    {
                        var task = SearchReminderSummaryAsync(SearchCondition);
                        ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    }
                }
            }
        }

        private void grdReminder_RowEnter(object sender, CellEventArgs e)
        {
            DisplaySelectedCustomer(e.RowIndex);
        }

        private void DisplaySelectedCustomer(int rowIndex)
        {
            var data = gridReminder.Rows[rowIndex].DataBoundItem as ReminderSummary;
            if (data == null || data.Id <= 0)
            {
                lblDispCustomerCode.Clear();
                lblDispCustomerName.Clear();
                gridHistory.DataSource = null;
                return;
            }

            lblDispCustomerCode.Text = data.CustomerCode;
            lblDispCustomerName.Text = data.CustomerName;

            ServiceProxyFactory.Do<ReminderServiceClient>(client =>
            {
                var result = client.GetSummaryHistoryItemsByReminderSummaryId(SessionKey, data.Id);

                if (result.ProcessResult.Result)
                {
                    gridHistory.DataSource = result.ReminderSummaryHistories;
                }
            });
        }

        #endregion

        #region webservice

        private async Task<ReminderSummaryHistoryResult> UpdateReminderSummaryHistoryAsync(ReminderSummaryHistory reminderSummaryHistory)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.UpdateReminderSummaryHistoryAsync(SessionKey, reminderSummaryHistory);
                return result;
            });

        private async Task<ReminderSummaryHistoriesResult> GetReminderSummaryHistoriesAsync(int reminderSummaryId)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetSummaryHistoryItemsByReminderSummaryIdAsync(SessionKey, reminderSummaryId);
                return result;
            });

        private async Task<CountResult> DeleteSummaryHistoryAsync(ReminderSummaryHistory reminderSummaryHistory)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.DeleteSummaryHistoryAsync(SessionKey, reminderSummaryHistory);
                return result;
            });

        private async Task<ReminderSummaryResult> GetSummaryItemsByDestinationAsync(ReminderSearch search, ReminderCommonSetting setting)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetSummaryItemsByDestinationAsync(SessionKey, search, setting);
                return result;
            });

        #endregion

        /// <summary>回収予定金額合計形式</summary>
        public string GetNumberFormat()
        {
            var displayFieldString = "#,###,###,###,##0";
            if (Precision > 0)
            {
                displayFieldString += "." + new string('0', Precision);
            }
            return displayFieldString;
        }

    }
}
