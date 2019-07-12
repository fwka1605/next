using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CollationSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerGroupMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GridSettingMasterService;
using Rac.VOne.Client.Screen.MatchingService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.PaymentAgencyMasterService;
using Rac.VOne.Common;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>個別消込</summary>
    public partial class PE0102 : VOneScreenBase
    {
        #region third party usable properties
        public Func<IEnumerable<ITransactionData>, bool> MatchingPostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> AdvanceReceivedPostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> NettingPostProcessor { get; set; }
        private bool IsPostProcessorImplemented
        {
            get
            {
                return MatchingPostProcessor != null
                    || AdvanceReceivedPostProcessor != null
                    || NettingPostProcessor != null;
            }
        }

        #endregion

        internal Collation Collation { private get; set; }
        internal CollationSearch CollationSearch { private get; set; }
        internal MatchingHeader MatchingHeader { private get; set; }
        internal bool UseAdvanceReceived
        {
            get { return cbxAdvanceReceived.Checked; }
            set { cbxAdvanceReceived.Checked = value; }
        }

        /// <summary>
        ///  一括消込画面 grid の Row Index
        /// </summary>
        internal int SelectRowIndex { private get; set; }

        /// <summary>
        ///  個別消込画面 表示内容について消込済かどうか判定
        ///   false : 未消込データ, true : 消込済データ
        /// </summary>
        internal bool IsMatched { private get; set; }

        internal GcMultiRow CollationGrid { private get; set; }
        private List<int> machedCollationIndices { get; set; } = new List<int>();
        internal List<int> MatchedCollationIndices
        {
            get { return machedCollationIndices; }
            set { machedCollationIndices = value; }
        }
        internal List<Web.Models.Section> Sections { private get; set; }
        internal List<Web.Models.Section> SectionsWithLoginUser { private get; set; }
        internal List<int> SectionIds { private get; set; }
        internal List<Department> Departments { private get; set; }
        internal List<Department> DepartmentsWithLoginUser { private get; set; }
        internal List<int> DepartmentIds { private get; set; }

        private CollationSetting CollationSetting { get; set; }
        private List<Billing> BillingList { get; set; } = null;
        private List<Receipt> ReceiptList { get; set; } = null;
        private List<GridSetting> ReceiptGridInfo { get; set; }
        private List<GridSetting> BillingGridInfo { get; set; }
        private List<Currency> CurrencyList { get; set; }
        private decimal CustomerFee { get; set; }
        /// <summary>
        ///  新規債権代表者となる得意先ID
        /// </summary>
        private int? NewSelectedParentCustomerId { get; set; }
        private int GridSelectedParentCustomerId { get; set; }
        /// <summary>
        ///  消込時に 選択されている請求データの 親（子）得意先の 債権代表者フラグ
        ///  Collation.IsParent または 消 チェックで選択した代表得意先 が 債権代表者となっているかどうか
        ///  消込済データ表示時には 一切考慮する必用なし
        /// </summary>
        private int GridSelectedParentCustomerIsParent { get; set; }
        private string GridSelectedParentCustomerCode { get; set; }
        private bool BillingGridChanged { get; set; } = false;
        private bool ReceiptGridChanged { get; set; } = false;
        private List<object> Search { get; set; }
        private int NoOfPrecision { get; set; }

        private bool IsReceiptEdited { get; set; }

        #region 初期化処理

        public PE0102()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
            Text = "個別消込";
        }

        private void InitializeUserComponent()
        {
            grdBilling.SetupShortcutKeys();
            grdBilling.GridColorType = GridColorType.Special;
            grdReceipt.SetupShortcutKeys();
            grdReceipt.GridColorType = GridColorType.Special;
        }

        private void InitializeHandlers()
        {
            grdReceipt.CellDoubleClick += grdReceipt_CellDoubleClick;
            grdReceipt.CellEditedFormattedValueChanged += grdReceipt_CellEditedFormattedValueChanged;
            grdReceipt.CellValueChanged += grdReceipt_CellValueChanged;

            grdBilling.CellDoubleClick += grdBilling_CellDoubleClick;
            grdBilling.CellEditedFormattedValueChanged += grdBilling_CellEditedFormattedValueChanged;
            grdBilling.CellValidated += grdBilling_CellValidated;
            grdBilling.CellValueChanged += grdBilling_CellValueChanged;

            grdReceipt.CellFormatting += grid_CellFormatting;
            grdBilling.CellFormatting += grid_CellFormatting;
            Action<object, EventArgs> currentCellChangedHandler = (sender, e) =>
            {
                var grid = sender as Common.Controls.VOneGridControl;
                if (grid?.CurrentCell == null) return;
                grid.Refresh();
            };
            grdReceipt.CurrentCellChanged += (sender, e) => currentCellChangedHandler(sender, e);
            grdBilling.CurrentCellChanged += (sender, e) => currentCellChangedHandler(sender, e);
            cbxDispLeftRight.CheckedChanged += (sender, e) => UpdateScreen_cbxDispLeftRightChanged();
            cbxAdvanceReceived.CheckedChanged += (sender, e) => UpdateScreen_cbxAdvanceReceivedChanged();

        }

        private void PE0102_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                Settings.SetCheckBoxValue<PE0102>(Login, cbxDispLeftRight);

                InitializeForData(LoadInitialDataAsync());
                InitializeControlsEnabled();
                if(CollationSetting.BillingReceiptDisplayOrder == 1)
                    InitializeGridLocation();

                UpdateScreen_cbxDispLeftRightChanged();
                UpdateScreen_cbxAdvanceReceivedChanged();

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task LoadInitialDataAsync()
        {
            var tasks = new List<Task>();
            tasks.Add(LoadCompanyAsync());
            tasks.Add(LoadCollationSettingAsync());
            if (ApplicationControl == null)
                tasks.Add(LoadApplicationControlAsync());
            tasks.Add(LoadBillingGridSettingAsync());
            tasks.Add(LoadReceiptGridSettingAsync());
            tasks.Add(LoadCurrencyAsync());
            tasks.Add(LoadControlColorAsync());
            await Task.WhenAll(tasks);

            if (UseMFWebApi && !IsMatched)
            {
                var setting = await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterService.WebApiSettingMasterClient client) =>
                (await client.GetByIdAsync(SessionKey, CompanyId, WebApiType.MoneyForward))?.WebApiSetting);
                if (setting != null)
                {
                    var setter = new MFCInvoice.MFBillingStatusSetter {
                        Login           = Login,
                        OwnerForm       = ParentForm,
                        IsMatched       = true,
                        WebApiSetting   = setting,
                    };
                    MatchingPostProcessor = setter.Update;
                }
            }

            tasks.Clear();
            tasks.Add(LoadBillingsAsync());
            tasks.Add(LoadReceiptsAsync());
            await Task.WhenAll(tasks);
        }

        private void UpdateScreen_cbxDispLeftRightChanged()
        {
            if (cbxDispLeftRight.Checked)
            {
                cbxDispLeftRight.Text = "左右表示";
                DisplayLeftRight();
            }
            else
            {
                cbxDispLeftRight.Text = "上下表示";
                DisplayTopUnder();
            }
        }

        private void UpdateScreen_cbxAdvanceReceivedChanged()
        {
            if (cbxAdvanceReceived.Checked)
            {
                cbxAdvanceReceived.Text = "繰越を前受とする";
            }
            else
            {
                cbxAdvanceReceived.Text = "繰越を請求残とする";
            }
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("消込");
            BaseContext.SetFunction02Caption("印刷");
            BaseContext.SetFunction03Caption("入金入力");
            BaseContext.SetFunction04Caption("請求検索");
            BaseContext.SetFunction05Caption("入金検索");
            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction08Caption("前へ");
            BaseContext.SetFunction09Caption("次へ");
            BaseContext.SetFunction10Caption("戻る");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(true);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Matching;
            OnF02ClickHandler = Print;
            OnF03ClickHandler = InputReceipt;
            OnF04ClickHandler = SearchBilling;
            OnF05ClickHandler = SearchReceipt;
            OnF06ClickHandler = ExportData;
            OnF08ClickHandler = MovePrevious;
            OnF09ClickHandler = MoveNext;
            OnF10ClickHandler = Close;
        }

        private void InitializeGridLocation()
        {
            spcMatching.Panel1.Controls.Remove(grdBilling);
            spcMatching.Panel1.Controls.Add(grdReceipt);
            spcMatching.Panel2.Controls.Remove(grdReceipt);
            spcMatching.Panel2.Controls.Add(grdBilling);

            int gbxBillingX = gbxBilling.Location.X;
            int gbxBillingY = gbxBilling.Location.Y;
            int gbxReceiptX = gbxReceipt.Location.X;
            int gbxReceiptY = gbxReceipt.Location.Y;

            gbxBilling.Location = new System.Drawing.Point(gbxReceiptX, gbxReceiptY);
            gbxReceipt.Location = new System.Drawing.Point(gbxBillingX, gbxBillingY);

        }

        private void InitializeControlsEnabled()
        {
            if (!UseDiscount)
            {
                lblDiscount.Visible = false;
                nmbDiscount.Visible = false;
                btnEditDiscount.Visible = false;
            }
            if (!UseForeignCurrency)
            {
                lblCurrencyCode.Visible = false;
                txtCurrencyCode.Visible = false;
            }
            if (!UseScheduledPayment || UseDeclaredAmount)
            {
                cbxAdvanceReceived.Visible = false;
            }
            if (!IsMatched)
            {
                if (UseBillingFilter)
                {
                    BaseContext.SetFunction07Enabled(true);
                    BaseContext.SetFunction07Caption("請求絞込");
                    OnF07ClickHandler = ShowBillingFilterDialg;
                }
                else
                {
                    BaseContext.SetFunction07Enabled(false);
                    BaseContext.SetFunction07Caption("");
                }
            }
            else
            {
                btnBillingCheckAll.Enabled = false;
                btnBillingClear.Enabled = false;
                btnBillingUncheckAll.Enabled = false;
                btnReceiptCheckAll.Enabled = false;
                btnSimulation.Enabled = false;
                btnEditDiscount.Enabled = false;
                btnReceiptUncheckAll.Enabled = false;
                btnReceiptClear.Enabled = false;
                nmbTransferFee.Enabled = false;
                nmbBillingTaxDifference.Enabled = false;
                nmbReceiptTaxDifference.Enabled = false;
                lblBillingMatching.Text = "消込額";
                lblReceiptMatching.Text = "消込額";
                cbxAdvanceReceived.Visible = false;

                BaseContext.SetFunction07Caption("メモ参照");
                OnF07ClickHandler = ShowMatchingMemo;

                BaseContext.SetFunction01Enabled(false);
                BaseContext.SetFunction02Enabled(false);
                BaseContext.SetFunction03Enabled(false);
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction05Enabled(false);
            }
        }

        /// <summary>個別消込 消込/未消込 データ表示処理</summary>
        private void InitializeForData(Task task)
        {
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (IsMatched)
            {
                BaseContext.SetFunction07Enabled(!string.IsNullOrEmpty(MatchingHeader?.Memo));
            }
            else
            {
                if (Collation?.PaymentAgencyId == 0)
                {
                    BaseContext.SetFunction03Enabled(true);
                    BaseContext.SetFunction04Enabled(true);
                }
                else
                {
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction04Enabled(false);
                }
            }

            datReceiptRecordedAtTo.Value = CollationSearch.RecordedAtTo;
            datBillingDueAtTo.Value = CollationSearch.DueAtTo;

            if (Collation != null && !IsMatched)
            {
                txtCusName.Text = Collation.DispCustomerName;
                GridSelectedParentCustomerId = Collation.CustomerId;
                GridSelectedParentCustomerCode = Collation.CustomerCode;
                GridSelectedParentCustomerIsParent = Collation.IsParent;
            }
            else if (MatchingHeader != null && IsMatched)
            {
                txtCusName.Text = MatchingHeader.DispCustomerName;
                GridSelectedParentCustomerId = MatchingHeader.CustomerId ?? 0;
                GridSelectedParentCustomerCode = MatchingHeader.CustomerCode;
            }


            BaseContext.SetFunction08Enabled(SelectRowIndex > 0);
            BaseContext.SetFunction09Enabled(SelectRowIndex < CollationGrid.Rows.Count -1);

            InitializeNumberControls();

            InitializeBillingGridTemplate();
            SetBillingData();
            InitializeReceiptGridTemplate();
            SetReceiptData();
        }

        private void InitializeNumberControls()
        {
            nmbTransferFee.DisplayFields.Clear();
            nmbBillingTaxDifference.DisplayFields.Clear();
            nmbReceiptTaxDifference.DisplayFields.Clear();
            nmbDiscount.DisplayFields.Clear();

            var currencyCode = "";
            var bankFee = 0M;
            var receiptTaxDiff = 0M;
            var billingTaxDiff = 0M;
            //var discount = 0M;
            if (IsMatched && MatchingHeader != null)
            {
                currencyCode = MatchingHeader.CurrencyCode;
                bankFee = MatchingHeader.BankTransferFee;
                receiptTaxDiff = (MatchingHeader.TaxDifference < 0M) ? MatchingHeader.TaxDifference * -1M : 0M;
                billingTaxDiff = (MatchingHeader.TaxDifference > 0M) ? MatchingHeader.TaxDifference : 0M;
            }
            else if (!IsMatched && Collation != null)
            {
                currencyCode = Collation.CurrencyCode;
                bankFee = Collation.BankTransferFee;
                receiptTaxDiff = (Collation.TaxDifference < 0M) ? Collation.TaxDifference * -1M : 0M;
                billingTaxDiff = (Collation.TaxDifference > 0M) ? Collation.TaxDifference : 0M;
            }
            nmbTransferFee.Value = bankFee;
            nmbReceiptTaxDifference.Value = receiptTaxDiff;
            nmbBillingTaxDifference.Value = billingTaxDiff;
            txtCurrencyCode.Text = currencyCode;

            var precision = UseForeignCurrency && !string.IsNullOrEmpty(currencyCode)
                ? CurrencyList.FirstOrDefault(x => x.Code == currencyCode)?.Precision ?? 0 : 0;

            NoOfPrecision = precision;
            nmbTransferFee.Fields.DecimalPart.MaxDigits = precision;
            nmbBillingTaxDifference.Fields.DecimalPart.MaxDigits = precision;
            nmbReceiptTaxDifference.Fields.DecimalPart.MaxDigits = precision;
            nmbDiscount.Fields.DecimalPart.MaxDigits = precision;

            nmbTransferFee.DisplayFields.AddRange(GetNumberFormat("#,###,##0", NoOfPrecision), "", "", "-", "");
            nmbBillingTaxDifference.DisplayFields.AddRange(GetNumberFormat("#,##0", NoOfPrecision), "", "", "-", "");
            nmbReceiptTaxDifference.DisplayFields.AddRange(GetNumberFormat("#,##0", NoOfPrecision), "", "", "-", "");
            nmbDiscount.DisplayFields.AddRange(GetNumberFormat("##,###,###,##0", NoOfPrecision), "", "", "-", "");

        }

        private void InitializeReceiptGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var template = new Template();
            var gridCount = ReceiptGridInfo.Count;
            #region header 1
            var totalwidth = 40;
            var titleWidth = 40;
            foreach (var item in ReceiptGridInfo)
            {
                totalwidth += item.DisplayWidth;
                if (item.ColumnName == "PayerName")
                {
                    titleWidth += item.DisplayWidth;
                }
            }
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, titleWidth, "receiptInfo1", caption: "入金情報" ),
                new CellSetting(height, totalwidth - titleWidth, "receiptInfo2" )
            });
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();
            #endregion
            #region header 2
            foreach (var item in ReceiptGridInfo)
            {
                var width = item.DisplayWidth;
                var name = ConvertReceiptColumnName(item.ColumnName);

                if (string.IsNullOrEmpty(name)) continue;

                if (name == "AssignmentFlag")
                    builder.Items.Add(
                        new CellSetting(height, 40, "CheckBox", caption: "消" )
                    );
                else
                    builder.Items.Add(
                        new CellSetting(height, item.DisplayWidth, name, caption: item.ColumnNameJp, sortable: true )
                    );
            }
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();
            #endregion
            #region row
            foreach (var item in ReceiptGridInfo)
            {
                var width = item.DisplayWidth;
                var name = ConvertReceiptColumnName(item.ColumnName);

                if (string.IsNullOrEmpty(name)) continue;

                if (name == "AssignmentFlag")
                {
                    var cell = new CellSetting(height, 40, "CheckBox", cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false);
                    if (IsMatched)
                    {
                        cell.Enabled = false;
                        cell.Value = true;
                    }
                    else
                    {
                        cell.DataField = nameof(Receipt.Checked);
                    }
                    builder.Items.Add(cell);
                }
                else
                {
                    var cell = new CellSetting(height, item.DisplayWidth, name, dataField: name);
                    if (name == "RecordedAt"
                     || name == "BillDrawAt"
                     || name == "DueAt")
                    {
                        cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                    }
                    if (name == "ReceiptAmount"
                     || name == "RemainAmount"
                     || name == "AssignmentAmount")
                    {
                        cell.CellInstance = builder.GetTextBoxCurrencyCell(NoOfPrecision);
                    }
                    if (name == "BankCode"
                     || name == "BranchCode"
                     || name == "SectionCode"
                     || name == "CustomerCode"
                     || name == "NettingState")
                    {
                        cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter);
                    }
                    builder.Items.Add(cell);
                }
            }
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 0, nameof(Receipt.Id)               , dataField: nameof(Receipt.Id)),
                new CellSetting(height, 0, nameof(Receipt.ReceiptHeaderId)  , dataField: nameof(Receipt.ReceiptHeaderId)),
            });
            #endregion
            builder.BuildRowOnly(template);
            grdReceipt.Template = template;
            grdReceipt.RowsDefaultCellStyle.BackColor = ColorContext.ControlEnableBackColor;
            grdReceipt.RowsDefaultCellStyle.SelectionBackColor = ColorContext.MatchingGridReceiptSelectedCellBackColor;
            grdReceipt.RowsDefaultCellStyle.SelectionForeColor = ColorContext.ControlForeColor;
            grdReceipt.CurrentCellBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);
            grdReceipt.CurrentRowBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);
            grdReceipt.AllowAutoExtend = false;
        }

        private string ConvertReceiptColumnName(string name)
        {
            return name == "ReceiptCategoryName" ? nameof(Receipt.CategoryName)
                 : name == "TargetAmount" ? (IsMatched ? nameof(Receipt.AssignmentAmount) : "")
                 : name == "Memo" ? nameof(Receipt.ReceiptMemo)
                 : name == "VirtualBranchCode" ? nameof(Receipt.PayerCodePrefix)
                 : name == "VirtualAccountNumber" ? nameof(Receipt.PayerCodeSuffix)
                 : ((name == "SectionCode" || name == "SectionName") && !UseSection) ? ""
                 : name;
        }

        private void InitializeBillingGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var template = new Template();
            var gridCount = BillingGridInfo.Count;
            #region header 1
            var totalwidth = 40;
            var titleWidth = 40;
            foreach (var item in BillingGridInfo)
            {
                totalwidth += item.DisplayWidth;
                if (item.ColumnName == "CustomerCode"
                 || item.ColumnName == "CustomerName")
                {
                    titleWidth += item.DisplayWidth;
                }
            }
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, titleWidth, "BillingInfo", caption: "請求情報" ),
                new CellSetting(height, totalwidth - titleWidth, Name = "BillingInfo2" )
            });
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();
            #endregion
            #region header2
            foreach (var item in BillingGridInfo)
            {
                var width = item.DisplayWidth;
                var name = ConvertBillingColumnName(item.ColumnName);

                if (string.IsNullOrEmpty(name)) continue;

                if (name == "AssignmentFlag")
                {
                    builder.Items.Add(new CellSetting(height, 40, "CheckBox", caption: "消") );
                }
                else
                {
                    builder.Items.Add(new CellSetting(height, item.DisplayWidth, name, caption: item.ColumnNameJp, sortable: true));
                }
            }
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();
            #endregion
            #region row
            foreach (var item in BillingGridInfo)
            {
                var width = item.DisplayWidth;
                var name = ConvertBillingColumnName(item.ColumnName);
                if (string.IsNullOrEmpty(name)) continue;

                if (name == "AssignmentFlag")
                {
                    builder.Items.Add(new CellSetting(height, 40, "CheckBox", dataField: nameof(Billing.Checked), cell: builder.GetCheckBoxCell(isBoolType:true), value: true, enabled: !IsMatched, readOnly: IsMatched ));
                }
                else
                {
                    var cell = new CellSetting(height, item.DisplayWidth, name, dataField: name);
                    if (name == "BilledAt" || name == "DueAt" || name == "SalesAt")
                    {
                        cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                    }
                    if (name == "BillingAmount"
                     || name == "RemainAmount"
                     || name == "DiscountAmount"
                     || name == "TargetAmount"
                     || name == "AssignmentAmount")
                    {
                        cell.CellInstance = builder.GetTextBoxCurrencyCell(NoOfPrecision);
                        if (name == nameof(Billing.TargetAmount) && !IsMatched)
                        {
                            cell.CellInstance = builder.GetNumberCellCurrencyInput(NoOfPrecision, NoOfPrecision, 0);
                            cell.ReadOnly = false;
                        }
                    }
                    if (name == "CustomerCode"
                     || name == "ScheduledPaymentKey")
                    {
                        cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter);
                    }
                    builder.Items.Add(cell);
                }
            }

            #endregion
            builder.BuildRowOnly(template);
            grdBilling.Template = template;
            grdBilling.RowsDefaultCellStyle.BackColor = ColorContext.ControlEnableBackColor;
            grdBilling.RowsDefaultCellStyle.SelectionBackColor = ColorContext.MatchingGridBillingSelectedCellBackColor;
            grdBilling.RowsDefaultCellStyle.SelectionForeColor = ColorContext.ControlForeColor;
            grdBilling.CurrentCellBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);
            grdBilling.CurrentRowBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);
            grdBilling.AllowAutoExtend = false;
        }

        private string ConvertBillingColumnName(string name)
        {
            return name == "DiscountAmountSummary" ? nameof(Billing.DiscountAmount)
                 : name == "MatchingAmount" ? (IsMatched ? nameof(Billing.AssignmentAmount) : "")
                 : name == "BillingCategory" ? nameof(Billing.BillingCategoryCodeAndName)
                 : name == "InputType" ? nameof(Billing.InputTypeName)
                 : name == "TargetAmount" && IsMatched
                     || name == "ScheduledPaymentKey" && !UseScheduledPayment ? ""
                 : name;
        }

        private void SetBillingData()
        {
            if (BillingList != null)
            {
                foreach (var b in BillingList)
                    b.Checked = true;
                Invoke(new System.Action(() =>
                {
                    grdBilling.DataSource = new BindingSource(BillingList, null);
                }));
                if (IsMatched)
                {
                    nmbTransferFee.Value = MatchingHeader.BankTransferFee;
                    var taxDiff = MatchingHeader.TaxDifference;
                    if (taxDiff < 0M)
                        nmbReceiptTaxDifference.Value = taxDiff * -1M;
                    else if (taxDiff > 0M)
                        nmbBillingTaxDifference.Value = taxDiff;
                }
            }
            CalculateAmount(skipReceiptSide: true);
            BillingGridChanged = false;
        }

        private void SetReceiptData()
        {
            if (ReceiptList != null)
            {
                foreach (var receipt in ReceiptList)
                    receipt.Checked = true;
                int receiptCount = ReceiptList.Count;
                Invoke(new System.Action(() =>
                {
                    grdReceipt.DataSource = new BindingSource(ReceiptList, null);
                }));
            }
            CalculateAmount(skipBillingSide: true);
            ReceiptGridChanged = false;
        }

        private MatchingBillingSearch GetBillingSearchCondition()
        {
            var option = new MatchingBillingSearch {
                CompanyId           = CompanyId,
                BillingDataType     = CollationSearch.BillingType,
                DueAtFrom           = CollationSearch.DueAtFrom,
                DueAtTo             = CollationSearch.DueAtTo,
                UseDepartmentWork   = CollationSearch.UseDepartmentWork,
                ClientKey           = CollationSearch.ClientKey,
                UseCashOnDueDates   = ApplicationControl.UseCashOnDueDates,
            };
            if (IsMatched)
            {
                option.CurrencyId = MatchingHeader.CurrencyId;
                option.PaymentAgencyId = MatchingHeader.PaymentAgencyId;
                option.ParentCustomerId = MatchingHeader.CustomerId;
                option.IsParent = GridSelectedParentCustomerIsParent;
                option.MatchingHeaderId = MatchingHeader.Id;
            }
            else
            {
                option.CurrencyId = Collation.CurrencyId;
                option.PaymentAgencyId = Collation.PaymentAgencyId;
                option.ParentCustomerId = Collation.CustomerId;
                option.IsParent = Collation.IsParent;

            }
            return option;
        }

        private MatchingReceiptSearch GetReceiptSearchCondtion()
        {
            var option = new MatchingReceiptSearch();
            option.CompanyId = CompanyId;
            option.RecordedAtFrom = CollationSearch.RecordedAtFrom;
            option.RecordedAtTo = CollationSearch.RecordedAtTo;
            option.BillingDataType = CollationSearch.BillingType;
            option.UseCashOnDueDates = ApplicationControl.UseCashOnDueDates;
            option.UseScheduledPayment = ApplicationControl.UseScheduledPayment;
            option.ClientKey = CollationSearch.ClientKey;
            if (IsMatched)
            {
                option.PaymentAgencyId = MatchingHeader.PaymentAgencyId;
                option.ParentCustomerId = MatchingHeader.CustomerId;
                option.MatchingHeaderId = MatchingHeader.Id;
            }
            else
            {
                option.PaymentAgencyId = Collation.PaymentAgencyId;
                option.ParentCustomerId = Collation.CustomerId;
                option.PayerName = Collation.PayerName ?? string.Empty;
                option.CurrencyId = Collation.CurrencyId;
            }

            return option;
        }

        #endregion

        #region Function Key Event
        [OperationLog("メモ参照")]
        private void ShowMatchingMemo()
        {
            if (!IsMatched
                || MatchingHeader == null) return;

            // todo : MessageBox?
            MessageBox.Show(MatchingHeader.Memo, "消込メモ");
        }

        [OperationLog("請求絞込")]
        private void ShowBillingFilterDialg()
        {
            if (IsMatched) return;

            try
            {
                var filterForm = ApplicationContext.Create(nameof(PE0107));
                var filterScreen = filterForm.GetAll<PE0107>().FirstOrDefault();
                filterScreen.BillingGridValue = BillingGridInfo;
                filterScreen.NoOfPrecision = NoOfPrecision;
                filterScreen.BillingList = ((IEnumerable)grdBilling.DataSource).Cast<Billing>().ToList();

                filterScreen.BillingTaxDifference = nmbBillingTaxDifference.Value;
                var receiptAmtList = new List<KeyValuePair<string, string>>();
                receiptAmtList.Add(new KeyValuePair<string, string>("ReceiptCount", lblReceiptCount.Text));
                receiptAmtList.Add(new KeyValuePair<string, string>("ReceiptAmountTotal", lblReceiptAmountTotal.Text));
                receiptAmtList.Add(new KeyValuePair<string, string>("ReceiptTargetAmount", lblReceiptTargetAmount.Text));
                receiptAmtList.Add(new KeyValuePair<string, string>("ReceiptTaxDifference", nmbReceiptTaxDifference.Value.ToString()));
                receiptAmtList.Add(new KeyValuePair<string, string>("TransferFee", nmbTransferFee.Value.ToString()));
                receiptAmtList.Add(new KeyValuePair<string, string>("CurrencyCode", txtCurrencyCode.Text));

                filterScreen.ReceiptAmtList = receiptAmtList;

                filterForm.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = ApplicationContext.ShowDialog(ParentForm, filterForm);

                var currentRowIndex = grdBilling.CurrentRow?.Index ?? -1;
                if (result == DialogResult.OK)
                {
                    if (filterScreen.FilterBillingList != null)
                    {
                        grdBilling.DataSource = new BindingSource(filterScreen.FilterBillingList, null);
                        CalculateAmount(skipReceiptSide: true);
                        if (Collation?.PaymentAgencyId == 0)
                        {
                            SelectParentCustomer();
                        }
                        if (grdBilling.RowCount > 0 && currentRowIndex >= 0)
                            grdBilling.CurrentCellPosition = new CellPosition(currentRowIndex, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("戻る")]
        private void Close()
        {
            if ((BillingGridChanged || ReceiptGridChanged)
                && !ShowConfirmDialog(MsgQstConfirmClose))
            {
                return;
            }

            if (machedCollationIndices.Any() ||
                CollationSetting.ReloadCollationData == 1 ||
                IsReceiptEdited)
            {
                ParentForm.DialogResult = DialogResult.OK;
            }
            else
            {
                ParentForm.DialogResult = DialogResult.Cancel;
            }
            Settings.SaveControlValue<PE0102>(Login, cbxDispLeftRight.Name, cbxDispLeftRight.Checked);
        }

        [OperationLog("入金入力")]
        private void InputReceipt()
        {
            try
            {
                var remainAmount = 0M;
                if (string.IsNullOrEmpty(lblBillingMatchingRemain.Text)
                    || !decimal.TryParse(lblBillingMatchingRemain.Text, out remainAmount)
                    || remainAmount == 0M)
                {
                    ShowWarningDialog(MsgWngSelectBillingForMatchingAmount);
                    return;
                }

                var form = ApplicationContext.Create(nameof(PD0301));
                var screen = form.GetAll<PD0301>().FirstOrDefault();
                if (screen == null) return;
                screen.ReturnScreen = this;
                screen.InRecordedAtFrom = CollationSearch.RecordedAtFrom;
                screen.InRecordedAtTo = CollationSearch.RecordedAtTo;
                screen.InRemainAmount = remainAmount;
                screen.InCustomerCode = GridSelectedParentCustomerCode;
                screen.InCustomerId = GridSelectedParentCustomerId;
                screen.InClientKey = CollationSearch.ClientKey;
                if (Collation != null && Collation.CurrencyCode != null)
                {
                    screen.InCurrencyCode = Collation.CurrencyCode;
                }
                form.StartPosition = FormStartPosition.CenterParent;
                var result = ApplicationContext.ShowDialog(ParentForm, form);

                if (result != DialogResult.OK) return;

                var currentRowindex = grdReceipt.CurrentRow?.Index ?? -1;

                var receiptIds = screen.OutReceiptId;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<MatchingServiceClient>();
                    var receiptResult = await service.searchReceiptByIdAsync(SessionKey, receiptIds);

                    if (!receiptResult.ProcessResult.Result) return;

                    var receipts = GetReceiptBindings().ToList();
                    foreach (var receipt in receiptResult.Receipts)
                    {
                        if (receipts.Any(x => x.Id == receipt.Id)) continue;
                        receipt.Checked = true;
                        receipts.Add(receipt);
                    }
                    ReceiptList = receipts;

                    grdReceipt.DataSource = new BindingSource(ReceiptList, null);
                    grdReceipt.CurrentCellBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);
                    grdReceipt.CurrentRowBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);

                    CalculateAmount(skipBillingSide: true);
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (grdReceipt.RowCount > 0 && currentRowindex >= 0)
                    grdReceipt.CurrentCellPosition = new CellPosition(currentRowindex, 0);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("請求検索")]
        private void SearchBilling()
        {
            var form = ApplicationContext.Create(nameof(PE0103));
            var screen = form.GetAll<PE0103>().FirstOrDefault();
            screen.Departments = Departments;
            screen.DepartmentIds = DepartmentIds;
            screen.ClientKey = CollationSearch.ClientKey;
            screen.DueAtFrom = CollationSearch.DueAtFrom;
            screen.DueAtTo = CollationSearch.DueAtTo;
            if (!string.IsNullOrEmpty(GridSelectedParentCustomerCode))
            {
                screen.ParentCustomerId = Collation.CustomerId;
            }
            screen.CurrencyCode = Collation.CurrencyCode;

            var billings = GetBillingBindings().ToList();
            screen.BillingId.AddRange(billings.Select(x => x.Id));
            form.StartPosition = FormStartPosition.CenterParent;
            var result = ApplicationContext.ShowDialog(ParentForm, form);

            if (result != DialogResult.OK
                || screen.BillingInfo == null) return;
            var currentRowIndex = grdBilling.CurrentRow?.Index ?? -1;
            var items = screen.BillingInfo;
            foreach (var x in items)
                x.TargetAmountBuffer = x.TargetAmount;
            billings.AddRange(items);
            grdBilling.DataSource = new BindingSource(billings, null);
            grdBilling.CurrentCellBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);
            grdBilling.CurrentRowBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);

            CalculateAmount(skipReceiptSide: true);
            if (grdBilling.RowCount > 0 && currentRowIndex >= 0)
                grdBilling.CurrentCellPosition = new CellPosition(currentRowIndex, 0);
        }

        [OperationLog("入金検索")]
        private void SearchReceipt()
        {
            var form = ApplicationContext.Create(nameof(PE0104));
            var screen = form.GetAll<PE0104>().FirstOrDefault();
            screen.ClientKey = CollationSearch.ClientKey;
            screen.Sections = Sections;
            screen.SectionIds = SectionIds;
            screen.RecordedAtFrom = CollationSearch.RecordedAtFrom;
            screen.RecordedAtTo = CollationSearch.RecordedAtTo;
            screen.CurrencyCode = Collation.CurrencyCode;

            var receipts = GetReceiptBindings().ToList();
            screen.ReceiptId.AddRange(receipts.Select(x => x.Id));
            form.StartPosition = FormStartPosition.CenterParent;
            var result = ApplicationContext.ShowDialog(ParentForm, form);

            if (result != DialogResult.OK
                || screen.ReceiptInfo == null) return;

            var currentRowIndex = grdReceipt.CurrentRow?.Index ?? -1;
            receipts.AddRange(screen.ReceiptInfo);
            grdReceipt.DataSource = new BindingSource(receipts, null);
            grdReceipt.CurrentCellBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);
            grdReceipt.CurrentRowBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);

            CalculateAmount(skipBillingSide: true);

            if (grdReceipt.RowCount > 0 && currentRowIndex >= 0)
                grdReceipt.CurrentCellPosition = new CellPosition(currentRowIndex, 0);
        }

        [OperationLog("消込")]
        private void Matching()
        {
            try
            {
                MatchingInner();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }


        [OperationLog("前へ")]
        private void MovePrevious()
        {
            MoveTo(SelectRowIndex - 1);
        }

        [OperationLog("次へ")]
        private void MoveNext()
        {
            MoveTo(SelectRowIndex + 1);
        }

        /// <summary>
        ///  一括消込画面の 指定した Index への移動
        /// </summary>
        /// <param name="index"></param>
        private bool MoveTo(int index)
        {
            ClearStatusMessage();
            var result = false;
            var itemsCount = CollationGrid?.Rows.Count ?? 0;
            if (itemsCount == 0
                || index < 0
                || itemsCount <= index) return result;
            var indexBuffered = SelectRowIndex;
            try
            {
                var loadTask = new List<Task>();
                var customerId = (int?)null;
                if (IsMatched)
                {
                    MatchingHeader = CollationGrid.Rows[index].DataBoundItem as MatchingHeader;
                    customerId = MatchingHeader?.CustomerId;
                }
                else
                {
                    Collation = CollationGrid.Rows[index].DataBoundItem as Collation;
                    customerId = Collation?.CustomerId;
                }
                SelectRowIndex = index;
                loadTask.Add(LoadBillingsAsync());
                loadTask.Add(LoadReceiptsAsync());

                InitializeForData(Task.WhenAll(loadTask));
                result = true;
            }
            catch (Exception ex)
            {
                SelectRowIndex = indexBuffered;
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return result;
        }

        [OperationLog("エクスポート")]
        private void ExportData()
        {
            try
            {
                var list = new List<ExportMatchingIndividual>();

                var billingList = GetBillingBindings().ToList();
                var receiptList = GetReceiptBindings().ToList();

                if (billingList.Count == 0 && receiptList.Count == 0)
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                var pathTask = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, pathTask, false, SessionKey);
                var serverPath = pathTask.Result;
                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"個別消込データ{DateTime.Now:yyyyMMddHHmmss}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new MatchingIndividualFileDefinition(ApplicationControl);
                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                definition.SetCurrencyFormat(NoOfPrecision);
                var fieldNumber = (UseForeignCurrency) ? 1 : 0;
                definition.SetFieldsSetting(BillingGridInfo, definition.ConvertBillingSettingToFieldDefinition, fieldNumber);
                fieldNumber += BillingGridInfo?.Count ?? 0;
                definition.SetFieldsSetting(ReceiptGridInfo, definition.ConvertReceiptSettingToFieldDefinition, fieldNumber);

                for (int i = 0; i < Math.Max(billingList.Count, receiptList.Count); i++)
                {
                    Billing b = billingList.Count > i ? billingList[i] : null;
                    Receipt r = receiptList.Count > i ? receiptList[i] : null;
                    list.Add(new ExportMatchingIndividual(b, r));
                }
                foreach (ExportMatchingIndividual export in list)
                {
                    int rowcount = list.IndexOf(export);
                    if (rowcount < grdBilling.RowCount && Convert.ToInt32(grdBilling.Rows[rowcount].Cells["celCheckBox"].Value) == 1)
                        export.BillCheck = "レ";
                    if (rowcount < grdReceipt.RowCount && Convert.ToInt32(grdReceipt.Rows[rowcount].Cells["celCheckBox"].Value) == 1)
                        export.RecCheck = "レ";
                    if (UseForeignCurrency)
                    {
                        export.CurrencyCode = txtCurrencyCode.Text;
                    }
                }
                var firstLine = string.Join(exporter.RowDef.Delimiter, definition.Fields.Where(x => !x.Ignored)
                    .Select(x =>
                          x.FieldNumber == definition.BillCheckField.FieldNumber ? "請求情報"
                        : x.FieldNumber == definition.RecCheckField.FieldNumber  ? "入金情報"
                        : "").ToArray());
                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, list, cancel, progress, sb => sb.AppendLine(firstLine));
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                DispStatusMessage(MsgInfFinishExport);
                Settings.SavePath<Web.Models.ExportMatchingIndividual>(Login, filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        [OperationLog("印刷")]
        private void Print()
        {
            try
            {
                var list = new List<Web.Models.ExportMatchingIndividual>();
                List<Billing> bList = ((IEnumerable)grdBilling.DataSource).Cast<Billing>().ToList();
                List<Receipt> rList = ((IEnumerable)grdReceipt.DataSource).Cast<Receipt>().ToList();

                for (int i = 0; i < Math.Max(bList.Count, rList.Count); i++)
                {
                    Billing b = bList.Count > i ? bList[i] : null;
                    Receipt r = rList.Count > i ? rList[i] : null;
                    list.Add(new ExportMatchingIndividual(b, r));
                }

                if (!(list?.Any() ?? false))
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }

                SetSearchData();
                var taxBillingDiff = "";
                var taxReceiptDiff = "";
                var bankFee = "";
                var discountTotal = "";
                taxBillingDiff = GetFormattedNumberString(Convert.ToDecimal(nmbBillingTaxDifference.Value), NoOfPrecision);
                taxReceiptDiff = GetFormattedNumberString(Convert.ToDecimal(nmbReceiptTaxDifference.Value), NoOfPrecision);
                bankFee = GetFormattedNumberString(Convert.ToDecimal(nmbTransferFee.Value), NoOfPrecision);
                discountTotal = GetFormattedNumberString(Convert.ToDecimal(nmbDiscount.Value), NoOfPrecision);

                var report = new GrapeCity.ActiveReports.SectionReport();

                if (CollationSetting.BillingReceiptDisplayOrder == 1)
                {
                    var matchingReceiptBillingReport = new MatchingIndividualReceiptBillingSectionReport();
                    matchingReceiptBillingReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    matchingReceiptBillingReport.SetAmountSetting(NoOfPrecision, taxBillingDiff, taxReceiptDiff, bankFee, discountTotal, ApplicationControl.UseDiscount);
                    matchingReceiptBillingReport.Name = "個別消込画面" + DateTime.Now.ToString("yyyyMMdd");
                    matchingReceiptBillingReport.SetPageDataSetting(list, BillingGridInfo, ReceiptGridInfo);
                    matchingReceiptBillingReport.shpBilling.BackColor = ColorContext.MatchingGridBillingBackColor;
                    report = matchingReceiptBillingReport;
                }
                else
                {
                    var matchingBillingReceiptReport = new MatchingIndividualSectionReport();
                    matchingBillingReceiptReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    matchingBillingReceiptReport.SetAmountSetting(NoOfPrecision, taxBillingDiff, taxReceiptDiff, bankFee, discountTotal, ApplicationControl.UseDiscount);
                    matchingBillingReceiptReport.Name = "個別消込画面" + DateTime.Now.ToString("yyyyMMdd");
                    matchingBillingReceiptReport.SetPageDataSetting(list, BillingGridInfo, ReceiptGridInfo);
                    matchingBillingReceiptReport.shpBilling.BackColor = ColorContext.MatchingGridBillingBackColor;
                    report = matchingBillingReceiptReport;
                }

                var reportCondition = new SearchConditionSectionReport();
                reportCondition.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "個別消込画面");
                reportCondition.SetPageDataSetting(Search);
                reportCondition.Name = "個別消込画面" + DateTime.Now.ToString("yyyyMMdd");
                var pathTask = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, Task.WhenAll(pathTask, Task.Run(() =>
                {
                    report.Run(false);
                    reportCondition.SetPageNumber(report.Document.Pages.Count);
                    reportCondition.Run(false);

                })), false, SessionKey);
                var serverPath = pathTask.Result;
                report.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)reportCondition.Document.Pages.Clone());
                ShowDialogPreview(ParentForm, report, serverPath);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        #endregion

        #region event handler

        private void btnEditDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                this.ButtonClicked(btnEditDiscount);
                var items = GetBillingBindings().Where(x => x.Checked).ToArray();
                if (!items.Any())
                {
                    DispStatusMessage(MsgWngSelectionRequired, "請求データ");
                    return;
                }
                var currentRowIndex = grdBilling.CurrentRow?.Index ?? -1;
                var checkIndexDiscount = grdBilling.Rows
                    .Where(x => Convert.ToBoolean(x.Cells["celCheckBox"].Value))
                    .Select(x => x.Index).ToList();

                var result = DialogResult.Cancel;
                using (var form = ApplicationContext.Create(nameof(PE0105)))
                {
                    var screen = form.GetAll<PE0105>().FirstOrDefault();
                    foreach (var billing in items)
                        billing.CurrencyPrecision = NoOfPrecision;
                    screen.billingInfo = items;
                    result = ApplicationContext.ShowDialog(ParentForm, form, true);
                }
                ProgressDialog.Start(ParentForm, LoadBillingsAsync(), false, SessionKey);
                SetBillingData();
                if (checkIndexDiscount.Count != 0)
                {
                    foreach (Row row in grdBilling.Rows)
                        grdBilling.SetValue(row.Index, "celCheckBox", checkIndexDiscount.Contains(row.Index));
                }
                CalculateAmount(skipReceiptSide: true);
                if (grdBilling.RowCount > 0 && currentRowIndex >= 0)
                    grdBilling.CurrentCellPosition = new CellPosition(currentRowIndex, 0);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnBillingClear_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                this.ButtonClicked(btnBillingClear, alternativeText: "請求：" + btnBillingClear.Text);

                if (BillingGridChanged
                    && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                nmbBillingTaxDifference.Value = 0;
                ProgressDialog.Start(ParentForm, LoadBillingsAsync(), false, SessionKey);
                SetBillingData();
                if (Collation?.PaymentAgencyId == 0)
                {
                    SelectParentCustomer();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnReceiptClear_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                this.ButtonClicked(btnReceiptClear, alternativeText: "入金：" + btnReceiptClear.Text);
                if (ReceiptGridChanged
                    && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                nmbTransferFee.Value = 0;
                nmbReceiptTaxDifference.Value = 0;
                ProgressDialog.Start(ParentForm,
                    Task.Run(async () => await LoadReceiptsAsync()), false, SessionKey);
                SetReceiptData();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                this.ButtonClicked(btnSimulation);
                var remainAmount = GetReceiptBindings().Where(x => x.Checked).Sum(x => x.RemainAmount);

                if (grdBilling.Rows.Count == 0
                    || grdReceipt.Rows.Count == 0
                    || remainAmount <= 0)
                {
                    ShowWarningDialog(MsgWngNotExistSearchData);
                    return;
                }

                remainAmount = remainAmount + (nmbReceiptTaxDifference.Value ?? 0M);
                var transferFee = nmbTransferFee.Value ?? 0M;
                remainAmount = remainAmount + transferFee;

                if (!ShowConfirmDialog(MsgQstConfirmMatchingSimulation))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var billings = GetBillingBindings()
                    .OrderBy(x => x.DueAt)
                    .ThenBy(x => x.TargetAmount).ToArray();
                var indices = GetMatchedIndices(billings, remainAmount);
                if (indices == null) return;

                for (var i = 0; i < billings.Length; i++)
                {
                    billings[i].Checked = indices.Contains(i);
                }
                grdBilling.DataSource = new BindingSource(new List<Billing>(billings), "");
                BillingGridChanged = true;

                var cellNameTarget = "celTargetAmount";
                var cellNameDueAt = "celDueAt";
                grdBilling.Sort(new SortItem[]
                {
                    new SortItem(0, SortOrder.Descending),
                    new SortItem(cellNameDueAt, SortOrder.Ascending),
                    new SortItem(cellNameTarget, SortOrder.Ascending)
                });
                CalculateAmount(skipReceiptSide: true);

                if (Collation?.PaymentAgencyId == 0)
                {
                    SelectParentCustomer();
                }
                ShowWarningDialog(MsgInfNotFoundMatchingAmt);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void DisplayLeftRight()
        {
            spcMatching.Orientation = Orientation.Vertical;
            spcMatching.SplitterDistance = spcMatching.Width / 2;
        }

        private void DisplayTopUnder()
        {
            int spcHeight = spcMatching.Height;
            int splitterDistance = CollationSetting.BillingReceiptDisplayOrder == 0 ? spcHeight / 5 * 3 : spcHeight / 5 * 2;
            spcMatching.Orientation = Orientation.Horizontal;
            spcMatching.SplitterDistance = splitterDistance;
        }

        private void btnBillingCheckAll_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                this.ButtonClicked(btnBillingCheckAll, alternativeText: "請求：" + btnBillingCheckAll.Text);
                grdBilling.CellEditedFormattedValueChanged -= grdBilling_CellEditedFormattedValueChanged;
                grdBilling.EndEdit();
                var modified = false;

                for (var i = 0; i < grdBilling.Rows.Count; i++)
                {
                    if (!Convert.ToBoolean(grdBilling.GetEditedFormattedValue(i, "celCheckBox")))
                    {
                        grdBilling.SetValue(i, "celCheckBox", true);
                        modified = true;
                    }
                }
                grdBilling.CellEditedFormattedValueChanged += grdBilling_CellEditedFormattedValueChanged;
                BillingGridChanged |= modified;

                CalculateAmount(skipReceiptSide: true);

                if (Collation?.PaymentAgencyId == 0)
                    SelectParentCustomer();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnBillingUncheckAll_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                this.ButtonClicked(btnBillingUncheckAll, alternativeText: "請求：" + btnBillingUncheckAll.Text);

                grdBilling.CellEditedFormattedValueChanged -= grdBilling_CellEditedFormattedValueChanged;
                grdBilling.EndEdit();
                var modified = false;
                for (int i = 0; i < grdBilling.RowCount; i++)
                {
                    if (Convert.ToBoolean(grdBilling.GetEditedFormattedValue(i, "celCheckBox")))
                    {
                        grdBilling.SetValue(i, "celCheckBox", false);
                        modified = true;
                    }
                }
                grdBilling.CellEditedFormattedValueChanged += grdBilling_CellEditedFormattedValueChanged;
                BillingGridChanged |= modified;

                CalculateAmount(skipReceiptSide: true);
                if (Collation?.PaymentAgencyId == 0)
                    SelectParentCustomer();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnReceiptCheckAll_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            this.ButtonClicked(btnReceiptCheckAll, alternativeText: "入金：" + btnReceiptCheckAll.Text);

            grdReceipt.CellEditedFormattedValueChanged -= grdReceipt_CellEditedFormattedValueChanged;
            grdReceipt.EndEdit();
            var modified = false;
            for (int i = 0; i < grdReceipt.Rows.Count(); i++)
            {
                if (!Convert.ToBoolean(grdReceipt.GetEditedFormattedValue(i, "celCheckBox")))
                {
                    grdReceipt.SetValue(i, "celCheckBox", true);
                    modified = true;
                }
            }
            grdReceipt.CellEditedFormattedValueChanged += grdReceipt_CellEditedFormattedValueChanged;
            ReceiptGridChanged |= modified;

            CalculateAmount(skipBillingSide: true);
        }

        private void btnReceiptUncheckAll_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            this.ButtonClicked(btnReceiptUncheckAll, alternativeText: "入金：" + btnReceiptUncheckAll.Text);

            grdReceipt.CellEditedFormattedValueChanged -= grdReceipt_CellEditedFormattedValueChanged;
            grdReceipt.EndEdit();
            var modified = false;
            for (int i = 0; i < grdReceipt.RowCount; i++)
            {
                if (Convert.ToBoolean(grdReceipt.GetEditedFormattedValue(i, "celCheckBox")))
                {
                    grdReceipt.SetValue(i, "celCheckBox", false);
                    modified = true;
                }
            }
            grdReceipt.CellEditedFormattedValueChanged += grdReceipt_CellEditedFormattedValueChanged;
            ReceiptGridChanged |= modified;

            CalculateAmount(skipBillingSide: true);
        }

        private void nmbBillingTaxDifference_Validated(object sender, CancelEventArgs e)
        {
            CalculateAmount(skipReceiptSide: true);
        }

        private void nmbReceiptTaxDifference_Validated(object sender, EventArgs e)
        {
            CalculateAmount(skipBillingSide: true);
        }

        private void nmbTransferFee_Validated(object sender, EventArgs e)
        {
            CalculateAmount(skipBillingSide: true);
        }

        #endregion

        #region grid event handler (common)

        private void grid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            var grid = sender as Common.Controls.VOneGridControl;
            if (grid == null
                || e.Scope != CellScope.Row
                || e.RowIndex != grid.CurrentRow.Index
                || e.CellName == "celCheckBox" && !grid.CurrentRow["celCheckBox"].Enabled
                ) return;
            var color = (grid.Equals(grdBilling))
                ? ColorContext.MatchingGridBillingSelectedRowBackColor
                : ColorContext.MatchingGridReceiptSelectedRowBackColor;
            e.CellStyle.BackColor = color;
            e.CellStyle.DisabledBackColor = color;
        }

        #endregion

        #region billing grid event handler

        private void grdBilling_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.CellName != "celCheckBox") return;
            grdBilling.EndEdit();
        }

        private void grdBilling_CellValueChanged(object sender, CellEventArgs e)
        {
            CalculateAmount(skipReceiptSide: true);
            if (Collation?.PaymentAgencyId == 0)
                SelectParentCustomer();
        }

        private void grdBilling_CellValidated(object sender, CellEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.RowIndex < 0
                || e.CellName != "celTargetAmount") return;

            var billing = grdBilling.Rows[e.RowIndex].DataBoundItem as Billing;
            if (billing?.Checked ?? false)
            {
                if (billing?.TargetAmount == 0M)
                {
                    ShowWarningDialog(MsgWngNotAllowdTargetAmountIsZero);
                    billing.TargetAmount = billing.TargetAmountBuffer;
                }
                CalculateAmount(skipReceiptSide: true);
            }
        }

        private void grdBilling_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.RowIndex < 0
                || !(e.CellName == "celCustomerCode" || e.CellName == "celCustomerName" || e.CellName == "celMemo")) return;

            if (e.CellName == "celCustomerCode" || e.CellName == "celCustomerName")
            {
                var customerCode = (grdBilling.Rows[e.RowIndex].DataBoundItem as Billing)?.CustomerCode;
                using (var form = ApplicationContext.Create(nameof(PB0501)))
                {
                    var screen = form.GetAll<PB0501>().FirstOrDefault();
                    form.StartPosition = FormStartPosition.CenterParent;
                    screen.CustomerCode = customerCode;
                    screen.ReturnScreen = this;
                    var result = ApplicationContext.ShowDialog(ParentForm, form);

                    //if (result == DialogResult.OK)
                    //{
                    //    // update some flags
                    //}
                }
            }
            else if (e.CellName == "celMemo")
            {
                var id = (grdBilling.Rows[e.RowIndex].DataBoundItem as Billing)?.Id;

                Debug.Assert(id.HasValue);

                using (var form = ApplicationContext.Create(nameof(PH9906)))
                {
                    var screen = form.GetAll<PH9906>().First();
                    screen.Id = id.Value;
                    screen.MemoType = MemoType.BillingMemo;
                    screen.Memo = Convert.ToString(grdBilling.GetValue(e.RowIndex, "celMemo"));
                    screen.InitializeParentForm("請求メモ");

                    var result = ApplicationContext.ShowDialog(ParentForm, form, true);
                    if (result != DialogResult.OK) return;

                    grdBilling.SetValue(e.RowIndex, "celMemo", screen.Memo);
                }
            }
        }

        #endregion

        #region receipt grid event handler

        private void grdReceipt_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.CellName != "celCheckBox") return;
            grdReceipt.EndEdit();
        }

        private void grdReceipt_CellValueChanged(object sender, CellEventArgs e)
        {
            CalculateAmount(skipBillingSide: true);
        }

        private void grdReceipt_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.RowIndex < 0
                || IsMatched) return;

            if (e.CellName == "celExcludeCategoryName")
            {
                var receipt = grdReceipt.CurrentRow.DataBoundItem as Receipt;
                Debug.Assert(receipt != null);
                Debug.Assert(receipt.Id > 0L);

                if (receipt.NettingId > 0) return;

                DialogResult excludeResult = DialogResult.None;
                using (var form = ApplicationContext.Create(nameof(PD0502)))
                {
                    var screen = form.GetAll<PD0502>().FirstOrDefault();
                    if (screen == null) return;
                    screen.CurrentReceipt = receipt;
                    screen.NoOfpre = NoOfPrecision;
                    form.StartPosition = FormStartPosition.CenterParent;
                    excludeResult = ApplicationContext.ShowDialog(ParentForm, form);
                    if (excludeResult != DialogResult.OK) return;
                    IsReceiptEdited = true;
                }
                try
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var client = factory.Create<MatchingServiceClient>();
                        var result = await client.searchReceiptByIdAsync(SessionKey, new long[] { receipt.Id });
                        if (!result.ProcessResult.Result) return;
                        var modified = result.Receipts.FirstOrDefault();
                        if (modified == null) return;

                        var source = GetReceiptBindings().ToList();
                        if (source == null) return;

                        var index = source.FindIndex(x => x.Id == modified.Id);
                        if (index < 0) return;
                        source[index] = modified;

                        ReceiptList = source;
                        grdReceipt.DataSource = new BindingSource(ReceiptList, "");

                        var available = modified.RemainAmount != 0M;
                        foreach (var row in grdReceipt.Rows.Where(x => (x.DataBoundItem as Receipt)?.Id == modified.Id))
                        {
                            row["celCheckBox"].Value = available;
                            row["celCheckBox"].Enabled = available;
                        }
                        CalculateAmount(skipBillingSide: true);
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
            }
            else if (e.CellName == "celReceiptMemo")
            {
                var id = (grdReceipt.Rows[e.RowIndex].DataBoundItem as Receipt)?.Id;

                Debug.Assert(id.HasValue);

                using (var form = ApplicationContext.Create(nameof(PH9906)))
                {
                    var screen = form.GetAll<PH9906>().First();
                    screen.Id = id.Value;
                    screen.MemoType = MemoType.ReceiptMemo;
                    screen.Memo = Convert.ToString(grdReceipt.GetValue(e.RowIndex, "celReceiptMemo"));
                    screen.InitializeParentForm("入金メモ");

                    var result = ApplicationContext.ShowDialog(ParentForm, form, true);
                    if (result != DialogResult.OK) return;
                    grdReceipt.SetValue(e.RowIndex, "celReceiptMemo", screen.Memo);
                }
            }
            else
            {
                try
                {
                    //入金データ修正画面の表示
                    var receipt = grdReceipt.CurrentRow.DataBoundItem as Receipt;
                    Debug.Assert(receipt != null);
                    Debug.Assert(receipt.Id > 0L);

                    if (receipt.NettingId > 0)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "相殺", "修正");
                        return;
                    } 

                    Receipt currentReceipt = null;

                    ServiceProxyFactory.Do<ReceiptService.ReceiptServiceClient>(client =>
                    {
                        var result = client.Get(SessionKey, new long[] { receipt.Id });
                        if (result.ProcessResult.Result
                            && result.Receipts != null
                            && result.Receipts.Any(x => x != null))
                            currentReceipt = result.Receipts.FirstOrDefault();
                    });

                    if (currentReceipt == null)
                        return;

                    if (currentReceipt.InputType != 2)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "入力以外", "修正");
                        return;
                    }

                    if (currentReceipt.AssignmentFlag != 0)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "消込済", "修正");
                        return;
                    }

                    if (currentReceipt.ExcludeFlag == 1)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "対象外", "修正");
                        return;
                    }

                    if (currentReceipt.OutputAt.HasValue)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "仕訳出力済", "修正");
                        return;
                    }

                    if (currentReceipt.OriginalReceiptId.HasValue)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "前受", "修正");
                        return;
                    }

                    var existAdvanceReceived = false;
                    ServiceProxyFactory.Do<ReceiptService.ReceiptServiceClient>(client =>
                    {
                        var result = client.ExistOriginalReceipt(SessionKey, receipt.Id);
                        if (result.ProcessResult.Result)
                        {
                            existAdvanceReceived = result.Exist;
                        }
                    });

                    if (existAdvanceReceived)
                    {
                        ShowWarningDialog(MsgWngSelectDataNotEditable, "前受振替済", "修正");
                        return;
                    }

                    ClearStatusMessage();

                    using (var form = ApplicationContext.Create(nameof(PD0301)))
                    {
                        var screen = form.GetAll<PD0301>().FirstOrDefault();
                        if (screen == null) return;
                        screen.CurrentReceipt = currentReceipt;
                        screen.ReturnScreen = this;
                        screen.InRecordedAtFrom = CollationSearch.RecordedAtFrom;
                        screen.InRecordedAtTo = CollationSearch.RecordedAtTo;
                        screen.InCustomerCode = GridSelectedParentCustomerCode;
                        screen.InCustomerId = GridSelectedParentCustomerId;
                        //screen.InClientKey = CollationSearch.ClientKey;
                        if (Collation != null && Collation.CurrencyCode != null)
                        {
                            screen.InCurrencyCode = Collation.CurrencyCode;
                        }
                        form.StartPosition = FormStartPosition.CenterParent;
                        var result = ApplicationContext.ShowDialog(ParentForm, form);

                        if (result != DialogResult.OK) return;
                        IsReceiptEdited = true;

                        //ProgressDialog.Start(ParentForm, Task.Run(async () => await LoadReceiptList()), false, SessionKey);
                        //SetReceiptData();

                        var currentRowindex = grdReceipt.CurrentRow?.Index ?? -1;
                        var receiptIds = screen.OutReceiptId;
                        var receipts = GetReceiptBindings().ToList();

                        if (screen.IsDeleted)
                        {
                            foreach (var id in receiptIds)
                            {
                                foreach (var r in receipts.Where(x => x.Id == id).ToArray())
                                    receipts.Remove(r);
                            }
                        }
                        else
                        {
                            var task = ServiceProxyFactory.LifeTime(async factory =>
                            {
                                var service = factory.Create<MatchingServiceClient>();
                                var receiptResult = await service.searchReceiptByIdAsync(SessionKey, receiptIds);

                                if (!receiptResult.ProcessResult.Result) return;

                                foreach (var r in receiptResult.Receipts)
                                {
                                    if (currentRowindex >= 0 && receipts[currentRowindex].Id == r.Id)
                                    {
                                        r.Checked = receipts[currentRowindex].Checked;
                                        receipts[currentRowindex] = r;
                                    }
                                }
                            });
                            ProgressDialog.Start(ParentForm, task, false, SessionKey);
                        }

                        ReceiptList = receipts;

                        grdReceipt.DataSource = new BindingSource(ReceiptList, null);
                        grdReceipt.CurrentCellBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);
                        grdReceipt.CurrentRowBorderLine = new Line(LineStyle.None, System.Drawing.Color.Empty);

                        CalculateAmount(skipBillingSide: true);
                        if (grdReceipt.RowCount > 0 && currentRowindex >= 0)
                            grdReceipt.CurrentCellPosition = new CellPosition(currentRowindex, 0);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
            }
        }

        #endregion

        #region matching

        #region matching validation

        /// <summary>消込前 検証処理</summary>
        /// <remarks>
        ///  未選択確認
        ///   請求側
        ///    消込対象額 金額検証
        ///    債権代表者 グループ検証
        ///  正負混在の場合、金額不一致は消込不可
        /// </remarks>
        private bool ValidateForMatching(Dictionary<int, Customer> newChildren, List<CustomerGroup> group)
        {
            grdBilling.EndEdit();
            grdReceipt.EndEdit();

            if (!ValidateBillingDataForMatching()) return false;

            var receipts = GetReceiptBindings().Where(x => x.Checked).ToList();
            if (!receipts.Any())
            {
                ShowWarningDialog(MsgWngNoSelectDataForMatching, " 入金データ");
                return false;
            }

            var billings = GetBillingBindings().Where(x => x.Checked).ToList();

            var isPaymentAgency = Collation.PaymentAgencyId > 0;
            if (!isPaymentAgency
                && !ValidateBillingCustomerGroupForMatching(billings, newChildren, group)) return false;

            return ValidateTargetAmountSign(billings, receipts);
        }

        /// <summary>請求側 検証処理</summary>
        /// <returns></returns>
        private bool ValidateBillingDataForMatching()
        {
            var billingCheckedAny = false;
            for (int i = 0; i < grdBilling.RowCount; i++)
            {
                var doMatching = Convert.ToBoolean(grdBilling.GetEditedFormattedValue(i, "celCheckBox"));
                if (!doMatching) continue;
                billingCheckedAny = true;

                var remain = Convert.ToDecimal(grdBilling.GetEditedFormattedValue(i, "celRemainAmount"));
                var target = Convert.ToDecimal(grdBilling.GetEditedFormattedValue(i, "celTargetAmount"));
                // TODO : discount
                if (target == 0M)
                {
                    grdBilling.CurrentCellPosition = new CellPosition(i, "celTargetAmount");
                    ShowWarningDialog(MsgWngNotAllowdTargetAmountIsZero);
                    return false;
                }
                var arg1 = "";
                if (0M < remain)
                {
                    if (!UseScheduledPayment && remain < target) arg1 = "請求残以下の「消込対象額」";
                    if (target < 0M) arg1 = (!UseForeignCurrency || Collation?.CurrencyCode == DefaultCurrencyCode)
                        ? "1円以上の「消込対象額」" : "1 以上の「消込対象額」";
                }
                else
                {
                    if (!UseScheduledPayment && target < remain) arg1 = " 請求残以上の「消込対象額」";
                    if (0M < target) arg1 = (!UseForeignCurrency || Collation?.CurrencyCode == DefaultCurrencyCode)
                        ? "-1円以下の「消込対象額」" : "-1 以下の「消込対象額」";
                }
                if (!string.IsNullOrEmpty(arg1))
                {
                    grdBilling.CurrentCellPosition = new CellPosition(i, "celTargetAmount");
                    ShowWarningDialog(MsgWngInputRequired, arg1);
                    return false;
                }
            }
            if (!billingCheckedAny)
            {
                ShowWarningDialog(MsgWngNoSelectDataForMatching, "請求データ");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 選択された請求データと、債権代表者グループの検証処理
        /// 選択された債権代表者(単独得意先) と 他の得意先コードの整合性検証
        /// 下記の場合に検証処理成功とする
        /// 1. 親得意先が選択されている場合 他の得意先が、
        ///   -a 親のグループに所属している
        ///   -b どのグループにも属していない 単独得意先
        /// 2. 子得意先が選択されている場合 他の得意先が、
        ///   -b どのグループにも属していない 単独得意先
        /// 上記 2. の場合は、債権代表者を選択するダイアログを表示し、
        /// 子の得意先を債権代表者へと変更する必用がある
        /// </summary>
        /// <param name="billings">選択された <see cref="Billing"/>一覧</param>
        /// <returns></returns>
        /// <remarks>
        /// 選択された 債権代表者グループ を取得
        /// 選択された 請求データ の 得意先ID が 上記 グループに含まれるか
        /// 含まれていない場合、 得意先が 債権代表者 となってはいないか
        /// 他の債権代表者 グループに含まれてはないか
        /// ダイアログ表示後に確認すべき事項は何か
        /// </remarks>
        private bool ValidateBillingCustomerGroupForMatching(List<Billing> billings, Dictionary<int, Customer> newChildren, List<CustomerGroup> customerGroups)
        {
            var valid = false;
            System.Action messaging = null;
            var task = Task.Run(async () =>
            {
                if (GridSelectedParentCustomerIsParent == 1)
                {
                    var result = await GetCustomerGroupByParentId(GridSelectedParentCustomerId);
                    customerGroups.AddRange(result);
                }

                var customers = billings.GroupBy(x => x.CustomerId).Select(x => new Customer
                {
                    Id = x.Key,
                    Code = x.Min(g => g.CustomerCode),
                    Name = x.Min(g => g.CustomerName),
                    ParentCustomerId = x.Min(g => g.ParentCustomerId),
                    IsParent = x.Min(g => g.IsParent)
                });

                foreach (var customer in customers)
                {
                    if (GridSelectedParentCustomerId == customer.Id) continue;

                    if (GridSelectedParentCustomerIsParent == 1
                        && customerGroups.Any(x => x.ChildCustomerId == customer.Id)) continue;

                    if (customer.IsParent == 1
                        || await ExistCustomerGroup(customer.Id))
                    {
                        messaging = () => ShowWarningDialog(MsgWngOtherChildCustomer, customer.Code);
                        return;
                    }
                    if (!newChildren.ContainsKey(customer.Id))
                        newChildren.Add(customer.Id, customer);
                }
                valid = true;
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (!valid)
                messaging?.Invoke();
            return valid;
        }

        /// <summary>請求/入金 の正負符合および合計金額の検証処理</summary>
        /// <param name="billings"></param>
        /// <param name="receipts"></param>
        /// <returns></returns>
        private bool ValidateTargetAmountSign(List<Billing> billings, List<Receipt> receipts)
        {
            var billingAmount = 0M; var isAllNegativeBilling = true;
            foreach (var billing in billings)
            {
                billingAmount += billing.TargetAmount;
                if (billing.TargetAmount > 0M) isAllNegativeBilling = false;
            }
            var receiptAmount = 0M; var isAllNegativeReceipt = true;
            foreach (var receipt in receipts)
            {
                receiptAmount += receipt.RemainAmount;
                if (receipt.RemainAmount > 0M) isAllNegativeReceipt = false;
            }
            var isAllNegative = isAllNegativeBilling && isAllNegativeReceipt;
            var billTaxDiff = nmbBillingTaxDifference.Value ?? 0M;
            var rcptTaxDiff = nmbReceiptTaxDifference.Value ?? 0M;
            var discount = nmbDiscount.Value ?? 0M;
            var bankFee = nmbTransferFee.Value ?? 0M;
            var billingTarget = billingAmount + billTaxDiff - discount;
            var receiptTarget = receiptAmount + rcptTaxDiff + bankFee;
            if (isAllNegative && billingTarget < 0M && receiptTarget < 0M) return true;
            if (billingTarget != receiptTarget && billingTarget < 0M)
            {
                ShowWarningDialog(MsgWngNotAllowedReceiptRemainIncrease);
                return false;
            }
            if (billingTarget != receiptTarget && receiptTarget < 0M)
            {
                ShowWarningDialog(MsgWngNotAllowedBillingRemainIncrease);
                return false;
            }

            return true;
        }

        #endregion

        #region matching confirmation

        /// <summary>
        ///  前受消込時に伝票日付を取得する処理
        /// </summary>
        /// <param name="billings"></param>
        /// <returns></returns>
        private bool ConfirmMatchingRecordedAt(IEnumerable<Billing> billings, IEnumerable<Receipt> receipts, out DateTime? recordedAt)
        {
            var dateType = CollationSetting?.AdvanceReceivedRecordedDateType ?? 0;

            recordedAt
                = dateType == 1 ? DateTime.Today
                : dateType == 2 ? billings.Max(x => x.BilledAt)
                : dateType == 3 ? billings.Max(x => x.SalesAt)
                : dateType == 4 ? billings.Max(x => x.ClosingAt)
                : dateType == 5 ? billings.Max(x => x.DueAt)
                : dateType == 6 ? receipts.Where(x => /*前受*/x.OriginalReceiptId.HasValue).Max(x => x.RecordedAt)
                : (DateTime?)null;

            using (var form = ApplicationContext.Create(nameof(PE0110)))
            {
                var screen = form.GetAll<PE0110>().First();
                screen.AdvanceReceiveSetting = 3;
                screen.AdvanceDat = recordedAt;

                screen.InitializeParentForm("前受金消込処理年月日の設定");

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (dialogResult == DialogResult.OK)
                {
                    ClearStatusMessage();
                    recordedAt = screen.AdvanceReceiveRecordDate;
                    return true;
                }
            }
            return false;
        }

        /// <summary>消込前 債権代表者グループ 確定処理
        /// グループに含まれない 子の得意先をグループに加えるか
        /// グループに属していない異なる得意先の代表者をどれに設定するか
        /// </summary>
        /// <param name="newChildren"></param>
        /// <param name="groupchildCusId"></param>
        /// <returns></returns>
        private bool ConfirmCustomerGroup(Dictionary<int, Customer> newChildren, List<int> groupchildCusId)
        {
            if (GridSelectedParentCustomerIsParent == 1)
            {
                foreach (var key in newChildren.Keys)
                {
                    if (!ShowConfirmDialog(MsgQstConfirmMatchingWithRegisterCustomerGroup, newChildren[key].Code, txtCusName.Text))
                    {
                        DispStatusMessage(MsgInfProcessCanceled);
                        return false;
                    }
                    groupchildCusId.Add(key);
                }
            }

            NewSelectedParentCustomerId = null;
            if (GridSelectedParentCustomerIsParent == 0
                && newChildren.Count > 0)
            {
                if (!newChildren.ContainsKey(GridSelectedParentCustomerId))
                {
                    newChildren.Add(GridSelectedParentCustomerId, new Customer
                    {
                        Id = GridSelectedParentCustomerId,
                        Code = GridSelectedParentCustomerCode,
                        Name = txtCusName.Text,
                        IsParent = 0
                    });
                }
                using (var form = ApplicationContext.Create(nameof(PE0113)))
                {
                    var screen = form.GetAll<PE0113>().First();
                    screen.ComboBoxInitializer = cmb =>
                    {
                        foreach (var customer in newChildren.Values.OrderBy(x => x.Code))
                            cmb.Items.Add(new GrapeCity.Win.Editors.ListItem($"{customer.Code}：{customer.Name}", customer.Id));
                        if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
                    };
                    screen.InitializeParentForm("債権代表者選択");
                    var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);
                    var parentId = 0;
                    if (dialogResult != DialogResult.OK
                        || !int.TryParse(screen.ParentCustomerId, out parentId))
                    {
                        DispStatusMessage(MsgInfCancelProcess, "消込処理");
                        return false;
                    }
                    groupchildCusId.AddRange(newChildren.Values.Where(x => x.Id != parentId).Select(x => x.Id).ToArray());
                    NewSelectedParentCustomerId = parentId;
                }
            }

            return true;
        }

        /// <summary>請求/入金から、消込の組み合わせを取得する処理</summary>
        /// <param name="source"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private MatchingSource SolveMatchingPattern(MatchingSource source, CollationSearch option)
        {
            MatchingSource result = null;
            var task = ServiceProxyFactory.DoAsync<MatchingServiceClient>(async client =>
            {
                var webResult = await client.SolveAsync(SessionKey, source, option);
                if (webResult.ProcessResult.Result)
                    result = webResult.MatchingSource;
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return result;
        }

        /// <summary>消込の実施/ 前受振替の実施を確認する処理</summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private bool ConfirmMatchingDoOrNotWithAdvanceReceived(MatchingSource source)
        {
            var remainAmount = 0M;
            var carryOverAmount = 0M;
            if (source.RemainType == 1)
                remainAmount = Convert.ToDecimal(lblBillingMatchingRemain.Text);
            else if (source.RemainType == 2)
                remainAmount = Convert.ToDecimal(lblReceiptMatchingRemain.Text);
            else if (source.RemainType == 3)
            {
                remainAmount = Convert.ToDecimal(lblReceiptMatchingRemain.Text);
                carryOverAmount = source.Matchings.Last().ReceiptRemain;
            }
            using (var dialog = CreateDialog<dlgMatchingConfirm>())
            {
                dialog.RemainType = source.RemainType;
                dialog.RemainAmount = GetFormattedNumberString(remainAmount, NoOfPrecision);
                dialog.CarryOverAmount = GetFormattedNumberString(carryOverAmount, NoOfPrecision);
                dialog.CustomerInformation = txtCusName.Text;
                var result = ApplicationContext.ShowDialog(ParentForm, dialog, true);
                if (result == DialogResult.Cancel
                    || result == DialogResult.No && source.RemainType != 3)
                {
                    DispStatusMessage(MsgInfCancelProcess, "消込処理");
                    return false;
                }
                if (source.RemainType == 3 && result == DialogResult.No)
                    source.RemainType = 2;
                source.MatchingHeader.Memo = dialog.MatchingMemo;
            }
            return true;
        }

        /// <summary>前受振替時、得意先の選択処理</summary>
        /// <param name="source"></param>
        /// <param name="groups"></param>
        /// <param name="newChildren"></param>
        /// <returns></returns>
        private bool ConfirmAdvanceReceivedCustomerId(MatchingSource source,
            List<CustomerGroup> groups,
            Dictionary<int, Customer> newChildren)
        {
            var customerIds = groups.Select(x => x.ChildCustomerId).Distinct()
                .Concat(groups.Select(x => x.ParentCustomerId).Distinct())
                .Concat(newChildren.Keys.Distinct())
                .Concat(new int[] { GridSelectedParentCustomerId }).Distinct().ToArray();
            if (customerIds.Length == 1)
            {
                source.AdvanceReceivedCustomerId = customerIds.First();
                return true;
            }
            using (var form = ApplicationContext.Create(nameof(PE0109)))
            {
                var screen = form.GetAll<PE0109>().First();
                screen.CustomerCode = GridSelectedParentCustomerCode;
                screen.CustomerId = GridSelectedParentCustomerId;
                screen.CustomerIdList = customerIds;

                screen.InitializeParentForm("得意先コード指定");

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (dialogResult == DialogResult.OK)
                {
                    ClearStatusMessage();
                    source.AdvanceReceivedCustomerId = screen.AdvancedCustomerId;
                }
                else
                {
                    DispStatusMessage(MsgInfCancelProcess, "消込処理");
                    return false;
                }
            }
            return true;
        }

        #endregion

        /// <summary>消込データ登録処理</summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private async Task<MatchingResult> SaveMatchingAsync(MatchingSource source)
        {
            MatchingResult result = null;
            try
            {
                int? customerId = NewSelectedParentCustomerId ?? GridSelectedParentCustomerId;
                if (customerId == 0) customerId = null;
                int? paymentAgencyId = Collation.PaymentAgencyId;
                if (paymentAgencyId == 0) paymentAgencyId = null;
                source.CustomerId = customerId;
                source.PaymentAgencyId = paymentAgencyId;

                if (customerId.HasValue)
                {
                    var customer = await GetCustomerAsync(customerId.Value);
                    source.UseKanaLearning = customer?.UseKanaLearning ?? 0;
                    source.UseFeeLearning = customer?.UseFeeLearning ?? 0;
                }
                if (paymentAgencyId.HasValue)
                {
                    var agency = await GetPaymentAgencyAsync(paymentAgencyId.Value);
                    source.UseKanaLearning = agency?.UseKanaLearning ?? 0;
                    source.UseFeeLearning = agency?.UseFeeLearning ?? 0;
                }

                await ServiceProxyFactory.DoAsync<MatchingServiceClient>(async client
                    => result = await client.MatchingIndividuallyAsync(SessionKey, source));
                var success = result?.ProcessResult.Result ?? false;
                #region post process
                if (success && NettingPostProcessor != null)
                {
                    var syncResult = true;
                    if (success && result.NettingReceipts.Any())
                        syncResult = NettingPostProcessor.Invoke(result.NettingReceipts.Select(x => x as ITransactionData));
                    success &= syncResult;
                    if (!syncResult && result != null)
                        result.MatchingErrorType = MatchingErrorType.PostProcessError;
                }
                if (success && IsPostProcessorImplemented)
                {
                    var syncResult = true;
                    if (success && AdvanceReceivedPostProcessor != null && result.AdvanceReceiveds.Any())
                    {
                        var advanceReceived = await GetNewReceiptsByIds(result.AdvanceReceiveds.Select(x => x.ReceiptId));
                        if (advanceReceived.Any())
                            syncResult = AdvanceReceivedPostProcessor.Invoke(advanceReceived.Select(x => x as ITransactionData));
                        success &= syncResult;
                    }
                    if (success && MatchingPostProcessor != null && result.Matchings.Any())
                        syncResult = MatchingPostProcessor.Invoke(result.Matchings.Select(x => x as ITransactionData));
                    success &= syncResult;
                    if (!syncResult && result != null)
                        result.MatchingErrorType = MatchingErrorType.PostProcessError;
                }
                #endregion
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
            return result;
        }

        private bool MatchingInner()
        {
            ClearStatusMessage();

            #region validation

            grdBilling.EndEdit();
            grdReceipt.EndEdit();

            if (!ValidateChildren()) return false;

            var newChildren = new Dictionary<int, Customer>();
            var groups = new List<CustomerGroup>();
            if (!ValidateForMatching(newChildren, groups)) return false;

            #endregion

            var receipts = GetReceiptBindings().Where(x => x.Checked).ToList();
            var billings = GetBillingBindings().Where(x => x.Checked).ToList();

            #region confirmation
            var existAdvanceReceived = receipts.Any(x => x.OriginalReceiptId.HasValue);

            DateTime? recordedAt = null;
            if (existAdvanceReceived
                && !ConfirmMatchingRecordedAt(billings, receipts, out recordedAt))
            {
                DispStatusMessage(MsgInfCancelProcess, "消込処理");
                return false;
            }
            var groupchildCusId = new List<int>();
            if (!ConfirmCustomerGroup(newChildren, groupchildCusId)) return false;

            var receiptTaxDifference = nmbReceiptTaxDifference.Value ?? 0M;
            var billingTaxDifference = nmbBillingTaxDifference.Value ?? 0M;
            var taxDiff = billingTaxDifference - receiptTaxDifference;
            // web call solve matching async
            var requestSource = new MatchingSource
            {
                Billings = billings,
                Receipts = receipts,
                BankTransferFee = nmbTransferFee.Value ?? 0M,
                TaxDifference = taxDiff,
            };
            var option = CollationSearch;
            option.AdvanceReceivedRecordedAt = recordedAt;
            option.DoTransferAdvanceReceived = cbxAdvanceReceived.Checked;
            option.UseAdvanceReceived = CollationSetting.UseAdvanceReceived == 1 && Collation.ParentCustomerId > 0;
            var source = SolveMatchingPattern(requestSource, option);
            if (source == null || source.Matchings == null)
            {
                ShowWarningDialog(MsgErrMatchingError);
                return false;
            }
            source.CompanyId = CompanyId;
            source.ClientKey = option.ClientKey;
            source.MatchingProcessType = 1;
            source.LoginUserId = Login.UserId;
            source.ChildCustomerIds = groupchildCusId;

            if (!ConfirmMatchingDoOrNotWithAdvanceReceived(source)) return false;
            if (source.RemainType == 3
                && !ConfirmAdvanceReceivedCustomerId(source, groups, newChildren)) return false;
            #endregion

            var task = SaveMatchingAsync(source);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            var result = task.Result;

            #region post process
            if (result == null)
            {
                ShowWarningDialog(MsgErrMatchingError);
                return false;
            }
            if (result.MatchingErrorType == MatchingErrorType.BillingRemainChanged)
            {
                ShowWarningDialog(MsgWngIncludeOtherUserMatchedData, "請求");
                return false;
            }
            if (result.MatchingErrorType == MatchingErrorType.BillingDiscountChanged)
            {
                ShowWarningDialog(MsgWngIncludeOtherUserMatchedData, "歩引");
                return false;
            }
            if (result.MatchingErrorType == MatchingErrorType.ReceiptRemainChanged)
            {
                ShowWarningDialog(MsgWngIncludeOtherUserMatchedData, "入金");
                return false;
            }
            if (result.MatchingErrorType == MatchingErrorType.PostProcessError)
            {
                ShowWarningDialog(MsgErrPostProcessFailure);
                return false;
            }

            if (!machedCollationIndices.Contains(SelectRowIndex))
                machedCollationIndices.Add(SelectRowIndex);

            nmbTransferFee.Value = 0;
            nmbBillingTaxDifference.Value = 0;
            nmbReceiptTaxDifference.Value = 0;

            var tasks = new List<Task>();
            tasks.Add(LoadBillingsAsync());
            tasks.Add(LoadReceiptsAsync());
            ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

            SetBillingData();
            SetReceiptData();

            if (Collation?.PaymentAgencyId == 0)
                SelectParentCustomer();
            DispStatusMessage(MsgInfMatchingProcessFinish);
            #endregion

            return true;
        }

        #endregion

        #region simulate

        /// <summary>消込候補となる請求データの組み合わせを探す処理</summary>
        /// <param name="billingInfo">表示中の全請求データ</param>
        /// <param name="SearchValue">消込チェックを付けている入金データの入金残の合計＋手数料＋消費税誤差</param>
        /// <returns>消込候補となる請求データの組み合わせ</returns>
        private List<int> GetMatchedIndices(Billing[] billingInfo, decimal SearchValue)
        {
            List<int> indices = null;
            BillingIndicesResult result = null;
            var task = ServiceProxyFactory.DoAsync<MatchingServiceClient>(async client
                => result = await client.SimulateAsync(SessionKey, billingInfo, SearchValue));

            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (!result.ProcessResult.Result)
            {
                ShowWarningDialog(MsgWngMuchPatternAndNeedReset);
                return null;
            }
            if (result.Indices.Count == 0)
            {
                ShowWarningDialog(MsgWngCannotFoundPairPattern);
                return null;
            }
            indices = result.Indices;
            return indices;
        }

        #endregion

        #region subFunction

        /// <summary>代表得意先設定</summary>
        /// <remarks>
        /// 債権代表者グループが存在しない場合,最も上にある請求データの得意先
        /// 請求データの中に債権代表者グループに含まれる請求データが存在する場合、そのグループの債権代表者
        /// </remarks>
        private void SelectParentCustomer()
        {
            var billings = GetBillingBindings()?.Where(x => x.Checked);
            var first = billings.FirstOrDefault(x => x.IsParent == 1 || x.CustomerCode != x.ParentCustomerCode)
                ?? billings.FirstOrDefault();

            txtCusName.Text = first?.ParentCustomerName ?? string.Empty;
            GridSelectedParentCustomerId = first?.ParentCustomerId ?? 0;
            GridSelectedParentCustomerCode = first?.ParentCustomerCode ?? string.Empty;
            GridSelectedParentCustomerIsParent = (first?.IsParent == 1 || first?.CustomerCode != first?.ParentCustomerCode) ? 1 : 0;
        }

        /// <summary>印刷のため検索条件準備処理</summary>
        private void SetSearchData()
        {
            Search = new List<object>();
            string CusName = txtCusName.Text;
            if (Collation.PaymentAgencyId != 0)
            {
                Search.Add(new SearchData("代表得意先", Collation.PaymentAgencyCode + ":" + Collation.PaymentAgencyName));
            }
            else if (!string.IsNullOrEmpty(GridSelectedParentCustomerCode))
            {
                Search.Add(new SearchData("代表得意先", GridSelectedParentCustomerCode + ":" + CusName));
            }
            else
            {
                Search.Add(new SearchData("代表得意先", "(指定なし)"));

            }


            if (CollationSetting.UseFromToNarrowing == 0)
            {
                    Search.Add(new SearchData("入金日", datReceiptRecordedAtTo.GetPrintValue()));
                    Search.Add(new SearchData("入金予定日", datBillingDueAtTo.GetPrintValue()));
            }
            else
            {
                var waveDash = " ～ ";
                Search.Add(new SearchData("入金日",
                    CollationSearch.RecordedAtFrom.GetPrintValue() + waveDash + datReceiptRecordedAtTo.GetPrintValue()));
                Search.Add(new SearchData("入金予定日",
                    CollationSearch.DueAtFrom.GetPrintValue() + waveDash + datBillingDueAtTo.GetPrintValue()));
            }

            if (ApplicationControl.UseForeignCurrency == 1)
            {
                if (string.IsNullOrEmpty(txtCurrencyCode.Text))
                {
                    Search.Add(new SearchData("通貨コード", "(指定なし)"));
                }
                else
                {
                    Search.Add(new SearchData("通貨コード", txtCurrencyCode.Text));
                }
            }
        }

        private string GetNumberFormat(string displayFieldString, int displayScale, char decimalFormatChar = '0')
        {
            if (0 < displayScale)
            {
                displayFieldString += $".{new string(decimalFormatChar, displayScale)}";
            }
            return displayFieldString;
        }

        private string GetFormattedNumberString(decimal value, int displayScale, char decimalFormatChar = '0')
        {
            var displayFieldString = GetNumberFormat("#,###,###,###,##0", displayScale, decimalFormatChar);
            return value.ToString(displayFieldString);
        }

        #endregion

        #region calculation

        /// <summary>合計金額計算</summary>
        /// <param name="skipBillingSide">請求側合計 を 行わない</param>
        /// <param name="skipReceiptSide">入金側合計 を 行わない</param>
        private void CalculateAmount(bool skipBillingSide = false, bool skipReceiptSide = false)
        {
            if (!skipBillingSide) CalculateBillingTotal();
            if (!skipReceiptSide) CalculateReceiptTotal();
            CalculateMatchingTargetAmount();
        }

        /// <summary>合計計算「請求側」</summary>
        private void CalculateBillingTotal()
        {
            var items = GetBillingBindings();
            var totalCount = items.Count();
            var billingTotal = 0M;
            var targetTotal = 0M;
            var discountTotal = 0M;
            items = items.Where(x => x.Checked);
            var checkedCount = items.Count();
            foreach (var item in items)
            {
                billingTotal += item.BillingAmount;
                targetTotal += (IsMatched ? item.AssignmentAmount : item.TargetAmount);
                discountTotal += item.DiscountAmount;
            }
            lblBillingCount.Text = $"{checkedCount:#,##0} / {totalCount:#,##0}";
            nmbDiscount.Value = discountTotal;
            targetTotal += ((nmbBillingTaxDifference.Value ?? 0M) - (nmbDiscount.Value ?? 0M));
            lblBillingAmountTotal.Text = GetFormattedNumberString(billingTotal, NoOfPrecision);
            lblBillingTargetAmount.Text = GetFormattedNumberString(targetTotal, NoOfPrecision);
        }

        /// <summary>合計計算「入金側」</summary>
        private void CalculateReceiptTotal()
        {
            var items = GetReceiptBindings();
            var totalCount = items.Count();
            var receiptSum = 0M;
            var remainSum = 0M;
            items = items.Where(x => x.Checked);
            var checkedCount = items.Count();
            foreach (var item in items)
            {
                receiptSum += item.ReceiptAmount;
                remainSum += (IsMatched ? item.AssignmentAmount : item.RemainAmount);
            }
            lblReceiptCount.Text = $"{checkedCount:#,##0} / {totalCount:#,##0}";
            remainSum += ((nmbReceiptTaxDifference.Value ?? 0M) + (nmbTransferFee.Value ?? 0M));
            lblReceiptAmountTotal.Text = GetFormattedNumberString(receiptSum, NoOfPrecision);
            lblReceiptTargetAmount.Text = GetFormattedNumberString(remainSum, NoOfPrecision);
        }

        /// <summary>消込予定額計算</summary>
        private void CalculateMatchingTargetAmount()
        {
            var billingTarget = Convert.ToDecimal(lblBillingTargetAmount.Text);
            var receiptTarget = Convert.ToDecimal(lblReceiptTargetAmount.Text);
            var billingAllMinus = GetBillingBindings().All(x => x.TargetAmount < 0M);
            var receiptAllMinus = GetReceiptBindings().All(x => x.RemainAmount < 0M);
            var allMinus = billingAllMinus && receiptAllMinus;

            Func<decimal, decimal, decimal> solver = Math.Min;
            if (allMinus) solver = Math.Max;

            var targetAmount = solver(billingTarget, receiptTarget);

            lblBillingMatchingAmount.Text = GetFormattedNumberString(targetAmount, NoOfPrecision);
            lblReceiptMatchingAmount.Text = GetFormattedNumberString(targetAmount, NoOfPrecision);
            lblReceiptMatchingRemain.Text = GetFormattedNumberString((receiptTarget - targetAmount), NoOfPrecision); //消込予定額
            lblBillingMatchingRemain.Text = GetFormattedNumberString((billingTarget - targetAmount), NoOfPrecision); //消込予定額
        }

        #endregion

        #region web service call

        private async Task LoadBillingGridSettingAsync()
        {
            await ServiceProxyFactory.DoAsync<GridSettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, Login.UserId, GridId.BillingMatchingIndividual);
                if (result.ProcessResult.Result)
                    BillingGridInfo = result.GridSettings;
            });
        }

        private async Task LoadReceiptGridSettingAsync()
        {
            await ServiceProxyFactory.DoAsync<GridSettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, Login.UserId, GridId.ReceiptMatchingIndividual);
                if (result.ProcessResult.Result)
                    ReceiptGridInfo = result.GridSettings;
            });
        }

        private async Task LoadBillingsAsync()
        {
            await ServiceProxyFactory.DoAsync<MatchingServiceClient>(async client =>
            {
                var option = GetBillingSearchCondition();
                var result = await client.SearchBillingDataAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                {
                    BillingList = result.Billings;
                    foreach (var x in BillingList)
                        x.TargetAmountBuffer = x.TargetAmount;
                }
            });
        }

        private async Task LoadReceiptsAsync()
        {
            await ServiceProxyFactory.DoAsync<MatchingServiceClient>(async client =>
            {
                var option = GetReceiptSearchCondtion();
                var result = await client.SearchReceiptDataAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    ReceiptList = result.Receipts;
            });
        }

        private async Task LoadCollationSettingAsync()
        {
            await ServiceProxyFactory.DoAsync<CollationSettingMasterClient>(async client =>
            {
                var result = await client.GetAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    CollationSetting = result.CollationSetting;
            });
        }

        private async Task<bool> ExistCustomerGroup(int childCustomerId)
        {
            var exist = false;
            await ServiceProxyFactory.DoAsync<CustomerGroupMasterClient>(async client =>
            {
                var result = await client.ExistCustomerAsync(SessionKey, childCustomerId);
                exist = result.Exist;
            });
            return exist;
        }

        private async Task<List<CustomerGroup>> GetCustomerGroupByParentId(int parentCustomerId)
        {
            List<CustomerGroup> result = null;
            await ServiceProxyFactory.DoAsync<CustomerGroupMasterClient>(async client =>
            {
                var groupResult = await client.GetByParentAsync(SessionKey, parentCustomerId);
                result = groupResult?.CustomerGroups;
            });
            return result;
        }

        private async Task LoadCurrencyAsync()
        {
            var option = new CurrencySearch { CompanyId = CompanyId };
            await ServiceProxyFactory.DoAsync<CurrencyMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, option);
                if (result.ProcessResult.Result)
                    CurrencyList = result.Currencies;
            });
        }

        private async Task<Customer> GetCustomerAsync(int customerId)
        {
            var result = await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client)
                => await client.GetAsync(SessionKey, new int[] { customerId }));
            if (result.ProcessResult.Result)
                return result.Customers.FirstOrDefault();
            return null;
        }

        private async Task<PaymentAgency> GetPaymentAgencyAsync(int paymentAgencyId)
        {
            var result = await ServiceProxyFactory.DoAsync(async (PaymentAgencyMasterClient client)
                => await client.GetAsync(SessionKey, new int[] { paymentAgencyId }));
            if (result.ProcessResult.Result)
                return result.PaymentAgencies.FirstOrDefault();
            return null;
        }

        private async Task<List<Receipt>> GetNewReceiptsByIds(IEnumerable<long> receiptIds)
        {
            var result = await ServiceProxyFactory.DoAsync(async (ReceiptService.ReceiptServiceClient client)
                => await client.GetAsync(SessionKey, receiptIds.ToArray()));
            if (result.ProcessResult.Result)
                return result.Receipts;
            return new List<Receipt>();
        }

        #endregion

        #region get binding source

        /// <summary>
        ///  グリッドのソート順を考慮して <see cref="Billing"/> の <see cref="IEnumerable{T}"/>を取得
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Billing> GetBillingBindings()
            => GetBindingSource<Billing>(grdBilling);

        /// <summary>
        ///  グリッドのソート順を考慮して <see cref="Receipt"/> の <see cref="IEnumerable{T}"/>を取得
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Receipt> GetReceiptBindings()
            => GetBindingSource<Receipt>(grdReceipt);

        /// <summary>
        ///  グリッドのソート順を考慮して <see cref="IEnumerable{T}"/>を取得
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="grid"></param>
        /// <returns></returns>
        /// <remarks>
        ///  grid.DataSource を直接キャストするより パフォーマンスは悪い
        ///  並び順を考慮して取得するには grid.Rows へのアクセスが必要
        /// </remarks>
        private IEnumerable<TModel> GetBindingSource<TModel>(Common.Controls.VOneGridControl grid)
            where TModel : class
            => grid.Rows.Select(x => x.DataBoundItem as TModel);

        #endregion
    }
}