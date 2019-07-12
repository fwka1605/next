using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金部門振替処理</summary>
    public partial class PD0801 : VOneScreenBase
    {
        public Func<IEnumerable<ITransactionData>, IEnumerable<ReceiptSectionTransfer>, bool> InsertReceiptPostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> UpdateReceiptPostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> DeleteReceiptPostProcessor { get; set; }

        private bool IsPostProcessorImplemented
        {
            get
            {
                return InsertReceiptPostProcessor != null
                    && UpdateReceiptPostProcessor != null
                    && DeleteReceiptPostProcessor != null;
            }
        }

        private int CurrencyId { get; set; }
        private int Precision { get; set; }
        private string CellName(string value) => $"cel{value}";
        private List<string> LegalPersonalities { get; set; }

        public PD0801()
        {
            InitializeComponent();
            grdReceiptData.SetupShortcutKeys();
            Text = "入金部門振替処理";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcReceiptSectionTransfer.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcReceiptSectionTransfer.SelectedIndex == 0)
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
            txtPayerName.Validated += (sender, e)
                => txtPayerName.Text = EbDataHelper.ConvertToPayerName(txtPayerName.Text, LegalPersonalities);
        }

        #region 初期化
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmToClear;

            BaseContext.SetFunction03Caption("登録");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Save;

            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);

            BaseContext.SetFunction05Caption("振替済印刷");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = Print;

            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);

            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);

            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        private void PD0801_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();
                if (Company == null)
                {
                    Task loadCompany = LoadCompanyAsync();
                    loadTask.Add(loadCompany);
                }
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(LoadLegalPersonalitiesAsync());

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                if (ApplicationControl != null)
                {
                    lblCurrencyCode.Visible = UseForeignCurrency;
                    txtCurrencyCode.Visible = UseForeignCurrency;
                    btnCurrencyCode.Visible = UseForeignCurrency;
                }

                InitializeGrid();
                SetFormat();
                Clear();

                //CheckboxのValueを戻す
                string cbxCustomerSetting = Settings.RestoreControlValue<PD0801>(Login, cbxCustomerCodeTo.Name);
                if (!string.IsNullOrEmpty(cbxCustomerSetting))
                {
                    cbxCustomerCodeTo.Checked = Convert.ToBoolean(cbxCustomerSetting);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeGrid()
        {
            var expression = new DataExpression(ApplicationControl);
            var template = new Template();
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  35, "CancelFlag"                     , caption: "解除", sortable: true),
                new CellSetting(height,  90, nameof(Receipt.AssignmentFlag)   , caption: "消込区分", sortable: true),
                new CellSetting(height,  90, nameof(Receipt.RecordedAt)       , caption: "入金日", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.CategoryCodeName) , caption: "入金区分", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.SectionCode)      , caption: "入金部門コード", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.SectionName)      , caption: "入金部門名", sortable: true),
                new CellSetting(height, 130, "TransferSectionCode"            , caption: "振替入金部門コード", sortable: true),
                new CellSetting(height,   0, "btnTransferSectionCode"         , caption: "振替入金部門コードボタン", sortable: true),
                new CellSetting(height, 100, "TransferSectionName"            , caption: "振替入金部門名", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.ReceiptAmount)    , caption: "入金額", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.RemainAmount)     , caption: "入金残", sortable: true),
                new CellSetting(height, 100, "TransferAmount"                 , caption: "振替金額", sortable: true),
                new CellSetting(height, 150, nameof(Receipt.TransferMemo)     , caption: "振替メモ", sortable: true),
                new CellSetting(height,   0, "btnMemo"                        , caption: "メモボタン", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.CustomerCode)     , caption: "得意先コード", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.CustomerName)     , caption: "得意先名", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.PayerName)        , caption: "振込依頼人名", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.BankName)         , caption: "入金銀行", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.BranchName)       , caption: "入金支店", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.SourceBankName)   , caption: "仕向銀行", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.SourceBranchName) , caption: "仕向支店", sortable: true),
                new CellSetting(height, 100, nameof(Receipt.ReceiptMemo)      , caption: "入金メモ", sortable: true),
            });
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  35, "CancelFlag", readOnly: false, cell: builder.GetCheckBoxCell()),
                new CellSetting(height,  90, nameof(Receipt.AssignmentFlagName), enabled: false, dataField: nameof(Receipt.AssignmentFlagName), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), tabStop: false),
                new CellSetting(height,  90, nameof(Receipt.RecordedAt), enabled: false, dataField: nameof(Receipt.RecordedAt), cell: builder.GetDateCell_yyyyMMdd(), tabStop: false),
                new CellSetting(height, 100, nameof(Receipt.CategoryCodeName), enabled: false, dataField: nameof(Receipt.CategoryCodeName), cell: builder.GetTextBoxCell(), tabStop: false),
                new CellSetting(height, 100, nameof(Receipt.SectionCode), enabled: false, dataField: nameof(Receipt.SectionCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), tabStop: false),
                new CellSetting(height, 100, nameof(Receipt.SectionName), enabled: false, dataField: nameof(Receipt.SectionName), cell: builder.GetTextBoxCell(), tabStop: false),
                new CellSetting(height, 100, "TransferSectionCode", readOnly: false, cell: builder.GetTextBoxCell(maxLength: expression.SectionCodeLength, ime: ImeMode.Disable, format: expression.SectionCodeFormatString)),
                new CellSetting(height,  30, "btnTransferSectionCode", cell: builder.GetButtonCell(), value: "…"),
                new CellSetting(height, 100, "TransferSectionName", tabStop: false,enabled: false, cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, nameof(Receipt.ReceiptAmount), tabStop: false,enabled: false, dataField: nameof(Receipt.ReceiptAmount), cell: builder.GetNumberCellCurrency(Precision,Precision,0)),
                new CellSetting(height, 100, nameof(Receipt.RemainAmount), tabStop: false,enabled: false, dataField: nameof(Receipt.RemainAmount), cell: builder.GetNumberCellCurrency(Precision,Precision,0)),
                new CellSetting(height, 100, "TransferAmount",readOnly: false, enabled: false, cell: builder.GetNumberCellCurrency(Precision,Precision,0)),
                new CellSetting(height, 120, nameof(Receipt.TransferMemo), tabStop: false,enabled: false, dataField: nameof(Receipt.TransferMemo), cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana)),
                new CellSetting(height,  30, "btnMemo", readOnly: false, enabled: false, cell: builder.GetButtonCell(), value: "…"),
                new CellSetting(height, 100, nameof(Receipt.CustomerCode), tabStop: false,enabled: false, dataField: nameof(Receipt.CustomerCode), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 100, nameof(Receipt.CustomerName), tabStop: false,enabled: false, dataField: nameof(Receipt.CustomerName), cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, nameof(Receipt.PayerName), tabStop: false,enabled: false, dataField: nameof(Receipt.PayerName), cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, nameof(Receipt.BankName), tabStop: false,enabled: false, dataField: nameof(Receipt.BankName), cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, nameof(Receipt.BranchName), tabStop: false,enabled: false, dataField: nameof(Receipt.BranchName), cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, nameof(Receipt.SourceBankName), tabStop: false,enabled: false, dataField: nameof(Receipt.SourceBankName), cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, nameof(Receipt.SourceBranchName), tabStop: false,enabled: false, dataField: nameof(Receipt.SourceBranchName), cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, nameof(Receipt.ReceiptMemo), tabStop: false,enabled: false, dataField: nameof(Receipt.ReceiptMemo), cell: builder.GetTextBoxCell()),
                new CellSetting(height,   0, nameof(Receipt.UseAdvanceReceived), dataField: nameof(Receipt.UseAdvanceReceived), visible: false),
                new CellSetting(height,   0, nameof(ReceiptSectionTransfer.ReceiptId), dataField: nameof(Receipt.Id), visible: false),
                new CellSetting(height,   0, nameof(ReceiptSectionTransfer.SourceSectionId), dataField: nameof(Receipt.SectionId), visible: false),
                new CellSetting(height,   0, nameof(ReceiptSectionTransfer.DestinationSectionId), visible: false),
            });

            builder.BuildRowOnly(template);
            grdReceiptData.Template = template;
            grdReceiptData.SetRowColor(ColorContext);
            grdReceiptData.HideSelection = true;
        }
        #endregion

        #region 検索
        [OperationLog("検索")]
        private void Search()
        {
            try
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

                if (!CheckData()) return;

                var receiptSearch = PrepareReceiptSearchCondition();
                var receiptDataList = GetReceiptSearchData(receiptSearch);
                SetReceiptData(receiptDataList);
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool CheckData()
        {
            ClearStatusMessage();

            if (!datRecordedAtFrom.ValidateRange(datRecordedAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblRecordedAt.Text))) return false;

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCodeFrom.Text))) return false;

            if (cbxPartAssignment.Checked == false && cbxNoAssignment.Checked == false)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "消込区分");
                btnCategoryCode.Focus();
                SendKeys.Send("{TAB}");
                return false;
            }

            if (UseForeignCurrency && txtCurrencyCode.Text == "")
            {
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Focus();
                return false;
            }
            return true;
        }

        /// <summary> 検索条件のためModelを生成する </summary>
        /// <returns> 生成されたModel </returns>
        private ReceiptSearch PrepareReceiptSearchCondition()
        {
            var receiptSearch = new ReceiptSearch();

            if (datRecordedAtFrom.Value.HasValue)
            {
                receiptSearch.RecordedAtFrom = datRecordedAtFrom.Value?.Date;
            }
            if (datRecordedAtTo.Value.HasValue)
            {
                receiptSearch.RecordedAtTo = datRecordedAtTo.Value?.Date.AddDays(1).AddMilliseconds(-1);
            }
            if (!string.IsNullOrEmpty(txtPayerName.Text))
            {
                receiptSearch.PayerName = txtPayerName.Text;
            }
            if (!string.IsNullOrEmpty(txtCategoryCode.Text))
            {
                receiptSearch.ReceiptCategoryCode = txtCategoryCode.Text;
            }

            var assignments
                = (cbxNoAssignment  .Checked ? (int)AssignmentFlagChecked.NoAssignment   : (int)AssignmentFlagChecked.None)
                | (cbxPartAssignment.Checked ? (int)AssignmentFlagChecked.PartAssignment : (int)AssignmentFlagChecked.None);
            receiptSearch.AssignmentFlag = assignments;

            if (!string.IsNullOrEmpty(txtCustomerCodeFrom.Text))
            {
                receiptSearch.CustomerCodeFrom = txtCustomerCodeFrom.Text;
            }
            if (!string.IsNullOrEmpty(txtCustomerCodeTo.Text))
            {
                receiptSearch.CustomerCodeTo = txtCustomerCodeTo.Text;
            }
            if (!string.IsNullOrEmpty(txtSectionCode.Text))
            {
                receiptSearch.SectionCode = txtSectionCode.Text;
            }
            if (cbxMemo.Checked)
            {
                receiptSearch.ExistsMemo = 1;
            }
            else
            {
                receiptSearch.ExistsMemo = 0;
            }
            if (!string.IsNullOrEmpty(txtMemo.Text))
            {
                receiptSearch.ReceiptMemo = txtMemo.Text;
            }
            if (UseForeignCurrency)
            {
                receiptSearch.UseForeignCurrencyFlg = UseForeignCurrency ? 1 : 0;

                if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    receiptSearch.CurrencyId = CurrencyId;
                }
            }
            return receiptSearch;
        }

        /// <summary> 検索条件でReceiptデータを取得 </summary>
        /// <param name="receiptSearch">　検索条件のModel　</param>
        /// <returns> Receipt List </returns>
        private List<Receipt> GetReceiptSearchData(ReceiptSearch receiptSearch)
        {
            List<Receipt> receiptDataList = null;
            receiptSearch.CompanyId = CompanyId;
            receiptSearch.LoginUserId = Login.UserId;
            ReceiptsResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptServiceClient>();
                result = await service.GetItemsAsync(SessionKey, receiptSearch);

            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (result.ProcessResult.Result)
            {
                receiptDataList = result.Receipts.ToList().GroupBy(c => new { c.Id }).Select(c => c.First()).ToList();
            }
            return receiptDataList;
        }

        /// <summary> 検索条件で取得したデータを設定 </summary>
        /// <param name="receiptDataList">　検索条件で取得したデータList　</param>
        private void SetReceiptData(List<Receipt> receiptDataList)
        {
            if (receiptDataList != null && receiptDataList.Any())
            {
                var receiptTotal = 0M;
                var remainTotal = 0M;

                InitializeGrid();
                grdReceiptData.DataSource = new BindingSource(receiptDataList, null);
                receiptTotal = grdReceiptData.Rows.Where(x => !string.IsNullOrEmpty(x[CellName(nameof(Receipt.ReceiptAmount))].DisplayText.ToString()))
                        .Sum(x => Convert.ToDecimal(x[CellName(nameof(Receipt.ReceiptAmount))].DisplayText.ToString()));
                remainTotal = grdReceiptData.Rows.Where(x => !string.IsNullOrEmpty(x[CellName(nameof(Receipt.RemainAmount))].DisplayText.ToString()))
                    .Sum(x => Convert.ToDecimal(x[CellName(nameof(Receipt.RemainAmount))].DisplayText.ToString()));
                lblCount.Text = grdReceiptData.RowCount.ToString("#,##0");
                lblReceiptAmountTotal.Text = GetAmountPrecision(receiptTotal);
                lblRemainAmountTotal.Text = GetAmountPrecision(remainTotal);
                tbcReceiptSectionTransfer.SelectedIndex = 1;
                BaseContext.SetFunction03Enabled(true);

                for (int i = 0; i < receiptDataList.Count; i++)
                {
                    if (receiptDataList[i].CancelFlag == false)
                        grdReceiptData.Rows[i].Cells[CellName("CancelFlag")].Enabled = false;
                    else
                        grdReceiptData.Rows[i].Cells[CellName("CancelFlag")].Enabled = true;
                }
            }
            else
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                BaseContext.SetFunction03Enabled(false);
                grdReceiptData.Rows.Clear();
                lblCount.Text = "0";
                lblReceiptAmountTotal.Text = "0";
                lblRemainAmountTotal.Text = "0";
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
            datRecordedAtFrom.Clear();
            datRecordedAtTo.Clear();
            txtPayerName.Clear();
            txtCategoryCode.Clear();
            lblCategoryName.Clear();
            txtCustomerCodeFrom.Clear();
            lblCustomerNameFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameTo.Clear();
            txtSectionCode.Clear();
            lblSectionName.Clear();
            txtMemo.Clear();
            txtCurrencyCode.Clear();
            lblCount.Clear();
            lblReceiptAmountTotal.Clear();
            lblRemainAmountTotal.Clear();
            cbxMemo.Checked = false;
            cbxPartAssignment.Checked = true;
            cbxNoAssignment.Checked = true;
            grdReceiptData.Rows.Clear();
            ClearStatusMessage();
            BaseContext.SetFunction03Enabled(false);
            tbcReceiptSectionTransfer.SelectedIndex = 0;
            datRecordedAtFrom.Select();
            Modified = false;         
        }
        #endregion

        #region 登録
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                grdReceiptData.EndEdit();
                if (!RequiredCheck()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var rectiptSectionTransferList = PrepareReceiptSectionTransfer();

                ReceiptSectionTransfersResult result = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<ReceiptServiceClient>();
                    result = await service.SaveReceiptSectionTransferAsync(SessionKey, rectiptSectionTransferList.ToArray(), Login.UserId);
                });

                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                var success = result.ProcessResult.Result;
                var syncResult = true;
                if (success && IsPostProcessorImplemented)
                {
                    if (result.DeleteReceipts.Any())
                    {
                        syncResult = DeleteReceiptPostProcessor.Invoke(result.DeleteReceipts);
                        success &= syncResult;
                    }
                    if (success && result.InsertReceipts.Any())
                    {
                        syncResult = InsertReceiptPostProcessor.Invoke(result.InsertReceipts, result.ReceiptSectionTransfers);
                        success &= syncResult;
                    }
                    if (success && result.UpdateReceipts.Any())
                    {
                        syncResult = UpdateReceiptPostProcessor.Invoke(result.UpdateReceipts);
                        success &= syncResult;
                    }
                }
                if (!syncResult)
                {
                    ShowWarningDialog(MsgErrPostProcessFailure);
                    return;
                }
                if (!success)
                {
                    if (!result.NotClearFlag)
                    {
                        ShowWarningDialog(MsgWngNotClearDataOfTransferCannotCancel);
                        return;
                    }
                    if (result.TransferFlag)
                    {
                        ShowWarningDialog(MsgWngTransferToOtherSectionAndCannotCancel);
                        return;
                    }
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }

                var receiptSearch = PrepareReceiptSearchCondition();
                var receiptDataList = GetReceiptSearchData(receiptSearch);
                SetReceiptData(receiptDataList);
                Modified = false;
                tbcReceiptSectionTransfer.SelectedIndex = 1;
                DispStatusMessage(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool RequiredCheck()
        {
            ClearStatusMessage();

            int cancleFlgCount = grdReceiptData.Rows
               .Where(x => Convert.ToBoolean(x[CellName("CancelFlag")].EditedFormattedValue)).Count();

            int amountCount = grdReceiptData.Rows
                .Where(x => !string.IsNullOrEmpty(x[CellName("TransferSectionCode")].DisplayText.ToString())).Count();

            if (cancleFlgCount == 0 && amountCount == 0)
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "更新するデータ");
                grdReceiptData.Rows[0].Cells[CellName("CancelFlag")].Selected = true;
                return false;
            }

            foreach (Row row in grdReceiptData.Rows)
            {
                if (row.Cells[CellName("TransferSectionCode")].Value != null &&
                    row.Cells[CellName(nameof(Receipt.UseAdvanceReceived))].Value.ToString() == "1")
                {
                    ShowWarningDialog(MsgWngAdvancedReceiptDataCannotxxx, "振替処理");
                    return false;
                }
            }
            return true;
        }

        /// <summary>　登録のためReceiptSectionTransferデータを準備</summary>
        /// <returns> 作成されたReceiptSectionTransfer List</returns>
        private List<ReceiptSectionTransfer> PrepareReceiptSectionTransfer()
        {
            var receiptSectionTransferList = new List<ReceiptSectionTransfer>();
            receiptSectionTransferList.Clear();

            foreach (Row row in grdReceiptData.Rows)
            {
                if (Convert.ToInt32(row.Cells[CellName("CancelFlag")].Value) == 1 || Convert.ToBoolean(row.Cells[CellName("CancelFlag")].Value) ||
                    row.Cells[CellName("TransferSectionCode")].Value != null)
                {
                    var receiptSectionTransfer = new ReceiptSectionTransfer();

                    receiptSectionTransfer.ReceiptId = Convert.ToInt64(row.Cells[CellName(nameof(ReceiptSectionTransfer.ReceiptId))].Value);
                    receiptSectionTransfer.CancelFlag = Convert.ToBoolean(row.Cells[CellName("CancelFlag")].Value);
                    receiptSectionTransfer.SourceSectionId = Convert.ToInt32(row.Cells[CellName(nameof(ReceiptSectionTransfer.SourceSectionId))].Value);
                    receiptSectionTransfer.DestinationSectionId = Convert.ToInt32(row.Cells[CellName(nameof(ReceiptSectionTransfer.DestinationSectionId))].Value);
                    receiptSectionTransfer.SourceAmount = Convert.ToDecimal(row.Cells[CellName(nameof(Receipt.RemainAmount))].Value);
                    receiptSectionTransfer.DestinationAmount = Convert.ToDecimal(row.Cells[CellName("TransferAmount")].Value);
                    receiptSectionTransfer.TransferMemo = row.Cells[CellName(nameof(Receipt.TransferMemo))].Value?.ToString();
                    receiptSectionTransferList.Add(receiptSectionTransfer);
                }
            }
            return receiptSectionTransferList;
        }
        #endregion

        #region 振替済印刷
        [OperationLog("振替済印刷")]
        private void Print()
        {
            ClearStatusMessage();

            try
            {
                if (!ShowConfirmDialog(MsgQstPrintReceiptSectionTransferData))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var receiptSectionTransferPrintList = GetReceiptSectionTransferForPrint();

                if (receiptSectionTransferPrintList != null && receiptSectionTransferPrintList.Any())
                {
                    int maxPrecision = 0;
                    if (UseForeignCurrency)
                        maxPrecision = receiptSectionTransferPrintList.Max(x => x.Precision);

                    Invoke(new System.Action(() =>
                    {
                        var receiptSectionTransferReport = new ReceiptSectionTransferSectionReport();
                        receiptSectionTransferReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        receiptSectionTransferReport.Name = "入金部門振替済チェックリスト" + DateTime.Today.ToString("yyyyMMdd");
                        receiptSectionTransferReport.SetData(receiptSectionTransferPrintList, UseForeignCurrency, maxPrecision);

                        ProgressDialog.Start(ParentForm, Task.Run(() =>
                        {
                            receiptSectionTransferReport.Run(false);
                        }), false, SessionKey);

                        ShowDialogPreview(ParentForm,
                            receiptSectionTransferReport, getExportPdfPath(),
                            owner => owner.Invoke(new System.Action(() => UpdateReceiptSectionTransferPrintFlag(owner))));
                    }));
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

        private string getExportPdfPath()
        {
            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        /// <summary> 印刷のためデータを取得する </summary>
        /// <returns>　取得したReceiptSectionTransfer List</returns>
        private List<ReceiptSectionTransfer> GetReceiptSectionTransferForPrint()
        {
            List<ReceiptSectionTransfer> receiptSectionTransferList = null;
            ReceiptSectionTransfersResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptServiceClient>();
                result = await service.GetReceiptSectionTransferForPrintAsync(SessionKey, CompanyId);
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (result.ProcessResult.Result)
            {
                receiptSectionTransferList = result.ReceiptSectionTransfers;
            }
            return receiptSectionTransferList;
        }

        private void UpdateReceiptSectionTransferPrintFlag(Form owner)
        {
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptServiceClient>();
                await service.UpdateReceiptSectionTransferPrintFlagAsync(SessionKey, CompanyId);
            });
            ProgressDialog.Start(owner, task, false, SessionKey);
        }
        #endregion

        #region 終了
        [OperationLog("終了")]
        private void Exit()
        {
            try
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

                Settings.SaveControlValue<PD0801>(Login, cbxCustomerCodeTo.Name, cbxCustomerCodeTo.Checked);

                ClearStatusMessage();
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
            tbcReceiptSectionTransfer.SelectedIndex = 0;
        }

        #endregion

        #region 検索ボタンClick Event
        private void btnCategoryCode_Click(object sender, EventArgs e)
        {
            var receiptCategory = this.ShowReceiptCategorySearchDialog();
            if (receiptCategory != null)
            {
                txtCategoryCode.Text = receiptCategory.Code;
                lblCategoryName.Text = receiptCategory.Name;
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

        private void btnSectionCode_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                txtSectionCode.Text = section.Code;
                lblSectionName.Text = section.Name;
                ClearStatusMessage();
            }
        }

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                Precision = currency.Precision;
                CurrencyId = currency.Id;
                ClearStatusMessage();
            }
        }
        #endregion

        #region Validated Event

        private void grdReceiptData_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space) && grdReceiptData.CurrentRow.Index >= 0)
            {
                if (grdReceiptData.CurrentCell.Name == CellName("btnMemo") && grdReceiptData.CurrentCell.ReadOnly == false
                    && grdReceiptData.CurrentCell.Enabled)
                {
                    ShowMemoDialog(grdReceiptData.CurrentRow.Index);
                    Modified = true;
                }
                else if (grdReceiptData.CurrentCell.Name == CellName("btnTransferSectionCode") && grdReceiptData.CurrentCell.ReadOnly == false
                    && grdReceiptData.CurrentCell.Enabled)
                {
                    ShowTransferSectionDialog(grdReceiptData.CurrentRow.Index);
                    Modified = true;
                }
            }
        }

        private void txtCategoryCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();

                CategoriesResult result = null;
                var categoryCode = txtCategoryCode.Text;

                if (string.IsNullOrEmpty(categoryCode) || string.IsNullOrWhiteSpace(categoryCode))
                {
                    lblCategoryName.Clear();
                    return;
                }

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CategoryMasterClient>();
                    result = await service.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Receipt, new string[] { txtCategoryCode.Text });
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result != null && result.Categories.Any())
                {
                    txtCategoryCode.Text = result.Categories[0].Code;
                    lblCategoryName.Text = result.Categories[0].Name;
                }
                else
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "入金区分", txtCategoryCode.Text);
                    txtCategoryCode.Clear();
                    lblCategoryName.Clear();
                    txtCategoryCode.Focus();
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
                if (string.IsNullOrEmpty(txtCustomerCodeFrom.Text)
                                    || string.IsNullOrWhiteSpace(txtCustomerCodeFrom.Text))
                {
                    lblCustomerNameFrom.Clear();
                    if (cbxCustomerCodeTo.Checked)
                    {
                        txtCustomerCodeTo.Clear();
                        lblCustomerNameTo.Clear();
                    }
                    ClearStatusMessage();
                }
                else
                {
                    List<Customer> customerList = GetDataByCustomerCode(txtCustomerCodeFrom.Text);

                    if (customerList.Any())
                    {
                        lblCustomerNameFrom.Text = (customerList[0].Name.ToString());
                        if (cbxCustomerCodeTo.Checked)
                        {
                            txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                            lblCustomerNameTo.Text = (customerList[0].Name.ToString());
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
                if (string.IsNullOrEmpty(txtCustomerCodeTo.Text)
                                    || string.IsNullOrWhiteSpace(txtCustomerCodeTo.Text))
                {
                    lblCustomerNameTo.Clear();
                    ClearStatusMessage();
                }
                else
                {
                    List<Customer> customerList = GetDataByCustomerCode(txtCustomerCodeTo.Text);

                    if (customerList.Any())
                    {
                        lblCustomerNameTo.Text = (customerList[0].Name.ToString());
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

        private void txtSectionCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();

                if (string.IsNullOrEmpty(txtSectionCode.Text)
                    || string.IsNullOrWhiteSpace(txtSectionCode.Text))
                {
                    lblSectionName.Clear();
                }
                else
                {
                    Web.Models.Section section = GetSectionData(txtSectionCode.Text);
                    if (section != null)
                    {
                        txtSectionCode.Text = section.Code;
                        lblSectionName.Text = section.Name;
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "入金部門", txtSectionCode.Text);
                        txtSectionCode.Clear();
                        lblSectionName.Clear();
                        txtSectionCode.Focus();
                    }
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
                CurrenciesResult result = null;
                if (!string.IsNullOrEmpty(txtCurrencyCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CurrencyMasterClient>();
                        result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (result.ProcessResult.Result)
                    {
                        if (result.Currencies.Any())
                        {
                            ClearStatusMessage();
                            txtCurrencyCode.Text = result.Currencies[0].Code;
                            Precision = result.Currencies[0].Precision;
                            CurrencyId = result.Currencies[0].Id;
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                            txtCurrencyCode.Clear();
                            txtCurrencyCode.Focus();
                            CurrencyId = 0;
                        }
                    }
                }
                else
                {
                    ClearStatusMessage();
                    txtCurrencyCode.Clear();
                    CurrencyId = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region Grid Cell Validated Event
        private void grdReceiptData_CellValidated(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var transferSectionCodeCell = grdReceiptData.Rows[e.RowIndex].Cells[CellName("TransferSectionCode")];
                var transferAmount = grdReceiptData.Rows[e.RowIndex].Cells[CellName("TransferAmount")];

                if (e.CellName == CellName("TransferSectionCode") && transferSectionCodeCell.ReadOnly == false)
                {
                    // 振替入金部門コード Validated
                    ValidateTransferSectionCode(e.RowIndex);
                }
                else if (e.CellName == CellName("TransferAmount") && transferAmount.Enabled)
                {
                    // 振替金額　Validate
                    ValidateTransferAmount(e.RowIndex);
                    Modified = true;
                }
            }
        }

        /// <summary> 振替入金部門コード Validated </summary>
        /// <param name="rowIndex"> Validatedした行のIndex</param>
        private void ValidateTransferSectionCode(int rowIndex)
        {
            ClearStatusMessage();

            if (grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Value == null)
            {
                ClearInputDataInGrid(rowIndex);
            }
            else if (grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Value.ToString() == grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.SectionCode))].Value.ToString())
            {
                ShowWarningDialog(MsgWngCannotTransferSameReceiptSection);
                ClearInputDataInGrid(rowIndex);
                Modified = true;
            }
            else
            {
                var code = grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Value.ToString();
                if (ApplicationControl.SectionCodeType == 0)
                    code = code.PadLeft(ApplicationControl.SectionCodeLength, '0');

                Web.Models.Section section = GetSectionData(code);
                if (section == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "入金部門", code);
                    ClearInputDataInGrid(rowIndex);
                }
                else
                {
                    grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Value = section.Code;
                    grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionName")].Value = section.Name;
                    grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(ReceiptSectionTransfer.DestinationSectionId))].Value = section.Id;
                    grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Enabled = true;
                    grdReceiptData.Rows[rowIndex].Cells[CellName("btnMemo")].Enabled = true;
                    grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value = grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.RemainAmount))].Value;
                }
                Modified = true;
            }
        }

        /// <summary>Gridの入力するCellをクリア</summary>
        /// <param name="rowIndex">Validatedした行のIndex</param>
        private void ClearInputDataInGrid(int rowIndex)
        {
            grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Value = null;
            grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(ReceiptSectionTransfer.DestinationSectionId))].Value = null;
            grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionName")].Value = null;
            grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value = null;
            grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Enabled = false;
            grdReceiptData.Rows[rowIndex].Cells[CellName("btnMemo")].Enabled = false;
        }

        /// <summary> 振替金額 Validated </summary>
        /// <param name="rowIndex"> Validatedした行のIndex</param>
        private void ValidateTransferAmount(int rowIndex)
        {
            ClearStatusMessage();
            var transferAmount = Convert.ToDecimal(grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value);
            var remainAmount = Convert.ToDecimal(grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.RemainAmount))].Value);
            if (transferAmount == 0)
            {
                ShowWarningDialog(MsgWngInputRequired, "振替金額");
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value = grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.RemainAmount))].Value;
            }
            else if (transferAmount > remainAmount)
            {
                ShowWarningDialog(MsgWngInputRequired, "入金残よりも少ない振替金額");
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value = grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.RemainAmount))].Value;
            }
        }
        #endregion

        #region Grid Button Click Event
        private void grdReceiptData_CellContentButtonClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var transferSectionCodeCell = grdReceiptData.Rows[e.RowIndex].Cells[CellName("TransferSectionCode")];
                var btnMemoCell = grdReceiptData.Rows[e.RowIndex].Cells[CellName("btnMemo")];

                // 振替入金部門コード 検索ボタン
                if (e.CellName == CellName("btnTransferSectionCode") && transferSectionCodeCell.ReadOnly == false)
                {
                    ShowTransferSectionDialog(e.RowIndex);
                    Modified = true;
                }
                // 振替メモ　ボタン
                else if (e.CellName == CellName("btnMemo") && btnMemoCell.ReadOnly == false)
                {
                    ShowMemoDialog(e.RowIndex);
                    Modified = true;
                }
            }
        }

        private void ShowTransferSectionDialog(int rowIndex)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                if (grdReceiptData.Rows[rowIndex].Cells["celSectionCode"].Value?.ToString() == section.Code)
                {
                    ShowWarningDialog(MsgWngCannotTransferSameReceiptSection);
                    grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Value = null;
                    grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(ReceiptSectionTransfer.DestinationSectionId))].Value = null;
                    grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionName")].Value = null;
                    grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value = null;
                    grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.TransferMemo))].Value = null;
                    grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Enabled = false;
                    grdReceiptData.Rows[rowIndex].Cells[CellName("btnMemo")].Enabled = false;
                    return;
                }
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Value = section.Code;
                grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(ReceiptSectionTransfer.DestinationSectionId))].Value = section.Id;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionName")].Value = section.Name;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Enabled = true;
                grdReceiptData.Rows[rowIndex].Cells[CellName("btnMemo")].Enabled = true;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value = grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.RemainAmount))].Value;
                ClearStatusMessage();
            }
        }

        private void ShowMemoDialog(int rowIndex)
        {
            var memo = "";
            var memoCell = grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.TransferMemo))];

            if (memoCell.Value != null)
                memo = memoCell.Value.ToString();

            using (var form = ApplicationContext.Create(nameof(PH9906)))
            {
                var screen = form.GetAll<PH9906>().First();
                screen.Id = 0L;
                screen.MemoType = MemoType.TransferMemo;
                screen.Memo = memo;
                screen.InitializeParentForm("振替メモ");
                DialogResult memoDialog = ApplicationContext.ShowDialog(ParentForm, form, true);
                if (memoDialog == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(screen.Memo))
                    {
                        memoCell.Value = screen.Memo;
                    }
                    else
                    {
                        memoCell.Value = null;
                    }
                }
            }
        }
        #endregion

        #region 解除 CheckedChanged のため

        private void grdReceiptData_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cbxCancleFlagCell = grdReceiptData.Rows[e.RowIndex].Cells[CellName("CancelFlag")];

                if (e.CellName == CellName("CancelFlag") && cbxCancleFlagCell.Enabled)
                {
                    // 解除 CheckedChanged
                    CheckChangedByCheckBox(e.RowIndex);
                    Modified = true;
                }
            }
        }

        private void grdReceiptData_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cbxCancelFlagCell = grdReceiptData.Rows[e.RowIndex].Cells[CellName("CancelFlag")];

                if (e.CellName == CellName("CancelFlag") && cbxCancelFlagCell.Enabled)
                {
                    // 解除 CheckedChanged
                    CheckChangedByCheckBox(e.RowIndex);
                    Modified = true;
                }
            }
        }

        /// <summary> Gridにある解除 CheckedChanged Event </summary>
        /// <param name="rowIndex">チェックした行のIndex</param>
        private void CheckChangedByCheckBox(int rowIndex)
        {
            var checkValue = Convert.ToBoolean(grdReceiptData.Rows[rowIndex].Cells[CellName("CancelFlag")].EditedFormattedValue);
            if (checkValue)
            {
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Value = null;
                grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(ReceiptSectionTransfer.DestinationSectionId))].Value = null;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionName")].Value = null;
                grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.TransferMemo))].Value = null;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value = grdReceiptData.Rows[rowIndex].Cells[CellName(nameof(Receipt.RemainAmount))].Value;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].ReadOnly = true;
                grdReceiptData.Rows[rowIndex].Cells[CellName("btnTransferSectionCode")].ReadOnly = true;
                grdReceiptData.Rows[rowIndex].Cells[CellName("btnMemo")].ReadOnly = true;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].ReadOnly = true;

                grdReceiptData.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
                for (int i = 0; i < grdReceiptData.Columns.Count; i++)
                {
                    grdReceiptData.Rows[rowIndex].Cells[i].Enabled = true;
                }
            }
            else
            {
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].ReadOnly = false;
                grdReceiptData.Rows[rowIndex].Cells[CellName("btnTransferSectionCode")].ReadOnly = false;
                grdReceiptData.Rows[rowIndex].Cells[CellName("btnMemo")].ReadOnly = false;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].ReadOnly = false;

                grdReceiptData.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Empty;
                for (int i = 0; i < grdReceiptData.Columns.Count; i++)
                {
                    grdReceiptData.Rows[rowIndex].Cells[i].Enabled = false;
                }
                grdReceiptData.Rows[rowIndex].Cells[CellName("CancelFlag")].Enabled = true;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferSectionCode")].Enabled = true;
                grdReceiptData.Rows[rowIndex].Cells[CellName("btnTransferSectionCode")].Enabled = true;
                grdReceiptData.Rows[rowIndex].Cells[CellName("TransferAmount")].Value = null;
            }
        }
        #endregion

        #region その他Function
        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);
            txtCustomerCodeFrom.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeFrom.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeFrom.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;

            txtCustomerCodeTo.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeTo.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeTo.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeTo.PaddingChar = expression.CustomerCodePaddingChar;

            txtSectionCode.Format = expression.SectionCodeFormatString;
            txtSectionCode.MaxLength = expression.SectionCodeLength;
            txtSectionCode.PaddingChar = expression.SectionCodePaddingChar;
        }

        /// <summary> 得意先コードでデータを取得する </summary>
        /// <param name="Code">得意先コード </param>
        /// <returns> Customer List </returns>
        private List<Customer> GetDataByCustomerCode(string code)
        {
            var customerList = new List<Customer>();
            Web.Models.CustomersResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (result.ProcessResult.Result)
            {
                customerList = result.Customers;
            }
            return customerList;
        }

        /// <summary> 入金部門コードでデータを取得する </summary>
        /// <param name="code">入金部門コード </param>
        /// <returns> 入金部門データ </returns>
        private Web.Models.Section GetSectionData(string code)
        {
            Web.Models.Section section = null;
            SectionsResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result && result.Sections.Any())
            {
                section = result.Sections[0];
            }
            return section;
        }

        /// <summary> Setting value with precision </summary>
        /// <param name="amount"> Value to set precision </param>
        /// <returns> Value with precision </returns>
        private string GetAmountPrecision(decimal amount)
        {
            string amountWithPrecision;
            var displayFormat = "#,###,###,###,##0";
            var displayFormatString = "0";

            if (Precision > 0)
            {
                displayFormat += ".";
                for (int i = 0; i < Precision; i++)
                {
                    displayFormat += displayFormatString;
                }
            }
            amountWithPrecision = amount.ToString(displayFormat);

            return amountWithPrecision;
        }

        private async Task LoadLegalPersonalitiesAsync()
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
