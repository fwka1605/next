using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CollationSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerFeeMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.MatchingService;
using Rac.VOne.Client.Screen.PaymentAgencyFeeMasterService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using Section = Rac.VOne.Web.Models.Section;

namespace Rac.VOne.Client.Screen
{
    /// <summary>一括消込</summary>
    public partial class PE0101 : VOneScreenBase
    {
        #region third party usable properties

        public Func<IEnumerable<ITransactionData>, bool> MatchingPostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> CancelPostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> DeletePostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> NettingPostProcessor { get; set; }
        /// <summary>
        /// 一括消込/消込取消 の キャンセル許可 初期値 true
        /// </summary>
        public virtual bool Cancellable { get; set; } = true;

        private bool IsPostProcessorImplemanted
        {
            get
            {
                return MatchingPostProcessor != null
                    || CancelPostProcessor   != null
                    || DeletePostProcessor   != null;
            }
        }

        #endregion

        /// <summary>入金予定日 算出用 現在日付 + N 日の値 管理マスター：回収予定範囲 の値</summary>
        private int BillingDueAtOffset { get; set; }
        /// <summary>手数料誤差範囲 通貨 JPY 日本円の場合に 管理マスター：手数料誤差 の値を利用</summary>
        private decimal BankFeeTolerance { get; set; }
        /// <summary>消費税誤差 一括消込時の許容誤差。入金額±誤差まで消込可</summary>
        private decimal TaxDifferenceTolerance { get; set; }

        private List<int> SectionIds { get; set; }
        private List<Section> Sections { get; set; }
        private List<Section> SectionsWithLoginUser { get; set; }
        private List<int> DepartmentIds { get; set; }
        private List<Department> Departments { get; set; }
        private List<Department> DepartmentsWithLoginUser { get; set; }

        private int? CurrencyId { get; set; }
        private int Precision { get; set; }

        /// <summary>照合設定</summary>
        private CollationSetting CollationSetting { get; set; }
        private byte[] ClientKey { get; set; }
        private CollationSearch CollationSearch { get; set; } = new CollationSearch();

        /// <summary>小数点以下桁数</summary>
        private int FormatNumber { get; set; }
        private List<int> MatchedIndex { get; set; } = new List<int>();

        /// <summary>照合後、自動的に消込オプション有効時の対応</summary>
        /// <remarks>
        /// 一括消込画面で、消込完了が一度でも行われたら、trueにする
        /// 照合後、自動消込の機能が有効な場合、消込解除の後、照合処理を実行すると、再度消込れる現象の回避用
        /// </remarks>
        private bool MatchingCancelExecuted { get; set; }

        /// <summary>消込完了データを表示</summary>
        private bool IsMatched { get { return cbxShowMatched.Checked; } }
        private List<object> SearchCondition { get; set; }
        private IEnumerable<string> LegalPersonalities { get; set; }
        private List<MatchingOrder> MatchingBillingOrder { get; set; } = new List<MatchingOrder>();
        private List<MatchingOrder> MatchingReceiptOrder { get; set; } = new List<MatchingOrder>();

        private enum eSortType
        {
            None = 0,
            CheckBox,
            AdvanceReceive_ASC,
            AdvanceReceive_DESC
        }

        private eSortType sortType { get; set; } = eSortType.None;

        /// <summary>登録済 得意先手数料 key CustomerId</summary>
        private SortedList<int, CustomerFee[]> CustomerFees { get; set; }
        /// <summary>登録済 決済代行会社手数料 key PaymentAgencyId</summary>
        private SortedList<int, PaymentAgencyFee[]> PaymentAgencyFees { get; set; }
        private Color MatchingGridBillingBackColor { get; set; }
        private Color MatchingGridReceiptBackColor { get; set; }
        private Color ControlDisableBackColor { get; set; }
        private Color GridLineColor { get; set; }
        private int GridDefaultWidth;
        private int nameWidth { get { return UseForeignCurrency ? 125 : 145; } }
        private bool BillingPriority = true;

      
        private bool BillingReceiptOrder { get { return CollationSetting.BillingReceiptDisplayOrder == 0; } }
        private TaskProgressManager ProgressManager { get; set; }
        private System.Action OnCancelHandler { get; set; }

        #region initialize

        public PE0101()
        {
            InitializeComponent();
            grdCollation.SetupShortcutKeys();
            Text = "一括消込";

            datReceiptRecordedAtTo.Value = DateTime.Today;
            GridDefaultWidth = grdCollation.Width;

            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcMain.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcMain.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = ExitForm;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
            pnlCreateAt.EnabledChanged += (sender, e) => {
                var pnl = sender as Panel;
                if (pnl == null) return;
                if (CollationSetting?.SetSystemDateToCreateAtFilter == 1 && (pnl?.Enabled ?? false))
                {
                    datCreateAtFrom.Value = DateTime.Today;
                    datCreateAtTo.Value = GetLastTimeOfADay(DateTime.Today);
                }
                else
                {
                    datCreateAtFrom.Clear();
                    datCreateAtTo.Clear();
                }

            };
        }
        private void InitializeUserComponent()
        {
            grdCollation.CellFormatting += grid_CellFormatting;
        }

        #region グリッド作成

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);

            if (UseForeignCurrency)
            {
                nmbDisplayPrecision.Value = FormatNumber;
                Precision = FormatNumber;

                if (FormatNumber == 5)
                {
                    btnIncrease.Enabled = false;
                }

                if (FormatNumber == 0)
                {
                    btnDecrease.Enabled = false;
                }
            }

            var height = builder.DefaultRowHeight;
            var template = new Template();

            var memoField = (IsMatched) ? "DispMemo" : "DispAdvanceReceivedCount";
            var memoDataField = (IsMatched) ? "Memo" : "AdvanceReceivedCount";
            var memoDisplayCaption = (IsMatched) ? "メモ" : "前受";
            var checkBoxCaption = (IsMatched) ? "解除" : "一括";
            var billingWidth = UseForeignCurrency ? BillingReceiptOrder ? 420 : 380 : 400;
            var receiptWidth = UseForeignCurrency ? 275 : 295;
            var spaceHeaderStart = BillingReceiptOrder ? 90 : UseForeignCurrency ? 130 : 90;
            var billingHeaderCellIndex = BillingReceiptOrder ? 1 : 2;
            var receiptHeaderCellIndex = !BillingReceiptOrder ? 1 : 2;

            var indexCustomerCode = BillingReceiptOrder ? 8 : 11;
            var indexCustomerName = BillingReceiptOrder ? 9 : 12;
            var indexBillingCount = BillingReceiptOrder ? 10 : 13;
            var indexBillingAmount = BillingReceiptOrder ? 11 : 14;
            var indexPayerName = BillingReceiptOrder ? 12 : 8;
            var indexReceiptCount = BillingReceiptOrder ? 13 : 9;
            var indexReceiptAmount = BillingReceiptOrder ? 14 : 10;

            Precision = UseForeignCurrency ? Precision : 0;

            var posX = new int[] { };
            if (BillingReceiptOrder)
            {
                posX = new int[]
                {
                    0,spaceHeaderStart,spaceHeaderStart + billingWidth, spaceHeaderStart + billingWidth + receiptWidth,
                    0, 30, 60,
                    UseForeignCurrency ? 90 : 0,
                    UseForeignCurrency ? 130 : 90,
                    UseForeignCurrency ? 235 : 195,
                    UseForeignCurrency ? 360 : 340,
                    UseForeignCurrency ? 390 : 370,
                    UseForeignCurrency ? 510 : 490,
                    635, 665, 785, 810, 900, 925
                };
            }
            else
            {
                posX = new int[]
                {
                    0,spaceHeaderStart,spaceHeaderStart + receiptWidth, spaceHeaderStart + receiptWidth + billingWidth,
                    0, 30, 60,
                    UseForeignCurrency ? 90 : 0,
                    UseForeignCurrency ? 130 : 90,
                    UseForeignCurrency ? 255 : 235,
                    UseForeignCurrency ? 285 : 265,
                    UseForeignCurrency ? 405 : 385,
                    UseForeignCurrency ? 510 : 490,
                    635, 665, 785, 810, 900, 925
                };
            }

            var posY = new int[] { 0, height, height * 2 };

            #region header row1
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, spaceHeaderStart, "kuuhakustart", location: new Point(posX[0], posY[0])),
                new CellSetting(height, billingWidth, "seikyuujyouhou", caption: "請求情報", location: new Point(posX[billingHeaderCellIndex], posY[0])),
                new CellSetting(height, receiptWidth, "nyukinnjyouhou", caption: "入金情報", location: new Point(posX[receiptHeaderCellIndex], posY[0])),
                new CellSetting(height, 140, "kuuhakuend", location: new Point(posX[3], posY[0])),
            });
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();
            #endregion

            #region header row2
            builder.Items.Add(new CellSetting(height, 30, "Header", location: new Point(posX[4], posY[0]) ));
            if (IsMatched)
            {
                builder.Items.Add(new CellSetting(height, 30, "CheckBox", caption: checkBoxCaption, location: new Point(posX[5], posY[0])));
            }
            else
            {
                builder.Items.Add(new CellSetting(height, 30, "CheckBox", caption: checkBoxCaption, location: new Point(posX[5], posY[0]), dropDown: GetDropDownCheckCellSort()));
            }
            builder.Items.Add(new CellSetting(height, 30, "Individual", caption: "個別", location: new Point(posX[6], posY[0])));

            if (UseForeignCurrency)
            {
                builder.Items.Add(new CellSetting(height, 40, "CurrencyCode", caption: "通貨", location: new Point(posX[7], posY[0]) ));
            }

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,       105, "DispCustomerCode", caption: "得意先コード"      , location: new Point(posX[indexCustomerCode] , posY[0])),
                new CellSetting(height, nameWidth, "DispCustomerName", caption: "得意先名（代表者）", location: new Point(posX[indexCustomerName] , posY[0])),
                new CellSetting(height,        30, "BillingCount"    , caption: "件数"              , location: new Point(posX[indexBillingCount] , posY[0])),
                new CellSetting(height,       120, "BillingAmount"   , caption: "金額"              , location: new Point(posX[indexBillingAmount], posY[0])),
                new CellSetting(height, nameWidth, "PayerName"       , caption: "振込依頼人名"      , location: new Point(posX[indexPayerName]    , posY[0])),
                new CellSetting(height,        30, "ReceiptCount"    , caption: "件数"              , location: new Point(posX[indexReceiptCount] , posY[0])),
                new CellSetting(height,       120, "ReceiptAmount"   , caption: "金額"              , location: new Point(posX[indexReceiptAmount], posY[0])),
                new CellSetting(height,        25, "ShareTransferFee", caption: "手"                , location: new Point(posX[15]                , posY[0])),
                new CellSetting(height,        90, "DispDifferent"   , caption: "差額"              , location: new Point(posX[16]                , posY[0]))
            });
            if (IsMatched)
            {
                builder.Items.Add(new CellSetting(height, 25, memoField, caption: memoDisplayCaption, location: new Point(posX[17], posY[0]) ));
            }
            else
            {
                builder.Items.Add(new CellSetting(height, 25, memoField, caption: memoDisplayCaption, location: new Point(posX[17], posY[0]), dropDown: GetDropDownAdvanceReceivedStateCellSort() ));
            }
            builder.BuildHeaderOnly(template);
            #endregion

            #region rowItems
            var thickBorder = new Border()
            {
                Top     = new Line(LineStyle.Thin  , GridLineColor),
                Bottom  = new Line(LineStyle.Thin  , GridLineColor),
                Left    = new Line(LineStyle.Thin  , GridLineColor),
                Right   = new Line(LineStyle.Medium, GridLineColor),
            };
            var billingBorder = BillingReceiptOrder ? thickBorder : null;
            var receiptBorder = !BillingReceiptOrder ? thickBorder : null;
            var cbxCell = builder.GetCheckBoxCell(isBoolType: true);
            builder.Items.Clear();
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 30, "Header"    , location: new Point(posX[4], posY[0]), cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 30, "CheckBox"  , location: new Point(posX[5], posY[0]), cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false, enabled: true, dataField: "Checked" ),
                new CellSetting(height, 30, "Individual", location: new Point(posX[6], posY[0]) ,cell: builder.GetButtonCell(), value: "…")
            });

            if (UseForeignCurrency)
            {
                builder.Items.Add(new CellSetting(height, 40, "CurrencyCode",  dataField: "CurrencyCode", location: new Point(posX[7], posY[0]), cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), backColor: BillingReceiptOrder ? MatchingGridBillingBackColor : Color.Empty ));
            }

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,       105, "DispCustomerCode"   , dataField: "DispCustomerCode"     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), location: new Point(posX[indexCustomerCode], posY[0]) , backColor: MatchingGridBillingBackColor),
                new CellSetting(height, nameWidth, "DispCustomerName"   , dataField: "DispCustomerName"     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , location: new Point(posX[indexCustomerName], posY[0]) , backColor: MatchingGridBillingBackColor),
                new CellSetting(height,        30, "BillingCount"       , dataField: "DispBillingCount"     , cell: builder.GetNumberCell(MultiRowContentAlignment.MiddleRight)  , location: new Point(posX[indexBillingCount], posY[0]) , backColor: MatchingGridBillingBackColor),
                new CellSetting(height,       120, "BillingAmount"      , dataField: "DispBillingAmount"    , cell: builder.GetNumberCellCurrency(Precision, Precision, 0)       , location: new Point(posX[indexBillingAmount], posY[0]), backColor: MatchingGridBillingBackColor, border: billingBorder),
                new CellSetting(height, nameWidth, "PayerName"          , dataField: "PayerName"            , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)  , location: new Point(posX[indexPayerName], posY[0]) ),
                new CellSetting(height,        30, "ReceiptCount"       , dataField: "DispReceiptCount"     , cell: builder.GetNumberCell(MultiRowContentAlignment.MiddleRight)  , location: new Point(posX[indexReceiptCount], posY[0])),
                new CellSetting(height,       120, "ReceiptAmount"      , dataField: "DispReceiptAmount"    , cell: builder.GetNumberCellCurrency(Precision, Precision, 0)       , location: new Point(posX[indexReceiptAmount], posY[0]), border: receiptBorder),
                new CellSetting(height,        25, "ShareTransferFee"   , dataField: "DispShareTransferFee" , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), location: new Point(posX[15], posY[0])),
                new CellSetting(height,        90, "DispDifferent"      , dataField: "DispDifferent"        , cell: builder.GetNumberCellCurrency(Precision, Precision, 0)       , location: new Point(posX[16], posY[0])),
                new CellSetting(height,        25, memoField            , dataField: memoField              , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), location: new Point(posX[17], posY[0])),
                new CellSetting(height,         0, "CheckOn"            , dataField: "Checked"        ),
                new CellSetting(height,         0, "UseFeeTolerance"    , dataField: "UseFeeTolerance"),
                new CellSetting(height,         0, "BillingCountValue"  , dataField: "BillingCount"   ),
                new CellSetting(height,         0, "ReceiptCountValue"  , dataField: "ReceiptCount"   ),
                new CellSetting(height,         0, "UpdateFlag"         , dataField: "UpdateFlag"     ),
                new CellSetting(height,         0, "BankTransferFee"    , dataField: "BankTransferFee"),
                new CellSetting(height,         0, "DisplayOrder"       , dataField: "DisplayOrder"   ),
                new CellSetting(height,         0, memoDataField        , dataField: memoDataField    ),
                new CellSetting(height,         0, "Different"          , dataField: "Different"      ),
                new CellSetting(height,         0, "CustomerCode"       , dataField: "CustomerCode"   ),
                new CellSetting(height,         0, "CustomerName"       , dataField: "CustomerName"   ),
                new CellSetting(height,         0, "DupeCheck"          , dataField: "DupeCheck"      ),
                new CellSetting(height,         0, "BillingPriority"    , dataField: "BillingPriority"),
                new CellSetting(height,         0, "ReceiptPriority"    , dataField: "ReceiptPriority"),
                new CellSetting(height,         0, "BillingDisplayOrder", dataField: "BillingDisplayOrder"),
                new CellSetting(height,         0, "ReceiptDisplayOrder", dataField: "ReceiptDisplayOrder"),
            });

            builder.BuildRowOnly(template);
            grdCollation.Template = template;
            #endregion

            grdCollation.DefaultCellStyle.BackColor = Color.Empty;
            grdCollation.AlternatingRowsDefaultCellStyle.BackColor = Color.Empty;
        }

        private void grid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.RowIndex < 0) return;
            if (!IsMatched)
            {
                var collation = grdCollation.Rows[e.RowIndex].DataBoundItem as Collation;
                if (collation == null) return;
                if (collation.UpdateFlag)
                    grdCollation[e.RowIndex, e.CellIndex].Style.BackColor = ControlDisableBackColor;
                else if (e.CellName == "celDispDifferent")
                {
                    var color = (collation.DispDifferent.HasValue
                        && grdCollation[e.RowIndex, "celCheckBox"].Enabled)
                        ? MatchingGridReceiptBackColor : SystemColors.Window;
                    grdCollation[e.RowIndex, e.CellIndex].Style.BackColor = color;
                }
            }
            else
            {
                if (e.CellName == "celDispDifferent")
                {
                    grdCollation[e.RowIndex, e.CellIndex].Style.BackColor = MatchingGridReceiptBackColor;
                }
            }
        }

        private void grid_CellClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                if (e.CellName == "lblseikyuujyouhou")
                {
                    var cell1 = grdCollation.ColumnHeaders[0]["lblseikyuujyouhou"];
                    cell1.Style.Font = new Font(cell1.Style.Font, FontStyle.Bold);
                    var cell2 = grdCollation.ColumnHeaders[0]["lblnyukinnjyouhou"];
                    cell2.Style.Font = new Font(cell2.Style.Font, FontStyle.Regular);
                    BillingPriority = true;
                    SortGridData(eSortType.None);

                }
                else if (e.CellName == "lblnyukinnjyouhou")
                {
                    var cell1 = grdCollation.ColumnHeaders[0]["lblseikyuujyouhou"];
                    cell1.Style.Font = new Font(cell1.Style.Font, FontStyle.Regular);
                    var cell2 = grdCollation.ColumnHeaders[0]["lblnyukinnjyouhou"];
                    cell2.Style.Font = new Font(cell2.Style.Font, FontStyle.Bold);
                    BillingPriority = false;
                    SortGridData(eSortType.None);
                }
            }
        }

        private void grid_SizeChanged(object sender, EventArgs e)
        {
            var gridWidthDefault = GridDefaultWidth;
            var w = grdCollation.Size.Width;
            var columnWidth = nameWidth + (w - gridWidthDefault) / 2;
            var cell1 = grdCollation.ColumnHeaders[1]["lblDispCustomerName"];
            cell1.HorizontalResize(columnWidth - cell1.Width);
            var cell2 = grdCollation.ColumnHeaders[1]["lblPayerName"];
            cell2.HorizontalResize(columnWidth - cell2.Width);
        }

        #endregion

        private void PE0101_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();


                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);

                SetApplicationSetting();

                if (ColorContext != null)
                {
                    MatchingGridBillingBackColor = ColorContext.MatchingGridBillingBackColor;
                    MatchingGridReceiptBackColor = ColorContext.MatchingGridReceiptBackColor;
                    ControlDisableBackColor = ColorContext.ControlDisableBackColor;
                    GridLineColor = ColorContext.GridLineColor;
                }

                nmbDisplayPrecision.Value = Settings.RestoreControlValue<PE0101, decimal>(Login, nmbDisplayPrecision.Name) ?? 2M;
                FormatNumber = Convert.ToInt32(nmbDisplayPrecision.Value);
                //Settings.SetCheckBoxValue<PE0101>(Login, cbxUseSection);
                BillingPriority = Settings.RestoreControlValue<PE0101, bool>(Login, nameof(BillingPriority)) ?? true;
                InitializeGridTemplate();
                SuspendLayout();
                tbcMain.SelectedIndex = 1;
                tbcMain.SelectedIndex = 0;
                ResumeLayout();
                InitializeDepartmentSelection();
                InitializeSectionSelection();

               
                datReceiptRecordedAtFrom.Clear();
                datBillingDueAtFrom.Clear();
                BaseContext.SetFunction05Caption(UseDistribution ? "配信" : "");
                cbxShowMatched.CheckedChanged += cbxShowMatched_CheckedChanged;

                if (BillingPriority)
                {
                    grdCollation.ColumnHeaders[0]["lblseikyuujyouhou"].Style.Font = new Font(grdCollation.ColumnHeaders[0][1].Style.Font, FontStyle.Bold);
                }
                else
                {
                    grdCollation.ColumnHeaders[0]["lblnyukinnjyouhou"].Style.Font = new Font(grdCollation.ColumnHeaders[0][2].Style.Font, FontStyle.Bold);
                }


                grdCollation.CellClick += grid_CellClick;
                grdCollation.SizeChanged += grid_SizeChanged;

                tbcMain.SelectedIndexChanged += (s, ev) =>
                {
                    var dispResult = tbcMain.SelectedIndex == 1;
                    lblMatchedData.Visible = (dispResult && cbxShowMatched.Checked);
                    lblMatchedData.BackColor = Color.Yellow;
                    BaseContext.SetFunction07Enabled(dispResult);
                };

                var billingItems = new[] {
                    new { name = lblBillingMatchingItemName1, order = lblBillingSortOrder1 },
                    new { name = lblBillingMatchingItemName2, order = lblBillingSortOrder2 },
                    new { name = lblBillingMatchingItemName3, order = lblBillingSortOrder3 },
                    new { name = lblBillingMatchingItemName4, order = lblBillingSortOrder4 },
                    new { name = lblBillingMatchingItemName5, order = lblBillingSortOrder5 },
                    new { name = lblBillingMatchingItemName6, order = lblBillingSortOrder6 },
                    new { name = lblBillingMatchingItemName7, order = lblBillingSortOrder7 },
                    new { name = lblBillingMatchingItemName8, order = lblBillingSortOrder8 },
                };
                foreach (var pair in MatchingBillingOrder.Where(x => x.Available == 1).Select((x, i) => new { x, i }))
                {
                    var index = pair.i;
                    billingItems[index].name.Text = $"{index + 1}.{pair.x.ItemNameJp}";
                    billingItems[index].order.Text = pair.x.SortOrderJp;
                }
                var receiptItems = new[] {
                    new { name = lblReceiptMatchingItemName1, order = lblReceiptSortOrder1 },
                    new { name = lblReceiptMatchingItemName2, order = lblReceiptSortOrder2 },
                    new { name = lblReceiptMatchingItemName3, order = lblReceiptSortOrder3 },
                    new { name = lblReceiptMatchingItemName4, order = lblReceiptSortOrder4 },
                    new { name = lblReceiptMatchingItemName5, order = lblReceiptSortOrder5 },
                    new { name = lblReceiptMatchingItemName6, order = lblReceiptSortOrder6 },
                    new { name = lblReceiptMatchingItemName7, order = lblReceiptSortOrder7 },
                    new { name = lblReceiptMatchingItemName8, order = lblReceiptSortOrder8 },
                };
                foreach (var pair in MatchingReceiptOrder.Where(x => x.Available == 1).Select((x, i) => new { x, i }))
                {
                    var index = pair.i;
                    receiptItems[index].name.Text = $"{index + 1}.{pair.x.ItemNameJp}";
                    receiptItems[index].order.Text = pair.x.SortOrderJp;
                }

                if (CollationSetting.UseFromToNarrowing == 0)
                {
                    datReceiptRecordedAtTo.Select();
                }
                else
                {
                    datReceiptRecordedAtFrom.Select();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                    LoadCompanyAsync(),
                    LoadApplicationControlAsync(),
                    LoadFunctionAuthorities(FunctionType.CancelMatching),
                    LoadControlColorAsync(),
                    LoadGeneralSettings(),
                    LoadCollationSetting(),
                    LoadLegalPersonality(),
                    LoadMatchingBillingOrder(),
                    LoadMatchingReceiptOrder(),
                    LoadClientKeyAsync(),
                    LoadDepartmentsAsync(),
                };

            await Task.WhenAll(tasks);

            if (UseMFWebApi)
            {
                var setting = await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterService.WebApiSettingMasterClient client) =>
                    (await client.GetByIdAsync(SessionKey, CompanyId, Rac.VOne.Common.WebApiType.MoneyForward))?.WebApiSetting);
                if (setting != null)
                {
                    var matchingSetter = new MFCInvoice.MFBillingStatusSetter {
                        Login           = Login,
                        OwnerForm       = ParentForm,
                        IsMatched       = true,
                        WebApiSetting   = setting,
                    };
                    MatchingPostProcessor = matchingSetter.Update;

                    var cancelSetter = new MFCInvoice.MFBillingStatusSetter {
                        Login           = Login,
                        OwnerForm       = ParentForm,
                        IsMatched       = false,
                        WebApiSetting   = setting,
                    };
                    CancelPostProcessor = cancelSetter.Update;
                }
            }

            if (!UseSection)
            {
                DepartmentsWithLoginUser = new List<Department>();
                Sections = new List<Section>();
                SectionsWithLoginUser = new List<Section>();
                return;
            }
            await Task.WhenAll(
                LoadDepartmentsByLoginUserIdAsync(),
                LoadSectionAsync(),
                LoadSectionWithLoginAsync()
                );
        }
        private void InitializeDepartmentSelection()
        {
            if (!UseSection || DepartmentsWithLoginUser.Count == 0)
            {
                lblDepartmentName.Text = "すべて";
                DepartmentIds = Departments.Select(x => x.Id).ToList();
            }
            else
            {
                if (Departments.Count == DepartmentsWithLoginUser.Count)
                {
                    lblDepartmentName.Text = "すべて";
                }
                else if (DepartmentsWithLoginUser.Count == 1)
                {
                    lblDepartmentName.Text = DepartmentsWithLoginUser.First().Name;
                }
                else
                {
                    lblDepartmentName.Text = "請求部門絞込有";
                }
                DepartmentIds = DepartmentsWithLoginUser.Select(x => x.Id).ToList();
            }
        }
        private void InitializeSectionSelection()
        {
            if (SectionsWithLoginUser.Count == 0)
            {
                lblSectionName.Text = "すべて";
                SectionIds = Sections.Select(item => item.Id).ToList();
            }
            else
            {
                if (Sections.Count == SectionsWithLoginUser.Count)
                {
                    lblSectionName.Text = "すべて";
                }
                else if (SectionsWithLoginUser.Count == 1)
                {
                    lblSectionName.Text = SectionsWithLoginUser.FirstOrDefault().Name;
                }
                else
                {
                    lblSectionName.Text = "入金部門絞込有";
                }
                SectionIds = SectionsWithLoginUser.Select(item => item.Id).ToList();
            }
        }

        #endregion

        #region Function Key Event

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("照合");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Collate;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearItems;

            BaseContext.SetFunction03Caption("一括消込");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = MatchingSequential;

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = PrintData;

            BaseContext.SetFunction05Enabled(false);
            OnF05ClickHandler = SendMail;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = ExportData;

            BaseContext.SetFunction07Caption("検索");
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = SearchText;

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitForm;
        }
      
        [OperationLog("照合")]
        private void Collate()
        {
            var result = CollateInner();
            if (!result
                || CollationSetting?.AutoMatching == 0
                || MatchingCancelExecuted) return;
            var matchingResult = MatchingInner(silent: true);

            if (matchingResult ?? false)
                CollateInner();

        }

        [OperationLog("表示")]
        private void DispMatchedData()
        {
            SearchMatchingHeaders();
        }


        [OperationLog("クリア")]
        private void ClearItems()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            DoClear();
            tbcMain.SelectedIndex = 0;
        }

        [OperationLog("一括消込")]
        private void MatchingSequential()
        {
            var matchingResut = MatchingInner();
            if ( matchingResut.HasValue && !matchingResut.Value) return; // error
            if (!matchingResut.HasValue && !ShowConfirmDialog(MsgInfConfirmCollate))
            {
                DoClear();
                DispStatusMessage(MsgInfCancelProcess);
                return;
            }
            if ( matchingResut.HasValue && !ShowConfirmDialog(MsgInfFinishMatchingSeqAndConfirmCollate))
            {
                DoClear();
                DispStatusMessage(MsgInfProcessFinish);
                return;
            }
            CollateInner();
        }


        [OperationLog("消込解除")]
        private void CancelMatching()
        {
            var cancelResult = CancelInner();
            if ( cancelResult.HasValue) MatchingCancelExecuted |= cancelResult.Value;
            if ( cancelResult.HasValue && !cancelResult.Value) return; // error
            if (!cancelResult.HasValue && !ShowConfirmDialog(MsgInfConfirmGetMatched))
            {
                DoClear();
                DispStatusMessage(MsgInfCancelProcess);
                return;
            }
            if ( cancelResult.HasValue && !ShowConfirmDialog(MsgInfFinishCancelAndConfrimGetMatched))
            {
                DoClear();
                DispStatusMessage(MsgInfProcessFinish);
                return;
            }
            SearchMatchingHeaders();
        }

        [OperationLog("印刷")]
        private void PrintData()
        {
            try
            {
                ClearStatusMessage();

                if (IsMatched)
                {
                    var matchinghistorySearchForm = ApplicationContext.Create(nameof(PE0301));
                    PE0301 matchinghistoryScreen = matchinghistorySearchForm.GetAll<PE0301>().FirstOrDefault();
                    matchinghistorySearchForm.StartPosition = FormStartPosition.CenterParent;
                    matchinghistoryScreen.ReturnScreen = this;
                    ApplicationContext.ShowDialog(ParentForm, matchinghistorySearchForm);
                }
                else
                {
                    var checkType = GetCheckedType();

                    List<Collation> collationList = null;
                    using (var form = ApplicationContext.Create(nameof(PE0112)))
                    {
                        var screen = form.GetAll<PE0112>().First();
                        screen.OutputType = 0; // 0:印刷
                        screen.ParentCheckType = checkType;

                        screen.InitializeParentForm("一括消込チェックリスト　出力内容指定");

                        var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                        if (dialogResult != DialogResult.OK)
                        {
                            DispStatusMessage(MsgInfCancelProcess, "印刷");
                            return;
                        }
                        collationList = GetOutputCollcations(screen.CheckType);
                    }


                    if (!(collationList?.Any() ?? false))
                    {
                        ShowWarningDialog(MsgWngPrintDataNotExist);
                        return;
                    }

                    Task<string> pathTask = GetServerPath();
                    var matchingReport = new GrapeCity.ActiveReports.SectionReport();

                    if (!BillingReceiptOrder)
                    {
                        var matchingReceiptBillingReport = new MatchingSequentialReceiptBillingSectionReport();
                        matchingReceiptBillingReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        matchingReceiptBillingReport.Name = "一括消込チェックリスト" + DateTime.Today.ToString("yyyyMMdd");
                        matchingReceiptBillingReport.SetPageDataSetting(collationList, true, ApplicationControl.UseForeignCurrency, FormatNumber, CollationSearch);
                        matchingReport = matchingReceiptBillingReport;
                    }
                    else
                    {
                        var matchingBillingReceiptReport = new MatchingSequentialSectionReport();
                        matchingBillingReceiptReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        matchingBillingReceiptReport.Name = "一括消込チェックリスト" + DateTime.Today.ToString("yyyyMMdd");
                        matchingBillingReceiptReport.SetPageDataSetting(collationList, true, ApplicationControl.UseForeignCurrency, FormatNumber, CollationSearch);
                        matchingReport = matchingBillingReceiptReport;
                    }

                    var searchConditonReport = new SearchConditionSectionReport();
                    searchConditonReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "一括消込チェックリスト");
                    searchConditonReport.Name = "一括消込チェックリスト" + DateTime.Today.ToString("yyyyMMdd");
                    searchConditonReport.SetPageDataSetting(SearchCondition);
                    ProgressDialog.Start(ParentForm, Task.WhenAll(pathTask, Task.Run(() =>
                    {
                        matchingReport.Run(false);
                        searchConditonReport.SetPageNumber(matchingReport.Document.Pages.Count);
                        searchConditonReport.Run(false);

                    })), false, SessionKey);

                    var serverPath = pathTask.Result;
                    matchingReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchConditonReport.Document.Pages.Clone());
                    ShowDialogPreview(ParentForm, matchingReport, serverPath);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("エクスポート")]
        private void ExportData()
        {
            ClearStatusMessage();
            try
            {
                var checkType = GetCheckedType();

                using (var form = ApplicationContext.Create(nameof(PE0112)))
                {
                    var screen = form.GetAll<PE0112>().First();
                    screen.OutputType = 1;
                    screen.ParentCheckType = checkType;
                    screen.InitializeParentForm($"{(IsMatched ? "消込完了" : "一括消込")}チェックリスト　出力内容指定");

                    var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                    if (dialogResult != DialogResult.OK)
                    {
                        DispStatusMessage(MsgInfCancelProcess, "エクスポート");
                        return;
                    }
                    checkType = screen.CheckType;
                }

                var result = IsMatched
                    ? ExportMatchingHeaderData(checkType)
                    : ExportCollationData(checkType);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        private bool ExportCollationData(int checkType)
        {
            var result = false;
            try
            {
                var items = GetOutputCollcations(checkType);
                if (!(items?.Any() ?? false))
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return result;
                }

                var task = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var serverPath = task.Result;
                serverPath = Util.GetDirectoryName(serverPath);

                var filePath = string.Empty;
                var fileName = $"一括消込チェックリスト{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return result;

                var exporter = GetExporter(GetCollationFileDefinition());
                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, items, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return result;
                }

                result = true;
                DispStatusMessage(MsgInfFinishExport);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
            return result;
        }
        private bool ExportMatchingHeaderData(int checkType)
        {
            var result = false;
            try
            {
                var items = GetOutputMatchingHeaders(checkType);
                if (!(items?.Any() ?? false))
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return result;
                }

                var task = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var serverPath = task.Result;
                serverPath = Util.GetDirectoryName(serverPath);

                var filePath = string.Empty;
                var fileName = $"消込完了チェックリスト{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return result;

                var exporter = GetExporter(GetMatchedFileDefinition());
                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, items, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return result;
                }

                result = true;
                DispStatusMessage(MsgInfFinishExport);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
            return result;
        }

        private string GetOutputAmountFormat()
        {
            var precision = 0;
            if (UseForeignCurrency) precision = (int)(nmbDisplayPrecision.Value ?? 0);
            return precision == 0 ? "0" : "0." + new string('0', precision);
        }

        private CsvExporter<TModel> GetExporter<TModel>(RowDefinition<TModel> definition)
            where TModel : class, new()
        {
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;
            return exporter;
        }

        private MatchingSequentialFileDefinition GetCollationFileDefinition()
        {
            var definition = new MatchingSequentialFileDefinition(new DataExpression(ApplicationControl));
            if (definition.CurrencyCodeField.Ignored = (!UseForeignCurrency))
            {
                definition.CurrencyCodeField.FieldName = null;
            }
            var format = GetOutputAmountFormat();
            definition.BillingAmountField.Format = value => value.ToString(format);
            definition.ReceiptAmountField.Format = value => value.ToString(format);
            definition.Different.Format = value => value.ToString(format);
            return definition;
        }

        private MatchingHeaderFileDefinition GetMatchedFileDefinition()
        {
            var definition = new MatchingHeaderFileDefinition(new DataExpression(ApplicationControl));
            if (definition.CurrencyCodeField.Ignored = (!UseForeignCurrency))
            {
                definition.CurrencyCodeField.FieldName = null;
            }
            var format = GetOutputAmountFormat();
            definition.BillingAmountField.Format = value => value.ToString(format);
            definition.ReceiptAmountField.Format = value => value.ToString(format);
            definition.DifferentField.Format = value => value.ToString(format);
            return definition;
        }

        private List<Collation> GetOutputCollcations(int outputType)
        {
            var result = new List<Collation>();
            var items = grdCollation.Rows.Select(x => x.DataBoundItem as Collation);
            result.AddRange(items?
                .Where(x => outputType == 0
                    || outputType == 1 && x.Checked
                    || outputType == 2 && !x.Checked));
            return result;
        }

        private List<MatchingHeader> GetOutputMatchingHeaders(int outputType)
        {
            var result = new List<MatchingHeader>();
            result.AddRange(grdCollation.Rows.Select(x => x.DataBoundItem as MatchingHeader)
                .Where(x => outputType == 0
                   || outputType == 1 && x.Checked
                   || outputType == 2 && !x.Checked));
            return result;
        }

        [OperationLog("配信")]
        private void SendMail()
        {
            var form = ApplicationContext.Create(nameof(PE0108));
            form.StartPosition = FormStartPosition.CenterParent;
            ApplicationContext.ShowDialog(ParentForm, form);
        }

        [OperationLog("検索")]
        private void SearchText()
        {
            using (var form = ApplicationContext.Create(nameof(PE0115)))
            {
                var screen = form.GetAll<PE0115>().First();
                screen.grid = grdCollation;
                screen.InitializeParentForm("一括消込 検索");
                ApplicationContext.ShowDialog(ParentForm, form, true);
            }
        }

        [OperationLog("全選択")]
        private void SelectAll()
        {
            grdCollation.EndEdit();

            foreach (Row row in grdCollation.Rows
                .Where(x => x["celCheckBox"].Enabled && !Convert.ToBoolean(x["celCheckBox"].Value)))
            {
                Modified = true;
                row["celCheckBox"].Value = true;
            }
        }

        [OperationLog("全解除")]
        private void DeselectAll()
        {
            grdCollation.EndEdit();
            foreach (Row row in grdCollation.Rows
                .Where(x => x["celCheckBox"].Enabled && Convert.ToBoolean(x["celCheckBox"].Value)))
            {
                Modified = true;
                row.Cells["celCheckBox"].Value = false;
            }
        }

        [OperationLog("終了")]
        private void ExitForm()
        {
            try
            {
                //Settings.SaveControlValue<PE0101>(Login, cbxUseSection.Name, cbxUseSection.Checked);
                Settings.SaveControlValue<PE0101>(Login, nmbDisplayPrecision.Name, nmbDisplayPrecision.Value);
                Settings.SaveControlValue<PE0101>(Login, nameof(BillingPriority), BillingPriority);

                if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

                ParentForm.Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void ReturnToSearchCondition()
        {
            tbcMain.SelectedIndex = 0;
        }

        #endregion

        #region collation

        private async Task<List<Collation>> GetCollationsAsync(CollationSearch option)
        {
            List<Collation> result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = async () => await proxy?.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();

                var tasks = new List<Task>();
                if (option.UseDepartmentWork)
                    tasks.Add(Util.SaveWorkDepartmentTargetAsync(Login, ClientKey, DepartmentIds.ToArray()));
                if (option.UseSectionWork)
                    tasks.Add(Util.SaveWorkSectionTargetAsync(Login, ClientKey, SectionIds.ToArray()));
                if (CustomerFees == null)
                    tasks.Add(LoadCustomerFees());
                if (PaymentAgencyFees == null)
                    tasks.Add(LoadPaymentAgencyFees());
                await Task.WhenAll(tasks);

                await ServiceProxyFactory.DoAsync<MatchingServiceClient>(async clinet =>
                {
                    var webResult = await clinet.CollateAsync(SessionKey, option, hubConnection.ConnectionId);
                    if (webResult.ProcessResult.Result)
                        result = webResult.Collation;
                });
            }
            return result;
        }
        private void InitializeGridCheckBoxEnabled(List<Collation> collations)
        {
            // TODO: ScheduledPayment(Income) and DisplayBillingAmount
            var taxTolerance = TaxDifferenceTolerance;
            for (var i = 0; i < grdCollation.Rows.Count; i++)
            {
                var collation = collations[i];
                var feeTolerance = UseForeignCurrency ? collation.CurrencyTolerance : BankFeeTolerance;

                var checkable = collation.VerifyCheckable(CollationSetting, feeTolerance, taxTolerance, UseForeignCurrency,
                    col => IsBankTransferFeeRegistered(col, checkFee: false),
                    col => IsBankTransferFeeRegistered(col, checkFee: true));
                grdCollation[i, "celCheckBox"].Enabled = checkable;

                var duped = collation.DupeCheck == 1;
                if (duped)
                {
                    grdCollation[i, "celCheckBox"].Enabled = false;
                    grdCollation[i, "celCheckBox"].Value = false;
                    var color = ColorContext.CollationDupedReceiptCellBackColor;
                    grdCollation[i, "celPayerName"].Style.BackColor = color;
                    grdCollation[i, "celReceiptCount"].Style.BackColor = color;
                    grdCollation[i, "celReceiptAmount"].Style.BackColor = color;
                }
            }
        }
        private async Task<bool> SetCollationsResultAsync(CollationSearch option)
        {
            var getResult = false;
            List<Collation> collations = null;
            try
            {
                collations = await GetCollationsAsync(option);
                getResult = true;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (!getResult) // 例外
            {
                ProgressManager.Abort();
                ShowWarningDialog(MsgErrSomethingError, "照合処理");
                return false;
            }
            if (collations == null) // キャンセル
            {
                ProgressManager.Canceled = true;
                return false;
            }
            grdCollation.DataSource = new BindingSource(collations, null);
            InitializeGridCheckBoxEnabled(collations);

            if (!collations.Any()) // データなし
            {
                BaseContext.SetFunction03Enabled(false);
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction05Enabled(false);
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction07Enabled(false);
                BaseContext.SetFunction08Enabled(false);
                BaseContext.SetFunction09Enabled(false);

                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                ShowWarningDialog(MsgWngNotExistSearchData);
                return false;
            }

            // データあり
            if (CollationSetting.AutoSortMatchingEnabledData == 1)
                sortType = eSortType.CheckBox;
            SortGridData(sortType);
            tbcMain.SelectedTab = tbpCollationResult;

            if (collations.Any(x => x.DupeCheck == 1))
            {
                ShowWarningDialog(MsgWngReceiptDataDuped);
            }
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction06Enabled(true);
            if (UseDistribution)
                BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(true);
            cbxShowMatched.Enabled = false;
            grid_SizeChanged(grdCollation, EventArgs.Empty);
            ProgressManager.UpdateState();

            return true;
        }
        private bool CollateInner()
        {
            ClearStatusMessage();
            if (!ValidateChildren()) return false;
            if (!ValidateInputValues()) return false;

            var option = (CollationSearch = GetCollationOption());

            var taskList = new List<TaskProgress>();

            taskList.Add(new TaskProgress("請求・入金データの取得"));
            var collationOrder = LoadCollationOrder()
                .Where(x => x.Available == 1)
                .OrderBy(x => x.ExecutionOrder).ToArray();
            foreach (var collation in collationOrder)
            {
                var name = string.Empty;
                switch (collation.CollationTypeId)
                {
                    case 0: name = "専用入金口座照合"; break;
                    case 1: name = "得意先コード照合"; break;
                    case 2: name = "学習履歴照合"; break;
                    case 3: name = "マスターカナ照合"; break;
                    case 4: name = "番号照合"; break;
                }
                if (string.IsNullOrEmpty(name)) continue;
                taskList.Add(new TaskProgress(name));
            }
            taskList.Add(new TaskProgress("未照合データの取得"));
            taskList.Add(new TaskProgress("照合結果の取得"));
            taskList.Add(new TaskProgress("画面の更新"));

            ProgressManager = new TaskProgressManager(taskList);

            var task = SetCollationsResultAsync(option);
            var caption = "一括消込照合";
            var dialogResult = ProgressDialogStart(ParentForm, caption, task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
            if (dialogResult == DialogResult.Cancel)
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            SetSearchData();
            Modified = false;
            return task.Result;
        }

        #endregion

        #region matching sequential

        private async Task<MatchingResult> MatchingSequentialAsync(Collation[] collations, CollationSearch option)
        {
            MatchingResult result = null;
            try
            {
                using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                    () => ProgressManager.UpdateState(),
                    () => ProgressManager.Abort(),
                    (connection, proxy) => OnCancelHandler = async () => await proxy?.Invoke("Cancel", connection.ConnectionId)))
                {
                    await hubConnection.Start();
                    NLogHandler.WriteDebug(this, "一括消込 消込処理開始");
                    result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                        => await client.SequentialMatchingAsync(SessionKey, collations, option, hubConnection.ConnectionId));
                    NLogHandler.WriteDebug(this, "一括消込 消込処理終了");
                }
                var success = result?.ProcessResult.Result ?? false;
                if (!success)
                {
                    ProgressManager.Abort();
                }
                if (success && NettingPostProcessor != null)
                {
                    var syncResult = true;
                    if (result.NettingReceipts.Any())
                        syncResult = NettingPostProcessor.Invoke(result.NettingReceipts.Select(x => x as ITransactionData));
                    success &= syncResult;
                    if (!syncResult && result != null)
                        result.MatchingErrorType = MatchingErrorType.PostProcessError;
                }
                if (success && IsPostProcessorImplemanted)
                {
                    var syncResult = true;
                    syncResult = MatchingPostProcessor.Invoke(result.Matchings.Select(x => x as ITransactionData));
                    success &= syncResult;
                    if (!syncResult && result != null)
                        result.MatchingErrorType = MatchingErrorType.PostProcessError;
                }

                ProgressManager.UpdateState();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return result;
        }
        private bool ConfirmMatchingContinue(Collation[] collations, CollationSearch option, bool silent = false)
        {
            var hasAdvanceReceived = collations.Any(x => x.AdvanceReceivedCount > 0);
            var recordedAtType = CollationSetting.AdvanceReceivedRecordedDateType;
            var requireSelectionRecordedAt = hasAdvanceReceived
                && (recordedAtType == 0 || recordedAtType == 1);
            if (requireSelectionRecordedAt)
            {
                using (var form = ApplicationContext.Create(nameof(PE0110)))
                {
                    var screen = form.GetAll<PE0110>().First();
                    screen.AdvanceReceiveSetting = recordedAtType;
                    screen.InitializeParentForm("前受金消込処理年月日の設定");
                    var res = ApplicationContext.ShowDialog(ParentForm, form, true);
                    if (res != DialogResult.OK)
                    {
                        DispStatusMessage(MsgInfCancelProcess, "一括消込");
                        return false;
                    }
                    option.AdvanceReceivedRecordedAt = screen.AdvanceReceiveRecordDate.Date;
                }
            }
            if (!silent && !ShowConfirmDialog(MsgQstConfirmStartSeqMatching))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 一括消込処理
        /// </summary>
        /// <param name="silent"></param>
        /// <returns>
        /// true    : 正常終了
        /// false   : 異常終了/エラー
        /// null    : 消込処理実行中のキャンセル
        /// </returns>
        private bool? MatchingInner(bool silent = false)
        {
            ClearStatusMessage();
            grdCollation.EndEdit();
            var collations = GetCheckedArray<Collation>();
            #region validation
            if (!collations.Any())
            {
                if (!silent) ShowWarningDialog(MsgWngSelectionRequired, "一括消込を行う明細");
                return false;
            }
            #endregion
            var option = CollationSearch;
            if (!ConfirmMatchingContinue(collations, option, silent)) return false;

            ProgressManager = new TaskProgressManager(new List<TaskProgress> {
                    new TaskProgress("消込処理", collations.Length),
                    new TaskProgress("一括消込処理"),
                });
            var task = MatchingSequentialAsync(collations, option);
            var dialogResult = ProgressDialogStart(ParentForm, "一括消込", task, ProgressManager, Login, AutoCloseProgressDialog, Cancellable, OnCancelHandler);
            if (dialogResult == DialogResult.Cancel)
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return null;
            }
            var result = task.Result;
            var index = result?.ErrorIndex ?? -1;
            if (result == null)
            {
                ShowWarningDialog(MsgErrSomethingError, "一括消込");
                return false;
            }
            var errorType = result.MatchingErrorType;
            var row = index > -1 ? GetCheckedRows().Skip(index).FirstOrDefault() : null;
            if (errorType == MatchingErrorType.BillingRemainChanged )
            {
                if (row != null) row.Selected = true;
                ShowWarningDialog(MsgWngIncludeOtherUserMatchedData, "請求");
                return false;
            }
            if (errorType == MatchingErrorType.ReceiptRemainChanged )
            {
                if (row != null) row.Selected = true;
                ShowWarningDialog(MsgWngIncludeOtherUserMatchedData, "入金");
                return false;
            }
            if (errorType == MatchingErrorType.PostProcessError)
            {
                ShowWarningDialog(MsgErrPostProcessFailure);
                return false;
            }

            Modified = false;
            DispStatusMessage(MsgInfMatchingProcessFinish);
            return result.ProcessResult.Result;
        }

        #endregion

        #region get matching headers

        private async Task<List<MatchingHeader>> GetMatchingHeadersAsync(CollationSearch option)
        {
            List<MatchingHeader> result = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = async () => await proxy?.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<MatchingServiceClient>(async client =>
                {
                    var webResult = await client.SearchMatchedDataAsync(SessionKey, option, hubConnection.ConnectionId);
                    if (webResult.ProcessResult.Result)
                        result = webResult.MatchingHeaders;
                });
            }
            return result;
        }

        private async Task<bool> SetMatchingHeadersAsync(CollationSearch option)
        {
            var getResult = false;
            List<MatchingHeader> headers = null;
            try
            {
                var tasks = new List<Task>();
                if (option.UseDepartmentWork)
                    tasks.Add(Util.SaveWorkDepartmentTargetAsync(Login, ClientKey, DepartmentIds.ToArray()));
                if (option.UseSectionWork)
                    tasks.Add(Util.SaveWorkSectionTargetAsync(Login, ClientKey, SectionIds.ToArray()));
                if (tasks.Any())
                    await Task.WhenAll(tasks);

                headers = await GetMatchingHeadersAsync(option);
                getResult = true;
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (!getResult)
            {
                ProgressManager.Abort();
                ShowWarningDialog(MsgErrSomethingError, "消込済データ取得");
                return false;
            }
            if (headers == null)
            {
                ProgressManager.Canceled = true;
                return false;
            }
            grdCollation.DataSource = new BindingSource(headers, null);
            if (!headers.Any())
            {
                BaseContext.SetFunction03Enabled(false);
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction05Enabled(false);
                BaseContext.SetFunction06Enabled(false);
                BaseContext.SetFunction07Enabled(false);
                BaseContext.SetFunction08Enabled(false);
                BaseContext.SetFunction09Enabled(false);

                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                ShowWarningDialog(MsgWngNotExistSearchData);
                return false;
            }
            SortGridData(sortType);
            tbcMain.SelectedTab = tbpCollationResult;
            BaseContext.SetFunction03Enabled(Authorities[FunctionType.CancelMatching]);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction06Enabled(true);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(true);
            cbxShowMatched.Enabled = false;
            grid_SizeChanged(grdCollation, EventArgs.Empty);
            ProgressManager.UpdateState();
            return true;
        }

        private bool SearchMatchingHeaders()
        {
            ClearStatusMessage();
            if (!ValidateChildren()) return false;
            if (!ValidateInputValues()) return false;

            var option = (CollationSearch = GetCollationOption());
            var taskList = new List<TaskProgress>();
            taskList.Add(new TaskProgress("消込完了データの取得"));
            taskList.Add(new TaskProgress("画面の更新"));
            ProgressManager = new TaskProgressManager(taskList);
            var task = SetMatchingHeadersAsync(option);
            var caption = "消込完了データ検索";
            var dialogResult = ProgressDialogStart(ParentForm, caption, task, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
            if (dialogResult == DialogResult.Cancel)
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }
            SetSearchData();
            return task.Result;
        }

        #endregion

        #region cancel matching

        private async Task<MatchingResult> CancelAsync(MatchingHeader[] headers)
        {
            MatchingResult result = null;
            try
            {
                using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                    () => ProgressManager.UpdateState(),
                    () => ProgressManager.Abort(),
                    (connection, proxy) => OnCancelHandler = async () => await proxy?.Invoke("Cancel", connection.ConnectionId)))
                {
                    await hubConnection.Start();
                    result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                        => await client.CancelMatchingAsync(SessionKey, headers, Login.UserId, hubConnection.ConnectionId));
                }
                var success = result?.ProcessResult.Result ?? false;
                if (!success)
                {
                    ProgressManager.Abort();
                }
                if (success && IsPostProcessorImplemanted)
                {
                    var syncResult = true;
                    if (DeletePostProcessor != null)
                    {
                        syncResult = DeletePostProcessor.Invoke(result.DeleteReceipts.Select(x => x as ITransactionData));
                        success &= syncResult;
                    }
                    if (success && CancelPostProcessor != null)
                    {
                        syncResult = CancelPostProcessor.Invoke(result.Matchings.Select(x => x as ITransactionData));
                    }
                    success &= syncResult;
                    result.ProcessResult.Result = success;
                    if (!success)
                        result.MatchingErrorType = MatchingErrorType.PostProcessError;
                }
                ProgressManager.UpdateState();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return result;
        }

        /// <summary>
        /// 消込解除処理
        /// </summary>
        /// <returns>
        /// true    : 正常終了
        /// false   : 異常終了
        /// null    : 消込解除実行中のキャンセル
        /// </returns>
        private bool? CancelInner()
        {
            ClearStatusMessage();
            grdCollation.EndEdit();
            var headers = GetCheckedArray<MatchingHeader>();
            if (!headers.Any())
            {
                ShowWarningDialog(MsgWngSelectionRequired, "消込解除を行う明細");
                return false;
            }
            if (!ShowConfirmDialog(MsgQstConfirmCancel))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }

            ProgressManager = new TaskProgressManager(new List<TaskProgress>() {
                    new TaskProgress("解除処理", headers.Length),
                    new TaskProgress("消込解除処理"),
                });
            var task = CancelAsync(headers);
            NLogHandler.WriteDebug(this, "一括消込 消込解除開始");
            var dialogResult = ProgressDialogStart(ParentForm, "消込解除", task, ProgressManager, Login, AutoCloseProgressDialog, Cancellable, OnCancelHandler);
            NLogHandler.WriteDebug(this, "一括消込 消込解除終了");
            if (dialogResult == DialogResult.Cancel)
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return null;
            }
            var result = task.Result;
            var index = result?.ErrorIndex ?? -1;
            if (result == null)
            {
                ShowWarningDialog(MsgErrSomethingError, "消込解除");
                return false;
            }
            var errorType = result.MatchingErrorType;
            var row = index > -1 ? GetCheckedRows().Skip(index).First() : null;
            var header = index > -1 ? headers[index] : null;
            if (errorType == MatchingErrorType.BillingOmitted)
            {
                if (row != null) row.Selected = true;
                ShowWarningDialog(MsgWngCancelMatchingError, $"請求データ - 得意先：{header.DispCustomerCode} {header.DispCustomerName}");
                return false;
            }
            if (errorType == MatchingErrorType.ReceiptOmitted)
            {
                if (row != null) row.Selected = true;
                ShowWarningDialog(MsgWngCancelMatchingError, $"入金データ - 振込依頼人名：{header.PayerName}");
                return false;
            }
            if (errorType == MatchingErrorType.CashOnDueDateOmitted)
            {
                if (row != null) row.Selected = true;
                ShowWarningDialog(MsgWngCancelMatchingError, $"期日入金予定データ - 得意先：{header.DispCustomerCode} {header.DispCustomerName}");
                return false;
            }

            if (errorType == MatchingErrorType.PostProcessError)
            {
                ShowWarningDialog(MsgErrPostProcessFailure);
                return false;
            }

            if (errorType != MatchingErrorType.None)
            {
                if (row != null) row.Selected = true;
                ShowWarningDialog(MsgWngAlreadyUpdated);
                return false;
            }

            Modified = false;
            DispStatusMessage(MsgInfFinishCancelation);
            return result.ProcessResult.Result;
        }

        #endregion

        #region call web service
        private List<CollationOrder> LoadCollationOrder()
        {
            return ServiceProxyFactory.Do((CollationSettingMasterClient client) =>
            {
                var result = client.GetCollationOrder(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    return result.CollationOrders;
                }
                else return null;
            });
        }

        private async Task LoadGeneralSettings()
        {
            var keyDueAtOffset = "回収予定範囲";
            var keyFeeTolerance = "手数料誤差";
            var keyTaxTolerance = "消費税誤差";
            await ServiceProxyFactory.DoAsync<GeneralSettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    var offset = 0;
                    if (int.TryParse(result?.GeneralSettings?
                            .FirstOrDefault(x => x.Code == keyDueAtOffset)?.Value, out offset))
                        BillingDueAtOffset = offset;
                    var feeTolerance = 0M;
                    if (decimal.TryParse(result?.GeneralSettings?
                            .FirstOrDefault(x => x.Code == keyFeeTolerance)?.Value, out feeTolerance))
                        BankFeeTolerance = feeTolerance;
                    var taxTolerance = 0M;
                    if (decimal.TryParse(result?.GeneralSettings?
                            .FirstOrDefault(x => x.Code == keyTaxTolerance)?.Value, out taxTolerance))
                        TaxDifferenceTolerance = taxTolerance;
                }
            });
            datBillingDueAtTo.Value = DateTime.Today.AddDays(BillingDueAtOffset);
        }

        private async Task LoadCollationSetting()
        {

            await ServiceProxyFactory.DoAsync<CollationSettingMasterClient>(async client =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                    CollationSetting = result.CollationSetting;

            });

            if (CollationSetting.UseFromToNarrowing == 0)
            {
                datReceiptRecordedAtFrom.Visible = false;
                datBillingDueAtFrom.Visible = false;
                lblReceiptRecordedAtFromTo.Visible = false;
                lblBillingDueAtFromTo.Visible = false;
                lblReceiptRecordedAtBefore.Visible = true;
                lblBillingDueAtBefore.Visible = true;

                var diff = datReceiptRecordedAtTo.Location.X - datReceiptRecordedAtFrom.Location.X;
                lblReceiptRecordedAtBefore.Left = lblReceiptRecordedAtBefore.Left - diff;
                lblBillingDueAtBefore.Left = lblBillingDueAtBefore.Left - diff;
                datReceiptRecordedAtTo.Left = datReceiptRecordedAtFrom.Location.X;
                datBillingDueAtTo.Left = datBillingDueAtFrom.Location.X;
            }
            else
            {
                datReceiptRecordedAtFrom.Visible = true;
                datBillingDueAtFrom.Visible = true;
                lblReceiptRecordedAtFromTo.Visible = true;
                lblBillingDueAtFromTo.Visible = true;
                lblReceiptRecordedAtBefore.Visible = false;
                lblBillingDueAtBefore.Visible = false;

            }

        }

        private async Task LoadLegalPersonality()
        {
            await ServiceProxyFactory.DoAsync<JuridicalPersonalityMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    LegalPersonalities = result?
                        .JuridicalPersonalities
                        .Select(x => x.Kana).ToList() ?? new List<string>();
                else
                    LegalPersonalities = new List<string>();
            });
        }

        private async Task LoadCustomerFees()
        {
            await ServiceProxyFactory.DoAsync<CustomerFeeMasterClient>(async client =>
            {
                var result = await client.GetForExportAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    CustomerFees = new SortedList<int, CustomerFee[]>(
                        result.CustomerFee.GroupBy(x => x.CustomerId)
                            .ToDictionary(g => g.Key,
                            g => g.ToArray()));
                }
            });
        }

        private async Task LoadPaymentAgencyFees()
        {
            await ServiceProxyFactory.DoAsync<PaymentAgencyFeeMasterClient>(async client =>
            {
                var result = await client.GetForExportAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    PaymentAgencyFees = new SortedList<int, PaymentAgencyFee[]>(
                        result.PaymentAgencyFees.GroupBy(x => x.PaymentAgencyId)
                            .ToDictionary(g => g.Key,
                            g => g.ToArray()));
                }
            });
        }

        private async Task LoadMatchingBillingOrder()
        {
            await ServiceProxyFactory.DoAsync<CollationSettingMasterClient>(async client =>
            {
                var result = await client.GetMatchingBillingOrderAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    MatchingBillingOrder = result.MatchingOrders;
                }
            });
        }

        private async Task LoadMatchingReceiptOrder()
        {
            await ServiceProxyFactory.DoAsync<CollationSettingMasterClient>(async client =>
            {
                var result = await client.GetMatchingReceiptOrderAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    MatchingReceiptOrder = result.MatchingOrders;
                }
            });
        }

        private void SetApplicationSetting()
        {
            pnlCurrency.Visible = UseForeignCurrency;
            if (!UseForeignCurrency)
            {
                lblDisplayPrecision.Hide();
                nmbDisplayPrecision.Hide();
                btnIncrease.Hide();
                btnDecrease.Hide();
            }

            pnlSection.Visible = UseSection;

            if (!UseCashOnDueDates)
            {
                pnlBillingData.Hide();
            }
            else
            {
                cmbBillingData.SelectedIndex = 0;
            }

            if (!UseScheduledPayment || UseDeclaredAmount)
            {
                rdoCarryBilling.Hide();
                rdoCarryReceive.Hide();
                gbxAdvance.Hide();
            }

            pnlFooter.Visible = UseForeignCurrency || (UseScheduledPayment && !UseDeclaredAmount);
        }


        /// <summary>
        /// 手数料誤差の登録有無を確認する。
        /// </summary>
        /// <param name="collation"></param>
        /// <param name="checkFee">
        /// 手数料誤差と照合結果(差額)の一致チェックまで行うか否か
        /// false : { 得意先ID／決済代行会社ID } が一致する手数料誤差が登録されているか否かを確認
        /// true  : { 得意先ID／決済代行会社ID, 通貨ID, 照合結果(差額) } が一致する手数料誤差が登録されているか否かを確認
        /// </param>
        /// <returns>
        /// 得意先/ 決済代行会社 与えられたデータから判断して存在確認を行う
        /// </returns>
        private bool IsBankTransferFeeRegistered(Collation collation, bool checkFee)
        {
            if (collation.CustomerId != 0)
                return CustomerFees.IsBankTransferFeeRegistered(collation.CustomerId, collation.CurrencyId, collation.Different, checkFee);
            if (collation.PaymentAgencyId != 0)
                return PaymentAgencyFees.IsBankTransferFeeRegistered(collation.PaymentAgencyId, collation.CurrencyId, collation.Different, checkFee);
            return false;
        }

        private async Task LoadSectionWithLoginAsync()
            => SectionsWithLoginUser = await Util.GetSectionByLoginUserIdAsync(Login);

        private async Task LoadSectionAsync()
            => Sections = await Util.GetSectionByCodesAsync(Login, codes: null);

        private async Task LoadDepartmentsAsync()
            => Departments = await Util.GetDepartmentByCodesAsync(Login, codes: null);

        private async Task LoadDepartmentsByLoginUserIdAsync()
            => DepartmentsWithLoginUser = await Util.GetDepartmentByLoginUserIdAsync(Login);

        private async Task LoadClientKeyAsync()
            => ClientKey = await Util.CreateClientKey(Login, nameof(PE0101));

        private async Task<string> GetServerPath()
            => await Util.GetGeneralSettingServerPathAsync(Login);

        #endregion

        #region 検索dialog

        private void btnDepartmentCode_Click(object sender, EventArgs e)
        {
            var allSelected = Departments.Count == DepartmentIds.Count;
            using (var form = ApplicationContext.Create(nameof(PE0114)))
            {
                var screen = form.GetAll<PE0114>().First();
                screen.AllSelected = allSelected;
                screen.InitialIds = DepartmentIds;
                screen.InitializeParentForm("請求部門絞込");

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);
                if (dialogResult == DialogResult.OK)
                {
                    ClearStatusMessage();
                    lblDepartmentName.Text = screen.SelectedState;
                    DepartmentIds = screen.SelectedIds;
                }
            }
        }

        private void btnSectionName_Click(object sender, EventArgs e)
        {
            var allSelected = Sections.Count == SectionIds.Count;

            using (var form = ApplicationContext.Create(nameof(PE0106)))
            {
                var screen = form.GetAll<PE0106>().First();
                screen.AllSection = allSelected;
                screen.InitialIds = SectionIds;

                screen.InitializeParentForm("入金部門絞込");

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (dialogResult == DialogResult.OK)
                {
                    ClearStatusMessage();
                    lblSectionName.Text = screen.SelectedState;
                    SectionIds = screen.SelectedIds;
                }
            }
        }

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                ClearStatusMessage();
                txtCurrencyCode.Text = currency.Code;
                CurrencyId = currency.Id;
            }
        }

        #endregion

        #region 小数点以下 表示桁数 変更

        private void btnIncreaseDecrease_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(sender as Button);
            var increase = btnIncrease.Equals(sender);
            var value = nmbDisplayPrecision.Value ?? 0M;
            if (increase)
                value = Math.Min(5M, value + 1M);
            else
                value = Math.Max(0M, value - 1M);
            nmbDisplayPrecision.Value = value;
            SetIncreaseDecreaseEnabled(value);
            SetFieldValue();
        }

        private void nmbDisplayPrecision_Validated(object sender, EventArgs e)
        {
            if (!nmbDisplayPrecision.Value.HasValue) return;
            var value = nmbDisplayPrecision.Value.Value;
            SetIncreaseDecreaseEnabled(value);
            // TODO: 単純に カーソル移動しただけでは、処理を行わないようにする必要あり
            SetFieldValue();
        }

        private void SetIncreaseDecreaseEnabled(decimal value)
        {
            btnIncrease.Enabled = value < 5M;
            btnDecrease.Enabled = 0M < value;
        }

        private void SetFieldValue()
        {
            if (nmbDisplayPrecision.Value.HasValue)
            {
                FormatNumber = Convert.ToInt32(nmbDisplayPrecision.Value);
            }
            var newBindingSource = new BindingSource();
            newBindingSource.DataSource = grdCollation.DataSource;

            InitializeGridTemplate();
            grid_SizeChanged(grdCollation, EventArgs.Empty);
            grdCollation.SuspendLayout();
            grdCollation.DataSource = newBindingSource;

            if (!IsMatched)
            {
                var collation = ((IEnumerable)newBindingSource).Cast<Collation>().ToList();
                InitializeGridCheckBoxEnabled(collation);
            }

            SortGridData(sortType);
            grdCollation.ResumeLayout();
        }

        #endregion

        #region event handler

        private void grdCollationResult_CellClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex < 0
                || e.CellName != "celIndividual") return;

            try
            {
                var form = ApplicationContext.Create(nameof(PE0102));
                var screen = form.GetAll<PE0102>().FirstOrDefault();
                screen.CollationSearch = CollationSearch;
                screen.UseAdvanceReceived = rdoCarryReceive.Checked;
                screen.Sections = Sections;
                screen.SectionsWithLoginUser = SectionsWithLoginUser;
                screen.SectionIds = SectionIds;
                screen.Departments = Departments;
                screen.DepartmentsWithLoginUser = DepartmentsWithLoginUser;
                screen.DepartmentIds = DepartmentIds;

                if (IsMatched)
                {
                    var header = (MatchingHeader)grdCollation.Rows[e.RowIndex].DataBoundItem;
                    screen.MatchingHeader = header;
                }
                else
                {
                    var collation = (Collation)grdCollation.Rows[e.RowIndex].DataBoundItem;
                    screen.Collation = collation;
                }
                screen.CollationGrid = grdCollation;
                screen.SelectRowIndex = e.RowIndex;
                screen.IsMatched = IsMatched;

                form.StartPosition = FormStartPosition.CenterParent;
                var result = ApplicationContext.ShowDialog(ParentForm, form);

                if (screen.UseAdvanceReceived)
                {
                    rdoCarryReceive.Checked = true;
                }
                else
                {
                    rdoCarryBilling.Checked = true;
                }

                if (result != DialogResult.OK) return;

                CustomerFees?.Clear();
                CustomerFees = null;
                PaymentAgencyFees?.Clear();
                PaymentAgencyFees = null;

                if (CollationSetting?.ReloadCollationData == 1)
                {
                    if (IsMatched)
                        SearchMatchingHeaders();
                    else
                        CollateInner();
                }
                else
                {
                    MatchedIndex = screen.MatchedCollationIndices.Count > 0 ? screen.MatchedCollationIndices : new List<int> { e.RowIndex };
                    ApplyMatchedCollationRowColor();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ApplyMatchedCollationRowColor()
        {
            foreach (int index in MatchedIndex)
            {
                if (index < 0 || grdCollation.RowCount <= index) continue;
                grdCollation.Rows[index].Cells["celUpdateFlag"].Value = true;
                var row = grdCollation.Rows[index];
                grdCollation.CurrentCellPosition = new CellPosition(index, 2);

                foreach (Cell cell in row.Cells)
                {
                    if (cell.CellIndex == 0) continue;
                    cell.Style.BackColor = ColorContext.ControlDisableBackColor;
                    if (cell.Name == "celCheckBox")
                    {
                        cell.Value = false;
                        cell.Enabled = false;
                    }
                }
                grdCollation.Rows[index].BackColor = ColorContext.ControlDisableBackColor;
            }
        }

        private void grdCollationResult_CellContentDoubleClick(object sender, CellEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.RowIndex < 0
                || !(e.CellName == "celDispCustomerCode" || e.CellName == "celDispCustomerName")) return;

            try
            {
                var result = DialogResult.Cancel;
                if (IsMatched)
                {
                    var header = (MatchingHeader)grdCollation.Rows[e.RowIndex].DataBoundItem;

                    if (string.IsNullOrEmpty(header.PaymentAgencyCode)
                        && (!string.IsNullOrEmpty(header.CustomerCode)))
                    {
                        var code = header.CustomerCode;
                        var form = ApplicationContext.Create(nameof(PB0501));
                        var screen = form.GetAll<PB0501>().FirstOrDefault();
                        form.StartPosition = FormStartPosition.CenterParent;
                        screen.CustomerCode = code;
                        screen.ReturnScreen = this;

                        result = ApplicationContext.ShowDialog(ParentForm, form);
                    }
                    else if (!string.IsNullOrEmpty(header.PaymentAgencyCode)
                        && (string.IsNullOrEmpty(header.CustomerCode)))
                    {
                        var code = header.PaymentAgencyCode;
                        var form = ApplicationContext.Create(nameof(PB1901));
                        var screen = form.GetAll<PB1901>().FirstOrDefault();
                        form.StartPosition = FormStartPosition.CenterParent;
                        screen.PaymentAgencyCode = code;
                        result = ApplicationContext.ShowDialog(ParentForm, form);
                    }
                }
                else
                {
                    var collation = (Collation)grdCollation.Rows[e.RowIndex].DataBoundItem;

                    if (string.IsNullOrEmpty(collation.PaymentAgencyCode)
                    && !string.IsNullOrEmpty(collation.CustomerCode))
                    {
                        string code = collation.CustomerCode;
                        var form = ApplicationContext.Create(nameof(PB0501));
                        var screen = form.GetAll<PB0501>().FirstOrDefault();
                        form.StartPosition = FormStartPosition.CenterParent;
                        screen.CustomerCode = code;
                        screen.ReturnScreen = this;

                        result = ApplicationContext.ShowDialog(ParentForm, form);
                    }
                    else if (!string.IsNullOrEmpty(collation.PaymentAgencyCode)
                        && (string.IsNullOrEmpty(collation.CustomerCode)))
                    {
                        string code = collation.PaymentAgencyCode;
                        var form = ApplicationContext.Create(nameof(PB1901));
                        var screen = form.GetAll<PB1901>().FirstOrDefault();
                        form.StartPosition = FormStartPosition.CenterParent;
                        screen.PaymentAgencyCode = code;
                        result = ApplicationContext.ShowDialog(ParentForm, form);
                    }
                }

                if (result == DialogResult.OK)
                {
                    //再照合
                    CustomerFees?.Clear();
                    CustomerFees = null;
                    PaymentAgencyFees?.Clear();
                    PaymentAgencyFees = null;

                    if (IsMatched)
                        SearchMatchingHeaders();
                    else
                        CollateInner();
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

                if (string.IsNullOrEmpty(txtCurrencyCode.Text))
                {
                    CurrencyId = null;
                    return;
                }

                CurrenciesResult result = null;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CurrencyMasterClient>();
                    result = await service.GetByCodeAsync(
                        SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });
                });

                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result && result.Currencies != null)
                {
                    ClearStatusMessage();
                    SetCurrency(result.Currencies.FirstOrDefault());
                }
                else
                {
                    SetCurrency(null);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetCurrency(Currency currency)
        {
            if (currency != null)
            {
                CurrencyId = currency.Id;
            }
            else
            {
                ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                txtCurrencyCode.Clear();
                CurrencyId = null;
                txtCurrencyCode.Focus();
            }
        }

        private void btnInitializeSectionSelection_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnInitializeSectionSelection);
            InitializeSectionSelection();
        }

        private void btnInitializeDepartmentSelection_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnInitializeDepartmentSelection);
            InitializeDepartmentSelection();
        }

        private void cbxShowMatched_CheckedChanged(object sender, EventArgs e)
        {
            BaseContext.SetFunction01Caption(IsMatched ? "表示" : "照合");
            OnF01ClickHandler = IsMatched ? (System.Action)DispMatchedData : Collate;

            BaseContext.SetFunction03Caption(IsMatched ? "消込解除" : "一括消込");
            OnF03ClickHandler = IsMatched ? (System.Action)CancelMatching : MatchingSequential;
            InitializeGridTemplate();
            pnlCreateAt.Visible = IsMatched;
            pnlCreateAt.Enabled = IsMatched;
        }

        #endregion

        #region その他
        private void DoClear()
        {
            ClearStatusMessage();
            datReceiptRecordedAtTo.Value = DateTime.Today; //入金日
            datBillingDueAtTo.Value = DateTime.Today.AddDays(BillingDueAtOffset); //対象範囲
            datReceiptRecordedAtFrom.Clear();
            datBillingDueAtFrom.Clear();
            txtCurrencyCode.Clear();

            cbxShowMatched.Enabled = true;
            cbxShowMatched.Checked = false;
            CurrencyId = null;
            sortType = eSortType.None;

            cmbBillingData.SelectedIndex = 0; //請求データ
            cbxShowMatched.Checked = false;
            rdoDisplayTargetAmount.Checked = true;

            grdCollation.DataSource = null;
            BaseContext.SetFunction03Caption("一括消込");
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            tbcMain.SelectedTab = tbpCollationCondition;

            if (CollationSetting.UseFromToNarrowing == 0)
            {
                datReceiptRecordedAtTo.Focus();
            }
            else
            {
                datReceiptRecordedAtFrom.Focus();
            }



            Modified = false;
        }

        private CollationSearch GetCollationOption() => new CollationSearch
        {
            CompanyId                   = CompanyId,
            ClientKey                   = ClientKey,
            RecordedAtFrom              = datReceiptRecordedAtFrom.Value,
            RecordedAtTo                = datReceiptRecordedAtTo.Value.Value.Date,
            DueAtFrom                   = datBillingDueAtFrom.Value,
            DueAtTo                     = datBillingDueAtTo.Value.Value.Date,
            CurrencyId                  = CurrencyId,
            BillingType                 = UseCashOnDueDates ? cmbBillingData.SelectedIndex : 0,
            AmountType                  = rdoDisplayTargetAmount.Checked ? 1 : 0,
            Approved                    = !UseAuthorization,
            UseDepartmentWork           = Departments.Count != DepartmentIds.Count,
            UseSectionWork              = Sections.Count != SectionIds.Count,
            LoginUserId                 = Login.UserId,
            DoTransferAdvanceReceived   = UseScheduledPayment && !UseDeclaredAmount && rdoCarryReceive.Checked,
            RecordedAtType              = CollationSetting.AdvanceReceivedRecordedDateType,
            AdvanceReceivedRecordedAt   = null,
            UseAdvanceReceived          = CollationSetting.UseAdvanceReceived == 1,
            CreateAtFrom                = datCreateAtFrom.Value,
            CreateAtTo                  = GetLastTimeOfADay(datCreateAtTo.Value),
        };

        /// <summary>指定した 日付の最終時間をセットした <see cref="DateTime"/>を取得</summary>
        private DateTime? GetLastTimeOfADay(DateTime? date)
            => date?.Date.AddDays(1).AddMilliseconds(-1);


        private IEnumerable<Row> GetCheckedRows()
            => grdCollation.Rows.Where(x => Convert.ToBoolean(x["celCheckBox"].Value));

        private TModel[] GetCheckedArray<TModel>() where TModel : class
            => GetCheckedRows().Select(x => x.DataBoundItem as TModel).ToArray();

        private bool ValidateInputValues()
        {
            if (!datReceiptRecordedAtTo.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblReceiptRecordedAt.Text))) return false;
            if (!datReceiptRecordedAtFrom.ValidateRange(datReceiptRecordedAtTo,
             () => ShowWarningDialog(MsgWngInputRangeChecked, lblReceiptRecordedAt.Text))) return false;

            if (!datBillingDueAtTo.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblBillingDueAt.Text))) return false;
            if (!datBillingDueAtFrom.ValidateRange(datBillingDueAtTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblBillingDueAt.Text))) return false;

            if (!datCreateAtFrom.ValidateRange(datCreateAtTo, () => ShowWarningDialog(MsgWngInputRangeChecked, lblCreateAt.Text))) return false;

            return true;
        }

        //検索条件設定
        private void SetSearchData()
        {
            SearchCondition = new List<object>();
            if (CollationSetting.UseFromToNarrowing == 0)
            {
                SearchCondition.Add(new SearchData(lblReceiptRecordedAt.Text, datReceiptRecordedAtTo.GetPrintValue()));
                SearchCondition.Add(new SearchData(lblBillingDueAt.Text, datBillingDueAtTo.GetPrintValue()));
            }
            else
            {
                var waveDash = " ～ ";
                SearchCondition.Add(new SearchData(lblReceiptRecordedAt.Text,
                    datReceiptRecordedAtFrom.GetPrintValue() + waveDash + datReceiptRecordedAtTo.GetPrintValue()));
                SearchCondition.Add(new SearchData(lblBillingDueAt.Text,
                    datBillingDueAtFrom.GetPrintValue() + waveDash + datBillingDueAtTo.GetPrintValue()));
            }

            if (UseForeignCurrency)
            {
                SearchCondition.Add(new SearchData(lblCurrencyCode.Text, txtCurrencyCode.GetPrintValue()));
            }
            SearchCondition.Add(new SearchData(lblDepartmentCode.Text, lblDepartmentName.GetPrintValue()));
            if (UseSection)
            {
                SearchCondition.Add(new SearchData(lblSection.Text, lblSectionName.GetPrintValue()));
            }
            if (UseCashOnDueDates)
            {
                SearchCondition.Add(new SearchData(lblBillingData.Text, cmbBillingData.GetPrintValue()));
            }
            SearchCondition.Add(new SearchData(lblDisplayValue.Text, rdoDisplayBillingAmount.Checked ? rdoDisplayBillingAmount.Text : rdoDisplayTargetAmount.Text));
        }

        private int GetCheckedType()
        {
            var allChecked = false;
            var allUnchecked = false;
            if (!IsMatched)
            {
                var collations = ((IEnumerable)grdCollation.DataSource).Cast<Collation>();
                allChecked = collations.All(item => item.Checked);
                allUnchecked = collations.All(item => !item.Checked);
            }
            else
            {
                var matchingHeaders = ((IEnumerable)grdCollation.DataSource).Cast<MatchingHeader>();
                allChecked = matchingHeaders.All(x => x.Checked);
                allUnchecked = matchingHeaders.All(x => !x.Checked);
            }

            var checkType = 0;
            //チェックOFFのデータが無かった
            if (allChecked)
            {
                checkType = 1;
            }
            //チェックONのデータが無かった場合
            if (allUnchecked)
            {
                checkType = 2;
            }
            return checkType;
        }

        private void grdCollationResult_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdCollation.CurrentCell.Name == "celCheckBox")
            {
                Modified = true;
            }
        }

        #endregion

        #region グリッドソート関連

        private void SortGridData(eSortType type = eSortType.None)
        {
            var sortItems = new List<SortItem> {
                new SortItem("celDupeCheck", SortOrder.Descending)
            };

            switch (type)
            {
                case eSortType.CheckBox:
                    sortItems.Add(new SortItem("celCheckBox", SortOrder.Descending));
                    break;
                case eSortType.AdvanceReceive_ASC:
                    sortItems.Add(new SortItem("celAdvanceReceivedCount", SortOrder.Ascending));
                    break;
                case eSortType.AdvanceReceive_DESC:
                    sortItems.Add(new SortItem("celAdvanceReceivedCount", SortOrder.Descending));
                    break;
                default:
                    break;
            }

            if (BillingPriority) //請求
            {
                sortItems.Add(new SortItem("celBillingPriority", SortOrder.Ascending));
                sortItems.Add(new SortItem("celBillingDisplayOrder", SortOrder.Ascending));
                sortItems.Add(new SortItem("celDisplayOrder", SortOrder.Ascending));
            }
            else //入金
            {
                sortItems.Add(new SortItem("celReceiptPriority", SortOrder.Ascending));
                sortItems.Add(new SortItem("celReceiptDisplayOrder", SortOrder.Ascending));
                sortItems.Add(new SortItem("celDisplayOrder", SortOrder.Ascending));
            }

            grdCollation.Sort(sortItems.ToArray());
            if (grdCollation.RowCount > 0) grdCollation.CurrentCellPosition = new CellPosition(0, "celCheckBox");
            ActiveControl = grdCollation;
        }

        private HeaderDropDownList GetHeaderDropDown(IEnumerable<DropDownItem> items)
        {
            var list = new HeaderDropDownList();
            foreach (var item in items)
                list.Items.Add(item);
            return list;
        }

        private HeaderDropDownList GetDropDownCheckCellSort()
        {
            var item1 = new DropDownSortItem();
            item1.Name = "Item1";
            item1.Text = "ソート";
            item1.SortOrder = SortOrder.Ascending;
            item1.Click += CheckDropDownItemSelectChange;

            var item2 = new DropDownSortItem();
            item2.Name = "Item2";
            item2.Text = "ソート解除";
            item2.SortOrder = SortOrder.Ascending;
            item2.Click += CheckDropDownItemSelectChange;
            return GetHeaderDropDown(new DropDownSortItem[] { item1, item2 });
        }

        private HeaderDropDownList GetDropDownAdvanceReceivedStateCellSort()
        {
            var item1 = new DropDownSortItem();
            item1.Name = "Item1";
            item1.Text = "昇順にソート";
            item1.SortOrder = SortOrder.Ascending;
            item1.Click += AdvanceReceivedDropDownItemSelectChange;

            var item2 = new DropDownSortItem();
            item2.Name = "Item2";
            item2.Text = "降順にソート";
            item2.SortOrder = SortOrder.Descending;
            item2.Click += AdvanceReceivedDropDownItemSelectChange;

            var item3 = new DropDownSortItem();
            item3.Name = "Item3";
            item3.Text = "ソート解除";
            item3.SortOrder = SortOrder.Ascending;
            item3.Click += AdvanceReceivedDropDownItemSelectChange;
            return GetHeaderDropDown(new DropDownSortItem[] { item1, item2, item3 });
        }

        private void CheckDropDownItemSelectChange(object sender, EventArgs e)
        {
            var columnHeaderCell = (ColumnHeaderCell)grdCollation.ColumnHeaders[1].Cells["lblCheckBox"];
            sortType = eSortType.None;

            if (columnHeaderCell.DropDownList.Items[0].Checked)
            {
                sortType = eSortType.CheckBox;
            }
            SortGridData(sortType);
        }

        private void AdvanceReceivedDropDownItemSelectChange(object sender, EventArgs e)
        {
            var columnHeaderCell = (ColumnHeaderCell)grdCollation.ColumnHeaders[1].Cells["lblDispAdvanceReceivedCount"];
            sortType = eSortType.None;
            if (columnHeaderCell.DropDownList.Items[0].Checked)
            {
                sortType = eSortType.AdvanceReceive_ASC;
            }
            else if (columnHeaderCell.DropDownList.Items[1].Checked)
            {
                sortType = eSortType.AdvanceReceive_DESC;
            }
            SortGridData(sortType);
        }

        #endregion

    }

}
