using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.CompanyMasterService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ReceiptExcludeService;
using Rac.VOne.Client.Screen.ApplicationControlMasterService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Message;
using System.Diagnostics;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>対象外入金データ入力</summary>
    public partial class PD0502 : VOneScreenBase
    {
        public Func<IEnumerable<ITransactionData>, bool> SavePostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> ExcludePostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> DeletePostProcessor { get; set; }

        private bool IsPostProessorImplemented
        {
            get
            {
                return SavePostProcessor != null
                    && ExcludePostProcessor != null
                    && DeletePostProcessor != null;
            }
        }

        public Receipt CurrentReceipt { get; set; }
        public int NoOfpre { get; set; }
        private List<Category> CategoryList { get; set; } = null;
        IEnumerable<ReceiptExclude> SearchResult;
        private bool IsSaved { get; set; } = false;
        private string FieldString { get; set; } = "#,###,###,###,##0";
        private string CellName(string value) => $"cel{value}";

        private const int MaxRowsCount = 10;

        public PD0502()
        {
            InitializeComponent();
            grdReceiptExcInput.SetupShortcutKeys();
            grdReceiptExcInput.GridColorType = GridColorType.Input;
            Text = "対象外入金データ入力";
            grdReceiptExcInput.DataBindingComplete += grid_DataBindingComplete;
        }

        private void PD0502_Load(object sender, EventArgs e)
        {
            SetScreenName();
            try
            {
                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SetCurrencyDisplayString(NoOfpre);

                txtReceiptId.Text = CurrentReceipt.Id.ToString();
                txtCurrencyCode.Text = CurrentReceipt.CurrencyCode.ToString();

                txtReceiptId.Enabled = false;
                txtCurrencyCode.Enabled = false;

                lblCurrencyCode.Visible = UseForeignCurrency;
                txtCurrencyCode.Visible = UseForeignCurrency;

                InitializeGrid();

                GetReceiptSearch();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetCurrencyDisplayString(int displayScale)
        {
            var displayFieldString = "#,###,###,###,##0";
            var displayFormatString = "0";

            if (displayScale > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < displayScale; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }

            FieldString = displayFieldString;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        [OperationLog("戻る")]
        private void Exit()
        {
            if (Modified
                && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            ClearStatusMessage();

            if (IsSaved)
                ParentForm.DialogResult = DialogResult.OK;

            BaseForm.Close();

        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            Func<Cell> getNumberCell = () => UseForeignCurrency
                ? builder.GetNumberCellCurrency(NoOfpre, NoOfpre, 0)
                : builder.GetNumberCell();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 150, nameof(ReceiptExclude.ExcludeAmount)     , caption: "対象外金額", readOnly: false, dataField: nameof(ReceiptExclude.ExcludeAmount), cell: getNumberCell()),
                new CellSetting(height, 250, nameof(ReceiptExclude.ExcludeCategoryId) , caption: "対象外区分", readOnly: false, dataField: nameof(ReceiptExclude.ExcludeCategoryId), cell: CreateDatabindingComboBoxCell(builder.GetComboBoxCell())),
                new CellSetting(height, 165, nameof(ReceiptExclude.OutputAt)          , caption: "仕訳日"    , dataField: nameof(ReceiptExclude.OutputAt), cell:  builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height,  70, "Clear"                                  , caption: "クリア"    , value: "クリア", cell:  builder.GetButtonCell()),
            });

            grdReceiptExcInput.Template = builder.Build();
            grdReceiptExcInput.HideSelection = true;
            grdReceiptExcInput.AllowUserToResize = false;
        }

        private ComboBoxCell CreateDatabindingComboBoxCell(ComboBoxCell comboBoxCell)
        {
            CategoriesResult result = null;
            CategorySearch categorySearch = new CategorySearch();
            categorySearch.CompanyId = CompanyId;
            categorySearch.CategoryType = 4;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();

                result = await service.GetItemsAsync(SessionKey, categorySearch);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                CategoryList = result.Categories;

                comboBoxCell.DataSource = new BindingSource(CategoryList, null);
                comboBoxCell.DisplayMember = nameof(Category.Name);
                comboBoxCell.ValueMember = nameof(Category.Id);
            }

            return comboBoxCell;
        }

        private void GetReceiptSearch()
        {
            ReceiptExcludesResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptExcludeServiceClient>();

                result = await service.GetByReceiptIdAsync(SessionKey, CurrentReceipt.Id);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                SearchResult = result.ReceiptExcludes;
                SearchDataBind(SearchResult);
            }
        }

        private void SearchDataBind(IEnumerable<ReceiptExclude> ReceiptExcludes)
        {
            var dataCount = ReceiptExcludes.Count();
            var excludeAmountTotal = 0M;
            var existOutputAt = false;

            var table = new DataTable();
            table.Columns.Add(nameof(ReceiptExclude.ExcludeAmount), typeof(decimal));
            table.Columns.Add(nameof(ReceiptExclude.ExcludeCategoryId), typeof(int));
            table.Columns.Add(nameof(ReceiptExclude.OutputAt), typeof(DateTime));
            table.Columns.Add(nameof(ReceiptExclude.CreateBy), typeof(int));
            table.Columns.Add(nameof(ReceiptExclude.CreateAt), typeof(DateTime));

            foreach (var item in ReceiptExcludes)
            {
                var dr = table.NewRow();

                dr[nameof(ReceiptExclude.ExcludeAmount)] = item.ExcludeAmount;
                dr[nameof(ReceiptExclude.ExcludeCategoryId)] = item.ExcludeCategoryId;
                dr[nameof(ReceiptExclude.OutputAt)] = (object)item.OutputAt ?? DBNull.Value;
                dr[nameof(ReceiptExclude.CreateBy)] = item.CreateBy;
                dr[nameof(ReceiptExclude.CreateAt)] = item.CreateAt;

                if (item.OutputAt.HasValue)
                {
                    existOutputAt = true;
                }

                table.Rows.Add(dr);

                excludeAmountTotal += item.ExcludeAmount;
            }

            for (int i = dataCount; i < MaxRowsCount; i++)
            {
                var row = table.NewRow();

                table.Rows.Add(row);
            }

            grdReceiptExcInput.DataSource = table;

            var remainAmount = CurrentReceipt.RemainAmount;
            lblReceiptAmountTotal.Text = remainAmount.ToString(FieldString);
            lblExcludeAmountTotal.Text = excludeAmountTotal.ToString(FieldString);
            lblDifferenceAmountTotal.Text = remainAmount.ToString(FieldString);

            var deletable = dataCount > 0 && !existOutputAt;
            BaseContext.SetFunction03Enabled(deletable);
        }

        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!ValidateChildren()) return;

                if (!ValidateInputValues())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                ClearStatusMessage();

                var result = SaveReceiptExclude();
                if (!result) return;

                Modified = false;

                CurrentReceipt.RemainAmount = Convert.ToDecimal(lblDifferenceAmountTotal.Text);
                CurrentReceipt.ExcludeAmount = Convert.ToDecimal(lblExcludeAmountTotal.Text);

                GetReceiptSearch();

                DispStatusMessage(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                if (!ValidateChildren()) return;

                ClearStatusMessage();

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var result = DeleteExclude();
                if (!result) return;

                Modified = false;

                CurrentReceipt.RemainAmount = Convert.ToDecimal(lblDifferenceAmountTotal.Text) + Convert.ToDecimal(lblExcludeAmountTotal.Text);
                CurrentReceipt.ExcludeAmount = 0;

                GetReceiptSearch();
                DispStatusMessage(MsgInfDeleteSuccess);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateInputValues()
        {
            grdReceiptExcInput.EndEdit();
            var empty = true;
            foreach (var row in grdReceiptExcInput.Rows)
            {
                var dataRow = (row.DataBoundItem as DataRowView)?.Row;
                Debug.Assert(dataRow != null);
                if (dataRow.Field<DateTime?>(nameof(ReceiptExclude.OutputAt)).HasValue)
                {
                    empty = false;
                    continue;
                }
                var amount = dataRow.Field<decimal?>(nameof(ReceiptExclude.ExcludeAmount));
                var categoryId = dataRow.Field<int?>(nameof(ReceiptExclude.ExcludeCategoryId));
                if (!amount.HasValue && !categoryId.HasValue) continue;
                if (!amount.HasValue)
                {
                    grdReceiptExcInput.ClearSelection();
                    row[CellName(nameof(ReceiptExclude.ExcludeAmount))].Selected = true;
                    ShowWarningDialog(MsgWngInputRequired, "対象外金額");
                    return false;
                }
                if (!categoryId.HasValue)
                {
                    grdReceiptExcInput.ClearSelection();
                    row[CellName(nameof(ReceiptExclude.ExcludeCategoryId))].Selected = true;
                    ShowWarningDialog(MsgWngSelectionRequired, "対象外区分");
                    return false;
                }
                empty = false;
            }
            if (empty)
            {
                ShowWarningDialog(MsgWngInputGridRequired);
                return false;
            }
            return true;
        }

        private bool SaveReceiptExclude()
        {
            var table = grdReceiptExcInput.DataSource as DataTable;
            var receiptId = CurrentReceipt.Id;
            var updateItems = table.Rows.Cast<DataRow>()
                .Where(x => x.Field<decimal?>(nameof(ReceiptExclude.ExcludeAmount)).HasValue
                    && x.Field<int?>(nameof(ReceiptExclude.ExcludeCategoryId)).HasValue)
                .Select(x => new ReceiptExclude {
                    ReceiptId           = receiptId,
                    ExcludeFlag         = 1,
                    ExcludeAmount       = x.Field<decimal>(nameof(ReceiptExclude.ExcludeAmount)),
                    ExcludeCategoryId   = x.Field<int>(nameof(ReceiptExclude.ExcludeCategoryId)),
                    OutputAt            = x.Field<DateTime?>(nameof(ReceiptExclude.OutputAt)),
                    CreateBy            = x.Field<int?>(nameof(ReceiptExclude.CreateBy)) ?? Login.UserId,
                    UpdateBy            = Login.UserId,
                    ReceiptUpdateAt     = CurrentReceipt.UpdateAt,
                });

            if (!updateItems.Any())
            {
                updateItems = new[] {
                    new ReceiptExclude {
                        ReceiptId           = receiptId,
                        ExcludeFlag         = 0,
                        ExcludeAmount       = 0M,
                        ExcludeCategoryId   = null,
                        OutputAt            = null,
                        CreateBy            = Login.UserId,
                        UpdateBy            = Login.UserId,
                    }
                };
            }

            return UpdateExcludeAmount(updateItems,
                () => ShowWarningDialog(MsgErrSaveError));
        }

        private bool DeleteExclude()
        {
            var updateItems = new List<ReceiptExclude> {
                new ReceiptExclude {
                ReceiptId           = CurrentReceipt.Id,
                ExcludeFlag         = 0,
                ExcludeAmount       = 0M,
                ExcludeCategoryId   = null,
                CreateBy            = Login.UserId,
                UpdateBy            = Login.UserId,
                ReceiptUpdateAt     = CurrentReceipt.UpdateAt,
            }};

            return UpdateExcludeAmount(updateItems,
                () => ShowWarningDialog(MsgErrDeleteError));
        }

        private bool UpdateExcludeAmount(IEnumerable<ReceiptExclude> updateItems, System.Action errorMessaging)
        {
            var deleteItems = new List<ReceiptExclude>();
            var newItems = new List<ReceiptExclude>();
            var receiptItems = new List<Receipt>();
            var receiptId = CurrentReceipt.Id;

            var success = true;
            var syncResult = true;
            var updateValid = true;
            ProgressDialog.Start(ParentForm, async (cancel, progress) =>
            {
                if (IsPostProessorImplemented)
                {
                    await ServiceProxyFactory.DoAsync(async (ReceiptExcludeServiceClient client) =>
                    {
                        var result = await client.GetByReceiptIdAsync(SessionKey, receiptId);
                        if (result.ProcessResult.Result
                            && result.ReceiptExcludes != null
                            && result.ReceiptExcludes.Any(x => x != null))
                        {
                            deleteItems.AddRange(result.ReceiptExcludes);
                        }
                    });
                }
                await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
                {
                    var saveResult = await client.SaveExcludeAmountAsync(SessionKey, updateItems.ToArray());
                    success = saveResult?.ProcessResult.Result ?? false;
                    newItems.AddRange(saveResult.ReceiptExclude.Where(x => x.Id != 0L));

                    if (!success)
                    {
                        updateValid = !(saveResult.ProcessResult.ErrorCode == Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated);
                    }

                    if (success)
                    {
                        var receiptResult = await client.GetAsync(SessionKey, updateItems.Select(x => x.ReceiptId).Distinct().ToArray());
                        var updatedReceipt = receiptResult.Receipts.First();
                        CurrentReceipt = updatedReceipt;
                        if (IsPostProessorImplemented)
                            receiptItems.AddRange(receiptResult.Receipts);
                    }
                });
                if (!success) return;
                if (IsPostProessorImplemented)
                {
                    syncResult = DeletePostProcessor.Invoke(deleteItems.Select(x => x as ITransactionData));
                    if (!syncResult)
                        return;
                    syncResult = ExcludePostProcessor.Invoke(newItems.Select(x => x as ITransactionData));
                    if (!syncResult)
                        return;
                    syncResult = SavePostProcessor.Invoke(receiptItems.Select(x => x as ITransactionData));
                }
                IsSaved = true;
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
                errorMessaging?.Invoke();
                return false;
            }
            return success;
        }

        #region grid event handler
        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            ClearStatusMessage();

            if (e.CellIndex == 0 || e.CellIndex == 1)
                Modified = true;
        }

        private void grid_CellValidated(object sender, CellEventArgs e)
        {
            if (e.CellName != CellName(nameof(ReceiptExclude.ExcludeAmount)))
                return;

            var rowExAmt = 0M;
            var errFlag = false;
            var errFlag1 = false;

            var remainAmount = CurrentReceipt.RemainAmount + CurrentReceipt.ExcludeAmount;
            decimal? cellExAmt = null;

            if (string.IsNullOrEmpty(grdReceiptExcInput.Rows[e.RowIndex].Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].DisplayText))
                cellExAmt = null;
            else
                cellExAmt = Convert.ToDecimal(grdReceiptExcInput.Rows[e.RowIndex].Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].EditedFormattedValue);

            if (cellExAmt == 0)
            {
                ShowWarningDialog(MsgWngInputRequired, "対象外金額");
                errFlag = true;
            }
            else if (remainAmount > 0 && cellExAmt < 0)
            {
                ShowWarningDialog(MsgWngPlusSignPair);
                errFlag = true;
            }
            else if (remainAmount <= 0 && cellExAmt > 0)
            {
                ShowWarningDialog(MsgWngMinusSignPair);
                errFlag = true;
            }

            for (int i = 0; i < grdReceiptExcInput.RowCount; i++)
            {
                rowExAmt += Convert.ToDecimal(grdReceiptExcInput.Rows[i].Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].EditedFormattedValue);
            }

            lblExcludeAmountTotal.Text = (rowExAmt).ToString(FieldString);
            lblDifferenceAmountTotal.Text = (CurrentReceipt.RemainAmount + CurrentReceipt.ExcludeAmount - Convert.ToDecimal(lblExcludeAmountTotal.Text)).ToString(FieldString);

            if (remainAmount > 0 && remainAmount < rowExAmt)
            {
                ShowWarningDialog(MsgWngExcludeAmtOverReceipt);
                errFlag1 = true;
            }
            else if (remainAmount <= 0 && remainAmount > rowExAmt)
            {
                ShowWarningDialog(MsgWngExcludeAmtOverReceipt);
                errFlag1 = true;
            }

            if (errFlag1 || errFlag)
            {
                lblExcludeAmountTotal.Text = (rowExAmt - Convert.ToDecimal(grdReceiptExcInput.Rows[e.RowIndex].Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].EditedFormattedValue)).ToString(FieldString);
                lblDifferenceAmountTotal.Text = (CurrentReceipt.RemainAmount + CurrentReceipt.ExcludeAmount - Convert.ToDecimal(lblExcludeAmountTotal.Text)).ToString(FieldString);

                grdReceiptExcInput.Rows[e.RowIndex].Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].Value = DBNull.Value;
                grdReceiptExcInput.CurrentCell = grdReceiptExcInput.CurrentRow.Cells[0];
                grdReceiptExcInput.BeginEdit(true);
            }
        }

        private void grid_CellContentButtonClick(object sender, CellEventArgs e)
        {
            if (e.CellName == CellName("Clear"))
            {
                if (grdReceiptExcInput.CurrentRow.Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].Enabled != false)
                {
                    if (!string.IsNullOrWhiteSpace(grdReceiptExcInput.CurrentRow.Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].DisplayText))
                    {
                        decimal excAmt = Convert.ToDecimal(grdReceiptExcInput.CurrentRow.Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].Value);
                        lblExcludeAmountTotal.Text = (Convert.ToDecimal(lblExcludeAmountTotal.Text) - excAmt).ToString(FieldString);
                        lblDifferenceAmountTotal.Text = (CurrentReceipt.RemainAmount + CurrentReceipt.ExcludeAmount - Convert.ToDecimal(lblExcludeAmountTotal.Text)).ToString(FieldString);
                    }

                    grdReceiptExcInput.CurrentRow.Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))].Value = DBNull.Value;
                    grdReceiptExcInput.CurrentRow.Cells[CellName(nameof(ReceiptExclude.ExcludeCategoryId))].Value = DBNull.Value;
                    grdReceiptExcInput.CurrentRow.Cells[CellName(nameof(Receipt.OutputAt))].Value = DBNull.Value;

                    grdReceiptExcInput.CurrentCell = grdReceiptExcInput.CurrentRow.Cells[CellName(nameof(ReceiptExclude.ExcludeAmount))];
                    Modified = true;
                }
            }
        }

        private void grid_DataBindingComplete(object sendeer, MultiRowBindingCompleteEventArgs e)
        {
            foreach (var row in grdReceiptExcInput.Rows.Where(x => !string.IsNullOrEmpty(Convert.ToString(x[CellName(nameof(Receipt.OutputAt))].Value))))
                foreach (var cell in row.Cells)
                {
                    cell.ReadOnly = true;
                    cell.Enabled = false;
                }
        }
        #endregion
    }
}
