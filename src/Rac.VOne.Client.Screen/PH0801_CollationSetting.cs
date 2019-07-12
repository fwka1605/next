using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.Editors;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Common.MultiRow;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Screen.CollationSettingMasterService;
using Rac.VOne.Client.Common;
using Rac.VOne.Web.Models;
using System.Diagnostics;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary> 照合ロジック設定 </summary>
    public partial class PH0801 : VOneScreenBase
    {
        private int DragDropDestRowIndex { get; set; } = -1;
        private Dictionary<int, string> CollationOrderName { get; set; } = new Dictionary<int, string>();
        private string CellName(string value) => $"cel{value}";

        #region Initialize

        public PH0801()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            grdCollationOrder.SetupShortcutKeys();
            grdMatchingBillingOrder.SetupShortcutKeys();
            grdMatchingReceiptOrder.SetupShortcutKeys();
            Text = "照合ロジック設定";
        }

        private void InitializeHandlers()
        {
            foreach (var control in this.GetAll<Control>())
            {
                if (control is CheckBox)
                    ((CheckBox)control).CheckedChanged += OnChanged;
                if (control is RadioButton)
                    ((RadioButton)control).CheckedChanged += OnChanged;
            }
            cmbAdvanceReceivedRecordedDateType.SelectedIndexChanged += OnChanged;
            cmbAdvanceReceivedRecordedDateType.SelectedValueChanged += OnChanged;
            cmbSortOrderColumn.SelectedIndexChanged += OnChanged;
            cmbSortOrderColumn.SelectedValueChanged += OnChanged;
            cmbSortOrder.SelectedIndexChanged += OnChanged;
            cmbSortOrder.SelectedValueChanged += OnChanged;
            grdMatchingBillingOrder.CurrentCellDirtyStateChanged    += OnChanged;
            grdMatchingReceiptOrder.CurrentCellDirtyStateChanged    += OnChanged;
            grdCollationOrder.CurrentCellDirtyStateChanged          += OnChanged;
        }


        private void PH0801_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

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

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask.ToArray()), false, SessionKey);

                SetComboData();

                InitializeLocalDictionary();

                SetInitialCollationSetting();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetInitialCollationSetting()
        {
            var loadTask = new List<Task>();
            var collationSetting = new CollationSetting();
            var collationOrder = new List<CollationOrder>();
            var matchingBillingOrder = new List<MatchingOrder>();
            var matchingReceiptOrder = new List<MatchingOrder>();

            var loadCollationSettingTask = GetCollationSettingAsync();
            loadCollationSettingTask.ContinueWith(task => collationSetting = task.Result);
            loadTask.Add(loadCollationSettingTask);

            var loadCollationOrderTask = GetCollationOrdersAsync();
            loadCollationOrderTask.ContinueWith(task => collationOrder.AddRange(task.Result));
            loadTask.Add(loadCollationOrderTask);

            var loadBillingOrderTask = GetBillingOrdersAsync();
            loadBillingOrderTask.ContinueWith(task => matchingBillingOrder.AddRange(task.Result));
            loadTask.Add(loadBillingOrderTask);

            var loadReceiptOrderTask = GetReceiptOrdersAsync();
            loadReceiptOrderTask.ContinueWith(task => matchingReceiptOrder.AddRange(task.Result));
            loadTask.Add(loadReceiptOrderTask);

            ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask.ToArray()), false, SessionKey);

            ClearCollation(collationSetting);

            CreateCollationOrderGridTemplate();

            DisplayCollationOrderGrid(collationOrder);

            CreateBillingOrderGridTemplate();

            DisplayBillingOrderGrid(matchingBillingOrder);

            CreateReceiptOrderGridTemplate();

            DisplayReceiptOrderGrid(matchingReceiptOrder);

            Modified = false;

            cbxReloadCollationData.Select();
        }

        private void ClearCollation(CollationSetting setting)
        {
            if (UseSection)
            {
                cbxUseApportionMenu.Enabled = false;
            }

            cbxReloadCollationData.Checked = setting?.ReloadCollationData == 1;
            cbxAutoMatching.Checked = setting?.AutoMatching == 1;
            cbxAutoSortMatchingEnabledData.Checked = setting?.AutoSortMatchingEnabledData == 1;
            cbxUseFromToNarrowing.Checked = setting?.UseFromToNarrowing == 1;
            cbxSetSystemDateToCreateAtFilter.Checked = setting?.SetSystemDateToCreateAtFilter == 1;
            cbxPrioritizeMatchingIndividuallyMultipleReceipts.Checked = setting?.PrioritizeMatchingIndividuallyMultipleReceipts == 1;
            cbxForceShareTransferFee.Checked = setting?.ForceShareTransferFee == 1;
            cbxPrioritizeMatchingIndividuallyTaxTolerance.Checked = setting?.PrioritizeMatchingIndividuallyTaxTolerance == 1;
            cbxUseApportionMenu.Checked = setting?.UseApportionMenu == 1;
            cbxRequiredCustomer.Checked = setting?.RequiredCustomer == 1;
            cbxAutoAssignCustomer.Checked = setting?.AutoAssignCustomer == 1;
            cbxLearnKanaHistory.Checked = setting?.LearnKanaHistory == 1;
            cbxLearnSpecifiedCustomerKana.Checked = setting?.LearnSpecifiedCustomerKana == 1;
            cbxUseAdvanceReceived.Checked = setting?.UseAdvanceReceived == 1;
            cbxRemoveSpaceFromPayerName.Checked = setting?.RemoveSpaceFromPayerName == 1;
            cbxCalculateTaxByInputId.Checked = setting?.CalculateTaxByInputId == 1;
            if (setting == null)
            {
                rdoDisplayOrder.Checked = true;
                rdoBillingReceiptOrder.Checked = true;
                cmbAdvanceReceivedRecordedDateType.SelectedIndex = cmbAdvanceReceivedRecordedDateType.Items.Count > 0 ? 0 : -1;
                rdoStandard.Checked = true;
                cmbSortOrderColumn.SelectedIndex = cmbSortOrderColumn.Items.Count > 0 ? 0 : -1;
                cmbSortOrder.SelectedIndex = cmbSortOrder.Items.Count > 0 ? 0 : -1;
            }
            else
            {
                rdoDisplayOrder.Checked = setting.MatchingSilentSortedData == 0;
                rdoMatchingOrder.Checked = setting.MatchingSilentSortedData == 1;
                rdoBillingReceiptOrder.Checked = setting.BillingReceiptDisplayOrder == 0;
                rdoReceiptBillingOrder.Checked = setting.BillingReceiptDisplayOrder == 1;
                ListItem advRecvRecordedDateType = cmbAdvanceReceivedRecordedDateType.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.SubItems[1].Value == setting.AdvanceReceivedRecordedDateType);
                cmbAdvanceReceivedRecordedDateType.SelectedIndex = advRecvRecordedDateType != null ? cmbAdvanceReceivedRecordedDateType.Items.IndexOf(advRecvRecordedDateType) : -1;
                rdoStandard.Checked = setting.JournalizingPattern == 0;
                rdoGeneral.Checked = setting.JournalizingPattern == 1;
                ListItem sortOrderColumn = cmbSortOrderColumn.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.SubItems[1].Value == setting.SortOrderColumn);
                cmbSortOrderColumn.SelectedIndex = sortOrderColumn != null ? cmbSortOrderColumn.Items.IndexOf(sortOrderColumn) : -1;
                ListItem sortOrder = cmbSortOrder.Items.Cast<ListItem>().FirstOrDefault(i => (int)i.SubItems[1].Value == setting.SortOrder);
                cmbSortOrder.SelectedIndex = sortOrder != null ? cmbSortOrder.Items.IndexOf(sortOrder) : -1;
            }

            if (!cbxUseApportionMenu.Checked)
            {
                cbxRequiredCustomer.Enabled = false;
                cbxAutoAssignCustomer.Enabled = false;
                cbxLearnKanaHistory.Enabled = false;
            }
        }

        private void SetComboData()
        {
            //前受伝票日付設定方法
            cmbAdvanceReceivedRecordedDateType.Items.Add(new ListItem("0:未入力", 0));
            cmbAdvanceReceivedRecordedDateType.Items.Add(new ListItem("1:システム日付", 1));
            cmbAdvanceReceivedRecordedDateType.Items.Add(new ListItem("2:請求日", 2));
            cmbAdvanceReceivedRecordedDateType.Items.Add(new ListItem("3:売上日", 3));
            cmbAdvanceReceivedRecordedDateType.Items.Add(new ListItem("4:請求締日", 4));
            cmbAdvanceReceivedRecordedDateType.Items.Add(new ListItem("5:入金予定日", 5));
            cmbAdvanceReceivedRecordedDateType.Items.Add(new ListItem("6:入金日", 6));
            cmbAdvanceReceivedRecordedDateType.SelectedIndex = 0;

            //一括消込 入金情報表示順設定
            cmbSortOrderColumn.Items.Add(new ListItem($"{(int)SequencialCollationSortColumn.PayerName}:振込依頼人名", (int)SequencialCollationSortColumn.PayerName));
            cmbSortOrderColumn.Items.Add(new ListItem($"{(int)SequencialCollationSortColumn.ReceiptRecordedAt}:入金日", (int)SequencialCollationSortColumn.ReceiptRecordedAt));
            cmbSortOrderColumn.Items.Add(new ListItem($"{(int)SequencialCollationSortColumn.ReceiptId}:入金ID", (int)SequencialCollationSortColumn.ReceiptId));
            cmbSortOrderColumn.SelectedIndex = 0;
            cmbSortOrder.Items.Add(new ListItem("0:昇順", 0));
            cmbSortOrder.Items.Add(new ListItem("1:降順", 1));
            cmbSortOrder.SelectedIndex = 0;
        }

        private void InitializeLocalDictionary()
        {
            CollationOrderName.Add(0, "専用入金口座照合");
            CollationOrderName.Add(1, "得意先コード照合");
            CollationOrderName.Add(2, "学習履歴照合");
            CollationOrderName.Add(3, "マスターカナ照合");
            CollationOrderName.Add(4, "番号照合");
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SaveCollationSetting;

            BaseContext.SetFunction02Caption("再表示");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearCollationSetting;

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

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitCollationSetting;
        }
        #endregion

        #region Function Key Event

        [OperationLog("登録")]
        private void SaveCollationSetting()
        {
            ClearStatusMessage();
            try
            {
                if (!ValidateInputValues()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var collationOrder = new List<CollationOrder>();
                var billingOrder = new List<MatchingOrder>();
                var receiptOrder = new List<MatchingOrder>();

                var setting = new CollationSetting();
                setting.CompanyId                   = CompanyId;
                setting.ReloadCollationData         = cbxReloadCollationData.Checked ? 1 : 0;
                setting.AutoMatching                = cbxAutoMatching.Checked ? 1 : 0;
                setting.AutoSortMatchingEnabledData = cbxAutoSortMatchingEnabledData.Checked ? 1 : 0;
                setting.UseFromToNarrowing = (cbxUseFromToNarrowing.Checked) ? 1 : 0;
                setting.SetSystemDateToCreateAtFilter = cbxSetSystemDateToCreateAtFilter.Checked ? 1 : 0;
                setting.PrioritizeMatchingIndividuallyMultipleReceipts =
                    cbxPrioritizeMatchingIndividuallyMultipleReceipts.Checked ? 1 : 0;
                setting.ForceShareTransferFee       = cbxForceShareTransferFee.Checked ? 1 : 0;
                setting.PrioritizeMatchingIndividuallyTaxTolerance = cbxPrioritizeMatchingIndividuallyTaxTolerance.Checked ? 1 : 0;
                setting.UseApportionMenu            = cbxUseApportionMenu.Checked ? 1 : 0;
                setting.RequiredCustomer            = cbxRequiredCustomer.Checked ? 1 : 0;
                setting.AutoAssignCustomer          = cbxAutoAssignCustomer.Checked ? 1 : 0;
                setting.LearnKanaHistory            = cbxLearnKanaHistory.Checked ? 1 : 0;
                setting.LearnSpecifiedCustomerKana  = cbxLearnSpecifiedCustomerKana.Checked ? 1 : 0;
                setting.UseAdvanceReceived          = cbxUseAdvanceReceived.Checked ? 1 : 0;
                setting.MatchingSilentSortedData    = rdoDisplayOrder.Checked ? 0 : 1;
                setting.AdvanceReceivedRecordedDateType = Convert.ToInt32(cmbAdvanceReceivedRecordedDateType.SelectedItem.SubItems[1].Value);
                setting.BillingReceiptDisplayOrder  = rdoBillingReceiptOrder.Checked ? 0 : 1;
                setting.RemoveSpaceFromPayerName    = cbxRemoveSpaceFromPayerName.Checked ? 1 : 0;
                setting.JournalizingPattern         = rdoStandard.Checked ? 0 : 1;
                setting.CalculateTaxByInputId       = cbxCalculateTaxByInputId.Checked ? 1 : 0;
                setting.SortOrderColumn             = Convert.ToInt32(cmbSortOrderColumn.SelectedItem.SubItems[1].Value);
                setting.SortOrder                   = Convert.ToInt32(cmbSortOrder.SelectedItem.SubItems[1].Value);

                foreach (var row in grdCollationOrder.Rows)
                {
                    var collation = new CollationOrder();
                    collation.ExecutionOrder = int.Parse(row.Cells[CellName("Order")].Value.ToString());
                    collation.Available = int.Parse(row.Cells[CellName(nameof(CollationOrder.Available))].Value.ToString());
                    var collationTypeName = row.Cells[CellName("CollationTypeName")].Value.ToString();
                    collation.CollationTypeId = CollationOrderName.FirstOrDefault(x => x.Value == collationTypeName).Key;
                    collation.CompanyId = CompanyId;
                    collation.CreateBy = Login.UserId;
                    collation.UpdateBy = Login.UserId;

                    collationOrder.Add(collation);
                }

                foreach (var row in grdMatchingBillingOrder.Rows)
                {
                    var matchingBillingOrder = new MatchingOrder();
                    matchingBillingOrder.ExecutionOrder = int.Parse(row.Cells[CellName("Order")].Value.ToString());
                    matchingBillingOrder.Available = int.Parse(row.Cells[CellName(nameof(MatchingOrder.Available))].Value.ToString());
                    matchingBillingOrder.ItemName = row.Cells[CellName("BillingItemName")].Value.ToString();
                    matchingBillingOrder.TransactionCategory = 1;
                    matchingBillingOrder.SortOrder = int.Parse(row.Cells[CellName(nameof(MatchingOrder.SortOrder))].Value.ToString());
                    matchingBillingOrder.CompanyId = CompanyId;
                    matchingBillingOrder.CreateBy = Login.UserId;
                    matchingBillingOrder.UpdateBy = Login.UserId;

                    billingOrder.Add(matchingBillingOrder);
                }

                foreach (var row in grdMatchingReceiptOrder.Rows)
                {
                    var matchingReceiptOrder = new MatchingOrder();
                    matchingReceiptOrder.ExecutionOrder = int.Parse(row.Cells[CellName("Order")].Value.ToString());
                    matchingReceiptOrder.Available = int.Parse(row.Cells[CellName(nameof(MatchingOrder.Available))].Value.ToString());
                    matchingReceiptOrder.ItemName = row.Cells[CellName("ReceiptItemName")].Value.ToString();
                    matchingReceiptOrder.SortOrder = int.Parse(row.Cells[CellName(nameof(MatchingOrder.SortOrder))].Value.ToString());
                    matchingReceiptOrder.TransactionCategory = 2;
                    matchingReceiptOrder.CompanyId = CompanyId;
                    matchingReceiptOrder.CreateBy = Login.UserId;
                    matchingReceiptOrder.UpdateBy = Login.UserId;

                    receiptOrder.Add(matchingReceiptOrder);
                }

                var task = ServiceProxyFactory.DoAsync(async (CollationSettingMasterClient client)
                    => await client.SaveAsync(SessionKey,
                    setting, collationOrder.ToArray(),
                    billingOrder.ToArray(),
                    receiptOrder.ToArray()));

                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var result = task.Result;
                if (result != null && result.ProcessResult.Result)
                {
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }

                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("再表示")]
        private void ClearCollationSetting()
        {
            ClearStatusMessage();
            try
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

                SetInitialCollationSetting();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void ExitCollationSetting()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            ParentForm.Close();
        }

        #endregion

        #region Display Grid Data
        private void CreateCollationOrderGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  50, "Order"                         , caption: "順序"            , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight)),
                new CellSetting(height,  50, nameof(CollationOrder.Available), caption: "使用"            , cell: builder.GetCheckBoxCell(), readOnly: false),
                new CellSetting(height, 115, "CollationTypeName"             , caption: "照合ロジック名称"),
            });

            grdCollationOrder.Template = builder.Build();
        }

        private void DisplayCollationOrderGrid(List<CollationOrder> collationOrder)
        {
            if (!collationOrder.Any())
            {
                collationOrder = GetInitialCollationOrder();
            }

            int collationOrderCount = collationOrder.Count;
            grdCollationOrder.RowCount = collationOrderCount;

            for (var i = 0; i < collationOrderCount; i++)
            {
                grdCollationOrder.Rows[i][CellName("Order")].Value = collationOrder[i].ExecutionOrder;
                grdCollationOrder.Rows[i][CellName(nameof(MatchingOrder.Available))].Value = collationOrder[i].Available;
                grdCollationOrder.Rows[i][CellName("CollationTypeName")].Value = CollationOrderName[collationOrder[i].CollationTypeId];
            }
        }

        private List<CollationOrder> GetInitialCollationOrder()
        {
            var collationOrder = new List<CollationOrder>();

            collationOrder.AddRange(new CollationOrder[]
            {
                new CollationOrder()
                {
                    ExecutionOrder = 1, CompanyId = CompanyId,
                    CollationTypeId = 0, Available = 1
                },
                new CollationOrder()
                {
                    ExecutionOrder = 2, CompanyId = CompanyId,
                    CollationTypeId = 1, Available = 1
                },
                new CollationOrder()
                {
                    ExecutionOrder = 3, CompanyId = CompanyId,
                    CollationTypeId = 2, Available = 1
                },
                new CollationOrder()
                {
                    ExecutionOrder = 4, CompanyId = CompanyId,
                    CollationTypeId = 3, Available = 1
                }
            });
            return collationOrder;
        }

        private void CreateBillingOrderGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var combo = GetComboBoxCell(builder);
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  45, "Order"                        , caption: "順序"  , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight)),
                new CellSetting(height,  45, nameof(MatchingOrder.Available), caption: "使用"  , cell: builder.GetCheckBoxCell(), readOnly: false),
                new CellSetting(height, 250, "BillingItemNameJp"              , caption: "項目名"),
                new CellSetting(height,  50, nameof(MatchingOrder.SortOrder), caption: "ソート", cell: combo, readOnly: false),
                new CellSetting(height,   0, "BillingItemName", visible: false),
            });
            grdMatchingBillingOrder.Template = builder.Build();
        }

        private void DisplayBillingOrderGrid(List<MatchingOrder> matchingBillingOrder)
        {

            if (!matchingBillingOrder.Any())
            {
                matchingBillingOrder = GetInitialMatchingBillingOrder();
            }

            int billingOrderCount = matchingBillingOrder.Count;
            grdMatchingBillingOrder.RowCount = billingOrderCount;

            for (var i = 0; i < billingOrderCount; i++)
            {
                grdMatchingBillingOrder.Rows[i][CellName("Order")].Value = matchingBillingOrder[i].ExecutionOrder;
                grdMatchingBillingOrder.Rows[i][CellName(nameof(MatchingOrder.Available))].Value = matchingBillingOrder[i].Available;
                grdMatchingBillingOrder.Rows[i][CellName("BillingItemNameJp")].Value = matchingBillingOrder[i].ItemNameJp;
                grdMatchingBillingOrder.Rows[i][CellName(nameof(MatchingOrder.SortOrder))].Value = matchingBillingOrder[i].SortOrder;
                grdMatchingBillingOrder.Rows[i][CellName("BillingItemName")].Value = matchingBillingOrder[i].ItemName;
            }
        }

        private void CreateReceiptOrderGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var combo = GetComboBoxCell(builder);
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  45, "Order"                        , caption: "順序"  , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight)),
                new CellSetting(height,  45, nameof(MatchingOrder.Available), caption: "使用"  , cell: builder.GetCheckBoxCell(), readOnly: false),
                new CellSetting(height, 250, "ReceiptItemNameJp"              , caption: "項目名"),
                new CellSetting(height,  50, nameof(MatchingOrder.SortOrder), caption: "ソート", cell: combo, readOnly: false),
                new CellSetting(height,   0, "ReceiptItemName", visible: false),
            });
            grdMatchingReceiptOrder.Template = builder.Build();
        }

        private void DisplayReceiptOrderGrid(List<MatchingOrder> matchingReceiptOrder)
        {
            if (!matchingReceiptOrder.Any())
            {
                matchingReceiptOrder = GetInitialMatchingReceiptOrder();
            }

            int receiptOrderCount = matchingReceiptOrder.Count;
            grdMatchingReceiptOrder.RowCount = receiptOrderCount;

            for (var i = 0; i < receiptOrderCount; i++)
            {
                grdMatchingReceiptOrder.Rows[i][CellName("Order")].Value = matchingReceiptOrder[i].ExecutionOrder;
                grdMatchingReceiptOrder.Rows[i][CellName(nameof(MatchingOrder.Available))].Value = matchingReceiptOrder[i].Available;
                grdMatchingReceiptOrder.Rows[i][CellName("ReceiptItemNameJp")].Value = matchingReceiptOrder[i].ItemNameJp;
                grdMatchingReceiptOrder.Rows[i][CellName(nameof(MatchingOrder.SortOrder))].Value = matchingReceiptOrder[i].SortOrder;
                grdMatchingReceiptOrder.Rows[i][CellName("ReceiptItemName")].Value = matchingReceiptOrder[i].ItemName;
            }
        }

        private List<MatchingOrder> GetInitialMatchingReceiptOrder()
        {
            var matchingReceiptOrder = new List<MatchingOrder>();
            var executionOrder = 0;

            if (UseScheduledPayment)
            {
                matchingReceiptOrder.Add(new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder,
                    CompanyId = CompanyId,
                    ItemName = "NettingFlag",
                    SortOrder = 1,
                    Available = 1
                });
            }

            matchingReceiptOrder.AddRange(new MatchingOrder[]
            {
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "ReceiptRemainSign", SortOrder = 0, Available = 1
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "RecordedAt", SortOrder = 0, Available = 1
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "PayerName", SortOrder = 0, Available = 1
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "SourceBankName", SortOrder = 0, Available = 1
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "SourceBranchName", SortOrder = 0, Available = 1
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "ReceiptRemainAmount", SortOrder = 1, Available = 0
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "ReceiptCategory", SortOrder = 0, Available = 0
                }
            });

            return matchingReceiptOrder;
        }

        private List<MatchingOrder> GetInitialMatchingBillingOrder()
        {
            var matchingBillingOrder = new List<MatchingOrder>();
            var executionOrder = 1;
            matchingBillingOrder.Add(new MatchingOrder()
            {
                ExecutionOrder = executionOrder, CompanyId = CompanyId,
                ItemName = "BillingRemainSign", SortOrder = 0, Available = 1
            });

            if (UseCashOnDueDates)
            {
                matchingBillingOrder.Add(new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "CashOnDueDatesFlag", SortOrder = 1, Available = 1
                });
            }

            matchingBillingOrder.AddRange(new MatchingOrder[]
            {
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "DueAt", SortOrder = 0, Available = 1
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "CustomerCode", SortOrder = 0, Available = 1
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "BilledAt", SortOrder = 0, Available = 1
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "BillingRemainAmount", SortOrder = 1, Available = 0
                },
                new MatchingOrder()
                {
                    ExecutionOrder = ++executionOrder, CompanyId = CompanyId,
                    ItemName = "BillingCategory", SortOrder = 0, Available = 0
                }
            });

            return matchingBillingOrder;
        }

        #endregion

        #region ドラッグ＆ドロップ

        private void GridMouseDown(object sender, MouseEventArgs e)
        {
            var grdCommon = sender as GcMultiRow;

            if (grdCommon.CurrentCellPosition.CellIndex != 2) return;

            var hitTestInfo = grdCommon.HitTest(e.X, e.Y);

            if (hitTestInfo.Type == HitTestType.Row)
            {
                grdCommon.DoDragDrop(grdCommon.Rows[hitTestInfo.SectionIndex], DragDropEffects.Move);
            }
        }

        private void GridDragDrop(object sender, DragEventArgs e)
        {
            var grdCommon = sender as GcMultiRow;
            var clientPoint = grdCommon.PointToClient(new System.Drawing.Point(e.X, e.Y));
            var hitTestInfo = grdCommon.HitTest(clientPoint.X, clientPoint.Y);
            var sourceRow = (Row)e.Data.GetData(typeof(Row));

            if (sourceRow.GcMultiRow.Name != grdCommon.Name)
            {
                DragDropDestRowIndex = -1; return;
            }
            var cellValues = new object[sourceRow.Cells.Count];

            for (var j = 0; j < sourceRow.Cells.Count; j++)
            {
                cellValues[j] = sourceRow.Cells[j].Value;
            }

            if (hitTestInfo.SectionIndex != -1)
            {
                if (sourceRow.Index < hitTestInfo.SectionIndex)
                {
                    grdCommon.Rows.Insert(hitTestInfo.SectionIndex + 1, cellValues);
                }
                else
                {
                    grdCommon.Rows.Insert(hitTestInfo.SectionIndex, cellValues);
                }
                if (DragDropDestRowIndex == -1)
                {
                    DragDropDestRowIndex = 0;
                }
                grdCommon.Rows[DragDropDestRowIndex].Selected = true;
            }
            else
            {
                grdCommon.Rows.Add(cellValues);
                grdCommon.Rows[grdCommon.RowCount - 1].Selected = true;
            }
            sourceRow.GcMultiRow.Rows.Remove(sourceRow);

            //順序
            for (var i = 0; i<grdCommon.Rows.Count; i++)
            {
                grdCommon.Rows[i][CellName("Order")].Value = i + 1;
            }

            grdCommon.Invalidate();

            DragDropDestRowIndex = -1;
        }

        private void GridDragLeave(object sender, EventArgs e)
        {
            var grdCommon = sender as GcMultiRow;
            grdCommon.Invalidate();
            DragDropDestRowIndex = -1;
        }

        private void GridDragOver(object sender, DragEventArgs e)
        {
            Modified = true;

            e.Effect = DragDropEffects.Move;
            var grdCommon = sender as GcMultiRow;
            var clientPoint = grdCommon.PointToClient(new Point(e.X, e.Y));
            var hitTestInfo = grdCommon.HitTest(clientPoint.X, clientPoint.Y);
            var newRowIndex = 0;
            var sourceRow = (Row)e.Data.GetData(typeof(Row));
            if (hitTestInfo.Type == HitTestType.Row)
            {
                if (sourceRow.Index < hitTestInfo.SectionIndex)
                {
                    DragDropDestRowIndex = hitTestInfo.SectionIndex + 1;
                }
                else
                {
                    DragDropDestRowIndex = hitTestInfo.SectionIndex;
                }
            }
            else if (hitTestInfo.Type == HitTestType.None)
            {
                DragDropDestRowIndex = grdCommon.RowCount;
            }
            else if (hitTestInfo.Type == HitTestType.HorizontalScrollBar)
            {
                var topPos = grdCommon.FirstDisplayedCellPosition;
                if (topPos.RowIndex < grdCommon.RowCount)
                {
                    newRowIndex = topPos.RowIndex + 1;
                }
                else
                {
                    newRowIndex = topPos.RowIndex;
                }
                grdCommon.FirstDisplayedCellPosition = new CellPosition(newRowIndex, topPos.CellIndex);
            }
            else if (hitTestInfo.Type == HitTestType.ColumnHeader)
            {
                var topPos = grdCommon.FirstDisplayedCellPosition;

                if (topPos.RowIndex != 0)
                {
                    newRowIndex = topPos.RowIndex < 0 ? 0 : topPos.RowIndex - 1;
                    grdCommon.FirstDisplayedCellPosition = new CellPosition(newRowIndex, topPos.CellIndex);
                }
            }
            grdCommon.Invalidate();
        }

        private void GridSectionPainting(object sender, SectionPaintingEventArgs e)
        {
            var grdCommon = sender as GcMultiRow;
            using (Pen redPen = new Pen(Color.Red, 3))
            {
                if (e.Scope == CellScope.Row)
                {
                    if (e.SectionIndex == DragDropDestRowIndex)
                    {
                        e.PaintSectionBackground(e.SectionBounds);
                        e.PaintSectionBorder(e.SectionBounds);
                        e.PaintCells(e.SectionBounds);

                        if (grdCommon.CurrentCellPosition.CellIndex == 2)
                        {
                            e.Graphics.DrawLine(redPen, e.SectionBounds.Left, e.SectionBounds.Top,
                                e.SectionBounds.Right, e.SectionBounds.Top);
                        }
                        e.Handled = true;
                    }
                    else if (DragDropDestRowIndex == grdCommon.RowCount && grdCommon.CurrentCellPosition.CellIndex == 2)
                    {
                        e.Graphics.DrawLine(redPen, e.SectionBounds.Left, e.SectionBounds.Bottom,
                                e.SectionBounds.Right, e.SectionBounds.Bottom);
                    }
                }
            }
        }
        #endregion

        #region Web Service
        private async Task<CollationSetting> GetCollationSettingAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (CollationSettingMasterClient client)
                => await client.GetAsync(SessionKey, CompanyId));
            if (result.ProcessResult.Result)
                return result.CollationSetting;
            return null;
        }

        private async Task<List<CollationOrder>> GetCollationOrdersAsync()
        {
            var result =  await ServiceProxyFactory.DoAsync(async (CollationSettingMasterClient client)
                =>  await client.GetCollationOrderAsync(SessionKey, CompanyId));
            if (result.ProcessResult.Result)
                return result.CollationOrders;
            return new List<CollationOrder>();
        }

        private async Task<List<MatchingOrder>> GetBillingOrdersAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (CollationSettingMasterClient client)
                => await client.GetMatchingBillingOrderAsync(SessionKey, CompanyId));
            if (result.ProcessResult.Result)
                return result.MatchingOrders;
            return new List<MatchingOrder>();
        }

        private async Task<List<MatchingOrder>> GetReceiptOrdersAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (CollationSettingMasterClient client)
                => await client.GetMatchingReceiptOrderAsync(SessionKey, CompanyId));
            if (result.ProcessResult.Result)
                return result.MatchingOrders;
            return new List<MatchingOrder>();
        }
        #endregion

        #region event handler
        private void OnChanged(object sender, EventArgs e)
        {
            Modified = true;

            if (sender != cbxUseApportionMenu) return;

            var use = cbxUseApportionMenu.Checked;

            cbxRequiredCustomer.Enabled = use;
            cbxAutoAssignCustomer.Enabled = use;
            cbxLearnKanaHistory.Enabled = use;

            if (!use)
            {
                cbxRequiredCustomer.Checked = use;
                cbxAutoAssignCustomer.Checked = use;
                cbxLearnKanaHistory.Checked = use;
            }
        }

        private ComboBoxCell GetComboBoxCell(GcMultiRowTemplateBuilder builder )
        {
            var comboBoxCell = builder.GetComboBoxCell();
            comboBoxCell.ValueAsIndex = true;
            comboBoxCell.Items.Add("昇順");
            comboBoxCell.Items.Add("降順");
            return comboBoxCell;
        }

        private bool ValidateInputValues()
        {
            if (!grdCollationOrder.Rows.Any(x => Convert.ToBoolean(x[CellName(nameof(CollationOrder.Available))].EditedFormattedValue)))
            {
                ShowWarningDialog(MsgWngCheckAtleastOneCollationLogic);
                return false;
            }
            return true;
        }

        #endregion
    }
}