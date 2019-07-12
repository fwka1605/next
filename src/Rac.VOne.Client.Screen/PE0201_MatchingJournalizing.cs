using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CollationSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ExportFieldSettingMasterService;
using Rac.VOne.Client.Screen.MatchingService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>消込仕訳出力</summary>
    public partial class PE0201 : VOneScreenBase
    {
        private int CurrencyId { get; set; }
        private int precision;
        /// <summary>小数点以下桁数</summary>
        private int Precision
        {
            get { return precision; }
            set {
                precision = value;
                MoneyFormat      = precision == 0 ? "#,##0" : ("#,##0." + new string('0', precision));
                MoneyFormatPlane = precision == 0 ?     "0" : (    "0." + new string('0', precision));
            }
        }
        private const string CountFormat = "#,##0";
        private string MoneyFormat { get; set; } = "#,##0";
        private string MoneyFormatPlane { get; set; } = "0";

        private List<MatchingJournalizing> ExtractMatching { get; set; }
        private GeneralJournalizingsResult ExtractGeneralResult { get; set; }
        private List<GeneralJournalizing> ExtractGeneral { get { return ExtractGeneralResult?.GeneralJournalizings; } }
        private int ExtractedCount { get { return IsStandardPattern
                    ? ExtractMatching?.Count ?? 0
                    : ExtractGeneral?.Count ?? 0; } }
        private decimal ExtractedAmount { get { return IsStandardPattern
                    ? ExtractMatching?.Sum(x => x.Amount) ?? 0M
                    : ExtractGeneral?.Sum(x => x.Amount) ?? 0M; } }
        private CollationSetting CollationSetting { get; set; }
        /// <summary>照合設定 仕訳出力設定 0 : 標準 の場合に true
        /// 標準の時のみ帳票印刷処理を許可</summary>
        private bool IsStandardPattern { get { return CollationSetting?.JournalizingPattern == 0; } }

        private string CellName(string value) => $"cel{value}";

        private List<ColumnNameSetting> ColumnNames { get; set; }


        #region 初期化

        public PE0201()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            grid.SetupShortcutKeys();
            Text = "消込仕訳出力";
            cbxExportMatchedReceiptData.CheckedChanged += (sender, e) =>
            {
                cbxContainAdvanceReceivedOccured.Enabled = cbxExportMatchedReceiptData.Checked;
                cbxContainAdvanceReceivedMatching.Enabled = cbxExportMatchedReceiptData.Checked;
                if (!cbxExportMatchedReceiptData.Checked)
                {
                    cbxContainAdvanceReceivedOccured.Checked = false;
                    cbxContainAdvanceReceivedMatching.Checked = false;
                }
            };
        }

        private void PE0201_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);

                ClearControlValues();
                Settings.SetCheckBoxValue<PE0201>(Login, cbxExportMatchedReceiptData);
                Settings.SetCheckBoxValue<PE0201>(Login, cbxContainAdvanceReceivedOccured);
                Settings.SetCheckBoxValue<PE0201>(Login, cbxContainAdvanceReceivedMatching);

                InitializeGridTemplate();
                InitializeControlsValue();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task InitializeLoadDataAsync()
        {
            var loadDefaultCurrencyTask = GetCurrencyAsync(DefaultCurrencyCode);
            //tasks.Add(loadDefaultCurrencyTask);
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadCollationSettingAsync(),
                LoadColumnNameSettingAsync(),
                loadDefaultCurrencyTask
            };
            await Task.WhenAll(tasks);
            var currency = loadDefaultCurrencyTask.Result;
            if (currency != null)
            {
                CurrencyId = currency.Id;
                Precision = currency.Precision;
            }
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("抽出");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction04Caption("出力");
            BaseContext.SetFunction05Caption("出力設定");
            BaseContext.SetFunction06Caption("再出力");
            BaseContext.SetFunction07Caption("再印刷");
            BaseContext.SetFunction08Caption("取消");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            OnF01ClickHandler = Extract;
            OnF02ClickHandler = ClearMatchingJournalizing;
            OnF03ClickHandler = PrintMatchingJournalizing;
            OnF04ClickHandler = ExportMatchingJournalizing;
            OnF05ClickHandler = CallExportFieldSetting;
            OnF06ClickHandler = ReExportMatchingJournalizing;
            OnF07ClickHandler = RePrintMatchingJournalizing;
            OnF08ClickHandler = CancelMatchingJournalizing;
            OnF10ClickHandler = ExitMatchingJournalizing;
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            var widthCcy = (UseForeignCurrency ? 80 : 0);
            var widthAmt = (UseForeignCurrency ? 100 : 180);
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 50      , nameof(JournalizingSummary.Selected)    , dataField: nameof(JournalizingSummary.Selected)    , caption: "選択"  , cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false),
                new CellSetting(height, 135     , nameof(JournalizingSummary.OutputAt)    , dataField: nameof(JournalizingSummary.OutputAt)    , caption: "仕訳日", cell: builder.GetDateCell_yyyyMMddHHmmss(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 100     , nameof(JournalizingSummary.Count)       , dataField: nameof(JournalizingSummary.Count)       , caption: "件数"  , cell: builder.GetNumberCell(MultiRowContentAlignment.MiddleRight)),
                new CellSetting(height, widthCcy, nameof(JournalizingSummary.CurrencyCode), dataField: nameof(JournalizingSummary.CurrencyCode), caption: "通貨"  , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, widthAmt, nameof(JournalizingSummary.Amount)      , dataField: nameof(JournalizingSummary.Amount)      , caption: "金額"  , cell: builder.GetTextBoxCurrencyCell(Precision)),
            });

            grid.Template = builder.Build();
            grid.HideSelection = true;
        }

        private void InitializeControlsValue()
        {
            txtCurrencyCode.Clear();

            if (!UseForeignCurrency)
            {
                lblCurrencyCode.Visible = false;
                txtCurrencyCode.Visible = false;
                btnCurrencyCode.Visible = false;

                DisplayGridData();
            }
            else
            {
                BaseContext.SetFunction01Enabled(false);
                grid.DataSource = null;
            }

            var path = WriteFilePath();

            if (!string.IsNullOrWhiteSpace(path))
            {
                var fileName = $"消込仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                txtFilePath.Text = Path.Combine(path, fileName);
            }
            txtFilePath.Select();
            txtFilePath.Enabled = !LimitAccessFolder;
        }

        #endregion

        #region ファンクションキーイベント

        [OperationLog("抽出")]
        private void Extract()
        {
            try
            {
                ClearStatusMessage();

                if (!datRecordedAtFrom.ValidateRange(datRecordedAtTo,
                    () => ShowWarningDialog(MsgWngInputRangeChecked, lblRecordedAt.Text))) return;

                var extractTask = ExtractJournalizingAsync();

                NLogHandler.WriteDebug(this, "消込仕訳出力 抽出開始");
                ProgressDialog.Start(ParentForm, extractTask, false, SessionKey);
                NLogHandler.WriteDebug(this, "消込仕訳出力 抽出終了");

                if (extractTask.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, extractTask.Exception, SessionKey);
                    ShowWarningDialog(MsgErrSomethingError, "抽出");
                    return;
                }

                var result = extractTask.Result;

                if (result) DispStatusMessage(MsgInfDataExtracted);
                else ShowWarningDialog(MsgWngNotExistSearchData);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void ClearMatchingJournalizing()
        {
            ClearControlValues();
            txtCurrencyCode.Clear();
            txtFilePath.Select();

            if (UseForeignCurrency)
            {
                BaseContext.SetFunction01Enabled(false);
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction07Enabled(false);
                BaseContext.SetFunction08Enabled(false);

                grid.DataSource = null;
            }

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            datRecordedAtFrom.Enabled = true;
            datRecordedAtTo.Enabled = true;
            txtCurrencyCode.Enabled = true;
            btnCurrencyCode.Enabled = true;
            ClearStatusMessage();
        }

        [OperationLog("印刷")]
        private void PrintMatchingJournalizing()
        {
            try
            {
                ClearStatusMessage();

                if (!ValidateAuthorization()) return;

                var extractMatching = GetFilteredExtractData(ExtractMatching);
                DisplayPrintData(extractMatching);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("出力")]
        private void ExportMatchingJournalizing()
        {
            try
            {
                ClearStatusMessage();
                if (!ValidateForExport()
                 || !ValidateAuthorization()) return;

                var filePathMatchedReceipt = string.Empty;
                var fileName = $"消込済み入金データ_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (cbxExportMatchedReceiptData.Checked
                    && !ShowSaveFileDialog(GetDirectory(), fileName, out filePathMatchedReceipt,
                        cancellationMessaging: () => DispStatusMessage(MsgInfProcessCanceled))) return;

                ProgressDialog.Start(ParentForm, ExportMatchingJournalizingInner(filePathMatchedReceipt), false, SessionKey);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "出力");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }


        [OperationLog("再出力")]
        private void ReExportMatchingJournalizing()
        {
            try
            {
                ClearStatusMessage();
                var pathMatching = string.Empty;
                var pathReceipt = string.Empty;

                if (!ShowSaveFileDialog(GetDirectory(),
                    $"再出力消込仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                    out pathMatching,
                    cancellationMessaging: () => DispStatusMessage(MsgInfProcessCanceled))) return;

                if (cbxExportMatchedReceiptData.Checked
                    && !ShowSaveFileDialog(GetDirectory(),
                            $"再出力消込済み入金データ_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                            out pathReceipt,
                            cancellationMessaging: () => DispStatusMessage(MsgInfProcessCanceled))) return;

                var outputAt = GetSelectedOutputAt();

                var reoutputResult = ReoutputMatchingJournalizingInner(outputAt, pathMatching, pathReceipt);

                if (reoutputResult)
                    DispStatusMessage(MsgInfFinishedSuccessReOutputProcess);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "出力");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("再印刷")]
        private void RePrintMatchingJournalizing()
        {
            try
            {
                ClearStatusMessage();
                var outputAt = GetSelectedOutputAt();
                DisplayPrintData(null, outputAt, true);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("終了")]
        private void ExitMatchingJournalizing()
        {
            Settings.SaveControlValue<PE0201>(Login, cbxExportMatchedReceiptData.Name, cbxExportMatchedReceiptData.Checked);
            Settings.SaveControlValue<PE0201>(Login, cbxContainAdvanceReceivedOccured.Name, cbxContainAdvanceReceivedOccured.Checked);
            Settings.SaveControlValue<PE0201>(Login, cbxContainAdvanceReceivedMatching.Name, cbxContainAdvanceReceivedMatching.Checked);
            ParentForm.Close();
        }

        [OperationLog("出力設定")]
        private void CallExportFieldSetting()
        {
            ClearStatusMessage();
            using (var form = ApplicationContext.Create(nameof(PH9904)))
            {
                var screen = form.GetAll<PH9904>().First();
                screen.ExportFileType = 1;
                screen.InitializeParentForm("出力項目設定");
                ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }

        [OperationLog("取消")]
        private void CancelMatchingJournalizing()
        {
            try
            {
                ClearStatusMessage();
                if (!ShowConfirmDialog(MsgQstConfirmCancelJournalizing)) return;
                var task = CancelMatchingJournalizingInnerAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrSomethingError, "取消");
                    return;
                }
                DispStatusMessage(MsgInfFinishedSuccessJournalizingCancelingProcess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region extract

        private async Task<bool> ExtractJournalizingAsync()
        {
            var enable = false;
            if (IsStandardPattern)
            {
                ExtractMatching = await ExtractMatchingJournalizingAsync();
                enable = ExtractMatching.Any();
            }
            else
            {
                ExtractGeneralResult = await ExtractGeneralJournalizingAsync();
                enable = ExtractGeneral.Any();
            }
            datRecordedAtFrom.Enabled = ! enable;
            datRecordedAtTo  .Enabled = ! enable;
            lblOutputNumber.Clear();
            lblOutputAmount.Clear();
            BaseContext.SetFunction03Enabled(enable && IsStandardPattern);
            BaseContext.SetFunction04Enabled(enable);
            lblExtractNumber.Text = ExtractedCount.ToString(CountFormat);
            lblExtractAmount.Text = ExtractedAmount.ToString(MoneyFormat);
            return enable;
        }

        #endregion

        #region output journalizing

        private async Task<bool> ExportMatchingJournalizingInner(string filePathMatchedReceipt)
        {
            var summary = await GetMatchingJournalizingSummary(isOutputted: false);
            var filePathMatching = txtFilePath.Text;
            var count = 0;
            var amount = 0M;
            if (IsStandardPattern)
            {
                var source = GetFilteredExtractData(ExtractMatching);
                count = source.Count;
                amount = source.Sum(x => x.Amount);
                if (summary.Sum(x => x.Amount) != amount)
                {
                    ShowWarningDialog(MsgWngNotEqualAbstractAmountAndUpdateAmount);
                    return false;
                }
                if (!ExportMatchingJournalizing(filePathMatching, source)) return false;
            }
            else
            {
                var extracted = ExtractGeneralResult;
                var source = GetFilteredExtractData(extracted.GeneralJournalizings);
                extracted.GeneralJournalizings = source;
                count = source.Count;
                amount = source.Sum(x => x.Amount);
                if (summary.Sum(x => x.Amount) != amount)
                {
                    ShowWarningDialog(MsgWngNotEqualAbstractAmountAndUpdateAmount);
                    return false;
                }
                if (!ExportGeneralJournalizing(filePathMatching, extracted)) return false;
            }
            if (cbxExportMatchedReceiptData.Checked)
            {
                var tasks = new List<Task>();
                var loadMatchedReceiptTask = GetMatchedReceiptDataAsync();
                tasks.Add(loadMatchedReceiptTask);
                var loadExportFieldSettingTask = GetExportFieldSettingAsync();
                tasks.Add(loadExportFieldSettingTask);
                await Task.WhenAll(tasks);

                var settings = loadExportFieldSettingTask?.Result;
                var source =  loadMatchedReceiptTask?.Result;
                if (!ExportMatchedReceiptData(filePathMatchedReceipt, source, settings)) return false;
            }

            var updateResult = await UpdateOutputAtAsync();
            if (!updateResult)
            {
                ShowWarningDialog(MsgErrExportError);
                return false;
            }
            var summaries = await GetMatchingJournalizingSummary();
            if (summaries != null)
            {
                grid.DataSource = summaries;
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction07Enabled(false);
                BaseContext.SetFunction08Enabled(false);
            }
            lblOutputNumber.Text = count.ToString(CountFormat);
            lblOutputAmount.Text = amount.ToString(MoneyFormat);

            DispStatusMessage(MsgInfFinishDataExtracting);
            BaseContext.SetFunction04Enabled(false);
            return true;
        }

        #endregion

        #region cancel journalizing
        private async Task<bool> CancelMatchingJournalizingInnerAsync()
        {
            var outputAt = GetSelectedOutputAt();
            var result = await CancelOuputAtAsync(outputAt);
            if (!result) return false;

            var summaries = await GetMatchingJournalizingSummary();
            if (summaries != null)
                grid.DataSource = summaries;

            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);

            return true;
        }

        #endregion

        #region re-output journalizing

        private bool ReoutputMatchingJournalizingInner(
            List<DateTime> outputAt, string pathMatching, string pathReceipt)
        {
            Task<List<MatchingJournalizing>> loadMatchingJournalizingTask = null;
            Task<GeneralJournalizingsResult> loadGeneralJournalizingTask = null;
            Task<List<MatchedReceipt>> loadMatchedReceiptTask = null;
            Task<List<ExportFieldSetting>> loadExportFieldSettingTask = null;

            var tasks = new List<Task>();
            if (IsStandardPattern)
                tasks.Add(loadMatchingJournalizingTask = ExtractMatchingJournalizingAsync(outputAt));
            else
                tasks.Add(loadGeneralJournalizingTask = ExtractGeneralJournalizingAsync(outputAt));
            if (cbxExportMatchedReceiptData.Checked)
            {
                tasks.Add(loadMatchedReceiptTask = GetMatchedReceiptDataAsync(outputAt));
                tasks.Add(loadExportFieldSettingTask = GetExportFieldSettingAsync());
            }

            ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
            if (tasks.Any(x => x.Exception != null))
            {
                foreach (var ex in tasks.Where(x => x.Exception != null).Select(x => x.Exception))
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrExportError);
                return false;
            }

            if ( IsStandardPattern && !ExportMatchingJournalizing(pathMatching, loadMatchingJournalizingTask.Result)) return false;
            if (!IsStandardPattern && !ExportGeneralJournalizing(pathMatching , loadGeneralJournalizingTask.Result)) return false;
            if (cbxExportMatchedReceiptData.Checked)
            {
                var matchedReceiptList = loadMatchedReceiptTask?.Result;
                var exportFieldSettingList = loadExportFieldSettingTask?.Result;
                ExportMatchedReceiptData(pathReceipt, matchedReceiptList, exportFieldSettingList);
            }
            return true;
        }


        #endregion

        #region その他イベント

        private void ClearControlValues()
        {
            datRecordedAtFrom.Clear();
            datRecordedAtTo.Clear();
            lblExtractAmount.Clear();
            lblExtractNumber.Clear();
            lblOutputAmount.Clear();
            lblOutputNumber.Clear();
        }

        private void DisplayGridData()
        {
            var task  = GetMatchingJournalizingSummary();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var summary = task.Result;
            grid.DataSource = new BindingSource(summary, null);
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();

            if (string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                txtCurrencyCode.Clear();
                CurrencyId = 0;
                return;
            }
            try
            {
                var code = txtCurrencyCode.Text;
                var task = GetCurrencyAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var currency = task.Result;
                if (currency == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                    txtCurrencyCode.Clear();
                    txtCurrencyCode.Focus();
                    CurrencyId = 0;
                    return;
                }
                CurrencyId = currency.Id;
                Precision = currency.Precision;
                ClearStatusMessage();
                DisplayCurrencyCode();

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                CurrencyId = currency.Id;
                Precision = currency.Precision;
                ClearStatusMessage();
                DisplayCurrencyCode();
            }
        }

        private void DisplayCurrencyCode()
        {
            BaseContext.SetFunction01Enabled(true);
            txtCurrencyCode.Enabled = false;
            btnCurrencyCode.Enabled = false;
            InitializeGridTemplate();
            DisplayGridData();
        }

        private void btnWriteFile_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var filePath = string.Empty;
            var fileName = $"消込仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            if (!ShowSaveFileDialog(GetDirectory(), fileName, out filePath)) return;
            txtFilePath.Text = filePath;
        }

        private string GetDirectory()
        {
            var filePath = txtFilePath.Text;
            var initialDirectory = string.Empty;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                return initialDirectory;
            }

            if (Directory.Exists(filePath))
                return filePath;

            try
            {
                initialDirectory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(initialDirectory))
                    initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            catch (Exception)
            {
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            return initialDirectory;
        }

        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellName != CellName(nameof(JournalizingSummary.Selected))) return;
            grid.EndEdit();
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellName != CellName(nameof(JournalizingSummary.Selected))) return;
            var enable = grid.Rows.Any(x => Convert.ToBoolean(x[CellName(nameof(JournalizingSummary.Selected))].EditedFormattedValue));
            BaseContext.SetFunction06Enabled(enable);
            BaseContext.SetFunction07Enabled(enable && IsStandardPattern);
            BaseContext.SetFunction08Enabled(enable);
        }

        #endregion

        #region Web Service

        private string WriteFilePath()
        {
            var serverPath = string.Empty;
            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            serverPath = task.Result;
            return serverPath;
        }

        /// <summary>仕訳出力 履歴<see cref="JournalizingSummary"/>取得</summary>
        /// <param name="isOutputted">仕訳出力時 false 指定 金額検証用 抽出後 金額が変わっていないか確認する用途</param>
        /// <returns></returns>
        private async Task<List<JournalizingSummary>> GetMatchingJournalizingSummary(bool isOutputted = true)
        {
            var option = GetJournalizingOption();
            option.IsOutputted = isOutputted;
            if (isOutputted)
            {
                option.RecordedAtFrom = null;
                option.RecordedAtTo = null;
            }
            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.GetMatchingJournalizingSummaryAsync(SessionKey, option));
            if (result.ProcessResult.Result)
                return result.JournalizingsSummaries;
            return null;
        }

        private async Task<bool> UpdateOutputAtAsync()
        {
            var option = GetJournalizingOption();
            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.UpdateOutputAtAsync(SessionKey, option));
            return result?.ProcessResult.Result ?? false;
        }

        private async Task<bool> CancelOuputAtAsync(List<DateTime> outputAt)
        {
            var option = GetJournalizingOption(outputAt);
            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.CancelMatchingJournalizingAsync(SessionKey, option));
            return result?.ProcessResult.Result ?? false;
        }

        private async Task<Currency> GetCurrencyAsync(string code)
        {
            var result = await ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client)
                => await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code }));
            if (result.ProcessResult.Result)
                return result.Currencies.FirstOrDefault();
            return null;
        }

        private async Task<List<ExportFieldSetting>> GetExportFieldSettingAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (ExportFieldSettingMasterClient client)
                => await client.GetItemsByExportFileTypeAsync(SessionKey, CompanyId, (int)CsvExportFileType.MatchedReceiptData));
            if (result.ProcessResult.Result)
                return result.ExportFieldSettings;
            return new List<ExportFieldSetting>();
        }

        private async Task<List<MatchingJournalizing>> ExtractMatchingJournalizingAsync(List<DateTime> outputAt = null)
        {
            var option = GetJournalizingOption(outputAt);
            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.ExtractMatchingJournalizingAsync(SessionKey, option));
            if (result.ProcessResult.Result)
                return result.MatchingJournalizings;
            return new List<MatchingJournalizing>();
        }

        /// <summary>汎用仕訳出力データ抽出処理
        /// 関連マスターの dictionary を GeneralJournalizingsResult に保持</summary>
        /// <param name="outputAt"></param>
        /// <returns></returns>
        private async Task<GeneralJournalizingsResult> ExtractGeneralJournalizingAsync(List<DateTime> outputAt = null)
        {
            var option = GetJournalizingOption(outputAt);
            var getResult = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.ExtractGeneralJournalizingAsync(SessionKey, option));
            if (!(getResult?.ProcessResult.Result ?? false)) return null;
            var details = getResult.GeneralJournalizings;
            await LoadTransactionModelsAndCustomersAsync(details);
            await LoadMasterAndHeadersAsync(getResult);
            return getResult;
        }
        private async Task LoadTransactionModelsAndCustomersAsync(List<GeneralJournalizing> details)
        {
            var customerIds = new HashSet<int>();
            var receiptIds = new HashSet<long>();
            var excludeIds = new HashSet<long>();
            var matchingIds = new HashSet<long>();
            var billingIds = new HashSet<long>();
            var advanceIds = new HashSet<long>();
            foreach (var detail in details)
            {
                foreach (var id in detail.GetCustomerIds().Distinct()) AddId(customerIds, id);
                AddId(receiptIds, detail.ReceiptId);
                AddId(receiptIds, detail.ScheduledIncomeReceiptId);
                AddId(billingIds, detail.BillingId);
                AddId(matchingIds, detail.MatchingId);
                AddId(excludeIds, detail.ReceiptExcludeId);
                AddId(advanceIds, detail.AdvanceReceivedBackupId);
            }
            var dicReceipt = new Dictionary<long, Receipt>();
            var dicMemo = new Dictionary<long, ReceiptMemo>();
            var dicExclude = new Dictionary<long, ReceiptExclude>();
            var dicBilling = new Dictionary<long, Billing>();
            var dicMatching = new Dictionary<long, Matching>();
            var dicAdvance = new Dictionary<long, AdvanceReceivedBackup>();
            var dicCustomer = new Dictionary<int, Customer>();
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var tasks = new List<Task>();
                if (receiptIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<ReceiptService.ReceiptServiceClient>();
                    var result = await client.GetAsync(SessionKey, receiptIds.ToArray());
                    if (result.ProcessResult.Result) dicReceipt = result.Receipts.ToDictionary(x => x.Id);
                }));
                if (receiptIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<ReceiptMemoService.ReceiptMemoServiceClient>();
                    var result = await client.GetItemsAsync(SessionKey, receiptIds.ToArray());
                    if (result.ProcessResult.Result) dicMemo = result.ReceiptMemo.ToDictionary(x => x.ReceiptId);
                }));
                if (excludeIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<ReceiptExcludeService.ReceiptExcludeServiceClient>();
                    var result = await client.GetByIdsAsync(SessionKey, excludeIds.ToArray());
                    if (result.ProcessResult.Result) dicExclude = result.ReceiptExcludes.ToDictionary(x => x.Id);
                }));
                if (billingIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<BillingService.BillingServiceClient>();
                    var result = await client.GetAsync(SessionKey, billingIds.ToArray());
                    if (result.ProcessResult.Result) dicBilling = result.Billing.ToDictionary(x => x.Id);
                }));
                if (matchingIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<MatchingServiceClient>();
                    var result = await client.GetAsync(SessionKey, matchingIds.ToArray());
                    if (result.ProcessResult.Result) dicMatching = result.Matchings.ToDictionary(x => x.Id);
                }));
                if (advanceIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<AdvanceReceivedBackupService.AdvanceReceivedBackupServiceClient>();
                    var result = await client.GetByIdsAsync(SessionKey, advanceIds.ToArray());
                    if (result.ProcessResult.Result) dicAdvance = result.AdvanceReceivedBackups.ToDictionary(x => x.Id);
                }));
                if (customerIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<CustomerMasterService.CustomerMasterClient>();
                    var result = await client.GetAsync(SessionKey, customerIds.ToArray());
                    if (result.ProcessResult.Result) dicCustomer = result.Customers.ToDictionary(x => x.Id);
                }));
                await Task.WhenAll(tasks);
            });
            foreach (var detail in details)
            {
                detail.Receipt = GetModel(dicReceipt, detail.ReceiptId);
                detail.ReceiptMemo = GetModel(dicMemo, detail.ReceiptId);
                detail.ScheduledReceipt = GetModel(dicReceipt, detail.ScheduledIncomeReceiptId);
                detail.ScheduledReceiptMemo = GetModel(dicMemo, detail.ScheduledIncomeReceiptId);
                detail.Billing = GetModel(dicBilling, detail.BillingId);
                detail.Matching = GetModel(dicMatching, detail.MatchingId);
                detail.ReceiptExclude = GetModel(dicExclude, detail.ReceiptExcludeId);
                detail.AdvanceReceipt = GetModel(dicAdvance, detail.AdvanceReceivedBackupId);
                detail.Customer = GetModel(dicCustomer, detail.BillingCustomerId);
                detail.ParentCustomer = GetModel(dicCustomer, detail.BillingParentCustomerId);
                detail.ReceiptCustomer = GetModel(dicCustomer, detail.ReceiptCustomerId);
                detail.ReceiptParentCustomer = GetModel(dicCustomer, detail.ReceiptParentCustomerId);
                detail.AdvanceReceivedCustomer = GetModel(dicCustomer, detail.AdvanceReceivedCustomerId);
                detail.AdvanceReceivedParentCustomer = GetModel(dicCustomer, detail.AdvanceReceivedParentCustomerId);
            }
        }
        private async Task LoadMasterAndHeadersAsync(GeneralJournalizingsResult source)
        {
            var details = source.GeneralJournalizings;
            var categoryIds = new HashSet<int>();
            var staffIds = new HashSet<int>();
            var loginUserIds = new HashSet<int>();
            var receiptHeaderIds = new HashSet<long>();
            var matchingHeaderIds = new HashSet<long>();
            foreach (var detail in details)
            {
                foreach (var id in detail.GetCategoryIds().Distinct()) AddId(categoryIds, id);
                foreach (var id in detail.GetStaffIds().Distinct()) AddId(staffIds, id);
                foreach (var id in detail.GetLoginUserIds().Distinct()) AddId(loginUserIds, id);
                foreach (var id in detail.GetReceiptHeaderIds().Distinct()) AddId(receiptHeaderIds, id);
                AddId(matchingHeaderIds, detail.MatchingHeaderId);
            }
            var sectionIds = new HashSet<int>();
            var dicCategory = new Dictionary<int, Category>();
            var dicSection = new Dictionary<int, Web.Models.Section>();
            var dicStaff = new Dictionary<int, Staff>();
            var dicLoginUser = new Dictionary<int, LoginUser>();
            var dicReceiptHeader = new Dictionary<long, ReceiptHeader>();
            var dicMatchingHeader = new Dictionary<long, MatchingHeader>();
            var dicBankAccount = new Dictionary<Tuple<string, string, int, string>, BankAccount>();
            var dicGeneralSettings = new Dictionary<string, string>();
            var dicDpeartment = new Dictionary<int, Department>();
            var dicDepartmentByCode = new Dictionary<string, Department>();
            var dicAccountTitle = new Dictionary<int, AccountTitle>();
            var dicAccountTitleByCode = new Dictionary<string, AccountTitle>();
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var tasks = new List<Task>();
                if (categoryIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<CategoryMasterService.CategoryMasterClient>();
                    var result = await client.GetAsync(SessionKey, categoryIds.ToArray());
                    if (result.ProcessResult.Result) dicCategory = result.Categories.ToDictionary(x => x.Id);
                }));
                if (staffIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<StaffMasterService.StaffMasterClient>();
                    var result = await client.GetAsync(SessionKey, staffIds.ToArray());
                    if (result.ProcessResult.Result) dicStaff = result.Staffs.ToDictionary(x => x.Id);
                }));
                if (loginUserIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<LoginUserMasterService.LoginUserMasterClient>();
                    var result = await client.GetAsync(SessionKey, loginUserIds.ToArray());
                    if (result.ProcessResult.Result) dicLoginUser = result.Users.ToDictionary(x => x.Id);
                }));
                if (receiptHeaderIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<ReceiptHeaderService.ReceiptHeaderServiceClient>();
                    var result = await client.GetAsync(SessionKey, receiptHeaderIds.ToArray());
                    if (result.ProcessResult.Result) dicReceiptHeader = result.ReceiptHeaders.ToDictionary(x => x.Id);
                }));
                if (matchingHeaderIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<MatchingServiceClient>();
                    var result = await client.GetHeaderItemsAsync(SessionKey, matchingHeaderIds.ToArray());
                    if (result.ProcessResult.Result) dicMatchingHeader = result.MatchingHeaders.ToDictionary(x => x.Id);
                }));
                tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<BankAccountMasterService.BankAccountMasterClient>();
                    var result = await client.GetItemsAsync(SessionKey, CompanyId, new BankAccountSearch { CompanyId = CompanyId });
                    if (result.ProcessResult.Result)
                        dicBankAccount = result.BankAccounts.ToDictionary(x
                            => Tuple.Create(x.BankCode, x.BranchCode, x.AccountTypeId, x.AccountNumber));
                }));
                tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<GeneralSettingMasterService.GeneralSettingMasterClient>();
                    var result = await client.GetItemsAsync(SessionKey, CompanyId);
                    if (result.ProcessResult.Result) dicGeneralSettings = result.GeneralSettings.ToDictionary(x => x.Code, x => x.Value);
                }));
                tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<DepartmentMasterService.DepartmentMasterClient>();
                    var result = await client.GetItemsAsync(SessionKey, CompanyId);
                    if (result.ProcessResult.Result)
                    {
                        dicDpeartment = result.Departments.ToDictionary(x => x.Id);
                        dicDepartmentByCode = result.Departments.ToDictionary(x => x.Code);
                    }
                }));
                tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<AccountTitleMasterService.AccountTitleMasterClient>();
                    var result = await client.GetItemsAsync(SessionKey, new AccountTitleSearch { CompanyId = CompanyId });
                    if (result.ProcessResult.Result)
                    {
                        dicAccountTitle = result.AccountTitles.ToDictionary(x => x.Id);
                        dicAccountTitleByCode = result.AccountTitles.ToDictionary(x => x.Code);
                    }
                }));
                await Task.WhenAll(tasks);
            });
            var paymentAgencyIds = new HashSet<int>();
            var bankAccountKeys = new HashSet<Tuple<string, string, int, string>>();
            #region general settings
            var suspentDepartment = GetModel(dicDepartmentByCode, dicGeneralSettings, "仮受部門コード");
            var suspentAccountTitle = GetModel(dicAccountTitleByCode, dicGeneralSettings, "仮受科目コード");
            var feeDepartment = GetModel(dicDepartmentByCode, dicGeneralSettings, "振込手数料部門コード");
            var feeAccountTitle = GetModel(dicAccountTitleByCode, dicGeneralSettings, "振込手数料科目コード");
            var debitTaxDepartment = GetModel(dicDepartmentByCode, dicGeneralSettings, "借方消費税誤差部門コード");
            var debitTaxAccountTitle = GetModel(dicAccountTitleByCode, dicGeneralSettings, "借方消費税誤差科目コード");
            var creditTaxDepartment = GetModel(dicDepartmentByCode, dicGeneralSettings, "貸方消費税誤差部門コード");
            var creditTaxAccountTitle = GetModel(dicAccountTitleByCode, dicGeneralSettings, "貸方消費税誤差科目コード");
            var receiptDepartment = GetModel(dicDepartmentByCode, dicGeneralSettings, "入金部門コード");
            #endregion
            foreach (var detail in details)
            {
                detail.GeneralSettings = dicGeneralSettings;
                detail.SuspentReceiptDepartment = suspentDepartment;
                detail.SuspentReceiptAccountTitle = suspentAccountTitle;
                detail.TransferFeeDepartment = feeDepartment;
                detail.TransferFeeAccountTitle = feeAccountTitle;
                detail.DebitTaxDiffDepartment = debitTaxDepartment;
                detail.DebitTaxDiffAccountTitle = debitTaxAccountTitle;
                detail.CreditTaxDiffDepartment = creditTaxDepartment;
                detail.CreditTaxDiffAccountTitle = creditTaxAccountTitle;
                detail.ReceiptDepartment = receiptDepartment;
                detail.SetCategory(dicCategory);
                detail.ReceiptHeader = GetModel(dicReceiptHeader, detail.ReceiptHeaderId);
                detail.ScheduledReceiptHeader = GetModel(dicReceiptHeader, detail.ScheduledReceiptHeaderId);
                detail.AdvanceReceivedHeader = GetModel(dicReceiptHeader, detail.AdvanceReceivedHeaderId);
                detail.MatchingHeader = GetModel(dicMatchingHeader, detail.MatchingHeaderId);
                AddId(paymentAgencyIds, detail.CollectCategoryPaymentAgencyId);
                detail.SetBankAccount(dicBankAccount);
                foreach (var id in detail.GetSectionIds().Distinct()) AddId(sectionIds, id);
            }
            var dicPaymentAgency = new Dictionary<int, PaymentAgency>();
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var tasks = new List<Task>();
                if (sectionIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<SectionMasterService.SectionMasterClient>();
                    var result = await client.GetAsync(SessionKey, sectionIds.ToArray());
                    if (result.ProcessResult.Result) dicSection = result.Sections.ToDictionary(x => x.Id);
                }));
                if (paymentAgencyIds.Any()) tasks.Add(Task.Run(async () =>
                {
                    var client = factory.Create<PaymentAgencyMasterService.PaymentAgencyMasterClient>();
                    var result = await client.GetAsync(SessionKey, paymentAgencyIds.ToArray());
                    if (result.ProcessResult.Result) dicPaymentAgency = result.PaymentAgencies.ToDictionary(x => x.Id);
                }));
                await Task.WhenAll(tasks);
            });

            source.DepartmentDictionary = dicDpeartment;
            source.StaffDictionary = dicStaff;
            source.LoginUserDictionary = dicLoginUser;
            source.SectionDictionary = dicSection;
            source.AccountTitleDictionary = dicAccountTitle;
            source.PaymentAgencyDictionary = dicPaymentAgency;
        }
        private void AddId<TValue>(HashSet<TValue> ids, TValue? id) where TValue : struct
        {
            if (!id.HasValue) return;
            AddId(ids, id.Value);
        }
        private void AddId<TValue>(HashSet<TValue> ids, TValue id) where TValue : struct
        {
            if (ids.Contains(id)) return;
            ids.Add(id);
        }
        private TModel GetModel<TValue, TModel>(Dictionary<TValue, TModel> dic, TValue? id)
            where TValue : struct
            where TModel : class
        {
            if (!id.HasValue) return null;
            return GetModel(dic, id.Value);
        }
        private TModel GetModel<TValue, TModel>(Dictionary<TValue, TModel> dic, TValue id)
            where TValue : struct
            where TModel : class
        {
            if (!dic.ContainsKey(id)) return null;
            return dic[id];
        }
        private TModel GetModel<TModel>(Dictionary<string, TModel> dic, Dictionary<string, string> dicSetting, string key) where TModel: class
        {
            if (string.IsNullOrEmpty(key) || !dicSetting.ContainsKey(key)) return null;
            var code = dicSetting[key];
            if (string.IsNullOrEmpty(code) || !dic.ContainsKey(code)) return null;
            return dic[code];
        }

        private async Task<List<MatchedReceipt>> GetMatchedReceiptDataAsync(List<DateTime> outputAt = null)
        {
            var option = GetJournalizingOption(outputAt);
            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.GetMatchedReceiptAsync(SessionKey, option));
            if (result.ProcessResult.Result)
                return result.MatchedReceipts;
            return new List<MatchedReceipt>();
        }

        private async Task LoadCollationSettingAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (CollationSettingMasterClient client)
                => await client.GetAsync(SessionKey, CompanyId));
            if (result?.ProcessResult.Result ?? false)
                CollationSetting = result.CollationSetting;
            else
                CollationSetting = new CollationSetting();
        }

        private async Task LoadColumnNameSettingAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterService.ColumnNameSettingMasterClient client)
                => await client.GetItemsAsync(SessionKey, CompanyId));
            if (result.ProcessResult.Result)
                ColumnNames = result.ColumnNames;
            else
                ColumnNames = new List<ColumnNameSetting>();
        }

        #endregion

        #region 出力処理
        /// <summary>承認機能 利用時に制限する機能</summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<MatchingJournalizing> GetFilteredExtractData(List<MatchingJournalizing> source)
            => UseAuthorization ? source.Where(x => x.Approved).ToList() : source;
        private List<GeneralJournalizing> GetFilteredExtractData(List<GeneralJournalizing> source)
            => UseAuthorization ? source.Where(x => x.Approved).ToList() : source;

        private JournalizingOption GetJournalizingOption(List<DateTime> outputAt = null)
            => new JournalizingOption
            {
                CompanyId = CompanyId,
                CurrencyId = CurrencyId,
                RecordedAtFrom = datRecordedAtFrom.Value,
                RecordedAtTo = datRecordedAtTo.Value,
                OutputAt = outputAt ?? new List<DateTime>(),
                ContainAdvanceReceivedOccured = cbxContainAdvanceReceivedOccured.Checked,
                ContainAdvanceReceivedMatching = cbxContainAdvanceReceivedMatching.Checked,
                UseDiscount = UseDiscount,
                IsGeneral = CollationSetting?.JournalizingPattern == 1,
                LoginUserId = Login.UserId,
            };

        /// <summary>｢消込仕訳｣ファイルをエクスポートする。</summary>
        private bool ExportMatchingJournalizing(string path, List<MatchingJournalizing> details)
        {
            var definition = new MatchingJournalizingFileDefinition(new DataExpression(ApplicationControl));
            if (!UseForeignCurrency)
                definition.CurrencyCodeField.Ignored = true;

            definition.AmountField.Format = value => value.ToString(MoneyFormatPlane);
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            NLogHandler.WriteDebug(this, "消込仕訳出力 出力開始");
            var exportDialogResult = ProgressDialog.Start(ParentForm, (cancel, progress) =>
            {
                return exporter.ExportAsync(path, details, cancel, progress);
            }, true, SessionKey);
            NLogHandler.WriteDebug(this, "消込仕訳出力 出力終了");
            if (exportDialogResult == DialogResult.Cancel)
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            if (exporter.Exception != null)
            {
                ShowWarningMessageWithExport(exporter.Exception);
                return false;
            }
            return true;
        }

        /// <summary>汎用仕訳出力 実施</summary>
        private bool ExportGeneralJournalizing(string path, GeneralJournalizingsResult source)
        {
            var details = source.GeneralJournalizings;
            var definition = new GeneralJournalizingFileDefinition(new DataExpression(ApplicationControl), MoneyFormatPlane);
            definition.AccountTitles   = source.AccountTitleDictionary;
            definition.Departments     = source.DepartmentDictionary;
            definition.Sections        = source.SectionDictionary;
            definition.Staffs          = source.StaffDictionary;
            definition.LoginUsers      = source.LoginUserDictionary;
            definition.PaymentAgencies = source.PaymentAgencyDictionary;
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;
            var dialogResult = ProgressDialog.Start(ParentForm, async (cancel, progress)
                => await exporter.ExportAsync(path, details, cancel, progress),
                true, SessionKey);
            if (dialogResult == DialogResult.Cancel)
            {
                DispStatusMessage(MsgInfCancelProcess);
                return false;
            }
            if (exporter.Exception != null)
            {
                ShowWarningMessageWithExport(exporter.Exception);
                return false;
            }
            return true;
        }

        /// <summary>｢消込済み入金データ｣ファイルをエクスポートする。</summary>
        private bool ExportMatchedReceiptData(string path, List<MatchedReceipt> details, List<ExportFieldSetting> settings)
        {
            if (!details.Any()) return false;
            var definition = new MatchedReceiptFileDefinition(
                new DataExpression(ApplicationControl), settings, ColumnNames);
            var decimalFormat = "0" + ((Precision == 0) ? string.Empty : "." + new string('0', Precision));

            if (definition.AmountField != null)
                definition.AmountField.Format = value => value.ToString(decimalFormat);
            if (definition.ReceiptAmountField != null)
                definition.ReceiptAmountField.Format = value => value.ToString(decimalFormat);


            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            var exportDialogResult = ProgressDialog.Start(ParentForm, (cancel, progress) =>
            {
                return exporter.ExportAsync(path, details, cancel, progress);
            }, true, SessionKey);
            if (exportDialogResult == DialogResult.Cancel)
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            if (exporter.Exception != null)
            {
                ShowWarningMessageWithExport(exporter.Exception);
                return false;
            }
            return true;
        }
        private void ShowWarningMessageWithExport(Exception ex)
        {
            if (ex == null) return;
            NLogHandler.WriteErrorLog(this, ex, SessionKey);
            if (ex.HResult == new UnauthorizedAccessException().HResult)
                ShowWarningDialog(MsgErrUnauthorizedAccess);
            else
                ShowWarningDialog(MsgErrSomethingError, "出力");
        }

        #endregion

        #region 印刷処理
        /// <summary></summary>
        /// <param name="source"></param>
        /// <param name="outputAt"></param>
        /// <param name="rePrint">再印刷</param>
        private void DisplayPrintData(List<MatchingJournalizing> source, List<DateTime> outputAt = null, bool rePrint = false)
        {
            MatchingJournalizingReport matchingJournalizingReport = null;

            ProgressDialog.Start(ParentForm, Task.Run(async () =>
            {
                if (rePrint) source = await ExtractMatchingJournalizingAsync(outputAt);

                if (source.Any())
                {
                    var printFileName = (rePrint ? "再出力消込仕訳" : "消込仕訳");
                    matchingJournalizingReport = new MatchingJournalizingReport();
                    matchingJournalizingReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    matchingJournalizingReport.Name = printFileName + $"_{DateTime.Now:yyyyMMdd_HHmmss}";
                    matchingJournalizingReport.SetData(source, Precision, UseForeignCurrency);
                    matchingJournalizingReport.Run(false);
                }

            }), false, SessionKey);

            if (!source.Any())
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }
            if (source.Any())
            {
                var serverPath = WriteFilePath();
                ShowDialogPreview(ParentForm, matchingJournalizingReport, serverPath);
                var messageId = !rePrint ? MsgInfPrintExtractedData : MsgInfReprintedSelectData;
                DispStatusMessage(messageId);
            }
        }

        #endregion

        #region validation

        private bool ValidateAuthorization()
        {
            if (!UseAuthorization) return true;
            var approvedCount = 0;
            var unApprovedCount = 0;
            if (IsStandardPattern)
            {
                approvedCount = ExtractMatching.Count(x => x.Approved);
                unApprovedCount = ExtractMatching.Count - approvedCount;
            }
            else
            {
                approvedCount = ExtractGeneral.Count(x => x.Approved);
                unApprovedCount = ExtractGeneral.Count - approvedCount;
            }
            var anyApproved = approvedCount > 0;
            if (! anyApproved)
            {
                ShowWarningDialog(MsgWngNoData, "承認済みの");
                return false;
            }

            if (!ShowConfirmDialog(MsgQstConfirmPrintForApprovalCount, approvedCount.ToString(), unApprovedCount.ToString()))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            return true;
        }

        private bool ValidateForExport()
        {
            var file = txtFilePath.Text;

            if (string.IsNullOrWhiteSpace(file))
            {
                ShowWarningDialog(MsgWngInputRequired, "書込ファイル");
                return false;
            }

            var specialCharacters = "/:*?|<>\"";
            var index = 0;

            if (file.Length < 3)
            {
                index = file.IndexOfAny(specialCharacters.ToCharArray());
            }
            else
            {
                index = file.Substring(2).IndexOfAny(specialCharacters.ToCharArray());
            }
            if (index != -1)
            {
                ShowWarningDialog(MsgWngInvalidCharacterAtWriteFile);
                return false;
            }

            try
            {
                var directoryName = Path.GetDirectoryName(file);

                if (!Directory.Exists(directoryName))
                {
                    if (Directory.Exists(Path.GetPathRoot(file)) && file.Length == 3)
                    {
                        return true;
                    }
                    ShowWarningDialog(MsgErrNotExistsFolderAndCancelProcess, Path.GetDirectoryName(file));
                    return false;
                }

                return true;
            }
            catch
            {
                ShowWarningDialog(MsgWngInvalidCharacterAtWriteFile);
                return false;
                throw;
            }
        }

        private List<DateTime> GetSelectedOutputAt()
            => grid.Rows
            .Select(x => x.DataBoundItem as JournalizingSummary)
            .Where(x => x.Selected)
            .Select(x => x.OutputAt.Value)
            .ToList();

        #endregion

        #region ラベルBorderColorについて
        private void lblExtractNumber_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblExtractNumber.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblExtractAmount_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblExtractAmount.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblOutputNumber_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblOutputNumber.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblOutputAmount_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblOutputAmount.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }
        #endregion


    }
}

