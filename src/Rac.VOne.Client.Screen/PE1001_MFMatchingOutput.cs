using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
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
    /// <summary> MFクラウド会計 消込結果連携 </summary>
    public partial class PE1001 : VOneScreenBase
    {
        private int CurrencyId { get; set; }
        private int precision { get; set; }
        private int Precision
        {
            get { return precision; }
            set
            {
                precision = value;
                MoneyFormat = precision == 0 ? "#,##0" : ("#,##0." + new string('0', precision));
            }
        }
        private const string CountFormat = "#,##0";
        private string MoneyFormat { get; set; } = "#,##0";
        private string MoneyFormatPlane { get; set; } = "0";
        private List<MatchingJournalizing> ExtractMatching { get; set; }
        private int ExtractedCount => ExtractMatching?.Count ?? 0;
        private decimal ExtractedAmount => ExtractMatching?.Sum(x => x.Amount) ?? 0M;
        private string CellName(string value) => $"cel{value}";

        #region Initialize
        public PE1001()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            Text = "MFクラウド会計 消込結果連携";
            grid.SetupShortcutKeys();
        }

        private void InitializeHandlers()
        {
            Load += PE1001_Load;
            lblExtractNumber.Paint += LabelOnPaint;
            lblExtractAmount.Paint += LabelOnPaint;
            lblOutputNumber.Paint += LabelOnPaint;
            lblOutputAmount.Paint += LabelOnPaint;
            grid.CellEditedFormattedValueChanged += grid_CellEditedFormattedValueChanged;
            grid.CellValueChanged += grid_CellValueChanged;
            btnFilePath.Click += btnFilePath_Click;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("抽出");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction04Caption("出力");
            BaseContext.SetFunction06Caption("再出力");
            //BaseContext.SetFunction07Caption("再印刷");
            BaseContext.SetFunction08Caption("取消");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Extract;
            OnF02ClickHandler = Clear;
            //OnF03ClickHandler = Print;
            OnF04ClickHandler = Output;
            OnF06ClickHandler = ReOutput;
            //OnF07ClickHandler = RePrint;
            OnF08ClickHandler = Cancel;
            OnF10ClickHandler = Close;
        }

        private void PE1001_Load(object sender, EventArgs e)
        {
            SetScreenName();
            Settings.SetCheckBoxValue<PE1001>(Login, cbxSubAccountTitle);
            ProgressDialog.Start(ParentForm, InitializeLoadAsync(), false, SessionKey);
        }

        private async Task InitializeLoadAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
            };
            await Task.WhenAll(tasks);

            Clear();

            var currency = await GetCurrencyAsync(DefaultCurrencyCode);
            if (currency != null)
            {
                CurrencyId = currency.Id;
                Precision = currency.Precision;
            }
            InitializeGridTemplate();
            InitializeControlsValue();
            await SetHistoryDataAsync();
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[] {
                new CellSetting(height, 50 , nameof(JournalizingSummary.Selected)    , dataField: nameof(JournalizingSummary.Selected)    , caption: "選択"  , cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false),
                new CellSetting(height, 135, nameof(JournalizingSummary.OutputAt)    , dataField: nameof(JournalizingSummary.OutputAt)    , caption: "仕訳日", cell: builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height, 100, nameof(JournalizingSummary.Count)       , dataField: nameof(JournalizingSummary.Count)       , caption: "件数"  , cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height, 180, nameof(JournalizingSummary.Amount)      , dataField: nameof(JournalizingSummary.Amount)      , caption: "金額"  , cell: builder.GetTextBoxCurrencyCell(Precision)),
            });
            grid.Template = builder.Build();
            grid.HideSelection = true;
        }

        private void InitializeControlsValue()
        {
            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            var path = task.Result;
            if (!string.IsNullOrWhiteSpace(path))
            {
                var fileName = $"消込仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                txtFilePath.Text = Path.Combine(path, fileName);
            }
            txtFilePath.Select();
            txtFilePath.Enabled = !LimitAccessFolder;
        }

        private async Task SetHistoryDataAsync()
        {
            var history = await GetMatchingJournalizingSummaryAsync();
            grid.DataSource = new BindingSource(history, null);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction08Enabled(false);
        }

        #endregion

        #region function keys

        [OperationLog("抽出")]
        private void Extract()
        {
            ClearStatusMessage();
            if (!ValidateInputValues()) return;
            try
            {
                var loadTask = LoadExtractedDataAsync();
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                if (loadTask.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, loadTask.Exception, SessionKey);
                    ShowWarningDialog(MsgErrSomethingError, "抽出");
                    return;
                }
                var result = loadTask.Result;
                if (!result)
                {
                    ShowWarningDialog(MsgWngNotExistSearchData);
                    return;
                }

                BaseContext.SetFunction04Enabled(true);
                DispStatusMessage(MsgInfDataExtracted);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            datRecordedAtFrom.Clear();
            datRecordedAtTo.Clear();
            datRecordedAtFrom.Enabled = true;
            datRecordedAtTo.Enabled = true;
            lblExtractNumber.Clear();
            lblExtractAmount.Clear();
            lblOutputNumber.Clear();
            lblOutputAmount.Clear();

            BaseContext.SetFunction04Enabled(false);
            ClearStatusMessage();
        }

        [OperationLog("印刷")]
        private void Print()
        {
            ClearStatusMessage();
            if (!ValidateAuthorization()) return;

            var source = GetFilteredExtractMatching(ExtractMatching);
            PrintReport(source);
        }

        [OperationLog("出力")]
        private void Output()
        {
            ClearStatusMessage();
            if (!ValidateForExport()
             || !ValidateAuthorization()) return;

            var task = OutputJournalizingAsync();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return;
            }
            if (task.Result)
                DispStatusMessage(MsgInfFinishDataExtracting);
        }

        [OperationLog("再出力")]
        private void ReOutput()
        {
            ClearStatusMessage();
            var filePath = string.Empty;

            if (!ShowSaveFileDialog(GetDirectory(),
                $"再出力消込仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                out filePath,
                cancellationMessaging: () => DispStatusMessage(MsgInfProcessCanceled))) return;

            var task = ReOutputJournalizingAsync(filePath);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return;
            }

            if (task.Result)
                DispStatusMessage(MsgInfFinishedSuccessReOutputProcess);

        }

        [OperationLog("再印刷")]
        private void RePrint()
        {
            ClearStatusMessage();
            PrintReport(null, GetSelectedOutputAts());
        }

        [OperationLog("取消")]
        private void Cancel()
        {
            ClearStatusMessage();
            if (!ShowConfirmDialog(MsgQstConfirmCancelJournalizing)) return;
            var task = CancelOutputAtAsync(GetSelectedOutputAts());
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "取消");
                return;
            }

            ProgressDialog.Start(ParentForm, SetHistoryDataAsync(), false, SessionKey);
        }

        [OperationLog("終了")]
        private void Close()
        {
            Settings.SaveControlValue<PE1001>(Login, cbxSubAccountTitle.Name, cbxSubAccountTitle.Checked);
            ParentForm.Close();
        }
        #endregion

        #region extract
        private bool ValidateInputValues()
        {
            if (!datRecordedAtFrom.ValidateRange(datRecordedAtTo, () => ShowWarningDialog(MsgWngInputRangeChecked, lblRecordedAt.Text))) return false;
            return true;
        }

        private async Task<bool> LoadExtractedDataAsync()
        {
            ExtractMatching = await ExtractAsync();
            var enable = ExtractMatching.Any();
            datRecordedAtFrom.Enabled = !enable;
            datRecordedAtTo.Enabled = !enable;

            lblOutputNumber.Clear();
            lblOutputAmount.Clear();

            lblExtractNumber.Text = ExtractedCount.ToString("#,##0");
            lblExtractAmount.Text = ExtractedAmount.ToString("#,##0");
            return enable;
        }
        #endregion

        #region validate authorization
        private bool ValidateAuthorization()
        {
            if (!UseAuthorization) return true;
            var approvedCount = ExtractMatching.Count(x => x.Approved);
            var unapporvedCount = ExtractMatching.Count - approvedCount;
            if (approvedCount == 0)
            {
                ShowWarningDialog(MsgWngNoData, "承認済みの");
                return false;
            }

            if (!ShowConfirmDialog(MsgQstConfirmPrintForApprovalCount, approvedCount.ToString(), unapporvedCount.ToString()))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            return true;
        }
        #endregion

        #region validate for export
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
                index = file.IndexOfAny(specialCharacters.ToCharArray());
            else
                index = file.Substring(2).IndexOfAny(specialCharacters.ToCharArray());

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
                        return true;

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
        #endregion

        #region get folder
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
        #endregion

        #region filtered
        private List<MatchingJournalizing> GetFilteredExtractMatching(List<MatchingJournalizing> source)
        {
            if (!UseAuthorization) return source;
            return source.Where(x => x.Approved).ToList();
        }

        #endregion

        #region journalizing option

        private JournalizingOption GetJournalizingOption(List<DateTime> outputAt = null) => new JournalizingOption
        {
            CompanyId = CompanyId,
            CurrencyId = CurrencyId,
            RecordedAtFrom = datRecordedAtFrom.Value,
            RecordedAtTo = datRecordedAtTo.Value,
            OutputAt = outputAt ?? new List<DateTime>(),
            UseDiscount = UseDiscount,
            LoginUserId = Login.UserId,
            OutputCustoemrName = cbxSubAccountTitle.Checked,
        };

        #endregion

        #region print report

        private void PrintReport(List<MatchingJournalizing> source, List<DateTime> outputAt = null)
        {
            var rePrint = (outputAt?.Any() ?? false);
            MatchingJournalizingReport report = null;
            var path = string.Empty;
            var task = Task.Run(async () => {
                if (rePrint) source = await ExtractAsync(outputAt);
                if (source.Any())
                {
                    var fileName = $"{(rePrint ? "再出力" : "")}消込仕訳_{DateTime.Now:yyyyMMdd_HHmmss}";
                    report = new MatchingJournalizingReport();
                    report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    report.Name = fileName;
                    report.SetData(source, Precision, UseForeignCurrency);
                    report.Run(false);
                }
                path = await Util.GetGeneralSettingServerPathAsync(Login);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
                return;
            }
            if (!source.Any())
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }

            ShowDialogPreview(ParentForm, report, path);
            DispStatusMessage(rePrint ? MsgInfReprintedSelectData
                                      : MsgInfPrintExtractedData);
        }

        #endregion

        #region output journalizing
        private async Task<bool> OutputJournalizingAsync()
        {
            var summary = await GetMatchingJournalizingSummaryAsync(isOutputted: false);
            var source = GetFilteredExtractMatching(ExtractMatching);
            var count = source.Count;
            var amount = source.Sum(x => x.Amount);
            if (!(count == summary.Sum(x => x.Count) && amount == summary.Sum(x => x.Amount)))
            {
                DoActionOnUI(() => ShowWarningDialog(MsgWngNotEqualAbstractAmountAndUpdateAmount));
                return false;
            }
            var filePath = txtFilePath.Text;
            if (!ExportMatchingJournalizing(filePath, source)) return false;

            var updateResult = await UpdateOutpuAtAsync();
            if (!updateResult)
            {
                DoActionOnUI(() => ShowWarningDialog(MsgErrSaveError));
                return false;
            }

            await SetHistoryDataAsync();

            lblOutputNumber.Text = count.ToString("#,##0");
            lblOutputAmount.Text = amount.ToString("#,##0");

            BaseContext.SetFunction04Enabled(false);
            return true;
        }
        #endregion

        #region reoutpt Journalizing
        private async Task<bool> ReOutputJournalizingAsync(string filePath)
        {
            var source = await ExtractAsync(GetSelectedOutputAts());

            if (!ExportMatchingJournalizing(filePath, source)) return false;

            return true;
        }
        #endregion

        #region ExportProcess
        /// <summary>｢消込仕訳｣ファイルをエクスポートする。</summary>
        private bool ExportMatchingJournalizing(string path, List<MatchingJournalizing> details)
        {
            var definition = new MFMatchingJournalizingFileDefinition(new DataExpression(ApplicationControl));

            definition.DebitAmountField.Format = value => value.ToString(MoneyFormatPlane);
            definition.CreditAmountField.Format = value => value.ToString(MoneyFormatPlane);
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

        #region grid helper
        private List<DateTime> GetSelectedOutputAts()
            => grid.Rows
            .Select(x => x.DataBoundItem as JournalizingSummary)
            .Where(x => x.Selected && x.OutputAt.HasValue)
            .Select(x => x.OutputAt.Value).ToList();

        #endregion

        #region call web services
        private async Task<List<MatchingJournalizing>> ExtractAsync(List<DateTime> outputAt = null)
        {
            var option = GetJournalizingOption(outputAt);
            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.MFExtractMatchingJournalizingAsync(SessionKey, option));
            if (result.ProcessResult.Result)
                return result.MatchingJournalizings;
            return new List<MatchingJournalizing>();
        }

        private async Task<Currency> GetCurrencyAsync(string code)
            => (await ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client)
                => await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code })))?.Currencies.FirstOrDefault();


        private async Task<List<JournalizingSummary>> GetMatchingJournalizingSummaryAsync(
            bool isOutputted = true)
            => await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client) =>
            {
                var option = GetJournalizingOption();
                option.IsOutputted = isOutputted;
                if (isOutputted)
                {
                    option.RecordedAtFrom = null;
                    option.RecordedAtTo = null;
                }
                var webResult = await client.GetMatchingJournalizingSummaryAsync(SessionKey, option);
                if (webResult.ProcessResult.Result)
                    return webResult.JournalizingsSummaries;
                return new List<JournalizingSummary>();
            });


        private async Task<bool> UpdateOutpuAtAsync()
        {
            var option = GetJournalizingOption();
            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.UpdateOutputAtAsync(SessionKey, option));
            return result?.ProcessResult.Result ?? false;
        }

        private async Task<bool> CancelOutputAtAsync(List<DateTime> outputAt)
        {
            var option = GetJournalizingOption(outputAt);
            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.CancelMatchingJournalizingAsync(SessionKey, option));
            return result?.ProcessResult.Result ?? false;
        }

        #endregion

        #region event handlers

        private void grid_CellEditedFormattedValueChanged(object sender, GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellName != CellName(nameof(JournalizingSummary.Selected))) return;
            grid.EndEdit();
        }

        private void grid_CellValueChanged(object sender, GrapeCity.Win.MultiRow.CellEventArgs e)
        {
            if (e.CellName != CellName(nameof(JournalizingSummary.Selected))) return;
            var enable = grid.Rows.Any(x => (x.DataBoundItem as JournalizingSummary).Selected);
            BaseContext.SetFunction06Enabled(enable);
            BaseContext.SetFunction08Enabled(enable);
        }

        private void LabelOnPaint(object sender, PaintEventArgs e)
            => ControlPaint.DrawBorder(e.Graphics, ((Control)sender).DisplayRectangle, Color.Wheat, ButtonBorderStyle.Solid);

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var filePath = string.Empty;
            var fileName = $"消込仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            if (!ShowSaveFileDialog(GetDirectory(), fileName, out filePath)) return;
            txtFilePath.Text = filePath;
        }
        #endregion

    }
}
