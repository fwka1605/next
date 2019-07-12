using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.ReportService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>滞留明細一覧表</summary>
    public partial class PF0401 : VOneScreenBase
    {
        // 変数宣言
        private int Precision { get; set; }
        List<ReportSetting> ReportSettingList { get; set; }
        List<object> SearchListCondition { get; set; }
        private List<ColumnNameSetting> ColumnSettingInfo { get; set; }
        private ArrearagesListSearch SearchConditon { get; set; }
        private string CellName(string value) => $"cel{value}";
      
        public PF0401()
        {
            InitializeComponent();
            grdArrearagesList.SetupShortcutKeys();
            Text = "滞留明細一覧表";
            ReportSettingList = new List<ReportSetting>();
            InitializeHandlers();
        }
        private void InitializeHandlers()
        {
            tbcArrearagesList.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcArrearagesList.SelectedIndex == 0)
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

        #region PF0401 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = Print;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Caption("設定");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = PrintSetting;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PF0401_Load(object sender, EventArgs e)
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
                tbcArrearagesList.SelectedIndex = 0;
                ResumeLayout();
                SetInitialSetting();
                InitializeArrearagesListGrid();
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
                    ColumnSettingInfo = result.ColumnNames.Where(x => x.TableName == "Billing").ToList();
                    
                }
            });
        }
        #endregion

        #region 画面の Format & Warning Setting
        private void SetInitialSetting()
        {
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);

                txtFromDepartmentCode.Format = expression.DepartmentCodeFormatString;
                txtFromDepartmentCode.MaxLength = expression.DepartmentCodeLength;
                txtFromDepartmentCode.PaddingChar = expression.DepartmentCodePaddingChar;

                txtToDapartmentCode.Format = expression.DepartmentCodeFormatString;
                txtToDapartmentCode.MaxLength = expression.DepartmentCodeLength;
                txtToDapartmentCode.PaddingChar = expression.DepartmentCodePaddingChar;

                txtFromStaffCode.Format = expression.StaffCodeFormatString;
                txtFromStaffCode.MaxLength = expression.StaffCodeLength;
                txtFromStaffCode.PaddingChar = expression.StaffCodePaddingChar;

                txtToStaffCode.Format = expression.StaffCodeFormatString;
                txtToStaffCode.MaxLength = expression.StaffCodeLength;
                txtToStaffCode.PaddingChar = expression.StaffCodePaddingChar;

                if (UseForeignCurrency)
                {
                    lblCurrency.Visible = true;
                    txtCurrencyCode.Visible = true;
                    btnCurrency.Visible = true;
                }
            }

            Settings.SetCheckBoxValue<PF0401>(Login, cbxDepartment);
            Settings.SetCheckBoxValue<PF0401>(Login, cbxStaff);

            datPaymentDate.Value = DateTime.Today;
            this.ActiveControl = datPaymentDate;
            datPaymentDate.Focus();
        }

        private async Task<Currency> GetCurrencyInfo()
        {
            var currency = new Currency();
            BaseContext.SetFunction07Enabled(true);

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });

                currency = result.Currencies.FirstOrDefault();

                if (currency != null)
                    Precision = currency.Precision;
            });

            return currency;
        }

        #endregion

        #region グリッド作成
        private void InitializeArrearagesListGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var isAggregate = cbxCustomer.Checked;
            var widthId = isAggregate ? 0 : 90;
            var widthNormal = isAggregate ? 0 : 115;
            var widthDay = isAggregate ? 0 : 60;
            var widthCcy = UseForeignCurrency ? 60 : 0;
            var nameNote1 = "";
            var nameNote2 = "";
            var nameNote3 = "";
            var nameNote4 = "";
            foreach (var setting in ColumnSettingInfo)
            {
                switch (setting.ColumnName)
                {
                    case nameof(ArrearagesList.Note1): nameNote1 = setting.DisplayColumnName; break;
                    case nameof(ArrearagesList.Note2): nameNote2 = setting.DisplayColumnName; break;
                    case nameof(ArrearagesList.Note3): nameNote3 = setting.DisplayColumnName; break;
                    case nameof(ArrearagesList.Note4): nameNote4 = setting.DisplayColumnName; break;
                }
            }
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,     widthId, nameof(ArrearagesList.Id)                 , dataField: nameof(ArrearagesList.Id)                 , caption: "請求ID"        , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight), sortable: true),
                new CellSetting(height,         115, nameof(ArrearagesList.CustomerCode)       , dataField: nameof(ArrearagesList.CustomerCode)       , caption: "得意先コード"  , cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height,         150, nameof(ArrearagesList.CustomerName)       , dataField: nameof(ArrearagesList.CustomerName)       , caption: "得意先名"      , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.BilledAt)           , dataField: nameof(ArrearagesList.BilledAt)           , caption: "請求日"        , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.SalesAt)            , dataField: nameof(ArrearagesList.SalesAt)            , caption: "売上日"        , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.ClosingAt)          , dataField: nameof(ArrearagesList.ClosingAt)          , caption: "請求締日"      , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.DueAt)              , dataField: nameof(ArrearagesList.DueAt)              , caption: "入金予定日"    , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.OriginalDueAt)      , dataField: nameof(ArrearagesList.OriginalDueAt)      , caption: "当初予定日"    , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height,    widthCcy, nameof(ArrearagesList.CurrencyCode)       , dataField: nameof(ArrearagesList.CurrencyCode)       , caption: "通貨コード"    , cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height,         120, nameof(ArrearagesList.RemainAmount)       , dataField: nameof(ArrearagesList.RemainAmount)       , caption: "回収予定金額"  , cell: builder.GetNumberCellCurrency(Precision, Precision, 0), sortable: true),
                new CellSetting(height,    widthDay, nameof(ArrearagesList.ArrearagesDayCount) , dataField: nameof(ArrearagesList.ArrearagesDayCount) , caption: "滞留日数"      , cell: builder.GetNumberCell(MultiRowContentAlignment.MiddleRight), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.CodeAndName)        , dataField: nameof(ArrearagesList.CodeAndName)        , caption: "回収区分"      , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.InvoiceCode)        , dataField: nameof(ArrearagesList.InvoiceCode)        , caption: "請求書番号"    , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.Note1)              , dataField: nameof(ArrearagesList.Note1)              , caption: nameNote1       , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.Memo)               , dataField: nameof(ArrearagesList.Memo)               , caption: "メモ"          , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.CustomerStaffName)  , dataField: nameof(ArrearagesList.CustomerStaffName)  , caption: "相手先担当者"  , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.CustomerNote)       , dataField: nameof(ArrearagesList.CustomerNote)       , caption: "得意先備考"    , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.Tel)                , dataField: nameof(ArrearagesList.Tel)                , caption: "電話番号"      , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height,         115, nameof(ArrearagesList.DepartmentCode)     , dataField: nameof(ArrearagesList.DepartmentCode)     , caption: "請求部門コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height,         150, nameof(ArrearagesList.DepartmentName)     , dataField: nameof(ArrearagesList.DepartmentName)     , caption: "請求部門名"    , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height,         115, nameof(ArrearagesList.StaffCode)          , dataField: nameof(ArrearagesList.StaffCode)          , caption: "担当者コード"  , cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height,         150, nameof(ArrearagesList.StaffName)          , dataField: nameof(ArrearagesList.StaffName)          , caption: "担当者名"      , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.Note2)              , dataField: nameof(ArrearagesList.Note2)              , caption: nameNote2       , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.Note3)              , dataField: nameof(ArrearagesList.Note3)              , caption: nameNote3       , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ArrearagesList.Note4)              , dataField: nameof(ArrearagesList.Note4)              , caption: nameNote4       , cell: builder.GetTextBoxCell(), sortable: true)
            });
            grdArrearagesList.Template = builder.Build();
            grdArrearagesList.HideSelection = true;
        }
        #endregion

        #region Function Key Event
        [OperationLog("検索")]
        private void Search()
        {
            try
            {
                ClearStatusMessage();

                if (!ValidateSearchData()) return;
                SearchConditon = GetSearchDataCondition();
                int count = 0;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    count = await SearchArrearagesList(SearchConditon);
                });
                NLogHandler.WriteDebug(this, "滞留明細一覧表 検索開始");
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                NLogHandler.WriteDebug(this, "滞留明細一覧表 検索終了");

                if (count == 0)
                    ShowWarningDialog(MsgWngNotExistSearchData);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>検索項目チェック</summary>
        /// <returns>検索チェック結果（bool）</returns>
        private bool ValidateSearchData()
        {
            if (!datPaymentDate.Value.HasValue)
            {
                tbcArrearagesList.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "入金基準日");
                datPaymentDate.Focus();
                return false;
            }

            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                tbcArrearagesList.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Focus();
                return false;
            }

            if (!txtFromDepartmentCode.ValidateRange(txtToDapartmentCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblFromDepartment.Text))) return false;

            if (!txtFromStaffCode.ValidateRange(txtToStaffCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaff.Text))) return false;

            return true;
        }

        /// <summary>検索データ</summary>
        /// <returns>ArrearagesListSearch</returns>
        private ArrearagesListSearch GetSearchDataCondition()
        {
            var arrearagesListSearch = new ArrearagesListSearch();

            arrearagesListSearch.BaseDate = datPaymentDate.Value.Value;

            if (cbxMemo.Checked)
            {
                arrearagesListSearch.ExistsMemo = true;
            }
            if (!string.IsNullOrWhiteSpace(txtMemo.Text))
            {
                arrearagesListSearch.BillingMemo = txtMemo.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                arrearagesListSearch.CurrencyCode = txtCurrencyCode.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtFromDepartmentCode.Text))
            {
                arrearagesListSearch.DepartmentCodeFrom = txtFromDepartmentCode.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtToDapartmentCode.Text))
            {
                arrearagesListSearch.DepartmentCodeTo = txtToDapartmentCode.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtFromStaffCode.Text))
            {
                arrearagesListSearch.StaffCodeFrom = txtFromStaffCode.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtToStaffCode.Text))
            {
                arrearagesListSearch.StaffCodeTo = txtToStaffCode.Text;
            }
            if (cbxCustomer.Checked)
            {
                arrearagesListSearch.CustomerSummaryFlag = true;
            }

            return arrearagesListSearch;
        }

        /// <summary>検索処理</summary>
        /// <param name="arrearagesSearch">検索条件</param>
        private async Task<int> SearchArrearagesList(ArrearagesListSearch arrearagesSearch)
        {
            var result = new ArrearagesListsResult();
            decimal totalAmount = 0M;
            int count = 0;       

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReportServiceClient>();
                result = await service.ArrearagesListAsync(SessionKey, CompanyId, arrearagesSearch);
                if (result.ProcessResult.Result)
                {
                    grdArrearagesList.DataSource = new BindingSource(result.ArrearagesLists, null);
                    count = result.ArrearagesLists.Count;
                    if (count > 0)
                    {
                        InitializeArrearagesListGrid();
                        totalAmount = grdArrearagesList.Rows.Where(x => !string.IsNullOrEmpty(x[CellName(nameof(ArrearagesList.RemainAmount))].DisplayText.ToString()))
                                    .Sum(x => Convert.ToDecimal(x[CellName(nameof(ArrearagesList.RemainAmount))].DisplayText.ToString()));
                        lblRecoveryTotalAmount.Text = totalAmount.ToString(GetNumberFormat());
                        tbcArrearagesList.SelectedTab = tabArrearagesListResult;
                        BaseContext.SetFunction04Enabled(true);
                        BaseContext.SetFunction06Enabled(true);
                        BaseContext.SetFunction07Enabled(false);
                        tbcArrearagesList.SelectedIndex = 1;
                    }
                    else
                    {
                        tbcArrearagesList.SelectedTab = tabArrearagesListSearch;
                        grdArrearagesList.DataSource = null;
                        lblRecoveryTotalAmount.Clear();
                        BaseContext.SetFunction04Enabled(false);
                        BaseContext.SetFunction06Enabled(false);                       
                    }
                }
            });

            return count;
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            datPaymentDate.Value = DateTime.Today;
            cbxCustomer.Checked = false;
            txtFromDepartmentCode.Clear();
            lblFromDepartmentName.Clear();
            cbxMemo.Checked = false;
            txtMemo.Clear();
            txtToDapartmentCode.Clear();
            lblToDepartmentName.Clear();
            txtCurrencyCode.Clear();
            txtFromStaffCode.Clear();
            lblFromStaffName.Clear();
            txtToStaffCode.Clear();
            lblToStaffName.Clear();
            lblRecoveryTotalAmount.Clear();
            cbxMemo.Enabled = true;
            txtMemo.Enabled = true;
            grdArrearagesList.Rows.Clear();
            tbcArrearagesList.SelectedTab = tabArrearagesListSearch;
            datPaymentDate.Select();
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(true);
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

        [OperationLog("印刷")]
        private void Print()
        {
            ClearStatusMessage();

            try
            {
                ReportSearchCondition();

                GrapeCity.ActiveReports.SectionReport report = null;
                var arrearagesList = grdArrearagesList.Rows.Select(x => x.DataBoundItem as ArrearagesList).ToList();
                string serverPath = string.Empty;

                if (arrearagesList.Any())
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        serverPath = await GetServerPath();
                        var divVal = 1;
                        if (!UseForeignCurrency)
                        {
                            switch (ReportSettingList[4].ItemKey)
                            {
                                case "1": divVal = 1000; break;
                                case "2": divVal = 10000; break;
                                case "3": divVal = 1000000; break;
                            }
                        }

                        for (var i = 0; i < arrearagesList.Count; i++)
                        {
                            decimal remainAmount = RoundingType.Ceil.Calc(arrearagesList[i].RemainAmount, Precision).Value;
                            if (Precision == 0)
                            {
                                arrearagesList[i].TotalAmount = decimal.Truncate(remainAmount / divVal);
                            }
                            else
                            {
                                arrearagesList[i].TotalAmount = remainAmount / divVal;
                            }
                        }

                        var searchConditionReport = new SearchConditionSectionReport();
                        searchConditionReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "滞留明細一覧表");
                        searchConditionReport.Name = "滞留明細一覧表" + DateTime.Today.ToString("yyyyMMdd");
                        searchConditionReport.SetPageDataSetting(SearchListCondition);

                        if (SearchConditon.CustomerSummaryFlag)
                        {
                            var arrearagesCustomerListReport = new ArrearagesCustomerListReport();
                            arrearagesCustomerListReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            arrearagesCustomerListReport.Name = "滞留明細一覧表" + DateTime.Today.ToString("yyyyMMdd");
                            arrearagesCustomerListReport.SetData(arrearagesList, Precision, ReportSettingList);

                            arrearagesCustomerListReport.Run(false);
                            searchConditionReport.SetPageNumber(arrearagesCustomerListReport.Document.Pages.Count);
                            searchConditionReport.Run(false);

                            arrearagesCustomerListReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchConditionReport.Document.Pages.Clone());
                            report = arrearagesCustomerListReport;
                        }
                        else
                        {
                            var arrearagesListReport = new ArrearagesListReport();
                            arrearagesListReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            arrearagesListReport.Name = "滞留明細一覧表" + DateTime.Today.ToString("yyyyMMdd");
                            arrearagesListReport.SetData(arrearagesList, Precision, ReportSettingList, ColumnSettingInfo);

                            arrearagesListReport.Run(false);
                            searchConditionReport.SetPageNumber(arrearagesListReport.Document.Pages.Count);
                            searchConditionReport.Run(false);

                            arrearagesListReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchConditionReport.Document.Pages.Clone());
                            report = arrearagesListReport;
                        }
                    });
                    ProgressDialog.Start(ParentForm, Task.Run(() => task), false, SessionKey);

                    ShowDialogPreview(ParentForm, report, serverPath);
                }
                else
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist, "印刷");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        /// <summary>Get Server Path </summary>
        /// <returns>Server Path</returns>
        private async Task<string> GetServerPath()
        {
            string serverPath = string.Empty;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var pathService = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await pathService.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });

            return serverPath;
        }

        /// <summary>Reportのコンディション</summary>
        private void ReportSearchCondition()
        {
            SearchListCondition = new List<object>();

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                await SetReportSetting();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            SearchListCondition.Add(new SearchData("入金基準日", SearchConditon.BaseDate.ToShortDateString()));
            SearchListCondition.Add(new SearchData("得意先毎に集計", cbxCustomer.Checked ? "あり" : "なし"));
            SearchListCondition.Add(new SearchData("メモ有り", cbxMemo.Checked ? "有り" : "なし"));

            var billingMemo = SearchConditon.BillingMemo;
            SearchListCondition.Add(new SearchData("請求メモ", !string.IsNullOrWhiteSpace(billingMemo) ? billingMemo : "(指定なし)"));

            if (UseForeignCurrency)
            {
                var currencyCode = SearchConditon.CurrencyCode;
                SearchListCondition.Add(new SearchData("通貨コード", !string.IsNullOrWhiteSpace(currencyCode) ? currencyCode : "(指定なし)"));
            }

            var department = SearchConditon.DepartmentCodeFrom;
            if (!string.IsNullOrWhiteSpace(department))
                department += !string.IsNullOrWhiteSpace(lblFromDepartmentName.Text) ? " : " + lblFromDepartmentName.Text : string.Empty;
            else
                department = "(指定なし)";
            department += " ～ ";

            if (!string.IsNullOrWhiteSpace(SearchConditon.DepartmentCodeTo))
            {
                var departmentTo = SearchConditon.DepartmentCodeTo;
                department += !string.IsNullOrWhiteSpace(lblToDepartmentName.Text) ? departmentTo += " : " + lblToDepartmentName.Text : departmentTo;
            }
            else
                department += "(指定なし)";
            SearchListCondition.Add(new SearchData("請求部門コード", department));

            var staffCode = SearchConditon.StaffCodeFrom;
            if (!string.IsNullOrWhiteSpace(staffCode))
                staffCode += !string.IsNullOrWhiteSpace(lblFromStaffName.Text) ? " : " + lblFromStaffName.Text : string.Empty;
            else
                staffCode = "(指定なし)";
            staffCode += " ～ ";

            if (!string.IsNullOrWhiteSpace(SearchConditon.StaffCodeTo))
            {
                var staffTo = SearchConditon.StaffCodeTo;
                staffCode += !string.IsNullOrWhiteSpace(lblToStaffName.Text) ? staffTo += " : " + lblToStaffName.Text : staffTo;
            }
            else
                staffCode += "(指定なし)";
            SearchListCondition.Add(new SearchData("担当者コード", staffCode));

            if (cbxCustomer.Checked == false)
            {
                string outputOrder = null;
                if (ReportSettingList[5].ItemId == "ReportOutputOrder")
                {
                    if (ReportSettingList[0].ItemKey == "1")
                    {
                        outputOrder += "請求部門,";
                    }
                    if (ReportSettingList[1].ItemKey == "1")
                    {
                        outputOrder += "担当者,";
                    }
                    if (ReportSettingList[2].ItemKey == "1")
                    {
                        outputOrder += "得意先,";
                    }
                    if (ReportSettingList[3].ItemKey == "1")
                    {
                        outputOrder += "入金予定日,";
                    }
                    if (ReportSettingList[5].ItemKey == "0" && ReportSettingList[2].ItemKey == "0")
                    {
                        outputOrder += "得意先,";
                    }
                    if (ReportSettingList[5].ItemKey == "2")
                    {
                        outputOrder += "ID,";
                    }
                    if (ReportSettingList[6].ItemKey == "0")
                    {
                        outputOrder += "請求日,";
                    }
                    if (ReportSettingList[6].ItemKey == "1")
                    {
                        outputOrder += "売上日,";
                    }
                    if (ReportSettingList[6].ItemKey == "2")
                    {
                        outputOrder += "請求締日,";
                    }
                    if (ReportSettingList[6].ItemKey == "3" && ReportSettingList[3].ItemKey == "0")
                    {
                        outputOrder += "入金予定日,";
                    }
                    if (ReportSettingList[6].ItemKey == "4")
                    {
                        outputOrder += "当初予定日,";
                    }
                }
                else
                {
                    outputOrder += "(指定なし),";
                }

                if (outputOrder != null)
                {
                    outputOrder = outputOrder.Substring(0, outputOrder.Length - 1);
                }
                SearchListCondition.Add(new SearchData("出力順", outputOrder));

                var baseDate = "(指定なし)";
                if (ReportSettingList[6].ItemKey == "0" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "請求日";
                }
                else if (ReportSettingList[6].ItemKey == "1" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "売上日";
                }
                else if (ReportSettingList[6].ItemKey == "2" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "請求締日";
                }
                else if (ReportSettingList[6].ItemKey == "3" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "入金予定日";
                }
                else if (ReportSettingList[6].ItemKey == "4" && ReportSettingList[6].ItemId == "ReportBaseDateWithOriginal")
                {
                    baseDate = "当初予定日";
                }
                SearchListCondition.Add(new SearchData("日付基準", baseDate));
            }

            if (!UseForeignCurrency && ReportSettingList[4].ItemId == "ReportUnitPrice")
            {
                var unit = "";
                switch (ReportSettingList[4].ItemKey)
                {
                    case "0": unit = "円"; break;
                    case "1": unit = "千円"; break;
                    case "2": unit = "万円"; break;
                    case "3": unit = "百万円"; break;
                }
                SearchListCondition.Add(new SearchData("単位", unit));
            }
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();

                List<ArrearagesList> arrearagesList = grdArrearagesList.Rows.Select(x => x.DataBoundItem as ArrearagesList).ToList();
                arrearagesList.ForEach(x => x.BaseDate = SearchConditon.BaseDate);

                string serverPath = string.Empty;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    serverPath = await GetServerPath();
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"滞留明細一覧表{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new ArrearagesListFileDefinition(new DataExpression(ApplicationControl), ColumnSettingInfo);
                if (definition.CurrencyCodeField.Ignored = !UseForeignCurrency)
                {
                    definition.CurrencyCodeField.FieldName = null;
                }
                if (definition.IDField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.IDField.FieldName = null;
                }
                if (definition.BilledAtField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.BilledAtField.FieldName = null;
                }
                if (definition.SalesAtField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.SalesAtField.FieldName = null;
                }
                if (definition.ClosingAtField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.ClosingAtField.FieldName = null;
                }
                if (definition.DueAtField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.DueAtField.FieldName = null;
                }
                if (definition.OriginalDueAtField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.OriginalDueAtField.FieldName = null;
                }
                if (definition.ArrearagesDayCountField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.ArrearagesDayCountField.FieldName = null;
                }
                if (definition.CodeAndNameField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.CodeAndNameField.FieldName = null;
                }
                if (definition.InvoiceCodeField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.InvoiceCodeField.FieldName = null;
                }
                if (definition.Note1Field.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.Note1Field.FieldName = null;
                }
                if (definition.MemoField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.MemoField.FieldName = null;
                }
                if (definition.CustomerStaffNameField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.CustomerStaffNameField.FieldName = null;
                }
                if (definition.NoteField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.NoteField.FieldName = null;
                }
                if (definition.TelField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.TelField.FieldName = null;
                }
                if (definition.Note2Field.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.Note2Field.FieldName = null;
                }
                if (definition.Note3Field.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.Note3Field.FieldName = null;
                }
                if (definition.Note4Field.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.Note4Field.FieldName = null;
                }

                definition.AmountField.Format = value => value.ToString("0." + new string('0', Precision));

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, arrearagesList, cancel, progress);
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

        [OperationLog("設定")]
        private void PrintSetting()
        {
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PF0401);
                screen.InitializeParentForm("帳票設定");
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }

        [OperationLog("終了")]
        private void Exit()
        {
            try
            {
                Settings.SaveControlValue<PF0401>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PF0401>(Login, cbxStaff.Name, cbxStaff.Checked);
                ParentForm.Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void ReturnToSearchCondition()
        {
            tbcArrearagesList.SelectedIndex = 0;
        }
        #endregion

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var currency = await GetCurrencyInfo();

                        if (currency == null)
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                            txtCurrencyCode.Focus();
                            txtCurrencyCode.Clear();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code.ToString();
                Precision = currency.Precision;
                ClearStatusMessage();
            }
        }

        private void txtFromDepartmentCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var fromDepartmentCode = txtFromDepartmentCode.Text;
                var fromDepartmentName = string.Empty;

                if (!string.IsNullOrEmpty(fromDepartmentCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { fromDepartmentCode });
                        var departmentResult = result.Departments.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            fromDepartmentCode = departmentResult.Code;
                            fromDepartmentName = departmentResult.Name;
                            ClearStatusMessage();
                        }

                        txtFromDepartmentCode.Text = fromDepartmentCode;
                        lblFromDepartmentName.Text = fromDepartmentName;

                        if (cbxDepartment.Checked)
                        {
                            txtToDapartmentCode.Text = fromDepartmentCode;
                            lblToDepartmentName.Text = fromDepartmentName;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    if (cbxDepartment.Checked)
                    {
                        txtToDapartmentCode.Text = fromDepartmentCode;
                        lblToDepartmentName.Clear();
                    }
                    lblFromDepartmentName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnFromDepartment_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtFromDepartmentCode.Text = department.Code;
                lblFromDepartmentName.Text = department.Name;
                if (cbxDepartment.Checked)
                {
                    txtToDapartmentCode.Text = department.Code;
                    lblToDepartmentName.Text = department.Name;
                }
                ClearStatusMessage();
            }
        }     

        private void btnToDepartment_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtToDapartmentCode.Text = department.Code;
                lblToDepartmentName.Text = department.Name;
                ClearStatusMessage();
            }
        }

        private void txtToDapartmentCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var toDepartmentCode = txtToDapartmentCode.Text;
                var toDepartmentName = string.Empty;

                if (!string.IsNullOrEmpty(toDepartmentCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { toDepartmentCode });
                        var departmentResult = result.Departments.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            toDepartmentCode = departmentResult.Code;
                            toDepartmentName = departmentResult.Name;
                            ClearStatusMessage();
                        }
                        txtToDapartmentCode.Text = toDepartmentCode;
                        lblToDepartmentName.Text = toDepartmentName;
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtToDapartmentCode.Text = toDepartmentCode;
                    lblToDepartmentName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtFromStaffCode_Validated(object sender, EventArgs e)
        {
            var fromStaffCode = txtFromStaffCode.Text;
            var fromStaffName = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(fromStaffCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<StaffMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { fromStaffCode });
                        var staffResult = result.Staffs.FirstOrDefault();

                        if (staffResult != null)
                        {
                            fromStaffCode = staffResult.Code;
                            fromStaffName = staffResult.Name;
                            ClearStatusMessage();
                        }

                        txtFromStaffCode.Text = fromStaffCode;
                        lblFromStaffName.Text = fromStaffName;

                        if (cbxStaff.Checked)
                        {
                            txtToStaffCode.Text = fromStaffCode;
                            lblToStaffName.Text = fromStaffName;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    if (cbxStaff.Checked)
                    {
                        txtToStaffCode.Text = fromStaffCode;
                        lblToStaffName.Clear();
                    }
                    lblFromStaffName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtToStaffCode_Validated(object sender, EventArgs e)
        {
            var toStaffCode = txtToStaffCode.Text;
            var toStaffName = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(toStaffCode))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<StaffMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { toStaffCode });
                        var staffResult = result.Staffs.FirstOrDefault();

                        if (staffResult != null)
                        {
                            toStaffCode = staffResult.Code;
                            toStaffName = staffResult.Name;
                            ClearStatusMessage();
                        }
                        txtToStaffCode.Text = toStaffCode;
                        lblToStaffName.Text = toStaffName;
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    lblToStaffName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnFromStaff_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtFromStaffCode.Text = staff.Code;
                lblFromStaffName.Text = staff.Name;
                if (cbxStaff.Checked)
                {
                    txtToStaffCode.Text = staff.Code;
                    lblToStaffName.Text = staff.Name;
                }

                ClearStatusMessage();
            }
        }

        private void btnToStaff_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtToStaffCode.Text = staff.Code;
                lblToStaffName.Text = staff.Name;
                ClearStatusMessage();
            }
        }

        private void cbxCustomer_Click(object sender, EventArgs e)
        {
            if (cbxCustomer.Checked)
            {
                cbxMemo.Enabled = false;
                txtMemo.Enabled = false;
                txtMemo.Clear();
                cbxMemo.Checked = false;
            }
            else
            {
                cbxMemo.Enabled = true;
                txtMemo.Enabled = true;
            }
        }

        private void grdArrearagesList_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.CellName != "celMemo") return;

            var data = grdArrearagesList.Rows[e.RowIndex].DataBoundItem as ArrearagesList;
            if (data == null) return;
            using (var form = ApplicationContext.Create(nameof(PH9906)))
            {
                var screen = form.GetAll<PH9906>().First();
                screen.Id = data.Id;
                screen.MemoType = MemoType.BillingMemo;
                screen.Memo = data.Memo;
                screen.InitializeParentForm("請求メモ");

                if (ApplicationContext.ShowDialog(ParentForm, form, true) == DialogResult.OK)
                {
                    grdArrearagesList.Rows[e.RowIndex].Cells["celMemo"].Value = screen.Memo;
                }
            }
        }

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
    }
}
