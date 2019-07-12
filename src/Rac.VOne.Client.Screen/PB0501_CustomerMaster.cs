using GrapeCity.Win.Editors;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.AccountTitleMasterService;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerDiscountMasterService;
using Rac.VOne.Client.Screen.CustomerFeeMasterService;
using Rac.VOne.Client.Screen.CustomerGroupMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.Extensions;
using Rac.VOne.Client.Screen.Importer;
using Rac.VOne.Client.Screen.ImportSettingMasterService;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.KanaHistoryCustomerMasterService;
using Rac.VOne.Client.Screen.NettingService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common;
using Rac.VOne.Import;
using Rac.VOne.Message;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Web.Models.FunctionType;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>得意先マスター</summary>
    public partial class PB0501 : VOneScreenBase
    {
        private string CategoryIndex { get; set; }
        private string LessThanCategoryIndex { get; set; }
        private string GreaterThanCategoryIndex1 { get; set; }
        private string GreaterThanCategoryIndex2 { get; set; }
        private string GreaterThanCategoryIndex3 { get; set; }
        private int CustomerId { get; set; }
        private int SaveCustomerId { get; set; }
        private int? DepartmentId1 { get; set; }
        private int? DepartmentId2 { get; set; }
        private int? DepartmentId3 { get; set; }
        private int? DepartmentId4 { get; set; }
        private int? DepartmentId5 { get; set; }
        private int? AccountTitleId1 { get; set; }
        private int? AccountTitleId2 { get; set; }
        private int? AccountTitleId3 { get; set; }
        private int? AccountTitleId4 { get; set; }
        private int? AccountTitleId5 { get; set; }
        private int StaffId { get; set; }
        private DateTime UpdateAtCustomer { get; set; }
        private DateTime UpdateAtCusPayment { get; set; }
        private DateTime UpdateAtCusDiscount1 { get; set; }
        private DateTime UpdateAtCusDiscount2 { get; set; }
        private DateTime UpdateAtCusDiscount3 { get; set; }
        private DateTime UpdateAtCusDiscount4 { get; set; }
        private DateTime UpdateAtCusDiscount5 { get; set; }
        private string SelectedCodeCategoryId { get; set; }
        private int UseLimitDateCategoryId { get; set; }
        private int UseLimitDateCategoryGreatherId1 { get; set; }
        private int UseLimitDateCategoryGreatherId2 { get; set; }
        private int UseLimitDateCategoryGreatherId3 { get; set; }
        private Department DepartmentResult { get; set; } = null;
        private AccountTitle AccountTitleResult { get; set; } = null;
        public string CustomerCode { get; set; } = null;
        public VOneScreenBase ReturnScreen { get; set; }
        private bool ChangeFee { get; set; } = false;
        /// <summary>
        /// メニュー権限があるかどうか
        /// </summary>
        private bool MenuAuthority { get; set; }
        private List<Category> CollectCategoryList { get; set; }
        private List<Category> LessThanCollectCategoryList { get; set; }
        private List<Category> GreatherThanCollectCategoryList { get; set; }
        private Customer LoadedCustomer { get; set; }
        private IEnumerable<string> LegalPersonalities { get; set; }
        private string PatternNo { get; set; }
        public PB0501()
        {
            InitializeComponent();
            Text = "得意先マスター";
        }

        #region InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmToClear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction04Caption("印刷指示");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = DoPrint;

            BaseContext.SetFunction05Caption("インポート");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Caption("取込設定");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = OpenImporterSetting;

            BaseContext.SetFunction08Caption("登録手数料");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = OpenRegistrationFee;

            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PB0501_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var tasks = new List<Task>();
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadComboDataLessThanCollectCategoryInfoAsync());
                tasks.Add(LoadComboDataGreatherThanCollectCategoryInfoAsync());
                tasks.Add(LoadCollectCategoryInfoAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadFunctionAuthorities(MasterImport, MasterExport));
                tasks.Add(LoadMenuAuthorityListAsync());
                tasks.Add(LoadLegalPersonalitiesAsync());
                if (!string.IsNullOrEmpty(CustomerCode))
                    tasks.Add(LoadCustomerAsync(CustomerCode));
                //tasks.Add(LoadAllCustomersAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                if (!UseDiscount)
                {
                    tbcMain.TabPages.Remove(tbpDiscount);
                }

                SetOptionControls();
                SetFormat();
                ComboDataBind();
                SetHonorificCombo();
                AddHandlers();
                SetComboForCollectCategoryId();
                SetComboForLessThanCollectCategoryId();
                SetComboForAllGreaterThanCollectCategoryId();
                Clear();

                if (CustomerCode != null && LoadedCustomer != null)
                {
                    var customer = LoadedCustomer; // CustomerList.Where(x => x.Code == CustomerCode).Select(x => x).FirstOrDefault();
                    ProgressDialog.Start(ParentForm, SetCustomerData(customer), false, SessionKey);
                    SetControlsCallingByOtherProcess();
                    btnCustomerCode.Enabled = false;
                }

                this.ActiveControl = CustomerCode == null ? txtCustomerCode : txtCustomerName;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region Set Data in Form
        /// <summary> DBからCustomerデータを項目に設定 </summary>
        /// <param name="customer"> DBから取得したデータのModel</param>
        private async Task SetCustomerData(Customer customer)
        {
            if (customer == null) return;
            var parentHasChildren = false;
            if (customer.IsParent == 1)
            {
                parentHasChildren = await HasChildCustomer(customer.Id);
            }

            var exclusiveAccountNumber = customer.ExclusiveAccountNumber.ToString();
            CustomerId = customer.Id;
            StaffId = customer.StaffId;

            txtCustomerCode.Text = customer.Code;
            txtCustomerName.Text = customer.Name;
            txtCustomerKana.Text = customer.Kana;
            if (customer.ParentCode != null)
            {
                txtParentCustomerCode.Text = customer.ParentCode;
                txtParentCustomerName.Text = customer.ParentName;
                cbxCustomerIsParent.Enabled = false;
            }
            else
            {
                txtParentCustomerCode.Clear();
                txtParentCustomerName.Clear();
                cbxCustomerIsParent.Enabled = !parentHasChildren;
            }
            txtStaffCode.Text = customer.StaffCode;
            lblStaffName.Text = customer.StaffName;
            mskPostalCode.Text = customer.PostalCode;
            txtAddress1.Text = customer.Address1;
            txtAddress2.Text = customer.Address2;
            txtCustomerTel.Text = customer.Tel;
            txtCustomerFax.Text = customer.Fax;
            txtTransferAccountNumber.Text = customer.TransferAccountNumber;
            txtExclusiveBankCode.Text = customer.ExclusiveBankCode;
            txtExclusiveBankName.Text = customer.ExclusiveBankName;
            txtExclusiveBranchCode.Text = customer.ExclusiveBranchCode;
            txtExclusiveBranchName.Text = customer.ExclusiveBranchName;
            if (exclusiveAccountNumber != null && exclusiveAccountNumber.Length == 10)
            {
                txtExclusiveAccountNumber.Text = exclusiveAccountNumber.Substring(0, 3);
                txtAccountNumber.Text = exclusiveAccountNumber.Substring(exclusiveAccountNumber.Length - 7);
            }
            else
            {
                txtExclusiveAccountNumber.Clear();
                txtAccountNumber.Clear();
            }
            txtDestinationDepartmentName.Text = customer.DestinationDepartmentName;
            txtCustomerStaffName.Text = customer.CustomerStaffName;
            cmbHonorific.Text = customer.Honorific;
            txtTransferBrachCode.Text = customer.TransferBranchCode;
            txtTransferBranchName.Text = customer.TransferBranchName;
            if (customer.ExclusiveAccountTypeId != null)
            {
                int? accountType = customer.ExclusiveAccountTypeId;
                switch (accountType)
                {
                    case 1:
                        cmbExclusiveAccountTypeId.SelectedIndex = 0;
                        break;

                    case 2:
                        cmbExclusiveAccountTypeId.SelectedIndex = 1;
                        break;

                    case 4:
                        cmbExclusiveAccountTypeId.SelectedIndex = 2;
                        break;

                    case 5:
                        cmbExclusiveAccountTypeId.SelectedIndex = 3;
                        break;
                }
            }
            else
            {
                cmbExclusiveAccountTypeId.SelectedIndex = -1;
            }
            cmbShareTransferFee.SelectedIndex = customer.ShareTransferFee;
            if (customer.CreditLimit != 0M)
                nmbCreditLimit.Value = customer.CreditLimit;
            else
                nmbCreditLimit.Clear();

            var issueBillEachTime = (customer.ClosingDay == 0); // 都度請求
            cbxIssueBillEachTime.Checked = issueBillEachTime;
            txtClosingDay.Text = GetFormattedClosingDay(customer.ClosingDay.ToString());
            cmbCollectCategoryId.SelectedValue = customer.CollectCategoryCode.ToString() + ":" + customer.CollectCategoryName.ToString();
            txtCollectOffsetMonth.Text = customer.CollectOffsetMonth.ToString();
            txtCollectOffsetDay.Text = GetFormattedClosingDay(customer.CollectOffsetDay.ToString(), !(issueBillEachTime));
            cbxCustomerIsParent.Checked = customer.IsParent == 1;
            txtNote.Text = customer.Note;
            if (customer.SightOfBill != null)
                txtSightOfBill.Text = customer.SightOfBill.ToString();
            txtDensaiCode.Text = customer.DensaiCode;
            txtCreditCode.Text = customer.CreditCode;
            txtCreditRank.Text = customer.CreditRank;
            txtTransferBankCode.Text = customer.TransferBankCode;
            txtTransferBankName.Text = customer.TransferBankName;

            if (customer.TransferAccountTypeId.HasValue)
            {
                var accountType = customer.TransferAccountTypeId.Value;
                switch (accountType)
                {
                    case 1:
                        cmbTransferAccountTypeId.SelectedIndex = 0;
                        break;

                    case 2:
                        cmbTransferAccountTypeId.SelectedIndex = 1;
                        break;

                    case 3:
                        cmbTransferAccountTypeId.SelectedIndex = 2;
                        break;

                    case 9:
                        cmbTransferAccountTypeId.SelectedIndex = 3;
                        break;
                }
            }
            else
            {
                cmbTransferAccountTypeId.SelectedIndex = -1;
            }
            txtTransferNewCode.Text = customer.TransferNewCode;
            txtTransferAccountName.Text = customer.TransferAccountName;
            txtTransferCustomerCode.Text = customer.TransferCustomerCode;
            cbxReceiveAccountId1.Checked = customer.ReceiveAccountId1 == 1;
            cbxReceiveAccountId2.Checked = customer.ReceiveAccountId2 == 1;
            cbxReceiveAccountId3.Checked = customer.ReceiveAccountId3 == 1;
            cbxUseFeeLearning.Checked = customer.UseFeeLearning == 1;
            cbxUseKanaLearning.Checked = customer.UseKanaLearning == 1;
            cmbHolidayFlag.SelectedIndex = customer.HolidayFlag;
            cbxUseFeeTolerance.Checked = customer.UseFeeTolerance == 1;
            cbxPrioritizeMatchingIndividually.Checked = customer.PrioritizeMatchingIndividually == 1;
            cbxExcludeInvoicePublish.Checked = customer.ExcludeInvoicePublish == 1;
            cbxExcludeReminderPublish.Checked = customer.ExcludeReminderPublish == 1;
            txtCollationKey.Text = customer.CollationKey;
            UpdateAtCustomer = customer.UpdateAt;

            await SetCustomerPaymentContract();
            await SetCustomerDiscount();

            txtCustomerCode.Enabled = false;
            if (CustomerCode == null && !(ReturnScreen is PE0102))
            {
                BaseContext.SetFunction03Enabled(true);
            }

            Modified = false;
            ClearStatusMessage();
        }

        /// <summary> DBからCustomerPaymentContractデータを項目に設定 </summary>
        private async Task SetCustomerPaymentContract()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomerPaymentContractsResult result = await service.GetPaymentContractAsync(SessionKey, new int[] { CustomerId });

                if (result.ProcessResult.Result)
                {
                    List<CustomerPaymentContract> paymentResult = result.Payments;
                    if (paymentResult.Any())
                    {
                        CustomerPaymentContract payment = paymentResult[0];
                        nmbThresholdValue.Value = payment.ThresholdValue;
                        cmbLessThanCollectCategoryId.SelectedValue = payment.LessThanCode.ToString() + ":" + payment.LessThanName.ToString();
                        cmbGreaterThanCollectCategoryId1.SelectedValue = payment.GreaterThanCode1.ToString() + ":" + payment.GreaterThanName1.ToString();
                        cmbGreaterThanCollectCategoryId1.Enabled = true;
                        nmbGreaterThanRate1.Value = payment.GreaterThanRate1;
                        cmbGreaterThanRoundingMode1.SelectedIndex = payment.GreaterThanRoundingMode1 ?? -1;
                        if (payment.GreaterThanSightOfBill1 != 0)
                        {
                            txtGreaterThanSightOfBill1.Text = payment.GreaterThanSightOfBill1.ToString();
                        }

                        var value1 = payment.GreaterThanRate1.ToString().Split('.');
                        if (int.Parse(value1[0]) != 100)
                        {
                            if (payment.GreaterThanCode2 != null)
                            {
                                cmbGreaterThanCollectCategoryId2.SelectedValue = payment.GreaterThanCode2.ToString() + ":" + payment.GreaterThanName2.ToString();
                            }
                            cmbGreaterThanCollectCategoryId2.Enabled = true;
                            nmbGreaterThanRate2.Value = payment.GreaterThanRate2;
                            cmbGreaterThanRoundingMode2.SelectedIndex = payment.GreaterThanRoundingMode2 ?? -1;
                            if (payment.GreaterThanSightOfBill2 != 0)
                                txtGreaterThanSightOfBill2.Text = payment.GreaterThanSightOfBill2.ToString();
                        }

                        if (payment.GreaterThanRate2 != null)
                        {
                            var value2 = payment.GreaterThanRate2.ToString().Split('.');
                            if (int.Parse(value1[0]) + int.Parse(value2[0]) != 100)
                            {
                                if (payment.GreaterThanCode3 != null)
                                {
                                    cmbGreaterThanCollectCategoryId3.SelectedValue = payment.GreaterThanCode3.ToString() + ":" + payment.GreaterThanName3.ToString();
                                    cmbGreaterThanCollectCategoryId3.Enabled = true;
                                    nmbGreaterThanRate3.Value = payment.GreaterThanRate3;
                                    cmbGreaterThanRoundingMode3.SelectedIndex = payment.GreaterThanRoundingMode3 ?? -1;
                                }
                                if (payment.GreaterThanSightOfBill3 != 0)
                                    txtGreaterThanSightOfBill3.Text = payment.GreaterThanSightOfBill3.ToString();
                            }
                        }
                        UpdateAtCusPayment = payment.UpdateAt;
                        Modified = false;
                    }
                }
            });
        }

        /// <summary> DBからCustomerDiscountデータを項目に設定 </summary>
        private async Task SetCustomerDiscount()
        {
            var result = await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client)
                => await client.GetDiscountAsync(SessionKey, CustomerId));
            if (!result.ProcessResult.Result) return;
            ClearCustomerDiscountItems();
            List<CustomerDiscount> discountResult = result.CustomerDiscounts;
            if (discountResult.Any())
            {
                for (int i = 0; i < discountResult.Count; i++)
                {
                    CustomerDiscount discount = discountResult[i];
                    nmbMinValue.Value = discount.MinValue;

                    if (discount.Sequence == 1)
                    {
                        nmbRate.Value = discount.Rate;
                        cmbRoundingMode.SelectedIndex = discount.RoundingMode;
                        txtDepartmentId.Text = discount.DepartmentCode;
                        lblDepartmentName.Text = discount.DepartmentName;
                        txtAccountTitleId.Text = discount.AccountTitleCode;
                        lblAccountTitleName.Text = discount.AccountTitleName;
                        txtSubCode.Text = discount.SubCode;
                        DepartmentId1 = discount.DepartmentId;
                        AccountTitleId1 = discount.AccountTitleId;
                        UpdateAtCusDiscount1 = discount.UpdateAt;
                    }
                    else if (discount.Sequence == 2)
                    {
                        nmbRate2.Value = discount.Rate;
                        cmbRoundingMode2.SelectedIndex = discount.RoundingMode;
                        txtDepartmentId2.Text = discount.DepartmentCode;
                        lblDepartmentName2.Text = discount.DepartmentName;
                        txtAccountTitleId2.Text = discount.AccountTitleCode;
                        lblAccountTitleName2.Text = discount.AccountTitleName;
                        txtSubCode2.Text = discount.SubCode;
                        DepartmentId2 = discount.DepartmentId;
                        AccountTitleId2 = discount.AccountTitleId;
                        UpdateAtCusDiscount2 = discount.UpdateAt;
                    }
                    else if (discount.Sequence == 3)
                    {
                        nmbRate3.Value = discount.Rate;
                        cmbRoundingMode3.SelectedIndex = discount.RoundingMode;
                        txtDepartmentId3.Text = discount.DepartmentCode;
                        lblDepartmentName3.Text = discount.DepartmentName;
                        txtAccountTitleId3.Text = discount.AccountTitleCode;
                        lblAccountTitleName3.Text = discount.AccountTitleName;
                        txtSubCode3.Text = discount.SubCode;
                        DepartmentId3 = discount.DepartmentId;
                        AccountTitleId3 = discount.AccountTitleId;
                        UpdateAtCusDiscount3 = discount.UpdateAt;
                    }
                    else if (discount.Sequence == 4)
                    {
                        nmbRate4.Value = discount.Rate;
                        cmbRoundingMode4.SelectedIndex = discount.RoundingMode;
                        txtDepartmentId4.Text = discount.DepartmentCode;
                        lblDepartmentName4.Text = discount.DepartmentName;
                        txtAccountTitleId4.Text = discount.AccountTitleCode;
                        lblAccountTitleName4.Text = discount.AccountTitleName;
                        txtSubCode4.Text = discount.SubCode;
                        DepartmentId4 = discount.DepartmentId;
                        AccountTitleId4 = discount.AccountTitleId;
                        UpdateAtCusDiscount4 = discount.UpdateAt;
                    }
                    else
                    {
                        nmbRate5.Value = discount.Rate;
                        cmbRoundingMode5.SelectedIndex = discount.RoundingMode;
                        txtDepartmentId5.Text = discount.DepartmentCode;
                        lblDepartmentName5.Text = discount.DepartmentName;
                        txtAccountTitleId5.Text = discount.AccountTitleCode;
                        lblAccountTitleName5.Text = discount.AccountTitleName;
                        txtSubCode5.Text = discount.SubCode;
                        DepartmentId5 = discount.DepartmentId;
                        AccountTitleId5 = discount.AccountTitleId;
                        UpdateAtCusDiscount5 = discount.UpdateAt;
                    }
                    Modified = false;
                }
            }
        }

        /// <summary> 営業担当者コードで取得したデータを設定する </summary>
        private void SetStaffCodeVal()
        {
            Staff staffResult = null;
            string staffCode = txtStaffCode.Text;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { staffCode });

                if (result.ProcessResult.Result)
                {
                    staffResult = result.Staffs.FirstOrDefault();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (staffResult != null)
            {
                lblStaffName.Text = staffResult.Name;
                StaffId = staffResult.Id;
                ClearStatusMessage();
            }
            else
            {
                ClearStatusMessage();
                ShowWarningDialog(MsgWngMasterNotExist, "営業担当者", txtStaffCode.Text);
                lblStaffName.Clear();
                txtStaffCode.Clear();
                txtStaffCode.Focus();
            }
        }
        #endregion

        #region ComboData Binding
        /// <summary> 約定金額未満コンボでデータを設定 </summary>
        private void SetComboForLessThanCollectCategoryId()
        {
            if (LessThanCollectCategoryList != null)
            {
                for (int i = 0; i < LessThanCollectCategoryList.Count; i++)
                {
                    int index = cmbLessThanCollectCategoryId.Items.Add(new ListItem(LessThanCollectCategoryList[i].Code.ToString() + ":" + LessThanCollectCategoryList[i].Name.ToString(), LessThanCollectCategoryList[i].Id));
                    cmbLessThanCollectCategoryId.Items[i].Tag = LessThanCollectCategoryList[i];
                }
            }
        }

        /// <summary> 約定金額以上①と約定金額以上②と約定金額以上③コンボでデータを設定 </summary>
        private void SetComboForAllGreaterThanCollectCategoryId()
        {
            if (GreatherThanCollectCategoryList != null)
            {
                for (int i = 0; i < GreatherThanCollectCategoryList.Count; i++)
                {
                    int index1 = cmbGreaterThanCollectCategoryId1.Items.Add(new ListItem(GreatherThanCollectCategoryList[i].Code.ToString() + ":" + GreatherThanCollectCategoryList[i].Name.ToString(), GreatherThanCollectCategoryList[i].Id));
                    cmbGreaterThanCollectCategoryId1.Items[i].Tag = GreatherThanCollectCategoryList[i];

                    int index2 = cmbGreaterThanCollectCategoryId2.Items.Add(new ListItem(GreatherThanCollectCategoryList[i].Code.ToString() + ":" + GreatherThanCollectCategoryList[i].Name.ToString(), GreatherThanCollectCategoryList[i].Id));
                    cmbGreaterThanCollectCategoryId2.Items[i].Tag = GreatherThanCollectCategoryList[i];

                    int index3 = cmbGreaterThanCollectCategoryId3.Items.Add(new ListItem(GreatherThanCollectCategoryList[i].Code.ToString() + ":" + GreatherThanCollectCategoryList[i].Name.ToString(), GreatherThanCollectCategoryList[i].Id));
                    cmbGreaterThanCollectCategoryId3.Items[i].Tag = GreatherThanCollectCategoryList[i];
                }
            }
        }

        /// <summary> 回収方法コンボでデータを設定 </summary>
        private void SetComboForCollectCategoryId()
        {
            if (CollectCategoryList != null)
            {
                for (int i = 0; i < CollectCategoryList.Count; i++)
                {
                    int index = cmbCollectCategoryId.Items.Add(new ListItem(CollectCategoryList[i].Code.ToString() + ":" + CollectCategoryList[i].Name.ToString(), CollectCategoryList[i].Id));
                    cmbCollectCategoryId.Items[i].Tag = CollectCategoryList[i];
                }
            }
        }

        private void ComboDataBind()
        {
            // for cmbShareTransferFee
            cmbShareTransferFee.Items.Add(new ListItem("相手先", 0));
            cmbShareTransferFee.Items.Add(new ListItem("自社", 1));

            // for cmbHolidayFlag
            cmbHolidayFlag.Items.Add(new ListItem("考慮しない", 0));
            cmbHolidayFlag.Items.Add(new ListItem("休業日の前", 1));
            cmbHolidayFlag.Items.Add(new ListItem("休業日の後", 2));
            cmbHolidayFlag.SelectedIndex = 0;

            // for comboGreaterThanRoundingMode1
            cmbGreaterThanRoundingMode1.Items.Add(new ListItem("端数", 0));
            cmbGreaterThanRoundingMode1.Items.Add(new ListItem("一", 1));
            cmbGreaterThanRoundingMode1.Items.Add(new ListItem("十", 2));
            cmbGreaterThanRoundingMode1.Items.Add(new ListItem("百", 3));
            cmbGreaterThanRoundingMode1.Items.Add(new ListItem("千", 4));
            cmbGreaterThanRoundingMode1.Items.Add(new ListItem("万", 5));
            cmbGreaterThanRoundingMode1.Items.Add(new ListItem("十万", 6));

            // for comboGreaterThanRoundingMode2
            cmbGreaterThanRoundingMode2.Items.Add(new ListItem("端数", 0));
            cmbGreaterThanRoundingMode2.Items.Add(new ListItem("一", 1));
            cmbGreaterThanRoundingMode2.Items.Add(new ListItem("十", 2));
            cmbGreaterThanRoundingMode2.Items.Add(new ListItem("百", 3));
            cmbGreaterThanRoundingMode2.Items.Add(new ListItem("千", 4));
            cmbGreaterThanRoundingMode2.Items.Add(new ListItem("万", 5));
            cmbGreaterThanRoundingMode2.Items.Add(new ListItem("十万", 6));

            // for comboGreaterThanRoundingMode3
            cmbGreaterThanRoundingMode3.Items.Add(new ListItem("端数", 0));
            cmbGreaterThanRoundingMode3.Items.Add(new ListItem("一", 1));
            cmbGreaterThanRoundingMode3.Items.Add(new ListItem("十", 2));
            cmbGreaterThanRoundingMode3.Items.Add(new ListItem("百", 3));
            cmbGreaterThanRoundingMode3.Items.Add(new ListItem("千", 4));
            cmbGreaterThanRoundingMode3.Items.Add(new ListItem("万", 5));
            cmbGreaterThanRoundingMode3.Items.Add(new ListItem("十万", 6));

            //for cmbExclusiveAccountTypeId
            cmbExclusiveAccountTypeId.Items.Add(new ListItem("普通", 1));
            cmbExclusiveAccountTypeId.Items.Add(new ListItem("当座", 2));
            cmbExclusiveAccountTypeId.Items.Add(new ListItem("貯蓄", 4));
            cmbExclusiveAccountTypeId.Items.Add(new ListItem("通知", 5));

            //for cmbTransferAccountTypeId
            cmbTransferAccountTypeId.Items.Add(new ListItem("普通", 1));
            cmbTransferAccountTypeId.Items.Add(new ListItem("当座", 2));
            cmbTransferAccountTypeId.Items.Add(new ListItem("納税準備", 3));
            cmbTransferAccountTypeId.Items.Add(new ListItem("その他", 9));

            // for cmbRoundingMode
            cmbRoundingMode.Items.Add(new ListItem("切捨", 0));
            cmbRoundingMode.Items.Add(new ListItem("切上", 1));
            cmbRoundingMode.Items.Add(new ListItem("四捨五入", 2));
            cmbRoundingMode.Items.Add(new ListItem("銀行丸め", 3));

            // for cmbRoundingMode2
            cmbRoundingMode2.Items.Add(new ListItem("切捨", 0));
            cmbRoundingMode2.Items.Add(new ListItem("切上", 1));
            cmbRoundingMode2.Items.Add(new ListItem("四捨五入", 2));
            cmbRoundingMode2.Items.Add(new ListItem("銀行丸め", 3));

            // for cmbRoundingMode3
            cmbRoundingMode3.Items.Add(new ListItem("切捨", 0));
            cmbRoundingMode3.Items.Add(new ListItem("切上", 1));
            cmbRoundingMode3.Items.Add(new ListItem("四捨五入", 2));
            cmbRoundingMode3.Items.Add(new ListItem("銀行丸め", 3));

            // for cmbRoundingMode
            cmbRoundingMode4.Items.Add(new ListItem("切捨", 0));
            cmbRoundingMode4.Items.Add(new ListItem("切上", 1));
            cmbRoundingMode4.Items.Add(new ListItem("四捨五入", 2));
            cmbRoundingMode4.Items.Add(new ListItem("銀行丸め", 3));

            // for cmbRoundingMode5
            cmbRoundingMode5.Items.Add(new ListItem("切捨", 0));
            cmbRoundingMode5.Items.Add(new ListItem("切上", 1));
            cmbRoundingMode5.Items.Add(new ListItem("四捨五入", 2));
            cmbRoundingMode5.Items.Add(new ListItem("銀行丸め", 3));
        }

        private void SetHonorificCombo()
        {
            cmbHonorific.MaxLength = 6;
            cmbHonorific.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Char;
            cmbHonorific.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cmbHonorific.DropDownStyle = ComboBoxStyle.DropDown;
            cmbHonorific.Items.Add("御中");
            cmbHonorific.Items.Add("様");
            cmbHonorific.Items.Add("先生");
            cmbHonorific.AcceptsTabChar = GrapeCity.Win.Editors.TabCharMode.Filter;
            cmbHonorific.AcceptsCrLf = GrapeCity.Win.Editors.CrLfMode.Filter;
        }

        #endregion

        #region 登録処理
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!CheckData())
                    return;

                ZeroLeftPaddingWithoutValidated();

                if (!CheckDivisionSum())
                    return;
                if (!CheckFractionalUnit())
                    return;
                if (UseDiscount)
                {
                    if (!CheckRoundingMode())
                        return;
                    if (!CheckDiscountRate())
                        return;
                }
                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                Task<bool> task = SaveCustomer();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    Clear();
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool CheckData()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "得意先コード");
                txtCustomerCode.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtCustomerName.Text.Trim()))
            {
                ShowWarningDialog(MsgWngInputRequired, "得意先名");
                txtCustomerName.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtCustomerKana.Text.Trim()))
            {
                ShowWarningDialog(MsgWngInputRequired, "得意先名カナ");
                txtCustomerKana.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cmbShareTransferFee.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "手数料負担区分");
                cmbShareTransferFee.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtStaffCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "営業担当者");
                txtStaffCode.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtClosingDay.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "締日");
                txtClosingDay.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtCollectOffsetMonth.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "回収予定（月、日）");
                txtCollectOffsetMonth.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtCollectOffsetDay.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "回収予定（月、日）");
                txtCollectOffsetDay.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cmbCollectCategoryId.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "回収方法");
                cmbCollectCategoryId.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtSightOfBill.Text) && UseLimitDateCategoryId == 1)
            {
                ShowWarningDialog(MsgWngInputRequired, "回収サイト");
                txtSightOfBill.Focus();
                return false;
            }
            else if (nmbThresholdValue.Value == null && cmbCollectCategoryId.Text == "00:約定")
            {
                ShowWarningDialog(MsgWngInputRequired, "約定金額");
                nmbThresholdValue.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cmbLessThanCollectCategoryId.Text) && SelectedCodeCategoryId == "00")
            {
                ShowWarningDialog(MsgWngInputRequired, "約定金額未満");
                cmbLessThanCollectCategoryId.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cmbGreaterThanCollectCategoryId1.Text) && SelectedCodeCategoryId == "00")
            {
                ShowWarningDialog(MsgWngInputRequired, "約定金額以上①");
                cmbGreaterThanCollectCategoryId1.Focus();
                return false;
            }
            else if (cmbGreaterThanCollectCategoryId1.Text != "" && nmbGreaterThanRate1.Value == null)
            {
                ShowWarningDialog(MsgWngInputRequired, "分割①");
                nmbGreaterThanRate1.Focus();
                return false;
            }
            else if (cmbGreaterThanCollectCategoryId1.Text != "" && string.IsNullOrWhiteSpace(cmbGreaterThanRoundingMode1.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "端数単位");
                cmbGreaterThanRoundingMode1.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtGreaterThanSightOfBill1.Text) && UseLimitDateCategoryGreatherId1 == 1 && txtGreaterThanSightOfBill1.Enabled)
            {
                ShowWarningDialog(MsgWngInputRequired, "回収サイト");
                txtGreaterThanSightOfBill1.Focus();
                return false;
            }
            else if (cmbGreaterThanCollectCategoryId2.Enabled != false && string.IsNullOrWhiteSpace(cmbGreaterThanCollectCategoryId2.Text) && SelectedCodeCategoryId == "00")
            {
                ShowWarningDialog(MsgWngInputRequired, "約定金額以上②");
                cmbGreaterThanCollectCategoryId2.Focus();
                return false;
            }
            else if (nmbGreaterThanRate2.Value == null && cmbGreaterThanCollectCategoryId2.Text != "" && cmbGreaterThanCollectCategoryId2.Text != null)
            {
                ShowWarningDialog(MsgWngInputRequired, "分割②");
                nmbGreaterThanRate2.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cmbGreaterThanRoundingMode2.Text) && cmbGreaterThanCollectCategoryId2.Text != "")
            {
                ShowWarningDialog(MsgWngInputRequired, "端数単位");
                cmbGreaterThanRoundingMode2.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtGreaterThanSightOfBill2.Text) && UseLimitDateCategoryGreatherId2 == 1 && txtGreaterThanSightOfBill2.Enabled)
            {
                ShowWarningDialog(MsgWngInputRequired, "回収サイト");
                txtGreaterThanSightOfBill2.Focus();
                return false;
            }
            else if (cmbGreaterThanCollectCategoryId3.Enabled != false && string.IsNullOrWhiteSpace(cmbGreaterThanCollectCategoryId3.Text) && SelectedCodeCategoryId == "00")
            {
                ShowWarningDialog(MsgWngInputRequired, "約定金額以上③");
                cmbGreaterThanCollectCategoryId3.Focus();
                return false;
            }
            else if (nmbGreaterThanRate3.Value == null && cmbGreaterThanCollectCategoryId3.Text != "")
            {
                ShowWarningDialog(MsgWngInputRequired, "分割③");
                nmbGreaterThanRate3.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cmbGreaterThanRoundingMode3.Text) && cmbGreaterThanCollectCategoryId3.Text != "")
            {
                ShowWarningDialog(MsgWngInputRequired, "端数単位");
                cmbGreaterThanRoundingMode3.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtGreaterThanSightOfBill3.Text) && UseLimitDateCategoryGreatherId3 == 1 && txtGreaterThanSightOfBill3.Enabled)
            {
                ShowWarningDialog(MsgWngInputRequired, "回収サイト");
                txtGreaterThanSightOfBill3.Focus();
                return false;
            }
            else if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text) && string.IsNullOrWhiteSpace(txtExclusiveAccountNumber.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblExclusiveAccountNumber.Text);
                txtExclusiveAccountNumber.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtAccountNumber.Text) && !string.IsNullOrWhiteSpace(txtExclusiveAccountNumber.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblAccountNumber.Text);
                txtAccountNumber.Focus();
                return false;
            }
            else if (mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length > 0 &&
                     mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length != 7)
            {
                ShowWarningDialog(MsgWngInputNeedxxDigits, lblPostalCode.Text, "7桁");
                mskPostalCode.Focus();
                return false;
            }
            ClearStatusMessage();
            return true;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            #region 基本設定１
            if (IsNeedValidate(ApplicationControl.CustomerCodeType, txtCustomerCode.TextLength, ApplicationControl.CustomerCodeLength))
            {
                txtCustomerCode.Text = ZeroLeftPadding(txtCustomerCode);
                txtCustomerCode_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.StaffCodeType, txtStaffCode.TextLength, ApplicationControl.StaffCodeLength))
            {
                txtStaffCode.Text = ZeroLeftPadding(txtStaffCode);
                txtStaffCode_Validated(null, null);
            }
            if (!IsValidClosingDay(txtClosingDay.Text, txtClosingDay.MaxLength))
            {
                txtClosingDay_Validated(null, null);
            }
            if (!IsValidClosingDay(txtCollectOffsetDay.Text, txtCollectOffsetDay.MaxLength))
            {
                txtCollectOffsetDay_Validated(null, null);
            }
            #endregion

            #region 基本設定２
            if (IsNeedValidate(0, txtExclusiveBankCode.TextLength, txtExclusiveBankCode.MaxLength))
            {
                txtExclusiveBankCode.Text = ZeroLeftPadding(txtExclusiveBankCode);
            }
            if (IsNeedValidate(0, txtExclusiveBranchCode.TextLength, txtExclusiveBranchCode.MaxLength))
            {
                txtExclusiveBranchCode.Text = ZeroLeftPadding(txtExclusiveBranchCode);
            }
            if (IsNeedValidate(0, txtExclusiveAccountNumber.TextLength, txtExclusiveAccountNumber.MaxLength))
            {
                txtExclusiveAccountNumber.Text = ZeroLeftPadding(txtExclusiveAccountNumber);
            }
            if (IsNeedValidate(0, txtAccountNumber.TextLength, txtAccountNumber.MaxLength))
            {
                txtAccountNumber.Text = ZeroLeftPadding(txtAccountNumber);
            }
            if (IsNeedValidate(0, txtTransferBankCode.TextLength, txtTransferBankCode.MaxLength))
            {
                txtTransferBankCode.Text = ZeroLeftPadding(txtTransferBankCode);
            }
            if (IsNeedValidate(0, txtTransferBrachCode.TextLength, txtTransferBrachCode.MaxLength))
            {
                txtTransferBrachCode.Text = ZeroLeftPadding(txtTransferBrachCode);
            }
            if (IsNeedValidate(0, txtTransferAccountNumber.TextLength, txtTransferAccountNumber.MaxLength))
            {
                txtTransferAccountNumber.Text = ZeroLeftPadding(txtTransferAccountNumber);
            }
            #endregion

            #region 歩引設定
            if (UseDiscount)
            {
                if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentId.TextLength, ApplicationControl.DepartmentCodeLength))
                {
                    txtDepartmentId.Text = ZeroLeftPadding(txtDepartmentId);
                    txtDepartmentId_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.AccountTitleCodeType, txtAccountTitleId.TextLength, ApplicationControl.AccountTitleCodeLength))
                {
                    txtAccountTitleId.Text = ZeroLeftPadding(txtAccountTitleId);
                    txtAccountTitleId_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentId2.TextLength, ApplicationControl.DepartmentCodeLength))
                {
                    txtDepartmentId2.Text = ZeroLeftPadding(txtDepartmentId2);
                    txtDepartmentId2_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.AccountTitleCodeType, txtAccountTitleId2.TextLength, ApplicationControl.AccountTitleCodeLength))
                {
                    txtAccountTitleId2.Text = ZeroLeftPadding(txtAccountTitleId2);
                    txtAccountTitleId2_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentId3.TextLength, ApplicationControl.DepartmentCodeLength))
                {
                    txtDepartmentId3.Text = ZeroLeftPadding(txtDepartmentId3);
                    txtDepartmentId3_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.AccountTitleCodeType, txtAccountTitleId3.TextLength, ApplicationControl.AccountTitleCodeLength))
                {
                    txtAccountTitleId3.Text = ZeroLeftPadding(txtAccountTitleId3);
                    txtAccountTitleId3_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentId4.TextLength, ApplicationControl.DepartmentCodeLength))
                {
                    txtDepartmentId4.Text = ZeroLeftPadding(txtDepartmentId4);
                    txtDepartmentId4_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.AccountTitleCodeType, txtAccountTitleId4.TextLength, ApplicationControl.AccountTitleCodeLength))
                {
                    txtAccountTitleId4.Text = ZeroLeftPadding(txtAccountTitleId4);
                    txtAccountTitleId4_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.DepartmentCodeType, txtDepartmentId5.TextLength, ApplicationControl.DepartmentCodeLength))
                {
                    txtDepartmentId5.Text = ZeroLeftPadding(txtDepartmentId5);
                    txtDepartmentId5_Validated(null, null);
                }
                if (IsNeedValidate(ApplicationControl.AccountTitleCodeType, txtAccountTitleId5.TextLength, ApplicationControl.AccountTitleCodeLength))
                {
                    txtAccountTitleId5.Text = ZeroLeftPadding(txtAccountTitleId5);
                    txtAccountTitleId5_Validated(null, null);
                }
                nmbRate_Validated(null, null);
                nmbRate2_Validated(null, null);
                nmbRate3_Validated(null, null);
                nmbRate4_Validated(null, null);
                nmbRate5_Validated(null, null);
            }
            #endregion

            #region 強制実行Validatedイベント
            txtCustomerKana_Validated(null, null);
            txtTransferBankName_Validated(null, null);
            txtTransferBranchName_Validated(null, null);

            nmbGreaterThanRate1_Validated(null, null);
            nmbGreaterThanRate2_Validated(null, null);
            nmbGreaterThanRate3_Validated(null, null);
            #endregion
        }

        private bool CheckDivisionSum()
        {
            if (nmbGreaterThanRate1.Enabled != false || nmbGreaterThanRate2.Enabled != false || nmbGreaterThanRate3.Enabled != false)
            {
                var divisionsum = Convert.ToDecimal(nmbGreaterThanRate1.Value) + Convert.ToDecimal(nmbGreaterThanRate2.Value) +
                                  Convert.ToDecimal(nmbGreaterThanRate3.Value);

                if (divisionsum < 100 || divisionsum > 100)
                {
                    ShowWarningDialog(MsgWngInputRequired, "分割");
                    nmbGreaterThanRate1.Focus();
                    return false;
                }
            }
            ClearStatusMessage();
            return true;
        }

        private bool CheckFractionalUnit()
        {
            if (cmbGreaterThanRoundingMode1.Enabled != false || cmbGreaterThanRoundingMode2.Enabled != false || cmbGreaterThanRoundingMode2.Enabled != false)
            {
                if (!((cmbGreaterThanRoundingMode1.Text == "端数" && cmbGreaterThanRoundingMode2.Text != "端数" && cmbGreaterThanRoundingMode3.Text != "端数")
                    || (cmbGreaterThanRoundingMode1.Text != "端数" && cmbGreaterThanRoundingMode2.Text == "端数" && cmbGreaterThanRoundingMode3.Text != "端数")
                    || (cmbGreaterThanRoundingMode1.Text != "端数" && cmbGreaterThanRoundingMode2.Text != "端数" && cmbGreaterThanRoundingMode3.Text == "端数")))
                {
                    ShowWarningDialog(MsgWngSelectAtleastOneRounding);
                    cmbGreaterThanRoundingMode1.Focus();
                    return false;
                }
            }
            ClearStatusMessage();
            return true;
        }

        private bool CheckRoundingMode()
        {
            if (nmbRate.Value != null && cmbRoundingMode.SelectedIndex == -1)
            {
                ShowWarningDialog(MsgWngInputRequired, "歩引率①-端数処理");
                cmbRoundingMode.Focus();
                return false;
            }
            else if (nmbRate2.Value != null && cmbRoundingMode2.SelectedIndex == -1)
            {
                ShowWarningDialog(MsgWngInputRequired, "歩引率②-端数処理");
                cmbRoundingMode2.Focus();
                return false;
            }
            else if (nmbRate3.Value != null && cmbRoundingMode3.SelectedIndex == -1)
            {
                ShowWarningDialog(MsgWngInputRequired, "歩引率③-端数処理");
                cmbRoundingMode3.Focus();
                return false;
            }
            else if (nmbRate4.Value != null && cmbRoundingMode4.SelectedIndex == -1)
            {
                ShowWarningDialog(MsgWngInputRequired, "歩引率④-端数処理");
                cmbRoundingMode4.Focus();
                return false;
            }
            else if (nmbRate5.Value != null && cmbRoundingMode5.SelectedIndex == -1)
            {
                ShowWarningDialog(MsgWngInputRequired, "歩引率⑤-端数処理");
                cmbRoundingMode5.Focus();
                return false;
            }
            ClearStatusMessage();
            return true;
        }

        private bool CheckDiscountRate()
        {
            var disratesum = Convert.ToDecimal(nmbRate.Value) + Convert.ToDecimal(nmbRate2.Value) + Convert.ToDecimal(nmbRate3.Value) + Convert.ToDecimal(nmbRate4.Value) + Convert.ToDecimal(nmbRate5.Value);

            if (disratesum > 100)
            {
                ShowWarningDialog(MsgWngNoRegisOver100PerDiscount, "歩引率");
                nmbRate.Focus();
                return false;
            }
            ClearStatusMessage();
            return true;
        }

        /// <summary>　登録前に得意先データを準備</summary>
        /// <returns>作成されたCustomerModel</returns>
        private Customer PrepareCustomerData()
        {
            Customer customer = new Customer();
            customer.CompanyId = CompanyId;
            customer.Code = txtCustomerCode.Text;
            customer.Name = txtCustomerName.Text.Trim();
            customer.Kana = txtCustomerKana.Text.Trim();
            customer.PostalCode = mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length == 0 ?
                                  string.Empty : mskPostalCode.Text;
            customer.Address1 = txtAddress1.Text.Trim();
            customer.Address2 = txtAddress2.Text.Trim();
            customer.Tel = txtCustomerTel.Text;
            customer.Fax = txtCustomerFax.Text;
            customer.DestinationDepartmentName = txtDestinationDepartmentName.Text.Trim();
            customer.CustomerStaffName = txtCustomerStaffName.Text.Trim();
            customer.Honorific = cmbHonorific.Text.Trim();
            customer.ExclusiveAccountNumber = txtExclusiveAccountNumber.Text.ToString() + txtAccountNumber.Text.ToString();
            if (cmbExclusiveAccountTypeId.SelectedIndex != -1)
                customer.ExclusiveAccountTypeId = Convert.ToInt32(cmbExclusiveAccountTypeId.SelectedItem.SubItems[1].Value);
            customer.ExclusiveBankCode = txtExclusiveBankCode.Text;
            customer.ExclusiveBankName = txtExclusiveBankName.Text.Trim();
            customer.ExclusiveBranchCode = txtExclusiveBranchCode.Text;
            customer.ExclusiveBranchName = txtExclusiveBranchName.Text.Trim();
            customer.ShareTransferFee = cmbShareTransferFee.SelectedIndex;
            customer.CreditLimit = nmbCreditLimit.Value ?? 0M;
            customer.ClosingDay = int.Parse(txtClosingDay.Text);
            customer.CollectCategoryId = int.Parse(CategoryIndex);
            customer.StaffId = StaffId;
            if (cbxCustomerIsParent.Checked) customer.IsParent = 1;
            else customer.IsParent = 0;
            customer.Note = txtNote.Text.Trim();
            if (txtSightOfBill.Text != "")
                customer.SightOfBill = int.Parse(txtSightOfBill.Text);
            customer.DensaiCode = txtDensaiCode.Text;
            customer.CreditCode = txtCreditCode.Text;
            customer.CreditRank = txtCreditRank.Text;
            customer.TransferAccountNumber = txtTransferAccountNumber.Text;
            if (cmbTransferAccountTypeId.SelectedIndex != -1)
                customer.TransferAccountTypeId = Convert.ToInt32(cmbTransferAccountTypeId.SelectedItem.SubItems[1].Value);
            customer.TransferBankCode = txtTransferBankCode.Text;
            customer.TransferBankName = txtTransferBankName.Text.Trim();
            customer.TransferBranchCode = txtTransferBrachCode.Text;
            customer.TransferBranchName = txtTransferBranchName.Text.Trim();
            customer.TransferNewCode = txtTransferNewCode.Text;
            customer.TransferAccountName = txtTransferAccountName.Text.Trim();
            customer.TransferCustomerCode = txtTransferCustomerCode.Text.Trim();
            customer.ReceiveAccountId1 = cbxReceiveAccountId1.Checked ? 1 : 0;
            customer.ReceiveAccountId2 = cbxReceiveAccountId2.Checked ? 1 : 0;
            customer.ReceiveAccountId3 = cbxReceiveAccountId3.Checked ? 1 : 0;
            customer.UseFeeLearning = cbxUseFeeLearning.Checked ? 1 : 0;
            customer.UseKanaLearning = cbxUseKanaLearning.Checked ? 1 : 0;
            customer.UseFeeTolerance = cbxUseFeeTolerance.Checked ? 1 : 0;
            customer.PrioritizeMatchingIndividually = cbxPrioritizeMatchingIndividually.Checked ? 1 : 0;
            customer.ExcludeInvoicePublish = cbxExcludeInvoicePublish.Checked ? 1 : 0;
            customer.ExcludeReminderPublish = cbxExcludeReminderPublish.Checked ? 1 : 0;
            customer.CollationKey = txtCollationKey.Text;
            customer.HolidayFlag = cmbHolidayFlag.SelectedIndex;
            customer.CollectOffsetMonth = int.Parse(txtCollectOffsetMonth.Text);
            customer.CollectOffsetDay = int.Parse(txtCollectOffsetDay.Text);
            customer.StringValue1 = null;
            customer.StringValue2 = null;
            customer.StringValue3 = null;
            customer.StringValue4 = null;
            customer.StringValue5 = null;
            customer.IntValue1 = null;
            customer.IntValue2 = null;
            customer.IntValue3 = null;
            customer.IntValue4 = null;
            customer.IntValue5 = null;
            customer.NumericValue1 = null;
            customer.NumericValue2 = null;
            customer.NumericValue2 = null;
            customer.NumericValue2 = null;
            customer.NumericValue2 = null;
            customer.CreateBy = Login.UserId;
            customer.UpdateBy = Login.UserId;
            customer.UpdateAt = UpdateAtCustomer;

            return customer;
        }

        private async Task<bool> SaveCustomer()
        {
            Customer customer = PrepareCustomerData();
            var succeeded = false;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                var result = await service.SaveAsync(Login.SessionKey, customer);
                if (result.ProcessResult.Result)
                {
                    if (result.Customer != null)
                    {
                        SaveCustomerId = result.Customer.Id;

                        //SaveまたはDeleteをすることをチェック
                        await SaveOrDeletePaymentContract();

                        if (UseDiscount)
                            await SaveOrDeleteCustomerDiscount();

                        succeeded = true;
                        if (CustomerCode != null)
                        {
                            CustomerCode = null;
                            ChangeFee = false;
                            ParentForm.DialogResult = DialogResult.OK;
                        }
                    }
                }
            });
            return succeeded;
        }

        private async Task SaveOrDeletePaymentContract()
        {
            if (cmbCollectCategoryId.Text == "00:約定")
            {
                await SavePaymentContract();
            }
            else
            {
                await DeletePaymentContract();
            }
        }

        /// <summary>登録前にCustomerPaymentContractデータを準備</summary>
        /// <returns>生成されたCustomerPaymentContract Model</returns>
        private CustomerPaymentContract PreparePaymentContractData()
        {
            var paymentContract = new CustomerPaymentContract();

            paymentContract.CustomerId = SaveCustomerId;
            paymentContract.ThresholdValue = nmbThresholdValue.Value.Value;
            paymentContract.LessThanCollectCategoryId = int.Parse(LessThanCategoryIndex);
            paymentContract.GreaterThanCollectCategoryId1 = int.Parse(GreaterThanCategoryIndex1);
            paymentContract.GreaterThanRate1 = nmbGreaterThanRate1.Value ?? 0M;
            paymentContract.GreaterThanRoundingMode1 = cmbGreaterThanRoundingMode1.SelectedIndex;
            if (txtGreaterThanSightOfBill1.Text != "")
                paymentContract.GreaterThanSightOfBill1 = int.Parse(txtGreaterThanSightOfBill1.Text);
            else
                paymentContract.GreaterThanSightOfBill1 = 0;
            if (GreaterThanCategoryIndex2 != null)
                paymentContract.GreaterThanCollectCategoryId2 = int.Parse(GreaterThanCategoryIndex2);
            else
                paymentContract.GreaterThanCollectCategoryId2 = 0;
            paymentContract.GreaterThanRate2 = nmbGreaterThanRate2.Value;
            if (cmbGreaterThanRoundingMode2.SelectedIndex != -1)
                paymentContract.GreaterThanRoundingMode2 = cmbGreaterThanRoundingMode2.SelectedIndex;
            if (txtGreaterThanSightOfBill2.Text != "")
                paymentContract.GreaterThanSightOfBill2 = int.Parse(txtGreaterThanSightOfBill2.Text);
            if (GreaterThanCategoryIndex3 != null)
                paymentContract.GreaterThanCollectCategoryId3 = int.Parse(GreaterThanCategoryIndex3);
            else
                paymentContract.GreaterThanCollectCategoryId3 = 0;
            paymentContract.GreaterThanRate3 = nmbGreaterThanRate3.Value;
            if (cmbGreaterThanRoundingMode3.SelectedIndex != -1)
                paymentContract.GreaterThanRoundingMode3 = cmbGreaterThanRoundingMode3.SelectedIndex;
            if (txtGreaterThanSightOfBill3.Text != "")
                paymentContract.GreaterThanSightOfBill3 = int.Parse(txtGreaterThanSightOfBill3.Text);
            paymentContract.CreateBy = Login.UserId;
            paymentContract.UpdateBy = Login.UserId;
            paymentContract.UpdateAt = UpdateAtCusPayment;

            return paymentContract;
        }

        public async Task SavePaymentContract()
        {
            CustomerPaymentContract paymentContract = PreparePaymentContractData();

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomerPaymentContractResult result = await service.SavePaymentContractAsync(SessionKey, paymentContract);
            });
        }

        public async Task DeletePaymentContract()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CountResult deleteResult = await service.DeletePaymentContractAsync(SessionKey, SaveCustomerId);
            });
        }

        /// <summary>登録前にCustomerDiscountデータを準備</summary>
        /// <returns>生成されたCustomerDiscount Model</returns>
        private CustomerDiscount PrepareDataDiscountRate1()
        {
            var cusDiscount1 = new CustomerDiscount();

            cusDiscount1.CustomerId = SaveCustomerId;
            cusDiscount1.Sequence = 1;
            cusDiscount1.Rate = nmbRate.Value ?? 0M;
            cusDiscount1.RoundingMode = cmbRoundingMode.SelectedIndex;
            cusDiscount1.MinValue = nmbMinValue.Value ?? 0M;
            cusDiscount1.DepartmentId = DepartmentId1;
            cusDiscount1.AccountTitleId = AccountTitleId1;
            cusDiscount1.SubCode = txtSubCode.Text;
            cusDiscount1.CreateBy = Login.UserId;
            cusDiscount1.UpdateBy = Login.UserId;
            cusDiscount1.UpdateAt = UpdateAtCusDiscount1;

            return cusDiscount1;
        }

        /// <summary>登録前にCustomerDiscountデータを準備</summary>
        /// <returns>生成されたCustomerDiscount Model</returns>
        private CustomerDiscount PrepareDataDiscountRate2()
        {
            var cusDiscount2 = new CustomerDiscount();

            cusDiscount2.CustomerId = SaveCustomerId;
            cusDiscount2.Sequence = 2;
            cusDiscount2.Rate = nmbRate2.Value ?? 0M;
            cusDiscount2.RoundingMode = cmbRoundingMode2.SelectedIndex;
            cusDiscount2.MinValue = nmbMinValue.Value ?? 0M;
            cusDiscount2.DepartmentId = DepartmentId2;
            cusDiscount2.AccountTitleId = AccountTitleId2;
            cusDiscount2.SubCode = txtSubCode2.Text;
            cusDiscount2.CreateBy = Login.UserId;
            cusDiscount2.UpdateBy = Login.UserId;
            cusDiscount2.UpdateAt = UpdateAtCusDiscount2;
            return cusDiscount2;
        }

        /// <summary>登録前にCustomerDiscountデータを準備</summary>
        /// <returns>生成されたCustomerDiscount Model</returns>
        private CustomerDiscount PrepareDataDiscountRate3()
        {
            var cusDiscount3 = new CustomerDiscount();

            cusDiscount3.CustomerId = SaveCustomerId;
            cusDiscount3.Sequence = 3;
            cusDiscount3.Rate = nmbRate3.Value ?? 0M;
            cusDiscount3.RoundingMode = cmbRoundingMode3.SelectedIndex;
            cusDiscount3.MinValue = nmbMinValue.Value ?? 0M;
            cusDiscount3.DepartmentId = DepartmentId3;
            cusDiscount3.AccountTitleId = AccountTitleId3;
            cusDiscount3.SubCode = txtSubCode3.Text;
            cusDiscount3.CreateBy = Login.UserId;
            cusDiscount3.UpdateBy = Login.UserId;
            cusDiscount3.UpdateAt = UpdateAtCusDiscount3;
            return cusDiscount3;
        }

        /// <summary>登録前にCustomerDiscountデータを準備</summary>
        /// <returns>生成されたCustomerDiscount Model</returns>
        private CustomerDiscount PrepareDataDiscountRate4()
        {
            var cusDiscount4 = new CustomerDiscount();

            cusDiscount4.CustomerId = SaveCustomerId;
            cusDiscount4.Sequence = 4;
            cusDiscount4.Rate = nmbRate4.Value ?? 0M;
            cusDiscount4.RoundingMode = cmbRoundingMode4.SelectedIndex;
            cusDiscount4.MinValue = nmbMinValue.Value ?? 0M;
            cusDiscount4.DepartmentId = DepartmentId4;
            cusDiscount4.AccountTitleId = AccountTitleId4;
            cusDiscount4.SubCode = txtSubCode4.Text;
            cusDiscount4.CreateBy = Login.UserId;
            cusDiscount4.UpdateBy = Login.UserId;
            cusDiscount4.UpdateAt = UpdateAtCusDiscount4;
            return cusDiscount4;
        }

        /// <summary>登録前にCustomerDiscountデータを準備</summary>
        /// <returns>生成されたCustomerDiscount Model</returns>
        private CustomerDiscount PrepareDataDiscountRate5()
        {
            var cusDiscount5 = new CustomerDiscount();

            cusDiscount5.CustomerId = SaveCustomerId;
            cusDiscount5.Sequence = 5;
            cusDiscount5.Rate = nmbRate5.Value ?? 0M;
            cusDiscount5.RoundingMode = cmbRoundingMode5.SelectedIndex;
            cusDiscount5.MinValue = nmbMinValue.Value ?? 0M;
            cusDiscount5.DepartmentId = DepartmentId5;
            cusDiscount5.AccountTitleId = AccountTitleId5;
            cusDiscount5.SubCode = txtSubCode5.Text;
            cusDiscount5.CreateBy = Login.UserId;
            cusDiscount5.UpdateBy = Login.UserId;
            cusDiscount5.UpdateAt = UpdateAtCusDiscount5;
            return cusDiscount5;
        }

        public async Task SaveOrDeleteCustomerDiscount()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                if (nmbRate.Value.HasValue && nmbRate.Value > 0)
                {
                    CustomerDiscount cusDiscount = PrepareDataDiscountRate1();
                    CustomerDiscountResult result = await service.SaveDiscountAsync(SessionKey, cusDiscount);
                }
                else if (cmbRoundingMode.SelectedIndex != -1 && (nmbRate.Value.HasValue || nmbRate.Value == 0))
                {
                    CountResult deleteResult = await service.DeleteDiscountAsync(SessionKey, SaveCustomerId, 1);
                }

                if (nmbRate2.Value.HasValue && nmbRate2.Value > 0)
                {
                    CustomerDiscount cusDiscount = PrepareDataDiscountRate2();
                    CustomerDiscountResult result = await service.SaveDiscountAsync(SessionKey, cusDiscount);
                }
                else if (cmbRoundingMode2.SelectedIndex != -1 && (nmbRate2.Value.HasValue || nmbRate2.Value == 0))
                {
                    CountResult deleteResult = await service.DeleteDiscountAsync(SessionKey, SaveCustomerId, 2);
                }

                if (nmbRate3.Value.HasValue && nmbRate3.Value > 0)
                {
                    CustomerDiscount cusDiscount = PrepareDataDiscountRate3();
                    CustomerDiscountResult result = await service.SaveDiscountAsync(SessionKey, cusDiscount);
                }
                else if (cmbRoundingMode3.SelectedIndex != -1 && (nmbRate3.Value.HasValue || nmbRate3.Value == 0))
                {
                    CountResult deleteResult = await service.DeleteDiscountAsync(SessionKey, SaveCustomerId, 3);
                }

                if (nmbRate4.Value.HasValue && nmbRate4.Value > 0)
                {
                    CustomerDiscount cusDiscount = PrepareDataDiscountRate4();
                    CustomerDiscountResult result = await service.SaveDiscountAsync(SessionKey, cusDiscount);
                }
                else if (cmbRoundingMode4.SelectedIndex != -1 && (nmbRate4.Value.HasValue || nmbRate4.Value == 0))
                {
                    CountResult deleteResult = await service.DeleteDiscountAsync(SessionKey, SaveCustomerId, 4);
                }

                if (nmbRate5.Value.HasValue && nmbRate5.Value > 0)
                {
                    CustomerDiscount cusDiscount = PrepareDataDiscountRate5();
                    CustomerDiscountResult result = await service.SaveDiscountAsync(SessionKey, cusDiscount);
                }
                else if (cmbRoundingMode5.SelectedIndex != -1 && (nmbRate5.Value.HasValue || nmbRate5.Value == 0))
                {
                    CountResult deleteResult = await service.DeleteDiscountAsync(SessionKey, SaveCustomerId, 5);
                }
            });
        }
        #endregion

        #region Validate Event
        private void txtCustomerCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerCode.Text == "")
                {
                    txtCustomerCode.Clear();
                    ClearStatusMessage();
                    return;
                }
                var code = txtCustomerCode.Text;
                ProgressDialog.Start(ParentForm, SetCustomerInformationToUIAsync(code), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private async Task SetCustomerInformationToUIAsync(string code)
        {
            await LoadCustomerAsync(code);
            var customer = LoadedCustomer;
            if (customer != null)
            {
                //DBからデータを入力項目に設定
                await SetCustomerData(customer);
            }
            else
            {
                Modified = true;
                BaseContext.SetFunction03Enabled(false);
            }
            if (customer == null)
            {
                DispStatusMessage(MsgInfNewData, "得意先コード");
            }
        }


        private void txtStaffCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtStaffCode.Text == "")
                {
                    lblStaffName.Clear();
                    ClearStatusMessage();
                }
                else
                {
                    SetStaffCodeVal();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtClosingDay_Validated(object sender, EventArgs e)
        {
            string closingDay = txtClosingDay.Text;
            txtClosingDay.Text = GetFormattedClosingDay(closingDay);

            // 00入力時は「都度請求」にチェックする
            if (txtClosingDay.Text == "00")
            {
                cbxIssueBillEachTime.Checked = true;
            }
        }

        private void txtCollectOffsetDay_Validated(object sender, EventArgs e)
        {
            string collectOffsetDay = txtCollectOffsetDay.Text;
            txtCollectOffsetDay.Text = GetFormattedClosingDay(collectOffsetDay, !cbxIssueBillEachTime.Checked);
        }

        private void nmbGreaterThanRate1_Validated(object sender, EventArgs e)
        {
            if (nmbGreaterThanRate1.Value.HasValue)
            {
                nmbGreaterThanRate1.Value = Math.Round(nmbGreaterThanRate1.Value.Value, 1);
            }

            if (nmbGreaterThanRate1.Value == null || nmbGreaterThanRate1.Value == 0 || nmbGreaterThanRate1.Value >= 100)
            {
                cmbGreaterThanCollectCategoryId2.SelectedIndex = -1;
                GreaterThanCategoryIndex2 = "0";
                nmbGreaterThanRate2.Clear();
                cmbGreaterThanRoundingMode2.SelectedIndex = -1;
                txtGreaterThanSightOfBill2.Clear();

                cmbGreaterThanCollectCategoryId2.Enabled = false;
                nmbGreaterThanRate2.Enabled = false;
                cmbGreaterThanRoundingMode2.Enabled = false;
                txtGreaterThanSightOfBill2.Enabled = false;

                cmbGreaterThanCollectCategoryId3.SelectedIndex = -1;
                GreaterThanCategoryIndex3 = "0";
                nmbGreaterThanRate3.Clear();
                cmbGreaterThanRoundingMode3.SelectedIndex = -1;
                txtGreaterThanSightOfBill3.Clear();

                cmbGreaterThanCollectCategoryId3.Enabled = false;
                nmbGreaterThanRate3.Enabled = false;
                cmbGreaterThanRoundingMode3.Enabled = false;
                txtGreaterThanSightOfBill3.Enabled = false;
            }
            else
            {
                cmbGreaterThanCollectCategoryId2.Enabled = true;
            }
        }

        private void nmbGreaterThanRate2_Validated(object sender, EventArgs e)
        {
            if (nmbGreaterThanRate2.Value.HasValue)
            {
                nmbGreaterThanRate2.Value = Math.Round(nmbGreaterThanRate2.Value.Value, 1);
            }

            var rate = nmbGreaterThanRate1.Value + nmbGreaterThanRate2.Value;

            if (nmbGreaterThanRate2.Value == null || nmbGreaterThanRate2.Value == 0 || rate >= 100)
            {
                cmbGreaterThanCollectCategoryId3.SelectedIndex = -1;
                nmbGreaterThanRate3.Clear();
                cmbGreaterThanRoundingMode3.SelectedIndex = -1;
                txtGreaterThanSightOfBill3.Clear();

                cmbGreaterThanCollectCategoryId3.Enabled = false;
                nmbGreaterThanRate3.Enabled = false;
                cmbGreaterThanRoundingMode3.Enabled = false;
                txtGreaterThanSightOfBill3.Enabled = false;
            }
            else
            {
                cmbGreaterThanCollectCategoryId3.Enabled = true;
            }
        }

        private void nmbGreaterThanRate3_Validated(object sender, EventArgs e)
        {
            if (nmbGreaterThanRate3.Value.HasValue)
            {
                nmbGreaterThanRate3.Value = Math.Round(nmbGreaterThanRate3.Value.Value, 1);
            }
        }

        private void nmbRate_Validated(object sender, EventArgs e)
        {
            if (nmbRate.Value.HasValue)
            {
                nmbRate.Value = Math.Round(nmbRate.Value.Value, 2);
            }
        }

        private void nmbRate2_Validated(object sender, EventArgs e)
        {
            if (nmbRate2.Value.HasValue)
            {
                nmbRate2.Value = Math.Round(nmbRate2.Value.Value, 2);
            }
        }

        private void nmbRate3_Validated(object sender, EventArgs e)
        {
            if (nmbRate3.Value.HasValue)
            {
                nmbRate3.Value = Math.Round(nmbRate3.Value.Value, 2);
            }
        }

        private void nmbRate4_Validated(object sender, EventArgs e)
        {
            if (nmbRate4.Value.HasValue)
            {
                nmbRate4.Value = Math.Round(nmbRate4.Value.Value, 2);
            }
        }

        private void nmbRate5_Validated(object sender, EventArgs e)
        {
            if (nmbRate5.Value.HasValue)
            {
                nmbRate5.Value = Math.Round(nmbRate5.Value.Value, 2);
            }
        }

        private void txtCustomerKana_Validated(object sender, EventArgs e)
        {
            txtCustomerKana.Text = ConvertPayerName(txtCustomerKana.Text.Trim());
        }

        private void txtTransferBankName_Validated(object sender, EventArgs e)
        {
            txtTransferBankName.Text = ConvertPayerName(txtTransferBankName.Text.Trim());
        }

        private void txtTransferBranchName_Validated(object sender, EventArgs e)
        {
            txtTransferBranchName.Text = ConvertPayerName(txtTransferBranchName.Text.Trim());
        }

        private void txtDepartmentId_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDepartmentId.Text))
            {
                lblDepartmentName.Clear();
                DepartmentId1 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<DepartmentMasterClient>();
                    DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtDepartmentId.Text });

                    if (result.ProcessResult.Result)
                        DepartmentResult = result.Departments.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (DepartmentResult != null)
                {
                    lblDepartmentName.Text = DepartmentResult.Name;
                    txtDepartmentId.Text = DepartmentResult.Code;
                    DepartmentId1 = DepartmentResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "部門", txtDepartmentId.Text);
                    lblDepartmentName.Clear();
                    txtDepartmentId.Clear();
                    DepartmentId1 = null;
                    txtDepartmentId.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtAccountTitleId_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAccountTitleId.Text))
            {
                lblAccountTitleName.Clear();
                AccountTitleId1 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                string[] code = new string[1];
                code[0] = txtAccountTitleId.Text;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<AccountTitleMasterClient>();
                    AccountTitlesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, code);

                    if (result.ProcessResult.Result)
                        AccountTitleResult = result.AccountTitles.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (AccountTitleResult != null)
                {
                    lblAccountTitleName.Text = AccountTitleResult.Name;
                    txtAccountTitleId.Text = AccountTitleResult.Code;
                    AccountTitleId1 = AccountTitleResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "科目", txtAccountTitleId.Text);
                    lblAccountTitleName.Clear();
                    txtAccountTitleId.Clear();
                    AccountTitleId1 = null;
                    txtAccountTitleId.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentId2_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDepartmentId2.Text))
            {
                lblDepartmentName2.Clear();
                DepartmentId2 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<DepartmentMasterClient>();
                    DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtDepartmentId2.Text });

                    if (result.ProcessResult.Result)
                        DepartmentResult = result.Departments.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (DepartmentResult != null)
                {
                    lblDepartmentName2.Text = DepartmentResult.Name;
                    txtDepartmentId2.Text = DepartmentResult.Code;
                    DepartmentId2 = DepartmentResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "部門", txtDepartmentId2.Text);
                    lblDepartmentName2.Clear();
                    txtDepartmentId2.Clear();
                    DepartmentId2 = null;
                    txtDepartmentId2.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtAccountTitleId2_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAccountTitleId2.Text))
            {
                lblAccountTitleName2.Clear();
                AccountTitleId2 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<AccountTitleMasterClient>();
                    AccountTitlesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtAccountTitleId2.Text });

                    if (result.ProcessResult.Result)
                        AccountTitleResult = result.AccountTitles.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (AccountTitleResult != null)
                {
                    lblAccountTitleName2.Text = AccountTitleResult.Name;
                    txtAccountTitleId2.Text = AccountTitleResult.Code;
                    AccountTitleId2 = AccountTitleResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "科目", txtAccountTitleId2.Text);
                    lblAccountTitleName2.Clear();
                    txtAccountTitleId2.Clear();
                    AccountTitleId2 = null;
                    txtAccountTitleId2.Focus();
                }

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentId3_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDepartmentId3.Text))
            {
                lblDepartmentName3.Clear();
                DepartmentId3 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<DepartmentMasterClient>();
                    DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtDepartmentId3.Text });

                    if (result.ProcessResult.Result)
                        DepartmentResult = result.Departments.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (DepartmentResult != null)
                {
                    lblDepartmentName3.Text = DepartmentResult.Name;
                    txtDepartmentId3.Text = DepartmentResult.Code;
                    DepartmentId3 = DepartmentResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "部門", txtDepartmentId3.Text);
                    lblDepartmentName3.Clear();
                    txtDepartmentId3.Clear();
                    DepartmentId3 = null;
                    txtDepartmentId3.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtAccountTitleId3_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAccountTitleId3.Text))
            {
                lblAccountTitleName3.Clear();
                AccountTitleId3 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                string[] code = new string[1];
                code[0] = txtAccountTitleId3.Text;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<AccountTitleMasterClient>();
                    AccountTitlesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, code);

                    if (result.ProcessResult.Result)
                        AccountTitleResult = result.AccountTitles.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (AccountTitleResult != null)
                {
                    lblAccountTitleName3.Text = AccountTitleResult.Name;
                    txtAccountTitleId3.Text = AccountTitleResult.Code;
                    AccountTitleId3 = AccountTitleResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "科目", txtAccountTitleId3.Text);
                    lblAccountTitleName3.Clear();
                    txtAccountTitleId3.Clear();
                    AccountTitleId3 = null;
                    txtAccountTitleId3.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentId4_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDepartmentId4.Text))
            {
                lblDepartmentName4.Clear();
                DepartmentId4 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<DepartmentMasterClient>();
                    DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtDepartmentId4.Text });

                    if (result.ProcessResult.Result)
                        DepartmentResult = result.Departments.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (DepartmentResult != null)
                {
                    lblDepartmentName4.Text = DepartmentResult.Name;
                    txtDepartmentId4.Text = DepartmentResult.Code;
                    DepartmentId4 = DepartmentResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "部門", txtDepartmentId4.Text);
                    lblDepartmentName4.Clear();
                    txtDepartmentId4.Clear();
                    DepartmentId4 = null;
                    txtDepartmentId4.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtAccountTitleId4_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAccountTitleId4.Text))
            {
                lblAccountTitleName4.Clear();
                AccountTitleId4 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                string[] code = new string[1];
                code[0] = txtAccountTitleId4.Text;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<AccountTitleMasterClient>();
                    AccountTitlesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, code);

                    if (result.ProcessResult.Result)
                        AccountTitleResult = result.AccountTitles.FirstOrDefault();

                });

                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (AccountTitleResult != null)
                {
                    lblAccountTitleName4.Text = AccountTitleResult.Name;
                    txtAccountTitleId4.Text = AccountTitleResult.Code;
                    AccountTitleId4 = AccountTitleResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "科目", txtAccountTitleId4.Text);
                    lblAccountTitleName4.Clear();
                    txtAccountTitleId4.Clear();
                    AccountTitleId4 = null;
                    txtAccountTitleId4.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentId5_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDepartmentId5.Text))
            {
                lblDepartmentName5.Clear();
                DepartmentId5 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<DepartmentMasterClient>();
                    DepartmentsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtDepartmentId5.Text });

                    if (result.ProcessResult.Result)
                        DepartmentResult = result.Departments.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (DepartmentResult != null)
                {
                    lblDepartmentName5.Text = DepartmentResult.Name;
                    txtDepartmentId5.Text = DepartmentResult.Code;
                    DepartmentId5 = DepartmentResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "部門", txtDepartmentId5.Text);
                    lblDepartmentName5.Clear();
                    txtDepartmentId5.Clear();
                    DepartmentId5 = null;
                    txtDepartmentId5.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtAccountTitleId5_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAccountTitleId5.Text))
            {
                lblAccountTitleName5.Clear();
                AccountTitleId5 = null;
                ClearStatusMessage();
                return;
            }

            try
            {
                string[] code = new string[1];
                code[0] = txtAccountTitleId5.Text;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<AccountTitleMasterClient>();
                    AccountTitlesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, code);

                    if (result.ProcessResult.Result)
                        AccountTitleResult = result.AccountTitles.FirstOrDefault();

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (AccountTitleResult != null)
                {
                    lblAccountTitleName5.Text = AccountTitleResult.Name;
                    txtAccountTitleId5.Text = AccountTitleResult.Code;
                    AccountTitleId5 = AccountTitleResult.Id;
                    ClearStatusMessage();
                }
                else
                {
                    ClearStatusMessage();
                    ShowWarningDialog(MsgWngMasterNotExist, "科目", txtAccountTitleId5.Text);
                    lblAccountTitleName5.Clear();
                    txtAccountTitleId5.Clear();
                    AccountTitleId5 = null;
                    txtAccountTitleId5.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region SelectedIndexChange Event
        private void cmbCollectCategoryId_SelectedIndexChangedEvent(object sender, EventArgs e)
        {
            if (cmbCollectCategoryId.Text != "")
            {
                SelectedCodeCategoryId = cmbCollectCategoryId.Text.ToString().Substring(0, 2);
                var code = CollectCategoryList.Where(c => c.Code == SelectedCodeCategoryId);
                var selectUseLimitDate = code.Select(x => x.UseLimitDate);
                UseLimitDateCategoryId = selectUseLimitDate.First();
            }

            if (cmbCollectCategoryId.SelectedIndex != -1)
            {
                Category selected = cmbCollectCategoryId.Items[cmbCollectCategoryId.SelectedIndex].Tag as Category;
                CategoryIndex = selected.Id.ToString();
            }

            if (UseLimitDateCategoryId == 1)
            {
                txtSightOfBill.Enabled = true;
            }
            else
            {
                txtSightOfBill.Enabled = false;
                txtSightOfBill.Clear();
            }
            if (SelectedCodeCategoryId == "00")
            {
                nmbThresholdValue.Enabled = true;
                cmbLessThanCollectCategoryId.Enabled = true;
                cmbGreaterThanCollectCategoryId1.Enabled = true;
            }
            else
            {
                nmbThresholdValue.Clear();
                cmbLessThanCollectCategoryId.Text = "";

                cmbGreaterThanCollectCategoryId1.Text = "";
                nmbGreaterThanRate1.Clear();
                cmbGreaterThanRoundingMode1.Text = "";
                txtGreaterThanSightOfBill1.Clear();

                cmbGreaterThanCollectCategoryId2.Text = "";
                nmbGreaterThanRate2.Clear();
                cmbGreaterThanRoundingMode2.Text = "";
                txtGreaterThanSightOfBill2.Clear();

                cmbGreaterThanCollectCategoryId3.Text = "";
                nmbGreaterThanRate3.Clear();
                cmbGreaterThanRoundingMode3.Text = "";
                txtGreaterThanSightOfBill3.Clear();

                nmbThresholdValue.Enabled = false;
                cmbLessThanCollectCategoryId.Enabled = false;
                cmbLessThanCollectCategoryId.SelectedIndex = -1;

                cmbGreaterThanCollectCategoryId1.Enabled = false;
                cmbGreaterThanCollectCategoryId1.SelectedIndex = -1;
                nmbGreaterThanRate1.Enabled = false;
                cmbGreaterThanRoundingMode1.Enabled = false;
                cmbGreaterThanRoundingMode1.SelectedIndex = -1;
                txtGreaterThanSightOfBill1.Enabled = false;

                cmbGreaterThanCollectCategoryId2.Enabled = false;
                cmbGreaterThanCollectCategoryId2.SelectedIndex = -1;
                nmbGreaterThanRate2.Enabled = false;
                cmbGreaterThanRoundingMode2.Enabled = false;
                cmbGreaterThanRoundingMode2.SelectedIndex = -1;
                txtGreaterThanSightOfBill2.Enabled = false;

                cmbGreaterThanCollectCategoryId3.Enabled = false;
                cmbGreaterThanCollectCategoryId3.SelectedIndex = -1;
                nmbGreaterThanRate3.Enabled = false;
                cmbGreaterThanRoundingMode3.Enabled = false;
                cmbGreaterThanRoundingMode3.SelectedIndex = -1;
                txtGreaterThanSightOfBill3.Enabled = false;
            }
        }

        private void cmbGreaterThanCollectCategoryId1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGreaterThanCollectCategoryId1.Text != "")
            {
                string selectedCodeGreatherId1 = cmbGreaterThanCollectCategoryId1.Text.ToString().Substring(0, 2);
                var code = GreatherThanCollectCategoryList.Where(c => c.Code == selectedCodeGreatherId1);
                var selectUseLimitDate = code.Select(x => x.UseLimitDate);
                UseLimitDateCategoryGreatherId1 = selectUseLimitDate.First();
            }

            if (cmbGreaterThanCollectCategoryId1.SelectedIndex != -1)
            {
                Category selected = cmbGreaterThanCollectCategoryId1.Items[cmbGreaterThanCollectCategoryId1.SelectedIndex].Tag as Category;
                GreaterThanCategoryIndex1 = selected.Id.ToString();
            }

            if (cmbGreaterThanCollectCategoryId1.Text == "")
            {
                nmbGreaterThanRate1.Clear();
                cmbGreaterThanRoundingMode1.SelectedIndex = -1;
                txtGreaterThanSightOfBill1.Clear();

                nmbGreaterThanRate1.Enabled = false;
                cmbGreaterThanRoundingMode1.Enabled = false;
                txtGreaterThanSightOfBill1.Enabled = false;
            }
            else
            {
                nmbGreaterThanRate1.Enabled = true;
                cmbGreaterThanRoundingMode1.Enabled = true;

                if (UseLimitDateCategoryGreatherId1 == 1)
                {
                    txtGreaterThanSightOfBill1.Enabled = true;
                }
                else
                {
                    txtGreaterThanSightOfBill1.Enabled = false;
                    txtGreaterThanSightOfBill1.Clear();
                }
            }
        }

        private void cmbGreaterThanCollectCategoryId2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGreaterThanCollectCategoryId2.Text != "")
            {
                string selectedCodeGreatherId2 = cmbGreaterThanCollectCategoryId2.Text.ToString().Substring(0, 2);
                var code = GreatherThanCollectCategoryList.Where(c => c.Code == selectedCodeGreatherId2);
                var selectUseLimitDate = code.Select(x => x.UseLimitDate);
                UseLimitDateCategoryGreatherId2 = selectUseLimitDate.First();
            }

            if (cmbGreaterThanCollectCategoryId2.SelectedIndex != -1)
            {
                Category selected = cmbGreaterThanCollectCategoryId2.Items[cmbGreaterThanCollectCategoryId2.SelectedIndex].Tag as Category;
                GreaterThanCategoryIndex2 = selected.Id.ToString();
            }

            if (cmbGreaterThanCollectCategoryId2.Text == "")
            {
                nmbGreaterThanRate2.Clear();
                cmbGreaterThanRoundingMode2.SelectedIndex = -1;
                txtGreaterThanSightOfBill2.Clear();

                nmbGreaterThanRate2.Enabled = false;
                cmbGreaterThanRoundingMode2.Enabled = false;
                txtGreaterThanSightOfBill2.Enabled = false;
            }
            else
            {
                nmbGreaterThanRate2.Enabled = true;
                cmbGreaterThanRoundingMode2.Enabled = true;

                if (UseLimitDateCategoryGreatherId2 == 1)
                {
                    txtGreaterThanSightOfBill2.Enabled = true;
                }
                else
                {
                    txtGreaterThanSightOfBill2.Enabled = false;
                    txtGreaterThanSightOfBill2.Clear();
                }
            }
        }

        private void cmbGreaterThanCollectCategoryId3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGreaterThanCollectCategoryId3.Text != "")
            {
                string selectedCodeGreatherId3 = cmbGreaterThanCollectCategoryId3.Text.ToString().Substring(0, 2);
                var code = GreatherThanCollectCategoryList.Where(c => c.Code == selectedCodeGreatherId3);
                var selectUseLimitDate = code.Select(x => x.UseLimitDate);
                UseLimitDateCategoryGreatherId3 = selectUseLimitDate.First();
            }

            if (cmbGreaterThanCollectCategoryId3.SelectedIndex != -1)
            {
                Category selected = cmbGreaterThanCollectCategoryId3.Items[cmbGreaterThanCollectCategoryId3.SelectedIndex].Tag as Category;
                GreaterThanCategoryIndex3 = selected.Id.ToString();
            }

            if (cmbGreaterThanCollectCategoryId3.Text == "")
            {
                nmbGreaterThanRate3.Value = null;
                cmbGreaterThanRoundingMode3.SelectedIndex = -1;
                txtGreaterThanSightOfBill3.Clear();

                nmbGreaterThanRate3.Enabled = false;
                cmbGreaterThanRoundingMode3.Enabled = false;
                txtGreaterThanSightOfBill3.Enabled = false;
            }
            else
            {
                nmbGreaterThanRate3.Enabled = true;
                cmbGreaterThanRoundingMode3.Enabled = true;

                if (UseLimitDateCategoryGreatherId3 == 1)
                {
                    txtGreaterThanSightOfBill3.Enabled = true;
                }
                else
                {
                    txtGreaterThanSightOfBill3.Enabled = false;
                    txtGreaterThanSightOfBill3.Clear();

                }
            }
        }

        private void cmbShareTransferFee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbShareTransferFee.Text == "自社")
            {
                cbxUseFeeLearning.Enabled = true;
                cbxUseFeeTolerance.Enabled = true;

                var exist = LoadedCustomer != null && (!(ReturnScreen is PI0201 || ReturnScreen is PI0205));
                BaseContext.SetFunction08Enabled(exist);
            }
            else
            {
                cbxUseFeeLearning.Checked = false;
                cbxUseFeeTolerance.Checked = false;

                cbxUseFeeLearning.Enabled = false;
                cbxUseFeeTolerance.Enabled = false;

                BaseContext.SetFunction08Enabled(false);
            }
        }

        private void cmbLessThanCollectCategoryId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLessThanCollectCategoryId.SelectedIndex != -1)
            {
                Category selected = cmbLessThanCollectCategoryId.Items[cmbLessThanCollectCategoryId.SelectedIndex].Tag as Category;
                LessThanCategoryIndex = selected.Id.ToString();
            }
        }
        #endregion

        #region 検索ダイアログ Click Event
        private void btnCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;
                var customer = this.ShowCustomerMinSearchDialog();
                if (customer == null) return;
                ProgressDialog.Start(ParentForm, LoadCustomerFromCodeSearchAsync(customer.Code), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task LoadCustomerFromCodeSearchAsync(string code)
        {
            await LoadCustomerAsync(code);
            var customer = LoadedCustomer;
            await SetCustomerData(customer);
        }


        private void btnStaffCode_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCode.Text = staff.Code;
                lblStaffName.Text = staff.Name;
                StaffId = staff.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentId.Text = department.Code;
                lblDepartmentName.Text = department.Name;
                DepartmentId1 = department.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnAccountTitle_Click(object sender, EventArgs e)
        {
            var accTitle = this.ShowAccountTitleSearchDialog();
            if (accTitle != null)
            {
                txtAccountTitleId.Text = accTitle.Code;
                lblAccountTitleName.Text = accTitle.Name;
                AccountTitleId1 = accTitle.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnDepartment2_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentId2.Text = department.Code;
                lblDepartmentName2.Text = department.Name;
                DepartmentId2 = department.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnAccountTitle2_Click(object sender, EventArgs e)
        {
            var accTitle = this.ShowAccountTitleSearchDialog();
            if (accTitle != null)
            {
                txtAccountTitleId2.Text = accTitle.Code;
                lblAccountTitleName2.Text = accTitle.Name;
                AccountTitleId2 = accTitle.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnDepartment3_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentId3.Text = department.Code;
                lblDepartmentName3.Text = department.Name;
                DepartmentId3 = department.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnAccountTitle3_Click(object sender, EventArgs e)
        {
            var accTitle = this.ShowAccountTitleSearchDialog();
            if (accTitle != null)
            {
                txtAccountTitleId3.Text = accTitle.Code;
                lblAccountTitleName3.Text = accTitle.Name;
                AccountTitleId3 = accTitle.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnDepartment4_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentId4.Text = department.Code;
                lblDepartmentName4.Text = department.Name;
                DepartmentId4 = department.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnAccountTitle4_Click(object sender, EventArgs e)
        {
            var accTitle = this.ShowAccountTitleSearchDialog();
            if (accTitle != null)
            {
                txtAccountTitleId4.Text = accTitle.Code;
                lblAccountTitleName4.Text = accTitle.Name;
                AccountTitleId4 = accTitle.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnDepartment5_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentId5.Text = department.Code;
                lblDepartmentName5.Text = department.Name;
                DepartmentId5 = department.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void btnAccountTitle5_Click(object sender, EventArgs e)
        {
            var accTitle = this.ShowAccountTitleSearchDialog();
            if (accTitle != null)
            {
                txtAccountTitleId5.Text = accTitle.Code;
                lblAccountTitleName5.Text = accTitle.Name;
                AccountTitleId5 = accTitle.Id;
                ClearStatusMessage();
            }
            //ChangeFlg = false;
        }

        private void nmbGreaterThanRate1_Click(object sender, EventArgs e)
        {
            if (nmbGreaterThanRate1.Value.HasValue)
            {
                decimal greaterRate1 = Convert.ToDecimal(nmbGreaterThanRate1.Value);
                nmbGreaterThanRate1.Value = Convert.ToDecimal(greaterRate1.ToString("0.#"));
                nmbGreaterThanRate1.SelectionStart = 0;
                nmbGreaterThanRate1.SelectionLength = nmbGreaterThanRate1.Text.Length;
            }
        }

        private void nmbGreaterThanRate2_Click(object sender, EventArgs e)
        {
            if (nmbGreaterThanRate2.Value.HasValue)
            {
                decimal greaterRate2 = Convert.ToDecimal(nmbGreaterThanRate2.Value);
                nmbGreaterThanRate2.Value = Convert.ToDecimal(greaterRate2.ToString("0.#"));
                nmbGreaterThanRate2.SelectionStart = 0;
                nmbGreaterThanRate2.SelectionLength = nmbGreaterThanRate2.Text.Length;
            }
        }

        private void nmbGreaterThanRate3_Click(object sender, EventArgs e)
        {
            if (nmbGreaterThanRate3.Value.HasValue)
            {
                decimal greaterRate3 = Convert.ToDecimal(nmbGreaterThanRate3.Value);
                nmbGreaterThanRate3.Value = Convert.ToDecimal(greaterRate3.ToString("0.#"));
                nmbGreaterThanRate3.SelectionStart = 0;
                nmbGreaterThanRate3.SelectionLength = nmbGreaterThanRate3.Text.Length;
            }
        }

        private void nmbRate_Click(object sender, EventArgs e)
        {
            if (nmbRate.Value.HasValue)
            {
                decimal rate1 = Convert.ToDecimal(nmbRate.Value);
                nmbRate.Value = Convert.ToDecimal(rate1.ToString("0.##"));
                nmbRate.SelectionStart = 0;
                nmbRate.SelectionLength = nmbRate.Text.Length;
            }
        }

        private void nmbRate2_Click(object sender, EventArgs e)
        {
            if (nmbRate2.Value.HasValue)
            {
                decimal rate2 = Convert.ToDecimal(nmbRate2.Value);
                nmbRate2.Value = Convert.ToDecimal(rate2.ToString("0.##"));
                nmbRate2.SelectionStart = 0;
                nmbRate2.SelectionLength = nmbRate2.Text.Length;
            }
        }

        private void nmbRate3_Click(object sender, EventArgs e)
        {
            if (nmbRate3.Value.HasValue)
            {
                decimal rate3 = Convert.ToDecimal(nmbRate3.Value);
                nmbRate3.Value = Convert.ToDecimal(rate3.ToString("0.##"));
                nmbRate3.SelectionStart = 0;
                nmbRate3.SelectionLength = nmbRate3.Text.Length;
            }
        }

        private void nmbRate4_Click(object sender, EventArgs e)
        {
            if (nmbRate4.Value.HasValue)
            {
                decimal rate4 = Convert.ToDecimal(nmbRate4.Value);
                nmbRate4.Value = Convert.ToDecimal(rate4.ToString("0.##"));
                nmbRate4.SelectionStart = 0;
                nmbRate4.SelectionLength = nmbRate4.Text.Length;
            }
        }

        private void nmbRate5_Click(object sender, EventArgs e)
        {
            if (nmbRate5.Value.HasValue)
            {
                decimal rate5 = Convert.ToDecimal(nmbRate5.Value);
                nmbRate5.Value = Convert.ToDecimal(rate5.ToString("0.##"));
                nmbRate5.SelectionStart = 0;
                nmbRate5.SelectionLength = nmbRate5.Text.Length;
            }
        }
        #endregion

        #region クリア処理
        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            Clear();
        }

        private void Clear()
        {
            // Tab1
            txtCustomerCode.Clear();
            txtCustomerName.Clear();
            txtCustomerKana.Clear();
            txtParentCustomerCode.Clear();
            txtParentCustomerName.Clear();
            cmbShareTransferFee.SelectedIndex = -1;
            txtStaffCode.Clear();
            lblStaffName.Clear();
            txtClosingDay.Clear();
            txtCollectOffsetMonth.Clear();
            txtCollectOffsetDay.Clear();
            cmbHolidayFlag.SelectedIndex = 0;
            cmbCollectCategoryId.SelectedIndex = -1;
            txtSightOfBill.Clear();
            nmbThresholdValue.Clear();
            cmbLessThanCollectCategoryId.SelectedIndex = -1;
            cmbGreaterThanCollectCategoryId1.SelectedIndex = -1;
            nmbGreaterThanRate1.Clear();
            cmbGreaterThanRoundingMode1.SelectedIndex = -1;
            txtGreaterThanSightOfBill1.Clear();
            cmbGreaterThanCollectCategoryId2.SelectedIndex = -1;
            nmbGreaterThanRate2.Clear();
            cmbGreaterThanRoundingMode2.SelectedIndex = -1;
            txtGreaterThanSightOfBill2.Clear();
            cmbGreaterThanCollectCategoryId3.SelectedIndex = -1;
            nmbGreaterThanRate3.Clear();
            cmbGreaterThanRoundingMode3.SelectedIndex = -1;
            txtGreaterThanSightOfBill3.Clear();
            txtDensaiCode.Clear();
            txtCreditCode.Clear();
            nmbCreditLimit.Clear();
            txtCreditRank.Clear();
            txtCollationKey.Clear();

            cbxCustomerIsParent.Checked = false;
            cbxCustomerIsParent.Enabled = true;
            cbxUseFeeLearning.Checked = false;
            cbxUseFeeTolerance.Checked = false;
            cbxUseFeeLearning.Enabled = false;
            cbxUseFeeTolerance.Enabled = false;
            cbxIssueBillEachTime.Checked = false;
            cbxPrioritizeMatchingIndividually.Checked = false;
            cbxExcludeInvoicePublish.Checked = false;
            cbxExcludeReminderPublish.Checked = false;

            // Tab2
            txtExclusiveBankCode.Clear();
            txtExclusiveBankName.Clear();
            txtExclusiveBankName.Clear();
            txtExclusiveBankName.Clear();
            txtExclusiveBranchCode.Clear();
            txtExclusiveAccountNumber.Clear();
            txtExclusiveBranchName.Clear();
            txtAccountNumber.Clear();
            cmbExclusiveAccountTypeId.SelectedIndex = -1;
            txtTransferBankCode.Clear();
            txtTransferBankName.Clear();
            txtTransferBrachCode.Clear();
            txtTransferBranchName.Clear();
            txtTransferAccountNumber.Clear();
            cmbTransferAccountTypeId.SelectedIndex = -1;
            txtTransferNewCode.Clear();
            txtTransferCustomerCode.Clear();
            txtTransferAccountName.Clear();
            mskPostalCode.Text = "";
            txtAddress1.Clear();
            txtAddress2.Clear();
            txtCustomerTel.Clear();
            txtCustomerFax.Clear();
            txtDestinationDepartmentName.Clear();
            txtCustomerStaffName.Clear();
            cmbHonorific.Clear();
            txtNote.Clear();

            // Tab3
            ClearCustomerDiscountItems();

            txtCustomerCode.Enabled = true;
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction08Enabled(false);

            txtSightOfBill.Enabled = false;

            nmbThresholdValue.Enabled = false;
            cmbLessThanCollectCategoryId.Enabled = false;

            cmbGreaterThanCollectCategoryId1.Enabled = false;
            nmbGreaterThanRate1.Enabled = false;
            cmbGreaterThanRoundingMode1.Enabled = false;
            txtGreaterThanSightOfBill1.Enabled = false;

            cmbGreaterThanCollectCategoryId2.Enabled = false;
            nmbGreaterThanRate2.Enabled = false;
            cmbGreaterThanRoundingMode2.Enabled = false;
            txtGreaterThanSightOfBill2.Enabled = false;

            cmbGreaterThanCollectCategoryId3.Enabled = false;
            nmbGreaterThanRate3.Enabled = false;
            cmbGreaterThanRoundingMode3.Enabled = false;
            txtGreaterThanSightOfBill3.Enabled = false;

            txtBankName1.Text = Company?.BankName1 ?? "";
            txtBranchName1.Text = Company?.BranchName1 ?? "";
            txtAccountType1.Text = Company?.AccountType1 ?? "";
            txtAccountNumber1.Text = Company?.AccountNumber1 ?? "";
            cbxReceiveAccountId1.Checked = true;

            txtBankName2.Text = Company?.BankName2 ?? "";
            txtBranchName2.Text = Company?.BranchName2 ?? "";
            txtAccountType2.Text = Company?.AccountType2 ?? "";
            txtAccountNumber2.Text = Company?.AccountNumber2 ?? "";
            cbxReceiveAccountId2.Checked = true;

            txtBankName3.Text = Company?.BankName3 ?? "";
            txtBranchName3.Text = Company?.BranchName3 ?? "";
            txtAccountType3.Text = Company?.AccountType3 ?? "";
            txtAccountNumber3.Text = Company?.AccountNumber3 ?? "";
            cbxReceiveAccountId3.Checked = true;

            txtSightOfBill.Enabled = false;
            nmbThresholdValue.DisplayFields.Clear();
            nmbThresholdValue.DisplayFields.AddRange("#,###,###,##0", "", "", "-", "");
            nmbThresholdValue.Fields.DecimalPart.MaxDigits = 0;
            nmbThresholdValue.Fields.DecimalPart.MinDigits = 0;
            nmbCreditLimit.DisplayFields.Clear();
            nmbCreditLimit.DisplayFields.AddRange("#,###,###,##0", "", "", "-", "");
            nmbCreditLimit.Fields.DecimalPart.MaxDigits = 0;
            nmbCreditLimit.Fields.DecimalPart.MinDigits = 0;

            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);

            ClearStatusMessage();
            txtCustomerCode.Focus();
            Modified = false;
        }

        /// <summary> Tab3にあるCustomerDiscountデータをクリア </summary>
        private void ClearCustomerDiscountItems()
        {
            nmbMinValue.Value = 1M;

            nmbRate.Clear();
            cmbRoundingMode.SelectedIndex = -1;
            txtDepartmentId.Clear();
            lblDepartmentName.Clear();
            txtAccountTitleId.Clear();
            lblAccountTitleName.Clear();
            txtSubCode.Clear();

            nmbRate2.Clear();
            cmbRoundingMode2.SelectedIndex = -1;
            txtDepartmentId2.Clear();
            lblDepartmentName2.Clear();
            txtAccountTitleId2.Clear();
            lblAccountTitleName2.Clear();
            txtSubCode2.Clear();

            nmbRate3.Clear();
            cmbRoundingMode3.SelectedIndex = -1;
            txtDepartmentId3.Clear();
            lblDepartmentName3.Clear();
            txtAccountTitleId3.Clear();
            lblAccountTitleName3.Clear();
            txtSubCode3.Clear();

            nmbRate4.Clear();
            cmbRoundingMode4.SelectedIndex = -1;
            txtDepartmentId4.Clear();
            lblDepartmentName4.Clear();
            txtAccountTitleId4.Clear();
            lblAccountTitleName4.Clear();
            txtSubCode4.Clear();

            nmbRate5.Clear();
            cmbRoundingMode5.SelectedIndex = -1;
            txtDepartmentId5.Clear();
            lblDepartmentName5.Clear();
            txtAccountTitleId5.Clear();
            lblAccountTitleName5.Clear();
            txtSubCode5.Clear();

        }
        #endregion

        #region 削除処理
        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                var validateTask = CheckCustomerIdForDelete();
                ProgressDialog.Start(ParentForm, validateTask, false, SessionKey);
                if (!validateTask.Result) return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var deltask = DeleteCustomer();
                ProgressDialog.Start(ParentForm, deltask, false, SessionKey);

                if (deltask.Result)
                {
                    Clear();
                    DispStatusMessage(MsgInfDeleteSuccess);
                }
                else
                    ShowWarningDialog(MsgErrDeleteError);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary> 削除するCustomerIdは他ののテーブルにあるかどうかを存在チェック</summary>
        private async Task<bool> CheckCustomerIdForDelete()
        {
            int deleteCustomerId = CustomerId;
            Action<string, string[]> errorHandler = (id, args) => ShowWarningDialog(id, args);

            var customerGroupDataValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerGroupMasterClient>();
                var result = await service.ExistCustomerAsync(SessionKey, deleteCustomerId);
                customerGroupDataValid = !result.Exist;
                if (!customerGroupDataValid)
                {
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "債権代表者マスター", lblCustomerCode.Text });
                }
            });
            if (!customerGroupDataValid) return false;

            var billingDataValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                var result = await service.ExistCustomerAsync(SessionKey, deleteCustomerId);
                billingDataValid = !result.Exist;
                if (!billingDataValid)
                {
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "請求データ", lblCustomerCode.Text });
                }
            });
            if (!billingDataValid) return false;

            var kanaHistoryDataValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<KanaHistoryCustomerMasterClient>();
                var result = await client.ExistCustomerAsync(SessionKey, deleteCustomerId);
                kanaHistoryDataValid = !result.Exist;
                if (!kanaHistoryDataValid)
                {
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "カナ学習履歴", lblCustomerCode.Text });
                }
            });
            if (!kanaHistoryDataValid) return false;

            var receiptDataValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<ReceiptServiceClient>();
                var result = await client.ExistCustomerAsync(SessionKey, deleteCustomerId);
                receiptDataValid = !result.Exist;
                if (!receiptDataValid)
                {
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "入金データ", lblCustomerCode.Text });
                }
            });
            if (!receiptDataValid) return false;

            var nettingDataValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<NettingServiceClient>();
                var result = await client.ExistCustomerAsync(SessionKey, deleteCustomerId);
                nettingDataValid = !result.Exist;
                if (!nettingDataValid)
                {
                    errorHandler.Invoke(MsgWngDeleteConstraint, new string[] { "入金予定相殺データ", lblCustomerCode.Text });
                }
            });
            if (!nettingDataValid) return false;

            return true;
        }

        private async Task<bool> DeleteCustomer()
        {
            CountResult deleteResult = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();

                deleteResult = await service.DeleteAsync(SessionKey, CustomerId);
            });
            return (deleteResult.ProcessResult.Result && deleteResult.Count > 0);
        }
        #endregion

        #region 印刷指示
        [OperationLog("印刷指示")]
        private void DoPrint()
        {
            var form = ApplicationContext.Create(nameof(PB0502));
            form.StartPosition = FormStartPosition.CenterParent;
            PB0502 screen = form.GetAll<PB0502>().FirstOrDefault();
            if (screen != null)
            {
                screen.ButtonKey = "F4";
            }
            var result = ApplicationContext.ShowDialog(ParentForm, form);
        }
        #endregion

        #region インポート処理
        [OperationLog("インポート")]
        private void Import()
        {
            try
            {
                int importFileType = 0;

                using (var form = ApplicationContext.Create(nameof(PB0504)))
                {
                    var screen = form.GetAll<PB0504>().First();
                    screen.InitializeParentForm("インポート方法の選択");

                    if (ApplicationContext.ShowDialog(ParentForm, form, true) != DialogResult.OK)
                    {
                        DispStatusMessage(MsgInfCancelProcess, "インポート");
                        return;
                    }
                    importFileType = screen.ImportFileType;
                    PatternNo = screen.PatternNo;
                }
                ImportSetting setting = null;
                var getSettingTask = Util.GetMasterImportSettingAsync(Login, importFileType);
                ProgressDialog.Start(ParentForm, getSettingTask, false, SessionKey);
                setting = getSettingTask.Result;

                switch (importFileType)
                {
                    case ImportFileType.Customer:
                        ImportCustomer(setting);
                        break;
                    case ImportFileType.CustomerFee:
                        ImportFee(setting);
                        break;
                    case ImportFileType.CustomerDiscount:
                        ImportDiscount(setting);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ImportCustomer(ImportSetting setting)
        {
            var importer = new CustomerImporter(Login, ApplicationControl);
            importer.PatternNo = PatternNo;

            ProgressDialog.Start(ParentForm, importer.InitializeAsync(), false, SessionKey);

            if (!importer.IsImporterSettingRegistered)
            {
                ShowWarningDialog(MsgErrNoSettingError);
                return;
            }

            setting.ImportFileName = importer.GetInitialFilePath();
            using (var form = ApplicationContext.Create(nameof(PH9907)))
            {
                var screen = form.GetAll<PH9907>().First();
                var result = screen.ConfirmImportSetting(ParentForm, setting);
                if (result != DialogResult.OK)
                {
                    DispStatusMessage(MsgInfCancelProcess, "インポート");
                    return;
                }
            }
            NLogHandler.WriteDebug(this, "インポート処理開始");
            ImportResult res = null;
            DialogResult pgres = DialogResult.None;
            try
            {
                pgres = ProgressDialog.Start(ParentForm, async (cancel, progress) =>
                {
                    res = await importer.ImportAsync(setting.ImportFileName, setting.ImportMode, setting.GetErrorLogPath());
                }, true, SessionKey);
            }
            catch (AggregateException ex)
            {
                NLogHandler.WriteErrorLog(this, ex.InnerException, SessionKey);
            }
            if (pgres == DialogResult.Cancel)
            {
                DispStatusMessage(MsgInfCancelProcess, "インポート");
            }
            else if (pgres == DialogResult.Abort)
            {
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
            else if (res != null)
            {
                var message = XmlMessenger.GetMessageInfo(res.GetMessageId());
                if (message.Category == MessageCategory.Information)
                {
                    DispStatusMessage(message);
                }
                else
                {
                    ShowWarningDialog(message.ID);
                }
            }
            NLogHandler.WriteDebug(this, "インポート処理終了");
        }

        private void ImportFee(ImportSetting importSetting) // , dlgMasterImport dlg
        {
            try
            {
                var definition = new CustomerRegistrationFeeFileDefinition(
                    new DataExpression(ApplicationControl));

                definition.CurrencyCodeField.Ignored = !UseForeignCurrency;
                definition.CustomerIdField.GetModelsByCode = val =>
                    {
                        Dictionary<string, Customer> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var customerService = factory.Create<CustomerMasterClient>();
                            CustomersResult result = customerService.GetByCode(SessionKey, CompanyId, val);

                            if (result.ProcessResult.Result)
                            {
                                product = result.Customers
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, Customer>();
                    };

                definition.CurrencyCodeField.GetModelsByCode = code =>
                    {
                        Dictionary<string, Currency> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var currencyService = factory.Create<CurrencyMasterClient>();
                            CurrenciesResult result = currencyService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result && result.Currencies.Any())
                            {
                                product = result.Currencies
                                .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, Currency>();
                    };

                var importer = definition.CreateImporter(m => new { m.CustomerCode, m.CurrencyCode, m.Fee });
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await GetFeeAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportFeeAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting, Clear);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<List<CustomerFee>> GetFeeAsync()
        {
            List<CustomerFee> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerFeeMasterClient>();
                CustomerFeeResult result = await service.GetForExportAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    list = new List<CustomerFee>(result.CustomerFee);
                }
            });

            return list ?? new List<CustomerFee>();
        }

        private async Task<ImportResult> RegisterForImportFeeAsync(UnitOfWork<CustomerFee> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerFeeMasterClient>();
                result = await service.ImportAsync(SessionKey, imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }

        private void ImportDiscount(ImportSetting importSetting) //, dlgMasterImport dlg
        {
            CsvImporter<CustomerDiscount, string> importer = null;

            try
            {
                var definition = new CustomerDiscountFileDefinition(new DataExpression(ApplicationControl));

                #region 得意先コード
                definition.CustomerIdField.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, Customer> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var customerService = factory.Create<CustomerMasterClient>();
                            CustomersResult result = customerService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.Customers
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, Customer>();
                    };
                #endregion

                #region 部門コード1
                definition.DepartmentId1Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, Department> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var departmentService = factory.Create<DepartmentMasterClient>();
                            DepartmentsResult result = departmentService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.Departments
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, Department>();
                    };
                #endregion

                #region 科目コード1
                definition.AccountTitleId1Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, AccountTitle> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var accountTitleService = factory.Create<AccountTitleMasterClient>();
                            AccountTitlesResult result = accountTitleService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.AccountTitles
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, AccountTitle>();
                    };
                #endregion

                #region 部門コード2
                definition.DepartmentId2Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, Department> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var departmentService = factory.Create<DepartmentMasterClient>();
                            DepartmentsResult result = departmentService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.Departments
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, Department>();
                    };
                #endregion

                #region 科目コード2
                definition.AccountTitleId2Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, AccountTitle> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var accountTitleService = factory.Create<AccountTitleMasterClient>();
                            AccountTitlesResult result = accountTitleService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.AccountTitles
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, AccountTitle>();
                    };
                #endregion

                #region 部門コード3
                definition.DepartmentId3Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, Department> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var departmentService = factory.Create<DepartmentMasterClient>();
                            DepartmentsResult result = departmentService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.Departments
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, Department>();
                    };
                #endregion

                #region 科目コード3
                definition.AccountTitleId3Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, AccountTitle> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var accountTitleService = factory.Create<AccountTitleMasterClient>();
                            AccountTitlesResult result = accountTitleService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.AccountTitles
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, AccountTitle>();
                    };
                #endregion

                #region 部門コード4
                definition.DepartmentId4Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, Department> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var departmentService = factory.Create<DepartmentMasterClient>();
                            DepartmentsResult result = departmentService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.Departments
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, Department>();
                    };
                #endregion

                #region 科目コード4
                definition.AccountTitleId4Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, AccountTitle> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var accountTitleService = factory.Create<AccountTitleMasterClient>();
                            AccountTitlesResult result = accountTitleService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.AccountTitles
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, AccountTitle>();
                    };
                #endregion

                #region 部門コード5
                definition.DepartmentId5Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, Department> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var departmentService = factory.Create<DepartmentMasterClient>();
                            DepartmentsResult result = departmentService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.Departments
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, Department>();
                    };
                #endregion

                #region 科目コード5
                definition.AccountTitleId5Field.GetModelsByCode
                    = code =>
                    {
                        Dictionary<string, AccountTitle> product = null;
                        ServiceProxyFactory.LifeTime(factory =>
                        {
                            var accountTitleService = factory.Create<AccountTitleMasterClient>();
                            AccountTitlesResult result = accountTitleService.GetByCode(SessionKey, CompanyId, code);

                            if (result.ProcessResult.Result)
                            {
                                product = result.AccountTitles
                                    .ToDictionary(c => c.Code);
                            }
                        });
                        return product ?? new Dictionary<string, AccountTitle>();
                    };
                #endregion

                #region その他
                decimal rate1 = 0M;
                decimal rate2 = 0M;
                decimal rate3 = 0M;
                decimal rate4 = 0M;
                decimal rate5 = 0M;

                #region 歩引率 1
                definition.Rate1Field.ValidateAdditional
                    = (val, param) =>
                    {
                        var reports = new List<WorkingReport>();

                        // 値チェック
                        foreach (var pair in val)
                        {
                            rate1 = pair.Value.Rate1 ?? 0;
                            rate2 = pair.Value.Rate2 ?? 0;
                            rate3 = pair.Value.Rate3 ?? 0;
                            rate4 = pair.Value.Rate4 ?? 0;
                            rate5 = pair.Value.Rate5 ?? 0;

                            decimal? totalRate = rate1 + rate2 + rate3 + rate4 + rate5;

                            if (totalRate > 100)
                            {
                                reports.Add(new WorkingReport
                                {
                                    LineNo = pair.Key,
                                    FieldNo = definition.Rate1Field.FieldIndex,
                                    FieldName = definition.Rate1Field.FieldName,
                                    Message = "歩引率の合計が100を超えるためインポートできません。",
                                });
                            }
                        }

                        return reports;
                    };
                #endregion

                #region 歩引率 2
                definition.Rate2Field.ValidateAdditional
                    = (val, param) =>
                    {
                        var reports = new List<WorkingReport>();

                        // 値チェック
                        foreach (var pair in val)
                        {
                            rate1 = pair.Value.Rate1 ?? 0;
                            rate2 = pair.Value.Rate2 ?? 0;
                            rate3 = pair.Value.Rate3 ?? 0;
                            rate4 = pair.Value.Rate4 ?? 0;
                            rate5 = pair.Value.Rate5 ?? 0;

                            decimal? totalRate = rate1 + rate2 + rate3 + rate4 + rate5;

                            if (totalRate > 100)
                            {
                                reports.Add(new WorkingReport
                                {
                                    LineNo = pair.Key,
                                    FieldNo = definition.Rate2Field.FieldIndex,
                                    FieldName = definition.Rate2Field.FieldName,
                                    Message = "歩引率の合計が100を超えるためインポートできません。",
                                });
                            }
                        }

                        return reports;
                    };
                #endregion

                #region 歩引率 3
                definition.Rate3Field.ValidateAdditional
                    = (val, param) =>
                    {
                        var reports = new List<WorkingReport>();

                        // 値チェック
                        foreach (var pair in val)
                        {
                            rate1 = pair.Value.Rate1 ?? 0;
                            rate2 = pair.Value.Rate2 ?? 0;
                            rate3 = pair.Value.Rate3 ?? 0;
                            rate4 = pair.Value.Rate4 ?? 0;
                            rate5 = pair.Value.Rate5 ?? 0;

                            decimal? totalRate = rate1 + rate2 + rate3 + rate4 + rate5;

                            if (totalRate > 100)
                            {
                                reports.Add(new WorkingReport
                                {
                                    LineNo = pair.Key,
                                    FieldNo = definition.Rate3Field.FieldIndex,
                                    FieldName = definition.Rate3Field.FieldName,
                                    Message = "歩引率の合計が100を超えるためインポートできません。",
                                });
                            }
                        }

                        return reports;
                    };
                #endregion

                #region 歩引率 4
                definition.Rate4Field.ValidateAdditional
                    = (val, param) =>
                    {
                        var reports = new List<WorkingReport>();

                        // 値チェック
                        foreach (var pair in val)
                        {
                            rate1 = pair.Value.Rate1 ?? 0;
                            rate2 = pair.Value.Rate2 ?? 0;
                            rate3 = pair.Value.Rate3 ?? 0;
                            rate4 = pair.Value.Rate4 ?? 0;
                            rate5 = pair.Value.Rate5 ?? 0;

                            decimal? totalRate = rate1 + rate2 + rate3 + rate4 + rate5;

                            if (totalRate > 100)
                            {
                                reports.Add(new WorkingReport
                                {
                                    LineNo = pair.Key,
                                    FieldNo = definition.Rate4Field.FieldIndex,
                                    FieldName = definition.Rate4Field.FieldName,
                                    Message = "歩引率の合計が100を超えるためインポートできません。",
                                });
                            }
                        }

                        return reports;
                    };
                #endregion

                #region 歩引率 5
                definition.Rate5Field.ValidateAdditional
                    = (val, param) =>
                    {
                        var reports = new List<WorkingReport>();

                        // 値チェック
                        foreach (var pair in val)
                        {
                            rate1 = pair.Value.Rate1 ?? 0;
                            rate2 = pair.Value.Rate2 ?? 0;
                            rate3 = pair.Value.Rate3 ?? 0;
                            rate4 = pair.Value.Rate4 ?? 0;
                            rate5 = pair.Value.Rate5 ?? 0;

                            decimal? totalRate = rate1 + rate2 + rate3 + rate4 + rate5;

                            if (totalRate > 100)
                            {
                                reports.Add(new WorkingReport
                                {
                                    LineNo = pair.Key,
                                    FieldNo = definition.Rate5Field.FieldIndex,
                                    FieldName = definition.Rate5Field.FieldName,
                                    Message = "歩引率の合計が100を超えるためインポートできません。",
                                });
                            }
                        }

                        return reports;
                    };
                #endregion
                #endregion

                importer = definition.CreateImporter(m => m.CustomerCode);
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await GetDiscountAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportDiscountAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting, Clear);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<List<CustomerDiscount>> GetDiscountAsync()
        {
            List<CustomerDiscount> customerDiscountList = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomerDiscountsResult result = await service.GetDiscountItemsAsync(SessionKey, new CustomerSearch() { CompanyId = CompanyId });

                if (result.ProcessResult.Result)
                {
                    customerDiscountList = result.CustomerDiscounts;
                }
            });

            return customerDiscountList ?? new List<CustomerDiscount>();
        }

        private async Task<ImportResult> RegisterForImportDiscountAsync(UnitOfWork<CustomerDiscount> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerDiscountMasterClient>();
                result = await service.ImportAsync(SessionKey, imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }

        #endregion

        #region エクスポート
        [OperationLog("エクスポート")]
        private void Export()
        {
            using (var form = ApplicationContext.Create(nameof(PB0502)))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                PB0502 screen = form.GetAll<PB0502>().FirstOrDefault();
                if (screen != null)
                {
                    screen.ButtonKey = "F6";
                }
                var result = ApplicationContext.ShowDialog(ParentForm, form);
            }
        }
        #endregion

        #region 取込設定
        [OperationLog("取込設定")]
        private void OpenImporterSetting()
        {
            using (var form = ApplicationContext.Create(nameof(PB0503)))
            {
                var result = ApplicationContext.ShowDialog(BaseForm, form);
            }
        }
        #endregion

        #region 登録手数料
        /// <summary> 登録手数料ダイアログのため </summary>
        [OperationLog("登録手数料")]
        private void OpenRegistrationFee()
        {
            using (var form = ApplicationContext.Create(nameof(PB0505)))
            {
                var screen = form.GetAll<PB0505>().First();
                screen.CustomerId = CustomerId;
                if (CustomerCode != null && ReturnScreen is PE0102)
                {
                    screen.RegFee = false;
                }
                else if (CustomerCode != null && ReturnScreen is PE0101 && (MenuAuthority == false))
                {
                    screen.RegFee = false;
                }
                screen.InitializeParentForm("登録手数料");

                var result = ApplicationContext.ShowDialog(BaseForm, form, true);
                if (CustomerCode != null && ReturnScreen is PE0101)
                {
                    ChangeFee = screen.PaymentChangeFee;
                }
            }
        }
        #endregion

        #region 終了
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            if (CustomerCode != null)
            {
                CustomerCode = null;
                ParentForm.DialogResult = ChangeFee ? DialogResult.OK : DialogResult.Cancel;
                return;
            }

            ParentForm.Close();
        }
        #endregion

        #region 都度請求

        /// <summary>
        /// 「都度請求」チェックボックスの状態変更
        /// </summary>
        private void cbxIssueBillEachTime_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxIssueBillEachTime.Checked)
            {
                txtClosingDay.Enabled = false;
                txtClosingDay.Text = "00";
                txtCollectOffsetMonth.Visible = false;
                txtCollectOffsetMonth.Text = "0";
                lblAfterMonth.Text = "請求日後";
                txtCollectOffsetDay.Clear();
                lblDay.Text = "日以内";
                lblLimitDay.Visible = false;
            }
            else
            {
                txtClosingDay.Enabled = true;
                txtClosingDay.Clear();
                txtCollectOffsetMonth.Visible = true;
                txtCollectOffsetMonth.Clear();
                lblAfterMonth.Text = "ヶ月後の";
                txtCollectOffsetDay.Clear();
                lblDay.Text = "日";
                lblLimitDay.Visible = true;
            }
        }

        #endregion 都度請求

        #region その他Function

        private string ConvertPayerName(string value)
            => string.IsNullOrEmpty(value)
            ? value
            : EbDataHelper.ConvertToPayerName(value, LegalPersonalities);


        /// <summary> その他画面のためForm設定 </summary>
        private void SetControlsCallingByOtherProcess()
        {

            if (MenuAuthority &&
               (ReturnScreen is PE0101 || ReturnScreen is PI0201 || ReturnScreen is PI0205))
            {
                BaseContext.SetFunction01Enabled(true);
            }
            else
            {
                EnableTab(this.tbpBaseConfig1, false);
                EnableTab(this.tbpBaseConfig2, false);
                if (UseDiscount)
                    EnableTab(this.tbpDiscount, false);

                BaseContext.SetFunction01Enabled(false);
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

        private void EnableTab(TabPage page, bool enable)
        {
            foreach (Control ctl in page.Controls) ctl.Enabled = enable;
        }

        private void SetOptionControls()
        {
            cbxExcludeInvoicePublish.Visible = UsePublishInvoice;
            cbxExcludeReminderPublish.Visible = UseReminder;

            if (!UsePublishInvoice)
            {
                cbxExcludeReminderPublish.Location = new System.Drawing.Point(cbxExcludeInvoicePublish.Location.X, cbxExcludeInvoicePublish.Location.Y);
                cbxExcludeReminderPublish.TabIndex = cbxExcludeInvoicePublish.TabIndex;
            }
        }

        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);

            txtCustomerCode.Format = expression.CustomerCodeFormatString;
            txtCustomerCode.MaxLength = expression.CustomerCodeLength;
            txtCustomerCode.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;

            txtStaffCode.Format = expression.StaffCodeFormatString;
            txtStaffCode.MaxLength = expression.StaffCodeLength;
            txtStaffCode.PaddingChar = expression.StaffCodePaddingChar;

            txtDepartmentId.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentId2.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentId3.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentId4.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentId5.MaxLength = expression.DepartmentCodeLength;

            txtAccountTitleId.MaxLength = expression.AccountTitleCodeLength;
            txtAccountTitleId2.MaxLength = expression.AccountTitleCodeLength;
            txtAccountTitleId3.MaxLength = expression.AccountTitleCodeLength;
            txtAccountTitleId4.MaxLength = expression.AccountTitleCodeLength;
            txtAccountTitleId5.MaxLength = expression.AccountTitleCodeLength;

            txtDepartmentId.Format = expression.DepartmentCodeFormatString;
            txtDepartmentId2.Format = expression.DepartmentCodeFormatString;
            txtDepartmentId3.Format = expression.DepartmentCodeFormatString;
            txtDepartmentId4.Format = expression.DepartmentCodeFormatString;
            txtDepartmentId5.Format = expression.DepartmentCodeFormatString;

            txtDepartmentId.PaddingChar = expression.DepartmentCodePaddingChar;
            txtDepartmentId2.PaddingChar = expression.DepartmentCodePaddingChar;
            txtDepartmentId3.PaddingChar = expression.DepartmentCodePaddingChar;
            txtDepartmentId4.PaddingChar = expression.DepartmentCodePaddingChar;
            txtDepartmentId5.PaddingChar = expression.DepartmentCodePaddingChar;

            txtAccountTitleId.Format = expression.AccountTitleCodeFormatString;
            txtAccountTitleId2.Format = expression.AccountTitleCodeFormatString;
            txtAccountTitleId3.Format = expression.AccountTitleCodeFormatString;
            txtAccountTitleId4.Format = expression.AccountTitleCodeFormatString;
            txtAccountTitleId5.Format = expression.AccountTitleCodeFormatString;

            txtAccountTitleId.PaddingChar = expression.AccountTitleCodePaddingChar;
            txtAccountTitleId2.PaddingChar = expression.AccountTitleCodePaddingChar;
            txtAccountTitleId3.PaddingChar = expression.AccountTitleCodePaddingChar;
            txtAccountTitleId4.PaddingChar = expression.AccountTitleCodePaddingChar;
            txtAccountTitleId5.PaddingChar = expression.AccountTitleCodePaddingChar;

            txtCustomerTel.Format = expression.TelFaxFormatString;
            txtCustomerFax.Format = expression.TelFaxFormatString;

        }

        private void AddHandlers()
        {
            foreach (var control in this.GetAll<Control>())
            {
                if (control is Common.Controls.VOneTextControl
                    || control is Common.Controls.VOneNumberControl
                    || control is Common.Controls.VOneMaskControl)
                {
                    control.TextChanged += OnContentChanged;
                }
                else if (control is Common.Controls.VOneComboControl)
                {
                    ((Common.Controls.VOneComboControl)control).SelectedIndexChanged += OnContentChanged;
                }
                else if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged += OnContentChanged;
                }
            }

        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void mskPostalCode_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate { SetMaskedTextBoxSelectAll((Common.Controls.VOneMaskControl)sender); });
        }

        private void SetMaskedTextBoxSelectAll(Common.Controls.VOneMaskControl txtbox)
        {
            mskPostalCode.SelectAll();
        }

        /// <summary>
        /// ｢締日｣｢回収予定(日)｣入力値の書式設定と値調整
        /// </summary>
        /// <param name="dayString">｢締日｣｢回収予定(日)｣入力値</param>
        /// <param name="adjust99">true:28～99を99に調整，false:調整なし</param>
        /// <returns></returns>
        private string GetFormattedClosingDay(string dayString, bool adjust99 = true)
        {
            if (string.IsNullOrWhiteSpace(dayString))
            {
                return null;
            }

            var day = int.Parse(dayString);

            if (adjust99 && 28 <= day)
            {
                day = 99;
            }

            return day.ToString("00");
        }

        #endregion その他Function

        #region TextChanged Event
        private void txtDay_TextChanged(object sender, EventArgs e)
        {
            var control = sender as Common.Controls.VOneTextControl;

            if (cbxIssueBillEachTime.Checked && control == txtCollectOffsetDay)
                return; // 都度請求時は0も入力可とする

            int value;
            if (!int.TryParse(control.Text, out value) || (value == 0))
                control.Clear();

        }
        #endregion

        #region Webサービス呼び出し

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


        private async Task LoadMenuAuthorityListAsync()
        {
            await ServiceProxyFactory.DoAsync(async (MenuAuthorityMasterService.MenuAuthorityMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, Login.UserId);
                MenuAuthority = (result?.ProcessResult.Result ?? false)
                ? result.MenuAuthorities.Any(x => x.MenuId == nameof(PB0501))
                : false;                
            });
        }

        private async Task<bool> HasChildCustomer(int ParentCustomerId)
            => await ServiceProxyFactory.DoAsync(async (CustomerGroupMasterClient client) =>
            {
                var result = await client.HasChildAsync(SessionKey, ParentCustomerId);
                return (result?.ProcessResult.Result ?? false) ? result.Exist : false;
            });

        private async Task<Customer> GetCustomerAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
            {
                if (string.IsNullOrEmpty(code)) return null;
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                    return result.Customers.FirstOrDefault();
                return null;
            });
        private async Task LoadCustomerAsync(string code)
            => LoadedCustomer = await GetCustomerAsync(code);

        private async Task LoadCollectCategoryInfoAsync()
        {
            var option = new CategorySearch();
            option.CompanyId = CompanyId;
            option.CategoryType = CategoryType.Collect;

            await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                {
                    CollectCategoryList = result.Categories;
                }
            });
        }

        private async Task LoadComboDataLessThanCollectCategoryInfoAsync()
        {
            int useLimitDate = 0;
            var option = new CategorySearch();
            option.CompanyId = CompanyId;
            option.CategoryType = CategoryType.Collect;
            option.UseLimitDate = useLimitDate;

            await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                {
                    LessThanCollectCategoryList = result.Categories;
                    var cus = LessThanCollectCategoryList.Where(x => x.Code != "00");
                    LessThanCollectCategoryList = cus.ToList();
                }
            });
        }

        private async Task LoadComboDataGreatherThanCollectCategoryInfoAsync()
        {
            var option = new CategorySearch();
            option.CompanyId = CompanyId;
            option.CategoryType = CategoryType.Collect;

            await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                {
                    GreatherThanCollectCategoryList = result.Categories;
                    var greatherThan = GreatherThanCollectCategoryList.Where(x => x.Code != "00");
                    GreatherThanCollectCategoryList = greatherThan.ToList();
                }
            });
        }

        private async Task<ImportSetting> GetMasterImportSettingAsync(int importFileType)
            => await ServiceProxyFactory.DoAsync(async (ImportSettingMasterClient client) =>
            {
                var getRes = await client.GetAsync(SessionKey, CompanyId, importFileType);
                if (getRes.ProcessResult.Result)
                    return getRes.ImportSetting;
                return null;
            });

        #endregion

    }
}
