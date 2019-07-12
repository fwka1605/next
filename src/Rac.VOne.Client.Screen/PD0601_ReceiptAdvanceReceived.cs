using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Common;
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
    /// <summary>前受一括振替処理</summary>
    public partial class PD0601 : VOneScreenBase
    {
        public Func<IEnumerable<ITransactionData>, bool> SavePostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> CancelPostProcessor { get; set; }
        private bool IsPostProcessorImplemented { get { return SavePostProcessor != null && CancelPostProcessor != null; } }
        private List<Receipt> ReceiptDataInfo { get; set; } = new List<Receipt>();
        private int CurrencyID { get; set; } = 0;
        private int NoOfpre { get; set; } = 0;
        private int CustomerId { get; set; } = 0;
        private int ReceiptCategoryId { get; set; } = 0;

        private string AmountFormat = "#,###,###,###,##0";
        private string CellName(string value) => $"cel{value}";
        private ColumnNameSettingsResult ColumnNameList { get; set; }
        private bool IsChecked(Row row) => Convert.ToBoolean(row[CellName("CheckBox")].Value);
        private bool IsGridModified
        {
            get
            {
                return grdAdvanceReceipt.Rows
                    .Any(x => ((x.DataBoundItem as Receipt)?.CustomerCode != (x.DataBoundItem as Receipt)?.CustomerCodeBuffer)
                        || IsChecked(x));
            }
        }

        private ReceiptSearch LastSearchCondition;
        private List<string> LegalPersonalities { get; set; }

        public PD0601()
        {
            InitializeComponent();
            grdAdvanceReceipt.SetupShortcutKeys();
            Text = "前受一括計上処理";
            InitializeHandlers();
        }
        private void InitializeHandlers()
        {
            tbcReceiptAdvanceReceived.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcReceiptAdvanceReceived.SelectedIndex == 0)
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

            txtPlayerName.Validated += (sender, e)
                => txtPlayerName.Text = EbDataHelper.ConvertToPayerName(txtPlayerName.Text, LegalPersonalities);

            cbxAdvanceReceipt.CheckedChanged += new EventHandler(CheckedChanged);
        }

        #region 画面の初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("更新");
            BaseContext.SetFunction04Caption("振替・分割");
            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);

            OnF01ClickHandler = Search;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = UpdateAdvanceReceipt;
            OnF04ClickHandler = ShowReceiptAdvanceReceivedSplit;
            OnF08ClickHandler = SelectAll;
            OnF09ClickHandler = UnSelectAll;
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region Search Dialog Setting

        private void btnCustomerTo_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeTo.Text = customer.Code;
                lblCustomerNameTo.Text = customer.Name;
                ClearStatusMessage();
            }
        }

        private void btnCustomerFrom_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeFrom.Text = customer.Code;
                lblCustomerNameFrom.Text = customer.Name;

                if (cbxCustomer.Checked)
                {
                    txtCustomerCodeTo.Text = customer.Code;
                    lblCustomerNameTo.Text = customer.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnSectionNameSearchFrom_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                txtSectionCodeFrom.Text = section.Code;
                lblSectionNameFrom.Text = section.Name;

                if (cbxSection.Checked)
                {
                    txtSectionCodeTo.Text = section.Code;
                    lblSectionNameTo.Text = section.Name;
                }
            }
        }

        private void btnSectionNameToSearch_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                txtSectionCodeTo.Text = section.Code;
                lblSectionNameTo.Text = section.Name;
            }
        }

        private void btnReceiptCategoryNameSearch_Click(object sender, EventArgs e)
        {
            var receiptCategory = this.ShowReceiptCategorySearchDialog();
            if (receiptCategory != null)
            {
                txtReceiptCategoryFrom.Text = receiptCategory.Code;
                lblCategoryNameFrom.Text = receiptCategory.Name;

                if (cbxCategory.Checked)
                {
                    txtReceiptCategoryTo.Text = receiptCategory.Code;
                    lblCategoryNameTo.Text = receiptCategory.Name;
                    ReceiptCategoryId = receiptCategory.Id;
                }
            }
        }

        private void btnReceiptCategorySearch_Click(object sender, EventArgs e)
        {
            var receiptCategory = this.ShowReceiptCategorySearchDialog();
            if (receiptCategory != null)
            {
                txtReceiptCategoryTo.Text = receiptCategory.Code;
                lblCategoryNameTo.Text = receiptCategory.Name;
                ReceiptCategoryId = receiptCategory.Id;
            }
        }

        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            var code = txtCustomerCodeFrom.Text;
            var name = "";

            try
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    name = GetCustomerName(code);
                }

                lblCustomerNameFrom.Text = name;

                if (cbxCustomer.Checked)
                {
                    txtCustomerCodeTo.Text = code;
                    lblCustomerNameTo.Text = name;
                    ClearStatusMessage();
                }
                if (string.IsNullOrWhiteSpace(code) || (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(name)))
                {
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCustomerCodeTo_Validated(object sender, EventArgs e)
        {
            var code = txtCustomerCodeTo.Text;
            var name = "";

            try
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    name = GetCustomerName(code);
                }

                lblCustomerNameTo.Text = name;

                if (string.IsNullOrWhiteSpace(code) || (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(name)))
                {
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtSectionCodeFrom_Validated(object sender, EventArgs e)
        {
            var code = txtSectionCodeFrom.Text;
            var name = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    name = GetSectionName(code);
                }

                lblSectionNameFrom.Text = name;

                if (cbxSection.Checked)
                {
                    txtSectionCodeTo.Text = code;
                    lblSectionNameTo.Text = name;
                }
                if (string.IsNullOrWhiteSpace(code) || (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(name)))
                {
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtSectionCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                var code = txtSectionCodeTo.Text;
                lblSectionNameTo.Text = GetSectionName(txtSectionCodeTo.Text);
                if (string.IsNullOrWhiteSpace(code) || (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(lblSectionNameTo.Text)))
                {
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private string GetSectionName(string code)
        {
            var Name = "";
            var sectionResult = new Web.Models.Section();

            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                SectionsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (result.ProcessResult.Result)
                {
                    sectionResult = result.Sections.FirstOrDefault();

                    if (sectionResult != null)
                    {
                        Name = sectionResult.Name;
                    }
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return Name;
        }

        private string GetCategoryName(string code)
        {
            var Name = "";
            var categoryResult = new Category();

            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                CategoriesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Receipt, new[] { code });
                if (result.ProcessResult.Result)
                {
                    categoryResult = result.Categories.FirstOrDefault();

                    if (categoryResult != null)
                    {
                        Name = categoryResult.Name;
                    }
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return Name;
        }

        private void txtReceiptCategoryFrom_Validated(object sender, EventArgs e)
        {
            var code = txtReceiptCategoryFrom.Text;
            var name = "";

            try
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    name = GetCategoryName(code);
                }

                lblCategoryNameFrom.Text = name;

                if (cbxCategory.Checked)
                {
                    txtReceiptCategoryTo.Text = code;
                    lblCategoryNameTo.Text = name;
                }
                if (string.IsNullOrWhiteSpace(code) || (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(name)))
                {
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtReceiptCategoryTo_Validated(object sender, EventArgs e)
        {
            try
            {
                lblCategoryNameTo.Text = GetCategoryName(txtReceiptCategoryTo.Text);
                if (string.IsNullOrWhiteSpace(txtReceiptCategoryTo.Text) || (!string.IsNullOrWhiteSpace(txtReceiptCategoryTo.Text) && !string.IsNullOrWhiteSpace(lblCategoryNameTo.Text)))
                {
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCurrencyCodeSearch_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                CurrencyID = currency.Id;
                NoOfpre = currency.Precision;
                SetCurrencyDisplayString(NoOfpre);
                ClearStatusMessage();
            }
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

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
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

        private void GetCurrencyCode()
        {
            CurrenciesResult result = null;

            if (txtCurrencyCode.Text != "")
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CurrencyMasterClient>();

                    result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });
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
        #endregion

        #region CheckedChanged Event

        private void CheckedChanged(object sender, EventArgs e)
        {
            cbxFullAssignment.Enabled = cbxAdvanceReceipt.Checked;

            if (!cbxAdvanceReceipt.Checked)
                cbxFullAssignment.Checked = false;
        }

        #endregion

        #region Function Key
        [OperationLog("クリア")]
        public void Clear()
        {
            if (IsGridModified
                && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            tbcReceiptAdvanceReceived.SelectedIndex = 0;
            ClearAll();
        }

        private void ClearAll()
        {
            txtPlayerName.Clear();
            txtReceiptCategoryFrom.Clear();
            txtReceiptCategoryTo.Clear();
            lblCategoryNameFrom.Clear();
            lblCategoryNameTo.Clear();
            txtCustomerCodeFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();
            txtSectionCodeFrom.Clear();
            txtSectionCodeTo.Clear();
            lblSectionNameFrom.Clear();
            lblSectionNameTo.Clear();
            txtMemoExist.Clear();
            txtCurrencyCode.Clear();
            datReceiptAtFrom.Clear();
            datReceiptAtTo.Clear();
            datReceiptAtFrom.Select();           
            lblReceiptCount.Clear();
            lblReceiptAmountTotal.Clear();
            lblRemainAmountTotal.Clear();
            grdAdvanceReceipt.DataSource = null;
            ClearStatusMessage();

            EnableFunctionKeys(enabled: false);

            cbxMemoExist.Checked = false;
            cbxPartAssignment.Checked = true;
            cbxNoAssignment.Checked = true;
            cbxAdvanceReceipt.Enabled = true;
            cbxAdvanceReceipt.Checked = false;
            cbxFullAssignment.Enabled = false;
            cbxFullAssignment.Checked = false;
        }

        [OperationLog("検索")]
        public void Search()
        {
            try
            {
                if (IsGridModified
                    && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

                if (!RequiredCheck()) return;

                ReceiptSearch receiptSearchCondition = SearchCondition();
                GetReceiptSearch(receiptSearchCondition);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("全選択")]
        public void SelectAll()
        {
            ClearStatusMessage();
            grdAdvanceReceipt.EndEdit();

            foreach (var row in grdAdvanceReceipt.Rows)
            {
                row.Cells[CellName("CheckBox")].Value = 1;
                SetModifiedRow(row);
            }
        }

        [OperationLog("全解除")]
        public void UnSelectAll()
        {
            ClearStatusMessage();
            grdAdvanceReceipt.EndEdit();

            foreach (var row in grdAdvanceReceipt.Rows)
            {
                row.Cells[CellName("CheckBox")].Value = 0;
                SetModifiedRow(row);
            }
        }

        [OperationLog("更新")]
        public void UpdateAdvanceReceipt()
        {
            if (!ValidateInputValue()) return;

            if (!ShowConfirmDialog(MsgQstConfirmUpdate, ""))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            var isCancel = cbxAdvanceReceipt.Checked;
            var target = grdAdvanceReceipt.Rows.Where(x => IsChecked(x));
            var updateItems = target
                .Select(x => x.DataBoundItem as Receipt)
                .Select(receipt => new AdvanceReceived
                {
                    ReceiptId           = isCancel ? receipt.Id : 0L,
                    OriginalReceiptId   = isCancel ? (receipt.OriginalReceiptId ?? 0L) : receipt.Id,
                    CustomerId          = receipt.CustomerId ?? 0,
                    OriginalUpdateAt    = receipt.UpdateAt,
                    ReceiptCategoryId   = ReceiptCategoryId,
                    CompanyId           = CompanyId,
                    LoginUserId         = Login.UserId,
                }).ToArray();

            var success = true;
            var updateValid = true;
            var saveItems = new List<Receipt>();
            var deleteItems = new List<Receipt>();

            ProgressDialog.Start(ParentForm, async (cancel, progress) =>
            {
                AdvanceReceivedResult result = null;

                try
                {
                    var sessionKey = SessionKey;
                    await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
                    {
                        if (isCancel && IsPostProcessorImplemented)
                        {
                            var getResult = await client.GetAsync(sessionKey, updateItems.Select(x => x.ReceiptId).ToArray());
                            if (getResult.ProcessResult.Result
                                && getResult.Receipts != null
                                && getResult.Receipts.Any(x => x != null))
                            {
                                deleteItems.AddRange(getResult.Receipts);
                            }
                        }
                        result = isCancel
                            ? await client.CancelAdvanceReceivedAsync(sessionKey, updateItems)
                            : await client.SaveAdvanceReceivedAsync(sessionKey, updateItems);
                        success = result.ProcessResult.Result;
                        updateValid = result.ProcessResult.ErrorCode != ErrorCode.OtherUserAlreadyUpdated;
                        if (result.ProcessResult.Result)
                        {
                            if (IsPostProcessorImplemented)
                            {
                                var ids = new List<long>(result.AdvancedReceiveItems.Select(x => x.ReceiptId));
                                ids.AddRange(result.AdvancedReceiveItems.Select(x => x.OriginalReceiptId));

                                var getResult = await client.GetAsync(sessionKey, ids.ToArray());
                                if (getResult.ProcessResult.Result
                                    && getResult.Receipts != null
                                    && getResult.Receipts.Any(x => x != null))
                                {
                                    saveItems.AddRange(getResult.Receipts);
                                }
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    success = false;
                    Debug.Assert(false, ex.Message);
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }

                if (success && IsPostProcessorImplemented)
                {
                    var syncResult = true;
                    if (isCancel)
                    {
                        syncResult = CancelPostProcessor.Invoke(deleteItems.Select(x => x as ITransactionData));
                        success &= syncResult;
                    }
                    syncResult = SavePostProcessor.Invoke(saveItems.Select(x => x as ITransactionData));
                    success &= syncResult;
                }

            }, false, SessionKey);

            if (!updateValid)
            {
                ShowWarningDialog(MsgWngAlreadyUpdated);
                return;
            }

            if (!success)
            {
                ShowWarningDialog(MsgErrUpdateError);
                return;
            }

            tbcReceiptAdvanceReceived.SelectedIndex = 1;
            grdAdvanceReceipt.DataSource = null;

            ReceiptSearch receiptSearchCondition = SearchCondition();
            GetReceiptSearch(receiptSearchCondition);

            DispStatusMessage(MsgInfUpdateSuccess);
        }

        private bool ValidateInputValue()
        {
            var target = grdAdvanceReceipt.Rows.Where(x => IsChecked(x));
            if (!cbxAdvanceReceipt.Checked)
            {
                foreach (var row in target)
                {
                    var receipt = row.DataBoundItem as Receipt;
                    if (!string.IsNullOrEmpty(receipt.CustomerCode)
                        && receipt.CustomerId.HasValue) continue;
                    row[CellName(nameof(Receipt.CustomerCode))].Selected = true;
                    ShowWarningDialog(MsgWngInputRequired, "得意先コード");
                    return false;
                }
            }
            if (!target.Any())
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "更新するデータ");
                return false;
            }
            return true;
        }

        private bool ValidateEditable(Receipt advanceReceipt)
        {
            if (advanceReceipt.RemainAmountFlag > 0)
            {
                ShowWarningDialog(MsgWngOriginalReceiptRemainAmountNotZero);
                return false;
            }

            if (advanceReceipt.ReceiptStatusFlag == (int)ReceiptStatus.Deleted)
            {
                ShowWarningDialog(MsgWngOriginalReceiptAlreadyDeleted);
                return false;
            }
            return true;
        }

        [OperationLog("振替・分割")]
        public void ShowReceiptAdvanceReceivedSplit()
        {
            var cell = grdAdvanceReceipt.SelectedCells.FirstOrDefault();

            if (cell != null)
            {
                var row = grdAdvanceReceipt.Rows[cell.RowIndex];
                var advanceReceipt = (Receipt)row.DataBoundItem;

                if (!ValidateEditable(advanceReceipt))
                    return;

                var form = ApplicationContext.Create(nameof(PD0602));
                form.StartPosition = FormStartPosition.CenterParent;
                var screen = form.GetAll<PD0602>().FirstOrDefault();
                screen.SelectedAdvanceReceipt = advanceReceipt;

                ApplicationContext.ShowDialog(ParentForm, form);

                if (screen.IsDataRegistered)
                {
                    Search();
                }
            }
        }

        [OperationLog("終了")]
        public void Exit()
        {
            if (IsGridModified
                && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            BaseForm.Close();
            checkStorage();
        }

        private void ReturnToSearchCondition()
        {
            tbcReceiptAdvanceReceived.SelectedIndex = 0;
        }         

        public void checkStorage()
        {
            Settings.SaveControlValue<PD0601>(Login, cbxCategory.Name, cbxCategory.Checked);
            Settings.SaveControlValue<PD0601>(Login, cbxCustomer.Name, cbxCustomer.Checked);
            Settings.SaveControlValue<PD0601>(Login, cbxSection.Name, cbxSection.Checked);
        }
        #endregion

        #region GridView Event

        private void grdAdvanceReceipt_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.CellName == CellName("Memo"))
                {
                    var receipt = grdAdvanceReceipt.Rows[e.RowIndex].DataBoundItem as Receipt;
                    if (receipt == null) return;

                    using (var form = ApplicationContext.Create(nameof(PH9906)))
                    {
                        var screen = form.GetAll<PH9906>().First();
                        screen.Id = receipt.Id;
                        screen.MemoType = MemoType.ReceiptMemo;
                        screen.Memo = receipt.ReceiptMemo;
                        screen.InitializeParentForm("入金メモ");
                        if (ApplicationContext.ShowDialog(ParentForm, form, true) == DialogResult.OK)
                        {
                            if (string.IsNullOrEmpty(screen.Memo))
                            {
                                grdAdvanceReceipt.Rows[e.RowIndex].Cells[CellName("Memo")].Value = "";
                            }
                            else
                            {
                                grdAdvanceReceipt.Rows[e.RowIndex].Cells[CellName("Memo")].Value = screen.Memo;
                            }
                        }
                    }
                }
                else
                {
                    if (LastSearchCondition != null)
                    {
                        if ((LastSearchCondition.AdvanceReceivedFlg ?? 0) == 1) // 検索実行時｢前受のデータのみ表示｣にチェックされていたか否か
                        {
                            ShowReceiptAdvanceReceivedSplit();
                        }
                    }
                }
            }
        }

        private void grdAdvanceReceipt_CellClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Row row = grdAdvanceReceipt.Rows[e.RowIndex];

            if ((!cbxAdvanceReceipt.Checked) && IsChecked(row) && e.CellName == CellName("BtnCustomerCode"))
            {
                var customer = this.ShowCustomerMinSearchDialog();
                if (customer != null)
                {
                    row.Cells[CellName(nameof(Receipt.CustomerCode))].Value = customer.Code;
                    row.Cells[CellName(nameof(Receipt.CustomerName))].Value = customer.Name;
                    row.Cells[CellName(nameof(Receipt.CustomerId))].Value = customer.Id;
                }
            }
            else if(cbxAdvanceReceipt.Checked && e.CellName == CellName("CheckBox"))
            {
                SetModifiedAdvancedReceiptCheckedBox();
            }
        }

        private void grdAdvanceReceipt_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellName == CellName(nameof(Receipt.CustomerCode))) return;
            if (cbxAdvanceReceipt.Checked) return;
            grdAdvanceReceipt.EndEdit();
            Row row = grdAdvanceReceipt.Rows[e.RowIndex];
            SetModifiedRow(row);
        }

        private void grdAdvanceReceipt_CellValidated(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Row row = grdAdvanceReceipt.Rows[e.RowIndex];
            if (!IsChecked(row)) return;

            try
            {
                string code = null;

                if (e.CellName.ToString() == CellName(nameof(Receipt.CustomerCode)) &&
                        row.Cells[CellName(nameof(Receipt.CustomerCode))].EditedFormattedValue != null)
                {
                    code = row.Cells[CellName(nameof(Receipt.CustomerCode))].EditedFormattedValue.ToString();
                    if (code != "")
                    {
                        if (ApplicationControl?.CustomerCodeType == 0)
                        {
                            code = code.PadLeft(ApplicationControl.CustomerCodeLength, '0');
                        }

                        string name = GetCustomerName(code);

                        if (!string.IsNullOrEmpty(name))
                        {
                            ClearStatusMessage();
                            grdAdvanceReceipt.EndEdit();
                            row.Cells[CellName(nameof(Receipt.CustomerId))].Value = CustomerId;
                            row.Cells[CellName(nameof(Receipt.CustomerCode))].Value = code;
                            row.Cells[CellName(nameof(Receipt.CustomerName))].Value = name;
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "得意先", code);
                            row.Cells[CellName(nameof(Receipt.CustomerName))].Value = "";
                            row.Cells[CellName(nameof(Receipt.CustomerCode))].Value = "";
                        }
                    }
                }
                else if (e.CellName.ToString() == CellName(nameof(Receipt.CustomerCode)) &&
                        row.Cells[CellName(nameof(Receipt.CustomerCode))].EditedFormattedValue == null)
                {
                    row.Cells[CellName(nameof(Receipt.CustomerName))].Value = "";
                    row.Cells[CellName(nameof(Receipt.CustomerId))].Value = null;
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grdAdvanceReceipt_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (cbxAdvanceReceipt.Checked)
            {
                SetModifiedAdvancedReceiptCheckedBox();
            }
            else
            {
                grdAdvanceReceipt.EndEdit();
                Row row = grdAdvanceReceipt.Rows[e.RowIndex];
                SetModifiedRow(row);
            }
        }

        private void SetModifiedAdvancedReceiptCheckedBox()
        {
            grdAdvanceReceipt.CommitEdit();
            var check = Convert.ToBoolean(grdAdvanceReceipt.CurrentCell.Value);
            var orgReceiptId = Convert.ToInt64(grdAdvanceReceipt.GetValue(grdAdvanceReceipt.CurrentCell.RowIndex, CellName(nameof(Receipt.OriginalReceiptId))));

            foreach (var row in grdAdvanceReceipt.Rows.Where(x => (x.DataBoundItem as Receipt)?.OriginalReceiptId == orgReceiptId).ToList())
                row.Cells[CellName("CheckBox")].Value = check ? 1 : 0;
        }

        #endregion

        #region その他のFunction
        private void PD0601_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var tasks = new List<Task>();
                if (Company == null) tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null) tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadLegalPersonalitiesAsyn());

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                Settings.SetCheckBoxValue<PD0601>(Login, cbxCategory);
                Settings.SetCheckBoxValue<PD0601>(Login, cbxCustomer);
                Settings.SetCheckBoxValue<PD0601>(Login, cbxSection);

                LoadColumnNameItems();
                SetOnFormLoad();
                InitializeGrid();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetOnFormLoad()
        {
            var expression = new DataExpression(ApplicationControl);
            //pnlSection.Visible = UseSection;
            lblSectionCode.Visible = UseSection;
            txtSectionCodeFrom.Visible = UseSection;
            btnSectionSearchFrom.Visible = UseSection;
            lblSectionNameFrom.Visible = UseSection;
            lblSectionWave.Visible = UseSection;
            cbxSection.Visible = UseSection;
            txtSectionCodeTo.Visible = UseSection;
            btnSectionSearchTo.Visible = UseSection;
            lblSectionNameTo.Visible = UseSection;
            cbxSectionWithUser.Visible = UseSection;

            //pnlCurrency.Visible = UseForeignCurrency;
            lblCurrency.Visible = UseForeignCurrency;
            txtCurrencyCode.Visible = UseForeignCurrency;
            btnCurrencySearch.Visible = UseForeignCurrency;

            txtCustomerCodeFrom.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeFrom.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeFrom.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;

            txtCustomerCodeTo.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeTo.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeTo.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeTo.PaddingChar = expression.CustomerCodePaddingChar;

            txtSectionCodeFrom.Format = expression.SectionCodeFormatString;
            txtSectionCodeFrom.MaxLength = expression.SectionCodeLength;
            txtSectionCodeFrom.PaddingChar = expression.SectionCodePaddingChar;

            txtSectionCodeTo.Format = expression.SectionCodeFormatString;
            txtSectionCodeTo.MaxLength = expression.SectionCodeLength;
            txtSectionCodeTo.PaddingChar = expression.SectionCodePaddingChar;

            txtReceiptCategoryFrom.PaddingChar = '0';
            txtReceiptCategoryTo.PaddingChar = '0';

            ClearAll();
        }

        private void LoadColumnNameItems()
        {
            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ColumnNameSettingMasterClient>();
                ColumnNameList = await service.GetItemsAsync(SessionKey, CompanyId);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }

        private void GetReceiptSearch(ReceiptSearch recSearch)
        {
            ReceiptsResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptServiceClient>();

                result = await service.GetItemsAsync(SessionKey, recSearch);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                LastSearchCondition = recSearch;

                if (result.Receipts.Any())
                {
                    ReceiptDataInfo = result.Receipts;

                    InitializeGrid();

                    grdAdvanceReceipt.DataSource = new BindingSource(ReceiptDataInfo, null);

                    SearchDataBind();

                    EnableFunctionKeys(enabled: true);

                    cbxAdvanceReceipt.Enabled = false;
                }
                else
                {
                    if (grdAdvanceReceipt.Rows.Count > 0)
                        grdAdvanceReceipt.Rows.Clear();

                    lblReceiptCount.Text = "0";
                    lblReceiptAmountTotal.Text = 0.ToString(AmountFormat);
                    lblRemainAmountTotal.Text = 0.ToString(AmountFormat);
                    EnableFunctionKeys(enabled: false);
                    ShowWarningDialog(MsgWngNotExistSearchData);
                }
            }

            return;
        }

        private void SearchDataBind()
        {
            tbcReceiptAdvanceReceived.SelectedIndex = 1;

            var dataCount = grdAdvanceReceipt.RowCount;
            var receiptTotal = 0M;
            var remainTotal = 0M;

            foreach (var item in ReceiptDataInfo)
            {
                receiptTotal += item.ReceiptAmount;
                remainTotal += item.RemainAmount;
                item.CustomerCodeBuffer = item.CustomerCode;

                var row = grdAdvanceReceipt.Rows.FirstOrDefault(x => (x.DataBoundItem as Receipt)?.Id == item.Id);
                row.Cells[CellName("CheckBox")].Enabled = item.ReceiptStatusFlag == (int)ReceiptStatus.None && item.RemainAmountFlag == 0;
            }

            lblReceiptCount.Text = dataCount.ToString("#,##0");
            lblReceiptAmountTotal.Text = receiptTotal.ToString(AmountFormat);
            lblRemainAmountTotal.Text = remainTotal.ToString(AmountFormat);
        }

        private void EnableFunctionKeys(bool enabled)
        {
            BaseContext.SetFunction03Enabled(enabled);
            BaseContext.SetFunction04Enabled(enabled && cbxAdvanceReceipt.Checked);
            BaseContext.SetFunction08Enabled(enabled);
            BaseContext.SetFunction09Enabled(enabled);
        }

        private ReceiptSearch SearchCondition()
        {
            ReceiptSearch receiptSearch = new ReceiptSearch();
            receiptSearch.CompanyId = CompanyId;
            receiptSearch.LoginUserId = Login.UserId;

            if (datReceiptAtFrom.Value.HasValue)
            {
                receiptSearch.RecordedAtFrom = datReceiptAtFrom.Value.Value;
            }
            if (datReceiptAtTo.Value.HasValue)
            {
                receiptSearch.RecordedAtTo = datReceiptAtTo.Value.Value.AddDays(1).AddMilliseconds(-1);
            }
            if (!string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                receiptSearch.PayerName = txtPlayerName.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtReceiptCategoryFrom.Text))
            {
                receiptSearch.ReceiptCategoryCodeFrom = txtReceiptCategoryFrom.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtReceiptCategoryTo.Text))
            {
                receiptSearch.ReceiptCategoryCodeTo = txtReceiptCategoryTo.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text))
            {
                receiptSearch.CustomerCodeFrom = txtCustomerCodeFrom.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text))
            {
                receiptSearch.CustomerCodeTo = txtCustomerCodeTo.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtSectionCodeFrom.Text))
            {
                receiptSearch.SectionCodeFrom = txtSectionCodeFrom.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtSectionCodeTo.Text))
            {
                receiptSearch.SectionCodeTo = txtSectionCodeTo.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                receiptSearch.CurrencyCode = txtCurrencyCode.Text;
            }

            receiptSearch.UseForeignCurrencyFlg = UseForeignCurrency ? 1 : 0;
            if (UseForeignCurrency && !string.IsNullOrEmpty(txtCurrencyCode.Text))
                receiptSearch.CurrencyId = CurrencyID;

            if (cbxMemoExist.Checked)
            {
                receiptSearch.ExistsMemo = 1;
            }

            if (!string.IsNullOrWhiteSpace(txtMemoExist.Text))
            {
                receiptSearch.ReceiptMemo = txtMemoExist.Text;
            }

            if (cbxAdvanceReceipt.Checked)
            {
                receiptSearch.AdvanceReceivedFlg = 1;
            }
            else
            {
                receiptSearch.AdvanceReceivedFlg = 0;
            }

            if (cbxSectionWithUser.Checked)
            {
                receiptSearch.UseSectionMaster = true;
            }

            var assignments
                = (cbxNoAssignment  .Checked ? (int)AssignmentFlagChecked.NoAssignment   : (int)AssignmentFlagChecked.None)
                | (cbxPartAssignment.Checked ? (int)AssignmentFlagChecked.PartAssignment : (int)AssignmentFlagChecked.None)
                | (cbxFullAssignment.Checked ? (int)AssignmentFlagChecked.FullAssignment : (int)AssignmentFlagChecked.None);
            receiptSearch.AssignmentFlag = assignments;

            return receiptSearch;
        }

        private void InitializeGrid()
        {
            Template template = new Template();
            var headerCaption = (cbxAdvanceReceipt.Checked) ? "解除" : "前受";
            var width = (cbxAdvanceReceipt.Checked) ? 110 : 0;

            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            var center = MultiRowContentAlignment.MiddleCenter;
            Func<GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell> getMasterCode = () =>
            {
                var cell = builder.GetTextBoxCell(center);
                cell.AcceptsCrLf = GrapeCity.Win.Editors.CrLfMode.Filter;
                cell.AcceptsTabChar = GrapeCity.Win.Editors.TabCharMode.Filter;
                cell.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
                return cell;
            };
            Func<Cell> getCustomerCode = () =>
            {
                var cell = getMasterCode();
                cell.Style.ImeMode = txtCustomerCodeFrom.ImeMode;
                cell.Format = txtCustomerCodeFrom.Format;
                cell.MaxLength = txtCustomerCodeFrom.MaxLength;
                return cell;
            };
            Func<Cell> getSectionCode = () =>
            {
                var cell = getMasterCode();
                cell.Style.ImeMode = txtSectionCodeFrom.ImeMode;
                cell.Format = txtSectionCodeFrom.Format;
                cell.MaxLength = txtSectionCodeFrom.MaxLength;
                return cell;
            };

            #region header
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "CheckBox"                      , caption: headerCaption),
                new CellSetting(height,  width, nameof(Receipt.ReceiptStatusFlag)  , caption: "入金状態", sortable: true ),
                new CellSetting(height,  85, nameof(Receipt.AssignmentFlag)  , caption: "消込区分", sortable: true ),
                new CellSetting(height,  85, nameof(Receipt.RecordedAt)      , caption: "入金日", sortable: true ),
                new CellSetting(height, 100, nameof(Receipt.CategoryCodeName), caption: "入金区分", sortable: true ),
                new CellSetting(height, 130, nameof(Receipt.CustomerCode)    , caption: "得意先コード", sortable: true ),
                new CellSetting(height,   0, "btnCustomerCode"               , caption: "ボタン", sortable: true ),
                new CellSetting(height, 140, nameof(Receipt.CustomerName)    , caption: "得意先名", sortable: true ),
                new CellSetting(height, 140, nameof(Receipt.PayerName)       , caption: "振込依頼人名", sortable: true ),
            });

            if (UseForeignCurrency)
            {
                builder.Items.Add(new CellSetting(height, 60, nameof(Receipt.CurrencyCode), caption: "通貨コード", sortable: true));
            }

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 110, nameof(Receipt.ReceiptAmount), caption: "入金額", sortable: true ),
                new CellSetting(height, 110, nameof(Receipt.RemainAmount) , caption: "入金残", sortable: true ),
                new CellSetting(height, 110, nameof(Receipt.ExcludeAmount), caption: "対象外金額", sortable: true ),
            });
            if (UseSection)
            {
                builder.Items.Add(new CellSetting(height, 115, nameof(Receipt.SectionCode), caption: "入金部門コード", sortable: true));
                builder.Items.Add(new CellSetting(height, 140, nameof(Receipt.SectionName), caption: "入金部門名", sortable: true));
            }

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  95, nameof(Receipt.BankName), caption: "入金銀行", sortable: true ),
                new CellSetting(height,  95, nameof(Receipt.BranchName), caption: "入金支店", sortable: true ),
                new CellSetting(height,  95, nameof(Receipt.SourceBankName), caption: "仕向銀行", sortable: true ),
                new CellSetting(height,  95, nameof(Receipt.SourceBranchName), caption: "仕向支店", sortable: true ),
                new CellSetting(height, 150, "Memo", caption: "メモ", sortable: true ),
            });

            var receiptData = ColumnNameList.ColumnNames.Where(x => x.TableName == "Receipt").ToArray();

            for (var i = 0; i < receiptData.Length; i++)
            {
                var columnName = receiptData[i].DisplayColumnName;
                builder.Items.Add(new CellSetting(height, 150, $"Note{(i + 1)}", caption: columnName, sortable: true) );
            }
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();
            #endregion

            #region row
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "CheckBox"                         , readOnly: false, cell: builder.GetCheckBoxCell(), value: 0 ),
                new CellSetting(height,  width, nameof(Receipt.ReceiptStatusFlag)     , dataField: nameof(Receipt.ReceiptStatusDisp), cell: builder.GetTextBoxCell() ),
                new CellSetting(height,  85, nameof(Receipt.AssignmentFlag)     , dataField: nameof(Receipt.AssignmentFlagName), cell: builder.GetTextBoxCell() ),
                new CellSetting(height,  85, nameof(Receipt.RecordedAt)         , dataField: nameof(Receipt.RecordedAt),cell: builder.GetDateCell_yyyyMMdd() ),
                new CellSetting(height, 100, nameof(Receipt.CategoryCodeName)   , dataField: nameof(Receipt.CategoryCodeName),cell: builder.GetTextBoxCell() ),
                new CellSetting(height,  95, nameof(Receipt.CustomerCode)       , dataField: nameof(Receipt.CustomerCode), cell: getCustomerCode() ),
                new CellSetting(height,  35, "BtnCustomerCode"                  , enabled: false, cell: builder.GetButtonCell(), value: "…" ),
                new CellSetting(height, 140, nameof(Receipt.CustomerName)       , dataField: nameof(Receipt.CustomerName), cell: builder.GetTextBoxCell() ),
                new CellSetting(height, 140, nameof(Receipt.PayerName)          , dataField: nameof(Receipt.PayerName), cell: builder.GetTextBoxCell() ),
            });

            if (UseForeignCurrency)
            {
                builder.Items.Add(new CellSetting(height, Width = 60, Name = nameof(Receipt.CurrencyCode), dataField: nameof(Receipt.CurrencyCode), cell: builder.GetTextBoxCell(center) ));
            }

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, Width = 110, Name = nameof(Receipt.ReceiptAmount), dataField: nameof(Receipt.ReceiptAmount), cell: builder.GetTextBoxCurrencyCell(NoOfpre) ),
                new CellSetting(height, Width = 110, Name = nameof(Receipt.RemainAmount), dataField: nameof(Receipt.RemainAmount), cell: builder.GetTextBoxCurrencyCell(NoOfpre) ),
                new CellSetting(height, Width = 110, Name = nameof(Receipt.ExcludeAmount), dataField: nameof(Receipt.ExcludeAmount), cell: builder.GetTextBoxCurrencyCell(NoOfpre) ),
            });
            if (UseSection)
            {
                builder.Items.Add(new CellSetting(height, Width = 115, Name = nameof(Receipt.SectionCode), dataField: nameof(Receipt.SectionCode), cell: getSectionCode() ));
                builder.Items.Add(new CellSetting(height, Width = 140, Name = nameof(Receipt.SectionName), dataField: nameof(Receipt.SectionName), cell: builder.GetTextBoxCell() ));
            }

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, Width =  95, Name = nameof(Receipt. BankName), dataField: nameof(Receipt. BankName),cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width =  95, Name = nameof(Receipt. BranchName), dataField: nameof(Receipt. BranchName),cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width =  95, Name = nameof(Receipt.SourceBankName), dataField: nameof(Receipt.SourceBankName),cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width =  95, Name = nameof(Receipt.SourceBranchName), dataField: nameof(Receipt.SourceBranchName),cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width = 150, Name = "Memo", dataField: nameof(Receipt.ReceiptMemo),cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width = 150, Name = nameof(Receipt.Note1), dataField: nameof(Receipt.Note1), cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width = 150, Name = nameof(Receipt.Note2), dataField: nameof(Receipt.Note2), cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width = 150, Name = nameof(Receipt.Note3), dataField: nameof(Receipt.Note3), cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width = 150, Name = nameof(Receipt.Note4), dataField: nameof(Receipt.Note4), cell: builder.GetTextBoxCell() ),
                new CellSetting(height, Width =   0, Name = nameof(Receipt.Id), visible: true, dataField: nameof(Receipt.Id) ),
                new CellSetting(height, Width =   0, Name = nameof(Receipt.CustomerId), visible: true, dataField: nameof(Receipt.CustomerId) ),
                new CellSetting(height, Width =   0, Name = "OriginalUpdateAt", visible: true, dataField: nameof(Receipt.UpdateAt) ),
                new CellSetting(height, Width =   0, Name = nameof(Receipt.OriginalReceiptId), visible: true, dataField: nameof(Receipt.OriginalReceiptId) ),
                new CellSetting(height, Width =   0, Name = nameof(Receipt.CustomerCodeBuffer), visible: false, dataField: nameof(Receipt.CustomerCode) ),
            });
            builder.BuildRowOnly(template);
            #endregion

            grdAdvanceReceipt.Template = template;
            grdAdvanceReceipt.FreezeLeftCellIndex = 0;
            grdAdvanceReceipt.SetRowColor(ColorContext);
        }

        private string GetCustomerName(string code)
        {
            var Name = "";
            var customerResult = new Customer();

            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomersResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (result.ProcessResult.Result)
                {
                    customerResult = result.Customers.FirstOrDefault();

                    if (customerResult != null)
                    {
                        Name = customerResult.Name;
                        CustomerId = customerResult.Id;
                    }
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return Name;
        }

        private bool RequiredCheck()
        {
            if (!datReceiptAtFrom.ValidateRange(datReceiptAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblReceiptAt.Text)))
            {
                tbcReceiptAdvanceReceived.SelectedIndex = 0;
                return false;
            }

            if (!txtReceiptCategoryFrom.ValidateRange(txtReceiptCategoryTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblReceiptCategory.Text)))
            {
                tbcReceiptAdvanceReceived.SelectedIndex = 0;
                return false;
            }

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCode.Text)))
            {
                tbcReceiptAdvanceReceived.SelectedIndex = 0;
                return false;
            }

            if (!txtSectionCodeFrom.ValidateRange(txtSectionCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblSectionCode.Text)))
            {
                tbcReceiptAdvanceReceived.SelectedIndex = 0;
                return false;
            }

            if (UseForeignCurrency && string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text);
                tbcReceiptAdvanceReceived.SelectedIndex = 0;
                txtCurrencyCode.Select();
                return false;
            }

            if (cbxFullAssignment.Checked == false && cbxPartAssignment.Checked == false && cbxNoAssignment.Checked == false)
            {
                ShowWarningDialog(MsgWngSelectionRequired, lblAssignmentFlag.Text);
                tbcReceiptAdvanceReceived.SelectedIndex = 0;
                cbxFullAssignment.Select();
                return false;
            }

            return true;
        }

        private void SetModifiedRow(Row row)
        {
            bool CheckFlg = IsChecked(row);
            row.Cells[CellName(nameof(Receipt.CustomerCode))].ReadOnly = !CheckFlg;
            row.Cells[CellName("BtnCustomerCode")].Enabled = CheckFlg;
        }

        #endregion

        #region call web service
        private async Task LoadLegalPersonalitiesAsyn()
            => await ServiceProxyFactory.DoAsync<JuridicalPersonalityMasterService.JuridicalPersonalityMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    LegalPersonalities = result.JuridicalPersonalities.Select(x => x.Kana).ToList();
                else
                    LegalPersonalities = new List<string>();
            });
        #endregion
    }
}
