using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;
using Header = Rac.VOne.Web.Models.PeriodicBillingSetting;

namespace Rac.VOne.Client.Screen
{
    /// <summary>定期請求明細設定</summary>
    public partial class PC1602 : VOneScreenBase
    {
        internal Header SettingHeader { get; set; }
        /// <summary>備考1 年月を出力する場合は、編集不可</summary>
        internal bool Note1Enabled { private get; set; }
        /// <summary>備考2 年月を出力する場合は、編集不可</summary>
        internal bool Note2Enabled { private get; set; }

        private List<ColumnNameSetting> ColumnNames { get; set; }
        private List<InputControl> InputControls { get; set; }
        private CollationSetting CollationSetting { get; set; }
        private List<TaxClass> TaxClasses { get; set; }
        private List<AccountTitle> AccountTitles { get; set; }
        private List<GeneralSetting> GeneralSetting { get; set; }
        private List<Category> Categories { get; set; }
        internal Customer CustomerSelected { private get; set; }
        private Currency CurrencySelected { get; set; }
        private string AmountFormat { get; set; }
        private bool UseControlInputNote { get; set; } = false;

        private DataExpression Expression { get; set; }
        /// <summary>請求書単位で消費税計算を行う</summary>
        private bool CalculateTaxByInputId => CollationSetting?.CalculateTaxByInputId == 1;
        /// <summary>消費税計算用 端数処理タイプ</summary>
        private RoundingType TaxCalculationRoundingType { get; set; }
        /// <summary>金額（数量 * 単価) 計算用 端数処理タイプ</summary>
        private RoundingType PriceCalculationRoundingType { get; set; }
        private int Precision { get; set; }
        private const int MaxGridRowCount = 10;
        private const decimal MaxAmount = 99999999999.99999M;
        #region initialize

        public PC1602()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            grid.GridColorType = GridColorType.Input;
            grid.SetupShortcutKeys();
            Text = "定期請求明細設定";

            txtBillingInputId.Enabled = false;
            txtCustomerCode.Enabled = false;
            btnCustomer.Enabled = false;
            txtDestinationCode.Enabled = false;
            btnDestination.Enabled = false;
            datBilledAt.Enabled = false;
            datSalesAt.Enabled = false;
            datClosingAt.Enabled = false;
            datDueAt.Enabled = false;
            txtInvoiceCode.Enabled = false;
        }

        private void InitializeHandlers()
        {
            txtCurrencyCode     .Validated += txtCurrencyCode_Validated;
            txtDepartmentCode   .Validated += txtDepartmentCode_Validated;
            txtStaffCode        .Validated += txtStaffCode_Validated;
            btnDepartment   .Click += btnDepartment_Click;
            btnStaff        .Click += btnStaff_Click;
            btnCurrency     .Click += btnCurrency_Click;

            cmbCollectCategory.SelectedIndexChanged += OnContentChanged;
            txtDepartmentCode   .TextChanged += OnContentChanged;
            txtStaffCode        .TextChanged += OnContentChanged;

            grid.CellEnter                      += grid_CellEnter;
            grid.CellLeave                      += grid_CellLeave;
            grid.CellValueChanged               += grid_CellValueChanged;
            grid.CellContentButtonClick         += grid_CellContentButtonClick;
            grid.CurrentCellDirtyStateChanged   += grid_CurrentCellDirtyStateChanged;
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += PC1602_Load;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("確定");
            BaseContext.SetFunction02Caption(""); // クリア
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction08Caption("行コピー");
            BaseContext.SetFunction09Caption("検索");
            BaseContext.SetFunction10Caption("戻る");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Confirm;
            //OnF02ClickHandler = Clear;
            OnF08ClickHandler = CopyRowData;
            OnF09ClickHandler = Search;
            OnF10ClickHandler = Close;
        }

        private void PC1602_Load(object sender, EventArgs e)
        {
            SetScreenName();
            ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadColumnNameSettingAsync(),
                LoadInputControlAsync(),
                LoadCollationSettingAsync(),
                LoadTaxClassAsync(),
                LoadAccountTitlesAsync(),
                LoadGeneralSettingsAsync(),
                LoadCategoriesAsync(),
                GetUseControlInputNoteAsync(),
            };
            await Task.WhenAll(tasks);
            #region 強制的に 歩引機能をOFF
            // TODO: 歩引機能を有効化する場合は、Detail にカラムを追加して、作成処理も対応する必要がある
            ApplicationControl.UseDiscount = 0;
            #endregion
            TaxCalculationRoundingType = GetRoundingType("消費税計算端数処理");
            PriceCalculationRoundingType = GetRoundingType("金額計算端数処理");

            foreach (var task in tasks.Where(x => x.Exception != null))
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);

            pnlTaxByInputId.Visible = CalculateTaxByInputId;
            InitializeTextBoxFormat();
            InitializeCombobox();


            if (!UseForeignCurrency)
            {
                var currency = (await Util.GetCurrenciesAsync(Login, new[] { Constants.DefaultCurrencyCode })).FirstOrDefault();

                SetCurrency(currency);

                lblCurrency.Visible = false;
                txtCurrencyCode.Visible = false;
                btnCurrency.Visible = false;
            }
            InitializeGridTemplate();
            SetDetailsToGrid();
            lblStatus.BackColor = Color.Aqua;
            Modified = false;
        }

        private void InitializeTextBoxFormat()
        {
            Expression = new DataExpression(ApplicationControl);

            txtDepartmentCode   .Format        = Expression.DepartmentCodeFormatString;
            txtDepartmentCode   .MaxLength     = Expression.DepartmentCodeLength;
            txtDepartmentCode   .PaddingChar   = Expression.DepartmentCodePaddingChar;

            txtStaffCode        .Format        = Expression.StaffCodeFormatString;
            txtStaffCode        .MaxLength     = Expression.StaffCodeLength;
            txtStaffCode        .PaddingChar   = Expression.StaffCodePaddingChar;
        }

        private void InitializeCombobox()
        {
            foreach (var category in Categories.Where(x => x.CategoryType == CategoryType.Collect && x.UseInput == 1))
                cmbCollectCategory.Items.Add(new GrapeCity.Win.Editors.ListItem(category.CodeAndName, category.Id));

            var id = SettingHeader.CollectCategoryId;
            if (id == 0) id = CustomerSelected.CollectCategoryId;
            var item = cmbCollectCategory.Items.Cast<GrapeCity.Win.Editors.ListItem>().FirstOrDefault(x => (int)x.SubItems[1].Value == id);
            cmbCollectCategory.SelectedItem = item;
        }

        #region initialize grid

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);
            var height = builder.DefaultRowHeight;
            var wDct = UseDiscount ? 110 : 0;
            var wNt1 = 440 - wDct;

            var posX = UseDiscount
                ? new[] {
                    0, 110, 220, 310, 420, 530,           860, 890,
                    0, 110, 220, 310, 420, 530, 640, 750,      890,
                } : new[] {
                    0, 110, 220, 310, 420, 420,           860, 890,
                    0, 110, 220, 310, 420, 530, 640, 750,      890,
                };
            var posY = new[] { 0, height };

            var template = new Template();
            #region ヘッダ
            builder.Items.AddRange(new[]
            {
                #region ヘッダー1
                new CellSetting(height, Width =  110, nameof(Billing.SalesAt)            , location: new Point(posX[0], posY[0]), caption: "売上日"  ),
                new CellSetting(height, Width =  110, nameof(Billing.BillingCategoryCode), location: new Point(posX[1], posY[0]), caption: "請求区分"),
                new CellSetting(height, Width =   90, nameof(Billing.TaxClassId)         , location: new Point(posX[2], posY[0]), caption: "税区"    ),
                new CellSetting(height, Width =  110, nameof(Billing.DebitAccountTitleId), location: new Point(posX[3], posY[0]), caption: "債権科目"),
                new CellSetting(height, Width = wDct, nameof(Billing.UseDiscount)        , location: new Point(posX[4], posY[0]), caption: "歩引計算"),
                new CellSetting(height, Width = wNt1, nameof(Billing.Note1)              , location: new Point(posX[5], posY[0]), caption: GetColumnName(nameof(Billing.Note1))),
                new CellSetting(height, Width =   30, nameof(Billing.Memo)               , location: new Point(posX[6], posY[0]) ),
                new CellSetting(height, Width =   40, "MemoButton"                       , location: new Point(posX[7], posY[0]) ),
                #endregion

                #region ヘッダー2
                new CellSetting(height, Width =  110, nameof(Billing.BillingId)          , location: new Point(posX[ 8], posY[1]), caption: "請求ID"  ),
                new CellSetting(height, Width =  110, nameof(Billing.ContractNumber)     , location: new Point(posX[ 9], posY[1]), caption: "契約番号"),
                new CellSetting(height, Width =   90, nameof(Billing.Quantity)           , location: new Point(posX[10], posY[1]), caption: "数量"    ),
                new CellSetting(height, Width =  110, nameof(Billing.UnitSymbol)         , location: new Point(posX[11], posY[1]), caption: "単位"    ),
                new CellSetting(height, Width =  110, nameof(Billing.UnitPrice)          , location: new Point(posX[12], posY[1]), caption: "単価"    ),
                new CellSetting(height, Width =  110, nameof(Billing.Price)              , location: new Point(posX[13], posY[1]), caption: "金額(抜)"),
                new CellSetting(height, Width =  110, nameof(Billing.TaxAmount)          , location: new Point(posX[14], posY[1]), caption: "消費税"  ),
                new CellSetting(height, Width =  140, nameof(Billing.BillingAmount)      , location: new Point(posX[15], posY[1]), caption: "請求額"  ),
                new CellSetting(height, Width =   40, "ClearButton"                      , location: new Point(posX[16], posY[1]) ),
                #endregion
            });
            builder.BuildHeaderOnly(template);
            #endregion

            #region データ
            builder.Items.Clear();
            builder.Items.AddRange(GetRowCellSettings(builder, posX, posY, wDct, wNt1));
            builder.BuildRowOnly(template);
            #endregion

            grid.Template = template;
            grid.CurrentCellBorderLine = builder.GetLine(LineStyle.Thick);
            grid.MultiSelect = false;
            grid.RowCount = MaxGridRowCount;
            grid.HideSelection = false;

            for (var i = 0; i < grid.RowCount; i++)
            {
                var row = grid.Rows[i];
                if (datSalesAt.Value.HasValue) row.Cells[CellName(nameof(Billing.SalesAt))].Value = datSalesAt.Value;
                var cellContractNo = row.Cells[CellName(nameof(Billing.ContractNumber))];
                cellContractNo.Enabled = false;
                cellContractNo.Selectable = false;
            }
        }

        private string GetColumnName(string columnName) =>
            ColumnNames.FirstOrDefault(x => x.ColumnName == columnName)?.DisplayColumnName;

        private InputControl GetInputControl(string columnName) =>
            InputControls?.FirstOrDefault(x => x.ColumnName == columnName);

        private IEnumerable<CellSetting> GetRowCellSettings(GcMultiRowTemplateBuilder builder,
            int[] posX, int[] posY, int wDct, int wNt1)
        {
            var height = builder.DefaultRowHeight;
            var dotted = LineStyle.Dotted;
            var bdTp = builder.GetBorder(left: dotted, right: dotted, bottom: dotted);
            var name = string.Empty;
            var maxLengthUnit = UseControlInputNote ? GrapeCity.Win.Editors.LengthUnit.Byte : GrapeCity.Win.Editors.LengthUnit.Char;
            var maxLength = UseControlInputNote ? NoteInputByteCount : 100;
            InputControl input = null;
            #region データ1
            input = GetInputControl(name = nameof(Billing.SalesAt));
            yield return new CellSetting(height,  110, name, border: bdTp, location: new Point(posX[ 0], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 0, tabStop: input?.IsTabStop ?? true, cell: builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter, true), enabled: false);
            input = GetInputControl(name = nameof(Billing.BillingCategoryId));
            yield return new CellSetting(height,  110, name, border: bdTp, location: new Point(posX[ 1], posY[0]), dataField: name, readOnly: true, visible: false, cell: builder.GetTextBoxCell());
            name = nameof(Billing.BillingCategoryCode);
            yield return new CellSetting(height,  110, name, border: bdTp, location: new Point(posX[ 1], posY[0]), dataField: name, readOnly: false, visible: true, tabIndex: input?.TabIndex ?? 1, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(ime: ImeMode.Disable, format: "9", maxLength: 2));
            name = nameof(Billing.BillingCategoryName);
            yield return new CellSetting(height,  110, name, border: bdTp, location: new Point(posX[ 1], posY[0]), dataField: name, readOnly: true, visible: false, tabIndex: input?.TabIndex ?? 1, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell());
            name = nameof(Billing.UseLongTermAdvancedReceived);
            yield return new CellSetting(height,  110, name, border: bdTp, location: new Point(posX[ 1], posY[0]), dataField: name, readOnly: true, visible: false, cell: builder.GetTextBoxCell());
            input = GetInputControl(name = nameof(Billing.TaxClassId));
            yield return new CellSetting(height,   90, name, border: bdTp, location: new Point(posX[ 2], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 2, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(ime: ImeMode.Disable, format: "9", maxLength: 1));
            name = "TaxClassIdName";
            yield return new CellSetting(height,   90, name, border: bdTp, location: new Point(posX[ 2], posY[0]), readOnly: true, visible: false, tabIndex: input?.TabIndex ?? 2, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell());
            input = GetInputControl(name = nameof(Billing.DebitAccountTitleId));
            yield return new CellSetting(height,  110, name, border: bdTp, location: new Point(posX[ 1], posY[0]), dataField: name, readOnly: true, visible: false, cell: builder.GetTextBoxCell());
            name = nameof(Billing.AccountTitleCode);
            yield return new CellSetting(height,  110, name, border: bdTp, location: new Point(posX[ 3], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 3, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(ime: ImeMode.Disable, format: Expression.AccountTitleCodeFormatString, maxLength: Expression.AccountTitleCodeLength));
            name = "AccountTitleName";
            yield return new CellSetting(height,  110, name, border: bdTp, location: new Point(posX[ 3], posY[0]), readOnly: true, visible: false, tabIndex: input?.TabIndex ?? 3, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell());
            input = GetInputControl(name = nameof(Billing.UseDiscount));
            yield return new CellSetting(height, wDct, name, border: bdTp, location: new Point(posX[ 4], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 4, tabStop: input?.IsTabStop ?? true, cell: builder.GetCheckBoxCell());
            input = GetInputControl(name = nameof(Billing.Note1));
            yield return new CellSetting(height, wNt1, name, border: bdTp, location: new Point(posX[ 5], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 5, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(maxLength: maxLength, ime: ImeMode.Hiragana, maxLengthUnit: maxLengthUnit), enabled: Note1Enabled);
            name = "MemoSymbol";
            yield return new CellSetting(height,   30, name, border: bdTp, location: new Point(posX[ 6], posY[0]), tabStop: false, cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter));
            name = "MemoButton";
            yield return new CellSetting(height,   40, name, border: bdTp, location: new Point(posX[ 7], posY[0]), tabStop: false, cell: builder.GetButtonCell(), value: "備考");
            #endregion
            var bdBm = builder.GetBorder(left: dotted, top: dotted, right: dotted);
            #region データ2
            name = nameof(Billing.BillingId);
            yield return new CellSetting(height,  110, name, border: bdBm, location: new Point(posX[ 8], posY[1]), enabled: false, dataField: name, readOnly: false, tabStop: false);
            input = GetInputControl(name = nameof(Billing.ContractNumber));
            yield return new CellSetting(height,  110, name, border: bdBm, location: new Point(posX[ 9], posY[1]), enabled: false, dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 6, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(ime: ImeMode.Disable, format: "A9", maxLength: 20));
            input = GetInputControl(name = nameof(Billing.Quantity));
            yield return new CellSetting(height,   90, name, border: bdBm, location: new Point(posX[10], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 7, tabStop: input?.IsTabStop ?? true, cell: GetQuantityNumberCell(builder));
            input = GetInputControl(name = nameof(Billing.UnitSymbol));
            yield return new CellSetting(height,  110, name, border: bdBm, location: new Point(posX[11], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 8, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(maxLength: 3, ime: ImeMode.Hiragana));
            input = GetInputControl(name = nameof(Billing.UnitPrice));
            yield return new CellSetting(height,  110, name, border: bdBm, location: new Point(posX[12], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 9, tabStop: input?.IsTabStop ?? true, cell: GetUnitPriceNumberCell(builder, Precision, Precision, 0));
            input = GetInputControl(name = nameof(Billing.Price));
            yield return new CellSetting(height,  110, name, border: bdBm, location: new Point(posX[13], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 10, tabStop: input?.IsTabStop ?? true, cell: GetPriceNumberCell(builder, Precision, Precision, 0));
            input = GetInputControl(name = nameof(Billing.TaxAmount));
            yield return new CellSetting(height,  110, name, border: bdBm, location: new Point(posX[14], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 11, tabStop: input?.IsTabStop ?? true, cell: GetPriceNumberCell(builder, Precision, Precision, 0));
            input = GetInputControl(name = nameof(Billing.BillingAmount));
            yield return new CellSetting(height,  140, name, border: bdBm, location: new Point(posX[15], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 12, tabStop: input?.IsTabStop ?? true, cell: GetPriceNumberCell(builder, Precision, Precision, 0), enabled: !CalculateTaxByInputId);
            name = "ClearButton";
            yield return new CellSetting(height,   40, name, border: bdBm, location: new Point(posX[16], posY[1]), cell: builder.GetButtonCell(), tabStop: false, value: "クリア");
            #endregion
            #region データ3
            yield return new CellSetting(height, 0, nameof(Billing.Memo), dataField: nameof(Billing.Memo));
            yield return new CellSetting(height, 0, nameof(Billing.Note2), dataField: nameof(Billing.Note2));
            yield return new CellSetting(height, 0, nameof(Billing.Note3), dataField: nameof(Billing.Note3));
            yield return new CellSetting(height, 0, nameof(Billing.Note4), dataField: nameof(Billing.Note4));
            yield return new CellSetting(height, 0, nameof(Billing.Note5), dataField: nameof(Billing.Note5));
            yield return new CellSetting(height, 0, nameof(Billing.Note6), dataField: nameof(Billing.Note6));
            yield return new CellSetting(height, 0, nameof(Billing.Note7), dataField: nameof(Billing.Note7));
            yield return new CellSetting(height, 0, nameof(Billing.Note8), dataField: nameof(Billing.Note8));
            yield return new CellSetting(height, 0, nameof(Billing.BillingDivisionContract));
            yield return new CellSetting(height, 0, nameof(Billing.BillingDiscountId), dataField: nameof(Billing.BillingDiscountId));
            #endregion
        }

        private GcNumberCell GetQuantityNumberCell(GcMultiRowTemplateBuilder builder)
        {
            var fieldFormat = "#####.##";
            var displayFormat = "####0.00";
            var numberCell = builder.GetNumberCell(fieldFormat: fieldFormat, displayFormat: displayFormat);
            numberCell.Fields.IntegerPart.MaxDigits = 5;
            numberCell.HighlightText = true;
            numberCell.AllowDeleteToNull = true;
            return numberCell;
        }

        private GcNumberCell GetUnitPriceNumberCell(GcMultiRowTemplateBuilder builder, int fieldScale, int displayScale, int roundPattern, string displayFormatString = "0")
        {
            var cell = builder.GetNumberCellCurrency(fieldScale, displayScale, roundPattern, displayFormatString);
            cell.Fields.IntegerPart.MaxDigits = 5;
            cell.HighlightText = true;
            cell.AllowDeleteToNull = true;
            return cell;
        }

        private GcNumberCell GetPriceNumberCell(GcMultiRowTemplateBuilder builder, int fieldScale, int displayScale, int roundPattern, string displayFormatString = "0")
        {
            var cell = builder.GetNumberCellCurrencyInput(fieldScale, displayScale, roundPattern, displayFormatString);
            cell.Fields.IntegerPart.MaxDigits = 11;
            cell.HighlightText = true;
            cell.AllowDeleteToNull = true;
            return cell;
        }

        #endregion

        private void SetDetailsToGrid()
        {
            foreach (var pair in SettingHeader.Details.Take(MaxGridRowCount).Select((x, index) => new { x, index }))
            {
                var row = grid.Rows[pair.index];
                var detail = pair.x;
                if (detail.DebitAccountTitleId.HasValue)
                {
                    var code = AccountTitles.FirstOrDefault(x => x.Id == detail.DebitAccountTitleId)?.Code;
                    row.Cells[CellName(nameof(Billing.AccountTitleCode))].Value = code;
                    SetAccountTitleByCode(pair.index);
                }
                row.Cells[CellName(nameof(Billing.Quantity))].Value = detail.Quantity;
                row.Cells[CellName(nameof(Billing.UnitSymbol))].Value = detail.UnitSymbol;
                row.Cells[CellName(nameof(Billing.UnitPrice))].Value = detail.UnitPrice;
                row.Cells[CellName(nameof(Billing.Price))].Value = detail.Price;
                row.Cells[CellName(nameof(Billing.TaxAmount))].Value = detail.TaxAmount;
                row.Cells[CellName(nameof(Billing.BillingAmount))].Value = detail.BillingAmount;
                row.Cells[CellName(nameof(Billing.Memo))].Value = detail.Memo;
                row.Cells[CellName("MemoSymbol")].Value = detail.IsAnyNoteInputted ? "○" : "";
                row.Cells[CellName(nameof(Billing.Note1))].Value = detail.Note1;
                row.Cells[CellName(nameof(Billing.Note2))].Value = detail.Note2;
                row.Cells[CellName(nameof(Billing.Note3))].Value = detail.Note3;
                row.Cells[CellName(nameof(Billing.Note4))].Value = detail.Note4;
                row.Cells[CellName(nameof(Billing.Note5))].Value = detail.Note5;
                row.Cells[CellName(nameof(Billing.Note6))].Value = detail.Note6;
                row.Cells[CellName(nameof(Billing.Note7))].Value = detail.Note7;
                row.Cells[CellName(nameof(Billing.Note8))].Value = detail.Note8;
                //row.Cells[CellName(nameof(Billing.BillingDiscountId))].Value = detail.BillingDiscountId;
                SetBillingCategory(pair.index, GetBillingCategory(x => x.Id == detail.BillingCategoryId));
                //if (UseDiscount && detail.BillingDiscountId.HasValue)
                //    row.Cells[CellName(nameof(Billing.UseDiscount))].Value = 1;
                SetTaxClass(pair.index, TaxClasses.FirstOrDefault(x => x.Id == detail.TaxClassId));
            }
            CalculateTotalAmount();
        }

        #endregion

        #region function keys

        [OperationLog("確定")]
        private void Confirm()
        {
            ClearStatusMessage();

            if (!ValidateInputValues()) return;
            if (!ValidateDetailValues()) return;


            ParentForm.DialogResult = DialogResult.OK;
        }

        //[OperationLog("クリア")]
        //private void Clear()
        //{

        //}

        [OperationLog("行コピー")]
        private void CopyRowData()
        {
            var index = grid.Rows.LastOrDefault(x => !IsRowEmpty(x))?.Index;
            if (!index.HasValue)
            {
                ShowWarningDialog(MsgWngInputGridRequired);
                return;
            }
            var rowIndex = index.Value;
            if (rowIndex >= 9)
            {
                ShowWarningDialog(MsgWngAllDetailLinesFilled);
                return;
            }

            var newRowIndex = rowIndex + 1;
            for (var i = 0; i < grid.Rows[rowIndex].Cells.Count; i++)
            {
                var cell = grid.Rows[rowIndex].Cells[i];
                var dataField = cell.DataField;
                if (dataField == nameof(Billing.BillingId)) continue;

                grid.Rows[newRowIndex].Cells[i].Value = dataField != nameof(Billing.ContractNumber) ? cell.Value : null;
                switch (dataField)
                {
                    case nameof(Billing.BillingCategoryName):
                        if (cell.Value != null)
                            grid.Rows[newRowIndex].Cells[CellName(nameof(Billing.BillingCategoryName))].Visible = true;
                        break;
                    case nameof(Billing.TaxClassId):
                        if (cell.Value != null)
                        {
                            grid.Rows[newRowIndex].Cells[CellName("TaxClassIdName")].Visible = true;
                            var id = (int)cell.Value;
                            SetTaxClass(newRowIndex, TaxClasses.FirstOrDefault(x => x.Id == id));
                        }
                        break;
                    case nameof(Billing.AccountTitleCode):
                        if (cell.Value != null)
                            grid.Rows[newRowIndex].Cells[CellName("AccountTitleName")].Visible = true;
                        break;
                    //case nameof(Billing.ContractNumber):
                    //    bool isAble = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))].Enabled;
                    //    grid.Rows[newRowIndex].Cells[CellName(nameof(Billing.ContractNumber))].Enabled = isAble;
                    //    grid.Rows[newRowIndex].Cells[CellName(nameof(Billing.ContractNumber))].Selectable = isAble;
                    //    break;
                    default:
                        break;
                }

            }
            CalculateTotalAmount();

        }

        [OperationLog("検索")]
        private void Search()
        {
            var rowIndex = grid.CurrentRow.Index;
            var field = grid.CurrentCell.DataField;

            switch (field)
            {
                case nameof(Billing.BillingCategoryCode):
                    ShowBillingCategorySearchDialog(rowIndex);
                    break;

                case nameof(Billing.TaxClassId):
                    ShowTaxClassSearchDialog(rowIndex);
                    break;

                case nameof(Billing.AccountTitleCode):
                    ShowAccTitleSearchDialog(rowIndex);
                    break;

                //case nameof(Billing.ContractNumber):
                //    var cellIndex = grid.CurrentCell.CellIndex;
                //    var cell = (GcTextBoxCell)grid.Rows[rowIndex].Cells[cellIndex];
                //    if (cell.Enabled)
                //    {
                //        if (!ValidateCustomerCode()) return;
                //        ShowContractNumberSearchDialog(rowIndex);
                //    }
                //    break;

                default:
                    break;
            }

            ActiveControl = grid;
        }

        [OperationLog("戻る")]
        private void Close()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region confirm

        private bool ValidateInputValues()
        {
            if (UseForeignCurrency && !txtCurrencyCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text))) return false;
            if (!txtDepartmentCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblDepartment.Text))) return false;
            if (!txtStaffCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblStaff.Text))) return false;
            if (CalculateTaxByInputId && lblTotalTaxAmount.Text != lblTotalTaxByInputId.Text)
            {
                ShowWarningDialog(MsgWngTaxAmountDifference);
                return false;
            }

            return true;
        }

        private bool ValidateDetailValues()
        {
            this.ActiveControl = grid;

            var rows = grid.Rows.Where(x => !IsRowEmpty(x)).ToArray();
            if (!rows.Any())
            {
                ShowWarningDialog(MsgWngInputGridRequired);
                return false;
            }

            foreach (var row in rows)
            {
                var cellBillingCategoryCode     = row.Cells[CellName(nameof(Billing.BillingCategoryCode))];
                var cellTaxClassId              = row.Cells[CellName(nameof(Billing.TaxClassId))];
                var cellBillingAmount           = row.Cells[CellName(nameof(Billing.BillingAmount))];
                var cellQuantity                = row.Cells[CellName(nameof(Billing.Quantity))];
                var cellUnitPrice               = row.Cells[CellName(nameof(Billing.UnitPrice))];
                var cellPrice                   = row.Cells[CellName(nameof(Billing.Price))];
                var billingCategoryCode = IsCellEmpty(cellBillingCategoryCode) ? string.Empty : cellBillingCategoryCode.Value.ToString();
                var taxClassId          = IsCellEmpty(cellTaxClassId) ? string.Empty : cellTaxClassId.Value.ToString();
                var BillingAmountValue  = IsCellEmpty(cellBillingAmount) ? string.Empty : cellBillingAmount.Value.ToString();
                var billingAmount       = string.IsNullOrEmpty(BillingAmountValue) ? 0M : Convert.ToInt64(cellBillingAmount.Value);
                var quantity            = IsCellEmpty(cellQuantity) ? 0M : Convert.ToInt64(cellQuantity.Value);
                var unitPrice           = IsCellEmpty(cellUnitPrice) ? 0M : Convert.ToInt64(cellUnitPrice.Value);
                var price               = IsCellEmpty(cellPrice) ? 0M : Convert.ToInt64(cellPrice.Value);

                if (string.IsNullOrWhiteSpace(billingCategoryCode))
                {
                    ShowWarningDialog(MsgWngInputRequired, "請求区分");
                    grid.ClearSelection();
                    grid.CurrentCell = cellBillingCategoryCode;
                    return false;
                }

                if (string.IsNullOrWhiteSpace(taxClassId))
                {
                    ShowWarningDialog(MsgWngInputRequired, "税区");
                    grid.ClearSelection();
                    grid.CurrentCell = cellTaxClassId;
                    return false;
                }

                if (string.IsNullOrWhiteSpace(BillingAmountValue))
                {
                    ShowWarningDialog(MsgWngInputRequired, "請求額");
                    grid.ClearSelection();
                    grid.CurrentCell = cellBillingAmount;
                    return false;
                }

                if (billingAmount == 0M)
                {
                    ShowWarningDialog(MsgWngInputExceptZeroAmt, "請求額");
                    grid.ClearSelection();
                    grid.CurrentCell = cellBillingAmount;
                    return false;
                }

                if (quantity != 0 && unitPrice != 0 && price != 0
                    && (price != quantity * unitPrice)
                    && (!ShowConfirmDialog(MsgQstContinueUnmatchCalculation))) return false;
            }

            return true;
        }


        internal IEnumerable<PeriodicBillingSettingDetail> GetBillingsFromGrid()
            => grid.Rows.Where(x => !IsRowEmpty(x)).Select(x => GetBillingFromRowData(x));
        private PeriodicBillingSettingDetail GetBillingFromRowData(Row row)
        {
            var cellBillingCategoryId           = row.Cells[CellName(nameof(Billing.BillingCategoryId))];
            var cellTaxClassId                  = row.Cells[CellName(nameof(Billing.TaxClassId))];
            var cellDebitAccountTitleId         = row.Cells[CellName(nameof(Billing.DebitAccountTitleId))];
            var cellQuantity                    = row.Cells[CellName(nameof(Billing.Quantity))];
            var cellUnitSymbol                  = row.Cells[CellName(nameof(Billing.UnitSymbol))];
            var cellUnitPrice                   = row.Cells[CellName(nameof(Billing.UnitPrice))];
            var cellPrice                       = row.Cells[CellName(nameof(Billing.Price))];
            var cellTaxAmount                   = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellBillingAmount               = row.Cells[CellName(nameof(Billing.BillingAmount))];
            var cellNote1                       = row.Cells[CellName(nameof(Billing.Note1))];
            var cellNote2                       = row.Cells[CellName(nameof(Billing.Note2))];
            var cellNote3                       = row.Cells[CellName(nameof(Billing.Note3))];
            var cellNote4                       = row.Cells[CellName(nameof(Billing.Note4))];
            var cellNote5                       = row.Cells[CellName(nameof(Billing.Note5))];
            var cellNote6                       = row.Cells[CellName(nameof(Billing.Note6))];
            var cellNote7                       = row.Cells[CellName(nameof(Billing.Note7))];
            var cellNote8                       = row.Cells[CellName(nameof(Billing.Note8))];
            var cellMemo                        = row.Cells[CellName(nameof(Billing.Memo))];

            var billingCategoryId = IsCellEmpty(cellBillingCategoryId) ? 0 : Convert.ToInt32(cellBillingCategoryId.Value);
            var taxClassId = IsCellEmpty(cellTaxClassId) ? 0 : Convert.ToInt32(cellTaxClassId.Value);
            var debitAccountTitleId = (int?)cellDebitAccountTitleId.Value;
            var quantity            = IsCellEmpty(cellQuantity) ? (decimal?)null : Convert.ToDecimal(cellQuantity.Value);
            var unitSymbol          = IsCellEmpty(cellUnitSymbol) ? (string)null : cellUnitSymbol.Value.ToString();
            var unitPrice           = IsCellEmpty(cellUnitPrice) ? (decimal?)null : Convert.ToDecimal(cellUnitPrice.Value);
            var price               = IsCellEmpty(cellPrice) ? 0M : Convert.ToDecimal(cellPrice.Value);
            var taxAmount           = IsCellEmpty(cellTaxAmount) ? 0M : Convert.ToDecimal(cellTaxAmount.Value);
            var billingAmount       = IsCellEmpty(cellBillingAmount) ? 0M : Convert.ToDecimal(cellBillingAmount.Value);
            var note1               = Convert.ToString(cellNote1.Value).Trim();
            var note2               = Convert.ToString(cellNote2.Value).Trim();
            var note3               = Convert.ToString(cellNote3.Value).Trim();
            var note4               = Convert.ToString(cellNote4.Value).Trim();
            var note5               = Convert.ToString(cellNote5.Value).Trim();
            var note6               = Convert.ToString(cellNote6.Value).Trim();
            var note7               = Convert.ToString(cellNote7.Value).Trim();
            var note8               = Convert.ToString(cellNote8.Value).Trim();
            var memo                = IsCellEmpty(cellMemo) ? string.Empty : cellMemo.Value.ToString();

            return new PeriodicBillingSettingDetail {
                BillingCategoryId    = billingCategoryId,
                TaxClassId           = taxClassId,
                DebitAccountTitleId  = debitAccountTitleId,
                Quantity             = quantity,
                UnitSymbol           = unitSymbol,
                UnitPrice            = unitPrice,
                Price                = price,
                TaxAmount            = taxAmount,
                BillingAmount        = billingAmount,
                Note1                = note1,
                Note2                = note2,
                Note3                = note3,
                Note4                = note4,
                Note5                = note5,
                Note6                = note6,
                Note7                = note7,
                Note8                = note8,
                Memo                 = memo,
            };
        }


        #endregion

        #region event handler

        private void OnContentChanged(object sender, EventArgs e) => Modified = true;

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtCurrencyCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                CurrencySelected = null;
                Precision = 0;
                return;
            }
            var task = GetCurrencyAsync(code);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var currency = task.Result;
            if (currency == null)
            {
                txtCurrencyCode.Clear();
                txtCurrencyCode.Focus();
                CurrencySelected = null;
                ShowWarningDialog(MsgWngMasterNotExist, "通貨", code);
                return;
            }
            SetCurrency(currency);
        }

        private void txtDepartmentCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtDepartmentCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblDepartmentName.Clear();
                return;
            }
            var task = GetDepartmentAsync(code);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var department = task.Result;
            if (department == null)
            {
                txtDepartmentCode.Clear();
                lblDepartmentName.Clear();
                txtDepartmentCode.Focus();
                ShowWarningDialog(MsgWngMasterNotExist, "請求部門", code);
                return;
            }
            SetDepartment(department);
        }

        private void txtStaffCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtStaffCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                lblStaffName.Clear();
                return;
            }
            var task = GetStaffAsync(code);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var staff = task.Result;
            if (staff == null)
            {
                txtStaffCode.Clear();
                lblStaffName.Clear();
                txtStaffCode.Focus();
                ShowWarningDialog(MsgWngMasterNotExist, "担当者", code);
                return;
            }
        }

        private void SetCurrency(Currency currency)
        {
            if (currency == null) return;
            var requireRest = CurrencySelected != null && currency.Id != CurrencySelected.Id;
            txtCurrencyCode.Text = currency.Code;
            Precision = currency.Precision;
            CurrencySelected = currency;
            SetAmountFormat(Precision);
            if (!requireRest) return;
            InitializeGridTemplate();
        }

        private void SetDepartment(Department department)
        {
            if (department == null) return;
            txtDepartmentCode.Text = department.Code;
            lblDepartmentName.Text = department.Name;
        }

        private void SetStaff(Staff staff)
        {
            if (staff == null) return;
            txtStaffCode.Text = staff.Code;
            lblStaffName.Text = staff.Name;
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            SetCurrency(this.ShowCurrencySearchDialog());
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            SetDepartment(this.ShowDepartmentSearchDialog());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            SetStaff(this.ShowStaffSearchDialog());
        }

        private void SetAmountFormat(int scale)
        {
            var format = "#,##0";
            if (scale > 0) format += "." + new string('0', scale);
            AmountFormat = format;
        }

        #endregion

        #region grid event handler

        private void grid_CellContentButtonClick(object sender, CellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            var cellIndex = e.CellIndex;
            var cell = grid.Rows[rowIndex].Cells[cellIndex];
            var columnName = e.CellName;

            if (columnName == CellName("ClearButton"))
                ClearCellContent(rowIndex);
            else if (columnName == CellName("MemoButton"))
                ShowBillingMemoDialog(rowIndex);
        }

        private void grid_CellEnter(object sender, CellEventArgs e)
        {
            if (ActiveControl != grid) return;

            var rowIndex = e.RowIndex;
            var row = grid.Rows[rowIndex];
            var cell = row.Cells[e.CellIndex];
            switch (cell.Name)
            {
                case "celBillingCategoryCode":
                case "celAccountTitleCode":
                    BaseContext.SetFunction09Enabled(true);
                    break;

                case "celBillingCategoryName":
                    grid.CurrentCell = row.Cells[CellName(nameof(Billing.BillingCategoryCode))];
                    row.Cells[CellName(nameof(Billing.BillingCategoryName))].Visible = false;
                    break;

                case "celTaxClassId":
                case "celTaxClassIdName":
                    BaseContext.SetFunction09Enabled(true);
                    grid.CurrentCell = row.Cells[CellName(nameof(Billing.TaxClassId))];
                    row.Cells[CellName("TaxClassIdName")].Visible = false;
                    break;

                case "celAccountTitleName":
                    grid.CurrentCell = row.Cells[CellName(nameof(Billing.AccountTitleCode))];
                    row.Cells[CellName("AccountTitleName")].Visible = false;
                    break;

                case "celContractNumber":
                    if (cell.Enabled)
                        BaseContext.SetFunction09Enabled(true);
                    break;
                default:
                    BaseContext.SetFunction09Enabled(false);
                    break;
            }
            grid.BeginEdit(true);
        }

        private void grid_CellLeave(object sender, CellEventArgs e)
        {
            grid.EndEdit();

            var rowIndex = e.RowIndex;
            var cell = grid.Rows[rowIndex].Cells[e.CellIndex];
            var dataField = cell.DataField;
            switch (dataField)
            {
                case nameof(Billing.BillingCategoryCode):
                    SetBillingCategoryByCode(rowIndex);
                    CalculateTotalAmount();
                    break;

                case nameof(Billing.TaxClassId):
                    SetTaxClassByCode(rowIndex);
                    CalculateTotalAmount();
                    break;

                case nameof(Billing.AccountTitleCode):
                    SetAccountTitleByCode(rowIndex);
                    break;
                case nameof(Billing.ContractNumber):
                    //if (cell.Value != null && !ValidateContractNumber(rowIndex))
                    //{
                    //    cell.Value = null;
                    //    grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))];
                    //}
                    break;

                default:
                    break;
            }

        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            Modified = true;
        }

        private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            var rowIndex = grid.CurrentRow.Index;
            var cell = grid.Rows[rowIndex].Cells[grid.CurrentCell.CellIndex];
            if (!(cell is GcNumberCell)) return;

            grid.CommitEdit();

            var dataField = cell.DataField;
            switch (dataField)
            {
                case nameof(Billing.Quantity):
                case nameof(Billing.UnitPrice):
                    SetBillingAmounFromQuantityWithUnitPrice(rowIndex);
                    break;
                case nameof(Billing.Price):
                    SetBillingAmountFromPrice(rowIndex);
                    break;
                case nameof(Billing.TaxAmount):
                    SetPriceOrBillingAmountFromTax(rowIndex);
                    break;
                case nameof(Billing.BillingAmount):
                    SetPriceAndTaxFromBillingAmount(rowIndex);
                    break;
                default:
                    return;
            }
            CalculateTotalAmount();

        }

        #endregion

        #region grid helper

        private string CellName(string name) => $"cel{name}";

        private bool IsCellEmpty(Cell cell)
            => (cell.Value == null
             || string.IsNullOrEmpty(cell.Value.ToString())
             || cell.Name == CellName(nameof(Billing.UseDiscount)) && 0.Equals(cell.Value));

        private bool IsButtonCell(Cell cell)
            => cell.Name == CellName("MemoButton") || cell.Name == CellName("ClearButton");

        private bool IsRowEmpty(int rowIndex)
            => IsRowEmpty(grid.Rows[rowIndex]);

        private bool IsRowEmpty(Row row)
            => row.Cells.Where(x => !(x.Name == CellName(nameof(Billing.SalesAt)) || IsButtonCell(x)))
                .All(x => IsCellEmpty(x));

        private void ClearCellContent(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            foreach (var cell in row.Cells
                .Where(x => !(IsButtonCell(x) || x.Name == CellName(nameof(Billing.SalesAt)))))
            {
                cell.Value = null;
                if (cell.Name == CellName(nameof(Billing.BillingAmount)))
                    cell.Enabled = !CalculateTaxByInputId;
            }

            CalculateTotalAmount();

            grid.ClearSelection();
            grid.CurrentCell = row.Cells[CellName(nameof(Billing.SalesAt))];

            var cellContractNumber = row.Cells[CellName(nameof(Billing.ContractNumber))];
            cellContractNumber.Enabled = false;
            cellContractNumber.Selectable = false;
        }

        private void SetBillingCategoryByCode(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var cellBillingCategoryCode = row.Cells[CellName(nameof(Billing.BillingCategoryCode))];

            if (cellBillingCategoryCode.Value != null)
            {
                var code = cellBillingCategoryCode.Value.ToString();

                if (code.Length < 2)
                {
                    code = code.PadLeft(2, '0');
                    cellBillingCategoryCode.Value = code;
                }
                SetBillingCategory(rowIndex, GetBillingCategoryByCode(code));
            }
            else
            {
                cellBillingCategoryCode.Value = null;
                row.Cells[CellName(nameof(Billing.BillingCategoryId))].Value = null;
                row.Cells[CellName(nameof(Billing.BillingCategoryName))].Value = null;
                row.Cells[CellName(nameof(Billing.UseLongTermAdvancedReceived))].Value = null;
                row.Cells[CellName(nameof(Billing.UseDiscount))].Value = null;
                row.Cells[CellName(nameof(Billing.TaxClassId))].Value = null;
                row.Cells[CellName("TaxClassIdName")].Value = null;
                row.Cells[CellName(nameof(Billing.BillingDivisionContract))].Value = null;
                grid.EndEdit();
            }
        }

        private void SetBillingCategory(int rowIndex, Category category)
        {
            var row = grid.Rows[rowIndex];

            var cellBillingCategoryCode         = row.Cells[CellName(nameof(Billing.BillingCategoryCode))];
            var cellBillingCategoryId           = row.Cells[CellName(nameof(Billing.BillingCategoryId))];
            var cellBillingCategoryName         = row.Cells[CellName(nameof(Billing.BillingCategoryName))];
            var cellUseLongTermAdvancedReceived = row.Cells[CellName(nameof(Billing.UseLongTermAdvancedReceived))];
            var cellUseDiscount                 = row.Cells[CellName(nameof(Billing.UseDiscount))];
            var cellTaxClassId                  = row.Cells[CellName(nameof(Billing.TaxClassId))];
            var cellTaxClassIdName              = row.Cells[CellName("TaxClassIdName")];
            var cellBillingDiscountId           = row.Cells[CellName(nameof(Billing.BillingDiscountId))];
            var cellContractNumber              = row.Cells[CellName(nameof(Billing.ContractNumber))];
            var cellBillingDivisionContract     = row.Cells[CellName(nameof(Billing.BillingDivisionContract))];

            if (category == null)
            {
                cellBillingCategoryCode.Value = string.Empty;
                cellBillingCategoryId           .Value = null;
                cellBillingCategoryName         .Value = null;
                cellUseLongTermAdvancedReceived .Value = null;
                cellUseDiscount                 .Value = null;
                cellTaxClassId                  .Value = null;
                cellTaxClassIdName              .Value = null;
                cellBillingDiscountId           .Value = null;
                cellContractNumber              .Enabled = false;
                cellContractNumber              .Value = null;
                cellBillingDivisionContract     .Value = null;
                return;
            }

            cellBillingCategoryCode.Visible = true;
            cellBillingCategoryName.Visible = true;
            int? originalCategoryId = (int?)cellBillingCategoryId.Value;
            if (originalCategoryId.HasValue && category.Id == originalCategoryId.Value) return;
            cellBillingCategoryId   .Value = category.Id;
            cellBillingCategoryCode .Value = category.Code;
            cellBillingCategoryName .Value = category.CodeAndName;
            cellUseLongTermAdvancedReceived.Value = category.UseLongTermAdvanceReceived;

            if (UseDiscount) cellUseDiscount.Value = category.UseDiscount;
            var originalTaxClassId = (int?)cellTaxClassId.Value;
            if (category.TaxClassId != originalTaxClassId)
                SetTaxClass(rowIndex, TaxClasses.FirstOrDefault(x => x.Id == category.TaxClassId));

            if (category.UseLongTermAdvanceReceived == 1 && RegisterContractInAdvance)
            {
                cellBillingDivisionContract.Value = 1;

                cellContractNumber.Enabled = true;
                cellContractNumber.Selectable = true;

                return;
            }
            cellContractNumber.Value = string.Empty;
            cellContractNumber.Enabled = false;

            if (category.UseLongTermAdvanceReceived == 0 && !RegisterContractInAdvance)
            {
                cellBillingDivisionContract.Value = 2;
            }
            else if (category.UseLongTermAdvanceReceived == 1 && !RegisterContractInAdvance)
            {
                cellBillingDivisionContract.Value = 3;
            }
        }

        public void SetTaxClassByCode(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var cellTaxClassId = row.Cells[CellName(nameof(Billing.TaxClassId))];

            var id = 0;
            if (cellTaxClassId.Value == null
                || !int.TryParse(cellTaxClassId.Value?.ToString(), out id))
            {
                cellTaxClassId.Value = null;
                row.Cells[CellName("TaxClassIdName")].Value = null;
                grid.EndEdit();
                return;
            }

            if (!int.TryParse(cellTaxClassId.Value.ToString(), out id))
            {
                cellTaxClassId.Value = null;
                return;
            }
            var target = TaxClasses.FirstOrDefault(x => x.Id == id);
            if (target == null) return;
            SetTaxClass(rowIndex, target);
        }

        public void SetTaxClass(int rowIndex, TaxClass taxClass)
        {
            var row = grid.Rows[rowIndex];
            var cellTaxClassId = row.Cells[CellName(nameof(Billing.TaxClassId))];
            var cellTaxClassIdName = row.Cells[CellName("TaxClassIdName")];

            if (taxClass == null)
            {
                cellTaxClassId.Value = null;
                cellTaxClassIdName.Value = string.Empty;
                return;
            }

            cellTaxClassId.Value = taxClass.Id;
            cellTaxClassIdName.Visible = true;
            cellTaxClassIdName.Value = taxClass.Id + ":" + taxClass.Name;
        }

        public void SetAccountTitleByCode(int rowIndex)
        {
            var cellAccountTitleCode = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.AccountTitleCode))];

            if (!IsCellEmpty(cellAccountTitleCode))
            {
                if (Expression.AccountTitleCodeFormatString == "9")
                    cellAccountTitleCode.Value = cellAccountTitleCode.Value.ToString().PadLeft(Expression.AccountTitleCodeLength, '0');

                SettingAccountTitle(rowIndex, cellAccountTitleCode.Value.ToString());
            }
            else
            {
                cellAccountTitleCode.Value = null;
                grid.Rows[rowIndex].Cells[CellName(nameof(Billing.DebitAccountTitleId))].Value = null;
                grid.Rows[rowIndex].Cells[CellName("AccountTitleName")].Value = null;
                grid.EndEdit();
            }
        }

        public void SettingAccountTitle(int rowIndex, string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            var account = AccountTitles.FirstOrDefault(x => x.Code == code);
            var cellAccountTitleCode = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.AccountTitleCode))];
            var cellAccountTitleName = grid.Rows[rowIndex].Cells[CellName("AccountTitleName")];
            var cellDebitAccountTitleId = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.DebitAccountTitleId))];
            if (account != null)
            {
                cellAccountTitleCode.Value = account.Code;
                cellDebitAccountTitleId.Value = account.Id;
                cellAccountTitleName.Visible = true;
                cellAccountTitleName.Value = account.Name;
            }
            else
            {
                cellAccountTitleCode.Value = string.Empty;
                grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.Note1))];
            }
        }

        private void ShowBillingMemoDialog(int rowIndex)
        {

            var row = grid.Rows[rowIndex];
            var memoCell = row.Cells[CellName(nameof(Billing.Memo))];
            var note2Cell = row.Cells[CellName(nameof(Billing.Note2))];
            var note3Cell = row.Cells[CellName(nameof(Billing.Note3))];
            var note4Cell = row.Cells[CellName(nameof(Billing.Note4))];
            var note5Cell = row.Cells[CellName(nameof(Billing.Note5))];
            var note6Cell = row.Cells[CellName(nameof(Billing.Note6))];
            var note7Cell = row.Cells[CellName(nameof(Billing.Note7))];
            var note8Cell = row.Cells[CellName(nameof(Billing.Note8))];

            var memo = Convert.ToString(memoCell.Value);
            var note2 = Convert.ToString(note2Cell.Value);
            var note3 = Convert.ToString(note3Cell.Value);
            var note4 = Convert.ToString(note4Cell.Value);
            var note5 = Convert.ToString(note5Cell.Value);
            var note6 = Convert.ToString(note6Cell.Value);
            var note7 = Convert.ToString(note7Cell.Value);
            var note8 = Convert.ToString(note8Cell.Value);


            using (var form = ApplicationContext.Create(nameof(PC0202)))
            {
                var screen = form.GetAll<PC0202>().First();
                screen.txtMemo.Text = memo;
                screen.txtNote2.Enabled = Note2Enabled;
                if (Note2Enabled) screen.txtNote2.Text = note2;
                screen.txtNote3.Text = note3;
                screen.txtNote4.Text = note4;
                screen.txtNote5.Text = note5;
                screen.txtNote6.Text = note6;
                screen.txtNote7.Text = note7;
                screen.txtNote8.Text = note8;
                screen.UseControlInputNote = UseControlInputNote;

                screen.InitializeParentForm("請求メモ/備考入力");

                var memoDialog = ApplicationContext.ShowDialog(ParentForm, form, true);
                if (memoDialog != DialogResult.OK) return;

                Modified |= screen.Modified;

                row.Cells["celMemoSymbol"].Value = screen.IsAnyNoteInputted ? "○" : "";
                memoCell.Value = screen.txtMemo.Text;
                note2Cell.Value = screen.txtNote2.Text;
                note3Cell.Value = screen.txtNote3.Text;
                note4Cell.Value = screen.txtNote4.Text;
                note5Cell.Value = screen.txtNote5.Text;
                note6Cell.Value = screen.txtNote6.Text;
                note7Cell.Value = screen.txtNote7.Text;
                note8Cell.Value = screen.txtNote8.Text;
            }
        }

        private void ShowBillingCategorySearchDialog(int rowIndex)
        {
            ClearStatusMessage();

            var billingCategory = this.ShowBillingCategorySearchDialog(useInput: true);
            if (billingCategory == null) return;
            SetBillingCategory(rowIndex, billingCategory);
            grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.TaxClassId))];
            grid.BeginEdit(true);
        }

        private void ShowTaxClassSearchDialog(int rowIndex)
        {
            ClearStatusMessage();

            var taxClass = this.ShowTaxClassSearchDialog();
            if (taxClass != null)
            {
                var cellTaxClassId = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.TaxClassId))];
                var cellTaxClassIdName = grid.Rows[rowIndex].Cells[CellName("TaxClassIdName")];
                cellTaxClassId.Value = taxClass.Id;
                cellTaxClassIdName.Visible = true;
                cellTaxClassIdName.Value = taxClass.Id + ":" + taxClass.Name;

                grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.AccountTitleCode))];
                grid.BeginEdit(true);
            }
        }

        private void ShowAccTitleSearchDialog(int rowIndex)
        {
            ClearStatusMessage();

            var accountTitle = this.ShowAccountTitleSearchDialog();

            if (accountTitle != null)
            {
                var cellAccountTitleCode = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.AccountTitleCode))];
                var cellAccountTitleName = grid.Rows[rowIndex].Cells[CellName("AccountTitleName")];
                cellAccountTitleCode.Value = accountTitle.Code;
                cellAccountTitleName.Visible = true;
                cellAccountTitleName.Value = accountTitle.Name;
            }

            if (!UseDiscount)
                grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.Note1))];
            else
                grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.UseDiscount))];

            grid.BeginEdit(true);
        }

        //private void ShowContractNumberSearchDialog(int rowIndex)
        //{
        //    ClearStatusMessage();
        //    var contract = this.ShowBillingDivisionContractSearchDialog();
        //    if (contract != null)
        //    {
        //        var cellContractNumber = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))];
        //        cellContractNumber.Value = contract.ContractNumber;
        //        if (ValidateContractNumber(rowIndex))
        //        {
        //            grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.Quantity))];
        //        }
        //        else
        //        {
        //            cellContractNumber.Value = null;
        //            grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))];
        //        }
        //        grid.BeginEdit(true);
        //    }
        //}


        #endregion

        #region calculate tax / price / total

        /// <summary>管理コードから、金額端数処理を取得する処理
        /// 事前に GeneralSetting に マスターのデータを格納しておく必要がある</summary>
        /// <param name="code">管理コード</param>
        private RoundingType GetRoundingType(string code)
        {
            var value = GetGeneralSettingValue(code);
            if (string.IsNullOrEmpty(value)) return RoundingType.Floor;
            return (RoundingType)int.Parse(value);
        }

        /// <summary>税抜 → 外税 計算</summary>
        private decimal CalculateExclusiveTax(decimal amount) => CalculateTax(rate => (amount * rate));
        /// <summary>税込 → 内税 計算</summary>
        private decimal CalculateInclusiveTax(decimal amount) => CalculateTax(rate => (amount * rate / (1M + rate)));

        private decimal CalculateTax(Func<decimal, decimal> calculateFunction)
        {
            var rate = GetTaxRate(datBilledAt.Value);
            if (rate == 0M) return 0M;
            return TaxCalculationRoundingType.Calc(calculateFunction(rate), Precision) ?? 0M;
        }

        /// <summary>管理マスター <see cref="Generalsetting"/>から、日付を渡して 税率を取得する
        /// 1989年 以前の日付を渡しても 旧消費税率 3% が連携される
        /// 管理マスターでは、% * 100 の値を登録している
        ///   3 % → 0300
        ///   5 % → 0500
        ///   8 % → 0800
        ///  10 % → 1000 という文字列となっている</summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private decimal GetTaxRate(DateTime? date)
        {
            if (!date.HasValue) return 0M;

            var applyDate1 = GetGeneralSettingValue("新税率開始年月日");
            var applyDate2 = GetGeneralSettingValue("新税率開始年月日2");
            var applyDate3 = GetGeneralSettingValue("新税率開始年月日3");

            var target = date.Value.ToString("yyyyMMdd");
            var key = string.Empty;
            if (target.CompareTo(applyDate1) < 0) key = "旧消費税率";
            else if (applyDate1.CompareTo(target) <= 0 && target.CompareTo(applyDate2) < 0) key = "新消費税率";
            else if (applyDate2.CompareTo(target) <= 0 && target.CompareTo(applyDate3) < 0) key = "新消費税率2";
            else if (applyDate3.CompareTo(target) <= 0) key = "新消費税率3";
            if (string.IsNullOrEmpty(key)) return 0M;
            var value = GetGeneralSettingValue(key);
            var result = 0M;
            return decimal.TryParse(value, out result) ? (result / 10000M) : 0M;
        }

        /// <summary>数量/単価 から 金額（抜)/消費税/請求額 を設定
        /// 金額計算が"自動" の場合のみ実施
        /// 税区分 1:外税 の場合のみ税計算を行う
        /// 上記以外の区分の場合は、金額（抜）にも値を設定しない</summary>
        private void SetBillingAmounFromQuantityWithUnitPrice(int rowIndex)
        {
            if (rdoManual.Checked) return;
            var row = grid.Rows[rowIndex];

            var cellQuantity = row.Cells[CellName(nameof(Billing.Quantity))];
            var cellUnitPrice = row.Cells[CellName(nameof(Billing.UnitPrice))];
            var cellPrice = row.Cells[CellName(nameof(Billing.Price))];
            var cellTaxAmount = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellBillingAmount = row.Cells[CellName(nameof(Billing.BillingAmount))];

            var quantity = Convert.ToDecimal(cellQuantity.Value);
            var unitPrice = Convert.ToDecimal(cellUnitPrice.Value);
            var amount = PriceCalculationRoundingType.Calc(quantity * unitPrice, Precision) ?? 0M;
            var taxId = GetTaxClassId(rowIndex);

            if (taxId != (int)TaxClassId.TaxExclusive)
            {
                cellPrice.Value = null;
                cellTaxAmount.Value = null;
                cellBillingAmount.Value = amount;
                return;
            }

            if (quantity == 0M || unitPrice == 0M || amount == 0M)
            {
                cellPrice.Value = null;
                cellTaxAmount.Value = null;
                cellBillingAmount.Value = null;
                return;
            }
            var tax = CalculateExclusiveTax(amount);
            cellPrice.Value = amount;
            cellTaxAmount.Value = tax;
            var billingAmount = amount + tax;
            if (billingAmount <= MaxAmount)
                cellBillingAmount.Value = billingAmount;
        }

        /// <summary>金額（抜） から、請求額を設定 税区 1:外税のみ動作</summary>
        private void SetBillingAmountFromPrice(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var cellPrice = row.Cells[CellName(nameof(Billing.Price))];
            var cellTaxAmount = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellBillingAmount = row.Cells[CellName(nameof(Billing.BillingAmount))];

            var price = (decimal?)cellPrice.EditedFormattedValue;
            if (!price.HasValue)
            {
                cellTaxAmount.Value = null;
                cellBillingAmount.Value = null;
                return;
            }

            var taxId = GetTaxClassId(rowIndex);
            var isTax = (taxId == (int)TaxClassId.TaxExclusive) || (taxId == (int)TaxClassId.TaxInclusive);
            var tax = isTax ? CalculateExclusiveTax(price.Value): 0;
            cellTaxAmount.Value = tax;
            var amount = price + tax;
            if (amount <= MaxAmount) cellBillingAmount.Value = amount;
        }

        /// <summary>消費税 から、金額（抜） または 請求額を設定 照合ロジック オプションで動作変化</summary>
        private void SetPriceOrBillingAmountFromTax(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var cellPrice = row.Cells[CellName(nameof(Billing.Price))];
            var cellTax = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellAmount = row.Cells[CellName(nameof(Billing.BillingAmount))];
            var price = (decimal?)cellPrice.EditedFormattedValue;
            var tax = (decimal?)(cellTax.EditedFormattedValue);
            var amount = (decimal?)cellAmount.EditedFormattedValue;
            if (!tax.HasValue) return;

            if (!cellAmount.Enabled)
            {
                cellAmount.Value = (price ?? 0M) + (tax ?? 0M);
            }
            else
            {
                cellPrice.Value = (amount ?? 0M) - (tax ?? 0M);
            }

        }

        /// <summary>請求額 から、金額（抜） と 消費税 を設定</summary>
        private void SetPriceAndTaxFromBillingAmount(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var cellPrice = row.Cells[CellName(nameof(Billing.Price))];
            var cellTaxAmount = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellBillingAmount = row.Cells[CellName(nameof(Billing.BillingAmount))];

            var amount = (decimal?)cellBillingAmount.EditedFormattedValue;

            if (!amount.HasValue)
            {
                cellPrice.Value = null;
                cellTaxAmount.Value = null;
                return;
            }

            var taxId = GetTaxClassId(rowIndex);
            var isTax = (taxId == (int)TaxClassId.TaxExclusive) || (taxId == (int)TaxClassId.TaxInclusive);
            var tax = isTax ? CalculateInclusiveTax(amount.Value) : 0;
            var price = (amount ?? 0M) - tax;
            cellPrice.Value = price;
            cellTaxAmount.Value = tax;
        }

        private void CalculateTotalAmount()
        {
            var totalPrice = 0M;
            var totalTax = 0M;
            var totalAmount = 0M;

            var exclusiveAmount = 0M;

            for (var i = 0; i < grid.RowCount; i++)
            {
                var cellPrice = grid.Rows[i].Cells[CellName(nameof(Billing.Price))];
                var cellTax = grid.Rows[i].Cells[CellName(nameof(Billing.TaxAmount))];
                var cellAmount = grid.Rows[i].Cells[CellName(nameof(Billing.BillingAmount))];

                var price = Convert.ToDecimal(cellPrice.EditedFormattedValue);
                var tax = Convert.ToDecimal(cellTax.EditedFormattedValue);
                var amount = Convert.ToDecimal(cellAmount.EditedFormattedValue);

                totalPrice += price;
                totalTax += tax;
                totalAmount += amount;

                var taxId = GetTaxClassId(i);
                if (taxId == (int)TaxClassId.TaxExclusive || taxId == (int)TaxClassId.TaxInclusive)
                    exclusiveAmount += price;
            }
            var taxById = CalculateExclusiveTax(exclusiveAmount);

            lblTotalPrice.Text = totalPrice.ToString(AmountFormat);
            lblTotalTaxAmount.Text = totalTax.ToString(AmountFormat);
            lblTotalTaxByInputId.Text = taxById.ToString(AmountFormat);
            lblTotalBillingAmount.Text = totalAmount.ToString(AmountFormat);

        }

        private int? GetTaxClassId(int rowIndex)
        {
            if (rowIndex < 0 || MaxGridRowCount <= rowIndex) return null;
            var cell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.TaxClassId))];
            var id = 0;
            return int.TryParse(cell.Value?.ToString(), out id) ? (int?)id : null;
        }

        #endregion

        #region call web service

        private async Task LoadColumnNameSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterService.ColumnNameSettingMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    ColumnNames = result.ColumnNames.Where(x => x.TableName == nameof(Billing)).ToList();
            });

        private async Task LoadInputControlAsync()
            => await ServiceProxyFactory.DoAsync(async (InputControlMasterService.InputControlMasterClient client) => {
                var result = await client.GetAsync(SessionKey, CompanyId, Login.UserId, InputGridType.Billing);
                if (result.ProcessResult.Result)
                    InputControls = result.InputControls;
            });

        private async Task LoadCollationSettingAsync()
            => await ServiceProxyFactory.DoAsync(async (CollationSettingMasterService.CollationSettingMasterClient client) => {
                var result = await client.GetAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    CollationSetting = result.CollationSetting;
            });

        private async Task LoadTaxClassAsync()
            => await ServiceProxyFactory.DoAsync(async (TaxClassMasterService.TaxClassMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey);
                if (result.ProcessResult.Result)
                    TaxClasses = result.TaxClass;
            });

        private async Task LoadAccountTitlesAsync()
            => await ServiceProxyFactory.DoAsync(async (AccountTitleMasterService.AccountTitleMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, new AccountTitleSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    AccountTitles = result.AccountTitles;
            });

        private async Task LoadGeneralSettingsAsync()
            => await ServiceProxyFactory.DoAsync(async (GeneralSettingMasterService.GeneralSettingMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    GeneralSetting = result.GeneralSettings;
            });

        private async Task LoadCategoriesAsync()
            => await ServiceProxyFactory.DoAsync(async (CategoryMasterService.CategoryMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, new CategorySearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    Categories = result.Categories;
            });

        private async Task<Currency> GetCurrencyAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CurrencyMasterService.CurrencyMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result)
                    return result.Currencies.FirstOrDefault();
                return null;
            });

        private async Task<Department> GetDepartmentAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (DepartmentMasterService.DepartmentMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result)
                    return result.Departments.FirstOrDefault();
                return null;
            });

        private async Task<Staff> GetStaffAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (StaffMasterService.StaffMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result)
                    return result.Staffs.FirstOrDefault();
                return null;
            });

        private async Task GetUseControlInputNoteAsync()
            => UseControlInputNote = await Util.IsControlInputNoteAsync(Login);

        #endregion

        #region load data
        private string GetGeneralSettingValue(string code) => GeneralSetting?.FirstOrDefault(x => x.Code == code)?.Value;

        private Category GetCategory(Predicate<Category> filter) => Categories.FirstOrDefault(x => filter(x));
        private Category GetBillingCategory(Predicate<Category> filter)
            => GetCategory(x => x.CategoryType == CategoryType.Billing && filter(x));
        private Category GetBillingCategoryByCode(string code) => GetBillingCategory(x => x.Code == code);

        #endregion
    }
}
