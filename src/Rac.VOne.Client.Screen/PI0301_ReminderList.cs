using GrapeCity.ActiveReports;
using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.ReminderService;
using Rac.VOne.Client.Screen.ReminderSettingService;
using Rac.VOne.Client.Screen.StatusMasterService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections;
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
    /// <summary>督促管理帳票</summary>
    public partial class PI0301 : VOneScreenBase
    {
        #region メンバー
        private int Precision { get; set; }
        private string CellName(string value) => $"cel{value}";
        private ReminderCommonSetting ReminderCommonSetting { get; set; }
        private List<GridSetting> GridSettingList { get; set; }
        private bool IsCustomerUnit
            => ReminderCommonSetting.ReminderManagementMode == (int)ReminderManagementMode.ByCustomer;
        private class FKeysName
        {
            internal const string F01 = "照会";
            internal const string F02 = "クリア";
            internal const string F04 = "印刷";
            internal const string F06 = "エクスポート";
            internal const string F10 = "終了";
        }
        #endregion

        #region 初期化
        public PI0301()
        {
            InitializeComponent();
            grid.SetupShortcutKeys();
            Text = "督促管理帳票";
            InitializeHandlers();
        }
        private void PI0201_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                loadTask.Add(LoadControlColorAsync());
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                loadTask.Add(LoadStatusComboAsync());
                loadTask.Add(LoadReminderCommonSetting());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SuspendLayout();
                tbcReminder.SelectedIndex = 0;
                ResumeLayout();
                SetInitialSetting();
                ClearControls();
                grid.Text = "";

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
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
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption(FKeysName.F01);
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption(FKeysName.F02);
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption(FKeysName.F04);
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = Print;

            BaseContext.SetFunction06Caption(FKeysName.F06);
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction10Caption(FKeysName.F10);
            OnF10ClickHandler = Exit;
        }
        private void SetInitialSetting()
        {
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);
                txtLoginUserCode.MaxLength = ApplicationControl.LoginUserCodeLength;
                txtLoginUserCode.PaddingChar = expression.LoginUserCodePaddingChar;
                txtLoginUserCode.Format = expression.LoginUserCodeFormatString;

                txtFromCustomerCode.Format = expression.CustomerCodeFormatString;
                txtFromCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtFromCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;
                txtFromCustomerCode.ImeMode = expression.CustomerCodeImeMode();


                txtToCustomerCode.Format = expression.CustomerCodeFormatString;
                txtToCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtToCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;
                txtToCustomerCode.ImeMode = expression.CustomerCodeImeMode();

                SetVisible();
            }

            Settings.SetCheckBoxValue<PF0401>(Login, cbxCustomer);
        }
        private void SetVisible()
        {
            if (IsCustomerUnit)
            {
                pnlBaseDate.Visible = false;
                pnlArrearDays.Visible = false;
                pnlStatus.Visible = false;
                pnlReminderFlag.Visible = false;
                pnlAssignmentFlag.Visible = false;
            }

            if (UseForeignCurrency)
            {
                pnlCurrency.Visible = false;
            }
        }
        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var templateComboCell = builder.GetComboBoxCell();
            ServiceProxyFactory.Do<ReminderSettingServiceClient>(client =>
            {
                var result = client.GetReminderTemplateSettings(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    templateComboCell.DataSource = result.ReminderTemplateSettings;
                    templateComboCell.ValueMember = nameof(ReminderTemplateSetting.Id);
                    templateComboCell.DisplayMember = nameof(ReminderTemplateSetting.Name);
                }
            });

            GridSettingList = GetGridSettingList();
            GetGridCells(builder);

            grid.Template = builder.Build();
            grid.HideSelection = true;
        }
        private void GetGridCells(GcMultiRowTemplateBuilder builder)
        {
            var height = builder.DefaultRowHeight;

            foreach (var gs in GridSettingList)
            {
                var cell = new CellSetting(height,
                    gs.DisplayWidth,
                    gs.ColumnName,
                    dataField: gs.ColumnName,
                    caption: gs.ColumnNameJp,
                    sortable: false);

                switch (gs.ColumnName)
                {
                    case nameof(ReminderHistory.CustomerCodeDisplay):
                    case nameof(ReminderHistory.CurrencyCode):
                    case nameof(ReminderHistory.OutputFlagName):
                        cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter);
                        break;
                    case nameof(ReminderHistory.CustomerNameDisplay):
                    case nameof(ReminderHistory.InputTypeName):
                    case nameof(ReminderHistory.StatusCodeAndName):
                    case nameof(ReminderHistory.Memo):
                    case nameof(ReminderHistory.CreateByName):
                        cell.CellInstance = builder.GetTextBoxCell();
                        break;
                    case nameof(ReminderHistory.CalculateBaseDateDisplay):
                        cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                        break;
                    case nameof(ReminderHistory.ReminderAmount):
                        cell.CellInstance = builder.GetNumberCellCurrency(Precision, Precision, 0);
                        break;
                    case nameof(ReminderHistory.CreateAt):
                        cell.CellInstance = builder.GetDateCell_yyyyMMddHHmmss();
                        break;
                    case nameof(ReminderHistory.ArrearsDaysDisplay):
                        cell.CellInstance = builder.GetNumberCell();
                        break;
                    default:
                        cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft);
                        break;
                }
                builder.Items.Add(cell);
            }
        }
        private List<GridSetting> GetGridSettingList()
        {
            var widthCcy = UseForeignCurrency ? 60 : 0;
            var baseDateCaption = "当初予定日";
            if (ReminderCommonSetting?.CalculateBaseDate == (int)CalculateBaseDate.DueAt)
                baseDateCaption = "入金予定日";
            if (ReminderCommonSetting?.CalculateBaseDate == (int)CalculateBaseDate.BilledAt)
                baseDateCaption = "請求日";
            var widthDay = 115;


            var list = new List<GridSetting>();

            list.Add(new GridSetting { DisplayWidth = 115, ColumnName = nameof(ReminderHistory.CustomerCodeDisplay), ColumnNameJp = "得意先コード"});
            list.Add(new GridSetting{DisplayWidth = 150, ColumnName = nameof(ReminderHistory.CustomerNameDisplay), ColumnNameJp = "得意先名" });
            if(!IsCustomerUnit)
                list.Add(new GridSetting { DisplayWidth = widthDay, ColumnName = nameof(ReminderHistory.CalculateBaseDateDisplay), ColumnNameJp = baseDateCaption });
            if (!IsCustomerUnit)
                list.Add(new GridSetting { DisplayWidth = 70, ColumnName = nameof(ReminderHistory.ArrearsDaysDisplay), ColumnNameJp = "滞留日数" });
            list.Add(new GridSetting{DisplayWidth = widthCcy, ColumnName = nameof(ReminderHistory.CurrencyCode), ColumnNameJp = "通貨コード" });
            list.Add(new GridSetting{DisplayWidth = 120, ColumnName = nameof(ReminderHistory.ReminderAmount), ColumnNameJp = "滞留金額" });
      
            list.Add(new GridSetting{DisplayWidth = 120, ColumnName = nameof(ReminderHistory.InputTypeName), ColumnNameJp = "アクション" });
            if (!IsCustomerUnit)
                list.Add(new GridSetting{DisplayWidth = 100, ColumnName = nameof(ReminderHistory.StatusCodeAndName), ColumnNameJp = "ステータス" });
            list.Add(new GridSetting{DisplayWidth = 400, ColumnName = nameof(ReminderHistory.Memo), ColumnNameJp = "対応記録" });
            if (!IsCustomerUnit)
                list.Add(new GridSetting{DisplayWidth = 60, ColumnName = nameof(ReminderHistory.OutputFlagName), ColumnNameJp = "督促区分" });
            list.Add(new GridSetting { DisplayWidth = 140, ColumnName = nameof(ReminderHistory.CreateAt), ColumnNameJp = "更新日時" });
            list.Add(new GridSetting { DisplayWidth = 100, ColumnName = nameof(ReminderHistory.CreateByName), ColumnNameJp = "更新者名" });
            return list;
        }
        #endregion

        #region ファンクションキー イベント
        #region F1/照会
        [OperationLog(FKeysName.F01)]
        private void Search()
        {
            try
            {
                ClearStatusMessage();
                if (!ValidateForSearch()) return;
                var task = SearchReminderListAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrSomethingError, "照会");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "照会");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private bool ValidateForSearch()
        {
            if (!datPaymentDate.Value.HasValue)
            {
                tbcReminder.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, lblCalculateBaseDate.Text);
                datPaymentDate.Focus();
                return false;
            }

            if (!cbxNoOutput.Checked && !cbxOutputed.Checked)
            {
                ShowWarningDialog(MsgWngSelectionRequired, lblReminderFlag.Text);
                cbxNoOutput.Focus();
                return false;
            }

            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                tbcReminder.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text);
                txtCurrencyCode.Focus();
                return false;
            }

            if (!txtFromArrearDays.ValidateRange(txtToArrearDays,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblArrearDays.Text))) return false;


            if (!cbxFullAssignment.Checked && !cbxNoAssignment.Checked)
            {
                cbxNoAssignment.Focus();
                ShowWarningDialog(MsgWngSelectionRequired, lblAssignmentFlag.Text);
                return false;
            }

            if (!txtFromCustomerCode.ValidateRange(txtToCustomerCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomer.Text))) return false;

            if (!datCreateAtFrom.ValidateRange(datCreateAtTo,
           () => ShowWarningDialog(MsgWngInputRangeChecked, lblUpdateAt.Text))) return false;

            return true;
        }
        private ReminderSearch GetSearchCondition()
        {
            var reminderSearch = new ReminderSearch();
            reminderSearch.CompanyId = CompanyId;
            reminderSearch.CurrencyCode = UseForeignCurrency ? reminderSearch.CurrencyCode = txtCurrencyCode.Text : DefaultCurrencyCode;

            if(!IsCustomerUnit)
            {
                reminderSearch.CalculateBaseDate = datPaymentDate.Value.Value;
                reminderSearch.ArrearDaysFrom = (int?)txtFromArrearDays.Value;
                reminderSearch.ArrearDaysTo = (int?)txtToArrearDays.Value;
                reminderSearch.Status = (int)cmbStatus.SelectedValue;

                reminderSearch.Status = (int)cmbStatus.SelectedValue;

                if (cbxNoOutput.Checked && cbxOutputed.Checked) reminderSearch.OutputFlag = null;
                else if (cbxNoOutput.Checked) reminderSearch.OutputFlag = 0;
                else if (cbxOutputed.Checked) reminderSearch.OutputFlag = 1;

                int assignmentFlag;
                if (cbxNoAssignment.Checked && cbxFullAssignment.Checked)
                {
                    assignmentFlag = (int)AssignmentFlagChecked.All;
                }
                else if (cbxNoAssignment.Checked && !cbxFullAssignment.Checked)
                {
                    assignmentFlag = (int)AssignmentFlagChecked.NoAssignment;
                }
                else if (!cbxNoAssignment.Checked && cbxFullAssignment.Checked)
                {
                    assignmentFlag = (int)AssignmentFlagChecked.FullAssignment;
                }
                else
                {
                    assignmentFlag = (int)AssignmentFlagChecked.None;
                }

                reminderSearch.AssignmentFlg = assignmentFlag;
            }
            if (datCreateAtFrom.Value.HasValue)
                reminderSearch.CreateAtFrom = datCreateAtFrom.Value.Value;
            if (datCreateAtTo.Value.HasValue)
            {
                var updateAtTo = datCreateAtTo.Value.Value;
                reminderSearch.CreateAtTo = updateAtTo.Date.AddDays(1).AddMilliseconds(-1);
            }
            if (!string.IsNullOrWhiteSpace(txtReminderMemo.Text))
                reminderSearch.ReminderMemo = txtReminderMemo.Text;
            if (!string.IsNullOrWhiteSpace(txtFromCustomerCode.Text))
                reminderSearch.CustomerCodeFrom = txtFromCustomerCode.Text;
            if (!string.IsNullOrWhiteSpace(txtToCustomerCode.Text))
                reminderSearch.CustomerCodeTo = txtToCustomerCode.Text;
            if (!string.IsNullOrEmpty(txtLoginUserCode.Text))
                reminderSearch.CreateByCode = txtLoginUserCode.Text;


            reminderSearch.ReminderManaged = !IsCustomerUnit;

            return reminderSearch;
        }
        private async Task<bool> SearchReminderListAsync()
        {
            try
            {
                var condition = GetSearchCondition();

                var history = await GetReminderListAsync(condition);
                if (history == null)
                {
                    return false;
                }
                else if ( history.Count == 0)
                {
                    tbcReminder.SelectedTab = tabReminderSearch;
                    grid.DataSource = null;
                    txtCount.Clear();
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction06Enabled(false);
                    ShowWarningDialog(MsgWngNotExistSearchData);
                    return true;
                }

                if (IsCustomerUnit)
                {
                    var customerCodeBk = string.Empty;
                    foreach (var h in history)
                    {
                        if (customerCodeBk != h.CustomerCode)
                        {
                            h.IsTheTop = true;
                            customerCodeBk = h.CustomerCode;
                        }
                    }
                }
                else
                {
                    var reminderIdBk = 0;
                    foreach (var h in history)
                    {
                        if (reminderIdBk != h.ReminderId)
                        {
                            h.IsTheTop = true;
                            reminderIdBk = h.ReminderId;
                        }
                    }
                }

                grid.DataSource = new BindingSource(history, null);
                txtCount.Text = history.Count.ToString("#,###");
                InitializeGrid();
                tbcReminder.SelectedTab = tabReminderResult;
                BaseContext.SetFunction04Enabled(true);
                BaseContext.SetFunction06Enabled(true);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
        }
        public string GetNumberFormat()
        {
            var displayFieldString = "#,###,###,###,##0";
            if (Precision > 0)
            {
                displayFieldString += "." + new string('0', Precision);
            }
            return displayFieldString;
        }
        #endregion

        #region F2/クリア
        [OperationLog(FKeysName.F02)]
        private void Clear()
        {
            ClearControls();
        }
        private void ClearControls()
        {
            ClearStatusMessage();
            datPaymentDate.Value = DateTime.Today;
            txtFromArrearDays.Clear();
            txtToArrearDays.Clear();
            txtLoginUserCode.Clear();
            lblLoginUserName.Clear();
            cmbStatus.SelectedIndex = 0;
            cbxNoOutput.Checked = true;
            cbxOutputed.Checked = true;
            cbxNoAssignment.Checked = true;
            cbxFullAssignment.Checked = true;
            txtCurrencyCode.Clear();
            txtFromCustomerCode.Clear();
            lblFromCustomerName.Clear();
            txtToCustomerCode.Clear();
            lblToCustomerName.Clear();
            datCreateAtFrom.Clear();
            datCreateAtTo.Clear();
            txtReminderMemo.Clear();
            grid.DataSource = null;
            txtCount.Clear();
            tbcReminder.SelectedTab = tabReminderSearch;
            datPaymentDate.Select();
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            if (IsCustomerUnit)
                this.ActiveControl = datCreateAtFrom;
            else
                this.ActiveControl = datPaymentDate;
        }
        #endregion

        #region F4/印刷
        [OperationLog(FKeysName.F04)]
        private void Print()
        {
            try
            {
                ClearStatusMessage();
                if (grid.RowCount == 0)
                {
                    DispStatusMessage(MsgWngSelectionRequired, "印刷するデータ");
                    return;
                }

                var task = CreateReport();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrSomethingError, "照会");
                    return;
                }

            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "照会");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private async Task<bool> CreateReport()
        {
            try
            {
                SectionReport report;
                if (IsCustomerUnit)
                {
                    var rimindeReport = new ReminderListByCustomerReport();
                    rimindeReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    report = rimindeReport;
                }
                else
                {
                    var rimindeReport = new ReminderListByReminderReport();
                    rimindeReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    rimindeReport.lblDueDate.Text = ReminderCommonSetting.CalculateBaseDate == 0
                        ? "当初予定日"
                        : "入金予定日";
                    report = rimindeReport;
                }
               
                report.DataSource = grid.DataSource;
                report.Run();
                ShowDialogPreview(ParentForm, report, await GetServerPath());
                return true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
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
        #endregion

        #region F6/エクスポート
        [OperationLog(FKeysName.F06)]
        private async void Export()
        {
            try
            {
                var serverPath = await GetServerPath();

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                var filePath = string.Empty;
                var fileName = $"督促管理帳票{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var exportBillingInvoice = ((IEnumerable)grid.DataSource).Cast<ReminderHistory>().ToList();
                var definition = new ReminderListFileDefinition(new DataExpression(ApplicationControl), GridSettingList);

                var decimalFormat = "###,##0";

                definition.ReminderAmountField.Format = value => value.ToString(decimalFormat);

                definition.SetFieldsSetting(GridSettingList, definition.ConvertSettingToField);

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm,
                    (cancel, progress) => { return exporter.ExportAsync(filePath, exportBillingInvoice, cancel, progress); },
                    true,
                    SessionKey);

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
        #endregion

        #region F10/終了
        [OperationLog(FKeysName.F10)]
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
        private void txtLoginUserCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLoginUserCode.Text))
                {
                    LoginUser userResult = null;
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<LoginUserMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtLoginUserCode.Text });

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
                else
                {
                    ClearStatusMessage();
                    lblLoginUserName.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void btnLoginUser_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                txtLoginUserCode.Text = loginUser.Code;
                lblLoginUserName.Text = loginUser.Name;
                ClearStatusMessage();
            }
        }
        #endregion

        #region Web Service
        private async Task<List<ReminderHistory>> GetReminderListAsync(ReminderSearch condition) =>
                    await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetReminderListAsync(SessionKey, condition, ReminderCommonSetting);
                if (result.ProcessResult.Result && result.ReminderHistories.Count >= 0)
                {
                    return result.ReminderHistories;
                }
                else
                {
                    return null;
                }
            });
        private async Task LoadStatusComboAsync()
        {
            await ServiceProxyFactory.DoAsync<StatusMasterClient>(async client =>
            {
                cmbStatus.Items.Add(new ListItem("完了以外", -1));
                cmbStatus.Items.Add(new ListItem("すべて", 0));

                var result = await client.GetStatusesByStatusTypeAsync(SessionKey, CompanyId, (int)StatusType.Reminder);
                if (result.ProcessResult.Result)
                {
                    foreach (var s in result.Statuses)
                    {
                        cmbStatus.Items.Add(new ListItem(s.Name, s.Id));
                    }
                }

                cmbStatus.ValueSubItemIndex = 1;
                cmbStatus.TextSubItemIndex = 0;
            });
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
        #endregion
    }
}
