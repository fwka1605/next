using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.PeriodicBillingSettingMasterService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Web.Models.FunctionType;
using Detail = Rac.VOne.Web.Models.PeriodicBillingSettingDetail;
using Header = Rac.VOne.Web.Models.PeriodicBillingSetting;

namespace Rac.VOne.Client.Screen
{
    /// <summary>定期請求パターンマスター</summary>
    /// <remarks>
    /// 税額が変わるタイミング
    /// 税率変更が起きるタイミングで、大量の参照新規を行えという形になる
    /// todo: UseDiscount の制御をしたい場合、明細に UseDiscount の flag が必要
    /// </remarks>
    public partial class PC1601 : VOneScreenBase
    {

        private Header SettingHeader { get; set; } = new Header();
        private Customer CustomerSelected { get; set; }
        private List<HolidayCalendar> Holidays { get; set; } = new List<HolidayCalendar>();
        private List<Department> Departments { get; set; }
        private List<Staff> Staffs { get; set; }
        private List<Currency> Currencies { get; set; }
        private DataExpression Expression { get; set; }

        #region initialize

        public PC1601()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            grid.SetupShortcutKeys();
            Text = "定期請求パターンマスター";

            cmbCycle.Items.AddRange(new[] {
                "毎月",
                "3カ月ごと",
                "6カ月ごと",
                "12カ月ごと"
            });
        }

        private void InitializeHandlers()
        {
            Load += PC1601_Load;
            txtPatternName      .TextChanged  += OnContentChanged;
            txtCustomerCode     .TextChanged  += OnContentChanged;
            txtDestinationCode  .TextChanged  += OnContentChanged;
            cmbCycle   .SelectedIndexChanged  += OnContentChanged;
            txtBilledDay        .TextChanged  += OnContentChanged;
            datStartMonth       .ValueChanged += OnContentChanged;
            datEndMonth         .ValueChanged += OnContentChanged;
            txtInvoiceCode      .TextChanged  += OnContentChanged;
            rdoSetFixedValueToNote1.CheckedChanged += OnContentChanged;
            rdoSetFixedValueToNote2.CheckedChanged += OnContentChanged;

            cmbCycle.SelectedIndexChanged += (sender, e) => SetDateInformation();
            datStartMonth.ValueChanged += (sender, e) => SetDateInformation();
            datEndMonth.ValueChanged += (sender, e) => SetDateInformation();
            txtCustomerCode     .Validated += txtCustomerCode_Validated;
            txtDestinationCode  .Validated += txtDestinationCode_Validated;
            txtBilledDay        .Validated += txtBilledDay_Validated;
            btnPattern      .Click += btnPattern_Click;
            btnCustomer     .Click += btnCustomer_Click;
            btnDestination  .Click += btnDestination_Click;
            btnDetail       .Click += btnDetail_Click;
            grid.CellContentDoubleClick += grid_CellContentDoubleClick;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction08Caption("参照新規");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Save;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = Delete;
            OnF06ClickHandler = Export;
            OnF08ClickHandler = LoadBillingData;
            OnF10ClickHandler = Close;
        }

        private void PC1601_Load(object sender, EventArgs e)
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
                LoadHolidayCalendarsAsync(),
                LoadDepartmentsAsync(),
                LoadStaffsAsync(),
                LoadCurrenciesAsync(),
                LoadFunctionAuthorities(MasterExport),
            };
            await Task.WhenAll(tasks);

            lblInvoiceCodeRemarks.Visible = UsePublishInvoice;
            InitializeTextBoxFormat();
            ClearControlValues();
            InitializeGridTemplate();
            await LoadSettingsAsync();
        }

        private async Task LoadSettingsAsync()
        {
            grid.DataSource = new BindingSource(await GetSettingsAsync(), null);
            BaseContext.SetFunction06Enabled(grid.RowCount > 0 && Authorities[MasterExport]);
        }

        private void InitializeTextBoxFormat()
        {
            Expression = new DataExpression(ApplicationControl);
            txtCustomerCode.MaxLength   = Expression.CustomerCodeLength;
            txtCustomerCode.Format      = Expression.CustomerCodeFormatString;
            txtCustomerCode.PaddingChar = Expression.CustomerCodePaddingChar;
            txtCustomerCode.ImeMode     = Expression.CustomerCodeImeMode();

            txtDestinationCode.MaxLength    = 2;
            txtDestinationCode.Format       = "9";
            txtDestinationCode.PaddingChar  = '0';
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var center = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new[] {
                new CellSetting(height,  60, "RowHeader", cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 150, nameof(Header.Name)            , nameof(Header.Name)           , caption: "パターン名"  , sortable: true),
                new CellSetting(height,  80, nameof(Header.CustomerCode)    , nameof(Header.CustomerCode)   , caption: "得意先コード", sortable: true, cell: builder.GetTextBoxCell(center)),
                new CellSetting(height, 150, nameof(Header.CustomerName)    , nameof(Header.CustomerName)   , caption: "得意先名"    , sortable: true),
                new CellSetting(height,  60, nameof(Header.DestinationCode) , nameof(Header.DestinationCode), caption: "送付先"      , sortable: true, cell: builder.GetTextBoxCell(center)),
                new CellSetting(height, 100, nameof(Header.DestinationName) , nameof(Header.DestinationName), caption: "宛名"        , sortable: true),
                new CellSetting(height,  70, nameof(Header.BilledDay)       , nameof(Header.BilledDay)      , caption: "請求日"      , sortable: true, cell: builder.GetTextBoxCell(center)),
                new CellSetting(height,  70, nameof(Header.StartMonth)      , nameof(Header.StartMonth)     , caption: "開始月"      , sortable: true, cell: builder.GetDateCell_yyyyMM()),
                new CellSetting(height,  70, nameof(Header.EndMonth)        , nameof(Header.EndMonth)       , caption: "終了月"      , sortable: true, cell: builder.GetDateCell_yyyyMM()),
                new CellSetting(height, 100, nameof(Header.InvoiceCode)     , nameof(Header.InvoiceCode)    , caption: "請求書番号"  , sortable: true),
            });

            grid.Template = builder.Build();
        }

        #endregion

        #region function keys

        [OperationLog("登録")]
        private void Save()
        {
            ClearStatusMessage();
            if (!ValidateInputValues()) return;
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            var result = SaveSetting();
            if (!result)
            {
                ShowWarningDialog(MsgErrSaveError);
                return;
            }

            ProgressDialog.Start(ParentForm, LoadSettingsAsync(), false, SessionKey);
            ClearControlValues();

            DispStatusMessage(MsgInfSaveSuccess);
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            ClearControlValues();
        }

        [OperationLog("削除")]
        private void Delete()
        {
            ClearStatusMessage();
            if (!ValidateForDelete()) return;
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            var result = DeleteSettingData();
            if (!result)
            {
                ShowWarningDialog(MsgErrDeleteError);
                return;
            }

            ProgressDialog.Start(ParentForm, LoadSettingsAsync(), false, SessionKey);
            ClearControlValues();

            DispStatusMessage(MsgInfDeleteSuccess);
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            ClearStatusMessage();

            var headers = (grid.DataSource as BindingSource)?.DataSource as List<Header>;
            if (!(headers?.Any() ?? false))
            {
                ShowWarningDialog(MsgWngNoExportData);
                return;
            }

            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var path = task.Result;

            var filePath = string.Empty;
            var fileName = $"定期請求パターンマスター_{DateTime.Today:yyyyMMdd}.csv";
            if (!ShowSaveExportFileDialog(path, fileName, out filePath)) return;

            var result = ExportCsvFile(headers, filePath);
            if (!result)
            {
                ShowWarningDialog(MsgErrExportError);
                return;
            }
            DispStatusMessage(MsgInfFinishExport);
        }

        [OperationLog("参照新規")]
        private void LoadBillingData()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

            var billings = GetBillingDataFromBillingSearch();
            if (billings == null) return;
            ProgressDialog.Start(ParentForm, SetBillingToSetting(billings), false, SessionKey);
        }

        [OperationLog("終了")]
        private void Close()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }

        #endregion

        #region save

        private bool ValidateInputValues()
        {
            if (!txtPatternName.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblPatternName.Text))) return false;
            if (!txtCustomerCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblCustomerCode.Text))) return false;
            if (!cmbCycle.ValidateInputted(() => ShowWarningDialog(MsgWngSelectionRequired, lblBilledCycle.Text))) return false;
            if (!txtBilledDay.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblBilledCycle.Text))) return false;
            if (!datStartMonth.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblStartMonth.Text))) return false;

            if (!datStartMonth.ValidateRange(datEndMonth, () => ShowWarningDialog(MsgWngInputRangeChecked, "年月"))) return false;

            if (datEndMonth.Value.HasValue && SettingHeader.LastCreateYearMonth.HasValue
                && datEndMonth.Value.Value < SettingHeader.LastCreateYearMonth.Value)
            {
                datEndMonth.Focus();
                ShowWarningDialog(MsgWngInputValidDateForParameters, lblEndMonth.Text, SettingHeader.LastCreateYearMonth.Value.ToString("yyyy/MM"));
                return false;
            }

            if (!SettingHeader.IsDetailsInputted)
            {
                ShowWarningDialog(MsgWngInputRequired, lblDetails.Text);
                return false;
            }
            return true;
        }

        private int GetSelectedBilledCycle()
            => cmbCycle.SelectedIndex == 0 ?  1
             : cmbCycle.SelectedIndex == 1 ?  3
             : cmbCycle.SelectedIndex == 2 ?  6
             : cmbCycle.SelectedIndex == 3 ? 12 : -1;

        private DateTime? ToFirstDay(DateTime? dat)
            => dat.HasValue ? (DateTime?)new DateTime(dat.Value.Year, dat.Value.Month, 1) : null;

        private void PrepareForSaveSetttingHeader()
        {
            SettingHeader.CompanyId     = CompanyId;
            SettingHeader.Name          = txtPatternName.Text;
            SettingHeader.BilledCycle   = GetSelectedBilledCycle();
            SettingHeader.BilledDay     = int.Parse(txtBilledDay.Text);
            SettingHeader.StartMonth    = ToFirstDay(datStartMonth.Value).Value;
            SettingHeader.EndMonth      = ToFirstDay(datEndMonth.Value);
            SettingHeader.InvoiceCode   = txtInvoiceCode.Text;
            SettingHeader.SetBillingNote1 = rdoSetFixedValueToNote1.Checked ? 0 : 1;
            SettingHeader.SetBillingNote2 = rdoSetFixedValueToNote2.Checked ? 0 : 1;
            SettingHeader.CreateBy      = Login.UserId;
            SettingHeader.UpdateBy      = Login.UserId;
        }

        private bool SaveSetting()
        {
            PrepareForSaveSetttingHeader();
            var task = SaveAsync(SettingHeader);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return false;
            }
            var result = task.Result;
            SettingHeader = result ?? new Header();
            return result != null;
        }

        #endregion

        #region clear

        private void ClearControlValues()
        {
            txtPatternName.Clear();
            txtCustomerCode.Clear();
            lblCustomerName.Clear();
            txtDestinationCode.Clear();
            lblDestinationName.Clear();
            txtDestinationCode.Enabled = false;
            btnDestination.Enabled = false;
            cmbCycle.SelectedIndex = -1;
            txtBilledDay.Clear();
            datStartMonth.Clear();
            datEndMonth.Clear();
            txtInvoiceCode.Clear();
            rdoSetFixedValueToNote1.Checked = true;
            rdoSetFixedValueToNote2.Checked = true;

            SettingHeader = new Header();

            txtCustomerCode .Enabled = true;
            btnCustomer     .Enabled = true;
            cmbCycle        .Enabled = true;
            txtBilledDay    .Enabled = true;
            datStartMonth   .Enabled = true;

            lblDisplayRepeatCount.Clear();

            SetDetailInputStatus();

            Modified = false;

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(true);
        }

        #endregion

        #region delete

        private bool ValidateForDelete() => (SettingHeader.Id != 0L);

        private bool DeleteSettingData()
        {
            var task = DeleteAsnc(SettingHeader.Id);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        #endregion

        #region export

        // for whom
        private bool ExportCsvFile(List<Header> headers, string filePath)
        {
            var definition = new PeriodicBillingSettingFileDifinition(Expression);
            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            ProgressDialog.Start(ParentForm, (cancel, progress)
                => exporter.ExportAsync(filePath, headers, cancel, progress), true, SessionKey);
            if (exporter.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                return false;
            }
            return true;
        }

        #endregion

        #region load from billing data

        /// <summary>請求データ検索 から 請求データの取得</summary>
        private List<Billing> GetBillingDataFromBillingSearch()
        {
            Billing selected = null;
            using (var form = ApplicationContext.Create(nameof(PC0301)))
            {
                var screen = form.GetAll<PC0301>().FirstOrDefault();
                screen.ReturnScreen = this;
                form.StartPosition = FormStartPosition.CenterParent;
                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form);
                if (dialogResult != DialogResult.OK) return null;
                selected = screen.grdSearchResult.CurrentRow.DataBoundItem as Billing;
            }
            if (selected == null) return null;
            if (!selected.BillingInputId.HasValue) return new List<Billing> { selected };
            var task = GetBillingByInputIdAsync(selected.BillingInputId.Value);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }

        private async Task<Header> ConvertBillingsToSettingAsync(List<Billing> billings)
        {
            var first = billings.First();
            var header = new Header {
                CompanyId           = CompanyId,
                CurrencyId          = first.CurrencyId,
                CustomerId          = first.CustomerId,
                CustomerCode        = first.CustomerCode,
                CustomerName        = first.CustomerName,
                DestinationId       = first.DestinationId,
                DepartmentId        = first.DepartmentId,
                StaffId             = first.StaffId,
                CollectCategoryId   = first.CollectCategoryId,
            };
            header.BilledCycle = 1;
            header.BilledDay = ConvertValidBilledDay(first.BilledAt.Day);
            header.StartMonth = ToFirstDay(DateTime.Today).Value;
            if (header.DestinationId.HasValue)
            {
                var destination = (await GetDestinationAsync(first.CustomerId))
                    .FirstOrDefault(x => x.Id == first.DestinationId.Value);
                header.DestinationCode = destination.Code;
                header.DestinationName = destination.ToString();
            }
            header.Details = billings.Select((x, index) => ConvertBillingToDetail(x, index)).ToList();
            return header;
        }
        private async Task SetBillingToSetting(List<Billing> billings)
        {
            var header = await ConvertBillingsToSettingAsync(billings);
            await SetHeaderToControlsAsync(header, isReferencedNewData: true);
        }

        #endregion

        #region event handlers

        private void OnContentChanged(object sender, EventArgs e) => Modified = true;

        private void txtCustomerCode_Validated(object sender, EventArgs e)
        {
            var code = txtCustomerCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                ClearCustomerInfomation();
                SetDestinationControlEnabled(false);
                return;
            }
            var load = GetCustomerByCodeAsync(code);
            ProgressDialog.Start(ParentForm, load, false, SessionKey);
            var customer = load.Result;
            if (customer == null)
            {
                ClearCustomerInfomation();
                txtCustomerCode.Focus();
                ShowWarningDialog(MsgWngMasterNotExist, "得意先", code);
                return;
            }
            SetCustomerInformation(customer);
        }

        private void txtDestinationCode_Validated(object sender, EventArgs e)
        {
            var code = txtDestinationCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                ClearDestinationInfomation();
                return;
            }
            var load = GetDestinationByCodeAsync(SettingHeader.CustomerId, code);
            ProgressDialog.Start(ParentForm, load, false, SessionKey);
            var destination = load.Result;
            if (destination == null)
            {
                ClearDestinationInfomation();
                txtDestinationCode.Focus();
                ShowWarningDialog(MsgWngMasterNotExist, "送付先", code);
                return;
            }
            SetDestinationInfomation(destination);
        }

        private void txtBilledDay_Validated(object sender, EventArgs e)
        {
            try {
                if (string.IsNullOrEmpty(txtBilledDay.Text)) return;
                var day = 0;
                if (!int.TryParse(txtBilledDay.Text, out day)) return;
                day = ConvertValidBilledDay(day);
                txtBilledDay.Text = day.ToString();
            }
            finally {
                SetDateInformation();
            }
        }

        /// <summary>有効な請求日 へ変換 1..27/99</summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private int ConvertValidBilledDay(int day)
        {
            if (day <= 0) day = 1;
            if (28 <= day) day = 99;
            return day;
        }

        private void btnPattern_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;
            var header = this.ShowPeriodicBillingSettingSearchDialog();
            if (header == null) return;
            ProgressDialog.Start(ParentForm, SetHeaderToControlsAsync(header), false, SessionKey);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var customerMin = this.ShowCustomerMinSearchDialog();
            Customer customer = null;
            if (customerMin != null)
            {
                var task = GetCustomerByCodeAsync(customerMin.Code);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                customer = task.Result;
            }
            SetCustomerInformation(customer);
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            SetDestinationInfomation(this.ShowDestinationSearchDialog(SettingHeader?.CustomerId ?? 0));
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            ShowDeitalSettingDialog();
        }

        private void grid_CellContentDoubleClick(object sender, GrapeCity.Win.MultiRow.CellEventArgs e)
        {
            if (e.Scope != GrapeCity.Win.MultiRow.CellScope.Row) return;
            var header = grid.Rows[e.RowIndex].DataBoundItem as Header;
            if (header == null) return;
            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;
            ProgressDialog.Start(ParentForm, SetHeaderToControlsAsync(header), false, SessionKey);
        }

        #endregion

        #region set/clear master data to controls

        private void ClearCustomerInfomation()
        {
            txtCustomerCode.Clear();
            lblCustomerName.Clear();
            if (SettingHeader != null) SettingHeader.CustomerId = 0;
        }

        private void SetCustomerInformation(Customer customer)
        {
            CustomerSelected = customer;
            if (customer == null) return;
            txtCustomerCode.Text = customer.Code;
            lblCustomerName.Text = customer.Name;
            SettingHeader.CustomerId = customer.Id;
            SetDestinationControlEnabled(true);
        }

        private void SetDestinationControlEnabled(bool enabled)
        {
            txtDestinationCode.Enabled = enabled;
            btnDestination.Enabled = enabled;
            if (enabled) return;
            ClearDestinationInfomation();
        }

        private void ClearDestinationInfomation()
        {
            txtDestinationCode.Clear();
            lblDestinationName.Clear();
            SettingHeader.DestinationId = null;
        }

        private void SetDestinationInfomation(Destination destination)
        {
            if (destination == null) return;
            txtDestinationCode.Text = destination.Code;
            lblDestinationName.Text = destination.ToString();
            SettingHeader.DestinationId = destination.Id;
        }

        private void SetDateInformation()
        {

            ClearStatusMessage();

            var cycleSelecteted = (cmbCycle.SelectedIndex >= 0);
            var startSelected = datStartMonth.Value.HasValue;
            var daySelected = !string.IsNullOrEmpty(txtBilledDay.Text);
            var customerSelected = CustomerSelected != null;
            if (!startSelected || !daySelected || !customerSelected)
            {
                datBilledAt.Clear();
                datSalesAt.Clear();
                datClosingAt.Clear();
                datDueAt.Clear();
                lblDisplayRepeatCount.Clear();
            }
            else
            {
                var ym = datStartMonth.Value.Value.Date;
                var day = int.Parse(txtBilledDay.Text);
                var billedAt = new DateTime(ym.Year, ym.Month,
                    ((28 < day) ? DateTime.DaysInMonth(ym.Year, ym.Month) : day));
                datBilledAt.Value = billedAt;
                datSalesAt .Value = billedAt;
                var closingAt = CustomerSelected.GetClosingAt(billedAt);
                datClosingAt.Value = closingAt;
                if (closingAt.HasValue)
                    datDueAt.Value = CustomerSelected.GetDueAt(closingAt.Value, Holidays);

                if (datEndMonth.Value.HasValue
                    && daySelected && cycleSelecteted
                    && datStartMonth.Value.Value <= datEndMonth.Value.Value)
                {
                    var end = datEndMonth.Value.Value;
                    var start = datStartMonth.Value.Value;
                    var months = (end.Year * 12 + end.Month) - (start.Year * 12 + start.Month);
                    var cycle = GetSelectedBilledCycle();
                    var count = (months / cycle) + 1;
                    lblDisplayRepeatCount.Text = count.ToString();
                }
                else
                    lblDisplayRepeatCount.Clear();
            }

            btnDetail.Enabled = cycleSelecteted && startSelected && daySelected && customerSelected;
        }

        private void SetDetailInputStatus()
        {
            lblDetailInputStatus.Text = SettingHeader.IsDetailsInputted
                ? $"入力済 請求額計：{SettingHeader.BillingAmount:#,##0}"
                : "未入力";
        }

        /// <summary><see cref="Header"/>をコントロールに設定
        /// 参照新規も流用</summary>
        /// <param name="header"></param>
        /// <param name="isReferencedNewData">参照新規か否か</param>
        /// <returns></returns>
        private async Task SetHeaderToControlsAsync(Header header, bool isReferencedNewData = false)
        {
            if (header == null) return;
            txtPatternName.Text = header.Name;

            if (header.LastCreateYearMonth.HasValue)
            {
                txtCustomerCode.Enabled = false;
                btnCustomer.Enabled = false;
                cmbCycle.Enabled = false;
                txtBilledDay.Enabled = false;
                datStartMonth.Enabled = false;
            }

            var customer = await GetCustomerByCodeAsync(header.CustomerCode);
            SetCustomerInformation(customer);
            if (!string.IsNullOrEmpty(header.DestinationCode))
                SetDestinationInfomation(await GetDestinationByCodeAsync(customer.Id, header.DestinationCode));
            cmbCycle.SelectedIndex
                = header.BilledCycle ==  1 ? 0
                : header.BilledCycle ==  3 ? 1
                : header.BilledCycle ==  6 ? 2
                : header.BilledCycle == 12 ? 3 : -1;
            txtBilledDay.Text = header.BilledDay.ToString();
            datStartMonth.Value = header.StartMonth;
            datEndMonth.Value = header.EndMonth;
            txtInvoiceCode.Text = header.InvoiceCode;
            if (header.SetBillingNote1 == 0)
                rdoSetFixedValueToNote1.Checked = true;
            else
                rdoSetYearMonthToNote1.Checked = true;
            if (header.SetBillingNote2 == 0)
                rdoSetFixedValueToNote2.Checked = true;
            else
                rdoSetYearMonthToNote2.Checked = true;
            SettingHeader = header;
            SetDetailInputStatus();
            Modified = false;

            BaseContext.SetFunction03Enabled(!isReferencedNewData);
            BaseContext.SetFunction08Enabled(false);
        }

        #endregion

        #region show detail setting

        private void ShowDeitalSettingDialog()
        {
            using (var form = ApplicationContext.Create(nameof(PC1602)))
            {
                var staffId = SettingHeader.StaffId;
                if (staffId == 0) staffId = CustomerSelected.StaffId;
                var staff = Staffs.First(x => x.Id == staffId);
                var departmentId = SettingHeader.DepartmentId;
                if (departmentId == 0) departmentId = staff.DepartmentId;
                var department = Departments.First(x => x.Id == departmentId);

                var screen = form.GetAll<PC1602>().First();
                #region initialize PC1602 header/setting values
                screen.datBilledAt.Value = datBilledAt.Value;
                screen.datSalesAt.Value = datSalesAt.Value;
                screen.datClosingAt.Value = datClosingAt.Value;
                screen.datDueAt.Value = datDueAt.Value;
                screen.txtCustomerCode.Text = txtCustomerCode.Text;
                screen.lblCustomerName.Text = lblCustomerName.Text;
                screen.CustomerSelected = CustomerSelected;
                screen.txtDestinationCode.Text = txtDestinationCode.Text;
                screen.lblDestinationName.Text = lblDestinationName.Text;
                screen.txtInvoiceCode.Text = txtInvoiceCode.Text;

                screen.txtStaffCode.Text = staff.Code;
                screen.lblStaffName.Text = staff.Name;
                screen.txtDepartmentCode.Text = department.Code;
                screen.lblDepartmentName.Text = department.Name;
                screen.Note1Enabled = rdoSetFixedValueToNote1.Checked;
                screen.Note2Enabled = rdoSetFixedValueToNote2.Checked;

                screen.SettingHeader = SettingHeader;
                #endregion

                if (ApplicationContext.ShowDialog(ParentForm, form) != DialogResult.OK) return;

                SettingHeader.DepartmentId = Departments.First(x => x.Code == screen.txtDepartmentCode.Text).Id;
                SettingHeader.StaffId = Staffs.First(x => x.Code == screen.txtStaffCode.Text).Id;
                SettingHeader.CollectCategoryId = (int)screen.cmbCollectCategory.SelectedItem.SubItems[1].Value;
                SettingHeader.CurrencyId = Currencies.First(x => x.Code == screen.txtCurrencyCode.Text).Id;
                SettingHeader.Details = screen.GetBillingsFromGrid()
                    .Select((x, index) => {
                        x.DisplayOrder = index + 1;
                        return x;
                    }).ToList();
                SetDetailInputStatus();
            }
        }

        /// <summary>参照新規用</summary>
        private Detail ConvertBillingToDetail(Billing x, int index)
            => new Detail {
                DisplayOrder        = index + 1,
                BillingCategoryId   = x.BillingCategoryId,
                TaxClassId          = x.TaxClassId,
                DebitAccountTitleId = x.DebitAccountTitleId,
                Quantity            = x.Quantity,
                UnitSymbol          = x.UnitSymbol,
                UnitPrice           = x.UnitPrice,
                Price               = x.Price,
                TaxAmount           = x.TaxAmount,
                BillingAmount       = x.BillingAmount,
                Note1               = x.Note1 ?? string.Empty,
                Note2               = x.Note2 ?? string.Empty,
                Note3               = x.Note3 ?? string.Empty,
                Note4               = x.Note4 ?? string.Empty,
                Note5               = x.Note5 ?? string.Empty,
                Note6               = x.Note6 ?? string.Empty,
                Note7               = x.Note7 ?? string.Empty,
                Note8               = x.Note8 ?? string.Empty,
                Memo                = x.Memo  ?? string.Empty,
            };

        #endregion

        #region call web services
        private async Task<List<Billing>> GetBillingByInputIdAsync(long inputId) =>
            await ServiceProxyFactory.DoAsync(async (BillingService.BillingServiceClient client) =>
            {
                var option = new BillingSearch {
                    CompanyId = CompanyId,
                    BillingInputId = inputId,
                };
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Billings;
                return new List<Billing>();
            });

        private async Task LoadHolidayCalendarsAsync()
            => await ServiceProxyFactory.DoAsync(async (HolidayCalendarMasterService.HolidayCalendarMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, new HolidayCalendarSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    Holidays = result.HolidayCalendars;
            });

        private async Task<Customer> GetCustomerByCodeAsync(string code)
            => await ServiceProxyFactory.DoAsync(async (CustomerMasterService.CustomerMasterClient client) => {
                var result = await client.GetByCodeAsync(SessionKey, CompanyId, new[] { code });
                if (result.ProcessResult.Result)
                    return result.Customers.FirstOrDefault();
                return null;
            });

        private async Task LoadDepartmentsAsync()
            => await ServiceProxyFactory.DoAsync(async (DepartmentMasterService.DepartmentMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    Departments = result.Departments;
            });

        private async Task LoadStaffsAsync()
            => await ServiceProxyFactory.DoAsync(async (StaffMasterService.StaffMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, new StaffSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    Staffs = result.Staffs;
            });

        private async Task LoadCurrenciesAsync()
            => await ServiceProxyFactory.DoAsync(async (CurrencyMasterService.CurrencyMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, new CurrencySearch { });
                if (result.ProcessResult.Result)
                    Currencies = result.Currencies;
            });

        private async Task<Destination> GetDestinationByCodeAsync(int customerId, string code)
            => (await GetDestinationAsync(customerId, code)).FirstOrDefault();
        private async Task<List<Destination>> GetDestinationAsync(int customerId, string code = null)
            => await ServiceProxyFactory.DoAsync(async (DestinationMasterService.DestinationMasterClient client) => {
                var option = new DestinationSearch {
                    CompanyId = CompanyId,
                    CustomerId = customerId,
                };
                if (!string.IsNullOrEmpty(code)) option.Codes = new[] { code };
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.Destinations;
                return new List<Destination>();
            });

        private async Task<List<Header>> GetSettingsAsync()
            => await ServiceProxyFactory.DoAsync(async (PeriodicBillingSettingMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, new PeriodicBillingSettingSearch {
                    CompanyId = CompanyId,
                });
                if (result.ProcessResult.Result)
                    return result.PeriodicBillingSettings;
                return new List<Header>();
            });

        private async Task<Header> SaveAsync(Header setting)
            => await ServiceProxyFactory.DoAsync(async (PeriodicBillingSettingMasterClient client) => {
                var result = await client.SaveAsync(SessionKey, setting);
                if (result.ProcessResult.Result)
                    return result.PeriodicBillingSetting;
                return null;
            });

        private async Task<bool> DeleteAsnc(long id)
            => await ServiceProxyFactory.DoAsync(async (PeriodicBillingSettingMasterClient client) => {
                var result = await client.DeleteAsync(SessionKey, id);
                return (result?.ProcessResult.Result ?? false) && result.Count > 0;
            });

        #endregion
    }
}
