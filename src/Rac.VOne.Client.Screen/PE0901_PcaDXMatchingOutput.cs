using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Common;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.HttpClients;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.CollationSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.MatchingService;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.PcaModels;
using Rac.VOne.Client.Reports;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;


namespace Rac.VOne.Client.Screen
{
    /// <summary>PCA会計DX 消込結果連携
    /// </summary>
    /// <remarks>
    /// 通常の消込仕訳をそのまま出力する
    /// 集計などが必要な場合は消込関連のWebサービスを修正する必要がある
    /// 基本的には、部門、科目、得意先のコードを PCAのマスターと同一にする必要がある
    /// 連携については、PCA 会社基本情報 (Comp) を参照して、部門を設定する
    /// 科目補助（得意先）が必要なものは PCA 科目 (Kmk) を参照して、補助科目を設定する
    /// 連携処理は 10件ずつ トランザクションがかかるが、10件以上にトランザクションが掛からないため、
    /// 連携前に、科目補助のマスター整備が行われているかどうかを確認した上で連携するようになっている
    /// 承認処理 などのオプションが地味に面倒
    /// </remarks>
    public partial class PE0901 : VOneScreenBase
    {
        private int CurrencyId { get; set; }
        private int precision;
        private int Precision
        {
            get { return precision; }
            set {
                precision = value;
                MoneyFormat = precision == 0 ? "#,##0" : ("#,##0." + new string('0', precision));
            }
        }
        private const string CountFormat = "#,##0";
        private string MoneyFormat { get; set; } = "#,##0";
        private string MoenyFormatPlane { get; set; }

        private List<string> Errors { get; set; } = new List<string>();

        private List<MatchingJournalizing> ExtractMatching { get; set; }
        private List<BEInputSlip> InputSlips { get; set; }
        private BEComp BEComp { get; set; }
        private Dictionary<string, BEKmk> PcaAccountTitleDictionary { get; set; }
        private SortedList<string, BEHojo[]> PcaCustomerDictionary { get; set; }
        private Dictionary<string, BEBu> PcaDepartmentDictionary { get; set; }
        private int ExtractedCount => ExtractMatching?.Count ?? 0;
        private decimal ExtractedAmount => ExtractMatching?.Sum(x => x.Amount) ?? 0M;

        private string CellName(string value) => $"cel{value}";

        /// <summary>PCA DX Web API client</summary>
        private readonly PcaWebApiClient client;

        #region initialize

        public PE0901()
        {
            InitializeComponent();
            client = new PcaWebApiClient();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            Text = "PCA会計DX 消込結果連携";
            grid.SetupShortcutKeys();
            client.ErrorListener = message => Errors.Add(message);
        }


        private void InitializeHandlers()
        {
            Load += PE0901_Load;
            lblExtractNumber.Paint += LabelOnPaint;
            lblExtractAmount.Paint += LabelOnPaint;
            lblOutputNumber .Paint += LabelOnPaint;
            lblOutputAmount .Paint += LabelOnPaint;
            grid.CellEditedFormattedValueChanged += grid_CellEditedFormattedValueChanged;
            grid.CellValueChanged                += grid_CellValueChanged;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("抽出");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("印刷");
            BaseContext.SetFunction04Caption("出力");
            BaseContext.SetFunction05Caption("連携設定");
            //BaseContext.SetFunction06Caption("再出力");
            BaseContext.SetFunction07Caption("再印刷");
            BaseContext.SetFunction08Caption("取消");
            BaseContext.SetFunction09Caption("ログ出力");
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
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Extract;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = Print;
            OnF04ClickHandler = Output;
            OnF05ClickHandler = ShowSetting;
            //OnF06ClickHandler = ReOutput;
            OnF07ClickHandler = RePrint;
            OnF08ClickHandler = Cancel;
            OnF09ClickHandler = OutputErrorLog;
            OnF10ClickHandler = Close;
        }

        private void PE0901_Load(object sender, EventArgs e)
        {
            SetScreenName();
            ProgressDialog.Start(ParentForm, InitializeLoadAsync(), false, SessionKey);
        }

        private async Task InitializeLoadAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadWebApiSettingAsync(),
            };
            await Task.WhenAll(tasks);

            Clear();

            if (!UseForeignCurrency)
            {
                var currency = await GetCurrencyAsync(DefaultCurrencyCode);
                if (currency != null)
                {
                    CurrencyId = currency.Id;
                    Precision = currency.Precision;
                }
                lblCurrencyCode.Visible = false;
                txtCurrencyCode.Visible = false;
                btnCurrencyCode.Visible = false;
                InitializeGridTemplate();
                await SetHistoryDataAsync();
            }
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            var widthCcy = (UseForeignCurrency ? 80 : 0);
            var widthAmt = (UseForeignCurrency ? 100 : 180);
            var middleCenter = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new CellSetting[] {
                new CellSetting(height, 50      , nameof(JournalizingSummary.Selected)    , dataField: nameof(JournalizingSummary.Selected)    , caption: "選択"  , cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false),
                new CellSetting(height, 135     , nameof(JournalizingSummary.OutputAt)    , dataField: nameof(JournalizingSummary.OutputAt)    , caption: "仕訳日", cell: builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height, 100     , nameof(JournalizingSummary.Count)       , dataField: nameof(JournalizingSummary.Count)       , caption: "件数"  , cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height, widthCcy, nameof(JournalizingSummary.CurrencyCode), dataField: nameof(JournalizingSummary.CurrencyCode), caption: "通貨"  , cell: builder.GetTextBoxCell(middleCenter)),
                new CellSetting(height, widthAmt, nameof(JournalizingSummary.Amount)      , dataField: nameof(JournalizingSummary.Amount)      , caption: "金額"  , cell: builder.GetTextBoxCurrencyCell(Precision)),
            });
            grid.Template = builder.Build();
            grid.HideSelection = true;
        }

        private async Task SetHistoryDataAsync()
        {
            var history = await GetMatchingJournalizingAsummaryAsnc();
            grid.DataSource = new BindingSource(history, null);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
        }

        #endregion

        #region function keys

        [OperationLog("抽出")]
        private void Extract()
        {
            ClearStatusMessage();
            if (!ValidateInputValues()) return;
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
            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction03Enabled(true);

            var loadPcaTask = LoadPcaMasterDataAsync();
            ProgressDialog.Start(ParentForm, loadPcaTask, false, SessionKey);
            if (Errors.Any()) BaseContext.SetFunction09Enabled(true);
            if (loadPcaTask.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, loadPcaTask.Exception, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "抽出");
                return;
            }
            var loadResult = loadPcaTask.Result;
            if (!loadResult)
            {
                ShowWarningDialog(MsgErrSomethingError, "抽出");
                return;
            }

            if (!TransferExtractedData())
            {
                if (Errors.Any()) BaseContext.SetFunction09Enabled(true);
                ShowWarningDialog(MsgErrTransformErrorWithLog);
                return;
            }
            BaseContext.SetFunction04Enabled(true);
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            Errors.Clear();

            datRecordedAtFrom.Clear();
            datRecordedAtTo  .Clear();
            datRecordedAtFrom.Enabled = true;
            datRecordedAtTo  .Enabled = true;
            lblExtractNumber.Clear();
            lblExtractAmount.Clear();
            lblOutputNumber.Clear();
            lblOutputAmount.Clear();
            txtCurrencyCode.Clear();
            if (UseForeignCurrency)
            {
                txtCurrencyCode.Enabled = true;
                btnCurrencyCode.Enabled = true;
                CurrencyId = 0;
                Precision = 0;
                grid.DataSource = null;
                BaseContext.SetFunction01Enabled(false);
            }
            else
                BaseContext.SetFunction01Enabled(true);

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            ClearStatusMessage();
        }

        [OperationLog("印刷")]
        private void Print()
        {
            ClearStatusMessage();

            if (!ValidateAuthorization()) return;

            var filtered = GetFilteredExtractMatching(ExtractMatching);

            PrintReport(filtered);

        }

        [OperationLog("出力")]
        private void Output()
        {
            ClearStatusMessage();
            var task = OutputJournalizingAsync();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return;
            }

            DispStatusMessage(MsgInfFinishDataExtracting);
        }

        [OperationLog("連携設定")]
        private void ShowSetting()
        {
            ClearStatusMessage();
            var settingSaveResult = DialogResult.Cancel;
            using (var form = ApplicationContext.Create(nameof(PH1201)))
            {
                var screen = form.GetAll<PH1201>().First();
                screen.ParentScreen = this;
                settingSaveResult = ApplicationContext.ShowDialog(ParentForm, form);
            }
            if (settingSaveResult != DialogResult.OK) return;
            ProgressDialog.Start(ParentForm, LoadWebApiSettingAsync(), false, SessionKey);
        }

        //[OperationLog("再出力")]
        //private void ReOutput()
        //{

        //}

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

        [OperationLog("ログ出力")]
        private void OutputErrorLog()
        {
            ClearStatusMessage();
            var pathTask = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, pathTask, false, SessionKey);
            var path = string.Empty;
            if (!ShowSaveFileDialog(pathTask.Result,
                $"PCA会計DXエラーログ{DateTime.Today:yyyyMMdd}.txt",
                out path, filter: "すべてのファイル (*.*)|*.*|TXTファイル (*.txt)|*.txt"))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            var result = false;
            try
            {
                using (var writer = new System.IO.StreamWriter(path, true, Encoding.GetEncoding(932)))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
                    writer.WriteLine(string.Join(Environment.NewLine, Errors));
                    writer.WriteLine();
                }
                result = true;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (!result) return;
        }

        [OperationLog("終了")]
        private void Close()
        {
            ParentForm.Close();
        }

        #endregion

        #region extract

        private bool ValidateInputValues()
        {
            if (UseForeignCurrency && !txtCurrencyCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblCurrencyCode.Text))) return false;
            if (!datRecordedAtFrom.ValidateRange(datRecordedAtTo, () => ShowWarningDialog(MsgWngInputRangeChecked, lblRecordedAt.Text))) return false;
            return true;
        }

        private async Task<bool> LoadExtractedDataAsync()
        {
            ExtractMatching = await ExtractAsync();
            var enable = ExtractMatching.Any();
            datRecordedAtFrom.Enabled = !enable;
            datRecordedAtTo.Enabled = !enable;
            txtCurrencyCode.Enabled = UseForeignCurrency && !enable;
            btnCurrencyCode.Enabled = UseForeignCurrency && !enable;

            lblOutputNumber.Clear();
            lblOutputAmount.Clear();

            lblExtractNumber.Text = ExtractedCount.ToString("#,##0");
            lblExtractAmount.Text = ExtractedAmount.ToString("#,##0");
            return enable;
        }

        private bool TransferExtractedData()
        {
            InputSlips = GetFilteredExtractMatching(ExtractMatching).Convert(
                PcaAccountTitleDictionary,
                PcaDepartmentDictionary,
                PcaCustomerDictionary,
                BEComp.RequireDepartmentAllAccount,
                BEComp.RequireDepartmentPLAccount,
                message => Errors.Add(message));
            return !Errors.Any();
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

        #region filtered

        private List<MatchingJournalizing> GetFilteredExtractMatching(List<MatchingJournalizing> source)
        {
            if (!UseAuthorization) return source;
            return source.Where(x => x.Approved).ToList();
        }

        #endregion

        #region journalizing option

        private JournalizingOption GetJournalizingOption(List<DateTime> outputAt = null) => new JournalizingOption {
            CompanyId = CompanyId,
            CurrencyId = CurrencyId,
            RecordedAtFrom = datRecordedAtFrom.Value,
            RecordedAtTo = datRecordedAtTo.Value,
            OutputAt = outputAt ?? new List<DateTime>(),
            UseDiscount = UseDiscount,
            LoginUserId = Login.UserId,
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
                                      : MsgInfPrintExtractedData );
        }

        #endregion

        #region output journalizing

        private async Task<bool> OutputJournalizingAsync()
        {
            var summary = await GetMatchingJournalizingAsummaryAsnc(isOutputted: false);
            var source = GetFilteredExtractMatching(ExtractMatching);
            var count = source.Count;
            var amount = source.Sum(x => x.Amount);
            if (!(count == summary.Sum(x => x.Count) && amount == summary.Sum(x => x.Amount)))
            {
                DoActionOnUI(() => ShowWarningDialog(MsgWngNotEqualAbstractAmountAndUpdateAmount));
                return false;
            }

            var errorInfo = new StringBuilder();
            var createResult = await client.CreateSlipAsync(InputSlips,
                message => errorInfo.AppendLine(message));
            if (!createResult)
            {
                NLogHandler.WriteErrorLog(this, errorInfo.ToString());
                DoActionOnUI(() => ShowWarningDialog(MsgErrSaveError));
                return false;
            }

            var updateResult = await UpdateOutpuAtAsync();
            if (!updateResult)
            {
                // TODO: add correct message
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
                => await client.ExtractMatchingJournalizingAsync(SessionKey, option));
            if (result.ProcessResult.Result)
                return result.MatchingJournalizings;
            return new List<MatchingJournalizing>();
        }

        private async Task<Currency> GetCurrencyAsync(string code)
            => (await ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client)
                => await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code })))?.Currencies.FirstOrDefault();

        private async Task LoadWebApiSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client) => {
                var result = await client.GetByIdAsync(SessionKey, CompanyId, WebApiType.PcaDX);
                if (result.ProcessResult.Result)
                    this.client.WebApiSetting = result.WebApiSetting;
            });

        private async Task<List<JournalizingSummary>> GetMatchingJournalizingAsummaryAsnc(
            bool isOutputted = true)
            => await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client) =>
            {
                var option = GetJournalizingOption();
                option.IsOutputted = isOutputted;
                if (isOutputted)
                {
                    option.RecordedAtFrom = null;
                    option.RecordedAtTo   = null;
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

        #region call web api

        private async Task<bool> LoadPcaMasterDataAsync()
        {
            Errors.Clear();
            var dataArea = await client.SelectDataAreaAsync(SelectAreaForm.SelectByGUI);
            if (!dataArea) return false;
            var tasks = new List<Task> {
                LoadBECompAsync(),
                LoadBEBuAsync(),
                LoadBEKmkAsync(),
                LoadBEHojoAsync()
            };
            await Task.WhenAll(tasks);
            if (tasks.Any(x => x.Exception != null))
            {
                foreach (var task in tasks.Where(x => x.Exception != null))
                    NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return false;
            }
            return true;
        }
        private async Task LoadBECompAsync()
            => BEComp = await client.GetCompAsync();

        private async Task LoadBEKmkAsync()
            => PcaAccountTitleDictionary = (await client.GetKmkAsync()).ToList().ToDictionary(x => x.Code);

        private async Task LoadBEBuAsync()
            => PcaDepartmentDictionary = (await client.GetBuAsync()).ToList().ToDictionary(x => x.Code);

        private async Task LoadBEHojoAsync()
            => PcaCustomerDictionary = new SortedList<string, BEHojo[]>(
                (await client.GetHojoAsync()).ToList().GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.ToArray()));

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
            //BaseContext.SetFunction06Enabled(enable);
            BaseContext.SetFunction07Enabled(enable);
            BaseContext.SetFunction08Enabled(enable);
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                CurrencyId = 0;
                return;
            }
            var code = txtCurrencyCode.Text;
            var task = GetCurrencyAsync(code);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var currency = task.Result;
            if (currency == null)
            {
                txtCurrencyCode.Clear();
                txtCurrencyCode.Focus();
                ShowWarningDialog(MsgWngMasterNotExist, "通貨", code);
                CurrencyId = 0;
                return;
            }
            SetCurrencyInfo(currency);
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var currency = this.ShowCurrencySearchDialog();
            if (currency == null) return;
            txtCurrencyCode.Text = currency.Code;
            SetCurrencyInfo(currency);
        }

        private void SetCurrencyInfo(Currency currency)
        {
            CurrencyId = currency.Id;
            Precision = currency.Precision;
            BaseContext.SetFunction01Enabled(true);
            txtCurrencyCode.Enabled = false;
            btnCurrencyCode.Enabled = false;
            InitializeGridTemplate();
            ProgressDialog.Start(ParentForm, SetHistoryDataAsync(), false, SessionKey);
        }

        private void LabelOnPaint(object sender, PaintEventArgs e)
            => ControlPaint.DrawBorder(e.Graphics, ((Control)sender).DisplayRectangle, Color.Wheat, ButtonBorderStyle.Solid);

        #endregion

    }
}
