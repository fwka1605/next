using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GridSettingMasterService;
using Rac.VOne.Common;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections;
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
    /// <summary>入金予定入力</summary>
    public partial class PC0901 : VOneScreenBase
    {
        BillingSearch BillingSearchCondition { get; set; }
        private List<decimal> PaymentList { get; set; } = new List<decimal>();
        private int Precision { get; set; } = 0;
        private int IsParentFlg { get; set; } = 0;
        public int CustomerId { get; set; }
        public int CurrencyId { get; set; }
        public string ParCustomerCode { get; set; }
        public string ParCustomerName { get; set; }
        public int ParCustomerId { get; set; }
        public string ParCurrencyCode { get; set; }
        public int ParCurrencyId { get; set; }
        List<object> SearchListCondition { get; set; }
        private List<GridSetting> GridSettingInfo { get; set; }
        private bool IsChecked(Row row) => Convert.ToBoolean(row[CellName("UpdateFlag")].Value);
        private bool IsGridModified
        { get { return grdBillingSearch.Rows.Any(x => ((x.DataBoundItem as Billing)?.PaymentAmount != PaymentList[x.Index]) || (!IsChecked(x))); } }

        private string CellName(string value) => $"cel{value}";

        public PC0901()
        {
            InitializeComponent();
            grdBillingSearch.SetupShortcutKeys();
            Text = "入金予定入力";
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            tbcPaymentScheduleInput.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcPaymentScheduleInput.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = Close;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
        }


        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("更新");
            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction05Caption("相殺入力");
            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction07Caption("絞込");
            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);

            OnF01ClickHandler = Search;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = UpdateData;
            OnF04ClickHandler = DoPrint;
            OnF05ClickHandler = NettingInput;
            OnF06ClickHandler = Export;
            OnF07ClickHandler = Filter;
            OnF08ClickHandler = CheckAll;
            OnF09ClickHandler = UnCheckAll;
            OnF10ClickHandler = Close;

        }

        [OperationLog("検索")]
        private void Search()
        {
            if (!ValidateChildren()) return;

            ClearStatusMessage();
            if (!ValidateSearchData())
                return;

            if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            try
            {
                BillingSearchCondition = GetSearchCondition();
                SearchBillingData(BillingSearchCondition);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            ClearStatusMessage();
            InitializeFunctionKeys();
            tbcPaymentScheduleInput.SelectedIndex = 0;
            ClearControl();
        }

        [OperationLog("更新")]
        private void UpdateData()
        {
            try
            {
                grdBillingSearch.EndEdit();
                if (!ValidateChildren()) return;
                ClearStatusMessage();
                if (!ValidateUpdate()) return;

                using (var form = CreateDialog<dlgScheduledPaymentKeyInput>())
                {
                    DialogResult dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);
                    if (this.Confirmed(dialogResult) == DialogResult.No)
                    {
                        DispStatusMessage(MsgInfProcessCanceled);
                        return;
                    }
                    UpdateBillingData(form.paymentKey);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateUpdate()
        {
            if (!datPaymentDueDate.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "請求入金予定日");
                datPaymentDueDate.Focus();
                return false;
            }
            if (!grdBillingSearch.Rows.Any(x => IsChecked(x)))
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "更新するデータ");
                return false;
            }
            return true;
        }

        [OperationLog("印刷")]
        private void DoPrint()
        {
            ClearStatusMessage();

            try
            {
                List<Billing> billingList = ((IEnumerable)grdBillingSearch.DataSource).Cast<Billing>().ToList();

                if (billingList.Any())
                {
                    var billReport = new PaymentScheduleInputSectionReport();
                    billReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    billReport.Name = "入金予定リスト" + DateTime.Today.ToString("yyyyMMdd");
                    billReport.SetData(billingList, Precision);

                    var searchConditionReport = new SearchConditionSectionReport();
                    searchConditionReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "入金予定リスト");
                    searchConditionReport.Name = "入金予定リスト" + DateTime.Today.ToString("yyyyMMdd");
                    searchConditionReport.SetPageDataSetting(SearchListCondition);
                    ProgressDialog.Start(ParentForm, Task.Run(() =>
                    {
                        billReport.Run(false);
                        searchConditionReport.SetPageNumber(billReport.Document.Pages.Count);
                        searchConditionReport.Run(false);
                    }), false, SessionKey);

                    billReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchConditionReport.Document.Pages.Clone());

                    ShowDialogPreview(ParentForm, billReport, GetServerPath());
                }
                else
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist, "印刷");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("相殺入力")]
        private void NettingInput()
        {
            ClearStatusMessage();
            var offsetInput = ApplicationContext.Create(nameof(PC0902));

            PC0902 screen = offsetInput.GetAll<PC0902>().FirstOrDefault();
            screen.CurrencyId = ParCurrencyId;
            screen.CustomerId = ParCustomerId;
            screen.CustomerCode = ParCustomerCode;
            screen.CustomerName = ParCustomerName;
            screen.CurrencyCode = ParCurrencyCode;

            offsetInput.StartPosition = FormStartPosition.CenterParent;
            var result = ApplicationContext.ShowDialog(ParentForm, offsetInput);

        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();
                List<Billing> list = grdBillingSearch.Rows.Select(x => x.DataBoundItem as Billing).ToList();
                var serverPath = GetServerPath();

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                var filePath = string.Empty;
                var fileName = $"入金予定リスト{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new PaymentScheduleInputFileDefinition(new DataExpression(ApplicationControl), GridSettingInfo);
                var decimalFormat = "0" + ((Precision == 0) ? string.Empty : "." + new string('0', Precision));

                definition.BillingAmountField.Format = value => value.ToString(decimalFormat);
                definition.RemainAmountField.Format = value => value.ToString(decimalFormat);
                definition.PaymentAmountField.Format = value => value.ToString(decimalFormat);
                definition.OffsetAmountField.Format = value => value.ToString(decimalFormat);
                definition.DiscountAmountSummaryField.Format = value => value.ToString(decimalFormat);

                definition.SetFieldsSetting(GridSettingInfo, definition.ConvertSettingToField);

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, list, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                DispStatusMessage(MsgInfFinishExport);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        [OperationLog("全選択")]
        private void CheckAll()
        {
            ClearStatusMessage();
            this.grdBillingSearch.EndEdit();
            this.grdBillingSearch.SuspendLayout();
            for (int i = 0; i < grdBillingSearch.Rows.Count; i++)
            {
                this.grdBillingSearch.SetValue(i, CellName("UpdateFlag"), 1);
                this.grdBillingSearch.Rows[i][CellName(nameof(Billing.PaymentAmount))].ReadOnly = false;
            }
            this.grdBillingSearch.ResumeLayout();
            ChangeAmountValue();
        }

        [OperationLog("全解除")]
        private void UnCheckAll()
        {
            ClearStatusMessage();
            this.grdBillingSearch.EndEdit();
            this.grdBillingSearch.SuspendLayout();
            for (int i = 0; i < grdBillingSearch.Rows.Count; i++)
            {
                decimal RemainAmount = Convert.ToDecimal(this.grdBillingSearch.GetValue(i, CellName(nameof(Billing.RemainAmount))));
                decimal PaymentAmount = Convert.ToDecimal(PaymentList[i]);
                decimal OffsetAmount = Convert.ToDecimal(this.grdBillingSearch.GetValue(i, CellName(nameof(Billing.OffsetAmount))));

                this.grdBillingSearch.SetValue(i, CellName("UpdateFlag"), 0);
                this.grdBillingSearch.Rows[i][CellName(nameof(Billing.PaymentAmount))].ReadOnly = true;
                this.grdBillingSearch.SetValue(i, CellName(nameof(Billing.PaymentAmount)), PaymentAmount);
                this.grdBillingSearch.SetValue(i, CellName(nameof(Billing.OffsetAmount)), RemainAmount - PaymentAmount);

                ResetGridBackColor(i);
            }
            this.grdBillingSearch.ResumeLayout();
            ChangeAmountValue();
        }

        [OperationLog("終了")]
        private void Close()
        {
            try
            {
                Settings.SaveControlValue<PC0901>(Login, cbxUseReceiptSection.Name, cbxUseReceiptSection.Checked);

                if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClose))
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
        private void ReturnToSearchCondition()
        {
            tbcPaymentScheduleInput.SelectedIndex = 0;
        }

        /// <summary> Initialize Grid Setting</summary>
        private void InitializeGridSetting()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var useCurrency = ApplicationControl.UseForeignCurrency == 1;
            var widthName = UseForeignCurrency ? 115 : 135;
            var widthCcy = UseForeignCurrency ? 40 : 0;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var middleRight = MultiRowContentAlignment.MiddleRight;

            var lineNoCell = new CellSetting(height, 40, "LineNo", cell : builder.GetRowHeaderCell());
            builder.Items.Add(lineNoCell);

            foreach (var gs in GridSettingInfo)
            {
                var cell = new CellSetting(height, gs.DisplayWidth, gs.ColumnName, caption: gs.ColumnNameJp, dataField: gs.ColumnName, sortable: true);

                if (gs.ColumnName == "UpdateFlag")
                {
                    cell.CellInstance = builder.GetCheckBoxCell();
                    cell.ReadOnly = false;
                    cell.Value = 1;
                }

                if (gs.ColumnName == nameof(Billing.InvoiceCode)
                 || gs.ColumnName == nameof(Billing.CustomerCode)
                 || gs.ColumnName == nameof(Billing.DepartmentCode)
                 || gs.ColumnName == nameof(Billing.CurrencyCode)
                 || gs.ColumnName == nameof(Billing.StaffCode))
                {
                    cell.CellInstance = builder.GetTextBoxCell(middleCenter);
                }

                if (gs.ColumnName == nameof(Billing.BilledAt)
                 || gs.ColumnName == nameof(Billing.BillingDueAt) 
                 || gs.ColumnName == nameof(Billing.SalesAt) 
                 || gs.ColumnName == nameof(Billing.ClosingAt))
                {
                    cell.CellInstance = builder.GetDateCell_yyyyMMdd();
                }

                if (gs.ColumnName == nameof(Billing.BillingAmount)
                 || gs.ColumnName == nameof(Billing.RemainAmount)
                 || gs.ColumnName == nameof(Billing.OffsetAmount))
                {
                    cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                }

                if (gs.ColumnName == nameof(Billing.InputType))
                    cell.DataField = nameof(Billing.InputTypeName);

                if (gs.ColumnName == nameof(Billing.PaymentAmount))
                {
                    cell.CellInstance = builder.GetNumberCellCurrencyInput(Precision, Precision, 0);
                    cell.ReadOnly = false;
                }

                if (gs.ColumnName == "BillingId")
                {
                    cell.CellInstance = builder.GetTextBoxCell(middleRight);
                    cell.DataField = nameof(Billing.Id);
                }

                if (gs.ColumnName == "DiscountAmountSummary")
                {
                    cell.DataField = nameof(Billing.DiscountAmount);
                    cell.CellInstance = builder.GetTextBoxCurrencyCell(Precision);
                }

                if (gs.ColumnName == "BillingCategory")
                    cell.DataField = nameof(Billing.BillingCategoryCodeAndName);

                if (gs.ColumnName == "CollectCategory")
                    cell.DataField = nameof(Billing.CollectCategoryCodeAndName);

                builder.Items.Add(cell);
            }

            grdBillingSearch.Template = builder.Build();
            grdBillingSearch.HideSelection = true;
            grdBillingSearch.FreezeLeftCellIndex = 1;
        }

        /// <summary> Display Format For NumberControl</summary>
        private void InitializeNumberControlSetting()
        {
            Precision = ApplicationControl.UseForeignCurrency == 1 ? Precision : 0;
            nmbTotalBillingAmount.DisplayFields.Clear();
            nmbTotalOffsetAmount.DisplayFields.Clear();
            nmbTotalPaymentAmount.DisplayFields.Clear();
            nmbTotalRemainAmount.DisplayFields.Clear();
            nmbTotalBillingAmount.DisplayFields.AddRange(GetCurrencyFormat(displayScale: Precision), "", "", "-", "");
            nmbTotalOffsetAmount.DisplayFields.AddRange(GetCurrencyFormat(displayScale: Precision), "", "", "-", "");
            nmbTotalPaymentAmount.DisplayFields.AddRange(GetCurrencyFormat(displayScale: Precision), "", "", "-", "");
            nmbTotalRemainAmount.DisplayFields.AddRange(GetCurrencyFormat(displayScale: Precision), "", "", "-", "");
        }

        /// <summary>LoadCategory For Combox Box</summary>
        private async Task LoadBillingCategoryCombo()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                CategoriesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, 1, null);
                if (result.ProcessResult.Result)
                {
                    for (int i = 0; i < result.Categories.Count; i++)
                    {
                        cmbBillingCategory.Items.Add(new ListItem(result.Categories[i].Code + " : " + result.Categories[i].Name, result.Categories[i].Id));
                    }
                }
            });
        }

        /// <summary>Search Billing Data</summary>
        /// <param name="blSearch">BillingSearch Condition</param>
        private void SearchBillingData(BillingSearch blSearch)
        {
            blSearch.CompanyId = CompanyId;
            BillingsResult result = null;
            var success = false;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                result = await service.GetItemsAsync(SessionKey, blSearch);
                success = (result?.ProcessResult.Result ?? false)
                                && result.Billings.Any();
            });

            ProgressDialog.Start(ParentForm, Task.WhenAll(task), false, SessionKey);

            if (success)
            {
                List<Billing> billingSearch = result.Billings.ToList();
                InitializeGridSetting();
                InitializeNumberControlSetting();
                tbcPaymentScheduleInput.SelectedIndex = 1;
                grdBillingSearch.DataSource = new BindingSource(result.Billings, null);
                ChangeAmountValue();
                EnableFunctionKeys();
                PaymentList = billingSearch.Select(i => i.PaymentAmount).ToList();
                datPaymentDueDate.Value = billingSearch.Max(x => x.DueAt);
                InitializeFreedomSearchCondition();
            }
            else
            {
                tbcPaymentScheduleInput.SelectedIndex = 0;
                DisableFunctionKeys();
                ClearControl(clearHeader: false);
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
        }
        /// <summary>Update Billing Data</summary>
        /// <param name="schedulePaymentKey">ScheduledPaymentKey Of Billing</param>
        private void UpdateBillingData(string schedulePaymentKey)
        {
            var targets = grdBillingSearch.Rows.Where(x => IsChecked(x))
                .Select(x => {
                    var billing = x.DataBoundItem as Billing;
                    billing.DueAt                 = datPaymentDueDate.Value.Value;
                    billing.UpdateBy              = Login.UserId;
                    billing.ScheduledPaymentKey   = schedulePaymentKey;
                    return billing;
                }).ToList();

            var success = false;
            var task = ServiceProxyFactory.DoAsync(async (BillingServiceClient client) =>
            {
                var result = await client.InputScheduledPaymentAsync(SessionKey, targets.ToArray());
                success = result?.ProcessResult.Result ?? false;
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (success)
            {
                tbcPaymentScheduleInput.SelectedIndex = 0;
                DisableFunctionKeys();
                ClearControl(clearHeader: false);
                InitializeSetting();
                DispStatusMessage(MsgInfUpdateSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrUpdateError);
            }

        }

        /// <summary>For Getting Search Condtion</summary>
        private BillingSearch GetSearchCondition()
        {
            var billSearch = new BillingSearch();
            billSearch.CompanyId = CompanyId;
            billSearch.LoginUserId = Login.UserId;
            if (!string.IsNullOrWhiteSpace(txtCustomerCode.Text))
            {
                billSearch.BsCustomerCode = txtCustomerCode.Text;
                ParCustomerCode = txtCustomerCode.Text;
                ParCustomerName = lblCustomerName1.Text;
                ParCustomerId = CustomerId;
                billSearch.IsParentFlg = IsParentFlg;
                billSearch.CustomerId = CustomerId;
            }
            if (datBillDateFrom.Value.HasValue)
                billSearch.BsBilledAtFrom = datBillDateFrom.Value.Value;

            if (datBillDateTo.Value.HasValue)
                billSearch.BsBilledAtTo = datBillDateTo.Value.Value;

            if (datPaymentDateFrom.Value.HasValue)
                billSearch.BsDueAtFrom = datPaymentDateFrom.Value.Value;

            if (datPaymentDateTo.Value.HasValue)
                billSearch.BsDueAtTo = datPaymentDateTo.Value.Value;

            if (!string.IsNullOrWhiteSpace(txtPaymentCodeKey.Text))
                billSearch.BsScheduledPaymentKey = txtPaymentCodeKey.Text;

            if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                billSearch.BsCurrencyCode = txtCurrencyCode.Text;
                ParCurrencyCode = txtCurrencyCode.Text;
                ParCurrencyId = CurrencyId;
            }
            if (!string.IsNullOrWhiteSpace(txtInvoiceNoForm.Text))
                billSearch.BsInvoiceCodeFrom = txtInvoiceNoForm.Text;

            if (!string.IsNullOrWhiteSpace(txtInvoiceNoTo.Text))
                billSearch.BsInvoiceCodeTo = txtInvoiceNoTo.Text;

            if (!string.IsNullOrWhiteSpace(txtInvoiceNo.Text))
                billSearch.BsInvoiceCode = txtInvoiceNo.Text;

            if (cmbBillingCategory.SelectedItem != null)
                billSearch.BsBillingCategoryId = Convert.ToInt32(cmbBillingCategory.SelectedItem.SubItems[1].Value);


            if (ApplicationControl.UseReceiptSection == 1 && cbxUseReceiptSection.Checked)
            {
                billSearch.UseSectionMaster = true;
            }
            else
            {
                billSearch.UseSectionMaster = false;
            }

            billSearch.AssignmentFlg
                = (int)Rac.VOne.Common.Constants.AssignmentFlagChecked.NoAssignment
                | (int)Rac.VOne.Common.Constants.AssignmentFlagChecked.PartAssignment;

            ReportSearchCondition();
            return billSearch;

        }

        /// <summary>For Validating  Search Data</summary>
        ///<returns>Validating Result</returns>
        private bool ValidateSearchData()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerCode.Text))
            {
                tbcPaymentScheduleInput.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "得意先コード");
                txtCustomerCode.Focus();
                return false;
            }

            if (!datBillDateFrom.ValidateRange(datBillDateTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblBillDate.Text)))
            {
                tbcPaymentScheduleInput.SelectedIndex = 0;
                return false;
            }

            if (!datPaymentDateFrom.ValidateRange(datPaymentDateTo,
               () => ShowWarningDialog(MsgWngInputRangeChecked, lblPaymentDate.Text)))
            {
                tbcPaymentScheduleInput.SelectedIndex = 0;
                return false;
            }

            if (ApplicationControl.UseForeignCurrency == 1 && string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                tbcPaymentScheduleInput.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Focus();
                return false;
            }

            if (!txtInvoiceNoForm.ValidateRange(txtInvoiceNoTo,
              () => ShowWarningDialog(MsgWngInputRangeChecked, lblInvoiceNo.Text)))
            {
                tbcPaymentScheduleInput.SelectedIndex = 0;
                return false;
            }
            return true;

        }

        /// <summary>For Clearing  All Control Io Form</summary>
        private void ClearControl(bool clearHeader = true)
        {
            ClearStatusMessage();
            if (clearHeader)
            {
                txtCustomerCode.Clear();
                lblCustomerName1.Clear();
                datBillDateFrom.Clear();
                datBillDateTo.Clear();
                datPaymentDateFrom.Clear();
                datPaymentDateTo.Clear();
                txtPaymentCodeKey.Clear();
                txtCurrencyCode.Clear();
                txtInvoiceNoForm.Clear();
                txtInvoiceNoTo.Clear();
                txtInvoiceNo.Clear();
                cmbBillingCategory.Clear();
                cmbBillingCategory.SelectedIndex = -1;
            }
            grdBillingSearch.DataSource = null;
            nmbTotalBillingAmount.Clear();
            nmbTotalRemainAmount.Clear();
            nmbTotalPaymentAmount.Clear();
            nmbTotalOffsetAmount.Clear();
            datPaymentDueDate.Value = DateTime.Today;
            InitializeFreedomSearchCondition();
            this.ActiveControl = txtCustomerCode;
            txtCustomerCode.Focus();
        }

        /// <summary>For Enable Function Keys</summary>
        private void EnableFunctionKeys()
        {
            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(true);
            BaseContext.SetFunction04Enabled(true);
            BaseContext.SetFunction05Enabled(true);
            BaseContext.SetFunction06Enabled(true);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction08Enabled(true);
            BaseContext.SetFunction09Enabled(true);
            BaseContext.SetFunction10Enabled(true);

        }

        /// <summary>For Disable Function Keys</summary>
        private void DisableFunctionKeys()
        {
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
        }

        /// <summary>For Changing Total Amount Value</summary>
        private void ChangeAmountValue()
        {
            List<Billing> arrearagesList = grdBillingSearch.Rows.Where(x => x.Visible).Select(x => x.DataBoundItem as Billing).ToList();

            nmbTotalBillingAmount.Value = arrearagesList.Sum(x => x.BillingAmount);
            nmbTotalRemainAmount.Value = arrearagesList.Sum(x => x.RemainAmount);
            nmbTotalPaymentAmount.Value = arrearagesList.Sum(x => x.PaymentAmount);
            nmbTotalOffsetAmount.Value = arrearagesList.Sum(x => x.OffsetAmount);
        }

        /// <summary> Reset For Grid Row Background Color</summary>
        /// <param name="row"> Row Index To Reset Background</param>
        private void ResetGridBackColor(int row)
        {
            this.grdBillingSearch.Rows[row].DefaultCellStyle.BackColor = Color.Empty;
        }

        private void grdBillingSearch_CellClick(object sender, CellEventArgs e)
        {
            if (e.CellName.ToString() == "celUpdateFlag")
            {
                if (this.grdBillingSearch.Rows[e.RowIndex].Cells[e.CellIndex].Value != null && this.grdBillingSearch.Rows[e.RowIndex].Cells[e.CellIndex].Value.ToString() == "1")
                {
                    grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].ReadOnly = true;
                    grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].Value = Convert.ToDecimal(PaymentList[e.RowIndex]);
                    grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.OffsetAmount))].Value = 
                        Convert.ToDecimal(grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.RemainAmount))].Value) - 
                        Convert.ToDecimal(grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].Value);

                    ResetGridBackColor(e.RowIndex);
                }
            }

            //modified total value
            ChangeAmountValue();
        }

        private void btnCustomerCode_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerSearchDialog();
            if (customer != null)
            {
                txtCustomerCode.Text = customer.Code;
                lblCustomerName1.Text = customer.Name;
                CustomerId = customer.Id;
                IsParentFlg = customer.IsParent;
                ClearStatusMessage();
            }
        }

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code;
                CurrencyId = currency.Id;
                Precision = currency.Precision;
                ClearStatusMessage();
            }
        }

        private void PC0901_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync()
                    .ContinueWith(t =>
                    {
                        return LoadGridSetting();
                    })
                    .Unwrap());
                }
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                loadTask.Add(LoadBillingCategoryCombo());
                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SuspendLayout();
                tbcPaymentScheduleInput.SelectedIndex = 1;
                tbcPaymentScheduleInput.SelectedIndex = 0;
                ResumeLayout();
                InitializeSetting();
                InitializeGridSetting();
                SetFreedomSearchCombo();
                ClearControl();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

        }

        /// <summary>For Initializing Control</summary>
        private void InitializeSetting()
        {
            datPaymentDueDate.Value = DateTime.Today;

            Settings.SetCheckBoxValue<PC0901>(Login, cbxUseReceiptSection);

            var expression = new DataExpression(ApplicationControl);

            txtCustomerCode.Format = expression.CustomerCodeFormatString;
            txtCustomerCode.MaxLength = expression.CustomerCodeLength;
            txtCustomerCode.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;

            if (ApplicationControl.UseForeignCurrency == 0)
            {
                txtCurrencyCode.Hide();
                btnCurrencyCode.Hide();
                lblCurrencyCode.Hide();

                int yDifference = txtCurrencyCode.Height + txtCurrencyCode.Margin.Top + txtCurrencyCode.Margin.Bottom;

                lblInvoiceNo.Location = new Point(lblInvoiceNo.Location.X, lblInvoiceNo.Location.Y - yDifference);
                lblInvoiceNoWave.Location = new Point(lblInvoiceNoWave.Location.X, lblInvoiceNoWave.Location.Y - yDifference);
                txtInvoiceNoForm.Location = new Point(txtInvoiceNoForm.Location.X, txtInvoiceNoForm.Location.Y - yDifference);
                txtInvoiceNoTo.Location = new Point(txtInvoiceNoTo.Location.X, txtInvoiceNoTo.Location.Y - yDifference);

                lblInvoice.Location = new Point(lblInvoice.Location.X, lblInvoice.Location.Y - yDifference);
                txtInvoiceNo.Location = new Point(txtInvoiceNo.Location.X, txtInvoiceNo.Location.Y - yDifference);

                lblBillingCategory.Location = new Point(lblBillingCategory.Location.X, lblBillingCategory.Location.Y - yDifference);
                cmbBillingCategory.Location = new Point(cmbBillingCategory.Location.X, cmbBillingCategory.Location.Y - yDifference);
                if (ApplicationControl.UseReceiptSection == 1)
                    cbxUseReceiptSection.Location = new Point(cbxUseReceiptSection.Location.X, cbxUseReceiptSection.Location.Y - yDifference);
            }

            if (ApplicationControl.UseReceiptSection == 0)
            {
                cbxUseReceiptSection.Hide();
            }

            this.ActiveControl = txtCustomerCode;
            txtCustomerCode.Focus();
        }

        private void txtCustomerCode_Validated(object sender, EventArgs e)
        {
            var customerName = "";
            if (!string.IsNullOrWhiteSpace(txtCustomerCode.Text))
            {
                try
                {
                    CustomersResult result = null;
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CustomerMasterClient>();
                        result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCustomerCode.Text });
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (result.Customers.Any())
                    {
                        customerName = result.Customers[0].Name;
                        CustomerId = result.Customers[0].Id;
                        IsParentFlg = result.Customers[0].IsParent;
                        lblCustomerName1.Text = customerName;
                        ClearStatusMessage();
                    }
                    else
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "得意先", txtCustomerCode.Text);
                        txtCustomerCode.Clear();
                        lblCustomerName1.Clear();
                        txtCustomerCode.Focus();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
            }
            else
            {
                txtCustomerCode.Clear();
                lblCustomerName1.Clear();
                ClearStatusMessage();
            }
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            try
            {
                var currencyResult = new Currency();

                if (!string.IsNullOrEmpty(txtCurrencyCode.Text))
                {
                    CurrenciesResult result = null;
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CurrencyMasterClient>();
                        result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });

                        currencyResult = result.Currencies.FirstOrDefault();
                        if (currencyResult != null)
                        {
                            CurrencyId = currencyResult.Id;
                            Precision = currencyResult.Precision;
                            ClearStatusMessage();
                            return;
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (currencyResult == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                        CurrencyId = 0;
                        txtCurrencyCode.Clear();
                        txtCurrencyCode.Focus();
                    }
                }
                else
                {
                    ClearStatusMessage();
                    txtCurrencyCode.Clear();
                    CurrencyId = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #region WebServices

        private async Task LoadGridSetting()
        {
            await ServiceProxyFactory.DoAsync(async (GridSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, Login.UserId, GridId.PaymentScheduleInput);
                if (result.ProcessResult.Result)
                    GridSettingInfo = result.GridSettings;
            });
        }

        /// <summary>Get Server Path </summary>
        /// <returns>Server Path</returns>
        private string GetServerPath()
        {
            var serverPath = string.Empty;
            ProgressDialog.Start(ParentForm, Task.Run(() =>
            {
                serverPath = Util.GetGeneralSettingServerPathAsync(Login).Result;
            }), false, SessionKey);

            return serverPath;
        }

        #endregion

        /// <summary>ReportSearchCondition To Show In Report </summary>
        private void ReportSearchCondition()
        {
            SearchListCondition = new List<object>();

            if (!string.IsNullOrWhiteSpace(txtCustomerCode.Text))
            {
                SearchListCondition.Add(new SearchData("得意先コード", txtCustomerCode.Text + " : " + lblCustomerName1.Text));
            }
            else
            {
                SearchListCondition.Add(new SearchData("得意先コード", "(指定なし)"));
            }

            if (datBillDateFrom.Value.HasValue && datBillDateTo.Value.HasValue)
            {
                SearchListCondition.Add(new SearchData("請求日", datBillDateFrom.Value.Value + " ～ " + datBillDateTo.Value.Value));
            }
            else if (datBillDateFrom.Value.HasValue)
            {
                SearchListCondition.Add(new SearchData("請求日", datBillDateFrom.Value.Value + " ～ (指定なし)"));
            }
            else if (datBillDateTo.Value.HasValue)
            {
                SearchListCondition.Add(new SearchData("請求日", "(指定なし) ～ " + datBillDateTo.Value.Value));
            }
            else
            {
                SearchListCondition.Add(new SearchData("請求日", "(指定なし) ～ (指定なし) "));
            }

            if (datPaymentDateFrom.Value.HasValue && datPaymentDateTo.Value.HasValue)
            {
                SearchListCondition.Add(new SearchData("入金予定日", datPaymentDateFrom.Value.Value + " ～ " + datPaymentDateTo.Value.Value));
            }
            else if (datPaymentDateFrom.Value.HasValue)
            {
                SearchListCondition.Add(new SearchData("入金予定日", datPaymentDateFrom.Value.Value + " ～ (指定なし)"));
            }
            else if (datPaymentDateTo.Value.HasValue)
            {
                SearchListCondition.Add(new SearchData("入金予定日", "(指定なし) ～ " + datPaymentDateTo.Value.Value));
            }
            else
            {
                SearchListCondition.Add(new SearchData("入金予定日", "(指定なし) ～ (指定なし) "));
            }

            if (!string.IsNullOrWhiteSpace(txtPaymentCodeKey.Text))
            {
                SearchListCondition.Add(new SearchData("入金予定キー", txtPaymentCodeKey.Text));
            }
            else
            {
                SearchListCondition.Add(new SearchData("入金予定キー", "(指定なし)"));
            }

            if (ApplicationControl.UseForeignCurrency == 1)
            {
                if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    SearchListCondition.Add(new SearchData("通貨コード", txtCurrencyCode.Text));
                }
                else
                {
                    SearchListCondition.Add(new SearchData("通貨コード", "(指定なし)"));
                }
            }

            if (!string.IsNullOrWhiteSpace(txtInvoiceNoForm.Text) && !string.IsNullOrWhiteSpace(txtInvoiceNoTo.Text))
            {
                SearchListCondition.Add(new SearchData("請求書番号", txtInvoiceNoForm.Text + " ～ " + txtInvoiceNoTo.Text));
            }
            else if (!string.IsNullOrWhiteSpace(txtInvoiceNoForm.Text))
            {
                SearchListCondition.Add(new SearchData("請求書番号", txtInvoiceNoForm.Text + " ～ (指定なし)"));
            }
            else if (!string.IsNullOrWhiteSpace(txtInvoiceNoTo.Text))
            {
                SearchListCondition.Add(new SearchData("請求書番号", "(指定なし) ～ " + txtInvoiceNoTo.Text));
            }
            else
            {
                SearchListCondition.Add(new SearchData("請求書番号", "(指定なし) ～ (指定なし) "));
            }

            if (!string.IsNullOrWhiteSpace(txtInvoiceNo.Text))
            {
                SearchListCondition.Add(new SearchData("請求書番号", txtInvoiceNo.Text));
            }
            else
            {
                SearchListCondition.Add(new SearchData("請求書番号", "(指定なし)"));
            }

            if (cmbBillingCategory.SelectedItem != null)
            {
                SearchListCondition.Add(new SearchData("請求区分", cmbBillingCategory.Text));
            }
            else
            {
                SearchListCondition.Add(new SearchData("請求区分", "(指定なし)"));
            }

            if (ApplicationControl.UseReceiptSection == 1)     //RAC対応済
            {
                if (cbxUseReceiptSection.Checked)
                {
                    SearchListCondition.Add(new SearchData("入金部門対応マスターを使用", "使用"));
                }
                else
                {
                    SearchListCondition.Add(new SearchData("入金部門対応マスターを使用", ""));
                }
            }
        }

        private void grdBillingSearch_CellValueChanged(object sender, CellEventArgs e)
        {
            var style = new CellStyle();
            var columnsCount = this.grdBillingSearch.Columns.Count;
            grdBillingSearch.EndEdit();

            if (e.CellName.ToString() == CellName(nameof(Billing.PaymentAmount))
                && this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName("UpdateFlag")].Value != null
                && this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName("UpdateFlag")].Value.ToString() == "1")
            {
                var newPayment = 0M;
                var oldPayment = 0M;
                var billingAmount = 0M;
                var paymentAmount = 0M;

                billingAmount = Convert.ToDecimal(this.grdBillingSearch.Rows[e.RowIndex][CellName(nameof(Billing.RemainAmount))].Value);
                paymentAmount = Convert.ToDecimal(this.grdBillingSearch.Rows[e.RowIndex][CellName(nameof(Billing.PaymentAmount))].Value);
                var offsetAmount = billingAmount - paymentAmount;

                if ((billingAmount < 0 && paymentAmount < 0) || (billingAmount >= 0 && paymentAmount >= 0))
                {
                    this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.OffsetAmount))].Value = Convert.ToDecimal(offsetAmount);
                }
                else
                {
                    if (PaymentList[e.RowIndex] != paymentAmount)
                    {
                        ShowWarningDialog(MsgWngInvalidValueSignForRemainAmount);
                        this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].Value = 0M;
                        this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.OffsetAmount))].Value = 
                            Convert.ToDecimal(this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.RemainAmount))].Value) - 
                            Convert.ToDecimal(this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].Value);
                    }
                }

                newPayment = Convert.ToDecimal(this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].Value);
                oldPayment = PaymentList[e.RowIndex];
                if (oldPayment != newPayment)
                {
                    this.grdBillingSearch.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
                }
                else
                {
                    ResetGridBackColor(e.RowIndex);
                }
            }
            else if (e.CellName.ToString() == CellName("UpdateFlag"))
            {
                if (this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName("UpdateFlag")].Value != null && this.grdBillingSearch.Rows[e.RowIndex].Cells["celUpdateFlag"].Value.ToString() == "1")
                {
                    this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].ReadOnly = false;
                }
                else
                {
                    this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].ReadOnly = true;
                    this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].Value = Convert.ToDecimal(PaymentList[e.RowIndex]);
                    this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.OffsetAmount))].Value = 
                        Convert.ToDecimal(this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.RemainAmount))].Value) - 
                        Convert.ToDecimal(this.grdBillingSearch.Rows[e.RowIndex].Cells[CellName(nameof(Billing.PaymentAmount))].Value);
                    ResetGridBackColor(e.RowIndex);
                }
            }
            //modified total value
            ChangeAmountValue();
        }

        private void SetFreedomSearchCombo()
        {
            for (var i = 1; i < GridSettingInfo.Count; i++)
            {
                if (GridSettingInfo[i].DisplayWidth == 0 ||
                    GridSettingInfo[i].ColumnName == "CurrencyCode" && !UseForeignCurrency ||
                    GridSettingInfo[i].ColumnName == "DiscountAmountSummary" && !UseDiscount) continue;

                cmbFreedomSearch1.Items.Add(new ListItem(GridSettingInfo[i].ColumnNameJp, GridSettingInfo[i].ColumnName));
                cmbFreedomSearch2.Items.Add(new ListItem(GridSettingInfo[i].ColumnNameJp, GridSettingInfo[i].ColumnName));
                cmbFreedomSearch3.Items.Add(new ListItem(GridSettingInfo[i].ColumnNameJp, GridSettingInfo[i].ColumnName));
            }
        }

        private void InitializeFreedomSearchCondition()
        {
            cmbFreedomSearch1.SelectedIndex = 0;
            cmbFreedomSearch2.SelectedIndex = 0;
            cmbFreedomSearch3.SelectedIndex = 0;
            txtFreedomSearch1.Clear();
            txtFreedomSearch2.Clear();
            txtFreedomSearch3.Clear();

            var isEnable = grdBillingSearch.RowCount > 0;
            cmbFreedomSearch1.Enabled = isEnable;
            cmbFreedomSearch2.Enabled = isEnable;
            cmbFreedomSearch3.Enabled = isEnable;
            txtFreedomSearch1.Enabled = isEnable;
            txtFreedomSearch2.Enabled = isEnable;
            txtFreedomSearch3.Enabled = isEnable;
        }

        [OperationLog("絞込")]
        private void Filter()
        {
            grdBillingSearch.CommitEdit();
            grdBillingSearch.SuspendLayout();

            var currencyCommaFormat = GetCurrencyFormat(displayScale: Precision);
            var currencyNoCommaFormat = GetCurrencyFormat(displayScale: Precision, splitComma: false);

            foreach (Row row in grdBillingSearch.Rows)
            {
                row.Visible = true;
                var columnName = "";
                var searchValue = "";
                for (var i = 0; i <= 2; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (cmbFreedomSearch1.SelectedValue != null && !string.IsNullOrEmpty(txtFreedomSearch1.Text))
                            {
                                columnName = cmbFreedomSearch1.SelectedItem.SubItems[1].Value.ToString();
                                searchValue = txtFreedomSearch1.Text;
                            }
                            else
                                continue;
                            break;
                        case 1:
                            if (cmbFreedomSearch2.SelectedValue != null && !string.IsNullOrEmpty(txtFreedomSearch2.Text))
                            {
                                columnName = cmbFreedomSearch2.SelectedItem.SubItems[1].Value.ToString();
                                searchValue = txtFreedomSearch2.Text;
                            }
                            else
                                continue;
                            break;
                        case 2:
                            if (cmbFreedomSearch3.SelectedValue != null && !string.IsNullOrEmpty(txtFreedomSearch3.Text))
                            {
                                columnName = cmbFreedomSearch3.SelectedItem.SubItems[1].Value.ToString();
                                searchValue = txtFreedomSearch3.Text;
                            }
                            else
                               continue;
                            break;
                    }

                    switch(columnName)
                    {
                        case nameof(Billing.BillingId):
                            if (!row.Cells[CellName(nameof(Billing.BillingId))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.InvoiceCode):
                            if (!row.Cells[CellName(nameof(Billing.InvoiceCode))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.CustomerCode):
                            if (!row.Cells[CellName(nameof(Billing.CustomerCode))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.CustomerName):
                            if (!row.Cells[CellName(nameof(Billing.CustomerName))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.DepartmentCode):
                            if (!row.Cells[CellName(nameof(Billing.DepartmentCode))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.DepartmentName):
                            if (!row.Cells[CellName(nameof(Billing.DepartmentName))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.CurrencyCode):
                            if (!row.Cells[CellName(nameof(Billing.CurrencyCode))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.BillingAmount):
                            if (!(Convert.ToDecimal(row.Cells[CellName(nameof(Billing.BillingAmount))].Value.ToString()).ToString(currencyCommaFormat).Contains(searchValue) ||
                                  Convert.ToDecimal(row.Cells[CellName(nameof(Billing.BillingAmount))].Value.ToString()).ToString(currencyNoCommaFormat).Contains(searchValue)))
                                row.Visible = false;
                            break;
                        case "DiscountAmountSummary":
                            if (!(Convert.ToDecimal(row.Cells[CellName("DiscountAmountSummary")].Value.ToString()).ToString(currencyCommaFormat).Contains(searchValue) ||
                                  Convert.ToDecimal(row.Cells[CellName("DiscountAmountSummary")].Value.ToString()).ToString(currencyNoCommaFormat).Contains(searchValue)))
                                row.Visible = false;
                            break;
                        case nameof(Billing.PaymentAmount):
                            if (!(Convert.ToDecimal(row.Cells[CellName(nameof(Billing.PaymentAmount))].Value.ToString()).ToString(currencyCommaFormat).Contains(searchValue) ||
                                  Convert.ToDecimal(row.Cells[CellName(nameof(Billing.PaymentAmount))].Value.ToString()).ToString(currencyNoCommaFormat).Contains(searchValue)))
                                row.Visible = false;
                            break;
                        case nameof(Billing.OffsetAmount):
                            if (!(Convert.ToDecimal(row.Cells[CellName(nameof(Billing.OffsetAmount))].Value.ToString()).ToString(currencyCommaFormat).Contains(searchValue) ||
                                  Convert.ToDecimal(row.Cells[CellName(nameof(Billing.OffsetAmount))].Value.ToString()).ToString(currencyNoCommaFormat).Contains(searchValue)))
                                row.Visible = false;
                            break;
                        case nameof(Billing.BilledAt):
                            if (!Convert.ToDateTime(row.Cells[CellName(nameof(Billing.BilledAt))].Value).ToString("yyyy/MM/dd").Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.BillingDueAt):
                            if (!Convert.ToDateTime(row.Cells[CellName(nameof(Billing.BillingDueAt))].Value).ToString("yyyy/MM/dd").Contains(searchValue))
                                row.Visible = false;
                            break;
                        case "BillingCategory":
                            if (!row.Cells[CellName("BillingCategory")].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.ScheduledPaymentKey):
                            if (!row.Cells[CellName(nameof(Billing.ScheduledPaymentKey))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.SalesAt):
                            if (!Convert.ToDateTime(row.Cells[CellName(nameof(Billing.SalesAt))].Value).ToString("yyyy/MM/dd").Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.ClosingAt):
                            if (!Convert.ToDateTime(row.Cells[CellName(nameof(Billing.ClosingAt))].Value).ToString("yyyy/MM/dd").Contains(searchValue))
                                row.Visible = false;
                            break;
                        case "CollectCategory":
                            if (!row.Cells[CellName("CollectCategory")].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Note1):
                            if (!row.Cells[CellName(nameof(Billing.Note1))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Note2):
                            if (!row.Cells[CellName(nameof(Billing.Note2))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Note3):
                            if (!row.Cells[CellName(nameof(Billing.Note3))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Note4):
                            if (!row.Cells[CellName(nameof(Billing.Note4))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.StaffCode):
                            if (!row.Cells[CellName(nameof(Billing.StaffCode))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.StaffName):
                            if (!row.Cells[CellName(nameof(Billing.StaffName))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.InputType):
                            if (!row.Cells[CellName(nameof(Billing.InputType))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Memo):
                            if (!row.Cells[CellName(nameof(Billing.Memo))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Note5):
                            if (!row.Cells[CellName(nameof(Billing.Note5))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Note6):
                            if (!row.Cells[CellName(nameof(Billing.Note6))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Note7):
                            if (!row.Cells[CellName(nameof(Billing.Note7))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                        case nameof(Billing.Note8):
                            if (!row.Cells[CellName(nameof(Billing.Note8))].Value.ToString().Contains(searchValue))
                                row.Visible = false;
                            break;
                    }
                }
            }

            grdBillingSearch.ResumeLayout();
            ChangeAmountValue();
        }

        private string GetCurrencyFormat(string displayFieldString = "#,###,###,###,##0", int displayScale = 0, string displayFormatString = "0", bool splitComma = true)
        {
            if (displayScale > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < displayScale; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }
            return splitComma ? displayFieldString : displayFieldString.Replace(",", "");
        }

    }
}
