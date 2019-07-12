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
using Rac.VOne.Client.Common.HttpClients;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.HatarakuDBJournalizingService;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using Rac.VOne.Web.Models;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>働くDB 消込結果連携</summary>
    public partial class PE0801 : VOneScreenBase
    {
        private int Precision { get; set; }
        private int CurrencyId { get; set; }
        private HatarakuDBClient WebApiClient { get; set; }
        private WebApiSetting WebApiSetting { get; set; }
        #region initialize

        public PE0801()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            Text = "働くDB 消込結果連携";
            WebApiClient = new HatarakuDBClient();
        }

        private void InitializeHandlers()
        {
            Load += PE0801_Load;
            grid.CellEditedFormattedValueChanged += grid_CellEditedFormattedValueChanged;
            grid.CellValueChanged                += grid_CellValueChanged;
            lblDispExtractNumber.Paint           += LabelOnPaint;
            lblDispExtractAmount.Paint           += LabelOnPaint;
            txtCurrencyCode.Validated            += txtCurrencyCode_Validated;
            btnCurrencyCode.Click                += btnCurrencyCode_Click;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("連携");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction08Caption("取消");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Transfer;
            OnF02ClickHandler = Clear;
            OnF08ClickHandler = Cancel;
            OnF10ClickHandler = Close;
        }

        private void PE0801_Load(object sender, EventArgs e)
        {
            SetScreenName();
            ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
            ClearControlValues();
            InitializeGridTemplate();
            InitializeControlValues();
            if (WebApiSetting == null)
            {
                ShowWarningDialog(MsgWngNotSettingMaster, "働くDB WebAPI 連携設定");
            }
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                Task.Run(async () => WebApiSetting = await GetWebApiSettingAsync()),
            };
            if (!UseForeignCurrency)
                tasks.Add(LoadCurrencyAsync(Constants.DefaultCurrencyCode));
            await Task.WhenAll(tasks);
            BaseContext.SetFunction01Enabled(WebApiSetting != null);
            WebApiClient.WebApiSetting = WebApiSetting;
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var widthCcy = UseForeignCurrency ? 80 : 0;
            var widthAmt = UseForeignCurrency ? 100 : 180;
            var middleCenter = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new CellSetting[] {
                new CellSetting(height,       50, nameof(JournalizingSummary.Selected)    , dataField: nameof(JournalizingSummary.Selected)    , caption: "選択"  , cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false),
                new CellSetting(height,      135, nameof(JournalizingSummary.OutputAt)    , dataField: nameof(JournalizingSummary.OutputAt)    , caption: "仕訳日", cell: builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height,      100, nameof(JournalizingSummary.Count)       , dataField: nameof(JournalizingSummary.Count)       , caption: "件数"  , cell: builder.GetNumberCell()),
                new CellSetting(height, widthCcy, nameof(JournalizingSummary.CurrencyCode), dataField: nameof(JournalizingSummary.CurrencyCode), caption: "通貨"  , cell: builder.GetTextBoxCell(middleCenter)),
                new CellSetting(height, widthAmt, nameof(JournalizingSummary.Amount)      , dataField: nameof(JournalizingSummary.Amount)      , caption: "金額"  , cell: builder.GetTextBoxCurrencyCell(Precision)),
            });
            grid.Template = builder.Build();
            grid.HideSelection = true;
        }

        private void InitializeControlValues()
        {
            txtCurrencyCode.Clear();
            if (!UseForeignCurrency)
            {
                lblCurrencyCode.Visible = false;
                txtCurrencyCode.Visible = false;
                btnCurrencyCode.Visible = false;
                LoadHistoryData();
            }
            else
            {
                grid.DataSource = null;
            }
        }
        #endregion


        #region function keys

        [OperationLog("連携")]
        private void Transfer()
        {
            ClearStatusMessage();
            if (!ValidateInputValues()) return;
            var task = TransferInnerAsync();

            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                ShowWarningDialog(MsgErrSomethingError, "働くDB データ連携");
                return;
            }

            DispStatusMessage(MsgInfProcessFinish);
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            ClearControlValues();
        }

        [OperationLog("取消")]
        private void Cancel()
        {
            ClearStatusMessage();
            if (!ShowConfirmDialog(MsgQstConfirmCancelJournalizing)) return;
            var task = CancelInnerAsync();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (!task.Result)
            {
                ShowWarningDialog(MsgErrSomethingError, "取消");
                return;
            }
            if (task.Exception != null)
            {
                ShowWarningDialog(MsgErrSomethingError, "取消");
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return;
            }
            DispStatusMessage(MsgInfFinishedSuccessJournalizingCancelingProcess);
        }

        [OperationLog("終了")]
        private void Close()
        {
            ClearStatusMessage();
            ParentForm.Close();
        }

        #endregion

        #region clear

        private void ClearControlValues()
        {
            datRecordedAtFrom.Clear();
            datRecordedAtTo.Clear();
            txtCurrencyCode.Clear();

            if (UseForeignCurrency)
            {
                grid.DataSource = null;
            }
            else
            {
                LoadHistoryData();
            }
        }

        #endregion

        #region load data

        private void LoadHistoryData()
        {
            var task = GetHistoryAsync();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var source = task.Result;
            grid.DataSource = new BindingSource(source, null);
        }

        #endregion

        #region transfer

        private bool ValidateInputValues()
        {
            if (UseForeignCurrency
                && !txtCurrencyCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblCurrencyCode.Text))) return false;
            if (!datRecordedAtFrom.ValidateRange(datRecordedAtTo, () => ShowWarningDialog(MsgWngInputRangeChecked, lblRecordedAt.Text))) return false;
            return true;
        }

        private async Task<bool> TransferInnerAsync()
        {
            var data = await GetTransferDataAsync();
            if (data == null || !data.Any())
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return false;
            }
            var outputResult = false;
            try
            {
                var result = await WebApiClient.OutputAsync(data);
                outputResult = true;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (!outputResult)
            {
                ShowWarningDialog(MsgErrSomethingError, "働くDB データ連携");
                return false;
            }

            var updateResult = false;
            try
            {
                updateResult = await UpdateAsync();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (!updateResult)
            {
                ShowWarningDialog(MsgErrSomethingError, "働くDB データ連携");
                return false;
            }

            var source = await GetHistoryAsync();
            grid.DataSource = new BindingSource(source, null);

            lblDispExtractNumber.Text = data.Count.ToString("#,##0");
            lblDispExtractAmount.Text = data.Sum(x => x.AssignmentAmount).ToString("#,##0");

            return true;
        }

        #endregion

        #region cancel

        private async Task<bool> CancelInnerAsync()
        {
            var outputAts = GetSelectedOutputAt();

            var cancelResult = await CancelAsync(outputAts);
            if (!cancelResult) return false;

            var summaries = await GetHistoryAsync();
            grid.DataSource = new BindingSource(summaries, null);

            BaseContext.SetFunction08Enabled(false);

            return true;
        }

        #endregion

        #region call web service

        private List<DateTime> GetSelectedOutputAt() => grid.Rows
            .Select(x => x.DataBoundItem as JournalizingSummary)
            .Where(x => x.Selected)
            .Select(x => x.OutputAt.Value)
            .ToList();

        private JournalizingOption GetJournalizingOption(List<DateTime> outputAt = null) => new JournalizingOption
        {
            CompanyId = CompanyId,
            CurrencyId = CurrencyId,
            RecordedAtFrom = datRecordedAtFrom.Value,
            RecordedAtTo = datRecordedAtTo.Value,
            OutputAt = outputAt ?? new List<DateTime>(),
            LoginUserId = Login.UserId,
        };

        private async Task<List<JournalizingSummary>> GetHistoryAsync(bool isOutputted = true)
            => await ServiceProxyFactory.DoAsync(async (HatarakuDBJournalizingServiceClient client) => {
                var option = GetJournalizingOption();
                option.IsOutputted = isOutputted;
                if (isOutputted) {
                    option.RecordedAtFrom = null;
                    option.RecordedAtTo = null;
                }
                var result = await client.GetSummaryAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.JournalizingsSummaries;
                return new List<JournalizingSummary>();
            });

        private async Task<List<HatarakuDBData>> GetTransferDataAsync(List<DateTime> outputAt = null)
        {
            var option = GetJournalizingOption(outputAt);
            return await ServiceProxyFactory.DoAsync(async (HatarakuDBJournalizingServiceClient client) => {
                var result = await client.ExtractAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.HatarakuDBData;
                return null;
            });
        }

        private async Task LoadCurrencyAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CurrencyMasterService.CurrencyMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result) {
                    var currency = result.Currencies.FirstOrDefault();
                    if (currency == null) return;
                    CurrencyId = currency.Id;
                    Precision  = currency.Precision;
                }
            });

        private async Task<WebApiSetting> GetWebApiSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client) => {
                var result = await client.GetByIdAsync(SessionKey, CompanyId, WebApiType.HatarakuDb);
                if (result.ProcessResult.Result)
                    return result.WebApiSetting;
                return null;
            });

        private async Task<bool> UpdateAsync()
            => await ServiceProxyFactory.DoAsync(async (HatarakuDBJournalizingServiceClient client)
                => (await client.UpdateAsync(SessionKey, GetJournalizingOption()))?.ProcessResult.Result ?? false);


        private async Task<bool> CancelAsync(List<DateTime> outputAt)
            => await ServiceProxyFactory.DoAsync(async (HatarakuDBJournalizingServiceClient client)
                => (await client.CancelAsync(SessionKey, GetJournalizingOption(outputAt)))?.ProcessResult.Result ?? false);

        #endregion

        #region event handler

        private string CellName(string value) => $"cel{value}";
        private void grid_CellEditedFormattedValueChanged(object sender, GrapeCity.Win.MultiRow.CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellName != CellName(nameof(JournalizingSummary.Selected))) return;
            grid.EndEdit();
        }

        private void grid_CellValueChanged(object sender, GrapeCity.Win.MultiRow.CellEventArgs e)
        {
            if (e.CellName != CellName(nameof(JournalizingSummary.Selected))) return;
            var enabled = grid.Rows.Any(x => (x.DataBoundItem as JournalizingSummary)?.Selected ?? false);
            BaseContext.SetFunction08Enabled(enabled);
        }

        private void LabelOnPaint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            ControlPaint.DrawBorder(e.Graphics, control.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            CurrencyId = 0;

            if (string.IsNullOrEmpty(txtCurrencyCode.Text)) return;

            var task = LoadCurrencyAsync(txtCurrencyCode.Text);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return;
            }
            if (CurrencyId == 0)
            {
                ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                txtCurrencyCode.Clear();
                txtCurrencyCode.Focus();
                return;
            }
        }

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var currency = this.ShowCurrencySearchDialog();
            if (currency == null) return;
            txtCurrencyCode.Text = currency.Code;
            CurrencyId = currency.Id;
            Precision = currency.Precision;
        }

        #endregion
    }
}
