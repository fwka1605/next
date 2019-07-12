using GrapeCity.Win.Editors;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.PaymentAgencyMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>決済代行会社マスター</summary>
    public partial class PB1901 : VOneScreenBase
    {
        private int PaymentAgencyId { get; set; }
        public string PaymentAgencyCode { get; set; }
        private List<PaymentFileFormat> PaymentFileFormatList { get; set; }
        private List<Category> CollectCategoryList { get; set; }
        private IEnumerable<string> LegalPersonalities { get; set; }
        private DateTime UpdateAt { get; set; }
        private bool MatchingChangeFee { get; set; }
        private bool MenuAuthority { get; set; }

        #region 画面の初期化
        public PB1901()
        {
            InitializeComponent();
            Text = "決済代行会社マスター";
            AddHandlers();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SavePaymentAgency;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearPaymentAgency;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = DeletePaymentAgency;

            BaseContext.SetFunction08Caption("登録手数料");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = OpenRegistrationFee;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitPaymentAgency;
        }

        private void PB1901_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                this.ActiveControl = txtPaymentAgencyCode;
                txtPaymentAgencyCode.Select();
                txtPaymentAgencyCode.PaddingChar = '0';
                var tasks = new List<Task>();
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(BindFileFormatCombo());
                tasks.Add(LoadCollectCategoryInfoAsync());
                tasks.Add(LoadLegalPersonalitiesAsync());
                tasks.Add(LoadMenuAuthority());
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                ComboBind();
                Clear();
                if (PaymentAgencyCode != null)
                {
                    txtPaymentAgencyCode.Text = PaymentAgencyCode;
                    txtPaymentAgencyCode_Validated(this, e);
                    MatchingFormSetting();
                    this.ActiveControl = txtConsigneeCode;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ComboBind()
        {
            cmbShareTransferFee.Items.Add(new ListItem("相手先", 0));
            cmbShareTransferFee.Items.Add(new ListItem("自社", 1));

            cmbAccountType.Items.Add(new ListItem("普通", 1));
            cmbAccountType.Items.Add(new ListItem("当座", 2));
            cmbAccountType.Items.Add(new ListItem("納税準備", 3));
            cmbAccountType.Items.Add(new ListItem("その他", 9));
        }

        private void MatchingFormSetting()
        {
            if (MenuAuthority)
            {
                BaseContext.SetFunction01Enabled(true);
            }
            else
            {
                BaseContext.SetFunction01Enabled(false);
                this.Enabled = false;
            }
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Caption("戻る");
        }
        private void AddHandlers()
        {
            foreach (Control control in this.GetAll<Common.Controls.VOneTextControl>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }
            foreach (Control control in this.GetAll<Common.Controls.VOneComboControl>())
            {
                ((Common.Controls.VOneComboControl)control).SelectedIndexChanged +=
                    new EventHandler(OnContentChanged);
            }
            foreach (Control control in this.GetAll<CheckBox>())
            {
                ((CheckBox)control).CheckedChanged +=
                    new EventHandler(OnContentChanged);
            }
            foreach (Control control in this.GetAll<Common.Controls.VOneNumberControl>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }
        #endregion

        #region Function Key Event
        [OperationLog("登録")]
        private void SavePaymentAgency()
        {
            ClearStatusMessage();
            if (!RequireFieldsChecking()) return;
            ZeroLeftPaddingWithoutValidated();
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            Save();
            txtPaymentAgencyCode.Select();
        }

        [OperationLog("クリア")]
        private void ClearPaymentAgency()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            Clear();
            ClearStatusMessage();
        }

        public void Clear()
        {
            txtPaymentAgencyCode.Clear();
            txtPaymentAgencyName.Clear();
            txtPaymentAgencyKana.Clear();
            txtConsigneeCode.Clear();
            txtBankCode.Clear();
            txtBankName.Clear();
            txtBranchCode.Clear();
            txtBranchName.Clear();
            cmbAccountType.SelectedIndex = -1;
            txtAccountNumber.Clear();
            cmbShareTransferFee.SelectedIndex = -1;
            cbxUseFeeTolerance.Checked = false;
            cbxUseFeeLearning.Checked = false;
            cbxUseKanaLearning.Checked = false;
            txtOutputFileName.Clear();
            cbxAppendDate.Checked = false;
            nmbDueDateOffset.Clear();
            cmbFileFormat.SelectedIndex = -1;
            lblContractCode.Enabled = false;
            txtContractCode.Enabled = false;
            txtContractCode.Clear();
            cbxConsiderUncollected.Checked = false;
            cmbConsiderUncollected.SelectedIndex = -1;
            cbxUseFeeTolerance.Enabled = false;
            cbxUseFeeLearning.Enabled = false;
            cmbConsiderUncollected.Enabled = false;
            txtPaymentAgencyCode.Enabled = true;
            BaseContext.SetFunction03Enabled(false);
            txtPaymentAgencyCode.Focus();
            this.ActiveControl = txtPaymentAgencyCode;
            PaymentAgencyId = 0;
            Modified = false;
        }

        [OperationLog("削除")]
        private void DeletePaymentAgency()
        {
            try
            {
                var categoryFlag = false;
                var taskCategory = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CategoryMasterClient>();
                    var checkResult = await service.ExistPaymentAgencyAsync(SessionKey, PaymentAgencyId);
                    categoryFlag = checkResult.Exist;
                });
                ProgressDialog.Start(ParentForm, taskCategory, false, SessionKey);

                if (categoryFlag)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, "区分マスター", "決済代行会社コード");
                    return;
                }
                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    if (!ShowConfirmDialog(MsgQstConfirmDelete))
                    {
                        DispStatusMessage(MsgInfProcessCanceled);
                        return;
                    }
                    bool succeeded = await Delete();
                    if (succeeded)
                    {
                        Clear();
                        DispStatusMessage(MsgInfDeleteSuccess);
                    }
                    else
                    {
                        ShowWarningDialog(MsgErrDeleteError);
                    }
                    txtPaymentAgencyCode.Focus();
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void ExitPaymentAgency()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.DialogResult = MatchingChangeFee ? DialogResult.OK : DialogResult.Cancel;
            MatchingChangeFee = false;
            ParentForm.Close();
        }

        [OperationLog("登録手数料")]
        private void OpenRegistrationFee()
        {
            using (var form = ApplicationContext.Create(nameof(PB0505)))
            {
                var screen = form.GetAll<PB0505>().First();
                screen.CustomerId = PaymentAgencyId;
                if (MenuAuthority == false)
                {
                    screen.RegFee = false;
                }
                screen.PaymentAgencyFlag = true;
                screen.InitializeParentForm("登録手数料");
                var result = ApplicationContext.ShowDialog(BaseForm, form, true);
                MatchingChangeFee = screen.PaymentChangeFee;
            }
        }

        private void btnPaymentAgencyCode_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

            var paymentAgency = this.ShowPaymentAgencySearchDialog();
            if (paymentAgency != null)
            {
                FillData(paymentAgency);
                txtPaymentAgencyCode.Enabled = false;
                Modified = false;
            }
        }

        private void btnBankCode_Click(object sender, EventArgs e)
        {
            var bankBranch = this.ShowBankBranchSearchDialog();
            if (bankBranch != null)
            {
                txtBankCode.Text = bankBranch.BankCode;
                txtBankName.Text = bankBranch.BankKana;
                txtBranchCode.Text = bankBranch.BranchCode;
                txtBranchName.Text = bankBranch.BranchKana;
            }
        }
        #endregion

        #region Webサービス呼び出し
        
        private async Task LoadMenuAuthority()
        {
            await ServiceProxyFactory.DoAsync(async (MenuAuthorityMasterService.MenuAuthorityMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, Login.UserId);
                MenuAuthority = (result?.ProcessResult.Result ?? false)
                ? result.MenuAuthorities.Any(x => x.MenuId == nameof(PB1901))
                : false;
            });
        }

        /// <summary>法人格除去用</summary>
        private async Task LoadLegalPersonalitiesAsync()
        {
            await ServiceProxyFactory.DoAsync<JuridicalPersonalityMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    LegalPersonalities = result.JuridicalPersonalities.Select(x => x.Kana);
            });
        }

        private async Task LoadCollectCategoryInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                var categorySearch = new CategorySearch();
                categorySearch.CategoryType = 3;
                categorySearch.CompanyId = CompanyId;
                categorySearch.UseAccountTransfer = 0;
                var result = await service.GetItemsAsync(SessionKey, categorySearch);

                if (result.ProcessResult.Result)
                {
                    CollectCategoryList = result.Categories;
                    if (CollectCategoryList != null)
                    {
                        for (int i = 0; i < CollectCategoryList.Count; i++)
                        {
                            cmbConsiderUncollected.Items.Add(new ListItem(CollectCategoryList[i].Name, CollectCategoryList[i].Id));
                            cmbConsiderUncollected.Items[i].Tag = CollectCategoryList[i];
                        }
                    }
                }
            });
        }

        private async Task BindFileFormatCombo()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<PaymentAgencyMasterClient>();
                var result = await service.GetFormatItemsAsync(SessionKey);

                if (result.ProcessResult.Result)
                {
                    PaymentFileFormatList = result.PaymentFileFormats;
                    if (PaymentFileFormatList != null)
                    {
                        for (int i = 0; i < PaymentFileFormatList.Count; i++)
                        {
                            cmbFileFormat.Items.Add(new ListItem(PaymentFileFormatList[i].Name, PaymentFileFormatList[i].Id));
                            cmbFileFormat.Items[i].Tag = PaymentFileFormatList[i];
                        }
                    }
                }
            });
        }

        private void Save()
        {
            var paymentAgencyInsert = new PaymentAgency();
            paymentAgencyInsert.Id = PaymentAgencyId;
            paymentAgencyInsert.CompanyId = CompanyId;
            paymentAgencyInsert.Code = txtPaymentAgencyCode.Text;
            paymentAgencyInsert.ConsigneeCode = txtConsigneeCode.Text;
            paymentAgencyInsert.Name = txtPaymentAgencyName.Text.Trim();
            paymentAgencyInsert.Kana = txtPaymentAgencyKana.Text;
            paymentAgencyInsert.BankCode = txtBankCode.Text;
            paymentAgencyInsert.BankName = txtBankName.Text.Trim();
            paymentAgencyInsert.BranchCode = txtBranchCode.Text;
            paymentAgencyInsert.BranchName = txtBranchName.Text.Trim();
            paymentAgencyInsert.AccountTypeId = Convert.ToInt32(cmbAccountType.SelectedItem.SubItems[1].Value);
            paymentAgencyInsert.AccountNumber = txtAccountNumber.Text;
            paymentAgencyInsert.ShareTransferFee = cmbShareTransferFee.SelectedIndex;
            paymentAgencyInsert.UseFeeLearning = cbxUseFeeLearning.Checked ? 1 : 0;
            paymentAgencyInsert.UseFeeTolerance = cbxUseFeeTolerance.Checked ? 1 : 0;
            paymentAgencyInsert.UseKanaLearning = cbxUseKanaLearning.Checked ? 1 : 0;
            paymentAgencyInsert.ConsiderUncollected = cbxConsiderUncollected.Checked ? 1 : 0;

            paymentAgencyInsert.DueDateOffset = Convert.ToInt32(nmbDueDateOffset.Value);
            paymentAgencyInsert.FileFormatId = Convert.ToInt32(cmbFileFormat.SelectedItem.SubItems[1].Value);
            paymentAgencyInsert.ContractCode = txtContractCode.Text.Trim();
            if (cbxConsiderUncollected.Checked)
            {
                paymentAgencyInsert.CollectCategoryId = Convert.ToInt32(cmbConsiderUncollected.SelectedItem.SubItems[1].Value);
            }
            paymentAgencyInsert.OutputFileName = txtOutputFileName.Text;
            paymentAgencyInsert.AppendDate = cbxAppendDate.Checked ? 1 : 0;

            paymentAgencyInsert.CreateBy = Login.UserId;
            paymentAgencyInsert.UpdateBy = Login.UserId;
            paymentAgencyInsert.UpdateAt = UpdateAt;

            PaymentAgencyResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<PaymentAgencyMasterClient>();
                result = await service.SaveAsync(SessionKey, paymentAgencyInsert);
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);


            if (result.PaymentAgency != null)
            {
                Clear();
                DispStatusMessage(MsgInfSaveSuccess);
                if (PaymentAgencyCode != null)
                {
                    ParentForm.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }
        }

        private async Task<bool> Delete()
        {
            CountResult deletePayment = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<PaymentAgencyMasterClient>();
                deletePayment = await service.DeleteAsync(SessionKey, PaymentAgencyId);
            });
            return deletePayment.ProcessResult.Result && deletePayment.Count > 0;
        }
        #endregion

        #region Form Sub Function
        private bool RequireFieldsChecking()
        {
            if (!txtPaymentAgencyCode.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblPaymentAgencyCode.Text))) return false;

            if (!txtConsigneeCode.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblConsigneeCode.Text))) return false;

            if (!txtPaymentAgencyName.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblPaymentAgencyName.Text))) return false;

            if (string.IsNullOrWhiteSpace(txtPaymentAgencyKana.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "決済代行会社名カナ");
                txtPaymentAgencyKana.Select();
                return false;
            }
            if (!txtBankCode.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblBankCode.Text))) return false;
            if (!txtBankName.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblBankName.Text))) return false;
            if (!txtBranchCode.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblBranchCode.Text))) return false;
            if (!txtBranchName.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblBranchName.Text))) return false;

            if (string.IsNullOrWhiteSpace(cmbAccountType.Text))
            {
                ShowWarningDialog(MsgWngSelectionRequired, "預金種別");
                cmbAccountType.Select();
                return false;
            }
            if (!txtAccountNumber.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblAccountNumber.Text))) return false;

            if (string.IsNullOrWhiteSpace(cmbShareTransferFee.Text))
            {
                ShowWarningDialog(MsgWngSelectionRequired, "手数料負担区分");
                cmbShareTransferFee.Select();
                return false;
            }
            if (!nmbDueDateOffset.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "入金予定日");
                nmbDueDateOffset.Select();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbFileFormat.Text))
            {
                ShowWarningDialog(MsgWngSelectionRequired, "口座振替データフォーマット");
                cmbFileFormat.Select();
                return false;
            }
            if (cbxConsiderUncollected.Checked
                && cmbConsiderUncollected.SelectedIndex == -1)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "回収区分");
                cmbConsiderUncollected.Select();
                return false;
            }

            if (!txtOutputFileName.ValidateInputted(()
                => ShowWarningDialog(MsgWngInputRequired, lblOutputFileName.Text))) return false;

            if (!Util.ValidateFileName(txtOutputFileName.Text))
            {
                ShowWarningDialog(MsgWngInvalidCharacterAtWriteFile);
                txtOutputFileName.Focus();
                return false;
            }

                return true;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(0, txtPaymentAgencyCode.TextLength, txtPaymentAgencyCode.MaxLength))
            {
                txtPaymentAgencyCode.Text = ZeroLeftPadding(txtPaymentAgencyCode);
                txtPaymentAgencyCode_Validated(null, null);
            }
            if (IsNeedValidate(0, txtBankCode.TextLength, txtBankCode.MaxLength))
            {
                txtBankCode.Text = ZeroLeftPadding(txtBankCode);
            }
            if (IsNeedValidate(0, txtBranchCode.TextLength, txtBranchCode.MaxLength))
            {
                txtBranchCode.Text = ZeroLeftPadding(txtBranchCode);
            }
            if (IsNeedValidate(0, txtAccountNumber.TextLength, txtAccountNumber.MaxLength))
            {
                txtAccountNumber.Text = ZeroLeftPadding(txtAccountNumber);
            }
            txtPaymentAgencyKana_Validated(null, null);
            txtBankName_Validated(null, null);
            txtBranchName_Validated(null, null);
        }

        private void cmbShareTransferFee_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (cmbShareTransferFee.Text == "自社" && PaymentAgencyId != 0)
            {
                cbxUseFeeTolerance.Enabled = true;
                cbxUseFeeLearning.Enabled = true;
                BaseContext.SetFunction08Enabled(true);
            }
            else if (cmbShareTransferFee.Text == "自社")
            {
                cbxUseFeeTolerance.Enabled = true;
                cbxUseFeeLearning.Enabled = true;
            }
            else
            {
                cbxUseFeeTolerance.Checked = false;
                cbxUseFeeLearning.Checked = false;

                cbxUseFeeTolerance.Enabled = false;
                cbxUseFeeLearning.Enabled = false;

                BaseContext.SetFunction08Enabled(false);
            }
        }

        private void cbxConsiderUncollected_CheckedChanged(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (cbxConsiderUncollected.Checked)
            {
                cmbConsiderUncollected.Enabled = true;
            }
            else
            {
                cmbConsiderUncollected.Enabled = false;
                cmbConsiderUncollected.SelectedIndex = -1;
            }
        }

        private void txtPaymentAgencyKana_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                var converted = txtPaymentAgencyKana.Text;

                if (!string.IsNullOrEmpty(converted))
                {
                    converted = EbDataHelper.ConvertToPayerName(converted, LegalPersonalities);
                }
                txtPaymentAgencyKana.Text = converted;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtBankName_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var converted = txtBankName.Text;

            if (!string.IsNullOrEmpty(converted))
            {
                converted = EbDataHelper.RemoveProhibitCharacter(converted);
            }
            txtBankName.Text = converted;
        }

        private void txtBranchName_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var converted = txtBranchName.Text;

            if (!string.IsNullOrEmpty(converted))
            {
                converted = EbDataHelper.RemoveProhibitCharacter(converted);
            }
            txtBranchName.Text = converted;
        }

        private void txtPaymentAgencyCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (txtPaymentAgencyCode.Text != "")
                {
                    Task<PaymentAgency> task = GetPaymentAgencyDataByCode();
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (task.Result != null)
                    {
                        FillData(task.Result);
                        txtPaymentAgencyCode.Enabled = false;
                        Modified = false;
                    }
                    else
                    {
                        BaseContext.SetFunction03Enabled(false);
                        DispStatusMessage(MsgInfNewData, "決済代行会社コード");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<PaymentAgency> GetPaymentAgencyDataByCode()
        {
            PaymentAgency paymentAgencyByCode = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var paymentAgencyCode = new string[] { txtPaymentAgencyCode.Text };

                var service = factory.Create<PaymentAgencyMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, paymentAgencyCode);
                if (result.ProcessResult.Result)
                {
                    paymentAgencyByCode = result.PaymentAgencies.FirstOrDefault();
                }
            });
            return paymentAgencyByCode;
        }

        private void FillData(PaymentAgency paymentAgency)
        {
            PaymentAgencyId = paymentAgency.Id;
            UpdateAt = paymentAgency.UpdateAt;
            txtPaymentAgencyCode.Text = paymentAgency.Code;
            txtConsigneeCode.Text = paymentAgency.ConsigneeCode;
            txtPaymentAgencyName.Text = paymentAgency.Name;
            txtPaymentAgencyKana.Text = paymentAgency.Kana;
            txtBankCode.Text = paymentAgency.BankCode;
            txtBankName.Text = paymentAgency.BankName;
            txtBranchCode.Text = paymentAgency.BranchCode;
            txtBranchName.Text = paymentAgency.BranchName;
            if (paymentAgency.AccountTypeId == 1)
            {
                cmbAccountType.SelectedIndex = 0;
            }
            else if (paymentAgency.AccountTypeId == 2)
            {
                cmbAccountType.SelectedIndex = 1;
            }
            else if (paymentAgency.AccountTypeId == 3)
            {
                cmbAccountType.SelectedIndex = 2;
            }
            else if (paymentAgency.AccountTypeId == 9)
            {
                cmbAccountType.SelectedIndex = 3;
            }
            else
            {
                cmbAccountType.SelectedIndex = -1;
            }
            txtAccountNumber.Text = paymentAgency.AccountNumber;
            cmbShareTransferFee.SelectedIndex = paymentAgency.ShareTransferFee;

            cbxUseFeeTolerance.Checked = (paymentAgency.UseFeeTolerance == 1);
            cbxUseFeeLearning.Checked = (paymentAgency.UseFeeLearning == 1);
            cbxUseKanaLearning.Checked = (paymentAgency.UseKanaLearning == 1);
            cbxConsiderUncollected.Checked = (paymentAgency.ConsiderUncollected == 1);
            txtOutputFileName.Text = paymentAgency.OutputFileName;
            cbxAppendDate.Checked = (paymentAgency.AppendDate == 1);

            nmbDueDateOffset.Text = paymentAgency.DueDateOffset.ToString();

            var selectId = PaymentFileFormatList.Where(x => x.Id == (paymentAgency.FileFormatId));
            cmbFileFormat.SelectedValue = selectId.Select(s => s.Name).First();
            txtContractCode.Text = paymentAgency.ContractCode;

            if (paymentAgency.ConsiderUncollected == 1)
            {
                var cateogrySelectId = CollectCategoryList.Where(x => x.Id == (paymentAgency.CollectCategoryId));
                if (cateogrySelectId != null)
                {
                    cmbConsiderUncollected.SelectedValue = cateogrySelectId.Select(s => s.Name).First();
                }
            }

            if (PaymentAgencyCode == null)
            {
                BaseContext.SetFunction03Enabled(true);
            }

            if (cmbShareTransferFee.Text == "自社")
            {
                BaseContext.SetFunction08Enabled(true);
            }
        }
        #endregion

        private void cmbFileFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFileFormat.SelectedIndex != -1
                && Convert.ToInt32(cmbFileFormat.SelectedItem.SubItems[1].Value) == (int)AccountTransferFileFormatId.InternetJPBankFixed)
            {
                lblContractCode.Enabled = true;
                txtContractCode.Enabled = true;
            }
            else
            {
                lblContractCode.Enabled = false;
                txtContractCode.Enabled = false;
                txtContractCode.Clear();
            }

        }
    }
}
