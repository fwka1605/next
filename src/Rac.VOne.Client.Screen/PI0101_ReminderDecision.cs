using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.ReminderService;
using Rac.VOne.Client.Screen.ReminderSettingService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
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
    /// <summary>督促データ確定</summary>
    public partial class PI0101 : VOneScreenBase
    {
        // 変数宣言
        private int Precision { get; set; }
        List<ReportSetting> ReportSettingList { get; set; }
        List<object> SearchListCondition { get; set; }
        private List<ColumnNameSetting> ColumnSettingInfo { get; set; }
        private ArrearagesListSearch SearchConditionOld { get; set; }
        private ReminderSearch SearchCondition { get; set; }
        private string CellName(string value) => $"cel{value}";
        private ReminderCommonSetting ReminderCommonSetting { get; set; }
        private ReminderSummarySetting[] ReminderSummarySetting { get; set; }

        /// <summary>確定済データを表示</summary>
        private bool IsCancelReminder { get { return cbxShowCancelReminderData.Checked; } }

        public PI0101()
        {
            InitializeComponent();
            grdReminder.SetupShortcutKeys();
            Text = "督促データ確定";
            ReportSettingList = new List<ReportSetting>();
            InitializeHandlers();
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
            cbxShowCancelReminderData.CheckedChanged += cbxShowCancelReminderData_CheckedChanged;
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

            BaseContext.SetFunction03Caption("確定");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Decision;

            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);

            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);

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
        private void PI0101_Load(object sender, EventArgs e)
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
                    loadTask.Add(LoadCompanyAsync());
                if (ReminderCommonSetting == null)
                    loadTask.Add(LoadReminderCommonSetting());
                if (ReminderSummarySetting == null)
                    loadTask.Add(LoadReminderSummarySetting());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SuspendLayout();
                tbcReminder.SelectedIndex = 0;
                ResumeLayout();
                SetInitialSetting();
                //InitializeArrearagesListGrid();
                grdReminder.Text = "";
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
                    ColumnSettingInfo = result.ColumnNames.Where(x => x.TableName == "Billing").ToList();
                    
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

                txtFromDepartmentCode.Format = expression.DepartmentCodeFormatString;
                txtFromDepartmentCode.MaxLength = expression.DepartmentCodeLength;
                txtFromDepartmentCode.PaddingChar = expression.DepartmentCodePaddingChar;

                txtToDapartmentCode.Format = expression.DepartmentCodeFormatString;
                txtToDapartmentCode.MaxLength = expression.DepartmentCodeLength;
                txtToDapartmentCode.PaddingChar = expression.DepartmentCodePaddingChar;

                txtFromStaffCode.Format = expression.StaffCodeFormatString;
                txtFromStaffCode.MaxLength = expression.StaffCodeLength;
                txtFromStaffCode.PaddingChar = expression.StaffCodePaddingChar;

                txtToStaffCode.Format = expression.StaffCodeFormatString;
                txtToStaffCode.MaxLength = expression.StaffCodeLength;
                txtToStaffCode.PaddingChar = expression.StaffCodePaddingChar;

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
            }

            Settings.SetCheckBoxValue<PF0401>(Login, cbxDepartment);
            Settings.SetCheckBoxValue<PF0401>(Login, cbxStaff);
            Settings.SetCheckBoxValue<PF0401>(Login, cbxCustomer);

            datPaymentDate.Value = DateTime.Today;
            this.ActiveControl = datPaymentDate;
            datPaymentDate.Focus();
        }

        private async Task<Currency> GetCurrencyInfo()
        {
            var currency = new Currency();
            BaseContext.SetFunction07Enabled(true);

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
        private void InitializeArrearagesListGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var widthDay = 115;
            var widthCcy = UseForeignCurrency ? 60 : 0;

            var baseDateCaption = "当初予定日";
            if (ReminderCommonSetting?.CalculateBaseDate == (int)CalculateBaseDate.DueAt)
                baseDateCaption = "入金予定日";
            if (ReminderCommonSetting?.CalculateBaseDate == (int)CalculateBaseDate.BilledAt)
                baseDateCaption = "請求日";

            builder.Items.Add(new CellSetting(height,  30, nameof(Reminder.Checked),      dataField: nameof(Reminder.Checked),      caption: "選択",        cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false));
            builder.Items.Add(new CellSetting(height, 115, nameof(Reminder.CustomerCode), dataField: nameof(Reminder.CustomerCode), caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 150, nameof(Reminder.CustomerName), dataField: nameof(Reminder.CustomerName), caption: "得意先名",    cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, widthDay, nameof(Reminder.CalculateBaseDate), dataField: nameof(Reminder.CalculateBaseDate), caption: baseDateCaption, cell: builder.GetDateCell_yyyyMMdd(), sortable: true));
            builder.Items.Add(new CellSetting(height, 70, nameof(Reminder.DetailCount), dataField: nameof(Reminder.DetailCount), caption: "明細件数", cell: builder.GetNumberCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, widthCcy, nameof(Reminder.CurrencyCode), dataField: nameof(Reminder.CurrencyCode), caption: "通貨コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(Reminder.RemainAmount), dataField: nameof(Reminder.RemainAmount), caption: "請求残", cell: builder.GetNumberCellCurrency(Precision, Precision, 0), sortable: true));
            //builder.Items.Add(new CellSetting(height, 120, nameof(Reminder.ReminderAmount), dataField: nameof(Reminder.ReminderAmount), caption: "滞留金額", cell: builder.GetNumberCellCurrency(Precision, Precision, 0), sortable: true));
            builder.Items.Add(new CellSetting(height,  70, nameof(Reminder.ArrearsDays), dataField: nameof(Reminder.ArrearsDays), caption: "滞留日数", cell: builder.GetNumberCell(), sortable: true));
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
                    builder.Items.Add(new CellSetting(height, 40, nameof(Reminder.DestinationCode), dataField: nameof(Reminder.DestinationCode), caption: "", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
                    builder.Items.Add(new CellSetting(height, 400, nameof(Reminder.DestinationDisplay), dataField: nameof(Reminder.DestinationDisplay), caption: "送付先", cell: builder.GetTextBoxCell()));
                }
            }
            grdReminder.Template = builder.Build();
            grdReminder.HideSelection = true;
            grdReminder.AllowAutoExtend = false;
            grdReminder.FreezeLeftCellName = CellName(nameof(Reminder.Checked));
        }
        #endregion

        #region Function Key Event
        [OperationLog("検索")]
        private void Search()
        {
            var count = SearchReminder();

            if (count == 0)
            {
                tbcReminder.SelectedTab = tabReminderSearch;
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
        }

        private int SearchReminder()
        {
            ClearStatusMessage();

            try
            {
                if (!ValidateSearchData()) return -1;
                SearchCondition = GetSearchDataCondition();
                int count = 0;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    count = await SearchReminderAsync(SearchCondition);
                });
                NLogHandler.WriteDebug(this, "督促データ 検索開始");
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                NLogHandler.WriteDebug(this, "督促データ 検索終了");

                return count;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrDataSearch);
                return -1;
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

            if (!datPaymentDate.Value.HasValue)
            {
                tbcReminder.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "入金基準日");
                datPaymentDate.Focus();
                return false;
            }

            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                tbcReminder.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Focus();
                return false;
            }

            if (!txtFromArrearDays.ValidateRange(txtToArrearDays,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblArrearDays.Text))) return false;

            if (!txtFromDepartmentCode.ValidateRange(txtToDapartmentCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblFromDepartment.Text))) return false;

            if (!txtFromStaffCode.ValidateRange(txtToStaffCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaff.Text))) return false;

            if (!txtFromCustomerCode.ValidateRange(txtToCustomerCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomer.Text))) return false;

            return true;
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
                ReminderSummarySetting = setting.ToArray();
            });
        }

        /// <summary>検索データ</summary>
        /// <returns>ArrearagesListSearch</returns>
        private ReminderSearch GetSearchDataCondition()
        {
            var reminderSearch = new ReminderSearch();

            reminderSearch.CompanyId = CompanyId;
            reminderSearch.CalculateBaseDate = datPaymentDate.Value.Value;

            if (cbxMemo.Checked)
                reminderSearch.ExistsMemo = true;
            if (!string.IsNullOrWhiteSpace(txtMemo.Text))
                reminderSearch.BillingMemo = txtMemo.Text;

            reminderSearch.CurrencyCode = UseForeignCurrency ? reminderSearch.CurrencyCode = txtCurrencyCode.Text : Constants.DefaultCurrencyCode;
            reminderSearch.ArrearDaysFrom = (int?)txtFromArrearDays.Value;
            reminderSearch.ArrearDaysTo = (int?)txtToArrearDays.Value;

            if (!string.IsNullOrWhiteSpace(txtFromDepartmentCode.Text))
                reminderSearch.DepartmentCodeFrom = txtFromDepartmentCode.Text;
            if (!string.IsNullOrWhiteSpace(txtToDapartmentCode.Text))
                reminderSearch.DepartmentCodeTo = txtToDapartmentCode.Text;
            if (!string.IsNullOrWhiteSpace(txtFromStaffCode.Text))
                reminderSearch.StaffCodeFrom = txtFromStaffCode.Text;
            if (!string.IsNullOrWhiteSpace(txtToStaffCode.Text))
                reminderSearch.StaffCodeTo = txtToStaffCode.Text;
            if (!string.IsNullOrWhiteSpace(txtFromCustomerCode.Text))
                reminderSearch.CustomerCodeFrom = txtFromCustomerCode.Text;
            if (!string.IsNullOrWhiteSpace(txtToCustomerCode.Text))
                reminderSearch.CustomerCodeTo = txtToCustomerCode.Text;

            return reminderSearch;
        }

        private async Task<int> SearchReminderAsync(ReminderSearch reminderSearch)
        {
            var result = new ReminderResult();
            decimal totalAmount = 0m;
            int count = 0;

            await ServiceProxyFactory.DoAsync<ReminderServiceClient>(async client =>
            {
                result = await client.GetItemsAsync(SessionKey, SearchCondition, ReminderCommonSetting, ReminderSummarySetting);
                grdReminder.DataSource = new BindingSource(result.Reminder, null);
                count = result.Reminder.Count;
                if (count > 0)
                {
                    InitializeArrearagesListGrid();
                    totalAmount = result.Reminder.Sum(x => x.RemainAmount);
                    lblRecoveryTotalAmount.Text = totalAmount.ToString(GetNumberFormat());
                    tbcReminder.SelectedTab = tabReminderResult;
                    cbxShowCancelReminderData.Enabled = false;
                    BaseContext.SetFunction03Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                }
                else
                {
                    grdReminder.DataSource = null;
                    lblRecoveryTotalAmount.Clear();
                    cbxShowCancelReminderData.Enabled = true;
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
            });
            return count;
        }

        [OperationLog("取消検索")]
        private void SearchCancelDecisionData()
        {
            var count = SearchCancelReminder();

            if (count == 0)
            {
                tbcReminder.SelectedTab = tabReminderSearch;
                ShowWarningDialog(MsgWngNotExistSearchData);
            }

        }

        private int SearchCancelReminder()
        {
            ClearStatusMessage();
            try
            {
                if (!ValidateSearchData()) return -1;
                SearchCondition = GetSearchDataCondition();
                decimal totalAmount = 0m;
                int count = 0;

                var task = GetCancelDecisionItemsAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (task.Result.ProcessResult.Result && task.Result.Reminder.Any())
                {
                    var reminders = task.Result.Reminder;
                    grdReminder.DataSource = null;
                    grdReminder.DataSource = new BindingSource(reminders, null);
                    InitializeArrearagesListGrid();
                    totalAmount = task.Result.Reminder.Sum(x => x.RemainAmount);
                    lblRecoveryTotalAmount.Text = totalAmount.ToString(GetNumberFormat());
                    tbcReminder.SelectedTab = tabReminderResult;
                    cbxShowCancelReminderData.Enabled = false;
                    BaseContext.SetFunction03Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                    count++;
                }
                else
                {
                    grdReminder.DataSource = null;
                    lblRecoveryTotalAmount.Clear();
                    cbxShowCancelReminderData.Enabled = true;
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
                return count;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrDataSearch);
                return -1;
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            datPaymentDate.Value = DateTime.Today;
            txtFromDepartmentCode.Clear();
            lblFromDepartmentName.Clear();
            cbxMemo.Checked = false;
            txtMemo.Clear();
            txtFromArrearDays.Clear();
            txtToArrearDays.Clear();
            txtToDapartmentCode.Clear();
            lblToDepartmentName.Clear();
            txtCurrencyCode.Clear();
            txtFromStaffCode.Clear();
            lblFromStaffName.Clear();
            txtToStaffCode.Clear();
            lblToStaffName.Clear();
            txtFromCustomerCode.Clear();
            lblFromCustomerName.Clear();
            txtToCustomerCode.Clear();
            lblToCustomerName.Clear();
            lblRecoveryTotalAmount.Clear();
            cbxMemo.Enabled = true;
            txtMemo.Enabled = true;
            grdReminder.DataSource = null;
            tbcReminder.SelectedTab = tabReminderSearch;
            datPaymentDate.Select();
            cbxShowCancelReminderData.Checked = false;
            cbxShowCancelReminderData.Enabled = true;
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
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

        [OperationLog("確定")]
        private void Decision()
        {
            ClearStatusMessage();

            if (!ValidateInputValue()) return;

            if (!ShowConfirmDialog(MsgQstConfirmUpdate, "")) return;

            var Reminder = grdReminder.Rows.Select(x => x.DataBoundItem as Reminder).Where(x => x.Checked);
            var success = false;

            ProgressDialog.Start(ParentForm, async (cancel, progress) =>
            {
                try
                {
                    await ServiceProxyFactory.DoAsync<ReminderServiceClient>(async client =>
                    {
                        var result = await client.CreateAsync(SessionKey, CompanyId, Login.UserId, ApplicationControl.UseForeignCurrency, Reminder.ToArray(), ReminderCommonSetting, ReminderSummarySetting);
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
                grdReminder.DataSource = null;
                SearchReminder();
                DispStatusMessage(MsgInfUpdateSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrUpdateError);
            }
        }

        private bool ValidateInputValue()
        {
            if (!grdReminder.Rows.Any(x => Convert.ToBoolean(x[CellName(nameof(Reminder.Checked))].Value)))
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "更新するデータ");
                return false;
            }
            return true;
        }

        [OperationLog("確定取消")]
        private void CancelReminder()
        {
            ClearStatusMessage();
            try
            {
                var reminders = grdReminder.Rows.Select(x => x.DataBoundItem as Reminder).Where(x => x.Checked);

                if (!reminders.Any())
                {
                    ShowWarningDialog(MsgWngSelectionRequired, "確定取消を行う明細");
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmForCancel)) return;

                var task = CancelRemindersAsync(reminders.ToArray());
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (task.Result.ProcessResult.Result && task.Result.Count > 0)
                {
                    SearchCancelReminder();
                    DispStatusMessage(MsgInfProcessFinish);
                }
                else
                    ShowWarningDialog(MsgErrSomethingError, "取消");

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "取消");
            }
        }

        [OperationLog("印刷")]
        private void Print()
        {
            ClearStatusMessage();

            try
            {
                ReportSearchCondition();

                GrapeCity.ActiveReports.SectionReport report = null;
                var arrearagesList = grdReminder.Rows.Select(x => x.DataBoundItem as ArrearagesList).ToList();
                string serverPath = string.Empty;

                if (arrearagesList.Any())
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        serverPath = await GetServerPath();
                        var divVal = 1;
                        if (!UseForeignCurrency)
                        {
                            switch (ReportSettingList[4].ItemKey)
                            {
                                case "1": divVal = 1000; break;
                                case "2": divVal = 10000; break;
                                case "3": divVal = 1000000; break;
                            }
                        }

                        for (var i = 0; i < arrearagesList.Count; i++)
                        {
                            decimal remainAmount = RoundingType.Ceil.Calc(arrearagesList[i].RemainAmount, Precision).Value;
                            if (Precision == 0)
                            {
                                arrearagesList[i].TotalAmount = decimal.Truncate(remainAmount / divVal);
                            }
                            else
                            {
                                arrearagesList[i].TotalAmount = remainAmount / divVal;
                            }
                        }

                        var searchConditionReport = new SearchConditionSectionReport();
                        searchConditionReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "滞留明細一覧表");
                        searchConditionReport.Name = "滞留明細一覧表" + DateTime.Today.ToString("yyyyMMdd");
                        searchConditionReport.SetPageDataSetting(SearchListCondition);

                        if (SearchConditionOld.CustomerSummaryFlag)
                        {
                            var arrearagesCustomerListReport = new ArrearagesCustomerListReport();
                            arrearagesCustomerListReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            arrearagesCustomerListReport.Name = "滞留明細一覧表" + DateTime.Today.ToString("yyyyMMdd");
                            arrearagesCustomerListReport.SetData(arrearagesList, Precision, ReportSettingList);

                            arrearagesCustomerListReport.Run(false);
                            searchConditionReport.SetPageNumber(arrearagesCustomerListReport.Document.Pages.Count);
                            searchConditionReport.Run(false);

                            arrearagesCustomerListReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchConditionReport.Document.Pages.Clone());
                            report = arrearagesCustomerListReport;
                        }
                        else
                        {
                            var arrearagesListReport = new ArrearagesListReport();
                            arrearagesListReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            arrearagesListReport.Name = "滞留明細一覧表" + DateTime.Today.ToString("yyyyMMdd");
                            arrearagesListReport.SetData(arrearagesList, Precision, ReportSettingList, ColumnSettingInfo);

                            arrearagesListReport.Run(false);
                            searchConditionReport.SetPageNumber(arrearagesListReport.Document.Pages.Count);
                            searchConditionReport.Run(false);

                            arrearagesListReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchConditionReport.Document.Pages.Clone());
                            report = arrearagesListReport;
                        }
                    });
                    ProgressDialog.Start(ParentForm, Task.Run(() => task), false, SessionKey);

                    ShowDialogPreview(ParentForm, report, serverPath);
                }
                else
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist, "印刷");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
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

        /// <summary>Reportのコンディション</summary>
        private void ReportSearchCondition()
        {
            SearchListCondition = new List<object>();

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                await SetReportSetting();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (datPaymentDate.Value.HasValue)
            {
                SearchListCondition.Add(new SearchData("入金基準日", (datPaymentDate.Value.Value).ToShortDateString()));
            }

            if (cbxCustomer.Checked)
            {
                SearchListCondition.Add(new SearchData("得意先毎に集計", "あり"));
            }
            else
            {
                SearchListCondition.Add(new SearchData("得意先毎に集計", "なし"));
            }

            if (cbxMemo.Checked)
            {
                SearchListCondition.Add(new SearchData("メモ有り", "有り"));
            }
            else
            {
                SearchListCondition.Add(new SearchData("メモ有り", ""));
            }

            if (!string.IsNullOrEmpty(txtMemo.Text))
            {
                SearchListCondition.Add(new SearchData("請求メモ", txtMemo.Text));
            }
            else
            {
                SearchListCondition.Add(new SearchData("請求メモ", "(指定なし)"));
            }

            if (UseForeignCurrency)
            {
                if (!string.IsNullOrEmpty(txtCurrencyCode.Text))
                {
                    SearchListCondition.Add(new SearchData("通貨コード", txtCurrencyCode.Text));
                }
                else
                {
                    SearchListCondition.Add(new SearchData("通貨コード", "(指定なし)"));
                }
            }

            var department = "(指定なし)";
            if (!string.IsNullOrEmpty(txtFromDepartmentCode.Text))
            {
                department = txtFromDepartmentCode.Text;

                if (!string.IsNullOrEmpty(lblFromDepartmentName.Text))
                {
                    department += " : " + lblFromDepartmentName.Text;
                }
            }
            department += " ～ ";

            if (!string.IsNullOrEmpty(txtToDapartmentCode.Text))
            {
                department += txtToDapartmentCode.Text;

                if (!string.IsNullOrEmpty(lblToDepartmentName.Text))
                {
                    department += " : " + lblToDepartmentName.Text;
                }
            }
            else
            {
                department += "(指定なし)";
            }
            SearchListCondition.Add(new SearchData("請求部門コード", department));

            var staffCode = "(指定なし)";
            if (!string.IsNullOrEmpty(txtFromStaffCode.Text))
            {
                staffCode = txtFromStaffCode.Text;
                if (!string.IsNullOrEmpty(lblFromStaffName.Text))
                {
                    staffCode += " : " + lblFromStaffName.Text;
                }
            }
            staffCode += " ～ ";

            if (!string.IsNullOrEmpty(txtToStaffCode.Text))
            {
                staffCode += txtToStaffCode.Text;
                if (!string.IsNullOrEmpty(lblToStaffName.Text))
                {
                    staffCode += " : " + lblToStaffName.Text;
                }
            }
            else staffCode += "(指定なし)";
            SearchListCondition.Add(new SearchData("担当者コード", staffCode));

            if (cbxCustomer.Checked == false)
            {
                string outputOrder = null;
                if (ReportSettingList[5].ItemId == "ReportOutputOrder")
                {
                    if (ReportSettingList[0].ItemKey == "1")
                    {
                        outputOrder += "請求部門,";
                    }
                    if (ReportSettingList[1].ItemKey == "1")
                    {
                        outputOrder += "担当者,";
                    }
                    if (ReportSettingList[2].ItemKey == "1")
                    {
                        outputOrder += "得意先,";
                    }
                    if (ReportSettingList[3].ItemKey == "1")
                    {
                        outputOrder += "入金予定日,";
                    }
                    if (ReportSettingList[5].ItemKey == "0" && ReportSettingList[2].ItemKey == "0")
                    {
                        outputOrder += "得意先,";
                    }
                    if (ReportSettingList[5].ItemKey == "2")
                    {
                        outputOrder += "ID,";
                    }
                    if (ReportSettingList[6].ItemKey == "0")
                    {
                        outputOrder += "請求日,";
                    }
                    if (ReportSettingList[6].ItemKey == "1")
                    {
                        outputOrder += "売上日,";
                    }
                    if (ReportSettingList[6].ItemKey == "2")
                    {
                        outputOrder += "請求締日,";
                    }
                    if (ReportSettingList[6].ItemKey == "3" && ReportSettingList[3].ItemKey == "0")
                    {
                        outputOrder += "入金予定日,";
                    }
                    if (ReportSettingList[6].ItemKey == "4")
                    {
                        outputOrder += "当初予定日,";
                    }
                }
                else
                {
                    outputOrder += "(指定なし),";
                }

                if (outputOrder != null)
                {
                    outputOrder = outputOrder.Substring(0, outputOrder.Length - 1);
                }
                SearchListCondition.Add(new SearchData("出力順", outputOrder));

                var baseDate = "(指定なし)";
                if (ReportSettingList[6].ItemKey == "0" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "請求日";
                }
                else if (ReportSettingList[6].ItemKey == "1" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "売上日";
                }
                else if (ReportSettingList[6].ItemKey == "2" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "請求締日";
                }
                else if (ReportSettingList[6].ItemKey == "3" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "入金予定日";
                }
                else if (ReportSettingList[6].ItemKey == "4" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "当初予定日";
                }
                SearchListCondition.Add(new SearchData("日付基準", baseDate));
            }

            if (!UseForeignCurrency && ReportSettingList[4].ItemId == "ReportUnitPrice")
            {
                var unit = "";
                switch (ReportSettingList[4].ItemKey)
                {
                    case "0": unit = "円"; break;
                    case "1": unit = "千円"; break;
                    case "2": unit = "万円"; break;
                    case "3": unit = "百万円"; break;
                }
                SearchListCondition.Add(new SearchData("単位", unit));
            }
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();

                List<ArrearagesList> arrearagesList = grdReminder.Rows.Select(x => x.DataBoundItem as ArrearagesList).ToList();

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
                var fileName = $"滞留明細一覧表{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new ArrearagesListFileDefinition(new DataExpression(ApplicationControl), ColumnSettingInfo);
                if (definition.CurrencyCodeField.Ignored = !UseForeignCurrency)
                {
                    definition.CurrencyCodeField.FieldName = null;
                }
                if (definition.IDField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.IDField.FieldName = null;
                }
                if (definition.BilledAtField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.BilledAtField.FieldName = null;
                }
                if (definition.SalesAtField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.SalesAtField.FieldName = null;
                }
                if (definition.ClosingAtField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.ClosingAtField.FieldName = null;
                }
                if (definition.DueAtField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.DueAtField.FieldName = null;
                }
                if (definition.OriginalDueAtField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.OriginalDueAtField.FieldName = null;
                }
                if (definition.ArrearagesDayCountField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.ArrearagesDayCountField.FieldName = null;
                }
                if (definition.CodeAndNameField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.CodeAndNameField.FieldName = null;
                }
                if (definition.InvoiceCodeField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.InvoiceCodeField.FieldName = null;
                }
                if (definition.Note1Field.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.Note1Field.FieldName = null;
                }
                if (definition.MemoField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.MemoField.FieldName = null;
                }
                if (definition.CustomerStaffNameField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.CustomerStaffNameField.FieldName = null;
                }
                if (definition.NoteField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.NoteField.FieldName = null;
                }
                if (definition.TelField.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.TelField.FieldName = null;
                }
                if (definition.Note2Field.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.Note2Field.FieldName = null;
                }
                if (definition.Note3Field.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.Note3Field.FieldName = null;
                }
                if (definition.Note4Field.Ignored = (SearchConditionOld.CustomerSummaryFlag))
                {
                    definition.Note4Field.FieldName = null;
                }

                definition.AmountField.Format = value => value.ToString("0." + new string('0', Precision));

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, arrearagesList, cancel, progress);
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

        [OperationLog("設定")]
        private void PrintSetting()
        {
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PF0401);
                screen.InitializeParentForm("帳票設定");
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
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
                reminder.Checked = check;
            }
        }

        [OperationLog("終了")]
        private void Exit()
        {
            try
            {
                Settings.SaveControlValue<PF0401>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PF0401>(Login, cbxStaff.Name, cbxStaff.Checked);
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

        private void txtFromDepartmentCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var fromDepartmentCode = txtFromDepartmentCode.Text;
                var fromDepartmentName = string.Empty;

                if (!string.IsNullOrEmpty(fromDepartmentCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { fromDepartmentCode });
                        var departmentResult = result.Departments.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            fromDepartmentCode = departmentResult.Code;
                            fromDepartmentName = departmentResult.Name;
                            ClearStatusMessage();
                        }

                        txtFromDepartmentCode.Text = fromDepartmentCode;
                        lblFromDepartmentName.Text = fromDepartmentName;

                        if (cbxDepartment.Checked)
                        {
                            txtToDapartmentCode.Text = fromDepartmentCode;
                            lblToDepartmentName.Text = fromDepartmentName;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    if (cbxDepartment.Checked)
                    {
                        txtToDapartmentCode.Text = fromDepartmentCode;
                        lblToDepartmentName.Clear();
                    }
                    lblFromDepartmentName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnFromDepartment_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtFromDepartmentCode.Text = department.Code;
                lblFromDepartmentName.Text = department.Name;
                if (cbxDepartment.Checked)
                {
                    txtToDapartmentCode.Text = department.Code;
                    lblToDepartmentName.Text = department.Name;
                }
                ClearStatusMessage();
            }
        }     

        private void btnToDepartment_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtToDapartmentCode.Text = department.Code;
                lblToDepartmentName.Text = department.Name;
                ClearStatusMessage();
            }
        }

        private void txtToDapartmentCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var toDepartmentCode = txtToDapartmentCode.Text;
                var toDepartmentName = string.Empty;

                if (!string.IsNullOrEmpty(toDepartmentCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { toDepartmentCode });
                        var departmentResult = result.Departments.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            toDepartmentCode = departmentResult.Code;
                            toDepartmentName = departmentResult.Name;
                            ClearStatusMessage();
                        }
                        txtToDapartmentCode.Text = toDepartmentCode;
                        lblToDepartmentName.Text = toDepartmentName;
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtToDapartmentCode.Text = toDepartmentCode;
                    lblToDepartmentName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtFromStaffCode_Validated(object sender, EventArgs e)
        {
            var fromStaffCode = txtFromStaffCode.Text;
            var fromStaffName = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(fromStaffCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<StaffMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { fromStaffCode });
                        var staffResult = result.Staffs.FirstOrDefault();

                        if (staffResult != null)
                        {
                            fromStaffCode = staffResult.Code;
                            fromStaffName = staffResult.Name;
                            ClearStatusMessage();
                        }

                        txtFromStaffCode.Text = fromStaffCode;
                        lblFromStaffName.Text = fromStaffName;

                        if (cbxStaff.Checked)
                        {
                            txtToStaffCode.Text = fromStaffCode;
                            lblToStaffName.Text = fromStaffName;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    if (cbxStaff.Checked)
                    {
                        txtToStaffCode.Text = fromStaffCode;
                        lblToStaffName.Clear();
                    }
                    lblFromStaffName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtToStaffCode_Validated(object sender, EventArgs e)
        {
            var toStaffCode = txtToStaffCode.Text;
            var toStaffName = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(toStaffCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<StaffMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { toStaffCode });
                        var staffResult = result.Staffs.FirstOrDefault();

                        if (staffResult != null)
                        {
                            toStaffCode = staffResult.Code;
                            toStaffName = staffResult.Name;
                            ClearStatusMessage();
                        }
                        txtToStaffCode.Text = toStaffCode;
                        lblToStaffName.Text = toStaffName;
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    lblToStaffName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnFromStaff_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtFromStaffCode.Text = staff.Code;
                lblFromStaffName.Text = staff.Name;
                if (cbxStaff.Checked)
                {
                    txtToStaffCode.Text = staff.Code;
                    lblToStaffName.Text = staff.Name;
                }

                ClearStatusMessage();
            }
        }

        private void btnToStaff_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtToStaffCode.Text = staff.Code;
                lblToStaffName.Text = staff.Name;
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

        private void grdArrearagesList_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.CellName != "celMemo") return;

            var data = grdReminder.Rows[e.RowIndex].DataBoundItem as ArrearagesList;
            if (data == null) return;
            using (var form = ApplicationContext.Create(nameof(PH9906)))
            {
                var screen = form.GetAll<PH9906>().First();
                screen.Id = data.Id;
                screen.MemoType = MemoType.BillingMemo;
                screen.Memo = data.Memo;
                screen.InitializeParentForm("請求メモ");

                if (ApplicationContext.ShowDialog(ParentForm, form, true) == DialogResult.OK)
                {
                    grdReminder.Rows[e.RowIndex].Cells["celMemo"].Value = screen.Memo;
                }
            }
        }

        private void cbxShowCancelReminderData_CheckedChanged(object sender, EventArgs e)
        {
            OnF01ClickHandler = IsCancelReminder ? (System.Action)SearchCancelDecisionData : Search;

            BaseContext.SetFunction03Caption(IsCancelReminder ? "確定取消" : "確定");
            OnF03ClickHandler = IsCancelReminder ? (System.Action)CancelReminder : Decision;
        }

        private async Task<ReminderResult> GetCancelDecisionItemsAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetCancelDecisionItemsAsync(SessionKey, SearchCondition, ReminderCommonSetting, ReminderSummarySetting.ToArray());
                return result;
            });

        private async Task<CountResult> CancelRemindersAsync(Reminder[] reminders)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.CancelRemindersAsync(SessionKey, reminders);
                return result;
            });

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
