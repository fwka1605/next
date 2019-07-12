using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GridSettingMasterService;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.ReceiptExcludeService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金データ検索</summary>
    public partial class PD0501 : VOneScreenBase
    {
        public Func<IEnumerable<ITransactionData>, bool> SavePostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> ExcludePostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> DeletePostProcessor { get; set; }

        #region local properties
        private DataExpression Expression { get; set; }
        public VOneScreenBase ReturnScreen { get; set; }
        public Receipt CurrentReceipt { get; set; }
        private int? DialogLoginUserID { get; set; }
        private int GinkoJohoID { get; set; } = 0;
        private string BankBranchID { get; set; } = null;
        private int CustomerID1 { get; set; } = 0;
        private int CustomerID2 { get; set; } = 0;
        private int CurrencyID { get; set; } = 0;
        private int SectionID1 { get; set; } = 0;
        private int SectionID2 { get; set; } = 0;
        private int ReceiptCategoryID1 { get; set; } = 0;
        private int ReceiptCategoryID2 { get; set; } = 0;
        private int UseForeignCurrencyInt { get; set; }
        private int UseSectionInt { get; set; }
        private int NoOfpre { get; set; } = 0;
        private string ServerPath { get; set; } = null;
        private bool CellValueChanged { get; set; }
        private List<Receipt> Receipts { get; set; }
        List<object> search { get; set; }
        private Category CategoryResult { get; set; } = new Category();
        private List<Category> CategoryList { get; set; } = null;
        private ReceiptSearch ReceiptSearchCondition { get; set; } = null;
        private IEnumerable<string> LegalPersonalities { get; set; }
        private string AmountFormat { get; set; } = "#,###,###,###,##0";
        private string CellName(string value) => $"cel{value}";

        /// <summary>編集を行った行の背景色</summary>
        private Color ModifiedRowBackColor { get; } = Color.LightCyan;

        private bool IsGridModified
        { get { return grdReceiptSearch.Rows.Any(x => (x.DataBoundItem as Receipt)?.IsModified ?? false); } }

        private bool Editable
        { get { return ReturnScreen == null; } }

        /// <summary>入力区分 すべて</summary>
        private const int InputTypeAll = 0;
        /// <summary>入力区分 入金入力</summary>
        private const int InputTypeReceiptInput = 2;
        private List<GridSetting> GridSettingInfo { get; set; }
        #endregion

        #region Initial
        public PD0501()
        {
            InitializeComponent();
            grdReceiptSearch.SetupShortcutKeys();
            Text = "入金データ検索";
            InitializeHandlers();
        }
        private void InitializeHandlers()
        {
            tbcReceiptSearch.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcReceiptSearch.SelectedIndex == 0)
                {
                    if (ReturnScreen is PD0301)
                    {
                        BaseContext.SetFunction10Caption("戻る");
                        OnF10ClickHandler = Return;
                    }
                    else
                    {
                        BaseContext.SetFunction10Caption("終了");
                        OnF10ClickHandler = Close;
                    }
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
            txtSourceBankName.Validated += (sender, e) => txtSourceBankName.Text = EbDataHelper.ConvertToValidEbKana(txtSourceBankName.Text.Trim());
            txtSourceBranchName.Validated += (sender, e) => txtSourceBranchName.Text = EbDataHelper.ConvertToValidEbKana(txtSourceBranchName.Text.Trim());
        }
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("修正");
            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction05Caption("分割対象外");
            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction07Caption("一括対象外");
            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(true);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(true);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Search;
            OnF02ClickHandler = ConfirmToClear;
            OnF03ClickHandler = CallReceiptInput;
            OnF04ClickHandler = DoPrint;
            OnF05ClickHandler = ExcludeFromSplitting;
            OnF06ClickHandler = ExportData;
            OnF07ClickHandler = ExcludeAllFromSplitting;
            OnF08ClickHandler = SelectAll;
            OnF09ClickHandler = DeselectAll;
            OnF10ClickHandler = Exit;
        }


        [OperationLog("戻る")]
        private void Return()
        {
            Exit();
        }

        [OperationLog("終了")]
        private void Close()
        {
            Exit();
        }

        private void Exit()
        {
            try
            {
                Settings.SaveControlValue<PD0501>(Login, cbxCustomer.Name, cbxCustomer.Checked);
                Settings.SaveControlValue<PD0501>(Login, cbxSection.Name, cbxSection.Checked);
                Settings.SaveControlValue<PD0501>(Login, cbxReceiptCategory.Name, cbxReceiptCategory.Checked);
                Settings.SaveControlValue<PD0501>(Login, cbxUseReceiptSection.Name, cbxUseReceiptSection.Checked);
                Settings.SaveControlValue<PD0501>(Login, cbxReceiptAmount.Name, cbxReceiptAmount.Checked);

                if (IsGridModified
                    && !ShowConfirmDialog(MsgQstConfirmClose))
                    return;

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
            tbcReceiptSearch.SelectedIndex = 0;
        }


        private void PD0501_Load(object sender, EventArgs e)
        {
            SetScreenName();
            try
            {
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
                loadTask.Add(LoadCategoryFlagComboAsync());

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                if (Authorities == null)
                {
                    loadTask.Add(LoadFunctionAuthorities(FunctionType.ModifyReceipt));
                }

                loadTask.Add(LoadLegalPersonalities());

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                GetFormatData();
                AddedComboDatas();
                InitializeGrid();

                SuspendLayout();
                tbcReceiptSearch.SelectedIndex = 1;
                tbcReceiptSearch.SelectedIndex = 0;
                ResumeLayout();

                Clear();

                CheckBoxSetting();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            if (ReturnScreen is PD0301)
            {
                cmbReciptInputType.SelectedIndex = InputTypeReceiptInput;
                cmbReciptInputType.Enabled = false;
                cmbReceiptExcludeFlag.SelectedIndex = 0;
                cmbReceiptExcludeFlag.Enabled = false;
                cmbCategoryFlag.Enabled = false;

                BaseContext.SetFunction03Caption("選択");
                BaseContext.SetFunction10Caption("戻る");
                grdReceiptSearch.ReadOnly = true;
            }

            cmbAmountRange.SelectedIndex = 0;

            foreach (GridSetting gs in GridSettingInfo)
            {
                switch (gs.ColumnName)
                {
                    case nameof(Receipt.Note1):
                        lblNote1.Text = gs.ColumnNameJp;
                        break;

                    case nameof(Receipt.Note2):
                        lblNote2.Text = gs.ColumnNameJp;
                        break;

                    case nameof(Receipt.Note3):
                        lblNote3.Text = gs.ColumnNameJp;
                        break;

                    case nameof(Receipt.Note4):
                        lblNote4.Text = gs.ColumnNameJp;
                        break;
                }
            }
        }

        private void CheckBoxSetting()
        {
            Settings.SetCheckBoxValue<PD0501>(Login, cbxCustomer);
            Settings.SetCheckBoxValue<PD0501>(Login, cbxSection);
            Settings.SetCheckBoxValue<PD0501>(Login, cbxReceiptCategory);
            Settings.SetCheckBoxValue<PD0501>(Login, cbxUseReceiptSection);
            Settings.SetCheckBoxValue<PD0501>(Login, cbxReceiptAmount);
        }

        private void GetFormatData()
        {
            if (ApplicationControl != null)
            {
                Expression = new DataExpression(ApplicationControl);

                txtLoginUserCode.Format = Expression.LoginUserCodeFormatString;
                txtLoginUserCode.MaxLength = Expression.LoginUserCodeLength;

                if (ApplicationControl?.LoginUserCodeType == 0) txtLoginUserCode.PaddingChar = '0';
                if (ApplicationControl?.CustomerCodeType == 0) txtCustomerCodeFrom.PaddingChar = '0';
                if (ApplicationControl?.CustomerCodeType == 0) txtCustomerCodeTo.PaddingChar = '0';
                if (ApplicationControl?.SectionCodeType == 0) txtSectionCodeFrom.PaddingChar = '0';
                if (ApplicationControl?.SectionCodeType == 0) txtSectionCodeTo.PaddingChar = '0';

                txtCustomerCodeFrom.Format = Expression.CustomerCodeFormatString;
                txtCustomerCodeFrom.MaxLength = Expression.CustomerCodeLength;

                txtCustomerCodeTo.Format = Expression.CustomerCodeFormatString;
                txtCustomerCodeTo.MaxLength = Expression.CustomerCodeLength;

                var isKanaCode = ApplicationControl.CustomerCodeType == 2;
                txtCustomerCodeFrom.ImeMode = isKanaCode ? ImeMode.KatakanaHalf : ImeMode.Disable;
                txtCustomerCodeTo.ImeMode = isKanaCode ? ImeMode.KatakanaHalf : ImeMode.Disable;

                txtSectionCodeFrom.Format = Expression.SectionCodeFormatString;
                txtSectionCodeFrom.MaxLength = Expression.SectionCodeLength;

                txtSectionCodeTo.Format = Expression.SectionCodeFormatString;
                txtSectionCodeTo.MaxLength = Expression.SectionCodeLength;

                nmbReceiptAmountFrom.Fields.DecimalPart.MaxDigits = UseForeignCurrency ? 5 : 0;
                nmbReceiptAmountTo.Fields.DecimalPart.MaxDigits = UseForeignCurrency ? 5 : 0;

                //（桁数は、コントロールに応じて変更）
                if (UseForeignCurrency)
                {
                    nmbReceiptAmountFrom.DisplayFields.AddRange("###,###,###,##0.00000", "", "", "-", "");
                    nmbReceiptAmountTo.DisplayFields.AddRange("###,###,###,##0.00000", "", "", "-", "");
                }
                else
                {
                    nmbReceiptAmountFrom.DisplayFields.AddRange("###,###,###,##0", "", "", "-", "");
                    nmbReceiptAmountTo.DisplayFields.AddRange("###,###,###,##0", "", "", "-", "");
                }

                lblCurrencyCode.Visible = UseForeignCurrency;
                txtCurrencyCode.Visible = UseForeignCurrency;
                btnCurrency.Visible = UseForeignCurrency;

                lblSectionCode.Visible = UseSection;
                txtSectionCodeFrom.Visible = UseSection;
                btnSectionCodeFrom.Visible = UseSection;
                lblDisSectionCodeFrom.Visible = UseSection;
                lblSectionFromTo.Visible = UseSection;
                cbxSection.Visible = UseSection;
                txtSectionCodeTo.Visible = UseSection;
                btnSectionCodeTo.Visible = UseSection;
                lblDisSectionCodeTo.Visible = UseSection;
                cbxUseReceiptSection.Visible = UseSection;

                txtBankCode.PaddingChar = '0';
                txtBranchCode.PaddingChar = '0';
                txtAccountNumber.PaddingChar = '0';
                txtprivateBankCode.PaddingChar = '0';
                txtprivateBranchCode.PaddingChar = '0';
                txtprivateAccountNumber.PaddingChar = '0';
                txtReceiptBillBankCode.PaddingChar = '0';
                txtReceiptBillBranchCode.PaddingChar = '0';
                txtReceiptCategoryFrom.PaddingChar = '0';
                txtReceiptCategoryTo.PaddingChar = '0';
            }

        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            var middleCenter = MultiRowContentAlignment.MiddleCenter;

            foreach (var gridSetting in GridSettingInfo)
            {
                var cell = new CellSetting(height, gridSetting.DisplayWidth, gridSetting.ColumnName, caption: gridSetting.ColumnNameJp, dataField: gridSetting.ColumnName, sortable: true);
                if (gridSetting.ColumnName == nameof(Receipt.ExcludeFlag))
                {
                    cell.CellInstance = builder.GetCheckBoxCell();
                    cell.ReadOnly = false;
                    cell.SortDropDown = false;
                }
                if (gridSetting.ColumnName == "ExcludeCategory")
                {
                    cell.DataField = nameof(Receipt.ExcludeCategoryId);
                    cell.CellInstance = CreateDatabindingComboBoxCell(builder.GetComboBoxCell());
                    cell.ReadOnly = false;
                }
                if (gridSetting.ColumnName == "AssignmentState")
                {
                    cell.DataField = nameof(Receipt.AssignmentFlagName);
                    cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                }
                if (gridSetting.ColumnName == nameof(Receipt.RecordedAt)
                 || gridSetting.ColumnName == nameof(Receipt.DueAt)
                 || gridSetting.ColumnName == nameof(Receipt.BillDrawAt))
                {
                    cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                }
                if (gridSetting.ColumnName == nameof(Receipt.OutputAt))
                {
                    cell.CellInstance = builder.GetDateCell_yyyyMMddHHmmss();
                }
                if (gridSetting.ColumnName == nameof(Receipt.CustomerCode)
                 || gridSetting.ColumnName == nameof(Receipt.CurrencyCode)
                 || gridSetting.ColumnName == nameof(Receipt.SectionCode)
                 || gridSetting.ColumnName == nameof(Receipt.BankCode)
                 || gridSetting.ColumnName == nameof(Receipt.BranchCode)
                 || gridSetting.ColumnName == nameof(Receipt.BillBankCode)
                 || gridSetting.ColumnName == nameof(Receipt.BillBranchCode)
                 || gridSetting.ColumnName == nameof(Receipt.AccountNumber))
                {
                    cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                }
                if (gridSetting.ColumnName == "ReceiptCategoryName")
                {
                    cell.DataField = nameof(Receipt.CategoryCodeName);
                }
                if (gridSetting.ColumnName == "Memo")
                {
                    cell.DataField = nameof(Receipt.ReceiptMemo);
                }
                if (gridSetting.ColumnName == nameof(Receipt.InputType))
                {
                    cell.DataField = nameof(Receipt.InputTypeName);
                }
                if (gridSetting.ColumnName == "VirtualBranchCode")
                {
                    cell.DataField = nameof(Receipt.PayerCodePrefix);
                    cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                }
                if (gridSetting.ColumnName == "VirtualAccountNumber")
                {
                    cell.DataField = nameof(Receipt.PayerCodeSuffix);
                    cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                }
                if (gridSetting.ColumnName == nameof(Receipt.ReceiptAmount)
                 || gridSetting.ColumnName == nameof(Receipt.RemainAmount)
                 || gridSetting.ColumnName == nameof(Receipt.ExcludeAmount))
                {
                    cell.CellInstance = builder.GetTextBoxCurrencyCell(NoOfpre);
                }
                builder.Items.Add(cell);
            }
            grdReceiptSearch.Template = builder.Build();
            grdReceiptSearch.HideSelection = true;
            grdReceiptSearch.FreezeLeftCellName = CellName("ExcludeCategory");
            grdReceiptSearch.AllowAutoExtend = false;
        }
        #endregion

        #region InitComboBind
        private void AddedComboDatas()
        {
            var accountTypeList = new Dictionary<int, ListItem>
            {
                {0, new ListItem("すべて", 0) },
                {1, new ListItem("普通", 1) },
                {2, new ListItem("当座", 2) },
                {4, new ListItem("貯蓄", 4) },
                {5, new ListItem("通知", 5) },
                {8, new ListItem("外貨", 8) },
            };
            foreach (var pair in accountTypeList)
            {
                int accountTypeIndex = 0;
                accountTypeIndex = cmbAccoutType.Items.Add(pair.Value);
                cmbAccoutType.Items[accountTypeIndex].Tag = pair.Key;
            }

            cmbReciptInputType.Items.Add(new ListItem("すべて", 0));
            cmbReciptInputType.Items.Add(new ListItem("EB取込", 1));
            cmbReciptInputType.Items.Add(new ListItem("入力", 2));
            cmbReciptInputType.Items.Add(new ListItem("インポーター取込", 3));
            cmbReciptInputType.Items.Add(new ListItem("電債取込", 4));

            cmbAmountRange.Items.Add(new ListItem("入金額", 0));
            cmbAmountRange.Items.Add(new ListItem("入金残", 1));

            cmbReceiptExcludeFlag.Items.Add(new ListItem("通常入金", 0));
            cmbReceiptExcludeFlag.Items.Add(new ListItem("対象外入金", 1));
            cmbReceiptExcludeFlag.Items.Add(new ListItem("すべて", 2));
        }

        private async Task LoadGridSetting()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GridSettingMasterClient>();

                var result = await service.GetItemsAsync(
                        SessionKey,
                        CompanyId, Login.UserId, GridId.ReceiptSearch);

                if (result.ProcessResult.Result)
                {
                    GridSettingInfo = result.GridSettings;
                }
            });
        }
        private async Task LoadCategoryFlagComboAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();

                CategorySearch categorySearch = new CategorySearch();
                categorySearch.CompanyId = CompanyId;
                categorySearch.CategoryType = 4;

                var result = await service.GetItemsAsync(SessionKey, categorySearch);

                if (result.ProcessResult.Result)
                {
                    CategoryList = result.Categories;

                    cmbCategoryFlag.Items.Add(new ListItem("すべて", 0));

                    for (int i = 0; i < CategoryList.Count; i++)
                    {
                        cmbCategoryFlag.Items.Add(new ListItem(CategoryList[i].CodeAndName, CategoryList[i].Id));
                    }
                }
            });
        }

        private ComboBoxCell CreateDatabindingComboBoxCell(ComboBoxCell comboBoxCell)
        {
            CategoriesResult result = null;

            var option = new CategorySearch();
            option.CompanyId = CompanyId;
            option.CategoryType = 4;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();

                result = await service.GetItemsAsync(SessionKey, option);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                CategoryList = result.Categories;

                var comboSource = CategoryList.Select(c => new { c.Id, c.Name }).ToArray();
                comboBoxCell.DataSource = comboSource;
                comboBoxCell.DisplayMember = "Name";
                comboBoxCell.ValueMember = "Id";
            }

            return comboBoxCell;
        }

        /// <summary>法人格除去用</summary>
        private async Task LoadLegalPersonalities()
        {
            try
            {
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<JuridicalPersonalityMasterClient>();
                    var result = await client.GetItemsAsync(SessionKey, CompanyId);

                    if (result.ProcessResult.Result)
                    {
                        LegalPersonalities = result.JuridicalPersonalities.Select(x => x.Kana);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion

        #region SearchDialogClickFunction
        private void btn_saishukoshinsha_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                txtLoginUserCode.Text = loginUser.Code;
                lblDisLoginUserCode.Text = loginUser.Name;
                DialogLoginUserID = loginUser.Id;
                ClearStatusMessage();
            }
        }

        private void btn_tsuka_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                ClearStatusMessage();
                txtCurrencyCode.Text = currency.Code;
                CurrencyID = currency.Id;
                NoOfpre = currency.Precision;
                SetCurrencyDisplayString(NoOfpre);
            }
        }

        private void btn_GinkoJoho_Click(object sender, EventArgs e)
        {
            var bankAccount = this.ShowBankAccountSearchDialog();
            if (bankAccount != null)
            {
                txtBankCode.Text = bankAccount.BankCode;
                txtBranchCode.Text = bankAccount.BranchCode;
                cmbAccoutType.SelectedItem = cmbAccoutType.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.Tag == bankAccount.AccountTypeId);
                txtAccountNumber.Text = bankAccount.AccountNumber;
                GinkoJohoID = bankAccount.Id;
                ClearStatusMessage();
            }
        }

        private void btn_code_Click(object sender, EventArgs e)
        {
            var bankBranch = this.ShowBankBranchSearchDialog();
            if (bankBranch != null)
            {
                txtReceiptBillBankCode.Text = bankBranch.BankCode;
                txtReceiptBillBranchCode.Text = bankBranch.BranchCode;
                BankBranchID = bankBranch.BankCode;
                ClearStatusMessage();
            }
        }

        private void btn_CustomerCode1_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeFrom.Text = customer.Code;
                lblDisCustomerCodeFrom.Text = customer.Name;
                if (cbxCustomer.Checked)
                {
                    txtCustomerCodeTo.Text = customer.Code;
                    lblDisCustomerCodeTo.Text = customer.Name;
                }

                CustomerID1 = customer.Id;
                ClearStatusMessage();
            }
        }

        private void btn_CustomerCode2_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeTo.Text = customer.Code;
                lblDisCustomerCodeTo.Text = customer.Name;
                CustomerID2 = customer.Id;
                ClearStatusMessage();
            }
        }

        private void btn_SectionCode1_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                txtSectionCodeFrom.Text = section.Code;
                lblDisSectionCodeFrom.Text = section.Name;
                if (cbxCustomer.Checked)
                {
                    txtSectionCodeTo.Text = section.Code;
                    lblDisSectionCodeTo.Text = section.Name;
                }

                SectionID1 = section.Id;
                ClearStatusMessage();
            }
        }

        private void btn_SectionCode2_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                txtSectionCodeTo.Text = section.Code;
                lblDisSectionCodeTo.Text = section.Name;
                SectionID2 = section.Id;
                ClearStatusMessage();
            }
        }

        private void btn_ReceiptCategoryCode1_Click(object sender, EventArgs e)
        {
            var receiptCategory = this.ShowReceiptCategorySearchDialog();
            if (receiptCategory != null)
            {
                txtReceiptCategoryFrom.Text = receiptCategory.Code;
                lblDisReceiptCategoryFrom.Text = receiptCategory.Name;
                if (cbxReceiptCategory.Checked)
                {
                    txtReceiptCategoryTo.Text = receiptCategory.Code;
                    lblDisReceiptCategoryTo.Text = receiptCategory.Name;
                }

                ReceiptCategoryID1 = receiptCategory.Id;
                ClearStatusMessage();
            }
        }

        private void btn_ReceiptCategoryCode2_Click(object sender, EventArgs e)
        {
            var receiptCategory = this.ShowReceiptCategorySearchDialog();
            if (receiptCategory != null)
            {
                txtReceiptCategoryTo.Text = receiptCategory.Code;
                lblDisReceiptCategoryTo.Text = receiptCategory.Name;
                ReceiptCategoryID2 = receiptCategory.Id;
                ClearStatusMessage();
            }
        }
        #endregion

        #region FunctionKeyEvent
        [OperationLog("検索")]
        private void Search()
        {
            try
            {
                if (!ValidateChildren()) return;

                ClearStatusMessage();

                if (IsGridModified
                    && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                    return;

                if (!RequiredCheck())
                    return;

                ReceiptSearchCondition = SearchCondition();
                GetReceiptSearch(ReceiptSearchCondition);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            if (IsGridModified
                && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            Clear();
            CellValueChanged = false;
            tbcReceiptSearch.SelectedIndex = 0;
        }

        [OperationLog("修正")]
        private void CallReceiptInput()
        {
            try
            {
                if (grdReceiptSearch.RowCount == 0) return;

                var receipt = grdReceiptSearch.CurrentRow.DataBoundItem as Receipt;
                Receipt currentReceipt = null;

                ServiceProxyFactory.Do<ReceiptServiceClient>(client =>
                {
                    var result = client.Get(SessionKey, new long[] { receipt.Id });
                    if (result.ProcessResult.Result
                        && result.Receipts != null
                        && result.Receipts.Any(x => x != null))
                        currentReceipt = result.Receipts.FirstOrDefault();
                });

                if (currentReceipt == null)
                    return;

                // DBから値を取得して
                if (currentReceipt.AssignmentFlag != 0)
                {
                    ShowWarningDialog(MsgWngSelectDataNotEditable, "消込済", "修正");
                    return;
                }

                if (currentReceipt.ExcludeFlag == 1)
                {
                    ShowWarningDialog(MsgWngSelectDataNotEditable, "対象外", "修正");
                    return;
                }

                if (currentReceipt.OutputAt.HasValue)
                {
                    ShowWarningDialog(MsgWngSelectDataNotEditable, "仕訳出力済", "修正");
                    return;
                }

                if (currentReceipt.OriginalReceiptId.HasValue)
                {
                    ShowWarningDialog(MsgWngSelectDataNotEditable, "前受", "修正");
                    return;
                }

                var existAdvanceReceived = false;
                ServiceProxyFactory.Do<ReceiptServiceClient>(client =>
                {
                    var result = client.ExistOriginalReceipt(SessionKey, receipt.Id);
                    if (result.ProcessResult.Result)
                    {
                        existAdvanceReceived = result.Exist;
                    }
                });

                if (existAdvanceReceived)
                {
                    ShowWarningDialog(MsgWngSelectDataNotEditable, "前受振替済", "修正");
                    return;
                }

                ClearStatusMessage();

                if (ReturnScreen is PD0301)
                {
                    this.CurrentReceipt = receipt;
                    ParentForm.DialogResult = DialogResult.OK;
                }
                else
                {
                    var form = ApplicationContext.Create(nameof(PD0301));
                    var screen = form.GetAll<PD0301>().FirstOrDefault();
                    screen.CurrentReceipt = receipt;
                    screen.ReturnScreen = this;
                    form.StartPosition = FormStartPosition.CenterParent;
                    var result = ApplicationContext.ShowDialog(ParentForm, form);

                    if (result == DialogResult.OK)
                    {
                        GetReceiptSearch(ReceiptSearchCondition);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("印刷")]
        private void DoPrint()
        {
            try
            {
                if (grdReceiptSearch.RowCount > 0)
                {
                    ReceiptSearchSectionReport receiptReport = new ReceiptSearchSectionReport();
                    ReceiptSearchConditionSectionReport receiptConditionReport = new ReceiptSearchConditionSectionReport();

                    receiptReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    receiptReport.Name = "入金データ一覧" + DateTime.Today.ToString("yyyyMMdd");
                    receiptReport.SetData(Receipts, NoOfpre, UseSection, false, GridSettingInfo);

                    receiptConditionReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    receiptConditionReport.Name = "入金データ一覧" + DateTime.Today.ToString("yyyyMMdd");

                    ReceiptSearchCondition.PageCount = Receipts.Count / 13;

                    receiptConditionReport.SetPageDataSetting(search);

                    ProgressDialog.Start(ParentForm, Task.Run(() =>
                    {
                        receiptReport.Run(false);
                        receiptConditionReport.SetPageNumber(receiptReport.Document.Pages.Count);
                        receiptConditionReport.Run(false);

                    }), false, SessionKey);

                    receiptReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)receiptConditionReport.Document.Pages.Clone());

                    if (ServerPath == null)
                        LoadServerPath();

                    if (!Directory.Exists(ServerPath))
                        ServerPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    var result = ShowDialogPreview(ParentForm, receiptReport);

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

        [OperationLog("分割対象外")]
        private void ExcludeFromSplitting()
        {
            try
            {
                if (grdReceiptSearch.RowCount > 0)
                {
                    var RemainAmt = Convert.ToDecimal(grdReceiptSearch.CurrentRow.Cells[CellName(nameof(Receipt.RemainAmount))].Value);
                    var ExcludeAmt = Convert.ToDecimal(grdReceiptSearch.CurrentRow.Cells[CellName(nameof(Receipt.ExcludeAmount))].Value);
                    var RowTotalAmt = RemainAmt + ExcludeAmt;

                    if (RowTotalAmt == 0)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "消込済", "対象外処理");
                    }
                    else
                    {
                        var form = ApplicationContext.Create(nameof(PD0502));
                        PD0502 screen = form.GetAll<PD0502>().FirstOrDefault();

                        if (screen != null)
                        {
                            Receipt current = (Receipt)grdReceiptSearch.CurrentRow.DataBoundItem;
                            current.UseForeignCurrency = UseForeignCurrency ? 1 : 0;

                            screen.CurrentReceipt = current;
                            screen.NoOfpre = NoOfpre;
                            form.StartPosition = FormStartPosition.CenterParent;
                            ApplicationContext.ShowDialog(ParentForm, form);

                            GetReceiptSearch(ReceiptSearchCondition);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("エクスポート")]
        private void ExportData()
        {
            var source = grdReceiptSearch.DataSource as BindingSource;
            List<Web.Models.Receipt> list = source.DataSource as List<Receipt>;
            try
            {
                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                if (ServerPath == null)
                    LoadServerPath();

                if (!Directory.Exists(ServerPath))
                    ServerPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                var filePath = string.Empty;
                var fileName = $"入金データ{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(ServerPath, fileName, out filePath)) return;

                var definition = new ReceiptSearchFileDefinition(new DataExpression(ApplicationControl), GridSettingInfo);
                var decimalFormat = "0" + ((NoOfpre == 0) ? string.Empty : "." + new string('0', NoOfpre));
                definition.ReceiptAmountField.Format = value => value.ToString(decimalFormat);
                definition.RemainAmountField.Format = value => value.ToString(decimalFormat);
                definition.ExcludeAmountField.Format = value => value.ToString(decimalFormat);
                definition.SetFieldsSetting(GridSettingInfo, definition.ConvertSettingToField);
                definition.DeleteAtField.Ignored = true;

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
                Settings.SavePath<Web.Models.Receipt>(Login, filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        [OperationLog("一括対象外")]
        private void ExcludeAllFromSplitting()
        {
            ClearStatusMessage();

            try
            {
                grdReceiptSearch.EndEdit();
                if (!grdReceiptSearch.Rows.Any(x => (x.DataBoundItem as Receipt).IsModified))
                {
                    ShowWarningDialog(MsgWngNotExistUpdateData, "更新を行うデータ");
                    return;
                }

                if (!(ValidateInputValues())) return;

                if (!ShowConfirmDialog(MsgQstConfirmUpdate, ""))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var result = UpdateReceiptExcludes();
                if (!result) return;

                GetReceiptSearch(ReceiptSearchCondition);
                CellValueChanged = false;
                DispStatusMessage(MsgInfUpdateSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("全選択")]
        private void SelectAll()
        {
            if (!(CategoryList?.Any() ?? false))
            {
                ShowWarningDialog(MsgWngNotSettingMaster, "対象外区分マスター");
                return;
            }

            using (var form = ApplicationContext.Create(nameof(PD0503)))
            {
                var screen = form.GetAll<PD0503>().First();
                screen.InitializeParentForm("対象外区分選択");
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);
                if (dialogResult == DialogResult.OK)
                {
                    foreach (var row in grdReceiptSearch.Rows)
                    {
                        var receipt = row.DataBoundItem as Receipt;
                        if (receipt.CanEditable && row.Cells[CellName("ExcludeFlag")].Enabled)
                        {
                            receipt.ExcludeFlag = 1;
                            receipt.ExcludeCategoryId = screen.ExcludeCategoryId;
                            SetModifiedRow(row.Index);
                        }
                    }
                }
            }
            grdReceiptSearch.EndEdit();
            grdReceiptSearch.Focus();
            SetFunctionKeysEnabled();
            CalculateSum();
        }

        [OperationLog("全解除")]
        private void DeselectAll()
        {
            grdReceiptSearch.EndEdit();
            grdReceiptSearch.Focus();
            foreach (var row in grdReceiptSearch.Rows)
            {
                var receipt = row.DataBoundItem as Receipt;
                if (receipt.CanEditable)
                {
                    receipt.ExcludeFlag = 0;
                    SetModifiedRow(row.Index);
                }
            }
            SetFunctionKeysEnabled();
            CalculateSum();
        }
        #endregion

        #region 検証処理
        private bool ValidateInputValues()
        {
            foreach (var row in grdReceiptSearch.Rows)
            {
                var receipt = row.DataBoundItem as Receipt;
                if (receipt == null) continue;
                if (receipt.ExcludeFlag == 1
                    && !receipt.ExcludeCategoryId.HasValue)
                {
                    grdReceiptSearch.Focus();
                    grdReceiptSearch.CurrentCell = row[CellName("ExcludeCategory")];
                    ShowWarningDialog(MsgWngSelectionRequired, "対象外区分");
                    return false;
                }
            }
            return true;
        }
        private bool RequiredCheck()
        {
            if (UseForeignCurrency)
            {
                if (string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    txtCurrencyCode.Select();
                    ShowWarningDialog(MsgWngInputRequired, lblCurrencyCode.Text);
                    return false;
                }
            }

            if (!datReceiptAtFrom.ValidateRange(datReceiptAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblReceiptAt.Text))) return false;

            if (!datUpdateAtFrom.ValidateRange(datUpdateAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblUpdateAt.Text))) return false;

            if (!datBillDrawerAtFrom.ValidateRange(datBillDrawerAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblReceiptBillDrawerAt.Text))) return false;

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCode.Text))) return false;

            if (!txtSectionCodeFrom.ValidateRange(txtSectionCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblSectionCode.Text))) return false;

            if (!txtReceiptCategoryFrom.ValidateRange(txtReceiptCategoryTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblnyukinkubun.Text))) return false;

            if (!nmbReceiptAmountFrom.ValidateRange(nmbReceiptAmountTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, cmbAmountRange.Text))) return false;

            return true;
        }

        private ReceiptSearch SearchCondition()
        {
            var options = new ReceiptSearch();
            search = new List<object>();

            options.LoginUserId = Login.UserId;

            #region 検索条件
            if (datReceiptAtFrom.Value.HasValue)
            {
                options.RecordedAtFrom = datReceiptAtFrom.Value;
            }
            if (datReceiptAtTo.Value.HasValue)
            {
                options.RecordedAtTo = datReceiptAtTo.Value;
            }
            if (!string.IsNullOrEmpty(txtPayerName.Text))
            {
                options.PayerName = txtPayerName.Text;
            }

            if (datUpdateAtFrom.Value.HasValue)
            {
                options.UpdateAtFrom = datUpdateAtFrom.Value;
            }
            if (datUpdateAtTo.Value.HasValue)
            {
                options.UpdateAtTo = datUpdateAtTo.Value;
            }

            if (!string.IsNullOrEmpty(txtLoginUserCode.Text))
            {
                options.UpdateBy = DialogLoginUserID;
            }

            options.UseForeignCurrencyFlg = UseForeignCurrency ? 1 : 0;
            if (UseForeignCurrency)
            {
                if (!string.IsNullOrEmpty(txtCurrencyCode.Text))
                {
                    options.CurrencyId = CurrencyID;
                }
            }

            if (!string.IsNullOrEmpty(txtBankCode.Text))
            {
                options.BankCode = txtBankCode.Text;
            }

            if (!string.IsNullOrEmpty(txtBranchCode.Text))
            {
                options.BranchCode = txtBranchCode.Text;
            }

            if (cmbAccoutType.SelectedIndex != 0)
            {
                options.AccountTypeId = Convert.ToInt32(cmbAccoutType.SelectedItem.SubItems[1].Value);
            }

            if (!string.IsNullOrEmpty(txtAccountNumber.Text))
            {
                options.AccountNumber = txtAccountNumber.Text;
            }

            if (!string.IsNullOrEmpty(txtprivateBankCode.Text))
            {
                options.PrivateBankCode = txtprivateBankCode.Text;
            }

            if (!string.IsNullOrEmpty(txtprivateBranchCode.Text))
            {
                options.PayerCodePrefix = txtprivateBranchCode.Text;
            }

            if (!string.IsNullOrEmpty(txtprivateAccountNumber.Text))
            {
                options.PayerCodeSuffix = txtprivateAccountNumber.Text;
            }

            if (!string.IsNullOrEmpty(txtReceiptBillNumber.Text))
            {
                options.BillNumber = txtReceiptBillNumber.Text;
            }

            if (!string.IsNullOrEmpty(txtReceiptBillBankCode.Text))
            {
                options.BillBankCode = txtReceiptBillBankCode.Text;
            }

            if (!string.IsNullOrEmpty(txtReceiptBillBranchCode.Text))
            {
                options.BillBranchCode = txtReceiptBillBranchCode.Text;
            }

            if (datBillDrawerAtFrom.Value.HasValue)
            {
                options.BillDrawAtFrom = datBillDrawerAtFrom.Value;
            }
            if (datBillDrawerAtTo.Value.HasValue)
            {
                options.BillDrawAtTo = datBillDrawerAtTo.Value;
            }

            if (!string.IsNullOrEmpty(txtReceiptBillDrawer.Text))
            {
                options.BillDrawer = txtReceiptBillDrawer.Text;
            }

            if (!string.IsNullOrEmpty(txtCustomerCodeFrom.Text))
            {
                options.CustomerCodeFrom = txtCustomerCodeFrom.Text;
            }
            if (!string.IsNullOrEmpty(txtCustomerCodeTo.Text))
            {
                options.CustomerCodeTo = txtCustomerCodeTo.Text;
            }

            if (UseSection)
            {
                if (!string.IsNullOrEmpty(txtSectionCodeFrom.Text))
                {
                    options.SectionCodeFrom = txtSectionCodeFrom.Text;
                }
                if (!string.IsNullOrEmpty(txtSectionCodeTo.Text))
                {
                    options.SectionCodeTo = txtSectionCodeTo.Text;
                }
            }

            options.UseSectionMaster = cbxUseReceiptSection.Checked;

            options.ExistsMemo = cbxReceiptMemo.Checked ? 1 : 0;

            if (!string.IsNullOrEmpty(txtReceiptMemo.Text))
            {
                options.ReceiptMemo = txtReceiptMemo.Text;
            }

            if (cmbReciptInputType.SelectedIndex != 0)
            {
                options.InputType = cmbReciptInputType.SelectedIndex;
            }
            options.ReceiptCategoryCodeFrom = txtReceiptCategoryFrom.Text;
            options.ReceiptCategoryCodeTo = txtReceiptCategoryTo.Text;

            var assignments
                = (cbxNoAssignment.Checked   ? (int)AssignmentFlagChecked.NoAssignment   : (int)AssignmentFlagChecked.None)
                | (cbxPartAssignment.Checked ? (int)AssignmentFlagChecked.PartAssignment : (int)AssignmentFlagChecked.None)
                | (cbxFullAssignment.Checked ? (int)AssignmentFlagChecked.FullAssignment : (int)AssignmentFlagChecked.None);
            options.AssignmentFlag = assignments;
            var assignmentState = new List<string>();
            if (cbxNoAssignment.Checked)   assignmentState.Add(cbxNoAssignment.Text);
            if (cbxPartAssignment.Checked) assignmentState.Add(cbxPartAssignment.Text);
            if (cbxFullAssignment.Checked) assignmentState.Add(cbxFullAssignment.Text);
            var assignmentFlag = string.Join("、", assignmentState);

            int ExcludeFlag = 3;
            if (cmbReceiptExcludeFlag.SelectedIndex == 0
                || cmbReceiptExcludeFlag.SelectedIndex == 1
                || cmbReceiptExcludeFlag.SelectedIndex == 2)
            {
                options.ExcludeFlag = Convert.ToInt32(cmbReceiptExcludeFlag.SelectedIndex);
                ExcludeFlag = Convert.ToInt32(cmbReceiptExcludeFlag.SelectedIndex);
            }

            if (cmbCategoryFlag.SelectedIndex != -1)
            {
                var excludeId = Convert.ToInt32(cmbCategoryFlag.SelectedItem.SubItems[1].Value);
                if (excludeId > 0)
                    options.ExcludeCategoryId = excludeId;
            }

            string AmountType = "(指定なし)";
            var amountIndex = cmbAmountRange.SelectedIndex;
            if (cmbAmountRange.SelectedIndex != -1)
            {
                AmountType = cmbAmountRange.Text;
            }
            var FromAmount = "(指定なし)";
            if (nmbReceiptAmountFrom.Value.HasValue)
            {
                if (amountIndex == 0)
                    options.ReceiptAmountFrom = nmbReceiptAmountFrom.Value;
                else if (amountIndex == 1)
                    options.RemainAmountFrom = nmbReceiptAmountFrom.Value;

                FromAmount = nmbReceiptAmountFrom.Value.ToString();
            }
            var ToAmount = "(指定なし)";
            if (nmbReceiptAmountTo.Value.HasValue)
            {
                if (amountIndex == 0)
                    options.ReceiptAmountTo = nmbReceiptAmountTo.Value;
                else if (amountIndex == 1)
                    options.RemainAmountTo = nmbReceiptAmountTo.Value;
                ToAmount = nmbReceiptAmountTo.Value.ToString();
            }

            options.SourceBankName = txtSourceBankName.Text;
            options.SourceBranchName = txtSourceBranchName.Text;
            options.Note1 = txtNote1.Text;
            options.Note2 = txtNote2.Text;
            options.Note3 = txtNote3.Text;
            options.Note4 = txtNote4.Text;

            if (ReturnScreen is PD0301)
            {
                options.IsEditable = true;
            }

            #endregion

            #region 印刷のため検索条件設定

            var waveDash = " ～ ";
            var ReceiptFrom = datReceiptAtFrom.GetPrintValue();
            var ReceiptTo = datReceiptAtTo.GetPrintValue();

            var SectionCodeFrom = txtSectionCodeFrom.GetPrintValueCode(lblDisSectionCodeFrom);
            var SectionCodeTo = txtSectionCodeTo.GetPrintValueCode(lblDisSectionCodeTo);
            string UpdateFrom = datUpdateAtFrom.GetPrintValue();
            string UpdateTo = datUpdateAtTo.GetPrintValue();
            string receiptCategoryFrom = txtReceiptCategoryFrom.GetPrintValueCode(lblDisReceiptCategoryFrom);
            string receiptCategoryTo = txtReceiptCategoryTo.GetPrintValueCode(lblDisReceiptCategoryTo);
            string DrawerAtFrom = datBillDrawerAtFrom.GetPrintValue();
            string DrawerAtTo = datBillDrawerAtTo.GetPrintValue();
            string CustomerCodeFrom = txtCustomerCodeFrom.GetPrintValueCode(lblDisCustomerCodeFrom);
            string CustomerCodeTo = txtCustomerCodeTo.GetPrintValueCode(lblDisCustomerCodeTo);

            if (ApplicationControl.UseReceiptSection == 1 && ApplicationControl.UseForeignCurrency == 1)
            {
                search.Add(new SearchData(lblReceiptAt.Text, ReceiptFrom + waveDash + ReceiptTo,
                    lblSectionCode.Text, SectionCodeFrom + waveDash + SectionCodeTo));

                search.Add(new SearchData(lblSectionCode.Text, txtPayerName.GetPrintValue(),
                    cbxUseReceiptSection.Text, cbxUseReceiptSection.Checked ? "使用" : ""));

                search.Add(new SearchData(lblUpdateAt.Text, UpdateFrom + waveDash + UpdateTo,
                     cbxReceiptMemo.Text, cbxReceiptMemo.Checked ? "有り" : ""));

                search.Add(new SearchData(lblLoginUserCode.Text, txtLoginUserCode.GetPrintValueCode(lblDisLoginUserCode),
                    "入金メモ", txtReceiptMemo.GetPrintValue()));

                search.Add(new SearchData(lblCurrencyCode.Text, txtCurrencyCode.GetPrintValue(),
                    lblReceiptInputType.Text, cmbReciptInputType.GetPrintValue()));

                search.Add(new SearchData(lblPrivateBankCode.Text, txtBankCode.GetPrintValue(),
                    lblnyukinkubun.Text, receiptCategoryFrom + waveDash + receiptCategoryTo));

                search.Add(new SearchData(lblBranchCode.Text, txtBranchCode.GetPrintValue(),
                    lbl_keshigomukubun.Text, assignmentFlag));

                search.Add(new SearchData(lblAccountType.Text, cmbAccoutType.GetPrintValue(),
                    lblReceiptExcludeFlag.Text, cmbReceiptExcludeFlag.GetPrintValue()));

                search.Add(new SearchData(lblAccountNumber.Text, txtAccountNumber.GetPrintValue(),
                    lbl_CategoryFlag.Text, cmbCategoryFlag.GetPrintValue()));

                search.Add(new SearchData("専用銀行コード", txtprivateBankCode.GetPrintValue(),
                    lblAmountRange.Text, cmbAmountRange.GetPrintValue()));

                search.Add(new SearchData("専用支店コード", txtprivateBranchCode.GetPrintValue(),
                    "金額指定", nmbReceiptAmountFrom.GetPrintValue(AmountFormat) + waveDash + nmbReceiptAmountTo.GetPrintValue(AmountFormat)));

                search.Add(new SearchData("専用口座番号", txtprivateBranchCode.GetPrintValue(),
                    lblSourceBankName.Text, txtSourceBankName.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillNumber.Text, txtReceiptBillNumber.GetPrintValue(),
                    lblSourceBranchName.Text, txtSourceBranchName.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillBankCode.Text, txtReceiptBillBankCode.GetPrintValue(),
                    lblNote1.Text, txtNote1.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillBranchCode.Text, txtReceiptBillBranchCode.GetPrintValue(),
                    lblNote2.Text, txtNote2.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillDrawerAt.Text, DrawerAtFrom + waveDash + DrawerAtTo,
                     lblNote3.Text, txtNote3.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillDrawer.Text, txtReceiptBillDrawer.GetPrintValue(),
                     lblNote4.Text, txtNote4.GetPrintValue()));

                search.Add(new SearchData(lblCustomerCode.Text, CustomerCodeFrom + waveDash + CustomerCodeTo,
                    "", ""));
            }
            else if (ApplicationControl.UseReceiptSection == 0 && ApplicationControl.UseForeignCurrency == 1)
            {
                search.Add(new SearchData(lblReceiptAt.Text, ReceiptFrom + waveDash + ReceiptTo,
                    lblCustomerCode.Text, CustomerCodeFrom + waveDash + CustomerCodeTo));

                search.Add(new SearchData(lblSectionCode.Text, txtPayerName.GetPrintValue(),
                    cbxReceiptMemo.Text, cbxReceiptMemo.Checked ? "有り" : ""));

                search.Add(new SearchData(lblUpdateAt.Text, UpdateFrom + waveDash + UpdateTo,
                     "入金メモ", txtReceiptMemo.GetPrintValue()));

                search.Add(new SearchData(lblLoginUserCode.Text, txtLoginUserCode.GetPrintValueCode(lblDisLoginUserCode),
                    lblReceiptInputType.Text, cmbReciptInputType.GetPrintValue()));

                search.Add(new SearchData(lblCurrencyCode.Text, txtCurrencyCode.GetPrintValue(),
                    lblnyukinkubun.Text, receiptCategoryFrom + waveDash + receiptCategoryTo));

                search.Add(new SearchData(lblPrivateBankCode.Text, txtBankCode.GetPrintValue(),
                    lbl_keshigomukubun.Text, assignmentFlag));

                search.Add(new SearchData(lblBranchCode.Text, txtBranchCode.GetPrintValue(),
                    lblReceiptExcludeFlag.Text, cmbReceiptExcludeFlag.GetPrintValue()));

                search.Add(new SearchData(lblAccountType.Text, cmbAccoutType.GetPrintValue(),
                    lbl_CategoryFlag.Text, cmbCategoryFlag.GetPrintValue()));

                search.Add(new SearchData(lblAccountNumber.Text, txtAccountNumber.GetPrintValue(),
                    lblAmountRange.Text, cmbAmountRange.GetPrintValue()));

                search.Add(new SearchData("専用銀行コード", txtprivateBankCode.GetPrintValue(),
                    "金額指定", nmbReceiptAmountFrom.GetPrintValue(AmountFormat) + waveDash + nmbReceiptAmountTo.GetPrintValue(AmountFormat)));

                search.Add(new SearchData("専用支店コード", txtprivateBranchCode.GetPrintValue(),
                    lblSourceBankName.Text, txtSourceBankName.GetPrintValue()));

                search.Add(new SearchData("専用口座番号", txtprivateBranchCode.GetPrintValue(),
                    lblSourceBranchName.Text, txtSourceBranchName.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillNumber.Text, txtReceiptBillNumber.GetPrintValue(),
                    lblNote1.Text, txtNote1.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillBankCode.Text, txtReceiptBillBankCode.GetPrintValue(),
                    lblNote2.Text, txtNote2.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillBranchCode.Text, txtReceiptBillBranchCode.GetPrintValue(),
                    lblNote3.Text, txtNote3.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillDrawerAt.Text, DrawerAtFrom + waveDash + DrawerAtTo,
                     lblNote4.Text, txtNote4.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillDrawer.Text, txtReceiptBillDrawer.GetPrintValue(),
                     "", ""));

            }
            else if (ApplicationControl.UseReceiptSection == 1 && ApplicationControl.UseForeignCurrency == 0)
            {
                search.Add(new SearchData(lblReceiptAt.Text, ReceiptFrom + waveDash + ReceiptTo,
                    lblSectionCode.Text, SectionCodeFrom + waveDash + SectionCodeTo));

                search.Add(new SearchData(lblSectionCode.Text, txtPayerName.GetPrintValue(),
                    cbxUseReceiptSection.Text, cbxUseReceiptSection.Checked ? "使用" : ""));

                search.Add(new SearchData(lblUpdateAt.Text, UpdateFrom + waveDash + UpdateTo,
                     cbxReceiptMemo.Text, cbxReceiptMemo.Checked ? "有り" : ""));

                search.Add(new SearchData(lblLoginUserCode.Text, txtLoginUserCode.GetPrintValueCode(lblDisLoginUserCode),
                    "入金メモ", txtReceiptMemo.GetPrintValue()));

                search.Add(new SearchData(lblPrivateBankCode.Text, txtBankCode.GetPrintValue(),
                    lblReceiptInputType.Text, cmbReciptInputType.GetPrintValue()));

                search.Add(new SearchData(lblBranchCode.Text, txtBranchCode.GetPrintValue(),
                    lblnyukinkubun.Text, receiptCategoryFrom + waveDash + receiptCategoryTo));

                search.Add(new SearchData(lblAccountType.Text, cmbAccoutType.GetPrintValue(),
                    lbl_keshigomukubun.Text, assignmentFlag));

                search.Add(new SearchData(lblAccountNumber.Text, txtAccountNumber.GetPrintValue(),
                    lblReceiptExcludeFlag.Text, cmbReceiptExcludeFlag.GetPrintValue()));

                search.Add(new SearchData("専用銀行コード", txtprivateBankCode.GetPrintValue(),
                    lbl_CategoryFlag.Text, cmbCategoryFlag.GetPrintValue()));

                search.Add(new SearchData("専用支店コード", txtprivateBranchCode.GetPrintValue(),
                    lblAmountRange.Text, cmbAmountRange.GetPrintValue()));

                search.Add(new SearchData("専用口座番号", txtprivateBranchCode.GetPrintValue(),
                    "金額指定", nmbReceiptAmountFrom.GetPrintValue(AmountFormat) + waveDash + nmbReceiptAmountTo.GetPrintValue(AmountFormat)));

                search.Add(new SearchData(lblReceiptBillNumber.Text, txtReceiptBillNumber.GetPrintValue(),
                    lblSourceBankName.Text, txtSourceBankName.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillBankCode.Text, txtReceiptBillBankCode.GetPrintValue(),
                    lblSourceBranchName.Text, txtSourceBranchName.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillBranchCode.Text, txtReceiptBillBranchCode.GetPrintValue(),
                    lblNote1.Text, txtNote1.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillDrawerAt.Text, DrawerAtFrom + waveDash + DrawerAtTo,
                    lblNote2.Text, txtNote2.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillDrawer.Text, txtReceiptBillDrawer.GetPrintValue(),
                     lblNote3.Text, txtNote3.GetPrintValue()));

                search.Add(new SearchData(lblCustomerCode.Text, CustomerCodeFrom + waveDash + CustomerCodeTo,
                     lblNote4.Text, txtNote4.GetPrintValue()));
            }
            else
            {

                search.Add(new SearchData(lblReceiptAt.Text, ReceiptFrom + waveDash + ReceiptTo,
                    lblCustomerCode.Text, CustomerCodeFrom + waveDash + CustomerCodeTo));

                search.Add(new SearchData(lblSectionCode.Text, txtPayerName.GetPrintValue(),
                    cbxReceiptMemo.Text, cbxReceiptMemo.Checked ? "有り" : ""));

                search.Add(new SearchData(lblUpdateAt.Text, UpdateFrom + waveDash + UpdateTo,
                     "入金メモ", txtReceiptMemo.GetPrintValue()));

                search.Add(new SearchData(lblLoginUserCode.Text, txtLoginUserCode.GetPrintValueCode(lblDisLoginUserCode),
                    lblReceiptInputType.Text, cmbReciptInputType.GetPrintValue()));

                search.Add(new SearchData(lblPrivateBankCode.Text, txtBankCode.GetPrintValue(),
                    lblnyukinkubun.Text, receiptCategoryFrom + waveDash + receiptCategoryTo));

                search.Add(new SearchData(lblBranchCode.Text, txtBranchCode.GetPrintValue(),
                    lbl_keshigomukubun.Text, assignmentFlag));

                search.Add(new SearchData(lblAccountType.Text, cmbAccoutType.GetPrintValue(),
                    lblReceiptExcludeFlag.Text, cmbReceiptExcludeFlag.GetPrintValue()));

                search.Add(new SearchData(lblAccountNumber.Text, txtAccountNumber.GetPrintValue(),
                    lbl_CategoryFlag.Text, cmbCategoryFlag.GetPrintValue()));

                search.Add(new SearchData("専用銀行コード", txtprivateBankCode.GetPrintValue(),
                    lblAmountRange.Text, cmbAmountRange.GetPrintValue()));

                search.Add(new SearchData("専用支店コード", txtprivateBranchCode.GetPrintValue(),
                    "金額指定", nmbReceiptAmountFrom.GetPrintValue(AmountFormat) + waveDash + nmbReceiptAmountTo.GetPrintValue(AmountFormat)));

                search.Add(new SearchData("専用口座番号", txtprivateBranchCode.GetPrintValue(),
                    lblSourceBankName.Text, txtSourceBankName.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillNumber.Text, txtReceiptBillNumber.GetPrintValue(),
                    lblSourceBranchName.Text, txtSourceBranchName.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillBankCode.Text, txtReceiptBillBankCode.GetPrintValue(),
                    lblNote1.Text, txtNote1.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillBranchCode.Text, txtReceiptBillBranchCode.GetPrintValue(),
                    lblNote2.Text, txtNote2.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillDrawerAt.Text, DrawerAtFrom + waveDash + DrawerAtTo,
                    lblNote3.Text, txtNote3.GetPrintValue()));

                search.Add(new SearchData(lblReceiptBillDrawer.Text, txtReceiptBillDrawer.GetPrintValue(),
                     lblNote4.Text, txtNote4.GetPrintValue()));
            }
            #endregion
            return options;
        }

        public class SearchData
        {
            public string SearchName1 { get; set; }
            public string SearchValue1 { get; set; }
            public string SearchName2 { get; set; }
            public string SearchValue2 { get; set; }
            public SearchData() { }
            public SearchData(string Name1, string Value1, string Name2, string Value2)
            {
                SearchName1 = Name1;
                SearchValue1 = Value1;
                SearchName2 = Name2;
                SearchValue2 = Value2;
            }
        }

        private void GetReceiptSearch(ReceiptSearch recSearch)
        {
            recSearch.CompanyId = CompanyId;
            recSearch.LoginUserId = Login.UserId;

            ReceiptsResult result = null;

            var task = ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
            {
                result = await client.GetItemsAsync(SessionKey, recSearch);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                if (result.Receipts.Any())
                {
                    Receipts = result.Receipts;

                    InitializeGrid();

                    grdReceiptSearch.DataSource = new BindingSource(Receipts, null);

                    SearchDataBind();

                    EnableFunctionKeys(enabled: true);
                }
                else
                {
                    if (grdReceiptSearch.Rows.Count > 0)
                        grdReceiptSearch.Rows.Clear();

                    lblReceiptCount.Text = "0";
                    lblReceiptAmountTotal.Text = "0";
                    lblRemainAmountTotal.Text = 0.ToString(AmountFormat);
                    EnableFunctionKeys(enabled: false);
                    ShowWarningDialog(MsgWngNotExistSearchData);

                }
            }

            return;
        }

        private void SearchDataBind()
        {
            var dataCount = grdReceiptSearch.RowCount;
            var receiptTotal = 0M;
            var remainTotal = 0M;

            foreach (var row in grdReceiptSearch.Rows)
            {
                var receipt = row.DataBoundItem as Receipt;
                if ((receipt.RemainAmount + receipt.ExcludeAmount) != receipt.ReceiptAmount
                    || receipt.RecExcOutputAt == 1)
                {
                    row[0].Enabled = false;
                    row[1].Enabled = false;
                }
                if (receipt.ExcludeFlag == 0)
                {
                    row[1].Enabled = false;
                }
                receiptTotal += receipt.ReceiptAmount;
                remainTotal += receipt.RemainAmount;
            }
            lblReceiptCount.Text = dataCount.ToString("#,##0");
            lblReceiptAmountTotal.Text = receiptTotal.ToString(AmountFormat);
            lblRemainAmountTotal.Text = remainTotal.ToString(AmountFormat);

            tbcReceiptSearch.SelectedIndex = 1;

        }
        #endregion

        #region Function for Other Event
        private void LoadServerPath()
        {
            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var path = task.Result;
            if (!string.IsNullOrEmpty(path))
                ServerPath = path;
        }

        private void EnableFunctionKeys(bool enabled)
        {
            BaseContext.SetFunction03Enabled(enabled && Authorities[FunctionType.ModifyReceipt]);
            BaseContext.SetFunction04Enabled(enabled && Editable);
            BaseContext.SetFunction05Enabled(enabled && Editable);
            BaseContext.SetFunction06Enabled(enabled && Editable);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(enabled && Editable);
            BaseContext.SetFunction09Enabled(enabled && Editable);
        }

        public void Clear()
        {
            ClearStatusMessage();
            datReceiptAtFrom.Clear();
            datReceiptAtTo.Clear();
            txtPayerName.Clear();
            datUpdateAtFrom.Clear();
            datUpdateAtTo.Clear();
            txtLoginUserCode.Clear();
            lblDisLoginUserCode.Clear();
            txtCurrencyCode.Clear();
            txtBankCode.Clear();
            txtBranchCode.Clear();
            txtAccountNumber.Clear();
            txtprivateBankCode.Clear();
            txtprivateBranchCode.Clear();
            txtprivateAccountNumber.Clear();
            txtReceiptBillNumber.Clear();
            txtReceiptBillBankCode.Clear();
            txtReceiptBillBranchCode.Clear();
            txtReceiptBillDrawer.Clear();
            datBillDrawerAtFrom.Clear();
            datBillDrawerAtTo.Clear();
            txtCustomerCodeFrom.Clear();
            lblDisCustomerCodeFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblDisCustomerCodeTo.Clear();
            txtSectionCodeFrom.Clear();
            lblDisSectionCodeFrom.Clear();
            txtSectionCodeTo.Clear();
            lblDisSectionCodeTo.Clear();
            txtReceiptMemo.Clear();
            txtReceiptCategoryFrom.Clear();
            lblDisReceiptCategoryFrom.Clear();
            txtReceiptCategoryTo.Clear();
            lblDisReceiptCategoryTo.Clear();
            cmbCategoryFlag.Clear();
            nmbReceiptAmountFrom.Clear();
            nmbReceiptAmountTo.Clear();
            txtSourceBankName.Clear();
            txtSourceBranchName.Clear();
            txtNote1.Clear();
            txtNote2.Clear();
            txtNote3.Clear();
            txtNote4.Clear();
            cbxFullAssignment.Checked = false;
            cbxReceiptMemo.Checked = false;
            cbxPartAssignment.Checked = true;
            cbxNoAssignment.Checked = true;

            cmbAccoutType.SelectedIndex = 0;
            tbcReceiptSearch.SelectedIndex = 0;

            cmbReciptInputType.SelectedIndex = ReturnScreen is PD0301
                ? InputTypeReceiptInput : InputTypeAll;

            cmbReceiptExcludeFlag.SelectedIndex = 0;
            cmbAmountRange.SelectedIndex = cmbAmountRange.Items.Count > 0 ? 0 : -1;
            cmbCategoryFlag.Enabled = false;
            cmbCategoryFlag.SelectedItem = null;

            grdReceiptSearch.DataSource = null;
            lblReceiptCount.Clear();
            lblReceiptAmountTotal.Clear();
            lblRemainAmountTotal.Clear();

            DialogLoginUserID = null;

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            datReceiptAtFrom.Select();
        }

        private bool UpdateReceiptExcludes()
        {
            var updateItems = grdReceiptSearch.Rows.Select(x => x.DataBoundItem as Receipt)
                .Where(x => x.IsModified && x.CanEditable)
                .Select(x => new ReceiptExclude {
                        ReceiptId           = x.Id,
                        CreateBy            = Login.UserId,
                        UpdateBy            = Login.UserId,
                        ExcludeFlag         = x.ExcludeFlag,
                        ExcludeCategoryId   = x.ExcludeCategoryId,
                        ExcludeAmount       = x.ExcludeAmount,
                        ReceiptUpdateAt     = x.UpdateAt,
                    });

            var isPostProcessorImplemented = SavePostProcessor != null
                && ExcludePostProcessor != null
                && DeletePostProcessor != null;

            var deleteList = new List<ReceiptExclude>();
            var newItems = new List<ReceiptExclude>();
            var receiptItems = new List<Receipt>();
            var success = true;
            var syncResult = true;
            var updateValid = true;

            ProgressDialog.Start(ParentForm, async (cancel, progress) =>
            {
                if (isPostProcessorImplemented)
                {
                    await ServiceProxyFactory.DoAsync(async (ReceiptExcludeServiceClient client) =>
                    {
                        foreach (var item in updateItems)
                        {
                            var result = await client.GetByReceiptIdAsync(SessionKey, item.ReceiptId);
                            if (result.ProcessResult.Result
                                && result.ReceiptExcludes != null
                                && result.ReceiptExcludes.Any(x => x != null))
                            {
                                deleteList.AddRange(result.ReceiptExcludes);
                            }
                        }
                    });
                }
                if (success)
                {
                    await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
                    {
                        var saveResult = await client.SaveExcludeAmountAsync(SessionKey, updateItems.ToArray());
                        if (saveResult.ProcessResult.Result)
                        {
                            newItems.AddRange(saveResult.ReceiptExclude.Where(x => x.Id != 0));

                            if (isPostProcessorImplemented)
                            {
                                var receiptResult = await client.GetAsync(SessionKey, updateItems.Select(x => x.ReceiptId).ToArray());
                                receiptItems.AddRange(receiptResult.Receipts);
                            }
                        }
                        else
                        {
                            updateValid = !(saveResult.ProcessResult.ErrorCode == Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated);
                        }
                    });

                }
                if (success && isPostProcessorImplemented)
                {
                    if (DeletePostProcessor != null)
                        syncResult = DeletePostProcessor.Invoke(deleteList.Select(x => x as ITransactionData));
                    success &= syncResult;
                    if (!success) return;
                    if (ExcludePostProcessor != null)
                        syncResult = ExcludePostProcessor.Invoke(newItems.Select(x => x as ITransactionData));
                    success &= syncResult;
                    if (!success) return;
                    if (SavePostProcessor != null)
                        syncResult = SavePostProcessor.Invoke(receiptItems.Select(x => x as ITransactionData));
                    success &= syncResult;
                }

            }, false, SessionKey);
            if (!syncResult)
            {
                ShowWarningDialog(MsgErrPostProcessFailure);
                return false;
            }
            if (!updateValid)
            {
                ShowWarningDialog(MsgWngAlreadyUpdated);
                return false;
            }
            if (!success)
            {
                ShowWarningDialog(MsgErrUpdateError);
                return false;
            }
            return success;
        }

        private void SetCurrencyDisplayString(int displayScale)
        {
            var displayFieldString = "#,###,###,###,##0";
            string displayFormatString = "0";

            if (displayScale > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < displayScale; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }

            AmountFormat = displayFieldString;
        }

        private void SetFunctionKeysEnabled()
        {
            BaseContext.SetFunction04Enabled(!IsGridModified);
            BaseContext.SetFunction06Enabled(!IsGridModified);
            BaseContext.SetFunction07Enabled(IsGridModified);
        }

        private void CalculateSum()
        {
            var receiptAmount = 0M;
            var remainAmount = 0M;
            var count = grdReceiptSearch.RowCount;
            var source = grdReceiptSearch.DataSource as BindingSource;
            var list = source?.DataSource as List<Receipt>;
            if (list != null)
            {
                foreach (var receipt in list)
                {
                    if (receipt == null) continue;
                    receiptAmount += receipt.ReceiptAmount;
                    remainAmount += receipt.RemainAmount;
                }
            }

            lblReceiptCount.Text = count.ToString("#,##0");
            lblReceiptAmountTotal.Text = receiptAmount.ToString(AmountFormat);
            lblRemainAmountTotal.Text = remainAmount.ToString(AmountFormat);
        }
        #endregion

        #region 画面のためWebサービス機能
        private void getLoginUserDataByCode()
        {
            System.Action clearLoginUesrInfo = () =>
            {
                lblDisLoginUserCode.Text = null;
                DialogLoginUserID = null;
                ClearStatusMessage();
            };
            if (string.IsNullOrEmpty(txtLoginUserCode.Text))
            {
                clearLoginUesrInfo();
                return;
            }

            UsersResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<LoginUserMasterClient>();

                result = await service.GetByCodeAsync(
                    SessionKey,
                    CompanyId,
                    new string[] { txtLoginUserCode.Text });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.Users.Any())
            {
                ClearStatusMessage();
                Invoke(new System.Action(() =>
                {
                    var userData = result.Users[0];
                    lblDisLoginUserCode.Text = userData.Name;
                    DialogLoginUserID = userData.Id;
                }));
            }
            else
            {
                clearLoginUesrInfo();
                ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", txtLoginUserCode.Text);
                txtLoginUserCode.Clear();
                txtLoginUserCode.Select();
            }
        }

        private void GetCurrencyCode()
        {
            CurrenciesResult result = null;

            if (txtCurrencyCode.Text != "")
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CurrencyMasterClient>();

                    result = await service.GetByCodeAsync(
                            SessionKey,
                            CompanyId,
                            new string[] { txtCurrencyCode.Text });
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!result.Currencies.Any())
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);

                    txtCurrencyCode.Text = null;
                    CurrencyID = 0;
                    txtCurrencyCode.Focus();
                }
                else
                {
                    ClearStatusMessage();
                    CurrencyID = result.Currencies[0].Id;
                    NoOfpre = result.Currencies[0].Precision;
                    SetCurrencyDisplayString(NoOfpre);
                }
            }
            else
            {
                ClearStatusMessage();
                txtCurrencyCode.Text = null;
                CurrencyID = 0;
            }
        }

        private void getCustomerFromDataByCode()
        {
            string CustomerCode = txtCustomerCodeFrom.Text;

            // if Empty
            if (string.IsNullOrEmpty(CustomerCode)
                || string.IsNullOrWhiteSpace(CustomerCode))
            {
                if (cbxCustomer.Checked)
                {
                    txtCustomerCodeTo.Text = string.Empty;
                    lblDisCustomerCodeTo.Text = string.Empty;
                    CustomerID2 = 0;
                }
                lblDisCustomerCodeFrom.Text = string.Empty;
                CustomerID1 = 0;
                ClearStatusMessage();
                return;
            }

            Web.Models.CustomersResult result = GetCustomerData(CustomerCode);

            if (result.Customers.Any())
            {
                Web.Models.Customer userData = result.Customers[0];

                if (cbxCustomer.Checked)
                {
                    txtCustomerCodeFrom.Text = userData.Code;
                    lblDisCustomerCodeFrom.Text = userData.Name;
                    txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                    lblDisCustomerCodeTo.Text = userData.Name;
                    CustomerID2 = userData.Id;
                }
                else
                {
                    txtCustomerCodeFrom.Text = userData.Code;
                    lblDisCustomerCodeFrom.Text = userData.Name;
                }

                CustomerID1 = userData.Id;
            }
            else
            {
                if (cbxCustomer.Checked)
                {
                    txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                    lblDisCustomerCodeFrom.Clear();
                    lblDisCustomerCodeTo.Clear();
                }
                else
                {
                    lblDisCustomerCodeFrom.Clear();
                }
                CustomerID1 = 0;
                CustomerID2 = 0;
            }
            ClearStatusMessage();
        }

        private void getCustomerToDataByCode()
        {
            string CustomerCode = txtCustomerCodeTo.Text;

            // if Empty
            if (string.IsNullOrEmpty(CustomerCode))
            {
                lblDisCustomerCodeTo.Text = null;
                CustomerID2 = 0;
                ClearStatusMessage();
                return;
            }

            Web.Models.CustomersResult result = GetCustomerData(CustomerCode);

            if (result.Customers.Any())
            {

                Web.Models.Customer userData = result.Customers[0];
                txtCustomerCodeTo.Text = userData.Code;
                lblDisCustomerCodeTo.Text = userData.Name;
                CustomerID2 = userData.Id;
            }
            else
            {
                lblDisCustomerCodeTo.Clear();
                CustomerID2 = 0;
            }
            ClearStatusMessage();
        }

        private CustomersResult GetCustomerData(string customerCode)
        {
            CustomersResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();

                result = await service.GetByCodeAsync(SessionKey,
                    CompanyId, new string[] { customerCode });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return result;
        }

        private void getSectionCodeFromDataByCode()
        {
            string sectionCode = txtSectionCodeFrom.Text;

            if (string.IsNullOrEmpty(sectionCode)
                || string.IsNullOrWhiteSpace(sectionCode))
            {
                if (cbxSection.Checked)
                {
                    txtSectionCodeTo.Text = string.Empty;
                    lblDisSectionCodeTo.Text = string.Empty;
                    SectionID2 = 0;
                }
                lblDisSectionCodeFrom.Text = string.Empty;
                SectionID1 = 0;
                ClearStatusMessage();
                return;
            }

            List<Web.Models.Section> list = GetSectionData();

            Web.Models.Section result = null;
            result = list.Find(s => s.Code == sectionCode);

            if (result != null)
            {
                if (cbxSection.Checked)
                {
                    txtSectionCodeFrom.Text = result.Code;
                    lblDisSectionCodeFrom.Text = result.Name;
                    txtSectionCodeTo.Text = txtSectionCodeFrom.Text;
                    lblDisSectionCodeTo.Text = result.Name;
                    SectionID2 = result.Id;
                }
                else
                {
                    txtSectionCodeFrom.Text = result.Code;
                    lblDisSectionCodeFrom.Text = result.Name;
                }

                SectionID1 = result.Id;
            }
            else
            {
                if (cbxSection.Checked)
                {
                    txtSectionCodeTo.Text = txtSectionCodeFrom.Text;
                    lblDisSectionCodeFrom.Clear();
                    lblDisSectionCodeTo.Clear();
                }
                else
                {
                    lblDisSectionCodeFrom.Clear();
                }
                SectionID1 = 0;
                SectionID2 = 0;
            }
            ClearStatusMessage();
        }

        private void getSectionToDataByCode()
        {
            string sectionCode = txtSectionCodeTo.Text;

            // if Empty
            if (string.IsNullOrEmpty(sectionCode)
                || string.IsNullOrWhiteSpace(sectionCode))
            {
                lblDisSectionCodeTo.Text = null;
                SectionID2 = 0;
                ClearStatusMessage();
                return;
            }

            List<Web.Models.Section> list = GetSectionData();

            Web.Models.Section result = null;
            result = list.Find(s => s.Code == sectionCode);

            if (result != null)
            {
                txtSectionCodeTo.Text = result.Code;
                lblDisSectionCodeTo.Text = result.Name;
                SectionID2 = result.Id;
            }
            else
            {
                lblDisSectionCodeTo.Clear();
                SectionID2 = 0;
            }
            ClearStatusMessage();
        }

        private List<Web.Models.Section> GetSectionData()
        {
            List<Web.Models.Section> list = null;

            ServiceProxyFactory.Do<SectionMasterClient>(client =>
            {
                var task = client.GetByCodeAsync(SessionKey, CompanyId, Code: null)
                .ContinueWith(t =>
                {
                    if (t.Result.ProcessResult.Result && t.Result.Sections != null)
                        list = t.Result.Sections;

                    return t.Result;
                }, TaskScheduler.FromCurrentSynchronizationContext());

                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            });

            return list;
        }

        private void getCategoryFromDataByCode()
        {
            string CategoryCode = txtReceiptCategoryFrom.Text;

            // if Empty
            if (string.IsNullOrEmpty(CategoryCode)
                || string.IsNullOrWhiteSpace(CategoryCode))
            {
                if (cbxReceiptCategory.Checked)
                {
                    txtReceiptCategoryTo.Text = string.Empty;
                    lblDisReceiptCategoryTo.Text = string.Empty;
                    ReceiptCategoryID2 = 0;
                }
                lblDisReceiptCategoryFrom.Text = string.Empty;
                ReceiptCategoryID1 = 0;
                ClearStatusMessage();
                return;
            }

            Web.Models.CategoriesResult result = GetCategoryData(CategoryCode);

            if (result.Categories.Any())
            {
                Web.Models.Category CategoryResult = result.Categories.FirstOrDefault();

                if (CategoryResult != null)
                {
                    if (cbxReceiptCategory.Checked)
                    {
                        lblDisReceiptCategoryFrom.Text = CategoryResult.Name;
                        txtReceiptCategoryTo.Text = txtReceiptCategoryFrom.Text;
                        lblDisReceiptCategoryTo.Text = CategoryResult.Name;
                        ReceiptCategoryID2 = CategoryResult.Id;
                    }
                    else
                    {
                        lblDisReceiptCategoryFrom.Text = CategoryResult.Name;
                    }

                    ReceiptCategoryID1 = CategoryResult.Id;
                }
            }
            else
            {
                if (cbxReceiptCategory.Checked)
                {
                    txtReceiptCategoryTo.Text = txtReceiptCategoryFrom.Text;
                    lblDisReceiptCategoryFrom.Clear();
                    lblDisReceiptCategoryTo.Clear();
                }
                else
                {
                    lblDisReceiptCategoryFrom.Clear();
                }
                ReceiptCategoryID1 = 0;
                ReceiptCategoryID2 = 0;
            }
            ClearStatusMessage();
        }

        private void getCategoryToDataByCode()
        {
            string CategoryCode = txtReceiptCategoryTo.Text;

            // if Empty
            if (string.IsNullOrEmpty(CategoryCode)
                || string.IsNullOrWhiteSpace(CategoryCode))
            {
                lblDisReceiptCategoryTo.Text = null;
                ReceiptCategoryID2 = 0;
                ClearStatusMessage();
                return;
            }

            Web.Models.CategoriesResult result = GetCategoryData(CategoryCode);

            if (result.ProcessResult.Result)
            {
                CategoryResult = result.Categories.FirstOrDefault();

                if (CategoryResult != null)
                {
                    lblDisReceiptCategoryTo.Text = CategoryResult.Name;
                    ReceiptCategoryID2 = CategoryResult.Id;
                }
                else
                {
                    lblDisReceiptCategoryTo.Clear();
                    ReceiptCategoryID2 = 0;
                }
                ClearStatusMessage();
            }
        }

        private CategoriesResult GetCategoryData(string categoryCode)
        {
            CategoriesResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();

                result = await service.GetByCodeAsync(SessionKey,
                    CompanyId, 2, new[] { categoryCode });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return result;
        }
        #endregion

        #region Form Control Event
        private void txt_LoginUserCode_Validated(object sender, EventArgs e)
        {
            try
            {
                getLoginUserDataByCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_CurrencyCode_Validated(object sender, EventArgs e)
        {
            try
            {
                GetCurrencyCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_CustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                getCustomerFromDataByCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_CustomerCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                getCustomerToDataByCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_SectionCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                getSectionCodeFromDataByCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_SectionCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                getSectionToDataByCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_receiptCategoryFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                getCategoryFromDataByCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txt_receiptCategoryTo_Validated(object sender, EventArgs e)
        {
            try
            {
                getCategoryToDataByCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void nmb_FromReceiptAmount_Validated(object sender, EventArgs e)
        {
            if (cbxReceiptAmount.Checked)
            {
                nmbReceiptAmountTo.Value = nmbReceiptAmountFrom.Value;
            }
        }

        private void txtPayerName_Validated(object sender, EventArgs e)
        {
            txtPayerName.Text = EbDataHelper.ConvertToPayerName(txtPayerName.Text, LegalPersonalities);
        }

        private void cmb_ReceiptExcludeFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReceiptExcludeFlag.SelectedIndex == 1)
            {
                cmbCategoryFlag.Enabled = true;
                cmbCategoryFlag.SelectedIndex = 0;
            }
            else
            {
                cmbCategoryFlag.Enabled = false;
                cmbCategoryFlag.SelectedItem = null;
            }
        }

        private void grd_ReceiptSearch_CellDoubleClick(object sender, CellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                if (e.CellName == CellName(nameof(Receipt.ExcludeFlag))
                    || e.CellName == CellName("ExcludeCategory")) return;
                if (e.CellName == CellName("Memo"))
                {
                    var receipt = grdReceiptSearch.CurrentRow.DataBoundItem as Receipt;

                    if (receipt.AssignmentFlag > 0)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "消込済", "修正");
                        return;
                    }
                    using (var form = ApplicationContext.Create(nameof(PH9906)))
                    {
                        var screen = form.GetAll<PH9906>().First();
                        screen.Id = receipt.Id;
                        screen.MemoType = MemoType.ReceiptMemo;
                        screen.Memo = Convert.ToString(grdReceiptSearch.GetValue(e.RowIndex, "celMemo"));
                        screen.InitializeParentForm("入金メモ");
                        if (ApplicationContext.ShowDialog(ParentForm, form, true) == DialogResult.OK)
                        {
                            GetReceiptSearch(ReceiptSearchCondition);
                        }
                    }
                }
                else
                {
                    if (Authorities[FunctionType.ModifyReceipt])
                    {
                        CallReceiptInput();
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngPermissionDeniedForEdit);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grd_ReceiptSearch_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.RowIndex < 0
                || e.CellIndex < 0 || 1 < e.CellIndex) return;

            CellValueChanged = true;

            grdReceiptSearch.EndEdit();

            var row = grdReceiptSearch.CurrentRow;
            SetModifiedRow(row.Index, e.CellIndex == 0);
            SetFunctionKeysEnabled();
            CalculateSum();
        }

        /// <summary>該当行の判定</summary>
        /// <param name="row"></param>
        /// <param name="doValueChange"></param>
        /// <param name="excludeCategoryId">置き換える 対象外区分ID</param>
        private void SetModifiedRow(int index, bool doValueChange = true, int? excludeCategoryId = null)
        {
            Row row = grdReceiptSearch.Rows[index];
            var receipt = row.DataBoundItem as Receipt;

            var doExclude = receipt.ExcludeFlag == 1;
            row[CellName("ExcludeCategory")].Enabled = doExclude;

            if (doValueChange)
            {
                if (doExclude)
                {
                    receipt.ExcludeAmount += receipt.RemainAmount;
                    if (excludeCategoryId.HasValue && !receipt.ExcludeCategoryId.HasValue)
                        receipt.ExcludeCategoryId = excludeCategoryId;
                    receipt.RemainAmount = 0M;
                }
                else
                {
                    receipt.RemainAmount += receipt.ExcludeAmount;
                    receipt.ExcludeAmount = 0M;
                    receipt.ExcludeCategoryId = null;
                }
            }

            var backColor = receipt.IsModified ? ModifiedRowBackColor : Color.Empty;
            row.DefaultCellStyle.BackColor = backColor;
            row.DefaultCellStyle.DisabledBackColor = backColor;
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            var row = e.RowIndex;
            SetModifiedRow(row, e.CellIndex == 0);
            SetFunctionKeysEnabled();
            CalculateSum();
        }
        #endregion
    }
}
