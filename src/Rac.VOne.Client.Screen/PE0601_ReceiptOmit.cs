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
    /// <summary>未消込入金データ削除</summary>
    public partial class PE0601 : VOneScreenBase
    {
        private int NoOfPre { get; set; }
        private int? LoginUserId { get; set; }
        private int CurrencyId { get; set; }
        private List<GridSetting> GridSettingInfo { get; set; }
        private List<Receipt> Receipts { get; set; }
        private List<object> ReceiptSearchReportList { get; set; }
        private IEnumerable<string> LegalPersonalities { get; set; }
        private string CellName(string value) => $"cel{value}";
        private bool IsGridModified { get { return grid.Rows.Any(x => (x.DataBoundItem as Receipt).Checked); } }

        public PE0601()
        {
            InitializeComponent();
            grid.SetupShortcutKeys();
            Text = "未消込入金データ削除";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcReceiptOmit.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcReceiptOmit.SelectedIndex == 0)
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
            txtSourceBankName.Validated += (sender, e) => txtSourceBankName.Text = EbDataHelper.ConvertToValidEbKana(txtSourceBankName.Text.Trim());
            txtSourceBranchName.Validated += (sender, e) => txtSourceBranchName.Text = EbDataHelper.ConvertToValidEbKana(txtSourceBranchName.Text.Trim());

            grid.CellValueChanged               += grid_CellValueChanged;
            grid.CurrentCellDirtyStateChanged   += grid_CurrentCellDirtyStateChanged;
            grid.CellFormatting                 += grid_CellFormatting;
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
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction06Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(true);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Search;
            OnF02ClickHandler = ConfirmToClear;
            OnF03ClickHandler = Delete;
            OnF04ClickHandler = DoPrint;
            OnF06ClickHandler = ExportData;
            OnF08ClickHandler = SelectAll;
            OnF09ClickHandler = DeselectAll;
            OnF10ClickHandler = Exit;

        }

        private void PE0601_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
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

                if (Authorities == null)
                {
                    loadTask.Add(LoadFunctionAuthorities(FunctionType.RecoverReceipt, FunctionType.ModifyReceipt));
                }

                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                loadTask.Add(LoadLegalPersonalities());

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SetFormat();
                BindComboData();
                InitializeGrid();
                Clear();
                SetCheckBoxSetting();
                cmbAmountRange.SelectedIndex = 0;
                cbxDelete.Checked = false;
                cbxDelete.Enabled = true;
                datDeleteFrom.Enabled = false;
                datDeleteTo.Enabled = false;
                datDeleteFrom.Clear();
                datDeleteTo.Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task LoadGridSetting()
        {
            await ServiceProxyFactory.DoAsync<GridSettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, Login.UserId, GridId.ReceiptSearch);
                if (result.ProcessResult.Result)
                {
                    GridSettingInfo = result.GridSettings;
                }
            });
        }

        private async Task LoadLegalPersonalities()
        {
            try
            {
                await ServiceProxyFactory.DoAsync<JuridicalPersonalityMasterClient>(async client =>
                {
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

        private void SetFormat()
        {
            if (GridSettingInfo != null)
            {
                foreach (var gridSetting in GridSettingInfo)
                {
                    switch(gridSetting.ColumnName)
                    {
                        case "Note1":lblNote1.Text = gridSetting.ColumnNameJp;
                                     break;
                        case "Note2":lblNote2.Text = gridSetting.ColumnNameJp;
                                     break;
                        case "Note3":lblNote3.Text = gridSetting.ColumnNameJp;
                                     break;
                        case "Note4":lblNote4.Text = gridSetting.ColumnNameJp;
                                     break;
                    }
                }
            }
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);

                txtLoginUserCode.Format = expression.LoginUserCodeFormatString;
                txtLoginUserCode.MaxLength = expression.LoginUserCodeLength;
                txtLoginUserCode.PaddingChar = expression.LoginUserCodePaddingChar;

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

                if (!UseForeignCurrency)
                {
                    nmbRemainAmountTotal.DisplayFields.AddRange("#,###,###,###,##0", "", "", "-", "");
                    nmbReceiptAmountTotal.DisplayFields.AddRange("#,###,###,###,##0", "", "", "-", "");
                }
                else
                {
                    nmbReceiptAmountFrom.Fields.DecimalPart.MaxDigits = 5;
                    nmbReceiptAmountTo.Fields.DecimalPart.MaxDigits = 5;
                }

                lblCurrencyCode.Visible = UseForeignCurrency;
                txtCurrencyCode.Visible = UseForeignCurrency;
                btnCurrencyCode.Visible = UseForeignCurrency;

                lblSectionCodeFrom.Visible = UseSection;
                txtSectionCodeFrom.Visible = UseSection;
                btnSectionCodeFrom.Visible = UseSection;
                lblSectionNameFrom.Visible = UseSection;
                lblSectionCodeToWave.Visible = UseSection;
                cbxSectionCodeTo.Visible = UseSection;
                txtSectionCodeTo.Visible = UseSection;
                btnSectionCodeTo.Visible = UseSection;
                lblSectionNameTo.Visible = UseSection;
                cbxUseReceiptSection.Visible = UseSection;

                txtBankCode.PaddingChar = '0';
                txtBranchCode.PaddingChar = '0';
                txtAccountNumber.PaddingChar = '0';
                txtPayerBankCode.PaddingChar = '0';
                txtPayerCodePre.PaddingChar = '0';
                txtPayerCodePost.PaddingChar = '0';
                txtBillBankCode.PaddingChar = '0';
                txtBillBranchCode.PaddingChar = '0';
            }
        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var middleRight = MultiRowContentAlignment.MiddleRight;

            var captionDelete = (cbxDelete.Checked) ? "復元" : "削除";
            var readOnly = !(Authorities[FunctionType.ModifyReceipt] || Authorities[FunctionType.RecoverReceipt]);

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 40, nameof(Receipt.Checked), dataField: nameof(Receipt.Checked), caption: captionDelete, cell: builder.GetCheckBoxCell(isBoolType: true), sortable: true, readOnly: readOnly),
                new CellSetting(height, cbxDelete.Checked? 100 : 0, nameof(Receipt.DeleteAt), caption: "削除日", dataField: nameof(Receipt.DeleteAt), cell: builder.GetDateCell_yyyyMMdd(), sortable: true),
            });

            foreach (var gs in GridSettingInfo)
            {
                var cell = new CellSetting(height, gs.DisplayWidth, gs.ColumnName, caption: gs.ColumnNameJp, dataField: gs.ColumnName, sortable: true);
                switch (gs.ColumnName)
                {
                    case nameof(Receipt.Id):
                        cell.CellInstance = builder.GetTextBoxCell(middleRight);
                        break;
                    case "AssignmentState":
                        cell.DataField = nameof(Receipt.AssignmentFlagName);
                        cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                        break;
                    case nameof(Receipt.RecordedAt):
                    case nameof(Receipt.DueAt):
                    case nameof(Receipt.BillDrawAt):
                        cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                        break;
                    case nameof(Receipt.OutputAt):
                        cell.CellInstance = builder.GetDateCell_yyyyMMddHHmmss();
                        break;
                    case nameof(Receipt.CustomerCode):
                    case nameof(Receipt.CurrencyCode):
                    case nameof(Receipt.SectionCode):
                    case nameof(Receipt.BankCode):
                    case nameof(Receipt.BranchCode):
                    case nameof(Receipt.BillBankCode):
                    case nameof(Receipt.BillBranchCode):
                    case nameof(Receipt.AccountNumber):
                        cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                        break;
                    case nameof(Receipt.CustomerName):
                    case nameof(Receipt.PayerName):
                    case nameof(Receipt.Note1):
                    case nameof(Receipt.Note2):
                    case nameof(Receipt.Note3):
                    case nameof(Receipt.Note4):
                    case nameof(Receipt.SectionName):
                    case nameof(Receipt.BankName):
                    case nameof(Receipt.BranchName):
                    case nameof(Receipt.SourceBankName):
                    case nameof(Receipt.SourceBranchName):
                    case nameof(Receipt.BillNumber):
                    case nameof(Receipt.BillDrawer):
                        break;
                    case "ReceiptCategoryName":
                        cell.DataField = nameof(Receipt.CategoryCodeName);
                        break;
                    case "Memo":
                        cell.DataField = nameof(Receipt.ReceiptMemo);
                        break;
                    case nameof(Receipt.InputType):
                        cell.DataField = nameof(Receipt.InputTypeName);
                        break;
                    case "VirtualBranchCode":
                        cell.DataField = nameof(Receipt.PayerCodePrefix);
                        cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                        break;
                    case "VirtualAccountNumber":
                        cell.DataField = nameof(Receipt.PayerCodeSuffix);
                        cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                        break;
                    case nameof(Receipt.ReceiptAmount):
                    case nameof(Receipt.RemainAmount):
                    case nameof(Receipt.ExcludeAmount):
                        cell.CellInstance = builder.GetTextBoxCurrencyCell(NoOfPre);
                        break;
                    default: continue;
                }
                builder.Items.Add(cell);
            }

            grid.Template = builder.Build();
            grid.FreezeLeftCellIndex = (cbxDelete.Checked) ? 1 : 0;
            grid.HideSelection = true;
            grid.AllowAutoExtend = false;
        }

        private void BindComboData()
        {
            //預金種別コンボでデータを設定
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
                accountTypeIndex = cmbAccountType.Items.Add(pair.Value);
                cmbAccountType.Items[accountTypeIndex].Tag = pair.Key;
            }

            //入力区分コンボでデータを設定
            cmbInputType.Items.Add(new ListItem("すべて", 0));
            cmbInputType.Items.Add(new ListItem("EB取込", 1));
            cmbInputType.Items.Add(new ListItem("入力", 2));
            cmbInputType.Items.Add(new ListItem("インポーター取込", 3));
            cmbInputType.Items.Add(new ListItem("電債取込", 4));

            //金額範囲コンボでデータを設定
            cmbAmountRange.Items.Add(new ListItem("入金額", 0));
            cmbAmountRange.Items.Add(new ListItem("入金残", 1));
        }

        private void SetCheckBoxSetting()
        {
            Settings.SetCheckBoxValue<PE0601>(Login, cbxCustomerCodeTo);
            Settings.SetCheckBoxValue<PE0601>(Login, cbxSectionCodeTo);
            Settings.SetCheckBoxValue<PE0601>(Login, cbxUseReceiptSection);
            Settings.SetCheckBoxValue<PE0601>(Login, cbxAmount);
            Settings.SetCheckBoxValue<PE0601>(Login, cbxCategoryCodeTo);
        }
        #endregion

        #region 検索 処理
        [OperationLog("検索")]
        private void Search()
        {
            try
            {
                ClearStatusMessage();

                if (!ValidateChildren()) return;

                if (!RequiredCheck()) return;

                if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

                ReceiptSearch ReceiptSearchCondition = SearchCondition();
                SetReceiptSearch(ReceiptSearchCondition);

                ReceiptSearchReportList = CreateReportCondition();

                if (grid.RowCount > 0)
                {
                    if (cbxDelete.Checked)
                    {
                        BaseContext.SetFunction03Enabled(Authorities[FunctionType.RecoverReceipt]);
                        BaseContext.SetFunction08Enabled(Authorities[FunctionType.RecoverReceipt]);
                        BaseContext.SetFunction09Enabled(Authorities[FunctionType.RecoverReceipt]);
                    }
                    else
                    {
                        BaseContext.SetFunction03Enabled(Authorities[FunctionType.ModifyReceipt]);
                        BaseContext.SetFunction08Enabled(Authorities[FunctionType.ModifyReceipt]);
                        BaseContext.SetFunction09Enabled(Authorities[FunctionType.ModifyReceipt]);
                    }
                }
                else
                {
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
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

            if (cbxPartAssignment.Checked == false && cbxNoAssignment.Checked == false)
            {
                ShowWarningDialog(MsgWngSelectionRequired, lblMatchingType.Text);
                btnCategoryCodeTo.Select();
                SendKeys.Send("{TAB}");
                return false;
            }

            if (!datRecordedAtFrom.ValidateRange(datRecordedAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblRecordedAt.Text))) return false;

            if (!datDeleteFrom.ValidateRange(datDeleteTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDelete.Text))) return false;

            if (!datUpdateAtFrom.ValidateRange(datUpdateAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblUpdateAt.Text))) return false;

            if (!datBillDrawAtFrom.ValidateRange(datBillDrawAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblBillDrawAt.Text))) return false;

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerFrom.Text))) return false;

            if (!txtSectionCodeFrom.ValidateRange(txtSectionCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblSectionCodeFrom.Text))) return false;

            if (!txtCategoryCodeFrom.ValidateRange(txtCategoryCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCategoryCode.Text))) return false;

            if (!nmbReceiptAmountFrom.ValidateRange(nmbReceiptAmountTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, cmbAmountRange.SelectedValue.ToString()))) return false;

            return true;
        }

        /// <summary>検索条件のためModelを生成する。</summary>
        /// <returns>生成されたModel</returns>
        private ReceiptSearch SearchCondition()
        {
            var receiptSearch = new ReceiptSearch();

            receiptSearch.LoginUserId = Login.UserId;

            if (datRecordedAtFrom.Value.HasValue)
            {
                receiptSearch.RecordedAtFrom = datRecordedAtFrom.Value?.Date;
            }

            if (datRecordedAtTo.Value.HasValue)
            {
                receiptSearch.RecordedAtTo = datRecordedAtTo.Value?.Date.AddDays(1).AddMilliseconds(-1);
            }

            if (txtPayerName.Text != "")
            {
                receiptSearch.PayerName = txtPayerName.Text;
            }

            if (datUpdateAtFrom.Value.HasValue)
            {
                receiptSearch.UpdateAtFrom = datUpdateAtFrom.Value?.Date;
            }

            if (datUpdateAtTo.Value.HasValue)
            {
                receiptSearch.UpdateAtTo = datUpdateAtTo.Value?.Date.AddDays(1).AddMilliseconds(-1);
            }

            if (!string.IsNullOrEmpty(txtLoginUserCode.Text))
            {
                receiptSearch.UpdateBy = LoginUserId;
            }

            if (UseForeignCurrency)
            {
                receiptSearch.UseForeignCurrencyFlg = 1;

                if (txtCurrencyCode.Text != "")
                {
                    receiptSearch.CurrencyId = CurrencyId;
                }
            }

            if (txtBankCode.Text != "")
            {
                receiptSearch.BankCode = txtBankCode.Text;
            }

            if (txtBranchCode.Text != "")
            {
                receiptSearch.BranchCode = txtBranchCode.Text;
            }

            if (cmbAccountType.SelectedIndex != 0)
            {
                receiptSearch.AccountTypeId = Convert.ToInt32(cmbAccountType.SelectedItem.SubItems[1].Value);
            }

            if (txtAccountNumber.Text != "")
            {
                receiptSearch.AccountNumber = txtAccountNumber.Text;
            }

            if (txtPayerBankCode.Text != "")
            {
                receiptSearch.PrivateBankCode = txtPayerBankCode.Text;
            }

            if (txtPayerCodePre.Text != "")
            {
                receiptSearch.PayerCodePrefix = txtPayerCodePre.Text;
            }

            if (txtPayerCodePost.Text != "")
            {
                receiptSearch.PayerCodeSuffix = txtPayerCodePost.Text;
            }

            if (txtBillNumber.Text != "")
            {
                receiptSearch.BillNumber = txtBillNumber.Text;
            }

            if (txtBillBankCode.Text != "")
            {
                receiptSearch.BillBankCode = txtBillBankCode.Text;
            }

            if (txtBillBranchCode.Text != "")
            {
                receiptSearch.BillBranchCode = txtBillBranchCode.Text;
            }

            if (datBillDrawAtFrom.Value.HasValue)
            {
                receiptSearch.BillDrawAtFrom = datBillDrawAtFrom.Value?.Date;
            }
            if (datBillDrawAtTo.Value.HasValue)
            {
                receiptSearch.BillDrawAtTo = datBillDrawAtTo.Value?.Date.AddDays(1).AddMilliseconds(-1);
            }

            if (txtBillDrawer.Text != "")
            {
                receiptSearch.BillDrawer = txtBillDrawer.Text;
            }

            if (txtCustomerCodeFrom.Text != "")
            {
                receiptSearch.CustomerCodeFrom = txtCustomerCodeFrom.Text;
            }
            if (txtCustomerCodeTo.Text != "")
            {
                receiptSearch.CustomerCodeTo = txtCustomerCodeTo.Text;
            }

            if (UseSection)
            {
                if (txtSectionCodeFrom.Text != "")
                {
                    receiptSearch.SectionCodeFrom = txtSectionCodeFrom.Text;
                }
                if (txtSectionCodeTo.Text != "")
                {
                    receiptSearch.SectionCodeTo = txtSectionCodeTo.Text;
                }
            }

            if (cbxUseReceiptSection.Checked)
            {
                receiptSearch.UseSectionMaster = true;
            }
            else
            {
                receiptSearch.UseSectionMaster = false;
            }

            if (cbxMemo.Checked)
            {
                receiptSearch.ExistsMemo = 1;
            }
            else
            {
                receiptSearch.ExistsMemo = 0;
            }

            if (txtMemo.Text != "")
            {
                receiptSearch.ReceiptMemo = txtMemo.Text;
            }

            if (cmbInputType.SelectedIndex > 0)
            {
                receiptSearch.InputType = cmbInputType.SelectedIndex;
            }

            if (txtCategoryCodeFrom.Text != "")
            {
                receiptSearch.ReceiptCategoryCodeFrom = txtCategoryCodeFrom.Text;
            }
            if (txtCategoryCodeTo.Text != "")
            {
                receiptSearch.ReceiptCategoryCodeTo = txtCategoryCodeTo.Text;
            }

            var assingments
                = (cbxNoAssignment  .Checked ? (int)AssignmentFlagChecked.NoAssignment   : (int)AssignmentFlagChecked.None)
                | (cbxPartAssignment.Checked ? (int)AssignmentFlagChecked.PartAssignment : (int)AssignmentFlagChecked.None)
                | (cbxFullAssignment.Checked ? (int)AssignmentFlagChecked.FullAssignment : (int)AssignmentFlagChecked.None);
            receiptSearch.AssignmentFlag = assingments;

            var amountType = cmbAmountRange.SelectedIndex;
            if (nmbReceiptAmountFrom.Value.HasValue)
            {
                if (amountType == 0)
                    receiptSearch.ReceiptAmountFrom = nmbReceiptAmountFrom.Value;
                else if (amountType == 1)
                    receiptSearch.RemainAmountFrom = nmbReceiptAmountFrom.Value;
            }
            if (nmbReceiptAmountTo.Value.HasValue)
            {
                if (amountType == 0)
                    receiptSearch.ReceiptAmountTo = nmbReceiptAmountTo.Value;
                else if (amountType == 1)
                    receiptSearch.RemainAmountTo = nmbReceiptAmountTo.Value;
            }

            if (txtSourceBankName.Text != "")
            {
                receiptSearch.SourceBankName = txtSourceBankName.Text;
            }

            if (txtSourceBranchName.Text != "")
            {
                receiptSearch.SourceBranchName = txtSourceBranchName.Text;
            }

            if (txtNote1.Text != "")
            {
                receiptSearch.Note1 = txtNote1.Text;
            }

            if (txtNote2.Text != "")
            {
                receiptSearch.Note2 = txtNote2.Text;
            }

            if (txtNote3.Text != "")
            {
                receiptSearch.Note3 = txtNote3.Text;
            }

            if (txtNote4.Text != "")
            {
                receiptSearch.Note4 = txtNote4.Text;
            }
            if (cbxDelete.Checked)
            {
                receiptSearch.DeleteFlg = 1;
            }
            else
            {
                receiptSearch.DeleteFlg = 0;
            }
            if (datDeleteFrom.Value.HasValue)
            {
                receiptSearch.DeleteAtFrom = datDeleteFrom.Value?.Date;
            }
            if (datDeleteTo.Value.HasValue)
            {
                receiptSearch.DeleteAtTo = datDeleteTo.Value?.Date.AddDays(1).AddMilliseconds(-1);
            }
            return receiptSearch;
        }

        /// <summary> 検索条件で取得したデータを設定 </summary>
        /// <param name="recSearch">　検索条件のModel　</param>
        private void SetReceiptSearch(ReceiptSearch recSearch)
        {
            recSearch.CompanyId = CompanyId;
            recSearch.LoginUserId = Login.UserId;
            var receiptTotal = 0M;
            var remainTotal = 0M;

            ReceiptsResult result = null;
            var searchTask = ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
            {
                result = await client.GetItemsAsync(SessionKey, recSearch);
            });
            ProgressDialog.Start(ParentForm, searchTask, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                if (result.Receipts.Any())
                {
                    Receipts = result.Receipts;
                    InitializeGrid();
                    grid.DataSource = new BindingSource(Receipts, null);
                    if (UseForeignCurrency)
                    {
                        nmbReceiptAmountTotal.DisplayFields.Clear();
                        nmbRemainAmountTotal.DisplayFields.Clear();
                        nmbReceiptAmountTotal.DisplayFields.AddRange(GetNumberFormat("#,###,###,###,##0", NoOfPre), "", "", "-", "");
                        nmbRemainAmountTotal.DisplayFields.AddRange(GetNumberFormat("#,###,###,###,##0", NoOfPre), "", "", "-", "");
                    }
                    foreach (var receipt in Receipts)
                    {
                        receiptTotal += receipt.ReceiptAmount;
                        remainTotal += receipt.RemainAmount;
                    }
                    nmbReceiptCount.Value = grid.RowCount;
                    nmbReceiptAmountTotal.Value = receiptTotal;
                    nmbRemainAmountTotal.Value = remainTotal;
                    tbcReceiptOmit.SelectedIndex = 1;
                    tbcReceiptOmit.Select();
                    BaseContext.SetFunction04Enabled(true);
                    BaseContext.SetFunction06Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                    cbxDelete.Enabled = false;
                    datDeleteFrom.Enabled = false;
                    datDeleteTo.Enabled = false;
                }
                else
                {
                    grid.Rows.Clear();
                    nmbReceiptCount.Clear();
                    nmbReceiptAmountTotal.Clear();
                    nmbRemainAmountTotal.Clear();
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction06Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                    tbcReceiptOmit.SelectedIndex = 0;
                    ShowWarningDialog(MsgWngNotExistSearchData);
                }
            }
            return;
        }
        #endregion

        #region クリア 処理
        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            tbcReceiptOmit.SelectedIndex = 0;
            Clear();
        }

        private void Clear()
        {
            ClearStatusMessage();
            datRecordedAtFrom.Clear();
            datRecordedAtTo.Clear();
            txtPayerName.Clear();
            datUpdateAtFrom.Clear();
            datUpdateAtTo.Clear();
            txtLoginUserCode.Clear();
            lblLoginUserName.Clear();
            txtCurrencyCode.Clear();
            txtBankCode.Clear();
            txtBranchCode.Clear();
            txtAccountNumber.Clear();
            cmbAccountType.SelectedIndex = 0;
            txtPayerBankCode.Clear();
            txtPayerCodePre.Clear();
            txtPayerCodePost.Clear();
            txtBillNumber.Clear();
            txtBillBankCode.Clear();
            txtBillBranchCode.Clear();
            datBillDrawAtFrom.Clear();
            datBillDrawAtTo.Clear();
            txtBillDrawer.Clear();
            txtCustomerCodeFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameFrom.Clear();
            lblCustomerNameTo.Clear();
            txtSectionCodeFrom.Clear();
            txtSectionCodeTo.Clear();
            lblSectionNameFrom.Clear();
            lblSectionNameTo.Clear();
            txtMemo.Clear();
            cmbInputType.SelectedIndex = 0;
            cmbAmountRange.SelectedIndex = cmbAmountRange.Items.Count > 0 ? 0 : -1;
            txtCategoryCodeFrom.Clear();
            txtCategoryCodeTo.Clear();
            lblCategoryNameFrom.Clear();
            lblCategoryNameTo.Clear();
            nmbReceiptAmountFrom.Clear();
            nmbReceiptAmountTo.Clear();
            txtSourceBankName.Clear();
            txtSourceBranchName.Clear();
            txtNote1.Clear();
            txtNote2.Clear();
            txtNote3.Clear();
            txtNote4.Clear();
            cbxDelete.Enabled = true;
            if (cbxDelete.Checked)
            {
                datDeleteFrom.Enabled = true;
                datDeleteTo.Enabled = true;
                datDeleteFrom.Clear();
                datDeleteTo.Clear();
            }
            cbxMemo.Checked = false;
            cbxFullAssignment.Checked = false;
            cbxFullAssignment.Enabled = false;
            cbxPartAssignment.Checked = true;
            cbxNoAssignment.Checked = true;

            grid.DataSource = null;
            nmbReceiptCount.Clear();
            nmbReceiptAmountTotal.Clear();
            nmbRemainAmountTotal.Clear();

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            datRecordedAtFrom.Select();
        }
        #endregion

        #region 削除/復元

        [OperationLog("削除")]
        private void Delete()
        {
            DeleteOrRecoveryReceipt();
        }

        [OperationLog("復元")]
        private void Recovery()
        {
            DeleteOrRecoveryReceipt();
        }

        private void DeleteOrRecoveryReceipt()
        {
            try
            {
                ClearStatusMessage();

                var items = grid.Rows.Select(x => x.DataBoundItem as Receipt)
                    .Where(x => x.Checked)
                    .Select(x => new Transaction(x)).ToArray();

                if (!items.Any())
                {
                    ShowWarningDialog(MsgWngSelectionRequired, "入金データ");
                    return;
                }
                string msgQuestionConfirm = (cbxDelete.Checked) ? MsgQstConfirmRestoreDeletedData : MsgQstConfirmDelBillReceiptOmitData;
                string errorMessage = (cbxDelete.Checked) ? MsgErrRecoveryError : MsgErrDeleteError;

                if (!ShowConfirmDialog(msgQuestionConfirm, "入金データ"))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var success = true;
                var updateValid = true;
                var task = ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
                {
                    int doDelete = (cbxDelete.Checked) ? 0 : 1;
                    var result = await client.OmitAsync(SessionKey, doDelete, Login.UserId, items);
                    success = result.ProcessResult.Result;
                    updateValid = result.ProcessResult.ErrorCode != Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated;
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
                nmbReceiptCount.Clear();
                nmbReceiptAmountTotal.Clear();
                nmbRemainAmountTotal.Clear();
                BaseContext.SetFunction03Enabled(false);
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction08Enabled(false);
                BaseContext.SetFunction09Enabled(false);
                Search();
                var messageId = (cbxDelete.Checked) ? MsgInfFinishReturnBalanceProcess : MsgInfDeleteSuccess;
                DispStatusMessage(messageId);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        [OperationLog("印刷")]
        public void DoPrint()
        {
            try
            {
                var receiptReport = new ReceiptSearchSectionReport();
                string printTitle = cbxDelete.Checked ? "入金未消込削除一覧表" : "入金未消込一覧表";
                receiptReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                receiptReport.Name = printTitle + DateTime.Now.ToString("yyyyMMdd");
                receiptReport.SetData(Receipts, NoOfPre, UseSection, cbxDelete.Checked, GridSettingInfo);
                receiptReport.lbltitle.Text = printTitle;

                var searchReport = new ReceiptSearchConditionSectionReport();
                
                searchReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, printTitle);
                searchReport.Name = printTitle + DateTime.Now.ToString("yyyyMMdd");
                searchReport.SetPageDataSetting(ReceiptSearchReportList);

                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    receiptReport.Run(false);
                    searchReport.SetPageNumber(receiptReport.Document.Pages.Count);
                    searchReport.Run(false);
                }), false, SessionKey);

                receiptReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchReport.Document.Pages.Clone());

                ShowDialogPreview(ParentForm, receiptReport,GetServerPath());
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
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

        private List<object> CreateReportCondition()
        {
            var search = new List<object>();
            #region DBのため検索条件
            string recordAtFrom = string.IsNullOrWhiteSpace(datRecordedAtFrom.Value.ToString()) ? "(指定なし)" : Convert.ToDateTime(datRecordedAtFrom.Value).ToString("yyyy/MM/dd");
            string recordAtTo = string.IsNullOrWhiteSpace(datRecordedAtTo.Value.ToString()) ? "(指定なし)" : Convert.ToDateTime(datRecordedAtTo.Value).ToString("yyyy/MM/dd");
            string billDrawer = string.IsNullOrWhiteSpace(txtBillDrawer.Text) ? "(指定なし)" : txtBillDrawer.Text.ToString();

            string CustomerCodeFrom = string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text) ? "(指定なし)" : txtCustomerCodeFrom.Text;
            CustomerCodeFrom += string.IsNullOrWhiteSpace(lblCustomerNameFrom.Text) ? " " : " : " + lblCustomerNameFrom.Text;

            string CustomerCodeTo = string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text) ? "(指定なし)" : txtCustomerCodeTo.Text;
            CustomerCodeTo += string.IsNullOrWhiteSpace(lblCustomerNameTo.Text) ? " " : " : " + lblCustomerNameTo.Text;

            string deleteFrom = string.IsNullOrWhiteSpace(datDeleteFrom.Value.ToString()) ? "(指定なし)" : Convert.ToDateTime(datDeleteFrom.Value).ToString("yyyy/MM/dd");
            string deleteTo = string.IsNullOrWhiteSpace(datDeleteTo.Value.ToString()) ? "(指定なし)" : Convert.ToDateTime(datDeleteTo.Value).ToString("yyyy/MM/dd");
            string UpdateFrom = string.IsNullOrWhiteSpace(datUpdateAtFrom.Value.ToString()) ? "(指定なし)" : Convert.ToDateTime(datUpdateAtFrom.Value).ToString("yyyy/MM/dd");
            string UpdateTo = string.IsNullOrWhiteSpace(datUpdateAtTo.Value.ToString()) ? "(指定なし)" : Convert.ToDateTime(datUpdateAtTo.Value).ToString("yyyy/MM/dd");

            string receiptCategoryCodeFrom = string.IsNullOrWhiteSpace(txtCategoryCodeFrom.Text) ? "(指定なし)" : txtCategoryCodeFrom.Text;
            receiptCategoryCodeFrom += string.IsNullOrWhiteSpace(lblCategoryNameFrom.Text) ? " " : " : " + lblCategoryNameFrom.Text;

            string receiptCategoryCodeTo = string.IsNullOrWhiteSpace(txtCategoryCodeTo.Text) ? "(指定なし)" : txtCategoryCodeTo.Text;
            receiptCategoryCodeTo += string.IsNullOrWhiteSpace(lblCategoryNameTo.Text) ? " " : " : " + lblCategoryNameTo.Text;

            string drawerDateFrom = string.IsNullOrWhiteSpace(datBillDrawAtFrom.Value.ToString()) ? "(指定なし)" : Convert.ToDateTime(datBillDrawAtFrom.Value).ToString("yyyy/MM/dd");
            string drawerDateTo = string.IsNullOrWhiteSpace(datBillDrawAtTo.Value.ToString()) ? "(指定なし)" : Convert.ToDateTime(datBillDrawAtTo.Value).ToString("yyyy/MM/dd");

            string existMemo = "";
            if (cbxMemo.Checked)
                existMemo = "有り";

            string useSection = "";
            if (UseSection)
            {
                if (cbxUseReceiptSection.Checked) useSection = "使用";
            }

            string matching = "";
            if (cbxPartAssignment.Checked && cbxNoAssignment.Checked == false)
                matching = "一部消込";
            else if (cbxPartAssignment.Checked == false && cbxNoAssignment.Checked)
                matching += "未消込";
            if (cbxPartAssignment.Checked && cbxNoAssignment.Checked)
                matching = "一部消込 / 未消込";

            string SectionCodeFrom = string.IsNullOrWhiteSpace(txtSectionCodeFrom.Text) ? "(指定なし)" : txtSectionCodeFrom.Text;
            SectionCodeFrom += string.IsNullOrWhiteSpace(lblSectionNameFrom.Text) ? " " : " : " + lblSectionNameFrom.Text;

            string SectionCodeTo = string.IsNullOrWhiteSpace(txtSectionCodeTo.Text) ? "(指定なし)" : txtSectionCodeTo.Text;
            SectionCodeTo += string.IsNullOrWhiteSpace(lblSectionNameTo.Text) ? " " : " : " + lblSectionNameTo.Text;

            string receiptAmountFrom = cmbAmountRange.GetPrintValue();
            receiptAmountFrom += string.IsNullOrWhiteSpace(nmbReceiptAmountFrom.Text) ? " (指定なし)" : " " + nmbReceiptAmountFrom.Text;
            string receiptAmountTo = string.IsNullOrWhiteSpace(nmbReceiptAmountTo.Text) ? "(指定なし)" : nmbReceiptAmountTo.Text;

            string loginUserName = string.IsNullOrWhiteSpace(txtLoginUserCode.Text) ? "(指定なし) " : txtLoginUserCode.Text;
            loginUserName += string.IsNullOrWhiteSpace(lblLoginUserName.Text) ? " " : " : " + lblLoginUserName.Text;

            string currencyCode = string.IsNullOrWhiteSpace(txtCurrencyCode.Text) ? "(指定なし)" : txtCurrencyCode.Text;
            string bankCode = string.IsNullOrWhiteSpace(txtBankCode.Text) ? "(指定なし)" : txtBankCode.Text;
            string branchCode = string.IsNullOrWhiteSpace(txtBranchCode.Text) ? "(指定なし)" : txtBranchCode.Text;
            string accountNumber = string.IsNullOrWhiteSpace(txtAccountNumber.Text) ? "(指定なし)" : txtAccountNumber.Text;
            string payerBankCode = string.IsNullOrWhiteSpace(txtPayerBankCode.Text) ? "(指定なし)" : txtPayerBankCode.Text;
            string payerCodePre = string.IsNullOrWhiteSpace(txtPayerCodePre.Text) ? "(指定なし)" : txtPayerCodePre.Text;
            string payerCodePost = string.IsNullOrWhiteSpace(txtPayerCodePost.Text) ? "(指定なし)" : txtPayerCodePost.Text;
            string note1 = string.IsNullOrWhiteSpace(txtNote1.Text) ? "(指定なし)" : txtNote1.Text;
            string note2 = string.IsNullOrWhiteSpace(txtNote2.Text) ? "(指定なし)" : txtNote2.Text;
            string note3 = string.IsNullOrWhiteSpace(txtNote3.Text) ? "(指定なし)" : txtNote3.Text;
            string note4 = string.IsNullOrWhiteSpace(txtNote4.Text) ? "(指定なし)" : txtNote4.Text;
            string billNumber = string.IsNullOrWhiteSpace(txtBillNumber.Text) ? "(指定なし)" : txtBillNumber.Text;
            string billBankCode = string.IsNullOrWhiteSpace(txtBillBankCode.Text) ? "(指定なし)" : txtBillBankCode.Text;
            string billBranchCode = string.IsNullOrWhiteSpace(txtBillBranchCode.Text) ? "(指定なし)" : txtBillBranchCode.Text;
            string sourceBankName = string.IsNullOrWhiteSpace(txtSourceBankName.Text) ? "(指定なし)" : txtSourceBankName.Text;
            string sourceBranchName = string.IsNullOrWhiteSpace(txtSourceBranchName.Text) ? "(指定なし)" : txtSourceBranchName.Text;
            string payerName = string.IsNullOrWhiteSpace(txtPayerName.Text) ? "(指定なし)" : txtPayerName.Text;
            string billDrawerName = string.IsNullOrWhiteSpace(txtBillDrawer.Text) ? "(指定なし)" : txtBillDrawer.Text;
            string memo = string.IsNullOrWhiteSpace(txtMemo.Text) ? "(指定なし)" : txtMemo.Text;

            search.Add(new SearchData("入金日", recordAtFrom + " ～ " + recordAtTo, "振出人", billDrawerName));

            #region CheckingOfFlgState

            if (cbxDelete.Checked)
            {
                search.Add(new SearchData("削除日", deleteFrom + " ～ " + deleteTo, "得意先コード", CustomerCodeFrom + " ～ " + CustomerCodeTo));

                if (UseSection && UseForeignCurrency)
                {
                    search.Add(new SearchData("振込依頼人名", payerName,
                        "入金部門コード", SectionCodeFrom + " ～ " + SectionCodeTo));
                    search.Add(new SearchData("最終更新日", UpdateFrom + " ～ " + UpdateTo, "メモ有り", existMemo));
                    //最終更新者
                    search.Add(new SearchData("最終更新者", loginUserName,
                            "入金メモ", memo));
                    //通貨コード
                    search.Add(new SearchData("通貨コード", currencyCode, "入金部門対応マスターを使用", useSection));
                    //銀行情報 銀行コード
                    search.Add(new SearchData("銀行情報 銀行コード", bankCode, "入力区分", cmbInputType.SelectedValue.ToString()));
                    //銀行情報 支店コード
                    search.Add(new SearchData("銀行情報 支店コード", branchCode, "消込区分", matching));
                    //銀行情報 預金種別
                    search.Add(new SearchData("銀行情報 預金種別", cmbAccountType.SelectedIndex.ToString() + " : " + cmbAccountType.SelectedValue.ToString(),
                        "入金区分", receiptCategoryCodeFrom + " ～ " + receiptCategoryCodeTo));
                    search.Add(new SearchData("銀行情報 口座番号", accountNumber,
                        "金額範囲", receiptAmountFrom + " ～ " + receiptAmountTo));
                    search.Add(new SearchData("専用入金口座 銀行コード", payerBankCode,
                        "仕向銀行", sourceBankName));
                    search.Add(new SearchData("専用入金口座 支店コード", payerCodePre,
                        "仕向支店", sourceBranchName));
                    search.Add(new SearchData("専用入金口座 口座番号", payerCodePost,
                        lblNote1.Text, note1));
                    search.Add(new SearchData("手形券面情報 手形番号", billNumber,
                         lblNote2.Text, note2));
                    search.Add(new SearchData("手形券面情報 券面銀行コード", billBankCode,
                        lblNote3.Text, note3));
                    search.Add(new SearchData("手形券面情報 券面支店コード", billBranchCode,
                        lblNote4.Text, note4));
                    search.Add(new SearchData("振出日", drawerDateFrom + " ～ " + drawerDateTo, " ", " "));

                }
                else if (UseForeignCurrency && !UseSection)
                {
                    search.Add(new SearchData("振込依頼人名", payerName,
                        "メモ有り", existMemo));
                    search.Add(new SearchData("最終更新日", UpdateFrom + " ～ " + UpdateTo,
                        "入金メモ", memo));
                    //最終更新者
                    search.Add(new SearchData("最終更新者", loginUserName,
                            "入力区分", cmbInputType.SelectedValue.ToString()));
                    //通貨コード
                    search.Add(new SearchData("通貨コード", currencyCode,
                            "消込区分", matching));
                    //銀行情報 銀行コード
                    search.Add(new SearchData("銀行情報 銀行コード", bankCode,
                        "入金区分", receiptCategoryCodeFrom + " ～ " + receiptCategoryCodeTo));
                    //銀行情報 支店コード
                    search.Add(new SearchData("銀行情報 支店コード", branchCode,
                        "金額範囲", receiptAmountFrom + " ～ " + receiptAmountTo));
                    //銀行情報 預金種別
                    search.Add(new SearchData("銀行情報 預金種別", cmbAccountType.SelectedIndex.ToString() + " : " + cmbAccountType.SelectedValue.ToString(),
                        "仕向銀行", sourceBankName));
                    search.Add(new SearchData("銀行情報 口座番号", accountNumber,
                        "仕向支店", sourceBranchName));
                    search.Add(new SearchData("専用入金口座 銀行コード", payerBankCode,
                         lblNote1.Text, note1));
                    search.Add(new SearchData("専用入金口座 支店コード", payerCodePre,
                        lblNote2.Text, note2));
                    search.Add(new SearchData("専用入金口座 口座番号", payerCodePost,
                          lblNote3.Text, note3));
                    search.Add(new SearchData("手形券面情報 手形番号", billNumber,
                        lblNote4.Text, note4));
                    search.Add(new SearchData("手形券面情報 券面銀行コード", billBankCode, " ", " "));
                    search.Add(new SearchData("手形券面情報 券面支店コード", billBranchCode, " ", " "));
                    search.Add(new SearchData("振出日", drawerDateFrom + " ～ " + drawerDateTo, " ", " "));

                }
                else if (!UseForeignCurrency && UseSection)
                {
                    search.Add(new SearchData("振込依頼人名", payerName,
                                                            "入金部門コード", SectionCodeFrom + " ～ " + SectionCodeTo));
                    search.Add(new SearchData("最終更新日", UpdateFrom + " ～ " + UpdateTo, "メモ有り", existMemo));
                    //最終更新者
                    search.Add(new SearchData("最終更新者", loginUserName,
                            "入金メモ", memo));
                    //通貨コード
                    search.Add(new SearchData("銀行情報 銀行コード", bankCode, "入金部門対応マスターを使用", useSection));
                    //銀行情報 銀行コード
                    search.Add(new SearchData("銀行情報 支店コード", branchCode, "入力区分", cmbInputType.SelectedValue.ToString()));
                    //銀行情報 支店コード
                    search.Add(new SearchData("銀行情報 預金種別", cmbAccountType.SelectedIndex.ToString() + " : " + cmbAccountType.SelectedValue.ToString(),
                        "消込区分", matching));
                    //銀行情報 預金種別
                    search.Add(new SearchData("銀行情報 口座番号", accountNumber,
                        "入金区分", receiptCategoryCodeFrom + " ～ " + receiptCategoryCodeTo));
                    search.Add(new SearchData("専用入金口座 銀行コード", payerBankCode,
                        "金額範囲", receiptAmountFrom + " ～ " + receiptAmountTo));
                    search.Add(new SearchData("専用入金口座 支店コード", payerCodePre,
                        "仕向銀行", sourceBankName));
                    search.Add(new SearchData("専用入金口座 口座番号", payerCodePost,
                        "仕向支店", sourceBranchName));
                    search.Add(new SearchData("手形券面情報 手形番号", billNumber,
                         lblNote1.Text, note1));
                    search.Add(new SearchData("手形券面情報 券面銀行コード", billBankCode,
                         lblNote2.Text, note2));
                    search.Add(new SearchData("手形券面情報 券面支店コード", billBranchCode,
                          lblNote3.Text, note3));
                    search.Add(new SearchData("振出日", drawerDateFrom + " ～ " + drawerDateTo,
                        lblNote4.Text, note4));
                }
                else
                {
                    search.Add(new SearchData("振込依頼人名", payerName, "メモ有り", existMemo));
                    search.Add(new SearchData("最終更新日", UpdateFrom + " ～ " + UpdateTo,
                        "入金メモ", memo));
                    //最終更新者
                    search.Add(new SearchData("最終更新者", loginUserName, "入力区分",
                        cmbInputType.SelectedValue.ToString()));
                    //銀行情報 銀行コード
                    search.Add(new SearchData("銀行情報 銀行コード", bankCode,
                        "消込区分", matching));
                    //銀行情報 支店コード
                    search.Add(new SearchData("銀行情報 支店コード", branchCode,
                        "入金区分", receiptCategoryCodeFrom + " ～ " + receiptCategoryCodeTo));
                    //銀行情報 預金種別
                    search.Add(new SearchData("銀行情報 預金種別", cmbAccountType.SelectedIndex.ToString() + " : " + cmbAccountType.SelectedValue.ToString(),
                        "金額範囲", receiptAmountFrom + " ～ " + receiptAmountTo));
                    search.Add(new SearchData("銀行情報 口座番号", accountNumber,
                         "仕向銀行", sourceBankName));
                    search.Add(new SearchData("専用入金口座 銀行コード", payerBankCode,
                       "仕向支店", sourceBranchName));
                    search.Add(new SearchData("専用入金口座 支店コード", payerCodePre,
                        lblNote1.Text, note1));
                    search.Add(new SearchData("専用入金口座 口座番号", payerCodePost,
                        lblNote2.Text, note2));
                    search.Add(new SearchData("手形券面情報 手形番号", billNumber,
                        lblNote3.Text, note3));
                    search.Add(new SearchData("手形券面情報 券面銀行コード", billBankCode,
                         lblNote4.Text, note4));
                    search.Add(new SearchData("手形券面情報 券面支店コード", billBranchCode, " ", " "));
                    search.Add(new SearchData("振出日", drawerDateFrom + " ～ " + drawerDateTo, " ", " "));
                }
            }
            else
            {
                search.Add(new SearchData("振込依頼人名", payerName, "得意先コード",
                    CustomerCodeFrom + " ～ " + CustomerCodeTo));
                if (UseSection && UseForeignCurrency)
                {
                    search.Add(new SearchData("最終更新日", UpdateFrom + " ～ " + UpdateTo,
                        "入金部門コード", SectionCodeFrom + " ～ " + SectionCodeTo));
                    search.Add(new SearchData("最終更新者", loginUserName,
                                        "メモ有り", existMemo));
                    search.Add(new SearchData("通貨コード", currencyCode, "入金メモ", memo));
                    //銀行情報 銀行コード
                    search.Add(new SearchData("銀行情報 銀行コード", bankCode,
                        "入金部門対応マスターを使用", useSection));
                    //銀行情報 支店コード
                    search.Add(new SearchData("銀行情報 支店コード", branchCode,
                        "入力区分", cmbInputType.SelectedValue.ToString()));
                    //銀行情報 預金種別
                    search.Add(new SearchData("銀行情報 預金種別", cmbAccountType.SelectedIndex.ToString() + " : " + cmbAccountType.SelectedValue.ToString(),
                        "消込区分", matching));
                    search.Add(new SearchData("銀行情報 口座番号", accountNumber,
                        "入金区分", receiptCategoryCodeFrom + " ～ " + receiptCategoryCodeTo));
                    search.Add(new SearchData("専用入金口座 銀行コード", payerBankCode,
                        "金額範囲", receiptAmountFrom + " ～ " + receiptAmountTo));
                    search.Add(new SearchData("専用入金口座 支店コード", payerCodePre,
                        "仕向銀行", sourceBankName));
                    search.Add(new SearchData("専用入金口座 口座番号", payerCodePost,
                        "仕向支店", sourceBranchName));
                    search.Add(new SearchData("手形券面情報 手形番号", billNumber,
                        lblNote1.Text, note1));
                    search.Add(new SearchData("手形券面情報 券面銀行コード", billBankCode,
                         lblNote2.Text, note2));
                    search.Add(new SearchData("手形券面情報 券面支店コード", billBranchCode,
                         lblNote3.Text, note3));
                    search.Add(new SearchData("振出日", drawerDateFrom + " ～ " + drawerDateTo,
                         lblNote4.Text, note4));
                }
                else if (!UseSection && UseForeignCurrency)
                {
                    search.Add(new SearchData("最終更新日", UpdateFrom + " ～ " + UpdateTo,
                            "メモ有り", existMemo));
                    search.Add(new SearchData("最終更新者", loginUserName,
                            "入金メモ", memo));
                    search.Add(new SearchData("通貨コード", currencyCode,
                            "入力区分", cmbInputType.SelectedValue.ToString()));
                    //銀行情報 銀行コード
                    search.Add(new SearchData("銀行情報 銀行コード", bankCode,
                            "消込区分", matching));
                    //銀行情報 支店コード
                    search.Add(new SearchData("銀行情報 支店コード", branchCode,
                        "入金区分", receiptCategoryCodeFrom + " ～ " + receiptCategoryCodeTo));
                    //銀行情報 預金種別
                    search.Add(new SearchData("銀行情報 預金種別", cmbAccountType.SelectedIndex.ToString() + " : " + cmbAccountType.SelectedValue.ToString(),
                        "金額範囲", receiptAmountFrom + " ～ " + receiptAmountTo));
                    search.Add(new SearchData("銀行情報 口座番号", accountNumber,
                        "仕向銀行", sourceBankName));
                    search.Add(new SearchData("専用入金口座 銀行コード", payerBankCode,
                        "仕向支店", sourceBranchName));
                    search.Add(new SearchData("専用入金口座 支店コード", payerCodePre, lblNote1.Text, note1));
                    search.Add(new SearchData("専用入金口座 口座番号", payerCodePost, lblNote2.Text, note2));
                    search.Add(new SearchData("手形券面情報 手形番号", billNumber, lblNote3.Text, note3));
                    search.Add(new SearchData("手形券面情報 券面銀行コード", billBankCode, lblNote4.Text, note4));
                    search.Add(new SearchData("手形券面情報 券面支店コード", billBranchCode, " ", " "));
                    search.Add(new SearchData("振出日", drawerDateFrom + " ～ " + drawerDateTo, " ", " "));
                }
                else if (!UseForeignCurrency && UseSection)
                {
                    search.Add(new SearchData("最終更新日", UpdateFrom + " ～ " + UpdateTo,
                        "入金部門コード", SectionCodeFrom + " ～ " + SectionCodeTo));
                    search.Add(new SearchData("最終更新者", loginUserName,
                                        "メモ有り", existMemo));
                    search.Add(new SearchData("銀行情報 銀行コード", bankCode,
                        "入金メモ", memo));
                    //銀行情報 銀行コード
                    search.Add(new SearchData("銀行情報 支店コード", branchCode,
                        "入金部門対応マスターを使用", useSection));
                    //銀行情報 支店コード
                    search.Add(new SearchData("銀行情報 預金種別", cmbAccountType.SelectedIndex.ToString() + " : " + cmbAccountType.SelectedValue.ToString(),
                         "入力区分", cmbInputType.SelectedValue.ToString()));
                    //銀行情報 預金種別
                    search.Add(new SearchData("銀行情報 口座番号", accountNumber, "消込区分", matching));
                    search.Add(new SearchData("専用入金口座 銀行コード", payerBankCode,
                        "入金区分", receiptCategoryCodeFrom + " ～ " + receiptCategoryCodeTo));
                    search.Add(new SearchData("専用入金口座 支店コード", payerCodePre,
                        "金額範囲", receiptAmountFrom + " ～ " + receiptAmountTo));
                    search.Add(new SearchData("専用入金口座 口座番号", payerCodePost,
                        "仕向銀行", sourceBankName));
                    search.Add(new SearchData("手形券面情報 手形番号", billNumber,
                        "仕向支店", sourceBranchName));
                    search.Add(new SearchData("手形券面情報 券面銀行コード", billBankCode,
                        lblNote1.Text, note1));
                    search.Add(new SearchData("手形券面情報 券面支店コード", billBranchCode,
                        lblNote2.Text, note2));
                    search.Add(new SearchData("振出日", drawerDateFrom + " ～ " + drawerDateTo,
                        lblNote3.Text, note3));
                    search.Add(new SearchData(" ", " ", lblNote4.Text, note4));
                }
                else
                {
                    search.Add(new SearchData("最終更新日", UpdateFrom + " ～ " + UpdateTo,
                        "メモ有り", existMemo));
                    search.Add(new SearchData("最終更新者", loginUserName,
                        "入金メモ", memo));
                    //銀行情報 銀行コード
                    search.Add(new SearchData("銀行情報 銀行コード", bankCode,
                        "入力区分", cmbInputType.SelectedValue.ToString()));
                    //銀行情報 支店コード
                    search.Add(new SearchData("銀行情報 支店コード", branchCode, "消込区分", matching));
                    //銀行情報 預金種別
                    search.Add(new SearchData("銀行情報 預金種別", cmbAccountType.SelectedIndex.ToString() + " : " + cmbAccountType.SelectedValue.ToString(),
                        "入金区分", receiptCategoryCodeFrom + " ～ " + receiptCategoryCodeTo));
                    search.Add(new SearchData("銀行情報 口座番号", accountNumber,
                       "金額範囲", receiptAmountFrom + " ～ " + receiptAmountTo));
                    search.Add(new SearchData("専用入金口座 銀行コード", payerBankCode,
                        "仕向銀行", sourceBankName));
                    search.Add(new SearchData("専用入金口座 支店コード", payerCodePre,
                        "仕向支店", sourceBranchName));
                    search.Add(new SearchData("専用入金口座 口座番号", payerCodePost,
                        lblNote1.Text, note1));
                    search.Add(new SearchData("手形券面情報 手形番号", billNumber,
                        lblNote2.Text, note2));
                    search.Add(new SearchData("手形券面情報 券面銀行コード", billBankCode,
                        lblNote3.Text, note3));
                    search.Add(new SearchData("手形券面情報 券面支店コード", billBranchCode,
                        lblNote4.Text, note4));
                    search.Add(new SearchData("振出日", drawerDateFrom + " ～ " + drawerDateTo, " ", " "));
                }
            }
            #endregion
            return search;
        }

        #region エクスポート処理
        [OperationLog("エクスポート")]
        private void ExportData()
        {
            try
            {
                string serverPath = GetServerPath();
                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"未消込入金データ削除{DateTime.Now:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new ReceiptSearchFileDefinition(new DataExpression(ApplicationControl), GridSettingInfo);

                int FieldNumber = (CurrencyId == 1) ? 1 : 0;
                if (definition.CurrencyIdField.Ignored = (!UseForeignCurrency))
                {
                    definition.CurrencyIdField.FieldName = null;
                }

                if (definition.DeleteAtField.Ignored = (!cbxDelete.Checked))
                {
                    definition.DeleteAtField.FieldName = null;
                }

                definition.ExcludeAmountField.FieldName = null;
                definition.ExcludeCategoryNameField.Ignored = true;
                definition.ExcludeCategoryNameField.FieldName = null;

                if ((definition.SectionIdField.Ignored = !UseSection) || (definition.SectionNameField.Ignored = !UseSection))
                {
                    definition.SectionNameField.FieldName = null;
                }

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;
                var format = NoOfPre == 0 ? "0" : "0." + new string('0', NoOfPre);
                definition.ReceiptAmountField.Format = value => value.ToString(format);
                definition.RemainAmountField.Format =  value => value.ToString(format);
                definition.ExcludeAmountField.Format = value => value.ToString(format);

                definition.SetFieldsSetting(GridSettingInfo, definition.ConvertSettingToField);

                definition.ExcludeFlagField.Ignored = true;
                definition.ExcludeCategoryNameField.Ignored = true;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, Receipts, cancel, progress);
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

        /// <summary>管理マスターで設定したサーバパスを取得</summary>
        /// <returns>サーバパス</returns>
        private string GetServerPath()
        {
            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        #endregion

        #region 全選択
        [OperationLog("全選択")]
        private void SelectAll()
        {
            ClearStatusMessage();
            CheckAll(isChecked: true);
        }

        private void CheckAll(bool isChecked)
        {
            grid.EndEdit();
            foreach (var receipt in grid.Rows.Select(x => x.DataBoundItem as Receipt))
                receipt.Checked = isChecked;
            grid.Focus();
        }
        #endregion

        #region 全解除
        [OperationLog("全解除")]
        private void DeselectAll()
        {
            ClearStatusMessage();
            CheckAll(isChecked: false);
        }
        #endregion

        #region 終了
        [OperationLog("終了")]
        private void Exit()
        {
            try
            {
                if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

                Settings.SaveControlValue<PE0601>(Login, cbxCustomerCodeTo.Name, cbxCustomerCodeTo.Checked);
                Settings.SaveControlValue<PE0601>(Login, cbxSectionCodeTo.Name, cbxSectionCodeTo.Checked);
                Settings.SaveControlValue<PE0601>(Login, cbxUseReceiptSection.Name, cbxUseReceiptSection.Checked);
                Settings.SaveControlValue<PE0601>(Login, cbxAmount.Name, cbxAmount.Checked);
                Settings.SaveControlValue<PE0601>(Login, cbxCategoryCodeTo.Name, cbxCategoryCodeTo.Checked);
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
            tbcReceiptOmit.SelectedIndex = 0;
        }
        #endregion

        #region Validate Event

        private void txtPayerName_Validated(object sender, EventArgs e)
        {
            txtPayerName.Text = EbDataHelper.ConvertToPayerName(txtPayerName.Text, LegalPersonalities);
        }

        private void txtLoginUserCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtLoginUserCode.Text))
                {
                    lblLoginUserName.Text = null;
                    LoginUserId = null;
                    ClearStatusMessage();
                    return;
                }

                UsersResult result = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<LoginUserMasterClient>();
                    result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtLoginUserCode.Text });
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result && result.Users.Any())
                {
                    lblLoginUserName.Text = result.Users[0].Name;
                    LoginUserId = result.Users[0].Id;
                    ClearStatusMessage();
                }
                else
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", txtLoginUserCode.Text);
                    txtLoginUserCode.Clear();
                    lblLoginUserName.Text = null;
                    LoginUserId = null;
                    txtLoginUserCode.Select();
                    return;
                }
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
                if (string.IsNullOrEmpty(txtCurrencyCode.Text))
                {
                    ClearStatusMessage();
                    txtCurrencyCode.Clear();
                    CurrencyId = 0;
                    return;
                }

                CurrenciesResult result = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CurrencyMasterClient>();
                    result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result)
                {
                    if (!result.Currencies.Any())
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                        txtCurrencyCode.Clear();
                        CurrencyId = 0;
                        txtCurrencyCode.Select();
                        return;
                    }
                    else
                    {
                        CurrencyId = result.Currencies[0].Id;
                        NoOfPre = result.Currencies[0].Precision;
                        ClearStatusMessage();
                    }
                }
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
                if (txtCustomerCodeFrom.Text == "")
                {
                    lblCustomerNameFrom.Text = null;
                    if (cbxCustomerCodeTo.Checked)
                    {
                        txtCustomerCodeTo.Clear();
                        lblCustomerNameTo.Clear();
                    }
                    ClearStatusMessage();
                    return;
                }
                else
                {
                    List<Customer> customerList = GetDataByCustomerCode(txtCustomerCodeFrom.Text);
                    if (customerList.Any())
                    {
                        lblCustomerNameFrom.Text = customerList[0].Name;
                        if (cbxCustomerCodeTo.Checked)
                        {
                            txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                            lblCustomerNameTo.Text = customerList[0].Name;
                        }
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblCustomerNameFrom.Clear();
                        if (cbxCustomerCodeTo.Checked)
                        {
                            txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                            lblCustomerNameTo.Clear();
                        }
                        ClearStatusMessage();
                    }
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
            try
            {
                if (txtCustomerCodeTo.Text == "")
                {
                    lblCustomerNameTo.Text = null;
                    ClearStatusMessage();
                    return;
                }
                else
                {
                    List<Customer> customerList = GetDataByCustomerCode(txtCustomerCodeTo.Text);
                    if (customerList.Any())
                    {
                        lblCustomerNameTo.Text = customerList[0].Name;
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblCustomerNameTo.Clear();
                        ClearStatusMessage();
                    }
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
            try
            {
                if (txtSectionCodeFrom.Text == "")
                {
                    lblSectionNameFrom.Text = null;
                    if (cbxSectionCodeTo.Checked)
                    {
                        txtSectionCodeTo.Clear();
                        lblSectionNameTo.Clear();
                    }
                    ClearStatusMessage();
                    return;
                }
                else
                {
                    List<Web.Models.Section> sectionList = GetDataBySectionCode(txtSectionCodeFrom.Text);
                    if (sectionList.Any())
                    {
                        lblSectionNameFrom.Text = sectionList[0].Name;
                        if (cbxSectionCodeTo.Checked)
                        {
                            txtSectionCodeTo.Text = txtSectionCodeFrom.Text;
                            lblSectionNameTo.Text = sectionList[0].Name;
                        }
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblSectionNameFrom.Clear();
                        if (cbxSectionCodeTo.Checked)
                        {
                            txtSectionCodeTo.Text = txtSectionCodeFrom.Text;
                            lblSectionNameTo.Clear();
                        }
                        ClearStatusMessage();
                    }
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
                if (txtSectionCodeTo.Text == "")
                {
                    lblSectionNameTo.Text = null;
                    ClearStatusMessage();
                    return;
                }
                else
                {
                    List<Web.Models.Section> sectionList = GetDataBySectionCode(txtSectionCodeTo.Text);
                    if (sectionList.Any())
                    {
                        lblSectionNameTo.Text = sectionList[0].Name;
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblSectionNameTo.Clear();
                        ClearStatusMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCategoryCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtCategoryCodeFrom.Text == "")
                {
                    lblCategoryNameFrom.Text = null;
                    if (cbxCategoryCodeTo.Checked)
                    {
                        txtCategoryCodeTo.Clear();
                        lblCategoryNameTo.Clear();
                    }
                    ClearStatusMessage();
                    return;
                }
                else
                {
                    List<Category> categoryList = GetDataByCategoryCode(txtCategoryCodeFrom.Text);
                    if (categoryList.Any())
                    {
                        lblCategoryNameFrom.Text = categoryList[0].Name;
                        if (cbxCategoryCodeTo.Checked)
                        {
                            txtCategoryCodeTo.Text = txtCategoryCodeFrom.Text;
                            lblCategoryNameTo.Text = categoryList[0].Name;
                        }
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblCategoryNameFrom.Clear();
                        if (cbxCategoryCodeTo.Checked)
                        {
                            txtCategoryCodeTo.Text = txtCategoryCodeFrom.Text;
                            lblCategoryNameTo.Clear();
                        }
                        ClearStatusMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCategoryCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtCategoryCodeTo.Text == "")
                {
                    lblCategoryNameTo.Text = null;
                    ClearStatusMessage();
                    return;
                }
                else
                {
                    List<Category> categoryList = GetDataByCategoryCode(txtCategoryCodeTo.Text);
                    if (categoryList.Any())
                    {
                        lblCategoryNameTo.Text = categoryList[0].Name;
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblCategoryNameTo.Clear();
                        ClearStatusMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void nmbReceiptAmountFrom_Validated(object sender, EventArgs e)
        {
            if(cbxAmount.Checked)
            {
                nmbReceiptAmountTo.Value = nmbReceiptAmountFrom.Value;
            }
        }
        #endregion

        #region 検索ダイアログ Click Event
        private void btnLoginUserCode_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                txtLoginUserCode.Text = loginUser.Code;
                lblLoginUserName.Text = loginUser.Name;
                LoginUserId = loginUser.Id;
                ClearStatusMessage();
            }
        }

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                CurrencyId = currency.Id;
                NoOfPre = currency.Precision;
                ClearStatusMessage();
            }
        }

        private void btnBank_Click(object sender, EventArgs e)
        {
            var bankAccount = this.ShowBankAccountSearchDialog();
            if (bankAccount != null)
            {
                txtBankCode.Text = bankAccount.BankCode;
                txtBranchCode.Text = bankAccount.BranchCode;
                cmbAccountType.SelectedItem = cmbAccountType.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.Tag == bankAccount.AccountTypeId);
                txtAccountNumber.Text = bankAccount.AccountNumber;
                ClearStatusMessage();
            }
        }

        private void btnBankBranch_Click(object sender, EventArgs e)
        {
            var bankBranch = this.ShowBankBranchSearchDialog();
            if (bankBranch != null)
            {
                txtBillBankCode.Text = bankBranch.BankCode;
                txtBillBranchCode.Text = bankBranch.BranchCode;
                ClearStatusMessage();
            }
        }

        private void btnCustomerCodeFrom_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeFrom.Text = customer.Code;
                lblCustomerNameFrom.Text = customer.Name;
                if (cbxCustomerCodeTo.Checked)
                {
                    txtCustomerCodeTo.Text = customer.Code;
                    lblCustomerNameTo.Text = customer.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnCustomerCodeTo_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeTo.Text = customer.Code;
                lblCustomerNameTo.Text = customer.Name;
                ClearStatusMessage();
            }
        }

        private void btnSectionCodeFrom_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                txtSectionCodeFrom.Text = section.Code;
                lblSectionNameFrom.Text = section.Name;
                if (cbxSectionCodeTo.Checked)
                {
                    txtSectionCodeTo.Text = section.Code;
                    lblSectionNameTo.Text = section.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnSectionCodeTo_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                txtSectionCodeTo.Text = section.Code;
                lblSectionNameTo.Text = section.Name;
                ClearStatusMessage();
            }
        }

        private void btnCategoryCodeFrom_Click(object sender, EventArgs e)
        {
            var category = this.ShowReceiptCategorySearchDialog();
            if (category != null)
            {
                txtCategoryCodeFrom.Text = category.Code;
                lblCategoryNameFrom.Text = category.Name;
                if (cbxCategoryCodeTo.Checked)
                {
                    txtCategoryCodeTo.Text = category.Code;
                    lblCategoryNameTo.Text = category.Name;
                }
                ClearStatusMessage();
             }
        }

        private void btnCategoryCodeTo_Click(object sender, EventArgs e)
        {
            var category = this.ShowReceiptCategorySearchDialog();
            if (category != null)
            {
                txtCategoryCodeTo.Text = category.Code;
                lblCategoryNameTo.Text = category.Name;
                ClearStatusMessage();
            }
        }
        #endregion

        #region その他Function
        private List<Customer> GetDataByCustomerCode(string Code)
        {
            var customerList = new List<Customer>();
            Web.Models.CustomersResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { Code });

                if (result.ProcessResult.Result)
                {
                    customerList = result.Customers;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return customerList;
        }

        private List<Web.Models.Section> GetDataBySectionCode(string Code)
        {
            var sectionList = new List<Web.Models.Section>();
            SectionsResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { Code });

                if (result.ProcessResult.Result)
                {
                    sectionList = result.Sections;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return sectionList;
        }

        private List<Category> GetDataByCategoryCode(string Code)
        {
            var categoryList = new List<Category>();
            Web.Models.CategoriesResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Receipt, new[] { Code });

                if (result.ProcessResult.Result)
                {
                    categoryList = result.Categories;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return categoryList;
        }

        private void cbxDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxDelete.Checked)
            {
                datDeleteFrom.Enabled = true;
                datDeleteTo.Enabled = true;
                BaseContext.SetFunction03Caption("復元");
                OnF03ClickHandler = Recovery;
            }
            else
            {
                datDeleteFrom.Clear();
                datDeleteTo.Clear();
                datDeleteFrom.Enabled = false;
                datDeleteTo.Enabled = false;
                BaseContext.SetFunction03Caption("削除");
                OnF03ClickHandler = Delete;
            }
            InitializeGrid();
        }

        public string GetNumberFormat(string displayFieldString, int displayScale, string displayFormatString = "0")
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


        private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            grid.CommitEdit();
        }
        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellIndex != 0) return;
            grid.EndEdit();
        }

        private void grid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.Scope != CellScope.Row) return;
            var requireBackColorChange = (grid.Rows[e.RowIndex].DataBoundItem as Receipt)?.Checked ?? false;
            if (!requireBackColorChange) return;
            e.CellStyle.BackColor = Color.LightCyan;
        }

        #endregion

        #endregion
    }
}
