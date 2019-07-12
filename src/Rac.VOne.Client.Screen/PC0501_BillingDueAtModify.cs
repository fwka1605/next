using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using Billing = Rac.VOne.Web.Models.BillingDueAtModify;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金予定日変更</summary>
    public partial class PC0501 : VOneScreenBase
    {
        public Func<IEnumerable<ITransactionData>, bool> SavePostProcessor { get; set; }
        private int Precision { get; set; }
        private Currency Currency { get; set; }
        private Color ModifiedRowBackColor { get; } = Color.LightCyan;
        private bool IsGridModified
        {
            get
            {
                return grid.Rows.Select(x => x.DataBoundItem as Billing).Any(x =>
                IsModifiedDueAtModified(x) || IsCollectCategoryModified(x));
            }
        }

        private List<Billing> LoadedSource { get; set; }

        private Billing GetLoadedItem(Billing item)
            => LoadedSource.FirstOrDefault(x => IsSameBilling(x, item));

        private bool IsSameBilling(Billing x, Billing y)
            => y.BillingInputId.HasValue && x.BillingInputId == y.BillingInputId
           || !y.BillingInputId.HasValue && x.Id == y.Id;

        private bool IsModifiedDueAtModified(Billing item)
            => GetLoadedItem(item)?.ModifiedDueAt != item.ModifiedDueAt;

        private bool IsCollectCategoryModified(Billing item)
            => GetLoadedItem(item)?.CollectCategoryId != item.CollectCategoryId;

        #region initialize

        public PC0501()
        {
            InitializeComponent();
            grid.SetupShortcutKeys();
            Text = "入金予定日変更";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcBillingDueAt.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcBillingDueAt.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = Close; ;
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

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("更新");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Search;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = Save;
            OnF10ClickHandler = Close;
        }
        private void PC0501_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);

                SuspendLayout();
                tbcBillingDueAt.SelectedIndex = 1;
                tbcBillingDueAt.SelectedIndex = 0;
                ResumeLayout();
                ClearControl(this);
                IntitializeControlFormat();
                InitializeGridTemplate();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadApplicationControlAsync(),
                LoadCompanyAsync(),
                LoadControlColorAsync()
            };
            await Task.WhenAll(tasks);

            foreach (var task in tasks.Where(x => x.Exception != null))
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);

        }

        /// <summary>Initialize Control format, length ...</summary>
        private void IntitializeControlFormat()
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

            if (ApplicationControl.UseForeignCurrency == 0)
            {
                lblCurrencyCode.Hide();
                txtCurrencyCode.Hide();
                btnCurrencyCodeSearch.Hide();
            }

            if (ApplicationControl.UseReceiptSection == 0)
            {
                cbxUseSectionFilter.Hide();
            }

            Settings.SetCheckBoxValue<PC0501>(Login, cbxCustomerCode);
            Settings.SetCheckBoxValue<PC0501>(Login, cbxDepartmentCode);
            Settings.SetCheckBoxValue<PC0501>(Login, cbxUseSectionFilter);
            Settings.SetCheckBoxValue<PC0501>(Login, cbxStaffCode);
        }

        #endregion

        [OperationLog("検索")]
        public void Search()
        {
            try
            {
                if (!ValidateChildren()) return;

                ClearStatusMessage();
                if (!ValidateSearchData())
                    return;

                if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                    return;

                ProgressDialog.Start(ParentForm, LoadBillingsAsync(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        public void Clear()
        {
            if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            tbcBillingDueAt.SelectedIndex = 0;
            ClearControl(this);
        }

        [OperationLog("更新")]
        public void Save()
        {
            try
            {
                if (!ValidateChildren()) return;

                ClearStatusMessage();
                if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                ProgressDialog.Start(ParentForm, UpdateBillingServiceItems(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        public void Close()
        {
            try
            {
                Settings.SaveControlValue<PC0501>(Login, cbxCustomerCode.Name, cbxCustomerCode.Checked);
                Settings.SaveControlValue<PC0501>(Login, cbxDepartmentCode.Name, cbxDepartmentCode.Checked);
                Settings.SaveControlValue<PC0501>(Login, cbxUseSectionFilter.Name, cbxUseSectionFilter.Checked);
                Settings.SaveControlValue<PC0501>(Login, cbxStaffCode.Name, cbxStaffCode.Checked);

                if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClose))
                {
                    return;
                }

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
            tbcBillingDueAt.SelectedIndex = 0;
        }


        /// <summary>set Currency Info </summary>
        private async Task SetCurrencyInfo()
        {
            Currency = await GetCurrencyAsync(txtCurrencyCode.Text);
            Precision = Currency?.Precision ?? 0;
        }

        /// <summary>グリッド テンプレート初期化</summary>
        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            Precision = UseForeignCurrency ? Precision : 0;
            var wCcy = UseForeignCurrency ? 40 : 0;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,   40, "Header"                  , cell: builder.GetRowHeaderCell(), tabStop: false  ),
                new CellSetting(height,   58, "Id"                      , dataField: nameof(Billing.Id)                     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight), tabStop: false              , caption: "請求ID"      ),
                new CellSetting(height,   58, "BillingInputId"          , dataField: nameof(Billing.BillingInputId)         , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight), tabStop: false              , caption: "入力ID"      ),
                new CellSetting(height,  100, "InvoiceCode"             , dataField: nameof(Billing.InvoiceCode)            , cell: builder.GetTextBoxCell(), tabStop: false                                                  , caption: "請求書番号"  ),
                new CellSetting(height,  100, "CustomerCode"            , dataField: nameof(Billing.CustomerCode)           , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), tabStop: false             , caption: "得意先コード"),
                new CellSetting(height,  167, "CustomerName"            , dataField: nameof(Billing.CustomerName)           , cell: builder.GetTextBoxCell(), tabStop: false                                                  , caption: "得意先名"    ),
                new CellSetting(height, wCcy, "CurrencyCode"            , dataField: nameof(Billing.CurrencyCode)           , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), tabStop: false             , caption: "通貨"        ),
                new CellSetting(height,  100, "BsClosingAt"             , dataField: nameof(Billing.ClosingAt)              , cell: builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter),  tabStop: false      , caption: "請求締日"    ),
                new CellSetting(height,  100, "OriginalDueAt"           , dataField: nameof(Billing.OriginalDueAt)          , cell: builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter),  tabStop: false      , caption: "当初予定日"  ),
                new CellSetting(height,  100, "ModifiedDueAt"           , dataField: nameof(Billing.ModifiedDueAt)          , cell: builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter,true), readOnly: false , caption: "入金予定日"  ),
                new CellSetting(height,  120, "TargetAmount"            , dataField: nameof(Billing.TargetAmount)           , cell: builder.GetNumberCellCurrency(Precision, Precision, 0), tabStop: false                    , caption: "入金予定額"  ),
                new CellSetting(height,  150, "OrigiranlCollectCategory", dataField: nameof(Billing.OriginalCollectCategory), cell: builder.GetTextBoxCell(), tabStop: false                                                  , caption: "当初区分"    ),
                new CellSetting(height,   80, "CollectCategory"         , dataField: nameof(Billing.CollectCategory)        , cell: builder.GetTextBoxCell(), tabStop: false                                                  , caption: "回収区分"    ),
                new CellSetting(height,   30, "CollectCategorySearch"   , cell: builder.GetButtonCell(MultiRowContentAlignment.MiddleCenter) , value: "…"  ),
                new CellSetting(height,    0, "CollectCategoryId"       , dataField: nameof(Billing.CollectCategoryId), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)  ),
                new CellSetting(height,    0, "DueAt"                   , dataField: nameof(Billing.DueAt), cell: builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter) ),
            });

            grid.Template = builder.Build();
            grid.HideSelection = true;
            grid.AllowAutoExtend = false;

            grid.EditMode = EditMode.EditOnKeystrokeOrShortcutKey;
            grid.AllowClipboard = true;
            grid.AllowDrop = true;
            grid.MultiSelect = true;
            grid.ShortcutKeyManager.Register(EditingActions.Paste, Keys.Control | Keys.V);
            grid.ShortcutKeyManager.Register(EditingActions.Clear, Keys.Delete);
            grid.ShortcutKeyManager.Register(EditingActions.Clear, Keys.Back);
        }

        private async Task LoadBillingsAsync()
        {
            var option = GetSearchCondition();
            var billings = await GetBillingAsync(option);
            LoadedSource = billings.Select(x => x.Clone()).ToList();
            if (!billings.Any())
            {
                grid.DataSource = null;
                ShowWarningDialog(MsgWngNotExistSearchData);
                tbcBillingDueAt.SelectedIndex = 0;
                return;
            }

            if (UseForeignCurrency) await SetCurrencyInfo();

            InitializeGridTemplate();
            grid.DataSource = new BindingSource(billings, null);
            tbcBillingDueAt.SelectedIndex = 1;

            ClearStatusMessage();
            grid.Focus();
            BaseContext.SetFunction03Enabled(false);
        }

        /// <summary>Update Billing Items </summary>
        private async Task UpdateBillingServiceItems()
        {
            grid.EndEdit();

            var success = true;

            var targets = grid.Rows.Select(x => x.DataBoundItem as Billing)
                .Where(x => IsModifiedDueAtModified(x) || IsCollectCategoryModified(x))
                .Select(x => {
                    if (IsModifiedDueAtModified(x)) x.NewDueAt = x.ModifiedDueAt;
                    if (IsCollectCategoryModified(x)) x.NewCollectCategoryId = x.CollectCategoryId;
                    x.UpdateBy = Login.UserId;
                    return x;
                }).ToList();
            var result = await UpdateAsync(targets);

            if (!result.ProcessResult.Result
                || SavePostProcessor != null && result.Billings == null)
            {
                success = false;
            }

            if (!success)
            {
                ShowWarningDialog(MsgErrUpdateError);
                return;
            }

            var syncResult = true;
            if (success && SavePostProcessor != null)
            {
                syncResult = SavePostProcessor.Invoke(result.Billings.Select(x => x as ITransactionData));
            }
            success &= syncResult;

            if (!syncResult)
            {
                ShowWarningDialog(MsgErrPostProcessFailure);
                return;
            }

            await LoadBillingsAsync();
            DispStatusMessage(MsgInfUpdateSuccess);

        }

        /// <summary>Get Condition For Search Data </summary>
        /// <returns>Search Condition For Billing</returns>
        private BillingSearch GetSearchCondition()
        {
            var bls = new BillingSearch();
            bls.CompanyId = CompanyId;
            bls.LoginUserId = Login.UserId;

            if (datBilledAtFrom.Value.HasValue)
            {
                bls.BsBilledAtFrom = datBilledAtFrom.Value.Value;
            }
            if (datBilledAtTo.Value.HasValue)
            {
                bls.BsBilledAtTo = datBilledAtTo.Value.Value;
            }
            if (datClosingAtFrom.Value.HasValue)
            {
                bls.BsClosingAtFrom = datClosingAtFrom.Value.Value;
            }
            if (datClosingAtTo.Value.HasValue)
            {
                bls.BsClosingAtTo = datClosingAtTo.Value.Value;
            }
            if (datDueAtFrom.Value.HasValue)
            {
                bls.BsDueAtFrom = datDueAtFrom.Value.Value;
            }
            if (datDueAtTo.Value.HasValue)
            {
                bls.BsDueAtTo = datDueAtTo.Value.Value;
            }
            if (!string.IsNullOrWhiteSpace(txtInvoiceCode.Text))
            {
                bls.BsInvoiceCode = txtInvoiceCode.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtInvoiceCodeFrom.Text))
            {
                bls.BsInvoiceCodeFrom = txtInvoiceCodeFrom.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtInvoiceCodeTo.Text))
            {
                bls.BsInvoiceCodeTo = txtInvoiceCodeTo.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                bls.BsCurrencyCode = txtCurrencyCode.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtDepartmentCodeFrom.Text))
            {
                bls.BsDepartmentCodeFrom = txtDepartmentCodeFrom.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtDepartmentCodeTo.Text))
            {
                bls.BsDepartmentCodeTo = txtDepartmentCodeTo.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtStaffCodeFrom.Text))
            {
                bls.BsStaffCodeFrom = txtStaffCodeFrom.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtStaffCodeTo.Text))
            {
                bls.BsStaffCodeTo = txtStaffCodeTo.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text))
            {
                bls.BsCustomerCodeFrom = txtCustomerCodeFrom.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text))
            {
                bls.BsCustomerCodeTo = txtCustomerCodeTo.Text;
            }

            var assingmentFlag
                = (int)Rac.VOne.Common.Constants.AssignmentFlagChecked.NoAssignment
                | (int)Rac.VOne.Common.Constants.AssignmentFlagChecked.PartAssignment;
            bls.AssignmentFlg = assingmentFlag;

            if (ApplicationControl.UseReceiptSection == 1 && cbxUseSectionFilter.Checked)
            {
                bls.UseSectionMaster = true;
            }
            else
            {
                bls.UseSectionMaster = false;
            }
            return bls;
        }

        /// <summary>Clear Control </summary>
        private void ClearControl(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c is VOneTextControl)
                {
                    ((VOneTextControl)c).Clear();
                }
                else if (c is VOneDateControl)
                {
                    ((VOneDateControl)c).Clear();
                }
                else if (c is VOneGridControl)
                {
                    ((VOneGridControl)c).DataSource = null;
                }
                if (c.Controls.Count > 0)
                {
                    ClearControl(c);
                }
            }

            ClearStatusMessage();

            lblDepartmentNameFrom.Clear();
            lblDepartmentNameTo.Clear();
            lblStaffNameFrom.Clear();
            lblStaffNameTo.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();
            datBilledAtFrom.Focus();
            BaseContext.SetFunction03Enabled(false);
        }

        /// <summary>Validate For SearchData </summary>
        private bool ValidateSearchData()
        {
            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                tbcBillingDueAt.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Focus();
                return false;
            }

            if (!datBilledAtFrom.ValidateRange(datBilledAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "請求日")))
            {
                tbcBillingDueAt.SelectedIndex = 0;
                return false;
            }

            if (!datClosingAtFrom.ValidateRange(datClosingAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "請求締日")))
            {
                tbcBillingDueAt.SelectedIndex = 0;
                return false;
            }

            if (!datDueAtFrom.ValidateRange(datDueAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "入金予定日")))
            {
                tbcBillingDueAt.SelectedIndex = 0;
                return false;
            }

            if (!txtInvoiceCodeFrom.ValidateRange(txtInvoiceCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "請求書番号")))
            {
                tbcBillingDueAt.SelectedIndex = 0;
                return false;
            }

            if (!txtDepartmentCodeFrom.ValidateRange(txtDepartmentCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "請求部門コード")))
            {
                tbcBillingDueAt.SelectedIndex = 0;
                return false;
            }

            if (!txtStaffCodeFrom.ValidateRange(txtStaffCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "担当者コード")))
            {
                tbcBillingDueAt.SelectedIndex = 0;
                return false;
            }

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "得意先コード")))
            {
                tbcBillingDueAt.SelectedIndex = 0;
                return false;
            }

            return true;
        }


        /// <summary>Set Customer Info </summary>
        /// <param name="control">txtCustomerCodeFrom,txtCustomerCodeTo</param>
        private async Task SetCustomerInfo(VOneTextControl control)
        {
            var customerName = "";
            var isFrom = txtCustomerCodeFrom.Equals(control);
            if (!string.IsNullOrWhiteSpace(control.Text))
            {
                var customer = await GetCustomerAsync(control.Text);
                customerName = customer?.Name;
            }

            ClearStatusMessage();
            if (isFrom)
            {
                txtCustomerCodeFrom.Text = control.Text;
                lblCustomerNameFrom.Text = customerName;
                if (cbxCustomerCode.Checked)
                {
                    txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                    lblCustomerNameTo.Text = lblCustomerNameFrom.Text;
                }
            }
            else
            {
                lblCustomerNameTo.Text = customerName;
            }
        }

        /// <summary>Set Department Info </summary>
        /// <param name="control">txtDepartmentCodeFrom,txtDepartmentCodeTo</param>
        private async Task SetDepartmentInfo(VOneTextControl control)
        {
            var departmentName = "";
            var isFromControl = txtDepartmentCodeFrom.Equals(control);

            if (!string.IsNullOrWhiteSpace(control.Text))
            {
                var department = await GetDepartmentAsync(control.Text);
                departmentName = department?.Name;
            }

            ClearStatusMessage();
            if (isFromControl)
            {
                txtDepartmentCodeFrom.Text = control.Text;
                lblDepartmentNameFrom.Text = departmentName;
                if (cbxDepartmentCode.Checked)
                {
                    txtDepartmentCodeTo.Text = txtDepartmentCodeFrom.Text;
                    lblDepartmentNameTo.Text = lblDepartmentNameFrom.Text;
                }
            }
            else
            {
                lblDepartmentNameTo.Text = departmentName;
            }
        }

        /// <summary>Set Staff Info </summary>
        /// <param name="control">txtStaffCodeFrom,txtSatffCodeTo</param>
        private async Task SetStaffInfo(VOneTextControl control)
        {
            var staffName = "";
            var isFrom = txtStaffCodeFrom.Equals(control);
            if (!string.IsNullOrWhiteSpace(control.Text))
            {
                var staff = await GetStaffAsync(control.Text);
                staffName = staff?.Name;
            }

            ClearStatusMessage();
            if (isFrom)
            {
                lblStaffNameFrom.Text = staffName;
                txtStaffCodeFrom.Text = control.Text;
                if (cbxStaffCode.Checked)
                {
                    lblStaffNameTo.Text = lblStaffNameFrom.Text;
                    txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                }
            }
            else
            {
                lblStaffNameTo.Text = staffName;
            }
        }


        /// <summary>Set Grid BackGround Color and Change Edit Mode When Cell Value Change</summary>
        /// <param name="row">Row Index Of Current Cell</param>
        private void SetModifiedRow(int index)
        {
            Row row = grid.Rows[index];
            var billing = row.DataBoundItem as Billing;
            var backColor = IsGridModified ? ModifiedRowBackColor : Color.Empty;
            row.DefaultCellStyle.BackColor = backColor;
        }
        private void SetFunctionKeysEnabled()
        {
            BaseContext.SetFunction03Enabled(IsGridModified);
        }

        #region event handlers
        private void btnCustomerCodeSearch_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                ClearStatusMessage();
                txtCustomerCodeFrom.Text = customer.Code;
                lblCustomerNameFrom.Text = customer.Name;

                if (cbxCustomerCode.Checked)
                {
                    txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                    lblCustomerNameTo.Text = lblCustomerNameFrom.Text;
                }
            }
        }

        private void btnCustomerCodeToSearch_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                ClearStatusMessage();
                lblCustomerNameTo.Text = customer.Name;
                txtCustomerCodeTo.Text = customer.Code;
            }
        }

        private void btnCurrencyCodeSearch_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                ClearStatusMessage();
            }
        }

        private void btnDepartmentCodeFromSearch_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                ClearStatusMessage();
                txtDepartmentCodeFrom.Text = department.Code;
                lblDepartmentNameFrom.Text = department.Name;

                if (cbxDepartmentCode.Checked)
                {
                    txtDepartmentCodeTo.Text = txtDepartmentCodeFrom.Text;
                    lblDepartmentNameTo.Text = lblDepartmentNameFrom.Text;
                }
            }
        }

        private void btnDepartmentCodeToSearch_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                ClearStatusMessage();
                txtDepartmentCodeTo.Text = department.Code;
                lblDepartmentNameTo.Text = department.Name;
            }
        }

        private void btnLoginUserCodeFromSearch_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                ClearStatusMessage();
                txtStaffCodeFrom.Text = staff.Code;
                lblStaffNameFrom.Text = staff.Name;

                if (cbxStaffCode.Checked)
                {
                    txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                    lblStaffNameTo.Text = lblStaffNameFrom.Text;
                }
            }
        }

        private void btnLoginCodeToSearch_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                ClearStatusMessage();
                txtStaffCodeTo.Text = staff.Code;
                lblStaffNameTo.Text = staff.Name;
            }
        }

        private void grdBillingSearch_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName != "celCollectCategorySearch") return;
            var category = this.ShowCollectCategroySearchDialog();
            if (category == null) return;
            var billing = grid.Rows[e.RowIndex].DataBoundItem as Billing;
            if (billing == null) return;
            billing.CollectCategoryCode = category.Code;
            billing.CollectCategoryName = category.Name;
            billing.CollectCategoryId = category.Id;

            var row = grid.CurrentRow;
            SetModifiedRow(row.Index);
            SetFunctionKeysEnabled();
        }

        private void txtDepartmentCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                ProgressDialog.Start(ParentForm, SetDepartmentInfo((VOneTextControl)sender), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                ProgressDialog.Start(ParentForm, SetDepartmentInfo((VOneTextControl)sender), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtStaffCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                ProgressDialog.Start(ParentForm, SetStaffInfo((VOneTextControl)sender), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtStaffCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                ProgressDialog.Start(ParentForm, SetStaffInfo((VOneTextControl)sender), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                ProgressDialog.Start(ParentForm, SetCustomerInfo((VOneTextControl)sender), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                ProgressDialog.Start(ParentForm, SetCustomerInfo((VOneTextControl)sender), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    ProgressDialog.Start(ParentForm, SetCurrencyInfo(), false, SessionKey);
                    if (Currency == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                        txtCurrencyCode.Focus();
                        txtCurrencyCode.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grdBillingSearch_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.RowIndex < 0) return;

            grid.CommitEdit();

            var row = grid.CurrentRow;
            SetModifiedRow(row.Index);
            SetFunctionKeysEnabled();
        }

        #endregion

        #region call web services

        private async Task<Customer> GetCustomerAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result)
                    return result.Customers.FirstOrDefault();
                return null;
            });

        private async Task<Department> GetDepartmentAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (DepartmentMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result)
                    return result.Departments.FirstOrDefault();
                return null;
            });

        private async Task<Staff> GetStaffAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (StaffMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result)
                    return result.Staffs.FirstOrDefault();
                return null;
            });

        private async Task<Currency> GetCurrencyAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result)
                    return result.Currencies.FirstOrDefault();
                return null;
            });

        private async Task<List<Billing>> GetBillingAsync(BillingSearch option)
            => await ServiceProxyFactory.DoAsync(async (BillingServiceClient client) => {
                var result = await client.GetDueAtModifyItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Billings;
                return new List<Billing>();
            });

        private async Task<BillingsResult> UpdateAsync(List<Billing> billings)
            => await ServiceProxyFactory.DoAsync(async (BillingServiceClient client)
                => (await client.UpdateDueAtAsync(SessionKey, billings.ToArray())));

        #endregion

        private void grid_ClipboardOperating(object sender, ClipboardOperationEventArgs e)
        {
            if (e.ClipboardOperation != ClipboardOperation.Paste) return;

            if (grid.SelectedCells.Any(x => x.Name != "celModifiedDueAt"))
            {
                e.Handled = true;
                return;
            }

            var copyDate = string.Empty;
            DateTime date;
            if (Clipboard.ContainsText())
                copyDate = Clipboard.GetText();

            if (!DateTime.TryParse(copyDate, out date)) return;

            foreach (var cell in grid.SelectedCells)
            {
                cell.Value = date;
                SetModifiedRow(cell.RowIndex);
            }
            e.Handled = true;
            SetFunctionKeysEnabled();
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0 ||
                grid.SelectedCells.Any(x => x.Name != "celModifiedDueAt")) return;

            grid.CommitEdit();
            foreach(var cell in grid.SelectedCells)
                SetModifiedRow(cell.RowIndex);
            SetFunctionKeysEnabled();

        }

        private void grid_DataError(object sender, DataErrorEventArgs e)
        {
            //日付以外のフォーマットの為
        }
    }
}
