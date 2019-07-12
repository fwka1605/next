using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerLedgerService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
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
using static Rac.VOne.Client.Reports.Settings.PF0501;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>得意先別消込台帳</summary>
    public partial class PF0501 : VOneScreenBase
    {
        #region variable decleration

        private int CurrencyId { get; set; }
        private int CustomerID1 { get; set; }
        private int CustomerID2 { get; set; }

        /// <summary>外貨利用時の小数点以下桁数</summary>
        private int Precision { get; set; }
        private string CurrentParentCode { get; set; }
        private int CurrentParentCodeIndex { get; set; }

        private List<ReportSetting> ReportSettingList { get; set; } = new List<ReportSetting>();
        private List<string> ParentCusCodeList { get; set; }
        private List<CustomerLedger> CustomerLedgerData { get; set; }
        private List<CustomerLedger> GridDataForParentCd { get; set; }
        private string ServerPath { get; set; }
        private string CellName(string value) => $"cel{value}";
        private TaskProgressManager ProgressManager;
        public Customer Customer { get; set; }
        private bool IsCalledByOtherForm
            => Customer != null;


        #endregion

        #region 画面初期表示
        public PF0501()
        {
            InitializeComponent();
            grdCustomerLedger.SetupShortcutKeys();
            grdCustomerLedger.GridColorType = GridColorType.Special;
            Text = "得意先別消込台帳";
        }

        private void InitializeHandlers()
        {
            tbcCustomerLedger.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcCustomerLedger.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = Exit;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
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
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintReport;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Caption("設定");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = OpenPrintSetting;

            BaseContext.SetFunction08Caption("前へ");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = MoveToPrevious;

            BaseContext.SetFunction09Caption("次へ");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = MoveToNext;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        private void PF0501_Load(object sender, EventArgs e)
        {
            SetScreenName();

            try
            {
                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SuspendLayout();
                tbcCustomerLedger.SelectedIndex = 1;
                tbcCustomerLedger.SelectedIndex = 0;
                ResumeLayout();

                InitializeControlsFormat();

                ClearData();

                Settings.SetCheckBoxValue<PF0501>(Login, cbxCustomer);

                if (IsCalledByOtherForm)
                {
                    txtCustomerCodeFrom.Text = Customer.Code;
                    txtCustomerCodeTo.Text = Customer.Code;
                    lblDisCustomerCodeFrom.Text = Customer.Name;
                    lblDisCustomerCodeTo.Text = Customer.Name;
                    datClosingFrom.Value = new DateTime(2000, 1, 1);
                    datClosingTo.Value = new DateTime(3000, 1, 1);

                    tbcCustomerLedger.Selecting += (sdr, ev) =>
                    {
                        if (ev.TabPageIndex != 1)
                            ev.Cancel = true;
                    };

                    Search();

                    if (grdCustomerLedger.Rows.Count == 0)
                        BaseForm.Close();
                    BaseContext.SetFunction01Enabled(false);
                    BaseContext.SetFunction02Enabled(false);
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction06Enabled(false);
                }
                else
                {
                    InitializeHandlers();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeGrid(CustomerLedgerSearch option)
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var template = new Template();

            var dateWidth = 80;
            var currencyWidth = UseForeignCurrency ? 40 : 0;
            var sectionWidth = (UseSection && option.DisplaySection) ? 115 : 0;
            var departmentWidth = (!option.RequireBillingSubtotal
                && !option.RequireBillingInputIdSubotal && option.DisplayDepartment) ? 115 : 0;
            var invoiceCodeWidth = (option.DisplayInvoiceCode) ? 115 : 0;
            var categoryWidth = (option.DisplayBillingCategory) ? 115 : 0;
            var accountTitleWidth = (option.DisplayDebitAccountTitle) ? 115 : 0;
            var symbolWidth = (option.DisplayMatchingSymbol) ? 70 : 0;
            var amountWidth = option.DisplayMatchingSymbol ? 185 : 115;
            var slipTotalWidth = (option.DisplaySlipTotal) ? 115 : 0;
            var receiptWidth = (option.RequireReceiptData) ? 115 : 0;

            var borderLeftDotted = builder.GetBorder(
                left: option.DisplayMatchingSymbol
                ? LineStyle.Dotted
                : LineStyle.Thin);

            var borderRightDotted = builder.GetBorder(
                right: LineStyle.Dotted);

            var cellRecordedAt = builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter);
            cellRecordedAt.AlternateText.DisplayNull.Text = " ";

            builder.Items.AddRange(new CellSetting[]
           {
                # region ヘッダー1
                new CellSetting(height,                30, "Header"),
                new CellSetting(height,         dateWidth, nameof(CustomerLedger.RecordedAt)      , caption: "日付"),
                new CellSetting(height,      sectionWidth, nameof(CustomerLedger.SectionName)     , caption: "入金部門"),
                new CellSetting(height,   departmentWidth, nameof(CustomerLedger.DepartmentName)  , caption: "請求部門"),
                new CellSetting(height,  invoiceCodeWidth, nameof(CustomerLedger.InvoiceCode)     , caption: "請求書番号"),
                new CellSetting(height,     categoryWidth, nameof(CustomerLedger.CategoryName)    , caption: "区分"),
                new CellSetting(height, accountTitleWidth, nameof(CustomerLedger.AccountTitleName), caption: "債権科目"),
                new CellSetting(height,     currencyWidth, nameof(ArrearagesList.CurrencyCode)    , caption: "通貨"),
                new CellSetting(height,       amountWidth, nameof(CustomerLedger.BillingAmount)   , caption: "請求金額"),
                new CellSetting(height,    slipTotalWidth, nameof(CustomerLedger.SlipTotal)       , caption: "伝票合計"),
                new CellSetting(height,      receiptWidth, nameof(CustomerLedger.ReceiptAmount)   , caption: "入金額"),
                new CellSetting(height,       amountWidth, nameof(CustomerLedger.MatchingAmount)  , caption: "消込額"),
                new CellSetting(height,               115, nameof(CustomerLedger.RemainAmount)    , caption: "残高"),
                new CellSetting(height,               115, nameof(ArrearagesList.CustomerCode)    , caption: "得意先コード"),
                new CellSetting(height,               115, nameof(CustomerLedger.CustomerName)    , caption: "得意先名"),
                #endregion
           });

            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                #region データ1
                new CellSetting(height,                30, "Header"                 , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,         dateWidth, nameof(CustomerLedger.RecordedAt)           , dataField: nameof(CustomerLedger.RecordedAt)           , cell: cellRecordedAt),
                new CellSetting(height,      sectionWidth, nameof(CustomerLedger.SectionName)          , dataField: nameof(CustomerLedger.SectionName)          ),
                new CellSetting(height,   departmentWidth, nameof(CustomerLedger.DepartmentName)       , dataField: nameof(CustomerLedger.DepartmentName)       ),
                new CellSetting(height,  invoiceCodeWidth, nameof(CustomerLedger.InvoiceCode)          , dataField: nameof(CustomerLedger.InvoiceCode)          ),
                new CellSetting(height,     categoryWidth, nameof(CustomerLedger.CategoryName)         , dataField: nameof(CustomerLedger.CategoryName)         ),
                new CellSetting(height, accountTitleWidth, nameof(CustomerLedger.AccountTitleName)     , dataField: nameof(CustomerLedger.AccountTitleName)     ),
                new CellSetting(height,     currencyWidth, nameof(ArrearagesList.CurrencyCode)         , dataField: nameof(ArrearagesList.CurrencyCode)         , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height,       symbolWidth, nameof(CustomerLedger.MatchingSymbolBilling), dataField: nameof(CustomerLedger.MatchingSymbolBilling), cell: builder.GetTextBoxCell(), border: borderRightDotted),
                new CellSetting(height,               115, nameof(CustomerLedger.BillingAmount)        , dataField: nameof(CustomerLedger.BillingAmount)        , cell: builder.GetTextBoxCurrencyCell(Precision), border: borderLeftDotted),
                new CellSetting(height,    slipTotalWidth, nameof(CustomerLedger.SlipTotal)            , dataField: nameof(CustomerLedger.SlipTotal)            , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height,      receiptWidth, nameof(CustomerLedger.ReceiptAmount)        , dataField: nameof(CustomerLedger.ReceiptAmount)        , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height,       symbolWidth, nameof(CustomerLedger.MatchingSymbolReceipt), dataField: nameof(CustomerLedger.MatchingSymbolReceipt), cell: builder.GetTextBoxCell(), border: borderRightDotted),
                new CellSetting(height,               115, nameof(CustomerLedger.MatchingAmount)       , dataField: nameof(CustomerLedger.MatchingAmount)       , cell: builder.GetTextBoxCurrencyCell(Precision), border: borderLeftDotted),
                new CellSetting(height,               115, nameof(CustomerLedger.RemainAmount)         , dataField: nameof(CustomerLedger.RemainAmount)         , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height,               115, nameof(ArrearagesList.CustomerCode)         , dataField: nameof(ArrearagesList.CustomerCode)         , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height,               115, nameof(CustomerLedger.CustomerName)         , dataField: nameof(CustomerLedger.CustomerName)         , cell: builder.GetTextBoxCell()),
                #endregion
            });

            builder.BuildRowOnly(template);
            var spanDateWidth = dateWidth + sectionWidth + departmentWidth + invoiceCodeWidth + categoryWidth + accountTitleWidth;
            var cellSpan = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight);
            cellSpan.Size = new Size(spanDateWidth, height);
            cellSpan.Name = CellName("AtSpan");
            cellSpan.Location = new Point(30, cellSpan.Top);
            cellSpan.Visible = false;
            cellSpan.Style.Border = ColorContext.GridLine();
            cellSpan.ReadOnly = true;
            template.Row.Cells.Add(cellSpan);

            grdCustomerLedger.Template = template;

            grdCustomerLedger.AllowAutoExtend = false;
            grdCustomerLedger.HideSelection = true;

        }

        #endregion

        #region FunctionKeyEvent
        [OperationLog("照会")]
        private void Search()
        {
            ClearStatusMessage();

            if (!ValidateForSearch())
                return;
            try
            {
                ProgressDialog.Start(ParentForm, LoadReportSettingAsync(), false, SessionKey);
                var option = GetSearchOption();

                var list = GetBaseTasks(option);
                list.Add(new TaskProgress($"{ParentForm.Text} 画面表示"));

                ProgressManager = new TaskProgressManager(list);
                var task = LoadCustomerLedgerAsync(option);
                NLogHandler.WriteDebug(this, "得意先別消込台帳 照会開始");
                ProgressDialogStart(ParentForm, ParentForm.Text, task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                NLogHandler.WriteDebug(this, "得意先別消込台帳 照会終了");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private List<TaskProgress> GetBaseTasks(CustomerLedgerSearch option)
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求データ抽出"));
            if (option.RequireReceiptData)
                list.Add(new TaskProgress($"{ParentForm.Text} 入金データ抽出"));
            list.Add(new TaskProgress($"{ParentForm.Text} 消込データ抽出"));
            list.Add(new TaskProgress($"{ParentForm.Text} 元帳データ取得"));
            list.Add(new TaskProgress($"{ParentForm.Text} 繰越データ取得"));
            if (option.DisplayMatchingSymbol)
                list.Add(new TaskProgress($"{ParentForm.Text} 消込キー情報取得"));
            list.Add(new TaskProgress($"{ParentForm.Text} 元帳データ整形"));
            return list;
        }

        private async Task LoadCustomerLedgerAsync(CustomerLedgerSearch option)
        {            
            CustomerLedgerData = await GetCustomerLedgerDataAsync(option);
            if (CustomerLedgerData == null)
            {
                grdCustomerLedger.Template = null;
                ProgressManager.Canceled = true;
                ProgressManager.UpdateState();
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            if (!CustomerLedgerData.Any())
            {
                grdCustomerLedger.Template = null;
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                ShowWarningDialog(MsgWngNotExistSearchData);

                return;           }
            else 
                {
                BaseContext.SetFunction07Enabled(false);
            }

            InitializeGrid(option);

            ParentCusCodeList = CustomerLedgerData.OrderBy(x => x.ParentCustomerCode)
                .GroupBy(x => x.ParentCustomerCode)
                .Select(g => g.Key).ToList();

            CurrentParentCode = ParentCusCodeList.FirstOrDefault();
            CurrentParentCodeIndex = 0;

            if (ReportSettingList != null && ReportSettingList.Any())
            {
                if (!UseForeignCurrency)
                    lblDispReportUnitPrice.Text = ReportSettingList[8].ItemValue;

                lblDispReportAdvancedReceivedType.Text = ReportSettingList[5].ItemValue;
                lblDispReportDoOrNot.Text = ReportSettingList[4].ItemValue;
                lblDispReportTargetDate.Text = ReportSettingList[0].ItemValue;
                lblDispReportTotal.Text = ReportSettingList[3].ItemValue;
                lblDispReportTotalByDay.Text = ReportSettingList[1].ItemValue;               
            }

            await BindGridData();
            ProgressManager.UpdateState();
         
            SetF8F9Enabled(CurrentParentCode);
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearData();            
        }

      

    

        [OperationLog("印刷")]
        private void PrintReport()
        {
            try
            {
                if (!ValidateForSearch())
                    return;

                ClearStatusMessage();

                ProgressDialog.Start(ParentForm, LoadReportSettingAsync(), false, SessionKey);
                var option = GetSearchOption();
                option.IsPrint = true;
                var list = GetBaseTasks(option);
                list.Add(new TaskProgress("帳票の作成"));
                ProgressManager = new TaskProgressManager(list);
                var task = GetCustomerLedgerReportAsync(option);
                var dialogResult = ProgressDialogStart(ParentForm, ParentForm.Text + " 印刷", task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                if (dialogResult != DialogResult.OK) return;
                var report = task.Result;
                if (report == null) return;
                ShowDialogPreview(ParentForm, report, ServerPath);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private async Task<CustomerLedgerSectionReport> GetCustomerLedgerReportAsync(CustomerLedgerSearch option )
        {
            var source = await GetCustomerLedgerDataAsync(option);
            if (source == null)
            {
                ProgressManager.Canceled = true;
                DispStatusMessage(MsgInfProcessCanceled);
                return null;
            }
            if (!source.Any())
            {
                ProgressManager.NotFind();
                ShowWarningDialog(MsgWngNotExistSearchData);
                return null;
            }

            CustomerLedgerSectionReport report = null;
            await Task.Run(() => report = CreateCustomerLedgerReport(source, option));

            if (ServerPath == null)
                await LoadServerPathAsync();

            if (!Directory.Exists(ServerPath))
                ServerPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ProgressManager.UpdateState();

            return report;
        }

        private CustomerLedgerSectionReport CreateCustomerLedgerReport(List<CustomerLedger> source, CustomerLedgerSearch option)
        {
            var report = new CustomerLedgerSectionReport();
            report.SearchOption = option;
            report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
            report.Name = "得意先別消込台帳" + DateTime.Today.ToString("yyyyMMdd");
            report.SetData(source, Precision);

            var subReport = new CustomerLedgerSearchConditionSectionReport();
            subReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "得意先別消込台帳");
            subReport.Name = "得意先別消込台帳" + DateTime.Today.ToString("yyyyMMdd");
            var subReportData = GetSubReportData();
            subReport.SetPageDataSetting(subReportData);

            report.Run(false);
            subReport.SetPageNumber(report.Document.Pages.Count);
            subReport.Run(false);

            report.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)subReport.Document.Pages.Clone());

            return report;
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            ClearStatusMessage();

            if (!ValidateForSearch())
                return;

            try
            {
                ProgressDialog.Start(ParentForm, LoadServerPathAsync(), false, SessionKey);

                if (!Directory.Exists(ServerPath))
                    ServerPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                var filePath = string.Empty;
                var fileName = $"得意先別消込台帳{datClosingFrom.Value:yyMM}_{datClosingTo.Value:yyMM}.csv";

                if (!ShowSaveExportFileDialog(ServerPath, fileName, out filePath)) return;

                ProgressDialog.Start(ParentForm, LoadReportSettingAsync(), false, SessionKey);
                var option = GetSearchOption();

                var list = GetBaseTasks(option);
                list.Add(new TaskProgress("データ出力"));
                ProgressManager = new TaskProgressManager(list);
                var task = ExportCustomerLedgerAsync(option, filePath);
                var dialogResult = ProgressDialogStart(ParentForm, ParentForm.Text + " エクスポート", task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                if (dialogResult != DialogResult.OK)
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                if (task.Result == 0)
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }
                DispStatusMessage(MsgInfFinishExport);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<int> ExportCustomerLedgerAsync(CustomerLedgerSearch option, string filePath)
        {
            var exportCusLedger = await GetCustomerLedgerDataAsync(option);

            if (!exportCusLedger.Any())
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                return 0;
            }

            var exportList = new List<CustomerLedger>();

            var total = "総合計";
            var balance = "繰越";

            for (var i = 0; i < exportCusLedger.Count; i++)
            {
                if (exportCusLedger[i].RecordType == 0)
                    exportCusLedger[i].RecordTypeName = exportCusLedger[i].RecordedAt?.ToShortDateString();

                if (exportCusLedger[i].RecordType == 1
                    || exportCusLedger[i].RecordType == 2
                    || exportCusLedger[i].RecordType == 3)
                {
                    exportCusLedger[i].RecordedAt = null;
                    exportCusLedger[i].ParentCustomerCode = null;
                    exportCusLedger[i].ParentCustomerName = null;

                    if (exportCusLedger[i].RecordType == 1)
                        exportCusLedger[i].RecordTypeName = balance;
                    else if (exportCusLedger[i].RecordType == 2)
                        exportCusLedger[i].RecordTypeName = $"{exportCusLedger[i].YearMonth:yyyy/MM} 合計";
                    else
                        exportCusLedger[i].RecordTypeName = total;
                }

                if (!UseForeignCurrency)
                {
                    if (exportCusLedger[i].BillingAmount < 1)
                        exportCusLedger[i].BillingAmount = 0;

                    if (exportCusLedger[i].ReceiptAmount < 1)
                        exportCusLedger[i].ReceiptAmount = 0;

                    if (exportCusLedger[i].RemainAmount < 1)
                        exportCusLedger[i].RemainAmount = 0;

                    if (exportCusLedger[i].MatchingAmount < 1)
                        exportCusLedger[i].MatchingAmount = 0;

                    if (exportCusLedger[i].SlipTotal < 1)
                        exportCusLedger[i].SlipTotal = 0;
                }
            }

            var definition = new CustomerLedgerFileDefinition(new DataExpression(ApplicationControl));

            if (definition.CurrencyCodeField.Ignored = (!UseForeignCurrency))
                definition.CurrencyCodeField.FieldName = null;

            if (definition.SectionNameField.Ignored = !(UseSection && !option.DoGroupReceipt && option.DisplaySection))
                definition.SectionNameField.FieldName = null;

            if (definition.DepartmentNameField.Ignored = !(!option.RequireBillingSubtotal
                && !option.RequireBillingInputIdSubotal && option.DisplayDepartment))
                definition.DepartmentNameField.FieldName = null;

            if (definition.InvoiceCodeField.Ignored = !(option.DisplayInvoiceCode))
                definition.InvoiceCodeField.FieldName = null;

            if (definition.CategoryNameField.Ignored = !(option.DisplayBillingCategory))
                definition.CategoryNameField.FieldName = null;

            if (definition.DebitAccountTitleNameField.Ignored = !(option.DisplayDebitAccountTitle))
                definition.DebitAccountTitleNameField.FieldName = null;

            if (definition.MatchingSymbolBillingField.Ignored = !(option.DisplayMatchingSymbol))
                definition.MatchingSymbolBillingField.FieldName = null;

            if (definition.MatchingSymbolReceiptField.Ignored = !(option.DisplayMatchingSymbol))
                definition.MatchingSymbolReceiptField.FieldName = null;

            if (definition.SlipTotalField.Ignored = !(option.DisplaySlipTotal))
                definition.SlipTotalField.FieldName = null;

            if (definition.ReceiptAmountField.Ignored = !(option.RequireReceiptData))
                definition.ReceiptAmountField.FieldName = null;

            var format = "0" + ((Precision == 0) ? string.Empty : "." + new string('0', Precision));

            definition.ReceiptAmountField.Format = value => value.ToString(format);
            definition.RemainAmountField.Format = value => value.ToString(format);
            definition.MatchingAmountField.Format = value => value.ToString(format);
            definition.BillingAmountField.Format = value => value.ToString(format);
            definition.SlipTotalField.Format = value => value.ToString(format);

            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = Login.CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            ProgressDialog.Start(ParentForm, (cancel, progress) =>
            {
                return exporter.ExportAsync(filePath, exportCusLedger, cancel, progress);

            }, true, SessionKey);

            var result = await exporter.ExportAsync(filePath, exportCusLedger, new System.Threading.CancellationToken(false), new Progress<int>());
            if (result == 0) ProgressManager.Abort();
            ProgressManager.UpdateState();
            return result;
        }

        [OperationLog("設定")]
        private void OpenPrintSetting()
        {
            ClearStatusMessage();
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PF0501);
                screen.InitializeParentForm("帳票設定");
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }

        [OperationLog("前へ")]
        private void MoveToPrevious()
        {
            ClearStatusMessage();

            CurrentParentCodeIndex = CurrentParentCodeIndex - 1;

            try
            {
                ProgressDialog.Start(ParentForm, BindGridData(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("次へ")]
        private void MoveToNext()
        {
            ClearStatusMessage();

            CurrentParentCodeIndex = CurrentParentCodeIndex + 1;

            try
            {
                ProgressDialog.Start(ParentForm, BindGridData(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void Exit()
        {
            Settings.SaveControlValue<PF0501>(Login, cbxCustomer.Name, cbxCustomer.Checked);

            BaseForm.Close();
        }
        private void ReturnToSearchCondition()
        {
            tbcCustomerLedger.SelectedIndex = 0;
        }
        #endregion

        #region サブ処理

        private void InitializeControlsFormat()
        {
            if (ApplicationControl == null) return;

            var expression = new DataExpression(ApplicationControl);

            txtCustomerCodeFrom.Format      = expression.CustomerCodeFormatString;
            txtCustomerCodeFrom.MaxLength   = expression.CustomerCodeLength;
            txtCustomerCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;
            txtCustomerCodeFrom.ImeMode     = expression.CustomerCodeImeMode();

            txtCustomerCodeTo.Format        = expression.CustomerCodeFormatString;
            txtCustomerCodeTo.MaxLength     = expression.CustomerCodeLength;
            txtCustomerCodeTo.PaddingChar   = expression.CustomerCodePaddingChar;
            txtCustomerCodeTo.ImeMode       = expression.CustomerCodeImeMode();

            lblCurrencyCode.Visible = UseForeignCurrency;
            txtCurrencyCode.Visible = UseForeignCurrency;
            btnCurrency.Visible = UseForeignCurrency;

            txtClosingDate.PaddingChar = '0';

            nmbThreshold.Visible                    = !UseForeignCurrency;
            txtLessCollectCategoryName.Visible      = !UseForeignCurrency;
            txtGreaterCollectCategoryName1.Visible  = !UseForeignCurrency;
            nmbGreaterRate1.Visible                 = !UseForeignCurrency;
            txtGreaterRoundingMode1.Visible         = !UseForeignCurrency;
            txtGreaterCollectCategoryName2.Visible  = !UseForeignCurrency;
            nmbGreaterRate2.Visible                 = !UseForeignCurrency;
            txtGreaterRoundingMode2.Visible         = !UseForeignCurrency;
            txtGreaterCollectCategoryName3.Visible  = !UseForeignCurrency;
            nmbGreaterRate3.Visible                 = !UseForeignCurrency;
            txtGreaterRoundingMode3.Visible         = !UseForeignCurrency;
            lblLessThan.Visible                     = !UseForeignCurrency;
            lblPercent1.Visible                     = !UseForeignCurrency;
            lblPercent2.Visible                     = !UseForeignCurrency;
            lblPercent3.Visible                     = !UseForeignCurrency;
            lblAbove1.Visible                       = !UseForeignCurrency;
            lblAbove2.Visible                       = !UseForeignCurrency;
            lblAbove3.Visible                       = !UseForeignCurrency;

            lblReportUnitPrice.Visible              = !UseForeignCurrency;
            lblDispReportUnitPrice.Visible          = !UseForeignCurrency;
            
        }

        private void ClearData()
        {
            ClearStatusMessage();

            txtCustomerCodeFrom.Clear();
            lblDisCustomerCodeFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblDisCustomerCodeTo.Clear();

            datClosingFrom.Clear();
            datClosingTo.Clear();

            rdoAccountBalance.Select();
            txtClosingDate.Clear();
            txtClosingDate.Enabled = false;

            if (UseForeignCurrency)
            {
                CurrencyId = 0;
                txtCurrencyCode.Clear();
            }
            else
            {
                nmbThreshold.Clear();
                txtLessCollectCategoryName.Clear();
                txtGreaterCollectCategoryName1.Clear();
                nmbGreaterRate1.Clear();
                txtGreaterRoundingMode1.Clear();
                txtGreaterCollectCategoryName2.Clear();
                nmbGreaterRate2.Clear();
                txtGreaterRoundingMode2.Clear();
                txtGreaterCollectCategoryName3.Clear();
                nmbGreaterRate3.Clear();
                txtGreaterRoundingMode3.Clear();

                lblDispReportUnitPrice.Clear();

                txtCurrencyCode.Text = Rac.VOne.Common.Constants.DefaultCurrencyCode;
                txtCurrencyCode_Validated(txtCurrencyCode, EventArgs.Empty);

            }

            lblDispReportAdvancedReceivedType.Clear();
            lblDispReportDoOrNot.Clear();
            lblDispReportTargetDate.Clear();
            lblDispReportTotal.Clear();
            lblDispReportTotalByDay.Clear();

            grdCustomerLedger.DataSource = null;
            grdCustomerLedger.Template = null;

            tbcCustomerLedger.SelectedIndex = 0;

            txtCustomerCode.Clear();
            lblDispCustomerName.Clear();
            txtCategoryName.Clear();

            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            tbcCustomerLedger.SelectedIndex = 0;
            BaseContext.SetFunction07Enabled(true);

            this.ActiveControl = txtCustomerCodeFrom;
        }

        private bool ValidateForSearch()
        {
            if (string.IsNullOrEmpty(txtCustomerCodeFrom.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "得意先コード");
                txtCustomerCodeFrom.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtCustomerCodeTo.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "得意先コード");
                txtCustomerCodeTo.Select();
                return false;
            }

            if (!datClosingFrom.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "処理月");
                datClosingFrom.Select();
                return false;
            }

            if (!datClosingTo.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "処理月");
                datClosingTo.Select();
                return false;
            }

            if (!datClosingFrom.ValidateRange(datClosingTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "処理月"))) return false;

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCode.Text))) return false;

            if (rdoCustomerBalance.Checked
                && txtClosingDate.Enabled
                && string.IsNullOrEmpty(txtClosingDate.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblClosingDate.Text);
                txtClosingDate.Select();
                return false;
            }

            if (UseForeignCurrency
                && string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                txtCurrencyCode.Select();
                ShowWarningDialog(MsgWngInputRequired, lblCurrencyCode.Text);
                return false;

            }

            return true;
        }

        private void SetF8F9Enabled(string CurrentParentCode)
        {
            var F8Enabled = false;
            var F9Enabled = false;

            if (ParentCusCodeList != null)
            {
                for (var i = 0; i < ParentCusCodeList.Count; i++)
                {
                    if (CurrentParentCode == ParentCusCodeList[i])
                    {
                        CurrentParentCodeIndex = i;
                        break;
                    }
                }

                if (CurrentParentCodeIndex >= 0 && ParentCusCodeList.Count > 1)
                {
                    if (CurrentParentCodeIndex >= 1)
                        F8Enabled = true;
                    if (CurrentParentCodeIndex >= 0 && CurrentParentCodeIndex < (ParentCusCodeList.Count - 1))
                        F9Enabled = true;
                }
            }

            BaseContext.SetFunction08Enabled(F8Enabled);
            BaseContext.SetFunction09Enabled(F9Enabled);
        }

        private CustomerLedgerSearch GetSearchOption()
        {
            var option = new CustomerLedgerSearch();

            if (!string.IsNullOrEmpty(txtCustomerCodeFrom.Text))
                option.CustomerCodeFrom = txtCustomerCodeFrom.Text;
            if (!string.IsNullOrEmpty(txtCustomerCodeTo.Text))
                option.CustomerCodeTo = txtCustomerCodeTo.Text;
            if (!string.IsNullOrEmpty(txtClosingDate.Text))
                option.ClosingDay = int.Parse(txtClosingDate.Text);

            option.InitializeYearMonth(option.ClosingDay ?? Company.ClosingDay,
                datClosingFrom.Value.Value, datClosingTo.Value.Value);


            option.RemainType = (int)GetReportSettingRemainType();
            option.CompanyId = CompanyId;
            option.CurrencyId = CurrencyId;
            option.UseBilledAt = GetReportSettingTargetDate() == ReportTargetDate.BilledAt;
            option.GroupBillingType = (int)GetReportSettingTotalByDay();
            option.BillingSlipType = (int)GetReportSettingSlipTotal();
            option.DoGroupReceipt = GetReportSettingReceiptGrouping() == ReportDoOrNot.Do;
            option.DisplaySection = GetReportSettingDisplaySection() == ReportDoOrNot.Do;
            option.DisplayDepartment = GetReportSettingDisplayDepartment() == ReportDoOrNot.Do;
            option.DisplayMatchingSymbol = GetReportSettingDisplaySymbol() == ReportDoOrNot.Do;
            option.RequireMonthlyBreak = GetReportSettingMonthlyBreak() == ReportDoOrNot.Do;
            var unit = GetReportSettingUnitPrice();
            option.UnitPrice =
                unit == ReportUnitPrice.Per1     ? 1M     :
                unit == ReportUnitPrice.Per1000  ? 1000M  :
                unit == ReportUnitPrice.Per10000 ? 10000M : 1000000M;
            return option;
        }

        private List<object> GetSubReportData()
        {
            var searchReport = new List<object>();
            var waveDash = " ～ ";

            var customerCodeFrom = txtCustomerCodeFrom.GetPrintValueCode(lblDisCustomerCodeFrom);
            var customerCodeTo = txtCustomerCodeTo.GetPrintValueCode(lblDisCustomerCodeTo);
            searchReport.Add(new SearchData("得意先コード", customerCodeFrom + waveDash + customerCodeTo));

            var closingFrom = datClosingFrom.GetPrintValue();
            var closingTo = datClosingTo.GetPrintValue();
            searchReport.Add(new SearchData("処理月", closingFrom + waveDash + closingTo));

            var rdoType = rdoAccountBalance.Checked ? "会計帳簿の締日残高" : "得意先ごとの締日残高";
            searchReport.Add(new SearchData("残高タイプ", rdoType));

            var ClosingDate = "";
            if (!string.IsNullOrEmpty(txtClosingDate.Text))
                ClosingDate = txtClosingDate.Text;
            searchReport.Add(new SearchData("締日", ClosingDate));

            if (UseForeignCurrency)
            {
                var currency = "(指定なし)";
                if (!string.IsNullOrEmpty(txtCurrencyCode.Text))
                    currency = txtCurrencyCode.Text;
                searchReport.Add(new SearchData("通貨コード", currency));
            }

            var billiOrSaleDate = GetReportSettingNameTargetDate();
            searchReport.Add(new SearchData("集計基準日", billiOrSaleDate));

            var billingRemain = GetReportSettingNameBillingRemainType();
            searchReport.Add(new SearchData("請求残高計算方法", billingRemain));

            if (!UseForeignCurrency)
            {
                var unit = GetReportSettingNameUnitPrice();
                searchReport.Add(new SearchData("金額単位", unit));
            }

            return searchReport;
        }



        private Customer GetCustomerData(string customerCode)
        {
            var task = GetCustomerAsync(customerCode);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        private async Task BindCusPaymentContractData(List<CustomerLedger> bindData)
        {
            txtCustomerCode.Text = bindData[0].ParentCustomerCode;
            lblDispCustomerName.Text = bindData[0].ParentCustomerName;

            CustomerPaymentContract cusPaymentContract =
                    await GetCustomerPaymentDataAsync(bindData[0].ParentCustomerId);

            if (cusPaymentContract != null)
            {
                txtCategoryName.Text = cusPaymentContract.CollectCategoryName;

                if (!UseForeignCurrency)
                {
                    var threshold = cusPaymentContract.ThresholdValue.ToString();
                    nmbThreshold.Value = threshold != "" ? Convert.ToDecimal(threshold.Substring(0, threshold.IndexOf("."))) : 0;
                    txtLessCollectCategoryName.Text = cusPaymentContract.LessThanName;
                    txtGreaterCollectCategoryName1.Text = cusPaymentContract.GreaterThanName1;
                    txtGreaterCollectCategoryName2.Text = cusPaymentContract.GreaterThanName2;
                    txtGreaterCollectCategoryName3.Text = cusPaymentContract.GreaterThanName3;
                    nmbGreaterRate1.Text = cusPaymentContract.GreaterThanRate1.ToString();
                    nmbGreaterRate2.Text = cusPaymentContract.GreaterThanRate2.ToString();
                    nmbGreaterRate3.Text = cusPaymentContract.GreaterThanRate3.ToString();
                    txtGreaterRoundingMode1.Text = cusPaymentContract.GreaterThanRoundingMode1 == 0 ? "端数処理" : "";
                    txtGreaterRoundingMode2.Text = cusPaymentContract.GreaterThanRoundingMode2 == 0 ? "端数処理" : "";
                    txtGreaterRoundingMode3.Text = cusPaymentContract.GreaterThanRoundingMode3 == 0 ? "端数処理" : "";
                }
            }
            else
            {
                Customer customer =
                    await GetCustomerAsync(bindData[0].ParentCustomerCode);

                if (customer != null)
                    txtCategoryName.Text = customer.CollectCategoryName;
                else
                    txtCategoryName.Clear();

                nmbThreshold.Clear();
                txtLessCollectCategoryName.Clear();
                txtGreaterCollectCategoryName1.Clear();
                txtGreaterCollectCategoryName2.Clear();
                txtGreaterCollectCategoryName3.Clear();
                nmbGreaterRate1.Clear();
                nmbGreaterRate2.Clear();
                nmbGreaterRate3.Clear();
                txtGreaterRoundingMode1.Clear();
                txtGreaterRoundingMode2.Clear();
                txtGreaterRoundingMode3.Clear();
            }
        }

        private async Task BindGridData()
        {
            CurrentParentCode = ParentCusCodeList[CurrentParentCodeIndex];

            var gridBindData = CustomerLedgerData.Where(x => x.ParentCustomerCode == CurrentParentCode).ToList();
            GridDataForParentCd = gridBindData;

            await BindCusPaymentContractData(gridBindData);

            grdCustomerLedger.DataSource = new BindingSource(gridBindData, null);
            tbcCustomerLedger.SelectedIndex = 1;

            grdCustomerLedger.Rows[0].Selected = false;

            SetF8F9Enabled(CurrentParentCode);
        }

        #endregion

        #region Webサービス呼び出し
        private async Task LoadServerPathAsync()
            => ServerPath = await Util.GetGeneralSettingServerPathAsync(Login);

        private async Task LoadReportSettingAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReportSettingMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId, nameof(PF0501));

                if (result.ProcessResult.Result)
                    ReportSettingList = result.ReportSettings.OrderBy(r => r.DisplayOrder).ToList();
            });
        }
        private System.Action OnCancelHandler { get; set; }
        private async Task<List<CustomerLedger>> GetCustomerLedgerDataAsync(CustomerLedgerSearch option)
        {
            List<CustomerLedger> searchResult = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync(async (CustomerLedgerServiceClient client) =>
                {
                    var result = await client.GetAsync(SessionKey, option, hubConnection.ConnectionId);
                    if (result.ProcessResult.Result)
                        searchResult = result.CustomerLedgers;
                });
            }
            return searchResult;
        }

        private async Task<CustomerPaymentContract> GetCustomerPaymentDataAsync(int customerId)
        {
            CustomerPaymentContract searchResult = null;

            CustomerPaymentContractsResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                result = await service.GetPaymentContractAsync(SessionKey, new int[] { customerId });

                if (result.ProcessResult.Result && result.Payments.Any())
                {
                    searchResult = result.Payments[0];
                }
            });

            return searchResult;
        }

        private async Task<Customer> GetCustomerAsync(string customerCode)
        {
            Customer searchResult = null;

            CustomersResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { customerCode });

                if (result.ProcessResult.Result && result.Customers.Any())
                {
                    searchResult = result.Customers[0];
                }
            });

            return searchResult;
        }

        #endregion

        #region event handlers
        private void btnCurrency_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                ClearStatusMessage();
                txtCurrencyCode.Text = currency.Code;
                CurrencyId = currency.Id;
                Precision = currency.Precision;
            }
        }

        private void btnCustomerCode1_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeFrom.Text = customer.Code;
                lblDisCustomerCodeFrom.Text = customer.Name;
                if (cbxCustomer.Checked)
                {
                    txtCustomerCodeTo.Text = customer.Code;
                    lblDisCustomerCodeTo.Text = customer.Name;
                }

                CustomerID1 = customer.Id;
                ClearStatusMessage();
            }
        }

        private void btnCustomerCode2_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeTo.Text = customer.Code;
                lblDisCustomerCodeTo.Text = customer.Name;
                CustomerID2 = customer.Id;
                ClearStatusMessage();
            }
        }
        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            CurrenciesResult result = null;

            try
            {
                if (string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    ClearStatusMessage();
                    txtCurrencyCode.Clear();
                    CurrencyId = 0;
                    return;
                }

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CurrencyMasterClient>();
                    result = await service.GetByCodeAsync(
                            SessionKey,
                            CompanyId,
                            new string[] { txtCurrencyCode.Text });
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result == null || !result.Currencies.Any())
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);

                    txtCurrencyCode.Text = null;
                    CurrencyId = 0;
                    txtCurrencyCode.Select();
                }
                else
                {
                    ClearStatusMessage();
                    CurrencyId = result.Currencies[0].Id;
                    Precision = result.Currencies[0].Precision;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                var customerCode = txtCustomerCodeFrom.Text;

                // if Empty
                if (string.IsNullOrEmpty(customerCode)
                    || string.IsNullOrWhiteSpace(customerCode))
                {
                    if (cbxCustomer.Checked)
                    {
                        txtCustomerCodeTo.Text = string.Empty;
                        lblDisCustomerCodeTo.Text = string.Empty;
                        CustomerID2 = 0;
                    }
                    lblDisCustomerCodeFrom.Text = string.Empty;
                    CustomerID1 = 0;
                    ClearStatusMessage();
                    return;
                }

                var result = GetCustomerData(customerCode);

                if (result != null)
                {
                    if (cbxCustomer.Checked)
                    {
                        txtCustomerCodeFrom.Text = result.Code;
                        lblDisCustomerCodeFrom.Text = result.Name;
                        txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                        lblDisCustomerCodeTo.Text = result.Name;
                        CustomerID2 = result.Id;
                    }
                    else
                    {
                        txtCustomerCodeFrom.Text = result.Code;
                        lblDisCustomerCodeFrom.Text = result.Name;
                    }
                    CustomerID1 = result.Id;
                }
                else
                {
                    if (cbxCustomer.Checked)
                    {
                        txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                        lblDisCustomerCodeFrom.Clear();
                        lblDisCustomerCodeTo.Clear();
                    }
                    else
                    {
                        lblDisCustomerCodeFrom.Clear();
                    }
                    CustomerID1 = 0;
                    CustomerID2 = 0;
                }
                ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                var CustomerCode = txtCustomerCodeTo.Text;

                // if Empty
                if (string.IsNullOrEmpty(CustomerCode))
                {
                    lblDisCustomerCodeTo.Text = null;
                    CustomerID2 = 0;
                    ClearStatusMessage();
                    return;
                }

                var result = GetCustomerData(CustomerCode);

                if (result != null)
                {
                    txtCustomerCodeTo.Text = result.Code;
                    lblDisCustomerCodeTo.Text = result.Name;
                    CustomerID2 = result.Id;
                }
                else
                {
                    lblDisCustomerCodeTo.Clear();
                    CustomerID2 = 0;
                }
                ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void rdoCustomerBalance_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCustomerBalance.Checked)
            {
                txtClosingDate.Enabled = true;
                txtClosingDate.Required = true;
            }
            else
            {
                txtClosingDate.Clear();
                txtClosingDate.Enabled = false;
            }
        }

        private void txtClosingDate_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClosingDate.Text)) return;

            var value = txtClosingDate.Text;
            var day = 0;
            if (!int.TryParse(value, out day)
                || day == 0)
            {
                txtClosingDate.Clear();
                return;
            }

            if (28 <= day)
                txtClosingDate.Text = "99";
        }
        private void grdCustomerLedger_DataBindingComplete(object sender, MultiRowBindingCompleteEventArgs e)
        {
            if (grdCustomerLedger.RowCount == 0) return;

            for (int i = 0; i < grdCustomerLedger.RowCount; i++)
            {
                var ledger = grdCustomerLedger.Rows[i].DataBoundItem as CustomerLedger;
                if (ledger == null) continue;

                var recordType = ledger.RecordType;

                if (recordType == CustomerLedger.RecordTypeAlias.DataRecord)
                {
                    grdCustomerLedger.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {

                    grdCustomerLedger.Rows[i].Cells[CellName("AtSpan")].Visible = true;

                    grdCustomerLedger.Rows[i].Cells[CellName(nameof(CustomerLedger.RecordedAt))].Visible = false;
                    grdCustomerLedger.Rows[i].Cells[CellName(nameof(CustomerLedger.SectionName))].Visible = false;
                    grdCustomerLedger.Rows[i].Cells[CellName(nameof(CustomerLedger.DepartmentName))].Visible = false;
                    grdCustomerLedger.Rows[i].Cells[CellName(nameof(CustomerLedger.InvoiceCode))].Visible = false;
                    grdCustomerLedger.Rows[i].Cells[CellName(nameof(CustomerLedger.CategoryName))].Visible = false;
                    grdCustomerLedger.Rows[i].Cells[CellName(nameof(CustomerLedger.AccountTitleName))].Visible = false;

                    if (recordType == CustomerLedger.RecordTypeAlias.CarryOverRecord)
                    {
                        grdCustomerLedger.Rows[i].DefaultCellStyle.BackColor = Color.Khaki;
                        grdCustomerLedger.Rows[i].Cells[CellName("AtSpan")].Value = " 繰越";
                    }
                    else if (recordType == CustomerLedger.RecordTypeAlias.MonthlySubtotalRecord)
                    {
                        grdCustomerLedger.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;
                        grdCustomerLedger.Rows[i].Cells[CellName("AtSpan")].Value = $"{ledger.YearMonth:yyyy/MM} 合計";
                    }
                    else
                    {
                        grdCustomerLedger.Rows[i].DefaultCellStyle.BackColor = Color.NavajoWhite;
                        grdCustomerLedger.Rows[i].Cells[CellName("AtSpan")].Value = " 総合計";
                    }
                }
            }
        }

        #endregion

        #region report settings

        private ReportTargetDate GetReportSettingTargetDate() => ReportSettingList.GetReportSetting<ReportTargetDate>(TargetDate);
        private ReportTotalByDay GetReportSettingTotalByDay() => ReportSettingList.GetReportSetting<ReportTotalByDay>(TotalByDay);
        private ReportDoOrNot GetReportSettingMonthlyBreak() => ReportSettingList.GetReportSetting<ReportDoOrNot>(MonthlyBreak);
        private ReportTotal GetReportSettingSlipTotal() => ReportSettingList.GetReportSetting<ReportTotal>(SlipTotal);
        private ReportDoOrNot GetReportSettingReceiptGrouping() => ReportSettingList.GetReportSetting<ReportDoOrNot>(ReceiptGrouping);
        private ReportAdvanceReceivedType GetReportSettingRemainType() => ReportSettingList.GetReportSetting<ReportAdvanceReceivedType>(RemainType);
        private ReportDoOrNot GetReportSettingDisplayDepartment() => ReportSettingList.GetReportSetting<ReportDoOrNot>(DisplayDepartmentCode);
        private ReportDoOrNot GetReportSettingDisplaySection() => ReportSettingList.GetReportSetting<ReportDoOrNot>(DisplaySectionCode);
        private ReportUnitPrice GetReportSettingUnitPrice() => ReportSettingList.GetReportSetting<ReportUnitPrice>(UnitPrice);
        private ReportDoOrNot GetReportSettingDisplaySymbol() => ReportSettingList.GetReportSetting<ReportDoOrNot>(DisplaySymbol);

        private string GetReportSettingNameTargetDate()
        {
            var target = GetReportSettingTargetDate();
            return
                target == ReportTargetDate.BilledAt ? "請求日" :
                target == ReportTargetDate.SalesAt ? "売上日" : string.Empty;
        }
        private string GetReportSettingNameBillingRemainType()
        {
            var remain = GetReportSettingRemainType();
            return
                remain == ReportAdvanceReceivedType.UseMatchingAmount ? "消込額を使用する" :
                remain == ReportAdvanceReceivedType.UseReceiptAmount ? "入金額を使用する" :
                remain == ReportAdvanceReceivedType.UseMatchingAmountWithReceiptAmount ? "消込額を使用して入金額を表示" : string.Empty;
        }
        private string GetReportSettingNameUnitPrice()
        {
            var unit = GetReportSettingUnitPrice();
            return
                unit == ReportUnitPrice.Per1 ? "円" :
                unit == ReportUnitPrice.Per1000 ? "千円" :
                unit == ReportUnitPrice.Per10000 ? "万円" :
                unit == ReportUnitPrice.Per1000000 ? "百万円" : string.Empty;
        }

        #endregion
    }
}
