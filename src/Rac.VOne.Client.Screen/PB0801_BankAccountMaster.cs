using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.BankAccountMasterService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Web.Models.FunctionType;
using Section = Rac.VOne.Web.Models.Section;

namespace Rac.VOne.Client.Screen
{
    /// <summary>銀行口座マスター / 銀行科目変換マスター</summary>
    public partial class PB0801 : VOneScreenBase
    {
        #region 変数宣言

        private List<BankAccount> BankAccountList { get; set; }
        private BankAccount CurrentAccount { get; set; }

        private const int ReceiptCategoryType = 2;

        private int BankId { get; set; } = 0;
        private int? SectionId { get; set; }
        private int? ReceiptCategoryId { get; set; }
        #endregion

        #region PB0801 銀行口座マスター
        public PB0801()
        {
            InitializeComponent();
            grdBankAccount.SetupShortcutKeys();
            Text = "銀行口座マスター";
            BankAccountList = new List<BankAccount>();
            AddHandlers();
        }
        #endregion

        #region PB0801 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction04Enabled(false);

            BaseContext.SetFunction05Caption("インポート");
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PB0801_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();

                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());

                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());

                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));

                Task<List<BankAccount>> loadListTask = LoadListAsync();
                loadTask.Add(loadListTask.ContinueWith(task => BankAccountList.AddRange(task.Result)));

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                InitializeGridTemplate();
                grdBankAccount.DataSource = new BindingSource(BankAccountList, null);

                ComboDataBind();
                DisplaySectionInput();
                txtCategoryCode.MaxLength = 2;
                txtCategoryCode.PaddingChar = '0';
                ClearAll();

                BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
                BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region grdBankAccount_CellDoubleClick
        private void grdBankAccount_CellDoubleClick(object sender, CellEventArgs e)
        {
            ClearStatusMessage();

            if (e.RowIndex >= 0)
            {
                var fill = true;

                if (Modified)
                    fill = ShowConfirmDialog(MsgQstConfirmUpdateData);

                if (fill)
                {
                    SetBankAccount(BankAccountList[e.RowIndex]);
                    Modified = false;
                    txtBankCode.Select();
                }
            }
        }
        #endregion

        #region txtCategoryCode_Validated
        private void txtCategoryCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();

                var categoryType = 2; // 入金区分
                var categoryCode = txtCategoryCode.Text;

                if (string.IsNullOrEmpty(categoryCode)
                    || string.IsNullOrWhiteSpace(categoryCode))
                {
                    lblCategoryName.Clear();
                    ReceiptCategoryId = null;
                    return;
                }

                Category category = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CategoryMasterClient>();
                    CategoriesResult result = await service.GetByCodeAsync(
                        Login.SessionKey,
                        Login.CompanyId,
                        categoryType, new string[] { categoryCode });

                    if (result.ProcessResult.Result && result.Categories.Any())
                    {
                        category = result.Categories[0];
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (category != null)
                {
                    txtCategoryCode.Text = category.Code;
                    lblCategoryName.Text = category.Name;
                    ReceiptCategoryId = category.Id;
                }
                else
                {
                    txtCategoryCode.Clear();
                    lblCategoryName.Clear();
                    ReceiptCategoryId = null;
                    ShowWarningDialog(MsgWngMasterNotExist, "入金区分", categoryCode);
                    txtCategoryCode.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region btnReceiptCategoryNameSearch_Click
        private void btnReceiptCategoryNameSearch_Click(object sender, EventArgs e)
        {
            var receiptCategory = this.ShowReceiptCategorySearchDialog();
            if (receiptCategory != null)
            {
                ClearStatusMessage();
                txtCategoryCode.Text = receiptCategory.Code;
                lblCategoryName.Text = receiptCategory.Name;
                ReceiptCategoryId = receiptCategory.Id;
            }
        }
        #endregion

        #region txtSectionCode_Validated
        private void txtSectionCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                var sectionCode = txtSectionCode.Text;

                if (string.IsNullOrEmpty(sectionCode)
                    || string.IsNullOrWhiteSpace(sectionCode))
                {
                    lblSectionName.Clear();
                    SectionId = null;
                    return;
                }

                Section section = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<SectionMasterClient>();
                    SectionsResult result = await service.GetByCodeAsync(
                        Login.SessionKey,
                        Login.CompanyId,
                        new string[] { sectionCode });

                    if (result.ProcessResult.Result && result.Sections.Any())
                    {
                        section = result.Sections[0];
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (section != null)
                {
                    txtSectionCode.Text = section.Code;
                    lblSectionName.Text = section.Name;
                    SectionId = section.Id;
                }
                else
                {
                    txtSectionCode.Clear();
                    lblSectionName.Clear();
                    SectionId = null;
                    ShowWarningDialog(MsgWngMasterNotExist, "入金部門", sectionCode);
                    txtSectionCode.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region btnSectionNameSearch_Click
        private void btnSectionNameSearch_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                ClearStatusMessage();
                txtSectionCode.Text = section.Code;
                lblSectionName.Text = section.Name;
                SectionId = section.Id;
            }
        }
        #endregion

        #region グリッド初期設定
        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                  , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 115, "BankCode"            , dataField: nameof(BankAccount.BankCode)           , cell: builder.GetTextBoxCell(middleCenter) , caption: "銀行コード"         ),
                new CellSetting(height, 125, "BankName"            , dataField: nameof(BankAccount.BankName)           , cell: builder.GetTextBoxCell()             , caption: "銀行名"             ),
                new CellSetting(height, 115, "BranchCode"          , dataField: nameof(BankAccount.BranchCode)         , cell: builder.GetTextBoxCell(middleCenter) , caption: "支店コード"         ),
                new CellSetting(height, 125, "BranchName"          , dataField: nameof(BankAccount.BranchName)         , cell: builder.GetTextBoxCell()             , caption: "支店名"             ),
                new CellSetting(height, 125, "AccountTypeName"     , dataField: nameof(BankAccount.AccountTypeName)    , cell: builder.GetTextBoxCell()             , caption: "預金種別"           ),
                new CellSetting(height, 130, "AccountNumber"       , dataField: nameof(BankAccount.AccountNumber)      , cell: builder.GetTextBoxCell(middleCenter) , caption: "口座番号"           ),
                new CellSetting(height, 125, "SectionName"         , dataField: nameof(BankAccount.SectionName)        , cell: builder.GetTextBoxCell()             , caption: "入金部門名"         ),
                new CellSetting(height,   0, "Id"                  , dataField: nameof(BankAccount.Id)                 ),
                new CellSetting(height,   0, "AccountTypeId"       , dataField: nameof(BankAccount.AccountTypeId)      ),
                new CellSetting(height,   0, "SectionId"           , dataField: nameof(BankAccount.SectionId)          ),
                new CellSetting(height,   0, "ReceiptCategoryId"   , dataField: nameof(BankAccount.ReceiptCategoryId)  ),
                new CellSetting(height,   0, "ReceiptCategoryCode" , dataField: nameof(BankAccount.ReceiptCategoryCode)),
                new CellSetting(height,   0, "ReceiptCategoryName" , dataField: nameof(BankAccount.ReceiptCategoryName))
            });

            HideSectionName(builder);
            grdBankAccount.Template = builder.Build();
        }
        #endregion

        #region グリッドのリスト設定
        private async Task<List<BankAccount>> LoadListAsync()
        {
            List<BankAccount> list = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankAccountMasterClient>();
                BankAccountsResult result = await service.GetItemsAsync(
                        Login.SessionKey,
                        Login.CompanyId,
                        new BankAccountSearch());

                if (result.ProcessResult.Result)
                {
                    list = result.BankAccounts;
                }
            });
            return list ?? new List<BankAccount>();
        }
        #endregion

        #region 入金部門名列表示・非表示制御
        private void HideSectionName(GcMultiRowTemplateBuilder builder)
        {
            if (!UseSection)
            {
                builder.Items[1].Width = 115; // 銀行コード
                builder.Items[2].Width = 155; // 銀行名
                builder.Items[3].Width = 115; // 支店コード
                builder.Items[4].Width = 155; // 支店名
                builder.Items[5].Width = 160; // 預金種別
                builder.Items[6].Width = 160; // 口座番号
                builder.Items[7].Width = 0;  // 入金部門名
            }
        }
        #endregion

        #region 入金部門コード表示・非表示制御
        private void DisplaySectionInput()
        {
            var expression = new DataExpression(ApplicationControl);

            if (UseSection)
            {
                txtSectionCode.MaxLength = expression.SectionCodeLength;
                txtSectionCode.Format = expression.SectionCodeFormatString;

                if (ApplicationControl?.SectionCodeType == 0)
                    txtSectionCode.PaddingChar = '0';
            }
            else
            {
                lblSectionCode.Visible = false;
                txtSectionCode.Visible = false;
                btnSectionNameSearch.Visible = false;
                lblSectionName.Visible = false;
            }
        }
        #endregion

        #region 預金種別コンボボックス設定
        private void ComboDataBind()
        {
            cmbAccountType.Items.Add(new ListItem("普通", 1));
            cmbAccountType.Items.Add(new ListItem("当座", 2));
            cmbAccountType.Items.Add(new ListItem("貯蓄", 4));
            cmbAccountType.Items.Add(new ListItem("通知", 5));

            if (UseForeignCurrency)
                cmbAccountType.Items.Add(new ListItem("外貨", 8));
        }
        #endregion

        #region 入力項目チェック
        private bool ValidateInput(bool isDelete = false)
        {
            if (!ValidateChildren())
                return false;

            if (string.IsNullOrEmpty(txtBankCode.Text)
                || string.IsNullOrWhiteSpace(txtBankCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "銀行コード");
                txtBankCode.Focus();
                return false;
            }

            if ((string.IsNullOrEmpty(txtBankName.Text)
                || string.IsNullOrWhiteSpace(txtBankName.Text))
                && !isDelete)
            {
                ShowWarningDialog(MsgWngInputRequired, "銀行名");
                txtBankName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtBranchCode.Text)
                || string.IsNullOrWhiteSpace(txtBranchCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "支店コード");
                txtBranchCode.Focus();
                return false;
            }

            if ((string.IsNullOrEmpty(txtBranchName.Text)
                || string.IsNullOrWhiteSpace(txtBranchName.Text))
                && !isDelete)
            {
                ShowWarningDialog(MsgWngInputRequired, "支店名");
                txtBranchName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(cmbAccountType.Text)
                || string.IsNullOrWhiteSpace(cmbAccountType.Text))
            {
                ShowWarningDialog(MsgWngSelectionRequired, "預金種別");
                cmbAccountType.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtAccountNumber.Text)
                || string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "口座番号");
                txtAccountNumber.Focus();
                return false;
            }

            if ((string.IsNullOrEmpty(txtCategoryCode.Text)
                || string.IsNullOrWhiteSpace(txtCategoryCode.Text))
                && !isDelete)
            {
                ShowWarningDialog(MsgWngInputRequired, "入金区分コード");
                txtCategoryCode.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region クリア処理
        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();

            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            ClearAll();
        }
        #endregion

        #region 全てクリア処理
        private void ClearAll()
        {
            ClearStatusMessage();
            SetBankAccount(null);
            ActiveControl = txtBankCode;
            Modified = false;
        }
        #endregion

        #region 終了処理
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            BaseForm.Close();
        }
        #endregion

        #region 入力項目へ設定処理
        private void SetBankAccount(BankAccount account)
        {
            CurrentAccount = account ?? new BankAccount();

            BankId = CurrentAccount.Id;
            txtBankCode.Text = CurrentAccount.BankCode;
            txtBankName.Text = CurrentAccount.BankName;
            txtBranchCode.Text = CurrentAccount.BranchCode;
            txtBranchName.Text = CurrentAccount.BranchName;

            if (account != null)
            {
                var accountType = CurrentAccount.AccountTypeId;
                switch (accountType)
                {
                    case 1:
                        cmbAccountType.SelectedIndex = 0;
                        break;

                    case 2:
                        cmbAccountType.SelectedIndex = 1;
                        break;

                    case 4:
                        cmbAccountType.SelectedIndex = 2;
                        break;

                    case 5:
                        cmbAccountType.SelectedIndex = 3;
                        break;
                }

                if (UseForeignCurrency && accountType == 8)
                    cmbAccountType.SelectedIndex = 4;
            }
            else
            {
                cmbAccountType.SelectedValue = "";
            }

            txtAccountNumber.Text = CurrentAccount.AccountNumber;

            ReceiptCategoryId = CurrentAccount.ReceiptCategoryId;
            txtCategoryCode.Text = CurrentAccount.ReceiptCategoryCode;
            lblCategoryName.Text = CurrentAccount.ReceiptCategoryName;

            SectionId = CurrentAccount.SectionId;
            txtSectionCode.Text = CurrentAccount.SectionCode;
            lblSectionName.Text = CurrentAccount.SectionName;


            if (account == null)
                cbxImportSkipping.Checked = false;
            else
                cbxImportSkipping.Checked = Convert.ToBoolean(
                    CurrentAccount.ImportSkipping);
        }
        #endregion

        #region 登録・削除用オブジェクトへ設定処理
        private BankAccount BankAccountInfo()
        {
            var bankAccount = new BankAccount();
            Model.CopyTo(CurrentAccount, bankAccount, true);

            bankAccount.Id = BankId;
            bankAccount.CompanyId = Login.CompanyId;
            bankAccount.BankCode = txtBankCode.Text;
            bankAccount.BranchCode = txtBranchCode.Text;
            bankAccount.AccountTypeId = Convert.ToInt32(cmbAccountType.SelectedItem.SubItems[1].Value);

            if (bankAccount.AccountTypeId != 0)
            {
                var accountType = bankAccount.AccountTypeId;
                switch (accountType)
                {
                    case 1:
                        bankAccount.AccountTypeName = "普通";
                        break;

                    case 2:
                        bankAccount.AccountTypeName = "当座";
                        break;

                    case 4:
                        bankAccount.AccountTypeName = "貯蓄";
                        break;

                    case 5:
                        bankAccount.AccountTypeName = "通知";
                        break;

                    case 8:
                        bankAccount.AccountTypeName = "外貨";
                        break;
                }
            }

            bankAccount.AccountNumber = txtAccountNumber.Text;
            bankAccount.BankName = txtBankName.Text.Trim();
            bankAccount.BranchName = txtBranchName.Text.Trim();
            bankAccount.ReceiptCategoryId = ReceiptCategoryId;
            bankAccount.ReceiptCategoryCode = txtCategoryCode.Text;
            bankAccount.ReceiptCategoryName = lblCategoryName.Text;
            bankAccount.SectionId = SectionId;
            bankAccount.SectionCode = txtSectionCode.Text;
            bankAccount.SectionName = lblSectionName.Text;

            bankAccount.ImportSkipping = Convert.ToInt32(cbxImportSkipping.Checked);
            bankAccount.CreateBy = Login.UserId;
            bankAccount.UpdateBy = Login.UserId;

            return bankAccount;
        }
        #endregion

        #region 既存口座データ取得処理
        private async Task<BankAccount> GetBankAccountAsync(BankAccount bankAccount)
        {
            BankAccount product = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankAccountMasterClient>();
                BankAccountResult result = await service.GetByCodeAsync(
                    Login.SessionKey,
                    Login.CompanyId,
                    bankAccount.BankCode,
                    bankAccount.BranchCode,
                    bankAccount.AccountTypeId,
                    bankAccount.AccountNumber);

                if (result.ProcessResult.Result)
                {
                    product = result.BankAccount;
                }
            });
            return product;
        }
        #endregion

        #region 登録処理
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!ValidateChildren())
                    return;

                if (!ValidateInput())
                    return;

                ZeroLeftPaddingWithoutValidated();

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var bankAccount = BankAccountInfo();
                bankAccount.Id = 0;

                BankAccountResult result = null;
                List<BankAccount> newList = null;
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    var org = await GetBankAccountAsync(bankAccount);
                    if (org != null)
                        bankAccount.Id = org.Id;

                    await ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<BankAccountMasterClient>();
                        result = await service.SaveAsync(
                            Login.SessionKey,
                            bankAccount);
                        if (result.ProcessResult.Result && result.BankAccount != null)
                        {
                            newList = await LoadListAsync();
                        }
                    });
                }), false, SessionKey);

                if (result.BankAccount != null)
                {
                    BankAccountList.Clear();
                    BankAccountList.AddRange(newList);
                    grdBankAccount.DataSource = new BindingSource(BankAccountList, null);
                    ClearAll();
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
        #endregion

        #region 削除処理
        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                if (!ValidateChildren())
                    return;

                if (!ValidateInput(isDelete: true))
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var bankCode = txtBankCode.Text;
                var branchCode = txtBranchCode.Text;
                int accountTypeId = Convert.ToInt32(cmbAccountType.SelectedItem.SubItems[1].Value);
                var accountNumber = txtAccountNumber.Text;

                CurrentAccount = BankAccountList.Find(b => (b.BankCode == bankCode)
                    && (b.BranchCode == branchCode)
                    && (b.AccountTypeId == accountTypeId)
                    && (b.AccountNumber == accountNumber));

                BankId = CurrentAccount != null ? CurrentAccount.Id : 0;

                CountResult deleteResult = new CountResult();
                List<BankAccount> newList = null;
                if (BankId > 0)
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<BankAccountMasterClient>();
                        deleteResult = await service.DeleteAsync(
                            Login.SessionKey,
                            BankId);

                        if (deleteResult.ProcessResult.Result && deleteResult.Count > 0)
                        {
                            newList = await LoadListAsync();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }

                if (deleteResult.Count > 0)
                {
                    BankAccountList.Clear();
                    BankAccountList.AddRange(newList);
                    grdBankAccount.DataSource = new BindingSource(BankAccountList, null);
                    ClearAll();
                    DispStatusMessage(MsgInfDeleteSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrDeleteError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region インポート処理
        [OperationLog("インポート")]
        private void Import()
        {
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.BankAccount);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new BankAccountFileDefinition(new DataExpression(ApplicationControl));
                definition.CategoryIdField.GetModelsByCode = val =>
                {
                    Dictionary<string, Category> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var categoryMaster = factory.Create<CategoryMasterClient>();
                        CategoriesResult result = categoryMaster.GetByCode(
                                Login.SessionKey, Login.CompanyId, 2, val);
                        if (result.ProcessResult.Result)
                        {
                            product = result.Categories
                                .ToDictionary(c => c.Code);
                        }
                    });
                    return product ?? new Dictionary<string, Category>();
                };
                definition.SectionIdField.Ignored = !UseSection;
                definition.SectionIdField.GetModelsByCode = val =>
                {
                    Dictionary<string, Section> product = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var sectionMaster = factory.Create<SectionMasterClient>();
                        SectionsResult result = sectionMaster.GetByCode(
                                Login.SessionKey, Login.CompanyId, val);
                        if (result.ProcessResult.Result)
                        {
                            product = result.Sections
                                .ToDictionary(c => c.Code);
                        }
                    });
                    return product ?? new Dictionary<string, Section>();
                };

                var importer = definition.CreateImporter(m => new
                {
                    m.BankCode,
                    m.BranchCode,
                    m.AccountTypeId,
                    m.AccountNumber
                });
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = Login.CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await LoadListAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);


                var importResult = DoImport(importer, importSetting, ClearAll);
                if (!importResult) return;

                BankAccountList.Clear();
                Task<List<BankAccount>> loadListTask = LoadListAsync();
                ProgressDialog.Start(ParentForm, loadListTask, false, SessionKey);
                BankAccountList.AddRange(loadListTask.Result);
                grdBankAccount.DataSource = new BindingSource(BankAccountList, null);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }
        #endregion

        #region インポートデータ登録処理
        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<BankAccount> imported)
        {
            ImportResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankAccountMasterClient>();
                result = await service.ImportAsync(Login.SessionKey,
                    imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }
        #endregion

        #region エクスポート処理
        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();
                List<BankAccount> list = null;
                string serverPath = null;

                var loadTask = LoadListAsync();
                var task = ServiceProxyFactory.LifeTime(async factory =>
                 {
                     var service = factory.Create<GeneralSettingMasterClient>();
                     GeneralSettingResult result = await service.GetByCodeAsync(
                             Login.SessionKey, Login.CompanyId, "サーバパス");

                     if (result.ProcessResult.Result)
                     {
                         serverPath = result.GeneralSetting?.Value;
                     }
                 });
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask, task), false, SessionKey);
                list = loadTask.Result;

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"銀行口座マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new BankAccountFileDefinition(new DataExpression(ApplicationControl));
                definition.SectionIdField.Ignored = !UseSection;
                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, list, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                DispStatusMessage(MsgInfFinishExport);
                Settings.SavePath<BankAccount>(Login, filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }
        #endregion

        #region 全入力項目変更イベント処理
        private void AddHandlers()
        {
            foreach (Control control in gbxBankAccountInput.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }

                if (control is Common.Controls.VOneComboControl)
                {
                    ((Common.Controls.VOneComboControl)control).SelectedIndexChanged +=
                        new EventHandler(OnContentChanged);
                }

                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }

                if (control is RadioButton)
                {
                    ((RadioButton)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }
        #endregion

        #region 組み込み版対応の前ゼロ埋め処理
        private void ZeroLeftPaddingWithoutValidated()
        {
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
            if (IsNeedValidate(0, txtCategoryCode.TextLength, txtCategoryCode.MaxLength))
            {
                txtCategoryCode.Text = ZeroLeftPadding(txtCategoryCode);
                txtCategoryCode_Validated(null, null);
            }
            if (UseSection && IsNeedValidate(ApplicationControl.SectionCodeType, txtSectionCode.TextLength, ApplicationControl.SectionCodeLength))
            {
                txtSectionCode.Text = ZeroLeftPadding(txtSectionCode);
                txtSectionCode_Validated(null, null);
            }
        }
        #endregion
    }
}