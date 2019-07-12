using GrapeCity.Win.Editors;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>個別消込・入金検索</summary>
    public partial class PE0104 : VOneScreenBase
    {
        public string CurrencyCode { get; set; } = "";
        public DateTime? RecordedAtFrom { get; set; }
        public DateTime? RecordedAtTo { get; set; }
        public List<long> ReceiptId { get; set; } = new List<long>();
        public bool UsingSectionMaster { get; set; }
        public Receipt[] ReceiptInfo { get; set; }
        private List<Category> CategoryList { get; set; } = null;
        internal byte[] ClientKey { private get; set; }
        internal List<Section> Sections { private get; set; }
        internal List<int> SectionIds { private get; set; }
        private List<int> SectionIdsInner { get; set; }

        private List<string> LegalPersonalities { get; set; }

        private int? UpdateBy { get; set; }

        public PE0104()
        {
            InitializeComponent();
            InitializeUserComponent();
        }
        private void InitializeUserComponent()
        {
            Text = "個別消込・入金検索";
            txtPayerName.Validated += (sender, e)
                => txtPayerName.Text = EbDataHelper.ConvertToPayerName(txtPayerName.Text, LegalPersonalities);
        }

        private void PE0104_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                cmbAmountType.SelectedIndex = 0;
                cmbInputKubun.SelectedIndex = 0;
                datNyuukinFrom.Clear();
                datNyuukinFrom.Value = RecordedAtFrom;
                datNyuukinTo.Value = RecordedAtTo;
                datUpdateAtFrom.Clear();
                datUpdateAtTo.Clear();
                var tasks = new List<Task>();
                if (Company == null)
                {
                    tasks.Add(LoadCompanyAsync());
                }
                if (ApplicationControl == null)
                {
                    tasks.Add(LoadApplicationControlAsync());
                }
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadNyuukinCategoryCombo());
                tasks.Add(LoadColumnNameSettingAsync());
                tasks.Add(LoadLegalPersonalitiesAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
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
                    txtSecCodeFrom.Format = expression.SectionCodeFormatString;
                    txtSecCodeFrom.MaxLength = expression.SectionCodeLength;
                    txtSecCodeFrom.PaddingChar = expression.SectionCodePaddingChar;
                    txtSecCodeTo.Format = expression.SectionCodeFormatString;
                    txtSecCodeTo.MaxLength = expression.SectionCodeLength;
                    txtSecCodeTo.PaddingChar = expression.SectionCodePaddingChar;
                    txtUpdatedBy.Format = expression.LoginUserCodeFormatString;
                    txtUpdatedBy.MaxLength = expression.LoginUserCodeLength;
                    txtUpdatedBy.PaddingChar = expression.LoginUserCodePaddingChar;
                }
                pnlSection.Visible = UseSection;
                var format = UseForeignCurrency
                    ? "###,###,###,##0.00000"
                    : "###,###,###,##0";
                nmbAmountFrom.DisplayFields.AddRange(format, "", "", "-", "");
                nmbAmountTo.DisplayFields.AddRange(format, "", "", "-", "");
                InitializeSectionSelection();
                Settings.SetCheckBoxValue<PE0104>(Login, cbxCusCode);
                Settings.SetCheckBoxValue<PE0104>(Login, cbxSecCode);

                txtPayerCodePrefix.PaddingChar = '0';
                txtPayerCodesuffix.PaddingChar = '0';
                datNyuukinFrom.Select();
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
            OnF01ClickHandler = ReceiptSearch;
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Close;

        }

        private void InitializeSectionSelection()
        {
            if (Sections.Count == SectionIds.Count)
            {
                lblSectionName.Text = "すべて";
            }
            else if (SectionIds.Count == 1)
            {
                lblSectionName.Text = Sections.First(x => SectionIds.Contains(x.Id)).Name;
            }
            else
            {
                lblSectionName.Text = "入金部門絞込有";
            }
            SectionIdsInner = SectionIds;
        }

        #region functionKey Event

        [OperationLog("検索")]
        private void ReceiptSearch()
        {
            if (!ValidateChildren()) return;
            if (!ValidateForSearch()) return;
            ReceiptsResult resultReceipt = null;
            ReceiptSearch receiptSearch = GetSearchCondition();
            try
            {
                var task = LoadReceiptAsync(receiptSearch);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                resultReceipt = task.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if (!resultReceipt.ProcessResult.Result)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }

            var searchResult = resultReceipt.Receipts.Where(x => !ReceiptId.Contains(x.Id)).ToArray();
            if (searchResult.Length == 0)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                return;
            }
            else
            {
                ReceiptInfo = searchResult;
                ParentForm.DialogResult = DialogResult.OK;
            }
        }

        private async Task<ReceiptsResult> LoadReceiptAsync(ReceiptSearch options)
        {
            var useSectionFilter = UseSection && Sections.Count != SectionIdsInner.Count;
            if (useSectionFilter)
                await Util.SaveWorkSectionTargetAsync(Login, ClientKey, SectionIdsInner.ToArray());
            return await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client)
                => await client.GetItemsAsync(SessionKey, options));
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            ClearControls();
            datNyuukinFrom.Value = RecordedAtFrom;
            datNyuukinTo.Value = RecordedAtTo;
            datNyuukinFrom.Focus();
        }

        [OperationLog("戻る")]
        public void Close()
        {
            Settings.SaveControlValue<PE0104>(Login, cbxCusCode.Name, cbxCusCode.Checked);
            Settings.SaveControlValue<PE0104>(Login, cbxSecCode.Name, cbxSecCode.Checked);
            BaseForm.Close();
        }

        #endregion

        #region event handlers

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
                    if (cbxCusCode.Checked)
                    {
                        txtCusCodeTo.Text = customer.Code;
                        lblCusNameTo.Text = customer.Name;
                    }
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

        private void btnSecCodeFrom_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var section = this.ShowSectionSearchDialog();
            if (section == null) return;
            txtSecCodeFrom.Text = section.Code;
            lblSecNameFrom.Text = section.Name;
            if (cbxSecCode.Checked)
            {
                txtSecCodeTo.Text = section.Code;
                lblSecNameTo.Text = section.Name;
            }
        }

        private void btnSecCodeTo_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var section = this.ShowSectionSearchDialog();
            if (section == null) return;
            txtSecCodeTo.Text = section.Code;
            lblSecNameTo.Text = section.Name;
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            var allSelected = Sections.Count == SectionIdsInner.Count;
            using (var form = ApplicationContext.Create(nameof(PE0106)))
            {
                var screen = form.GetAll<PE0106>().First();
                screen.AllSection = allSelected;
                screen.InitialIds = SectionIdsInner;
                screen.InitializeParentForm("入金部門絞込");
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);
                if (dialogResult == DialogResult.OK)
                {
                    ClearStatusMessage();
                    lblSectionName.Text = screen.SelectedState;
                    SectionIdsInner = screen.SelectedIds;
                }
            }
        }

        private void btnInitializeSectionSelection_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnInitializeSectionSelection);
            InitializeSectionSelection();
        }

        private void btnUpdatedBy_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                if (loginUser.Code == null && loginUser.Name == null)
                {
                    txtUpdatedBy.Clear();
                    lblUpdateBy.Clear();
                }
                else
                {
                    txtUpdatedBy.Text = loginUser.Code;
                    lblUpdateBy.Text = loginUser.Name;
                    UpdateBy = loginUser.Id;
                }
                ClearStatusMessage();
            }
        }

        private void txtCusCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                SetCustomerName(from: true);
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
                ClearStatusMessage();
                SetCustomerName(from: false);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtSecCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                SetSectionName(from: true);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtSecCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                SetSectionName(from: false);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtUpdatedBy_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtUpdatedBy.Text))
                {
                    SetLoginUserName();
                }
                else
                {
                    ClearStatusMessage();
                    lblUpdateBy.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
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
                    if (lblSectionName.Equals(c)) continue;
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
                    if (cmbNyukinKubun.Equals(c) && CategoryList != null)
                    {
                        ((VOneComboControl)c).SelectedIndex = 0;
                    }
                    if (cmbInputKubun.Equals(c))
                    {
                        ((VOneComboControl)c).SelectedIndex = 0;
                    }
                    if (cmbAmountType.Equals(c))
                    {
                        ((VOneComboControl)c).SelectedIndex = ((VOneComboControl)c).Items.Count > 0 ? 0 : -1;
                    }
                }
                else if (cbxReceiptMemo.Equals(c))
                {
                    cbxReceiptMemo.Checked = false;
                }
            }
        }
        #endregion

        #region subFunction

        private bool ValidateForSearch()
        {
            if (!datNyuukinFrom.ValidateRange(datNyuukinTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblNyuukin.Text))) return false;
            if (!nmbAmountFrom.ValidateRange(nmbAmountTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, "金額"))) return false;
            if (!txtCusCodeFrom.ValidateRange(txtCusCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCusCode.Text))) return false;
            if (!txtSecCodeFrom.ValidateRange(txtSecCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblSecCode.Text))) return false;
            if (!datUpdateAtFrom.ValidateRange(datUpdateAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblUpdateAt.Text))) return false;
            return true;
        }

        private ReceiptSearch GetSearchCondition()
        {
            var options = new ReceiptSearch();
            options.CompanyId = CompanyId;
            options.RecordedAtFrom = datNyuukinFrom.Value;
            options.RecordedAtTo = datNyuukinTo.Value;
            options.PayerName = txtPayerName.Text;

            if (datUpdateAtFrom.Value.HasValue)
            {
                options.UpdateAtFrom = datUpdateAtFrom.Value.Value.Date;
            }
            if (datUpdateAtTo.Value.HasValue)
            {
                DateTime fromNyuukin = datUpdateAtTo.Value.Value;
                options.UpdateAtTo = fromNyuukin.Date.AddDays(1).AddMilliseconds(-1);
            }
            options.UpdateBy = UpdateBy;

            options.CustomerCodeFrom = txtCusCodeFrom.Text;
            options.CustomerCodeTo = txtCusCodeTo.Text;
            options.SectionCodeFrom = txtSecCodeFrom.Text;
            options.SectionCodeTo = txtSecCodeTo.Text;
            options.ExistsMemo = cbxReceiptMemo.Checked ? 1 : 0;
            options.ReceiptMemo = txtReceiptMemo.Text;
            if (cmbInputKubun.SelectedIndex > 0)
            {
                options.InputType = cmbInputKubun.SelectedIndex;
            }
            if (cmbNyukinKubun.SelectedItem != null)
            {
                options.ReceiptCategoryId = Convert.ToInt32(cmbNyukinKubun.SelectedItem.SubItems[1].Value);
            }

            if (cmbAmountType.SelectedIndex == 0)
            {
                options.RemainAmountFrom = nmbAmountFrom.Value;
                options.RemainAmountTo = nmbAmountTo.Value;
            }
            else
            {
                options.ReceiptAmountFrom = nmbAmountFrom.Value;
                options.ReceiptAmountTo = nmbAmountTo.Value;
            }
            options.Note1 = txtNote1.Text;
            options.Note2 = txtNote2.Text;
            options.Note3 = txtNote3.Text;
            options.Note4 = txtNote4.Text;
            options.PayerCodePrefix = txtPayerCodePrefix.Text;
            options.PayerCodeSuffix = txtPayerCodesuffix.Text;

            options.UseForeignCurrencyFlg = ApplicationControl.UseForeignCurrency;
            options.AssignmentFlag
                = (int)AssignmentFlagChecked.NoAssignment
                | (int)AssignmentFlagChecked.PartAssignment;
            if (UseSection)
            {
                options.UseSectionWork = Sections.Count != SectionIdsInner.Count;
                options.ClientKey = ClientKey;
            }
            options.LoginUserId = Login.UserId;
            if (!string.IsNullOrEmpty(CurrencyCode))
            {
                options.CurrencyCode = CurrencyCode;
            }
            options.KobetsuType = "Matching";
            return options;
        }

        #endregion

        #region web service call
        private async Task LoadLegalPersonalitiesAsync()
            => await ServiceProxyFactory.DoAsync<JuridicalPersonalityMasterService.JuridicalPersonalityMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    LegalPersonalities = result.JuridicalPersonalities.Select(x => x.Kana).ToList();
                else
                    LegalPersonalities = new List<string>();
            });

        /// <summary> 入金区分設定 </summary>
        private async Task LoadNyuukinCategoryCombo()
        {
            CategoriesResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                result = await service.GetByCodeAsync(SessionKey,
                    CompanyId, CategoryType.Receipt, new string[] { }
                );
            });

            if (result.ProcessResult.Result)
            {
                CategoryList = result.Categories;
                if (CategoryList != null)
                {
                    cmbNyukinKubun.Items.Add(new ListItem("すべて", 0));
                    foreach (var category in CategoryList)
                    {
                        cmbNyukinKubun.Items.Add(new ListItem(category.Code + " : " + category.Name, category.Id));
                    }
                    cmbNyukinKubun.SelectedIndex = 0;
                }
            }
        }

        private async Task LoadColumnNameSettingAsync() =>
            await Util.LoadColumnNameSettingAsync(Login, nameof(Receipt), settings =>
            {
                foreach (var setting in settings)
                {
                    VOneLabelControl label = null;
                    if (setting.ColumnName == nameof(Receipt.Note1)) label = lblNote1;
                    if (setting.ColumnName == nameof(Receipt.Note2)) label = lblNote2;
                    if (setting.ColumnName == nameof(Receipt.Note3)) label = lblNote3;
                    if (setting.ColumnName == nameof(Receipt.Note4)) label = lblNote4;
                    if (label == null) continue;
                    label.Text = setting.DisplayColumnName;
                }
            });

        /// <summary> 得意先名設定 </summary>
        /// <param name="from"> 得意先「From/To」</param>
        private void SetCustomerName(bool from)
        {
            var name = "";
            if (from)
            {
                var code = txtCusCodeFrom.Text;
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
                var code = txtCusCodeTo.Text;
                if (!string.IsNullOrEmpty(code))
                {
                    name = GetCustomerName(code);
                }
                lblCusNameTo.Text = name;
            }
        }

        private void SetSectionName(bool from)
        {
            var name = "";
            if (from)
            {
                var code = txtSecCodeFrom.Text;
                if (!string.IsNullOrEmpty(code))
                {
                    name = GetSectionName(code);
                }
                lblSecNameFrom.Text = name;
                if (cbxSecCode.Checked)
                {
                    txtSecCodeTo.Text = code;
                    lblSecNameTo.Text = name;
                }
            }
            else
            {
                var code = txtSecCodeTo.Text;
                if (!string.IsNullOrEmpty(code))
                {
                    name = GetSectionName(code);
                }
                lblSecNameTo.Text = name;
            }
        }

        /// <summary> 得意先名取得 </summary>
        /// <param name="code"> 得意先コード</param>
        /// <returns>得意先名</returns>
        private string GetCustomerName(string code)
        {
            var task = ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result) return result.Customers.FirstOrDefault();
                return null;
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task?.Result?.Name;
        }

        private string GetSectionName(string code)
        {
            var task = ServiceProxyFactory.DoAsync(async (SectionMasterService.SectionMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result) return result.Sections.FirstOrDefault();
                return null;
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task?.Result?.Name;
        }

        /// <summary> 最終更新者名設定 </summary>
        private void SetLoginUserName()
        {
            string name = "";
            string code = "";
            code = txtUpdatedBy.Text;
            if (!string.IsNullOrWhiteSpace(code))
            {
                name = GetLoginUserName(code);
            }
            if (string.IsNullOrEmpty(name))
            {
                ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", code);
                lblUpdateBy.Clear();
                txtUpdatedBy.Clear();
                UpdateBy = null;
                txtUpdatedBy.Select();
            }
            else
            {
                lblUpdateBy.Text = name;
            }
        }

        /// <summary> 最終更新者取得 </summary>
        /// <param name="code"> 最終更新者コード</param>
        /// <returns>最終更新者名</returns>
        private string GetLoginUserName(string code)
        {
            var name = string.Empty;
            UsersResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<LoginUserMasterClient>();

                result = await service.GetByCodeAsync(SessionKey,
                CompanyId, new string[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var user = result?.Users?.FirstOrDefault(x => x != null);
            if (user != null)
            {
                ClearStatusMessage();
                name = user.Name;
                UpdateBy = user.Id;
            }
            return name;
        }

        #endregion

    }
}
