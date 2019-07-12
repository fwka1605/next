using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document.Section;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingInvoiceService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CompanyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.DestinationMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ExportFieldSettingMasterService;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.GridSettingMasterService;
using Rac.VOne.Client.Screen.InvoiceSettingService;
using Rac.VOne.Client.Screen.PdfOutputSettingMasterService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Common;
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
    /// <summary>請求書発行</summary>
    public partial class PC0401 : VOneScreenBase
    {
        #region メンバー
        private TaskProgressManager ProgressManager;
        private System.Action OnCancelHandler { get; set; }
        private BillingInvoiceSearch BillingInvoiceSearchCondition { get; set; }
        private string CellName(string value) => $"cel{value}";
        private List<GridSetting> GridSettingInfo { get; set; }
        private byte[] ClientKey { get; set; }
        private int Precision { get; set; }
        private string Note1Name { get; set; }
        private List<CompanyLogo> CompanyLogos { get; set; }
        private int CollectCategoryId { get; set; }
        private List<InvoiceTemplateSetting> TmplateList { get; set; }
        private enum TaskResult {
            Success = 0,
            Modified,
            Error,
            cancel,
        }
        private  List<BillingInvoiceDetailForExport> ListBillingInvoiceDetailForExport { get; set; }
        private List<long> InputIds_PrintTarget { get; set; }
        private SectionReport Report { get; set; }
        private PdfOutputSetting PdfSetting { get; set; }
        private static class FKeyNames
        {
            internal const string F01 = "照会";
            internal const string F02 = "クリア";
            internal const string F03 = "発行";
            internal const string F07 = "エクスポート";
            internal const string F08 = "全選択";
            internal const string F09 = "全解除";
            internal const string F10_close = "終了";
            internal const string F10_return = "戻る";
        }
        #endregion

        #region　初期化
        public PC0401()
        {
            InitializeComponent();
            Text = "請求書発行";
            Precision = UseForeignCurrency ? Precision : 0;
        }
        private void PC0401_Load(object sender, EventArgs e)
        {
            try
            {
                var tasks = new List<Task>();
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (Authorities == null)
                    tasks.Add(LoadFunctionAuthorities(FunctionType.ModifyBilling));
                //tasks.Add(LoadGridSettingAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadClientKeyAsync());
                tasks.Add(LoadColumnNameSettingAsync());
                tasks.Add(LoadCompanyLogosAsync());
                //tasks.Add(SetGeneralSettingAsync());
                //tasks.Add(LoadColumnNameSettingAsync());
                //tasks.Add(LoadLegalPersonalities());

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks.ToArray()), false, SessionKey);

                SetScreenName();
                grid.SetupShortcutKeys();
                InitializeHandlers();
                SetFormLoad();
                InitializeGridTemplate();
                Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption(FKeyNames.F01);
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption(FKeyNames.F02);
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = FuncClear;

            BaseContext.SetFunction03Caption(FKeyNames.F03);
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = PublishInvoices;

            BaseContext.SetFunction07Caption(FKeyNames.F07);
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = Export;

            BaseContext.SetFunction08Caption(FKeyNames.F08);
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption(FKeyNames.F09);
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption(FKeyNames.F10_close);
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Close;
        }
        private void InitializeHandlers()
        {
            tbcBillingSearch.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcBillingSearch.SelectedIndex == 0)
                {
                 
                        BaseContext.SetFunction10Caption(FKeyNames.F10_close);
                        OnF10ClickHandler = Close;
                }
                else
                {
                    BaseContext.SetFunction10Caption(FKeyNames.F10_return);
                    OnF10ClickHandler = Return;
                }
            };
        }
        private void SetFormLoad()
        {
            Settings.SetCheckBoxValue<PC0401>(Login, cbxDepartment);
            Settings.SetCheckBoxValue<PC0401>(Login, cbxCustomer);

            if (ApplicationControl != null)
            {
                //txtDepartmentFrom.MaxLength = ApplicationControl.DepartmentCodeLength;
                //txtDepartmentTo.MaxLength = ApplicationControl.DepartmentCodeLength;
                //txtCustomerFrom.MaxLength = ApplicationControl.CustomerCodeLength;
                //txtCustomerTo.MaxLength = ApplicationControl.CustomerCodeLength;
                var expression = new DataExpression(ApplicationControl);

                txtDepartmentFrom.PaddingChar = expression.DepartmentCodePaddingChar;
                txtDepartmentTo.PaddingChar = expression.DepartmentCodePaddingChar;
                txtCustomerFrom.PaddingChar = expression.CustomerCodePaddingChar;
                txtCustomerTo.PaddingChar = expression.CustomerCodePaddingChar;

                txtDepartmentFrom.Format = expression.DepartmentCodeFormatString;
                txtDepartmentFrom.MaxLength = expression.DepartmentCodeLength;

                txtDepartmentTo.Format = expression.DepartmentCodeFormatString;
                txtDepartmentTo.MaxLength = expression.DepartmentCodeLength;

                txtCustomerFrom.Format = expression.CustomerCodeFormatString;
                txtCustomerFrom.MaxLength = expression.CustomerCodeLength;

                txtCustomerTo.Format = expression.CustomerCodeFormatString;
                txtCustomerTo.MaxLength = expression.CustomerCodeLength;

                txtCustomerFrom.ImeMode = expression.CustomerCodeImeMode();
                txtCustomerTo.ImeMode = expression.CustomerCodeImeMode();
            }
        }

        #endregion

        #region グリッド設定
        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            GridSettingInfo = GetGridSettingList();

            foreach (var gs in GridSettingInfo)
            {
                var cell = new CellSetting(height,
                    gs.DisplayWidth,
                    gs.ColumnName,
                    dataField: gs.ColumnName,
                    caption: gs.ColumnNameJp,
                    sortable: true);


                switch (gs.ColumnName)
                {
                    case nameof(BillingInvoice.Checked):
                        cell.CellInstance = builder.GetCheckBoxCell();
                        cell.ReadOnly = false;
                        cell.Enabled = true;
                        break;

                    case nameof(BillingInvoice.InvoiceTemplateId):
                        cell.CellInstance = CreateDatabindingComboBoxCell(builder.GetComboBoxCell());
                        cell.ReadOnly = false;
                        break;

                    case nameof(BillingInvoice.AmountSum) :
                    case nameof(BillingInvoice.RemainAmountSum):
                        cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                        break;

                    case nameof(BillingInvoice.ClosingAt):
                    case nameof(BillingInvoice.BilledAt):
                        cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                        break;

                    case nameof(BillingInvoice.DetailsCount):
                        cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight);
                        break;

                    case nameof(BillingInvoice.InvoiceCode):
                    case nameof(BillingInvoice.CustomerCode):
                    case nameof(BillingInvoice.DepartmentCode):
                    case nameof(BillingInvoice.StaffCode):
                        cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter);
                        break;

                    case nameof(BillingInvoice.PublishAt):
                    case nameof(BillingInvoice.PublishAt1st):
                        cell.Width = 0;
                        break;

                    case nameof(BillingInvoice.DestnationContent):
                        //送付先コード
                        var cell1 = new CellSetting(height,
                          30,
                          nameof(BillingInvoice.DestnationCode),
                          dataField: nameof(BillingInvoice.DestnationCode),
                          caption: "",
                          sortable: false,
                          enabled: true,
                          readOnly: false,
                          cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter, ime: ImeMode.Disable, format: "9", maxLength: 2));
                        builder.Items.Add(cell1);

                        //送付先検索ボタン
                        var cell2 = new CellSetting(height,
                            30,
                            nameof(BillingInvoice.DestnationButton),
                            dataField: nameof(BillingInvoice.DestnationButton),
                            caption: "",
                            sortable: false,
                            cell: builder.GetButtonCell());
                        builder.Items.Add(cell2);

                        //送付先
                        cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft);
                        break;

                    default:
                        cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft);
                        break;
                }

                builder.Items.Add(cell);
            }

            grid.Template = builder.Build();
            grid.AllowAutoExtend = false;
            grid.FreezeLeftCellName = CellName(nameof( BillingInvoice.Checked));
        }
        private List<GridSetting> GetGridSettingList()
        {
            var list = new List<GridSetting>();
            list.Add(new GridSetting { DisplayWidth = 0, ColumnName = nameof(BillingInvoice.TemporaryBillingInputId), ColumnNameJp = "仮請求書ID" });
            list.Add(new GridSetting { DisplayWidth = 0, ColumnName = nameof(BillingInvoice.BillingInputId), ColumnNameJp = "請求書ID" });
            list.Add(new GridSetting { DisplayWidth = 0, ColumnName = nameof(BillingInvoice.UpdateAt), ColumnNameJp = "更新日時" });
            list.Add(new GridSetting { DisplayWidth = 0, ColumnName = nameof(BillingInvoice.CustomerId)});
            list.Add(new GridSetting { DisplayWidth = 0, ColumnName = nameof(BillingInvoice.DestinationId) });

            var list2 = GetGridSetting();
            list.AddRange(list2);
            return list;
        }
        private ComboBoxCell CreateDatabindingComboBoxCell(ComboBoxCell comboBoxCell)
        {
            var result = SearchInfo();

            TmplateList = new List<InvoiceTemplateSetting>();
            TmplateList.Add(new InvoiceTemplateSetting() { Id = 0, Name = string.Empty });
            if (result != null)
            {

                TmplateList.AddRange(result.ToList());

                var comboSource = TmplateList.Select(c => new { c.Id, c.Name }).ToArray();
                comboBoxCell.DataSource = comboSource;
                comboBoxCell.DisplayMember = "Name";
                comboBoxCell.ValueMember = "Id";
            }

            return comboBoxCell;
        }
        #endregion

        #region ファンクションキー押下処理

        #region F01/照会
        [OperationLog(FKeyNames.F01)]
        private void Search()
        {
            try
            {
                ClearStatusMessage();
                if (!ValidateSearchOptions()) return;
                DisplayInvoices();
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F01);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private bool ValidateSearchOptions()
        {
            if (!datBilledAtFrom.ValidateRange(datBilledAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblBilledAt.Text))) return false;

            if (!txtDepartmentFrom.ValidateRange(txtDepartmentTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDepartment.Text))) return false;

            if (!txtCustomerFrom.ValidateRange(txtCustomerTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomer.Text))) return false;
            return true;
        }
        private void DisplayInvoices()
        {
            ClearGrid();
            var option = GetSearchOption();

            var list = new List<TaskProgress> {
                new TaskProgress($"{ParentForm.Text} 初期化"),
                new TaskProgress($"{ParentForm.Text} 未発行件数照会"),
                new TaskProgress($"{ParentForm.Text} 請求書データ照会"),
            };
            ProgressManager = new TaskProgressManager(list);

            var task = TaskSerchInvoices(option);
            NLogHandler.WriteDebug(this, "請求書データ 照会開始");
            ProgressDialogStart(ParentForm,
                ParentForm.Text,
                task,
                ProgressManager,
                Login,
                AutoCloseProgressDialog,
                true,
                OnCancelHandler);
            NLogHandler.WriteDebug(this, "請求書データ 照会終了");
        }
        private BillingInvoiceSearch GetSearchOption()
        {
            var option = GetSearchOptionBase();
            option.ClosingAt            = datClosingAt.Value;
            option.BilledAtFrom         = datBilledAtFrom.Value;
            option.BilledAtTo           = datBilledAtTo.Value;
            option.CollectCategoryId    = CollectCategoryId;
            option.DepartmentCodeFrom   = txtDepartmentFrom.Text;
            option.DepartmentCodeTo     = txtDepartmentTo.Text;
            option.CustomerCodeFrom     = txtCustomerFrom.Text;
            option.CustomerCodeTo       = txtCustomerTo.Text;
            return option;
        }
        private BillingInvoiceSearch GetSearchOptionBase()
        {
            var reportSetting = GetReportSetting();
            var invoiceReportSetting = new BillingInvoiceReport.BillingInvoiceReportSetting(reportSetting.ReportSettings);

            var option = new BillingInvoiceSearch();
            option.CompanyId = Login.CompanyId;
            option.ClientKey = ClientKey;
            option.IsPublished = false;
            option.ReportId = nameof(PC0401);
            option.ReportInvoiceAmount = (int)invoiceReportSetting.ReportInvoiceAmount;
            return option;
        }
        private List<TaskProgress> GetSearchTasks()
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求書データ照会"));
            return list;
        }

        private async Task TaskSerchInvoices(BillingInvoiceSearch option)
        {
            ProgressManager.UpdateState();
            BillingInvoiceSearchCondition = option;
            var optionForCount = GetSearchOptionBase();
            var countRresut = await GetBillingInvoiceCountAsync(optionForCount);

            if (ProgressManager.Canceled)            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            if (countRresut == null)
            {
                ProgressManager.Abort();
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F01);
                return;
            }
            else if (countRresut.ProcessResult.Result
              && countRresut.Count > 0)
            {
                txtNoPublishCount.Text = countRresut.Count.ToString("#,0");
            }
            else if (countRresut.Count < 1)
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                ProgressManager.UpdateState();
                txtNoPublishCount.Text = "0";
                txtBillingCount.Text = "0";
                txtBillingAmount.Text = "0";
                txtRemainAmount.Text = "0";
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }
            else
            {
                ProgressManager.Abort();
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F01);
                return;
            }

            var source = await GetBillingInvoicesAsync(option);

            if (ProgressManager.Canceled)
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            if (source == null)
            {
                ProgressManager.Abort();
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F01);
                return;
            }
            else if (!source.Any())
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                txtBillingCount.Text = "0";
                txtBillingAmount.Text = "0";
                txtRemainAmount.Text = "0";
                ShowWarningDialog(MsgWngNotExistSearchData);
                SetUnableFunctionKeys();
            }
            else
            {
                tbcBillingSearch.SelectedIndex = 1;
                grid.DataSource = new BindingSource(source, null);

                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction07Enabled(true);
                BaseContext.SetFunction08Enabled(true);
                BaseContext.SetFunction09Enabled(true);

                txtBillingCount.Text = source.Count().ToString("#,0");
                txtBillingAmount.Text = source.Sum(x => x.AmountSum).ToString("#,0");
                txtRemainAmount.Text = source.Sum(x => x.RemainAmountSum).ToString("#,0");
                SetFunctionKeyEnable();
            }
            ProgressManager.UpdateState();
        }
        private void SetUnableFunctionKeys()
        {
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
        }
        #endregion

        #region F02クリア
        [OperationLog(FKeyNames.F02)]
        private void FuncClear()
        {
            ClearStatusMessage();
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void Clear()
        {
            Task.Run(DeleteWorkTableAsync);
            SetUnableFunctionKeys();
            tbcBillingSearch.SelectedIndex = 0;
            ClearGrid();
            ClearControls();
            datClosingAt.Select();
        }
        private void ClearControls()
        {
            datClosingAtDisp.Clear();
            datBilledAtFromDisp.Clear();
            datBilledAtToDisp.Clear();
            datClosingAt.Clear();
            datBilledAtFrom.Clear();
            datBilledAtTo.Clear();
            txtCollectCategoryCode.Clear();
            lblCollectCategoryName.Clear();
            txtDepartmentFrom.Clear();
            txtDepartmentTo.Clear();
            txtCustomerFrom.Clear();
            txtCustomerTo.Clear();
            lblDepartmentNameFrom.Clear();
            lblDepartmentNameTo.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();
            CollectCategoryId = 0;
            BillingInvoiceSearchCondition = null;
            txtRemainAmount.Clear();
            txtBillingAmount.Clear();
            txtBillingCount.Clear();
            txtNoPublishCount.Clear();
        }
        private void ClearGrid()
        {
            grid.DataSource = null;
        }
        #endregion

        #region F03/発行
        [OperationLog(FKeyNames.F03)]
        private void PublishInvoices()
        {
            try
            {
                InputIds_PrintTarget = new List<long>();
                ClearStatusMessage();
                grid.EndEdit();

                if (!grid.Rows.Any(x => (x.DataBoundItem as BillingInvoice).Checked == 1))
                {
                    ShowWarningDialog(MsgWngNotExistUpdateData, "更新を行うデータ");
                    return;
                }

                if (grid.Rows.Any(x => (x.DataBoundItem as BillingInvoice).Checked == 1
                && (x.DataBoundItem as BillingInvoice).InvoiceTemplateId < 1))
                {
                    ShowWarningDialog(MsgWngNotExistUpdateData, "文面パターン");
                    return;
                }

                var dialog = GetInstancePublishDialog();
                ApplicationContext.ShowDialog(this, dialog);

                if (dialog.Result == dlgPublishInvoices.PublishResult.Print)
                {
                    PdfSetting = GetPdfOutputSetting();
                    if (PdfSetting.IsAllInOne)
                        PrintingAllInOne();
                    else
                        PrintingByReport();
                    }
                else if (dialog.Result == dlgPublishInvoices.PublishResult.CSV)
                    OutputCSV();
                else
                    DispStatusMessage(MsgInfProcessCanceled);
                }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F03);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private dlgPublishInvoices GetInstancePublishDialog()
        {
            var dialog = new dlgPublishInvoices();
            dialog.ApplicationContext = ApplicationContext;
            dialog.ApplicationControl = ApplicationControl;
            dialog.CompanyInfo = Company;
            dialog.StartPosition = FormStartPosition.CenterScreen;
            return dialog;
        }

        #region 発行（PDF）
        private async void PrintingAllInOne()
        {
            try
            {
                if (!ShowConfirmDialog(MsgQstConfirmStartXXX, FKeyNames.F03))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                Report = null;
                ProgressManager = new TaskProgressManager(GetBaseProgress());
                var task = PrintTaskAllInOne();
                var dialogResult = ProgressDialogStart(ParentForm,
                    ParentForm.Text + " 発行",
                    task,
                    ProgressManager,
                    Login,
                    AutoCloseProgressDialog,
                    true,
                    OnCancelHandler);

                if (task.Result == TaskResult.Modified)
                {
                    ShowModifiedMessage();
                    DisplayInvoices();
                    return;
                }

                if (dialogResult != DialogResult.OK
                    || task.Result != TaskResult.Success
                    || Report == null)
                {
                    if (task.Result == TaskResult.cancel)
                    {
                    DispStatusMessage(MsgInfProcessCanceled);
                }
                    else
                {
                        ShowWarningDialog(MsgErrSomethingError, FKeyNames.F03);
                }
                    return;
                }

                    var serverPath = GetServerPath();
                    ShowDialogPreview(ParentForm, Report, serverPath);

                DisplayInvoices();
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F03);
                await CancelPublishAsync();
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private async void PrintingByReport()
        {
            try
            {
                Report = null;
                var selectedPath = string.Empty;
                var rootBrowserPath = new List<string>();
                if (!LimitAccessFolder ?
                    !ShowFolderBrowserDialog("", out selectedPath) :
                    !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmStartXXX, FKeyNames.F03))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var list = GetBaseProgress();
                list.Add(new TaskProgress($"{ParentForm.Text} 請求書データ照会"));
                ProgressManager = new TaskProgressManager(list);
                var path = (!LimitAccessFolder) ? selectedPath : rootBrowserPath.FirstOrDefault();
                var task = PrintTaskByReport(path);
                var dialogResult = ProgressDialogStart(ParentForm,
                    ParentForm.Text + " 発行",
                    task,
                    ProgressManager,
                    Login,
                    AutoCloseProgressDialog,
                    false,
                    OnCancelHandler);

                if (task.Result == TaskResult.Modified)
                {
                    ShowModifiedMessage();
                    DisplayInvoices();
                    return;
                }
                else if (task.Result == TaskResult.Error)
                {
                    ShowWarningDialog(MsgErrSomethingError, FKeyNames.F03);
                    return;
                }

                if (dialogResult != DialogResult.OK)
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, FKeyNames.F03);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                await CancelPublishAsync();
            }
        }
        private async Task<TaskResult> PrintTaskAllInOne()
        {
            try
            {
                var result = await BuildAndUpdateBillingSource();

                if (ProgressManager.Canceled)
                {
                    await CancelPublishAsync();
                    return TaskResult.cancel;
                }

                if (result != TaskResult.Success)
                {
                    await CancelPublishAsync();
                    return result;
                }
                Report = await CreateReport();

                if (ProgressManager.Canceled)
                    return TaskResult.cancel;

                return TaskResult.Success;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ProgressManager.Abort();
                await CancelPublishAsync();
                return TaskResult.Error;
            }
        }
        private async Task<TaskResult> PrintTaskByReport(string path)
        {
            try
            {
                var result = await BuildAndUpdateBillingSource(); // upatestate 1, 2
                if (result != TaskResult.Success)
                {
                    await CancelPublishAsync();
                    return result;
                }

                var fileList = new List<string>();

                foreach (var id in InputIds_PrintTarget)
                {
                    var option = new BillingInvoiceDetailSearch();
                    option.CompanyId = Login.CompanyId;
                    option.BillingInputIds = new long[] { id };
                    using (var report = await CreateReport(option, false))
                    {
                        var exporter = new PdfReportExporter();
                        var reportName = PdfSetting.FileName.Replace("[CODE]", report.CustomerCode);
                        reportName = reportName.Replace("[NAME]", report.CustomerName);
                        reportName = reportName.Replace("[NO]", report.InvoiceCode);
                        reportName = reportName.Replace("[DATE]", report.InvoiceDate.ToString("yyyyMMdd"));

                        var fullPath = Util.GetUniqueFileName($"{path}\\{reportName}.pdf");
                        exporter.PdfExport(report, fullPath);
                        if (PdfSetting.UseZip) fileList.Add(fullPath);
                    }
                }

                if (PdfSetting.UseZip)
                    Util.ArchivesAsZip(fileList, path, $"請求書{DateTime.Now.ToString("yyyyMMdd")}", PdfSetting.MaximumByte);
                ProgressManager.UpdateState(); // updatestate 3

                ClearGrid();
                await TaskSerchInvoices(GetSearchOption()); // updatestate 4,5,6
                return TaskResult.Success;
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ProgressManager.Abort();
                await CancelPublishAsync();
                return TaskResult.Error;
            }
        }
        private async Task<TaskResult> BuildAndUpdateBillingSource()
        {
            var targetInvoices = new List<BillingInvoiceForPublish>();
            foreach (var row in grid.Rows)
            {
                var isChecked = Convert.ToBoolean(row.Cells[CellName(nameof(BillingInvoice.Checked))].Value);
                if (isChecked)
                {
                    var invoice = row.DataBoundItem as BillingInvoice;
                    var invoiceTemplateFixedString = TmplateList.Where(x => x.Id == invoice.InvoiceTemplateId).FirstOrDefault().FixedString;

                    targetInvoices.Add(new BillingInvoiceForPublish
                    {
                        CompanyId                   = Login.CompanyId,
                        ClientKey                   = ClientKey,
                        TemporaryBillingInputId     = invoice.TemporaryBillingInputId,
                        BillingInputId              = invoice.BillingInputId,
                        InvoiceCode                 = invoice.InvoiceCode,
                        BilledAt                    = invoice.BilledAt,
                        ClosingAt                   = invoice.ClosingAt,
                        InvoiceTemplateId           = invoice.InvoiceTemplateId,
                        InvoiceTemplateFixedString  = invoiceTemplateFixedString,
                        UpdateAt                    = invoice.UpdateAt,
                        DestinationId               = invoice.DestinationId
                    });
                }
            }
            
            var result = await PublishInvoicesAsync(ClientKey, targetInvoices.ToArray());

            if (result != null
                && result.ProcessResult != null
                && result.ProcessResult.ErrorCode == ErrorCode.OtherUserAlreadyUpdated)
            {
                ProgressManager.Abort();
                return TaskResult.Modified;
            }
            else if (result == null
                || result.ProcessResult == null
                || !result.ProcessResult.Result)
            {
                ProgressManager.Abort();
                return TaskResult.Error;
            }

                InputIds_PrintTarget.AddRange(result.BillingInputIds.ToList());
                ProgressManager.UpdateState();
                return TaskResult.Success;
            }
        private async Task<BillingInvoiceReport> CreateReport(
            BillingInvoiceDetailSearch billingInvoiceDetailSearch = null,
            bool doProgressUpdate = true
            )
        {
            //請求書データ取得
            BillingInvoiceDetailSearch option;
            if (billingInvoiceDetailSearch == null)
            {
                option = new BillingInvoiceDetailSearch();
                option.CompanyId = Login.CompanyId;
                option.BillingInputIds = InputIds_PrintTarget.ToArray();
            }
            else
            {
                option = billingInvoiceDetailSearch;
            }

            //請求書作成
            var reportSetting = GetReportSetting();
            if (reportSetting == null)
            {
                ProgressManager.Abort();
            }
            var report = new BillingInvoiceReport(true);

            report.Company = Company;
            report.CompanyLogos = CompanyLogos;
            report.Initialize(reportSetting.ReportSettings);
            report.lblNote1.Text = Note1Name;

            var reportSource = await GetBillingInvoicesDetailsForPrintAsync(option);
            report.CustomerCode = reportSource.FirstOrDefault().CustomerCode;
            report.CustomerName = reportSource.FirstOrDefault().CustomerName;
            report.InvoiceCode = reportSource.FirstOrDefault().InvoiceCode;
            if (report.Setting.ReportInvoiceDate == BillingInvoiceReport.ReportInvoiceDate.BilledAt)
            {
                report.InvoiceDate = reportSource.FirstOrDefault().BilledAt;
            }
            else if (report.Setting.ReportInvoiceDate == BillingInvoiceReport.ReportInvoiceDate.ClosingAt)
            {
                report.InvoiceDate = reportSource.FirstOrDefault().ClosingAt;
            }

            var invoiceDetails = report.BuildDataSource(reportSource, Company);
            report.DataSource = invoiceDetails;

            if(doProgressUpdate) ProgressManager.UpdateState();

            BillingInvoiceReport report2 = null;
            if (report.Setting.OutputCopy == ReportDoOrNot.Do)
            {
                report2 = new BillingInvoiceReport(true, true);
                report2.Company = Company;
                report2.CompanyLogos = CompanyLogos;
                report2.Initialize(reportSetting.ReportSettings);
                report2.lblNote1.Text = Note1Name;
                report2.DataSource = invoiceDetails;
            }

            report.Run(false);
            if (report2 != null)
            {
                report2.Run(false);
                report.Document.Pages.AddRange((PagesCollection)report2.Document.Pages.Clone());
            }
            if (doProgressUpdate) ProgressManager.UpdateState();
            return report;
        }
        private List<TaskProgress> GetBaseProgress()
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求書データ取得"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求書の作成"));
            return list;
        }
        #endregion

        #region 発行(CSV)
        private async void OutputCSV()
        {
            try
            {
                ClearStatusMessage();
                var pathMatching = string.Empty;
                var pathReceipt = string.Empty;
                ListBillingInvoiceDetailForExport = null;
                if (!ShowSaveFileDialog(GetServerPath(),
                    $"請求書明細データ_{DateTime.Now:yyyyMMdd}.csv",
                    out pathMatching,
                    cancellationMessaging: () => DispStatusMessage(MsgInfProcessCanceled))) return;
                if (!ShowConfirmDialog(MsgQstConfirmStartXXX, "CSV出力"))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var result = OutputInvoiceDetails(pathMatching);

                if (result == TaskResult.Error)
                {
                    ShowWarningDialog(MsgErrSomethingError, "出力");
                }
                else if (result == TaskResult.Modified)
                {
                    ShowModifiedMessage();
                    DisplayInvoices();
                }
                else if (result == TaskResult.Success)
                {
                    DisplayInvoices();
                    DispStatusMessage(MsgInfFinishExport);
                }
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "出力");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                await CancelPublishAsync();
            }
        }
        private TaskResult OutputInvoiceDetails(string path)
        {
            var task = GetSourceForOutput();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (task.Result != TaskResult.Success)
                return task.Result;

            ExportInvoiceDetailsData(path);
            return TaskResult.Success;
        }
        private async Task<TaskResult> GetSourceForOutput()
        {
            try
            {
            var result = await BuildAndUpdateBillingSource();
            if (result != TaskResult.Success)
            {
                    await CancelPublishAsync();
                return result;
            }
            ListBillingInvoiceDetailForExport = await GetDetailsForExport(InputIds_PrintTarget.ToArray());
            return TaskResult.Success;
        }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                await CancelPublishAsync();
                return TaskResult.Error;
            }
        }
        private bool ExportInvoiceDetailsData(string path)
        {

            var gridsetting = Task.Run(GetExportFieldSettingAsync).Result;
            var definition = new BillingInvoiceDetailFileDifinition(new DataExpression(ApplicationControl), gridsetting);
            var decimalFormat = "0";

            if (definition.BillingAmountField != null)
                definition.BillingAmountField.Format = value => value.ToString(decimalFormat);
            if (definition.RemainAmountField != null)
                definition.RemainAmountField.Format = value => value.ToString(decimalFormat);
            if (definition.TaxAmountField != null)
                definition.TaxAmountField.Format = value => value.ToString(decimalFormat);
            if (definition.PriceField != null)
                definition.PriceField.Format = value => value.ToString(decimalFormat);
            if (definition.QuantityField != null)
                definition.QuantityField.Format = value => value.ToString(decimalFormat);
            if (definition.UnitPriceField != null)
                definition.UnitPriceField.Format = value => value.ToString(decimalFormat);

            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            ProgressDialog.Start(ParentForm, (cancel, progress) =>
            {
               return exporter.ExportAsync(path, ListBillingInvoiceDetailForExport, cancel, progress);
            }, true, SessionKey);

            if (exporter.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                ShowWarningDialog(MsgErrExportError);
                return false;
            }
            return true;
        }
        #endregion

        #endregion

        #region F07/エクスポート
        [OperationLog(FKeyNames.F07)]
        private void Export()
        {
            try
            {
                var serverPath = GetServerPath();

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"請求書データ{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var exportBillingInvoice = ((IEnumerable)grid.DataSource).Cast<BillingInvoice>().ToList();
                var definition = new BillingInvoiceFileDifinition(new DataExpression(ApplicationControl), GridSettingInfo);

                var decimalFormat = "0";

                definition.AmountSumField.Format = value => value.ToString(decimalFormat);
                definition.RemainAmountSumField.Format = value => value.ToString(decimalFormat);

                definition.SetFieldsSetting(GridSettingInfo, definition.ConvertSettingToField);

                definition.PublishAtField.Ignored = true;
                definition.PublishAt1stField.Ignored = true;

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
        #endregion

        #region F08/全選択＆F09/全解除
        [OperationLog(FKeyNames.F08)]
        private void SelectAll()
        {
            grid.EndEdit();
            grid.Focus();
            foreach (var row in grid.Rows)
            {
                var receipt = row.DataBoundItem as BillingInvoice;
                if (row.Cells[CellName(nameof(BillingInvoice.Checked))].Enabled)
                {
                    receipt.Checked = 1;
                    //SetModifiedRow(row.Index);
                }
            }
            SetFunctionKeyEnable();
            //SetFunctionKeysEnabled();
            //CalculateSum();
        }

        [OperationLog(FKeyNames.F09)]
        private void DeselectAll()
        {
            grid.EndEdit();
            grid.Focus();
            foreach (var row in grid.Rows)
            {
                var receipt = row.DataBoundItem as BillingInvoice;
                if (row.Cells[CellName(nameof(BillingInvoice.Checked))].Enabled)
                {
                    receipt.Checked = 0;
                    //SetModifiedRow(row.Index);
                }
            }
            SetFunctionKeyEnable();
            //SetFunctionKeysEnabled();
            //CalculateSum();
        }
        #endregion

        #region F10/戻る＆F10/終了
        [OperationLog(FKeyNames.F10_return)]
        private void Return()
        {
            tbcBillingSearch.SelectedIndex = 0;
        }

        [OperationLog(FKeyNames.F10_close)]
        private void Close()
        {
            Task.Run(DeleteWorkTableAsync);
            ExitBilling();
        }

        private void ExitBilling()
        {
            try
            {
                Settings.SaveControlValue<PC0401>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PC0401>(Login, cbxCustomer.Name, cbxCustomer.Checked);
                BaseForm.Close();
                return;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #endregion

        #region Webサービス呼び出し
        private async Task<List<BillingInvoice>> GetBillingInvoicesAsync(BillingInvoiceSearch searchOption)
        {
            List<BillingInvoice> result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<BillingInvoiceServiceClient>(async client =>
                {
                    var getResult = await client.GetAsync(SessionKey, searchOption, hubConnection.ConnectionId);
                    if (getResult.ProcessResult.Result)
                        result = getResult.BillingInvoices;
                });
            }
            return result;
        }
        private async Task<BillingInputResult> PublishInvoicesAsync(byte[] clientKey,BillingInvoiceForPublish[] billingInvoiceThin)
        {
            BillingInputResult result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<BillingInvoiceServiceClient>(async client =>
                {
                   result = await client.PublishInvoicesAsync(SessionKey, hubConnection.ConnectionId, billingInvoiceThin, Login.UserId);
                   
                });
            }
            return result;
        }
        private async Task<List<BillingInvoiceDetailForPrint>> GetBillingInvoicesDetailsForPrintAsync(BillingInvoiceDetailSearch billingInvoiceDetailSearch) =>
           await ServiceProxyFactory.DoAsync(async (BillingInvoiceServiceClient client) =>
        {
            var result = await client.GetDetailsForPrintAsync(SessionKey, billingInvoiceDetailSearch);
            if(result.ProcessResult.Result) return result.BillingInvoicesDetails;
            return null;
        });
        private ReportSettingsResult GetReportSetting()
        {
            ReportSettingsResult result = null;
            ServiceProxyFactory.LifeTime(factory =>
            {
                var client = factory.Create<ReportSettingMasterClient>();
                var getResult = client.GetItems(
                       SessionKey, CompanyId, nameof(PC0401));
                if (getResult.ProcessResult.Result)
                {
                    result = getResult;
                }

            });
            return result;
        }
        private async Task LoadColumnNameSettingAsync()
        {
            await ServiceProxyFactory.DoAsync<ColumnNameSettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                   var BillingColumnNameInfo = result.ColumnNames.Where(x => x.TableName == nameof(Billing)).ToList();

                    
                    foreach (ColumnNameSetting cm in BillingColumnNameInfo)
                    {
                        if (cm.ColumnName == "Note1")
                        {
                            Note1Name = string.IsNullOrEmpty(cm.Alias) ? cm.OriginalName : cm.Alias;
                        }
                    }

                }
            });
        }
        private async Task LoadCompanyLogosAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CompanyMasterClient>();
                var result = await service.GetLogosAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    CompanyLogos = result.CompanyLogos;
                }
            });
        }
        private IEnumerable<InvoiceTemplateSetting> SearchInfo()
        {
            InvoiceTemplateSettingsResult result = null;
            ServiceProxyFactory.LifeTime(factory =>
            {
                var client = factory.Create<InvoiceSettingServiceClient>();
                result = client.GetInvoiceTemplateSettings(SessionKey, Login.CompanyId);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.InvoiceTemplateSettings;
        }
        private async Task<CountResult> GetBillingInvoiceCountAsync(BillingInvoiceSearch searchOption)
        {
            CountResult result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<BillingInvoiceServiceClient>(async client =>
                {
                    result = await client.GetCountAsync(SessionKey, searchOption, hubConnection.ConnectionId);
                });
            }
            return result;
        }
        private async Task DeleteWorkTableAsync()
        {
            await ServiceProxyFactory.DoAsync<BillingInvoiceServiceClient>(async client =>
            {
                var result = await client.DeleteWorkTableAsync(SessionKey, ClientKey);
            });
        }
        private async Task LoadClientKeyAsync()
            => ClientKey = await Util.CreateClientKey(Login, nameof(PC0401));
        private async Task<List<ExportFieldSetting>> GetExportFieldSettingAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (ExportFieldSettingMasterClient client)
                => await client.GetItemsByExportFileTypeAsync(SessionKey, CompanyId, (int)CsvExportFileType.PublishInvoiceData));
            if (result.ProcessResult.Result)
                return result.ExportFieldSettings;
            return new List<ExportFieldSetting>();
        }
        private async Task<List<BillingInvoiceDetailForExport>> GetDetailsForExport(long[] BillingInputIds)
        {
            List<BillingInvoiceDetailForExport> result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<BillingInvoiceServiceClient>(async client =>
                {
                    var getResult = await client.GetDetailsForExportAsync(SessionKey, BillingInputIds,Login.CompanyId, hubConnection.ConnectionId);
                    if (getResult.ProcessResult.Result)
                        result = getResult.BillingInvoicesDetails;
                });
            }
            return result;
        }
        private List<GridSetting> GetGridSetting()
        {
            var list = new List<GridSetting>();

            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GridSettingMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId, Login.UserId, GridId.BillingInvoicePublish);

                if (result.ProcessResult.Result)
                {
                    list = result.GridSettings;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return list;
        }
        private async Task<CountResult> CancelPublishAsync()
        {
            if (InputIds_PrintTarget == null
                || InputIds_PrintTarget.Count < 1)
                return null;

            CountResult result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<BillingInvoiceServiceClient>(async client =>
                {
                    result = await client.CancelPublishAsync(SessionKey, hubConnection.ConnectionId, InputIds_PrintTarget.ToArray());
                });
            }

            if (result.ProcessResult.Result)
                InputIds_PrintTarget = null;

            return result;
        }
        private async Task<List<Destination>> GetDestinationsAsync(int customerId, string code = null) =>
          await ServiceProxyFactory.DoAsync(async (DestinationMasterClient client) => {
              var option = new DestinationSearch
              {
                  CompanyId = CompanyId,
                  CustomerId = customerId,
              };
              if (!string.IsNullOrEmpty(code)) option.Codes = new[] { code };
              var result = await client.GetItemsAsync(SessionKey, option);
              if (result.ProcessResult.Result)
                  return result.Destinations;
              return new List<Destination>();
          });
        private async Task<Customer> GetCustomerAsync(int id) =>
          await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
          {
              var result = await client.GetAsync(SessionKey, new int[] { id });
              if (result.ProcessResult.Result)
                  return result.Customers.FirstOrDefault();
              return null;
          });
        private PdfOutputSetting GetPdfOutputSetting() =>
      ServiceProxyFactory.Do((PdfOutputSettingMasterClient client) =>
     {
         var result = client.Get(
               SessionKey,
               Login.CompanyId,
               (int)PdfOutputSettingReportType.Invoice,
               Login.UserId);

         if (result == null || result.ProcessResult == null || !result.ProcessResult.Result)
             return null;
         return result.PdfOutputSetting;
     });
        #endregion

        #region イベントハンドラー

        #region グリッドイベント
        private void grdSearchResult_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellName != CellName(nameof(BillingInvoice.Checked))) return;
            grid.EndEdit();
        }
        private void grdSearchResult_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellName == CellName(nameof(BillingInvoice.DestnationCode)))
            {
                var invoice = grid.Rows[e.RowIndex].DataBoundItem as BillingInvoice;

                if (string.IsNullOrEmpty(invoice.DestnationCode))
                {
                    invoice.SetDestination();
                    return;
                }
                var task = GetDestinationsAsync(invoice.CustomerId, invoice.DestnationCode);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var destination = task.Result.FirstOrDefault();
                if (destination == null)
                {
                    var inputCode = invoice.DestnationCode;
                    invoice.SetDestination();

                    ShowWarningDialog(MsgWngMasterNotExist, "送付先", inputCode);
                    return;
                }
                invoice.SetDestination(destination);
                grid.DataSource = grid.DataSource;
            }
            if (e.CellName == CellName(nameof(BillingInvoice.Checked))) SetFunctionKeyEnable();
        }
        private Customer GetCustomer(int customerId)
        {
            var task = GetCustomerAsync(customerId);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        private void grid_DataError(object sender, DataErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }
        private void grid_CellDoubleClick(object sender, CellEventArgs e)
        {
            try
            {
                ClearStatusMessage();
                grid.EndEdit();
                if (e.RowIndex < 0
                    || e.CellName == CellName(nameof(BillingInvoice.Checked))
                    || e.CellName == CellName(nameof(BillingInvoice.DestnationCode))
                    || e.CellName == CellName(nameof(BillingInvoice.DestnationButton))) return;

                var invoice = grid.Rows[e.RowIndex].DataBoundItem as BillingInvoice;

                if (invoice.InvoiceTemplateId < 1)
                {
                    ShowWarningDialog(MsgWngNotExistUpdateData, "文面パターン");
                    return;
                }
                ProgressDialog.Start(ParentForm, async (cancel, progress) =>
                {
                    var Ids = new List<long>();
                    var id = (long)grid.GetValue(e.RowIndex, CellName(nameof(BillingInvoice.TemporaryBillingInputId)));
                    Ids.Add(id);

                    var billingInvoiceDetailSearch = new BillingInvoiceDetailSearch();
                    billingInvoiceDetailSearch.CompanyId = Login.CompanyId;
                    billingInvoiceDetailSearch.TemporaryBillingInputIds = Ids.ToArray();
                    billingInvoiceDetailSearch.ClientKey = ClientKey;
                    billingInvoiceDetailSearch.InvoiceTemplateId = invoice.InvoiceTemplateId;
                    if (invoice.DestinationId.HasValue) billingInvoiceDetailSearch.DestinationId = invoice.DestinationId.Value;
                    var report = await CreateReport(billingInvoiceDetailSearch);

                    ShowDialogPreview(ParentForm, report, GetServerPath());
                }, false, SessionKey);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void grid_CellContentButtonClick(object sender, CellEventArgs e)
        {
            if (e.CellName == CellName(nameof(BillingInvoice.DestnationButton)))
            {
                var customerId = (int)grid.GetValue(e.RowIndex, CellName(nameof(BillingInvoice.CustomerId)));
                ClearStatusMessage();
                var destination = this.ShowDestinationSearchDialog(customerId);
                if (destination == null) return;
                var invoice = grid.Rows[e.RowIndex].DataBoundItem as BillingInvoice;
                invoice.SetDestination(destination);
            }
        }

        #endregion
        private void SetFunctionKeyEnable()
        {
            var isCheckedForCreateInvoice = grid.Rows.Any(x => Convert.ToBoolean(x[CellName(nameof(BillingInvoice.Checked))].EditedFormattedValue));
            BaseContext.SetFunction03Enabled(isCheckedForCreateInvoice);
        }
        private void imdClosingAt_Validated(object sender, EventArgs e)
        {
            datClosingAtDisp.Value = datClosingAt.Value;
        }
        private void datBilledAtFrom_Validated(object sender, EventArgs e)
        {
            datBilledAtFromDisp.Value = datBilledAtFrom.Value;
        }
        private void datBilledAtTo_Validated(object sender, EventArgs e)
        {
            datBilledAtToDisp.Value = datBilledAtTo.Value;
        }
        private void txtDepartmentFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                var departmentResult = new Department();

                var departmentCodeFrom = txtDepartmentFrom.Text;
                var departmentNameTo = string.Empty;
                var departmentNameFrom = string.Empty;

                if (!string.IsNullOrEmpty(departmentCodeFrom))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { departmentCodeFrom });

                        departmentResult = result.Departments.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            departmentCodeFrom = departmentResult.Code;
                            departmentNameFrom = departmentResult.Name;
                            departmentNameTo = departmentResult.Name;
                            ClearStatusMessage();
                        }

                        txtDepartmentFrom.Text = departmentCodeFrom;
                        lblDepartmentNameFrom.Text = departmentNameFrom;

                        if (cbxDepartment.Checked)
                        {
                            txtDepartmentTo.Text = departmentCodeFrom;
                            lblDepartmentNameTo.Text = departmentNameTo;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    if (cbxDepartment.Checked)
                    {
                        txtDepartmentTo.Text = departmentCodeFrom;
                        lblDepartmentNameTo.Clear();
                    }
                    lblDepartmentNameFrom.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void txtDepartmentTo_Validated(object sender, EventArgs e)
        {
            try
            {
                var departmentResult = new Department();

                var departmentCodeTo = txtDepartmentTo.Text;
                var departmentNameTo = string.Empty;

                if (!string.IsNullOrEmpty(departmentCodeTo))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { departmentCodeTo });

                        departmentResult = result.Departments.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            departmentCodeTo = departmentResult.Code;
                            departmentNameTo = departmentResult.Name;
                            ClearStatusMessage();
                        }
                        txtDepartmentTo.Text = departmentCodeTo;
                        lblDepartmentNameTo.Text = departmentNameTo;
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtDepartmentTo.Text = departmentCodeTo;
                    lblDepartmentNameTo.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void txtCustomerFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                SetCustomerFromData(txtCustomerFrom.Text);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void txtCustomerTo_Validated(object sender, EventArgs e)
        {
            try
            {
                SetCustomerToData(txtCustomerTo.Text);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void SetCustomerToData(string customerCodeTo)
        {
            var customerResult = new Customer();

            var customerNameTo = string.Empty;

            if (!string.IsNullOrEmpty(customerCodeTo))
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CustomerMasterClient>();
                    var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { customerCodeTo });

                    customerResult = result.Customers.FirstOrDefault();

                    if (customerResult != null)
                    {
                        customerCodeTo = customerResult.Code;
                        customerNameTo = customerResult.Name;
                        ClearStatusMessage();
                    }
                    txtCustomerTo.Text = customerCodeTo;
                    lblCustomerNameTo.Text = customerNameTo;
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            else
            {
                txtCustomerTo.Text = customerCodeTo;
                lblCustomerNameTo.Clear();
                ClearStatusMessage();
            }
        }
        private void SetCustomerFromData(string customerCodeFrom)
        {
            var customerResult = new Customer();
            var customerNameTo = string.Empty;
            var customerNameFrom = string.Empty;

            if (!string.IsNullOrEmpty(customerCodeFrom))
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CustomerMasterClient>();
                    var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { customerCodeFrom });

                    customerResult = result.Customers.FirstOrDefault();

                    if (customerResult != null)
                    {
                        customerCodeFrom = customerResult.Code;
                        customerNameFrom = customerResult.Name;
                        customerNameTo = customerResult.Name;
                        ClearStatusMessage();
                    }

                    txtCustomerFrom.Text = customerCodeFrom;
                    lblCustomerNameFrom.Text = customerNameFrom;

                    if (cbxCustomer.Checked)
                    {
                        txtCustomerTo.Text = customerCodeFrom;
                        lblCustomerNameTo.Text = customerNameFrom;
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            else
            {
                if (cbxCustomer.Checked)
                {
                    txtCustomerTo.Text = customerCodeFrom;
                    lblCustomerNameTo.Clear();
                }
                lblCustomerNameFrom.Clear();
                ClearStatusMessage();
            }
        }
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                if (sender == btnCustomerFrom)
                {
                    txtCustomerFrom.Text = customer.Code;
                    lblCustomerNameFrom.Text = customer.Name;
                    if (cbxCustomer.Checked)
                    {
                        txtCustomerTo.Text = customer.Code;
                        lblCustomerNameTo.Text = customer.Name;
                    }
                }
                else
                {
                    txtCustomerTo.Text = customer.Code;
                    lblCustomerNameTo.Text = customer.Name;
                }
                ClearStatusMessage();
            }
        }
        private void btnDepartment_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                if (sender == btnDepartmentFrom)
                {
                    txtDepartmentFrom.Text = department.Code;
                    lblDepartmentNameFrom.Text = department.Name;
                    if (cbxDepartment.Checked)
                    {
                        txtDepartmentTo.Text = department.Code;
                        lblDepartmentNameTo.Text = department.Name;
                    }
                }
                else
                {
                    txtDepartmentTo.Text = department.Code;
                    lblDepartmentNameTo.Text = department.Name;
                }
                ClearStatusMessage();
            }
        }
        private void txtCollectCategoryCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var categoryResult = new Category();

                if (!string.IsNullOrEmpty(txtCollectCategoryCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CategoryMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Collect,
                            new[] { txtCollectCategoryCode.Text });

                        categoryResult = result.Categories.FirstOrDefault();
                        if (categoryResult != null)
                        {
                            txtCollectCategoryCode.Text = categoryResult.Code;
                            lblCollectCategoryName.Text = categoryResult.Name;
                            CollectCategoryId = categoryResult.Id;
                            ClearStatusMessage();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (categoryResult == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "回収区分", txtCollectCategoryCode.Text);
                        txtCollectCategoryCode.Clear();
                        lblCollectCategoryName.Clear();
                        CollectCategoryId = 0;
                        txtCollectCategoryCode.Focus();
                    }
                }
                else
                {
                    ClearStatusMessage();
                    lblCollectCategoryName.Clear();
                    CollectCategoryId = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void btnCollectCategory_Click(object sender, EventArgs e)
        {
            var collectCategory = this.ShowCollectCategroySearchDialog();
            if (collectCategory != null)
            {
                txtCollectCategoryCode.Text = collectCategory.Code;
                lblCollectCategoryName.Text = collectCategory.Name;
                CollectCategoryId = collectCategory.Id;
                ClearStatusMessage();
            }
        }
        #endregion

        #region Common
        private void ShowModifiedMessage()
        {
            MessageBox.Show(@"他ユーザーにより請求データが変更されているため、処理をキャンセルしました。
            請求書データを再取得します。",
                "",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
    }
        #endregion
}
}
