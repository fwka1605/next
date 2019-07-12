using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document.Section;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingInvoiceService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CompanyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ExportFieldSettingMasterService;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.GridSettingMasterService;
using Rac.VOne.Client.Screen.InvoiceSettingService;
using Rac.VOne.Client.Screen.PdfOutputSettingMasterService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
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
    /// <summary>
    /// 請求書再発行
    /// </summary>
    public partial class PC1401 : VOneScreenBase
    {
        #region メンバー＆コンストラクター
        private int Precision { get; set; }
        private List<GridSetting> GridSettingInfo { get; set; }
        private List<InvoiceTemplateSetting> TmplateList { get; set; }
        private List<CompanyLogo> CompanyLogos { get; set; }
        private string Note1Name { get; set; }
        private byte[] ClientKey { get; set; }
        private TaskProgressManager ProgressManager;
        private BillingInvoiceSearch BillingInvoiceSearchCondition { get; set; }
        private System.Action OnCancelHandler { get; set; }
        private string CellName(string value) => $"cel{value}";
        private int CollectCategoryId { get; set; }
        private long[] PublishTargetIds { get; set; }
        private PdfOutputSetting PdfSetting { get; set; }
        public PC1401()
        {
            InitializeComponent();
            Text = "請求書再発行";
            Precision = UseForeignCurrency ? Precision : 0;
        }
        #endregion

        #region 初期化
        private void PC1401_Load(object sender, EventArgs e)
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
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadClientKeyAsync());
                tasks.Add(LoadColumnNameSettingAsync());
                tasks.Add(LoadCompanyLogosAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks.ToArray()), false, SessionKey);

                SetScreenName();
                grid.SetupShortcutKeys();
                InitializeHandlers();
                InitializeGridTemplate();
                SetFormLoad();
                Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void InitializeHandlers()
        {
            tbcBillingSearch.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcBillingSearch.SelectedIndex == 0)
                {

                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = Close;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = Return;
                }
            };
        }
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("照会");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = FuncClear;

            BaseContext.SetFunction03Caption("再発行");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = RePublishInvoices;

            BaseContext.SetFunction04Caption("発行取消");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = CancelPiblish;

            BaseContext.SetFunction05Enabled(false);

            BaseContext.SetFunction06Enabled(false);

            BaseContext.SetFunction07Caption("エクスポート");
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = Export;

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Close;
        }
        private void SetFormLoad()
        {
            Settings.SetCheckBoxValue<PC1401>(Login, cbxDepartment);
            Settings.SetCheckBoxValue<PC1401>(Login, cbxCustomer);
            Settings.SetCheckBoxValue<PC1401>(Login, cbxStaff);
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);

                txtStaffFrom.PaddingChar = expression.StaffCodePaddingChar;
                txtStaffTo.PaddingChar = expression.StaffCodePaddingChar;
                txtDepartmentFrom.PaddingChar = expression.DepartmentCodePaddingChar;
                txtDepartmentTo.PaddingChar = expression.DepartmentCodePaddingChar;
                txtCustomerFrom.PaddingChar = expression.CustomerCodePaddingChar;
                txtCustomerTo.PaddingChar = expression.CustomerCodePaddingChar;

                txtDepartmentFrom.Format = expression.DepartmentCodeFormatString;
                txtDepartmentFrom.MaxLength = expression.DepartmentCodeLength;

                txtDepartmentTo.Format = expression.DepartmentCodeFormatString;
                txtDepartmentTo.MaxLength = expression.DepartmentCodeLength;

                txtStaffFrom.Format = expression.StaffCodeFormatString;
                txtStaffFrom.MaxLength = expression.StaffCodeLength;

                txtStaffTo.Format = expression.StaffCodeFormatString;
                txtStaffTo.MaxLength = expression.StaffCodeLength;

                txtCustomerFrom.Format = expression.CustomerCodeFormatString;
                txtCustomerFrom.MaxLength = expression.CustomerCodeLength;

                txtCustomerTo.Format = expression.CustomerCodeFormatString;
                txtCustomerTo.MaxLength = expression.CustomerCodeLength;

                txtCustomerFrom.ImeMode = expression.CustomerCodeImeMode();
                txtCustomerTo.ImeMode = expression.CustomerCodeImeMode();
            }
        }

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
                        cell.ReadOnly = true;
                        cell.Enabled = false;
                        break;

                    case nameof(BillingInvoice.AmountSum):
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

                    case nameof(BillingInvoice.DestnationContent):
                        //送付先コード
                        var cell1 = new CellSetting(height,
                            30,
                            nameof(BillingInvoice.DestnationCode),
                            dataField: nameof(BillingInvoice.DestnationCode),
                            caption: "",
                            sortable: false,
                            cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter));
                        builder.Items.Add(cell1);

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
            grid.FreezeLeftCellName = CellName(nameof(BillingInvoice.Checked));
        }
        private List<GridSetting> GetGridSettingList()
        {
            var list = new List<GridSetting>();
            list.Add(new GridSetting { DisplayWidth = 0, ColumnName = nameof(BillingInvoice.TemporaryBillingInputId), ColumnNameJp = "仮請求書ID" });
            list.Add(new GridSetting { DisplayWidth = 0, ColumnName = nameof(BillingInvoice.BillingInputId), ColumnNameJp = "請求書ID" });

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

        #endregion

        #region ファンクションキー押下処理
        #region 照会
        [OperationLog("照会")]
        private void Search()
        {
            ClearStatusMessage();
            try
            {
                if (!cbxFullAssignment.Checked
                 && !cbxPartAssignment.Checked
                 && !cbxNoAssignment.Checked)
                {
                    cbxNoAssignment.Focus();
                    ShowWarningDialog(MsgWngSelectionRequired, lblAssignmentFlag.Text);
                    return;
                }
                if (!ValidateSearchOptions()) return;

                DisplayInvoices();
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "照会");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private bool ValidateSearchOptions()
        {
            if (!datBilledAtFrom.ValidateRange(datBilledAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblBilledAt.Text))) return false;
            if (!datPublishAtFrom.ValidateRange(datPublishAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblPublisAt.Text))) return false;
            if (!datPublishAtFirstFrom.ValidateRange(datPublishAtFirstTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblPublishAt1st.Text))) return false;
            if (!txtInvoiceCodeFrom.ValidateRange(txtInvoiceCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblInvoiceCodeFromTo.Text))) return false;
            if (!txtDepartmentFrom.ValidateRange(txtDepartmentTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDepartment.Text))) return false;
            if (!txtCustomerFrom.ValidateRange(txtCustomerTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomer.Text))) return false;
            if (!txtStaffFrom.ValidateRange(txtStaffTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaff.Text))) return false;
            return true;
        }
        private void DisplayInvoices()
        {
            var list = GetSearchTasks();
            ProgressManager = new TaskProgressManager(list);

            var task = TaskSerchInvoices();
            NLogHandler.WriteDebug(this, "請求書データ 照会開始");
            ProgressDialogStart(ParentForm
                , ParentForm.Text
                , task
                , ProgressManager
                , Login
                , AutoCloseProgressDialog
                , true
                , OnCancelHandler);
            NLogHandler.WriteDebug(this, "請求書データ 照会終了");
        }
        private BillingInvoiceSearch GetSearchOption()
        {
            var option =  GetSerchOptionBase();
            option.ClosingAt = datClosingAt.Value;
            option.BilledAtFrom = datBilledAtFrom.Value;
            option.BilledAtTo = datBilledAtTo.Value;
            option.CollectCategoryId = CollectCategoryId;
            option.DepartmentCodeFrom = txtDepartmentFrom.Text;
            option.DepartmentCodeTo = txtDepartmentTo.Text;
            option.CustomerCodeFrom = txtCustomerFrom.Text;
            option.CustomerCodeTo = txtCustomerTo.Text;

            option.PublishAtFrom = datPublishAtFrom.Value;
            if (datPublishAtTo.Value.HasValue)
            {
                var updateAtTo = datPublishAtTo.Value.Value;
                option.PublishAtTo = updateAtTo.Date.AddDays(1).AddMilliseconds(-1);
            }
            option.PublishAtFirstFrom = datPublishAtFirstFrom.Value;
            if (datPublishAtFirstTo.Value.HasValue)
            {
                var updateAtTo = datPublishAtFirstTo.Value.Value;
                option.PublishAtFirstTo = updateAtTo.Date.AddDays(1).AddMilliseconds(-1);
            }
            option.InvoiceCodeFrom = txtInvoiceCodeFrom.Text;
            option.InvoiceCodeTo = txtInvoiceCodeTo.Text;
            option.InvoiceCode = txtInvoiceCode.Text;
            option.StaffCodeFrom = txtStaffFrom.Text;
            option.StaffCodeTo = txtStaffTo.Text;
            var assignmentFlag
               = (cbxNoAssignment.Checked ? (int)AssignmentFlagChecked.NoAssignment : (int)AssignmentFlagChecked.None)
               | (cbxPartAssignment.Checked ? (int)AssignmentFlagChecked.PartAssignment : (int)AssignmentFlagChecked.None)
               | (cbxFullAssignment.Checked ? (int)AssignmentFlagChecked.FullAssignment : (int)AssignmentFlagChecked.None);
            option.AssignmentFlg = assignmentFlag;
            return option;
        }
        private BillingInvoiceSearch GetSerchOptionBase()
        {
            var option = new BillingInvoiceSearch();
            option.CompanyId = Login.CompanyId;
            option.ClientKey = ClientKey;
            option.IsPublished = true;
            option.ReportId = nameof(PC0401);

            return option;
        }
        private List<TaskProgress> GetSearchTasks()
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求書データ照会"));

            return list;
        }
        private async Task TaskSerchInvoices()
        {
            var option = GetSearchOption();
            ClearGrid();
            BillingInvoiceSearchCondition = option;

            var source = await GetBillingInvoicesAsync(option);
            if (source == null)
            {
                ProgressManager.Abort();
                ShowWarningDialog(MsgErrSomethingError, "照会");
                return;
            }
            else if (!source.Any())
            {
                ProgressManager.NotFind();
                txtBillingCount.Text = "0";
                txtBillingAmount.Text = "0";
                txtRemainAmount.Text = "0";
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
            else
            {
                tbcBillingSearch.SelectedIndex = 1;
                grid.DataSource = new BindingSource(source, null);

                BaseContext.SetFunction07Enabled(true);
                BaseContext.SetFunction08Enabled(true);
                BaseContext.SetFunction09Enabled(true);

                txtBillingCount.Text = source.Count().ToString("#,0");
                txtBillingAmount.Text = source.Sum(x => x.AmountSum).ToString("#,0");
                txtRemainAmount.Text = source.Sum(x => x.RemainAmountSum).ToString("#,0");
            }
            ProgressManager.UpdateState();
            SetFunctionKeyEnable();
        }
        private void SetFunctionKeyEnable()
        {
            var isCheckedForCreateInvoice = grid.Rows.Any(x => Convert.ToBoolean(x[CellName(nameof(BillingInvoice.Checked))].EditedFormattedValue));
            BaseContext.SetFunction03Enabled(isCheckedForCreateInvoice);
            BaseContext.SetFunction04Enabled(isCheckedForCreateInvoice);

        }
        #endregion

        #region クリア
        [OperationLog("クリア")]
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
            ClearGrid();
            tbcBillingSearch.SelectedIndex = 0;
            ClearControls();
            SetUnableFunctionKeys();
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
            lblStaffNameFrom.Clear();
            lblStaffNameTo.Clear();
            CollectCategoryId = 0;
            txtRemainAmount.Clear();
            txtBillingAmount.Clear();
            txtBillingCount.Clear();
            datPublishAtFrom.Clear();
            datPublishAtTo.Clear();
            datPublishAtFirstFrom.Clear();
            datPublishAtFirstTo.Clear();
            txtInvoiceCodeFrom.Clear();
            txtInvoiceCodeTo.Clear();
            txtInvoiceCode.Clear();
            txtStaffFrom.Clear();
            txtStaffTo.Clear();
            cbxFullAssignment.Checked = false;
            cbxPartAssignment.Checked = true;
            cbxNoAssignment.Checked = true;
        }
        private void ClearGrid()
        {
            grid.DataSource = null;
        }
        private void SetUnableFunctionKeys()
        {
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
        }

        #endregion

        #region 再発行
        [OperationLog("再発行")]
        private void RePublishInvoices()
        {
            try
            {
                ClearStatusMessage();
                grid.EndEdit();

                if (!grid.Rows.Any(x => (x.DataBoundItem as BillingInvoice).Checked == 1))
                {
                    ShowWarningDialog(MsgWngNotExistUpdateData, "更新を行うデータ");
                    return;
                }

                var targetInvoices = new List<long>();
                foreach (var row in grid.Rows)
                {
                    var isChecked = Convert.ToBoolean(row.Cells[CellName(nameof(BillingInvoice.Checked))].Value);
                    if (isChecked)
                    {
                        var billingInputId = Convert.ToInt64(row.Cells[CellName(nameof(BillingInvoice.BillingInputId))].Value);
                        targetInvoices.Add(billingInputId);
                    }
                }

                PublishTargetIds = targetInvoices.ToArray();

                var form = new dlgPublishInvoices();
                form.ApplicationContext = ApplicationContext;
                form.ApplicationControl = ApplicationControl;
                form.CompanyInfo = Company;
                form.StartPosition = FormStartPosition.CenterScreen;
                ApplicationContext.ShowDialog(this, form);
                if (form.Result == dlgPublishInvoices.PublishResult.Print)
                {
                    PdfSetting = GetPdfOutputSetting();
                    if (PdfSetting.IsAllInOne)
                    {
                        PrintingAllInOne();
                    }
                    else
                    {
                        PrintingByReport();
                    }
                }
                else if (form.Result == dlgPublishInvoices.PublishResult.CSV)
                {
                    OutputCSV();
                }
                else
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #region 再発行（PDF）
        private void PrintingAllInOne()
        {
            try
            {
                //プリント
                var serverPath = GetServerPath();
                var list = GetBaseProgress();
                ProgressManager = new TaskProgressManager(list);
                var task = LoadInvoicesDetails();
                var dialogResult = ProgressDialogStart(ParentForm,
                    ParentForm.Text + " 印刷",
                    task,
                    ProgressManager,
                    Login,
                    AutoCloseProgressDialog,
                    true,
                    OnCancelHandler);

                if (dialogResult != DialogResult.OK)
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var report = task.Result;
                if (report == null)
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }

                Action<Form> EndPrint = async (Form) =>
                {
                   await UpdatePublishAtAsync(PublishTargetIds);
                };

                ShowDialogPreview(ParentForm, report, serverPath, EndPrint);
                DisplayInvoices();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void PrintingByReport()
        {
            try
            {
                var selectedPath = string.Empty;
                var rootBrowserPath = new List<string>();
                if (!LimitAccessFolder ?
                    !ShowFolderBrowserDialog("", out selectedPath) :
                    !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmStartXXX, "発行"))
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
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "発行");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private async Task PrintTaskByReport(string path)
        {
            try
            {
                var fileList = new List<string>();
                ProgressManager.UpdateState();

                foreach (var id in PublishTargetIds)
                {
                    var option = new BillingInvoiceDetailSearch();
                    option.CompanyId = Login.CompanyId;
                    option.BillingInputIds = new long[] { id };
                    using (var report = await CreateReport(option, GetReportSetting(), false))
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
                ProgressManager.UpdateState();

                if (PdfSetting.UseZip)
                    Util.ArchivesAsZip(fileList, path, $"請求書{DateTime.Now.ToString("yyyyMMdd")}", PdfSetting.MaximumByte);

                ClearGrid();
                ProgressManager.UpdateState();
                await TaskSerchInvoices();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ProgressManager.Abort();
            }
        }
        private List<TaskProgress> GetBaseProgress()
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求書データ取得"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求書の作成"));
            return list;
        }
        private async Task<SectionReport> LoadInvoicesDetails()
        {
            //請求書データ取得
            var option = new BillingInvoiceDetailSearch();
            option.CompanyId = Login.CompanyId;
            option.BillingInputIds = PublishTargetIds;

            //請求書作成
            var reportSetting = GetReportSetting();
            if (reportSetting == null)
            {
                ProgressManager.Abort();
            }
            ProgressManager.UpdateState();

            var report = await CreateReport( option, reportSetting, true);
            ProgressManager.UpdateState();
            return report;
        }
        private async Task<BillingInvoiceReport> CreateReport(BillingInvoiceDetailSearch option,
            ReportSettingsResult reportSetting,
            bool doUpdateState)
        {
            var report = new BillingInvoiceReport(false);
            report.Company = Company;
            report.CompanyLogos = CompanyLogos;
            report.Initialize(reportSetting.ReportSettings);
            report.lblNote1.Text = Note1Name;
            var reportSource = await GetBillingInvoicesDetailsAsync(option);
            report.CustomerCode = reportSource.FirstOrDefault().CustomerCode;
            report.CustomerName = reportSource.FirstOrDefault().CustomerName;
            report.InvoiceCode = reportSource.FirstOrDefault().InvoiceCode;
            var invoiceDetails = report.BuildDataSource(reportSource, Company);
            if (doUpdateState)
            {
                ProgressManager.UpdateState();
            }
            report.DataSource = invoiceDetails;

            BillingInvoiceReport report2 = null;
            if (report.Setting.OutputCopy == ReportDoOrNot.Do)
            {
                report2 = new BillingInvoiceReport(false, true);
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
            return report;
        }
        #endregion

        #region 再発行(CSV)
        private void OutputCSV()
        {
            try
            {
                ClearStatusMessage();
                var pathMatching = string.Empty;
                var pathReceipt = string.Empty;

                if (!ShowSaveFileDialog(GetServerPath(),
                    $"再発行請求書明細データ_{DateTime.Now:yyyyMMdd}.csv",
                    out pathMatching,
                    cancellationMessaging: () => DispStatusMessage(MsgInfProcessCanceled))) return;

                if (!ShowConfirmDialog(MsgQstConfirmStartXXX, "CSV出力"))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var reoutputResult = OutputInvoiceDetailsData(pathMatching);

                if (reoutputResult)
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
            }
        }
        private bool OutputInvoiceDetailsData(string path)
        {
            Task<List<BillingInvoiceDetailForExport>> loadBillingInvoiceDetail = null;

            var tasks = new List<Task>();
            tasks.Add(loadBillingInvoiceDetail = CSVOutputTask());

            ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
            if (tasks.Any(x => x.Exception != null))
            {
                foreach (var ex in tasks.Where(x => x.Exception != null).Select(x => x.Exception))
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrExportError);
                return false;
            }
            ExportInvoiceDetailsData(path, loadBillingInvoiceDetail.Result);
            return true;
        }
        private async Task<List<BillingInvoiceDetailForExport>> CSVOutputTask()
        {
            return await GetDetailsForExport(PublishTargetIds);
        }
        private bool ExportInvoiceDetailsData(string path, List<BillingInvoiceDetailForExport> details)
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

            ProgressDialog.Start(ParentForm,async (cancel, progress) =>
            {
                var count = exporter.ExportAsync(path, details, cancel, progress);
                await UpdatePublishAtAsync(PublishTargetIds);
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

        #region 発行取消
        [OperationLog("発行取消")]
        private void CancelPiblish()
        {
            ClearStatusMessage();
            try
            {
                if (!grid.Rows.Any(x => (x.DataBoundItem as BillingInvoice).Checked == 1))
                {
                    ShowWarningDialog(MsgWngNotExistUpdateData, "更新を行うデータ");
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmStartXXX, "発行取消"))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var list = GetCancelPublishTasks();
                ProgressManager = new TaskProgressManager(list);

                var task = CancelPublishTask();
                NLogHandler.WriteDebug(this, "請求書 作成開始");
                ProgressDialogStart(ParentForm, ParentForm.Text, task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                NLogHandler.WriteDebug(this, "請求書 作成終了");
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private List<TaskProgress> GetCancelPublishTasks()
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 発行取消処理"));

            return list;
        }

        private async Task CancelPublishTask()
        {
            var targetInvoices = new List<long>();
            foreach (var row in grid.Rows)
            {
                var isChecked = Convert.ToBoolean(row.Cells[CellName(nameof(BillingInvoice.Checked))].Value);
                if (isChecked)
                {
                    var billingInputId = Convert.ToInt64(row.Cells[CellName(nameof(BillingInvoice.BillingInputId))].Value);

                    targetInvoices.Add(billingInputId);
                }
            }
            ProgressManager.UpdateState();

            var result = await CancelPublishAsync( targetInvoices.ToArray());
            if (result == null
                || result.ProcessResult == null
                || !result.ProcessResult.Result)
            {
                ProgressManager.Abort();
            }
            else
            {
                await TaskSerchInvoices();
            }
        }
        #endregion

        #region エクスポート
        [OperationLog("エクスポート")]
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
                var fileName = $"発行済請求書データ{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                //if (!ShowConfirmDialog(MsgQstConfirmExport))
                //{
                //    DispStatusMessage(MsgInfProcessCanceled);
                //    return;
                //}

                var exportBillingInvoice = ((IEnumerable)grid.DataSource).Cast<BillingInvoice>().ToList();
                var definition = new BillingInvoiceFileDifinition(new DataExpression(ApplicationControl), GridSettingInfo);

                var decimalFormat = "0";

                definition.AmountSumField.Format = value => value.ToString(decimalFormat);
                definition.RemainAmountSumField.Format = value => value.ToString(decimalFormat);

                definition.SetFieldsSetting(GridSettingInfo, definition.ConvertSettingToField);

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, exportBillingInvoice, cancel, progress);
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

        #region 全選択＆全解除
        [OperationLog("全選択")]
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

        [OperationLog("全解除")]
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

        #region 戻る＆終了
        [OperationLog("戻る")]
        private void Return()
        {
            tbcBillingSearch.SelectedIndex = 0;
        }

        [OperationLog("終了")]
        private void Close()
        {
            Task.Run(DeleteWorkTableAsync);
            ExitBilling();
        }

        private void ExitBilling()
        {
            try
            {
                Settings.SaveControlValue<PC1401>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PC1401>(Login, cbxCustomer.Name, cbxCustomer.Checked);
                Settings.SaveControlValue<PC1401>(Login, cbxStaff.Name, cbxStaff.Checked);
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
        private async Task<CountResult> CancelPublishAsync(long[] billingInputIds)
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
                    result = await client.CancelPublishAsync(SessionKey, hubConnection.ConnectionId, billingInputIds);
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
            => ClientKey = await Util.CreateClientKey(Login, nameof(PC1401));
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
        private async Task<List<BillingInvoiceDetailForPrint>> GetBillingInvoicesDetailsAsync(BillingInvoiceDetailSearch billingInvoiceDetailSearch)
       => await ServiceProxyFactory.DoAsync(async (BillingInvoiceServiceClient client) =>
                {
                    var getResult = await client.GetDetailsForPrintAsync(SessionKey, billingInvoiceDetailSearch);
                    if (getResult.ProcessResult.Result)
                        return getResult.BillingInvoicesDetails;
                    return null;
                });

        private async Task<List<ExportFieldSetting>> GetExportFieldSettingAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (ExportFieldSettingMasterClient client)
                => await client.GetItemsByExportFileTypeAsync(SessionKey, CompanyId, (int)CsvExportFileType.PublishInvoiceData));
            if (result.ProcessResult.Result)
                return result.ExportFieldSettings;
            return new List<ExportFieldSetting>();
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
        private async Task<CountResult> UpdatePublishAtAsync(long[] billingInputIds)
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
                    result = await client.UpdatePublishAtAsync(SessionKey, hubConnection.ConnectionId, billingInputIds);
                });
            }
            return result;
        }
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
                if (e.RowIndex < 0 || e.CellName == CellName(nameof(BillingInvoice.Checked))){ return; }
                ProgressDialog.Start(ParentForm, async (cancel, progress) =>
                {
                    var Ids = new List<long>();
                    var id = (long)grid.GetValue(e.RowIndex, CellName(nameof(BillingInvoice.BillingInputId)));
                    Ids.Add(id);
                    PublishTargetIds = Ids.ToArray();

                    //プリント
                    var serverPath = GetServerPath();
                    var report = await Task.Run(() => LoadInvoicesDetails());

                    ShowDialogPreview(ParentForm, report, serverPath);
                }, false, SessionKey);


            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellName != CellName(nameof(BillingInvoice.Checked))) return;
            SetFunctionKeyEnable();
        }
        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellName != CellName(nameof(BillingInvoice.Checked))) return;
            grid.EndEdit();
        }
        #endregion

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
        private void txtStaffFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                var staffResult = new Staff();
                var staffCodeFrom = txtStaffFrom.Text;
                var staffNameTo = string.Empty;
                var staffNameFrom = string.Empty;

                if (!string.IsNullOrEmpty(staffCodeFrom))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<StaffMasterClient>();

                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { staffCodeFrom });

                        staffResult = result.Staffs.FirstOrDefault();
                        if (staffResult != null)
                        {
                            staffCodeFrom = staffResult.Code;
                            staffNameFrom = staffResult.Name;
                            staffNameTo = staffResult.Name;
                            ClearStatusMessage();
                        }

                        txtStaffFrom.Text = staffCodeFrom;
                        lblStaffNameFrom.Text = staffNameFrom;

                        if (cbxStaff.Checked)
                        {
                            txtStaffTo.Text = staffCodeFrom;
                            lblStaffNameTo.Text = staffNameTo;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    if (cbxStaff.Checked)
                    {
                        txtStaffTo.Text = staffCodeFrom;
                        lblStaffNameTo.Clear();
                    }
                    lblStaffNameFrom.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

        }
        private void txtStaffTo_Validated(object sender, EventArgs e)
        {
            try
            {
                var staffResult = new Staff();
                var staffCodeTo = txtStaffTo.Text;
                var staffNameTo = string.Empty;

                if (!string.IsNullOrEmpty(staffCodeTo))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<StaffMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { staffCodeTo });

                        staffResult = result.Staffs.FirstOrDefault();

                        if (staffResult != null)
                        {
                            staffCodeTo = staffResult.Code;
                            staffNameTo = staffResult.Name;
                            ClearStatusMessage();
                        }
                        txtStaffTo.Text = staffCodeTo;
                        lblStaffNameTo.Text = staffNameTo;
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    lblStaffNameTo.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
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
        private void btnStaff_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                if (sender == btnStaffFrom)
                {
                    txtStaffFrom.Text = staff.Code;
                    lblStaffNameFrom.Text = staff.Name;
                    if (cbxStaff.Checked)
                    {
                        txtStaffTo.Text = staff.Code;
                        lblStaffNameTo.Text = staff.Name;
                    }
                }
                else
                {
                    txtStaffTo.Text = staff.Code;
                    lblStaffNameTo.Text = staff.Name;
                }
                ClearStatusMessage();
            }

        }
        #endregion
    }
}
