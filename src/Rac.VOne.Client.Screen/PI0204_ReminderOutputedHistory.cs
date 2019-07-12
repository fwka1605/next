using GrapeCity.ActiveReports.Document.Section;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.PdfOutputSettingMasterService;
using Rac.VOne.Client.Screen.ReminderService;
using Rac.VOne.Client.Screen.ReminderSettingService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>督促状発行履歴</summary>
    public partial class PI0204 : VOneScreenBase
    {
        // 変数宣言
        private int Precision { get; set; }
        List<ReportSetting> ReportSettingList { get; set; }
        private List<ColumnNameSetting> ColumnNameSettingInfo { get; set; }
        private ReminderOutputedSearch SearchCondition { get; set; }
        private string CellName(string value) => $"cel{value}";
        private ReminderCommonSetting ReminderCommonSetting { get; set; }
        private List<ReminderSummarySetting> ReminderSummarySetting { get; set; }
        private bool UseDestinationSummarized { get; set; }

        public PI0204()
        {
            InitializeComponent();
            grdReminder.SetupShortcutKeys();
            Text = "督促状発行履歴";
            ReportSettingList = new List<ReportSetting>();
            InitializeHandlers();
        }
        private void InitializeHandlers()
        {
            tbcReminder.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcReminder.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("戻る");
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

            BaseContext.SetFunction03Caption("再出力");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Reoutput;

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

            BaseContext.SetFunction10Caption("戻る");
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PI0203_Load(object sender, EventArgs e)
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
                loadTask.Add(LoadReminderSummarySetting());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SuspendLayout();
                tbcReminder.SelectedIndex = 0;
                ResumeLayout();
                SetInitialSetting();
                InitializeGrid();
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
                    ColumnNameSettingInfo = result.ColumnNames.Where(x => x.TableName == "Billing").ToList();
                    
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
            }

            Settings.SetCheckBoxValue<PF0401>(Login, cbxCustomer);

            this.ActiveControl = datOutputAtFrom;
            datOutputAtFrom.Focus();
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
        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var widthCcy = UseForeignCurrency ? 60 : 0;

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
            
            builder.Items.Add(new CellSetting(height, 30, nameof(ReminderOutputed.Checked),      dataField: nameof(ReminderOutputed.Checked),      caption: "選択",        cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false));
            builder.Items.Add(new CellSetting(height, 140, nameof(ReminderOutputed.OutputAt), dataField: nameof(ReminderOutputed.OutputAt), caption: "発行日時", cell: builder.GetDateCell_yyyyMMddHHmmss(), sortable: true));
            builder.Items.Add(new CellSetting(height, 70, nameof(ReminderOutputed.OutputNoPaddingZero), dataField: nameof(ReminderOutputed.OutputNoPaddingZero), caption: "発行番号", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 115, nameof(ReminderOutputed.CustomerCode), dataField: nameof(ReminderOutputed.CustomerCode), caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 150, nameof(ReminderOutputed.CustomerName), dataField: nameof(ReminderOutputed.CustomerName), caption: "得意先名", cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, 70, nameof(ReminderOutputed.BillingCount), dataField: nameof(ReminderOutputed.BillingCount), caption: "明細件数", cell: builder.GetNumberCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, widthCcy, nameof(ReminderOutputed.CurrencyCode), dataField: nameof(ReminderOutputed.CurrencyCode), caption: "通貨コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(ReminderOutputed.RemainAmount), dataField: nameof(ReminderOutputed.RemainAmount), caption: "滞留金額", cell: builder.GetNumberCellCurrency(Precision, Precision, 0), sortable: true));
            builder.Items.Add(new CellSetting(height, 180, nameof(ReminderOutputed.ReminderTemplateId), dataField: nameof(ReminderOutputed.ReminderTemplateId), caption: "文面パターン", cell: templateComboCell, readOnly: false));
            if (UseDestinationSummarized)
            {
                builder.Items.Add(new CellSetting(height, 40, nameof(ReminderOutputed.DestinationCode), dataField: nameof(ReminderOutputed.DestinationCode), caption: "", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
                builder.Items.Add(new CellSetting(height, 240, nameof(ReminderOutputed.DestinationDisplay), dataField: nameof(ReminderOutputed.DestinationDisplay), caption: "送付先", cell: builder.GetTextBoxCell()));
            }
            builder.Items.Add(new CellSetting(height, 60, "Preview", caption: "プレビュー", cell: builder.GetButtonCell(), readOnly: false));

            grdReminder.Template = builder.Build();
            grdReminder.HideSelection = true;
            grdReminder.AllowAutoExtend = false;
            if (UseDestinationSummarized)
                grdReminder.FreezeLeftCellName = CellName(nameof(ReminderOutputed.Checked));
        }
        #endregion

        #region Function Key Event
        [OperationLog("検索")]
        private void Search()
        {
            SearchReminderOutputed();
        }

        private void SearchReminderOutputed()
        {
            try
            {
                ClearStatusMessage();

                var loadTask = new List<Task>();
                loadTask.Add(LoadReminderCommonSetting());
                loadTask.Add(LoadReminderSummarySetting());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                if (!ValidateSearchData()) return;
                SearchCondition = GetSearchDataCondition();
                int count = 0;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    count = await SearchReminderOutputedAsync(SearchCondition);
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

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
                return false;
            }

            if (ReminderSummarySetting == null || ReminderSummarySetting.Count == 0)
            {
                return false;
            }

            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                tbcReminder.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Focus();
                return false;
            }

            if (!datOutputAtFrom.ValidateRange(datOutputAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblOutputAt.Text))) return false;

            if (!nmbOutputNoFrom.ValidateRange(nmbOutputNoTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblOutputNo.Text))) return false;

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
                ReminderSummarySetting = setting;
                UseDestinationSummarized = ReminderSummarySetting.Any(x => x.ColumnName == "DestinationCode" && x.Available == 1);
            });
        }

        /// <summary>検索データ</summary>
        /// <returns>ArrearagesListSearch</returns>
        private ReminderOutputedSearch GetSearchDataCondition()
        {
            var reminderSearch = new ReminderOutputedSearch();

            reminderSearch.CompanyId = CompanyId;

            reminderSearch.OutputAtFrom = datOutputAtFrom.Value;
            reminderSearch.OutputAtTo = datOutputAtTo.Value;
            reminderSearch.OutputNoFrom = (int?)nmbOutputNoFrom.Value;
            reminderSearch.OutputNoTo = (int?)nmbOutputNoTo.Value;

            reminderSearch.CurrencyCode = UseForeignCurrency ? reminderSearch.CurrencyCode = txtCurrencyCode.Text : Constants.DefaultCurrencyCode;
            
            if (!string.IsNullOrWhiteSpace(txtFromCustomerCode.Text))
                reminderSearch.CustomerCodeFrom = txtFromCustomerCode.Text;
            if (!string.IsNullOrWhiteSpace(txtToCustomerCode.Text))
                reminderSearch.CustomerCodeTo = txtToCustomerCode.Text;

            reminderSearch.UseDestinationSummarized = UseDestinationSummarized;

            return reminderSearch;
        }

        private async Task<int> SearchReminderOutputedAsync(ReminderOutputedSearch search)
        {
            var result = new ReminderOutputedResult();
            decimal totalAmount = 0m;
            int count = 0;

            await ServiceProxyFactory.DoAsync<ReminderServiceClient>(async client =>
            {
                result = await client.GetOutputedItemsAsync(SessionKey, SearchCondition);
                grdReminder.DataSource = new BindingSource(result.ReminderOutputed, null);
                if (result.ProcessResult.Result)
                    count = result.ReminderOutputed.Count;

                if (count > 0)
                {
                    totalAmount = result.ReminderOutputed.Sum(x => x.RemainAmount);
                    tbcReminder.SelectedTab = tabReminderResult;
                    BaseContext.SetFunction03Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                }
                else
                {
                    tbcReminder.SelectedTab = tabReminderSearch;
                    grdReminder.DataSource = null;
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
            });
            return count;
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            txtCurrencyCode.Clear();
            datOutputAtFrom.Clear();
            datOutputAtTo.Clear();
            nmbOutputNoFrom.Clear();
            nmbOutputNoTo.Clear();
            txtFromCustomerCode.Clear();
            lblFromCustomerName.Clear();
            txtToCustomerCode.Clear();
            lblToCustomerName.Clear();
            grdReminder.DataSource = null;
            tbcReminder.SelectedTab = tabReminderSearch;
            datOutputAtFrom.Select();
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

        [OperationLog("再出力")]
        private void Reoutput()
        {
            ClearStatusMessage();
            if (!ValidateInputValueForPrint()) return;

            try
            {
                string serverPath = GetServerPath();

                var reminderOutputed = new ReminderOutputed();
                reminderOutputed.OutputNos = grdReminder.Rows.Select(x => x.DataBoundItem as ReminderOutputed)
                    .Where(x => x.Checked)
                    .Select(x => x.OutputNo).Distinct().ToArray();
                reminderOutputed.DestinationIds = grdReminder.Rows.Select(x => x.DataBoundItem as ReminderOutputed)
                    .Where(x => x.Checked && x.DestinationId != null)
                    .Select(x => x.DestinationId.Value).Distinct().ToArray();

                var path = string.Empty;
                var pdfSetting = GetPdfOutputSetting();
                if (!pdfSetting.IsAllInOne)
                {
                    var selectedPath = string.Empty;
                    var rootBrowserPath = new List<string>();
                    if (!LimitAccessFolder
                        ? !ShowFolderBrowserDialog(serverPath, out selectedPath)
                        : !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, Constants.FolderBrowserType.SelectFolder))
                    {
                        DispStatusMessage(MsgInfProcessCanceled);
                        return;
                    }
                    path = (!LimitAccessFolder ? selectedPath : rootBrowserPath.First());
                }

                if (!reminderOutputed.OutputNos.Any())
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist, "再出力");
                    return;
                }

                if (!pdfSetting.IsAllInOne && !ShowConfirmDialog(MsgQstConfirmOutput))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var reportList = new List<ReminderReport>();
                var isCanceled = true;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    List<ReminderBilling> reminderBilling = null;
                    List<ReminderTemplateSetting> templateSettings = null;

                    var reminderClient = factory.Create<ReminderServiceClient>();
                    var reminderResult = (!UseDestinationSummarized)
                        ? await reminderClient.GetReminderBillingForReprintAsync(SessionKey, CompanyId, reminderOutputed)
                        : await reminderClient.GetReminderBillingForReprintByDestinationAsync(SessionKey, CompanyId, reminderOutputed);
                    if (reminderResult.ProcessResult.Result) reminderBilling = reminderResult.ReminderBilling;

                    var templateSettingClient = factory.Create<ReminderSettingServiceClient>();
                    var templateSettingResult = await templateSettingClient.GetReminderTemplateSettingsAsync(SessionKey, CompanyId);
                    if (templateSettingResult.ProcessResult.Result) templateSettings = templateSettingResult.ReminderTemplateSettings;

                    var now = DateTime.Now;
                    ReminderReport allReport = null;

                    foreach (var r in grdReminder.Rows.Select(x => x.DataBoundItem as ReminderOutputed).Where(x => x.Checked))
                    {
                        var reminderReport = UtilReminder.CreateReminderReport(reminderBilling.Where(x => x.OutputNo == r.OutputNo),
                            ReminderCommonSetting,
                            ReminderSummarySetting,
                            templateSettings.First(x => x.Id == r.ReminderTemplateId),
                            Company,
                            ColumnNameSettingInfo,
                            now,
                            pdfSetting);


                        if (pdfSetting.IsAllInOne)
                        {
                            if (allReport == null)
                                allReport = reminderReport;
                            else
                                allReport.Document.Pages.AddRange((PagesCollection)reminderReport.Document.Pages.Clone());
                        }
                        else
                        {
                            if (r.DestinationId != null)
                                reportList.Add(reminderReport);
                            else
                                reportList.Add(reminderReport);
                        }

                    }

                    if (pdfSetting.IsAllInOne)
                    {
                        Action<Form> outputHandler = owner => isCanceled = false;
                        ShowDialogPreview(ParentForm, allReport, GetServerPath(), outputHandler);
                    }
                    else
                    {
                        var exporter = new PdfReportExporter();
                        var fileList = new List<string>();
                        foreach (var rpt in reportList)
                        {
                            var filePath = Util.GetUniqueFileName(Path.Combine(path, $"{rpt.Name}.pdf"));
                            exporter.PdfExport(rpt, filePath);
                            if (pdfSetting.UseZip)
                                fileList.Add(filePath);
                        }
                        if (pdfSetting.UseZip)
                            Util.ArchivesAsZip(fileList, path, $"督促状{now.ToString("yyyyMMdd")}", pdfSetting.MaximumByte);
                    }
                });

                var result = ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (pdfSetting.IsAllInOne && isCanceled)
                    return;
                else if (result == DialogResult.OK)
                    DispStatusMessage(MsgInfFinishedSuccessReOutputProcess);
                else
                    ShowWarningDialog(MsgErrCreateReportError);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private bool ValidateInputValueForPrint()
        {
            if (!ValidateAnyCheck())
            {
                ShowWarningDialog(MsgWngSelectionRequired, "印刷するデータ");
                return false;
            }

            for (var i = 0; i < grdReminder.Rows.Count; i++)
            {
                if (grdReminder.GetValue(i, CellName(nameof(ReminderOutputed.ReminderTemplateId))) == null)
                {
                    ShowWarningDialog(MsgWngSelectionRequired, "文面パターン");
                    grdReminder.Focus();
                    grdReminder.CurrentCell = grdReminder.Rows[i][CellName(nameof(ReminderOutputed.ReminderTemplateId))];
                    return false;
                }
            }

            return true;
        }

        private bool ValidateAnyCheck()
        {
            if (!grdReminder.Rows.Select(x => x.DataBoundItem as ReminderOutputed).Any(x => x.Checked))
            {
                return false;
            }
            return true;
        }

        /// <summary>Get Server Path </summary>
        /// <returns>Server Path</returns>
        private string GetServerPath()
        {
            var serverPath = "";
            var login = ApplicationContext.Login;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                var result = await service.GetByCodeAsync(
                        login.SessionKey, login.CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return serverPath;
        }
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
                var reminder = row.DataBoundItem as ReminderOutputed;
                reminder.Checked = check;
            }
        }

        [OperationLog("戻る")]
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

        #endregion

        #region GridEventHandler

        private void grdReminder_CellClick(object sender, CellEventArgs e)
        {
            if (e.CellName != CellName("Preview")) return;

            var data = grdReminder.Rows[e.RowIndex].DataBoundItem as ReminderOutputed;
            if (data == null) return;

            try
            {
                data.OutputNos = new int[] { data.OutputNo };
                if (UseDestinationSummarized && data.DestinationId.HasValue)
                    data.DestinationIds = new int[] { data.DestinationId.Value };

                string serverPath = GetServerPath();

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    List<ReminderBilling> reminderBilling = null;
                    List<ReminderTemplateSetting> templateSettings = null;

                    var reminderClient = factory.Create<ReminderServiceClient>();
                    var reminderResult = (UseDestinationSummarized && data.DestinationId.HasValue)
                                            ? await reminderClient.GetReminderBillingForReprintByDestinationAsync(SessionKey, CompanyId, data)
                                            : await reminderClient.GetReminderBillingForReprintAsync(SessionKey, CompanyId, data);

                    if (!reminderResult.ProcessResult.Result)
                    {
                        ShowWarningDialog(MsgErrCreateReportError);
                        return;
                    }

                    reminderBilling = reminderResult.ReminderBilling;

                    var templateSettingClient = factory.Create<ReminderSettingServiceClient>();
                    var templateSettingResult = await templateSettingClient.GetReminderTemplateSettingsAsync(SessionKey, CompanyId);
                    if (templateSettingResult.ProcessResult.Result) templateSettings = templateSettingResult.ReminderTemplateSettings;

                    var pdfSetting = GetPdfOutputSetting();
                    var reminderReport = UtilReminder.CreateReminderReport(reminderBilling,
                        ReminderCommonSetting,
                        ReminderSummarySetting,
                        templateSettings.First(x => x.Id == data.ReminderTemplateId),
                        Company,
                        ColumnNameSettingInfo,
                        DateTime.Now,
                        pdfSetting);

                    ShowDialogPreview(ParentForm, reminderReport, serverPath);
                });
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        #endregion
    }
}
