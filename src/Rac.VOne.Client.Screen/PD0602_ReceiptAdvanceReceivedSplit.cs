using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.AdvanceReceivedBackupService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// 前受分割処理
    /// </summary>
    public partial class PD0602 : VOneScreenBase
    {
        /// <summary>
        /// フォーム起動パラメータ。
        /// 前画面(前受一括計上処理)で選択された前受金データ。
        /// <para/>
        /// 画面データ読み込み(LoadData)後、読み込んだReceiptデータ(1行目のデータ)で置き換える。
        /// </summary>
        public Receipt SelectedAdvanceReceipt { get; set; }

        /// <summary>
        /// フォーム実行結果。
        /// 一度でも登録／振替取消を実行(完了)した場合はtrue、そうでない場合はfalse。
        /// 遷移元画面で再検索を行うかどうかの判定に用いる。
        /// </summary>
        public bool IsDataRegistered { get; private set; }


        /// <summary>
        /// 前受金データの振替元となる入金データ
        /// </summary>
        /// <remarks>
        /// SelectedAdvanceReceipt.OriginalReceiptId で Receipt を検索したもの
        /// </remarks>
        private Receipt OriginalReceipt { get; set; }

        /// <summary>
        /// OriginalReceiptに紐付く前受金データ
        /// </summary>
        private List<Receipt> AdvanceReceipts { get; set; }

        /// <summary>
        /// 前受金バックアップデータ
        /// </summary>
        private AdvanceReceivedBackup AdvanceReceiptBackup { get; set; }

        /// <summary>
        /// OriginalReceipt.CurrencyId に対応する通貨マスタ
        /// </summary>
        private Currency OriginalReceiptCurrency { get; set; }

        /// <summary>
        /// 項目名称マスタ
        /// </summary>
        private List<ColumnNameSetting> ColumnNameSettings { get; set; }

        /// <summary>
        /// 区分マスタ(入金区分のみ)
        /// </summary>
        private List<Category> ReceiptCategories { get; set; }

        /// <summary>
        /// 書式設定マスタ
        /// </summary>
        private DataExpression DataExpressionSettings { get; set; }

        /// <summary>
        /// 金額入力フォーマット
        /// </summary>
        private string AmountInputFormat { get; set; }

        /// <summary>
        /// 金額表示フォーマット
        /// </summary>
        private string AmountDisplayFormat { get; set; }

        /// <summary>
        /// 金額入力制限値
        /// </summary>
        /// <remarks>
        /// 99,999,999,999 / 99,999,999,999.9 / 99,999,999,999.99 など Currency.Precision 設定値に従ってセットされる。
        /// 負数を入力する場合でも、この変数は絶対値を保持する。
        /// </remarks>
        private decimal AmountLimit { get; set; }

        /// <summary>
        /// 前受金状態
        /// </summary>
        /// <remarks>
        /// よい名前を思いつかないので画面設計書の状態番号で列挙する
        /// </remarks>
        private enum AdvanceReceiptState
        {
            /// <summary>
            /// 画面設計書 データ遷移図②③<para/>
            /// 計上仕訳未出力の場合
            /// </summary>
            State23,

            /// <summary>
            /// 画面設計書 データ遷移図④<para/>
            /// 計上仕訳出力前に振替・分割済で、計上仕訳出力済の場合
            /// </summary>
            State4,

            /// <summary>
            /// 画面設計書 データ遷移図⑤<para/>
            /// 計上仕訳出力済で、振替・分割していない場合
            /// </summary>
            State5,

            /// <summary>
            /// 画面設計書 データ遷移図⑥<para/>
            /// 計上仕訳出力済で、振替・分割実行済、振替仕訳未出力
            /// </summary>
            State6,

            /// <summary>
            /// 画面設計書 データ遷移図⑦<para/>
            /// 計上仕訳出力済で、振替・分割実行済、振替仕訳出力済
            /// </summary>
            State7
        }

        /// <summary>
        /// 前受金状態
        /// </summary>
        private AdvanceReceiptState AdvanceReceiptStatus { get; set; }

        /// <summary>
        /// グリッド表示データ。
        /// 表示順は保持しない。
        /// </summary>
        private List<GridRow> GridRows
        {
            get
            {
                if (grid.DataSource == null)
                {
                    return null;
                }

                var bindingData = (BindingSource)grid.DataSource;
                return (List<GridRow>)bindingData.List;
            }

            set
            {
                grid.DataSource = new BindingSource(value, null);
            }
        }

        /// <summary>
        /// Receipt.AssignmentFlag
        /// </summary>
        private enum ReceiptAssignmentFlag
        {
            未消込 = 0,
            一部消込 = 1,
            消込済 = 2,
        }

        /// <summary>
        /// ApplicationControl.CustomerCodeType
        /// </summary>
        private enum CustomerCodeType
        {
            数字 = 0,
            英数 = 1,
            英数カナ = 2,
        }

        #region 初期化

        public PD0602()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            Text = "前受金振替・分割処理";
            grid.SetupShortcutKeys();
            grid.GridColorType = GridColorType.Input;
        }

        private void InitializeHandlers()
        {
            grid.CellValueChanged       += grid_CellValueChanged;
            grid.CellBeginEdit          += grid_CellBeginEdit;
            grid.CellContentClick       += grid_CellContentClick;
            grid.CellContentButtonClick += grid_CellContentButtonClick;
            grid.CellEnter += (sender, e) => { if (grid.CurrentCell.Enabled) grid.BeginEdit(selectAll: true); };
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("削除");
            BaseContext.SetFunction03Caption("再表示");
            BaseContext.SetFunction04Caption("振替取消");
            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction10Caption("戻る");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(true);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = RegisterReceipts;
            OnF02ClickHandler = DeleteSelectedReceipts;
            OnF03ClickHandler = ReloadAdvanceReceipt;
            OnF04ClickHandler = CancelSplit;
            OnF08ClickHandler = SelectAllCheckBoxes;
            OnF09ClickHandler = DeselectAllCheckBoxes;
            OnF10ClickHandler = Exit;
        }

        private void PD0602_Load(object sender, EventArgs e)
        {
            SetScreenName();

            var getReceiptColumnNameSettings = GetColumnNameSettingsAsync();
            var getCategory = GetCategoriesAsync();

            var loadTask = new List<Task>
            {
                ApplicationControl == null ? LoadApplicationControlAsync() : Task.CompletedTask,
                Company == null            ? LoadCompanyAsync()            : Task.CompletedTask,
                LoadControlColorAsync(),
                getReceiptColumnNameSettings,
                getCategory,
            };
            ProgressDialog.Start(BaseForm, Task.WhenAll(loadTask), false, SessionKey);

            ColumnNameSettings = getReceiptColumnNameSettings.Result;
            ReceiptCategories = getCategory.Result;

            Debug.Assert(SelectedAdvanceReceipt.OriginalReceiptId != null);
            Debug.Assert(ApplicationContext != null);
            Debug.Assert(ApplicationContext.Login != null);
            Debug.Assert(ApplicationControl != null);
            Debug.Assert(ColumnNameSettings != null);
            Debug.Assert(ReceiptCategories != null);

            lblOrgCurrencyCode.Visible = UseForeignCurrency;
            txtOrgCurrencyCode.Visible = UseForeignCurrency;

            lblOrgSectionCode.Visible = UseSection;
            txtOrgSectionCode.Visible = UseSection;
            lblOrgSectionName.Visible = UseSection;

            lblOrgNote1.Text = GetNotesCaption(nameof(Receipt.Note1));
            lblOrgNote2.Text = GetNotesCaption(nameof(Receipt.Note2));
            lblOrgNote3.Text = GetNotesCaption(nameof(Receipt.Note3));
            lblOrgNote4.Text = GetNotesCaption(nameof(Receipt.Note4));

            LoadData();
        }

        private void InitializeOriginalReceiptInfo()
        {
            nmbOrgReceiptAmount.Fields.SetFields(AmountInputFormat, "", "", "-", "");
            nmbOrgReceiptAmount.DisplayFields.Clear();
            nmbOrgReceiptAmount.DisplayFields.AddRange(AmountDisplayFormat, "", "", "-", "");
            nmbOrgRemainAmount.Fields.SetFields(AmountInputFormat, "", "", "-", "");
            nmbOrgRemainAmount.DisplayFields.Clear();
            nmbOrgRemainAmount.DisplayFields.AddRange(AmountDisplayFormat, "", "", "-", "");

            datOrgRecordedAt.Value = OriginalReceipt.RecordedAt;
            txtOrgPayerName.Text = OriginalReceipt.PayerName;
            txtOrgCustomerCode.Text = OriginalReceipt.CustomerCode;
            lblOrgCustomerName.Text = OriginalReceipt.CustomerName;
            txtOrgSectionCode.Text = OriginalReceipt.SectionCode;
            lblOrgSectionName.Text = OriginalReceipt.SectionName;
            nmbOrgReceiptAmount.Value = OriginalReceipt.ReceiptAmount;
            txtOrgReceiptCategory.Text = OriginalReceipt.CategoryCodeName;
            nmbOrgRemainAmount.Value = OriginalReceipt.ReceiptAmount - OriginalReceipt.AssignmentAmount;
            txtOrgMemo.Text = OriginalReceipt.ReceiptMemo;
            txtOrgNote1.Text = OriginalReceipt.Note1;
            txtOrgNote2.Text = OriginalReceipt.Note2;
            txtOrgNote3.Text = OriginalReceipt.Note3;
            txtOrgNote4.Text = OriginalReceipt.Note4;
            txtOrgCurrencyCode.Text = OriginalReceipt.CurrencyCode;
        }

        private void InitializeNewLineInfo()
        {
            txtCustomerCode.Format = DataExpressionSettings.CustomerCodeFormatString;
            txtCustomerCode.MaxLength = DataExpressionSettings.CustomerCodeLength;
            txtCustomerCode.ImeMode = DataExpressionSettings.CustomerCodeImeMode();
            txtCustomerCode.PaddingChar = DataExpressionSettings.CustomerCodePaddingChar;

            nmbReceiptAmount.MinValue = (0 <= OriginalReceipt.ReceiptAmount) ? 1 : -AmountLimit;
            nmbReceiptAmount.MaxValue = (0 <= OriginalReceipt.ReceiptAmount) ? AmountLimit : -1;
            nmbReceiptAmount.Fields.SetFields(AmountInputFormat, "", "", "-", "");
            nmbReceiptAmount.DisplayFields.Clear();
            nmbReceiptAmount.DisplayFields.AddRange(AmountDisplayFormat, "", "", "-", "");
        }

        private void InitializeAdvanceReceipts()
        {
            nmbTransferAmountTotal.Fields.SetFields(AmountInputFormat, "", "", "-", "");
            nmbTransferAmountTotal.DisplayFields.Clear();
            nmbTransferAmountTotal.DisplayFields.AddRange(AmountDisplayFormat, "", "", "-", "");
            nmbSplitRemaining.Fields.SetFields(AmountInputFormat, "", "", "-", "");
            nmbSplitRemaining.DisplayFields.Clear();
            nmbSplitRemaining.DisplayFields.AddRange(AmountDisplayFormat, "", "", "-", "");

            InitializeGrid();
        }

        #region GridRowData

        private class GridRow
        {
            public readonly Receipt Model;

            public GridRow(Receipt receiptModel)
            {
                Model = receiptModel;
            }

            public bool CanEdit { get { return Model.AssignmentFlag == (int)ReceiptAssignmentFlag.未消込; } }

            /// <summary>｢削除｣チェックボックス状態</summary>
            public int ToBeDeleted { get; set; }

            /// <summary>消込区分</summary>
            public string AssignmentFlagName { get { return Model.AssignmentFlagName; } }

            /// <summary>処理予定日</summary>
            public DateTime? ProcessingAt
            {
                get { return Model.ProcessingAt; }
                set { Model.ProcessingAt = value; }
            }

            /// <summary>得意先ID</summary>
            public int? CustomerId
            {
                get { return Model.CustomerId; }
                set { Model.CustomerId = value; }
            }

            /// <summary>得意先コード</summary>
            public string CustomerCode
            {
                get { return Model.CustomerCode; }
                set { Model.CustomerCode = value; }
            }

            /// <summary>得意先名</summary>
            public string CustomerName
            {
                get { return Model.CustomerName; }
                set { Model.CustomerName = value; }
            }

            /// <summary>入金額</summary>
            public decimal ReceiptAmount
            {
                get { return Model.ReceiptAmount; }
                set { Model.ReceiptAmount = value; }
            }

            /// <summary>メモ</summary>
            public string Memo
            {
                get { return Model.ReceiptMemo; }
                set { Model.ReceiptMemo = value; }
            }

            /// <summary>備考1</summary>
            public string Note1
            {
                get { return Model.Note1; }
                set { Model.Note1 = value; }
            }

            /// <summary>備考2</summary>
            public string Note2
            {
                get { return Model.Note2; }
                set { Model.Note2 = value; }
            }

            /// <summary>備考3</summary>
            public string Note3
            {
                get { return Model.Note3; }
                set { Model.Note3 = value; }
            }

            /// <summary>備考4</summary>
            public string Note4
            {
                get { return Model.Note4; }
                set { Model.Note4 = value; }
            }
        }

        #endregion GridRowData

        private void InitializeGrid()
        {
            grid.Template = GetGridTemplate();
        }

        private Template GetGridTemplate()
        {
            var template = new Template();
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var rowHeight = builder.DefaultRowHeight;

            #region Header

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight,  40, "ToBeDeleted"         , caption: "削除"                        ),
                new CellSetting(rowHeight,  85, "AssignmentFlagName"  , caption: "消込区分"                  , sortable: true ),
                new CellSetting(rowHeight, 110, "ProcessingAt"        , caption: "処理予定日"                , sortable: true ),
                new CellSetting(rowHeight, 130, "CustomerCode"        , caption: "得意先コード"              , sortable: true ),
                new CellSetting(rowHeight,   0, "SearchCustomerButton", caption: ""                          , sortable: true ),
                new CellSetting(rowHeight, 130, "CustomerName"        , caption: "得意先名"                  , sortable: true ),
                new CellSetting(rowHeight, 120, "ReceiptAmount2"      , caption: "入金額"                    , sortable: true ),
                new CellSetting(rowHeight, 200, "Memo"                , caption: "メモ"                      , sortable: true ),
                new CellSetting(rowHeight, 200, "Note1"               , caption: GetNotesCaption("Note1")    , sortable: true ),
                new CellSetting(rowHeight, 200, "Note2"               , caption: GetNotesCaption("Note2")    , sortable: true ),
                new CellSetting(rowHeight, 200, "Note3"               , caption: GetNotesCaption("Note3")    , sortable: true ),
                new CellSetting(rowHeight, 200, "Note4"               , caption: GetNotesCaption("Note4")    , sortable: true ),
            });

            builder.BuildHeaderOnly(template);

            #endregion Header

            builder.Items.Clear();

            #region Rows

            var precision = OriginalReceiptCurrency.Precision;
            var center = MultiRowContentAlignment.MiddleCenter;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight, 40,  "ToBeDeleted",        dataField: "ToBeDeleted",        cell: builder.GetCheckBoxCell(),                                     readOnly: false ), // 削除
                new CellSetting(rowHeight, 85,  "AssignmentFlagName", dataField: "AssignmentFlagName", cell: builder.GetTextBoxCell() ),                                                       // 消込区分
                new CellSetting(rowHeight, 110, "ProcessingAt",       dataField: "ProcessingAt",       cell: builder.GetDateCell_yyyyMMdd(isInput: true),                   readOnly: false ), // 処理予定日
                new CellSetting(rowHeight, 95,  "CustomerCode",       dataField: "CustomerCode",       cell: builder.GetTextBoxCell(center),                                readOnly: false ), // 得意先コード
                new CellSetting(rowHeight, 35,  "SearchCustomerButton",                                cell: builder.GetButtonCell(), value: "…" ),                                           // 得意先検索ボタン
                new CellSetting(rowHeight, 130, "CustomerName",       dataField: "CustomerName",       cell: builder.GetTextBoxCell() ),                                                       // 得意先名
                new CellSetting(rowHeight, 120, "ReceiptAmount",      dataField: "ReceiptAmount",      cell: builder.GetNumberCellCurrency(precision, precision, 0),        readOnly: false ), // 入金額
                new CellSetting(rowHeight, 200, "Memo",               dataField: "Memo",               cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana), readOnly: false ), // メモ
                new CellSetting(rowHeight, 200, "Note1",              dataField: "Note1",              cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana), readOnly: false ), // 備考1
                new CellSetting(rowHeight, 200, "Note2",              dataField: "Note2",              cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana), readOnly: false ), // 備考2
                new CellSetting(rowHeight, 200, "Note3",              dataField: "Note3",              cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana), readOnly: false ), // 備考3
                new CellSetting(rowHeight, 200, "Note4",              dataField: "Note4",              cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana), readOnly: false ), // 備考4
            });

            // 個別設定
            foreach (var cs in builder.Items)
            {
                if (cs.Name == "CustomerCode")
                {
                    var cell = (GcTextBoxCell)cs.CellInstance;
                    cell.Format = DataExpressionSettings.CustomerCodeFormatString;
                    cell.MaxLength = DataExpressionSettings.CustomerCodeLength;
                    cell.Style.ImeMode = DataExpressionSettings.CustomerCodeImeMode();
                }
                else if (cs.Name == "ReceiptAmount")
                {
                    var cell = (GcNumberCell)cs.CellInstance;
                    cell.MaxValue = (0 <= OriginalReceipt.ReceiptAmount) ? AmountLimit : -1;
                    cell.MinValue = (0 <= OriginalReceipt.ReceiptAmount) ? 0 : -AmountLimit;
                    cell.Fields.SetFields(AmountInputFormat, "", "", "-", "");
                }
            }

            builder.BuildRowOnly(template);

            #endregion Rows

            return template;
        }

        #endregion 初期化

        #region 画面イベント

        [OperationLog("行削除")] // F2
        private void DeleteSelectedReceipts()
        {
            try
            {
                var source = (BindingSource)grid.DataSource;

                var targets = GridRows.Where(r => r.ToBeDeleted == 1).ToArray();
                targets.ForEach(r => source.Remove(r));

                UpdateGridSummary();

                Modified = true;
                BaseContext.SetFunction02Enabled(false);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("再表示")] // F3
        private void ReloadAdvanceReceipt()
        {
            try
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                {
                    return;
                }

                LoadData();
                ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("全選択")] // F8
        private void SelectAllCheckBoxes()
        {
            try
            {
                GridRows.ForEach(r => r.ToBeDeleted = r.CanEdit ? 1 : 0);
                grid.Refresh();

                BaseContext.SetFunction02Enabled(0 < GridRows.Count(r => r.CanEdit));
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("全解除")] // F9
        private void DeselectAllCheckBoxes()
        {
            try
            {
                GridRows.ForEach(r => r.ToBeDeleted = 0);
                grid.Refresh();

                BaseContext.SetFunction02Enabled(false);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("戻る")] // F10
        private void Exit()
        {
            try
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                {
                    return;
                }

                BaseForm.Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// ｢行の追加｣グループボックス内の｢追加｣ボタン
        /// </summary>
        [OperationLog("行追加")]
        private void btnAddLines_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();

            try
            {
                if ((grid.RowCount + nmbNumberOfLines.Value) > nmbNumberOfLines.MaxValue)
                {
                    ShowWarningDialog(MsgWngAdvancedReceiveSplitRowCountError);
                    return;
                }

                var source = (BindingSource)grid.DataSource;
                Console.WriteLine(grid.RowCount);
                for (var i = 0; i < nmbNumberOfLines.Value; i++)
                {
                    var receipt = new Receipt
                    {
                        ProcessingAt = datProcessingAt.Value,
                        ReceiptAmount = nmbReceiptAmount.Value ?? ((0 <= OriginalReceipt.ReceiptAmount) ? 0 : -1),
                    };

                    if (!string.IsNullOrEmpty(txtCustomerCode.Text))
                    {
                        var customer = GetCustomer(txtCustomerCode.Text);
                        if (customer == null)
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "得意先", txtCustomerCode.Text);
                            return;
                        }

                        receipt.CustomerId = customer.Id;
                        receipt.CustomerCode = customer.Code;
                        receipt.CustomerName = customer.Name;
                    }

                    source.Add(new GridRow(receipt));
                }

                Modified = true;
                UpdateGridSummary();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("登録")] // F01
        private void RegisterReceipts()
        {
            ClearStatusMessage();

            try
            {
                var error = ValidateForRegister();
                if (error != null)
                {
                    if (error is ValidationError<Cell>)
                    {
                        var actualError = (ValidationError<Cell>)error;
                        if (actualError.FocusTarget != null)
                        {
                            grid.CurrentCell = actualError.FocusTarget;
                            grid.Focus();
                        }
                    }
                    else if (error is ValidationError<Control>)
                    {
                        var actualError = (ValidationError<Control>)error;
                        if (actualError.FocusTarget != null)
                        {
                            actualError.FocusTarget.Focus();
                        }
                    }

                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    return;
                }

                var splitReceipts = GridRows.Where(row => /* 未消込のみ */ row.CanEdit).Select(row => new AdvanceReceivedSplit
                {
                    ProcessingAt = row.ProcessingAt,
                    CustomerId = row.CustomerId.Value,
                    StaffId = null, // UNDONE: 現時点では｢担当者｣入力欄を実装していないのでnullをセット
                    ReceiptAmount = row.ReceiptAmount,
                    Memo = row.Memo,
                    Note1 = row.Note1,
                    Note2 = row.Note2,
                    Note3 = row.Note3,
                    Note4 = row.Note4,
                });

                var task = AdvanceReceivedDataSplitAsync(SessionKey, CompanyId, Login.UserId, OriginalReceiptCurrency.Id, OriginalReceipt.Id, splitReceipts.ToArray());
                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }

                IsDataRegistered = true;

                LoadData();
                ShowWarningDialog(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("振替取消")] // F04
        private void CancelSplit()
        {
            ClearStatusMessage();

            try
            {
                if (!ShowConfirmDialog(MsgQstConfirmUpdate, "振替を取り消して、元の前受データに戻します。" + Environment.NewLine))
                {
                    return;
                }

                var task = CancelAdvanceReceivedDataSplitAsync(SessionKey, CompanyId, Login.UserId, OriginalReceiptCurrency.Id, OriginalReceipt.Id);
                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                if (!task.Result)
                {
                    ShowWarningDialog(MsgErrUpdateError);
                    return;
                }

                IsDataRegistered = true;

                LoadData();
                ShowWarningDialog(MsgInfUpdateSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// ｢行の追加｣グループボックス内の得意先コード検索ボタン
        /// </summary>
        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = this.ShowCustomerMinSearchDialog();
                if (customer != null)
                {
                    txtCustomerCode.Text = customer.Code;
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>
        /// グリッド内の得意先コード検索ボタン
        /// </summary>
        private void grid_CellContentButtonClick(object sender, CellEventArgs e)
        {
            try
            {
                var row = (GridRow)grid.Rows[e.RowIndex].DataBoundItem;

                if (e.CellName == "celSearchCustomerButton")
                {
                    if (!row.CanEdit || grid.ReadOnly)
                    {
                        return;
                    }

                    var customer = this.ShowCustomerMinSearchDialog();
                    if (customer != null)
                    {
                        if (customer.Code != row.CustomerCode)
                        {
                            Modified = true;
                        }

                        row.CustomerId = customer.Id;
                        row.CustomerCode = customer.Code;
                        row.CustomerName = customer.Name;

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

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            try
            {
                if (e.CellName != "celToBeDeleted")
                {
                    Modified = true;
                }


                // 削除チェックボックス
                if (e.CellName == "celToBeDeleted")
                {
                    BaseContext.SetFunction02Enabled(GridRows.Any(r => r.ToBeDeleted == 1));
                }

                // 得意先コード
                else if (e.CellName == "celCustomerCode")
                {
                    var row = (GridRow)grid.Rows[e.RowIndex].DataBoundItem;
                    try
                    {
                        if (string.IsNullOrEmpty(row.CustomerCode))
                        {
                            row.CustomerId = null;
                            row.CustomerName = null;
                            return;
                        }

                        if (ApplicationControl.CustomerCodeType == (int)CustomerCodeType.数字)
                        {
                            row.CustomerCode = row.CustomerCode.PadLeft(ApplicationControl.CustomerCodeLength, '0');
                        }

                        var customer = GetCustomer(row.CustomerCode);
                        if (customer == null)
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "得意先", row.CustomerCode);
                            row.CustomerId = null;
                            row.CustomerCode = null;
                            row.CustomerName = null;
                            return;
                        }

                        row.CustomerId = customer.Id;
                        row.CustomerName = customer.Name;

                        ClearStatusMessage();
                    }
                    finally
                    {
                        grid.Refresh();
                    }
                }

                // 入金額
                else if (e.CellName == "celReceiptAmount")
                {
                    UpdateGridSummary();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grid_CellContentClick(object sender, CellEventArgs e)
        {
            try
            {
                if (e.CellName == "celToBeDeleted")
                {
                    grid.EndEdit();
                    BaseContext.SetFunction02Enabled(GridRows.Any(r => r.ToBeDeleted == 1));
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grid_CellBeginEdit(object sender, CellBeginEditEventArgs e)
        {
            try
            {
                var row = (GridRow)grid.Rows[e.RowIndex].DataBoundItem;

                if (!row.CanEdit)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #endregion 画面イベント

        #region Functions

        private void LoadData()
        {
            var getAdvanceReceipts = GetAdvanceReceiptsAsync(SelectedAdvanceReceipt.OriginalReceiptId.Value);
            var getAdvanceReceiptBackup = GetAdvanceReceivedBackupAsync(SelectedAdvanceReceipt.OriginalReceiptId.Value);
            var getCurrency = GetCurrencyAsync(SelectedAdvanceReceipt.CurrencyId);

            var loadTask = new List<Task> { getAdvanceReceipts, getAdvanceReceiptBackup, getCurrency };
            ProgressDialog.Start(BaseForm, Task.WhenAll(loadTask), false, SessionKey);

            OriginalReceipt = getAdvanceReceipts.Result?.OriginalReceipt;
            AdvanceReceipts = getAdvanceReceipts.Result?.AdvanceReceipts;
            AdvanceReceiptBackup = getAdvanceReceiptBackup.Result;
            OriginalReceiptCurrency = getCurrency.Result;

            Debug.Assert(OriginalReceipt != null);
            Debug.Assert(AdvanceReceipts != null);
            Debug.Assert(1 <= AdvanceReceipts.Count);
            Debug.Assert(OriginalReceiptCurrency != null);

            SelectedAdvanceReceipt = DeepCopy(AdvanceReceipts.First()); // 1行目のデータで置き換え(どの行でも問題ない)
            AdvanceReceiptStatus = DetermineAdvanceReceiptStatus();

            var decimalPart = (0 < OriginalReceiptCurrency.Precision) ? $".{new string('0', OriginalReceiptCurrency.Precision)}" : "";
            AmountInputFormat = "##,###,###,###" + decimalPart;
            AmountDisplayFormat = "##,###,###,##0" + decimalPart;

            var limitStr = $"99999999999";
            if (0 < OriginalReceiptCurrency.Precision)
            {
                limitStr += "." + new string('9', OriginalReceiptCurrency.Precision);
            }
            AmountLimit = decimal.Parse(limitStr);

            DataExpressionSettings = new DataExpression(ApplicationControl);

            Debug.Assert(!string.IsNullOrWhiteSpace(AmountInputFormat));
            Debug.Assert(!string.IsNullOrWhiteSpace(AmountDisplayFormat));
            Debug.Assert(0 < AmountLimit);
            Debug.Assert(DataExpressionSettings != null);

            InitializeOriginalReceiptInfo();
            InitializeNewLineInfo();
            InitializeAdvanceReceipts();

            SetGridRows(AdvanceReceipts.Select(r => new GridRow(r)));
            SetScreenMode();

            Modified = false;
        }

        private void SetGridRows(IEnumerable<GridRow> gridRows)
        {
            GridRows = gridRows.ToList();
            grid.DataSource = new BindingSource(GridRows, null);

            UpdateGridSummary();
        }

        /// <summary>
        /// 振替額合計と残額を再計算する。
        /// </summary>
        private void UpdateGridSummary()
        {
            nmbTransferAmountTotal.Value = GridRows.Sum(r => r.ReceiptAmount);
            nmbSplitRemaining.Value = nmbOrgRemainAmount.Value.Value - nmbTransferAmountTotal.Value;
        }

        /// <summary>
        /// 前受金状態を決定する。
        /// </summary>
        private AdvanceReceiptState DetermineAdvanceReceiptStatus()
        {
            if (SelectedAdvanceReceipt.OutputAt == null && AdvanceReceiptBackup == null)
            {
                return AdvanceReceiptState.State23;
            }

            if (AdvanceReceiptBackup == null)
            {
                if (1 < AdvanceReceipts.Count)
                {
                    return AdvanceReceiptState.State4;
                }
                else
                {
                    return AdvanceReceiptState.State5;
                }
            }
            else
            {
                if (AdvanceReceiptBackup.TransferOutputAt == null)
                {
                    return AdvanceReceiptState.State6;
                }
                else
                {
                    return AdvanceReceiptState.State7;
                }
            }

            throw new NotImplementedException($"前受金状態の判別に失敗しました。SelectedAdvanceReceipt.Id = {SelectedAdvanceReceipt.Id}");
        }

        /// <summary>
        /// 前受金状態に従って画面モードを制御する。
        /// </summary>
        private void SetScreenMode()
        {
            BaseContext.SetFunction01Enabled(true);     // 登録
            BaseContext.SetFunction02Enabled(false);    // 行削除
            BaseContext.SetFunction03Enabled(true);     // 再表示
            BaseContext.SetFunction04Enabled(false);    // 振替取消
            BaseContext.SetFunction08Enabled(true);     // 全選択
            BaseContext.SetFunction09Enabled(true);     // 全解除
            BaseContext.SetFunction10Enabled(true);     // 戻る

            switch (AdvanceReceiptStatus)
            {
                case AdvanceReceiptState.State23:
                    grpNewLine.Enabled = true;
                    grid.ReadOnly = false;
                    datOutputAt.Clear();
                    datTransferOutputAt.Clear();
                    datTransferDate.Clear();
                    datTransferDate.Enabled = false;
                    cbxTransferOutputAt.Checked = false;
                    break;

                case AdvanceReceiptState.State4:
                    grpNewLine.Enabled = false;
                    grid.ReadOnly = true;
                    datOutputAt.Value = SelectedAdvanceReceipt.OutputAt;
                    datTransferOutputAt.Clear();
                    datTransferDate.Clear();
                    datTransferDate.Enabled = false;
                    cbxTransferOutputAt.Checked = false;
                    BaseContext.SetFunction01Enabled(false);
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                    break;

                case AdvanceReceiptState.State5:
                    grpNewLine.Enabled = true;
                    grid.ReadOnly = false;
                    datOutputAt.Value = SelectedAdvanceReceipt.OutputAt;
                    datTransferOutputAt.Clear();
                    datTransferDate.Clear();
                    datTransferDate.Enabled = true;
                    cbxTransferOutputAt.Checked = false;
                    break;

                case AdvanceReceiptState.State6:
                    grpNewLine.Enabled = true;
                    grid.ReadOnly = false;
                    datOutputAt.Value = AdvanceReceiptBackup.OutputAt;
                    datTransferOutputAt.Clear();
                    datTransferDate.Value = SelectedAdvanceReceipt.RecordedAt;
                    datTransferDate.Enabled = GridRows.All(r => r.CanEdit);
                    cbxTransferOutputAt.Checked = true;
                    BaseContext.SetFunction04Enabled(GridRows.All(r => r.CanEdit));
                    break;

                case AdvanceReceiptState.State7:
                    grpNewLine.Enabled = false;
                    grid.ReadOnly = true;
                    datOutputAt.Value = AdvanceReceiptBackup.OutputAt;
                    datTransferOutputAt.Value = AdvanceReceiptBackup.TransferOutputAt;

                    datTransferDate.Value = SelectedAdvanceReceipt.RecordedAt;
                    datTransferDate.Enabled = false;
                    cbxTransferOutputAt.Checked = true;
                    BaseContext.SetFunction01Enabled(false);
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                    break;

                default:
                    if (Enum.IsDefined(typeof(AdvanceReceiptState), AdvanceReceiptStatus))
                        throw new NotImplementedException($"AdvanceReceiptStatus = {AdvanceReceiptStatus}");
                    else
                        throw new InvalidOperationException($"AdvanceReceiptStatus = {(int)AdvanceReceiptStatus}");
            }
        }

        /// <summary>
        /// 備考名称取得
        /// </summary>
        private string GetNotesCaption(string columnName)
        {
            return ColumnNameSettings
                .FirstOrDefault(s => s.TableName == nameof(Receipt) && s.ColumnName == columnName)
                .DisplayColumnName;
        }

        private class ValidationError
        {
            public string MessageId { get; private set; }
            public string[] MessageArgs { get; private set; }

            public ValidationError(string messageId, params string[] messageArgs)
            {
                MessageId = messageId;
                MessageArgs = messageArgs;
            }
        }

        private class ValidationError<TFocusTarget> : ValidationError
        {
            public TFocusTarget FocusTarget { get; private set; }

            public ValidationError(TFocusTarget focusTarget, string messageId, params string[] messageArgs) : base(messageId, messageArgs)
            {
                FocusTarget = focusTarget;
            }
        }

        private ValidationError ValidateForRegister()
        {
            // グリッドにデータが１行以上表示されている、かつ、残額が 0 ではない
            if (GridRows.Count != 0 && nmbSplitRemaining.Value != 0)
            {
                return new ValidationError(MsgWngAdvanceReceivedAmountUnmatchBeforeAndAfter);
            }

            // 計上仕訳済のデータを初めて更新する場合
            if (AdvanceReceiptBackup == null)
            {
                // データの分割を行っておらず、且つ、得意先コードの変更を行っていない
                if (GridRows.Count == 1 && GridRows.Single().CustomerCode == SelectedAdvanceReceipt.CustomerCode)
                {
                    return new ValidationError(MsgWngNoData, "登録できる");
                }
            }

            // 行数
            if (GridRows.Count == 0)
            {
                return new ValidationError(MsgWngInputGridRequired);
            }

            // 前受仕訳計上済(前受仕訳出力済)
            var s5 = (AdvanceReceiptStatus == AdvanceReceiptState.State5);
            var s6 = (AdvanceReceiptStatus == AdvanceReceiptState.State6);

            if (s5 || s6)
            {
                if (datTransferDate.Value == null)
                {
                    return new ValidationError<Control>(datTransferDate, MsgWngInputRequired, "振替処理年月日");
                }
                if (datTransferDate.Value < OriginalReceipt.RecordedAt)
                {
                    return new ValidationError<Control>(datTransferDate, MsgWngInputParam1DateAfterParam2Date, "振替処理年月日", "入金日");
                }
            }

            // グリッド各行
            foreach (var row in grid.Rows
                .Select(row => new { Index = row.Index, BoundData = (GridRow)row.DataBoundItem })
                .Where(row => row.BoundData.CanEdit)) // 未消込のみ
            {
                var cell_ProcessingAt = grid.Rows[row.Index].Cells["celProcessingAt"];
                var cell_CustomerCode = grid.Rows[row.Index].Cells["celCustomerCode"];
                var cell_ReceiptAmount = grid.Rows[row.Index].Cells["celReceiptAmount"];

                if (row.BoundData.ProcessingAt.HasValue)
                {
                    var date_name = // tuple(比較対象日, 比較対象項目名)
                        datTransferDate.Value.HasValue
                        ? new Tuple<DateTime, string>(datTransferDate.Value.Value, "振替処理年月日")
                        : new Tuple<DateTime, string>(OriginalReceipt.RecordedAt, "入金日");

                    if (row.BoundData.ProcessingAt.Value < date_name.Item1)
                    {
                        return new ValidationError<Cell>(cell_ProcessingAt, MsgWngInputParam1DateAfterParam2Date, "処理予定日", date_name.Item2);
                    }
                }

                if (string.IsNullOrEmpty(row.BoundData.CustomerCode))
                {
                    return new ValidationError<Cell>(cell_CustomerCode, MsgWngInputRequired, "得意先コード");
                }
                if (row.BoundData.ReceiptAmount == 0)
                {
                    return new ValidationError<Cell>(cell_ReceiptAmount, MsgWngInputRequired, "入金額");
                }
            };

            return null;
        }

        #endregion Functions

        #region Web Service

        private async Task<AdvanceReceiptsResult> GetAdvanceReceiptsAsync(long originalReceiptId)
        {
            AdvanceReceiptsResult result = null;
            try
            {
                await ServiceProxyFactory.DoAsync<ReceiptServiceClient>(async client
                    => result = await client.GetAdvanceReceiptsAsync(SessionKey, originalReceiptId));
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            return result;
        }

        private async Task<AdvanceReceivedBackup> GetAdvanceReceivedBackupAsync(long originalReceiptId)
        {
            AdvanceReceivedBackupResult result = null;
            try
            {
                await ServiceProxyFactory.DoAsync<AdvanceReceivedBackupServiceClient>(async client
                    => result = await client.GetByOriginalReceiptIdAsync(SessionKey, originalReceiptId));
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            return result.AdvanceReceivedBackup;
        }

        private Customer GetCustomer(string code)
        {
            Customer result = null;
            Task.Run(async () =>
            {
                result = await GetCustomerAsync(code);
            }).Wait();

            return result;
        }

        private async Task<Customer> GetCustomerAsync(string code)
        {
            CustomersResult result = null;
            try
            {
                await ServiceProxyFactory.DoAsync<CustomerMasterClient>(async client
                    => result = await client.GetByCodeAsync(SessionKey,CompanyId, new[] { code} ));
                if (result == null || result.ProcessResult.Result == false)
                    return null;
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            return result.Customers.SingleOrDefault();
        }

        private async Task<Currency> GetCurrencyAsync(int currencyId)
        {
            CurrenciesResult result = null;
            try
            {
                await ServiceProxyFactory.DoAsync<CurrencyMasterClient>(async client
                    => result = await client.GetAsync(SessionKey, new[] { currencyId }));
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            return result.Currencies.SingleOrDefault();
        }

        private async Task<List<ColumnNameSetting>> GetColumnNameSettingsAsync()
        {
            ColumnNameSettingsResult result = null;
            try
            {
                await ServiceProxyFactory.DoAsync<ColumnNameSettingMasterClient>(async client
                    => result = await client.GetItemsAsync(SessionKey, CompanyId));
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            return result.ColumnNames;
        }

        private async Task<List<Category>> GetCategoriesAsync()
        {
            CategoriesResult result = null;
            try
            {
                await ServiceProxyFactory.DoAsync<CategoryMasterClient>(async client
                    => result = await client.GetByCodeAsync(SessionKey, CompanyId, CategoryType.Receipt, null));
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            return result.Categories;
        }

        private static async Task<bool> AdvanceReceivedDataSplitAsync(string sessionKey, int companyId, int loginUserId, int currencyId, long originalReceiptId, AdvanceReceivedSplit[] advanceReceivedSplitList)
        {
            var result = await ServiceProxyFactory.DoAsync(async (ReceiptService.ReceiptServiceClient client)
                => await client.AdvanceReceivedDataSplitAsync(sessionKey, companyId, loginUserId, currencyId, originalReceiptId, advanceReceivedSplitList));
            return result?.ProcessResult.Result ?? false;
        }

        private static async Task<bool> CancelAdvanceReceivedDataSplitAsync(string sessionKey, int companyId, int loginUserId, int currencyId, long originalReceiptId)
        {
            var result = await ServiceProxyFactory.DoAsync(async (ReceiptService.ReceiptServiceClient client)
                => await client.CancelAdvanceReceivedDataSplitAsync(sessionKey, companyId, loginUserId, currencyId, originalReceiptId));
            return result?.ProcessResult.Result ?? false;
        }

        #endregion Web Service

        #region Helper

        public static T DeepCopy<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        #endregion Helper
    }
}
