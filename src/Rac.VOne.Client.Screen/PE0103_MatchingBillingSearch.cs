using GrapeCity.Win.Editors;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>個別消込・請求検索</summary>
    public partial class PE0103 : VOneScreenBase
    {
        public List<long> BillingId { get; set; } = new List<long>();
        public Web.Models.Billing[] BillingInfo { get; set; }
        //public bool UsingSectionMaster { get; set; }
        public int? ParentCustomerId { get; set; } = 0;
        public string CurrencyCode { get; set; } = "";
        //public int[] SectionId { get; set; } = null;
        public DateTime? DueAtFrom { get; set; }
        public DateTime? DueAtTo { get; set; }
        internal byte[] ClientKey { private get; set; }
        internal List<Department> Departments { private get; set; }
        internal List<int> DepartmentIds { private get; set; }
        private List<int> DepartmentIdsInner { get; set; }
        private List<Category> CategoryList { get; set; } = null;

        private List<string> LegalPersonarities { get; set; }

        private int? UpdateBy { get; set; }

        public PE0103()
        {
            InitializeComponent();
            InitializeHandlers();
            Text = "個別消込・請求検索";
        }

        private void InitializeHandlers()
        {
            txtCusNameKana.Validated += (sender, e) => txtCusNameKana.Text = EbDataHelper.ConvertToPayerName(txtCusNameKana.Text, LegalPersonarities);
            txtPayerName.Validated += (sender, e) => txtPayerName.Text = EbDataHelper.ConvertToPayerName(txtPayerName.Text, LegalPersonarities);
        }

        private void PE0103_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                datBilledAtFrom.Focus();
                datBilledAtFrom.Clear();
                datBilledAtTo.Clear();
                datDueAtFrom.Value = DueAtFrom;
                datDueAtTo.Value = DueAtTo;
                datSalesAtFrom.Clear();
                datSalesAtTo.Clear();
                datUpdateAtFrom.Clear();
                datUpdateAtTo.Clear();
                cmbAmountType.SelectedIndex = 0;
                datBilledAtFrom.Focus();
                var loadTask = new List<Task>();
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(LoadLeagalPersonarities());
                loadTask.Add(LoadColumnNameSettingAsync());
                SuspendLayout();
                ResumeLayout();
                InitializeDepartmentSelection();
                loadTask.Add(LoadBillingCategoryCombo());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SetComboForInputType();
                if (ApplicationControl != null)
                {
                    var expression = new DataExpression(ApplicationControl);
                    txtCusCodeFrom.Format = expression.CustomerCodeFormatString;
                    txtCusCodeFrom.MaxLength = expression.CustomerCodeLength;
                    txtCusCodeFrom.ImeMode = expression.CustomerCodeImeMode();
                    txtCusCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;
                    txtCusCodeTo.Format = expression.CustomerCodeFormatString;
                    txtCusCodeTo.MaxLength = expression.CustomerCodeLength;
                    txtCusCodeTo.ImeMode = expression.CustomerCodeImeMode();
                    txtCusCodeTo.PaddingChar = expression.CustomerCodePaddingChar;
                    txtDepCodeFrom.Format = expression.DepartmentCodeFormatString;
                    txtDepCodeFrom.MaxLength = expression.DepartmentCodeLength;
                    txtDepCodeFrom.PaddingChar = expression.DepartmentCodePaddingChar;
                    txtDepCodeTo.Format = expression.DepartmentCodeFormatString;
                    txtDepCodeTo.MaxLength = expression.DepartmentCodeLength;
                    txtDepCodeTo.PaddingChar = expression.DepartmentCodePaddingChar;
                    txtStaffCodeFrom.Format = expression.StaffCodeFormatString;
                    txtStaffCodeFrom.MaxLength = expression.StaffCodeLength;
                    txtStaffCodeFrom.PaddingChar = expression.StaffCodePaddingChar;
                    txtStaffCodeTo.Format = expression.StaffCodeFormatString;
                    txtStaffCodeTo.MaxLength = expression.StaffCodeLength;
                    txtStaffCodeTo.PaddingChar = expression.StaffCodePaddingChar;
                    txtUpdateBy.Format = expression.LoginUserCodeFormatString;
                    txtUpdateBy.MaxLength = expression.LoginUserCodeLength;
                    txtUpdateBy.PaddingChar = expression.LoginUserCodePaddingChar;

                }
                var format = UseForeignCurrency
                    ? "###,###,###,##0.00000"
                    : "###,###,###,##0";
                nmbAmountFrom.DisplayFields.AddRange(format, "", "", "-", "");
                nmbAmountTo.DisplayFields.AddRange(format, "", "", "-", "");

                Settings.SetCheckBoxValue<PE0103>(Login, cbxCusCode);
                Settings.SetCheckBoxValue<PE0103>(Login, cbxDeCode);
                Settings.SetCheckBoxValue<PE0103>(Login, cbxInvoiceCode);
                Settings.SetCheckBoxValue<PE0103>(Login, cbxStaffCode);
                Settings.SetCheckBoxValue<PE0103>(Login, cbxAmountType);
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
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction10Caption("戻る");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = BillingSearch;
            OnF02ClickHandler = Clear;
            OnF10ClickHandler = Close;
        }

        private void InitializeDepartmentSelection()
        {
            if (Departments.Count == DepartmentIds.Count)
            {
                lblDepartmentName.Text = "すべて";
            }
            else if (DepartmentIds.Count == 1)
            {
                lblDepartmentName.Text = Departments.First(x => DepartmentIds.Contains(x.Id)).Name;
            }
            else
            {
                lblDepartmentName.Text = "請求部門絞込有";
            }
            DepartmentIdsInner = DepartmentIds;
        }

        #region functionKey Event

        [OperationLog("検索")]
        public void BillingSearch()
        {
            if (!ValidateChildren()) return;

            if (!CheckInput()) return;

            var billingSearch = GetSearchCondition();

            BillingsResult resultBilling = null;
            try
            {
                var task = LoadBillingAsync(billingSearch);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                resultBilling = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (!resultBilling.ProcessResult.Result)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }

            var searchResult = resultBilling.Billings.Where(x => !BillingId.Contains(x.Id)).ToArray();
            if (searchResult.Length == 0)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }
            else
            {
                BillingInfo = searchResult;
                ParentForm.DialogResult = DialogResult.OK;
            }
        }

        private async Task<BillingsResult> LoadBillingAsync(BillingSearch searchOption)
        {
            var useDepartmentFilter = Departments.Count != DepartmentIdsInner.Count;
            if (useDepartmentFilter)
                await Util.SaveWorkDepartmentTargetAsync(Login, ClientKey, DepartmentIdsInner.ToArray());
            return await ServiceProxyFactory.DoAsync(async (BillingServiceClient client)
                =>await client.GetItemsAsync(SessionKey, searchOption));
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            ClearControls();
            datDueAtFrom.Value = DueAtFrom;
            datDueAtTo.Value = DueAtTo;
            datBilledAtFrom.Focus();
        }

        [OperationLog("戻る")]
        public void Close()
        {
            Settings.SaveControlValue<PE0103>(ApplicationContext.Login, cbxCusCode.Name, cbxCusCode.Checked);
            Settings.SaveControlValue<PE0103>(ApplicationContext.Login, cbxDeCode.Name, cbxDeCode.Checked);
            Settings.SaveControlValue<PE0103>(ApplicationContext.Login, cbxInvoiceCode.Name, cbxInvoiceCode.Checked);
            Settings.SaveControlValue<PE0103>(ApplicationContext.Login, cbxStaffCode.Name, cbxStaffCode.Checked);
            Settings.SaveControlValue<PE0103>(ApplicationContext.Login, cbxAmountType.Name, cbxAmountType.Checked);
            BaseForm.Close();
        }

        #endregion

        #region commonFunction

        /// <summary>画面クリア</summary>
        private void ClearControls()
        {
            var controls = this.GetAll<Control>();
            foreach (var c in controls)
            {
                if (c is VOneTextControl)
                {
                    if (lblDepartmentName.Equals(c)) continue;
                    ((VOneTextControl)c).Clear();
                }
                else if (c is VOneDateControl)
                {
                    ((VOneDateControl)c).Clear();
                }
                else if (c is VOneNumberControl)
                {
                    ((VOneNumberControl)c).Clear();
                }
                else if (c is VOneComboControl)
                {
                    if (cmbSeikyuKubun.Equals(c) && CategoryList != null)
                    {
                        ((VOneComboControl)c).SelectedIndex = 0;
                    }
                    if (cmbInputType.Equals(c))
                    {
                        ((VOneComboControl)c).SelectedIndex = 0;
                    }
                    if (cmbAmountType.Equals(c))
                    {
                        ((VOneComboControl)c).SelectedIndex = ((VOneComboControl)c).Items.Count > 0 ? 0 : -1;
                    }
                }
                else if (cbxMemo.Equals(c))
                {
                    cbxMemo.Checked = false;
                }
            }
        }

        #endregion

        #region clickEvent

        private void btnCusCodeFrom_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                if (customer.Code == null && customer.Name == null)
                {
                    txtCusCodeFrom.Clear();
                    lblCusNameFrom.Clear();
                }
                else
                {
                    txtCusCodeFrom.Text = customer.Code;
                    lblCusNameFrom.Text = customer.Name;
                }
                if (cbxCusCode.Checked)
                {
                    txtCusCodeTo.Text = txtCusCodeFrom.Text;
                    lblCusNameTo.Text = lblCusNameFrom.Text;
                }
                ClearStatusMessage();
            }
        }

        private void btnCusCodeTo_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                if (customer.Code == null && customer.Name == null)
                {
                    txtCusCodeTo.Clear();
                    lblCusNameTo.Clear();
                }
                else
                {
                    txtCusCodeTo.Text = customer.Code;
                    lblCusNameTo.Text = customer.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnStaffCodeFrom_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                if (staff.Code == null && staff.Name == null)
                {
                    txtStaffCodeFrom.Clear();
                    lblStaffNameFrom.Clear();
                }
                else
                {
                    txtStaffCodeFrom.Text = staff.Code;
                    lblStaffNameFrom.Text = staff.Name;
                }
                if (cbxStaffCode.Checked)
                {
                    txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                    lblStaffNameTo.Text = lblStaffNameFrom.Text;
                }
                ClearStatusMessage();
            }
        }

        private void btnStaffCodeTo_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                if (staff.Code == null && staff.Name == null)
                {
                    txtStaffCodeTo.Clear();
                    lblStaffNameTo.Clear();
                }
                else
                {
                    txtStaffCodeTo.Text = staff.Code;
                    lblStaffNameTo.Text = staff.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnDeCodeFrom_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                if (department.Code == null && department.Name == null)
                {
                    txtDepCodeFrom.Clear();
                    lblDepNameFrom.Clear();
                }
                else
                {
                    txtDepCodeFrom.Text = department.Code;
                    lblDepNameFrom.Text = department.Name;
                }
                if (cbxDeCode.Checked)
                {
                    txtDepCodeTo.Text = txtDepCodeFrom.Text;
                    lblDepNameTo.Text = lblDepNameFrom.Text;
                }
                ClearStatusMessage();
            }
        }

        private void btnDeCodeTo_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                if (department.Code == null && department.Name == null)
                {
                    txtDepCodeTo.Clear();
                    lblDepNameTo.Clear();
                }
                else
                {
                    txtDepCodeTo.Text = department.Code;
                    lblDepNameTo.Text = department.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            var allSelected = Departments.Count == DepartmentIdsInner.Count;
            using (var form = ApplicationContext.Create(nameof(PE0114)))
            {
                var screen = form.GetAll<PE0114>().First();
                screen.AllSelected = allSelected;
                screen.InitialIds = DepartmentIdsInner;
                screen.InitializeParentForm("請求部門絞込");

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);
                if (dialogResult == DialogResult.OK)
                {
                    ClearStatusMessage();
                    lblDepartmentName.Text = screen.SelectedState;
                    DepartmentIdsInner = screen.SelectedIds;
                }
            }
        }

        private void btnInitializeDepartmentSelection_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnInitializeDepartmentSelection);
            InitializeDepartmentSelection();
        }

        private void btnUpdateBy_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                if (loginUser.Code == null && loginUser.Name == null)
                {
                    txtUpdateBy.Clear();
                    lblUpdateByName.Clear();
                }
                else
                {
                    txtUpdateBy.Text = loginUser.Code;
                    lblUpdateByName.Text = loginUser.Name;
                    UpdateBy = loginUser.Id;
                }
                ClearStatusMessage();
            }
        }

        private void nmbAmountFrom_Validated(object sender, EventArgs e)
        {
            if (cbxAmountType.Checked)
            {
                nmbAmountTo.Value = nmbAmountFrom.Value;
            }
        }

        private void txtCusCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCusCodeFrom.Text))
                {
                    SetCustomerName(from: true);
                }
                else
                {
                    ClearStatusMessage();
                    lblCusNameFrom.Clear();
                    if (cbxCusCode.Checked)
                    {
                        txtCusCodeTo.Clear();
                        lblCusNameTo.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCusCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCusCodeTo.Text))
                {
                    SetCustomerName(from: false);
                }
                else
                {
                    ClearStatusMessage();
                    lblCusNameTo.Clear();
                }
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
                if (!string.IsNullOrEmpty(txtStaffCodeFrom.Text))
                {
                    SetStaffName(from: true);
                }
                else
                {
                    ClearStatusMessage();
                    lblStaffNameFrom.Clear();
                    if (cbxStaffCode.Checked)
                    {
                        txtStaffCodeTo.Clear();
                        lblStaffNameTo.Clear();
                    }
                }
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
                if (!string.IsNullOrEmpty(txtStaffCodeTo.Text))
                {
                    SetStaffName(from: false);
                }
                else
                {
                    ClearStatusMessage();
                    lblStaffNameTo.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDeCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtDepCodeFrom.Text))
                {
                    SetDepartmentName(from: true);
                }
                else
                {
                    ClearStatusMessage();
                    lblDepNameFrom.Clear();
                    if (cbxDeCode.Checked)
                    {
                        txtDepCodeTo.Clear();
                        lblDepNameTo.Clear();

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDeCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtDepCodeTo.Text))
                {
                    SetDepartmentName(from: false);
                }
                else
                {
                    ClearStatusMessage();
                    lblDepNameTo.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtUpdateBy_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtUpdateBy.Text))
                {
                    SetLoginUserName();
                }
                else
                {
                    ClearStatusMessage();
                    lblUpdateByName.Clear();
                    UpdateBy = null;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtInvoceCodeFrom_Validated(object sender, EventArgs e)
        {
            if (cbxInvoiceCode.Checked)
            {
                txtInvoceCodeTo.Text = txtInvoceCodeFrom.Text;
            }
        }

        private void nmbAmountFrom_Validating(object sender, EventArgs e)
        {
            if (cbxAmountType.Checked)
            {
                nmbAmountTo.Value = nmbAmountFrom.Value;
            }
        }

        #endregion

        #region subFunction

        private BillingSearch GetSearchCondition()
        {
            var options = new BillingSearch();
            options.CompanyId = CompanyId;
            options.BsBilledAtFrom = datBilledAtFrom.Value;
            options.BsBilledAtTo = datBilledAtTo.Value;

            options.BsSalesAtFrom = datSalesAtFrom.Value;
                options.BsSalesAtTo = datSalesAtTo.Value;

            options.BsDueAtFrom = datDueAtFrom.Value;
                options.BsDueAtTo = datDueAtTo.Value;

            options.BsCustomerName = txtCusName.Text;
            options.BsCustomerNameKana = txtCusNameKana.Text;
            options.BsPayerName = txtPayerName.Text;

            options.BsCustomerCodeFrom = txtCusCodeFrom.Text;
            options.BsCustomerCodeTo = txtCusCodeTo.Text;
            options.BsStaffCodeFrom = txtStaffCodeFrom.Text;
            options.BsStaffCodeTo = txtStaffCodeTo.Text;
            options.BsDepartmentCodeFrom = txtDepCodeFrom.Text;
            options.BsDepartmentCodeTo = txtDepCodeTo.Text;

            options.BsInvoiceCodeFrom = txtInvoceCodeFrom.Text;
            options.BsInvoiceCodeTo = txtInvoceCodeTo.Text;
            options.BsInvoiceCode = txtInvoiceCode.Text;

            if (cmbAmountType.SelectedIndex == 0)
            {
                options.BsRemaingAmountFrom = nmbAmountFrom.Value;
                options.BsRemaingAmountTo = nmbAmountTo.Value;
            }
            else
            {
                options.BsBillingAmountFrom = nmbAmountFrom.Value;
                options.BsBillingAmountTo = nmbAmountTo.Value;
            }

            options.ExistsMemo = cbxMemo.Checked;

            options.BsMemo = txtMemo.Text;

            if (cmbInputType.ValueMember != "")
                options.BsInputType = Convert.ToInt32(cmbInputType.SelectedItem.SubItems[1].Value);

            options.BsUpdateAtFrom = datUpdateAtFrom.Value;
            if (datUpdateAtTo.Value.HasValue)
            {
                var updatedTo = datUpdateAtTo.Value.Value;
                options.BsUpdateAtTo = updatedTo.Date.AddDays(1).AddMilliseconds(-1);
            }
            options.BsUpdateBy = UpdateBy;
            options.BsNote1 = txtNote1.Text;
            options.BsNote2 = txtNote2.Text;
            options.BsNote3 = txtNote3.Text;
            options.BsNote4 = txtNote4.Text;
            options.BsNote5 = txtNote5.Text;
            options.BsNote6 = txtNote6.Text;
            options.BsNote7 = txtNote7.Text;
            options.BsNote8 = txtNote8.Text;

            options.AssignmentFlg
                = (int)AssignmentFlagChecked.NoAssignment
                | (int)AssignmentFlagChecked.PartAssignment;

            if (ParentCustomerId != null && ParentCustomerId != 0)
            {
                options.ParentCustomerId = ParentCustomerId ?? 0;
            }
            options.UseDepartmentWork = Departments.Count != DepartmentIdsInner.Count;
            options.ClientKey = ClientKey;
            options.LoginUserId = Login.UserId;
            if (!string.IsNullOrEmpty(CurrencyCode))
            {
                options.BsCurrencyCode = CurrencyCode;
            }
            if (cmbSeikyuKubun.SelectedItem != null)
            {
                options.BsBillingCategoryId = Convert.ToInt32(cmbSeikyuKubun.SelectedItem.SubItems[1].Value);
            }
            options.KobetsuType = "Matching";
            return options;
        }

        private bool CheckInput()
        {
            if (!datBilledAtFrom.ValidateRange(datBilledAtTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblBilledAt.Text))) return false;
            if (!datSalesAtFrom.ValidateRange(datSalesAtTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblSalesAt.Text))) return false;
            if (!datDueAtFrom.ValidateRange(datDueAtTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblDueAt.Text))) return false;
            if (!txtCusCodeFrom.ValidateRange(txtCusCodeTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblCusCode.Text))) return false;
            if (!txtStaffCodeFrom.ValidateRange(txtStaffCodeTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaffCode.Text))) return false;
            if (!txtDepCodeFrom.ValidateRange(txtDepCodeTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblDepCode.Text))) return false;
            if (!txtInvoceCodeFrom.ValidateRange(txtInvoceCodeTo,
              () => ShowWarningDialog(MsgWngInputRangeChecked, lblInvoceCode.Text))) return false;
            if (!nmbAmountFrom.ValidateRange(nmbAmountTo,
              () => ShowWarningDialog(MsgWngInputRangeChecked, "金額"))) return false;
            if (!datUpdateAtFrom.ValidateRange(datUpdateAtTo,
              () => ShowWarningDialog(MsgWngInputRangeChecked, lblUpdateAt.Text))) return false;
            return true;
        }

        /// <summary> 請求区分設定 </summary>
        private async Task LoadBillingCategoryCombo()
        {
            CategoriesResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, 1, new string[] { });
            });
            if (result.ProcessResult.Result)
            {
                CategoryList = result.Categories;
                cmbSeikyuKubun.Items.Add(new ListItem("すべて", 0));
                for (int i = 0; i < CategoryList.Count(); i++)
                {
                    cmbSeikyuKubun.Items.Add(new ListItem(CategoryList[i].Code + " : " + CategoryList[i].Name, CategoryList[i].Id));
                }
                cmbSeikyuKubun.SelectedIndex = 0;
            }
        }

        private async Task LoadLeagalPersonarities()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<JuridicalPersonalityMasterClient>();
                var result = await client.GetItemsAsync(SessionKey, CompanyId);

            });
        }

        private async Task LoadColumnNameSettingAsync() =>
            await Util.LoadColumnNameSettingAsync(Login, nameof(Billing), settings =>
            {
                foreach (var setting in settings)
                {
                    VOneLabelControl label = null;
                    if (setting.ColumnName == nameof(Billing.Note1)) label = lblNote1;
                    if (setting.ColumnName == nameof(Billing.Note2)) label = lblNote2;
                    if (setting.ColumnName == nameof(Billing.Note3)) label = lblNote3;
                    if (setting.ColumnName == nameof(Billing.Note4)) label = lblNote4;
                    if (setting.ColumnName == nameof(Billing.Note5)) label = lblNote5;
                    if (setting.ColumnName == nameof(Billing.Note6)) label = lblNote6;
                    if (setting.ColumnName == nameof(Billing.Note7)) label = lblNote7;
                    if (setting.ColumnName == nameof(Billing.Note8)) label = lblNote8;
                    if (label == null) continue;
                    label.Text = setting.DisplayColumnName;
                }
            });

        /// <summary> 得意先名設定 </summary>
        /// <param name="from"> 得意先「From/To」</param>
        private void SetCustomerName(bool from)
        {
            if (from)
            {
                string code = txtCusCodeFrom.Text;
                string name = "";
                if (!string.IsNullOrEmpty(code))
                {
                    name = GetCustomerName(code);
                }
                lblCusNameFrom.Text = name;
                if (cbxCusCode.Checked)
                {
                    lblCusNameTo.Text = name;
                    txtCusCodeTo.Text = code;
                }
            }
            else
            {
                string code = txtCusCodeTo.Text;
                string name = "";
                if (!string.IsNullOrEmpty(code))
                {
                    name = GetCustomerName(code);
                    lblCusNameTo.Text = name;
                }
            }
        }

        /// <summary> 得意先名取得 </summary>
        /// <param name="code"> 得意先コード</param>
        /// <returns>得意先名</returns>
        private string GetCustomerName(string code)
        {
            var name = string.Empty;
            CustomersResult result = null;
            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var customer = result?.Customers?.FirstOrDefault(x => x != null);
            if (customer != null) name = customer.Name;
            return name;
        }

        /// <summary> 担当者名設定 </summary>
        /// <param name="from"> 担当者「From/To」</param>
        private void SetStaffName(bool from)
        {
            string name = "";
            string code = "";
            if (from)
            {
                code = txtStaffCodeFrom.Text;
            }
            else
            {
                code = txtStaffCodeTo.Text;
            }
            if (!string.IsNullOrEmpty(code))
            {
                name = GetStaffName(code);
            }
            if (from)
            {
                lblStaffNameFrom.Text = name;

                if (cbxStaffCode.Checked)
                {
                    lblStaffNameTo.Text = name;
                    txtStaffCodeTo.Text = code;
                }
            }
            else
            {
                lblStaffNameTo.Text = name;
            }
        }

        /// <summary> 担当者名取得 </summary>
        /// <param name="code"> 担当者コード</param>
        /// <returns>担当者名</returns>
        private string GetStaffName(string code)
        {
            var name = string.Empty;
            StaffsResult result = null;
            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var staff = result?.Staffs?.FirstOrDefault(x => x != null);
            if (staff != null) name = staff.Name;
            return name;
        }

        /// <summary> 請求部門名設定 </summary>
        /// <param name="from"> 請求部門「From/To」</param>
        private void SetDepartmentName(bool from)
        {
            string name = "";
            string code = "";
            if (from)
            {
                code = txtDepCodeFrom.Text;
            }
            else
            {
                code = txtDepCodeTo.Text;
            }

            if (!string.IsNullOrEmpty(code))
            {
                name = GetDepartmentName(code);
            }
            if (from)
            {
                lblDepNameFrom.Text = name;
                if (cbxDeCode.Checked)
                {
                    lblDepNameTo.Text = name;
                    txtDepCodeTo.Text = code;
                }
            }
            else
            {
                lblDepNameTo.Text = name;
            }
        }

        /// <summary> 請求部門名取得 </summary>
        /// <param name="code"> 請求部門コード</param>
        /// <returns>請求部門名</returns>
        private string GetDepartmentName(string code)
        {
            var name = string.Empty;
            DepartmentsResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var department = result?.Departments?.FirstOrDefault(x => x != null);
            if (department != null)
            {
                name = department.Name;
                ClearStatusMessage();
            }
            return name;
        }

        /// <summary> 最終更新者名設定 </summary>
        private void SetLoginUserName()
        {
            string name = "";
            string code = "";
            code = txtUpdateBy.Text;
            if (!string.IsNullOrEmpty(code))
            {
                name = GetLoginUserName(code);
            }
            if (string.IsNullOrEmpty(name))
            {
                ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", code);
                lblUpdateByName.Clear();
                txtUpdateBy.Clear();
                UpdateBy = null;
                txtUpdateBy.Focus();
            }
            else
            {
                lblUpdateByName.Text = name;
            }
        }

        /// <summary> 最終更新者取得 </summary>
        /// <param name="code"> 最終更新者コード</param>
        /// <returns>最終更新者名</returns>
        private string GetLoginUserName(string code)
        {
            string name = string.Empty;
            UsersResult result = null;
            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<LoginUserMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var user = result?.Users?.FirstOrDefault(x => x != null);
            if (user != null)
            {
                name = user.Name;
                ClearStatusMessage();
                UpdateBy = user.Id;
            }
            return name;
        }

        /// <summary> 入力区分設定 </summary>
        private void SetComboForInputType()
        {
            cmbInputType.Items.Add(new ListItem("すべて", 0));
            cmbInputType.Items.Add(new ListItem("取込", (int)BillingInputType.Importer));
            cmbInputType.Items.Add(new ListItem("入力", (int)BillingInputType.BillingInput));
            cmbInputType.Items.Add(new ListItem("定期請求", (int)BillingInputType.PeriodicBilling));

            cmbInputType.SelectedIndex = 0;
        }
        #endregion
    }
}
