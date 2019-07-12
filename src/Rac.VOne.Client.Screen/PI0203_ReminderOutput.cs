using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using GrapeCity.ActiveReports.Document.Section;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.PdfOutputSettingMasterService;
using Rac.VOne.Client.Screen.ReminderService;
using Rac.VOne.Client.Screen.DestinationMasterService;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Web.Models;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>督促状発行</summary>
    public partial class PI0203 : VOneScreenBase
    {
        // 変数宣言
        private int Precision { get; set; }
        List<ReportSetting> ReportSettingList { get; set; }
        private List<ColumnNameSetting> ColumnNameSettingInfo { get; set; }
        private ReminderSearch SearchCondition { get; set; }
        private string CellName(string value) => $"cel{value}";
        public ReminderCommonSetting ReminderCommonSetting { get; set; }
        public List<ReminderSummarySetting> ReminderSummarySetting { get; set; }
        public Currency Currency { get; set; }
        public int CustomerId { get; set; }
        public DateTime CalculateBaseDate { get; set; }
        public string CurrencyCode { get; set; }
        private Customer Customer { get; set; }
        public IEnumerable<ReminderSummary> ReminderSummary { get; set; }
        public List<ReminderTemplateSetting> ReminderTemplateSetting { get; set; }
        private bool UseDestinationSummarized { get; set; }
        private bool IsCellEmpty(Cell cell)
            => (cell.Value == null
             || string.IsNullOrEmpty(cell.Value.ToString())
             || cell.Name == CellName(nameof(Web.Models.ReminderSummary.DestinationCode)) && 0.Equals(cell.Value));
        private PdfOutputSetting PdfSetting { get; set; }
        public PI0203()
        {
            InitializeComponent();
            grdReminder.SetupShortcutKeys();
            Text = "督促状発行";
            ReportSettingList = new List<ReportSetting>();
        }

        #region PF0401 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("出力");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Print;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

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
                
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SuspendLayout();
                ResumeLayout();
                SetInitialSetting();
                ClearControls();
                grdReminder.Text = "";
                InitializeReminderGrid();

                var templateId = ReminderTemplateSetting.First().Id;
                foreach(var r in ReminderSummary)
                {
                    r.TemplateId = templateId;
                }
                grdReminder.DataSource = new BindingSource(ReminderSummary, null);
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
            if (ReminderTemplateSetting?.Count > 0)
            {
                cmbReminderTemplate.TextSubItemIndex = 0;
                cmbReminderTemplate.ValueSubItemIndex = 1;
                cmbReminderTemplate.Items.AddRange(ReminderTemplateSetting.Select(x =>
                    new ListItem(x.Name, x.Id)).ToArray());
            }

            UseDestinationSummarized = ReminderSummarySetting.Any(x => x.ColumnName == "DestinationCode" && x.Available == 1);
            if (UseDestinationSummarized)
            {
                BaseContext.SetFunction09Caption("検索");
                BaseContext.SetFunction09Enabled(false);
                OnF09ClickHandler = ShowDestinationCodeSearchDialog;
            }
        }

        #endregion

        #region グリッド作成
        private void InitializeReminderGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var widthCcy = UseForeignCurrency ? 60 : 0;
            var widthTmp = UseDestinationSummarized ? 308 : 368;

            var reminderAmountCell = builder.GetNumberCellCurrency(Precision, Precision, 0);
            reminderAmountCell.AllowDeleteToNull = true;

            var arrearsDaysCell = builder.GetNumberCell();
            arrearsDaysCell.AllowDeleteToNull = true;

            var templateCell = builder.GetComboBoxCell();
            if (ReminderTemplateSetting?.Count > 0)
            {
                templateCell.DataSource = ReminderTemplateSetting;
                templateCell.ValueMember = nameof(Web.Models.ReminderTemplateSetting.Id);
                templateCell.DisplayMember = nameof(Web.Models.ReminderTemplateSetting.Name);
            }

            builder.Items.Add(new CellSetting(height, 115, nameof(Web.Models.ReminderSummary.CustomerCode), dataField: nameof(Web.Models.ReminderSummary.CustomerCode), caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 200, nameof(Web.Models.ReminderSummary.CustomerName), dataField: nameof(Web.Models.ReminderSummary.CustomerName), caption: "得意先名",    cell: builder.GetTextBoxCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, 70, nameof(Web.Models.ReminderSummary.BillingCount), dataField: nameof(Web.Models.ReminderSummary.BillingCount), caption: "明細件数", cell: builder.GetNumberCell(), sortable: true));
            builder.Items.Add(new CellSetting(height, widthCcy, nameof(Web.Models.ReminderSummary.CurrencyCode), dataField: nameof(Web.Models.ReminderSummary.CurrencyCode), caption: "通貨コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true));
            builder.Items.Add(new CellSetting(height, 120, nameof(Web.Models.ReminderSummary.ReminderAmount), dataField: nameof(Web.Models.ReminderSummary.ReminderAmount), caption: "滞留金額", cell: reminderAmountCell, sortable: true));
            builder.Items.Add(new CellSetting(height, widthTmp, nameof(Web.Models.ReminderSummary.TemplateId), dataField: nameof(Web.Models.ReminderSummary.TemplateId),  caption: "文面パターン", cell: templateCell, readOnly: false, sortable: true));
            if (UseDestinationSummarized)
                builder.Items.Add(new CellSetting(height, 60, nameof(Web.Models.ReminderSummary.DestinationCode), dataField: nameof(Web.Models.ReminderSummary.DestinationCode), caption: "送付先", cell: builder.GetTextBoxCell(align: middleCenter, ime: ImeMode.Disable, format: "9", maxLength: 2), readOnly: false, sortable: true));
            builder.Items.Add(new CellSetting(height, 60, "Preview", caption: "プレビュー", cell: builder.GetButtonCell(), readOnly: false));

            grdReminder.Template = builder.Build();
            grdReminder.HideSelection = true;
            grdReminder.AllowAutoExtend = false;

            grdReminder.EditMode = GrapeCity.Win.MultiRow.EditMode.EditOnKeystrokeOrShortcutKey;
            grdReminder.AllowClipboard = true;
            grdReminder.AllowDrop = true;
            grdReminder.MultiSelect = true;
            grdReminder.ShortcutKeyManager.Register(EditingActions.Paste, Keys.Control | Keys.V);
            grdReminder.ShortcutKeyManager.Register(EditingActions.Clear, Keys.Delete);
            grdReminder.ShortcutKeyManager.Register(EditingActions.Clear, Keys.Back);
        }

        #endregion

        #region Function Key Event

        private void ClearControls()
        {
            ClearStatusMessage();
            grdReminder.DataSource = null;
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

        [OperationLog("出力")]
        private void Print()
        {
            ClearStatusMessage();
            if (!ValidateInputValueForPrint())
                return;

            try
            {
                var path = string.Empty;
                PdfSetting = GetPdfOutputSetting();
                if (!PdfSetting.IsAllInOne)
                {
                    var serverPath = GetServerPath();
                    var selectedPath = string.Empty;
                    var rootBrowserPath = new List<string>();
                    if (!LimitAccessFolder ?
                        !ShowFolderBrowserDialog(serverPath, out selectedPath) :
                        !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder))
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

                if (!ReminderSummary.Any())
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist, "出力");
                    return;
                }

                grdReminder.CommitEdit();
                Task task = null;
                if (!UseDestinationSummarized)
                     task = PrintSummarized(path);
                else
                    task = PrintDestinationSummarized(path);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrExportError);
            }
        }

        private async Task PrintSummarized(string path)
        {
            try
            {
                var reminderOutputed = new List<ReminderOutputed>();
                var updateResult = new CountResult();


                List<ReminderBilling> reminderBilling = null;
                var outputNo = 1;

                var reminderResult = await GetReminderBillingForSummaryPrintAsync(ReminderSummary.Select(x => x.CustomerId).ToArray());
                reminderBilling = reminderResult.ReminderBilling;
                var countResult = await GetMaxOutputNoAsync();
                if(countResult.ProcessResult.Result)
                    outputNo = countResult.Count + 1;

                var reportList = new List<ReminderReport>();
                var now = DateTime.Now;
                ReminderReport allReport = null;

                foreach (var r in ReminderSummary)
                {
                    var template = ReminderTemplateSetting.First(x => x.Id == r.TemplateId);
                    var source = reminderBilling.Where(x => x.CustomerId == r.CustomerId);

                    foreach (var b in source)
                    {
                        var output = new ReminderOutputed();
                        output.OutputNo = outputNo;
                        output.BillingId = b.Id;
                        output.ReminderId = b.ReminderId;
                        output.RemainAmount = b.RemainAmount;
                        output.BillingAmount = b.BillingAmount;
                        output.ReminderTemplateId = r.TemplateId;
                        output.OutputAt = now;
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
                    {
                        reportList.Add(report);
                    }

                    outputNo++;
                }

                if (PdfSetting.IsAllInOne)
                {
                    Action<Form> outputHandler = owner =>
                    {
                        var taskOutput = SaveOutputAtReminderSummaryAsync(reminderOutputed.ToArray(), ReminderSummary.ToArray());
                        ProgressDialog.Start(owner, taskOutput, false, SessionKey);
                        updateResult = taskOutput.Result;
                    };
                    updateResult = null;
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
                        if (PdfSetting.UseZip) fileList.Add(filePath);
                    }
                    if (PdfSetting.UseZip)
                        Util.ArchivesAsZip(fileList, path, $"督促状{now.ToString("yyyyMMdd")}", PdfSetting.MaximumByte);
                    updateResult = await SaveOutputAtReminderSummaryAsync(reminderOutputed.ToArray(), ReminderSummary.ToArray());
                }

                if (PdfSetting.IsAllInOne && updateResult == null)
                    return;
                else if (!updateResult.ProcessResult.Result || updateResult.Count <= 0)
                    ShowWarningDialog(MsgErrExportError);
                else
                    ParentForm.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrExportError);
            }
        }

        private async Task PrintDestinationSummarized(string path)
        {
            try
            {
                var reminderOutputed = new List<ReminderOutputed>();
                var updateResult = new CountResult();

                var option = new DestinationSearch();
                option.CompanyId = CompanyId;

                var destinations = await GetDestinationsAsync(option);

                foreach (var summary in ReminderSummary)
                {
                    if (summary.DestinationCode == null || string.IsNullOrEmpty(summary.DestinationCode))
                    {
                        summary.NoDestination = true;
                        summary.DestinationIdInput = null;
                        continue;
                    }

                    var destination = destinations.FirstOrDefault(x => x.CustomerId == summary.CustomerId && x.Code == summary.DestinationCode);
                    if (destination == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "送付先", summary.DestinationCode);
                        return;
                    }
                    summary.DestinationIdInput = destination.Id;
                }

                var summaryList = new List<ReminderSummary>();
                var gDestination = ReminderSummary.GroupBy(x => new { x.CustomerCode, x.DestinationCode }).Select(x => x);

                foreach (var grs in gDestination)
                {
                    var summary = grs.First();
                    var destinationIds = new List<ReminderSummary>();
                    foreach (var item in ReminderSummary.Where(x => x.CustomerId == summary.CustomerId && x.DestinationCode == summary.DestinationCode))
                    {
                        if (item.DestinationId == null)
                            summary.NoDestination = true;
                        else
                            destinationIds.Add(item);
                    }
                    summary.CompanyId = CompanyId;
                    summary.CustomerIds = new int[] { summary.CustomerId };
                    summary.DestinationIds = destinationIds.Select(x => x.DestinationId.Value).Distinct().ToArray();
                    summaryList.Add(summary);
                }

                var reminderBillingResult = await GetReminderBillingForSummaryPrintByDestinationCodeAsync(summaryList);
                var reminderBilling = reminderBillingResult.ReminderBilling;

                var outputNo = 1;

                var countResult = await GetMaxOutputNoAsync();
                if (countResult.ProcessResult.Result)
                    outputNo = countResult.Count + 1;

                var reportList = new List<ReminderReport>();
                var now = DateTime.Now;
                ReminderReport allReport = null;

                foreach (var gdr in gDestination)
                {
                    var r = gdr.First();

                    var template = ReminderTemplateSetting.First(x => x.Id == r.TemplateId);
                    var source = reminderBilling.Where(x => x.CustomerId == r.CustomerId && x.DestinationId == r.DestinationIdInput);

                    foreach (var b in source)
                    {
                        var output = new ReminderOutputed();
                        output.OutputNo = outputNo;
                        output.BillingId = b.Id;
                        output.ReminderId = b.ReminderId;
                        output.RemainAmount = b.RemainAmount;
                        output.BillingAmount = b.BillingAmount;
                        output.ReminderTemplateId = r.TemplateId;
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

                if (PdfSetting.IsAllInOne)
                {
                    Action<Form> outputHandler = owner =>
                    {
                        var taskOutput = SaveOutputAtReminderSummaryAsync(reminderOutputed.ToArray(), ReminderSummary.ToArray());
                        ProgressDialog.Start(owner, taskOutput, false, SessionKey);
                        updateResult = taskOutput.Result;
                    };
                    updateResult = null;
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
                        if (PdfSetting.UseZip)
                            fileList.Add(filePath);
                    }
                    if (PdfSetting.UseZip)
                        Util.ArchivesAsZip(fileList, path, $"督促状{now.ToString("yyyyMMdd")}", PdfSetting.MaximumByte);
                    updateResult = await SaveOutputAtReminderSummaryAsync(reminderOutputed.ToArray(), ReminderSummary.ToArray());
                }

                if (PdfSetting.IsAllInOne && updateResult == null)
                    return;
                else if (!updateResult.ProcessResult.Result || updateResult.Count <= 0)
                    ShowWarningDialog(MsgErrExportError);
                else
                    ParentForm.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrExportError);
            }
        }

        private bool ValidateInputValueForPrint()
        {
            for (var i = 0; i < grdReminder.Rows.Count; i++)
            {
                var r = grdReminder.Rows[i].DataBoundItem as ReminderSummary;
                if (r.TemplateId <= 0)
                {
                    DispStatusMessage(MsgWngSelectionRequired, lblReminderTemplate.Text);
                    //grdReminder.CurrentCellPosition = new CellPosition(i, CellName(nameof(ReminderSummary.TemplateId)));
                    grdReminder.Select();
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>Get Server Path </summary>
        /// <returns>Server Path</returns>
        private string GetServerPath()
        {
            string serverPath = string.Empty;
            ServiceProxyFactory.Do<GeneralSettingMasterClient>(client =>
            {
                GeneralSettingResult result = client.GetByCode(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            return serverPath;
        }

        private async Task<CountResult> SaveOutputAtReminderSummaryAsync(ReminderOutputed[] reminderOutputed, ReminderSummary[] reminderSummary)
        {
            CountResult countResult = null;
            await ServiceProxyFactory.DoAsync<ReminderServiceClient>(async client =>
            {
                countResult = await client.UpdateReminderSummaryOutputedAsync(SessionKey, Login.UserId, reminderOutputed, reminderSummary);
            });
            return countResult;
        }

        [OperationLog("検索")]
        private void ShowDestinationCodeSearchDialog()
        {
            var destination = this.ShowDestinationSearchDialog(CustomerId);
            if (destination == null) return;

            grdReminder.CurrentRow.Cells[CellName(nameof(Web.Models.ReminderSummary.DestinationCode))].Value = destination.Code;
            grdReminder.CurrentRow.Cells[CellName(nameof(Web.Models.ReminderSummary.DestinationCode))].Selected = true;
            BaseContext.SetFunction09Enabled(false);
        }

        [OperationLog("戻る")]
        private void Exit()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region ControlEventHandler

        private void cmbReminderTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReminderTemplate.SelectedIndex < 0) return;

            var id = (int)cmbReminderTemplate.SelectedValue;

            foreach(var r in ReminderSummary)
            {
                r.TemplateId = id;
            }
        }

        #endregion

        #region GridEventHandler

        private void grdReminder_CellClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.CellName != CellName("Preview")) return;

            grdReminder.CommitEdit();

            try
            {
                string serverPath = GetServerPath();
                var summary = grdReminder.Rows[e.RowIndex].DataBoundItem as ReminderSummary;

                if (UseDestinationSummarized)
                {
                    var option = new DestinationSearch();
                    option.CompanyId = CompanyId;
                    option.CustomerId = summary.CustomerId;

                    var taskDestination = GetDestinationsAsync(option);
                    ProgressDialog.Start(ParentForm, taskDestination, false, SessionKey);
                    var destinations = taskDestination.Result;

                    var destinationIds = new List<ReminderSummary>();
                    var inputCode = string.Empty;

                    var cell = grdReminder.Rows[e.RowIndex].Cells[CellName(nameof(Web.Models.ReminderSummary.DestinationCode))];
                    if (IsCellEmpty(cell))
                    {
                        summary.DestinationIdInput = null;
                    }
                    else
                    {
                        inputCode = cell.Value.ToString().PadLeft(2, '0');
                        if (!destinations.Any(x => x.Code == inputCode))
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "送付先", inputCode);
                            return;
                        }
                        summary.DestinationIdInput = destinations.FirstOrDefault(x => x.Code == inputCode).Id;
                    }

                    var qry = from row in grdReminder.Rows where (string)(row.Cells[CellName(nameof(Web.Models.ReminderSummary.CustomerCode))].Value) == summary.CustomerCode select row;
                    foreach (var row in qry)
                    {
                        var bsd = row.DataBoundItem as ReminderSummary;
                        var code = Convert.ToString(row.Cells[CellName(nameof(Web.Models.ReminderSummary.DestinationCode))].Value);
                        if (code == inputCode)
                        {
                            if (bsd.DestinationId.HasValue)
                                destinationIds.Add(bsd);
                            else
                                summary.NoDestination = true;
                        }
                    }

                    summary.CompanyId = CompanyId;
                    summary.CustomerIds = new int[] { summary.CustomerId };
                    summary.DestinationIds = destinationIds.Select(x => x.DestinationId.Value).Distinct().ToArray();
                }

                var summaryList = new List<ReminderSummary>();
                summaryList.Add(summary);

                var task = UseDestinationSummarized ?
                    GetReminderBillingForSummaryPrintByDestinationCodeAsync(summaryList) :
                    GetReminderBillingForSummaryPrintAsync(ReminderSummary.Select(x => x.CustomerId).ToArray());
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!task.Result.ProcessResult.Result)
                {
                    ShowWarningDialog(MsgErrCreateReportError);
                    return;
                }

                var reminderBillings = task.Result.ReminderBilling;

                var template = ReminderTemplateSetting.First(x => x.Id == summary.TemplateId);

                PdfSetting = GetPdfOutputSetting();
                var report = UtilReminder.CreateReminderReport(reminderBillings,
                    ReminderCommonSetting,
                    ReminderSummarySetting,
                    template,
                    Company,
                    ColumnNameSettingInfo,
                    DateTime.Now,
                    PdfSetting);

                ShowDialogPreview(ParentForm, report, serverPath);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        #endregion

        #region webservice

        private async Task<ReminderBillingResult> GetReminderBillingForSummaryPrintAsync(int[] customerIds)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetReminderBillingForSummaryPrintAsync(SessionKey, CompanyId, customerIds);
                return result;
            });

        private async Task<ReminderBillingResult> GetReminderBillingForSummaryPrintByDestinationCodeAsync(IEnumerable<ReminderSummary> reminderSummary)
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetReminderBillingForSummaryPrintByDestinationCodeAsync(SessionKey, reminderSummary.ToArray());
                return result;
            });

        private async Task<List<Destination>> GetDestinationsAsync(DestinationSearch option) =>
            await ServiceProxyFactory.DoAsync(async (DestinationMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Destinations;
                return new List<Destination>();
            });

        private async Task<CountResult> GetMaxOutputNoAsync()
            => await ServiceProxyFactory.DoAsync(async (ReminderServiceClient client) =>
            {
                var result = await client.GetMaxOutputNoAsync(SessionKey, CompanyId);
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

        private void grdReminder_CellValidated(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.CellName != CellName(nameof(Web.Models.ReminderSummary.DestinationCode))) return;

            grdReminder.EndEdit();
            var celDestinationCode = grdReminder.Rows[e.RowIndex].Cells[CellName(nameof(Web.Models.ReminderSummary.DestinationCode))];
            if (IsCellEmpty(celDestinationCode))
            {
                celDestinationCode.Value = string.Empty;
                return;
            }
            celDestinationCode.Value = celDestinationCode.Value.ToString().PadLeft(2, '0');
        }

        private void grdReminder_CellEnter(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.CellName != CellName(nameof(Web.Models.ReminderSummary.DestinationCode)))
            {
                BaseContext.SetFunction09Enabled(false);
                return;
            }
            var summary = grdReminder.Rows[e.RowIndex].DataBoundItem as ReminderSummary;
            CustomerId = summary.CustomerId;
            BaseContext.SetFunction09Enabled(true);
        }

        private void grdReminder_ClipboardOperating(object sender, ClipboardOperationEventArgs e)
        {
            if (e.ClipboardOperation != ClipboardOperation.Paste) return;

            if (grdReminder.SelectedCells.Any(x => x.Name != CellName(nameof(Web.Models.ReminderSummary.DestinationCode))))
            {
                e.Handled = true;
                return;
            }

            var copyCode = string.Empty;
            int code;

            if (Clipboard.ContainsText())
                copyCode = Clipboard.GetText();

            if (!int.TryParse(copyCode, out code)) return;

            foreach (var cell in grdReminder.SelectedCells)
            {
                cell.Value = code.ToString().PadLeft(2, '0');
            }
            e.Handled = true;
        }

    }
}
