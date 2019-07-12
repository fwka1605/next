using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.MatchingHistoryService;
using Rac.VOne.Client.Screen.SectionMasterService;
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
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>消込履歴データ検索</summary>
    public partial class PE0301 : VOneScreenBase
    {
        private List<MatchingHistory> MatchingHistoryList { get; set; } = new List<MatchingHistory>();
        private List<object> SearchReportList { get; set; } = new List<Object>();
        private DataTable ResultTable { get; set; } = new DataTable();
        private int PayerWidth { get; set; } = 100;
        private int CurrencyId { get; set; } = 0;
        private int PrecisionLength { get; set; } = 0;
        private string ServerPath { get; set; } = null;
        private Dictionary<string, string> ColumnNameCaptionDictionary { get; set; } // ColumnName -> GridCaption
        private List<Category> BillingCategoryList { get; set; } // 請求区分コンボボックス選択肢用

        private Color TotalRowBackColor { get; } = Color.LightCyan;
        private TaskProgressManager ProgressManager;
        public VOneScreenBase ReturnScreen { get; set; }

        private IEnumerable<string> LegalPersonalities { get; set; }


        public PE0301()
        {
            InitializeComponent();
            grdSearchResult.SetupShortcutKeys();
            Text = "消込履歴データ検索";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcMatchingHistory.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcMatchingHistory.SelectedIndex == 0)
                {
                    if (ReturnScreen is PE0101)
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
        }

        #region 初期化処理
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SearchMatching;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearFormInput;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintMatching;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportMatching;

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);


            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = CloseMatching;
        }

        [OperationLog("戻る")]
        private void Return()
        {
            CloseMatching();
        }

        [OperationLog("終了")]
        private void Close()
        {
            CloseMatching();
        }

        private void InitializeGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var tmp = new Template();
            var widthRH = 40;
            var widthSecCd = UseSection ? 115 : 0;
            var widthSecNm = UseSection ? 120 : 0;
            var widthVacct = rdoDispalyVirtualAccount.Checked ? 100 : 0;
            var borderLeftDouble = new Border(
                new GrapeCity.Win.MultiRow.Line(LineStyle.Double, ColorContext.GridLineColor),
                new GrapeCity.Win.MultiRow.Line(LineStyle.Thin, ColorContext.GridLineColor),
                new GrapeCity.Win.MultiRow.Line(LineStyle.Thin, ColorContext.GridLineColor),
                new GrapeCity.Win.MultiRow.Line(LineStyle.Thin, ColorContext.GridLineColor));

            #region 表示するグリット設定
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,    widthRH, "Header"),
                new CellSetting(height,        128, "CreateAt"                  , caption: "消込日時"),
                new CellSetting(height, widthSecCd, "SectionCode"               , caption: "入金部門コード"),
                new CellSetting(height, widthSecNm, "SectionName"               , caption: "入金部門名"),
                new CellSetting(height,        115, "DepartmentCode"            , caption: "請求部門コード"),
                new CellSetting(height,        120, "DepartmentName"            , caption: "請求部門名"),
                new CellSetting(height,        115, "CustomerCode"              , caption: "得意先コード"),
                new CellSetting(height,        120, "CustomerName"              , caption: "得意先名"),
                new CellSetting(height,        115, "BilledAt"                  , caption: "請求日"),
                new CellSetting(height,        115, "SalesAt"                   , caption: "売上日"),
                new CellSetting(height,        140, "InvoiceCode"               , caption: "請求書番号"),
                new CellSetting(height,        120, "BillingCategory"           , caption: "請求区分"),
                new CellSetting(height,        120, "CollectCategory"           , caption: "回収区分"),
                new CellSetting(height,        120, "BillingAmount"             , caption: "請求額(税込)"),
                new CellSetting(height,        120, "BillingAmountExcludingTax" , caption: "請求額(税抜)"),
                new CellSetting(height,        120, "TaxAmount"                 , caption: "消費税"),
                new CellSetting(height,        120, "MatchingAmount"            , caption: "消込金額"),
                new CellSetting(height,        120, "BillingRemain"             , caption: "請求残"),
                new CellSetting(height,        160, "BillingNote1"              , caption: ColumnNameCaptionDictionary["BillingNote1"]),
                new CellSetting(height,        160, "BillingNote2"              , caption: ColumnNameCaptionDictionary["BillingNote2"]),
                new CellSetting(height,        160, "BillingNote3"              , caption: ColumnNameCaptionDictionary["BillingNote3"]),
                new CellSetting(height,        160, "BillingNote4"              , caption: ColumnNameCaptionDictionary["BillingNote4"]),
                new CellSetting(height,        115, "RecordedAt"                , caption: "入金日"),
                new CellSetting(height,         80, "ReceiptId"                 , caption: "入金ID"),
                new CellSetting(height,        120, "ReceiptCategoryCode"       , caption: "入金区分"),
                new CellSetting(height,        120, "ReceiptAmount"             , caption: "入金額"),
                new CellSetting(height,        120, "ReceiptRemain"             , caption: "入金残"),
                new CellSetting(height,        120, "PayerName"                 , caption: "振込依頼人名"),
                new CellSetting(height,        100, "BankCode"                  , caption: "銀行コード"),
                new CellSetting(height,        120, "BankName"                  , caption: "銀行名"),
                new CellSetting(height,        100, "BranchCode"                , caption: "支店コード"),
                new CellSetting(height,        120, "BranchName"                , caption: "支店名"),
                new CellSetting(height,        100, "AccountNumber"             , caption: "口座番号"),
                new CellSetting(height,        160, "ReceiptNote1"              , caption: ColumnNameCaptionDictionary["ReceiptNote1"]),
                new CellSetting(height,        160, "ReceiptNote2"              , caption: ColumnNameCaptionDictionary["ReceiptNote2"]),
                new CellSetting(height,        160, "ReceiptNote3"              , caption: ColumnNameCaptionDictionary["ReceiptNote3"]),
                new CellSetting(height,        160, "ReceiptNote4"              , caption: ColumnNameCaptionDictionary["ReceiptNote4"]),
                new CellSetting(height, widthVacct, "VirtualBranchCode"         , caption: "仮想支店コード"),
                new CellSetting(height, widthVacct, "VirtualAccountNumber"      , caption: "仮想口座番号"),
                new CellSetting(height,        160, "LoginUserName"             , caption: "消込実行ユーザー"),
                new CellSetting(height,         50, "MatchingProcessTypeString" , caption: "消込"),
                new CellSetting(height,        160, "MatchingMemo"              , caption: "消込メモ"),
            });
            builder.BuildHeaderOnly(tmp);
            builder.Items.Clear();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, Width =    widthRH, Name = "Header", cell:  builder.GetRowHeaderCell()),
                new CellSetting(height, Width =        128, Name = "CreateAt", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.CreateAt), backColor: Color.White ),
                new CellSetting(height, Width = widthSecCd, Name = "SectionCode", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.SectionCode), backColor: Color.White ),
                new CellSetting(height, Width = widthSecNm, Name = "SectionName", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.SectionName), backColor: Color.White ),
                new CellSetting(height, Width =        115, Name = "DepartmentCode", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.DepartmentCode), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "DepartmentName", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.DepartmentName), backColor: Color.White ),
                new CellSetting(height, Width =        115, Name = "CustomerCode", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.CustomerCode), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "CustomerName", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.CustomerName), backColor: Color.White ),
                new CellSetting(height, Width =        115, Name = "BilledAt", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.BilledAt), backColor: Color.White ),
                new CellSetting(height, Width =        115, Name = "SalesAt", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.SalesAt), backColor: Color.White ),
                new CellSetting(height, Width =        140, Name = "InvoiceCode", cell:  builder.GetTextBoxCell(), dataField: nameof(MatchingHistory.InvoiceCode), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "BillingCategory", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.BillingCategory), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "CollectCategory", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.CollectCategory), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "BillingAmount", cell:  builder.GetNumberCellCurrency(PrecisionLength,PrecisionLength,0), dataField: nameof(MatchingHistory.BillingAmount), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "BillingAmountExcludingTax", cell:  builder.GetNumberCellCurrency(PrecisionLength,PrecisionLength,0), dataField: nameof(MatchingHistory.BillingAmountExcludingTax), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "TaxAmount", cell:  builder.GetNumberCellCurrency(PrecisionLength,PrecisionLength,0), dataField: nameof(MatchingHistory.TaxAmount), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "MatchingAmount", cell:  builder.GetNumberCellCurrency(PrecisionLength,PrecisionLength,0), dataField: nameof(MatchingHistory.MatchingAmount), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "BillingRemain", cell:  builder.GetNumberCellCurrency(PrecisionLength,PrecisionLength,0), dataField: nameof(MatchingHistory.BillingRemain), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "BillingNote1", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.BillingNote1), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "BillingNote2", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.BillingNote2), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "BillingNote3", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.BillingNote3), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "BillingNote4", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.BillingNote4), backColor: Color.White ),
                new CellSetting(height, Width =        115, Name = "RecordedAt", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.RecordedAt), backColor: Color.White, border: borderLeftDouble ),
                new CellSetting(height, Width =         80, Name = "ReceiptId", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight), dataField: nameof(MatchingHistory.ReceiptId), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "ReceiptCategory", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.ReceiptCategory), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "ReceiptAmount", cell:  builder.GetNumberCellCurrency(PrecisionLength,PrecisionLength,0), dataField: nameof(MatchingHistory.ReceiptAmount), backColor: Color.White ),
                new CellSetting(height, Width =         95, Name = "ReceiptRemain", cell:  builder.GetNumberCellCurrency(PrecisionLength,PrecisionLength,0), dataField: nameof(MatchingHistory.ReceiptRemain), backColor: Color.White ),
                new CellSetting(height, Width =         25, Name = "AdvanceReceivedOccuredString", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.AdvanceReceivedOccuredString), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "PayerName", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.PayerName), backColor: Color.White ),
                new CellSetting(height, Width =        100, Name = "BankCode", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.BankCode), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "BankName", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.BankName), backColor: Color.White ),
                new CellSetting(height, Width =        100, Name = "BranchCode", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.BranchCode), backColor: Color.White ),
                new CellSetting(height, Width =        120, Name = "BranchName", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.BranchName), backColor: Color.White ),
                new CellSetting(height, Width =        100, Name = "AccountNumber", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.AccountNumber), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "ReceiptNote1", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.ReceiptNote1), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "ReceiptNote2", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.ReceiptNote2), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "ReceiptNote3", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.ReceiptNote3), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "ReceiptNote4", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.ReceiptNote4), backColor: Color.White ),
                new CellSetting(height, Width = widthVacct, Name = "VirtualBranchCode", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.VirtualBranchCode), backColor: Color.White ),
                new CellSetting(height, Width = widthVacct, Name = "VirtualAccountNumber", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.VirtualAccountNumber), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "LoginUserName", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.LoginUserName), backColor: Color.White ),
                new CellSetting(height, Width =         50, Name = "MatchingProcessTypeString", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: nameof(MatchingHistory.MatchingProcessTypeString), backColor: Color.White ),
                new CellSetting(height, Width =        160, Name = "MatchingMemo", cell:  builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft), dataField: nameof(MatchingHistory.MatchingMemo), backColor: Color.White ),
                new CellSetting(height, Width =          0, Name = "RowType", cell:  builder.GetTextBoxCell(),dataField: "RowType", backColor: Color.White)
            });
            builder.BuildRowOnly(tmp);

            #endregion

            #region 「消込日時（消込単位）計」行

            // 行ヘッダ
            int left = 0;
            int width = widthRH;

            var timeHeaderCell = builder.GetRowHeaderCell();
            timeHeaderCell.Size = new Size(width, height);
            timeHeaderCell.Name = "celTimeHeader";
            timeHeaderCell.Location = new Point(0, timeHeaderCell.Top);
            timeHeaderCell.Visible = false;
            timeHeaderCell.Style.Border = ColorContext.GridLine();
            tmp.Row.Cells.Add(timeHeaderCell);

            // ｢*消込日時（消込単位）計: yyyy/MM/dd HH:mm:ss｣
            left += width;                                         // ｢消込日時｣の左端まで進める
            width = 128 + widthSecCd + widthSecNm      // ｢消込日時｣～｢入金部門名｣
                  + 115 + 120 + 115 + 120 + 115 + 115 + 140 + 120 + 120; // ｢請求部門コード｣～｢回収区分｣

            var newCell = new GcTextBoxCell();
            newCell.Size = new Size(width, height);
            newCell.Name = "CreateAtSpan";
            newCell.DataField = "CreateAt";
            newCell.Location = new Point(left, newCell.Top);
            newCell.Visible = false;
            newCell.Style.Border = ColorContext.GridLine();
            newCell.Style.BackColor = TotalRowBackColor;
            newCell.ReadOnly = true;
            newCell.TabStop = false;
            tmp.Row.Cells.Add(newCell);

            // ｢備考(請求)｣～｢入金区分｣の空白
            left += width + 120 + 120 + 120 + 120 + 120;    // ｢備考(請求)｣の左端まで進める
            width = 160 + 160 + 160 + 160; // ｢備考(請求)｣～｢備考4（請求）｣

            var newCellEmpty = new GcTextBoxCell();
            newCellEmpty.Size = new Size(width, height);
            newCellEmpty.Name = "EmptySpan";
            newCellEmpty.Location = new Point(left, newCell.Top);
            newCellEmpty.Visible = false;
            newCellEmpty.Style.Border = ColorContext.GridLine();
            newCellEmpty.Style.BackColor = TotalRowBackColor;
            newCellEmpty.ReadOnly = true;
            newCellEmpty.TabStop = false;
            tmp.Row.Cells.Add(newCellEmpty);

            // ｢備考(請求)｣～｢入金区分｣の空白
            left += width;    // ｢備考(請求)｣の左端まで進める
            width = 115 + 80 + 120; // ｢入金日｣～｢入金区分｣

            var receiptEmptyCell = new GcTextBoxCell();
            receiptEmptyCell.Size = new Size(width, height);
            receiptEmptyCell.Name = "receiptEmptySpan";
            receiptEmptyCell.Location = new Point(left, newCellEmpty.Top);
            receiptEmptyCell.Visible = false;
            receiptEmptyCell.Style.Border = borderLeftDouble;
            receiptEmptyCell.Style.BackColor = TotalRowBackColor;
            receiptEmptyCell.ReadOnly = true;
            receiptEmptyCell.TabStop = false;
            tmp.Row.Cells.Add(receiptEmptyCell);

            // ｢メモ｣
            left += width + 120 + 25 + 95;                                            // ｢振込依頼人名｣の左端まで進める
            width = 120 + 100 + 120 + 100 + 120 + 100 + (4 * 160) + (2 * PayerWidth); // ｢振込依頼人名｣～｢仮想口座番号｣

            var newCellMemo = new GcTextBoxCell();
            newCellMemo.Size = new Size(width, height);
            newCellMemo.Name = "MemoSpan";
            newCellMemo.Location = new Point(left, newCell.Top);
            newCellMemo.Visible = false;
            newCellMemo.Style.Border = ColorContext.GridLine();
            newCellMemo.Style.BackColor = TotalRowBackColor;
            newCellMemo.ReadOnly = true;
            newCellMemo.TabStop = false;
            tmp.Row.Cells.Add(newCellMemo);

            #endregion

            #region 「総計」行

            // 行ヘッダ
            left = 0;
            width = widthRH;

            var totalHeaderCell = builder.GetRowHeaderCell();
            totalHeaderCell.Size = new Size(width, height);
            totalHeaderCell.Name = "celTotalHeader";
            totalHeaderCell.Location = new Point(left, totalHeaderCell.Top);
            totalHeaderCell.Visible = false;
            totalHeaderCell.Style.Border = ColorContext.GridLine();
            tmp.Row.Cells.Add(totalHeaderCell);

            // ｢*総計｣
            left += width;                                        // ｢消込日時｣の左端まで進める
            width = 128 + widthSecCd + widthSecNm     // ｢消込日時｣～｢入金部門名｣
                  + 115 + 120 + 115 + 120 + 115 + 115 + 140 + 120 + 120 // ｢請求部門コード｣～｢回収区分｣
                  + 120 + 120 + 120;                              // ｢請求額(税込)｣｢請求額(税抜)｣｢消費税｣

            var newCelltotal = new GcTextBoxCell();
            newCelltotal.Size = new Size(width, height);
            newCelltotal.Name = "CreateAtTotal";
            newCelltotal.DataField = "CreateAt";
            newCelltotal.Location = new Point(left, newCell.Top);
            newCelltotal.Visible = false;
            newCelltotal.Style.Border = ColorContext.GridLine();
            newCelltotal.Style.BackColor = TotalRowBackColor;
            newCelltotal.ReadOnly = true;
            newCelltotal.TabStop = false;
            tmp.Row.Cells.Add(newCelltotal);

            // ｢請求残｣～ 右端までの空白
            left += width + 120;                                   // ｢請求残｣の左端まで進める
            width = 120 + (4 * 160)                                // ｢請求残｣～｢備考(請求)｣
                  + 115 + 80 + 120 + 120 + 25 + 95                 // ｢入金日｣～｢入金残｣
                  + 120 + 100 + 120 + 100 + 120 + 100              // ｢振込依頼人名｣～｢口座番号｣
                  + (4 * 160) + (2 * PayerWidth) + 160 + 50 + 160; // ｢備考(入金)｣～

            var newCellLast = new GcTextBoxCell();
            newCellLast.Size = new Size(width, height);
            newCellLast.Name = "TotalLast";
            newCellLast.Location = new Point(left, newCell.Top);
            newCellLast.Visible = false;
            newCellLast.Style.Border = ColorContext.GridLine();
            newCellLast.Style.BackColor = TotalRowBackColor;
            newCellLast.ReadOnly = true;
            newCellLast.TabStop = false;
            tmp.Row.Cells.Add(newCellLast);

            #endregion

            grdSearchResult.Template = tmp;
        }

        private void PE0301_Load(object sender, EventArgs e)
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
                loadTask.Add(LoadBillingCategoryListAsync());
                loadTask.Add(LoadColumnNameSetting());
                loadTask.Add(LoadLegalPersonalities());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                SettingFormControl();
                InitializeGrid();
                InitializeBankAccountCombo();
                InitializeBillingCategoryCombo();

                SuspendLayout();
                tbcMatchingHistory.SelectedIndex = 1;
                tbcMatchingHistory.SelectedIndex = 0;
                ResumeLayout();
                ClearFormInput();

                Settings.SetCheckBoxValue<PE0301>(Login, cbxCustomer);
                Settings.SetCheckBoxValue<PE0301>(Login, cbxDepartment);
                Settings.SetCheckBoxValue<PE0301>(Login, cbxSection);
                Settings.SetCheckBoxValue<PE0301>(Login, cbxUseCorrespondDepartment);
                Settings.SetCheckBoxValue<PE0301>(Login, cbxLoginUser);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            if (ReturnScreen is PE0101)
            {
                BaseContext.SetFunction10Caption("戻る");
            }
        }



        private void InitializeBankAccountCombo()
        {
            cmbAccountType.Items.Add(new GrapeCity.Win.Editors.ListItem("すべて", 0));
            cmbAccountType.Items.Add(new GrapeCity.Win.Editors.ListItem("普通", 1));
            cmbAccountType.Items.Add(new GrapeCity.Win.Editors.ListItem("当座", 2));
            cmbAccountType.Items.Add(new GrapeCity.Win.Editors.ListItem("貯蓄", 4));
            cmbAccountType.Items.Add(new GrapeCity.Win.Editors.ListItem("通知", 5));

            if (UseForeignCurrency)
            {
                cmbAccountType.Items.Add(new GrapeCity.Win.Editors.ListItem("外貨", 8));
            }
        }

        private void InitializeBillingCategoryCombo()
        {
            cmbBillingCategory.Items.Add(new GrapeCity.Win.Editors.ListItem("すべて", 0));
            cmbBillingCategory.TextSubItemIndex = 0;
            cmbBillingCategory.ValueSubItemIndex = 1;
            cmbBillingCategory.SelectedValue = 0;

            if (BillingCategoryList == null)
            {
                throw new InvalidOperationException("BillingCategoryList is null.");
            }

            var options = BillingCategoryList.Select(c => new GrapeCity.Win.Editors.ListItem($"{c.Code}:{c.Name}", c.Id));
            cmbBillingCategory.Items.AddRange(options.ToArray());
        }

        private void SettingFormControl()
        {
            var expression = new DataExpression(ApplicationControl);

            txtCustomerFrom.Format = expression.CustomerCodeFormatString;
            txtCustomerFrom.MaxLength = expression.CustomerCodeLength;
            txtCustomerFrom.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerFrom.PaddingChar = expression.CustomerCodePaddingChar;
            txtCustomerTo.Format = expression.CustomerCodeFormatString;
            txtCustomerTo.MaxLength = expression.CustomerCodeLength;
            txtCustomerTo.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerTo.PaddingChar = expression.CustomerCodePaddingChar;

            txtDepartmentFrom.Format = expression.DepartmentCodeFormatString;
            txtDepartmentFrom.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentFrom.PaddingChar = expression.DepartmentCodePaddingChar;
            txtDepartmentTo.Format = expression.DepartmentCodeFormatString;
            txtDepartmentTo.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentTo.PaddingChar = expression.DepartmentCodePaddingChar;

            txtSectionFrom.Format = expression.SectionCodeFormatString;
            txtSectionFrom.MaxLength = expression.SectionCodeLength;
            txtSectionFrom.PaddingChar = expression.SectionCodePaddingChar;
            txtSectionTo.Format = expression.SectionCodeFormatString;
            txtSectionTo.MaxLength = expression.SectionCodeLength;
            txtSectionTo.PaddingChar = expression.SectionCodePaddingChar;

            txtLoginUserFrom.Format = expression.LoginUserCodeFormatString;
            txtLoginUserFrom.MaxLength = expression.LoginUserCodeLength;
            txtLoginUserFrom.PaddingChar = expression.LoginUserCodePaddingChar;
            txtLoginUserTo.Format = expression.LoginUserCodeFormatString;
            txtLoginUserTo.MaxLength = expression.LoginUserCodeLength;
            txtLoginUserTo.PaddingChar = expression.LoginUserCodePaddingChar;

            if (!UseSection)
            {
                txtSectionFrom.Visible = false;
                txtSectionTo.Visible = false;
                lblSection.Visible = false;
                lblSectionFrom.Visible = false;
                lblSectionTo.Visible = false;
                lblsign7.Visible = false;
                cbxSection.Visible = false;
                btnSectionFrom.Visible = false;
                btnSectionTo.Visible = false;
                cbxUseCorrespondDepartment.Visible = false;
            }

            if (!UseForeignCurrency)
            {
                pnlCurrency.Visible = false;
                nmbBillingAmountFrom.Fields.DecimalPart.MaxDigits = 0;
                nmbBillingAmountTo.Fields.DecimalPart.MaxDigits = 0;
                nmbReceiptAmountFrom.Fields.DecimalPart.MaxDigits = 0;
                nmbReceiptAmountTo.Fields.DecimalPart.MaxDigits = 0;
            }
            else
            {
                pnlCurrency.Visible = true;
                nmbBillingAmountFrom.Fields.DecimalPart.MaxDigits = 5;
                nmbBillingAmountTo.Fields.DecimalPart.MaxDigits = 5;
                nmbReceiptAmountFrom.Fields.DecimalPart.MaxDigits = 5;
                nmbReceiptAmountTo.Fields.DecimalPart.MaxDigits = 5;
            }

            txtBankCode.PaddingChar = '0';
            txtBranchCode.PaddingChar = '0';
            txtAccountNumber.PaddingChar = '0';
        }
        #endregion

        #region Webサービス呼び出し
        private async Task<string> GetCustomerName(string code)
        {
            string custName = "";
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CustomerMasterClient>();
                CustomersResult result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });

                if (result.ProcessResult.Result && result.Customers.Any(x => x != null))
                {
                    custName = result.Customers[0].Name;
                }
            });
            return custName;
        }

        private async Task<string> GetDepartmentName(string code)
        {
            var deptName = "";
            await ServiceProxyFactory.DoAsync<DepartmentMasterClient>(async client =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result && result.Departments.Any(x => x != null))
                {
                    deptName = result.Departments[0].Name;
                }
            });
            return deptName;
        }

        private async Task<string> GetSectionName(string code)
        {
            var sectionName = "";
            await ServiceProxyFactory.DoAsync<SectionMasterClient>(async client =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result
                && result.Sections.Any(x => x != null))
                {
                    sectionName = result.Sections.First().Name;
                }
            });
            return sectionName;
        }

        private async Task<string> GetLoginUserName(string code)
        {
            var loginUserName = "";
            await ServiceProxyFactory.DoAsync<LoginUserMasterClient>(async client =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result && result.Users.Any(x => x != null))
                {
                    loginUserName = result.Users[0].Name;
                }
            });
            return loginUserName;
        }

        private async Task LoadBillingCategoryListAsync()
        {
            var list = new List<Category>();
            await ServiceProxyFactory.DoAsync<CategoryMasterClient>(async client =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, Rac.VOne.Common.CategoryType.Billing, null);
                if (result.ProcessResult.Result)
                {
                    list = result.Categories;
                }
            });
            BillingCategoryList = list;
        }

        private System.Action OnCancelHandler { get; set; }
        private async Task LoadMatchingHistoryListAsync(MatchingHistorySearch searchCondition)
        {
            List<MatchingHistory> list = null;
            using (var hubConnection = HubConnectionFactory.CreateProgressHubConnection(
                () => ProgressManager.UpdateState(),
                () => ProgressManager.Abort(),
                (connection, proxy) => OnCancelHandler = () => proxy?.Invoke("Cancel", connection.ConnectionId)))
            {
                await hubConnection.Start();
                await ServiceProxyFactory.DoAsync<MatchingHistoryServiceClient>(async client =>
                {
                    var matchingResult = await client.GetAsync(SessionKey, searchCondition, hubConnection.ConnectionId);
                    if (matchingResult.ProcessResult.Result)
                        list = matchingResult.MatchingHistorys;
                });
            }
            MatchingHistoryList.AddRange(list ?? new List<MatchingHistory>());
            if (list == null)
            {
                grdSearchResult.DataSource = null;
                grdSearchResult.Refresh();
                ProgressManager.Canceled = true;
                return;
            }
            if (!MatchingHistoryList.Any())
            {
                grdSearchResult.DataSource = null;
                grdSearchResult.Refresh();
                ShowWarningDialog(MsgWngNotExistSearchData);
                ProgressManager.NotFind();
                ProgressManager.UpdateState();
                return;
            }

            tbcMatchingHistory.SelectedIndex = 1;
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction06Enabled(true);
            CreateDataTable();
            SettingGridResultTable();
            ProgressManager.UpdateState();
        }

        private async Task SaveOutputAtMatching(long[] headerIds)
        {
            await ServiceProxyFactory.DoAsync<MatchingHistoryServiceClient>(async client
                => await client.SaveOutputAtAsync(SessionKey, headerIds));
        }

        private async Task<bool> GetCurrencyDataCheck(string currencyCode)
        {
            CurrenciesResult result = null;
            await ServiceProxyFactory.DoAsync<CurrencyMasterClient>(async client =>
            {
                result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { currencyCode });
                if (result.ProcessResult.Result && result.Currencies.Any(x => x != null))
                {
                    CurrencyId = result.Currencies[0].Id;
                    PrecisionLength = result.Currencies[0].Precision;
                }
            });
            return (result.Currencies.Any() && result.Currencies[0] != null);
        }

        private void LoadServerPath()
        {
            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var path = task.Result;
            if (!string.IsNullOrEmpty(path))
                ServerPath = path;
        }

        private async Task LoadColumnNameSetting()
        {
            var columns = new List<Tuple<string, string, string, string>> // TableName, ColumnName, Key, DefaultCaption
            {
                new Tuple<string, string, string, string>("Billing", "Note1", "BillingNote1", "備考"),
                new Tuple<string, string, string, string>("Billing", "Note2", "BillingNote2", "備考2"),
                new Tuple<string, string, string, string>("Billing", "Note3", "BillingNote3", "備考3"),
                new Tuple<string, string, string, string>("Billing", "Note4", "BillingNote4", "備考4"),
                new Tuple<string, string, string, string>("Receipt", "Note1", "ReceiptNote1", "備考"),
                new Tuple<string, string, string, string>("Receipt", "Note2", "ReceiptNote2", "備考2"),
                new Tuple<string, string, string, string>("Receipt", "Note3", "ReceiptNote3", "備考3"),
                new Tuple<string, string, string, string>("Receipt", "Note4", "ReceiptNote4", "備考4"),
            };

            ColumnNameCaptionDictionary = new Dictionary<string, string>(); // ColumnName -> Caption

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<ColumnNameSettingMasterClient>();

                foreach (var column in columns)
                {
                    var result = await client.GetAsync(SessionKey, CompanyId, column.Item1, column.Item2);
                    if (result.ProcessResult.Result && result.ColumnName != null)
                    {
                        // 優先順位: DefaultCaption < OriginalName < Alias
                        if (!string.IsNullOrWhiteSpace(result.ColumnName.Alias))
                        {
                            ColumnNameCaptionDictionary[column.Item3] = result.ColumnName.Alias;
                        }
                        else if (!string.IsNullOrWhiteSpace(result.ColumnName.OriginalName))
                        {
                            ColumnNameCaptionDictionary[column.Item3] = result.ColumnName.OriginalName;
                        }
                        else
                        {
                            ColumnNameCaptionDictionary[column.Item3] = column.Item4;
                        }
                    }
                }
            });
        }
        #endregion

        #region Function Key Event
        [OperationLog("検索")]
        private void SearchMatching()
        {
            try
            {
                if (!ValidateChildren()) return;

                ClearStatusMessage();
                MatchingHistoryList.Clear();

                if (!ValidateInputData()) return;

                var list = new List<TaskProgress>()
                {
                    new TaskProgress($"{ParentForm.Text} 初期化"),
                    new TaskProgress($"{ParentForm.Text} 対象データ抽出"),
                    new TaskProgress($"{ParentForm.Text} 請求データ抽出"),
                    new TaskProgress($"{ParentForm.Text} 入金データ抽出"),
                    new TaskProgress($"{ParentForm.Text} 消込データ登録"),
                    new TaskProgress($"{ParentForm.Text} 履歴データ取得"),
                    new TaskProgress("画面の更新"),
                };
                ProgressManager = new TaskProgressManager(list);

                InitializeGrid();
                var searchCondition = CreateSearchCondition();
                var loadListTask = LoadMatchingHistoryListAsync(searchCondition);
                NLogHandler.WriteDebug(this, "消込履歴データ検索 開始");
                var dialogResult = ProgressDialogStart(ParentForm, "消込履歴データ検索", loadListTask, ProgressManager, Login, AutoCloseProgressDialog, true, OnCancelHandler);
                NLogHandler.WriteDebug(this, "消込履歴データ検索 終了");

                if (dialogResult != DialogResult.OK)
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void ClearFormInput()
        {
            tbcMatchingHistory.SelectedIndex = 0;
            RecursiveClearControls(this.Controls);
            grdSearchResult.DataSource = "";
            grdSearchResult.Refresh();
            cmbAccountType.SelectedIndex = 0;
            ClearStatusMessage();
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            this.ActiveControl = rdoMatchingCreatedOrder;
            cbxMatchingSequential.Checked = true;
            cbxMatchingIndividual.Checked = true;
            txtPayerName.Clear();
        }

        [OperationLog("印刷")]
        private void PrintMatching()
        {
            try
            {
                if (!MatchingHistoryList.Any())
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }
                ClearStatusMessage();
                var matchingHeaderIds = MatchingHistoryList.GroupBy(x => x.MatchingHeaderId).Select(g => g.Key).ToArray();
                var matchingReport = new MatchingHistorySectionReport();
                matchingReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, rdoTakeTotal.Checked);
                matchingReport.Name = "消込履歴データ一覧" + DateTime.Now.ToString("yyyyMMdd");
                var reportData = new List<ExportMatchingHistory>();
                reportData = PrepareMatchingHistoryData(true);

                var timeSort = (rdoMatchingCreatedOrder.Checked && rdoTakeTotal.Checked);
                matchingReport.SetPageDataSetting(reportData, timeSort, PrecisionLength, ColumnNameCaptionDictionary["ReceiptNote1"]);

                var searchReport = new SearchConditionSectionReport();

                searchReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "消込履歴データ 一覧");
                searchReport.Name = "消込履歴データ" + DateTime.Now.ToString("yyyyMMdd");
                searchReport.SetPageDataSetting(SearchReportList);

                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    matchingReport.Run(false);
                    searchReport.SetPageNumber(matchingReport.Document.Pages.Count);
                    searchReport.Run(false);
                }), false, SessionKey);
                matchingReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchReport.Document.Pages.Clone());

                if (ServerPath == null)
                {
                    LoadServerPath();
                }

                if (!Directory.Exists(ServerPath))
                {
                    ServerPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                Action<Form> outputHandler = owner => ProgressDialog.Start(owner, SaveOutputAtMatching(matchingHeaderIds), false, SessionKey);

                ShowDialogPreview(ParentForm, matchingReport, ServerPath, outputHandler);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("エクスポート")]
        public void ExportMatching()
        {
            try
            {
                ClearStatusMessage();
                if (ServerPath == null)
                {
                    LoadServerPath();
                }

                if (!Directory.Exists(ServerPath))
                {
                    ServerPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"消込履歴データ{DateTime.Now:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(ServerPath, fileName, out filePath)) return;

                var exportList = new List<ExportMatchingHistory>();
                var definition = new MatchingHistoryFileDefinition(new DataExpression(ApplicationControl));
                if (definition.SectionCodeField.Ignored = (!UseSection))
                {
                    definition.SectionCodeField.FieldName = null;
                }
                if (definition.SectionNameField.Ignored = (!UseSection))
                {
                    definition.SectionNameField.FieldName = null;
                }
                if (definition.VirtualBranchCode.Ignored = (rdoNotDisplayVirtualAccount.Checked))
                {
                    definition.VirtualBranchCode.FieldName = null;
                }
                if (definition.VirtualAccountNumberField.Ignored = (rdoNotDisplayVirtualAccount.Checked))
                {
                    definition.VirtualAccountNumberField.FieldName = null;
                }
                definition.BillingNote1Field.FieldName = ColumnNameCaptionDictionary["BillingNote1"];
                definition.BillingNote2Field.FieldName = ColumnNameCaptionDictionary["BillingNote2"];
                definition.BillingNote3Field.FieldName = ColumnNameCaptionDictionary["BillingNote3"];
                definition.BillingNote4Field.FieldName = ColumnNameCaptionDictionary["BillingNote4"];
                definition.ReceiptNote1Field.FieldName = ColumnNameCaptionDictionary["ReceiptNote1"];
                definition.ReceiptNote2Field.FieldName = ColumnNameCaptionDictionary["ReceiptNote2"];
                definition.ReceiptNote3Field.FieldName = ColumnNameCaptionDictionary["ReceiptNote3"];
                definition.ReceiptNote4Field.FieldName = ColumnNameCaptionDictionary["ReceiptNote4"];

                if (grdSearchResult.Columns["celBillingAmountExcludingTax"].IsCollapsed) // ｢請求額(税抜)｣列が畳まれているかで判定
                {
                    // ｢請求額(税込)｣
                    definition.BilledAmountExcludingTaxField.Ignored = true;
                    definition.TaxAmountField.Ignored = true;
                }
                else
                {
                    // 「請求金額(税込)」｢請求額(税抜)｣｢消費税｣
                    definition.BilledAmountExcludingTaxField.Ignored = false;
                    definition.TaxAmountField.Ignored = false;
                }

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;
                exportList = PrepareMatchingHistoryData(false);

                DialogResult result = ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, exportList, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                if (result == DialogResult.OK)
                {
                    long[] idList = exportList.GroupBy(x => x.MatchingHeaderId)
                        .Where(g => g.Key != null)
                        .Select(g => (long)g.Key).ToArray();

                    ProgressDialog.Start(ParentForm, SaveOutputAtMatching(idList), false, SessionKey);
                    DispStatusMessage(MsgInfFinishExport);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        public void CloseMatching()
        {
            Settings.SaveControlValue<PE0301>(ApplicationContext.Login, cbxCustomer.Name, cbxCustomer.Checked);
            Settings.SaveControlValue<PE0301>(ApplicationContext.Login, cbxDepartment.Name, cbxDepartment.Checked);
            Settings.SaveControlValue<PE0301>(ApplicationContext.Login, cbxSection.Name, cbxSection.Checked);
            Settings.SaveControlValue<PE0301>(ApplicationContext.Login, cbxUseCorrespondDepartment.Name, cbxUseCorrespondDepartment.Checked);
            Settings.SaveControlValue<PE0301>(ApplicationContext.Login, cbxLoginUser.Name, cbxLoginUser.Checked);
            BaseForm.Close();
        }
        #endregion
        private void ReturnToSearchCondition()
        {
            tbcMatchingHistory.SelectedIndex = 0;
        }

        #region Search Dialog Setting
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                if (sender == btnCustomerFrom)
                {
                    txtCustomerFrom.Text = customer.Code;
                    lblCustomerFrom.Text = customer.Name;
                    if (cbxCustomer.Checked) SetCustomerTo(customer);
                }
                else SetCustomerTo(customer);
                ClearStatusMessage();
            }
        }

        private void SetCustomerTo(CustomerMin customer)
        {
            txtCustomerTo.Text = customer.Code;
            lblCustomerTo.Text = customer.Name;
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                if (sender == btnDepartmentFrom)
                {
                    txtDepartmentFrom.Text = department.Code;
                    lblDepartmentFrom.Text = department.Name;
                    if (cbxDepartment.Checked) SetDepartmentTo(department);
                }
                else SetDepartmentTo(department);
                ClearStatusMessage();
            }
        }

        private void SetDepartmentTo(Department choiceDepartment)
        {
            txtDepartmentTo.Text = choiceDepartment.Code;
            lblDepartmentTo.Text = choiceDepartment.Name;
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                if (sender == btnSectionFrom)
                {
                    txtSectionFrom.Text = section.Code;
                    lblSectionFrom.Text = section.Name;
                    if (cbxSection.Checked) SetSectionTo(section);
                }
                else SetSectionTo(section);
                ClearStatusMessage();
            }
        }

        private void SetSectionTo(Web.Models.Section choiceSection)
        {
            txtSectionTo.Text = choiceSection.Code;
            lblSectionTo.Text = choiceSection.Name;
        }

        private void btnLoginUser_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                if (sender == btnLoginUserFrom)
                {
                    txtLoginUserFrom.Text = loginUser.Code;
                    lblLoginUserFrom.Text = loginUser.Name;
                    if (cbxLoginUser.Checked) SetLoginUserTo(loginUser);
                }
                else SetLoginUserTo(loginUser);
                ClearStatusMessage();
            }
        }

        private void SetLoginUserTo(LoginUser choiceLoginUser)
        {
            txtLoginUserTo.Text = choiceLoginUser.Code;
            lblLoginUserTo.Text = choiceLoginUser.Name;
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                CurrencyId = currency.Id;
                PrecisionLength = currency.Precision;
                ClearStatusMessage();
            }
        }

        private void btnDepositAccount_Click(object sender, EventArgs e)
        {
            var bankAccount = this.ShowBankAccountSearchDialog();
            if (bankAccount != null)
            {
                txtBankCode.Text = bankAccount.BankCode;
                txtBranchCode.Text = bankAccount.BranchCode;
                int selectedIndex = -1;
                if (bankAccount.AccountTypeId == 4) selectedIndex = 3;
                else if (bankAccount.AccountTypeId == 5) selectedIndex = 4;
                else if (bankAccount.AccountTypeId == 8) selectedIndex = 5;
                else selectedIndex = bankAccount.AccountTypeId;
                cmbAccountType.SelectedIndex = selectedIndex;
                txtAccountNumber.Text = bankAccount.AccountNumber;
            }
        }
        #endregion

        #region Form Sub Function
        private MatchingHistorySearch CreateSearchCondition()
        {
            var condition = new MatchingHistorySearch();
            SearchReportList = new List<object>();
            condition.CompanyId = CompanyId;
            condition.LoginUserId = Login.UserId;
            int sorting = 0;
            string sortOrder = "";
            if (rdoMatchingCreatedOrder.Checked)
            {
                sorting = 0;
                sortOrder = "消込日時順";
            }
            else if (rdoCustomerOrder.Checked)
            {
                sorting = 1;
                sortOrder = "得意先コード順";
            }
            else if (rdoReceiptOrder.Checked)
            {
                sorting = 2;
                sortOrder = "入金ID順";
            }
            condition.OutputOrder = sorting;
            SearchReportList.Add(new SearchData(gbxOutputOrder.Text, sortOrder));

            string createDate = "(指定なし) ～ ";
            if (datCreateAtFrom.Value.ToString() != "" && datCreateAtFrom.Value.ToString() != null)
            {
                condition.InputAtFrom = Convert.ToDateTime(datCreateAtFrom.Value);
                createDate = datCreateAtFrom.Value.ToString() + " ～ ";
            }
            if (datCreateAtTo.Value.ToString() != "" && datCreateAtTo.Value.ToString() != null)
            {
                DateTime dtInputTo = Convert.ToDateTime(datCreateAtTo.Value);
                condition.InputAtTo = dtInputTo.Date.AddDays(1).AddMilliseconds(-1);
                createDate += condition.InputAtTo.ToString();
            }
            else createDate += "(指定なし)";
            SearchReportList.Add(new SearchData(lblCreateAt.Text, createDate));

            string recordDate = "(指定なし) ～ ";
            if (datRecordedAtFrom.Value.ToString() != "" && datRecordedAtFrom.Value.ToString() != null)
            {
                condition.RecordedAtFrom = Convert.ToDateTime(datRecordedAtFrom.Value);
                recordDate = datRecordedAtFrom.Value.ToString() + " ～ ";
            }
            if (datRecordedAtTo.Value.ToString() != "" && datRecordedAtTo.Value.ToString() != null)
            {
                DateTime dtRecordTo = Convert.ToDateTime(datRecordedAtTo.Value);
                condition.RecordedAtTo = dtRecordTo.Date.AddDays(1).AddMilliseconds(-1);
                recordDate += condition.RecordedAtTo.ToString();
            }
            else recordDate += "(指定なし)";
            SearchReportList.Add(new SearchData(lblRecordedAt.Text, recordDate));

            string receiptId = "(指定なし) ～ ";
            if (nmbReceiptIdFrom.Value.ToString() != "" && nmbReceiptIdFrom.Value.ToString() != null)
            {
                condition.ReceiptIdFrom = long.Parse(nmbReceiptIdFrom.Value.ToString());
                receiptId = nmbReceiptIdFrom.Value.ToString() + " ～ ";
            }
            if (nmbReceiptIdTo.Value.ToString() != "" && nmbReceiptIdTo.Value.ToString() != null)
            {
                condition.ReceiptIdTo = long.Parse(nmbReceiptIdTo.Value.ToString());
                receiptId += nmbReceiptIdTo.Value.ToString();
            }
            else receiptId += "(指定なし)";
            SearchReportList.Add(new SearchData(lblReceiptId.Text, receiptId));

            condition.BankCode = txtBankCode.Text ?? string.Empty;
            if (txtBankCode.Text != null && txtBankCode.Text != "")
                SearchReportList.Add(new SearchData(lblBankCode.Text, txtBankCode.Text));
            else
                SearchReportList.Add(new SearchData(lblBankCode.Text, "(指定なし)"));

            condition.BranchCode = txtBranchCode.Text ?? string.Empty;
            if (txtBranchCode.Text != null && txtBranchCode.Text != "")
                SearchReportList.Add(new SearchData(lblBranchCode.Text, txtBranchCode.Text));
            else
                SearchReportList.Add(new SearchData(lblBranchCode.Text, "(指定なし)"));

            if (cmbAccountType.SelectedIndex > 0)
                condition.AccountType = Convert.ToInt32(cmbAccountType.SelectedItem.SubItems[1].Value);
            SearchReportList.Add(new SearchData(lblAccountType.Text, cmbAccountType.SelectedValue.ToString()));

            condition.AccountNumber = txtAccountNumber.Text ?? string.Empty;
            if (txtAccountNumber.Text != null && txtAccountNumber.Text != "")
                SearchReportList.Add(new SearchData(lblAccountNumber.Text, txtAccountNumber.Text));
            else
                SearchReportList.Add(new SearchData(lblAccountNumber.Text, "(指定なし)"));

            condition.CurrencyCode = txtCurrencyCode.Text ?? string.Empty;
            if (UseForeignCurrency)
            {
                if (txtAccountNumber.Text != null && txtAccountNumber.Text != "")
                    SearchReportList.Add(new SearchData(lblCurrency.Text, txtCurrencyCode.Text));
                else
                    SearchReportList.Add(new SearchData(lblCurrency.Text, "(指定なし)"));
            }

            string matchingDate = "(指定なし) ～ ";
            if (datMatchingCreateAtFrom.Value.ToString() != "" && datMatchingCreateAtFrom.Value.ToString() != null)
            {
                condition.CreateAtFrom = Convert.ToDateTime(datMatchingCreateAtFrom.Value);
                matchingDate = datMatchingCreateAtFrom.Value.ToString() + " ～ ";
            }
            if (datMatchingCreateAtTo.Value.ToString() != "" && datMatchingCreateAtTo.Value.ToString() != null)
            {
                DateTime dtCreateAtTo = Convert.ToDateTime(datMatchingCreateAtTo.Value);
                condition.CreateAtTo = dtCreateAtTo.AddSeconds(3599);
                matchingDate += condition.CreateAtTo.ToString();
            }
            else matchingDate += "(指定なし)";
            SearchReportList.Add(new SearchData(lblMatchingCreate.Text, matchingDate));

            condition.CustomerCodeFrom = txtCustomerFrom.Text ?? string.Empty;
            condition.CustomerCodeTo = txtCustomerTo.Text ?? string.Empty;

            string customerData = "(指定なし) ～ ";
            if (txtCustomerFrom.Text != null && txtCustomerFrom.Text != "")
                customerData = txtCustomerFrom.Text + " : " + lblCustomerFrom.Text + " ～ ";

            if (txtCustomerTo.Text != null && txtCustomerTo.Text != "")
                customerData += txtCustomerTo.Text + " : " + lblCustomerTo.Text;
            else
                customerData += "(指定なし)";
            SearchReportList.Add(new SearchData(lblCustomer.Text, customerData));

            condition.DepartmentCodeFrom = txtDepartmentFrom.Text ?? string.Empty;
            condition.DepartmentCodeTo = txtDepartmentTo.Text ?? string.Empty;

            string departmentData = "(指定なし) ～ ";
            if (txtDepartmentFrom.Text != null && txtDepartmentFrom.Text != "")
                departmentData = txtDepartmentFrom.Text + " : " + lblDepartmentFrom.Text + " ～ ";

            if (txtDepartmentTo.Text != null && txtDepartmentTo.Text != "")
                departmentData += txtDepartmentTo.Text + " : " + lblDepartmentTo.Text;
            else
                departmentData += "(指定なし)";
            SearchReportList.Add(new SearchData(lblDepartment.Text, departmentData));

            condition.SectionCodeFrom = txtSectionFrom.Text ?? string.Empty;
            condition.SectionCodeTo = txtSectionTo.Text ?? string.Empty;
            condition.UseSectionMaster = cbxUseCorrespondDepartment.Checked;
            if (UseSection)
            {
                string sectionData = "(指定なし) ～ ";
                if (txtSectionFrom.Text != null && txtSectionFrom.Text != "")
                    sectionData = txtSectionFrom.Text + " : " + lblSectionFrom.Text + " ～ ";

                if (txtSectionTo.Text != null && txtSectionTo.Text != "")
                    sectionData += txtSectionTo.Text + " : " + lblSectionTo.Text;
                else
                    sectionData += "(指定なし)";
                SearchReportList.Add(new SearchData(lblSection.Text, sectionData));

                string useSection = "";
                if (cbxUseCorrespondDepartment.Checked)
                    useSection = "使用";
                SearchReportList.Add(new SearchData(cbxUseCorrespondDepartment.Text, useSection));
            }

            condition.LoginUserFrom = txtLoginUserFrom.Text ?? string.Empty;
            condition.LoginUserTo = txtLoginUserTo.Text ?? string.Empty;

            string loginData = "(指定なし) ～ ";
            if (txtLoginUserFrom.Text != null && txtLoginUserFrom.Text != "")
                loginData = txtLoginUserFrom.Text + " : " + lblLoginUserFrom.Text + " ～ ";

            if (txtLoginUserTo.Text != null && txtLoginUserTo.Text != "")
                loginData += txtLoginUserTo.Text + " : " + lblLoginUserTo.Text;
            else
                loginData += "(指定なし)";
            SearchReportList.Add(new SearchData(lblLoginUser.Text, loginData));

            condition.ExistsMemo = cbxMemo.Checked;

            string exitMemo = "";
            if (cbxMemo.Checked)
                exitMemo = "有り";
            SearchReportList.Add(new SearchData(cbxMemo.Text, exitMemo));

            condition.Memo = txtMemo.Text ?? string.Empty;
            if (txtMemo.Text != null && txtMemo.Text != "")
                SearchReportList.Add(new SearchData("消込メモ", txtMemo.Text));
            else
                SearchReportList.Add(new SearchData("消込メモ", "(指定なし)"));

            if ( cbxMatchingSequential.Checked && !cbxMatchingIndividual.Checked)
                SearchReportList.Add(new SearchData("消込種別",cbxMatchingSequential.Text));
            if (!cbxMatchingSequential.Checked &&  cbxMatchingIndividual.Checked)
                SearchReportList.Add(new SearchData("消込種別", cbxMatchingIndividual.Text));
            if ( cbxMatchingSequential.Checked &&  cbxMatchingIndividual.Checked)
                SearchReportList.Add(new SearchData("消込種別", cbxMatchingSequential.Text + "/" + cbxMatchingIndividual.Text));

            string BillingAmount = "(指定なし) ～ ";
            if (nmbBillingAmountFrom.Value.ToString() != "" && nmbBillingAmountFrom.Value.ToString() != null)
            {
                condition.BillingAmountFrom = Convert.ToDecimal(nmbBillingAmountFrom.Value);
                BillingAmount = nmbBillingAmountFrom.Value.ToString() + " ～ ";
            }
            if (nmbBillingAmountTo.Value.ToString() != "" && nmbBillingAmountTo.Value.ToString() != null)
            {
                condition.BillingAmountTo = Convert.ToDecimal(nmbBillingAmountTo.Value);
                BillingAmount += nmbBillingAmountTo.Value.ToString();
            }
            else BillingAmount += "(指定なし)";
            SearchReportList.Add(new SearchData(lblBillingAmount.Text, BillingAmount));

            string receiptAmount = "(指定なし) ～ ";
            if (nmbReceiptAmountFrom.Value.ToString() != "" && nmbReceiptAmountFrom.Value.ToString() != null)
            {
                condition.ReceiptAmountFrom = Convert.ToDecimal(nmbReceiptAmountFrom.Value);
                receiptAmount = nmbReceiptAmountFrom.Value.ToString() + " ～ ";
            }
            if (nmbReceiptAmountTo.Value.ToString() != "" && nmbReceiptAmountTo.Value.ToString() != null)
            {
                condition.ReceiptAmountTo = Convert.ToDecimal(nmbReceiptAmountTo.Value);
                receiptAmount += nmbReceiptAmountTo.Value.ToString();
            }
            else receiptAmount += "(指定なし)";
            SearchReportList.Add(new SearchData(lblReceiptAmount.Text, receiptAmount));

            int billintCategoryId = (int)cmbBillingCategory.SelectedValue;
            condition.BillingCategoryId = billintCategoryId != 0 ? billintCategoryId : (int?)null;
            SearchReportList.Add(new SearchData(lblBillingCategory.Text, cmbBillingCategory.Text));

            SearchReportList.Add(new SearchData(lblPayerName.Text, txtPayerName.Text));

            condition.OnlyNonOutput = cbxOutputAt.Checked;
            string output = "";
            if (cbxOutputAt.Checked)
                output = "未出力";
            SearchReportList.Add(new SearchData(cbxOutputAt.Text, output));

            if ( cbxMatchingSequential.Checked && !cbxMatchingIndividual.Checked)
            {
                condition.MatchingProcessType = 0;
            }
            if (!cbxMatchingSequential.Checked &&  cbxMatchingIndividual.Checked)
            {
                condition.MatchingProcessType = 1;
            }

            if (txtPayerName != null && txtPayerName.Text != "")
            {
                condition.PayerName = txtPayerName.Text;
            }

            return condition;
        }

        private bool ValidateInputData()
        {
            Func<Common.Controls.VOneDateControl, Common.Controls.VOneDateControl,
                string, bool> ValidDateRange = (datetFrom, dateTo, label)
                => datetFrom.ValidateRange(dateTo, () => ShowWarningDialog(MsgWngInputRangeChecked, label));

            Func<Common.Controls.VOneDateTimeControl, Common.Controls.VOneDateTimeControl,
                string, bool> ValidDateTimeRange = (dateFrom, dateTo, label)
                => dateFrom.ValidateRange(dateTo, () => ShowWarningDialog(MsgWngInputRangeChecked, label, null));

            Func<Common.Controls.VOneTextControl, Common.Controls.VOneTextControl,
                string, bool> ValidTextFieldRange = (textFrom, textTo, label)
                => textFrom.ValidateRange(textTo, () => ShowWarningDialog(MsgWngInputRangeChecked, label, null));

            Func<Common.Controls.VOneNumberControl, Common.Controls.VOneNumberControl,
                string, bool> ValidNumberFieldRange = (numberFrom, numberTo, label)
                => numberFrom.ValidateRange(numberTo, () => ShowWarningDialog(MsgWngInputRangeChecked, label, null));

            if (UseForeignCurrency && string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text, null);
                this.ActiveControl = txtCurrencyCode;
                return false;
            }

            if (!ValidDateRange(datCreateAtFrom, datCreateAtTo, lblCreateAt.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidDateRange(datRecordedAtFrom, datRecordedAtTo, lblRecordedAt.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidNumberFieldRange(nmbReceiptIdFrom, nmbReceiptIdTo, lblReceiptId.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidDateTimeRange(datMatchingCreateAtFrom, datMatchingCreateAtTo, lblMatchingCreate.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidTextFieldRange(txtCustomerFrom, txtCustomerTo, lblCustomer.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidTextFieldRange(txtDepartmentFrom, txtDepartmentTo, lblDepartment.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidTextFieldRange(txtSectionFrom, txtSectionTo, lblSection.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidTextFieldRange(txtLoginUserFrom, txtLoginUserTo, lblLoginUser.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidNumberFieldRange(nmbBillingAmountFrom, nmbBillingAmountTo, lblBillingAmount.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!ValidNumberFieldRange(nmbReceiptAmountFrom, nmbReceiptAmountTo, lblReceiptAmount.Text))
            {
                tbcMatchingHistory.SelectedIndex = 0;
                return false;
            }

            if (!cbxMatchingSequential.Checked && !cbxMatchingIndividual.Checked)
            {
                cbxMatchingSequential.Focus();
                ShowWarningDialog(MsgWngSelectionRequired, lblMatchingProcessType.Text);
                return false;
            }

            return true;
        }

        private void RecursiveClearControls(ControlCollection formControl)
        {
            foreach (Control ctrl in formControl)
            {
                if (ctrl is Common.Controls.VOneTextControl)
                    ctrl.ResetText();
                else if (ctrl is Common.Controls.VOneComboControl)
                    ctrl.ResetText();
                else if (ctrl is Common.Controls.VOneDispLabelControl)
                    ctrl.ResetText();
                else if (ctrl is Common.Controls.VOneNumberControl)
                    ctrl.ResetText();
                else if (ctrl is Common.Controls.VOneDateControl)
                    ctrl.ResetText();
                else if (ctrl is Common.Controls.VOneDateTimeControl)
                    ctrl.ResetText();
                else
                    RecursiveClearControls(ctrl.Controls);
            }
            rdoMatchingCreatedOrder.Checked = true;
            rdoDispalyVirtualAccount.Checked = true;
            rdoTakeTotal.Checked = true;
            cbxOutputAt.Checked = true;
            cbxMemo.Checked = false;
        }

        /// <summary>検索結果をグリットに表示するためDataTable化に変更
        /// 特殊な請求区分・入金区分の設定
        /// 時計と総計の計算
        /// </summary>
        private void CreateDataTable()
        {
            //TODO: MatchingHistoryList を DataSource にできないか
            ResultTable = new DataTable();
            #region Table initialize
            ResultTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("CreateAt", typeof(string)),
                new DataColumn("CreateAtSource", typeof(string)),
                new DataColumn("SectionCode", typeof(string)),
                new DataColumn("SectionName", typeof(string)),
                new DataColumn("DepartmentCode", typeof(string)),
                new DataColumn("DepartmentName", typeof(string)),
                new DataColumn("CustomerCode", typeof(string)),
                new DataColumn("CustomerName", typeof(string)),
                new DataColumn("BilledAt", typeof(string)),
                new DataColumn("SalesAt", typeof(string)),
                new DataColumn("InvoiceCode", typeof(string)),
                new DataColumn("BillingCategory", typeof(string)),
                new DataColumn("CollectCategory", typeof(string)),
                new DataColumn("BillingAmount", typeof(decimal)),
                new DataColumn("BillingAmountExcludingTax", typeof(decimal)),
                new DataColumn("TaxAmount", typeof(decimal)),
                new DataColumn("MatchingAmount", typeof(decimal)),
                new DataColumn("BillingRemain", typeof(decimal)),
                new DataColumn("BillingNote1", typeof(string)),
                new DataColumn("BillingNote2", typeof(string)),
                new DataColumn("BillingNote3", typeof(string)),
                new DataColumn("BillingNote4", typeof(string)),
                new DataColumn("RecordedAt", typeof(string)),
                new DataColumn("ReceiptId", typeof(string)),
                new DataColumn("ReceiptCategory", typeof(string)),
                new DataColumn("ReceiptAmount", typeof(decimal)),
                new DataColumn("AdvanceReceivedOccuredString", typeof(string)),
                new DataColumn("ReceiptRemain", typeof(decimal)),
                new DataColumn("PayerName", typeof(string)),
                new DataColumn("BankCode", typeof(string)),
                new DataColumn("BankName", typeof(string)),
                new DataColumn("BranchCode", typeof(string)),
                new DataColumn("BranchName", typeof(string)),
                new DataColumn("AccountNumber", typeof(string)),
                new DataColumn("ReceiptNote1", typeof(string)),
                new DataColumn("ReceiptNote2", typeof(string)),
                new DataColumn("ReceiptNote3", typeof(string)),
                new DataColumn("ReceiptNote4", typeof(string)),
                new DataColumn("VirtualBranchCode", typeof(string)),
                new DataColumn("VirtualAccountNumber", typeof(string)),
                new DataColumn("LoginUserName", typeof(string)),
                new DataColumn("MatchingProcessTypeString", typeof(string)),
                new DataColumn("MatchingMemo", typeof(string)),
                new DataColumn("RowType", typeof(int)),
                new DataColumn("MatchingHeaderId", typeof(long))
            });
            #endregion
            DataRow dataRow = null;
            var compare = new MatchingHistory();
            var billingAmountSum = 0m;
            var billingAmountExcludingTaxSum = 0m;
            var taxAmountSum = 0m;
            var matchingAmountSum = 0m;
            var billingRemainSum = 0m;
            var receiptAmountSum = 0m;
            var receiptRemainSum = 0m;
            var matchingAmountGrandTotal = 0m;

            foreach (var key in MatchingHistoryList)
            {
                bool same = false;

                if (compare.MatchingHeaderId > 0)
                {
                    if (compare.CreateAt == key.CreateAt)
                    {
                        same = true;
                    }
                    else if (rdoTakeTotal.Checked)
                    {
                        dataRow = ResultTable.NewRow();
                        dataRow["CreateAt"] = "*消込日時（消込単位）計:" + compare.CreateAt;
                        dataRow["BillingAmount"] = billingAmountSum;
                        dataRow["BillingAmountExcludingTax"] = billingAmountExcludingTaxSum;
                        dataRow["TaxAmount"] = taxAmountSum;
                        dataRow["MatchingAmount"] = matchingAmountSum;
                        dataRow["BillingRemain"] = billingRemainSum;
                        dataRow["ReceiptAmount"] = receiptAmountSum;
                        dataRow["ReceiptRemain"] = receiptRemainSum;
                        dataRow["MatchingMemo"] = compare.MatchingMemo;
                        dataRow["LoginUserName"] = compare.LoginUserName;
                        dataRow["MatchingProcessTypeString"] = compare.MatchingProcessTypeString;
                        dataRow["RowType"] = 1;
                        ResultTable.Rows.Add(dataRow);

                        billingAmountSum = 0m;
                        billingAmountExcludingTaxSum = 0m;
                        taxAmountSum = 0m;
                        matchingAmountSum = 0m;
                        billingRemainSum = 0m;
                        receiptAmountSum = 0m;
                        receiptRemainSum = 0m;
                    }
                }

                dataRow = ResultTable.NewRow();
                dataRow["MatchingHeaderId"] = key.MatchingHeaderId;
                dataRow["CreateAt"] = (same == false) ? key.CreateAt : (DateTime?)null;
                dataRow["CreateAtSource"] = key.CreateAt;
                if (compare.MatchingHeaderId > 0 && same)
                {
                    if (compare.SectionCode != key.SectionCode)
                    {
                        dataRow["SectionCode"] = key.SectionCode;
                        dataRow["SectionName"] = key.SectionName;
                    }
                    if (compare.DepartmentCode != key.DepartmentCode)
                    {
                        dataRow["DepartmentCode"] = key.DepartmentCode;
                        dataRow["DepartmentName"] = key.DepartmentName;
                    }
                    if (compare.CustomerCode != key.CustomerCode)
                    {
                        dataRow["CustomerCode"] = key.CustomerCode;
                        dataRow["CustomerName"] = key.CustomerName;
                    }
                }
                else
                {
                    dataRow["SectionCode"] = (same == false) ? key.SectionCode : string.Empty;
                    dataRow["SectionName"] = (same == false) ? key.SectionName : string.Empty;
                    dataRow["DepartmentCode"] = (same == false) ? key.DepartmentCode : string.Empty;
                    dataRow["DepartmentName"] = (same == false) ? key.DepartmentName : string.Empty;
                    dataRow["CustomerCode"] = (same == false) ? key.CustomerCode : string.Empty;
                    dataRow["CustomerName"] = (same == false) ? key.CustomerName : string.Empty;
                }

                if (key.BilledAt != null)
                {
                    dataRow["BilledAt"] = DateTime.Parse(key.BilledAt.ToString()).ToShortDateString();
                    dataRow["SalesAt"] = DateTime.Parse(key.SalesAt.ToString()).ToShortDateString();
                    dataRow["InvoiceCode"] = key.InvoiceCode;
                    dataRow["BillingCategory"] = key.BillingCategory;
                    dataRow["CollectCategory"] = key.CollectCategory;
                    if (key.BillingAmount.HasValue)
                    {
                        dataRow["BillingAmount"] = key.BillingAmount;
                        dataRow["BillingAmountExcludingTax"] = key.BillingAmountExcludingTax;
                        dataRow["TaxAmount"] = key.TaxAmount;
                        billingAmountSum += key.BillingAmount.Value;
                        billingAmountExcludingTaxSum += key.BillingAmountExcludingTax.Value;
                        taxAmountSum += key.TaxAmount.Value;
                    }
                    if (key.BillingRemain.HasValue)
                    {
                        dataRow["BillingRemain"] = key.BillingRemain;
                        billingRemainSum += key.BillingRemain.Value;
                    }
                    if (key.MatchingAmount.HasValue)
                    {
                        dataRow["MatchingAmount"] = key.MatchingAmount;
                        matchingAmountSum += key.MatchingAmount.Value;
                    }
                    dataRow["BillingNote1"] = key.BillingNote1;
                    dataRow["BillingNote2"] = key.BillingNote2;
                    dataRow["BillingNote3"] = key.BillingNote3;
                    dataRow["BillingNote4"] = key.BillingNote4;
                }

                if (key.RecordedAt != null)
                {
                    dataRow["RecordedAt"] = DateTime.Parse(key.RecordedAt.ToString()).ToShortDateString();
                    dataRow["ReceiptId"] = key.ReceiptId;
                    dataRow["ReceiptCategory"] = key.ReceiptCategory;
                    dataRow["AdvanceReceivedOccuredString"] = key.AdvanceReceivedOccuredString;
                    dataRow["PayerName"] = key.PayerName;
                    dataRow["BankCode"] = key.BankCode;
                    dataRow["BankName"] = key.BankName;
                    dataRow["BranchCode"] = key.BranchCode;
                    dataRow["BranchName"] = key.BranchName;
                    dataRow["AccountNumber"] = key.AccountNumber;
                    dataRow["ReceiptNote1"] = key.ReceiptNote1;
                    dataRow["ReceiptNote2"] = key.ReceiptNote2;
                    dataRow["ReceiptNote3"] = key.ReceiptNote3;
                    dataRow["ReceiptNote4"] = key.ReceiptNote4;
                    dataRow["VirtualBranchCode"] = key.VirtualBranchCode;
                    dataRow["VirtualAccountNumber"] = key.VirtualAccountNumber;
                    dataRow["LoginUserName"] = key.LoginUserName;
                    dataRow["MatchingProcessTypeString"] = key.MatchingProcessTypeString;
                    dataRow["MatchingMemo"] = key.MatchingMemo;

                    if (key.ReceiptAmount.HasValue)
                    {
                        dataRow["ReceiptAmount"] = key.ReceiptAmount;
                        receiptAmountSum += key.ReceiptAmount.Value;
                    }
                    if (key.ReceiptRemain.HasValue)
                    {
                        dataRow["ReceiptRemain"] = key.ReceiptRemain;
                        receiptRemainSum += key.ReceiptRemain.Value;
                    }
                }

                dataRow["RowType"] = 0;
                ResultTable.Rows.Add(dataRow);
                if (key.MatchingAmount.HasValue) matchingAmountGrandTotal += key.MatchingAmount.Value;

                compare = key;
            }

            if (rdoTakeTotal.Checked)
            {
                dataRow = ResultTable.NewRow();
                dataRow["CreateAt"] = "*消込日時（消込単位）計:" + compare.CreateAt;
                dataRow["BillingAmount"] = billingAmountSum;
                dataRow["BillingAmountExcludingTax"] = billingAmountExcludingTaxSum;
                dataRow["TaxAmount"] = taxAmountSum;
                dataRow["MatchingAmount"] = matchingAmountSum;
                dataRow["BillingRemain"] = billingRemainSum;
                dataRow["ReceiptAmount"] = receiptAmountSum;
                dataRow["ReceiptRemain"] = receiptRemainSum;
                dataRow["MatchingMemo"] = compare.MatchingMemo;
                dataRow["LoginUserName"] = compare.LoginUserName;
                dataRow["MatchingProcessTypeString"] = compare.MatchingProcessTypeString;
                dataRow["RowType"] = 1;
                ResultTable.Rows.Add(dataRow);
            }

            dataRow = ResultTable.NewRow();
            dataRow["CreateAt"] = "*総計*";
            dataRow["MatchingAmount"] = matchingAmountGrandTotal; //MatchingHistoryList.Sum(x => x.MatchingAmount);
            dataRow["RowType"] = 2;
            ResultTable.Rows.Add(dataRow);
            grdSearchResult.DataSource = ResultTable;
        }

        private void SettingGridResultTable()
        {
            if (rdoTaxIncluded.Checked)
            {
                // ｢請求額(税込)｣
                grdSearchResult.Columns["celBillingAmountExcludingTax"].Collapse();
                grdSearchResult.Columns["celTaxAmount"].Collapse();
            }
            else
            {
                // 「請求金額(税込)」｢請求額(税抜)｣｢消費税｣
                grdSearchResult.Columns["celBillingAmountExcludingTax"].Expand();
                grdSearchResult.Columns["celTaxAmount"].Expand();
            }

            foreach (var row in grdSearchResult.Rows)
            {
                var rowType = (int)row.Cells["celRowType"].Value;

                // 消込日時（消込単位）計
                if (rowType == 1)
                {
                    row.Cells["celTimeHeader"].Visible = true;
                    row.Cells["CreateAtSpan"].Visible = true;
                    row.Cells["EmptySpan"].Visible = true;
                    row.Cells["receiptEmptySpan"].Visible = true;
                    row.Cells["MemoSpan"].Visible = true;

                    row.Cells["celBillingAmount"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celBillingAmountExcludingTax"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celTaxAmount"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celMatchingAmount"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celBillingRemain"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celReceiptRemain"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celReceiptAmount"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celAdvanceReceivedOccuredString"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celLoginUserName"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celMatchingProcessTypeString"].Style.BackColor = TotalRowBackColor;
                    row.Cells["celMatchingMemo"].Style.BackColor = TotalRowBackColor;
                }

                // 最終行(総計)
                if (row.Index == grdSearchResult.RowCount - 1)
                {
                    row.Cells["celTotalHeader"].Visible = true;
                    row.Cells["CreateAtTotal"].Visible = true;
                    row.Cells["TotalLast"].Visible = true;

                    row.Cells["celLoginUserName"].Visible = false;
                    row.Cells["celMatchingProcessTypeString"].Visible = false;

                    row.Cells["celMatchingAmount"].Style.BackColor = TotalRowBackColor;
                }

                // 消込日時（消込単位）計／総計
                if (rowType != 0)
                {
                    row.Cells["celHeader"].Visible = false;
                    row.Cells["celCreateAt"].Visible = false;
                    row.Cells["celSectionCode"].Visible = false;
                    row.Cells["celSectionName"].Visible = false;
                    row.Cells["celDepartmentCode"].Visible = false;
                    row.Cells["celDepartmentName"].Visible = false;
                    row.Cells["celCustomerCode"].Visible = false;
                    row.Cells["celCustomerName"].Visible = false;
                    row.Cells["celBilledAt"].Visible = false;
                    row.Cells["celSalesAt"].Visible = false;
                    row.Cells["celInvoiceCode"].Visible = false;
                    row.Cells["celBillingCategory"].Visible = false;
                    row.Cells["celBillingNote1"].Visible = false;
                    row.Cells["celBillingNote2"].Visible = false;
                    row.Cells["celBillingNote3"].Visible = false;
                    row.Cells["celBillingNote4"].Visible = false;
                    row.Cells["celRecordedAt"].Visible = false;
                    row.Cells["celReceiptId"].Visible = false;
                    row.Cells["celReceiptCategory"].Visible = false;
                    row.Cells["celPayerName"].Visible = false;
                    row.Cells["celBankCode"].Visible = false;
                    row.Cells["celBankName"].Visible = false;
                    row.Cells["celBranchCode"].Visible = false;
                    row.Cells["celBranchName"].Visible = false;
                    row.Cells["celAccountNumber"].Visible = false;
                    row.Cells["celReceiptNote1"].Visible = false;
                    row.Cells["celReceiptNote2"].Visible = false;
                    row.Cells["celReceiptNote3"].Visible = false;
                    row.Cells["celReceiptNote4"].Visible = false;
                    row.Cells["celVirtualBranchCode"].Visible = false;
                    row.Cells["celVirtualAccountNumber"].Visible = false;
                }
            }
        }

        /// <summary>帳票とエクスポートのためデータをExportMatchingHistoryリストに設定</summary>
        private List<ExportMatchingHistory> PrepareMatchingHistoryData(bool isPrint)
        {
            var source = new List<ExportMatchingHistory>();
            foreach (DataRow dr in ResultTable.Rows)
            {
                if (isPrint && !dr["RowType"].Equals(0))
                {
                    continue;
                }

                var history = new ExportMatchingHistory();
                if (dr["MatchingHeaderId"].ToString() != "")
                    history.MatchingHeaderId = Convert.ToInt64(dr["MatchingHeaderId"].ToString());
                history.CreateAt = dr["CreateAt"].ToString();
                history.CreateAtSource = dr["CreateAtSource"].ToString();
                history.SectionCode = dr["SectionCode"].ToString();
                history.SectionName = dr["SectionName"].ToString();
                history.DepartmentCode = dr["DepartmentCode"].ToString();
                history.DepartmentName = dr["DepartmentName"].ToString();
                history.CustomerCode = dr["CustomerCode"].ToString();
                history.CustomerName = dr["CustomerName"].ToString();
                history.BilledAt = dr["BilledAt"].ToString();
                history.SalesAt = dr["SalesAt"].ToString();
                history.InvoiceCode = dr["InvoiceCode"].ToString();
                history.BillingCategory = dr["BillingCategory"].ToString();
                history.CollectCategory = dr["CollectCategory"].ToString();
                history.BillingNote1 = dr["BillingNote1"].ToString();
                history.BillingNote2 = dr["BillingNote2"].ToString();
                history.BillingNote3 = dr["BillingNote3"].ToString();
                history.BillingNote4 = dr["BillingNote4"].ToString();
                history.RecordedAt = dr["RecordedAt"].ToString();
                history.ReceiptId = dr["ReceiptId"].ToString();
                history.ReceiptCategory = dr["ReceiptCategory"].ToString();
                history.PayerName = dr["PayerName"].ToString();
                history.BankCode = dr["BankCode"].ToString();
                history.BankName = dr["BankName"].ToString();
                history.BranchCode = dr["BranchCode"].ToString();
                history.BranchName = dr["BranchName"].ToString();
                history.AccountNumber = dr["AccountNumber"].ToString();
                history.ReceiptNote1 = dr["ReceiptNote1"].ToString();
                history.ReceiptNote2 = dr["ReceiptNote2"].ToString();
                history.ReceiptNote3 = dr["ReceiptNote3"].ToString();
                history.ReceiptNote4 = dr["ReceiptNote4"].ToString();
                history.VirtualBranchCode = dr["VirtualBranchCode"].ToString();
                history.VirtualAccountNumber = dr["VirtualAccountNumber"].ToString();
                history.LoginUserName = dr["LoginUserName"].ToString();
                history.MatchingProcessTypeString = dr["MatchingProcessTypeString"].ToString();
                history.MatchingMemo = dr["MatchingMemo"].ToString();

                if (dr["BillingAmount"].ToString() != "")
                {
                    history.BillingAmount = Math.Round(decimal.Parse(dr["BillingAmount"].ToString()), PrecisionLength).ToString();
                }
                if (dr["BillingAmountExcludingTax"].ToString() != "")
                {
                    history.BillingAmountExcludingTax = Math.Round(decimal.Parse(dr["BillingAmountExcludingTax"].ToString()), PrecisionLength).ToString();
                }
                if (dr["TaxAmount"].ToString() != "")
                {
                    history.TaxAmount = Math.Round(decimal.Parse(dr["TaxAmount"].ToString()), PrecisionLength).ToString();
                }
                if (dr["MatchingAmount"].ToString() != "")
                {
                    history.MatchingAmount = Math.Round(Convert.ToDecimal(dr["MatchingAmount"].ToString()), PrecisionLength).ToString();
                }
                if (dr["BillingRemain"].ToString() != "")
                {
                    history.BillingRemain = Math.Round(Convert.ToDecimal(dr["BillingRemain"].ToString()), PrecisionLength).ToString();
                }
                if (dr["ReceiptAmount"].ToString() != "")
                {
                    history.ReceiptAmount = Math.Round(Convert.ToDecimal(dr["ReceiptAmount"].ToString()), PrecisionLength).ToString();
                }
                if (dr["AdvanceReceivedOccuredString"].ToString() != "")
                {
                    history.AdvanceReceivedOccuredString = dr["AdvanceReceivedOccuredString"].ToString();
                }
                if (dr["ReceiptRemain"].ToString() != "")
                {
                    history.ReceiptRemain = Math.Round(Convert.ToDecimal(dr["ReceiptRemain"].ToString()), PrecisionLength).ToString();
                }

                source.Add(history);
            }
            return source;
        }
        #endregion

        #region Control Properties Change Event
        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            string rdoName = ((RadioButton)sender).Name;
            if (rdoName == "rdoMatchingCreatedOrder")
            {
                rdoTakeTotal.Enabled = true;
                rdoTakeTotal.Checked = true;
                rdoNoTakeTotal.Enabled = true;
                rdoNoTakeTotal.Checked = false;
            }
            else
            {
                rdoTakeTotal.Enabled = false;
                rdoTakeTotal.Checked = false;
                rdoNoTakeTotal.Enabled = false;
                rdoNoTakeTotal.Checked = true;
            }
        }

        private void txtCustomerFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                string name = "";
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtCustomerFrom.Text))
                {
                    SetCustomer(null, null);
                    return;
                }
                Task<string> loadTask = GetCustomerName(txtCustomerFrom.Text);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                name = loadTask.Result;
                SetCustomer(txtCustomerFrom.Text, name);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetCustomer(string code, string name)
        {
            txtCustomerFrom.Text = code;
            lblCustomerFrom.Text = name;
            if (cbxCustomer.Checked)
            {
                txtCustomerTo.Text = code;
                lblCustomerTo.Text = name;
            }
        }

        private void txtCustomerTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtCustomerTo.Text))
                {
                    txtCustomerTo.Clear();
                    lblCustomerTo.Clear();
                    return;
                }
                Task<string> loadTask = GetCustomerName(txtCustomerTo.Text);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                lblCustomerTo.Text = loadTask.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                string name = "";
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtDepartmentFrom.Text))
                {
                    SetDepartment(null, null);
                    return;
                }
                Task<string> loadTask = GetDepartmentName(txtDepartmentFrom.Text);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                name = loadTask.Result;
                SetDepartment(txtDepartmentFrom.Text, name);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetDepartment(string code, string name)
        {
            txtDepartmentFrom.Text = code;
            lblDepartmentFrom.Text = name;
            if (cbxDepartment.Checked)
            {
                txtDepartmentTo.Text = code;
                lblDepartmentTo.Text = name;
            }
        }

        private void txtDepartmentTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtDepartmentTo.Text))
                {
                    txtDepartmentTo.Clear();
                    lblDepartmentTo.Clear();
                    return;
                }
                Task<string> loadTask = GetDepartmentName(txtDepartmentTo.Text);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                lblDepartmentTo.Text = loadTask.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtSectionFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                string name = "";
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtSectionFrom.Text))
                {
                    SetSection(null, null);
                    return;
                }
                Task<string> loadTask = GetSectionName(txtSectionFrom.Text);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                name = loadTask.Result;
                SetSection(txtSectionFrom.Text, name);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetSection(string code, string name)
        {
            txtSectionFrom.Text = code;
            lblSectionFrom.Text = name;
            if (cbxSection.Checked)
            {
                txtSectionTo.Text = code;
                lblSectionTo.Text = name;
            }
        }

        private void txtSectionTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtSectionTo.Text))
                {
                    txtSectionTo.Clear();
                    lblSectionTo.Clear();
                    return;
                }
                Task<string> loadTask = GetSectionName(txtSectionTo.Text);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                lblSectionTo.Text = loadTask.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtLoginUserFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                string name = "";
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtLoginUserFrom.Text))
                {
                    SetLoginUser(null, null);
                    return;
                }
                Task<string> loadTask = GetLoginUserName(txtLoginUserFrom.Text);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                name = loadTask.Result;
                SetLoginUser(txtLoginUserFrom.Text, name);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetLoginUser(string code, string name)
        {
            txtLoginUserFrom.Text = code;
            lblLoginUserFrom.Text = name;
            if (cbxLoginUser.Checked)
            {
                txtLoginUserTo.Text = code;
                lblLoginUserTo.Text = name;
            }
        }

        private void txtLoginUserTo_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (string.IsNullOrEmpty(txtLoginUserTo.Text))
                {
                    txtLoginUserTo.Clear();
                    lblLoginUserTo.Clear();
                    return;
                }
                Task<string> loadTask = GetLoginUserName(txtLoginUserTo.Text);
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                lblLoginUserTo.Text = loadTask.Result;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    Task<bool> loadTask = GetCurrencyDataCheck(txtCurrencyCode.Text);
                    ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                    if (!loadTask.Result)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                        txtCurrencyCode.Clear();
                        PrecisionLength = 0;
                        this.ActiveControl = txtCurrencyCode;
                    }
                    else
                    {
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
        private void txtPayerName_Validated(object sender, EventArgs e)
        {
            txtPayerName.Text = EbDataHelper.ConvertToPayerName(txtPayerName.Text, LegalPersonalities);
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

        private void txtCode_Leave(object sender, EventArgs e)
        {
            ClearStatusMessage();
        }

        private void rdoNotShow_CheckedChanged(object sender, EventArgs e)
        {
            PayerWidth = 0;
            InitializeGrid();
        }

        private void rdoIndicate_CheckedChanged(object sender, EventArgs e)
        {
            PayerWidth = 100;
            InitializeGrid();
        }
        #endregion
    }
}
