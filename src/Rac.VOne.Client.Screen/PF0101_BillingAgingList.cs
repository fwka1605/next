using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingAgingListService;
using Rac.VOne.Client.Screen.BillingBalanceService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
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
using static Rac.VOne.Client.Reports.Settings.PF0101;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>請求残高年齢表</summary>
    public partial class PF0101 : VOneScreenBase
    {
        private int? CurrencyId { get; set; }
        private int Precision { get; set; }
        private string CellName(string value) => $"cel{value}";
        private string MonthlyRemain0 { get; set; }
        private string MonthlyRemain1 { get; set; }
        private string MonthlyRemain2 { get; set; }
        private string MonthlyRemain3 { get; set; }
        private List<ReportSetting> ReportSettingList { get; set; } = new List<ReportSetting>();
        private BillingAgingListSearch BillingAgingSearchCondition { get; set; }
        private List<object> BillingAgingSearchReportList { get; set; }
        private List<object> DetailSearchConditionList { get; set; }
        private TaskProgressManager ProgressManager;


        #region 画面の初期化
        public PF0101()
        {
            InitializeComponent();
            grdSearchResult.SetupShortcutKeys();
            grdSearchResult.GridColorType = GridColorType.Special;
            Text = "請求残高年齢表";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            grdSearchResult.CellFormatting += grdSearchResult_CellFormatting;
            grdSearchResult.CellEditedFormattedValueChanged += grd_CellEditedFormattedValueChanged;
            tbcBillingAgingList.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcBillingAgingList.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = ExitBillingAging;
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
            OnF01ClickHandler = SearchBillingAgingList;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearBillingAging;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintBillingAging;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportBillingAging;

            BaseContext.SetFunction07Caption("設定");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = OpenPrintSetting;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitBillingAging;
        }

        private void PF0101_Load(object sender, EventArgs e)
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
                InitializeControlsFormat();
                if (!UseForeignCurrency)
                {
                    lblCurrency.Visible = false;
                    txtCurrencyCode.Visible = false;
                    btnCurrencyCode.Visible = false;
                    lblUnitPrice.Visible = true;
                    lblDispUnitPrice.Visible = true;
                    txtCurrencyCode.Text = Rac.VOne.Common.Constants.DefaultCurrencyCode;
                    txtCurrencyCode_Validated(txtCurrencyCode, EventArgs.Empty);
                }
                ClearBillingAging();
                datBaseDate.Select();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetHeaderFormat()
        {
            if (!datBaseDate.Value.HasValue) return;
            var yearMonth = datBaseDate.Value.Value;
            if (yearMonth.Year < 2000) return;

            MonthlyRemain0 = $"{yearMonth:MM}月発生額";
            MonthlyRemain1 = $"{yearMonth.AddMonths(-1):MM}月発生額";
            MonthlyRemain2 = $"{yearMonth.AddMonths(-2):MM}月発生額";
            MonthlyRemain3 = $"{yearMonth.AddMonths(-3):MM}月以前発生額";
        }

        private void InitializeControlsFormat()
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
            txtClosingDate.PaddingChar = '0';
            Settings.SetCheckBoxValue<PF0101>(Login, cbxCustomer);
            Settings.SetCheckBoxValue<PF0101>(Login, cbxDepartment);
            Settings.SetCheckBoxValue<PF0101>(Login, cbxStaff);
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);
            var height = builder.DefaultRowHeight;
            var template = new Template();

            var currencyWidth = UseForeignCurrency ? 50 : 0;
            var customerNameWidth = UseForeignCurrency ? 200 : 250;
            var chkShow = builder.GetCheckBoxCell();
            chkShow.Appearance = Appearance.Button;
            chkShow.Style = new CellStyle() { TextAlign = MultiRowContentAlignment.MiddleCenter };
            chkShow.TrueValue = "-";
            chkShow.FalseValue = "+";

            var billingRemainType = GetReportSettingBillingRemainType();
            var groupType = GetReportSettingCustomerGroup();

            chkShow.Text =
                groupType == ReportCustomerGroup.ParentOnly ? "+" :
                groupType == ReportCustomerGroup.ParentWithChildren ? "-" : "";
            var isMatching = billingRemainType == ReportAdvanceReceivedType.UseMatchingAmount;

            var cellCheckBoxWidth = groupType == ReportCustomerGroup.PlainCusomter ? 0 : 25;
            var cellHeaderReceiptWidth = isMatching ? 0 : 240;
            var cellReceiptWidth = isMatching ? 0 : 120;
            var cellMatchingHeight = isMatching ? height * 2 : height;
            var cellMatchingY = isMatching ? 0 : height;
            var cellMoveX = isMatching ? 120 : 0;

            var widthsHeader = new int[]
            {
                40, 120, customerNameWidth, currencyWidth,
                cellHeaderReceiptWidth, cellReceiptWidth, 120
            };

            var widths = new int[]
            {
                40, cellCheckBoxWidth, 120 - cellCheckBoxWidth, customerNameWidth - 50, 50,
                currencyWidth, cellReceiptWidth, 120
            };

            var posX = new int[]
            {
                0, 40, cellCheckBoxWidth > 0 ? 40 + cellCheckBoxWidth : 40, 160,
                UseForeignCurrency ? 310 : 360 /* 通貨コード表示・非表示制御 */ ,
                360, 410, 530, 650, 890 - cellMoveX, 1010 - cellMoveX,
                1130 - cellMoveX, 1250 - cellMoveX, 1370 - cellMoveX,
                650, 770 - cellMoveX
            };
            var posY = new int[] { 0, height };
            var borderLeftDotted = builder.GetBorder(
                cellCheckBoxWidth > 0 ?
                LineStyle.Dotted :
                LineStyle.Thin,
                LineStyle.Thin,
                LineStyle.Thin,
                LineStyle.Thin);

            var borderRightDotted = builder.GetBorder(
                LineStyle.Thin,
                LineStyle.Thin,
                LineStyle.Dotted,
                LineStyle.Thin);

            var borderThick = builder.GetBorder(
                LineStyle.Thin,
                LineStyle.Thin,
                LineStyle.Medium,
                LineStyle.Thin);
            builder.Items.AddRange(new CellSetting[]
            {
                # region ヘッダー1
                new CellSetting(        height * 2, widthsHeader[0], "Header"                                     , location: new Point(posX[ 0], posY[0])),
                new CellSetting(        height * 2, widthsHeader[1], nameof(BillingAgingList.CustomerCode)        , location: new Point(posX[ 1], posY[0]), caption: "得意先コード"),
                new CellSetting(        height * 2, widthsHeader[2], nameof(BillingAgingList.CustomerName)        , location: new Point(posX[ 3], posY[0]), caption: "得意先名"    ),
                new CellSetting(        height * 2, widthsHeader[3], nameof(BillingAgingList.CurrencyCode)        , location: new Point(posX[ 5], posY[0]), caption: "通貨"        ),
                new CellSetting(        height * 2, widthsHeader[6], nameof(BillingAgingList.LastMonthRemain)     , location: new Point(posX[ 6], posY[0]), caption: "前月請求残"  ),
                new CellSetting(        height * 2, widthsHeader[6], nameof(BillingAgingList.CurrentMonthSales)   , location: new Point(posX[ 7], posY[0]), caption: "当月売上高"  ),
                new CellSetting(            height, widthsHeader[4], "HeaderReceipt"                              , location: new Point(posX[ 8], posY[0]), caption: "当月入金"),
                #region ヘッダー2
                new CellSetting(            height, widthsHeader[5], nameof(BillingAgingList.CurrentMonthReceipt) , location: new Point(posX[14], posY[1]), caption: "当月入金"),
                new CellSetting(cellMatchingHeight, widthsHeader[6], nameof(BillingAgingList.CurrentMonthMatching), location: new Point(posX[15], cellMatchingY), caption: "当月消込"),
                #endregion
                new CellSetting(        height * 2, widthsHeader[6], nameof(BillingAgingList.CurrentMonthRemain)  , location: new Point(posX[ 9], posY[0]), caption: "当月請求残"),
                new CellSetting(        height * 2, widthsHeader[6], nameof(BillingAgingList.MonthlyRemain0)      , location: new Point(posX[10], posY[0]), caption: MonthlyRemain0),
                new CellSetting(        height * 2, widthsHeader[6], nameof(BillingAgingList.MonthlyRemain1)      , location: new Point(posX[11], posY[0]), caption: MonthlyRemain1),
                new CellSetting(        height * 2, widthsHeader[6], nameof(BillingAgingList.MonthlyRemain2)      , location: new Point(posX[12], posY[0]), caption: MonthlyRemain2),
                new CellSetting(        height * 2, widthsHeader[6], nameof(BillingAgingList.MonthlyRemain3)      , location: new Point(posX[13], posY[0]), caption: MonthlyRemain3),
                #endregion
            });

            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                #region データ1
                new CellSetting(height, widths[0], "Header"                                     , location: new Point(posX[ 0], posY[0]), cell: builder.GetRowHeaderCell()),
                new CellSetting(height, widths[1], "chk"                                        , location: new Point(posX[ 1], posY[0]), border: borderRightDotted, cell: chkShow, readOnly: false),
                new CellSetting(height, widths[2], nameof(BillingAgingList.CustomerCode)        , location: new Point(posX[ 2], posY[0]), dataField: nameof(BillingAgingList.Code)             , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), border: borderLeftDotted),
                new CellSetting(height, widths[3], nameof(BillingAgingList.CustomerName)        , location: new Point(posX[ 3], posY[0]), dataField: nameof(BillingAgingList.Name)             , cell: builder.GetTextBoxCell(), border: borderRightDotted),
                new CellSetting(height, widths[4], nameof(BillingAgingList.TotalText)           , location: new Point(posX[ 4], posY[0]), dataField: nameof(BillingAgingList.TotalText)        , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight), border: borderLeftDotted ),
                new CellSetting(height, widths[5], nameof(BillingAgingList.CurrencyCode)        , location: new Point(posX[ 5], posY[0]), dataField: nameof(BillingAgingList.CurrencyCode)     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, widths[7], nameof(BillingAgingList.LastMonthRemain)     , location: new Point(posX[ 6], posY[0]), dataField: nameof(BillingAgingList.LastMonthRemain)  , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height, widths[7], nameof(BillingAgingList.CurrentMonthSales)   , location: new Point(posX[ 7], posY[0]), dataField: nameof(BillingAgingList.CurrentMonthSales), cell: builder.GetTextBoxCurrencyCell(Precision)),
                #region データ2
                new CellSetting(height, widths[6], nameof(BillingAgingList.CurrentMonthReceipt) , location: new Point(posX[14], posY[0]), dataField: nameof(BillingAgingList.CurrentMonthReceipt) , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height, widths[7], nameof(BillingAgingList.CurrentMonthMatching), location: new Point(posX[15], posY[0]), dataField: nameof(BillingAgingList.CurrentMonthMatching), cell: builder.GetTextBoxCurrencyCell(Precision)),
                #endregion
                new CellSetting(height, widths[7], nameof(BillingAgingList.CurrentMonthRemain)  , location: new Point(posX[ 9], posY[0]), dataField:nameof(BillingAgingList.CurrentMonthRemain), cell: builder.GetTextBoxCurrencyCell(Precision), border: borderThick),
                new CellSetting(height, widths[7], nameof(BillingAgingList.MonthlyRemain0)      , location: new Point(posX[10], posY[0]), dataField: nameof(BillingAgingList.MonthlyRemain0)   , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height, widths[7], nameof(BillingAgingList.MonthlyRemain1)      , location: new Point(posX[11], posY[0]), dataField: nameof(BillingAgingList.MonthlyRemain1)   , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height, widths[7], nameof(BillingAgingList.MonthlyRemain2)      , location: new Point(posX[12], posY[0]), dataField: nameof(BillingAgingList.MonthlyRemain2)   , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height, widths[7], nameof(BillingAgingList.MonthlyRemain3)      , location: new Point(posX[13], posY[0]), dataField: nameof(BillingAgingList.MonthlyRemain3)   , cell: builder.GetTextBoxCurrencyCell(Precision)),
                #endregion
            });

            builder.BuildRowOnly(template);
            grdSearchResult.AllowAutoExtend = false;
            grdSearchResult.Template = template;
            grdSearchResult.HideSelection = true;

            if (UseForeignCurrency)
            {
                grdSearchResult.FreezeLeftCellName = CellName(nameof(BillingAgingList.CurrencyCode));
            }
            else
            {
                grdSearchResult.FreezeLeftCellName = CellName(nameof(BillingAgingList.TotalText));
            }
        }

        #endregion

        #region Function Key Event

        [OperationLog("照会")]
        private void SearchBillingAgingList()
        {
            if (!ValidateChildren()) return;
            if (!ValidateForSearch()) return;

            ClearStatusMessage();
            DetailSearchConditionList = GetDetailReportCondition();

            try
            {
                ProgressDialog.Start(ParentForm, LoadReportSettingAsync(), false, SessionKey);
                var option = GetSearchOption();

                var list = GetBaseTasks(option);
                list.Add(new TaskProgress("画面の更新"));
                ProgressManager = new TaskProgressManager(list);

                var task = LoadBillingAgingList(option);
                NLogHandler.WriteDebug(this, "請求残高年齢表 照会開始");
                ProgressDialogStart(ParentForm, ParentForm.Text, task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                NLogHandler.WriteDebug(this, "請求残高年齢表 照会終了");
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private List<TaskProgress> GetBaseTasks(BillingAgingListSearch option)
        {
            var list = new List<TaskProgress>();
            list.Add(new TaskProgress($"{ParentForm.Text} 初期化"));
            list.Add(new TaskProgress($"{ParentForm.Text} 請求データ集計"));
            if (option.BillingRemainType > 0)
                list.Add(new TaskProgress($"{ParentForm.Text} 入金データ集計"));
            list.Add(new TaskProgress($"{ParentForm.Text} 消込データ集計"));
            list.Add(new TaskProgress($"{ParentForm.Text} 集計データ取得"));
            list.Add(new TaskProgress($"{ParentForm.Text} 集計データ整形"));
            return list;
        }

        private async Task LoadBillingAgingList(BillingAgingListSearch option)
        {
            BillingAgingSearchCondition = option;
            if (UseForeignCurrency)
            {
                await LoadCurrencyInfoAsync();
            }

            BaseContext.SetFunction07Enabled(true);
            var source = await GetBillingAgingListAsync(option);
            if (source == null)
            {
                grdSearchResult.Template = null;
                ProgressManager.Canceled = true;
                DispStatusMessage(MsgInfProcessCanceled);
            }
            else if (!source.Any())
            {
                grdSearchResult.Template = null;
                ProgressManager.NotFind();
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
            else
            {
                SetSearchOptionInformation();
                SetHeaderFormat();
                InitializeGridTemplate();
                grdSearchResult.DataSource = new BindingSource(source, null);
                BaseContext.SetFunction07Enabled(false);
                tbcBillingAgingList.SelectedIndex = 1;
            }
            ProgressManager.UpdateState();
        }

        private void SetSearchOptionInformation()
        {
            lblDispSelectedBaseDate.Text = GetReportSettingNameTargetDate();
            var staffSubtotal = GetReportSettingStaffTotal();
            lblDispSubTotalStaff.Text = staffSubtotal == ReportDoOrNot.NotDo ? "しない" : "する";
            var departmentSubtotal = GetReportSettingDepartmentTotal();
            lblDispSubTotalDepartment.Text = departmentSubtotal == ReportDoOrNot.NotDo ? "しない" : "する";
            lblDispStaffSelection.Text = GetReportSettingNameStaffSelection();
            lblDispCustomerGroup.Text = GetReportSettingNameCustomerGroup();
            lblDispAdvanceReceiptType.Text = GetReportSettingNameBillingRemainType();
            lblDispUnitPrice.Text = GetReportSettingNameUnitPrice();
        }

        //照会条件設定
        private List<object> GetSearchReportCondition()
        {
            var waveDash = " ～ ";
            var searchReport = new List<object>();
            var format = "yyyy/MM";
            searchReport.Add(new SearchData(lblBaseDate.Text, datBaseDate.GetPrintValue(format)));


            searchReport.Add(new SearchData(lblRemainType.Text, rdoClosingDate.Checked ? rdoClosingDate.Text : rdoCustomer.Text));

            searchReport.Add(new SearchData(lblClosingDate.Text, txtClosingDate.GetPrintValue()));

            searchReport.Add(new SearchData(lblDepartment.Text, txtDepartmentCodeFrom.GetPrintValueCode(lblDepartmentNameFrom)
                + waveDash + txtDepartmentCodeTo.GetPrintValueCode(lblDepartmentNameTo)));

            searchReport.Add(new SearchData(lblStaff.Text, txtStaffCodeFrom.GetPrintValueCode(lblStaffNameFrom)
                + waveDash + txtStaffCodeTo.GetPrintValueCode(lblStaffNameTo)));

            searchReport.Add(new SearchData(lblCustomer.Text, txtCustomerCodeFrom.GetPrintValueCode(lblCustomerNameFrom)
                + waveDash + txtCustomerCodeTo.GetPrintValueCode(lblCustomerNameTo)));

            if (UseForeignCurrency)
            {
                searchReport.Add(new SearchData(lblCurrency.Text, txtCurrencyCode.GetPrintValue()));
            }
            var billiOrSaleDate = GetReportSettingNameTargetDate();
            searchReport.Add(new SearchData("集計基準日", billiOrSaleDate));

            var billingRemain = GetReportSettingNameBillingRemainType();

            searchReport.Add(new SearchData("請求残高計算方法", billingRemain));

            var customerGroup = GetReportSettingNameCustomerGroup();
            searchReport.Add(new SearchData("得意先集計方法", customerGroup));

            var staffSelection = GetReportSettingNameStaffSelection();
            searchReport.Add(new SearchData("担当者集計方法", staffSelection));

            if (!UseForeignCurrency)
            {
                var unit = GetReportSettingNameUnitPrice();
                searchReport.Add(new SearchData("金額単位", unit));
            }

            return searchReport;

        }

        private List<object> GetDetailReportCondition()
        {
            var waveDash = " ～ ";
            var detailReport = new List<object>();
            var format = "yyyy/MM";

            detailReport.Add(new SearchData(lblBaseDate.Text, datBaseDate.GetPrintValue(format)));

            detailReport.Add(new SearchData(lblDepartment.Text, txtDepartmentCodeFrom.GetPrintValueCode(lblDepartmentNameFrom)
               + waveDash + txtDepartmentCodeTo.GetPrintValueCode(lblDepartmentNameTo)));

            detailReport.Add(new SearchData(lblStaff.Text, txtStaffCodeFrom.GetPrintValueCode(lblStaffNameFrom)
                + waveDash + txtStaffCodeTo.GetPrintValueCode(lblStaffNameTo)));

            if (UseForeignCurrency)
            {
                detailReport.Add(new SearchData(lblCurrency.Text, txtCurrencyCode.GetPrintValue()));
            }
            var billiOrSaleDate = GetReportSettingNameTargetDate();
            detailReport.Add(new SearchData("集計基準日", billiOrSaleDate));


            var staffSelection = GetReportSettingNameStaffSelection();
            detailReport.Add(new SearchData("担当者集計方法", staffSelection));

            if (!UseForeignCurrency)
            {
                var unit = GetReportSettingNameUnitPrice();
                detailReport.Add(new SearchData("金額単位", unit));
            }

            return detailReport;
        }

        private BillingAgingListSearch GetSearchOption()
        {
            var option = new BillingAgingListSearch();
            option.CompanyId = CompanyId;
            if (!string.IsNullOrEmpty(txtClosingDate.Text))
                option.ClosingDay = int.Parse(txtClosingDate.Text);
            if (datBaseDate.Value.HasValue)
                option.SetYearMonth(datBaseDate.Value.Value, Company.ClosingDay);

            option.LoginUserId = Login.UserId;
            option.TargetDate = (int)GetReportSettingTargetDate();
            var staffType = GetReportSettingStaffSelection();
            option.IsMasterStaff = staffType == ReportStaffSelection.ByCustomerMaster;
            var subStaff = GetReportSettingStaffTotal();
            option.RequireStaffSubtotal = subStaff == ReportDoOrNot.Do;
            var subDept = GetReportSettingDepartmentTotal();
            option.RequireDepartmentSubtotal = subDept == ReportDoOrNot.Do;

            var remain = GetReportSettingBillingRemainType();
            option.BillingRemainType = (int)remain;

            var group = GetReportSettingCustomerGroup();
            option.ConsiderCustomerGroup = group != ReportCustomerGroup.PlainCusomter;

            var unit = GetReportSettingUnitPrice();
            option.UnitValue =
                UseForeignCurrency ? 1M :
                unit == ReportUnitPrice.Per1 ? 1M :
                unit == ReportUnitPrice.Per1000 ? 1000M :
                unit == ReportUnitPrice.Per10000 ? 10000M :
                unit == ReportUnitPrice.Per1000000 ? 1000000M : 1M;



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

            option.CurrencyId = CurrencyId;
            return option;
        }

        private bool ValidateForSearch()
        {
            if (!datBaseDate.Value.HasValue)
            {
                datBaseDate.Focus();
                ShowWarningDialog(MsgWngInputRequired, lblBaseDate.Text);
                return false;
            }
            if (txtClosingDate.Enabled && string.IsNullOrEmpty(txtClosingDate.Text))
            {
                txtClosingDate.Focus();
                ShowWarningDialog(MsgWngInputRequired, lblClosingDate.Text);
                return false;
            }
            if (UseForeignCurrency
                && string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                txtCurrencyCode.Focus();
                ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text);
                return false;

            }
            if (!txtDepartmentCodeFrom.ValidateRange(txtDepartmentCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDepartment.Text))) return false;

            if (!txtStaffCodeFrom.ValidateRange(txtStaffCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaff.Text))) return false;

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomer.Text))) return false;

            var closingDay = (rdoClosingDate.Checked) ? Company.ClosingDay : int.Parse(txtClosingDate.Text);
            var carryOverAt = GetLastCarryOverAt();
            if (carryOverAt.HasValue)
            {
                var carryDate = carryOverAt.Value;
                var baseDate = datBaseDate.Value.Value.Date;
                if (carryDate.Day <= closingDay)
                {
                    if ((baseDate.Year == carryOverAt.Value.Year && baseDate.Month < carryOverAt.Value.Month)
                        || baseDate.Year < carryOverAt.Value.Year)
                    {
                        datBaseDate.Focus();
                        ShowWarningDialog(MsgWngRequiredOverCarryOverDate, lblBaseDate.Text);
                        return false;
                    }
                }
                else
                {
                    if ((baseDate.Year == carryOverAt.Value.Year && baseDate.Month < carryOverAt.Value.Month + 1)
                       || baseDate.Year < carryOverAt.Value.Year)
                    {
                        datBaseDate.Focus();
                        ShowWarningDialog(MsgWngRequiredOverCarryOverDate, lblBaseDate.Text);
                        return false;
                    }
                }
            }

            return true;
        }

        [OperationLog("印刷")]
        private void PrintBillingAging()
        {

            if (!ValidateForSearch())
            {
                return;
            }
            ClearStatusMessage();

            try
            {
                SetHeaderFormat();

                var serverPath = string.Empty;
                try
                {
                    var tasks = new List<Task> {
                        Task.Run(async () => serverPath = await Util.GetGeneralSettingServerPathAsync(Login)),
                        LoadReportSettingAsync(),
                    };
                    ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
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

                var option = GetSearchOption();
                var list = GetBaseTasks(option);
                list.Add(new TaskProgress("帳票の作成"));
                ProgressManager = new TaskProgressManager(list);

                var task = GetBillingAgingListReportAsync(option);
                var dialogResult = ProgressDialogStart(ParentForm, ParentForm.Text + " 印刷", task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
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
                ShowDialogPreview(ParentForm, report, serverPath);
            }

            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private async Task<GrapeCity.ActiveReports.SectionReport> GetBillingAgingListReportAsync(BillingAgingListSearch option)
        {
            BillingAgingSearchReportList = GetSearchReportCondition();
            if (UseForeignCurrency)
                await LoadCurrencyInfoAsync();

            var source = await GetBillingAgingListAsync(option);
            if (source == null)
            {
                ProgressManager.Canceled = true;
                return null;
            }
            if (GetReportSettingCustomerGroup() == ReportCustomerGroup.ParentOnly)
            {
                source = source.Where(x
                    => x.RecordType != 0
                    || !x.ParentCustomerId.HasValue
                    || x.ParentCustomerFlag == 1).ToList();
            }

            if (!source.Any())
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                return null;
            }

            GrapeCity.ActiveReports.SectionReport report = null;
            await Task.Run( () => report = GetReportBase(source));

            ProgressManager.UpdateState();

            return report;
        }

        private GrapeCity.ActiveReports.SectionReport GetReportBase(List<BillingAgingList> list)
        {
            var title = "請求残高年齢表";
            var outputName = $"{title}{DateTime.Today:yyyyMMdd}";
            GrapeCity.ActiveReports.SectionReport report = null;
            var remain = GetReportSettingBillingRemainType();
            if (remain == ReportAdvanceReceivedType.UseMatchingAmount)
            {
                var billingAgingReport1 = new BillingAgingListSectionReport1();
                billingAgingReport1.ConsiderCustomerGroup = GetReportSettingCustomerGroup() != ReportCustomerGroup.PlainCusomter;
                billingAgingReport1.DisplayCustomerCode = GetReportSettingDisplayCustomerCode() == ReportDoOrNot.Do;
                billingAgingReport1.RequireStaffSubtotal = GetReportSettingStaffTotal() == ReportDoOrNot.Do;
                billingAgingReport1.RequireDepartmentSubtotal = GetReportSettingDepartmentTotal() == ReportDoOrNot.Do;

                billingAgingReport1.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                billingAgingReport1.lblMonthlyRemain0.Text = MonthlyRemain0;
                billingAgingReport1.lblMonthlyRemain1.Text = MonthlyRemain1;
                billingAgingReport1.lblMonthlyRemain2.Text = MonthlyRemain2;
                billingAgingReport1.lblMonthlyRemain3.Text = MonthlyRemain3;
                billingAgingReport1.SetData(list, Precision, ApplicationControl.UseForeignCurrency);
                report = billingAgingReport1;
            }
            else
            {
                var billingAgingReport = new BillingAgingListSectionReport();
                billingAgingReport.ConsiderCustomerGroup = GetReportSettingCustomerGroup() != ReportCustomerGroup.PlainCusomter;
                billingAgingReport.DisplayCutsomerCode = GetReportSettingDisplayCustomerCode() == ReportDoOrNot.Do;
                billingAgingReport.RequireStaffSubtotal = GetReportSettingStaffTotal() == ReportDoOrNot.Do;
                billingAgingReport.RequireDepartmentSubtotal = GetReportSettingDepartmentTotal() == ReportDoOrNot.Do;
                billingAgingReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                billingAgingReport.lblMonthlyRemain0.Text = MonthlyRemain0;
                billingAgingReport.lblMonthlyRemain1.Text = MonthlyRemain1;
                billingAgingReport.lblMonthlyRemain2.Text = MonthlyRemain2;
                billingAgingReport.lblMonthlyRemain3.Text = MonthlyRemain3;
                billingAgingReport.SetData(list, Precision, ApplicationControl.UseForeignCurrency);
                report = billingAgingReport;
            }
            report.Name = outputName;
            var searchReport = new SearchConditionSectionReport();
            searchReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, title);
            searchReport.Name = outputName;
            searchReport.SetPageDataSetting(BillingAgingSearchReportList);
            report.Document.Name = outputName;
            report.Run(false);
            searchReport.SetPageNumber(report.Document.Pages.Count);
            searchReport.Run(false);
            report.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchReport.Document.Pages.Clone());
            return report;
        }

        [OperationLog("設定")]
        private void OpenPrintSetting()
        {
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PF0101);
                screen.InitializeParentForm("帳票設定");
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }

        [OperationLog("エクスポート")]
        private void ExportBillingAging()
        {
            ClearStatusMessage();
            if (!ValidateForSearch()) return;
            SetHeaderFormat();

            try
            {
                var serverPath = string.Empty;
                var tasks = new List<Task> {
                    Task.Run(async () => serverPath = await Util.GetGeneralSettingServerPathAsync(Login)),
                    LoadReportSettingAsync(),
                };
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"請求残高年齢表{datBaseDate.Value:yyMM}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var option = GetSearchOption();
                var list = GetBaseTasks(option);
                list.Add(new TaskProgress("データ出力"));

                ProgressManager = new TaskProgressManager(list);
                var task = ExportBillingAgingListAsync(filePath, option);
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

        private async Task<int> ExportBillingAgingListAsync(string filePath, BillingAgingListSearch option)
        {
            if (UseForeignCurrency)
            {
                await LoadCurrencyInfoAsync();
            }
            var source = await GetBillingAgingListAsync(option);

            if (source == null)
            {
                ProgressManager.Canceled = true;
                return 0;
            }
            if (!source.Any())
            {
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                return 0;
            }

            var group = GetReportSettingCustomerGroup();
            if (group != ReportCustomerGroup.PlainCusomter)
            {
                foreach (var item in source)
                {
                    if (!item.ParentCustomerId.HasValue
                    || item.ParentCustomerFlag != 0) continue;
                    item.CustomerName = $" *{item.CustomerName}";
                }
            }
            var exportBillingAgingList = source.Where(x
                => x.RecordType == 0
                && (group != ReportCustomerGroup.ParentOnly
                    || !x.ParentCustomerId.HasValue
                    || x.ParentCustomerFlag == 1)).ToList();


            var definition = new BillingAgingListFileDefinition(
                    new DataExpression(ApplicationControl), MonthlyRemain0, MonthlyRemain1, MonthlyRemain2, MonthlyRemain3);

            var remain = GetReportSettingBillingRemainType();
            if (definition.CurrentMonthReceiptField.Ignored = (remain == ReportAdvanceReceivedType.UseMatchingAmount))
            {
                definition.CurrentMonthReceiptField.FieldName = null;
            }
            var subDep = GetReportSettingDepartmentTotal();
            if (definition.DepartmentCodeField.Ignored = (subDep == ReportDoOrNot.NotDo))
            {
                definition.DepartmentCodeField.FieldName = null;
            }
            if (definition.DepartmentNameField.Ignored = (subDep == ReportDoOrNot.NotDo))
            {
                definition.DepartmentNameField.FieldName = null;
            }
            var subStaff = GetReportSettingStaffTotal();
            if (definition.StaffCodeField.Ignored = (subStaff == ReportDoOrNot.NotDo))
            {
                definition.StaffCodeField.FieldName = null;
            }
            if (definition.StaffNameField.Ignored = (subStaff == ReportDoOrNot.NotDo))
            {
                definition.StaffNameField.FieldName = null;
            }

            var decimalFormat = "0" + ((Precision == 0) ? string.Empty : "." + new string('0', Precision));
            definition.LastMonthRemainField.Format = value => value.ToString(decimalFormat);
            definition.CurrenctMonthSaleField.Format = value => value.ToString(decimalFormat);
            definition.CurrentMonthReceiptField.Format = value => value.ToString(decimalFormat);
            definition.CurrentMonthMatchingField.Format = value => value.ToString(decimalFormat);
            definition.CurrenttMonthRemainField.Format = value => value.ToString(decimalFormat);
            definition.MonthlyRemain0Field.Format = value => value.ToString(decimalFormat);
            definition.MonthlyRemain1Field.Format = value => value.ToString(decimalFormat);
            definition.MonthlyRemain2Field.Format = value => value.ToString(decimalFormat);
            definition.MonthlyRemain3Field.Format = value => value.ToString(decimalFormat);

            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = Login.CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            var result = await exporter.ExportAsync(filePath, exportBillingAgingList, new System.Threading.CancellationToken(false), new Progress<int>());
            if (result == 0) ProgressManager.Abort();
            ProgressManager.UpdateState();
            return result;
        }

        [OperationLog("クリア")]
        private void ClearBillingAging()
        {
            ClearStatusMessage();
            Clear();
        }

        private void Clear()
        {
            rdoClosingDate.Checked = true;
            txtClosingDate.Clear();
            txtClosingDate.Enabled = false;
            tbcBillingAgingList.SelectedIndex = 0;
            datBaseDate.Clear();
            datBaseDate.Focus();
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
            txtCurrencyCode.Clear();
            lblDispSelectedBaseDate.Clear();
            lblDispSubTotalStaff.Clear();
            lblDispSubTotalDepartment.Clear();
            lblDispStaffSelection.Clear();
            lblDispCustomerGroup.Clear();
            lblDispAdvanceReceiptType.Clear();
            lblDispUnitPrice.Clear();
            BaseContext.SetFunction07Enabled(true);
        }

        [OperationLog("終了")]
        private void ExitBillingAging()
        {
            try
            {
                Settings.SaveControlValue<PF0101>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PF0101>(Login, cbxCustomer.Name, cbxCustomer.Checked);
                Settings.SaveControlValue<PF0101>(Login, cbxStaff.Name, cbxStaff.Checked);
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
            tbcBillingAgingList.SelectedIndex = 0;
        }

        #endregion

        #region Webサービス呼び出し
        private async Task LoadReportSettingAsync()
        {
            var result = new ReportSettingsResult();
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<ReportSettingMasterClient>();
                result = await client.GetItemsAsync(
                       SessionKey, CompanyId, nameof(PF0101));
            });
            if (result.ProcessResult.Result)
            {
                ReportSettingList = result.ReportSettings;
            }
        }

        private async Task LoadCurrencyInfoAsync()
        {
            Currency currency = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CurrencyMasterClient>();
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });
                if (result.ProcessResult.Result)
                    currency = result.Currencies?.FirstOrDefault();
            });
            CurrencyId = currency?.Id;
            if (currency != null)
            {
                Precision = currency.Precision;
            }
        }

        private async Task<string> LoadInitialDirectoryAsync()
        {
            var result = string.Empty;
            const string code = "サーバパス";
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<GeneralSettingMasterClient>();
                var settingResult = await client.GetByCodeAsync(SessionKey, CompanyId, code);

                if (settingResult.ProcessResult.Result)
                    result = settingResult.GeneralSetting?.Value;
            });
            return result;
        }

        private DateTime? GetLastCarryOverAt()
        {
            var task = LoadLastCarryOverAtAsync();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        private async Task<DateTime?> LoadLastCarryOverAtAsync()
        {
            DateTime? result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<BillingBalanceServiceClient>();
                var getResult = await client.GetLastCarryOverAtAsync(SessionKey, CompanyId);
                if (getResult.ProcessResult.Result)
                    result = getResult.LastCarryOverAt;
            });
            return result;
        }

        private System.Action OnCancelHandler { get; set; }

        private async Task<List<BillingAgingList>> GetBillingAgingListAsync(BillingAgingListSearch searchOption)
        {
            List<BillingAgingList> result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<BillingAgingListServiceClient>(async client =>
                {
                    var getResult = await client.GetAsync(SessionKey, searchOption, hubConnection.ConnectionId);
                    if (getResult.ProcessResult.Result)
                        result = getResult.BillingAgingLists;
                });
            }
            return result;
        }

        private async Task<List<BillingAgingListDetail>> GetBillingAgingListDetailAsync(
            BillingAgingListSearch searchOption)
        {
            List<BillingAgingListDetail> result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<BillingAgingListServiceClient>();
                var getResult = await client.GetDetailsAsync(SessionKey, searchOption);
                if (getResult.ProcessResult.Result)
                    result = getResult.BillingAgingListDetails;

            });
            return result ?? new List<BillingAgingListDetail>();
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

        #region event handlers

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

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                CurrencyId = currency.Id;
                ClearStatusMessage();
            }
        }

        private void rdoCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCustomer.Checked)
            {
                txtClosingDate.Enabled = true;
                txtClosingDate.Focus();
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

            if (txtClosingDate.Text == "00")
            {
                txtClosingDate.Clear();
                txtClosingDate.Focus();
                return;
            }
            if (int.Parse(txtClosingDate.Text) >= 28)
            {
                txtClosingDate.Text = "99";
            }
        }

        private void txtDepartmentCodeFrom_Validated(object sender, EventArgs e)
        {

            var departmentCodeFrom = txtDepartmentCodeFrom.Text;
            var departmentNameTo = string.Empty;
            var departmentNameFrom = string.Empty;
            Department department = null;
            try
            {

                if (string.IsNullOrEmpty(departmentCodeFrom))
                {
                    if (cbxDepartment.Checked)
                    {
                        txtDepartmentCodeTo.Text = departmentCodeFrom;
                        lblDepartmentNameTo.Clear();
                    }
                    lblDepartmentNameFrom.Clear();
                    ClearStatusMessage();
                    return;
                }
                var task = GetDepartmentAsync(departmentCodeFrom);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                department = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (department != null)
            {
                departmentCodeFrom = department.Code;
                departmentNameFrom = department.Name;
                departmentNameTo = department.Name;
                ClearStatusMessage();
            }

            txtDepartmentCodeFrom.Text = departmentCodeFrom;
            lblDepartmentNameFrom.Text = departmentNameFrom;

            if (cbxDepartment.Checked)
            {
                txtDepartmentCodeTo.Text = departmentCodeFrom;
                lblDepartmentNameTo.Text = departmentNameTo;
            }
        }

        private void txtDepartmentCodeTo_Validated(object sender, EventArgs e)
        {

            var departmentCodeTo = txtDepartmentCodeTo.Text;
            var departmentNameTo = string.Empty;
            Department department = null;
            try
            {

                if (string.IsNullOrEmpty(departmentCodeTo))
                {
                    txtDepartmentCodeTo.Text = departmentCodeTo;
                    lblDepartmentNameTo.Clear();
                    ClearStatusMessage();
                    return;
                }
                var task = GetDepartmentAsync(departmentCodeTo);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                department = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if (department != null)
            {
                departmentCodeTo = department.Code;
                departmentNameTo = department.Name;
                ClearStatusMessage();
            }
            txtDepartmentCodeTo.Text = departmentCodeTo;
            lblDepartmentNameTo.Text = departmentNameTo;

        }

        private void txtStaffCodeFrom_Validated(object sender, EventArgs e)
        {
            var staffCodeFrom = txtStaffCodeFrom.Text;
            var staffNameTo = string.Empty;
            var staffNameFrom = string.Empty;
            Staff staff = null;

            try
            {
                if (string.IsNullOrEmpty(staffCodeFrom))
                {
                    if (cbxStaff.Checked)
                    {
                        txtStaffCodeTo.Text = staffCodeFrom;
                        lblStaffNameTo.Clear();
                    }
                    lblStaffNameFrom.Clear();
                    ClearStatusMessage();
                    return;
                }
                var task = GetStaffAsync(staffCodeFrom);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                staff = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if (staff != null)
            {
                staffCodeFrom = staff.Code;
                staffNameFrom = staff.Name;
                staffNameTo = staff.Name;
                ClearStatusMessage();
            }

            txtStaffCodeFrom.Text = staffCodeFrom;
            lblStaffNameFrom.Text = staffNameFrom;

            if (cbxStaff.Checked)
            {
                txtStaffCodeTo.Text = staffCodeFrom;
                lblStaffNameTo.Text = staffNameTo;
            }
        }

        private void txtStaffCodeTo_Validated(object sender, EventArgs e)
        {
            var staffCodeTo = txtStaffCodeTo.Text;
            var staffNameTo = string.Empty;
            Staff staff = null;

            try
            {
                if (string.IsNullOrEmpty(staffCodeTo))
                {
                    lblStaffNameTo.Clear();
                    ClearStatusMessage();
                    return;
                }
                var task = GetStaffAsync(staffCodeTo);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                staff = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if (staff != null)
            {
                staffCodeTo = staff.Code;
                staffNameTo = staff.Name;
                ClearStatusMessage();
            }
            txtStaffCodeTo.Text = staffCodeTo;
            lblStaffNameTo.Text = staffNameTo;
        }

        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            var customerCodeFrom = txtCustomerCodeFrom.Text;
            var customerNameTo = string.Empty;
            var customerNameFrom = string.Empty;

            Customer customer = null;
            try
            {
                if (string.IsNullOrEmpty(customerCodeFrom))
                {
                    if (cbxCustomer.Checked)
                    {
                        txtCustomerCodeTo.Text = customerCodeFrom;
                        lblCustomerNameTo.Clear();
                    }
                    lblCustomerNameFrom.Clear();
                    ClearStatusMessage();
                    return;
                }
                {
                }
                var task = GetCustomerAsync(customerCodeFrom);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                customer = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if (customer != null)
            {
                customerCodeFrom = customer.Code;
                customerNameFrom = customer.Name;
                customerNameTo = customer.Name;
                ClearStatusMessage();
            }

            txtCustomerCodeFrom.Text = customerCodeFrom;
            lblCustomerNameFrom.Text = customerNameFrom;

            if (cbxCustomer.Checked)
            {
                txtCustomerCodeTo.Text = customerCodeFrom;
                lblCustomerNameTo.Text = customerNameFrom;
            }

        }

        private void txtCustomerCodeTo_Validated(object sender, EventArgs e)
        {
            var customerCodeTo = txtCustomerCodeTo.Text;
            var customerNameTo = string.Empty;
            Customer customerResult = null;
            try
            {

                if (string.IsNullOrEmpty(customerCodeTo))
                {
                    txtCustomerCodeTo.Text = customerCodeTo;
                    lblCustomerNameTo.Clear();
                    ClearStatusMessage();
                    return;
                }
                var task = GetCustomerAsync(customerCodeTo);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                customerResult = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if (customerResult != null)
            {
                customerCodeTo = customerResult.Code;
                customerNameTo = customerResult.Name;
                ClearStatusMessage();
            }
            txtCustomerCodeTo.Text = customerCodeTo;
            lblCustomerNameTo.Text = customerNameTo;

        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                CurrencyId = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                ProgressDialog.Start(ParentForm, LoadCurrencyInfoAsync(), false, SessionKey);
                if (!CurrencyId.HasValue)
                {
                    txtCurrencyCode.Clear();
                    txtCurrencyCode.Focus();
                    ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                    return;
                }
                ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region grid event handler

        private void grd_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            ClearStatusMessage();
            if (e.CellName != CellName("chk")) return;
            var row = grdSearchResult.Rows[e.RowIndex];
            var rowData = row.DataBoundItem as BillingAgingList;
            if (rowData == null
                || rowData.ParentCustomerFlag == 0) return;
            try
            {
                Cursor = Cursors.WaitCursor;
                grdSearchResult.SuspendLayout();
                grdSearchResult.CellEditedFormattedValueChanged -= grd_CellEditedFormattedValueChanged;

                var parentId = rowData.ParentCustomerId;
                var cbx = row[CellName("chk")] as CheckBoxCell;
                var visible = "+".Equals(cbx.Text);
                cbx.Text = visible ? "-" : "+";
                cbx.Value = cbx.Text;
                var step = e.RowIndex + 1;
                while (step < grdSearchResult.RowCount)
                {
                    row = grdSearchResult.Rows[step];
                    rowData = row.DataBoundItem as BillingAgingList;
                    if (parentId != rowData.ParentCustomerId) break;
                    row.Visible = visible;
                    step++;
                }
            }
            finally
            {
                grdSearchResult.ResumeLayout(true);
                ClearStatusMessage();
                Cursor = Cursors.Default;
                grdSearchResult.CellEditedFormattedValueChanged += grd_CellEditedFormattedValueChanged;
            }

        }

        private void grdSearchResult_DataBindingComplete(object sender, MultiRowBindingCompleteEventArgs e)
        {
            if (grdSearchResult.RowCount == 0) return;
            if (GetReportSettingCustomerGroup() != ReportCustomerGroup.ParentOnly) return;

            foreach (Row row in grdSearchResult.Rows)
            {
                var data = row.DataBoundItem as BillingAgingList;
                if (data == null) continue;
                if (data?.ParentCustomerFlag == 0
                    && data.RecordType == 0
                    && data.ParentCustomerId.HasValue)
                    row.Visible = false;
            }

        }

        //債権代表者表示切り替えボタン何もしないため設定処理
        private void grdSearchResult_CellPainting(object sender, CellPaintingEventArgs e)
        {
            if (e.CellName == CellName("chk"))
            {
                var data = grdSearchResult.Rows[e.RowIndex].DataBoundItem as BillingAgingList;
                if (data == null) return;
                if (data.ParentCustomerFlag == 1
                    && data.RecordType == 0) return;

                var rect = e.CellBounds;
                e.PaintBackground(rect);
                e.PaintBorder(rect);
                e.PaintWaveLine(rect);
                e.Handled = true;
            }
        }


        private Color StaffTotalBackColor { get; } = Color.LightCyan;
        private Color DeparatmentTotalBackColor { get; } = Color.NavajoWhite;
        private Color GrandTotalBackColor { get; } = Color.Khaki;
        /// <summary>
        /// 合計行 背景色変更/ 債権代表者グループ 得意先名 マーク表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSearchResult_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.RowIndex < 0
                || e.CellName == CellName("Header")) return;

            var data = grdSearchResult.Rows[e.RowIndex].DataBoundItem as BillingAgingList;
            if (data == null) return;
            if (data.RecordType == 0)
            {
                if (e.CellName == CellName(nameof(BillingAgingList.CustomerName))
                    && data.ParentCustomerId.HasValue
                    && data.ParentCustomerFlag == 0
                    && GetReportSettingCustomerGroup() != ReportCustomerGroup.PlainCusomter)
                    e.Value = $" *{e.Value}";
            }
            else
            {
                e.CellStyle.BackColor =
                    data.RecordType == 1 ? StaffTotalBackColor :
                    data.RecordType == 2 ? DeparatmentTotalBackColor :
                    data.RecordType == 3 ? GrandTotalBackColor : Color.Empty;
                if (data.RecordType == 3
                    && UseForeignCurrency
                    && e.CellName == CellName(nameof(BillingAgingList.TotalText)))
                {
                    e.Value = "通貨計";
                }
            }

        }

        //PF0102 請求残高年齢表(明細)画面に遷移処理
        private void grdSearchResult_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var detail = grdSearchResult.Rows[e.RowIndex].DataBoundItem as BillingAgingList;

            var recordType = detail.RecordType;
            var parentCustomerId = detail.ParentCustomerId;
            var parentFlag = detail.ParentCustomerFlag;
            var currentCustomerCode = detail.CustomerCode;

            if (recordType != 0
                || parentCustomerId.HasValue && parentFlag == 0
                || !(e.CellName.StartsWith(CellName("MonthlyRemain")))) return;

            var customerId = detail.CustomerId;
            var offset =
                e.CellName == CellName(nameof(BillingAgingList.MonthlyRemain0)) ? 0 :
                e.CellName == CellName(nameof(BillingAgingList.MonthlyRemain1)) ? 1 :
                e.CellName == CellName(nameof(BillingAgingList.MonthlyRemain2)) ? 2 :
                e.CellName == CellName(nameof(BillingAgingList.MonthlyRemain3)) ? 3 : 0;

            var option = GetSearchOption();
            option.MonthOffset = offset;
            option.CustomerId = customerId;
            if (GetReportSettingDepartmentTotal() == ReportDoOrNot.Do)
            {
                option.DepartmentCodeFrom = detail.DepartmentCode;
                option.DepartmentCodeTo = detail.DepartmentCode;
            }
            if (GetReportSettingStaffTotal() == ReportDoOrNot.Do)
            {
                option.StaffCodeFrom = detail.StaffCode;
                option.StaffCodeTo = detail.StaffCode;
            }

            List<BillingAgingListDetail> list = null;
            try
            {
                var task = GetBillingAgingListDetailAsync(option);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                list = task.Result;
            }
            catch (AggregateException ex)
            {
                Debug.Fail(ex.InnerException.ToString());
                NLogHandler.WriteErrorLog(this, ex.InnerException, SessionKey);
            }

            if (!list.Any())
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }
            using (var form = ApplicationContext.Create(nameof(PF0102)))
            {
                var screen = form.GetAll<PF0102>().FirstOrDefault();
                form.StartPosition = FormStartPosition.CenterParent;
                screen.BillingAgingDetailList = list;
                screen.ReportCondition = DetailSearchConditionList;

                BillingAgingSearchCondition.CustomerCodeFrom = currentCustomerCode;

                BillingAgingSearchCondition.DepartmentNameFrom = lblDepartmentNameFrom.Text;
                BillingAgingSearchCondition.DepartmentNameTo = lblDepartmentNameTo.Text;
                BillingAgingSearchCondition.StaffNameFrom = lblStaffNameFrom.Text;
                BillingAgingSearchCondition.StaffNameTo = lblStaffNameTo.Text;
                screen.SearchCondition = BillingAgingSearchCondition;
                screen.Precision = Precision;
                var result = ApplicationContext.ShowDialog(ParentForm, form);
            }
        }

        #endregion

        #region report settings

        private ReportTargetDate GetReportSettingTargetDate() => ReportSettingList.GetReportSetting<ReportTargetDate>(TargetDate);
        private ReportAdvanceReceivedType GetReportSettingBillingRemainType() => ReportSettingList.GetReportSetting<ReportAdvanceReceivedType>(BillingRemainType);
        private ReportCustomerGroup GetReportSettingCustomerGroup() => ReportSettingList.GetReportSetting<ReportCustomerGroup>(CustomerGroupType);
        private ReportStaffSelection GetReportSettingStaffSelection() => ReportSettingList.GetReportSetting<ReportStaffSelection>(StaffType);
        private ReportDoOrNot GetReportSettingDepartmentTotal() => ReportSettingList.GetReportSetting<ReportDoOrNot>(DepartmentTotal);
        private ReportDoOrNot GetReportSettingStaffTotal() => ReportSettingList.GetReportSetting<ReportDoOrNot>(StaffTotal);
        private ReportDoOrNot GetReportSettingDisplayCustomerCode() => ReportSettingList.GetReportSetting<ReportDoOrNot>(DisplayCustomerCode);
        private ReportUnitPrice GetReportSettingUnitPrice() => ReportSettingList.GetReportSetting<ReportUnitPrice>(UnitPrice);

        private string GetReportSettingNameTargetDate()
        {
            var target = GetReportSettingTargetDate();
            return target == ReportTargetDate.BilledAt ? "請求日"
                : target == ReportTargetDate.SalesAt ? "売上日" : string.Empty;
        }
        private string GetReportSettingNameBillingRemainType()
        {
            var remain = GetReportSettingBillingRemainType();
            return
                remain == ReportAdvanceReceivedType.UseMatchingAmount ? "消込額を使用する" :
                remain == ReportAdvanceReceivedType.UseReceiptAmount ? "入金額を使用する" :
                remain == ReportAdvanceReceivedType.UseMatchingAmountWithReceiptAmount ? "消込額を使用して入金額を表示" : string.Empty;
        }
        private string GetReportSettingNameCustomerGroup()
        {
            var group = GetReportSettingCustomerGroup();
            return
                group == ReportCustomerGroup.PlainCusomter ? "得意先" :
                group == ReportCustomerGroup.ParentWithChildren ? "債権代表者/得意先" :
                group == ReportCustomerGroup.ParentOnly ? "債権代表者" : string.Empty;
        }
        private string GetReportSettingNameStaffSelection()
        {
            var staff = GetReportSettingStaffSelection();
            return
                staff == ReportStaffSelection.ByBillingData ? "請求データ" :
                staff == ReportStaffSelection.ByCustomerMaster ? "得意先マスター" : string.Empty;
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
