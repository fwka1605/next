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
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>未消込請求データ削除</summary>
    public partial class PE0501 : VOneScreenBase
    {
        private int Precision { get; set; }
        private int UserId { get; set; }
        private int BillingCategoryId { get; set; }
        private int CollectCategoryId { get; set; }
        private int CurrencyId { get; set; }
        private BillingSearch BillingSearchCondition { get; set; }
        private List<object> BillingSearchReportList { get; set; }
        private List<Billing> Billings { get; set; }
        private List<GridSetting> GridSettingInfo { get; set; }
        private List<ColumnNameSetting> BillingColumnNameInfo { get; set; } = new List<ColumnNameSetting>();
        private string CellName(string value) => $"cel{value}";
        private bool IsGridModified { get { return grid.Rows.Any(x => (x.DataBoundItem as Billing).Checked); } }

        private List<string> LegalPersonalities { get; set; }

        public PE0501()
        {
            InitializeComponent();
            grid.SetupShortcutKeys();
            Text = "未消込請求データ削除";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcBillingOmit.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcBillingOmit.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = ExitBilling;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
            txtCustomerKana.Validated += (sender, e) => txtCustomerKana.Text = EbDataHelper.ConvertToPayerName(txtCustomerKana.Text, LegalPersonalities);

            grid.CellValueChanged                += grid_CellValueChanged;
            grid.CurrentCellDirtyStateChanged    += grid_CurrentCellDirtyStateChanged;
            grid.CellFormatting                  += grid_CellFormatting;

        }

        #region Initialize

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(true);

            OnF01ClickHandler = SearchBilling;
            OnF02ClickHandler = ClearBilling;
            OnF03ClickHandler = Delete;
            OnF04ClickHandler = PrintBilling;
            OnF06ClickHandler = ExportBilling;
            OnF08ClickHandler = CheckAllBilling;
            OnF09ClickHandler = UncheckAllBilling;
            OnF10ClickHandler = ExitBilling;
        }

        private void PE0501_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var tasks = new List<Task>();

                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadGridSetting());

                if (Authorities == null)
                    tasks.Add(LoadFunctionAuthorities(FunctionType.RecoverBilling, FunctionType.ModifyBilling));
                tasks.Add(LoadControlColorAsync());
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                tasks.Add(LoadColumnNameSettingAsync());
                tasks.Add(LoadLegalPersonalitiesAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks.ToArray()), false, SessionKey);

                SetFormLoad();

                ClearBilling();

                Settings.SetCheckBoxValue<PE0501>(Login, cbxDepartment);
                Settings.SetCheckBoxValue<PE0501>(Login, cbxCustomer);
                Settings.SetCheckBoxValue<PE0501>(Login, cbxStaff);
                Settings.SetCheckBoxValue<PE0501>(Login, cbxUsePaymentDepartment);
                Settings.SetCheckBoxValue<PE0501>(Login, cbxAmountsRange);
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
                var service = factory.Create<GridSettingMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId, Login.UserId, GridId.BillingSearch);

                if (result.ProcessResult.Result)
                {
                    GridSettingInfo = result.GridSettings;
                }
            });
        }

        private async Task<List<Billing>> GetDataAsync()
        {
            List<Billing> billingList = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                var result = await service.GetItemsAsync(SessionKey, BillingSearchCondition);

                if (result.ProcessResult.Result)
                {
                    billingList = result.Billings;
                }
            });

            return billingList ?? new List<Billing>();
        }

        private async Task LoadColumnNameSettingAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ColumnNameSettingMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    BillingColumnNameInfo = result.ColumnNames.Where(x => x.TableName == "Billing").ToList();
                }
            });
        }

        private async Task LoadLegalPersonalitiesAsync()
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

        private void CreateGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            Precision = (UseForeignCurrency) ? Precision : 0;

            var deleteFlag = (cbxDeletedAt.Checked) ? "復元" : "削除";
            var readOnly = !(Authorities[FunctionType.ModifyBilling] || Authorities[FunctionType.RecoverBilling]);
            var middleRight = MultiRowContentAlignment.MiddleRight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 40, nameof(Billing.Checked), caption: deleteFlag, dataField: nameof(Billing.Checked), cell: builder.GetCheckBoxCell(isBoolType: true), sortable: true, readOnly: readOnly),
                new CellSetting(height, cbxDeletedAt.Checked ? 100 : 0, nameof(Billing.DeleteAt), caption: "削除日", dataField: nameof(Billing.DeleteAt), cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
            });

            foreach (var gs in GridSettingInfo)
            {
                var cell = new CellSetting(height, gs.DisplayWidth, gs.ColumnName, dataField: gs.ColumnName, caption: gs.ColumnNameJp, sortable: true);
                switch (gs.ColumnName)
                {
                    case nameof(Billing.Id):
                        cell.CellInstance = builder.GetTextBoxCell(middleRight);
                        break;
                    case nameof(Billing.ResultCode):
                        cell.DataField = nameof(Billing.ResultCodeName);
                        break;
                    case nameof(Billing.InputType):
                        cell.DataField = nameof(Billing.InputTypeName);
                        break;
                    case nameof(Billing.Confirm):
                        cell.DataField = nameof(Billing.ConfirmName);
                        break;
                    case "BillingCategory":
                        cell.DataField = nameof(Billing.BillingCategoryCodeAndName);
                        break;
                    case "CollectCategory":
                        cell.DataField = nameof(Billing.CollectCategoryCodeAndName);
                        break;
                    case "AssignmentState":
                        cell.DataField = nameof(Billing.AssignmentFlagName);
                        break;
                    case nameof(Billing.CustomerCode):
                    case nameof(Billing.StaffCode):
                    case nameof(Billing.CurrencyCode):
                    case nameof(Billing.DepartmentCode):
                        cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                        break;
                    case nameof(Billing.ContractNumber):
                        cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                        break;
                    case nameof(Billing.BilledAt):
                    case nameof(Billing.SalesAt):
                    case nameof(Billing.ClosingAt):
                    case nameof(Billing.DueAt):
                    case nameof(Billing.RequestDate):
                    case nameof(Billing.FirstRecordedAt):
                    case nameof(Billing.LastRecordedAt):
                        cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                        break;
                    case nameof(Billing.BillingAmount):
                    case nameof(Billing.TaxAmount):
                    case nameof(Billing.RemainAmount):
                    case nameof(Billing.DiscountAmount1):
                    case nameof(Billing.DiscountAmount2):
                    case nameof(Billing.DiscountAmount3):
                    case nameof(Billing.DiscountAmount4):
                    case nameof(Billing.DiscountAmount5):
                        cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                        break;
                    case "Price": // Billing.BillingAmountExcludingTax ｢請求金額(税抜)｣ グリッド表示設定にはPriceで定義されている。Billingテーブルには別にPrice(金額)が定義されている。
                        cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                        cell.DataField = nameof(Billing.BillingAmountExcludingTax);
                        break;
                    case "DiscountAmountSummary":
                        cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                        cell.DataField = nameof(Billing.DiscountAmount);
                        break;
                    default: break;
                }
                builder.Items.Add(cell);
            }

            grid.FreezeLeftCellIndex = (cbxDeletedAt.Checked) ? 1 : 0;
            grid.Template = builder.Build();
            grid.AllowAutoExtend = false;
        }

        private void SetFormLoad()
        {
            var expression = new DataExpression(ApplicationControl);

            txtLoginUserCode.Format = expression.LoginUserCodeFormatString;
            txtLoginUserCode.MaxLength = expression.LoginUserCodeLength;
            txtLoginUserCode.PaddingChar = expression.LoginUserCodePaddingChar;

            txtDepartmentFrom.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentFrom.Format = expression.DepartmentCodeFormatString;
            txtDepartmentFrom.PaddingChar = expression.DepartmentCodePaddingChar;

            txtDepartmentTo.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentTo.Format = expression.DepartmentCodeFormatString;
            txtDepartmentTo.PaddingChar = expression.DepartmentCodePaddingChar;

            txtStaffFrom.MaxLength = expression.StaffCodeLength;
            txtStaffFrom.Format = expression.StaffCodeFormatString;
            txtStaffFrom.PaddingChar = expression.StaffCodePaddingChar;

            txtStaffTo.MaxLength = expression.StaffCodeLength;
            txtStaffTo.Format = expression.StaffCodeFormatString;
            txtStaffTo.PaddingChar = expression.StaffCodePaddingChar;

            txtCustomerFrom.MaxLength = expression.CustomerCodeLength;
            txtCustomerFrom.Format = expression.CustomerCodeFormatString;
            txtCustomerFrom.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerFrom.PaddingChar = expression.CustomerCodePaddingChar;

            txtCustomerTo.MaxLength = expression.CustomerCodeLength;
            txtCustomerTo.Format = expression.CustomerCodeFormatString;
            txtCustomerTo.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerTo.PaddingChar = expression.CustomerCodePaddingChar;

            if (!UseSection)
            {
                cbxUsePaymentDepartment.Visible = false;
            }

            if (!UseForeignCurrency)
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

            cmbAmountsRange.Items.Add(new ListItem("請求金額（税込）", 0));
            cmbAmountsRange.Items.Add(new ListItem("請求残", 1));
            cmbAmountsRange.SelectedIndex = 0;

            cmbInputType.Items.Add(new ListItem("すべて", 0));
            cmbInputType.Items.Add(new ListItem("取込", (int)BillingInputType.Importer));
            cmbInputType.Items.Add(new ListItem("入力", (int)BillingInputType.BillingInput));

            if (UseCashOnDueDates)
                cmbInputType.Items.Add(new ListItem("期日入金予定", (int)BillingInputType.CashOnDueDate));
            cmbInputType.Items.Add(new ListItem("定期請求", (int)BillingInputType.PeriodicBilling));

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
            CreateGridTemplate();
        }
        #endregion

        #region Function key event

        [OperationLog("検索")]
        private void SearchBilling()
        {
            if (!RequiredChecking()) return;

            if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

            try
            {
                SearchBillingData();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SearchBillingData()
        {
            ClearStatusMessage();
            BillingSearchCondition = SearchCondition();
            BillingSearchReportList = SettingSearchCondition();

            nmbBilledAmount.DisplayFields.Clear();
            nmbRemainAmount.DisplayFields.Clear();
            nmbBilledAmount.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbRemainAmount.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");

            SetBillingSearch(BillingSearchCondition);

            if (grid.RowCount > 0)
            {
                if (cbxDeletedAt.Checked)
                {
                    BaseContext.SetFunction03Enabled(Authorities[FunctionType.RecoverBilling]);
                    BaseContext.SetFunction08Enabled(Authorities[FunctionType.RecoverBilling]);
                    BaseContext.SetFunction09Enabled(Authorities[FunctionType.RecoverBilling]);
                }
                else
                {
                    BaseContext.SetFunction03Enabled(Authorities[FunctionType.ModifyBilling]);
                    BaseContext.SetFunction08Enabled(Authorities[FunctionType.ModifyBilling]);
                    BaseContext.SetFunction09Enabled(Authorities[FunctionType.ModifyBilling]);
                }
            }
            else
            {
                BaseContext.SetFunction03Enabled(false);
                BaseContext.SetFunction08Enabled(false);
                BaseContext.SetFunction09Enabled(false);
            }
        }

        private bool RequiredChecking()
        {
            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrency.Text))
            {
                txtCurrency.Select();
                ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text);
                return false;
            }

            if (!datBilledAtFrom.ValidateRange(datBilledAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblBilledAt.Text))) return false;

            if (!datSalesAtFrom.ValidateRange(datSalesAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblSalesAt.Text))) return false;

            if (!datDueAtFrom.ValidateRange(datDueAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDueAt.Text))) return false;

            if (!datDeletedAtFrom.ValidateRange(datDeletedAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDeletedAt.Text))) return false;

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

            if (cbxNoAssignment.Checked == false && cbxPartAssignment.Checked == false)
            {
                cbxPartAssignment.Focus();
                ShowWarningDialog(MsgWngSelectionRequired, lblAssignmentFlag.Text);
                return false;
            }

            return true;
        }

        private BillingSearch SearchCondition()
        {
            var billingItem = new BillingSearch();

            billingItem.LoginUserId = Login.UserId;

            billingItem.BsInputType = Convert.ToInt32(cmbInputType.SelectedItem.SubItems[1].Value);

            if (datBilledAtFrom.Value.HasValue)
            {
                billingItem.BsBilledAtFrom = Convert.ToDateTime(datBilledAtFrom.Value);
            }

            if (datBilledAtTo.Value.HasValue)
            {
                billingItem.BsBilledAtTo = Convert.ToDateTime(datBilledAtTo.Value);
            }

            if (datDueAtFrom.Value.HasValue)
            {
                billingItem.BsDueAtFrom = Convert.ToDateTime(datDueAtFrom.Value);
            }

            if (datDueAtTo.Value.HasValue)
            {
                billingItem.BsDueAtTo = Convert.ToDateTime(datDueAtTo.Value);
            }

            if (datSalesAtFrom.Value.HasValue)
            {
                billingItem.BsSalesAtFrom = Convert.ToDateTime(datSalesAtFrom.Value);
            }

            if (datSalesAtTo.Value.HasValue)
            {
                billingItem.BsSalesAtTo = Convert.ToDateTime(datSalesAtTo.Value);
            }

            if (datDeletedAtFrom.Enabled && datDeletedAtFrom.Value.HasValue)
            {
                billingItem.BsDeleteAtFrom = Convert.ToDateTime(datDeletedAtFrom.Value);
            }

            if (datDeletedAtTo.Enabled && datDeletedAtTo.Value.HasValue)
            {
                var deleteAtTo = Convert.ToDateTime(datDeletedAtTo.Value);
                billingItem.BsDeleteAtTo = deleteAtTo.Date.AddDays(1).AddMilliseconds(-1);
            }

            if (!string.IsNullOrEmpty(txtInvoiceCodeFrom.Text))
            {
                billingItem.BsInvoiceCodeFrom = txtInvoiceCodeFrom.Text;
            }

            if (!string.IsNullOrEmpty(txtInvoiceCodeTo.Text))
            {
                billingItem.BsInvoiceCodeTo = txtInvoiceCodeTo.Text;
            }

            if (!string.IsNullOrEmpty(txtInvoiceCode.Text))
            {
                billingItem.BsInvoiceCode = txtInvoiceCode.Text;
            }

            if (datUpdateAtFrom.Value.HasValue)
            {
                billingItem.BsUpdateAtFrom = Convert.ToDateTime(datUpdateAtFrom.Value);
            }

            if (datUpdateAtTo.Value.HasValue)
            {
                var updateAtTo = Convert.ToDateTime(datUpdateAtTo.Value);
                billingItem.BsUpdateAtTo = updateAtTo.Date.AddDays(1).AddMilliseconds(-1);
            }

            billingItem.LoginUserCode = txtLoginUserCode.Text;

            billingItem.UserId = UserId;

            if (cbxMemo.Checked)
            {
                billingItem.ExistsMemo = true;
            }

            if (!string.IsNullOrEmpty(txtmemo.Text))
            {
                billingItem.BsMemo = txtmemo.Text;
            }

            billingItem.BsBillingCategoryId = BillingCategoryId;

            billingItem.CollectCategoryId = CollectCategoryId;

            if (!string.IsNullOrEmpty(txtDepartmentFrom.Text))
            {
                billingItem.BsDepartmentCodeFrom = txtDepartmentFrom.Text;
            }

            if (!string.IsNullOrEmpty(txtDepartmentTo.Text))
            {
                billingItem.BsDepartmentCodeTo = txtDepartmentTo.Text;
            }

            if (!string.IsNullOrEmpty(txtStaffFrom.Text))
            {
                billingItem.BsStaffCodeFrom = txtStaffFrom.Text;
            }

            if (!string.IsNullOrEmpty(txtStaffTo.Text))
            {
                billingItem.BsStaffCodeTo = txtStaffTo.Text;
            }

            if (!string.IsNullOrEmpty(txtCustomerFrom.Text))
            {
                billingItem.BsCustomerCodeFrom = txtCustomerFrom.Text;
            }

            if (!string.IsNullOrEmpty(txtCustomerTo.Text))
            {
                billingItem.BsCustomerCodeTo = txtCustomerTo.Text;
            }

            if (!string.IsNullOrEmpty(txtCustomerName.Text))
            {
                billingItem.BsCustomerName = txtCustomerName.Text;
            }

            if (!string.IsNullOrEmpty(txtCustomerKana.Text))
            {
                billingItem.BsCustomerNameKana = txtCustomerKana.Text;
            }

            if (UseSection && cbxUsePaymentDepartment.Checked)
            {
                billingItem.UseSectionMaster = true;
            }
            else
            {
                billingItem.UseSectionMaster = false;
            }

            billingItem.CurrencyId = CurrencyId;

            var assignment
                = (cbxNoAssignment  .Checked ? (int)AssignmentFlagChecked.NoAssignment   : (int)AssignmentFlagChecked.None)
                | (cbxPartAssignment.Checked ? (int)AssignmentFlagChecked.PartAssignment : (int)AssignmentFlagChecked.None)
                | (cbxFullAssignment.Checked ? (int)AssignmentFlagChecked.FullAssignment : (int)AssignmentFlagChecked.None);
            billingItem.AssignmentFlg = assignment;

            if (cmbAmountsRange.SelectedIndex == 0)
            {
                billingItem.BsBillingAmountFrom = nmbAmountsRangeFrom.Value;
                billingItem.BsBillingAmountTo = nmbAmountsRangeTo.Value;
            }
            else
            {
                billingItem.BsRemaingAmountFrom = nmbAmountsRangeFrom.Value;
                billingItem.BsRemaingAmountTo = nmbAmountsRangeTo.Value;
            }

            billingItem.BsNote1 = txtNote1.Text;
            billingItem.BsNote2 = txtNote2.Text;
            billingItem.BsNote3 = txtNote3.Text;
            billingItem.BsNote4 = txtNote4.Text;
            billingItem.BsNote5 = txtNote5.Text;
            billingItem.BsNote6 = txtNote6.Text;
            billingItem.BsNote7 = txtNote7.Text;
            billingItem.BsNote8 = txtNote8.Text;

            if (cbxDeletedAt.Checked)
            {
                billingItem.IsDeleted = true;
            }

            billingItem.RequireRecordedAt = GridSettingInfo // ｢初回入金日｣｢最終入金日｣が検索結果に表示される場合のみ対象データを検索する。
                .Where(gs => gs.ColumnName == "FirstRecordedAt" || gs.ColumnName == "LastRecordedAt")
                .Any(gs => 0 < gs.DisplayWidth);

            return billingItem;
        }

        private void SetBillingSearch(BillingSearch billingItem)
        {
            billingItem.CompanyId = CompanyId;
            billingItem.LoginUserId = Login.UserId;
            var remainAmount = 0M;
            var totalAmount = 0M;
            BillingsResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                result = await service.GetItemsAsync(SessionKey, billingItem);
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            var resultBillings = result.Billings.Where(x => x.BillingInputPublishAt == null);
            if (resultBillings.Any())
            {
                Billings = resultBillings.ToList();
                CreateGridTemplate();
                grid.DataSource = new BindingSource(Billings, null);

                foreach (var billing in Billings)
                {
                    totalAmount += billing.BillingAmount;
                    remainAmount += billing.RemainAmount;
                }

                nmbNumber.Value = Billings.Count();
                nmbBilledAmount.Value = totalAmount;
                nmbRemainAmount.Value = remainAmount;
                BaseContext.SetFunction04Enabled(true);
                BaseContext.SetFunction06Enabled(true);
                tbcBillingOmit.SelectedIndex = 1;
                cbxDeletedAt.Enabled = false;
            }
            else
            {
                tbcBillingOmit.SelectedIndex = 0;
                ShowWarningDialog(MsgWngNotExistSearchData);
                grid.DataSource = null;
                nmbNumber.Clear();
                nmbRemainAmount.Clear();
                nmbBilledAmount.Clear();
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction06Enabled(false);
            }
        }

        //検索条件設定
        private List<Object> SettingSearchCondition()
        {
            var waveDash = " ～ ";
            var list = new List<object> {
                new SearchData(lblBilledAt.Text, datBilledAtFrom.GetPrintValue() + waveDash + datBilledAtTo.GetPrintValue()),
                new SearchData(lblSalesAt.Text, datSalesAtFrom.GetPrintValue() + waveDash + datSalesAtTo.GetPrintValue()),
                new SearchData(lblDueAt.Text, datDueAtFrom.GetPrintValue() + waveDash + datDueAtTo.GetPrintValue())
            };

            if (BillingSearchCondition.IsDeleted)
            {
                list.Add(new SearchData(lblDeletedAt.Text, datDeletedAtFrom.GetPrintValue() + waveDash + datDeletedAtTo.GetPrintValue()));
            }

            list.AddRange(new List<object> {
                new SearchData(lblInvoiceCodeFromTo.Text, txtInvoiceCodeFrom.GetPrintValue() + waveDash + txtInvoiceCodeTo.GetPrintValue()),
                new SearchData(lblInvoiceCode.Text, txtInvoiceCode.GetPrintValue()),
                new SearchData(lblUpdateAt.Text, datUpdateAtFrom.GetPrintValue() + waveDash + datUpdateAtTo.GetPrintValue()),
                new SearchData(lblLoginUName.Text, txtLoginUserCode.GetPrintValueCode(lblLoginUserName)),
                new SearchData(cbxMemo.Text, cbxMemo.Checked ? "有り" : string.Empty),
                new SearchData("メモ内容", txtmemo.GetPrintValue()),
                new SearchData(lblBillingCategory.Text, txtBillingCategory.GetPrintValueCode(lblBillingCategoryName)),
                new SearchData(lblCollectCategory.Text, txtCollectCategory.GetPrintValueCode(lblCollectCategoryName)),
                new SearchData(lblInputType.Text, cmbInputType.GetPrintValue()),
                new SearchData(lblDepartment.Text, txtDepartmentFrom.GetPrintValueCode(lblDepartmentNameFrom) + waveDash + txtDepartmentTo.GetPrintValueCode(lblDepartmentNameTo)),
                new SearchData(lblStaff.Text, txtStaffFrom.GetPrintValueCode(lblStaffNameFrom) + waveDash + txtStaffTo.GetPrintValueCode(lblStaffNameTo)),
                new SearchData(lblCustomer.Text, txtCustomerFrom.GetPrintValueCode(lblCustomerNameFrom) + waveDash + txtCustomerTo.GetPrintValueCode(lblCustomerNameTo)),
                new SearchData(lblCustomerName.Text, txtCustomerName.GetPrintValue()),
                new SearchData(lblKana.Text, txtCustomerKana.GetPrintValue())
            });

            if (UseSection)
                list.Add(new SearchData(cbxUsePaymentDepartment.Text, cbxUsePaymentDepartment.Checked ? "使用" : string.Empty));

            if (UseForeignCurrency)
                list.Add(new SearchData(lblCurrency.Text, txtCurrency.GetPrintValue()));

            var condition = new List<string>();
            foreach (CheckBox cbx in new[] { cbxPartAssignment, cbxNoAssignment })
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

        [OperationLog("クリア")]
        private void ClearBilling()
        {
            if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            txtInvoiceCodeFrom.Clear();
            txtInvoiceCodeTo.Clear();
            txtInvoiceCode.Clear();
            txtLoginUserCode.Clear();
            txtmemo.Clear();
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
            txtNote1.Clear();
            txtNote2.Clear();
            txtNote3.Clear();
            txtNote4.Clear();
            txtNote5.Clear();
            txtNote6.Clear();
            txtNote7.Clear();
            txtNote8.Clear();

            nmbAmountsRangeFrom.Clear();
            nmbAmountsRangeTo.Clear();
            nmbNumber.Clear();
            nmbRemainAmount.Clear();
            nmbBilledAmount.Clear();

            lblLoginUserName.Clear();
            lblBillingCategoryName.Clear();
            lblCollectCategoryName.Clear();
            lblDepartmentNameFrom.Clear();
            lblDepartmentNameTo.Clear();
            lblStaffNameFrom.Clear();
            lblStaffNameTo.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();

            cmbInputType.SelectedIndex = cmbInputType.Items.Count > 0 ? 0 : -1;
            cmbAmountsRange.SelectedIndex = cmbAmountsRange.Items.Count > 0 ? 0 : -1;

            cbxMemo.Checked = false;
            cbxPartAssignment.Checked = true;
            cbxNoAssignment.Checked = true;

            datBilledAtFrom.Clear();
            datBilledAtTo.Clear();
            datDueAtFrom.Clear();
            datDueAtTo.Clear();
            datSalesAtFrom.Clear();
            datSalesAtTo.Clear();
            datUpdateAtFrom.Clear();
            datUpdateAtTo.Clear();
            datDeletedAtFrom.Clear();
            datDeletedAtTo.Clear();

            cbxDeletedAt.Enabled = true;

            datDeletedAtFrom.Enabled = cbxDeletedAt.Checked;
            datDeletedAtTo.Enabled = cbxDeletedAt.Checked;

            grid.DataSource = null;

            ClearStatusMessage();

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            CollectCategoryId = 0;
            BillingCategoryId = 0;

            tbcBillingOmit.SelectedIndex = 0;
            datBilledAtFrom.Select();
        }

        [OperationLog("削除")]
        private void Delete()
        {
            DeleteOrRecoveryBilling();
        }

        [OperationLog("復元")]
        private void Recovery()
        {
            DeleteOrRecoveryBilling();
        }

        private void DeleteOrRecoveryBilling()
        {
            try
            {
                ClearStatusMessage();

                var items = grid.Rows.Select(x => x.DataBoundItem as Billing)
                    .Where(x => x.Checked)
                    .Select(x => new Transaction(x)).ToArray();

                if (!items.Any())
                {
                    ShowWarningDialog(MsgWngSelectionRequired, "請求データ");
                    return;
                }

                var msgQuestionConfirm = (cbxDeletedAt.Checked) ? MsgQstConfirmRestoreDeletedData : MsgQstConfirmDelBillReceiptOmitData;
                var errorMessage = (cbxDeletedAt.Checked) ? MsgErrRecoveryError : MsgErrDeleteError;

                if (!ShowConfirmDialog(msgQuestionConfirm, "請求データ"))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var success = true;
                var updateValid = true;
                var task = ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
                {
                    var doDelete = (cbxDeletedAt.Checked) ? 0 : 1;
                    var result = await client.OmitAsync(SessionKey, doDelete, Login.UserId, items);
                    success = result.ProcessResult.Result;
                    updateValid = (result.ProcessResult.ErrorCode != ErrorCode.OtherUserAlreadyUpdated);
                });

                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!updateValid)
                {
                    ShowWarningDialog(MsgWngAlreadyUpdated);
                    return;
                }
                if (!success)
                {
                    ShowWarningDialog(errorMessage);
                    return;
                }
                grid.DataSource = null;
                nmbNumber.Clear();
                nmbRemainAmount.Clear();
                nmbBilledAmount.Clear();
                BaseContext.SetFunction03Enabled(false);
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction08Enabled(false);
                BaseContext.SetFunction09Enabled(false);
                SearchBillingData();
                var messageId = (cbxDeletedAt.Checked) ? MsgInfFinishReturnBalanceProcess : MsgInfDeleteSuccess;
                DispStatusMessage(messageId);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("印刷")]
        private void PrintBilling()
        {
            try
            {
                var reportBillings = new List<Billing>();

                BillingServiceSearchSectionReport BillingOmitReport = null;

                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    reportBillings = await GetDataAsync();

                    if (reportBillings.Any())
                    {
                        BillingOmitReport = new BillingServiceSearchSectionReport();

                        BillingOmitReport.gfCustomerTotal.Visible = false;
                        BillingOmitReport.gfStaffTotal.Visible = false;
                        BillingOmitReport.gfDepartmentTotal.Visible = false;

                        BillingOmitReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        string printTitle = BillingSearchCondition.IsDeleted ? "請求未消込削除一覧表" : "請求未消込一覧表";
                        BillingOmitReport.Name = printTitle + DateTime.Today.ToString("yyyyMMdd");
                        BillingOmitReport.SetData(reportBillings, Precision, BillingSearchCondition.IsDeleted, lblNote1.Text);
                        BillingOmitReport.lbltitle.Text = printTitle;
                        
                        var searchReport = new SearchConditionSectionReport();

                        searchReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, printTitle);
                        searchReport.Name = printTitle + DateTime.Today.ToString("yyyyMMdd");
                        searchReport.SetPageDataSetting(BillingSearchReportList);

                        BillingOmitReport.Run(false);
                        searchReport.SetPageNumber(BillingOmitReport.Document.Pages.Count);
                        searchReport.Run(false);

                        BillingOmitReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchReport.Document.Pages.Clone());
                    }
                }), false, SessionKey);

                if (reportBillings.Any())
                {
                    var serverPath = GetServerPath();
                    ShowDialogPreview(ParentForm, BillingOmitReport, serverPath);
                }
                else
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
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
                var fileName = $"未消込請求データ削除{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var exportBilling = ((IEnumerable)grid.DataSource).Cast<Billing>().ToList();

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

                if (definition.DeleteAtField.Ignored = (!cbxDeletedAt.Checked))
                {
                    definition.DeleteAtField.FieldName = null;
                }
                definition.SetFieldsSetting(GridSettingInfo, definition.ConvertSettingToField);

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

        [OperationLog("全選択")]
        private void CheckAllBilling()
        {
            ClearStatusMessage();
            CheckAll(isChecked: true);
        }

        [OperationLog("全解除")]
        private void UncheckAllBilling()
        {
            ClearStatusMessage();
            CheckAll(isChecked: false);
        }

        private void CheckAll(bool isChecked)
        {
            grid.EndEdit();
            foreach (var billing in grid.Rows.Select(x => x.DataBoundItem as Billing))
                billing.Checked = isChecked;
            grid.Focus();
        }

        [OperationLog("終了")]
        private void ExitBilling()
        {
            try
            {
                if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

                Settings.SaveControlValue<PE0501>(Login, cbxDepartment.Name, cbxDepartment.Checked);
                Settings.SaveControlValue<PE0501>(Login, cbxCustomer.Name, cbxCustomer.Checked);
                Settings.SaveControlValue<PE0501>(Login, cbxStaff.Name, cbxStaff.Checked);
                Settings.SaveControlValue<PE0501>(Login, cbxUsePaymentDepartment.Name, cbxUsePaymentDepartment.Checked);
                Settings.SaveControlValue<PE0501>(Login, cbxAmountsRange.Name, cbxAmountsRange.Checked);
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
            tbcBillingOmit.SelectedIndex = 0;
        }
        #endregion

        #region Validated event

        private void txtDepartmentFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
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
                ClearStatusMessage();
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
                ClearStatusMessage();
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
                ClearStatusMessage();
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
                ClearStatusMessage();
                GetCustomerFromDataByCode(txtCustomerFrom.Text);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void GetCustomerFromDataByCode(string customerCodeFrom)
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
            }
        }

        private void txtCustomerTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                GetCustomerToDataByCode(txtCustomerTo.Text);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void GetCustomerToDataByCode(string customerCodeTo)
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
            }
        }

        private void txtLoginUserCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var userResult = new LoginUser();

                if (!string.IsNullOrEmpty(txtLoginUserCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<LoginUserMasterClient>();
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtLoginUserCode.Text });

                        userResult = result.Users.FirstOrDefault();

                        if (userResult != null)
                        {
                            txtLoginUserCode.Text = userResult.Code;
                            lblLoginUserName.Text = userResult.Name;
                            UserId = userResult.Id;
                            ClearStatusMessage();
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", txtLoginUserCode.Text);
                            txtLoginUserCode.Clear();
                            lblLoginUserName.Clear();
                            UserId = 0;
                        }
                    });

                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (userResult == null)
                    {
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
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Billing, new[] { txtBillingCategory.Text });

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
                            else
                            {
                                ShowWarningDialog(MsgWngMasterNotExist, "請求区分", txtBillingCategory.Text);
                                txtBillingCategory.Clear();
                                lblBillingCategoryName.Clear();
                                BillingCategoryId = 0;
                            }
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (categoryResult == null)
                    {
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
                        var result = await service.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Collect, new[] { txtCollectCategory.Text });

                        categoryResult = result.Categories.FirstOrDefault();

                        if (categoryResult != null)
                        {
                            txtCollectCategory.Text = categoryResult.Code;
                            lblCollectCategoryName.Text = categoryResult.Name;
                            CollectCategoryId = categoryResult.Id;
                            ClearStatusMessage();
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "回収区分", txtCollectCategory.Text);
                            txtCollectCategory.Clear();
                            lblCollectCategoryName.Clear();
                            CollectCategoryId = 0;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (categoryResult == null)
                    {
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

        private void txtCurrency_Validated(object sender, EventArgs e)
        {
            try
            {
                var currencyResult = new Currency();

                if (!string.IsNullOrEmpty(txtCurrency.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CurrencyMasterClient>();

                        CurrenciesResult result = null;
                        result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrency.Text });

                        currencyResult = result.Currencies.FirstOrDefault();

                        if (currencyResult != null)
                        {
                            CurrencyId = currencyResult.Id;
                            Precision = currencyResult.Precision;
                            ClearStatusMessage();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (currencyResult == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrency.Text);
                        txtCurrency.Clear();
                        CurrencyId = 0;
                        Precision = 0;
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

        private void nmbAmountsRangeFrom_Validated(object sender, EventArgs e)
        {
            if (cbxAmountsRange.Checked)
            {
                nmbAmountsRangeTo.Value = nmbAmountsRangeFrom.Value;
            }
        }
        #endregion

        #region Search dialog click event

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

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrency.Text = currency.Code;
                CurrencyId = currency.Id;
                Precision = currency.Precision;
                ClearStatusMessage();
            }
        }
        #endregion

        #region other event

        private void cbxDeletedAt_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxDeletedAt.Checked)
            {
                BaseContext.SetFunction03Caption("復元");
                datDeletedAtFrom.Enabled = true;
                datDeletedAtTo.Enabled = true;
                OnF03ClickHandler = Recovery;
            }
            else
            {
                BaseContext.SetFunction03Caption("削除");
                datDeletedAtFrom.Clear();
                datDeletedAtTo.Clear();
                datDeletedAtFrom.Enabled = false;
                datDeletedAtTo.Enabled = false;
                OnF03ClickHandler = Delete;
            }
            CreateGridTemplate();
        }

        private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            grid.CommitEdit(DataErrorContexts.Commit);

        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellIndex != 0) return;
            grid.EndEdit();

        }

        private void grid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.Scope != CellScope.Row) return;
            var requireBackColorChange = (grid.Rows[e.RowIndex].DataBoundItem as Billing)?.Checked ?? false;
            if (!requireBackColorChange) return;
            e.CellStyle.BackColor = Color.LightCyan;
        }

        private string GetServerPath()
        {
            var serverPath = "";

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return serverPath;
        }

        private string GetNumberFormat()
        {
            var displayFormatString = "0";
            var displayFieldString = "#,###,###,###,##0";
            if (Precision > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < Precision; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }
            return displayFieldString;
        }
        #endregion
    }
}
