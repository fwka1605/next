using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.AccountTitleMasterService;
using Rac.VOne.Client.Screen.BillingDivisionContractMasterService;
using Rac.VOne.Client.Screen.BillingDivisionSettingMasterService;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CollationSettingMasterService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.DestinationMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.HolidayCalendarMasterService;
using Rac.VOne.Client.Screen.InputControlMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Client.Screen.TaxClassMasterService;
using Rac.VOne.Client.Screen.InvoiceSettingService;
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
    /// <summary>請求データ入力</summary>
    public partial class PC0201 : VOneScreenBase
    {
        #region variable decleration
        internal VOneScreenBase ReturnScreen { get; set; }
        private enum InputMode
        {
            /// <summary>単独</summary>
            Default,
            /// <summary>参照新規</summary>
            InputFrom,
            /// <summary>請求データ検索</summary>
            Search,
            /// <summary>簡易請求書発行</summary>
            Invoice
        }
        /// <summary>入力画面の状態</summary>
        private InputMode Mode
        {
            get
            {
                if (ReturnScreen is PC0301) return InputMode.Search;
                if (ReturnScreen == null)
                {
                    if (CurrentBilling == null) return InputMode.Default;
                    return InputMode.InputFrom;
                }

                return InputMode.Invoice;
            }
        }

        private DataExpression Expression { get; set; }
        internal Billing CurrentBilling { get; set; }
        private List<Billing> CurrentGridBilling { get; set; }
        private List<InputControl> InputControls { get; set; }
        private CollationSetting CollationSetting { get; set; }
        private List<Staff> Staffs { get; set; }
        private List<Currency> Currencies { get; set; }
        private List<Department> Departments { get; set; }
        private List<AccountTitle> AccountTitles { get; set; }
        private List<HolidayCalendar> Holidays { get; set; }
        private List<Category> BillingCategories { get; set; }
        private List<Category> CollectCategories { get; set; }
        private List<GeneralSetting> GeneralSettings { get; set; }
        private List<ColumnNameSetting> ColumnSettings { get; set; }
        private List<ReportSetting> ReportSetting { get; set; }

        /// <summary>請求書単位で消費税計算を行う</summary>
        private bool CalculateTaxByInputId => CollationSetting?.CalculateTaxByInputId == 1;
        /// <summary>消費税計算用 端数処理タイプ</summary>
        private RoundingType TaxCalculationRoundingType { get; set; }
        /// <summary>金額（数量 * 単価) 計算用 端数処理タイプ</summary>
        private RoundingType PriceCalculationRoundingType { get; set; }

        private List<TaxClass> TaxClasses { get; set; }
        private Customer SelectedCustomer { get; set; }
        private int Precision { get; set; }
        private string AmountFormat { get; set; }
        private int StaffId { get; set; }
        private int CurrencyId { get; set; }
        private int CustomerId { get; set; }
        private int DepartmentId { get; set; }
        private int? DestinationId { get; set; }
        private string CellName(string value) => $"cel{value}";
        private int MaxGridRowCount { get; set; }
        private bool UseControlInputNote { get; set; } = false;

        private const int DefaultMaxGridRowCount = 10;
        private const decimal MaxAmount = 99999999999.99999M;

        #endregion

        #region Initialization
        public PC0201()
        {
            InitializeComponent();
            grid.GridColorType = GridColorType.Input;
            grid.SetupShortcutKeys();
            grid.EditMode = EditMode.EditOnEnter;
            Text = "請求データ入力";
            InitializeHandlers();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction05Caption("入力設定");
            BaseContext.SetFunction07Caption("参照新規");
            BaseContext.SetFunction08Caption("行コピー");
            BaseContext.SetFunction09Caption("検索");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Save;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = Delete;
            OnF05ClickHandler = ShowInputSettingMaster;
            OnF07ClickHandler = ShowInputPattern;
            OnF08ClickHandler = CopyGridRowData;
            OnF09ClickHandler = ShowCodeSearchDialog;
            OnF10ClickHandler = Exit;
        }

        private void PC0201_Load(object sender, EventArgs e)
        {
            SetScreenName();
            try
            {
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            Modified = false;
        }

        private async Task InitializeLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadGridSettingAsync(),
                LoadCollationSettingInfoAsync(),
                LoadCurrencyInfoAsync(),
                LoadStaffInfoAsync(),
                LoadDepartmentInfoAsync(),
                LoadAccountTitleInfoAsync(),
                LoadHolidayCalendarInfoAsync(),
                LoadInputControlInfoAsync(),
                LoadBillingCategoryAsync(),
                LoadCollectCategoryAsync(),
                LoadGeneralSettingAsync(),
                LoadTaxClassAsync(),
                GetUseControlInputNoteAsync(),
            };
            await Task.WhenAll(tasks);
            if (tasks.Any(x => x.Exception != null))
                foreach (var task in tasks)
                    NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
            InitializeTextBoxFormats();
            GetMaxGridRowCount();
            CommonFormLoad();
            InitializeControls();

            TaxCalculationRoundingType = GetRoundingType("消費税計算端数処理");
            PriceCalculationRoundingType = GetRoundingType("金額計算端数処理");
            if (Mode != InputMode.Default)
            {
                await LoadCurrentGridBillingAsync();
                SetGridRowCount();
                await SetBillingFormAsync();
                CalculateTotalAmount();
            }

            rdoAuto.Checked = Common.Settings.Settings.RestoreControlValue<PC0201, bool>(Login, rdoAuto.Name) ?? false;
        }

        private void InitializeTextBoxFormats()
        {
            Expression = new DataExpression(ApplicationControl);

            txtCustomerCode.MaxLength = Expression.CustomerCodeLength;
            txtCustomerCode.Format = Expression.CustomerCodeFormatString;
            txtCustomerCode.PaddingChar = Expression.CustomerCodePaddingChar;
            txtCustomerCode.ImeMode = Expression.CustomerCodeImeMode();

            txtStaffCode.MaxLength = Expression.StaffCodeLength;
            txtStaffCode.Format = Expression.StaffCodeFormatString;
            txtStaffCode.PaddingChar = Expression.StaffCodePaddingChar;

            txtDepartmentCode.MaxLength = Expression.DepartmentCodeLength;
            txtDepartmentCode.Format = Expression.DepartmentCodeFormatString;
            txtDepartmentCode.PaddingChar = Expression.DepartmentCodePaddingChar;

            txtDestinationCode.MaxLength = 2;
            txtDestinationCode.Format = "9";
            txtDestinationCode.PaddingChar = '0';
        }

        private void InitializeControls()
        {
            var statusText = string.Empty;
            var statusColor = Color.Cyan;
            var isEditable = true;
            var isStandardInput = false;
            var isFirstRowOnly = false;

            switch (Mode)
            {
                case InputMode.Default:
                case InputMode.InputFrom:
                    statusText = "新規";
                    isStandardInput = true;
                    break;
                case InputMode.Search:
                    isEditable = !(CurrentBilling.AssignmentFlag != 0
                                || CurrentBilling.OutputAt != null
                                || CurrentBilling.InputType == (int)BillingInputType.CashOnDueDate
                                || CurrentBilling.ResultCode == 0
                                || CurrentBilling.DeleteAt != null
                                || CurrentBilling.BillingInputPublishAt != null);
                    isFirstRowOnly = isEditable && CurrentBilling.InputType == (int)BillingInputType.Importer;
                    statusText = isEditable ? "修正" : "表示";
                    statusColor = Color.Yellow;
                    break;
                case InputMode.Invoice:
                    isEditable = false;
                    statusText = "表示";
                    statusColor = Color.Yellow;
                    break;
            }

            lblStatus.Text = statusText;
            lblStatus.BackColor = statusColor;


            lblInputType.Visible = !isStandardInput;
            lblInputTypeName.Visible = !isStandardInput;
            if (lblInputType.Visible && CurrentBilling != null)
                lblInputTypeName.Text = CurrentBilling.InputType == (int)BillingInputType.Importer ? "取込" :
                                        CurrentBilling.InputType == (int)BillingInputType.BillingInput ? "入力" : "定期請求";

            rdoAuto.Enabled = isStandardInput;
            rdoManual.Enabled = isStandardInput;

            gbxHeader.Enabled = isEditable;
            grid.ReadOnly = !isEditable;

            pnlTaxByInputId.Visible = CalculateTaxByInputId;

            if (CurrentBilling != null)
            {
                if (string.IsNullOrEmpty(CurrentBilling.BillingCategoryCode))
                {
                    var category = GetInputBillingCateogry(x => x.Id == CurrentBilling.BillingCategoryId);
                    CurrentBilling.BillingCategoryCode = category.Code;
                    CurrentBilling.BillingCategoryName = category.Name;
                }
                Precision = Currencies.FirstOrDefault(x => x.Id == CurrentBilling.CurrencyId).Precision;
            }
            else
            {
                Precision = Currencies.FirstOrDefault(x => x.Code == DefaultCurrencyCode).Precision;
            }

            InitializeGridTemplate();

            if (isFirstRowOnly)
            {
                grid.Rows[0].Cells[CellName("ClearButton")].Enabled = false;
                foreach (var row in grid.Rows.Skip(1))
                    foreach (var cell in row.Cells)
                    {
                        cell.Enabled = false;
                        cell.Selectable = false;
                    }
            }

            BaseContext.SetFunction01Enabled(isEditable);
            if (!(isStandardInput))
            {
                BaseContext.SetFunction02Caption("");
                BaseContext.SetFunction02Enabled(false);
                if (Mode == InputMode.Search)
                    BaseContext.SetFunction03Enabled(isEditable);
                BaseContext.SetFunction05Enabled(false);
                BaseContext.SetFunction07Caption("");
                BaseContext.SetFunction07Enabled(false);
                BaseContext.SetFunction08Enabled(isEditable && !isFirstRowOnly);
                BaseContext.SetFunction10Caption("戻る");
            }
        }

        private async Task SetBillingFormAsync()
        {
            if (((CurrentBilling.InputType == (int)BillingInputType.Importer || CurrentBilling.InputType == (int)BillingInputType.CashOnDueDate)
                  && (Mode == InputMode.InputFrom
                   || Mode == InputMode.Search
                   || Mode == InputMode.Invoice))
                || (CurrentBilling.InputType == (int)BillingInputType.BillingInput && Mode == InputMode.Invoice))
            {
                var isInvoicePublish = CurrentBilling.InputType == (int)BillingInputType.Importer && CurrentBilling.BillingInputId.HasValue
                                        && (CurrentGridBilling?.Any() ?? false);
                var billing = !isInvoicePublish ? CurrentBilling : CurrentGridBilling[0];
                var billings = !isInvoicePublish ? new List<Billing> { CurrentBilling } : CurrentGridBilling;
                await SetBillingHeaderAsync(billing);
                SetBillingDetails(billings);
            }
            else if ((CurrentBilling.InputType == (int)BillingInputType.BillingInput && (CurrentGridBilling?.Any() ?? false))
                && (Mode == InputMode.InputFrom || Mode == InputMode.Search))
            {
                await SetBillingHeaderAsync(CurrentGridBilling[0]);
                SetBillingDetails(CurrentGridBilling);
            }
            else if ((CurrentBilling.InputType == (int)BillingInputType.PeriodicBilling && (CurrentGridBilling?.Any() ?? false))
                && (Mode == InputMode.Search))
            {
                await SetBillingHeaderAsync(CurrentGridBilling[0]);
                SetBillingDetails(CurrentGridBilling);
            }
        }

        private async Task SetBillingHeaderAsync(Billing billing)
        {
            txtInvoiceCode.Text = billing.InvoiceCode;

            if (Mode != InputMode.InputFrom)
            {
                txtBillingInputId.Text = billing.BillingInputId.ToString();
                datBilledAt.Value = billing.BilledAt;
                datSalesAt.Value = billing.SalesAt;
                datClosingAt.Value = billing.ClosingAt;
                datDueAt.Value = billing.DueAt;
            }
            var item = cmbCategory.Items.Cast<GrapeCity.Win.Editors.ListItem>()
                .FirstOrDefault(x => (int)x.SubItems[1].Value == billing.CollectCategoryId);
            if (item != null) cmbCategory.SelectedItem = item;

            SetStaffInfo(billing.StaffId);
            SetCurrencyInfo(billing.CurrencyId);
            await SetCustomerInfo(billing.CustomerId);
            SetDepartmentInfo(billing.DepartmentId);

            if (billing.DestinationId.HasValue)
            {
                var destination = (await GetDestinationsAsync(billing.CustomerId)).FirstOrDefault(x => x.Id == billing.DestinationId);
                if (destination != null)
                {
                    DestinationId = destination.Id;
                    txtDestinationCode.Text = destination.Code;
                    lblDestinationName.Text = destination.ToString();
                }
            }

        }

        /// <summary>請求データをグリッドに設定する。</summary>
        /// <param name="startRow">グリッドの開始行(0 origin)</param>
        /// <param name="billings">請求データ</param>
        private void SetBillingDetails(IEnumerable<Billing> billings)
        {
            var take = Math.Min(grid.RowCount, billings.Count());
            var target = billings.Take(take).ToArray();
            var i = 0;
            foreach (var billing in billings.Take(take))
            {
                var row = grid.Rows[i];
                //債権科目
                if (billing.DebitAccountTitleId != null)
                {
                    var accountTitleId = billing.DebitAccountTitleId.Value;
                    var accountTitleCode = AccountTitles.FirstOrDefault(x => x.Id == accountTitleId)?.Code;
                    SettingAccountTitle(i, accountTitleCode);
                }
                if (Mode != InputMode.InputFrom)
                {
                    row.Cells[CellName(nameof(Billing.SalesAt))].Value = billing.SalesAt;
                    row.Cells[CellName(nameof(Billing.BillingId))].Value = billing.Id;
                    row.Cells[CellName(nameof(Billing.ContractNumber))].Value = billing.ContractNumber;
                }
                row.Cells[CellName(nameof(Billing.Quantity))].Value = billing.Quantity;
                row.Cells[CellName(nameof(Billing.UnitSymbol))].Value = billing.UnitSymbol;
                row.Cells[CellName(nameof(Billing.UnitPrice))].Value = billing.UnitPrice;
                row.Cells[CellName(nameof(Billing.Price))].Value = billing.Price;
                row.Cells[CellName(nameof(Billing.TaxAmount))].Value = billing.TaxAmount;
                row.Cells[CellName(nameof(Billing.BillingAmount))].Value = billing.BillingAmount;
                row.Cells[CellName(nameof(Billing.Memo))].Value = billing.Memo;
                row.Cells[CellName("MemoSymbol")].Value = billing.IsAnyNoteInputted ? "○" : "";
                row.Cells[CellName(nameof(Billing.Note1))].Value = billing.Note1;
                row.Cells[CellName(nameof(Billing.Note2))].Value = billing.Note2;
                row.Cells[CellName(nameof(Billing.Note3))].Value = billing.Note3;
                row.Cells[CellName(nameof(Billing.Note4))].Value = billing.Note4;
                row.Cells[CellName(nameof(Billing.Note5))].Value = billing.Note5;
                row.Cells[CellName(nameof(Billing.Note6))].Value = billing.Note6;
                row.Cells[CellName(nameof(Billing.Note7))].Value = billing.Note7;
                row.Cells[CellName(nameof(Billing.Note8))].Value = billing.Note8;
                row.Cells[CellName(nameof(Billing.BillingDiscountId))].Value = billing.BillingDiscountId;
                SetCategory(i, BillingCategories.FirstOrDefault(x => x.Id == billing.BillingCategoryId));
                if (UseDiscount && billing.BillingDiscountId.HasValue)
                    row.Cells[CellName(nameof(Billing.UseDiscount))].Value = 1;
                SetTaxClass(i, TaxClasses.FirstOrDefault(x => x.Id == billing.TaxClassId));
                i++;
            }
        }

        private void SetCurrencyInfo(int id)
        {
            if (id != 0)
            {
                Currency currency = Currencies.Find(c => c.Id == id);
                if (currency == null) return;

                CurrencyId = currency.Id;
                txtCurrencyCode.Text = currency.Code;
                SetCurrencyDisplayString(Precision);
            }
        }

        private async Task SetCustomerInfo(int id)
        {
            if (id != 0 && CustomerId != id)
            {
                var customer = await GetCustomerByIdAsync(id);
                if (customer == null) return;

                CustomerId = customer.Id;
                txtCustomerCode.Text = customer.Code;
                lblCustomerName.Text = customer.Name;

                SelectedCustomer = customer;

                txtDestinationCode.Enabled = true;
                btnDestination.Enabled = true;
            }
        }

        private void SetStaffInfo(int id)
        {
            if (id == 0) return;
            var staff = Staffs.FirstOrDefault(c => c.Id == id);
            if (staff == null) return;

            StaffId = staff.Id;
            txtStaffCode.Text = staff.Code;
            lblStaffName.Text = staff.Name;
        }

        private void SetDepartmentInfo(int id)
        {
            if (id == 0) return;
            var department = Departments.FirstOrDefault(c => c.Id == id);
            if (department == null) return;
            DepartmentId = department.Id;
            txtDepartmentCode.Text = department.Code;
            lblDepartmentName.Text = department.Name;
        }

        private Category GetInputBillingCateogry(Predicate<Category> filter)
        {
            return BillingCategories
                .FirstOrDefault(x => x.UseInput == 1 && filter(x));
        }

        #endregion

        #region initialize grid template

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

        private void InitializeGridTemplate()
        {
            var template = new Template();
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);

            var height = builder.DefaultRowHeight;
            var widthDiscount = UseDiscount ? 110 : 0;
            var widthNote1 = UseDiscount ? 330 : 440;

            var posX = UseDiscount
                ?   new[] {
                    0, 110, 220, 310, 420, 530,           860, 890,
                    0, 110, 220, 310, 420, 530, 640, 750,      890,
                } : new[] {
                    0, 110, 220, 310, 420, 420,           860, 890,
                    0, 110, 220, 310, 420, 530, 640, 750,      890,
                }; // 1行目 歩引きの影響で消える項目の初期値変更だけ
            var posY = new[] { 0, height };

            #region ヘッダ
            builder.Items.AddRange(new[]
            {
                #region ヘッダー1
                new CellSetting(height, Width =           110, nameof(Billing.SalesAt)            , location: new Point(posX[0], posY[0]), caption: "売上日"  ),
                new CellSetting(height, Width =           110, nameof(Billing.BillingCategoryCode), location: new Point(posX[1], posY[0]), caption: "請求区分"),
                new CellSetting(height, Width =            90, nameof(Billing.TaxClassId)         , location: new Point(posX[2], posY[0]), caption: "税区"    ),
                new CellSetting(height, Width =           110, nameof(Billing.DebitAccountTitleId), location: new Point(posX[3], posY[0]), caption: "債権科目"),
                new CellSetting(height, Width = widthDiscount, nameof(Billing.UseDiscount)        , location: new Point(posX[4], posY[0]), caption: "歩引計算"),
                new CellSetting(height, Width =    widthNote1, nameof(Billing.Note1)              , location: new Point(posX[5], posY[0]), caption: GetColumnName(nameof(Billing.Note1))),
                new CellSetting(height, Width =            30, nameof(Billing.Memo)               , location: new Point(posX[6], posY[0]) ),
                new CellSetting(height, Width =            40, "MemoButton"                       , location: new Point(posX[7], posY[0]) ),
                #endregion

                #region ヘッダー2
                new CellSetting(height, Width =           110, nameof(Billing.BillingId)          , location: new Point(posX[ 8], posY[1]), caption: "請求ID"  ),
                new CellSetting(height, Width =           110, nameof(Billing.ContractNumber)     , location: new Point(posX[ 9], posY[1]), caption: "契約番号"),
                new CellSetting(height, Width =            90, nameof(Billing.Quantity)           , location: new Point(posX[10], posY[1]), caption: "数量"    ),
                new CellSetting(height, Width =           110, nameof(Billing.UnitSymbol)         , location: new Point(posX[11], posY[1]), caption: "単位"    ),
                new CellSetting(height, Width =           110, nameof(Billing.UnitPrice)          , location: new Point(posX[12], posY[1]), caption: "単価"    ),
                new CellSetting(height, Width =           110, nameof(Billing.Price)              , location: new Point(posX[13], posY[1]), caption: "金額(抜)"),
                new CellSetting(height, Width =           110, nameof(Billing.TaxAmount)          , location: new Point(posX[14], posY[1]), caption: "消費税"  ),
                new CellSetting(height, Width =           140, nameof(Billing.BillingAmount)      , location: new Point(posX[15], posY[1]), caption: "請求額"  ),
                new CellSetting(height, Width =            40, "ClearButton"                      , location: new Point(posX[16], posY[1]) ),
                #endregion
            });
            builder.BuildHeaderOnly(template);
            #endregion

            #region データ
            builder.Items.Clear();
            builder.Items.AddRange(GetRowCellSettings(builder, posX, posY, widthDiscount, widthNote1));
            builder.BuildRowOnly(template);
            #endregion

            grid.Template = template;
            grid.CurrentCellBorderLine = builder.GetLine(LineStyle.Thick);
            grid.MultiSelect = false;
            grid.RowCount = MaxGridRowCount;
            grid.HideSelection = false;

            for (var i = 0; i < grid.RowCount; i++)
            {
                var cellContractNo = grid.Rows[i].Cells[CellName(nameof(Billing.ContractNumber))];
                cellContractNo.Enabled = false;
                cellContractNo.Selectable = false;
            }
        }

        private string GetColumnName(string columnName) =>
            ColumnSettings.FirstOrDefault(x => x.ColumnName == columnName)?.DisplayColumnName;

        private InputControl GetInputControl(string columnName) =>
            InputControls?.FirstOrDefault(x => x.ColumnName == columnName);

        private IEnumerable<CellSetting> GetRowCellSettings(GcMultiRowTemplateBuilder builder,
            int[] posX, int[] posY, int wdDct, int wdNt1)
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
            yield return new CellSetting(height,   110, name, border: bdTp, location: new Point(posX[0], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 0, tabStop: input?.IsTabStop ?? true, cell: builder.GetDateCell_yyyyMMdd(MultiRowContentAlignment.MiddleCenter, true));
            input = GetInputControl(name = nameof(Billing.BillingCategoryId));
            yield return new CellSetting(height,   110, name, border: bdTp, location: new Point(posX[1], posY[0]), dataField: name, readOnly: true, visible: false, cell: builder.GetTextBoxCell());
            name = nameof(Billing.BillingCategoryCode);
            yield return new CellSetting(height,   110, name, border: bdTp, location: new Point(posX[1], posY[0]), dataField: name, readOnly: false, visible: true, tabIndex: input?.TabIndex ?? 1, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(ime: ImeMode.Disable, format: "9", maxLength: 2));
            name = nameof(Billing.BillingCategoryName);
            yield return new CellSetting(height,   110, name, border: bdTp, location: new Point(posX[1], posY[0]), dataField: name, readOnly: true, visible: false, tabIndex: input?.TabIndex ?? 1, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell());
            name = nameof(Billing.UseLongTermAdvancedReceived);
            yield return new CellSetting(height,   110, name, border: bdTp, location: new Point(posX[1], posY[0]), dataField: name, readOnly: true, visible: false, cell: builder.GetTextBoxCell());
            input = GetInputControl(name = nameof(Billing.TaxClassId));
            yield return new CellSetting(height,    90, name, border: bdTp, location: new Point(posX[2], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 2, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(ime: ImeMode.Disable, format: "9", maxLength: 1));
            name = "TaxClassIdName";
            yield return new CellSetting(height,    90, name, border: bdTp, location: new Point(posX[2], posY[0]), readOnly: true, visible: false, tabIndex: input?.TabIndex ?? 2, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell());
            input = GetInputControl(name = nameof(Billing.DebitAccountTitleId));
            yield return new CellSetting(height,   110, name, border: bdTp, location: new Point(posX[1], posY[0]), dataField: name, readOnly: true, visible: false, cell: builder.GetTextBoxCell());
            name = nameof(Billing.AccountTitleCode);
            yield return new CellSetting(height,   110, name, border: bdTp, location: new Point(posX[3], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 3, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(ime: ImeMode.Disable, format: Expression.AccountTitleCodeFormatString, maxLength: Expression.AccountTitleCodeLength));
            name = "AccountTitleName";
            yield return new CellSetting(height,   110, name, border: bdTp, location: new Point(posX[3], posY[0]), readOnly: true, visible: false, tabIndex: input?.TabIndex ?? 3, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell());
            input = GetInputControl(name = nameof(Billing.UseDiscount));
            yield return new CellSetting(height, wdDct, name, border: bdTp, location: new Point(posX[4], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 4, tabStop: input?.IsTabStop ?? true, cell: builder.GetCheckBoxCell());
            input = GetInputControl(name = nameof(Billing.Note1));
            yield return new CellSetting(height, wdNt1, name, border: bdTp, location: new Point(posX[5], posY[0]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 5, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(maxLength: maxLength, ime: ImeMode.Hiragana, maxLengthUnit: maxLengthUnit));
            name = "MemoSymbol";
            yield return new CellSetting(height,    30, name, border: bdTp, location: new Point(posX[6], posY[0]), tabStop: false, cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter));
            name = "MemoButton";
            yield return new CellSetting(height,    40, name, border: bdTp, location: new Point(posX[7], posY[0]), tabStop: false, cell: builder.GetButtonCell(), value: "備考");
            #endregion
            var bdBm = builder.GetBorder(left: dotted, top: dotted, right: dotted);
            #region データ2
            name = nameof(Billing.BillingId);
            yield return new CellSetting(height,   110, name, border: bdBm, location: new Point(posX[8], posY[1]), enabled: false, dataField: name, readOnly: false, tabStop: false);
            input = GetInputControl(name = nameof(Billing.ContractNumber));
            yield return new CellSetting(height,   110, name, border: bdBm, location: new Point(posX[9], posY[1]), enabled: false, dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 6, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(ime: ImeMode.Disable, format: "A9", maxLength: 20));
            input = GetInputControl(name = nameof(Billing.Quantity));
            yield return new CellSetting(height,    90, name, border: bdBm, location: new Point(posX[10], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 7, tabStop: input?.IsTabStop ?? true, cell: GetQuantityNumberCell(builder));
            input = GetInputControl(name = nameof(Billing.UnitSymbol));
            yield return new CellSetting(height,   110, name, border: bdBm, location: new Point(posX[11], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 8, tabStop: input?.IsTabStop ?? true, cell: builder.GetTextBoxCell(maxLength: 3, ime: ImeMode.Hiragana));
            input = GetInputControl(name = nameof(Billing.UnitPrice));
            yield return new CellSetting(height,   110, name, border: bdBm, location: new Point(posX[12], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 9, tabStop: input?.IsTabStop ?? true, cell: GetUnitPriceNumberCell(builder, Precision, Precision, 0));
            input = GetInputControl(name = nameof(Billing.Price));
            yield return new CellSetting(height,   110, name, border: bdBm, location: new Point(posX[13], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 10, tabStop: input?.IsTabStop ?? true, cell: GetPriceNumberCell(builder, Precision, Precision, 0));
            input = GetInputControl(name = nameof(Billing.TaxAmount));
            yield return new CellSetting(height,   110, name, border: bdBm, location: new Point(posX[14], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 11, tabStop: input?.IsTabStop ?? true, cell: GetPriceNumberCell(builder, Precision, Precision, 0));
            input = GetInputControl(name = nameof(Billing.BillingAmount));
            yield return new CellSetting(height,   140, name, border: bdBm, location: new Point(posX[15], posY[1]), dataField: name, readOnly: false, tabIndex: input?.TabIndex ?? 12, tabStop: input?.IsTabStop ?? true, cell: GetPriceNumberCell(builder, Precision, Precision, 0), enabled: !CalculateTaxByInputId);
            name = "ClearButton";
            yield return new CellSetting(height,    40, name, border: bdBm, location: new Point(posX[16], posY[1]), cell: builder.GetButtonCell(), tabStop: false, value: "クリア");
            #endregion
            #region データ3
            yield return new CellSetting(height, 0, nameof(Billing.Memo) , dataField: nameof(Billing.Memo));
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

        #endregion

        #region Function Keys
        [OperationLog("登録")]
        private void Save()
        {
            ClearStatusMessage();
            try
            {
                if (!ValidateHeaderValues() || !ValidateDetailValues())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                // todo validate update items
                var billings = GetBillingList();

                var saveResult = SaveBillingInput(billings);

                if (ReturnScreen != null && saveResult)
                    ParentForm.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool SaveBillingInput(List<Billing> saveItems, bool isNewBilling = true)
        {
            var task = SaveAsync(saveItems);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var billings = task.Result;

            var success = billings.Any();
            if (!success)
            {
                ShowWarningDialog(MsgErrSaveError);
                return false;
            }
            ClearAll();
            if (isNewBilling)
                DispStatusMessage(MsgInfSaveSuccess);
            return success;
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            ClearAll();
            Modified = false;
        }

        [OperationLog("削除")]
        private void Delete()
        {
            ClearStatusMessage();

            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                var inputId = CurrentBilling?.BillingInputId;
                List<Billing> items = null;
                List<BillingDivisionContract> contracts = null;
                var loadTask = Task.Run(async () => {
                    if (inputId.HasValue)
                        items = await GetBillingByInputIdAsync(inputId.Value);
                    else
                        items = new List<Billing> { await GetBillingById(CurrentBilling.Id) };

                    if (UseLongTermAdvanceReceived)
                        contracts = await GetContractAsync(items.Select(x => x.Id));
                    else
                        contracts = new List<BillingDivisionContract>();
                });
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);


                foreach (var billing in items)
                {
                    if (!(ValidateBillingStatus(billing, false)))
                    {
                        // location set find
                        return;
                    }

                    if (UseLongTermAdvanceReceived
                        && contracts.Any(x => x.BillingId == billing.Id && x.Comfirm == 1))
                    {
                        ShowWarningDialog(MsgWngBillDivContractedDataCannotUpdateOrDelete, "削除");
                        return;
                    }
                }

                var success = true;
                var ids = items.Select(x => x.Id).ToArray();
                Task<CountResult> deleteTask = null;
                if (inputId.HasValue)
                    deleteTask = DeleteByInputIdAsync(inputId.Value);
                else
                    deleteTask = DeleteByIdsAsync(ids);
                ProgressDialog.Start(ParentForm, deleteTask, false, SessionKey);
                var deleteResult = deleteTask.Result;
                if (deleteResult.ProcessResult.Result)
                    success = deleteResult.Count > 0;
                else
                    success = false;

                if (!success)
                {
                    ShowWarningDialog(MsgErrDeleteError);
                    return;
                }

                ClearAll();

                if (ReturnScreen != null)
                {
                    ParentForm.DialogResult = DialogResult.OK;
                    return;
                }

                DispStatusMessage(MsgInfDeleteSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private List<Billing> GetBillingList()
        {
            if (!UseForeignCurrency)
                CurrencyId = GetCurrencyByCode(DefaultCurrencyCode).Id;
            return grid.Rows.Where(x => !IsRowEmpty(x)).Select(x => GetBillingFromRowData(x)).ToList();
        }

        private Billing GetBillingFromRowData(Row row)
        {

            var cellSalesAt                     = row.Cells[CellName(nameof(Billing.SalesAt))];
            var cellBillingCategoryId           = row.Cells[CellName(nameof(Billing.BillingCategoryId))];
            var cellUseLongTermAdvancedReceived = row.Cells[CellName(nameof(Billing.UseLongTermAdvancedReceived))];
            var cellTaxClassId                  = row.Cells[CellName(nameof(Billing.TaxClassId))];
            var cellDebitAccountTitleId         = row.Cells[CellName(nameof(Billing.DebitAccountTitleId))];
            var cellMemo                        = row.Cells[CellName(nameof(Billing.Memo))];
            var cellBillingDivisionContractFlg  = row.Cells[CellName(nameof(Billing.BillingDivisionContract))];
            var cellUseDiscount                 = row.Cells[CellName(nameof(Billing.UseDiscount))];
            var cellBillingDiscountId           = row.Cells[CellName(nameof(Billing.BillingDiscountId))];

            var cellBillingId       = row.Cells[CellName(nameof(Billing.BillingId))];
            var cellContractNumber  = row.Cells[CellName(nameof(Billing.ContractNumber))];
            var cellQuantity        = row.Cells[CellName(nameof(Billing.Quantity))];
            var cellUnitSymbol      = row.Cells[CellName(nameof(Billing.UnitSymbol))];
            var cellUnitPrice       = row.Cells[CellName(nameof(Billing.UnitPrice))];
            var cellPrice           = row.Cells[CellName(nameof(Billing.Price))];
            var cellTaxAmount       = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellBillingAmount   = row.Cells[CellName(nameof(Billing.BillingAmount))];
            var cellNote1 = row.Cells[CellName(nameof(Billing.Note1))];
            var cellNote2 = row.Cells[CellName(nameof(Billing.Note2))];
            var cellNote3 = row.Cells[CellName(nameof(Billing.Note3))];
            var cellNote4 = row.Cells[CellName(nameof(Billing.Note4))];
            var cellNote5 = row.Cells[CellName(nameof(Billing.Note5))];
            var cellNote6 = row.Cells[CellName(nameof(Billing.Note6))];
            var cellNote7 = row.Cells[CellName(nameof(Billing.Note7))];
            var cellNote8 = row.Cells[CellName(nameof(Billing.Note8))];

            var billingCategoryId = IsCellEmpty(cellBillingCategoryId) ? 0 : Convert.ToInt32(cellBillingCategoryId.Value);
            var taxClassId = IsCellEmpty(cellTaxClassId) ? 0 : Convert.ToInt32(cellTaxClassId.Value);

            int? debitAccountTitleId = null;
            if (!IsCellEmpty(cellDebitAccountTitleId))
                debitAccountTitleId = Convert.ToInt32(cellDebitAccountTitleId.Value);

            var memo = IsCellEmpty(cellMemo) ? string.Empty : cellMemo.Value.ToString();
            var billingDivisionContractFlg = IsCellEmpty(cellBillingDivisionContractFlg) ? 0 : Convert.ToInt32(cellBillingDivisionContractFlg.Value);
            var useDiscount = IsCellEmpty(cellUseDiscount) ? false : Convert.ToBoolean(cellUseDiscount.Value);
            var billingDiscountId = IsCellEmpty(cellBillingDiscountId) ? null : cellBillingDiscountId.Value;

            var billingId       = IsCellEmpty(cellBillingId)        ? 0L                : Convert.ToInt64(cellBillingId.Value);
            var contractNumber  = IsCellEmpty(cellContractNumber)   ? null              : cellContractNumber.Value.ToString();
            var quantity        = IsCellEmpty(cellQuantity)         ? (decimal?)null    : Convert.ToDecimal(cellQuantity.Value);
            var unitSymbol      = IsCellEmpty(cellUnitSymbol)       ? (string)null      : cellUnitSymbol.Value.ToString();
            var unitPrice       = IsCellEmpty(cellUnitPrice)        ? (decimal?)null    : Convert.ToDecimal(cellUnitPrice.Value);
            var price           = IsCellEmpty(cellPrice)            ? 0M                : Convert.ToDecimal(cellPrice.Value);
            var taxAmount       = IsCellEmpty(cellTaxAmount)        ? 0M                : Convert.ToDecimal(cellTaxAmount.Value);
            var billingAmount   = IsCellEmpty(cellBillingAmount)    ? 0M                : Convert.ToDecimal(cellBillingAmount.Value);
            var note1 = Convert.ToString(cellNote1.Value).Trim();
            var note2 = Convert.ToString(cellNote2.Value).Trim();
            var note3 = Convert.ToString(cellNote3.Value).Trim();
            var note4 = Convert.ToString(cellNote4.Value).Trim();
            var note5 = Convert.ToString(cellNote5.Value).Trim();
            var note6 = Convert.ToString(cellNote6.Value).Trim();
            var note7 = Convert.ToString(cellNote7.Value).Trim();
            var note8 = Convert.ToString(cellNote8.Value).Trim();

            var salesAt = Convert.ToDateTime(cellSalesAt.Value);

            var billing = new Billing();
            billing.CompanyId = CompanyId;
            billing.InvoiceCode = txtInvoiceCode.Text;
            billing.CurrencyId = CurrencyId;

            if (!string.IsNullOrWhiteSpace(txtBillingInputId.Text))
                billing.BillingInputId = long.Parse(txtBillingInputId.Text);

            billing.CustomerId = CustomerId;
            billing.CustomerCode = txtCustomerCode.Text;
            billing.BilledAt = Convert.ToDateTime(datBilledAt.Value);
            billing.ClosingAt = Convert.ToDateTime(datClosingAt.Value);
            billing.DueAt = Convert.ToDateTime(datDueAt.Value);

            var category = cmbCategory.SelectedItem.SubItems[0].Value.ToString();
            billing.CollectCategoryId = CollectCategories.Where(y => y.Code == (category.Split('：')[0])).Select(y => y.Id).FirstOrDefault();

            billing.StaffId = StaffId;
            billing.DepartmentId = DepartmentId;
            billing.DestinationId = DestinationId;
            billing.CreateBy = Login.UserId;
            billing.UpdateBy = Login.UserId;
            billing.CurrencyPrecision = Precision;
            billing.UseDiscount = UseDiscount && useDiscount;

            if (billingDiscountId != null)
                billing.BillingDiscountId = Convert.ToInt64(billingDiscountId);

            billing.ContractNumber = contractNumber;
            billing.BillingDivisionContract = billingDivisionContractFlg;
            billing.Id = billingId;
            billing.SalesAt = salesAt;
            billing.BillingCategoryId = billingCategoryId;
            billing.UseLongTermAdvancedReceived = Convert.ToInt32(cellUseLongTermAdvancedReceived.Value.ToString());
            billing.TaxClassId = taxClassId;
            billing.DebitAccountTitleId = debitAccountTitleId;
            billing.Quantity = quantity;
            billing.UnitSymbol = unitSymbol;
            billing.UnitPrice = unitPrice;
            billing.Price = price;
            billing.TaxAmount = taxAmount;
            billing.BillingAmount = billingAmount;
            billing.Note1 = note1;
            billing.Note2 = note2;
            billing.Note3 = note3;
            billing.Note4 = note4;
            billing.Note5 = note5;
            billing.Note6 = note6;
            billing.Note7 = note7;
            billing.Note8 = note8;
            billing.InputType = (int)BillingInputType.BillingInput;
            billing.Memo = memo;

            return billing;
        }

        [OperationLog("入力設定")]
        private void ShowInputSettingMaster()
        {
            using (var form = ApplicationContext.Create(nameof(PH9903)))
            {
                var screen = form.GetAll<PH9903>().First();
                screen.FormName = nameof(PC0201);
                screen.InitializeParentForm("入力設定");

                var result = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (result == DialogResult.OK)
                {
                    // タブの反映
                    InputControls = screen.InputControlList;
                    ClearStatusMessage();
                    InitializeGridTemplate();
                }
            }
        }

        [OperationLog("参照新規")]
        private void ShowInputPattern()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            ClearAll();

            using (var form = ApplicationContext.Create(nameof(PC0301)))
            {
                var screen = form.GetAll<PC0301>().FirstOrDefault();
                screen.ReturnScreen = this;
                form.StartPosition = FormStartPosition.CenterParent;
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form);
                if (dialogResult != DialogResult.OK
                    || CurrentBilling == null) return;
            }
            ProgressDialog.Start(ParentForm, SetPastBillingAsTemplateAsync(), false, SessionKey);

        }

        private async Task SetPastBillingAsTemplateAsync()
        {
            grid.EndEdit();
            await LoadCurrentGridBillingAsync();
            CommonFormLoad();
            InitializeControls();
            SetGridRowCount();
            await SetBillingFormAsync();
            CalculateTotalAmount();
        }

        [OperationLog("行コピー")]
        private void CopyGridRowData()
        {
            var index = grid.Rows.LastOrDefault(x => !IsRowEmpty(x))?.Index;
            if (!index.HasValue)
            {
                ShowWarningDialog(MsgWngInputGridRequired);
                return;
            }
            var rowIndex = index.Value;
            if (rowIndex >= (grid.Rows.Count() - 1))
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

                    case nameof(Billing.ContractNumber):
                        bool isAble = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))].Enabled;
                        grid.Rows[newRowIndex].Cells[CellName(nameof(Billing.ContractNumber))].Enabled = isAble;
                        grid.Rows[newRowIndex].Cells[CellName(nameof(Billing.ContractNumber))].Selectable = isAble;
                        break;
                    default:
                        break;
                }

            }
            CalculateTotalAmount();
        }

        [OperationLog("検索")]
        private void ShowCodeSearchDialog()
        {
            var rowIndex = grid.CurrentCell.RowIndex;
            var dataField = grid.CurrentCell.DataField;
            switch (dataField)
            {
                case nameof(Billing.BillingCategoryCode):
                    ShowCategorySearchDialog(rowIndex);
                    break;

                case nameof(Billing.TaxClassId):
                    ShowTaxClassSearchDialog(rowIndex);
                    break;

                case nameof(Billing.AccountTitleCode):
                    ShowAccTitleSearchDialog(rowIndex);
                    break;

                case nameof(Billing.ContractNumber):
                    var cellIndex = grid.CurrentCell.CellIndex;
                    var cell = (GcTextBoxCell)grid.Rows[rowIndex].Cells[cellIndex];
                    if (cell.Enabled)
                    {
                        if (!ValidateCustomerCode()) return;
                        ShowContractNumberSearchDialog(rowIndex);
                    }
                    break;

                default:
                    break;
            }
            this.ActiveControl = grid;
        }

        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            Common.Settings.Settings.SaveControlValue<PC0201>(Login, rdoAuto.Name, rdoAuto.Checked);

            BaseForm.Close();
        }
        #endregion

        #region 入力項目変更イベント

        private void InitializeHandlers()
        {
            foreach (var control in gbxHeader.Controls)
            {
                if (control is Common.Controls.VOneDateControl)
                    ((Common.Controls.VOneDateControl)control).ValueChanged += OnContentChanged;

                if (control is Common.Controls.VOneTextControl)
                    ((Common.Controls.VOneTextControl)control).TextChanged += OnContentChanged;

                if (control is Common.Controls.VOneComboControl)
                    ((Common.Controls.VOneComboControl)control).SelectedIndexChanged += OnContentChanged;
            }
            rdoAuto.CheckedChanged += (sender, e) => Common.Settings.Settings.SaveControlValue<PC0201>(Login, rdoAuto.Name, rdoAuto.Checked);
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        #endregion

        #region validation for save/delete/...

        private bool ValidateHeaderValues()
        {
            if (UseForeignCurrency && !txtCurrencyCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblCurrency.Text))) return false;
            if (!datBilledAt.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblBilledAt.Text))) return false;
            if (!ValidateCustomerCode()) return false;
            if (!datClosingAt.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblClosingAt.Text))) return false;
            if (!datDueAt.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblDueAt.Text))) return false;
            if (!txtStaffCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblStaff.Text))) return false;
            if (!txtDepartmentCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblDepartment.Text))) return false;
            if (!cmbCategory.ValidateInputted(() => ShowWarningDialog(MsgWngSelectionRequired, lblCategory.Text))) return false;
            if (cmbCategory.SelectedItem.SubItems[0].Value.ToString() == "00：約定")
            {
                var categoryId = CollectCategories.Where(x => x.Code == "00").Select(x => x.Id).FirstOrDefault();
                var customer = GetCustomerByCode();
                if (customer != null && customer.CollectCategoryId != categoryId)
                {
                    ShowWarningDialog(MsgWngNotContractCollectCategoryCustomer, txtCustomerCode.Text);
                    txtCustomerCode.Focus();
                    return false;
                }
            }
            if (!datBilledAt.ValidateRange(datClosingAt,
                () => ShowWarningDialog(MsgWngInputValidDateForParameters, lblClosingAt.Text, lblBilledAt.Text))) return false;
            if (!datClosingAt.ValidateRange(datDueAt,
                () => ShowWarningDialog(MsgWngInputValidDateForParameters, lblDueAt.Text, lblClosingAt.Text))) return false;

            if (CalculateTaxByInputId && lblTotalTaxAmount.Text != lblTotalTaxByInputId.Text)
            {
                ShowWarningDialog(MsgWngTaxAmountDifference);
                return false;
            }
            return true;
        }

        private bool ValidateCustomerCode() =>
            (txtCustomerCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblCustomer.Text)));

        private bool IsRowEmpty(int rowIndex)
            => IsRowEmpty(grid.Rows[rowIndex]);

        private bool IsRowEmpty(Row row)
            => row.Cells.Where(x => !(x.Name == CellName(nameof(Billing.SalesAt)) || IsButtonCell(x)))
                .All(x => IsCellEmpty(x));

        private bool IsButtonCell(Cell cell)
            => cell.Name == CellName("MemoButton") || cell.Name == CellName("ClearButton");

        private bool IsCellEmpty(Cell cell)
            => (cell.Value == null
             || string.IsNullOrEmpty(cell.Value.ToString())
             || cell.Name == CellName(nameof(Billing.UseDiscount)) && 0.Equals(cell.Value));


        private bool ValidateDetailValues()
        {
            this.ActiveControl = grid;

            var rows = grid.Rows.Where(x => !IsRowEmpty(x)).ToArray();
            if (!rows.Any())
            {
                ShowWarningDialog(MsgWngInputGridRequired);
                return false;
            }
            BillingDivisionSetting setting = null;

            foreach (var row in rows)
            {
                var cellSalesAt                     = row.Cells[CellName(nameof(Billing.SalesAt))];
                var cellBillingCategoryCode         = row.Cells[CellName(nameof(Billing.BillingCategoryCode))];
                var cellTaxClassId                  = row.Cells[CellName(nameof(Billing.TaxClassId))];
                var cellContractNumber              = row.Cells[CellName(nameof(Billing.ContractNumber))];
                var cellBillingAmount               = row.Cells[CellName(nameof(Billing.BillingAmount))];
                var cellBillingDivisionContractFlg  = row.Cells[CellName(nameof(Billing.BillingDivisionContract))];
                var cellQuantity                    = row.Cells[CellName(nameof(Billing.Quantity))];
                var cellUnitPrice                   = row.Cells[CellName(nameof(Billing.UnitPrice))];
                var cellPrice                       = row.Cells[CellName(nameof(Billing.Price))];
                var salesAt             = IsCellEmpty(cellSalesAt) ? string.Empty : cellSalesAt.Value.ToString();
                var billingCategoryCode = IsCellEmpty(cellBillingCategoryCode) ? string.Empty : cellBillingCategoryCode.Value.ToString();
                var taxClassId          = IsCellEmpty(cellTaxClassId) ? string.Empty : cellTaxClassId.Value.ToString();
                var contractNumber      = IsCellEmpty(cellContractNumber) ? string.Empty : cellContractNumber.Value.ToString();
                var BillingAmountValue  = IsCellEmpty(cellBillingAmount) ? string.Empty : cellBillingAmount.Value.ToString();
                var billingAmount       = string.IsNullOrEmpty(BillingAmountValue) ? 0M : Convert.ToInt64(cellBillingAmount.Value);
                var quantity            = IsCellEmpty(cellQuantity) ? 0M : Convert.ToInt64(cellQuantity.Value);
                var unitPrice           = IsCellEmpty(cellUnitPrice) ? 0M : Convert.ToInt64(cellUnitPrice.Value);
                var price               = IsCellEmpty(cellPrice) ? 0M : Convert.ToInt64(cellPrice.Value);

                if (string.IsNullOrWhiteSpace(salesAt))
                {
                    ShowWarningDialog(MsgWngInputRequired, lblSalesAt.Text);
                    grid.ClearSelection();
                    grid.CurrentCell = cellSalesAt;
                    return false;
                }

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

                if (cellContractNumber.Enabled)
                {
                    if (string.IsNullOrWhiteSpace(contractNumber))
                    {
                        ShowWarningDialog(MsgWngInputRequired, "契約番号");
                        grid.ClearSelection();
                        grid.CurrentCell = cellContractNumber;
                        return false;
                    }

                    if (!ValidateContractNumber(row.Index))
                        return false;

                }

                if (Convert.ToInt32(cellBillingDivisionContractFlg.Value) == 3)
                {
                    if (setting == null)
                    {
                        var task = GetDivisionSettingAsync();
                        ProgressDialog.Start(ParentForm, task, false, SessionKey);
                        setting = task.Result;
                        if (setting == null)
                        {
                            ShowWarningDialog(MsgWngNoDefaultSettingAtBillingDivisionContract);
                            grid.ClearSelection();
                            return false;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(BillingAmountValue))
                {
                    ShowWarningDialog(MsgWngInputRequired, "請求額");
                    grid.ClearSelection();
                    grid.CurrentCell = cellBillingAmount;
                    return false;
                }

                if (billingAmount == 0)
                {
                    ShowWarningDialog(MsgWngInputExceptZeroAmt, "請求額");
                    grid.ClearSelection();
                    grid.CurrentCell = cellBillingAmount;
                    return false;
                }

                if (quantity != 0 && unitPrice != 0 && price != 0
                    && (price != quantity * unitPrice)
                    && (!ShowConfirmDialog(MsgQstContinueUnmatchCalculation)))  return false;
            }

            return true;
        }

        private BillingDivisionContract GetContractInfo(string contractNumber)
        {
            var task = GetContractAsync(CustomerId, contractNumber);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result.FirstOrDefault();
        }

        private bool ValidateContractNumber(int rowIndex)
        {
            ClearStatusMessage();

            var cellContractNumber = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))];
            var cellBillingId = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.BillingId))];
            var contractNumber = IsCellEmpty(cellContractNumber) ? string.Empty : cellContractNumber.Value.ToString();
            var billingId = IsCellEmpty(cellBillingId) ? 0L : long.Parse(cellBillingId.Value.ToString());

            if (string.IsNullOrEmpty(contractNumber) || !ValidateContractNumberDuplication(rowIndex, contractNumber, cellContractNumber))
                return false;

            var list = GetContractInfo(contractNumber);
            if (string.IsNullOrEmpty(contractNumber) || !ValidateContractNumberExistence(cellContractNumber, list))
                return false;

            if (string.IsNullOrEmpty(contractNumber) || !ValidateContractNumberRelation(cellContractNumber, billingId, list))
                return false;

            return true;
        }

        /// <summary>契約番号が明細内で重複していないか検証</summary>
        private bool ValidateContractNumberDuplication(int rowIndex, string contractNumber, Cell cellContractNumber)
        {
            for (var i = 0; i < grid.RowCount; i++)
            {
                if (IsRowEmpty(i))
                    continue;

                if (i == rowIndex)
                    continue;

                var nextCellContractNumber = grid.Rows[i].Cells[CellName(nameof(Billing.ContractNumber))];
                var nextcontractNumber = IsCellEmpty(nextCellContractNumber) ? string.Empty : nextCellContractNumber.Value.ToString();

                if (contractNumber == nextcontractNumber)
                {
                    ShowWarningDialog(MsgWngAlreadyRegistData, "契約番号");
                    grid.ClearSelection();
                    grid.CurrentCell = cellContractNumber;
                    return false;
                }
            }

            return true;
        }

        /// <summary>契約番号がDB登録済かどうかの確認</summary>
        private bool ValidateContractNumberExistence(Cell cellContractNumber, BillingDivisionContract list)
        {
            if (list == null)
            {
                ShowWarningDialog(MsgWngNotRegistedContractNo);
                grid.ClearSelection();
                grid.CurrentCell = cellContractNumber;
                return false;
            }

            return true;
        }

        /// <summary>契約番号が他の請求IDと紐づいていないか確認</summary>
        private bool ValidateContractNumberRelation(Cell cellContractNumber, long billingId, BillingDivisionContract list)
        {
            if (list.BillingId.HasValue && !list.BillingId.Equals(billingId))
            {
                ShowWarningDialog(MsgWngAlreadyRegistData, "契約番号");
                grid.ClearSelection();
                grid.CurrentCell = cellContractNumber;
                return false;
            }

            return true;
        }

        #endregion

        #region Sub Function

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


        private void ClearAll()
        {
            ClearStatusMessage();
            InitializeGridTemplate();
            CalculateTotalAmount();

            txtBillingInputId.Clear();
            txtInvoiceCode.Clear();

            cmbCategory.Clear();

            datBilledAt.Clear();
            datSalesAt.Clear();
            datClosingAt.Clear();
            datDueAt.Clear();

            CustomerId = 0;
            txtCustomerCode.Clear();
            lblCustomerName.Clear();
            txtCustomerCode.Enabled = true;
            btnCustomer.Enabled = true;

            DepartmentId = 0;
            txtDepartmentCode.Clear();
            lblDepartmentName.Clear();
            txtDepartmentCode.Enabled = true;
            btnDepartment.Enabled = true;

            CurrencyId = 0;
            txtCurrencyCode.Clear();
            txtCurrencyCode.Enabled = true;
            btnCurrency.Enabled = true;

            StaffId = 0;
            txtStaffCode.Clear();
            lblStaffName.Clear();
            txtStaffCode.Enabled = true;
            btnStaffCode.Enabled = true;

            DestinationId = null;
            txtDestinationCode.Clear();
            lblDestinationName.Clear();
            txtDestinationCode.Enabled = false;
            btnDestination.Enabled = false;

            cmbCategory.Clear();
            cmbCategory.SelectedItem = null;

            lblInputTypeName.Clear();

            grid.DataSource = null;

            lblTotalPrice.Text = "0";
            lblTotalTaxAmount.Text = "0";
            lblTotalBillingAmount.Text = "0";

            SelectedCustomer = null;

            BaseContext.SetFunction09Enabled(false);
            this.ActiveControl = txtCustomerCode;
            Modified = false;
        }

        private Customer GetCustomerByCode()
        {
            var task = GetCustomerByCodeAsync(txtCustomerCode.Text);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        /// <summary>DB登録されている billing の値から、更新削除が行えるか確認する処理</summary>
        private bool ValidateBillingStatus(Billing billing, bool isUpdate)
        {
            var state = isUpdate ? "更新" : "削除";
            if (billing == null)
            {
                ShowWarningDialog(MsgWngInvalidBillingNoForBilling, billing.Id.ToString());
                return false;
            }

            if (billing.AssignmentFlag > 0)
            {
                ShowWarningDialog(MsgWngMatchedDataCannotUpdateOrDelete, state);
                return false;
            }

            if (billing.OutputAt.HasValue)
            {
                ShowWarningDialog(MsgWngJournalizedCannotUpdateAndDelete, state);
                return false;
            }

            if (billing.InputType == (int)BillingInputType.CashOnDueDate)
            {
                ShowWarningDialog(MsgWngPaymentScheduledDataCannotUpdateOrDelete, state);
                return false;
            }

            if (billing.DeleteAt.HasValue)
            {
                ShowWarningDialog(MsgWngDeletedDataCannotUpdateOrDelete, state);
                return false;
            }

            if (billing.ResultCode == 0)
            {
                ShowWarningDialog(MsgWngAccountTransferredOrWithdrawnDataCannotUpdateOrDelete, state);
                return false;
            }

            return true;
        }



        private void CommonFormLoad()
        {
            ClearAll();
            rdoManual.Checked = true;
            lblInputType.Hide();
            lblInputTypeName.Hide();

            txtBillingInputId.Enabled = false;

            // 通貨コード表示制御
            if (Mode == InputMode.Default)
                DisplayCurrencyInput();
            else
                DisplayCurrencyInput(false);
        }

        private void DisplayCurrencyInput(bool enabled = true)
        {
            if (UseForeignCurrency)
            {
                SetCurrencyDisplayString(Precision);
                lblCurrency.Visible = true;
                txtCurrencyCode.Visible = true;
                btnCurrency.Visible = true;
                txtCurrencyCode.Enabled = enabled;
                btnCurrency.Enabled = enabled;
            }
            else
            {
                SetCurrencyDisplayString(0);
                lblCurrency.Visible = false;
                txtCurrencyCode.Visible = false;
                btnCurrency.Visible = false;
            }
        }

        private void ClearCellContent(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            foreach (var cell in row.Cells.Where(x => !IsButtonCell(x)))
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

        private void SetCategory(int rowIndex, Category category)
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
            var cellBillingDivisionContract     = row.Cells[CellName(nameof(Billing.BillingDivisionContract))];
            var cellContractNumber              = row.Cells[CellName(nameof(Billing.ContractNumber))];

            var originalCategoryId = (int?)cellBillingCategoryId.Value;

            if (category == null)
            {
                cellBillingCategoryCode.Value = string.Empty;
                cellBillingCategoryId           .Value = null;
                cellBillingCategoryName         .Value = null;
                cellUseLongTermAdvancedReceived .Value = null;
                cellUseDiscount                 .Value = null;
                cellTaxClassId                  .Value = null;
                cellTaxClassIdName              .Value = null;
                cellBillingDivisionContract     .Value = null;
                cellContractNumber.Enabled = false;
                return;
            }

            cellBillingCategoryCode.Visible = true;
            cellBillingCategoryName.Visible = true;

            if (category.Id == originalCategoryId) return;
            cellBillingCategoryId.Value = category.Id;
            cellBillingCategoryCode.Value = category.Code;
            cellBillingCategoryName.Value = category.CodeAndName;
            cellUseLongTermAdvancedReceived.Value = category.UseLongTermAdvanceReceived;
            cellUseDiscount.Value = (UseDiscount && category.UseDiscount == 1) ? (object)category.UseDiscount : null;

            var originalTaxClassId = (int?)cellTaxClassId.Value;
            if (category.TaxClassId != originalCategoryId)
            {
                SetTaxClass(rowIndex, TaxClasses.First(x => x.Id == category.TaxClassId));
            }

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
            SetTaxClass(rowIndex, target);
        }

        public void SetTaxClass(int rowIndex, TaxClass taxClass)
        {
            var row = grid.Rows[rowIndex];
            var cellTaxClassId      = row.Cells[CellName(nameof(Billing.TaxClassId))];
            var cellTaxClassIdName  = row.Cells[CellName("TaxClassIdName")];

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

        private void SetCustomerRelatedInformation()
        {
            if (cmbCategory.SelectedIndex == -1)
            {
                var target = cmbCategory.Items.Cast<GrapeCity.Win.Editors.ListItem>()
                        .FirstOrDefault(i => (int)i.SubItems[1].Value == SelectedCustomer.CollectCategoryId);

                if (target != null)
                    cmbCategory.SelectedItem = target;
            }

            var staff = new Staff();
            if (string.IsNullOrWhiteSpace(txtStaffCode.Text))
            {
                staff = Staffs.FirstOrDefault(x => x.Id == SelectedCustomer.StaffId);
                if (staff != null)
                {
                    txtStaffCode.Text = staff.Code;
                    lblStaffName.Text = staff.Name;
                    StaffId = staff.Id;
                    ClearStatusMessage();
                }
            }

            if (string.IsNullOrWhiteSpace(txtDepartmentCode.Text) && staff != null)
            {
                var department = Departments.FirstOrDefault(x => x.Id == staff.DepartmentId);
                if (department != null)
                {
                    txtDepartmentCode.Text = department.Code;
                    lblDepartmentName.Text = department.Name;
                    DepartmentId = department.Id;
                    ClearStatusMessage();
                }
            }

            if (datBilledAt.Value.HasValue && SelectedCustomer != null && SelectedCustomer.Code != "")
            {
                datClosingAt.Value  = SelectedCustomer.GetClosingAt(datBilledAt.Value.Value);
                datDueAt.Value      = SelectedCustomer.GetDueAt(datClosingAt.Value.Value, Holidays);
            }

            txtDestinationCode.Enabled = true;
            btnDestination.Enabled = true;

        }

        #endregion

        #region 検索ボタンクリック処理
        private void btnStaffCode_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCode.Text = staff.Code;
                lblStaffName.Text = staff.Name;
                StaffId = staff.Id;
            }
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                SetCurrencyInformation(currency);
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var customer = this.ShowCustomerSearchDialog();
            if (customer == null)
            {
                return;
            }
            txtCustomerCode.Text = customer.Code;
            lblCustomerName.Text = customer.Name;
            CustomerId = customer.Id;
            SelectedCustomer = customer;
            SetCustomerRelatedInformation();
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentCode.Text = department.Code;
                lblDepartmentName.Text = department.Name;
                DepartmentId = department.Id;
            }
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var destination = this.ShowDestinationSearchDialog(CustomerId);
            if (destination == null)
            {
                return;
            }
            DestinationId           = destination.Id;
            txtDestinationCode.Text = destination.Code;
            lblDestinationName.Text = destination.ToString();
        }
        #endregion

        #region ShowDialog for grid detail data

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
                screen.txtNote2.Text = note2;
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

        private void ShowCategorySearchDialog(int rowIndex)
        {
            ClearStatusMessage();

            var billingCategory = this.ShowBillingCategorySearchDialog(useInput: true);

            var cellBillingCategoryCode = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.BillingCategoryCode))];
            var cellBillingCategoryId = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.BillingCategoryId))];
            var cellBillingCategoryName = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.BillingCategoryName))];
            var cellUseLongTermAdvancedReceived = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.UseLongTermAdvancedReceived))];
            var cellUseDiscount = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.UseDiscount))];
            var cellTaxClassId = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.TaxClassId))];
            var cellTaxClassIdName = grid.Rows[rowIndex].Cells[CellName("TaxClassIdName")];

            if (billingCategory != null)
            {
                cellBillingCategoryCode.Visible = true;
                cellBillingCategoryName.Visible = true;
                cellBillingCategoryId.Value = billingCategory.Id;
                cellBillingCategoryCode.Value = billingCategory.Code;
                cellUseLongTermAdvancedReceived.Value = billingCategory.UseLongTermAdvanceReceived;
                cellBillingCategoryName.Value = billingCategory.Code + ":" + billingCategory.Name;

                cellUseDiscount.Value = billingCategory.UseDiscount;

                if (cellTaxClassId.Value == null)
                {
                    cellTaxClassId.Value = billingCategory.TaxClassId;
                    cellTaxClassIdName.Visible = false;
                    cellTaxClassIdName.Value = billingCategory.TaxClassId + ":" + billingCategory.TaxClassName;
                }
                grid.CurrentCell = cellTaxClassId;
                grid.BeginEdit(true);
            }
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

        private void ShowContractNumberSearchDialog(int rowIndex)
        {
            ClearStatusMessage();
            var contract = this.ShowBillingDivisionContractSearchDialog();
            if (contract != null)
            {
                var cellContractNumber = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))];
                cellContractNumber.Value = contract.ContractNumber;
                if (ValidateContractNumber(rowIndex))
                {
                    grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.Quantity))];
                }
                else
                {
                    cellContractNumber.Value = null;
                    grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))];
                }
                grid.BeginEdit(true);
            }
        }

        private void SetBillingCategoryByCode(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var cell = row.Cells[CellName(nameof(Billing.BillingCategoryCode))];
            var code = cell.Value?.ToString();
            if (!string.IsNullOrEmpty(code) && code.Length < 2) code = code.PadLeft(2, '0');
            var category = GetInputBillingCateogry(x => x.Code == code);
            SetCategory(rowIndex, category);
        }

        #endregion

        #region header control event handlers

        private void txtStaffCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();

            var code = txtStaffCode.Text;
            if (string.IsNullOrWhiteSpace(code))
            {
                lblStaffName.Clear();
                return;
            }

            try
            {
                var staff = Staffs.FirstOrDefault(x => x.Code == code);
                if (staff == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "担当者", code);
                    txtStaffCode.Clear();
                    lblStaffName.Clear();
                    txtStaffCode.Select();
                    return;
                }

                StaffId = staff.Id;
                lblStaffName.Text = staff.Name;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtCurrencyCode.Text;
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    CurrencyId = 0;
                    return;
                }
                var currency = GetCurrencyByCode(code);
                if (currency == null)
                {
                    CurrencyId = 0;
                    ShowWarningDialog(MsgWngMasterNotExist, "通貨", code);
                    txtCurrencyCode.Clear();
                    txtCurrencyCode.Select();
                    return;
                }

                SetCurrencyInformation(currency);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private Currency GetCurrencyByCode(string currencyCode)
        {
            return Currencies.FirstOrDefault(x => x.Code == currencyCode);
        }

        private void SetCurrencyInformation(Currency currency)
        {
            if (currency == null) return;

            var sameCurrency = CurrencyId == currency.Id;
            CurrencyId = currency.Id;
            Precision = currency.Precision;
            SetCurrencyDisplayString(Precision);

            if (!sameCurrency)
            {
                ClearStatusMessage();
                InitializeGridTemplate();
                lblTotalPrice.Text = 0.ToString(AmountFormat);
                lblTotalTaxAmount.Text = 0.ToString(AmountFormat);
                lblTotalBillingAmount.Text = 0.ToString(AmountFormat);
            }
        }

        private void txtCustomerCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();

            try
            {
                var code = txtCustomerCode.Text;
                if (string.IsNullOrEmpty(code))
                {
                    CustomerId = 0;
                    ClearWithCustomerCode();
                    SelectedCustomer = null;
                    return;
                }
                if (code == SelectedCustomer?.Code) return;
                var task = GetCustomerByCodeAsync(code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                SelectedCustomer = task.Result;
                if (SelectedCustomer == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "得意先", code);
                    CustomerId = 0;
                    txtCustomerCode.Clear();
                    ClearWithCustomerCode();
                    txtCustomerCode.Select();

                    return;
                }

                txtCustomerCode.Text = SelectedCustomer.Code;
                lblCustomerName.Text = SelectedCustomer.Name;
                CustomerId = SelectedCustomer.Id;

                SetCustomerRelatedInformation();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ClearWithCustomerCode()
        {
            lblCustomerName.Clear();
            cmbCategory.SelectedIndex = -1;
            datClosingAt.Clear();
            datDueAt.Clear();
            txtStaffCode.Clear();
            lblStaffName.Clear();
            txtDepartmentCode.Clear();
            lblDepartmentName.Clear();

            txtDestinationCode.Clear();
            lblDestinationName.Clear();
            txtDestinationCode.Enabled = false;
            btnDestination.Enabled = false;
        }

        private void txtDepartmentCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtDepartmentCode.Text;
            if (string.IsNullOrWhiteSpace(code))
            {
                lblDepartmentName.Clear();
                return;
            }

            try
            {
                var department = Departments.FirstOrDefault(x => x.Code == code);
                if (department == null)
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "請求部門", code);
                    txtDepartmentCode.Clear();
                    lblDepartmentName.Clear();
                    txtDepartmentCode.Select();
                    return;
                }
                DepartmentId = department.Id;
                lblDepartmentName.Text = department.Name;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void datBilledAt_Validated(object sender, EventArgs e)
        {
            if (!datBilledAt.Value.HasValue)
            {
                datClosingAt.Clear();
                datDueAt.Clear();
                return;
            }
            if (datSalesAt.Value.HasValue) return;
            datSalesAt.Value = datBilledAt.Value;
            if (SelectedCustomer == null) return;
            datClosingAt.Value  = SelectedCustomer.GetClosingAt(datBilledAt.Value.Value);
            datDueAt.Value      = SelectedCustomer.GetDueAt(datClosingAt.Value.Value, Holidays);
        }

        private void datSalesAt_ValueChanged(object sender, EventArgs e)
        {
            foreach (var row in grid.Rows)
            {
                if (row.Cells[CellName(nameof(Billing.SalesAt))].Enabled)
                    row.Cells[CellName(nameof(Billing.SalesAt))].Value = datSalesAt.Value;
            }
        }

        private void txtDestinationCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var code = txtDestinationCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                DestinationId = null;
                txtDestinationCode.Clear();
                lblDestinationName.Clear();
                return;
            }
            var task = GetDestinationsAsync(CustomerId, code);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var destination = task.Result.FirstOrDefault();
            if (destination == null)
            {
                DestinationId = null;
                txtDestinationCode.Clear();
                lblDestinationName.Clear();
                txtDestinationCode.Select();
                ShowWarningDialog(MsgWngMasterNotExist, "送付先", code);
                return;
            }

            DestinationId = destination.Id;
            txtDestinationCode.Text = destination.Code;
            lblDestinationName.Text = destination.ToString();

        }

        #endregion

        #region grid event handlers

        private void grid_CellContentButtonClick(object sender, CellEventArgs e)
        {
            if (grid.ReadOnly) return;
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
            if ((ActiveControl != grid) || grid.ReadOnly) return;

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
            if (grid.ReadOnly) return;
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
                    if (cell.Value != null && !ValidateContractNumber(rowIndex))
                    {
                        cell.Value = null;
                        grid.CurrentCell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.ContractNumber))];
                    }
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
            if (grid.ReadOnly) return;
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

        #region calculation amount / tax / grand total

        /// <summary>管理コードから、金額端数処理を取得する処理
        /// 事前に GeneralSetting に マスターのデータを格納しておく必要がある</summary>
        /// <param name="code">管理コード</param>
        /// <returns></returns>

        private RoundingType GetRoundingType(string code)
        {
            var value = GeneralSettings.Find(s => s.Code == code)?.Value;
            if (string.IsNullOrEmpty(value)) return RoundingType.Floor;
            return (RoundingType)int.Parse(value);
        }

        /// <summary>税抜 → 外税 計算</summary>
        private decimal CalculateExclusiveTax(decimal amount) => CalculateTax(rate => (amount * rate));
        /// <summary>税込 → 内税 計算</summary>
        private decimal CalculateInclusiveTax(decimal amount) => CalculateTax(rate => (amount * rate / (1M + rate)));

        private decimal CalculateTax(Func<decimal, decimal> calculateInnter)
        {
            var rate = GetTaxRate(datBilledAt.Value);
            if (rate == 0M) return 0M;
            return TaxCalculationRoundingType.Calc(calculateInnter(rate), Precision) ?? 0M;
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

            var applyDate1 = GeneralSettings.Find(s => s.Code == "新税率開始年月日").Value;
            var applyDate2 = GeneralSettings.Find(s => s.Code == "新税率開始年月日2").Value;
            var applyDate3 = GeneralSettings.Find(s => s.Code == "新税率開始年月日3").Value;

            var target = date.Value.ToString("yyyyMMdd");
            var key = string.Empty;
            if                                           (target.CompareTo(applyDate1) < 0) key = "旧消費税率";
            else if (applyDate1.CompareTo(target) <= 0 && target.CompareTo(applyDate2) < 0) key = "新消費税率";
            else if (applyDate2.CompareTo(target) <= 0 && target.CompareTo(applyDate3) < 0) key = "新消費税率2";
            else if (applyDate3.CompareTo(target) <= 0)                                     key = "新消費税率3";
            if (string.IsNullOrEmpty(key)) return 0M;
            var value = GeneralSettings.Find(s => s.Code == key).Value;
            var result = 0M;
            return decimal.TryParse(value, out result) ? (result / 10000M) : 0M;
        }

        /// <summary> 管理マスターから請求入力明細件数を取得。取得失敗はデフォールトに設定 </summary>
        private void GetMaxGridRowCount()
        {
            if (GeneralSettings == null)
            {
                MaxGridRowCount = DefaultMaxGridRowCount;
                return;
            }

            var rowCount = GeneralSettings.Find(s => s.Code == "請求入力明細件数").Value;
            MaxGridRowCount = string.IsNullOrEmpty(rowCount) ? DefaultMaxGridRowCount : Convert.ToInt32(rowCount);
        }

        private void SetGridRowCount()
        {
            if (CurrentGridBilling == null)
            {
                grid.RowCount = MaxGridRowCount;
                return;
            }

            var currentBillingCount = CurrentGridBilling.Count();
            grid.RowCount = currentBillingCount > MaxGridRowCount ? currentBillingCount : MaxGridRowCount;
        }

        /// <summary>数量/単価 から 金額（抜)/消費税/請求額 を設定
        /// 金額計算が"自動" の場合のみ実施
        /// 税区分 1:外税 の場合のみ税計算を行う
        /// 上記以外の区分の場合は、金額（抜）にも値を設定しない</summary>
        private void SetBillingAmounFromQuantityWithUnitPrice(int rowIndex)
        {
            if (rdoManual.Checked) return;
            var row = grid.Rows[rowIndex];

            var cellQuantity        = row.Cells[CellName(nameof(Billing.Quantity))];
            var cellUnitPrice       = row.Cells[CellName(nameof(Billing.UnitPrice))];
            var cellPrice           = row.Cells[CellName(nameof(Billing.Price))];
            var cellTaxAmount       = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellBillingAmount   = row.Cells[CellName(nameof(Billing.BillingAmount))];

            var quantity    = Convert.ToDecimal(cellQuantity.Value);
            var unitPrice   = Convert.ToDecimal(cellUnitPrice.Value);
            var amount      = PriceCalculationRoundingType.Calc(quantity * unitPrice, Precision) ?? 0M;
            var taxId = GetTaxClassId(rowIndex);

            if (taxId != (int)TaxClassId.TaxExclusive)
            {
                cellPrice           .Value = null;
                cellTaxAmount       .Value = null;
                cellBillingAmount   .Value = amount;
                return;
            }

            if (quantity == 0M || unitPrice == 0M || amount == 0M)
            {
                cellPrice.Value         = null;
                cellTaxAmount.Value     = null;
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
            var cellPrice           = row.Cells[CellName(nameof(Billing.Price))];
            var cellTaxAmount       = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellBillingAmount   = row.Cells[CellName(nameof(Billing.BillingAmount))];

            var price = (decimal?)cellPrice.EditedFormattedValue;
            if (!price.HasValue)
            {
                cellTaxAmount.Value = null;
                cellBillingAmount.Value = null;
                return;
            }

            var taxId = GetTaxClassId(rowIndex);
            var isTax = (taxId == (int)TaxClassId.TaxExclusive) || (taxId == (int)TaxClassId.TaxInclusive);
            var tax = !isTax ? 0 : CalculateExclusiveTax(price.Value);
            cellTaxAmount.Value = tax;
            var amount = price + tax;
            if (amount <= MaxAmount) cellBillingAmount.Value = amount;
        }

        /// <summary>消費税 から、金額（抜） または 請求額を設定 照合ロジック オプションで動作変化</summary>
        private void SetPriceOrBillingAmountFromTax(int rowIndex)
        {
            var row = grid.Rows[rowIndex];
            var cellPrice   = row.Cells[CellName(nameof(Billing.Price))];
            var cellTax     = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellAmount  = row.Cells[CellName(nameof(Billing.BillingAmount))];
            var price   = (decimal?)cellPrice.EditedFormattedValue;
            var tax     = (decimal?)(cellTax.EditedFormattedValue);
            var amount  = (decimal?)cellAmount.EditedFormattedValue;
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
            var cellPrice           = row.Cells[CellName(nameof(Billing.Price))];
            var cellTaxAmount       = row.Cells[CellName(nameof(Billing.TaxAmount))];
            var cellBillingAmount   = row.Cells[CellName(nameof(Billing.BillingAmount))];

            var amount = (decimal?)cellBillingAmount.EditedFormattedValue;

            if (!amount.HasValue)
            {
                cellPrice.Value = null;
                cellTaxAmount.Value = null;
                return;
            }

            var taxId = GetTaxClassId(rowIndex);
            var isTax = (taxId == (int)TaxClassId.TaxExclusive) || (taxId == (int)TaxClassId.TaxInclusive);
            var tax = !isTax ? 0 : CalculateInclusiveTax(amount.Value);
            var price = (amount ?? 0M) - tax;
            cellPrice.Value = price;
            cellTaxAmount.Value = tax;
        }

        private void CalculateTotalAmount()
        {
            var totalPrice  = 0M;
            var totalTax    = 0M;
            var totalAmount = 0M;

            var exclusiveAmount = 0M;

            for (var i = 0; i < grid.RowCount; i++)
            {
                var cellPrice   = grid.Rows[i].Cells[CellName(nameof(Billing.Price))];
                var cellTax     = grid.Rows[i].Cells[CellName(nameof(Billing.TaxAmount))];
                var cellAmount  = grid.Rows[i].Cells[CellName(nameof(Billing.BillingAmount))];

                var price       = Convert.ToDecimal(cellPrice    .EditedFormattedValue);
                var tax         = Convert.ToDecimal(cellTax      .EditedFormattedValue);
                var amount      = Convert.ToDecimal(cellAmount   .EditedFormattedValue);

                totalPrice  += price;
                totalTax    += tax;
                totalAmount += amount;

                var taxId = GetTaxClassId(i);
                if (taxId == (int)TaxClassId.TaxExclusive || taxId == (int)TaxClassId.TaxInclusive) exclusiveAmount += price;
            }
            var taxById = CalculateExclusiveTax(exclusiveAmount);

            lblTotalPrice           .Text = totalPrice.ToString(AmountFormat);
            lblTotalTaxAmount       .Text = totalTax.ToString(AmountFormat);
            lblTotalTaxByInputId    .Text = taxById.ToString(AmountFormat);
            lblTotalBillingAmount   .Text = totalAmount.ToString(AmountFormat);

        }

        private int? GetTaxClassId(int rowIndex)
        {
            if (rowIndex < 0 || MaxGridRowCount <= rowIndex) return null;
            var cell = grid.Rows[rowIndex].Cells[CellName(nameof(Billing.TaxClassId))];
            var id = 0;
            return int.TryParse(cell.Value?.ToString(), out id) ? (int?)id : null;
        }

        #endregion

        #region call web services

        private async Task<Billing> GetBillingById(long id) =>
            await ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
            {
                var result = await client.GetAsync(SessionKey, new[] { id });
                if (result.ProcessResult.Result)
                    return result.Billing.FirstOrDefault();
                return null;
            });

        private async Task<List<Billing>> GetBillingByInputIdAsync(long inputId) =>
            await ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
            {
                var option = new BillingSearch
                {
                    CompanyId = CompanyId,
                    BillingInputId = inputId,
                };
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Billings;
                return new List<Billing>();
            });

        private async Task<BillingDivisionSetting> GetDivisionSettingAsync() =>
            await ServiceProxyFactory.DoAsync(async (BillingDivisionSettingMasterClient client) =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    return result.BillingDivisionSetting;
                return null;
            });

        private async Task<List<BillingDivisionContract>> GetContractAsync(IEnumerable<long> billingIds) =>
            await ServiceProxyFactory.DoAsync(async (BillingDivisionContractMasterClient client) => {
                var result = await client.GetByBillingIdsAsync(SessionKey, billingIds.ToArray());
                if (result.ProcessResult.Result)
                    return result.BillingDivisionContracts;
                return new List<BillingDivisionContract>();
            });

        private async Task<List<BillingDivisionContract>> GetContractAsync(int customerId, string number) =>
            await ServiceProxyFactory.DoAsync(async (BillingDivisionContractMasterClient client) =>
            {
                var result = await client.GetByContractNumberAsync(SessionKey, CompanyId, customerId, number);
                if (result.ProcessResult.Result)
                    return result.BillingDivisionContracts;
                return new List<BillingDivisionContract>();
            });

        private async Task<List<Billing>> SaveAsync(IEnumerable<Billing> billings) =>
            await ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
            {
                var result = await client.SaveForInputAsync(SessionKey, billings.ToArray());
                if (result.ProcessResult.Result)
                    return result.Billings;
                return new List<Billing>();
            });

        private async Task<CountResult> DeleteByInputIdAsync(long inputId) =>
            await ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
                await client.DeleteByInputIdAsync(SessionKey, inputId, 0,
                    ApplicationControl.RegisterContractInAdvance,
                    ApplicationControl.UseDiscount,
                    Login.UserId));

        private async Task<CountResult> DeleteByIdsAsync(IEnumerable<long> ids) =>
            await ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
                await client.DeleteAsync(SessionKey, ids.ToArray(), 0,
                    ApplicationControl.RegisterContractInAdvance,
                    ApplicationControl.UseDiscount,
                    Login.UserId));

        /// <summary>請求データ検索/参照新規 InputId が同一の請求データ取得処理</summary>
        private async Task LoadCurrentGridBillingAsync()
        {
            if (CurrentBilling == null
                || (CurrentBilling.InputType == (int)BillingInputType.Importer && !CurrentBilling.BillingInputId.HasValue)
                || CurrentBilling.InputType == (int)BillingInputType.CashOnDueDate
                || !CurrentBilling.BillingInputId.HasValue) return;
            CurrentGridBilling = await GetBillingByInputIdAsync(CurrentBilling.BillingInputId.Value);
        }

        private async Task LoadGridSettingAsync() =>
            await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    ColumnSettings = result.ColumnNames.Where(x => x.TableName == "Billing").ToList();
            });

        private async Task LoadBillingCategoryAsync() =>
            await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                var option = new CategorySearch
                {
                    CompanyId = CompanyId,
                    CategoryType = CategoryType.Billing,
                };
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    BillingCategories = result.Categories;
            });

        private async Task LoadCollectCategoryAsync() =>
            await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                var option = new CategorySearch
                {
                    CompanyId = CompanyId,
                    CategoryType = CategoryType.Collect,
                };
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                {
                    CollectCategories = result.Categories;
                    foreach (var category in CollectCategories)
                        cmbCategory.Items.Add(new GrapeCity.Win.Editors.ListItem(category.CodeAndName, category.Id));
                }
            });

        private async Task LoadGeneralSettingAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<GeneralSettingMasterClient>();
                var result = await client.GetItemsAsync(SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    GeneralSettings = result.GeneralSettings;
                }
            });
        }

        private async Task LoadCollationSettingInfoAsync()
            => await ServiceProxyFactory.DoAsync(async (CollationSettingMasterClient client) =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    CollationSetting = result.CollationSetting;
            });

        private async Task LoadCurrencyInfoAsync()
            => await ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, new CurrencySearch());
                if (result.ProcessResult.Result)
                    Currencies = result.Currencies;
            });

        private async Task LoadStaffInfoAsync()
            => await ServiceProxyFactory.DoAsync(async (StaffMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, new StaffSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    Staffs = result.Staffs;
            });

        private async Task LoadDepartmentInfoAsync()
            => await ServiceProxyFactory.DoAsync(async (DepartmentMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    Departments = result.Departments;
            });

        private async Task LoadAccountTitleInfoAsync()
            => await ServiceProxyFactory.DoAsync(async (AccountTitleMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, new AccountTitleSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    AccountTitles = result.AccountTitles;
            });

        private async Task LoadHolidayCalendarInfoAsync()
            => await ServiceProxyFactory.DoAsync(async (HolidayCalendarMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, new HolidayCalendarSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    Holidays = result.HolidayCalendars;
            });

        private async Task LoadInputControlInfoAsync()
        {
            await ServiceProxyFactory.DoAsync(async (InputControlMasterClient client) =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId, Login.UserId, InputGridType.Billing);
                if (result.ProcessResult.Result)
                    InputControls = result.InputControls;
            });
        }

        private async Task LoadTaxClassAsync() =>
            await ServiceProxyFactory.DoAsync(async (TaxClassMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey);
                if (result.ProcessResult.Result)
                    TaxClasses = result.TaxClass;
            });

        private async Task<Customer> GetCustomerByIdAsync(int id) =>
            await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
            {
                var result = await client.GetAsync(SessionKey, new int[] { id });
                if (result.ProcessResult.Result)
                    return result.Customers.FirstOrDefault();
                return null;
            });

        private async Task<Customer> GetCustomerByCodeAsync(string code) =>
            await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
            {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new string[] { code });
                if (result.ProcessResult.Result)
                    return result.Customers.FirstOrDefault();
                return null;
            });

        private async Task<List<Destination>> GetDestinationsAsync(int customerId, string code = null) =>
            await ServiceProxyFactory.DoAsync(async (DestinationMasterClient client) => {
                var option = new DestinationSearch {
                    CompanyId = CompanyId,
                    CustomerId = CustomerId,
                };
                if (!string.IsNullOrEmpty(code)) option.Codes = new[] { code };
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Destinations;
                return new List<Destination>();
            });

        private async Task GetUseControlInputNoteAsync()
            => UseControlInputNote = await Util.IsControlInputNoteAsync(Login);

        #endregion

    }
}
