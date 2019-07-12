using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.ReportService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Common;
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
using static Rac.VOne.Web.Models.FunctionType;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金予定明細表</summary>
    public partial class PF0301 : VOneScreenBase
    {
        private int Precision { get; set; } = 0;
        private List<ReportSetting> ReportSettingList { get; set; } = new List<ReportSetting>();
        private List<object> SearchListCondition { get; set; }
        private List<ColumnNameSetting> ColumnSettingInfo { get; set; }
        private ScheduledPaymentListSearch SearchConditon { get; set; }
        private string CellName(string value) => $"cel{value}";

        #region 画面の初期化
        public PF0301()
        {
            InitializeComponent();
            grdSchedulePaymentList.SetupShortcutKeys();
            Text = "入金予定明細表";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcPaymentList.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcPaymentList.SelectedIndex == 0)
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
        private void PF0301_Load(object sender, EventArgs e)
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
                            return LoadColumnNameSetting();
                        })
                        .Unwrap());
                }

                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                loadTask.Add(LoadFunctionAuthorities(MasterExport));

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                Clear();
                SetInitialSetting();
                InitializeScheduledPaymentListGrid();
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

        #region Intial Setting
        private void SetInitialSetting()
        {
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);

                txtFromDepartmentCode.Format = expression.DepartmentCodeFormatString;
                txtFromDepartmentCode.MaxLength = expression.DepartmentCodeLength;
                txtFromDepartmentCode.PaddingChar = expression.DepartmentCodePaddingChar;

                txtToDepartmentCode.Format = expression.DepartmentCodeFormatString;
                txtToDepartmentCode.MaxLength = expression.DepartmentCodeLength;
                txtToDepartmentCode.PaddingChar = expression.DepartmentCodePaddingChar;

                txtFromStaffCode.Format = expression.StaffCodeFormatString;
                txtFromStaffCode.MaxLength = expression.StaffCodeLength;
                txtFromStaffCode.PaddingChar = expression.StaffCodePaddingChar;

                txtToStaffCode.Format = expression.StaffCodeFormatString;
                txtToStaffCode.MaxLength = expression.StaffCodeLength;
                txtToStaffCode.PaddingChar = expression.StaffCodePaddingChar;

                txtFromCustomerCode.Format = expression.CustomerCodeFormatString;
                txtFromCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtFromCustomerCode.ImeMode = expression.CustomerCodeImeMode();
                txtFromCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;

                txtToCustomerCode.Format = expression.CustomerCodeFormatString;
                txtToCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtToCustomerCode.ImeMode = expression.CustomerCodeImeMode();
                txtToCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;

                if (UseForeignCurrency)
                {
                    lblCurrencyCode.Visible = true;
                    txtCurrencyCode.Visible = true;
                    btnCurrencySearch.Visible = true;
                }
            }

            txtCategoryCode.PaddingChar = '0';
            Settings.SetCheckBoxValue<PF0301>(Login, cbxDepartment);
            Settings.SetCheckBoxValue<PF0301>(Login, cbxStaff);
            Settings.SetCheckBoxValue<PF0301>(Login, cbxCustomer);
        }

        private async Task LoadColumnNameSetting()
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

        #region F1検索
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
                    count = await SearchScheduledPaymentList(SearchConditon);
                });
                NLogHandler.WriteDebug(this, "入金予定明細表 検索開始");
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                NLogHandler.WriteDebug(this, "入金予定明細表 検索終了");

                if (count == 0)
                    ShowWarningDialog(MsgWngNotExistSearchData);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>検索のため検索条件を設計する</summary>
        /// <returns>ScheduledPaymentListSearch</returns>
        private ScheduledPaymentListSearch GetSearchDataCondition()
        {
            var paymentListSearch = new ScheduledPaymentListSearch();

            paymentListSearch.BaseDate = datReferenceDate.Value.Value;

            if (datBilledAtFrom.Value.HasValue)
            {
                paymentListSearch.BilledAtFrom = datBilledAtFrom.Value.Value;
            }

            if (datBilledAtTo.Value.HasValue)
            {
                paymentListSearch.BilledAtTo = datBilledAtTo.Value.Value;
            }

            if (datDueAtFrom.Value.HasValue)
            {
                paymentListSearch.DueAtFrom = datDueAtFrom.Value.Value;
            }

            if (datDueAtTo.Value.HasValue)
            {
                paymentListSearch.DueAtTo = datDueAtTo.Value.Value;
            }

            if (datClosingAtFrom.Value.HasValue)
            {
                paymentListSearch.ClosingAtFrom = datClosingAtFrom.Value.Value;
            }

            if (datClosingAtTo.Value.HasValue)
            {
                paymentListSearch.ClosingAtTo = datClosingAtTo.Value.Value;
            }

            if (!string.IsNullOrWhiteSpace(txtInvoiceFrom.Text))
            {
                paymentListSearch.InvoiceCodeFrom = txtInvoiceFrom.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtInvoiceTo.Text))
            {
                paymentListSearch.InvoiceCodeTo = txtInvoiceTo.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtInvoiceCode.Text))
            {
                paymentListSearch.InvoiceCode = txtInvoiceCode.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtCategoryCode.Text))
            {
                paymentListSearch.CategoryCode = txtCategoryCode.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                paymentListSearch.CurrencyCode = txtCurrencyCode.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtFromDepartmentCode.Text))
            {
                paymentListSearch.DepartmentCodeFrom = txtFromDepartmentCode.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtToDepartmentCode.Text))
            {
                paymentListSearch.DepartmentCodeTo = txtToDepartmentCode.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtFromStaffCode.Text))
            {
                paymentListSearch.StaffCodeFrom = txtFromStaffCode.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtToStaffCode.Text))
            {
                paymentListSearch.StaffCodeTo = txtToStaffCode.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtFromCustomerCode.Text))
            {
                paymentListSearch.CustomerCodeFrom = txtFromCustomerCode.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtToCustomerCode.Text))
            {
                paymentListSearch.CustomerCodeTo = txtToCustomerCode.Text;
            }

            if (cbxCustomerAggregate.Checked)
            {
                paymentListSearch.CustomerSummaryFlag = true;
            }

            return paymentListSearch;
        }

        /// <summary>検索項目チェック</summary>
        /// <returns>検索チェック結果（bool）</returns>
        private bool ValidateSearchData()
        {
            if (!datReferenceDate.Value.HasValue)
            {
                tbcPaymentList.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "基準日");
                datReferenceDate.Focus();
                return false;
            }

            if (UseForeignCurrency && string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                tbcPaymentList.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Focus();
                return false;
            }

            if (!datBilledAtFrom.ValidateRange(datBilledAtTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, "請求日"))) return false;

            if (!datDueAtFrom.ValidateRange(datDueAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "入金予定日"))) return false;

            if (!datClosingAtFrom.ValidateRange(datClosingAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "請求締日"))) return false;

            if (!txtInvoiceFrom.ValidateRange(txtInvoiceTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "請求書番号"))) return false;

            if (!txtFromDepartmentCode.ValidateRange(txtToDepartmentCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "請求部門コード"))) return false;

            if (!txtFromStaffCode.ValidateRange(txtToStaffCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "担当者コード"))) return false;

            if (!txtFromCustomerCode.ValidateRange(txtToCustomerCode,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "得意先コード"))) return false;

            return true;
        }

        /// <summary>ReportSettingデータ取得処理</summary>
        private async Task SetReportSetting()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReportSettingMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId, nameof(PF0301));

                if (result.ProcessResult.Result)
                {
                    ReportSettingList = result.ReportSettings;
                }
            });
        }

        /// <summary>印刷のため検索条件を設計する</summary>
        private void ReportSearchCondition()
        {
            SearchListCondition = new List<object>();
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                await SetReportSetting();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            SearchListCondition.Add(new SearchData("基準日", SearchConditon.BaseDate.ToShortDateString()));

            var billedAt = SearchConditon.BilledAtFrom?.ToShortDateString() ?? "(指定なし)";
            billedAt += " ～ ";
            billedAt += SearchConditon.BilledAtTo?.ToShortDateString() ?? "(指定なし)";
            SearchListCondition.Add(new SearchData("請求日", billedAt));

            var dueAt = SearchConditon.DueAtFrom?.ToShortDateString() ?? "(指定なし)";
            dueAt += " ～ ";
            dueAt += SearchConditon.DueAtTo?.ToShortDateString() ?? "(指定なし)";
            SearchListCondition.Add(new SearchData("入金予定日", dueAt));

            var closingAt = SearchConditon.ClosingAtFrom?.ToShortDateString() ?? "(指定なし)";
            closingAt += " ～ ";
            closingAt += SearchConditon.ClosingAtTo?.ToShortDateString() ?? "(指定なし)";
            SearchListCondition.Add(new SearchData("請求締日", closingAt));

            var invoiceCode = SearchConditon.InvoiceCodeFrom ?? "(指定なし)";
            invoiceCode += " ～ ";
            invoiceCode += SearchConditon.InvoiceCodeTo ?? "(指定なし)";
            SearchListCondition.Add(new SearchData("請求書番号（範囲）", invoiceCode));

            var invoiceNo = SearchConditon.InvoiceCode;
            SearchListCondition.Add(new SearchData("請求書番号（あいまい検索）", !string.IsNullOrWhiteSpace(invoiceNo) ? invoiceNo : "(指定なし)"));

            if (!string.IsNullOrWhiteSpace(SearchConditon.CategoryCode))
            {
                var category = SearchConditon.CategoryCode;
                category += !string.IsNullOrWhiteSpace(lblCategoryName.Text) ? " : " + lblCategoryName.Text : string.Empty;
                SearchListCondition.Add(new SearchData("回収区分", category));
            }
            else
                SearchListCondition.Add(new SearchData("回収区分", "(指定なし)"));


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

            var customerCode = SearchConditon.CustomerCodeFrom;
            if (!string.IsNullOrWhiteSpace(customerCode))
                customerCode += !string.IsNullOrWhiteSpace(lblFromCustomerName.Text) ? " : " + lblFromCustomerName.Text : string.Empty;
            else
                customerCode = "(指定なし)";
            customerCode += " ～ ";

            if (!string.IsNullOrWhiteSpace(SearchConditon.CustomerCodeTo))
            {
                var customerTo = SearchConditon.CustomerCodeTo;
                customerCode += !string.IsNullOrWhiteSpace(lblToCustomerName.Text) ? customerTo += " : " + lblToCustomerName.Text : customerTo;
            }
            else
                customerCode += "(指定なし)";
            SearchListCondition.Add(new SearchData("得意先コード", customerCode));

            SearchListCondition.Add(new SearchData("得意先毎に集計", cbxCustomerAggregate.Checked ? "あり" : "なし"));

            if (cbxCustomerAggregate.Checked == false)
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
                        outputOrder += "請求ID,";
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
                if (ReportSettingList[6].ItemKey == "0" && ReportSettingList[6].ItemId == "ReportBaseDate")
                {
                    baseDate = "請求日";
                }
                else if (ReportSettingList[6].ItemKey == "1" && ReportSettingList[6].ItemId == "ReportBaseDate")
                {
                    baseDate = "売上日";
                }
                else if (ReportSettingList[6].ItemKey == "2" && ReportSettingList[6].ItemId == "ReportBaseDate")
                {
                    baseDate = "請求締日";
                }
                else if (ReportSettingList[6].ItemKey == "3" && ReportSettingList[6].ItemId == "ReportBaseDate")
                {
                    baseDate = "入金予定日";
                }
                SearchListCondition.Add(new SearchData("日付基準", baseDate));
            }

            if (!UseForeignCurrency && ReportSettingList[4].ItemId == "ReportUnitPrice")
            {
                var unit = "";
                if (ReportSettingList[4].ItemKey == "0")
                {
                    unit = "円";
                }
                else if (ReportSettingList[4].ItemKey == "1")
                {
                    unit = "千円";
                }
                else if (ReportSettingList[4].ItemKey == "2")
                {
                    unit = "万円";
                }
                else if (ReportSettingList[4].ItemKey == "3")
                {
                    unit = "百万円";
                }
                SearchListCondition.Add(new SearchData("単位", unit));
            }
        }

        /// <summary>検索結果</summary>
        /// <param name="paymentSearch">検索条件</param>
        private async Task<int> SearchScheduledPaymentList(ScheduledPaymentListSearch paymentSearch)
        {
            int count = 0;
            decimal totalAmount = 0M;
            BaseContext.SetFunction07Enabled(true);

            await ServiceProxyFactory.LifeTime(async factory =>
            {    
                var service = factory.Create<ReportServiceClient>();
                var result = await service.ScheduledPaymentListAsync(SessionKey, CompanyId, paymentSearch);
                if (result.ProcessResult.Result)
                {
                    grdSchedulePaymentList.DataSource = new BindingSource(result.ScheduledPaymentLists, null);
                    count = result.ScheduledPaymentLists.Count;
                    BaseContext.SetFunction07Enabled(true);
                   
                    if (count > 0)
                    {
                        InitializeScheduledPaymentListGrid();
                        totalAmount = grdSchedulePaymentList.Rows.Where(x => !string.IsNullOrWhiteSpace(x[CellName(nameof(ScheduledPaymentList.RemainAmount))].DisplayText.ToString()))
                                    .Sum(x => Convert.ToDecimal(x[CellName(nameof(ScheduledPaymentList.RemainAmount))].DisplayText.ToString()));
                        lblRecoveryTotalAmount.Text = totalAmount.ToString(GetNumberFormat());
                        tbcPaymentList.SelectedTab = tbpSearchResult;
                        BaseContext.SetFunction04Enabled(true);
                        BaseContext.SetFunction06Enabled(true);
                        BaseContext.SetFunction07Enabled(false);
                        tbcPaymentList.SelectedIndex = 1;                    
                    }
                    else
                    {
                        tbcPaymentList.SelectedTab = tbpSearchCondition;
                        grdSchedulePaymentList.DataSource = null;
                        lblRecoveryTotalAmount.Clear();
                        BaseContext.SetFunction04Enabled(false);
                        BaseContext.SetFunction06Enabled(false);
                        BaseContext.SetFunction07Enabled(true);
                       
                    }
                   
                }
            });

            return count;
        }

        /// <summary>StringFormatFor回収予定金額合計</summary>
        public string GetNumberFormat()
        {
            var displayFieldString = "#,###,###,###,##0";
            if (Precision > 0)
            {
                displayFieldString += "." + new string('0', Precision);
            }
            return displayFieldString;
        }
        #endregion

        #region F2クリア
        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            cbxCustomerAggregate.Checked = false;
            lblFromDepartmentName.Clear();
            txtFromDepartmentCode.Clear();
            lblToDepartmentName.Clear();
            lblToStaffName.Clear();
            lblToCustomerName.Clear();
            lblFromStaffName.Clear();
            lblFromCustomerName.Clear();
            txtToDepartmentCode.Clear();
            txtToStaffCode.Clear();
            txtToCustomerCode.Clear();
            txtFromCustomerCode.Clear();
            txtFromStaffCode.Clear();
            datBilledAtFrom.Clear();
            datBilledAtTo.Clear();
            datDueAtFrom.Clear();
            datDueAtTo.Clear();
            datClosingAtFrom.Clear();
            datClosingAtTo.Clear();
            txtCurrencyCode.Clear();
            txtCategoryCode.Clear();
            lblCategoryName.Clear();
            txtInvoiceCode.Clear();
            txtInvoiceFrom.Clear();
            txtInvoiceTo.Clear();
            lblRecoveryTotalAmount.Clear();
            grdSchedulePaymentList.DataSource = null;
            tbcPaymentList.SelectedTab = tbpSearchCondition;
            datReferenceDate.Value = DateTime.Today;
            this.ActiveControl = datReferenceDate;
            datReferenceDate.Focus();
            BaseContext.SetFunction07Enabled(true);
        }
        #endregion

        #region F4印刷
        [OperationLog("印刷")]
        private void Print()
        {
            try
            {
                ClearStatusMessage();
                ReportSearchCondition();

                GrapeCity.ActiveReports.SectionReport report = null;
                var paymentList = grdSchedulePaymentList.Rows.Select(x => x.DataBoundItem as ScheduledPaymentList).ToList();
                string serverPath = string.Empty;

                if (paymentList.Any())
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

                        for (var i = 0; i < paymentList.Count; i++)
                        {
                            decimal dec = RoundingType.Ceil.Calc(paymentList[i].RemainAmount, Precision).Value;
                            if (Precision == 0)
                            {
                                paymentList[i].TotalAmount = decimal.Truncate(dec / divVal);
                            }
                            else
                            {
                                paymentList[i].TotalAmount = dec / divVal;
                            }
                        }

                        var searchConditionReport = new SearchConditionSectionReport();
                        searchConditionReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "入金予定明細表");
                        searchConditionReport.Name = "入金予定明細表" + DateTime.Today.ToString("yyyyMMdd");
                        searchConditionReport.SetPageDataSetting(SearchListCondition);

                        if (SearchConditon.CustomerSummaryFlag)
                        {
                            var paymentListReport = new ScheduledPaymentCustomerListSectionReport();
                            paymentListReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            paymentListReport.Name = "入金予定明細表" + DateTime.Today.ToString("yyyyMMdd");
                            paymentListReport.SetData(paymentList, Precision, ReportSettingList);

                            paymentListReport.Run(false);
                            searchConditionReport.SetPageNumber(paymentListReport.Document.Pages.Count);
                            searchConditionReport.Run(false);

                            paymentListReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchConditionReport.Document.Pages.Clone());
                            report = paymentListReport;
                        }
                        else
                        {
                            var paymentListReport = new ScheduledPaymentListSectionReport();
                            paymentListReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                            paymentListReport.Name = "入金予定明細表" + DateTime.Today.ToString("yyyyMMdd");
                            paymentListReport.SetData(paymentList, Precision, ReportSettingList, ColumnSettingInfo);

                            paymentListReport.Run(false);
                            searchConditionReport.SetPageNumber(paymentListReport.Document.Pages.Count);
                            searchConditionReport.Run(false);

                            paymentListReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchConditionReport.Document.Pages.Clone());
                            report = paymentListReport;
                        }

                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

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
        #endregion

        #region F6エクスポート
        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();
                var paymentList = grdSchedulePaymentList.Rows.Select(x => x.DataBoundItem as ScheduledPaymentList).ToList();

                paymentList.ForEach(x => x.BaseDate = SearchConditon.BaseDate);

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
                var fileName = $"入金予定明細表{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new ScheduledPaymentListFileDefinition(new DataExpression(ApplicationControl), ColumnSettingInfo);
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
                if (definition.DelayDivisionField.Ignored = (SearchConditon.CustomerSummaryFlag))
                {
                    definition.DelayDivisionField.FieldName = null;
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
                    return exporter.ExportAsync(filePath, paymentList, cancel, progress);
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

        private async Task<string> GetServerPath()
        {
            string serverPath = string.Empty;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                    serverPath = result.GeneralSetting?.Value;
            });

            return serverPath;
        }
        #endregion

        #region F7設定
        [OperationLog("設定")]
        private void PrintSetting()
        {
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PF0301);
                screen.InitializeParentForm("帳票設定");
                form.StartPosition = FormStartPosition.CenterParent;
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }
        #endregion

        #region F10終了
        [OperationLog("終了")]
        private void Exit()
        {
            try
            {
                Settings.SaveControlValue<PF0301>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PF0301>(Login, cbxStaff.Name, cbxStaff.Checked);
                Settings.SaveControlValue<PF0301>(Login, cbxCustomer.Name, cbxCustomer.Checked);

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
            tbcPaymentList.SelectedIndex = 0;
        }
        #endregion

        #region グリッド作成
        private void InitializeScheduledPaymentListGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var isAggregate = cbxCustomerAggregate.Checked;
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
                    case nameof(ScheduledPaymentList.Note1): nameNote1 = setting.DisplayColumnName; break;
                    case nameof(ScheduledPaymentList.Note2): nameNote2 = setting.DisplayColumnName; break;
                    case nameof(ScheduledPaymentList.Note3): nameNote3 = setting.DisplayColumnName; break;
                    case nameof(ScheduledPaymentList.Note4): nameNote4 = setting.DisplayColumnName; break;
                }
            }

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,     widthId, nameof(ScheduledPaymentList.Id)            , dataField: nameof(ScheduledPaymentList.Id)            , caption: "請求ID"        , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight), sortable: true),
                new CellSetting(height,         115, nameof(ScheduledPaymentList.CustomerCode)  , dataField: nameof(ScheduledPaymentList.CustomerCode)  , caption: "得意先コード"  , cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height,         150, nameof(ScheduledPaymentList.CustomerName)  , dataField: nameof(ScheduledPaymentList.CustomerName)  , caption: "得意先名"      , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.BilledAt)      , dataField: nameof(ScheduledPaymentList.BilledAt)      , caption: "請求日"        , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.SalesAt)       , dataField: nameof(ScheduledPaymentList.SalesAt)       , caption: "売上日"        , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.ClosingAt)     , dataField: nameof(ScheduledPaymentList.ClosingAt)     , caption: "請求締日"      , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.DueAt)         , dataField: nameof(ScheduledPaymentList.DueAt)         , caption: "入金予定日"    , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.OriginalDueAt) , dataField: nameof(ScheduledPaymentList.OriginalDueAt) , caption: "当初予定日"    , cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
                new CellSetting(height,    widthCcy, nameof(ScheduledPaymentList.CurrencyCode)  , dataField: nameof(ScheduledPaymentList.CurrencyCode)  , caption: "通貨コード"    , cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height,         120, nameof(ScheduledPaymentList.RemainAmount)  , dataField: nameof(ScheduledPaymentList.RemainAmount)  , caption: "回収予定金額"  , cell:  builder.GetNumberCellCurrency(Precision, Precision, 0), sortable: true),
                new CellSetting(height,    widthDay, nameof(ScheduledPaymentList.DelayDivision) , dataField: nameof(ScheduledPaymentList.DelayDivision) , caption: "遅延"          , cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.CodeAndName)   , dataField: nameof(ScheduledPaymentList.CodeAndName)   , caption: "回収区分"      , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.InvoiceCode)   , dataField: nameof(ScheduledPaymentList.InvoiceCode)   , caption: "請求書番号"    , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height,         115, nameof(ScheduledPaymentList.DepartmentCode), dataField: nameof(ScheduledPaymentList.DepartmentCode), caption: "請求部門コード", cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height,         150, nameof(ScheduledPaymentList.DepartmentName), dataField: nameof(ScheduledPaymentList.DepartmentName), caption: "請求部門名"    , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height,         115, nameof(ScheduledPaymentList.StaffCode)     , dataField: nameof(ScheduledPaymentList.StaffCode)     , caption: "担当者コード"  , cell: builder.GetTextBoxCell(middleCenter), sortable: true),
                new CellSetting(height,         150, nameof(ScheduledPaymentList.StaffName)     , dataField: nameof(ScheduledPaymentList.StaffName)     , caption: "担当者名"      , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.Note1)         , dataField: nameof(ScheduledPaymentList.Note1)         , caption: nameNote1       , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.Note2)         , dataField: nameof(ScheduledPaymentList.Note2)         , caption: nameNote2       , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.Note3)         , dataField: nameof(ScheduledPaymentList.Note3)         , caption: nameNote3       , cell: builder.GetTextBoxCell(), sortable: true),
                new CellSetting(height, widthNormal, nameof(ScheduledPaymentList.Note4)         , dataField: nameof(ScheduledPaymentList.Note4)         , caption: nameNote4       , cell: builder.GetTextBoxCell(), sortable: true)
            });
            grdSchedulePaymentList.Template = builder.Build();
            grdSchedulePaymentList.HideSelection = true;
        }
        #endregion

        #region コード検索ボタン押下時
        private void btnCurrencySearch_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code.ToString();
                Precision = currency.Precision;
                ClearStatusMessage();
            }
        }

        private void DepatmentSearchClick(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                if (sender == btnFromDepSearch)
                {
                    txtFromDepartmentCode.Text = department.Code;
                    lblFromDepartmentName.Text = department.Name;
                    if (cbxDepartment.Checked)
                    {
                        txtToDepartmentCode.Text = department.Code;
                        lblToDepartmentName.Text = department.Name;
                    }
                }
                else
                {
                    txtToDepartmentCode.Text = department.Code;
                    lblToDepartmentName.Text = department.Name;
                }
                ClearStatusMessage();
            }
        }

        private void StaffSearchClick(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                if (sender == btnFromStaffSearch)
                {
                    txtFromStaffCode.Text = staff.Code;
                    lblFromStaffName.Text = staff.Name;
                    if (cbxStaff.Checked)
                    {
                        txtToStaffCode.Text = staff.Code;
                        lblToStaffName.Text = staff.Name;
                    }
                }
                else
                {
                    txtToStaffCode.Text = staff.Code;
                    lblToStaffName.Text = staff.Name;
                }
                ClearStatusMessage();
            }
        }

        private void CustomerSearchClick(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                if (sender == btnFromCusSearch)
                {
                    txtFromCustomerCode.Text = customer.Code;
                    lblFromCustomerName.Text = customer.Name;
                    if (cbxCustomer.Checked)
                    {
                        txtToCustomerCode.Text = customer.Code;
                        lblToCustomerName.Text = customer.Name;
                    }
                }
                else
                {
                    txtToCustomerCode.Text = customer.Code;
                    lblToCustomerName.Text = customer.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnCategorySearch_Click(object sender, EventArgs e)
        {
            var category = this.ShowCollectCategroySearchDialog();
            if (category != null)
            {
                txtCategoryCode.Text = category.Code;
                lblCategoryName.Text = category.Name;
                ClearStatusMessage();
            }
        }
        #endregion

        #region Validated Event
        private void txtCategoryCode_Validated(object sender, EventArgs e)
        {
            var categoryResult = new Category();

            try
            {
                if (!string.IsNullOrWhiteSpace(txtCategoryCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CategoryMasterClient>();
                        CategoriesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Collect, new string[] { txtCategoryCode.Text });

                        categoryResult = result.Categories.FirstOrDefault();

                        if (categoryResult != null)
                        {
                            txtCategoryCode.Text = categoryResult.Code;
                            lblCategoryName.Text = categoryResult.Name;
                            ClearStatusMessage();
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "回収区分", txtCategoryCode.Text);
                            txtCategoryCode.Clear();
                            lblCategoryName.Clear();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (categoryResult == null)
                    {
                        txtCategoryCode.Focus();
                    }
                }
                else
                {
                    ClearStatusMessage();
                    txtCategoryCode.Clear();
                    lblCategoryName.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtFromDepartmentCode_Validated(object sender, EventArgs e)
        {
            var departmentResult = new Department();

            try
            {
                if (!string.IsNullOrWhiteSpace(txtFromDepartmentCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtFromDepartmentCode.Text });

                        departmentResult = result.Departments.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            txtFromDepartmentCode.Text = departmentResult.Code;
                            lblFromDepartmentName.Text = departmentResult.Name;
                            if (cbxDepartment.Checked)
                            {
                                txtToDepartmentCode.Text = txtFromDepartmentCode.Text;
                                lblToDepartmentName.Text = lblFromDepartmentName.Text;
                            }
                            ClearStatusMessage();
                        }
                        else
                        {
                            lblFromDepartmentName.Clear();
                            if (cbxDepartment.Checked)
                            {
                                lblToDepartmentName.Clear();
                                txtToDepartmentCode.Text = txtFromDepartmentCode.Text;
                            }
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    if (cbxDepartment.Checked)
                    {
                        txtToDepartmentCode.Clear();
                        lblToDepartmentName.Clear();
                    }
                    txtFromDepartmentCode.Clear();
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

        private void txtToDepartmentCode_Validated(object sender, EventArgs e)
        {
            var departmentResult = new Department();

            try
            {
                if (!string.IsNullOrWhiteSpace(txtToDepartmentCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<DepartmentMasterClient>();
                        DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtToDepartmentCode.Text });

                        departmentResult = result.Departments.FirstOrDefault();

                        if (departmentResult != null)
                        {
                            txtToDepartmentCode.Text = departmentResult.Code;
                            lblToDepartmentName.Text = departmentResult.Name;
                            ClearStatusMessage();
                        }
                        else
                        {
                            lblToDepartmentName.Clear();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtToDepartmentCode.Clear();
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
            var staffResult = new Staff();

            try
            {
                if (!string.IsNullOrWhiteSpace(txtFromStaffCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<StaffMasterClient>();
                        StaffsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtFromStaffCode.Text });

                        staffResult = result.Staffs.FirstOrDefault();

                        if (staffResult != null)
                        {
                            txtFromStaffCode.Text = staffResult.Code;
                            lblFromStaffName.Text = staffResult.Name;
                            if (cbxStaff.Checked)
                            {
                                txtToStaffCode.Text = staffResult.Code;
                                lblToStaffName.Text = staffResult.Name;
                            }
                            ClearStatusMessage();
                        }
                        else
                        {
                            lblFromStaffName.Clear();
                            if (cbxStaff.Checked)
                            {
                                lblToStaffName.Clear();
                                txtToStaffCode.Text = txtFromStaffCode.Text;
                            }
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtFromStaffCode.Clear();
                    lblFromStaffName.Clear();
                    if (cbxStaff.Checked)
                    {
                        txtToStaffCode.Clear();
                        lblToStaffName.Clear();
                    }
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
            var staffResult = new Staff();

            try
            {
                if (!string.IsNullOrWhiteSpace(txtToStaffCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<StaffMasterClient>();
                        StaffsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtToStaffCode.Text });

                        staffResult = result.Staffs.FirstOrDefault();

                        if (staffResult != null)
                        {
                            txtToStaffCode.Text = staffResult.Code;
                            lblToStaffName.Text = staffResult.Name;
                            ClearStatusMessage();
                        }
                        else
                        {
                            lblToStaffName.Clear();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtToStaffCode.Clear();
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

        private void txtFromCustomerCode_Validated(object sender, EventArgs e)
        {
            var customerResult = new Customer();

            try
            {
                if (!string.IsNullOrWhiteSpace(txtFromCustomerCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CustomerMasterClient>();
                        CustomersResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtFromCustomerCode.Text });

                        customerResult = result.Customers.FirstOrDefault();

                        if (customerResult != null)
                        {
                            txtFromCustomerCode.Text = customerResult.Code;
                            lblFromCustomerName.Text = customerResult.Name;
                            if (cbxCustomer.Checked)
                            {
                                txtToCustomerCode.Text = customerResult.Code;
                                lblToCustomerName.Text = customerResult.Name;
                            }
                            ClearStatusMessage();
                        }
                        else
                        {
                            lblFromCustomerName.Clear();
                            if (cbxCustomer.Checked)
                            {
                                lblToCustomerName.Clear();
                                txtToCustomerCode.Text = txtFromCustomerCode.Text;
                            }
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtFromCustomerCode.Clear();
                    lblFromCustomerName.Clear();
                    if (cbxCustomer.Checked)
                    {
                        txtToCustomerCode.Clear();
                        lblToCustomerName.Clear();
                    }
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtToCustomerCode_Validated(object sender, EventArgs e)
        {
            var customerResult = new Customer();

            try
            {
                if (!string.IsNullOrWhiteSpace(txtToCustomerCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CustomerMasterClient>();
                        CustomersResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtToCustomerCode.Text });

                        customerResult = result.Customers.FirstOrDefault();

                        if (customerResult != null)
                        {
                            txtToCustomerCode.Text = customerResult.Code;
                            lblToCustomerName.Text = customerResult.Name;
                            ClearStatusMessage();
                        }
                        else
                        {
                            lblToCustomerName.Clear();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }
                else
                {
                    txtToCustomerCode.Clear();
                    lblToCustomerName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
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

        private async Task<Currency> GetCurrencyInfo()
        {
            var currency = new Currency();

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
    }
}