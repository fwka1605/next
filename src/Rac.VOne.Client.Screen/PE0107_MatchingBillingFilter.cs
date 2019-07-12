using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections;
using System.Collections.Generic;
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
    /// <summary>
    /// 個別消込・請求絞込
    /// </summary>
    public partial class PE0107 : VOneScreenBase
    {
        public List<GridSetting> BillingGridValue { get; set; }
        public int NoOfPrecision { get; set; }
        public List<Billing> BillingList { get; set; } = null;
        public decimal? BillingTaxDifference { get; set; }
        public List<KeyValuePair<string, string>> ReceiptAmtList { get; set; }
        public List<Billing> FilterBillingList { get; set; } = null;
        private List<Billing> FilterList { get; set; } = new List<Billing>();
        private int LoginUserId { get; set; }
        private string CurrencyCode { get; set; }

        public PE0107()
        {
            InitializeComponent();
            grdFilterResult.SetupShortcutKeys();
            Text = "個別消込・請求絞込";
            InitializeHandlers();
        }


        private void InitializeHandlers()
        {
            tbBillingFilter.SelectedIndexChanged += (sender, e) =>
            {
                if (tbBillingFilter.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = Return;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
        }

        #region 初期化処理
        private void PE0107_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var tasks = new List<Task> {
                    LoadApplicationControlAsync(),
                    LoadCompanyAsync(),
                    LoadControlColorAsync(),
                };
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                if (ApplicationControl != null)
                {

                    if (ApplicationControl.UseDiscount == 0)
                    {
                        lblDiscount.Visible = false;
                        nmbDiscount.Visible = false;
                    }
                }
                SetFormat();
                CreateBillingGridTemplate();
                FilterList = BillingList;
                grdFilterResult.DataSource = new BindingSource(FilterList, null);
                tbBillingFilter.SelectedIndex = 0;
                SetAmountData();
                SetComboData();
                SetComboForBillingCategory();
                SetComboForCollectCategory();

                Settings.SetCheckBoxValue<PE0107>(Login, cbxDepartmentCodeTo);
                Settings.SetCheckBoxValue<PE0107>(Login, cbxStaffCodeTo);
                Settings.SetCheckBoxValue<PE0107>(Login, cbxCustomerCodeTo);
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

            BaseContext.SetFunction01Caption("絞込");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Filter;

            BaseContext.SetFunction02Caption("絞込クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = FilterClear;

            BaseContext.SetFunction03Caption("選択確定");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = SelectionConfirm;

            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(true);
            OnF08ClickHandler = AllSelect;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(true);
            OnF09ClickHandler = AllUnSelect;

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Return;
        }

        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);

            txtDepartmentCodeFrom.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentCodeFrom.Format = expression.DepartmentCodeFormatString;
            txtDepartmentCodeFrom.PaddingChar = expression.DepartmentCodePaddingChar;

            txtDepartmentCodeTo.MaxLength = expression.DepartmentCodeLength;
            txtDepartmentCodeTo.Format = expression.DepartmentCodeFormatString;
            txtDepartmentCodeTo.PaddingChar = expression.DepartmentCodePaddingChar;

            txtStaffCodeFrom.MaxLength = expression.StaffCodeLength;
            txtStaffCodeFrom.Format = expression.StaffCodeFormatString;
            txtStaffCodeFrom.PaddingChar = expression.StaffCodePaddingChar;

            txtStaffCodeTo.MaxLength = expression.StaffCodeLength;
            txtStaffCodeTo.Format = expression.StaffCodeFormatString;
            txtStaffCodeTo.PaddingChar = expression.StaffCodePaddingChar;

            txtCustomerCodeFrom.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeFrom.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeFrom.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeFrom.PaddingChar = expression.CustomerCodePaddingChar;

            txtCustomerCodeTo.MaxLength = expression.CustomerCodeLength;
            txtCustomerCodeTo.Format = expression.CustomerCodeFormatString;
            txtCustomerCodeTo.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCodeTo.PaddingChar = expression.CustomerCodePaddingChar;

            txtLoginUserCode.MaxLength = expression.LoginUserCodeLength;
            txtLoginUserCode.Format = expression.LoginUserCodeFormatString;
            txtLoginUserCode.PaddingChar = expression.LoginUserCodePaddingChar;
        }

        private void CreateBillingGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var tmp = new Template();
            var gridCount = BillingGridValue.Count;
            #region header
            foreach (var item in BillingGridValue)
            {
                var width = item.DisplayWidth;
                var name = item.ColumnName;

                if (name == "MatchingAmount") continue;
                if (name == "ScheduledPaymentKey" && !UseScheduledPayment) continue;

                if (name == "DiscountAmountSummary") name = "DiscountAmount";
                if (name == "BillingCategory") name = "BillingCategoryCodeAndName";
                if (name == "InputType") name = "InputTypeName";

                var cell = new CellSetting(height, item.DisplayWidth, name, caption: item.ColumnNameJp, sortable: true);
                if (name == "AssignmentFlag")
                {
                    cell.Width = 40;
                    cell.SortDropDown = false;
                    cell.Caption = "消";
                }
                builder.Items.Add(cell);
            }
            builder.BuildHeaderOnly(tmp);
            builder.Items.Clear();
            #endregion
            #region row
            foreach (var item in BillingGridValue)
            {
                var width = item.DisplayWidth;
                var name = item.ColumnName;
                if (name == "MatchingAmount") continue;
                if (name == "ScheduledPaymentKey" && !UseScheduledPayment) continue;

                if (name == "DiscountAmountSummary") name = "DiscountAmount";
                if (name == "BillingCategory") name = "BillingCategoryCodeAndName";
                if (name == "InputType") name = "InputTypeName";

                var cell = new CellSetting(height, item.DisplayWidth, name, dataField: name);
                if (name == "AssignmentFlag")
                {
                    cell.CellInstance = builder.GetCheckBoxCell(isBoolType: true);
                    cell.Value = true;
                    cell.Width = 40;
                    cell.ReadOnly = false;
                    cell.Name = "CheckBox";
                    cell.DataField = "Checked";
                }
                if (name == "BilledAt" || name == "SalesAt" || name == "DueAt")
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
                }
                if (name == "CustomerCode"
                 || name == "ScheduledPaymentKey")
                {
                    cell.CellInstance = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter);
                }
                builder.Items.Add(cell);
            } //end foreach grid
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 0, "InputTypeId"        , dataField: "InputType"        , visible: false),
                new CellSetting(height, 0, "UpdateBy"           , dataField: "UpdateBy"         , visible: false ),
                new CellSetting(height, 0, "BillingCategoryId"  , dataField: "BillingCategoryId", visible: false ),
                new CellSetting(height, 0, "CollectCategoryId"  , dataField: "CollectCategoryId", visible: false ),
                new CellSetting(height, 0, "DepartmentCode"     , dataField: "DepartmentCode"   , visible: false ),
                new CellSetting(height, 0, "StaffCode"          , dataField: "StaffCode"        , visible: false ),
                new CellSetting(height, 0, "UpdateAt"           , dataField: "UpdateAt"         , visible: false )
            });
            #endregion
            builder.BuildRowOnly(tmp);
            grdFilterResult.Template = tmp;
        }

        private void SetAmountData()
        {
            lblBillingDisplayCount.Text = grdFilterResult.RowCount.ToString("#,##0") + "/" + FilterList.Count.ToString("#,##0");

            nmbBillingTaxDifference.Value = Convert.ToDecimal(GetNumberDoubleValue(Convert.ToDecimal(BillingTaxDifference), NoOfPrecision));

            var receitCount = ReceiptAmtList.First(x => x.Key == "ReceiptCount").Value;
            var receiptAmountTotal = ReceiptAmtList.First(x => x.Key == "ReceiptAmountTotal").Value;
            var receiptTargetAmount = ReceiptAmtList.First(x => x.Key == "ReceiptTargetAmount").Value;
            var receiptTaxDifference = ReceiptAmtList.First(x => x.Key == "ReceiptTaxDifference").Value;
            var transferFee = ReceiptAmtList.First(x => x.Key == "TransferFee").Value;
            var currencyCode = ReceiptAmtList.First(c => c.Key == "CurrencyCode").Value;

            lblReceiptCount.Text = receitCount;
            lblReceiptAmountTotal.Text = receiptAmountTotal;
            lblReceiptTargetAmount.Text = receiptTargetAmount;
            CurrencyCode = currencyCode;

            nmbReceiptTaxDifference.Value = Convert.ToDecimal(GetNumberDoubleValue(Convert.ToDecimal(receiptTaxDifference), NoOfPrecision));
            nmbTransferFee.Value = Convert.ToDecimal(GetNumberDoubleValue(Convert.ToDecimal(transferFee), NoOfPrecision));

            CalculateBillingTotal();
            CalculateMatchingPlanAmount();
        }

        private void SetComboData()
        {
            //入力区分
            cmbInputType.Items.Add(new ListItem("すべて", 0));
            cmbInputType.Items.Add(new ListItem("取込", (int)BillingInputType.Importer));
            cmbInputType.Items.Add(new ListItem("入力", (int)BillingInputType.BillingInput));
            cmbInputType.Items.Add(new ListItem("定期請求", (int)BillingInputType.PeriodicBilling));

            cmbInputType.SelectedIndex = 0;

            //自由検索項目
            for (var i = 1; i < BillingGridValue.Count; i++)
            {
                if (BillingGridValue[i].DisplayWidth != 0)
                {
                    if (BillingGridValue[i].ColumnName == "DiscountAmountSummary" && ApplicationControl.UseDiscount == 0)
                        continue;
                    if (BillingGridValue[i].ColumnName == "ScheduledPaymentKey" && ApplicationControl.UseScheduledPayment == 0)
                        continue;
                    if (BillingGridValue[i].ColumnName == "MatchingAmount")
                        continue;
                    cmbFreedomSearch1.Items.Add(new ListItem(BillingGridValue[i].ColumnNameJp, BillingGridValue[i].ColumnName));
                    cmbFreedomSearch2.Items.Add(new ListItem(BillingGridValue[i].ColumnNameJp, BillingGridValue[i].ColumnName));
                    cmbFreedomSearch3.Items.Add(new ListItem(BillingGridValue[i].ColumnNameJp, BillingGridValue[i].ColumnName));
                }
            }

            //金額範囲
            cmbBillingAmount.Items.Add(new ListItem("請求金額（税込）", 0));
            cmbBillingAmount.Items.Add(new ListItem("請求残", 1));
            cmbBillingAmount.SelectedIndex = 0;
        }

        /// <summary> 請求区分コンボボックス設定 </summary>
        private void SetComboForBillingCategory()
        {
            var BillingCategoryList = GetCategoryForCombo(1);

            if (BillingCategoryList != null)
            {
                cmbBillingCategoryCodeName.Items.Add(new ListItem("すべて", 0));
                foreach (var category in BillingCategoryList)
                {
                    cmbBillingCategoryCodeName.Items.Add(new ListItem(category.Code + ":" + category.Name, category.Id));
                }
                cmbBillingCategoryCodeName.SelectedIndex = 0;
            }
        }

        /// <summary> 回収区分コンボボックス設定 </summary>
        private void SetComboForCollectCategory()
        {
            var CollectCategoryList = GetCategoryForCombo(3);

            if (CollectCategoryList != null)
            {
                cmbCollectCategoryCodeName.Items.Add(new ListItem("すべて", 0));
                foreach (var category in CollectCategoryList)
                {
                    cmbCollectCategoryCodeName.Items.Add(new ListItem(category.Code + ":" + category.Name, category.Id));
                }
                cmbCollectCategoryCodeName.SelectedIndex = 0;
            }
        }
        #endregion

        #region 絞込
        [OperationLog("絞込")]
        private void Filter()
        {
            ClearStatusMessage();

            if (!CheckData()) return;
            tbBillingFilter.SelectedIndex = 1;

            foreach (var row in grdFilterResult.Rows)
            {
                row.Visible = true;

                //請求日
                if (datBilledAtFrom.Value.HasValue && !datBilledAtTo.Value.HasValue && Convert.ToDateTime(row.Cells["celBilledAt"].Value) < datBilledAtFrom.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }
                if (!datBilledAtFrom.Value.HasValue && datBilledAtTo.Value.HasValue && Convert.ToDateTime(row.Cells["celBilledAt"].Value) > datBilledAtTo.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }
                if (datBilledAtFrom.Value.HasValue && datBilledAtTo.Value.HasValue && (Convert.ToDateTime(row.Cells["celBilledAt"].Value) < datBilledAtFrom.Value.Value
                    || Convert.ToDateTime(row.Cells["celBilledAt"].Value) > datBilledAtTo.Value.Value))
                {
                    row.Visible = false;
                    continue;
                }

                //入金日
                if (datDueAtFrom.Value.HasValue && !datDueAtTo.Value.HasValue && Convert.ToDateTime(row.Cells["celDueAt"].Value) < datDueAtFrom.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }
                if (!datDueAtFrom.Value.HasValue && datDueAtTo.Value.HasValue && Convert.ToDateTime(row.Cells["celDueAt"].Value) > datDueAtTo.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }
                if (datDueAtFrom.Value.HasValue && datDueAtTo.Value.HasValue && (Convert.ToDateTime(row.Cells["celDueAt"].Value) < datDueAtFrom.Value.Value
                    || Convert.ToDateTime(row.Cells["celDueAt"].Value) > datDueAtTo.Value.Value))
                {
                    row.Visible = false;
                    continue;
                }

                //最終更新日
                if (datUpdateAtFrom.Value.HasValue && !datUpdateAtTo.Value.HasValue && Convert.ToDateTime(row.Cells["celUpdateAt"].Value) < datUpdateAtFrom.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }
                if (!datUpdateAtFrom.Value.HasValue && datUpdateAtTo.Value.HasValue && Convert.ToDateTime(row.Cells["celUpdateAt"].Value) > datUpdateAtTo.Value?.Date.AddDays(1).AddMilliseconds(-1))
                {
                    row.Visible = false;
                    continue;
                }
                if (datUpdateAtFrom.Value.HasValue && datUpdateAtTo.Value.HasValue && (Convert.ToDateTime(row.Cells["celUpdateAt"].Value) < datUpdateAtFrom.Value.Value
                    || Convert.ToDateTime(row.Cells["celUpdateAt"].Value) > datUpdateAtTo.Value?.Date.AddDays(1).AddMilliseconds(-1)))
                {
                    row.Visible = false;
                    continue;
                }

                //最終更新者
                if (!string.IsNullOrEmpty(txtLoginUserCode.Text) && Convert.ToInt32(row.Cells["celUpdateBy"].Value) != LoginUserId)
                {
                    row.Visible = false;
                    continue;
                }

                //請求区分
                if (cmbBillingCategoryCodeName.SelectedIndex != 0 &&
                    Convert.ToInt32(row.Cells["celBillingCategoryId"].Value) != Convert.ToInt32(cmbBillingCategoryCodeName.SelectedItem.SubItems[1].Value))
                {
                    row.Visible = false;
                    continue;
                }

                //入力区分
                if (Convert.ToInt32(cmbInputType.SelectedItem.SubItems[1].Value) != 0 &&
                    Convert.ToInt32(row.Cells["celInputTypeId"].Value) != Convert.ToInt32(cmbInputType.SelectedItem.SubItems[1].Value))
                {
                    row.Visible = false;
                    continue;
                }

                //回収区分
                if (cmbCollectCategoryCodeName.SelectedIndex != 0 && Convert.ToInt32(row.Cells["celCollectCategoryId"].Value) != Convert.ToInt32(cmbCollectCategoryCodeName.SelectedItem.SubItems[1].Value))
                {
                    row.Visible = false;
                    continue;
                }

                //請求部門コード
                if (!string.IsNullOrEmpty(txtDepartmentCodeFrom.Text) && string.IsNullOrEmpty(txtDepartmentCodeTo.Text)
                    && string.Compare(row.Cells["celDepartmentCode"].Value.ToString(), txtDepartmentCodeFrom.Text) < 0)
                {
                    row.Visible = false;
                    continue;
                }
                if (string.IsNullOrEmpty(txtDepartmentCodeFrom.Text) && !string.IsNullOrEmpty(txtDepartmentCodeTo.Text)
                    && string.Compare(row.Cells["celDepartmentCode"].Value.ToString(), txtDepartmentCodeTo.Text) > 0)
                {
                    row.Visible = false;
                    continue;
                }
                if (!string.IsNullOrEmpty(txtDepartmentCodeFrom.Text) && !string.IsNullOrEmpty(txtDepartmentCodeTo.Text)
                    && (string.Compare(row.Cells["celDepartmentCode"].Value.ToString(), txtDepartmentCodeFrom.Text) < 0 ||
                    string.Compare(row.Cells["celDepartmentCode"].Value.ToString(), txtDepartmentCodeTo.Text) > 0))
                {
                    row.Visible = false;
                    continue;
                }

                //担当者コード
                if (!string.IsNullOrEmpty(txtStaffCodeFrom.Text) && string.IsNullOrEmpty(txtStaffCodeTo.Text)
                    && string.Compare(row.Cells["celStaffCode"].Value.ToString(), txtStaffCodeFrom.Text) < 0)
                {
                    row.Visible = false;
                    continue;
                }
                if (string.IsNullOrEmpty(txtStaffCodeFrom.Text) && !string.IsNullOrEmpty(txtStaffCodeTo.Text)
                    && string.Compare(row.Cells["celStaffCode"].Value.ToString(), txtStaffCodeTo.Text) > 0)
                {
                    row.Visible = false;
                    continue;
                }
                if (!string.IsNullOrEmpty(txtStaffCodeFrom.Text) && !string.IsNullOrEmpty(txtStaffCodeTo.Text)
                    && (string.Compare(row.Cells["celStaffCode"].Value.ToString(), txtStaffCodeFrom.Text) < 0 ||
                    string.Compare(row.Cells["celStaffCode"].Value.ToString(), txtStaffCodeTo.Text) > 0))
                {
                    row.Visible = false;
                    continue;
                }

                //得意先コード
                if (!string.IsNullOrEmpty(txtCustomerCodeFrom.Text) && string.IsNullOrEmpty(txtCustomerCodeTo.Text)
                        && string.Compare(row.Cells["celCustomerCode"].Value.ToString(), txtCustomerCodeFrom.Text) < 0)
                {
                    row.Visible = false;
                    continue;
                }
                if (string.IsNullOrEmpty(txtCustomerCodeFrom.Text) && !string.IsNullOrEmpty(txtCustomerCodeTo.Text)
                    && string.Compare(row.Cells["celCustomerCode"].Value.ToString(), txtCustomerCodeTo.Text) > 0)
                {
                    row.Visible = false;
                    continue;
                }
                if (!string.IsNullOrEmpty(txtCustomerCodeFrom.Text) && !string.IsNullOrEmpty(txtCustomerCodeTo.Text)
                    && (string.Compare(row.Cells["celCustomerCode"].Value.ToString(), txtCustomerCodeFrom.Text) < 0 ||
                    string.Compare(row.Cells["celCustomerCode"].Value.ToString(), txtCustomerCodeTo.Text) > 0))
                {
                    row.Visible = false;
                    continue;
                }

                //金額範囲
                if (cmbBillingAmount.SelectedIndex == 0 && nmbAmountFrom.Value.HasValue && Convert.ToDecimal(row.Cells["celBillingAmount"].Value) < nmbAmountFrom.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }
                if (cmbBillingAmount.SelectedIndex == 0 && nmbAmountTo.Value.HasValue && Convert.ToDecimal(row.Cells["celBillingAmount"].Value) > nmbAmountTo.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }
                if (cmbBillingAmount.SelectedIndex == 1 && nmbAmountFrom.Value.HasValue && Convert.ToDecimal(row.Cells["celRemainAmount"].Value) < nmbAmountFrom.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }
                if (cmbBillingAmount.SelectedIndex == 1 && nmbAmountTo.Value.HasValue && Convert.ToDecimal(row.Cells["celRemainAmount"].Value) > nmbAmountTo.Value.Value)
                {
                    row.Visible = false;
                    continue;
                }

                var columnName = "";
                var searchValue = "";
                foreach (var i in Enumerable.Range(0, 3))
                {
                    Common.Controls.VOneComboControl combo = null;
                    Common.Controls.VOneTextControl text = null;
                    switch (i)
                    {
                        case 0: combo = cmbFreedomSearch1; text = txtFreedomSearch1; break;
                        case 1: combo = cmbFreedomSearch2; text = txtFreedomSearch2; break;
                        case 2: combo = cmbFreedomSearch3; text = txtFreedomSearch3; break;
                    }
                    if (combo == null
                        || text == null
                        || combo.SelectedValue == null
                        || string.IsNullOrEmpty(text.Text)) continue;

                    columnName = combo.SelectedItem.SubItems[1].Value.ToString();
                    searchValue = text.Text;
                    switch (columnName)
                    {
                        case "BilledAt":
                            if (!Convert.ToDateTime(row.Cells["celBilledAt"].Value).ToString("yyyy/MM/dd").Contains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "BillingAmount":
                            if (!row.Cells["celBillingAmount"].Value.ToString().Contains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "BillingCategory":
                            if (!row.Cells["celBillingCategory"].Value.ToString().Contains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "CustomerCode":
                            if (!row.Cells["celCustomerCode"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "CustomerName":
                            if (!row.Cells["celCustomerName"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "DepartmentName":
                            if (!row.Cells["celDepartmentName"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "DiscountAmountSummary":
                            if (!row.Cells["celDiscountAmount"].Value.ToString().Contains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "DueAt":
                            if (!Convert.ToDateTime(row.Cells["celDueAt"].Value).ToString("yyyy/MM/dd").Contains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "InputType":
                            if (!row.Cells["celInputType"].Value.ToString().Contains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "InvoiceCode":
                            if (!row.Cells["celInvoiceCode"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Memo":
                            if ((row.Cells["celMemo"].Value == null) || row.Cells["celMemo"].Value != null && !row.Cells["celMemo"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Note1":
                            if (!row.Cells["celNote1"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Note2":
                            if (!row.Cells["celNote2"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Note3":
                            if (!row.Cells["celNote3"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Note4":
                            if (!row.Cells["celNote4"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Note5":
                            if (!row.Cells["celNote5"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Note6":
                            if (!row.Cells["celNote6"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Note7":
                            if (!row.Cells["celNote7"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "Note8":
                            if (!row.Cells["celNote8"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "RemainAmount":
                            if (!row.Cells["celRemainAmount"].Value.ToString().Contains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "ScheduledPaymentKey":
                            if (!row.Cells["celScheduledPaymentKey"].Value.ToString().RoughlyContains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                        case "TargetAmount":
                            if (!row.Cells["celTargetAmount"].Value.ToString().Contains(searchValue))
                            {
                                row.Visible = false;
                            }
                            break;
                    }
                }
            }
            int count = grdFilterResult.Rows
               .Where(x => Convert.ToBoolean(x["celCustomerCode"].Visible)).Count();
            lblBillingDisplayCount.Text = count.ToString("#,##0") + "/" + FilterList.Count.ToString("#,##0");
        }

        private bool CheckData()
        {
            if (!datBilledAtFrom.ValidateRange(datBilledAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblBilledAt.Text))) return false;

            if (!datDueAtFrom.ValidateRange(datDueAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDueAt.Text))) return false;

            if (!datUpdateAtFrom.ValidateRange(datUpdateAtTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblUpdateAt.Text))) return false;

            if (!txtDepartmentCodeFrom.ValidateRange(txtDepartmentCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblDepartmentCodeFrom.Text))) return false;

            if (!txtStaffCodeFrom.ValidateRange(txtStaffCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblStaffCodeFrom.Text))) return false;

            if (!txtCustomerCodeFrom.ValidateRange(txtCustomerCodeTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, lblCustomerCode.Text))) return false;

            if (!nmbAmountFrom.ValidateRange(nmbAmountTo,
                () => ShowWarningDialog(MsgWngInputRangeChecked, cmbBillingAmount.SelectedValue.ToString()))) return false;

            return true;
        }

        #endregion

        #region 絞込クリア
        [OperationLog("絞込クリア")]
        private void FilterClear()
        {
            ClearStatusMessage();
            datBilledAtFrom.Clear();
            datBilledAtTo.Clear();
            datDueAtFrom.Clear();
            datDueAtTo.Clear();
            datUpdateAtFrom.Clear();
            datUpdateAtTo.Clear();
            txtLoginUserCode.Clear();
            lblLoginUserName.Clear();
            cmbBillingCategoryCodeName.SelectedIndex = 0;
            cmbCollectCategoryCodeName.SelectedIndex = 0;
            cmbInputType.SelectedIndex = 0;
            txtDepartmentCodeFrom.Clear();
            lblDepartmentNameFrom.Clear();
            txtDepartmentCodeTo.Clear();
            lblDepartmentNameTo.Clear();
            txtStaffCodeFrom.Clear();
            lblStaffNameFrom.Clear();
            txtStaffCodeTo.Clear();
            lblStaffNameTo.Clear();
            txtCustomerCodeFrom.Clear();
            lblCustomerCodeFrom.Clear();
            txtCustomerCodeTo.Clear();
            lblCustomerNameTo.Clear();
            cmbBillingAmount.SelectedIndex = 0;
            nmbAmountFrom.Clear();
            nmbAmountTo.Clear();
            cmbFreedomSearch1.SelectedIndex = -1;
            cmbFreedomSearch2.SelectedIndex = -1;
            cmbFreedomSearch3.SelectedIndex = -1;
            txtFreedomSearch1.Clear();
            txtFreedomSearch2.Clear();
            txtFreedomSearch3.Clear();
            grdFilterResult.DataSource = new BindingSource(FilterList, null);
            tbBillingFilter.SelectedIndex = 0;
            SetAmountData();
        }
        #endregion

        #region 全選択
        [OperationLog("全選択")]
        private void AllSelect()
        {
            ClearStatusMessage();
            grdFilterResult.EndEdit();

            foreach (var row in grdFilterResult.Rows.Where(x => x.Visible))
                grdFilterResult.SetValue(row.Index, "celCheckBox", true);

            CalculateBillingTotal();
            CalculateMatchingPlanAmount();
            Modified = true;
        }
        #endregion

        #region 全選択
        [OperationLog("全解除")]
        private void AllUnSelect()
        {
            ClearStatusMessage();
            grdFilterResult.EndEdit();
            foreach (Row row in grdFilterResult.Rows.Where(x => x.Visible))
                grdFilterResult.SetValue(row.Index, "celCheckBox", false);
            CalculateBillingTotal();
            CalculateMatchingPlanAmount();
            Modified = true;
        }
        #endregion

        #region 選択確定
        [OperationLog("選択確定")]
        private void SelectionConfirm()
        {
            if (!ShowConfirmDialog(MsgQstConfirmSelection)) return;
            FilterBillingList = ((IEnumerable)grdFilterResult.DataSource).Cast<Billing>().ToList();
            ParentForm.DialogResult = DialogResult.OK;
            ParentForm.Close();
        }
        #endregion

        #region 戻る
        [OperationLog("戻る")]
        private void Return()
        {
            try
            {
                Settings.SaveControlValue<PE0107>(Login, cbxDepartmentCodeTo.Name, cbxDepartmentCodeTo.Checked);
                Settings.SaveControlValue<PE0107>(Login, cbxCustomerCodeTo.Name, cbxCustomerCodeTo.Checked);
                Settings.SaveControlValue<PE0107>(Login, cbxStaffCodeTo.Name, cbxStaffCodeTo.Checked);

                if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
                ParentForm.DialogResult = DialogResult.Cancel;
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
            tbBillingFilter.SelectedIndex = 0;
        }
        #endregion

        #region その他Function
        public string GetNumberDoubleValue(decimal value, int displayScale, string displayFormatString = "0")
        {
            var displayFieldString = "#,###,###,###,##0";
            if (displayScale > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < displayScale; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }
            string data = value.ToString(displayFieldString);
            return data;
        }

        /// <summary>合計計算「請求側」</summary>
        private void CalculateBillingTotal()
        {
            var billCount = FilterList.Count();
            var billingTotal = 0M;
            var clearTargetAmount = 0M;
            var checkedCount = 0;
            var discountAmt = 0M;
            for (int i = 0; i < billCount; i++)
            {
                if (Convert.ToBoolean(FilterList[i].Checked))
                {
                    checkedCount++;
                    billingTotal += Convert.ToDecimal(FilterList[i].BillingAmount);
                    clearTargetAmount += Convert.ToDecimal(FilterList[i].TargetAmount);
                    discountAmt += Convert.ToDecimal(FilterList[i].DiscountAmount);
                }
            }
            lblBillingCount.Text = checkedCount.ToString("#,##0") + "/" + billCount.ToString("#,##0");

            //消込対象額
            var targetAmt = 0M;
            if (ApplicationControl.UseDiscount == 1)
            {
                nmbDiscount.Value = Convert.ToDecimal(GetNumberDoubleValue(discountAmt, NoOfPrecision));
                targetAmt = clearTargetAmount + (nmbBillingTaxDifference.Value ?? 0M) - nmbDiscount.Value ?? 0M;
            }
            else
            {
                targetAmt = clearTargetAmount + nmbBillingTaxDifference.Value ?? 0M;
            }
            lblBillingAmountTotal.Text = GetNumberDoubleValue(billingTotal, NoOfPrecision);
            lblBillingTargetAmount.Text = GetNumberDoubleValue(targetAmt, NoOfPrecision);
        }

        /// <summary>消込予定額計算</summary>
        private void CalculateMatchingPlanAmount()
        {
            decimal targeAmount = Convert.ToDecimal(lblBillingTargetAmount.Text);
            decimal payTargetAmount = Convert.ToDecimal(lblReceiptTargetAmount.Text);
            var planAmt = 0M;
            if (!string.IsNullOrEmpty(lblBillingTargetAmount.Text) && !string.IsNullOrEmpty(lblReceiptTargetAmount.Text))
            {
                if (targeAmount < payTargetAmount)
                {
                    planAmt = targeAmount;
                }
                else
                {
                    planAmt = payTargetAmount;
                }
            }
            else if (!string.IsNullOrWhiteSpace(lblBillingTargetAmount.Text))
            {
                planAmt = targeAmount;
            }
            else if (!string.IsNullOrWhiteSpace(lblReceiptTargetAmount.Text))
            {
                planAmt = payTargetAmount;
            }
            lblBillingMatchingAmount.Text = GetNumberDoubleValue(planAmt, NoOfPrecision);
            lblReceiptMatchingAmount.Text = GetNumberDoubleValue(planAmt, NoOfPrecision);
            var keshiGomuTargetAmtPay = Convert.ToDecimal(lblReceiptTargetAmount.Text.ToString());
            lblReceiptMatchingRemain.Text = GetNumberDoubleValue((keshiGomuTargetAmtPay - planAmt), NoOfPrecision); //消込予定額
            //請求のため
            var keshiGomuTargetAmount = Convert.ToDecimal(lblBillingTargetAmount.Text.ToString());
            lblBillingMatchingRemain.Text = GetNumberDoubleValue((keshiGomuTargetAmount - planAmt), NoOfPrecision); //消込予定額
        }
        #endregion

        #region 検索ダイアログ Click Event
        private void btnLoginUserCode_Click(object sender, EventArgs e)
        {
            var loginUser = this.ShowLoginUserSearchDialog();
            if (loginUser != null)
            {
                txtLoginUserCode.Text = loginUser.Code;
                lblLoginUserName.Text = loginUser.Name;
                LoginUserId = loginUser.Id;
                ClearStatusMessage();
            }
        }

        private void btnDepartmentCodeFrom_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentCodeFrom.Text = department.Code;
                lblDepartmentNameFrom.Text = department.Name;

                if (cbxDepartmentCodeTo.Checked)
                {
                    txtDepartmentCodeTo.Text = department.Code;
                    lblDepartmentNameTo.Text = department.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnDepartmentCodeTo_Click(object sender, EventArgs e)
        {
            var department = this.ShowDepartmentSearchDialog();
            if (department != null)
            {
                txtDepartmentCodeTo.Text = department.Code;
                lblDepartmentNameTo.Text = department.Name;
                ClearStatusMessage();
            }
        }

        private void btnStaffCodeFrom_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCodeFrom.Text = staff.Code;
                lblStaffNameFrom.Text = staff.Name;

                if (cbxStaffCodeTo.Checked)
                {
                    txtStaffCodeTo.Text = staff.Code;
                    lblStaffNameTo.Text = staff.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnStaffCodeTo_Click(object sender, EventArgs e)
        {
            var staff = this.ShowStaffSearchDialog();
            if (staff != null)
            {
                txtStaffCodeTo.Text = staff.Code;
                lblStaffNameTo.Text = staff.Name;
                ClearStatusMessage();
            }
        }

        private void btnCustomerCodeFrom_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeFrom.Text = customer.Code;
                lblCustomerCodeFrom.Text = customer.Name;

                if (cbxCustomerCodeTo.Checked)
                {
                    txtCustomerCodeTo.Text = customer.Code;
                    lblCustomerNameTo.Text = customer.Name;
                }
                ClearStatusMessage();
            }
        }

        private void btnCustomerCodeTo_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomerCodeTo.Text = customer.Code;
                lblCustomerNameTo.Text = customer.Name;
                ClearStatusMessage();
            }
        }
        #endregion

        #region Validate Event
        private void txtLoginUserCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtLoginUserCode.Text))
                {
                    lblLoginUserName.Clear();
                    LoginUserId = 0;
                    ClearStatusMessage();
                    return;
                }

                UsersResult result = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<LoginUserMasterClient>();
                    result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtLoginUserCode.Text });
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result && result.Users.Any())
                {
                    lblLoginUserName.Text = result.Users[0].Name;
                    LoginUserId = result.Users[0].Id;
                    ClearStatusMessage();
                }
                else
                {
                    ShowWarningDialog(MsgWngMasterNotExist, "ログインユーザー", txtLoginUserCode.Text);
                    txtLoginUserCode.Clear();
                    lblLoginUserName.Text = null;
                    LoginUserId = 0;
                    txtLoginUserCode.Select();
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtDepartmentCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDepartmentCodeFrom.Text))
                {
                    lblDepartmentNameFrom.Clear();
                    if (cbxDepartmentCodeTo.Checked)
                    {
                        txtDepartmentCodeTo.Clear();
                        lblDepartmentNameTo.Clear();
                    }
                    ClearStatusMessage();
                }
                else
                {
                    var department = GetDataByDepartmentCode(txtDepartmentCodeFrom.Text);

                    if (department == null)
                    {
                        lblDepartmentNameFrom.Clear();
                        if (cbxDepartmentCodeTo.Checked)
                        {
                            txtDepartmentCodeTo.Text = txtDepartmentCodeFrom.Text;
                            lblDepartmentNameTo.Clear();
                        }
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblDepartmentNameFrom.Text = department.Name;
                        if (cbxDepartmentCodeTo.Checked)
                        {
                            txtDepartmentCodeTo.Text = txtDepartmentCodeFrom.Text;
                            lblDepartmentNameTo.Text = department.Name;
                        }
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

        private void txtDepartmentCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDepartmentCodeTo.Text))
                {
                    lblDepartmentNameTo.Clear();
                    ClearStatusMessage();
                }
                else
                {
                    var departmentList = GetDataByDepartmentCode(txtDepartmentCodeTo.Text);

                    if (departmentList == null)
                    {
                        lblDepartmentNameTo.Clear();
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblDepartmentNameTo.Text = departmentList.Name;
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

        private void txtStaffCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtStaffCodeFrom.Text))
                {
                    lblStaffNameFrom.Clear();
                    if (cbxStaffCodeTo.Checked)
                    {
                        txtStaffCodeTo.Clear();
                        lblStaffNameTo.Clear();
                    }
                    ClearStatusMessage();
                }
                else
                {
                    var staff = GetDataByStaffCode(txtStaffCodeFrom.Text);

                    if (staff == null)
                    {
                        lblStaffNameFrom.Clear();
                        if (cbxStaffCodeTo.Checked)
                        {
                            txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                            lblStaffNameTo.Clear();
                        }
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblStaffNameFrom.Text = staff.Name;
                        if (cbxStaffCodeTo.Checked)
                        {
                            txtStaffCodeTo.Text = txtStaffCodeFrom.Text;
                            lblStaffNameTo.Text = staff.Name;
                        }
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

        private void txtStaffCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtStaffCodeTo.Text))
                {
                    lblStaffNameTo.Clear();
                    ClearStatusMessage();
                }
                else
                {
                    var staffList = GetDataByStaffCode(txtStaffCodeTo.Text);

                    if (staffList == null)
                    {
                        lblStaffNameTo.Clear();
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblStaffNameTo.Text = staffList.Name;
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

        private void txtCustomerCodeFrom_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCustomerCodeFrom.Text))
                {
                    lblCustomerCodeFrom.Clear();
                    if (cbxCustomerCodeTo.Checked)
                    {
                        txtCustomerCodeTo.Clear();
                        lblCustomerNameTo.Clear();
                    }
                    ClearStatusMessage();
                }
                else
                {
                    var customer = GetDataByCustomerCode(txtCustomerCodeFrom.Text);

                    if (customer == null)
                    {
                        lblCustomerCodeFrom.Clear();
                        if (cbxCustomerCodeTo.Checked)
                        {
                            txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                            lblCustomerNameTo.Clear();
                        }
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblCustomerCodeFrom.Text = customer.Name;
                        if (cbxCustomerCodeTo.Checked)
                        {
                            txtCustomerCodeTo.Text = txtCustomerCodeFrom.Text;
                            lblCustomerNameTo.Text = customer.Name;
                        }
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

        private void txtCustomerCodeTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCustomerCodeTo.Text))
                {
                    lblCustomerNameTo.Clear();
                    ClearStatusMessage();
                }
                else
                {
                    var customerList = GetDataByCustomerCode(txtCustomerCodeTo.Text);

                    if (customerList == null)
                    {
                        lblCustomerNameTo.Clear();
                        ClearStatusMessage();
                    }
                    else
                    {
                        lblCustomerNameTo.Text = customerList.Name;
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
        #endregion

        #region データ取得
        /// <summary>区分データを取得する </summary>
        /// <param name="categoryType">categoryType Value </param>
        /// <returns>CategoryList</returns>
        private List<Category> GetCategoryForCombo(int categoryType)
        {
            List<Category> billingCategoryList = null;
            CategoriesResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                var categorySearch = new CategorySearch();
                categorySearch.CompanyId = CompanyId;
                categorySearch.CategoryType = categoryType;

                result = await service.GetItemsAsync(Login.SessionKey, categorySearch);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                billingCategoryList = result.Categories;
            }

            return billingCategoryList;
        }

        /// <summary> 得意先コードでデータを取得する </summary>
        /// <param name="code">得意先コード </param>
        /// <returns>Customer</returns>
        private Customer GetDataByCustomerCode(string code)
        {
            Customer customerList = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomersResult result = await service.GetByCodeAsync(SessionKey,
                    CompanyId, new string[] { code });

                if (result.ProcessResult.Result)
                {
                    customerList = result.Customers.FirstOrDefault();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return customerList;
        }

        /// <summary> 担当者コードでデータを取得する </summary>
        /// <param name="code">担当者コード </param>
        /// <returns>Staff</returns>
        private Staff GetDataByStaffCode(string code)
        {
            Staff staffList = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<StaffMasterClient>();
                StaffsResult result = await service.GetByCodeAsync(SessionKey,
                    CompanyId, new string[] { code });

                if (result.ProcessResult.Result)
                {
                    staffList = result.Staffs.FirstOrDefault();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return staffList;
        }

        /// <summary> 請求部門コードでデータを取得する </summary>
        /// <param name="code">請求部門コード</param>
        /// <returns>Department</returns>
        private Department GetDataByDepartmentCode(string code)
        {
            Department departmentList = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                DepartmentsResult result = await service.GetByCodeAsync(SessionKey,
                    CompanyId, new string[] { code });

                if (result.ProcessResult.Result)
                {
                    departmentList = result.Departments.FirstOrDefault();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return departmentList;
        }
        #endregion

        #region GridValueChange Event
        private void grdFilterResult_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.CellName == "celCheckBox")
            {
                CheckValueChange(e.RowIndex);
                CalculateBillingTotal();
                CalculateMatchingPlanAmount();
                Modified = true;
            }
        }

        private void grdFilterResult_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.RowIndex >= 0 && e.CellName == "celCheckBox")
            {
                CheckValueChange(e.RowIndex);
                CalculateBillingTotal();
                CalculateMatchingPlanAmount();
                Modified = true;
            }
        }

        private void CheckValueChange(int rowIndex)
        {
            var checkValue = Convert.ToBoolean(grdFilterResult.Rows[rowIndex].Cells["celCheckBox"].EditedFormattedValue);
            if (checkValue)
            {
                grdFilterResult.Rows[rowIndex].Cells["celCheckBox"].Value = true;
            }
            else
            {
                grdFilterResult.Rows[rowIndex].Cells["celCheckBox"].Value = false;
            }
        }
        #endregion


        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                var billingList = grdFilterResult.Rows
                    .Where(x => Convert.ToBoolean(x["celCheckBox"].Visible))
                    .Select(x => x.DataBoundItem as Billing).ToList();

                if (!(billingList?.Any() ?? false))
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                var serverPath = GetServerPath();
                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"個別消込請求データ{DateTime.Now:yyyyMMddHHmmss}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new MatchingBillingFilterFileDefinition(new DataExpression(ApplicationControl));
                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                if (definition.CurrencyCodeField.Ignored = !UseForeignCurrency)
                {
                    definition.CurrencyCodeField.FieldName = null;
                }
                if (definition.ScheduledPaymentKeyField.Ignored = !UseScheduledPayment)
                {
                    definition.ScheduledPaymentKeyField.FieldName = null;
                }
                var format = NoOfPrecision == 0 ? "0" : "0." + new string('0', NoOfPrecision);
                definition.BillingAmountField.Format = value => value.ToString(format);
                definition.RemainAmountField.Format = value => value.ToString(format);
                definition.TargetAmountField.Format = value => value.ToString(format);
                definition.DiscountAmountField.Format = value => value.ToString(format);

                int FieldNumber = ApplicationControl.UseForeignCurrency;

                foreach (var billItem in BillingGridValue)
                {
                    if (billItem.ColumnName == "AssignmentFlag")
                    {
                        definition.BillCheckField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillCheckField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillCheckField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "CustomerCode")
                    {
                        definition.CustomerCodeField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.CustomerCodeField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.CustomerCodeField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "CustomerName")
                    {
                        definition.CustomerNameField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.CustomerNameField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.CustomerNameField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "BilledAt")
                    {
                        definition.BilledAtField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BilledAtField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BilledAtField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "DueAt")
                    {
                        definition.DueAtField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.DueAtField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.DueAtField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "BillingAmount")
                    {
                        definition.BillingAmountField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingAmountField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingAmountField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "RemainAmount")
                    {
                        definition.RemainAmountField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.RemainAmountField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.RemainAmountField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "TargetAmount")
                    {
                        definition.TargetAmountField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.TargetAmountField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.TargetAmountField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "DiscountAmountSummary")
                    {
                        definition.DiscountAmountField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.DiscountAmountField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.DiscountAmountField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "InvoiceCode")
                    {
                        definition.InvoiceCodeField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.InvoiceCodeField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.InvoiceCodeField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "BillingCategory")
                    {
                        definition.BillingCategoryField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingCategoryField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingCategoryField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "DepartmentName")
                    {
                        definition.BillingDepartmentField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingDepartmentField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingDepartmentField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "Note1")
                    {
                        definition.BillingNote1Field.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingNote1Field.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingNote1Field.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "Note2")
                    {
                        definition.BillingNote2Field.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingNote2Field.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingNote2Field.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "Note3")
                    {
                        definition.BillingNote3Field.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingNote3Field.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingNote3Field.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "Note4")
                    {
                        definition.BillingNote4Field.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingNote4Field.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingNote4Field.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "Memo")
                    {
                        definition.BillingMemoField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingMemoField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingMemoField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "InputType")
                    {
                        definition.BillingInputTypeField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.BillingInputTypeField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.BillingInputTypeField.Ignored = (billItem.DisplayWidth == 0);
                    }
                    else if (billItem.ColumnName == "ScheduledPaymentKey")
                    {
                        definition.ScheduledPaymentKeyField.FieldNumber = billItem.DisplayOrder + FieldNumber;
                        definition.ScheduledPaymentKeyField.FieldName = (billItem.DisplayWidth == 0) ? "" : billItem.ColumnNameJp;
                        definition.ScheduledPaymentKeyField.Ignored = (billItem.DisplayWidth == 0);
                    }
                }

                foreach (Billing export in billingList)
                {
                    int rowcount = billingList.IndexOf(export);
                    if (rowcount < grdFilterResult.RowCount && Convert.ToBoolean(grdFilterResult.Rows[rowcount].Cells["celCheckBox"].Value))
                        export.BillCheck = "レ";
                    else
                        export.BillCheck = "";

                    if (UseForeignCurrency)
                    {
                        export.CurrencyCode = CurrencyCode;
                    }
                }

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, billingList, cancel, progress);
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

        /// <summary>管理マスターで設定したサーバパスを取得</summary>
        /// <returns>サーバパス</returns>
        private string GetServerPath()
        {
            var task = Util.GetGeneralSettingServerPathAsync(Login);
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            return task.Result;
        }
    }
}
