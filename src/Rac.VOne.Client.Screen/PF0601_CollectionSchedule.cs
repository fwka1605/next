using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CollectionScheduleService;
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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Client.Reports.Settings.PF0601;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>回収予定表</summary>
    public partial class PF0601 : VOneScreenBase
    {
        private List<ReportSetting> ReportSettingList { get; set; } = new List<ReportSetting>();
        private string UncollectedAmountLast { get; set; }
        private string UncollectedAmount0 { get; set; }
        private string UncollectedAmount1 { get; set; }
        private string UncollectedAmount2 { get; set; }
        private string UncollectedAmount3 { get; set; }
        private string CellName(string value) => $"cel{value}";
        private TaskProgressManager ProgressManager;

        #region initialize

        public PF0601()
        {
            InitializeComponent();
            grdSearchResult.SetupShortcutKeys();
            grdSearchResult.GridColorType = GridColorType.Special;
            Text = "回収予定表";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcCollectionSchedule.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcCollectionSchedule.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = ExitCollectionSchedule;
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
            OnF01ClickHandler = SearchCollectionSchedule;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearCollectionSchedule;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintCollectionSchedule;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportCollectionSchedule;

            BaseContext.SetFunction07Caption("設定");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = OpenPrintSetting;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitCollectionSchedule;
        }

        private void PF0601_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                loadTask.Add(LoadControlColorAsync());
                if (Company == null)
                {
                    loadTask.Add(Task.Run(async () =>
                    {
                        await LoadCompanyAsync();
                        await LoadReportSettingAsync();
                    }));
                }
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SetDataFormat();
                ClearCollectionSchedule();
                if (UseForeignCurrency)
                {
                    dlblPriceUnit.Visible = false;
                    lblAmountUnit.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetHeaderFormat()
        {
            if (datBaseMonth.Value.HasValue)
            {
                DateTime inputDate = datBaseMonth.Value.Value;
                UncollectedAmountLast = inputDate.Month == 1 ? 12 + "月迄未回収" : (inputDate.Month - 1).ToString().PadLeft(2, '0') + "月迄未回収";

                UncollectedAmount0 = inputDate.Month.ToString().PadLeft(2, '0') + "月";

                UncollectedAmount1 = int.Parse(UncollectedAmount0.ToString().Substring(0, 2)) == 12 ? 1.ToString().PadLeft(2, '0') + "月"
                    : (int.Parse(UncollectedAmount0.Substring(0, 2)) + 1).ToString().PadLeft(2, '0') + "月";

                UncollectedAmount2 = int.Parse(UncollectedAmount1.ToString().Substring(0, 2)) == 12 ? 1.ToString().PadLeft(2, '0') + "月"
                    : (int.Parse(UncollectedAmount1.Substring(0, 2)) + 1).ToString().PadLeft(2, '0') + "月";

                UncollectedAmount3 = int.Parse(UncollectedAmount2.ToString().Substring(0, 2)) == 12 ? 1.ToString().PadLeft(2, '0') + "月以降"
                    : (int.Parse(UncollectedAmount2.Substring(0, 2)) + 1).ToString().PadLeft(2, '0') + "月以降";
            }
        }

        private void SetDataFormat()
        {
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);
                txtCustomerCodeFrom.Format = expression.CustomerCodeFormatString;
                txtCustomerCodeFrom.MaxLength = expression.CustomerCodeLength;
                txtCustomerCodeFrom.ImeMode = expression.CustomerCodeImeMode();
                txtCustomerCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;
                txtCustomerCodeTo.Format = expression.CustomerCodeFormatString;
                txtCustomerCodeTo.MaxLength = expression.CustomerCodeLength;
                txtCustomerCodeTo.ImeMode = expression.CustomerCodeImeMode();
                txtCustomerCodeTo.PaddingChar = expression.CustomerCodePaddingChar;
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
            }
            Settings.SetCheckBoxValue<PF0601>(Login, cbxCustomer);
            Settings.SetCheckBoxValue<PF0601>(Login, cbxDepartment);
            Settings.SetCheckBoxValue<PF0601>(Login, cbxStaff);
        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);
            var height = builder.DefaultRowHeight;
            var dispDept = GetReportSettingDisplayDepartment() == ReportDoOrNot.Do;
            var deptWidth = dispDept ? 120 : 0;
            var template = new Template();
            var posX = new int[]
               {
                   0,
                   40,
                   240,
                   270,
                   390,
                   dispDept ? 510  : 390 ,
                   dispDept ? 610  : 490 ,
                   dispDept ? 730  : 610 ,
                   dispDept ? 850  : 730 ,
                   dispDept ? 970  : 850 ,
                   dispDept ? 1090 : 970 ,
                   dispDept ? 1210 : 1090,
                   dispDept ? 1310 : 1190
               };

            var positionY = new int[] { 0, height };

            var bdLDbl = builder.GetBorder(
               left: LineStyle.Double,
               lineColor: ColorContext.GridLineColor);
            builder.Items.AddRange(new CellSetting[]
            {
                # region ヘッダー1
                new CellSetting(height * 2,        40, "Header"                                         , location: new Point(posX[0] ,      0)),
                new CellSetting(height    ,       200, nameof(CollectionSchedule.CustomerInfo)          , location: new Point(posX[1] ,      0), caption: "得意先"),
                new CellSetting(height * 2,        30, nameof(CollectionSchedule.Closing)               , location: new Point(posX[2] ,      0), caption: "締\r\n日"),
                new CellSetting(height * 2,       120, nameof(CollectionSchedule.StaffName)             , location: new Point(posX[3] ,      0), caption: "担当者"),
                new CellSetting(height * 2, deptWidth, nameof(CollectionSchedule.DepartmentName)        , location: new Point(posX[4] ,      0), caption: "請求部門"),
                new CellSetting(height * 2,       100, nameof(CollectionSchedule.CollectCategoryName)   , location: new Point(posX[5] ,      0), caption: "区分"),
                new CellSetting(height * 2,       120, nameof(CollectionSchedule.UncollectedAmountLast) , location: new Point(posX[6] ,      0), caption: UncollectedAmountLast),
                new CellSetting(height * 2,       120, nameof(CollectionSchedule.UncollectedAmount0)    , location: new Point(posX[7] ,      0), caption: UncollectedAmount0),
                new CellSetting(height * 2,       120, nameof(CollectionSchedule.UncollectedAmount1)    , location: new Point(posX[8] ,      0), caption: UncollectedAmount1),
                new CellSetting(height * 2,       120, nameof(CollectionSchedule.UncollectedAmount2)    , location: new Point(posX[9] ,      0), caption: UncollectedAmount2),
                new CellSetting(height * 2,       120, nameof(CollectionSchedule.UncollectedAmount3)    , location: new Point(posX[10],      0), caption: UncollectedAmount3),
                new CellSetting(height    ,       240, "Total"                                          , location: new Point(posX[11],      0), caption: "合計"),
                #endregion
                #region ヘッダー2
                new CellSetting(height    ,       200, "CustomerCollectCategoryName"                    , location: new Point(posX[1] , height), caption: "マスター回収方法"),
                new CellSetting(height    ,       100, "CollectCategoryTotal"                           , location: new Point(posX[11], height), caption: "区分"),
                new CellSetting(height    ,       120, nameof(CollectionSchedule.UncollectedAmountTotal), location: new Point(posX[12], height), caption: "金額"),
                #endregion
            });

            builder.BuildHeaderOnly(template);
            template.ColumnHeaders[0].Cells[2].Style.Multiline = MultiRowTriState.True;
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                # region データ1
                new CellSetting(height,        40, "Header"                                         , location: new Point(posX[0] , 0), dataField: nameof(CollectionSchedule.RowId)                 , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,       200, nameof(CollectionSchedule.CustomerInfo)          , location: new Point(posX[1] , 0), dataField: nameof(CollectionSchedule.CustomerInfo)          , cell: builder.GetTextBoxCell()),
                new CellSetting(height,        30, nameof(CollectionSchedule.Closing)               , location: new Point(posX[2] , 0), dataField: nameof(CollectionSchedule.Closing)               , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height,       120, nameof(CollectionSchedule.StaffName)             , location: new Point(posX[3] , 0), dataField: nameof(CollectionSchedule.StaffName)             , cell: builder.GetTextBoxCell()),
                new CellSetting(height, deptWidth, nameof(CollectionSchedule.DepartmentName)        , location: new Point(posX[4] , 0), dataField: nameof(CollectionSchedule.DepartmentName)        , cell: builder.GetTextBoxCell()),
                new CellSetting(height,       100, nameof(CollectionSchedule.CollectCategoryName)   , location: new Point(posX[5] , 0), dataField: nameof(CollectionSchedule.CollectCategoryName)   , cell: builder.GetTextBoxCell()),
                new CellSetting(height,       120, nameof(CollectionSchedule.UncollectedAmountLast) , location: new Point(posX[6] , 0), dataField: nameof(CollectionSchedule.UncollectedAmountLast) , cell: builder.GetTextBoxCurrencyCell(), border: bdLDbl),
                new CellSetting(height,       120, nameof(CollectionSchedule.UncollectedAmount0)    , location: new Point(posX[7] , 0), dataField: nameof(CollectionSchedule.UncollectedAmount0)    , cell: builder.GetTextBoxCurrencyCell(), border: bdLDbl),
                new CellSetting(height,       120, nameof(CollectionSchedule.UncollectedAmount1)    , location: new Point(posX[8] , 0), dataField: nameof(CollectionSchedule.UncollectedAmount1)    , cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,       120, nameof(CollectionSchedule.UncollectedAmount2)    , location: new Point(posX[9] , 0), dataField: nameof(CollectionSchedule.UncollectedAmount2)    , cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,       120, nameof(CollectionSchedule.UncollectedAmount3)    , location: new Point(posX[10], 0), dataField: nameof(CollectionSchedule.UncollectedAmount3)    , cell: builder.GetTextBoxCurrencyCell()),
                new CellSetting(height,       100, "CollectCategoryTotal"                           , location: new Point(posX[11], 0), dataField: nameof(CollectionSchedule.CollectCategoryName)   , cell: builder.GetTextBoxCell(), border: bdLDbl),
                new CellSetting(height,       120, nameof(CollectionSchedule.UncollectedAmountTotal), location: new Point(posX[12], 0), dataField: nameof(CollectionSchedule.UncollectedAmountTotal), cell: builder.GetTextBoxCurrencyCell()),
                #endregion
            });

            builder.BuildRowOnly(template);
            grdSearchResult.Template = template;
            grdSearchResult.HideSelection = true;
            grdSearchResult.AllowAutoExtend = false;
        }

        #endregion

        #region Function Key event

        [OperationLog("照会")]
        private void SearchCollectionSchedule()
        {
            try
            {
                if (!ValidateInputValues()) return;
                ClearStatusMessage();

                var list = GetBaseTask();
                list.Add(new TaskProgress($"{ParentForm.Text} 画面表示"));

                ProgressManager = new TaskProgressManager(list);
                var searchTask = SearchAsync();
                NLogHandler.WriteDebug(this, "回収予定表 照会開始");
                var dialogResult = ProgressDialogStart(ParentForm, ParentForm.Text, searchTask, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                NLogHandler.WriteDebug(this, "回収予定表 照会終了");

                if (dialogResult != DialogResult.OK)
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

        private List<TaskProgress> GetBaseTask()
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求データ抽出"));
            list.Add(new TaskProgress($"{ParentForm.Text} 消込データ抽出"));
            list.Add(new TaskProgress($"{ParentForm.Text} 集計データ取得"));
            list.Add(new TaskProgress($"{ParentForm.Text} 集計データ整形"));
            return list;
        }

        private async Task<bool> SearchAsync()
        {

            var option = GetSearchCondition();
            var list = await CollectionScheduleListAsync(option);

            if (list == null)
            {
                grdSearchResult.Template = null;
                ProgressManager.Canceled = true;
                ProgressManager.UpdateState();
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            if (!list.Any())
            {
                grdSearchResult.Template = null;
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                ShowWarningDialog(MsgWngNotExistSearchData);
                return false;
            }
            bool noSearchData = false;

            dlblSubtotalUnit.Text = GetReportSettingNameSubtotalUnit();
            dlblStaffSelection.Text = GetReportSettingNameStaffSelection();
            dlblPriceUnit.Text = GetReportSettingNameUnitPrice();
            SetHeaderFormat();
            InitializeGrid();
            //SetOrderedList();
            grdSearchResult.DataSource = new BindingSource(list, null);
            tbcCollectionSchedule.SelectedIndex = 1;
            BaseContext.SetFunction07Enabled(false);
      
            noSearchData = true;
            ProgressManager.UpdateState();
            return noSearchData;
        }

        [OperationLog("クリア")]
        private void ClearCollectionSchedule()
        {
            ClearStatusMessage();
            Clear();
        }

        private void Clear()
        {
            tbcCollectionSchedule.SelectedIndex = 0;
            datBaseMonth.Clear();
            datBaseMonth.Select();
            ActiveControl = datBaseMonth;
            grdSearchResult.Template = null;
            txtDepartmentCodeFrom.Clear();
            lblDepartmentNameFrom.Clear();
            txtDepartmentCodeTo.Clear();
            lblDepartmentNameTo.Clear();
            txtStaffCodeFrom.Clear();
            txtStaffCodeTo.Clear();
            lblStaffNameFrom.Clear();
            lblStaffNameTo.Clear();
            txtCustomerCodeFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();
            dlblSubtotalUnit.Clear();
            dlblStaffSelection.Clear();
            dlblPriceUnit.Clear();
            BaseContext.SetFunction07Enabled(true);
        }

        [OperationLog("印刷")]
        private void PrintCollectionSchedule()
        {
            try
            {
                if (!ValidateInputValues()) return;
                ClearStatusMessage();

                var serverPath = GetServerPath();

                var list = GetBaseTask();
                list.Add(new TaskProgress("帳票の作成"));
                ProgressManager = new TaskProgressManager(list);
                var task = GetCollectionScheduleReportAsync();
                var dialogResult = ProgressDialogStart(ParentForm, ParentForm.Text + " 印刷", task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                if (dialogResult != DialogResult.OK)
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var result = task.Result;
                if (result == null)
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

        private async Task<CollectionScheduleSectionReport> GetCollectionScheduleReportAsync()
        {
            var option = GetSearchCondition();
            option.IsPrint = true;
            SetHeaderFormat();

            var list = await CollectionScheduleListAsync(option);
            if (list == null)
            {
                ProgressManager.Canceled = true;
                return null;
            }
            if (!list.Any())
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                return null;
            }
            CollectionScheduleSectionReport report = null;
            await Task.Run(() => report = CreateReport(list));
            ProgressManager.UpdateState();
            return report;
        }

        private CollectionScheduleSectionReport CreateReport(List<CollectionSchedule> list)
        {
            var title = "回収予定表";
            var reportName = title + DateTime.Today.ToString("yyyyMMdd");
            var report = new CollectionScheduleSectionReport();
            report.GroupByDepartment = GetReportSettingDisplayDepartment() == ReportDoOrNot.Do;
            report.NewPagePerDepartment = GetReportSettingDepartmentNewPage() == ReportDoOrNot.Do;
            report.NewPagePerStaff = GetReportSettingStaffNewPage() == ReportDoOrNot.Do;
            report.lblUncollectedAmountLast.Text = UncollectedAmountLast;
            report.lblUncollectAmount0.Text = UncollectedAmount0;
            report.lblUncollectAmount1.Text = UncollectedAmount1;
            report.lblUncollectAmount2.Text = UncollectedAmount2;
            report.lblUncollectAmount3.Text = UncollectedAmount3;
            report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
            report.Name = reportName;

            report.SetData(list);
            var subReport = new SearchConditionSectionReport();
            subReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, title);
            subReport.Name = reportName;
            var fileterInfo = GetFilterInfo();
            subReport.SetPageDataSetting(fileterInfo);

            report.Run(false);
            subReport.SetPageNumber(report.Document.Pages.Count);
            subReport.Run(false);

            report.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)subReport.Document.Pages.Clone());

            return report;
        }

        private string GetServerPath()
        {
            var serverPath = string.Empty;
            try
            {
                var t = LoadServerPathAsync();
                ProgressDialog.Start(ParentForm, t, false, SessionKey);
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
            return serverPath;
        }

        [OperationLog("設定")]
        private void OpenPrintSetting()
        {
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PF0601);
                screen.InitializeParentForm("帳票設定");
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }

        [OperationLog("エクスポート")]
        private void ExportCollectionSchedule()
        {
            try
            {
                ClearStatusMessage();
                if (!ValidateInputValues()) return;

                var serverPath = GetServerPath();
                var filePath = string.Empty;
                var fileName = $"回収予定表{datBaseMonth.Value:yyMM}.csv";

                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var list = GetBaseTask();
                list.Add(new TaskProgress("データ出力"));
                ProgressManager = new TaskProgressManager(list);

                var task = ExportCollectionScheduleAsync(filePath);
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
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<int> ExportCollectionScheduleAsync(string filePath)
        {
            var option = GetSearchCondition();
            var list = await CollectionScheduleListAsync(option);
            if (list == null)
            {
                ProgressManager.Canceled = true;
                return 0;
            }
            if (!list.Any())
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                return 0;
            }

            SetHeaderFormat();

            var definition = new CollectionScheduleFileDefinition(new DataExpression(ApplicationControl));
            definition.UncollectedAmountLastField.FieldName = UncollectedAmountLast;
            definition.UncollectedAmount0Field.FieldName = UncollectedAmount0;
            definition.UncollectedAmount1Field.FieldName = UncollectedAmount1;
            definition.UncollectedAmount2Field.FieldName = UncollectedAmount2;
            definition.UncollectedAmount3Field.FieldName = UncollectedAmount3;

            definition.UncollectedAmountLastField.Format = value => value.ToString("0");
            definition.UncollectedAmount0Field.Format = value => value.ToString("0");
            definition.UncollectedAmount1Field.Format = value => value.ToString("0");
            definition.UncollectedAmount2Field.Format = value => value.ToString("0");
            definition.UncollectedAmount3Field.Format = value => value.ToString("0");
            definition.UncollectedAmountTotalField.Format = value => value.ToString("0");
            if (definition.DepartmentNameField.Ignored =
                (GetReportSettingDisplayDepartment() == ReportDoOrNot.NotDo))
            {
                definition.DepartmentNameField.FieldName = null;
            }
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            var result = await exporter.ExportAsync(filePath, list, new System.Threading.CancellationToken(false), new Progress<int>());
            if (result == 0) ProgressManager.Abort();
            ProgressManager.UpdateState();
            return result;
        }

        private bool ValidateInputValues()
        {
            if (!datBaseMonth.Value.HasValue)
            {
                datBaseMonth.Select();
                ShowWarningDialog(MsgWngInputRequired, lblBaseMonth.Text);
                return false;
            }
            if (!txtDepartmentCodeFrom.ValidateRange(txtDepartmentCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDepartmentCode.Text))) return false;

            if (!txtStaffCodeFrom.ValidateRange(txtStaffCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaffCode.Text))) return false;

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCode.Text))) return false;
            return true;
        }

        private List<object> GetFilterInfo()
        {
            var list = new List<object>();
            var waveDash = " ～ ";
            list.Add(new SearchData(lblBaseMonth.Text, datBaseMonth.GetPrintValue("yyyy/MM")));
            list.Add(new SearchData(lblDepartmentCode.Text,
                txtDepartmentCodeFrom.GetPrintValueCode(lblDepartmentNameFrom) + waveDash +
                txtDepartmentCodeTo.GetPrintValueCode(lblDepartmentNameTo)));
            list.Add(new SearchData(lblStaffCode.Text,
                txtStaffCodeFrom.GetPrintValueCode(lblStaffNameFrom) + waveDash +
                txtStaffCodeTo.GetPrintValueCode(lblStaffNameTo)));
            list.Add(new SearchData(lblCustomerCode.Text,
                txtCustomerCodeFrom.GetPrintValueCode(lblCustomerNameFrom) + waveDash +
                txtCustomerCodeTo.GetPrintValueCode(lblCustomerNameTo)));
            list.Add(new SearchData("得意先集計方法", GetReportSettingNameSubtotalUnit()));
            list.Add(new SearchData("担当者集計方法", GetReportSettingNameStaffSelection()));
            if (!UseForeignCurrency)
                list.Add(new SearchData("金額単位", GetReportSettingNameUnitPrice()));
            return list;
        }

        private CollectionScheduleSearch GetSearchCondition()
        {
            var option = new CollectionScheduleSearch();
            option.CompanyId = CompanyId;

            if (datBaseMonth.Value.HasValue)
            {
                var dat = datBaseMonth.Value.Value;
                var closingDay = Company.ClosingDay;
                option.YearMonth = new DateTime(dat.Year, dat.Month,
                    closingDay == 99 ? DateTime.DaysInMonth(dat.Year, dat.Month) : closingDay);
            }
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

            option.DisplayDepartment = GetReportSettingDisplayDepartment() == ReportDoOrNot.Do;
            option.DisplayParent = GetReportSettingSubtotalUnit() == ReportSubtotalUnit.CustomerGroup;
            option.UseMasterStaff = GetReportSettingStaffSelection() == ReportStaffSelection.ByCustomerMaster;
            option.UnitPrice = GetReportSettingValueUnitPrice();
            option.NewPagePerDepartment = GetReportSettingDepartmentNewPage() == ReportDoOrNot.Do;
            option.NewPagePerStaff = GetReportSettingStaffNewPage() == ReportDoOrNot.Do;
            return option;

        }

        [OperationLog("終了")]
        private void ExitCollectionSchedule()
        {
            try
            {
                Settings.SaveControlValue<PF0601>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PF0601>(Login, cbxCustomer.Name, cbxCustomer.Checked);
                Settings.SaveControlValue<PF0601>(Login, cbxStaff.Name, cbxStaff.Checked);
                BaseForm.Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void ReturnToSearchCondition()
        {
            tbcCollectionSchedule.SelectedIndex = 0;
        }

        #endregion

        #region Webサービス呼び出し
        private async Task LoadReportSettingAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<ReportSettingMasterClient>();
                var result = await client.GetItemsAsync(SessionKey, CompanyId, nameof(PF0601));

                if (result.ProcessResult.Result)
                {
                    ReportSettingList = result.ReportSettings;
                }
            });
        }

        private System.Action OnCancelHandler { get; set; }

        private async Task<List<CollectionSchedule>> CollectionScheduleListAsync(CollectionScheduleSearch option)
        {
            List<CollectionSchedule> collectionScheduleList = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))
            {

                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<CollectionScheduleServiceClient>(async client =>
                {
                    var result = await client.GetAsync(SessionKey, option, hubConnection.ConnectionId);
                    if (result.ProcessResult.Result)
                        collectionScheduleList = result.CollectionSchedules;
                });
            }

            return collectionScheduleList;
        }

        private async Task<string> LoadServerPathAsync()
        {
            var path = string.Empty;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<GeneralSettingMasterClient>();
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    path = result.GeneralSetting?.Value;
                }
            });
            return path;
        }

        private async Task<Department> GetDepartmentAsync(string code)
        {
            Department department = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<DepartmentMasterClient>();
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                    department = result.Departments.FirstOrDefault();
            });
            return department;
        }

        private async Task<Staff> GetStaffAsync(string code)
        {
            Staff staff = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<StaffMasterClient>();
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                    staff = result.Staffs.FirstOrDefault();
            });
            return staff;
        }

        private async Task<Customer> GetCustomerAsync(string code)
        {
            Customer customer = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CustomerMasterClient>();
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                    customer = result.Customers.FirstOrDefault();
            });
            return customer;
        }

        #endregion

        #region event handler

        private void btnDepartmentCode_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                if (sender == btnDepartmentCodeFrom)
                {
                    txtDepartmentCodeFrom.Text = department.Code;
                    lblDepartmentNameFrom.Text = department.Name;
                    if (cbxDepartment.Checked)
                    {
                        txtDepartmentCodeTo.Text = department.Code;
                        lblDepartmentNameTo.Text = department.Name;
                    }
                }
                else
                {
                    txtDepartmentCodeTo.Text = department.Code;
                    lblDepartmentNameTo.Text = department.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnStaffCode_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                if (sender == btnStaffCodeFrom)
                {
                    txtStaffCodeFrom.Text = staff.Code;
                    lblStaffNameFrom.Text = staff.Name;
                    if (cbxStaff.Checked)
                    {
                        txtStaffCodeTo.Text = staff.Code;
                        lblStaffNameTo.Text = staff.Name;
                    }
                }
                else
                {
                    txtStaffCodeTo.Text = staff.Code;
                    lblStaffNameTo.Text = staff.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnCustomerCode_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                if (sender == btnCustomerCodeFrom)
                {
                    txtCustomerCodeFrom.Text = customer.Code;
                    lblCustomerNameFrom.Text = customer.Name;
                    if (cbxCustomer.Checked)
                    {
                        txtCustomerCodeTo.Text = customer.Code;
                        lblCustomerNameTo.Text = customer.Name;
                    }
                }
                else
                {
                    txtCustomerCodeTo.Text = customer.Code;
                    lblCustomerNameTo.Text = customer.Name;
                }
                ClearStatusMessage();
            }
        }

        private void txtDepartmentCodeFrom_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();

            var code = txtDepartmentCodeFrom.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblDepartmentNameFrom.Clear();
                if (cbxDepartment.Checked)
                {
                    txtDepartmentCodeTo.Clear();
                    lblDepartmentNameTo.Clear();
                }
                return;
            }

            Department department = null;
            try
            {
                var task = GetDepartmentAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                department = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            var name = department?.Name ?? string.Empty;
            lblDepartmentNameFrom.Text = name;
            if (cbxDepartment.Checked)
            {
                txtDepartmentCodeTo.Text = code;
                lblDepartmentNameTo.Text = name;
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

            Department department = null;
            try
            {
                var task = GetDepartmentAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                department = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            lblDepartmentNameTo.Text = department?.Name ?? string.Empty;
        }

        private void txtStaffCodeFrom_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();

            var code = txtStaffCodeFrom.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblStaffNameFrom.Clear();
                if (cbxStaff.Checked)
                {
                    txtStaffCodeTo.Clear();
                    lblStaffNameTo.Clear();
                }
                return;
            }

            Staff staff = null;
            try
            {
                var task = GetStaffAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                staff = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            var name = staff?.Name ?? string.Empty;
            txtStaffCodeFrom.Text = code;
            lblStaffNameFrom.Text = name;
            if (cbxStaff.Checked)
            {
                txtStaffCodeTo.Text = code;
                lblStaffNameTo.Text = name;
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

            Staff staff = null;
            try
            {
                var task = GetStaffAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                staff = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            lblStaffNameTo.Text = staff?.Name ?? string.Empty;
        }

        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtCustomerCodeFrom.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblCustomerNameFrom.Clear();
                if (cbxCustomer.Checked)
                {
                    txtCustomerCodeTo.Clear();
                    lblCustomerNameTo.Clear();
                }
                return;
            }

            Customer customer = null;
            try
            {
                var task = GetCustomerAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                customer = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            var name = customer?.Name ?? string.Empty;
            txtCustomerCodeFrom.Text = code;
            lblCustomerNameFrom.Text = name;
            if (cbxCustomer.Checked)
            {
                txtCustomerCodeTo.Text = code;
                lblCustomerNameTo.Text = name;
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
            Customer customer = null;
            try
            {
                var task = GetCustomerAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                customer = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            lblCustomerNameTo.Text = customer?.Name ?? string.Empty;
        }

        private void grid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.RowIndex < 0
                || grdSearchResult.RowCount <= e.RowIndex)
                return;

            var current = grdSearchResult.Rows[e.RowIndex].DataBoundItem as CollectionSchedule;
            var next = e.RowIndex + 1 < grdSearchResult.RowCount
                ? grdSearchResult.Rows[e.RowIndex + 1].DataBoundItem as CollectionSchedule : null;
            e.CellStyle.Border = new Border()
            {
                Left = e.CellName == CellName(nameof(CollectionSchedule.UncollectedAmountLast))
                    || e.CellName == CellName(nameof(CollectionSchedule.UncollectedAmount0))
                    || e.CellName == CellName("CollectCategoryTotal") ?
                         new Line() { Color = ColorContext.GridLineColor, Style = LineStyle.Double } :
                         new Line() { Color = ColorContext.GridLineColor, Style = LineStyle.Thin },
                Top = new Line() { Color = ColorContext.GridLineColor, Style = current.RowId.HasValue ? LineStyle.Thin : LineStyle.Dotted },
                Right = new Line() { Color = ColorContext.GridLineColor, Style = LineStyle.Thin },
                Bottom = new Line() { Color = ColorContext.GridLineColor, Style = (next?.RowId.HasValue ?? true) ? LineStyle.Thin : LineStyle.Dotted }
            };
            if (current.RecordType > 0)
            {
                if (e.CellName != CellName("Header"))
                    e.CellStyle.BackColor = Color.Khaki;

                if (e.CellName == CellName(nameof(CollectionSchedule.CustomerInfo)))
                    e.CellStyle.TextAlign = MultiRowContentAlignment.MiddleCenter;
            }
        }

        #endregion

        #region report settings

        private ReportSubtotalUnit GetReportSettingSubtotalUnit()
            => ReportSettingList.GetReportSetting<ReportSubtotalUnit>(CustomerType);


        private ReportStaffSelection GetReportSettingStaffSelection()
            => ReportSettingList.GetReportSetting<ReportStaffSelection>(StaffSelection);


        private ReportDoOrNot GetReportSettingStaffNewPage()
            => ReportSettingList.GetReportSetting<ReportDoOrNot>(StaffNewPage);


        private ReportDoOrNot GetReportSettingDepartmentNewPage()
            => ReportSettingList.GetReportSetting<ReportDoOrNot>(DepartmentNewPage);


        private ReportDoOrNot GetReportSettingDisplayDepartment()
            => ReportSettingList.GetReportSetting<ReportDoOrNot>(DisplayDepartment);

        private ReportUnitPrice GetReportSettingUnitPrice()
            => ReportSettingList.GetReportSetting<ReportUnitPrice>(UnitPrice);

        private string GetReportSettingNameSubtotalUnit()
        {
            var unit = GetReportSettingSubtotalUnit();
            return unit == ReportSubtotalUnit.PlainCustomer ? "得意先" : "債権代表者";
        }

        private string GetReportSettingNameStaffSelection()
        {
            var staffSelection = GetReportSettingStaffSelection();
            return staffSelection == ReportStaffSelection.ByBillingData ? "請求データ" : "得意先マスター";
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

        private decimal GetReportSettingValueUnitPrice()
        {
            var unit = GetReportSettingUnitPrice();
            return
                UseForeignCurrency ? 1M :
                unit == ReportUnitPrice.Per1 ? 1M :
                unit == ReportUnitPrice.Per1000 ? 1000M :
                unit == ReportUnitPrice.Per10000 ? 10000M :
                unit == ReportUnitPrice.Per1000000 ? 1000000M : 1M;
        }

        #endregion
    }
}
