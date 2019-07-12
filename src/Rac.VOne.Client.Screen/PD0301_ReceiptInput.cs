using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.BankBranchMasterService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CollationSettingMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.InputControlMasterService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using Section = Rac.VOne.Web.Models.Section;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金データ入力</summary>
    public partial class PD0301 : VOneScreenBase
    {
        #region 変数宣言
        public Func<IEnumerable<ITransactionData>, bool> SavePostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> DeletePostProcessor { get; set; }

        public VOneScreenBase ReturnScreen { get; set; }

        /// <summary><see cref="PD0501"/>入金データ検索からの呼び出し</summary>
        private bool FromReceiptSearch
        { get { return ReturnScreen is PD0501; } }

        /// <summary><see cref="PE0102"/>個別消込画面からの呼び出し</summary>
        private bool FromMatchingIndividual
        { get { return ReturnScreen is PE0102; } }

        private bool FromMatchingInput
        { get { return FromMatchingIndividual && IsNewData; } }

        private bool FromMatchingEdit
        { get { return FromMatchingIndividual && !IsNewData; } }

        /// <summary>他画面からの呼び出し</summary>
        private bool FromOtherForm
        { get { return ReturnScreen != null; } }

        /// <summary>現在表示しているReceipt の値
        /// 入金データ検索 → 入金データ入力
        /// 入金データ入力 → F8/入金検索 呼び出しの値
        /// 個別消込 → 修正時の値
        /// </summary>
        public Receipt CurrentReceipt { get; set; }

        private bool IsNewData { get { return CurrentReceipt == null; } }
        /// <summary>入金データ 入力区分:2入力 で、全フィールドが更新対象</summary>
        private bool UpdateAll
        { get { return CurrentReceipt?.InputType == ReceiptInputType; } }

        /// <summary>得意先ID 個別消込画面からの連携</summary>
        public int InCustomerId { get; set; }
        /// <summary>得意先コード 個別消込画面からの連携</summary>
        public string InCustomerCode { get; set; }
        /// <summary>通貨コード 個別消込画面からの連携</summary>
        public string InCurrencyCode { get; set; }
        /// <summary>入金日 一括消込画面で指定した入金日From</summary>
        public DateTime? InRecordedAtFrom { get; set; }
        /// <summary>入金日 一括消込画面で指定した入金日To</summary>
        public DateTime? InRecordedAtTo { get; set; }
        /// <summary>クライアントキー 個別消込画面より取得</summary>
        public byte[] InClientKey { get; set; }

        /// <summary>請求残 個別消込画面の値</summary>
        public decimal InRemainAmount { get; set; }
        /// <summary>入金ID 個別消込画面へ戻す登録/修正/削除した入金ID の配列</summary>
        public long[] OutReceiptId { get; set; }
        public bool IsDeleted { get; set; }

        private List<ReceiptInput> ReceiptInputList { get; set; } = new List<ReceiptInput>();
        private List<Customer> CustomerList { get; set; }
        private List<Section> SectionList { get; set; }
        private List<Currency> CurrencyList { get; set; }
        private List<Category> CategoryList { get; set; }
        private List<BankBranch> BankBranchList { get; set; }
        private List<InputControl> InputControlList { get; set; }
        private CollationSetting CollationSettingInfo { get; set; }
        private List<ColumnNameSetting> ColumnNameList { get; set; }

        private int CustomerId { get; set; }
        private int SectionId { get; set; }
        private int CurrencyId { get; set; }
        private string AmountFormat { get; set; }
        private int CurrencyPrecision { get; set; } = 5;

        private const int ReceiptUseInput = 1;
        private const int ReceiptCategoryType = 2;
        private const string SaveReceipt = "登録";
        private const string UpdateReceipt = "更新";
        private const string DeleteReceipt = "削除";

        /// <summary>入金データ 入力種別 2:入金データ入力</summary>
        private int ReceiptInputType => (int)Constants.ReceiptInputType.ReceiptInput;

        #endregion

        #region initialization

        public PD0301()
        {
            InitializeComponent();
            grdReceiptInput.SetupShortcutKeys();
            grdReceiptInput.GridColorType = GridColorType.Input;
            Text = "入金データ入力";
            InitializeContentChangegHandlers();
        }
        private void InitializeContentChangegHandlers()
        {
            foreach (var control in this.GetAll<Control>())
            {
                if (control is VOneDateControl)
                    ((VOneDateControl)control).ValueChanged += OnContentChanged;
                if (control is VOneTextControl)
                    ((VOneTextControl)control).TextChanged += OnContentChanged;
                if (control is CheckBox)
                    ((CheckBox)control).CheckedChanged += OnContentChanged;
            }
        }

        private void OnContentChanged(object sender, EventArgs e) => Modified = true;

        private void PD0301_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                LoadData();
                InitializeControlsEnabled();

                var expression = new DataExpression(ApplicationControl);
                InitializeCustomerControl(expression);
                InitializeSectionControlFormat(expression);
                InitializeCurrencyControl();

                SetCurrentReceiptInfo();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction04Caption("請求内訳");
            BaseContext.SetFunction05Caption("入力設定");
            BaseContext.SetFunction08Caption("入金検索");
            BaseContext.SetFunction09Caption("検索");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Save;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = Delete;
            OnF04ClickHandler = CallBillingSearch;
            OnF05ClickHandler = CallInputControlMaster;
            OnF08ClickHandler = CallReceiptSearch;
            OnF09ClickHandler = CallNameSearch;
            OnF10ClickHandler = Exit;
        }

        /// <summary>入金データ入力に必要なデータのロード処理</summary>
        /// <remarks>
        /// Company
        /// ApplicationControl
        /// CollationSetting
        /// Customer * MatchingIndividually
        /// Section * UseSection
        /// Currency
        /// Category * ReceiptCategory
        /// BankBranch * UseLimitDate
        /// InputControl
        /// </remarks>
        private void LoadData()
        {
            var loadTask = new List<Task>();
            loadTask.Add(LoadControlColorAsync());

            if (Company == null)
                loadTask.Add(LoadCompanyAsync());

            if (ApplicationControl == null)
                loadTask.Add(LoadApplicationControlAsync());

            if (CollationSettingInfo == null)
                loadTask.Add(LoadCollationSettingInfoAsync());

            if (InCustomerId != 0
                && FromMatchingIndividual)
                loadTask.Add(LoadCustomerInfoAsync());

            if (SectionList == null)
                loadTask.Add(LoadSectionInfoAsync());

            if (CurrencyList == null)
                loadTask.Add(LoadCurrencyInfoAsync());

            if (CategoryList == null)
                loadTask.Add(LoadCategoryInfoAsync());

            if (BankBranchList == null)
                loadTask.Add(LoadBankBranchInfoAsync());

            if (InputControlList == null)
                loadTask.Add(LoadInputControlInfoAsync());

            if (ColumnNameList == null)
                loadTask.Add(LoadColumnNameInfoAsync());

            ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
        }

        private void InitializeControlsEnabled()
        {
            var enabled = !(FromReceiptSearch) || UpdateAll;
            datRecordedAt.Enabled = enabled;
            txtSectionCode.Enabled = enabled;
            btnSectionCode.Enabled = enabled;
            grdReceiptInput.Enabled = enabled;

            if (CurrentReceipt?.InputType == (int)Constants.ReceiptInputType.EbFile)
            {
                cbxSaveKanaHistory.Visible = true;
                cbxSaveKanaHistory.Checked = CollationSettingInfo
                    ?.LearnSpecifiedCustomerKana == 1;
            }
            else
            {
                cbxSaveKanaHistory.Visible = false;
            }

            var fromMatching = FromMatchingIndividual;
            lblMatchingRecordedAt.Visible = fromMatching;
            lblMatchingBillingRemain.Visible = FromMatchingInput;
            lblMatchingBillingRemainAmount.Visible = FromMatchingInput;
            lblMatchingDifferent.Visible = FromMatchingInput;
            lblMatchingDifferentAmount.Visible = FromMatchingInput;

            if (fromMatching)
            {
                if (IsNewData)
                {
                    CustomerId = InCustomerId;
                    if (CustomerId != 0)
                    {
                        var customer = CustomerList.Find(c => c.Id == CustomerId);
                        if (customer == null)
                            return;

                        txtCustomerCode.Text = customer.Code;
                        lblCustomerName.Text = customer.Name;
                        lblPayerName.Text = customer.Kana;

                        var isParent = customer.IsParent == 1;
                        txtCustomerCode.Enabled = CustomerList.Count > 1;
                        btnCustomerCode.Enabled = CustomerList.Count > 1;
                    }
                }
                else
                {
                    CustomerId = CurrentReceipt.CustomerId.Value;
                    var customer = CustomerList.Find(c => c.Id == CustomerId);
                    if (customer == null)
                        return;

                    txtCustomerCode.Text = customer.Code;
                    lblCustomerName.Text = customer.Name;
                    lblPayerName.Text = customer.Kana;

                    txtCustomerCode.Enabled = false;
                    btnCustomerCode.Enabled = false;
                }

                if (!string.IsNullOrEmpty(InCurrencyCode))
                {
                    var currency = CurrencyList
                        .Find(c => (c.Code == InCurrencyCode));
                    if (currency == null)
                        return;

                    CurrencyId = currency.Id;
                    txtCurrencyCode.Text = currency.Code;
                    CurrencyPrecision = currency.Precision;
                    SetCurrencyDisplayString(CurrencyPrecision);
                }

                if (FromMatchingIndividual)
                {
                    var toText = InRecordedAtTo.Value.ToString("yyyy/MM/dd");
                    var fromText = InRecordedAtFrom.HasValue
                         ? InRecordedAtFrom.Value.ToString("yyyy/MM/dd") + " ～ "
                         : string.Empty;

                        lblMatchingRecordedAt.Text = fromText  + toText;
                }
            }

            var calledbyOtherForm = FromOtherForm;
            if (calledbyOtherForm)
            {
                BaseContext.SetFunction02Enabled(false);
                if (FromMatchingInput)
                    BaseContext.SetFunction03Enabled(false);
                else
                    BaseContext.SetFunction03Enabled(UpdateAll);
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction05Enabled(false);
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction07Enabled(false);
                BaseContext.SetFunction08Enabled(false);
                BaseContext.SetFunction09Enabled(false);
                BaseContext.SetFunction10Caption("戻る");
            }
        }

        #region 入金情報の設定

        private void SetCurrentReceiptInfo()
        {
            SetReceiptHeader();
            InitializeGridTemplate();
            SetReceiptDetail();
            CalculateTotalAmount();
            Modified = false;
        }

        private void SetReceiptHeader()
        {
            datRecordedAt.Value = DateTime.Today;

            lblStatus.Text = IsNewData ? "新規" : "修正";
            lblStatus.BackColor = IsNewData ? Color.Cyan : Color.Yellow;

            if (IsNewData) return;

            lblReceiptId.Text = CurrentReceipt.Id.ToString();
            lblPayerName.Text = CurrentReceipt.PayerName;
            datRecordedAt.Value = CurrentReceipt.RecordedAt;

            var currentCurrencyId = CurrentReceipt.CurrencyId;
            SetCurrencyInput(currentCurrencyId, string.Empty);

            var currentCustomerId = CurrentReceipt.CustomerId ?? 0;
            SetCustomerInput(currentCustomerId);

            var currentSectionId = CurrentReceipt.SectionId ?? 0;
            SetSectionInput(currentSectionId, string.Empty);
        }

        private void SetReceiptDetail()
        {
            if (IsNewData) return;
            var rowIndex = 0;
            var currentCategoryId = CurrentReceipt.ReceiptCategoryId;
            SetCategoryInputEnabled(currentCategoryId, string.Empty, rowIndex);

            var currentBankCode = CurrentReceipt.BillBankCode;
            var currentBranchCode = CurrentReceipt.BillBranchCode;
            SetBankBranchInputEnabled(currentBankCode, currentBranchCode, rowIndex);

            grdReceiptInput.Rows[rowIndex].Cells["celNote1"].Value = CurrentReceipt.Note1;
            grdReceiptInput.Rows[rowIndex].Cells["celDueAt"].Value = CurrentReceipt.DueAt;
            grdReceiptInput.Rows[rowIndex].Cells["celReceiptAmount"].Value = CurrentReceipt.ReceiptAmount;
            grdReceiptInput.Rows[rowIndex].Cells["celMemo"].Value = CurrentReceipt.ReceiptMemo;
            grdReceiptInput.Rows[rowIndex].Cells["celMemoSymbol"].Value =
                !string.IsNullOrEmpty(CurrentReceipt.ReceiptMemo) ? "○" : "";
            grdReceiptInput.Rows[rowIndex].Cells["celBillNumber"].Value = CurrentReceipt.BillNumber;
            grdReceiptInput.Rows[rowIndex].Cells["celBillBankCode"].Value = CurrentReceipt.BillBankCode;
            grdReceiptInput.Rows[rowIndex].Cells["celBillBranchCode"].Value = CurrentReceipt.BillBranchCode;
            grdReceiptInput.Rows[rowIndex].Cells["celBillDrawAt"].Value = CurrentReceipt.BillDrawAt;
            grdReceiptInput.Rows[rowIndex].Cells["celBillDrawer"].Value = CurrentReceipt.BillDrawer;
            grdReceiptInput.Rows[rowIndex].Cells["celNote2"].Value = CurrentReceipt.Note2;
            grdReceiptInput.Rows[rowIndex].Cells["celNote3"].Value = CurrentReceipt.Note3;
            grdReceiptInput.Rows[rowIndex].Cells["celNote4"].Value = CurrentReceipt.Note4;
            // row index 1 以降は編集不可
            for (var i = 1; i < grdReceiptInput.Rows.Count; i++)
            {
                foreach (var cell in grdReceiptInput.Rows[i].Cells)
                    cell.Enabled = false;
            }
        }

        #endregion

        #region グリッドテンプレート初期化
        private void InitializeGridTemplate()
        {
            var template = new Template();

            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);

            var height = builder.DefaultRowHeight;
            var precision = (ApplicationControl?.UseForeignCurrency == 1) ? CurrencyPrecision : 0;
            var numberCell = builder.GetNumberCellCurrencyInput(precision, precision, 0);
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var cellBorder = new Border(new Line(LineStyle.Thin, Color.Black));
            numberCell.AllowDeleteToNull = true;

            var posX = new int[]
            {
                0, 30, 180, 530, 645, 845, 875, 915,
                30, 180, 255, 330, 530, 645,
                30, 330, 645
            };
            var posY = new int[] { 0, height, height * 2 };

            var cells = InputControlList;
            var columnName1 = ColumnNameList.Find(x => x.TableName == "Receipt" && x.ColumnName == "Note1");
            var columnName2 = ColumnNameList.Find(x => x.TableName == "Receipt" && x.ColumnName == "Note2");
            var columnName3 = ColumnNameList.Find(x => x.TableName == "Receipt" && x.ColumnName == "Note3");
            var columnName4 = ColumnNameList.Find(x => x.TableName == "Receipt" && x.ColumnName == "Note4");

            var note1 = columnName1?.DisplayColumnName;
            var note2 = columnName2?.DisplayColumnName;
            var note3 = columnName3?.DisplayColumnName;
            var note4 = columnName4?.DisplayColumnName;

            #region ヘッダ
            builder.Items.AddRange(new CellSetting[]
            {
                #region ヘッダー1
                new CellSetting(height * 3,  30, "RowHeader"     , location: new Point(posX[ 0], posY[0])                           ),
                new CellSetting(height    , 150, "CategoryCode"  , location: new Point(posX[ 1], posY[0]), caption: "入金区分"     ),
                new CellSetting(height    , 350, "Note1"         , location: new Point(posX[ 2], posY[0]), caption: note1          ),
                new CellSetting(height    , 115, "DueAt"         , location: new Point(posX[ 3], posY[0]), caption: "期日"         ),
                new CellSetting(height    , 200, "ReceiptAmount" , location: new Point(posX[ 4], posY[0]), caption: "金額"         ),
                new CellSetting(height    ,  30, "Memo"          , location: new Point(posX[ 5], posY[0]), caption: ""             ),
                new CellSetting(height    ,  40, "MemoButton"    , location: new Point(posX[ 6], posY[0]), caption: "編集"         ),
                new CellSetting(height    ,  40, "ClearButton"   , location: new Point(posX[ 7], posY[0]), caption: "ボタン"       ),
                #endregion
                #region ヘッダー2
                new CellSetting(height    , 150, "BillNumber"    , location: new Point(posX[ 8], posY[1]), caption: "手形番号"     ),
                new CellSetting(height    ,  75, "BillBankCode"  , location: new Point(posX[ 9], posY[1]), caption: "銀行コード"   ),
                new CellSetting(height    ,  75, "BillBranchCode", location: new Point(posX[10], posY[1]), caption: "支店コード"   ),
                new CellSetting(height    , 200, "BankBranchName", location: new Point(posX[11], posY[1]), caption: "手形券面銀行" ),
                new CellSetting(height    , 115, "BillDrawAt"    , location: new Point(posX[12], posY[1]), caption: "振出日"       ),
                new CellSetting(height    , 310, "BillDrawer"    , location: new Point(posX[13], posY[1]), caption: "振出人"       ),
                #endregion
                #region ヘッダー3
                new CellSetting(height    , 300, "Note2"         , location: new Point(posX[14], posY[2]), caption: note2          ),
                new CellSetting(height    , 315, "Note3"         , location: new Point(posX[15], posY[2]), caption: note3          ),
                new CellSetting(height    , 310, "Note4"         , location: new Point(posX[16], posY[2]), caption: note4          )
                #endregion
            });
            #endregion

            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            var dotted = LineStyle.Dotted;
            var bdTop = builder.GetBorder(left: dotted, right: dotted, bottom: dotted);
            var bdMid = builder.GetBorder(all: dotted);
            var bdBtm = builder.GetBorder(left: dotted, top: dotted, right: dotted);
            #region データ
            builder.Items.AddRange(new CellSetting[]
            {
                #region データ1
                new CellSetting(height * 3,  30, "RowHeader"       , border: bdTop, location: new Point(posX[ 0], posY[0]), cell: builder.GetRowHeaderCell() ),
                new CellSetting(height    , 150, "CategoryId"      , border: bdTop, location: new Point(posX[ 1], posY[0]), dataField: nameof(ReceiptInput.ReceiptCategoryId)                , readOnly: true , cell: builder.GetTextBoxCell() , visible: false),
                new CellSetting(height    , 150, "CategoryCode"    , border: bdTop, location: new Point(posX[ 1], posY[0]), dataField: nameof(Category.Code)                                 , readOnly: false, tabIndex: cells[0].TabIndex , tabStop: cells[0].IsTabStop , cell: builder.GetTextBoxCell(format: "9", maxLength: 2, ime: ImeMode.Disable) , visible: true ),
                new CellSetting(height    , 150, "CategoryCodeName", border: bdTop, location: new Point(posX[ 1], posY[0]), dataField: nameof(Category.Name)                                 , readOnly: true , tabIndex: cells[0].TabIndex , tabStop: cells[0].IsTabStop , cell: builder.GetTextBoxCell()                                                , visible: false),
                new CellSetting(height    , 350, "Note1"           , border: bdTop, location: new Point(posX[ 2], posY[0]), dataField: nameof(ReceiptInput.Note1)                            , readOnly: false, tabIndex: cells[1].TabIndex , tabStop: cells[1].IsTabStop , cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana) ),
                new CellSetting(height    , 115, "DueAt"           , border: bdTop, location: new Point(posX[ 3], posY[0]), dataField: nameof(ReceiptInput.DueAt)           , enabled: false , readOnly: false, tabIndex: cells[2].TabIndex , tabStop: cells[2].IsTabStop , cell: builder.GetDateCell_yyyyMMdd(isInput: true) ),
                new CellSetting(height    , 200, "ReceiptAmount"   , border: bdTop, location: new Point(posX[ 4], posY[0]), dataField: nameof(ReceiptInput.ReceiptAmount)                    , readOnly: false, tabIndex: cells[3].TabIndex , tabStop: cells[3].IsTabStop , cell: numberCell ),
                new CellSetting(height    ,  30, "MemoSymbol"      , border: bdTop, location: new Point(posX[ 5], posY[0])                                                                   , readOnly: true                               , tabStop: false              , cell: builder.GetTextBoxCell(middleCenter)      ),
                new CellSetting(height    ,  40, "MemoButton"      , border: bdTop, location: new Point(posX[ 6], posY[0])                                                                   , readOnly: true                               , tabStop: false              , cell: builder.GetButtonCell(), value: "メモ"   ),
                new CellSetting(height    ,  40, "ClearButton"     , border: bdTop, location: new Point(posX[ 7], posY[0])                                                                   , readOnly: true                               , tabStop: false              , cell: builder.GetButtonCell(), value: "クリア" ),
                #endregion
                #region データ2
                new CellSetting(height    , 150, "BillNumber"      , border: bdMid, location: new Point(posX[ 8], posY[1]), dataField: nameof(ReceiptInput.BillNumber)      , enabled: false , readOnly: false, tabIndex: cells[4].TabIndex , tabStop: cells[4].IsTabStop , cell: builder.GetTextBoxCell(maxLength:20,format:"NA9") ),
                new CellSetting(height    ,  75, "BillBankCode"    , border: bdMid, location: new Point(posX[ 9], posY[1]), dataField: nameof(ReceiptInput.BillBankCode)    , enabled: false , readOnly: false, tabIndex: cells[5].TabIndex , tabStop: cells[5].IsTabStop , cell: builder.GetTextBoxCell(maxLength:4, format:"9", ime:ImeMode.Disable,align: middleCenter) ),
                new CellSetting(height    ,  75, "BillBranchCode"  , border: bdMid, location: new Point(posX[10], posY[1]), dataField: nameof(ReceiptInput.BillBranchCode)  , enabled: false , readOnly: false, tabIndex: cells[6].TabIndex , tabStop: cells[6].IsTabStop , cell: builder.GetTextBoxCell(maxLength:3, format:"9", ime:ImeMode.Disable,align: middleCenter) ),
                new CellSetting(height    , 200, "BankBranchName"  , border: bdMid, location: new Point(posX[11], posY[1])                                                  , enabled: false , readOnly: true                               , tabStop: false              , cell: builder.GetTextBoxCell(), selectable: false                   ),
                new CellSetting(height    , 115, "BillDrawAt"      , border: bdMid, location: new Point(posX[12], posY[1]), dataField: nameof(ReceiptInput.BillDrawAt)      , enabled: false , readOnly: false, tabIndex: cells[7].TabIndex , tabStop: cells[7].IsTabStop , cell: builder.GetDateCell_yyyyMMdd(isInput: true)                   ),
                new CellSetting(height    , 310, "BillDrawer"      , border: bdMid, location: new Point(posX[13], posY[1]), dataField: nameof(ReceiptInput.BillDrawer)      , enabled: false , readOnly: false, tabIndex: cells[8].TabIndex , tabStop: cells[8].IsTabStop , cell: builder.GetTextBoxCell(maxLength:48, ime: ImeMode.Hiragana)   ),
                #endregion
                #region データ3
                new CellSetting(height    , 300, "Note2"           , border: bdBtm, location: new Point(posX[14], posY[2]), dataField: nameof(ReceiptInput.Note2)                            , readOnly: false, tabIndex: cells[9].TabIndex , tabStop: cells[9].IsTabStop , cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana) ),
                new CellSetting(height    , 315, "Note3"           , border: bdBtm, location: new Point(posX[15], posY[2]), dataField: nameof(ReceiptInput.Note3)                            , readOnly: false, tabIndex: cells[10].TabIndex, tabStop: cells[10].IsTabStop, cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana) ),
                new CellSetting(height    , 310, "Note4"           , border: bdBtm, location: new Point(posX[16], posY[2]), dataField: nameof(ReceiptInput.Note4)                            , readOnly: false, tabIndex: cells[11].TabIndex, tabStop: cells[11].IsTabStop, cell: builder.GetTextBoxCell(maxLength: 100, ime: ImeMode.Hiragana) ),
                new CellSetting(height    ,   0, "UseLimitDate"                                                           , dataField: nameof(Category.UseLimitDate)                         , readOnly: true ),
                new CellSetting(height    ,   0, "Memo"                                                                   , dataField: nameof(ReceiptInput.Memo)                             , readOnly: true ),
                #endregion
            });
            #endregion

            builder.BuildRowOnly(template);
            grdReceiptInput.Template = template;
            grdReceiptInput.RowCount = 5;
            grdReceiptInput.AllowUserToResize = false;
            grdReceiptInput.CurrentCellBorderLine = builder.GetLine(LineStyle.Medium);
            grdReceiptInput.EditMode = EditMode.EditOnEnter;
            grdReceiptInput.HideSelection = false;

            for (var i = 0; i < grdReceiptInput.RowCount; i++)
            {
                CellEnabled(i, false);
            }
        }
        #endregion

        #endregion

        #region header controls event handlers
        private void txtCustomerCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                var customerCode = txtCustomerCode.Text;
                var setPayerName = (IsNewData || UpdateAll);

                if (string.IsNullOrEmpty(customerCode))
                {
                    lblCustomerName.Clear();
                    CustomerId = 0;
                    lblPayerName.Text = setPayerName ? null : lblPayerName.Text;
                    return;
                }

                Customer customer = null;
                if (FromMatchingIndividual)
                {
                    customer = CustomerList?.Find(c => c.Code == customerCode);
                }
                else
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CustomerMasterClient>();
                        CustomersResult result = await service.GetByCodeAsync(
                            SessionKey,
                            CompanyId,
                            new string[] { customerCode });

                        if (result.ProcessResult.Result && result.Customers.Any())
                        {
                            customer = result.Customers.FirstOrDefault();
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                }

                if (customer != null)
                {
                    txtCustomerCode.Text = customer.Code;
                    lblCustomerName.Text = customer.Name;
                    lblPayerName.Text = setPayerName ? customer.Kana : lblPayerName.Text;
                    CustomerId = customer.Id;
                }
                else
                {
                    txtCustomerCode.Clear();
                    lblCustomerName.Clear();
                    lblPayerName.Clear();
                    CustomerId = 0;

                    if (FromMatchingIndividual)
                    {
                        ShowWarningDialog(MsgWngInputCustomerNotExistsAtCustomerGroup, customerCode);
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "得意先", customerCode);
                    }
                    txtCustomerCode.Select();
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
                var sectionCode = txtSectionCode.Text;

                if (string.IsNullOrEmpty(sectionCode)
                    || string.IsNullOrWhiteSpace(sectionCode))
                {
                    lblSectionName.Clear();
                    SectionId = 0;
                    return;
                }

                Section section = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<SectionMasterClient>();
                    SectionsResult result = await service.GetByCodeAsync(
                        SessionKey,
                        CompanyId,
                        new string[] { sectionCode });

                    if (result.ProcessResult.Result && result.Sections.Any())
                    {
                        section = result.Sections.FirstOrDefault();
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
                    SectionId = 0;
                    ShowWarningDialog(MsgWngMasterNotExist, "入金部門", sectionCode);
                    txtSectionCode.Select();
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
                ClearStatusMessage();
                var currencyCode = txtCurrencyCode.Text;

                if (string.IsNullOrEmpty(currencyCode)
                    || string.IsNullOrWhiteSpace(currencyCode))
                {
                    CurrencyId = 0;
                    return;
                }

                Currency currency = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CurrencyMasterClient>();
                    CurrenciesResult result = await service.GetByCodeAsync(
                        SessionKey,
                        CompanyId,
                        new string[] { currencyCode });

                    if (result.ProcessResult.Result && result.Currencies.Any())
                    {
                        currency = result.Currencies.FirstOrDefault();
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (currency != null)
                {
                    SetCurrencyInformation(currency);
                }
                else
                {
                    var code = txtCurrencyCode.Text;
                    txtCurrencyCode.Clear();
                    CurrencyId = 0;
                    txtCurrencyCode.Select();
                    ShowWarningDialog(MsgWngMasterNotExist, "通貨", code);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCustomerCode_Click(object sender, EventArgs e)
        {
            var setPayerName = IsNewData || UpdateAll;
            var loader = new CustomerGridLoader(ApplicationContext);
            if (FromMatchingIndividual)
            {
                loader.Key = CustomerGridLoader.SearchCustomer.Group;
                loader.CustomerId = new int[] { InCustomerId };
            }
            var customer = this.ShowCustomerSearchDialog(loader: loader);
            if (customer != null)
            {
                ClearStatusMessage();
                txtCustomerCode.Text = customer.Code;
                lblCustomerName.Text = customer.Name;
                lblPayerName.Text = setPayerName ? customer.Kana : lblPayerName.Text;
                CustomerId = customer.Id;
            }
        }

        private void btnSectionCode_Click(object sender, EventArgs e)
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

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                SetCurrencyInformation(currency);
            }
        }

        #endregion

        #region grid event handlers

        private void grdReceiptInput_CellValueChanged(object sender, CellEventArgs e)
        {
            Modified = true;
        }
        private void grdReceiptInput_CellContentButtonClick(object sender, CellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            var cellIndex = e.CellIndex;
            var cell = grdReceiptInput.Rows[rowIndex].Cells[cellIndex];
            var columnName = e.CellName;
            Debug.WriteLine("CellContentButtonClick Column Name " + columnName);

            if (columnName == "celClearButton")
                CellContentClear(rowIndex);
            else if (columnName == "celMemoButton")
                ShowReceiptMemoDialog(rowIndex);
        }

        private void grdReceiptInput_CellEnter(object sender, CellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            var cellIndex = e.CellIndex;
            var cell = grdReceiptInput.Rows[rowIndex].Cells[cellIndex];
            var column = cell.DataField;
            Debug.WriteLine("CellEnter Column Name " + column);

            if (ActiveControl != grdReceiptInput)
                return;

            switch (column)
            {
                case nameof(Category.Code):
                    BaseContext.SetFunction09Enabled(true);
                    break;

                case nameof(Category.Name):
                    grdReceiptInput.CurrentCell = grdReceiptInput.Rows[rowIndex].Cells["celCategoryCode"];
                    grdReceiptInput.Rows[rowIndex].Cells["celCategoryCodeName"].Visible = false;
                    BaseContext.SetFunction09Enabled(true);
                    break;

                case nameof(ReceiptInput.BillBankCode):
                case nameof(ReceiptInput.BillBranchCode):
                    BaseContext.SetFunction09Enabled(true);
                    break;

                default:
                    BaseContext.SetFunction09Enabled(false);
                    break;
            }
        }

        private void grdReceiptInput_CellValidating(object sender, CellValidatingEventArgs e)
        {
            grdReceiptInput.EndEdit();

            var rowIndex = e.RowIndex;
            var cellIndex = e.CellIndex;
            var cell = grdReceiptInput.Rows[rowIndex].Cells[cellIndex];
            var column = cell.DataField;
            var value = string.Empty;
            Debug.WriteLine("CellValidating Column Name " + column);

            if (column != nameof(Category.Code))
                return;

            if (cell.EditedFormattedValue == null || cell.EditedFormattedValue == DBNull.Value
                || string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
            {
                ClearCellCategory(rowIndex);
                CellEnabled(rowIndex, false);
                return;
            }

            var cellCategoryId = grdReceiptInput.Rows[rowIndex].Cells["celCategoryId"];
            var cellCategoryCode = grdReceiptInput.Rows[rowIndex].Cells["celCategoryCode"];
            var cellCategoryCodeName = grdReceiptInput.Rows[rowIndex].Cells["celCategoryCodeName"];
            var cellUseLimitDate = grdReceiptInput.Rows[rowIndex].Cells["celUseLimitDate"];

            value = cellCategoryCode.EditedFormattedValue.ToString();
            var categoryCode = value.PadLeft(2, '0');

            Category category = CategoryList
                .Find(x => (x.Code == categoryCode) && string.CompareOrdinal(x.Code, "98") < 0);

            if (category == null)
            {
                ClearCellCategory(rowIndex);
                CellEnabled(rowIndex, false);
                return;
            }

            cellCategoryCodeName.Visible = true;
            cellCategoryId.Value = category.Id;
            cellCategoryCode.Value = category.Code;
            cellCategoryCodeName.Value = category.Code + ":" + category.Name;
            cellUseLimitDate.Value = category.UseLimitDate;

            if (category.UseLimitDate == 1)
                CellEnabled(rowIndex, true);
            else
                CellEnabled(rowIndex, false);
        }

        private void grdReceiptInput_CellValidated(object sender, CellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            var cellIndex = e.CellIndex;
            var cell = grdReceiptInput.Rows[rowIndex].Cells[cellIndex];
            var column = cell.DataField;
            var value = string.Empty;
            Debug.WriteLine("CellValidated Column Name " + column);

            if (column != nameof(ReceiptInput.BillBankCode)
                && column != nameof(ReceiptInput.BillBranchCode))
                return;

            var cellBankCode = grdReceiptInput.Rows[rowIndex].Cells["celBillBankCode"];
            var cellBranchCode = grdReceiptInput.Rows[rowIndex].Cells["celBillBranchCode"];
            var cellBankBranch = grdReceiptInput.Rows[rowIndex].Cells["celBankBranchName"];

            if (cell.EditedFormattedValue == null || cell.EditedFormattedValue == DBNull.Value
                || string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
            {
                cellBankBranch.Value = null;
                return;
            }

            value = cell.EditedFormattedValue.ToString();

            switch (column)
            {
                case nameof(ReceiptInput.BillBankCode):
                    cell.Value = value.PadLeft(4, '0');
                    break;

                case nameof(ReceiptInput.BillBranchCode):
                    cell.Value = value.PadLeft(3, '0');
                    break;

                default:
                    break;
            }

            if (cellBankCode.EditedFormattedValue == null || cellBankCode.EditedFormattedValue == DBNull.Value
                || string.IsNullOrEmpty(cellBankCode.EditedFormattedValue.ToString()))
                return;

            if (cellBranchCode.EditedFormattedValue == null || cellBranchCode.EditedFormattedValue == DBNull.Value
                || string.IsNullOrEmpty(cellBranchCode.EditedFormattedValue.ToString()))
                return;

            cellBankBranch.Value = null;
            var bankCode = cellBankCode.Value.ToString();
            var branchCode = cellBranchCode.Value.ToString();

            BankBranch bankBranch = BankBranchList
                .Find(b => (b.BankCode == bankCode && b.BranchCode == branchCode));

            if (bankBranch == null)
                return;

            cellBankBranch.Value = bankBranch.BankName + " " + bankBranch.BranchName;
        }

        private void grdReceiptInput_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            if (grdReceiptInput.CurrentCell.DataField == nameof(ReceiptInput.ReceiptAmount))
                grdReceiptInput.CellLeave += grdReceiptInput_CellLeave;
            else
                grdReceiptInput.CellLeave -= grdReceiptInput_CellLeave;
        }

        private void grdReceiptInput_CellLeave(object sender, CellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            var cellIndex = e.CellIndex;
            var cell = grdReceiptInput.Rows[rowIndex].Cells[cellIndex];
            var column = cell.DataField;

            if (column != nameof(ReceiptInput.ReceiptAmount))
                return;

            CalculateTotalAmount();
        }

        #endregion

        #region grid helper

        private void CellEnabled(int rowIndex, bool enabled)
        {
            var cellDueAt = grdReceiptInput.Rows[rowIndex].Cells["celDueAt"];
            var cellBillNumber = grdReceiptInput.Rows[rowIndex].Cells["celBillNumber"];
            var cellBillBankCode = grdReceiptInput.Rows[rowIndex].Cells["celBillBankCode"];
            var cellBillBranchCode = grdReceiptInput.Rows[rowIndex].Cells["celBillBranchCode"];
            var cellBankBranchName = grdReceiptInput.Rows[rowIndex].Cells["celBankBranchName"];
            var cellBillDrawAt = grdReceiptInput.Rows[rowIndex].Cells["celBillDrawAt"];
            var cellBillDrawer = grdReceiptInput.Rows[rowIndex].Cells["celBillDrawer"];

            cellDueAt.Enabled = enabled;
            cellBillNumber.Enabled = enabled;
            cellBillBankCode.Enabled = enabled;
            cellBillBranchCode.Enabled = enabled;
            cellBillDrawAt.Enabled = enabled;
            cellBillDrawer.Enabled = enabled;

            cellDueAt.Selectable = enabled;
            cellBillNumber.Selectable = enabled;
            cellBillBankCode.Selectable = enabled;
            cellBillBranchCode.Selectable = enabled;
            cellBillDrawAt.Selectable = enabled;
            cellBillDrawer.Selectable = enabled;

            if (!enabled)
            {
                cellDueAt.Value = null;
                cellBillNumber.Value = null;
                cellBillBankCode.Value = null;
                cellBillBranchCode.Value = null;
                cellBankBranchName.Value = null;
                cellBillDrawAt.Value = null;
                cellBillDrawer.Value = null;
            }
        }

        /// <summary>区分セルクリア処理</summary>
        private void ClearCellCategory(int rowIndex)
        {
            grdReceiptInput.Rows[rowIndex].Cells["celCategoryId"].Value = null;
            grdReceiptInput.Rows[rowIndex].Cells["celCategoryCode"].Value = null;
            grdReceiptInput.Rows[rowIndex].Cells["celCategoryCodeName"].Value = null;
            grdReceiptInput.Rows[rowIndex].Cells["celUseLimitDate"].Value = null;
        }


        #endregion


        #region 通貨情報設定
        private void SetCurrencyInformation(Currency currency)
        {
            if (currency == null) return;

            var sameCurrency = CurrencyId == currency.Id;
            CurrencyId = currency.Id;
            CurrencyPrecision = currency.Precision;
            SetCurrencyDisplayString(CurrencyPrecision);

            if (!sameCurrency)
            {
                ClearStatusMessage();
                InitializeGridTemplate();
                lblTotalAmount.Text = 0.ToString(AmountFormat);
                lblMatchingBillingRemainAmount.Text = 0.ToString(AmountFormat);
                lblMatchingDifferentAmount.Text = 0.ToString(AmountFormat);
            }
        }
        #endregion

        #region 得意先情報取得
        private async Task LoadCustomerInfoAsync()
        {
            var customer = await GetCustomerAsync(InCustomerId);
            if (customer == null) return;
            if (customer.IsParent == 1)
            {
                await LoadCustomerGroupInfoAsync();
                if (CustomerList != null
                    && !CustomerList.Any(x => x.Id == customer.Id))
                    CustomerList.Add(customer);
            }
            else
            {
                CustomerList = new List<Customer> { customer };
            }
        }

        private async Task<Customer> GetCustomerCommonAsync(Func<CustomerMasterClient, Task<Customer>> callback)
            => await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) => await callback(client));

        private async Task<Customer> GetCustomerAsync(int customerId)
            => await GetCustomerCommonAsync(async client =>
            {
                var result = await client.GetAsync(SessionKey, new int[] { customerId });
                return result?.Customers?.FirstOrDefault(x => x != null);
            });

        #endregion

        #region 債権代表者情報取得
        private async Task LoadCustomerGroupInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomersResult result = await service.GetCustomerGroupAsync(
                    SessionKey,
                    CompanyId,
                    InCustomerId);

                if (result.ProcessResult.Result)
                {
                    CustomerList = result.Customers;
                }
            });
        }
        #endregion

        #region 入金部門情報取得
        private async Task LoadSectionInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                SectionsResult result = await service.GetByCodeAsync(SessionKey, CompanyId, Code: null);

                if (result.ProcessResult.Result)
                {
                    SectionList = result.Sections;
                }
            });
        }
        #endregion

        #region 通貨情報取得
        private async Task LoadCurrencyInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                CurrenciesResult result = await service.GetItemsAsync(
                    SessionKey,
                    CompanyId,
                    new CurrencySearch());

                if (result.ProcessResult.Result)
                {
                    CurrencyList = result.Currencies;
                }
            });
        }
        #endregion

        #region 入金区分情報取得
        private async Task LoadCategoryInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CategoryMasterClient>();
                var option = new CategorySearch {
                    CompanyId       = CompanyId,
                    CategoryType    = CategoryType.Receipt,
                    UseInput        = ReceiptUseInput,
                };
                CategoriesResult result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                {
                    CategoryList = result.Categories;
                }
            });
        }
        #endregion

        #region 銀行・支店情報取得
        private async Task LoadBankBranchInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BankBranchMasterClient>();
                BankBranchsResult result = await service.GetItemsAsync(
                    SessionKey,
                    CompanyId, new BankBranchSearch());

                if (result.ProcessResult.Result)
                {
                    BankBranchList = result.BankBranches;
                }
            });
        }
        #endregion

        #region 照合ロジック情報取得
        private async Task LoadCollationSettingInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CollationSettingMasterClient>();
                CollationSettingResult result = await service.GetAsync(
                    SessionKey,
                    CompanyId);

                if (result.ProcessResult.Result)
                {
                    CollationSettingInfo = result.CollationSetting;
                }
            });
        }
        #endregion

        #region 入力設定情報取得
        private async Task LoadInputControlInfoAsync()
        {
            await ServiceProxyFactory.DoAsync(async (InputControlMasterClient client) =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId, Login.UserId, InputGridType.Receipt);
                if (result.ProcessResult.Result)
                    InputControlList = result.InputControls;
                if (!(InputControlList?.Any() ?? false))
                    InputControlList = Enumerable.Range(1, 12).Select(x => new InputControl {
                        TabIndex = x,
                        TabStop = 1,
                    }).ToList();
            });
        }
        #endregion

        #region 項目名称情報取得
        private async Task LoadColumnNameInfoAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ColumnNameSettingMasterClient>();
                ColumnNameSettingsResult result = await service.GetItemsAsync(
                    SessionKey,
                    CompanyId);

                if (result.ProcessResult.Result)
                {
                    ColumnNameList = result.ColumnNames;
                }
            });
        }
        #endregion

        #region 得意先コードフォーマット設定
        private void InitializeCustomerControl(DataExpression expression)
        {
            if (expression == null) return;
            txtCustomerCode.MaxLength = expression.CustomerCodeLength;
            txtCustomerCode.Format = expression.CustomerCodeFormatString;
            txtCustomerCode.ImeMode = expression.CustomerCodeImeMode();
            txtCurrencyCode.PaddingChar = expression.CustomerCodePaddingChar;
        }
        #endregion

        #region 入金部門コード表示制御
        private void InitializeSectionControlFormat(DataExpression expression)
        {
            if (expression == null) return;
            if (UseSection)
            {
                txtSectionCode.MaxLength = expression.SectionCodeLength;
                txtSectionCode.Format = expression.SectionCodeFormatString;
                txtSectionCode.PaddingChar = expression.SectionCodePaddingChar;

                lblSection.Visible = true;
                txtSectionCode.Visible = true;
                btnSectionCode.Visible = true;
                lblSectionName.Visible = true;

            }
            else
            {
                lblSection.Visible = false;
                txtSectionCode.Visible = false;
                btnSectionCode.Visible = false;
                lblSectionName.Visible = false;
            }
        }
        #endregion

        #region 通貨コード表示制御
        private void InitializeCurrencyControl()
        {
            var enabled = (IsNewData && !FromMatchingIndividual);
            var use = UseForeignCurrency;

            SetCurrencyDisplayString(use ? CurrencyPrecision : 0);
            lblCurrency.Visible = use;
            txtCurrencyCode.Visible = use;
            btnCurrencyCode.Visible = use;
            txtCurrencyCode.Enabled = use && enabled;
            btnCurrencyCode.Enabled = use && enabled;
        }
        #endregion

        #region 全体登録処理
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                grdReceiptInput.EndEdit();

                if (!ValidateChildren()) return;

                ClearStatusMessage();

                if (IsNewData || UpdateAll)
                {
                    if (!ValidateInput() || !ValidateGridInput()) return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                SetReceiptInputList(!IsNewData);

                if (!IsNewData
                    && !CheckLatestReceiptInput())
                {
                    return;
                }

                var saveResult = SaveReceiptInput();

                if (!saveResult) return;

                if (FromReceiptSearch
                    || FromMatchingIndividual)
                {
                    ParentForm.DialogResult = DialogResult.OK;
                    return;
                }
                ClearAll();
                DispStatusMessage(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 新規登録処理
        private bool SaveReceiptInput()
        {
            var success = true;
            var syncResult = true;
            var updateValid = true;
            ReceiptInputsResult result = null;

            ProgressDialog.Start(ParentForm, Task.Run(async () =>
            {
                await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
                {
                    result = await client.SaveAsync(
                        SessionKey,
                        ReceiptInputList.ToArray(),
                        InClientKey,
                        InCustomerId);

                    success = result.ProcessResult.Result
                        && (result.ReceiptInputs?.Any(x => x != null) ?? false);

                    if (!success)
                    {
                        updateValid = !(result.ProcessResult.ErrorCode == ErrorCode.OtherUserAlreadyUpdated);
                        return;
                    }

                    if (SavePostProcessor != null)
                    {
                        syncResult = SavePostProcessor.Invoke(
                            result.ReceiptInputs.Select(x => x as ITransactionData));
                    }
                    success &= syncResult;
                });
            }), false, SessionKey);

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
                ShowWarningDialog(MsgErrSaveError);
                return false;
            }

            ReceiptInputList = result.ReceiptInputs;

            if (FromMatchingIndividual)
                OutReceiptId = ReceiptInputList.Select(x => x.Id).ToArray();
            return success;
        }
        #endregion

        #region 検証処理（フォーム）
        private bool ValidateInput()
        {
            if (!datRecordedAt.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "入金日");
                datRecordedAt.Select();
                return false;
            }

            // 一括(個別)消込画面から遷移のみチェック
            // 一括(個別)消込画面で指定した「入金日」の範囲外の場合
            if (FromMatchingIndividual)
            {
                if (InRecordedAtTo.Value < datRecordedAt.Value
                    || (InRecordedAtFrom.HasValue && InRecordedAtFrom.Value > datRecordedAt.Value))
                {
                    ShowWarningDialog(MsgWngInputDateBeforeIndividualMatchingRecordedAt);
                    datRecordedAt.Select();
                    return false;
                }
            }

            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Select();
                return false;
            }

            if (IsNewData || UpdateAll)
            {
                if (string.IsNullOrEmpty(txtCustomerCode.Text))
                {
                    ShowWarningDialog(MsgWngInputRequired, "得意先コード");
                    txtCustomerCode.Select();
                    return false;
                }
            }

            if (UseSection && string.IsNullOrEmpty(txtSectionCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "入金部門コード");
                txtSectionCode.Select();
                return false;
            }

            return true;
        }
        #endregion

        #region 検証処理（グリッド）
        private bool ValidateGridInput()
        {
            var allEmpty = false;
            ActiveControl = grdReceiptInput;
            for (var i = 0; i < grdReceiptInput.RowCount; i++)
            {
                if (ValidateGridEmptyByRow(i))
                {
                    allEmpty = true;
                }
                else
                {
                    allEmpty = false;
                    break;
                }
            }

            if (allEmpty)
            {
                ShowWarningDialog(MsgWngInputGridRequired);
                return false;
            }

            for (var i = 0; i < grdReceiptInput.RowCount; i++)
            {
                if (ValidateGridEmptyByRow(i))
                    continue;

                var cellCategoryCode = grdReceiptInput.Rows[i].Cells["celCategoryCode"];
                var cellUseLimitDate = grdReceiptInput.Rows[i].Cells["celUseLimitDate"];
                var cellReceiptAmount = grdReceiptInput.Rows[i].Cells["celReceiptAmount"];
                var cellDueAt = grdReceiptInput.Rows[i].Cells["celDueAt"];
                var cellBillBankCode = grdReceiptInput.Rows[i].Cells["celBillBankCode"];
                var cellBillBranchCode = grdReceiptInput.Rows[i].Cells["celBillBranchCode"];
                var cellBankBranchName = grdReceiptInput.Rows[i].Cells["celBankBranchName"];
                var cellBillDrawAt = grdReceiptInput.Rows[i].Cells["celBillDrawAt"];

                string categoryCode = ValidateGridEmptyByCell(cellCategoryCode) ? string.Empty : cellCategoryCode.Value.ToString();
                var receiptAmount = (decimal?)cellReceiptAmount.EditedFormattedValue;
                var dueAt = (DateTime?)cellDueAt.EditedFormattedValue;
                var billBankCode = (string)cellBillBankCode.EditedFormattedValue;
                var billBranchCode = (string)cellBillBranchCode.EditedFormattedValue;
                var bankBranchName = (string)cellBankBranchName.EditedFormattedValue;
                var billDrawAt = (DateTime?)cellBillDrawAt.EditedFormattedValue;

                var requireDueAt = Convert.ToString(cellUseLimitDate.EditedFormattedValue) == "1";

                if (string.IsNullOrEmpty(categoryCode))
                {
                    ShowWarningDialog(MsgWngInputRequired, "入金区分");
                    grdReceiptInput.CurrentCell = cellCategoryCode;
                    return false;
                }

                if (requireDueAt
                    && !dueAt.HasValue)
                {
                    ShowWarningDialog(MsgWngInputRequired, "期日");
                    grdReceiptInput.CurrentCell = cellDueAt;
                    return false;
                }

                if (!receiptAmount.HasValue)
                {
                    ShowWarningDialog(MsgWngInputRequired, "金額");
                    grdReceiptInput.CurrentCell = cellReceiptAmount;
                    return false;
                }

                if (receiptAmount == 0M)
                {
                    ShowWarningDialog(MsgWngInputExceptZeroAmt, "金額");
                    grdReceiptInput.CurrentCell = cellReceiptAmount;
                    return false;
                }

                // 銀行コード・支店コード
                // 銀行コード 支店コード 何れかに入力があり、マスターに存在しない場合
                if (cellBillBankCode.Enabled
                    && cellBillBranchCode.Enabled
                    && (!string.IsNullOrEmpty(billBankCode)
                        || !string.IsNullOrEmpty(billBranchCode))
                        && string.IsNullOrEmpty(bankBranchName))
                {
                    ShowWarningDialog(MsgWngInputDataNotExistsAtMaster, "銀行・支店", "銀行・支店コード");
                    grdReceiptInput.CurrentCell = cellBillBankCode;
                    return false;
                }

                if (dueAt.HasValue
                    && dueAt.Value < datRecordedAt.Value)
                {
                    ShowWarningDialog(MsgWngInputParam1DateAfterParam2Date, "期日", "入金日");
                    grdReceiptInput.CurrentCell = cellDueAt;
                    return false;
                }

                if (billDrawAt.HasValue
                    && datRecordedAt.Value < billDrawAt.Value)
                {
                    ShowWarningDialog(MsgWngInputParam1DateBeforeParam2Date, "振出日", "入金日");
                    grdReceiptInput.CurrentCell = cellBillDrawAt;
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region グリッド・行ごとのチェック
        private bool ValidateGridEmptyByRow(int rowIndex)
        {
            var isEmpty = false;

            for (var i = 1; i < grdReceiptInput.Rows[rowIndex].Cells.Count; i++)
            {
                var cell = grdReceiptInput.Rows[rowIndex].Cells[i];

                if (cell.Name.Equals("celMemoButton") || cell.Name.Equals("celClearButton"))
                    continue;

                if (ValidateGridEmptyByCell(cell))
                {
                    isEmpty = true;
                }
                else
                {
                    isEmpty = false;
                    break;
                }
            }
            return isEmpty;
        }
        #endregion

        #region グリッド・セルごとのチェック
        private bool ValidateGridEmptyByCell(Cell cell)
            => cell.Value == null
            || cell.Value == DBNull.Value
            || string.IsNullOrEmpty(Convert.ToString(cell.Value));
        #endregion

        #region 入金データ状態確認処理（更新・削除のみ）
        /// <summary>更新直前のDB確認処理</summary>
        /// <param name="isUpdate">true:更新処理, false :削除処理</param>
        /// <returns></returns>
        private bool CheckLatestReceiptInput(bool isUpdate = true)
        {
            var mode = isUpdate ? UpdateReceipt : DeleteReceipt;
            var receiptId = ReceiptInputList[0].Id;

            Receipt receipt = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptServiceClient>();
                ReceiptsResult result = await service.GetAsync(
                    SessionKey,
                    new long[] { receiptId });

                if (result.ProcessResult.Result && result.Receipts.Count > 0)
                {
                    receipt = result.Receipts.FirstOrDefault();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (receipt == null)
            {
                ShowWarningDialog(MsgWngInvalidReceiptIdForReceipt, receiptId.ToString());
                return false;
            }

            if (receipt.AssignmentFlag > 0)
            {
                ShowWarningDialog(MsgWngMatchedDataCannotUpdateOrDelete, mode);
                return false;
            }

            if (receipt.OutputAt.HasValue)
            {
                ShowWarningDialog(MsgWngJournalizedCannotUpdateAndDelete, mode);
                return false;
            }

            if (receipt.OriginalReceiptId.HasValue)
            {
                ShowWarningDialog(MsgWngAdvancedReceiptDataCannotxxx, mode);
                return false;
            }

            if (receipt.DeleteAt.HasValue)
            {
                ShowWarningDialog(MsgWngDeletedDataCannotUpdateOrDelete, mode);
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

            if (Modified
                && !ShowConfirmDialog(MsgQstConfirmClear))
            {
                return;
            }

            ClearAll();
        }
        #endregion

        #region 全項目の初期化
        private void ClearAll()
        {
            ClearStatusMessage();

            if (!IsNewData && !FromOtherForm)
            {
                CurrentReceipt = null;
            }
            SetReceiptHeader();

            InitializeGridTemplate();
            ReceiptInputList.Clear();

            CalculateTotalAmount();

            lblReceiptId.Clear();

            datRecordedAt.Value = DateTime.Today;
            lblMatchingRecordedAt.Text = string.Empty;

            lblPayerName.Clear();

            CustomerId = 0;
            txtCustomerCode.Clear();
            lblCustomerName.Clear();
            txtCustomerCode.Enabled = true;
            btnCustomerCode.Enabled = true;

            SectionId = 0;
            txtSectionCode.Clear();
            lblSectionName.Clear();
            txtSectionCode.Enabled = true;
            btnSectionCode.Enabled = true;

            CurrencyId = 0;
            txtCurrencyCode.Clear();
            txtCurrencyCode.Enabled = true;
            btnCurrencyCode.Enabled = true;

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            ActiveControl = datRecordedAt;
            Modified = false;
        }
        #endregion

        #region セル内容クリア
        private void CellContentClear(int rowIndex)
        {
            CellEnabled(rowIndex, false);

            for (var i = 1; i < grdReceiptInput.Rows[rowIndex].Cells.Count; i++)
            {
                var name = grdReceiptInput.Rows[rowIndex].Cells[i].Name;
                if (!name.Equals("celMemoButton") && !name.Equals("celClearButton"))
                    grdReceiptInput.Rows[rowIndex].Cells[i].Value = null;
            }

            CalculateTotalAmount();

            grdReceiptInput.ClearSelection();
            grdReceiptInput.CurrentCell = grdReceiptInput.Rows[rowIndex].Cells["celCategoryCode"];
        }
        #endregion

        #region 削除処理
        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                if (!ReceiptInputList.Any())
                    SetReceiptInputList();

                if (!CheckLatestReceiptInput(isUpdate: false))
                    return;

                var success = true;
                var syncResult = true;
                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) =>
                    {
                        var deleteResult = await client.DeleteAsync(SessionKey, ReceiptInputList[0].Id);

                        success = deleteResult.ProcessResult.Result && deleteResult.Count > 0;

                        if (success && DeletePostProcessor != null)
                        {
                            syncResult = DeletePostProcessor.Invoke(new[] { ReceiptInputList[0] as ITransactionData });
                        }
                        success &= syncResult;
                    });
                }), false, SessionKey);

                if (!syncResult)
                {
                    ShowWarningDialog(MsgErrPostProcessFailure);
                    return;
                }

                if (!success)
                {
                    ShowWarningDialog(MsgErrDeleteError);
                    return;
                }

                if (FromReceiptSearch || FromMatchingIndividual)
                {
                    IsDeleted = true;
                    if (FromMatchingIndividual)
                        OutReceiptId = ReceiptInputList.Select(x => x.Id).ToArray();
                    ParentForm.DialogResult = DialogResult.OK;
                    return;
                }

                ClearAll();
                DispStatusMessage(MsgInfDeleteSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 合計計算
        private void CalculateTotalAmount()
        {
            var totalAmount = 0M;
            for (var i = 0; i < grdReceiptInput.RowCount; i++)
            {
                var amount = (decimal?)grdReceiptInput.GetEditedFormattedValue(i, "celReceiptAmount");
                totalAmount += (amount ?? 0M);
            }
            lblTotalAmount.Text = totalAmount.ToString(AmountFormat);

            if (lblMatchingBillingRemainAmount.Visible)
            {
                var remainAmount = InRemainAmount;
                var difAmount = remainAmount - totalAmount;
                lblTotalAmount.Text = totalAmount.ToString(AmountFormat);
                lblMatchingBillingRemainAmount.Text = remainAmount.ToString(AmountFormat);
                lblMatchingDifferentAmount.Text = difAmount.ToString(AmountFormat);
            }
        }
        #endregion

        #region 金額表示フォーマット設定
        #endregion
        private void SetCurrencyDisplayString(int displayScale)
        {
            var displayFieldString = "#,###,###,###,##0";
            var displayFormatString = "0";

            if (displayScale > 0)
            {
                displayFieldString += ".";
                for (var i = 0; i < displayScale; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }

            AmountFormat = displayFieldString;
        }

        #region 請求データ検索画面へ遷移
        #endregion
        #region 入力設定画面へ遷移
        #endregion
        #region 入金データ検索画面へ遷移
        #endregion
        #region コード検索


        [OperationLog("請求内訳")]
        private void CallBillingSearch()
        {
            using (var form = ApplicationContext.Create(nameof(PC0301)))
            {
                PC0301 screen = form.GetAll<PC0301>().FirstOrDefault();

                if (screen != null)
                {
                    screen.ReturnScreen = this;
                    screen.CustomerCode = txtCustomerCode.Text;
                    screen.CurrencyCode = txtCurrencyCode.Text;
                    form.StartPosition = FormStartPosition.CenterParent;
                    ApplicationContext.ShowDialog(ParentForm, form);
                }
            }
        }
        [OperationLog("入力設定")]
        private void CallInputControlMaster()
        {
            using (var form = ApplicationContext.Create(nameof(PH9903)))
            {
                var screen = form.GetAll<PH9903>().First();
                screen.FormName = nameof(PD0301);
                screen.InitializeParentForm("入力設定");
                var result = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (result == DialogResult.OK)
                {
                    InputControlList = screen.InputControlList;
                    ClearStatusMessage();
                    InitializeGridTemplate();
                    CalculateTotalAmount();
                }
            }
        }

        [OperationLog("入金検索")]
        private void CallReceiptSearch()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            using (var form = ApplicationContext.Create(nameof(PD0501)))
            {
                var screen = form.GetAll<PD0501>().First();
                screen.ReturnScreen = this;
                form.StartPosition = FormStartPosition.CenterParent;
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form);

                if (dialogResult != DialogResult.OK) return;

                CurrentReceipt = screen.CurrentReceipt;
                PD0301_Load(this, EventArgs.Empty);
                Debug.WriteLine("CurrentReceipt Id is " + CurrentReceipt.Id);
            }
        }

        [OperationLog("検索")]
        private void CallNameSearch()
        {
            var rowIndex = grdReceiptInput.CurrentCell.RowIndex;
            var field = grdReceiptInput.CurrentCell.DataField;

            switch (field)
            {
                case nameof(Category.Code):
                    ShowCategorySearchDialog(rowIndex);
                    break;

                case nameof(ReceiptInput.BillBankCode):
                case nameof(ReceiptInput.BillBranchCode):
                    ShowBankBranchSearchDialog(rowIndex);
                    break;

                default:
                    break;
            }

            ActiveControl = grdReceiptInput;
        }
        #endregion

        #region 入金データ入力終了
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified)
            {
                var exit = ShowConfirmDialog(MsgQstConfirmClose);
                if (!exit)
                    return;
            }
            BaseForm.Close();
        }
        #endregion

        #region 入金データ検索表示設定
        /// <summary>
        /// マスターの値セット
        /// グリッドの初期化
        /// Receipt の値設定
        /// 3つの処理を分ける
        /// </summary>
        /// <returns></returns>
        public void SetReceiptInputForm()
        {
            lblReceiptId.Text = CurrentReceipt.Id.ToString();
            lblPayerName.Text = CurrentReceipt.PayerName;
            datRecordedAt.Value = CurrentReceipt.RecordedAt;

            var currentCurrencyId = CurrentReceipt.CurrencyId;
            SetCurrencyInput(currentCurrencyId, string.Empty);

            var currentCustomerId = CurrentReceipt.CustomerId ?? 0;
            SetCustomerInput(currentCustomerId);

            var currentSectionId = CurrentReceipt.SectionId ?? 0;
            SetSectionInput(currentSectionId, string.Empty);

            InitializeGridTemplate();

            var rowIndex = 0;
            var currentCategoryId = CurrentReceipt.ReceiptCategoryId;
            SetCategoryInputEnabled(currentCategoryId, string.Empty, rowIndex);

            var currentBankCode = CurrentReceipt.BillBankCode;
            var currentBranchCode = CurrentReceipt.BillBranchCode;
            SetBankBranchInputEnabled(currentBankCode, currentBranchCode, rowIndex);

            grdReceiptInput.Rows[rowIndex].Cells["celNote1"].Value = CurrentReceipt.Note1;
            grdReceiptInput.Rows[rowIndex].Cells["celDueAt"].Value = CurrentReceipt.DueAt;
            grdReceiptInput.Rows[rowIndex].Cells["celReceiptAmount"].Value = CurrentReceipt.ReceiptAmount;
            grdReceiptInput.Rows[rowIndex].Cells["celMemo"].Value = CurrentReceipt.ReceiptMemo;
            grdReceiptInput.Rows[rowIndex].Cells["celMemoSymbol"].Value =
                !string.IsNullOrEmpty(CurrentReceipt.ReceiptMemo) ? "○" : "";
            grdReceiptInput.Rows[rowIndex].Cells["celBillNumber"].Value = CurrentReceipt.BillNumber;
            grdReceiptInput.Rows[rowIndex].Cells["celBillBankCode"].Value = CurrentReceipt.BillBankCode;
            grdReceiptInput.Rows[rowIndex].Cells["celBillBranchCode"].Value = CurrentReceipt.BillBranchCode;
            grdReceiptInput.Rows[rowIndex].Cells["celBillDrawAt"].Value = CurrentReceipt.BillDrawAt;
            grdReceiptInput.Rows[rowIndex].Cells["celBillDrawer"].Value = CurrentReceipt.BillDrawer;
            grdReceiptInput.Rows[rowIndex].Cells["celNote2"].Value = CurrentReceipt.Note2;
            grdReceiptInput.Rows[rowIndex].Cells["celNote3"].Value = CurrentReceipt.Note3;
            grdReceiptInput.Rows[rowIndex].Cells["celNote4"].Value = CurrentReceipt.Note4;
        }
        #endregion

        #region 得意先コード・名称・振込依頼人名
        private Customer CustomerBuff { get; set; }

        private async Task LoadCustomer(int customerId)
        {
            CustomerBuff = await GetCustomerAsync(customerId);
        }

        private void SetCustomerInput(int id)
        {
            CustomerBuff = null;
            if (id != 0)
            {
                CustomerBuff = null;
                var load = new List<Task>();
                load.Add(LoadCustomer(id));
                ProgressDialog.Start(ParentForm, Task.WhenAll(load), false, SessionKey);
            }

            var customer = CustomerBuff;

            if (customer == null)
                return;

            CustomerId = customer.Id;
            txtCustomerCode.Text = customer.Code;
            lblCustomerName.Text = customer.Name;
            lblPayerName.Text = UpdateAll ? customer.Kana : lblPayerName.Text;
        }
        #endregion

        #region 入金部門コード・名称
        private void SetSectionInput(int id, string code)
        {
            if (id != 0 || !string.IsNullOrEmpty(code))
            {
                Section section = SectionList
                    .Find(s => (s.Id == id) || (s.Code == code));

                if (section == null)
                    return;

                SectionId = section.Id;
                txtSectionCode.Text = section.Code;
                lblSectionName.Text = section.Name;
            }
        }
        #endregion

        #region 通貨コード・名称
        private void SetCurrencyInput(int id, string code)
        {
            if (id != 0 || !string.IsNullOrEmpty(code))
            {
                Currency currency = CurrencyList
                    .Find(c => (c.Id == id) || (c.Code == code));

                if (currency == null)
                    return;

                CurrencyId = currency.Id;
                txtCurrencyCode.Text = currency.Code;
                CurrencyPrecision = currency.Precision;
                SetCurrencyDisplayString(CurrencyPrecision);
            }
        }
        #endregion

        #region 区分コード・名称（活性・非活性制御）
        private void SetCategoryInputEnabled(int id, string code, int rowIndex)
        {
            if (id != 0 || !string.IsNullOrEmpty(code))
            {
                Category category = CategoryList
                    .Find(c => (c.Id == id) || (c.Code == code));

                if (category == null)
                    return;

                var categoryId = category.Id;
                var categoryCode = category.Code;
                var categoryName = category.Name;
                grdReceiptInput.Rows[rowIndex].Cells["celCategoryId"].Value = categoryId;
                grdReceiptInput.Rows[rowIndex].Cells["celCategoryCode"].Value = categoryCode;
                grdReceiptInput.Rows[rowIndex].Cells["celCategoryCodeName"].Value = categoryCode + ":" + categoryName;
                grdReceiptInput.Rows[rowIndex].Cells["celUseLimitDate"].Value = category.UseLimitDate;
                grdReceiptInput.Rows[rowIndex].Cells["celCategoryCodeName"].Visible = true;

                if (category.UseLimitDate == 1)
                    CellEnabled(rowIndex, true);
            }
        }
        #endregion

        #region 銀行・支店コード・名称（活性・非活性制御）
        private void SetBankBranchInputEnabled(string bankCode, string branchCode, int rowIndex)
        {
            if (!string.IsNullOrEmpty(bankCode) && !string.IsNullOrEmpty(branchCode))
            {
                BankBranch bankBranch = BankBranchList
                    .Find(b => (b.BankCode == bankCode
                    && b.BranchCode == branchCode));

                if (bankBranch == null)
                    return;

                grdReceiptInput.Rows[rowIndex].Cells["celBankBranchName"].Value
                    = bankBranch.BankName + " " + bankBranch.BranchName;
            }
        }
        #endregion

        #region 登録・更新リストの設定
        private void SetReceiptInputList(bool updateMode = false)
        {
            for (var i = 0; i < grdReceiptInput.RowCount; i++)
            {
                if (ValidateGridEmptyByRow(i))
                    continue;

                var receipt = SetReceiptInput(grdReceiptInput.Rows[i], updateMode);
                ReceiptInputList.Add(receipt);
            }
        }
        #endregion

        /// <summary>登録用 <see cref="ReceiptInput"/>に値を設定する処理</summary>
        private ReceiptInput SetReceiptInput(Row row, bool updateMode = false)
        {
            var cellCategoryId = row.Cells["celCategoryId"];
            var cellReceiptAmount = row.Cells["celReceiptAmount"];
            var cellDueAt = row.Cells["celDueAt"];
            var cellNote1 = row.Cells["celNote1"];
            var cellNote2 = row.Cells["celNote2"];
            var cellNote3 = row.Cells["celNote3"];
            var cellNote4 = row.Cells["celNote4"];
            var cellBillNumber = row.Cells["celBillNumber"];
            var cellBillBankCode = row.Cells["celBillBankCode"];
            var cellBillBranchCode = row.Cells["celBillBranchCode"];
            var cellBillDrawAt = row.Cells["celBillDrawAt"];
            var cellBillDrawer = row.Cells["celBillDrawer"];
            var cellMemo = row.Cells["celMemo"];

            int categoryId = ValidateGridEmptyByCell(cellCategoryId) ? 0 : Convert.ToInt32(cellCategoryId.Value);
            decimal receiptAmount = ValidateGridEmptyByCell(cellReceiptAmount) ? 0 : Convert.ToDecimal(cellReceiptAmount.Value);
            string note1 = ValidateGridEmptyByCell(cellNote1) ? string.Empty : cellNote1.Value.ToString().Trim();
            string note2 = ValidateGridEmptyByCell(cellNote2) ? string.Empty : cellNote2.Value.ToString().Trim();
            string note3 = ValidateGridEmptyByCell(cellNote3) ? string.Empty : cellNote3.Value.ToString().Trim();
            string note4 = ValidateGridEmptyByCell(cellNote4) ? string.Empty : cellNote4.Value.ToString().Trim();
            string billNumber = ValidateGridEmptyByCell(cellBillNumber) ? string.Empty : cellBillNumber.Value.ToString();
            string billBankCode = ValidateGridEmptyByCell(cellBillBankCode) ? string.Empty : cellBillBankCode.Value.ToString();
            string billBranchCode = ValidateGridEmptyByCell(cellBillBranchCode) ? string.Empty : cellBillBranchCode.Value.ToString();
            string billDrawer = ValidateGridEmptyByCell(cellBillDrawer) ? string.Empty : cellBillDrawer.Value.ToString();
            string memo = ValidateGridEmptyByCell(cellMemo) ? string.Empty : cellMemo.Value.ToString();

            var dueAt = (DateTime?)cellDueAt.EditedFormattedValue;

            var billDrawAt = (DateTime?)cellBillDrawAt.EditedFormattedValue;

            long receiptId = CurrentReceipt?.Id ?? 0L;
            var recordedAt = datRecordedAt.Value.Value;
            string payerName = !string.IsNullOrEmpty(lblPayerName.Text) ? lblPayerName.Text : string.Empty;

            int? saveCustomerId = null;
            if (CustomerId != 0)
                saveCustomerId = CustomerId;

            int? saveSectionId = null;
            if (SectionId != 0)
                saveSectionId = SectionId;

            var isEbFile = (!IsNewData && CurrentReceipt?.InputType == (int)Constants.ReceiptInputType.EbFile);
            var jpyId = CurrencyList.Exists(x => x.Code == "JPY")
                ? CurrencyList.Find(x => x.Code == "JPY").Id
                : 0;

            var receiptInput = new ReceiptInput();
            receiptInput.Id = receiptId;
            receiptInput.CompanyId = CompanyId;
            receiptInput.CurrencyId = CurrencyId != 0 ? CurrencyId : jpyId;
            receiptInput.ReceiptHeaderId = null;
            receiptInput.ReceiptCategoryId = categoryId;
            receiptInput.CustomerId = saveCustomerId;
            receiptInput.SectionId = saveSectionId;
            receiptInput.InputType = updateMode ? CurrentReceipt.InputType : ReceiptInputType;
            receiptInput.Apportioned = 1;
            receiptInput.Approved = 1;
            receiptInput.RecordedAt = recordedAt;
            receiptInput.OriginalRecordedAt = null;
            receiptInput.ReceiptAmount = receiptAmount;
            receiptInput.AssignmentAmount = 0;
            receiptInput.RemainAmount = receiptAmount;
            receiptInput.AssignmentFlag = 0;
            receiptInput.PayerCode = isEbFile ? CurrentReceipt.PayerCode : "";
            receiptInput.PayerName = isEbFile ? CurrentReceipt.PayerName : payerName;
            receiptInput.PayerNameRaw = isEbFile ? CurrentReceipt.PayerNameRaw : payerName;
            receiptInput.SourceBankName = isEbFile ? CurrentReceipt.SourceBankName : "";
            receiptInput.SourceBranchName = isEbFile ? CurrentReceipt.SourceBranchName : "";
            receiptInput.OutputAt = null;
            receiptInput.DueAt = dueAt;
            receiptInput.MailedAt = null;
            receiptInput.OriginalReceiptId = null;
            receiptInput.ExcludeFlag = 0;
            receiptInput.ExcludeCategoryId = null;
            receiptInput.ExcludeAmount = 0;
            receiptInput.ReferenceNumber = "";
            receiptInput.RecordNumber = "";
            receiptInput.DensaiRegisterAt = null;
            receiptInput.Note1 = note1;
            receiptInput.Note2 = note2;
            receiptInput.Note3 = note3;
            receiptInput.Note4 = note4;
            receiptInput.BillNumber = billNumber;
            receiptInput.BillBankCode = billBankCode;
            receiptInput.BillBranchCode = billBranchCode;
            receiptInput.BillDrawAt = billDrawAt;
            receiptInput.BillDrawer = billDrawer;
            receiptInput.DeleteAt = null;
            receiptInput.CreateBy = Login.UserId;
            receiptInput.UpdateBy = Login.UserId;
            receiptInput.Memo = memo;
            receiptInput.LearnKanaHistory =
                cbxSaveKanaHistory.Visible && cbxSaveKanaHistory.Checked;
            if (updateMode)
            {
                receiptInput.UpdateAt = CurrentReceipt.UpdateAt;
            }

            return receiptInput;
        }

        #region show dialog code search / memo input

        private void ShowCategorySearchDialog(int rowIndex)
        {
            var category = this.ShowReceiptCategorySearchDialog(search: new CategorySearch
            {
                CompanyId = CompanyId,
                CategoryType = CategoryType.Receipt,
                SearchPredicate = IsUsableReceiptCategory
            });
            if (category == null) return;
            ClearStatusMessage();
            var cellCategoryId = grdReceiptInput.Rows[rowIndex].Cells["celCategoryId"];
            var cellCategoryCode = grdReceiptInput.Rows[rowIndex].Cells["celCategoryCode"];
            var cellCategoryCodeName = grdReceiptInput.Rows[rowIndex].Cells["celCategoryCodeName"];
            var cellUseLimitDate = grdReceiptInput.Rows[rowIndex].Cells["celUseLimitDate"];

            cellCategoryCode.Visible = true;
            cellCategoryCodeName.Visible = false;
            cellCategoryId.Value = category.Id;
            cellCategoryCode.Value = category.Code;
            cellCategoryCodeName.Value = category.Code + ":" + category.Name;

            CellEnabled(rowIndex, category.UseLimitDate == 1);
        }

        private bool IsUsableReceiptCategory(Category category)
            => string.CompareOrdinal(category.Code, "98") < 0;

        private void ShowBankBranchSearchDialog(int rowIndex)
        {
            var bankBranch = this.ShowBankBranchSearchDialog();
            if (bankBranch != null)
            {
                ClearStatusMessage();
                grdReceiptInput.Rows[rowIndex].Cells["celBillBankCode"].Value = bankBranch.BankCode;
                grdReceiptInput.Rows[rowIndex].Cells["celBillBranchCode"].Value = bankBranch.BranchCode;
                grdReceiptInput.Rows[rowIndex].Cells["celBankBranchName"].Value =
                    bankBranch.BankName + " " + bankBranch.BranchName;
            }
        }

        private void ShowReceiptMemoDialog(int rowIndex)
        {
            string memo = "";

            var memoCell = grdReceiptInput.Rows[rowIndex].Cells["celMemo"];

            if (memoCell.Value != null)
                memo = memoCell.Value.ToString();

            using (var form = ApplicationContext.Create(nameof(PH9906)))
            {
                var screen = form.GetAll<PH9906>().First();
                screen.MemoType = MemoType.ReceiptMemo;
                screen.Memo = memo;
                screen.InitializeParentForm("入金メモ");
                var memoDialog = ApplicationContext.ShowDialog(ParentForm, form, true);
                if (memoDialog == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(screen.Memo))
                    {
                        memoCell.Value = screen.Memo;
                        grdReceiptInput.Rows[rowIndex].Cells["celMemoSymbol"].Value = "○";
                    }
                    else
                    {
                        memoCell.Value = null;
                        grdReceiptInput.Rows[rowIndex].Cells["celMemoSymbol"].Value = "";
                    }
                }
            }
        }

        #endregion

    }
}
