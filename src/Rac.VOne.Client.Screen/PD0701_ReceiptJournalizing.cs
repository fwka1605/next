using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReceiptService;
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
    /// <summary>入金仕訳出力</summary>
    public partial class PD0701 : VOneScreenBase
    {
        #region Variable Decleration
        private int CurrencyId { get; set; }
        private int precision;
        /// <summary>選択された 通貨の 小数点以下桁数</summary>
        private int Precision { get { return precision; } set {
                precision = value;
                MoneyFormat         = precision == 0 ? "#,##0" : ("#,##0." + new string('0', precision));
                MoneyFormatPlane    = precision == 0 ?     "0" : (    "0." + new string('0', precision));
            }
        }
        private const string CountFormat = "#,##0";
        private string MoneyFormat { get; set; } = "#,##0";
        private string MoneyFormatPlane { get; set; } = "0";
        private string ServerPath { get; set; }
        private string FieldString { get; set; } = "#,###,###,###,##0";
        private List<ReceiptJournalizing> ExtractReceipt { get; set; }
        private List<GeneralJournalizing> ExtractGeneral { get { return ExtractGeneralResult?.GeneralJournalizings; } }
        private GeneralJournalizingsResult ExtractGeneralResult { get; set; }
        private int ExtractedCount { get { return IsStandardPattern
                    ? ExtractReceipt?.Count ?? 0
                    : ExtractGeneral?.Count ?? 0; } }
        private decimal ExtractedAmount { get { return IsStandardPattern
                    ? ExtractReceipt?.Sum(x => x.Amount) ?? 0M
                    : ExtractGeneral?.Sum(x => x.Amount) ?? 0M; } }

        private List<ColumnNameSetting> ColumnSettings { get; set; }
        private string CellName(string value) => $"cel{value}";
        private CollationSetting CollationSetting { get; set; }

        private bool IsStandardPattern { get { return CollationSetting?.JournalizingPattern == 0; } }
        #endregion

        #region Initialization
        public PD0701()
        {
            InitializeComponent();
            grdReceiptOutput.SetupShortcutKeys();
            Text = "入金仕訳出力";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("抽出");
            BaseContext.SetFunction01Enabled(false);
            OnF01ClickHandler = Extract;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = Print;

            BaseContext.SetFunction04Caption("出力");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = Output;

            BaseContext.SetFunction06Caption("再出力");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = ReOutput;

            BaseContext.SetFunction07Caption("再印刷");
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = RePrint;

            BaseContext.SetFunction08Caption("取消");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = Cancel;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        private void PD0701_Load(object sender, EventArgs e)
        {
            SetScreenName();
            try
            {
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);

                lblCurrency.Visible = UseForeignCurrency;
                txtCurrencyCode.Visible = UseForeignCurrency;
                btnCurrency.Visible = UseForeignCurrency;
                txtOutputPath.Enabled = !LimitAccessFolder;

                SetOutputPath();
                InitializeGridTemplate();
                ClearData();

                if (!UseForeignCurrency)
                    ProgressDialog.Start(ParentForm, LoadCurrencyAndHistoryAsync(DefaultCurrencyCode), false, SessionKey);
                txtOutputPath.Select();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task>();

            if (ApplicationControl == null) tasks.Add(LoadApplicationControlAsync());
            if (Company == null) tasks.Add(LoadCompanyAsync());
            tasks.Add(LoadControlColorAsync());
            tasks.Add(LoadGridSettingAsync());
            tasks.Add(LoadCollationSettingAsync());
            await Task.WhenAll(tasks);
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,                             50, nameof(JournalizingSummary.Selected)    , dataField: nameof(JournalizingSummary.Selected)    , caption: "選択"  , readOnly: false, cell: builder.GetCheckBoxCell(isBoolType: true)),
                new CellSetting(height,                            135, nameof(JournalizingSummary.OutputAt)    , dataField: nameof(JournalizingSummary.OutputAt)    , caption: "仕訳日", cell: builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height,                            100, nameof(JournalizingSummary.Count)       , dataField: nameof(JournalizingSummary.Count)       , caption: "件数"  , cell:  builder.GetNumberCell()),
                new CellSetting(height, UseForeignCurrency ?  80 :   0, nameof(JournalizingSummary.CurrencyCode), dataField: nameof(JournalizingSummary.CurrencyCode), caption: "通貨"  , cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, UseForeignCurrency ? 100 : 180, nameof(JournalizingSummary.Amount)      , dataField: nameof(JournalizingSummary.Amount)      , caption: "金額"  , cell:  builder.GetTextBoxCurrencyCell(Precision)),
            });

            grdReceiptOutput.Template = builder.Build();
            grdReceiptOutput.HideSelection = true;
            grdReceiptOutput.AllowUserToResize = false;
        }

        #endregion

        #region  Function Keys処理
        [OperationLog("抽出")]
        private void Extract()
        {
            ClearStatusMessage();
            try
            {
                if (!datRecordAtFrom.ValidateRange(datRecordAtTo,
                    () => ShowWarningDialog(MsgWngInputRangeChecked, "入金日")))
                    return;

                var extractTask = ExtractJournalizingAsync();

                ProgressDialog.Start(ParentForm, extractTask, false, SessionKey);

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
        private void Clear()
        {
            ClearData();
        }

        #region 印刷
        [OperationLog("印刷")]
        private void Print()
        {
            ClearStatusMessage();
            try
            {
                WritePrintFile(false, null);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private void WritePrintFile(bool isRePrint, List<ReceiptJournalizing> rePrintList)
        {
            List<ReceiptJournalizing> printList = null;
            string pdfFileName = string.Empty;

            if (!isRePrint)
            {
                printList = ExtractReceipt;
                pdfFileName = "入金仕訳" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }
            else
            {
                printList = rePrintList;
                pdfFileName = "再出力入金仕訳" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }

            if (printList != null && printList.Any())
            {
                var receiptReport = new ReceiptJournalizingSectionReport();

                receiptReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                receiptReport.Name = pdfFileName;
                receiptReport.SetData(printList, Precision, UseForeignCurrency, ColumnSettings);

                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    receiptReport.Run(false);
                }), false, SessionKey);

                if (ServerPath == null)
                    SetOutputPath();

                if (!Directory.Exists(ServerPath))
                    ServerPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                var result = ShowDialogPreview(ParentForm, receiptReport, ServerPath);

                if (result == DialogResult.Cancel)
                {
                    DispStatusMessage(isRePrint ? MsgInfReprintedSelectData : MsgInfPrintExtractedData);
                }
            }
        }
        #endregion

        #region 出力
        [OperationLog("出力")]
        private void Output()
        {
            if (!ValidateInputValuesForOutput()) return;

            var task = OutputJournalizingInner();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Exception != null)
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
        }

        private bool ValidateInputValuesForOutput()
        {
            ClearStatusMessage();

            var file = txtOutputPath.Text;

            if (string.IsNullOrWhiteSpace(file))
            {
                ShowWarningDialog(MsgWngInputRequired, lblWriteFile.Text);
                txtOutputPath.Focus();
                return false;
            }

            var specialCharacters = "/:*?|<>\"";
            var index = 0;

            if (txtOutputPath.Text.Length > 2)
                index = file.Substring(2).IndexOfAny(specialCharacters.ToCharArray());
            else
                index = file.IndexOfAny(specialCharacters.ToCharArray());

            try
            {
                if (index == -1)
                {
                    var filePath = Path.GetDirectoryName(file);
                    if (!Directory.Exists(filePath) && !(Directory.Exists(Path.GetPathRoot(file)) && file.Length == 3))
                    {
                        ShowWarningDialog(MsgErrNotExistsFolderAndCancelProcess, filePath);
                        txtOutputPath.Focus();
                        return false;
                    }
                }
                else
                {
                    ShowWarningDialog(MsgWngInvalidCharacterAtWriteFile);
                    txtOutputPath.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgWngInvalidCharacterAtWriteFile);
                txtOutputPath.Focus();
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }

            return true;
        }

        #endregion

        [OperationLog("再出力")]
        private void ReOutput()
        {
            ClearStatusMessage();
            try
            {
                var filePath = string.Empty;
                var fileName = $"再出力入金仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                if (!ShowSaveFileDialog(GetDirectory(), fileName, out filePath,
                    cancellationMessaging: () => DispStatusMessage(MsgInfProcessCanceled))) return;


                var outputAt = GetCheckedOutputAt();

                if (!ReoutputJournalizingInner(outputAt, filePath)) return;

                DispStatusMessage(MsgInfFinishedSuccessReOutputProcess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("再印刷")]
        private void RePrint()
        {
            ClearStatusMessage();

            try
            {
                var outputAt = GetCheckedOutputAt();
                var task = ExtractReceiptJournalizingAsync(outputAt);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                    ShowWarningDialog(MsgErrSomethingError, "再印刷");
                    return;
                }

                WritePrintFile(true, task.Result);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("取消")]
        private void Cancel()
        {
            ClearStatusMessage();
            if (!ShowConfirmDialog(MsgQstConfirmCancelJournalizing))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {

                var outputAt = GetCheckedOutputAt();
                var task = CancelOutputAtAsync(outputAt);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrSomethingError, "取消");
                    return;
                }

                ClearData();
                if (!UseForeignCurrency)
                    ProgressDialog.Start(ParentForm, LoadCurrencyAndHistoryAsync(DefaultCurrencyCode), false, SessionKey);
                DispStatusMessage(MsgInfFinishedSuccessJournalizingCancelingProcess);

            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "取消");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void Exit()
        {
            BaseForm.Close();
        }
        #endregion

        #region extract

        private async Task<bool> ExtractJournalizingAsync()
        {
            var enable = false;
            if (IsStandardPattern)
            {
                ExtractReceipt = await ExtractReceiptJournalizingAsync();
                enable = ExtractReceipt.Any();
            }
            else
            {
                ExtractGeneralResult = await ExtractGeneralJournalizingAsync();
                enable = ExtractGeneral.Any();
            }
            datRecordAtFrom.Enabled = !enable;
            datRecordAtTo.Enabled   = !enable;
            lblDispOutputNo.Clear();
            lblDispOutputAmt.Clear();
            SetFunctionKey34(enable);
            lblDispExtractNo.Text = ExtractedCount.ToString(CountFormat);
            lblDispExtractAmt.Text = ExtractedAmount.ToString(MoneyFormat);
            return enable;
        }

        #endregion

        #region output journalizing

        private async Task<bool> OutputJournalizingInner()
        {
            var summary = await GetReceiptJournalizingSummaryAsync(isOutputted: false);
            var path = txtOutputPath.Text;
            var count = 0;
            var amount = 0M;
            if (IsStandardPattern)
            {
                var source = ExtractReceipt; // filter approved 
                count = source.Count;
                amount = source.Sum(x => x.Amount);
                if (summary.Sum(x => x.Amount) != amount)
                {
                    ShowWarningDialog(MsgWngNotEqualAbstractAmountAndUpdateAmount);
                    return false;
                }
                if (!ExportReceiptJournalizing(path, source)) return false;
            }
            else
            {
                var extracted = ExtractGeneralResult;
                var source = extracted.GeneralJournalizings;
                count = source.Count;
                amount = source.Sum(x => x.Amount);
                if (summary.Sum(x => x.Amount) != amount)
                {
                    ShowWarningDialog(MsgWngNotEqualAbstractAmountAndUpdateAmount);
                    return false;
                }
                if (!ExportGeneralJournalizing(path, extracted)) return false;
            }

            var updateResult = await UpdateOutputAtAsync();
            if (!updateResult)
            {
                ShowWarningDialog(MsgErrExportError);
                return false;
            }
            var summaries = await GetReceiptJournalizingSummaryAsync();
            if (summaries != null)
                SetGridData(summaries);
            lblDispOutputNo.Text = count.ToString(CountFormat);
            lblDispOutputAmt.Text = amount.ToString(MoneyFormat);

            DispStatusMessage(MsgInfFinishDataExtracting);
            BaseContext.SetFunction04Enabled(false);
            return true;
        }
        private bool ExportReceiptJournalizing(string path, List<ReceiptJournalizing> source)
        {
            var definition = new ReceiptJournalizingFileDefinition(new DataExpression(ApplicationControl));
            if (definition.CurrencyCodeField.Ignored = (!UseForeignCurrency))
                definition.CurrencyCodeField.FieldName = null;
            definition.AmountField.Format = value => value.ToString(MoneyFormatPlane);
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            var dialogResult = ProgressDialog.Start(ParentForm, (cancel, progress)
                => exporter.ExportAsync(path, source, cancel, progress), true, SessionKey);
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

        private bool ExportGeneralJournalizing(string path, GeneralJournalizingsResult source)
        {
            var details = source.GeneralJournalizings;
            var definition = new GeneralJournalizingFileDefinition(new DataExpression(ApplicationControl), MoneyFormatPlane);
            definition.AccountTitles = source.AccountTitleDictionary;
            definition.Departments = source.DepartmentDictionary;
            definition.Sections = source.SectionDictionary;
            definition.Staffs = source.StaffDictionary;
            definition.LoginUsers = source.LoginUserDictionary;
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

        #region re-output journalizing

        private bool ReoutputJournalizingInner(List<DateTime> outputAt, string path)
        {
            var tasks = new List<Task>();
            Task<List<ReceiptJournalizing>> loadReceipt = null;
            Task<GeneralJournalizingsResult> loadGeneral = null;
            if (IsStandardPattern)
                tasks.Add(loadReceipt = ExtractReceiptJournalizingAsync(outputAt));
            else
                tasks.Add(loadGeneral = ExtractGeneralJournalizingAsync(outputAt));

            ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
            if (tasks.Any(x => x.Exception != null))
            {
                foreach (var ex in tasks.Where(x => x.Exception != null).Select(x => x.Exception))
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrExportError);
                return false;
            }
            if ( IsStandardPattern && !ExportReceiptJournalizing(path, loadReceipt.Result)) return false;
            if (!IsStandardPattern && !ExportGeneralJournalizing(path, loadGeneral.Result)) return false;
            return true;
        }

        #endregion

        #region グリッドの処理
        private void grdReceiptOutput_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            ChangedCellEvent(e.CellName);
        }

        private void grdReceiptOutput_CellValueChanged(object sender, CellEventArgs e)
        {
            ChangedCellEvent(e.CellName);
        }

        private void ChangedCellEvent(string cellName)
        {
            grdReceiptOutput.EndEdit();

            if (cellName != CellName(nameof(JournalizingSummary.Selected))) return;

            var outputAt = GetCheckedOutputAt();
            var anyChecked = outputAt.Any();
            SetFunctionKey678(anyChecked);
        }
        #endregion

        #region ボタンクリックイベント
        private void btnCurrency_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency == null) return;

            txtCurrencyCode.Text = currency.Code;
            CurrencyId = currency.Id;
            Precision = currency.Precision;

            ProgressDialog.Start(ParentForm, LoadCurrencyAndHistoryAsync(txtCurrencyCode.Text), false, SessionKey);
        }

        private void btnWriteFile_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var filePath = string.Empty;
            var fileName = $"入金仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            if (!ShowSaveFileDialog(GetDirectory(), fileName, out filePath)) return;
            txtOutputPath.Text = filePath;
        }

        private string GetDirectory()
        {
            var filePath = txtOutputPath.Text;
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

        #region サブ処理

        private void ClearData()
        {
            ClearStatusMessage();

            //DateField
            datRecordAtFrom.Clear();
            datRecordAtTo.Clear();

            // レベール
            lblDispExtractNo.Clear();
            lblDispExtractAmt.Clear();
            lblDispOutputNo.Clear();
            lblDispOutputAmt.Clear();

            // 通貨コード
            if (UseForeignCurrency)
            {
                CurrencyId = 0;
                txtCurrencyCode.Clear();
                txtCurrencyCode.Enabled = true;
                btnCurrency.Enabled = true;

                //グリッドをクリア
                grdReceiptOutput.DataSource = null;
                BaseContext.SetFunction01Enabled(false);
                SetFunctionKey678(false);
            }
            else
            {
                BaseContext.SetFunction01Enabled(true);
            }

            datRecordAtFrom.Enabled = true;
            datRecordAtTo.Enabled = true;

            SetFunctionKey34(false);

            this.ActiveControl = txtOutputPath;
            txtOutputPath.Select();
        }

        private void SetOutputPath()
        {
            ClearStatusMessage();
            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var path = task.Result;
            if (!string.IsNullOrEmpty(path))
                ServerPath = path;
            var fileName = $"入金仕訳_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            if (!string.IsNullOrEmpty(ServerPath))
                txtOutputPath.Text = Path.Combine(ServerPath, fileName);
            else
                ServerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
        }

        private void SetGridData(List<JournalizingSummary> dataSource)
        {
            InitializeGridTemplate();
            grdReceiptOutput.DataSource = new BindingSource(dataSource, null);

            BaseContext.SetFunction01Enabled(true);

            if (UseForeignCurrency)
            {
                txtCurrencyCode.Enabled = false;
                btnCurrency.Enabled = false;
            }
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);

            this.ActiveControl = grdReceiptOutput;
        }

        private void SetFunctionKey34(bool enabled)
        {
            BaseContext.SetFunction03Enabled(enabled && IsStandardPattern);
            BaseContext.SetFunction04Enabled(enabled);
        }
        private void SetFunctionKey678(bool enabled)
        {
            BaseContext.SetFunction06Enabled(enabled);
            BaseContext.SetFunction07Enabled(enabled && IsStandardPattern);
            BaseContext.SetFunction08Enabled(enabled);
        }


        private void SetCurrencyDisplayString(int displayScale)
        {
            var displayFieldString = "#,###,###,###,##0";
            var displayFormatString = "0";

            if (displayScale > 0)
            {
                displayFieldString += ".";
                for (var i = 0; i < displayScale; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }

            FieldString = displayFieldString;
        }
        #endregion

        #region Validated イベント
        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();

            if (string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                txtCurrencyCode.Clear();
                CurrencyId = 0;
                Precision = 0;
                return;
            }
            ProgressDialog.Start(ParentForm, LoadCurrencyAndHistoryAsync(txtCurrencyCode.Text), false, SessionKey);

        }
        #endregion

        #region ラベルBorderColor
        private void lblDispExtractNo_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblDispExtractNo.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblDispExtractAmt_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblDispExtractAmt.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblDispOutputNo_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblDispOutputNo.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void lblDispOutputAmt_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, lblDispOutputAmt.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }
        #endregion

        #region call web service
        private async Task LoadCurrencyAndHistoryAsync(string code)
        {
            var currency = await GetCurrencyAsync(code);
            if (currency == null) return;
            CurrencyId = currency.Id;
            Precision = currency.Precision;
            var summaries = await GetReceiptJournalizingSummaryAsync();
            SetGridData(summaries);
        }

        private async Task<Currency> GetCurrencyAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result) return result.Currencies.FirstOrDefault();
                return null;
            });
        private async Task LoadGridSettingAsync()
        {
            await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(Login.SessionKey, Login.CompanyId);
                if (result.ProcessResult.Result)
                    ColumnSettings = result.ColumnNames
                        .Where(x => x.TableName == nameof(Receipt)).ToList();
            });
        }


        private async Task<List<JournalizingSummary>> GetReceiptJournalizingSummaryAsync(bool isOutputted = true)
        {
            var option = GetJournalizingOption();
            option.IsOutputted = isOutputted;
            if (isOutputted)
            {
                option.RecordedAtFrom = null;
                option.RecordedAtTo = null;
            }
            var result = await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client)
                => await client.GetReceiptJournalizingSummaryAsync(SessionKey, option));
            if (result.ProcessResult.Result)
                return result.JournalizingsSummaries;
            return null;
        }

        private async Task<List<ReceiptJournalizing>> ExtractReceiptJournalizingAsync(List<DateTime> outputAt = null)
        {
            var option = GetJournalizingOption(outputAt);
            var result = await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client)
                => await client.ExtractReceiptJournalizingAsync(SessionKey, option));
            if (result.ProcessResult.Result)
                return result.ReceiptJournalizings;
            return new List<ReceiptJournalizing>();
        }
        private async Task<GeneralJournalizingsResult> ExtractGeneralJournalizingAsync(List<DateTime> outputAt = null)
        {
            var option = GetJournalizingOption(outputAt);
            var result = await ServiceProxyFactory.Do(async (ReceiptServiceClient client)
                => await client.ExtractReceiptGeneralJournalizingAsync(SessionKey, option));
            if (result == null) return new GeneralJournalizingsResult();
            if (!result.ProcessResult.Result) return result;
            var details = result.GeneralJournalizings;
            await LoadTransactionModelsAndCustomersAsync(details);
            await LoadMasterAndHeadersAsync(result);
            return result;
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
                    var client = factory.Create<MatchingService.MatchingServiceClient>();
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
                    var client = factory.Create<MatchingService.MatchingServiceClient>();
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
        private TModel GetModel<TModel>(Dictionary<string, TModel> dic, Dictionary<string, string> dicSetting, string key) where TModel : class
        {
            if (string.IsNullOrEmpty(key) || !dicSetting.ContainsKey(key)) return null;
            var code = dicSetting[key];
            if (string.IsNullOrEmpty(code) || !dic.ContainsKey(code)) return null;
            return dic[code];
        }

        private async Task<bool> UpdateOutputAtAsync()
        {
            var option = GetJournalizingOption();
            var result = await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client)
                => await client.UpdateOutputAtAsync(SessionKey, option));
            return result?.ProcessResult.Result ?? false;
        }

        private async Task<bool> CancelOutputAtAsync(List<DateTime> outputAt)
        {
            var option = GetJournalizingOption(outputAt);
            var result = await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client)
                => await client.CancelReceiptJournalizingAsync(SessionKey, option));
            return result?.ProcessResult.Result ?? false;
        }

        private async Task LoadCollationSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (CollationSettingMasterService.CollationSettingMasterClient client) =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    CollationSetting = result.CollationSetting;
                else
                    CollationSetting = new CollationSetting();
            });

        #region checked outputAt list

        private List<DateTime> GetCheckedOutputAt() => grdReceiptOutput.Rows
            .Select(x => x.DataBoundItem as JournalizingSummary)
            .Where(x => x.Selected)
            .Select(x => x.OutputAt.Value)
            .ToList();

        #endregion

        #region journalizing option helper
        private JournalizingOption GetJournalizingOption(List<DateTime> outputAt = null)
            => new JournalizingOption
            {
                CompanyId = CompanyId,
                CurrencyId = CurrencyId,
                RecordedAtFrom = datRecordAtFrom.Value,
                RecordedAtTo = datRecordAtTo.Value,
                OutputAt = outputAt ?? new List<DateTime>(),
                UseDiscount = UseDiscount,
                IsGeneral = CollationSetting?.JournalizingPattern == 1,
                LoginUserId = Login.UserId,
            };
        #endregion

        #endregion
    }
}
