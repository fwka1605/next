using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingBalanceService;
using Rac.VOne.Client.Screen.CreditAgingListService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
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
using static Rac.VOne.Client.Reports.Settings.PF0201;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>債権総額管理表</summary>
    public partial class PF0201 : VOneScreenBase
    {
        private List<ReportSetting> ReportSettingList { get; set; }
        private CreditAgingListSearch CreditAgingListSearch { get; set; }
        private List<object> DetailSearchConditionList { get; set; }
        private string ArrivalDueDate1 { get; set; }
        private string ArrivalDueDate2 { get; set; }
        private string ArrivalDueDate3 { get; set; }
        private string ArrivalDueDate4 { get; set; }
        private string CellName(string value) => $"cel{value}";
        private TaskProgressManager ProgressManager;

        #region 初期化
        public PF0201()
        {
            InitializeComponent();
            grid.SetupShortcutKeys();
            grid.GridColorType = GridColorType.Special;
            Text = "債権総額管理表";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            grid.CellEditedFormattedValueChanged += grd_CellEditedFormattedValueChanged;
            grid.CellFormatting += grd_CellFormatting;
            grid.CellPainting += grd_CellPainting;
            grid.DataBindingComplete += grd_DataBindingComplete;

            tbcCreditAgingList.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcCreditAgingList.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = ExitCreditAgingList;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
            cbxFilterPositiveCreditBalance.EnabledChanged += CheckBox_EnabledCanged;
            cbxCalculateCreditLimitRegistered.EnabledChanged += CheckBox_EnabledCanged;
            rdoCustomerClosingDay.CheckedChanged += (sender, e) => {
                ClearStatusMessage();
                txtClosingDate.Enabled = rdoCustomerClosingDay.Checked;
                if (txtClosingDate.Enabled)
                    txtClosingDate.Select();
                else
                    txtClosingDate.Clear();
            };
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("照会");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SearchCreditAgingList;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearCreditAgingList;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintCreditAgingList;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportCreditAgingList;

            BaseContext.SetFunction07Caption("設定");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = SettingCreditAgingList;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitCreditAgingList;
        }

        private void PF0201_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var tasks = new List<Task>();
                if (Company == null) tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null) tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadReportSettingAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                Settings.SetCheckBoxValue<PF0201>(Login, cbxDepartmentCode);
                Settings.SetCheckBoxValue<PF0201>(Login, cbxStaff);
                Settings.SetCheckBoxValue<PF0201>(Login, cbxCustomer);

                ClearItem();
                SetFormat();
                InitializeControlsWithReportSettings();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetFormat()
        {
            if (ApplicationControl == null) return;

            var expression = new DataExpression(ApplicationControl);
            txtDepartmentCodeFrom.Format = expression.DepartmentCodeFormatString;
            txtDepartmentCodeFrom.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentCodeFrom.PaddingChar = expression.DepartmentCodePaddingChar;
            txtDepartmentCodeTo.Format = expression.DepartmentCodeFormatString;
            txtDepartmentCodeTo.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentCodeTo.PaddingChar = expression.DepartmentCodePaddingChar;

            txtStaffCodeFrom.Format = expression.StaffCodeFormatString;
            txtStaffCodeFrom.MaxLength = expression.StaffCodeLength;
            txtStaffCodeFrom.PaddingChar = expression.StaffCodePaddingChar;
            txtStaffCodeTo.Format = expression.StaffCodeFormatString;
            txtStaffCodeTo.MaxLength = expression.StaffCodeLength;
            txtStaffCodeTo.PaddingChar = expression.StaffCodePaddingChar;

            txtCustomerCodeFrom.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeFrom.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeFrom.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;

            txtCustomerCodeTo.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeTo.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeTo.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeTo.PaddingChar = expression.CustomerCodePaddingChar;

            if (!UseForeignCurrency)
            {
                lblPriceUnit.Visible = true;
                lblReportUnitPrice.Visible = true;
            }
        }

        private void InitializeControlsWithReportSettings()
        {
            var enableCreditLimitCalculate = (GetReportSettingStaffSelection() == ReportStaffSelection.ByCustomerMaster);
            pnlFilterPositiveCreditBalance.Enabled = enableCreditLimitCalculate;
            cbxCalculateCreditLimitRegistered.Enabled = enableCreditLimitCalculate;
            cbxFilterPositiveCreditBalance_CheckedChanged(cbxFilterPositiveCreditBalance, EventArgs.Empty);
        }

        #endregion

        #region Function Key
        [OperationLog("照会")]
        private void SearchCreditAgingList()
        {
            if (!ValidateChildren()) return;
            if (!ValidateForSearch()) return;

            ClearStatusMessage();

            try
            {
                ProgressDialog.Start(Parent, LoadReportSettingAsync(), false, SessionKey);
                var option = GetSearchOptoin();

                var list = GetBaseProgressTask(option);
                list.Add(new TaskProgress("画面の更新"));
                ProgressManager = new TaskProgressManager(list);
                NLogHandler.WriteDebug(this, "債権総額管理表 照会開始");
                ProgressDialogStart(ParentForm, ParentForm.Text, LoadCreditAgingListAsync(option), ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                NLogHandler.WriteDebug(this, "債権総額管理表 照会終了");
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private List<TaskProgress> GetBaseProgressTask(CreditAgingListSearch option)
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 期日到来データ集計"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求データ集計"));
            if (option.ConsiderReceiptAmount)
                list.Add(new TaskProgress($"{ParentForm.Text} 入金データ集計"));
            list.Add(new TaskProgress($"{ParentForm.Text} 消込データ集計"));
            list.Add(new TaskProgress($"{ParentForm.Text} 集計データ取得"));
            list.Add(new TaskProgress($"{ParentForm.Text} 集計データ整形"));
            return list;
        }

        private async Task LoadCreditAgingListAsync(CreditAgingListSearch option)
        {
            CreditAgingListSearch = option;
            DetailSearchConditionList = SettingSearchCondition();

            BaseContext.SetFunction07Enabled(true);

            var list = await GetCreditAgingListAsync(option);
            if (list == null)
            {
                grid.Template = null;
                ProgressManager.Canceled = true;
                DispStatusMessage(MsgInfProcessCanceled);
            }
            if (!list.Any())
            {
                grid.Template = null;
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }

            BindDueDate();
            await DisplayReportSettingData();
            InitializeGridTemplate();
            grid.DataSource = new BindingSource(list, null);
            BaseContext.SetFunction07Enabled(false);
            tbcCreditAgingList.SelectedIndex = 1;

            ProgressManager.UpdateState();
        }

        private bool ValidateForSearch()
        {
            if (!datBaseDate.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblBaseDate.Text);
                datBaseDate.Select();
                return false;
            }
            if (txtClosingDate.Enabled && string.IsNullOrEmpty(txtClosingDate.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblClosingDate.Text);
                txtClosingDate.Select();
                return false;
            }

            if (!txtDepartmentCodeFrom.ValidateRange(txtDepartmentCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDepartment.Text))) return false;

            if (!txtStaffCodeFrom.ValidateRange(txtStaffCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaff.Text))) return false;

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCode.Text))) return false;

            return true;
        }

        private CreditAgingListSearch GetSearchOptoin()
        {
            var option = new CreditAgingListSearch();
            option.CompanyId = CompanyId;
            if (!string.IsNullOrEmpty(txtClosingDate.Text))
                option.ClosingDay = int.Parse(txtClosingDate.Text);
            if (datBaseDate.Value.HasValue)
                option.SetYearMonth(datBaseDate.Value.Value, Company.ClosingDay);
            if (!string.IsNullOrEmpty(txtDepartmentCodeFrom.Text))
                option.DepartmentCodeFrom = txtDepartmentCodeFrom.Text;
            if (!string.IsNullOrEmpty(txtDepartmentCodeTo.Text))
                option.DepartmentCodeTo = txtDepartmentCodeTo.Text;
            if (!string.IsNullOrEmpty(txtStaffCodeFrom.Text))
                option.StaffCodeFrom = txtStaffCodeFrom.Text;
            if (!string.IsNullOrEmpty(txtStaffCodeTo.Text))
                option.StaffCodeTo = txtStaffCodeTo.Text;
            if (!string.IsNullOrEmpty(txtCustomerCodeFrom.Text))
                option.CustomerCodeFrom = txtCustomerCodeFrom.Text;
            if (!string.IsNullOrEmpty(txtCustomerCodeTo.Text))
                option.CustomerCodeTo = txtCustomerCodeTo.Text;
            option.FilterPositiveCreditBalance = cbxFilterPositiveCreditBalance.Checked;
            option.ConsiderGroupWithCalculate = cbxFilterPositiveCreditBalance.Checked && rdoFilterWithCustomerGroup.Checked;
            option.CalculateCreditLimitRegistered = cbxCalculateCreditLimitRegistered.Checked;

            option.RequireDepartmentTotal = GetReportSettingDepartmentTotal() == ReportDoOrNot.Do;
            option.RequireStaffTotal = GetReportSettingStaffTotal() == ReportDoOrNot.Do;
            option.UseMasterStaff = GetReportSettingStaffSelection() == ReportStaffSelection.ByCustomerMaster;
            option.ConsiderCustomerGroup = GetReportSettingCustomerGroupType() != ReportCustomerGroup.PlainCusomter;
            option.ConsiderReceiptAmount = GetReportSettingReceiptType() == ReportReceiptType.UseReceiptAmount;
            option.UseBilledAt = GetReportSettingTargetDate() == ReportTargetDate.BilledAt;
            var unit = GetReportSettingUnitPrice();
            option.UnitPrice
                = unit == ReportUnitPrice.Per1 ? 1M
                : unit == ReportUnitPrice.Per1000 ? 1000M
                : unit == ReportUnitPrice.Per10000 ? 10000M
                : unit == ReportUnitPrice.Per1000000 ? 1000000M : 1M;
            option.UseParentCustomerCredit = GetReportSettingCreditLimitType() == ReportCreditLimitType.UseParentCustomerCredit;

            return option;
        }

        private List<object> SettingSearchCondition()
        {
            var searchReport = new List<object>();
            var detailReport = new List<object>();

            var baseDate = datBaseDate.Value?.ToString("yyyy/MM");
            searchReport.Add(new SearchData("処理月", baseDate));
            detailReport.Add(new SearchData("処理月", baseDate));
            var rdoType = rdoCompanyClosingDay.Checked ? "会計帳簿の締日残高" : "得意先ごとの締日残高";
            searchReport.Add(new SearchData("残高タイプ", rdoType));
            searchReport.Add(new SearchData("締日", txtClosingDate.GetPrintValue()));
            var wavedash = "{0} ～ {1}";
            var department = string.Format(wavedash,
                txtDepartmentCodeFrom.GetPrintValueCode(lblDepartmentNameFrom),
                txtDepartmentCodeTo.GetPrintValueCode(lblDepartmentNameTo));
            searchReport.Add(new SearchData("請求部門コード", department));
            detailReport.Add(new SearchData("請求部門コード", department));
            var staff = string.Format(wavedash,
                txtStaffCodeFrom.GetPrintValueCode(lblStaffNameFrom),
                txtStaffCodeTo.GetPrintValueCode(lblStaffNameTo));
            searchReport.Add(new SearchData("担当者コード", staff));
            detailReport.Add(new SearchData("担当者コード", staff));
            var customer = string.Format(wavedash,
                txtCustomerCodeFrom.GetPrintValueCode(lblCustomerNameFrom),
                txtCustomerCodeTo.GetPrintValueCode(lblCustomerNameTo));
            searchReport.Add(new SearchData("得意先コード", customer));

            var creditAmtMinus = (cbxFilterPositiveCreditBalance.Checked) ?  "する": "しない";
            searchReport.Add(new SearchData("与信残高がマイナスのもののみ表示", creditAmtMinus));

            var creditRemainDecision = " ";
            if (cbxFilterPositiveCreditBalance.Checked)
            {
                if (rdoFilterWithCustomerGroup.Checked)
                {
                    creditRemainDecision = "債権代表者の総計額で判定";
                }
                else
                {
                    creditRemainDecision = "子会社の個別残高で判定";
                }
            }
            searchReport.Add(new SearchData("与信残高判定方法", creditRemainDecision));

            var creditLimit = "(指定なし)";
            if (cbxCalculateCreditLimitRegistered.Checked)
            {
                creditLimit = "する";
            }
            else
            {
                creditLimit = "しない";
            }
            searchReport.Add(new SearchData("与信限度額が0でも残計算する", creditLimit));

            var billiOrSaleDate = GetReportSettingTargetDate() == ReportTargetDate.BilledAt ?  "請求日" : "売上日";
            searchReport.Add(new SearchData("集計基準日", billiOrSaleDate));
            detailReport.Add(new SearchData("集計基準日", billiOrSaleDate));

            var billingRemain = GetReportSettingReceiptType() == ReportReceiptType.UseMatchingAmount ? "消込額を使用する" : "入金額を使用する";
            searchReport.Add(new SearchData("債権総額計算方法", billingRemain));

            var groupType = GetReportSettingCustomerGroupType();
            var customerGroup =
                groupType == ReportCustomerGroup.PlainCusomter      ? "得意先" :
                groupType == ReportCustomerGroup.ParentWithChildren ? "債権代表者/得意先" : "債権代表者";
            searchReport.Add(new SearchData("得意先集計方法", customerGroup));

            var staffSelection = GetReportSettingStaffSelection() == ReportStaffSelection.ByBillingData ? "請求データ" : "得意先マスター";
            searchReport.Add(new SearchData("担当者集計方法", staffSelection));
            detailReport.Add(new SearchData("担当者集計方法", staffSelection));

            var unitType = GetReportSettingUnitPrice();
            var unit = string.Empty;
            switch (unitType)
            {
                case ReportUnitPrice.Per1: unit = "円"; break;
                case ReportUnitPrice.Per1000: unit = "千円"; break;
                case ReportUnitPrice.Per10000: unit = "万円"; break;
                case ReportUnitPrice.Per1000000: unit = "百万円"; break;
            }

            if (!UseForeignCurrency)
            {
                searchReport.Add(new SearchData("金額単位", unit));
                detailReport.Add(new SearchData("金額単位", unit));
            }

            var creditType = GetReportSettingCreditLimitType() == ReportCreditLimitType.UseCustomerSummaryCredit ? "得意先の与信限度額を集計する" : "債権代表者の与信限度額を使用する";
            searchReport.Add(new SearchData("与信限度額集計方法", creditType));
            detailReport.Add(new SearchData("与信限度額集計方法", creditType));

            DetailSearchConditionList = detailReport;
            return searchReport;

        }

        private async Task DisplayReportSettingData()
        {
            await LoadReportSettingAsync();
            lblTargetDate.Text            = GetReportSettingTargetDate()         == ReportTargetDate.BilledAt ? "請求日" : "売上日";
            lblReportStaffSelection.Text  = GetReportSettingStaffSelection()     == ReportStaffSelection.ByBillingData ? "請求データ" : "得意先マスター";
            lblReportReceiptType.Text     = GetReportSettingReceiptType()        == ReportReceiptType.UseMatchingAmount ? "消込額を使用する" : "入金額を使用する";
            lblReportDoOrNot1.Text        = GetReportSettingDepartmentTotal()    == ReportDoOrNot.NotDo ? "しない" : "する";
            lblReportDoOrNot2.Text        = GetReportSettingStaffTotal()         == ReportDoOrNot.NotDo ? "しない" : "する";
            lblReportCreditLimitType.Text = GetReportSettingCreditLimitType()    == ReportCreditLimitType.UseCustomerSummaryCredit ? "得意先" : "債権代表者";

            var customerGroup = GetReportSettingCustomerGroupType();
            switch (customerGroup)
            {
                case ReportCustomerGroup.PlainCusomter     : lblReportCustomerGroup.Text = "得意先";            break;
                case ReportCustomerGroup.ParentWithChildren: lblReportCustomerGroup.Text = "債権代表者/得意先"; break;
                case ReportCustomerGroup.ParentOnly        : lblReportCustomerGroup.Text = "債権代表者";        break;
            }

            var unitPrice = GetReportSettingUnitPrice();
            switch (unitPrice)
            {
                case ReportUnitPrice.Per1      : lblReportUnitPrice.Text = "円"; break;
                case ReportUnitPrice.Per1000   : lblReportUnitPrice.Text = "千円"; break;
                case ReportUnitPrice.Per10000  : lblReportUnitPrice.Text = "万円"; break;
                case ReportUnitPrice.Per1000000: lblReportUnitPrice.Text = "百万円"; break;
            }
        }

        [OperationLog("クリア")]
        private void ClearCreditAgingList()
        {
            ClearStatusMessage();
            ClearItem();
        }

        private void ClearItem()
        {
            datBaseDate.Clear();
            rdoCompanyClosingDay.Checked = true;
            txtClosingDate.Enabled = false;
            txtClosingDate.Clear();
            txtDepartmentCodeFrom.Clear();
            lblDepartmentNameFrom.Clear();
            txtDepartmentCodeTo.Clear();
            lblDepartmentNameTo.Clear();
            txtStaffCodeFrom.Clear();
            lblStaffNameFrom.Clear();
            txtStaffCodeTo.Clear();
            lblStaffNameTo.Clear();
            txtCustomerCodeFrom.Clear();
            lblCustomerNameFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameTo.Clear();
            cbxFilterPositiveCreditBalance.Checked = false;
            rdoFilterWithCustomerGroup.Checked = true;
            rdoFilterWithCustomerGroup.Enabled = false;
            rdoFilterEachChildren.Checked = false;
            rdoFilterEachChildren.Enabled = false;
            cbxCalculateCreditLimitRegistered.Checked = false;
            grid.DataSource = null;
            ClearGridHeader();
            tbcCreditAgingList.SelectedIndex = 0;
            datBaseDate.Select();
            lblTargetDate.Clear();
            lblReportStaffSelection.Clear();
            lblReportReceiptType.Clear();
            lblReportDoOrNot1.Clear();
            lblReportDoOrNot2.Clear();
            lblReportCustomerGroup.Clear();
            lblReportUnitPrice.Clear();
            BaseContext.SetFunction07Enabled(true);
        }

        private void ClearGridHeader()
        {
            var template = new Template();
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var rowHeight = builder.DefaultRowHeight;
            builder.Items.Add(new CellSetting(rowHeight, 0, "Header"));
            builder.BuildHeaderOnly(template);
            grid.Template = template;
        }

        [OperationLog("印刷")]
        private void PrintCreditAgingList()
        {
            ClearStatusMessage();
            if (!ValidateForSearch()) return;

            try
            {
                BindDueDate();

                var serverPath = string.Empty;
                try
                {
                    var loadTask = Task.Run(async () => serverPath = await Util.GetGeneralSettingServerPathAsync(Login));
                    ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                ProgressDialog.Start(ParentForm, LoadReportSettingAsync(), false, SessionKey);
                var option = GetSearchOptoin();

                var list = GetBaseProgressTask(option);
                list.Add(new TaskProgress("帳票の作成"));
                ProgressManager = new TaskProgressManager(list);
                var task = GetCreditAgingListReportAsync(option);
                var dialogResult = ProgressDialogStart(ParentForm, ParentForm.Text + " 印刷", task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                if (dialogResult != DialogResult.OK)
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var result = task.Result;
                if (result == null)
                {
                    ShowWarningDialog(MsgErrCreateReportError);
                    return;
                }
                else if (result.Document.Pages.Count == 0)
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }

                ShowDialogPreview(ParentForm, result, serverPath);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private async Task<CreditAgingListReport> GetCreditAgingListReportAsync(CreditAgingListSearch option)
        {
            CreditAgingListSearch = option;
            DetailSearchConditionList = SettingSearchCondition();
            var source = await GetCreditAgingListAsync(option);
            if (source == null)
            {
                ProgressManager.Canceled = true;
                return null;
            }

            if (!source.Any())
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                return new CreditAgingListReport();
            }

            CreditAgingListReport report = null;
            await Task.Run(() => report = CreateCreditAgingListReport(source));

            ProgressManager.UpdateState();
            return report;
        }

        private CreditAgingListReport CreateCreditAgingListReport(List<CreditAgingList> source)
        {
            var title = "債権総額管理表";
            var outputName = $"{title}{DateTime.Today:yyyyMMdd}";

            var report = new CreditAgingListReport();
            report.Name = outputName;
            report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
            report.DisplayCustomerCode = GetReportSettingDisplayCustomerCode() == ReportDoOrNot.Do;
            report.ConsiderCustomerGroup = GetReportSettingCustomerGroupType() != ReportCustomerGroup.PlainCusomter;
            report.RequireDepartmentTotal = GetReportSettingDepartmentTotal() == ReportDoOrNot.Do;
            report.RequireStaffTotal = GetReportSettingStaffTotal() == ReportDoOrNot.Do;
            report.UseMasterStaff = GetReportSettingStaffSelection() == ReportStaffSelection.ByCustomerMaster;
            report.lblArrivalDueDate1.Text = ArrivalDueDate1;
            report.lblArrivalDueDate2.Text = ArrivalDueDate2;
            report.lblArrivalDueDate3.Text = ArrivalDueDate3;
            report.lblArrivalDueDate4.Text = ArrivalDueDate4;
            report.SetPageDataSetting(source);
            report.Document.Name = outputName;
            report.Run(false);

            var searchReport = new SearchConditionSectionReport();
            searchReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, title);
            searchReport.Name = outputName;
            searchReport.SetPageDataSetting(DetailSearchConditionList);
            searchReport.SetPageNumber(report.Document.Pages.Count);
            searchReport.Run(false);
            report.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchReport.Document.Pages.Clone());
            return report;

        }

        [OperationLog("エクスポート")]
        private void ExportCreditAgingList()
        {
            ClearStatusMessage();
            if (!ValidateForSearch()) return;
            BindDueDate();

            try
            {
                var serverPath = string.Empty;
                var loadTask = Task.Run(async () => serverPath = await Util.GetGeneralSettingServerPathAsync(Login));
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"債権総額管理表{datBaseDate.Value.Value:yyMM}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                ProgressDialog.Start(ParentForm, LoadReportSettingAsync(), false, SessionKey);
                var option = GetSearchOptoin();
                var list = GetBaseProgressTask(option);
                list.Add(new TaskProgress("データ出力"));

                ProgressManager = new TaskProgressManager(list);
                var task = ExportCreditAgingListAsync(filePath, option);
                var dialogResult = ProgressDialogStart(ParentForm, ParentForm.Text + " エクスポート", task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                if (dialogResult != DialogResult.OK)
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                if (task.Result == 0)
                {
                    DispStatusMessage(MsgWngNoExportData);
                    return;
                }
                DispStatusMessage(MsgInfFinishExport);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<int> ExportCreditAgingListAsync(string filePath, CreditAgingListSearch option)
        {
            var source = await GetCreditAgingListAsync(option);
            if (source == null)
            {
                ProgressManager.Canceled = true;
                return 0;
            }

            var considerGroup = (GetReportSettingCustomerGroupType() != ReportCustomerGroup.PlainCusomter);
            source = source.Where(x => x.RecordType == 0)
                .Select(x =>
                {
                    if (!considerGroup
                    || !x.ParentCustomerId.HasValue
                    || x.ParentCustomerFlag != 0) return x;
                    x.CustomerName = $" *{x.CustomerName}";
                    return x;
                }).ToList();

            if (!source.Any())
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                return 0;
            }

            var definition = new CreditAgingListFileDefinition(new DataExpression(ApplicationControl));
            definition.ArrivalDueDate1Field.FieldName = ArrivalDueDate1;
            definition.ArrivalDueDate2Field.FieldName = ArrivalDueDate2;
            definition.ArrivalDueDate3Field.FieldName = ArrivalDueDate3;
            definition.ArrivalDueDate4Field.FieldName = ArrivalDueDate4;

            var decimalFormat = "#,##0";
            definition.CreditAmountField.Format = value => value.ToString(decimalFormat);
            definition.UnsettledRemainField.Format = value => value.ToString(decimalFormat);
            definition.BillingRemainField.Format = value => value.ToString(decimalFormat);
            definition.CreditLimitField.Format = value => value.ToString(decimalFormat);
            definition.CreditRemainField.Format = value => value.ToString(decimalFormat);
            definition.ArrivalDueDate1Field.Format = value => value.ToString(decimalFormat);
            definition.ArrivalDueDate2Field.Format = value => value.ToString(decimalFormat);
            definition.ArrivalDueDate3Field.Format = value => value.ToString(decimalFormat);
            definition.ArrivalDueDate4Field.Format = value => value.ToString(decimalFormat);

            if (GetReportSettingDepartmentTotal() == ReportDoOrNot.NotDo)
            {
                definition.DepartmentCodeField.Ignored = true;
                definition.DepartmentNameField.Ignored = true;
            }
            if (GetReportSettingStaffTotal() == ReportDoOrNot.NotDo)
            {
                definition.StaffCodeField.Ignored = true;
                definition.StaffNameField.Ignored = true;
            }

            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = Login.CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            var result = await exporter.ExportAsync(filePath, source, new System.Threading.CancellationToken(false), new Progress<int>());
            if (result == 0) ProgressManager.Abort();
            ProgressManager.UpdateState();
            return result;
        }

        [OperationLog("設定")]
        private void SettingCreditAgingList()
        {
            ClearStatusMessage();
            var reload = false;
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PF0201);
                screen.InitializeParentForm("帳票設定");
                form.StartPosition = FormStartPosition.CenterParent;
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
                reload = screen.SettingSaved;
            }
            if (!reload) return;
            ProgressDialog.Start(Parent, LoadReportSettingAsync(), false, SessionKey);
            InitializeControlsWithReportSettings();
        }

        [OperationLog("終了")]
        private void ExitCreditAgingList()
        {
            BaseForm.Close();
            SaveSettingValues();
        }
        private void ReturnToSearchCondition()
        {
            tbcCreditAgingList.SelectedIndex = 0;
        }

        #endregion

        private void InitializeGridTemplate()
        {
            var template = new Template();
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);

            var height = builder.DefaultRowHeight;
            var chkShow = builder.GetCheckBoxCell();
            chkShow.Appearance = Appearance.Button;
            chkShow.Style = new CellStyle() { TextAlign = MultiRowContentAlignment.MiddleCenter };
            chkShow.TrueValue = "-";
            chkShow.FalseValue = "+";
            var groupType = GetReportSettingCustomerGroupType();
            chkShow.Text =
                groupType == ReportCustomerGroup.ParentOnly ? "+" :
                groupType == ReportCustomerGroup.ParentWithChildren ? "-" : "";

            var considerGroup = groupType != ReportCustomerGroup.PlainCusomter;
            var codeWidth = considerGroup ? 85 : 115;
            var cbxWidth = considerGroup ? 30 : 0;
            var displayCreditLimit = GetReportSettingStaffSelection() == ReportStaffSelection.ByCustomerMaster;
            var creditWidth  = displayCreditLimit ? 130 : 0;
            var balanceWidth = displayCreditLimit ? 150 : 0;
            var markWidth    = displayCreditLimit ?  20 : 0;

            var bdLNonRThn = builder.GetBorder(left: LineStyle.None, right: LineStyle.Thin);
            var bdLNonRPal = builder.GetBorder(left: builder.GetLine(LineStyle.None), right: builder.GetLine(lineColor: Color.LightGray));
            var bdLNonRDbl = builder.GetBorder(left: LineStyle.None  , right: LineStyle.Double);
            var bdLDblRDbl = builder.GetBorder(left: LineStyle.Double, right: LineStyle.Double);
            var bdLDblRDtd = builder.GetBorder(left: LineStyle.Double, right: LineStyle.Dotted);
            var bdLNonRDtd = builder.GetBorder(left: LineStyle.None  , right: LineStyle.Dotted);

            #region header
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,           40, "Header"),
                new CellSetting(height,          115, nameof(CreditAgingList.CustomerCode)   , caption: "得意先コード"),
                new CellSetting(height,          130, nameof(CreditAgingList.CustomerName)   , caption: "得意先名"),
                new CellSetting(height,          80 , nameof(CreditAgingList.TotalText)      , caption: "回収条件"),
                new CellSetting(height,          130, nameof(CreditAgingList.CreditAmount)   , caption: "当月債権総額"),
                new CellSetting(height,          130, nameof(CreditAgingList.UnsettledRemain), caption: "当月末未決済残高"),
                new CellSetting(height,          130, nameof(CreditAgingList.BillingRemain)  , caption: "当月請求残高"),
                new CellSetting(height,  creditWidth, nameof(CreditAgingList.CreditLimit)    , caption: "与信限度額"),
                new CellSetting(height, balanceWidth, nameof(CreditAgingList.CreditBalance)  , caption: "当月与信残高"),
                new CellSetting(height,          130, nameof(CreditAgingList.ArrivalDueDate1), caption: ArrivalDueDate1),
                new CellSetting(height,          130, nameof(CreditAgingList.ArrivalDueDate2), caption: ArrivalDueDate2),
                new CellSetting(height,          130, nameof(CreditAgingList.ArrivalDueDate3), caption: ArrivalDueDate3),
                new CellSetting(height,          130, nameof(CreditAgingList.ArrivalDueDate4), caption: ArrivalDueDate4)

            });
            builder.BuildHeaderOnly(template);
            #endregion

            #region row
            builder.Items.Clear();
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,          40, "Header", border: bdLNonRThn, cell: builder.GetRowHeaderCell()),
                new CellSetting(height,    cbxWidth, "chk", readOnly: false, border: bdLNonRPal, cell: chkShow),
                new CellSetting(height,   codeWidth, nameof(CreditAgingList.CustomerCode)     , dataField: nameof(CreditAgingList.Code)             , border: bdLNonRThn, cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height,         130, nameof(CreditAgingList.CustomerName)     , dataField: nameof(CreditAgingList.Name)             , border: bdLNonRThn, cell: builder.GetTextBoxCell()),
                new CellSetting(height,          80, nameof(CreditAgingList.TotalText)        , dataField: nameof(CreditAgingList.TotalText)        , border: bdLNonRThn, cell: builder.GetTextBoxCell()),
                new CellSetting(height,         130, nameof(CreditAgingList.CreditAmount)     , dataField: nameof(CreditAgingList.CreditAmount)     , border: bdLNonRDbl, cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,         130, nameof(CreditAgingList.UnsettledRemain)  , dataField: nameof(CreditAgingList.UnsettledRemain)  , border: bdLDblRDbl, cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,         130, nameof(CreditAgingList.BillingRemain)    , dataField: nameof(CreditAgingList.BillingRemain)    , border: bdLDblRDbl, cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height, creditWidth, nameof(CreditAgingList.CreditLimit)      , dataField: nameof(CreditAgingList.CreditLimit)      , border: bdLDblRDtd, cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height, creditWidth, nameof(CreditAgingList.CreditBalance)    , dataField: nameof(CreditAgingList.CreditBalance)    , border: bdLNonRPal, cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,   markWidth, nameof(CreditAgingList.CreditBalanceMark), dataField: nameof(CreditAgingList.CreditBalanceMark), border: bdLNonRDbl, cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height,         130, nameof(CreditAgingList.ArrivalDueDate1)  , dataField: nameof(CreditAgingList.ArrivalDueDate1)  , border: bdLDblRDtd, cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,         130, nameof(CreditAgingList.ArrivalDueDate2)  , dataField: nameof(CreditAgingList.ArrivalDueDate2)  , border: bdLNonRDtd, cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,         130, nameof(CreditAgingList.ArrivalDueDate3)  , dataField: nameof(CreditAgingList.ArrivalDueDate3)  , border: bdLNonRDtd, cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,         130, nameof(CreditAgingList.ArrivalDueDate4)  , dataField: nameof(CreditAgingList.ArrivalDueDate4)  , border: bdLNonRThn, cell: builder.GetTextBoxCurrencyCell()),
            });
            builder.BuildRowOnly(template);
            #endregion
            grid.AllowAutoExtend = false;
            grid.Template = template;
            grid.FreezeLeftCellIndex = 4;
            grid.HideSelection = true;
        }

        private void SaveSettingValues()
        {
            Settings.SaveControlValue<PF0201>(Login, cbxDepartmentCode.Name, cbxDepartmentCode.Checked);
            Settings.SaveControlValue<PF0201>(Login, cbxStaff.Name, cbxStaff.Checked);
            Settings.SaveControlValue<PF0201>(Login, cbxCustomer.Name, cbxCustomer.Checked);
        }

        private void BindDueDate()
        {
            if (!datBaseDate.Value.HasValue) return;
            var dat = datBaseDate.Value.Value;
            if (dat.Year == 9999 && dat.Month >= 9) return;
            ArrivalDueDate1 = $"{dat.AddMonths(1):MM}月期日到来";
            ArrivalDueDate2 = $"{dat.AddMonths(2):MM}月期日到来";
            ArrivalDueDate3 = $"{dat.AddMonths(3):MM}月期日到来";
            ArrivalDueDate4 = $"{dat.AddMonths(4):MM}月以降期日到来";
        }

        #region event handlers

        private void CheckBox_EnabledCanged(object sender, EventArgs e)
        {
            var cbx = sender as CheckBox;
            if (cbx == null || cbx.Enabled) return;
            cbx.Checked = false;
        }

        private void cbxFilterPositiveCreditBalance_CheckedChanged(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (cbxFilterPositiveCreditBalance.Checked
                && GetReportSettingCustomerGroupType() != ReportCustomerGroup.PlainCusomter
                && GetReportSettingCreditLimitType() == ReportCreditLimitType.UseCustomerSummaryCredit)
            {
                rdoFilterWithCustomerGroup.Enabled = true;
                rdoFilterEachChildren.Enabled = true;
                return;
            }

            rdoFilterWithCustomerGroup.Enabled = false;
            rdoFilterEachChildren.Enabled = false;
            rdoFilterWithCustomerGroup.Checked = true;
        }

        private void txtDepartmentCodeFrom_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtDepartmentCodeFrom.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblDepartmentNameFrom.Clear();
            }
            else
            {
                var task = GetDepartmentAsync(code);
                try
                {
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    var dept = task.Result;
                    if (dept != null)
                        txtDepartmentCodeFrom.Text = dept.Code;
                    lblDepartmentNameFrom.Text = dept?.Name ?? string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }

            }
            if (cbxDepartmentCode.Checked)
            {
                txtDepartmentCodeTo.Text = txtDepartmentCodeFrom.Text;
                lblDepartmentNameTo.Text = lblDepartmentNameFrom.Text;
            }
        }

        private void txtDepartmentCodeTo_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtDepartmentCodeTo.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblDepartmentNameTo.Clear();
                return;
            }

            var task = GetDepartmentAsync(code);
            try
            {
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var dept = task.Result;
                if (dept != null)
                    txtDepartmentCodeTo.Text = dept.Code;
                lblDepartmentNameTo.Text = dept?.Name ?? string.Empty;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtStaffCodeFrom_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtStaffCodeFrom.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblStaffNameFrom.Clear();
            }
            else
            {
                var task = GetStaffAsync(code);
                try
                {
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    var staff = task.Result;
                    if (staff != null)
                        txtStaffCodeFrom.Text = staff.Code;
                    lblStaffNameFrom.Text = staff?.Name ?? string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }

            }
            if (cbxStaff.Checked)
            {
                txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                lblStaffNameTo.Text = lblStaffNameFrom.Text;
            }
        }

        private void txtStaffCodeTo_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtStaffCodeTo.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblStaffNameTo.Clear();
                return;
            }
            try
            {
                var task = GetStaffAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var staff = task.Result;
                if (staff != null)
                    txtStaffCodeTo.Text = staff.Code;
                lblStaffNameTo.Text = staff?.Name ?? string.Empty;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtCustomerCodeFrom.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblCustomerNameFrom.Clear();
            }
            else
            {
                var task = GetCustomerAsync(code);
                try
                {
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    var customer = task.Result;
                    if (customer != null)
                    {
                        txtCustomerCodeFrom.Text = customer.Code;
                    }
                    lblCustomerNameFrom.Text = customer?.Name ?? string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
            }
            if (cbxCustomer.Checked)
            {
                txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                lblCustomerNameTo.Text = lblCustomerNameFrom.Text;
            }
        }

        private void txtCustomerCodeTo_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtCustomerCodeTo.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblCustomerNameTo.Clear();
                return;
            }
            var task = GetCustomerAsync(code);
            try
            {
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var customer = task.Result;
                if (customer != null)
                    txtCustomerCodeTo.Text = customer.Code;
                lblCustomerNameTo.Text = customer?.Name ?? string.Empty;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnDepartmentCodeFrom_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var department = this.ShowDepartmentSearchDialog();
            if (department == null) return;
            txtDepartmentCodeFrom.Text = department.Code;
            lblDepartmentNameFrom.Text = department.Name;

            if (cbxDepartmentCode.Checked)
            {
                txtDepartmentCodeTo.Text = txtDepartmentCodeFrom.Text;
                lblDepartmentNameTo.Text = lblDepartmentNameFrom.Text;
            }
        }

        private void btnDepartmentCodeTo_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department == null) return;
            txtDepartmentCodeTo.Text = department.Code;
            lblDepartmentNameTo.Text = department.Name;
            ClearStatusMessage();
        }

        private void btnStaffCodeFrom_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff == null) return;
            txtStaffCodeFrom.Text = staff.Code;
            lblStaffNameFrom.Text = staff.Name;

            if (cbxStaff.Checked)
            {
                txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                lblStaffNameTo.Text = lblStaffNameFrom.Text;
            }
            ClearStatusMessage();
        }

        private void btnStaffCodeTo_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff == null) return;
            txtStaffCodeTo.Text = staff.Code;
            lblStaffNameTo.Text = staff.Name;
            ClearStatusMessage();
        }

        private void btnCustomerCodeFrom_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer == null) return;
            txtCustomerCodeFrom.Text = customer.Code;
            lblCustomerNameFrom.Text = customer.Name;

            if (cbxCustomer.Checked)
            {
                txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                lblCustomerNameTo.Text = lblCustomerNameFrom.Text;
            }
            ClearStatusMessage();
        }

        private void btnCustomerCodeTo_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer == null) return;
            txtCustomerCodeTo.Text = customer.Code;
            lblCustomerNameTo.Text = customer.Name;
            ClearStatusMessage();
        }

        private void txtClosingDate_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtClosingDate.Text))
            {
                txtClosingDate.Text = txtClosingDate.Text.PadLeft(2, '0');

                if (txtClosingDate.Text == "00")
                {
                    txtClosingDate.Text = "";
                    txtClosingDate.Select();
                    return;
                }

                if (int.Parse(txtClosingDate.Text) >= 28)
                {
                    txtClosingDate.Text = "99";
                }
            }
        }
        #endregion

        #region Webサービス呼び出し

        private async Task LoadReportSettingAsync()
        {
            var result = new ReportSettingsResult();
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReportSettingMasterClient>();
                result = await service.GetItemsAsync(
                   SessionKey, CompanyId, nameof(PF0201));

                if (result.ProcessResult.Result)
                {
                    ReportSettingList = result.ReportSettings;
                }
            });
        }

        private System.Action OnCancelHandler { get; set; }

        private async Task<List<CreditAgingList>> GetCreditAgingListAsync(CreditAgingListSearch searchOption)
        {
            List<CreditAgingList> result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))

            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<CreditAgingListServiceClient>(async client =>
                {
                    var getResult = await client.GetAsync(SessionKey, searchOption, hubConnection.ConnectionId);
                    if (getResult.ProcessResult.Result)
                        result = getResult.CreditAgingLists;
                });
            }
            return result;
        }

        private DateTime? GetLastCarryOverAt()
        {
            DateTime? lastCarryOverAt = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingBalanceServiceClient>();
                BillingBalanceResult result = await service.GetLastCarryOverAtAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    lastCarryOverAt = result.LastCarryOverAt;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return lastCarryOverAt;
        }

        private async Task<string> LoadInitialDirectoryAsync()
        {
            var serverPath = "";
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await service.GetByCodeAsync(
                        SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            return serverPath;
        }

        private async Task<Department> GetDepartmentAsync(string code)
        {
            Department result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<DepartmentMasterClient>();
                var getResult = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (getResult.ProcessResult.Result)
                    result = getResult.Departments?.FirstOrDefault();
            });
            return result;
        }

        private async Task<Staff> GetStaffAsync(string code)
        {
            Staff result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<StaffMasterClient>();
                var getResult = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (getResult.ProcessResult.Result)
                    result = getResult.Staffs?.FirstOrDefault();
            });
            return result;
        }

        private async Task<Customer> GetCustomerAsync(string code)
        {
            Customer result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CustomerMasterClient>();
                var getResult = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (getResult.ProcessResult.Result)
                    result = getResult.Customers?.FirstOrDefault();
            });
            return result;
        }

        #endregion

        #region　grid event handler

        private void grd_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            ClearStatusMessage();
            if (e.CellName != CellName("chk")) return;
            var row = grid.Rows[e.RowIndex];
            var rowData = row.DataBoundItem as CreditAgingList;
            if (rowData == null
                || rowData.ParentCustomerFlag == 0) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                grid.SuspendLayout();
                grid.CellEditedFormattedValueChanged -= grd_CellEditedFormattedValueChanged;

                var parentId = rowData.ParentCustomerId;
                var cbx = row[CellName("chk")] as CheckBoxCell;
                var visible = "+".Equals(cbx.Text);
                cbx.Text = visible ? "-" : "+";
                cbx.Value = cbx.Text;
                var step = e.RowIndex + 1;
                while (step < grid.RowCount)
                {
                    row = grid.Rows[step];
                    rowData = row.DataBoundItem as CreditAgingList;
                    if (parentId != rowData.ParentCustomerId) break;
                    row.Visible = visible;
                    step++;
                }
            }
            finally
            {
                grid.ResumeLayout(true);
                Cursor = Cursors.Default;
                grid.CellEditedFormattedValueChanged += grd_CellEditedFormattedValueChanged;
                ClearStatusMessage();
            }
        }
        private Color StaffTotalBackColor { get; } = Color.LightCyan;
        private Color DepartmentTotalBackColor { get; } = Color.NavajoWhite;
        private Color GrandTotalBackColor { get; } = Color.Khaki;
        private void grd_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.RowIndex < 0
                || e.CellName == CellName("Header")) return;

            var data = grid.Rows[e.RowIndex].DataBoundItem as CreditAgingList;
            if (data == null) return;
            if (data.RecordType == 0)
            {
                if (e.CellName == CellName(nameof(CreditAgingList.CustomerName))
                    && data.ParentCustomerId.HasValue
                    && data.ParentCustomerFlag == 0
                    && GetReportSettingCustomerGroupType() != ReportCustomerGroup.PlainCusomter)
                    e.Value = $" *{e.Value}";
            }
            else
            {
                e.CellStyle.BackColor =
                    data.RecordType == 1 ? StaffTotalBackColor :
                    data.RecordType == 2 ? DepartmentTotalBackColor :
                    data.RecordType == 3 ? GrandTotalBackColor : Color.Empty;
                if (e.CellName == CellName(nameof(CreditAgingList.TotalText)))
                    e.CellStyle.TextAlign = MultiRowContentAlignment.MiddleRight;
            }
        }
        private void grd_CellPainting(object sender, CellPaintingEventArgs e)
        {
            if (e.CellName.ToString() == CellName("chk"))
            {
                var data = grid.Rows[e.RowIndex].DataBoundItem as CreditAgingList;
                if (data == null) return;
                if (data.ParentCustomerFlag == 1
                    && data.RecordType == 0) return;

                var rect = e.CellBounds;
                e.PaintBackground(rect);
                e.PaintBorder(rect);
                e.PaintWaveLine(rect);
                e.Handled = true;

                var cbxCell = grid.Rows[e.RowIndex].Cells[CellName("chk")] as CheckBoxCell;
                cbxCell.Text = "";
            }
        }

        private void grd_DataBindingComplete(object sender, MultiRowBindingCompleteEventArgs e)
        {
            if (grid.RowCount == 0
            || (GetReportSettingCustomerGroupType() != ReportCustomerGroup.ParentOnly)) return;

            foreach (Row row in grid.Rows)
            {
                var data = row.DataBoundItem as CreditAgingList;
                if (data == null) continue;
                if (data?.ParentCustomerFlag == 0
                    && data.RecordType == 0
                    && data.ParentCustomerId.HasValue)
                    row.Visible = false;
            }
        }
        #endregion


        #region report settings

        private ReportTargetDate GetReportSettingTargetDate() => ReportSettingList.GetReportSetting<ReportTargetDate>(TargetDate);
        private ReportReceiptType GetReportSettingReceiptType() => ReportSettingList.GetReportSetting<ReportReceiptType>(ReceiptType);
        private ReportCustomerGroup GetReportSettingCustomerGroupType() => ReportSettingList.GetReportSetting<ReportCustomerGroup>(CustomerGroupType);
        private ReportStaffSelection GetReportSettingStaffSelection() => ReportSettingList.GetReportSetting<ReportStaffSelection>(StaffType);
        private ReportDoOrNot GetReportSettingDepartmentTotal() => ReportSettingList.GetReportSetting<ReportDoOrNot>(DepartmentTotal);
        private ReportDoOrNot GetReportSettingStaffTotal() => ReportSettingList.GetReportSetting<ReportDoOrNot>(StaffTotal);
        private ReportDoOrNot GetReportSettingDisplayCustomerCode() => ReportSettingList.GetReportSetting<ReportDoOrNot>(DisplayCustomerCode);
        private ReportUnitPrice GetReportSettingUnitPrice() => ReportSettingList.GetReportSetting<ReportUnitPrice>(UnitPrice);
        private ReportCreditLimitType GetReportSettingCreditLimitType() => ReportSettingList.GetReportSetting<ReportCreditLimitType>(CreditLimitType);

        #endregion

    }
}
