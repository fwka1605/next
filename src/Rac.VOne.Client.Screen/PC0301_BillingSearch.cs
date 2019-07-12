using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.GridSettingMasterService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.ReportSettingMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Client.Reports.Settings.PC0301;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>請求データ検索</summary>
    public partial class PC0301 : VOneScreenBase
    {
        #region メンバー＆コンストラクター
        public VOneScreenBase ReturnScreen { get; set; }
        private int Precision { get; set; }
        private int UserId { get; set; }
        private int BillingCategoryId { get; set; }
        private int CollectCategoryId { get; set; }
        private int CurrencyId { get; set; }
        private string BilledAtFrom { get; set; }
        public string CustomerCode { get; set; }
        public string CurrencyCode { get; set; }
        private bool FromReceiptInput
        {
            get { return ReturnScreen is PD0301; }
        }
        private BillingSearch BillingSearchCondition { get; set; }
        private List<Object> BillingSearchReportList { get; set; }
        private List<Billing> Billings { get; set; }
        //List<ReportSetting> ReportSettingList { get; set; } = new List<ReportSetting>();
        private List<GridSetting> GridSettingInfo { get; set; }
        public Billing CurrentBilling { get; set; }
        private List<ColumnNameSetting> BillingColumnNameInfo { get; set; } = new List<ColumnNameSetting>();
        private List<string> LegalPersonalities { get; set; }
        public PC0301()
        {
            InitializeComponent();
            grdSearchResult.SetupShortcutKeys();
            Text = "請求データ検索";
            InitializeHandlers();
        }
        private void InitializeHandlers()
        {
            tbcBillingSearch.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcBillingSearch.SelectedIndex == 1)
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
                if (ReturnScreen != null)
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = Return;
                }
                else
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = Close;
                }

            };
            txtCustomerKana.Validated += (sender, e) => txtCustomerKana.Text = EbDataHelper.ConvertToPayerName(txtCustomerKana.Text, LegalPersonalities);
        }
        #endregion

        #region 初期化処理
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SearchBilling;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearBilling;

            BaseContext.SetFunction03Caption("修正");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = CallBillingInput;

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = PrintBilling;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = ExportBilling;

            BaseContext.SetFunction07Caption("帳票設定");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = PrintSettingBilling;

            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitBilling;
        }
        private void PC0301_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var tasks = new List<Task>();
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (Authorities == null)
                    tasks.Add(LoadFunctionAuthorities(FunctionType.ModifyBilling));
                tasks.Add(LoadGridSettingAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(SetGeneralSettingAsync());
                tasks.Add(LoadColumnNameSettingAsync());
                tasks.Add(LoadLegalPersonalities());

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks.ToArray()), false, SessionKey);

                SetFormLoad();

                ClearBilling();

                Settings.SetCheckBoxValue<PC0301>(Login, cbxDepartment);
                Settings.SetCheckBoxValue<PC0301>(Login, cbxCustomer);
                Settings.SetCheckBoxValue<PC0301>(Login, cbxStaff);
                Settings.SetCheckBoxValue<PC0301>(Login, cbxUseReceiptSection);
                Settings.SetCheckBoxValue<PC0301>(Login, cbxAmountsRange);

                if (ReturnScreen != null)
                {
                    cbxNoAssignment.Checked = true;
                    cbxFullAssignment.Checked = true;
                    cbxPartAssignment.Checked = true;

                }

                if (ReturnScreen is PD0301)
                {
                    ProgressDialog.Start(ParentForm, SetCustomerCode(), false, SessionKey);
                    if (UseForeignCurrency)
                    {
                        txtCurrency.Text = CurrencyCode;
                        txtCurrency_Validated(txtCurrency, new EventArgs());
                    }
                    BaseContext.SetFunction07Enabled(false);
                    BaseContext.SetFunction10Caption("戻る");
                }

                if (ReturnScreen is PC0201)
                {
                    BaseContext.SetFunction03Caption("選択");
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction05Enabled(false);
                    BaseContext.SetFunction06Enabled(false);
                    BaseContext.SetFunction07Enabled(false);
                    BaseContext.SetFunction10Caption("戻る");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void SetFormLoad()
        {
            cmbAmountsRange.Items.Add(new ListItem("請求金額（税込）", 0));
            cmbAmountsRange.Items.Add(new ListItem("請求残", 1));
            cmbAmountsRange.SelectedIndex = 0;

            if (ApplicationControl != null)
            {
                txtLoginUserCode.MaxLength = ApplicationControl.LoginUserCodeLength;
                txtDepartmentFrom.MaxLength = ApplicationControl.DepartmentCodeLength;
                txtDepartmentTo.MaxLength = ApplicationControl.DepartmentCodeLength;
                txtCustomerFrom.MaxLength = ApplicationControl.CustomerCodeLength;
                txtCustomerTo.MaxLength = ApplicationControl.CustomerCodeLength;
                txtStaffFrom.MaxLength = ApplicationControl.StaffCodeLength;
                txtStaffTo.MaxLength = ApplicationControl.StaffCodeLength;


                var expression = new DataExpression(ApplicationControl);

                txtStaffFrom.PaddingChar = expression.StaffCodePaddingChar;
                txtStaffTo.PaddingChar = expression.StaffCodePaddingChar;
                txtDepartmentFrom.PaddingChar = expression.DepartmentCodePaddingChar;
                txtDepartmentTo.PaddingChar = expression.DepartmentCodePaddingChar;
                txtCustomerFrom.PaddingChar = expression.CustomerCodePaddingChar;
                txtCustomerTo.PaddingChar = expression.CustomerCodePaddingChar;
                txtLoginUserCode.PaddingChar = expression.LoginUserCodePaddingChar;


                txtLoginUserCode.Format = expression.LoginUserCodeFormatString;

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

                if (ApplicationControl.UseReceiptSection == 0)
                {
                    cbxUseReceiptSection.Visible = false;
                }

                if (ApplicationControl.UseForeignCurrency == 0)
                {
                    lblCurrency.Visible = false;
                    txtCurrency.Visible = false;
                    btnCurrency.Visible = false;
                }
                else
                {
                    nmbAmountsRangeFrom.Fields.DecimalPart.MaxDigits = 5;
                    nmbAmountsRangeTo.Fields.DecimalPart.MaxDigits = 5;
                }

                if (ApplicationControl.UseAccountTransfer == 0)
                {
                    cbxRequestDate.Visible = false;
                }
            }

            foreach (ColumnNameSetting cm in BillingColumnNameInfo)
            {
                Common.Controls.VOneLabelControl label = null;
                if (cm.ColumnName == "Note1") label = lblNote1;
                if (cm.ColumnName == "Note2") label = lblNote2;
                if (cm.ColumnName == "Note3") label = lblNote3;
                if (cm.ColumnName == "Note4") label = lblNote4;
                if (cm.ColumnName == "Note5") label = lblNote5;
                if (cm.ColumnName == "Note6") label = lblNote6;
                if (cm.ColumnName == "Note7") label = lblNote7;
                if (cm.ColumnName == "Note8") label = lblNote8;
                if (label == null) continue;
                label.Text = cm.DisplayColumnName;
            }

            InitializeGridTemplate();

            cmbInputType.Items.Add(new ListItem("すべて", 0));
            cmbInputType.Items.Add(new ListItem("取込", (int)BillingInputType.Importer));
            cmbInputType.Items.Add(new ListItem("入力", (int)BillingInputType.BillingInput));
            if (UseCashOnDueDates)
                cmbInputType.Items.Add(new ListItem("期日入金予定", (int)BillingInputType.CashOnDueDate));
            cmbInputType.Items.Add(new ListItem("定期請求", (int)BillingInputType.PeriodicBilling));
        }
        private async Task SetGeneralSettingAsync()
        {
            var generalSettingValue = 0;

            var generalSettingResult = new GeneralSetting();

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();

                var result = await service.GetByCodeAsync(
                        SessionKey,
                        CompanyId, "請求データ検索開始月範囲");
                if (result.ProcessResult.Result)
                {
                    generalSettingResult = result.GeneralSetting;

                    if (generalSettingResult != null)
                    {
                        generalSettingValue = Convert.ToInt32(generalSettingResult.Value);
                    }
                    else
                    {
                        generalSettingValue = 0;
                    }
                }

                var dt = DateTime.Today;
                var month = dt.AddMonths(-generalSettingValue);
                BilledAtFrom = month.ToString("yyyy/MM/dd");
                datBilledAtFrom.Text = BilledAtFrom;
            });
        }
        private async Task LoadGridSettingAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GridSettingMasterClient>();

                var result = await service.GetItemsAsync(
                        SessionKey, CompanyId, Login.UserId, GridId.BillingSearch);

                if (result.ProcessResult.Result)
                {
                    GridSettingInfo = result.GridSettings;
                }
            });
        }
        private async Task SetCustomerCode()
        {
            txtCustomerFrom.Text = CustomerCode;
            txtCustomerTo.Text = CustomerCode;

            if (!string.IsNullOrEmpty(CustomerCode))
            {
                await ServiceProxyFactory.DoAsync<CustomerMasterClient>(async client =>
                {
                    var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { CustomerCode });
                    if (result.ProcessResult.Result)
                    {
                        var customerResult = result.Customers.FirstOrDefault();
                        if (customerResult != null)
                        {
                            lblCustomerNameFrom.Text = customerResult.Name;
                            lblCustomerNameTo.Text = customerResult.Name;
                        }
                    }
                });
            }
        }
        private async Task LoadColumnNameSettingAsync()
        {
            await ServiceProxyFactory.DoAsync<ColumnNameSettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    BillingColumnNameInfo = result.ColumnNames.Where(x => x.TableName == nameof(Billing)).ToList();
            });
        }
        private async Task LoadLegalPersonalities()
        {
            await ServiceProxyFactory.DoAsync<JuridicalPersonalityMasterService.JuridicalPersonalityMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    LegalPersonalities = result.JuridicalPersonalities.Select(x => x.Kana).ToList();
                else
                    LegalPersonalities = new List<string>();
            });
        }
        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            Precision = UseForeignCurrency ? Precision : 0;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var middleRight = MultiRowContentAlignment.MiddleRight;
            foreach (var gs in GridSettingInfo)
            {
                var cell = new CellSetting(height, gs.DisplayWidth, gs.ColumnName, dataField: gs.ColumnName, caption: gs.ColumnNameJp, sortable: true);
                if (gs.ColumnName == nameof(Billing.Id))
                {
                    cell.CellInstance = builder.GetTextBoxCell(middleRight);
                }
                if (gs.ColumnName == nameof(Billing.CustomerCode)
                 || gs.ColumnName == nameof(Billing.StaffCode)
                 || gs.ColumnName == nameof(Billing.CurrencyCode)
                 || gs.ColumnName == nameof(Billing.DepartmentCode))
                {
                    cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                }
                if (gs.ColumnName == nameof(Billing.BilledAt)
                 || gs.ColumnName == nameof(Billing.SalesAt)
                 || gs.ColumnName == nameof(Billing.ClosingAt)
                 || gs.ColumnName == nameof(Billing.DueAt)
                 || gs.ColumnName == nameof(Billing.RequestDate)
                 || gs.ColumnName == nameof(Billing.FirstRecordedAt)
                 || gs.ColumnName == nameof(Billing.LastRecordedAt)
                 )
                {
                    cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                }
                if (gs.ColumnName == nameof(Billing.BillingAmount)
                 || gs.ColumnName == nameof(Billing.TaxAmount)
                 || gs.ColumnName == nameof(Billing.RemainAmount)
                 || gs.ColumnName == nameof(Billing.DiscountAmount1)
                 || gs.ColumnName == nameof(Billing.DiscountAmount2)
                 || gs.ColumnName == nameof(Billing.DiscountAmount3)
                 || gs.ColumnName == nameof(Billing.DiscountAmount4)
                 || gs.ColumnName == nameof(Billing.DiscountAmount5)
                 )
                {
                    cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                }
                if (gs.ColumnName == nameof(Billing.ResultCode))
                {
                    cell.DataField = nameof(Billing.ResultCodeName);
                }
                if (gs.ColumnName == nameof(Billing.InputType))
                {
                    cell.DataField = nameof(Billing.InputTypeName);
                }
                if (gs.ColumnName == "BillingCategory")
                {
                    cell.DataField = nameof(Billing.BillingCategoryCodeAndName);
                }
                if (gs.ColumnName == "CollectCategory")
                {
                    cell.DataField = nameof(Billing.CollectCategoryCodeAndName);
                }
                if (gs.ColumnName == "AssignmentState")
                {
                    cell.DataField = nameof(Billing.AssignmentFlagName);
                }
                if (gs.ColumnName == "Price") // Billing.BillingAmountExcludingTax ｢請求金額(税抜)｣ グリッド表示設定にはPriceで定義されている。Billingテーブルには別にPrice(金額)が定義されている。
                {
                    cell.DataField = nameof(Billing.BillingAmountExcludingTax);
                    cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                }
                if (gs.ColumnName == "DiscountAmountSummary")
                {
                    cell.DataField = nameof(Billing.DiscountAmount);
                    cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                }
                builder.Items.Add(cell);
            }
            grdSearchResult.Template = builder.Build();
            grdSearchResult.AllowAutoExtend = false;
        }

        #endregion

        #region ファンクションキー押下処理
        [OperationLog("戻る")]
        private void Return()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        [OperationLog("終了")]
        private void Close()
        {
            ExitBilling();
        }

        [OperationLog("修正")]
        private void CallBillingInput()
        {
            try
            {
                if (grdSearchResult.RowCount == 0) return;

                var billing = grdSearchResult.CurrentRow.DataBoundItem as Billing;
                var billingSearch = new BillingSearch();
                billingSearch.CompanyId = CompanyId;
                billingSearch.LoginUserId = Login.UserId;
                billingSearch.BillingId = billing.Id;
                Billing currentBilling = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<BillingServiceClient>();
                    var result = await service.GetItemsAsync(SessionKey, billingSearch);

                    if (result.ProcessResult.Result
                        && result.Billings.Any()
                        && result.Billings.Any(x => x != null))
                    {
                        currentBilling = result.Billings.FirstOrDefault();
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (currentBilling == null)
                    return;

                ClearStatusMessage();

                if (ReturnScreen == null)
                {
                    var form = ApplicationContext.Create(nameof(PC0201));
                    var screen = form.GetAll<PC0201>().FirstOrDefault();
                    screen.CurrentBilling = currentBilling;
                    screen.ReturnScreen = this;
                    form.StartPosition = FormStartPosition.CenterParent;
                    var result = ApplicationContext.ShowDialog(ParentForm, form);
                    if (result != DialogResult.OK) return;

                    ProgressDialog.Start(ParentForm, SetBillingSearch(BillingSearchCondition), false, SessionKey);
                }
                else if (ReturnScreen is PC0201)
                {
                    var screen = ReturnScreen as PC0201;
                    screen.CurrentBilling = billing;
                    ParentForm.DialogResult = DialogResult.OK;
                }
                else if (ReturnScreen is PC1601)
                {
                    ParentForm.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("検索")]
        private void SearchBilling()
        {
            try
            {
                if (!ValidateChildren()) return;

                if (!ValidateSearchOptions())
                {
                    return;
                }
                ClearStatusMessage();
                BillingSearchCondition = SearchCondition();
                BillingSearchReportList = SettingSearchCondition();

                var task = Task.CompletedTask;
                if (ApplicationControl.UseForeignCurrency == 1)
                {
                    task = SetCurrencyInfo();
                }
                var result =  ProgressDialog.Start(ParentForm,
                    task.ContinueWith(t => SetBillingSearch(BillingSearchCondition),
                            TaskScheduler.FromCurrentSynchronizationContext()).Unwrap(),
                    false,
                    SessionKey);
                if (result == DialogResult.Abort)
                {
                    ShowWarningDialog(MsgErrDataSearch);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrDataSearch);
            }
        }
        private List<object> SettingSearchCondition()
        {
            var waveDash = " ～ ";
            var list = new List<object>
            {
                new SearchData(lblBilledAt.Text, datBilledAtFrom.GetPrintValue() + waveDash + datBilledAtTo.GetPrintValue()),
                new SearchData(lblSalesAt.Text, datSalesAtFrom.GetPrintValue() + waveDash + datSalesAtTo.GetPrintValue()),
                new SearchData(lblDueAt.Text, datDueAtFrom.GetPrintValue() + waveDash + datDueAtTo.GetPrintValue()),
                new SearchData(lblInvoiceCodeFromTo.Text, txtInvoiceCodeFrom.GetPrintValue() + waveDash + txtInvoiceCodeTo.GetPrintValue()),
                new SearchData(lblInvoiceCode.Text, txtInvoiceCode.GetPrintValue()),
                new SearchData(lblUpdateAt.Text, datUpdateAtFrom.GetPrintValue() + waveDash + datUpdateAtTo.GetPrintValue()),
                new SearchData(lblLoginUName.Text, txtLoginUserCode.GetPrintValueCode(lblLoginUserName)),
                new SearchData(cbxMemo.Text, cbxMemo.Checked ? "有り" : string.Empty),
                new SearchData("メモ内容", txtMemo.GetPrintValue()),
                new SearchData(lblBillingCategory.Text, txtBillingCategory.GetPrintValueCode(lblBillingCategoryName)),
                new SearchData(lblCollectCategory.Text, txtCollectCategory.GetPrintValueCode(lblCollectCategoryName)),
                new SearchData(lblInputType.Text, cmbInputType.GetPrintValue()),
                new SearchData(lblDepartment.Text,txtDepartmentFrom.GetPrintValueCode(lblDepartmentNameFrom) + waveDash + txtDepartmentTo.GetPrintValueCode(lblDepartmentNameTo)),
                new SearchData(lblStaff.Text, txtStaffFrom.GetPrintValueCode(lblStaffNameFrom) + waveDash + txtStaffTo.GetPrintValueCode(lblStaffNameTo)),
                new SearchData(lblCustomer.Text, txtCustomerFrom.GetPrintValueCode(lblCustomerNameFrom) + waveDash + txtCustomerTo.GetPrintValueCode(lblCustomerNameTo)),
                new SearchData(lblCustomerName.Text, txtCustomerName.GetPrintValue()),
                new SearchData(lblKana.Text, txtCustomerKana.GetPrintValue())
            };

            if (ApplicationControl.UseReceiptSection != 0)
                list.Add(new SearchData(cbxUseReceiptSection.Text, cbxUseReceiptSection.Checked ? "使用" : string.Empty));

            list.Add(new SearchData(cbxRequestDate.Text, cbxRequestDate.Checked ? "表示" : string.Empty));

            if (ApplicationControl.UseForeignCurrency != 0)
                list.Add(new SearchData(lblCurrency.Text, txtCurrency.GetPrintValue()));

            var condition = new List<string>();
            foreach (CheckBox cbx in new[] { cbxFullAssignment, cbxPartAssignment, cbxNoAssignment })
            {
                if (cbx.Checked) condition.Add(cbx.Text);
            }
            list.Add(new SearchData(lblAssignmentFlag.Text, string.Join("/", condition)));

            list.Add(new SearchData(lblAmountsRange.Text,
                cmbAmountsRange.GetPrintValue() + nmbAmountsRangeFrom.GetPrintValue() + waveDash + nmbAmountsRangeTo.GetPrintValue()));
            list.Add(new SearchData(lblNote1.Text, txtNote1.GetPrintValue()));
            list.Add(new SearchData(lblNote2.Text, txtNote2.GetPrintValue()));
            list.Add(new SearchData(lblNote3.Text, txtNote3.GetPrintValue()));
            list.Add(new SearchData(lblNote4.Text, txtNote4.GetPrintValue()));
            list.Add(new SearchData(lblNote5.Text, txtNote5.GetPrintValue()));
            list.Add(new SearchData(lblNote6.Text, txtNote6.GetPrintValue()));
            list.Add(new SearchData(lblNote7.Text, txtNote7.GetPrintValue()));
            list.Add(new SearchData(lblNote8.Text, txtNote8.GetPrintValue()));


            return list;
        }
        private async Task SetCurrencyInfo()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                var result = await service.GetByCodeAsync(Login.SessionKey,
                    Login.CompanyId, new string[] { txtCurrency.Text });

                if (result.ProcessResult.Result)
                {
                    Precision = result.Currencies[0].Precision;
                }
            });
        }

        private bool ValidateSearchOptions()
        {
            if (UseForeignCurrency && string.IsNullOrWhiteSpace(txtCurrency.Text))
            {
                txtCurrency.Focus();
                ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text);
                return false;
            }

            if (!datBilledAtFrom.ValidateRange(datBilledAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblBilledAt.Text))) return false;

            if (!datSalesAtFrom.ValidateRange(datSalesAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblSalesAt.Text))) return false;

            if (!datDueAtFrom.ValidateRange(datDueAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDueAt.Text))) return false;

            if (!txtInvoiceCodeFrom.ValidateRange(txtInvoiceCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblInvoiceCodeFromTo.Text))) return false;

            if (!datUpdateAtFrom.ValidateRange(datUpdateAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblUpdateAt.Text))) return false;

            if (!txtDepartmentFrom.ValidateRange(txtDepartmentTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDepartment.Text))) return false;

            if (!txtStaffFrom.ValidateRange(txtStaffTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaff.Text))) return false;

            if (!txtCustomerFrom.ValidateRange(txtCustomerTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomer.Text))) return false;

            if (!nmbAmountsRangeFrom.ValidateRange(nmbAmountsRangeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, cmbAmountsRange.SelectedValue.ToString()))) return false;

            if (!cbxFullAssignment.Checked && !cbxNoAssignment.Checked && !cbxPartAssignment.Checked)
            {
                cbxNoAssignment.Focus();
                ShowWarningDialog(MsgWngSelectionRequired, lblAssignmentFlag.Text);
                return false;
            }

            return true;
        }

        private BillingSearch SearchCondition()
        {
            var options = new BillingSearch();

            options.LoginUserId             = Login.UserId;
            options.BsInputType             = Convert.ToInt32(cmbInputType.SelectedItem.SubItems[1].Value);
            options.BsBilledAtFrom          = datBilledAtFrom.Value;
            options.BsBilledAtTo            = datBilledAtTo.Value;
            options.BsDueAtFrom             = datDueAtFrom.Value;
            options.BsDueAtTo               = datDueAtTo.Value;
            options.BsSalesAtFrom           = datSalesAtFrom.Value;
            options.BsSalesAtTo             = datSalesAtTo.Value;
            options.BsUpdateAtFrom          = datUpdateAtFrom.Value;
            if (datUpdateAtTo.Value.HasValue)
            {
                var updateAtTo = datUpdateAtTo.Value.Value;
                options.BsUpdateAtTo = updateAtTo.Date.AddDays(1).AddMilliseconds(-1);
            }

            options.BsInvoiceCodeFrom       = txtInvoiceCodeFrom.Text;
            options.BsInvoiceCodeTo         = txtInvoiceCodeTo.Text;
            options.BsInvoiceCode           = txtInvoiceCode.Text;
            options.LoginUserCode           = txtLoginUserCode.Text;
            options.BsMemo                  = txtMemo.Text;
            options.BsDepartmentCodeFrom    = txtDepartmentFrom.Text;
            options.BsDepartmentCodeTo      = txtDepartmentTo.Text;
            options.BsStaffCodeFrom         = txtStaffFrom.Text;
            options.BsStaffCodeTo           = txtStaffTo.Text;
            options.BsCustomerCodeFrom      = txtCustomerFrom.Text;
            options.BsCustomerCodeTo        = txtCustomerTo.Text;
            options.BsCustomerName          = txtCustomerName.Text;
            options.BsCustomerNameKana      = txtCustomerKana.Text;
            options.BsNote1                 = txtNote1.Text;
            options.BsNote2                 = txtNote2.Text;
            options.BsNote3                 = txtNote3.Text;
            options.BsNote4                 = txtNote4.Text;
            options.BsNote5                 = txtNote5.Text;
            options.BsNote6                 = txtNote6.Text;
            options.BsNote7                 = txtNote7.Text;
            options.BsNote8                 = txtNote8.Text;

            options.UserId                  = UserId;
            options.ExistsMemo              = cbxMemo.Checked;
            options.BsBillingCategoryId     = BillingCategoryId;
            options.CollectCategoryId       = CollectCategoryId;


            options.UseSectionMaster = UseSection && cbxUseReceiptSection.Checked;
            options.RequestDate = cbxRequestDate.Checked ? 1 : 0;
            options.CurrencyId = CurrencyId;

            var assignmentFlag
                = (cbxNoAssignment  .Checked ? (int)AssignmentFlagChecked.NoAssignment   : (int)AssignmentFlagChecked.None)
                | (cbxPartAssignment.Checked ? (int)AssignmentFlagChecked.PartAssignment : (int)AssignmentFlagChecked.None)
                | (cbxFullAssignment.Checked ? (int)AssignmentFlagChecked.FullAssignment : (int)AssignmentFlagChecked.None);
            options.AssignmentFlg = assignmentFlag;

            if (cmbAmountsRange.SelectedIndex == 0)
            {
                options.BsBillingAmountFrom = nmbAmountsRangeFrom.Value;
                options.BsBillingAmountTo = nmbAmountsRangeTo.Value;
            }
            else
            {
                options.BsRemaingAmountFrom = nmbAmountsRangeFrom.Value;
                options.BsRemaingAmountTo = nmbAmountsRangeTo.Value;
            }

            // ｢初回入金日｣｢最終入金日｣が検索結果に表示される場合のみ対象データを検索する。
            options.RequireRecordedAt = GridSettingInfo
                .Where(gs => gs.ColumnName == nameof(Billing.FirstRecordedAt) || gs.ColumnName == nameof(Billing.LastRecordedAt))
                .Any(gs => 0 < gs.DisplayWidth);

            return options;
        }

        private async Task SetBillingSearch(BillingSearch SearchItem)
        {
            var login = ApplicationContext.Login;
            SearchItem.CompanyId = login.CompanyId;
            SearchItem.LoginUserId = login.UserId;

            var format = GetNumberFormat("#,##0", Precision);

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                var result = await service.GetItemsAsync(login.SessionKey, SearchItem);

                if (result == null
                || result.Billings == null
                || !result.ProcessResult.Result)
                {
                    
                }

                var remainAmount = 0M;
                var billingAmount = 0M;
                var count = 0;
                if (result.Billings.Any())
                {
                    Billings = result.Billings;
                    InitializeGridTemplate();
                    grdSearchResult.DataSource = new BindingSource(Billings, null);

                    foreach (var billing in Billings)
                    {
                        billingAmount += billing.BillingAmount;
                        remainAmount += billing.RemainAmount;
                    }
                    count = Billings.Count;

                    if (ReturnScreen == null)
                    {
                        BaseContext.SetFunction03Enabled(!FromReceiptInput && Authorities[FunctionType.ModifyBilling]);
                        BaseContext.SetFunction04Enabled(true);
                        BaseContext.SetFunction06Enabled(true);
                        BaseContext.SetFunction07Enabled(false);
                    }

                    if(ReturnScreen is PC0201)
                    {
                        BaseContext.SetFunction03Caption("選択");
                    }
                    tbcBillingSearch.SelectedIndex = 1;
                }
                else
                {
                    if (ReturnScreen is PC0201)
                    {
                        BaseContext.SetFunction07Enabled(false);
                    }
                    tbcBillingSearch.SelectedIndex = 0;
                    ShowWarningDialog(MsgWngNotExistSearchData);
                    grdSearchResult.DataSource = null;
                    BaseContext.SetFunction06Enabled(false);
                }
                txtBillingCount.Text = count.ToString("#,##0");
                txtBillingAmount.Text = billingAmount.ToString(format);
                txtRemainAmount.Text = remainAmount.ToString(format);
            });
            return;
        }
        private string GetNumberFormat(string displayFieldString, int displayScale, string displayFormatString = "0")
        {
            if (displayScale > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < displayScale; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }
            return displayFieldString;
        }

        [OperationLog("クリア")]
        private void ClearBilling()
        {
            txtInvoiceCodeFrom.Clear();
            txtInvoiceCodeTo.Clear();
            txtInvoiceCode.Clear();
            datUpdateAtFrom.Clear();
            datUpdateAtTo.Clear();
            txtLoginUserCode.Clear();
            txtMemo.Clear();
            txtBillingCategory.Clear();
            txtCollectCategory.Clear();
            txtDepartmentFrom.Clear();
            txtDepartmentTo.Clear();
            txtStaffFrom.Clear();
            txtStaffTo.Clear();
            txtCustomerFrom.Clear();
            txtCustomerTo.Clear();
            txtCustomerName.Clear();
            txtCustomerKana.Clear();
            txtCurrency.Clear();

            nmbAmountsRangeFrom.Clear();
            nmbAmountsRangeTo.Clear();
            txtBillingCount.Clear();
            txtRemainAmount.Clear();
            txtBillingAmount.Clear();

            txtNote1.Clear();
            txtNote2.Clear();
            txtNote3.Clear();
            txtNote4.Clear();
            txtNote5.Clear();
            txtNote6.Clear();
            txtNote7.Clear();
            txtNote8.Clear();

            lblLoginUserName.Clear();
            lblBillingCategoryName.Clear();
            lblCollectCategoryName.Clear();
            lblDepartmentNameFrom.Clear();
            lblStaffNameFrom.Clear();
            lblStaffNameTo.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();
            lblDepartmentNameTo.Clear();

            cmbInputType.SelectedIndex = cmbInputType.Items.Count > 0 ? 0 : -1;
            cmbAmountsRange.SelectedIndex = cmbAmountsRange.Items.Count > 0 ? 0 : -1;

            if (ReturnScreen != null)
            {
                cbxFullAssignment.Checked = true;
            }
            else
            {
                cbxFullAssignment.Checked = false;
            }

            cbxNoAssignment.Checked = true;
            cbxPartAssignment.Checked = true;
            cbxRequestDate.Checked = false;
            cbxMemo.Checked = false;
            ClearStatusMessage();

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);

            if(ReturnScreen == null)
            {
                BaseContext.SetFunction07Enabled(true);
            }
            else
            {
                BaseContext.SetFunction07Enabled(false);
            }

            datBilledAtFrom.Value = Convert.ToDateTime(BilledAtFrom);
            datBilledAtTo.Clear();
            datDueAtFrom.Clear();
            datDueAtTo.Clear();
            datSalesAtFrom.Clear();
            datSalesAtTo.Clear();

            CollectCategoryId = 0;
            BillingCategoryId = 0;

            tbcBillingSearch.SelectedIndex = 0;
            datBilledAtFrom.Select();
            grdSearchResult.DataSource = null;
        }

        [OperationLog("印刷")]
        private void PrintBilling()
        {
            try
            {
                var bindingSource = grdSearchResult.DataSource as BindingSource;
                var list = bindingSource?.DataSource as List<Billing>;
                if (!(list?.Any() ?? false))
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }

                var reportBillings = Model.CopyTo(list);

                if (!(reportBillings?.Any() ?? false))
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }

                var serverPath = GetServerPath();
                BillingServiceSearchSectionReport billReport = null;

                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    await ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<ReportSettingMasterClient>();

                        var result = await service.GetItemsAsync(SessionKey, CompanyId, nameof(PC0301));

                        List<ReportSetting> setting = null;
                        if (result.ProcessResult.Result)
                        {
                            setting = result.ReportSettings;
                        }

                        var departmentSubtotal = setting.GetReportSetting<ReportDoOrNot>(DepartmentSubtotal);
                        var staffSubtotal = setting.GetReportSetting<ReportDoOrNot>(StaffSubtotal);
                        var customerSubtotal = setting.GetReportSetting<ReportDoOrNot>(CustomerSubtotal);
                        var unitPrice = setting.GetReportSetting<ReportUnitPrice>(UnitPrice);
                        var outputOrder = setting.GetReportSetting<ReportOutputOrder>(OutputOrder);
                        var orderDateType = setting.GetReportSetting<ReportBaseDate>(OrderDateType);

                        var orders = reportBillings.AsQueryable().OrderBy(x => 0);
                        var items = new List<string>();


                        var title = $"請求データ一覧{DateTime.Today:yyyyMMdd}";
                        billReport = new BillingServiceSearchSectionReport();
                        billReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        billReport.Name = title;
                        billReport.lblNote1.Text = lblNote1.Text;

                        switch (unitPrice)
                        {
                            case ReportUnitPrice.Per1000: billReport.UnitPrice = 1000M; break;
                            case ReportUnitPrice.Per10000: billReport.UnitPrice = 10000M; break;
                            case ReportUnitPrice.Per1000000: billReport.UnitPrice = 1000000M; break;
                            default: billReport.UnitPrice = 1M; break;
                        }

                        if (departmentSubtotal == ReportDoOrNot.Do)
                        {
                            items.Add(nameof(Billing.DepartmentCode));
                            orders = orders.ThenBy(x => x.DepartmentCode);
                        }
                        else
                        {
                            billReport.gfDepartmentTotal.Visible = false;
                        }

                        if (staffSubtotal == ReportDoOrNot.Do)
                        {
                            items.Add(nameof(Billing.StaffCode));
                            orders = orders.ThenBy(x => x.StaffCode);
                        }
                        else
                        {
                            billReport.gfStaffTotal.Visible = false;
                        }

                        if (customerSubtotal == ReportDoOrNot.Do)
                        {
                            items.Add(nameof(Billing.CustomerCode));
                            orders = orders.ThenBy(x => x.CustomerCode);
                        }
                        else
                        {
                            billReport.gfCustomerTotal.Visible = false;
                        }

                        switch (outputOrder)
                        {
                            case ReportOutputOrder.ByCustomerCode:
                                items.Add(nameof(Billing.CustomerCode));
                                orders = orders.ThenBy(x => x.CustomerCode);
                                break;
                            case ReportOutputOrder.ByDate:
                                switch (orderDateType)
                                {
                                    case ReportBaseDate.BilledAt:
                                        items.Add(nameof(Billing.BilledAt));
                                        orders = orders.ThenBy(x => x.BilledAt);
                                        break;
                                    case ReportBaseDate.SalesAt:
                                        items.Add(nameof(Billing.SalesAt));
                                        orders = orders.ThenBy(x => x.SalesAt);
                                        break;
                                    case ReportBaseDate.ClosingAt:
                                        items.Add(nameof(Billing.ClosingAt));
                                        orders = orders.ThenBy(x => x.ClosingAt);
                                        break;
                                    case ReportBaseDate.DueAt:
                                        items.Add(nameof(Billing.DueAt));
                                        orders = orders.ThenBy(x => x.DueAt);
                                        break;
                                }
                                break;
                            case ReportOutputOrder.ById:
                                items.Add(nameof(Billing.Id));
                                orders = orders.ThenBy(x => x.Id);
                                break;
                        }

                        billReport.Precision = Precision;

                        var reportSource = orders.ToList();
                        billReport.DataSource = reportSource;


                        var searchReport = new SearchConditionSectionReport();
                        searchReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "請求データ一覧");
                        searchReport.Name = title;

                        var reportOptions = new List<object>(BillingSearchReportList);
                        if (!UseForeignCurrency)
                            reportOptions.Add(setting.GetSearchData(UnitPrice));
                        reportOptions.Add(setting.GetSearchData(OutputOrder));
                        reportOptions.Add(setting.GetSearchData(OrderDateType));

                        searchReport.SetPageDataSetting(reportOptions);

                        billReport.Run(false);
                        searchReport.SetPageNumber(billReport.Document.Pages.Count);
                        searchReport.Run(false);

                        billReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchReport.Document.Pages.Clone());
                    });

                }), false, SessionKey);

                ShowDialogPreview(ParentForm, billReport, serverPath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private TEnum GetReportSetting<TEnum>(List<ReportSetting> setting, int displayOrder)
            where TEnum : struct
        {
            TEnum result = default(TEnum);
            var value = setting.FirstOrDefault(x => x.DisplayOrder == displayOrder).ItemKey;
            Enum.TryParse(value, true, out result);
            return result;
        }

        [OperationLog("エクスポート")]
        private void ExportBilling()
        {
            try
            {
                var serverPath = GetServerPath();

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"請求データ{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var exportBilling = ((IEnumerable)grdSearchResult.DataSource).Cast<Billing>().ToList();
                var definition = new BillingSearchFileDefinition(new DataExpression(ApplicationControl), GridSettingInfo);

                var decimalFormat = "0" + ((Precision == 0) ? string.Empty : "." + new string('0', Precision));

                definition.BillingAmountField.Format = value => value.ToString(decimalFormat);
                definition.RemainAmountField.Format = value => value.ToString(decimalFormat);
                definition.DiscountAmount1Field.Format = value => value.ToString(decimalFormat);
                definition.DiscountAmount2Field.Format = value => value.ToString(decimalFormat);
                definition.DiscountAmount3Field.Format = value => value.ToString(decimalFormat);
                definition.DiscountAmount4Field.Format = value => value.ToString(decimalFormat);
                definition.DiscountAmount5Field.Format = value => value.ToString(decimalFormat);
                definition.DiscountAmountTotalField.Format = value => value.ToString(decimalFormat);
                definition.PriceField.Format = value => value.ToString(decimalFormat);
                definition.TaxAmountField.Format = value => value.ToString(decimalFormat);

                definition.SetFieldsSetting(GridSettingInfo, definition.ConvertSettingToField);
                definition.DeleteAtField.Ignored = true;

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, exportBilling, cancel, progress);
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

        [OperationLog("帳票設定")]
        private void PrintSettingBilling()
        {
            using (var form = ApplicationContext.Create(nameof(PH9905)))
            {
                var screen = form.GetAll<PH9905>().First();
                screen.FormName = nameof(PC0301);
                screen.InitializeParentForm("帳票設定");
                form.StartPosition = FormStartPosition.CenterParent;
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }
        
        private void ExitBilling()
        {
            try
            {
                Settings.SaveControlValue<PC0301>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PC0301>(Login, cbxCustomer.Name, cbxCustomer.Checked);
                Settings.SaveControlValue<PC0301>(Login, cbxStaff.Name, cbxStaff.Checked);
                Settings.SaveControlValue<PC0301>(Login, cbxUseReceiptSection.Name, cbxUseReceiptSection.Checked);
                Settings.SaveControlValue<PC0301>(Login, cbxAmountsRange.Name, cbxAmountsRange.Checked);
                BaseForm.Close();
                return;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void ReturnToSearchCondition()
        {
            tbcBillingSearch.SelectedIndex = 0;
        }
        #endregion

        #region イベントハンドラー
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

        private void txtLoginUserCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLoginUserCode.Text))
                {
                    LoginUser userResult = null;
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<LoginUserMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtLoginUserCode.Text });

                        if (result.ProcessResult.Result)
                        {
                            userResult = result.Users.FirstOrDefault();
                        }

                        if (userResult != null)
                        {
                            txtLoginUserCode.Text = userResult.Code;
                            lblLoginUserName.Text = userResult.Name;
                            UserId = userResult.Id;
                            ClearStatusMessage();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (userResult == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", txtLoginUserCode.Text);
                        txtLoginUserCode.Clear();
                        lblLoginUserName.Clear();
                        UserId = 0;
                        txtLoginUserCode.Focus();
                    }
                }
                else
                {
                    ClearStatusMessage();
                    lblLoginUserName.Clear();
                    UserId = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnLoginUser_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                txtLoginUserCode.Text = loginUser.Code;
                lblLoginUserName.Text = loginUser.Name;
                UserId = loginUser.Id;
                ClearStatusMessage();
            }
        }

        private void txtBillingCategory_Validated(object sender, EventArgs e)
        {
            try
            {
                var categoryResult = new Category();

                if (!string.IsNullOrEmpty(txtBillingCategory.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CategoryMasterClient>();

                        var result = await service.GetByCodeAsync(
                            SessionKey,
                            CompanyId, CategoryType.Billing,
                            new[] { txtBillingCategory.Text });
                        if (result.ProcessResult.Result)
                        {
                            categoryResult = result.Categories.FirstOrDefault();
                            if (categoryResult != null)
                            {
                                txtBillingCategory.Text = categoryResult.Code;
                                lblBillingCategoryName.Text = categoryResult.Name;
                                BillingCategoryId = categoryResult.Id;
                                ClearStatusMessage();
                            }
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (categoryResult == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "請求区分", txtBillingCategory.Text);
                        txtBillingCategory.Clear();
                        lblBillingCategoryName.Clear();
                        BillingCategoryId = 0;
                        txtBillingCategory.Focus();
                    }
                }
                else
                {
                    ClearStatusMessage();
                    lblBillingCategoryName.Clear();
                    BillingCategoryId = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnBillingCategory_Click(object sender, EventArgs e)
        {
            var billingCategory = this.ShowBillingCategorySearchDialog();
            if (billingCategory != null)
            {
                txtBillingCategory.Text = billingCategory.Code;
                lblBillingCategoryName.Text = billingCategory.Name;
                BillingCategoryId = billingCategory.Id;
                ClearStatusMessage();
            }
        }

        private void txtCollectCategory_Validated(object sender, EventArgs e)
        {
            try
            {
                var categoryResult = new Category();

                if (!string.IsNullOrEmpty(txtCollectCategory.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CategoryMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Collect,
                            new[] { txtCollectCategory.Text });

                        categoryResult = result.Categories.FirstOrDefault();
                        if (categoryResult != null)
                        {
                            txtCollectCategory.Text = categoryResult.Code;
                            lblCollectCategoryName.Text = categoryResult.Name;
                            CollectCategoryId = categoryResult.Id;
                            ClearStatusMessage();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (categoryResult == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "回収区分", txtCollectCategory.Text);
                        txtCollectCategory.Clear();
                        lblCollectCategoryName.Clear();
                        CollectCategoryId = 0;
                        txtCollectCategory.Focus();
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
                txtCollectCategory.Text = collectCategory.Code;
                lblCollectCategoryName.Text = collectCategory.Name;
                CollectCategoryId = collectCategory.Id;
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

        private void nmbAmountsRangeFrom_Validated(object sender, EventArgs e)
        {
            if (cbxAmountsRange.Checked)
            {
                nmbAmountsRangeTo.Value = nmbAmountsRangeFrom.Value;
            }
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrency.Text = currency.Code;
                CurrencyId = currency.Id;
                ClearStatusMessage();
            }
        }

        private void txtCurrency_Validated(object sender, EventArgs e)
        {
            try
            {
                var currencyResult = new Currency();

                if (!string.IsNullOrEmpty(txtCurrency.Text))
                {
                    CurrenciesResult result = null;
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CurrencyMasterClient>();
                        result = await service.GetByCodeAsync(
                              SessionKey,
                              CompanyId,
                              new string[] { txtCurrency.Text });

                        currencyResult = result.Currencies.FirstOrDefault();
                        if (currencyResult != null)
                        {
                            CurrencyId = currencyResult.Id;
                            ClearStatusMessage();
                            return;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (currencyResult == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrency.Text);
                        CurrencyId = 0;
                        txtCurrency.Clear();
                        txtCurrency.Focus();
                    }
                }
                else
                {
                    ClearStatusMessage();
                    txtCurrency.Clear();
                    CurrencyId = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grdSearchResult_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (ReturnScreen is PD0301) return;

            var billingId = 0L;
            var billingAmount = 0M;
            var Amount = new decimal[5];

            if (e.RowIndex >= 0)
            {
                var columnName = e.CellName;

                if (UseDiscount && columnName == "celDiscountAmount1" || columnName == "celDiscountAmount2" ||
                    columnName == "celDiscountAmount3" || columnName == "celDiscountAmount4" ||
                    columnName == "celDiscountAmount5" || columnName == "celDiscountAmount")
                {
                    billingId = long.Parse(grdSearchResult.Rows[e.RowIndex].Cells["celId"].DisplayText);
                    billingAmount = Convert.ToDecimal(grdSearchResult.Rows[e.RowIndex].Cells["celBillingAmount"].DisplayText);

                    using (var form = ApplicationContext.Create(nameof(PC0302)))
                    {
                        var screen = form.GetAll<PC0302>().First();
                        screen.BillingId = billingId;
                        screen.BillingAmount = billingAmount;
                        screen.Precision = Precision;

                        screen.InitializeParentForm("歩引額調整");

                        var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                        if (dialogResult == DialogResult.OK)
                        {
                            grdSearchResult.Rows[e.RowIndex].Cells["celDiscountAmount1"].Value = screen.Discount1;
                            grdSearchResult.Rows[e.RowIndex].Cells["celDiscountAmount2"].Value = screen.Discount2;
                            grdSearchResult.Rows[e.RowIndex].Cells["celDiscountAmount3"].Value = screen.Discount3;
                            grdSearchResult.Rows[e.RowIndex].Cells["celDiscountAmount4"].Value = screen.Discount4;
                            grdSearchResult.Rows[e.RowIndex].Cells["celDiscountAmount5"].Value = screen.Discount5;
                            grdSearchResult.Rows[e.RowIndex].Cells["celDiscountAmount"].Value = screen.TotalDiscount;
                        }
                    }
                }
                else
                {
                    if (columnName == "celMemo")
                    {
                        billingId = long.Parse(grdSearchResult.Rows[e.RowIndex].Cells["celId"].DisplayText);

                        using (var form = ApplicationContext.Create(nameof(PH9906)))
                        {
                            var screen = form.GetAll<PH9906>().First();
                            screen.Id = billingId;
                            screen.MemoType = MemoType.BillingMemo;
                            screen.Memo = Convert.ToString(grdSearchResult.GetValue(e.RowIndex, "celMemo"));
                            screen.InitializeParentForm("請求メモ");
                            if (ApplicationContext.ShowDialog(ParentForm, form, true) == DialogResult.OK)
                            {
                                grdSearchResult.Rows[e.RowIndex].Cells["celMemo"].Value = screen.Memo;
                            }
                        }
                    }
                    else
                    {
                        if (Authorities[FunctionType.ModifyBilling])
                        {
                            CallBillingInput();
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngPermissionDeniedForEdit);
                        }
                    }
                }
            }
        }

        #endregion

        #region Webサービス呼び出し
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
        private async Task<List<Billing>> GetDataAsync()
        {
            List<Billing> billingList = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                var result = await service.GetItemsAsync(
                        SessionKey, BillingSearchCondition);

                if (result.ProcessResult.Result)
                {
                    billingList = result.Billings;
                }
            });

            return billingList ?? new List<Billing>();
        }
        #endregion
    }
}
