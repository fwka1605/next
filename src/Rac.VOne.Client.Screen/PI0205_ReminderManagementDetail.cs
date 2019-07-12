using GrapeCity.ActiveReports.Document.Section;
using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DestinationMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.PdfOutputSettingMasterService;
using Rac.VOne.Client.Screen.ReminderService;
using Rac.VOne.Client.Screen.ReminderSettingService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.StatusMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
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
    /// <summary>督促データ管理 督促データ一覧</summary>
    public partial class PI0205 : VOneScreenBase
    {
        // 変数宣言
        private int Precision { get; set; }
        List<ReportSetting> ReportSettingList { get; set; }
        private List<ColumnNameSetting> ColumnNameSettingInfo { get; set; }
        private ReminderSearch SearchCondition { get; set; }
        private string CellName(string value) => $"cel{value}";
        private ReminderCommonSetting ReminderCommonSetting { get; set; }
        private List<ReminderSummarySetting> ReminderSummarySetting { get; set; }
        private Currency Currency { get; set; }
        public int CustomerId { get; set; }
        public DateTime CalculateBaseDate { get; set; }
        public string CurrencyCode { get; set; }
        public int? ArrearDaysFrom { get; set; }
        public int? ArrearDaysTo { get; set; }
        private Customer Customer { get; set; }
        public bool IsCustomerEdit { get; set; }
        private bool UseDestinationSummarized { get; set; }
        private bool IsCellEmpty(Cell cell)
            => (cell.Value == null
             || string.IsNullOrEmpty(cell.Value.ToString())
             || cell.Name == CellName(nameof(Reminder.DestinationCode)) && 0.Equals(cell.Value));
        private PdfOutputSetting PdfSetting { get; set; }
        public PI0205()
        {
            InitializeComponent();
            grdReminder.SetupShortcutKeys();
            Text = "督促データ管理 督促データ一覧";
            ReportSettingList = new List<ReportSetting>();
            grdReminder.DataBindingComplete += grdReminder_DataBindingComplete;
        }

        #region PF0401 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("");
            BaseContext.SetFunction01Enabled(false);

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);

            BaseContext.SetFunction03Caption("一括変更");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = StatusChangeAltogether;

            BaseContext.SetFunction04Caption("督促状発行");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = Print;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(true);
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(true);
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption("戻る");
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
                loadTask.Add(LoadCustomerAsync(CustomerId));
                loadTask.Add(LoadCurrencyAsync(CurrencyCode));
                
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SuspendLayout();
                ResumeLayout();
                SetInitialSetting();
                InitializeHistoryGrid();
                ClearControls();
                grdReminder.Text = "";
                SearchReminder();
                datPaymentDate.Value = CalculateBaseDate;
                SetCustomerValue();
                IsCustomerEdit = false;
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

        private async Task LoadCustomerAsync(int customerId)
        {
            await ServiceProxyFactory.DoAsync<CustomerMasterClient>(async client =>
            {
                var result = await client.GetAsync(SessionKey, new int[] { customerId });

                if (result.ProcessResult.Result)
                {
                    Customer = result.Customers.First();
                }
            });
        }

        private async Task LoadCurrencyAsync(string currencyCode)
        {
            await ServiceProxyFactory.DoAsync<CurrencyMasterClient>(async client =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { currencyCode });

                if (result.ProcessResult.Result)
                {
                    Currency = result.Currencies.First();
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

                txtCustomerCode.Format = expression.CustomerCodeFormatString;
                txtCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;
            }

            ServiceProxyFactory.Do<ReminderSettingServiceClient>(client =>
            {
                var result = client.GetReminderTemplateSettings(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    cmbReminderTemplate.TextSubItemIndex = 0;
                    cmbReminderTemplate.ValueSubItemIndex = 1;
                    cmbReminderTemplate.Items.AddRange(result.ReminderTemplateSettings.Select(x =>
                        new ListItem(x.Name, x.Id)).ToArray());
                }
            });
        }

        #endregion

        #region グリッド作成
        private void InitializeReminderGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var widthDay = 115;
            var widthCcy = UseForeignCurrency ? 60 : 0;

            var baseDateCaption = ReminderCommonSetting.BaseDateCaption;

            var reminderAmountCell = builder.GetNumberCellCurrency(Precision, Precision, 0);
            reminderAmountCell.AllowDeleteToNull = true;

            var arrearsDaysCell = builder.GetNumberCell();
            arrearsDaysCell.AllowDeleteToNull = true;

            builder.Items.Add(new CellSetting(height,  30, nameof(Reminder.Checked),      dataField: nameof(Reminder.Checked),      caption: "選択",        cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false));
            builder.Items.Add(new CellSetting(height, 115, nameof(Reminder.CustomerCode), dataField: nameof(Reminder.CustomerCode), caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 150, nameof(Reminder.CustomerName), dataField: nameof(Reminder.CustomerName), caption: "得意先名",    cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, widthDay, nameof(Reminder.CalculateBaseDate), dataField: nameof(Reminder.CalculateBaseDate), caption: baseDateCaption, cell: builder.GetDateCell_yyyyMMdd(), sortable: true));
            builder.Items.Add(new CellSetting(height, 70, nameof(Reminder.DetailCount), dataField: nameof(Reminder.DetailCount), caption: "明細件数", cell: builder.GetNumberCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, widthCcy, nameof(Reminder.CurrencyCode), dataField: nameof(Reminder.CurrencyCode), caption: "通貨コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(Reminder.RemainAmount), dataField: nameof(Reminder.RemainAmount), caption: "請求残", cell: builder.GetNumberCellCurrency(Precision, Precision, 0), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(Reminder.ReminderAmount), dataField: nameof(Reminder.ReminderAmount), caption: "滞留金額", cell: reminderAmountCell, sortable: true));
            if (ReminderCommonSetting.DisplayArrearsInterest == 1)
                builder.Items.Add(new CellSetting(height, 80, nameof(Reminder.ArrearsInterest), dataField: nameof(Reminder.ArrearsInterest), caption: "延滞利息", cell: builder.GetNumberCell(), sortable: true));
            builder.Items.Add(new CellSetting(height,  70, nameof(Reminder.ArrearsDays), dataField: nameof(Reminder.ArrearsDays), caption: "滞留日数", cell: arrearsDaysCell, sortable: true));
            builder.Items.Add(new CellSetting(height, 100, nameof(Reminder.StatusCodeAndName), dataField: nameof(Reminder.StatusCodeAndName), caption: "ステータス", cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, 240, nameof(Reminder.Memo), dataField: nameof(Reminder.Memo), caption: "対応記録", cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, 0, nameof(Reminder.Id), dataField: nameof(Reminder.Id)));
            if (ReminderSummarySetting != null)
            {
                if (ReminderSummarySetting.Any(x => x.ColumnName == "ClosingAt" && x.Available == 1))
                    builder.Items.Add(new CellSetting(height, widthDay, nameof(Reminder.ClosingAt), dataField: nameof(Reminder.ClosingAt), caption: "請求締日", cell: builder.GetDateCell_yyyyMMdd(), sortable: true));
                if (ReminderSummarySetting.Any(x => x.ColumnName == "InvoiceCode" && x.Available == 1))
                    builder.Items.Add(new CellSetting(height, widthDay, nameof(Reminder.InvoiceCode), dataField: nameof(Reminder.InvoiceCode), caption: "請求書番号", cell: builder.GetTextBoxCell(), sortable: true));
                if (ReminderSummarySetting.Any(x => x.ColumnName == "CollectCategory" && x.Available == 1))
                    builder.Items.Add(new CellSetting(height, widthDay, nameof(Reminder.CollectCategoryCodeAndName), dataField: nameof(Reminder.CollectCategoryCodeAndName), caption: "回収区分", cell: builder.GetTextBoxCell(), sortable: true));
                if (ReminderSummarySetting.Any(x => x.ColumnName == "Department" && x.Available == 1))
                {
                    builder.Items.Add(new CellSetting(height, 115, nameof(Reminder.DepartmentCode), dataField: nameof(Reminder.DepartmentCode), caption: "請求部門コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
                    builder.Items.Add(new CellSetting(height, 150, nameof(Reminder.DepartmentName), dataField: nameof(Reminder.DepartmentName), caption: "請求部門名", cell: builder.GetTextBoxCell(), sortable: true));
                }
                if (ReminderSummarySetting.Any(x => x.ColumnName == "Staff" && x.Available == 1))
                {
                    builder.Items.Add(new CellSetting(height, 115, nameof(Reminder.StaffCode), dataField: nameof(Reminder.StaffCode), caption: "担当者コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
                    builder.Items.Add(new CellSetting(height, 150, nameof(Reminder.StaffName), dataField: nameof(Reminder.StaffName), caption: "担当者名", cell: builder.GetTextBoxCell(), sortable: true));
                }
                if (ReminderSummarySetting.Any(x => x.ColumnName == "DestinationCode" && x.Available == 1))
                {
                    builder.Items.Add(new CellSetting(height, 40, nameof(Reminder.DestinationCode), dataField: nameof(Reminder.DestinationCode), caption: "", cell: builder.GetTextBoxCell(middleCenter), sortable: true, readOnly: false));
                    builder.Items.Add(new CellSetting(height, 240, nameof(Reminder.DestinationDisplay), dataField: nameof(Reminder.DestinationDisplay), caption: "送付先", cell: builder.GetTextBoxCell()));
                }
            }
            //builder.Items.Add(new CellSetting(height, 120, nameof(Reminder.CustomerStaffName), caption: "相手先担当者", cell: builder.GetTextBoxCell(), sortable: true));
            //builder.Items.Add(new CellSetting(height, 120, nameof(Reminder.CustomerNote), caption: "得意先備考", cell: builder.GetTextBoxCell(), sortable: true));
            //builder.Items.Add(new CellSetting(height, 120, nameof(Reminder.CustomerTel), caption: "電話番号", cell: builder.GetTextBoxCell(), sortable: true));
            grdReminder.Template = builder.Build();
            grdReminder.HideSelection = true;
            grdReminder.AllowAutoExtend = false;
            grdReminder.FreezeLeftCellName = CellName(nameof(Reminder.Checked));

            grdReminder.EditMode = GrapeCity.Win.MultiRow.EditMode.EditOnKeystrokeOrShortcutKey;
            grdReminder.AllowClipboard = true;
            grdReminder.AllowDrop = true;
            grdReminder.MultiSelect = true;
            grdReminder.ShortcutKeyManager.Register(EditingActions.Paste, Keys.Control | Keys.V);
            grdReminder.ShortcutKeyManager.Register(EditingActions.Clear, Keys.Delete);
            grdReminder.ShortcutKeyManager.Register(EditingActions.Clear, Keys.Back);

        }

        private void InitializeHistoryGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;

            var memoCell = builder.GetTextBoxCell();
            memoCell.Style.Multiline = MultiRowTriState.True;
            memoCell.Style.WordWrap = MultiRowTriState.True;

            builder.Items.Add(new CellSetting(height, 140, nameof(ReminderHistory.CreateAt), dataField: nameof(ReminderHistory.CreateAt), caption: "更新日時", cell: builder.GetDateCell_yyyyMMddHHmmss(MultiRowContentAlignment.MiddleCenter)));
            builder.Items.Add(new CellSetting(height, 100, nameof(ReminderHistory.InputTypeName), dataField: nameof(ReminderHistory.InputTypeName), caption: "アクション", cell: builder.GetTextBoxCell()));
            builder.Items.Add(new CellSetting(height, 180, nameof(ReminderHistory.StatusCodeAndName), dataField: nameof(ReminderHistory.StatusCodeAndName), caption: "ステータス", cell: builder.GetTextBoxCell()));
            builder.Items.Add(new CellSetting(height, 120, nameof(ReminderHistory.ReminderAmount), dataField: nameof(ReminderHistory.ReminderAmount), caption: "滞留金額", cell: builder.GetNumberCellCurrency(Precision, Precision, 0)));
            builder.Items.Add(new CellSetting(height, 250, nameof(ReminderHistory.Memo), dataField: nameof(ReminderHistory.Memo), caption: "対応記録", cell: memoCell));
            builder.Items.Add(new CellSetting(height, 60, nameof(ReminderHistory.OutputFlagName), dataField: nameof(ReminderHistory.OutputFlagName), caption: "督促区分", cell: builder.GetTextBoxCell(middleCenter)));
            builder.Items.Add(new CellSetting(height, 85, nameof(ReminderHistory.CreateByName), dataField: nameof(ReminderHistory.CreateByName), caption: "更新者名", cell: builder.GetTextBoxCell(middleCenter)));
            builder.Items.Add(new CellSetting(height,  0, nameof(ReminderHistory.StatusId), dataField: nameof(ReminderHistory.StatusId)));
            grdHistory.Template = builder.Build();
            grdHistory.HideSelection = true;
            grdHistory.AllowAutoExtend = false;
        }
        #endregion

        #region Function Key Event
        private void SearchReminder()
        {
            try
            {
                ClearStatusMessage();

                var loadTask = new List<Task>();
                loadTask.Add(LoadReminderCommonSetting());
                loadTask.Add(LoadReminderSummarySetting());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                UseDestinationSummarized = ReminderSummarySetting.Any(x => x.ColumnName == "DestinationCode" && x.Available == 1);

                //if (!ValidateSearchData()) return;
                SearchCondition = GetSearchDataCondition();
                int count = 0;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    count = await SearchReminderAsync(SearchCondition);
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

        /// <summary>検索データ</summary>
        /// <returns>ArrearagesListSearch</returns>
        private ReminderSearch GetSearchDataCondition()
        {
            var reminderSearch = new ReminderSearch();

            reminderSearch.CompanyId = CompanyId;
            reminderSearch.CalculateBaseDate = CalculateBaseDate;
            reminderSearch.CurrencyCode = UseForeignCurrency ? reminderSearch.CurrencyCode = CurrencyCode : Constants.DefaultCurrencyCode;
            
            reminderSearch.CustomerCodeFrom = Customer.Code;
            reminderSearch.CustomerCodeTo = Customer.Code;
            reminderSearch.ArrearDaysFrom = ArrearDaysFrom;
            reminderSearch.ArrearDaysTo = ArrearDaysTo;
            reminderSearch.ReminderManaged = true;

            reminderSearch.AssignmentFlg = (int)AssignmentFlagChecked.NoAssignment | (int)AssignmentFlagChecked.PartAssignment;

            return reminderSearch;
        }

        private async Task<int> SearchReminderAsync(ReminderSearch reminderSearch)
        {
            var result = new ReminderResult();
            int count = 0;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var reminderClient = factory.Create<ReminderServiceClient>();
                result = await reminderClient.GetItemsAsync(SessionKey, SearchCondition, ReminderCommonSetting, ReminderSummarySetting.ToArray());

                var settingClient = factory.Create<ReminderSettingServiceClient>();
                var levelResult = await settingClient.GetReminderLevelSettingsAsync(SessionKey, CompanyId);
                
                if (result.ProcessResult.Result && result.Reminder.Count > 0)
                {
                    var reminders = result.Reminder;
                    if (levelResult.ProcessResult.Result && levelResult.ReminderLevelSettings.Count > 0)
                    {
                        var levelSetting = levelResult.ReminderLevelSettings;
                        var maxArrearsDay = reminders.Max(x => x.ArrearsDays);
                        var level = levelSetting.LastOrDefault(x => maxArrearsDay >= x.ArrearDays);
                        if (level != null) cmbReminderTemplate.SelectedValue = level.ReminderTemplateId;
                    }

                    if (ReminderCommonSetting.DisplayArrearsInterest == 1)
                    {
                        var rate = (decimal)ReminderCommonSetting.ArrearsInterestRate;
                        foreach(var r in reminders)
                        {
                            if (r.Id > 0)
                            {
                                var interest = (decimal)Amount.Calc(RoundingType.Round, (decimal)r.ReminderAmount * (decimal)r.ArrearsDays * rate / 100 / 365, Precision);
                                r.ArrearsInterest = interest;
                            }
                            else
                            {
                                r.ArrearsInterest = null;
                            }
                        }
                    }

                    InitializeReminderGrid();
                    grdReminder.DataSource = new BindingSource(reminders, null);
                    lblRemainAmountTotal.Text = result.Reminder.Sum(x => x.RemainAmount).ToString(GetNumberFormat());
                    lblReminderAmountTotal.Text = result.Reminder.Sum(x => x.ReminderAmount ?? 0).ToString(GetNumberFormat());
                    BaseContext.SetFunction03Enabled(true);

                    var excludeReminderPublish = reminders.Any(x => x.ExcludeReminderPublish == 1);
                    BaseContext.SetFunction04Enabled(!excludeReminderPublish);

                    BaseContext.SetFunction06Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                    count = result.Reminder.Count;
                }
                else
                {
                    grdReminder.DataSource = null;
                    grdHistory.DataSource = null;
                    lblRemainAmountTotal.Clear();
                    lblReminderAmountTotal.Clear();
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction06Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
            });
            return count;
        }

        private void ClearControls()
        {
            ClearStatusMessage();
            datPaymentDate.Value = DateTime.Today;
            txtCustomerCode.Clear();
            txtCustomerName.Clear();
            lblRemainAmountTotal.Clear();
            lblReminderAmountTotal.Clear();
            grdReminder.DataSource = null;
            grdHistory.DataSource = null;
            datPaymentDate.Select();

            this.ActiveControl = datPaymentDate;
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

            var Reminder = grdReminder.Rows.Select(x => x.DataBoundItem as Reminder).Where(x => x.Checked);
            StatusChange(Reminder);
        }

        private bool ValidateAnyCheck()
        {
            if (!grdReminder.Rows.Any(x => Convert.ToBoolean(x[CellName(nameof(Reminder.Checked))].Value)))
            {
                return false;
            }
            return true;
        }

        private void StatusChange(IEnumerable<Reminder> reminders)
        {
            using (var form = ApplicationContext.Create(nameof(PI0202)))
            {
                var screen = form.GetAll<PI0202>().First();
                screen.InitializeUserComponent(Currency.Precision, ReminderCommonSetting.CalculateBaseDate, reminders.Count() == 1 ? reminders.First() : null);

                var statusList = new List<Status>();
                ServiceProxyFactory.Do<StatusMasterClient>(client =>
                {
                    var result = client.GetStatusesByStatusType(SessionKey, CompanyId, (int)Constants.StatusType.Reminder);
                    if (result.ProcessResult.Result) statusList = result.Statuses;
                });

                screen.ComboBoxInitializer = cmb =>
                {
                    if (statusList.Count > 0)
                    {
                        foreach (var s in statusList)
                        {
                            cmb.Items.Add(new ListItem(s.Name, s.Id));
                        }
                        cmb.ValueSubItemIndex = 1;
                        cmb.TextSubItemIndex = 0;
                    }
                    if (reminders.Count() == 1) cmb.SelectedValue = reminders.First().StatusId;
                };
                screen.InitializeParentForm("対応記録");
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (dialogResult != DialogResult.OK)
                {
                    return;
                }

                foreach (var r in reminders)
                {
                    if (screen.UpdateStatus)
                        r.StatusId = screen.StatusId;

                    if (screen.UpdateMemo)
                        r.Memo = screen.Memo;
                }
            }

            var success = false;

            ProgressDialog.Start(ParentForm, async (cancel, progress) =>
            {
                try
                {
                    await ServiceProxyFactory.DoAsync<ReminderServiceClient>(async client =>
                    {
                        var result = await client.UpdateStatusAsync(SessionKey, Login.UserId, reminders.ToArray());
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
                grdReminder.DataSource = null;
                SearchReminder();
                DispStatusMessage(MsgInfUpdateSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrUpdateError);
            }
        }

        private void HistoryChange(ReminderHistory reminderHistory)
        {
            using (var form = ApplicationContext.Create(nameof(PI0206)))
            {
                var screen = form.GetAll<PI0206>().First();
                screen.InitializeUserComponent(Currency.Precision, reminderHistory);

                var statusList = new List<Status>();
                ServiceProxyFactory.Do<StatusMasterClient>(client =>
                {
                    var result = client.GetStatusesByStatusType(SessionKey, CompanyId, (int)Constants.StatusType.Reminder);
                    if (result.ProcessResult.Result) statusList = result.Statuses;
                });

                screen.ComboBoxInitializer = cmb =>
                {
                    if (statusList.Any())
                    {
                        foreach (var status in statusList)
                            cmb.Items.Add(new ListItem(status.Name, status.Id));

                        cmb.ValueSubItemIndex = 1;
                        cmb.TextSubItemIndex = 0;
                    }
                    cmb.SelectedValue = reminderHistory.StatusId;
                };

                screen.InitializeParentForm("対応記録");
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (dialogResult != DialogResult.OK) return;

                if (screen.UpdateStatus)
                    reminderHistory.StatusId = (int)screen.cmbStatus.SelectedValue;

                if (screen.UpdateMemo)
                    reminderHistory.Memo = screen.txtMemo.Text;

                reminderHistory.IsUpdateStatusMemo = IsUpdateReminderStatusAndMemo(reminderHistory);

                if (!screen.IsDelete && (screen.UpdateStatus || screen.UpdateMemo))
                    UpdateHistory(reminderHistory);
                else if (screen.IsDelete)
                    DeleteHistory(reminderHistory);

                if (reminderHistory.IsUpdateStatusMemo)
                    UpdateReminderListDisplay();
            }
        }

        private void UpdateHistory(ReminderHistory reminderHistory)
        {
            var success = false;

            Task task = UpdateReminderHistoryAsync(reminderHistory)
            .ContinueWith(async t =>
            {
                if (t.Result.ProcessResult.Result)
                {
                    var result = await GetReminderHistoriesAsync(reminderHistory.ReminderId);
                    success = result.ProcessResult.Result;
                    grdHistory.DataSource = result.ReminderHistories;
                }

            }, TaskScheduler.FromCurrentSynchronizationContext())
            .Unwrap();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (success)
                DispStatusMessage(MsgInfUpdateSuccess);
            else
                ShowWarningDialog(MsgErrUpdateError);
        }

        private void DeleteHistory(ReminderHistory reminderHistory)
        {
            var success = false;

            Task task = DeleteHistoryAsync(reminderHistory)
            .ContinueWith(async t =>
            {
                if (t.Result.ProcessResult.Result)
                {
                    var result = await GetReminderHistoriesAsync(reminderHistory.ReminderId);
                    success = result.ProcessResult.Result;
                    grdHistory.DataSource = result.ReminderHistories;
                }

            }, TaskScheduler.FromCurrentSynchronizationContext())
            .Unwrap();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (success)
                DispStatusMessage(MsgInfDeleteSuccess);
            else
                ShowWarningDialog(MsgErrDeleteError);
        }

        private bool IsUpdateReminderStatusAndMemo(ReminderHistory reminderHistory)
        {
            var source = grdHistory?.DataSource as List<ReminderHistory>;
            if (source == null) return false;

            return reminderHistory.CreateAt == source.FirstOrDefault().CreateAt;
        }

        private void UpdateReminderListDisplay()
        {
            var task = GetReminderItemsAsync();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Result.ProcessResult.Result && task.Result.Reminder.Any())
            {
                var reminders = task.Result.Reminder;
                grdReminder.DataSource = null;
                grdReminder.DataSource = new BindingSource(reminders, null);
            }
        }

        [OperationLog("督促状発行")]
        private void Print()
        {
            ClearStatusMessage();
            if (!ValidateInputValueForPrint()) return;

            var path = string.Empty;
            ProgressDialog.Start(ParentForm, Task.Run(() =>
            {
                path = Util.GetGeneralSettingServerPathAsync(Login).Result;
            }), false, SessionKey);

            if (!Directory.Exists(path))
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            PdfSetting = GetPdfOutputSetting();
            if (!PdfSetting.IsAllInOne)
            {
                var selectedPath = string.Empty;
                var rootBrowserPath = new List<string>();
                if (!LimitAccessFolder ?
                    !ShowFolderBrowserDialog(path, out selectedPath) :
                    !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, Constants.FolderBrowserType.SelectFolder))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                path = (!LimitAccessFolder ? selectedPath : rootBrowserPath.First());
            }

            if (!PdfSetting.IsAllInOne && !ShowConfirmDialog(MsgQstConfirmOutput))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                Task task;
                if (!UseDestinationSummarized)
                    task = PrintReminderDetails(path);
                else
                    task = PrintDestinationSummarized(path);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private async Task PrintReminderDetails(string path)
        {
            var report = new ReminderReport();
            var reminderOutputed = new List<ReminderOutputed>();
            var reminders = grdReminder.Rows.Select(x => x.DataBoundItem as Reminder)
                .Where(x => x.Checked);

            if (!reminders.Any())
                ShowWarningDialog(MsgWngPrintDataNotExist, "印刷");

            var reminderIds = reminders.Select(x => x.Id).Distinct().ToArray();
            var now = DateTime.Now;

            List<ReminderBilling> reminderBilling = null;
            List<ReminderTemplateSetting> templateSettings = null;
            var outputNo = 1;

            var reminderResult = await GetReminderBillingForPrintAsync(reminderIds);
            if (reminderResult.ProcessResult.Result) reminderBilling = reminderResult.ReminderBilling;

            var countResult = await GetMaxOutputNoAsync();
            if (countResult.ProcessResult.Result) outputNo = countResult.Count + 1;

            var templateSettingResult = await GetReminderTemplateSettingsAsync();
            if (templateSettingResult.ProcessResult.Result) templateSettings = templateSettingResult.ReminderTemplateSettings;

            var template = templateSettings.First(x => x.Id == (int)cmbReminderTemplate.SelectedValue);

            foreach (var b in reminderBilling)
            {
                var output = new ReminderOutputed();
                output.OutputNo = outputNo;
                output.BillingId = b.Id;
                output.ReminderId = b.ReminderId;
                output.RemainAmount = b.RemainAmount;
                output.BillingAmount = b.BillingAmount;
                output.ReminderTemplateId = (int)cmbReminderTemplate.SelectedValue;
                output.OutputAt = now;
                reminderOutputed.Add(output);

                b.OutputNo = outputNo;
            }

            report = UtilReminder.CreateReminderReport(reminderBilling,
                ReminderCommonSetting,
                ReminderSummarySetting,
                template,
                Company,
                ColumnNameSettingInfo,
                now,
                PdfSetting);

            CountResult outputResult = null;
            if (PdfSetting.IsAllInOne)
            {
                Action<Form> outputHandler = owner =>
                {
                    var taskOutput = SaveOutputAtReminder(reminderOutputed);
                    ProgressDialog.Start(owner, taskOutput, false, SessionKey);
                    outputResult = taskOutput.Result;
                };
                ShowDialogPreview(ParentForm, report, path, outputHandler);
            }
            else
            {
                var filePath = Util.GetUniqueFileName(Path.Combine(path, $"{report.Name}.pdf"));
                var exporter = new PdfReportExporter();
                exporter.PdfExport(report, filePath);
                if (PdfSetting.UseZip)
                {
                    Util.ArchivesAsZip(new List<string> { filePath },
                    path,
                    $"督促状{now.ToString("yyyyMMdd")}",
                    PdfSetting.MaximumByte);
                }
                outputResult = await SaveOutputAtReminder(reminderOutputed);
            }

            if (PdfSetting.IsAllInOne && outputResult == null)
                return;
            else if (!outputResult.ProcessResult.Result || outputResult.Count <= 0)
                ShowWarningDialog(MsgErrExportError);
            else
                DispStatusMessage(MsgInfOutputUnreadData, "選択したデータ");
        }

        private async Task PrintDestinationSummarized(string path)
        {
            var reminders = grdReminder.Rows.Select(x => x.DataBoundItem as Reminder)
                .Where(x => x.Checked);

            if (!reminders.Any())
                ShowWarningDialog(MsgWngPrintDataNotExist, "印刷");

            var option = new DestinationSearch();
            option.CompanyId = CompanyId;
            option.CustomerId = CustomerId;

            var destinations = await GetDestinationsAsync(option);

            foreach (var reminder in reminders)
            {
                if (reminder.DestinationCode == null || string.IsNullOrEmpty(reminder.DestinationCode))
                {
                    reminder.NoDestination = true;
                    continue;
                }

                var destination = destinations.FirstOrDefault(x => x.Code == reminder.DestinationCode);
                if (destination == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "送付先", reminder.DestinationCode);
                    return;
                }
                reminder.DestinationIdInput = destination.Id;
            }

            var reminderList = new List<Reminder>();
            var gDestination = reminders.GroupBy(x => new { x.CustomerCode, x.DestinationCode }).Select(x => x);

            foreach (var grs in gDestination)
            {
                var reminder = grs.First();
                var rIds = new List<int>();
                var destinationIds = new List<int>();
                foreach (var item in reminders.Where(x => x.CustomerId == reminder.CustomerId && x.DestinationCode == reminder.DestinationCode))
                {
                    if (item.DestinationId == null)
                        reminder.NoDestination = true;
                    else
                        destinationIds.Add(item.DestinationId.Value);
                    rIds.Add(item.Id);
                }
                reminder.CompanyId = CompanyId;
                reminder.Ids = rIds.ToArray();
                reminder.DestinationIds = destinationIds.ToArray();
                reminderList.Add(reminder);
            }

            var rbResult = false;
            var noResult = false;
            var tsResult = false;

            var reminderBilling = new List<ReminderBilling>();
            var reminderTemplateSettings = new List<ReminderTemplateSetting>();
            var outputNo = 1;

            await GetReminderBillingForPrintByDestinationCodeAsync(reminderList)
                .ContinueWith(async t =>
                {
                    rbResult = t.Result.ProcessResult.Result;
                    if (rbResult)
                    {
                        reminderBilling = t.Result.ReminderBilling;
                        var countResult = await GetMaxOutputNoAsync();
                        noResult = countResult.ProcessResult.Result;
                        if (noResult)
                        {
                            outputNo = countResult.Count + 1;
                            var templateSettings = await GetReminderTemplateSettingsAsync();
                            tsResult = templateSettings.ProcessResult.Result;
                            if (tsResult)
                            {
                                reminderTemplateSettings = templateSettings.ReminderTemplateSettings;
                            }
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext())
                .Unwrap();

            if (!rbResult || !noResult || !tsResult || string.IsNullOrEmpty(path))
            {
                ShowWarningDialog(MsgErrExportError);
                return;
            }

            var reminderOutputed = new List<ReminderOutputed>();
            var reportList = new List<ReminderReport>();
            var now = DateTime.Now;
            var template = reminderTemplateSettings.First(x => x.Id == (int)cmbReminderTemplate.SelectedValue);
            ReminderReport allReport = null;

            foreach (var gdr in gDestination)
            {
                var r = gdr.First();
                var source = reminderBilling.Where(x => x.CustomerId == r.CustomerId && x.DestinationId == r.DestinationIdInput);

                foreach (var b in source)
                {
                    var output = new ReminderOutputed();
                    output.OutputNo = outputNo;
                    output.BillingId = b.Id;
                    output.ReminderId = b.ReminderId;
                    output.RemainAmount = b.RemainAmount;
                    output.BillingAmount = b.BillingAmount;
                    output.ReminderTemplateId = (int)cmbReminderTemplate.SelectedValue;
                    output.OutputAt = now;
                    output.DestinationId = r.DestinationIdInput;
                    reminderOutputed.Add(output);

                    b.OutputNo = outputNo;
                }

                var report = UtilReminder.CreateReminderReport(source,
                    ReminderCommonSetting,
                    ReminderSummarySetting,
                    template,
                    Company,
                    ColumnNameSettingInfo,
                    now,
                    PdfSetting);

                if (PdfSetting.IsAllInOne)
                {
                    if (allReport == null)
                        allReport = report;
                    else
                        allReport.Document.Pages.AddRange((PagesCollection)report.Document.Pages.Clone());
                }
                else
                    reportList.Add(report);

                outputNo++;
            }

            CountResult outputResult = null;
            if (PdfSetting.IsAllInOne)
            {
                Action<Form> outputHandler = owner =>
                {
                    var taskOutput = SaveOutputAtReminder(reminderOutputed);
                    ProgressDialog.Start(owner, taskOutput, false, SessionKey);
                    outputResult = taskOutput.Result;
                };
                ShowDialogPreview(ParentForm, allReport, path, outputHandler);
            }
            else
            {
                var fileList = new List<string>();
                foreach (var rpt in reportList)
                {
                    var filePath = Util.GetUniqueFileName(Path.Combine(path, $"{rpt.Name}.pdf"));
                    var exporter = new PdfReportExporter();
                    exporter.PdfExport(rpt, filePath);
                    if (PdfSetting.UseZip)
                        fileList.Add(filePath);
                }
                if (PdfSetting.UseZip)
                    Util.ArchivesAsZip(fileList, path, $"督促状{now.ToString("yyyyMMdd")}", PdfSetting.MaximumByte);
                outputResult = await SaveOutputAtReminder(reminderOutputed);
            }

            if (PdfSetting.IsAllInOne && outputResult == null)
                return;
            else if (!outputResult.ProcessResult.Result || outputResult.Count <= 0)
                ShowWarningDialog(MsgErrExportError);
            else
                DispStatusMessage(MsgInfOutputUnreadData, "選択したデータ");
        }

        private bool ValidateInputValueForPrint()
        {
            if (!ValidateAnyCheck())
            {
                ShowWarningDialog(MsgWngSelectionRequired, "印刷するデータ");
                return false;
            }

            for(var i = 0; i < grdReminder.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(grdReminder.GetValue(i, CellName(nameof(Reminder.Checked))))) continue;

                if (cmbReminderTemplate.SelectedIndex == -1)
                {
                    ShowWarningDialog(MsgWngSelectionRequired, lblReminderTemplate.Text);
                    cmbReminderTemplate.Focus();
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

        private async Task<CountResult> SaveOutputAtReminder(List<ReminderOutputed> reminderOutpued)
        {
            CountResult result = null;
            await ServiceProxyFactory.DoAsync<ReminderServiceClient>(async client =>
            {
                result = await client.UpdateReminderOutputedAsync(SessionKey, Login.UserId, reminderOutpued.ToArray());

                if (!result.ProcessResult.Result || result.Count <= 0)
                {
                    DispStatusMessage(MsgErrUpdateError);
                }
            });
            return result;
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();
                var reminderList = grdReminder.Rows.Select(x => x.DataBoundItem as Reminder).ToList();
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
                var fileName = $"督促データ一覧{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new ReminderManagementDetailFileDifinition(new DataExpression(ApplicationControl), ReminderCommonSetting.BaseDateCaption);
                definition.RemainAmountField.Format = value => value.ToString("0." + new string('0', Precision));
                definition.ReminderAmountField.Format = value => value.ToString("0." + new string('0', Precision));

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                foreach (var r in reminderList)
                    r.BaseDate = (DateTime)datPaymentDate.Value;

                if (ReminderCommonSetting.DisplayArrearsInterest == 0)
                    definition.ArrearsInterestField.Ignored = true;

                if (ReminderSummarySetting != null)
                {
                    if (ReminderSummarySetting.Any(x => x.ColumnName == "ClosingAt" && x.Available == 0))
                        definition.ClosingAtField.Ignored = true;
                    if (ReminderSummarySetting.Any(x => x.ColumnName == "InvoiceCode" && x.Available == 0))
                        definition.InvoiceCodeField.Ignored = true;
                    if (ReminderSummarySetting.Any(x => x.ColumnName == "CollectCategory" && x.Available == 0))
                        definition.CollectCategoryField.Ignored = true;
                    if (ReminderSummarySetting.Any(x => x.ColumnName == "Department" && x.Available == 0))
                    {
                        definition.DepartmentCodeField.Ignored = true;
                        definition.DepartmentNameField.Ignored = true;
                    }
                    if (ReminderSummarySetting.Any(x => x.ColumnName == "Staff" && x.Available == 0))
                    {
                        definition.StaffCodeField.Ignored = true;
                        definition.StaffNameField.Ignored = true;
                    }
                    if (ReminderSummarySetting.Any(x => x.ColumnName == "DestinationCode" && x.Available == 0))
                        definition.DestinationCodeField.Ignored = true;
                }

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

        [OperationLog("全選択")]
        private void SelectAll()
        {
            grdReminder.EndEdit();
            grdReminder.Focus();
            AllCheckedChange(true);
        }

        [OperationLog("全解除")]
        private void DeselectAll()
        {
            grdReminder.EndEdit();
            grdReminder.Focus();
            AllCheckedChange(false);
        }

        private void AllCheckedChange(bool check)
        {
            foreach (var row in grdReminder.Rows)
            {
                var reminder = row.DataBoundItem as Reminder;
                if (row.Cells[CellName(nameof(Reminder.Checked))].Enabled)
                    reminder.Checked = check;
            }
        }

        [OperationLog("戻る")]
        private void Exit()
        {
            ParentForm.Close();
        }

        #endregion

        #region GridEventHandler

        private void grdReminder_CellDoubleClick(object sender, CellEventArgs e)
        {
            if ((e.RowIndex < 0) ||
                (e.CellName == CellName(nameof(Reminder.Checked))) ||
                (e.CellName == CellName(nameof(Reminder.DestinationCode)))) return;

            var isCustomer = e.CellName == CellName(nameof(Reminder.CustomerCode)) ||
                             e.CellName == CellName(nameof(Reminder.CustomerName));

            if (!isCustomer)
            {
                var data = grdReminder.Rows[e.RowIndex].DataBoundItem as Reminder;
                if (data == null || data.Id <= 0) return;

                var list = new List<Reminder>() { data };
                StatusChange(list);
            }
            else if (isCustomer)
            {
                var reminder = (Reminder)grdReminder.Rows[e.RowIndex].DataBoundItem;

                string code = reminder.CustomerCode;
                var form = ApplicationContext.Create(nameof(PB0501));
                var screen = form.GetAll<PB0501>().FirstOrDefault();
                form.StartPosition = FormStartPosition.CenterParent;
                screen.CustomerCode = code;
                screen.ReturnScreen = this;

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form);

                if (dialogResult != DialogResult.OK) return;

                IsCustomerEdit = true;

                Task task = SearchReminderAsync(SearchCondition)
                .ContinueWith(async t =>
                {
                    if (t.Result == 0)
                    {
                        SetCustomerValue(doClear : true);
                        ShowWarningDialog(MsgWngNotExistSearchData);
                    }
                    else
                    {
                        await LoadCustomerAsync(CustomerId);
                        SetCustomerValue();
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext())
                .Unwrap();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

            }
        }

        private void grdReminder_CellClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var data = grdReminder.Rows[e.RowIndex].DataBoundItem as Reminder;
            if (data == null || data.Id <= 0)
            {
                grdHistory.DataSource = null;
            }
            else
            {
                ServiceProxyFactory.Do<ReminderServiceClient>(client =>
                {
                    var result = client.GetHistoryItemsByReminderId(SessionKey, data.Id);

                    if (result.ProcessResult.Result)
                        grdHistory.DataSource = result.ReminderHistories;
                });
            }
        }

        private void grdReminder_DataBindingComplete(object sender, MultiRowBindingCompleteEventArgs e)
        {
            foreach (var r in grdReminder.Rows)
            {
                var reminder = r.DataBoundItem as Reminder;
                if (reminder.Id <= 0)
                {
                    r[CellName(nameof(Reminder.Checked))].Enabled = false;
                }
            }
        }

        private void grdHistory_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0 || ReminderCommonSetting.ReminderManagementMode != (int)ReminderManagementMode.ByReminder) return;

            var data = grdHistory.Rows[e.RowIndex].DataBoundItem as ReminderHistory;
            if (data == null) return;

            if (data.InputType != (int)ReminderHistory.ReminderHistoryInputType.StatusChange)
            {
                ShowWarningDialog(MsgWngEditableOnlyManualInput, "手入力履歴");
                return;
            }

            ClearStatusMessage();
            HistoryChange(data);
        }

        #endregion

        private void SetCustomerValue(bool doClear = false)
        {
            txtCustomerCode.Text      = doClear ? string.Empty : Customer?.Code;
            txtCustomerName.Text      = doClear ? string.Empty : Customer?.Name;
            txtCustomerStaffName.Text = doClear ? string.Empty : Customer?.CustomerStaffName;
            txtTel.Text               = doClear ? string.Empty : Customer?.Tel;
            txtFax.Text               = doClear ? string.Empty : Customer?.Fax;
            txtCustomerNote.Text      = doClear ? string.Empty : Customer?.Note;
        }

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

        #region webservice

        private async Task<ReminderHistoryResult> UpdateReminderHistoryAsync(ReminderHistory reminderHistory)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.UpdateReminderHistoryAsync(SessionKey, reminderHistory);
                return result;
            });

        private async Task<ReminderHistoriesResult> GetReminderHistoriesAsync(int reminderId)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetHistoryItemsByReminderIdAsync(SessionKey, reminderId);
                return result;
            });

        private async Task<CountResult> DeleteHistoryAsync(ReminderHistory reminderHistory)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) => 
            {
                var result = await client.DeleteHistoryAsync(SessionKey, reminderHistory);
                return result;
            });

        private async Task<ReminderResult> GetReminderItemsAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, SearchCondition, ReminderCommonSetting, ReminderSummarySetting.ToArray());
                return result;
            });

        private async Task<List<Destination>> GetDestinationsAsync(DestinationSearch option) =>
            await ServiceProxyFactory.DoAsync(async (DestinationMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Destinations;
                return new List<Destination>();
            });

        private async Task<ReminderBillingResult> GetReminderBillingForPrintByDestinationCodeAsync(IEnumerable<Reminder> reminders)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetReminderBillingForPrintByDestinationCodeAsync(SessionKey, reminders.ToArray());
                return result;
            });

        private async Task<ReminderBillingResult> GetReminderBillingForPrintAsync(int[] reminderIds)
           => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
           {
               var result = await client.GetReminderBillingForPrintAsync(SessionKey, CompanyId, reminderIds);
               return result;
           });


     

        private async Task<CountResult> GetMaxOutputNoAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetMaxOutputNoAsync(SessionKey, CompanyId);
                return result;
            });

        private async Task<ReminderTemplateSettingsResult> GetReminderTemplateSettingsAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderSettingServiceClient client) =>
            {
                var result = await client.GetReminderTemplateSettingsAsync(SessionKey, CompanyId);
                return result;
            });
        private PdfOutputSetting GetPdfOutputSetting() =>
        ServiceProxyFactory.Do((PdfOutputSettingMasterClient client) =>
        {
            var result = client.Get(
                    SessionKey,
                    Login.CompanyId,
                    (int)PdfOutputSettingReportType.Reminder,
                    Login.UserId);

            if (result == null || result.ProcessResult == null || !result.ProcessResult.Result)
                return null;
            return result.PdfOutputSetting;
        });
        #endregion

        private void grdReminder_ClipboardOperating(object sender, ClipboardOperationEventArgs e)
        {
            if (e.ClipboardOperation != ClipboardOperation.Paste) return;

            if (grdReminder.SelectedCells.Any(x => x.Name != CellName(nameof(Reminder.DestinationCode))))
            {
                e.Handled = true;
                return;
            }

            var copyCode = string.Empty;
            int code;

            if (Clipboard.ContainsText())
                copyCode = Clipboard.GetText();

            if (!int.TryParse(copyCode, out code)) return;

            var destinationDisplay = GetDestinationDisplay(code.ToString().PadLeft(2, '0'));

            foreach (var cell in grdReminder.SelectedCells)
            {
                cell.Value = code.ToString().PadLeft(2, '0');
                grdReminder.Rows[cell.RowIndex].Cells[CellName(nameof(Reminder.DestinationDisplay))].Value = destinationDisplay;
            }
            e.Handled = true;
        }

        private void grdReminder_CellValidated(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.CellName != CellName(nameof(Reminder.DestinationCode))) return;

            grdReminder.EndEdit();
            var celDestinationCode = grdReminder.Rows[e.RowIndex].Cells[CellName(nameof(Reminder.DestinationCode))];
            if (IsCellEmpty(celDestinationCode))
            {
                celDestinationCode.Value = string.Empty;
                return;
            }
            celDestinationCode.Value = celDestinationCode.Value.ToString().PadLeft(2, '0');

            var destinationDisplay = GetDestinationDisplay(celDestinationCode.Value.ToString());
            grdReminder.Rows[e.RowIndex].Cells[CellName(nameof(Reminder.DestinationDisplay))].Value = destinationDisplay;
        }

        private void grdReminder_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.CellName != CellName(nameof(Reminder.DestinationCode))) return;

            var celDestinationCode = grdReminder.Rows[e.RowIndex].Cells[CellName(nameof(Reminder.DestinationCode))];
            if (!IsCellEmpty(celDestinationCode)) return;

            var destinationDisplay = GetCustomerDestinationDisplay();
            grdReminder.Rows[e.RowIndex].Cells[CellName(nameof(Reminder.DestinationDisplay))].Value = destinationDisplay;
        }

        private string GetDestinationDisplay(string code)
        {
            var destinationDisplay = string.Empty;

            var option = new DestinationSearch();
            option.CompanyId = CompanyId;
            option.CustomerId = CustomerId;
            option.Codes = new string[] { code };
            var task = GetDestinationsAsync(option);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (!task.Result.Any()) return destinationDisplay;

            var destination = task.Result.FirstOrDefault();
            var postalCode = !string.IsNullOrEmpty(destination.PostalCode) ? "〒" + destination.PostalCode : string.Empty;
            destinationDisplay = postalCode + " " + destination.Name + destination.Address1 + destination.Address2 + " " + destination.DepartmentName + " " + destination.Addressee + destination.Honorific;

            return destinationDisplay;
        }

        private string GetCustomerDestinationDisplay()
        {
            var destinationDisplay = string.Empty;
            var postalCode = !string.IsNullOrEmpty(Customer.PostalCode) ? "〒" + Customer.PostalCode : string.Empty;
            destinationDisplay = postalCode + " " + Customer.Name + Customer.Address1 + Customer.Address2 + " " + Customer.DestinationDepartmentName + " " + Customer.CustomerStaffName + Customer.Honorific;

            return destinationDisplay;
        }

    }
}
